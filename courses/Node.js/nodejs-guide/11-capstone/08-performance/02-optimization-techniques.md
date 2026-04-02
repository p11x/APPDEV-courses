# Optimization Techniques

## What You'll Learn

- Database query optimization
- Response compression
- Pagination best practices

## Database Optimization

```js
// BEFORE — N+1 query
const bookmarks = await db.bookmark.findMany();
for (const b of bookmarks) {
  b.user = await db.user.findUnique({ where: { id: b.userId } });
}

// AFTER — single query with join
const bookmarks = await db.bookmark.findMany({
  include: { user: true },
});
```

## Pagination

```js
// Cursor-based pagination (recommended for large datasets)
app.get('/api/bookmarks', async (req, res) => {
  const { cursor, limit = 20 } = req.query;

  const bookmarks = await db.bookmark.findMany({
    take: parseInt(limit) + 1,  // Fetch one extra to check if there are more
    ...(cursor && { cursor: { id: cursor }, skip: 1 }),
    orderBy: { createdAt: 'desc' },
  });

  const hasMore = bookmarks.length > parseInt(limit);
  const items = hasMore ? bookmarks.slice(0, -1) : bookmarks;

  res.json({
    items,
    nextCursor: hasMore ? items[items.length - 1].id : null,
  });
});
```

## Next Steps

For load testing, continue to [Load Testing](./03-load-testing.md).
