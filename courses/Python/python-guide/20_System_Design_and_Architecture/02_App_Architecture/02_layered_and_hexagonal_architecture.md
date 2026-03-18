# 🏛️ Layered and Hexagonal Architecture

> Two architecture patterns that keep your code testable and maintainable.

## 🎯 What You'll Learn

- Layered Architecture (N-Tier)
- Hexagonal Architecture (Ports and Adapters)
- Practical comparison
- Refactoring example

## 📦 Prerequisites

- Completion of [01_structuring_large_projects.md](./01_structuring_large_projects.md)

---

## Layered Architecture (N-Tier)

Separate concerns into horizontal layers:

```
Presentation Layer
     ↓
Business Logic Layer
     ↓
Data Access Layer
     ↓
Database
```

### Example: FastAPI App

```python
# presentation/api/routes.py
from fastapi import APIRouter, Depends
from ..services import UserService
from ..models import UserCreate

router = APIRouter()

@router.post("/users/")
def create_user(
    user: UserCreate,
    service: UserService = Depends()
):
    return service.create_user(user)


# business/services/user_service.py
from ..repositories import UserRepository

class UserService:
    def __init__(self, repo: UserRepository):
        self.repo = repo
    
    def create_user(self, user_data):
        # Business logic here
        return self.repo.create(user_data)


# data/repositories/user_repo.py
from ..models import User

class UserRepository:
    def create(self, user_data):
        # Data access logic
        return User(**user_data)
```

### Pros and Cons

| Pros | Cons |
|------|------|
| Simple to understand | Can become rigid |
| Clear separation | Layers may leak |
| Easy to test layers | Changes ripple through layers |

---

## Hexagonal Architecture (Ports and Adapters)

Core domain isolated from external concerns:

```
          Adapters
     ┌─────────────┐
     │             │
     │   Core      │◄──┐
     │   Domain    │   │
     │             │   │
     └─────────────┘   │
          ▲             │
          │             │
     ┌─────────────┐   │
     │             │   │
     │   Ports     │   │
     │ (Interfaces)│   │
     └─────────────┘   │
          ▲             │
          │             │
┌─────────────┐   ┌─────────────┐
│             │   │             │
│  Adapter    │   │  Adapter    │
│ (SQLite)    │   │ (HTTP)      │
│             │   │             │
└─────────────┘   └─────────────┘
```

### Core Domain (Zero Dependencies)

```python
# domain/models.py
from dataclasses import dataclass
from typing import Protocol

@dataclass
class Transaction:
    id: str
    amount: float
    description: str

class TransactionRepository(Protocol):
    def save(self, transaction: Transaction) -> None: ...
    def get_by_id(self, id: str) -> Transaction | None: ...

class TransactionService:
    def __init__(self, repo: TransactionRepository):
        self.repo = repo
    
    def add_transaction(self, amount: float, description: str) -> Transaction:
        txn = Transaction(
            id=f"txn_{hash(str(amount)+description)}",
            amount=amount,
            description=description
        )
        self.repo.save(txn)
        return txn
```

### Adapters (External Concerns)

```python
# adapters/sqlite_repo.py
import sqlite3
from ..domain.models import Transaction, TransactionRepository

class SQLiteTransactionRepository:
    def __init__(self, db_path: str):
        self.conn = sqlite3.connect(db_path)
        self._init_db()
    
    def _init_db(self):
        self.conn.execute("""
            CREATE TABLE IF NOT EXISTS transactions (
                id TEXT PRIMARY KEY,
                amount REAL,
                description TEXT
            )
        """)
    
    def save(self, transaction: Transaction) -> None:
        self.conn.execute(
            "INSERT INTO transactions VALUES (?, ?, ?)",
            (transaction.id, transaction.amount, transaction.description)
        )
        self.conn.commit()
    
    def get_by_id(self, id: str) -> Transaction | None:
        cursor = self.conn.execute(
            "SELECT * FROM transactions WHERE id = ?", (id,)
        )
        row = cursor.fetchone()
        if row:
            return Transaction(id=row[0], amount=row[1], description=row[2])
        return None


# adapters/http_adapter.py
from fastapi import APIRouter, HTTPException
from ..domain.models import Transaction, TransactionService

router = APIRouter()

# This would be injected with actual service
def get_transaction_service() -> TransactionService:
    # In real app, this would come from dependency injection
    from adapters.sqlite_repo import SQLiteTransactionRepository
    from domain.models import TransactionService
    
    repo = SQLiteTransactionRepository("transactions.db")
    return TransactionService(repo)

@router.post("/transactions/")
def create_transaction(
    amount: float,
    description: str,
    service: TransactionService = Depends(get_transaction_service)
):
    txn = service.add_transaction(amount, description)
    return {"id": txn.id}
```

