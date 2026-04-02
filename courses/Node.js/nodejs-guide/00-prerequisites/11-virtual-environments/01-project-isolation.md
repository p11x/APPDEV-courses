# Project Isolation Techniques

## What You'll Learn

- Creating isolated Node.js environments
- Managing dependencies per project
- Local vs global package installation
- Environment variable setup

## Project Isolation Overview

Project isolation ensures each Node.js project has its own dependencies and configuration, preventing conflicts between projects.

## Local vs Global Installation

### Global Installation

```bash
# Install globally (available system-wide)
npm install -g nodemon
yarn global add nodemon
pnpm add -g nodemon

# List global packages
npm list -g
yarn global list
pnpm list -g

# Global packages location
npm root -g
yarn global dir
pnpm root -g
```

### Local Installation

```bash
# Install locally (project-specific)
npm install express
yarn add express
pnpm add express

# Install as dev dependency
npm install --save-dev jest
yarn add --dev jest
pnpm add -D jest

# Local packages location
./node_modules/
```

### When to Use Each

```bash
# Global: CLI tools used across projects
npm install -g typescript
npm install -g nodemon
npm install -g eslint

# Local: Project dependencies
npm install express
npm install mongoose
npm install dotenv
```

## Node Modules Management

### Understanding node_modules

```bash
# node_modules structure
project/
├── node_modules/
│   ├── express/
│   ├── lodash/
│   └── ...
├── package.json
└── package-lock.json
```

### Cleaning node_modules

```bash
# Remove node_modules
rm -rf node_modules

# Reinstall dependencies
npm install
yarn install
pnpm install

# Clean install (removes and reinstalls)
npm ci
yarn install --frozen-lockfile
pnpm install --frozen-lockfile
```

### Ignoring node_modules

```gitignore
# .gitignore
node_modules/
```

```bash
# Never commit node_modules
git rm -r --cached node_modules
echo "node_modules/" >> .gitignore
git add .gitignore
git commit -m "Remove node_modules from git"
```

## Environment Variables

### Creating .env Files

```bash
# .env
NODE_ENV=development
PORT=3000
DATABASE_URL=postgresql://localhost:5432/mydb
API_KEY=your-api-key-here
SECRET_KEY=your-secret-key

# .env.local (local overrides, not committed)
DATABASE_URL=postgresql://localhost:5432/mydb_local

# .env.production
NODE_ENV=production
DATABASE_URL=postgresql://prod-server:5432/mydb
```

### Loading Environment Variables

```javascript
// Using dotenv
require('dotenv').config();

// Or with ES modules
import 'dotenv/config';

// Access variables
console.log(process.env.PORT);        // 3000
console.log(process.env.DATABASE_URL); // postgresql://...
console.log(process.env.NODE_ENV);     // development
```

### Environment-Specific Files

```bash
# Load order (first found wins)
.env.local
.env.development  # When NODE_ENV=development
.env.production   # When NODE_ENV=production
.env
```

```javascript
// Load environment-specific file
require('dotenv').config({
    path: `.env.${process.env.NODE_ENV}`
});
```

## Docker for Isolation

### Basic Dockerfile

```dockerfile
# Dockerfile
FROM node:20-alpine

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci --only=production

# Copy application
COPY . .

# Expose port
EXPOSE 3000

# Start application
CMD ["npm", "start"]
```

### Docker Compose

```yaml
# docker-compose.yml
version: '3.8'

services:
  app:
    build: .
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - DATABASE_URL=postgresql://db:5432/mydb
    depends_on:
      - db
    volumes:
      - .:/app
      - /app/node_modules

  db:
    image: postgres:15
    environment:
      - POSTGRES_DB=mydb
      - POSTGRES_USER=user
      - POSTGRES_PASSWORD=password
    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  postgres_data:
```

### Running with Docker

```bash
# Build and run
docker-compose up --build

# Run in background
docker-compose up -d

# Stop
docker-compose down

# View logs
docker-compose logs -f app
```

