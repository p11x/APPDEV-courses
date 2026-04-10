# Svelte Integration Methods

## OVERVIEW

Svelte provides excellent Web Component support. This guide covers using custom elements in Svelte and wrapping Svelte components as custom elements.

## IMPLEMENTATION DETAILS

### Using Web Components in Svelte

```svelte
<!-- Using custom element -->
<script>
  let value = '';
  
  function handleChange(e) {
    value = e.detail.value;
  }
</script>

<my-element
  bind:value
  on:change={handleChange}
/>
```

### Svelte as Custom Element

```svelte
<!-- MyComponent.svelte -->
<script>
  export let name = 'World';
</script>

<div>Hello {name}!</div>

<!-- Compile as custom element -->
<svelte:options customElement="my-component" />
```

## NEXT STEPS

Proceed to **08_Interoperability/08_6_Micro-Frontend-Architecture**.