# Background Threading

## Learning Objectives

1. Understanding Android threading model
2. Using Dispatchers for thread management
3. Implementing background work with Coroutines
4. Managing thread pools for concurrent operations
5. Optimizing performance for background tasks

## Prerequisites

- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Flow Implementation](./02_Flow_Implementation.md)
- [Async Task Management](./04_Async_Task_Management.md)

## Section 1: Android Threading Model

Android has a single main thread for UI operations and multiple background threads for heavy work. Understanding this model is essential for responsive apps.

Key threading concepts:
- Main thread (UI thread) for UI updates
- Background threads for network, database, file operations
- Handler and Looper for thread communication
- AndroidDispatchers for coroutine thread management

```kotlin
import kotlinx.coroutines.*
import android.os.Handler
import android.os.Looper

// Android thread utilities
object ThreadUtils {
    
    // Main thread handler
    private val mainHandler = Handler(Looper.getMainLooper())
    
    // Check if on main thread
    fun isMainThread(): Boolean = Looper.myLooper() == Looper.getMainLooper()
    
    // Execute on main thread
    fun runOnMain(block: () -> Unit) {
        if (isMainThread()) {
            block()
        } else {
            mainHandler.post(block)
        }
    }
    
    // Execute on main thread with delay
    fun runOnMainDelayed(block: () -> Unit, delayMs: Long) {
        mainHandler.postDelayed(block, delayMs)
    }
    
    // Execute on background thread
    fun runOnBackground(block: () -> Unit) {
        Thread {
            block()
        }.start()
    }
}

// Dispatchers overview
class DispatcherExamples {
    
    // Main thread - for UI updates
    fun mainThreadExample(scope: CoroutineScope) {
        scope.launch(Dispatchers.Main) {
            // Update UI here
            // This runs on the main thread
        }
    }
    
    // IO dispatcher - for I/O operations
    fun ioDispatcherExample(scope: CoroutineScope) {
        scope.launch(Dispatchers.IO) {
            // Network calls
            // File operations
            // Database queries
            // All I/O operations
        }
    }
    
    // Default dispatcher - for CPU-intensive work
    fun defaultDispatcherExample(scope: CoroutineScope) {
        scope.launch(Dispatchers.Default) {
            // Image processing
            // JSON parsing
            // Complex calculations
            // All CPU-intensive work
        }
    }
    
    // Unconfined - starts in caller thread
    fun unconfinedExample(scope: CoroutineScope) {
        scope.launch(Dispatchers.Unconfined) {
            // Starts in the thread that called launch
            // Then switches to whatever thread continuation runs on
        }
    }
}

// Custom dispatcher for specific work
class CustomDispatchers {
    
    // Create single-threaded dispatcher
    private val singleThreadDispatcher = Executors.newSingleThreadExecutor().asCoroutineDispatcher()
    
    // Create thread pool dispatcher
    private val threadPoolDispatcher = Executors.newFixedThreadPool(4).asCoroutineDispatcher()
    
    fun useCustomDispatcher(scope: CoroutineScope) {
        // Use single thread for ordered operations
        scope.launch(singleThreadDispatcher) {
            // Operations here run on same thread
            // Good for sequential operations
        }
        
        // Use thread pool for parallel operations
        scope.launch(threadPoolDispatcher) {
            // Operations here run on available thread pool
            // Good for parallel processing
        }
    }
    
    fun cleanup() {
        singleThreadDispatcher.close()
        threadPoolDispatcher.close()
    }
}

import java.util.concurrent.Executors
import kotlinx.coroutines.executors.*
```

## Section 2: Dispatchers in Practice

Using Dispatchers effectively for different types of background work.

