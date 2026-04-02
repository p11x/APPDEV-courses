---
title: "Next.js Bootstrap Integration"
topic: "Framework Integration"
difficulty: 3
duration: "45 minutes"
prerequisites: ["Next.js App Router", "React Server Components", "CSS Modules"]
tags: ["nextjs", "bootstrap", "ssr", "app-router", "server-components"]
---

## Overview

Integrating Bootstrap 5 with Next.js requires navigating server-side rendering (SSR), React Server Components (RSC), and Next.js's App Router architecture. The primary challenge is that Bootstrap's JavaScript components (modals, dropdowns, tooltips) use browser APIs and must be client-side only. CSS can be imported globally or via CSS Modules with SCSS, while JavaScript components need `"use client"` directives or dynamic imports with `ssr: false`.

Next.js supports Bootstrap through multiple approaches: direct CSS/SCSS import, `react-bootstrap` (which is SSR-compatible), or CSS-in-JS alternatives like Tailwind CSS for utility-first styling. The recommended approach for production is `react-bootstrap` for components + Bootstrap SCSS for theming, which provides full SSR support without jQuery.

## Basic Implementation

```bash
npx create-next-app@latest my-next-bootstrap
cd my-next-bootstrap
npm install react-bootstrap bootstrap
```

### App Router Setup

```tsx
// app/layout.tsx
import 'bootstrap/dist/css/bootstrap.min.css';
import type { Metadata } from 'next';

export const metadata: Metadata = {
  title: 'Next.js Bootstrap App',
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
```

### Server Component Page

```tsx
// app/page.tsx
import { Container, Row, Col, Card } from 'react-bootstrap';

export default function HomePage() {
  return (
    <Container className="py-5">
      <Row>
        <Col md={8} className="mx-auto">
          <Card>
            <Card.Header>
              <h5 className="mb-0">Next.js + Bootstrap</h5>
            </Card.Header>
            <Card.Body>
              <Card.Title>Server-Rendered Bootstrap</Card.Title>
              <Card.Text>
                This page is a React Server Component with Bootstrap styling.
              </Card.Text>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
}
```

### Client Interactive Component

```tsx
// components/InteractiveModal.tsx
'use client';

import { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

export function InteractiveModal() {
  const [show, setShow] = useState(false);
  const [formData, setFormData] = useState({ name: '', email: '' });

  return (
    <>
      <Button variant="primary" onClick={() => setShow(true)}>
        Open Form Modal
      </Button>

      <Modal show={show} onHide={() => setShow(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title>Contact Form</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group className="mb-3">
              <Form.Label>Name</Form.Label>
              <Form.Control
                type="text"
                value={formData.name}
                onChange={(e) => setFormData(prev => ({ ...prev, name: e.target.value }))}
              />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                value={formData.email}
                onChange={(e) => setFormData(prev => ({ ...prev, email: e.target.value }))}
              />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShow(false)}>Cancel</Button>
          <Button variant="primary" onClick={() => { setShow(false); }}>Submit</Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}
```

## Advanced Variations

### Custom SCSS Theme

```scss
// app/globals.scss
$primary: #6366f1;
$secondary: #64748b;
$enable-rounded: true;
$enable-shadows: true;

@import 'bootstrap/scss/bootstrap';
```

```tsx
// app/layout.tsx
import './globals.scss';
```

### Dynamic Import for Heavy Components

```tsx
// app/dashboard/page.tsx
import dynamic from 'next/dynamic';

const DataTable = dynamic(() => import('@/components/DataTable'), {
  ssr: false,
  loading: () => <div className="spinner-border text-primary" role="status">
    <span className="visually-hidden">Loading...</span>
  </div>,
});

export default function DashboardPage() {
  return (
    <div className="container py-5">
      <h1>Dashboard</h1>
      <DataTable />
    </div>
  );
}
```

### Server Actions with Bootstrap Forms

```tsx
// app/actions.ts
'use server';

export async function submitContact(formData: FormData) {
  const name = formData.get('name') as string;
  const email = formData.get('email') as string;

  // Server-side validation and processing
  if (!name || !email) {
    return { error: 'All fields are required' };
  }

  return { success: true, message: `Thank you, ${name}!` };
}
```

