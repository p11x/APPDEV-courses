---
Category: DevOps
Subcategory: Containerization
Concept: Docker Basics
Purpose: Understanding containerization with Docker for cloud deployments
Difficulty: beginner
Prerequisites: Linux Fundamentals
RelatedFiles: 02_Advanced_Docker.md
UseCase: Containerized application deployment
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## WHY

Docker containers are fundamental to modern cloud-native applications and microservices.

## WHAT

### Docker Concepts

**Image**: Container template

**Container**: Running image instance

**Dockerfile**: Image definition

**Registry**: Image storage

## HOW

### Example: Simple Dockerfile

```dockerfile
FROM node:18-alpine

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

EXPOSE 3000

CMD ["node", "server.js"]
```

### Build and Run

```bash
# Build image
docker build -t myapp:latest .

# Run container
docker run -p 3000:3000 myapp:latest
```

## CROSS-REFERENCES

### Related Technologies

- ECS: AWS container service
- EKS: Kubernetes service
- Fargate: Serverless containers