# Writing Technical Documentation

## What You'll Learn

- The four types of technical documentation and when to use each
- How to write READMEs that developers actually read
- The anatomy of an effective Architecture Decision Record (ADR)
- How the Divio documentation system organizes knowledge
- How to write API documentation that stays up-to-date
- Documentation as code: treating docs with the same rigor as software

## Prerequisites

This guide builds on everything in folders 00–51. You should understand:
- RESTful API design (folder 04)
- Project structure and architecture (folder 51)
- Version control with Git (folder 53)

## The Four Types of Documentation

Every piece of technical documentation falls into one of four categories. Understanding this classification helps you write the right thing:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        DOCUMENTATION TAXONOMY                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  TUTORIALS                                                        │   │
│  │  "Learning-oriented"                                              │   │
│  │  → "Learn X by building Y"                                       │   │
│  │  → Step-by-step, assumes no prior knowledge                       │   │
│  │  → This guide (folders 00–51) is a tutorial series              │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  HOW-TO GUIDES                                                    │   │
│  │  "Task-oriented"                                                  │   │
│  │  → "Accomplish a specific goal"                                  │   │
│  │  → Assumes some knowledge, focuses on getting things done         │   │
│  │  → "How to deploy to production with Docker"                     │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  REFERENCE                                                       │   │
│  │  "Information-oriented"                                           │   │
│  │  → "Complete information about a tool or API"                    │   │
│  │  → Comprehensive, accurate, no opinions                           │   │
│  │  → API docs, language references, library documentation          │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  EXPLANATION                                                      │   │
│  │  "Understanding-oriented"                                         │   │
│  │  → "Deep dive into a concept"                                     │   │
│  │  → Explains why, provides context and background                 │   │
│  │  → "Why we chose PostgreSQL over MongoDB"                        │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

The most common mistake developers make is writing tutorials when they need reference docs, or explanation when they need a how-to guide. Before you start writing, ask yourself: "What is the reader trying to accomplish?"

## The Divio Documentation System

The Divio system (from the company behind Django) provides a mental framework for organizing documentation. It maps directly to the four types above:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                         DIVIO DOCUMENTATION SYSTEM                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│   Tutorial                    How-to Guide                                  │
│   ├─ Learn by doing          ├─ Solve a problem                           │
│   ├─ Step-by-step            ├─ Prerequisite knowledge assumed            │
│   ├─ No prior knowledge      ├─ Practical focus                           │
│   └─ Safe environment        └─ Alternative solutions allowed             │
│                                                                             │
│   Reference                   Explanation                                  │
│   ├─ Complete information    ├─ Deep understanding                        │
│   ├─ No opinions             ├─ Context and background                   │
│   ├─ Searchable              ├─ Analysis and rationale                   │
│   └─ Machine-readable        └─ Connects concepts                         │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

Your project needs all four types. A common anti-pattern is having only tutorials (like this guide) but no reference documentation (API docs) or explanation (architecture decisions).

## Writing Effective READMEs

The README is your project's first impression. A good README answers three questions in under 60 seconds:

1. **What does this project do?** (one sentence)
2. **How do I get it running?** (three steps max)
3. **Where do I go next?** (link to docs)

### Before and After: A README Transformation

Here's a README that developers will close immediately:

```markdown
# My API Project

This is a REST API built with FastAPI. It has authentication
and database integration. There are various endpoints for
users and items.

To run it:
1. Install dependencies
2. Set up the database
3. Run the server

For more info check the docs folder.

Contributions welcome!
```

🔍 **Line-by-Line Breakdown:**

1. `# My API Project` — Generic title tells nothing
2. `This is a REST API built with FastAPI` — Vague; "REST API" describes half the web
3. `It has authentication and database integration` — Says "has" without explaining why
4. `various endpoints for users and items` — Could be literally anything
5. The install instructions are so vague you'd need another doc to understand them
6. `Contributions welcome!` — Meaningless without a contribution guide

Now here's a README that respects the reader's time:

