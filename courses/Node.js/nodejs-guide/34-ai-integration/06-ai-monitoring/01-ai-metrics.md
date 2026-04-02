# AI Metrics

## What You'll Learn

- How to track AI API usage metrics
- How to measure response quality
- How to set up monitoring dashboards
- How to detect anomalies in AI usage

## Usage Tracking

```ts
// ai-metrics.ts

import { Counter, Histogram } from 'prom-client';

// Token usage counter
export const tokenCounter = new Counter({
  name: 'ai_tokens_total',
  help: 'Total tokens used',
  labelNames: ['model', 'type'],  // type: prompt | completion
});

// Request duration
export const requestDuration = new Histogram({
  name: 'ai_request_duration_seconds',
  help: 'AI API request duration',
  labelNames: ['model', 'status'],
  buckets: [0.5, 1, 2, 5, 10, 30],
});

// Cost tracking
export const costCounter = new Counter({
  name: 'ai_cost_usd_total',
  help: 'Total AI API cost in USD',
  labelNames: ['model'],
});
```

## Middleware

```ts
// ai-middleware.ts

import { tokenCounter, requestDuration, costCounter } from './ai-metrics.js';

const MODEL_COSTS: Record<string, { prompt: number; completion: number }> = {
  'gpt-4o': { prompt: 0.005, completion: 0.015 },
  'gpt-4o-mini': { prompt: 0.00015, completion: 0.0006 },
  'text-embedding-3-small': { prompt: 0.00002, completion: 0 },
};

export function trackAIUsage(model: string, usage: { prompt_tokens: number; completion_tokens: number }) {
  tokenCounter.inc({ model, type: 'prompt' }, usage.prompt_tokens);
  tokenCounter.inc({ model, type: 'completion' }, usage.completion_tokens);

  const cost = MODEL_COSTS[model];
  if (cost) {
    const totalCost =
      (usage.prompt_tokens / 1000) * cost.prompt +
      (usage.completion_tokens / 1000) * cost.completion;
    costCounter.inc({ model }, totalCost);
  }
}
```

## Next Steps

For cost tracking, continue to [AI Cost Tracking](./02-ai-cost-tracking.md).
