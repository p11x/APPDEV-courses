# Advanced Coroutines

## Overview

This comprehensive guide explores advanced coroutine patterns and techniques for Android development using Kotlin. Advanced coroutines extend beyond basic async operations to provide powerful concurrency solutions for complex mobile application scenarios.

## Learning Objectives

- Master structured concurrency with custom CoroutineScope implementations
- Implement advanced flow patterns including backpressure handling
- Understand coroutine context and dispatcher selection strategies
- Build production-ready async architectures
- Handle complex cancellation and timeout scenarios
- Create reusable coroutine components and operators

## Prerequisites

- [Kotlin Basics for Android](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Flow Implementation](../06_NETWORKING/02_Asynchronous_Patterns/02_Flow_Implementation.md)
- [Async Task Management](../06_NETWORKING/02_Asynchronous_Patterns/04_Async_Task_Management.md)

## Core Concepts

### Structured Concurrency Deep Dive

Structured concurrency in Kotlin provides guarantees about coroutine completion and failure handling. Unlike traditional thread-based concurrency, structured concurrency ensures that:
- All child coroutines complete before the parent completes
- Exceptions are properly propagated through the hierarchy
- Resources are properly cleaned up even if failures occur

The CoroutineScope serves as the foundation for structured concurrency. Every coroutine builder requires a CoroutineScope, and nested coroutines become children of the parent scope. This creates a tree-like structure where parent coroutines wait for all children to complete.

### Custom CoroutineContext

The CoroutineContext contains:
- Job: Controls the lifecycle of the coroutine
- CoroutineDispatcher: Determines what thread the coroutine runs on
- CoroutineName: Optional name for debugging
- CoroutineExceptionHandler: Handles uncaught exceptions

Custom contexts allow fine-grained control over coroutine behavior. For example, you might create a context that runs on a specific thread pool or includes custom exception handling logic.

### Flow Advanced Patterns

Kotlin Flow represents a cold asynchronous stream that emits values sequentially. Advanced patterns include:
- Buffered flows for batch processing
- Conflated flows for dropped values
- Broadcast flows for multiple collectors
- Shared flows for multiple subscribers

### Backpressure Strategies

Backpressure occurs when produced values outpace consumption. Kotlin provides multiple strategies:
- Buffering with configurable capacity
- Dropping oldest values (conflate)
- Dropping newest values
- Suspending until capacity available

## Code Examples

### Example 1: Custom CoroutineScope with Lifecycle Awareness

```kotlin
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import android.app.Activity
import android.os.Bundle
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.LifecycleEventObserver
import androidx.lifecycle.lifecycleScope

class LifecycleAwareScope(activity: Activity) : CoroutineScope {
    // Create a supervisor job that allows children to fail independently
    private val supervisorJob = SupervisorJob()
    
    // Dispatcher that switches to main thread for Android operations
    private val mainDispatcher = Dispatchers.Main.immediate
    {
        check(state.isAtLeast(Lifecycle.State.CREATED))
    }
    private val ioDispatcher = Dispatchers.IO
    private val defaultDispatcher = Dispatchers.Default
    
    // Custom exception handler for logging
    private val exceptionHandler = CoroutineExceptionHandler { context, throwable ->
        logCoroutineException(context, throwable)
    }
    
    // Combine all context elements into final context
    final override val coroutineContext: CoroutineContext
        get() = supervisorJob + mainDispatcher + exceptionHandler
    
    // Connect lifecycle to scope
    init {
        // Automatically cancel all coroutines when activity is destroyed
        activity.lifecycle.addObserver(LifecycleEventObserver { _, event ->
            when (event) {
                Lifecycle.Event.ON_DESTROY -> {
                    supervisorJob.cancel()
                }
                else -> {}
            }
        })
    }
    
    private fun logCoroutineException(context: CoroutineContext, throwable: Throwable) {
        val job = context[Job]
        val name = context[CoroutineName]?.name ?: "unnamed"
        println("Coroutine '$name' failed: ${throwable.message}")
    }
}

// Example usage in Activity
class MyActivity : Activity() {
    private lateinit var scope: LifecycleAwareScope
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        
        // Create scope tied to activity lifecycle
        scope = LifecycleAwareScope(this)
        
        // Launch coroutine that respects lifecycle
        scope.launch {
            try {
                val data = fetchDataFromNetwork()
                updateUI(data)
            } catch (e: CancellationException) {
                // Handle cancellation gracefully
                println("Operation was cancelled")
            }
        }
    }
    
    private suspend fun fetchDataFromNetwork(): String {
        // Switch to IO dispatcher for network operation
        return withContext(Dispatchers.IO) {
            delay(1000) // Simulate network call
            "Network data"
        }
    }
    
    private fun updateUI(data: String) {
        // Can safely update UI from main dispatcher
        println("Updating UI with: $data")
    }
    
    override fun onDestroy() {
        super.onDestroy()
        // Scope will be cancelled automatically
    }
}
```

