# Understanding package.json

## 📌 What You'll Learn
- What package.json is and why it matters
- The different fields and their purposes
- How to manage dependencies

## 🧠 Concept Explained (Plain English)

`package.json` is like your project's ID card. It tells the world important things about your project: its name, version, what packages it needs, and how to run it. Every Node.js and Express project has one.

Think of package.json as a manifest (hence the name) — it's a list that describes everything about your project. When you share your project with others, they can look at this file and immediately know how to set it up and what it does.

The file is written in JSON (JavaScript Object Notation), which is just a way of storing data in a format that's easy for computers to read and write.

## 💻 Sample package.json

Here's what a typical Express project's package.json looks like:

```json
{
    "name": "my-express-app",
    "version": "1.0.0",
    "description": "My first Express application",
    "type": "module",
    "main": "server.js",
    "scripts": {
        "start": "node server.js",
        "dev": "node --watch server.js",
        "test": "jest"
    },
    "keywords": ["express", "nodejs", "api"],
    "author": "Your Name",
    "license": "MIT",
    "dependencies": {
        "express": "^5.0.0"
    },
    "devDependencies": {
        "jest": "^29.0.0"
    }
}
```

## Understanding Each Field

| Field | Purpose | Example |
|-------|---------|---------|
| `name` | Your project's name | "my-express-app" |
| `version` | Current version number | "1.0.0" |
| `description` | Brief description | "A REST API for..." |
| `type` | Module system used | "module" for ESM |
| `main` | Entry point file | "server.js" |
| `scripts` | Shortcut commands | start, test, dev |
| `keywords` | Searchable tags | ["express", "api"] |
| `author` | Who created it | "Your Name" |
| `license` | Legal info | "MIT" |
| `dependencies` | Required packages | express, mongoose |
| `devDependencies` | Dev-only packages | jest, nodemon |

## Understanding Version Numbers

Version numbers use **semantic versioning** (semver):

```
^5.0.0
│ │ │
│ │ └── Patch version (bug fixes)
│ └──── Minor version (new features)
└────── Major version (breaking changes)
```

| Symbol | Meaning |
|--------|---------|
| `^5.0.0` | Any 5.x.x version (accepts minor/patch updates) |
| `~5.0.0` | Any 5.0.x version (only accepts patch updates) |
| `5.0.0` | Exactly version 5.0.0 |

## Managing Dependencies

### Adding Dependencies

```bash
# Add to dependencies (needed in production)
npm install express
npm install express --save  # same as above

# Add to devDependencies (only needed for development)
npm install jest --save-dev
```

### Installing All Dependencies

When you clone a project, install all dependencies at once:

```bash
# Reads package.json and installs everything listed
npm install
```

### Updating Dependencies

```bash
# Check for outdated packages
npm outdated

# Update to latest allowed versions
npm update
```

## Adding Scripts

Scripts let you define shortcuts for common commands:

```json
{
    "scripts": {
        "start": "node server.js",
        "dev": "nodemon server.js",
        "test": "jest --coverage",
        "lint": "eslint ."
    }
}
```

Then run them:
```bash
npm start      # Runs: node server.js
npm run dev    # Runs: nodemon server.js
npm test       # Runs: jest --coverage
```

## ⚠️ Common Mistakes

**1. Not using version constraints**
Always use `^` or `~` in version numbers. Using exact versions can cause issues when packages are updated.

**2. Mixing dependencies and devDependencies**
Only put production dependencies in `dependencies`. Testing and build tools go in `devDependencies`.

**3. Forgetting to run npm install**
When you clone a project, always run `npm install` first to download dependencies.

## ✅ Quick Recap

- package.json describes your project
- Dependencies are packages your code needs
- devDependencies are for development only
- Scripts define shortcut commands
- Use semantic versioning with ^ or ~

## 🔗 What's Next

Let's learn about package-lock.json, which ensures consistent installations.
