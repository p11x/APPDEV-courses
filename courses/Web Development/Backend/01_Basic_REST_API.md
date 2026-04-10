---
Category: Web Development
Subcategory: Backend
Concept: REST API Design
Purpose: Understanding RESTful API design principles
Difficulty: beginner
Prerequisites: HTTP Fundamentals
RelatedFiles: 02_Advanced_REST.md
UseCase: Building cloud-native APIs
LastUpdated: 2025
---

## WHY

REST APIs are the foundation of cloud-native applications and microservices.

## WHAT

### REST Principles

- **Stateless**: No server-side state
- **Client-Server**: Independent components
- **Uniform Interface**: Resource-based URLs
- **Cacheable**: Response caching

### HTTP Methods

| Method | Action |
|--------|--------|
| GET | Read |
| POST | Create |
| PUT | Update (full) |
| PATCH | Update (partial) |
| DELETE | Remove |

## HOW

### Example: API Design

```javascript
// GET /users - List all users
app.get('/users', async (req, res) => {
  const users = await User.findAll();
  res.json(users);
});

// GET /users/:id - Get user by ID
app.get('/users/:id', async (req, res) => {
  const user = await User.findById(req.params.id);
  res.json(user);
});

// POST /users - Create user
app.post('/users', async (req, res) => {
  const user = await User.create(req.body);
  res.status(201).json(user);
});
```

## BEST PRACTICES

- Use nouns for resources (/users not /getUsers)
- Version APIs (/v1/users)
- Return appropriate status codes
- Handle errors consistently

## CROSS-REFERENCES

### Related Technologies

- GraphQL: Alternative API
- API Gateway: AWS API management