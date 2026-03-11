# API Testing

## Concept Title and Overview

Now that you've created REST APIs, you need to know how to test them. In this lesson, you'll learn to use Postman and VS Code REST Client to test your Spring Boot endpoints. These tools are essential for verifying your backend works correctly before connecting it to Angular.

## Real-World Importance and Context

Imagine building a complex machine without ever testing it—you'd never know if it works! API testing is exactly that: verifying your backend endpoints work correctly.

Professional developers test their APIs constantly:
- During development to verify individual endpoints
- Before connecting the frontend
- As part of automated testing pipelines
- When debugging production issues

Learning these tools will make you a more effective developer and help you debug issues faster.

## Detailed Step-by-Step Explanation

### Postman Installation and Setup

Postman is a powerful API testing tool used by millions of developers.

#### Installation:

**Windows/macOS/Linux:**
1. Visit: https://www.postman.com/downloads/
2. Download the desktop app (recommended over web version)
3. Install and launch
4. Create a free account (optional but recommended for saving work)

#### Postman Interface Overview:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         POSTMAN INTERFACE                               │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  ← → GET  ▼ │ https://api.example.com/users           │ Send │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  PARAMS   AUTH   HEADERS   BODY   PRE-SCRIPT  TESTS   SETTINGS │   │
│  ├─────────────────────────────────────────────────────────────────┤   │
│  │                                                                 │   │
│  │  Key-Value Editor                                              │   │
│  │  ○ header  : value                                             │   │
│  │  ○ key     : value                                             │   │
│  │                                                                 │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  RESPONSE                                                       │   │
│  │  Body   Cookies   Headers   Test Results                       │   │
│  │                                                                 │   │
│  │  {                                                             │   │
│  │    "id": 1,                                                    │   │
│  │    "name": "John Doe",                                         │   │
│  │    "email": "john@example.com"                                 │   │
│  │  }                                                             │   │
│  │                                                                 │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Creating and Organizing Request Collections

Collections help organize related API requests:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                     POSTMAN COLLECTIONS                                │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  📁 My API Project                                                     │
│  │  ├── 👤 Users                                                    │
│  │  │  ├── ○ Get All Users                                          │
│  │  │  ├── ○ Get User by ID                                          │
│  │  │  ├── ○ Create User                                            │
│  │  │  ├── ○ Update User                                            │
│  │  │  └── ○ Delete User                                            │
│  │  │                                                                  │
│  │  ├── 📦 Products                                                 │
│  │  │  ├── ○ Get All Products                                       │
│  │  │  ├── ○ Get Product by ID                                      │
│  │  │  └── ○ Create Product                                         │
│  │  │                                                                  │
│  │  └── 🔐 Auth                                                     │
│  │     ├── ○ Login                                                  │
│  │     └── ○ Register                                               │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

**Creating a Collection:**
1. Click "Collections" in the left sidebar
2. Click the "+" icon
3. Name your collection (e.g., "Employee API")
4. Right-click collection → "Add Request"

### Testing All HTTP Methods

Let's test each HTTP method with a running Spring Boot app:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                   TESTING HTTP METHODS IN POSTMAN                        │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  GET REQUEST                                                            │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Method: GET                                                      │   │
│  │ URL: http://localhost:8080/api/users                           │   │
│  │                                                                  │   │
│  │ Click "Send" → See list of users in response                   │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  POST REQUEST (Creating Data)                                           │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Method: POST                                                     │   │
│  │ URL: http://localhost:8080/api/users                           │   │
│  │ Headers: Content-Type: application/json                        │   │
│  │ Body (raw, JSON):                                               │   │
│  │ {                                                               │   │
│  │   "name": "Jane Smith",                                         │   │
│  │   "email": "jane@example.com",                                  │   │
│  │   "role": "USER"                                                │   │
│  │ }                                                               │   │
│  │                                                                  │   │
│  │ Click "Send" → See created user with ID in response            │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  PUT REQUEST (Updating Data)                                            │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Method: PUT                                                     │   │
│  │ URL: http://localhost:8080/api/users/1                        │   │
│  │ Headers: Content-Type: application/json                        │   │
│  │ Body (raw, JSON):                                               │   │
│  │ {                                                               │   │
│  │   "name": "Jane Updated",                                       │   │
│  │   "email": "updated@example.com",                               │   │
│  │   "role": "ADMIN",                                              │   │
│  │   "active": true                                                │   │
│  │ }                                                               │   │
│  │                                                                  │   │
│  │ Click "Send" → See updated user in response                    │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  DELETE REQUEST (Deleting Data)                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Method: DELETE                                                   │   │
│  │ URL: http://localhost:8080/api/users/1                         │   │
│  │                                                                  │   │
│  │ Click "Send" → See 204 No Content response                     │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Handling Authentication in Requests

For APIs that require authentication:

**1. Basic Auth:**
```
Authorization → Type: Basic Auth
Username: admin
Password: secret123
```

**2. Bearer Token (JWT):**
```
Authorization → Type: Bearer Token
Token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**3. API Key:**
```
Authorization → Type: API Key
Key: X-API-Key
Value: your-api-key-here
Add to: Header
```

### VS Code REST Client Extension

The REST Client extension lets you test APIs directly in VS Code without leaving your editor.

#### Installation:

1. Open VS Code
2. Go to Extensions (Ctrl+Shift+X)
3. Search for "REST Client"
4. Install "REST Client" by Huachao Mao

#### Creating a Request File:

Create a new file called `api-test.http`:

```http
### Get all users
GET http://localhost:8080/api/users
Accept: application/json

