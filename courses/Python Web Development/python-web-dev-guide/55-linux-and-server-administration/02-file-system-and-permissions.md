# File System and Permissions

## What You'll Learn

- Understanding Linux permissions
- chmod, chown, chgrp commands
- Special permissions (setuid, setgid, sticky bit)
- Working with directories
- Symbolic vs numeric notation

## Prerequisites

- Completed `01-introduction-to-linux.md`

## Permission Types

Every file has three types of permissions:

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PERMISSION TYPES                                          │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  READ (r) — 4                                                               │
│  • Files: Can view contents                                               │
│  • Directories: Can list files                                            │
│                                                                             │
│  WRITE (w) — 2                                                             │
│  • Files: Can modify contents                                             │
│  • Directories: Can add/remove files                                      │
│                                                                             │
│  EXECUTE (x) — 1                                                           │
│  • Files: Can run as program                                              │
│  • Directories: Can enter (cd into)                                       │
│                                                                             │
│  WHO:                                                                       │
│  • Owner (u) — The user who owns the file                                 │
│  • Group (g) — Users in the file's group                                  │
│  • Others (o) — Everyone else                                             │
│  • All (a) — Owner, group, and others                                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Viewing Permissions

```bash
# List with details
ls -l

# Example output:
# -rw-r--r-- 1 ubuntu ubuntu  1234 Jan  1 12:00 file.txt
# drwxr-xr-x 2 ubuntu ubuntu  4096 Jan  1 12:00 myfolder
```

🔍 **What this means:**
- First character: `-` (file), `d` (directory), `l` (symbolic link)
- Next 9 characters: permissions (owner, group, others)
- `rw-r--r--` = owner can read/write, group can read, others can read

## Changing Permissions: chmod

### Symbolic Notation

```bash
# Add execute for owner
chmod u+x script.sh

# Remove write for group and others
chmod go-w file.txt

# Add read and execute for everyone
chmod a+rx program

# Set specific permissions
chmod u=rw,go=r file.txt
```

### Numeric Notation

```bash
# Common permissions:
chmod 755 file.sh        # rwxr-xr-x (owner: rwx, group: r-x, others: r-x)
chmod 644 file.txt       # rw-r--r-- (owner: rw-, group: r--, others: r--)
chmod 600 key.pem        # rw------- (owner: rw-, nothing for group/others)
chmod 777 folder         # rwxrwxrwx (everyone can everything) ⚠️ DANGER
```

🔍 **Numeric breakdown:**
- First digit: owner (rwx = 4+2+1 = 7)
- Second digit: group (r-x = 4+0+1 = 5)
- Third digit: others (r-x = 4+0+1 = 5)

## Changing Ownership: chown

```bash
# Change file owner
chown username file.txt

# Change owner and group
chown username:group file.txt

# Change group only
chgrp groupname file.txt

# Recursive (all files in folder)
chown -R username:group folder/
```

🔍 **What this does:**
- `chown` changes the user and/or group that owns a file
- Only root can change ownership

## Special Permissions

### SetUID (s)

```bash
# Set on an executable
chmod u+s /usr/bin/program

# Example: /usr/bin/passwd has setuid
# -rwsr-xr-x 1 root root ...
```

🔍 **What this does:**
- When run, program runs as the file's owner (not the user)
- Used for `passwd` so users can change their own password

### SetGID (s)

```bash
# Set on directory
chmod g+s /shared/folder

# Example permissions: drwxrwsr-x
```

🔍 **What this does:**
- Files created in directory inherit the directory's group
- Useful for shared folders

### Sticky Bit (t)

```bash
# Set on directory (like /tmp)
chmod +t /tmp

# Example: drwxrwxrwt
```

🔍 **What this does:**
- Users can only delete their own files
- Prevents users from deleting others' files in /tmp

## Working with Directories

```bash
# Create directory with default permissions
mkdir myfolder

# Create with specific permissions
mkdir -m 755 secured-folder

# Change directory permissions
chmod 755 myfolder

# Recursive
chmod -R 755 folder/
```

## Practical Examples

### Web Server Permissions

```bash
# Web root (Apache/Nginx need to read)
chown -R www-data:www-data /var/www/html
chmod -R 755 /var/www/html

# Files that need write (uploads, logs)
chmod 775 /var/www/html/uploads
chmod 664 /var/www/html/uploads/*

# Sensitive files (shouldn't be web-accessible)
chmod 600 .env
chmod 600 config/secrets.py
```

### SSH Keys

```bash
# Private key should be owner only
chmod 600 ~/.ssh/id_rsa
chmod 644 ~/.ssh/id_rsa.pub

# SSH config
chmod 600 ~/.ssh/config

# .ssh directory
chmod 700 ~/.ssh
```

## umask

The default permission for new files:

```bash
# View current umask
umask

# Common umask: 022
# Files: 666 - 022 = 644
# Dirs:  777 - 022 = 755
```

## Summary

- Permissions: read (r=4), write (w=2), execute (x=1)
- Use chmod to change permissions
- Use chown to change ownership
- Special permissions: setuid, setgid, sticky bit
- Web servers typically run as www-data

## Next Steps

→ Continue to `03-process-management.md` to learn about managing running programs.
