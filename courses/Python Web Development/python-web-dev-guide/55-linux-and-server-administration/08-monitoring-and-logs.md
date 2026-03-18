# Monitoring and Logs

## What You'll Learn

- System monitoring tools
- Log file locations
- Log rotation
- Resource monitoring
- Alerting basics

## Prerequisites

- Completed `07-systemd-and-services.md`

## Log Locations

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    IMPORTANT LOG FILES                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  SYSTEM:                                                                   │
│  • /var/log/syslog — System messages                                      │
│  • /var/log/kern.log — Kernel messages                                    │
│  • /var/log/dmesg — Driver messages                                      │
│                                                                             │
│  AUTHENTICATION:                                                            │
│  • /var/log/auth.log — Login attempts (Debian/Ubuntu)                   │
│  • /var/log/secure — Login attempts (RHEL/CentOS)                      │
│                                                                             │
│  WEB SERVER:                                                               │
│  • /var/log/nginx/access.log                                              │
│  • /var/log/nginx/error.log                                               │
│  • /var/log/apache2/access.log                                           │
│                                                                             │
│  APPLICATION:                                                              │
│  • /var/log/myapp/                                                       │
│                                                                             │
│  SYSTEMD:                                                                  │
│  • journalctl                                                              │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Monitoring Tools

### top/htop

```bash
# Interactive process monitor
top
htop

# Show processes by memory
ps aux --sort=-%mem | head

# Show processes by CPU
ps aux --sort=-%cpu | head
```

### Resource Monitoring

```bash
# Memory usage
free -h

# Disk usage
df -h
du -sh *

# CPU info
lscpu
uptime

# I/O stats
iostat -x 1
```

### Network Monitoring

```bash
# Show network connections
ss -tunapl

# Show listening ports
ss -tulpn

# Bandwidth monitoring
iftop
nethogs
```

## Log Analysis

### tail

```bash
# Follow logs in real-time
tail -f /var/log/nginx/access.log

# Show last 100 lines
tail -n 100 /var/log/syslog
```

### grep for logs

```bash
# Find errors
grep -i error /var/log/nginx/error.log

# Find specific IP
grep "192.168.1.100" /var/log/nginx/access.log

# Count requests per IP
awk '{print $1}' /var/log/nginx/access.log | sort | uniq -c | sort -nr
```

## Log Rotation

### Configure logrotate

Edit `/etc/logrotate.d/nginx`:

```
/var/log/nginx/*.log {
    daily
    missingok
    rotate 14
    compress
    delaycompress
    notifempty
    create 0640 www-data adm
    sharedscripts
    postrotate
        [ -f /var/run/nginx.pid ] && kill -USR1 `cat /var/run/nginx.pid`
    endscript
}
```

## Monitoring Services

### monit

```bash
# Install
sudo apt install monit

# Configuration
sudo nano /etc/monit/monitrc
```

### Netdata

```bash
# Install
bash <(curl -Ss https://my-netdata.io/kickstart.sh)
```

## Summary

- Key logs: /var/log/ for system, journalctl for systemd
- Use htop, top for process monitoring
- Use tail -f to follow logs
- Logrotate manages log sizes

## Next Steps

→ Continue to `09-securing-linux-server.md` to learn server security.
