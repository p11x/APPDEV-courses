# Docker Logs

## Overview

Logging is essential for debugging and monitoring containerized applications. Docker provides comprehensive logging mechanisms to capture stdout and stderr from containers, making it easier to troubleshoot issues and monitor application behavior.

## Prerequisites

- Basic Docker knowledge
- Understanding of container concepts

## Core Concepts

### Log Drivers

Docker supports multiple logging drivers:
- **json-file**: Default, JSON format to file
- **syslog**: System logging
- **journald**: Journal logging
- **gelf**: Graylog Extended Log Format
- **fluentd**: Fluentd logging

## Step-by-Step Examples

### Viewing Logs

```bash
# View container logs
docker logs container_name

# Follow logs in real-time
docker logs -f container_name

# Show timestamps
docker logs -t container_name

# Show last N lines
docker logs --tail 100 container_name
```

### Log Configuration

```bash
# Run with specific log driver
docker run \
  --log-driver=syslog \
  --log-opt syslog-address=tcp://localhost:514 \
  nginx
```

### Docker Compose Logging

```yaml
# docker-compose.yml
services:
  web:
    image: nginx
    logging:
      driver: json-file
      options:
        max-size: "10m"
        max-file: "3"
```

## Common Mistakes

- **Not rotating logs**: Logs can fill disk space.
- **Not using log drivers**: Use appropriate drivers for production.

## What's Next

Continue to [Exec and Inspect](./02-exec-and-inspect.md)
