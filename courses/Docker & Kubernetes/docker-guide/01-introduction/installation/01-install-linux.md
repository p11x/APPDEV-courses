# Install Docker on Linux

## Overview

Docker runs natively on Linux, making it the platform of choice for production deployments. This guide covers installing Docker Engine on popular Linux distributions including Ubuntu, Debian, Fedora, and CentOS. Installing Docker on Linux gives you the best performance and most features, as containers share the host kernel directly without any virtualization layer.

## Prerequisites

- A Linux system (Ubuntu 20.04+, Debian 11+, Fedora 38+, or CentOS Stream 9+)
- Root or sudo access to the system
- Basic command-line knowledge
- Internet connection to download packages
- 64-bit architecture (required for Docker)

## Core Concepts

### Docker Engine Components

When you install Docker on Linux, you get several components:

- **dockerd** - The Docker daemon that runs in the background
- **docker** - The Docker CLI client
- **containerd** - Container runtime that manages container lifecycle
- **runc** - Low-level container runtime
- **docker-compose** - Tool for defining multi-container applications

### Installation Methods

There are several ways to install Docker on Linux:

1. **Official Docker Repository** (Recommended) - Install from Docker's repositories for easy updates
2. **Package Manager** - Install using your distribution's package manager
3. **Convenience Script** - Automated installation for testing (not for production)
4. **Binary Installation** - Manual installation of static binaries

The official Docker repository method is recommended because it provides easy installation and updates through your system's package manager.

## Step-by-Step Examples

### Install Docker on Ubuntu and Debian

These instructions work for Ubuntu 20.04+, Debian 11+, and similar Debian-based distributions:

```bash
# Update package index
# Ensures you have the latest package information
sudo apt update

# Install dependencies for adding repositories over HTTPS
# ca-certificates for SSL, curl for downloading, gnupg for signing keys
sudo apt install -y ca-certificates curl gnupg lsb-release

# Add Docker's official GPG key
# This verifies package authenticity
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

# Set up the Docker stable repository
# This adds Docker's apt repository to your system
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

# Update package index again to include Docker's repository
sudo apt update

# Install Docker Engine, containerd, and Docker Compose
# -y confirms all prompts automatically
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# Verify the installation
# This runs a test container that prints a message
sudo docker run --rm hello-world
```

### Install Docker on Fedora

Fedora uses the dnf package manager:

```bash
# Remove any existing Docker installations
sudo dnf remove docker \
  docker-client \
  docker-client-latest \
  docker-common \
  docker-latest \
  docker-latest-logrotate \
  docker-logrotate \
  docker-engine

# Install required dependencies
sudo dnf -y install dnf-plugins-core

# Add Docker repository
# This creates a repo file for Docker's packages
sudo dnf config-manager --add-repo \
  https://download.docker.com/linux/fedora/docker-ce.repo

# Install Docker Engine and components
sudo dnf install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# Start and enable Docker service
# enable makes Docker start on boot
sudo systemctl start docker
sudo systemctl enable docker

# Verify installation
sudo docker run --rm hello-world
```

### Install Docker on CentOS Stream 9

CentOS Stream uses similar steps to Fedora:

```bash
# Remove old Docker versions
sudo dnf remove -y docker docker-client docker-client-latest docker-common \
  docker-latest docker-latest-logrotate docker-logrotate docker-engine

# Add Docker repository
sudo dnf config-manager --add-repo \
  https://download.docker.com/linux/centos/docker-ce.repo

# Install Docker
sudo dnf install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# Start Docker
sudo systemctl start docker
sudo systemctl enable docker

# Verify
sudo docker run --rm hello-world
```

### Configure Docker as a Non-Root User

Running Docker as root is a security risk. Configure your user to run Docker:

```bash
# Create docker group if it doesn't exist
sudo groupadd docker

# Add your user to the docker group
# Replace 'username' with your actual username
sudo usermod -aG docker $USER

# Activate group changes (re-login required for permanent effect)
# This applies the new group without re-logging
newgrp docker

# Verify you can run Docker without sudo
docker run --rm hello-world
```

### Install Docker Compose Standalone (Optional)

If you need docker-compose as a standalone command:

```bash
# Download the current stable release
# Replace v2.24.0 with the latest version from https://github.com/docker/compose/releases
curl -L "https://github.com/docker/compose/releases/download/v2.24.0/docker-compose-$(uname -s)-$(uname -m)" -o /tmp/docker-compose

# Move to your PATH
sudo mv /tmp/docker-compose /usr/local/bin/docker-compose

# Make executable
sudo chmod +x /usr/local/bin/docker-compose

# Verify
docker-compose --version
```

## Common Mistakes

- **Installing from distribution repos**: Distribution-packaged Docker versions are often outdated. Always install from Docker's official repository.
- **Forgetting to add user to docker group**: Running Docker commands with sudo every time is cumbersome and a security risk.
- **Not starting Docker service**: After installation, Docker doesn't start automatically on some distributions. Always check with `systemctl status docker`.
- **Firewall blocking Docker**: Some Linux firewalls (like ufw or firewalld) may block container networking. Configure them to allow Docker traffic.
- **Not enabling Docker at startup**: For servers, enable Docker with `systemctl enable docker` so it starts after reboots.

## Quick Reference

| Command | Description |
|---------|-------------|
| `sudo apt update && sudo apt install docker-ce` | Install on Debian/Ubuntu |
| `sudo dnf install docker-ce` | Install on Fedora/CentOS |
| `sudo systemctl start docker` | Start Docker daemon |
| `sudo systemctl enable docker` | Enable Docker on boot |
| `sudo usermod -aG docker $USER` | Add user to docker group |
| `docker run --rm hello-world` | Test installation |

## What's Next

Now that Docker is installed, continue to [Install Docker on macOS](./02-install-macos.md) if you're using a Mac, or skip to [Install Docker on Windows](./03-install-windows.md) for Windows installation.
