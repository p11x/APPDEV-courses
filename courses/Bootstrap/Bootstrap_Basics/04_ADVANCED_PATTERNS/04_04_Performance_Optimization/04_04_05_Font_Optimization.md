---
title: "Font Optimization"
module: "Performance Optimization"
difficulty: 2
duration: "25 minutes"
prerequisites: ["CSS fonts", "Typography"]
tags: ["fonts", "performance", "preload", "variable-fonts"]
---

# Font Optimization

## Overview

Custom web fonts can significantly impact page load performance, often causing layout shifts (FOIT/FOUT) and blocking initial rendering. Optimizing fonts with `font-display`, subsetting, preloading, and variable fonts reduces font payloads by 50-90% while eliminating invisible text during loading.

## Basic Implementation

Configure `font-display: swap` for immediate text rendering:

```css
@font-face {
  font-family: 'Inter';
  font-style: normal;
  font-weight: 400;
  font-display: swap;
  src: url('/fonts/inter-v13-latin-regular.woff2') format('woff2');
}

@font-face {
  font-family: 'Inter';
  font-style: normal;
  font-weight: 700;
  font-display: swap;
  src: url('/fonts/inter-v13-latin-700.woff2') format('woff2');
}
```

Preload critical fonts in HTML:

```html
<head>
  <!-- Preload critical fonts -->
  <link rel="preload" href="/fonts/inter-v13-latin-regular.woff2"
        as="font" type="font/woff2" crossorigin>
  <link rel="preload" href="/fonts/inter-v13-latin-700.woff2"
        as="font" type="font/woff2" crossorigin>

  <link rel="stylesheet" href="/css/fonts.css">
</head>
```

Apply fonts with Bootstrap's SCSS variables:

```scss
$font-family-base: 'Inter', system-ui, -apple-system, sans-serif;
$headings-font-weight: 700;

@import "bootstrap/scss/bootstrap";
```

## Advanced Variations

Use variable fonts to replace multiple font files:

```css
/* Single variable font replaces multiple weights */
@font-face {
  font-family: 'Inter';
  font-style: normal;
  font-weight: 100 900;
  font-display: swap;
  src: url('/fonts/inter-v13-latin-variable.woff2') format('woff2-variations'),
       url('/fonts/inter-v13-latin-variable.woff2') format('woff2');
}

/* Use any weight within the variable range */
body { font-family: 'Inter', sans-serif; font-weight: 400; }
h1 { font-weight: 700; }
.fw-light { font-weight: 300; }
```

Subset fonts to include only used characters:

```bash
# Install pyftsubset
pip install fonttools brotli

# Subset to Latin characters only
pyftsubset inter-regular.ttf \
  --output-file=inter-regular-subset.woff2 \
  --flavor=woff2 \
  --layout-features='kern,liga' \
  --unicodes='U+0000-00FF,U+0131,U+0152-0153,U+02BB-02BC,U+2000-206F'

# Create CSS with subset font
pyftsubset inter-regular.ttf \
  --output-file=inter-regular-latin-ext.woff2 \
  --flavor=woff2 \
  --unicodes='U+0100-024F,U+0259,U+1E00-1EFF'
```

Implement font loading strategy with JavaScript:

```js
// font-loader.js
const fontFaces = [
  new FontFace('Inter', 'url(/fonts/inter-variable.woff2)', {
    style: 'normal',
    weight: '100 900',
    display: 'swap'
  })
];

Promise.all(fontFaces.map(font => font.load()))
  .then(fonts => {
    fonts.forEach(font => document.fonts.add(font));
    document.documentElement.classList.add('fonts-loaded');
  })
  .catch(err => console.warn('Font loading failed:', err));
```

```css
/* Fallback font with matching metrics */
@font-face {
  font-family: 'Inter-fallback';
  src: local('Arial');
  ascent-override: 90%;
  descent-override: 22%;
  line-gap-override: 0%;
  size-adjust: 107%;
}

body {
  font-family: 'Inter', 'Inter-fallback', system-ui, sans-serif;
}
```

## Best Practices

1. Always use `font-display: swap` or `optional` for web fonts
2. Preload the 1-2 most critical font files
3. Use WOFF2 format for best compression
4. Subset fonts to required character ranges
5. Use variable fonts to reduce HTTP requests
6. Provide metric-matched fallback fonts to reduce CLS
7. Limit font families to 2-3 per page
8. Self-host fonts instead of using third-party CDNs when possible
9. Cache fonts aggressively with long max-age headers
10. Monitor font loading performance with Lighthouse
11. Use `unicode-range` to load only needed character sets
12. Test font rendering across different operating systems

## Common Pitfalls

1. Missing `font-display` causing invisible text (FOIT)
2. Preloading too many font files
3. Not providing fallback fonts causing layout shift
4. Using font CDN without performance consideration
5. Loading entire font when only Latin subset is needed
6. Not using `crossorigin` attribute on font preloads
7. Ignoring font file size in performance audits
8. Blocking render with synchronous font loading

## Accessibility Considerations

- Ensure font sizes meet minimum readability standards
- Provide sufficient font weight contrast for hierarchy
- Test with user font size preferences
- Support `prefers-reduced-motion` for font transitions
- Verify font legibility for users with dyslexia

## Responsive Behavior

- Test font rendering at all viewport sizes
- Ensure font sizes scale appropriately
- Verify line-height remains readable on mobile
- Test with browser zoom at 200%+
