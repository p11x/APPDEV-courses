# Creating Reusable npm Packages

## What You'll Learn

- Package structure and organization
- Dual CJS/ESM package setup
- TypeScript declaration generation
- Package testing strategies

## Package Structure

```
my-package/
├── src/
│   ├── index.ts
│   ├── utils.ts
│   └── types.ts
├── dist/
│   ├── index.js      (CJS)
│   ├── index.mjs     (ESM)
│   └── index.d.ts    (Types)
├── test/
│   └── index.test.ts
├── package.json
├── tsconfig.json
├── README.md
├── LICENSE
└── .gitignore
```

## Dual CJS/ESM package.json

```json
{
    "name": "@myorg/my-package",
    "version": "1.0.0",
    "description": "A useful utility package",
    "type": "module",
    "main": "./dist/index.cjs",
    "module": "./dist/index.mjs",
    "types": "./dist/index.d.ts",
    "exports": {
        ".": {
            "types": "./dist/index.d.ts",
            "import": "./dist/index.mjs",
            "require": "./dist/index.cjs"
        }
    },
    "files": ["dist", "README.md", "LICENSE"],
    "scripts": {
        "build": "node build.js",
        "test": "node --test",
        "prepublishOnly": "npm run build && npm test"
    },
    "engines": { "node": ">=20.0.0" },
    "license": "MIT"
}
```

## Build Script

```javascript
// build.js
import { build } from 'esbuild';

await build({
    entryPoints: ['src/index.ts'],
    bundle: true,
    platform: 'node',
    target: 'node20',
    format: 'esm',
    outfile: 'dist/index.mjs',
    external: [],
});

await build({
    entryPoints: ['src/index.ts'],
    bundle: true,
    platform: 'node',
    target: 'node20',
    format: 'cjs',
    outfile: 'dist/index.cjs',
    external: [],
});

console.log('Build complete');
```

## Best Practices Checklist

- [ ] Use TypeScript for type safety
- [ ] Support both CJS and ESM
- [ ] Include comprehensive README
- [ ] Add LICENSE file
- [ ] Use `files` field to control published content
- [ ] Test before publishing

## Cross-References

- See [Publishing](../06-package-publishing/01-publishing-workflow.md) for publishing
- See [Testing](../13-package-testing/01-unit-testing.md) for testing
- See [Package.json](../02-package-json/01-fields-reference.md) for configuration

## Next Steps

Continue to [Package Testing](../13-package-testing/01-unit-testing.md) for testing strategies.
