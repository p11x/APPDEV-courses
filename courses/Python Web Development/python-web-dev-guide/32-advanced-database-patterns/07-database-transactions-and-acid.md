# Database Transactions and ACID

## What You'll Learn
- ACID properties and what they guarantee
- Database transaction isolation levels and their tradeoffs
- Implementing transactions in SQLAlchemy (sync and async)
- Handling savepoints for partial rollbacks
- Distributed transactions and the CAP theorem

## Prerequisites
- Completed `06-redis-as-primary-database.md` — Redis as primary database
- Completed `05-databases/02-sqlalchemy-orm.md` — SQLAlchemy fundamentals
- Understanding of SQL and database fundamentals

## ACID: The Four Guarantees

ACID is a set of properties that ensure database transactions are processed reliably:

```
┌─────────────────────────────────────────────────────────────────┐
│                         ACID                                    │
├─────────────────────────────────────────────────────────────────┤
│  A │ Atomicity    │ All operations succeed, or none do         │
│  C │ Consistency  │ Database moves from valid state to valid  │
│  I │ Isolation    │ Concurrent transactions don't interfere     │
│  D │ Durability   │ Committed data survives crashes           │
└─────────────────────────────────────────────────────────────────┘
```

### Atomicity
All operations in a transaction succeed together, or none do. Think of it like a flight booking: payment + seat reservation + ticket generation must all succeed, or the whole thing is cancelled.

### Consistency
The database starts in a valid state and ends in a valid state. Constraints, triggers, and cascades ensure data integrity.

### Isolation
Concurrent transactions don't see each other's intermediate states. Results are the same as if they ran sequentially.

### Durability
Once committed, data persists even if the system crashes. Achieved through write-ahead logs and replication.

## Transaction Isolation Levels

Isolation is the trickiest property. Higher isolation = fewer anomalies but more locking = slower concurrency.

| Level | Dirty Reads | Non-Repeatable Reads | Phantom Reads |
|-------|-------------|---------------------|----------------|
| READ UNCOMMITTED | Possible | Possible | Possible |
| READ COMMITTED | ❌ Impossible | Possible | Possible |
| REPEATABLE READ | ❌ Impossible | ❌ Impossible | Possible |
| SERIALIZABLE | ❌ Impossible | ❌ Impossible | ❌ Impossible |

PostgreSQL defaults to READ COMMITTED. MySQL/InnoDB defaults to REPEATABLE READ.

### Setting Isolation in SQLAlchemy

```python
from sqlalchemy import create_engine, text
from sqlalchemy.orm import sessionmaker
from sqlalchemy.engine import Engine

engine = create_engine(
    "postgresql://user:pass@localhost/mydb",
    isolation_level="READ COMMITTED"  # Default for PostgreSQL
)

# For stricter isolation
engine_strict = create_engine(
    "postgresql://user:pass@localhost/mydb",
    isolation_level="REPEATABLE READ"
)
```

## Implementing Transactions in SQLAlchemy

### Synchronous Transactions

```python
from sqlalchemy import Column, Integer, String, create_engine
from sqlalchemy.orm import sessionmaker, DeclarativeBase

class Base(DeclarativeBase):
    pass

class Account(Base):
    __tablename__ = "accounts"
    
    id = Column(Integer, primary_key=True)
    name = Column(String)
    balance = Column(Integer)  # In cents

engine = create_engine("postgresql://user:pass@localhost/mydb")
SessionLocal = sessionmaker(bind=engine)

def transfer_money(from_id: int, to_id: int, amount: int) -> bool:
    """
    Transfer money between accounts atomically.
    Uses database transaction with proper locking.
    """
    session = SessionLocal()
    try:
        # Start transaction explicitly
        # PostgreSQL auto-begins, but explicit is clearer
        
        # Lock accounts in consistent order (prevents deadlocks!)
        # Always lock in ascending ID order
        accounts = sorted([from_id, to_id])
        
        for account_id in accounts:
            # SELECT FOR UPDATE - acquires row lock
            account = session.query(Account).filter(
                Account.id == account_id
            ).with_for_update().first()
            
            if not account:
                raise ValueError(f"Account {account_id} not found")
        
        # Get current accounts (now locked)
        from_account = session.query(Account).filter(Account.id == from_id).first()
        to_account = session.query(Account).filter(Account.id == to_id).first()
        
        # Check balance
        if from_account.balance < amount:
            raise ValueError("Insufficient funds")
        
        # Transfer
        from_account.balance -= amount
        to_account.balance += amount
        
        # Commit - releases locks
        session.commit()
        return True
        
    except Exception as e:
        session.rollback()  # Undo all changes
        raise e
    finally:
        session.close()
```

