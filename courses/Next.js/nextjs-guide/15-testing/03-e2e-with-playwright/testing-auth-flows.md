# Testing Authentication Flows

## What You'll Learn
- Testing login/logout flows
- Testing protected routes
- Testing session persistence
- Handling auth in E2E tests

## Prerequisites
- Understanding of Playwright
- Knowledge of auth concepts
- Familiarity with E2E testing

## Concept Explained Simply

Testing authentication is critical because it's the gatekeeper to your app. You need to verify that unauthorized users can't access protected content, authorized users can access what they should, and sessions work correctly.

Think of it like testing security doors: you test that the door locks properly, that only people with keys can enter, and that the key still works after some time.

## Complete Code Example

### Setting Up Auth Testing

```typescript
// tests/auth.spec.ts
import { test, expect } from "@playwright/test";

test.describe("Authentication", () => {
  // Helper to login
  async function login(page, email = "test@example.com") {
    // Set cookie directly to simulate logged-in state
    await page.goto("/login");
    await page.fill('[name="email"]', email);
    await page.fill('[name="password"]', "password123");
    await page.click('button[type="submit"]');
  }
  
  test("user can log in", async ({ page }) => {
    await page.goto("/login");
    
    // Fill login form
    await page.fill('[name="email"]', "test@example.com");
    await page.fill('[name="password"]', "password123");
    await page.click('button[type="submit"]');
    
    // Should redirect to dashboard
    await expect(page).toHaveURL(/.*dashboard/);
    await expect(page.locator("text=Welcome")).toBeVisible();
  });
  
  test("user can log out", async ({ page }) => {
    // Login first
    await login(page);
    
    // Click logout
    await page.click("text=Log Out");
    
    // Should redirect to login or home
    await expect(page).toHaveURL(/.*(login|)\/?$/);
  });
  
  test("shows error for invalid credentials", async ({ page }) => {
    await page.goto("/login");
    
    // Try with wrong password
    await page.fill('[name="email"]', "test@example.com");
    await page.fill('[name="password"]', "wrongpassword");
    await page.click('button[type="submit"]');
    
    // Should show error
    await expect(page.locator("text=Invalid credentials")).toBeVisible();
  });
});
```

### Testing Protected Routes

```typescript
// tests/protected-routes.spec.ts
import { test, expect } from "@playwright/test";

test.describe("Protected Routes", () => {
  test("redirects to login when accessing protected route", async ({ page }) => {
    // Try to access protected page directly
    await page.goto("/dashboard");
    
    // Should redirect to login
    await expect(page).toHaveURL(/.*login/);
  });
  
  test("allows access when logged in", async ({ page }) => {
    // First login via UI
    await page.goto("/login");
    await page.fill('[name="email"]', "test@example.com");
    await page.fill('[name="password"]', "password123");
    await page.click('button[type="submit"]');
    
    // Now access protected page
    await page.goto("/dashboard");
    await expect(page).toHaveURL(/.*dashboard/);
    await expect(page.locator("text=Dashboard")).toBeVisible();
  });
  
  test("remembers login across page refreshes", async ({ page }) => {
    // Login
    await page.goto("/login");
    await page.fill('[name="email"]', "test@example.com");
    await page.fill('[name="password"]', "password123");
    await page.click('button[type="submit"]');
    
    // Refresh page
    await page.reload();
    
    // Should still be logged in
    await expect(page.locator("text=Welcome")).toBeVisible();
  });
});
```

### Testing Registration

