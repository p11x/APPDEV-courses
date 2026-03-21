# Docker Setup

## Dockerfile

```dockerfile
FROM node:18-alpine

WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

EXPOSE 3000
CMD ["npm", "start"]
```

## Build and Run

```bash
docker build -t nextjs-app .
docker run -p 3000:3000 nextjs-app
```
