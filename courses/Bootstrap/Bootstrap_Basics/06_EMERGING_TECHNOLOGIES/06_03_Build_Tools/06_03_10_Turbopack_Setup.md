---
title: "Turbopack with Bootstrap"
slug: "turbopack-bootstrap"
difficulty: 3
tags: ["bootstrap", "turbopack", "nextjs", "bundler", "development"]
prerequisites:
  - "06_03_04_Next_JS_Setup"
  - "06_03_09_ESBuild_Bootstrap"
related:
  - "06_03_09_ESBuild_Bootstrap"
  - "06_03_11_CDN_Bundlers"
duration: "35 minutes"
---

# Turbopack with Bootstrap

## Overview

Turbopack is the Rust-based successor to Webpack, designed for maximum development speed. In Next.js, Turbopack replaces Webpack as the development bundler, providing near-instant Hot Module Replacement (HMR) updates. When combined with Bootstrap, Turbopack handles SCSS compilation, CSS modules, and component updates with sub-50ms response times. This creates a development experience where Bootstrap style changes appear instantly in the browser without full page reloads.

## Basic Implementation

Set up Next.js with Turbopack and Bootstrap for fast development.

```bash
npx create-next-app@latest my-bootstrap-app --typescript --app --use-npm
cd my-bootstrap-app
npm install bootstrap bootstrap-icons @types/bootstrap sass
```

```json
// package.json
{
  "scripts": {
    "dev": "next dev --turbopack",
    "build": "next build",
    "start": "next start"
  }
}
```

```scss
// src/app/globals.scss
@import "bootstrap/scss/bootstrap";
@import "bootstrap-icons/font/bootstrap-icons.css";

:root {
  --bs-primary: #6366f1;
  --bs-primary-rgb: 99, 102, 241;
}
```

```tsx
// src/app/layout.tsx
import 'bootstrap/dist/css/bootstrap.min.css';
import './globals.scss';

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
          <div className="container">
            <a className="navbar-brand" href="#">Bootstrap App</a>
          </div>
        </nav>
        <main className="container mt-4">{children}</main>
      </body>
    </html>
  );
}
```

```tsx
// src/app/page.tsx
export default function Home() {
  return (
    <div className="row">
      <div className="col-md-8">
        <div className="card">
          <div className="card-body">
            <h1 className="card-title">Welcome</h1>
            <p className="card-text">This app uses Turbopack for instant updates.</p>
            <button className="btn btn-primary">
              <i className="bi bi-rocket-takeoff me-2"></i>Get Started
            </button>
          </div>
        </div>
      </div>
      <div className="col-md-4">
        <div className="card bg-light">
          <div className="card-body">
            <h5>Stats</h5>
            <ul className="list-unstyled mb-0">
              <li><strong>12ms</strong> HMR update</li>
              <li><strong>0ms</strong> CSS injection</li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}
```

## Advanced Variations

### Bootstrap with CSS Modules

Use CSS Modules alongside Bootstrap for component-scoped styling with Turbopack.

```tsx
// src/components/FeatureCard.tsx
import styles from './FeatureCard.module.scss';

interface FeatureCardProps {
  icon: string;
  title: string;
  description: string;
  variant?: 'primary' | 'success' | 'warning';
}

export default function FeatureCard({ icon, title, description, variant = 'primary' }: FeatureCardProps) {
  return (
    <div className={`card h-100 ${styles.featureCard}`}>
      <div className="card-body text-center">
        <div className={`${styles.iconWrapper} bg-${variant} bg-opacity-10 rounded-circle mx-auto mb-3`}>
          <i className={`bi bi-${icon} text-${variant} fs-3`}></i>
        </div>
        <h5 className="card-title">{title}</h5>
        <p className="card-text text-muted">{description}</p>
      </div>
    </div>
  );
}
```

```scss
// src/components/FeatureCard.module.scss
.featureCard {
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  border: 1px solid var(--bs-border-color);

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
  }
}

.iconWrapper {
  width: 64px;
  height: 64px;
  display: flex;
  align-items: center;
  justify-content: center;
}
```

### Dynamic Bootstrap Import with Turbopack

