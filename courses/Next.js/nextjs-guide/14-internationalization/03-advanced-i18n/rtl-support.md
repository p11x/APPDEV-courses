# RTL (Right-to-Left) Support

## What You'll Learn
- Understanding RTL languages
- Setting up RTL support in Next.js
- CSS considerations for RTL
- Testing RTL layouts

## Prerequisites
- Understanding of CSS
- Knowledge of internationalization
- Familiarity with Tailwind or CSS Modules

## Concept Explained Simply

Some languages are written from right to left (RTL) instead of left to right (LTR). Arabic, Hebrew, and Persian are examples. When your app supports these languages, the entire UI needs to flip — navigation goes to the right side, text aligns to the right, and icons that pointed left now point right.

Think of it like reading a manga — even though it's in Japanese (which can be written horizontally), the panels flow right to left. Your app needs to adapt similarly when the language changes.

## Complete Code Example

### Setting HTML Direction

```typescript
// app/[locale]/layout.tsx
import { getTranslations } from "next-intl/server";

const rtlLocales = ["ar", "he", "fa", "ur"];

export default async function LocaleLayout({
  children,
  params,
}: {
  children: React.ReactNode;
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  
  // Determine text direction
  const dir = rtlLocales.includes(locale) ? "rtl" : "ltr";
  
  return (
    <html lang={locale} dir={dir}>
      <body>{children}</body>
    </html>
  );
}
```

### CSS for RTL with CSS Modules

```css
/* components/Navbar.module.css */
.container {
  display: flex;
  justify-content: space-between;
  padding: 1rem;
}

/* Automatic RTL using logical properties */
.marginStart {
  margin-left: 1rem;  /* In LTR: left side */
                     /* In RTL: right side! */
}

/* Explicit RTL/LTR using dir selector */
.container[dir="ltr"] .icon {
  margin-right: 0.5rem;
}

.container[dir="rtl"] .icon {
  margin-left: 0.5rem;
}
```

### Tailwind RTL Support

```tsx
// tailwind.config.ts
export default {
  content: [
    "./app/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      // Tailwind handles RTL automatically with:
      // ms- (margin-start) instead of ml-
      // me- (margin-end) instead of mr-
      // ps- (padding-start) instead of pl-
      // pe- (padding-end) instead of pr-
    },
  },
  plugins: [],
};
```

```tsx
// components/Navbar.tsx
export default function Navbar() {
  return (
    <nav className="flex justify-between p-4">
      <div className="flex items-center gap-2">
        {/* Use gap instead of margin for spacing */}
        <span className="font-bold">Logo</span>
      </div>
      
      <ul className="flex gap-4">
        {/* gap handles spacing automatically in RTL */}
        <li>Home</li>
        <li>About</li>
      </ul>
    </nav>
  );
}
```

### Flipping Icons for RTL

```tsx
// components/ArrowIcon.tsx
interface ArrowIconProps {
  direction: "left" | "right";
}

export default function ArrowIcon({ direction }: ArrowIconProps) {
  // Option 1: Transform based on direction
  // But actually, for most icons you want them to point
  // in the semantic direction regardless of text direction
  
  // Option 2: Use CSS transform with dir attribute
  return (
    <svg 
      className={direction === "left" ? "rtl:rotate-180" : ""}
      width="20" 
      height="20" 
      viewBox="0 0 24 24"
    >
      <path d="M15 18l-6-6 6-6" />
    </svg>
  );
}
```

### Handling Directional UI Components

```tsx
// components/Button.tsx
interface ButtonProps {
  children: React.ReactNode;
  icon?: React.ReactNode;
  iconPosition: "left" | "right";
}

export default function Button({ children, icon, iconPosition }: ButtonProps) {
  return (
    <button className="flex items-center gap-2">
      {iconPosition === "left" && icon}
      {children}
      {iconPosition === "right" && icon}
    </button>
  );
}

/* 
 * With "flex" and "gap", the order in JSX controls visual order.
 * For RTL, you don't need to swap positions — flexbox handles it!
 * 
 * LTR: [icon] [text]     when iconPosition="left"
 * RTL: [icon] [text]     (same order, but positioned correctly)
 */
```

