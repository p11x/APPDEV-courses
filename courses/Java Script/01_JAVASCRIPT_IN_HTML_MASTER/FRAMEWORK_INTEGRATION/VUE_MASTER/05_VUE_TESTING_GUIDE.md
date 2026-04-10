# 🟢 Vue Testing Complete Guide

## Testing Vue.js Applications

---

## Table of Contents

1. [Testing Setup](#testing-setup)
2. [Component Testing](#component-testing)
3. [Testing Composables](#testing-composables)
4. [Testing Stores](#testing-stores)
5. [Mocking](#mocking)

---

## Testing Setup

### Installation

```bash
npm install --save-dev vitest @vue/test-utils jsdom
```

### Configuration

```javascript
// vitest.config.js
import { defineConfig } from 'vitest/config'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  test: {
    environment: 'jsdom'
  }
})
```

```json
// package.json
{
  "scripts": {
    "test": "vitest",
    "test:run": "vitest run"
  }
}
```

---

## Component Testing

### Basic Test

```javascript
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import MyButton from './MyButton.vue'

describe('MyButton', () => {
  it('renders properly', () => {
    const wrapper = mount(MyButton, {
      props: { label: 'Click me' }
    })
    
    expect(wrapper.text()).toContain('Click me')
  })
  
  it('emits click event', async () => {
    const wrapper = mount(MyButton)
    
    await wrapper.trigger('click')
    
    expect(wrapper.emitted('click')).toBeTruthy()
  })
})
```

---

## Testing Composables

### Composable Test

```javascript
import { describe, it, expect } from 'vitest'
import { useCounter } from './useCounter'

describe('useCounter', () => {
  it('increments count', () => {
    const { count, increment } = useCounter()
    
    increment()
    
    expect(count.value).toBe(1)
  })
})
```

---

## Testing Stores

### Store Test

```javascript
import { describe, it, expect, beforeEach } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useCounterStore } from './counter'

describe('counter store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })
  
  it('increments count', () => {
    const store = useCounterStore()
    
    store.increment()
    
    expect(store.count).toBe(1)
  })
})
```

---

## Mocking

### Mocking API

```javascript
import { vi } from 'vitest'

global.fetch = vi.fn(() => 
  Promise.resolve({
    json: () => Promise.resolve({ name: 'John' })
  })
)
```

---

## Summary

### Key Takeaways

1. **Vitest**: Fast test runner
2. **Vue Test Utils**: Component testing
3. **Composables**: Test like regular functions
4. **Stores**: Test with Pinia

### Next Steps

- Continue with Angular: [01_ANGULAR_FUNDAMENTALS.md](../ANGULAR_MASTER/01_ANGULAR_FUNDAMENTALS.md)
- Practice test patterns
- Implement E2E tests

---

## Cross-References

- **Previous**: [04_VUE_STATE_MANAGEMENT.md](04_VUE_STATE_MANAGEMENT.md)
- **Next**: [01_ANGULAR_FUNDAMENTALS.md](../ANGULAR_MASTER/01_ANGULAR_FUNDAMENTALS.md)

---

*Last updated: 2024*