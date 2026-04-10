# Android Profiler

## Learning Objectives

1. Understanding Android Profiler capabilities
2. Using CPU profiler for performance analysis
3. Using Memory profiler for memory leak detection
4. Using Network profiler for network monitoring
5. Using Energy profiler for battery analysis
6. Interpreting profiler data for optimization

```kotlin
package com.kotlin.debugging.profiler
```

---

## Prerequisites

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 09_PERFORMANCE/01_Performance_Optimization/02_Battery_Optimization.md
- See: 01_SETUP_ENVIRONMENT/01_IDE_Installation_and_Configuration/01_Android_Studio_Setup.md

---

## Core Concepts

### Android Profiler Overview

- **Real-time Monitoring**: CPU, Memory, Network, Energy
- **Method Tracing**: Detailed function-level analysis
- **Heap Dumps**: Memory snapshot analysis
- **Event Timeline**: Synchronize with UI events

### SECTION 1: CPU Profiler

```kotlin
/**
 * CPU Profiler Integration
 * 
 * Using CPU profiler for performance analysis.
 */
class CPUProfiler {
    
    // Enable CPU profiler programmatically
    class ProfilerControl {
        
        fun startCpuProfiling(session: android.os.Debug) {
            // Start method tracing
            android.os.Debug.startMethodTracing("my_trace")
        }
        
        fun stopCpuProfiling() {
            // Stop method tracing
            android.os.Debug.stopMethodTracing()
        }
        
        // Use with Trace API for custom sections
        fun traceCustomSection(label: String, block: () -> Unit) {
            android.os.Trace.beginSection(label)
            try {
                block()
            } finally {
                android.os.Trace.endSection()
            }
        }
    }
    
    // Recording options
    class RecordingOptions {
        
        enum class SamplerType {
            SAMPLED,  // Sample at intervals (lower overhead)
            INSTRUMENTED  // Instrument every method (accurate)
        }
        
        fun createRecordingOptions(): android.tools.profiler.proto.Common.RecordingOptions {
            return android.tools.profiler.proto.Common.RecordingOptions.newBuilder()
                .setMode(android.tools.profiler.proto.Common.RecordingOptions.Mode.SAMPLED)
                .setSamplingIntervalUs(10000)  // 10ms
                .build()
        }
    }
    
    // Analyzing CPU data
    class CPUAnalysis {
        
        fun analyzeTopMethods(traceData: android.tools.profiler.proto.Profiler TraceData): List<MethodInfo> {
            val topMethods = mutableListOf<MethodInfo>()
            
            traceData.callstackList.forEach { callstack ->
                val method = callstack.methodName
                val selfTime = callstack.selfTimeUs
                val totalTime = callstack.totalTimeUs
                
                topMethods.add(MethodInfo(method, selfTime, totalTime))
            }
            
            return topMethods.sortedByDescending { it.totalTime }
                .take(10)  // Top 10 methods
        }
        
        data class MethodInfo(
            val name: String,
            val selfTimeUs: Long,
            val totalTimeUs: Long
        )
    }
    
    // Thread analysis
    class ThreadAnalysis {
        
        fun analyzeThreadState(thread: Thread): ThreadState {
            val state = thread.state
            
            return when (state) {
                Thread.State.RUNNABLE -> ThreadState.RUNNING
                Thread.State.BLOCKED -> ThreadState.BLOCKED
                Thread.State.WAITING -> ThreadState.WAITING
                Thread.State.TIMED_WAITING -> ThreadState.TIMED_WAITING
                Thread.State.NEW -> ThreadState.NEW
                Thread.State.TERMINATED -> ThreadState.TERMINATED
                else -> ThreadState.UNKNOWN
            }
        }
        
        fun getBlockedThreads(threads: List<Thread>): List<ThreadInfo> {
            return threads
                .filter { it.state == Thread.State.BLOCKED }
                .map { ThreadInfo(it.name, it.id, it.state) }
        }
        
        enum class ThreadState {
            RUNNING, BLOCKED, WAITING, TIMED_WAITING, NEW, TERMINATED, UNKNOWN
        }
        
        data class ThreadInfo(val name: String, val id: Long, val state: Thread.State)
    }
}
```

