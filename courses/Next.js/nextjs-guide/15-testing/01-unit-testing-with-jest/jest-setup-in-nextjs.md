# Jest Setup in Next.js

## What You'll Learn
- Setting up Jest for Next.js
- Configuring TypeScript support
- Writing your first test
- Understanding test structure

## Prerequisites
- Basic JavaScript/TypeScript knowledge
- Understanding of unit testing concepts
- A Next.js project

## Concept Explained Simply

Jest is like a robot that automatically checks if your code works correctly. You tell it what to test by writing "test cases" — small programs that verify specific behaviors. When you run Jest, it executes all your tests and tells you what passed and what failed.

Setting up Jest with Next.js lets you test your components, functions, and logic to make sure everything works as expected. This catches bugs before they reach your users.

## Complete Code Example

### Installation

```bash
# Install Jest and required packages
npm install --save-dev jest jest-environment-jsdom @testing-library/react @testing-library/jest-dom @types/jest ts-jest

# Or use the Next.js testing preset
npm install --save-dev @next/bundle-analyzer @testing-library/dom
```

### Configuration

```typescript
// jest.config.ts
import type { Config } from "jest";
import nextJest from "next/jest";

const createJestConfig = nextJest({
  // Provide the path to your Next.js app
  dir: "./",
});

// Add any custom config to be passed to Jest
const customJestConfig: Config = {
  // Setup files to run before each test
  setupFilesAfterEnv: ["<rootDir>/jest.setup.ts"],
  
  // Module name mapper for path aliases
  moduleNameMapper: {
    "^@/(.*)$": "<rootDir>/src/$1",
  },
  
  // Test environment
  testEnvironment: "jsdom",
  
  // Which files to test
  testMatch: [
    "**/__tests__/**/*.[jt]s?(x)",
    "**/?(*.)+(spec|test).[jt]s?(x)",
  ],
  
  // Collect coverage from these files
  collectCoverageFrom: [
    "src/**/*.{ts,tsx}",
    "!src/**/*.d.ts",
    "!src/**/*.stories.tsx",
  ],
};

export default createJestConfig(customJestConfig);
```

### Setup File

```typescript
// jest.setup.ts
import "@testing-library/jest-dom";

// Mock Next.js router
jest.mock("next/navigation", () => ({
  useRouter: () => ({
    push: jest.fn(),
    replace: jest.fn(),
    prefetch: jest.fn(),
    back: jest.fn(),
  }),
  usePathname: () => "/",
  useSearchParams: () => new URLSearchParams(),
}));

// Mock Next.js cookies
jest.mock("next/headers", () => ({
  cookies: () => ({
    get: jest.fn(),
    set: jest.fn(),
  }),
}));
```

### Package.json Scripts

```json
{
  "scripts": {
    "test": "jest",
    "test:watch": "jest --watch",
    "test:coverage": "jest --coverage"
  }
}
```

### Writing Your First Test

```typescript
// __tests__/hello.test.ts

// Simple function to test
export function greet(name: string): string {
  return `Hello, ${name}!`;
}

// Test case
describe("greet function", () => {
  it("should return a greeting with the name", () => {
    const result = greet("Alice");
    expect(result).toBe("Hello, Alice!");
  });
  
  it("should handle empty name", () => {
    const result = greet("");
    expect(result).toBe("Hello, !");
  });
});
```

### Running Tests

```bash
# Run all tests
npm test

# Run tests in watch mode
npm run test:watch

# Run tests with coverage
npm run test:coverage
```

## Test Structure

```typescript
// __tests__/example.test.ts

// describe: Groups related tests
describe("Feature being tested", () => {
  
  // it or test: Individual test case
  it("should do something specific", () => {
    // Arrange: Set up test data
    const input = "test";
    
    // Act: Execute the function
    const result = someFunction(input);
    
    // Assert: Check the result
    expect(result).toBe("expected output");
  });
  
  // Another test
  test("handles edge case", () => {
    // ...
  });
});
```

## Common Matchers

| Matcher | Description | Example |
|---------|-------------|---------|
| `toBe(value)` | Exact equality | `expect(2 + 2).toBe(4)` |
| `toEqual(object)` | Deep equality | `expect({a:1}).toEqual({a:1})` |
| `toBeTruthy()` | Is truthy | `expect("hello").toBeTruthy()` |
| `toBeFalsy()` | Is falsy | `expect(null).toBeFalsy()` |
| `toContain(item)` | Array contains | `expect([1,2,3]).toContain(2)` |
| `toThrow()` | Function throws | `expect(() => bad()).toThrow()` |

## Common Mistakes

### Mistake 1: Not Cleaning Up Between Tests

```typescript
// WRONG - Shared state between tests
let counter = 0;

test("increment", () => {
  counter++;
  expect(counter).toBe(1);
});

test("increment again", () => {
  counter++; // Counter is already 1!
  expect(counter).toBe(1); // Fails!
});

// CORRECT - Reset state in each test
let counter = 0;

test("increment", () => {
  counter++;
  expect(counter).toBe(1);
});

test("increment again", () => {
  counter = 0; // Reset first
  counter++;
  expect(counter).toBe(1);
});
```

### Mistake 2: Testing Implementation Instead of Behavior

```typescript
// WRONG - Testing implementation details
test("uses array", () => {
  const result = processData();
  expect(result.data).toBeInstanceOf(Array); // Too specific!
});

// CORRECT - Test the behavior/output
test("returns processed data", () => {
  const result = processData();
  expect(result.data).toEqual(["expected", "output"]);
});
```

### Mistake 3: Not Using Async/Await

```typescript
// WRONG - Not awaiting async code
test("fetches user", async () => {
  const user = await getUser(1);
  // This won't wait!
});

// CORRECT - Properly await
test("fetches user", async () => {
  const user = await getUser(1);
  expect(user.name).toBe("Alice");
});
```

## Summary

- Install Jest with `npm install --save-dev jest @testing-library/react`
- Create `jest.config.ts` with Next.js configuration
- Create `jest.setup.ts` for mocks and setup
- Use `describe()` to group tests and `it()` for individual cases
- Use `expect()` with matchers to make assertions
- Run tests with `npm test`

## Next Steps

- [testing-server-components.md](./testing-server-components.md) - Testing Server Components
- [testing-server-actions.md](./testing-server-actions.md) - Testing Server Actions