```markdown
# PyMind — AI Knowledge Assistant

A production-ready FastAPI application that lets users upload
PDFs, DOCX, and text files, then chat with an AI that answers
questions using only the uploaded documents (RAG architecture).

## Quick Start

```bash
# 1. Clone and enter directory
git clone github.com/yourorg/pymind && cd pymind

# 2. Start infrastructure
docker compose up -d postgres redis

# 3. Run the application
pip install -e .
uvicorn app.main:app --reload
```

Open http://localhost:8000/docs to explore the API.

## Features

| Feature | Description |
|---------|-------------|
| Document Upload | PDF, DOCX, TXT with automatic text extraction |
| Semantic Search | pgvector-powered similarity search over your documents |
| RAG Chat | Context-aware AI responses with source citations |
| JWT Auth | Secure registration and session management |

## Architecture

```
┌──────────┐     ┌──────────┐     ┌──────────┐
│  Client  │────▶│ FastAPI  │────▶│PostgreSQL│
└──────────┘     └────┬─────┘     └──────────┘
                      │
                      │            ┌──────────┐
                      └───────────▶│  Redis  │
                                   └──────────┘
```

## Documentation

- [API Reference](http://localhost:8000/docs) — Interactive API explorer
- [Deployment Guide](docs/deployment.md) — Production deployment
- [Architecture Decision Records](docs/adr/) — Why we built things this way

## Tech Stack

| Component | Technology |
|-----------|------------|
| API Framework | FastAPI 0.109+ |
| Database | PostgreSQL 16 + pgvector |
| Cache/Sessions | Redis 7 |
| Authentication | JWT (python-jose) |
| ORM | SQLAlchemy 2.0 async |
| Testing | pytest + pytest-asyncio |

## License

MIT — see LICENSE file
```

🔍 **Line-by-Line Breakdown:**

1. **Project name + one-line description** — Immediately answers "what"
2. **Quick Start section** — Three commands, works copy-paste
3. **Features table** — Scannable list of capabilities
4. **Architecture diagram** — ASCII art shows the system at a glance
5. **Links to deeper documentation** — Answers "where next"
6. **Tech Stack table** — Precise versions, not generic names

The difference? The first README describes what the author did. The second README serves the reader's needs.

## Architecture Decision Records (ADRs)

An ADR documents a significant architectural decision: what was decided, why, and what alternatives were considered. Teams that don't write ADRs repeat the same discussions every six months.

### ADR Template

```markdown
# ADR-001: Use PostgreSQL with pgvector for document embeddings

## Status
Accepted

## Context
We need to store document embeddings for semantic search. The options are:
- PostgreSQL with pgvector extension
- Pinecone (managed vector DB)
- Weaviate (self-hosted or managed)
- Chroma (local-first)

## Decision
We will use PostgreSQL with the pgvector extension.

## Consequences

### Positive
- Single database for documents and embeddings
- pgvector is production-tested (pgvector/pgvector repository has 15k+ stars)
- No additional managed service costs
- Full SQL query capabilities for metadata filtering

### Negative
- Approximate nearest neighbor (ANN) index not as optimized as specialized vector DBs
- Vertical scaling limits (horizontal scaling requires more setup)

## Alternatives Considered

### Pinecone
- Pro: Managed, excellent ANN performance at scale
- Con: Additional cost, data leaves our infrastructure, complex migration if we leave

### Weaviate
- Pro: GraphQL API, excellent performance
- Con: Another service to operate, more complex than pgvector

## Review Date
2025-03-18 (or when scaling issues emerge)
```

🔍 **Why This ADR Works:**

1. **Status is explicit** — "Accepted" means this is current policy, not a proposal
2. **Context explains the problem** — Without this, future developers won't understand why this matters
3. **Decision is specific** — "PostgreSQL with pgvector" is actionable
4. **Consequences are balanced** — Shows you thought through trade-offs, not just the happy path
5. **Alternatives are documented** — Prevents re-litigating the same decision
6. **Review date** — ADRs become stale; this creates accountability

### When to Write an ADR

