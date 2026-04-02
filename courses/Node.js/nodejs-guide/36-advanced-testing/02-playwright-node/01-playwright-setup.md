# Playwright Setup

## What You'll Learn

- What Playwright is and how it works
- How to install and configure Playwright
- How to write your first E2E test
- How Playwright compares to Cypress

## Setup

```bash
npm init playwright@latest
```

```ts
// playwright.config.ts

import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: 'html',
  use: {
    baseURL: 'http://localhost:3000',
    trace: 'on-first-retry',
  },
  projects: [
    { name: 'chromium', use: { ...devices['Desktop Chrome'] } },
    { name: 'firefox', use: { ...devices['Desktop Firefox'] } },
    { name: 'webkit', use: { ...devices['Desktop Safari'] } },
  ],
  webServer: {
    command: 'npm run dev',
    url: 'http://localhost:3000',
    reuseExistingServer: !process.env.CI,
  },
});
```

## First Test

```ts
// tests/home.test.ts

import { test, expect } from '@playwright/test';

test('homepage has title', async ({ page }) => {
  await page.goto('/');
  await expect(page).toHaveTitle(/My App/);
});

test('can navigate to about', async ({ page }) => {
  await page.goto('/');
  await page.click('text=About');
  await expect(page).toHaveURL('/about');
});
```

## Running Tests

```bash
npx playwright test              # Run all tests
npx playwright test --headed     # Show browser
npx playwright test --ui         # Open UI mode
npx playwright show-report       # View HTML report
```

## Next Steps

For E2E patterns, continue to [Playwright E2E](./02-playwright-e2e.md).
