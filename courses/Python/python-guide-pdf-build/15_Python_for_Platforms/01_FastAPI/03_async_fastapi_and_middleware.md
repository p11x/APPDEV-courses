# ⚡ Async FastAPI and Middleware

## 🎯 What You'll Learn

- Async endpoints
- Dependency injection with Depends
- Background tasks
- Middleware

---

## Async Endpoints

```python
from fastapi import FastAPI
import asyncio

app = FastAPI()

# Regular endpoint
@app.get("/sync")
def sync_endpoint():
    return {"method": "sync"}

# Async endpoint - use when you have I/O
@app.get("/async")
async def async_endpoint():
    # Simulate async I/O
    await asyncio.sleep(0.1)
    return {"method": "async"}

# When to use each:
# - async: I/O-bound (database, API calls, file reads)
# - sync: CPU-bound (computations)
```

---

## Dependency Injection

```python
from fastapi import Depends

# Dependency function
async def get_db():
    """Database session dependency."""
    db = DatabaseConnection()
    try:
        yield db
    finally:
        db.close()

# Use in endpoint
@app.get("/users")
async def list_users(db = Depends(get_db)):
    return db.query("SELECT * FROM users")

# Auth dependency
async def get_current_user(authorization: str = Depends(lambda: "user")):
    # Verify token...
    return {"user_id": 1}

@app.get("/protected")
async def protected(user = Depends(get_current_user)):
    return user
```

---

## Background Tasks

```python
from fastapi import BackgroundTasks

def send_email(email: str, message: str):
    # This runs AFTER the response is sent!
    print(f"Sending email to {email}: {message}")

@app.post("/submit")
async def submit_form(
    email: str,
    background_tasks: BackgroundTasks
):
    # Returns immediately, email sends in background
    background_tasks.add_task(send_email, email, "Thank you!")
    return {"status": "submitted"}
```

---

## Middleware

```python
from fastapi import Request
from fastapi.middleware.cors import CORSMiddleware
import time

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Custom middleware
@app.middleware("http")
async def add_process_time(request: Request, call_next):
    start = time.time()
    response = await call_next(request)
    process_time = time.time() - start
    response.headers["X-Process-Time"] = str(process_time)
    return response
```

---

## Lifespan Events

```python
from contextlib import asynccontextmanager

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup
    print("Starting up...")
    await connect_to_database()
    
    yield  # App runs here
    
    # Shutdown
    print("Shutting down...")
    await close_database()

app = FastAPI(lifespan=lifespan)
```

---

## Project Structure

```
app/
├── main.py          # Entry point
├── routers/
│   ├── users.py
│   └── items.py
├── models.py        # Pydantic models
├── dependencies.py  # Depends functions
└── database.py      # DB connection
```

---

## ✅ Summary

- Use async for I/O-bound endpoints
- Depends() for dependency injection
- BackgroundTasks for work after response
- Middleware for request/response processing

## 🔗 Further Reading

- [FastAPI Dependencies](https://fastapi.tiangolo.com/tutorial/dependencies/)
