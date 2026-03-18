# Introduction to Linux

## What You'll Learn

- What Linux is and why it matters for web developers
- Linux distributions and choosing one
- The file system layout
- Basic system concepts
- Why web apps run on Linux

## Prerequisites

None—this is an introduction for those new to Linux.

## What Is Linux?

Linux is an operating system, like Windows or macOS, but:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    WHY LINUX FOR WEB DEVELOPMENT                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ FREE AND OPEN SOURCE                                                   │
│  • No license costs                                                       │
│  • Transparent, auditable code                                             │
│                                                                             │
│  ✅ RUNS MOST WEBSERVERS                                                   │
│  • 96% of top web servers run Linux                                       │
│  • AWS, Google Cloud, Azure mostly Linux                                  │
│                                                                             │
│  ✅ POWERFUL AND FLEXIBLE                                                  │
│  • Full control over the system                                           │
│  • Minimal resource usage                                                  │
│                                                                             │
│  ✅ SECURITY                                                                │
│  • Strong permission model                                                 │
│  • Large security community                                               │
│  • Fast security updates                                                   │
│                                                                             │
│  ✅ DEVELOPER ECOSYSTEM                                                    │
│  • Most dev tools work on Linux                                          │
│  • Docker, Kubernetes designed for Linux                                 │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Distributions

Linux comes in many flavors (distributions):

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    POPULAR DISTRIBUTIONS                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  SERVER:                                                                    │
│  • Ubuntu Server — Most popular, great docs                               │
│  • Debian — Stable, conservative                                          │
│  • Rocky Linux — RHEL-compatible, free                                    │
│  • AlmaLinux — RHEL-compatible, free                                      │
│  • Amazon Linux — Optimized for AWS                                       │
│                                                                             │
│  DESKTOP:                                                                   │
│  • Ubuntu — Beginner-friendly                                             │
│  • Fedora — Cutting-edge                                                  │
│  • Arch Linux — Rolling release, advanced                                 │
│  • Linux Mint — Very beginner-friendly                                    │
│                                                                             │
│  FOR WEB DEV:                                                              │
│  → Ubuntu Server (most common)                                           │
│  → Debian (if you prefer stability)                                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## The File System

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    LINUX FILE SYSTEM                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  /                    ← Root (top of everything)                          │
│  ├── bin/             ← Essential binaries                                │
│  ├── boot/            ← Boot files (kernel, GRUB)                         │
│  ├── dev/             ← Device files                                      │
│  ├── etc/             ← Configuration files                              │
│  ├── home/            ← User home directories                            │
│  │   └── username/                                                   │
│  ├── lib/             ← Shared libraries                                 │
│  ├── media/           ← Removable media mount points                     │
│  ├── mnt/             ← Temporary mount points                           │
│  ├── opt/             ← Optional/third-party software                    │
│  ├── proc/            ← Process information (virtual)                    │
│  ├── root/            ← Root user's home                                  │
│  ├── run/             ← Runtime data                                     │
│  ├── sbin/            ← System binaries                                   │
│  ├── srv/             ← Service data                                      │
│  ├── sys/             ← System information (virtual)                     │
│  ├── tmp/             ← Temporary files                                   │
│  ├── usr/             ← User programs                                    │
│  └── var/             ← Variable data (logs, cache)                      │
│      ├── log/         ← System logs                                       │
│      └── www/         ← Web server files (often)                          │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Important Directories for Web Dev

```bash
# Where config files live
/etc/

# Where logs are stored
/var/log/

# Web server root (Debian/Ubuntu)
/var/www/html/

# User home directories
/home/

# System logs
/var/log/syslog
/var/log/nginx/
/var/log/apache2/
```

## Users and Permissions

Linux has a multi-user system:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    USERS AND PERMISSIONS                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  USERS:                                                                    │
│  • root — Superuser (can do anything)                                    │
│  • www-data — Web server user                                             │
│  • ubuntu — Regular user (your login)                                    │
│                                                                             │
│  PERMISSIONS:                                                              │
│  • r (read) — 4                                                           │
│  • w (write) — 2                                                          │
│  • x (execute) — 1                                                        │
│                                                                             │
│  OWNERS:                                                                   │
│  • User — The file's owner                                                │
│  • Group — The file's group                                               │
│  • Others — Everyone else                                                 │
│                                                                             │
│  Example:                                                                  │
│  -rwxr-xr-x 1 user group 1234 Jan 1 12:00 file                           │
│  ^^^^                                                                     │
│  Owner: rwx (7)                                                            │
│  Group: r-x (5)                                                            │
│  Others: r-x (5)                                                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Getting Started

### Accessing Linux

```bash
# SSH into a server
ssh username@server-ip-address

# Example
ssh ubuntu@192.168.1.100

# With key
ssh -i ~/.ssh/my-key.pem ubuntu@server-ip
```

### Basic Commands

```bash
# Who am I?
whoami

# Current directory
pwd

# List files
ls -la

# Change directory
cd /var/www

# Create directory
mkdir myproject

# Create file
touch index.html

# Edit file
nano file.txt    # Simple
vim file.txt     # Powerful
```

## System Information

```bash
# Check OS
cat /etc/os-release

# Check kernel
uname -a

# Check disk space
df -h

# Check memory
free -h

# Check CPU
lscpu

# Check processes
top
htop  # Better if installed
```

## Updating the System

```bash
# Ubuntu/Debian
sudo apt update           # Refresh package lists
sudo apt upgrade         # Install updates
sudo apt full-upgrade    # Include new dependencies

# Check for specific package
apt policy nginx
```

## Summary

- Linux powers most web servers and cloud infrastructure
- Ubuntu Server is a great starting point
- Key directories: /etc (config), /var/log (logs), /home (users)
- Users and permissions control access
- SSH lets you connect to remote servers

## Next Steps

→ Continue to `02-file-system-and-permissions.md` to learn more about file permissions.
