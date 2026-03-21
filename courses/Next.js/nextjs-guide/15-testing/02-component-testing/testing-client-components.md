# Testing Client Components

## What You'll Learn
- Testing interactive Client Components
- Testing form submissions
- Testing component state
- Handling async operations

## Prerequisites
- Understanding of React Testing Library
- Knowledge of Client Component directives
- Familiarity with state testing

## Concept Explained Simply

Client Components are the interactive parts of your app — buttons that respond to clicks, forms that validate input, dropdowns that open and close. Testing them means simulating how a real user would interact: clicking buttons, typing in fields, and checking that things change on screen.

Think of it like a puppet show: you're the puppeteer pulling strings to make the puppet (component) move, and you're also the audience checking that the puppet does what it's supposed to do.

## Complete Code Example

### Interactive Component

```typescript
// components/TodoList.tsx
"use client";

import { useState } from "react";

interface Todo {
  id: number;
  text: string;
  completed: boolean;
}

export default function TodoList() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [input, setInput] = useState("");
  
  const addTodo = () => {
    if (!input.trim()) return;
    
    setTodos([
      ...todos,
      { id: Date.now(), text: input, completed: false }
    ]);
    setInput("");
  };
  
  const toggleTodo = (id: number) => {
    setTodos(todos.map(todo =>
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  };
  
  const deleteTodo = (id: number) => {
    setTodos(todos.filter(todo => todo.id !== id));
  };
  
  return (
    <div>
      <h1>Todo List</h1>
      
      <div>
        <input
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="Add a todo..."
          data-testid="todo-input"
        />
        <button 
          onClick={addTodo}
          data-testid="add-button"
        >
          Add
        </button>
      </div>
      
      <ul data-testid="todo-list">
        {todos.map(todo => (
          <li key={todo.id} className={todo.completed ? "completed" : ""}>
            <input
              type="checkbox"
              checked={todo.completed}
              onChange={() => toggleTodo(todo.id)}
              data-testid={`checkbox-${todo.id}`}
            />
            <span>{todo.text}</span>
            <button
              onClick={() => deleteTodo(todo.id)}
              data-testid={`delete-${todo.id}`}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
      
      {todos.length === 0 && (
        <p data-testid="empty-message">No todos yet!</p>
      )}
    </div>
  );
}
```

### Testing the Component

```typescript
// __tests__/components/TodoList.test.tsx
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import "@testing-library/jest-dom";
import TodoList from "../../components/TodoList";

describe("TodoList Component", () => {
  it("renders empty state initially", () => {
    render(<TodoList />);
    
    expect(screen.getByTestId("empty-message")).toHaveTextContent("No todos yet!");
  });
  
  it("adds a new todo", () => {
    render(<TodoList />);
    
    // Type in input
    const input = screen.getByTestId("todo-input");
    fireEvent.change(input, { target: { value: "Learn testing" } });
    
    // Click add button
    const addButton = screen.getByTestId("add-button");
    fireEvent.click(addButton);
    
    // Check todo was added
    expect(screen.getByText("Learn testing")).toBeInTheDocument();
    expect(screen.getByTestId("empty-message")).not.toBeInTheDocument();
  });
  
  it("toggles todo completion", () => {
    render(<TodoList />);
    
    // Add todo
    fireEvent.change(screen.getByTestId("todo-input"), { target: { value: "Test" } });
    fireEvent.click(screen.getByTestId("add-button"));
    
    // Get the checkbox
    const checkbox = screen.getByTestId("checkbox-1");
    
    // Toggle
    fireEvent.click(checkbox);
    
    // Check it's marked complete
    const todoItem = screen.getByText("Test").closest("li");
    expect(todoItem).toHaveClass("completed");
  });
  
  it("deletes a todo", () => {
    render(<TodoList />);
    
    // Add todo
    fireEvent.change(screen.getByTestId("todo-input"), { target: { value: "To delete" } });
    fireEvent.click(screen.getByTestId("add-button"));
    
    // Delete
    fireEvent.click(screen.getByTestId("delete-1"));
    
    // Check it's gone
    expect(screen.queryByText("To delete")).not.toBeInTheDocument();
    expect(screen.getByTestId("empty-message")).toBeInTheDocument();
  });
});
```

### Form Component

```typescript
// components/ContactForm.tsx
"use client";

import { useState } from "react";

interface FormErrors {
  name?: string;
  email?: string;
  message?: string;
}

export default function ContactForm() {
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    message: ""
  });
  const [errors, setErrors] = useState<FormErrors>({});
  const [isSubmitted, setIsSubmitted] = useState(false);
  
  const validate = (): boolean => {
    const newErrors: FormErrors = {};
    
    if (!formData.name.trim()) {
      newErrors.name = "Name is required";
    }
    
    if (!formData.email.trim()) {
      newErrors.email = "Email is required";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = "Invalid email format";
    }
    
    if (!formData.message.trim()) {
      newErrors.message = "Message is required";
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };
  
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (validate()) {
      setIsSubmitted(true);
    }
  };
  
  if (isSubmitted) {
    return (
      <div data-testid="success-message">
        <p>Thank you for your message!</p>
      </div>
    );
  }
  
  return (
    <form onSubmit={handleSubmit} data-testid="contact-form">
      <div>
        <label htmlFor="name">Name</label>
        <input
          id="name"
          value={formData.name}
          onChange={(e) => setFormData({ ...formData, name: e.target.value })}
          data-testid="name-input"
        />
        {errors.name && <span role="alert">{errors.name}</span>}
      </div>
      
      <div>
        <label htmlFor="email">Email</label>
        <input
          id="email"
          type="email"
          value={formData.email}
          onChange={(e) => setFormData({ ...formData, email: e.target.value })}
          data-testid="email-input"
        />
        {errors.email && <span role="alert">{errors.email}</span>}
      </div>
      
      <div>
        <label htmlFor="message">Message</label>
        <textarea
          id="message"
          value={formData.message}
          onChange={(e) => setFormData({ ...formData, message: e.target.value })}
          data-testid="message-input"
        />
        {errors.message && <span role="alert">{errors.message}</span>}
      </div>
      
      <button type="submit" data-testid="submit-button">
        Send
      </button>
    </form>
  );
}
```

