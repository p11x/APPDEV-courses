<!-- FILE: 02_routing_and_views/02_http_methods/02_put_patch_delete.md -->

## Overview

Beyond GET and POST, HTTP defines several other methods: **PUT**, **PATCH**, and **DELETE**. These are essential for building RESTful APIs and handling CRUD (Create, Read, Update, Delete) operations. PUT replaces an entire resource, PATCH updates part of it, and DELETE removes it. This file shows how to handle these methods in Flask.

## Prerequisites

- Understanding of GET and POST (from the previous file)
- Basic understanding of REST APIs
- Familiarity with JSON data

## Core Concepts

### PUT vs PATCH

| Method | Purpose | Idempotent | Description |
|--------|---------|------------|-------------|
| **PUT** | Replace entire resource | Yes | Sends complete replacement; missing fields are set to null/default |
| **PATCH** | Partial update | No (usually) | Sends only changed fields; other fields remain unchanged |
| **DELETE** | Remove resource | Yes | Deletes the specified resource |

### Idempotency

An operation is **idempotent** if performing it multiple times has the same effect as performing it once:
- PUT is idempotent: replacing a resource twice gives the same result
- POST is not idempotent: creating a resource twice creates two resources

### Method Override

HTML forms only support GET and POST. To use PUT, PATCH, or DELETE from HTML forms, you need to use a workaround: a hidden `_method` field that Flask can intercept and treat as the specified method.

## Code Walkthrough

### Full CRUD Example with All Methods

```python
# crud_example.py — Complete CRUD operations with PUT, PATCH, DELETE
from flask import Flask, request, jsonify, abort

app = Flask(__name__)

# In-memory database (use a real database in production)
# Format: {id: {"title": str, "content": str}}
posts = {
    1: {"title": "First Post", "content": "Hello World!"},
    2: {"title": "Second Post", "content": "Another post here."}
}
next_id = 3  # Auto-increment ID

# Helper function to get post or 404
def get_post_or_404(post_id):
    """Get a post by ID or abort with 404."""
    if post_id not in posts:
        abort(404, description="Post not found")
    return posts[post_id]

# CREATE - POST /posts (create new post)
@app.route("/posts", methods=["POST"])
def create_post():
    """
    Create a new blog post.
    
    Request body (JSON):
        {"title": "string", "content": "string"}
    
    Returns (201):
        {"id": int, "title": "string", "content": "string"}
    """
    global next_id
    
    # Get JSON data from request
    data = request.get_json()
    
    # Validate required fields
    if not data or "title" not in data:
        abort(400, description="Title is required")
    
    # Create the post
    post_id = next_id
    next_id += 1
    posts[post_id] = {
        "title": data["title"],
        "content": data.get("content", "")  # Optional field
    }
    
    # Return 201 Created with the new resource
    return jsonify({"id": post_id, **posts[post_id]}), 201

# READ ALL - GET /posts (list all posts)
@app.route("/posts", methods=["GET"])
def get_posts():
    """
    Get all posts.
    
    Returns (200):
        [{"id": 1, "title": "...", "content": "..."}, ...]
    """
    # Convert dict to list with IDs
    return jsonify([{"id": pid, **post} for pid, post in posts.items()])

# READ ONE - GET /posts/<id> (get single post)
@app.route("/posts/<int:post_id>", methods=["GET"])
def get_post(post_id):
    """
    Get a specific post by ID.
    
    Returns (200):
        {"id": int, "title": "string", "content": "string"}
    """
    post = get_post_or_404(post_id)
    return jsonify({"id": post_id, **post})

# UPDATE - PUT /posts/<id> (replace entire post)
@app.route("/posts/<int:post_id>", methods=["PUT"])
def update_post_put(post_id):
    """
    Replace an entire post (PUT).
    
    Request body (JSON):
        {"title": "string", "content": "string"}
    
    All fields are required - missing fields become empty.
    """
    post = get_post_or_404(post_id)
    data = request.get_json()
    
    if not data or "title" not in data:
        abort(400, description="Title is required for PUT")
    
    # Replace entire resource
    posts[post_id] = {
        "title": data["title"],
        "content": data.get("content", "")  # Empty if not provided
    }
    
    return jsonify({"id": post_id, **posts[post_id]})

# UPDATE - PATCH /posts/<id> (partial update)
@app.route("/posts/<int:post_id>", methods=["PATCH"])
def update_post_patch(post_id):
    """
    Partially update a post (PATCH).
    
    Request body (JSON):
        {"title": "string"}  # Only include fields to update
    
    Only updates provided fields; others remain unchanged.
    """
    post = get_post_or_404(post_id)
    data = request.get_json()
    
    if not data:
        abort(400, description="No data provided")
    
    # Only update fields that are provided
    if "title" in data:
        posts[post_id]["title"] = data["title"]
    if "content" in data:
        posts[post_id]["content"] = data["content"]
    
    return jsonify({"id": post_id, **posts[post_id]})

# DELETE - DELETE /posts/<id>
@app.route("/posts/<int:post_id>", methods=["DELETE"])
def delete_post(post_id):
    """
    Delete a post.
    
    Returns (204):
        No content
    """
    get_post_or_404(post_id)
    del posts[post_id]
    
    # Return 204 No Content (successful, no response body)
    return "", 204

if __name__ == "__main__":
    app.run(debug=True)
```

