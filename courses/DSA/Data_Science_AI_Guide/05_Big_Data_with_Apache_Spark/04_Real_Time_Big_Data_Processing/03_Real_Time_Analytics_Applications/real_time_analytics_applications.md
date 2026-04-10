# Real-Time Analytics Applications

## I. INTRODUCTION

### What are Real-Time Analytics Applications?
Real-time analytics applications process data as it arrives to generate immediate insights and actions. Using Spark Streaming and Structured Streaming, organizations can build dashboards, detect anomalies, and trigger automated responses based on live data streams from various sources.

### Why is it Important in Big Data?
Real-time analytics enables immediate business responses. It supports fraud detection, operational monitoring, and customer experience optimization. It provides competitive advantages through faster decision-making.

## II. IMPLEMENTATION

```python
"""
Real-Time Analytics Applications
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F

spark = SparkSession.builder \
    .appName("AnalyticsDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("REAL-TIME ANALYTICS APPLICATIONS")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: REAL-TIME DASHBOARD
# ============================================================================

print("\n1. REAL-TIME DASHBOARD METRICS")
print("-" * 50)

# Simulate clickstream data
clicks = [(i, f"page_{i % 10}", f"user_{i % 100}") for i in range(1000)]
df_clicks = spark.createDataFrame(clicks, ["id", "page", "user"])

# Real-time metrics
metrics = df_clicks.groupBy("page").agg(
    F.count("*").alias("views"),
    F.countDistinct("user").alias("unique_users")
).orderBy(F.desc("views"))

print("\nReal-time page views:")
metrics.show(10)

# ============================================================================
# EXAMPLE 2: ANOMALY DETECTION
# ============================================================================

print("\n2. ANOMALY DETECTION")
print("-" * 50)

# Transaction data
transactions = [(i, random.randint(1, 1000), 100 + i % 500) for i in range(100)]
df_txn = spark.createDataFrame(transactions, ["id", "user_id", "amount"])

# Calculate baseline statistics
stats = df_txn.agg(
    F.avg("amount").alias("mean"),
    F.stddev("amount").alias("stddev")
).collect()[0]

mean = stats["mean"]
stddev = stats["stddev"]

# Flag anomalies (2 std deviations)
threshold = mean + 2 * stddev
anomalies = df_txn.filter(F.col("amount") > threshold)

print(f"\nThreshold: {threshold:.2f}")
print(f"Anomalies detected: {anomalies.count()}")

# ============================================================================
# EXAMPLE 3: STREAMS JOIN FOR ANALYTICS
# ============================================================================

print("\n3. JOIN STREAMS FOR ANALYTICS")
print("-" * 50)

# User events
events = [(1, "login", "2024-01-01"), (2, "buy", "2024-01-01")]
df_events = spark.createDataFrame(events, ["user_id", "action", "date"])

# User profiles
profiles = [(1, "John", "Premium"), (2, "Jane", "Basic")]
df_profiles = spark.createDataFrame(profiles, ["user_id", "name", "tier"])

# Join for enriched analytics
enriched = df_events.join(df_profiles, "user_id")
print("\nEnriched user events:")
enriched.show()

# ============================================================================
# EXAMPLE 4: TIME-SERIES AGGREGATIONS
# ============================================================================

print("\n4. TIME-SERIES AGGREGATIONS")
print("-" * 50)

# Time-series data
ts = [(f"2024-01-01 {i:02d}:00:00", random.randint(10, 100)) for i in range(24)]
df_ts = spark.createDataFrame(ts, ["timestamp", "value"])

# Hourly aggregations
df_agg = df_ts.groupBy("timestamp").agg(
    F.sum("value").alias("total"),
    F.avg("value").alias("average")
)

print("\nHourly aggregations:")
df_agg.show(5)

# ============================================================================
# EXAMPLE 5: RATE ALERTS
# ============================================================================

print("\n5. RATE ALERTS")
print("-" * 50)

# Calculate rates
rates = df_clicks.groupBy("page").agg(
    F.count("*").alias("total_clicks")
)

# Alert on threshold
alert_threshold = 50
alerts = rates.filter(F.col("total_clicks") > alert_threshold)

print(f"\nAlerts (> {alert_threshold} clicks):")
alerts.show()

# ============================================================================
# EXAMPLE 6: SESSION ANALYSIS
# ============================================================================

print("\n6. SESSION ANALYSIS")
print("-" * 50)

# Session events
sessions = [
    ("s1", 1, "view"),
    ("s1", 2, "cart"),
    ("s1", 3, "buy"),
    ("s2", 1, "view"),
    ("s2", 2, "view"),
]
df_sessions = spark.createDataFrame(sessions, ["session_id", "step", "action"])

# Session funnel
funnel = df_sessions.groupBy("session_id").pivot("action").count().fillna(0)

print("\nSession funnel:")
funnel.show()

print("\n" + "=" * 70)
print("REAL-TIME ANALYTICS COMPLETE")
print("=" * 70)
```

## III. CONCLUSION

Real-time analytics enables immediate insights and actions.
