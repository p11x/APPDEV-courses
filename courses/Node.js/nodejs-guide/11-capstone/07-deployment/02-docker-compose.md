# Docker Compose

## What You'll Build In This File

Docker Compose configuration for NodeMark with PostgreSQL and health checks.

## Complete Docker Compose File

Create `docker-compose.yml`:

```yaml
# docker-compose.yml - NodeMark with PostgreSQL

services:
  # NodeMark REST API
  api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - DB_HOST=postgres
      - DB_PORT=5432
      - DB_NAME=nodemark
      - DB_USER=nodemark
      - DB_PASSWORD=nodemark_password
      - JWT_SECRET=production-secret-change-this
    depends_on:
      postgres:
        condition: service_healthy
    restart: unless-stopped
    healthcheck:
      test: ["CMD", "wget", "--spider", "-q", "http://localhost:3000/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 10s

  # PostgreSQL database
  postgres:
    image: postgres:15-alpine
    environment:
      - POSTGRES_USER=nodemark
      - POSTGRES_PASSWORD=nodemark_password
      - POSTGRES_DB=nodemark
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    restart: unless-stopped
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U nodemark -d nodemark"]
      interval: 10s
      timeout: 5s
      retries: 5
      start_period: 10s

volumes:
  # Persistent database storage
  postgres_data:

networks:
  default:
    name: nodemark-network
```

## docker-compose.override.yml

Create for local development:

```yaml
# docker-compose.override.yml - Development overrides
# Automatically loaded by docker-compose

services:
  api:
    build:
      target: builder  # Use builder stage for hot reload
    environment:
      - NODE_ENV=development
      - DB_HOST=postgres
    volumes:
      - ./src:/app/src  # Live reload source
    command: npm run dev
```

## Running with Docker Compose

```bash
# Start all services
docker compose up -d

# View logs
docker compose logs -f

# Stop all services
docker compose down

# Run migrations in the container
docker compose exec api npm run migrate

# Access PostgreSQL
docker compose exec postgres psql -U nodemark
```

## How It Connects

This connects to concepts from:
- [10-deployment/docker/02-docker-compose.md](../../../10-deployment/docker/02-docker-compose.md)

## Common Mistakes

- Not using health checks
- Not waiting for database to be ready
- Using wrong network configuration

## Try It Yourself

### Exercise 1: Start Services
Run docker compose up and verify all services start.

### Exercise 2: Test API
Make requests to the API running in Docker.

### Exercise 3: Run Migrations
Run migrations inside the container.

## Next Steps

Continue to [03-run-guide.md](./03-run-guide.md) for the complete run guide.
