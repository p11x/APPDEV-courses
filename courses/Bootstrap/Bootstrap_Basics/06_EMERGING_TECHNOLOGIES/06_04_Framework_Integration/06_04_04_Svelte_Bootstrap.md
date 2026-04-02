---
title: "Svelte Bootstrap Integration"
topic: "Framework Integration"
difficulty: 3
duration: "40 minutes"
prerequisites: ["Svelte basics", "Svelte stores", "SvelteKit fundamentals"]
tags: ["svelte", "bootstrap", "svelte-stores", "reactive", "components"]
---

## Overview

Svelte does not have an official Bootstrap component library equivalent to React Bootstrap or NG-Bootstrap. Instead, integration involves combining Bootstrap 5's CSS with Svelte's reactive paradigm — using Svelte's class directives, stores, transitions, and component system to manage Bootstrap component behavior. Libraries like `sveltestrap` provide pre-built Svelte components for common Bootstrap widgets, while custom integration leverages Svelte's compiler advantages for minimal runtime overhead.

Svelte's reactive class binding (`class:active={condition}`), two-way binding (`bind:checked`), and built-in transition system (`transition:fade`, `transition:slide`) naturally complement Bootstrap's interactive components. The framework's compile-time approach means no virtual DOM overhead, producing optimized vanilla JavaScript that manipulates Bootstrap's CSS classes and ARIA attributes directly.

## Basic Installation

### With SvelteKit

```bash
npm create svelte@latest my-svelte-bootstrap
cd my-svelte-bootstrap
npm install bootstrap @popperjs/core
npm install --save-dev sass
```

Add Bootstrap CSS globally:

```svelte
<!-- src/routes/+layout.svelte -->
<script>
  import '../scss/app.scss';
</script>

<slot />
```

```scss
// src/scss/app.scss
$primary: #7c3aed;
@import 'bootstrap/scss/bootstrap';
```

### With Sveltestrap

```bash
npm install sveltestrap
```

```svelte
<!-- src/routes/+page.svelte -->
<script>
  import { Container, Row, Col, Card, CardBody, CardTitle, CardText, Button } from 'sveltestrap';
</script>

<Container class="py-5">
  <Row>
    <Col md="8" class="mx-auto">
      <Card>
        <CardBody>
          <CardTitle>Svelte + Bootstrap</CardTitle>
          <CardText>Bootstrap components in Svelte.</CardText>
          <Button color="primary">Get Started</Button>
        </CardBody>
      </Card>
    </Col>
  </Row>
</Container>
```

## Advanced Variations

### Custom Bootstrap Components with Reactive Classes

```svelte
<!-- src/lib/components/Modal.svelte -->
<script>
  import { createEventDispatcher, onMount, onDestroy } from 'svelte';
  import { fly, fade } from 'svelte/transition';

  export let open = false;
  export let title = '';
  export let size = '';
  export let centered = false;

  const dispatch = createEventDispatcher();

  let modalEl;

  $: sizeClass = size ? `modal-${size}` : '';

  function close() {
    open = false;
    dispatch('close');
  }

  function handleKeydown(e) {
    if (e.key === 'Escape') close();
  }

  $: if (open) {
    document.body.classList.add('modal-open');
    document.addEventListener('keydown', handleKeydown);
  } else {
    document.body.classList.remove('modal-open');
    document.removeEventListener('keydown', handleKeydown);
  }

  onDestroy(() => {
    document.body.classList.remove('modal-open');
    document.removeEventListener('keydown', handleKeydown);
  });
</script>

{#if open}
  <div class="modal fade show d-block" tabindex="-1" role="dialog"
       aria-modal="true" bind:this={modalEl}
       transition:fade={{ duration: 150 }}>
    <div class="modal-dialog {sizeClass}" class:modal-dialog-centered={centered}
         transition:fly={{ y: 20, duration: 200 }}>
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">{title}</h5>
          <button type="button" class="btn-close" on:click={close}
                  aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <slot />
        </div>
        <div class="modal-footer">
          <slot name="footer">
            <button class="btn btn-secondary" on:click={close}>Close</button>
          </slot>
        </div>
      </div>
    </div>
  </div>
  <div class="modal-backdrop fade show" transition:fade={{ duration: 150 }}
       on:click={close}></div>
{/if}
```

### Svelte Store for Toast Notifications

