# Section 19: Deployment & DevOps

## Building for Production

### Angular Build
```bash
ng build --configuration=production
# Output: dist/ folder
```

### Spring Boot Build
```bash
./mvnw clean package
# Output: target/*.jar
```

---

## Deployment Options

### Backend (Spring Boot)
1. **Heroku** - Easy deployment
2. **AWS Elastic Beanstalk** - Scalable
3. **Docker** - Containerized

### Frontend (Angular)
1. **Netlify** - Free hosting
2. **Vercel** - Fast deployment
3. **GitHub Pages** - Free static hosting
4. **AWS S3 + CloudFront** - CDN

---

## Docker (Optional)

### Dockerfile (Backend)
```dockerfile
FROM openjdk:17
COPY target/*.jar app.jar
EXPOSE 8080
ENTRYPOINT ["java", "-jar", "app.jar"]
```

### Docker Compose
```yaml
version: '3.8'
services:
  backend:
    build: ./backend
    ports:
      - "8080:8080"
  frontend:
    build: ./frontend
    ports:
      - "80:80"
  database:
    image: mcr.microsoft.com/mssql/server
```

---

## CI/CD with GitHub Actions

```yaml
name: Deploy
on: [push]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Build Angular
        run: npm install && npm run build
      - name: Build Spring Boot
        run: cd backend && mvn package
```

---

## Summary

Deployment = Build + Host + Configure + Monitor
