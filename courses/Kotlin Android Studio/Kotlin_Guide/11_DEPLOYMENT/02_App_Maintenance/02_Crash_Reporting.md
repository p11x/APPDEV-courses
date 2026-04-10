# Crash Reporting

## Learning Objectives

1. Understanding crash reporting fundamentals
2. Implementing Firebase Crashlytics
3. Analyzing crash reports and diagnostics
4. Creating custom crash handling
5. Setting up crash alerts and notifications
6. Implementing non-fatal exception tracking

## Prerequisites

- [Analytics Integration](./01_Analytics_Integration.md)
- [Firebase App Distribution](../01_App_Distribution/02_Firebase_App_Distribution.md)

## Section 1: Crash Reporting Fundamentals

Crash reporting is essential for maintaining app quality and understanding issues that affect your users. When your app crashes, the crash report provides detailed information about the state of the app, the device, and the code path that led to the failure. This information is crucial for reproducing and fixing issues.

Android crash reporting works at multiple levels. Native crashes occur in C/C++ code and generate tombstone files. Java/Kotlin crashes occur in the Dalvik or ART runtime and create stack traces. ANRs (Application Not Responding) happen when the main thread is blocked for too long. Understanding these different crash types helps you diagnose issues effectively.

Effective crash reporting involves more than just capturing crashes - it requires proper configuration to capture meaningful data, intelligent grouping to identify related issues, and workflows to ensure crashes are fixed. Modern crash reporting solutions like Firebase Crashlytics provide these capabilities out of the box while allowing customization for specific needs.

```kotlin
// Crash reporting configuration and setup
package com.example.myapp.crashreporting

import android.content.Context
import com.google.firebase.crashlytics.FirebaseCrashlytics
import com.google.firebase.crashlytics.customlogstrategy.CrashlyticsCustomLogStrategy

class CrashReportingManager(private val context: Context) {
    
    private val crashlytics = FirebaseCrashlytics.getInstance()
    
    fun initialize() {
        // Enable crashlytics
        crashlytics.setCrashlyticsCollectionEnabled(true)
        
        // Set custom keys for crash context
        crashlytics.setCustomKey("app_version", getAppVersion())
        crashlytics.setCustomKey("build_type", getBuildType())
        crashlytics.setCustomKey("min_sdk", getMinSdk())
        crashlytics.setCustomKey("target_sdk", getTargetSdk())
    }
    
    // Set user identification for crash attribution
    fun setUserIdentifier(userId: String) {
        crashlytics.setUserId(userId)
    }
    
    // Add custom keys for crash context
    fun setCustomKeys(vararg keyValues: Pair<String, Any>) {
        keyValues.forEach { (key, value) ->
            when (value) {
                is String -> crashlytics.setCustomKey(key, value)
                is Boolean -> crashlytics.setCustomKey(key, value)
                is Double -> crashlytics.setCustomKey(key, value)
                is Float -> crashlytics.setCustomKey(key, value)
                is Int -> crashlytics.setCustomKey(key, value)
                is Long -> crashlytics.setCustomKey(key, value)
            }
        }
    }
    
    // Log messages for crash context
    fun logMessage(tag: String, message: String) {
        crashlytics.log("[$tag] $message")
    }
    
    // Record a non-fatal exception
    fun recordException(throwable: Throwable, severity: Severity = Severity.ERROR) {
        crashlytics.recordException(throwable)
    }
    
    // Record custom exception with additional context
    fun recordCustomException(
        tag: String,
        message: String,
        stackTrace: Array<StackTraceElement>
    ) {
        val customException = CustomException(tag, message, stackTrace)
        crashlytics.recordException(customException)
    }
    
    // Force a test crash for validation
    fun forceTestCrash() {
        throw TestCrashException("Test crash for Crashlytics validation")
    }
    
    private fun getAppVersion(): String {
        return try {
            val packageInfo = context.packageManager
                .getPackageInfo(context.packageName, 0)
            packageInfo.versionName ?: "unknown"
        } catch (e: Exception) {
            "unknown"
        }
    }
    
    private fun getBuildType(): String {
        return "release"  // Or determine from BuildConfig
    }
    
    private fun getMinSdk(): Int {
        return 24  // Or determine from BuildConfig
    }
    
    private fun getTargetSdk(): Int {
        return 34  // Or determine from BuildConfig
    }
}

enum class Severity {
    DEBUG,
    INFO,
    WARNING,
    ERROR
}

class CustomException(
    private val tag: String,
    private val message: String,
    private val customStackTrace: Array<StackTraceElement>
) : Exception("$tag: $message") {
    
    override fun getStackTrace(): Array<StackTraceElement> {
        return customStackTrace
    }
}

class TestCrashException(message: String) : RuntimeException(message)
```

