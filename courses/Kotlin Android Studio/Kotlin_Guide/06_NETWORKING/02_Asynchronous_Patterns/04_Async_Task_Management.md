# Async Task Management

## Learning Objectives

1. Managing multiple concurrent async tasks
2. Implementing task cancellation and cleanup
3. Creating task dependencies and ordering
4. Using coroutine scopes for task management
5. Implementing progress reporting for async tasks

## Prerequisites

- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Flow Implementation](./02_Flow_Implementation.md)
- [Callback Patterns](./03_Callback_Patterns.md)

## Section 1: Task Management Fundamentals

Managing async tasks properly is crucial for responsive apps and resource management. Kotlin coroutines provide excellent tools for this.

Key concepts:
- CoroutineScope for task lifecycle
- Job for cancellation control
- SupervisorJob for fault tolerance
- Structured concurrency

```kotlin
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow

// Task state representation
sealed class TaskState<out T> {
    object Pending : TaskState<Nothing>()
    object Running : TaskState<Nothing>()
    data class Success<T>(val result: T) : TaskState<T>()
    data class Failure(val error: Throwable) : TaskState<Nothing>()
    object Cancelled : TaskState<Nothing>()
}

// Task wrapper for managing state
class ManagedTask<T>(
    private val scope: CoroutineScope
) {
    private var job: Job? = null
    private val _state = MutableStateFlow<TaskState<T>>(TaskState.Pending)
    val state: StateFlow<TaskState<T>> = _state
    
    fun execute(block: suspend () -> T) {
        cancel()
        
        job = scope.launch {
            _state.value = TaskState.Running
            try {
                val result = block()
                _state.value = TaskState.Success(result)
            } catch (e: CancellationException) {
                _state.value = TaskState.Cancelled
            } catch (e: Exception) {
                _state.value = TaskState.Failure(e)
            }
        }
    }
    
    fun cancel() {
        job?.cancel()
    }
    
    fun isActive(): Boolean = job?.isActive ?: false
    
    fun isCancelled(): Boolean = job?.isCancelled ?: false
}

// Simple task manager
class SimpleTaskManager(
    private val scope: CoroutineScope
) {
    private val tasks = mutableMapOf<String, Job>()
    
    fun <T> submitTask(
        id: String,
        block: suspend () -> T
    ): Deferred<T> {
        // Cancel existing task with same ID
        tasks[id]?.cancel()
        
        return scope.async {
            block()
        }.also { deferred ->
            tasks[id] = deferred
            deferred.invokeOnCompletion {
                tasks.remove(id)
            }
        }
    }
    
    fun cancelTask(id: String) {
        tasks[id]?.cancel()
        tasks.remove(id)
    }
    
    fun cancelAll() {
        tasks.values.forEach { it.cancel() }
        tasks.clear()
    }
    
    fun getTaskCount(): Int = tasks.size
}
```

## Section 2: Task Dependencies and Ordering

Managing task dependencies ensures proper execution order when tasks depend on each other.

