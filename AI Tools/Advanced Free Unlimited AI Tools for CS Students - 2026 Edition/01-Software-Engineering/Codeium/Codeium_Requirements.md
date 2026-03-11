# Codeium - System Requirements

## Hardware Requirements

| Component | Minimum | Recommended | Notes |
|-----------|---------|-------------|-------|
| RAM | 4 GB | 8 GB | For IDE + Codeium |
| Disk Space | 500 MB | 1 GB | Cached models |
| CPU | Dual-core | Quad-core | Faster processing |
| GPU | Not required | Optional | No GPU needed |

## Software Requirements

### Operating System

| OS | Version | Support Status |
|----|---------|----------------|
| Windows | 10, 11 | ✅ Supported |
| macOS | 10.15+ | ✅ Supported |
| Linux | Ubuntu 20.04+, Debian 10+ | ✅ Supported |

### IDE/Editor Requirements

| IDE | Version | Extension Required |
|-----|---------|-------------------|
| VS Code | 1.65+ | codeium.codeium |
| IntelliJ IDEA | 2020.1+ | Codeium |
| PyCharm | 2020.1+ | Codeium |
| WebStorm | 2020.1+ | Codeium |
| PhpStorm | 2020.1+ | Codeium |
| RubyMine | 2020.1+ | Codeium |
| GoLand | 2020.1+ | Codeium |
| CLion | 2020.1+ | Codeium |
| Rider | 2020.1+ | Codeium |
| Eclipse | 2021-06+ | Codeium |
| Jupyter | 1.0+ | jupyterlab-codeium |

### Browser Requirements (for Web)

| Browser | Version | Notes |
|---------|---------|-------|
| Chrome | 90+ | Recommended |
| Firefox | 88+ | Supported |
| Edge | 90+ | Supported |
| Safari | 14+ | Limited support |

## Network Requirements

| Requirement | Specification |
|-------------|---------------|
| Internet | Required for activation |
| Connection | Stable broadband |
| Latency | < 100ms recommended |
| Bandwidth | 1 Mbps minimum |

## Account Requirements

| Requirement | Details |
|-------------|---------|
| Email | Required for activation |
| Verification | Email verification needed |
| Student Email | .edu not required for free tier |

## Dependencies

### For VS Code Extension

```json
{
  "vscode": "^1.65.0",
  "node": ">=14.0.0"
}
```

### For JetBrains

- Java Runtime 11+
- Bundled with IDE

### For Vim/Neovim

- Vim 8.2+ or Neovim 0.6+
- Node.js 14+ (for plugin)

## Compatibility Matrix

### Python Versions

| Version | Support |
|---------|---------|
| 2.7 | ⚠️ Limited |
| 3.6+ | ✅ Full |

### JavaScript/TypeScript

| Version | Support |
|---------|---------|
| ES5 | ✅ Full |
| ES6+ | ✅ Full |
| TypeScript 4.x | ✅ Full |
| TypeScript 5.x | ✅ Full |

### Other Languages

| Language | Min Version |
|----------|-------------|
| Java | 8+ |
| C/C++ | C11, C++17 |
| Go | 1.11+ |
| Rust | 1.40+ |
| Ruby | 2.0+ |
| PHP | 5.6+ |
| Swift | 5.0+ |
| Kotlin | 1.0+ |

---

*Back to [Codeium README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*