## Section 2: Firebase Crashlytics Implementation

Firebase Crashlytics provides comprehensive crash reporting with automatic grouping, breadcrumbs, and integration with other Firebase services. Setting it up correctly ensures you capture all relevant crash data.

```kotlin
// Build configuration for Crashlytics
// build.gradle.kts (project level)
plugins {
    id("com.android.application") version "8.2.0" apply false
    id("org.jetbrains.kotlin.android") version "1.9.0" apply false
    id("com.google.gms.google-services") version "4.4.0" apply false
    id("com.google.firebase.crashlytics") version "2.9.9" apply false
}

// build.gradle.kts (app module)
plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("com.google.gms.google-services")
    id("com.google.firebase.crashlytics")
}

android {
    buildTypes {
        release {
            // Enable Crashlytics for release builds
            isMinifyEnabled = true
            
            // Enable Crashlytics mapping upload
            // This happens automatically with the plugin
        }
        
        debug {
            // Disable Crashlytics in debug to avoid noise
            isDebuggable = true
        }
    }
}

dependencies {
    implementation(platform("com.google.firebase:firebase-bom:32.7.0"))
    implementation("com.google.firebase:firebase-crashlytics-ktx")
    implementation("com.google.firebase:firebase-analytics-ktx")
}

// Crashlytics configuration file (crashlytics.properties)
// google-services.json contains the Crashlytics configuration
```

```kotlin
// Application initialization with Crashlytics
package com.example.myapp.crashreporting

import android.app.Application
import android.content.Context
import com.google.firebase.crashlytics.FirebaseCrashlytics

class MyApplication : Application() {
    
    override fun onCreate() {
        super.onCreate()
        
        // Initialize Crashlytics with custom configuration
        initializeCrashlytics()
        
        // Set up global exception handler
        setupExceptionHandlers()
    }
    
    private fun initializeCrashlytics() {
        val crashlytics = FirebaseCrashlytics.getInstance()
        
        // Enable Crashlytics collection based on build type
        val isDebugBuild = isDebugBuild()
        crashlytics.setCrashlyticsCollectionEnabled(!isDebugBuild)
        
        if (isDebugBuild) {
            crashlytics.log("Crashlytics disabled for debug build")
        }
        
        // Set build info
        crashlytics.setCustomKey("app_version", getAppVersion())
        crashlytics.setCustomKey("build_type", if (isDebugBuild) "debug" else "release")
        
        // Enable performance monitoring for release
        // This requires the performance SDK
    }
    
    private fun setupExceptionHandlers() {
        val defaultHandler = Thread.getDefaultUncaughtExceptionHandler()
        
        // Set custom uncaught exception handler
        Thread.setDefaultUncaughtExceptionHandler { thread, throwable ->
            // Log crash details before the default handler
            handleUncaughtException(thread, throwable)
            
            // Call the default handler to allow the app to crash normally
            defaultHandler?.uncaughtException(thread, throwable)
        }
        
        // Also handle exceptions in the main thread
        registerActivityLifecycleCallbacks(object : android.app.Application.ActivityLifecycleCallbacks {
            override fun onActivityCreated(activity: android.app.Activity, savedInstanceState: android.os.Bundle?) {}
            override fun onActivityStarted(activity: android.app.Activity) {}
            override fun onActivityResumed(activity: android.app.Activity) {}
            override fun onActivityPaused(activity: android.app.Activity) {}
            override fun onActivityStopped(activity: android.app.Activity) {}
            override fun onActivitySaveInstanceState(activity: android.app.Activity, outState: android.os.Bundle) {}
            override fun onActivityDestroyed(activity: android.app.Activity) {}
            
            // Handle exceptions during activity lifecycle
            override fun onActivityCreated(
                activity: android.app.Activity,
                savedInstanceState: android.os.Bundle?
            ) {
                try {
                    // Wrap activity creation
                } catch (e: Exception) {
                    handleException(e, activity.javaClass.simpleName)
                    throw e
                }
            }
        })
    }
    
    private fun handleUncaughtException(thread: Thread, throwable: Throwable) {
        val crashlytics = FirebaseCrashlytics.getInstance()
        
        // Add thread information
        crashlytics.setCustomKey("thread_name", thread.name)
        crashlytics.setCustomKey("thread_id", thread.id.toString())
        
        // Log the exception
        crashlytics.recordException(throwable)
        
        // Additional logging
        crashlytics.log("Uncaught exception on thread: ${thread.name}")
    }
    
    private fun handleException(throwable: Throwable, context: String) {
        val crashlytics = FirebaseCrashlytics.getInstance()
        crashlytics.setCustomKey("exception_context", context)
        crashlytics.recordException(throwable)
    }
    
    private fun isDebugBuild(): Boolean {
        return (applicationContext.packageManager
            .getApplicationInfo(packageName, 0)
            .flags and android.content.pm.ApplicationInfo.FLAG_DEBUGGABLE) != 0
    }
    
    private fun getAppVersion(): String {
        return packageManager.getPackageInfo(packageName, 0).versionName ?: "unknown"
    }
}
```

