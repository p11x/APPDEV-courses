# Google Fonts

## Using Google Fonts

```typescript
import { Inter, Roboto } from "next/font/google";

const inter = Inter({ 
  subsets: ["latin"],
  display: "swap",
});

const roboto = Roboto({
  weight: ["400", "700"],
  subsets: ["latin"],
});
```

## Options

- `subsets` - Character sets
- `display` - Font display strategy
- `weight` - Font weights
- `style` - Font styles
