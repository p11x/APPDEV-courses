# Work Manager

## Overview

WorkManager is Android's solution for background task scheduling. It provides a robust, declarative API for deferrable work that survives app restarts and device reboots.

## Learning Objectives

- Implement background tasks with WorkManager
- Configure work constraints and retry policies
- Chain complex work operations
- Handle work states and observe progress
- Optimize battery and system resources

## Prerequisites

- [Coroutines Basics](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Background Threading](../06_NETWORKING/02_Asynchronous_Patterns/05_Background_Threading.md)

## Core Concepts

### WorkRequest Types

WorkManager supports two request types:
- OneTimeWorkRequest: Single execution
- PeriodicWorkRequest: Scheduled recurring work

### Constraints

Work constraints control when work executes:
- Network type (connected/unmetered)
- Battery not low
- Device idle
- Storage not low

### Work States

Work progresses through states: ENQUEUED, RUNNING, SUCCEEDED, FAILED, BLOCKED, CANCELLED

## Code Examples

### Example 1: Basic Work Implementation

```kotlin
import android.content.Context
import androidx.work.*
import kotlinx.coroutines.delay
import java.util.concurrent.TimeUnit

/**
 * Simple worker for data sync
 * Demonstrates basic Worker implementation
 */
class DataSyncWorker(
    private val context: Context,
    workerParams: WorkerParameters
) : CoroutineWorker(context, workerParams) {
    
    override suspend fun doWork(): Result {
        return try {
            val userId = inputData.getString(KEY_USER_ID) ?: return Result.failure()
            
            // Simulate sync operation
            val syncedData = syncData(userId)
            
            // Output result
            val outputData = workDataOf(
                KEY_SYNCED_COUNT to syncedData,
                KEY_TIMESTAMP to System.currentTimeMillis()
            )
            
            Result.success(outputData)
        } catch (e: Exception) {
            if (runAttemptCount < MAX_RETRIES) {
                Result.retry()
            } else {
                Result.failure(workDataOf(KEY_ERROR to e.message))
            }
        }
    }
    
    private suspend fun syncData(userId: String): Int {
        // Simulate network delay
        delay(1000)
        return 10 // Number of items synced
    }
    
    companion object {
        const val KEY_USER_ID = "user_id"
        const val KEY_SYNCED_COUNT = "synced_count"
        const val KEY_TIMESTAMP = "timestamp"
        const val KEY_ERROR = "error"
        const val MAX_RETRIES = 3
    }
}

/**
 * WorkManager configuration and scheduling
 */
class WorkScheduler(private val context: Context) {
    
    /**
     * Schedule one-time sync work
     */
    fun scheduleDataSync(userId: String) {
        val constraints = Constraints.Builder()
            .setRequiredNetworkType(NetworkType.CONNECTED)
            .setRequiresBatteryNotLow(true)
            .build()
        
        val inputData = workDataOf(
            DataSyncWorker.KEY_USER_ID to userId
        )
        
        val syncRequest = OneTimeWorkRequestBuilder<DataSyncWorker>()
            .setConstraints(constraints)
            .setInputData(inputData)
            .setBackoffCriteria(
                BackoffPolicy.EXPONENTIAL,
                10, TimeUnit.SECONDS
            )
            .addTag("data_sync")
            .build()
        
        WorkManager.getInstance(context)
            .enqueueUniqueWork(
                "data_sync_$userId",
                ExistingWorkPolicy.REPLACE,
                syncRequest
            )
    }
    
    /**
     * Schedule periodic backup
     */
    fun schedulePeriodicBackup() {
        val constraints = Constraints.Builder()
            .setRequiredNetworkType(NetworkType.UNMETERED)
            .setRequiresBatteryNotLow(true)
            .setRequiresCharging(true)
            .build()
        
        val backupRequest = PeriodicWorkRequestBuilder<BackupWorker>(
            24, TimeUnit.HOURS,
            1, TimeUnit.HOURS // Flex interval
        )
            .setConstraints(constraints)
            .addTag("backup")
            .build()
        
        WorkManager.getInstance(context)
            .enqueueUniquePeriodicWork(
                "periodic_backup",
                ExistingPeriodicWorkPolicy.KEEP,
                backupRequest
            )
    }
    
    /**
     * Cancel specific work
     */
    fun cancelWork(workId: String) {
        WorkManager.getInstance(context).cancelWorkById(java.util.UUID.fromString(workId))
    }
    
    /**
     * Cancel all work with tag
     */
    fun cancelAllDataSyncWork() {
        WorkManager.getInstance(context).cancelAllWorkByTag("data_sync")
    }
}

/**
 * Periodic backup worker
 */
class BackupWorker(
    private val context: Context,
    workerParams: WorkerParameters
) : CoroutineWorker(context, workerParams) {
    
    override suspend fun doWork(): Result {
        return try {
            performBackup()
            Result.success()
        } catch (e: Exception) {
            Result.retry()
        }
    }
    
    private suspend fun performBackup() {
        // Simulate backup operation
        delay(2000)
    }
}
```

