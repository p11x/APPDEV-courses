# Beginner Learning Path

## Overview
This curated path is designed for developers who know JavaScript fundamentals but are new to React. Follow this path to build a solid foundation in React development, going from "hello world" to your first complete React application.

**Estimated Time:** ~40 hours  
**Prerequisites:** Basic JavaScript, HTML, CSS, terminal usage

## Who This Path Is For

- You've written JavaScript but haven't used React
- You understand variables, functions, arrays, objects, and ES6+ syntax
- You've built static websites with HTML/CSS
- You're comfortable using the command line

## Phase 1: Core React Fundamentals (Week 1-2)

This phase builds your foundation. Don't skip—these concepts apply to everything else!

### Week 1: React Basics & Components

| Day | File | Description |
|-----|------|-------------|
| 1 | [01-intro-to-react.md](../01-foundations/01-what-is-react/01-intro-to-react.md) | What is React and why it matters |
| 2 | [02-virtual-dom-explained.md](../01-foundations/01-what-is-react/02-virtual-dom-explained.md) | How React updates the DOM efficiently |
| 3 | [03-react-vs-vanilla-js.md](../01-foundations/01-what-is-react/03-react-vs-vanilla-js.md) | Comparing React approach to vanilla JS |
| 4 | [01-create-react-app-vs-vite.md](../01-foundations/02-setup-and-tooling/01-create-react-app-vs-vite.md) | Setting up your development environment |
| 5 | [01-creating-your-first-component.md](../02-components/01-functional-components/01-creating-your-first-component.md) | Your first React component |
| 6-7 | [02-props-explained.md](../02-components/01-functional-components/02-props-explained.md) | Passing data to components |

**Milestone Checkpoint:** You can create a React app and build a simple component that accepts props.

### Week 2: JSX & State

| Day | File | Description |
|-----|------|-------------|
| 8 | [01-jsx-syntax-rules.md](../01-foundations/03-jsx-deep-dive/01-jsx-syntax-rules.md) | JSX syntax fundamentals |
| 9 | [02-expressions-in-jsx.md](../01-foundations/03-jsx-deep-dive/02-expressions-in-jsx.md) | Using JavaScript in JSX |
| 10 | [01-useState-complete-guide.md](../03-hooks/01-core-hooks/01-useState-complete-guide.md) | Adding state to components |
| 11 | [02-useEffect-complete-guide.md](../03-hooks/01-core-hooks/02-useEffect-complete-guide.md) | Side effects and lifecycle |
| 12 | [03-useRef-complete-guide.md](../03-hooks/01-core-hooks/03-useRef-complete-guide.md) | Working with references |
| 13-14 | Review & Practice | Build a counter app from scratch |

**Milestone Checkpoint:** You can build an interactive counter with increment/decrement buttons using useState and useEffect.

## Phase 2: Building Real Applications (Week 3-4)

Now apply what you've learned to build actual features.

### Week 3: Data & Events

| Day | File | Description |
|-----|------|-------------|
| 15 | [01-lifting-state-up.md](../04-state-management/01-local-state/01-lifting-state-up.md) | Sharing state between components |
| 16 | [01-building-controlled-forms.md](../07-forms/01-controlled-forms/01-building-controlled-forms.md) | Building forms with React |
| 17 | [01-fetch-api-in-react.md](../06-data-fetching/01-fetch-api-and-axios/01-fetch-api-in-react.md) | Fetching data from APIs |
| 18 | [01-react-router-basics.md](../05-routing/01-react-router-basics/01-setting-up-react-router-v6.md) | Adding navigation to apps |
| 19 | [02-route-params-and-nested-routes.md](../05-routing/01-react-router-basics/02-route-params-and-nested-routes.md) | Dynamic routing |
| 20 | [03-programmatic-navigation.md](../05-routing/01-react-router-basics/03-programmatic-navigation.md) | Navigation in code |
| 21 | Review & Practice | Build a TODO app with localStorage |

**Milestone Checkpoint:** You can build a TODO app with add/delete/toggle, persisted to localStorage.

### Week 4: Styling & Project Structure

| Day | File | Description |
|-----|------|-------------|
| 22 | [01-css-modules-setup.md](../08-styling/01-css-modules/01-css-modules-setup.md) | Styling components with CSS Modules |
| 23 | [02-dynamic-class-names.md](../08-styling/01-css-modules/02-dynamic-class-names.md) | Conditional styling |
| 24 | [02-responsive-design-in-tailwind.md](../08-styling/02-tailwind-css/02-responsive-design-in-tailwind.md) | Responsive layouts |
| 25 | [01-project-structure-best-practices.md](../01-foundations/02-setup-and-tooling/02-project-structure-best-practices.md) | Organizing project files |
| 26-28 | Portfolio Project | Build a weather app or blog |

**Milestone Checkpoint:** You can create a complete, styled application with multiple pages.

## Phase 3: Level Up (Week 5)

Add professional-grade patterns to your toolkit.

### Week 5: Advanced Patterns

| Day | File | Description |
|-----|------|-------------|
| 29 | [01-children-prop.md](../02-components/02-component-composition/01-children-prop.md) | Component composition |
| 30 | [02-component-slots-pattern.md](../02-components/02-component-composition/02-component-slots-pattern.md) | Flexible component APIs |
| 31 | [01-context-api-basics.md) | Global state with Context |
| 32 | [02-consuming-context-with-hooks.md](../04-state-management/02-context-api/02-consuming-context-with-hooks.md) | Using context in components |
| 33 | [01-tanstack-query-setup.md](../06-data-fetching/02-react-query/01-tanstack-query-setup.md) | Professional data fetching |
| 34 | [01-protected-routes-auth-guard.md](../05-routing/02-advanced-routing/01-protected-routes-auth-guard.md) | Authentication flow |
| 35-40 | Final Project | Build a full CRUD app with auth |

**Milestone Checkpoint:** You can build a complete application with authentication, data fetching, and CRUD operations.

## What to Skip

These files are **too advanced** for the beginner path—save them for later:

- [01-useReducer-for-complex-state.md](../03-hooks/02-advanced-hooks/01-useReducer-for-complex-state.md) — After you've built several apps
- [03-useMemo-and-useCallback.md](../03-hooks/02-advanced-hooks/03-useMemo-and-useCallback.md) — When debugging performance
- [01-react-memo-explained.md](../09-performance/01-rendering-optimization/02-react-memo-explained.md) — After learning performance
- [01-micro-frontend-introduction.md](../16-architecture/03-scalability/01-micro-frontend-introduction.md) — For advanced architecture

## Key Takeaways

After completing this path, you will be able to:

1. ✅ Create React applications from scratch using Vite
2. ✅ Build interactive UI with components, props, and state
3. ✅ Handle user input with controlled forms
4. ✅ Fetch and display data from APIs
5. ✅ Navigate between pages with React Router
6. ✅ Style applications with CSS Modules or Tailwind
7. ✅ Manage global state with Context
8. ✅ Build complete CRUD applications

## Next Steps

Ready to continue? Head to the [Intermediate Learning Path](intermediate-path.md) to learn TypeScript, performance optimization, testing, and Next.js.

**Or jump directly to topics that interest you:**
- [TypeScript fundamentals](../12-typescript/01-ts-fundamentals/01-typescript-setup-with-vite.md)
- [Testing with Vitest](../10-testing/01-unit-testing/01-vitest-setup-with-react.md)
- [Next.js introduction](../13-nextjs/01-nextjs-foundations/01-nextjs-vs-react-spa.md)
