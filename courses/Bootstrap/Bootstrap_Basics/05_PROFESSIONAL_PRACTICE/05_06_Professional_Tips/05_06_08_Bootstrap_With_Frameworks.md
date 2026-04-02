---
title: "Bootstrap with JavaScript Frameworks"
description: "Integrate Bootstrap 5 with React, Vue, and Angular using framework-specific libraries and component patterns."
difficulty: 3
tags: ["bootstrap", "react", "vue", "angular", "frameworks", "react-bootstrap"]
prerequisites: ["05_01_Introduction", "05_03_Components", "05_05_Advanced"]
---

# Bootstrap with JavaScript Frameworks

## Overview

Bootstrap's CSS works with any framework, but its JavaScript components (modals, dropdowns, tooltips) conflict with virtual DOM-based frameworks that manage their own DOM. Framework-specific libraries — `react-bootstrap`, `bootstrap-vue-next`, and `ng-bootstrap` — provide native component wrappers that integrate with each framework's lifecycle, state management, and rendering model. Choosing the right integration approach depends on whether you need Bootstrap's CSS only, its full component suite, or a hybrid approach.

## Basic Implementation

**React with react-bootstrap** replaces Bootstrap's jQuery-era JS with React components that manage state through React's ecosystem.

```jsx
// Install: npm install react-bootstrap bootstrap
import 'bootstrap/dist/css/bootstrap.min.css';
import { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

function App() {
  const [show, setShow] = useState(false);
  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  return (
    <div className="container py-5">
      <Button variant="primary" onClick={handleShow}>
        Open Modal
      </Button>

      <Modal show={show} onHide={handleClose} centered>
        <Modal.Header closeButton>
          <Modal.Title>Contact Form</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group className="mb-3">
              <Form.Label>Email address</Form.Label>
              <Form.Control type="email" placeholder="name@example.com" />
            </Form.Group>
            <Form.Group className="mb-3">
              <Form.Label>Message</Form.Label>
              <Form.Control as="textarea" rows={3} />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>Cancel</Button>
          <Button variant="primary" onClick={handleClose}>Submit</Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
}
```

**Vue with bootstrap-vue-next** provides Vue 3 components wrapping Bootstrap 5.

```vue
<!-- Install: npm install bootstrap-vue-next bootstrap -->
<template>
  <div class="container py-5">
    <BButton @click="showModal = true" variant="primary">
      Open Modal
    </BButton>

    <BModal v-model="showModal" title="Contact Form" centered>
      <BForm>
        <BFormGroup label="Email" class="mb-3">
          <BFormInput type="email" placeholder="name@example.com" />
        </BFormGroup>
        <BFormGroup label="Message" class="mb-3">
          <BFormTextarea rows="3" />
        </BFormGroup>
      </BForm>
      <template #footer="{ hide }">
        <BButton variant="secondary" @click="hide()">Cancel</BButton>
        <BButton variant="primary" @click="hide()">Submit</BButton>
      </template>
    </BModal>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { BButton, BModal, BForm, BFormGroup, BFormInput, BFormTextarea } from 'bootstrap-vue-next';
import 'bootstrap/dist/css/bootstrap.min.css';

const showModal = ref(false);
</script>
```

## Advanced Variations

**Angular with ng-bootstrap** provides native Angular widgets with no jQuery or Bootstrap JS dependency.

```typescript
// Install: npm install @ng-bootstrap/ng-bootstrap bootstrap
// app.module.ts
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [NgbModule],
})
export class AppModule {}

// app.component.ts
import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-root',
  template: `
    <div class="container py-5">
      <button class="btn btn-primary" (click)="open(content)">Open Modal</button>

      <ng-template #content let-modal>
        <div class="modal-header">
          <h5 class="modal-title">Contact Form</h5>
          <button type="button" class="btn-close" (click)="modal.dismiss()"></button>
        </div>
        <div class="modal-body">
          <div class="mb-3">
            <label class="form-label">Email</label>
            <input type="email" class="form-control" placeholder="name@example.com">
          </div>
          <div class="mb-3">
            <label class="form-label">Message</label>
            <textarea class="form-control" rows="3"></textarea>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary" (click)="modal.close()">Cancel</button>
          <button class="btn btn-primary" (click)="modal.close()">Submit</button>
        </div>
      </ng-template>
    </div>
  `
})
export class AppComponent {
  constructor(private modalService: NgbModal) {}
  open(content: any) { this.modalService.open(content, { centered: true }); }
}
```

