# Verifying Your Node.js Setup

## What You'll Learn

- How to verify Node.js is installed correctly
- How to check npm (Node Package Manager) version
- How to run your first JavaScript script
- How to troubleshoot common installation issues

## Checking Your Node.js Installation

Now that you've installed Node.js (either directly or through nvm), let's verify everything is working correctly.

### Check Node.js Version

Open your terminal (Command Prompt on Windows, Terminal on macOS/Linux) and run:

```bash
node --version
```

You should see output like:
```
v20.11.0
```

This tells you Node.js is installed and you're using version 20.11.0.

### Check npm Version

npm (Node Package Manager) comes bundled with Node.js. Check its version:

```bash
npm --version
```

You should see output like:
```
10.2.4
```

This confirms npm is working correctly.

### Check Node.js and npm Together

You can check both at once using:

```bash
node -v && npm -v
```

The `-v` flag is a shorthand for `--version` in both tools.

## Running Your First Script

### Step 1: Create a JavaScript File

Create a new file called `hello.js` in your project folder and add this code:

```javascript
// A simple script to verify Node.js is working
console.log('Hello from Node.js!');

// Display some version information
console.log('Node.js version:', process.version);
console.log('Platform:', process.platform);
```

### Step 2: Run the Script

In your terminal, run:

```bash
node hello.js
```

You should see output like:
```
Hello from Node.js!
Node.js version: v20.11.0
Platform: win32
```

### Understanding the Output

- `process.version`: Shows the exact Node.js version you're running
- `process.platform`: Shows your operating system (`win32` for Windows, `darwin` for macOS, `linux` for Linux)

## Understanding npm

npm is the **Node Package Manager** - a tool that comes with Node.js to help you:
- Download and install third-party packages (libraries)
- Manage project dependencies
- Run scripts

### What is a Package?

A **package** is a collection of JavaScript code that someone else wrote and shared. Packages can range from small utilities to entire frameworks.

### Popular npm Packages

Some well-known packages include:
- **Express**: A web framework for building servers
- **React**: A library for building user interfaces
- **VS Code**: Microsoft's code editor (yes, it's built with Node.js!)

## Code Example: Exploring npm

Create a script to explore npm:

```javascript
// This script demonstrates npm and process information
console.log('=== npm Information ===\n');

// Check npm version
console.log('npm version:', process.env.npm_config_user_agent?.split('npm/')[1] || 'unknown');

// List environment variables related to npm (if any)
const npmVars = Object.keys(process.env)
  .filter(key => key.startsWith('npm_'));
  
if (npmVars.length > 0) {
  console.log('\nnpm-related environment variables:');
  npmVars.forEach(key => console.log(`  ${key}`));
} else {
  console.log('\nNo npm environment variables set');
}

// Show Node.js paths
console.log('\n=== Node.js Paths ===');
console.log('execPath:', process.execPath);
console.log('cwd:', process.cwd());
```

Run it with:
```bash
node check-npm.js
```

## Troubleshooting Common Issues

### Issue 1: "node is not recognized"

If you see `'node' is not recognized as an internal or external command` on Windows:

1. **If using nvm for Windows**: Run Command Prompt as Administrator, then run `nvm on`
2. **Check PATH**: Make sure Node.js is in your system PATH
3. **Restart terminal**: Close and reopen Command Prompt

### Issue 2: Wrong Node.js Version

If you installed Node.js but get an unexpected version:

```bash
# Check which node is being used
where node    # Windows
which node    # macOS/Linux

# Check nvm current version
nvm current   # If using nvm

# Switch to desired version
nvm use 20    # Or whatever version you want
```

### Issue 3: npm Not Working

If npm commands fail:

```bash
# Clear npm cache
npm cache clean --force

# Reinstall npm (if needed)
npm install npm@latest -g
```

### Issue 4: Permission Errors on macOS/Linux

If you get permission errors when installing global packages:

```bash
# DON'T use sudo (this is dangerous!)
# Instead, set npm to use a different directory
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'

# Add to your .bashrc or .zshrc:
echo 'export PATH=~/.npm-global/bin:$PATH' >> ~/.bashrc
source ~/.bashrc
```

## Best Practices

### 1. Always Check Versions First
When troubleshooting, always check:
```bash
node --version
npm --version
```

### 2. Keep Node.js Updated
Use nvm to stay on the latest LTS version:
```bash
nvm install --lts
nvm alias default lts
```

### 3. Use LTS (Long Term Support) Versions
For production applications, always use LTS versions. They receive bug fixes and security updates for longer.

## Try It Yourself

### Exercise 1: Version Check
Run the version check commands and note down:
- Your Node.js version
- Your npm version
- Your operating system

### Exercise 2: Create a Simple Calculator
Create a JavaScript file that:
1. Adds two numbers (5 + 3)
2. Multiplies two numbers (4 * 7)
3. Prints both results

### Exercise 3: Explore process Object
Create a script that prints various properties of the `process` object (platform, arch, pid, etc.).

## Next Steps

Now that you've verified your setup is working, let's learn about the Node REPL (Read-Eval-Print Loop), which is an interactive way to experiment with JavaScript. Continue to [Node REPL Guide](../03-repl-guide.md).
