# Text Editors and IDEs

## What You'll Learn

- The difference between text editors and IDEs
- Popular options for Python development
- Choosing the right tool for your needs
- Essential editor features
- Extensions and customization

## Prerequisites

- Completed `03-shell-basics-bash-zsh.md`

## Text Editor vs IDE

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    TEXT EDITOR VS IDE                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  TEXT EDITOR:                                                               │
│  • Lightweight, fast                                                        │
│  • Edit text files                                                          │
│  • Basic features (syntax highlighting, search)                            │
│  • Examples: VS Code, Sublime, Vim, Emacs                                 │
│                                                                             │
│  IDE (Integrated Development Environment):                                  │
│  • Full development environment                                            │
│  • Built-in debugger, testing, profiling                                   │
│  • Project management                                                      │
│  • Examples: PyCharm, IntelliJ                                             │
│                                                                             │
│  FOR PYTHON WEB DEV:                                                        │
│  • VS Code (editor with extensions) ← Recommended                        │
│  • PyCharm (full IDE)                                                      │
│  • Sublime Text (lightweight editor)                                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## VS Code (Recommended)

The most popular choice for Python development:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    VS CODE FOR PYTHON                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  WHY VS CODE:                                                               │
│  ✅ Free and open source                                                   │
│  ✅ Excellent Python support via extensions                               │
│  ✅ Lightweight but powerful                                               │
│  ✅ Great community and extensions                                        │
│  ✅ Works on Windows, Mac, Linux                                          │
│  ✅ Built-in terminal                                                      │
│  ✅ Git integration                                                       │
│  ✅ Remote development (SSH, containers)                                  │
│                                                                             │
│  KEY EXTENSIONS:                                                            │
│  • Python (Microsoft)                                                     │
│  • Pylance (type checking)                                                │
│  • Black (formatting)                                                      │
│  • Flake8 (linting)                                                       │
│  • GitLens (Git integration)                                              │
│  • Docker                                                                 │
│  • Remote - SSH                                                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Setting Up VS Code for Python

```bash
# Install VS Code
# Download from code.visualstudio.com

# Install Python extension
# 1. Open VS Code
# 2. Ctrl+P / Cmd+P
# 3. Type: ext install ms-python.python

# Set Python interpreter
# 1. Ctrl+Shift+P / Cmd+Shift+P
# 2. Type: Python: Select Interpreter
# 3. Choose your Python
```

### VS Code Settings

```json
{
    "python.linting.enabled": true,
    "python.linting.pylintEnabled": false,
    "python.linting.flake8Enabled": true,
    "python.formatting.provider": "black",
    "editor.formatOnSave": true,
    "editor.rulers": [88],
    "files.trimTrailingWhitespace": true
}
```

## PyCharm

A full IDE for Python:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PYCHARM OPTIONS                                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  PYCHARM COMMUNITY (Free):                                                 │
│  • Code editing, navigation                                               │
│  • Code completion and analysis                                           │
│  • Refactoring                                                             │
│  • Git integration                                                         │
│  • Debugger                                                                │
│  • Testing support                                                         │
│                                                                             │
│  PYCHARM PROFESSIONAL (Paid):                                              │
│  • Everything in Community                                                │
│  • Django support                                                          │
│  • Flask support                                                           │
│  • Database tools                                                         │
│  • Remote debugging                                                        │
│  • Profiling                                                               │
│                                                                             │
│  BEST FOR:                                                                  │
│  • Large projects                                                          │
│  • When you want everything included                                      │
│  • Deep framework support                                                  │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### PyCharm Features

| Feature | VS Code | PyCharm |
|---------|---------|---------|
| Python debugging | ✅ | ✅ |
| Code completion | ✅ | ✅ |
| Refactoring | Basic | Advanced |
| Framework support | Extensions | Built-in |
| Database tools | Extensions | Built-in |
| Memory profiling | No | ✅ |
| Price | Free | Paid (Pro) |

