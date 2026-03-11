# PostgreSQL - Installation

## Windows Installation

### Using Installer
1. Download installer from [postgresql.org](https://www.postgresql.org/download/windows/)
2. Run the installer and follow wizard
3. Set password for postgres superuser
4. Choose port (default 5432)
5. Complete installation

### Using Chocolatey
```powershell
choco install postgresql
```

## macOS Installation

### Using Homebrew
```bash
brew install postgresql
brew services start postgresql
```

## Linux Installation

### Ubuntu/Debian
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

### CentOS/RHEL
```bash
sudo yum install postgresql-server postgresql-contrib
sudo postgresql-setup --initdb
sudo systemctl start postgresql
```

## Connect to PostgreSQL

```bash
sudo -u postgres psql
```

---

*Back to [Databases & Systems README](../README.md)*