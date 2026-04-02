# pnpm Workspace

## What You'll Learn

- How to set up pnpm workspaces
- How to manage dependencies across workspaces
- How to run scripts in workspaces
- How to filter workspaces

## Setup

```yaml
# pnpm-workspace.yaml

packages:
  - 'packages/*'
  - 'apps/*'
  - 'tools/*'
```

## Project Structure

```
my-monorepo/
├── pnpm-workspace.yaml
├── package.json          → Root (private: true)
├── pnpm-lock.yaml
├── packages/
│   ├── ui/
│   │   └── package.json → @myorg/ui
│   └── utils/
│       └── package.json → @myorg/utils
└── apps/
    ├── web/
    │   └── package.json → @myorg/web
    └── api/
        └── package.json → @myorg/api
```

## Commands

```bash
# Install all workspace dependencies
pnpm install

# Add dependency to specific workspace
pnpm --filter @myorg/api add express
pnpm -F @myorg/web add react

# Run script in specific workspace
pnpm --filter @myorg/api dev
pnpm -F @myorg/web build

# Run in all workspaces
pnpm -r run build

# Run in topological order
pnpm -r --workspace-concurrency=1 run build

# Run only affected packages
pnpm -r --filter='...[origin/main]' run test
```

## Workspace Protocol

```json
// packages/api/package.json

{
  "dependencies": {
    "@myorg/utils": "workspace:*"
  }
}
```

| Protocol | Version |
|----------|---------|
| `workspace:*` | Any version |
| `workspace:^` | Semver caret |
| `workspace:~` | Semver tilde |

## Next Steps

For pnpm vs npm, continue to [pnpm vs npm](./04-pnpm-vs-npm.md).