---

## SECTION 2: Memory Profiler

```kotlin
/**
 * Memory Profiler Integration
 * 
 * Using memory profiler for memory analysis.
 */
class MemoryProfiler {
    
    // Trigger heap dump programmatically
    class HeapDumpTrigger {
        
        fun requestHeapDump(
            context: android.content.Context,
            fileName: String = "heap_dump_${System.currentTimeMillis()}"
        ): java.io.File? {
            val file = java.io.File(context.cacheDir, "$fileName.hprof")
            
            try {
                android.os.Debug.dumpHprofData(file.absolutePath)
                return file
            } catch (e: Exception) {
                e.printStackTrace()
                return null
            }
        }
        
        // Use Android Profiler to capture heap dump
        fun captureViaProfiler() {
            // In Android Studio: Profiler > Memory > Dump
        }
    }
    
    // Memory allocation tracking
    class AllocationTracking {
        
        fun startAllocationTracking() {
            // Enable allocation tracking
            android.os.Debug.startAllocCounting()
        }
        
        fun stopAllocationTracking(): AllocationStats {
            android.os.Debug.stopAllocCounting()
            
            return AllocationStats(
                allocCount = android.os.Debug.getAllocCount(),
                allocSize = android.os.Debug.getAllocSize(),
                freeCount = android.os.Debug.getFreeCount(),
                freeSize = android.os.Debug.getFreeSize()
            )
        }
        
        fun getGlobalAllocCount(): Long = android.os.Debug.getGlobalAllocCount()
        fun getGlobalAllocSize(): Long = android.os.Debug.getGlobalAllocSize()
        
        data class AllocationStats(
            val allocCount: Long,
            val allocSize: Long,
            val freeCount: Long,
            val freeSize: Long
        )
    }
    
    // Memory pressure detection
    class MemoryPressureDetector(
        private val context: android.content.Context
    ) {
        
        private val componentCallbacks = object : android.content.ComponentCallbacks2 {
            override fun onTrimMemory(level: Int) {
                val description = when (level) {
                    android.content.ComponentCallbacks2.TRIM_MEMORY_RUNNING_MODERATE -> 
                        "Moderate memory pressure"
                    android.content.ComponentCallbacks2.TRIM_MEMORY_RUNNING_LOW -> 
                        "Low memory"
                    android.content.ComponentCallbacks2.TRIM_MEMORY_RUNNING_CRITICAL -> 
                        "Critical memory"
                    android.content.ComponentCallbacks2.TRIM_MEMORY_UI_HIDDEN -> 
                        "UI hidden - release memory"
                    android.content.ComponentCallbacks2.TRIM_MEMORY_BACKGROUND -> 
                        "Background - release memory"
                    android.content.ComponentCallbacks2.TRIM_MEMORY_MODERATE -> 
                        "Moderate trim"
                    android.content.ComponentCallbacks2.TRIM_MEMORY_COMPLETE -> 
                        "Complete trim"
                    else -> "Unknown"
                }
                
                handleMemoryPressure(level, description)
            }
            
            override fun onConfigurationChanged(newConfig: android.content.res.Configuration) {}
            override fun onLowMemory() {
                handleMemoryPressure(
                    android.content.ComponentCallbacks2.TRIM_MEMORY_COMPLETE,
                    "Low memory warning"
                )
            }
        }
        
        fun register() {
            context.registerComponentCallbacks(componentCallbacks)
        }
        
        fun unregister() {
            context.unregisterComponentCallbacks(componentCallbacks)
        }
        
        private fun handleMemoryPressure(level: Int, description: String) {
            // Release caches, bitmaps, etc.
            clearCaches()
            trimMemory(level)
        }
        
        private fun clearCaches() {}
        private fun trimMemory(level: Int) {}
    }
    
    // Activity memory monitoring
    class ActivityMemoryMonitor {
        
        fun getMemoryInfo(context: android.content.Context): android.app.ActivityManager.MemoryInfo {
            val activityManager = context.getSystemService(
                android.content.Context.ACTIVITY_SERVICE
            ) as android.app.ActivityManager
            
            val memoryInfo = android.app.ActivityManager.MemoryInfo()
            activityManager.getMemoryInfo(memoryInfo)
            
            return memoryInfo
        }
        
        fun isLowMemory(context: android.content.Context): Boolean {
            return getMemoryInfo(context).lowMemory
        }
        
        fun getAvailableMemory(context: android.content.Context): Long {
            return getMemoryInfo(context).availMem
        }
        
        fun getTotalMemory(context: android.content.Context): Long {
            return getMemoryInfo(context).totalMem
        }
        
        fun getMemoryClass(context: android.content.Context): Int {
            val activityManager = context.getSystemService(
                android.content.Context.ACTIVITY_SERVICE
            ) as android.app.ActivityManager
            
            return activityManager.memoryClass
        }
        
        fun getLargeMemoryClass(context: android.content.Context): Int {
            val activityManager = context.getSystemService(
                android.content.Context.ACTIVITY_SERVICE
            ) as android.app.ActivityManager
            
            return activityManager.largeMemoryClass
        }
    }
}
```

