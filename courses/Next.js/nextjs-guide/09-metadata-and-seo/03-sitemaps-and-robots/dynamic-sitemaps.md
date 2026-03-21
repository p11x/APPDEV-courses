# Dynamic Sitemaps

## Generate from Database

```typescript
// src/app/sitemap.ts
export default async function sitemap() {
  const posts = await db.post.findMany({
    select: { slug: true, updatedAt: true },
  });
  
  const blogs = posts.map((post) => ({
    url: `https://example.com/blog/${post.slug}`,
    lastModified: post.updatedAt,
    changeFrequency: "weekly" as const,
    priority: 0.7,
  }));
  
  return [
    {
      url: "https://example.com",
      lastModified: new Date(),
      changeFrequency: "daily",
      priority: 1,
    },
    ...blogs,
  ];
}
```
