# Ollama Installation Guide

## Installation Methods

### Method 1: macOS (Recommended)

#### Option A: Direct Download

1. Visit [ollama.com/download](https://ollama.com/download)
2. Download the macOS installer
3. Open the downloaded file
4. Follow the installation wizard
5. Launch Ollama from Applications

#### Option B: Homebrew

```bash
# Install Homebrew (if not installed)
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Install Ollama
brew install ollama
```

### Method 2: Linux

#### Option A: Installation Script (Recommended)

```bash
# Download and install
curl -fsSL https://ollama.com/install.sh | sh
```

#### Option B: Manual Installation

```bash
# Download the binary
sudo curl -L https://ollama.com/linux/ollama -o /usr/local/bin/ollama

# Make it executable
sudo chmod +x /usr/local/bin/ollama

# Start the service (optional, for systemd)
sudo systemctl enable ollama
```

#### Option C: Docker

```bash
# Pull the
docker pull o imagellama/ollama

# Run with GPU support
docker run --gpus all -v ollama:/root/.ollama -p 11434:11434 ollama/ollama

# Run CPU only
docker run -v ollama:/root/.ollama -p 11434:11434 ollama/ollama
```

### Method 3: Windows

#### Option A: WSL2 (Recommended)

1. Install WSL2:
```powershell
wsl --install
```

2. Then follow Linux installation in WSL2

#### Option B: Direct Download (Preview)

1. Visit [ollama.com/download](https://ollama.com/download)
2. Download Windows preview version
3. Run the installer
4. Launch Ollama from Start Menu

## Verifying Installation

### Check Installation

```bash
# Verify Ollama is installed
ollama --version

# Expected output: ollama version 0.x.x
```

### Start Ollama Service

```bash
# Start the Ollama service
ollama serve

# In another terminal, verify it's running
ollama list
```

## Downloading Models

### First Model Download

```bash
# Download a small model (recommended for beginners)
ollama pull phi3

# Or download Llama 3.2 (recommended for coding)
ollama pull llama3.2

# Check available models
ollama list
```

### Popular Models

| Model | Command | Size | Best For |
|-------|---------|------|----------|
| Phi 3 | `ollama pull phi3` | 2.4 GB | Lightweight tasks |
| Llama 3.2 | `ollama pull llama3.2` | 1.3 GB | General purpose |
| Mistral | `ollama pull mistral` | 4.1 GB | Balanced |
| Codellama | `ollama pull codellama` | 3.8 GB | Code generation |

### Model Tags

```bash
# List available tags for a model
ollama show llama3.2

# Pull specific version
ollama pull llama3.2:1b
ollama pull llama3.2:3b
```

## Basic Usage

### Running Models

```bash
# Run interactively
ollama run llama3.2

# Run with prompt
ollama run llama3.2 "Explain recursion in programming"

# Run specific model
ollama run phi3 "Write a Python function for binary search"
```

### Using the API

```bash
# Generate endpoint
curl http://localhost:11434/api/generate -d '{
  "model": "llama3.2",
  "prompt": "Hello!",
  "stream": false
}'

# Chat endpoint
curl http://localhost:11434/api/chat -d '{
  "model": "llama3.2",
  "messages": [
    { "role": "user", "content": "Hello!" }
  ]
}'
```

## Configuration

### Environment Variables

```bash
# Set custom model directory
export OLLAMA_MODELS=/path/to/models

# Set host (for remote access)
export OLLAMA_HOST=0.0.0.0:11434

# GPU offload (0-1)
export OLLAMA_GPU_OVERHEAD=0
```

### Configuration File

Create `~/.ollama/config.yaml`:

```yaml
host: 127.0.0.1:11434
models: ~/.ollama/models
```

## VS Code Integration

### Using with Continue Extension

1. Install Continue extension in VS Code
2. Configure to use Ollama:
   - Open Continue settings
   - Select "Ollama" as provider
   - Choose model (e.g., llama3.2, codellama)

### Using with CodeGPT or Other Extensions

Many VS Code AI extensions support Ollama:

1. Install your preferred extension
2. Set endpoint to `http://localhost:11434`
3. Select model from Ollama

## Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| "command not found" | Add to PATH or restart terminal |
| GPU not detected | Install CUDA drivers |
| Slow performance | Use smaller model or enable GPU |
| Port in use | Change port in config |

### Getting Help

- GitHub Issues: [github.com/ollama/ollama/issues](https://github.com/ollama/ollama/issues)
- Discord: [discord.gg/ollama](https://discord.gg/ollama)
- Documentation: [docs.ollama.ai](https://docs.ollama.ai)