### Pros and Cons

| Pros | Cons |
|------|------|
| Highly testable | More initial setup |
| Framework independent | More indirection |
| Easy to swap adapters | Learning curve |
| Clear boundaries | More files |

---

## Practical Comparison

| Aspect | Layered | Hexagonal |
|--------|---------|-----------|
| Setup | Simple | More complex |
| Testing | Test layers | Test core with fakes |
| Flexibility | Moderate | High |
| Best For | Most apps | Complex, testable systems |

---

## Refactoring Example

### Before: Tangled FastAPI + SQLite

```python
# ❌ BEFORE: Everything mixed together
from fastapi import FastAPI
import sqlite3

app = FastAPI()
conn = sqlite3.connect("app.db")

@app.post("/users/")
def create_user(name: str, email: str):
    # Business logic + data access mixed!
    conn.execute(
        "INSERT INTO users (name, email) VALUES (?, ?)",
        (name, email)
    )
    conn.commit()
    
    # More business logic here...
    return {"id": 1, "name": name, "email": email}
```

### After: Hexagonal Architecture

```python
# ✅ AFTER: Clean separation

# domain/models.py
from dataclasses import dataclass

@dataclass
class User:
    id: int
    name: str
    email: str

# domain/ports.py
from typing import Protocol

class UserRepository(Protocol):
    def save(self, user: User) -> None: ...
    def get_by_id(self, id: int) -> User | None: ...

# domain/services.py
from ..ports import UserRepository
from ..models import User

class UserService:
    def __init__(self, repo: UserRepository):
        self.repo = repo
    
    def create_user(self, name: str, email: str) -> User:
        user = User(
            id=hash(f"{name}{email}"),  # Simplified
            name=name,
            email=email
        )
        self.repo.save(user)
        return user

# adapters/sqlite_user_repo.py
import sqlite3
from ..domain.models import User
from ..domain.ports import UserRepository

class SQLiteUserRepository:
    def __init__(self, db_path: str):
        self.conn = sqlite3.connect(db_path)
        self._init_db()
    
    def _init_db(self):
        self.conn.execute("""
            CREATE TABLE IF NOT EXISTS users (
                id INTEGER PRIMARY KEY,
                name TEXT,
                email TEXT
            )
        """)
    
    def save(self, user: User) -> None:
        self.conn.execute(
            "INSERT INTO users (name, email) VALUES (?, ?)",
            (user.name, user.email)
        )
        self.conn.commit()
    
    def get_by_id(self, id: int) -> User | None:
        cursor = self.conn.execute(
            "SELECT * FROM users WHERE id = ?", (id,)
        )
        row = cursor.fetchone()
        if row:
            return User(id=row[0], name=row[1], email=row[2])
        return None

# api/main.py
from fastapi import FastAPI, Depends
from ..domain.models import User
from ..domain.services import UserService
from ..adapters.sqlite_user_repo import SQLiteUserRepository

app = FastAPI()

def get_user_service() -> UserService:
    repo = SQLiteUserRepository("users.db")
    return UserService(repo)

@app.post("/users/")
def create_user(
    name: str,
    email: str,
    service: UserService = Depends(get_user_service)
):
    user = service.create_user(name, email)
    return {"id": user.id, "name": user.name, "email": user.email}
```

---

## Summary

✅ **Layered** — simple, good for most projects

✅ **Hexagonal** — testable, framework-independent

✅ **Core domain** — zero external dependencies

✅ **Ports** — interfaces the core defines

✅ **Adapters** — concrete implementations

---

## ➡️ Next Steps

Continue to [03_refactoring_in_practice.md](./03_refactoring_in_practice.md)

---

## 🔗 Further Reading

- [Hexagonal Architecture](https://alistair.cockburn.us/hexagonal-architecture/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
