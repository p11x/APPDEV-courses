# TimescaleDB for Time-Series Data

## What You'll Learn
- What time-series data is and why regular databases struggle with it
- How TimescaleDB extends PostgreSQL for time-series workloads
- Designing schemas for time-series data with automatic partitioning
- Writing and querying time-series data efficiently
- Using continuous aggregates for pre-computed rollups

## Prerequisites
- Completed `03-read-replicas-and-connection-routing.md` — PostgreSQL knowledge
- Completed `05-databases/02-sqlalchemy-orm.md` — SQLAlchemy experience
- Understanding of SQL aggregation functions

## What Is Time-Series Data?

Time-series data is data points indexed by time — sensor readings, stock prices, user events, metrics. The key characteristic: you're primarily querying recent data, but need to retain historical data for analysis.

Examples:
- IoT sensors: temperature, humidity every second
- Application metrics: requests/sec, latency p99, error rates
- Financial: stock prices every millisecond
- User analytics: page views, clicks, sessions

The challenge: millions of rows per day, queries usually filter by time, and you need fast aggregations.

## How TimescaleDB Works

TimescaleDB is a PostgreSQL extension that automatically partitions time-series data into "chunks" (like partitions):

```
Regular PostgreSQL Table:
┌────────────────────────────────────────────┐
│              all_data                       │
│  [1M rows — full table scan for queries!]  │
└────────────────────────────────────────────┘

TimescaleDB Hypertable:
┌────────────────────────────────────────────┐
│            hypertable (logical)              │
├──────────┬──────────┬──────────┬──────────┤
│  chunk_1 │  chunk_2 │  chunk_3 │  chunk_4 │
│  (Jan)   │  (Feb)   │  (Mar)   │  (Apr)   │
└──────────┴──────────┴──────────┴──────────┘
   [100K]     [100K]      [100K]     [100K]
```

Queries automatically target only relevant chunks.

## Setting Up TimescaleDB

```bash
# Install TimescaleDB (Ubuntu/Debian)
sudo apt install timescaledb-2-postgresql-15

# Or use Docker
docker run -d --name timescaledb -p 5432:5432 \
  -e POSTGRES_PASSWORD=password \
  timescale/timescaledb:latest-pg15
```

Enable the extension in PostgreSQL:

```sql
CREATE EXTENSION IF NOT EXISTS timescaledb;
```

## Creating a Hypertable

```python
from sqlalchemy import Column, Integer, BigInteger, Float, Timestamp, text
from sqlalchemy.dialects.postgresql import insert
from sqlalchemy.orm import DeclarativeBase
from sqlalchemy import create_engine
from datetime import datetime, timedelta
import random

engine = create_engine("postgresql://user:pass@localhost:5432/metrics")

class Base(DeclarativeBase):
    pass

class Metric(Base):
    __tablename__ = "metrics"
    
    time = Column(Timestamp, primary_key=True)
    device_id = Column(Integer, primary_key=True)
    temperature = Column(Float)
    humidity = Column(Float)

# Create hypertable (run once)
with engine.connect() as conn:
    conn.execute(text("""
        SELECT create_hypertable(
            'metrics', 
            'time',
            chunk_time_interval => INTERVAL '1 day',
            if_not_exists => TRUE
        )
    """))
    conn.commit()
```

🔍 **Line-by-Line Breakdown:**
1. `create_hypertable('metrics', 'time')` — Converts the regular table to a hypertable, partitioning by the `time` column
2. `chunk_time_interval => INTERVAL '1 day'` — Creates a new chunk every day. Older chunks get archived/compacted
3. `if_not_exists => TRUE` — Safe to run multiple times

## Writing Time-Series Data

```python
from sqlalchemy.orm import sessionmaker
from datetime import datetime, timedelta

Session = sessionmaker(bind=engine)

def write_metrics(device_id: int, metrics: list[dict]):
    """Batch insert metrics for a device."""
    session = Session()
    try:
        for m in metrics:
            session.add(Metric(
                time=m["time"],
                device_id=device_id,
                temperature=m["temperature"],
                humidity=m["humidity"]
            ))
        session.commit()
    finally:
        session.close()

# Simulate 1000 devices, 1 day of data
def generate_test_data():
    base_time = datetime.utcnow().replace(hour=0, minute=0, second=0)
    
    for device_id in range(1000):
        metrics = []
        for hour in range(24):
            for minute in range(60):
                metrics.append({
                    "time": base_time + timedelta(hours=hour, minutes=minute),
                    "temperature": 20 + random.gauss(0, 5),
                    "humidity": 50 + random.gauss(0, 10)
                })
        write_metrics(device_id, metrics)
```

## Efficient Time-Range Queries

TimescaleDB automatically prunes chunks:

```python
def query_hourly_stats(device_id: int, date: datetime) -> list[dict]:
    """Get hourly averages for a device on a specific day."""
    session = Session()
    result = session.execute(text("""
        SELECT 
            time_bucket('1 hour', time) AS hour,
            avg(temperature) AS avg_temp,
            avg(humidity) AS avg_humidity
        FROM metrics
        WHERE device_id = :device_id
          AND time >= :day
          AND time < :day + INTERVAL '1 day'
        GROUP BY hour
        ORDER BY hour
    """), {
        "device_id": device_id,
        "day": date
    })
    
    return [dict(row._mapping) for row in result]
```

