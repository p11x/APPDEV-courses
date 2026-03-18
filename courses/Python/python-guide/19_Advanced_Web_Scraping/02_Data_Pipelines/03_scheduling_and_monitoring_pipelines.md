# ⏰ Scheduling and Monitoring Pipelines

> Run your scraper automatically and know when it breaks.

## 🎯 What You'll Learn

- Scheduling options: schedule, APScheduler, cron, GitHub Actions
- GitHub Actions as a free cloud scheduler
- Logging and monitoring your pipeline
- Detecting website changes and alerting
- Error recovery with checkpoints

## 📦 Prerequisites

- Completion of [02_storing_and_managing_scraped_data.md](./02_storing_and_managing_scraped_data.md)
- Understanding of SQLite for storing run metadata
- Basic knowledge of logging in Python

---

## Scheduling Options

| Tool | Best For | Limitations |
|------|----------|-------------|
| **schedule** | Simple hobby scripts | Single machine, simple needs |
| **APScheduler** | More complex scheduling | More setup |
| **cron** | Linux/macOS system jobs | Platform-specific |
| **GitHub Actions** | Free cloud scheduling | 6-hour minimum interval |

---

## Simple Scheduling with schedule Library

```bash
pip install schedule
```

```python
import schedule
import time
import httpx
from datetime import datetime


def scrape_job():
    """The job to run on schedule."""
    
    print(f"🔄 Starting scrape at {datetime.now()}")
    
    try:
        # Your scraping logic here
        response = httpx.get("https://example.com")
        
        # Process data...
        
        print(f"✅ Scrape complete: {response.status_code}")
        
    except Exception as e:
        print(f"❌ Scrape failed: {e}")


def run_scheduler():
    """Run the scheduler loop."""
    
    # Schedule jobs
    schedule.every().hour.do(scrape_job)           # Every hour
    schedule.every().day.at("08:00").do(scrape_job)  # Every day at 8am
    
    # Run continuously
    while True:
        schedule.run_pending()  # Check if any jobs need to run
        time.sleep(60)  # Check every minute


# Usage
# Uncomment to run:
# run_scheduler()

print("Scheduler configured. Jobs will run at:")
print("  - Every hour")
print("  - Every day at 08:00")
```

### 💡 Explanation

- `schedule.every().hour.do(job)` — runs job every hour
- `schedule.every().day.at("08:00").do(job)` — runs daily at specific time
- `schedule.run_pending()` — checks if it's time to run a job

---

## GitHub Actions (Free Cloud Scheduling)

GitHub Actions can run your scraper on a schedule for free:

### Creating the Workflow File

```yaml
# .github/workflows/scrape.yml
name: Daily Scrape

on:
  # Run on a schedule (every day at 8am UTC)
  schedule:
    - cron: '0 8 * * *'
  
  # Also allow manual trigger
  workflow_dispatch:

jobs:
  scrape:
    runs-on: ubuntu-latest
    
    steps:
      - name: Checkout code
        uses: actions/checkout@v4
      
      - name: Set up Python
        uses: actions/setup-python@v5
        with:
          python-version: '3.12'
      
      - name: Install dependencies
        run: |
          pip install -r requirements.txt
      
      - name: Run scraper
        run: python scraper.py
        env:
          # Secrets are available as env vars
          TELEGRAM_BOT_TOKEN: ${{ secrets.TELEGRAM_BOT_TOKEN }}
          TELEGRAM_CHAT_ID: ${{ secrets.TELEGRAM_CHAT_ID }}
      
      - name: Upload data as artifact
        uses: actions/upload-artifact@v4
        with:
          name: scraped-data
          path: data/
          retention-days: 7
      
      - name: Commit data changes
        uses: stefanzweifel/git-auto-commit-action@v5
        with:
          commit_message: "Update scraped data"
          file_pattern: data/*
```

### 💡 Explanation

- `on: schedule: - cron: '0 8 * * *'` — runs daily at 8am UTC
- `workflow_dispatch` — allows manual trigger from GitHub UI
- `actions/upload-artifact` — saves scraped data as downloadable file
- `git-auto-commit-action` — commits data back to repo

### Adding Secrets

