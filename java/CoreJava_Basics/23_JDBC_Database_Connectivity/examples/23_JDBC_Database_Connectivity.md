# Java JDBC and Database Connectivity

## Table of Contents
1. [Introduction to JDBC](#introduction-to-jdbc)
2. [JDBC Drivers](#jdbc-drivers)
3. [Database Connections](#database-connections)
4. [CRUD Operations](#crud-operations)
5. [PreparedStatement](#preparedstatement)
6. [Transaction Management](#transaction-management)
7. [Code Examples](#code-examples)

---

## 1. Introduction to JDBC

### What is JDBC?

**JDBC (Java Database Connectivity)** is an API for connecting and executing queries with databases.

```
┌─────────────────────────────────────────────────────────────┐
│                      JDBC ARCHITECTURE                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   ┌─────────────────────────────────────────────────────┐  │
│   │              Java Application                        │  │
│   └─────────────────────┬───────────────────────────────┘  │
│                         │ JDBC API                          │
│   ┌─────────────────────▼───────────────────────────────┐  │
│   │              JDBC Driver Manager                     │  │
│   └─────────────────────┬───────────────────────────────┘  │
│                         │                                    │
│   ┌─────────────────────▼───────────────────────────────┐  │
│   │              JDBC Driver (Specific to DB)            │  │
│   └─────────────────────┬───────────────────────────────┘  │
│                         │                                    │
│   ┌─────────────────────▼───────────────────────────────┐  │
│   │              Database (MySQL, Oracle, etc.)          │  │
│   └─────────────────────────────────────────────────────┘  │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. JDBC Drivers

### Types of JDBC Drivers

| Type | Description | Performance |
|------|-------------|-------------|
| Type 1 | JDBC-ODBC Bridge | Slow |
| Type 2 | Native-API | Medium |
| Type 3 | Network Protocol | Fast |
| Type 4 | Native Protocol (Pure Java) | Fastest |

---

## 3. Database Connections

### Basic Connection Steps

```java
// 1. Load driver
Class.forName("com.mysql.cj.jdbc.Driver");

// 2. Create connection
Connection conn = DriverManager.getConnection(
    "jdbc:mysql://localhost:3306/mydb",
    "username",
    "password"
);

// 3. Create statement
Statement stmt = conn.createStatement();

// 4. Execute query
ResultSet rs = stmt.executeQuery("SELECT * FROM users");

// 5. Process results
while (rs.next()) {
    System.out.println(rs.getString("name"));
}

// 6. Close resources
rs.close();
stmt.close();
conn.close();
```

---

## 4. CRUD Operations

### Create (INSERT)

```java
String sql = "INSERT INTO users (name, email) VALUES ('John', 'john@example.com')";
int rows = stmt.executeUpdate(sql);
```

### Read (SELECT)

```java
String sql = "SELECT * FROM users WHERE id = 1";
ResultSet rs = stmt.executeQuery(sql);
if (rs.next()) {
    String name = rs.getString("name");
}
```

### Update

```java
String sql = "UPDATE users SET name = 'Jane' WHERE id = 1";
int rows = stmt.executeUpdate(sql);
```

### Delete

```java
String sql = "DELETE FROM users WHERE id = 1";
int rows = stmt.executeUpdate(sql);
```

---

## 5. PreparedStatement

### Why Use PreparedStatement?

- **Prevents SQL Injection**
- **Better Performance** (compiled query)
- **Parameter Binding**

```java
String sql = "INSERT INTO users (name, email) VALUES (?, ?)";
PreparedStatement pstmt = conn.prepareStatement(sql);

pstmt.setString(1, "John");
pstmt.setString(2, "john@example.com");
pstmt.executeUpdate();
```

---

## 6. Transaction Management

### ACID Properties

```java
conn.setAutoCommit(false);

try {
    // Multiple operations
    stmt.executeUpdate("UPDATE accounts SET balance = balance - 100 WHERE id = 1");
    stmt.executeUpdate("UPDATE accounts SET balance = balance + 100 WHERE id = 2");
    
    conn.commit();  // Save changes
} catch (Exception e) {
    conn.rollback();  // Undo changes
    e.printStackTrace();
}
```

---

## 7. Code Examples

### JDBCBasicDemo

```java
import java.sql.*;

/**
 * Simulated JDBC Demo (without actual database)
 * Shows the pattern of JDBC operations
 */
public class JDBCBasicDemo {
    
    // Simulated database in memory
    private static String[][] users = {
        {"1", "John Doe", "john@example.com"},
        {"2", "Jane Smith", "jane@example.com"},
        {"3", "Bob Wilson", "bob@example.com"}
    };
    
    public static void main(String[] args) {
        System.out.println("=== JDBC CONCEPTS DEMO ===\n");
        
        System.out.println("In real JDBC, you would:");
        
        // 1. Load Driver
        System.out.println("1. Load Driver: Class.forName(\"com.mysql.cj.jdbc.Driver\")");
        
        // 2. Get Connection
        System.out.println("\n2. Get Connection:");
        System.out.println("   Connection conn = DriverManager.getConnection(");
        System.out.println("       \"jdbc:mysql://localhost:3306/mydb\",");
        System.out.println("       \"username\", \"password\");");
        
        // 3. Create Statement
        System.out.println("\n3. Create Statement:");
        System.out.println("   Statement stmt = conn.createStatement();");
        
        // 4. Execute Query
        System.out.println("\n4. Execute Query:");
        System.out.println("   ResultSet rs = stmt.executeQuery(\"SELECT * FROM users\");");
        
        // 5. Process Results
        System.out.println("\n5. Process Results:");
        System.out.println("   while (rs.next()) {");
        System.out.println("       String name = rs.getString(\"name\");");
        System.out.println("   }");
        
        // Demo with simulated data
        System.out.println("\n=== Simulated Results ===");
        System.out.println("ID | Name           | Email");
        System.out.println("---|----------------|----------------");
        for (String[] user : users) {
            System.out.println(user[0] + " | " + user[1] + " | " + user[2]);
        }
        
        // PreparedStatement example
        System.out.println("\n=== PreparedStatement Example ===");
        System.out.println("String sql = \"INSERT INTO users (name, email) VALUES (?, ?)\";");
        System.out.println("PreparedStatement pstmt = conn.prepareStatement(sql);");
        System.out.println("pstmt.setString(1, \"New User\");");
        System.out.println("pstmt.setString(2, \"new@example.com\");");
        
        // Transaction example
        System.out.println("\n=== Transaction Example ===");
        System.out.println("conn.setAutoCommit(false);");
        System.out.println("try {");
        System.out.println("    stmt.executeUpdate(\"UPDATE accounts SET balance = balance - 100\");");
        System.out.println("    stmt.executeUpdate(\"UPDATE accounts SET balance = balance + 100\");");
        System.out.println("    conn.commit();");
        System.out.println("} catch (Exception e) {");
        System.out.println("    conn.rollback();");
        System.out.println("}");
    }
}
```

---

## Summary

### Key Takeaways

1. **JDBC API** - Standard Java API for database access
2. **Driver Manager** - Manages JDBC drivers
3. **Connection** - Represents database connection
4. **Statement/PreparedStatement** - Executes SQL
5. **ResultSet** - Contains query results
6. **Transactions** - Group operations with commit/rollback

---

*JDBC Complete!*
