# Dynamic Metadata from API

```typescript
// src/app/blog/[slug]/page.tsx
export async function generateMetadata({ params }: Props) {
  const { slug } = await params;
  
  const post = await fetch(`https://api.example.com/posts/${slug}`)
    .then(res => res.json());
    
  return {
    title: post.title,
    description: post.excerpt,
    openGraph: {
      title: post.title,
      images: [post.coverImage],
    },
  };
}
```

Fetch data from your API to generate metadata dynamically.