1. Go to your repo on GitHub
2. Settings → Secrets and variables → Actions
3. Add secrets like `TELEGRAM_BOT_TOKEN`

---

## Logging Your Pipeline

```python
import logging
from datetime import datetime
from pathlib import Path


def setup_logging(log_file: str = "scraper.log") -> logging.Logger:
    """Configure logging to file and console."""
    
    # Create logger
    logger = logging.getLogger("scraper")
    logger.setLevel(logging.DEBUG)
    
    # File handler - detailed logs
    file_handler = logging.FileHandler(log_file)
    file_handler.setLevel(logging.DEBUG)
    file_format = logging.Formatter(
        "%(asctime)s | %(levelname)-8s | %(message)s",
        datefmt="%Y-%m-%d %H:%M:%S"
    )
    file_handler.setFormatter(file_format)
    
    # Console handler - info and above
    console_handler = logging.StreamHandler()
    console_handler.setLevel(logging.INFO)
    console_format = logging.Formatter("%(levelname)-8s | %(message)s")
    console_handler.setFormatter(console_format)
    
    # Add handlers
    logger.addHandler(file_handler)
    logger.addHandler(console_handler)
    
    return logger


def run_scraper_with_logging():
    """Run scraper with full logging."""
    
    logger = setup_logging()
    
    logger.info("Starting scraper")
    start_time = datetime.now()
    
    try:
        # Simulate scraping
        logger.debug("Fetching URLs...")
        
        # ... scraping logic ...
        
        logger.info(f"Scraped 100 products")
        
        duration = (datetime.now() - start_time).total_seconds()
        logger.info(f"Completed in {duration:.1f} seconds")
        
    except Exception as e:
        logger.exception(f"Scraper failed: {e}")
        raise
    
    finally:
        logger.info("=" * 50)


# Usage
run_scraper_with_logging()
```

### 💡 Explanation

- `logging.FileHandler` — writes detailed logs to file
- `logging.StreamHandler` — prints to console
- `logger.exception()` — logs full traceback
- Different levels: DEBUG, INFO, WARNING, ERROR, CRITICAL

---

## Monitoring with Run Metadata

```python
import sqlite3
from datetime import datetime
from dataclasses import dataclass


@dataclass
class RunMetadata:
    """Metadata about a scrape run."""
    
    run_id: int
    start_time: datetime
    source: str
    records_fetched: int = 0
    errors: int = 0
    duration_seconds: float = 0.0
    status: str = "running"


class RunMonitor:
    """Monitor and track scrape runs."""
    
    def __init__(self, db_path: str = "monitor.db"):
        self.db_path = db_path
        self._init_db()
    
    def _init_db(self) -> None:
        """Initialize monitoring database."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS scrape_runs (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                start_time TEXT NOT NULL,
                source TEXT NOT NULL,
                records_fetched INTEGER DEFAULT 0,
                errors INTEGER DEFAULT 0,
                duration_seconds REAL DEFAULT 0,
                status TEXT DEFAULT 'running',
                error_message TEXT
            )
        """)
        
        conn.commit()
        conn.close()
    
    def start_run(self, source: str) -> RunMetadata:
        """Start tracking a new run."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        start_time = datetime.now().isoformat()
        
        cursor.execute("""
            INSERT INTO scrape_runs (start_time, source, status)
            VALUES (?, ?, 'running')
        """, (start_time, source))
        
        run_id = cursor.lastrowid
        conn.commit()
        conn.close()
        
        return RunMetadata(
            run_id=run_id,
            start_time=datetime.now(),
            source=source
        )
    
    def end_run(self, meta: RunMetadata) -> None:
        """Mark run as complete."""
        
        duration = (datetime.now() - meta.start_time).total_seconds()
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            UPDATE scrape_runs
            SET records_fetched = ?,
                errors = ?,
                duration_seconds = ?,
                status = ?
            WHERE id = ?
        """, (
            meta.records_fetched,
            meta.errors,
            duration,
            "success" if meta.errors == 0 else "failed",
            meta.run_id
        ))
        
        conn.commit()
        conn.close()
    
    def record_error(self, meta: RunMetadata, error: str) -> None:
        """Record an error during the run."""
        
        meta.errors += 1
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            UPDATE scrape_runs
            SET errors = ?, error_message = ?
            WHERE id = ?
        """, (meta.errors, error[:500], meta.run_id))
        
        conn.commit()
        conn.close()
    
    def get_recent_runs(self, limit: int = 10) -> list[dict]:
        """Get recent run history."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            SELECT 
                id,
                start_time,
                source,
                records_fetched,
                errors,
                duration_seconds,
                status
            FROM scrape_runs
            ORDER BY start_time DESC
            LIMIT ?
        """, (limit,))
        
        rows = cursor.fetchall()
        conn.close()
        
        return [
            {
                "id": row[0],
                "start_time": row[1],
                "source": row[2],
                "records": row[3],
                "errors": row[4],
                "duration": row[5],
                "status": row[6]
            }
            for row in rows
        ]
    
    def check_health(self) -> dict:
        """Check overall scraper health."""
        
        recent = self.get_recent_runs(5)
        
        if not recent:
            return {"status": "unknown", "message": "No runs recorded"}
        
        failed = sum(1 for r in recent if r["status"] == "failed")
        success_rate = (len(recent) - failed) / len(recent) * 100
        
        return {
            "status": "healthy" if success_rate >= 80 else "degraded",
            "success_rate": f"{success_rate:.0f}%",
            "recent_runs": recent
        }


# Usage
monitor = RunMonitor()

# Track a run
meta = monitor.start_run("https://example.com/products")

try:
    # Do scraping...
    meta.records_fetched = 100
    
except Exception as e:
    monitor.record_error(meta, str(e))
    print(f"Error: {e}")

finally:
    monitor.end_run(meta)

# Check health
health = monitor.check_health()
print(f"Status: {health['status']}")
print(f"Success rate: {health['success_rate']}")
```

