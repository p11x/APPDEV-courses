# API Design Principles

## Introduction

**Application Programming Interface (API) Design Principles** refer to the set of guidelines, best practices, and architectural approaches used to create well-structured, maintainable, and developer-friendly interfaces that enable different software systems to communicate with each other. An API acts as a contract between a service provider and consumers, defining how requests and responses should be structured.

### Why API Design Matters

- **Developer Experience (DX)**: Well-designed APIs are intuitive and easy to use, reducing the learning curve for developers consuming your services.
- **Maintainability**: Good API design allows for easier updates, scaling, and bug fixes without breaking existing integrations.
- **Scalability**: Properly designed APIs can handle increased load and evolve without requiring consumers to make significant changes.
- **Interoperability**: Standardized APIs enable different systems, regardless of technology stack, to communicate seamlessly.
- **Security**: Thoughtful API design incorporates security best practices to protect data and prevent unauthorized access.

---

## RESTful API Design Best Practices

REST (Representational State Transfer) is an architectural style that defines a set of constraints for creating web services. RESTful APIs use HTTP requests to perform CRUD (Create, Read, Update, Delete) operations on resources.

### Core Principles

1. **Client-Server Separation**: The client and server operate independently, allowing each to evolve separately.
2. **Statelessness**: Each request contains all information necessary to process it; the server stores no session state.
3. **Cacheability**: Responses can be marked as cacheable or non-cacheable to improve performance.
4. **Uniform Interface**: Resources are identified by URIs, and interactions use standard HTTP methods.
5. **Layered System**: Intermediate servers (proxies, gateways) can be inserted without affecting the client-server interaction.

---

## Resource Naming Conventions

Resource naming is one of the most critical aspects of API design. Follow these conventions:

### Use Nouns, Not Verbs

```http
# Good
GET /users
POST /orders
GET /products/123

# Bad
GET /getUsers
POST /createOrder
GET /getProductById
```

### Use Plural Nouns for Collections

```http
GET /users          # Collection of users
GET /users/123      # Specific user
GET /users/123/orders  # Nested resource
```

### Use Lowercase with Hyphens

```http
# Good
GET /user-profiles
GET /order-items

# Avoid
GET /userProfiles
GET /order_items
```

### Use Hierarchical Structure for Nested Resources

```http
GET /users/123/orders           # Orders belonging to user 123
GET /orders/456/items          # Items in order 456
GET /users/123/addresses       # User addresses
```

---

## HTTP Methods and Status Codes

### HTTP Methods (Verbs)

| Method | Description | Idempotent |
|--------|-------------|------------|
| GET | Retrieve a resource | Yes |
| POST | Create a new resource | No |
| PUT | Replace a resource entirely | Yes |
| PATCH | Partially update a resource | No |
| DELETE | Remove a resource | Yes |
| HEAD | Retrieve headers only | Yes |
| OPTIONS | List supported methods | Yes |

### HTTP Status Codes

#### 1xx - Informational
- `100 Continue`: Server is ready to accept the request
- `101 Switching Protocols`: Server is switching protocols

#### 2xx - Success
- `200 OK`: Request succeeded
- `201 Created`: Resource was successfully created
- `204 No Content`: Request succeeded but no content to return
- `202 Accepted`: Request accepted for processing

#### 3xx - Redirection
- `301 Moved Permanently`: Resource has moved permanently
- `304 Not Modified`: Cached response is still valid

#### 4xx - Client Errors
- `400 Bad Request`: Invalid syntax or malformed request
- `401 Unauthorized`: Authentication required
- `403 Forbidden`: Server refuses to authorize
- `404 Not Found`: Resource not found
- `409 Conflict`: Request conflicts with current state
- `422 Unprocessable Entity`: Valid request but unable to process
- `429 Too Many Requests`: Rate limit exceeded

