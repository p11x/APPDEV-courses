# Pagination and Sorting

## Concept Title and Overview

In this lesson, you'll learn how to implement pagination and sorting in Spring Boot APIs, essential for handling large datasets efficiently.

## Real-World Importance and Context

Loading thousands of records at once is inefficient. Pagination splits data into pages; sorting allows ordering results.

## Detailed Step-by-Step Explanation

### Pageable Interface

Spring Data provides `Pageable` for pagination:

```java
Pageable pageable = PageRequest.of(page, size);
Pageable pageableWithSort = PageRequest.of(page, size, Sort.by("name").descending());
```

### Repository Methods

```java
public interface EmployeeRepository extends JpaRepository<Employee, Long> {
    Page<Employee> findAll(Pageable pageable);
    
    Page<Employee> findByDepartment(String department, Pageable pageable);
}
```

### Service Layer

```java
public Page<Employee> getEmployees(int page, int size, String sortBy, String sortDir) {
    Sort sort = sortDir.equalsIgnoreCase("desc") 
        ? Sort.by(sortBy).descending() 
        : Sort.by(sortBy).ascending();
    
    Pageable pageable = PageRequest.of(page, size, sort);
    return employeeRepository.findAll(pageable);
}
```

### Controller

```java
@GetMapping
public ResponseEntity<Page<Employee>> getEmployees(
        @RequestParam(defaultValue = "0") int page,
        @RequestParam(defaultValue = "10") int size,
        @RequestParam(defaultValue = "id") String sortBy,
        @RequestParam(defaultValue = "asc") String sortDir) {
    
    Page<Employee> employees = employeeService.getEmployees(page, size, sortBy, sortDir);
    return ResponseEntity.ok(employees);
}
```

## Angular Integration

```typescript
getEmployees(page: number, size: number): Observable<Page<Employee>> {
  return this.http.get<Page<Employee>>(`${this.apiUrl}?page=${page}&size=${size}`);
}
```

## Student Hands-On Exercises

### Exercise 1: Add Pagination (Easy)
Add pagination to your employee endpoint.

### Exercise 2: Add Sorting (Medium)
Add sorting capability.

### Exercise 3: Angular Paginator (Hard)
Create an Angular component with pagination controls.

---

## Summary

You've learned:
- Pageable interface
- Page and Sort
- Repository and service implementation
- Angular handling

---

**Next Lesson**: In the next lesson, we'll explore [File Upload API](20_File_Upload_API.md).
