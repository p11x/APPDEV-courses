# ⚡ Async Databases and Migrations

## 🎯 What You'll Learn

- Async SQLite with aiosqlite
- SQLAlchemy async
- Alembic migrations

---

## Async SQLite

```bash
pip install aiosqlite
```

```python
import aiosqlite
import asyncio

async def main():
    async with aiosqlite.connect("myapp.db") as db:
        # Create table
        await db.execute("""
            CREATE TABLE IF NOT EXISTS users (
                id INTEGER PRIMARY KEY,
                name TEXT
            )
        """)
        await db.commit()
        
        # Insert
        await db.execute("INSERT INTO users (name) VALUES (?)", ("Alice",))
        await db.commit()
        
        # Query
        async with db.execute("SELECT * FROM users") as cursor:
            rows = await cursor.fetchall()
            for row in rows:
                print(row)

asyncio.run(main())
```

---

## SQLAlchemy Async

```bash
pip install sqlalchemy[asyncio] aiosqlite
```

```python
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker

engine = create_async_engine("sqlite+aiosqlite:///myapp.db")
async_session = sessionmaker(engine, class_=AsyncSession, expire_on_commit=False)

async def get_users():
    async with async_session() as session:
        result = await session.execute(select(User))
        return result.scalars().all()
```

---

## Alembic Migrations

```bash
# Install
pip install alembic

# Initialize
alembic init alembic

# Create migration
alembic revision --autogenerate -m "add users table"

# Run migrations
alembic upgrade head

# Rollback
alembic downgrade -1
```

### Migration File

```python
"""add users table

Revision ID: abc123
Revises: 
Create Date: 2024-01-15 10:00:00

"""
from alembic import op
import sqlalchemy as sa

revision = 'abc123'
down_revision = None
branch_labels = None
depends_on = None

def upgrade():
    op.create_table(
        'users',
        sa.Column('id', sa.Integer(), nullable=False),
        sa.Column('name', sa.String(length=100), nullable=False),
        sa.PrimaryKeyConstraint('id')
    )

def downgrade():
    op.drop_table('users')
```

---

## ✅ Summary

- Use aiosqlite for async SQLite
- SQLAlchemy async with AsyncSession
- Alembic for version-controlled schema migrations

## 🔗 Further Reading

- [Alembic Documentation](https://alembic.sqlalchemy.org/)
