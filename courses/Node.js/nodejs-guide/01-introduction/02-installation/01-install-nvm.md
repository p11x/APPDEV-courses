# Installing nvm (Node Version Manager)

## What You'll Learn

- What nvm is and why you should use it
- How to install nvm on Windows, macOS, and Linux
- How to install and switch between Node.js versions
- Best practices for managing Node.js versions

## What is nvm?

**nvm** (Node Version Manager) is a tool that lets you install and manage multiple versions of Node.js on your computer. Why would you want this?

- **Different projects require different versions**: Some older projects might need Node.js 14, while newer projects use Node.js 20.
- **Testing compatibility**: You can easily test your code on multiple Node.js versions.
- **Latest features**: You can quickly upgrade to the latest Node.js version when it's released.

There are two versions of nvm:
- **nvm for Windows**: A separate project (nvm-windows)
- **nvm for macOS/Linux**: The original nvm (works via shell)

## Installing nvm on Windows

### Step 1: Uninstall Existing Node.js

Before installing nvm on Windows, uninstall any existing Node.js installation:
1. Go to Windows Settings > Apps
2. Find Node.js in the list
3. Click Uninstall

### Step 2: Download and Install nvm for Windows

1. Visit: https://github.com/coreybutler/nvm/releases
2. Download the latest `nvm-setup.exe`
3. Run the installer
4. Follow the installation prompts

### Step 3: Verify Installation

Open a new Command Prompt or PowerShell window and run:

```bash
nvm version
```

You should see a version number like `1.1.12`.

## Installing nvm on macOS and Linux

### Step 1: Install nvm via cURL or Wget

Open your terminal and run one of these commands:

**Using cURL:**
```bash
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.7/install.sh | bash
```

**Using Wget:**
```bash
wget -qO- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.7/install.sh | bash
```

### Step 2: Add nvm to Your Shell Profile

The installer adds these lines to your profile automatically. If it doesn't, add them manually:

**For bash (add to ~/.bashrc):**
```bash
export NVM_DIR="$HOME/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"
```

**For zsh (add to ~/.zshrc):**
```bash
export NVM_DIR="$HOME/.nvm"
[ -s "$Nvm_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"
```

### Step 3: Verify Installation

Close and reopen your terminal, then run:

```bash
nvm --version
```

You should see a version number.

## Installing Node.js with nvm

### Install the Latest LTS Version

**On Windows (Command Prompt):**
```bash
nvm install lts
```

**On macOS/Linux (Terminal):**
```bash
nvm install --lts
```

### Install a Specific Version

You can install any Node.js version you need:

```bash
# Install Node.js version 20
nvm install 20

# Install Node.js version 18
nvm install 18

# Install the latest version (not LTS)
nvm install latest
```

### List Available Versions

See what versions are installed and which is currently active:

```bash
nvm list
```

On Windows, use:
```bash
nvm list installed
```

## Switching Between Versions

### Use a Specific Version

```bash
# Switch to Node.js version 20
nvm use 20

# Switch to the LTS version
nvm use --lts
```

On Windows, you might need to run Command Prompt as Administrator.

### Set a Default Version

```bash
# Set Node.js 20 as the default for new terminals
nvm alias default 20
```

### Check Current Version

```bash
# See which version is currently active
node --version

# Or use nvm
nvm current
```

## Code Example: Managing Node.js Versions

Here's a quick overview of common nvm commands:

```bash
# Install Node.js 20
nvm install 20

# Use Node.js 20 in current terminal
nvm use 20

# Verify it's working
node --version   # Should show v20.x.x
npm --version    # Should show a version number

# Install Node.js 18 as well
nvm install 18

# Switch to Node.js 18
nvm use 18

# Verify
node --version   # Should show v18.x.x

# Switch back to Node.js 20
nvm use 20
```

## How It Works

### How nvm Works on macOS/Linux

nvm works by downloading and installing different Node.js versions in your home directory (typically `~/.nvm/versions/node/`). When you run `nvm use 20`, it:

1. Finds the installed Node.js v20 directory
2. Updates symbolic links (or modifies PATH) to point to that version
3. The `node` command now runs that specific version

### How nvm Works on Windows

nvm for Windows works differently. It:
1. Installs each Node.js version in `C:\Program Files\nodejs\`
2. Uses environment variables to select which version is active
3. Creates separate directories for each version (e.g., `v20.0.0`)

## Common Mistakes

### Mistake 1: Not Closing the Terminal
After installing nvm or changing versions, always close and reopen your terminal. The new settings won't take effect in the current session.

### Mistake 2: Forgetting to Use `nvm use`
Installing Node.js doesn't automatically activate it. You must run `nvm use <version>` or set a default alias.

### Mistake 3: Confusing nvm with npm
- **nvm**: Manages Node.js versions
- **npm**: The Node Package Manager (comes with Node.js) - manages project dependencies

They are different tools!

### Mistake 4: Installing nvm Without Uninstalling Existing Node.js
On Windows, make sure to uninstall Node.js before installing nvm, or the installation might fail.

## Try It Yourself

### Exercise 1: Install nvm
Install nvm on your operating system using the appropriate method above.

### Exercise 2: Install Two Versions
Install both Node.js 18 and Node.js 20 using nvm.

### Exercise 3: Switch Versions
Switch between the two versions and verify with `node --version`.

## Next Steps

Now that you have nvm installed, let's verify your setup is working correctly. Continue to [Verify Setup](../02-verify-setup.md) to check that Node.js and npm are properly installed.
