# Log Parsing

## What You'll Learn

- Parsing application logs
- Extracting structured data
- Analyzing log patterns

## Prerequisites

- Completed `06-csv-processing.md`

## Common Log Formats

```python
import re
from dataclasses import dataclass
from datetime import datetime

# Apache Common Log Format
# 127.0.0.1 - - [10/Oct/2000:13:55:36 -0700] "GET /apache_pb.gif HTTP/1.0" 200 2326

APACHE_PATTERN = r'(\S+) (\S+) (\S+) \[([^\]]+)\] "(\S+) (\S+)[^"]*" (\d+) (\d+)'

# nginx error log
# 2024/01/15 10:30:45 [error] 1234#1234: *5678 connection timed out

NGINX_ERROR_PATTERN = r'(\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}) \[(\w+)\] \d+#\d+: \*(\d+) (.+)'

# Python logging
# 2024-01-15 10:30:45,INFO:__main__:Application started

PYTHON_LOG_PATTERN = r'(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}),(\w+):(\S+):(.+)'
```

## Parsing Application Logs

```python
from dataclasses import dataclass
from datetime import datetime

@dataclass
class LogEntry:
    timestamp: datetime
    level: str
    logger: str
    message: str
    extra: dict = None

def parse_log_line(line: str) -> LogEntry | None:
    """Parse a Python log line."""
    pattern = r'(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}),(\w+):(\S+):(.+)'
    match = re.match(pattern, line.strip())
    
    if not match:
        return None
    
    timestamp_str, level, logger, message = match.groups()
    timestamp = datetime.strptime(timestamp_str, '%Y-%m-%d %H:%M:%S')
    
    return LogEntry(
        timestamp=timestamp,
        level=level,
        logger=logger,
        message=message.strip()
    )

# Usage
with open("app.log") as f:
    for line in f:
        entry = parse_log_line(line)
        if entry and entry.level == "ERROR":
            print(f"{entry.timestamp}: {entry.message}")
```

## Parsing JSON Logs

```python
import json

def parse_json_log(line: str) -> dict | None:
    """Parse JSON log line."""
    try:
        return json.loads(line)
    except json.JSONDecodeError:
        return None

# Example JSON log
# {"timestamp": "2024-01-15T10:30:45Z", "level": "INFO", "message": "Request processed"}
```

## Analyzing Logs

```python
from collections import Counter, defaultdict
from dataclasses import dataclass

@dataclass
class LogStats:
    total: int
    by_level: Counter
    by_hour: Counter
    errors: list

def analyze_logs(filepath: str) -> LogStats:
    """Analyze log file and return statistics."""
    by_level = Counter()
    by_hour = Counter()
    errors = []
    total = 0
    
    with open(filepath) as f:
        for line in f:
            entry = parse_log_line(line)
            if not entry:
                continue
            
            total += 1
            by_level[entry.level] += 1
            by_hour[entry.timestamp.hour] += 1
            
            if entry.level == "ERROR":
                errors.append(entry.message)
    
    return LogStats(
        total=total,
        by_level=by_level,
        by_hour=by_hour,
        errors=errors
    )

# Usage
stats = analyze_logs("app.log")
print(f"Total: {stats.total}")
print(f"Errors: {stats.by_level['ERROR']}")
```

## Real-Time Log Processing

```python
import time

def tail_log(filepath: str):
    """Follow a log file in real-time."""
    with open(filepath, "r") as f:
        # Start at end of file
        f.seek(0, 2)
        
        while True:
            line = f.readline()
            if not line:
                time.sleep(0.1)
                continue
            
            yield line.strip()

# Usage
for line in tail_log("app.log"):
    entry = parse_log_line(line)
    if entry and entry.level == "ERROR":
        print(f"ERROR: {entry.message}")
```

## Summary

- Use regex for parsing different log formats
- Parse JSON logs with json module
- Analyze logs for patterns and errors

## Next Steps

Continue to `08-string-manipulation-techniques.md`.
