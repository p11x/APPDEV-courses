# Ollama Installation

## Installation Methods

### macOS Installation
```bash
# Using Homebrew (recommended)
brew install ollama

# Or download directly
curl -fsSL https://ollama.com/install.sh | sh
```

### Linux Installation
```bash
# Install via curl
curl -fsSL https://ollama.com/install.sh | sh

# For Ubuntu/Debian, you can also use:
# Download the .deb package from https://github.com/ollama/ollama/releases
sudo dpkg -i ollama_amd64.deb
```

### Windows Installation
- Download the installer from [ollama.com/download](https://ollama.com/download)
- Run the installer and follow the prompts
- Ollama will be installed to your system

## Verification
After installation, verify Ollama is working:
```bash
ollama --version
```

## Downloading Models
Once installed, pull your first model:
```bash
# Pull Llama 2 (default model)
ollama pull llama2

# Pull CodeLlama for coding assistance
ollama pull codellama

# Pull Mistral for general tasks
ollama pull mistral
```

## Starting Ollama
```bash
# Start Ollama server (runs in background)
ollama serve

# In another terminal, interact with the model
ollama run llama2
```

## Quick Start for Students

1. Install Ollama using one of the methods above
2. Run `ollama pull llama2` to download the model
3. Run `ollama run llama2` to start chatting
4. For coding help, use CodeLlama: `ollama run codellama`

## Model Storage
- Models are stored in `~/.ollama/models` by default
- You can check available models with `ollama list`
- Remove unused models with `ollama rm <model-name>`