---

## SECTION 3: Network Profiler

```kotlin
/**
 * Network Profiler Integration
 * 
 * Using network profiler for network analysis.
 */
class NetworkProfiler {
    
    // Network request tracking
    class NetworkTracker(private val context: android.content.Context) {
        
        private val requests = mutableListOf<NetworkRequest>()
        
        fun trackRequest(
            url: String,
            method: String,
            headers: Map<String, String>,
            body: ByteArray?
        ) {
            val request = NetworkRequest(
                id = System.currentTimeMillis(),
                url = url,
                method = method,
                headers = headers,
                body = body,
                startTime = System.currentTimeMillis()
            )
            
            requests.add(request)
        }
        
        fun trackResponse(
            requestId: Long,
            statusCode: Int,
            headers: Map<String, String>,
            body: ByteArray?
        ) {
            val request = requests.find { it.id == requestId }
            request?.let {
                it.statusCode = statusCode
                it.responseHeaders = headers
                it.responseBody = body
                it.endTime = System.currentTimeMillis()
            }
        }
        
        fun getAllRequests(): List<NetworkRequest> = requests.toList()
        
        fun getRequestById(id: Long): NetworkRequest? = requests.find { it.id == id }
        
        fun getSlowRequests(thresholdMs: Long): List<NetworkRequest> {
            return requests.filter { request ->
                request.endTime?.let { it - request.startTime > thresholdMs } ?: false
            }
        }
        
        data class NetworkRequest(
            val id: Long,
            val url: String,
            val method: String,
            val headers: Map<String, String>,
            val body: ByteArray?,
            val startTime: Long,
            var endTime: Long? = null,
            var statusCode: Int? = null,
            var responseHeaders: Map<String, String>? = null,
            var responseBody: ByteArray? = null
        )
    }
    
    // OkHttp interceptor for network tracking
    class NetworkTrackingInterceptor : okhttp3.Interceptor {
        
        private val tracker = NetworkTracker(android.app.Application())
        
        override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
            val request = chain.request()
            
            tracker.trackRequest(
                url = request.url.toString(),
                method = request.method,
                headers = request.headers.toMap(),
                body = request.body?.let { readBody(it) }
            )
            
            val response = chain.proceed(request)
            
            tracker.trackResponse(
                requestId = System.currentTimeMillis(),  // Simplified
                statusCode = response.code,
                headers = response.headers.toMap(),
                body = response.body?.bytes()
            )
            
            return response
        }
        
        private fun readBody(body: okhttp3.RequestBody): ByteArray? {
            return try {
                val buffer = java.nio.ByteBuffer.allocate(body.contentLength().toInt())
                body.writeTo(java.io.BufferedOutputStream(
                    java.io.ByteArrayOutputStream().also { it.write(buffer.array()) }
                ))
                buffer.array()
            } catch (e: Exception) {
                null
            }
        }
        
        private fun okhttp3.Headers.toMap(): Map<String, String> {
            val map = mutableMapOf<String, String>()
            for (i in 0 until size) {
                map[name(i) = value(i)]
            }
            return map
        }
    }
    
    // Network statistics
    class NetworkStats(private val context: android.content.Context) {
        
        private var startTime = 0L
        private var totalBytesSent = 0L
        private var totalBytesReceived = 0L
        
        fun startMonitoring() {
            startTime = System.currentTimeMillis()
        }
        
        fun addBytesSent(bytes: Long) {
            totalBytesSent += bytes
        }
        
        fun addBytesReceived(bytes: Long) {
            totalBytesReceived += bytes
        }
        
        fun getStats(): NetworkStatistics {
            val duration = if (startTime > 0) System.currentTimeMillis() - startTime else 0
            
            return NetworkStatistics(
                durationMs = duration,
                totalBytesSent = totalBytesSent,
                totalBytesReceived = totalBytesReceived,
                requestsCount = 0,  // Track separately
                avgResponseTimeMs = 0  // Track separately
            )
        }
        
        data class NetworkStatistics(
            val durationMs: Long,
            val totalBytesSent: Long,
            val totalBytesReceived: Long,
            val requestsCount: Int,
            val avgResponseTimeMs: Long
        )
    }
}
```

