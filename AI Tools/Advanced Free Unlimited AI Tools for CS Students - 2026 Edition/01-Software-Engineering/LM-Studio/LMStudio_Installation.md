# LM Studio Installation Guide

## Installation Methods

### Method 1: Direct Download (Recommended)

#### Windows

1. Visit [lmstudio.ai](https://lmstudio.ai)
2. Click "Download for Windows"
3. Run the downloaded `.exe` file
4. Follow the installation wizard
5. Launch LM Studio from Start Menu

#### macOS

1. Visit [lmstudio.ai](https://lmstudio.ai)
2. Click "Download for macOS"
3. Open the `.dmg` file
4. Drag LM Studio to Applications folder
5. Launch from Applications

#### Linux

1. Visit [lmstudio.ai](https://lmstudio.ai)
2. Click "Download for Linux"
3. Run the `.AppImage` or `.deb` package
4. Make executable: `chmod +x LM-Studio-*.AppImage`
5. Run: `./LM-Studio-*.AppImage`

### Method 2: Package Managers

#### Windows (winget)

```powershell
winget install LMStudio.LMStudio
```

#### macOS (Homebrew)

```bash
# Not available via Homebrew yet
# Use direct download instead
```

#### Linux (Flatpak)

```bash
# Add Flathub repository
flatpak remote-add --if-not-exists flathub https://flathub.org/repo/flathub.flatpakrepo

# Install
flatpak install flathub ai.lmstudio.LMStudio
```

## First Launch Setup

### Initial Configuration

1. Launch LM Studio
2. Wait for initial setup to complete
3. The app will check for GPU acceleration

### GPU Configuration

LM Studio automatically detects GPU:

| GPU Type | Automatic Detection |
|----------|---------------------|
| NVIDIA | Shows CUDA availability |
| Apple Silicon | Shows Metal support |
| None | Runs in CPU mode |

## Downloading Models

### Using Built-in Model Browser

1. Click the "Discover" icon in sidebar
2. Browse or search for models
3. Click "Download" on desired model
4. Wait for download to complete

### Manual Model Loading

1. Click "Add Model" button
2. Navigate to downloaded GGUF file
3. Select file to load

### Recommended Models for Students

| Model | Size | Use Case |
|-------|------|----------|
| Phi 3 Mini | 2 GB | Learning, testing |
| Llama 3.2 1B | 1.3 GB | Fast responses |
| Llama 3.2 3B | 2 GB | Balanced |
| Mistral 7B | 4 GB | General purpose |

## Basic Usage

### Starting a Chat

1. Click "Chat" in sidebar
2. Select a model from dropdown
3. Wait for model to load
4. Start typing messages

### Model Parameters

Adjust in settings:

| Parameter | Range | Description |
|-----------|-------|-------------|
| Temperature | 0-2 | Creativity level |
| Max Tokens | 1-8192 | Response length |
| Top P | 0-1 | Sampling threshold |
| Context | 1024-128K | Memory size |

## API Server

### Starting Local Server

1. Click "Server" in sidebar
2. Click "Start Server"
3. Note the port (default: 1234)
4. Use with external applications

### API Endpoint

```bash
# Example curl request
curl http://localhost:1234/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{
    "model": "llama-3.2-3b-instruct",
    "messages": [{"role": "user", "content": "Hello"}]
  }'
```

## Configuration

### Settings Location

- Windows: `%APPDATA%\LM Studio\config.yaml`
- macOS: `~/Library/Application Support/lm-studio/config.yaml`
- Linux: `~/.config/lm-studio/config.yaml`

### Common Settings

```yaml
# Example config
model_directory: ~/lm-studio/models
default_temperature: 0.7
default_max_tokens: 2048
context_length: 4096
gpu_offload: 1
```

## Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| Model won't load | Check available RAM |
| GPU not detected | Update GPU drivers |
| Slow responses | Use smaller model |
| Download fails | Check internet connection |

### GPU Acceleration Not Working

1. Check GPU is detected in settings
2. Update NVIDIA drivers or macOS
3. Try CPU-only mode

### Out of Memory

- Use smaller model
- Reduce context length
- Close other applications

## Getting Help

- Discord: [discord.gg/lmstudio](https://discord.gg/lmstudio)
- GitHub: [github.com/lmstudio-ai/lmstudio](https://github.com/lmstudio-ai/lmstudio)
- Website: [lmstudio.ai](https://lmstudio.ai)