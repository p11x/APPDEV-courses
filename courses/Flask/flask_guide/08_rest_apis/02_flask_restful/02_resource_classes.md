<!-- FILE: 08_rest_apis/02_flask_restful/02_resource_classes.md -->

## Overview

Resource classes define API endpoints with methods for each HTTP verb.

## Code Walkthrough

### Complete Resource

```python
from flask_restful import Resource, Api

class UserListResource(Resource):
    def get(self):
        return {"users": [{"id": 1, "name": "Alice"}]}
    
    def post(self):
        return {"id": 2}, 201

class UserResource(Resource):
    def get(self, user_id):
        return {"id": user_id, "name": "Alice"}
    
    def put(self, user_id):
        return {"id": user_id, "updated": True}
    
    def delete(self, user_id):
        return {"deleted": True}, 204

api.add_resource(UserListResource, "/api/users")
api.add_resource(UserResource, "/api/users/<int:user_id>")
```

## Next Steps

Now use resource classes. Continue to [03_request_parsing.md](03_request_parsing.md) to learn request parsing.