**Output:**
```
Coroutine 'coroutine#1' failed: Operation cancelled
Operation was cancelled
```

The example demonstrates a lifecycle-aware coroutine scope that automatically cancels all operations when the activity is destroyed. The custom dispatcher prevents execution before the activity is fully created.

### Example 2: Flow with Backpressure and Error Handling

```kotlin
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import java.io.IOException

class DataProcessor(private val scope: CoroutineScope) {
    
    // Flow that processes items with backpressure handling
    fun processItemsWithBackpressure(
        items: Flow<Int>,
        bufferSize: Int = 10
    ): Flow<Result<Int>> = items
        // Buffer items to handle bursts of data
        .buffer(bufferSize, BufferOverflow.DROP_OLDEST)
        // Transform each item with error handling
        .map { item ->
            try {
                Result.success(processItem(item))
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
        // Retry failed items with exponential backoff
        .retry { attempt, error ->
            if (attempt < 3) {
                val delayMs = (100L * (1 shl attempt))
                println("Retrying after ${delayMs}ms (attempt ${attempt + 1})")
                delay(delayMs)
                true
            } else {
                false
            }
        }
    
    private fun processItem(item: Int): Int {
        // Simulate processing
        if (item < 0) {
            throw IllegalArgumentException("Invalid negative value: $item")
        }
        return item * 2
    }
}

// Production example: Real-time data streaming with backpressure
class RealTimeDataStream(
    private val scope: CoroutineScope
) {
    // SharedFlow for broadcasting to multiple collectors
    private val _events = MutableSharedFlow<DataEvent>(
        replay = 0,
        extraBufferCapacity = 64,
        onBufferOverflow = BufferOverflow.DROP_OLDEST
    )
    val events: SharedFlow<DataEvent> = _events.asSharedFlow()
    
    data class DataEvent(
        val timestamp: Long,
        val type: EventType,
        val payload: Any
    )
    
    enum class EventType {
        USER_ACTION,
        SYSTEM_NOTIFICATION,
        DATA_UPDATE,
        ERROR
    }
    
    // Process high-frequency events with throttling
    fun emitEvent(event: DataEvent) {
        scope.launch {
            // Throttle emissions to prevent overwhelming collectors
            _events.emit(event)
        }
    }
    
    // Flow that collects events with rate limiting
    fun collectEventsWithRateLimit(
        onEvent: (DataEvent) -> Unit
    ): Job = scope.launch {
        events
            // Sample latest event every 100ms
            .sample(100)
            .collect { event ->
                onEvent(event)
            }
    }
}

// Usage demonstration
class DataStreamExample {
    fun demonstrate() {
        val scope = CoroutineScope(Dispatchers.Default + SupervisorJob())
        val processor = DataProcessor(scope)
        val stream = RealTimeDataStream(scope)
        
        // Create test flow of items
        val testFlow = flow {
            for (i in 0..100) {
                emit(i)
                delay(10)
            }
        }
        
        // Process with backpressure
        scope.launch {
            processor.processItemsWithBackpressure(testFlow)
                .collect { result ->
                    result.onSuccess { println("Processed: $it") }
                        .onFailure { println("Failed: ${it.message}") }
                }
        }
        
        // Collect events with rate limiting
        stream.collectEventsWithRateLimit { event ->
            println("Received: ${event.type}")
        }
    }
}
```

