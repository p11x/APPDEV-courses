# Testing Server Actions

## What You'll Learn
- Testing Server Actions
- Mocking revalidation
- Testing form submissions
- Error handling in tests

## Prerequisites
- Understanding of Server Actions
- Knowledge of Jest setup
- Familiarity with async testing

## Concept Explained Simply

Server Actions are async functions that run on the server, so testing them is similar to testing any async function. You call them directly with test data and check the results. The key is making sure you mock any external dependencies like databases or external APIs.

Think of testing Server Actions like testing a restaurant recipe: you provide specific ingredients (test data), follow the cooking steps (call the action), and verify the result tastes right (check the output).

## Complete Code Example

### Sample Server Action

```typescript
// lib/actions.ts
"use server";

import { db } from "@/lib/db";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";

export async function createUser(formData: FormData) {
  const name = formData.get("name") as string;
  const email = formData.get("email") as string;
  
  // Validate input
  if (!name || !email) {
    return { error: "Name and email are required" };
  }
  
  // Check if user exists
  const existing = await db.user.findUnique({
    where: { email },
  });
  
  if (existing) {
    return { error: "User already exists" };
  }
  
  // Create user
  const user = await db.user.create({
    data: { name, email },
  });
  
  // Revalidate and redirect
  revalidatePath("/users");
  redirect(`/users/${user.id}`);
}

export async function updateUser(id: string, data: { name?: string; email?: string }) {
  const user = await db.user.update({
    where: { id },
    data,
  });
  
  revalidatePath(`/users/${id}`);
  return user;
}

export async function deleteUser(id: string) {
  await db.user.delete({
    where: { id },
  });
  
  revalidatePath("/users");
  return { success: true };
}
```

### Testing Server Actions

```typescript
// __tests__/lib/actions.test.ts

// Mock the database
jest.mock("@/lib/db", () => ({
  db: {
    user: {
      findUnique: jest.fn(),
      create: jest.fn(),
      update: jest.fn(),
      delete: jest.fn(),
    },
  },
}));

// Mock next/cache
jest.mock("next/cache", () => ({
  revalidatePath: jest.fn(),
}));

// Mock next/navigation
jest.mock("next/navigation", () => ({
  redirect: jest.fn(),
}));

import { db } from "@/lib/db";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";

import { createUser, updateUser, deleteUser } from "../lib/actions";

describe("Server Actions", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });
  
  describe("createUser", () => {
    it("should create a new user with valid data", async () => {
      // Arrange
      const mockUser = { id: "1", name: "Alice", email: "alice@example.com" };
      (db.user.findUnique as jest.Mock).mockResolvedValue(null);
      (db.user.create as jest.Mock).mockResolvedValue(mockUser);
      
      const formData = new FormData();
      formData.append("name", "Alice");
      formData.append("email", "alice@example.com");
      
      // Act
      const result = await createUser(formData);
      
      // Assert
      expect(db.user.findUnique).toHaveBeenCalledWith({
        where: { email: "alice@example.com" }
      });
      expect(db.user.create).toHaveBeenCalledWith({
        data: { name: "Alice", email: "alice@example.com" }
      });
      expect(revalidatePath).toHaveBeenCalledWith("/users");
      expect(redirect).toHaveBeenCalledWith("/users/1");
    });
    
    it("should return error for missing name", async () => {
      const formData = new FormData();
      formData.append("email", "alice@example.com");
      
      const result = await createUser(formData);
      
      expect(result).toEqual({ error: "Name and email are required" });
    });
    
    it("should return error if user already exists", async () => {
      // Arrange
      const existingUser = { id: "1", name: "Bob", email: "alice@example.com" };
      (db.user.findUnique as jest.Mock).mockResolvedValue(existingUser);
      
      const formData = new FormData();
      formData.append("name", "Alice");
      formData.append("email", "alice@example.com");
      
      // Act
      const result = await createUser(formData);
      
      // Assert
      expect(result).toEqual({ error: "User already exists" });
      expect(db.user.create).not.toHaveBeenCalled();
    });
  });
  
  describe("updateUser", () => {
    it("should update user and revalidate path", async () => {
      // Arrange
      const updatedUser = { id: "1", name: "Alice Updated", email: "alice@example.com" };
      (db.user.update as jest.Mock).mockResolvedValue(updatedUser);
      
      // Act
      const result = await updateUser("1", { name: "Alice Updated" });
      
      // Assert
      expect(db.user.update).toHaveBeenCalledWith({
        where: { id: "1" },
        data: { name: "Alice Updated" }
      });
      expect(revalidatePath).toHaveBeenCalledWith("/users/1");
      expect(result).toEqual(updatedUser);
    });
  });
  
  describe("deleteUser", () => {
    it("should delete user and revalidate path", async () => {
      // Arrange
      (db.user.delete as jest.Mock).mockResolvedValue({});
      
      // Act
      const result = await deleteUser("1");
      
      // Assert
      expect(db.user.delete).toHaveBeenCalledWith({
        where: { id: "1" }
      });
      expect(revalidatePath).toHaveBeenCalledWith("/users");
      expect(result).toEqual({ success: true });
    });
  });
});
```

