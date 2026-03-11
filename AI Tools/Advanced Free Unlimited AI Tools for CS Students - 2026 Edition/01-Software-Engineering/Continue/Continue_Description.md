# Continue - Detailed Description

## Overview

Continue is an open-source AI code assistant that transforms your IDE into a powerful coding companion. Unlike proprietary solutions, Continue is fully transparent, customizable, and can run entirely locally.

## Key Capabilities

### 1. Open-Source Architecture

- **Transparency**: All code is open source on GitHub
- **Customization**: Modify and extend as needed
- **Community**: Active development and support

### 2. Multi-Provider Support

Continue supports multiple LLM providers:

| Provider | Type | Description |
|----------|------|-------------|
| Ollama | Local | Run models locally |
| Anthropic | Cloud | Claude models |
| OpenAI | Cloud | GPT models |
| Azure OpenAI | Cloud | Enterprise OpenAI |
| Google | Cloud | Gemini models |

### 3. Local-First Design

- **Privacy**: Code stays on your machine
- **Offline**: Works without internet
- **No Limits**: Unlimited queries

### 4. IDE Integration

- **VS Code**: Full feature support
- **JetBrains**: IntelliJ, PyCharm, WebStorm
- **Remote**: Works over SSH

## Technical Details

### Architecture

- **Plugin**: Built as VS Code/JetBrains extension
- **Config**: JSON-based configuration
- **Context**: Automatic codebase awareness
- **Slash Commands**: Quick actions

### Security

- **Local Processing**: Code never leaves your machine (with Ollama)
- **API Keys**: Stored securely in config
- **No Telemetry**: Optional anonymous usage data

## Comparison with Other Tools

| Feature | Continue | Copilot | Codeium |
|---------|----------|---------|---------|
| Open Source | ✅ | ❌ | ❌ |
| Local Run | ✅ | ❌ | ❌ |
| Free Tier | ✅ | Limited | ✅ |
| Self-Hosted | ✅ | ❌ | ❌ |

---

*Back to [Continue README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*