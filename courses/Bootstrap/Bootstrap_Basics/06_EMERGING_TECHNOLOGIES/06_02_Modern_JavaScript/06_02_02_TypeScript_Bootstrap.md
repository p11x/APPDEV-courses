---
title: TypeScript with Bootstrap
category: Emerging Technologies
difficulty: 3
time: 30 min
tags: bootstrap5, typescript, type-definitions, generics, component-wrappers
---

## Overview

TypeScript adds static type safety to Bootstrap JavaScript interactions, catching errors at compile time rather than runtime. Bootstrap 5 ships with bundled type definitions, enabling IntelliSense for component options, methods, and events. Typed component wrappers provide additional safety by enforcing correct configuration and return types for Bootstrap's API.

## Basic Implementation

Bootstrap's type definitions enable full TypeScript support out of the box.

```ts
// Import Bootstrap with types
import { Modal, Dropdown, Toast, Tooltip } from 'bootstrap';
import type { ModalOptions, TooltipOptions } from 'bootstrap';

// Typed modal configuration
const modalOptions: ModalOptions = {
  backdrop: 'static',
  keyboard: false,
  focus: true
};

// Initialize with type safety
const modalElement = document.getElementById('exampleModal') as HTMLElement;
const modal = new Modal(modalElement, modalOptions);

// TypeScript catches invalid options at compile time
const badOptions: ModalOptions = {
  backdrop: 'invalid' // TS error: Type '"invalid"' is not assignable
};

// Typed event listeners
modalElement.addEventListener('show.bs.modal', (event: Event) => {
  const modalEvent = event as CustomEvent;
  console.log('Modal is showing', modalEvent.target);
});

// Type-safe method calls
function closeModal(): void {
  if (modal) {
    modal.hide();
  }
}
```

```html
<button class="btn btn-primary" type="button" id="openBtn">Open Modal</button>
<div class="modal fade" id="exampleModal" tabindex="-1"
     aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Typed Modal</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal"
                aria-label="Close"></button>
      </div>
      <div class="modal-body">TypeScript-safe Bootstrap modal.</div>
    </div>
  </div>
</div>
```

## Advanced Variations

Typed component wrappers add generic constraints, validation, and lifecycle hooks to Bootstrap components.

```ts
// Generic typed Bootstrap component wrapper
abstract class BootstrapComponentWrapper<TOptions, TInstance> {
  protected instance: TInstance | null = null;
  protected element: HTMLElement;

  constructor(
    elementOrSelector: HTMLElement | string,
    protected options: TOptions
  ) {
    this.element = typeof elementOrSelector === 'string'
      ? document.querySelector(elementOrSelector) as HTMLElement
      : elementOrSelector;

    if (!this.element) {
      throw new Error(`Element not found: ${elementOrSelector}`);
    }

    this.instance = this.createInstance();
  }

  protected abstract createInstance(): TInstance;
  abstract show(): void;
  abstract hide(): void;
  abstract dispose(): void;
}

// Typed Modal wrapper
interface TypedModalOptions {
  backdrop?: boolean | 'static';
  keyboard?: boolean;
  focus?: boolean;
}

class TypedModal extends BootstrapComponentWrapper<TypedModalOptions, Modal> {
  protected createInstance(): Modal {
    return new Modal(this.element, {
      backdrop: this.options.backdrop ?? true,
      keyboard: this.options.keyboard ?? true,
      focus: this.options.focus ?? true
    });
  }

  show(): void {
    this.instance?.show();
  }

  hide(): void {
    this.instance?.hide();
  }

  dispose(): void {
    this.instance?.dispose();
  }

  onShow(callback: (event: Event) => void): this {
    this.element.addEventListener('show.bs.modal', callback);
    return this;
  }

  onShown(callback: (event: Event) => void): this {
    this.element.addEventListener('shown.bs.modal', callback);
    return this;
  }

  onHide(callback: (event: Event) => void): this {
    this.element.addEventListener('hide.bs.modal', callback);
    return this;
  }
}

// Usage
const modal = new TypedModal('#exampleModal', {
  backdrop: 'static',
  keyboard: false
});

modal
  .onShow(() => console.log('Opening...'))
  .onShown(() => console.log('Opened'))
  .show();
```

```ts
// Typed toast factory
type ToastType = 'success' | 'danger' | 'warning' | 'info';

interface ToastConfig {
  type: ToastType;
  title: string;
  message: string;
  delay?: number;
}

function createTypedToast(config: ToastConfig): Toast {
  const container = document.getElementById('toastContainer')
    || createToastContainer();

  const toastEl = document.createElement('div');
  toastEl.className = `toast align-items-center text-bg-${config.type} border-0`;
  toastEl.setAttribute('role', 'alert');
  toastEl.setAttribute('aria-live', 'assertive');
  toastEl.setAttribute('aria-atomic', 'true');

  toastEl.innerHTML = `
    <div class="d-flex">
      <div class="toast-body">
        <strong>${config.title}</strong>: ${config.message}
      </div>
      <button type="button" class="btn-close btn-close-white me-2 m-auto"
              data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
  `;

  container.appendChild(toastEl);
  return new Toast(toastEl, { autohide: true, delay: config.delay ?? 5000 });
}
```

## Best Practices

1. Always install `bootstrap` package which includes type definitions
2. Use `import type` for type-only imports to enable better tree-shaking
3. Cast `document.getElementById` results as `HTMLElement` for Bootstrap component constructors
4. Create typed wrappers for frequently used component patterns
5. Use discriminated unions for component variants (e.g., `ToastType`)
6. Leverage TypeScript generics to create reusable component factories
7. Define interfaces for custom event payloads from Bootstrap events
8. Use `satisfies` operator to validate configuration objects against Bootstrap types
9. Enable `strict` mode in `tsconfig.json` for maximum type safety
10. Use `as const` assertions for string literal types in Bootstrap attributes

## Common Pitfalls

1. **Missing type definitions** - Ensure `@types/bootstrap` or Bootstrap's bundled types are accessible in your `tsconfig.json` paths.
2. **Null element assertions** - `document.getElementById` returns `Element | null`. Always null-check before passing to Bootstrap constructors.
3. **Event type casting** - Bootstrap events are `Event` type. Cast to `CustomEvent` for `detail` property access.
4. **Generic complexity** - Over-abstracting Bootstrap wrappers adds complexity without benefit. Keep wrappers simple.
5. **Module augmentation issues** - Extending Bootstrap types via `declare module` can conflict with updates. Prefer composition over augmentation.

## Accessibility Considerations

TypeScript wrappers should enforce accessibility requirements at the type level. Create interfaces that require `ariaLabel` properties for icon-only buttons, enforce `aria-labelledby` for modal dialogs, and validate that interactive elements have accessible names. Use TypeScript's type system to make accessibility the path of least resistance.

## Responsive Behavior

TypeScript does not affect responsive behavior directly, but typed breakpoint utilities improve responsive code reliability. Create typed helpers for Bootstrap breakpoint matching and ensure that responsive utility class names are validated at compile time to prevent typo-based layout issues.
