# SQL Server Subqueries

## What is a Subquery?

A **subquery** (also called nested query or inner query) is a query nested inside another query. The inner query executes first, and its result is used by the outer query.

## Why Use Subqueries?

- Filter results based on unknown values
- Perform calculations that require intermediate results
- Break complex queries into simpler parts
- Compare values against aggregated data

## Types of Subqueries

### 1. Scalar Subquery
Returns a single value (one column, one row)

### 2. Table-Valued Subquery
Returns multiple rows and columns (like a temporary table)

### 3. Correlated Subquery
References columns from the outer query

## Setting Up Sample Tables

```sql
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100),
    Category VARCHAR(50),
    Price DECIMAL(10, 2),
    Stock INT
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    ProductID INT,
    Quantity INT,
    OrderDate DATE
);

INSERT INTO Products VALUES 
    (1, 'Laptop', 'Electronics', 999.99, 50),
    (2, 'Mouse', 'Electronics', 29.99, 200),
    (3, 'Keyboard', 'Electronics', 79.99, 150),
    (4, 'Notebook', 'Stationery', 2.99, 500),
    (5, 'Pen', 'Stationery', 0.99, 1000);

INSERT INTO Orders VALUES 
    (1, 1, 2, '2024-01-15'),
    (2, 2, 5, '2024-01-16'),
    (3, 1, 1, '2024-01-17'),
    (4, 4, 10, '2024-01-18');
```

## Scalar Subquery Example

Find products with price above average:

```sql
SELECT ProductName, Price
FROM Products
WHERE Price > (SELECT AVG(Price) FROM Products);
```

**Expected Output:**
```
+---------------+---------+
| ProductName   | Price   |
+---------------+---------+
| Laptop        | 999.99  |
| Mouse         | 29.99   |
| Keyboard      | 79.99   |
+---------------+---------+
```

## Subquery with IN

Find products that have been ordered:

```sql
SELECT ProductName, Price
FROM Products
WHERE ProductID IN (SELECT ProductID FROM Orders);
```

**Expected Output:**
```
+---------------+---------+
| ProductName   | Price   |
+---------------+---------+
| Laptop        | 999.99  |
| Mouse         | 29.99   |
| Notebook      | 2.99    |
+---------------+---------+
```

## Subquery with EXISTS

Find categories that have products:

```sql
SELECT Category
FROM Categories c
WHERE EXISTS (SELECT 1 FROM Products p WHERE p.Category = c.Category);
```

## Subquery in FROM Clause

Create a derived table:

```sql
SELECT * FROM (
    SELECT ProductName, Price, 
           CASE 
               WHEN Price > 100 THEN 'Expensive'
               WHEN Price > 10 THEN 'Medium'
               ELSE 'Cheap'
           END AS PriceCategory
    FROM Products
) AS ProductCategories
WHERE PriceCategory = 'Expensive';
```

**Expected Output:**
```
+---------------+-------+---------------+
| ProductName   | Price | PriceCategory |
+---------------+-------+---------------+
| Laptop        | 999.99| Expensive     |
| Keyboard      | 79.99 | Expensive     |
+---------------+-------+---------------+
```

## Correlated Subquery

A correlated subquery references the outer query. It runs once for each row:

```sql
SELECT p.ProductName, p.Price
FROM Products p
WHERE p.Price > (SELECT AVG(Price) FROM Products WHERE Category = p.Category);
```

This finds products whose price is above their category average.

## Subquery with UPDATE

Use subquery in UPDATE statement:

```sql
UPDATE Products
SET Stock = Stock + 10
WHERE ProductID = (SELECT ProductID FROM Products WHERE ProductName = 'Laptop');
```

## Subquery with DELETE

Use subquery in DELETE statement:

```sql
DELETE FROM Orders
WHERE ProductID = (SELECT ProductID FROM Products WHERE ProductName = 'Mouse');
```

## Subquery vs JOIN

Sometimes both can achieve the same result:

```sql
-- Using Subquery
SELECT ProductName
FROM Products
WHERE Category = 'Electronics';

-- Using JOIN (similar result)
SELECT DISTINCT p.ProductName
FROM Products p
INNER JOIN Categories c ON p.Category = c.CategoryName
WHERE c.CategoryName = 'Electronics';
```

## Key Points Summary

| Type | Description |
|------|-------------|
| Scalar | Returns single value |
| Table-Valued | Returns rows/columns |
| Correlated | References outer query |
| IN | Match against list |
| EXISTS | Check for existence |

---

*This topic should take about 5-7 minutes to explain in class.*
