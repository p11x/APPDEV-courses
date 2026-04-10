# Topic: Spark Streaming Basics
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Spark Streaming Basics

I. INTRODUCTION
Spark Streaming is the micro-batch component of Spark that enables processing 
of live data streams. This module covers streaming basics, DStreams, and 
fundamental streaming operations.

II. CORE CONCEPTS
- DStream (Discretized Stream)
- Streaming context
- Input sources
- Transformations on streams
- Output operations
- Checkpointing

III. IMPLEMENTATION (PySpark code)
"""

from pyspark.sql import SparkSession
from pyspark.sql import functions as F
from pyspark.streaming import StreamingContext
from pyspark import SparkConf
import time


def create_spark_session(app_name: str = "StreamingDemo") -> SparkSession:
    return SparkSession.builder \
        .appName(app_name) \
        .master("local[*]") \
        .getOrCreate()


def create_streaming_context(spark: SparkSession, batch_interval: int = 5) -> StreamingContext:
    """Create StreamingContext from Spark session."""
    ssc = StreamingContext(spark.sparkContext, batch_interval)
    print(f"\nStreaming Context created with {batch_interval}s batch interval")
    return ssc


def stream_from_socket(ssc: StreamingContext, host: str = "localhost", port: int = 9999) -> None:
    """Create stream from socket."""
    print("\nSocket Stream:")
    print(f"  Would connect to {host}:{port}")
    print("  Note: Requires netcat server running")
    
    lines = ssc.socketTextStream(host, port)
    print(f"  Stream created: {lines}")


def stream_from_files(ssc: StreamingContext, directory: str = "/tmp/stream_input") -> None:
    """Create stream from file directory."""
    print("\nFile Stream:")
    print(f"  Directory: {directory}")
    
    lines = ssc.textFileStream(directory)
    print(f"  File stream created")
    
    words = lines.flatMap(lambda line: line.split())
    word_counts = words.map(lambda word: (word, 1)).reduceByKey(lambda a, b: a + b)
    word_counts.pprint()


def streaming_transformations(ssc: StreamingContext) -> None:
    """Demonstrate streaming transformations."""
    print("\nStreaming Transformations:")
    
    data = [
        ("event1", "2024-01-01", 100),
        ("event2", "2024-01-01", 200),
    ]
    rdd = ssc.sparkContext.parallelize(data)
    stream = ssc.queueStream([rdd])
    
    transformed = stream.map(lambda x: (x[0], x[2] * 2))
    transformed.pprint()


def streaming_window_operations(ssc: StreamingContext) -> None:
    """Demonstrate window operations on streams."""
    print("\nWindow Operations:")
    
    data = [(1,), (2,), (3,)]
    rdd = ssc.sparkContext.parallelize(data)
    stream = ssc.queueStream([rdd])
    
    windowed = stream.window(10, 5)
    print("  Window configured (10s window, 5s slide)")


def streaming_output_operations(ssc: StreamingContext) -> None:
    """Demonstrate output operations."""
    print("\nOutput Operations:")
    print("  - print(): Print first 10 elements")
    print("  - saveAsTextFiles(): Save to text files")
    print("  - saveAsHadoopFiles(): Save to Hadoop")
    print("  - foreachRDD(): Custom RDD processing")


def core_implementation():
    print("=" * 60)
    print("SPARK STREAMING BASICS")
    print("=" * 60)
    
    spark = create_spark_session()
    ssc = create_streaming_context(spark, 5)
    
    stream_from_socket(ssc)
    stream_from_files(ssc)
    streaming_transformations(ssc)
    streaming_window_operations(ssc)
    streaming_output_operations(ssc)
    
    print("\nNote: Start streaming context with ssc.start()")
    ssc.stop(stopSparkContext=True)


def banking_example():
    print("\n" + "=" * 60)
    print("BANKING APPLICATION - Streaming")
    print("=" * 60)
    
    spark = create_spark_session("BankingStream")
    ssc = StreamingContext(spark.sparkContext, 5)
    
    print("  Transaction stream would process:")
    print("  - Real-time transaction alerts")
    print("  - Fraud detection")
    print("  - Account balance updates")
    
    ssc.stop(stopSparkContext=True)


def healthcare_example():
    print("\n" + "=" * 60)
    print("HEALTHCARE APPLICATION - Streaming")
    print("=" * 60)
    
    spark = create_spark_session("HealthcareStream")
    ssc = StreamingContext(spark.sparkContext, 5)
    
    print("  Patient monitoring stream would process:")
    print("  - Vital signs alerts")
    print("  - Medication notifications")
    print("  - Emergency triggers")
    
    ssc.stop(stopSparkContext=True)


def main():
    print("Executing Spark Streaming Basics implementation")
    try:
        core_implementation()
        banking_example()
        healthcare_example()
    except Exception as e:
        print(f"Error: {e}")


if __name__ == "__main__":
    main()
