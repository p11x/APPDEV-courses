# Kilo Code Installation Guide

## Installation Methods

### Method 1: VS Code Marketplace (Recommended)

#### Step 1: Install the Extension

1. Open VS Code
2. Navigate to Extensions panel (Ctrl+Shift+X / Cmd+Shift+X)
3. Search for "Kilo Code"
4. Click Install on "Kilo Code - AI Coding Assistant"

#### Step 2: Create Account

1. After installation, click the Kilo Code icon in sidebar
2. Sign up for a free account
3. Verify email address
4. Log in to activate features

#### Step 3: Initial Setup

1. Accept terms and conditions
2. Choose preferences (optional)
3. Kilo Code is ready to use

### Method 2: VSIX Installation

For offline installation:

1. Download VSIX from official website
2. In VS Code, go to Extensions panel
3. Click "..." menu
4. Select "Install from VSIX"
5. Choose downloaded file

## Verification

### Check Installation

1. Look for Kilo Code icon in VS Code sidebar
2. Open Command Palette (Ctrl+Shift+P / Cmd+Shift+P)
3. Type "Kilo Code: Hello" to verify

### First-Time Use

1. Open any code file
2. Start typing code
3. Kilo Code will suggest completions

## Configuration

### Settings Access

1. File > Preferences > Settings
2. Search for "Kilo Code"
3. Configure options

### Key Settings

| Setting | Description | Default |
|---------|-------------|---------|
| Enable | Toggle extension on/off | Enabled |
| Inline Suggest | Show inline completions | Enabled |
| Suggest Mode | Preview or immediate | Preview |

### Recommended Settings

```json
{
  "kilocode.enable": true,
  "kilocode.inlineSuggest": true,
  "kilocode.suggestPreview": true,
  "kilocode.maxTokens": 500
}
```

## Usage

### Getting Started

1. **Basic Completion**
   - Start typing code
   - Accept suggestions with Tab

2. **Generate Code**
   - Write comment describing desired code
   - Press Tab to accept

3. **Explain Code**
   - Select code
   - Right-click > Kilo Code: Explain

### Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Tab | Accept suggestion |
| Escape | Dismiss suggestion |
| Ctrl+Shift+P | Command palette |

### Using with Different Languages

Kilo Code automatically detects language and provides appropriate suggestions. No manual language selection needed.

## Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| No suggestions | Check internet connection |
| Slow response | Check network latency |
| Extension not loading | Reload VS Code |
| Not signed in | Click Kilo Code icon to login |

### Getting Help

- Website: [kilocode.io](https://kilocode.io)
- Documentation: [docs.kilocode.io](https://docs.kilocode.io)
- Support: [support.kilocode.io](https://support.kilocode.io)