```kotlin
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import okhttp3.OkHttpClient
import java.io.File
import java.util.concurrent.TimeUnit

// Repository with proper dispatcher usage
class OptimizedRepository(
    private val apiService: ApiService,
    private val database: Database
) {
    // Network operations on IO dispatcher
    suspend fun fetchUsers(): List<User> = withContext(Dispatchers.IO) {
        apiService.getUsers()
    }
    
    suspend fun fetchPosts(): List<Post> = withContext(Dispatchers.IO) {
        apiService.getPosts()
    }
    
    // Database operations on IO dispatcher
    suspend fun getCachedUsers(): List<User> = withContext(Dispatchers.IO) {
        database.users().getAll()
    }
    
    suspend fun saveUsers(users: List<User>) = withContext(Dispatchers.IO) {
        database.users().insertAll(users)
    }
    
    // CPU-intensive operations on Default dispatcher
    suspend fun processImage(file: File): ProcessedImage = withContext(Dispatchers.Default) {
        // Heavy image processing
        processImageFile(file)
    }
    
    suspend fun parseLargeJson(json: String): Data = withContext(Dispatchers.Default) {
        // Heavy JSON parsing
        parseJsonData(json)
    }
    
    suspend fun calculateStatistics(data: List<DataPoint>): Statistics = withContext(Dispatchers.Default) {
        // Complex calculations
        calculateStats(data)
    }
    
    private fun processImageFile(file: File): ProcessedImage {
        // Image processing logic
        return ProcessedImage(file.name, 0, 0, byteArrayOf())
    }
    
    private fun parseJsonData(json: String): Data {
        // JSON parsing logic
        return Data(emptyList())
    }
    
    private fun calculateStats(data: List<DataPoint>): Statistics {
        // Calculation logic
        return Statistics(0, 0.0)
    }
}

// Data classes
data class User(val id: Int, val name: String, val email: String)
data class Post(val id: Int, val userId: Int, val title: String, val body: String)
data class ProcessedImage(val name: String, val width: Int, val height: Int, val data: ByteArray)
data class Data(val items: List<Any>)
data class DataPoint(val value: Double)
data class Statistics(val count: Int, val average: Double)
data class Database(val users: UsersDao) {
    fun users() = users
}
data class UsersDao(val getAll: () -> List<User>, val insertAll: (List<User>) -> Unit)
interface ApiService {
    suspend fun getUsers(): List<User>
    suspend fun getPosts(): List<Post>
}

// Optimized network client
class OptimizedNetworkClient {
    
    // Configure for specific use case
    fun createOptimizedClient(): OkHttpClient {
        return OkHttpClient.Builder()
            // IO-bound operations need different timeouts
            .connectTimeout(15, TimeUnit.SECONDS)
            .readTimeout(15, TimeUnit.SECONDS)
            .writeTimeout(15, TimeUnit.SECONDS)
            
            // Connection pool for IO operations
            .connectionPool(ConnectionPool(
                maxIdleConnections = 10,
                keepAliveDuration = 5,
                timeUnit = TimeUnit.MINUTES
            ))
            
            // Cache for responses
            // .cache(Cache(cacheDir, cacheSize))
            
            .build()
    }
}
```

## Section 3: ViewModel Thread Management

Properly managing threads in ViewModels ensures smooth UI updates.

