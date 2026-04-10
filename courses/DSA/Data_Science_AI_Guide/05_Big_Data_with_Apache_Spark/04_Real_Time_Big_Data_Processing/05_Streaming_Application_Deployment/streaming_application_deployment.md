# Streaming Application Deployment

## I. INTRODUCTION

### What is Streaming Application Deployment?
Streaming application deployment involves configuring, packaging, and running Spark Streaming applications in production environments. This includes cluster deployment, resource allocation, monitoring, and high availability configurations to ensure reliable continuous data processing.

### Why is it Important in Big Data?
Production deployment ensures streaming applications run reliably 24/7. Proper deployment handles failures gracefully. It enables scaling to handle increased throughput.

## II. IMPLEMENTATION

```python
"""
Streaming Application Deployment
"""

from pyspark.sql import SparkSession

spark = SparkSession.builder \
    .appName("StreamingDeploymentDemo") \
    .master("local[*]") \
    .getOrCreate()

print("=" * 70)
print("STREAMING APPLICATION DEPLOYMENT")
print("=" * 70)

# ============================================================================
# EXAMPLE 1: CLUSTER CONFIGURATION
# ============================================================================

print("\n1. CLUSTER CONFIGURATION")
print("-" * 50)

# Key configurations for cluster deployment
config = {
    "spark.executor.memory": "4g",
    "spark.executor.cores": "2",
    "spark.executor.instances": "10",
    "spark.streaming.backpressure.enabled": "true",
    "spark.streaming.kafka.maxRatePerPartition": "1000"
}

for key, value in config.items():
    spark.conf.set(key, value)

print("Cluster configuration applied:")
for k, v in config.items():
    print(f"  {k}: {v}")

# ============================================================================
# EXAMPLE 2: KAFKA INTEGRATION
# ============================================================================

print("\n2. KAFKA INTEGRATION")
print("-" * 50)

# Kafka source configuration
kafka_config = {
    "kafka.bootstrap.servers": "kafka1:9092,kafka2:9092",
    "subscribe": "transactions,events",
    "startingOffsets": "earliest",
    "failOnDataLoss": "false"
}

print("Kafka integration configured:")
for k, v in kafka_config.items():
    print(f"  {k}: {v}")

# ============================================================================
# EXAMPLE 3: CHECKPOINTING
# ============================================================================

print("\n3. CHECKPOINTING CONFIGURATION")
print("-" * 50)

checkpoint_dir = "s3://bucket/checkpoint"

spark.conf.set("spark.streaming.checkpoint.directory", checkpoint_dir)
print(f"Checkpoint directory: {checkpoint_dir}")
print("Note: Required for stateful streaming")

# ============================================================================
# EXAMPLE 4: PACKAGING FOR DEPLOYMENT
# ============================================================================

print("\n4. PACKAGING FOR DEPLOYMENT")
print("-" * 50)

print("Deployment package:")
print("  - Application JAR/Wheel")
print("  - Dependencies bundled")
print("  - Configuration files")
print("  - Entry point script")

# ============================================================================
# EXAMPLE 5: RESOURCE ALLOCATION
# ============================================================================

print("\n5. RESOURCE ALLOCATION")
print("-" * 50)

# Dynamic allocation for streaming
spark.conf.set("spark.streaming.dynamicAllocation.enabled", "true")
spark.conf.set("spark.streaming.dynamicAllocation.minExecutors", "2")
spark.conf.set("spark.streaming.dynamicAllocation.maxExecutors", "10")

print("Dynamic allocation enabled")

# ============================================================================
# EXAMPLE 6: MONITORING CONFIGURATION
# ============================================================================

print("\n6. MONITORING CONFIGURATION")
print("-" * 50)

spark.conf.set("spark.metrics.conf", "metrics.properties")
spark.conf.set("spark.streaming.ui.retainedBatches", "100")

print("Monitoring configured:")
print("  - Metrics export enabled")
print("  - UI retention: 100 batches")

# ============================================================================
# EXAMPLE 7: FAILURE RECOVERY
# ============================================================================

print("\n7. FAILURE RECOVERY CONFIGURATION")
print("-" * 50)

spark.conf.set("spark.streaming.stopGracefullyOnShutdown", "true")
spark.conf.set("spark.streaming.failOnDataLoss", "false")

print("Recovery configuration:")
print("  - Graceful shutdown: enabled")
print("  - Fail on data loss: disabled")

# ============================================================================
# EXAMPLE 8: DEPLOYMENT MODES
# ============================================================================

print("\n8. DEPLOYMENT MODES")
print("-" * 50)

print("Spark deployment modes:")
print("  - cluster mode: Driver runs on cluster")
print("  - client mode: Driver runs locally")
print("  - kubernetes mode: Container orchestration")

print("\n" + "=" * 70)
print("DEPLOYMENT COMPLETE")
print("=" * 70)
```