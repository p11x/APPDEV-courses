<!-- FILE: 08_rest_apis/01_api_basics/02_returning_json.md -->

## Overview

REST APIs typically return JSON data. Flask's `jsonify()` function creates JSON responses.

## Code Walkthrough

### JSON Responses

```python
# app.py — Returning JSON
from flask import jsonify

@app.route("/api/users")
def get_users():
    users = [{"id": 1, "name": "Alice"}, {"id": 2, "name": "Bob"}]
    return jsonify(users)

@app.route("/api/users/<int:user_id>")
def get_user(user_id):
    user = {"id": user_id, "name": "Alice"}
    return jsonify(user)
```

## Next Steps

Now return JSON. Continue to [03_status_codes.md](03_status_codes.md) to learn HTTP status codes.