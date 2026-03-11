# Kilo Code Requirements

## System Requirements

### Minimum Requirements

| Component | Requirement |
|-----------|-------------|
| Operating System | Windows 10+, macOS 11+, Linux |
| VS Code Version | 1.75.0 or higher |
| RAM | 4 GB minimum |
| Storage | 200 MB |
| Internet | Required for AI features |

### Recommended Requirements

| Component | Recommendation |
|-----------|----------------|
| VS Code Version | Latest stable |
| RAM | 8 GB or higher |
| Internet | Broadband connection |

## Software Requirements

### Required Software

1. **Visual Studio Code**
   - Download from: [code.visualstudio.com](https://code.visualstudio.com)
   - Supported versions: 1.75+

2. **Internet Connection**
   - Required for AI processing
   - Latency affects response time

### Supported Platforms

| Platform | Version Support |
|----------|-----------------|
| Windows | Windows 10, 11 |
| macOS | 11 (Big Sur)+ |
| Linux | Ubuntu 20.04+, Debian 10+ |

## VS Code Extensions

### Prerequisites

- VS Code installed and running
- Account registration (free tier available)

### Extension Dependencies

No additional dependencies required beyond VS Code itself.

## Account Requirements

### Free Tier

- Email registration required
- No credit card needed
- Limited daily requests

### Features by Tier

| Feature | Free | Pro |
|---------|------|-----|
| Code completions | Limited | Unlimited |
| Length limits | Short | Extended |
| Priority | Standard | High |

## Environment Considerations

### For Students

- Works on most student laptops
- Free tier sufficient for learning
- Internet required for AI features

### Network Requirements

| Requirement | Detail |
|------------|--------|
| Bandwidth | 1 Mbps minimum |
| Latency | <200ms preferred |
| Stability | Stable connection needed |

## Hardware Recommendations

### By Use Case

| Use Case | RAM | CPU |
|----------|-----|-----|
| Light coding | 4 GB | Dual-core |
| Full development | 8 GB | Quad-core |
| Heavy projects | 16 GB | Multi-core |

## Compatibility

### VS Code Settings

Some settings may affect performance:

```json
{
  "editor.inlineSuggest": true,
  "editor.suggestPreview": true
}
```

### Conflicting Extensions

Some extensions may conflict:

- Other AI coding assistants
- Heavy language servers

### Firewall/Proxy

Ensure access to:

- `*.kilocode.io`
- `api.kilocode.io`
- VS Code marketplace