---
title: Server Components with Bootstrap
category: [Future Bootstrap, Cutting Edge]
difficulty: 3
time: 30 min
tags: bootstrap5, server-components, RSC, streaming, SSR
---

## Overview

React Server Components (RSC) and similar server-rendered component patterns enable rendering Bootstrap markup on the server while hydrating interactive parts on the client. This eliminates client-side JavaScript for static Bootstrap components, reduces bundle size, and enables streaming UI with progressive loading.

## Basic Implementation

A server component rendering a Bootstrap card with data fetched on the server.

```jsx
// ServerComponent.jsx (runs on server only)
export default async function DashboardCard({ metricId }) {
  const data = await fetch(`https://api.example.com/metrics/${metricId}`, {
    cache: 'no-store'
  }).then(r => r.json());

  return (
    <div className="card h-100">
      <div className="card-body">
        <h6 className="card-subtitle text-body-secondary mb-2">
          {data.label}
        </h6>
        <p className="display-6 fw-bold">{data.value}</p>
        <span className={`badge text-bg-${data.trend > 0 ? 'success' : 'danger'}`}>
          {data.trend > 0 ? '+' : ''}{data.trend}%
        </span>
      </div>
    </div>
  );
}

// page.jsx
import DashboardCard from './DashboardCard';

export default async function Page() {
  return (
    <div className="container py-4">
      <h1 className="mb-4">Dashboard</h1>
      <div className="row g-3">
        <div className="col-md-4">
          <DashboardCard metricId="revenue" />
        </div>
        <div className="col-md-4">
          <DashboardCard metricId="users" />
        </div>
        <div className="col-md-4">
          <DashboardCard metricId="orders" />
        </div>
      </div>
    </div>
  );
}
```

## Advanced Variations

Streaming Bootstrap components with React Suspense for progressive loading.

```jsx
import { Suspense } from 'react';

function SkeletonCard() {
  return (
    <div className="card h-100">
      <div className="card-body">
        <div className="placeholder-glow">
          <span className="placeholder col-6 mb-2"></span>
          <span className="placeholder col-4 display-6"></span>
        </div>
      </div>
    </div>
  );
}

export default async function StreamingDashboard() {
  return (
    <div className="container py-4">
      <div className="row g-3">
        <div className="col-md-4">
          <Suspense fallback={<SkeletonCard />}>
            <DashboardCard metricId="revenue" />
          </Suspense>
        </div>
        <div className="col-md-4">
          <Suspense fallback={<SkeletonCard />}>
            <DashboardCard metricId="users" />
          </Suspense>
        </div>
        <div className="col-md-4">
          <Suspense fallback={<SkeletonCard />}>
            <DashboardCard metricId="orders" />
          </Suspense>
        </div>
      </div>
    </div>
  );
}
```

Mixing server and client components with Bootstrap forms.

```jsx
// Server component: renders the form structure
export default async function ContactForm() {
  const fields = await getFormSchema();

  return (
    <form action={submitAction} className="needs-validation">
      {fields.map(field => (
        <div className="mb-3" key={field.name}>
          <label className="form-label" htmlFor={field.name}>
            {field.label}
          </label>
          {field.type === 'select' ? (
            <select className="form-select" id={field.name} name={field.name} required>
              {field.options.map(opt => (
                <option key={opt.value} value={opt.value}>{opt.label}</option>
              ))}
            </select>
          ) : (
            <input
              type={field.type}
              className="form-control"
              id={field.name}
              name={field.name}
              required={field.required}
            />
          )}
        </div>
      ))}
      <SubmitButton />
    </form>
  );
}

// SubmitButton.jsx (client component)
'use client';
import { useFormStatus } from 'react-dom';

export default function SubmitButton() {
  const { pending } = useFormStatus();
  return (
    <button type="submit" className="btn btn-primary" disabled={pending}>
      {pending && <span className="spinner-border spinner-border-sm me-2"></span>}
      {pending ? 'Sending...' : 'Submit'}
    </button>
  );
}
```

## Best Practices

1. Keep static Bootstrap components as server components — no client JS needed
2. Mark interactive components (`'use client'`) only when browser APIs are required
3. Use Suspense with Bootstrap skeleton placeholders for streaming loading states
4. Fetch data in server components to eliminate client-side waterfalls
5. Use Bootstrap's placeholder classes for skeleton loading UI
6. Pass serialized data from server to client components as props
7. Use server actions for form submissions without API routes
8. Avoid importing Bootstrap JS in server components — use client components for interactive behavior
9. Leverage `cache: 'no-store'` or `revalidate` for dynamic server data
10. Keep the client component boundary at the interactive leaf nodes

## Common Pitfalls

1. **Hydration mismatch** — Server-rendered HTML differs from client-rendered HTML
2. **Bootstrap JS in server components** — `window`/`document` references break on the server
3. **Over-using client components** — Marking large subtrees as `'use client'` negates the benefits
4. **Missing Suspense boundaries** — Without Suspense, streaming doesn't work
5. **Serialization errors** — Passing non-serializable props (functions, classes) across server/client boundary
6. **No progressive enhancement** — Forms break without JavaScript if server actions aren't configured
7. **Bundle bloat from client components** — Server components reduce bundle, but client components increase it
8. **Stale cache** — Server component data caching can serve outdated content

## Accessibility Considerations

Server-rendered HTML provides immediate content for screen readers without waiting for hydration. Ensure Bootstrap's ARIA attributes are included in server component markup. Use semantic HTML in server components so assistive tech works before JavaScript loads. Provide loading announcements via `aria-live` for Suspense boundaries.

## Responsive Behavior

Bootstrap's responsive grid classes work identically in server components since they're pure CSS. Server components render responsive HTML at request time, eliminating layout shifts from client-side rendering. Use server-side device detection to serve optimized Bootstrap component variants for mobile vs. desktop.
