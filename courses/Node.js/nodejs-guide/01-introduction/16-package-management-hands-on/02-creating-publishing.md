# Creating and Publishing npm Packages

## What You'll Learn

- Creating a reusable npm package
- Publishing to npm registry
- Versioning with semantic versioning
- Package configuration best practices

## Creating a Package

### Project Setup

```bash
mkdir my-utils-package
cd my-utils-package
npm init -y
```

### package.json Configuration

```json
{
  "name": "@yourname/string-utils",
  "version": "1.0.0",
  "description": "A collection of string utility functions",
  "type": "module",
  "main": "dist/index.js",
  "module": "dist/index.mjs",
  "types": "dist/index.d.ts",
  "exports": {
    ".": {
      "import": "./dist/index.mjs",
      "require": "./dist/index.js",
      "types": "./dist/index.d.ts"
    }
  },
  "files": ["dist", "README.md", "LICENSE"],
  "scripts": {
    "build": "node build.js",
    "test": "node --test",
    "prepublishOnly": "npm run build && npm test"
  },
  "keywords": ["string", "utils", "utilities"],
  "author": "Your Name <your@email.com>",
  "license": "MIT",
  "repository": {
    "type": "git",
    "url": "https://github.com/yourname/string-utils"
  },
  "engines": {
    "node": ">=20.0.0"
  }
}
```

### Source Code

```javascript
// src/index.js — Package entry point

export { capitalize } from './capitalize.js';
export { truncate } from './truncate.js';
export { slugify } from './slugify.js';
export { escapeHtml } from './escape.js';
```

```javascript
// src/capitalize.js
export function capitalize(str) {
    if (typeof str !== 'string') throw new TypeError('Expected a string');
    if (str.length === 0) return str;
    return str.charAt(0).toUpperCase() + str.slice(1);
}
```

```javascript
// src/truncate.js
export function truncate(str, maxLength = 50, suffix = '...') {
    if (typeof str !== 'string') throw new TypeError('Expected a string');
    if (str.length <= maxLength) return str;
    return str.slice(0, maxLength - suffix.length) + suffix;
}
```

```javascript
// src/slugify.js
export function slugify(str) {
    if (typeof str !== 'string') throw new TypeError('Expected a string');
    return str
        .toLowerCase()
        .trim()
        .replace(/[^\w\s-]/g, '')
        .replace(/[\s_]+/g, '-')
        .replace(/^-+|-+$/g, '');
}
```

```javascript
// src/escape.js
export function escapeHtml(str) {
    if (typeof str !== 'string') throw new TypeError('Expected a string');
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#39;',
    };
    return str.replace(/[&<>"']/g, char => map[char]);
}
```

### Tests

```javascript
// tests/capitalize.test.js
import { describe, it } from 'node:test';
import assert from 'node:assert';
import { capitalize } from '../src/capitalize.js';

describe('capitalize', () => {
    it('capitalizes first letter', () => {
        assert.strictEqual(capitalize('hello'), 'Hello');
    });
    
    it('handles empty string', () => {
        assert.strictEqual(capitalize(''), '');
    });
    
    it('throws on non-string', () => {
        assert.throws(() => capitalize(123), TypeError);
    });
});
```

## Building Dual CJS/ESM Package

```javascript
// build.js — Build script for dual format

import { build } from 'esbuild';

await build({
    entryPoints: ['src/index.js'],
    bundle: true,
    platform: 'node',
    target: 'node20',
    format: 'esm',
    outfile: 'dist/index.mjs',
    external: [], // list external deps here
});

await build({
    entryPoints: ['src/index.js'],
    bundle: true,
    platform: 'node',
    target: 'node20',
    format: 'cjs',
    outfile: 'dist/index.js',
    external: [],
});

console.log('Build complete');
```

## Publishing

### First-Time Setup

```bash
# Create npm account
npm adduser
# Or login if you have an account
npm login

# Verify login
npm whoami
```

### Publishing Process

```bash
# Dry run — see what would be published
npm pack --dry-run

# Create tarball (test locally)
npm pack
# Creates: yourname-string-utils-1.0.0.tgz

# Publish to npm
npm publish

# Publish scoped package (public)
npm publish --access public

# Publish beta version
npm publish --tag beta
```

### Version Management

```bash
# Bump patch (bug fixes): 1.0.0 → 1.0.1
npm version patch

# Bump minor (new features): 1.0.1 → 1.1.0
npm version minor

# Bump major (breaking changes): 1.1.0 → 2.0.0
npm version major

# Prerelease: 1.0.0 → 1.0.1-0
npm version prerelease

# Custom preid
npm version prerelease --preid=beta
# 1.0.0 → 1.0.1-beta.0
```

### Updating Published Package

```bash
# Make changes, update version, publish
npm version patch
npm publish

# Deprecate old version
npm deprecate @yourname/string-utils@"< 1.2.0" "Use 1.2.0 or later"

# Unpublish (within 72 hours)
npm unpublish @yourname/string-utils@1.0.0
```

## Local Development with npm link

```bash
# In your package directory
cd my-utils-package
npm link

# In your project directory
cd my-project
npm link @yourname/string-utils

# Now your project uses the local version
# Changes to package are reflected immediately

# Unlink when done
npm unlink @yourname/string-utils
```

## Best Practices Checklist

- [ ] Use `files` field to control published content
- [ ] Include README.md with usage examples
- [ ] Include LICENSE file
- [ ] Use `prepublishOnly` script for build + test
- [ ] Support both ESM and CJS via `exports` field
- [ ] Use semantic versioning strictly
- [ ] Run `npm pack --dry-run` before publishing
- [ ] Use scoped packages for organization

## Cross-References

- See [npm Hands-On](./01-npm-hands-on.md) for npm commands
- See [Dependency Management](./03-dependency-management.md) for dependency strategies
- See [Package.json Basics](../12-setup-hello-world/03-package-json-basics.md) for configuration

## Next Steps

Continue to [Dependency Management](./03-dependency-management.md) for advanced dependency strategies.
