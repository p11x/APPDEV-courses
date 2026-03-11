# SQLite - Installation

## Windows

### Using Installer
1. Download from [sqlite.org](https://www.sqlite.org/download.html)
2. Download sqlite-tools-win-x64-*.zip
3. Extract to a folder and add to PATH

### Using Chocolatey
```powershell
choco install sqlite
```

## macOS

```bash
# Pre-installed on macOS
# Or update via Homebrew
brew install sqlite
```

## Linux

```bash
# Ubuntu/Debian
sudo apt install sqlite3

# CentOS/RHEL
sudo yum install sqlite
```

## Verify Installation

```bash
sqlite3 --version
```

---

*Back to [Databases & Systems README](../README.md)*