# ⏰ Scheduled Tasks

## 🎯 What You'll Learn

- Using schedule library
- APScheduler for advanced scheduling
- System cron jobs

---

## schedule Library

```bash
pip install schedule
```

```python
import schedule
import time

def job():
    print("Running scheduled task!")

# Schedule jobs
schedule.every().day.at("09:00").do(job)
schedule.every(30).minutes.do(job)
schedule.every().monday.do(job)

# Run the scheduler
while True:
    schedule.run_pending()
    time.sleep(1)
```

---

## APScheduler

```bash
pip install apscheduler
```

```python
from apscheduler.schedulers.background import BackgroundScheduler

scheduler = BackgroundScheduler()

def job():
    print("Running!")

# Add job
scheduler.add_job(job, 'interval', minutes=30)
scheduler.add_job(job, 'cron', hour=9, minute=0)

scheduler.start()
```

---

## Cron Syntax

```
# ┌───────────── minute (0-59)
# │ ┌───────────── hour (0-23)
# │ │ ┌───────────── day of month (1-31)
# │ │ │ ┌───────────── month (1-12)
# │ │ │ │ ┌───────────── day of week (0-6, Sunday=0)
# │ │ │ │ │
# * * * * *

# Examples:
0 9 * * *        # Every day at 9:00
*/15 * * * *     # Every 15 minutes
0 0 * * 0        # Every Sunday at midnight
```

### Crontab Command

```bash
crontab -e        # Edit crontab
crontab -l        # List crontab
```

---

## ✅ Summary

- schedule: simple in-process scheduling
- APScheduler: advanced features, persistent jobs
- cron: system-level scheduling on Unix

## 🔗 Further Reading

- [schedule library](https://schedule.readthedocs.io/)
- [APScheduler](https://apscheduler.readthedocs.io/)
