# Nx Setup

## Overview
Nx is a powerful monorepo tool with advanced features like distributed caching, affected graph, and code generation.

## Prerequisites
- Node.js
- CLI experience

## Core Concepts

### Installation

```bash
# [File: Terminal]
# Create new Nx workspace
npx create-nx-workspace@latest myorg

# Add React to existing workspace
npm install -D @nx/react
nx g @nx/react:app myapp
```

### Project Structure

```
myorg/
├── apps/
│   ├── api/          # Express API
│   └── web/          # React app
├── libs/
│   ├── shared/
│   │   ├── ui/       # UI library
│   │   └── utils/    # Utilities
│   └── features/
│       └── products/ # Feature module
├── nx.json
├── workspace.json
└── package.json
```

### Generating Code

```bash
# [File: Terminal]
# Generate React component
nx g @nx/react:component libs/shared/ui/src/lib/Button

# Generate library
nx g @nx/react:library shared-ui --buildable

# Generate feature
nx g @nx/react:library features/products --buildable
```

### Nx Configuration

```json
// [File: nx.json]
{
  "$schema": "./node_modules/nx/schemas/nx-schema.json",
  "targetDefaults": {
    "build": {
      "dependsOn": ["^build"]
    },
    "test": {
      "dependsOn": ["^build"]
    }
  },
  "namedInputs": {
    "default": ["{projectRoot}/**/*"],
    "production": ["!{projectRoot}/**/*.spec.ts"]
  }
}
```

## Key Takeaways
- Nx provides powerful code generation
- Affected graph optimizes CI/CD
- Distributed caching speeds up builds

## What's Next
Continue to [PWA Fundamentals](03-pwa/01-pwa-fundamentals.md) for Progressive Web App development.