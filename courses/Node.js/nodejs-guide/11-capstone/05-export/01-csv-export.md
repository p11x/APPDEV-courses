# CSV Export Endpoint

## What You'll Build In This File

The GET /bookmarks/export endpoint using Transform streams to convert database results to CSV format.

## Complete Export Routes

Add to `src/routes/bookmarks.js` before the export statement:

```javascript
// src/routes/bookmarks.js - Add export routes

import { Transform } from 'stream';

/**
 * GET /bookmarks/export
 * Export bookmarks as CSV
 * 
 * Response: text/csv
 * id,title,url,description,tags,created_at
 * 1,Node.js,https://nodejs.org,JavaScript runtime,javascript;backend,2024-01-01
 */
router.get('/export', async (req, res) => {
  try {
    // Set response headers for CSV download
    res.setHeader('Content-Type', 'text/csv');
    res.setHeader('Content-Disposition', 'attachment; filename="bookmarks.csv"');
    
    // Create a transform stream to convert objects to CSV
    const jsonToCsv = new Transform({
      objectMode: true,
      transform(chunk, encoding, callback) {
        // Convert each bookmark to CSV row
        const row = [
          chunk.id,
          // Escape quotes in fields
          `"${(chunk.title || '').replace(/"/g, '""')}"`,
          `"${(chunk.url || '').replace(/"/g, '""')}"`,
          `"${(chunk.description || '').replace(/"/g, '""')}"`,
          `"${(chunk.tags || []).map(t => t.name).join(';')}"`,
          chunk.created_at
        ].join(',');
        
        callback(null, row + '\n');
      }
    });
    
    // Write CSV header first
    res.write('id,title,url,description,tags,created_at\n');
    
    // Stream all bookmarks for this user
    // Using a cursor would be more efficient for large datasets
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
    
    // Pipe each row through the transform
    for (const row of result.rows) {
      jsonToCsv.write(row);
    }
    
    // End the stream
    jsonToCsv.end();
    
    // Pipe to response
    jsonToCsv.pipe(res);
    
    // Handle stream errors
    jsonToCsv.on('error', (error) => {
      console.error('CSV export error:', error);
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
});

export default router;
```

## How Transform Streams Work

```
Database → jsonToCsv (Transform) → Response
           ↑ transforms each object
           ↑ into CSV row format
```

The Transform stream:
1. Receives objects in objectMode
2. Transforms each to a CSV row string
3. Outputs strings that get piped to the response

This is memory-efficient because we don't need to hold all data in memory.

## How It Connects

This connects to concepts from:
- [07-streams-and-buffers/streams/04-transform-streams.md](../../../07-streams-and-buffers/streams/04-transform-streams.md) - Transform streams
- [07-streams-and-buffers/streams/03-pipe.md](../../../07-streams-and-buffers/streams/03-pipe.md) - Piping streams

## Common Mistakes

- Not setting Content-Type header
- Forgetting to handle stream errors
- Not escaping CSV values (can break parsing)

## Try It Yourself

### Exercise 1: Export CSV
Create bookmarks and export as CSV.

### Exercise 2: Add More Fields
Add more fields to the CSV export.

### Exercise 3: Stream Large Data
Test with a large number of bookmarks.

## Next Steps

Continue to [02-json-export.md](./02-json-export.md) for JSON export.
