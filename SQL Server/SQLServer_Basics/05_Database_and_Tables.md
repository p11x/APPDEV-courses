# Database and Tables

## What is a Database?

A **Database** is an organized collection of data stored electronically. It acts as a container that holds all your tables and other objects.

### Key Points:
- Stores data logically
- Provides data security
- Allows multiple users to access data
- Enables data backup and recovery

## What is a Table?

A **Table** is the fundamental building block of a database. It's like a spreadsheet with rows and columns.

### Table Characteristics:
- Contains related data
- Organized in rows and columns
- Each row is a unique record
- Each column represents an attribute

## Rows (Records)

- Horizontal data entries
- Each row represents one complete entity
- Also called a "record"
- Example: One student, one product, one order

## Columns (Fields)

- Vertical data entries
- Each column represents one attribute
- Also called a "field" or "attribute"
- Has a specific data type (text, number, date, etc.)

## How Data is Stored

Think of a table like this:

```
┌─────────────────────────────────────────────────┐
│                   Table: Students               │
├─────────┬──────────┬─────────┬────────────────┤
│  ID     │  Name    │  Age    │  Department    │  ← Column Headers
├─────────┼──────────┼─────────┼────────────────┤
│  1      │  John    │  20     │  Computer Sci  │  ← Row 1
├─────────┼──────────┼─────────┼────────────────┤
│  2      │  Jane    │  21     │  Mathematics   │  ← Row 2
├─────────┼──────────┼─────────┼────────────────┤
│  3      │  Mike    │  19     │  Physics       │  ← Row 3
└─────────┴──────────┴─────────┴────────────────┘
```

## Simple Example Table

### Students Table

| ID | Name   | Age | Department    | Email                  |
|----|--------|-----|---------------|------------------------|
| 1  | John   | 20  | Computer Sci  | john@college.com       |
| 2  | Jane   | 21  | Mathematics   | jane@college.com       |
| 3  | Mike   | 19  | Physics       | mike@college.com       |
| 4  | Sarah  | 20  | Chemistry     | sarah@college.com      |

### Explanation:
- **ID**: Unique identifier for each student
- **Name**: Student's name
- **Age**: Student's age
- **Department**: Academic department
- **Email**: Contact email

## Key Takeaways

- **Database**: Container for all data
- **Table**: Structured data in rows and columns
- **Row**: One complete record/entity
- **Column**: One attribute of data
- Tables are the foundation of relational databases

---

*This topic should take about 5 minutes to explain in class.*
