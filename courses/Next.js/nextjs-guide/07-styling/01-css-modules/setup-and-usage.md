# CSS Modules Setup and Usage

## What You'll Learn
- Setting up CSS Modules
- Creating scoped styles

## Complete Example

```typescript
// src/app/page.tsx
import styles from "./page.module.css";

export default function Page() {
  return (
    <div className={styles.container}>
      <h1 className={styles.title}>Hello</h1>
    </div>
  );
}
```

```css
/* src/app/page.module.css */
.container {
  padding: 2rem;
  max-width: 800px;
  margin: 0 auto;
}

.title {
  font-size: 2rem;
  color: #333;
}
```

## Summary

- CSS Modules are automatically scoped
- No configuration needed
- Use `.module.css` extension
