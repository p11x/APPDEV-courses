# Update Strategies

## Learning Objectives

1. Understanding in-app update mechanisms
2. Implementing flexible updates
3. Configuring update priority and timing
4. Handling update failures and rollbacks
5. Managing mandatory vs optional updates
6. Optimizing update user experience

## Prerequisites

- [Crash Reporting](./02_Crash_Reporting.md)
- [Release Management](../01_App_Distribution/04_Release_Management.md)

## Section 1: In-App Update Fundamentals

Google Play In-App Updates API allows you to prompt users to update your app without leaving the app. This creates a smoother user experience compared to redirecting to the Play Store. The API supports two update types: flexible updates and immediate updates.

Flexible updates allow users to continue using the app while the update downloads in the background. Once the download is complete, the app needs to be restarted to apply the update. This is appropriate for non-critical updates where the user shouldn't be interrupted.

Immediate updates are similar to what happens when a user clicks "Update" in the Play Store - the app restarts immediately to apply the update. This is appropriate for critical updates like security patches or important bug fixes.

```kotlin
// In-app update configuration and implementation
package com.example.myapp.update

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import com.google.android.gms.appupdate.AppUpdateInfo
import com.google.android.gms.appupdate.AppUpdateManager
import com.google.android.gms.appupdate.AppUpdateManagerFactory
import com.google.android.gms.appupdate.AppUpdateType
import com.google.android.gms.appupdate.InAppUpdateResult
import com.google.android.gms.appupdate.AppUpdateFlowParameters
import com.google.android.gms.common.ConnectionResult
import com.google.android.gms.common.api.GoogleApi
import com.google.android.gms.common.api.GoogleApiClient
import com.google.android.gms.common.api.ResultCallback
import com.google.android.gms.common.api.Status
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlin.coroutines.resume

class InAppUpdateManager(private val activity: Activity) {
    
    private val appUpdateManager: AppUpdateManager by lazy {
        AppUpdateManagerFactory.create(activity)
    }
    
    private var updateFlowRequestCompleted = false
    
    // Check if an update is available
    suspend fun checkForUpdate(): UpdateCheckResult {
        return suspendCancellableCoroutine { continuation ->
            val appUpdateInfoTask = appUpdateManager.appUpdateInfo
            
            appUpdateInfoTask.addOnSuccessListener { appUpdateInfo ->
                val result = if (appUpdateInfo.updateAvailability() == 
                    AppUpdateInfo.UPDATE_AVAILABLE) {
                    
                    // Check if the update is appropriate for the current flow
                    val flexibleUpdate = appUpdateInfo.isUpdateTypeAllowed(AppUpdateType.FLEXIBLE)
                    val immediateUpdate = appUpdateInfo.isUpdateTypeAllowed(AppUpdateType.IMMEDIATE)
                    
                    UpdateCheckResult(
                        updateAvailable = true,
                        immediateAvailable = immediateUpdate,
                        flexibleAvailable = flexibleUpdate,
                        versionCode = appUpdateInfo.availableVersionCode(),
                        totalBytesToDownload = appUpdateInfo.totalBytesToDownload(),
                        priority = appUpdateInfo.priority()
                    )
                } else {
                    UpdateCheckResult(updateAvailable = false)
                }
                
                continuation.resume(result)
            }
            
            appUpdateInfoTask.addOnFailureListener { exception ->
                continuation.resume(
                    UpdateCheckResult(
                        updateAvailable = false,
                        error = exception.message
                    )
                )
            }
        }
    }
    
    // Start flexible update flow
    suspend fun startFlexibleUpdate(): UpdateFlowResult {
        return startUpdate(AppUpdateType.FLEXIBLE)
    }
    
    // Start immediate update flow
    suspend fun startImmediateUpdate(): UpdateFlowResult {
        return startUpdate(AppUpdateType.IMMEDIATE)
    }
    
    private suspend fun startUpdate(updateType: Int): UpdateFlowResult {
        return suspendCancellableCoroutine { continuation ->
            val appUpdateInfoTask = appUpdateManager.appUpdateInfo
            
            appUpdateInfoTask.addOnSuccessListener { appUpdateInfo ->
                if (!appUpdateInfo.isUpdateTypeAllowed(updateType)) {
                    continuation.resume(UpdateFlowResult(
                        success = false,
                        error = "Update type not allowed"
                    ))
                    return@addOnSuccessListener
                }
                
                val params = AppUpdateFlowParameters.Builder()
                    .setAllowAssetPackDeletion(false)
                    .build()
                
                appUpdateManager.startUpdateFlowForResult(
                    appUpdateInfo,
                    updateType,
                    activity,
                    REQUEST_CODE_UPDATE
                )
                
                // Note: Result is delivered via onActivityResult
                // For coroutine, we use a listener pattern
                continuation.resume(UpdateFlowResult(success = true))
            }
            
            appUpdateInfoTask.addOnFailureListener { exception ->
                continuation.resume(UpdateFlowResult(
                    success = false,
                    error = exception.message
                ))
            }
        }
    }
    
    // Complete flexible update after download
    fun completeFlexibleUpdate() {
        appUpdateManager.completeUpdate()
    }
    
    // Register listener for update state changes
    fun registerUpdateStateListener(listener: UpdateStateListener) {
        appUpdateManager.registerListener(listener)
    }
    
    // Unregister listener
    fun unregisterUpdateStateListener(listener: UpdateStateListener) {
        appUpdateManager.unregisterListener(listener)
    }
    
    // Check if user was redirected from update flow
    fun handleOnActivityResult(requestCode: Int, resultCode: Int): UpdateFlowResult {
        if (requestCode != REQUEST_CODE_UPDATE) {
            return UpdateFlowResult(success = false, error = "Invalid request code")
        }
        
        return when (resultCode) {
            Activity.RESULT_OK -> UpdateFlowResult(success = true)
            Activity.RESULT_CANCELED -> UpdateFlowResult(success = false, error = "User canceled")
            else -> UpdateFlowResult(success = false, error = "Result code: $resultCode")
        }
    }
    
    companion object {
        private const val REQUEST_CODE_UPDATE = 1001
    }
}

data class UpdateCheckResult(
    val updateAvailable: Boolean,
    val immediateAvailable: Boolean = false,
    val flexibleAvailable: Boolean = false,
    val versionCode: Long = 0,
    val totalBytesToDownload: Long = 0,
    val priority: Int = 0,
    val error: String? = null
)

data class UpdateFlowResult(
    val success: Boolean,
    val error: String? = null
)

// Update state listener for flexible updates
class UpdateStateListener : com.google.android.gms.appupdate.AppUpdateListener {
    
    override fun onStateUpdate(state: com.google.android.gms.appupdate.AppUpdateState) {
        when (state.appUpdateStatus()) {
            com.google.android.gms.appupdate.AppUpdateStatus.DOWNLOADED -> {
                // Flexible update downloaded - need to restart
                onUpdateDownloaded()
            }
            com.google.android.gms.appupdate.AppUpdateStatus.DOWNLOADING -> {
                val progress = state.bytesDownloaded()
                val total = state.totalBytesToDownload()
                onUpdateProgress(progress, total)
            }
            com.google.android.gms.appupdate.AppUpdateStatus.PENDING -> {
                onUpdatePending()
            }
            com.google.android.gms.appupdate.AppUpdateStatus.UNKNOWN -> {
                onUpdateUnknown()
            }
        }
    }
    
    private fun onUpdateDownloaded() {
        // Show UI prompting user to restart
    }
    
    private fun onUpdateProgress(downloaded: Long, total: Long) {
        val percentage = if (total > 0) (downloaded * 100 / total).toInt() else 0
    }
    
    private fun onUpdatePending() {
        // Update is pending - will continue when app is in foreground
    }
    
    private fun onUpdateUnknown() {
        // Unknown state
    }
}
```

