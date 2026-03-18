# Optimistic vs Pessimistic Locking

## What You'll Learn
- The fundamental concurrency problem in databases
- Pessimistic locking: lock before you modify
- Optimistic locking: check version, retry if changed
- When to choose each approach
- Implementing both in SQLAlchemy

## Prerequisites
- Completed `07-database-transactions-and-acid.md` — understanding of transactions
- Understanding of database isolation levels

## The Problem: Lost Updates

When two users read the same data and both try to update it, without locking one update can be lost:

```
Time    User A                    User B                   Database
────────────────────────────────────────────────────────────────────
T1      READ balance: $100                               $100
T2                                  READ balance: $100   $100
T3      UPDATE: +$50                                      $150
T4                                  UPDATE: +$75         ???

Result: 
  - Expected: $225 (100 + 50 + 75)
  - Actual:   $175 (100 + 75, A's update lost!)
```

This is called a "lost update" — the classic concurrency problem.

## Pessimistic Locking

**Lock first, then modify.** Acquires a lock before anyone can read, holds it until commit.

```
User A                           Database
─────────────────────────────────────────────────────
SELECT ... FOR UPDATE            Locks row
(waits until lock acquired)      
                                 
UPDATE balance = balance + 50    Executes update
COMMIT (releases lock)            Changes saved
```

### Implementing Pessimistic Locking

```python
from sqlalchemy import Column, Integer, String, Numeric, select
from sqlalchemy.orm import sessionmaker
from datetime import datetime

class BankAccount(Base):
    __tablename__ = "bank_accounts"
    
    id = Column(Integer, primary_key=True)
    account_number = Column(String, unique=True)
    balance = Column(Numeric(10, 2))
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)

SessionLocal = sessionmaker(bind=engine)

def withdraw_pessimistic(account_number: str, amount: float) -> bool:
    """Withdraw with pessimistic locking."""
    session = SessionLocal()
    try:
        # Lock the row - other transactions wait here
        result = session.execute(
            select(BankAccount)
            .where(BankAccount.account_number == account_number)
            .with_for_update()  # Pessimistic lock!
        )
        account = result.scalar_one_or_none()
        
        if not account:
            raise ValueError("Account not found")
        
        if account.balance < amount:
            raise ValueError("Insufficient funds")
        
        # Safe to update - we hold the lock
        account.balance -= amount
        
        session.commit()
        return True
        
    except Exception:
        session.rollback()
        raise
    finally:
        session.close()
```

🔍 **Line-by-Line Breakdown:**
1. `.with_for_update()` — Generates `SELECT ... FOR UPDATE`. PostgreSQL locks the row.
2. Other transactions trying to lock this row will block until we commit or rollback
3. We can safely read and update because no one else can modify until we release the lock

**Pros:**
- Guaranteed no conflicts
- Simple to reason about

**Cons:**
- Blocks other transactions (reduced concurrency)
- Can cause deadlocks if not careful
- Slow under high contention

## Optimistic Locking

**Modify first, check if changed.** Read without locking, but check a version number before updating.

```
User A                           Database
─────────────────────────────────────────────────────
SELECT balance, version          balance: $100, version: 1
                                 
User B                           Database
─────────────────────────────────────────────────────
SELECT balance, version          balance: $100, version: 1

User A
UPDATE balance = 150             Executes!
  WHERE version = 1              version becomes 2
  SET version = 2                1 row updated

User B
UPDATE balance = 175             FAILS!
  WHERE version = 1              No rows match (version is now 2)
  version = 2                    User B must retry!
```

### Implementing Optimistic Locking

```python
from sqlalchemy import Column, Integer, String, Numeric, DateTime
from sqlalchemy.orm import sessionmaker

class BankAccountOptimistic(Base):
    __tablename__ = "bank_accounts_optimistic"
    
    id = Column(Integer, primary_key=True)
    account_number = Column(String, unique=True)
    balance = Column(Numeric(10, 2))
    version = Column(Integer, default=1, nullable=False)  # Version column!
    updated_at = Column(DateTime)

def withdraw_optimistic(account_number: str, amount: float, max_retries: int = 3) -> bool:
    """
    Withdraw with optimistic locking.
    Retries if version changed between read and write.
    """
    session = SessionLocal()
    
    for attempt in range(max_retries):
        try:
            # Read current state (no lock!)
            account = session.query(BankAccountOptimistic).filter(
                BankAccountOptimistic.account_number == account_number
            ).first()
            
            if not account:
                raise ValueError("Account not found")
            
            if account.balance < amount:
                raise ValueError("Insufficient funds")
            
            # Calculate new balance
            new_balance = float(account.balance) - amount
            new_version = account.version + 1
            
            # Update only if version hasn't changed
            # This is the atomic check-and-set
            rows_updated = session.query(BankAccountOptimistic).filter(
                BankAccountOptimistic.account_number == account_number,
                BankAccountOptimistic.version == account.version  # Must match!
            ).update({
                "balance": new_balance,
                "version": new_version
            }, synchronize_session=False)
            
            if rows_updated == 0:
                # Version changed! Another transaction modified it
                session.rollback()
                continue  # Retry
            
            session.commit()
            return True
            
        except Exception as e:
            session.rollback()
            if attempt == max_retries - 1:
                raise
            # Retry on conflict
    
    return False
```

