# Python Backend Interview Prep

## What You'll Learn

- The five types of Python backend interviews
- Common Python gotchas interviewers test
- FastAPI/Django questions with model answers
- How to think out loud during a technical interview
- How to handle questions you don't know

## Prerequisites

This builds on all the technical content in the guide. You should have completed folders 00–51.

## The Five Types of Interviews

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    TYPES OF BACKEND INTERVIEWS                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. CODING (30–45 min)                                                    │
│     Algorithms and data structures, LeetCode-style                        │
│     Difficulty: Medium for junior, Hard for senior                        │
│                                                                             │
│  2. SYSTEM DESIGN (30–45 min)                                              │
│     Design a URL shortener, chat system, etc.                             │
│     Difficulty: Increases with seniority                                  │
│                                                                             │
│  3. BEHAVIORAL (30–45 min)                                                │
│     Past experiences, team work, conflict resolution                       │
│     Difficulty: N/A—preparation helps                                     │
│                                                                             │
│  4. TAKE-HOME (2–8 hours)                                                 │
│     Build something, present in follow-up                                  │
│     Difficulty: Shows real work                                           │
│                                                                             │
│  5. PAIR PROGRAMMING (30–60 min)                                          │
│     Code together with interviewer                                         │
│     Difficulty: Shows real collaboration                                   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Python-Specific Gotchas

Interviewers love testing these Python concepts:

### 1. Mutable Default Arguments

```python
# ❌ THE CLASSIC TRAP:
def add_item(item, items=[]):
    items.append(item)
    return items

add_item("a")  # Returns ["a"]
add_item("b")  # Returns ["a", "b"]  ← PROBLEM!
```

🔍 **Why it fails:**
Default arguments are evaluated ONCE at function definition, not each call. The list persists across calls.

**Correct answer:**
```python
def add_item(item, items=None):
    if items is None:
        items = []
    items.append(item)
    return items
```

### 2. Async/Await in Loops

```python
# ❌ WRONG (all run simultaneously, not awaited):
async def process_all(items: list[str]) -> list[dict]:
    results = []
    for item in items:
        results.append(process(item))  # Not awaited!
    return results

# ✅ CORRECT:
async def process_all(items: list[str]) -> list[dict]:
    results = []
    for item in items:
        results.append(await process(item))  # Awaited!
    return results

# ✅ ALSO CORRECT (concurrent):
async def process_all(items: list[str]) -> list[dict]:
    tasks = [process(item) for item in items]
    return await asyncio.gather(*tasks)
```

### 3. == vs is

```python
# ❌ WRONG:
if variable == None:  # Should use "is"

# ✅ CORRECT:
if variable is None:

# Why:
# == checks equality
# is checks identity (same object in memory)
```

### 4. List Comprehension vs Generator

```python
# List comprehension - eager (immediate):
def get_items():
    return [i for i in range(1000000)]  # Evaluates immediately

# Generator - lazy (on demand):
def get_items():
    return (i for i in range(1000000))  # Evaluates when iterated

# For large sequences, generators save memory
```

## FastAPI Questions

### Q: How does FastAPI handle async vs sync functions?

```markdown
FastAPI supports both sync and async functions:

Sync (def):
- Runs in a thread pool
- Good for blocking operations (file I/O, sync DB drivers)

Async (async def):
- Runs on the main event loop
- Good for non-blocking I/O (async DB drivers, HTTP calls)

FastAPI automatically detects which you're using:

```python
# Sync - runs in thread pool
@app.get("/sync")
def sync_endpoint():
    return {"message": "sync"}

# Async - runs on event loop
@app.get("/async")
async def async_endpoint():
    return {"message": "async"}
```

Key: Don't mix! If you use async def, all dependencies must also be async.
```

### Q: How do you handle errors in FastAPI?

```markdown
1. Use HTTPException for HTTP errors:
```python
from fastapi import HTTPException

@app.get("/user/{user_id}")
async def get_user(user_id: int):
    user = await db.get_user(user_id)
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    return user
```

2. Create custom exception handlers:
```python
from fastapi import Request
from fastapi.responses import JSONResponse

@app.exception_handler(ValueError)
async def value_error_handler(request: Request, exc: ValueError):
    return JSONResponse(status_code=400, content={"error": str(exc)})
```

3. Use Pydantic validation errors automatically.
```

### Q: How does FastAPI's dependency injection work?

