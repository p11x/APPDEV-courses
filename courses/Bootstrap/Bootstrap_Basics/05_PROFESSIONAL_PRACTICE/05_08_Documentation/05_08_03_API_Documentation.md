---
title: "API Documentation for Bootstrap JavaScript"
module: "Documentation"
difficulty: 2
estimated_time: 25
tags: ["API", "JavaScript", "documentation", "methods", "events"]
prerequisites: ["Bootstrap JS components", "JavaScript ES6+"]
---

## Overview

Bootstrap's JavaScript components expose a programmatic API with options, methods, events, and constructor patterns. Documenting this API for your project ensures developers understand how to initialize, control, and respond to component behavior programmatically. This guide covers documenting the full API surface including configuration options, public methods, event callbacks, and return values.

## Basic Implementation

**Component Constructor Documentation**

Document how to create component instances with available options.

```markdown
## Modal API

### Constructor
```javascript
new bootstrap.Modal(element, options?)
```

### Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| `element` | `Element \| string` | DOM element or selector for the modal |
| `options` | `Object` | Configuration options (see below) |

### Options
| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `backdrop` | `boolean \| 'static'` | `true` | Show backdrop. `'static'` prevents closing on click |
| `focus` | `boolean` | `true` | Focus the modal when shown |
| `keyboard` | `boolean` | `true` | Close on Escape key press |
```

**Public Methods**

Document every public method with parameters and return values.

```markdown
### Methods

| Method | Parameters | Returns | Description |
|--------|-----------|---------|-------------|
| `show()` | none | `void` | Shows the modal |
| `hide()` | none | `void` | Hides the modal |
| `toggle()` | none | `void` | Toggles modal visibility |
| `dispose()` | none | `void` | Destroys the instance and removes data |
| `handleUpdate()` | none | `void` | Recalculates modal position |

### Static Methods
| Method | Parameters | Returns | Description |
|--------|-----------|---------|-------------|
| `getInstance(element)` | `Element` | `Modal \| null` | Returns existing instance |
| `getOrCreateInstance(element)` | `Element` | `Modal` | Returns or creates instance |
```

**Event Documentation**

Document all events with their payload and timing.

```markdown
### Events

| Event | Fires When | Event Property |
|-------|-----------|----------------|
| `show.bs.modal` | Before show | `relatedTarget` |
| `shown.bs.modal` | After show animation | `relatedTarget` |
| `hide.bs.modal` | Before hide | - |
| `hidden.bs.modal` | After hide animation | - |

### Event Usage
```javascript
const modal = document.getElementById('confirmModal');
modal.addEventListener('show.bs.modal', (event) => {
  const trigger = event.relatedTarget;
  console.log('Triggered by:', trigger);
});

modal.addEventListener('hidden.bs.modal', () => {
  // Cleanup logic after modal closes
  document.getElementById('modalForm').reset();
});
```
```

## Advanced Variations

**Dropdown API with Configuration**

Complex components have more configuration options and methods.

```markdown
## Dropdown API

### Options
| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `autoClose` | `boolean \| 'inside' \| 'outside'` | `true` | Auto-close behavior |
| `boundary` | `string \| Element` | `'clippingParents'` | Dropdown boundary for positioning |
| `display` | `'dynamic' \| 'static'` | `'dynamic'` | Popper display strategy |
| `offset` | `array \| function` | `[0, 2]` | Offset `[skidding, distance]` |
| `popperConfig` | `object \| function` | `null` | Popper.js configuration override |

### Programmatic Control
```javascript
const dropdown = new bootstrap.Dropdown('#myDropdown', {
  autoClose: 'outside',
  offset: [0, 8]
});

// Show with custom logic
dropdown.show();

// Update Popper configuration
dropdown.update();
```
```

**Tooltip API with Custom Content**

```markdown
## Tooltip API

### Options
| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `title` | `string \| function` | `''` | Tooltip content |
| `placement` | `string \| function` | `'top'` | Position: top, bottom, left, right |
| `trigger` | `string` | `'hover focus'` | Trigger events |
| `html` | `boolean` | `false` | Allow HTML in title |
| `sanitize` | `boolean` | `true` | Sanitize HTML content |

### Custom Content
```javascript
const tooltip = new bootstrap.Tooltip(element, {
  title: () => {
    const count = element.dataset.count;
    return `<strong>${count}</strong> items remaining`;
  },
  html: true,
  placement: 'auto'
});
```
```

## Best Practices

1. **Document every public method** - no method should be undocumented
2. **Include parameter types** - TypeScript-style type annotations improve clarity
3. **Show real-world usage examples** for each method and event
4. **Document default values** for all options
5. **Include return types** even when the return is `void`
6. **Document event payloads** - what data is available in event handlers
7. **Note deprecations** clearly with migration guidance
8. **Document static methods** like `getInstance()` and `getOrCreateInstance()`
9. **Include error behavior** - what happens with invalid inputs
10. **Document lifecycle** - the order of initialization, events, and disposal
11. **Link to Bootstrap's official API docs** as a reference
12. **Version-match documentation** with the Bootstrap version in use

## Common Pitfalls

1. **Missing event documentation** - developers cannot hook into component lifecycle
2. **Not documenting static methods** - `getInstance` is essential for checking existing instances
3. **Omitting default values** - developers cannot determine behavior without them
4. **No disposal documentation** - memory leaks from improper cleanup
5. **Forgetting `getOrCreateInstance`** - developers create duplicate instances
6. **Not documenting `popperConfig`** - advanced positioning needs are invisible
7. **Missing return type documentation** - unclear whether methods return values
8. **No examples for event handling** - developers guess at event payloads
9. **Undocumented auto-close behavior** - dropdowns and popovers close unexpectedly
10. **Ignoring the `Sanitizer` API** - custom HTML sanitization needs are undocumented

## Accessibility Considerations

Document keyboard interactions supported by each component. Note which ARIA attributes are set automatically by Bootstrap and which require manual addition. Document focus management behavior - where focus moves on show, on hide, and during keyboard navigation. Include information about screen reader announcements triggered by API methods.

## Responsive Behavior

Document how JavaScript API behavior changes at different viewport sizes. Note any options that affect responsive behavior (e.g., tooltip placement fallbacks). Document touch event handling on mobile devices and any differences from mouse interactions. Include information about mobile-specific initialization options.
