# Twitter Card Metadata

## Usage

```typescript
export const metadata: Metadata = {
  twitter: {
    card: "summary_large_image",
    title: "My Page Title",
    description: "Page description for Twitter",
    creator: "@twitterhandle",
    images: ["/twitter-image.jpg"],
  },
};
```

## Card Types

- `summary` - Small image
- `summary_large_image` - Large image
- `app` - Mobile app
- `player` - Video/audio