---

## SECTION 4: Energy Profiler

```kotlin
/**
 * Energy Profiler Integration
 * 
 * Using energy profiler for battery analysis.
 */
class EnergyProfiler {
    
    // Energy consumption estimation
    class EnergyEstimator {
        
        // Approximate power consumption (mW)
        object PowerProfiles {
            const val SCREEN_ON = 200
            const val CPU_ACTIVE = 150
            const val WIFI_ACTIVE = 100
            const val GPS_ACTIVE = 50
            const val MOBILE_DATA = 150
            const val BLUETOOTH_ACTIVE = 20
            val CPU_IDLE = 10
            val SCREEN_FULL_BRIGHTNESS = 300
        }
        
        fun estimateEnergyConsumption(
            durationMs: Long,
            component: EnergyComponent
        ): Double {
            val powerMw = when (component) {
                EnergyComponent.CPU -> PowerProfiles.CPU_ACTIVE
                EnergyComponent.SCREEN -> PowerProfiles.SCREEN_ON
                EnergyComponent.WIFI -> PowerProfiles.WIFI_ACTIVE
                EnergyComponent.MOBILE_DATA -> PowerProfiles.MOBILE_DATA
                EnergyComponent.GPS -> PowerProfiles.GPS_ACTIVE
                EnergyComponent.BLUETOOTH -> PowerProfiles.BLUETOOTH_ACTIVE
            }
            
            // Energy = Power * Time
            // Convert mW * ms to mWh
            return powerMw * (durationMs / 1000.0) / 1000.0
        }
        
        enum class EnergyComponent {
            CPU, SCREEN, WIFI, MOBILE_DATA, GPS, BLUETOOTH
        }
    }
    
    // Battery consumption tracking
    class BatteryTracker(private val context: android.content.Context) {
        
        private val batteryManager = context.getSystemService(
            android.content.Context.BATTERY_SERVICE
        ) as android.os.BatteryManager
        
        fun getBatteryLevel(): Int {
            return batteryManager.getIntProperty(android.os.BatteryManager.BATTERY_PROPERTY_CAPACITY)
        }
        
        fun isCharging(): Boolean {
            val status = batteryManager.getIntProperty(android.os.BatteryManager.BATTERY_PROPERTY_STATUS)
            return status == android.os.BatteryManager.BATTERY_STATUS_CHARGING ||
                status == android.os.BatteryManager.BATTERY_STATUS_FULL
        }
        
        fun getBatteryChargeCounter(): Long {
            return batteryManager.getLongProperty(android.os.BatteryManager.BATTERY_PROPERTY_CHARGE_COUNTER)
        }
        
        fun getBatteryCurrentNow(): Long {
            return batteryManager.getLongProperty(android.os.BatteryManager.BATTERY_PROPERTY_CURRENT_NOW)
        }
        
        fun getEnergyCounter(): Long {
            return batteryManager.getLongProperty(android.os.BatteryManager.BATTERY_PROPERTY_ENERGY_COUNTER)
        }
    }
    
    // App standby buckets
    class AppStandbyBuckets(private val context: android.content.Context) {
        
        private val usageStatsManager = context.getSystemService(
            android.content.Context.USAGE_STATS_SERVICE
        ) as android.app.usage.UsageStatsManager
        
        fun getAppStandbyBucket(packageName: String): Int {
            return usageStatsManager.getAppStandbyBucket(packageName)
        }
        
        // Standby bucket levels (API 28+)
        object StandbyBuckets {
            const val ACTIVE = 10  // App is currently being used
            const val WORKING_SET = 20  // Recently used
            const val FREQUENT = 30  // Used often (every few days)
            const val RARE = 40  // Used rarely
            const val NEVER = 50  // Never used
            const val RESTRICTED = 60  // Restricted by battery optimization
        }
        
        fun isExemptFromRestrictions(packageName: String): Boolean {
            val bucket = getAppStandbyBucket(packageName)
            return bucket == StandbyBuckets.ACTIVE || bucket == StandbyBuckets.WORKING_SET
        }
    }
}
```

