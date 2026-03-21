# REX-Ray

## Overview

REX-Ray is a vendor-agnostic volume orchestration plugin for Docker that provides persistent storage for containers across various storage providers.

## Prerequisites

- Docker Engine 20.10+
- Root access

## Core Concepts

### What REX-Ray Provides

- Single API for multiple storage backends
- Snapshot and backup capabilities
- Support for AWS, Azure, Google, OpenStack, etc.

## Step-by-Step Examples

### Installing REX-Ray

```bash
# Install REX-Ray as Docker plugin
docker plugin install rexray/ebs \
  EBS_ACCESSKEY=xxx \
  EBS_SECRETKEY=xxx

# Verify installation
docker plugin ls
```

### Using REX-Ray Volumes

```bash
# Create volume using REX-Ray driver
docker volume create \
  --driver rexray/ebs \
  --name my-data

# Run container with REX-Ray volume
docker run -dit \
  -v my-data:/data \
  nginx
```

### Configuration

```bash
# REX-Ray config file
cat > /etc/rexray/config.yml <<EOF
rexray:
  logLevel: info
  storageDrivers:
    - ebs
  ebs:
    accessKey: xxx
    secretKey: xxx
EOF
```

## Gotchas for Docker Users

- **Plugin maintenance**: Project may have limited support
- **Credential security**: Store credentials securely

## Quick Reference

| Driver | Backend |
|--------|---------|
| rexray/ebs | AWS EBS |
| rexray/gce-dynamic | Google Cloud |
| rexray/azure-ud | Azure |

## What's Next

Continue to [Local Persist Plugin](./02-local-persist.md) for local volume persistence.
