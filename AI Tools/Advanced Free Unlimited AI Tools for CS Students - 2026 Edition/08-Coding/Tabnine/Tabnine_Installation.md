# Tabnine - Installation

## Installation Methods

### Method 1: VS Code (Recommended)

1. **Open VS Code**
   - Launch Visual Studio Code on your system

2. **Access Extensions Panel**
   - Click the Extensions icon in the Activity Bar (left sidebar)
   - Or press `Ctrl+Shift+X` (Windows/Linux) / `Cmd+Shift+X` (macOS)

3. **Search for Tabnine**
   - Type "Tabnine" in the search bar
   - Look for "Tabnine AI Code Completion" by Tabnine

4. **Install Extension**
   - Click the "Install" button
   - Wait for installation to complete

5. **Restart VS Code**
   - Reload VS Code to activate the extension

### Method 2: JetBrains IDEs (IntelliJ, WebStorm, PyCharm)

1. **Open IDE Settings**
   - Go to `File` → `Settings` (Windows/Linux) or `Preferences` (macOS)

2. **Navigate to Plugins**
   - Click on "Plugins" in the left panel
   - Click on "Marketplace" tab

3. **Search and Install**
   - Search for "Tabnine"
   - Click "Install" button

4. **Restart IDE**
   - Restart your JetBrains IDE

### Method 3: Eclipse

1. **Open Eclipse**
   - Launch Eclipse IDE

2. **Access Eclipse Marketplace**
   - Go to `Help` → `Eclipse Marketplace`

3. **Search for Tabnine**
   - Search "Tabnine AI Code Completion"
   - Click "Install"

4. **Confirm Installation**
   - Follow the wizard prompts
   - Restart Eclipse

### Method 4: Vim/Neovim

**Using Vim-Plug:**

```vim
" Add to your .vimrc
Plug 'TabnineByTailoredC/TabnineVim'
```

Then run:
```bash
vim +PlugInstall +qall
```

**Using Dein:**

```vim
" Add to your .vimrc
call dein#add('TabnineByTailuredC/TabnineVim')
```

### Method 5: Sublime Text

1. **Open Sublime Text**
   - Launch Sublime Text

2. **Access Package Control**
   - Press `Ctrl+Shift+P` (Windows/Linux) / `Cmd+Shift+P` (macOS)
   - Type "Package Control: Install Package"

3. **Search and Install**
   - Search for "Tabnine"
   - Click to install

### Method 6: Atom

1. **Open Atom**
   - Launch Atom editor

2. **Access Settings**
   - Go to `File` → `Settings`
   - Click on "Install" tab

3. **Search and Install**
   - Search for "Tabnine"
   - Click "Install"

### Method 7: Standalone Installation

**Download from Official Website:**

1. Visit: https://www.tabnine.com/install
2. Select your IDE/Editor
3. Download the appropriate package
4. Follow the installation wizard

---

## Post-Installation Setup

### First Launch

1. **Extension Activation**
   - Tabnine will automatically activate
   - You'll see a Tabnine icon in the status bar

2. **Initial Configuration**
   - Open Settings: `File` → `Preferences` → `Settings`
   - Search for "tabnine"

### Recommended Settings

```json
{
  "tabnine.enable": true,
  "tabnine.cloud_enabled": false,
  "tabnine.maxInlineSuggestions": 3,
  "tabnine.disable_line_completion": false,
  "tabnine.ignore_all_comments": false,
  "tabnine.auto_import": true
}
```

### Enable/Disable Cloud Mode

**For Privacy (Local Mode):**
```json
{
  "tabnine.cloud_enabled": false
}
```

**For Better Suggestions (Cloud Mode):**
```json
{
  "tabnine.cloud_enabled": true
}
```

---

## Verification

### Check Installation

1. **Status Bar**
   - Look for Tabnine icon in your editor's status bar

2. **Test Completion**
   - Start typing code
   - Tabnine suggestions should appear

3. **Commands**
   - Open command palette
   - Type "Tabnine" to see available commands

---

## Troubleshooting

### Common Issues

**Suggestions not appearing:**
- Check if extension is enabled
- Try disabling other AI extensions
- Restart your editor

**Slow performance:**
- Reduce `maxInlineSuggestions` to 1
- Disable cloud mode for local processing

**Conflicts with other tools:**
- Check extension order in settings
- Disable conflicting extensions

---

## Uninstallation

### VS Code
1. Go to Extensions
2. Find Tabnine
3. Click Uninstall

### JetBrains
1. Go to Settings → Plugins
2. Find Tabnine
3. Click Uninstall

---

*Back to [08-Coding README](../README.md)*
*Back to [Main README](../../README.md)*