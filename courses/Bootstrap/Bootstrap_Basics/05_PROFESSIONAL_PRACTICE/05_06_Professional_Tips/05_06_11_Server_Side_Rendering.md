---
title: Server-Side Rendering with Bootstrap
category: Professional
difficulty: 3
time: 40 min
tags: bootstrap5, ssr, server-side-rendering, node-js, hydration, critical-css, fouc
---

# Server-Side Rendering with Bootstrap

## Overview

Server-side rendering (SSR) with Bootstrap 5 requires careful handling of CSS loading, JavaScript initialization, and hydration to avoid flash of unstyled content (FOUC) and layout shifts. SSR frameworks like Next.js, Nuxt, and Astro render HTML on the server, delivering content immediately while JavaScript hydrates the page in the browser. Bootstrap's CSS must be inlined or preloaded to style the initial HTML, and JavaScript plugins must initialize only on the client after hydration completes.

## Basic Implementation

In Next.js, Bootstrap CSS should be imported globally and included in the server-rendered output:

```javascript
// pages/_app.js (Next.js)
import 'bootstrap/dist/css/bootstrap.min.css';
import { useEffect } from 'react';

export default function App({ Component, pageProps }) {
  useEffect(() => {
    import('bootstrap/dist/js/bootstrap.bundle.min.js');
  }, []);

  return <Component {...pageProps} />;
}
```

Preventing FOUC with critical CSS inlining:

```javascript
// next.config.js
const nextConfig = {
  experimental: {
    optimizeCss: true,
  },
  // Extract and inline critical CSS
  compiler: {
    styledComponents: true,
  },
};

module.exports = nextConfig;
```

Conditional Bootstrap JS loading:

```javascript
// Only run on client
function useBootstrap() {
  useEffect(() => {
    if (typeof window !== 'undefined') {
      import('bootstrap/dist/js/bootstrap.bundle.min.js').then(bootstrap => {
        window.bootstrap = bootstrap;
        // Re-initialize components after hydration
        document.querySelectorAll('[data-bs-toggle="tooltip"]')
          .forEach(el => new bootstrap.Tooltip(el));
      });
    }
  }, []);
}
```

## Advanced Variations

Server-side rendering with Astro and Bootstrap:

```astro
---
// pages/index.astro
import bootstrapCSS from 'bootstrap/dist/css/bootstrap.min.css?inline';
---

<html lang="en">
<head>
  <style set:html={bootstrapCSS}></style>
</head>
<body>
  <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
      <a class="navbar-brand" href="/">MyApp</a>
      <button class="navbar-toggler" type="button"
              data-bs-toggle="collapse"
              data-bs-target="#navContent">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navContent">
        <ul class="navbar-nav">
          <li class="nav-item">
            <a class="nav-link active" href="/">Home</a>
          </li>
        </ul>
      </div>
    </div>
  </nav>

  <script>
    import 'bootstrap/dist/js/bootstrap.bundle.min.js';
  </script>
</body>
</html>
```

Custom SSR hydration strategy with React:

```jsx
import { useState, useEffect, useRef } from 'react';

function BootstrapComponent({ children, component, options = {} }) {
  const ref = useRef(null);
  const [hydrated, setHydrated] = useState(false);

  useEffect(() => {
    setHydrated(true);
    if (ref.current && component) {
      import('bootstrap/dist/js/bootstrap.bundle.min.js').then(({ default: bootstrap }) => {
        const ComponentClass = bootstrap[component];
        if (ComponentClass) {
          new ComponentClass(ref.current, options);
        }
      });
    }
  }, [component]);

  return (
    <div ref={ref} className={hydrated ? 'bs-hydrated' : 'bs-ssr'}>
      {children}
    </div>
  );
}

// Usage
<BootstrapComponent component="Carousel" options={{ interval: 3000 }}>
  <div className="carousel-inner">
    <div className="carousel-item active">Slide 1</div>
  </div>
</BootstrapComponent>
```

Extracting critical CSS for SSR:

```javascript
// scripts/extract-critical.js
const critical = require('critical');
const path = require('path');

critical.generate({
  base: './build/',
  src: 'index.html',
  css: ['node_modules/bootstrap/dist/css/bootstrap.min.css'],
  dimensions: [
    { width: 375, height: 667 },
    { width: 1200, height: 900 },
  ],
  inline: true,
  extract: true,
  target: {
    css: 'critical.css',
    html: 'index-critical.html',
  },
});
```

## Best Practices

1. **Inline critical Bootstrap CSS** - Include above-the-fold styles in a `<style>` tag to prevent FOUC
2. **Defer Bootstrap JS to client** - Only load JavaScript plugins in `useEffect` or equivalent client-side hook
3. **Avoid `window` access during render** - Guard all browser API access with `typeof window !== 'undefined'` checks
4. **Use `suppressHydrationWarning`** - React attribute for elements where server/client markup intentionally differs
5. **Preload Bootstrap CSS** - Use `<link rel="preload" as="style">` for non-critical Bootstrap stylesheets
6. **Lazy load non-critical components** - Import carousel, offcanvas, and collapse JS only when components appear
7. **Set `min-height` on dynamic containers** - Prevent layout shift when JS-dependent content renders
8. **Use static markup for navigation** - Render nav structure server-side; enhance with JS collapse behavior on client
9. **Extract above-the-fold CSS** - Use critical CSS tools to inline only necessary rules for initial render
10. **Test with JavaScript disabled** - SSR should deliver a functional page without any JavaScript executing

## Common Pitfalls

1. **FOUC from external CSS** - Loading Bootstrap via `<link>` without inlining critical CSS causes unstyled flash
2. **Hydration mismatch** - Server-rendered HTML differs from client-rendered HTML due to date formatting, random IDs, or conditional logic
3. **Double initialization** - Initializing Bootstrap components on both server (SSR framework) and client causes duplicate event listeners
4. **Importing full Bootstrap JS on server** - `bootstrap.bundle.min.js` references `window` and `document`, breaking Node.js builds
5. **Missing CSS in SSR framework** - Forgetting to include Bootstrap CSS in the SSR framework's CSS pipeline
6. **Not handling dynamic routes** - SSR pages with dynamic content may render different Bootstrap components server vs client
7. **Ignoring `suppressHydrationWarning`** - Browser extensions, timestamps, and locale differences cause false hydration warnings

## Accessibility Considerations

SSR delivers accessible HTML before JavaScript loads, which is the ideal scenario for assistive technologies. Ensure server-rendered HTML includes correct ARIA attributes. Focus management for modals and dropdowns should only activate after client-side hydration to avoid conflicting with the framework's DOM reconciliation.

## Responsive Behavior

Bootstrap's responsive classes render correctly server-side because they are pure CSS. The responsive behavior works immediately on initial paint without JavaScript. Avoid JavaScript-dependent responsive logic (like custom breakpoints in JS) during SSR, as the server has no viewport information. Use CSS-only responsive patterns for server-rendered content.
