# Install Docker on macOS

## Overview

Docker Desktop for Mac provides the easiest way to run Docker containers on macOS. It includes the Docker daemon, Docker CLI, Docker Compose, and other tools you need to build and run containerized applications. Docker Desktop on Mac runs a lightweight Linux virtual machine in the background and provides seamless integration between macOS and Linux containers.

## Prerequisites

- macOS 10.15 (Catalina) or later (macOS 11 Big Sur, 12 Monterey, 13 Ventura, or 14 Sonoma recommended)
- At least 4 GB of RAM available for Docker Desktop
- macOS file system must be APFS (default on modern Macs)
- Administrator privileges for installation
- Apple Silicon (M1/M2/M3) or Intel processor

## Core Concepts

### Docker Desktop Components

When you install Docker Desktop on Mac, you get:

- **Docker Daemon (dockerd)** - Runs inside a Linux VM (HyperKit or VFKit)
- **Docker CLI (docker)** - Command-line tool that communicates with the daemon
- **Docker Compose** - Tool for multi-container applications
- **Docker Buildx** - Extended build functionality
- **Docker Scout** - Image analysis and security
- **Kubernetes** - Single-node Kubernetes for local development
- **Docker Dashboard** - GUI for managing containers

### Architecture Considerations

Docker Desktop runs containers inside a Linux virtual machine. For Apple Silicon Macs (M1, M2, M3), Docker uses the VFKit virtualization framework and can run both ARM64 and x86_64 containers through emulation. For Intel Macs, Docker uses HyperKit. This means all containers run as Linux containers, regardless of your Mac's processor architecture.

## Step-by-Step Examples

### Installing Docker Desktop

1. **Download Docker Desktop**
   
   Visit https://www.docker.com/products/docker-desktop/ and click "Download for Mac". Choose the correct version for your chip:
   
   - Apple Silicon: "Download for Mac with Apple Silicon"
   - Intel: "Download for Mac with Intel processor"

2. **Install Docker Desktop**
   
   ```bash
   # Double-click the .dmg file to open it
   # Drag Docker.app to Applications folder
   # Alternatively, use command line:
   cp -R /Volumes/Docker/Docker.app /Applications/
   ```

3. **Launch Docker Desktop**
   
   - Open Applications folder
   - Double-click Docker.app
   - Or from terminal: open -a Docker

4. **Accept Service Agreement**
   
   When Docker Desktop first launches, you'll see a service agreement. Read and accept it to continue.

5. **Verify Installation**
   
   ```bash
   # Check Docker version
   docker --version
   # Output: Docker version 26.0.0, build 8a792b1

   # Check Docker Compose version
   docker compose version
   # Output: Docker Compose version v2.24.0

   # Run a test container
   docker run --rm hello-world
   ```

### Configuring Docker Desktop

Open Docker Desktop settings by clicking the Docker icon in the menu bar and selecting "Settings":

```bash
# General tab settings (set via defaults):
# - Start Docker Desktop when you log in
# - Include VM in Time Machine backups
# - Use containerd for storing and retrieving images

# Resources tab:
# - CPU: Set CPU limit (default: half of your cores)
# - Memory: Set RAM limit (default: half of your RAM)
# - Disk: Set disk space for Docker (default: 64GB)
# - File sharing: Add directories to share with containers

# Kubernetes tab:
# - Enable Kubernetes cluster
# - Show system containers (advanced)
```

### Using Docker Without Sudo

On macOS, Docker Desktop is configured to allow your user to run Docker commands without sudo:

```bash
# Verify you can run Docker without sudo
docker ps

# If you get permission errors, add yourself to docker group
# Note: On macOS with Docker Desktop, you typically don't need this
# as Docker Desktop handles permissions automatically
```

### Advanced: Installing via Homebrew

You can also install Docker Desktop using Homebrew:

```bash
# Install Homebrew if not already installed
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Install Docker Desktop
brew install --cask docker

# Launch Docker Desktop (this opens the app)
open -a Docker

# Verify installation
docker --version
```

### Troubleshooting Installation Issues

```bash
# Check if Docker Desktop is running
# Look for the Docker icon in the menu bar
# If the icon is gray, Docker is not running

# Check Docker Desktop logs
# Click Docker icon > Diagnose and Feedback > View Logs

# Reset Docker Desktop to factory defaults
# Docker Desktop > Settings > Troubleshooting > Reset Docker Desktop

# Check if virtualization is enabled
# In Terminal:
sysctl kern.hv_support
# Should return: kern.hv_support: 1
```

### Docker Desktop and Rosetta

For Apple Silicon Macs running x86_64 containers, enable Rosetta support:

```bash
# Open Docker Desktop Settings
# Go to Features in Development > General
# Enable "Use Rosetta for x86/amd64 emulation"

# Or via command line:
defaults write com.docker.docker RosettaEnabled -bool true

# Restart Docker Desktop after enabling
```

## Common Mistakes

- **Installing wrong architecture version**: Make sure to download the Apple Silicon version for M1/M2/M3 Macs and Intel version for Intel Macs.
- **Insufficient resources**: Docker Desktop defaults may be too low for large projects. Increase CPU and memory in settings.
- **File sharing issues**: If containers can't access your code, add the directory in Settings > Resources > File Sharing.
- **Not waiting for Docker to start**: Docker Desktop takes time to initialize the Linux VM. Wait for the "Docker is running" message in the menu bar.
- **Running out of disk space**: Monitor disk usage in Docker Desktop dashboard. Clean up unused images and containers regularly.
- **Antivirus conflicts**: Some antivirus software can interfere with Docker. Add Docker to exceptions if needed.

## Quick Reference

| Task | Command/Action |
|------|----------------|
| Install | Download from docker.com or `brew install --cask docker` |
| Launch | `open -a Docker` or click Docker.app |
| Check version | `docker --version` |
| Test Docker | `docker run --rm hello-world` |
| Stop Docker | Click Docker icon > Quit Docker Desktop |
| Reset | Settings > Troubleshooting > Reset Docker Desktop |

## What's Next

Now that Docker is installed on macOS, continue to [Install Docker on Windows](./03-install-windows.md) if you also need Windows installation, or proceed to [What is an Image?](../images/understanding-images/01-what-is-an-image.md) to learn about Docker images.
