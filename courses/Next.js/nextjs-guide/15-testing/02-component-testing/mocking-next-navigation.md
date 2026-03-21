# Mocking Next.js Navigation

## What You'll Learn
- Mocking Next.js router in tests
- Testing navigation functions
- Handling useRouter and usePathname
- Testing redirects

## Prerequisites
- Understanding of React Testing Library
- Knowledge of Next.js navigation
- Familiarity with Jest mocks

## Concept Explained Simply

When you test components that use Next.js navigation (`useRouter`, `usePathname`, `useSearchParams`), you need to mock these functions. Otherwise, your tests will try to actually navigate, which won't work in a test environment.

Think of it like testing a car: you don't want it to actually drive down the street while you test the dashboard. You mount it on a test stand and simulate the engine. That's what mocking does for navigation.

## Complete Code Example

### Component Using Navigation

```typescript
// components/Navigation.tsx
"use client";

import Link from "next/link";
import { useRouter, usePathname } from "next/navigation";

export default function Navigation() {
  const router = useRouter();
  const pathname = usePathname();
  
  const navigateToHome = () => {
    router.push("/");
  };
  
  const navigateToDashboard = () => {
    router.push("/dashboard");
  };
  
  return (
    <nav>
      <ul>
        <li className={pathname === "/" ? "active" : ""}>
          <Link href="/">Home</Link>
        </li>
        <li className={pathname === "/dashboard" ? "active" : ""}>
          <Link href="/dashboard">Dashboard</Link>
        </li>
        <li className={pathname === "/settings" ? "active" : ""}>
          <Link href="/settings">Settings</Link>
        </li>
      </ul>
      
      <button onClick={navigateToHome}>Go Home</button>
      <button onClick={navigateToDashboard}>Go Dashboard</button>
    </nav>
  );
}
```

### Setting Up Mocks

```typescript
// jest.setup.ts (global setup)
import { jest } from "@jest/globals";

// Mock useRouter
jest.mock("next/navigation", () => ({
  useRouter: () => ({
    push: jest.fn(),
    replace: jest.fn(),
    prefetch: jest.fn(),
    back: jest.fn(),
    forward: jest.fn(),
    refresh: jest.fn(),
  }),
  usePathname: () => "/",
  useSearchParams: () => new URLSearchParams(),
  useParams: () => ({}),
}));
```

### Testing Navigation Components

```typescript
// __tests__/components/Navigation.test.tsx
import { render, screen, fireEvent } from "@testing-library/react";
import "@testing-library/jest-dom";
import Navigation from "../../components/Navigation";

describe("Navigation Component", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });
  
  it("renders navigation links", () => {
    render(<Navigation />);
    
    expect(screen.getByText("Home")).toBeInTheDocument();
    expect(screen.getByText("Dashboard")).toBeInTheDocument();
    expect(screen.getByText("Settings")).toBeInTheDocument();
  });
  
  it("calls router.push when button clicked", () => {
    const router = require("next/navigation").useRouter;
    const pushMock = jest.fn();
    router.mockReturnValue({ push: pushMock });
    
    render(<Navigation />);
    
    fireEvent.click(screen.getByText("Go Home"));
    
    expect(pushMock).toHaveBeenCalledWith("/");
  });
});
```

### Testing with Different Paths

```typescript
// __tests__/components/Navigation.paths.test.tsx
import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom";
import Navigation from "../../components/Navigation";

jest.mock("next/navigation", () => {
  let currentPath = "/";
  
  return {
    useRouter: () => ({
      push: jest.fn((path) => { currentPath = path; }),
      replace: jest.fn(),
      back: jest.fn(),
    }),
    usePathname: () => currentPath,
    useSearchParams: () => new URLSearchParams(),
  };
};

describe("Navigation Component - Path Awareness", () => {
  it("highlights home link when on home page", () => {
    // Reset to home path
    const pathname = require("next/navigation").usePathname;
    // This tests the active state based on pathname
  });
});
```

### Testing Search Params

