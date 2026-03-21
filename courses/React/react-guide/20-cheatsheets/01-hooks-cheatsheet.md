# React Hooks Cheatsheet

## Overview
A dense, scannable reference for all React 18 hooks. Each hook includes its purpose, signature, minimal example, and common mistakes.

---

## Core Hooks

### useState

**Purpose:** Add state to functional components

```tsx
// Basic usage
const [state, setState] = useState(initialValue);

// With type
const [count, setCount] = useState<number>(0);

// Functional update (use when new state depends on old)
setCount(prev => prev + 1);
```

**Common Mistake:**
```tsx
// ❌ WRONG - Using stale closure value
const [count, setCount] = useState(0);
useEffect(() => {
  const timer = setInterval(() => {
    console.log(count); // Always logs 0!
  }, 1000);
  return () => clearInterval(timer);
}, []); // Missing count in deps

// ✅ CORRECT - Add dependency or use functional form
useEffect(() => {
  const timer = setInterval(() => {
    console.log(count);
  }, 1000);
  return () => clearInterval(timer);
}, [count]);
```

---

### useEffect

**Purpose:** Handle side effects (data fetching, subscriptions, DOM manipulation)

```tsx
// Run on every render
useEffect(() => {
  console.log('Runs on every render');
});

// Run once on mount (empty deps)
useEffect(() => {
  console.log('Runs once');
}, []);

// Run when dependencies change
useEffect(() => {
  console.log('Runs when count changes');
}, [count]);

// Cleanup function
useEffect(() => {
  const subscription = api.subscribe(data => setData(data));
  return () => subscription.unsubscribe(); // Cleanup on unmount
}, []);
```

**Common Mistake:**
```tsx
// ❌ WRONG - Missing dependency
useEffect(() => {
  fetch(`/api/user/${userId}`).then(setUser);
}, []); // userId not in deps!

// ✅ CORRECT - Include all dependencies
useEffect(() => {
  fetch(`/api/user/${userId}`).then(setUser);
}, [userId]);
```

---

### useRef

**Purpose:** Persist values across renders without triggering re-renders

```tsx
// Basic ref
const inputRef = useRef<HTMLInputElement>(null);

// Access DOM element
<input ref={inputRef} />;
inputRef.current?.focus();

// Mutable value that doesn't trigger render
const countRef = useRef(0);
countRef.current++; // Won't cause re-render
```

**Three useRef Cases:**
```tsx
// 1. Mutable value (not for rendering)
const prevCount = useRef<number>();
const currentCount = 5;
prevCount.current = currentCount;

// 2. DOM reference
const buttonRef = useRef<HTMLButtonElement>(null);
<button ref={buttonRef}>Click</button>;

// 3. Initial value only (runs once)
const [state, setState] = useState(() => {
  // Expensive computation runs only once
  return computeInitialState();
});
```

---

### useContext

**Purpose:** Access context without prop drilling

```tsx
// Create context
const ThemeContext = createContext<string>('light');

// Use in component
const theme = useContext(ThemeContext);
```

---

### useReducer

**Purpose:** Manage complex state with multiple sub-values or complex transitions

```tsx
const [state, dispatch] = useReducer(
  (state, action) => {
    switch (action.type) {
      case 'increment':
        return { count: state.count + 1 };
      case 'decrement':
        return { count: state.count - 1 };
      default:
        return state;
    }
  },
  { count: 0 } // initial state
);

dispatch({ type: 'increment' });
```

---

### useMemo

**Purpose:** Memoize expensive calculations

```tsx
// Memoize computation
const expensiveValue = useMemo(() => {
  return data.filter(x => x.active).map(x => x.name);
}, [data]);

// Memoize object (prevents new object each render)
const options = useMemo(() => ({
  enabled: !!userId,
  staleTime: 5000,
}), [userId]);
```

---

### useCallback

**Purpose:** Memoize functions to prevent unnecessary re-renders

