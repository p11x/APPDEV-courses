# Message Queues with RabbitMQ

## Concept Title and Overview

In this lesson, you'll learn how to implement asynchronous messaging using RabbitMQ for decoupled communication between services.

## Real-World Importance and Context

Message queues enable asynchronous communication between services, improving scalability and reliability. They're essential for:
- Decoupling microservices
- Handling high loads
- Background job processing
- Event-driven architectures

## Detailed Step-by-Step Explanation

### Adding RabbitMQ

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-amqp</artifactId>
</dependency>
```

### Configuration

```java
@Configuration
public class RabbitMQConfig {
    
    @Bean
    public Queue taskQueue() {
        return new Queue("taskQueue", true);
    }
    
    @Bean
    public DirectExchange exchange() {
        return new DirectExchange("taskExchange");
    }
    
    @Bean
    public Binding binding(Queue taskQueue, DirectExchange exchange) {
        return BindingBuilder.bind(taskQueue).to(exchange).with("task.routing.key");
    }
}
```

### Publisher

```java
@Service
public class TaskPublisher {
    
    private final RabbitTemplate rabbitTemplate;
    
    public TaskPublisher(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }
    
    public void publishTask(Task task) {
        rabbitTemplate.convertAndSend("taskExchange", "task.routing.key", task);
    }
}
```

### Consumer

```java
@Service
public class TaskConsumer {
    
    @RabbitListener(queues = "taskQueue")
    public void consumeTask(Task task) {
        System.out.println("Received task: " + task.getTitle());
        // Process task
    }
}
```

---

## Summary

You've learned RabbitMQ basics and Spring Boot integration.
