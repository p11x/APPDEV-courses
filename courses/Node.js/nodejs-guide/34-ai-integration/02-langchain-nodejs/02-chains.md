# LangChain Chains

## What You'll Learn

- How to build chains with LangChain
- How to use LCEL (LangChain Expression Language)
- How to compose chains from smaller components
- How to handle errors in chains

## LCEL (LangChain Expression Language)

```ts
// chain.ts — Simple chain

import { model } from './llm.js';
import { ChatPromptTemplate } from '@langchain/core/prompts';
import { StringOutputParser } from '@langchain/core/output_parsers';

// Create a prompt template
const prompt = ChatPromptTemplate.fromMessages([
  ['system', 'You are a {role}. Respond in {style} style.'],
  ['human', '{input}'],
]);

// Create output parser
const outputParser = new StringOutputParser();

// Compose chain: prompt → model → parser
const chain = prompt.pipe(model).pipe(outputParser);

// Invoke chain
const result = await chain.invoke({
  role: 'Node.js expert',
  style: 'concise',
  input: 'What is middleware?',
});

console.log(result);
```

## Sequential Chains

```ts
// sequential-chain.ts — Chain with multiple steps

import { ChatPromptTemplate } from '@langchain/core/prompts';
import { model } from './llm.js';
import { StringOutputParser } from '@langchain/core/output_parsers';

// Step 1: Generate a topic
const topicPrompt = ChatPromptTemplate.fromTemplate(
  'Generate a single interesting fact about {subject}. Be brief.'
);

// Step 2: Expand on the topic
const expandPrompt = ChatPromptTemplate.fromTemplate(
  'Expand on this fact with more detail: {topic}'
);

// Build sequential chain
const topicChain = topicPrompt.pipe(model).pipe(new StringOutputParser());
const expandChain = expandPrompt.pipe(model).pipe(new StringOutputParser());

// Connect chains: output of first becomes input of second
const fullChain = topicChain.pipe(
  (topic) => expandChain.invoke({ topic })
);

const result = await fullChain.invoke({ subject: 'Node.js' });
console.log(result);
```

## Parallel Chains

```ts
// parallel-chain.ts — Run multiple chains in parallel

import { RunnableParallel } from '@langchain/core/runnables';
import { ChatPromptTemplate } from '@langchain/core/prompts';
import { model } from './llm.js';
import { StringOutputParser } from '@langchain/core/output_parsers';

const prosPrompt = ChatPromptTemplate.fromTemplate('List 3 pros of {topic}.');
const consPrompt = ChatPromptTemplate.fromTemplate('List 3 cons of {topic}.');

const prosChain = prosPrompt.pipe(model).pipe(new StringOutputParser());
const consChain = consPrompt.pipe(model).pipe(new StringOutputParser());

// Run both in parallel
const parallel = RunnableParallel.from({
  pros: prosChain,
  cons: consChain,
});

const result = await parallel.invoke({ topic: 'Node.js' });
console.log('Pros:', result.pros);
console.log('Cons:', result.cons);
```

## Next Steps

For agents, continue to [Agents](./03-agents.md).