```kotlin
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow

// Task dependency types
enum class DependencyType {
    NONE,       // No dependencies - run immediately
    WAIT_PREV,  // Wait for previous task
    WAIT_ALL    // Wait for all previous tasks
}

// Task with dependencies
data class Task<T>(
    val id: String,
    val block: suspend () -> T,
    val dependencies: List<String> = emptyList(),
    val retryCount: Int = 0,
    val timeout: Long = 0
)

// Task executor with dependency resolution
class TaskExecutor(
    private val scope: CoroutineScope
) {
    private val runningTasks = mutableMapOf<String, Job>()
    private val completedTasks = mutableMapOf<String, Any?>()
    private val taskResults = mutableMapOf<String, TaskResult<*>>()
    
    sealed class TaskResult<out T> {
        data class Success<T>(val result: T) : TaskResult<T>()
        data class Failure(val error: Throwable) : TaskResult<Nothing>()
        object Cancelled : TaskResult<Nothing>()
    }
    
    suspend fun <T> executeTask(task: Task<T>): T = coroutineScope {
        // Wait for dependencies
        task.dependencies.forEach { depId ->
            waitForTask(depId)
        }
        
        // Execute with retry
        var lastError: Throwable? = null
        repeat(task.retryCount + 1) { attempt ->
            if (attempt > 0) {
                delay(attempt * 1000L) // Exponential backoff
            }
            
            try {
                val result = withTimeout(task.timeout) {
                    task.block()
                }
                completedTasks[task.id] = result
                return@coroutineScope result
            } catch (e: Exception) {
                lastError = e
                if (e is CancellationException) throw e
            }
        }
        
        throw lastError ?: Exception("Task failed after retries")
    }
    
    private suspend fun waitForTask(depId: String) {
        val depJob = runningTasks[depId]
        depJob?.await()
        
        // Check if dependency succeeded
        val result = taskResults[depId]
        if (result is TaskResult.Failure) {
            throw Exception("Dependency $depId failed")
        }
    }
    
    fun cancelTask(taskId: String) {
        runningTasks[taskId]?.cancel()
    }
    
    fun cancelAll() {
        runningTasks.values.forEach { it.cancel() }
    }
}

// Sequential task execution
class SequentialTaskRunner(
    private val scope: CoroutineScope
) {
    private var previousDeferred: Deferred<*>? = null
    
    suspend fun <T> runSequentially(
        block: suspend () -> T
    ): T = coroutineScope {
        // Wait for previous task to complete
        previousDeferred?.await()
        
        // Run current task
        val deferred = async {
            block()
        }
        
        previousDeferred = deferred
        deferred.await()
    }
}

// Parallel task execution with barrier
class ParallelTaskRunner(
    private val scope: CoroutineScope
) {
    suspend fun <T> runInParallel(
        vararg blocks: suspend () -> T
    ): List<T> = coroutineScope {
        blocks.map { block ->
            async { block() }
        }.map { it.await() }
    }
    
    suspend fun runWithBarrier(
        vararg blocks: suspend () -> Unit
    ) = coroutineScope {
        // Launch all tasks
        val jobs = blocks.map { block ->
            launch { block() }
        }
        
        // Wait for all to complete
        jobs.forEach { it.join() }
    }
}
```

## Section 3: Progress Reporting

Implementing progress reporting for long-running async tasks.

```kotlin
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow

// Progress state
sealed class Progress {
    object Idle : Progress()
    data class Running(val current: Int, val total: Int, val message: String = "") : Progress()
    data class Success<T>(val result: T) : Progress()
    data class Error(val message: String) : Progress()
    object Cancelled : Progress()
}

// Progress reporter interface
interface ProgressReporter {
    val progress: StateFlow<Progress>
    
    fun reportProgress(current: Int, total: Int, message: String = "")
    fun reportSuccess(result: Any?)
    fun reportError(message: String)
    fun reportCancelled()
}

// Base progress reporter implementation
class BaseProgressReporter : ProgressReporter {
    private val _progress = MutableStateFlow<Progress>(Progress.Idle)
    override val progress: StateFlow<Progress> = _progress.asStateFlow()
    
    override fun reportProgress(current: Int, total: Int, message: String) {
        _progress.value = Progress.Running(current, total, message)
    }
    
    override fun reportSuccess(result: Any?) {
        _progress.value = Progress.Success(result!!)
    }
    
    override fun reportError(message: String) {
        _progress.value = Progress.Error(message)
    }
    
    override fun reportCancelled() {
        _progress.value = Progress.Cancelled
    }
}

// Progress-aware task
suspend fun withProgress(
    reporter: ProgressReporter,
    totalSteps: Int,
    block: suspend (ProgressReporter) -> Unit
) {
    try {
        reporter.reportProgress(0, totalSteps)
        block(reporter)
        reporter.reportSuccess(Unit)
    } catch (e: Exception) {
        reporter.reportError(e.message ?: "Unknown error")
    } catch (e: kotlinx.coroutines.CancellationException) {
        reporter.reportCancelled()
    }
}

// Download task with progress
class DownloadTask(
    private val scope: CoroutineScope,
    private val url: String,
    private val destination: String
) {
    private val _progress = MutableStateFlow(0)
    val progress: StateFlow<Int> = _progress.asStateFlow()
    
    private val _status = MutableStateFlow<DownloadStatus>(DownloadStatus.Idle)
    val status: StateFlow<DownloadStatus> = _status.asStateFlow()
    
    sealed class DownloadStatus {
        object Idle : DownloadStatus()
        object Downloading : DownloadStatus()
        data class Progress(val bytes: Long, val total: Long) : DownloadStatus()
        object Completed : DownloadStatus()
        data class Failed(val error: String) : DownloadStatus()
    }
    
    private var job: Job? = null
    
    fun start() {
        job = scope.launch {
            _status.value = DownloadStatus.Downloading
            
            try {
                // Simulate download with progress
                val totalSize = 1000L
                for (progress in 0..100 step 10) {
                    delay(100)
                    _progress.value = progress
                    _status.value = DownloadStatus.Progress(
                        bytes = (totalSize * progress / 100),
                        total = totalSize
                    )
                }
                _status.value = DownloadStatus.Completed
            } catch (e: Exception) {
                _status.value = DownloadStatus.Failed(e.message ?: "Download failed")
            }
        }
    }
    
    fun cancel() {
        job?.cancel()
        _status.value = DownloadStatus.Idle
    }
}
```