Write an ADR when a decision:
- Affects multiple services or the entire architecture
- Would be expensive to reverse later
- Took significant discussion or research
- Introduces a new technology or dependency

Don't write an ADR for:
- Minor implementation details
- Bug fixes
- UI/UX choices (unless they affect architecture)

## API Documentation Best Practices

The best API documentation is generated from code and stays synchronized automatically. FastAPI does this better than any framework:

```python
from fastapi import FastAPI, Depends
from pydantic import BaseModel, EmailStr
from datetime import datetime

app: FastAPI = FastAPI(
    title="PyMind API",
    description="AI Knowledge Assistant with RAG",
    version="1.0.0",
    docs_url="/docs",
    redoc_url="/redoc",
)

class UserCreate(BaseModel):
    email: EmailStr
    username: str
    password: str

@app.post(
    "/users/",
    response_model=UserResponse,
    summary="Create a new user",
    description="Register a new user account. "
                "Returns the created user without the password.",
    tags=["Authentication"],
)
async def create_user(user: UserCreate) -> UserResponse:
    """
    Create a new user in the system.
    
    The password is hashed using bcrypt before storage.
    An verification email is sent asynchronously.
    """
    # Implementation here
    pass
```

🔍 **Line-by-Line Breakdown:**

1. `title`, `description`, `version` — These become the OpenAPI metadata
2. `docs_url="/docs"` — Where the Swagger UI will be available
3. `response_model` — Automatically documents the response schema
4. `summary` — Short title in the API docs
5. `description` — Longer explanation in the docs
6. `tags` — Groups endpoints in the UI
7. Docstring — Further documentation that appears in the UI

The key insight: FastAPI generates OpenAPI schema from your type hints. Your docs are never out of sync because they're generated from the same code that runs.

### What to Include in API Docs

For each endpoint, document:

| Field | Example | Required? |
|-------|---------|-----------|
| Summary | "Create a new user" | Yes |
| Description | What this endpoint does in 1–3 sentences | Yes |
| Request body | Full schema with field descriptions | If applicable |
| Response | Success response + common error codes | Yes |
| Authentication | What credentials are needed | If applicable |
| Example | curl command or JSON request | Recommended |

```markdown
## Create a new user

Register a new user account in the system.

### Request

```json
{
  "email": "alice@example.com",
  "username": "alice",
  "password": "securepassword123"
}
```

### Responses

| Status | Description |
|--------|-------------|
| 201 | User created successfully |
| 400 | Email already registered |
| 422 | Validation error |

### Example

```bash
curl -X POST https://api.example.com/users/ \
  -H "Content-Type: application/json" \
  -d '{"email":"alice@example.com","username":"alice","password":"securepassword123"}'
```
```

## Real-World Application

### At a Startup (1–10 Engineers)

Your first hire after the founding engineer will need to ship code in their first week. Without documentation, they'll either:

- Ask you 20 questions a day (slowing everyone down)
- Make wrong assumptions and introduce bugs
- Spend days reading code to understand simple things

The minimum documentation a startup needs:
1. README with quick start (one page)
2. API reference (auto-generated from code)
3. ADRs for major decisions (5–10 total)

### At a Mid-Size Company (50–200 Engineers)

As teams grow, documentation becomes coordination. Different teams work on the same codebase:

- Team A changes the authentication flow
- Team B breaks because they depended on the old behavior

Documentation that prevents this:
1. Architecture diagrams updated with every major change
2. ADRs with explicit backward compatibility statements
3. Runbooks for operational procedures

### Open Source Project

For an open source project, documentation IS the product:

- Your README is the first impression for contributors
- Your API docs determine whether library users succeed
- Your CONTRIBUTING guide determines whether PRs are high quality

FastAPI's documentation (fastapi.tiangolo.com) is cited as a major reason for its 70k+ GitHub stars. The docs are:
- Auto-generated from code
- Available in multiple languages
- Include interactive examples

## Tools & Resources

