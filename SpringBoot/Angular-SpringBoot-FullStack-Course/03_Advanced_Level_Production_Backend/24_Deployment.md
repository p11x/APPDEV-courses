# Deployment

## Concept Title and Overview

In this lesson, you'll learn how to build and deploy Spring Boot applications to production.

## Real-World Importance and Context

Deployment is the final step—getting your application running where users can access it.

## Detailed Step-by-Step Explanation

### Maven Build Process

```bash
# Build the application
mvn clean package

# Skip tests during build
mvn clean package -DskipTests
```

### Creating Executable JAR

```bash
# After build, JAR is in target/
java -jar target/myapp-1.0.0.jar

# With custom port
java -jar target/myapp-1.0.0.jar --server.port=9090
```

### Environment Variables

```bash
# Set environment variables
export DATABASE_URL=jdbc:mysql://prod-server:3306/mydb
export DB_USERNAME=produser
export DB_PASSWORD=secret

# Run with environment variables
java -jar myapp.jar
```

### Deployment Platforms

**Heroku:**
1. Create Procfile: `web: java -jar target/myapp.jar`
2. Push to Heroku

**Render:**
1. Connect GitHub repository
2. Set build command: `./mvnw clean package -DskipTests`
3. Set start command: `java -jar target/myapp-1.0.0.jar`

**AWS:**
1. Create JAR
2. Use Elastic Beanstalk or ECS
3. Configure environment variables

## Health Check

```bash
# After deployment, verify health
curl https://your-app.com/actuator/health
```

## Student Hands-On Exercises

### Exercise 1: Build JAR (Easy)
Build your application and run the JAR locally.

### Exercise 2: Deploy to Render (Medium)
Deploy your application to Render.com.

### Exercise 3: Configure Production (Hard)
Set up production database and environment variables.

---

## Summary

You've learned:
- Maven build process
- Creating executable JARs
- Environment configuration
- Deployment to cloud platforms
- Health check verification

---

This completes the Advanced Level! You now have all the skills needed to build production-ready Spring Boot applications.

---

**Final Project**: Now it's time to apply everything you've learned in the Final Project!
