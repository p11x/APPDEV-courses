# Axios vs Fetch

## What You'll Learn

- Detailed comparison of Axios and Fetch
- When to use each
- Migration between them

## Comparison

```js
// Fetch
const res = await fetch('/api/users', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ name: 'Alice' }),
});
const data = await res.json();

// Axios
import axios from 'axios';
const { data } = await axios.post('/api/users', { name: 'Alice' });
// Auto JSON, auto error on non-2xx
```

## Recommendation

| Use Case | Recommendation |
|----------|---------------|
| Simple API calls | fetch (no dependency) |
| Complex interceptors | Axios |
| File upload with progress | Axios |
| Browser-only project | fetch |
| SSR/Node.js project | Either works |

## Next Steps

This concludes the frontend integration chapter. Return to the [full guide index](../index.html) to continue learning.