```typescript
// __tests__/components/ContactForm.test.tsx
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import "@testing-library/jest-dom";
import ContactForm from "../../components/ContactForm";

describe("ContactForm Component", () => {
  it("renders form fields", () => {
    render(<ContactForm />);
    
    expect(screen.getByTestId("name-input")).toBeInTheDocument();
    expect(screen.getByTestId("email-input")).toBeInTheDocument();
    expect(screen.getByTestId("message-input")).toBeInTheDocument();
  });
  
  it("shows validation errors on empty submit", () => {
    render(<ContactForm />);
    
    fireEvent.click(screen.getByTestId("submit-button"));
    
    expect(screen.getByRole("alert", { name: /name/i })).toBeInTheDocument();
    expect(screen.getByRole("alert", { name: /email/i })).toBeInTheDocument();
  });
  
  it("shows email format error", () => {
    render(<ContactForm />);
    
    fireEvent.change(screen.getByTestId("name-input"), { target: { value: "John" } });
    fireEvent.change(screen.getByTestId("email-input"), { target: { value: "not-email" } });
    fireEvent.change(screen.getByTestId("message-input"), { target: { value: "Hello" } });
    
    fireEvent.click(screen.getByTestId("submit-button"));
    
    expect(screen.getByRole("alert")).toHaveTextContent("Invalid email format");
  });
  
  it("submits successfully with valid data", () => {
    render(<ContactForm />);
    
    fireEvent.change(screen.getByTestId("name-input"), { target: { value: "John" } });
    fireEvent.change(screen.getByTestId("email-input"), { target: { value: "john@example.com" } });
    fireEvent.change(screen.getByTestId("message-input"), { target: { value: "Hello!" } });
    
    fireEvent.click(screen.getByTestId("submit-button"));
    
    expect(screen.getByTestId("success-message")).toBeInTheDocument();
  });
});
```

## Testing Async Operations

```typescript
// components/DataFetcher.tsx
"use client";

import { useState, useEffect } from "react";

interface User {
  id: number;
  name: string;
}

export default function DataFetcher() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  useEffect(() => {
    fetch("/api/users")
      .then(res => {
        if (!res.ok) throw new Error("Failed to fetch");
        return res.json();
      })
      .then(data => {
        setUsers(data);
        setLoading(false);
      })
      .catch(err => {
        setError(err.message);
        setLoading(false);
      });
  }, []);
  
  if (loading) return <div data-testid="loading">Loading...</div>;
  if (error) return <div data-testid="error">{error}</div>;
  
  return (
    <ul data-testid="user-list">
      {users.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

```typescript
// __tests__/components/DataFetcher.test.tsx
import { render, screen, waitFor } from "@testing-library/react";
import "@testing-library/jest-dom";
import DataFetcher from "../../components/DataFetcher";

// Mock fetch globally
global.fetch = jest.fn();

describe("DataFetcher Component", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });
  
  it("shows loading state initially", () => {
    // Mock pending fetch
    (fetch as jest.Mock).mockImplementation(() => 
      new Promise(() => {}) // Never resolves
    );
    
    render(<DataFetcher />);
    
    expect(screen.getByTestId("loading")).toBeInTheDocument();
  });
  
  it("shows users after successful fetch", async () => {
    const mockUsers = [
      { id: 1, name: "Alice" },
      { id: 2, name: "Bob" },
    ];
    
    (fetch as jest.Mock).mockResolvedValue({
      ok: true,
      json: () => Promise.resolve(mockUsers),
    });
    
    render(<DataFetcher />);
    
    await waitFor(() => {
      expect(screen.getByTestId("loading")).not.toBeInTheDocument();
    });
    
    expect(screen.getByText("Alice")).toBeInTheDocument();
    expect(screen.getByText("Bob")).toBeInTheDocument();
  });
  
  it("shows error on failed fetch", async () => {
    (fetch as jest.Mock).mockResolvedValue({
      ok: false,
    });
    
    render(<DataFetcher />);
    
    await waitFor(() => {
      expect(screen.getByTestId("error")).toBeInTheDocument();
    });
  });
});
```

## Summary

- Test Client Components by simulating user interactions
- Use fireEvent to trigger clicks, changes, and form submissions
- Check component state changes after interactions
- Test validation logic and error messages
- Use waitFor when dealing with async state changes
- Use data-testid for elements that are hard to query

## Next Steps

- [playwright-setup.md](../03-e2e-with-playwright/playwright-setup.md) - Setting up Playwright
- [writing-e2e-tests.md](../03-e2e-with-playwright/writing-e2e-tests.md) - E2E testing
