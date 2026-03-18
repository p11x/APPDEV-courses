# 🐳 Dockerfile for Python Applications

## 🎯 What You'll Learn

- Writing a production-ready Dockerfile
- Layer caching optimization
- Multi-stage builds

---

## Basic Dockerfile

```dockerfile
# Use official Python image
FROM python:3.12-slim

# Set working directory
WORKDIR /app

# Copy dependency files first (for layer caching!)
COPY pyproject.toml .

# Install dependencies
RUN pip install --no-cache-dir -r pyproject.toml

# Copy application code
COPY . .

# Run the application
CMD ["python", "main.py"]
```

### 💡 Line-by-Line Breakdown

```dockerfile
FROM python:3.12-slim          # Base image - slim is smaller
WORKDIR /app                   # Set working directory

COPY pyproject.toml .          # Copy dependency file FIRST
RUN pip install --no-cache-dir .  # Install dependencies (cached!)

COPY . .                        # Copy rest of application

CMD ["python", "main.py"]      # Run command
```

---

## .dockerignore

```text
__pycache__/
*.pyc
*.pyo
*.pyd
.Python
*.so
*.egg
*.egg-info/
dist/
build/
.env
.venv/
venv/
.git/
.gitignore
*.md
Dockerfile
docker-compose.yml
```

---

## Multi-Stage Build

```dockerfile
# Stage 1: Build
FROM python:3.12-slim AS builder

WORKDIR /app
COPY pyproject.toml .
RUN pip install --prefix=/install -r pyproject.toml

# Stage 2: Runtime
FROM python:3.12-slim

WORKDIR /app
COPY --from=builder /install /usr/local
COPY . .

CMD ["python", "main.py"]
```

---

## Running

```bash
# Build
docker build -t myapp .

# Run
docker run -p 8000:8000 --env-file .env myapp
```

---

## ✅ Summary

- Use slim images for smaller size
- Copy pyproject.toml before source for caching
- Use .dockerignore to exclude files
- Multi-stage builds reduce final image size

## 🔗 Further Reading

- [Docker Best Practices](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/)
