# ⚙️ Configuration Management

## 🎯 What You'll Learn

- Using .env files
- TOML configuration
- pydantic-settings
- Layered configuration

---

## .env Files

```bash
# .env
ANTHROPIC_API_KEY=sk-...
DATABASE_URL=sqlite:///myapp.db
DEBUG=true
```

```python
from dotenv import load_dotenv

load_dotenv()  # Loads .env

import os
api_key = os.environ.get("ANTHROPIC_API_KEY")
```

---

## TOML Config

```python
# config.toml
[database]
host = "localhost"
port = 5432

[api]
key = "secret"
rate_limit = 100
```

```python
import tomllib

with open("config.toml", "rb") as f:
    config = tomllib.load(f)

print(config["database"]["host"])
```

---

## pydantic-settings

```bash
pip install pydantic-settings
```

```python
from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    api_key: str
    database_url: str = "sqlite:///myapp.db"
    debug: bool = False
    
    model_config = {"env_file": ".env"}

settings = Settings()
print(settings.api_key)
```

---

## Layered Config

```python
# Priority: default → .env → CLI args

class Settings(BaseSettings):
    api_key: str = "default-key"  # Default
    database_url: str = "sqlite://default.db"
    
    model_config = {"env_file": ".env"}  # .env overrides
```

---

## ✅ Summary

- Use .env for secrets
- Use TOML for structured config
- Use pydantic-settings for type-safe settings

## 🔗 Further Reading

- [pydantic-settings](https://docs.pydantic.dev/latest/api/pydantic_settings/)
