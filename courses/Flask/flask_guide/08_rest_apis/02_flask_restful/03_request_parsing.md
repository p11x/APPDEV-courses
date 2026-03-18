<!-- FILE: 08_rest_apis/02_flask_restful/03_request_parsing.md -->

## Overview

Request parsing validates and converts incoming request data. Flask-RESTful's reqparse provides declarative parsing similar to WTForms.

## Code Walkthrough

### Request Parsing

```python
from flask_restful import reqparse

# Create parser
parser = reqparse.RequestParser()
parser.add_argument('name', type=str, required=True, help='Name required')
parser.add_argument('age', type=int, required=True, help='Age must be integer')
parser.add_argument('email', type=str, required=False)

class UserResource(Resource):
    def post(self):
        args = parser.parse_args()
        # args is dict: {'name': 'Alice', 'age': 25, 'email': 'alice@example.com'}
        return {'id': 1}, 201
```

## Next Steps

Now you can parse requests. Continue to [01_jwt_authentication.md](../03_api_security/01_jwt_authentication.md) to learn API security.