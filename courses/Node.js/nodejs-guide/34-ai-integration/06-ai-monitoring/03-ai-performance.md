# AI Performance

## What You'll Learn

- How to optimize AI API latency
- How to implement response caching
- How to reduce token usage
- How to handle rate limits

## Latency Optimization

```ts
// Use streaming for perceived performance
const stream = await openai.chat.completions.create({
  model: 'gpt-4o',
  messages,
  stream: true,  // User sees output immediately
});

// Use smaller models for simple tasks
const simpleTasks = {
  classify: 'gpt-4o-mini',    // Fast, cheap
  complex: 'gpt-4o',          // Slower, accurate
  embedding: 'text-embedding-3-small',
};

// Parallel requests
const [summary, questions, tags] = await Promise.all([
  summarize(text),
  generateQuestions(text),
  extractTags(text),
]);
```

## Response Caching

```ts
// Cache AI responses to avoid duplicate API calls

import crypto from 'node:crypto';
import Redis from 'ioredis';

const redis = new Redis();

async function cachedCompletion(messages: Array<{ role: string; content: string }>) {
  const hash = crypto
    .createHash('sha256')
    .update(JSON.stringify(messages))
    .digest('hex');

  const cached = await redis.get(`ai:cache:${hash}`);
  if (cached) return JSON.parse(cached);

  const completion = await openai.chat.completions.create({
    model: 'gpt-4o',
    messages,
  });

  await redis.set(
    `ai:cache:${hash}`,
    JSON.stringify(completion.choices[0].message),
    'EX',
    3600  // Cache for 1 hour
  );

  return completion.choices[0].message;
}
```

## Rate Limit Handling

```ts
// Retry with exponential backoff

async function withRetry<T>(fn: () => Promise<T>, maxRetries = 3): Promise<T> {
  for (let attempt = 0; attempt < maxRetries; attempt++) {
    try {
      return await fn();
    } catch (err) {
      if (err.status === 429) {
        const retryAfter = parseInt(err.headers?.['retry-after'] || '1');
        const delay = Math.pow(2, attempt) * 1000 + retryAfter * 1000;
        console.log(`Rate limited, retrying in ${delay}ms...`);
        await new Promise((r) => setTimeout(r, delay));
      } else {
        throw err;
      }
    }
  }
  throw new Error('Max retries exceeded');
}
```

## Next Steps

For debugging, continue to [AI Debugging](./04-ai-debugging.md).
