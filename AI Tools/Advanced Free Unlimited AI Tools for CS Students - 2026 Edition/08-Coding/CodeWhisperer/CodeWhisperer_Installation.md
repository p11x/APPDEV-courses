# CodeWhisperer - Installation

## Installation Methods

### Method 1: VS Code (Recommended)

1. **Open VS Code**
   - Launch Visual Studio Code on your system

2. **Access Extensions**
   - Click the Extensions icon in the Activity Bar
   - Or press `Ctrl+Shift+X` (Windows/Linux) / `Cmd+Shift+X` (macOS)

3. **Search for CodeWhisperer**
   - Search "AWS Toolkit" or "CodeWhisperer"
   - Look for "AWS Toolkit for VS Code" by Amazon

4. **Install Extension**
   - Click the "Install" button
   - Wait for installation to complete

5. **Restart VS Code**
   - Reload VS Code to activate the extension

### Method 2: JetBrains IDEs

1. **Open IDE**
   - Launch IntelliJ IDEA, PyCharm, WebStorm, or other JetBrains IDE

2. **Access Plugins**
   - Go to `File` → `Settings` (Windows/Linux) or `Preferences` (macOS)
   - Click on "Plugins"

3. **Search and Install**
   - Search for "AWS Toolkit"
   - Click "Install"

4. **Restart IDE**
   - Restart your JetBrains IDE

### Method 3: Visual Studio

1. **Open Visual Studio**
   - Launch Visual Studio 2022 or later

2. **Access Extensions**
   - Go to `Extensions` → `Manage Extensions`

3. **Search and Install**
   - Search for "AWS Toolkit for Visual Studio"
   - Click "Install"

: AWS Cloud9### Method 4

1. **Open AWS Console**
   - Navigate to AWS Cloud9 service

2. **Create/Open Environment**
   - Create a new or open existing Cloud9 environment

3. **CodeWhisperer Pre-installed**
   - AWS Cloud9 comes with CodeWhisperer pre-installed
   - Simply enable it in preferences

---

## Account Setup

### For Students (Free Individual Tier)

1. **No AWS Account Required**
   - CodeWhisperer Individual Tier is free
   - Just install the extension and start using

2. **Optional: AWS Builder ID**
   - Visit: https://aws.amazon.com/codewhisperer/
   - Click "Get Started"
   - Choose "Use for personal or educational use"
   - Sign in with email (no AWS account needed)

### First-Time Setup

1. **After Installation**
   - Open VS Code or your IDE
   - Look for the AWS icon in the status bar

2. **Enable CodeWhisperer**
   - Open Command Palette: `Ctrl+Shift+P`
   - Type "AWS: Enable CodeWhisperer"
   - Press Enter

3. **Start Using**
   - Begin typing code
   - Suggestions will appear automatically

---

## Configuration

### Enable/Disable CodeWhisperer

**VS Code:**
- Open Command Palette
- Type "AWS: Enable/Disable CodeWhisperer"

**JetBrains:**
- Go to Settings → AWS → CodeWhisperer
- Toggle enable/disable

### Settings

```json
{
  "aws.codewhisperer.enabled": true,
  "aws.codewhisperer.autoSuggestion": "enabled",
  "aws.codewhisperer.shareCodeWhispererContentWithAWS": false,
  "aws.codewhisperer.pseudonymizedTelemetry": false
}
```

### Language Selection

```json
{
  "aws.codewhisperer.supportedLanguages": [
    "python",
    "javascript",
    "typescript",
    "java",
    "csharp",
    "go",
    "rust"
  ]
}
```

---

## Verification

### Check Installation

1. **Look for AWS Icon**
   - AWS icon in VS Code status bar (left side)

2. **Test CodeWhisperer**
   - Type a comment describing a function
   - Wait for suggestion to appear

3. **Open CodeWhisperer Panel**
   - Press `Ctrl+Shift+P`
   - Type "AWS: Open CodeWhisperer"

---

## Troubleshooting

### Common Issues

**No suggestions appearing:**
- Check if CodeWhisperer is enabled
- Ensure internet connection
- Try restarting IDE

**Extension not loading:**
- Check VS Code version (1.70+ required)
- Try reinstalling extension
- Check for conflicts with other extensions

---

## Uninstallation

### VS Code
1. Go to Extensions
2. Find AWS Toolkit
3. Click Uninstall

### JetBrains
1. Go to Settings → Plugins
2. Find AWS Toolkit
3. Click Uninstall

---

## Quick Start Checklist

- [ ] Install AWS Toolkit extension
- [ ] Restart IDE
- [ ] Enable CodeWhisperer
- [ ] Start typing code
- [ ] Accept suggestions with Tab
- [ ] Enjoy faster coding!

---

*Back to [08-Coding README](../README.md)*
*Back to [Main README](../../README.md)*