# Playwright Setup

## What You'll Learn
- Installing Playwright
- Configuring for Next.js
- Writing your first E2E test
- Running tests

## Prerequisites
- Node.js installed
- A Next.js project
- Basic understanding of testing

## Concept Explained Simply

Playwright is like having a real person test your app — it opens an actual browser, clicks through your pages, fills forms, and checks that everything works. Unlike unit tests that test individual pieces, E2E (end-to-end) tests verify that your entire application works from the user's perspective.

Think of it like test-driving a car: you don't just test individual engine parts, you drive it around the block to make sure everything works together.

## Complete Code Example

### Installation

```bash
# Install Playwright
npm init playwright@latest

# Or install manually
npm install --save-dev @playwright/test
npx playwright install --with-deps chromium
```

### Configuration

```typescript
// playwright.config.ts
import { defineConfig, devices } from "@playwright/test";

export default defineConfig({
  // Run tests in parallel
  fullyParallel: true,
  
  // Fail fast on CI
  forbidOnly: !!process.env.CI,
  
  // Retry on CI
  retries: process.env.CI ? 2 : 0,
  
  // Workers
  workers: process.env.CI ? 1 : undefined,
  
  // Reporter
  reporter: "html",
  
  // Shared settings
  use: {
    // Base URL
    baseURL: "http://localhost:3000",
    
    // Collect trace on failure
    trace: "on-first-retry",
    
    // Take screenshot on failure
    screenshot: "only-on-failure",
  },
  
  // Projects to run
  projects: [
    {
      name: "chromium",
      use: { ...devices["Desktop Chrome"] },
    },
    {
      name: "firefox",
      use: { ...devices["Desktop Firefox"] },
    },
    {
      name: "webkit",
      use: { ...devices["Desktop Safari"] },
    },
  ],
  
  // Local web server
  webServer: {
    command: "npm run dev",
    url: "http://localhost:3000",
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
  },
});
```

### Test File

```typescript
// tests/example.spec.ts
import { test, expect } from "@playwright/test";

test("has title", async ({ page }) => {
  await page.goto("/");
  
  // Check title
  await expect(page).toHaveTitle(/My App/);
});

test("can navigate to about page", async ({ page }) => {
  await page.goto("/");
  
  // Click about link
  await page.click("text=About");
  
  // Check URL
  await expect(page).toHaveURL(/.*about/);
});
```

### Running Tests

```bash
# Run all tests
npx playwright test

# Run specific file
npx playwright test tests/example.spec.ts

# Run with UI
npx playwright test --ui

# Run in headed mode (see browser)
npx playwright test --headed

# Run specific project
npx playwright test --project=chromium
```

### Package.json Scripts

```json
{
  "scripts": {
    "test": "playwright test",
    "test:ui": "playwright test --ui",
    "test:headed": "playwright test --headed",
    "test:ci": "playwright test --reporter=line"
  }
}
```

## Test Structure

```typescript
// tests/home.spec.ts

// Test suite
test.describe("Home Page", () => {
  // Before each test
  test.beforeEach(async ({ page }) => {
    await page.goto("/");
  });
  
  // Individual test
  test("displays welcome message", async ({ page }) => {
    await expect(page.locator("h1")).toContainText("Welcome");
  });
  
  test("can search for products", async ({ page }) => {
    // Type in search
    await page.fill('[data-testid="search-input"]', "laptop");
    
    // Submit
    await page.click('[data-testid="search-button"]');
    
    // Check results
    await expect(page.locator('[data-testid="results"]')).toBeVisible();
  });
});
```

## Locators

```typescript
// Different ways to find elements

// By text
await page.click("text=Submit");
await page.click("text=Submit");

// By CSS selector
await page.click(".button-primary");
await page.fill("#email", "test@example.com");

// By role
await page.getByRole("button", { name: "Submit" }).click();
await page.getByRole("textbox", { name: "Email" }).fill("test@example.com");

// By label
await page.getByLabel("Email").fill("test@example.com");
await page.getByPlaceholder("Enter your email").fill("test@example.com");

// By test ID
await page.getByTestId("submit-button").click();
```

## Common Mistakes

### Mistake 1: Not Waiting for Navigation

```typescript
// WRONG - Click and immediately check
await page.click("text=About");
expect(page.url()).toContain("about"); // Might fail!

// CORRECT - Wait for navigation
await page.click("text=About");
await page.waitForURL(/.*about/);
```

### Mistake 2: Not Waiting for Elements

```typescript
// WRONG - Check immediately
await page.goto("/");
const element = page.locator(".loading");
// Might not be visible yet!

// CORRECT - Wait for element
await expect(page.locator(".loading")).toBeHidden();
```

### Mistake 3: Hardcoding Ports

```typescript
// WRONG - Hardcoded URL
await page.goto("http://localhost:3000");

// CORRECT - Use baseURL from config
await page.goto("/");
```

## Summary

- Install Playwright with `npm init playwright@latest`
- Configure baseURL and webServer in playwright.config.ts
- Write tests using test() and expect()
- Use locators (getByRole, getByLabel, getByText) to find elements
- Run tests with `npx playwright test`
- Use `--ui` flag for visual test debugging

## Next Steps

- [writing-e2e-tests.md](./writing-e2e-tests.md) - Writing comprehensive E2E tests
- [testing-auth-flows.md](./testing-auth-flows.md) - Testing authentication
