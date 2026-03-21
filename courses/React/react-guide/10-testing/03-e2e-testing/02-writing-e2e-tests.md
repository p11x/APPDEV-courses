# Writing E2E Tests

## Overview

Writing effective E2E tests involves testing complete user flows in a real browser. Playwright provides powerful APIs for interacting with pages, waiting for conditions, and verifying results.

## Prerequisites

- Playwright setup completed

## Core Concepts

### Complete Login Flow Test

```typescript
// File: e2e/login.spec.ts

import { test, expect } from '@playwright/test';

test('complete login flow', async ({ page }) => {
  // Navigate to login page
  await page.goto('/login');
  
  // Fill in credentials
  await page.fill('input[name="email"]', 'test@example.com');
  await page.fill('input[name="password"]', 'password123');
  
  // Submit form
  await page.click('button[type="submit"]');
  
  // Wait for navigation to dashboard
  await expect(page).toHaveURL(/.*dashboard/);
  
  // Verify logged in state
  await expect(page.getByText('Welcome, test@example.com')).toBeVisible();
});

test('login shows error with invalid credentials', async ({ page }) => {
  await page.goto('/login');
  
  await page.fill('input[name="email"]', 'wrong@example.com');
  await page.fill('input[name="password"]', 'wrongpassword');
  await page.click('button[type="submit"]');
  
  // Check for error message
  await expect(page.getByText('Invalid credentials')).toBeVisible();
});
```

### Locator Strategies

```typescript
// Prefer these (most reliable)
await page.getByRole('button', { name: 'Submit' });
await page.getByLabel('Email');
await page.getByPlaceholder('Enter your email');
await page.getByTestId('submit-button');

// Avoid these (fragile)
await page.locator('.button.primary');
await page.locator('button:nth-child(2)');
```

## Key Takeaways

- Test complete user flows
- Use reliable locators (getByRole, getByLabel)
- Wait for conditions explicitly
- Handle async operations properly

## What's Next

Continue to [CI/CD Testing Pipeline](/10-testing/03-e2e-testing/03-ci-cd-testing-pipeline.md) to learn about automating tests in CI/CD.