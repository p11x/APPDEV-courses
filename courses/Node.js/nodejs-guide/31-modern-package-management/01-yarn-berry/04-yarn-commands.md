# Yarn Commands Reference

## What You'll Learn

- All essential Yarn Berry commands
- Workspace management commands
- Script running and task orchestration
- Cache and cleanup commands

## Core Commands

```bash
# Initialize project
yarn init -2

# Install dependencies
yarn install                    # Install from lock file
yarn install --immutable       # CI mode — fail if lock file changed
yarn install --mode=skip-build # Skip postinstall scripts

# Add dependencies
yarn add express               # Add to dependencies
yarn add -D typescript         # Add to devDependencies
yarn add -P lodash             # Add to peerDependencies
yarn add express@4.18.0        # Specific version
yarn add express@^4.0.0        # Semver range
yarn add express@latest        # Latest version

# Remove dependencies
yarn remove express

# Upgrade dependencies
yarn up                        # Upgrade all
yarn up express                # Upgrade specific package
yarn up '*'                    # Upgrade everything
```

## Workspace Commands

```bash
# Run in specific workspace
yarn workspace @myorg/api dev

# Run in all workspaces
yarn workspaces foreach -A run build

# Flags:
# -A    All workspaces
# -p    Parallel
# -t    Topological order (deps first)
# -R    Include root
# -i    Interlaced output
# -v    Verbose

# Common combinations
yarn workspaces foreach -Apt run build   # All, parallel, topological
yarn workspaces foreach -Ap run test     # All, parallel
```

## Script Running

```bash
# Run script from package.json
yarn dev
yarn build
yarn test

# Run arbitrary command
yarn run eslint .
yarn run prettier --check .

# Run node
yarn node server.js
```

## Cache Commands

```bash
# List cached packages
yarn cache list

# Clean cache
yarn cache clean

# Verify cache integrity
yarn install --check-cache
```

## Info Commands

```bash
# List dependencies
yarn info

# Why is a package installed?
yarn why express

# Check outdated packages
yarn upgrade-interactive

# List workspaces
yarn workspaces list
```

## Constraints (Berry Only)

```yaml
# .yarnrc.yml
constraints:
  - |
    # All packages must have a license
    gen_enforced_field(WorkspaceCwd, 'license', 'MIT').
```

```bash
# Run constraints check
yarn constraints
yarn constraints --fix
```

## Next Steps

For pnpm, continue to [pnpm Setup](../02-pnpm-features/01-pnpm-setup.md).
