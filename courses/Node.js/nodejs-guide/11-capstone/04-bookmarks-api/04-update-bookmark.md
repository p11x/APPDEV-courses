# Update Bookmark Endpoint

## What You'll Build In This File

The PATCH /bookmarks/:id endpoint for partial updates with validation and ownership check.

## Add Update Route

Add to `src/routes/bookmarks.js`:

```javascript
// src/routes/bookmarks.js - Add after get route

/**
 * PATCH /bookmarks/:id
 * Update a bookmark (partial update)
 * 
 * Request body (partial - only include fields to update):
 * {
 *   "title": "Updated Title",
 *   "description": "Updated description"
 * }
 * 
 * Response (200):
 * {
 *   "id": 1,
 *   "title": "Updated Title",
 *   "url": "https://nodejs.org",
 *   "description": "Updated description",
 *   "tags": [...],
 *   "updatedAt": "2024-01-02T00:00:00.000Z"
 * }
 */
router.patch('/:id', async (req, res) => {
  try {
    // 1. Parse bookmark ID
    const bookmarkId = parseInt(req.params.id);
    
    if (isNaN(bookmarkId)) {
      return res.status(400).json({
        error: 'Bad Request',
        message: 'Invalid bookmark ID'
      });
    }
    
    // 2. Validate request body (partial validation)
    // Using safeParse allows partial objects
    const parsed = updateBookmarkSchema.safeParse(req.body);
    
    if (!parsed.success) {
      return res.status(400).json({
        error: 'Validation Error',
        issues: parsed.error.issues
      });
    }
    
    // Check if there's anything to update
    const updates = parsed.data;
    if (Object.keys(updates).length === 0) {
      return res.status(400).json({
        error: 'Bad Request',
        message: 'No fields to update'
      });
    }
    
    // 3. Check if bookmark exists AND belongs to user
    const checkResult = await query(
      'SELECT id FROM bookmarks WHERE id = $1 AND user_id = $2',
      [bookmarkId, req.user.userId]
    );
    
    if (checkResult.rows.length === 0) {
      return res.status(404).json({
        error: 'Not Found',
        message: 'Bookmark not found'
      });
    }
    
    // 4. Build dynamic UPDATE query
    const setClauses = [];
    const params = [];
    let paramIndex = 1;
    
    // Add each field to update
    if (updates.title !== undefined) {
      setClauses.push(`title = $${paramIndex++}`);
      params.push(updates.title);
    }
    if (updates.url !== undefined) {
      setClauses.push(`url = $${paramIndex++}`);
      params.push(updates.url);
    }
    if (updates.description !== undefined) {
      setClauses.push(`description = $${paramIndex++}`);
      params.push(updates.description);
    }
    
    // Add updated_at timestamp
    setClauses.push(`updated_at = CURRENT_TIMESTAMP`);
    
    // Add bookmark ID and user ID for WHERE clause
    params.push(bookmarkId, req.user.userId);
    
    // 5. Execute update query
    const result = await query(
      `UPDATE bookmarks 
       SET ${setClauses.join(', ')}
       WHERE id = $${paramIndex++} AND user_id = $${paramIndex}
       RETURNING id, title, url, description, created_at, updated_at`,
      params
    );
    
    const bookmark = result.rows[0];
    
    // 6. Get updated tags if provided
    let tags = [];
    if (updates.tags) {
      // Delete existing tag associations
      await query(
        'DELETE FROM bookmark_tags WHERE bookmark_id = $1',
        [bookmark.id]
      );
      
      // Add new tags (similar to create logic)
      for (const tagName of updates.tags) {
        const tagResult = await query(
          'INSERT INTO tags (user_id, name) VALUES ($1, $2) ON CONFLICT DO NOTHING RETURNING id',
          [req.user.userId, tagName]
        );
        
        let tagId;
        if (tagResult.rows.length > 0) {
          tagId = tagResult.rows[0].id;
        } else {
          const existingTag = await query(
            'SELECT id FROM tags WHERE user_id = $1 AND name = $2',
            [req.user.userId, tagName]
          );
          tagId = existingTag.rows[0].id;
        }
        
        await query(
          'INSERT INTO bookmark_tags (bookmark_id, tag_id) VALUES ($1, $2) ON CONFLICT DO NOTHING',
          [bookmark.id, tagId]
        );
        
        tags.push({ id: tagId, name: tagName });
      }
    }
    
    // 7. Return updated bookmark
    // If tags weren't updated, fetch current tags
    if (!updates.tags) {
      const tagsResult = await query(
        `SELECT t.id, t.name 
         FROM tags t 
         JOIN bookmark_tags bt ON t.id = bt.tag_id 
         WHERE bt.bookmark_id = $1`,
        [bookmark.id]
      );
      tags = tagsResult.rows;
    }
    
    res.json({
      id: bookmark.id,
      title: bookmark.title,
      url: bookmark.url,
      description: bookmark.description,
      tags,
      createdAt: bookmark.created_at,
      updatedAt: bookmark.updated_at
    });
    
  } catch (error) {
    // Handle unique constraint violation
    if (error.code === '23505') {
      return res.status(409).json({
        error: 'Conflict',
        message: 'You already have a bookmark with this URL'
      });
    }
    
    console.error('Update bookmark error:', error);
    res.status(500).json({
      error: 'Internal Server Error',
      message: 'Failed to update bookmark'
    });
  }
});
```

## How It Connects

This connects to:
- Partial validation with Zod (see [04-npm-and-packages/useful-packages/03-zod.md](../../../04-npm-and-packages/useful-packages/03-zod.md))

## Try It Yourself

### Exercise 1: Update Title
Update just the title of a bookmark.

### Exercise 2: Update Tags
Replace tags on an existing bookmark.

### Exercise 3: Empty Update
Try sending an empty body - should get error.

## Next Steps

Continue to [05-delete-bookmark.md](./05-delete-bookmark.md) to delete bookmarks.
