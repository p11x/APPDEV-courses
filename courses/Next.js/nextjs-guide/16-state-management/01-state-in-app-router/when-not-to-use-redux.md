# When NOT to Use Redux

## What You'll Learn
- Understanding when Redux is overkill
- Recognizing simpler alternatives
- Making smart state choices
- Avoiding over-engineering

## Prerequisites
- Basic knowledge of state management
- Understanding of React patterns
- Familiarity with App Router

## Concept Explained Simply

Redux is powerful, but it's also complex. Adding Redux to your app is like hiring a project manager — helpful for large teams working on complex projects, but unnecessary overhead for a small startup. Many Next.js apps don't need Redux at all.

Think of it this way: if you're building a small blog, you don't need enterprise-level state management. Use the simplest tool that works.

## When You DON'T Need Redux

### 1. URL State Handles Your Use Case

```typescript
// DON'T - Redux for filters
// store/filters.ts
const filtersSlice = createSlice({
  name: "filters",
  initialState: { category: "all", sort: "newest" },
  reducers: {
    setCategory: (state, action) => { state.category = action.payload },
    setSort: (state, action) => { state.sort = action.payload },
  },
});

// DO - URL state
const params = new URLSearchParams(searchParams);
const category = params.get("category") || "all";
```

### 2. Server Components Handle Data

```typescript
// DON'T - Redux for server data
// store/posts.ts
const postsSlice = createSlice({
  name: "posts",
  initialState: { data: [], loading: false },
  reducers: { setPosts: (state, action) => { ... } },
});

// DO - Server Component fetches directly
// app/posts/page.tsx
export default async function PostsPage() {
  const posts = await db.post.findMany(); // Direct fetch!
  return <PostList posts={posts} />;
}
```

### 3. Simple Local State Works

```typescript
// DON'T - Redux for a toggle
// store/ui.ts
const uiSlice = createSlice({
  name: "ui",
  initialState: { sidebarOpen: false },
  reducers: { toggleSidebar: (state) => { state.sidebarOpen = !state.sidebarOpen }},
});

// DO - useState
const [sidebarOpen, setSidebarOpen] = useState(false);
```

### 4. Component-Level State Is Enough

```typescript
// DON'T - Global state for component-specific data
// store/cart.ts
const cartSlice = createSlice({
  name: "cart",
  initialState: { items: [] },
  reducers: { addItem: (state, action) => { ... } },
});

// DO - Local state if only one component needs it
function AddToCartButton() {
  const [quantity, setQuantity] = useState(1);
  // ...
}
```

## When You Actually Need Global State

| Scenario | Solution |
|----------|----------|
| Auth state (logged in user) | NextAuth.js or Context |
| Theme preference | CSS + localStorage or Context |
| Shopping cart | Zustand (simpler than Redux) |
| Real-time updates | React Query (for server data) |
| Complex UI state | Zustand or Context |

## Alternatives Comparison

| Tool | Complexity | Best For |
|------|------------|----------|
| URL params | Very Low | Filters, sorting, pagination |
| useState | Low | Component-specific state |
| Context | Low | Auth, theme (rare updates) |
| Zustand | Medium | Global client state |
| React Query | Medium | Server state with caching |
| Redux Toolkit | High | Large team, complex updates |

## Zustand as a Simpler Alternative

```typescript
// Instead of Redux, use Zustand
// store/cart.ts
import { create } from "zustand";

interface CartItem {
  id: string;
  quantity: number;
}

interface CartState {
  items: CartItem[];
  addItem: (item: CartItem) => void;
  removeItem: (id: string) => void;
  clear: () => void;
}

export const useCartStore = create<CartState>((set) => ({
  items: [],
  addItem: (item) => set((state) => ({
    items: [...state.items, item]
  })),
  removeItem: (id) => set((state) => ({
    items: state.items.filter(i => i.id !== id)
  })),
  clear: () => set({ items: [] }),
}));

// Usage in component
"use client";
import { useCartStore } from "@/store/cart";

function CartButton({ product }) {
  const addItem = useCartStore((state) => state.addItem);
  
  return (
    <button onClick={() => addItem({ id: product.id, quantity: 1 })}>
      Add to Cart
    </button>
  );
}
```

## Common Mistakes

### Mistake 1: Adding Redux Before Needed

```typescript
// WRONG - Planning for complexity you don't have
// "What if we need to share state between components later?"
// Result: Over-engineered app!

// CORRECT - Start simple, add when needed
// "We need to share cart between header and checkout"
// Result: Add Zustand when actually needed
```

### Mistake 2: Redux for Server Data

```typescript
// WRONG - Redux with server data
export const fetchPosts = createAsyncThunk("posts/fetch", async () => {
  const res = await fetch("/api/posts");
  return res.json();
});

// Using in component
function Posts() {
  const dispatch = useDispatch();
  const posts = useSelector(selectPosts);
  
  useEffect(() => { dispatch(fetchPosts()); }, []);
  // ...
}

// CORRECT - Server Components do this automatically
export default async function PostsPage() {
  const posts = await db.post.findMany();
  return <PostsList posts={posts} />;
}
```

### Mistake 3: Not Using Built-in Solutions

```typescript
// WRONG - Redux for URL state
router.push("/");
const category = useSelector(selectCategory);

// CORRECT - Just use the URL
const searchParams = useSearchParams();
const category = searchParams.get("category");
```

## Summary

- Don't use Redux if URL state, Server Components, or useState solves your problem
- Start with the simplest solution and add complexity only when needed
- Zustand is a simpler alternative for global client state
- Server Components eliminate the need for Redux in many cases
- Consider React Query for server state caching

## Next Steps

- [zustand-setup.md](../02-zustand/zustand-setup.md) - Zustand for global state
- [url-as-state.md](./url-as-state.md) - URL state patterns