🔍 **Line-by-Line Breakdown:**
1. `SessionLocal()` — Creates a new session. Each session = one transaction context.
2. `with_for_update()` — Acquires a row-level lock (`SELECT ... FOR UPDATE`). Other transactions trying to lock this row will wait.
3. Locking in ascending ID order prevents deadlocks — if two transfers happen simultaneously (A→B and B→A), both will lock in same order.
4. `session.commit()` — Commits transaction, releases locks.
5. `session.rollback()` — On any error, undo all changes in this transaction.
6. `session.close()` — Always close to return connection to pool.

### Async Transactions

```python
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker

async_engine = create_async_engine(
    "postgresql+asyncpg://user:pass@localhost/mydb"
)

AsyncSessionLocal = sessionmaker(
    bind=async_engine,
    class_=AsyncSession,
    expire_on_commit=False
)

async def transfer_money_async(from_id: int, to_id: int, amount: int) -> bool:
    """Async version with transactions."""
    async with AsyncSessionLocal() as session:
        try:
            # Lock in consistent order
            accounts = sorted([from_id, to_id])
            
            for account_id in accounts:
                result = await session.execute(
                    select(Account).where(Account.id == account_id).with_for_update()
                )
                account = result.scalar_one_or_none()
                
                if not account:
                    raise ValueError(f"Account {account_id} not found")
            
            # Perform transfer
            from_account = await session.get(Account, from_id)
            to_account = await session.get(Account, to_id)
            
            if from_account.balance < amount:
                raise ValueError("Insufficient funds")
            
            from_account.balance -= amount
            to_account.balance += amount
            
            await session.commit()
            return True
            
        except Exception:
            await session.rollback()
            raise
```

## Savepoints for Partial Rollbacks

Savepoints let you roll back part of a transaction while keeping other changes:

```python
def complex_operation():
    session = SessionLocal()
    try:
        # Point 1
        session.add(Record(name="A"))
        session.flush()  # Gets ID but not committed
        
        # Savepoint
        sp1 = session.begin_nested()  # Creates savepoint
        
        try:
            # Point 2
            session.add(Record(name="B"))
            session.flush()
            
            # This might fail
            if should_fail:
                raise ValueError("Something went wrong")
                
        except Exception:
            session.rollback(sp1)  # Rollback to savepoint - only B is lost
            # A is still in transaction!
        
        # Point 3 - A still here
        session.add(Record(name="C"))
        session.commit()
        
    finally:
        session.close()
```

## The CAP Theorem

In distributed systems, you can only guarantee two of three:

```
                    ┌─────────────┐
                    │             │
                ┌───┤   CAP      ├───┐
                │   │             │   │
                ▼   └─────────────┘   ▼
          ┌──────────┐          ┌──────────┐
          │ Consistency│          │Availability│
          └─────┬─────┘          └─────┬─────┘
                │                     │
                └─────────┬───────────┘
                          ▼
                   ┌─────────────┐
                   │   Partition  │
                   │   Tolerance  │
                   └─────────────┘
```

**Consistency + Partition Tolerance (CP)**: MongoDB, PostgreSQL — sacrifice availability during network partitions
**Availability + Partition Tolerance (AP)**: Cassandra, DynamoDB — sacrifice consistency during partitions
**Consistency + Availability (CA)**: Not possible with partitions — only works in single-node systems

