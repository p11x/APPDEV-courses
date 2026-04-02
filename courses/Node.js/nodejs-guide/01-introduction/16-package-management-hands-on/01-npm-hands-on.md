# npm Package Installation and Usage

## What You'll Learn

- Installing and managing packages with npm
- Understanding dependency types
- Using packages in your code
- npm CLI commands reference

## Installing Packages

### Local Installation

```bash
# Install as production dependency
npm install express

# Install as dev dependency
npm install --save-dev eslint

# Install specific version
npm install express@4.18.2

# Install version range
npm install express@"^4.18.0"  # 4.18.x or higher minor
npm install express@"~4.18.0"  # 4.18.x only

# Install multiple packages
npm install express cors helmet dotenv
npm install -D typescript @types/node vitest
```

### Global Installation

```bash
# Install globally (CLI tools)
npm install -g nodemon
npm install -g pm2
npm install -g typescript

# List global packages
npm list -g

# Uninstall global
npm uninstall -g nodemon
```

### Package Lock File

```bash
# package-lock.json is auto-generated
# It locks exact versions for reproducible builds

# Install from lock file (CI/CD)
npm ci  # Faster, stricter than npm install
        # Deletes node_modules first
        # Fails if lock/package.json mismatch
```

## Understanding Dependencies

### package.json Dependencies

```json
{
  "dependencies": {
    "express": "^4.21.0",       // Production: shipped with your app
    "cors": "^2.8.5"
  },
  "devDependencies": {
    "eslint": "^9.0.0",         // Development: testing, linting, building
    "vitest": "^3.0.0"
  },
  "peerDependencies": {
    "react": ">=18.0.0"         // Expected to be provided by consumer
  },
  "optionalDependencies": {
    "fsevents": "^2.3.0"        // Nice to have, won't fail if missing
  },
  "bundledDependencies": [
    "special-package"            // Included in published tarball
  ]
}
```

### Version Ranges Explained

```bash
# Exact version
"express": "4.21.0"            # Exactly 4.21.0

# Caret (^): Allow minor + patch updates
"express": "^4.21.0"           # >=4.21.0 <5.0.0

# Tilde (~): Allow patch updates only
"express": "~4.21.0"           # >=4.21.0 <4.22.0

# Wildcard
"express": "*"                 # Any version (dangerous)

# Range
"express": ">=4.18.0 <5.0.0"  # Explicit range

# Latest
"express": "latest"            # Latest published version
```

## Using Packages in Code

### ES Modules (Recommended)

```javascript
// package.json has "type": "module"

// Default export
import express from 'express';

// Named exports
import { Router, json } from 'express';

// Namespace import
import * as path from 'node:path';

// Dynamic import (conditional loading)
const module = await import('./optional-feature.js');
```

### CommonJS (Legacy)

```javascript
// Default/entire module
const express = require('express');

// Destructured
const { Router, json } = require('express');

// JSON files
const config = require('./config.json');
```

## Essential npm Commands

```bash
# Package management
npm install              # Install all dependencies
npm ci                   # Clean install from lock file
npm uninstall <pkg>      # Remove package
npm update               # Update packages
npm outdated             # Check for updates

# Information
npm list                 # List installed packages
npm list --depth=0       # Top-level only
npm info <pkg>           # Package details
npm search <term>        # Search registry

# Scripts
npm start                # Run "start" script
npm test                 # Run "test" script
npm run <name>           # Run custom script

# Audit & Security
npm audit                # Check for vulnerabilities
npm audit fix            # Auto-fix vulnerabilities
npm audit fix --force    # Force fix (may break things)

# Publishing
npm login                # Login to npm registry
npm publish              # Publish package
npm version patch        # Bump version
npm deprecate <pkg> <msg> # Deprecate a version
```

## Troubleshooting

### Common Issues

```bash
# Clear npm cache
npm cache clean --force

# Fix permissions (never use sudo)
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'

# node_modules corrupted
rm -rf node_modules package-lock.json
npm install

# Peer dependency conflicts
npm install --legacy-peer-deps
```

## Best Practices Checklist

- [ ] Use `npm ci` in CI/CD pipelines
- [ ] Use `^` for dependency versions (allows security patches)
- [ ] Keep devDependencies separate from dependencies
- [ ] Run `npm audit` regularly
- [ ] Commit package-lock.json to version control
- [ ] Use `.npmignore` to exclude files from published packages

## Cross-References

- See [Creating and Publishing Packages](./02-creating-publishing.md) for package creation
- See [Dependency Management](./03-dependency-management.md) for advanced strategies
- See [Package.json Basics](../12-setup-hello-world/03-package-json-basics.md) for configuration

## Next Steps

Continue to [Creating and Publishing Packages](./02-creating-publishing.md) to learn package creation.