---

## Alerting on Failure

### Telegram Alert

```python
import httpx
from typing import Optional


class TelegramAlerter:
    """Send alerts via Telegram."""
    
    def __init__(self, bot_token: str, chat_id: str):
        self.bot_token = bot_token
        self.chat_id = chat_id
    
    def send(self, message: str) -> None:
        """Send a message to Telegram."""
        
        url = f"https://api.telegram.org/bot{self.bot_token}/sendMessage"
        
        data = {
            "chat_id": self.chat_id,
            "text": message,
            "parse_mode": "HTML"  # Allow formatting
        }
        
        response = httpx.post(url, data=data)
        response.raise_for_status()
        
        print(f"📱 Alert sent: {message[:50]}...")
    
    def send_error(self, job_name: str, error: str) -> None:
        """Send error alert."""
        
        message = f"🔴 <b>Scraper Failed</b>\n\n"
        message += f"Job: {job_name}\n"
        message += f"Error: {error[:200]}"
        
        self.send(message)
    
    def send_success(self, job_name: str, records: int, duration: float) -> None:
        """Send success alert."""
        
        message = f"🟢 <b>Scraper Complete</b>\n\n"
        message += f"Job: {job_name}\n"
        message += f"Records: {records}\n"
        message += f"Duration: {duration:.1f}s"
        
        self.send(message)


# Usage
# alerter = TelegramAlerter(
#     bot_token="123456:ABC-DEF1234ghIkl-zyx57W2v1u123ew11",
#     chat_id="123456789"
# )
#
# alerter.send_error("product_scraper", "Connection timeout")
# alerter.send_success("product_scraper", 100, 45.5)
```

---

## Detecting Website Changes

