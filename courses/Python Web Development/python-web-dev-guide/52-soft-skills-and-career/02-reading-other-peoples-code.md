# Reading Other People's Code

## What You'll Learn

- A systematic approach to understanding unfamiliar codebases
- How to find the entry point in any project
- Using git history and blame to understand why code exists
- Building a mental map of a large system
- How to read code effectively without getting overwhelmed
- Navigating FastAPI's source code as a practical exercise

## Prerequisites

This builds on all previous content. You should be comfortable with:
- Python and FastAPI (folders 00–51)
- Git basics (folder 53)

## The Challenge of Unknown Codebases

Every Python developer spends more time reading code than writing it. Studies show ratios of 5:1 or even 10:1. Yet most developers have never been taught how to read code—they just dive in and struggle.

The key insight: reading code is a skill separate from writing code. It requires different strategies, tools, and mental models.

## A Systematic Approach

Here's the order to approach any codebase:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CODEBASE NAVIGATION STRATEGY                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. START AT THE EDGE                                                       │
│     ├── Entry points (main.py, __main__.py, cli.py)                       │
│     ├── Configuration files (pyproject.toml, .env.example)                 │
│     └── README                                                              │
│                                                                             │
│  2. TRACE THE FLOW                                                          │
│     ├── How requests reach the application                                  │
│     ├── How responses are generated                                         │
│     └── What dependencies are injected                                      │
│                                                                             │
│  3. UNDERSTAND THE DOMAIN                                                  │
│     ├── What problem does this solve?                                       │
│     ├── What are the core entities?                                         │
│     └── How is data structured?                                             │
│                                                                             │
│  4. MAP THE ARCHITECTURE                                                   │
│     ├── Directory structure                                                 │
│     ├── Key modules and their responsibilities                              │
│     └── How components interact                                             │
│                                                                             │
│  5. DEEP DIVE INTO DETAILS                                                │
│     ├── Read the most important files                                       │
│     ├── Run tests to understand behavior                                    │
│     └── Make small changes to verify understanding                         │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Step 1: Finding Entry Points

Every application has an entry point where execution begins. Find it first:

```bash
# Look for entry points in pyproject.toml
grep -A5 "scripts\|console_scripts" pyproject.toml

# Look for main files
ls -la *.py

# Check for cli modules
find . -name "cli.py" -o -name "main.py" | head -10
```

In a FastAPI application, the entry point typically looks like this:

```python
# app/main.py
from fastapi import FastAPI
from contextlib import asynccontextmanager

@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup logic
    await database.connect()
    yield
    # Shutdown logic
    await database.disconnect()

app: FastAPI = FastAPI(lifespan=lifespan)

@app.get("/")
async def root() -> dict:
    return {"message": "Hello"}
```

🔍 **Line-by-Line Breakdown:**

1. `@asynccontextmanager` — Defines lifespan events; this is where the app starts and stops
2. `lifespan(app: FastAPI)` — FastAPI's startup/shutdown hook; all initialization happens here
3. `app: FastAPI = FastAPI(lifespan=lifespan)` — The application instance is created here
4. `@app.get("/")` — This is the root endpoint; from here, trace what happens for a request

Once you find the entry point, trace backward to understand:
- What dependencies are created at startup
- How the application is configured
- What middleware is applied

## Step 2: Using Git History to Understand Context

Code tells you what. Git history tells you why:

```bash
# See who wrote the most code in a file
git shortlog -srn -- path/to/file.py | head -5

# See the commit history for a specific function
git log -p --follow -S "def my_function" -- path/to/file.py | head -100

# See blame for a specific line
git blame -L 10,20 path/to/file.py
```

The `git blame` output is invaluable:

```bash
$ git blame app/services/user_service.py
a1b2c3d4 (alice 2024-01-15 12) def create_user(email: str) -> User:
a1b2c3d4 (alice 2024-01-15 12)     """Create a new user."""
e5f6g7h8 (bob   2024-02-20 15)     if User.get_by_email(email):
e5f6g7h8 (bob   2024-02-20 15)         raise UserExistsError()
i9j0k1l2 (alice 2024-01-15 12)     return User.create(email=email)
```