## Section 2: Implementing Update UI and UX

The update experience significantly impacts user perception and update compliance rates. A well-designed update flow respects the user's time while communicating the importance of updates.

```kotlin
// Update UI components and user flow management
package com.example.myapp.update.ui

import android.app.Activity
import android.content.Context
import android.view.LayoutInflater
import androidx.appcompat.app.AlertDialog
import com.example.myapp.R

class UpdateDialogManager(private val context: Context) {
    
    // Show dialog for flexible update
    fun showFlexibleUpdateDialog(
        versionName: String,
        releaseNotes: List<String>,
        onAccept: () -> Unit,
        onLater: () -> Unit
    ) {
        val message = buildString {
            append("A new version ($versionName) is available.\n\n")
            append("What's new:\n")
            releaseNotes.forEach { note ->
                append("• $note\n")
            }
            append("\n")
            append("You can continue using the app while the update downloads in the background.")
        }
        
        AlertDialog.Builder(context)
            .setTitle("Update Available")
            .setMessage(message)
            .setPositiveButton("Update Now") { dialog, _ ->
                onAccept()
                dialog.dismiss()
            }
            .setNegativeButton("Later") { dialog, _ ->
                onLater()
                dialog.dismiss()
            }
            .setCancelable(true)
            .show()
    }
    
    // Show dialog for immediate (mandatory) update
    fun showImmediateUpdateDialog(
        versionName: String,
        releaseNotes: List<String>,
        onAccept: () -> Unit
    ) {
        val message = buildString {
            append("A new version ($versionName) is required.\n\n")
            append("This update includes important improvements:\n")
            releaseNotes.forEach { note ->
                append("• $note\n")
            }
            append("\n")
            append("Please update now to continue using the app.")
        }
        
        AlertDialog.Builder(context)
            .setTitle("Update Required")
            .setMessage(message)
            .setPositiveButton("Update") { dialog, _ ->
                onAccept()
                dialog.dismiss()
            }
            .setCancelable(false)
            .show()
    }
    
    // Show dialog when flexible update is downloaded
    fun showUpdateReadyDialog(onRestart: () -> Unit) {
        AlertDialog.Builder(context)
            .setTitle("Update Ready")
            .setMessage("The update has been downloaded. Restart now to apply it.")
            .setPositiveButton("Restart Now") { dialog, _ ->
                onRestart()
                dialog.dismiss()
            }
            .setCancelable(false)
            .show()
    }
    
    // Show download progress
    fun showDownloadProgress(progress: Int, downloadedBytes: Long, totalBytes: Long) {
        // Implementation would show a progress dialog or update existing UI
    }
    
    // Show update failure dialog
    fun showUpdateFailedDialog(onRetry: () -> Unit, onCancel: () -> Unit) {
        AlertDialog.Builder(context)
            .setTitle("Update Failed")
            .setMessage("Unable to download the update. Please try again.")
            .setPositiveButton("Retry") { dialog, _ ->
                onRetry()
                dialog.dismiss()
            }
            .setNegativeButton("Cancel") { dialog, _ ->
                onCancel()
                dialog.dismiss()
            }
            .setCancelable(true)
            .show()
    }
}

// Update state holder for managing update UI
class UpdateStateHolder(private val updateManager: InAppUpdateManager) {
    
    enum class State {
        CHECKING,
        UPDATE_AVAILABLE_FLEXIBLE,
        UPDATE_AVAILABLE_IMMEDIATE,
        DOWNLOADING,
        DOWNLOADED,
        UPDATE_FAILED,
        UP_TO_DATE
    }
    
    var currentState: State = State.CHECKING
        private set
    
    var downloadProgress: Int = 0
        private set
    
    var errorMessage: String? = null
        private set
    
    suspend fun checkForUpdates(): UpdateCheckResult {
        currentState = State.CHECKING
        val result = updateManager.checkForUpdate()
        
        currentState = if (result.updateAvailable) {
            when {
                result.immediateAvailable -> State.UPDATE_AVAILABLE_IMMEDIATE
                result.flexibleAvailable -> State.UPDATE_AVAILABLE_FLEXIBLE
                else -> State.UPDATE_FAILED
            }
        } else {
            State.UP_TO_DATE
        }
        
        return result
    }
    
    fun updateDownloadProgress(progress: Int) {
        downloadProgress = progress
        currentState = State.DOWNLOADING
    }
    
    fun onUpdateDownloaded() {
        currentState = State.DOWNLOADED
    }
    
    fun onUpdateFailed(error: String?) {
        errorMessage = error
        currentState = State.UPDATE_FAILED
    }
    
    fun reset() {
        currentState = State.CHECKING
        downloadProgress = 0
        errorMessage = null
    }
}
```

