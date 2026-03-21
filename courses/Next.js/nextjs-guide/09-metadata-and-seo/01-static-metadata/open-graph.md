# Open Graph Metadata

## Basic Usage

```typescript
export const metadata: Metadata = {
  title: "My Page",
  description: "Page description",
  openGraph: {
    title: "My Page - OG Title",
    description: "OG Description",
    images: ["/og-image.jpg"],
    url: "https://example.com/page",
    siteName: "My Site",
    locale: "en_US",
    type: "website",
  },
};
```

## Summary

Open Graph controls how your page appears when shared on social media.
