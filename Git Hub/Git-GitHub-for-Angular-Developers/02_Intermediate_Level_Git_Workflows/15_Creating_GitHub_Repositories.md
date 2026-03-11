# Creating GitHub Repositories

## Topic Title
Setting Up and Managing GitHub Repositories for Angular Projects

## Concept Explanation

Creating a GitHub repository is the first step to share your Angular projects with the world or collaborate with your team. A well-organized repository makes your project accessible, maintainable, and attractive to contributors.

### Why Create GitHub Repositories?

1. **Share code**: Make your Angular projects accessible
2. **Collaboration**: Enable team work
3. **Backup**: Offsite code storage
4. **Portfolio**: Showcase your work
5. **Open Source**: Contribute to the community

## Methods to Create Repositories

### Method 1: GitHub Web Interface

#### Step-by-Step

1. **Log in to GitHub**
   - Go to github.com
   - Click "Sign in" if needed

2. **Create New Repository**
   - Click "+" in the top right
   - Select "New repository"

3. **Configure Repository**
   ```
   Owner: your-username
   Repository name: my-angular-app
   
   Description: (optional)
   
   Public ✓  Private ○
   
   ☑ Add a README file
   ☑ Add .gitignore (choose: Angular)
   ```

4. **Click "Create repository"**

### Method 2: GitHub CLI

```bash
# Install GitHub CLI
# Windows: choco install gh
# Mac: brew install gh
# Linux: see gh docs

# Log in
gh auth login

# Create repository
gh repo create my-angular-app --public --source --clone

# Or with options
gh repo create my-angular-app \
  --description "My Angular app" \
  --public \
  --add-gitignore Angular \
  --clone
```

### Method 3: Command Line (Existing Project)

```bash
# 1. Create repository on GitHub first
# (using web or CLI)

# 2. In your local project:
git remote add origin https://github.com/username/my-angular-app.git

# 3. Push your code
git push -u origin main
```

## Connecting Local Git to GitHub

### After Creating Empty Repository

```bash
# Navigate to your Angular project
cd my-angular-app

# Initialize Git (if not already)
git init

# Configure Git (if not already)
git config --global user.name "Your Name"
git config --global user.email "your@email.com"

# Add remote
git remote add origin https://github.com/username/my-angular-app.git

# Verify remote
git remote -v

# Stage all files
git add .

# Commit
git commit -m "Initial commit"

# Push to GitHub
git push -u origin main
```

### From GitHub with Template

```bash
# Clone with template
git clone https://github.com/github/gitignore temp-repo
cd temp-repo
rm -rf .git
# Your code here
git init
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/username/repo.git
git push -u origin main
```

## Repository Settings

### Basic Settings

Configure in GitHub:
```
Settings → Options
```

- Repository name
- Description
- Default branch
- Include README

### Collaborators

Add team members:
```
Settings → Collaborators → Add collaborator
```

Enter username or email.

### Branch Protection

Protect main branch:
```
Settings → Branches → Branch protection rules
→ Add rule
```

Options:
- Require pull request reviews
- Require status checks
- Require conversation resolution

## README Best Practices

### Structure

```markdown
# Project Name

Brief description of what this project does.

## Features

- Feature 1
- Feature 2

## Installation

```bash
npm install
ng serve
```

## Usage

Explain how to use the project.

## Contributing

How to contribute to this project.

## License

MIT
```

### Angular-Specific README

```markdown
# My Angular Dashboard

A responsive admin dashboard built with Angular 17.

## Features

- User management
- Data visualization
- Real-time updates
- Dark mode support

## Tech Stack

- Angular 17
- Angular Material
- NgRx for state management

## Getting Started

### Prerequisites

- Node.js 18+
- npm 9+

### Installation

```bash
git clone https://github.com/username/my-dashboard.git
cd my-dashboard
npm install
ng serve
```

Navigate to http://localhost:4200/

## Development

```bash
# Generate component
ng generate component components/user-list

# Run tests
ng test
```

## Build

```bash
ng build --configuration production
```

## License

MIT
```

## .gitignore for Angular

When creating repository, select "Angular" as .gitignore template:

```gitignore
# Angular
## Angular ##
.angular/
dist/
tmp/
app/**/*.js
app/**/*.js.map

# Dependencies
node_modules/

# IDEs
.vscode/
.idea/

# System
.DS_Store
Thumbs.db

# Environment
.env

# Logs
*.log
```

## Real-World Workflows

### New Project from Scratch

```bash
# 1. Create Angular project locally
ng new my-angular-app

# 2. Create GitHub repo (via web or CLI)
gh repo create my-angular-app --public --add-gitignore Angular

# 3. Push (already configured by CLI)
git push -u origin main
```

### Existing Local Project

```bash
# 1. Go to github.com
# 2. Create new repository (don't initialize)
# 3. Get URL

# In your project:
git remote add origin https://github.com/username/repo.git
git branch -M main
git push -u origin main
```

### Fork and Clone

```bash
# 1. Fork repository on GitHub
# 2. Clone your fork
git clone https://github.com/your-username/angular.git

# 3. Add upstream (original repo)
cd angular
git remote add upstream https://github.com/angular/angular.git

# 4. Keep updated
git fetch upstream
git merge upstream/main
```

## Best Practices

### 1. Good Repository Names

```bash
# ✓ Good
angular-user-dashboard
my-portfolio-website
ngx-file-uploader

# ✗ Bad
my-app
test-project
 stuff
```

### 2. Include Essential Files

- README.md - Documentation
- .gitignore - Exclude files
- LICENSE - Legal (MIT is common)
- CONTRIBUTING.md - Contribution guidelines

### 3. Write Good Descriptions

- Clear and concise
- Mention tech stack
- Highlight key features

### 4. Make First Commit Count

```bash
# Don't commit huge files
# Don't commit node_modules
# Include .gitignore before first commit
```

## Common Mistakes

### Mistake 1: Wrong Remote URL

```bash
# Check remote
git remote -v

# Fix if wrong
git remote set-url origin correct-url
```

### Mistake 2: Pushing Without .gitignore

```bash
# Already pushed node_modules?
# Add to .gitignore, then:
git rm -r --cached node_modules
git commit -m "Stop tracking node_modules"
git push
```

### Mistake 3: Wrong Branch Name

```bash
# Rename local branch
git branch -M main

# Push main, not master
git push -u origin main
```

## Exercises for Students

### Exercise 1: Create First Repository
1. Create new repository on GitHub
2. Add Angular .gitignore
3. Add README
4. Clone locally
5. Add an Angular file
6. Push back to GitHub

### Exercise 2: Connect Existing Project
1. Take an existing Angular project
2. Create GitHub repository
3. Connect and push

### Exercise 3: Fork and Clone
1. Fork an Angular library
2. Clone locally
3. Explore the code

## Summary

Creating GitHub repositories is essential:

- **Three methods**: Web, CLI, or command line
- **Remote connection**: Link local Git to GitHub
- **Essential files**: README, .gitignore, LICENSE
- **Settings**: Configure visibility, collaborators, protection

Key steps:
1. Create repository on GitHub
2. Connect with `git remote add`
3. Push with `git push -u`

Your Angular projects are now shareable!

---

**Next Lesson**: [GitHub Clone](./16_GitHub_Clone.md)

**Previous Lesson**: [Introduction to GitHub](./14_Introduction_to_GitHub.md)
