# REST API Best Practices with Express

## What You'll Learn

- RESTful API design principles
- API versioning strategies
- API documentation with OpenAPI
- API response standards

## RESTful API Design

```javascript
import express from 'express';
const app = express();
app.use(express.json());

// Resource-based routes
app.get('/api/users', listUsers);         // GET collection
app.get('/api/users/:id', getUser);       // GET single
app.post('/api/users', createUser);       // CREATE
app.put('/api/users/:id', replaceUser);   // REPLACE
app.patch('/api/users/:id', updateUser);  // UPDATE
app.delete('/api/users/:id', deleteUser); // DELETE

// Nested resources
app.get('/api/users/:userId/posts', getUserPosts);
app.post('/api/users/:userId/posts', createUserPost);

// Consistent response format
function success(res, data, meta = {}) {
    res.json({ data, meta });
}

function created(res, data, location) {
    if (location) res.location(location);
    res.status(201).json({ data });
}

function paginated(res, data, { page, limit, total }) {
    res.json({
        data,
        meta: {
            page,
            limit,
            total,
            totalPages: Math.ceil(total / limit),
        },
    });
}
```

## API Documentation with OpenAPI

```javascript
// swagger.json
{
    "openapi": "3.0.0",
    "info": { "title": "User API", "version": "1.0.0" },
    "paths": {
        "/api/users": {
            "get": {
                "summary": "List users",
                "parameters": [
                    { "name": "page", "in": "query", "schema": { "type": "integer" } },
                    { "name": "limit", "in": "query", "schema": { "type": "integer" } }
                ],
                "responses": {
                    "200": {
                        "description": "Success",
                        "content": {
                            "application/json": {
                                "schema": {
                                    "type": "object",
                                    "properties": {
                                        "data": { "type": "array" },
                                        "meta": { "type": "object" }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```

## Best Practices Checklist

- [ ] Use plural nouns for resources
- [ ] Use proper HTTP methods and status codes
- [ ] Implement pagination for collections
- [ ] Document all endpoints with OpenAPI
- [ ] Use consistent response format

## Cross-References

- See [Middleware](../03-middleware-guide/01-custom-middleware.md) for middleware
- See [Error Handling](../08-error-handling/01-centralized-errors.md) for errors
- See [Security](../05-security-implementation/01-helmet-cors.md) for security

## Next Steps

Continue to [GraphQL Integration](./02-graphql-integration.md) for GraphQL.