```kotlin
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*

// Base ViewModel with thread management
abstract class BaseViewModel : ViewModel() {
    
    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading.asStateFlow()
    
    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error.asStateFlow()
    
    // Protected launch that handles errors
    protected fun launchOnIO(
        block: suspend CoroutineScope.() -> Unit
    ): Job {
        return viewModelScope.launch(Dispatchers.IO) {
            try {
                block()
            } catch (e: Exception) {
                _error.value = e.message
            }
        }
    }
    
    // Launch with loading state
    protected fun launchWithLoading(
        block: suspend CoroutineScope.() -> Unit
    ): Job {
        return viewModelScope.launch {
            _isLoading.value = true
            try {
                withContext(Dispatchers.IO) {
                    block()
                }
            } catch (e: Exception) {
                _error.value = e.message
            } finally {
                _isLoading.value = false
            }
        }
    }
    
    // Protected launch for default dispatcher
    protected fun launchOnDefault(
        block: suspend CoroutineScope.() -> Unit
    ): Job {
        return viewModelScope.launch(Dispatchers.Default) {
            try {
                block()
            } catch (e: Exception) {
                _error.value = e.message
            }
        }
    }
    
    fun clearError() {
        _error.value = null
    }
}

// User ViewModel extending base
class UserViewModel(
    private val repository: OptimizedRepository
) : BaseViewModel() {
    
    private val _users = MutableStateFlow<List<User>>(emptyList())
    val users: StateFlow<List<User>> = _users.asStateFlow()
    
    init {
        loadUsers()
    }
    
    fun loadUsers() {
        launchWithLoading {
            val cached = repository.getCachedUsers()
            if (cached.isNotEmpty()) {
                _users.value = cached
            }
            
            try {
                val remote = repository.fetchUsers()
                _users.value = remote
                repository.saveUsers(remote)
            } catch (e: Exception) {
                if (cached.isEmpty()) {
                    _error.value = "Failed to load users: ${e.message}"
                }
            }
        }
    }
    
    fun refreshUsers() {
        launchWithLoading {
            val remote = repository.fetchUsers()
            _users.value = remote
            repository.saveUsers(remote)
        }
    }
}

// Image processing ViewModel
class ImageViewModel(
    private val repository: OptimizedRepository
) : BaseViewModel() {
    
    private val _processedImages = MutableStateFlow<List<ProcessedImage>>(emptyList())
    val processedImages: StateFlow<List<ProcessedImage>> = _processedImages.asStateFlow()
    
    private val _processingProgress = MutableStateFlow(0)
    val processingProgress: StateFlow<Int> = _processingProgress.asStateFlow()
    
    fun processImages(files: List<File>) {
        viewModelScope.launch {
            _isLoading.value = true
            _processingProgress.value = 0
            
            val results = withContext(Dispatchers.Default) {
                files.mapIndexed { index, file ->
                    val processed = repository.processImage(file)
                    _processingProgress.value = ((index + 1) * 100) / files.size
                    processed
                }
            }
            
            _processedImages.value = results
            _isLoading.value = false
        }
    }
}
```

## Section 4: Production Example - Background Processing Service

This example demonstrates comprehensive background threading for heavy operations.

