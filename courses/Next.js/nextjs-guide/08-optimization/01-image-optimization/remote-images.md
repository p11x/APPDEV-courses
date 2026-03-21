# Using Remote Images

## Configuration

```typescript
// next.config.ts
import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: "https",
        hostname: "images.unsplash.com",
      },
      {
        protocol: "https", 
        hostname: "example.com",
      },
    ],
  },
};

export default nextConfig;
```

## Usage

```typescript
import Image from "next/image";

export default function Page() {
  return (
    <Image
      src="https://images.unsplash.com/photo-1"
      alt="Photo"
      width={400}
      height={300}
    />
  );
}
```
