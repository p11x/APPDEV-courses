---
title: "Bootstrap Wrapper Library"
difficulty: 3
category: "Advanced Development"
subcategory: "Custom Frameworks"
prerequisites:
  - Bootstrap 5 Components
  - JavaScript ES6+
  - API Design Principles
---

## Overview

Creating a wrapper library around Bootstrap 5 involves building an abstraction layer that simplifies component usage, enforces consistency, and provides a cleaner API for development teams. A well-designed wrapper library reduces boilerplate, standardizes component patterns, and enables rapid development without sacrificing the underlying power of Bootstrap.

The core philosophy behind a wrapper library is to provide opinionated defaults while maintaining flexibility. Rather than exposing every Bootstrap option, you curate a focused API that covers 90% of use cases with minimal configuration. This approach reduces cognitive load, prevents misconfiguration, and accelerates onboarding for new developers.

Key architectural decisions include choosing between class-based and functional patterns, determining the level of abstraction, and establishing how customization flows through the component hierarchy. The wrapper should feel like a natural extension of Bootstrap rather than a replacement, leveraging Bootstrap's robust CSS and JavaScript foundation.

## Basic Implementation

A simple wrapper component wraps Bootstrap markup with a JavaScript API that handles configuration, rendering, and event management.

```html
<!-- Usage of the wrapper library -->
<div id="alert-container"></div>
<script src="bootstrap-wrapper.js"></script>
<script>
  const alert = BootstrapWrapper.alert({
    type: 'success',
    message: 'Operation completed successfully!',
    dismissible: true,
    container: '#alert-container'
  });
</script>
```

```js
// bootstrap-wrapper.js
class BootstrapWrapper {
  static alert(options) {
    const {
      type = 'info',
      message = '',
      dismissible = true,
      container = null,
      icon = null
    } = options;

    const alertEl = document.createElement('div');
    const classes = ['alert', `alert-${type}`, 'fade', 'show'];
    if (dismissible) classes.push('alert-dismissible');

    alertEl.className = classes.join(' ');
    alertEl.setAttribute('role', 'alert');

    let content = '';
    if (icon) content += `<i class="${icon} me-2"></i>`;
    content += message;
    if (dismissible) {
      content += `<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>`;
    }
    alertEl.innerHTML = content;

    const target = container ? document.querySelector(container) : document.body;
    target.appendChild(alertEl);

    const bsAlert = new bootstrap.Alert(alertEl);
    return {
      element: alertEl,
      close: () => bsAlert.close(),
      update: (msg) => { alertEl.querySelector('.me-2')?.after?.(msg) || (alertEl.innerHTML = content.replace(message, msg)); }
    };
  }

  static modal(options) {
    const {
      title = 'Modal',
      body = '',
      size = '',
      centered = false,
      backdrop = true,
      onConfirm = null,
      onCancel = null
    } = options;

    const modalId = `bw-modal-${Date.now()}`;
    const sizeClass = size ? `modal-${size}` : '';
    const centeredClass = centered ? 'modal-dialog-centered' : '';

    const modalHtml = `
      <div class="modal fade" id="${modalId}" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog ${sizeClass} ${centeredClass}">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">${title}</h5>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">${body}</div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button type="button" class="btn btn-primary bw-confirm">Confirm</button>
            </div>
          </div>
        </div>
      </div>`;

    document.body.insertAdjacentHTML('beforeend', modalHtml);
    const modalEl = document.getElementById(modalId);
    const bsModal = new bootstrap.Modal(modalEl, { backdrop });

    if (onConfirm) {
      modalEl.querySelector('.bw-confirm').addEventListener('click', () => {
        onConfirm();
        bsModal.hide();
      });
    }
    if (onCancel) {
      modalEl.addEventListener('hidden.bs.modal', onCancel, { once: true });
    }

    modalEl.addEventListener('hidden.bs.modal', () => modalEl.remove(), { once: true });

    return {
      show: () => bsModal.show(),
      hide: () => bsModal.hide(),
      element: modalEl
    };
  }
}
```

