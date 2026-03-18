# Phase 1 — Project Setup Checklist

Use this checklist to verify your implementation matches the reference code exactly.

## Prerequisites Checklist

- [ ] Python 3.11+ installed (`python --version`)
- [ ] Docker and Docker Compose installed (`docker --version`, `docker-compose --version`)
- [ ] OpenAI API key obtained from platform.openai.com

## Project Structure Checklist

- [ ] Root directory created with `pyproject.toml`
- [ ] Virtual environment created and activated
- [ ] Dependencies installed with `pip install -e ".[dev]"`
- [ ] Folder structure matches specification:

```
pymind/
├── app/
│   ├── __init__.py
│   ├── main.py
│   ├── config.py
│   └── dependencies.py
├── core/
│   ├── __init__.py
│   ├── database.py
│   └── redis.py
├── alembic/
│   └── env.py
├── tests/
│   └── __init__.py
├── .env.example
├── pyproject.toml
└── docker-compose.yml
```

## Code Files Checklist

### app/__init__.py

- [ ] Empty or contains package docstring

### app/config.py

- [ ] Settings class extends BaseSettings from pydantic_settings
- [ ] All settings have proper Field descriptions
- [ ] CORS_ORIGINS field_validator implemented
- [ ] is_development property implemented
- [ ] get_settings() function with @lru_cache

### app/dependencies.py

- [ ] get_db_session async generator function
- [ ] DbSession type alias using Annotated
- [ ] RedisClient type alias using Annotated

### app/core/__init__.py

- [ ] Imports and exports database and redis modules

### app/core/database.py

- [ ] Base class extends DeclarativeBase
- [ ] Global engine and async_session_maker variables
- [ ] init_db() async function
- [ ] close_db() async function
- [ ] get_session() async generator with commit/rollback

### app/core/redis.py

- [ ] Global redis_client variable
- [ ] init_redis() async function using redis.asyncio
- [ ] close_redis() async function
- [ ] get_redis() async function

### app/main.py

- [ ] lifespan asynccontextmanager implemented
- [ ] create_app() factory function
- [ ] CORS middleware added with settings.origins
- [ ] /health endpoint returns JSONResponse with status
- [ ] app instance created at module level

### pyproject.toml

- [ ] Project metadata (name, version, description)
- [ ] Python requirement >=3.11
- [ ] All required dependencies listed
- [ ] Dev dependencies in [project.optional-dependencies]
- [ ] Tool configurations for pytest, ruff, mypy

### docker-compose.yml

- [ ] postgres service with pgvector image
- [ ] redis service with redis:7-alpine
- [ ] app service with build configuration
- [ ] Proper port mappings
- [ ] Volume configurations
- [ ] Health checks for postgres and redis

### .env.example

- [ ] All environment variables documented
- [ ] Clear instructions for obtaining values

## Functionality Checklist

### Database Connection

- [ ] PostgreSQL container starts successfully
- [ ] App connects to database on startup
- [ ] Database connection properly closed on shutdown

### Redis Connection

- [ ] Redis container starts successfully
- [ ] App connects to Redis on startup
- [ ] Redis connection properly closed on shutdown

### Health Check

- [ ] GET /health returns 200 status
- [ ] Response contains "status": "healthy"
- [ ] Response shows current environment

### Configuration

- [ ] Settings load from .env file
- [ ] CORS origins parsed correctly
- [ ] Development mode detected properly

## Testing Checklist

### Manual Testing

```bash
# Start infrastructure
docker-compose up -d postgres redis

# Run app
uvicorn app.main:app --reload

# Test health endpoint
curl http://localhost:8000/health

# Expected response:
# {"status":"healthy","environment":"development"}
```

### Automated Testing (Future Phases)

- [ ] pytest installed and configured
- [ ] Test discovery working
- [ ] Async tests supported

## Docker Testing

```bash
# Build image
docker-compose build

# Start all services
docker-compose up -d

# Check logs
docker-compose logs -f app

# Check health
docker-compose exec app curl http://localhost:8000/health
```

## Common Issues and Fixes

| Issue | Cause | Fix |
|-------|-------|-----|
| ModuleNotFoundError | Package not installed | `pip install -e .` |
| Connection refused (DB) | PostgreSQL not running | `docker-compose up -d postgres` |
| Connection refused (Redis) | Redis not running | `docker-compose up -d redis` |
| Invalid JWT secret | Weak secret key | Generate with `openssl rand -hex 32` |
| CORS errors | Wrong origin in .env | Check CORS_ORIGINS matches frontend |

## Next Phase Preparation

Before proceeding to Phase 2, ensure:

- [ ] You can run the app without errors
- [ ] Health endpoint returns proper response
- [ ] Database and Redis connections work
- [ ] You understand the lifespan pattern
- [ ] You understand pydantic-settings

## Sign-off

When all items are checked, you have completed Phase 1:

- [ ] Project runs successfully
- [ ] All tests pass
- [ ] Ready to proceed to Phase 2
