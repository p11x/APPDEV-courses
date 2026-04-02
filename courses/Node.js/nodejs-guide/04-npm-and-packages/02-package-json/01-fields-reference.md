# Package.json Fields Reference

## What You'll Learn

- All package.json fields explained
- Required vs optional fields
- Field validation and best practices
- Common package.json patterns

## Core Fields

```json
{
    "name": "my-package",
    "version": "1.0.0",
    "description": "A comprehensive utility package",
    "main": "dist/index.js",
    "module": "dist/index.mjs",
    "types": "dist/index.d.ts",
    "exports": {
        ".": {
            "import": "./dist/index.mjs",
            "require": "./dist/index.js",
            "types": "./dist/index.d.ts"
        },
        "./utils": {
            "import": "./dist/utils.mjs",
            "require": "./dist/utils.js"
        }
    },
    "files": ["dist", "README.md", "LICENSE"],
    "keywords": ["utility", "helper", "nodejs"],
    "author": "Your Name <email@example.com>",
    "license": "MIT",
    "repository": {
        "type": "git",
        "url": "https://github.com/username/my-package.git",
        "directory": "packages/my-package"
    },
    "bugs": {
        "url": "https://github.com/username/my-package/issues",
        "email": "bugs@example.com"
    },
    "homepage": "https://github.com/username/my-package#readme",
    "private": false,
    "publishConfig": {
        "registry": "https://registry.npmjs.org/",
        "access": "public"
    }
}
```

## Dependency Fields

```json
{
    "dependencies": {
        "express": "^4.21.0",
        "lodash": "^4.17.21"
    },
    "devDependencies": {
        "eslint": "^9.0.0",
        "vitest": "^3.0.0",
        "typescript": "^5.4.0"
    },
    "peerDependencies": {
        "react": ">=18.0.0"
    },
    "peerDependenciesMeta": {
        "react": {
            "optional": true
        }
    },
    "optionalDependencies": {
        "fsevents": "^2.3.0"
    },
    "bundledDependencies": ["special-internal-pkg"],
    "overrides": {
        "semver": "^7.5.0"
    },
    "resolutions": {
        "lodash": "4.17.21"
    }
}
```

## Scripts and Configuration

```json
{
    "scripts": {
        "start": "node src/server.js",
        "dev": "node --watch src/server.js",
        "test": "node --test",
        "test:coverage": "node --test --experimental-test-coverage",
        "lint": "eslint src/",
        "lint:fix": "eslint src/ --fix",
        "format": "prettier --write 'src/**/*.js'",
        "build": "node build.js",
        "prepublishOnly": "npm run build && npm test",
        "prepare": "husky install",
        "postinstall": "node scripts/postinstall.js"
    },
    "type": "module",
    "engines": {
        "node": ">=20.0.0",
        "npm": ">=10.0.0"
    },
    "os": ["linux", "darwin", "win32"],
    "cpu": ["x64", "arm64"],
    "browser": "./dist/browser.js",
    "bin": {
        "my-cli": "./bin/cli.js"
    },
    "man": ["./man/my-cli.1"],
    "directories": {
        "lib": "src",
        "bin": "bin",
        "man": "man",
        "doc": "docs",
        "test": "test"
    },
    "config": {
        "port": 3000
    },
    "funding": {
        "type": "github",
        "url": "https://github.com/sponsors/username"
    },
    "workspaces": ["packages/*"]
}
```

## Field Reference Table

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| name | string | Yes | Package name (lowercase, no spaces) |
| version | string | Yes | Semantic version |
| description | string | No | Package description |
| main | string | No | Entry point (CommonJS) |
| module | string | No | Entry point (ESM) |
| types | string | No | TypeScript declarations |
| exports | object | No | Conditional exports |
| files | array | No | Files to include in package |
| scripts | object | No | Lifecycle scripts |
| dependencies | object | No | Production dependencies |
| devDependencies | object | No | Development dependencies |
| peerDependencies | object | No | Peer dependencies |
| engines | object | No | Node.js/npm version requirements |
| type | string | No | "module" for ESM |
| private | boolean | No | Prevent accidental publish |
| license | string | No | SPDX license identifier |

## Best Practices Checklist

- [ ] Always include name, version, description
- [ ] Use `exports` field for dual CJS/ESM support
- [ ] Use `files` to control published content
- [ ] Set `engines` to specify Node.js version
- [ ] Use `type: "module"` for new projects
- [ ] Include repository, bugs, homepage URLs
- [ ] Set appropriate license

## Cross-References

- See [Scripts and Automation](./02-scripts-automation.md) for npm scripts
- See [Semantic Versioning](../03-semantic-versioning/01-version-ranges.md) for versioning
- See [Package Development](../09-package-development/01-creating-packages.md) for development

## Next Steps

Continue to [Scripts and Automation](./02-scripts-automation.md) for npm scripts.
