# Cline Requirements

## System Requirements

### Minimum Requirements

| Component | Requirement |
|-----------|-------------|
| Operating System | Windows 10+, macOS 11+, Linux (Ubuntu 20.04+, Debian 10+) |
| VS Code Version | 1.75.0 or higher |
| RAM | 8 GB minimum (16 GB recommended) |
| Storage | 500 MB for extension |
| Internet | Required for API calls |

### Network Requirements

- Internet connection required
- Access to `api.anthropic.com` for Claude API
- Firewall/proxy must allow HTTPS outbound to port 443

## Prerequisites

### Required

1. **VS Code** - Visual Studio Code installed
   - Download from: [code.visualstudio.com](https://code.visualstudio.com)

2. **Anthropic API Key** - Required for AI functionality
   - Get free credits: [console.anthropic.com](https://console.anthropic.com)
   - Note: Free credits are limited; costs apply after depletion

### Optional (for enhanced functionality)

1. **Git** - For version control features
2. **Node.js** - For JavaScript/TypeScript project support
3. **Python** - For Python project support
4. **Docker** - For containerized development

## Environment Considerations

### For Students on Campus Networks

- May require VPN for API access
- Consider using local alternatives (Ollama, LM Studio) if network is restricted

### For Low-Bandwidth Situations

- Cline requires continuous API communication
- Not suitable for very slow connections
- Consider offline alternatives for better experience

## Important Cost Considerations

While Cline itself is free to install, it requires:

| Usage Type | Cost |
|------------|------|
| API Calls | Pay-per-token (~$3-15/month for typical student use) |
| Free Credits | ~$5 free credits for new users |
| Models | Claude 3.5 Sonnet default |

### Cost Management Tips

- Set spending limits in Anthropic console
- Use the extension's built-in cost tracking
- Consider switching to local models (Ollama) for cost savings
- Monitor usage through VS Code status bar