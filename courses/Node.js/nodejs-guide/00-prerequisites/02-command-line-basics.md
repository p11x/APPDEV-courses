# Command Line Basics

## What You'll Learn

- How to navigate the filesystem from the terminal
- Essential commands for Node.js development
- How to run Node.js scripts from the command line
- Environment variables and PATH
- Package management basics

## Terminal Basics

### Navigation Commands

```bash
# Print current directory
pwd

# List files and directories
ls              # Basic listing
ls -la          # Detailed listing with hidden files
ls -lh          # Human-readable file sizes

# Change directory
cd /home/user           # Absolute path
cd projects             # Relative path
cd ..                   # Go up one level
cd ~                    # Go to home directory
cd -                    # Go to previous directory

# Create directories
mkdir my-project        # Create single directory
mkdir -p src/utils      # Create nested directories

# Create files
touch index.js          # Create empty file
echo "hello" > file.txt # Create file with content

# Copy and move
cp file.js backup.js        # Copy file
cp -r src/ backup-src/      # Copy directory
mv old-name.js new-name.js  # Rename/move file
rm file.js                  # Delete file
rm -rf directory/           # Delete directory (careful!)

# View file contents
cat file.js             # Print entire file
head -20 file.js        # First 20 lines
tail -20 file.js        # Last 20 lines
less file.js            # Paginated view
```

### Running Node.js

```bash
# Run a JavaScript file
node index.js

# Run with arguments
node index.js --name Alice --port 3000

# Run with environment variables
NODE_ENV=production node index.js

# Run with inspect (debugging)
node --inspect index.js

# Run with watch mode (auto-restart on changes)
node --watch index.js

# Check Node.js version
node --version
node -v

# Check npm version
npm --version
npm -v
```

### npm Commands

```bash
# Initialize a project
npm init          # Interactive setup
npm init -y       # Accept all defaults

# Install packages
npm install express           # Install and save to dependencies
npm install -D typescript     # Save to devDependencies
npm install -g nodemon        # Install globally

# Run scripts (defined in package.json)
npm start           # Runs "start" script
npm test            # Runs "test" script
npm run build       # Runs "build" script
npm run dev         # Runs "dev" script

# Other useful commands
npm list            # Show installed packages
npm outdated        # Check for updates
npm audit           # Check for vulnerabilities
npm update          # Update packages
npm cache clean     # Clear npm cache
```

### Git Basics (for Node.js projects)

```bash
# Clone a repository
git clone https://github.com/user/repo.git

# Check status
git status

# Stage and commit
git add .                    # Stage all changes
git add index.js             # Stage specific file
git commit -m "Add feature"  # Commit with message

# Branch management
git branch feature-name      # Create branch
git checkout feature-name    # Switch to branch
git checkout -b feature-name # Create and switch
git merge feature-name       # Merge branch

# Push and pull
git push origin main         # Push to remote
git pull origin main         # Pull from remote
```

## Environment Variables

```bash
# Set temporarily (current session only)
export NODE_ENV=production
export PORT=3000

# Set for single command
NODE_ENV=production node server.js

# View all environment variables
env
echo $NODE_ENV

# In Node.js code, access with process.env
# const port = process.env.PORT || 3000;
```

## PATH

The PATH is a list of directories where the system looks for commands.

```bash
# View current PATH
echo $PATH

# Add Node.js to PATH (usually done by nvm installer)
export PATH="$HOME/.nvm/versions/node/v20/bin:$PATH"

# Find where a command is installed
which node
which npm
which npx
```

## File Permissions

```bash
# Make a file executable (for CLI tools)
chmod +x index.js

# Run an executable
./index.js

# Check permissions
ls -la index.js
```

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl+C` | Stop running process |
| `Ctrl+Z` | Pause running process |
| `Ctrl+L` | Clear terminal |
| `Ctrl+A` | Move to beginning of line |
| `Ctrl+E` | Move to end of line |
| `Ctrl+R` | Search command history |
| `Tab` | Auto-complete file/command |
| `↑` / `↓` | Navigate command history |

## Common Mistakes

### Mistake 1: Running node without the file

```bash
# WRONG — node enters REPL mode
node

# CORRECT — specify the file
node index.js
```

### Mistake 2: Forgetting to install dependencies

```bash
# WRONG — running without node_modules
node index.js
# Error: Cannot find module 'express'

# CORRECT — install first
npm install
node index.js
```

### Mistake 3: Running as root

```bash
# WRONG — running npm as root causes permission issues
sudo npm install -g some-package

# CORRECT — use nvm or fix npm permissions
# nvm handles this automatically
```

## Next Steps

Now you can navigate the terminal. Let's learn Git basics for version control. Continue to [Git Basics](./03-git-basics.md).
