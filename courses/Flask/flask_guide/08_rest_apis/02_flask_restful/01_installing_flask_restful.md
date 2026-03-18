<!-- FILE: 08_rest_apis/02_flask_restful/01_installing_flask_restful.md -->

## Overview

**Flask-RESTful** simplifies building REST APIs with Flask. It provides Resource classes that handle HTTP methods automatically.

## Core Concepts

### Installation

```bash
pip install flask-restful
```

## Code Walkthrough

### Flask-RESTful Example

```python
# app.py
from flask import Flask
from flask_restful import Resource, Api

app = Flask(__name__)
api = Api(app)

class HelloResource(Resource):
    def get(self):
        return {"message": "Hello, World!"}
    
    def post(self):
        return {"message": "Created"}, 201

api.add_resource(HelloResource, "/hello")
```

## Next Steps

Now install Flask-RESTful. Continue to [02_resource_classes.md](02_resource_classes.md) to learn resource classes.