# Database Configuration

## Concept Title and Overview

In this lesson, you'll learn how to configure databases in Spring Boot. We'll cover MySQL, PostgreSQL, connection pooling, and environment-specific configurations. This is crucial for moving from development to production.

## Real-World Importance and Context

Your application needs a database to store data. In development, you might use an in-memory database like H2. In production, you'll use MySQL or PostgreSQL. Understanding how to configure these connections is essential for any backend developer.

## Detailed Step-by-Step Explanation

### MySQL Connection Setup

MySQL is one of the most popular relational databases. Here's how to configure it:

**1. Add MySQL Dependency:**

```xml
<!-- pom.xml -->
<dependency>
    <groupId>com.mysql</groupId>
    <artifactId>mysql-connector-j</artifactId>
    <scope>runtime</scope>
</dependency>
```

**2. Configure application.properties:**

```properties
# MySQL Configuration
spring.datasource.url=jdbc:mysql://localhost:3306/mydb?useSSL=false&serverTimezone=UTC&allowPublicKeyRetrieval=true
spring.datasource.username=root
spring.datasource.password=secret
spring.datasource.driver-class-name=com.mysql.cj.jdbc.Driver

# JPA Configuration
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=true
spring.jpa.properties.hibernate.dialect=org.hibernate.dialect.MySQLDialect
```

**MySQL URL Parameters Explained:**
- `useSSL=false` - Disable SSL for local development
- `serverTimezone=UTC` - Set timezone
- `allowPublicKeyRetrieval=true` - Allow RSA key retrieval

### PostgreSQL Connection Setup

PostgreSQL is a powerful open-source database:

**1. Add PostgreSQL Dependency:**

```xml
<dependency>
    <groupId>org.postgresql</groupId>
    <artifactId>postgresql</artifactId>
    <scope>runtime</scope>
</dependency>
```

**2. Configure application.properties:**

```properties
# PostgreSQL Configuration
spring.datasource.url=jdbc:postgresql://localhost:5432/mydb
spring.datasource.username=postgres
spring.datasource.password=secret
spring.datasource.driver-class-name=org.postgresql.Driver

# JPA Configuration
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=true
spring.jpa.properties.hibernate.dialect=org.hibernate.dialect.PostgreSQLDialect
```

### Understanding application.properties Configuration

Here's a complete database configuration:

```properties
# ==================== DataSource Configuration ====================
# URL (JDBC connection string)
spring.datasource.url=jdbc:mysql://localhost:3306/employee_db

# Database credentials
spring.datasource.username=root
spring.datasource.password=mysecretpassword

# Driver class (auto-detected usually)
spring.datasource.driver-class-name=com.mysql.cj.jdbc.Driver

# Connection pool settings (HikariCP)
spring.datasource.hikari.maximum-pool-size=10
spring.datasource.hikari.minimum-idle=5
spring.datasource.hikari.connection-timeout=30000
spring.datasource.hikari.idle-timeout=600000
spring.datasource.hikari.max-lifetime=1800000

# ==================== JPA/Hibernate Configuration ====================
# Database platform (auto-detects usually)
spring.jpa.database-platform=org.hibernate.dialect.MySQLDialect

# DDL Auto: create, create-drop, update, validate, none
spring.jpa.hibernate.ddl-auto=update

# Show SQL in console
spring.jpa.show-sql=true

# Format SQL output
spring.jpa.properties.hibernate.format_sql=true

# Statistics (for debugging)
spring.jpa.properties.hibernate.generate_statistics=true

# Default batch fetch size
spring.jpa.properties.hibernate.default_batch_fetch_size=20
```

### Connection Pooling Basics

Connection pooling is essential for production applications:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    CONNECTION POOLING                                   │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  WITHOUT POOLING                         WITH POOLING                  │
│  ┌──────────┐                           ┌──────────────────┐         │
│  │ Request  │─────► Open Connection ──►│   Database       │         │
│  │          │◄──── Close Connection ◄──│                  │         │
│  └──────────┘                           └──────────────────┘         │
│                                                                         │
│  Every request:                                Pool maintains:          │
│  • Open connection                            • 10 connections          │
│  • Execute query                              • Reuse connections      │
│  • Close connection                           • Faster responses       │
│                                                                         │
│  Problems:                                Benefits:                    │
│  • Slow (connection overhead)              • Fast (reuse)              │
│  • Database overload                       • Efficient                 │
│  • Connection exhaustion                   • Scalable                 │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

Spring Boot uses HikariCP as the default connection pool:

```properties
# HikariCP Configuration
spring.datasource.hikari.maximum-pool-size=10      # Max connections
spring.datasource.hikari.minimum-idle=5            # Min idle connections
spring.datasource.hikari.connection-timeout=30000  # Timeout (ms)
spring.datasource.hikari.idle-timeout=600000       # Idle timeout (ms)
spring.datasource.hikari.max-lifetime=1800000       # Max lifetime (ms)
```

### Environment-Specific Configurations

Different environments need different configurations:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    ENVIRONMENT CONFIGURATIONS                           │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  Development:                                                           │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ • H2 in-memory database                                         │   │
│  │ • Auto-create tables (ddl-auto=create-drop)                    │   │
│  │ • Show SQL in console                                           │   │
│  │ • Relaxed security                                              │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  Staging/Production:                                                    │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ • MySQL/PostgreSQL                                              │   │
│  │ • Update tables only (ddl-auto=update)                          │   │
│  │ • No SQL shown                                                  │   │
│  │ • Full security                                                 │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