---

## Best Practices

1. **Use CPU Profiler**: Identify method-level performance bottlenecks
2. **Capture Heap Dumps**: Use Memory Profiler to find memory leaks
3. **Monitor Network**: Use Network Profiler for request analysis
4. **Track Energy**: Use Energy Profiler for battery impact analysis
5. **Synchronize Timeline**: Correlate events across all profilers
6. **Record Sessions**: Save profiler data for comparison
7. **Compare Before/After**: Test changes with baseline recordings
8. **Use Method Traces**: Identify specific slow methods
9. **Analyze Allocations**: Find memory allocation hotspots
10. **Monitor in Production**: Use Firebase Performance Monitoring

---

## Common Pitfalls and Solutions

### Pitfall 1: Profiler Affects Performance
- **Problem**: Profiler itself slows down app
- **Solution**: Use sampled tracing, minimize overhead

### Pitfall 2: Not Capturing at Right Time
- **Problem**: Missing the actual issue
- **Solution**: Reproduce issue while recording

### Pitfall 3: Not Using Proper Filters
- **Problem**: Too much data, hard to find issues
- **Solution**: Use filters for specific packages/methods

### Pitfall 4: Ignoring Thread Analysis
- **Problem**: Thread blocking not detected
- **Solution**: Check thread states in CPU profiler

### Pitfall 5: Not Tracking Over Time
- **Problem**: Memory leaks not detected early
- **Solution**: Regular heap dump comparison

---

## Troubleshooting Guide

### Issue: App Slow During Profiling
- **Steps**: 1. Use sampled tracing instead of instrumented 2. Reduce capture scope

### Issue: Can't Find Memory Leak
- **Steps**: 1. Take baseline heap dump 2. Perform actions 3. Take second dump 4. Compare

### Issue: Network Request Not Showing
- **Steps**: 1. Ensure OkHttp/Retrofit used 2. Check interceptor setup 3. Check Profiler filter

---

## EXAMPLE 1: Profiler Integration in Debug Build