```python
from bs4 import BeautifulSoup
import hashlib


class StructureMonitor:
    """Monitor website structure for changes."""
    
    def __init__(self, db_path: str = "structure.db"):
        self.db_path = db_path
        self._init_db()
    
    def _init_db(self) -> None:
        """Initialize database."""
        
        import sqlite3
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS structure_hashes (
                url TEXT PRIMARY KEY,
                content_hash TEXT NOT NULL,
                last_checked TEXT NOT NULL
            )
        """)
        
        conn.commit()
        conn.close()
    
    def compute_hash(self, html: str) -> str:
        """Compute hash of HTML content."""
        
        # Remove dynamic content (timestamps, etc)
        soup = BeautifulSoup(html, "html.parser")
        
        # Remove script and style tags
        for tag in soup(["script", "style", "time"]):
            tag.decompose()
        
        # Get stable content
        content = str(soup)
        
        # Hash it
        return hashlib.md5(content.encode()).hexdigest()
    
    def check_for_changes(self, url: str, html: str) -> dict:
        """Check if page structure has changed."""
        
        import sqlite3
        from datetime import datetime
        
        current_hash = self.compute_hash(html)
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            SELECT content_hash, last_checked
            FROM structure_hashes
            WHERE url = ?
        """, (url,))
        
        row = cursor.fetchone()
        
        if row is None:
            # First time seeing this URL
            cursor.execute("""
                INSERT INTO structure_hashes (url, content_hash, last_checked)
                VALUES (?, ?, ?)
            """, (url, current_hash, datetime.now().isoformat()))
            
            conn.commit()
            conn.close()
            
            return {"changed": False, "first_seen": True}
        
        old_hash, last_checked = row
        
        if old_hash != current_hash:
            # Structure changed!
            cursor.execute("""
                UPDATE structure_hashes
                SET content_hash = ?, last_checked = ?
                WHERE url = ?
            """, (current_hash, datetime.now().isoformat(), url))
            
            conn.commit()
            conn.close()
            
            return {"changed": True, "first_seen": False}
        
        conn.close()
        
        return {"changed": False, "first_seen": False}


# Usage
monitor = StructureMonitor()

response = httpx.get("https://example.com")
result = monitor.check_for_changes("https://example.com", response.text)

if result["changed"]:
    print("⚠️  Website structure has changed! Check selectors.")
else:
    print("✅ Structure unchanged")
```

---

## Full Monitoring Wrapper

```python
import logging
from datetime import datetime
from typing import Callable


class MonitoredPipeline:
    """Wrapper that adds monitoring to any pipeline."""
    
    def __init__(
        self,
        name: str,
        alerter: TelegramAlerter | None = None,
        logger: logging.Logger | None = None
    ):
        self.name = name
        self.alerter = alerter
        self.logger = logger or logging.getLogger(name)
        self.monitor = RunMonitor()
    
    def run(self, pipeline_func: Callable) -> None:
        """Run a pipeline with full monitoring."""
        
        # Start monitoring
        meta = self.monitor.start_run(self.name)
        self.logger.info(f"Starting {self.name}")
        
        start_time = datetime.now()
        
        try:
            # Run the actual pipeline
            pipeline_func()
            
            # Record success
            meta.records_fetched = 100  # Would come from pipeline
            duration = (datetime.now() - start_time).total_seconds()
            
            self.logger.info(f"Completed in {duration:.1f}s")
            self.monitor.end_run(meta)
            
            # Send alert if configured
            if self.alerter:
                self.alerter.send_success(
                    self.name,
                    meta.records_fetched,
                    duration
                )
            
        except Exception as e:
            # Record failure
            self.logger.exception(f"Failed: {e}")
            self.monitor.record_error(meta, str(e))
            self.monitor.end_run(meta)
            
            # Always alert on failure
            if self.alerter:
                self.alerter.send_error(self.name, str(e))
            
            raise


# Usage
def my_scraper_pipeline():
    """Your actual scraping code."""
    # ... scrape ...
    pass


# Setup
# alerter = TelegramAlerter(token, chat_id)
pipeline = MonitoredPipeline("product_scraper", alerter=None)
pipeline.run(my_scraper_pipeline)
```

---

## Summary

✅ **schedule** — simple scheduling for hobby scripts

✅ **GitHub Actions** — free cloud scheduling with artifacts

✅ **Logging** — use Python logging module with file + console

✅ **Run monitoring** — track runs in SQLite with metadata

✅ **Alerting** — send Telegram messages on success/failure

✅ **Structure monitoring** — detect when website changes

---

## ➡️ Next Steps

Continue to [03_Scraping_Projects/01_project_price_tracker.md](../03_Scraping_Projects/01_project_price_tracker.md) to build a complete price tracker project.

---

## 🔗 Further Reading

- [schedule Library](https://schedule.readthedocs.io/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Python Logging Documentation](https://docs.python.org/3/library/logging.html)
