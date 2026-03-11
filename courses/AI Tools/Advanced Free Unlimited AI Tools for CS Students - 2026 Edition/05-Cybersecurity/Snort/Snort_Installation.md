# Snort - Installation

## Installation Steps

### Linux (Ubuntu/Debian)

```bash
# Update package list
sudo apt update

# Install Snort
sudo apt install snort

# Verify installation
snort -V
```

### Linux (From Source)

```bash
# Install dependencies
sudo apt install build-essential libpcap-dev libpcre3-dev libnet1-dev zlib1g-dev

# Download Snort
wget https://www.snort.org/downloads/snort/snort-2.9.20.tar.gz

# Extract and compile
tar -xvzf snort-2.9.20.tar.gz
cd snort-2.9.20
./configure --enable-sourcefire
make
sudo make install
```

### Configuration

```bash
# Create required directories
sudo mkdir /var/log/snort
sudo mkdir /etc/snort/rules

# Edit snort.conf
sudo nano /etc/snort/snort.conf
```

---

*Back to [Cybersecurity README](../README.md)*