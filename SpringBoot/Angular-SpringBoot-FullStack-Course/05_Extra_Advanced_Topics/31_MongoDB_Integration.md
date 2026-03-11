# MongoDB Integration

## Concept Title and Overview

In this lesson, you'll learn how to integrate MongoDB with Spring Boot for flexible, document-based data storage.

## Real-World Importance and Context

MongoDB is a popular NoSQL database that stores data in flexible JSON-like documents. It's ideal for:
- Rapid prototyping
- Unstructured data
- Scalability requirements
- Flexible schemas

## Detailed Step-by-Step Explanation

### Adding MongoDB Dependency

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-data-mongodb</artifactId>
</dependency>
```

### Document Entity

```java
@Document(collection = "tasks")
public class Task {
    @Id
    private String id;
    private String title;
    private String description;
    private String status;
    private List<String> tags;
    private Map<String, Object> metadata;
    
    // Constructors, getters, setters
}
```

### Repository

```java
public interface TaskRepository extends MongoRepository<Task, String> {
    List<Task> findByStatus(String status);
    List<Task> findByTagsContaining(String tag);
}
```

### Service

```java
@Service
public class TaskService {
    private final TaskRepository taskRepository;
    
    public TaskService(TaskRepository taskRepository) {
        this.taskRepository = taskRepository;
    }
    
    public Task create(Task task) {
        return taskRepository.save(task);
    }
    
    public List<Task> findByStatus(String status) {
        return taskRepository.findByStatus(status);
    }
}
```

---

## Summary

You've learned MongoDB integration with Spring Boot.
