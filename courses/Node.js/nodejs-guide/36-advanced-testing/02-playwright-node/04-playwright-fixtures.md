# Playwright Fixtures

## What You'll Learn

- How to create custom fixtures
- How to share test data
- How to set up and tear down test state
- How to use built-in fixtures

## Custom Fixtures

```ts
// tests/fixtures.ts

import { test as base, type Page } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';

type MyFixtures = {
  loginPage: LoginPage;
  authenticatedPage: Page;
};

export const test = base.extend<MyFixtures>({
  loginPage: async ({ page }, use) => {
    const loginPage = new LoginPage(page);
    await use(loginPage);
  },

  authenticatedPage: async ({ page }, use) => {
    // Log in before each test
    await page.goto('/login');
    await page.fill('[name="email"]', 'alice@example.com');
    await page.fill('[name="password"]', 'secret');
    await page.click('button[type="submit"]');
    await page.waitForURL('/dashboard');

    await use(page);
  },
});

export { expect } from '@playwright/test';
```

## Using Fixtures

```ts
// tests/dashboard.test.ts

import { test, expect } from './fixtures';

test('dashboard shows user name', async ({ authenticatedPage }) => {
  await expect(authenticatedPage.getByText('Welcome, Alice')).toBeVisible();
});

test('dashboard has navigation', async ({ authenticatedPage }) => {
  await expect(authenticatedPage.getByRole('navigation')).toBeVisible();
});
```

## Next Steps

For CI, continue to [Playwright CI](./05-playwright-ci.md).
