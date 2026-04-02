# ChatGPT Integration

## What You'll Learn

- How to build a chat application with OpenAI
- How to manage conversation history
- How to use system prompts effectively
- How to implement streaming chat

## Chat Completion with History

```ts
// chat.ts — Multi-turn conversation

import openai from './openai.js';

interface Message {
  role: 'system' | 'user' | 'assistant';
  content: string;
}

class ChatSession {
  private messages: Message[] = [];
  private model: string;

  constructor(systemPrompt: string, model = 'gpt-4o') {
    this.model = model;
    this.messages = [
      { role: 'system', content: systemPrompt },
    ];
  }

  async sendMessage(userMessage: string): Promise<string> {
    this.messages.push({ role: 'user', content: userMessage });

    const completion = await openai.chat.completions.create({
      model: this.model,
      messages: this.messages,
      max_tokens: 1000,
      temperature: 0.7,
    });

    const assistantMessage = completion.choices[0].message.content || '';
    this.messages.push({ role: 'assistant', content: assistantMessage });

    return assistantMessage;
  }

  getHistory(): Message[] {
    return [...this.messages];
  }

  clear(): void {
    const systemMessage = this.messages[0];
    this.messages = [systemMessage];
  }
}

// Usage
const chat = new ChatSession('You are a Node.js expert. Be concise.');

const reply1 = await chat.sendMessage('What is the event loop?');
console.log(reply1);

const reply2 = await chat.sendMessage('How does it handle I/O?');
console.log(reply2);

const reply3 = await chat.sendMessage('Summarize what we discussed');
console.log(reply3);
```

## Streaming Chat

```ts
// streaming-chat.ts

import openai from './openai.js';

async function* chatStream(messages: Array<{ role: string; content: string }>) {
  const stream = await openai.chat.completions.create({
    model: 'gpt-4o',
    messages,
    stream: true,
  });

  for await (const chunk of stream) {
    const content = chunk.choices[0]?.delta?.content;
    if (content) yield content;
  }
}

// Usage
const messages = [
  { role: 'system', content: 'You are helpful.' },
  { role: 'user', content: 'Explain promises in Node.js' },
];

process.stdout.write('Assistant: ');
for await (const chunk of chatStream(messages)) {
  process.stdout.write(chunk);
}
console.log();
```

## Token Management

```ts
// token-management.ts — Track and limit token usage

import openai from './openai.js';

class TokenAwareChat {
  private messages: Array<{ role: string; content: string }> = [];
  private maxTokens: number;
  private model: string;

  constructor(model = 'gpt-4o', maxTokens = 4096) {
    this.model = model;
    this.maxTokens = maxTokens;
  }

  async chat(userMessage: string): Promise<string> {
    this.messages.push({ role: 'user', content: userMessage });

    // Trim history if approaching token limit
    while (this.estimateTokens() > this.maxTokens - 1000) {
      // Remove oldest non-system message
      if (this.messages.length > 2) {
        this.messages.splice(1, 1);  // Keep system message at index 0
      } else {
        break;
      }
    }

    const completion = await openai.chat.completions.create({
      model: this.model,
      messages: this.messages,
      max_tokens: 1000,
    });

    const reply = completion.choices[0].message.content || '';
    this.messages.push({ role: 'assistant', content: reply });

    // Log usage
    console.log(`Tokens: ${completion.usage?.total_tokens}`);

    return reply;
  }

  private estimateTokens(): number {
    // Rough estimate: 1 token ≈ 4 characters
    return this.messages.reduce((sum, m) => sum + Math.ceil(m.content.length / 4), 0);
  }
}
```

## System Prompts

```ts
// Good system prompt examples

const codeReviewer = `
You are a senior code reviewer. Review code for:
1. Security vulnerabilities
2. Performance issues
3. Code style violations
4. Logic errors
Respond in markdown with specific line references.
`;

const apiAssistant = `
You are an API assistant. You have access to these endpoints:
- GET /users — list users
- POST /users — create user
- GET /products — list products
Help users interact with the API. Ask for clarification when needed.
`;
```

## Next Steps

For embeddings, continue to [Embeddings API](./03-embeddings-api.md).
