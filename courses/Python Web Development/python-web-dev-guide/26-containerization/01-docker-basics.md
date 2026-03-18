# Docker Basics

## What You'll Learn
- Docker concepts
- Writing Dockerfiles
- Docker Compose

## Prerequisites
- Completed microservices folder

## Dockerfile

```dockerfile
# Use slim Python image
FROM python:3.11-slim

WORKDIR /app

# Install dependencies
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copy application
COPY . .

# Expose port
EXPOSE 8000

# Run application
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## Build and Run

```bash
# Build image
docker build -t myapp .

# Run container
docker run -p 8000:8000 myapp

# Run in background
docker run -d -p 8000:8000 --name myapp myapp
```

## Docker Compose

```yaml
version: '3.8'
services:
  app:
    build: .
    ports:
      - "8000:8000"
    environment:
      - DATABASE_URL=postgresql://user:pass@db:5432/myapp
    depends_on:
      - db
      - redis
  
  db:
    image: postgres:15
    environment:
      POSTGRES_USER: user
      POSTGRES_PASSWORD: pass
      POSTGRES_DB: myapp
  
  redis:
    image: redis:7
```

## Summary
- Use Docker for consistent environments
- Use Docker Compose for multi-container apps
- Keep images small with slim

## Next Steps
→ Continue to `02-optimizing-dockerfiles.md`
