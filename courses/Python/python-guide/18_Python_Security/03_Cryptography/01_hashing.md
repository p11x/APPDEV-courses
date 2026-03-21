# Hashing

## What You'll Learn
- hashlib
- bcrypt for passwords

## Prerequisites
- Read 04_web_security.md first

## Overview
One-way functions.

## Hashlib
Hash

```python
import hashlib
hashlib.sha256(b"data").hexdigest()
```

## Common Mistakes
- MD5 for secrets
- no salt

## Summary
- sha256 standard
- one-way
- fixed length

## Next Steps
Continue to **[](./)**
