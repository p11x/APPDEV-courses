# Tailwind CSS Features

## Core Features

### 1. Utility Classes

Tailwind provides utility classes for every CSS property:

| Category | Examples |
|----------|----------|
| Spacing | `p-4`, `m-2`, `gap-3` |
| Typography | `text-xl`, `font-bold`, `leading-tight` |
| Colors | `text-red-500`, `bg-blue-300` |
| Layout | `flex`, `grid`, `block`, `hidden` |
| Sizing | `w-full`, `h-64`, `max-w-lg` |
| Borders | `border`, `rounded-lg`, `divide-y` |

### 2. Responsive Design

Built-in responsive breakpoints:

| Breakpoint | Width | Prefix |
|------------|-------|--------|
| sm | 640px | `sm:` |
| md | 768px | `md:` |
| lg | 1024px | `lg:` |
| xl | 1280px | `xl:` |
| 2xl | 1536px | `2xl:` |

### 3. Dark Mode Support

Enable dark mode with simple class:

```html
<!-- Automatic mode -->
<html class="dark">

<!-- Manual toggle -->
<div class="dark:bg-gray-900">
```

### 4. Customization

Highly configurable via `tailwind.config.js`:

| Option | Description |
|--------|-------------|
| theme | Customize colors, spacing, fonts |
| plugins | Add official and community plugins |
| presets | Share configurations |

### 5. JIT Compiler

Just-in-Time compiler features:

- On-demand CSS generation
- Faster build times
- Arbitrary values support
- Full CSS feature support

### 6. State Variants

Handle different states:

| Variant | Usage |
|---------|-------|
| Hover | `hover:text-blue-500` |
| Focus | `focus:outline-none` |
| Active | `active:bg-blue-700` |
| Disabled | `disabled:opacity-50` |

### 7. Group Variants

Style children based on parent state:

```html
<div class="group">
  <button class="group-hover:bg-blue-500">
```

### 8. Peer Variants

Style siblings based on state:

```html
<input class="peer-checked:bg-blue-500">
```

### 9. Custom Values

Use arbitrary values:

```html
<div class="w-[calc(100%-2rem)]">
<div class="text-[#ff0000]">
```

## Advanced Features

### 10. CSS Grid

```html
<div class="grid grid-cols-3 gap-4">
<div class="grid-cols-[1fr,2fr,1fr]">
```

### 11. Flexbox

```html
<div class="flex justify-between items-center">
<div class="flex-col gap-4">
```

### 12. Transitions

```html
<button class="transition-all duration-300 hover:scale-105">
```

### 13. Animations

Built-in animations:

| Class | Effect |
|-------|--------|
| `animate-spin` | Rotating |
| `animate-pulse` | Pulsing |
| `animate-bounce` | Bouncing |

---

*Back to [Web Development README](../README.md)*
*Back to [Main README](../../README.md)*