## Section 3: Custom Exception Handling

Beyond automatic crash reporting, you may need to handle specific exception types differently or capture additional context for certain scenarios. Custom exception handling allows you to log exceptions that don't crash the app but still indicate problems.

```kotlin
// Custom exception handling utilities
package com.example.myapp.exception

import android.content.Context
import android.util.Log
import com.google.firebase.crashlytics.FirebaseCrashlytics

class ExceptionHandler(private val context: Context) {
    
    private val crashlytics = FirebaseCrashlytics.getInstance()
    
    // Handle expected exceptions that don't crash the app
    fun handleExpectedException(
        exception: Exception,
        context: String,
        recoveryAction: RecoveryAction? = null
    ) {
        // Log to console for debugging
        Log.e("ExceptionHandler", "Expected exception in $context: ${exception.message}")
        
        // Record in Crashlytics as non-fatal
        crashlytics.recordException(exception)
        
        // Add context information
        crashlytics.setCustomKey("exception_context", context)
        crashlytics.setCustomKey("exception_type", exception.javaClass.simpleName)
        
        // Attempt recovery if possible
        recoveryAction?.let { action ->
            attemptRecovery(action, exception)
        }
    }
    
    // Handle unexpected exceptions that may indicate bugs
    fun handleUnexpectedException(
        exception: Exception,
        source: String,
        severity: ExceptionSeverity = ExceptionSeverity.WARNING
    ) {
        Log.wtf("ExceptionHandler", "Unexpected exception from $source", exception)
        
        crashlytics.recordException(exception)
        crashlytics.setCustomKey("source", source)
        crashlytics.setCustomKey("severity", severity.name)
        
        // For critical exceptions, trigger alert
        if (severity == ExceptionSeverity.CRITICAL) {
            triggerAlert(exception, source)
        }
    }
    
    // Handle API errors with specific context
    fun handleApiError(
        errorCode: Int,
        errorMessage: String,
        endpoint: String,
        requestBody: Map<String, Any>? = null
    ) {
        val apiException = ApiException(errorCode, errorMessage, endpoint)
        
        crashlytics.recordException(apiException)
        crashlytics.setCustomKey("api_endpoint", endpoint)
        crashlytics.setCustomKey("error_code", errorCode)
        crashlytics.setCustomKey("error_message", errorMessage)
        
        // Log request body if available (be careful with sensitive data)
        requestBody?.let { body ->
            val sanitized = sanitizeForLogging(body)
            crashlytics.setCustomKey("request_body", sanitized.toString())
        }
        
        crashlytics.log("API error at $endpoint: $errorCode - $errorMessage")
    }
    
    // Handle database errors
    fun handleDatabaseError(
        operation: String,
        table: String,
        error: Exception
    ) {
        crashlytics.recordException(error)
        crashlytics.setCustomKey("db_operation", operation)
        crashlytics.setCustomKey("db_table", table)
        
        crashlytics.log("Database error during $operation on $table: ${error.message}")
    }
    
    private fun attemptRecovery(action: RecoveryAction, exception: Exception) {
        when (action) {
            is RecoveryAction.RETRY -> {
                Log.d("ExceptionHandler", "Will retry operation")
            }
            is RecoveryAction.FALLBACK -> {
                Log.d("ExceptionHandler", "Will use fallback: ${action.fallbackValue}")
            }
            is RecoveryAction.NAVIGATE -> {
                Log.d("ExceptionHandler", "Will navigate to: ${action.destination}")
            }
            is RecoveryAction.SHOW_ERROR -> {
                Log.d("ExceptionHandler", "Will show error to user: ${action.message}")
            }
        }
    }
    
    private fun triggerAlert(exception: Exception, source: String) {
        // Implement alert notification (email, Slack, etc.)
        Log.wtf("ExceptionHandler", "ALERT: Critical exception from $source")
    }
    
    private fun sanitizeForLogging(data: Map<String, Any>): Map<String, Any> {
        val sensitiveKeys = listOf("password", "token", "secret", "key", "credit_card", "ssn")
        
        return data.mapValues { (key, value) ->
            if (key.lowercase() in sensitiveKeys) {
                "***REDACTED***"
            } else {
                value
            }
        }
    }
}

class ApiException(
    val errorCode: Int,
    val errorMessage: String,
    val endpoint: String
) : Exception("API Error $errorCode: $errorMessage at $endpoint")

enum class ExceptionSeverity {
    DEBUG,
    INFO,
    WARNING,
    ERROR,
    CRITICAL
}

sealed class RecoveryAction {
    data class Retry(val maxAttempts: Int = 3) : RecoveryAction()
    data class Fallback(val fallbackValue: Any) : RecoveryAction()
    data class Navigate(val destination: String) : RecoveryAction()
    data class ShowError(val message: String) : RecoveryAction()
}
```

