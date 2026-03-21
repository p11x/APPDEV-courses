# Playwright Setup

## Overview

Playwright is a powerful end-to-end testing framework that can test applications across multiple browsers. Unlike unit tests, E2E tests run in a real browser and test complete user flows.

## Prerequisites

- Node.js 18+
- Basic testing knowledge

## Core Concepts

### Installing Playwright

```bash
npm init playwright@latest
# OR
npm install -D @playwright/test
npx playwright install --with-deps
```

### Configuration

```typescript
// File: playwright.config.ts

import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: 'html',
  use: {
    baseURL: 'http://localhost:5173',
    trace: 'on-first-retry',
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
    {
      name: 'firefox',
      use: { ...devices['Desktop Firefox'] },
    },
    {
      name: 'webkit',
      use: { ...devices['Desktop Safari'] },
    },
  ],
  webServer: {
    command: 'npm run dev',
    url: 'http://localhost:5173',
    reuseExistingServer: !process.env.CI,
  },
});
```

### First E2E Test

```typescript
// File: e2e/homepage.spec.ts

import { test, expect } from '@playwright/test';

test('homepage has title and main content', async ({ page }) => {
  await page.goto('/');
  
  // Check title
  await expect(page).toHaveTitle(/My App/);
  
  // Check heading
  await expect(page.getByRole('heading', { name: /welcome/i })).toBeVisible();
});

test('navigation works', async ({ page }) => {
  await page.goto('/');
  
  await page.click('text=About');
  await expect(page).toHaveURL(/.*about/);
  await expect(page.getByRole('heading', { name: /about/i })).toBeVisible();
});
```

## Key Takeaways

- Playwright runs tests in real browsers
- Configure multiple browser projects
- Use webServer to start dev server automatically
- Use trace for debugging failures

## What's Next

Continue to [Writing E2E Tests](/10-testing/03-e2e-testing/02-writing-e2e-tests.md) to learn about writing comprehensive E2E tests.