**Output:**
```
Work scheduled: data_sync_user123
Work scheduled: periodic_backup
Work status: SUCCEEDED
```

### Example 2: Complex Work Chaining

```kotlin
import androidx.work.*
import kotlinx.coroutines.delay

/**
 * Worker for fetching data from API
 */
class FetchDataWorker(
    context: Context,
    params: WorkerParameters
) : CoroutineWorker(context, params) {
    
    override suspend fun doWork(): Result {
        return try {
            val page = inputData.getInt(KEY_PAGE, 0)
            val pageSize = inputData.getInt(KEY_PAGE_SIZE, 20)
            
            val data = fetchPage(page, pageSize)
            
            val output = workDataOf(
                KEY_DATA to data,
                KEY_HAS_MORE to (data.size == pageSize)
            )
            
            Result.success(output)
        } catch (e: Exception) {
            Result.failure(workDataOf(KEY_ERROR to e.message))
        }
    }
    
    private suspend fun fetchPage(page: Int, pageSize: Int): List<String> {
        delay(500)
        return (page * pageSize until (page + 1) * pageSize).map { "Item $it" }
    }
    
    companion object {
        const val KEY_PAGE = "page"
        const val KEY_PAGE_SIZE = "page_size"
        const val KEY_DATA = "data"
        const val KEY_HAS_MORE = "has_more"
        const val KEY_ERROR = "error"
    }
}

/**
 * Worker for processing fetched data
 */
class ProcessDataWorker(
    context: Context,
    params: WorkerParameters
) : CoroutineWorker(context, params) {
    
    override suspend fun doWork(): Result {
        return try {
            val data = inputData.getStringArray(KEY_DATA) ?: return Result.failure()
            
            val processed = processItems(data)
            
            Result.success(workDataOf(
                KEY_PROCESSED_COUNT to processed.size,
                KEY_PROCESSED_DATA to processed.toTypedArray()
            ))
        } catch (e: Exception) {
            Result.failure(workDataOf(KEY_ERROR to e.message))
        }
    }
    
    private suspend fun processItems(data: Array<String>): List<String> {
        delay(300)
        return data.map { "Processed: $it" }
    }
    
    companion object {
        const val KEY_DATA = "data"
        const val KEY_PROCESSED_COUNT = "processed_count"
        const val KEY_PROCESSED_DATA = "processed_data"
        const val KEY_ERROR = "error"
    }
}

/**
 * Worker for saving data to database
 */
class SaveDataWorker(
    context: Context,
    params: WorkerParameters
) : CoroutineWorker(context, params) {
    
    override suspend fun doWork(): Result {
        return try {
            val data = inputData.getStringArray(KEY_DATA) ?: return Result.failure()
            
            saveToDatabase(data)
            
            Result.success(workDataOf(KEY_SAVED_COUNT to data.size))
        } catch (e: Exception) {
            Result.failure(workDataOf(KEY_ERROR to e.message))
        }
    }
    
    private suspend fun saveToDatabase(data: Array<String>) {
        delay(200)
    }
    
    companion object {
        const val KEY_DATA = "data"
        const val KEY_SAVED_COUNT = "saved_count"
        const val KEY_ERROR = "error"
    }
}

/**
 * Complex work chain manager
 */
class WorkChainManager(private val context: Context) {
    
    /**
     * Create parallel fetch operations
     */
    fun parallelFetch() {
        val constraints = Constraints.Builder()
            .setRequiredNetworkType(NetworkType.CONNECTED)
            .build()
        
        // Create multiple parallel fetch requests
        val fetchRequests = (0..2).map { page ->
            val inputData = workDataOf(
                FetchDataWorker.KEY_PAGE to page,
                FetchDataWorker.KEY_PAGE_SIZE to 20
            )
            
            OneTimeWorkRequestBuilder<FetchDataWorker>()
                .setConstraints(constraints)
                .setInputData(inputData)
                .build()
        }
        
        // Enqueue in parallel
        WorkManager.getInstance(context)
            .enqueue(fetchRequests)
    }
    
    /**
     * Create sequential work chain
     */
    fun sequentialWork() {
        val fetchWork = OneTimeWorkRequestBuilder<FetchDataWorker>()
            .setInputData(workDataOf(
                FetchDataWorker.KEY_PAGE to 0,
                FetchDataWorker.KEY_PAGE_SIZE to 100
            ))
            .build()
        
        val processWork = OneTimeWorkRequestBuilder<ProcessDataWorker>()
            .build()
        
        val saveWork = OneTimeWorkRequestBuilder<SaveDataWorker>()
            .build()
        
        // Chain work sequentially
        WorkManager.getInstance(context)
            .beginWith(fetchWork)
            .then(processWork)
            .then(saveWork)
            .enqueue()
    }
    
    /**
     * Create complex parallel-then-sequential chain
     */
    fun complexChain() {
        // Parallel fetch phase
        val fetchRequests = (0..2).map { page ->
            OneTimeWorkRequestBuilder<FetchDataWorker>()
                .setInputData(workDataOf(
                    FetchDataWorker.KEY_PAGE to page,
                    FetchDataWorker.KEY_PAGE_SIZE to 20
                ))
                .build()
        }
        
        // Process phase (sequential after parallel)
        val processWork = OneTimeWorkRequestBuilder<ProcessDataWorker>()
            .build()
        
        // Save phase
        val saveWork = OneTimeWorkRequestBuilder<SaveDataWorker>()
            .build()
        
        // Build complex chain
        WorkManager.getInstance(context)
            .enqueueUniqueWork(
                "complex_chain",
                ExistingWorkPolicy.REPLACE,
                createWorkGroup(fetchRequests, processWork, saveWork)
            )
    }
    
    private fun createWorkGroup(
        parallelWork: List<OneTimeWorkRequest<*>>,
        sequentialWork: OneTimeWorkRequest<*>,
        finalWork: OneTimeWorkRequest<*>
    ): OneTimeWorkRequest {
        // Combine parallel work into continuation
        val parallelContinuation = WorkManager.getInstance(context)
            .beginWith(parallelWork)
        
        // Add sequential work
        val chain = parallelContinuation.then(sequentialWork).then(finalWork)
        
        // This creates a combined work request
        return OneTimeWorkRequestBuilder<CombinedWorker>()
            .setInputData(workDataOf(KEY_CHAIN_ID to "complex_chain"))
            .build()
    }
    
    companion object {
        const val KEY_CHAIN_ID = "chain_id"
    }
}

/**
 * Worker that observes work chain status
 */
class WorkObserver(private val context: Context) {
    
    fun observeWork(workId: String) {
        WorkManager.getInstance(context)
            .getWorkInfoByIdLiveData(java.util.UUID.fromString(workId))
            .observeForever { workInfo ->
                when (workInfo?.state) {
                    WorkInfo.State.ENQUEUED -> println("Work enqueued")
                    WorkInfo.State.RUNNING -> {
                        val progress = workInfo.progress.getInt(KEY_PROGRESS, 0)
                        println("Work running: $progress%")
                    }
                    WorkInfo.State.SUCCEEDED -> println("Work succeeded")
                    WorkInfo.State.FAILED -> println("Work failed")
                    WorkInfo.State.CANCELLED -> println("Work cancelled")
                    else -> {}
                }
            }
    }
    
    /**
     * Get work infos by tag
     */
    fun getWorkInfosByTag(tag: String) {
        WorkManager.getInstance(context)
            .getWorkInfosByTagLiveData(tag)
            .observeForever { workInfos ->
                println("Work count: ${workInfos.size}")
                workInfos.forEach { info ->
                    println("  ${info.id}: ${info.state}")
                }
            }
    }
    
    companion object {
        const val KEY_PROGRESS = "progress"
    }
}

/**
 * Combined worker for complex chains
 */
class CombinedWorker(
    context: Context,
    params: WorkerParameters
) : CoroutineWorker(context, params) {
    
    override suspend fun doWork(): Result {
        return Result.success()
    }
}
```

