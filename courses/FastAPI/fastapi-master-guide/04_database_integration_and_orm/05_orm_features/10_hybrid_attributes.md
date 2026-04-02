# Hybrid Attributes

## Overview

Hybrid attributes provide both Python and SQL expressions for the same property.

## Implementation

### Python vs SQL Expressions

```python
# Example 1: Hybrid properties
from sqlalchemy.ext.hybrid import hybrid_property

class Product(Base):
    __tablename__ = "products"

    id = Column(Integer, primary_key=True)
    name = Column(String(100))
    price = Column(Float)
    tax_rate = Column(Float, default=0.1)

    @hybrid_property
    def price_with_tax(self):
        """Python expression"""
        return self.price * (1 + self.tax_rate)

    @price_with_tax.expression
    def price_with_tax(cls):
        """SQL expression"""
        return cls.price * (1 + cls.tax_rate)
```

### Querying with Hybrid Properties

```python
# Example 2: Querying hybrid properties
# Python side (instance)
product = db.query(Product).first()
print(product.price_with_tax)  # Uses Python expression

# SQL side (query)
expensive = db.query(Product).filter(
    Product.price_with_tax > 100
).all()  # Uses SQL expression
```

## Summary

Hybrid properties enable consistent behavior in Python and SQL.

## Next Steps

Continue learning about:
- [Scoped Session Management](./11_scoped_session_management.md)
- [Query Optimization](../../08_database_performance/01_query_optimization.md)