#### 5xx - Server Errors
- `500 Internal Server Error`: Unexpected server condition
- `502 Bad Gateway`: Invalid response from upstream server
- `503 Service Unavailable`: Server temporarily overloaded
- `504 Gateway Timeout`: Upstream server failed to respond

---

## Versioning Strategies

API versioning is essential for maintaining backward compatibility while evolving your API.

### Versioning Approaches

#### 1. URL Path Versioning (Most Common)

```http
GET /api/v1/users
GET /api/v2/users
```

**Pros**: Simple, explicit, easy to route
**Cons**: URL changes with version upgrade

#### 2. Query Parameter Versioning

```http
GET /api/users?version=1
GET /api/users?version=2
```

**Pros**: Single URL for all versions
**Cons**: Can be ignored by caching systems, less explicit

#### 3. Header Versioning

```http
GET /api/users
Accept-Version: v1
```

**Pros**: Clean URLs, flexible
**Cons**: Less discoverable, requires custom headers

#### 4. Content Negotiation

```http
GET /api/users
Accept: application/vnd.myapp.v1+json
```

**Pros**: Standard approach, URL remains clean
**Cons**: Complex to implement, less developer-friendly

### Versioning Best Practices

- Always version your API from the start
- Maintain backward compatibility within major versions
- Provide deprecation timelines (typically 12-24 months)
- Include version information in documentation and response headers

---

## Error Handling and Response Formats

### Standard Error Response Format

```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid request payload",
    "details": [
      {
        "field": "email",
        "message": "Invalid email format"
      },
      {
        "field": "password",
        "message": "Password must be at least 8 characters"
      }
    ],
    "trace_id": "abc-123-def-456"
  }
}
```

### Error Codes Reference

| Code | Description |
|------|-------------|
| INVALID_REQUEST | Request body is malformed |
| VALIDATION_ERROR | Request failed validation |
| UNAUTHORIZED | Missing or invalid authentication |
| FORBIDDEN | Authenticated but not authorized |
| NOT_FOUND | Resource does not exist |
| RATE_LIMITED | Too many requests |
| INTERNAL_ERROR | Server-side error |

---

## Pagination and Filtering

### Pagination

#### Offset-Based Pagination

```http
GET /users?limit=20&offset=0
```

Response:
```json
{
  "data": [...],
  "pagination": {
    "limit": 20,
    "offset": 0,
    "total": 1000,
    "has_more": true
  }
}
```

#### Cursor-Based Pagination

```http
GET /users?cursor=eyJpZCI6MTAwfQ
```

Response:
```json
{
  "data": [...],
  "pagination": {
    "next_cursor": "eyJpZCI6MTIwfQ",
    "has_more": true
  }
}
```

#### Keyset Pagination (Seek Method)

```http
GET /users?last_id=100&limit=20
```

**Cursor-based vs Offset-based**: Cursor-based is more efficient for large datasets and provides consistent results when data changes during pagination.

### Filtering

```http
GET /users?status=active&role=admin
GET /orders?created_after=2024-01-01&status=shipped
GET /products?category=electronics&price_min=100&price_max=500
```

### Sorting

```http
GET /users?sort=created_at:desc
GET /products?sort=price:asc,name:desc
```

---

## Security Considerations

### Authentication

#### API Keys

```http
GET /api/resource
X-API-Key: your-api-key-here
```

**Pros**: Simple, stateless
**Cons**: Less secure for sensitive operations

#### OAuth 2.0