**Output:**
```
Work enqueued
Work running: 50%
Work succeeded
Work count: 3
  work_id_1: SUCCEEDED
  work_id_2: SUCCEEDED
  work_id_3: SUCCEEDED
```

### Example 3: Production Work Implementation

```kotlin
import android.content.Context
import androidx.work.*
import kotlinx.coroutines.delay

/**
 * Worker for uploading files to cloud
 */
class UploadWorker(
    private val context: Context,
    workerParams: WorkerParameters
) : CoroutineWorker(context, workerParams) {
    
    override suspend fun doWork(): Result {
        val filePaths = inputData.getStringArray(KEY_FILE_PATHS) 
            ?: return Result.failure(workDataOf(KEY_ERROR to "No files specified"))
        
        val uploadedPaths = mutableListOf<String>()
        val failedPaths = mutableListOf<String>()
        
        for ((index, path) in filePaths.withIndex()) {
            setProgress(workDataOf(
                KEY_PROGRESS to ((index + 1) * 100 / filePaths.size),
                KEY_CURRENT_FILE to path
            ))
            
            try {
                val uploaded = uploadFile(path)
                if (uploaded) {
                    uploadedPaths.add(path)
                } else {
                    failedPaths.add(path)
                }
            } catch (e: Exception) {
                failedPaths.add(path)
            }
        }
        
        return if (failedPaths.isEmpty()) {
            Result.success(workDataOf(
                KEY_UPLOADED_COUNT to uploadedPaths.size,
                KEY_UPLOADED_PATHS to uploadedPaths.toTypedArray()
            ))
        } else if (runAttemptCount < MAX_RETRIES) {
            Result.retry()
        } else {
            Result.success(workDataOf(
                KEY_UPLOADED_COUNT to uploadedPaths.size,
                KEY_FAILED_COUNT to failedPaths.size,
                KEY_FAILED_PATHS to failedPaths.toTypedArray()
            ))
        }
    }
    
    private suspend fun uploadFile(path: String): Boolean {
        delay(1000)
        return true
    }
    
    companion object {
        const val KEY_FILE_PATHS = "file_paths"
        const val KEY_UPLOADED_COUNT = "uploaded_count"
        const val KEY_FAILED_COUNT = "failed_count"
        const val KEY_UPLOADED_PATHS = "uploaded_paths"
        const val KEY_FAILED_PATHS = "failed_paths"
        const val KEY_PROGRESS = "progress"
        const val KEY_CURRENT_FILE = "current_file"
        const val KEY_ERROR = "error"
        const val MAX_RETRIES = 3
    }
}

/**
 * Worker for notification scheduling
 */
class NotificationWorker(
    private val context: Context,
    workerParams: WorkerParameters
) : CoroutineWorker(context, workerParams) {
    
    override suspend fun doWork(): Result {
        val notificationId = inputData.getInt(KEY_NOTIFICATION_ID, 0)
        val title = inputData.getString(KEY_TITLE) ?: "Notification"
        val message = inputData.getString(KEY_MESSAGE) ?: ""
        
        showNotification(notificationId, title, message)
        
        return Result.success()
    }
    
    private fun showNotification(id: Int, title: String, message: String) {
        // Implementation would use NotificationManager
        println("Showing notification: $title - $message")
    }
    
    companion object {
        const val KEY_NOTIFICATION_ID = "notification_id"
        const val KEY_TITLE = "title"
        const val KEY_MESSAGE = "message"
    }
}

/**
 * Production work manager with complex requirements
 */
class ProductionWorkManager(private val context: Context) {
    
    /**
     * Schedule data sync with full configuration
     */
    fun scheduleFullSync() {
        val constraints = Constraints.Builder()
            .setRequiredNetworkType(NetworkType.CONNECTED)
            .setRequiresBatteryNotLow(true)
            .setRequiresStorageNotLow(true)
            .build()
        
        val inputData = workDataOf(
            SyncWorker.KEY_SYNC_TYPE to SyncWorker.SYNC_TYPE_FULL
        )
        
        val syncRequest = OneTimeWorkRequestBuilder<SyncWorker>(SyncWorker::class.java)
            .setConstraints(constraints)
            .setInputData(inputData)
            .setInitialDelay(5, TimeUnit.MINUTES)
            .setBackoffCriteria(
                BackoffPolicy.EXPONENTIAL,
                WorkRequest.MIN_BACKOFF_MILLIS,
                TimeUnit.MILLISECONDS
            )
            .addTag(TAG_SYNC)
            .addTag(TAG_BACKGROUND)
            .build()
        
        WorkManager.getInstance(context)
            .enqueueUniqueWork(
                WORK_FULL_SYNC,
                ExistingWorkPolicy.KEEP,
                syncRequest
            )
    }
    
    /**
     * Schedule incremental sync
     */
    fun scheduleIncrementalSync() {
        val constraints = Constraints.Builder()
            .setRequiredNetworkType(NetworkType.CONNECTED)
            .build()
        
        val syncRequest = OneTimeWorkRequestBuilder<SyncWorker>()
            .setConstraints(constraints)
            .setInputData(workDataOf(
                SyncWorker.KEY_SYNC_TYPE to SyncWorker.SYNC_TYPE_INCREMENTAL
            ))
            .addTag(TAG_SYNC)
            .build()
        
        WorkManager.getInstance(context)
            .enqueueUniqueWork(
                WORK_INCREMENTAL_SYNC,
                ExistingWorkPolicy.REPLACE,
                syncRequest
            )
    }
    
    /**
     * Schedule cleanup work
     */
    fun scheduleCleanup() {
        val cleanupRequest = PeriodicWorkRequestBuilder<CleanupWorker>(
            7, TimeUnit.DAYS
        )
            .setConstraints(Constraints.Builder()
                .setRequiresBatteryNotLow(true)
                .setRequiresDeviceIdle(true)
                .build())
            .addTag(TAG_CLEANUP)
            .build()
        
        WorkManager.getInstance(context)
            .enqueueUniquePeriodicWork(
                WORK_CLEANUP,
                ExistingPeriodicWorkPolicy.KEEP,
                cleanupRequest
            )
    }
    
    /**
     * Get work status
     */
    fun getWorkStatus(workName: String) {
        WorkManager.getInstance(context)
            .getWorkInfosForUniqueWorkLiveData(workName)
            .observeForever { workInfos ->
                workInfos?.forEach { info ->
                    println("Work: ${info.id}, State: ${info.state}")
                }
            }
    }
    
    /**
     * Cancel all scheduled work
     */
    fun cancelAllScheduledWork() {
        WorkManager.getInstance(context).cancelAllWork()
    }
    
    companion object {
        const val TAG_SYNC = "sync"
        const val TAG_BACKGROUND = "background"
        const val TAG_CLEANUP = "cleanup"
        const val TAG_UPLOAD = "upload"
        
        const val WORK_FULL_SYNC = "full_sync"
        const val WORK_INCREMENTAL_SYNC = "incremental_sync"
        const val WORK_CLEANUP = "cleanup"
        const val WORK_UPLOAD = "upload"
    }
}

/**
 * Sync worker with detailed implementation
 */
class SyncWorker(
    context: Context,
    params: WorkerParameters
) : CoroutineWorker(context, params) {
    
    override suspend fun doWork(): Result {
        val syncType = inputData.getInt(KEY_SYNC_TYPE, SYNC_TYPE_FULL)
        
        return try {
            setProgress(workDataOf(KEY_PROGRESS to 10))
            
            syncMetadata()
            setProgress(workDataOf(KEY_PROGRESS to 30))
            
            syncUsers()
            setProgress(workDataOf(KEY_PROGRESS to 50))
            
            syncData(syncType)
            setProgress(workDataOf(KEY_PROGRESS to 80))
            
            syncMedia()
            setProgress(workDataOf(KEY_PROGRESS to 100))
            
            Result.success(workDataOf(
                KEY_SYNC_TIME to System.currentTimeMillis(),
                KEY_SYNC_TYPE to syncType
            ))
        } catch (e: Exception) {
            Result.failure(workDataOf(KEY_ERROR to e.message))
        }
    }
    
    private suspend fun syncMetadata() { delay(200) }
    private suspend fun syncUsers() { delay(300) }
    private suspend fun syncData(type: Int) { delay(400) }
    private suspend fun syncMedia() { delay(500) }
    
    companion object {
        const val KEY_SYNC_TYPE = "sync_type"
        const val KEY_SYNC_TIME = "sync_time"
        const val KEY_PROGRESS = "progress"
        const val KEY_ERROR = "error"
        
        const val SYNC_TYPE_FULL = 0
        const val SYNC_TYPE_INCREMENTAL = 1
    }
}

/**
 * Cleanup worker for maintenance
 */
class CleanupWorker(
    context: Context,
    params: WorkerParameters
) : CoroutineWorker(context, params) {
    
    override suspend fun doWork(): Result {
        return try {
            clearOldCache()
            deleteTempFiles()
            optimizeDatabase()
            
            Result.success()
        } catch (e: Exception) {
            Result.retry()
        }
    }
    
    private suspend fun clearOldCache() { delay(100) }
    private suspend fun deleteTempFiles() { delay(100) }
    private suspend fun optimizeDatabase() { delay(100) }
}
```

**Output:**
```
Work enqueued
Progress: 10%
Progress: 30%
Progress: 50%
Progress: 80%
Progress: 100%
Work succeeded
Sync completed in 1500ms
```

## Best Practices

- Use setConstraints to control work execution
- Implement retry with exponential backoff
- Use tags for organizing and canceling work
- Observe work states with LiveData or Flow
- Keep workers lightweight and focused

## Common Pitfalls

### Problem: Work not executing
**Solution:** Check constraints are satisfied (network, battery, etc.)

### Problem: Duplicate work
**Solution:** Use ExistingWorkPolicy appropriately

### Problem: Work cancelled unexpectedly
**Solution:** Use setForeground for long-running work

## Troubleshooting Guide

**Q: Why isn't work running?**
A: Check constraints and battery status. Verify work is enqueued.

**Q: How to run work immediately?**
A: Use setExpedited or NO_CONSTRAINTS

**Q: How to handle work results?**
A: Use getWorkInfo or observe LiveData

## Cross-References

- [Coroutines Basics](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Background Threading](../06_NETWORKING/02_Asynchronous_Patterns/05_Background_Threading.md)
- [Notifications](./04_Notifications.md)