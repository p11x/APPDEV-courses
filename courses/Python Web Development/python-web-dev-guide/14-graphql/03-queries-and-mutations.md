# Queries and Mutations

## What You'll Learn
- GraphQL queries and mutations

## Queries

```python
@strawberry.type
class Post:
    id: int
    title: str
    content: str

@strawberry.type
class Query:
    @strawberry.field
    def posts(self) -> list[Post]:
        return [
            Post(id=1, title="First Post", content="Hello"),
            Post(id=2, title="Second Post", content="World")
        ]
    
    @strawberry.field
    def post(self, id: int) -> Post | None:
        return Post(id=id, title="Post", content="Content")
```

## Mutations

```python
@strawberry.type
class Mutation:
    @strawberry.mutation
    def create_post(self, title: str, content: str) -> Post:
        post = Post(id=3, title=title, content=content)
        return post
```

## Summary
- Queries fetch data
- Mutations modify data
- Both use GraphQL syntax