## Section 4: Production Example - Complete Task Management System

This example demonstrates a complete task management system with progress, cancellation, and error handling.

```kotlin
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import java.util.concurrent.ConcurrentHashMap

// Task configuration
data class TaskConfig(
    val id: String,
    val name: String,
    val description: String = "",
    val priority: Int = 0,  // Higher = more important
    val retryOnFailure: Boolean = true,
    val maxRetries: Int = 3,
    val timeout: Long = 30000
)

// Task result
sealed class TaskResult<out T> {
    data class Success<T>(val data: T) : TaskResult<T>()
    data class Error(val message: String, val throwable: Throwable? = null) : TaskResult<Nothing>()
    object Cancelled : TaskResult<Nothing>()
}

// Managed task interface
interface ManagedTask {
    val id: String
    val isActive: Boolean
    fun cancel()
}

// Task manager implementation
class TaskManager(
    private val scope: CoroutineScope
) {
    private val tasks = ConcurrentHashMap<String, Job>()
    private val taskResults = ConcurrentHashMap<String, TaskResult<*>>()
    private val taskConfigs = ConcurrentHashMap<String, TaskConfig>()
    
    private val _taskList = MutableStateFlow<List<String>>(emptyList())
    val taskList: StateFlow<List<String>> = _taskList.asStateFlow()
    
    // Submit a new task
    fun <T> submitTask(
        config: TaskConfig,
        block: suspend () -> T
    ): Deferred<TaskResult<T>> {
        // Cancel existing task with same ID
        cancelTask(config.id)
        
        taskConfigs[config.id] = config
        
        return scope.async {
            executeWithRetry(config) { block() }
        }.also { deferred ->
            tasks[config.id] = deferred
            updateTaskList()
            
            deferred.invokeOnCompletion { cause ->
                val result = if (cause == null) {
                    try {
                        TaskResult.Success(deferred.getCompleted())
                    } catch (e: Exception) {
                        TaskResult.Error(e.message ?: "Unknown error", e)
                    }
                } else if (cause is CancellationException) {
                    TaskResult.Cancelled
                } else {
                    TaskResult.Error(cause.message ?: "Task failed", cause)
                }
                
                taskResults[config.id] = result
                
                if (cause is CancellationException) {
                    tasks.remove(config.id)
                    updateTaskList()
                }
            }
        }
    }
    
    private suspend fun <T> executeWithRetry(
        config: TaskConfig,
        block: suspend () -> T
    ): T {
        var lastError: Throwable? = null
        
        repeat(config.maxRetries) { attempt ->
            if (attempt > 0 && config.retryOnFailure) {
                delay((attempt * 1000L).coerceAtMost(10000L))
            }
            
            try {
                return withTimeout(config.timeout) {
                    block()
                }
            } catch (e: CancellationException) {
                throw e
            } catch (e: Exception) {
                lastError = e
                if (!config.retryOnFailure) throw e
            }
        }
        
        throw lastError ?: Exception("Task failed after ${config.maxRetries} retries")
    }
    
    // Cancel specific task
    fun cancelTask(id: String) {
        tasks[id]?.cancel()
        tasks.remove(id)
        updateTaskList()
    }
    
    // Cancel all tasks
    fun cancelAll() {
        tasks.values.forEach { it.cancel() }
        tasks.clear()
        updateTaskList()
    }
    
    // Get task result
    fun getResult(id: String): TaskResult<*>? = taskResults[id]
    
    // Check if task is running
    fun isTaskRunning(id: String): Boolean = tasks[id]?.isActive ?: false
    
    private fun updateTaskList() {
        _taskList.value = tasks.keys.toList()
    }
}

// Repository with task management
class ManagedRepository(
    private val taskManager: TaskManager
) {
    private val apiService = ApiServiceHolder.apiService
    
    fun loadUsers(): Deferred<TaskResult<List<User>>> {
        return taskManager.submitTask(
            TaskConfig(
                id = "load_users",
                name = "Load Users",
                description = "Fetching user list from server",
                priority = 1
            )
        ) {
            apiService.getUsers()
        }
    }
    
    fun loadUserPosts(userId: Int): Deferred<TaskResult<List<Post>>> {
        return taskManager.submitTask(
            TaskConfig(
                id = "load_posts_$userId",
                name = "Load User Posts",
                description = "Fetching posts for user $userId",
                priority = 2
            )
        ) {
            apiService.getPostsByUser(userId)
        }
    }
    
    fun refreshAll(): Deferred<TaskResult<Unit>> {
        return taskManager.submitTask(
            TaskConfig(
                id = "refresh_all",
                name = "Refresh All Data",
                description = "Refreshing all cached data",
                priority = 0
            )
        ) {
            // Refresh users and posts in parallel
            coroutineScope {
                val usersDeferred = async { apiService.getUsers() }
                val postsDeferred = async { apiService.getPosts() }
                
                usersDeferred.await()
                postsDeferred.await()
            }
            Unit
        }
    }
}

// ViewModel with task management
class TaskViewModel(
    private val repository: ManagedRepository
) : ViewModel() {
    
    private val taskManager = TaskManager(viewModelScope)
    
    private val _users = MutableStateFlow<List<User>>(emptyList())
    val users: StateFlow<List<User>> = _users.asStateFlow()
    
    private val _posts = MutableStateFlow<List<Post>>(emptyList())
    val posts: StateFlow<List<Post>> = _posts.asStateFlow()
    
    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading.asStateFlow()
    
    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error.asStateFlow()
    
    private val _activeTasks = taskManager.taskList
    val activeTasks: StateFlow<List<String>> = _activeTasks
    
    fun loadUsers() {
        _isLoading.value = true
        
        val task = repository.loadUsers()
        
        viewModelScope.launch {
            val result = task.await()
            
            when (result) {
                is TaskResult.Success -> {
                    _users.value = result.data
                    _error.value = null
                }
                is TaskResult.Error -> {
                    _error.value = result.message
                }
                is TaskResult.Cancelled -> {
                    _error.value = "Task was cancelled"
                }
            }
            
            _isLoading.value = false
        }
    }
    
    fun loadUserPosts(userId: Int) {
        val task = repository.loadUserPosts(userId)
        
        viewModelScope.launch {
            val result = task.await()
            
            when (result) {
                is TaskResult.Success -> _posts.value = result.data
                is TaskResult.Error -> _error.value = result.message
                is TaskResult.Cancelled -> {}
            }
        }
    }
    
    fun refreshAll() {
        _isLoading.value = true
        
        val task = repository.refreshAll()
        
        viewModelScope.launch {
            when (val result = task.await()) {
                is TaskResult.Success -> {
                    loadUsers()
                }
                is TaskResult.Error -> {
                    _error.value = result.message
                    _isLoading.value = false
                }
                is TaskResult.Cancelled -> {
                    _isLoading.value = false
                }
            }
        }
    }
    
    fun cancelTask(taskId: String) {
        taskManager.cancelTask(taskId)
    }
    
    fun cancelAllTasks() {
        taskManager.cancelAll()
    }
    
    override fun onCleared() {
        super.onCleared()
        taskManager.cancelAll()
    }
}
```

