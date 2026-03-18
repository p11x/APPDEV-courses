# Terminal Emulators

## What You'll Learn

- Why your terminal choice matters
- Comparing popular terminal emulators
- Features to look for
- Customization options
- Windows, Mac, and Linux options

## Prerequisites

- Completed `01-command-line-basics.md`

## What Is a Terminal Emulator?

A terminal emulator is the application that provides your command line interface:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    TERMINAL VS SHELL                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  TERMINAL EMULATOR (the app):                                               │
│  • The window you type in                                                  │
│  • Handles display, fonts, colors                                          │
│  • Provides tabs, splits, hotkeys                                         │
│  Examples: iTerm2, Windows Terminal, Alacritty                           │
│                                                                             │
│  SHELL (the program running):                                              │
│  • Interprets your commands                                                │
│  • Runs programs                                                            │
│  • Handles scripting                                                       │
│  Examples: bash, zsh, fish, PowerShell                                    │
│                                                                             │
│  Think: Terminal = window, Shell = engine                                 │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Windows Options

### Windows Terminal (Recommended)

Microsoft's modern terminal:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WINDOWS TERMINAL FEATURES                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ Free and open source                                                    │
│  ✅ Modern UI with tabs                                                    │
│  ✅ GPU-accelerated rendering                                              │
│  ✅ Supports PowerShell, CMD, WSL, SSH                                   │
│  ✅ Customizable themes and fonts                                          │
│  ✅ Split panes                                                            │
│  ✅ Rich text support                                                       │
│                                                                             │
│  Install: Microsoft Store or GitHub                                        │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Other Windows Options

| Terminal | Best For |
|----------|----------|
| Windows Terminal | Modern features, performance |
| Hyper | Plugin ecosystem, web tech |
| ConEmu | Feature-rich, familiar |
| Cmder | Portable, Linux-style |

## macOS Options

### iTerm2 (Recommended)

The standard for macOS developers:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    ITERM2 FEATURES                                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ Split panes (horizontal/vertical)                                     │
│  ✅ Search with regex                                                      │
│  ✅ Copy mode with vim keybindings                                        │
│  ✅ Triggered (timed) actions                                              │
│  ✅ Semantic history (clickable output)                                   │
│  ✅ Instant replay (replay terminal)                                     │
│  ✅ Profile configurations                                                 │
│  ✅ Touch Bar support                                                      │
│                                                                             │
│  Install: iterm2.com                                                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Other macOS Options

| Terminal | Best For |
|----------|----------|
| iTerm2 | Full-featured, professional |
| Hyper | Web technologies, plugins |
| Alacritty | Performance, simplicity |
| Terminal.app | Built-in, simple |

## Linux Options

### Popular Choices

| Terminal | Best For |
|----------|----------|
| GNOME Terminal | Default GNOME |
| Konsole | KDE default, feature-rich |
| Alacritty | Performance |
| kitty | GPU rendering, extensibility |
| Tilix | Tiling, split panes |

## Cross-Platform Options

### Alacritty

A modern, cross-platform terminal:

```bash
# Installation
# macOS
brew install alacritty

# Linux
cargo install alacritty

# Features:
# • Blazing fast (GPU rendering)
# • Simple, minimal UI
# • Cross-platform
# • YAML configuration
```

### Hyper

Built on web technologies:

```bash
# macOS
brew install hyper

# Features:
# • Plugins (JavaScript)
# • Themes
# • Built on Electron
# • Highly customizable
```

### Warp

A new AI-powered terminal:

```bash
# Install
# Download from warp.dev

# Features:
# • AI-powered command completion
# • Blocks (structured output)
# • Built-in workflows
# • Modern UI
```

## Features Comparison

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    FEATURES TO LOOK FOR                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ESSENTIAL:                                                                 │
│  ✅ Tabs or workspaces                                                     │
│  ✅ Split panes                                                            │
│  ✅ Search                                                                 │
│  ✅ Copy/paste support                                                     │
│  ✅ Unicode/UTF-8 support                                                  │
│                                                                             │
│  IMPORTANT:                                                                 │
│  ✅ Customizable fonts and colors                                         │
│  ✅ Hotkey support                                                         │
│  ✅ Transparency/blur (for overlay)                                       │
│  ✅ Multiple profiles                                                      │
│  ✅ SSH support                                                            │
│                                                                             │
│  NICE TO HAVE:                                                              │
│  • GPU acceleration                                                        │
│  • Plugin ecosystem                                                        │
│  • Command completion                                                      │
│  • AI features                                                            │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Customization

### Fonts

Use a good monospace font:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    RECOMMENDED FONTS                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Programmer Fonts:                                                          │
│  • JetBrains Mono — Great readability, ligatures                          │
│  • Fira Code — Popular, ligatures                                          │
│  • Cascadia Code — Microsoft, ligatures                                    │
│  • Source Code Pro — Adobe                                                 │
│  • Hack — Classic choice                                                   │
│                                                                             │
│  Why Monospace?                                                            │
│  • Every character same width                                             │
│  • Easy to read code                                                       │
│  • Aligns columns                                                          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Colors

Most terminals support 256 colors or true color (24-bit):

```json
// Alacritty config (alacritty.yml)
colors:
  primary:
    background: '#1e1e1e'
    foreground: '#d4d4d4'
  cursor:
    cursor: '#ffffff'
    
font:
  normal:
    family: "JetBrains Mono"
  size: 12
```

### Profiles

Save different configurations for different uses:

- Development work
- Server administration
- General use

## Performance

For large projects or long-running sessions, performance matters:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PERFORMANCE TIPS                                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  GPU vs Software Rendering:                                                 │
│  • GPU-accelerated terminals are faster                                    │
│  • Alacritty, iTerm2, Windows Terminal use GPU                           │
│                                                                             │
│  If terminal is slow:                                                       │
│  • Try a different terminal                                                │
│  • Reduce scrollback history                                               │
│  • Disable transparency                                                     │
│  • Check for too many plugins                                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- Choose a modern terminal with the features you need
- Windows: Windows Terminal
- macOS: iTerm2
- Linux: Alacritty, Konsole, or your distro's default
- Consider GPU acceleration for performance
- Customize fonts and colors for comfort

## Next Steps

→ Continue to `03-shell-basics-bash-zsh.md` to learn about the shell programs that run inside your terminal.
