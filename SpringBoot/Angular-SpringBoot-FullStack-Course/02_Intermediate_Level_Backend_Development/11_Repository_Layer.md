# Repository Layer

## Concept Title and Overview

The Repository Layer is where all database operations happen. In this lesson, you'll learn about CrudRepository, JpaRepository, query method naming conventions, and custom queries.

## Real-World Importance and Context

The repository pattern abstracts database operations. Your service layer shouldn't know whether it's working with MySQL, PostgreSQL, or any other database. The repository handles all that complexity.

## Detailed Step-by-Step Explanation

### CrudRepository and JpaRepository

Spring Data provides two main repository interfaces:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    REPOSITORY INTERFACES                                  │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  CrudRepository<Entity, ID>                                            │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ • save(entity) - Save or update                                │   │
│  │ • findById(id) - Find by primary key                          │   │
│  │ • existsById(id) - Check if exists                            │   │
│  │ • findAll() - Find all entities                               │   │
│  │ • count() - Count entities                                     │   │
│  │ • deleteById(id) - Delete by ID                               │   │
│  │ • deleteAll() - Delete all                                    │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  JpaRepository<Entity, ID> extends CrudRepository                      │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ • saveAndFlush(entity) - Save and flush immediately           │   │
│  │ • findAll() - Find all (with sorting and pagination)          │   │
│  │ • flush() - Force flush                                       │   │
│  │ • deleteInBatch(entities) - Delete batch                      │   │
│  │ • getOne(id) - Get reference (lazy)                           │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  Recommendation: Use JpaRepository for more features                   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Built-in CRUD Methods

```java
public interface EmployeeRepository extends JpaRepository<Employee, Long> {
    // Inherited from JpaRepository -> CrudRepository:
    
    // Save entity (insert or update)
    Employee save(Employee employee);
    
    // Find by ID (returns Optional)
    Optional<Employee> findById(Long id);
    
    // Check if exists
    boolean existsById(Long id);
    
    // Find all
    List<Employee> findAll();
    
    // Count
    long count();
    
    // Delete
    void deleteById(Long id);
    void delete(Employee employee);
    void deleteAll();
}
```

### Query Method Naming Conventions

Spring Data JPA can automatically implement queries from method names:

```java
public interface EmployeeRepository extends JpaRepository<Employee, Long> {
    
    // FIND METHODS
    // Find by property
    List<Employee> findByName(String name);
    List<Employee> findByDepartment(String department);
    
    // Find with AND
    List<Employee> findByNameAndDepartment(String name, String department);
    
    // Find with OR
    List<Employee> findByDepartmentOrRole(String department, String role);
    
    // FIND WITH COMPARISON
    // Greater than
    List<Employee> findBySalaryGreaterThan(Double salary);
    
    // Less than
    List<Employee> findBySalaryLessThan(Double salary);
    
    // Between
    List<Employee> findBySalaryBetween(Double min, Double max);
    
    // LIKE queries
    List<Employee> findByNameContaining(String name);
    List<Employee> findByNameStartingWith(String prefix);
    List<Employee> findByNameEndingWith(String suffix);
    
    // Boolean
    List<Employee> findByActiveTrue();
    List<Employee> findByActiveFalse();
    
    // NULL checks
    List<Employee> findByDepartmentNull();
    List<Employee> findByDepartmentNotNull();
    
    // ORDERING
    List<Employee> findByDepartmentOrderBySalaryDesc(String department);
    List<Employee> findByRoleOrderByNameAsc(String role);
    
    // LIMITING
    Employee findFirstByOrderByCreatedAtDesc();
    List<Employee> findTop5ByActiveTrue();
    
    // COUNT
    long countByDepartment(String department);
    boolean existsByEmail(String email);
}
```

### @Query Annotation for Custom JPQL

When method names get too complex, use @Query:

```java
public interface EmployeeRepository extends JpaRepository<Employee, Long> {
    
    // JPQL Query
    @Query("SELECT e FROM Employee e WHERE e.department = :dept")
    List<Employee> findByDepartmentJpql(@Param("dept") String department);
    
    // JPQL with parameters
    @Query("SELECT e FROM Employee e WHERE e.salary >= :min AND e.salary <= :max")
    List<Employee> findBySalaryRange(@Param("min") Double min, @Param("max") Double max);
    
    // Native SQL Query
    @Query(value = "SELECT * FROM employees WHERE salary > :salary", nativeQuery = true)
    List<Employee> findHighEarners(@Param("salary") Double salary);
    
    // Named parameters
    @Query("SELECT e FROM Employee e WHERE e.name = :name AND e.role = :role")
    Employee findByNameAndRole(@Param("name") String name, @Param("role") String role);
    
    // UPDATE Query
    @Modifying
    @Query("UPDATE Employee e SET e.active = false WHERE e.salary < :salary")
    int deactivateLowEarners(@Param("salary") Double salary);
    
    // DELETE Query
    @Modifying
    @Query("DELETE FROM Employee e WHERE e.department = :dept")
    int deleteByDepartment(@Param("dept") String department);
}
```

### When to Use Each Repository Type

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    REPOSITORY SELECTION GUIDE                            │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  Use CrudRepository when:                                                │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ • Simple CRUD operations only                                   │   │
│  │ • No need for pagination/sorting                               │   │
│  │ • Lightweight data access                                      │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  Use JpaRepository when:                                                │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ • Need pagination and sorting                                   │   │
│  │ • Want batch operations                                        │   │
│  │ • Need to flush/clear persistence context                      │   │
│  │ • Most common choice for JPA projects                          │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  Use PagingAndSortingRepository when:                                   │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ • Need pagination without JPA-specific features                 │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

## Complete Implementation Example

```java
package com.example.demo.repository;

import com.example.demo.model.Employee;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface EmployeeRepository extends JpaRepository<Employee, Long> {
    
    // ─────────────────────────────────────────────────────────────
    // Query Methods
    // ─────────────────────────────────────────────────────────────
    
    Optional<Employee> findByEmail(String email);
    
    List<Employee> findByDepartment(String department);
    
    List<Employee> findByRole(String role);
    
    List<Employee> findByActive(boolean active);
    
    // Combined conditions
    List<Employee> findByDepartmentAndRole(String department, String role);
    
    List<Employee> findByDepartmentAndActive(String department, boolean active);
    
    // Comparison
    List<Employee> findBySalaryGreaterThan(Double salary);
    
    List<Employee> findBySalaryBetween(Double minSalary, Double maxSalary);
    
    // Like queries
    List<Employee> findByNameContaining(String namePart);
    
    // Ordering
    List<Employee> findByDepartmentOrderBySalaryDesc(String department);
    
    // Count
    long countByDepartment(String department);
    
    long countByRole(String role);
    
    // Existence check
    boolean existsByEmail(String email);
    
    // ─────────────────────────────────────────────────────────────
    // Custom Queries (JPQL)
    // ─────────────────────────────────────────────────────────────
    
    @Query("SELECT e FROM Employee e WHERE e.salary >= :minSalary")
    List<Employee> findHighEarners(@Param("minSalary") Double minSalary);
    
    @Query("SELECT e FROM Employee e WHERE e.department = :dept ORDER BY e.salary DESC")
    List<Employee> findByDepartmentSorted(@Param("dept") String department);
    
    @Query("SELECT e FROM Employee e WHERE e.name LIKE %:name%")
    List<Employee> searchByName(@Param("name") String name);
    
    // Aggregate functions
    @Query("SELECT AVG(e.salary) FROM Employee e WHERE e.department = :dept")
    Double getAverageSalaryByDepartment(@Param("dept") String department);
    
    @Query("SELECT MAX(e.salary) FROM Employee e")
    Double getHighestSalary();
    
    @Query("SELECT e.department, COUNT(e) FROM Employee e GROUP BY e.department")
    List<Object[]> countByDepartmentGrouped();
    
    // ─────────────────────────────────────────────────────────────
    // Modify Queries
    // ─────────────────────────────────────────────────────────────
    
    @Query("UPDATE Employee e SET e.active = false WHERE e.salary < :salary")
    void deactivateLowPaidEmployees(@Param("salary") Double salary);
    
    // ─────────────────────────────────────────────────────────────
    // Native Queries
    // ─────────────────────────────────────────────────────────────
    
    @Query(value = "SELECT * FROM employees WHERE salary > :salary ORDER BY salary DESC", 
           nativeQuery = true)
    List<Employee> findHighEarnersNative(@Param("salary") Double salary);
}
```

