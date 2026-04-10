# 🟢 Vue Components Complete Guide

## Professional Component Architecture in Vue

---

## Table of Contents

1. [Component Fundamentals](#component-fundamentals)
2. [Props and Events](#props-and-events)
3. [Slots and Scoped Slots](#slots-and-scoped-slots)
4. [Dynamic Components](#dynamic-components)
5. [Teleport and Fragments](#teleport-and-fragments)
6. [Mixins and Plugins](#mixins-and-plugins)
7. [Provide and Inject](#provide-and-inject)
8. [Custom Directives](#custom-directives)
9. [Real-World Examples](#real-world-examples)

---

## Component Fundamentals

### Single File Components

```vue
<!-- MyComponent.vue -->
<script setup>
import { ref } from 'vue'

const count = ref(0)
</script>

<template>
  <div class="my-component">
    <p>Count: {{ count }}</p>
    <button @click="count++">Increment</button>
  </div>
</template>

<style scoped>
.my-component {
  padding: 1rem;
}
</style>
```

### Component Registration

```javascript
// Global registration
app.component('MyComponent', MyComponent)

// Local registration
import MyComponent from './MyComponent.vue'

export default {
  components: {
    MyComponent
  }
}
```

---

## Props and Events

### Defining Props

```vue
<script setup>
defineProps({
  title: {
    type: String,
    default: 'Default Title',
    required: false
  },
  count: {
    type: Number,
    default: 0
  },
  items: {
    type: Array,
    default: () => []
  },
  user: {
    type: Object,
    default: () => ({ name: 'Guest' })
  }
})
</script>

<template>
  <div>
    <h2>{{ title }}</h2>
    <p>{{ count }}</p>
  </div>
</template>
```

### Prop Validation

```javascript
defineProps({
  // Type check
  id: Number,
  
  // Required
  name: {
    type: String,
    required: true
  },
  
  // Default
  age: {
    type: Number,
    default: 18
  },
  
  // Validator
  status: {
    type: String,
    validator: (value) => ['active', 'inactive'].includes(value)
  },
  
  // Multiple types
  value: [String, Number]
})
```

### Emitting Events

```vue
<script setup>
const emit = defineEmits(['update', 'delete', 'submit'])

function handleClick() {
  emit('update', { value: 'new value' })
}

function handleDelete() {
  emit('delete', 1)
}
</script>

<template>
  <div>
    <button @click="handleClick">Update</button>
    <button @click="handleDelete">Delete</button>
  </div>
</template>
```

---

## Slots and Scoped Slots

### Basic Slots

```vue
<!-- Card.vue -->
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

<style scoped>
.card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1rem;
}
</style>
```

### Scoped Slots

```vue
<!-- List.vue -->
<script setup>
defineProps({
  items: Array
})
</script>

<template>
  <ul>
    <li v-for="item in items" :key="item.id">
      <slot :item="item">{{ item.name }}</slot>
    </li>
  </ul>
</template>
```

```vue
<!-- Usage -->
<List :items="items">
  <template #default="{ item }">
    {{ item.name }} - {{ item.email }}
  </template>
</List>
```

---

## Dynamic Components

### Component Switching

```vue
<script setup>
import { ref } from 'vue'
import Home from './Home.vue'
import About from './About.vue'
import Contact from './Contact.vue'

const currentView = ref('Home')
</script>

<template>
  <div>
    <button @click="currentView = 'Home'">Home</button>
    <button @click="currentView = 'About'">About</button>
    <button @click="currentView = 'Contact'">Contact</button>
    
    <component :is="currentView" />
  </div>
</template>
```

### Keep Alive

```vue
<template>
  <keep-alive>
    <component :is="currentComponent" />
  </keep-alive>
</template>
```

---

## Teleport and Fragments

### Teleport

```vue
<template>
  <button @click="showModal = true">Open Modal</button>
  
  <Teleport to="body">
    <div v-if="showModal" class="modal">
      <p>Modal Content</p>
      <button @click="showModal = false">Close</button>
    </div>
  </Teleport>
</template>

<style scoped>
.modal {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: white;
  padding: 2rem;
}
</style>
```

### Fragments ( Vue 3)

```vue
<template>
  <!-- Multiple root elements allowed -->
  <header>Header</header>
  <main>Content</main>
  <footer>Footer</footer>
</template>
```

---

## Mixins and Plugins

### Mixins

```javascript
// mixins/loggable.js
export default {
  methods: {
    log(message) {
      console.log(`[${this.$options.name}]`, message)
    }
  }
}
```

```vue
<script>
import Loggable from './mixins/loggable'

export default {
  name: 'MyComponent',
  mixins: [Loggable],
  mounted() {
    this.log('Component mounted')
  }
}
</script>
```

### Plugins

```javascript
// plugins/myPlugin.js
export default {
  install(app, options) {
    app.config.globalProperties.$myMethod = () => {
      console.log('Plugin method')
    }
    
    app.directive('my-directive', {
      mounted(el, binding) {
        el.style.color = binding.value
      }
    })
  }
}
```

```javascript
// main.js
import { createApp } from 'vue'
import App from './App.vue'
import myPlugin from './plugins/myPlugin'

const app = createApp(App)
app.use(myPlugin, { option: true })
app.mount('#app')
```

---

## Provide and Inject

### Provide/Inject

```vue
<!-- ParentComponent.vue -->
<script setup>
import { provide, ref } from 'vue'

const count = ref(0)

function increment() {
  count.value++
}

provide('count', count)
provide('increment', increment)
</script>

<template>
  <div>
    <p>Parent: {{ count }}</p>
    <ChildComponent />
  </div>
</template>
```

```vue
<!-- ChildComponent.vue -->
<script setup>
import { inject } from 'vue'

const count = inject('count')
const increment = inject('increment')
</script>

<template>
  <div>
    <p>Child: {{ count }}</p>
    <button @click="increment">Increment</button>
  </div>
</template>
```

---

## Custom Directives

### Simple Directive

```javascript
// directives/focus.js
export default {
  mounted(el) {
    el.focus()
  }
}
```

```vue
<script setup>
import { vFocus } from './directives/focus'

<input v-focus />
</script>
```

### Directive with Value

```javascript
// directives/color.js
export default {
  mounted(el, binding) {
    el.style.color = binding.value
  },
  updated(el, binding) {
    el.style.color = binding.value
  }
}
```

```vue
<script setup>
import { vColor } from './directives/color'

<p v-color="'red'">Red text</p>
```

---

## Real-World Examples

### Modal Component

```vue
<!-- Modal.vue -->
<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  show: Boolean,
  title: String
})

const emit = defineEmits(['close'])

watch(() => props.show, (value) => {
  if (value) {
    document.body.style.overflow = 'hidden'
  } else {
    document.body.style.overflow = ''
  }
})

function close() {
  emit('close')
}
</script>

<template>
  <Teleport to="body">
    <div v-if="show" class="modal-overlay" @click="close">
      <div class="modal-content" @click.stop>
        <div class="modal-header">
          <h3>{{ title }}</h3>
          <button @click="close">&times;</button>
        </div>
        <div class="modal-body">
          <slot />
        </div>
      </div>
    </div>
  </Teleport>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-content {
  background: white;
  border-radius: 8px;
  padding: 1rem;
  max-width: 500px;
  width: 90%;
}

.modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}
</style>
```

### Dropdown Component

```vue
<!-- Dropdown.vue -->
<script setup>
import { ref, onMounted, onUnmounted } from 'vue'

defineProps({
  options: Array
})

const emit = defineEmits(['select'])

const isOpen = ref(false)
const dropdownRef = ref(null)

function toggle() {
  isOpen.value = !isOpen.value
}

function select(option) {
  emit('select', option)
  isOpen.value = false
}

function handleClickOutside(event) {
  if (dropdownRef.value && !dropdownRef.value.contains(event.target)) {
    isOpen.value = false
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

<template>
  <div ref="dropdownRef" class="dropdown">
    <button class="dropdown-toggle" @click="toggle">
      <slot>Select...</slot>
      <span>▼</span>
    </button>
    <ul v-if="isOpen" class="dropdown-menu">
      <li 
        v-for="option in options" 
        :key="option.value"
        @click="select(option)"
      >
        {{ option.label }}
      </li>
    </ul>
  </div>
</template>

<style scoped>
.dropdown {
  position: relative;
}

.dropdown-menu {
  position: absolute;
  top: 100%;
  left: 0;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  padding: 0;
  list-style: none;
  min-width: 150px;
}

.dropdown-menu li {
  padding: 0.5rem 1rem;
  cursor: pointer;
}

.dropdown-menu li:hover {
  background: #f5f5f5;
}
</style>
```

---

## Summary

### Key Takeaways

1. **Props**: Type-safe data passing
2. **Emits**: Component communication
3. **Slots**: Flexible content
4. **Dynamic**: Component switching
5. **Teleport**: Portal rendering
6. **Directives**: DOM manipulation

### Next Steps

- Continue with: [03_VUE_ROUTING_MASTER.md](03_VUE_ROUTING_MASTER.md)
- Practice building complex components
- Explore component libraries

---

## Cross-References

- **Previous**: [01_VUE_FUNDAMENTALS.md](01_VUE_FUNDAMENTALS.md)
- **Next**: [03_VUE_ROUTING_MASTER.md](03_VUE_ROUTING_MASTER.md)

---

*Last updated: 2024*