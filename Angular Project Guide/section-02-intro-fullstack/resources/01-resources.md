# Section 2: Resources

## Official Documentation

### Frontend (Angular)
- [Angular Official Website](https://angular.io/)
- [Angular Documentation](https://angular.io/docs)
- [Angular Tutorial](https://angular.io/tutorial)

### Backend (Spring Boot)
- [Spring Boot Reference](https://spring.io/projects/spring-boot)
- [Spring Boot Guides](https://spring.io/guides)
- [Spring Data JPA](https://spring.io/projects/spring-data-jpa)

### Database (SQL Server)
- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/)
- [T-SQL Reference](https://docs.microsoft.com/en-us/sql/t-sql/language-reference)

---

## REST API Best Practices

### Naming Conventions
- Use nouns for resources (products, users, orders)
- Use plural forms (/products not /product)
- Use kebab-case (/product-categories)
- Keep URLs lowercase

### HTTP Status Codes

| Code | Meaning | Usage |
|------|---------|-------|
| 200 | OK | Successful GET, PUT, DELETE |
| 201 | Created | Successful POST |
| 204 | No Content | Successful DELETE |
| 400 | Bad Request | Invalid input |
| 401 | Unauthorized | Missing authentication |
| 404 | Not Found | Resource doesn't exist |
| 500 | Server Error | Internal error |

---

## JSON Reference

### Data Types
```json
{
  "string": "Hello",
  "number": 42,
  "decimal": 3.14,
  "boolean": true,
  "null": null,
  "array": [1, 2, 3],
  "object": {"key": "value"}
}
```

### Common Patterns
```json
// Paginated response
{
  "data": [...],
  "page": 1,
  "totalPages": 10,
  "totalItems": 100
}

// Error response
{
  "error": "Validation failed",
  "message": "Price must be positive",
  "field": "price"
}
```

---

## Architecture Patterns

### MVC Pattern
- **Model** - Data and business logic
- **View** - User interface (Angular components)
- **Controller** - Handles requests (Spring controllers)

### Repository Pattern
- Abstracts database operations
- Provides clean API for data access
- Used in both Spring Data JPA and Angular services

---

## Additional Reading

### Books
- "Angular Development with TypeScript" by Yakov Fain
- "Spring Boot in Action" by Craig Walls
- "SQL Server Execution Plans" by Grant Fritchey

### Online Courses
- Official Angular tutorials
- Spring Boot guides
- Microsoft Learn for SQL Server

---

## Tools Reference

| Tool | Purpose | Link |
|------|---------|------|
| Postman | API Testing | postman.com |
| VS Code | Code Editor | code.visualstudio.com |
| IntelliJ | Java IDE | jetbrains.com/idea |
| SSMS | Database Management | microsoft.com/sql-server |

---

## Next Steps

Now that you understand full-stack architecture:
- **Section 3:** Learn HTML & CSS for frontend basics
- **Section 4:** Master JavaScript
- **Section 5:** Add Bootstrap for styling
