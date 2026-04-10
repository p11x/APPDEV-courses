# Event-Driven Architecture with Spark

## I. INTRODUCTION

### What is Event-Driven Architecture?
Event-driven architecture (EDA) is a design pattern where services communicate through events rather than direct calls. In this architecture, events are generated when something happens (like a user action or sensor reading), and interested services react to those events. Apache Spark Streaming serves as an excellent event processing engine in such architectures.

### Why is it Important in Big Data?
EDA enables loose coupling between services. It supports scalable, resilient systems. It allows asynchronous processing. It provides real-time responsiveness to business events.

## II. IMPLEMENTATION

```python
"""
Event-Driven Architecture with Spark
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F

spark = SparkSession.builder \
    .appName("EventDrivenDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("EVENT-DRIVEN ARCHITECTURE WITH SPARK")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: EVENT PRODUCER PATTERN
# ============================================================================

print("\n1. EVENT PRODUCER PATTERN")
print("-" * 50)

# Simulate events from various producers
events = [
    {"event_id": "e1", "type": "user_registered", "user_id": 1, "ts": "2024-01-01 10:00:00"},
    {"event_id": "e2", "type": "order_placed", "order_id": 101, "ts": "2024-01-01 10:01:00"},
    {"event_id": "e3", "type": "payment_received", "order_id": 101, "ts": "2024-01-01 10:02:00"},
]
df_events = spark.createDataFrame(events)

print("\nEvent stream:")
df_events.show()

# ============================================================================
# EXAMPLE 2: EVENT ROUTING
# ============================================================================

print("\n2. EVENT ROUTING")
print("-" * 50)

# Route events to handlers
def route_event(event_type):
    handlers = {
        "user_registered": "UserHandler",
        "order_placed": "OrderHandler", 
        "payment_received": "PaymentHandler"
    }
    return handlers.get(event_type, "UnknownHandler")

# Apply routing
routed = df_events.withColumn("handler", F.udf(route_event)("type"))
print("\nRouted events:")
routed.show()

# ============================================================================
# EXAMPLE 3: EVENT CORRELATION
# ============================================================================

print("\n3. EVENT CORRELATION")
print("-" * 50)

# Correlate related events
correlated = df_events.groupBy("order_id").agg(
    F.collect_list("type").alias("event_sequence"),
    F.min("ts").alias("start_time"),
    F.max("ts").alias("end_time")
)

print("\nCorrelated events:")
correlated.show()

# ============================================================================
# EXAMPLE 4: COMPLEX EVENT PROCESSING
# ============================================================================

print("\n4. COMPLEX EVENT PROCESSING")
print("-" * 50)

# Detect event patterns
pattern = correlated.withColumn(
    "has_full_flow",
    F.array_contains(F.col("event_sequence"), "order_placed") & 
    F.array_contains(F.col("event_sequence"), "payment_received")
)

print("\nEvent patterns detected:")
pattern.show()

# ============================================================================
# EXAMPLE 5: EVENT ENRICHMENT
# ============================================================================

print("\n5. EVENT ENRICHMENT")
print("-" * 50)

# Enrich with reference data
users = [(1, "John"), (2, "Jane")]
df_users = spark.createDataFrame(users, ["user_id", "name"])

enriched = df_events.join(df_users, "user_id", "left")
print("\nEnriched events:")
enriched.show()

print("\n" + "=" * 70)
print("EVENT-DRIVEN ARCHITECTURE COMPLETE")
print("=" * 70)
```