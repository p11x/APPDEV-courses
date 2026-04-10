# Vue Integration Strategies

## OVERVIEW

Vue integration requires handling v-model, props, and events. This guide covers Vue component wrappers and direct usage.

## IMPLEMENTATION DETAILS

### Vue Component Wrapper

```javascript
// my-element.js - Web Component
class MyElement extends HTMLElement {
  static get observedAttributes() { return ['value', 'disabled']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  attributeChangedCallback() { this.render(); }
  
  connectedCallback() { this.render(); }
  
  get value() { return this.getAttribute('value'); }
  set value(val) { this.setAttribute('value', val); }
  
  render() {
    this.shadowRoot.innerHTML = `<div>${this.value}</div>`;
  }
}
customElements.define('my-element', MyElement);
```

```vue
<!-- Vue wrapper component -->
<template>
  <my-element
    ref="elementRef"
    :value="modelValue"
    v-bind="$attrs"
    @change="handleChange"
  />
</template>

<script>
export default {
  name: 'MyElement',
  props: {
    modelValue: String
  },
  emits: ['update:modelValue'],
  methods: {
    handleChange(e) {
      this.$emit('update:modelValue', e.detail.value);
    }
  }
}
</script>
```

## NEXT STEPS

Proceed to **08_Interoperability/08_4_Angular-Integration-Patterns**.