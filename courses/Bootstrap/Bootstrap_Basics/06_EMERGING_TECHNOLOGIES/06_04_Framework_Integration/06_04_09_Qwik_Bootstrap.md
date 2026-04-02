---
title: "Qwik + Bootstrap"
slug: "qwik-bootstrap"
difficulty: 3
tags: ["bootstrap", "qwik", "resumability", "framework", "performance"]
prerequisites:
  - "06_04_01_React_Bootstrap"
  - "06_04_02_Vue_Bootstrap"
related:
  - "06_04_10_SolidJS_Bootstrap"
  - "06_04_11_Lit_Bootstrap"
duration: "40 minutes"
---

# Qwik + Bootstrap

## Overview

Qwik introduces resumability, a paradigm where applications start instantly without hydration. Unlike React or Vue, Qwik serializes the application state and event handlers on the server, resuming execution on the client without re-executing component code. Combined with Bootstrap, Qwik delivers zero-JavaScript-on-load pages that still support full interactivity. Bootstrap components render server-side and become interactive only when the user interacts with them, resulting in near-zero Time to Interactive (TTI).

## Basic Implementation

Set up a Qwik project with Bootstrap styling and basic components.

```bash
npm create qwik@latest my-bootstrap-qwik
cd my-bootstrap-qwik
npm install bootstrap bootstrap-icons sass
```

```scss
// src/global.scss
@import "bootstrap/scss/bootstrap";
@import "bootstrap-icons/font/bootstrap-icons.css";

$primary: #7c3aed;
$enable-rounded: true;
```

```tsx
// src/routes/layout.tsx
import { component$, Slot } from '@builder.io/qwik';
import '../global.scss';

export default component$(() => {
  return (
    <div>
      <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
        <div class="container">
          <a class="navbar-brand" href="/">
            <i class="bi bi-lightning-charge-fill me-2"></i>
            Qwik + Bootstrap
          </a>
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#nav">
            <span class="navbar-toggler-icon"></span>
          </button>
          <div class="collapse navbar-collapse" id="nav">
            <ul class="navbar-nav ms-auto">
              <li class="nav-item"><a class="nav-link active" href="/">Home</a></li>
              <li class="nav-item"><a class="nav-link" href="/about">About</a></li>
            </ul>
          </div>
        </div>
      </nav>
      <main class="container mt-4">
        <Slot />
      </main>
    </div>
  );
});
```

```tsx
// src/routes/index.tsx
import { component$, useSignal } from '@builder.io/qwik';

export default component$(() => {
  const count = useSignal(0);

  return (
    <div class="row">
      <div class="col-md-8">
        <div class="card">
          <div class="card-body">
            <h1 class="card-title">Welcome to Qwik</h1>
            <p class="card-text">This page loaded with zero JavaScript. Click the button to resume interactivity.</p>
            <button class="btn btn-primary" onClick$={() => count.value++}>
              Count: {count.value}
            </button>
          </div>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card bg-light">
          <div class="card-body">
            <h5>Performance</h5>
            <span class="badge bg-success">0kb JS on load</span>
          </div>
        </div>
      </div>
    </div>
  );
});
```

## Advanced Variations

### Lazy-Loaded Bootstrap Modal

Bootstrap modal that only loads its JavaScript when the user triggers it.

```tsx
import { component$, useSignal, $, useVisibleTask$ } from '@builder.io/qwik';

export default component$(() => {
  const modalRef = useSignal<HTMLDivElement>();
  const showModal = useSignal(false);

  const openModal = $(async () => {
    showModal.value = true;
    const { Modal } = await import('bootstrap/js/dist/modal');
    if (modalRef.value) {
      const modal = new Modal(modalRef.value);
      modal.show();
    }
  });

  return (
    <>
      <button class="btn btn-primary" onClick$={openModal}>
        <i class="bi bi-box-arrow-up-right me-2"></i>Open Modal
      </button>

      {showModal.value && (
        <div class="modal fade" ref={modalRef} tabIndex={-1}>
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">Lazy Modal</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
              </div>
              <div class="modal-body">
                <p>This modal's JavaScript was loaded only when you clicked the button.</p>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  );
});
```

### Qwik City with Bootstrap Forms

Server-side form handling with Bootstrap form validation.

```tsx
import { component$, useSignal } from '@builder.io/qwik';
import { routeAction$, Form } from '@builder.io/qwik-city';

export const useContactAction = routeAction$(async (data) => {
  const name = data.get('name') as string;
  const email = data.get('email') as string;
  const message = data.get('message') as string;

  if (!name || !email || !message) {
    return { success: false, error: 'All fields are required' };
  }

  return { success: true, message: `Thanks ${name}! We'll be in touch.` };
});

