# Section 12: Spring Boot - Setup & Configuration

## Introduction to Spring Boot

Spring Boot makes creating Java applications easy. It provides auto-configuration and embedded servers.

### Creating a Spring Boot Project

**Option 1: Spring Initializr**
1. Go to https://start.spring.io
2. Select: Maven, Java, Spring Boot version
3. Add dependencies: Spring Web, Spring Data JPA, SQL Server Driver, Validation
4. Click Generate

**Option 2: Command Line**
```bash
curl https://start.spring.io/starter.zip \
  -d groupId=com.example \
  -d artifactId=product-management \
  -d name=product-management \
  -d dependencies=web,data-jpa,mssql,validation \
  -o product-management.zip
```

### Project Structure
```
product-management/
├── src/
│   └── main/
│       ├── java/
│       │   └── com/example/product/
│       │       ├── ProductManagementApplication.java
│       │       └── controller/
│       │       └── service/
│       │       └── repository/
│       │       └── model/
│       └── resources/
│           ├── application.properties
│           └── data.sql
├── pom.xml
└── mvnw
```

---

## 12.2 Configuration

### application.properties
```properties
# Server
server.port=8080

# Database
spring.datasource.url=jdbc:sqlserver://localhost:1433;databaseName=productDB
spring.datasource.username=sa
spring.datasource.password=your_password
spring.datasource.driver-class-name=com.microsoft.sqlserver.jdbc.SQLServerDriver

# JPA
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=true
spring.jpa.properties.hibernate.format_sql=true

# JSON
spring.jackson.serialization.write-dates-as-timestamps=false
```

### pom.xml Dependencies
```xml
<dependencies>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-web</artifactId>
    </dependency>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-data-jpa</artifactId>
    </dependency>
    <dependency>
        <groupId>com.microsoft.sqlserver</groupId>
        <artifactId>mssql-jdbc</artifactId>
    </dependency>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-validation</artifactId>
    </dependency>
</dependencies>
```

---

## 12.3 Main Application

### ProductManagementApplication.java
```java
package com.example.product;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class ProductManagementApplication {
    public static void main(String[] args) {
        SpringApplication.run(ProductManagementApplication.class, args);
    }
}
```

---

## Running the Application

```bash
./mvnw spring-boot:run
```

---

## Summary

Spring Boot = Java + Spring - XML Configuration