```kotlin
/**
 * Profiler Integration
 * 
 * Adding profiler integration to debug builds.
 */
class ProfilerIntegration {
    
    class DebugConfiguration(private val context: android.content.Context) {
        
        private var isDebugBuild = android.os.BuildConfig.DEBUG
        
        fun initializeProfiler() {
            if (!isDebugBuild) return
            
            setupCpuProfiling()
            setupMemoryTracking()
            setupNetworkTracking()
        }
        
        private fun setupCpuProfiling() {
            // Enable systrace in debug builds
            android.os.Trace.beginSection("AppInit")
            // App initialization code
            android.os.Trace.endSection()
        }
        
        private fun setupMemoryTracking() {
            // Enable allocation tracking in debug builds
            if (isDebugBuild) {
                android.os.Debug.startAllocCounting()
            }
        }
        
        private fun setupNetworkTracking() {
            // Add network tracking interceptor in debug builds
            // This would be configured in the OkHttpClient setup
        }
        
        // Custom tracing for specific code sections
        inline fun <T> traceSection(sectionName: String, block: () -> T): T {
            return if (isDebugBuild) {
                android.os.Trace.beginSection(sectionName)
                try {
                    block()
                } finally {
                    android.os.Trace.endSection()
                }
            } else {
                block()
            }
        }
    }
    
    // Custom trace points
    class TracePoints {
        
        fun traceExpensiveOperation(operationName: String, block: () -> Unit) {
            android.os.Trace.beginSection("Expensive:$operationName")
            try {
                block()
            } finally {
                android.os.Trace.endSection()
            }
        }
        
        suspend fun <T> traceSuspendFunction(
            functionName: String,
            block: suspend () -> T
        ): T {
            android.os.Trace.beginSection("Suspend:$functionName")
            return try {
                block()
            } finally {
                android.os.Trace.endSection()
            }
        }
    }
    
    // Debug menu for profiler controls
    class DebugMenuActivity : android.app.Activity() {
        
        private val prefs = getSharedPreferences("debug_prefs", MODE_PRIVATE)
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(android.R.layout.activity_list)
            
            findViewById<android.widget.Button>(android.R.id.button1).setOnClickListener {
                startCpuProfiling()
            }
            
            findViewById<android.widget.Button>(android.R.id.button2).setOnClickListener {
                dumpHeap()
            }
            
            findViewById<android.widget.Button>(android.R.id.button3).setOnClickListener {
                captureNetworkTrace()
            }
        }
        
        private fun startCpuProfiling() {
            android.os.Debug.startMethodTracing("debug_trace")
            
            // Auto-stop after 30 seconds
            android.os.Handler(android.os.Looper.getMainLooper()).postDelayed({
                android.os.Debug.stopMethodTracing()
                // Save trace to storage
            }, 30000)
        }
        
        private fun dumpHeap() {
            val file = java.io.File(cacheDir, "heap_dump.hprof")
            android.os.Debug.dumpHprofData(file.absolutePath)
            
            // Show file path to user
            android.widget.Toast.makeText(
                this,
                "Heap dump saved: ${file.absolutePath}",
                android.widget.Toast.LENGTH_LONG
            ).show()
        }
        
        private fun captureNetworkTrace() {
            // Enable network tracking
            prefs.edit().putBoolean("network_tracking_enabled", true).apply()
        }
    }
}
```

---

## EXAMPLE 2: Profiler Data Analysis

