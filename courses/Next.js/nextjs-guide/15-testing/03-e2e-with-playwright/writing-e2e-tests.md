# Writing E2E Tests

## What You'll Learn
- Writing comprehensive E2E tests
- Testing user flows
- Handling forms and interactions
- Asserting page content

## Prerequisites
- Understanding of Playwright setup
- Basic test structure knowledge
- Familiarity with async/await

## Concept Explained Simply

Writing E2E (end-to-end) tests means writing a script that acts like a real user going through your app. You start at the homepage, click around, fill forms, and verify everything works as expected. These tests catch bugs that unit tests miss because they test the entire system together.

Think of it like following a recipe from start to finish: you gather ingredients (navigate to pages), prepare them (interact with elements), and cook the dish (verify results).

## Complete Code Example

### Testing a Complete User Flow

```typescript
// tests/user-flow.spec.ts
import { test, expect } from "@playwright/test";

test.describe("Complete User Flow", () => {
  test("user can sign up, login, and create a post", async ({ page }) => {
    // 1. Visit home page
    await page.goto("/");
    await expect(page).toHaveTitle(/My App/);
    
    // 2. Navigate to signup
    await page.click("text=Sign Up");
    await expect(page).toHaveURL(/.*signup/);
    
    // 3. Fill signup form
    await page.fill('[name="email"]', "test@example.com");
    await page.fill('[name="password"]', "password123");
    await page.fill('[name="confirmPassword"]', "password123");
    await page.click('button[type="submit"]');
    
    // 4. Should redirect to dashboard
    await expect(page).toHaveURL(/.*dashboard/);
    await expect(page.locator("text=Welcome")).toBeVisible();
    
    // 5. Navigate to create post
    await page.click("text=Create Post");
    await expect(page).toHaveURL(/.*posts\/new/);
    
    // 6. Create a post
    await page.fill('[name="title"]', "My First Post");
    await page.fill('[name="content"]', "This is my first post content!");
    await page.click('button[type="submit"]');
    
    // 7. Verify post was created
    await expect(page).toHaveURL(/.*posts/);
    await expect(page.locator("text=My First Post")).toBeVisible();
  });
});
```

### Testing a Form with Validation

```typescript
// tests/contact-form.spec.ts
import { test, expect } from "@playwright/test";

test.describe("Contact Form", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto("/contact");
  });
  
  test("shows validation errors for empty submission", async ({ page }) => {
    // Submit empty form
    await page.click('button[type="submit"]');
    
    // Check validation messages
    await expect(page.locator("text=Name is required")).toBeVisible();
    await expect(page.locator("text=Email is required")).toBeVisible();
    await expect(page.locator("text=Message is required")).toBeVisible();
  });
  
  test("shows error for invalid email", async ({ page }) => {
    await page.fill('[name="name"]', "John");
    await page.fill('[name="email"]', "not-an-email");
    await page.fill('[name="message"]', "Hello");
    
    await page.click('button[type="submit"]');
    
    await expect(page.locator("text=Invalid email format")).toBeVisible();
  });
  
  test("submits successfully with valid data", async ({ page }) => {
    await page.fill('[name="name"]', "John Doe");
    await page.fill('[name="email"]', "john@example.com");
    await page.fill('[name="message"]', "Hello, this is a test message.");
    
    await page.click('button[type="submit"]');
    
    // Should show success message
    await expect(page.locator("text=Thank you for your message!")).toBeVisible();
  });
});
```

### Testing Navigation and Routing