## Section 3: Update Strategies Based on Release Type

Different types of releases require different update strategies. Understanding when to use flexible vs immediate updates helps balance user experience with update adoption rates.

```kotlin
// Update strategy based on release type and priority
package com.example.myapp.update.strategy

import com.example.myapp.update.UpdateCheckResult
import com.example.myapp.update.InAppUpdateManager

class UpdateStrategyManager(
    private val updateManager: InAppUpdateManager
) {
    
    // Determine update strategy based on release metadata
    suspend fun determineUpdateStrategy(
        updateCheck: UpdateCheckResult,
        releaseType: ReleaseType,
        userSegment: UserSegment
    ): UpdateStrategy {
        
        // Critical releases always use immediate update
        if (releaseType == ReleaseType.CRITICAL) {
            return UpdateStrategy.IMMEDIATE
        }
        
        // Security releases always use immediate for all users
        if (releaseType == ReleaseType.SECURITY) {
            return UpdateStrategy.IMMEDIATE
        }
        
        // Determine based on release type and user segment
        return when (releaseType) {
            ReleaseType.MAJOR -> {
                // Major releases - immediate for power users, flexible for others
                if (userSegment == UserSegment.POWER_USER) {
                    UpdateStrategy.IMMEDIATE
                } else {
                    UpdateStrategy.FLEXIBLE
                }
            }
            
            ReleaseType.MINOR -> {
                // Minor releases - flexible for everyone
                UpdateStrategy.FLEXIBLE
            }
            
            ReleaseType.PATCH -> {
                // Patch releases - flexible, can be skipped
                if (userSegment == UserSegment.NEW_USER) {
                    UpdateStrategy.RECOMMENDED_FLEXIBLE
                } else {
                    UpdateStrategy.SKIPPABLE
                }
            }
            
            ReleaseType.EXPERIMENTAL -> {
                // Experimental - no forced updates
                UpdateStrategy.NONE
            }
            
            else -> UpdateStrategy.FLEXIBLE
        }
    }
    
    // Execute the determined strategy
    suspend fun executeStrategy(
        strategy: UpdateStrategy,
        releaseNotes: List<String>
    ): StrategyResult {
        return when (strategy) {
            UpdateStrategy.IMMEDIATE -> {
                try {
                    updateManager.startImmediateUpdate()
                    StrategyResult(StrategyStatus.STARTED, "Immediate update started")
                } catch (e: Exception) {
                    StrategyResult(StrategyStatus.FAILED, e.message ?: "Update failed")
                }
            }
            
            UpdateStrategy.FLEXIBLE,
            UpdateStrategy.RECOMMENDED_FLEXIBLE -> {
                try {
                    updateManager.startFlexibleUpdate()
                    StrategyResult(StrategyStatus.STARTED, "Flexible update started")
                } catch (e: Exception) {
                    StrategyResult(StrategyStatus.FAILED, e.message ?: "Update failed")
                }
            }
            
            UpdateStrategy.SKIPPABLE -> {
                // Don't force, just notify
                StrategyResult(StrategyStatus.SKIPPED, "Update is skippable")
            }
            
            UpdateStrategy.NONE -> {
                StrategyResult(StrategyStatus.SKIPPED, "No update required")
            }
        }
    }
}

enum class ReleaseType {
    CRITICAL,     // Security patches, critical bug fixes
    SECURITY,     // Security-related updates
    MAJOR,        // New features, breaking changes
    MINOR,        // New features, backward compatible
    PATCH,        // Bug fixes
    EXPERIMENTAL  // Beta features
}

enum class UserSegment {
    NEW_USER,      // New to the app (< 7 days)
    ACTIVE_USER,   // Regular user (7-30 days)
    POWER_USER,    // Heavy user (> 30 days, daily usage)
    AT_RISK_USER   // Users showing signs of churning
}

enum class UpdateStrategy {
    IMMEDIATE,              // Force update, block usage
    FLEXIBLE,               // Download in background, prompt to restart
    RECOMMENDED_FLEXIBLE,   // Recommend but allow skipping
    SKIPPABLE,              // Allow skipping
    NONE                    // No update UI
}

data class StrategyResult(
    val status: StrategyStatus,
    val message: String
)

enum class StrategyStatus {
    STARTED,
    FAILED,
    SKIPPED
}
```