## Section 5: Task Priority and Queue Management

Implementing priority-based task execution and queue management.

```kotlin
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import java.util.PriorityQueue
import java.util.concurrent.Executors

// Task priority levels
enum class TaskPriority(val value: Int) {
    CRITICAL(100),
    HIGH(75),
    NORMAL(50),
    LOW(25)
}

// Priority task wrapper
data class PriorityTask<T>(
    val priority: TaskPriority,
    val block: suspend () -> T
)

// Priority queue manager
class PriorityQueueManager(
    private val scope: CoroutineScope,
    private val maxConcurrent: Int = 3
) {
    private val waitingTasks = PriorityQueue<PriorityTask<*>>(
        compareByDescending { it.priority.value }
    )
    
    private val runningJobs = mutableSetOf<Job>()
    private val results = mutableMapOf<String, TaskResult<*>>()
    
    private val _queueSize = MutableStateFlow(0)
    val queueSize: StateFlow<Int> = _queueSize.asStateFlow()
    
    private val _runningCount = MutableStateFlow(0)
    val runningCount: StateFlow<Int> = _runningCount.asStateFlow()
    
    suspend fun <T> submitWithPriority(
        priority: TaskPriority,
        block: suspend () -> T
    ): T = coroutineScope {
        val task = PriorityTask(priority, block)
        
        // Add to queue
        waitingTasks.offer(task)
        updateQueueSize()
        
        // Wait for available slot
        while (runningJobs.size >= maxConcurrent) {
            delay(100)
        }
        
        // Execute task
        val deferred = async {
            block()
        }
        
        runningJobs.add(deferred)
        _runningCount.value = runningJobs.size
        
        try {
            val result = deferred.await()
            result
        } finally {
            runningJobs.remove(deferred)
            _runningCount.value = runningJobs.size
            updateQueueSize()
        }
    }
    
    private fun updateQueueSize() {
        _queueSize.value = waitingTasks.size
    }
}

// Coroutine dispatcher for task prioritization
class PrioritizedDispatcher(
    private val maxParallel: Int = 4
) : CoroutineDispatcher() {
    private val queue = ArrayDeque<Runnable>()
    private var running = 0
    
    override fun dispatch(context: CoroutineContext, block: Runnable) {
        synchronized(queue) {
            if (running < maxParallel) {
                running++
                executeNext(block)
            } else {
                queue.addLast(block)
            }
        }
    }
    
    private fun executeNext(block: Runnable) {
        Executors.newSingleThreadExecutor().execute {
            try {
                block.run()
            } finally {
                synchronized(queue) {
                    val next = queue.pollFirst()
                    if (next != null) {
                        executeNext(next)
                    } else {
                        running--
                    }
                }
            }
        }
    }
}
```

