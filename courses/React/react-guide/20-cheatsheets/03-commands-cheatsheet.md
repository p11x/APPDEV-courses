# Commands Cheatsheet

## Overview
A scannable reference for all CLI commands used across this guide.

---

## Vite Project Setup

```bash
# Create new Vite project
npm create vite@latest my-app -- --template react
npm create vite@latest my-app -- --template react-ts
npm create vite@latest my-app -- --template vue
npm create vite@latest my-app -- --template vanilla

# Start development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Lint code
npm run lint
```

---

## Package Installation Quick Reference

| Package | Install Command | Purpose | Module |
|---------|---------------|---------|--------|
| React Router | `npm i react-router-dom` | Routing | 05-routing |
| Zustand | `npm i zustand` | State | 04-state |
| TanStack Query | `npm i @tanstack/react-query` | Data fetching | 06-data |
| React Hook Form | `npm i react-hook-form` | Forms | 07-forms |
| Zod | `npm i zod` | Validation | 07-forms |
| Axios | `npm i axios` | HTTP client | 06-data |
| clsx | `npm i clsx` | Classnames | 08-styling |
| date-fns | `npm i date-fns` | Date utils | 08-styling |
| Framer Motion | `npm i framer-motion` | Animations | 15-animations |
| React Router | `npm i react-router-dom` | Routing | 05-routing |

---

## React Router Commands

```bash
# Install
npm i react-router-dom

# Basic setup (in App.tsx)
import { BrowserRouter, Routes, Route } from 'react-router-dom';

<BrowserRouter>
  <Routes>
    <Route path="/" element={<Home />} />
    <Route path="/about" element={<About />} />
  </Routes>
</BrowserRouter>
```

---

## TanStack Query Setup

```bash
# Install
npm i @tanstack/react-query

# Wrap app with provider
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const queryClient = new QueryClient();

<QueryClientProvider client={queryClient}>
  <App />
</QueryClientProvider>
```

---

## Vitest + RTL Setup

```bash
# Install
npm i -D vitest @testing-library/react @testing-library/jest-dom jsdom

# Add to package.json
"scripts": {
  "test": "vitest",
  "test:ui": "vitest --ui",
  "test:coverage": "vitest --coverage"
}

# Run tests
npm test              # Watch mode
npm test -- --run     # Single run
npm test -- --coverage # With coverage
```

---

## Playwright Setup

```bash
# Install
npm i -D @playwright/test
npx playwright install --with-deps chromium

# Run tests
npx playwright test              # Run all
npx playwright test auth         # Run auth tests
npx playwright test --ui          # Interactive UI
npx playwright test --reporter=list

# Open Playwright UI
npx playwright test --ui

# Generate tests
npx playwright codegen
```

---

## Docker Commands

```bash
# Build image
docker build -t my-app .

# Run container
docker run -p 3000:3000 my-app

# Docker Compose
docker-compose up           # Start all services
docker-compose up -d       # Detached
docker-compose down         # Stop all
docker-compose down -v      # Remove volumes

# Logs
docker-compose logs -f     # Follow logs
docker-compose logs -f app  # Specific service

# Rebuild
docker-compose up --build
```

---

## Turborepo Commands

```bash
# Install globally
npm i -g turbo

# Run tasks
turbo run build            # Build all
turbo run dev              # Dev all
turbo run test             # Test all
turbo run lint             # Lint all

# Specific app/package
turbo run dev --filter=web
turbo run build --filter=@myorg/ui

# Options
turbo run build --dry      # Dry run
turbo run build --force    # Force rebuild
```

---

## Git Hooks (Husky + lint-staged)

```bash
# Install
npm i -D husky lint-staged

# Initialize
npx husky install
npx husky add .husky/pre-commit "npx lint-staged"

# package.json config
{
  "lint-staged": {
    "*.{ts,tsx}": ["eslint --fix", "vitest --run"]
  }
}
```

---

## Vite Build Commands

```bash
# Development
npm run dev                # Start dev server
npm run dev -- --port 3000 # Custom port

# Production
npm run build              # Build for production
npm run preview            # Preview build

# With environment
npm run build -- --mode production

# Environment files
# .env.development
# .env.production
# .env.local (gitignored)
```

---

## Next.js Commands

```bash
# Create Next.js app
npx create-next-app@latest my-app
npx create-next-app@latest my-app --typescript --tailwind --eslint

# Development
npm run dev

# Production
npm run build
npm run start

# Linting
npm run lint

# Type checking
npm run type-check
```

---

## Package Manager Comparison

```bash
# npm
npm install
npm run
npm test

# pnpm (preferred for monorepos)
pnpm install
pnpm run
pnpm test

# yarn
yarn install
yarn run
yarn test
```

---

## Quick Command Reference

| Task | Command |
|------|---------|
| Install deps | `npm install` |
| Dev server | `npm run dev` |
| Build | `npm run build` |
| Test | `npm test` |
| Lint | `npm run lint` |
| Type check | `npx tsc --noEmit` |
| Format | `npx prettier --write .` |
| Preview build | `npm run preview` |
| Add package | `npm i package-name` |
| Add dev dep | `npm i -D package-name` |

---

## What's Next

- [Hooks Cheatsheet](01-hooks-cheatsheet.md) - React hooks
- [TypeScript Cheatsheet](02-typescript-react-cheatsheet.md) - TS patterns