```markdown
FastAPI's Depends() provides dependency injection:

1. Define dependencies:
```python
async def get_db():
    async with get_session() as session:
        yield session
```

2. Inject them:
```python
@app.get("/users")
async def list_users(db: AsyncSession = Depends(get_db)):
    return await db.execute(select(User))
```

Benefits:
- Reusable logic across endpoints
- Easy to mock in tests
- Automatic cleanup (yield)
```

## Django Questions

### Q: What's the difference between select_related and prefetch_related?

```python
# select_related - SQL JOIN (for ForeignKey, OneToOne)
# One additional query
user = User.objects.select_related('profile').get(id=1)
# SELECT * FROM users JOIN profiles ON ...

# prefetch_related - Separate query + Python join (for ManyToMany, Reverse FK)
# Two additional queries
posts = User.objects.prefetch_related('tags').get(id=1)
# SELECT * FROM users
# SELECT * FROM tags WHERE user_id IN (1)
```

### Q: How does Django's ORM handle N+1 queries?

```python
# ❌ N+1 Problem:
users = User.objects.all()  # 1 query
for user in users:
    print(user.profile.bio)  # N queries!

# ✅ Solution: select_related
users = User.objects.select_related('profile')  # 1 query with JOIN
```

## Behavioral Questions

Use the STAR method:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    STAR METHOD                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  S - Situation: Describe the context                                       │
│  T - Task: What was your responsibility?                                  │
│  A - Action: What did you specifically do?                               │
│  R - Result: What was the outcome?                                        │
│                                                                             │
│  Example: "Tell me about a time you had a conflict."                    │
│                                                                             │
│  S: "Our team disagreed on the architecture for the new API..."          │
│  T: "As lead, I needed to resolve the conflict..."                       │
│  A: "I scheduled a meeting, had each side present pros/cons..."          │
│  R: "We chose option B, shipped on time, and the team felt heard"       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Common Behavioral Questions

| Question | What They Test |
|----------|----------------|
| Tell me about yourself | Communication |
| Why do you want to work here? | Research, motivation |
| Greatest weakness | Self-awareness |
| Greatest strength | Confidence |
| Tell me about a conflict | Conflict resolution |
| Tell me about a failure | Growth mindset |
| Why should we hire you? | Self-advocacy |

## Thinking Out Loud

During coding questions, narrate your thought process:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    THINK OUT LOUD                                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ DO:                                                                    │
│  • "Let me think through this..."                                         │
│  • "I'm considering two approaches..."                                     │
│  • "The edge case here is..."                                             │
│  • "Let me verify with an example..."                                     │
│                                                                             │
│  ❌ DON'T:                                                                 │
│  • Sit in silence                                                          │
│  • Rush to code without planning                                           │
│  • Get stuck without asking for clarification                              │
│                                                                             │
│  Talking shows:                                                            │
│  • How you think                                                           │
│  • Your communication skills                                               │
│  • Whether you can collaborate                                             │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Handling Unknown Questions

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    HANDLING UNKNOWNS                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ BEST RESPONSE:                                                         │
│  "I'm not familiar with that specific thing, but based on                 │
│   what I know about [related topic], I'd expect it to work                │
│   like this..."                                                            │
│                                                                             │
│  Shows:                                                                    │
│  • You can extrapolate from knowledge                                       │
│  • You're honest                                                            │
│  • You understand the domain                                              │
│                                                                             │
│  ❌ BAD RESPONSE:                                                          │
│  "I don't know" (one-word answer)                                         │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Real-World Application

### At Different Company Types

| Company | Focus |
|---------|-------|
| Big Tech (Google, Meta) | LeetCode + System Design |
| Startups | Practical coding + Build from scratch |
| Mid-size | Mix of LeetCode and practical |
| Enterprise | Domain knowledge + Practical |

### Preparation Timeline

```
Week 1-2:  LeetCode easy/medium (5 problems/day)
Week 3-4:  System design (2 problems/day)  
Week 5:     Behavioral prep
Week 6:     Mock interviews
```

## Summary

- Master Python gotchas: mutable defaults, async/await, == vs is
- Know FastAPI and Django deeply
- Use STAR for behavioral questions
- Think out loud during coding
- It's okay to not know—show how you'd figure it out

## Next Steps

→ `11-system-design-interview-prep.md` — System design interview framework and common questions.
