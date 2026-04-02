# NPM Registry Structure and Package Resolution

## What You'll Learn

- npm registry architecture and internals
- Package resolution algorithm
- npmrc configuration deep dive
- Custom registry setup and authentication

## NPM Registry Architecture

```
NPM Registry Ecosystem:
─────────────────────────────────────────────
┌─────────────────────────────────────────┐
│           npm CLI (client)              │
│  ┌─────────┐ ┌─────────┐ ┌─────────┐  │
│  │ install │ │ publish │ │ search  │  │
│  └────┬────┘ └────┬────┘ └────┬────┘  │
│       │           │           │        │
│       └───────────┼───────────┘        │
│                   │                    │
│           ┌───────┴───────┐            │
│           │  npm registry │            │
│           │  (registry.   │            │
│           │   npmjs.org)  │            │
│           └───────┬───────┘            │
│                   │                    │
│       ┌───────────┼───────────┐        │
│       │           │           │        │
│  ┌────┴────┐ ┌────┴────┐ ┌────┴────┐  │
│  │ Package │ │  CDN /  │ │  Audit  │  │
│  │  API    │ │  Tarball│ │  DB     │  │
│  └─────────┘ └─────────┘ └─────────┘  │
└─────────────────────────────────────────┘
```

### Package Metadata Structure

```json
// Each package in the registry has this structure
{
    "name": "express",
    "description": "Fast, unopinionated, minimalist web framework",
    "dist-tags": { "latest": "4.21.0", "next": "5.0.0" },
    "versions": {
        "4.21.0": {
            "name": "express",
            "version": "4.21.0",
            "description": "...",
            "main": "index.js",
            "dependencies": { "accepts": "~1.3.8", "body-parser": "^1.20.3" },
            "devDependencies": { "eslint": "^9.0.0" },
            "dist": {
                "tarball": "https://registry.npmjs.org/express/-/express-4.21.0.tgz",
                "shasum": "abc123...",
                "integrity": "sha512-..."
            },
            "engines": { "node": ">= 0.10.0" },
            "license": "MIT"
        }
    },
    "time": { "created": "...", "modified": "...", "4.21.0": "..." },
    "maintainers": [{ "name": "dougwilson", "email": "..." }],
    "readme": "# Express\n..."
}
```

## Package Resolution Algorithm

```
npm install <package> Resolution:
─────────────────────────────────────────────
1. Read package.json dependencies
   │
2. For each dependency:
   ├─ Check node_modules/ for existing
   │  ├─ Found + version matches? → Use existing
   │  └─ Not found or wrong version? → Continue
   │
3. Fetch package metadata from registry
   ├─ GET registry.npmjs.org/<package>
   └─ Returns all versions + dist-tags
   │
4. Resolve version range to specific version
   ├─ "^4.18.0" → satisfies(>=4.18.0 <5.0.0)
   ├─ "~4.18.0" → satisfies(>=4.18.0 <4.19.0)
   └─ "4.18.0"  → exactly 4.18.0
   │
5. Download tarball
   ├─ GET <tarball-url>
   ├─ Verify integrity (sha512 hash)
   └─ Extract to node_modules/<package>
   │
6. Recursively resolve sub-dependencies
   ├─ Read <package>/package.json
   ├─ Resolve its dependencies
   └─ Check for conflicts with existing
   │
7. Write package-lock.json
   └─ Lock exact versions for reproducibility
```

### Dependency Tree Resolution

```
Dependency Tree Example:
─────────────────────────────────────────────
my-app@1.0.0
├── express@4.21.0
│   ├── accepts@1.3.8
│   │   ├── mime-types@2.1.35
│   │   │   └── mime-db@1.52.0
│   │   └── negotiator@0.6.3
│   ├── body-parser@1.20.3
│   │   ├── content-type@1.0.5
│   │   ├── raw-body@2.5.2
│   │   └── debug@2.6.9
│   └── cookie@0.6.0
└── lodash@4.17.21

npm uses a "flat" node_modules layout when possible:
node_modules/
├── express/
├── accepts/
├── mime-types/
├── mime-db/
├── negotiator/
├── body-parser/
├── lodash/
└── ...
```

## npmrc Configuration

### Configuration Files (Precedence)

```
Configuration Precedence (highest to lowest):
─────────────────────────────────────────────
1. Command-line flags       (--registry=https://...)
2. Environment variables    (npm_config_registry=...)
3. Project .npmrc           (./.npmrc)
4. User .npmrc              (~/.npmrc)
5. Global .npmrc            ($PREFIX/etc/npmrc)
6. Built-in defaults
```