## Section 4: ANR Detection and Handling

ANRs (Application Not Responding) occur when the main thread is blocked for too long, causing the system to display a dialog asking the user to close the app. Proper ANR detection and handling helps identify and fix performance issues.

```kotlin
// ANR detection and handling
package com.example.myapp.anr

import android.os.Handler
import android.os.Looper
import android.os.Message
import android.util.Log
import com.google.firebase.crashlytics.FirebaseCrashlytics

class ANRDetector {
    
    private val crashlytics = FirebaseCrashlytics.getInstance()
    private val mainHandler = Handler(Looper.getMainLooper())
    private val anrThresholdMs = 5000L // 5 seconds
    private var lastRunnableTime = System.currentTimeMillis()
    private var isMonitoring = false
    
    fun startMonitoring() {
        if (isMonitoring) return
        isMonitoring = true
        
        // Post a checker runnable that runs on the main thread
        mainHandler.postDelayed(anrChecker, anrThresholdMs)
    }
    
    fun stopMonitoring() {
        isMonitoring = false
        mainHandler.removeCallbacks(anrChecker)
    }
    
    fun onMainThreadActivity() {
        lastRunnableTime = System.currentTimeMillis()
    }
    
    private val anrChecker = object : Runnable {
        override fun run() {
            if (!isMonitoring) return
            
            val now = System.currentTimeMillis()
            val timeSinceLastActivity = now - lastRunnableTime
            
            if (timeSinceLastActivity > anrThresholdMs) {
                // Potential ANR detected
                detectANR(timeSinceLastActivity)
            }
            
            // Schedule next check
            mainHandler.postDelayed(this, anrThresholdMs)
        }
    }
    
    private fun detectANR(duration: Long) {
        val message = "Potential ANR: Main thread blocked for ${duration}ms"
        Log.w("ANRDetector", message)
        
        crashlytics.log(message)
        crashlytics.setCustomKey("anr_duration_ms", duration)
        crashlytics.setCustomKey("anr_timestamp", System.currentTimeMillis())
        
        // Get main thread stack trace
        val stackTrace = Looper.getMainLooper().thread.stackTrace
        crashlytics.log("Main thread stack at ANR:")
        stackTrace.take(20).forEach { element ->
            crashlytics.log("  ${element.toString()}")
        }
    }
}

// ANR prevention utilities
class MainThreadUtil {
    
    companion object {
        fun isMainThread(): Boolean {
            return Looper.myLooper() == Looper.getMainLooper()
        }
        
        fun ensureNotOnMainThread(operation: String) {
            if (isMainThread()) {
                throw IllegalStateException(
                    "Operation '$operation' must not be called on the main thread"
                )
            }
        }
        
        fun ensureOnMainThread(operation: String) {
            if (!isMainThread()) {
                throw IllegalStateException(
                    "Operation '$operation' must be called on the main thread"
                )
            }
        }
        
        // Execute on main thread with timeout
        fun executeOnMainThread(
            timeoutMs: Long = 5000,
            action: () -> Unit
        ) {
            if (isMainThread()) {
                action()
            } else {
                var completed = false
                Handler(Looper.getMainLooper()).post {
                    action()
                    completed = true
                }
                
                // Wait with timeout
                val startTime = System.currentTimeMillis()
                while (!completed && System.currentTimeMillis() - startTime < timeoutMs) {
                    Thread.sleep(50)
                }
                
                if (!completed) {
                    throw RuntimeException("Main thread execution timed out")
                }
            }
        }
    }
}

// Monitor long-running operations
class OperationMonitor {
    
    private val crashlytics = FirebaseCrashlytics.getInstance()
    private val longOperationThresholdMs = 3000L
    
    fun <T> monitorLongOperation(
        operationName: String,
        operation: () -> T
    ): T {
        val startTime = System.currentTimeMillis()
        
        try {
            return operation()
        } finally {
            val duration = System.currentTimeMillis() - startTime
            
            if (duration > longOperationThresholdMs) {
                val message = "Long operation '$operationName' took ${duration}ms"
                Log.w("OperationMonitor", message)
                crashlytics.log(message)
                crashlytics.setCustomKey("long_operation_${operationName}", duration)
            }
        }
    }
}
```