## Section 4: Handling Update Failures and Edge Cases

Update flows can fail in various ways - network issues, user cancellation, API errors, or download failures. Handling these gracefully maintains user trust and ensures eventual update adoption.

```kotlin
// Update failure handling and recovery
package com.example.myapp.update.recovery

import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import android.os.storage.StorageManager
import kotlinx.coroutines.delay

class UpdateFailureHandler(private val context: Context) {
    
    enum class FailureType {
        NETWORK_ERROR,
        STORAGE_ERROR,
        API_ERROR,
        USER_CANCELLED,
        TIMEOUT,
        UNKNOWN
    }
    
    // Determine failure type and suggest recovery
    fun analyzeFailure(errorCode: Int, errorMessage: String?): FailureAnalysis {
        val failureType = when {
            errorMessage?.contains("network", ignoreCase = true) == true ||
            errorMessage?.contains("connection", ignoreCase = true) == true -> 
                FailureType.NETWORK_ERROR
            
            errorMessage?.contains("storage", ignoreCase = true) == true ||
            errorMessage?.contains("space", ignoreCase = true) == true ->
                FailureType.STORAGE_ERROR
            
            errorMessage?.contains("API") == true ||
            errorMessage?.contains("code", ignoreCase = true) == true ->
                FailureType.API_ERROR
            
            errorMessage?.contains("cancel", ignoreCase = true) == true ->
                FailureType.USER_CANCELLED
            
            errorMessage?.contains("timeout", ignoreCase = true) == true ->
                FailureType.TIMEOUT
            
            else -> FailureType.UNKNOWN
        }
        
        return FailureAnalysis(
            failureType = failureType,
            canRetry = failureType !in listOf(FailureType.USER_CANCELLED, FailureType.UNKNOWN),
            recoveryAction = suggestRecovery(failureType),
            delayBeforeRetry = getRetryDelay(failureType)
        )
    }
    
    // Check preconditions before starting update
    fun validateUpdatePreconditions(): PreconditionResult {
        val issues = mutableListOf<String>()
        
        // Check network connectivity
        if (!isNetworkAvailable()) {
            issues.add("No network connection available")
        }
        
        // Check storage space
        if (!hasEnoughStorage()) {
            issues.add("Not enough storage space")
        }
        
        // Check if app is in foreground
        // (In-app updates require the app to be in foreground)
        
        return PreconditionResult(
            canProceed = issues.isEmpty(),
            issues = issues
        )
    }
    
    // Implement retry logic with exponential backoff
    suspend fun retryWithBackoff(
        maxAttempts: Int = 3,
        initialDelayMs: Long = 1000,
        action: suspend () -> Unit
    ): RetryResult {
        var lastError: Exception? = null
        
        for (attempt in 1..maxAttempts) {
            try {
                action()
                return RetryResult(success = true, attempts = attempt)
            } catch (e: Exception) {
                lastError = e
                
                if (attempt < maxAttempts) {
                    val delayMs = initialDelayMs * (attempt - 1) * 2
                    delay(delayMs)
                }
            }
        }
        
        return RetryResult(
            success = false,
            attempts = maxAttempts,
            error = lastError?.message
        )
    }
    
    private fun suggestRecovery(failureType: FailureType): RecoveryAction {
        return when (failureType) {
            FailureType.NETWORK_ERROR -> RecoveryAction.SHOW_NETWORK_DIALOG
            FailureType.STORAGE_ERROR -> RecoveryAction.SHOW_STORAGE_DIALOG
            FailureType.API_ERROR -> RecoveryAction.RETRY_LATER
            FailureType.USER_CANCELLED -> RecoveryAction.DISMISS
            FailureType.TIMEOUT -> RecoveryAction.RETRY_WITH_NOTIFICATION
            FailureType.UNKNOWN -> RecoveryAction.SHOW_ERROR_DIALOG
        }
    }
    
    private fun getRetryDelay(failureType: FailureType): Long {
        return when (failureType) {
            FailureType.NETWORK_ERROR -> 5000L
            FailureType.STORAGE_ERROR -> 0L  // No retry needed
            FailureType.API_ERROR -> 60000L  // Wait 1 minute
            FailureType.TIMEOUT -> 30000L    // Wait 30 seconds
            else -> 5000L
        }
    }
    
    private fun isNetworkAvailable(): Boolean {
        val connectivityManager = context.getSystemService(Context.CONNECTIVITY_SERVICE) 
            as ConnectivityManager
        
        val network = connectivityManager.activeNetwork ?: return false
        val capabilities = connectivityManager.getNetworkCapabilities(network) ?: return false
        
        return capabilities.hasCapability(NetworkCapabilities.NET_CAPABILITY_INTERNET)
    }
    
    private fun hasEnoughStorage(): Boolean {
        val storageManager = context.getSystemService(Context.STORAGE_SERVICE) as StorageManager
        val bytesAvailable = storageManager.getAllocatableBytes(
            android.os.Environment.getDataDirectory().statFs
        )
        
        val minRequired = 100 * 1024 * 1024L  // 100 MB minimum
        return bytesAvailable > minRequired
    }
}

data class FailureAnalysis(
    val failureType: FailureType,
    val canRetry: Boolean,
    val recoveryAction: RecoveryAction,
    val delayBeforeRetry: Long
)

enum class RecoveryAction {
    SHOW_NETWORK_DIALOG,
    SHOW_STORAGE_DIALOG,
    RETRY_WITH_NOTIFICATION,
    SHOW_ERROR_DIALOG,
    RETRY_LATER,
    DISMISS
}

data class PreconditionResult(
    val canProceed: Boolean,
    val issues: List<String>
)

data class RetryResult(
    val success: Boolean,
    val attempts: Int,
    val error: String? = null
)
```

