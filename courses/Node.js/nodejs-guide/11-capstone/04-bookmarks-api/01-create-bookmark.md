# Create Bookmark Endpoint

## What You'll Build In This File

The POST /bookmarks endpoint to create new bookmarks with Zod validation and proper error handling.

## Bookmark Validation Schema

First, create `src/schemas/bookmarks.js`:

```javascript
// src/schemas/bookmarks.js - Validation schemas for bookmarks

import { z } from 'zod';

/**
 * Create bookmark schema
 * - title: required, 1-500 chars
 * - url: required, valid URL format
 * - description: optional, max 2000 chars
 * - tags: optional array of tag names
 */
export const createBookmarkSchema = z.object({
  title: z.string()
    .min(1, 'Title is required')
    .max(500, 'Title must be less than 500 characters'),
  url: z.string()
    .min(1, 'URL is required')
    .url('Invalid URL format'),
  description: z.string()
    .max(2000, 'Description must be less than 2000 characters')
    .optional(),
  tags: z.array(z.string())
    .max(10, 'Maximum 10 tags allowed')
    .optional()
});

/**
 * Update bookmark schema (partial update)
 * All fields are optional - only provided fields are updated
 */
export const updateBookmarkSchema = z.object({
  title: z.string()
    .min(1, 'Title cannot be empty')
    .max(500)
    .optional(),
  url: z.string()
    .url('Invalid URL format')
    .optional(),
  description: z.string()
    .max(2000)
    .optional(),
  tags: z.array(z.string())
    .max(10)
    .optional()
});

/**
 * Query params schema for listing bookmarks
 */
export const listBookmarksSchema = z.object({
  limit: z.coerce.number()
    .min(1, 'Minimum limit is 1')
    .max(100, 'Maximum limit is 100')
    .default(20),
  offset: z.coerce.number()
    .min(0, 'Offset must be positive')
    .default(0),
  tag: z.coerce.number()
    .optional()
});
```

## Complete Create Bookmark Route

Create `src/routes/bookmarks.js`:

```javascript
// src/routes/bookmarks.js - Bookmark CRUD routes
// All routes are protected - require valid JWT

import { Router } from 'express';
import { query } from '../db/index.js';
import { authenticate } from '../middleware/auth.js';
import { createBookmarkSchema, updateBookmarkSchema } from '../schemas/bookmarks.js';

const router = Router();

// Apply authentication middleware to all routes in this router
router.use(authenticate);

/**
 * POST /bookmarks
 * Create a new bookmark
 * 
 * Request (with JWT):
 * {
 *   "title": "Node.js",
 *   "url": "https://nodejs.org",
 *   "description": "JavaScript runtime",
 *   "tags": ["javascript", "backend"]
 * }
 * 
 * Response (201):
 * {
 *   "id": 1,
 *   "title": "Node.js",
 *   "url": "https://nodejs.org",
 *   "description": "JavaScript runtime",
 *   "tags": ["javascript", "backend"],
 *   "createdAt": "2024-01-01T00:00:00.000Z"
 * }
 */
router.post('/', async (req, res) => {
  try {
    // 1. Validate request body using Zod
    const parsed = createBookmarkSchema.parse(req.body);
    
    const { title, url, description, tags } = parsed;
    
    // 2. Insert the bookmark with the user's ID from JWT
    const result = await query(
      `INSERT INTO bookmarks (user_id, title, url, description)
       VALUES ($1, $2, $3, $4)
       RETURNING id, title, url, description, created_at`,
      [req.user.userId, title, url, description || null]
    );
    
    const bookmark = result.rows[0];
    
    // 3. Handle tags if provided
    let bookmarkTags = [];
    
    if (tags && tags.length > 0) {
      // Insert tags and create associations
      for (const tagName of tags) {
        // Insert tag if it doesn't exist, get its ID
        const tagResult = await query(
          `INSERT INTO tags (user_id, name)
           VALUES ($1, $2)
           ON CONFLICT (user_id, name) DO NOTHING
           RETURNING id`,
          [req.user.userId, tagName]
        );
        
        // Get tag ID (either newly inserted or existing)
        let tagId;
        if (tagResult.rows.length > 0) {
          tagId = tagResult.rows[0].id;
        } else {
          // Tag already existed, get its ID
          const existingTag = await query(
            'SELECT id FROM tags WHERE user_id = $1 AND name = $2',
            [req.user.userId, tagName]
          );
          tagId = existingTag.rows[0].id;
        }
        
        // Link bookmark to tag
        await query(
          'INSERT INTO bookmark_tags (bookmark_id, tag_id) VALUES ($1, $2) ON CONFLICT DO NOTHING',
          [bookmark.id, tagId]
        );
        
        bookmarkTags.push({ id: tagId, name: tagName });
      }
    }
    
    // 4. Return the created bookmark
    res.status(201).json({
      id: bookmark.id,
      title: bookmark.title,
      url: bookmark.url,
      description: bookmark.description,
      tags: bookmarkTags,
      createdAt: bookmark.created_at
    });
    
  } catch (error) {
    // Handle validation errors
    if (error.name === 'ZodError') {
      return res.status(400).json({
        error: 'Validation Error',
        issues: error.issues
      });
    }
    
    // Handle unique constraint violation (duplicate URL for user)
    if (error.code === '23505' && error.constraint === 'bookmarks_user_id_url_key') {
      return res.status(409).json({
        error: 'Conflict',
        message: 'You already have a bookmark with this URL'
      });
    }
    
    console.error('Create bookmark error:', error);
    res.status(500).json({
      error: 'Internal Server Error',
      message: 'Failed to create bookmark'
    });
  }
});

export default router;
```

## How It Connects

This route connects to concepts from:
- [05-express-framework/getting-started/02-routing.md](../../../05-express-framework/getting-started/02-routing.md) - Express routing
- [05-express-framework/request-response/01-req-object.md](../../../05-express-framework/request-response/01-req-object.md) - Request body
- [04-npm-and-packages/useful-packages/03-zod.md](../../../04-npm-and-packages/useful-packages/03-zod.md) - Zod validation

## Common Mistakes

- Not validating URL format
- Not checking ownership when creating
- Not handling duplicate URL errors
- Forgetting to use parameterized queries

## Try It Yourself

### Exercise 1: Create a Bookmark
Register, login, then create a bookmark with curl.

### Exercise 2: Test Validation
Try creating a bookmark with an invalid URL.

### Exercise 3: Add Tags
Create a bookmark with multiple tags.

## Next Steps

Continue to [02-list-bookmarks.md](./02-list-bookmarks.md) to list bookmarks.
