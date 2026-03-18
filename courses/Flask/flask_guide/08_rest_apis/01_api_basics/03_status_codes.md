<!-- FILE: 08_rest_apis/01_api_basics/03_status_codes.md -->

## Overview

HTTP status codes indicate the result of API requests. Proper status codes are essential for good API design.

## Core Concepts

### Common Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK |
| 201 | Created |
| 400 | Bad Request |
| 401 | Unauthorized |
| 404 | Not Found |
| 500 | Server Error |

## Code Walkthrough

### Status Codes in Flask

```python
from flask import jsonify

@app.route("/api/users", methods=["POST"])
def create_user():
    # Return 201 for creation
    return jsonify({"id": 1}), 201

@app.route("/api/users/<int:user_id>")
def get_user(user_id):
    user = User.query.get(user_id)
    if not user:
        # Return 404 for not found
        return jsonify({"error": "Not found"}), 404
    return jsonify(user.to_dict())
```

## Next Steps

Now you understand status codes. Continue to [01_installing_flask_restful.md](../02_flask_restful/01_installing_flask_restful.md) to learn Flask-RESTful.