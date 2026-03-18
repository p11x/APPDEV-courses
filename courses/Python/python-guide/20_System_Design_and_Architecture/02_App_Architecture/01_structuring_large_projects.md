# 🏗️ Structuring Large Projects

> How to organize a Python project that has 20+ files without losing your mind.

## 🎯 What You'll Learn

- Evolution from script to application
- Recommended project structure
- Dependency direction rules
- __init__.py strategy
- Circular import prevention

## 📦 Prerequisites

- Completion of [06_Modules_and_Packages/01_importing.md](../../06_Modules_and_Packages/01_importing.md)

---

## Project Evolution

| Stage | Lines | Structure |
|-------|-------|-----------|
| **Script** | < 100 | Single file |
| **Module** | 100-500 | Multiple files |
| **Package** | 500-2000 | Proper package |
| **Application** | 2000+ | Layered architecture |

---

## Recommended Structure

```
myapp/
├── __init__.py          # Public API exports
├── main.py              # Entry point only
├── config.py            # All configuration
├── models/              # Data models
│   ├── __init__.py
│   ├── user.py
│   └── product.py
├── services/            # Business logic
│   ├── __init__.py
│   ├── user_service.py
│   └── product_service.py
├── repositories/        # Data access
│   ├── __init__.py
│   ├── user_repo.py
│   └── product_repo.py
├── api/                 # HTTP layer
│   ├── __init__.py
│   ├── routes.py
│   └── deps.py
├── cli/                 # CLI layer
│   ├── __init__.py
│   └── commands.py
└── utils/               # Shared helpers
    ├── __init__.py
    └── helpers.py
```

### Dependency Direction

```
api/ → services/ → repositories/ → models/
       ↘ utils/ ←
       
Never import upward!
```

---

## __init__.py Strategy

```python
# models/__init__.py

# Re-export public API only
from .user import User
from .product import Product

__all__ = ["User", "Product"]
```

---

## Circular Import Prevention

```python
# Solution 1: Move imports inside functions
class UserService:
    def get_user(self, id: int):
        # Import here, not at top
        from .models import User
        return User.get(id)


# Solution 2: Use TYPE_CHECKING
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .models import User


class UserService:
    # User is available for type hints, not runtime
    def get_user(self, id: int) -> "User":
        ...
```

---

## Summary

✅ Structure evolves with project size

✅ Layer dependencies flow downward

✅ __init__.py exports public API only

✅ Prevent circular imports with TYPE_CHECKING

---

## ➡️ Next Steps

Continue to [02_layered_and_hexagonal_architecture.md](./02_layered_and_hexagonal_architecture.md)

---

## 🔗 Further Reading

- [Python Packaging Guide](https://packaging.python.org/)
- [Structure](https://docs.python-guide.org/writing/structure/)