### Using SQLAlchemy's Built-in Version ID

SQLAlchemy has built-in support for optimistic locking:

```python
from sqlalchemy.orm import DeclarativeBase
from sqlalchemy import Column, Integer, String, Numeric
from sqlalchemy.orm import versioning

class Base(DeclarativeBase):
    pass

class BankAccount(Base):
    __tablename__ = "bank_accounts_sa"
    
    id = Column(Integer, primary_key=True)
    account_number = Column(String, unique=True)
    balance = Column(Numeric(10, 2))
    
    # SQLAlchemy manages version automatically!
    __mapper_args__ = {
        "version_id_col": Column(Integer, nullable=False, default=1)
    }

# When you update, SQLAlchemy automatically adds WHERE version = expected
# If 0 rows updated, raises StaleDataError
try:
    account.balance = new_balance
    session.commit()
except Exception as exc:
    session.rollback()
    # Handle StaleDataError - retry the whole operation
```

## Comparison Table

| Aspect | Pessimistic | Optimistic |
|--------|-------------|-------------|
| **Conflict detection** | Before read | Before write |
| **Locking** | Holds lock until commit | No locks |
| **Throughput** | Lower (contention) | Higher |
| **Latency** | Can block | Non-blocking |
| **Failures** | Blocks/waits | Retry needed |
| **Best for** | High contention | Low contention |

## When to Use Each

### Use Pessimistic When:
- Contention is high (frequent concurrent updates to same rows)
- Updates are expensive (don't want to retry expensive operations)
- You need guaranteed completion (don't want retry logic)
- Example: Inventory management, bank transfers

### Use Optimistic When:
- Contention is low (few concurrent updates)
- Updates are cheap (easy to retry)
- You want maximum throughput
- Example: User profile updates, document editing

## Production Considerations

- **Optimistic locking retry loops**: Must be exponential backoff to avoid thundering herd
- **Version column**: Always index it for performance
- **Pessimistic deadlock risk**: Always acquire locks in consistent order
- **Monitoring**: Track conflict/retry rates for optimistic; track wait times for pessimistic

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Using pessimistic on high-contention data

**Wrong:**
```python
# Every inventory update uses pessimistic lock
# Under high load, everything blocks!
def update_inventory(item_id, quantity):
    item = session.query(Item).filter(
        Item.id == item_id
    ).with_for_update().first()
    item.quantity += quantity
    session.commit()
```

**Why it fails:** All concurrent updates queue up. System crawls.

**Fix:** Use optimistic:
```python
def update_inventory(item_id, quantity):
    # Fast - no locking, just retry on conflict
    for attempt in range(3):
        # Update with version check
        ...
```

### ❌ Mistake 2: Forgetting to index version column

**Wrong:**
```python
class Product(Base):
    __tablename__ = "products"
    id = Column(Integer, primary_key=True)
    version = Column(Integer)  # No index!
```

**Why it fails:** Every optimistic update does a full table scan to find the row with matching version.

**Fix:**
```python
version = Column(Integer, index=True)  # Index for fast lookups
```

### ❌ Mistake 3: Infinite retry loop

**Wrong:**
```python
def withdraw(account_number, amount):
    while True:  # Infinite loop!
        try:
            # Optimistic update
            ...
        except StaleDataError:
            pass  # Just retry forever
```

**Why it fails:** Can hang forever under sustained contention.

**Fix:** Limit retries:
```python
def withdraw(account_number, amount, max_retries=3):
    for attempt in range(max_retries):
        try:
            ...
        except StaleDataError:
            if attempt == max_retries - 1:
                raise
    raise Exception("Failed after max retries")
```

## Summary

- Pessimistic locking locks before modifying — blocks concurrent access, guaranteed no conflicts
- Optimistic locking checks version on write — no blocking, but requires retry logic
- Choose pessimistic for high contention, optimistic for low contention
- Always have retry logic with max attempts for optimistic locking
- Index the version column for performance

## Next Steps

This completes the Advanced Database Patterns folder. Continue to `33-advanced-fastapi-patterns/01-custom-middleware.md` to learn about extending FastAPI with custom middleware.
