# GraphQL API Development

## Concept Title and Overview

In this lesson, you'll learn how to build GraphQL APIs with Spring Boot, an alternative to REST that provides more flexibility.

## Real-World Importance and Context

GraphQL allows clients to request exactly the data they need, reducing over-fetching and under-fetching. It's ideal for:
- Mobile applications
- Complex data requirements
- Rapid frontend development

## Detailed Step-by-Step Explanation

### Adding GraphQL

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-graphql</artifactId>
</dependency>
```

### GraphQL Schema

```graphql
type Task {
    id: ID!
    title: String!
    description: String
    status: TaskStatus!
    user: User!
}

type User {
    id: ID!
    name: String!
    email: String!
    tasks: [Task!]!
}

enum TaskStatus {
    PENDING
    IN_PROGRESS
    COMPLETED
}

type Query {
    tasks: [Task!]!
    task(id: ID!): Task
    users: [User!]!
}

type Mutation {
    createTask(input: CreateTaskInput!): Task!
    updateTask(id: ID!, input: UpdateTaskInput!): Task
    deleteTask(id: ID!): Boolean
}

input CreateTaskInput {
    title: String!
    description: String
    status: TaskStatus
}

input UpdateTaskInput {
    title: String
    description: String
    status: TaskStatus
}
```

### GraphQL Controller

```java
@RestController
public class TaskGraphQLController {
    
    @QueryMapping
    public List<Task> tasks() {
        return taskService.findAll();
    }
    
    @QueryMapping
    public Task task(@Argument Long id) {
        return taskService.findById(id).orElse(null);
    }
    
    @MutationMapping
    public Task createTask(@Argument CreateTaskInput input) {
        Task task = new Task();
        task.setTitle(input.getTitle());
        task.setDescription(input.getDescription());
        return taskService.save(task);
    }
}
```

---

## Summary

You've learned GraphQL fundamentals and Spring Boot integration.
