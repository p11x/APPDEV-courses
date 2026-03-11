# SQL Syntax

## Structure of a SQL Statement

SQL statements follow a specific structure. Most statements have these main parts:

```sql
SELECT column_name
FROM table_name
WHERE condition;
```

### Key Components:
1. **Keywords**: Reserved words with special meaning
2. **Identifiers**: Names of tables, columns, databases
3. **Operators**: For comparisons and calculations
4. **Clauses**: Components that define what to do

## Keywords

Keywords are reserved words that have special meaning in SQL. They cannot be used as identifiers.

### Common SQL Keywords:

| Keyword | Purpose |
|---------|---------|
| SELECT | Retrieve data |
| FROM | Specify table |
| WHERE | Filter conditions |
| INSERT | Add data |
| UPDATE | Modify data |
| DELETE | Remove data |
| CREATE | Make new objects |
| DROP | Delete objects |
| TABLE | Define table structure |

## Identifiers

Identifiers are names given to database objects:

- **Table names**: Students, Products, Orders
- **Column names**: Name, Age, Salary
- **Database names**: CollegeDB, ShopDB

### Naming Rules:
- Must start with a letter
- Can include letters, numbers, and underscore
- Cannot be a reserved keyword
- Case insensitive in SQL Server

## Case Sensitivity

In SQL Server:
- **Keywords**: Not case-sensitive (`SELECT` = `select`)
- **Identifiers**: Not case-sensitive by default
- **Data**: Case-sensitive for string comparison (depends on collation)

### Best Practice:
```sql
-- Use UPPERCASE for keywords
SELECT name, age
FROM students
WHERE age > 18;
```

## SQL Comments

Comments help document your code and are ignored by SQL Server.

### Single-line comments:
```sql
-- This is a single-line comment
SELECT * FROM Students;
```

### Multi-line comments:
```sql
/* This is a
   multi-line comment */
SELECT * FROM Students;
```

## Example Syntax

### Simple SELECT:
```sql
SELECT * FROM Students;
```

### SELECT with specific columns:
```sql
SELECT Name, Age FROM Students;
```

### SELECT with WHERE clause:
```sql
SELECT Name, Age
FROM Students
WHERE Age > 20;
```

### INSERT statement:
```sql
INSERT INTO Students (ID, Name, Age)
VALUES (1, 'John', 20);
```

### UPDATE statement:
```sql
UPDATE Students
SET Age = 21
WHERE ID = 1;
```

### DELETE statement:
```sql
DELETE FROM Students
WHERE ID = 1;
```

## Common Syntax Rules

1. **End with semicolon**: Each statement ends with `;`
2. **Use quotes**: String values use single quotes `'John'`
3. **Commas separate**: Columns and values are comma-separated
4. **Parentheses**: Required for function calls and grouped operations

## Key Takeaways

- SQL statements have specific structure
- Keywords are reserved words
- Use comments to document code
- End statements with semicolon
- String values need single quotes

---

*This topic should take about 5 minutes to explain in class.*
