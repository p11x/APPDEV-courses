# Framework Routing Master

Comprehensive guide to client-side routing in modern JavaScript frameworks. Covers route configuration, nested routes, guards, dynamic routing, and lazy loading.

## Table of Contents

1. [Client-Side Routing Basics](#client-side-routing-basics)
2. [React Router v6](#react-router-v6)
3. [Vue Router](#vue-router)
4. [Angular Router](#angular-router)
5. [Nested Routes](#nested-routes)
6. [Route Guards](#route-guards)
7. [Dynamic Routing](#dynamic-routing)
8. [Lazy Loading Routes](#lazy-loading-routes)
9. [Route Guards Implementation](#route-guards-implementation)
10. [Key Takeaways](#key-takeaways)
11. [Common Pitfalls](#common-pitfalls)

---

## Client-Side Routing Basics

Client-side routing provides seamless navigation without page reloads. It maintains the SPA experience while enabling deep linking and browser history.

### Core Concepts

- **URL Mapping**: Map URLs to components
- **History API**: Manage browser history
- **Route Matching**: Match paths to routes
- **Navigation Guards**: Control route access

---

## React Router v6

### Basic Setup

```javascript
// file: router/App.jsx
import React, { Suspense, lazy } from 'react';
import { BrowserRouter, Routes, Route, Link, Navigate } from 'react-router-dom';

const Home = lazy(() => import('../pages/Home'));
const About = lazy(() => import('../pages/About'));
const Users = lazy(() => import('../pages/Users'));
const UserProfile = lazy(() => import('../pages/UserProfile'));
const Login = lazy(() => import('../pages/Login'));
const Dashboard = lazy(() => import('../pages/Dashboard'));
const NotFound = lazy(() => import('../pages/NotFound'));

const App = () => {
  return (
    <BrowserRouter>
      <nav>
        <Link to="/">Home</Link>
        <Link to="/about">About</Link>
        <Link to="/users">Users</Link>
        <Link to="/dashboard">Dashboard</Link>
      </nav>

      <Suspense fallback={<div>Loading...</div>}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/users" element={<Users />} />
          <Route path="/users/:id" element={<UserProfile />} />
          <Route path="/login" element={<Login />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/404" element={<NotFound />} />
          <Route path="*" element={<Navigate to="/404" replace />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
};

export default App;
```

### Route Configuration

```javascript
// file: router/routes.js
import { Routes, Route, Navigate } from 'react-router-dom';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    exact: true,
  },
  {
    path: '/about',
    name: 'About',
    component: About,
  },
  {
    path: '/users',
    name: 'Users',
    component: Users,
    protected: true,
  },
  {
    path: '/users/:id',
    name: 'UserProfile',
    component: UserProfile,
    protected: true,
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Dashboard,
    protected: true,
    roles: ['admin', 'user'],
  },
  {
    path: '/settings',
    name: 'Settings',
    component: Settings,
    protected: true,
    roles: ['admin'],
  },
];

const RouteConfig = () => {
  const { isAuthenticated, user } = useAuth();

  return (
    <Routes>
      {routes.map((route) => (
        <Route
          key={route.path}
          path={route.path}
          element={
            route.protected && !isAuthenticated ? (
              <Navigate to="/login" replace />
            ) : route.protected && route.roles ? (
              route.roles.includes(user?.role) ? (
                <route.component />
              ) : (
                <Navigate to="/unauthorized" replace />
              )
            ) : (
              <route.component />
            )
          }
        />
      ))}
    </Routes>
  );
};

export default RouteConfig;
```

---

## Vue Router

### Setup

```javascript
// file: vue-router/index.js
import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/Home.vue';
import About from '../views/About.vue';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/about',
    name: 'About',
    component: About,
  },
  {
    path: '/users',
    name: 'Users',
    component: () => import('../views/Users.vue'),
    children: [
      {
        path: '',
        name: 'UsersList',
        component: () => import('../views/UsersList.vue'),
      },
      {
        path: ':id',
        name: 'UserProfile',
        component: () => import('../views/UserProfile.vue'),
        props: true,
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    }
    return { top: 0 };
  },
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = localStorage.getItem('token');

  if (to.meta.requiresAuth && !isAuthenticated) {
    next({ name: 'Login' });
  } else {
    next();
  }
});

export default router;
```

### Route Navigation

```vue
<!-- file: vue-router/Navigation.vue -->
<template>
  <div>
    <nav>
      <router-link to="/">Home</router-link>
      <router-link to="/about">About</router-link>
      <router-link :to="{ name: 'UsersList' }">Users</router-link>
    </nav>

    <router-view v-slot="{ Component }">
      <transition name="fade" mode="out-in">
        <component :is="Component" />
      </transition>
    </router-view>

    <div class="breadcrumbs">
      < router-link :to="{ name: 'Home' }">Home</router-link>
      <span v-for="(crumb, index) in breadcrumbs" :key="index">
        > <router-link :to="crumb.path">{{ crumb.name }}</router-link>
      </span>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const breadcrumbs = computed(() => {
  return route.matched
    .filter((r) => r.meta?.breadcrumb)
    .map((r) => ({
      name: r.meta.breadcrumb,
      path: r.path,
    }));
});
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
```

---

## Angular Router

### Router Module Setup

```typescript
// file: angular-router/app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { UserProfileComponent } from './users/user-profile.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminGuard } from './guards/admin.guard';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  {
    path: 'users',
    component: UsersComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', component: UsersListComponent },
      { path: ':id', component: UserProfileComponent },
    ],
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
    canActivate: [AdminGuard],
  },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      enableTracing: true,
      scrollPositionRestoration: 'enabled',
      anchorScrolling: 'enabled',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
```

### Route Parameters

```typescript
// file: angular-router/user-profile.component.ts
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  template: `
    <div>
      <h2>User Profile</h2>
      <p>User ID: {{ userId }}</p>
      <p>Tab: {{ tab }}</p>
      
      <nav>
        <a [routerLink]="['/users', userId, 'details']">Details</a>
        <a [routerLink]="['/users', userId, 'posts']">Posts</a>
        <a [routerLink]="['/users', userId, 'settings']">Settings</a>
      </nav>
      
      <router-outlet></router-outlet>
    </div>
  `,
})
export class UserProfileComponent implements OnInit {
  userId: string = '';
  tab: string = 'details';

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.userId = params.get('id') || '';
    });

    this.route.queryParamMap.subscribe((params) => {
      this.tab = params.get('tab') || 'details';
    });
  }
}
```

---

## Nested Routes

### React Nested Routes

```javascript
// file: router/NestedRoutes.jsx
import { BrowserRouter, Routes, Route, Link, Outlet } from 'react-router-dom';

const UsersLayout = () => (
  <div className="users-layout">
    <aside>
      <nav>
        <Link to="/users">All Users</Link>
        <Link to="/users/admins">Admins</Link>
        <Link to="/users/editors">Editors</Link>
      </nav>
    </aside>
    <main>
      <Outlet />
    </main>
  </div>
);

const UsersList = () => <div>All Users List</div>;
const UserAdmins = () => <div>Admins List</div>;
const UserEditors = () => <div>Editors List</div>;

const App = () => (
  <BrowserRouter>
    <Routes>
      <Route path="/users" element={<UsersLayout />}>
        <Route index element={<UsersList />} />
        <Route path="admins" element={<UserAdmins />} />
        <Route path="editors" element={<UserEditors />} />
      </Route>
    </Routes>
  </BrowserRouter>
);

export default App;
```

---

## Route Guards

### Auth Guard Implementation

```javascript
// file: guards/AuthGuard.jsx
import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

const AuthGuard = ({ children }) => {
  const { isAuthenticated, isLoading } = useAuth();
  const location = useLocation();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!isAuthenticated) {
    return (
      <Navigate
        to="/login"
        state={{ from: location }}
        replace
      />
    );
  }

  return children;
};

export default AuthGuard;

const RoleGuard = ({ children, allowedRoles }) => {
  const { user } = useAuth();

  if (!allowedRoles.includes(user?.role)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return children;
};

export { RoleGuard };
```

### Permission Guards

```javascript
// file: guards/PermissionGuard.jsx
import React, { useState, useEffect } from 'react';
import { Navigate } from 'react-router-dom';

const PermissionGuard = ({ permissions, children }) => {
  const [hasPermissions, setHasPermissions] = useState(false);
  const [isChecking, setIsChecking] = useState(true);

  useEffect(() => {
    const checkPermissions = async () => {
      const userPermissions = await fetchUserPermissions();
      const hasAccess = permissions.every((p) =>
        userPermissions.includes(p)
      );
      setHasPermissions(hasAccess);
      setIsChecking(false);
    };

    checkPermissions();
  }, [permissions]);

  if (isChecking) {
    return <div>Checking permissions...</div>;
  }

  if (!hasPermissions) {
    return <Navigate to="/unauthorized" replace />;
  }

  return children;
};

const ConditionalGuard = ({ condition, redirectTo = '/login', children }) => {
  if (!condition) {
    return <Navigate to={redirectTo} replace />;
  }

  return children;
};

export { PermissionGuard, ConditionalGuard };
```

---

## Dynamic Routing

### Dynamic Parameters

```javascript
// file: router/DynamicRoutes.jsx
import { Routes, Route, useParams, useSearchParams } from 'react-router-dom';

const ProductDetails = () => {
  const { productId } = useParams();
  const [searchParams, setSearchParams] = useSearchParams();
  const variant = searchParams.get('variant');

  return (
    <div>
      <h2>Product {productId}</h2>
      {variant && <p>Showing variant: {variant}</p>}
      <button onClick={() => setSearchParams({ variant: 'blue' })}>
        Blue
      </button>
      <button onClick={() => setSearchParams({ variant: 'red' })}>
        Red
      </button>
    </div>
  );
};

const SearchResults = () => {
  const [searchParams] = useSearchParams();
  const query = searchParams.get('q') || '';
  const sort = searchParams.get('sort') || 'relevance';
  const page = parseInt(searchParams.get('page')) || 1;

  return (
    <div>
      <h2>Results for "{query}"</h2>
      <p>Sort: {sort}</p>
      <p>Page: {page}</p>
      <button onClick={() => setSearchParams({ sort: 'price' })}>
        Price
      </button>
      <button onClick={() => setSearchParams({ sort: 'date' })}>
        Date
      </button>
    </div>
  );
};

const App = () => (
  <Routes>
    <Route path="/products/:productId" element={<ProductDetails />} />
    <Route path="/search" element={<SearchResults />} />
  </Routes>
);
```

---

## Lazy Loading Routes

### Code Splitting

```javascript
<file: router/LazyLoading.jsx>
// React.lazy for route-based code splitting

import React, { Suspense, lazy } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

const Home = lazy(() => import('./pages/Home'));
const About = lazy(() => import('./pages/About'));
const Dashboard = lazy(() => import('./pages/Dashboard'));
const Settings = lazy(() => import('./pages/Settings'));
const Profile = lazy(() => import('./pages/Profile'));
const Analytics = lazy(() => import('./pages/Analytics'));

const LoadingFallback = () => (
  <div className="loading-fallback">
    <Spinner /> Loading...
  </div>
);

const App = () => (
  <BrowserRouter>
    <Suspense fallback={<LoadingFallback />}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          }
        >
          <Route index element={<DashboardHome />} />
          <Route path="settings" element={<Settings />} />
          <Route path="profile" element={<Profile />} />
          <Route path="analytics" element={<Analytics />} />
        </Route>
      </Routes>
    </Suspense>
  </BrowserRouter>
);
```

### Prefetching Strategies

```javascript
// file: router/Prefetching.jsx
import { lazy, Suspense } from 'react';
import { Link, prefetch } from './prefetch';

const PrefetchLink = ({ to, children }) => (
  <Link
    to={to}
    onMouseEnter={() => {
      prefetch(to);
    }}
  >
    {children}
  </Link>
);

const Dashboard = lazy(() => import('./pages/Dashboard'));
const Analytics = lazy(() => import('./pages/Analytics'));

const App = () => (
  <>
    <PrefetchLink to="/dashboard">Dashboard</PrefetchLink>
    <PrefetchLink to="/analytics">Analytics</PrefetchLink>
  </>
);
```

---

## Route Guards Implementation

### Complete Guard System

```javascript
// file: guards/routerGuards.js

const createRouteGuard = (options = {}) => {
  const {
    authRequired = false,
    allowedRoles = [],
    requiredPermissions = [],
    redirectTo = '/login',
    checkVerification = true,
  } = options;

  return async (to, from, next) => {
    const user = getCurrentUser();
    const isAuthenticated = !!user?.token;

    if (authRequired && !isAuthenticated) {
      return next({ path: redirectTo, query: { redirect: to.path } });
    }

    if (authRequired && checkVerification && !user?.verified) {
      return next({ path: '/verify-email' });
    }

    if (allowedRoles.length > 0 && !allowedRoles.includes(user?.role)) {
      return next({ path: '/unauthorized' });
    }

    if (requiredPermissions.length > 0) {
      const hasPermissions = await checkUserPermissions(
        requiredPermissions
      );
      if (!hasPermissions) {
        return next({ path: '/unauthorized' });
      }
    }

    return next();
  };
};

const createDebouncedSearch = (delay = 300) => {
  let timeoutId;

  return (searchFn, ...args) => {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => searchFn(...args), delay);
  };
};

const searchUsers = createDebouncedSearch((query) => {
  console.log('Searching:', query);
});
```

---

## Key Takeaways

1. **React Router v6** uses Routes/Route syntax
2. **Lazy loading** reduces initial bundle size
3. **Route guards** control navigation access
4. **Nested routes** create layout hierarchies
5. **Dynamic segments** handle URL parameters
6. **Query parameters** enable filtering/sorting

---

## Common Pitfalls

1. **Not handling 404** routes leads to poor UX
2. **Nested path conflicts** cause routing issues
3. **Missing guards** expose protected routes
4. **Not lazy loading** causes large bundles
5. **Direct URL access** bypasses guards

---

## Related Files

- [01_FRAMEWORK_COMPARISON_MASTER](./01_FRAMEWORK_COMPARISON_MASTER.md)
- [02_COMPONENT_ARCHITECTURE_PATTERNS](./02_COMPONENT_ARCHITECTURE_PATTERNS.md)
- [06_FRAMEWORK_PERFORMANCE_OPTIMIZATION](./06_FRAMEWORK_PERFORMANCE_OPTIMIZATION.md)
- [08_FRAMEWORK_DEPLOYMENT_AND_BUILDING](./08_FRAMEWORK_DEPLOYMENT_AND_BUILDING.md)