```kotlin
import android.app.Service
import android.content.Intent
import android.os.IBinder
import androidx.lifecycle.LifecycleService
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import android.app.NotificationChannel
import android.app.NotificationManager
import android.os.Build
import android.app.Notification

// Background task configuration
data class BackgroundTaskConfig(
    val taskId: String,
    val title: String,
    val description: String,
    val priority: Int = 0,
    val requiresNetwork: Boolean = false,
    val requiresCharging: Boolean = false
)

// Work result
sealed class WorkResult<out T> {
    data class Success<T>(val data: T) : WorkResult<T>()
    data class Failure(val error: String) : WorkResult<Nothing>()
    object Cancelled : WorkResult<Nothing>()
}

// Background task executor
class BackgroundTaskExecutor(
    private val service: LifecycleService
) {
    private val scope = service.viewModelScope
    private val activeTasks = ConcurrentHashMap<String, Job>()
    
    private val _taskStates = MutableStateFlow<Map<String, TaskStatus>>(emptyMap())
    val taskStates: StateFlow<Map<String, TaskStatus>> = _taskStates.asStateFlow()
    
    sealed class TaskStatus {
        object Pending : TaskStatus()
        object Running : TaskStatus()
        data class Progress(val progress: Int) : TaskStatus()
        object Completed : TaskStatus()
        data class Failed(val error: String) : TaskStatus()
    }
    
    fun executeTask(
        config: BackgroundTaskConfig,
        work: suspend () -> Unit
    ): Job {
        // Cancel existing task with same ID
        activeTasks[config.taskId]?.cancel()
        
        val job = scope.launch(Dispatchers.Default) {
            try {
                updateTaskStatus(config.taskId, TaskStatus.Running)
                
                // Show notification
                showProgressNotification(config, 0)
                
                work()
                
                updateTaskStatus(config.taskId, TaskStatus.Completed)
                showCompletionNotification(config)
                
            } catch (e: CancellationException) {
                updateTaskStatus(config.taskId, TaskStatus.Cancelled)
            } catch (e: Exception) {
                updateTaskStatus(config.taskId, TaskStatus.Failed(e.message ?: "Error"))
            } finally {
                activeTasks.remove(config.taskId)
            }
        }
        
        activeTasks[config.taskId] = job
        return job
    }
    
    private fun updateTaskStatus(taskId: String, status: TaskStatus) {
        val currentStates = _taskStates.value.toMutableMap()
        currentStates[taskId] = status
        _taskStates.value = currentStates
    }
    
    private fun showProgressNotification(config: BackgroundTaskConfig, progress: Int) {
        // Create notification for progress
    }
    
    private fun showCompletionNotification(config: BackgroundTaskConfig) {
        // Create completion notification
    }
    
    fun cancelTask(taskId: String) {
        activeTasks[taskId]?.cancel()
        activeTasks.remove(taskId)
    }
    
    fun cancelAllTasks() {
        activeTasks.values.forEach { it.cancel() }
        activeTasks.clear()
    }
}

// Lifecycle-aware service
class BackgroundProcessingService : LifecycleService() {
    
    private lateinit var taskExecutor: BackgroundTaskExecutor
    
    override fun onCreate() {
        super.onCreate()
        taskExecutor = BackgroundTaskExecutor(this)
    }
    
    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        super.onStartCommand(intent, flags, startId)
        
        when (intent?.action) {
            ACTION_SYNC_USERS -> syncUsers()
            ACTION_PROCESS_IMAGES -> processImages()
            ACTION_CANCEL -> taskExecutor.cancelAllTasks()
        }
        
        return START_STICKY
    }
    
    private fun syncUsers() {
        taskExecutor.executeTask(
            BackgroundTaskConfig(
                taskId = "sync_users",
                title = "Syncing Users",
                description = "Downloading latest user data"
            )
        ) {
            // Sync logic here
            delay(5000)
        }
    }
    
    private fun processImages() {
        taskExecutor.executeTask(
            BackgroundTaskConfig(
                taskId = "process_images",
                title = "Processing Images",
                description = "Processing queued images"
            )
        ) {
            // Image processing logic
            delay(10000)
        }
    }
    
    override fun onDestroy() {
        taskExecutor.cancelAllTasks()
        super.onDestroy()
    }
    
    companion object {
        const val ACTION_SYNC_USERS = "com.example.ACTION_SYNC_USERS"
        const val ACTION_PROCESS_IMAGES = "com.example.ACTION_PROCESS_IMAGES"
        const val ACTION_CANCEL = "com.example.ACTION_CANCEL"
    }
}

// Notification helper
object NotificationHelper {
    
    private const val CHANNEL_ID = "background_tasks"
    
    fun createChannel(context: android.content.Context) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val channel = NotificationChannel(
                CHANNEL_ID,
                "Background Tasks",
                NotificationManager.IMPORTANCE_LOW
            ).apply {
                description = "Notifications for background tasks"
            }
            
            val notificationManager = context.getSystemService(NotificationManager::class.java)
            notificationManager.createNotificationChannel(channel)
        }
    }
    
    fun showProgress(context: android.content.Context, title: String, progress: Int) {
        val notification = Notification.Builder(context, CHANNEL_ID)
            .setContentTitle(title)
            .setSmallIcon(android.R.drawable.ic_popup_reminder)
            .setProgress(100, progress, false)
            .build()
        
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val notificationManager = context.getSystemService(NotificationManager::class.java)
            notificationManager.notify(title.hashCode(), notification)
        }
    }
}
```

## Section 5: Thread Optimization Techniques

Optimizing background work for better performance.