Lazy-load Bootstrap JavaScript components to reduce initial bundle size.

```tsx
// src/components/ModalWrapper.tsx
'use client';
import { useEffect, useRef } from 'react';

export default function ModalWrapper({ children, title }: { children: React.ReactNode; title: string }) {
  const modalRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    let modal: any;
    const loadModal = async () => {
      const { Modal } = await import('bootstrap');
      if (modalRef.current) {
        modal = new Modal(modalRef.current);
      }
    };
    loadModal();
    return () => modal?.dispose();
  }, []);

  return (
    <div className="modal fade" ref={modalRef} tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{title}</h5>
            <button type="button" className="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div className="modal-body">{children}</div>
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
            <button type="button" className="btn btn-primary">Save</button>
          </div>
        </div>
      </div>
    </div>
  );
}
```

### Turbopack-Specific Configuration

Configure Next.js with Turbopack-specific options for Bootstrap optimization.

```typescript
// next.config.ts
import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  turbopack: {
    rules: {
      '*.svg': {
        loaders: ['@svgr/webpack'],
        as: '*.js',
      },
    },
    resolveAlias: {
      '@bootstrap': 'bootstrap/scss',
    },
  },
  sassOptions: {
    includePaths: ['node_modules'],
    prependData: `@import "bootstrap/scss/functions"; @import "src/styles/variables";`,
  },
};

export default nextConfig;
```

## Best Practices

1. Use `next dev --turbopack` for development and standard `next build` for production
2. Import Bootstrap CSS globally in the root layout for consistent availability
3. Use `'use client'` directive on components that use Bootstrap JavaScript APIs
4. Lazy-load Bootstrap JS components with dynamic `import()` to reduce initial bundle
5. Dispose Bootstrap component instances in React `useEffect` cleanup functions
6. Use CSS Modules for component-specific styles alongside Bootstrap
7. Configure `sassOptions.includePaths` to include `node_modules` for SCSS imports
8. Keep Bootstrap variables in a separate `_variables.scss` file for easy customization
9. Use Turbopack's `resolveAlias` for cleaner Bootstrap import paths
10. Test production builds separately since Turbopack is development-only in Next.js
11. Use `@types/bootstrap` for TypeScript support with Bootstrap components
12. Avoid global CSS conflicts by scoping custom styles with CSS Modules
13. Monitor Turbopack console output for compilation warnings
14. Keep Bootstrap and Next.js versions in sync with latest stable releases

## Common Pitfalls

1. **Production confusion**: Turbopack only works in development mode, production uses Webpack
2. **Missing `'use client'`**: Bootstrap JS components fail in server components without the directive
3. **Memory leaks**: Not disposing Bootstrap Modal/Dropdown instances causes memory growth
4. **SCSS path issues**: Bootstrap SCSS imports failing without proper `includePaths` configuration
5. **CSS ordering**: Global Bootstrap CSS conflicting with CSS Module specificity
6. **Hydration mismatch**: Bootstrap modifying DOM during hydration causing React errors
7. **TypeScript errors**: Missing `@types/bootstrap` causing type errors on Bootstrap APIs

## Accessibility Considerations

Ensure Bootstrap's ARIA attributes are preserved during server-side rendering. Test that Bootstrap modals, dropdowns, and tooltips work correctly with React hydration. Verify that focus management in Bootstrap components does not conflict with React's virtual DOM. Use `suppressHydrationWarning` on elements where Bootstrap modifies attributes during initialization. Ensure dynamically imported Bootstrap components maintain proper ARIA roles after lazy loading.

```tsx
<html lang="en" suppressHydrationWarning>
  <body suppressHydrationWarning>
    {children}
  </body>
</html>
```

## Responsive Behavior

Bootstrap's responsive grid system works identically with Turbopack as with other bundlers. Use Next.js `useMediaQuery` or CSS-based responsive patterns. Turbopack's instant HMR means responsive layout changes appear immediately during development. Test responsive breakpoints with Next.js built-in mobile viewport simulation. Use Bootstrap's `container`, `row`, and `col-*` classes normally within Next.js pages and components.
