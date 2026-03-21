# Intermediate Learning Path

## Overview
This path is for developers who've built at least one React application and understand hooks, state management, and routing basics. You'll level up with TypeScript, performance optimization, testing, and Next.js.

**Estimated Time:** ~30 hours  
**Prerequisites:** Completed Beginner Path or equivalent experience

## Who This Path Is For

- You've built at least one React app (CRUD operations)
- You're comfortable with useState, useEffect, and useContext
- You understand component composition patterns
- You've used React Router for navigation

## Phase 1: TypeScript + Advanced Patterns (Week 1-2)

TypeScript transforms React development. Learn to build type-safe applications.

### Week 1: TypeScript Fundamentals

| Day | File | Description |
|-----|------|-------------|
| 1 | [01-typescript-setup-with-vite.md](../12-typescript/01-ts-fundamentals/01-typescript-setup-with-vite.md) | Adding TypeScript to React |
| 2 | [02-types-vs-interfaces-in-react.md](../12-typescript/01-ts-fundamentals/02-types-vs-interfaces-in-react.md) | Choosing types vs interfaces |
| 3 | [03-generics-in-react.md](../12-typescript/01-ts-fundamentals/03-generics-in-react.md) | Building reusable components |
| 4 | [01-typing-props-and-state.md](../12-typescript/02-typing-react/01-typing-props-and-state.md) | Type-safe components |
| 5 | [02-typing-hooks.md](../12-typescript/02-typing-react/02-typing-hooks.md) | Typed custom hooks |
| 6 | [03-typing-events-and-handlers.md](../12-typescript/02-typing-react/03-typing-events-and-handlers.md) | Event handling types |
| 7 | Review & Practice | Convert a JS app to TypeScript |

**Milestone Checkpoint:** You can confidently type React components, hooks, and event handlers.

### Week 2: Advanced TypeScript Patterns

| Day | File | Description |
|-----|------|-------------|
| 8 | [01-discriminated-unions-for-ui.md](../12-typescript/03-advanced-ts-patterns/01-discriminated-unions-for-ui.md) | Type-safe state machines |
| 9 | [02-utility-types-in-react.md](../12-typescript/03-advanced-ts-patterns/02-utility-types-in-react.md) | Partial, Pick, Omit |
| 10 | [04-typing-context-api.md](../12-typescript/02-typing-react/04-typing-context-api.md) | Type-safe Context |
| 11 | [03-satisfies-operator-and-const-assertions.md](../12-typescript/03-advanced-ts-patterns/03-satisfies-operator-and-const-assertions.md) | Const assertions |
| 12-14 | Project | Build a type-safe dashboard |

**Milestone Checkpoint:** You can create fully typed libraries and shared components.

## Phase 2: Performance + Testing (Week 3)

Make your apps fast and reliable with optimization and testing.

### Week 3: Performance & Testing

| Day | File | Description |
|-----|------|-------------|
| 15 | [01-understanding-react-renders.md](../09-performance/01-rendering-optimization/01-understanding-react-renders.md) | How React renders work |
| 16 | [02-react-memo-explained.md](../09-performance/01-rendering-optimization/02-react-memo-explained.md) | Preventing re-renders |
| 17 | [03-avoiding-unnecessary-rerenders.md](../09-performance/01-rendering-optimization/03-avoiding-unnecessary-rerenders.md) | Performance patterns |
| 18 | [01-vitest-setup-with-react.md](../10-testing/01-unit-testing/01-vitest-setup-with-react.md) | Setting up Vitest |
| 19 | [02-testing-components-with-rtl.md](../10-testing/01-unit-testing/02-testing-components-with-rtl.md) | Testing React components |
| 20 | [03-testing-custom-hooks.md](../10-testing/01-unit-testing/03-testing-custom-hooks.md) | Testing hooks |
| 21 | [01-user-event-testing.md](../10-testing/02-integration-testing/01-user-event-testing.md) | Integration testing |

**Milestone Checkpoint:** You can write tests for components and hooks, and optimize render performance.

## Phase 3: Next.js & Architecture (Week 4)

Build production-ready applications with Next.js and enterprise patterns.

### Week 4: Next.js Fundamentals

| Day | File | Description |
|-----|------|-------------|
| 22 | [01-nextjs-vs-react-spa.md](../13-nextjs/01-nextjs-foundations/01-nextjs-vs-react-spa.md) | Next.js vs React SPA |
| 23 | [02-app-router-fundamentals.md](../13-nextjs/01-nextjs-foundations/02-app-router-fundamentals.md) | App Router basics |
| 24 | [01-server-components-explained.md](../13-nextjs/02-rendering-strategies/01-server-components-explained.md) | Server vs Client Components |
| 25 | [02-static-generation-ssg.md](../13-nextjs/02-rendering-strategies/02-static-generation-ssg.md) | Static Site Generation |
| 26 | [04-incremental-static-regeneration-isr.md](../13-nextjs/02-rendering-strategies/04-incremental-static-regeneration-isr.md) | ISR patterns |
| 27 | [01-nextjs-api-routes.md](../13-nextjs/03-nextjs-features/01-nextjs-api-routes.md) | API routes |
| 28-30 | Project | Build a Next.js blog with SSG |

**Milestone Checkpoint:** You can build SEO-friendly applications with Next.js App Router.

## What to Skip

These files **overlap with beginner knowledge** — you already know this:

- Basic useState and useEffect (you use these daily)
- CSS Modules fundamentals (covered in beginner path)
- Basic React Router (you've used it before)
- Simple form handling (you've built forms)

Focus on:
- TypeScript integration
- Performance optimization
- Testing
- Next.js specifics

## Phase 4: Production Readiness (Week 5)

Deploy with confidence and prepare for interviews.

| Day | File | Description |
|-----|------|-------------|
| 31 | [01-deploying-react-to-vercel.md](../11-real-world-projects/03-full-stack-integration/03-deploying-react-to-vercel.md) | Deployment |
| 32 | [01-jwt-authentication-flow.md](../11-real-world-projects/01-auth-system/01-jwt-authentication-flow.md) | Auth patterns |
| 33 | [01-react-query-with-rest-api.md](../11-real-world-projects/03-full-stack-integration/02-react-query-with-rest-api.md) | Data fetching at scale |
| 34 | [02-mocking-api-calls.md](../10-testing/02-integration-testing/02-mocking-api-calls.md) | Testing API code |
| 35 | [01-playwright-setup.md](../10-testing/03-e2e-testing/01-playwright-setup.md) | E2E testing |

## Key Takeaways

After completing this path, you will be able to:

1. ✅ Build type-safe React applications with TypeScript
2. ✅ Optimize performance with memoization and code splitting
3. ✅ Write comprehensive tests (unit, integration, E2E)
4. ✅ Build production apps with Next.js App Router
5. ✅ Deploy to production with CI/CD
6. ✅ Implement authentication flows

## Next Steps

Ready to go deeper? Head to the [Advanced Learning Path](advanced-path.md) for:

- React internals (Reconciliation, Fiber)
- Design patterns and architecture
- Interview preparation
- Micro-frontends and scaling

**Or explore specific interests:**
- [Accessibility](../14-accessibility/01-a11y-foundations/01-wcag-guidelines-for-react.md)
- [Animations with Framer Motion](../15-animations/02-framer-motion/01-framer-motion-setup.md)
- [Monorepos with Turborepo](../18-ecosystem/02-monorepos/01-turborepo-setup.md)