## Service Layer Integration

```java
package com.example.demo.service;

import com.example.demo.model.Employee;
import com.example.demo.repository.EmployeeRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

@Service
@Transactional
public class EmployeeService {
    
    private final EmployeeRepository employeeRepository;
    
    public EmployeeService(EmployeeRepository employeeRepository) {
        this.employeeRepository = employeeRepository;
    }
    
    // CREATE
    public Employee create(Employee employee) {
        if (employeeRepository.existsByEmail(employee.getEmail())) {
            throw new IllegalArgumentException("Email already exists");
        }
        return employeeRepository.save(employee);
    }
    
    // READ ALL
    @Transactional(readOnly = true)
    public List<Employee> findAll() {
        return employeeRepository.findAll();
    }
    
    // READ BY ID
    @Transactional(readOnly = true)
    public Optional<Employee> findById(Long id) {
        return employeeRepository.findById(id);
    }
    
    // READ BY DEPARTMENT
    @Transactional(readOnly = true)
    public List<Employee> findByDepartment(String department) {
        return employeeRepository.findByDepartment(department);
    }
    
    // SEARCH
    @Transactional(readOnly = true)
    public List<Employee> searchByName(String name) {
        return employeeRepository.findByNameContaining(name);
    }
    
    // CUSTOM QUERY
    @Transactional(readOnly = true)
    public List<Employee> getHighEarners(Double minSalary) {
        return employeeRepository.findHighEarners(minSalary);
    }
    
    // AGGREGATE
    @Transactional(readOnly = true)
    public Double getAverageSalary(String department) {
        return employeeRepository.getAverageSalaryByDepartment(department);
    }
    
    // UPDATE
    public Optional<Employee> update(Long id, Employee updated) {
        return employeeRepository.findById(id)
            .map(employee -> {
                employee.setName(updated.getName());
                employee.setEmail(updated.getEmail());
                employee.setDepartment(updated.getDepartment());
                employee.setSalary(updated.getSalary());
                return employeeRepository.save(employee);
            });
    }
    
    // DELETE
    public boolean delete(Long id) {
        if (employeeRepository.existsById(id)) {
            employeeRepository.deleteById(id);
            return true;
        }
        return false;
    }
}
```

## Student Hands-On Exercises

### Exercise 1: Basic Queries (Easy)
Create a UserRepository with query methods for:
- findByUsername
- findByEmail
- findByRole
- findByActive

### Exercise 2: Complex Queries (Medium)
Add complex query methods:
- findByRoleAndActive
- findByCreatedAtAfter
- findBySalaryBetween

### Exercise 3: Custom JPQL (Medium)
Add custom JPQL queries:
- Get average salary by department
- Find users with names containing a string

### Exercise 4: Aggregation (Hard)
Create queries for:
- Count users by role
- Get highest salary
- Group by department with counts

### Exercise 5: Native Queries (Hard)
Convert a complex JPQL query to native SQL and compare the results.

---

## Summary

In this lesson, you've learned:
- CrudRepository vs JpaRepository
- Built-in CRUD methods
- Query method naming conventions
- @Query annotation for custom JPQL
- When to use each repository type

You can now create powerful database queries! In the next lesson, we'll build a complete CRUD API project.

---

**Next Lesson**: In the next lesson, we'll build a [Complete CRUD API Project](12_CRUD_API_Complete_Project.md) integrating everything we've learned.
