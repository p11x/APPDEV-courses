# Continue - System Requirements

## Hardware Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| RAM | 4 GB | 8 GB |
| Disk Space | 200 MB | 500 MB |
| CPU | Dual-core | Quad-core |

## Software Requirements

### Operating System

| OS | Version | Support Status |
|----|---------|----------------|
| Windows | 10, 11 | ✅ Supported |
| macOS | 10.15+ | ✅ Supported |
| Linux | Ubuntu 20.04+ | ✅ Supported |

### IDE Requirements

| IDE | Version | Extension Required |
|-----|---------|-------------------|
| VS Code | 1.65+ | continue.continue |
| IntelliJ IDEA | 2020.1+ | Continue |
| PyCharm | 2020.1+ | Continue |
| WebStorm | 2020.1+ | Continue |

## Network Requirements

| Requirement | Specification |
|-------------|---------------|
| Internet | Required for cloud models |
| Local Network | Required for remote development |

## Account Requirements

| Requirement | Details |
|-------------|---------|
| API Keys | Required for cloud models |
| Ollama | Optional for local |

## Dependencies

### For VS Code Extension

```json
{
  "vscode": "^1.65.0",
  "node": ">=14.0.0"
}
```

### For Local Models (Ollama)

| Component | Requirement |
|-----------|-------------|
| Ollama | Installed and running |
| Model | Downloaded (llama3, codellama, etc.) |

---

*Back to [Continue README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*