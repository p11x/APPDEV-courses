# OpenAI Setup

## What You'll Learn

- How to install and configure the OpenAI SDK
- How to set up API key management
- How to make basic API calls
- How OpenAI's API pricing and rate limits work

## Setup

```bash
mkdir openai-demo && cd openai-demo
npm init -y
npm install openai
```

```ts
// openai.ts — OpenAI client setup

import OpenAI from 'openai';

// Create client with API key from environment variable
const openai = new OpenAI({
  apiKey: process.env.OPENAI_API_KEY,
  // Optional: custom base URL for Azure OpenAI or proxies
  // baseURL: 'https://api.openai.com/v1',
});

export default openai;
```

## Environment Variables

```bash
# .env
OPENAI_API_KEY=sk-...
OPENAI_ORG_ID=org-...       # Optional
OPENAI_PROJECT_ID=proj-...  # Optional
```

## Basic Completion

```ts
// basic.ts — Simple text completion

import openai from './openai.js';

async function main() {
  const completion = await openai.chat.completions.create({
    model: 'gpt-4o',
    messages: [
      { role: 'system', content: 'You are a helpful assistant.' },
      { role: 'user', content: 'Explain what Node.js is in one sentence.' },
    ],
    max_tokens: 100,
    temperature: 0.7,
  });

  console.log(completion.choices[0].message.content);
  // "Node.js is a JavaScript runtime built on Chrome's V8 engine..."
}

main();
```

## Response Structure

```ts
const completion = await openai.chat.completions.create({ ... });

// Full response
console.log(completion.id);                    // "chatcmpl-abc123"
console.log(completion.model);                 // "gpt-4o"
console.log(completion.choices[0].message);    // { role: 'assistant', content: '...' }
console.log(completion.usage.prompt_tokens);   // 25
console.log(completion.usage.completion_tokens);// 50
console.log(completion.usage.total_tokens);    // 75
```

## Streaming

```ts
// streaming.ts — Stream responses in real-time

async function streamCompletion(prompt: string) {
  const stream = await openai.chat.completions.create({
    model: 'gpt-4o',
    messages: [{ role: 'user', content: prompt }],
    stream: true,
  });

  for await (const chunk of stream) {
    const content = chunk.choices[0]?.delta?.content;
    if (content) {
      process.stdout.write(content);
    }
  }
  console.log();  // Newline at end
}

await streamCompletion('Write a haiku about Node.js');
```

## Error Handling

```ts
import OpenAI from 'openai';

async function safeCompletion(prompt: string) {
  try {
    const completion = await openai.chat.completions.create({
      model: 'gpt-4o',
      messages: [{ role: 'user', content: prompt }],
    });
    return completion.choices[0].message.content;
  } catch (err) {
    if (err instanceof OpenAI.RateLimitError) {
      console.error('Rate limited — retry after', err.headers?.['retry-after']);
      throw err;
    }
    if (err instanceof OpenAI.AuthenticationError) {
      console.error('Invalid API key');
      throw err;
    }
    if (err instanceof OpenAI.APIError) {
      console.error(`API error: ${err.status} ${err.message}`);
      throw err;
    }
    throw err;
  }
}
```

## Next Steps

For ChatGPT integration, continue to [ChatGPT Integration](./02-chatgpt-integration.md).
