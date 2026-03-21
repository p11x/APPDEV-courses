# TypeScript + React Cheatsheet

## Overview
A dense reference for TypeScript patterns in React applications. Covers component typing, event handlers, utility types, and common patterns.

---

## Component Prop Patterns

### 1. Basic Props

```tsx
interface ButtonProps {
  label: string;
  onClick: () => void;
}

function Button({ label, onClick }: ButtonProps) {
  return <button onClick={onClick}>{label}</button>;
}
```

### 2. Props with Optional Fields

```tsx
interface InputProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;  // Optional
  disabled?: boolean;
  className?: string;
}
```

### 3. Props with Children

```tsx
interface CardProps {
  children: React.ReactNode;
  title?: string;
}

function Card({ children, title }: CardProps) {
  return (
    <div>
      {title && <h2>{title}</h2>}
      {children}
    </div>
  );
}
```

### 4. Props with Generic

```tsx
interface ListProps<T> {
  items: T[];
  renderItem: (item: T) => React.ReactNode;
  keyExtractor: (item: T) => string;
}

function List<T>({ items, renderItem, keyExtractor }: ListProps<T>) {
  return (
    <ul>
      {items.map(item => (
        <li key={keyExtractor(item)}>{renderItem(item)}</li>
      ))}
    </ul>
  );
}

// Usage
<List 
  items={users} 
  renderItem={user => <span>{user.name}</span>}
  keyExtractor={user => user.id}
/>
```

### 5. Polymorphic Components

```tsx
type PolymorphicRef<C extends React.ElementType> = 
  React.ComponentPropsWithRef<C>['ref'];

interface BoxProps<T extends React.ElementType = 'div'> {
  as?: T;
  children: React.ReactNode;
}

function Box <T extends React.ElementType = 'div'>({
  as,
  children,
  ...props
}: BoxProps<T> & Omit<React.ComponentPropsWithoutRef<T>, keyof BoxProps<T>>) {
  const Component = as || 'div';
  return <Component {...props}>{children}</Component>;
}

// Usage
<Box as="section" className="section">Content</Box>
<Box as="article">Content</Box>
```

### 6. Forward Ref

```tsx
interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {}

export const Input = React.forwardRef<HTMLInputElement, InputProps>(
  (props, ref) => {
    return <input ref={ref} {...props} />;
  }
);
Input.displayName = 'Input';
```

---

## Event Handler Types

### Common Event Types

```tsx
// Mouse events
onClick: React.MouseEventHandler<HTMLButtonElement>
onDoubleClick: React.MouseEventHandler<HTMLDivElement>
onMouseEnter: React.MouseEventHandler<HTMLDivElement>
onMouseLeave: React.MouseEventHandler<HTMLDivElement>

// Form events
onChange: React.ChangeEventHandler<HTMLInputElement>
onSubmit: React.FormEventHandler<HTMLFormElement>
onFocus: React.FocusEventHandler<HTMLInputElement>
onBlur: React.FocusEventHandler<HTMLInputElement>

// Keyboard events
onKeyDown: React.KeyboardEventHandler<HTMLInputElement>
onKeyUp: React.KeyboardEventHandler<HTMLInputElement>

// Clipboard events
onCopy: React.ClipboardEventHandler<HTMLInputElement>
onPaste: React.ClipboardEventHandler<HTMLInputElement>

// Drag events
onDrag: React.DragEventHandler<HTMLDivElement>
onDrop: React.DragEventHandler<HTMLDivElement>
```

### Using Event Handlers

```tsx
// Input change handler
const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  setValue(e.target.value);
};

// Form submit handler
const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
  e.preventDefault();
  console.log(formData);
};

// Getting value from different elements
const handleDivClick = (e: React.MouseEvent<HTMLDivElement>) => {
  const target = e.currentTarget;  // The div element
  const dataset = target.dataset;  // data-* attributes
};
```

---

## Utility Types in React

### Partial<T> - Make all properties optional

```tsx
interface User {
  id: string;
  name: string;
  email: string;
}

type PartialUser = Partial<User>;
// { id?: string; name?: string; email?: string; }
```

### Required<T> - Make all properties required

```tsx
interface Config {
  apiUrl?: string;
}

type RequiredConfig = Required<Config>;
// { apiUrl: string }
```

### Pick<T, K> - Select specific properties

```tsx
interface User {
  id: string;
  name: string;
  email: string;
  password: string;
}

type UserPreview = Pick<User, 'id' | 'name'>;
// { id: string; name: string; }
```

### Omit<T, K> - Exclude specific properties

```tsx
interface User {
  id: string;
  name: string;
  password: string;
}

type PublicUser = Omit<User, 'password'>;
// { id: string; name: string; }
```

### ReturnType<T> - Get function's return type

```tsx
function getUser() {
  return { id: '1', name: 'John' };
}

type UserReturn = ReturnType<typeof getUser>;
// { id: string; name: string }
```