## Section 5: Measuring Update Success

Tracking update metrics helps optimize your update strategy and understand user behavior around updates.

```kotlin
// Update metrics tracking
package com.example.myapp.update.metrics

import android.content.Context
import com.google.firebase.analytics.FirebaseAnalytics

class UpdateMetrics(private val context: Context) {
    
    private val analytics = FirebaseAnalytics.getContext(context)
    
    // Track update flow events
    fun logUpdateCheckRequested(versionCode: Long) {
        logEvent("update_check_requested", "version_code" to versionCode)
    }
    
    fun logUpdateShown(updateType: String, versionCode: Long) {
        logEvent("update_shown", 
            "update_type" to updateType,
            "version_code" to versionCode
        )
    }
    
    fun logUpdateAccepted(updateType: String, versionCode: Long) {
        logEvent("update_accepted",
            "update_type" to updateType,
            "version_code" to versionCode
        )
    }
    
    fun logUpdateRejected(updateType: String, versionCode: Long) {
        logEvent("update_rejected",
            "update_type" to updateType,
            "version_code" to versionCode
        )
    }
    
    fun logUpdateDownloaded(updateType: String, versionCode: Long, durationMs: Long) {
        logEvent("update_downloaded",
            "update_type" to updateType,
            "version_code" to versionCode,
            "download_duration_ms" to durationMs
        )
    }
    
    fun logUpdateInstalled(versionCode: Long) {
        logEvent("update_installed", "version_code" to versionCode)
    }
    
    fun logUpdateFailed(updateType: String, errorCode: Int, errorMessage: String) {
        logEvent("update_failed",
            "update_type" to updateType,
            "error_code" to errorCode,
            "error_message" to errorMessage
        )
    }
    
    fun logUpdateSkipped(versionCode: Long) {
        logEvent("update_skipped", "version_code" to versionCode)
    }
    
    fun logUserPostponed(updateType: String, timesPostponed: Int) {
        logEvent("update_postponed",
            "update_type" to updateType,
            "postpone_count" to timesPostponed
        )
    }
    
    private fun logEvent(eventName: String, vararg params: Pair<String, Any>) {
        val bundle = android.os.Bundle()
        params.forEach { (key, value) ->
            when (value) {
                is String -> bundle.putString(key, value)
                is Long -> bundle.putLong(key, value)
                is Int -> bundle.putInt(key, value)
                is Double -> bundle.putDouble(key, value)
            }
        }
        analytics.logEvent(eventName, bundle)
    }
    
    // Get update adoption rate (requires backend aggregation)
    fun calculateAdoptionRate(
        targetVersion: Long,
        totalUsers: Int,
        updatedUsers: Int
    ): Double {
        return if (totalUsers > 0) {
            (updatedUsers.toDouble() / totalUsers.toDouble()) * 100
        } else {
            0.0
        }
    }
}

// Update analytics summary
class UpdateAnalytics {
    
    data class UpdateMetricsSummary(
        val checkRequests: Int = 0,
        val updateShown: Int = 0,
        val updateAccepted: Int = 0,
        val updateRejected: Int = 0,
        val updateDownloaded: Int = 0,
        val updateInstalled: Int = 0,
        val updateFailed: Int = 0,
        val updateSkipped: Int = 0,
        val acceptanceRate: Double = 0.0,
        val installRate: Double = 0.0
    )
    
    fun calculateSummaryFromEvents(events: List<UpdateEvent>): UpdateMetricsSummary {
        val checks = events.count { it.type == EventType.CHECK_REQUESTED }
        val shown = events.count { it.type == EventType.SHOWN }
        val accepted = events.count { it.type == EventType.ACCEPTED }
        val rejected = events.count { it.type == EventType.REJECTED }
        val downloaded = events.count { it.type == EventType.DOWNLOADED }
        val installed = events.count { it.type == EventType.INSTALLED }
        val failed = events.count { it.type == EventType.FAILED }
        val skipped = events.count { it.type == EventType.SKIPPED }
        
        val acceptanceRate = if (shown > 0) (accepted.toDouble() / shown) * 100 else 0.0
        val installRate = if (accepted > 0) (installed.toDouble() / accepted) * 100 else 0.0
        
        return UpdateMetricsSummary(
            checkRequests = checks,
            updateShown = shown,
            updateAccepted = accepted,
            updateRejected = rejected,
            updateDownloaded = downloaded,
            updateInstalled = installed,
            updateFailed = failed,
            updateSkipped = skipped,
            acceptanceRate = acceptanceRate,
            installRate = installRate
        )
    }
}

enum class EventType {
    CHECK_REQUESTED,
    SHOWN,
    ACCEPTED,
    REJECTED,
    DOWNLOADED,
    INSTALLED,
    FAILED,
    SKIPPED,
    POSTPONED
}

data class UpdateEvent(
    val type: EventType,
    val timestamp: Long,
    val versionCode: Long,
    val updateType: String,
    val metadata: Map<String, Any> = emptyMap()
)
```

