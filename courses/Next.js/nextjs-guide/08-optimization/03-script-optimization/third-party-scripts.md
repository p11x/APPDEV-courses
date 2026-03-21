# Third-Party Scripts

## Common Examples

```typescript
import Script from "next/script";

// Google Analytics
<Script 
  src="https://www.googletagmanager.com/gtag/js?id=GA_ID"
  strategy="afterInteractive"
/>

// Facebook Pixel
<Script 
  src="https://connect.facebook.net/en_US/fbevents.js"
  strategy="afterInteractive"
/>

// Chat widgets
<Script 
  src="https://widget.example.com/widget.js"
  strategy="lazyOnload"
/>
```

## Best Practices

1. Use `afterInteractive` for most third-party scripts
2. Use `lazyOnload` for non-critical scripts
3. Load in layout for global scripts
