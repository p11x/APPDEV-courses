# Cache Invalidation

## What You'll Learn

- How to invalidate edge caches
- Cache busting strategies
- How to purge Cloudflare cache

## Strategies

| Strategy | How | Use When |
|----------|-----|----------|
| TTL-based | Set expiration time | Content changes predictably |
| Purge API | Call purge endpoint | Content changes on deploy |
| Versioned URLs | `/app.v2.js` | Static assets |
| Cache tags | Tag and purge by tag | Related content changes |

## Cloudflare Purge

```bash
# Purge everything
curl -X POST "https://api.cloudflare.com/client/v4/zones/{zone_id}/purge_cache" \
  -H "Authorization: Bearer {api_token}" \
  --data '{"purge_everything":true}'

# Purge by URL
curl -X POST "https://api.cloudflare.com/client/v4/zones/{zone_id}/purge_cache" \
  -H "Authorization: Bearer {api_token}" \
  --data '{"files":["https://example.com/api/users"]}'
```

## Versioned URLs

```html
<!-- Cache forever, bust with version query -->
<link rel="stylesheet" href="/styles.css?v=abc123">
<script src="/app.js?v=abc123"></script>
```

## Next Steps

For edge security, continue to [Edge Security](./04-edge-security.md).
