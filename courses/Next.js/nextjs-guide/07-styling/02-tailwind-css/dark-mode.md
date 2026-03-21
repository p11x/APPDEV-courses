# Dark Mode in Tailwind

## Enabling Dark Mode

```typescript
// tailwind.config.ts
export default {
  darkMode: 'class',
  // ...
}
```

## Using Dark Mode

```typescript
export default function Component() {
  return (
    <div className="bg-white dark:bg-gray-900">
      <h1 className="text-gray-900 dark:text-white">
        Hello
      </h1>
    </div>
  );
}
```

Add `class="dark"` to `<html>` for dark mode.
