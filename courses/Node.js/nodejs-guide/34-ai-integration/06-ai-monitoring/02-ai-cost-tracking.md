# AI Cost Tracking

## What You'll Learn

- How to track AI API costs per user/request
- How to set budget limits
- How to optimize costs
- How to report cost analytics

## Cost Tracker

```ts
// cost-tracker.ts

import Redis from 'ioredis';

const redis = new Redis();

interface CostConfig {
  model: string;
  promptCostPer1K: number;
  completionCostPer1K: number;
}

const COSTS: Record<string, CostConfig> = {
  'gpt-4o': { model: 'gpt-4o', promptCostPer1K: 0.005, completionCostPer1K: 0.015 },
  'gpt-4o-mini': { model: 'gpt-4o-mini', promptCostPer1K: 0.00015, completionCostPer1K: 0.0006 },
};

export async function trackCost(
  userId: string,
  model: string,
  usage: { prompt_tokens: number; completion_tokens: number }
) {
  const config = COSTS[model];
  if (!config) return;

  const cost =
    (usage.prompt_tokens / 1000) * config.promptCostPer1K +
    (usage.completion_tokens / 1000) * config.completionCostPer1K;

  // Track per user
  await redis.incrByFloat(`cost:user:${userId}`, cost);
  await redis.incrByFloat(`cost:daily:${new Date().toISOString().slice(0, 10)}`, cost);
  await redis.incrByFloat(`cost:model:${model}`, cost);

  return cost;
}

export async function getUserCost(userId: string): Promise<number> {
  return parseFloat(await redis.get(`cost:user:${userId}`) || '0');
}

export async function getDailyCost(): Promise<number> {
  const today = new Date().toISOString().slice(0, 10);
  return parseFloat(await redis.get(`cost:daily:${today}`) || '0');
}
```

## Budget Limits

```ts
// budget.ts

export async function checkBudget(userId: string, maxDailyCost: number): Promise<boolean> {
  const cost = await getUserCost(userId);
  return cost < maxDailyCost;
}

// Middleware
export async function budgetMiddleware(req: Request, res: Response, next: NextFunction) {
  const userId = req.user?.id;
  if (!userId) return next();

  const allowed = await checkBudget(userId, 10);  // $10/day limit
  if (!allowed) {
    return res.status(429).json({ error: 'Daily AI budget exceeded' });
  }

  next();
}
```

## Next Steps

For performance, continue to [AI Performance](./03-ai-performance.md).
