# Migration Guides

## OVERVIEW

Migration guides for transitioning from various frameworks and older Web Component versions to modern implementations.

## Framework Migration Paths

### From Polymer to Lit
```javascript
// Polymer style
Polymer({
  is: 'my-element',
  properties: { value: String }
});

// Lit style
import { LitElement, html } from 'lit';
export class MyElement extends LitElement {
  static properties = { value: String };
}
```

### From Stencil to Lit
```javascript
// Stencil
@Component({ tag: 'my-element' })
export class MyElement {
  @Prop() value: string;
}

// Lit equivalent
export class MyElement extends LitElement {
  @property() value = '';
}
```

### v0 to v1 Migration
```javascript
// v0 (deprecated)
document.registerElement('my-element', { extends: 'button' });

// v1
class MyElement extends HTMLButtonElement {}
customElements.define('my-element', MyElement, { extends: 'button' });
```

## Guide Completion

This concludes the comprehensive Web Components guide with 94 files across 14 sections.