```http
GET /api/resource
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

**Pros**: Industry standard, supports scopes
**Cons**: More complex implementation

#### JWT (JSON Web Tokens)

```json
{
  "header": {
    "alg": "RS256",
    "typ": "JWT"
  },
  "payload": {
    "sub": "1234567890",
    "name": "John Doe",
    "iat": 1516239022,
    "exp": 1516242622
  },
  "signature": "..."
}
```

### Rate Limiting

Include rate limit headers in responses:

```http
X-RateLimit-Limit: 1000
X-RateLimit-Remaining: 999
X-RateLimit-Reset: 1640000000
```

### Security Best Practices

1. **Use HTTPS exclusively** - Encrypt all traffic
2. **Implement CORS properly** - Restrict access to trusted domains
3. **Validate all input** - Never trust user data
4. **Use parameterized queries** - Prevent SQL injection
5. **Implement request size limits** - Prevent DoS attacks
6. **Log and monitor** - Detect suspicious activity
7. **Use CSRF tokens** - Protect against cross-site requests

---

## API Documentation

### OpenAPI Specification Example

```yaml
openapi: 3.0.3
info:
  title: User Management API
  version: 1.0.0
  description: API for managing users

servers:
  - url: https://api.example.com/v1
    description: Production server
  - url: https://staging.example.com/v1
    description: Staging server

paths:
  /users:
    get:
      summary: List all users
      parameters:
        - name: limit
          in: query
          schema:
            type: integer
            default: 20
        - name: offset
          in: query
          schema:
            type: integer
            default: 0
      responses:
        '200':
          description: Successful response
          content:
            application/json:
              schema:
                type: object
                properties:
                  data:
                    type: array
                    items:
                      $ref: '#/components/schemas/User'
    post:
      summary: Create a new user
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/CreateUserRequest'
      responses:
        '201':
          description: User created

components:
  schemas:
    User:
      type: object
      properties:
        id:
          type: string
          format: uuid
        email:
          type: string
          format: email
        name:
          type: string
    CreateUserRequest:
      type: object
      required:
        - email
        - name
      properties:
        email:
          type: string
          format: email
        name:
          type: string
```

### Documentation Tools

- **Swagger UI**: Interactive API documentation
- **Redoc**: Beautiful three-panel documentation
- **Apiary**: API design and documentation platform
- **Postman**: API development and documentation

---

## GraphQL vs REST

### REST Characteristics

| Aspect | REST |
|--------|------|
| Data Fetching | Multiple endpoints |
| Response Format | Fixed structure |
| Caching | HTTP caching |
| Over-fetching | Common issue |
| Versioning | URL-based |

### GraphQL Characteristics

| Aspect | GraphQL |
|--------|---------|
| Data Fetching | Single endpoint |
| Response Format | Flexible, client-defined |
| Caching | Custom implementation |
| Over-fetching | Avoided |
| Versioning | Schema evolution |

### GraphQL Example

```graphql
query GetUserWithOrders {
  user(id: "123") {
    name
    email
    orders(limit: 5) {
      id
      total
      items {
        product {
          name
          price
        }
      }
    }
  }
}
```

Response:
```json
{
  "data": {
    "user": {
      "name": "John Doe",
      "email": "john@example.com",
      "orders": [
        {
          "id": "456",
          "total": 99.99,
          "items": [
            {
              "product": {
                "name": "Widget",
                "price": 29.99
              }
            }
          ]
        }
      ]
    }
  }
}
```

### When to Use GraphQL

- Multiple clients with different data requirements
- Mobile apps with limited bandwidth
- Complex data relationships
- Rapid frontend development

### When to Use REST

- Simple CRUD operations
- Public APIs with broad audience
- Strong caching requirements
- Team familiarity with REST

---

## gRPC and Protocol Buffers

### What is gRPC?

gRPC is a high-performance, open-source RPC framework that uses HTTP/2 for transport and Protocol Buffers for serialization.

### Protocol Buffers Definition

```protobuf
syntax = "proto3";

package user;

service UserService {
  rpc GetUser (GetUserRequest) returns (User);
  rpc CreateUser (CreateUserRequest) returns (User);
  rpc ListUsers (ListUsersRequest) returns (ListUsersResponse);
  rpc StreamUserEvents (StreamRequest) returns (stream UserEvent);
}

message GetUserRequest {
  string user_id = 1;
}

message CreateUserRequest {
  string email = 1;
  string name = 2;
}

