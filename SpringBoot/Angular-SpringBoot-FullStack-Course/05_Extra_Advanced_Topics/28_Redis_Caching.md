# Redis Caching

## Concept Title and Overview

In this lesson, you'll learn how to implement caching in Spring Boot using Redis to dramatically improve application performance.

## Real-World Importance and Context

Redis is an in-memory data store that can cache frequently accessed data, reducing database load and improving response times. It's essential for:
- Frequently accessed data
- Session management
- Rate limiting
- Real-time analytics

## Detailed Step-by-Step Explanation

### Adding Redis Dependency

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-data-redis</artifactId>
</dependency>
```

### Redis Configuration

```java
@Configuration
@EnableCaching
public class RedisConfig {
    
    @Bean
    public RedisConnectionFactory redisConnectionFactory() {
        RedisStandaloneConfiguration config = new RedisStandaloneConfiguration();
        config.setHostName("localhost");
        config.setPort(6379);
        return new LettuceConnectionFactory(config);
    }
    
    @Bean
    public CacheManager cacheManager(RedisConnectionFactory connectionFactory) {
        // Configure cache with TTL
        RedisCacheConfiguration defaultConfig = RedisCacheConfiguration.defaultCacheConfig()
            .entryTtl(Duration.ofMinutes(10))
            .serializeKeysWith(RedisSerializationContext.SerializationPair
                .fromSerializer(new StringRedisSerializer()))
            .serializeValuesWith(RedisSerializationContext.SerializationPair
                .fromSerializer(new GenericJackson2JsonRedisSerializer()));
        
        Map<String, RedisCacheConfiguration> cacheConfigurations = new HashMap<>();
        cacheConfigurations.put("users", defaultConfig.entryTtl(Duration.ofMinutes(5)));
        cacheConfigurations.put("tasks", defaultConfig.entryTtl(Duration.ofMinutes(15)));
        
        return RedisCacheManager.builder(connectionFactory)
            .cacheDefaults(defaultConfig)
            .withInitialCacheConfigurations(cacheConfigurations)
            .build();
    }
}
```

### Using Cache Annotations

```java
@Service
public class TaskService {
    
    @Cacheable(value = "tasks", key = "#id")
    public Task getTaskById(Long id) {
        // Only executed if not in cache
        return taskRepository.findById(id).orElse(null);
    }
    
    @CachePut(value = "tasks", key = "#task.id")
    public Task updateTask(Task task) {
        return taskRepository.save(task);
    }
    
    @CacheEvict(value = "tasks", key = "#id")
    public void deleteTask(Long id) {
        taskRepository.deleteById(id);
    }
    
    @CacheEvict(value = "tasks", allEntries = true)
    public void clearTaskCache() {
        // Clear all task cache
    }
}
```

## Student Hands-On Exercises

### Exercise 1: Add Redis (Easy)
Configure Redis in your project

### Exercise 2: Cache Implementation (Medium)
Add caching to frequently accessed methods

---

## Summary

You've learned Redis caching basics and implementation in Spring Boot.
