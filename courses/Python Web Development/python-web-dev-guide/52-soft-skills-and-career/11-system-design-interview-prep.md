# System Design Interview Prep

## What You'll Learn

- The framework for solving any system design question
- How to design common backend systems
- Key concepts: scalability, databases, caching, load balancing
- Drawing architecture diagrams
- Handling trade-offs

## Prerequisites

- Completed Python backend interview prep
- Understanding of databases, APIs, and basic architecture

## The Four-Step Framework

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SYSTEM DESIGN FRAMEWORK                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  STEP 1: REQUIREMENTS CLARIFICATION (2-3 min)                              │
│  • Ask questions about scope                                                │
│  • Define what's IN and OUT of scope                                       │
│  • Note scale expectations                                                  │
│                                                                             │
│  STEP 2: HIGH-LEVEL DESIGN (5-10 min)                                      │
│  • Draw main components                                                     │
│  • Define data flow                                                         │
│  • Identify key services                                                    │
│                                                                             │
│  STEP 3: DEEP DIVE (10-15 min)                                            │
│  • Choose databases, discuss trade-offs                                      │
│  • Handle specific concerns (caching, auth, etc.)                         │
│  • Address edge cases                                                       │
│                                                                             │
│  STEP 4: WRAP UP (2-3 min)                                                 │
│  • Summess design                                                           │
│  • Mention monitoring, scaling                                             │
│  • What would you change at 10x scale?                                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Common System Design Questions

