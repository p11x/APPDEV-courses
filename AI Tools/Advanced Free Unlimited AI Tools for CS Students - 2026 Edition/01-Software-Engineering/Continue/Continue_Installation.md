# Continue - Installation Guide

## Installation Methods

### Method 1: VS Code (Recommended)

#### Step 1: Open VS Code

Launch Visual Studio Code on your system.

#### Step 2: Access Extensions

Navigate to the Extensions view:
- **Windows/Linux**: `Ctrl+Shift+X`
- **macOS**: `Cmd+Shift+X`

#### Step 3: Search for Continue

In the search bar, type: `Continue`

#### Step 4: Install Extension

1. Find "Continue" in the results
2. Click the "Install" button
3. Wait for installation to complete
4. Restart VS Code when prompted

#### Alternative: Command Line Installation

```bash
# Install via VS Code CLI
code --install-extension continue.continue
```

---

### Method 2: JetBrains IDEs

#### Step 1: Open Settings

- **Windows/Linux**: `File > Settings`
- **macOS**: `Code > Preferences > Settings`

#### Step 2: Navigate to Plugins

1. Go to "Plugins" tab
2. Click "Marketplace"
3. Search for "Continue"

#### Step 3: Install and Restart

1. Click "Install"
2. Restart your IDE

---

## Configuration

### Basic Configuration

Create or edit `~/.continue/config.json`:

```json
{
  "models": [
    {
      "provider": "ollama",
      "model": "llama3"
    }
  ]
}
```

### Advanced Configuration

```json
{
  "models": [
    {
      "provider": "openai",
      "model": "gpt-4",
      "api_key": "your-api-key"
    }
  ],
  "allowAnonymousTelemetry": true
}
```

### Provider Configuration

#### Ollama
```json
{
  "provider": "ollama",
  "model": "llama3",
  "api_base": "http://localhost:11434"
}
```

#### Anthropic Claude
```json
{
  "provider": "anthropic",
  "model": "claude-3-opus",
  "api_key": "your-api-key"
}
```

---

## Troubleshooting

### Issue: Extension Not Installing

**Symptoms**: Installation fails or shows error

**Solutions**:

1. **Check Internet Connection**
   ```bash
   ping continue.dev
   ```

2. **Check VS Code Version**
   - Ensure VS Code is version 1.65 or higher

3. **Clear Cache**
   ```bash
   # Close VS Code
   # Delete ~/.continue folder
   # Reopen VS Code
   ```

---

### Issue: Not Connecting to Ollama

**Symptoms**: Can't connect to local model

**Solutions**:

1. **Start Ollama**
   ```bash
   ollama serve
   ```

2. **Check Port**
   - Default: http://localhost:11434

3. **Verify Model**
   ```bash
   ollama list
   ```

---

### Issue: API Key Not Working

**Symptoms**: Authentication errors

**Solutions**:

1. **Verify API Key**
   - Check key is correct in config.json
   - Ensure no extra spaces or quotes

2. **Check Provider**
   - Verify provider name is correct

---

## Verification

### Check Installation Status

```bash
# VS Code - Check extensions
code --list-extensions | grep continue
```

### Test Configuration

1. Open VS Code
2. Open Continue panel (Ctrl+L or Cmd+L)
3. Send a message
4. Verify response

---

*Back to [Continue README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*