## Advanced Variations

Advanced wrapper implementations incorporate configuration objects, plugin systems, and factory patterns for scalable component generation.

```scss
// Wrapper-specific SCSS extending Bootstrap
@import 'bootstrap/scss/bootstrap';

$bw-defaults: (
  'animation-duration': 0.3s,
  'border-radius-scale': 1,
  'shadow-intensity': 'subtle'
);

@mixin bw-card-variant($variant) {
  $colors: map-get($bw-defaults, 'card-colors');
  border-left: 4px solid theme-color($variant);

  .bw-card__header {
    background-color: rgba(theme-color($variant), 0.1);
    border-bottom: 1px solid rgba(theme-color($variant), 0.2);
  }
}

.bw-card {
  @extend .card;
  border-radius: calc(#{$border-radius} * map-get($bw-defaults, 'border-radius-scale'));
  transition: transform map-get($bw-defaults, 'animation-duration') ease,
              box-shadow map-get($bw-defaults, 'animation-duration') ease;

  &:hover {
    transform: translateY(-2px);
    box-shadow: $box-shadow-lg;
  }

  &--primary { @include bw-card-variant('primary'); }
  &--success { @include bw-card-variant('success'); }
  &--warning { @include bw-card-variant('warning'); }
  &--danger  { @include bw-card-variant('danger'); }
}
```

```js
// Advanced: Component Factory Pattern
class ComponentFactory {
  constructor(config = {}) {
    this.defaults = config.defaults || {};
    this.plugins = new Map();
    this.middleware = [];
  }

  use(name, plugin) {
    this.plugins.set(name, plugin);
    plugin.install?.(this);
    return this;
  }

  addMiddleware(fn) {
    this.middleware.push(fn);
    return this;
  }

  create(type, options) {
    const config = { ...this.defaults[type], ...options };
    this.middleware.forEach(fn => fn(type, config));

    const plugin = this.plugins.get(type);
    if (!plugin) throw new Error(`Unknown component type: ${type}`);
    return plugin.create(config);
  }

  registerComponent(name, definition) {
    this.use(name, {
      create: (config) => {
        const el = document.createElement(definition.tag || 'div');
        el.className = definition.baseClass;

        if (definition.render) {
          el.innerHTML = definition.render(config);
        }
        if (definition.bind) {
          definition.bind(el, config);
        }
        return el;
      }
    });
  }
}

const factory = new ComponentFactory();
factory.registerComponent('dataTable', {
  tag: 'div',
  baseClass: 'table-responsive',
  render: (config) => {
    const headers = config.columns.map(c => `<th>${c.label}</th>`).join('');
    const rows = config.data.map(row => {
      const cells = config.columns.map(c => `<td>${row[c.key]}</td>`).join('');
      return `<tr>${cells}</tr>`;
    }).join('');
    return `<table class="table table-hover"><thead><tr>${headers}</tr></thead><tbody>${rows}</tbody></table>`;
  },
  bind: (el, config) => {
    if (config.sortable) {
      el.querySelectorAll('th').forEach(th => {
        th.style.cursor = 'pointer';
        th.addEventListener('click', () => config.onSort?.(th.textContent));
      });
    }
  }
});
```

## Best Practices

