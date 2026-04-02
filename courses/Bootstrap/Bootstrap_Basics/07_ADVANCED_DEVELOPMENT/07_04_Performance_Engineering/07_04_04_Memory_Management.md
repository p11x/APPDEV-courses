---
title: "Memory Management"
difficulty: 3
category: "Advanced Development"
subcategory: "Performance Engineering"
prerequisites:
  - JavaScript Memory Model
  - WeakMap/WeakRef
  - Bootstrap 5 Disposal API
---

## Overview

Memory management in Bootstrap applications focuses on preventing leaks caused by event listeners, Bootstrap plugin instances, DOM node references, and timer callbacks that persist after components are removed from the DOM. Bootstrap 5 provides `dispose()` methods on all plugins, but developers must call them explicitly when removing components. Failure to do so creates memory leaks that degrade performance over time, especially in single-page applications.

The disposal pattern requires every component creation to have a corresponding destruction call. In SPAs using frameworks like React, Vue, or Alpine.js, this happens during component unmount lifecycle hooks. For vanilla JavaScript, manual tracking with WeakMap ensures proper cleanup.

## Basic Implementation

```js
// Proper Bootstrap disposal pattern
class ComponentManager {
  constructor() {
    this.instances = new WeakMap();
  }

  create(element, ComponentClass, config) {
    // Check if instance already exists
    if (this.instances.has(element)) {
      return this.instances.get(element);
    }

    const instance = new ComponentClass(element, config);
    this.instances.set(element, instance);
    return instance;
  }

  destroy(element) {
    const instance = this.instances.get(element);
    if (instance) {
      if (typeof instance.dispose === 'function') {
        instance.dispose();
      }
      this.instances.delete(element);
    }
  }

  destroyAll(container) {
    // Destroy all Bootstrap instances within a container
    const components = ['Modal', 'Dropdown', 'Tooltip', 'Popover', 'Collapse', 'Offcanvas'];

    components.forEach(name => {
      if (bootstrap[name]) {
        container.querySelectorAll(`[data-bs-toggle="${name.toLowerCase()}"]`).forEach(el => {
          const instance = bootstrap[name].getInstance(el);
          if (instance) instance.dispose();
        });
      }
    });
  }
}

// Usage in SPA component lifecycle
const manager = new ComponentManager();

// Component mount
function onMount(container) {
  container.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(el => {
    manager.create(el, bootstrap.Tooltip);
  });
  container.querySelectorAll('[data-bs-toggle="modal"]').forEach(el => {
    manager.create(el, bootstrap.Modal);
  });
}

// Component unmount - CRITICAL for memory management
function onUnmount(container) {
  manager.destroyAll(container);
}
```

```js
// Event listener cleanup
class ManagedComponent {
  constructor(element) {
    this.element = element;
    this._listeners = [];

    this._addEventListener(element, 'click', this._handleClick.bind(this));
    this._addEventListener(window, 'resize', this._handleResize.bind(this));
    this._addEventListener(document, 'keydown', this._handleKey.bind(this));
  }

  _addEventListener(target, event, handler, options) {
    target.addEventListener(event, handler, options);
    this._listeners.push({ target, event, handler, options });
  }

  dispose() {
    // Remove all tracked listeners
    this._listeners.forEach(({ target, event, handler, options }) => {
      target.removeEventListener(event, handler, options);
    });
    this._listeners = [];
    this.element = null;
  }
}
```

```js
// Timer cleanup
class PollingComponent {
  constructor(element) {
    this.element = element;
    this._intervals = [];
    this._timeouts = [];

    this._startPolling();
  }

  _setInterval(fn, ms) {
    const id = setInterval(fn, ms);
    this._intervals.push(id);
    return id;
  }

  _setTimeout(fn, ms) {
    const id = setTimeout(fn, ms);
    this._timeouts.push(id);
    return id;
  }

  _startPolling() {
    this._setInterval(() => this._fetchData(), 30000);
  }

  dispose() {
    this._intervals.forEach(id => clearInterval(id));
    this._timeouts.forEach(id => clearTimeout(id));
    this._intervals = [];
    this._timeouts = [];
  }
}
```

## Advanced Variations

```js
// WeakRef-based cleanup for large objects
class ImageCarousel {
  constructor(element) {
    this.element = element;
    this._imageCache = new Map();

    // Use WeakRef for cached images so they can be GC'd
    this._weakCache = new Map();
  }

  _cacheImage(src, img) {
    this._weakCache.set(src, new WeakRef(img));

    // Registry for cleanup when image is GC'd
    const registry = new FinalizationRegistry((src) => {
      this._weakCache.delete(src);
    });
    registry.register(img, src);
  }

  _getImage(src) {
    const ref = this._weakCache.get(src);
    if (ref) {
      const img = ref.deref();
      if (img) return img;
    }
    // Image was garbage collected, create new one
    const img = new Image();
    img.src = src;
    this._cacheImage(src, img);
    return img;
  }
}
```

```js
// React integration with Bootstrap disposal
import { useEffect, useRef } from 'react';
import { Modal, Tooltip } from 'bootstrap';

function BootstrapModal({ show, onClose, children }) {
  const modalRef = useRef(null);
  const instanceRef = useRef(null);

  useEffect(() => {
    if (modalRef.current) {
      instanceRef.current = new Modal(modalRef.current);
    }

    return () => {
      // Cleanup on unmount
      instanceRef.current?.dispose();
      instanceRef.current = null;
    };
  }, []);

  useEffect(() => {
    if (show) instanceRef.current?.show();
    else instanceRef.current?.hide();
  }, [show]);

  return (
    <div ref={modalRef} className="modal fade" tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content">
          {children}
        </div>
      </div>
    </div>
  );
}
```

## Best Practices

1. **Always call dispose()** - Every Bootstrap plugin instance must be disposed when removed.
2. **Track event listeners** - Maintain a list of all attached listeners for easy cleanup.
3. **Use WeakMap for instances** - Prevents memory leaks when DOM elements are removed.
4. **Clear timers on dispose** - Intervals and timeouts must be cleared during component destruction.
5. **Use WeakRef for caches** - Large cached objects should use WeakRef to allow garbage collection.
6. **Hook into SPA lifecycle** - Call dispose in componentWillUnmount, onUnmounted, or cleanup functions.
7. **Test with DevTools Memory tab** - Take heap snapshots before and after mounting/unmounting components.
8. **Avoid global event listeners** - Prefer component-scoped listeners that clean up with the component.
9. **Nullify references** - Set element references to null in dispose() to enable GC.
10. **Use FinalizationRegistry** - For cleanup of resources associated with garbage-collected objects.

## Common Pitfalls

1. **Not calling dispose()** - Bootstrap instances persist in memory after DOM removal.
2. **Event listener accumulation** - Adding listeners without removing them on every re-render.
3. **Timer leaks** - Intervals that continue running after component removal.
4. **Closure references** - Closures capturing DOM nodes prevent garbage collection.
5. **Framework-specific disposal** - Forgetting to clean up in useEffect cleanup or beforeUnmount.

## Accessibility Considerations

Memory leaks in focus management code can trap focus in removed components. Always clean up focus trap references during disposal.

## Responsive Behavior

Resize observers must be disconnected during disposal to prevent memory leaks from viewport change listeners.

```js
dispose() {
  if (this._resizeObserver) {
    this._resizeObserver.disconnect();
    this._resizeObserver = null;
  }
}
```