message User {
  string id = 1;
  string email = 2;
  string name = 3;
  int64 created_at = 4;
}

message ListUsersRequest {
  int32 page_size = 1;
  string page_token = 2;
}

message ListUsersResponse {
  repeated User users = 1;
  string next_page_token = 2;
}

message StreamRequest {
  string user_id = 1;
}

message UserEvent {
  string user_id = 1;
  string event_type = 2;
  int64 timestamp = 3;
}
```

### gRPC Implementation (Go)

```go
package main

import (
    "context"
    "log"
    "net"
    
    "google.golang.org/grpc"
    pb "github.com/example/user-service/proto"
)

type UserService struct {
    pb.UnimplementedUserServiceServer
}

func (s *UserService) GetUser(ctx context.Context, req *pb.GetUserRequest) (*pb.User, error) {
    return &pb.User{
        Id:    req.UserId,
        Email: "user@example.com",
        Name:  "John Doe",
    }, nil
}

func (s *UserService) CreateUser(ctx context.Context, req *pb.CreateUserRequest) (*pb.User, error) {
    return &pb.User{
        Id:    "generated-id",
        Email: req.Email,
        Name:  req.Name,
    }, nil
}

func main() {
    lis, err := net.Listen("tcp", ":50051")
    if err != nil {
        log.Fatalf("Failed to listen: %v", err)
    }
    
    s := grpc.NewServer()
    pb.RegisterUserServiceServer(s, &UserService{})
    
    if err := s.Serve(lis); err != nil {
        log.Fatalf("Failed to serve: %v", err)
    }
}
```

### gRPC Implementation (Node.js)

```javascript
const grpc = require('@grpc/grpc-js');
const protoLoader = require('@grpc/proto-loader');

const packageDefinition = protoLoader.loadSync('user.proto', {
  keepCase: false,
  longs: String,
  enums: String,
  defaults: true,
  oneofs: true
});

const userProto = grpc.loadPackageDefinition(packageDefinition).user;

const client = new userProto.UserService('localhost:50051', grpc.credentials.createInsecure());

client.getUser({ userId: '123' }, (error, user) => {
  if (error) {
    console.error(error);
    return;
  }
  console.log('User:', user);
});

const call = client.streamUserEvents({ userId: '123' });
call.on('data', (event) => {
  console.log('Event:', event);
});
```

### gRPC vs REST Comparison

| Aspect | gRPC | REST |
|--------|------|------|
| Protocol | HTTP/2 | HTTP/1.1 |
| Serialization | Protocol Buffers | JSON/XML |
| Performance | High | Moderate |
| Code Generation | Yes | Optional |
| Browser Support | Limited | Universal |
| Streaming | Yes | Limited |

---

## API Design Patterns Flow Chart

```
┌─────────────────────────────────────────────────────────────────┐
│                    API Design Process                           │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  1. Identify Resources                                         │
│     - What data/exentities are exposed?                        │
│     - Users, Orders, Products, etc.                            │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  2. Define Resource URIs                                       │
│     - Use nouns (plural): /users, /orders                      │
│     - Use hierarchical structure: /users/{id}/orders          │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  3. Choose HTTP Methods                                        │
│     - GET    → Read                                            │
│     - POST   → Create                                          │
│     - PUT    → Replace                                         │
│     - PATCH  → Update                                          │
│     - DELETE → Remove                                          │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  4. Design Response Format                                     │
│     - Standard envelope: { data, meta, errors }                │
│     - Use consistent naming (camelCase)                        │
│     - Include hypermedia links (HATEOAS)                       │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  5. Implement Error Handling                                   │
│     - Use appropriate status codes                             │
│     - Provide detailed error messages                          │
│     - Include error codes and traces                           │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  6. Add Pagination & Filtering                                │
│     - Cursor-based for large datasets                          │
│     - Filterable query parameters                              │
│     - Sortable fields                                          │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  7. Security Implementation                                   │
│     - Authentication (OAuth2, JWT, API Keys)                   │
│     - Rate limiting                                            │
│     - Input validation                                          │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  8. Version Your API                                          │
│     - URL path versioning: /api/v1/                            │
│     - Plan for deprecation                                     │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│  9. Document Your API                                         │
│     - OpenAPI/Swagger specification                            │
│     - Interactive playground                                   │
│     - Examples and SDKs                                        │
└─────────────────────────────────────────────────────────────────┘
```

### API Request-Response Flow

```
┌──────────┐     ┌──────────────┐     ┌─────────────┐
│  Client  │────▶│   API Gate   │────▶│   Service   │
└──────────┘     │    way       │     │             │
                 └──────────────┘     └─────────────┘
                        │                    │
                        ▼                    ▼
                 ┌──────────────┐     ┌─────────────┐
                 │  Rate Limit  │     │   Validate  │
                 │    Check     │     │   Request   │
                 └──────────────┘     └─────────────┘
                        │                    │
                        ▼                    ▼
                 ┌──────────────┐     ┌─────────────┐
                 │   Authen-    │     │  Business   │
                 │   ticate     │     │   Logic     │
                 └──────────────┘     └─────────────┘
                                            │
                                            ▼
                                    ┌─────────────┐
                                    │   Database  │
                                    │  or Cache   │
                                    └─────────────┘
                                            │
                                            ▼
                                    ┌─────────────┐
                                    │  Transform  │
                                    │  Response   │
                                    └─────────────┘
                                            │
                                            ▼
                                    ┌─────────────┐
                 ┌──────────────────│  Send JSON  │
                 │                  │  Response   │
                 ▼                  └─────────────┘
          ┌──────────────┐
          │  Cacheable?  │
          │   (Yes/No)   │
          └──────────────┘
