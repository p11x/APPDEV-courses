---
title: "Solid.js + Bootstrap"
slug: "solidjs-bootstrap"
difficulty: 3
tags: ["bootstrap", "solidjs", "reactive", "framework", "performance"]
prerequisites:
  - "06_04_01_React_Bootstrap"
  - "06_04_09_Qwik_Bootstrap"
related:
  - "06_04_09_Qwik_Bootstrap"
  - "06_04_11_Lit_Bootstrap"
duration: "35 minutes"
---

# Solid.js + Bootstrap

## Overview

Solid.js uses fine-grained reactivity without a virtual DOM, updating only the exact DOM nodes that change. Combined with Bootstrap, this means class toggles, visibility changes, and content updates happen with surgical precision rather than re-rendering entire component trees. Solid's `createSignal` and `createEffect` primitives map directly to Bootstrap state changes, creating highly performant UIs that remain responsive even with complex dashboards and data-heavy interfaces.

## Basic Implementation

A Solid.js project with Bootstrap styling and reactive state management.

```bash
npm create solid@latest my-bootstrap-solid
cd my-bootstrap-solid
npm install bootstrap bootstrap-icons sass
```

```scss
// src/index.scss
@import "bootstrap/scss/bootstrap";
@import "bootstrap-icons/font/bootstrap-icons.css";

$success: #10b981;
$primary: #8b5cf6;
```

```tsx
// src/App.tsx
import { createSignal } from 'solid-js';
import './index.scss';

export default function App() {
  const [count, setCount] = createSignal(0);
  const [theme, setTheme] = createSignal<'light' | 'dark'>('light');

  return (
    <div classList={{ 'bg-dark text-light': theme() === 'dark' }} style={{ 'min-height': '100vh' }}>
      <nav class="navbar navbar-expand-lg" classList={{ 'navbar-dark bg-dark': theme() === 'dark', 'navbar-light bg-light': theme() === 'light' }}>
        <div class="container">
          <a class="navbar-brand" href="#">Solid + Bootstrap</a>
          <div class="d-flex gap-2">
            <button class="btn btn-sm" classList={{ 'btn-outline-light': theme() === 'dark', 'btn-outline-dark': theme() === 'light' }}
                    onClick={() => setTheme(t => t === 'light' ? 'dark' : 'light')}>
              <i classList={{ 'bi bi-moon': theme() === 'light', 'bi bi-sun': theme() === 'dark' }}></i>
            </button>
          </div>
        </div>
      </nav>
      <div class="container mt-4">
        <div class="card" classList={{ 'bg-dark border-secondary text-light': theme() === 'dark' }}>
          <div class="card-body text-center">
            <h1 class="display-4">{count()}</h1>
            <div class="d-flex justify-content-center gap-2">
              <button class="btn btn-danger" onClick={() => setCount(c => c - 1)}>
                <i class="bi bi-dash-lg"></i>
              </button>
              <button class="btn btn-success" onClick={() => setCount(c => c + 1)}>
                <i class="bi bi-plus-lg"></i>
              </button>
              <button class="btn btn-secondary" onClick={() => setCount(0)}>Reset</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
```

## Advanced Variations

### Fine-Grained Conditional Rendering

Bootstrap alerts that appear and disappear with minimal DOM updates.

```tsx
import { createSignal, Show } from 'solid-js';

export function AlertDemo() {
  const [visible, setVisible] = createSignal(true);
  const [variant, setVariant] = createSignal<'success' | 'warning' | 'danger'>('success');

  return (
    <div>
      <div class="btn-group mb-3">
        <button class="btn btn-outline-success btn-sm" onClick={() => setVariant('success')}>Success</button>
        <button class="btn btn-outline-warning btn-sm" onClick={() => setVariant('warning')}>Warning</button>
        <button class="btn btn-outline-danger btn-sm" onClick={() => setVariant('danger')}>Danger</button>
      </div>
      <button class="btn btn-primary btn-sm mb-3 ms-2" onClick={() => setVisible(v => !v)}>
        Toggle Alert
      </button>

      <Show when={visible()}>
        <div class="alert alert-dismissible" classList={{
          'alert-success': variant() === 'success',
          'alert-warning': variant() === 'warning',
          'alert-danger': variant() === 'danger',
        }} role="alert">
          <strong>Alert!</strong> This is a {variant()} alert with fine-grained reactivity.
          <button type="button" class="btn-close" onClick={() => setVisible(false)}></button>
        </div>
      </Show>
    </div>
  );
}
```

### Reactive Bootstrap Table with Sorting

A data table that sorts reactively without re-rendering unchanged rows.

