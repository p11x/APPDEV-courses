# LangChain Setup

## What You'll Learn

- What LangChain is and why it's useful
- How to set up LangChain with Node.js
- How LangChain's core concepts work
- How LangChain compares to direct API calls

## What Is LangChain?

LangChain is a framework for building applications powered by language models. It provides abstractions for chains (sequences of calls), agents (AI that uses tools), memory (conversation history), and document processing.

## Setup

```bash
npm install langchain @langchain/openai @langchain/core
```

```ts
// llm.ts — LangChain LLM setup

import { ChatOpenAI } from '@langchain/openai';

const model = new ChatOpenAI({
  modelName: 'gpt-4o',
  temperature: 0.7,
  openAIApiKey: process.env.OPENAI_API_KEY,
});

export { model };
```

## Basic Usage

```ts
// basic.ts

import { model } from './llm.js';
import { HumanMessage, SystemMessage } from '@langchain/core/messages';

// Simple call
const response = await model.invoke([
  new SystemMessage('You are a Node.js expert.'),
  new HumanMessage('What is the event loop?'),
]);

console.log(response.content);

// Streaming
const stream = await model.stream([
  new HumanMessage('Explain promises'),
]);

for await (const chunk of stream) {
  process.stdout.write(chunk.content as string);
}
```

## Comparison with Direct API

| Feature | Direct OpenAI | LangChain |
|---------|--------------|-----------|
| Setup | Simpler | More abstractions |
| Chains | Manual | Built-in |
| Agents | Manual | Built-in |
| Memory | Manual | Built-in |
| Document loading | Manual | Built-in |
| Overhead | None | Additional dependency |
| Best for | Simple calls | Complex AI workflows |

## Next Steps

For chains, continue to [Chains](./02-chains.md).
