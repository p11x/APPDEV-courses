# Code Review Etiquette

## What You'll Learn

- The difference between blocking and non-blocking comments
- How to give feedback that helps rather than frustrates
- What to look for when reviewing Python code specifically
- The "six-hat" framework for balanced reviews
- How to respond to review feedback professionally
- How to ask for reviews that get good feedback

## Prerequisites

This builds on all previous content. You should understand:
- Git and pull requests (folder 53)
- Your project's codebase (the guide content from 00–51)

## The Human Side of Code Review

Code review is one of the most important quality practices in software development, but it's also one of the most emotionally charged. You're asking someone to judge your work, and they're asking you to change code they wrote.

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                      THE CODE REVIEW DYNAMIC                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│    AUTHOR                           REVIEWER                               │
│    ──────                           ────────                               │
│                                                                             │
│    "I worked hard on this"      vs.  "This could be better"                │
│                                                                             │
│    "They don't understand        vs.  "This isn't the best                │
│     my design"                        approach"                             │
│                                                                             │
│    "Why are they being          vs.  "This is a great                     │
│     so nitpicky?"                     opportunity to learn"                │
│                                                                             │
│    ─────────────────────────────────────────────────────────────────────    │
│                                                                             │
│    The best code reviews happen when BOTH parties focus on                  │
│    the GOAL: building the best product together.                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

The key insight: code review isn't about your code or your ego—it's about the product. When you review, you're helping the team. When you receive reviews, someone is helping you.

## The Six-Hat Framework for Reviewers

Think of your review comment as wearing one of six hats. Each hat represents a different type of feedback:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        SIX HATS OF CODE REVIEW                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  🟢 HAT 1: CORRECTNESS                                                     │
│  "This function will crash if x is None"                                   │
│  → Bugs, logic errors, security vulnerabilities                            │
│                                                                             │
│  🔵 HAT 2: DESIGN                                                          │
│  "This should be a separate service, not in the router"                   │
│  → Architecture, separation of concerns                                   │
│                                                                             │
│  🟡 HAT 3: READABILITY                                                    │
│  "This variable name is confusing—call it user_id not temp"               │
│  → Clarity, maintainability                                               │
│                                                                             │
│  🟠 HAT 4: PERFORMANCE                                                     │
│  "This loop makes N database calls—use bulk_insert"                       │
│  → Efficiency, optimization                                                │
│                                                                             │
│  ⚪ HAT 5: TESTS                                                           │
│  "We should add a test for the edge case where x < 0"                    │
│  → Test coverage, edge cases                                              │
│                                                                             │
│  🟣 HAT 6: STYLE                                                           │
│  "This should use ruff—there's an unused import"                          │
│  → Formatting, linting, conventions                                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

Before you write a comment, ask yourself: "Which hat am I wearing?" This helps you:
- Be intentional about the type of feedback
- Separate blocking issues from nice-to-haves
- Not overwhelm the author with everything at once

## Writing Review Comments That Help

### The Best Review Comments

Great review comments explain:

1. **What** is wrong (be specific)
2. **Why** it's a problem (impact)
3. **How** to fix it (suggestion)

```python
# ❌ BAD COMMENT:
# This is wrong.

# ✅ GOOD COMMENT:
# This will raise a KeyError if 'email' is not in the payload.
# 
# Consider using:
# email = payload.get('email')
# if email is None:
#     raise ValueError("email is required")
```

🔍 **Why the Good Comment Works:**

1. **Specific** — Points to exact failure mode (KeyError)
2. **Contextual** — Explains when it fails
3. **Solution-oriented** — Shows actual code to fix it

### Blocking vs. Non-Blocking Comments

Use GitHub's review features to signal priority:

| Type | When to Use | GitHub Syntax |
|------|-------------|---------------|
| **Blocking** | Must fix before merge | Request changes |
| **Non-blocking** | Nice to have, optional | Comment only |
| **Question** | Seeking understanding | Comment only |
| **Praise** | Something done well | Comment only |