### Parameters<T> - Get function's parameter types

```tsx
function createUser(name: string, age: number) {
  return { name, age };
}

type CreateUserParams = Parameters<typeof createUser>;
// [string, number]
```

### ComponentProps - Get component's prop types

```tsx
type ButtonProps = React.ComponentProps<typeof Button>;
// { label: string; onClick: () => void; ... }
```

### HTMLAttributes<T> - HTML element attributes

```tsx
interface CustomInputProps extends React.HTMLAttributes<HTMLInputElement> {
  // Your custom props
  label: string;
}
// Automatically includes: className, onChange, disabled, ...
```

---

## Generic Component Patterns

### Generic List Component

```tsx
interface ListProps<TItem> {
  items: TItem[];
  onItemClick?: (item: TItem) => void;
}

function List<TItem>({ items, onItemClick }: ListProps<TItem>) {
  return (
    <ul>
      {items.map((item, index) => (
        <li key={index} onClick={() => onItemClick?.(item)}>
          {JSON.stringify(item)}
        </li>
      ))}
    </ul>
  );
}
```

### Generic API Hook

```tsx
function useFetch<T>(url: string) {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    fetch(url)
      .then(r => r.json())
      .then((data: T) => {
        setData(data);
        setLoading(false);
      });
  }, [url]);
  
  return { data, loading };
}

// Usage
const { data: users } = useFetch<User[]>('/api/users');
```

---

## Context Typing Pattern

### Creating Typed Context

```tsx
// Factory function for type-safe context
function createTypedContext<T>() {
  const context = React.createContext<T | undefined>(undefined);
  
  function useContext() {
    const value = React.useContext(context);
    if (value === undefined) {
      throw new Error('useContext must be used within Provider');
    }
    return value;
  }
  
  return [context.Provider, useContext] as const;
}

// Usage
const [ThemeProvider, useTheme] = createTypedContext<{
  theme: 'light' | 'dark';
  toggleTheme: () => void;
}>();

function App() {
  return (
    <ThemeProvider value={{ theme: 'light', toggleTheme: () => {} }}>
      <MyComponent />
    </ThemeProvider>
  );
}

function MyComponent() {
  const { theme } = useTheme(); // Fully typed!
}
```

---

## useRef Typing

### Three Cases

```tsx
// 1. Mutable ref (any value)
const countRef = useRef<number>(0);
countRef.current = 5;

// 2. DOM reference (element type)
const inputRef = useRef<HTMLInputElement>(null);
inputRef.current?.focus();

// 3. Read-only DOM reference (when you don't need to set value)
const buttonRef = useRef<HTMLButtonElement>(null);
```

---

## Type Assertion vs Guard vs Satisfies

### Type Assertion (as)

```tsx
// Tell TypeScript what type something is
const value = something as string;
const element = document.getElementById('app') as HTMLElement;
```

**When to use:** When you know more than TypeScript can infer.

### Type Guard

```tsx
// Narrow type based on runtime check
function isString(value: unknown): value is string {
  return typeof value === 'string';
}

function process(value: unknown) {
  if (isString(value)) {
    // TypeScript knows value is string here
    console.log(value.toUpperCase());
  }
}
```

### Satisfies Operator

```tsx
// Validate type while preserving inference

const config = {
  port: 3000,
  host: 'localhost',
} satisfies Record<string, string | number>;

// TypeScript knows exact values, not just string | number
config.port.toFixed(0); // ✓ Works
config.host.toUpperCase(); // ✓ Works
```

---

## tsconfig.json Strict Mode Flags

| Flag | What it Catches |
|------|-----------------|
| `strict: true` | Enables all strict flags below |
| `noImplicitAny` | Implicit `any` types |
| `strictNullChecks` | Null/undefined safety |
| `strictFunctionTypes` | Function parameter bivariance |
| `strictPropertyInitialization` | Uninitialized properties |
| `noImplicitReturns` | Missing return statements |
| `noImplicitThis` | Implicit `this` types |
| `alwaysStrict` | "use strict" in output |

---

## Quick Reference Table

| Pattern | Syntax |
|---------|--------|
| Optional props | `name?: string` |
| Children | `children: React.ReactNode` |
| Event handler | `onClick: React.MouseEventHandler` |
| Generic component | `<T>(props: Props<T>)` |
| Forward ref | `React.forwardRef<T, P>` |
| Partial props | `Partial<Props>` |
| Omit password | `Omit<User, 'password'>` |
| Context type | `createContext<T \| undefined>` |
| Mutable ref | `useRef<number>(0)` |
| DOM ref | `useRef<HTMLInputElement>(null)` |

---

## What's Next

- [Commands Cheatsheet](03-commands-cheatsheet.md) - CLI commands
- [Hooks Cheatsheet](01-hooks-cheatsheet.md) - React hooks
