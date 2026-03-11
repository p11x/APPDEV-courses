# Angular CLI and Workspace Creation

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Install and verify Angular CLI
- [ ] Create new Angular projects with various configurations
- [ ] Understand the Angular workspace structure
- [ ] Generate components, services, and other Angular artifacts
- [ ] Build and serve Angular applications

## Conceptual Explanation

**Visual Analogy**: Think of Angular CLI as a **factory assembly line** for your application. Instead of manually creating each file, configuring build tools, and setting up testing frameworks, the CLI does all the heavy lifting - like a robotic assembly system that builds cars from blueprints!

### What is Angular CLI?

Angular CLI (Command Line Interface) is a tool that helps you:
- Create new Angular projects
- Generate components, services, directives, pipes
- Build applications for production
- Run tests
- Add libraries to your project

## Real-World Application Context

### Why Use Angular CLI?

1. **Productivity**: Generate code in seconds instead of hours
2. **Consistency**: All Angular projects follow the same structure
3. **Best Practices**: Generated code follows Angular style guide
4. **Build Optimization**: Production builds are optimized automatically
5. **Community**: Huge ecosystem of community-built schematics

### Industry Use Cases

- **Enterprise Projects**: Standardized project setup across teams
- **Prototyping**: Quick scaffolding for new features
- **Monorepos**: Nx CLI extends Angular CLI for large-scale apps

## Step-by-Step Walkthrough

### Installing Angular CLI

#### Step 1: Prerequisites Check

First, verify Node.js and npm are installed:

```bash
node --version    # Should be 18.x or higher
npm --version     # Should be 9.x or higher
```

#### Step 2: Install Angular CLI Globally

```bash
npm install -g @angular/cli
```

Expected output:
```
added 5 packages in 10s
```

#### Step 3: Verify Installation

```bash
ng version
```

Expected output:
```
     _                      _                 ____ _     ___
    / \   _ __   __ _ _   _| | __ _ _ __     / ___| |   |_ _|
   / △ \ | '_ \ / _` | | | | |/ _` | '__|   | |   | |    | |
  / ___ \| | | | (_| | |_| | | (_| | |      | |___| |___ | |
 /_/   \_\_| |_|\__,_|\__,_|_|\__,_|_|       \____|_____|___|


Angular CLI: 17.x.x
Node: 18.x.x
Package Manager: npm 9.x.x
OS: win32 x64
```

### Creating a New Project

#### Step 1: Create New Angular Project

```bash
ng new my-first-app
```

This command:
1. Creates a new directory
2. Initializes a Git repository
3. Installs npm dependencies
4. Sets up Angular framework
5. Configures testing (Karma + Jasmine)

**Project Creation Output:**
```
√ Packages installed successfully.
√ Repository initialization completed.
√ Initialized git commit.
CREATE my-first-app/.editorconfig (x bytes)
CREATE my-first-app/.gitignore (x bytes)
CREATE my-first-app/README.md (x bytes)
CREATE my-first-app/angular.json (x bytes)
CREATE my-first-app/package.json (x bytes)
CREATE my-first-app/tsconfig.app.json (x bytes)
CREATE my-first-app/tsconfig.json (x bytes)
CREATE my-first-app/tsconfig.spec.json (x bytes)
CREATE my-first-app/src/main.ts (x bytes)
CREATE my-first-app/src/index.html (x bytes)
CREATE my-first-app/src/styles.scss (x bytes)
CREATE my-first-app/src/app/app.component.ts (x bytes)
CREATE my-first-app/src/app/app.component.html (x bytes)
CREATE my-first-app/src/app/app.component.spec.ts (x bytes)
CREATE my-first-app/src/app/app.component.scss (x bytes)
```

#### Step 2: Project Options

Here are common CLI options:

```bash
# With routing and SCSS
ng new my-app --routing --style=scss

# With routing and inline styles/templates
ng new my-app --routing --inline-template --inline-style

# Skip Git and install
ng new my-app --skip-git --skip-install

# Strict mode enabled
ng new my-app --strict

# Minimal files
ng new my-app --minimal

# Standalone components (default in Angular 17+)
ng new my-app --standalone
```