```tsx
import { createSignal, For, createMemo } from 'solid-js';

interface User { id: number; name: string; email: string; role: string; }

export function ReactiveTable() {
  const [users] = createSignal<User[]>([
    { id: 1, name: 'Alice', email: 'alice@example.com', role: 'Admin' },
    { id: 2, name: 'Bob', email: 'bob@example.com', role: 'User' },
    { id: 3, name: 'Charlie', email: 'charlie@example.com', role: 'Editor' },
  ]);

  const [sortField, setSortField] = createSignal<keyof User>('name');
  const [sortDir, setSortDir] = createSignal<'asc' | 'desc'>('asc');
  const [filter, setFilter] = createSignal('');

  const sortedUsers = createMemo(() => {
    const filtered = users().filter(u =>
      u.name.toLowerCase().includes(filter().toLowerCase()) ||
      u.email.toLowerCase().includes(filter().toLowerCase())
    );
    return filtered.sort((a, b) => {
      const valA = a[sortField()];
      const valB = b[sortField()];
      const cmp = valA < valB ? -1 : valA > valB ? 1 : 0;
      return sortDir() === 'asc' ? cmp : -cmp;
    });
  });

  function toggleSort(field: keyof User) {
    if (sortField() === field) {
      setSortDir(d => d === 'asc' ? 'desc' : 'asc');
    } else {
      setSortField(field);
      setSortDir('asc');
    }
  }

  return (
    <div class="card">
      <div class="card-header">
        <input type="text" class="form-control" placeholder="Filter..."
               value={filter()} onInput={e => setFilter(e.currentTarget.value)} />
      </div>
      <div class="card-body p-0">
        <table class="table table-hover mb-0">
          <thead class="table-light">
            <tr>
              <For each={['name', 'email', 'role'] as (keyof User)[]}>
                {(field) => (
                  <th style={{ cursor: 'pointer' }} onClick={() => toggleSort(field)}>
                    {field.charAt(0).toUpperCase() + field.slice(1)}
                    {sortField() === field && (
                      <i classList={{
                        'bi bi-caret-up-fill ms-1': sortDir() === 'asc',
                        'bi bi-caret-down-fill ms-1': sortDir() === 'desc',
                      }}></i>
                    )}
                  </th>
                )}
              </For>
            </tr>
          </thead>
          <tbody>
            <For each={sortedUsers()}>
              {(user) => (
                <tr>
                  <td>{user.name}</td>
                  <td>{user.email}</td>
                  <td><span class="badge bg-primary">{user.role}</span></td>
                </tr>
              )}
            </For>
          </tbody>
        </table>
      </div>
    </div>
  );
}
```

### Bootstrap Tabs with Solid Router

```tsx
import { useLocation, A } from '@solidjs/router';

export function TabNav() {
  const location = useLocation();

  const tabs = [
    { path: '/dashboard', label: 'Dashboard', icon: 'speedometer2' },
    { path: '/users', label: 'Users', icon: 'people' },
    { path: '/settings', label: 'Settings', icon: 'gear' },
  ];

  return (
    <ul class="nav nav-tabs mb-3">
      {tabs.map(tab => (
        <li class="nav-item">
          <A class="nav-link" classList={{ active: location.pathname === tab.path }}
             href={tab.path}>
            <i class={`bi bi-${tab.icon} me-1`}></i> {tab.label}
          </A>
        </li>
      ))}
    </ul>
  );
}
```

## Best Practices

1. Use `createSignal` for reactive primitive state and `createStore` for complex objects
2. Use `createMemo` for derived state to avoid recomputing on every render
3. Leverage `classList` directive for conditional Bootstrap classes instead of string concatenation
4. Use `<For>` for rendering lists to benefit from Solid's keyed reconciliation
5. Use `<Show>` for conditional rendering instead of ternary operators in JSX
6. Import Bootstrap CSS globally rather than per-component for consistent styling
7. Use `createEffect` to sync Bootstrap JavaScript component state with Solid signals
8. Avoid `createResource` for static data, use signals directly
9. Use `onCleanup` to dispose Bootstrap component instances (modals, tooltips)
10. Prefer `class` attribute over `className` in Solid JSX
11. Use `Suspense` for lazy-loaded Bootstrap component bundles
12. Keep component functions pure, avoiding side effects during render
13. Use Solid's `onMount` for Bootstrap JS initialization after DOM attachment
14. Test with Solid DevTools to verify fine-grained update behavior

## Common Pitfalls

1. **Destructuring signals**: Destructuring `createSignal` return value breaks reactivity tracking
2. **Calling signals in loops**: Reading signals inside loops without `<For>` loses fine-grained updates
3. **Missing `classList`**: Using string concatenation for classes instead of `classList` object
4. **Cleanup missing**: Not disposing Bootstrap component instances in `onCleanup`
5. **JSX in variables**: Storing JSX in variables outside reactive context breaks reactivity
6. **Virtual DOM thinking**: Writing code as if Solid re-renders entire components like React
7. **Missing key props**: Using `<For>` without unique keys causes incorrect DOM reconciliation

## Accessibility Considerations

Solid's fine-grained reactivity preserves DOM nodes, maintaining focus and ARIA state during updates. Use `aria-*` attributes directly in JSX. Ensure `<Show>` and `<For>` components preserve ARIA roles on wrapper elements. Test that Bootstrap modals and dropdowns maintain `aria-expanded` state through reactive updates. Use `role` attributes on list containers rendered with `<For>`. Announce dynamic changes with `aria-live` regions bound to signals.

```tsx
<div role="status" aria-live="polite">
  {notification()}
</div>
```

## Responsive Behavior

Solid.js does not alter Bootstrap's CSS or responsive behavior. All Bootstrap grid classes, responsive utilities, and breakpoints work identically. Use `classList` for responsive utility toggling based on signals. Test responsive layouts with Solid's DevTools to verify minimal DOM updates during resize. Use standard Bootstrap responsive patterns (`col-md-6`, `d-lg-none`) within Solid components.
