# Process Management

## What You'll Learn

- Understanding processes in Linux
- Viewing processes (ps, top, htop)
- Managing processes (kill, signals)
- Background processes
- Resource monitoring

## Prerequisites

- Completed `02-file-system-and-permissions.md`

## What Is a Process?

A process is a running program:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PROCESS CONCEPTS                                           │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  PROCESS:                                                                   │
│  • A running program                                                       │
│  • Has a unique PID (Process ID)                                          │
│  • Has an owner (user who started it)                                      │
│  • Uses system resources (CPU, memory)                                     │
│                                                                             │
│  PROCESS TYPES:                                                            │
│  • Parent process — Creates child processes                                │
│  • Child process — Created by parent                                       │
│  • Daemon — Background service                                             │
│  • Zombie — Dead process waiting for parent to collect                    │
│  • Orphan — Parent died, adopted by init                                   │
│                                                                             │
│  PROCESS STATES:                                                            │
│  • R — Running                                                            │
│  • S — Sleeping (waiting for event)                                       │
│  • D — Uninterruptible sleep                                              │
│  • T — Stopped                                                            │
│  • Z — Zombie                                                             │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Viewing Processes

### ps Command

```bash
# List all processes
ps aux

# Specific process
ps aux | grep nginx

# Show process tree
ps -ef

# Format output
ps -eo pid,user,comm,%cpu,%mem
```

🔍 **What this means:**
- `ps aux` — Show all processes for all users
- `a` — Show processes for all users
- `u` — Show user-oriented format
- `x` — Show processes without controlling terminal

### top Command

```bash
# Interactive process viewer
top

# Useful keys in top:
# M — Sort by memory
# P — Sort by CPU
# k — Kill process
# r — Renice (change priority)
```

### htop (Better top)

```bash
# Install
sudo apt install htop

# Run (more user-friendly)
htop
```

## Managing Processes

### kill

```bash
# Kill a process by PID
kill 1234

# Kill by name
pkill nginx

# Kill all processes by name
killall nginx
```

🔍 **Signals to kill:**
- `kill -15` (SIGTERM) — Graceful shutdown (default)
- `kill -9` (SIGKILL) — Force kill (can't be ignored)
- `kill -1` (SIGHUP) — Reload configuration

### Process Signals

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    COMMON SIGNALS                                            │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1 (SIGHUP)     — Hangup, reload config                                    │
│  2 (SIGINT)     — Interrupt (Ctrl+C)                                       │
│  9 (SIGKILL)    — Force kill (can't be caught)                            │
│  15 (SIGTERM)   — Graceful termination (default)                          │
│  18 (SIGCONT)   — Continue stopped process                                 │
│  19 (SIGSTOP)   — Stop process (can't be caught)                          │
│  20 (SIGTSTP)   — Terminal stop (Ctrl+Z)                                   │
│                                                                             │
│  EXAMPLES:                                                                  │
│  kill -15 1234   # Gracefully stop                                         │
│  kill -9 1234    # Force stop                                              │
│  kill -HUP 1234  # Reload config                                           │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Background Processes

### Running in Background

```bash
# Start in background with &
python myapp.py &

# Start, then put in background
python myapp.py
# Ctrl+Z to stop
bg

# Bring to foreground
fg
```

### nohup

```bash
# Run immune to hangup (keeps running after logout)
nohup python myapp.py &

# Output to file
nohup python myapp.py > output.log 2>&1 &
```

### screen

```bash
# Install
sudo apt install screen

# Start screen session
screen -S myapp

# Detach from screen (leave running)
# Press Ctrl+A, then D

# Reconnect
screen -r myapp

# List sessions
screen -ls
```

### tmux

```bash
# Start named session
tmux new -s myapp

# Detach
# Press Ctrl+B, then D

# Reconnect
tmux attach -t myapp

# List sessions
tmux ls
```

## Resource Monitoring

### Memory Usage

```bash
# Check memory
free -h

# Detailed memory info
cat /proc/meminfo

# Memory usage by process
ps -eo pid,user,%mem,comm --sort=-%mem | head
```

### CPU Usage

```bash
# CPU info
lscpu

# CPU usage per core
mpstat -P ALL

# Current CPU usage
top
```

### Disk Usage

```bash
# Disk space
df -h

# Directory sizes
du -sh *

# Largest directories
du -h --max-depth=1 | sort -h
```

## Priority and Nice Values

```bash
# Nice value: -20 (highest priority) to 19 (lowest)
# Default: 0

# Start with priority
nice -n 10 python myapp.py

# Change priority of running process
renice 5 -p 1234
```

## Summary

- Every running program is a process with a PID
- Use ps, top, htop to view processes
- Use kill to send signals to processes
- Use screen/tmux for persistent sessions
- Monitor with free, df, top

## Next Steps

→ Continue to `04-user-and-group-management.md` to learn about managing users and groups.
