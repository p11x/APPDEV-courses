# Installing Node.js and npm

## 📌 What You'll Learn
- How to install Node.js and npm
- Verifying your installation
- Understanding what Node.js and npm do

## 🧠 Concept Explained (Plain English)

Before you can use Express, you need to install Node.js and npm. But what are they?

**Node.js** is JavaScript that runs on your computer's server instead of in a web browser. It lets you write server-side applications using JavaScript — the same language you might use for frontend web development.

**npm** (Node Package Manager) comes with Node.js and is like a giant online store for code. Need a library to handle dates? Send emails? Build APIs? There's probably an npm package for it. npm lets you download and manage these packages in your projects.

Think of it like this: Node.js is the engine that runs your server code, and npm is the mechanic who brings in the tools (packages) you need.

## 💻 Installation Steps

### Option 1: Download from Website (Easiest)

1. Go to [nodejs.org](https://nodejs.org)
2. Download the **LTS** (Long Term Support) version — it's more stable
3. Run the installer and follow the prompts

### Option 2: Using a Version Manager (Recommended for Developers)

Version managers let you switch between different Node versions easily.

**On Windows:**
Download and install [nvm-windows](https://github.com/coreybutler/nvm-windows)

**On Mac or Linux:**
```bash
# Install nvm (Node Version Manager)
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash

# Restart your terminal, then:
nvm install 20      # Install Node.js version 20
nvm use 20          # Use Node.js version 20
nvm install --lts  # Install latest LTS version
```

## Verifying Installation

Open your terminal (Command Prompt on Windows, Terminal on Mac/Linux) and type:

```bash
# Check Node.js version
node --version

# Check npm version
npm --version

# Both should display version numbers like:
# node --version => v20.10.0
# npm --version => 10.2.3
```

## Understanding the Output

When you run these commands, you'll see something like:

```bash
$ node --version
v20.10.0

$ npm --version
10.2.3
```

The numbers tell you which version you have. For Express 5, you should use Node.js 18 or newer.

## 💻 Quick Test

Create a test file to verify everything works:

```javascript
// test.js
// Save this file and run: node test.js

console.log('Node.js is working!');
console.log('Current version:', process.version);

// npm is available for installing packages
console.log('npm is ready to use');
```

Run it with:
```bash
node test.js
```

## 🔍 Understanding npm

| Command | What It Does |
|---------|--------------|
| `npm init` | Create a new project |
| `npm install package` | Install a package locally |
| `npm install -g package` | Install package globally |
| `npm install package --save` | Add to dependencies |
| `npm install package --save-dev` | Add to dev dependencies |

## ⚠️ Common Mistakes

**1. Using an outdated Node version**
Express 5 requires Node.js 18 or newer. Older versions won't work.

**2. Not restarting the terminal**
After installing Node.js, close and reopen your terminal to ensure the new commands are available.

**3. Confusing npm with npx**
- `npm` = Node Package Manager (installs packages)
- `npx` = Node Package Executor (runs packages without installing)

## ✅ Quick Recap

- Download Node.js LTS from nodejs.org
- npm comes bundled with Node.js
- Verify installation with `node --version` and `npm --version`
- You need Node 18+ for Express 5

## 🔗 What's Next

Now let's create your first Express project and understand package.json.
