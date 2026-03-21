# Workspace Structure

## What You'll Learn
- Organize your monorepo effectively
- Create logical folder structure
- Separate concerns appropriately

## Prerequisites
- Understanding of Turborepo setup

## Do I Need This Right Now?
This helps you organize your monorepo in a way that's maintainable as it grows. Good structure prevents confusion and makes finding code easier.

## Concept Explained Simply

A well-organized monorepo is like a well-organized library. Books are grouped by topic, with clear signs pointing to each section. A messy monorepo is like throwing all books in one pile — impossible to find anything!

## Recommended Structure

```
my-turborepo/
├── .github/              # GitHub Actions CI/CD
│   └── workflows/
├── apps/                 # Deployable applications
│   ├── web/             # Main Next.js app
│   ├── docs/            # Documentation site
│   ├── admin/           # Admin dashboard
│   └── mobile-web/      # Mobile-optimized site
├── packages/            # Shared packages
│   ├── ui/              # Design system components
│   ├── config/          # Shared configuration
│   │   ├── eslint/
│   │   ├── typescript/
│   │   └── tailwind/
│   ├── utils/           # Utility functions
│   ├── hooks/           # Shared React hooks
│   ├── database/        # Database schemas/models
│   └── api/             # Shared API types/clients
├── scripts/             # Build/maintenance scripts
│   └── build-all.ts
├── tooling/             # Development tools
│   └── storybook/
├── turbo.json
├── package.json
└── tsconfig.json
```

## App Structure

```typescript
// apps/web/
├── public/
├── src/
│   ├── app/             # Next.js App Router
│   ├── components/     # App-specific components
│   ├── lib/            # App-specific utilities
│   └── styles/          # App-specific styles
├── .env.local
├── next.config.js
├── package.json
└── tsconfig.json
```

## Package Structure

```typescript
// packages/ui/
├── src/
│   ├── components/     # Component files
│   │   ├── Button/
│   │   │   ├── Button.tsx
│   │   │   ├── Button.stories.tsx
│   │   │   └── Button.test.tsx
│   │   └── Input/
│   ├── hooks/         # Package-specific hooks
│   ├── index.ts       # Main exports
│   └── theme.ts       # Theme configuration
├── .eslintrc.json
├── package.json
├── tsconfig.json
└── turbo.json         # Optional: package-specific config
```

## Shared Configuration

```typescript
// packages/config/typescript/base.json
{
  "compilerOptions": {
    "target": "ES2020",
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
    "noFallthroughCasesInSwitch": true
  }
}
```

```typescript
// packages/config/typescript/nextjs.json
{
  "extends": "./base.json",
  "compilerOptions": {
    "jsx": "preserve",
    "plugins": [{ "name": "next" }]
  }
}
```

```typescript
// packages/config/eslint/react.js
module.exports = {
  extends: [
    'next/core-web-vitals',
    'plugin:@typescript-eslint/recommended',
  ],
  rules: {
    '@typescript-eslint/no-unused-vars': 'warn',
  },
};
```

## Exports Structure

```typescript
// packages/ui/src/index.ts

// Export components
export * from './components/Button';
export * from './components/Input';
export * from './components/Card';

// Export hooks
export * from './hooks/useCounter';

// Export types
export type { ButtonProps } from './components/Button';
export type { InputProps } from './components/Input';
```

## Common Mistakes

### Mistake #1: Flat Structure
```typescript
// Wrong: Everything in root
/
├── web/
├── docs/
├── admin/
├── button.tsx        // Should be in a package!
├── utils.ts
└── types.ts
```

```typescript
// Better: Organized structure
apps/
├── web/
├── docs/
└── admin/
packages/
├── ui/
│   └── src/
│       └── button.tsx
└── utils/
    └── src/
        └── utils.ts
```

### Mistake #2: Not Using Consistent Naming
```typescript
// Wrong: Inconsistent package names
apps/web/
packages/ui-components/     // Different from below!
packages/utils/
packages/ts-config/         // Different naming convention
```

```typescript
// Correct: Consistent naming
apps/
  web/
  docs/
packages/
  ui/
  utils/
  config/
```

### Mistake #3: Too Many Nested Levels
```typescript
// Wrong: Too deep
packages/
  shared/
    ui/
      components/
        buttons/
          primary/
            PrimaryButton.tsx
```

```typescript
// Better: Flatter is easier to navigate
packages/
  ui/
    src/
      components/
        Button.tsx      // Simple!
```

## Summary
- Organize apps in `apps/` folder
- Shared code goes in `packages/`
- Group related config in `packages/config/`
- Keep TypeScript configs extendable
- Use consistent naming across packages
- Prefer flatter over deeply nested
- Document your structure in README

## Next Steps
- [shared-packages.md](./shared-packages.md) — Creating and using shared packages
