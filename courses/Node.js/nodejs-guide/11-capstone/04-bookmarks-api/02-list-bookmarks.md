# List Bookmarks Endpoint

## What You'll Build In This File

The GET /bookmarks endpoint with pagination, tag filtering, and ordering.

## Add List Route to bookmarks.js

Add this route to `src/routes/bookmarks.js` after the create route:

```javascript
// src/routes/bookmarks.js - Add after POST route

/**
 * GET /bookmarks
 * List bookmarks with pagination and optional tag filter
 * 
 * Query params:
 * - limit: number of results (default 20, max 100)
 * - offset: for pagination (default 0)
 * - tag: filter by tag ID
 * 
 * Example: GET /bookmarks?limit=10&offset=0&tag=1
 * 
 * Response (200):
 * {
 *   "bookmarks": [...],
 *   "pagination": {
 *     "total": 50,
 *     "limit": 10,
 *     "offset": 0,
 *     "hasMore": true
 *   }
 * }
 */
router.get('/', async (req, res) => {
  try {
    // 1. Parse and validate query parameters
    const { limit, offset, tag } = req.query;
    
    // Validate with defaults
    const parsedLimit = Math.min(parseInt(limit) || 20, 100);
    const parsedOffset = parseInt(offset) || 0;
    const parsedTag = tag ? parseInt(tag) : null;
    
    // 2. Build parameterized query
    let whereClause = 'WHERE b.user_id = $1';
    const params = [req.user.userId];
    
    // Add tag filter if provided
    if (parsedTag) {
      whereClause += ` AND EXISTS (
        SELECT 1 FROM bookmark_tags bt 
        JOIN tags t ON bt.tag_id = t.id 
        WHERE bt.bookmark_id = b.id AND t.id = $${params.length + 1}
      )`;
      params.push(parsedTag);
    }
    
    // 3. Get total count for pagination
    const countResult = await query(
      `SELECT COUNT(*) as total 
       FROM bookmarks b 
       ${whereClause}`,
      params
    );
    const total = parseInt(countResult.rows[0].total);
    
    // 4. Get bookmarks with pagination
    params.push(parsedLimit, parsedOffset);
    
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
      ${whereClause}
      GROUP BY b.id
      ORDER BY b.created_at DESC
      LIMIT $${params.length - 1} OFFSET $${params.length}`,
      params
    );
    
    // 5. Format response
    const bookmarks = result.rows.map(b => ({
      id: b.id,
      title: b.title,
      url: b.url,
      description: b.description,
      tags: b.tags,
      createdAt: b.created_at,
      updatedAt: b.updated_at
    }));
    
    // 6. Return with pagination info
    res.json({
      bookmarks,
      pagination: {
        total,
        limit: parsedLimit,
        offset: parsedOffset,
        hasMore: parsedOffset + bookmarks.length < total
      }
    });
    
  } catch (error) {
    console.error('List bookmarks error:', error);
    res.status(500).json({
      error: 'Internal Server Error',
      message: 'Failed to list bookmarks'
    });
  }
});
```

## How It Works

1. **Parse query params** - Extract limit, offset, and tag from URL
2. **Build WHERE clause** - Dynamically add conditions based on filters
3. **Get total count** - For pagination metadata
4. **Fetch paginated results** - Use LIMIT/OFFSET for pagination
5. **Aggregate tags** - Use PostgreSQL's json_agg for tag arrays

## Query Parameter Handling

Express parses query strings automatically:
- `?limit=10` becomes `req.query.limit = "10"` (string!)
- Use `parseInt()` to convert to numbers
- Zod's `z.coerce.number()` can also do this automatically

## How It Connects

This connects to concepts from:
- [05-express-framework/getting-started/02-routing.md](../../../05-express-framework/getting-started/02-routing.md) - Route parameters and query strings

## Common Mistakes

- Not validating query parameters (can cause NaN)
- Not handling pagination metadata correctly
- Forgetting to filter by user (security issue!)
- Using string concatenation instead of parameterized queries

## Try It Yourself

### Exercise 1: Test Pagination
Create multiple bookmarks and test pagination.

### Exercise 2: Filter by Tag
Create tags and filter bookmarks by tag.

### Exercise 3: Add Sorting
Add ability to sort by title or date.

## Next Steps

Continue to [03-get-bookmark.md](./03-get-bookmark.md) to get a single bookmark.
