# Parallel vs Sequential Fetching

## What You'll Learn
- The difference between parallel and sequential fetching
- When to use each approach
- Optimizing data loading performance

## Prerequisites
- Understanding of async Server Components
- Basic fetch API knowledge

## Concept Explained Simply

When your page needs to fetch data from multiple sources, you have two choices:

1. **Sequential** — Fetch one after another, waiting for each to complete
2. **Parallel** — Fetch all at the same time, wait for all to complete

Think of it like grocery shopping:
- **Sequential** = Go to store A, wait, then go to store B, wait, then go to store C
- **Parallel** = Call friend A to shop at store A, friend B at store B, friend C at store C, all at the same time

Parallel is almost always faster because you're not waiting for one thing to finish before starting the next.

## Sequential Fetching

Sequential means one fetch waits for the previous to complete:

```typescript
// Sequential - SLOWER
async function getData() {
  const user = await fetchUser();    // Wait for this...
  const posts = await fetchPosts();  // Then wait for this...
  const comments = await fetchComments();  // Then this...
  
  return { user, posts, comments };
}
```

Total time = time1 + time2 + time3

## Parallel Fetching

Parallel means all fetches start at the same time:

```typescript
// Parallel - FASTER
async function getData() {
  const [user, posts, comments] = await Promise.all([
    fetchUser(),     // Start all at once
    fetchPosts(),    // Start all at once
    fetchComments(), // Start all at once
  ]);
  
  return { user, posts, comments };
}
```

Total time = max(time1, time2, time3)

## Complete Code Examples

### Sequential Example

Use sequential when one fetch depends on another:

```typescript
// src/app/user/[id]/page.tsx
// We need the user first to get their posts
interface Props {
  params: Promise<{ id: string }>;
}

async function getUser(id: string) {
  const res = await fetch(`https://api.example.com/users/${id}`);
  return res.json();
}

async function getUserPosts(userId: string) {
  const res = await fetch(`https://api.example.com/users/${userId}/posts`);
  return res.json();
}

export default async function UserProfilePage({ params }: Props) {
  const { id } = await params;
  
  // Sequential: Must get user first, then posts depend on user
  const user = await getUser(id);
  const posts = await getUserPosts(id); // Can't start until we have user ID!

  return (
    <main style={{ padding: "2rem" }}>
      <header>
        <h1>{user.name}</h1>
        <p>{user.email}</p>
      </header>
      
      <section style={{ marginTop: "2rem" }}>
        <h2>Posts by {user.name}</h2>
        {posts.map((post: any) => (
          <article key={post.id} style={{ padding: "1rem", border: "1px solid #ddd", marginBottom: "1rem" }}>
            <h3>{post.title}</h3>
            <p>{post.content}</p>
          </article>
        ))}
      </section>
    </main>
  );
}
```

### Parallel Example

Use parallel when fetches are independent:

```typescript
// src/app/dashboard/page.tsx
// All data is independent - can fetch in parallel

async function getUserData() {
  const res = await fetch("https://api.example.com/user/me", { cache: "no-store" });
  return res.json();
}

async function getStats() {
  const res = await fetch("https://api.example.com/stats", { cache: "no-store" });
  return res.json();
}

async function getNotifications() {
  const res = await fetch("https://api.example.com/notifications", { cache: "no-store" });
  return res.json();
}

async function getRecentActivity() {
  const res = await fetch("https://api.example.com/activity", { cache: "no-store" });
  return res.json();
}

export default async function DashboardPage() {
  // Parallel: All fetches start at the same time!
  const [user, stats, notifications, activity] = await Promise.all([
    getUserData(),
    getStats(),
    getNotifications(),
    getRecentActivity(),
  ]);

  return (
    <main style={{ padding: "2rem" }}>
      <h1>Welcome back, {user.name}!</h1>
      
      <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "2rem", marginTop: "2rem" }}>
        <section style={{ padding: "1rem", backgroundColor: "#f5f5f5", borderRadius: "8px" }}>
          <h2>Stats</h2>
          <p>Total Posts: {stats.totalPosts}</p>
          <p>Total Views: {stats.totalViews}</p>
        </section>
        
        <section style={{ padding: "1rem", backgroundColor: "#f5f5f5", borderRadius: "8px" }}>
          <h2>Notifications</h2>
          <p>{notifications.unread} unread</p>
        </section>
      </div>
      
      <section style={{ marginTop: "2rem" }}>
        <h2>Recent Activity</h2>
        {activity.map((item: any) => (
          <div key={item.id} style={{ padding: "0.5rem", borderBottom: "1px solid #eee" }}>
            {item.description}
          </div>
        ))}
      </section>
    </main>
  );
}
```

### Mixed Approach

Sometimes you need both:

```typescript
// src/app/products/[id]/page.tsx
// Sequential for dependent data, parallel for independent

