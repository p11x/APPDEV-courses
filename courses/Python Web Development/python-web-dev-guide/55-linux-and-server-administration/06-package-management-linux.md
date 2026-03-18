# Package Management (Linux)

## What You'll Learn

- APT package manager (Ubuntu/Debian)
- Installing, updating, removing packages
- Adding repositories
- Snap and Flatpak

## Prerequisites

- Completed `05-networking-basics.md`

## APT (Debian/Ubuntu)

### Basic Commands

```bash
# Update package lists
sudo apt update

# Upgrade all packages
sudo apt upgrade

# Install a package
sudo apt install nginx

# Remove a package
sudo apt remove nginx

# Remove package and config
sudo apt purge nginx

# Search for packages
apt search nginx

# Show package info
apt show nginx
```

### Package Lists

```bash
# List installed packages
dpkg -l

# List files from package
dpkg -L nginx

# What package provides file?
dpkg -S /usr/bin/nginx
```

## Adding Repositories

### PPA (Ubuntu)

```bash
# Add PPA
sudo add-apt-repository ppa:nginx/stable

# Update and install
sudo apt update
sudo apt install nginx
```

### Third-Party Repos

```bash
# Add repository
sudo add-apt-repository "deb [arch=amd64] https://repo.example.com/ stable main"

# Or manually add to /etc/apt/sources.list.d/
```

## Snap

```bash
# Install snapd
sudo apt install snapd

# Find packages
snap find nginx

# Install
sudo snap install nginx

# Update
sudo snap refresh nginx

# List installed
snap list
```

## Summary

- Use apt for most packages on Debian/Ubuntu
- Always apt update before installing
- PPAs provide newer versions
- Snaps work across distributions

## Next Steps

→ Continue to `07-systemd-and-services.md` to learn about managing services.