```tsx
// app/contact/page.tsx
import { submitContact } from '@/app/actions';
import { Container, Form, Button, Alert } from 'react-bootstrap';

export default function ContactPage() {
  return (
    <Container className="py-5">
      <h1>Contact</h1>
      <Form action={submitContact}>
        <Form.Group className="mb-3">
          <Form.Label>Name</Form.Label>
          <Form.Control type="text" name="name" required />
        </Form.Group>
        <Form.Group className="mb-3">
          <Form.Label>Email</Form.Label>
          <Form.Control type="email" name="email" required />
        </Form.Group>
        <Button type="submit" variant="primary">Submit</Button>
      </Form>
    </Container>
  );
}
```

### Theme Provider with Dark Mode

```tsx
// components/ThemeProvider.tsx
'use client';

import { createContext, useContext, useState, useEffect } from 'react';

const ThemeCtx = createContext({ theme: 'light', toggle: () => {} });

export function BootstrapThemeProvider({ children }: { children: React.ReactNode }) {
  const [theme, setTheme] = useState('light');

  useEffect(() => {
    document.documentElement.setAttribute('data-bs-theme', theme);
  }, [theme]);

  return (
    <ThemeCtx.Provider value={{
      theme,
      toggle: () => setTheme(t => t === 'light' ? 'dark' : 'light'),
    }}>
      {children}
    </ThemeCtx.Provider>
  );
}

export const useTheme = () => useContext(ThemeCtx);
```

## Best Practices

1. **Import Bootstrap CSS globally** in `app/layout.tsx` to make styles available to all pages and components.
2. **Use `"use client"` directive** on components that use `react-bootstrap` interactive features (modals, dropdowns).
3. **Use `dynamic()` with `ssr: false`** for components that depend on browser-only APIs (charts, drag-and-drop).
4. **Keep Server Components for static Bootstrap markup** (cards, grids, typography) to reduce client-side JavaScript.
5. **Use `react-bootstrap` instead of raw Bootstrap JS** to avoid SSR hydration mismatches.
6. **Configure `sassOptions` in `next.config.js`** to silence Bootstrap's Sass deprecation warnings.
7. **Use CSS Modules** (`.module.scss`) for component-scoped styles alongside global Bootstrap CSS.
8. **Leverage Server Actions** for form submissions with Bootstrap form components.
9. **Use `Image` from `next/image`** with Bootstrap's `.img-fluid` pattern for optimized responsive images.
10. **Set `output: 'standalone'`** in `next.config.js` for Docker deployments with Bootstrap assets.

## Common Pitfalls

1. **Using Bootstrap JS (`bootstrap.bundle.js`) with `react-bootstrap`** causes duplicate event listeners and hydration errors.
2. **Interactive components without `"use client"`** fail with "useState is not defined" in Server Components.
3. **Importing SCSS in Server Components** works for CSS, but Sass features that depend on client runtime can cause issues.
4. **Not using `ssr: false`** for browser-dependent components causes "window/document is not defined" errors.
5. **CSS-in-JS and Bootstrap SCSS conflicting** when both define global styles — maintain clear separation.

## Accessibility Considerations

Next.js renders initial HTML server-side, so Bootstrap's semantic markup and ARIA attributes are present in the initial payload, improving accessibility for assistive technologies. Use `react-bootstrap` components which manage ARIA attributes automatically. Server Components ensure that accessible markup (landmarks, headings, alt text) is available before JavaScript hydration. Use Next.js's `<Script>` component with `strategy="lazyOnload"` for analytics that don't block accessibility-related interactivity.

## Responsive Behavior

Bootstrap's responsive grid works identically in Next.js Server and Client Components. The CSS classes (`col-md-6`, `d-lg-flex`) are applied statically in the initial HTML render. Use `react-bootstrap`'s `<Col md={6} lg={4}>` props for responsive sizing. Next.js's `Image` component handles responsive image sizing with `sizes` prop complementing Bootstrap's grid system. Server Components reduce the JavaScript payload, ensuring responsive Bootstrap layouts render faster on mobile devices.