You can see:
- Who wrote each line
- When it was written
- What changes have been made over time

**Practical tip:** When you find confusing code, use `git log` to find the PR that introduced it:

```bash
# Find the commit that added this function
git log --oneline --all -S "def confusing_function" | head -5

# See the PR description
git log -1 --format="%B" COMMIT_HASH
```

## Step 3: Building a Mental Map

For large codebases, you need a mental map before you can understand details. Here's how to build one:

### A. Directory Structure Analysis

Start by understanding the folder structure:

```bash
# See the overall structure
find . -type d -name "__pycache__" -prune -o -type d -print | head -30

# Or use tree (install with: pip install tre)
tree -L 2 -d
```

For a FastAPI project, a typical structure looks like:

```
app/
├── __init__.py          # Package initialization
├── main.py              # FastAPI app creation + lifespan
├── config.py            # Configuration (Pydantic settings)
├── dependencies.py      # FastAPI dependencies
├── models/              # SQLAlchemy models
│   ├── __init__.py
│   ├── user.py
│   └── item.py
├── schemas/             # Pydantic models
│   ├── __init__.py
│   ├── user.py
│   └── item.py
├── routers/             # API endpoints
│   ├── __init__.py
│   ├── auth.py
│   └── items.py
├── services/            # Business logic
│   ├── __init__.py
│   ├── auth_service.py
│   └── item_service.py
└── utils/               # Helper functions
    ├── __init__.py
    └── security.py
```

This structure follows a common pattern: organize by *layer* (models, schemas, routers, services). Other patterns exist, but recognizing layers helps you navigate.

### B. Understanding Dependencies

In FastAPI, dependencies flow from routers to services to models:

```
Request ──▶ Router ──▶ Service ──▶ Model ──▶ Database
    ▲          │          │
    └──────────┴──────────┘
         (dependencies)
```

To trace a request:

1. Find the router that handles the endpoint
2. Find what services it depends on
3. Find what models those services use

```python
# app/routers/items.py
from fastapi import APIRouter, Depends
from app.services.item_service import ItemService
from app.dependencies import get_item_service

router = APIRouter()

@router.get("/items/{item_id}")
async def get_item(
    item_id: int,
    service: ItemService = Depends(get_item_service)
) -> ItemResponse:
    return await service.get_item(item_id)
```

🔍 **Line-by-Line Breakdown:**

1. `@router.get("/items/{item_id}")` — This handles GET /items/{item_id}
2. `service: ItemService = Depends(get_item_service)` — The service is injected
3. `await service.get_item(item_id)` — The actual logic is in the service

Trace this pattern and you can understand any endpoint in minutes.

## Practical Exercise: Navigating FastAPI Source

Let's apply this to understanding FastAPI itself—the framework you use daily:

```bash
# Clone FastAPI to explore
git clone --depth 1 https://github.com/fastapi/fastapi.git
cd fastapi
```

### Step 1: Find the entry point

```bash
# FastAPI's main module
ls -la fastapi/
```

You'll find:
- `fastapi/__init__.py` — The main package
- `fastapi/api.py` — API class
- `fastapi/applications.py` — FastAPI app class

```python
# fastapi/__init__.py (simplified)
from fastapi.api import APIRoute
from fastapi.applications import FastAPI as FastAPI

__version__ = "0.109.0"
__all__ = ["FastAPI", "APIRoute"]
```

### Step 2: Find how routes are created

The core of FastAPI is how it converts a function to an endpoint:

```python
# fastapi/api.py (simplified)
class FastAPI:
    def get(self, path: str, **kwargs):
        return self.router.get(path, **kwargs)
    
    def post(self, path: str, **kwargs):
        return self.router.post(path, **kwargs)
```

Each HTTP method delegates to the router.

### Step 3: Find how the router works