### Testing with curl

```bash
# GET all posts
curl http://127.0.0.1:5000/posts
# [{"content": "Hello World!", "id": 1, "title": "First Post"}, ...]

# GET single post
curl http://127.0.0.1:5000/posts/1
# {"content": "Hello World!", "id": 1, "title": "First Post"}

# POST create new post
curl -X POST -H "Content-Type: application/json" -d '{"title":"New Post","content":"Content here"}' http://127.0.0.1:5000/posts
# {"content": "Content here", "id": 3, "title": "New Post"}

# PUT replace entire post
curl -X PUT -H "Content-Type: application/json" -d '{"title":"Updated Title","content":"Updated content"}' http://127.0.0.1:5000/posts/1
# {"content": "Updated content", "id": 1, "title": "Updated Title"}

# PATCH partial update
curl -X PATCH -H "Content-Type: application/json" -d '{"content":"Just content updated"}' http://127.0.0.1:5000/posts/1
# {"content": "Just content updated", "id": 1, "title": "Updated Title"}

# DELETE
curl -X DELETE http://127.0.0.1:5000/posts/1
# (no output - 204 No Content)
```

## Common Mistakes

❌ **Confusing PUT and PATCH**
```python
# WRONG — Using PUT for partial updates
@app.route("/posts/<int:id>", methods=["PUT"])
def update_post(id):
    data = request.get_json()
    # This would erase any fields not provided
    post.title = data.get("title")  # If 'content' not in data, loses content!
```

✅ **Correct — Use PATCH for partial updates**
```python
# CORRECT — Use PATCH when not all fields are required
@app.route("/posts/<int:id>", methods=["PATCH"])
def update_post(id):
    data = request.get_json()
    if "title" in data:
        post.title = data["title"]
    # Only updates what's provided; other fields stay the same
```

❌ **Not returning proper status codes**
```python
# WRONG — Returning 200 for resource creation
@app.route("/posts", methods=["POST"])
def create_post():
    return jsonify({"id": 1})  # Should return 201!
```

✅ **Correct — Use proper status codes**
```python
# CORRECT — Return 201 for creation, 204 for no content
@app.route("/posts", methods=["POST"])
def create_post():
    return jsonify({"id": 1}), 201  # 201 Created
```

## Quick Reference

| Method | Status Code | Use For |
|--------|-------------|---------|
| POST | 201 Created | Creating new resources |
| GET | 200 OK | Retrieving resources |
| PUT | 200 OK | Replacing entire resource |
| PATCH | 200 OK | Partially updating resource |
| DELETE | 204 No Content | Deleting resource |

## Next Steps

Now you understand all HTTP methods. Continue to [03_request_object.md](03_request_object.md) to learn more about Flask's request object and how to access headers, cookies, and files.