# Task Estimation

## What You'll Learn

- Why developers are notoriously bad at estimation
- The psychology behind estimation errors
- T-shirt sizing vs story points vs hours—when to use each
- The planning fallacy and how to correct for it
- Breaking tasks into estimable pieces
- Communicating delays professionally

## Prerequisites

This builds on your experience from the guide. You should have:
- Worked on at least one multi-week project (any of folders 00–51 built something)
- Experience with project management tools (Jira, Linear, GitHub Projects)

## Why Estimation Is Hard

Developers consistently overestimate what they can complete in a week and underestimate what they can complete in a month. This isn't laziness—it's psychology:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHY DEVELOPERS ESTIMATE POORLY                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. OPTIMISM BIAS                                                          │
│     We assume everything will go perfectly.                                │
│     We don't account for:                                                  │
│       • Unexpected bugs                                                    │
│       • Meetings and interruptions                                         │
│       • Code review delays                                                │
│       • Environment issues                                                │
│                                                                             │
│  2. UNKNOWN UNKNOWNS                                                       │
│     New projects have hidden complexity:                                    │
│       • Legacy code that's hard to modify                                   │
│       • Dependencies you don't understand                                   │
│       • Edge cases you didn't consider                                     │
│                                                                             │
│  3. ANCHORING                                                              │
│     The first number in a discussion anchors the estimate:                 │
│     "How long will this take?" "Maybe 3 days?"                            │
│     Once someone says a number, others agree too easily.                   │
│                                                                             │
│  4. COMPETENCY CONFUSION                                                  │
│     Junior devs think it's easy (no idea what can go wrong)               │
│     Senior devs think it's easy (they've seen everything go wrong)         │
│     The truth is somewhere in between.                                      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Estimation Methods Compared

### T-Shirt Sizing

The simplest method: estimate in sizes, not time.

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                       T-SHIRT SIZING                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  XS  = Less than 2 hours                                                   │
│  S   = Half a day (2–4 hours)                                              │
│  M   = A day (4–8 hours)                                                   │
│  L   = 2–3 days                                                            │
│  XL  = A week                                                               │
│  XXL = More than a week (break this down!)                                 │
│                                                                             │
│  PROS: Fast, no false precision                                            │
│  CONS: Hard to use for sprint planning                                      │
│                                                                             │
│  BEST FOR: Backlog prioritization, rough sorting                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

Use T-shirt sizing for:
- Backlog grooming (sorting which items are bigger)
- Initial project estimation (before detailed planning)
- When you have many items to estimate quickly

### Story Points

Story points measure relative effort, not time:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        STORY POINTS                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Instead of: "This will take 8 hours"                                     │
│  Say: "This is a 5-pointer"                                                │
│                                                                             │
│  The Fibonacci sequence is common: 1, 2, 3, 5, 8, 13, 21                  │
│  (Larger gaps as complexity increases)                                      │
│                                                                             │
│  PROS: Teams calibrate over time, accounts for complexity                │
│  CONS: Requires a mature team with velocity tracking                       │
│                                                                             │
│  BEST FOR: Scrum sprints, teams with consistent velocity                    │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

To use story points effectively:
1. Pick a "baseline" story everyone agrees on (e.g., "simple endpoint" = 2 points)
2. Compare other stories to the baseline
3. Track velocity (points completed per sprint)
4. Use velocity to predict future sprints

### Hours

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           HOURS                                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  PROS: Intuitive, stakeholders understand it                               │
│  CONS: Creates false confidence, hard to compare                           │
│                                                                             │
│  BEST FOR: Contracts, fixed-price work, contractor billing                │
│                                                                             │
│  ⚠️  WARNING: Only experienced developers should estimate in hours,        │
│      and even then, add 30–50% buffer.                                    │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Comparison Table

| Method | Best For | Worst For | Learning Curve |
|--------|----------|-----------|----------------|
| T-shirt sizes | Backlog sorting | Sprint planning | None |
| Story points | Agile sprints | Contracts | Medium |
| Hours | Fixed-price work | Complex projects | Low |
| Ideal days | Personal planning | Team coordination | Low |

## Breaking Down Unestimable Tasks

The key to good estimation: break work until each piece is small enough to estimate accurately.

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    BREAKING DOWN TASKS                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ❌ TOO BIG:                                                               │
│  "Implement user authentication"                                           │
│                                                                             │
│  ✅ BREAK DOWN INTO:                                                       │
│  1. "Create user model and migration"          → 2 hours                  │
│  2. "Add password hashing utilities"           → 2 hours                  │
│  3. "Create registration endpoint"             → 3 hours                  │
│  4. "Create login endpoint with JWT"           → 4 hours                  │
│  5. "Add password reset flow"                  → 4 hours                  │
│  6. "Write authentication tests"               → 3 hours                  │
│                                                                             │
│  TOTAL: 18 hours → ~3 days (with buffer)                                  │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### The "Three-Point" Technique

For each task, estimate three numbers:
- **Optimistic** (everything goes perfectly)
- **Nominal** (most likely)
- **Pessimistic** (everything goes wrong)

Then use the average: `(O + 4N + P) / 6`

```python
# Example: "Add user profile endpoint"

optimistic = 2   # hours, if everything works first try
nominal = 4       # hours, with typical debugging
pessimistic = 8  # hours, with major refactor needed

expected = (optimistic + 4 * nominal + pessimistic) / 6
# (2 + 16 + 8) / 6 = 26 / 6 ≈ 4.3 hours
```

