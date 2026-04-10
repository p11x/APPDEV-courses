# 🟢 Vue State Management Complete Guide

## Pinia for Vue.js Applications

---

## Table of Contents

1. [Introduction to Pinia](#introduction-to-pinia)
2. [Pinia Setup](#pinia-setup)
3. [Store Creation](#store-creation)
4. [Getters](#getters)
5. [Actions](#actions)
6. [Plugins](#plugins)
7. [Real-World Examples](#real-world-examples)

---

## Introduction to Pinia

### Why Pinia?

Pinia is the official state management solution for Vue.js. It's lighter and more intuitive than Vuex.

```bash
npm install pinia
```

---

## Pinia Setup

### Basic Setup

```javascript
// main.js
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.mount('#app')
```

---

## Store Creation

### Option Store

```javascript
// stores/counter.js
import { defineStore } from 'pinia'

export const useCounterStore = defineStore('counter', {
  state: () => ({
    count: 0
  }),
  getters: {
    double: (state) => state.count * 2
  },
  actions: {
    increment() {
      this.count++
    }
  }
})
```

### Setup Store

```javascript
// stores/setupStore.js
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useSetupStore = defineStore('counter', () => {
  const count = ref(0)
  
  const double = computed(() => count.value * 2)
  
  function increment() {
    count.value++
  }
  
  return { count, double, increment }
})
```

---

## Getters

### Basic Getters

```javascript
// stores/user.js
import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', {
  state: () => ({
    users: [
      { id: 1, name: 'John', age: 25 },
      { id: 2, name: 'Jane', age: 30 }
    ]
  }),
  getters: {
    userCount: (state) => state.users.length,
    adults: (state) => state.users.filter(u => u.age >= 18),
    getUserById: (state) => (id) => state.users.find(u => u.id === id)
  }
})
```

### Getters with Parameters

```javascript
getters: {
  getUserById: (state) => (id) => state.users.find(u => u.id === id)
}
```

---

## Actions

### Basic Actions

```javascript
// stores/counter.js
import { defineStore } from 'pinia'

export const useCounterStore = defineStore('counter', {
  state: () => ({
    count: 0
  }),
  actions: {
    increment() {
      this.count++
    },
    decrement() {
      this.count--
    },
    reset() {
      this.count = 0
    }
  }
})
```

### Async Actions

```javascript
// stores/user.js
import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', {
  state: () => ({
    users: [],
    loading: false
  }),
  actions: {
    async fetchUsers() {
      this.loading = true
      try {
        const response = await fetch('/api/users')
        this.users = await response.json()
      } finally {
        this.loading = false
      }
    }
  }
})
```

---

## Plugins

### Persist Plugin

```bash
npm install pinia-plugin-persistedstate
```

```javascript
// main.js
import { createPinia } from 'pinia'
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'

const pinia = createPinia()
pinia.use(piniaPluginPersistedstate)

app.use(pinia)
```

```javascript
// stores/counter.js
export const useCounterStore = defineStore('counter', {
  state: () => ({ count: 0 }),
  persist: true
})
```

---

## Using Store in Components

### Basic Usage

```vue
<script setup>
import { useCounterStore } from '../stores/counter'

const store = useCounterStore()
</script>

<template>
  <div>
    <p>Count: {{ store.count }}</p>
    <p>Double: {{ store.double }}</p>
    <button @click="store.increment()">Increment</button>
  </div>
</template>
```

---

## Summary

### Key Takeaways

1. **Pinia**: Official Vue state solution
2. **Stores**: State, getters, actions
3. **Setup**: Option or setup stores
4. **Plugins**: Persistence support

### Next Steps

- Continue with: [05_VUE_TESTING_GUIDE.md](05_VUE_TESTING_GUIDE.md)
- Study store organization
- Implement complex flows

---

## Cross-References

- **Previous**: [03_VUE_ROUTING_MASTER.md](03_VUE_ROUTING_MASTER.md)
- **Next**: [05_VUE_TESTING_GUIDE.md](05_VUE_TESTING_GUIDE.md)

---

*Last updated: 2024*