# Playwright Testing Patterns

## What You'll Learn

- Page Object Model with Playwright
- Visual regression testing
- Accessibility testing
- Performance testing

## Page Object Model

```ts
// pages/LoginPage.ts

import { type Page, type Locator } from '@playwright/test';

export class LoginPage {
  readonly page: Page;
  readonly emailInput: Locator;
  readonly passwordInput: Locator;
  readonly submitButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.emailInput = page.locator('[name="email"]');
    this.passwordInput = page.locator('[name="password"]');
    this.submitButton = page.locator('button[type="submit"]');
  }

  async goto() {
    await this.page.goto('/login');
  }

  async login(email: string, password: string) {
    await this.emailInput.fill(email);
    await this.passwordInput.fill(password);
    await this.submitButton.click();
  }
}
```

```ts
// tests/login-pom.test.ts

import { test, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';

test('login with valid credentials', async ({ page }) => {
  const loginPage = new LoginPage(page);
  await loginPage.goto();
  await loginPage.login('alice@example.com', 'secret');

  await expect(page).toHaveURL('/dashboard');
});
```

## Visual Regression

```ts
test('homepage matches screenshot', async ({ page }) => {
  await page.goto('/');
  await expect(page).toHaveScreenshot('homepage.png');
});
```

## Next Steps

For fixtures, continue to [Playwright Fixtures](./04-playwright-fixtures.md).