This mathematically accounts for uncertainty.

## The Planning Fallacy

The planning fallacy is our tendency to underestimate time even when we know about it:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                     THE PLANNING FALLACY                                    │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  You estimate:  3 days                                                      │
│  Actual time:   5 days                                                     │
│                                                                             │
│  Sound familiar?                                                           │
│                                                                             │
│  The trick: you've alreadypredicted you'd underestimate!                  │
│                                                                             │
│  ────────────────────────────────────────────────────────────────────────   │
│                                                                             │
│  SOLUTION: Add a buffer based on historical data                          │
│                                                                             │
│  Track your estimates vs. actuals for 2–3 weeks                           │
│  Calculate your "fudge factor": actual / estimated                        │
│  Apply to future estimates                                                 │
│                                                                             │
│  If you typically take 1.5x your estimate, multiply all                    │
│  future estimates by 1.5.                                                  │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Communicating Estimates

### The Stakeholder Conversation

When someone asks "how long will this take?":

```python
# ❌ WRONG: Giving a false sense of certainty
"This will take 3 days."

# ✅ RIGHT: Acknowledging uncertainty
"I'm estimating about 3 days based on what I know now. 
That's assuming no major unexpected issues. I'd plan for 4–5 days 
to be safe."
```

### When Estimates Change

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                  WHEN YOU'RE RUNNING LATE                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. DON'T PANIC                                                            │
│     Running late is normal. Don't hide it.                                 │
│                                                                             │
│  2. COMMUNICATE EARLY                                                       │
│     As soon as you know you're late, tell people.                          │
│     Don't wait until the original deadline.                                │
│                                                                             │
│  3. BE SPECIFIC                                                            │
│     "I'll be done Thursday" is better than "soon"                         │
│                                                                             │
│  4. EXPLAIN WHY                                                            │
│     "Found an edge case I didn't anticipate"                              │
│     "The API changed and I need to update our code"                       │
│                                                                             │
│  5. PROPOSE OPTIONS                                                        │
│     • Can I get help?                                                     │
│     • Can we reduce scope?                                                │
│     • What's the impact of being late?                                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Email Template for Delays

```
Subject: Update: [Feature Name] Timeline

Hi [Stakeholder],

I wanted to update you on [feature name]. 

Originally estimated: [date]
Updated estimate: [new date]

What's happening: [brief explanation—1 sentence]

Why: [one sentence on what you discovered]

Next steps: [what you're doing about it]

I'm available to discuss if you'd like.
```

## Real-World Application

### At a Startup

Startups live and die by estimates. You might tell the CEO "two weeks" for a feature, and they'll tell investors "end of month." When you're late, trust erodes.

The fix:
- Build track record of accuracy (even if conservative)
- Always add buffers publicly
- Communicate early when estimates slip

### At a Big Company

At Google or Meta, estimates matter less because there's always more work to do. The goal is predictability so managers can plan resources.

The fix:
- Use story points and track velocity
- Report percentage complete daily in sprint reviews
- Focus on "definition of done" rather than deadlines

### On Freelance Projects

If you're doing fixed-price work, accurate estimates directly affect your income.

The fix:
- Add 30–50% buffer to every estimate
- Only estimate in hours, never fixed price
- Scope creep kills estimates—document what's included explicitly

## Tools & Resources

| Tool | Purpose | Best For |
|------|---------|----------|
| Trello | Simple board | Small teams |
| Jira | Full project management | Enterprise |
| Linear | Fast issue tracking | Startups |
| GitHub Projects | Lightweight | Open source |

**Key resources:**
- [Planning Poker](https://planningpoker.com/) — Team estimation tool
- [Evidence-Based Scheduling](https://www.martinfowler.com/articles/evidence-based-scheduling.html) — Joel Spolsky's method

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Estimating Without Breaking Down Tasks

**Wrong:**
"Add user authentication" → 3 days

**Why it fails:**
"Add user authentication" contains dozens of hidden tasks. You won't see them until you're in the middle.

**Correct approach:**
Break into individual tasks, estimate each, sum them.

### ❌ Mistake 2: Not Adding Buffer

**Wrong:**
"2 days" for a task that might take 3

**Why it fails:**
You always underestimate something. Without buffer, you're always late.

**Correct approach:**
Multiply by 1.3–1.5, or use three-point estimation.

### ❌ Mistake 3: Not Communicating Delays

**Wrong:**
Hoping you'll catch up, not telling anyone

**Why it fails:**
Stakeholders make plans based on your estimates. Late surprises are worse than early warnings.

**Correct approach:**
Communicate immediately when you know you'll be late.

### ❌ Mistake 4: Confusing Effort with Duration

**Wrong:**
"4 hours of work" = "done by 5pm today"

**Why it fails:**
You have meetings, interrupts, code review. Work hours ≠ calendar hours.

**Correct approach:**
If a task is 4 hours of work and you have 2 hours of meetings, it takes 2 days.

## Summary

- Estimation is hard because of optimism bias and unknown unknowns
- Use T-shirt sizes for backlog sorting, story points for sprints, hours for contracts
- Break tasks until each piece is under 4 hours
- Track your actual vs. estimated time to find your personal "fudge factor"
- Communicate uncertainty in estimates and communicate delays immediately

## Next Steps

→ `05-commit-messages-and-pull-requests.md` — How to write commit messages and PRs that make your team more effective.
