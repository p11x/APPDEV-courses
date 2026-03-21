# When to Use a Monorepo

## What You'll Learn
- Identify when a monorepo makes sense
- Understand trade-offs of monorepo vs polyrepo
- Make informed architectural decisions

## Prerequisites
- Basic understanding of monorepos and Turborepo

## Do I Need This Right Now?
Not every project needs a monorepo. This guide helps you decide if the complexity is worth it for your situation. If you're just starting with a single Next.js app, you probably don't need this yet.

## Concept Explained Simply

A monorepo is like having all your tools in one big toolbox. A polyrepo (multiple repos) is like having separate toolboxes for each job. Both work, but:

- **Monorepo:** Everything in one place, easy to share, but can get messy
- **Polyrepo:** Clean separation, but harder to share code and coordinate changes

## When Monorepo Makes Sense

### Good Candidates for Monorepo:

1. **Multiple Apps That Share Code**
   - Main website + marketing site + dashboard
   - Mobile web + desktop web
   - Multiple micro-frontends

2. **Shared Design System**
   - Button, input, card components used everywhere
   - Consistent styling across products
   - Single source of truth for UI

3. **Shared Business Logic**
   - Same validation rules in multiple apps
   - Common API clients
   - Authentication utilities

4. **Team Coordination**
   - Need to make atomic changes across projects
   - Many developers working on related code
   - Want to avoid dependency hell

### When to Stick with Single Repo:

1. **Single Application**
   - One Next.js app, nothing else
   - No plans to add more apps
   - Simple dependencies

2. **Small Team**
   - Just 1-3 developers
   - Projects are unrelated
   - Low coordination overhead

3. **Different Deployment Needs**
   - Each project deploys independently to different targets
   - Different release cycles
   - No shared code

## Comparison Table

| Factor | Monorepo | Polyrepo |
|--------|----------|----------|
| **Sharing Code** | Easy | Harder |
| **Atomic Changes** | Yes | No |
| **Build Speed** | Faster with Turborepo | Usually fine |
| **Complexity** | Higher | Lower |
| **CI/CD** | More complex | Simpler |
| **Access Control** | All or nothing | Granular |
| **Onboarding** | Steeper | Easier |

## Signs You Need a Monorepo

- Copy-pasting code between projects
- Multiple package.json files with same dependencies
- Changes require updating multiple repos
- Hard to keep dependencies in sync
- Team spends time coordinating releases

## Signs You Don't Need One

- One repo, one app
- No shared code between projects
- Projects have different teams
- Simple deployment needs

## Common Mistakes

### Mistake #1: Premature Monorepo
```typescript
// Starting with monorepo for a single app
// Root
/
├── apps/
│   └── web/
│       └── package.json  // Only one app!
└── packages/
    └── empty/            // Nothing here yet!
```

This adds complexity without benefit. Start simple!

### Mistake #2: Not Using Turborepo
```typescript
// Monorepo without build optimization
// Every build runs everything
// Slow and wasteful!
```

Use Turborepo to get the benefits.

### Mistake #3: Too Many Shared Packages
```typescript
// Creating packages for everything
packages/
├── utils-string/
├── utils-number/
├── utils-array/
├── utils-object/
// Too granular! Just use utilities
```

Package per feature/domain, not per tiny utility.

## Summary
- Monorepos shine when sharing code across multiple apps
- Don't use monorepo for a single project
- Turborepo makes monorepos practical
- Consider team size and coordination needs
- Start simple, add complexity only when needed
- Signs you need one: copying code, dependency sync issues

## Next Steps
- [create-turbo.md](../02-turborepo-setup/create-turbo.md) — Setting up Turborepo
