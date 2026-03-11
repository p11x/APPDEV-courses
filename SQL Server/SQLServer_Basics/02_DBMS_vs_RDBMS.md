# DBMS vs RDBMS

## What is DBMS?

**DBMS (Database Management System)** is software that creates and manages databases. It provides an interface to store and retrieve data without knowing the internal details of how data is stored.

### Characteristics of DBMS:
- Stores data in flat files
- No relationship between data
- Limited data redundancy
- No strict rules for data integrity
- Example: File systems, XML databases

## What is RDBMS?

**RDBMS (Relational Database Management System)** is an advanced version of DBMS that stores data in tables with relationships between them. It uses SQL to manage data.

### Characteristics of RDBMS:
- Stores data in tables (rows and columns)
- Data is related through keys
- Enforces data integrity rules
- Supports relationships (one-to-one, one-to-many, many-to-many)
- Example: SQL Server, MySQL, Oracle, PostgreSQL

## Key Differences

| Feature | DBMS | RDBMS |
|---------|------|-------|
| Data Structure | Flat files | Tables |
| Relationships | No | Yes |
| Data Integrity | Limited | Enforced |
| Language | Proprietary | SQL (Standard) |
| Scalability | Limited | High |
| Example | XML, FoxPro | SQL Server, MySQL |

## Example Comparison

### DBMS Approach (File-based)
```
Student.txt -> "John,20,Computer Science"
Student.txt -> "Jane,21,Mathematics"
```

### RDBMS Approach (Table-based)
```sql
Students Table:
| ID | Name  | Age | Department       |
|----|-------|-----|------------------|
| 1  | John  | 20  | Computer Science |
| 2  | Jane  | 21  | Mathematics      |
```

## Why RD Preferred?

BMS is1. **Data Relationships**: Tables can be connected using keys
2. **Data Integrity**: Constraints ensure data accuracy
3. **SQL Standard**: Universal query language
4. **ACID Properties**: Atomicity, Consistency, Isolation, Durability

## Key Takeaways

- DBMS is a basic database system
- RDBMS is a relational database system with table relationships
- SQL Server is an **RDBMS**
- RDBMS provides better data management and integrity

---

*This topic should take about 5 minutes to explain in class.*
