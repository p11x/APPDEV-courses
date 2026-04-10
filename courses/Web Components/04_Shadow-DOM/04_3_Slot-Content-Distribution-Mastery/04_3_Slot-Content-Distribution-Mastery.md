# Slot Content Distribution Mastery

## OVERVIEW

Slots enable content projection in Shadow DOM, allowing light DOM content to appear within the shadow tree. This guide covers named slots, default content, slot API, and advanced patterns.

## TECHNICAL SPECIFICATIONS

### Slot Types

| Type | Description | Syntax |
|------|-------------|--------|
| Default | Unnamed slot for all content | `<slot></slot>` |
| Named | Specific content by slot attribute | `<slot name="header">` |
| Fallback | Default content when no projection | Default text in slot |

## IMPLEMENTATION DETAILS

### Basic Slot Usage

```javascript
class BasicSlotElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = `
      <style>
        :host { display: block; }
        .wrapper { border: 1px solid #ccc; padding: 16px; }
      </style>
      <div class="wrapper">
        <slot></slot>
      </div>
    `;
  }
}
```

```html
<!-- Usage -->
<basic-slot-element>
  <p>This content goes into the slot</p>
</basic-slot-element>
```

### Named Slots

```javascript
class NamedSlotElement extends HTMLElement {
  get template() {
    const t = document.createElement('template');
    t.innerHTML = `
      <style>
        :host { display: block; }
        header { background: #f5f5f5; padding: 12px; font-weight: bold; }
        footer { background: #fafafa; padding: 12px; border-top: 1px solid #eee; }
        main { padding: 16px; }
      </style>
      <header><slot name="header">Default Header</slot></header>
      <main><slot></slot></main>
      <footer><slot name="footer"></slot></footer>
    `;
    return t;
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    const clone = this.template.content.cloneNode(true);
    this.shadowRoot.appendChild(clone);
  }
}
```

```html
<!-- Usage -->
<named-slot-element>
  <span slot="header">Custom Header</span>
  <div>Main content</div>
  <button slot="footer">Action</button>
</named-slot-element>
```

## CODE EXAMPLES

### Slot Change Detection

```javascript
class SlotMonitor extends HTMLElement {
  #slots = [];
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.shadowRoot.innerHTML = `
      <slot></slot>
      <slot name="extra"></slot>
    `;
    
    this.#slots = Array.from(this.shadowRoot.querySelectorAll('slot'));
    this.#slots.forEach(slot => {
      slot.addEventListener('slotchange', () => this.#handleSlotChange(slot));
    });
  }
  
  #handleSlotChange(slot) {
    const nodes = slot.assignedNodes();
    console.log(`Slot "${slot.name}" changed:`, nodes.length, 'nodes');
  }
}
```

### Conditional Slot Rendering

```javascript
class ConditionalSlotElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.render();
  }
  
  render() {
    const hasHeader = this.querySelector('[slot="header"]');
    const hasFooter = this.querySelector('[slot="footer"]');
    
    this.shadowRoot.innerHTML = `
      <style>:host { display: block; }</style>
      ${hasHeader ? '<div class="header"><slot name="header"></slot></div>' : ''}
      <div class="body"><slot></slot></div>
      ${hasFooter ? '<div class="footer"><slot name="footer"></slot></div>' : ''}
    `;
  }
}
```

## NEXT STEPS

Proceed to **04_Shadow-DOM/04_4_Event-Bubbling-and-Targeting** for event handling.