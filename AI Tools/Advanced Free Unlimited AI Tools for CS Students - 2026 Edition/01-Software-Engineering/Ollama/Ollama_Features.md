# Ollama Features

## Core Features

### 1. Local LLM Execution

Ollama runs large language models entirely on your machine:

- No internet required after model download
- Complete privacy for your data
- No API costs or usage limits
- Consistent performance

### 2. Model Management

| Feature | Description |
|---------|-------------|
| Pull models | Download models from library |
| List models | View installed models |
| Remove models | Delete unused models |
| Run models | Execute models interactively |

### 3. Interactive Chat

Ollama provides an interactive chat interface:

```
$ ollama run llama3.2
>>> Explain Big O notation

Big O notation is a mathematical notation that describes...
```

### 4. API Server

Built-in REST API for integration:

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/generate` | POST | Generate completion |
| `/api/chat` | POST | Chat completion |
| `/api/tags` | GET | List available models |

## Model Features

### 5. Multiple Model Support

Ollama supports various model architectures:

| Model Family | Variants | Specialty |
|--------------|----------|-----------|
| Llama 3.2 | 1B, 3B, 7B, 70B | General purpose |
| Mistral | 7B | Balanced performance |
| Codellama | 7B, 13B, 70B | Code generation |
| Phi | 2.7B, 3.8B | Lightweight |
| Qwen | 0.5B - 72B | Multilingual |
| DeepSeek | 6.7B, 67B | Coding focus |

### 6. Model Quantization

Reduce model size with quantization:

| Type | Size Reduction | Quality |
|------|----------------|---------|
| Q4_0 | ~50% | Good |
| Q5_1 | ~40% | Very Good |
| Q8_0 | ~20% | Excellent |

### 7. Custom Model Import

Import GGUF format models:

```bash
# Import custom model
ollama import ./my-model.gguf

# Create Modelfile for custom model
ollama create mymodel -f Modelfile
```

## Technical Features

### 8. GPU Acceleration

NVIDIA GPU support for faster inference:

- Automatic GPU detection
- Configurable GPU layers
- Efficient memory management
- Mixed CPU/GPU operation

### 9. Context Window

| Model | Context Length |
|-------|----------------|
| Llama 3.2 | 128K |
| Mistral | 8K-32K |
| Codellama | 16K-100K |
| Phi | 4K |

### 10. Streaming Responses

Real-time response streaming:

```python
import requests

response = requests.post(
    'http://localhost:11434/api/generate',
    json={'model': 'llama3.2', 'prompt': 'Hello'},
    stream=True
)

for chunk in response.iter_lines():
    print(chunk)
```

## Integration Features

### 11. REST API

Full-featured API for custom applications:

```python
import requests

# Chat completion
response = requests.post(
    'http://localhost:11434/api/chat',
    json={
        'model': 'llama3.2',
        'messages': [
            {'role': 'user', 'content': 'Hello!'}
        ]
    }
)
print(response.json()['message']['content'])
```

### 12. Command-Line Interface

Powerful CLI for all operations:

| Command | Description |
|---------|-------------|
| `ollama list` | List models |
| `ollama pull` | Download model |
| `ollama run` | Run model |
| `ollama serve` | Start API server |
| `ollama create` | Create custom model |

### 13. Environment Variables

Flexible configuration:

```bash
# Custom port
OLLAMA_HOST=0.0.0.0:11434

# Model directory
OLLAMA_MODELS=/path/to/models

# GPU settings
OLLAMA_GPU_LAYERS=33

# Keep models loaded
OLLAMA_KEEP_ALIVE=-1
```

### 14. Systemd Service

Linux systemd integration:

```bash
# Install as service
sudo systemctl enable ollama
sudo systemctl start ollama

# Check status
systemctl status ollama
```

## Development Features

### 15. GGUF Support

Import GGUF format models:

```bash
# Using Llama.cpp
./main -m model.gguf --prompt "Hello"
```

### 16. Modelfiles

Customize model behavior:

```plaintext
FROM llama3.2
PARAMETER temperature 0.8
SYSTEM You are a helpful coding assistant specialized in Python.
```

### 17. Embeddings

Generate text embeddings:

```bash
curl http://localhost:11434/api/embeddings -d '{
  "model": "nomic-embed-text",
  "prompt": "The quick brown fox"
}'
```

## Advanced Features

### 18. Multi-Modal Models

Support for vision models:

| Model | Capability |
|-------|------------|
| LLaVA | Image understanding |
| BakLLaVA | Image understanding |

### 19. Function Calling

Structured output with function calls:

```json
{
  "model": "llama3.2",
  "tools": [{"type": "function", "function": {...}}]
}
```

### 20. Logging and Monitoring

Debug and monitor operations:

```bash
# View logs (Linux with systemd)
journalctl -u ollama -f

# Environment info
ollama info
```

## Productivity Features

### 21. Prompt Caching

Efficient caching for repeated prompts:

- Reduce response time
- Save computation
- Lower resource usage

### 22. Batch Processing

Process multiple prompts:

```bash
# Process file of prompts
for prompt in $(cat prompts.txt); do
  ollama run llama3.2 "$prompt"
done
```

### 23. System Tray (macOS)

Background operation:

- Quick access menu
- Status indicator
- Easy model switching