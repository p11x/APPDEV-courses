# NPM CLI Commands Reference and Advanced Usage

## What You'll Learn

- Complete npm CLI command reference
- Advanced npm commands and flags
- npm scripting and automation
- npm debugging and troubleshooting

## Core NPM Commands

```bash
# ── Package Management ─────────────────────
npm install                    # Install all dependencies
npm install <pkg>              # Install as dependency
npm install --save-dev <pkg>   # Install as devDependency
npm install -g <pkg>           # Install globally
npm install <pkg>@<version>    # Install specific version
npm install <pkg>@<tag>        # Install by tag (latest, next, beta)
npm uninstall <pkg>            # Remove package
npm update                     # Update all packages
npm update <pkg>               # Update specific package
npm outdated                   # Check for outdated packages
npm ls                         # List installed packages
npm ls --depth=0               # Top-level only
npm ls <pkg>                   # Show package dependency tree

# ── Publishing ─────────────────────────────
npm login                      # Login to registry
npm whoami                     # Show current user
npm publish                    # Publish package
npm publish --access public    # Publish scoped package publicly
npm publish --tag beta         # Publish with tag
npm unpublish <pkg>@<version>  # Unpublish version
npm deprecate <pkg> "<msg>"    # Deprecate a version
npm owner add <user> <pkg>     # Add package owner
npm owner ls <pkg>             # List package owners

# ── Scripts ────────────────────────────────
npm start                      # Run "start" script
npm test                       # Run "test" script
npm run <script>               # Run custom script
npm run                        # List available scripts
npm run <script> -- --flag     # Pass flags to script

# ── Configuration ──────────────────────────
npm config list                # Show all config
npm config get <key>           # Get config value
npm config set <key> <value>   # Set config value
npm config delete <key>        # Delete config value
npm config edit                # Edit config file

# ── Audit and Security ────────────────────
npm audit                      # Check for vulnerabilities
npm audit fix                  # Auto-fix vulnerabilities
npm audit fix --force          # Force fix (may break things)
npm audit signatures           # Verify package signatures

# ── Information ────────────────────────────
npm info <pkg>                 # Package details
npm info <pkg> version         # Latest version
npm info <pkg> versions        # All versions
npm search <term>              # Search registry
npm view <pkg> dependencies    # Show dependencies
npm explain <pkg>              # Why is this installed?
npm doctor                     # Check npm environment

# ── Cache ──────────────────────────────────
npm cache clean --force        # Clear cache
npm cache verify               # Verify cache integrity
npm pack                       # Create tarball
npm pack --dry-run             # Preview published files
```

## Advanced Commands

```bash
# ── Deterministic Installs ─────────────────
npm ci                         # Clean install from lock file
# - Deletes node_modules first
# - Strict: fails if package.json ≠ lock file
# - Faster than npm install for CI/CD

# ── Workspace Commands ─────────────────────
npm install -w packages/api    # Install in specific workspace
npm run test -ws               # Run script in all workspaces
npm run test -w packages/api   # Run in specific workspace

# ── Dry Runs ───────────────────────────────
npm install --dry-run          # Preview what would be installed
npm publish --dry-run          # Preview publish
npm uninstall --dry-run        # Preview uninstall

# ── Fund and Acknowledge ───────────────────
npm fund                       # Show funding info
npm fund <pkg>                 # Show specific package funding

# ── Token Management ──────────────────────
npm token create               # Create auth token
npm token create --read-only   # Read-only token
npm token create --cidr=...    # IP-restricted token
npm token list                 # List tokens
npm token revoke <id>          # Revoke token
```

## NPM Scripting

```json
{
    "scripts": {
        "start": "node src/server.js",
        "dev": "node --watch src/server.js",
        "test": "node --test",
        "test:watch": "node --test --watch",
        "test:coverage": "node --test --experimental-test-coverage",
        "lint": "eslint src/",
        "lint:fix": "eslint src/ --fix",
        "format": "prettier --write 'src/**/*.js'",
        "build": "node build.js",
        "prepublishOnly": "npm run build && npm test",
        "postinstall": "node scripts/postinstall.js",
        "preversion": "npm test",
        "postversion": "git push && git push --tags"
    }
}
```

### Lifecycle Scripts

```
npm Lifecycle Script Order:
─────────────────────────────────────────────
npm install:
1. preinstall
2. install
3. postinstall
4. prepublish (deprecated)
5. preprepare
6. prepare
7. postprepare

npm publish:
1. prepublishOnly
2. prepack
3. pack
4. postpack
5. publish
6. postpublish

npm version <type>:
1. preversion
2. version
3. postversion

npm test:
1. pretest
2. test
3. posttest
```

## Debugging NPM

```bash
# Verbose output
npm install --verbose
npm publish --verbose

# Log level
npm install --loglevel silly   # Most detailed
npm install --loglevel verbose
npm install --loglevel info
npm install --loglevel warn
npm install --loglevel error

# Timing
npm install --timing           # Show timing information

# Check npm environment
npm doctor

# Debug specific issues
npm install --prefer-offline   # Use cache, don't fetch
npm install --no-optional      # Skip optional dependencies
npm install --ignore-scripts   # Don't run lifecycle scripts
npm install --force            # Force reinstall
```

## Best Practices Checklist

- [ ] Use `npm ci` in CI/CD pipelines
- [ ] Use `--dry-run` before publish/uninstall
- [ ] Use lifecycle scripts for automation
- [ ] Use `npm doctor` to diagnose issues
- [ ] Use `npm explain` to understand dependency chains

## Cross-References

- See [Registry Structure](./01-registry-structure.md) for registry internals
- See [Package.json](../02-package-json/01-fields-reference.md) for package.json
- See [Dependency Management](../04-dependency-management/01-tree-analysis.md) for dependencies

## Next Steps

Continue to [Package.json Mastery](../02-package-json/01-fields-reference.md) for package.json details.