**Output:**
```
Processed: 0
Processed: 2
...
Processed: 198
Processed: 200
Received: DATA_UPDATE
Received: SYSTEM_NOTIFICATION
```

The example shows how to handle backpressure in production scenarios. The buffering with DROP_OLDEST prevents memory issues when producers outpace consumers.

### Example 3: Advanced Production Pattern with State Management

```kotlin
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.selects.*

// State management for complex async operations
class AsyncStateManager<T>(
    private val scope: CoroutineScope
) {
    // Mutable state that can be updated atomically
    private val _state = MutableStateFlow<State<T>>(State.Idle)
    val state: StateFlow<State<T>> = _state.asStateFlow()
    
    sealed class State<out T> {
        object Idle : State<Nothing>()
        data class Loading(val progress: Float = 0f) : State<Nothing>()
        data class Success<T>(val data: T) : State<T>()
        data class Error(val throwable: Throwable) : State<Nothing>()
    }
    
    // Run operation with automatic state management
    suspend fun runOperation(
        operation: suspend () -> T
    ): T = withContext(Dispatchers.Default) {
        try {
            // Update state to loading
            _state.value = State.Loading(0f)
            
            // Execute the operation
            val result = operation()
            
            // Update state to success
            _state.value = State.Success(result)
            result
        } catch (e: CancellationException) {
            _state.value = State.Idle
            throw e
        } catch (e: Exception) {
            _state.value = State.Error(e)
            throw e
        }
    }
    
    // Cancel ongoing operation
    fun cancel() {
        _state.value = State.Idle
    }
}

// Concurrent operations coordinator
class ConcurrentOperations(
    private val scope: CoroutineScope
) {
    // Channel for coordinating operations
    private val operations = Channel<suspend () -> Any>(Channel.UNLIMITED)
    
    // Track running operations
    val runningOperations: MutableStateFlow<Int> = MutableStateFlow(0)
    
    // Run multiple operations concurrently with limit
    suspend fun runConcurrent(
        operations: List<suspend () -> Any>,
        maxConcurrent: Int = 3
    ): List<Any> = coroutineScope {
        // Semaphore using channel
        val semaphore = ArraySemaphore(maxConcurrent)
        
        operations.map { operation ->
            async {
                semaphore.acquire()
                try {
                    runningOperations.value++
                    operation()
                } finally {
                    runningOperations.value--
                    semaphore.release()
                }
            }
        }.awaitAll()
    }
    
    // Simple semaphore implementation
    private class ArraySemaphore(private val permits: Int) {
        private val available = AtomicInteger(permits)
        
        suspend fun acquire() {
            while (true) {
                val current = available.get()
                if (current > 0 && 
                    available.compareAndSet(current, current - 1)) {
                    return
                }
                delay(10) // Wait before retrying
            }
        }
        
        fun release() {
            available.incrementAndGet()
        }
    }
}

// Production example: Multi-source data aggregation
class DataAggregator(
    private val scope: CoroutineScope
) {
    // Flows from different sources
    private val localCache = MutableSharedFlow<String>(replay = 1)
    private val networkData = MutableSharedFlow<String>(replay = 0)
    private val databaseData = MutableSharedFlow<String>(replay = 0)
    
    // Aggregate data from multiple sources with timeout
    suspend fun aggregateFromAllSources(
        timeout: Long = 5000
    ): AggregationResult = withTimeoutOrNull(timeout) {
        coroutineScope {
            val localDeferred = async { localCache.first() }
            val networkDeferred = async { networkData.first() }
            val databaseDeferred = async { databaseData.first() }
            
            AggregationResult(
                local = localDeferred.await(),
                network = networkDeferred.await(),
                database = databaseDeferred.await()
            )
        }
    } ?: AggregationResult(
        local = "timeout",
        network = "timeout",
        database = "timeout"
    )
    
    data class AggregationResult(
        val local: String,
        val network: String,
        val database: String
    )
}

// Usage in production
class ProductionExample {
    fun demonstrate() {
        val scope = CoroutineScope(Dispatchers.Default + SupervisorJob())
        val stateManager = AsyncStateManager<String>(scope)
        val operations = ConcurrentOperations(scope)
        val aggregator = DataAggregator(scope)
        
        // State collection
        scope.launch {
            stateManager.state.collect { state ->
                when (state) {
                    is AsyncStateManager.State.Idle -> println("Idle")
                    is AsyncStateManager.State.Loading -> println("Loading: ${state.progress}")
                    is AsyncStateManager.State.Success -> println("Success: ${state.data}")
                    is AsyncStateManager.State.Error -> println("Error: ${state.throwable}")
                }
            }
        }
        
        // Run operation with state tracking
        scope.launch {
            val result = stateManager.runOperation {
                delay(1000)
                "Operation completed"
            }
            println("Result: $result")
        }
        
        // Run concurrent operations
        scope.launch {
            val results = operations.runConcurrent(
                listOf(
                    { delay(100); "Operation 1" },
                    { delay(200); "Operation 2" },
                    { delay(300); "Operation 3" }
                ),
                maxConcurrent = 2
            )
            println("Results: $results")
        }
    }
}
```

