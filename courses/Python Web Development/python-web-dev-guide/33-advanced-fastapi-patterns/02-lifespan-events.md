# Lifespan Events in FastAPI

## What You'll Learn
- Application lifecycle management with lifespan events
- Startup and shutdown logic
- Resource pooling and connection management
- Background tasks during shutdown
- The difference between lifespan and middleware

## Prerequisites
- Completed `01-custom-middleware.md` — FastAPI middleware basics
- Understanding of async/await in Python 3.11+

## What Are Lifespan Events?

Lifespan events let you run code when your FastAPI application starts up and shuts down. Unlike middleware (which runs on every request), lifespan events run once per application lifecycle.

**Use cases:**
- Database connection pool initialization
- Cache warming
- Opening WebSocket connections
- Starting background workers
- Graceful shutdown (closing connections, saving state)

## Using the Lifespan Context Manager

FastAPI 0.109+ introduced a cleaner lifespan API using a context manager:

```python
from contextlib import asynccontextmanager
from fastapi import FastAPI
import logging

logger = logging.getLogger(__name__)

@asynccontextmanager
async def lifespan(app: FastAPI):
    """Application lifespan handler."""
    # STARTUP: Run when app starts
    logger.info("🚀 Application starting up...")
    
    # Initialize database connections
    await initialize_database_pool()
    
    # Warm up cache
    await warm_up_cache()
    
    logger.info("✅ Startup complete!")
    
    yield  # App runs here
    
    # SHUTDOWN: Run when app stops
    logger.info("🛑 Application shutting down...")
    
    # Close database connections
    await close_database_pool()
    
    # Save cache to disk
    await save_cache()
    
    logger.info("✅ Shutdown complete!")

app = FastAPI(lifespan=lifespan)

@app.get("/health")
async def health_check():
    return {"status": "healthy"}
```

🔍 **Line-by-Line Breakdown:**
1. `@asynccontextmanager` — Creates an async context manager for lifecycle management
2. `app: FastAPI` — The FastAPI app instance is passed to the lifespan
3. `await initialize_database_pool()` — Code runs once at startup before accepting requests
4. `yield` — The point where the app is fully running and handling requests
5. After yield — Code runs during graceful shutdown

## Complete Example: Database Pool Management

```python
from contextlib import asynccontextmanager
from fastapi import FastAPI
from databases import Database
import logging
from typing import AsyncGenerator

logger = logging.getLogger(__name__)

# Global database instance
database: Database | None = None

async def get_database() -> AsyncGenerator[Database, None]:
    """Dependency to get database instance."""
    if database is None:
        raise RuntimeError("Database not initialized")
    yield database

@asynccontextmanager
async def lifespan(app: FastAPI) -> AsyncGenerator[None, None]:
    """Manage database pool lifecycle."""
    global database
    
    # ─────────────────────────────────────────────
    # STARTUP
    # ─────────────────────────────────────────────
    logger.info("Connecting to database...")
    
    database = Database(
        "postgresql://user:pass@localhost/mydb",
        min_size=5,      # Minimum connections
        max_size=20,    # Maximum connections
    )
    
    # Actually establish connection
    await database.connect()
    
    logger.info(f"Database connected! Pool: min=5, max=20")
    
    # ─────────────────────────────────────────────
    # APP RUNNING
    # ─────────────────────────────────────────────
    yield
    
    # ─────────────────────────────────────────────
    # SHUTDOWN
    # ─────────────────────────────────────────────
    logger.info("Disconnecting from database...")
    
    if database:
        await database.disconnect()
    
    logger.info("Database disconnected!")

app = FastAPI(lifespan=lifespan)

@app.get("/users/{user_id}")
async def get_user(user_id: int):
    query = "SELECT * FROM users WHERE id = :user_id"
    return await database.fetch_one(query, {"user_id": user_id})
```

## Background Tasks During Shutdown

Handle graceful shutdown by running tasks before exiting:

```python
import asyncio
from contextlib import asynccontextmanager
from fastapi import FastAPI
from queue import Queue

# Background job queue
job_queue: Queue | None = None

@asynccontextmanager
async def lifespan(app: FastAPI):
    """Manage job processing lifecycle."""
    global job_queue
    
    # Startup: Start background worker
    job_queue = Queue()
    worker_task = asyncio.create_task(process_jobs())
    
    logger.info("Background worker started")
    
    yield
    
    # Shutdown: Wait for pending jobs
    logger.info("Waiting for pending jobs to complete...")
    job_queue.put(None)  # Signal worker to stop
    
    try:
        await asyncio.wait_for(worker_task, timeout=30.0)
    except asyncio.TimeoutError:
        logger.warning("Some jobs incomplete, forcing shutdown")
    
    logger.info("All jobs processed, shutdown complete")

async def process_jobs():
    """Background job processor."""
    while True:
        job = await asyncio.get_event_loop().run_in_executor(
            None, job_queue.get
        )
        if job is None:
            break
        await process_single_job(job)

async def process_single_job(job: dict):
    """Process a single job."""
    logger.info(f"Processing job: {job}")
    await asyncio.sleep(1)  # Simulate work

app = FastAPI(lifespan=lifespan)
```

## Lifespan vs Middleware

| Aspect | Lifespan | Middleware |
|--------|----------|------------|
| **Runs** | Once at startup/shutdown | Every request |
| **Purpose** | Resource management | Request/response processing |
| **Timing** | App lifecycle | HTTP request lifecycle |
| **Examples** | DB pool, cache, workers | Logging, auth, CORS |

## Storing State in App

Access app state from anywhere:

```python
from contextlib import asynccontextmanager
from fastapi import FastAPI
from dataclasses import dataclass

@dataclass
class AppState:
    """Application state container."""
    database_url: str
    redis_client: Redis | None = None
    startup_time: float = 0.0

@asynccontextmanager
async def lifespan(app: FastAPI):
    """Initialize and store app state."""
    import time
    
    state = AppState(
        database_url="postgresql://localhost/mydb",
        redis_client=Redis(),
    )
    state.startup_time = time.time()
    
    app.state = state
    
    yield
    
    # Cleanup
    await state.redis_client.close()

app = FastAPI(lifespan=lifespan)

@app.get("/info")
async def get_info(request: Request):
    state: AppState = request.app.state
    return {
        "database": state.database_url,
        "uptime": time.time() - state.startup_time
    }
```

## Production Considerations

- **Graceful shutdown**: Always close connections on shutdown to avoid data loss
- **Timeout**: Set timeouts on shutdown tasks to prevent hanging indefinitely
- **Error handling**: Wrap startup in try/except to prevent starting in broken state
- **Idempotency**: Make startup code idempotent (safe to run multiple times)

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Blocking code in lifespan

**Wrong:**
```python
@asynccontextmanager
async def lifespan(app: FastAPI):
    # Sync blocking call!
    time.sleep(10)
    yield
```

**Why it fails:** Blocks the event loop during startup.

**Fix:**
```python
@asynccontextmanager
async def lifespan(app: FastAPI):
    await asyncio.sleep(10)  # Non-blocking
    yield
```

### ❌ Mistake 2: Not handling startup failures

**Wrong:**
```python
@asynccontextmanager
async def lifespan(app: FastAPI):
    database = await connect()  # Might fail!
    yield
```

**Why it fails:** If startup fails, app might run in broken state.

**Fix:**
```python
@asynccontextmanager
async def lifespan(app: FastAPI):
    try:
        database = await connect()
    except Exception as e:
        logger.error(f"Startup failed: {e}")
        raise RuntimeError("Cannot start app") from e
    
    yield
```

### ❌ Mistake 3: Forgetting to close resources

**Wrong:**
```python
@asynccontextmanager
async def lifespan(app: FastAPI):
    database = Database(...)
    await database.connect()
    yield
    # No disconnect! Leaks connections!
```

**Why it fails:** Database connections stay open, causing resource exhaustion.

**Fix:**
```python
@asynccontextmanager
async def lifespan(app: FastAPI):
    database = Database(...)
    await database.connect()
    
    yield
    
    await database.disconnect()  # Clean shutdown
```

## Summary

- Lifespan events run once at startup and once at shutdown
- Use `@asynccontextmanager` with `lifespan` parameter
- Put initialization code before `yield`, cleanup code after
- Ideal for database pools, cache warming, background workers
- Always close resources on shutdown for graceful termination

## Next Steps

→ Continue to `03-custom-exception-handlers.md` to learn how to create custom error responses in FastAPI.
