# Backend Design Patterns for Java

## Table of Contents
1. [Repository Pattern](#repository-pattern)
2. [DAO Pattern](#dao-pattern)
3. [DTO Pattern](#dto-pattern)
4. [Service Layer Pattern](#service-layer-pattern)
5. [MVC Pattern](#mvc-pattern)
6. [Dependency Injection](#dependency-injection)

---

## 1. Repository Pattern

### What is the Repository Pattern?

An abstraction layer between business logic and data access.

```
┌─────────────────────────────────────────────────────────────┐
│                   REPOSITORY PATTERN                          │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   ┌─────────────────┐                                      │
│   │   Service      │  ← Uses Repository Interface          │
│   └────────┬────────┘                                      │
│            │                                                │
│   ┌────────▼────────┐                                      │
│   │ IRepository<T> │  ← Interface (abstraction)           │
│   └────────┬────────┘                                      │
│            │                                                │
│   ┌────────▼────────┐                                      │
│   │  Repository    │  ← Implementation                    │
│   │  (JDBC/Hibernate)                                      │
│   └─────────────────┘                                      │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Implementation

```java
// Repository interface
interface Repository<T> {
    T findById(int id);
    List<T> findAll();
    void save(T entity);
    void delete(int id);
}

// Concrete implementation
class UserRepository implements Repository<User> {
    @Override
    public User findById(int id) {
        // Database query
        return new User();
    }
    
    @Override
    public List<User> findAll() {
        // Database query
        return new ArrayList<>();
    }
}
```

---

## 2. DAO Pattern

### Data Access Object

```java
// DAO Interface
interface UserDAO {
    User getUserById(int id);
    List<User> getAllUsers();
    void insertUser(User user);
    void updateUser(User user);
    void deleteUser(int id);
}

// DAO Implementation
class UserDAOImpl implements UserDAO {
    private Connection conn;
    
    @Override
    public User getUserById(int id) {
        // JDBC code
        return new User();
    }
    // ... other methods
}
```

---

## 3. DTO Pattern

### Data Transfer Object

```java
// DTO - Transfer data between layers
class UserDTO {
    private int id;
    private String name;
    private String email;
    
    // Getters and setters
}

// Entity - Database representation
@Entity
class UserEntity {
    @Id
    private int id;
    private String name;
    private String password;
    // ...
}

// Conversion
class UserMapper {
    public static UserDTO toDTO(UserEntity entity) {
        // Map entity to DTO
    }
    
    public static UserEntity toEntity(UserDTO dto) {
        // Map DTO to entity
    }
}
```

---

## 4. Service Layer Pattern

### Business Logic Layer

```java
// Service Interface
interface UserService {
    UserDTO getUserById(int id);
    List<UserDTO> getAllUsers();
    void createUser(UserDTO userDTO);
}

// Service Implementation
class UserServiceImpl implements UserService {
    private UserDAO userDAO;
    
    public UserServiceImpl(UserDAO userDAO) {
        this.userDAO = userDAO;
    }
    
    @Override
    public UserDTO getUserById(int id) {
        UserEntity entity = userDAO.getUserById(id);
        return UserMapper.toDTO(entity);
    }
}
```

---

## 5. MVC Pattern

### Model-View-Controller

```
┌─────────────────────────────────────────────────────────────┐
│                       MVC PATTERN                             │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   ┌─────────┐      ┌─────────┐      ┌─────────┐           │
│   │  Model  │◄─────│         │─────►│  View   │           │
│   │  (Data) │      │Controller│      │  (UI)   │           │
│   └─────────┘      └─────────┘      └─────────┘           │
│                          │                                  │
│   - Entities      - Handles        - JSP/FreeMarker       │
│   - Business       Request         - JSON Response        │
│     Logic         Routing                                │
│   - Validation    - Calls Service                         │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 6. Dependency Injection

### What is DI?

Dependency Injection is a technique where an object receives other objects it depends on.

```java
// Constructor Injection
class UserService {
    private UserRepository userRepository;
    
    public UserService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }
}

// Usage
UserRepository repository = new UserRepository();
UserService service = new UserService(repository);
```

---

## Code Examples

### BackendPatternsDemo

```java
// Entity - Database model
class User {
    private int id;
    private String username;
    private String email;
    private String password;
    
    // Getters and setters
}

// DTO - For API responses
class UserDTO {
    private int id;
    private String username;
    private String email;
    
    public static UserDTO fromEntity(User user) {
        UserDTO dto = new UserDTO();
        dto.setId(user.getId());
        dto.setUsername(user.getUsername());
        dto.setEmail(user.getEmail());
        return dto;
    }
}

// DAO - Data access
class UserDAO {
    public User findById(int id) {
        // Simulated DB call
        return new User();
    }
}

// Service - Business logic
class UserService {
    private UserDAO userDAO;
    
    public UserService(UserDAO userDAO) {
        this.userDAO = userDAO;
    }
    
    public UserDTO getUserById(int id) {
        User user = userDAO.findById(id);
        return UserDTO.fromEntity(user);
    }
}

// Controller - API endpoint
class UserController {
    private UserService userService;
    
    public UserController(UserService userService) {
        this.userService = userService;
    }
    
    // Simulating GET /users/1
    public String getUser(int id) {
        UserDTO user = userService.getUserById(id);
        return user.toJson();
    }
}

// Demo
public class BackendPatternsDemo {
    public static void main(String[] args) {
        System.out.println("=== BACKEND DESIGN PATTERNS DEMO ===\n");
        
        // Build dependency chain
        UserDAO userDAO = new UserDAO();
        UserService userService = new UserService(userDAO);
        UserController userController = new UserController(userService);
        
        // Simulate API call
        System.out.println("GET /api/users/1");
        String response = userController.getUser(1);
        System.out.println("Response: " + response);
    }
}
```

---

## Summary

### Pattern Summary

| Pattern | Purpose |
|---------|---------|
| Repository | Abstract data access |
| DAO | Database operations |
| DTO | Data transfer between layers |
| Service | Business logic |
| MVC | Request handling |
| DI | Dependency management |

---

*Backend Patterns Complete!*