**Using Profile-Specific Properties:**

Create separate files:
- `application.properties` - Default
- `application-dev.properties` - Development
- `application-prod.properties` - Production

**Or use application.yml with profiles:**

```yaml
spring:
  config:
    activate:
      on-profile: dev
  datasource:
    url: jdbc:h2:mem:testdb
    driver-class-name: org.h2.Driver
  jpa:
    hibernate:
      ddl-auto: create-drop

---
spring:
  config:
    activate:
      on-profile: prod
  datasource:
    url: jdbc:mysql://prod-server:3306/mydb
    username: ${DB_USERNAME}
    password: ${DB_PASSWORD}
  jpa:
    hibernate:
      ddl-auto: validate
```

**Activate profiles:**

```properties
# In application.properties
spring.profiles.active=dev
```

Or via command line:
```bash
java -jar app.jar --spring.profiles.active=prod
```

## Complete Configuration Examples

### MySQL Complete Setup

```properties
# application.properties
spring.application.name=employee-manager

# DataSource
spring.datasource.url=jdbc:mysql://localhost:3306/employee_db?useSSL=false&serverTimezone=UTC&allowPublicKeyRetrieval=true
spring.datasource.username=root
spring.datasource.password=root123
spring.datasource.driver-class-name=com.mysql.cj.jdbc.Driver

# HikariCP Connection Pool
spring.datasource.hikari.maximum-pool-size=10
spring.datasource.hikari.minimum-idle=5
spring.datasource.hikari.connection-timeout=30000
spring.datasource.hikari.idle-timeout=600000
spring.datasource.hikari.max-lifetime=1800000

# JPA/Hibernate
spring.jpa.database-platform=org.hibernate.dialect.MySQLDialect
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=true
spring.jpa.properties.hibernate.format_sql=true
spring.jpa.properties.hibernate.use_sql_comments=true
spring.jpa.properties.hibernate.jdbc.batch_size=20

# Logging
logging.level.org.hibernate.SQL=DEBUG
logging.level.org.hibernate.type.descriptor.sql.BasicBinder=TRACE
```

### PostgreSQL Complete Setup

```properties
# application.properties
spring.application.name=employee-manager

# DataSource
spring.datasource.url=jdbc:postgresql://localhost:5432/employee_db
spring.datasource.username=postgres
spring.datasource.password=postgres123
spring.datasource.driver-class-name=org.postgresql.Driver

# HikariCP
spring.datasource.hikari.maximum-pool-size=20
spring.datasource.hikari.minimum-idle=5

# JPA/Hibernate
spring.jpa.database-platform=org.hibernate.dialect.PostgreSQLDialect
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=true
spring.jpa.properties.hibernate.format_sql=true

# PostgreSQL specific
spring.jpa.properties.hibernate.default_schema=public
```

### Using Environment Variables

For security, never hardcode passwords:

```properties
# Using environment variables
spring.datasource.url=${DATABASE_URL}
spring.datasource.username=${DB_USERNAME}
spring.datasource.password=${DB_PASSWORD}
```

Command line:
```bash
export DB_USERNAME=myuser
export DB_PASSWORD=mypassword
java -jar app.jar
```

Or use .env files:
```properties
# .env
DB_USERNAME=myuser
DB_PASSWORD=mypassword
DATABASE_URL=jdbc:mysql://localhost:3306/mydb
```

## H2 Database for Development

H2 is perfect for development and testing:

```properties
# H2 Configuration (development)
spring.datasource.url=jdbc:h2:mem:testdb
spring.datasource.driver-class-name=org.h2.Driver
spring.datasource.username=sa
spring.datasource.password=

# H2 Console (for debugging)
spring.h2.console.enabled=true
spring.h2.console.path=/h2-console

# JPA
spring.jpa.hibernate.ddl-auto=create-drop
spring.jpa.show-sql=true
```

Access H2 console at: http://localhost:8080/h2-console

## Student Hands-On Exercises

### Exercise 1: MySQL Setup (Easy)
Install MySQL and configure your Spring Boot application to connect to it. Create a simple entity and verify it creates the table.

### Exercise 2: PostgreSQL Setup (Medium)
Set up PostgreSQL and configure your application to use it instead of MySQL.

### Exercise 3: Environment Configuration (Medium)
Create separate configurations for:
- Development (H2)
- Staging (MySQL)
- Production (PostgreSQL)

Use Spring profiles to switch between them.

### Exercise 4: Connection Pool Tuning (Hard)
Research HikariCP and configure optimal settings for:
- A small application (10-50 users)
- A medium application (100-500 users)

### Exercise 5: Security (Hard)
Configure your application to:
- Read database credentials from environment variables
- Not expose sensitive data in logs
- Use SSL connections in production

---

## Summary

In this lesson, you've learned:
- How to configure MySQL database
- How to configure PostgreSQL database
- Connection pooling with HikariCP
- Environment-specific configurations
- Using profiles for different environments
- H2 database for development

Your application can now connect to databases! In the next lesson, we'll explore the Repository Layer in more detail.

---

**Next Lesson**: In the next lesson, we'll explore [Repository Layer](11_Repository_Layer.md) and learn about advanced query methods.