async function getProduct(id: string) {
  const res = await fetch(`https://api.example.com/products/${id}`);
  return res.json();
}

async function getRelatedProducts(categoryId: string) {
  const res = await fetch(`https://api.example.com/categories/${categoryId}/products`);
  return res.json();
}

async function getProductReviews(productId: string) {
  const res = await fetch(`https://api.example.com/products/${productId}/reviews`);
  return res.json();
}

export default async function ProductPage({ params }: Props) {
  const { id } = await params;
  
  // Step 1: Get product (must be first - other data depends on it)
  const product = await getProduct(id);
  
  // Step 2: These are independent - parallel!
  const [reviews, relatedProducts] = await Promise.all([
    getProductReviews(id),
    getRelatedProducts(product.categoryId),
  ]);

  return (
    <main style={{ padding: "2rem" }}>
      <h1>{product.name}</h1>
      <p>{product.description}</p>
      
      <section>
        <h2>Reviews</h2>
        {reviews.map((r: any) => <p key={r.id}>{r.text}</p>)}
      </section>
      
      <section>
        <h2>Related Products</h2>
        {relatedProducts.map((p: any) => <div key={p.id}>{p.name}</div>)}
      </section>
    </main>
  );
}
```

## Visual Comparison

```
Sequential (slower):
Request A ─────────────►
                      Request B ─────────────►
                                         Request C ─────────────►
Total time: A + B + C

Parallel (faster):
Request A ─────────────►
Request B ─────────────►
Request C ─────────────►
Total time: max(A, B, C)
```

## Common Mistakes

### Mistake #1: Using Sequential When Not Needed

```typescript
// ✗ Wrong: Unnecessarily sequential
async function Page() {
  const user = await getUser();     // Wait...
  const products = await getProducts(); // Wait again...
  const categories = await getCategories(); // And again...
}

// ✓ Correct: Parallel for independent data
async function Page() {
  const [user, products, categories] = await Promise.all([
    getUser(),
    getProducts(),
    getCategories(),
  ]);
}
```

### Mistake #2: Using Parallel When Sequential Required

```typescript
// ✗ Wrong: Can't fetch dependent data in parallel
async function Page() {
  const [user, posts] = await Promise.all([
    getUser(),                    // This gives us the user ID
    getPosts(user.id),            // But user isn't defined yet!
  ]);
}

// ✓ Correct: Sequential when data depends on each other
async function Page() {
  const user = await getUser();           // First
  const posts = await getPosts(user.id);  // Second (depends on user)
}
```

### Mistake #3: Not Handling Partial Failures

```typescript
// ✗ Wrong: If one fetch fails, all fail
async function Page() {
  const [user, posts] = await Promise.all([
    getUser(),   // Might fail
    getPosts(),  // Then this also fails even if it's fine
  ]);
}

// ✓ Correct: Handle failures gracefully
async function Page() {
  const [user, posts] = await Promise.allSettled([
    getUser(),
    getPosts(),
  ]);
  
  const userData = user.status === "fulfilled" ? user.value : null;
  const postsData = posts.status === "fulfilled" ? posts.value : [];
}
```

## Summary

- Use **parallel** fetching when data is independent (faster)
- Use **sequential** when data depends on each other (required)
- Use `Promise.all()` for parallel fetching
- Consider mixed approaches for complex pages
- `Promise.allSettled()` handles partial failures

## Next Steps

Now let's learn about composition patterns:

- [Passing Server Data to Client →](../../03-react-server-components/03-composition-patterns/passing-server-data-to-client.md)