### Complete RTL Layout Example

```tsx
// app/[locale]/layout.tsx
const rtlLocales = ["ar", "he", "fa", "ur", "yi"];

export default async function LocaleLayout({
  children,
  params,
}: {
  children: React.ReactNode;
  params: Promise<{ locale: string }>;
}) {
  const { locale } = await params;
  const dir = rtlLocales.includes(locale) ? "rtl" : "ltr";
  
  return (
    <html lang={locale} dir={dir}>
      <body className={dir === "rtl" ? "font-arabic" : "font-english"}>
        <div className="min-h-screen flex flex-col">
          <header className="flex justify-between items-center p-4 border-b">
            <div className="flex items-center gap-4">
              {/* Logo */}
              <h1>My App</h1>
            </div>
            <nav>
              <ul className="flex gap-4">
                <li>Home</li>
                <li>About</li>
              </ul>
            </nav>
          </header>
          
          <main className="flex-1 p-8">
            {children}
          </main>
          
          <footer className="p-4 border-t text-center">
            <p>© 2025 My App</p>
          </footer>
        </div>
      </body>
    </html>
  );
}
```

## Key CSS Properties for RTL

| LTR Property | RTL Equivalent | What It Does |
|--------------|---------------|--------------|
| `margin-left` | `margin-right` | Space on the left side |
| `margin-right` | `margin-left` | Space on the right side |
| `padding-left` | `padding-right` | Padding on the left |
| `padding-right` | `padding-left` | Padding on the right |
| `text-align: left` | `text-align: right` | Align text |

### Logical Properties (Recommended)

Modern CSS uses logical properties that automatically adapt:

| Logical Property | LTR Behavior | RTL Behavior |
|-----------------|--------------|--------------|
| `margin-inline-start` | margin-left | margin-right |
| `margin-inline-end` | margin-right | margin-left |
| `padding-inline-start` | padding-left | padding-right |
| `padding-inline-end` | padding-right | padding-left |
| `text-align: start` | left-aligned | right-aligned |
| `text-align: end` | right-aligned | left-aligned |

## Common Mistakes

### Mistake 1: Hardcoding Margin Direction

```typescript
// WRONG - Hardcoded left margin
<div style={{ marginLeft: "1rem" }}>
  Content
</div>

// CORRECT - Use logical property
<div style={{ marginInlineStart: "1rem" }}>
  Content
</div>

// Or with Tailwind:
<div className="ms-4">
  Content
</div>
```

### Mistake 2: Not Testing RTL

```typescript
// WRONG - Only testing in English
// Your app might break in Arabic!

// CORRECT - Test with RTL languages
// Visit /ar, /he, /fa versions of your app
```

### Mistake 3: Swapping Elements Manually

```typescript
// WRONG - Manual swapping
{locale === "ar" ? (
  <>
    {text} {icon}
  </>
) : (
  <>
    {icon} {text}
  </>
)}

// CORRECT - Use flexbox, it handles direction automatically
<div className="flex gap-2">
  {icon}
  {text}
</div>
```

## Summary

- Set `dir="rtl"` on the `<html>` element for RTL languages
- Common RTL languages: Arabic (ar), Hebrew (he), Persian (fa)
- Use logical CSS properties (margin-inline-start) instead of directional ones
- Tailwind has RTL-aware classes: ms-, me-, ps-, pe-
- Flexbox and Grid automatically handle RTL
- Test your app with RTL languages before deploying

## Next Steps

- [installing-next-intl.md](../02-next-intl-setup/installing-next-intl.md) - Review setup for RTL languages
- [locale-detection.md](../01-i18n-basics/locale-detection.md) - Detecting RTL locales