## Section 5: Crash Report Analysis and Management

Once crashes are being reported, you need to analyze them effectively, prioritize fixes, and track your progress. This involves understanding Crashlytics grouping, using custom keys for filtering, and establishing workflows for crash resolution.

```kotlin
// Crash analysis and reporting utilities
package com.example.myapp.crashanalysis

import com.google.firebase.crashlytics.FirebaseCrashlytics

class CrashAnalyzer {
    
    private val crashlytics = FirebaseCrashlytics.getInstance()
    
    // Get crash statistics for the current version
    fun getCrashStats(versionCode: Int): CrashStats {
        // This would typically use the Firebase API
        // For now, return a summary based on recent crashes
        
        return CrashStats(
            totalCrashes = getTotalCrashCount(),
            fatalCrashes = getFatalCrashCount(),
            nonFatalCrashes = getNonFatalCount(),
            anrCount = getAnrCount(),
            mostCommonIssue = getMostCommonIssue(),
            affectedUsers = getAffectedUserCount()
        )
    }
    
    // Group crashes by type for analysis
    fun getTopIssues(limit: Int = 10): List<CrashIssue> {
        // In production, use Crashlytics API to get issues
        // For demonstration, return sample data
        
        return listOf(
            CrashIssue(
                id = "issue_1",
                title = "NullPointerException in UserManager",
                crashCount = 45,
                affectedUsers = 120,
                firstSeen = System.currentTimeMillis() - 86400000 * 7,
                lastSeen = System.currentTimeMillis() - 3600000,
                severity = IssueSeverity.CRITICAL,
                deviceTypes = listOf("Pixel 4", "Samsung Galaxy S20")
            ),
            CrashIssue(
                id = "issue_2",
                title = "IllegalStateException in NetworkClient",
                crashCount = 23,
                affectedUsers = 67,
                firstSeen = System.currentTimeMillis() - 86400000 * 3,
                lastSeen = System.currentTimeMillis() - 7200000,
                severity = IssueSeverity.HIGH,
                deviceTypes = listOf("OnePlus 8", "Xiaomi Mi 10")
            ),
            CrashIssue(
                id = "issue_3",
                title = "ArrayIndexOutOfBoundsException in RecyclerView",
                crashCount = 12,
                affectedUsers = 34,
                firstSeen = System.currentTimeMillis() - 86400000 * 2,
                lastSeen = System.currentTimeMillis() - 14400000,
                severity = IssueSeverity.MEDIUM,
                deviceTypes = listOf("Various")
            )
        )
    }
    
    // Filter crashes by device or version
    fun filterCrashes(
        versionCode: Int? = null,
        deviceModel: String? = null,
        fromDate: Long? = null,
        toDate: Long? = null
    ): FilteredCrashes {
        return FilteredCrashes(
            versionCode = versionCode,
            deviceModel = deviceModel,
            fromDate = fromDate,
            toDate = toDate,
            issues = emptyList(), // Would be populated from API
            totalFiltered = 0
        )
    }
    
    // Create issue for tracking
    fun createIssueTracker(
        issueId: String,
        title: String,
        description: String,
        priority: IssuePriority
    ): IssueTracker {
        return IssueTracker(
            issueId = issueId,
            title = title,
            description = description,
            priority = priority,
            status = IssueStatus.OPEN,
            createdAt = System.currentTimeMillis(),
            assignedTo = null
        )
    }
    
    private fun getTotalCrashCount(): Int = 80
    private fun getFatalCrashCount(): Int = 12
    private fun getNonFatalCount(): Int = 68
    private fun getAnrCount(): Int = 5
    private fun getMostCommonIssue(): String = "NullPointerException in UserManager"
    private fun getAffectedUserCount(): Int = 234
}

data class CrashStats(
    val totalCrashes: Int,
    val fatalCrashes: Int,
    val nonFatalCrashes: Int,
    val anrCount: Int,
    val mostCommonIssue: String,
    val affectedUsers: Int
)

data class CrashIssue(
    val id: String,
    val title: String,
    val crashCount: Int,
    val affectedUsers: Int,
    val firstSeen: Long,
    val lastSeen: Long,
    val severity: IssueSeverity,
    val deviceTypes: List<String>
)

enum class IssueSeverity {
    CRITICAL,
    HIGH,
    MEDIUM,
    LOW
}

data class FilteredCrashes(
    val versionCode: Int?,
    val deviceModel: String?,
    val fromDate: Long?,
    val toDate: Long?,
    val issues: List<CrashIssue>,
    val totalFiltered: Int
)

data class IssueTracker(
    val issueId: String,
    val title: String,
    val description: String,
    val priority: IssuePriority,
    val status: IssueStatus,
    val createdAt: Long,
    val assignedTo: String?
)

enum class IssuePriority {
    P0, P1, P2, P3
}

enum class IssueStatus {
    OPEN,
    IN_PROGRESS,
    RESOLVED,
    CLOSED
}
```