**CSS-only integration** works when you only need Bootstrap's grid and utilities, avoiding JS component libraries entirely.

```jsx
// React: CSS-only approach — no react-bootstrap needed
import 'bootstrap/dist/css/bootstrap.min.css';

function CardGrid({ items }) {
  return (
    <div className="container py-5">
      <div className="row g-4">
        {items.map(item => (
          <div key={item.id} className="col-sm-6 col-lg-4">
            <div className="card h-100 border-0 shadow-sm">
              <img src={item.image} className="card-img-top" alt={item.title} />
              <div className="card-body">
                <h5 className="card-title">{item.title}</h5>
                <p className="card-text text-muted">{item.description}</p>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
```

## Best Practices

1. Use framework-specific libraries (react-bootstrap, ng-bootstrap) for interactive components
2. Use Bootstrap CSS directly for layout and utility classes without a wrapper library
3. Import only the Bootstrap components you use to reduce bundle size
4. Never load Bootstrap's full JS bundle alongside framework component libraries
5. Use CSS Modules or scoped styles to prevent Bootstrap class collisions
6. Leverage framework state management for modal/dropdown visibility instead of DOM manipulation
7. Configure tree-shaking in your bundler to eliminate unused Bootstrap exports
8. Use TypeScript with framework-specific Bootstrap libraries for type safety
9. Test component behavior with framework testing utilities (React Testing Library, Vue Test Utils)
10. Keep Bootstrap and its framework wrapper on compatible version tracks
11. Use `react-bootstrap`'s `as` prop or Vue slots for polymorphic component rendering

## Common Pitfalls

1. **Loading Bootstrap JS and react-bootstrap simultaneously** — Both attempt to manage the same DOM elements, causing conflicts and duplicate event handlers.

2. **Mixing direct DOM manipulation with framework rendering** — Using `document.querySelector` in React/Vue bypasses the virtual DOM and causes state inconsistencies.

3. **Not configuring CSS module scoping** — Bootstrap's global classes can leak across component boundaries in module-based architectures.

4. **Ignoring framework lifecycle in modals** — Modals opened via Bootstrap JS don't trigger React/Vue lifecycle hooks. Use framework-specific modal components.

5. **Version mismatches** — `react-bootstrap` v2 targets Bootstrap 5. Using it with Bootstrap 4 CSS produces broken components.

6. **Importing full Bootstrap CSS in SSR frameworks** — In Next.js or Nuxt, importing Bootstrap globally can cause hydration mismatches. Use dynamic imports or CSS-in-JS solutions.

## Accessibility Considerations

Framework-specific Bootstrap libraries maintain ARIA attributes and keyboard navigation, but custom implementations must replicate this behavior. When building custom components with only Bootstrap CSS, implement focus trapping for modals, `aria-expanded` toggling for dropdowns, and `role` attributes manually. Test with keyboard-only navigation and screen readers to ensure parity with Bootstrap's native accessibility.

## Responsive Behavior

Bootstrap's responsive utilities work identically regardless of framework. Use `className` (React), `class` (Vue), or `class` (Angular) bindings to apply responsive utility classes dynamically. Framework-specific conditional rendering can complement responsive utilities — for example, rendering a `<BSidebar>` component only on desktop while showing a `<BOffcanvas>` on mobile. Avoid duplicating responsive logic between CSS utilities and framework conditionals.
