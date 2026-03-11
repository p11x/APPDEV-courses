# Docker & Containerization

## Concept Title and Overview

In this lesson, you'll learn how to containerize your Spring Boot applications using Docker. Containerization ensures your application runs consistently across different environments.

## Real-World Importance and Context

Docker has revolutionized software deployment. It packages your application with all its dependencies, ensuring "it works on my machine" becomes a thing of the past. This is essential for:
- Consistent development environments
- Easy deployment
- Microservices architecture
- Scaling applications

## Detailed Step-by-Step Explanation

### Understanding Docker

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    DOCKER CONCEPTS                                        │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  IMAGE                                                                  │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ A read-only template for creating containers                     │   │
│  │ Contains: Application code, Runtime, Libraries, Dependencies     │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  CONTAINER                                                             │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ A running instance of an image                                   │   │
│  │ Isolated process that runs the application                      │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  DOCKERFILE                                                            │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Script that defines how to build an image                        │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  DOCKER HUB                                                            │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Public registry for sharing images                               │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Creating a Dockerfile for Spring Boot

```dockerfile
# Build stage
FROM maven:3.9-eclipse-temurin-21 AS build
WORKDIR /app
COPY pom.xml .
COPY src ./src
RUN mvn clean package -DskipTests

# Runtime stage
FROM eclipse-temurin:21-jre-alpine
WORKDIR /app

# Create non-root user for security
RUN addgroup -S spring && adduser -S spring -G spring
USER spring:spring

# Copy jar from build stage
COPY --from=build /app/target/*.jar app.jar

# Expose port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s \
  CMD wget --quiet --tries=1 --spider http://localhost:8080/actuator/health || exit 1

# Run application
ENTRYPOINT ["java", "-jar", "app.jar"]
```

### Multi-Stage Build (Optimized)

```dockerfile
# Build stage
FROM maven:3.9-eclipse-temurin-21 AS build
WORKDIR /build
COPY pom.xml .
RUN mvn dependency:go-offline
COPY src ./src
RUN mvn clean package -DskipTests

# Runtime stage (smaller image)
FROM eclipse-temurin:21-jre-alpine
WORKDIR /app
COPY --from=build /build/target/*.jar app.jar
EXPOSE 8080
ENTRYPOINT ["java", "-jar", "app.jar"]
```

### Docker Compose for Full-Stack Apps

```yaml
version: '3.8'

services:
  # Spring Boot Backend
  backend:
    build: ./backend
    ports:
      - "8080:8080"
    environment:
      - SPRING_PROFILES_ACTIVE=prod
      - SPRING_DATASOURCE_URL=jdbc:mysql://database:3306/mydb
      - SPRING_DATASOURCE_USERNAME=root
      - SPRING_DATASOURCE_PASSWORD=secret
    depends_on:
      - database
    networks:
      - app-network

  # Angular Frontend (served by Nginx)
  frontend:
    build: ./frontend
    ports:
      - "80:80"
    depends_on:
      - backend
    networks:
      - app-network

  # MySQL Database
  database:
    image: mysql:8.0
    ports:
      - "3306:3306"
    environment:
      - MYSQL_ROOT_PASSWORD=secret
      - MYSQL_DATABASE=mydb
    volumes:
      - mysql-data:/var/lib/mysql
    networks:
      - app-network

  # Redis Cache
  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    networks:
      - app-network

networks:
  app-network:
    driver: bridge

volumes:
  mysql-data:
```

### Docker Commands

```bash
# Build image
docker build -t myapp:1.0 .

# Run container
docker run -p 8080:8080 -d myapp:1.0

# Run with environment variables
docker run -p 8080:8080 \
  -e SPRING_PROFILES_ACTIVE=prod \
  -d myapp:1.0

# Run with Docker Compose
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all
docker-compose down
```

### Optimizing Docker Images

```dockerfile
# Use specific JDK version, not latest
FROM eclipse-temurin:21-jre-alpine

# Use layers efficiently
COPY pom.xml /tmp/
RUN mvn dependency:go-offline -f /tmp/pom.xml
COPY src /tmp/src
RUN mvn package -f /tmp/pom.xml -DskipTests

# Final stage
FROM eclipse-temurin:21-jre-alpine
COPY --from=0 /tmp/target/*.jar app.jar
ENTRYPOINT ["java", "-jar", "app.jar"]
```

## Angular Docker Setup

```dockerfile
# Build Angular app
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build -- --configuration=production

# Serve with Nginx
FROM nginx:alpine
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

```nginx
# nginx.conf
server {
    listen 80;
    server_name localhost;
    
    root /usr/share/nginx/html;
    index index.html;
    
    location / {
        try_files $uri $uri/ /index.html;
    }
    
    location /api {
        proxy_pass http://backend:8080;
    }
}
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Use multi-stage builds** - Smaller final images

2. **Don't run as root** - Security best practice

3. **Use specific versions** - Not "latest" tags

4. **Leverage caching** - Order Dockerfile commands optimally

5. **Use .dockerignore** - Exclude unnecessary files

### Common Pitfalls

1. **Large images** - Use Alpine base images

2. **Exposing secrets** - Use environment variables, not in image

3. **Not using health checks** - Container may appear healthy when app is down

4. **Port conflicts** - Remember to map ports

## Student Hands-On Exercises

### Exercise 1: Create Dockerfile (Easy)
Create a Dockerfile for your Spring Boot application

### Exercise 2: Docker Compose (Medium)
Set up Docker Compose with backend, database, and frontend

### Exercise 3: Multi-Stage Build (Hard)
Optimize your Dockerfile for production

---

## Summary

In this lesson, you've learned:
- Docker fundamentals and concepts
- Creating Dockerfiles for Spring Boot
- Docker Compose for full-stack applications
- Angular Docker setup
- Best practices and optimization

---

**Next Lesson**: In the next lesson, we'll explore Redis Caching.
