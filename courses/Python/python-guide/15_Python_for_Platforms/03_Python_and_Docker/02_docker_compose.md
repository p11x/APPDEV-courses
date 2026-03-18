# 📦 Docker Compose: Multi-Container Applications

## 🎯 What You'll Learn

- Writing docker-compose.yml
- Defining services, volumes, networks
- Running multi-container apps

---

## docker-compose.yml

```yaml
version: '3.8'

services:
  api:
    build: .
    ports:
      - "8000:8000"
    env_file:
      - .env
    depends_on:
      - db
      - redis
    volumes:
      - ./app:/app

  db:
    image: postgres:16
    environment:
      POSTGRES_USER: app
      POSTGRES_PASSWORD: secret
      POSTGRES_DB: myapp
    volumes:
      - postgres_data:/var/lib/postgresql/data

  redis:
    image: redis:7-alpine

volumes:
  postgres_data:
```

### 💡 Line-by-Line Breakdown

```yaml
services:                     # Define containers
  api:                       # Service name
    build: .                 # Build from Dockerfile
    ports: ["8000:8000"]    # Port mapping
    env_file: [.env]        # Environment variables
    depends_on: [db]        # Start after db

  db:                       # Database service
    image: postgres:16      # Use official image
    volumes: [postgres_data:/var/lib/postgresql/data]  # Persist data

volumes:                    # Named volumes
  postgres_data:
```

---

## Commands

```bash
# Start all services
docker compose up

# Start in background
docker compose up -d

# Stop all
docker compose down

# View logs
docker compose logs -f api

# Scale a service
docker compose up -d --scale api=3
```

---

## ✅ Summary

- docker-compose.yml defines multi-container applications
- Use volumes for persistent data
- Use depends_on for startup order

## 🔗 Further Reading

- [Docker Compose Documentation](https://docs.docker.com/compose/)