### Testing with Validation (Zod)

```typescript
// lib/actions.ts - With Zod validation
"use server";

import { z } from "zod";
import { db } from "@/lib/db";

const userSchema = z.object({
  name: z.string().min(1, "Name is required"),
  email: z.string().email("Invalid email"),
  age: z.number().min(18).optional(),
});

export async function createUserValidated(data: unknown) {
  const result = userSchema.safeParse(data);
  
  if (!result.success) {
    return { 
      error: "Validation failed", 
      details: result.error.flatten() 
    };
  }
  
  const user = await db.user.create({
    data: result.data,
  });
  
  return { success: true, user };
}
```

```typescript
// __tests__/lib/actions-zod.test.ts

jest.mock("@/lib/db", () => ({
  db: {
    user: { create: jest.fn() },
  },
}));

import { db } from "@/lib/db";
import { createUserValidated } from "../lib/actions";

describe("createUserValidated", () => {
  it("should create user with valid data", async () => {
    const mockUser = { id: "1", name: "Alice", email: "alice@example.com" };
    (db.user.create as jest.Mock).mockResolvedValue(mockUser);
    
    const result = await createUserValidated({
      name: "Alice",
      email: "alice@example.com",
    });
    
    expect(result.success).toBe(true);
    expect(result.user).toEqual(mockUser);
  });
  
  it("should return error for invalid email", async () => {
    const result = await createUserValidated({
      name: "Alice",
      email: "not-an-email",
    });
    
    expect(result.error).toBe("Validation failed");
    expect(result.details).toBeDefined();
  });
  
  it("should return error for missing name", async () => {
    const result = await createUserValidated({
      email: "alice@example.com",
    });
    
    expect(result.error).toBe("Validation failed");
  });
});
```

## Common Mistakes

### Mistake 1: Not Mocking Dependencies

```typescript
// WRONG - Will try to connect to real database!
test("creates user", async () => {
  const formData = new FormData();
  formData.append("name", "Alice");
  
  await createUser(formData); // Hits real database!
});
```

```typescript
// CORRECT - Mock the database
jest.mock("@/lib/db", () => ({
  db: {
    user: { create: jest.fn() },
  },
}));
```

### Mistake 2: Forgetting to Clear Mocks

```typescript
// WRONG - State leaks between tests
test("first", () => {
  mockFn.mockReturnValue(1);
});
test("second", () => {
  // mockFn still has old return value!
});

// CORRECT - Clear in beforeEach
beforeEach(() => {
  jest.clearAllMocks();
});
```

### Mistake 3: Testing redirect() without handling it

```typescript
// WRONG - redirect() throws error
test("creates and redirects", async () => {
  const formData = new FormData();
  formData.append("name", "Alice");
  
  await createUser(formData); // Throws redirect error!
});
```

```typescript
// CORRECT - Mock redirect
jest.mock("next/navigation", () => ({
  redirect: jest.fn(),
}));

test("creates and redirects", async () => {
  // Now redirect() is mocked and won't throw
});
```

## Summary

- Test Server Actions like regular async functions
- Mock external dependencies (database, APIs)
- Mock `next/cache` revalidation functions
- Mock `next/navigation` redirect function
- Test both success and error cases
- Use beforeEach to clear mocks between tests

## Next Steps

- [react-testing-library.md](../02-component-testing/react-testing-library.md) - Testing Client Components
- [writing-e2e-tests.md](../03-e2e-with-playwright/writing-e2e-tests.md) - E2E testing
