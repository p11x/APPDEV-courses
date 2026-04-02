# Normalization and Denormalization Strategies

## What You'll Learn

- Normal forms (1NF through 5NF)
- When to normalize vs denormalize
- Denormalization patterns
- Performance tradeoffs
- Practical decision framework

## Normalization Examples

```sql
-- UNNORMALIZED
CREATE TABLE orders_raw (
    order_id INT,
    customer_name VARCHAR(255),
    customer_email VARCHAR(255),
    items TEXT, -- "Product A:2:10.00,Product B:1:25.00"
    total DECIMAL
);

-- 1NF: Atomic values, no repeating groups
CREATE TABLE orders_1nf (
    order_id INT,
    customer_name VARCHAR(255),
    customer_email VARCHAR(255),
    product_name VARCHAR(255),
    quantity INT,
    price DECIMAL
);

-- 2NF: Remove partial dependencies
CREATE TABLE orders_2nf (
    order_id INT PRIMARY KEY,
    customer_name VARCHAR(255),
    customer_email VARCHAR(255)
);

CREATE TABLE order_items_2nf (
    order_id INT REFERENCES orders_2nf(order_id),
    product_name VARCHAR(255),
    quantity INT,
    price DECIMAL,
    PRIMARY KEY (order_id, product_name)
);

-- 3NF: Remove transitive dependencies
CREATE TABLE customers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL
);

CREATE TABLE orders (
    id SERIAL PRIMARY KEY,
    customer_id INT REFERENCES customers(id),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE products (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10,2) NOT NULL
);

CREATE TABLE order_items (
    order_id INT REFERENCES orders(id),
    product_id INT REFERENCES products(id),
    quantity INT NOT NULL,
    price_at_order DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (order_id, product_id)
);
```

## Denormalization Patterns

```sql
-- Pattern 1: Materialized view for reporting
CREATE MATERIALIZED VIEW order_summary AS
SELECT 
    o.id as order_id,
    c.name as customer_name,
    c.email as customer_email,
    COUNT(oi.product_id) as item_count,
    SUM(oi.quantity * oi.price_at_order) as total,
    o.created_at
FROM orders o
JOIN customers c ON c.id = o.customer_id
JOIN order_items oi ON oi.order_id = o.id
GROUP BY o.id, c.name, c.email, o.created_at;

-- Refresh periodically
REFRESH MATERIALIZED VIEW CONCURRENTLY order_summary;

-- Pattern 2: Computed columns
ALTER TABLE orders ADD COLUMN total DECIMAL(10,2);
ALTER TABLE orders ADD COLUMN item_count INT;

-- Trigger to maintain denormalized data
CREATE OR REPLACE FUNCTION update_order_totals()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE orders SET
        total = (SELECT SUM(quantity * price_at_order) FROM order_items WHERE order_id = COALESCE(NEW.order_id, OLD.order_id)),
        item_count = (SELECT COUNT(*) FROM order_items WHERE order_id = COALESCE(NEW.order_id, OLD.order_id))
    WHERE id = COALESCE(NEW.order_id, OLD.order_id);
    RETURN COALESCE(NEW, OLD);
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER order_items_changed
AFTER INSERT OR UPDATE OR DELETE ON order_items
FOR EACH ROW EXECUTE FUNCTION update_order_totals();

-- Pattern 3: Embedded data (for MongoDB-style)
-- Store frequently accessed related data together
CREATE TABLE user_profiles (
    user_id INT PRIMARY KEY REFERENCES users(id),
    name VARCHAR(255),
    email VARCHAR(255),
    recent_orders JSONB DEFAULT '[]',
    preferences JSONB DEFAULT '{}',
    stats JSONB DEFAULT '{}'
);
```

## Decision Framework

```
Normalize When:                    Denormalize When:
──────────────────────────────────────────────────────
Data is frequently updated         Data is read-heavy
Data integrity is critical         Performance is critical
Storage space is limited           Joins are too expensive
Multiple write paths exist         Data is append-heavy
Complex queries are common         Simple access patterns
Data consistency is priority       Reporting/analytics queries

Hybrid approach:
├── Normalize transactional data
├── Denormalize for read-heavy views
├── Use materialized views for reports
├── Cache denormalized data in application
└── Eventual consistency for non-critical denormalization
```

## Best Practices Checklist

- [ ] Start with 3NF normalization for transactional data
- [ ] Denormalize only when justified by performance metrics
- [ ] Use materialized views for complex reporting queries
- [ ] Maintain denormalized data with triggers or application logic
- [ ] Document all denormalization decisions
- [ ] Monitor consistency of denormalized data

## Cross-References

- See [Schema Design](./01-schema-design.md) for schema basics
- See [Indexing](./03-indexing-strategies.md) for performance
- See [Query Optimization](../02-database-performance-optimization/01-query-optimization.md) for queries

## Next Steps

Continue to [Indexing Strategies](./03-indexing-strategies.md) for index optimization.
