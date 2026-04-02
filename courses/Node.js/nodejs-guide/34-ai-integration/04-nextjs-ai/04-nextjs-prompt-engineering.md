# Prompt Engineering

## What You'll Learn

- How to write effective prompts for Node.js code generation
- How to use few-shot examples
- How to structure system prompts
- How to test and iterate on prompts

## Prompt Patterns

### System Prompt Template

```ts
const systemPrompt = `
You are a senior Node.js developer assistant.

Rules:
1. Use ES Modules (import/export) — never require()
2. Use async/await — never .then() chains
3. Use node: protocol for built-in imports
4. Handle all errors explicitly
5. Add inline comments on non-obvious lines

Format:
- Provide complete, runnable code
- Include all imports
- Use TypeScript when appropriate
`;
```

### Few-Shot Examples

```ts
const fewShotPrompt = `
Here are examples of how to write Node.js code:

Example 1: Reading a file
\`\`\`js
import { readFile } from 'node:fs/promises';
const content = await readFile('./file.txt', 'utf-8');
\`\`\`

Example 2: HTTP server
\`\`\`js
import { createServer } from 'node:http';
const server = createServer((req, res) => {
  res.writeHead(200);
  res.end('Hello');
});
server.listen(3000);
\`\`\`

Now, write code for: {userRequest}
`;
```

### Chain-of-Thought

```ts
const chainOfThought = `
Before writing code, think step by step:
1. What is the goal?
2. What inputs/outputs are needed?
3. What Node.js APIs are relevant?
4. What are the edge cases?
5. Write the code with error handling
`;
```

## Next Steps

For embeddings advanced, continue to [Embedding Models](../05-embeddings-advanced/01-embedding-models.md).
