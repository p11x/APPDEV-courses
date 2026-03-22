# Example293: Working with Dates and Times
from datetime import datetime, timedelta, date
import time

# Current datetime
print("DateTime:")
now = datetime.now()
print(f"Now: {now}")
print(f"Today: {datetime.today()}")
print(f"UTC now: {datetime.utcnow()}")

# Parse and format
print("\nParse and Format:")
dt = datetime.strptime("2024-12-25 10:30:00", "%Y-%m-%d %H:%M:%S")
print(f"Parsed: {dt}")
print(f"Formatted: {dt.strftime('%B %d, %Y')}")

# Timedelta
print("\nTimedelta:")
future = now + timedelta(days=10)
print(f"10 days from now: {future}")

past = now - timedelta(hours=24)
print(f"24 hours ago: {past}")

# Date arithmetic
print("\nDate Arithmetic:")
d1 = date(2024, 1, 1)
d2 = date(2024, 12, 31)
diff = d2 - d1
print(f"Days between: {diff.days}")

# Sleep and measure
print("\nTiming:")
start = time.time()
time.sleep(0.1)
end = time.time()
print(f"Elapsed: {end - start:.3f}s")

# Time zones (basic)
print("\nTime Zones:")
from datetime import timezone
utc = timezone.utc
dt_utc = datetime.now(utc)
print(f"UTC: {dt_utc}")
