# The next/image Component

## What You'll Learn
- Using the Image component
- Automatic optimization

## Complete Example

```typescript
import Image from "next/image";

export default function Page() {
  return (
    <Image
      src="/hero.jpg"
      alt="Hero image"
      width={800}
      height={600}
      priority
    />
  );
}
```

## Props

| Prop | Purpose |
|------|---------|
| `src` | Image source |
| `alt` | Accessibility text |
| `width` | Display width |
| `height` | Display height |
| `priority` | Preload image |
| `fill` | Fill parent container |