## npm Scripts for Isolation

### Development Scripts

```json
{
    "scripts": {
        "dev": "NODE_ENV=development nodemon src/index.js",
        "start": "NODE_ENV=production node src/index.js",
        "test": "NODE_ENV=test jest",
        "test:watch": "NODE_ENV=test jest --watch"
    }
}
```

### Running Scripts

```bash
# Run with environment
npm run dev
npm run start
npm run test

# Override environment
NODE_ENV=staging npm run start
```

## Isolation with npx

### Running Without Installation

```bash
# Run package without installing
npx create-react-app my-app
npx eslint src/
npx prettier --write .

# Run specific version
npx eslint@8.0.0 src/

# Run from GitHub
npx github:username/repo
```

### Creating Custom Scripts

```javascript
// scripts/cleanup.js
#!/usr/bin/env node

const fs = require('fs');
const path = require('path');

const directories = ['dist', 'coverage', '.cache'];

directories.forEach(dir => {
    const dirPath = path.join(process.cwd(), dir);
    if (fs.existsSync(dirPath)) {
        fs.rmSync(dirPath, { recursive: true });
        console.log(`Removed: ${dir}`);
    }
});
```

```json
{
    "scripts": {
        "cleanup": "node scripts/cleanup.js"
    }
}
```

## Version Isolation

### Using .nvmrc

```bash
# .nvmrc
20.10.0

# Use version
nvm use

# Install version
nvm install
```

### Using engines in package.json

```json
{
    "engines": {
        "node": ">=18.0.0",
        "npm": ">=9.0.0"
    }
}
```

```bash
# Enforce engine requirements
npm config set engine-strict true
```

## Dependency Isolation

### Using overrides/resolutions

```json
// package.json (npm)
{
    "overrides": {
        "lodash": "4.17.21"
    }
}

// package.json (yarn)
{
    "resolutions": {
        "lodash": "4.17.21"
    }
}
```

### Using peerDependencies

```json
{
    "peerDependencies": {
        "react": ">=16.8.0"
    }
}
```

## Troubleshooting Common Issues

### Dependency Conflicts

```bash
# Problem: Version conflicts between projects
# Solution: Use local installation

# Wrong
npm install -g express

# Right
npm install express
```

### Environment Variables Not Loading

```bash
# Problem: process.env variables undefined
# Solution: Check dotenv configuration

# Install dotenv
npm install dotenv

# Load at application start
require('dotenv').config();

# Check file path
console.log(process.cwd());
```

### Permission Errors

```bash
# Problem: EACCES errors
# Solution: Use local installation or fix permissions

# Use npx instead of global
npx eslint src/

# Or fix npm permissions
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
export PATH=~/.npm-global/bin:$PATH
```

## Best Practices Checklist

- [ ] Install dependencies locally, not globally
- [ ] Use .env files for environment variables
- [ ] Never commit .env files to git
- [ ] Use Docker for complete isolation
- [ ] Use npx for one-off commands
- [ ] Specify Node.js version in .nvmrc
- [ ] Use engines field in package.json
- [ ] Clean node_modules regularly
- [ ] Use lock files for reproducible builds
- [ ] Document environment setup in README

## Performance Optimization Tips

- Use Docker multi-stage builds
- Use .dockerignore to exclude unnecessary files
- Use npm ci for faster installs
- Cache Docker layers
- Use pnpm for disk-efficient installs
- Use npx for one-off tools
- Clean up unused dependencies

## Cross-References

- See [Dependencies Management](./02-dependencies-management.md) for dependency strategies
- See [Package Managers](../10-package-managers/) for npm/yarn/pnpm
- See [Node.js Installation](../05-nodejs-installation/) for version management
- See [Development Tools](../12-dev-tools-integration/) for IDE integration

## Next Steps

Now that project isolation is understood, let's manage dependencies. Continue to [Dependencies Management](./02-dependencies-management.md).