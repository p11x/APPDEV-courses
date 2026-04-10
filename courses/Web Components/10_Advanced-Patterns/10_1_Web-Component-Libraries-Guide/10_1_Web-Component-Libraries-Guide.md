# Web Component Libraries Guide

## OVERVIEW

This guide surveys popular Web Component libraries and their use cases. Understanding library capabilities helps you choose the right tool for your project.

## Popular Libraries

| Library | Size | Features | Use Case |
|---------|------|----------|----------|
| Lit | ~5KB | Reactive, templates | General purpose |
| Stencil | ~20KB | JSX, SSR | Enterprise |
| FAST | ~6KB | FAST design system | Modern apps |
| Hybrids | ~4KB | Proxy-based | Lightweight |

## Lit Usage

```javascript
import { LitElement, html, css } from 'lit';

export class MyElement extends LitElement {
  static styles = css`
    :host { display: block; }
  `;
  
  static properties = {
    value: { type: String }
  };
  
  render() {
    return html`<div>${this.value}</div>`;
  }
}
customElements.define('my-element', MyElement);
```

## NEXT STEPS

Proceed to **10_Advanced-Patterns/10_2_Component-Versioning-Strategies**.