```kotlin
/**
 * Profiler Data Analysis
 * 
 * Analyzing profiler data programmatically.
 */
class ProfilerDataAnalysis {
    
    // Analyze heap dump
    class HeapDumpAnalyzer {
        
        fun analyzeHeapDump(heapDumpFile: java.io.File): HeapAnalysisResult {
            // Use Android Studio's heap analysis
            // Or use standalone tools like MAT (Memory Analyzer Tool)
            
            val issues = mutableListOf<HeapIssue>()
            
            // Check for memory leaks
            issues.addAll(detectMemoryLeaks(heapDumpFile))
            
            // Check for large objects
            issues.addAll(findLargeObjects(heapDumpFile))
            
            // Check for bitmaps
            issues.addAll(analyzeBitmaps(heapDumpFile))
            
            return HeapAnalysisResult(issues)
        }
        
        private fun detectMemoryLeaks(heapDumpFile: java.io.File): List<HeapIssue> {
            // Implementation would use HPROF parser
            // Look for instances with significant retained heap
            return listOf()
        }
        
        private fun findLargeObjects(heapDumpFile: java.io.File): List<HeapIssue> {
            // Find objects > 1MB
            return listOf()
        }
        
        private fun analyzeBitmaps(heapDumpFile: java.io.File): List<HeapIssue> {
            // Find unreleased bitmaps
            return listOf()
        }
        
        data class HeapAnalysisResult(val issues: List<HeapIssue>)
        data class HeapIssue(
            val type: IssueType,
            val description: String,
            val size: Long,
            val instance: String?
        )
        enum class IssueType { MEMORY_LEAK, LARGE_OBJECT, BITMAP_LEAK, DUPLICATE_STRINGS }
    }
    
    // Analyze CPU trace
    class CPUTraceAnalyzer {
        
        fun analyzeTrace(traceFile: java.io.File): CPUAnalysisResult {
            val hotspots = mutableListOf<MethodHotspot>()
            
            // Parse trace file
            // Find top time-consuming methods
            
            return CPUAnalysisResult(hotspots)
        }
        
        data class MethodHotspot(
            val methodName: String,
            val selfTimeMs: Long,
            val totalTimeMs: Long,
            val callCount: Int
        )
        
        data class CPUAnalysisResult(val hotspots: List<MethodHotspot>)
    }
    
    // Generate profiler report
    class ProfilerReportGenerator {
        
        fun generateReport(
            cpuResult: CPUAnalysisResult,
            memoryResult: HeapAnalysisResult,
            networkStats: NetworkStats.NetworkStatistics
        ): String {
            return buildString {
                appendLine("=== Profiler Report ===")
                appendLine()
                appendLine("CPU Analysis:")
                cpuResult.hotspots.forEach { hotspot ->
                    appendLine("  ${hotspot.methodName}: ${hotspot.selfTimeMs}ms")
                }
                appendLine()
                appendLine("Memory Analysis:")
                memoryResult.issues.forEach { issue ->
                    appendLine("  ${issue.type}: ${issue.description} (${issue.size} bytes)")
                }
                appendLine()
                appendLine("Network Analysis:")
                appendLine("  Total sent: ${networkStats.totalBytesSent} bytes")
                appendLine("  Total received: ${networkStats.totalBytesReceived} bytes")
            }
        }
    }
}
```

---

## EXAMPLE 3: Continuous Profiling in CI/CD