```

---

## Code Examples

### REST API Implementation (Python Flask)

```python
from flask import Flask, request, jsonify
from functools import wraps

app = Flask(__name__)

# In-memory storage
users_db = {}
next_id = 1

def require_auth(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        auth_header = request.headers.get('Authorization')
        if not auth_header or not auth_header.startswith('Bearer '):
            return jsonify({'error': {'code': 'UNAUTHORIZED', 'message': 'Missing or invalid token'}}), 401
        return f(*args, **kwargs)
    return decorated

@app.route('/api/v1/users', methods=['GET'])
def list_users():
    limit = request.args.get('limit', 20, type=int)
    offset = request.args.get('offset', 0, type=int)
    status = request.args.get('status')
    
    users = list(users_db.values())
    
    if status:
        users = [u for u in users if u.get('status') == status]
    
    total = len(users)
    paginated = users[offset:offset + limit]
    
    return jsonify({
        'data': paginated,
        'pagination': {
            'limit': limit,
            'offset': offset,
            'total': total,
            'has_more': offset + limit < total
        }
    })

@app.route('/api/v1/users', methods=['POST'])
@require_auth
def create_user():
    global next_id
    
    data = request.get_json()
    if not data or 'email' not in data or 'name' not in data:
        return jsonify({
            'error': {
                'code': 'VALIDATION_ERROR',
                'message': 'Missing required fields',
                'details': [
                    {'field': 'email', 'message': 'Email is required'},
                    {'field': 'name', 'message': 'Name is required'}
                ]
            }
        }), 422
    
    user_id = str(next_id)
    next_id += 1
    
    user = {
        'id': user_id,
        'email': data['email'],
        'name': data['name'],
        'status': 'active',
        'created_at': '2024-01-15T10:30:00Z'
    }
    users_db[user_id] = user
    
    return jsonify({'data': user}), 201

@app.route('/api/v1/users/<user_id>', methods=['GET'])
@require_auth
def get_user(user_id):
    user = users_db.get(user_id)
    if not user:
        return jsonify({
            'error': {
                'code': 'NOT_FOUND',
                'message': f'User {user_id} not found'
            }
        }), 404
    
    return jsonify({'data': user})

@app.route('/api/v1/users/<user_id>', methods=['PUT'])
@require_auth
def update_user(user_id):
    user = users_db.get(user_id)
    if not user:
        return jsonify({
            'error': {
                'code': 'NOT_FOUND',
                'message': f'User {user_id} not found'
            }
        }), 404
    
    data = request.get_json()
    user.update(data)
    
    return jsonify({'data': user})

@app.route('/api/v1/users/<user_id>', methods=['DELETE'])
@require_auth
def delete_user(user_id):
    if user_id not in users_db:
        return jsonify({
            'error': {
                'code': 'NOT_FOUND',
                'message': f'User {user_id} not found'
            }
        }), 404
    
    del users_db[user_id]
    
    return '', 204

if __name__ == '__main__':
    app.run(debug=True, port=5000)
```

### REST API Implementation (Node.js Express)

```javascript
const express = require('express');
const rateLimit = require('express-rate-limit');
const helmet = require('helmet');

const app = express();

app.use(helmet());
app.use(express.json());

// Rate limiting
const limiter = rateLimit({
  windowMs: 15 * 60 * 1000,
  max: 100,
  message: { error: { code: 'RATE_LIMITED', message: 'Too many requests' } }
});
app.use('/api/', limiter);

// In-memory storage
const users = new Map();
let nextId = 1;

// Middleware for authentication
const authenticate = (req, res, next) => {
  const authHeader = req.headers.authorization;
  if (!authHeader || !authHeader.startsWith('Bearer ')) {
    return res.status(401).json({
      error: { code: 'UNAUTHORIZED', message: 'Missing or invalid token' }
    });
  }
  next();
};

// List users
app.get('/api/v1/users', authenticate, (req, res) => {
  const { limit = 20, offset = 0, status, sort = 'id:asc' } = req.query;
  
  let userList = Array.from(users.values());
  
  if (status) {
    userList = userList.filter(u => u.status === status);
  }
  
  const [sortField, sortOrder] = sort.split(':');
  userList.sort((a, b) => {
    const aVal = a[sortField];
    const bVal = b[sortField];
    const cmp = aVal < bVal ? -1 : aVal > bVal ? 1 : 0;
    return sortOrder === 'desc' ? -cmp : cmp;
  });
  
  const total = userList.length;
  const paginated = userList.slice(Number(offset), Number(offset) + Number(limit));
  
  res.json({
    data: paginated,
    pagination: { limit: Number(limit), offset: Number(offset), total, has_more: Number(offset) + Number(limit) < total }
  });
});

// Create user
app.post('/api/v1/users', authenticate, (req, res) => {
  const { email, name } = req.body;
  
  if (!email || !name) {
    return res.status(422).json({
      error: {
        code: 'VALIDATION_ERROR',
        message: 'Missing required fields',
        details: [
          !email && { field: 'email', message: 'Email is required' },
          !name && { field: 'name', message: 'Name is required' }
        ].filter(Boolean)
      }
    });
  }
  
  const id = String(nextId++);
  const user = { id, email, name, status: 'active', created_at: new Date().toISOString() };
  users.set(id, user);
  
  res.status(201).json({ data: user });
});

// Get user
app.get('/api/v1/users/:id', authenticate, (req, res) => {
  const user = users.get(req.params.id);
  if (!user) {
    return res.status(404).json({
      error: { code: 'NOT_FOUND', message: `User ${req.params.id} not found` }
    });
  }
  res.json({ data: user });
});

// Update user
app.put('/api/v1/users/:id', authenticate, (req, res) => {
  const user = users.get(req.params.id);
  if (!user) {
    return res.status(404).json({
      error: { code: 'NOT_FOUND', message: `User ${req.params.id} not found` }
    });
  }
  
  const updated = { ...user, ...req.body, id: user.id };
  users.set(req.params.id, updated);
  res.json({ data: updated });
});

// Delete user
app.delete('/api/v1/users/:id', authenticate, (req, res) => {
  if (!users.has(req.params.id)) {
    return res.status(404).json({
      error: { code: 'NOT_FOUND', message: `User ${req.params.id} not found` }
    });
  }
  users.delete(req.params.id);
  res.status(204).send();
});

app.listen(3000, () => console.log('API running on port 3000'));
```

### REST API Implementation (Java Spring Boot)

```java
package com.example.api.controller;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

@RestController
@RequestMapping("/api/v1/users")
public class UserController {

    private final Map<String, User> users = new ConcurrentHashMap<>();
    private long nextId = 1;

    @GetMapping
    public ResponseEntity<Map<String, Object>> listUsers(
            @RequestParam(defaultValue = "20") int limit,
            @RequestParam(defaultValue = "0") int offset,
            @RequestParam(required = false) String status) {
        
        List<User> userList = new ArrayList<>(users.values());
        
        if (status != null) {
            userList.removeIf(u -> !status.equals(u.getStatus()));
        }
        
        int total = userList.size();
        List<User> paginated = userList.subList(
            Math.min(offset, total),
            Math.min(offset + limit, total)
        );
        
        Map<String, Object> response = new HashMap<>();
        response.put("data", paginated);
        response.put("pagination", Map.of(
            "limit", limit,
            "offset", offset,
            "total", total,
            "has_more", offset + limit < total
        ));
        
        return ResponseEntity.ok(response);
    }

    @PostMapping
    public ResponseEntity<Map<String, Object>> createUser(@RequestBody CreateUserRequest request) {
        if (request.getEmail() == null || request.getName() == null) {
            Map<String, Object> error = new HashMap<>();
            error.put("error", Map.of(
                "code", "VALIDATION_ERROR",
                "message", "Missing required fields",
                "details", Arrays.asList(
                    Map.of("field", "email", "message", "Email is required"),
                    Map.of("field", "name", "message", "Name is required")
                )
            ));
            return ResponseEntity.status(HttpStatus.UNPROCESSABLE_ENTITY).body(error);
        }
        
        User user = new User();
        user.setId(String.valueOf(nextId++));
        user.setEmail(request.getEmail());
        user.setName(request.getName());
        user.setStatus("active");
        user.setCreatedAt(new Date());
        
        users.put(user.getId(), user);
        
        return ResponseEntity.status(HttpStatus.CREATED).body(Map.of("data", user));
    }

    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getUser(@PathVariable String id) {
        User user = users.get(id);
        if (user == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(
                Map.of("error", Map.of("code", "NOT_FOUND", "message", "User not found"))
            );
        }
        return ResponseEntity.ok(Map.of("data", user));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteUser(@PathVariable String id) {
        if (!users.containsKey(id)) {
            return ResponseEntity.notFound().build();
        }
        users.remove(id);
        return ResponseEntity.noContent().build();
    }

    static class User {
        private String id;
        private String email;
        private String name;
        private String status;
        private Date createdAt;
        
        public String getId() { return id; }
        public void setId(String id) { this.id = id; }
        public String getEmail() { return email; }
        public void setEmail(String email) { this.email = email; }
        public String getName() { return name; }
        public void setName(String name) { this.name = name; }
        public String getStatus() { return status; }
        public void setStatus(String status) { this.status = status; }
        public Date getCreatedAt() { return createdAt; }
        public void setCreatedAt(Date createdAt) { this.createdAt = createdAt; }
    }

    static class CreateUserRequest {
        private String email;
        private String name;
        
        public String getEmail() { return email; }
        public void setEmail(String email) { this.email = email; }
        public String getName() { return name; }
        public void setName(String name) { this.name = name; }
    }
}
```

---

## Real-World Examples from Companies

### Stripe API

Stripe provides an excellent example of well-designed APIs:

```http
# List charges
GET /v1/charges?limit=10

# Create a charge
POST /v1/charges
{
  "amount": 2000,
  "currency": "usd",
  "customer": "cus_123",
  "description": "Charge for order #12345"
}
```

**Best Practices from Stripe**:
- Consistent URL structure: `/v1/{resource}`
- Expandable nested resources
- Idiomatic response formatting
- Comprehensive error responses
- Pagination using `has_more` and pagination cursors

### GitHub API

```http
# List user repositories
GET /users/{username}/repos?sort=updated&direction=desc

# Response includes pagination
{
  "data": [...],
  "pagination": {
    "next": "https://api.github.com/user/repos?page=2"
  }
}
```

**Best Practices from GitHub**:
- RESTful resource naming
- Content negotiation
- Clear rate limit headers
- Hypermedia links in responses

### Twilio API

```http
# Send SMS
POST /2010-04-01/Accounts/{AccountSid}/Messages.json
{
  "To": "+1234567890",
  "From": "+0987654321",
  "Body": "Hello from API"
}
```

**Best Practices from Twilio**:
- URL-based versioning
- Resource-oriented design
- Clear error codes and messages

### Shopify API

```http
# Create a product
POST /admin/api/2024-01/products.json
{
  "product": {
    "title": "Widget",
    "body_html": "<p>Description</p>",
    "vendor": "Acme",
    "product_type": "Widgets"
  }
}
```

**Best Practices from Shopify**:
- Date-based versioning
- Nested resource creation
- Webhook support for events

---

## Best Practices Summary

### Design Principles

1. **Use nouns for resources, verbs for actions**
   - `GET /users` not `GET /getUsers`

2. **Keep URLs short and meaningful**
   - `/users/123/orders` is clearer than `/getUserOrders?userId=123`

3. **Use consistent naming conventions**
   - Choose snake_case, camelCase, or kebab-case and stick with it

4. **Version your API from day one**
   - `/api/v1/resource` provides clear upgrade path

5. **Use appropriate HTTP status codes**
   - 200 for success, 201 for creation, 400 for bad request, etc.

6. **Implement pagination for all list endpoints**
   - Use cursors for large datasets

7. **Provide comprehensive error responses**
   - Include error codes, messages, and field-level details

8. **Use HTTPS exclusively**
   - Never expose APIs over plain HTTP

9. **Implement rate limiting**
   - Protect your API from abuse

10. **Document everything**
    - Use OpenAPI/Swagger for machine-readable docs

### Security Checklist

- [ ] Use HTTPS for all endpoints
- [ ] Implement authentication (OAuth2, JWT, or API keys)
- [ ] Add rate limiting
- [ ] Validate all input
- [ ] Sanitize output to prevent injection
- [ ] Use security headers (Helmet, CORS)
- [ ] Log security events
- [ ] Keep dependencies updated
- [ ] Implement CSRF protection where applicable
- [ ] Use secure random number generation

### Performance Considerations

- [ ] Implement caching appropriately
- [ ] Use compression (gzip)
- [ ] Optimize database queries
- [ ] Use pagination to limit response size
- [ ] Consider async operations for long-running tasks
- [ ] Use CDN for static assets
- [ ] Monitor performance metrics

---

## Conclusion

API Design Principles are fundamental to building scalable, maintainable, and developer-friendly applications. By following RESTful conventions, implementing proper security, versioning strategically, and documenting thoroughly, you create APIs that stand the test of time.

Whether you choose REST, GraphQL, or gRPC depends on your specific use case, but the underlying principles of clarity, consistency, and developer experience remain universal.

Remember: **An API is a product you build for developers. Invest in making it exceptional.**

---

## References

- [RFC 7231 - HTTP/1.1 Semantics and Content](https://tools.ietf.org/html/rfc7231)
- [OpenAPI Specification](https://spec.openapis.org/oas/v3.0.3)
- [REST API Design - Best Practices](https://blog.restcase.com/)
- [Google API Design Guide](https://cloud.google.com/apis/design)
- [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines)
