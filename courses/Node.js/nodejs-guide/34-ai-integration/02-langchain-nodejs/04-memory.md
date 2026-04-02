# LangChain Memory

## What You'll Learn

- How conversation memory works in LangChain
- How to use different memory types
- How to persist conversation history
- How to limit memory size

## Memory Types

```ts
// memory.ts

import { BufferMemory } from 'langchain/memory';
import { ConversationSummaryMemory } from 'langchain/memory';

// Buffer memory — stores full conversation
const bufferMemory = new BufferMemory({
  memoryKey: 'chat_history',
  returnMessages: true,
});

// Summary memory — summarizes old messages to save tokens
const summaryMemory = new ConversationSummaryMemory({
  llm: model,
  memoryKey: 'chat_history',
});

// Window memory — keeps last N messages
import { BufferWindowMemory } from 'langchain/memory';
const windowMemory = new BufferWindowMemory({
  k: 10,  // Keep last 10 messages
  memoryKey: 'chat_history',
});
```

## Using Memory with Chains

```ts
// chat-with-memory.ts

import { ConversationChain } from 'langchain/chains';
import { BufferMemory } from 'langchain/memory';
import { model } from './llm.js';

const memory = new BufferMemory();

const chain = new ConversationChain({
  llm: model,
  memory,
});

// First message
await chain.call({ input: 'My name is Alice' });
// Response: "Nice to meet you, Alice!"

// Second message — memory is maintained
await chain.call({ input: 'What is my name?' });
// Response: "Your name is Alice."

// View memory
console.log(await memory.loadMemoryVariables({}));
```

## Persistent Memory

```ts
// persistent-memory.ts — Store memory in Redis

import { RedisChatMessageHistory } from '@langchain/redis';
import { BufferMemory } from 'langchain/memory';

function createMemory(sessionId: string) {
  return new BufferMemory({
    chatHistory: new RedisChatMessageHistory({
      sessionId,
      sessionTTL: 3600,  // Expire after 1 hour
      url: 'redis://localhost:6379',
    }),
    memoryKey: 'chat_history',
    returnMessages: true,
  });
}
```

## Next Steps

For document loaders, continue to [Document Loaders](./05-document-loaders.md).
