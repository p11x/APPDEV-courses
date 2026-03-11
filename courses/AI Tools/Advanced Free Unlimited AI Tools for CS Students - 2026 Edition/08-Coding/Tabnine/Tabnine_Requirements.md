# Tabnine - Requirements

## System Requirements

### Hardware Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| **RAM** | 4 GB | 8 GB or more |
| **Storage** | 200 MB | 500 MB |
| **Processor** | Intel Core i3 / AMD Ryzen 3 | Intel Core i5 / AMD Ryzen 5 |
| **Internet** | Required for cloud mode | Stable connection recommended |

### Supported Operating Systems

| OS | Version |
|----|---------|
| **Windows** | Windows 10, 11 (64-bit) |
| **macOS** | macOS 10.14 (Mojave) or later |
| **Linux** | Ubuntu 18.04+, Fedora 30+, Debian 10+ |

---

## Editor/IDE Requirements

### Supported Editors

| Editor | Version | Plugin |
|--------|---------|--------|
| **VS Code** | 1.40.0+ | Tabnine Extension |
| **IntelliJ IDEA** | 2019.3+ | Tabnine Plugin |
| **PyCharm** | 2019.3+ | Tabnine Plugin |
| **WebStorm** | 2019.3+ | Tabnine Plugin |
| **PhpStorm** | 2019.3+ | Tabnine Plugin |
| **RubyMine** | 2019.3+ | Tabnine Plugin |
| **GoLand** | 2019.3+ | Tabnine Plugin |
| **CLion** | 2019.3+ | Tabnine Plugin |
| **Rider** | 2019.3+ | Tabnine Plugin |
| **Eclipse** | 2019-09+ | Tabnine Plugin |
| **Vim** | 8.0+ | TabnineVim |
| **Neovim** | 0.6.0+ | TabnineVim |
| **Sublime Text** | 3.0+ | Tabnine Package |
| **Atom** | 1.40.0+ | Tabnine Package |

### Browser Requirements (for Web Dashboard)

| Browser | Version |
|---------|---------|
| **Chrome** | 80+ |
| **Firefox** | 75+ |
| **Edge** | 80+ |
| **Safari** | 13.1+ |

---

## Software Dependencies

### For Local Mode (Offline)

- **No internet required** after initial installation
- No additional runtime dependencies
- Works completely offline

### For Cloud Mode (Online)

- **Internet connection** required
- Firewall/proxy access to:
  - `*.tabnine.com`
  - `api.tabnine.com`

### Language Support

Tabnine supports 20+ programming languages:

| Language | Support Level |
|----------|---------------|
| **Python** | Full |
| **JavaScript/TypeScript** | Full |
| **Java** | Full |
| **C/C++** | Full |
| **C#** | Full |
| **Go** | Full |
| **Rust** | Full |
| **PHP** | Full |
| **Ruby** | Full |
| **Swift** | Full |
| **Kotlin** | Full |
| **Scala** | Full |
| **HTML/CSS** | Full |
| **SQL** | Full |
| **Shell/Bash** | Full |
| **R** | Basic |
| **Julia** | Basic |
| **Lua** | Basic |
| **Perl** | Basic |
| **Haskell** | Basic |

---

## Account Requirements

### Free Tier

- **No account required** for basic usage
- Email registration optional for:
  - Cloud sync settings
  - Personalized suggestions

### Optional Account Features

| Feature | Free | Premium |
|---------|------|---------|
| Basic code completion | ✓ | ✓ |
| Local AI model | ✓ | ✓ |
| Cloud AI suggestions | Limited | Unlimited |
| Team features | ✗ | ✓ |
| Custom AI training | ✗ | ✓ |

---

## Network Requirements

### Firewall Configuration

For corporate networks, allow:

```
# Outbound HTTPS
Host: *.tabnine.com
Port: 443
```

### Proxy Support

Tabnine supports HTTP/HTTPS proxies:

```json
{
  "tabnine.proxy": "http://proxy.example.com:8080",
  "tabnine.proxy_authorization": "basic"
}
```

---

## Performance Considerations

### RAM Usage

| Mode | RAM Usage |
|------|-----------|
| Local (offline) | ~100-200 MB |
| Cloud (online) | ~50-100 MB |

### CPU Usage

- **Idle**: <1% CPU
- **Active suggestions**: 1-5% CPU
- **Model loading**: 10-20% CPU (temporary)

---

## Compatibility

### Works With

- GitHub Copilot (can coexist)
- Other LSP-based tools
- Language servers (Jedi, Pyls, etc.)
- Prettier, ESLint
- Git integrations

### Known Conflicts

- Some older autocomplete plugins
- Power Mode extensions (may cause lag)

---

*Back to [08-Coding README](../README.md)*
*Back to [Main README](../../README.md)*