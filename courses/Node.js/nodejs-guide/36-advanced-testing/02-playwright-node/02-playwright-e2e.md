# Playwright E2E Testing

## What You'll Learn

- How to write E2E tests with Playwright
- How to test forms and user flows
- How to test API responses
- How to handle authentication in tests

## Testing User Flows

```ts
// tests/login.test.ts

import { test, expect } from '@playwright/test';

test('user can log in', async ({ page }) => {
  await page.goto('/login');

  // Fill form
  await page.fill('[name="email"]', 'alice@example.com');
  await page.fill('[name="password"]', 'secret');

  // Submit
  await page.click('button[type="submit"]');

  // Verify redirect and content
  await expect(page).toHaveURL('/dashboard');
  await expect(page.getByText('Welcome, Alice')).toBeVisible();
});

test('shows error for invalid credentials', async ({ page }) => {
  await page.goto('/login');

  await page.fill('[name="email"]', 'wrong@example.com');
  await page.fill('[name="password"]', 'wrong');
  await page.click('button[type="submit"]');

  await expect(page.getByText('Invalid credentials')).toBeVisible();
});
```

## API Testing

```ts
// tests/api.test.ts

import { test, expect } from '@playwright/test';

test('API returns users', async ({ request }) => {
  const response = await request.get('/api/users');
  expect(response.ok()).toBeTruthy();

  const users = await response.json();
  expect(users).toHaveLength(2);
});

test('API creates user', async ({ request }) => {
  const response = await request.post('/api/users', {
    data: { name: 'Charlie', email: 'charlie@example.com' },
  });

  expect(response.status()).toBe(201);
  const user = await response.json();
  expect(user.name).toBe('Charlie');
});
```

## Next Steps

For testing patterns, continue to [Playwright Testing](./03-playwright-testing.md).
