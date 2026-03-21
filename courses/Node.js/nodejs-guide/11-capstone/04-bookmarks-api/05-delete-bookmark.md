# Delete Bookmark Endpoint

## What You'll Build In This File

The DELETE /bookmarks/:id endpoint with ownership check and proper HTTP status codes.

## Add Delete Route

Add to `src/routes/bookmarks.js`:

```javascript
// src/routes/bookmarks.js - Add after update route

/**
 * DELETE /bookmarks/:id
 * Delete a bookmark
 * 
 * Response (204): No content - successful deletion
 * Response (404): Bookmark not found or not owned by user
 */
router.delete('/:id', async (req, res) => {
  try {
    // 1. Parse bookmark ID
    const bookmarkId = parseInt(req.params.id);
    
    if (isNaN(bookmarkId)) {
      return res.status(400).json({
        error: 'Bad Request',
        message: 'Invalid bookmark ID'
      });
    }
    
    // 2. Check if bookmark exists AND belongs to user
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
    
    // 3. Delete the bookmark
    // CASCADE will automatically delete bookmark_tags associations
    await query(
      'DELETE FROM bookmarks WHERE id = $1 AND user_id = $2',
      [bookmarkId, req.user.userId]
    );
    
    // 4. Return 204 No Content (successful deletion with no response body)
    res.status(204).send();
    
  } catch (error) {
    console.error('Delete bookmark error:', error);
    res.status(500).json({
      error: 'Internal Server Error',
      message: 'Failed to delete bookmark'
    });
  }
});

export default router;
```

## HTTP 204 No Content

When deleting a resource successfully, we return **204 No Content** instead of 200. This tells the client the operation succeeded but there's no data to return.

The `.send()` call with no arguments sends an empty response body.

## How It Connects

This completes the CRUD operations covered in:
- [05-express-framework/getting-started/02-routing.md](../../../05-express-framework/getting-started/02-routing.md)

## Common Mistakes

- Not checking ownership (IDOR vulnerability)
- Returning 200 instead of 204 for delete
- Not handling errors

## Try It Yourself

### Exercise 1: Delete a Bookmark
Create a bookmark then delete it.

### Exercise 2: Try to Delete Others' Bookmarks
Try to delete another user's bookmark (should get 404).

### Exercise 3: Verify Deletion
Try to get the deleted bookmark (should return 404).

## Complete Bookmarks Route Export

Make sure your `src/routes/bookmarks.js` ends with:

```javascript
// ... all routes above ...

export default router;
```

Now you have a complete REST API for bookmarks!

## Next Steps

Continue to [../../05-export/01-csv-export.md](../../05-export/01-csv-export.md) to add export functionality.
