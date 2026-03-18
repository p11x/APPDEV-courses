# Deploying with Docker

## What You'll Learn
- Creating Dockerfiles
- Building and running containers

## Prerequisites
- Completed preparing for production

## Dockerfile

```dockerfile
FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY . .

EXPOSE 8000

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
```

## Docker Compose

```yaml
version: '3'
services:
  web:
    build: .
    ports:
      - "8000:8000"
    environment:
      - DATABASE_URL=postgresql://user:pass@db/web
    depends_on:
      - db
  
  db:
    image: postgres:15
    environment:
      POSTGRES_USER: user
      POSTGRES_PASSWORD: pass
      POSTGRES_DB: web
```

## Commands

```bash
docker build -t myapp .
docker run -p 8000:8000 myapp
docker-compose up -d
```

## Summary
- Use Docker for consistent deployments
- Keep images small with slim Python
- Use docker-compose for multi-container apps
