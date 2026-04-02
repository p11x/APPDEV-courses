# End-to-End Testing with Playwright

## What You'll Learn

- Playwright setup and configuration
- Writing E2E tests
- Page object pattern
- Cross-browser testing
- Visual regression testing

## Playwright Setup

```bash
npm install --save-dev @playwright/test
npx playwright install
```

```javascript
// playwright.config.js
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
    testDir: './e2e',
    timeout: 30000,
    retries: process.env.CI ? 2 : 0,
    workers: process.env.CI ? 1 : undefined,
    reporter: [
        ['html', { open: 'never' }],
        ['junit', { outputFile: 'test-results/junit.xml' }],
    ],
    use: {
        baseURL: 'http://localhost:3000',
        screenshot: 'only-on-failure',
        trace: 'on-first-retry',
        video: 'on-first-retry',
    },
    projects: [
        { name: 'chromium', use: { ...devices['Desktop Chrome'] } },
        { name: 'firefox', use: { ...devices['Desktop Firefox'] } },
        { name: 'webkit', use: { ...devices['Desktop Safari'] } },
        { name: 'mobile', use: { ...devices['iPhone 14'] } },
    ],
    webServer: {
        command: 'npm run start:test',
        port: 3000,
        reuseExistingServer: !process.env.CI,
    },
});
```

## E2E Test Examples

```javascript
import { test, expect } from '@playwright/test';

test.describe('User Authentication', () => {
    test('user can login successfully', async ({ page }) => {
        await page.goto('/login');

        await page.fill('[data-testid="email"]', 'user@example.com');
        await page.fill('[data-testid="password"]', 'Password123!');
        await page.click('[data-testid="login-submit"]');

        await expect(page).toHaveURL('/dashboard');
        await expect(page.locator('[data-testid="welcome"]')).toContainText('Welcome');
    });

    test('shows error for invalid credentials', async ({ page }) => {
        await page.goto('/login');

        await page.fill('[data-testid="email"]', 'wrong@example.com');
        await page.fill('[data-testid="password"]', 'WrongPass');
        await page.click('[data-testid="login-submit"]');

        await expect(page.locator('[data-testid="error"]')).toContainText('Invalid credentials');
    });

    test('user can register', async ({ page }) => {
        await page.goto('/register');

        await page.fill('[data-testid="name"]', 'New User');
        await page.fill('[data-testid="email"]', `new-${Date.now()}@test.com`);
        await page.fill('[data-testid="password"]', 'SecurePass123!');
        await page.fill('[data-testid="confirm-password"]', 'SecurePass123!');
        await page.click('[data-testid="register-submit"]');

        await expect(page).toHaveURL('/dashboard');
    });
});

test.describe('User Journey', () => {
    test.use({ storageState: 'auth/user.json' }); // Pre-authenticated

    test('complete shopping flow', async ({ page }) => {
        // Browse products
        await page.goto('/products');
        await page.click('[data-testid="product-1"]');
        await expect(page.locator('[data-testid="product-title"]')).toBeVisible();

        // Add to cart
        await page.click('[data-testid="add-to-cart"]');
        await expect(page.locator('[data-testid="cart-count"]')).toContainText('1');

        // Checkout
        await page.click('[data-testid="cart-icon"]');
        await page.click('[data-testid="checkout"]');
        await expect(page).toHaveURL('/checkout');

        // Fill shipping
        await page.fill('[data-testid="address"]', '123 Main St');
        await page.fill('[data-testid="city"]', 'New York');
        await page.click('[data-testid="place-order"]');

        // Verify order
        await expect(page.locator('[data-testid="order-success"]')).toBeVisible();
    });
});
```

## Page Object Pattern

```javascript
// e2e/pages/LoginPage.js
export class LoginPage {
    constructor(page) {
        this.page = page;
        this.emailInput = page.locator('[data-testid="email"]');
        this.passwordInput = page.locator('[data-testid="password"]');
        this.submitButton = page.locator('[data-testid="login-submit"]');
        this.errorMessage = page.locator('[data-testid="error"]');
    }

    async goto() {
        await this.page.goto('/login');
    }

    async login(email, password) {
        await this.emailInput.fill(email);
        await this.passwordInput.fill(password);
        await this.submitButton.click();
    }

    async getError() {
        return this.errorMessage.textContent();
    }
}

// e2e/pages/DashboardPage.js
export class DashboardPage {
    constructor(page) {
        this.page = page;
        this.welcomeMessage = page.locator('[data-testid="welcome"]');
        this.logoutButton = page.locator('[data-testid="logout"]');
    }

    async getWelcomeText() {
        return this.welcomeMessage.textContent();
    }

    async logout() {
        await this.logoutButton.click();
    }
}

// Using page objects in tests
import { test, expect } from '@playwright/test';
import { LoginPage } from './pages/LoginPage.js';
import { DashboardPage } from './pages/DashboardPage.js';

test('login and verify dashboard', async ({ page }) => {
    const loginPage = new LoginPage(page);
    const dashboardPage = new DashboardPage(page);

    await loginPage.goto();
    await loginPage.login('user@example.com', 'Password123!');

    await expect(page).toHaveURL('/dashboard');
    expect(await dashboardPage.getWelcomeText()).toContain('Welcome');
});
```

## Auth Setup for E2E

```javascript
// e2e/auth.setup.js
import { test as setup, expect } from '@playwright/test';

setup('authenticate', async ({ page }) => {
    await page.goto('/login');
    await page.fill('[data-testid="email"]', 'test@example.com');
    await page.fill('[data-testid="password"]', 'TestPass123!');
    await page.click('[data-testid="login-submit"]');

    await expect(page).toHaveURL('/dashboard');

    // Save auth state
    await page.context().storageState({ path: 'auth/user.json' });
});
```

## Best Practices Checklist

- [ ] Use `data-testid` attributes for selectors
- [ ] Use page object pattern for maintainability
- [ ] Run across multiple browsers (Chromium, Firefox, WebKit)
- [ ] Capture screenshots and traces on failure
- [ ] Use auth state files for authenticated tests
- [ ] Keep E2E tests focused on critical user journeys
- [ ] Use `waitFor` for async elements

## Cross-References

- See [API Testing](../06-api-testing/01-rest-graphql.md) for API E2E
- See [CI/CD](../10-testing-automation/01-ci-cd-integration.md) for CI integration
- See [Production Testing](../11-testing-production/01-canary-feature-flags.md) for prod testing

## Next Steps

Continue to [API Testing](../06-api-testing/01-rest-graphql.md).
