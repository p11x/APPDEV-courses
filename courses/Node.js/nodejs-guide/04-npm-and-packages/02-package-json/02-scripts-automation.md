# NPM Scripts and Automation

## What You'll Learn

- npm lifecycle scripts
- Script chaining and automation
- Pre/post script hooks
- Cross-platform script execution

## Lifecycle Scripts

```json
{
    "scripts": {
        "preinstall": "echo 'Before install'",
        "install": "node scripts/install.js",
        "postinstall": "husky install",
        
        "prepublishOnly": "npm run build && npm test",
        "prepack": "npm run build",
        "postpack": "echo 'Package created'",
        
        "preversion": "npm test",
        "version": "npm run build && git add -A dist",
        "postversion": "git push && git push --tags",
        
        "prestart": "npm run build",
        "start": "node dist/server.js",
        "poststart": "echo 'Server started'",
        
        "pretest": "npm run lint",
        "test": "node --test",
        "posttest": "npm run coverage",
        
        "prestop": "echo 'Stopping...'",
        "stop": "kill $(cat .pidfile)",
        "poststop": "echo 'Stopped'"
    }
}
```

## Script Patterns

```json
{
    "scripts": {
        "dev": "node --watch src/server.js",
        "build": "node build.js",
        "start": "NODE_ENV=production node dist/server.js",
        
        "test": "node --test",
        "test:watch": "node --test --watch",
        "test:coverage": "node --test --experimental-test-coverage",
        
        "lint": "eslint src/",
        "lint:fix": "eslint src/ --fix",
        "format": "prettier --write 'src/**/*.js'",
        "format:check": "prettier --check 'src/**/*.js'",
        
        "db:migrate": "node scripts/migrate.js",
        "db:seed": "node scripts/seed.js",
        "db:reset": "npm run db:migrate && npm run db:seed",
        
        "clean": "rm -rf dist coverage",
        "prebuild": "npm run clean",
        "build": "node build.js",
        
        "release": "npm version patch && npm publish",
        "release:minor": "npm version minor && npm publish",
        "release:major": "npm version major && npm publish"
    }
}
```

## Cross-Platform Scripts

```bash
# Using cross-env for environment variables
npm install -D cross-env

# package.json
{
    "scripts": {
        "start": "cross-env NODE_ENV=production node server.js",
        "dev": "cross-env NODE_ENV=development node --watch server.js"
    }
}
```

## Best Practices Checklist

- [ ] Use pre/post hooks for automation
- [ ] Use cross-env for cross-platform env vars
- [ ] Use node --watch instead of nodemon (Node.js 18+)
- [ ] Keep scripts simple and composable
- [ ] Document custom scripts in README

## Cross-References

- See [Fields Reference](./01-fields-reference.md) for package.json fields
- See [Semantic Versioning](../03-semantic-versioning/01-version-ranges.md) for versioning
- See [Ecosystem Integration](../15-ecosystem-integration/01-github-actions.md) for CI/CD

## Next Steps

Continue to [Semantic Versioning](../03-semantic-versioning/01-version-ranges.md) for versioning.