## Best Practices

- Use immediate updates only for critical releases like security patches
- Test update flows on multiple devices and Android versions
- Show clear release notes so users understand the update benefits
- Implement retry logic for transient failures
- Track update metrics to measure success and identify issues
- Handle the case where users postpone updates multiple times
- Use feature flags alongside updates for gradual rollouts
- Implement update testing in pre-production environments
- Store update preferences for returning users
- Consider time-of-day for update prompts based on user activity patterns

## Common Pitfalls

- **Update dialog appears too frequently**
  - Solution: Cache update check results, don't check on every app launch
  
- **User can't find update after dismissing**
  - Solution: Provide a way to manually check for updates in settings
  
- **Update fails silently**
  - Solution: Implement proper error handling and user notification
  
- **Update doesn't complete when app goes to background**
  - Solution: In-app updates require app to be in foreground
  
- **Stale update info returned from cache**
  - Solution: Force fresh check with AppUpdateManager with specific parameters

## Troubleshooting Guide

**Q: In-app update dialog doesn't appear**
A: Check that the Play Store has an update available, app is in foreground, and update isn't already in progress.

**Q: Update always uses immediate type even when flexible is available**
A: Verify both update types are allowed in AppUpdateInfo before starting flow.

**Q: Download never completes**
A: Check network connectivity, ensure app is in foreground, verify storage space.

**Q: User wants to update but button is disabled**
A: This is likely an immediate update that's still being processed; check for errors.

## Advanced Tips

- Use App Update Testing Library to test update flows without Play Store
- Combine in-app updates with Firebase Remote Config for feature control
- Implement custom update channels for different user segments
- Use the Update API for programmatic update management

## Cross-References

- [Release Management](../01_App_Distribution/04_Release_Management.md) - Release types and strategies
- [Crash Reporting](./02_Crash_Reporting.md) - Crash data for update decisions
- [Analytics Integration](./01_Analytics_Integration.md) - Track update metrics
- [Google Play Store](../01_App_Distribution/01_Google_Play_Store.md) - Play Store update configuration