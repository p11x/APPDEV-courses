# Performance Monitoring and Scaling

## I. INTRODUCTION

### What is Performance Monitoring and Scaling?
Performance monitoring and scaling in Spark streaming involves tracking application metrics, identifying bottlenecks, and adjusting resources to maintain desired throughput and latency. This includes monitoring CPU, memory, processing time, and implementing both horizontal and vertical scaling strategies.

### Why is it Important in Big Data?
Proper monitoring ensures SLAs are met. Scaling handles increased workloads. Early detection prevents outages.

## II. IMPLEMENTATION

```python
"""
Performance Monitoring and Scaling
"""

from pyspark.sql import SparkSession

spark = SparkSession.builder \
    .appName("MonitoringDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("PERFORMANCE MONITORING AND SCALING")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: KEY METRICS TO MONITOR
# ============================================================================

print("\n1. KEY METRICS TO MONITOR")
print("-" * 50)

metrics = [
    "Processing Time",
    "Input Rate",
    "Delay",
    "Backpressure",
    "Executor Metrics",
    "GC Time"
]

print("Streaming metrics to monitor:")
for m in metrics:
    print(f"  - {m}")

# ============================================================================
# EXAMPLE 2: CUSTOM METRICS
# ============================================================================

print("\n2. CUSTOM METRICS")
print("-" * 50)

# Custom accumulator for tracking
from pyspark import AccumulatorParam

class ListParam(AccumulatorParam):
    def zero(self, initialValue):
        return []
    def addInPlace(self, v1, v2):
        return v1 + v2

counter = spark.sparkContext.accumulator(0, ListParam())
print("Custom accumulators for tracking custom metrics")

# ============================================================================
# EXAMPLE 3: SPARK UI ANALYSIS
# ============================================================================

print("\n3. SPARK UI ANALYSIS")
print("-" * 50)

print("Spark UI provides:")
print("  - Jobs tab: Job execution details")
print("  - Stages tab: Stage progress")
print("  - Storage tab: Cached data")
print("  - Streaming tab: Streaming stats")
print("  - Executors tab: Resource usage")

# ============================================================================
# EXAMPLE 4: SCALING STRATEGIES
# ============================================================================

print("\n4. SCALING STRATEGIES")
print("-" * 50)

scaling_config = {
    "spark.streaming.dynamicAllocation.enabled": "true",
    "spark.streaming.dynamicAllocation.minExecutors": "2",
    "spark.streaming.dynamicAllocation.maxExecutors": "20",
    "spark.streaming.dynamicAllocation.executorIdleTimeout": "60s"
}

for k, v in scaling_config.items():
    spark.conf.set(k, v)

print("Dynamic scaling enabled:")
for k, v in scaling_config.items():
    print(f"  {k}: {v}")

# ============================================================================
# EXAMPLE 5: BACKPRESSURE CONFIGURATION
# ============================================================================

print("\n5. BACKPRESSURE CONFIGURATION")
print("-" * 50)

spark.conf.set("spark.streaming.backpressure.enabled", "true")
spark.conf.set("spark.streaming.backpressure.initialRate", "1000")

print("Backpressure enabled to prevent overwhelming")

# ============================================================================
# EXAMPLE 6: RATE LIMITING
# ============================================================================

print("\n6. RATE LIMITING")
print("-" * 50)

spark.conf.set("spark.streaming.kafka.maxRatePerPartition", "500")
spark.conf.set("spark.streaming.receiver.maxRate", "100")

print("Rate limiting configured")

# ============================================================================
# EXAMPLE 7: PERFORMANCE TUNING
# ============================================================================

print("\n7. PERFORMANCE TUNING")
print("-" * 50)

tuning_config = {
    "spark.sql.shuffle.partitions": "200",
    "spark.default.parallelism": "100",
    "spark.memory.fraction": "0.6"
}

print("Performance tuning:")
for k, v in tuning_config.items():
    print(f"  {k}: {v}")

# ============================================================================
# EXAMPLE 8: ALERTING THRESHOLDS
# ============================================================================

print("\n8. ALERTING THRESHOLDS")
print("-" * 50)

thresholds = {
    "processing_delay_ms": 1000,
    "input_rate_mb": 100,
    "batch_processing_time_ms": 500
}

print("Alert thresholds:")
for k, v in thresholds.items():
    print(f"  {k}: {v}")

# ============================================================================
# EXAMPLE 9: LOGGING CONFIGURATION
# ============================================================================

print("\n9. LOGGING CONFIGURATION")
print("-" * 50)

spark.sparkContext.setLogLevel("WARN")

print("Log level set to WARN")
print("Note: Configure log4j.properties for file output")

# ============================================================================
# EXAMPLE 10: HEALTH CHECKS
# ============================================================================

print("\n10. HEALTH CHECKS")
print("-" * 50)

def check_health():
    """Health check function"""
    status = {
        "streaming_active": True,
        "last_batch_time": "2024-01-01 10:00:00",
        "queue_depth": 0
    }
    return status

health = check_health()
print("Health check results:")
for k, v in health.items():
    print(f"  {k}: {v}")

print("\n" + "=" * 70)
print("MONITORING AND SCALING COMPLETE")
print("=" * 70)
```