# Local Fonts

## Using Local Fonts

```typescript
import localFont from "next/font/local";

const myFont = localFont({
  src: "./fonts/MyFont.woff2",
  display: "swap",
});

export default function Layout({ children }) {
  return (
    <html lang="en">
      <body className={myFont.className}>
        {children}
      </body>
    </html>
  );
}
```

## Multiple Weights

```typescript
const fonts = localFont({
  src: [
    {
      path: "./fonts/Roboto-Regular.woff2",
      weight: "400",
      style: "normal",
    },
    {
      path: "./fonts/Roboto-Bold.woff2", 
      weight: "700",
      style: "normal",
    },
  ],
});
```