**Rule of thumb:** If the code works correctly and is maintainable, make it non-blocking. Don't block on style issues that can be auto-fixed.

```python
# BLOCKING EXAMPLE:
# Security issue: This SQL is vulnerable to injection.
# Must use parameterized queries.

# NON-BLOCKING EXAMPLE:
# Nit: This could use a dataclass instead of a dict.
# Not worth blocking the PR—can be addressed later.
```

### Comments That Teach

The best reviews make the whole team better:

```python
# ❌ MERE CORRECTION:
# This should be `await self.fetch()` because it's async.

# ✅ TEACHING:
# This function is async (defined with `async def`), so you need
# `await` when calling other async functions inside it.
# Without `await`, you'd get a coroutine object instead of the result.
# This is a common gotcha—see Python's async docs for more.
```

## What to Look for in Python Code

When reviewing Python specifically, focus on these areas:

### 1. Type Hints

```python
# ❌ MISSING TYPE HINTS:
def process_user(data):
    return User(data)

# ✅ PROPER TYPE HINTS:
def process_user(data: dict) -> User:
    return User(**data)
```

Look for:
- Missing type hints on function signatures
- Using `Any` when more specific types are possible
- Inconsistent return types

### 2. Async/Await

```python
# ❌ BLOCKING CALL IN ASYNC:
async def get_data():
    response = requests.get(url)  # BLOCKS the event loop!
    return response.json()

# ✅ CORRECT ASYNC:
async def get_data():
    async with httpx.AsyncClient() as client:
        response = await client.get(url)
        return response.json()
```

Look for:
- Using synchronous libraries (requests, synchronous database drivers) in async code
- Forgetting `await` on async functions
- Blocking operations that freeze the event loop

### 3. Error Handling

```python
# ❌ BARE EXCEPT:
try:
    do_something()
except:
    pass

# ✅ SPECIFIC EXCEPTIONS:
try:
    do_something()
except ValueError as e:
    logger.warning(f"Invalid value: {e}")
    raise
```

Look for:
- Bare `except:` clauses
- Swallowing exceptions without logging
- Losing the original exception context

### 4. SQL/ORM Patterns

```python
# ❌ N+1 QUERY PROBLEM:
users = session.query(User).all()
for user in users:
    print(user.profile.name)  # Each access = 1 query!

# ✅ EAGER LOADING:
users = session.query(User).options(selectinload(User.profile)).all()
for user in users:
    print(user.profile.name)  # 2 queries total
```

Look for:
- N+1 query patterns (looping and querying)
- Missing database indexes on foreign keys
- SQL injection vulnerabilities

## Receiving Feedback Professionally

Getting critical feedback on your code is hard. Here's how to handle it well:

### The Mental Shift

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                     SHIFT YOUR MINDSET                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  BEFORE:                              AFTER:                               │
│                                                                             │
│  "They think my code is bad"        →  "They're helping me improve"       │
│                                                                             │
│  "They don't understand            →  "I can explain my reasoning        │
│   my design"                            and they'll understand"            │
│                                                                             │
│  "Why are they being               →  "I can learn something              │
│   so nitpicky?"                        from their perspective"             │
│                                                                             │
│  "I'm a fraud, they're             →  "This is how we build              │
│   finding my weaknesses"                better software together"          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Practical Steps

1. **Read the entire review before responding.** Don't get defensive after the first comment.

2. **Ask clarifying questions.** If you don't understand, ask. Don't assume bad intent.

3. **Accept valid points.** If they're right, just say "good catch" and fix it.

4. **Push back politely on invalid points.** Explain your reasoning. You might be right.

5. **Thank them for reviews.** Every review is free help improving your code.

### Example Response

```
# When accepting feedback:
Good catch—I didn't consider that edge case. Fixed in abc123.

# When asking for clarification:
I'm not sure I follow. Can you point me to where this would cause a problem?

# When disagreeing:
I see your point, but I chose this approach because [reason]. 
What do you think about [alternative]?
```

