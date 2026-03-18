# 🛡️ Input Validation and Injection Prevention

## 🎯 What You'll Learn

- SQL injection prevention
- Command injection prevention
- Path traversal prevention

---

## SQL Injection

```python
# ❌ NEVER do this!
query = f"SELECT * FROM users WHERE name = '{user_input}'"

# ✅ ALWAYS use parameterized queries!
cursor.execute(
    "SELECT * FROM users WHERE name = ?",
    (user_input,)  # Tuple!
)
```

---

## Command Injection

```python
import subprocess

# ❌ NEVER do this!
subprocess.run(f"ls {user_input}", shell=True)

# ✅ Use list form, no shell!
subprocess.run(["ls", user_input], shell=False)
```

---

## Path Traversal

```python
from pathlib import Path

# ❌ NEVER do this!
file = open(f"/data/{user_input}")

# ✅ Validate path!
base = Path("/data")
requested = (base / user_input).resolve()

# Ensure it's within base
if not requested.is_relative_to(base):
    raise ValueError("Access denied!")
    
content = requested.read_text()
```

---

## Pydantic for Validation

```python
from pydantic import BaseModel, validator

class UserInput(BaseModel):
    email: str
    age: int
    
    @validator("email")
    def validate_email(cls, v):
        if "@" not in v:
            raise ValueError("Invalid email")
        return v
    
    @validator("age")
    def validate_age(cls, v):
        if v < 0 or v > 150:
            raise ValueError("Invalid age")
        return v
```

---

## eval/exec

```python
# ❌ NEVER use with user input!
result = eval(user_input)   # DANGEROUS!
result = exec(user_input)   # DANGEROUS!
```

---

## ✅ Summary

- Always use parameterized queries
- Never pass user input to shell commands
- Validate file paths
- Never use eval/exec with user input

## 🔗 Further Reading

- [OWASP Injection](https://owasp.org/www-community/Injection_Flaws)