```js
// src/lib/stores/toasts.js
import { writable } from 'svelte/store';

function createToastStore() {
  const { subscribe, update } = writable([]);

  function show({ message, variant = 'info', duration = 5000 }) {
    const id = Date.now();
    update(toasts => [...toasts, { id, message, variant, visible: true }]);

    if (duration > 0) {
      setTimeout(() => dismiss(id), duration);
    }

    return id;
  }

  function dismiss(id) {
    update(toasts => toasts.map(t =>
      t.id === id ? { ...t, visible: false } : t
    ));
    setTimeout(() => {
      update(toasts => toasts.filter(t => t.id !== id));
    }, 300);
  }

  return { subscribe, show, dismiss };
}

export const toasts = createToastStore();
```

```svelte
<!-- src/lib/components/ToastContainer.svelte -->
<script>
  import { toasts } from '$lib/stores/toasts';
  import { fly } from 'svelte/transition';
</script>

<div class="toast-container position-fixed bottom-0 end-0 p-3">
  {#each $toasts as toast (toast.id)}
    <div class="toast show" class:bg-{toast.variant}={toast.variant !== 'light'}
         role="alert" aria-live="assertive" aria-atomic="true"
         transition:fly={{ x: 200, duration: 300 }}>
      <div class="toast-header">
        <strong class="me-auto">Notification</strong>
        <button type="button" class="btn-close" on:click={() => toasts.dismiss(toast.id)}></button>
      </div>
      <div class="toast-body">{toast.message}</div>
    </div>
  {/each}
</div>
```

### Reactive Theme Switching

```svelte
<!-- src/lib/components/ThemeToggle.svelte -->
<script>
  import { writable } from 'svelte/store';

  export const theme = writable('light');

  function toggle() {
    theme.update(t => {
      const next = t === 'light' ? 'dark' : 'light';
      document.documentElement.setAttribute('data-bs-theme', next);
      return next;
    });
  }
</script>

<button class="btn btn-outline-secondary" on:click={toggle}
        aria-label="Toggle theme">
  {#if $theme === 'light'}
    <span>Dark Mode</span>
  {:else}
    <span>Light Mode</span>
  {/if}
</button>
```

## Best Practices

1. **Use `sveltestrap`** for well-tested component patterns; build custom components only when needed.
2. **Leverage `class:` directive** for conditional Bootstrap class application instead of string interpolation.
3. **Use `bind:` two-way binding** for form elements (`bind:value`, `bind:checked`, `bind:group`).
4. **Use Svelte's built-in transitions** (`fly`, `fade`, `slide`) instead of Bootstrap's CSS transitions for smoother animations.
5. **Keep Bootstrap CSS import global** in the layout file rather than per-component to avoid duplicate CSS.
6. **Use Svelte stores** for shared state (toasts, modals, theme) across components.
7. **Use `$:` reactive declarations** to recompute Bootstrap classes based on state changes.
8. **Set `onDestroy` cleanup** for event listeners and DOM class modifications (e.g., `modal-open` on body).
9. **Use `{#if}` blocks** to conditionally render Bootstrap components rather than hiding with CSS.
10. **Configure `svelte.config.js`** with `vitePreprocess()` for SCSS support in Svelte components.

## Common Pitfalls

1. **Not removing `modal-open` class from body** when component unmounts, leaving the page scrollable-locked.
2. **Using `class:` on Svelte components** that don't expose a `class` prop — use `$$props.class` forwarding.
3. **Forgetting `{#key}` block** when re-rendering Bootstrap components that depend on reactive data.
4. **Missing `@popperjs/core`** for tooltip/dropdown positioning when building custom popover components.
5. **Not using `svelte:window`** for responsive breakpoint detection when conditionally rendering mobile/desktop Bootstrap layouts.

## Accessibility Considerations

Svelte's `aria-*` attribute binding (`aria-expanded={isOpen}`) provides dynamic ARIA state management. Use `role` attributes in Svelte templates (`role="dialog"`, `role="alert"`). The `bind:this` directive enables focus management for modals and drawers. Svelte transitions respect `prefers-reduced-motion` through CSS media queries. Use `on:keydown` handlers for keyboard navigation in custom Bootstrap components (Escape to close, arrow keys in dropdowns).

## Responsive Behavior

Svelte components apply Bootstrap's responsive classes through the `class` directive. Use reactive variables to track viewport width:

```svelte
<script>
  let innerWidth;
</script>

<svelte:window bind:innerWidth />

<div class="row">
  <div class="col-12" class:col-md-6={innerWidth >= 768} class:col-lg-4={innerWidth >= 992}>
    Responsive content
  </div>
</div>
```

SvelteKit's `+layout.svelte` files handle responsive navigation with Bootstrap's `navbar-expand-*` classes. Use `{#if innerWidth >= 768}` blocks to render different Bootstrap layouts for mobile vs desktop.