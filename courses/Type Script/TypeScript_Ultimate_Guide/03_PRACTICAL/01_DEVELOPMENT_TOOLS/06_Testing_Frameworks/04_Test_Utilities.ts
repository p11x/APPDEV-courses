/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Testing_Frameworks
 * Purpose: Test utilities for TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * Test Utilities - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Utilities for testing
 * 💡 WHERE: Test helpers and fixtures
 * 🔧 HOW: Fixtures, helpers, matchers
 */

// ============================================================================
// SECTION 1: TEST FIXTURES
// ============================================================================

// Example 1.1: Fixture Factory
// ---------------------------------

function createUser(overrides?: Partial<User>): User {
  return {
    id: 1,
    name: "Test User",
    email: "test@example.com",
    isActive: true,
    createdAt: new Date(),
    ...overrides
  };
}

interface User {
  id: number;
  name: string;
  email: string;
  isActive: boolean;
  createdAt: Date;
}

// ============================================================================
// SECTION 2: TEST HELPERS
// ============================================================================

// Example 2.1: Async Test Helper
// ---------------------------------

async function wait(ms: number): Promise<void> {
  return new Promise(resolve => setTimeout(resolve, ms));
}

async function eventually(
  fn: () => Promise<void>,
  timeout = 5000
): Promise<void> {
  const start = Date.now();
  while (Date.now() - start < timeout) {
    try {
      await fn();
      return;
    } catch {
      await wait(100);
    }
  }
  throw new Error("Timeout waiting for condition");
}

// ============================================================================
// SECTION 3: MOCK FACTORY
// ============================================================================

// Example 3.1: Mock Factory
// ---------------------------------

function createMock<T>(overrides?: Partial<T>): jest.Mocked<T> {
  return {
    ...{} as T,
    ...overrides
  };
}

console.log("\n=== Test Utilities Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/06_Testing_Frameworks/05_Mocking_Strategies.ts");