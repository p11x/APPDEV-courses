# Fixing Common Performance Issues

## What You'll Learn
- Fix slow LCP
- Reduce CLS
- Improve FID/INP
- Common Next.js optimizations

## Prerequisites
- Understanding of Lighthouse scores

## Do I Need This Right Now?
These are the most common issues found in Next.js apps and how to fix them.

## Fixing LCP (Largest Contentful Paint)

### Issue: Large Hero Image
```typescript
// Wrong: Not optimized
<img src="/hero.jpg" />

// Fixed: Use next/image
import Image from 'next/image';

<Image 
  src="/hero.jpg" 
  alt="Hero"
  width={1200}
  height={600}
  priority  // Loads immediately!
  placeholder="blur"
  blurDataURL="data:image/jpeg..."
/>
```

### Issue: Slow Server Response
```typescript
// Wrong: Blocking render
export default async function Page() {
  const data = await fetch('https://slow-api.com/data').then(r => r.json());
  return <div>{data.content}</div>;
}

// Fixed: Use streaming with Suspense
import { Suspense } from 'react';

export default function Page() {
  return (
    <Suspense fallback={<Loading />}>
      <SlowComponent />
    </Suspense>
  );
}

async function SlowComponent() {
  const data = await fetch('https://slow-api.com/data').then(r => r.json());
  return <div>{data.content}</div>;
}
```

### Issue: Render-Blocking Resources
```typescript
// Wrong: CSS in head blocks rendering
<head>
  <link rel="stylesheet" href="/styles.css" />
</head>

// Fixed: Next.js handles this automatically with built-in CSS
// Just use:
// import './styles.css' in layout.tsx
```

## Fixing CLS (Cumulative Layout Shift)

### Issue: Images Without Dimensions
```typescript
// Wrong: Causes layout shift
<img src="image.jpg" />

// Fixed: Always set dimensions
<Image 
  src="image.jpg" 
  width={800} 
  height={600}
  alt="Description"
/>
```

### Issue: Dynamic Content Loading
```typescript
// Wrong: Content jumps when loads
<div>
  <StaticContent />
  {showModal && <Modal />}
</div>

// Fixed: Reserve space
<div>
  <StaticContent />
  {showModal && (
    <div style={{ minHeight: '200px' }}>
      <Modal />
    </div>
  )}
</div>
```

### Issue: Late-Loading Fonts
```typescript
// Wrong: Font causes shift
<head>
  <link href="https://fonts.googleapis.com/css2?family=Open+Sans" rel="stylesheet" />
</head>

// Fixed: Use next/font (automatically prevents shift)
import { Inter } from 'next/font/google';

const inter = Inter({ subsets: ['latin'] });

export default function Layout({ children }) {
  return (
    <html className={inter.className}>
      <body>{children}</body>
    </html>
  );
}
```

## Fixing FID/INP (Input Delay)

### Issue: Heavy JavaScript
```typescript
// Wrong: Large bundle blocks interaction
// page.tsx
import { useEffect } from 'react';

export default function Page() {
  useEffect(() => {
    // Heavy computation on load
    heavyFunction();
  }, []);
  
  return <button>Click me</button>;
}

// Fixed: Use dynamic import
import dynamic from 'next/dynamic';

const HeavyComponent = dynamic(() => import('./HeavyComponent'), {
  loading: () => <p>Loading...</p>,
});

export default function Page() {
  return (
    <>
      <button>Click me</button>
      <HeavyComponent />
    </>
  );
}
```

### Issue: Too Many Event Listeners
```typescript
// Wrong: Many listeners on single element
<div onClick={handleClick}>
  {items.map(item => (
    <div key={item.id}>{item.name}</div>
  ))}
</div>

// Fixed: Use event delegation
<ul onClick={(e) => {
  const li = e.target.closest('li');
  if (li) handleClick(li.id);
}}>
  {items.map(item => (
    <li key={item.id}>{item.name}</li>
  ))}
</ul>
```

## Quick Wins

### 1. Enable Compression
```javascript
// next.config.js
module.exports = {
  compress: true, // Enabled by default on Vercel
};
```

### 2. Use Static Generation
```typescript
// Use static rendering when possible
export const dynamic = 'force-static';

// Or default - Next.js caches automatically
```

### 3. Optimize Dependencies
```bash
# Check bundle size
npm run build -- --analyze

# Use smaller alternatives
# Instead of moment.js → date-fns
# Instead of lodash → lodash-es (tree-shakeable)
```

### 4. Configure Caching
```typescript
// Add caching headers
export async function GET() {
  return Response.json(data, {
    headers: {
      'Cache-Control': 'public, s-maxage=3600, stale-while-revalidate=86400',
    },
  });
}
```

## Summary
- LCP: Use next/image with priority, stream slow content
- CLS: Always set image dimensions, reserve space for dynamic content
- FID/INP: Code splitting, reduce main thread work
- Use next/font to prevent font layout shifts
- Enable compression and caching
- Check bundle analyzer regularly

## Next Steps
- [vercel-speed-insights.md](../02-nextjs-analytics/vercel-speed-insights.md) — Vercel analytics
