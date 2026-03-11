# SQL Server Architecture

## Understanding the Hierarchy

SQL Server follows a hierarchical structure:

```
SQL Server Instance
    │
    └── Database
            │
            └── Tables
                    │
                    ├── Rows
                    └── Columns
```

## SQL Server Instance

An **Instance** is a single installation of SQL Server that runs independently. You can have multiple instances on one machine:

- **Default Instance**: One per server (accessed by machine name)
- **Named Instance**: Multiple per server (accessed by machine_name\instance_name)

## Database

A **Database** is a container that stores related data. It contains:

- Tables (data storage)
- Views (virtual tables)
- Stored Procedures (precompiled code)
- Indexes (for fast data retrieval)

### Types of Databases:
- **System Databases**: Master, Model, MSDB, TempDB (for SQL Server operations)
- **User Databases**: Created by users for applications

## Tables

A **Table** is the fundamental storage unit in a relational database. Think of it like a spreadsheet:

- Organized in rows and columns
- Each row represents one record
- Each column represents one attribute

## Rows and Columns

### Rows (Records)
- Horizontal entries in a table
- Represents one complete entity
- Example: One student, one product, one order

### Columns (Fields)
- Vertical entries in a table
- Represents an attribute
- Has a specific data type

## Visual Diagram

```
┌─────────────────────────────────────────┐
│         SQL SERVER INSTANCE             │
│  ┌───────────────────────────────────┐  │
│  │           DATABASE                │  │
│  │  ┌─────────────────────────────┐  │  │
│  │  │         TABLE: Students     │  │  │
│  │  │  ID  │ Name  │ Age │ Dept   │  │  │
│  │  │──────┼───────┼─────┼────────│  │  │
│  │  │  1   │ John  │  20 │ CS     │  │  │
│  │  │  2   │ Jane  │  21 │ Math   │  │  │
│  │  │  3   │ Mike  │  19 │ Physics│  │  │
│  │  └─────────────────────────────┘  │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
```

## Simple Example

```
Table: Employees
┌─────────┬──────────┬────────┬──────────┐
│ EmpID   │ Name     │ Salary │ Dept     │
├─────────┼──────────┼────────┼──────────┤
│ 1001    │ Alice    │ 50000  │ HR       │
│ 1002    │ Bob      │ 60000  │ IT       │
│ 1003    │ Charlie  │ 55000  │ Finance  │
└─────────┴──────────┴────────┴──────────┘
  ↑        ↑         ↑        ↑
 Columns (Fields)
                ↑
            Rows (Records)
```

## Key Takeaways

- **Instance** → Running SQL Server software
- **Database** → Container for tables
- **Table** → Data organized in rows and columns
- **Row** → One complete record
- **Column** → One attribute of data

---

*This topic should take about 5 minutes to explain in class.*
