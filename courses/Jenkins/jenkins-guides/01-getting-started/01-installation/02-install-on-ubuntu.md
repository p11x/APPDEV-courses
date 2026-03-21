# Install Jenkins on Ubuntu 22.04

## What this covers

This guide walks through installing Jenkins on Ubuntu 22.04 (LTS) using the official Jenkins apt repository. You'll install Java 17 (required for Jenkins LTS 2.440+), add the Jenkins repository, install Jenkins, and configure it to start automatically on boot.

## Prerequisites

- Ubuntu 22.04 LTS server (physical or virtual)
- Root or sudo access
- At least 2GB RAM (4GB recommended for production)
- Internet connectivity

## Why Java 17?

Jenkins LTS 2.440+ requires Java 17 as the minimum version. Java 17 is the current Long Term Support version from Oracle/OpenJDK and provides better performance, security, and modern language features that Jenkins leverages.

## Step-by-Step Installation

### Step 1: Update Package Lists

First, ensure your package lists are up to date:

```bash
# Update the package index
sudo apt update
```

**What this does**: Downloads the latest package information from all configured repositories. The `sudo` command gives you temporary root privileges to install software.

### Step 2: Install Java 17 (OpenJDK)

Jenkins requires Java 17. Install the OpenJDK 17 package:

```bash
# Install OpenJDK 17 (Eclipse Temurin distribution)
# -y flag automatically answers "yes" to prompts
sudo apt install -y openjdk-17-jdk
```

**What this does**: Installs the OpenJDK 17 Development Kit, which includes:
- `java` command to run Java applications
- `javac` command to compile Java code
- Libraries and headers needed by Java applications

**Expected output**:
```
Reading package lists... Done
Building dependency tree
Reading state information... Done
The following NEW packages will be installed:
  openjdk-17-jdk
0 upgraded, 1 newly installed, 0 to remove and 0 not upgraded.
After this operation, 218 MB of additional disk space will be used.
Setting up openjdk-17-jdk (17.0.12+9) ...
openjdk-17-jdk installation completed.
```

Verify Java installation:

```bash
# Check Java version - should show "openjdk version "17.x.x""
java -version
```

### Step 3: Add the Jenkins GPG Key

Jenkins packages are signed to ensure authenticity. Add the Jenkins GPG key:

```bash
# Download and add the Jenkins GPG key
# -q: quiet mode (less verbose)
# -s: silent (no progress indicator)
# --no-check-certificate: skip SSL verification (not recommended for production)
curl -fsSL https://pkg.jenkins.io/debian/jenkins.io.key | sudo tee \
  /usr/share/keyrings/jenkins-keyring.asc > /dev/null
```

**What this does**: 
- Downloads the Jenkins signing key from the official Jenkins package repository
- Pipes it to `tee` which writes to the file AND displays output
- Redirects to /dev/null to keep the terminal clean
- The key is stored in `/usr/share/keyrings/jenkins-keyring.asc`

**Expected output**: No output (quiet mode)

### Step 4: Add the Jenkins APT Repository

Now add the Jenkins repository to your APT sources:

```bash
# Add Jenkins repository with signed packages
echo "deb [signed-by=/usr/share/keyrings/jenkins-keyring.asc] \
  https://pkg.jenkins.io/debian binary/" | sudo tee \
  /etc/apt/sources.list.d/jenkins.list > /dev/null
```

**What this does**:
- Creates a new apt source file for Jenkins
- `[signed-by=/usr/share/keyrings/jenkins-keyring.asc]` tells apt to use the GPG key we added
- `https://pkg.jenkins.io/debian` is the official Jenkins package repository
- `binary/` specifies we want pre-built packages (not source packages)

### Step 5: Update Package Lists Again

Now that we've added the Jenkins repository, update package lists again:

```bash
# Update package lists to include Jenkins packages
sudo apt update
```

**Expected output**:
```
Hit:1 https://pkg.jenkins.io/debian binary/ InRelease [~3.5 kB]
Building dependency tree... Done
jenkins is now available for installation.
```

### Step 6: Install Jenkins

Now install Jenkins:

```bash
# Install Jenkins
# -y: automatic yes to prompts
sudo apt install -y jenkins
```

**What this does**: Installs the Jenkins WAR (Web Application Archive) file and sets up:
- Jenkins service configuration
- Log files in `/var/log/jenkins/`
- Home directory in `/var/lib/jenkins/`
- User and group named `jenkins`

**Expected output**:
```
Setting up jenkins (2.440.1) ...
Running from: /usr/share/jenkins/jenkins.war
Jenkins startup is in progress
Jenkins fully up and running
```

### Step 7: Enable and Start Jenkins Service

Configure Jenkins to start automatically on boot and start it now:

```bash
# Enable Jenkins to start on boot AND start it immediately
# --now: start the service immediately in addition to enabling it
sudo systemctl enable --now jenkins
```

**What this does**:
- `systemctl enable jenkins` - Creates symlinks so Jenkins starts on boot
- `--now` - Also starts Jenkins right now (no reboot needed)

**Expected output**: No output (success)

Verify Jenkins is running:

```bash
# Check Jenkins service status
sudo systemctl status jenkins
```

**Expected output**:
```
● jenkins.service - Jenkins Automation Server
     Loaded: loaded (/lib/systemd/system/jenkins.service; enabled; vendor preset: enabled)
     Active: active (running) since Mon 2024-01-15 10:30:00 UTC; 5s ago
   Main PID: 1234 (java)
      CGroup: /lib/systemd/system/jenkins.service
              └─1234 /usr/bin/java -Djava.awt.headless=true -jar /usr/share/jenkins/jenkins.war
```

### Step 8: Open Firewall Port (If Enabled)

If you're using UFW firewall, allow port 8080:

```bash
# Allow incoming traffic on port 8080
sudo ufw allow 8080/tcp
# Reload firewall to apply changes
sudo ufw reload
```

**What this does**: Opens TCP port 8080 (the default Jenkins port) so you can access the web UI.

## Accessing Jenkins

Jenkins is now running! Access the web interface:

1. Open your browser
2. Navigate to: `http://your-server-ip:8080`
3. You'll see the Jenkins setup wizard

## Troubleshooting

### Java Not Found

If you see Java errors, ensure JAVA_HOME is set:

```bash
# Find Java installation path
sudo update-alternatives --config java
# Usually /usr/lib/jvm/java-17-openjdk-amd64
```

Add to `/etc/environment`:
```
JAVA_HOME="/usr/lib/jvm/java-17-openjdk-amd64"
```

### Port Already in Use

If port 8080 is taken, change it in `/etc/default/jenkins`:

```bash
# Edit Jenkins configuration
sudo nano /etc/default/jenkins
# Find HTTP_PORT and change it
# Then restart: sudo systemctl restart jenkins
```

### Permission Issues

If jobs fail with permission errors:

```bash
# Fix Jenkins home directory permissions
sudo chown -R jenkins:jenkins /var/lib/jenkins
sudo chown -R jenkins:jenkins /var/log/jenkins
```

## Next Steps

- **[Complete the Setup Wizard](04-initial-setup-wizard.md)** - Configure your new Jenkins instance
- **[Install Jenkins with Docker](03-install-with-docker.md)** - Alternative containerized installation
