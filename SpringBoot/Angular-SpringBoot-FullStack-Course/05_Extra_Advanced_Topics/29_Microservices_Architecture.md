# Microservices Architecture

## Concept Title and Overview

In this lesson, you'll learn about microservices architecture, a design approach where applications are built as a collection of small, independent services.

## Real-World Importance and Context

Microservices architecture enables teams to develop, deploy, and scale services independently. It's used by companies like Netflix, Amazon, and Uber.

## Detailed Step-by-Step Explanation

### Monolith vs Microservices

```
┌─────────────────────────────────────────────────────────────────────────┐
│                 MONOLITH vs MICROSERVICES                                │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  MONOLITHIC                      MICROSERVICES                         │
│  ┌──────────────────┐            ┌─────┐ ┌─────┐ ┌─────┐            │
│  │                  │            │User │ │Order│ │Paymt│            │
│  │  ┌──┐ ┌──┐ ┌──┐ │            │Svc  │ │Svc  │ │Svc  │            │
│  │  │UI│ │API│ │DB│ │            └──┬──┘ └──┬──┘ └──┬──┘            │
│  │  └──┘ └──┘ └──┘ │                 │       │       │               │
│  │       │         │                 └───────┼───────┘               │
│  │       └─────────┤                         │                         │
│  │                 │                    ┌────▼────┐                   │
│  └─────────────────┘                    │  API    │                   │
│                                        │ Gateway │                   │
│  Pros:                    Pros:        └─────────┘                    │
│  • Simple to develop      • Independent deployment                    │
│  • Easy to test           • Technology flexibility                    │
│  • Simple deployment      • Scalability                              │
│                                        Cons:                           │
│  Cons:                      • Complexity                             │
│  • Hard to scale          • Distributed debugging                   │
│  • Technology locked      • Network issues                          │
│  • Single point of failure                               │            │
└─────────────────────────────────────────────────────────────────────────┘
```

### Spring Cloud Components

```
┌─────────────────────────────────────────────────────────────────────────┐
│                 SPRING CLOUD COMPONENTS                                  │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  EUREKA (Service Discovery)                                            │
│  • Services register themselves                                        │
│  • Clients discover services dynamically                               │
│                                                                         │
│  API GATEWAY (Zuul/Spring Cloud Gateway)                              │
│  • Single entry point for all clients                                 │
│  • Request routing, authentication, rate limiting                     │
│                                                                         │
│  HYSTRIX (Circuit Breaker)                                            │
│  • Prevent cascade failures                                           │
│  • Fallback mechanisms                                               │
│                                                                         │
│  CONFIG SERVER                                                        │
│  • Centralized configuration                                         │
│  • Environment-specific settings                                      │
│                                                                         │
│  FEIGN CLIENT                                                         │
│  • Declarative REST client                                            │
│  • Load balancing                                                    │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Service Registration with Eureka

**Eureka Server:**
```xml
<dependency>
    <groupId>org.springframework.cloud</groupId>
    <artifactId>spring-cloud-starter-netflix-eureka-server</artifactId>
</dependency>
```

```java
@SpringBootApplication
@EnableEurekaServer
public class EurekaServerApplication {
    public static void main(String[] args) {
        SpringApplication.run(EurekaServerApplication.class, args);
    }
}
```

**Eureka Client:**
```xml
<dependency>
    <groupId>org.springframework.cloud</groupId>
    <artifactId>spring-cloud-starter-netflix-eureka-client</artifactId>
</dependency>
```

```properties
spring.application.name=user-service
eureka.client.service-url.defaultZone=http://localhost:8761/eureka/
eureka.instance.prefer-ip-address=true
```

### API Gateway

```xml
<dependency>
    <groupId>org.springframework.cloud</groupId>
    <artifactId>spring-cloud-starter-gateway</artifactId>
</dependency>
```

```yaml
spring:
  cloud:
    gateway:
      routes:
        - id: user-service
          uri: lb://user-service
          predicates:
            - Path=/users/**
        - id: task-service
          uri: lb://task-service
          predicates:
            - Path=/tasks/**
```

## Student Hands-On Exercises

### Exercise 1: Set up Eureka (Medium)
Create a Eureka server and register services

### Exercise 2: API Gateway (Hard)
Implement API Gateway for routing

---

## Summary

You've learned:
- Microservices architecture fundamentals
- Spring Cloud components
- Service discovery with Eureka
- API Gateway implementation

---

**Next Lesson**: Message Queues with RabbitMQ
