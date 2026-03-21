# JSON Export Endpoint

## What You'll Build In This File

The GET /bookmarks/export?format=json endpoint using Readable streams.

## Add JSON Export

Add this to the export routes section in `src/routes/bookmarks.js`:

```javascript
// src/routes/bookmarks.js - Add after CSV export

import { Readable } from 'stream';

/**
 * GET /bookmarks/export?format=json
 * Export bookmarks as JSON (newline-delimited JSON)
 * 
 * Response: application/x-ndjson (newline-delimited JSON)
 * Each line is a valid JSON object
 * {"id":1,"title":"Node.js","url":"https://nodejs.org"...}
 * {"id":2,"title":"Express","url":"https://expressjs.com"...}
 */
router.get('/export', async (req, res) => {
  // Get format from query string
  const format = req.query.format || 'csv';
  
  if (format === 'json') {
    return exportJson(req, res);
  }
  
  // If not JSON, fall back to CSV (from previous file)
  // ... (CSV export code here)
});

async function exportJson(req, res) {
  try {
    // Set response headers for JSON download
    res.setHeader('Content-Type', 'application/x-ndjson');
    res.setHeader('Content-Disposition', 'attachment; filename="bookmarks.json"');
    
    // Get all bookmarks
    const result = await query(
      `SELECT b.id, b.title, b.url, b.description, b.created_at,
              COALESCE(
                json_agg(json_build_object('id', t.id, 'name', t.name)) 
                FILTER (WHERE t.id IS NOT NULL),
                '[]'
              ) as tags
       FROM bookmarks b
       LEFT JOIN bookmark_tags bt ON b.id = bt.bookmark_id
       LEFT JOIN tags t ON bt.tag_id = t.id
       WHERE b.user_id = $1
       GROUP BY b.id
       ORDER BY b.id`,
      [req.user.userId]
    );
    
    // Create a readable stream from the data
    const stream = Readable.from(
      result.rows.map(bookmark => {
        // Format each bookmark as JSON
        return JSON.stringify({
          id: bookmark.id,
          title: bookmark.title,
          url: bookmark.url,
          description: bookmark.description,
          tags: bookmark.tags,
          createdAt: bookmark.created_at
        }) + '\n';
      })
    );
    
    // Pipe to response
    stream.pipe(res);
    
    // Handle errors
    stream.on('error', (error) => {
      console.error('JSON export error:', error);
      if (!res.headersSent) {
        res.status(500).json({ error: 'Export failed' });
      }
    });
    
  } catch (error) {
    console.error('Export error:', error);
    if (!res.headersSent) {
      res.status(500).json({
        error: 'Internal Server Error',
        message: 'Failed to export bookmarks'
      });
    }
  }
}
```

## Newline-Delimited JSON (NDJSON)

NDJSON is a format where each line is a valid JSON object:
```json
{"id":1,"title":"Node.js"}
{"id":2,"title":"Express"}
```

Benefits:
- Stream-friendly (no waiting for full array)
- Easy to parse (split by newline, parse each)
- Common format for data exports

## How It Connects

This connects to:
- [07-streams-and-buffers/streams/01-readable-streams.md](../../../07-streams-and-buffers/streams/01-readable-streams.md) - Readable streams

## Common Mistakes

- Not handling the format parameter
- Sending array instead of NDJSON for large exports
- Not handling stream errors

## Try It Yourself

### Exercise 1: Export JSON
Export bookmarks as JSON format.

### Exercise 2: Parse NDJSON
Write a script to parse the exported NDJSON.

### Exercise 3: Add Filter
Add tag filtering to exports.

## Next Steps

Continue to [../../06-testing/01-test-setup.md](../../06-testing/01-test-setup.md) to set up testing.
