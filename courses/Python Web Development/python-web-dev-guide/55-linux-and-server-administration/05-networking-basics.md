# Networking Basics

## What You'll Learn

- IP addresses and networking
- Ports and services
- DNS basics
- Firewall configuration
- Network tools

## Prerequisites

- Completed `04-user-and-group-management.md`

## IP Addresses

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    IP ADDRESS TYPES                                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  IPv4:                                                                      │
│  • 32-bit addresses (4 numbers 0-255)                                     │
│  • Example: 192.168.1.100                                                 │
│                                                                             │
│  PRIVATE IPv4 RANGES:                                                       │
│  • 10.0.0.0/8 (10.x.x.x)                                                 │
│  • 172.16.0.0/12 (172.16-31.x.x)                                        │
│  • 192.168.0.0/16 (192.168.x.x)                                          │
│                                                                             │
│  IPv6:                                                                      │
│  • 128-bit addresses                                                       │
│  • Example: 2001:0db8:85a3:0000:0000:8a2e:0370:7334                      │
│                                                                             │
│  PUBLIC vs PRIVATE:                                                         │
│  • Public — Internet-accessible                                            │
│  • Private — Only within local network                                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Network Interfaces

```bash
# List network interfaces
ip link show
ip addr

# Show routing table
ip route

# Check DNS
cat /etc/resolv.conf
```

## Ports

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    COMMON PORTS                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  WEB:                                                                      │
│  • 80    — HTTP (plain)                                                  │
│  • 443   — HTTPS (encrypted)                                             │
│                                                                             │
│  DEVELOPMENT:                                                               │
│  • 3000  — Node.js, some Python frameworks                               │
│  • 5000  — Flask development                                              │
│  • 8000  — Django, general dev server                                    │
│                                                                             │
│  DATABASE:                                                                 │
│  • 3306  — MySQL/MariaDB                                                │
│  • 5432  — PostgreSQL                                                    │
│  • 27017 — MongoDB                                                       │
│  • 6379  — Redis                                                         │
│                                                                             │
│  OTHER:                                                                     │
│  • 22    — SSH                                                            │
│  • 21    — FTP                                                            │
│  • 25    — SMTP (email)                                                  │
│                                                                             │
│  EPHEMERAL:                                                                │
│  • 32768-60999 — Temporary ports (client-side)                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Checking Network Connections

```bash
# Test connectivity
ping google.com

# Check open ports
netstat -tulpn
ss -tulpn

# Test specific port
nc -zv host.example.com 443
telnet host.example.com 80

# Trace route
traceroute google.com
```

## DNS

```bash
# Look up DNS
nslookup google.com
dig google.com
host google.com

# Flush DNS cache
sudo systemd-resolve --flush-caches  # systemd
sudo resolvectl flush-caches          # newer
```

## Firewall (ufw)

```bash
# Install
sudo apt install ufw

# Enable/disable
sudo ufw enable
sudo ufw disable

# Status
sudo ufw status verbose

# Allow connections
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw allow ssh

# Deny connections
sudo ufw deny 8080/tcp

# Delete rules
sudo ufw delete allow 80/tcp

# Services
sudo ufw allow 'Nginx Full'
sudo ufw allow 'OpenSSH'
```

## iptables

More advanced firewall:

```bash
# List rules
sudo iptables -L -n -v

# Allow established connections
sudo iptables -A INPUT -m state --state ESTABLISHED,RELATED -j ACCEPT

# Allow SSH
sudo iptables -A INPUT -p tcp --dport 22 -j ACCEPT

# Allow HTTP/HTTPS
sudo iptables -A INPUT -p tcp --dport 80 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 443 -j ACCEPT

# Drop everything else
sudo iptables -A INPUT -j DROP
```

## Summary

- Private IP ranges: 10.x.x.x, 172.16-31.x.x, 192.168.x.x
- Ports: 80/443 (web), 22 (SSH), database ports
- Use ufw for simple firewall management
- Check connectivity with ping, netstat, ss

## Next Steps

→ Continue to `06-package-management-linux.md` to learn about installing software.
