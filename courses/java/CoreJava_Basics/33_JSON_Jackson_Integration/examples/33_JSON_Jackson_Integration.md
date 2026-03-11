# Java JSON Handling with Jackson

## Table of Contents
1. [Introduction to JSON](#introduction-to-json)
2. [Jackson ObjectMapper](#jackson-objectmapper)
3. [Jackson Annotations](#jackson-annotations)
4. [Working with Complex Objects](#working-with-complex-objects)
5. [Angular JSON Handling](#angular-json-handling)

---

## 1. Introduction to JSON

### JSON in Java/Angular

JSON is the primary data format used for communication between Java backend and Angular frontend.

```
┌─────────────────────────────────────────────────────────────┐
│                      JSON OVERVIEW                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Java Object ──────► JSON ──────► Angular Object              │
│                                                              │
│   {                                    {                      │
│     "id": 1,         Serialization     "id": 1,            │
│     "name": "John",  ──────────────►    "name": "John",     │
│     "email": "john@email.com"           "email": "john@   │
│   }                                    }                    │
│                                                              │
│   Key Points:                                                │
│   ✓ JSON is language-independent                            │
│   ✓ Easy to read and write                                   │
│   ✓ Maps directly to Java objects                            │
│   ✓ Angular HTTP uses JSON                                    │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Jackson ObjectMapper

### Basic Operations

```java
import com.fasterxml.jackson.databind.ObjectMapper;

// Create ObjectMapper
ObjectMapper mapper = new ObjectMapper();

// Java Object to JSON String
User user = new User(1, "John", "john@email.com");
String json = mapper.writeValueAsString(user);

// JSON String to Java Object
String jsonString = "{\"id\":1,\"name\":\"John\",\"email\":\"john@email.com\"}";
User user = mapper.readValue(jsonString, User.class);

// JSON to Map
Map<String, Object> map = mapper.readValue(jsonString, Map.class);

// JSON to List
String jsonArray = "[1,2,3,4,5]";
List<Integer> list = mapper.readValue(jsonArray, List.class);
```

### ObjectMapper Configuration

```java
ObjectMapper mapper = new ObjectMapper();

// Enable pretty printing
mapper.enable(SerializationFeature.INDENT_OUTPUT);

// Ignore unknown properties
mapper.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);

// Handle dates
mapper.registerModule(new JavaTimeModule());
mapper.disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS);
```

---

## 3. Jackson Annotations

### Class-Level Annotations

```java
import com.fasterxml.jackson.annotation.*;

@JsonIgnoreProperties({"password", "secret"})
class User {
    private int id;
    private String name;
    private String email;
    private String password;  // Will be ignored
    
    @JsonIgnore
    private String secret;    // Will be ignored
    
    // @JsonProperty - Rename property
    @JsonProperty("user_name")
    private String name;
    
    // @JsonInclude - Only include non-null
    @JsonInclude(JsonInclude.Include.NON_NULL)
    private String nickname;
}

// @JsonCreator - Custom constructor
class User {
    @JsonCreator
    public User(@JsonProperty("id") int id, @JsonProperty("name") String name) {
        this.id = id;
        this.name = name;
    }
}
```

### Method-Level Annotations

```java
class User {
    private int id;
    private String name;
    
    @JsonGetter
    public int getId() { return id; }
    
    @JsonSetter
    public void setId(int id) { this.id = id; }
    
    // Ignore in serialization
    @JsonIgnore
    public String getFullName() { return name; }
}
```

---

## 4. Working with Complex Objects

### Nested Objects

```java
class User {
    private int id;
    private String name;
    private Address address;  // Nested object
    
    // Getters and setters
}

class Address {
    private String street;
    private String city;
    private String zipCode;
    
    // Getters and setters
}

// Serialization
User user = new User();
user.setId(1);
user.setName("John");
user.setAddress(new Address("123 Main St", "NYC", "10001"));

String json = mapper.writeValueAsString(user);
// {"id":1,"name":"John","address":{"street":"123 Main St","city":"NYC","zipCode":"10001"}}
```

### Lists and Arrays

```java
class User {
    private int id;
    private String name;
    private List<String> roles;
    private PhoneNumber[] phones;
    
    // Getters and setters
}

class PhoneNumber {
    private String type;
    private String number;
    
    // Getters and setters
}

// Usage
List<String> roles = Arrays.asList("ADMIN", "USER");
PhoneNumber[] phones = { new PhoneNumber("mobile", "123-456") };
User user = new User(1, "John", roles, phones);

String json = mapper.writeValueAsString(user);
```

### Maps

```java
class User {
    private int id;
    private String name;
    private Map<String, String> metadata;
    
    // Getters and setters
}

// Usage
Map<String, String> metadata = new HashMap<>();
metadata.put("department", "IT");
metadata.put("location", "NYC");

User user = new User();
user.setId(1);
user.setName("John");
user.setMetadata(metadata);

String json = mapper.writeValueAsString(user);
// {"id":1,"name":"John","metadata":{"department":"IT","location":"NYC"}}
```

---

## 5. Angular JSON Handling

### Angular HTTP Response

```typescript
// Angular automatically parses JSON
this.http.get<User>('/api/users/1').subscribe(user => {
  console.log(user.name);  // TypeScript knows the type
});

// Interface matches Java class
interface User {
  id: number;
  name: string;
  email: string;
  address?: Address;
}

interface Address {
  street: string;
  city: string;
  zipCode: string;
}
```

### JSON Date Handling

```java
// Java - Use LocalDateTime
class User {
    private LocalDateTime createdAt;
    private LocalDate birthDate;
}

// Java - Configure Jackson
ObjectMapper mapper = new ObjectMapper();
mapper.registerModule(new JavaTimeModule());

// Results in ISO-8601 format
// {"createdAt":"2024-01-15T10:30:00","birthDate":"2000-05-20"}
```

---

## Code Examples

### Complete Demo

```java
import com.fasterxml.jackson.databind.*;
import com.fasterxml.jackson.annotation.*;
import java.util.*;
import java.time.*;

class Address {
    private String street;
    private String city;
    private String zipCode;
    
    public Address() {}
    public Address(String street, String city, String zipCode) {
        this.street = street;
        this.city = city;
        this.zipCode = zipCode;
    }
    
    public String getStreet() { return street; }
    public void setStreet(String street) { this.street = street; }
    public String getCity() { return city; }
    public void setCity(String city) { this.city = city; }
    public String getZipCode() { return zipCode; }
    public void setZipCode(String zipCode) { this.zipCode = zipCode; }
}

@JsonIgnoreProperties({"password"})
class User {
    private int id;
    private String name;
    private String email;
    private String password;  // Will be ignored
    private Address address;
    private List<String> roles;
    private Map<String, String> preferences;
    
    public User() {}
    
    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public String getPassword() { return password; }
    public void setPassword(String password) { this.password = password; }
    
    public Address getAddress() { return address; }
    public void setAddress(Address address) { this.address = address; }
    
    public List<String> getRoles() { return roles; }
    public void setRoles(List<String> roles) { this.roles = roles; }
    
    public Map<String, String> getPreferences() { return preferences; }
    public void setPreferences(Map<String, String> preferences) { this.preferences = preferences; }
}

public class JacksonDemo {
    public static void main(String[] args) throws Exception {
        ObjectMapper mapper = new ObjectMapper();
        mapper.enable(SerializationFeature.INDENT_OUTPUT);
        
        // Create user object
        User user = new User();
        user.setId(1);
        user.setName("John Doe");
        user.setEmail("john@example.com");
        user.setPassword("secret123");  // Will be ignored in JSON
        
        user.setAddress(new Address("123 Main St", "New York", "10001"));
        
        user.setRoles(Arrays.asList("ADMIN", "USER"));
        
        Map<String, String> prefs = new HashMap<>();
        prefs.put("theme", "dark");
        prefs.put("language", "en");
        user.setPreferences(prefs);
        
        // Serialize to JSON
        System.out.println("=== SERIALIZATION ===");
        String json = mapper.writeValueAsString(user);
        System.out.println(json);
        
        // Deserialize from JSON
        System.out.println("\n=== DESERIALIZATION ===");
        String jsonInput = """
            {
              "id": 2,
              "name": "Jane Smith",
              "email": "jane@example.com",
              "address": {
                "street": "456 Oak Ave",
                "city": "Los Angeles",
                "zipCode": "90001"
              },
              "roles": ["USER"],
              "preferences": {
                "theme": "light"
              }
            }
            """;
        
        User newUser = mapper.readValue(jsonInput, User.class);
        System.out.println("Name: " + newUser.getName());
        System.out.println("City: " + newUser.getAddress().getCity());
        System.out.println("Roles: " + newUser.getRoles());
    }
}
```

---

## Summary

### Jackson Key Features

1. **ObjectMapper** - Main class for JSON processing
2. **@JsonIgnore** - Ignore properties
3. **@JsonProperty** - Rename properties
4. **@JsonIgnoreProperties** - Ignore multiple properties
5. **@JsonCreator** - Custom deserialization
6. **@JsonGetter/@JsonSetter** - Custom getters/setters

### Angular Integration

1. Angular HttpClient automatically parses JSON
2. Use TypeScript interfaces matching Java classes
3. Handle dates consistently (ISO-8601)
4. Use HttpInterceptor for authentication tokens

---

*JSON Handling Complete!*
