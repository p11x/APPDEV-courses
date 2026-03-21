# Install Jenkins with Docker

## What this covers

This guide walks through running Jenkins in Docker using the official `jenkins/jenkins:lts-jdk17` image. You'll learn how to persist Jenkins data using volumes, access the initial admin password, and understand Docker-in-Docker scenarios for advanced CI/CD workflows.

## Prerequisites

- Docker installed on your machine (see [Docker Desktop](https://www.docker.com/products/docker-desktop) for Windows/macOS or your Linux distribution's package manager)
- At least 4GB of RAM available for Docker
- Basic understanding of Docker concepts (images, containers, volumes)

## Why Run Jenkins in Docker?

Running Jenkins in Docker provides several benefits:

1. **Isolation**: Jenkins runs in its own container, separate from your host system
2. **Consistency**: Same environment across different machines
3. **Easy Updates**: Pull a new image to upgrade Jenkins
4. **Portability**: Move your Jenkins instance to any Docker host
5. **Clean Testing**: Spin up isolated test environments

## Step-by-Step Installation

### Step 1: Pull the Jenkins Image

Pull the official Jenkins LTS image with JDK 17:

```bash
# Pull the Jenkins LTS image with JDK 17
# -d: run container in detached mode (background)
# jenkins/jenkins:lts-jdk17: specific tag for Long Term Support with Java 17
docker pull jenkins/jenkins:lts-jdk17
```

**What this does**:
- Downloads the official Jenkins Docker image from Docker Hub
- `lts-jdk17` is the Long Term Support version with Java 17 (required for Jenkins 2.440+)

**Expected output**:
```
latest: Pulling from jenkins/jenkins
Digest: sha256:abc123...
Status: Image is up to date for jenkins/jenkins:lts-jdk17
```

### Step 2: Create a Docker Network (Optional but Recommended)

Create a network for Jenkins and its agents:

```bash
# Create a custom bridge network for Jenkins
# This allows containers to communicate by name
docker network create jenkins-network
```

**What this does**: Creates a Docker network so Jenkins can communicate with other containers (like Docker agents) using container names instead of IP addresses.

### Step 3: Run Jenkins Container

Run Jenkins with the necessary configuration:

```bash
# Run Jenkins container with volume persistence
# -d: detached mode (runs in background)
# -p 8080:8080:8080: maps host port 8080 to container port 8080 (Jenkins web UI)
# -p 50000:50000: maps host port 50000 to container port 50000 (Jenkins agent port)
# -v jenkins_home:/var/jenkins_home: persists Jenkins data across container restarts
# --name jenkins: names the container "jenkins" for easy reference
# -u root: runs as root user (needed for Docker-in-Docker scenarios)
# --network jenkins-network: attaches to custom network
# jenkins/jenkins:lts-jdk17: the image to run
docker run -d \
  -p 8080:8080 \
  -p 50000:50000 \
  -v jenkins_home:/var/jenkins_home \
  --name jenkins \
  -u root \
  --network jenkins-network \
  jenkins/jenkins:lts-jdk17
```

**What this does**:
| Flag | Explanation |
|------|-------------|
| `-d` | Run in detached mode (background) |
| `-p 8080:8080` | Map host port 8080 to container port 8080 (web UI) |
| `-p 50000:50000` | Map host port 50000 for Jenkins agent communication |
| `-v jenkins_home:/var/jenkins_home` | Create/use volume `jenkins_home` mounted at `/var/jenkins_home` |
| `--name jenkins` | Name the container "jenkins" |
| `-u root` | Run as root user (important for Docker-in-Docker) |
| `--network jenkins-network` | Attach to custom Docker network |

### Step 4: Get Initial Admin Password

Jenkins requires an initial admin password to unlock. Get it from the container logs:

```bash
# View Jenkins container logs
# --since 60s: show logs from the last 60 seconds
# tail -1: show only the last line
docker logs jenkins --since 60s | tail -20
```

Look for this line in the output:

```
Jenkins initial setup is required. An admin user has been created and a password generated.
Please use the following password to proceed to installation:

<PASSWORD>

This may also be found at: /var/jenkins_home/secrets/initialAdminPassword
```

**What this does**: Shows the Jenkins startup logs where the initial admin password is displayed.

Alternative method - read directly from the container:

```bash
# Read the initial admin password file from inside the container
# exec: run a command inside a running container
docker exec jenkins cat /var/jenkins_home/secrets/initialAdminPassword
```

**What this does**: Executes `cat` inside the running `jenkins` container to read the password file.

### Step 5: Access Jenkins Web Interface

Open your browser and navigate to:

```
http://localhost:8080
```

You'll see the Jenkins unlock screen. Enter the password you retrieved in Step 4.

## Understanding Volume Persistence

The `-v jenkins_home:/var/jenkins_home` flag is crucial:

```
┌─────────────────────────────────────────────────────────┐
│                    Docker Host                            │
│                                                          │
│  ┌─────────────────────────────────────────────────────┐ │
│  │              jenkins_home Volume                     │ │
│  │  (Persistent storage that survives container restart)│ │
│  │                                                      │ │
│  │  ├── jobs/          # Job configurations             │ │
│  │  ├── plugins/      # Installed plugins              │ │
│  │  ├── secrets/      # Credentials and keys           │ │
│  │  ├── workspace/    # Build workspaces               │ │
│  │  └── users/        # Jenkins users                  │ │
│  └─────────────────────────────────────────────────────┘ │
│                           │                               │
│                    Mounted at                             │
│                           ▼                               │
│  ┌─────────────────────────────────────────────────────┐ │
│  │              Jenkins Container                       │ │
│  │  /var/jenkins_home/  (inside container)            │ │
│  └─────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
```

**Why this matters**: Without the volume, all Jenkins data (jobs, builds, plugins, credentials) would be lost when the container is removed. The volume ensures persistence.

## Common Docker Commands

### Stop Jenkins

```bash
# Stop the Jenkins container
docker stop jenkins
```

### Start Jenkins

```bash
# Start the Jenkins container
docker start jenkins
```

### Restart Jenkins

```bash
# Restart Jenkins (stop then start)
docker restart jenkins
```

### View Logs

```bash
# Follow Jenkins logs in real-time
# -f: follow (like tail -f)
docker logs -f jenkins
```

### Access Container Shell

```bash
# Open a shell inside the Jenkins container
# -i: interactive, -t: allocate a pseudo-TTY
docker exec -it jenkins /bin/bash
```

### Remove and Recreate

```bash
# Completely remove Jenkins container (data is safe in volume)
docker rm -f jenkins

# Recreate with same configuration
docker run -d \
  -p 8080:8080 \
  -p 50000:50000 \
  -v jenkins_home:/var/jenkins_home \
  --name jenkins \
  -u root \
  --network jenkins-network \
  jenkins/jenkins:lts-jdk17
```

## Docker-in-Docker (DinD) Considerations

If you need Jenkins to build Docker images (use Docker commands inside jobs), you'll need Docker-in-Docker setup:

### Option 1: Mount Docker Socket

Mount the host's Docker socket into the Jenkins container:

```bash
# Run with Docker socket mounted
# -v /var/run/docker.sock:/var/run/docker.sock: shares host's Docker daemon
docker run -d \
  -p 8080:8080 \
  -p 50000:50000 \
  -v jenkins_home:/var/jenkins_home \
  -v /var/run/docker.sock:/var/run/docker.sock \
  --name jenkins \
  -u root \
  jenkins/jenkins:lts-jdk17
```

**⚠️ Security Warning**: This gives the Jenkins container root access to the host's Docker daemon. Only use in development/isolated environments.

### Option 2: Docker-in-Docker (dind)

Run a separate Docker daemon inside the container:

```bash
# Use the dind (Docker-in-Docker) image variant
docker run -d \
  -p 8080:8080 \
  -p 50000:50000 \
  -v jenkins_home:/var/jenkins_home \
  --name jenkins \
  --privileged \
  jenkins/jenkins:lts-jdk17-dind
```

**What this does**: The container runs its own Docker daemon, completely isolated from the host.

## Next Steps

- **[Complete the Setup Wizard](04-initial-setup-wizard.md)** - Configure your new Jenkins instance
- **[Install on Ubuntu](02-install-on-ubuntu.md)** - Native installation alternative