## Asking for Good Reviews

You can get better reviews by making them easier for reviewers:

### Write Good PR Descriptions

```markdown
# ❌ BAD PR DESCRIPTION:
Fixed the bug.

# ✅ GOOD PR DESCRIPTION:

## What Changed
- Fixed crash when user uploads empty file
- Added validation to reject files over 10MB

## Why
- Users were confused by the crash
- Large files were causing memory issues

## How to Test
1. Upload an empty file → should show error message
2. Upload 11MB file → should show "file too large"
3. Upload 5MB file → should succeed
```

### Keep PRs Small

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PR SIZE GUIDELINES                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Under 400 lines:   Easy to review, gets fast feedback                     │
│  400–1000 lines:   Standard PR, may take longer                          │
│  1000+ lines:      Hard to review thoroughly—consider splitting          │
│                                                                             │
│  Rule of thumb: If it takes more than 20 minutes to review,               │
│  it's probably too big.                                                    │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Review Your Own Code First

Before requesting review:

1. Read through your changes one more time
2. Run the linter
3. Run the tests
4. Check that your PR description answers: what, why, how to test

## Real-World Application

### At a Startup

At a small company, code review is often the only quality gate. You might review code from people who outrank you and people who outrank you will review yours. The key is treating all reviews as peer review—even from the CEO.

At Meta, I saw engineers spend 30 minutes writing a review comment that explained not just what was wrong but *why* it was wrong and *how to fix it*. This investment paid off: the team learned together, and the same mistakes didn't repeat.

### In Open Source

In open source, you review code from people you'll never meet. Be extra kind:
- They may not speak English as a first language
- They may be learning
- They have no obligation to accept your feedback

Open source maintainers: the best ones say "thank you" even for bad PRs because someone took the time to try to help.

## Tools & Resources

| Tool | Purpose |
|------|---------|
| GitHub PR reviews | Native code review |
| GitLab MR reviews | Native code review |
| GitHub Actions | Auto-lint on PRs |
| Ruff | Fast Python linter |
| pre-commit | Run checks before commit |

**Key resources:**
- [Google Engineering Practices](https://google.github.io/eng-practices/) — Excellent code review guide
- [The Art of Code Review](https://blog.pragmaticengineer.com/art-of-code-review/) — Practical advice

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Making Everything a Blocking Comment

**Wrong:**
Commenting on every style issue and requesting changes.

**Why it fails:**
You exhaust yourself and strain relationships. Minor issues shouldn't block merges.

**Correct approach:**
Use blocking comments only for correctness, security, and serious design flaws. Let style be auto-fixed.

### ❌ Mistake 2: Vague Comments

**Wrong:**
"This seems wrong" or "Fix this."

**Why it fails:**
The author doesn't know what to fix or why it's wrong.

**Correct approach:**
Be specific: explain what, why, and how to fix.

### ❌ Mistake 3: Taking Feedback Personally

**Wrong:**
Responding defensively to every comment.

**Why it fails:**
You miss valid feedback and damage relationships.

**Correct approach:**
Assume positive intent. Accept valid points. Push back politely on invalid ones.

### ❌ Mistake 4: Not Reviewing Your Own Code

**Wrong:**
Submitting PRs without self-reviewing first.

**Why it fails:**
You waste reviewers' time on issues you could have caught.

**Correct approach:**
Always self-review before requesting others' time.

## Summary

- Use the six-hat framework to categorize feedback: correctness, design, readability, performance, tests, style
- Make comments specific: what, why, and how to fix
- Distinguish blocking from non-blocking issues
- When receiving feedback, assume positive intent and focus on the code, not the person
- Write good PR descriptions and keep PRs small to get better reviews

## Next Steps

→ `04-task-estimation.md` — How to estimate development work accurately and communicate timelines.
