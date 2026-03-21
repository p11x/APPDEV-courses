# Next.js Image and Font Optimization

## Overview
Next.js provides built-in optimization features for images and fonts that significantly improve performance. The `next/image` component automatically optimizes images for different screen sizes and formats, while `next/font` automatically optimizes fonts and removes layout shift. These features work together to create faster, more performant web applications.

## Prerequisites
- Understanding of Next.js basics
- Familiarity with React components
- Basic CSS knowledge

## Core Concepts

### Using next/image
The next/image component provides automatic image optimization:

```tsx
// [File: app/page.tsx]
import Image from 'next/image';

export default function Page() {
  return (
    <div>
      {/* Basic usage */}
      <Image 
        src="/hero.jpg" 
        alt="Hero image"
        width={1200}
        height={600}
      />
      
      {/* Responsive images */}
      <Image 
        src="/product.jpg"
        alt="Product"
        sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
        fill
        style={{ objectFit: 'cover' }}
      />
      
      {/* Priority loading for LCP */}
      <Image 
        src="/banner.jpg"
        alt="Banner"
        width={800}
        height={400}
        priority // Loads immediately - important for above-the-fold images
      />
      
      {/* With blur placeholder */}
      <Image 
        src="/photo.jpg"
        alt="Photo"
        placeholder="blur"
        blurDataURL="data:image/jpeg;base64,..." // Base64 encoded low-res image
      />
    </div>
  );
}
```

### Using next/font
The next/font component optimizes Google Fonts and other font sources:

```tsx
// [File: app/layout.tsx]
import { Inter, Playfair_Display, JetBrains_Mono } from 'next/font/google';

// Google Font with subsets and display
const inter = Inter({ 
  subsets: ['latin'],
  display: 'swap',
  variable: '--font-inter',
});

// Multiple weights
const playfair = Playfair_Display({
  subsets: ['latin'],
  display: 'swap',
  variable: '--font-playfair',
});

// Monospace font
const jetbrains = JetBrains_Mono({
  subsets: ['latin'],
  display: 'swap',
  variable: '--font-jetbrains',
});

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en" className={`${inter.variable} ${playfair.variable} ${jetbrains.variable}`}>
      <body>
        {children}
      </body>
    </html>
  );
}
```

### Custom Font Files
Use custom font files with next/font:

```tsx
// [File: app/layout.tsx]
import localFont from 'next/font/local';

const myFont = localFont({
  src: './fonts/my-font.woff2',
  display: 'swap',
  variable: '--font-my-font',
  preload: true,
  fallback: ['system-ui', 'sans-serif'],
});

export default function Layout({ children }) {
  return (
    <html className={myFont.variable}>
      <body>{children}</body>
    </html>
  );
}
```

### Image Configuration
Configure image optimization in next.config.js:

```javascript
// [File: next.config.js]
/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    // Remote patterns for external images
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'cdn.example.com',
        pathname: '/images/**',
      },
      {
        protocol: 'https',
        hostname: '*.amazonaws.com',
      },
    ],
    
    // Local patterns
    localPatterns: ['**/*.avif', '**/*.webp'],
    
    // Image formats
    formats: ['image/avif', 'image/webp'],
    
    // Device sizes
    deviceSizes: [640, 750, 828, 1080, 1200, 1920, 2048],
    
    // Image sizes
    imageSizes: [16, 32, 48, 64, 96, 128, 256, 384],
    
    // Minimum cache TTL
    minimumCacheTTL: 60 * 60 * 24, // 24 hours
  },
};

module.exports = nextConfig;
```

## Common Mistakes

### Mistake 1: Not Using Priority for LCP Images
```tsx
// ❌ WRONG - Hero image loads lazily
<Image src="/hero.jpg" alt="Hero" width={1200} height={600} />

// ✅ CORRECT - Hero image loads immediately
<Image src="/hero.jpg" alt="Hero" width={1200} height={600} priority />
```

### Mistake 2: Wrong Sizes Attribute
```tsx
// ❌ WRONG - No sizes means full width on all devices
<Image src="/image.jpg" alt="Image" fill />

// ✅ CORRECT - Proper sizes for responsive layout
<Image 
  src="/image.jpg" 
  alt="Image" 
  fill
  sizes="(max-width: 768px) 100vw, 50vw"
/>
```

### Mistake 3: Not Defining Width and Height
```tsx
// ❌ WRONG - Missing dimensions causes layout shift
<Image src="/image.jpg" alt="Image" />

// ✅ CORRECT - Always define dimensions or use fill
<Image src="/image.jpg" alt="Image" width={800} height={600} />
// OR
<Image src="/image.jpg" alt="Image" fill />
```

## Real-World Example

Complete image gallery with optimization:

```tsx
// [File: app/components/ImageGallery.tsx]
import Image from 'next/image';

interface GalleryProps {
  images: Array<{
    id: string;
    src: string;
    alt: string;
    width: number;
    height: number;
  }>;
}

export default function ImageGallery({ images }: GalleryProps) {
  return (
    <div className="gallery">
      {images.map((image, index) => (
        <div key={image.id} className="gallery-item">
          <Image
            src={image.src}
            alt={image.alt}
            width={image.width}
            height={image.height}
            // First image is priority (LCP)
            priority={index === 0}
            // Responsive sizes
            sizes="(max-width: 640px) 100vw, (max-width: 1024px) 50vw, 33vw"
            // Blur placeholder for perceived performance
            placeholder="blur"
            blurDataURL={generateBlurPlaceholder(image.src)}
            // Styling
            style={{ objectFit: 'cover' }}
            className="gallery-image"
          />
        </div>
      ))}
    </div>
  );
}

function generateBlurPlaceholder(src: string): string {
  // In production, generate actual blur placeholder
  return 'data:image/jpeg;base64,/9j/4AAQSkZJRg...';
}
```

## Key Takeaways
- Use next/image for automatic image optimization
- Add priority prop for above-the-fold (LCP) images
- Use sizes attribute for responsive images
- Use next/font for automatic font optimization
- Use localFont for custom font files
- Configure remote patterns for external image sources
- Enable AVIF for better compression
- Use placeholder="blur" for better perceived performance

## What's Next
Continue to [Next.js Middleware and Auth](03-nextjs-middleware-and-auth.md) to learn about middleware and authentication patterns.