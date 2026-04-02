---
title: "Vue Bootstrap Setup"
topic: "Framework Integration"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Vue 3 fundamentals", "Composition API", "SFC syntax"]
tags: ["vue", "bootstrap-vue-next", "vue3", "composition-api", "components"]
---

## Overview

BootstrapVueNext provides Vue 3 components for Bootstrap 5, offering a complete component library that maps Bootstrap's HTML elements and jQuery interactions to Vue directives and components. The library leverages Vue 3's Composition API, Teleport for modals/dropdowns, and the reactivity system for state management. It replaces Bootstrap's JavaScript entirely with Vue-native implementations.

The project is the community successor to `bootstrap-vue` (which supported Vue 2). Components like `<BModal>`, `<BDropdown>`, `<BNavbar>`, and `<BTabs>` handle accessibility, animation, and keyboard navigation. Bootstrap's CSS is imported separately, and all Bootstrap utility classes work in Vue templates via the `class` binding.

## Basic Installation

```bash
npm create vue@latest my-vue-bootstrap-app
cd my-vue-bootstrap-app
npm install bootstrap bootstrap-vue-next @popperjs/core
```

Register the plugin globally:

```js
// src/main.js
import { createApp } from 'vue';
import { createBootstrap } from 'bootstrap-vue-next';
import App from './App.vue';

import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue-next/dist/bootstrap-vue-next.css';

const app = createApp(App);
app.use(createBootstrap());
app.mount('#app');
```

Basic component usage:

```vue
<!-- src/App.vue -->
<template>
  <BContainer class="py-5">
    <BRow>
      <BCol md="8" class="mx-auto">
        <BCard header="Vue Bootstrap">
          <BCardTitle>Component-based Bootstrap for Vue</BCardTitle>
          <BCardText>
            Bootstrap 5 components as Vue 3 primitives.
          </BCardText>
          <BButton variant="primary" @click="showToast">
            Show Toast
          </BButton>
        </BCard>
      </BCol>
    </BRow>
    <BToastOrchestrator />
  </BContainer>
</template>

<script setup>
import { useToast } from 'bootstrap-vue-next';

const toast = useToast();

function showToast() {
  toast.show({
    title: 'Hello!',
    body: 'Toast notification from Vue Bootstrap.',
    variant: 'success',
  });
}
</script>
```

## Advanced Variations

### Composition API Patterns

```vue
<!-- src/components/DataTable.vue -->
<template>
  <BTable
    :items="items"
    :fields="fields"
    :per-page="perPage"
    :current-page="currentPage"
    striped
    hover
    responsive
  >
    <template #cell(actions)="{ item }">
      <BButton size="sm" variant="outline-primary" @click="edit(item)">
        Edit
      </BButton>
      <BButton size="sm" variant="outline-danger" @click="remove(item.id)">
        Delete
      </BButton>
    </template>
  </BTable>
  <BPagination
    v-model="currentPage"
    :total-rows="items.length"
    :per-page="perPage"
    class="mt-3"
  />
</template>

<script setup>
import { ref, computed } from 'vue';

const props = defineProps({
  data: { type: Array, default: () => [] },
});

const emit = defineEmits(['edit', 'delete']);

const perPage = ref(10);
const currentPage = ref(1);

const fields = [
  { key: 'id', label: 'ID', sortable: true },
  { key: 'name', label: 'Name', sortable: true },
  { key: 'email', label: 'Email' },
  { key: 'actions', label: 'Actions' },
];

const items = computed(() => props.data);

function edit(item) { emit('edit', item); }
function remove(id) { emit('delete', id); }
</script>
```

### SCSS Theme Customization

```vue
<!-- src/App.vue -->
<style lang="scss">
// Import custom Bootstrap overrides
@import './scss/variables';
@import 'bootstrap/scss/bootstrap';
</style>
```

```scss
// src/scss/_variables.scss
$primary: #8b5cf6;
$enable-rounded: true;
$enable-shadows: true;
$theme-colors: (
  'brand': #f59e0b,
);
```

### Composables for Bootstrap State

```js
// src/composables/useModal.js
import { ref } from 'vue';

export function useModal() {
  const isOpen = ref(false);

  function open() { isOpen.value = true; }
  function close() { isOpen.value = false; }
  function toggle() { isOpen.value = !isOpen.value; }

  return { isOpen, open, close, toggle };
}
```

```vue
<!-- src/components/ConfirmDialog.vue -->
<template>
  <BModal v-model="isOpen" :title="title" @ok="onConfirm" @cancel="onCancel">
    <slot />
  </BModal>
</template>

<script setup>
import { useModal } from '@/composables/useModal';

defineProps({ title: String });
const emit = defineEmits(['confirm', 'cancel']);
const { isOpen, open, close } = useModal();

function onConfirm() { emit('confirm'); close(); }
function onCancel() { emit('cancel'); close(); }

defineExpose({ open, close });
</script>
```

## Best Practices

1. **Register `bootstrap-vue-next` globally** via `createBootstrap()` for automatic component availability in all templates.
2. **Use `B` prefixed components** (`BButton`, `BModal`) — these are the Vue 3 equivalents of Bootstrap's HTML elements.
3. **Import Bootstrap CSS separately** from component CSS to control versioning and customization.
4. **Use `v-model`** on modals, dropdowns, and toggles for reactive open/close state management.
5. **Leverage slots** (`#cell`, `#header`, `#default`) for customizing component internals.
6. **Use composables** (`useToast`, `useModal`) from `bootstrap-vue-next` for programmatic control.
7. **Keep `@popperjs/core`** installed — Bootstrap Vue relies on it for dropdown and tooltip positioning.
8. **Use `<Teleport to="body">`** (or rely on BVue's built-in teleport) for modals to avoid z-index stacking issues.
9. **Bind dynamic classes** with `:class` for conditional Bootstrap utility application.
10. **Use `<BFormValidFeedback>` and `<BFormInvalidFeedback>`** for accessible form validation states.

## Common Pitfalls

1. **Importing `bootstrap-vue` (Vue 2 version)** instead of `bootstrap-vue-next` (Vue 3) causes runtime errors.
2. **Forgetting `BToastOrchestrator`** in the root component prevents `useToast()` from displaying notifications.
3. **Using `data-bs-*` attributes** instead of Vue props — Bootstrap Vue manages state through props, not data attributes.
4. **Not installing `@popperjs/core`** breaks tooltip and popover positioning.
5. **Mixing jQuery Bootstrap plugins** with BootstrapVue components causes duplicate event handling.

## Accessibility Considerations

BootstrapVueNext components handle ARIA attribute management. `<BModal>` adds `role="dialog"`, `aria-modal="true"`, and manages focus trapping. `<BDropdown>` sets `aria-expanded` and manages keyboard navigation. `<BTabs>` uses `role="tablist"` with proper `aria-controls` and `aria-selected` attributes. Use the `aria-label` and `aria-labelledby` props on interactive components. The library follows WAI-ARIA Authoring Practices for all widget patterns.

## Responsive Behavior

BootstrapVue's grid components (`BContainer`, `BRow`, `BCol`) accept breakpoint props matching Bootstrap's CSS system. Use `<BCol sm="6" lg="4">` for responsive column sizing. The `BTable` component's `responsive` prop wraps it in a scrollable container on small screens. `BCollapse` and `BOffcanvas` components work across breakpoints with configurable `breakpoint` props for responsive collapse behavior.