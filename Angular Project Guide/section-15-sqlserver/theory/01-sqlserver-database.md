# Section 15: SQL Server Database Design

## Database Schema

### Products Table
```sql
-- Create Products Table
CREATE TABLE products (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(500),
    price DECIMAL(10,2) NOT NULL,
    category NVARCHAR(50),
    in_stock BIT DEFAULT 1,
    image_url NVARCHAR(255),
    created_at DATETIME2 DEFAULT GETDATE(),
    updated_at DATETIME2 DEFAULT GETDATE()
);

-- Insert Sample Data
INSERT INTO products (name, description, price, category, in_stock)
VALUES 
    ('Laptop Pro', 'High-performance laptop', 1299.99, 'Electronics', 1),
    ('Wireless Mouse', 'Ergonomic mouse', 49.99, 'Accessories', 1),
    ('Office Chair', 'Comfortable chair', 299.99, 'Furniture', 0);
```

### Categories Table
```sql
CREATE TABLE categories (
    id BIGINT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(50) NOT NULL UNIQUE,
    description NVARCHAR(200)
);

INSERT INTO categories (name, description)
VALUES 
    ('Electronics', 'Electronic devices'),
    ('Furniture', 'Home and office furniture'),
    ('Accessories', 'Computer accessories');
```

---

## SQL Queries

### Basic CRUD Operations
```sql
-- SELECT all products
SELECT * FROM products;

-- SELECT with condition
SELECT * FROM products WHERE category = 'Electronics';

-- INSERT
INSERT INTO products (name, price, category) 
VALUES ('New Product', 99.99, 'Electronics');

-- UPDATE
UPDATE products SET price = 1099.99 WHERE id = 1;

-- DELETE
DELETE FROM products WHERE id = 1;
```

### Advanced Queries
```sql
-- JOIN categories
SELECT p.name, p.price, c.name as category
FROM products p
INNER JOIN categories c ON p.category = c.name;

-- Aggregate functions
SELECT 
    category,
    COUNT(*) as total_products,
    AVG(price) as avg_price,
    MIN(price) as min_price,
    MAX(price) as max_price
FROM products
GROUP BY category;
```

---

## Summary

SQL = Structured Query Language for Database Operations
