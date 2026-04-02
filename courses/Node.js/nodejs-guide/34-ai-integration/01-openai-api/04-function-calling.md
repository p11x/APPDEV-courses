# Function Calling

## What You'll Learn

- What function calling is in OpenAI
- How to define tools for the AI to use
- How to execute tool calls and return results
- How to build an AI agent with tools

## What Is Function Calling?

Function calling lets the AI model decide when to call your functions and with what arguments. The model does not execute the function — it returns a structured request, and your code executes it.

```
User: "What's the weather in Tokyo?"
  │
  ▼
AI decides to call: getWeather({ city: "Tokyo" })
  │
  ▼
Your code executes: getWeather({ city: "Tokyo" }) → { temp: 22, conditions: "sunny" }
  │
  ▼
AI responds: "The weather in Tokyo is 22°C and sunny."
```

## Defining Tools

```ts
// tools.ts — Define available functions

import openai from './openai.js';

// Tool definitions for OpenAI
const tools: OpenAI.Chat.Completions.ChatCompletionTool[] = [
  {
    type: 'function',
    function: {
      name: 'getWeather',
      description: 'Get the current weather for a city',
      parameters: {
        type: 'object',
        properties: {
          city: {
            type: 'string',
            description: 'The city name',
          },
          unit: {
            type: 'string',
            enum: ['celsius', 'fahrenheit'],
            description: 'Temperature unit',
          },
        },
        required: ['city'],
      },
    },
  },
  {
    type: 'function',
    function: {
      name: 'searchDatabase',
      description: 'Search the user database',
      parameters: {
        type: 'object',
        properties: {
          query: { type: 'string', description: 'Search query' },
          limit: { type: 'number', description: 'Max results', default: 10 },
        },
        required: ['query'],
      },
    },
  },
  {
    type: 'function',
    function: {
      name: 'sendEmail',
      description: 'Send an email to a user',
      parameters: {
        type: 'object',
        properties: {
          to: { type: 'string', description: 'Recipient email' },
          subject: { type: 'string', description: 'Email subject' },
          body: { type: 'string', description: 'Email body' },
        },
        required: ['to', 'subject', 'body'],
      },
    },
  },
];

export { tools };
```

## Function Implementations

```ts
// functions.ts — Actual implementations

async function getWeather(args: { city: string; unit?: string }) {
  // In production, call a weather API
  return {
    city: args.city,
    temp: 22,
    unit: args.unit || 'celsius',
    conditions: 'sunny',
  };
}

async function searchDatabase(args: { query: string; limit?: number }) {
  // In production, query your database
  return {
    results: [
      { id: 1, name: 'Alice', email: 'alice@example.com' },
      { id: 2, name: 'Bob', email: 'bob@example.com' },
    ],
  };
}

async function sendEmail(args: { to: string; subject: string; body: string }) {
  // In production, send via email service
  console.log(`Sending email to ${args.to}: ${args.subject}`);
  return { sent: true, messageId: `msg-${Date.now()}` };
}

// Map function names to implementations
const functionMap: Record<string, (args: any) => Promise<any>> = {
  getWeather,
  searchDatabase,
  sendEmail,
};

export { functionMap };
```

## Agent Loop

```ts
// agent.ts — AI agent that can use tools

import openai from './openai.js';
import { tools } from './tools.js';
import { functionMap } from './functions.js';

async function agent(userMessage: string): Promise<string> {
  const messages: Array<OpenAI.Chat.Completions.ChatCompletionMessageParam> = [
    {
      role: 'system',
      content: 'You are a helpful assistant with access to tools. Use them when needed.',
    },
    { role: 'user', content: userMessage },
  ];

  // Agent loop — keep calling until AI responds with text (no more tool calls)
  while (true) {
    const completion = await openai.chat.completions.create({
      model: 'gpt-4o',
      messages,
      tools,
      tool_choice: 'auto',  // AI decides when to use tools
    });

    const responseMessage = completion.choices[0].message;

    // If no tool calls, return the text response
    if (!responseMessage.tool_calls) {
      return responseMessage.content || '';
    }

    // Execute each tool call
    messages.push(responseMessage);  // Add AI's response to history

    for (const toolCall of responseMessage.tool_calls) {
      const functionName = toolCall.function.name;
      const functionArgs = JSON.parse(toolCall.function.arguments);

      console.log(`Executing: ${functionName}(${JSON.stringify(functionArgs)})`);

      // Execute the function
      const functionResult = await functionMap[functionName](functionArgs);

      // Add result to messages
      messages.push({
        role: 'tool',
        tool_call_id: toolCall.id,
        content: JSON.stringify(functionResult),
      });
    }
    // Loop continues — AI sees the tool results and may call more tools or respond
  }
}

// Usage
const response = await agent('What is the weather in Tokyo?');
console.log(response);

const response2 = await agent('Search for users named Alice and email them a welcome message');
console.log(response2);
```

## Common Mistakes

### Mistake 1: Not Handling Multiple Tool Calls

```ts
// WRONG — only handling first tool call
const toolCall = responseMessage.tool_calls[0];  // Ignores others!

// CORRECT — iterate all tool calls
for (const toolCall of responseMessage.tool_calls) {
  await executeToolCall(toolCall);
}
```

### Mistake 2: No Timeout on Tool Execution

```ts
// WRONG — tool call hangs forever
const result = await functionMap[toolCall.function.name](args);

// CORRECT — add timeout
const result = await Promise.race([
  functionMap[toolCall.function.name](args),
  new Promise((_, reject) =>
    setTimeout(() => reject(new Error('Tool timeout')), 10_000)
  ),
]);
```

## Next Steps

For vector storage, continue to [Vector Storage](./05-vector-storage.md).