```typescript
// tests/registration.spec.ts
import { test, expect } from "@playwright/test";

test.describe("Registration", () => {
  test("can register new account", async ({ page }) => {
    await page.goto("/register");
    
    // Fill form
    await page.fill('[name="name"]', "New User");
    await page.fill('[name="email"]', "newuser@example.com");
    await page.fill('[name="password"]', "password123");
    await page.fill('[name="confirmPassword"]', "password123");
    await page.check('[name="terms"]');
    await page.click('button[type="submit"]');
    
    // Should be redirected to dashboard
    await expect(page).toHaveURL(/.*dashboard/);
    await expect(page.locator("text=Welcome New User")).toBeVisible();
  });
  
  test("shows error when passwords don't match", async ({ page }) => {
    await page.goto("/register");
    
    await page.fill('[name="name"]', "New User");
    await page.fill('[name="email"]', "newuser@example.com");
    await page.fill('[name="password"]', "password123");
    await page.fill('[name="confirmPassword"]', "differentpassword");
    await page.check('[name="terms"]');
    await page.click('button[type="submit"]');
    
    await expect(page.locator("text=Passwords do not match")).toBeVisible();
  });
  
  test("shows error for existing email", async ({ page }) => {
    // First create an account
    // (In real tests, might use API or database directly)
    
    // Try to register with same email
    await page.goto("/register");
    await page.fill('[name="name"]', "Duplicate User");
    await page.fill('[name="email"]', "existing@example.com");
    await page.fill('[name="password"]', "password123");
    await page.fill('[name="confirmPassword"]', "password123");
    await page.check('[name="terms"]');
    await page.click('button[type="submit"]');
    
    await expect(page.locator("text=Email already exists")).toBeVisible();
  });
});
```

### Testing Role-Based Access

```typescript
// tests/rbac.spec.ts
import { test, expect } from "@playwright/test";

test.describe("Role-Based Access", () => {
  test("regular user cannot access admin panel", async ({ page }) => {
    // Login as regular user
    await page.goto("/login");
    await page.fill('[name="email"]', "user@example.com");
    await page.fill('[name="password"]', "password123");
    await page.click('button[type="submit"]');
    
    // Try to access admin
    await page.goto("/admin");
    
    // Should be redirected or see forbidden message
    await expect(page.locator("text=Access Denied")).toBeVisible();
  });
  
  test("admin can access admin panel", async ({ page }) => {
    // Login as admin
    await page.goto("/login");
    await page.fill('[name="email"]', "admin@example.com");
    await page.fill('[name="password"]', "adminpassword123");
    await page.click('button[type="submit"]');
    
    // Access admin
    await page.goto("/admin");
    
    // Should see admin panel
    await expect(page.locator("text=Admin Panel")).toBeVisible();
    await expect(page.locator("text=User Management")).toBeVisible();
  });
});
```

## Using Storage State for Auth

```typescript
// playwright.config.ts
import { defineConfig } from "@playwright/test";

export default defineConfig({
  use: {
    // Use saved authentication state
    storageState: "./playwright/.auth/user.json",
  },
});
```

```typescript
// tests/auth.setup.ts
import { test as setup } from "@playwright/test";

setup("authenticate as regular user", async ({ page }) => {
  await page.goto("/login");
  await page.fill('[name="email"]', "user@example.com");
  await page.fill('[name="password"]', "password123");
  await page.click('button[type="submit"]');
  
  // Save state
  await page.context().storageState({ 
    path: "./playwright/.auth/user.json" 
  });
});
```

## Common Mistakes

### Mistake 1: Not Waiting for Redirect After Login

```typescript
// WRONG - Immediately checking
await page.click('button[type="submit"]');
expect(page.url()).toContain("dashboard"); // Might not redirect yet!

// CORRECT - Wait for redirect
await page.click('button[type="submit"]');
await expect(page).toHaveURL(/.*dashboard/);
```

### Mistake 2: Hardcoding Credentials in Tests

```typescript
// WRONG - Credentials in test file
await page.fill('[name="email"]', "admin@test.com");
await page.fill('[name="password"]', "secret123");

// CORRECT - Use environment variables
await page.fill('[name="email"]', process.env.TEST_EMAIL);
await page.fill('[name="password"]', process.env.TEST_PASSWORD);
```

### Mistake 3: Testing Auth State Without Persistence Check

```typescript
// WRONG - Not checking if login persists
await login(page);
await page.goto("/dashboard");
// Doesn't verify persistence!

// CORRECT - Reload and verify
await login(page);
await page.reload();
await expect(page.locator("text=Welcome")).toBeVisible();
```

## Summary

- Test login, logout, and registration flows
- Verify protected routes redirect unauthenticated users
- Check that sessions persist across page reloads
- Test role-based access control
- Use storage state for efficient authenticated tests
- Use environment variables for credentials

## Next Steps

- [running-lighthouse.md](../21-performance-auditing/01-lighthouse/running-lighthouse.md) - Performance testing
- [adding-tests.md](../23-fullstack-project/02-building-the-app/adding-tests.md) - Full project testing
