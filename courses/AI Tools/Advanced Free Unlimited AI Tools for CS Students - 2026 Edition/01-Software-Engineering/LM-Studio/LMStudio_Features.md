# LM Studio - Features

## Core Features

### 1. GUI Interface

User-friendly graphical interface for running LLMs locally:

- Model browser and downloader
- Chat interface
- Settings panel
- Model management

---

### 2. Model Management

| Feature | Description |
|---------|-------------|
| Browse | Search HuggingFace models |
| Download | One-click model downloads |
| Switch | Easy model switching |
| Storage | Organize models |

---

### 3. Chat Interface

Interactive AI chat without terminal:

- Conversation history
- Multiple chats
- Export options

---

### 4. Local API Server

Expose LLMs via HTTP API:

```bash
# Default endpoint
http://localhost:1234/v1/chat/completions
```

---

### 5. GPU Acceleration

| Feature | Support |
|---------|----------|
| NVIDIA CUDA | ✅ |
| Apple Silicon | ✅ |
| CPU Only | ✅ |

---

### 6. Model Quantization

Reduce model size while maintaining quality:

| Quantization | Size Reduction | Quality |
|-------------|----------------|---------|
| Q4_K_M | ~60% | High |
| Q5_K_S | ~50% | Very High |
| Q8_0 | ~30% | Very High |

---

## Additional Features

### 7. Cross-Platform

- macOS
- Windows
- Linux

### 8. No Internet Required

- Works offline
- Privacy-focused

---

*Back to [LM Studio README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*