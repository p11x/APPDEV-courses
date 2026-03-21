# Image Sizing Strategies

## Using Fill

```typescript
import Image from "next/image";

export default function Hero() {
  return (
    <div style={{ position: "relative", height: "400px" }}>
      <Image
        src="/hero.jpg"
        alt="Hero"
        fill
        style={{ objectFit: "cover" }}
      />
    </div>
  );
}
```

## Responsive Images

```typescript
<Image
  src="/image.jpg"
  alt="Responsive"
  sizes="(max-width: 768px) 100vw, 50vw"
  fill
/>
```

## Best Practices

1. Always set width and height (or use fill)
2. Use the `sizes` prop for responsive images
3. Use `priority` for above-the-fold images
4. Use modern formats (WebP, AVIF)