### 1. Design a URL Shortener

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    URL SHORTENER DESIGN                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Requirements Clarification:                                                │
│  • How many URLs? (Start: 1M, Scale: 1B)                                  │
│  • Read-heavy or write-heavy?                                              │
│  • Custom aliases?                                                          │
│  • Analytics needed?                                                       │
│                                                                             │
│  High-Level:                                                                │
│                                                                             │
│    ┌──────────┐      ┌──────────────┐      ┌─────────────┐                │
│    │  Client  │ ───▶ │  API Server  │ ───▶ │  Database   │                │
│    └──────────┘      └──────────────┘      └─────────────┘                │
│                            │                                                │
│                            ▼                                                │
│                       ┌──────────┐                                         │
│                       │ Cache    │                                         │
│                       └──────────┘                                         │
│                                                                             │
│  Key Decisions:                                                              │
│  • Short code: Base62 (a-z, A-Z, 0-9) = 62^6 = 56B possibilities          │
│  • Database: Hash-based partitioning                                        │
│  • Cache: Popular URLs (Zipf's law applies)                               │
│                                                                             │
│  Database Schema:                                                           │
│  ┌───────────────┬────────────────────┬─────────────┐                     │
│  │ id (PK)       │ short_code (index) │ long_url    │                     │
│  │ 1             │ abc123             │ https://... │                     │
│  └───────────────┴────────────────────┴─────────────┘                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### 2. Design a Twitter Clone

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    TWITTER CLONE DESIGN                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Core Features:                                                             │
│  • Tweet (140 chars + media)                                               │
│  • Follow users                                                             │
│  • Feed (timeline)                                                          │
│                                                                             │
│  Key Challenge: Feed Generation                                             │
│                                                                             │
│  Approach 1: Pull (Fan-out on read)                                        │
│  ┌────────────┐     ┌────────────┐     ┌────────────┐                     │
│  │ User A     │     │ Follows    │     │ Gets tweets│                     │
│  │ requests   │ ──▶ │ B, C, D    │ ──▶ │ from all   │                     │
│  │ feed       │     │            │     │ at query   │                     │
│  └────────────┘     └────────────┘     └────────────┘                     │
│  ✓ Simple, ✓ Real-time reads                                              │
│  ✗ Slow for users with many follows                                       │
│                                                                             │
│  Approach 2: Push (Fan-out on write)                                       │
│  ┌────────────┐     ┌────────────┐     ┌────────────┐                     │
│  │ User B     │     │ Tweet goes │     │ Stored in  │                     │
│  │ tweets     │ ──▶ │ to all     │ ──▶ │ followers'│                     │
│  │            │     │ followers' │     │ feed cache│                     │
│  └────────────┘     │ timelines  │     └────────────┘                     │
│                     └────────────┘                                        │
│  ✓ Fast reads, ✓ Good for many follows                                    │
│  ✗ Complex, ✗ Delayed for celebrity tweets                                 │
│                                                                             │
│  Hybrid: Push for normal users, pull for celebs                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### 3. Design a Rate Limiter

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    RATE LIMITER DESIGN                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Use Cases:                                                                 │
│  • Prevent abuse                                                             │
│  • Protect backend                                                          │
│  • API monetization                                                          │
│                                                                             │
│  Algorithm: Token Bucket                                                    │
│                                                                             │
│    ┌─────────────┐                                                          │
│    │  Bucket     │                                                          │
│    │  ┌───────┐  │  ◄── Tokens added at rate r                            │
│    │  │ ○ ○ ○ │  │                                                          │
│    │  │ ○ ○   │  │  ◄── Each request consumes 1 token                    │
│    │  └───────┘  │                                                          │
│    └─────────────┘                                                          │
│                                                                             │
│  Implementation (Redis + Lua):                                             │
│                                                                             │
│  ```lua                                                                     │
│  local key = KEYS[1]                                                        │
│  local limit = tonumber(ARGV[1])                                           │
│  local window = tonumber(ARGV[2])                                          │
│                                                                             │
│  local current = redis.call('GET', key)                                    │
│  if current and tonumber(current) >= limit then                            │
│      return 0                                                               │
│  end                                                                        │
│                                                                             │
│  local new_val = redis.call('INCR', key)                                   │
│  if new_val == 1 then                                                       │
│      redis.call('EXPIRE', key, window)                                     │
│  end                                                                        │
│                                                                             │
│  return new_val                                                             │
│  ```                                                                        │
│                                                                             │
│  Where to put it:                                                           │
│  • API Gateway (recommended)                                               │
│  • Middleware in API server                                                 │
│  • Redis directly (not recommended)                                         │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### 4. Design a Chat System

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CHAT SYSTEM DESIGN                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Requirements:                                                               │
│  • 1-on-1 messaging                                                         │
│  • Group chat                                                               │
│  • Online presence                                                          │
│  • Message history                                                          │
│                                                                             │
│  WebSocket Connection:                                                      │
│                                                                             │
│    Client A ◀───── WebSocket ─────▶ Server                                 │
│                                              │                              │
│                                              ▼                              │
│                                         ┌─────────┐                        │
│                                         │ Message │                        │
│                                         │  Queue  │                        │
│                                         └─────────┘                        │
│                                              │                              │
│                                              ▼                              │
│    Client B ◀───── WebSocket ─────▶ Server                                 │
│                                                                             │
│  Key Decisions:                                                             │
│  • Message storage: Append-only log + indexed table                        │
│  • Presence: Heartbeat every 30s, expire after 60s                        │
│  • Ordering: Lamport timestamp for distributed ordering                   │
│                                                                             │
│  Database Schema:                                                           │
│  ┌─────────────┬──────────────┬─────────────┬─────────────┐              │
│  │ message_id │ conversation │ sender_id   │ content     │              │
│  │            │ _id          │             │             │              │
│  │ UUID       │ UUID         │ UUID        │ TEXT        │              │
│  └─────────────┴──────────────┴─────────────┴─────────────┘              │
│                                                                             │
│  Indexes:                                                                   │
│  • (conversation_id, created_at)                                           │
│  • (sender_id, created_at)                                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Key Concepts

### Database Selection

| Use Case | Recommended |
|----------|-------------|
| Relational data | PostgreSQL |
| Simple key-value | Redis |
| Document storage | MongoDB |
| Time series | TimescaleDB |
| Search | Elasticsearch |
| Caching | Redis |
| Blob storage | S3 |

### Caching Strategies

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CACHING STRATEGIES                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Cache-Aside (Read):                                                       │
│  1. Check cache                                                             │
│  2. If miss, query DB                                                       │
│  3. Store in cache                                                          │
│  4. Return data                                                             │
│                                                                             │
│  Write-Through:                                                            │
│  1. Write to DB                                                             │
│  2. Write to cache                                                          │
│  3. Return                                                                  │
│                                                                             │
│  Write-Behind:                                                              │
│  1. Write to cache                                                          │
│  2. Async write to DB                                                       │
│                                                                             │
│  When to use each:                                                         │
│  • Cache-Aside: Read-heavy (most common)                                  │
│  • Write-Through: Write-heavy, need consistency                            │
│  • Write-Behind: Write-heavy, can tolerate eventual consistency           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Load Balancing

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    LOAD BALANCING ALGORITHMS                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Round Robin:                                                              │
│  Request 1 → Server A                                                      │
│  Request 2 → Server B                                                      │
│  Request 3 → Server C                                                      │
│  Request 4 → Server A                                                      │
│                                                                             │
│  Least Connections:                                                        │
│  Route to server with fewest active connections                           │
│                                                                             │
│  IP Hash:                                                                  │
│  Hash(client IP) → Consistent server                                       │
│  Useful for session affinity                                               │
│                                                                             │
│  Weighted:                                                                 │
│  Assign weights based on server capacity                                  │
│                                                                             │
│  Tools:                                                                    │
│  • HAProxy                                                                  │
│  • NGINX                                                                    │
│  • AWS ALB                                                                 │
│  • Cloudflare                                                              │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Trade-offs to Know

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    COMMON TRADE-OFFS                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Consistency vs Availability (CAP Theorem)                                 │
│  ┌─────────────────────────────────────────────────────────────────┐       │
│  │   You can only have 2 of 3:                                    │       │
│  │                                                                 │       │
│  │         Consistency    ←→    Availability                      │       │
│  │              ↑                  ↑                               │       │
│  │              │                  │                               │       │
│  │              └────────┬─────────┘                               │       │
│  │                       │                                          │       │
│  │                  Partition Tolerance                            │       │
│  │                  (always required)                               │       │
│  └─────────────────────────────────────────────────────────────────┘       │
│                                                                             │
│  Other Trade-offs:                                                         │
│  • SQL vs NoSQL: Consistency vs Scalability                               │
│  • Sync vs Async: Simplicity vs Performance                               │
│  • Monolith vs Microservices: Complexity vs Flexibility                   │
│  • Centralized vs Distributed DB: Simple vs Scalable                      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Scaling Concepts

### Horizontal vs Vertical

| Approach | Description | Pros | Cons |
|----------|-------------|------|------|
| Vertical | Bigger server | Simple | Hardware limits |
| Horizontal | More servers | Unlimited scale | Complex |

### Database Scaling

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DATABASE SCALING                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Read Replicas:                                                            │
│  ┌─────────┐     ┌─────────┐     ┌─────────┐                             │
│  │ Master  │ ──▶ │ Replica │ ──▶ │ Replica │                            │
│  │  (W)    │     │   (R)   │     │   (R)   │                            │
│  └─────────┘     └─────────┘     └─────────┘                             │
│                                                                             │
│  Sharding:                                                                 │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐                       │
│  │ Shard 1     │  │ Shard 2     │  │ Shard 3     │                       │
│  │ Users A-G   │  │ Users H-P   │  │ Users Q-Z   │                       │
│  └─────────────┘  └─────────────┘  └─────────────┘                       │
│                                                                             │
│  Sharding Key: user_id                                                     │
│                                                                             │
│  Problem: Cross-shard queries                                              │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## What Interviewers Want

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHAT GETS YOU HIRED                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ ASKS CLARIFYING QUESTIONS                                              │
│  • "How many users?"                                                       │
│  • "What's the read/write ratio?"                                         │
│  • "Any latency requirements?"                                             │
│                                                                             │
│  ✅ THINKS ABOUT TRADE-OFFS                                                │
│  • "We could use X, but Y would be better for..."                         │
│  • "This approach has a downside of..."                                   │
│                                                                             │
│  ✅ KNOWS THE BASICS                                                        │
│  • Can draw and explain architecture                                      │
│  • Understands databases, caching, load balancing                         │
│                                                                             │
│  ✅ HANDLES SCALING                                                         │
│  • Can discuss 10x, 100x scale                                             │
│  • Knows when to optimize                                                  │
│                                                                             │
│  ❌ JUMPS INTO CODE WITHOUT DISCUSSION                                      │
│  ❌ IGNORES REQUIREMENTS                                                    │
│  ❌ CAN'T EXPLAIN TRADE-OFFS                                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- Use the four-step framework: Clarify → High-level → Deep dive → Wrap up
- Know common designs: URL shortener, Twitter, Chat, Rate limiter
- Understand trade-offs (CAP, consistency models)
- Know when to use which database and caching strategy
- Always ask clarifying questions first

## Next Steps

→ `12-contributing-to-open-source.md` — How to contribute to open source and build community presence.
