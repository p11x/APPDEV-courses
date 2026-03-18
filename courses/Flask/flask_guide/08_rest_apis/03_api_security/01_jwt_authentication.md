<!-- FILE: 08_rest_apis/03_api_security/01_jwt_authentication.md -->

## Overview

**JWT (JSON Web Tokens)** are a popular method for securing REST APIs. This file covers implementing JWT authentication in Flask using PyJWT.

## Core Concepts

### How JWT Works

1. User logs in with credentials
2. Server generates signed token
3. Client stores token (usually in localStorage)
4. Client sends token in Authorization header
5. Server verifies token on each request

## Code Walkthrough

### JWT Authentication

```python
# app.py — JWT authentication
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

@app.route('/login')
def login():
    auth = request.authorization
    
    if auth and auth.password == 'password':
        token = jwt.encode({
            'user_id': auth.username,
            'exp': datetime.datetime.utcnow() + datetime.timedelta(minutes=30)
        }, app.config['SECRET_KEY'])
        
        return jsonify({'token': token})
    
    return jsonify({'message': 'Could not verify!'}), 401

@app.route('/protected')
@token_required
def protected(current_user):
    return jsonify({'message': f'Hello {current_user}!'})
```

## Next Steps

Now you can secure APIs. Continue to [02_protecting_endpoints.md](02_protecting_endpoints.md) to learn more endpoint protection.