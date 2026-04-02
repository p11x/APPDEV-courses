# Terminal Cheatsheet

## Quick Reference for Node.js Development

### File Operations

| Command | Description |
|---------|-------------|
| `ls -la` | List all files with details |
| `cd <dir>` | Change directory |
| `pwd` | Print current directory |
| `mkdir -p a/b/c` | Create nested directories |
| `touch file.js` | Create empty file |
| `cp src dest` | Copy file |
| `mv src dest` | Move/rename file |
| `rm file` | Delete file |
| `rm -rf dir` | Delete directory |
| `cat file` | Print file contents |
| `head -n 20 file` | First 20 lines |
| `tail -f log.txt` | Follow log file in real time |

### Node.js Commands

| Command | Description |
|---------|-------------|
| `node script.js` | Run a script |
| `node --watch script.js` | Run with auto-restart |
| `node --inspect script.js` | Run with debugger |
| `node -e "code"` | Evaluate inline code |
| `npx <package>` | Run a package without installing |

### npm Commands

| Command | Description |
|---------|-------------|
| `npm init -y` | Create package.json |
| `npm install` | Install all dependencies |
| `npm install <pkg>` | Install a package |
| `npm install -D <pkg>` | Install as dev dependency |
| `npm uninstall <pkg>` | Remove a package |
| `npm run <script>` | Run a script from package.json |
| `npm test` | Run tests |
| `npm outdated` | Check for outdated packages |
| `npm audit` | Check for vulnerabilities |
| `npm update` | Update packages |

### Git Commands

| Command | Description |
|---------|-------------|
| `git init` | Initialize repository |
| `git clone <url>` | Clone a repository |
| `git status` | Show changes |
| `git add .` | Stage all changes |
| `git commit -m "msg"` | Commit changes |
| `git push` | Push to remote |
| `git pull` | Pull from remote |
| `git branch <name>` | Create branch |
| `git checkout <branch>` | Switch branch |
| `git merge <branch>` | Merge branch |
| `git log --oneline` | View commit history |

### Process Management

| Command | Description |
|---------|-------------|
| `Ctrl+C` | Stop process |
| `Ctrl+Z` | Pause process |
| `fg` | Resume paused process |
| `bg` | Run paused process in background |
| `jobs` | List background jobs |
| `kill <pid>` | Kill process by ID |
| `lsof -i :3000` | Find process using port 3000 |
| `ps aux | grep node` | Find Node.js processes |

### Network

| Command | Description |
|---------|-------------|
| `curl http://localhost:3000` | HTTP GET request |
| `curl -X POST -d '{}' http://...` | HTTP POST request |
| `curl -i http://...` | Include response headers |
| `ping localhost` | Test connectivity |
| `netstat -tlnp` | List listening ports |

### Environment

| Command | Description |
|---------|-------------|
| `export VAR=value` | Set environment variable |
| `echo $VAR` | Print variable |
| `env` | List all variables |
| `which node` | Find node installation |
| `node -v` | Node.js version |
| `npm -v` | npm version |

### Searching

| Command | Description |
|---------|-------------|
| `grep "text" file` | Search in file |
| `grep -r "text" dir` | Search recursively |
| `find . -name "*.js"` | Find files by name |
| `wc -l file` | Count lines |

### Package.json Scripts Template

```json
{
  "scripts": {
    "start": "node server.js",
    "dev": "node --watch server.js",
    "test": "node --test",
    "lint": "eslint .",
    "format": "prettier --write .",
    "build": "tsc"
  }
}
```

## Now You're Ready

With these commands under your belt, you're ready to start the Node.js guide. Continue to [Chapter 01: Introduction](../01-introduction/what-is-nodejs/01-overview.md).
