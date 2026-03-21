# Testing Server Components

## What You'll Learn
- Testing Server Components
- Mocking data fetching
- Testing async components
- Best practices for server testing

## Prerequisites
- Understanding of Jest setup
- Knowledge of Server Components
- Familiarity with async/await

## Concept Explained Simply

Server Components run on the server, which makes them a bit tricky to test — you can't render them directly in a browser-like environment. However, you can test them by treating them as regular async functions that return JSX. The key is mocking the data they fetch so you can control what they return.

Think of testing Server Components like testing a recipe: you can test the cooking process without actually cooking by using mock ingredients. Similarly, you mock the database/API calls to test the component logic.

## Complete Code Example

### Sample Server Component

```typescript
// app/users/page.tsx
import { db } from "@/lib/db";

interface User {
  id: string;
  name: string;
  email: string;
}

async function getUsers(): Promise<User[]> {
  return db.user.findMany();
}

async function getUserCount(): Promise<number> {
  return db.user.count();
}

export default async function UsersPage() {
  const [users, count] = await Promise.all([
    getUsers(),
    getUserCount(),
  ]);
  
  return (
    <div>
      <h1>Users ({count})</h1>
      <ul>
        {users.map((user) => (
          <li key={user.id}>{user.name} - {user.email}</li>
        ))}
      </ul>
    </div>
  );
}
```

### Testing Server Component

```typescript
// __tests__/app/users/page.test.tsx
import { renderToString } from "react-dom/server";

// Mock the database
jest.mock("@/lib/db", () => ({
  db: {
    user: {
      findMany: jest.fn(),
      count: jest.fn(),
    },
  },
}));

import { db } from "@/lib/db";

// Import the component after mocking
// We need to test the async function, not render directly
import { getUsers, getUserCount } from "@/app/users/page";

describe("UsersPage Server Component", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });
  
  it("should fetch users from database", async () => {
    // Arrange: Mock database response
    const mockUsers = [
      { id: "1", name: "Alice", email: "alice@example.com" },
      { id: "2", name: "Bob", email: "bob@example.com" },
    ];
    
    (db.user.findMany as jest.Mock).mockResolvedValue(mockUsers);
    (db.user.count as jest.Mock).mockResolvedValue(2);
    
    // Act: Call the functions directly
    const users = await getUsers();
    const count = await getUserCount();
    
    // Assert: Check results
    expect(users).toEqual(mockUsers);
    expect(count).toBe(2);
    expect(db.user.findMany).toHaveBeenCalledTimes(1);
    expect(db.user.count).toHaveBeenCalledTimes(1);
  });
  
  it("should handle empty users", async () => {
    // Arrange
    (db.user.findMany as jest.Mock).mockResolvedValue([]);
    (db.user.count as jest.Mock).mockResolvedValue(0);
    
    // Act
    const users = await getUsers();
    const count = await getUserCount();
    
    // Assert
    expect(users).toEqual([]);
    expect(count).toBe(0);
  });
});
```

### Testing Server Component with Params

```typescript
// app/users/[id]/page.tsx
import { db } from "@/lib/db";

interface Props {
  params: Promise<{ id: string }>;
}

async function getUser(id: string) {
  const user = await db.user.findUnique({ where: { id } });
  if (!user) throw new Error("User not found");
  return user;
}

export default async function UserPage({ params }: Props) {
  const { id } = await params;
  const user = await getUser(id);
  
  return (
    <div>
      <h1>{user.name}</h1>
      <p>{user.email}</p>
    </div>
  );
}
```

```typescript
// __tests__/app/users/[id]/page.test.tsx

jest.mock("@/lib/db", () => ({
  db: {
    user: {
      findUnique: jest.fn(),
    },
  },
}));

import { db } from "@/lib/db";
import { getUser } from "@/app/users/[id]/page";

describe("UserPage Server Component", () => {
  it("should fetch user by id", async () => {
    const mockUser = { 
      id: "123", 
      name: "Alice", 
      email: "alice@example.com" 
    };
    
    (db.user.findUnique as jest.Mock).mockResolvedValue(mockUser);
    
    const user = await getUser("123");
    
    expect(user).toEqual(mockUser);
    expect(db.user.findUnique).toHaveBeenCalledWith({ where: { id: "123" } });
  });
  
  it("should throw error when user not found", async () => {
    (db.user.findUnique as jest.Mock).mockResolvedValue(null);
    
    await expect(getUser("999")).rejects.toThrow("User not found");
  });
});
```

## Testing Strategy for Server Components

### 1. Extract Logic to Testable Functions

```typescript
// Instead of testing the component directly,
// extract the data fetching logic

// app/page.tsx - Component
import { getData } from "./lib";

export default async function Page() {
  const data = await getData();
  return <div>{data.title}</div>;
}

// app/lib.ts - Testable functions
export async function getData() {
  // Your logic here
  return { title: "Hello" };
}

// __tests__/lib.test.ts - Test the logic
import { getData } from "./lib";

test("getData returns title", async () => {
  const data = await getData();
  expect(data.title).toBe("Hello");
});
```

### 2. Test Data Transformation

```typescript
// lib/transform.ts
export function transformUser(user: any) {
  return {
    id: user.id,
    displayName: user.name || user.email,
    initials: getInitials(user.name),
  };
}

function getInitials(name: string) {
  return name
    .split(" ")
    .map(n => n[0])
    .join("")
    .toUpperCase();
}

// __tests__/transform.test.ts
import { transformUser } from "../lib/transform";

describe("transformUser", () => {
  it("should create display name from name", () => {
    const user = transformUser({ id: "1", name: "Alice" });
    expect(user.displayName).toBe("Alice");
  });
  
  it("should create initials from name", () => {
    const user = transformUser({ id: "1", name: "Alice Smith" });
    expect(user.initials).toBe("AS");
  });
});
```

## Common Mistakes

### Mistake 1: Trying to Render Server Components Directly

```typescript
// WRONG - Can't render Server Components in jsdom
import { render } from "@testing-library/react";
import Page from "./page";

test("renders", () => {
  render(<Page />); // Won't work - it's async!
});
```

```typescript
// CORRECT - Test the async functions instead
import { getData } from "./page";
test("getData works", async () => {
  const data = await getData();
  expect(data).toBeDefined();
});
```

### Mistake 2: Not Mocking Async Operations

```typescript
// WRONG - Missing async handling
test("fetches data", () => {
  const data = getData(); // Returns a Promise!
  expect(data.title).toBe("Hello"); // Fails!
});

// CORRECT - Await the promise
test("fetches data", async () => {
  const data = await getData();
  expect(data.title).toBe("Hello");
});
```

### Mistake 3: Forgetting to Clear Mocks

```typescript
// WRONG - Mocks persist between tests
test("first call", () => {
  mockFn.mockReturnValue(1);
  expect(getData()).toBe(1);
});

test("second call", () => {
  // mockFn still returns 1!
});

// CORRECT - Clear mocks in beforeEach
beforeEach(() => {
  jest.clearAllMocks();
});
```

## Summary

- Server Components can't be rendered directly in Jest
- Extract and test the async data-fetching functions
- Mock database and API calls with jest.mock
- Test both success and error cases
- Use beforeEach to clear mocks between tests
- Consider testing data transformation separately from components

## Next Steps

- [testing-server-actions.md](./testing-server-actions.md) - Testing Server Actions
- [react-testing-library.md](../02-component-testing/react-testing-library.md) - Testing Client Components
