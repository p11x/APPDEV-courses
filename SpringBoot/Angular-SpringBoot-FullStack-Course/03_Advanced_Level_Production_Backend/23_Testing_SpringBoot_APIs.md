# Testing Spring Boot APIs

## Concept Title and Overview

In this how lesson, you'll learn to write unit and integration tests for Spring Boot applications.

## Real-World Importance and Context

Testing ensures your code works correctly and helps prevent regressions when making changes.

## Detailed Step-by-Step Explanation

### Unit Testing with JUnit and Mockito

```java
@SpringBootTest
class EmployeeServiceTest {
    
    @Mock
    private EmployeeRepository employeeRepository;
    
    @InjectMocks
    private EmployeeService employeeService;
    
    @Test
    void testFindById() {
        Employee employee = new Employee("John", "john@example.com", "Developer", "555-1234", 75000.0);
        when(employeeRepository.findById(1L)).thenReturn(Optional.of(employee));
        
        Optional<Employee> result = employeeService.findById(1L);
        
        assertTrue(result.isPresent());
        assertEquals("John", result.get().getName());
    }
}
```

### Controller Testing with MockMvc

```java
@WebMvcTest(EmployeeController.class)
class EmployeeControllerTest {
    
    @Autowired
    private MockMvc mockMvc;
    
    @MockBean
    private EmployeeService employeeService;
    
    @Test
    void testGetAllEmployees() throws Exception {
        when(employeeService.findAll()).thenReturn(List.of(new Employee()));
        
        mockMvc.perform(get("/api/employees"))
            .andExpect(status().isOk())
            .andExpect(jsonPath("$").isArray());
    }
}
```

### Integration Testing

```java
@SpringBootTest
@AutoConfigureMockMvc
class EmployeeIntegrationTest {
    
    @Autowired
    private MockMvc mockMvc;
    
    @Test
    void testCreateEmployee() throws Exception {
        String json = "{\"name\":\"John\",\"email\":\"john@example.com\",\"jobTitle\":\"Developer\"}";
        
        mockMvc.perform(post("/api/employees")
                .contentType(MediaType.APPLICATION_JSON)
                .content(json))
            .andExpect(status().isCreated());
    }
}
```

## Student Hands-On Exercises

### Exercise 1: Unit Tests (Easy)
Write unit tests for your EmployeeService.

### Exercise 2: Controller Tests (Medium)
Write controller tests using MockMvc.

### Exercise 3: Integration Tests (Hard)
Write integration tests for CRUD operations.

---

## Summary

You've learned:
- Unit testing with JUnit and Mockito
- Controller testing with MockMvc
- Integration testing

---

**Next Lesson**: In the final lesson, we'll explore [Deployment](24_Deployment.md).
