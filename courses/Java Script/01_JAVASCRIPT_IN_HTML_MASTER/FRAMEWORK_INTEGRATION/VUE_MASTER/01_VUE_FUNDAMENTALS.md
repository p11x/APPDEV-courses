# 🟢 Vue JavaScript Master Guide

## Comprehensive Vue.js Development for Professionals

---

## Table of Contents

1. [Introduction to Vue.js](#introduction-to-vuejs)
2. [Vue 3 Fundamentals](#vue-3-fundamentals)
3. [Template Syntax](#template-syntax)
4. [Reactivity System](#reactivity-system)
5. [Components Deep Dive](#components-deep-dive)
6. [Composition API](#composition-api)
7. [State Management](#state-management)
8. [Vue Router](#vue-router)
9. [Forms and Validation](#forms-and-validation)
10. [Best Practices](#best-practices)

---

## Introduction to Vue.js

### What is Vue.js?

Vue.js is a progressive JavaScript framework for building user interfaces. Created by Evan You in 2014, Vue combines the best features of Angular and React while remaining lightweight and flexible.

```
┌─────────────────────────────────────────────────────────────┐
│                  VUE.JS ECOSYSTEM                         │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐    │
│  │   Vue 3    │  │  Vue CLI   │  │   Vue Router   │    │
│  │  (Core)    │  │  (Build)  │  │  (Navigation)  │    │
│  └─────────────┘  └─────────────┘  └─────────────────┘    │
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐    │
│  │   Pinia    │  │  VueUse   │  │    Nuxt.js     │    │
│  │  (State)  │  │ (Utils)   │  │  (SSR/SSG)    │    │
│  └─────────────┘  └─────────────┘  └─────────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

### Why Learn Vue.js?

- **Easy Learning Curve**: Accessible for beginners
- **Flexibility**: Progressive from simple to complex
- **Performance**: Lightweight and fast
- **Community**: Large ecosystem and support

---

## Vue 3 Fundamentals

### Setup Vue 3

```bash
# Using npm
npm create vue@latest my-app

# Using Vite (recommended)
npm create vite@latest my-app -- --template vue
```

### Basic Component

```vue
<!-- App.vue -->
<script setup>
import { ref } from 'vue'

const count = ref(0)

function increment() {
  count.value++
}
</script>

<template>
  <div>
    <p>Count: {{ count }}</p>
    <button @click="increment">Increment</button>
  </div>
</template>
```

### Options API

```vue
<script>
export default {
  data() {
    return {
      message: 'Hello Vue!'
    }
  },
  methods: {
    greet() {
      console.log(this.message)
    }
  }
}
</script>

<template>
  <div>
    <p>{{ message }}</p>
    <button @click="greet">Greet</button>
  </div>
</template>
```

---

## Template Syntax

### Text Interpolation

```vue
<template>
  <div>
    <!-- Basic interpolation -->
    <p>{{ message }}</p>
    
    <!-- Expression -->
    <p>{{ message.toUpperCase() }}</p>
    
    <!-- Computed -->
    <p>{{ reversedMessage }}</p>
  </div>
</template>

<script>
export default {
  data() {
    return { message: 'Hello' }
  },
  computed: {
    reversedMessage() {
      return this.message.split('').reverse().join('')
    }
  }
}
</script>
```

### Directives

```vue
<template>
  <div>
    <!-- v-if conditional -->
    <p v-if="show">Shown</p>
    <p v-else>Hidden</p>
    
    <!-- v-for loop -->
    <ul>
      <li v-for="item in items" :key="item.id">
        {{ item.name }}
      </li>
    </ul>
    
    <!-- v-bind shorthand -->
    <img :src="imageUrl" :alt="description">
    
    <!-- v-on event -->
    <button @click="handleClick">Click</button>
    
    <!-- v-model -->
    <input v-model="inputValue">
    
    <!-- v-show -->
    <p v-show="show">Toggle</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      show: true,
      items: [{id: 1, name: 'Item 1'}],
      imageUrl: 'https://example.com/image.jpg',
      description: 'Image',
      inputValue: ''
    }
  },
  methods: {
    handleClick() {
      console.log('Clicked')
    }
  }
}
</script>
```

---

## Reactivity System

### Ref and Reactive

```javascript
import { ref, reactive, computed, watch } from 'vue'

// Ref for primitives
const count = ref(0)

// Reactive for objects
const state = reactive({
  user: null,
  loading: false
})

// Computed
const double = computed(() => count.value * 2)

// Watch
watch(count, (newVal, oldVal) => {
  console.log('Count changed:', newVal)
})
```

### Reactivity in Components

```vue
<script setup>
import { ref, onMounted, onUnmounted } from 'vue'

const count = ref(0)
const items = ref([])

function increment() {
  count.value++
}

onMounted(() => {
  console.log('Component mounted')
})

onUnmounted(() => {
  console.log('Component unmounted')
})
</script>

<template>
  <div>
    <p>Count: {{ count }}</p>
    <button @click="increment">Increment</button>
  </div>
</template>
```

---

## Components Deep Dive

### Props

```vue
<!-- ChildComponent.vue -->
<script setup>
defineProps({
  title: String,
  count: {
    type: Number,
    default: 0,
    required: true
  },
  items: {
    type: Array,
    default: () => []
  }
})
</script>

<template>
  <div>
    <h2>{{ title }}</h2>
    <p>Count: {{ count }}</p>
  </div>
</template>
```

```vue
<!-- ParentComponent.vue -->
<script setup>
import { ref } from 'vue'
import ChildComponent from './ChildComponent.vue'

const count = ref(5)
</script>

<template>
  <ChildComponent :count="count" title="My Component" />
</template>
```

### Events

```vue
<!-- ChildComponent.vue -->
<script setup>
const emit = defineEmits(['update', 'delete'])

function notify() {
  emit('update', 'new value')
}
</script>

<template>
  <button @click="notify">Notify</button>
</template>
```

```vue
<!-- ParentComponent.vue -->
<script setup>
function handleUpdate(value) {
  console.log('Updated:', value)
}
</script>

<template>
  <ChildComponent @update="handleUpdate" />
</template>
```

### Slots

```vue
<!-- CardComponent.vue -->
<template>
  <div class="card">
    <div class="header">
      <slot name="header">Default Header</slot>
    </div>
    <div class="body">
      <slot>Default Content</slot>
    </div>
    <div class="footer">
      <slot name="footer">Default Footer</slot>
    </div>
  </div>
</template>
```

```vue
<template>
  <CardComponent>
    <template #header>Card Title</template>
    <template #default>Content here</template>
  </CardComponent>
</template>
```

---

## Composition API

### Setup Function

```vue
<script>
import { ref, onMounted, onUnmounted } from 'vue'

export default {
  setup() {
    const count = ref(0)
    
    function increment() {
      count.value++
    }
    
    onMounted(() => {
      console.log('Mounted')
    })
    
    return { count, increment }
  }
}
</script>

<template>
  <div>
    <p>{{ count }}</p>
    <button @click="increment">Increment</button>
  </div>
</template>
```

### Composition API Best Practices

```vue
<script setup>
import { ref, computed, watch, onMounted } from 'vue'

// State
const count = ref(0)

// Computed
const double = computed(() => count.value * 2)

// Methods
function increment() {
  count.value++
}

// Watch
watch(count, (newVal) => {
  console.log('Count:', newVal)
})

// Lifecycle
onMounted(() => {
  console.log('Component mounted')
})
</script>

<template>
  <div>
    <p>{{ count }} x 2 = {{ double }}</p>
    <button @click="increment">Increment</button>
  </div>
</template>
```

### Reusable Composable

```javascript
// composables/useCounter.js
import { ref, computed } from 'vue'

export function useCounter(initialValue = 0) {
  const count = ref(initialValue)
  
  const double = computed(() => count.value * 2)
  const triple = computed(() => count.value * 3)
  
  function increment() {
    count.value++
  }
  
  function decrement() {
    count.value--
  }
  
  function reset() {
    count.value = initialValue
  }
  
  return {
    count,
    double,
    triple,
    increment,
    decrement,
    reset
  }
}
```

```vue
<script setup>
import { useCounter } from './composables/useCounter'

const { count, increment, double } = useCounter(5)
</script>

<template>
  <div>
    <p>{{ count }} x 2 = {{ double }}</p>
    <button @click="increment">Increment</button>
  </div>
</template>
```

---

## State Management

### Pinia Store

```bash
npm install pinia
```

```javascript
// stores/counter.js
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCounterStore = defineStore('counter', () => {
  const count = ref(0)
  
  const double = computed(() => count.value * 2)
  
  function increment() {
    count.value++
  }
  
  function decrement() {
    count.value--
  }
  
  return { count, double, increment, decrement }
})
```

### Using Store

```vue
<script setup>
import { useCounterStore } from './stores/counter'

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

### Vuex (Legacy)

```javascript
// store/index.js
import { createStore } from 'vuex'

export default createStore({
  state: {
    count: 0
  },
  mutations: {
    increment(state) {
      state.count++
    }
  },
  actions: {
    increment({ commit }) {
      commit('increment')
    }
  },
  getters: {
    double: state => state.count * 2
  }
})
```

---

## Vue Router

### Setup

```bash
npm install vue-router
```

```javascript
// router/index.js
import { createRouter, createWebHistory } from 'vue-router'
import Home from './views/Home.vue'
import About from './views/About.vue'

const routes = [
  { path: '/', component: Home },
  { path: '/about', component: About }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
```

```javascript
// main.js
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

const app = createApp(App)
app.use(router)
app.mount('#app')
```

### Router Usage

```vue
<template>
  <div id="app">
    <nav>
      <router-link to="/">Home</router-link>
      <router-link to="/about">About</router-link>
    </nav>
    <router-view />
  </div>
</template>
```

---

## Forms and Validation

### v-model

```vue
<template>
  <form @submit.prevent="submit">
    <input v-model="form.name" />
    <input v-model="form.email" type="email" />
    <button type="submit">Submit</button>
  </form>
</template>

<script setup>
import { reactive } from 'vue'

const form = reactive({
  name: '',
  email: ''
})

function submit() {
  console.log(form)
}
</script>
```

### Form Validation

```vue
<template>
  <form @submit.prevent="submit">
    <input v-model="form.name" />
    <span v-if="errors.name">{{ errors.name }}</span>
    
    <button type="submit">Submit</button>
  </form>
</template>

<script setup>
import { reactive, ref } from 'vue'

const form = reactive({
  name: ''
})

const errors = ref({})

function validate() {
  errors.value = {}
  
  if (!form.name) {
    errors.value.name = 'Name is required'
  }
  
  return Object.keys(errors.value).length === 0
}

function submit() {
  if (validate()) {
    console.log(form)
  }
}
</script>
```

---

## Best Practices

### Component Structure

```vue
<script setup>
import { ref, computed, watch } from 'vue'

// Props
defineProps({
  title: String
})

// Emits
const emit = defineEmits(['update'])

// State
const count = ref(0)

// Computed
const double = computed(() => count.value * 2)

// Methods
function increment() {
  count.value++
  emit('update', count.value)
}

// Watch
watch(count, (val) => {
  console.log('Count:', val)
})
</script>

<template>
  <div class="component">
    <p>{{ title }}: {{ double }}</p>
    <button @click="increment">Increment</button>
  </div>
</template>

<style scoped>
.component {
  padding: 1rem;
}
</style>
```

### Project Structure

```
src/
├── assets/
├── components/
│   └── CommonButton.vue
├── composables/
│   └── useCounter.js
├── layouts/
├── pages/
│   └── HomePage.vue
├── router/
│   └── index.js
├── stores/
│   └── counter.js
├── App.vue
└── main.js
```

---

## Summary

### Key Takeaways

1. **Vue 3**: Modern Vue.js with Composition API
2. **Reactivity**: ref() and reactive() for state
3. **Components**: Props, emits, and slots
4. **Pinia**: State management
5. **Router**: Navigation
6. **Composables**: Reusable logic

### Next Steps

- Continue with: [02_VUE_COMPONENTS_MASTER.md](02_VUE_COMPONENTS_MASTER.md)
- Practice with Vue CLI projects
- Explore Nuxt.js for SSR

---

## Cross-References

- **Previous**: [09_REACT_DEPLOYMENT_GUIDE.md](../REACT_MASTER/09_REACT_DEPLOYMENT_GUIDE.md)
- **Next**: [02_VUE_COMPONENTS_MASTER.md](02_VUE_COMPONENTS_MASTER.md)

---

*Last updated: 2024*