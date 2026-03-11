# Ollama - Functionalities

## Command Reference

### Model Management Commands

| Command | Description | Example |
|---------|-------------|---------|
| `ollama list` | List all installed models | `ollama list` |
| `ollama pull` | Download a model | `ollama pull llama3` |
| `ollama remove` | Delete a model | `ollama remove llama3` |
| `ollama show` | Show model info | `ollama show llama3` |

### Chat Commands

| Command | Description | Example |
|---------|-------------|---------|
| `ollama run` | Start interactive chat | `ollama run llama3` |

### Server Commands

| Command | Description | Example |
|---------|-------------|---------|
| `ollama serve` | Start API server | `ollama serve` |

---

## API Usage

### Chat Completions API

```bash
curl http://localhost:11434/api/chat -d '{
  "model": "llama3",
  "messages": [
    { "role": "user", "content": "Hello!" }
  ]
}'
```

### Response Format

```json
{
  "message": {
    "role": "assistant",
    "content": "Hello! How can I help you today?"
  },
  "done": true
}
```

### Generate API

```bash
curl http://localhost:11434/api/generate -d '{
  "model": "llama3",
  "prompt": "Write a Python function to reverse a string",
  "stream": false
}'
```

---

## Model Options

### Available Models

| Model | Size | Best For |
|-------|------|----------|
| llama3 | 4.7GB | General purpose |
| llama3:8b | 8GB | More capable |
| codellama | 3.8GB | Code generation |
| mistral | 4.1GB | Fast, efficient |
| phi3 | 2.3GB | Lightweight |
| neural-chat | 4.1GB | Conversations |

### Model Parameters

```json
{
  "temperature": 0.8,
  "top_p": 0.9,
  "top_k": 40,
  "num_ctx": 4096,
  "num_gpu": 1
}
```

---

## Integration Examples

### Python

```python
import requests

url = "http://localhost:11434/api/chat"
headers = {"Content-Type": "application/json"}
data = {
    "model": "llama3",
    "messages": [
        {"role": "user", "content": "Write hello world in Python"}
    ],
    "stream": False
}

response = requests.post(url, json=data, headers=headers)
print(response.json()["message"]["content"])
```

### Node.js

```javascript
const fetch = require('node-fetch');

const response = await fetch('http://localhost:11434/api/chat', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    model: 'llama3',
    messages: [{ role: 'user', content: 'Hello!' }]
  })
});

const data = await response.json();
console.log(data.message.content);
```

---

## Advanced Usage

### Streaming Responses

```bash
curl http://localhost:11434/api/chat -d '{
  "model": "llama3",
  "messages": [{"role": "user", "content": "Count to 5"}],
  "stream": true
}'
```

### System Prompts

```bash
curl http://localhost:11434/api/chat -d '{
  "model": "llama3",
  "messages": [
    {"role": "system", "content": "You are a helpful coding assistant."},
    {"role": "user", "content": "Write a function"}
  ]
}'
```

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Port in use | Stop existing Ollama process |
| Model not found | Run `ollama pull <model>` |
| Out of memory | Use smaller model |
| Slow responses | Check GPU acceleration |

---

*Back to [Ollama README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*