| Tool | Purpose | When to Use |
|------|---------|-------------|
| MkDocs | Static site generator | Hosting documentation |
| Material for MkDocs | Styled theme | Making docs beautiful |
| Swagger UI | Interactive API docs | Auto-generated from OpenAPI |
| ReDoc | Alternative API docs | Different visual style |
| pdoc | Python docstring generator | Library reference docs |
| MKDocs Material | Sphinx alternative | Simpler than Sphinx |
| Docusaurus | React-based docs | For more interactive docs |

**Key resources:**
- [Divio Documentation System](https://documentation.divio.com/) — Definitive guide to documentation types
- [Stripe API Docs](https://stripe.com/docs) — Best-in-class API documentation example
- [FastAPI Docs](https://fastapi.tiangolo.com/) — How auto-generated docs should look

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Writing Tutorials When You Need Reference Docs

**Wrong approach:**
Writing a step-by-step tutorial for every single feature. This creates massive documentation that takes forever to maintain and nobody reads cover-to-cover.

**Why it fails:**
Tutorials are for learning. Once someone knows your tool, they need reference docs to remind themselves of exact parameter names and response formats.

**Correct approach:**
Invest in auto-generated reference documentation (FastAPI provides this). Write tutorials for the top 3 most common tasks. Link to the reference for everything else.

### ❌ Mistake 2: Documentation That Drifts from Code

**Wrong approach:**
Manually writing API docs in a separate Markdown file that doesn't update when code changes.

```markdown
# User Endpoints

## Get User
GET /users/{id}
Returns user object
```

**Why it fails:**
The Markdown file has no connection to the code. When someone adds a new field to the user model, they forget to update the docs. After three months, the docs are useless.

**Correct approach:**
Generate API documentation from code using FastAPI's OpenAPI integration or tools like pdoc. If you must write manually, add a CI check that fails if docs don't match the OpenAPI schema.

### ❌ Mistake 3: No Contribution Guidelines

**Wrong approach:**
Just saying "contributions welcome" without explaining how to contribute.

```markdown
# Contributing

Pull requests are welcome!
```

**Why it fails:**
Someone who wants to contribute doesn't know:
- How to set up the development environment
- Whether they should branch from main or develop
- What tests to run before submitting
- How to write a good commit message
- Whether you use conventional commits

**Correct approach:**
Write a CONTRIBUTING.md that covers:
1. Development environment setup
2. Running tests locally
3. Code style requirements
4. PR process (branch naming, review expectations)
5. What makes a good PR description

```markdown
# Contributing to PyMind

## Development Setup

```bash
git clone git@github.com:yourorg/pymind.git
cd pymind
pip install -e ".[dev]"
pytest
```

## Code Style

We use:
- ruff for linting (see pyproject.toml)
- mypy for type checking
- pytest for testing

Run `make lint` before committing.

## Submitting PRs

1. Create a branch from `main`
2. Follow conventional commits: `feat: add new endpoint`
3. Include tests for new functionality
4. Update documentation if needed
5. PRs require one reviewer's approval
```

### ❌ Mistake 4: No Documentation for Decisions

**Wrong approach:**
Making architectural decisions in Slack messages or meetings without documenting them.

**Why it fails:**
Six months later:
- A new engineer asks "why did we choose X?"
- Nobody remembers the discussion
- The same debate happens again
- You make a different decision for different reasons

**Correct approach:**
Write an ADR for any decision that:
- Affects multiple components
- Would be expensive to reverse
- Took significant research

Keep ADRs in `docs/adr/` and link to them from relevant code comments.

## Summary

- Documentation falls into four types: tutorials, how-to guides, reference, and explanation—write the right type for your reader's need
- The Divio system provides a mental framework for organizing all project documentation
- READMEs should answer three questions in under 60 seconds: what, how to run, where next
- ADRs document architectural decisions so teams don't re-litigate the same issues
- Auto-generate API documentation from code using FastAPI's OpenAPI support
- Write CONTRIBUTING.md to help new contributors get started

## Next Steps

→ `02-reading-other-peoples-code.md` — How to navigate and understand unfamiliar codebases, a skill you'll use constantly as you work on projects with multiple contributors.
