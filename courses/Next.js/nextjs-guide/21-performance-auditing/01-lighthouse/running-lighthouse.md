# Running Lighthouse

## What You'll Learn
- Using Lighthouse for performance testing
- Understanding the metrics
- Interpreting results
- Improving scores

## Prerequisites
- A deployed Next.js app
- Browser with DevTools
- Basic performance knowledge

## Concept Explained Simply

Lighthouse is Google's free tool that analyzes your website and gives you scores for performance, accessibility, best practices, SEO, and more. It's like a health checkup for your website — you get a detailed report card showing what's working well and what needs improvement.

Think of it like a car's inspection: they check the engine (performance), brakes (accessibility), lights (best practices), and paperwork (SEO).

## Running Lighthouse

### In Chrome DevTools

1. Open your site in Chrome
2. Right-click → Inspect
3. Click the "Lighthouse" tab
4. Select what to test (Mobile or Desktop)
5. Click "Analyze page load"

### From Command Line

```bash
# Install Lighthouse CLI
npm install -g lighthouse

# Run audit
lighthouse https://your-site.com --output json --output-path report.json
```

## Understanding Scores

### Performance (0-100)

| Score | Rating | Meaning |
|-------|--------|---------|
| 90-100 | Green | Excellent |
| 50-89 | Yellow | Needs improvement |
| 0-49 | Red | Poor |

### Core Web Vitals

- **LCP** (Largest Contentful Paint) — Time to see main content
- **FID** (First Input Delay) — Time until page responds
- **CLS** (Cumulative Layout Shift) — Visual stability

## Common Issues

### Slow LCP

```typescript
// Optimize images
import Image from "next/image";

<Image 
  src="/hero.jpg"
  alt="Hero"
  width={1200}
  height={600}
  priority // Preloads image!
/>
```

### High CLS

```typescript
// Always set dimensions on images
<Image 
  src="/image.jpg"
  width={800}
  height={600}
  alt="..."
/>
```

### High FID/INP

```typescript
// Use Server Components for data
// Reduce client-side JavaScript
```

## Summary

- Run Lighthouse from Chrome DevTools or CLI
- Aim for 90+ performance score
- Focus on Core Web Vitals
- Fix issues in order of impact