```typescript
// components/SearchResults.tsx
"use client";

import { useSearchParams } from "next/navigation";

export default function SearchResults() {
  const searchParams = useSearchParams();
  const query = searchParams.get("q") || "";
  
  return (
    <div>
      <h1>Search Results</h1>
      {query ? (
        <p>Showing results for: "{query}"</p>
      ) : (
        <p>Enter a search term</p>
      )}
    </div>
  );
}
```

```typescript
// __tests__/components/SearchResults.test.tsx
import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom";
import SearchResults from "../../components/SearchResults";

// Mock with specific search params
const mockSearchParams = new URLSearchParams("q=hello");

jest.mock("next/navigation", () => ({
  useRouter: () => ({
    push: jest.fn(),
  }),
  usePathname: () => "/search",
  useSearchParams: () => mockSearchParams,
}));

describe("SearchResults Component", () => {
  it("displays search query", () => {
    render(<SearchResults />);
    
    expect(screen.getByText(/results for.*hello/i)).toBeInTheDocument();
  });
  
  it("shows enter term message when no query", () => {
    // Mock with empty params
    const emptyParams = new URLSearchParams();
    jest.doMock("next/navigation", () => ({
      useRouter: () => ({ push: jest.fn() }),
      usePathname: () => "/search",
      useSearchParams: () => emptyParams,
    }));
    
    render(<SearchResults />);
    
    expect(screen.getByText("Enter a search term")).toBeInTheDocument();
  });
});
```

### Testing Link Component

```typescript
// components/LinkExample.tsx
"use client";

import Link from "next/link";

export default function LinkExample() {
  return (
    <div>
      <Link href="/page1">Page 1</Link>
      <Link href="/page2" replace>Page 2 (replace)</Link>
      <Link href="/page3" prefetch={false}>Page 3 (no prefetch)</Link>
    </div>
  );
}
```

```typescript
// __tests__/components/LinkExample.test.tsx
import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom";
import LinkExample from "../../components/LinkExample";

describe("LinkExample Component", () => {
  it("renders Link components", () => {
    render(<LinkExample />);
    
    // Links are rendered as <a> tags
    expect(screen.getByText("Page 1")).toBeInTheDocument();
    expect(screen.getByText("Page 2 (replace)")).toBeInTheDocument();
    expect(screen.getByText("Page 3 (no prefetch)")).toBeInTheDocument();
    
    // Check href attributes
    expect(screen.getByText("Page 1")).toHaveAttribute("href", "/page1");
  });
});
```

## Common Mistakes

### Mistake 1: Not Clearing Router Mocks

```typescript
// WRONG - Router state persists between tests
test("test 1", () => {
  // mock called once
});

test("test 2", () => {
  // mock still has old calls!
});

// CORRECT - Clear in beforeEach
beforeEach(() => {
  jest.clearAllMocks();
});
```

### Mistake 2: Trying to Test Actual Navigation

```typescript
// WRONG - This will navigate away from test!
test("navigates", () => {
  render(<Component />);
  fireEvent.click(screen.getByText("Go to Page"));
  // Now test is on different page - broken!
});

// CORRECT - Mock the router
const pushMock = jest.fn();
jest.spyOn(require("next/navigation"), "useRouter")
  .mockReturnValue({ push: pushMock });

test("navigates", () => {
  render(<Component />);
  fireEvent.click(screen.getByText("Go to Page"));
  expect(pushMock).toHaveBeenCalledWith("/page");
});
```

### Mistake 3: Forgetting Mock Return Value Structure

```typescript
// WRONG - Incomplete mock
jest.mock("next/navigation", () => ({
  useRouter: () => ({}), // Missing methods!
}));

// CORRECT - Complete mock
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
```

## Summary

- Mock `next/navigation` to test components using navigation hooks
- Provide all router methods in the mock (push, replace, etc.)
- Use jest.clearAllMocks() in beforeEach to reset between tests
- Mock usePathname to test active link highlighting
- Mock useSearchParams to test query string handling
- Test Link components by checking rendered href attributes

## Next Steps

- [testing-client-components.md](./testing-client-components.md) - More client component testing
- [writing-e2e-tests.md](../03-e2e-with-playwright/writing-e2e-tests.md) - End-to-end testing
