# JWT Authentication

## What You'll Learn
- JWT token structure
- Implementing JWT in Flask

## Prerequisites
- Completed session-based auth

## JWT in Flask

```python
import jwt
from functools import wraps

SECRET_KEY = "your-secret"

def token_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        token = request.headers.get('Authorization')
        if not token:
            return jsonify({'message': 'Token missing'}), 401
        
        try:
            data = jwt.decode(token, SECRET_KEY, algorithms=['HS256'])
            current_user = User.query.get(data['user_id'])
        except:
            return jsonify({'message': 'Invalid token'}), 401
        
        return f(current_user, *args, **kwargs)
    return decorated

@app.route('/protected')
@token_required
def protected(current_user):
    return jsonify({'user': current_user.username})
```