```kotlin
/**
 * Continuous Profiling
 * 
 * Integrating profiling into CI/CD pipeline.
 */
class ContinuousProfiling {
    
    // Gradle tasks for profiling
    class ProfilerGradleTasks {
        
        // Build.gradle configuration
        /*
        android {
            defaultConfig {
                // Enable profiling in debug builds
                ndk {
                    abiFilters 'armeabi-v7a', 'arm64-v8a', 'x86', 'x86_64'
                }
            }
            buildTypes {
                debug {
                    // Enable profiling
                    debuggable true
                    jniDebuggable true
                }
            }
        }
        
        tasks.register("captureProfile") {
            group = "profiling"
            description = "Capture performance profile"
            doLast {
                // Execute profiling
            }
        }
        
        tasks.register("analyzeHeap") {
            group = "profiling"
            description = "Analyze heap dump"
        }
        */
        
        fun runProfilerTask() {
            // Execute gradle captureProfile
            val process = java.lang.Runtime.getRuntime().exec(
                arrayOf("./gradlew", "captureProfile", "--no-daemon")
            )
        }
    }
    
    // Firebase Performance Monitoring integration
    class FirebasePerformance {
        
        // Add to build.gradle:
        // implementation 'com.google.firebase:firebase-perf-ktx'
        
        fun initializeFirebasePerformance() {
            // Firebase Performance is automatically initialized
            
            // Add custom traces
            val trace = com.google.firebase.perf.FirebasePerfManager.getInstance()
            
            // Start custom trace
            // Note: Use Firebase Performance SDK for production
        }
        
        // Custom performance monitoring
        class CustomPerfMonitor {
            
            fun startTrace(traceName: String) {
                // Use Trace from Firebase Performance SDK
            }
            
            fun stopTrace() {
                // Stop and log trace
            }
            
            fun recordMetric(metricName: String, value: Long) {
                // Record metric
            }
        }
    }
    
    // CI/CD integration script
    class CICDIntegration {
        
        fun runProfileComparison(): Boolean {
            // 1. Build baseline
            // 2. Run profiler
            // 3. Store results
            // 4. Compare with previous
            // 5. Fail if regression
            
            return compareResults(baseline, current)
        }
        
        private fun compareResults(baseline: ProfileResult, current: ProfileResult): Boolean {
            val regressionThreshold = 1.2  // 20% regression
            
            val cpuRegression = current.cpuTime / baseline.cpuTime
            val memoryRegression = current.memoryUsage / baseline.memoryUsage
            
            return cpuRegression <= regressionThreshold &&
                memoryRegression <= regressionThreshold
        }
        
        data class ProfileResult(
            val cpuTime: Long,
            val memoryUsage: Long,
            val networkBytes: Long
        )
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**CPU Profiler Findings:**
- Method-level time breakdown
- Top time-consuming methods
- Thread states (running, blocked, waiting)
- Call stack visualization

**Memory Profiler Findings:**
- Heap dump with all objects
- Memory leak detection
- Allocation tracking
- Memory pressure events

**Network Profiler Findings:**
- Request/response timeline
- Request size and duration
- Response codes
- Payload details

**Energy Profiler Findings:**
- Power consumption estimate
- Wake lock analysis
- Job scheduler usage
- Background activity

**Profiler Tips:**
- Use sampled tracing for lower overhead
- Capture at right moment
- Save baseline for comparison
- Use filters to focus on relevant data

---

## Advanced Tips

- **Tip 1: Use Perfetto** - Advanced tracing tool for detailed analysis
- **Tip 2: Use systrace** - Command-line tracing for system-wide analysis
- **Tip 3: Add custom trace points** - Trace specific operations
- **Tip 4: Use MAT** - Standalone memory analysis tool
- **Tip 5: Firebase Performance** - Production monitoring

---

## Troubleshooting Guide (FAQ)

**Q: How do I capture a trace file?**
A: Use "adb shell am profile start/stop" or Android Studio Profiler

**Q: How do I analyze heap dump?**
A: Open .hprof file in Android Studio or use MAT

**Q: Why is profiler data large?**
A: Use sampled tracing, limit recording duration

**Q: How do I find memory leaks?**
A: Take two heap dumps and compare retained objects

---

## Advanced Tips and Tricks

- **Tip 1: Use on-device profiling** - Android Profiler on device
- **Tip 2: Record to file** - For detailed analysis offline
- **Tip 3: Use allocation tracking** - Find allocation hotspots
- **Tip 4: Correlate events** - Link CPU, Memory, Network events
- **Tip 5: Export data** - Save for team analysis

---

## CROSS-REFERENCES

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 09_PERFORMANCE/02_Debugging_Tools/02_Memory_Analysis.md
- See: 09_PERFORMANCE/02_Debugging_Tools/03_Network_Analysis.md

---

## END OF ANDROID PROFILER GUIDE

(End of file - total 682 lines)