1. **Maintain Bootstrap compatibility** - Never override Bootstrap core classes; extend them with namespaced prefixes (e.g., `bw-`) to avoid collisions.
2. **Use composition over inheritance** - Build complex components by composing simpler wrapper components rather than deep inheritance chains.
3. **Provide sensible defaults** - Every configuration option should have a sensible default so components work with zero configuration.
4. **Support progressive enhancement** - Components should degrade gracefully when JavaScript fails to load or execute.
5. **Document every public API** - Each method, property, and event must be documented with types, defaults, and examples.
6. **Version your API surface** - Use semantic versioning and deprecation warnings before breaking changes.
7. **Keep the wrapper thin** - The wrapper should orchestrate Bootstrap, not reimplement it. Delegate CSS to Bootstrap classes.
8. **Implement proper cleanup** - Every component that creates DOM elements or attaches listeners must expose a `destroy()` method.
9. **Use consistent naming conventions** - Adopt a naming pattern (e.g., `bw-componentname`) and apply it to classes, data attributes, and JavaScript identifiers.
10. **Test against Bootstrap updates** - Maintain a test suite that validates wrapper behavior across Bootstrap minor and patch releases.
11. **Expose escape hatches** - Allow advanced users to bypass the wrapper and access underlying Bootstrap instances directly.
12. **Minimize bundle impact** - The wrapper should add minimal overhead; use tree-shaking and selective imports.

## Common Pitfalls

1. **Over-abstraction** - Wrapping every Bootstrap feature leads to a massive API surface that's harder to learn than Bootstrap itself. Only abstract genuinely repetitive or error-prone patterns.
2. **Hardcoding Bootstrap markup** - Embedding full HTML templates in JavaScript makes maintenance difficult. Use template literals or separate template files.
3. **Ignoring Bootstrap's JavaScript API** - Reimplementing modal show/hide or dropdown logic instead of delegating to `bootstrap.Modal`, `bootstrap.Dropdown`, etc.
4. **Breaking Bootstrap's event system** - Not propagating Bootstrap's native events (`show.bs.modal`, `shown.bs.collapse`) through the wrapper, causing integration issues.
5. **Missing accessibility attributes** - Failing to include `aria-*` attributes, `role` attributes, and keyboard navigation that Bootstrap provides by default.
6. **Version coupling** - Tightly coupling to a specific Bootstrap version makes upgrades painful. Abstract version-specific behavior behind adapters.

## Accessibility Considerations

Wrapper libraries must preserve and enhance Bootstrap's built-in accessibility features. When creating custom components, ensure all ARIA attributes, keyboard navigation, and focus management from Bootstrap remain intact. Always test with screen readers (NVDA, VoiceOver) and keyboard-only navigation.

```html
<!-- Accessible wrapper component with proper ARIA -->
<div class="bw-toast-container" aria-live="polite" aria-atomic="true">
  <div class="bw-toast alert alert-success" role="status" aria-label="Success notification">
    <span class="bw-toast__icon" aria-hidden="true">
      <svg width="20" height="20"><use href="#icon-check"/></svg>
    </span>
    <span class="bw-toast__message">Your changes have been saved.</span>
    <button class="btn-close" aria-label="Dismiss notification" type="button"></button>
  </div>
</div>
```

Maintain focus trapping in modals, return focus to trigger elements on close, and ensure dynamically inserted content maintains logical tab order. Wrapper components that modify Bootstrap's default behavior must explicitly handle focus management.

## Responsive Behavior

Wrapper components should inherit Bootstrap's responsive utilities and grid system. When creating custom responsive patterns, leverage Bootstrap's breakpoint mixins and ensure wrapper components respect the same breakpoint conventions.

```scss
// Responsive wrapper card grid
.bw-card-grid {
  display: grid;
  gap: map-get($spacers, 3);
  grid-template-columns: 1fr;

  @include media-breakpoint-up(sm) {
    grid-template-columns: repeat(2, 1fr);
  }

  @include media-breakpoint-up(lg) {
    grid-template-columns: repeat(3, 1fr);
  }

  @include media-breakpoint-up(xl) {
    grid-template-columns: repeat(4, 1fr);
  }
}
```

Ensure that wrapper components expose responsive configuration options (e.g., `collapseBelow: 'md'`) rather than hardcoding breakpoint values. This allows teams to adapt the wrapper to different design requirements without modifying source code.
