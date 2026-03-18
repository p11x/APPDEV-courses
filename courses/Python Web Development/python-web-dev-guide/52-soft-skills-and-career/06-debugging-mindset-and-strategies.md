# Debugging Mindset and Strategies

## What You'll Learn

- The scientific method applied to debugging
- Rubber duck debugging and why it works
- Binary search debugging—finding problems faster
- How to write a minimal reproducible example
- When to ask for help and how to frame the question

## Prerequisites

This assumes you've encountered bugs in your code (from building the guide projects). No specific prerequisites beyond folders 00–51.

## The Debugging Mindset

Debugging is not about being smart—it's about being systematic. The best debuggers aren't the smartest; they're the most methodical:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    THE SCIENTIFIC METHOD FOR BUGS                            │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. OBSERVE                                                                 │
│     What exactly is happening? What's the error?                           │
│     What's the expected behavior?                                         │
│                                                                             │
│  2. HYPOTHESIZE                                                             │
│     What could cause this? Form multiple hypotheses.                       │
│                                                                             │
│  3. EXPERIMENT                                                              │
│     Test each hypothesis. Isolate variables.                               │
│                                                                             │
│  4. ANALYZE                                                                 │
│     What did the experiment reveal? Eliminate or confirm hypotheses.       │
│                                                                             │
│  5. REPEAT                                                                  │
│     If not solved, go back to hypotheses.                                  │
│                                                                             │
│  ────────────────────────────────────────────────────────────────────────   │
│                                                                             │
│  The key insight: don't guess—test.                                        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Strategy 1: Rubber Duck Debugging

Explain the problem out loud to an inanimate object (or colleague). This works because:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHY RUBBER DUCK DEBUGGING WORKS                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  When you explain your code to someone (or something):                    │
│                                                                             │
│  1. You slow down and think through each step                             │
│  2. You notice gaps in your reasoning                                     │
│  3. You often answer your own question                                     │
│                                                                             │
│  "I don't know why this isn't—"  ← Then you notice                       │
│  "Oh, I forgot that the function returns None by default"                  │
│                                                                             │
│  This is called "procedural memory retrieval"—saying                      │
│  things out loud helps you access information differently                  │
│  than silently thinking.                                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### How to Rubber Duck

1. Get a rubber duck (or stress ball, plant, cat)
2. Explain what your code is supposed to do
3. Explain what it's actually doing
4. Walk through each line
5. Ask: "Where did it stop working?"

## Strategy 2: Binary Search Debugging

Instead of looking at every line, eliminate half the problem space at a time:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    BINARY SEARCH DEBUGGING                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Problem space: A → B → C → D → E                                         │
│                                                                             │
│  Step 1: Check if bug exists at C?                                        │
│  Step 2: Yes? Check between A and C                                       │
│         No? Check between C and E                                          │
│                                                                             │
│  Each question eliminates HALF the possibilities.                          │
│  In 10 steps, you can find a bug in 1000+ lines.                         │
│                                                                             │
│  Example: API returns 500                                                 │
│  ──────────────────────────                                               │
│  Step 1: Is the database query working? (Check logs)                    │
│  Step 2: Is the serialization working? (Add print)                      │
│  Step 3: Is the service layer working? (Test in isolation)              │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Practical Binary Search

```python
# Instead of reading all your code to find the bug,
# use print/log statements to narrow down:

async def get_user(user_id: int) -> User:
    print(f"DEBUG: get_user called with {user_id}")
    
    # Check: Is the function being called?
    # YES: continue
    
    print("DEBUG: about to query database")
    user = await db.query(user_id)
    print(f"DEBUG: got user: {user}")
    
    # Check: Did we get a user from DB?
    # YES: continue
    
    print("DEBUG: about to serialize")
    return UserResponse.model_validate(user)
    
    # Check: Did serialization work?
    # If not, you'll see the error here
```

Each print statement tells you: "Did I get past this point?"

## Strategy 3: The Minimal Reproducible Example

When you need help, reduce the problem to its simplest form:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CREATING A MINIMAL EXAMPLE                               │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  BEFORE (hard to debug):                                                   │
│  - 10 files involved                                                       │
│  - Database, Redis, external APIs                                          │
│  - Complex setup                                                           │
│                                                                             │
│  AFTER (easy to debug):                                                   │
│  - 1 file                                                                  │
│  - Mock data only                                                          │
│  - 20 lines of code                                                        │
│                                                                             │
│  The process:                                                              │
│  1. Remove everything not directly related to the bug                     │
│  2. Replace external dependencies with mock data                           │
│  3. Simplify until you can't remove more                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Example: Reducing a Bug

```python
# ORIGINAL (complex, hard to test):
async def process_user_order(order_id: int, user_id: int):
    user = await get_user(user_id)  # DB call
    order = await get_order(order_id)  # DB call
    validate_user(user)
    validate_order(order)
    charge_payment(user.payment_method, order.total)  # External API
    send_confirmation_email(user.email)  # External API
    update_inventory(order.items)  # DB + external
    # Bug somewhere in here...

# REDUCED:
async def test_order_validation():
    # Replace all dependencies with mocks
    user = MockUser(email="test@test.com")
    order = MockOrder(total=100)
    
    # Just test the one thing that's failing
    validate_user(user)  # This fails! Bug found!
```

