# E2E Testing

## What You'll Learn

- How to write E2E tests
- How to test complete user flows
- How to handle authentication in E2E tests
- How to debug E2E tests

## User Flow Test

```ts
// tests/e2e/registration.test.ts

import { test, expect } from '@playwright/test';

test('complete registration flow', async ({ page }) => {
  // 1. Navigate to registration
  await page.goto('/register');

  // 2. Fill registration form
  await page.fill('[name="name"]', 'New User');
  await page.fill('[name="email"]', 'new@example.com');
  await page.fill('[name="password"]', 'SecurePass123!');
  await page.fill('[name="confirmPassword"]', 'SecurePass123!');

  // 3. Submit
  await page.click('button[type="submit"]');

  // 4. Verify redirect to dashboard
  await expect(page).toHaveURL('/dashboard');

  // 5. Verify welcome message
  await expect(page.getByText('Welcome, New User')).toBeVisible();

  // 6. Verify user can access protected content
  await page.click('text=Profile');
  await expect(page.getByText('new@example.com')).toBeVisible();
});
```

## Next Steps

For performance testing, continue to [Performance Testing](./05-performance-testing.md).