🔍 **Line-by-Line Breakdown:**
1. `time_bucket('1 hour', time)` — TimescaleDB function that truncates timestamps to 1-hour buckets (like `date_trunc` but with arbitrary intervals)
2. `GROUP BY hour` — Aggregates into hourly buckets
3. Chunk pruning happens automatically — if querying one day's data, only that day's chunk is scanned

## Continuous Aggregates

For frequently-queried aggregations, pre-compute them:

```python
def create_continuous_aggregate():
    """Create pre-computed hourly aggregates."""
    with engine.connect() as conn:
        # Create continuous aggregate
        conn.execute(text("""
            CREATE MATERIALIZED VIEW metrics_hourly
            WITH (timescaledb.continuous) AS
            SELECT 
                device_id,
                time_bucket('1 hour', time) AS hour,
                avg(temperature) AS avg_temp,
                avg(humidity) AS avg_humidity,
                count(*) AS sample_count
            FROM metrics
            GROUP BY device_id, hour
        """))
        
        # Add refresh policy - refresh last 2 days
        conn.execute(text("""
            SELECT add_continuous_aggregate_policy(
                'metrics_hourly',
                start_offset => INTERVAL '2 days',
                end_offset => INTERVAL '1 hour',
                schedule_interval => INTERVAL '1 hour'
            )
        """))
        conn.commit()
```

Query the pre-computed view:

```python
def query_aggregated(device_id: int, date: datetime):
    """Query pre-computed aggregates - much faster!"""
    with engine.connect() as conn:
        result = conn.execute(text("""
            SELECT * FROM metrics_hourly
            WHERE device_id = :device_id
              AND hour >= :day
              AND hour < :day + INTERVAL '1 day'
            ORDER BY hour
        """), {"device_id": device_id, "day": date})
        return [dict(row._mapping) for row in result]
```

## Compression

TimescaleDB supports compression to reduce storage by 90%:

```python
def enable_compression():
    """Enable compression on older chunks."""
    with engine.connect() as conn:
        conn.execute(text("""
            ALTER TABLE metrics SET (
                timescaledb.compress,
                timescaledb.compress_segmentby = 'device_id'
            )
        """))
        
        # Add compression policy - compress chunks older than 7 days
        conn.execute(text("""
            SELECT add_compression_policy(
                'metrics',
                compress_after => INTERVAL '7 days'
            )
        """))
        conn.commit()
```

## Retention Policies

Automatically drop old data:

```python
def setup_retention():
    """Automatically drop data older than 1 year."""
    with engine.connect() as conn:
        conn.execute(text("""
            SELECT add_retention_policy(
                'metrics',
                drop_after => INTERVAL '1 year'
            )
        """))
        conn.commit()
```

## Production Considerations

- **Compression impact**: Compressed data uses ~10x less storage but takes CPU to compress/decompress. Balance retention vs. compression timing.
- **Chunk sizing**: Smaller chunks = more efficient time queries but more metadata overhead. 1 day is a good default for high-volume data.
- **Continuous aggregate lag**: There's always some lag between raw data and materialized aggregates. For real-time dashboards, query both raw and aggregate and merge.
- **Backfilling**: When backfilling historical data, disable background policies temporarily or you may get lock contention.
- **Scaling**: For massive scale (>1 billion rows), consider TimescaleDB's distributed hypertables (multiple nodes).

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Using integer timestamps instead of proper timestamptz

**Wrong:**
```python
time = Column(Integer)  # Unix epoch as integer!
```

**Why it fails:** Loses timezone info, complicates queries, doesn't work with TimescaleDB functions.

**Fix:**
```python
from sqlalchemy.dialects.postgresql import TIMESTAMP
time = Column(TIMESTAMP(timezone=True), primary_key=True)
```

### ❌ Mistake 2: Not setting up retention policies

**Wrong:** Creating a hypertable and letting data grow forever.

**Why it fails:** Even with compression, you eventually hit storage limits. Queries slow down on huge hypertables.

**Fix:**
```python
conn.execute(text("""
    SELECT add_retention_policy('metrics', INTERVAL '1 year')
"""))
```

### ❌ Mistake 3: Querying without time filters

**Wrong:**
```python
session.query(Metric).filter(Metric.device_id == 1).all()  # Scans ALL chunks!
```

**Why it fails:** Without time filter, TimescaleDB has to scan every chunk. Even with good indexing, this is slow.

**Fix:**
```python
session.query(Metric).filter(
    Metric.device_id == 1,
    Metric.time >= start_time,  # Always include time filter!
    Metric.time < end_time
).all()
```

## Summary

- TimescaleDB extends PostgreSQL with automatic time-based partitioning ("chunks")
- Queries automatically prune irrelevant chunks — fast even with billions of rows
- Use continuous aggregates for pre-computed rollups of common queries
- Enable compression and retention policies to manage storage automatically
- Always filter by time in queries to get chunk pruning benefits

## Next Steps

→ Continue to `05-mongodb-with-motor-async.md` to learn about document databases with async Python drivers.
