# Bookmark CRUD Tests

## What You'll Build In This File

Complete tests for bookmark CRUD operations with authentication.

## Complete Bookmark Tests

Create `tests/bookmarks.test.js`:

```javascript
// tests/bookmarks.test.js - Bookmark endpoint tests
// Tests all CRUD operations with auth

import { test, describe, before, after } from 'node:test';
import assert from 'node:assert';
import request from 'supertest';

describe('Bookmark Endpoints', () => {
  let app;
  let baseUrl;
  let authToken;
  let testUser = { email: 'bookmark-test@test.com', password: 'testpass123' };
  
  before(async () => {
    // Import app
    const appModule = await import('../src/index.js');
    app = appModule.default;
    baseUrl = `http://localhost:${app.locals.port || 3000}`;
    
    // Create test user and get token
    const registerResponse = await request(baseUrl)
      .post('/auth/register')
      .send(testUser);
    
    authToken = registerResponse.body.token;
  });
  
  describe('POST /bookmarks (Create)', () => {
    test('should create bookmark with valid data', async () => {
      const bookmark = {
        title: 'Node.js',
        url: 'https://nodejs.org',
        description: 'JavaScript runtime'
      };
      
      const response = await request(baseUrl)
        .post('/bookmarks')
        .set('Authorization', `Bearer ${authToken}`)
        .send(bookmark)
        .expect('Content-Type', /json/);
      
      assert.strictEqual(response.status, 201);
      assert.strictEqual(response.body.title, bookmark.title);
      assert.strictEqual(response.body.url, bookmark.url);
    });
    
    test('should reject without authentication', async () => {
      const response = await request(baseUrl)
        .post('/bookmarks')
        .send({ title: 'Test', url: 'https://test.com' });
      
      assert.strictEqual(response.status, 401);
    });
    
    test('should reject invalid URL', async () => {
      const response = await request(baseUrl)
        .post('/bookmarks')
        .set('Authorization', `Bearer ${authToken}`)
        .send({ title: 'Test', url: 'not-a-url' });
      
      assert.strictEqual(response.status, 400);
    });
    
    test('should reject duplicate URL', async () => {
      const bookmark = {
        title: 'Node.js',
        url: 'https://nodejs.org'
      };
      
      // Try to create duplicate
      const response = await request(baseUrl)
        .post('/bookmarks')
        .set('Authorization', `Bearer ${authToken}`)
        .send(bookmark);
      
      assert.strictEqual(response.status, 409);
    });
  });
  
  describe('GET /bookmarks (List)', () => {
    test('should list user bookmarks', async () => {
      const response = await request(baseUrl)
        .get('/bookmarks')
        .set('Authorization', `Bearer ${authToken}`)
        .expect('Content-Type', /json/);
      
      assert.strictEqual(response.status, 200);
      assert(Array.isArray(response.body.bookmarks));
    });
    
    test('should support pagination', async () => {
      const response = await request(baseUrl)
        .get('/bookmarks?limit=10&offset=0')
        .set('Authorization', `Bearer ${authToken}`);
      
      assert(response.body.pagination);
      assert.strictEqual(response.body.pagination.limit, 10);
    });
    
    test('should reject without auth', async () => {
      const response = await request(baseUrl)
        .get('/bookmarks');
      
      assert.strictEqual(response.status, 401);
    });
  });
  
  describe('GET /bookmarks/:id (Get One)', () => {
    let bookmarkId;
    
    test('should get single bookmark', async () => {
      // First create a bookmark
      const createResponse = await request(baseUrl)
        .post('/bookmarks')
        .set('Authorization', `Bearer ${authToken}`)
        .send({
          title: 'Get Test',
          url: 'https://gettest.com'
        });
      
      bookmarkId = createResponse.body.id;
      
      // Then get it
      const response = await request(baseUrl)
        .get(`/bookmarks/${bookmarkId}`)
        .set('Authorization', `Bearer ${authToken}`);
      
      assert.strictEqual(response.status, 200);
      assert.strictEqual(response.body.title, 'Get Test');
    });
    
    test('should return 404 for non-existent bookmark', async () => {
      const response = await request(baseUrl)
        .get('/bookmarks/99999')
        .set('Authorization', `Bearer ${authToken}`);
      
      assert.strictEqual(response.status, 404);
    });
  });
  
  describe('PATCH /bookmarks/:id (Update)', () => {
    let bookmarkId;
    
    test('should update bookmark', async () => {
      // Create bookmark
      const createResponse = await request(baseUrl)
        .post('/bookmarks')
        .set('Authorization', `Bearer ${authToken}`)
        .send({
          title: 'Original Title',
          url: 'https://updatetest.com'
        });
      
      bookmarkId = createResponse.body.id;
      
      // Update it
      const response = await request(baseUrl)
        .patch(`/bookmarks/${bookmarkId}`)
        .set('Authorization', `Bearer ${authToken}`)
        .send({ title: 'Updated Title' });
      
      assert.strictEqual(response.status, 200);
      assert.strictEqual(response.body.title, 'Updated Title');
    });
  });
  
  describe('DELETE /bookmarks/:id (Delete)', () => {
    test('should delete bookmark', async () => {
      // Create bookmark
      const createResponse = await request(baseUrl)
        .post('/bookmarks')
        .set('Authorization', `Bearer ${authToken}`)
        .send({
          title: 'To Delete',
          url: 'https://deletetest.com'
        });
      
      const bookmarkId = createResponse.body.id;
      
      // Delete it
      const response = await request(baseUrl)
        .delete(`/bookmarks/${bookmarkId}`)
        .set('Authorization', `Bearer ${authToken}`);
      
      assert.strictEqual(response.status, 204);
      
      // Verify it's gone
      const getResponse = await request(baseUrl)
        .get(`/bookmarks/${bookmarkId}`)
        .set('Authorization', `Bearer ${authToken}`);
      
      assert.strictEqual(getResponse.status, 404);
    });
  });
});
```

## How It Connects

This connects to:
- [09-testing/integration-testing/01-supertest.md](../../../09-testing/integration-testing/01-supertest.md) - Testing Express

## Common Mistakes

- Not using auth token in requests
- Not cleaning up test data
- Testing implementation details instead of behavior

## Try It Yourself

### Exercise 1: Run Tests
Run bookmark tests and fix any issues.

### Exercise 2: Add Tag Tests
Add tests for tag filtering.

### Exercise 3: Add Export Tests
Add tests for CSV/JSON export.

## Next Steps

Continue to [../../07-deployment/01-dockerfile.md](../../07-deployment/01-dockerfile.md) for deployment.