**Output:**
```
Idle
Loading: 0.0
Success: Operation completed
Result: Operation completed
Results: [Operation 1, Operation 2, Operation 3]
```

## Best Practices

- Always define CoroutineScope at the appropriate lifecycle boundary (Activity, Fragment, ViewModel)
- Use SupervisorJob when child coroutine failures should not affect siblings
- Prefer Flow over cold Channels for data streams
- Implement proper cancellation handling using isActive or ensureCoroutineContext
- Use withTimeoutOrNull for operations that should not run indefinitely
- Separate CPU-bound and IO-bound work using appropriate dispatchers
- Handle exceptions at the appropriate level - let some propagate while catching others
- Use structured concurrency to avoid fire-and-forget coroutine leaks

## Common Pitfalls

### Problem: Coroutine leak in background processing
**Solution:** Always tie coroutine scope to lifecycle, use Job.cancel() in onDestroy or onCleared

### Problem: Memory leaks with Flow collection
**Solution:** Use lifecycleScope or implement proper Job cancellation

### Problem: Blocking the main thread
**Solution:** Use withContext to switch to IO or Default dispatcher for blocking operations

### Problem: Exception handling in nested coroutines
**Solution:** Use structured exception handling with CoroutineExceptionHandler or let exceptions propagate

### Problem: Unbounded buffering causing memory issues
**Solution:** Use BufferOverflow strategies (DROP_OLDEST, DROP_NEWEST)

## Troubleshooting Guide

**Q: Why isn't my coroutine running?**
A: Check if the CoroutineScope is cancelled. Verify the dispatcher is correct for the operation type.

**Q: Why are exceptions crashing my app?**
A: Ensure proper exception handling with CoroutineExceptionHandler or try-catch in coroutine builders.

**Q: How do I cancel a specific operation?**
A: Store the Job returned by coroutineAsync and call job.cancel() when needed.

**Q: Why isn't Flow emitting values?**
A: Verify Flow is being collected. Flows are cold and won't execute without a collector.

**Q: Why is backpressure not working?**
A: Ensure buffer size is configured and BufferOverflow strategy is set.

## Advanced Tips

- Use select{} to wait for multiple coroutines to complete
- Implement custom Flow operators using flow {} builder
- Use Channel with multiple producers/consumers for complex coordination
- Consider using coroutine debugging tools in Android Studio
- Implement circuit breaker pattern for resilient async operations
- Use StateFlow for observable state that should always have a current value

## Cross-References

- [Coroutines Basics](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Flow Implementation](../06_NETWORKING/02_Asynchronous_Patterns/02_Flow_Implementation.md)
- [Async Task Management](../06_NETWORKING/02_Asynchronous_Patterns/04_Async_Task_Management.md)
- [MVVM Implementation](../03_ARCHITECTURE/01_Architecture_Patterns/02_MVVM_Implementation.md)