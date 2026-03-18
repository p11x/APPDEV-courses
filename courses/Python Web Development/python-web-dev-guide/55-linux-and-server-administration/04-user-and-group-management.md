# User and Group Management

## What You'll Learn

- Creating and managing users
- Creating and managing groups
- The sudo system
- SSH key management
- User permissions for web servers

## Prerequisites

- Completed `03-process-management.md`

## Users in Linux

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    USER TYPES                                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ROOT USER (UID 0):                                                        │
│  • Superuser with full system access                                       │
│  • Can do anything                                                        │
│  • Use sparingly for security                                             │
│                                                                             │
│  SYSTEM USERS (UID 1-999):                                                 │
│  • Created by system services                                             │
│  • www-data (web server), postgres (database)                           │
│  • Usually can't login                                                    │
│                                                                             │
│  REGULAR USERS (UID 1000+):                                                │
│  • Created by administrators                                               │
│  • Can login, use sudo                                                    │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Creating and Managing Users

### Adding Users

```bash
# Create new user
sudo adduser username

# Create user without home directory
sudo useradd -r username

# Create user with specific options
sudo useradd -m -s /bin/bash -G docker,sudo username
```

### Managing Users

```bash
# Modify user
sudo usermod -aG groupname username  # Add to group
sudo usermod -L username            # Lock account
sudo usermod -U username            # Unlock account

# Delete user
sudo userdel username              # Keep home
sudo userdel -r username           # Remove home too
```

### Viewing Users

```bash
# List all users
cat /etc/passwd

# Current user
whoami

# Who is logged in
who
```

## Groups

```bash
# Create group
sudo groupadd groupname

# Add user to group
sudo usermod -aG groupname username

# Remove from group
sudo gpasswd -d username groupname

# List groups
groups
cat /etc/group
```

## sudo Configuration

### Using sudo

```bash
# Run command as root
sudo command

# Become root temporarily
sudo -i

# Edit sudoers file safely
sudo visudo
```

### sudoers File

```bash
# Give user full sudo access
username ALL=(ALL:ALL) ALL

# Give group sudo access
%groupname ALL=(ALL:ALL) ALL

# Passwordless sudo for specific commands
username ALL=(ALL) NOPASSWD: /usr/bin/systemctl restart nginx
```

## SSH Key Management

### Generating SSH Keys

```bash
# Generate SSH key pair
ssh-keygen -t ed25519 -C "your.email@example.com"

# Copy public key to server
ssh-copy-id username@server-ip
```

### SSH Configuration

```bash
# Create SSH config file
nano ~/.ssh/config

# Add server configuration
Host myserver
    HostName server-ip
    User username
    IdentityFile ~/.ssh/mykey
    Port 22
```

## Web Server Users

### www-data (Apache/Nginx)

```bash
# Find web server user
grep -E '^(User|Group)' /etc/nginx/nginx.conf
grep -E '^(User|Group)' /etc/apache2/apache2.conf

# Common web server users:
# - www-data (Debian/Ubuntu)
# - nginx (some installations)
# - apache (RHEL/CentOS)
```

### Running Your App as a User

```bash
# Create a service user
sudo adduser --system --group myapp

# Give ownership of app files
sudo chown -R myapp:myapp /var/www/myapp

# Run service as user
sudo -u myapp python myapp.py
```

## Best Practices

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SECURITY BEST PRACTICES                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  ✅ USE SUDO, NOT ROOT:                                                    │
│  • Don't login as root directly                                            │
│  • Use sudo for admin tasks                                                │
│                                                                             │
│  ✅ USE SSH KEYS:                                                          │
│  • Disable password authentication                                        │
│  • Use ed25519 or rsa keys                                                │
│                                                                             │
│  ✅ PRINCIPLE OF LEAST PRIVILEGE:                                         │
│  • Give users only the access they need                                   │
│  • Use groups for permissions                                            │
│                                                                             │
│  ✅ REGULAR ACCOUNTS:                                                      │
│  • Create personal accounts for each user                                  │
│  • Don't share accounts                                                   │
│                                                                             │
│  ✅ AUDIT:                                                                │
│  • Review /var/log/auth.log regularly                                     │
│  • Check who has sudo access                                              │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- Users: adduser, usermod, userdel
- Groups: groupadd, usermod -aG
- Use sudo for admin tasks
- SSH keys provide secure access
- Create separate users for web applications

## Next Steps

→ Continue to `05-networking-basics.md` to learn networking fundamentals.
