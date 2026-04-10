# 🟢 Vue Routing Complete Guide

## Navigation in Vue.js Applications

---

## Table of Contents

1. [Vue Router Setup](#vue-router-setup)
2. [Route Configuration](#route-configuration)
3. [Navigation](#navigation)
4. [Route Parameters](#route-parameters)
5. [Nested Routes](#nested-routes)
6. [Programmatic Navigation](#programmatic-navigation)
7. [Route Guards](#route-guards)
8. [Lazy Loading](#lazy-loading)

---

## Vue Router Setup

### Installation

```bash
npm install vue-router
```

### Basic Setup

```javascript
// router/index.js
import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import About from '../views/About.vue'
import User from '../views/User.vue'

const routes = [
  { path: '/', name: 'Home', component: Home },
  { path: '/about', name: 'About', component: About },
  { path: '/user/:id', name: 'User', component: User }
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

---

## Route Configuration

### Basic Routes

```javascript
const routes = [
  { 
    path: '/', 
    name: 'Home', 
    component: Home,
    meta: { title: 'Home Page' }
  },
  { 
    path: '/about', 
    name: 'About', 
    component: About 
  }
]
```

### Route with Children

```javascript
const routes = [
  {
    path: '/dashboard',
    component: Dashboard,
    children: [
      { path: '', component: DashboardOverview },
      { path: 'settings', component: DashboardSettings }
    ]
  }
]
```

---

## Navigation

### Router Links

```vue
<template>
  <nav>
    <router-link to="/">Home</router-link>
    <router-link to="/about">About</router-link>
    <router-link :to="{ name: 'User', params: { id: 1 }}">User</router-link>
  </nav>
</template>
```

### Active Links

```vue
<template>
  <nav>
    <router-link to="/" active-class="active">Home</router-link>
    <router-link to="/about" exact-active-class="active">About</router-link>
  </nav>
</template>

<style>
.router-link-active {
  color: red;
}
</style>
```

---

## Route Parameters

### URL Parameters

```vue
<!-- User.vue -->
<script setup>
import { useRoute } from 'vue-router'
import { computed } from 'vue'

const route = useRoute()
const userId = computed(() => route.params.id)
</script>

<template>
  <div>
    <p>User ID: {{ userId }}</p>
  </div>
</template>
```

### Query Parameters

```vue
<script setup>
import { useRoute } from 'vue-router'

const route = useRoute()
const page = computed(() => route.query.page)
const sort = computed(() => route.query.sort)
</script>
```

---

## Nested Routes

### Nested Structure

```javascript
const routes = [
  {
    path: '/settings',
    component: SettingsLayout,
    children: [
      { path: '', redirect: '/settings/profile' },
      { path: 'profile', component: ProfileSettings },
      { path: 'account', component: AccountSettings }
    ]
  }
]
```

```vue
<!-- SettingsLayout.vue -->
<template>
  <div class="settings-layout">
    <nav>
      <router-link to="/settings/profile">Profile</router-link>
      <router-link to="/settings/account">Account</router-link>
    </nav>
    <router-view />
  </div>
</template>
```

---

## Programmatic Navigation

### Using Router

```vue
<script setup>
import { useRouter } from 'vue-router'

const router = useRouter()

function goHome() {
  router.push('/')
}

function goWithParams() {
  router.push({ 
    name: 'User', 
    params: { id: 1 } 
  })
}

function replaceCurrent() {
  router.replace('/about')
}

function goBack() {
  router.go(-1)
}
</script>
```

---

## Route Guards

### Global Guards

```javascript
// router/index.js
router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/login')
  } else {
    next()
  }
})
```

### Route Guards

```javascript
const routes = [
  {
    path: '/admin',
    component: Admin,
    beforeEnter: (to, from, next) => {
      if (to.meta.requiresAuth) {
        next('/login')
      } else {
        next()
      }
    }
  }
]
```

### Component Guards

```vue
<script>
export default {
  beforeRouteEnter(to, from, next) {
    next(vm => {
      vm.loadData()
    })
  },
  beforeRouteUpdate(to, from, next) {
    this.loadData()
    next()
  },
  beforeRouteLeave(to, from, next) {
    if (this.hasChanges) {
      if (confirm('Leave?')) {
        next()
      } else {
        next(false)
      }
    } else {
      next()
    }
  }
}
</script>
```

---

## Lazy Loading

### Dynamic Imports

```javascript
const routes = [
  { path: '/', component: import('./views/Home.vue') },
  { path: '/about', component: import('./views/About.vue') }
]
```

### Named Chunks

```javascript
const routes = [
  { 
    path: '/dashboard', 
    component: () => import(/* webpackChunkName: "dashboard" */ './views/Dashboard.vue') 
  },
  { 
    path: '/settings', 
    component: () => import(/* webpackChunkName: "settings" */ './views/Settings.vue') 
  }
]
```

---

## Summary

### Key Takeaways

1. **Setup**: Configure router with routes
2. **Navigation**: Links and programmatic
3. **Parameters**: URL and query params
4. **Guards**: Authentication and permissions
5. **Lazy Loading**: Code splitting

### Next Steps

- Continue with: [04_VUE_STATE_MANAGEMENT.md](04_VUE_STATE_MANAGEMENT.md)
- Study route meta fields
- Implement route transitions

---

## Cross-References

- **Previous**: [02_VUE_COMPONENTS_MASTER.md](02_VUE_COMPONENTS_MASTER.md)
- **Next**: [04_VUE_STATE_MANAGEMENT.md](04_VUE_STATE_MANAGEMENT.md)

---

*Last updated: 2024*