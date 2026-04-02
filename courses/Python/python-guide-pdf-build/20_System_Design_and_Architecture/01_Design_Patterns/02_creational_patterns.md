# 🏗️ Creational Patterns

> Patterns for creating objects cleanly and flexibly.

## 🎯 What You'll Learn

- Singleton — ensure only one instance exists
- Factory Method — let subclasses decide which class to instantiate
- Abstract Factory — create families of related objects
- Builder — construct complex objects step by step
- Prototype — clone existing objects

## 📦 Prerequisites

- Completion of [01_solid_principles.md](./01_solid_principles.md)
- Understanding of classes and inheritance

---

## Singleton Pattern

Ensure only one instance exists:

```python
class SingletonMeta(type):
    """Metaclass for singleton pattern."""
    
    _instances: dict = {}  # Store single instance
    
    def __call__(cls, *args, **kwargs):
        """Create or return existing instance."""
        
        if cls not in cls._instances:
            # First time — create instance
            instance = super().__call__(*args, **kwargs)
            cls._instances[cls] = instance
        
        return cls._instances[cls]


class Database(metaclass=SingletonMeta):
    """Database connection — only one instance."""
    
    def __init__(self):
        self.connection = "connected to localhost"
    
    def query(self, sql: str) -> list:
        print(f"Executing: {sql}")
        return []


# Usage
db1 = Database()
db2 = Database()

print(db1 is db2)  # True — same instance!
```

### 💡 Explanation

- `SingletonMeta` metaclass controls instantiation
- First call creates instance, subsequent calls return it

---

## Factory Method

Let subclasses decide which class to create:

```python
from abc import ABC, abstractmethod


class DataParser(ABC):
    """Abstract parser."""
    
    @abstractmethod
    def parse(self, content: str) -> dict:
        ...


class JSONParser(DataParser):
    """Parse JSON."""
    
    def parse(self, content: str) -> dict:
        import json
        return json.loads(content)


class XMLParser(DataParser):
    """Parse XML."""
    
    def parse(self, content: str) -> dict:
        from xml.etree import ElementTree
        root = ElementTree.fromstring(content)
        return {"tag": root.tag, "text": root.text}


class CSVParser(DataParser):
    """Parse CSV."""
    
    def parse(self, content: str) -> list[dict]:
        import csv
        import io
        reader = csv.DictReader(io.StringIO(content))
        return list(reader)


class ParserFactory:
    """Factory for creating parsers."""
    
    @staticmethod
    def get_parser(format: str) -> DataParser:
        """Get appropriate parser for format."""
        
        parsers = {
            "json": JSONParser,
            "xml": XMLParser,
            "csv": CSVParser,
        }
        
        parser_class = parsers.get(format.lower())
        
        if not parser_class:
            raise ValueError(f"Unknown format: {format}")
        
        return parser_class()


# Usage
parser = ParserFactory.get_parser("json")
data = parser.parse('{"name": "Alice"}')
print(data)
```

---

## Builder Pattern

Construct complex objects step by step:

```python
class QueryBuilder:
    """Build SQL queries fluently."""
    
    def __init__(self):
        self._select = []
        self._from = ""
        self._where = []
        self._order_by = []
        self._limit_val = None
    
    def select(self, *columns) -> "QueryBuilder":
        """Add SELECT columns."""
        self._select = list(columns)
        return self
    
    def from_table(self, table: str) -> "QueryBuilder":
        """Add FROM clause."""
        self._from = table
        return self
    
    def where(self, condition: str) -> "QueryBuilder":
        """Add WHERE condition."""
        self._where.append(condition)
        return self
    
    def order_by(self, column: str, direction: str = "ASC") -> "QueryBuilder":
        """Add ORDER BY."""
        self._order_by.append(f"{column} {direction}")
        return self
    
    def limit(self, n: int) -> "QueryBuilder":
        """Add LIMIT."""
        self._limit_val = n
        return self
    
    def build(self) -> str:
        """Build final SQL query."""
        
        parts = []
        
        # SELECT
        cols = ", ".join(self._select) if self._select else "*"
        parts.append(f"SELECT {cols}")
        
        # FROM
        if self._from:
            parts.append(f"FROM {self._from}")
        
        # WHERE
        if self._where:
            where_str = " AND ".join(self._where)
            parts.append(f"WHERE {where_str}")
        
        # ORDER BY
        if self._order_by:
            parts.append(f"ORDER BY {', '.join(self._order_by)}")
        
        # LIMIT
        if self._limit_val:
            parts.append(f"LIMIT {self._limit_val}")
        
        return " ".join(parts)


# Usage — fluent chain
query = (
    QueryBuilder()
    .select("id", "name", "email")
    .from_table("users")
    .where("active = true")
    .where("age > 18")
    .order_by("name", "ASC")
    .limit(10)
    .build()
)

print(query)
# SELECT id, name, email FROM users WHERE active = true AND age > 18 ORDER BY name ASC LIMIT 10
```

---

## Summary

✅ **Singleton** — one instance only

✅ **Factory Method** — let subclasses decide

✅ **Builder** — fluent object construction

✅ **Prototype** — clone existing objects

---

## ➡️ Next Steps

Continue to [03_structural_and_behavioural_patterns.md](./03_structural_and_behavioural_patterns.md) for more patterns.

---

## 🔗 Further Reading

- [Design Patterns](https://refactoring.guru/design-patterns)
- [Python Design Patterns](https://python-patterns.guide/)
