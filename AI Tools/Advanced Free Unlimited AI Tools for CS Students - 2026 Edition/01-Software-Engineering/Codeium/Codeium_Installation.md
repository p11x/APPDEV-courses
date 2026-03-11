# Codeium - Installation Guide

## Installation Methods

### Method 1: VS Code (Recommended)

#### Step 1: Open VS Code

Launch Visual Studio Code on your system.

#### Step 2: Access Extensions

Navigate to the Extensions view:
- **Windows/Linux**: `Ctrl+Shift+X`
- **macOS**: `Cmd+Shift+X`

#### Step 3: Search for Codeium

In the search bar, type: `Codeium`

#### Step 4: Install Extension

1. Find "Codeium" in the results
2. Click the "Install" button
3. Wait for installation to complete
4. Restart VS Code when prompted

#### Alternative: Command Line Installation

```bash
# Install via VS Code CLI
code --install-extension codeium.codeium

# Verify installation
code --list-extensions | grep codeium
```

---

### Method 2: JetBrains IDEs

#### Step 1: Open Settings

- **Windows/Linux**: `File > Settings`
- **macOS**: `Code > Preferences > Settings`

#### Step 2: Navigate to Plugins

1. Go to "Plugins" tab
2. Click "Marketplace"
3. Search for "Codeium"

#### Step 3: Install and Restart

1. Click "Install"
2. Restart your IDE
3. Enable Codeium in settings

---

### Method 3: Vim/Neovim

#### Using vim-plug

```vim
" Add to your .vimrc
Plug 'Exafunction/codeium.vim'

" Then run
:PlugInstall
```

#### Using packer.nvim

```lua
-- Add to your packer config
use 'Exafunction/codeium.vim'
```

---

### Method 4: Jupyter Notebook

```bash
# Install the extension
pip install jupyterlab-codeium

# Enable the extension
jupyter labextension install @codeium-ai/jupyterlab-codeium
```

---

## Post-Installation Setup

### Step 1: Create Account

1. After installation, Codeium will prompt you
2. Click "Sign Up" or "Login"
3. Enter your email or sign in with Google/GitHub

### Step 2: Authenticate

```bash
# VS Code will show a code to enter on Codeium website
# Complete authentication in browser
# Return to VS Code - should show "Authenticated as [email]"
```

### Step 3: Configure Settings

```json
// In VS Code settings.json
{
  "codeium.enable": true,
  "codeium.advanced": {
    "apiUrl": "https://api.codeium.com",
    "managerUrl": "https://manager.codeium.com"
  }
}
```

---

## Troubleshooting

### Issue: Extension Not Installing

**Symptoms**: Installation fails or shows error

**Solutions**:

1. **Check Internet Connection**
   ```bash
   ping api.codeium.com
   ```

2. **Clear VS Code Cache**
   ```bash
   # Windows
   %APPDATA%\Codeium\Cache
   
   # macOS
   ~/Library/Caches/Codeium/
   
   # Linux
   ~/.config/Codeium/Cache/
   ```

3. **Check Version Compatibility**
   - Ensure VS Code is version 1.65 or higher
   - Update VS Code to latest version

---

### Issue: Authentication Failed

**Symptoms**: Cannot authenticate, login loop

**Solutions**:

1. **Check Firewall Settings**
   - Allow connections to `codeium.com`
   - Check corporate VPN settings

2. **Clear Authentication Data**
   ```bash
   # Windows
   del "%APPDATA%\Codeium\Data\*
   
   # macOS
   rm -rf ~/Library/Application\ Support/Codeium/Data/*
   
   # Linux
   rm -rf ~/.config/Codeium/Data/*
   ```

3. **Try Different Browser**
   - Use Chrome for authentication
   - Disable ad blockers

---

### Issue: Completions Not Working

**Symptoms**: No suggestions appearing

**Solutions**:

1. **Check if Codeium is Enabled**
   ```json
   {
     "codeium.enable": true
   }
   ```

2. **Check Language Support**
   - Ensure language is in supported list
   - Try with a different file

3. **Restart Codeium**
   - Press `Ctrl+Shift+P`
   - Type "Codeium: Restart"

4. **Check Logs**
   ```bash
   # View Codeium logs in VS Code
   # Help > Toggle Developer Tools > Console
   ```

---

### Issue: Slow Performance

**Symptoms**: Laggy completions, high CPU

**Solutions**:

1. **Reduce Model Size**
   ```json
   {
     "codeium.advanced.modelSize": "small"
   }
   ```

2. **Disable for Large Files**
   ```json
   {
     "codeium.enableForFilesLargerThan": 1000000
   }
   ```

3. **Clear Cache**
   ```bash
   # Close VS Code
   # Delete cache folder
   # Reopen VS Code
   ```

---

### Issue: JetBrains Not Showing Codeium

**Symptoms**: Extension installed but not visible

**Solutions**:

1. **Restart IDE Completely**
   - Close all JetBrains windows
   - Reopen IDE

2. **Check Plugin Settings**
   - Go to Settings > Plugins > Codeium
   - Ensure "Enabled" is checked

3. **Check IDE Version**
   - Ensure using IntelliJ 2020.1 or higher

---

## Verification

### Check Installation Status

```bash
# VS Code - Check extensions
code --list-extensions | grep codeium

# Should output: codeium.codeium
```

### Test Completions

1. Open any code file
2. Start typing a function
3. Look for Codeium suggestions (gray text with Codeium icon)
4. Press Tab to accept

---

*Back to [Codeium README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*