---
title: "Nuxt 3 Bootstrap Integration"
topic: "Framework Integration"
difficulty: 3
duration: "40 minutes"
prerequisites: ["Nuxt 3 fundamentals", "Vue 3 Composition API", "Nitro server"]
tags: ["nuxt", "bootstrap", "bootstrap-vue-next", "ssr", "modules"]
---

## Overview

Nuxt 3 is Vue 3's full-stack meta-framework with built-in SSR, file-based routing, and auto-imports. Integrating Bootstrap 5 involves using `bootstrap-vue-next` for Vue-native components and configuring Nuxt's module system for automatic component registration and SCSS compilation. Nuxt 3's `nuxt.config.ts` provides hooks for adding Bootstrap CSS, configuring Vite's SCSS preprocessor, and setting up SSR-safe component loading.

The key considerations are: SSR hydration safety (Bootstrap's browser-only code must be client-only), auto-import of `bootstrap-vue-next` components, SCSS variable customization through Nuxt's CSS configuration, and proper handling of Popper.js for tooltip/dropdown positioning during server rendering.

## Basic Implementation

```bash
npx nuxi@latest init my-nuxt-bootstrap
cd my-nuxt-bootstrap
npm install bootstrap bootstrap-vue-next @popperjs/core
npm install --save-dev sass
```

Configure `nuxt.config.ts`:

```ts
// nuxt.config.ts
export default defineNuxtConfig({
  css: [
    'bootstrap/dist/css/bootstrap.min.css',
    '~/assets/scss/main.scss',
  ],
  modules: ['bootstrap-vue-next/nuxt'],
  build: {
    transpile: ['bootstrap-vue-next'],
  },
  vite: {
    css: {
      preprocessorOptions: {
        scss: {
          additionalData: `$enable-deprecation-messages: false;`,
          silenceDeprecations: ['import', 'global-builtin'],
        },
      },
    },
  },
  compatibilityDate: '2024-11-01',
});
```

Create custom SCSS:

```scss
// assets/scss/main.scss
$primary: #8b5cf6;
$secondary: #64748b;

@import 'bootstrap/scss/bootstrap';
```

Basic page usage:

```vue
<!-- pages/index.vue -->
<template>
  <BContainer class="py-5">
    <BRow>
      <BCol md="8" class="mx-auto">
        <BCard header="Nuxt + Bootstrap">
          <BCardTitle>Server-Rendered Bootstrap</BCardTitle>
          <BCardText>
            Bootstrap 5 with Nuxt 3 SSR support.
          </BCardText>
          <BButton variant="primary" @click="count++">
            Clicks: {{ count }}
          </BButton>
        </BCard>
      </BCol>
    </BRow>
  </BContainer>
</template>

<script setup>
const count = ref(0);
</script>
```

## Advanced Variations

### Layout with Navigation

```vue
<!-- layouts/default.vue -->
<template>
  <div>
    <BNavbar variant="dark" expand="lg" class="bg-body-tertiary">
      <BContainer>
        <BNavbarBrand to="/">My App</BNavbarBrand>
        <BNavbarToggle target="nav-collapse" />
        <BCollapse id="nav-collapse" is-nav>
          <BNavbarNav class="ms-auto">
            <BNavItem to="/">Home</BNavItem>
            <BNavItem to="/about">About</BNavItem>
            <BNavItem to="/dashboard">Dashboard</BNavItem>
          </BNavbarNav>
        </BCollapse>
      </BContainer>
    </BNavbar>

    <main>
      <slot />
    </main>

    <footer class="bg-body-tertiary py-4 mt-5">
      <BContainer>
        <p class="text-center text-muted mb-0">
          &copy; {{ new Date().getFullYear() }} My App
        </p>
      </BContainer>
    </footer>

    <BToastOrchestrator />
  </div>
</template>
```

### Client-Only Modal Component

```vue
<!-- components/ClientModal.vue -->
<template>
  <ClientOnly>
    <BModal v-model="isOpen" :title="title" centered @ok="$emit('confirm')">
      <slot />
    </BModal>
    <template #fallback>
      <div class="d-none">Loading modal...</div>
    </template>
  </ClientOnly>
</template>

<script setup>
const props = defineProps({
  modelValue: Boolean,
  title: { type: String, default: 'Modal' },
});

const emit = defineEmits(['update:modelValue', 'confirm']);

const isOpen = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val),
});
</script>
```

### Nuxt Plugin for Bootstrap Initialization

```ts
// plugins/bootstrap.client.ts
export default defineNuxtPlugin((nuxtApp) => {
  nuxtApp.hook('app:mounted', () => {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipTriggerList.forEach(el => {
      new (window as any).bootstrap?.Tooltip?.(el);
    });
  });
});
```

### Composable for Toast Notifications

```ts
// composables/useNotifications.ts
import { useToast } from 'bootstrap-vue-next';

export function useNotifications() {
  const toast = useToast();

  function success(message: string) {
    toast.show({ title: 'Success', body: message, variant: 'success' });
  }

  function error(message: string) {
    toast.show({ title: 'Error', body: message, variant: 'danger' });
  }

  function info(message: string) {
    toast.show({ title: 'Info', body: message, variant: 'info' });
  }

  return { success, error, info };
}
```

## Best Practices

1. **Use `bootstrap-vue-next/nuxt` module** for automatic component registration and SSR handling.
2. **Use `<ClientOnly>` wrapper** for components that rely on browser APIs (tooltips, popovers, third-party widgets).
3. **Import Bootstrap CSS in `nuxt.config.ts` css array** for global availability across all pages.
4. **Use Nuxt's auto-imports** — `ref`, `computed`, `useRoute` are available without explicit imports.
5. **Configure Vite's `preprocessorOptions.scss`** in `nuxt.config.ts` for global SCSS variable injection.
6. **Place `BToastOrchestrator`** in the default layout so toast notifications work app-wide.
7. **Use `definePageMeta({ layout: false })`** for pages that need completely custom Bootstrap layouts.
8. **Keep `@popperjs/core`** as a dependency for correct tooltip/dropdown positioning.
9. **Use Nuxt's `useFetch`/`useAsyncData`** with Bootstrap table components for SSR data loading.
10. **Configure `build.transpile`** for `bootstrap-vue-next` to ensure proper SSR compilation.

## Common Pitfalls

1. **Missing `<ClientOnly>`** for browser-dependent components causes "document is not defined" SSR errors.
2. **Not setting `build.transpile: ['bootstrap-vue-next']`** causes ESM import errors during SSR.
3. **Forgetting `BToastOrchestrator`** in layout prevents `useToast()` from displaying notifications.
4. **Importing Bootstrap CSS from `assets/`** instead of `node_modules/` paths causes resolution failures.
5. **Mixing `bootstrap` JS with `bootstrap-vue-next`** creates duplicate event handlers and DOM conflicts.

## Accessibility Considerations

Nuxt 3's SSR ensures Bootstrap's semantic HTML and ARIA attributes are in the initial server-rendered payload, improving accessibility for screen readers. Use `useHead()` composable to set page titles and meta descriptions for each route. `bootstrap-vue-next` components manage ARIA attributes automatically. Use `definePageMeta({ accessibility: true })` patterns for custom accessibility annotations. Server-rendered Bootstrap markup ensures content is accessible even before JavaScript hydration completes.

## Responsive Behavior

Bootstrap's responsive grid works identically in Nuxt 3's SSR and client-rendered contexts. Use `BContainer`, `BRow`, `BCol` with breakpoint props (`sm`, `md`, `lg`). Nuxt's `<NuxtLayout>` component can render different layouts per breakpoint using `<ClientOnly>` with window width detection. The `BCollapse` component's `breakpoint` prop creates responsive collapsible navigation that expands at the specified breakpoint. All responsive CSS utilities are generated during Sass compilation and included in the server-rendered CSS.