```typescript
// tests/navigation.spec.ts
import { test, expect } from "@playwright/test";

test.describe("Navigation", () => {
  test("can navigate through main pages", async ({ page }) => {
    // Home
    await page.goto("/");
    await expect(page.locator("h1")).toContainText("Welcome");
    
    // About
    await page.click("text=About");
    await expect(page).toHaveURL(/.*about/);
    await expect(page.locator("h1")).toContainText("About");
    
    // Products
    await page.click("text=Products");
    await expect(page).toHaveURL(/.*products/);
    await expect(page.locator("h1")).toContainText("Products");
    
    // Contact
    await page.click("text=Contact");
    await expect(page).toHaveURL(/.*contact/);
    await expect(page.locator("h1")).toContainText("Contact");
  });
  
  test("navigation menu works on mobile", async ({ page }) => {
    // Set mobile viewport
    await page.setViewportSize({ width: 375, height: 667 });
    
    await page.goto("/");
    
    // Menu should be closed
    await expect(page.locator(".nav-links")).toBeHidden();
    
    // Open menu
    await page.click(".menu-toggle");
    await expect(page.locator(".nav-links")).toBeVisible();
    
    // Navigate
    await page.click(".nav-links a:first-child");
    await expect(page).toHaveURL(/.*home/);
  });
});
```

### Testing Async Data Loading

```typescript
// tests/products.spec.ts
import { test, expect } from "@playwright/test";

test.describe("Products Page", () => {
  test("shows loading state then displays products", async ({ page }) => {
    await page.goto("/products");
    
    // Should show loading
    await expect(page.locator("text=Loading...")).toBeVisible();
    
    // Wait for products to load
    await expect(page.locator('[data-testid="product-card"]')).toHaveCount(3);
    
    // Loading should be hidden
    await expect(page.locator("text=Loading...")).toBeHidden();
  });
  
  test("filter products by category", async ({ page }) => {
    await page.goto("/products");
    
    // Wait for products
    await expect(page.locator('[data-testid="product-card"]')).toHaveCount(3);
    
    // Filter by electronics
    await page.selectOption('[name="category"]', "electronics");
    
    // Should show filtered results
    await expect(page.locator('[data-testid="product-card"]')).toHaveCount(1);
    await expect(page.locator("text=Laptop")).toBeVisible();
  });
});
```

## Common Assertions

```typescript
// Page assertions
await expect(page).toHaveTitle("Expected Title");
await expect(page).toHaveURL(/.*expected/);
await expect(page).toHaveScreenshot("expected.png");

// Element assertions
await expect(locator).toBeVisible();
await expect(locator).toBeHidden();
await expect(locator).toBeEnabled();
await expect(locator).toBeDisabled();
await expect(locator).toContainText("Expected text");
await expect(locator).toHaveText("Exact text");
await expect(locator).toHaveValue("Expected value");
await expect(locator).toBeChecked();
await expect(locator).toHaveAttribute("href", "/expected");
```

## Common Mistakes

### Mistake 1: Not Waiting for Elements

```typescript
// WRONG - Immediately checking
await page.goto("/");
const element = page.locator(".new-element");
// Not waiting = flaky tests!

// CORRECT - Wait for element
await page.goto("/");
await expect(page.locator(".new-element")).toBeVisible();
```

### Mistake 2: Hardcoded Sleeps

```typescript
// WRONG - Using sleep
await page.goto("/");
await page.waitForTimeout(2000); // Bad practice!

// CORRECT - Wait for specific condition
await page.goto("/");
await expect(page.locator(".loaded")).toBeVisible();
```

### Mistake 3: Not Using Test IDs

```typescript
// WRONG - Fragile selectors
await page.click(".primary-btn"); // Could match multiple!

// CORRECT - Use data-testid
await page.getByTestId("submit-button").click();
```

## Summary

- E2E tests simulate real user interactions
- Use test.describe to group related tests
- Use test.beforeEach for setup
- Wait for elements with expect(locator).toBeVisible()
- Navigate with page.goto() and page.click()
- Fill forms with page.fill()
- Check results with assertions

## Next Steps

- [testing-auth-flows.md](./testing-auth-flows.md) - Testing authentication flows
- [jest-setup-in-nextjs.md](../01-unit-testing-with-jest/jest-setup-in-nextjs.md) - Unit testing
