# Securing Linux Server

## What You'll Learn

- SSH security
- Firewall configuration
- Fail2ban
- SSL/TLS basics
- Security updates

## Prerequisites

- Completed `08-monitoring-and-logs.md`

## SSH Security

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SSH HARDENING                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  CONFIG FILE: /etc/ssh/sshd_config                                         │
│                                                                             │
│  SETTINGS:                                                                  │
│  • PermitRootLogin no                                                      │
│  • PasswordAuthentication no                                               │
│  • PubkeyAuthentication yes                                                │
│  • MaxAuthTries 3                                                         │
│  • PermitEmptyPasswords no                                                │
│  • X11Forwarding no                                                      │
│                                                                             │
│  AFTER CHANGES:                                                            │
│  sudo systemctl restart sshd                                               │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## SSH Key Setup

```bash
# Generate key on local machine
ssh-keygen -t ed25519 -C "your@email.com"

# Copy to server
ssh-copy-id user@server-ip

# Or manually
cat ~/.ssh/id_ed25519.pub | ssh user@server "mkdir -p ~/.ssh && cat >> ~/.ssh/authorized_keys"
```

## Firewall (ufw)

```bash
# Basic setup
sudo ufw default deny incoming
sudo ufw default allow outgoing

# Allow SSH
sudo ufw allow 22/tcp

# Allow web
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp

# Enable
sudo ufw enable

# Check status
sudo ufw status
```

## Fail2ban

```bash
# Install
sudo apt install fail2ban

# Copy config
sudo cp /etc/fail2ban/jail.conf /etc/fail2ban/jail.local

# Edit settings
sudo nano /etc/fail2ban/jail.local
```

Basic settings:

```ini
[sshd]
enabled = true
port = ssh
filter = sshd
logpath = /var/log/auth.log
maxretry = 3
bantime = 3600
```

## Security Updates

```bash
# Install security updates automatically
sudo apt install unattended-upgrades

# Configure
sudo dpkg-reconfigure -plow unattended-upgrades

# Manual update
sudo apt update && sudo apt upgrade
```

## Summary

- Use SSH keys, disable password auth
- Configure firewall with ufw
- Install Fail2ban
- Keep system updated

## Next Steps

→ Continue to `10-nginx-and-reverse-proxy.md` to learn about web servers.
