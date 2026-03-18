# Systemd and Services

## What You'll Learn

- What systemd is
- Managing services with systemctl
- Creating service files
- Viewing logs with journalctl
- Timers and targets

## Prerequisites

- Completed `06-package-management-linux.md`

## Systemd Overview

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SYSTEMD COMPONENTS                                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  SYSTEMD MANAGES:                                                           │
│  • Services (daemons)                                                      │
│  • Sockets                                                                 │
│  • Devices                                                                 │
│  • Mount points                                                            │
│  • Timers                                                                  │
│  • Targets (like runlevels)                                               │
│                                                                             │
│  UNIT FILES:                                                               │
│  • .service — Running services                                            │
│  • .socket — IPC sockets                                                  │
│  • .target — Group of units                                               │
│  • .timer — Scheduled tasks                                               │
│  • .mount — Filesystem mounts                                             │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Managing Services

```bash
# Start service
sudo systemctl start nginx

# Stop service
sudo systemctl stop nginx

# Restart service
sudo systemctl restart nginx

# Reload config (if supported)
sudo systemctl reload nginx

# Check status
sudo systemctl status nginx

# Enable on boot
sudo systemctl enable nginx

# Disable on boot
sudo systemctl disable nginx

# Check if enabled
sudo systemctl is-enabled nginx

# List all services
systemctl list-units --type=service

# List failed services
systemctl --failed
```

## Creating a Service File

Create `/etc/systemd/system/myapp.service`:

```ini
[Unit]
Description=My Python Web Application
After=network.target

[Service]
Type=simple
User=www-data
Group=www-data
WorkingDirectory=/var/www/myapp
ExecStart=/var/www/myapp/venv/bin/python -m uvicorn main:app --host 0.0.0.0 --port 8000
Restart=always
RestartSec=5

[Install]
WantedBy=multi-user.target
```

Then:

```bash
# Reload systemd
sudo systemctl daemon-reload

# Start service
sudo systemctl start myapp

# Enable on boot
sudo systemctl enable myapp
```

## Viewing Logs

```bash
# View service logs
sudo journalctl -u nginx

# Follow logs
sudo journalctl -u nginx -f

# View recent logs
sudo journalctl -u nginx -n 50

# View since time
sudo journalctl -u nginx --since "1 hour ago"

# View logs from boot
journalctl -b

# Disk usage
journalctl --disk-usage
```

## Targets

```bash
# Change target (runlevel)
sudo systemctl isolate graphical.target

# Set default target
sudo systemctl set-default multi-user.target

# List targets
systemctl list-units --type=target
```

## Timers (Cron Alternative)

Create `/etc/systemd/system/daily-backup.timer`:

```ini
[Unit]
Description=Daily Backup Timer

[Timer]
OnCalendar=daily
Persistent=true

[Install]
WantedBy=timers.target
```

Create `/etc/systemd/system/daily-backup.service`:

```ini
[Unit]
Description=Daily Backup

[Service]
Type=oneshot
ExecStart=/usr/local/bin/backup.sh
```

Then enable:

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now daily-backup.timer
```

## Summary

- systemctl manages services
- Create .service files in /etc/systemd/system/
- journalctl views logs
- Timers replace cron jobs

## Next Steps

→ Continue to `08-monitoring-and-logs.md` to learn about system monitoring.
