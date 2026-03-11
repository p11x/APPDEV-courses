# Cline Installation Guide

## Installation Methods

### Method 1: VS Code Marketplace (Recommended)

#### Step 1: Install the Extension

1. Open VS Code
2. Navigate to Extensions panel (Ctrl+Shift+X / Cmd+Shift+X)
3. Search for "Cline"
4. Click Install on "Cline - AI coding assistant"

#### Step 2: Get Anthropic API Key

1. Visit [console.anthropic.com](https://console.anthropic.com)
2. Sign up for a free account
3. Navigate to API Keys section
4. Create a new API key
5. Copy the key (shown only once)

#### Step 3: Configure Cline

1. Open VS Code settings (File > Preferences > Settings)
2. Search for "Cline"
3. Find "Cline: API Key" setting
4. Paste your Anthropic API key
5. Or use Command Palette: Ctrl+Shift+P / Cmd+Shift+P
6. Type "Cline: Enter API Key" and paste your key

### Method 2: VSIX Installation

For offline installation:

1. Download VSIX from [GitHub releases](https://github.com/saoudrizwan/claude-dev/releases)
2. In VS Code, go to Extensions panel
3. Click "..." menu > "Install from VSIX"
4. Select the downloaded .vsix file
5. Configure API key as above

### Method 3: CLI Installation (Advanced)

#### macOS/Linux

```bash
# Install via Homebrew
brew install --cask claude-dev

# Or download binary
curl -L -o ~/cline https://github.com/saoudrizwan/claude-dev/releases/latest/download/claude-dev
chmod +x ~/cline
```

#### Windows

```powershell
# Using winget
winget install Saudrizwan.ClaudeDev

# Or download from GitHub releases
```

## Configuration

### Setting Up API Key via Environment Variable

```bash
# macOS/Linux
export ANTHROPIC_API_KEY="your-api-key-here"

# Windows (PowerShell)
$env:ANTHROPIC_API_KEY="your-api-key-here"

# Windows (CMD)
set ANTHROPIC_API_KEY=your-api-key-here
```

### VS Code Settings Configuration

Create or edit `.vscode/settings.json`:

```json
{
  "cline.apiKey": "${env:ANTHROPIC_API_KEY}",
  "cline.autoApproval": false,
  "cline.maxTokens": 4096,
  "cline.model": "claude-sonnet-4-20250514"
}
```

### Model Selection

Cline supports multiple models:

| Model | Setting Value | Best For |
|-------|---------------|----------|
| Claude 3.5 Sonnet | `claude-sonnet-4-20250514` | Balanced (default) |
| Claude 3 Opus | `claude-opus-4-20250514` | Complex reasoning |
| Claude 3 Haiku | `claude-haiku-3-20250514` | Fast responses |

### Configuring Self-Hosted Models

To use with Ollama or other local models:

```json
{
  "cline.customModelEndpoint": "http://localhost:11434/v1/chat/completions",
  "cline.customModelName": "llama3"
}
```

## Verification

### Check Installation

1. Look for Cline icon in VS Code sidebar
2. Open Command Palette
3. Type "Cline: Hello" to verify setup

### First-Time Setup

1. Open a project folder
2. Click Cline icon in sidebar
3. Enter your development task
4. Cline will initialize and begin working

## Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| API key not working | Verify key in Anthropic console; check for typos |
| Extension not loading | Reload VS Code; check developer console |
| No responses | Check internet connection; verify API credits |
| Rate limiting | Wait and retry; reduce request frequency |

### Getting Help

- GitHub Issues: [github.com/saoudrizwan/claude-dev/issues](https://github.com/saoudrizwan/claude-dev/issues)
- Discord: [discord.gg/claude-dev](https://discord.gg/claude-dev)
- Documentation: [docs.cline.bot](https://docs.cline.bot)