<!-- FILE: 08_rest_apis/03_api_security/02_protecting_endpoints.md -->

## Overview

Protecting API endpoints ensures that only authorized users can access certain resources. This file covers various methods to protect endpoints: decorators, middleware, and role-based access control.

## Core Concepts

### Protection Methods

1. **Decorators** — Wrap view functions to check authentication
2. **Middleware** — Apply protection to multiple routes
3. **Role-Based Access Control (RBAC)** — Restrict access by user role

## Code Walkthrough

### Decorator Protection

```python
# app.py — Protecting endpoints with decorators
from flask import Flask, request, jsonify
import jwt
import datetime
from functools import wraps

app = Flask(__name__)
app.config['SECRET_KEY'] = 'your-secret-key'

def token_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        token = request.headers.get('Authorization')
        if not token:
            return jsonify({'message': 'Token is missing!'}), 401
        try:
            data = jwt.decode(token, app.config['SECRET_KEY'], algorithms=["HS256"])
            current_user = data['user_id']
        except:
            return jsonify({'message': 'Token is invalid!'}), 401
        return f(current_user, *args, **kwargs)
    return decorated

def admin_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        token = request.headers.get('Authorization')
        if not token:
            return jsonify({'message': 'Token is missing!'}), 401
        try:
            data = jwt.decode(token, app.config['SECRET_KEY'], algorithms=["HS256"])
            if data.get('role') != 'admin':
                return jsonify({'message': 'Admin access required!'}), 403
            current_user = data['user_id']
        except:
            return jsonify({'message': 'Token is invalid!'}), 401
        return f(current_user, *args, **kwargs)
    return decorated

@app.route('/public')
def public():
    return jsonify({'message': 'Anyone can see this'})

@app.route('/protected')
@token_required
def protected(current_user):
    return jsonify({'message': f'Hello {current_user}!'})

@app.route('/admin')
@admin_required
def admin(current_user):
    return jsonify({'message': 'Welcome, admin!'})
```

### Middleware Protection (Before Request)

```python
# app.py — Middleware-style protection
from flask import Flask, request, jsonify, g
import jwt

app = Flask(__name__)
app.config['SECRET_KEY'] = 'your-secret-key'

@app.before_request
def check_token():
    # Skip token check for public endpoints
    if request.endpoint in ['public', 'login']:
        return
    
    token = request.headers.get('Authorization')
    if not token:
        return jsonify({'message': 'Token is missing!'}), 401
    try:
        data = jwt.decode(token, app.config['SECRET_KEY'], algorithms=["HS256"])
        g.current_user = data['user_id']
        g.user_role = data.get('role', 'user')
    except:
        return jsonify({'message': 'Token is invalid!'}), 401

@app.route('/public')
def public():
    return jsonify({'message': 'Anyone can see this'})

@app.route('/protected')
def protected():
    return jsonify({'message': f'Hello {g.current_user}!'})

@app.route('/admin')
def admin():
    if g.user_role != 'admin':
        return jsonify({'message': 'Admin access required!'}), 403
    return jsonify({'message': 'Welcome, admin!'})
```

## Next Steps

Now you can protect endpoints. Continue to [03_cors_handling.md](03_cors_handling.md) to learn about CORS.