## Best Practices

- **Use Structured Concurrency**: Use CoroutineScope for task lifecycle
- **Implement Cancellation**: Check isActive in long-running tasks
- **Use SupervisorJob**: Prevent one task failure from cancelling others
- **Handle All States**: Implement proper handling for success, failure, and cancellation
- **Report Progress**: Provide progress updates for long-running tasks

## Common Pitfalls

**Problem**: Tasks not being cancelled
**Solution**: Check isActive in task loops and use proper scope

**Problem**: Memory leaks from background tasks
**Solution**: Use proper lifecycle-bound coroutine scopes

**Problem**: Race conditions with shared resources
**Solution**: Use synchronization or structured concurrency

## Troubleshooting Guide

**Q: Why are tasks running out of order?**
A: Implement dependency tracking or use sequential execution

**Q: How to cancel dependent tasks?**
A: Use parent Job hierarchy with Structured Concurrency

**Q: Why is progress not updating?**
A: Ensure progress updates are on main thread or use proper StateFlow

## Advanced Tips

- **Custom Dispatchers**: Create dispatchers for specific task types
- **Task Batching**: Group similar tasks for efficient processing
- **WorkManager**: Use WorkManager for persistent background tasks

## Cross-References

- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Flow Implementation](./02_Flow_Implementation.md)
- [Background Threading](./05_Background_Threading.md) - Thread management
