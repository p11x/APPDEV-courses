# LM Studio - Functionalities

## Model Operations

### Download Models

1. Open LM Studio
2. Click "Download" in sidebar
3. Search for model (e.g., llama3, codellama)
4. Click Download

---

### Load Models

1. Select model from list
2. Click "Load"
3. Wait for loading to complete
4. Start chatting

---

## API Usage

### Start Local Server

1. Go to Developer > Local Server
2. Click "Start Server"
3. Default: http://localhost:1234

---

### Python Integration

```python
import requests

url = "http://localhost:1234/v1/chat/completions"
headers = {"Content-Type": "application/json"}
data = {
    "model": "llama3",
    "messages": [
        {"role": "user", "content": "Hello!"}
    ]
}

response = requests.post(url, json=data, headers=headers)
print(response.json()["choices"][0]["message"]["content"])
```

---

### cURL Usage

```bash
curl http://localhost:1234/v1/chat/completions \
  -H "Content-Type: application/json" \
  -d '{
    "model": "llama3",
    "messages": [{"role": "user", "content": "Hello!"}]
  }'
```

---

## Settings

### Model Settings

| Setting | Description |
|---------|-------------|
| Temperature | Response creativity |
| Top P | Nucleus sampling |
| Context Length | Max tokens |
| GPU Offload | Memory optimization |

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Out of memory | Use smaller model |
| Slow responses | Enable GPU acceleration |
| Can't load model | Check disk space |

---

*Back to [LM Studio README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*