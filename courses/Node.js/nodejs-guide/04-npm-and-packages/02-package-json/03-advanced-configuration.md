# Package.json Advanced Configuration

## What You'll Learn

- Conditional exports and subpath patterns
- Binary executables configuration
- Package metadata optimization
- Workspaces configuration

## Conditional Exports

```json
{
    "exports": {
        ".": {
            "types": "./dist/index.d.ts",
            "import": "./dist/index.mjs",
            "require": "./dist/index.cjs",
            "default": "./dist/index.js"
        },
        "./utils": {
            "types": "./dist/utils.d.ts",
            "import": "./dist/utils.mjs",
            "require": "./dist/utils.cjs"
        },
        "./package.json": "./package.json"
    }
}
```

## Binary Executables

```json
{
    "name": "my-cli-tool",
    "bin": {
        "my-cli": "./bin/cli.js",
        "my-other-tool": "./bin/other.js"
    }
}
```

```javascript
#!/usr/bin/env node
// bin/cli.js — Must have shebang line
import { parseArgs } from 'node:util';

const { values, positionals } = parseArgs({
    options: {
        help: { type: 'boolean', short: 'h' },
        version: { type: 'boolean', short: 'v' },
    },
    allowPositionals: true,
});

if (values.version) {
    console.log('1.0.0');
    process.exit(0);
}

console.log('Running my-cli...');
```

## Workspaces Configuration

```json
{
    "private": true,
    "workspaces": [
        "packages/*",
        "apps/*",
        "tools/*"
    ]
}
```

```bash
# Install all workspace dependencies
npm install

# Add dependency to specific workspace
npm install express -w packages/api

# Run script in specific workspace
npm run test -w packages/utils

# Run script in all workspaces
npm run build --workspaces
```

## Best Practices Checklist

- [ ] Use `exports` for conditional module resolution
- [ ] Include `./package.json` in exports
- [ ] Add shebang line to CLI scripts
- [ ] Use workspaces for monorepos
- [ ] Set `private: true` for monorepo roots

## Cross-References

- See [Fields Reference](./01-fields-reference.md) for basic fields
- See [Scripts](./02-scripts-automation.md) for npm scripts
- See [Monorepo](../07-monorepo-management/01-workspaces.md) for monorepos

## Next Steps

Continue to [Semantic Versioning](../03-semantic-versioning/01-version-ranges.md) for versioning.
