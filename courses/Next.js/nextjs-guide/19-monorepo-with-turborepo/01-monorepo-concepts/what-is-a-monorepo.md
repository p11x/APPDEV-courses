# What Is a Monorepo

## What You'll Learn
- Understanding monorepos
- Benefits and challenges
- How it compares to polyrepos
- When to consider a monorepo

## Prerequisites
- Basic JavaScript/TypeScript knowledge
- Understanding of package management
- Familiarity with multiple projects

## Concept Explained Simply

A monorepo (monolithic repository) is when you keep all your projects in a single git repository. Instead of having separate repos for your frontend, backend, design system, and utilities, they're all in one place.

Think of it like a big office vs. multiple offices. In a polyrepo (many repos), each team works in their own building and has to coordinate when they need something from another team. In a monorepo, everyone's in the same building — easy to share code and coordinate changes.

## Monorepo vs Polyrepo

### Polyrepo (Many Repos)

```
repo-frontend/     ← Separate git repo
repo-backend/      ← Separate git repo  
repo-shared/      ← Separate git repo
```

### Monorepo (One Repo)

```
my-app/
├── apps/
│   ├── web/           ← Your Next.js app
│   ├── mobile/        ← React Native app
│   └── admin/        ← Another web app
├── packages/
│   ├── ui/           ← Shared UI components
│   ├── utils/        ← Shared utilities
│   └── config/      ← Shared configs
└── turbo.json        ← Turborepo config
```

## Benefits

1. **Easy code sharing** — One import path for shared code
2. **Atomic changes** — Change API and update all consumers in one PR
3. **Unified tooling** — One lint, one build, one test config
4. **Developer mobility** — Anyone can work on any project
5. **Simpler dependency management** — One node_modules (mostly)

## Challenges

1. **Repository size** — Gets large over time
2. **CI/CD complexity** — Need to build/test intelligently
3. **Access control** — Can't easily restrict by repo
4. **Learning curve** — More complex structure

## When to Use a Monorepo

### Good for Monorepo:
- Multiple apps sharing code
- Design system/shared components
- Micro-frontends architecture
- Team needing atomic changes
- Company with multiple related projects

### Stick with Polyrepo:
- One or two simple projects
- Teams working on completely unrelated things
- Strict access control requirements
- Open source projects

## Tools

- **Turborepo** — Build system, smart caching
- **Nx** — Full monorepo toolkit
- **npm workspaces** — Built-in npm support
- **Yarn workspaces** — Yarn's solution
- **pnpm workspaces** — pnpm's solution

## Summary

- Monorepo = one repo containing multiple projects
- Makes sharing code between projects easy
- Tools like Turborepo make it practical
- Consider your team's actual needs before adopting

## Next Steps

- [turborepo-overview.md](./turborepo-overview.md) - Turborepo introduction
- [create-turbo.md](../02-turborepo-setup/create-turbo.md) - Setting up Turborepo