### Angular Workspace Structure

After creation, your project looks like this:

```
my-first-app/
├── .editorconfig          # Editor configuration
├── .gitignore             # Git ignore rules
├── angular.json           # Angular CLI configuration
├── package.json           # NPM dependencies
├── tsconfig.json          # TypeScript base config
├── tsconfig.app.json      # TypeScript for app
├── tsconfig.spec.json     # TypeScript for tests
├── README.md              # Project README
├── node_modules/          # NPM packages (after install)
└── src/
    ├── app/
    │   ├── app.component.ts
    │   ├── app.component.html
    │   ├── app.component.scss
    │   ├── app.component.spec.ts
    │   ├── app.config.ts          # App configuration (Angular 17+)
    │   └── app.routes.ts          # App routing (Angular 17+)
    ├── assets/                    # Static files (images, etc.)
    │   └── .gitkeep
    ├── index.html                 # Entry HTML
    ├── main.ts                    # App bootstrap
    ├── styles.scss                # Global styles
    └── environments/              # Environment configs
        ├── environment.ts
        └── environment.prod.ts
```

### Key Files Explained

| File | Purpose |
|------|---------|
| `angular.json` | Angular CLI configuration - defines build, serve, test options |
| `package.json` | Lists all npm dependencies and scripts |
| `tsconfig.json` | TypeScript compiler options |
| `src/main.ts` | Application bootstrap file |
| `src/index.html` | Main HTML file |
| `src/app/app.component.ts` | Root component |

## Generating Angular Artifacts

### Generate Component

```bash
ng generate component components/user-list
# or short form
ng g c components/user-list
```

Output:
```
CREATE src/app/components/user-list/user-list.component.html (x bytes)
CREATE src/app/components/user-list/user-list.component.ts (x bytes)
CREATE src/app/components/user-list/user-list.component.scss (x bytes)
CREATE src/app/components/user-list/user-list.component.spec.ts (x bytes)
```

### Generate Service

```bash
ng generate service services/user
# or
ng g s services/user
```

Output:
```
CREATE src/app/services/user.service.ts (x bytes)
CREATE src/app/services/user.service.spec.ts (x bytes)
```

### Generate Other Artifacts

```bash
# Directives
ng g d directives/highlight

# Pipes
ng g p pipes/date-format

# Guards
ng g g guards/auth

# Interceptors
ng g interceptor logging

# Classes
ng g class models/user

# Interfaces
ng g interface models/user

# Enums
ng g enum enums/user-role
```

### Generate with Options

```bash
# Inline template and style
ng g c components/user --inline-template --inline-style

# Skip tests
ng g c components/user --skip-tests

# Flat structure (no folder)
ng g c components/user --flat
```

## Building and Running Applications

### Development Server

```bash
ng serve
# or with options
ng serve --port 4200 --open
```

Expected output:
```
▲ [watch] build started...
▲ [watch] build completed in x ms
✗ [watch] build failed

● Local:   http://localhost:4200/
● Network: http://192.168.1.x:4200/

▲ [watch] build started...
```

### Build for Production

```bash
ng build
```

Expected output:
```
✔ Browser application bundle generation complete.
✔ Copying assets complete.
✔ Generating index HTML...
✔ Building for production...
Output directory: "dist/my-first-app"
```

### Build Options

```bash
# Production build with optimizations
ng build --configuration=production

# Development build (faster)
ng build --configuration=development

# Stats for bundle analysis
ng build --stats-json
```

### Running Tests

```bash
# Unit tests
ng test

# With coverage
ng test --code-coverage

# Single run
ng test --watch=false
```

## Code Example: Project Configuration

### angular.json Overview

