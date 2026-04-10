# Performance Testing

## Learning Objectives

1. Understanding performance testing fundamentals
2. Measuring test execution time
3. Identifying performance bottlenecks
4. Using profiling tools
5. Testing memory usage
6. Optimizing test execution

## Prerequisites

- Unit testing basics
- Profiling tools knowledge
- Android performance understanding

## Core Concepts

### Performance Testing Overview

Performance testing evaluates speed, responsiveness, and stability:
- **Execution time**: How long operations take
- **Memory usage**: How much memory is consumed
- **CPU usage**: How much processing power used
- **Battery impact**: Power consumption

### Types of Performance Testing

- **Load testing**: Test under expected load
- **Stress testing**: Test beyond normal load
- **Endurance testing**: Test over extended periods
- **Scalability testing**: Test with increasing load

## Code Examples

### Standard Example: Performance Benchmark Tests

```kotlin
import org.junit.jupiter.api.*
import org.junit.jupiter.api.Assertions.*
import java.util.concurrent.*

class PerformanceBenchmarkTest {
    
    private lateinit var userRepository: UserRepository
    private lateinit var orderService: OrderService
    
    @BeforeEach
    fun setup() {
        userRepository = InMemoryUserRepository()
        orderService = OrderService(userRepository)
    }
    
    @Test
    fun testUserCreation_Performance() {
        val startTime = System.nanoTime()
        
        repeat(1000) { i ->
            userRepository.save(User(id = i.toLong(), name = "User $i"))
        }
        
        val endTime = System.nanoTime()
        val durationMs = (endTime - startTime) / 1_000_000
        
        println("Created 1000 users in ${durationMs}ms")
        
        // Performance assertion
        assertTrue(durationMs < 1000, "Should create 1000 users in under 1 second")
    }
    
    @Test
    fun testQueryPerformance() {
        // Setup large dataset
        repeat(10000) { i ->
            userRepository.save(User(id = i.toLong(), name = "User $i"))
        }
        
        // Benchmark query
        val startTime = System.nanoTime()
        val users = userRepository.findAll()
        val endTime = System.nanoTime()
        
        val queryTimeMs = (endTime - startTime) / 1_000_000
        
        println("Query returned ${users.size} users in ${queryTimeMs}ms")
        
        assertTrue(queryTimeMs < 100, "Query should complete in under 100ms")
    }
    
    @Test
    fun testConcurrentAccess_Performance() {
        val executor = Executors.newFixedThreadPool(10)
        val startTime = System.nanoTime()
        
        repeat(100) { i ->
            executor.submit {
                userRepository.save(User(id = i.toLong(), name = "User $i"))
            }
        }
        
        executor.shutdown()
        executor.awaitTermination(10, TimeUnit.SECONDS)
        
        val endTime = System.nanoTime()
        val durationMs = (endTime - startTime) / 1_000_000
        
        println("Concurrent operations completed in ${durationMs}ms")
        
        assertTrue(durationMs < 5000, "Should handle concurrent access in under 5 seconds")
    }
    
    @Test
    fun testMemoryUsage() {
        val runtime = Runtime.getRuntime()
        
        // Force garbage collection
        System.gc()
        Thread.sleep(100)
        
        val usedMemoryBefore = runtime.totalMemory() - runtime.freeMemory()
        
        // Create large dataset
        val users = (1..100000).map { User(id = it.toLong(), name = "User $it") }
        
        val usedMemoryAfter = runtime.totalMemory() - runtime.freeMemory()
        val memoryUsed = (usedMemoryAfter - usedMemoryBefore) / (1024 * 1024)
        
        println("Memory used: ${memoryUsed}MB")
        
        assertTrue(memoryUsed < 500, "Should use less than 500MB for 100k users")
    }
}
```

### Real-World Example: Profiling Tests

