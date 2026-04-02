# LangChain Agents

## What You'll Learn

- How to create AI agents with LangChain
- How to define tools for agents
- How the agent loop works
- How to handle agent errors

## Creating an Agent

```ts
// agent.ts — LangChain agent with tools

import { model } from './llm.js';
import { AgentExecutor, createToolCallingAgent } from 'langchain/agents';
import { ChatPromptTemplate } from '@langchain/core/prompts';
import { DynamicStructuredTool } from '@langchain/core/tools';
import { z } from 'zod';

// Define tools
const weatherTool = new DynamicStructuredTool({
  name: 'getWeather',
  description: 'Get the current weather for a city',
  schema: z.object({
    city: z.string().describe('City name'),
  }),
  func: async ({ city }) => {
    // In production, call a weather API
    return JSON.stringify({ city, temp: 22, conditions: 'sunny' });
  },
});

const searchTool = new DynamicStructuredTool({
  name: 'searchDocs',
  description: 'Search documentation',
  schema: z.object({
    query: z.string().describe('Search query'),
  }),
  func: async ({ query }) => {
    // In production, search vector database
    return JSON.stringify({
      results: [
        { title: 'Node.js Event Loop', content: 'The event loop...' },
      ],
    });
  },
});

// Create prompt
const prompt = ChatPromptTemplate.fromMessages([
  ['system', 'You are a helpful assistant with access to tools.'],
  ['human', '{input}'],
  ['placeholder', '{agent_scratchpad}'],
]);

// Create agent
const agent = createToolCallingAgent({
  llm: model,
  tools: [weatherTool, searchTool],
  prompt,
});

// Create executor
const executor = new AgentExecutor({
  agent,
  tools: [weatherTool, searchTool],
  verbose: true,  // Log agent steps
  maxIterations: 5,
});

// Run agent
const result = await executor.invoke({
  input: 'What is the weather in Tokyo and search for Node.js docs?',
});

console.log(result.output);
```

## Next Steps

For memory, continue to [Memory](./04-memory.md).
