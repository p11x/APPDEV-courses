# Asking Good Technical Questions

## What You'll Learn

- The XY problem: asking about your solution instead of your problem
- How to write questions that get answered fast
- The Stack Overflow question template
- How to ask senior engineers for help without wasting their time
- How to use AI tools effectively for debugging

## Prerequisites

This is about communication skills—no specific technical prerequisites.

## The XY Problem

The most common mistake in asking technical questions is the XY problem:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         THE XY PROBLEM                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  X = what you want to do                                                   │
│  Y = how you're trying to do it                                            │
│                                                                             │
│  ❌ BAD QUESTION:                                                          │
│  "How do I convert a list to a dict?"                                     │
│                                                                             │
│  This asks about Y (the attempted solution).                               │
│                                                                             │
│  ✅ GOOD QUESTION:                                                         │
│  "I have a list of users and want to look them up by ID.                  │
│   What's the best way to store them for fast lookup?"                     │
│                                                                             │
│  This asks about X (the actual goal).                                     │
│                                                                             │
│  Why this matters:                                                         │
│  - There might be a better way to achieve X                              │
│  - Y might be the wrong approach entirely                                  │
│  - You'll learn more by explaining the goal                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### How XY Happens

```python
# ❌ BAD: Asking about your attempted solution
"I'm trying to use a list comprehension but can't figure out
how to filter by index. Can you show me?"

# This assumes a list comprehension is the right approach.
# Maybe it isn't!

# ✅ GOOD: Asking about your actual goal
"I have 10,000 user records and need to filter them by 
whether they've logged in recently. Should I use a list 
and filter, or is there a better approach?"

# This lets someone suggest the BEST solution for your actual problem.
```

## The Perfect Technical Question

A good technical question follows this structure:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    STRUCTURE OF A GOOD QUESTION                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. WHAT ARE YOU TRYING TO DO?                                            │
│     One sentence describing your goal.                                      │
│                                                                             │
│  2. WHAT HAVE YOU TRIED?                                                  │
│     Show the code that didn't work. Include relevant code.                │
│                                                                             │
│  3. WHAT HAPPENED?                                                        │
│     Include the exact error message or unexpected behavior.               │
│                                                                             │
│  4. WHAT DID YOU EXPECT?                                                  │
│     What should have happened instead?                                    │
│                                                                             │
│  5. WHAT ENVIRONMENT?                                                     │
│     OS, Python version, library versions.                                  │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Template for Technical Questions

### For Slack/Discord/Teams

```markdown
**What I'm trying to do:**
[One sentence: build X to achieve Y]

**What I've tried:**
```
python
# Your relevant code here
```

**What happened:**
[Exact error or unexpected behavior]

**What I expected:**
[What should have happened]

**Environment:**
- OS: 
- Python version:
- Library versions:
```

### For Stack Overflow

```markdown
## Summary
[One sentence: what are you trying to do]

## Background
[2-3 sentences: provide context, explain why this matters]

## What I've tried
```
python
# Your minimal, reproducible example
```

## The problem
[Exact error message, or "it doesn't do X like I expected"]

## Expected behavior
[What should happen]

## Environment
- Python 3.11+
- FastAPI 0.109
- PostgreSQL 16
```

## Asking Senior Engineers

When asking someone for help, be respectful of their time:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    ASKING SENIOR ENGINEERS FOR HELP                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  BEFORE YOU ASK:                                                          │
│  ✅ Try for 30–60 minutes on your own                                     │
│  ✅ Search the codebase for similar patterns                               │
│  ✅ Read the documentation                                                 │
│  ✅ Try rubber duck debugging                                               │
│                                                                             │
│  WHEN YOU ASK:                                                            │
│  ✅ Be specific about where you're stuck                                   │
│  ✅ Show what you've tried                                                 │
│  ✅ Ask a specific question ("should I use X or Y?")                       │
│  ✅ Give context: "I'm working on the auth refactor"                      │
│                                                                             │
│  DON'T:                                                                   │
│  ❌ "Can you help me?" (no context)                                       │
│  ❌ "I don't understand this" (no specific question)                     │
│  ❌ Send a message and wait for answer (follow up in 30 min)              │
│                                                                             │
│  KEY: Make it EASY for them to help you.                                  │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Example: Good Ask

