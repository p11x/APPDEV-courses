# Shadow DOM Debugging Guide

## OVERVIEW

Debugging Shadow DOM requires understanding the isolated nature of the shadow tree and using appropriate browser dev tools. This guide covers techniques for inspecting, debugging, and troubleshooting Shadow DOM issues.

## IMPLEMENTATION DETAILS

### Browser DevTools Debugging

```javascript
// Inspect shadow root in console
const element = document.querySelector('my-element');
console.log(element.shadowRoot);

// Query within shadow DOM
const button = element.shadowRoot.querySelector('button');

// Use $0 in DevTools Elements panel
// Select element in shadow DOM by clicking
```

### Common Debugging Techniques

```javascript
class DebugElement extends HTMLElement {
  // Add logging for debugging
  connectedCallback() {
    console.log('[DebugElement] Connected', {
      id: this.id,
      attributes: this.attributes,
      parent: this.parentElement?.tagName
    });
    this.render();
  }
  
  // Debug attribute changes
  attributeChangedCallback(name, oldVal, newVal) {
    console.log(`[DebugElement] ${name}: ${oldVal} -> ${newVal}`);
  }
}
```

## NEXT STEPS

Proceed to `04_Shadow-DOM/04_8_Cross-Shadow-DOM-Communication.md`.