```json
{
  "$schema": "./node_modules/@angular/cli/lib/config/schema.json",
  "version": 1,
  "newProjectRoot": "projects",
  "projects": {
    "my-first-app": {
      "projectType": "application",
      "root": "",
      "sourceRoot": "src",
      "prefix": "app",
      "architect": {
        "build": {
          "builder": "@angular-devkit/build-angular:application",
          "options": {
            "outputPath": "dist/my-first-app",
            "index": "src/index.html",
            "browser": "src/main.ts",
            "polyfills": ["zone.js"],
            "tsConfig": "tsconfig.app.json",
            "inlineStyleLanguage": "scss",
            "assets": ["src/favicon.ico", "src/assets"],
            "styles": ["src/styles.scss"],
            "scripts": []
          },
          "configurations": {
            "production": {
              "budgets": [
                {
                  "type": "initial",
                  "maximumWarning": "500kb",
                  "maximumError": "1mb"
                }
              ],
              "outputHashing": "all"
            }
          }
        },
        "serve": {
          "builder": "@angular-devkit/build-angular:dev-server",
          "configurations": {
            "production": {
              "buildTarget": "my-first-app:build:production"
            },
            "development": {
              "buildTarget": "my-first-app:build:development"
            }
          },
          "defaultConfiguration": "development"
        }
      }
    }
  }
}
```

## Best Practices

### 1. Use Standalone Components (Angular 17+)

```bash
# New projects use standalone by default
ng new my-app --standalone  # Explicit
```

### 2. Follow Naming Conventions

```bash
# Components: feature-name.component
ng g c user-profile

# Services: feature-name.service  
ng g c user-service

# Use prefixes to avoid collisions
ng g c my-profile  # 'my' is the prefix
```

### 3. Organize by Feature

```
src/
└── app/
    ├── features/
    │   ├── dashboard/
    │   │   ├── dashboard.component.ts
    │   │   └── dashboard.routes.ts
    │   └── users/
    │       ├── users.component.ts
    │       └── users.routes.ts
    └── shared/
        ├── components/
        └── services/
```

### 4. Use Environment Files

```typescript
// environment.ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:3000/api'
};

// environment.prod.ts
export const environment = {
  production: true,
  apiUrl: 'https://api.example.com'
};
```

## Common Pitfalls and Debugging

### Pitfall 1: Port Already in Use

```bash
# Error: Port 4200 is already in use
# Solution: Use a different port
ng serve --port 4201
```

### Pitfall 2: Module Not Found

```bash
# Error: Cannot find module '@angular/core'
# Solution: Reinstall dependencies
rm -rf node_modules
npm install
```

### Pitfall 3: Build Memory Issues

```bash
# Solution: Increase Node.js memory limit
NODE_OPTIONS="--max-old-space-size=4096" ng build
```

### Pitfall 4: TypeScript Version Mismatch

```bash
# Check TypeScript version
npm list typescript

# Install specific version
npm install typescript@~5.2.0 --save-dev
```

## Hands-On Exercise

### Exercise 1.4: CLI Mastery

**Objective**: Create and configure a complete Angular workspace

**Requirements**:
1. Create a new Angular project with routing and SCSS
2. Generate 3 components: header, footer, home
3. Generate 2 services: data, auth
4. Configure a production build
5. Run the application and verify it works

**Deliverable**: A working Angular project with generated artifacts

**Assessment Criteria**:
- [ ] Project created with routing and SCSS
- [ ] All 5 artifacts generated correctly
- [ ] Application runs without errors
- [ ] Production build succeeds
- [ ] Project structure follows best practices

## Extension Challenge

**Challenge**: Create a custom schematic

```bash
# Install schematics CLI
npm install @angular-devkit/schematics-cli -g

# Create a new schematic
schematics blank my-schematic

# Add generation logic
# This creates custom ng generate commands
```

## Summary

- Angular CLI is essential for Angular development
- Use `ng new` to create projects with proper configuration
- Generate artifacts with `ng g` commands
- Understand workspace structure for debugging
- Use environment files for configuration management

## Next Steps

In the next lecture, we'll explore Component Architecture in depth.
