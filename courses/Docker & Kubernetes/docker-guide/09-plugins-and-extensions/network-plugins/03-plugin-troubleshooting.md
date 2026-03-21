# Plugin Troubleshooting

## Overview

Docker plugins can sometimes fail to work correctly. This guide covers common issues and how to diagnose and fix them.

## Prerequisites

- Docker Engine 20.10+
- Plugin installation experience

## Step-by-Step Examples

### Checking Plugin Status

```bash
# List all plugins
docker plugin ls

# Inspect a specific plugin
docker plugin inspect my-plugin

# Check if plugin is enabled
docker plugin ls --filter enabled
```

### Enabling/Disabling Plugins

```bash
# Enable a disabled plugin
docker plugin enable my-plugin

# Disable an enabled plugin
docker plugin disable my-plugin

# Force disable (if stuck)
docker plugin disable -f my-plugin
```

### Viewing Logs

```bash
# Plugin logs may be in journalctl
sudo journalctl -u docker -f

# Check Docker daemon logs
sudo cat /var/log/docker.log
```

### Common Issues

- **Plugin not found**: Check docker plugin ls
- **Socket permission**: Ensure Docker has access
- **Config mismatch**: Verify plugin config

## Quick Reference

| Command | Description |
|---------|-------------|
| docker plugin ls | List plugins |
| docker plugin inspect | Inspect plugin |
| docker plugin enable | Enable plugin |
| docker plugin disable | Disable plugin |

## What's Next

Continue to [Docker Desktop Extensions](../desktop-extensions/01-docker-desktop-extensions.md) for desktop features.
