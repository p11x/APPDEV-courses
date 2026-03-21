# Semantic Release and Changelog

## 📌 What You'll Learn

- Conventional commits
- Semantic-release configuration
- Automatic versioning and changelog

## 🧠 Concept Explained (Plain English)

Semantic release automates your release process. It analyzes your git commits to determine the next version number, generates a changelog automatically, and publishes to npm. It follows semantic versioning (SemVer) - major for breaking changes, minor for new features, patch for bug fixes.

Conventional commits is a standard format for commit messages that makes it easy for tools to understand what changed. The format is: `type(scope): description`. For example, `feat(api): add user endpoint` or `fix(auth): resolve login bug`.

When you merge a feature to main, semantic-release reads the commits, determines version bump needed, creates a release, and publishes to npm.

## Conventional Commits

```
feat: add new feature
fix: bug fix
docs: documentation changes
style: formatting, no code change
refactor: code restructuring
perf: performance improvement
test: adding tests
build: build system changes
ci: CI configuration changes
chore: maintenance
```

Examples:
```
feat(auth): add password reset functionality
fix(api): handle null response from database
docs: update API documentation
refactor(users): extract validation to middleware
```

## Semantic Release Configuration

```js
// Install
// npm install -D semantic-release @semantic-release/changelog @semantic-release/npm @semantic-release/github

// .releaserc.json
{
  "branches": ["main"],
  "plugins": [
    "@semantic-release/commit-analyzer",
    "@semantic-release/release-notes-generator",
    "@semantic-release/changelog",
    "@semantic-release/npm",
    "@semantic-release/github"
  ]
}

// package.json
{
  "name": "my-express-api",
  "version": "1.0.0",
  "scripts": {
    "release": "semantic-release",
    "semantic-release": "semantic-release"
  }
}

// GitHub Actions - .github/workflows/release.yml
name: Release
on:
  push:
    branches: [main]

jobs:
  release:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0
          
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          registry-url: 'https://registry.npmjs.org'
          
      - run: npm ci
      
      - run: npm run build
      
      - run: npx semantic-release
        env:
          GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
          NPM_TOKEN: ${{ secrets.NPM_TOKEN }}
```

## Release Workflow

```
1. Developer commits with conventional format:
   git commit -m "feat(api): add user search endpoint"

2. Push to main:
   git push origin main

3. CI runs tests and linting

4. semantic-release analyzes commits:
   - "feat" → minor version bump (1.0.0 → 1.1.0)
   - "fix" → patch version bump
   - "feat!" or "BREAK CHANGE" → major version bump

5. Creates GitHub release with changelog

6. Publishes to npm with new version
```

## Customizing Changelog

```js
// .releaserc.json with custom configuration
{
  "branches": ["main"],
  "plugins": [
    [
      "@semantic-release/commit-analyzer",
      {
        "preset": "conventionalcommits",
        "releaseRules": [
          { "type": "feat", "release": "minor" },
          { "type": "fix", "release": "patch" },
          { "type": "perf", "release": "patch" },
          { "type": "refactor", "release": "patch" },
          { "type": "docs", "release": "patch" },
          { "type": "test", "release": "patch" },
          { "type": "build", "release": "patch" },
          { "type": "ci", "release": "patch" },
          { "type": "chore", "release": false },
          { "type": "BREAKING CHANGE", "release": "major" }
        ]
      }
    ],
    [
      "@semantic-release/release-notes-generator",
      {
        "preset": "conventionalcommits",
        "presetConfig": {
          "types": [
            { "type": "feat", "section": "Features", "hidden": false },
            { "type": "fix", "section": "Bug Fixes", "hidden": false }
          ]
        }
      }
    ],
    "@semantic-release/changelog",
    "@semantic-release/npm",
    "@semantic-release/github"
  ]
}
```

## CHANGELOG.md Generated Output

```markdown
# 1.1.0 (2024-01-15)

### Features

* **api:** add user search endpoint (abc123)

### Bug Fixes

* **auth:** resolve login bug (def456)
```

## ⚠️ Common Mistakes

1. **Not using conventional commits**: Without proper format, semantic-release can't determine version
2. **Skipping tests**: Always run tests in CI before release
3. **Missing NPM_TOKEN**: Set NPM token as secret in GitHub/GitLab

## ✅ Quick Recap

- Use conventional commit format: `type(scope): message`
- semantic-release auto-generates changelog and versions
- Configure in .releaserc.json
- Set up GitHub Actions for automated releases
- Commit types: feat (minor), fix (patch), BREAKING CHANGE (major)

## 🔗 What's Next

You've completed the Developer Experience section! You now have a comprehensive Express.js guide with advanced topics covering production readiness, architecture, API patterns, security, performance, integrations, and developer tooling.