## Sublime Text

A lightweight, fast editor:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SUBLIME TEXT                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  PROS:                                                                     │
│  • Extremely fast startup                                                  │
│  • Lightweight                                                             │
│  • Great for large files                                                  │
│  • "Goto Anything" (Ctrl+P)                                                │
│  • Multiple selections                                                     │
│  • Powerful snippets                                                       │
│                                                                             │
│  CONS:                                                                     │
│  • Less Python-specific features                                          │
│  • Paid (but unlimited trial)                                             │
│  • Requires plugins for Python debugging                                  │
│                                                                             │
│  KEY PACKAGES:                                                             │
│  • SublimeLinter                                                           │
│  • Python Improved                                                         │
│  • Anaconda (Python IDE features)                                         │
│  • GitGutter                                                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Vim/Neovim

For keyboard-focused developers:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    VIM / NEOVIM                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  PROS:                                                                     │
│  • Works in terminal (no GUI needed)                                       │
│  • Extremely efficient with keyboard                                      │
│  • Highly customizable                                                     │
│  • Built-in features (split, tabs, regex)                                 │
│  • Available everywhere                                                    │
│                                                                             │
│  CONS:                                                                     │
│  • Steep learning curve                                                    │
│  • Complex configuration                                                   │
│  • Less out-of-box Python support                                         │
│                                                                             │
│  KEY CONCEPTS:                                                              │
│  • Modes: Normal, Insert, Visual, Command                                  │
│  • Commands: dw, ci", gg, G, yy, pp                                       │
│  • Plugins: vim-plug, pathogen                                            │
│                                                                             │
│  FOR PYTHON:                                                               │
│  • coc.nvim (LSP)                                                          │
│  • vim-python                                                          │
│  •ale (linting)                                                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Comparison Table

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    CHOOSING YOUR EDITOR                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  BEGINNER:                                                                  │
│  → VS Code (easy to learn, great Python support)                          │
│                                                                             │
│  INTERMEDIATE:                                                              │
│  → VS Code (customize with extensions)                                   │
│  → PyCharm (full-featured)                                                │
│                                                                             │
│  ADVANCED / SERVER WORK:                                                   │
│  → Vim/Neovim (terminal-based)                                            │
│                                                                             │
│  PERFORMANCE-FOCUSED:                                                      │
│  → Sublime Text (fastest)                                                 │
│                                                                             │
│  LARGE PROJECTS:                                                           │
│  → PyCharm (best refactoring)                                            │
│                                                                             │
│  WEB DEV (Full Stack):                                                     │
│  → VS Code (great for all languages)                                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Essential Editor Features

Regardless of which editor you choose:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    MUST-HAVE FEATURES                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  CODE QUALITY:                                                              │
│  ✅ Syntax highlighting                                                    │
│  ✅ Auto-indentation                                                       │
│  ✅ Code completion                                                        │
│  ✅ Linting (error highlighting)                                           │
│  ✅ Formatting (Black, autopep8)                                          │
│                                                                             │
│  NAVIGATION:                                                                │
│  ✅ Go to definition                                                       │
│  ✅ Find references                                                        │
│  ✅ Search in files                                                       │
│  ✅ File tree/sidebar                                                      │
│                                                                             │
│  PRODUCTIVITY:                                                              │
│  ✅ Multiple cursors                                                       │
│  ✅ Snippets                                                               │
│  ✅ Terminal integration                                                  │
│  ✅ Git integration                                                        │
│                                                                             │
│  DEBUGGING:                                                                 │
│  ✅ Breakpoints                                                            │
│  ✅ Step through                                                          │
│  ✅ Variable inspection                                                    │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- VS Code is the best balance for most Python developers
- PyCharm offers the most features for large projects
- Sublime is great for speed
- Vim/Neovim for terminal-only work
- Choose based on your needs and workflow

## Next Steps

→ Continue to `05-package-managers.md` to learn about installing and managing Python packages.