## Common Python Debugging Patterns

### Pattern 1: Print Debugging

```python
# Quick and dirty but effective
async def complicated_function(x: int, y: str) -> dict:
    print(f"ENTER: x={x}, y={y}")
    try:
        result = await do_something(x, y)
        print(f"SUCCESS: {result}")
        return result
    except Exception as e:
        print(f"ERROR: {e}")
        raise
```

### Pattern 2: Logging

```python
import logging

logger = logging.getLogger(__name__)

async def complicated_function(x: int, y: str) -> dict:
    logger.debug("entering_function", extra={"x": x, "y": y})
    try:
        result = await do_something(x, y)
        logger.info("function_success", extra={"result": result})
        return result
    except Exception as e:
        logger.error("function_failed", extra={"error": str(e)})
        raise
```

### Pattern 3: PDB/ipdb

```python
# Add breakpoint
import ipdb

async def find_the_bug():
    result = await do_something()
    ipdb.set_trace()  # Execution stops here!
    # Now you can:
    # - n (next line)
    # - s (step into)
    # - p result (print result)
    # - c (continue)
    return process(result)
```

### Pattern 4: Using Tests to Debug

```python
# Write a failing test that reproduces the bug
def test_user_creation_with_duplicate_email():
    # This should raise an error
    with pytest.raises(EmailExistsError):
        create_user("test@example.com")
        create_user("test@example.com")  # Same email!
```

Run the test, see exactly where it fails.

## Debugging Common Web App Issues

### 1. API Returns 500

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DEBUGGING 500 ERRORS                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Step 1: Check the logs                                                   │
│  ```bash                                                                   │
│  docker compose logs app                                                   │
│  ```                                                                       │
│                                                                             │
│  Step 2: Find the traceback—look for the EXACT LINE that failed          │
│                                                                             │
│  Step 3: Reproduce locally with same input                                 │
│                                                                             │
│  Step 4: Add logging around that line to understand the state             │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### 2. Database Query Slow

```python
# Use SQLAlchemy echo to see queries:
engine = create_async_engine(URL, echo=True)

# Or add timing:
import time
start = time.time()
result = await session.execute(query)
print(f"Query took {time.time() - start:.2f}s")
```

### 3. Memory Leak

```python
# Use tracemalloc to find memory issues
import tracemalloc

tracemalloc.start()

# Run your code
await process_many_users()

# Print memory usage
current, peak = tracemalloc.get_traced_memory()
print(f"Current: {current / 1024 / 1024:.1f} MB")
print(f"Peak: {peak / 1024 / 1024:.1f} MB")
```

## When to Ask for Help

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHEN TO ASK FOR HELP                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ ASK IF:                                                               │
│  • You've spent 1+ hour stuck                                             │
│  • You're going in circles                                                │
│  • You need domain knowledge you don't have                               │
│  • The bug is in code you didn't write                                    │
│                                                                             │
│  ❌ DON'T ASK IF:                                                         │
│  • You haven't tried anything yet                                         │
│  • You just want someone to do your work                                  │
│  • You're frustrated and want to vent (do that to a duck instead)        │
│                                                                             │
│  KEY: Show you've tried. Even one hour of debugging                      │
│  demonstrates effort.                                                      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Real-World Application

### At Work

At a company, debugging production issues is your job. The best engineers:
- Keep calm when things break
- Use observability (logs, metrics, traces) before adding new logs
- Know when to roll back vs. debug in place
- Write postmortems to prevent recurrence

### On Your Own

When debugging personal projects:
- Start with rubber duck debugging (saves so much time)
- Use the scientific method—don't guess
- Create minimal reproductions

## Tools & Resources

| Tool | Purpose |
|------|---------|
| ipdb | Interactive debugger |
| PDB | Built-in Python debugger |
| Sentry | Production error tracking |
| logging | Structured logging |
| pytest.raises | Test-driven debugging |

```bash
pip install ipdb  # Best Python debugger
```

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Changing Code Randomly

**Wrong:**
Making random changes hoping something works.

**Why it fails:**
You'll introduce new bugs and won't understand what fixed it.

**Correct:**
Make one change, test, observe. Repeat.

### ❌ Mistake 2: Not Reading the Error

**Wrong:**
Skimming the error, then changing code.

**Why it fails:**
The error message usually tells you exactly what's wrong.

**Correct:**
Read the error message carefully—often the answer is in the traceback.

### ❌ Mistake 3: Not Isolating the Problem

**Wrong:**
Trying to debug while all your dependencies are involved.

**Why it fails:**
Too many variables—you can't tell what's causing the issue.

**Correct:**
Create a minimal reproduction that isolates the bug.

### ❌ Mistake 4: Not Taking Breaks

**Wrong:**
Spending 6 hours straight on the same bug.

**Why it fails:**
Your brain stops working after 2 hours. You make more mistakes.

**Correct:**
Take a break. Walk around. Come back with fresh eyes.

## Summary

- Debugging is methodical, not magical—use the scientific method
- Rubber duck debugging: explain the problem out loud
- Binary search: eliminate half the problem space at a time
- Minimal reproduction: isolate the bug to its simplest form
- When asking for help, show you've tried and have a specific question

## Next Steps

→ `07-asking-good-technical-questions.md` — How to ask technical questions that get answered quickly.