## Distributed Transactions

When data spans multiple databases, you need coordination:

### Two-Phase Commit (2PC)

```
Phase 1: Prepare
┌─────┐  prepare   ┌─────┐
│App  │ ─────────▶ │ DB1 │
└─────┘            └─────┘
       ◀───────────
        yes/no?

Phase 2: Commit
┌─────┐  commit   ┌─────┐
│App  │ ─────────▶ │ DB1 │
└─────┘            └─────┘
```

Two-phase commit is slow and can block. Use only when absolutely necessary.

### Saga Pattern

Instead of one atomic transaction, use a series of local transactions with compensation:

```python
async def place_order_saga(order_data: dict) -> str:
    """
    Saga: Create Order → Reserve Inventory → Process Payment
    If any step fails, compensate (undo) previous steps.
    """
    order_id = await create_order(order_data)  # Step 1
    
    try:
        await reserve_inventory(order_data["items"])  # Step 2
    except Exception:
        await cancel_order(order_id)  # Compensate step 1
        raise
    
    try:
        await process_payment(order_id, order_data["amount"])  # Step 3
    except Exception:
        await release_inventory(order_data["items"])  # Compensate step 2
        await cancel_order(order_id)  # Compensate step 1
        raise
    
    await confirm_order(order_id)  # Final step
    return order_id
```

## Production Considerations

- **Transaction scope**: Keep transactions short. Long-running transactions hold locks and block other queries.
- **Deadlocks**: Always acquire locks in consistent order (ascending ID). Monitor with `pg_stat_activity`.
- **Serialization failures**: With SERIALIZABLE, expect occasional conflicts. Retry logic is essential.
- **Distributed transactions**: Avoid if possible. They are slow and can be inconsistent during failures.

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not using transactions for multi-step operations

**Wrong:**
```python
def transfer_money(from_id, to_id, amount):
    # Two separate operations - can fail between them!
    withdraw(from_id, amount)
    deposit(to_id, amount)
```

**Why it fails:** If process crashes after withdraw but before deposit, money disappears.

**Fix:** Wrap in transaction:
```python
def transfer_money(from_id, to_id, amount):
    session = SessionLocal()
    try:
        # Both in one transaction
        withdraw(from_id, amount)
        deposit(to_id, amount)
        session.commit()
    except:
        session.rollback()
        raise
```

### ❌ Mistake 2: Long-running transactions

**Wrong:**
```python
def slow_operation():
    session = SessionLocal()
    for item in huge_list:  # Could take minutes!
        process(item)
    session.commit()  # Holds locks the whole time!
```

**Why it fails:** Holds locks for minutes, blocking all other writes to those tables.

**Fix:** Process in batches with commits:
```python
def fast_operation():
    session = SessionLocal()
    for batch in chunked(huge_list, 100):
        process_batch(batch)
        session.commit()  # Release locks between batches
```

### ❌ Mistake 3: Forgetting to close sessions

**Wrong:**
```python
def get_user(user_id):
    session = SessionLocal()  # Opened
    user = session.query(User).get(user_id)
    # Never closed! Connection leak!
```

**Why it fails:** Connection pool exhausts. Eventually all connections are used up.

**Fix:**
```python
def get_user(user_id):
    session = SessionLocal()
    try:
        return session.query(User).get(user_id)
    finally:
        session.close()  # Or use context manager
```

## Summary

- ACID provides strong guarantees: atomicity, consistency, isolation, durability
- Use transactions for any operation that touches multiple tables or needs atomicity
- `SELECT FOR UPDATE` acquires row locks — always lock in consistent order to prevent deadlocks
- Keep transactions short to minimize lock contention
- For distributed systems, consider Saga pattern instead of distributed transactions

## Next Steps

→ Continue to `08-optimistic-vs-pessimistic-locking.md` to learn how to handle concurrent updates at scale.
