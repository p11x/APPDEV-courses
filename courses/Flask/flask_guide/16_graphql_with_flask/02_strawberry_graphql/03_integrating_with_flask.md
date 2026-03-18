<!-- FILE: 16_graphql_with_flask/02_strawberry_graphql/03_integrating_with_flask.md -->

## Overview

Integrate Strawberry with Flask.

## Code Walkthrough

```python
# flask_graphql.py
from flask import Flask
import strawberry
from strawberry.flask.views import GraphQL

app = Flask(__name__)

@strawberry.type
class Query:
    @strawberry.field
    def hello(self) -> str:
        return "Hello, World!"

schema = strawberry.Schema(query=Query)

app.add_url_rule("/graphql", view_func=GraphQL.as_view(schema=schema))

if __name__ == "__main__":
    app.run()
```
