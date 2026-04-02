# AI Debugging

## What You'll Learn

- How to debug AI API calls
- How to log prompts and responses
- How to test AI integrations
- How to handle common AI errors

## Logging Prompts and Responses

```ts
// ai-logger.ts

import pino from 'pino';

const logger = pino();

export async function loggedCompletion(
  messages: Array<{ role: string; content: string }>,
  options: Record<string, unknown> = {}
) {
  const start = performance.now();

  logger.info({
    type: 'ai_request',
    model: options.model || 'gpt-4o',
    messageCount: messages.length,
    lastMessage: messages[messages.length - 1]?.content?.slice(0, 100),
  }, 'AI request started');

  try {
    const completion = await openai.chat.completions.create({
      model: 'gpt-4o',
      messages,
      ...options,
    });

    const duration = performance.now() - start;

    logger.info({
      type: 'ai_response',
      model: completion.model,
      tokens: completion.usage,
      duration: Math.round(duration),
      response: completion.choices[0].message.content?.slice(0, 200),
    }, 'AI response received');

    return completion;
  } catch (err) {
    logger.error({
      type: 'ai_error',
      error: err.message,
      status: err.status,
    }, 'AI request failed');
    throw err;
  }
}
```

## Common Errors

| Error | Cause | Solution |
|-------|-------|----------|
| 401 | Invalid API key | Check `OPENAI_API_KEY` |
| 429 | Rate limited | Add retry with backoff |
| 500 | OpenAI server error | Retry with backoff |
| 503 | Service unavailable | Retry after delay |
| Context length | Too many tokens | Truncate or summarize history |
| Invalid JSON | Model returned bad JSON | Add "respond in JSON" to prompt |

## Testing AI Code

```ts
// ai.test.ts

import { describe, it, mock } from 'node:test';
import assert from 'node:assert';

describe('AI Integration', () => {
  it('should return formatted response', async () => {
    // Mock the OpenAI response
    const mockCreate = mock.fn(async () => ({
      choices: [{ message: { content: 'Mocked response' } }],
      usage: { prompt_tokens: 10, completion_tokens: 20, total_tokens: 30 },
    }));

    // Test your function
    const result = await generateResponse('test input', mockCreate);
    assert.strictEqual(result, 'Mocked response');
  });

  it('should handle API errors', async () => {
    const mockCreate = mock.fn(async () => {
      throw { status: 429, message: 'Rate limited' };
    });

    await assert.rejects(
      () => generateResponse('test', mockCreate),
      { message: 'Rate limited' }
    );
  });
});
```

## Next Steps

This concludes Chapter 34. Return to the [guide index](../../index.html).