```python
# fastapi/routing.py (simplified)
class APIRoute(BaseRoute):
    def __init__(self, path: str, endpoint, **kwargs):
        self.path = path
        self.endpoint = endpoint
        # ... setup code ...
        
    async def endpoint_handler(self, request: Request) -> Response:
        # 1. Parse request parameters
        # 2. Call the endpoint function
        # 3. Serialize the response
        return await self._run_endpoint()
```

The key insight: FastAPI's magic is in parameter parsing. It inspects your function signature and automatically:
- Path parameters from the URL
- Query parameters from ?foo=bar
- Body parameters from JSON
- Headers from the request
- Dependencies from Depends()

This pattern—inspecting function signatures—is how most Python frameworks work. Once you see it, you can debug any framework.

## Step 4: Using Print Debugging to Understand Code

Sometimes you need to see what code actually does:

```python
# Add print statements to trace execution
async def complicated_function(x: int, y: str) -> dict:
    print(f"DEBUG: entering complicated_function with x={x}, y={y}")
    result = await do_something(x, y)
    print(f"DEBUG: leaving complicated_function, result={result}")
    return result
```

For production code, use structured logging instead:

```python
import logging

logger = logging.getLogger(__name__)

async def complicated_function(x: int, y: str) -> dict:
    logger.debug("entering_complicated_function", extra={"x": x, "y": y})
    result = await do_something(x, y)
    logger.debug("leaving_complicated_function", extra={"result": result})
    return result
```

## Step 5: Running Tests to Understand Behavior

Tests are executable documentation:

```bash
# Find tests related to a function
grep -r "def test_" tests/ | grep "function_name"

# Run a specific test to see output
pytest tests/test_specific.py::test_name -v -s
```

When you don't understand code, running its tests shows you:
- What inputs it expects
- What outputs it produces
- What edge cases are tested

## Tools & Resources

| Tool | Purpose | Installation |
|------|---------|--------------|
| py-spy | Sample-based profiler to see what code runs | `pip install py-spy` |
| pyfinder | Browse code from the command line | `pip install pyfinder` |
| grep / ripgrep | Search codebases | Built into most systems |
| git blame | See who wrote each line | Built into git |
| tree | See directory structure | `pip install tre` |

**Key resources:**
- [FastAPI Source Code](https://github.com/fastapi/fastapi) — Well-organized, readable framework
- [SQLAlchemy Source](https://github.com/sqlalchemy/sqlalchemy) — More complex but instructive
- [tiangolo/fastapi-applications](https://github.com/tiangolo/fastapi-applications) — Real-world FastAPI apps

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Starting in the Middle of the Code

**Wrong approach:**
Opening a random file in the middle of the project and trying to understand it in isolation.

**Why it fails:**
You have no context. You don't know where this code is called from or what it's supposed to do.

**Correct approach:**
Always start at entry points and trace forward. You can't understand a function in isolation—understand the system first.

### ❌ Mistake 2: Reading Every Line

**Wrong approach:**
Trying to read every line of code in a large codebase.

**Why it fails:**
A production codebase has millions of lines. You'll never finish and you'll forget what you read first.

**Correct approach:**
Read enough to understand the architecture, then dive only into the parts you need to modify.

### ❌ Mistake 3: Not Using Git History

**Wrong approach:**
Staring at confusing code and trying to deduce why it was written that way.

**Why it fails:**
Code often looks weird for good reasons (workarounds for bugs, legacy decisions, performance hacks). The only way to know is to find the commit that introduced it.

**Correct approach:**
Use `git blame` and `git log` to find context. The commit message and PR description explain why.

### ❌ Mistake 4: Not Running the Code

**Wrong approach:**
Reading code without executing it.

**Why it fails:**
Code tells you what it does. Running it shows you what actually happens (which may be different).

**Correct approach:**
- Run the application
- Make small changes
- See what breaks
- Add print statements to trace execution

## Summary

- Reading code is a skill separate from writing code—develop it deliberately
- Always start at entry points and trace the flow of execution
- Use `git blame` and `git log` to understand why code exists
- Build a mental map of the architecture before diving into details
- Use tests as executable documentation
- Make small changes to verify your understanding

## Next Steps

→ `03-code-review-etiquette.md` — How to give and receive feedback on code reviews professionally.