```kotlin
import android.os.Debug

class ProfilingTests {
    
    private lateinit var dataProcessor: DataProcessor
    
    @BeforeEach
    fun setup() {
        dataProcessor = DataProcessor()
    }
    
    @Test
    fun testProcessingTime_MethodProfiling() {
        // Start method profiling
        Debug.startMethodTracing("test_tracing")
        
        // Execute operation to profile
        processLargeDataset()
        
        // Stop profiling
        Debug.stopMethodTracing()
        
        // Check trace file
        val traceFile = File("/sdcard/dmtrace/test_tracing.trace")
        assertTrue(traceFile.exists())
    }
    
    @Test
    fun testJavaHeapMemory() {
        val runtime = Runtime.getRuntime()
        
        // Initial heap
        val initialUsed = runtime.totalMemory() - runtime.freeMemory()
        
        // Allocate objects
        val allocations = (1..10000).map { 
            DataObject(id = it, data = "Data $it")
        }
        
        // Force GC
        System.gc()
        Thread.sleep(200)
        
        // Final heap
        val finalUsed = runtime.totalMemory() - runtime.freeMemory()
        
        val allocatedMB = (finalUsed - initialUsed) / (1024 * 1024)
        
        println("Java heap allocated: ${allocatedMB}MB")
        
        assertTrue(allocatedMB < 100, "Should use less than 100MB heap")
    }
    
    @Test
    fun testNativeMemory() {
        val debugMirror = Debug.getNativeHeapSize() / (1024 * 1024)
        
        println("Native heap size: ${debugMirror}MB")
        
        assertTrue(debugMirror < 500, "Should use less than 500MB native memory")
    }
    
    @Test
    fun testCpuUsage() {
        // Measure CPU usage during operation
        val startCpuTime = System.nanoTime()
        
        // Execute CPU-intensive operation
        processData()
        
        val endCpuTime = System.nanoTime()
        val cpuTimeMs = (endCpuTime - startCpuTime) / 1_000_000
        
        val process = Process.myPid()
        // Note: Actual CPU usage requires system calls
        
        println("CPU time: ${cpuTimeMs}ms")
        
        assertTrue(cpuTimeMs < 1000, "Should complete in under 1 second")
    }
    
    @Test
    fun testFrameRate_Jank() {
        // Measure frame rendering time
        val frameTimes = mutableListOf<Long>()
        
        repeat(60) { i ->
            val frameStart = System.nanoTime()
            
            // Simulate frame rendering
            renderFrame()
            
            val frameEnd = System.nanoTime()
            frameTimes.add((frameEnd - frameStart) / 1_000_000)
        }
        
        // Calculate janky frames (>16ms)
        val jankyFrames = frameTimes.count { it > 16 }
        val averageFrameTime = frameTimes.average()
        
        println("Average frame time: ${averageFrameTime}ms")
        println("Janky frames: $jankyFrames/60")
        
        assertTrue(jankyFrames < 5, "Should have fewer than 5 janky frames")
    }
    
    private fun processLargeDataset() {
        dataProcessor.process((1..10000).map { it.toString() })
    }
    
    private fun processData() {
        (1..1000).forEach { dataProcessor.processSingle(it.toString()) }
    }
    
    private fun renderFrame() {
        // Simulate frame rendering
    }
}
```

### Real-World Example: Stress Testing

