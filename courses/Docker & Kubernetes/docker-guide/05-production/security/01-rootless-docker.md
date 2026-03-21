# Rootless Docker

## Overview

Rootless Docker allows running the Docker daemon and containers as a non-root user. This significantly improves security by reducing the attack surface and limiting the damage from potential container escapes. Rootless mode is available on Linux and provides a more secure way to run Docker in production and shared environments.

## Prerequisites

- Linux kernel 5.11+ (for full functionality)
- Root access for initial setup
- Understanding of user namespaces

## Core Concepts

### How Rootless Works

Rootless Docker uses:
- **User namespaces**: Maps root in container to non-root on host
- **Seccomp**: Restricts syscalls
- **AppArmor/SELinux**: Mandatory access control

### Benefits

- No root access required on host
- Reduced attack surface
- Better isolation
- Compliance with security policies

## Step-by-Step Examples

### Installing Rootless Docker

```bash
# Install rootless Docker
# Download and run the install script
curl -fsSL https://get.docker.com/rootless | sh

# Start Docker in rootless mode
# This starts the daemon as your user
~/bin/rootlesskit dockerd

# Or use systemd
systemctl --user enable docker
systemctl --user start docker
```

### Running Rootless Containers

```bash
# After setting up rootless Docker, run containers normally
docker run -d --name web nginx

# Check user in container
docker exec web whoami
# Output: root (inside container, but mapped outside)
```

### Configuring User Namespaces

```bash
# Enable user namespace remapping
# Edit /etc/docker/daemon.json
{
  "userns-remap": "default"
}

# Restart Docker
sudo systemctl restart docker
```

## Common Mistakes

- **Not understanding mapping**: Root in container = non-root on host.
- **Permission issues**: May need to adjust file permissions.

## Quick Reference

| Feature | Rootful | Rootless |
|---------|---------|----------|
| Daemon runs as | Root | Non-root |
| Container root | Actual root | Mapped |
| Security | Lower | Higher |
| Setup complexity | Simple | More involved |

## What's Next

Continue to [Image Scanning](./02-image-scanning.md)