```tsx
// Memoize callback
const handleClick = useCallback((id: string) => {
  console.log(id);
}, []); // Empty deps = stable reference

// With dependencies
const handleSubmit = useCallback((data: FormData) => {
  postForm(data);
}, [postForm]); // Recreated when postForm changes
```

---

## Advanced Hooks

### useTransition

**Purpose:** Mark updates as non-blocking (low priority)

```tsx
const [isPending, startTransition] = useTransition();

const handleChange = (value: string) => {
  setInput(value);
  startTransition(() => {
    // This can be interrupted
    setFilteredResults(filter(value));
  });
};
```

---

### useDeferredValue

**Purpose:** Defer rendering of a value

```tsx
const [text, setText] = useState('');
const deferredText = useDeferredValue(text);

// Render expensive component with deferred value
<List items={filter(deferredText)} />
```

---

### useId

**Purpose:** Generate unique IDs for accessibility

```tsx
const id = useId();
const labelId = `${id}-label`;
const inputId = `${id}-input`;

<label id={labelId}>Name</label>
<input id={inputId} aria-labelledby={labelId} />
```

---

### useSyncExternalStore

**Purpose:** Subscribe to external data sources (for library authors)

```tsx
const state = useSyncExternalStore(
  subscribe,  // Function to subscribe to changes
  getSnapshot, // Function to get current value
  getServerSnapshot // Optional server snapshot
);
```

---

### useInsertionEffect

**Purpose:** Inject styles before DOM mutations (for CSS-in-JS libraries)

```tsx
useInsertionEffect(() => {
  // Runs before DOM mutations
  styleElement.textContent = `.css { color: red }`;
  document.head.appendChild(styleElement);
}, []);
```

---

### useLayoutEffect

**Purpose:** Synchronous effect that runs after DOM mutations

```tsx
useLayoutEffect(() => {
  // Runs synchronously after all DOM mutations
  const rect = ref.current.getBoundingClientRect();
  console.log(rect);
}, []);
```

---

### useImperativeHandle

**Purpose:** Expose custom methods via ref

```tsx
useImperativeHandle(ref, () => ({
  focus: () => inputRef.current?.focus(),
  scrollIntoView: () => inputRef.current?.scrollIntoView(),
}), [inputRef]);
```

---

## Custom Hooks Rules

### Rules of Hooks

1. **Only call hooks at the top level** — Not in loops, conditions, or nested functions
2. **Only call hooks from React functions** — Components or custom hooks
3. **Use ESLint plugin** — `eslint-plugin-react-hooks`

### Dependency Array Decision Tree

```
Does your effect reference a value?
│
├─ NO → Can you remove the effect?
│   ├─ YES → Remove it
│   └─ NO → Use [] (run once on mount)
│
├─ YES → Is it from props or state?
│   ├─ NO (const defined in component) → Add to deps
│   └─ YES → Add to deps
│
└─ Is the value an object/function?
    ├─ YES → Consider useMemo/useCallback
    └─ NO → Add to deps normally
```

---

## Quick Reference Table

| Hook | When to Use |
|------|-------------|
| useState | Simple local state |
| useEffect | Side effects, subscriptions |
| useRef | DOM access, mutable values |
| useContext | Access context |
| useReducer | Complex state logic |
| useMemo | Expensive calculations |
| useCallback | Stable function refs |
| useTransition | Non-blocking updates |
| useDeferredValue | Defer rendering |
| useId | Unique IDs for a11y |
| useSyncExternalStore | External subscriptions |
| useLayoutEffect | Synchronous DOM measurements |
| useInsertionEffect | CSS-in-JS injection |
| useImperativeHandle | Expose imperative methods |

---

## What's Next

- [TypeScript React Cheatsheet](02-typescript-react-cheatsheet.md) - TypeScript patterns
- [Commands Cheatsheet](03-commands-cheatsheet.md) - CLI commands
