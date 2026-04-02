# Transaction Management

## Overview

Transactions ensure data integrity by grouping operations that must succeed or fail together. SQLAlchemy provides robust transaction support for FastAPI.

## Basic Transactions

### Transaction Fundamentals

```python
# Example 1: Basic transaction management
from sqlalchemy.orm import Session
from sqlalchemy import create_engine
from fastapi import FastAPI, Depends

app = FastAPI()

# Transaction basics with SQLAlchemy
def transfer_money(db: Session, from_id: int, to_id: int, amount: float):
    """
    Atomic transaction - all or nothing.
    If any operation fails, all changes are rolled back.
    """
    try:
        # Debit from source account
        from_account = db.query(Account).filter(Account.id == from_id).first()
        from_account.balance -= amount

        # Credit to destination account
        to_account = db.query(Account).filter(Account.id == to_id).first()
        to_account.balance += amount

        # Create transaction record
        transaction = Transaction(
            from_account_id=from_id,
            to_account_id=to_id,
            amount=amount
        )
        db.add(transaction)

        # Commit all changes
        db.commit()

        return {"success": True, "transaction_id": transaction.id}

    except Exception as e:
        # Rollback on any error
        db.rollback()
        raise e
```

## Context Manager Transactions

### Using Context Managers

```python
# Example 2: Context manager transactions
from contextlib import contextmanager

@contextmanager
def get_db_session():
    """Context manager for database sessions"""
    session = SessionLocal()
    try:
        yield session
        session.commit()
    except Exception:
        session.rollback()
        raise
    finally:
        session.close()

# Usage
def create_user_with_profile(user_data: dict, profile_data: dict):
    """Create user and profile in single transaction"""
    with get_db_session() as db:
        # Create user
        user = User(**user_data)
        db.add(user)
        db.flush()  # Get user.id without committing

        # Create profile
        profile = Profile(user_id=user.id, **profile_data)
        db.add(profile)

        # Both committed together
        return user
```

## Nested Transactions

### Savepoints

```python
# Example 3: Nested transactions with savepoints
def process_order(db: Session, order_data: dict):
    """
    Process order with nested transactions.
    Uses savepoints for partial rollback.
    """
    try:
        # Main transaction begins
        order = Order(**order_data)
        db.add(order)
        db.flush()

        # Savepoint for inventory update
        savepoint = db.begin_nested()
        try:
            for item in order_data["items"]:
                inventory = db.query(Inventory).filter(
                    Inventory.product_id == item["product_id"]
                ).with_for_update().first()  # Lock row

                if inventory.quantity < item["quantity"]:
                    raise ValueError(f"Insufficient inventory for {item['product_id']}")

                inventory.quantity -= item["quantity"]

            savepoint.commit()
        except ValueError:
            savepoint.rollback()
            # Continue with backorder logic
            create_backorder(db, order, item)

        # Payment processing (another savepoint)
        savepoint = db.begin_nested()
        try:
            payment = Payment(order_id=order.id, amount=order.total)
            db.add(payment)
            process_payment(payment)
            savepoint.commit()
        except PaymentError:
            savepoint.rollback()
            raise

        # Final commit
        db.commit()
        return order

    except Exception:
        db.rollback()
        raise
```

## Async Transactions

### Async Transaction Management

```python
# Example 4: Async transactions
from sqlalchemy.ext.asyncio import AsyncSession

async def async_transfer_money(
    session: AsyncSession,
    from_id: int,
    to_id: int,
    amount: float
):
    """Async transaction example"""
    async with session.begin():
        # Debit
        result = await session.execute(
            select(Account).where(Account.id == from_id).with_for_update()
        )
        from_account = result.scalar_one()
        from_account.balance -= amount

        # Credit
        result = await session.execute(
            select(Account).where(Account.id == to_id).with_for_update()
        )
        to_account = result.scalar_one()
        to_account.balance += amount

        # Transaction auto-commits on context exit

@app.post("/transfer/")
async def transfer(
    from_id: int,
    to_id: int,
    amount: float,
    session: AsyncSession = Depends(get_async_db)
):
    await async_transfer_money(session, from_id, to_id, amount)
    return {"success": True}
```

## Transaction Decorators

### Reusable Transaction Logic

```python
# Example 5: Transaction decorator
from functools import wraps

def atomic(func):
    """
    Decorator to wrap function in transaction.
    Auto-commits on success, rolls back on exception.
    """
    @wraps(func)
    def wrapper(*args, **kwargs):
        db = kwargs.get("db") or SessionLocal()
        try:
            result = func(*args, **kwargs)
            db.commit()
            return result
        except Exception:
            db.rollback()
            raise
        finally:
            if "db" not in kwargs:
                db.close()
    return wrapper

@atomic
def create_user_atomic(db: Session, username: str, email: str):
    """User creation with automatic transaction"""
    user = User(username=username, email=email)
    db.add(user)
    return user
```

## Best Practices

### Transaction Guidelines

```python
# Example 6: Transaction best practices
"""
Transaction Best Practices:

1. Keep transactions short
   - Hold locks for minimal time
   - Avoid long-running operations

2. Use appropriate isolation levels
   - READ COMMITTED: Most cases
   - SERIALIZABLE: Critical data

3. Handle deadlocks
   - Retry logic for deadlocks
   - Consistent lock ordering

4. Use savepoints for partial rollback
   - Complex operations
   - Non-critical failures

5. Always rollback on error
   - Never leave transactions open
   - Use context managers

6. Monitor transaction duration
   - Log long-running transactions
   - Alert on timeout
"""

from sqlalchemy import event

@event.listens_for(Session, "after_transaction_end")
def log_transaction(session, transaction):
    """Log transaction completion"""
    if transaction.is_active:
        print("Transaction still active")
    else:
        print("Transaction ended")
```

## Summary

| Pattern | Use Case | Example |
|---------|----------|---------|
| Simple commit | Single operation | `db.commit()` |
| Context manager | Multiple operations | `with session.begin()` |
| Savepoints | Partial rollback | `db.begin_nested()` |
| Async | Non-blocking I/O | `async with session.begin()` |

## Next Steps

Continue learning about:
- [Relationship Mapping](../02_sqlalchemy_basics/04_relationship_mapping.md) - Relationships
- [Query Optimization](../08_database_performance/01_query_optimization.md) - Performance
