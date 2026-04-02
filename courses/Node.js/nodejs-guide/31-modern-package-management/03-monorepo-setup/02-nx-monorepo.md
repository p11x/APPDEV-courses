# Nx Monorepo

## What You'll Learn

- What Nx is and how it works
- How to set up an Nx workspace
- How Nx caching works
- How Nx compares to other monorepo tools

## What Is Nx?

Nx is a build system with **smart computation caching** and **project graph analysis**. It knows which projects depend on which and only rebuilds what changed.

## Setup

```bash
# Create Nx workspace
npx create-nx-workspace@latest my-workspace

# Or add Nx to existing project
npx nx init
```

## Project Structure

```
my-workspace/
├── nx.json              → Nx configuration
├── package.json
├── apps/
│   ├── api/
│   └── web/
├── libs/
│   ├── shared/
│   └── ui/
└── tools/
```

## Commands

```bash
# Run targets
nx build api            # Build specific project
nx test web             # Test specific project
nx run-many -t build    # Build all projects
nx run-many -t build --projects=api,web  # Build specific

# Affected commands (only changed + dependent projects)
nx affected -t build
nx affected -t test
nx affected -t lint

# Graph visualization
nx graph                # Opens interactive dependency graph
```

## Caching

Nx caches task outputs. If inputs haven't changed, it restores from cache:

```json
// nx.json
{
  "targetDefaults": {
    "build": {
      "cache": true,
      "inputs": ["default", "^production"],
      "dependsOn": ["^build"]
    },
    "test": {
      "cache": true,
      "inputs": ["default", "^production", "{workspaceRoot}/jest.config.ts"]
    }
  }
}
```

## Next Steps

For Turborepo, continue to [Turbo Repo](./03-turbo-repo.md).