```kotlin
import kotlinx.coroutines.*
import java.util.concurrent.atomic.AtomicInteger

// Thread pool optimization
class ThreadPoolOptimizer {
    
    // Calculate optimal pool size
    fun calculateOptimalPoolSize(): Int {
        val cores = Runtime.getRuntime().availableProcessors()
        val coresForIO = cores * 2  // More threads for IO-bound work
        
        return when {
            cores <= 2 -> 2
            cores <= 4 -> cores * 2
            else -> cores + 1
        }
    }
    
    // Create optimized executor
    fun createOptimizedExecutor() = Executors.newFixedThreadPool(
        calculateOptimalPoolSize()
    )
    
    // Task batching for efficiency
    fun <T, R> batchTasks(
        items: List<T>,
        batchSize: Int,
        processor: (List<T>) -> List<R>
    ): List<R> = runBlocking(Dispatchers.Default) {
        items.chunked(batchSize)
            .map { batch ->
                async { processor(batch) }
            }
            .awaitAll()
            .flatten()
    }
}

// Memory-efficient processing
class MemoryEfficientProcessor {
    
    // Process large data in chunks
    suspend fun processLargeData(
        data: List<Long>,
        chunkSize: Int = 1000,
        processor: (List<Long>) -> Unit
    ) = withContext(Dispatchers.Default) {
        data.chunked(chunkSize).forEach { chunk ->
            processor(chunk)
            // Yield to prevent blocking
            yield()
        }
    }
    
    // Flow-based processing for large data
    fun processAsFlow(
        data: List<Long>,
        chunkSize: Int = 1000
    ): Flow<Long> = flow {
        data.chunked(chunkSize).forEach { chunk ->
            chunk.forEach { emit(it) }
            yield() // Emit control back to collector
        }
    }.flowOn(Dispatchers.Default)
    
    // Parallel processing with limited concurrency
    suspend fun <T, R> parallelProcess(
        items: List<T>,
        maxConcurrency: Int = 4,
        processor: suspend (T) -> R
    ): List<R> = withContext(Dispatchers.Default) {
        items.map { item ->
            async {
                processor(item)
            }
        }.awaitAll()
    }
}

// Coroutine pool for specific operations
class OperationDispatcher {
    
    private val ioDispatcher = Dispatchers.IO
    private val computationDispatcher = Dispatchers.Default
    private val networkDispatcher = Dispatchers.IO.limitedParallelism(10)
    private val databaseDispatcher = Dispatchers.IO.limitedParallelism(5)
    
    fun forNetwork() = networkDispatcher
    fun forDatabase() = databaseDispatcher
    fun forComputation() = computationDispatcher
    fun forIO() = ioDispatcher
}

// Progress tracking with atomic operations
class AtomicProgressTracker {
    
    private val processed = AtomicInteger(0)
    private val total: Int
    
    constructor(totalItems: Int) {
        total = totalItems
    }
    
    fun increment(): Int = processed.incrementAndGet()
    
    fun getProgress(): Int = (processed.get() * 100) / total
    
    fun isComplete(): Boolean = processed.get() >= total
}
```

## Best Practices

- **Use Appropriate Dispatchers**: IO for I/O, Default for CPU work, Main for UI
- **Avoid Main Thread Blocking**: Never do heavy work on main thread
- **Use Structured Concurrency**: Let coroutine scopes manage thread lifecycle
- **Limit Concurrency**: Use limitedParallelism to prevent overwhelming resources
- **Yield in Loops**: Call yield() in long loops to allow cancellation

## Common Pitfalls

**Problem**: Network on main thread
**Solution**: Always use Dispatchers.IO for network calls

**Problem**: Blocking the main thread
**Solution**: Use withContext to switch to background dispatcher

**Problem**: Too many concurrent operations
**Solution**: Use limitedParallelism to limit concurrency

**Problem**: Memory issues with large data
**Solution**: Process data in chunks using chunked()

## Troubleshooting Guide

**Q: Why is my app freezing?**
A: Check if background work is blocking main thread

**Q: How many threads should I use?**
A: Use Runtime.availableProcessors() for CPU work, 2x for I/O

**Q: Why is progress not updating?**
A: Ensure progress updates use proper Dispatchers

## Advanced Tips

- **WorkManager**: Use WorkManager for guaranteed background execution
- **Process Optimization**: Use profiling tools to identify bottlenecks
- **Thread Pools**: Create custom thread pools for specific operations

## Cross-References

- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Flow Implementation](./02_Flow_Implementation.md)
- [Async Task Management](./04_Async_Task_Management.md)
- [OkHttp Configuration](../01_HTTP_Communication/02_OkHttp_Configuration.md)
