# Accessibility in Shadow DOM

## OVERVIEW

Building accessible components requires special attention in Shadow DOM where standard accessibility patterns may not work as expected. This guide covers ARIA in Shadow DOM, focus management, keyboard navigation, and screen reader compatibility.

## TECHNICAL SPECIFICATIONS

### Accessibility in Shadow DOM

| Feature | Shadow DOM Behavior |
|---------|---------------------|
| ARIA attributes | Work normally |
| Focus management | Use standard methods |
| Screen readers | Need composed events |
| role attribute | Should be on shadow elements |

## IMPLEMENTATION DETAILS

### ARIA in Shadow DOM

```javascript
class AccessibleElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = `
      <style>
        [role="button"] {
          padding: 8px 16px;
          cursor: pointer;
          user-select: none;
        }
      </style>
      <div 
        role="button" 
        tabindex="0" 
        aria-pressed="false"
        aria-label="Toggle feature">
        <slot></slot>
      </div>
    `;
    
    const btn = this.shadowRoot.querySelector('[role="button"]');
    btn.addEventListener('click', () => this.toggle());
    btn.addEventListener('keydown', (e) => this.#handleKey(e));
  }
  
  toggle() {
    const btn = this.shadowRoot.querySelector('[role="button"]');
    const pressed = btn.getAttribute('aria-pressed') === 'true';
    btn.setAttribute('aria-pressed', !pressed);
  }
  
  #handleKey(e) {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      this.toggle();
    }
  }
}
```

### Focus Management

```javascript
class FocusableElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = `
      <div class="container" tabindex="0" role="listbox">
        <div class="item" role="option" tabindex="-1">Option 1</div>
        <div class="item" role="option" tabindex="-1">Option 2</div>
        <div class="item" role="option" tabindex="-1">Option 3</div>
      </div>
    `;
  }
  
  focus() {
    const container = this.shadowRoot.querySelector('[tabindex]');
    container?.focus();
  }
}
```

### Live Region Announcements

```javascript
class AnnouncingElement extends HTMLElement {
  #announce = null;
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = `
      <div aria-live="polite" aria-atomic="true" class="sr-only"></div>
      <div class="content"><slot></slot></div>
    `;
    
    this.#announce = this.shadowRoot.querySelector('[aria-live]');
  }
  
  announce(message) {
    this.#announce.textContent = '';
    setTimeout(() => {
      this.#announce.textContent = message;
    }, 100);
  }
}
```

## NEXT STEPS

Proceed to **04_Shadow-DOM/04_6_Performance-Optimization-Techniques** for performance.