## Best Practices

- Initialize Crashlytics in your Application class before other components
- Add custom keys that help reproduce issues (user action, state, etc.)
- Use breadcrumbs to understand the path leading to crashes
- Set user IDs to track crashes per user
- Test your crash reporting by forcing test crashes in development
- Prioritize crashes by affected users and severity
- Add device and app context to crash reports for better diagnosis
- Create workflows for crash triage and assignment
- Monitor crash-free users as a quality metric
- Keep Crashlytics enabled in release builds only

## Common Pitfalls

- **Crashlytics not collecting data**
  - Solution: Check google-services.json is configured, verify isCrashlyticsCollectionEnabled
  
- **Missing stack traces due to ProGuard**
  - Solution: Ensure ProGuard rules keep Crashlytics mapping files or use R8 with proper config
  
- **Too many duplicate issues**
  - Solution: Add more specific custom keys to help grouping
  
- **Crashes from old app versions**
  - Solution: Set up version filtering in the Crashlytics console
  
- **Sensitive data in crash reports**
  - Solution: Never log passwords, tokens, or PII to Crashlytics

## Troubleshooting Guide

**Q: No crashes appearing in console**
A: Wait 24 hours for processing, check debug mode is working, verify google-services.json is valid.

**Q: Stack traces are obfuscated**
A: Upload mapping files or check ProGuard/R8 configuration is correct.

**Q: Can't see user IDs in crash reports**
A: Set user IDs after Firebase initialization, use setUserId before potential crashes.

**Q: Too many duplicate crash issues**
A: Add custom keys to differentiate similar crashes (e.g., screen name, action).

## Advanced Tips

- Use Firebase Crashlytics with Performance Monitoring together
- Implement custom log strategies for specific debugging needs
- Create automated alerts for critical issues
- Export crash data to BigQuery for advanced analysis
- Use the Crashlytics REST API for custom integrations

## Cross-References

- [Analytics Integration](./01_Analytics_Integration.md) - Combined analytics and crash analytics
- [Google Play Store](../01_App_Distribution/01_Google_Play_Store.md) - Play Console crash data
- [Release Management](../01_App_Distribution/04_Release_Management.md) - Monitor releases for crashes
- [Update Strategies](./03_Update_Strategies.md) - Crash data for update decisions