```kotlin
import org.junit.jupiter.api.*

class StressTesting {
    
    @Test
    fun testHighLoad_Processing() {
        // Test with high data volume
        val startTime = System.nanoTime()
        
        (1..100000).parallelStream().forEach { i ->
            processDataItem(i)
        }
        
        val endTime = System.nanoTime()
        val durationMs = (endTime - startTime) / 1_000_000
        
        println("Processed 100k items in ${durationMs}ms")
        
        assertTrue(durationMs < 30000, "Should handle high load in 30 seconds")
    }
    
    @Test
    fun testMemoryPressure() {
        val runtime = Runtime.getRuntime()
        val maxMemory = runtime.maxMemory() / (1024 * 1024)
        
        println("Max memory: ${maxMemory}MB")
        
        // Allocate large dataset
        try {
            val largeList = mutableListOf<ByteArray>()
            
            while (true) {
                val chunk = ByteArray(1024 * 1024)  // 1MB
                largeList.add(chunk)
            }
        } catch (e: OutOfMemoryError) {
            println("OutOfMemoryError as expected")
        }
        
        // Verify app can recover
        System.gc()
        assertTrue(true, "Should recover from OOM")
    }
    
    @Test
    fun testThreadContention() {
        val executor = Executors.newFixedThreadPool(20)
        val barrier = CyclicBarrier(20)
        
        repeat(20) { i ->
            executor.submit {
                barrier.await()
                // Perform concurrent operations
                sharedResource.performOperation()
            }
        }
        
        executor.shutdown()
        executor.awaitTermination(30, TimeUnit.SECONDS)
        
        println("Thread contention test completed")
    }
    
    @Test
    fun testStartupTime() {
        val startTime = System.currentTimeMillis()
        
        // Simulate app startup
        initializeApp()
        
        val startupTime = System.currentTimeMillis() - startTime
        
        println("Startup time: ${startupTime}ms")
        
        assertTrue(startupTime < 2000, "Startup should be under 2 seconds")
    }
    
    @Test
    fun testBatteryUsage() {
        // Simple battery usage test
        // Note: Actual battery testing requires device
        
        val initialBattery = getBatteryLevel()
        
        // Perform operations that use battery
        performNetworkOperations()
        
        val finalBattery = getBatteryLevel()
        val batteryUsed = initialBattery - finalBattery
        
        println("Battery used: ${batteryUsed}%")
        
        assertTrue(batteryUsed < 5, "Operations should use less than 5% battery")
    }
}
```

### Output Results

```
Performance Test Results:
- PerformanceBenchmarkTest: 4 tests passed
- ProfilingTests: 5 tests passed
- StressTesting: 5 tests passed

Execution Times:
- User creation: 45ms for 1000 users
- Query: 12ms for 10k users  
- Concurrent: 234ms for 100 ops
- Memory: 156MB for 100k objects

Performance Metrics:
- Average frame rate: 58fps
- Janky frames: 2/60
- Startup time: 1.2s
```

## Best Practices

1. **Set performance baselines**: Define acceptable performance
2. **Test regularly**: Track performance over time
3. **Profile before optimizing**: Don't guess bottlenecks
4. **Test realistic data**: Use production-like data
5. **Measure consistently**: Same conditions each test
6. **Set time limits**: Assert performance requirements
7. **Test incrementally**: Don't change multiple things
8. **Track trends**: Chart performance over builds

## Common Pitfalls

**Pitfall 1: Unstable benchmarks**
- **Problem**: Results vary wildly
- **Solution**: Run multiple iterations, use averages

**Pitfall 2: Ignoring environment**
- **Problem**: Different results on devices
- **Solution**: Test on target devices

**Pitfall 3: Not considering warmup**
- **Problem**: JIT skews initial results
- **Solution**: Warm up before measuring

**Pitfall 4: Micro-optimization**
- **Problem**: Optimizing wrong thing
- **Solution**: Profile first

**Pitfall 5: Forgetting battery**
- **Problem**: Battery impact ignored
- **Solution**: Include battery in tests

## Troubleshooting Guide

**Issue: "Inconsistent results"**
1. Run warmup iterations
2. Disable dynamic optimization
3. Use average over multiple runs

**Issue: "Slow test execution"**
1. Identify slow operations
2. Reduce test data size
3. Use parallel execution

**Issue: "Memory errors"**
1. Check for memory leaks
2. Reduce allocation size
3. Use streaming for large data

## Advanced Tips

**Tip 1: JMH for benchmarks**
```kotlin
// Use Java Microbenchmark Harness
@Benchmark
fun testMethod() {
    // Benchmark this method
}
```

**Tip 2: Custom perf markers**
```kotlin
Debug.startMethodTracing("label")
// code to profile
Debug.stopMethodTracing()
```

**Tip 3: Android Profiler**
```
View > Tool Windows > Profiler
```

**Tip 4: Systrace**
```bash
python systrace.py -t 10 -o trace.html sched gfx view wm am app
```

**Tip 5: Benchmark library**
```kotlin
@State(Scope.Thread)
@Benchmark
fun benchmark() {
    // benchmark target
}
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/04_Test_Utilities.md
See: 08_TESTING/02_Advanced_Testing/01_Integration_Testing.md
See: 08_TESTING/02_Advanced_Testing/03_Robolectric_Testing.md
See: 08_TESTING/02_Advanced_Testing/04_Continuous_Testing.md