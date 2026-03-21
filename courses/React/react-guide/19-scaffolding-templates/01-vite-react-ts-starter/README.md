# Vite + React + TypeScript Production Template

## Overview
This is a complete, production-ready starter template for building React applications with Vite and TypeScript. It follows best practices for project structure, testing, and developer experience.

## Project Structure

```
my-vite-app/
├── src/
│   ├── components/
│   │   ├── ui/                    # Reusable primitive components
│   │   │   ├── Button/
│   │   │   │   ├── Button.tsx
│   │   │   │   ├── Button.test.tsx
│   │   │   │   ├── Button.stories.tsx
│   │   │   │   └── index.ts
│   │   │   ├── Input/
│   │   │   └── index.ts           # Barrel export
│   │   │
│   │   └── features/              # Feature-specific components
│   │       ├── auth/
│   │       │   ├── LoginForm.tsx
│   │       │   └── index.ts
│   │       └── dashboard/
│   │
│   ├── hooks/                     # Custom hooks
│   │   ├── useAuth.ts
│   │   ├── useDebounce.ts
│   │   └── index.ts
│   │
│   ├── services/                 # API layer (repository pattern)
│   │   ├── api/
│   │   │   ├── client.ts         # Axios instance
│   │   │   └── endpoints.ts
│   │   ├── auth/
│   │   │   └── authService.ts
│   │   └── index.ts
│   │
│   ├── stores/                    # Zustand stores
│   │   ├── authStore.ts
│   │   ├── uiStore.ts
│   │   └── index.ts
│   │
│   ├── types/                     # Shared TypeScript types
│   │   ├── user.ts
│   │   ├── api.ts
│   │   └── index.ts
│   │
│   ├── utils/                     # Pure utility functions
│   │   ├── formatters.ts
│   │   ├── validators.ts
│   │   └── index.ts
│   │
│   ├── pages/                     # Route-level components
│   │   ├── Home/
│   │   │   ├── HomePage.tsx
│   │   │   └── index.ts
│   │   ├── Dashboard/
│   │   └── index.ts
│   │
│   ├── layouts/                   # Layout components
│   │   ├── MainLayout.tsx
│   │   └── index.ts
│   │
│   ├── App.tsx                    # Root component with routing
│   ├── main.tsx                   # Entry point
│   └── vite-env.d.ts              # Vite type definitions
│
├── public/                        # Static assets
│   ├── favicon.svg
│   └── locales/                   # i18n files
│
├── tests/                         # Test utilities
│   ├── setup.ts                   # Vitest setup
│   └── mocks/
│
├── .env                           # Environment variables
├── .env.example                   # Environment template
├── .eslintrc.cjs                  # ESLint config
├── .prettierrc                    # Prettier config
├── tsconfig.json                  # TypeScript config
├── tsconfig.node.json             # Node TypeScript config
├── vite.config.ts                 # Vite config
├── index.html                     # HTML entry
└── package.json
```

## Required Packages

Install these packages to match this template:

```bash
# Core
npm install react react-dom react-router-dom

# State Management
npm install zustand @tanstack/react-query

# Forms & Validation
npm install react-hook-form zod @hookform/resolvers

# Styling
npm install clsx tailwind-merge class-variance-authority

# HTTP Client
npm install axios

# Utilities
npm install date-fns

# Dev Dependencies
npm install -D typescript @types/react @types/react-dom vite
npm install -D vitest @testing-library/react @testing-library/jest-dom jsdom
npm install -D eslint eslint-config-react-app
npm install -D prettier
```

## Configuration Files

### tsconfig.json

```json
{
  "compilerOptions": {
    "target": "ES2020",
    "useDefineForClassFields": true,
    "lib": ["ES2020", "DOM", "DOM.Iterable"],
    "module": "ESNext",
    "skipLibCheck": true,
    
    "moduleResolution": "bundler",
    "allowImportingTsExtensions": true,
    "resolveJsonModule": true,
    "isolatedModules": true,
    "noEmit": true,
    "jsx": "react-jsx",
    
    "strict": true,
    "noUnusedLocals": true,
    "noUnusedParameters": true,
    "noFallthroughCasesInSwitch": true,
    
    "baseUrl": ".",
    "paths": {
      "@/*": ["src/*"],
      "@components/*": ["src/components/*"],
      "@hooks/*": ["src/hooks/*"],
      "@services/*": ["src/services/*"],
      "@stores/*": ["src/stores/*"],
      "@types/*": ["src/types/*"],
      "@utils/*": ["src/utils/*"],
      "@pages/*": ["src/pages/*"]
    }
  },
  "include": ["src"],
  "references": [{ "path": "./tsconfig.node.json" }]
}
```

