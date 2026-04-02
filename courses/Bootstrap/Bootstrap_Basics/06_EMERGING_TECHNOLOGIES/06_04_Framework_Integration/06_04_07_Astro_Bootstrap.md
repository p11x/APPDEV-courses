---
title: "Astro Bootstrap Integration"
topic: "Framework Integration"
difficulty: 3
duration: "45 minutes"
prerequisites: ["Astro fundamentals", "Island architecture", "Partial hydration"]
tags: ["astro", "bootstrap", "islands", "partial-hydration", "ssg"]
---

## Overview

Astro's island architecture enables a unique approach to Bootstrap integration: static Bootstrap HTML renders at build time with zero JavaScript, while interactive Bootstrap components hydrate as isolated "islands" only where needed. This results in extremely fast page loads because the majority of Bootstrap markup (grids, cards, typography, navigation) ships as pure HTML/CSS, and JavaScript is loaded only for interactive widgets like modals, dropdowns, and carousels.

Astro supports any UI framework for islands — React with `react-bootstrap`, Vue with `bootstrap-vue-next`, or Svelte with `sveltestrap`. Bootstrap's CSS can be imported globally or per-component. The `client:*` directives (`client:load`, `client:visible`, `client:idle`) control when island JavaScript hydrates, giving fine-grained performance control.

## Basic Implementation

```bash
npm create astro@latest my-astro-bootstrap
cd my-astro-bootstrap
npm install bootstrap @popperjs/core
npm install react react-dom react-bootstrap
```

Import Bootstrap CSS globally:

```astro
---
// src/layouts/BaseLayout.astro
import 'bootstrap/dist/css/bootstrap.min.css';
---

<html lang="en">
  <head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>{Astro.props.title}</title>
  </head>
  <body>
    <slot />
  </body>
</html>
```

Static Bootstrap page:

```astro
---
// src/pages/index.astro
import BaseLayout from '../layouts/BaseLayout.astro';
import InteractiveModal from '../components/InteractiveModal.tsx';
---

<BaseLayout title="Astro + Bootstrap">
  <div class="container py-5">
    <div class="row">
      <div class="col-md-8 mx-auto">
        <div class="card">
          <div class="card-header">
            <h5 class="mb-0">Astro + Bootstrap</h5>
          </div>
          <div class="card-body">
            <h5 class="card-title">Static Island Architecture</h5>
            <p class="card-text">
              This card ships with zero JavaScript. The modal below hydrates on load.
            </p>
            <InteractiveModal client:load />
          </div>
        </div>
      </div>
    </div>
  </div>
</BaseLayout>
```

## Advanced Variations

### Multi-Framework Islands

```astro
---
// src/pages/dashboard.astro
import BaseLayout from '../layouts/BaseLayout.astro';
import ReactModal from '../components/ReactModal.tsx';
import VueDropdown from '../components/VueDropdown.vue';
import SvelteAlert from '../components/SvelteAlert.svelte';

const stats = [
  { label: 'Users', value: 1234 },
  { label: 'Revenue', value: '$56,789' },
  { label: 'Orders', value: 890 },
];
---

<BaseLayout title="Dashboard">
  <div class="container py-5">
    <div class="row g-4">
      {stats.map(stat => (
        <div class="col-md-4">
          <div class="card text-center">
            <div class="card-body">
              <h2 class="display-4">{stat.value}</h2>
              <p class="text-muted">{stat.label}</p>
            </div>
          </div>
        </div>
      ))}
    </div>

    <!-- Hydrate when visible in viewport -->
    <ReactModal client:visible />

    <!-- Hydrate on user interaction -->
    <VueDropdown client:idle />

    <!-- Always hydrate immediately -->
    <SvelteAlert client:load />
  </div>
</BaseLayout>
```

### React Interactive Modal Island

```tsx
// src/components/ReactModal.tsx
import { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

export default function ReactModal() {
  const [show, setShow] = useState(false);

  return (
    <>
      <Button variant="primary" onClick={() => setShow(true)}>
        Open React Modal
      </Button>
      <Modal show={show} onHide={() => setShow(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title>Interactive Island</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>This modal hydrated as a React island.</p>
          <Form.Group>
            <Form.Label>Your Name</Form.Label>
            <Form.Control type="text" placeholder="Enter name" />
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShow(false)}>Close</Button>
          <Button variant="primary" onClick={() => setShow(false)}>Save</Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}
```

### Custom SCSS Theme

```astro
---
// src/layouts/BaseLayout.astro
import '../styles/main.scss';
---
```

```scss
// src/styles/main.scss
$primary: #7c3aed;
$enable-rounded: true;

@import 'bootstrap/scss/bootstrap';
```

### Astro Component with Bootstrap Slots

```astro
---
// src/components/BSPageHeader.astro
interface Props {
  title: string;
  subtitle?: string;
  variant?: string;
}

const { title, subtitle, variant = 'primary' } = Astro.props;
---

<div class={`bg-${variant} text-white py-5 mb-4`}>
  <div class="container">
    <h1 class="display-4">{title}</h1>
    {subtitle && <p class="lead mb-0">{subtitle}</p>}
    <slot name="actions" />
  </div>
</div>
```

## Best Practices

1. **Use Astro components (`.astro`) for static Bootstrap markup** to ship zero JavaScript.
2. **Use `client:visible`** for below-the-fold interactive islands to defer JavaScript until scroll.
3. **Use `client:idle`** for non-critical interactive components (tooltips, popovers) to defer until browser idle.
4. **Use `client:load`** only for above-the-fold interactive elements that need immediate interactivity.
5. **Import Bootstrap CSS in the layout** for global availability without per-component duplication.
6. **Use multiple UI frameworks** for different islands if needed — Astro's architecture supports it.
7. **Create Astro components** for reusable static Bootstrap patterns (cards, heroes, sections).
8. **Use `<ViewTransitions />`** for SPA-like navigation that preserves Bootstrap component state.
9. **Configure `vite.css.preprocessorOptions.scss`** in `astro.config.mjs` for global SCSS settings.
10. **Use `astro:build:done` hook** to post-process Bootstrap CSS (purging, minification).

## Common Pitfalls

1. **Using `client:load` everywhere** negates Astro's partial hydration advantage — all islands load JavaScript immediately.
2. **Importing `react-bootstrap` in `.astro` files** — the library must only be used inside framework components.
3. **Missing `@popperjs/core`** breaks tooltip/dropdown positioning in hydrated islands.
4. **Not specifying the correct `client:*` directive** results in hydration errors or missed interactivity.
5. **Mixing static `.astro` Bootstrap markup with `react-bootstrap`** causes class name or structure inconsistencies.

## Accessibility Considerations

Astro's static-first approach ensures that semantic Bootstrap HTML and ARIA attributes are in the initial server-rendered output, providing full accessibility before any JavaScript loads. Static navigation, headings, and landmark regions are immediately available to screen readers. Interactive islands add ARIA state management (aria-expanded, aria-modal) during hydration. Use `aria-label` in Astro components for server-rendered landmarks. The `client:visible` directive ensures interactive accessibility features are available before the user reaches them.

## Responsive Behavior

Bootstrap's responsive CSS classes work identically in Astro's static templates and hydrated islands. The CSS is included globally, so all responsive utilities (`col-md-6`, `d-lg-none`, `fs-sm-5`) are available. Astro components render responsive Bootstrap grid layouts at build time, producing static HTML with the correct responsive classes. Interactive islands inherit the same responsive CSS context. Use Astro's `<Picture>` component with Bootstrap's `.img-fluid` for responsive optimized images with automatic format selection.