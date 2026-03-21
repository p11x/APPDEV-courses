# Composing Classes in CSS Modules

## Combining Styles

```typescript
// src/app/page.tsx
import styles from "./page.module.css";

export default function Page() {
  return (
    <div className={`${styles.container} ${styles.highlight}`}>
      Composed classes!
    </div>
  );
}
```

## Using classnames Library

```typescript
import classNames from "classnames";
import styles from "./page.module.css";

export default function Page({ isActive }) {
  return (
    <div className={classNames(styles.container, {
      [styles.active]: isActive
    })}>
      Conditional classes
    </div>
  );
}
```