### vite.config.ts

```typescript
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
      '@components': path.resolve(__dirname, './src/components'),
      '@hooks': path.resolve(__dirname, './src/hooks'),
      '@services': path.resolve(__dirname, './src/services'),
      '@stores': path.resolve(__dirname, './src/stores'),
      '@types': path.resolve(__dirname, './src/types'),
      '@utils': path.resolve(__dirname, './src/utils'),
      '@pages': path.resolve(__dirname, './src/pages'),
    },
  },
  
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['react', 'react-dom', 'react-router-dom'],
          query: ['@tanstack/react-query'],
        },
      },
    },
  },
});
```

### .env.example

```bash
# API
VITE_API_URL=http://localhost:3001
VITE_API_TIMEOUT=10000

# Feature Flags
VITE_ENABLE_ANALYTICS=false
VITE_DEBUG_MODE=false
```

## Step-by-Step Setup

### 1. Create the project

```bash
npm create vite@latest my-vite-app -- --template react-ts
cd my-vite-app
```

### 2. Install dependencies

```bash
npm install react-router-dom zustand @tanstack/react-query react-hook-form zod @hookform/resolvers axios clsx tailwind-merge date-fns
```

### 3. Configure TypeScript

Copy the tsconfig.json above to enable path aliases.

### 4. Configure Vite

Copy vite.config.ts to enable @ aliases.

### 5. Set up testing

```bash
npm install -D vitest @testing-library/react @testing-library/jest-dom jsdom
```

Create `tests/setup.ts`:

```typescript
import '@testing-library/jest-dom';
```

Create `vite.config.ts` test config:

```typescript
/// <reference types="vitest" />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: './tests/setup.ts',
  },
});
```

## Annotated Code Examples

### App.tsx

```typescript
// [File: src/App.tsx]
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { MainLayout } from './layouts';
import { HomePage, DashboardPage } from './pages';
import './App.css';

/**
 * QueryClient manages caching and synchronization of server state.
 * Configure default options for all queries here.
 */
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 5, // 5 minutes
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

function App() {
  return (
    /**
     * QueryClientProvider enables React Query for data fetching.
     * All components inside can now use useQuery and useMutation.
     */
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <MainLayout>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/dashboard" element={<DashboardPage />} />
          </Routes>
        </MainLayout>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;
```

### main.tsx

```typescript
// [File: src/main.tsx]
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

// StrictMode renders components twice in development
// to help find side effects. Remove in production.
ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
```

### Example Feature: Auth Store with Zustand

```typescript
// [File: src/stores/authStore.ts]
import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface User {
  id: string;
  email: string;
  name: string;
}

interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (user: User, token: string) => void;
  logout: () => void;
}

/**
 * Zustand store with persist middleware.
 * User stays logged in across page refreshes.
 */
export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      user: null,
      token: null,
      isAuthenticated: false,
      
      login: (user, token) => set({ 
        user, 
        token, 
        isAuthenticated: true 
      }),
      
      logout: () => set({ 
        user: null, 
        token: null, 
        isAuthenticated: false 
      }),
    }),
    {
      name: 'auth-storage', // localStorage key
    }
  )
);
```

### Example Feature: API Client

```typescript
// [File: src/services/api/client.ts]
import axios from 'axios';
import { useAuthStore } from '@/stores/authStore';

/**
 * Axios instance with interceptors for auth.
 * Automatically attaches token to requests.
 */
export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:3001',
  timeout: Number(import.meta.env.VITE_API_TIMEOUT) || 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor: add auth token
apiClient.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Response interceptor: handle auth errors
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      useAuthStore.getState().logout();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);
```

## Why This Structure Scales

1. **Feature-based organization** — Components related to a feature stay together
2. **Barrel exports** — Clean imports with `index.ts` files
3. **Path aliases** — No more `../../../components` spaghetti
4. **Separation of concerns** — Services for API, stores for state, hooks for logic
5. **Colocated testing** — Tests live next to the code they test

## Next Steps

1. Run `npm run dev` to start the dev server
2. Add your first feature in `src/components/features/`
3. Configure your API base URL in `.env`
4. Start building!

For more details, see:
- [React Query setup](../../06-data-fetching/02-react-query/01-tanstack-query-setup.md)
- [Zustand state management](../../04-state-management/03-external-state/01-zustand-intro-and-setup.md)
- [Testing with Vitest](../../10-testing/01-unit-testing/01-vitest-setup-with-react.md)
