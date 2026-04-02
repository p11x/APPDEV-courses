# Publishing Best Practices and Package Management

## What You'll Learn

- Package naming conventions
- Documentation requirements
- Rollback strategies
- Package lifecycle management

## Package Naming Conventions

```
Naming Rules:
─────────────────────────────────────────────
✓ Lowercase only
✓ No spaces
✓ URL-safe characters (no @ at start for unscoped)
✓ Max 214 characters
✓ Scoped: @scope/package-name

Good names:
├── express
├── lodash
├── @babel/core
├── @types/node
└── my-utility-package

Avoid:
├── node-* (reserved for Node.js core)
├── js-* (too generic)
├── _* (private convention)
└── UPPERCASE (not conventional)
```

## Documentation Requirements

```markdown
# README.md Template

## Installation
\`\`\`bash
npm install my-package
\`\`\`

## Usage
\`\`\`javascript
import { helper } from 'my-package';
const result = helper('input');
\`\`\`

## API Reference
### `helper(input, options?)`
- `input` (string) — Input to process
- `options` (object) — Configuration options
- Returns: processed result

## License
MIT
```

## Rollback Strategy

```bash
# Deprecate a bad version
npm deprecate my-package@1.2.3 "Critical bug — use 1.2.2"

# Unpublish (only within 72 hours)
npm unpublish my-package@1.2.3

# Publish fix
npm version patch
npm publish
```

## Best Practices Checklist

- [ ] Use descriptive package names
- [ ] Include comprehensive README
- [ ] Add TypeScript declarations
- [ ] Document all public APIs
- [ ] Use deprecation for bad versions

## Cross-References

- See [Publishing Workflow](./01-publishing-workflow.md) for publishing process
- See [Package Development](../09-package-development/01-creating-packages.md) for development
- See [Monorepo](../07-monorepo-management/01-workspaces.md) for monorepo publishing

## Next Steps

Continue to [Monorepo Management](../07-monorepo-management/01-workspaces.md) for monorepos.
