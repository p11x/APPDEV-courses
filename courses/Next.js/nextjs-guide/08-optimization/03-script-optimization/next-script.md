# The next/script Component

## Basic Usage

```typescript
import Script from "next/script";

export default function Page() {
  return (
    <>
      <Script src="https://example.com/script.js" />
    </>
  );
}
```

## Strategy Options

| Strategy | When |
|----------|------|
| `beforeInteractive` | Before any Next.js code |
| `afterInteractive` | After page becomes interactive |
| `lazyOnload` | During idle time |
| `worker` | In a web worker (with Partytown) |
