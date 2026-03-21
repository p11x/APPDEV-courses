# Adding Tests

## What You'll Learn
- Unit tests
- E2E tests
- CI integration

## Prerequisites
- App built

## Do I Need This Right Now?
Tests ensure your app works correctly.

## Testing Setup

This references **Section 15 (Testing)** — Jest + Playwright setup.

```typescript
// tests/tasks.test.ts
import { render, screen } from '@testing-library/react';
import { TaskList } from '@/components/TaskList';

test('renders tasks', () => {
  render(<TaskList tasks={[{ id: '1', title: 'Test Task', completed: false }]} />);
  expect(screen.getByText('Test Task')).toBeInTheDocument();
});
```

```typescript
// tests/dashboard.spec.ts
import { test, expect } from '@playwright/test';

test('dashboard shows tasks', async ({ page }) => {
  await page.goto('/dashboard');
  await expect(page.getByText('My Tasks')).toBeVisible();
});
```

## Summary
- Write unit tests with Jest
- Write e2e tests with Playwright
- Run in CI pipeline