### Get user by ID
GET http://localhost:8080/api/users/1
Accept: application/json

### Create a new user
POST http://localhost:8080/api/users
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "role": "USER"
}

### Update user
PUT http://localhost:8080/api/users/1
Content-Type: application/json

{
  "name": "John Updated",
  "email": "updated@example.com",
  "role": "ADMIN",
  "active": true
}

### Delete user
DELETE http://localhost:8080/api/users/1

### Get with query parameters
GET http://localhost:8080/api/users?role=ADMIN&active=true
```

**Running Requests:**
- Click "Send Request" above each `###` section
- Or use shortcut `Ctrl+Alt+R`

**Viewing Responses:**
- Response appears in a split pane to the right

### Advanced Postman Features

**1. Environment Variables:**

Create environments (Dev, Staging, Prod):

```bash
{{baseUrl}}/api/users
```

Where:
```
Dev: baseUrl = http://localhost:8080
Prod: baseUrl = https://api.production.com
```

**2. Pre-request Scripts:**

```javascript
// Generate timestamp
pm.environment.set("timestamp", new Date().toISOString());

// Generate random ID
pm.environment.set("randomId", Math.floor(Math.random() * 1000));
```

**3. Test Scripts:**

```javascript
// Test response status
pm.test("Status is 200", () => {
    pm.response.to.have.status(200);
});

// Test response body
pm.test("User has name", () => {
    const jsonData = pm.response.json();
    pm.expect(jsonData.name).to.not.be.null;
});

// Test response time
pm.test("Response time under 500ms", () => {
    pm.expect(pm.response.responseTime).to.be.below(500);
});
```

## Complete Testing Workflow

Here's a complete workflow for testing your Employee Management API:

### Step 1: Start Your Spring Boot Application

```bash
cd your-project
./mvnw spring-boot:run
```

### Step 2: Test Each Endpoint with Postman

**GET All Employees:**
```
GET http://localhost:8080/api/employees
```
Expected: 200 OK, JSON array

**POST Create Employee:**
```
POST http://localhost:8080/api/employees
Content-Type: application/json

{
  "name": "John Smith",
  "email": "john@company.com",
  "department": "Engineering",
  "salary": 75000
}
```
Expected: 201 Created, JSON with new employee

**GET Employee by ID:**
```
GET http://localhost:8080/api/employees/1
```
Expected: 200 OK, JSON employee object

**PUT Update Employee:**
```
PUT http://localhost:8080/api/employees/1
Content-Type: application/json

{
  "name": "John Smith Updated",
  "email": "john.updated@company.com",
  "department": "Sales",
  "salary": 80000
}
```
Expected: 200 OK, JSON updated employee

**DELETE Employee:**
```
DELETE http://localhost:8080/api/employees/1
```
Expected: 204 No Content

### Step 3: Test Error Cases

**GET Non-existent Employee:**
```
GET http://localhost:8080/api/employees/9999
```
Expected: 404 Not Found

**POST with Invalid Data:**
```
POST http://localhost:8080/api/employees
Content-Type: application/json

{
  "name": "",  // Invalid - empty
  "email": "not-an-email"  // Invalid format
}
```
Expected: 400 Bad Request

## Using curl (Command Line)

For quick testing without installing anything:

```bash
# GET request
curl http://localhost:8080/api/users

# GET with query params
curl "http://localhost:8080/api/users?role=admin"

# POST with body
curl -X POST http://localhost:8080/api/users \
  -H "Content-Type: application/json" \
  -d '{"name":"John","email":"john@example.com"}'

# PUT with body
curl -X PUT http://localhost:8080/api/users/1 \
  -H "Content-Type: application/json" \
  -d '{"name":"Jane","email":"jane@example.com"}'

# DELETE
curl -X DELETE http://localhost:8080/api/users/1

# With Bearer token
curl http://localhost:8080/api/protected \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Student Hands-On Exercises

### Exercise 1: Test Hello World (Easy)
Start your Spring Boot app and test the /api/hello endpoint using:
- Browser
- curl
- Postman
- VS Code REST Client

Document the results from each method.

### Exercise 2: Full CRUD Testing (Medium)
Test the complete CRUD cycle:
1. Create an employee (POST)
2. Get all employees (GET)
3. Get the new employee by ID (GET)
4. Update the employee (PUT)
5. Delete the employee (DELETE)
6. Verify deletion (GET - should return 404)

### Exercise 3: Error Testing (Medium)
Test your API's error handling:
- Request a non-existent resource
- Send invalid JSON
- Send incomplete data
- Document the error responses

### Exercise 4: Collection Creation (Medium)
Create a Postman collection with all your API endpoints. Include:
- All CRUD operations
- Proper request body examples
- Tests that verify responses

### Exercise 5: Automate Testing (Hard)
Write a test script in Postman that:
- Creates a user
- Verifies the user was created
- Updates the user
- Verifies the update
- Deletes the user
- Verifies deletion

---

## Summary

In this lesson, you've learned:
- How to install and use Postman
- How to create and organize request collections
- How to test all HTTP methods
- How to handle authentication
- How to use VS Code REST Client
- How to use curl for quick testing

You can now thoroughly test your Spring Boot APIs! With this foundation, you're ready to move into the Intermediate level where we'll explore database integration and more complex patterns.

---

**Next Lesson**: In the next lesson, we'll explore [Dependency Injection](08_Dependency_Injection.md) and understand the foundation of Spring's architecture.