```
Hey, I'm working on the user profile endpoint. I'm trying to add 
avatar upload but hitting a 413 error when files are over 1MB.

I've tried:
- Increasing FastAPI's request limit (didn't help)
- The file is definitely under the limit

The error says "Request too large." Any ideas?
```

This is good because:
- Shows effort (tried increasing limit)
- Has specific context (user profile, avatar upload)
- Includes the actual error
- Asks for ideas, not "fix it for me"

## Using AI Tools Effectively

AI (like ChatGPT, Claude, GitHub Copilot) can help with debugging—but you need to use it well:

### How to Get Good Results

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    USING AI FOR DEBUGGING                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ DO:                                                                   │
│  • Include the full error message                                          │
│  • Show relevant code (not entire file)                                   │
│  • Explain what you've already tried                                       │
│  • Ask specific questions                                                  │
│                                                                             │
│  ❌ DON'T:                                                                │
│  • Paste entire codebase and say "fix it"                                 │
│  • Ask vague questions                                                    │
│  • Trust the answer blindly                                               │
│                                                                             │
│  ⚠️  WARNING: AI can confidently give WRONG answers.                    │
│  Always verify suggestions before applying them.                          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Example: Effective AI Query

```
I'm getting this error with FastAPI:

KeyError: 'email'
Traceback:
  File "app.py", line 15, in <module>
    user = User(email=data['email'])
    
The data comes from a Pydantic model. Here's the relevant code:

class UserCreate(BaseModel):
    email: EmailStr
    
What might cause this KeyError when the field is defined in Pydantic?
```

This is effective because:
- Shows the exact error
- Includes relevant code
- Asks a specific question

### Example: Ineffective AI Query

```
My FastAPI app isn't working. Can you fix it?
```

This is ineffective because:
- No error message
- No code
- Too vague to help

## Questions to Ask Before Posting Anywhere

Before you ask, check:

1. **Did I read the error?** (Often the answer is in the traceback)
2. **Did I search?** (Someone else has probably had this issue)
3. **Did I try the obvious?** (Restart server, clear cache, etc.)
4. **Can I make a minimal reproduction?**

If you've done all these and you're still stuck—that's when you ask.

## Real-World Application

### On Stack Overflow

Stack Overflow has a strict culture. To get good answers:
- Write a title that summarizes the specific problem
- Include a minimal reproduction
- Tag appropriately
- Respond to comments

Good questions get answered in minutes. Bad questions get downvoted and closed.

### In Company Slack

At work, your question might be answered faster if you:
- Post in the right channel
- Use threads
- Wait for people to be available (not at 5pm Friday)
- Say "I'll keep looking but wanted to ask in case you know"

### On Discord/Reddit

Communities vary. Read the room:
- Some servers have question channels
- Be nice to newcomers
- Don't ask to ask—just ask

## Tools & Resources

| Resource | Purpose |
|----------|---------|
| Stack Overflow | Q&A database |
| Discord servers | Community support |
| Reddit r/python | General help |
| GitHub Issues | Library-specific bugs |

**Key resources:**
- [How to Ask Questions The Smart Way](http://catb.org/~esr/faqs/smart-questions.html) — Eric Raymond's classic guide
- [XY Problem](https://xyproblem.info/) — Explained in detail

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Asking Before Searching

**Wrong:**
Posting a question without searching first.

**Why it fails:**
Shows laziness. The answer is probably already online.

**Correct:**
Search first. Then ask if you can't find the answer.

### ❌ Mistake 2: Not Including Code

**Wrong:**
"FastAPI isn't working. Help!"

**Why it fails:**
No one can help without seeing code.

**Correct:**
Include minimal, relevant code.

### ❌ Mistake 3: Not Including the Error

**Wrong:**
"My function doesn't work. Can you tell me why?"

**Why it fails:**
"The code doesn't work" could mean anything.

**Correct:**
Include the exact error message.

### ❌ Mistake 4: Asking Vague Questions

**Wrong:**
"What's the best way to do auth in FastAPI?"

**Why it fails:**
Too broad. "Best" depends on context.

**Correct:**
Ask about your specific situation: "For a simple app with 100 users, should I use sessions or JWT?"

## Summary

- Avoid the XY problem: explain your goal, not just your attempted solution
- Structure questions with: what you're trying to do, what you tried, what happened, what you expected
- Show you've put in effort before asking
- Be specific and include code
- AI tools help but verify answers before applying

## Next Steps

→ `08-imposter-syndrome.md` — Managing the feeling that you don't belong and building confidence as a developer.