### .npmrc Configuration Reference

```ini
# ── Registry Configuration ──────────────────
registry=https://registry.npmjs.org/
# Scoped registry
@myorg:registry=https://npm.myorg.com/
# Multiple scopes
@scope1:registry=https://registry1.com/
@scope2:registry=https://registry2.com/

# ── Authentication ──────────────────────────
# Token-based auth (recommended for CI/CD)
//registry.npmjs.org/:_authToken=${NPM_TOKEN}
# Basic auth (deprecated)
//registry.npmjs.org/:username=myuser
//registry.npmjs.org/:_password=base64encoded
//registry.npmjs.org/:email=user@example.com

# ── Behavior Configuration ─────────────────
save-exact=true              # Save exact versions (no ^ or ~)
save-prefix=""               # Don't add prefix to versions
engine-strict=true           # Enforce engines field
audit-level=high             # Fail on high/critical vulnerabilities
fund=false                   # Don't show funding messages
progress=false               # Disable progress bar

# ── Proxy Configuration ────────────────────
proxy=http://proxy.company.com:8080
https-proxy=http://proxy.company.com:8080
no-proxy=localhost,127.0.0.1,.company.com

# ── Cache Configuration ────────────────────
cache=~/.npm/_cache
tmp=/tmp/npm
prefer-offline=false         # Prefer cached packages

# ── Scripts Configuration ──────────────────
ignore-scripts=false         # Run lifecycle scripts
script-shell=/bin/bash       # Shell for scripts

# ── Package Configuration ──────────────────
package-lock=true            # Generate package-lock.json
shrinkwrap=false             # Don't use npm-shrinkwrap.json
legacy-peer-deps=false       # Strict peer dependency resolution
```

### Environment-Specific Configuration

```bash
# .npmrc.development
registry=https://registry.npmjs.org/
save-exact=true
progress=true

# .npmrc.production
registry=https://registry.npmjs.org/
audit-level=high
engine-strict=true
ignore-scripts=true

# .npmrc.ci
registry=https://registry.npmjs.org/
//registry.npmjs.org/:_authToken=${NPM_TOKEN}
audit-level=critical
fund=false
progress=false

# Use with: npm ci --userconfig .npmrc.ci
```

## Custom Registry Setup

```bash
# Using Verdaccio (lightweight private registry)
npm install -g verdaccio
verdaccio  # Starts on http://localhost:4873

# Configure npm to use it
npm set registry http://localhost:4873

# Publish to local registry
npm publish --registry http://localhost:4873

# Install from local registry
npm install my-package --registry http://localhost:4873
```

```javascript
// verdaccio.yaml — Verdaccio configuration
storage: ./storage
plugins: ./plugins

web:
    title: Private NPM Registry

auth:
    htpasswd:
        file: ./htpasswd
        max_users: 100

uplinks:
    npmjs:
        url: https://registry.npmjs.org/
        timeout: 30s

packages:
    '@myorg/*':
        access: $authenticated
        publish: $authenticated
        proxy: npmjs
    '**':
        access: $all
        publish: $authenticated
        proxy: npmjs

server:
    keepAliveTimeout: 60

middlewares:
    audit:
        enabled: true

logs:
    - { type: stdout, format: pretty, level: http }
```

## Authentication Patterns

```bash
# Token authentication (recommended)
npm token create --read-only   # Create read-only token
npm token create --cidr=10.0.0.0/8  # Restrict to IP range

# Use token in CI/CD
export NPM_TOKEN=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
echo "//registry.npmjs.org/:_authToken=${NPM_TOKEN}" > ~/.npmrc

# GitHub Packages authentication
npm set //npm.pkg.github.com/:_authToken=${GITHUB_TOKEN}

# Azure Artifacts
vsts-npm-auth -config .npmrc
```

## Best Practices Checklist

- [ ] Use `.npmrc` for project-specific configuration
- [ ] Use tokens (not passwords) for authentication
- [ ] Store tokens in environment variables, not in files
- [ ] Use scoped registries for private packages
- [ ] Set `engine-strict=true` for production
- [ ] Use `save-exact=true` for reproducible builds
- [ ] Configure proxy settings for corporate networks

## Cross-References

- See [Package.json Mastery](../02-package-json/01-fields-reference.md) for package.json
- See [Semantic Versioning](../03-semantic-versioning/01-version-ranges.md) for versioning
- See [Private Registry](../05-private-registry/01-setup-guide.md) for private registries

## Next Steps

Continue to [Package.json Mastery](../02-package-json/01-fields-reference.md) for package.json details.
