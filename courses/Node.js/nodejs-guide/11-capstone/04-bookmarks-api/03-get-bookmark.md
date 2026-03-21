# Get Single Bookmark Endpoint

## What You'll Build In This File

The GET /bookmarks/:id endpoint with ownership verification and 404 handling.

## Add Get One Route

Add to `src/routes/bookmarks.js`:

```javascript
// src/routes/bookmarks.js - Add after list route

/**
 * GET /bookmarks/:id
 * Get a single bookmark by ID
 * 
 * Response (200):
 * {
 *   "id": 1,
 *   "title": "Node.js",
 *   "url": "https://nodejs.org",
 *   "description": "JavaScript runtime",
 *   "tags": [...],
 *   "createdAt": "2024-01-01T00:00:00.000Z",
 *   "updatedAt": "2024-01-01T00:00:00.000Z"
 * }
 * 
 * Response (404): Bookmark not found or not owned by user
 */
router.get('/:id', async (req, res) => {
  try {
    // 1. Parse bookmark ID from URL parameter
    const bookmarkId = parseInt(req.params.id);
    
    // Validate ID is a number
    if (isNaN(bookmarkId)) {
      return res.status(400).json({
        error: 'Bad Request',
        message: 'Invalid bookmark ID'
      });
    }
    
    // 2. Query for bookmark with tags
    // CRITICAL: Always filter by user_id to prevent accessing other users' bookmarks
    const result = await query(
      `SELECT 
        b.id, b.title, b.url, b.description, b.created_at, b.updated_at,
        COALESCE(
          json_agg(
            json_build_object('id', t.id, 'name', t.name)
          ) FILTER (WHERE t.id IS NOT NULL),
          '[]'
        ) as tags
      FROM bookmarks b
      LEFT JOIN bookmark_tags bt ON b.id = bt.bookmark_id
      LEFT JOIN tags t ON bt.tag_id = t.id
      WHERE b.id = $1 AND b.user_id = $2
      GROUP BY b.id`,
      [bookmarkId, req.user.userId]
    );
    
    // 3. Check if bookmark exists
    if (result.rows.length === 0) {
      return res.status(404).json({
        error: 'Not Found',
        message: 'Bookmark not found'
      });
    }
    
    // 4. Return the bookmark
    const bookmark = result.rows[0];
    res.json({
      id: bookmark.id,
      title: bookmark.title,
      url: bookmark.url,
      description: bookmark.description,
      tags: bookmark.tags,
      createdAt: bookmark.created_at,
      updatedAt: bookmark.updated_at
    });
    
  } catch (error) {
    console.error('Get bookmark error:', error);
    res.status(500).json({
      error: 'Internal Server Error',
      message: 'Failed to get bookmark'
    });
  }
});
```

## Security: Ownership Check

The critical line is:
```sql
WHERE b.id = $1 AND b.user_id = $2
```

This ensures users can ONLY access their own bookmarks. Without this check, any user could access any bookmark by guessing IDs - a serious security vulnerability!

## How It Connects

This connects to:
- [05-express-framework/getting-started/02-routing.md](../../../05-express-framework/getting-started/02-routing.md) - Route parameters

## Common Mistakes

- Not checking ownership (IDOR vulnerability)
- Not validating the ID parameter
- Returning wrong error message (don't reveal if bookmark exists)

## Try It Yourself

### Exercise 1: Get Own Bookmark
Create a bookmark and retrieve it by ID.

### Exercise 2: Try Other User's Bookmark
Try to access another user's bookmark (should return 404).

### Exercise 3: Invalid ID
Try accessing /bookmarks/invalid.

## Next Steps

Continue to [04-update-bookmark.md](./04-update-bookmark.md) to update bookmarks.