export default component$(() => {
  const action = useContactAction();

  return (
    <div class="row justify-content-center">
      <div class="col-md-6">
        <div class="card">
          <div class="card-header">
            <h5 class="mb-0">Contact Us</h5>
          </div>
          <div class="card-body">
            {action.value?.success ? (
              <div class="alert alert-success">{action.value.message}</div>
            ) : (
              <Form action={action} class="needs-validation">
                {action.value?.error && (
                  <div class="alert alert-danger">{action.value.error}</div>
                )}
                <div class="mb-3">
                  <label class="form-label" for="name">Name</label>
                  <input type="text" class="form-control" id="name" name="name" required />
                </div>
                <div class="mb-3">
                  <label class="form-label" for="email">Email</label>
                  <input type="email" class="form-control" id="email" name="email" required />
                </div>
                <div class="mb-3">
                  <label class="form-label" for="message">Message</label>
                  <textarea class="form-control" id="message" name="message" rows="4" required></textarea>
                </div>
                <button type="submit" class="btn btn-primary">Send</button>
              </Form>
            )}
          </div>
        </div>
      </div>
    </div>
  );
});
```

### Resumable Bootstrap Tabs

```tsx
import { component$, useSignal } from '@builder.io/qwik';

export default component$(() => {
  const activeTab = useSignal('home');

  return (
    <div class="card">
      <div class="card-header">
        <ul class="nav nav-tabs card-header-tabs">
          {['home', 'profile', 'settings'].map(tab => (
            <li class="nav-item" key={tab}>
              <button
                class={`nav-link ${activeTab.value === tab ? 'active' : ''}`}
                onClick$={() => activeTab.value = tab}
              >
                {tab.charAt(0).toUpperCase() + tab.slice(1)}
              </button>
            </li>
          ))}
        </ul>
      </div>
      <div class="card-body">
        {activeTab.value === 'home' && <p>Home content loaded resumably.</p>}
        {activeTab.value === 'profile' && <p>Profile content with zero hydration cost.</p>}
        {activeTab.value === 'settings' && <p>Settings panel - interactive on demand.</p>}
      </div>
    </div>
  );
});
```

## Best Practices

1. Use `useSignal` for reactive state instead of `useStore` for primitive values
2. Wrap event handlers with `$()` to enable lazy loading of handler code
3. Import Bootstrap JS components dynamically with `await import()` inside event handlers
4. Use Qwik's `<Form>` component for progressive enhancement of form submissions
5. Avoid `useVisibleTask$` when possible as it triggers client-side JavaScript
6. Use `routeAction$` and `routeLoader$` for server-side data fetching
7. Keep Bootstrap CSS loaded globally rather than component-scoped for consistency
8. Use Qwik's `component$` suffix convention for all components
9. Prefer server-side rendering with `onGet` for initial page load
10. Use `class` attribute instead of `className` in Qwik JSX
11. Defer Bootstrap tooltip and popover initialization until user interaction
12. Use `qwik-inspect` during development to verify resumability
13. Test that Bootstrap components work correctly without JavaScript enabled
14. Minimize `useVisibleTask$` to avoid defeating Qwik's zero-JS advantage

## Common Pitfalls

1. **Using `className`**: Qwik uses `class` not `className` like React
2. **Missing `$` suffix**: Forgetting `$()` on event handlers breaks lazy loading
3. **Eager imports**: Importing Bootstrap JS at the top level loads it on every page
4. **Hydration thinking**: Applying React/Vue hydration patterns that Qwik does not need
5. **Store overuse**: Using `useStore` for simple values when `useSignal` is sufficient
6. **Client-only components**: Writing components that assume browser APIs without guards
7. **Missing QRL types**: TypeScript errors from missing Qwik-specific type definitions

## Accessibility Considerations

Qwik's server-rendered HTML includes all Bootstrap ARIA attributes before JavaScript loads. Test that resumable components do not lose ARIA state during the resume phase. Use Qwik's `aria-*` attributes directly in JSX (prefixed with `aria-`). Ensure Bootstrap modals and dropdowns maintain `role` attributes after lazy loading. Verify that keyboard navigation works in zero-JS mode for static content. Announce dynamic content changes with `aria-live` regions using Qwik's `useSignal` reactivity.

```tsx
<div role="alert" aria-live="polite">
  {message.value}
</div>
```

## Responsive Behavior

Bootstrap's responsive grid works identically in Qwik. Qwik does not alter CSS output, so all `col-*`, `d-*`, and responsive utility classes function normally. Use standard Bootstrap responsive breakpoints. Verify that server-rendered HTML includes correct responsive classes before client-side JavaScript loads. Test responsive layouts by disabling JavaScript to confirm pure CSS responsive behavior. Qwik's zero-JS approach means responsive layout works even without JavaScript enabled.
