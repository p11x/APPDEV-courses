# Install Docker on Windows

## Overview

Docker Desktop for Windows provides the easiest way to run Docker containers on Windows 10 and Windows 11. It uses Windows Subsystem for Linux version 2 (WSL2) or Hyper-V to run a Linux virtual machine where containers actually execute. Docker Desktop provides a seamless experience with Windows, including native Windows container support for Windows-specific applications.

## Prerequisites

- Windows 11 64-bit (Home, Pro, or Enterprise) or Windows 10 64-bit (version 1903 or later, with May 2019 update)
- 4 GB RAM minimum (8 GB recommended)
- 64-bit processor with virtualization support (VT-x/AMD-V)
- WSL2 (recommended) or Hyper-V (for Windows 10 Pro/Enterprise)
- Administrator privileges for installation

## Core Concepts

### Docker Desktop on Windows Architecture

Docker Desktop on Windows runs containers in one of two modes:

1. **Linux Containers (default)** - Runs containers inside a WSL2 or Hyper-V Linux VM. This is the standard mode and supports most container images.

2. **Windows Containers** - Runs native Windows containers using Windows Server containers. This requires Windows Server or Windows 10/11 Enterprise/Pro with Hyper-V.

### WSL2 vs Hyper-V

- **WSL2** (Windows Subsystem for Linux 2): Recommended option, uses lightweight Linux kernel, faster startup, better performance, works on Windows 10 and 11 Home.
- **Hyper-V**: Alternative for Windows 10 Pro/Enterprise, provides full virtualization, may be required for some enterprise scenarios.

### Components Included

- Docker Engine and daemon
- Docker CLI client
- Docker Compose
- Docker Buildx
- Docker Scout
- Kubernetes (optional)
- Docker Dashboard

## Step-by-Step Examples

### Prerequisites Check

First, verify your system meets the requirements:

```powershell
# Check Windows version (run in PowerShell as Administrator)
# Windows 10: version 1903 or later
# Windows 11: any version
winver

# Check if virtualization is enabled
# Returns list of processors with virtualization flags
systeminfo | findstr /C:"Hyper-V"

# Check WSL2 status (Windows 11 or Windows 10)
wsl --status

# Enable required features on Windows 10
# Run PowerShell as Administrator
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart

# Set WSL2 as default
wsl --set-default-version 2
```

### Installing Docker Desktop

1. **Download Docker Desktop**
   
   Visit https://www.docker.com/products/docker-desktop/ and click "Download for Windows".

2. **Run the Installer**
   
   ```powershell
   # Double-click Docker Desktop Installer.exe
   # Or run from command line:
   Start-Process "Docker Desktop Installer.exe" -Wait

   # If using Winget:
   winget install Docker.DockerDesktop
   ```

3. **Installation Options**
   
   The installer will ask for configuration options:
   
   - Use WSL2 instead of Hyper-V (recommended)
   - Add shortcut to desktop
   - Use Windows containers instead of Linux containers (optional)

4. **Restart Your Computer**
   
   The installer will prompt you to restart. Save any open work before restarting.

### First Launch and Configuration

After installation and restart:

```powershell
# Launch Docker Desktop
# Search for "Docker" in Start menu and click Docker Desktop
# Or run from terminal:
start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"

# Wait for Docker to start (check status in system tray)
# The whale icon in the system tray shows Docker status

# Verify installation in PowerShell or Command Prompt
docker --version
# Output: Docker version 26.0.0, build 8a792b1

docker compose version
# Output: Docker Compose version v2.24.0

# Run test container
docker run --rm hello-world
```

### Configuring Docker Desktop

Access settings by right-clicking the Docker icon in the system tray:

```powershell
# General settings:
# - Start Docker Desktop when you log in
# - Use containerd for storing images
# - Enable Docker Compose v2

# Resources settings:
# - CPU limit (default: half of cores)
# - Memory limit (default: half of RAM)
# - Disk space for images
# - WSL integration settings

# Kubernetes settings:
# - Enable Kubernetes cluster
# - Show system containers
```

### Windows Container Mode (Optional)

To run Windows containers instead of Linux containers:

```powershell
# Right-click Docker icon in system tray
# Select "Switch to Windows containers..."

# Or use command line:
& 'C:\Program Files\Docker\Docker\DockerCli.exe' -SwitchDaemon

# Verify Windows container mode
docker info | findstr "OSType"
# Output: OSType: windows
```

### Using Docker with WSL2

For best performance, use Docker Desktop with WSL2:

```powershell
# Install a Linux distribution in WSL2
# Open Microsoft Store and install Ubuntu (or your preferred distro)

# After installation, set up Docker in WSL2:
docker run --rm hello-world

# Docker commands work directly in WSL2 terminal
# They automatically connect to Docker Desktop on Windows

# Speed up file access by storing code in WSL2:
# /home/username/project instead of /mnt/c/project
```

### Troubleshooting Common Issues

```powershell
# Docker Desktop fails to start
# 1. Check if WSL2 is properly installed:
wsl --list --verbose

# 2. Update WSL2 kernel:
wsl --update

# 3. Restart Docker Desktop:
# Right-click Docker icon > Restart Docker

# Check Docker Desktop logs:
# %LOCALAPPDATA%\Docker\log

# Reset Docker Desktop to factory defaults:
# Docker Settings > Troubleshooting > Reset Docker Desktop

# Check for conflicting software:
# Some antivirus, VPN, and virtualization software conflicts with Docker

# Hyper-V issues on Windows 10:
# Enable Hyper-V in Windows Features
# Search "Turn Windows features on or off"
# Check "Hyper-V" and "Windows Subsystem for Linux"
```

### Uninstalling Docker Desktop

```powershell
# Option 1: Use Windows Settings
# Settings > Apps > Docker Desktop > Uninstall

# Option 2: Use command line
start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe" uninstall

# Option 3: Use Winget
winget uninstall Docker.DockerDesktop

# Clean up WSL2 Docker data (optional)
wsl --unregister docker-desktop
wsl --unregister docker-desktop-data
```

## Common Mistakes

- **Not enabling virtualization**: BIOS/UEFI setting for virtualization must be enabled. Check your motherboard settings.
- **Installing wrong version**: Make sure to download the correct installer for your Windows version (ARM64 for ARM devices).
- **Insufficient WSL2 memory**: WSL2 can consume significant memory. Limit it in .wslconfig file.
- **Antivirus conflicts**: Some security software blocks Docker. Add Docker to your antivirus exclusions.
- **Using Linux containers on Windows Server**: Use Windows containers instead on Windows Server.
- **Not restarting after installation**: Docker Desktop requires a full restart to complete installation.

## Quick Reference

| Task | Command/Action |
|------|----------------|
| Install | Download from docker.com or `winget install Docker.DockerDesktop` |
| Launch | Search "Docker" in Start menu |
| Check version | `docker --version` |
| Test Docker | `docker run --rm hello-world` |
| Switch to Windows containers | Right-click Docker icon > Switch to Windows containers |
| Reset Docker | Settings > Troubleshooting > Reset |
| Stop Docker | Right-click Docker icon > Quit Docker Desktop |

## What's Next

Now that Docker is installed on your system, continue to [What is an Image?](../images/understanding-images/01-what-is-an-image.md) to learn about Docker images and how they form the foundation of containerized applications.
