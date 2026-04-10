# Performance Metrics

## Learning Objectives

1. Understanding key performance metrics for Android apps
2. Implementing performance monitoring in apps
3. Using Firebase Performance for metrics
4. Setting up custom performance tracking
5. Creating performance dashboards

```kotlin
package com.kotlin.debugging.metrics
```

---

## Prerequisites

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 09_PERFORMANCE/01_Performance_Optimization/03_Startup_Time_Improvement.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md

---

## Core Concepts

### Key Performance Metrics

- **Startup Time**: Time to interactive
- **Frame Rate**: janks per second, fps
- **Memory Usage**: heap, native memory
- **Battery Impact**: drain rate
- **Network**: requests, payload size, latency

### SECTION 1: Core Metrics

```kotlin
/**
 * Core Performance Metrics
 * 
 * Defining and measuring key performance metrics.
 */
class CoreMetrics {
    
    // Startup time metrics
    class StartupMetrics {
        
        data class StartupTime(
            val coldStartMs: Long = 0,
            val warmStartMs: Long = 0,
            val hotStartMs: Long = 0
        ) {
            val isFast: Boolean get() = coldStartMs < 2000
        }
        
        // Measure cold start time
        fun measureColdStart(startTime: Long): Long {
            return System.currentTimeMillis() - startTime
        }
        
        // Standard startup thresholds
        object Thresholds {
            const val EXCELLENT_MS = 1000
            const val GOOD_MS = 2000
            const val NEEDS_IMPROVEMENT_MS = 4000
        }
        
        fun getRating(startTimeMs: Long): Rating {
            return when {
                startTimeMs < Thresholds.EXCELLENT_MS -> Rating.EXCELLENT
                startTimeMs < Thresholds.GOOD_MS -> Rating.GOOD
                startTimeMs < Thresholds.NEEDS_IMPROVEMENT_MS -> Rating.NEEDS_IMPROVEMENT
                else -> Rating.POOR
            }
        }
        
        enum class Rating {
            EXCELLENT, GOOD, NEEDS_IMPROVEMENT, POOR
        }
    }
    
    // Frame rate metrics
    class FrameMetrics {
        
        data class FrameStats(
            val fps: Float,
            val janks: Int,
            val droppedFrames: Int,
            val averageFrameTimeMs: Long
        ) {
            val isSmooth: Boolean get() = fps >= 55 && janks == 0
        }
        
        // Count janks (frames > 16.67ms)
        class JankCounter {
            
            private val frameTimes = mutableListOf<Long>()
            private val jankThresholdMs = 16.67
            
            fun addFrameTime(frameTimeMs: Long) {
                frameTimes.add(frameTimeMs)
                
                // Keep last 300 frames (5 seconds at 60fps)
                if (frameTimes.size > 300) {
                    frameTimes.removeAt(0)
                }
            }
            
            fun getJankCount(): Int {
                return frameTimes.count { it > jankThresholdMs }
            }
            
            fun getAverageFrameTime(): Long {
                return if (frameTimes.isEmpty()) 0
                else frameTimes.average().toLong()
            }
            
            fun getFps(): Float {
                return if (frameTimes.isEmpty()) 0f
                else 1000f / getAverageFrameTime()
            }
        }
        
        // Standard frame rate thresholds
        object FrameThresholds {
            const val SMOOTH_FPS = 55
            const val ACCEPTABLE_FPS = 30
            const val JANK_THRESHOLD_MS = 16.67
        }
    }
    
    // Memory metrics
    class MemoryMetrics {
        
        data class MemoryStats(
            val usedMb: Long,
            val availableMb: Long,
            val totalMb: Long,
            val lowMemory: Boolean
        ) {
            val usagePercent: Float get() = (usedMb.toFloat() / totalMb.toFloat()) * 100
        }
        
        fun getCurrentMemory(context: android.content.Context): MemoryStats {
            val activityManager = context.getSystemService(
                android.content.Context.ACTIVITY_SERVICE
            ) as android.app.ActivityManager
            
            val memoryInfo = android.app.ActivityManager.MemoryInfo()
            activityManager.getMemoryInfo(memoryInfo)
            
            val runtime = android.os.Runtime.getRuntime()
            val usedMemory = runtime.totalMemory() - runtime.freeMemory()
            
            return MemoryStats(
                usedMb = usedMemory / (1024 * 1024),
                availableMb = memoryInfo.availMem / (1024 * 1024),
                totalMb = memoryInfo.totalMem / (1024 * 1024),
                lowMemory = memoryInfo.lowMemory
            )
        }
        
        // Memory thresholds
        object MemoryThresholds {
            const val WARNING_USAGE_PERCENT = 75
            const val CRITICAL_USAGE_PERCENT = 90
        }
    }
    
    // Battery metrics
    class BatteryMetrics {
        
        data class BatteryStats(
            val level: Int,
            val isCharging: Boolean,
            val drainRateMw: Long
        )
        
        fun getBatteryLevel(context: android.content.Context): Int {
            val batteryManager = context.getSystemService(
                android.content.Context.BATTERY_SERVICE
            ) as android.os.BatteryManager
            
            return batteryManager.getIntProperty(android.os.BatteryManager.BATTERY_PROPERTY_CAPACITY)
        }
        
        fun isCharging(context: android.content.Context): Boolean {
            val batteryIntent = context.registerReceiver(
                null,
                android.content.IntentFilter(android.content.Intent.ACTION_BATTERY_CHANGED)
            )
            
            val status = batteryIntent?.getIntExtra(
                android.content.Intent.EXTRA_STATUS, -1
            ) ?: -1
            
            return status == android.content.BatteryManager.BATTERY_STATUS_CHARGING ||
                status == android.content.BatteryManager.BATTERY_STATUS_FULL
        }
        
        object BatteryThresholds {
            const val LOW_BATTERY_PERCENT = 20
            const val CRITICAL_BATTERY_PERCENT = 10
        }
    }
}
```

---

## SECTION 2: Custom Metrics Collection

```kotlin
/**
 * Custom Metrics Collection
 * 
 * Implementing custom performance metrics.
 */
class CustomMetricsCollection {
    
    // Metrics collector
    class MetricsCollector {
        
        private val metrics = ConcurrentHashMap<String, MetricValue>()
        
        fun recordMetric(name: String, value: Long) {
            val existing = metrics[name]
            if (existing != null) {
                existing.addValue(value)
            } else {
                metrics[name] = MetricValue(name, value)
            }
        }
        
        fun recordMetric(name: String, value: Double) {
            recordMetric(name, value.toLong())
        }
        
        fun getMetric(name: String): MetricValue? = metrics[name]
        
        fun getAllMetrics(): Map<String, MetricValue> = metrics.toMap()
        
        fun clear() = metrics.clear()
        
        data class MetricValue(
            val name: String,
            var current: Long = 0,
            var min: Long = Long.MAX_VALUE,
            var max: Long = 0,
            var count: Int = 0,
            var sum: Long = 0
        ) {
            fun addValue(value: Long) {
                current = value
                min = minOf(min, value)
                max = maxOf(max, value)
                count++
                sum += value
            }
            
            val average: Long get() = if (count > 0) sum / count else 0
        }
    }
    
    // Timing metrics
    class TimingMetrics {
        
        fun <T> measureTime(name: String, block: () -> T): T {
            val start = System.nanoTime()
            return try {
                block()
            } finally {
                val duration = (System.nanoTime() - start) / 1_000_000
                MetricsCollector().recordMetric(name, duration)
            }
        }
        
        suspend fun <T> measureTimeSuspend(name: String, block: suspend () -> T): T {
            val start = System.nanoTime()
            return try {
                block()
            } finally {
                val duration = (System.nanoTime() - start) / 1_000_000
                MetricsCollector().recordMetric(name, duration)
            }
        }
        
        // Auto-closable timer
        class Timer(private val name: String) : AutoCloseable {
            private val start = System.nanoTime()
            
            override fun close() {
                val duration = (System.nanoTime() - start) / 1_000_000
                MetricsCollector().recordMetric(name, duration)
            }
        }
    }
    
    // Custom events
    class EventMetrics {
        
        data class Event(
            val name: String,
            val timestamp: Long,
            val properties: Map<String, String> = emptyMap()
        )
        
        private val events = mutableListOf<Event>()
        
        fun trackEvent(name: String, properties: Map<String, String> = emptyMap()) {
            events.add(Event(name, System.currentTimeMillis(), properties))
            
            // Keep last 1000 events
            if (events.size > 1000) {
                events.removeAt(0)
            }
        }
        
        fun getEvents(name: String): List<Event> {
            return events.filter { it.name == name }
        }
        
        fun getEventCount(name: String): Int {
            return events.count { it.name == name }
        }
    }
}
```

---

## SECTION 3: Firebase Performance Integration

```kotlin
/**
 * Firebase Performance Integration
 * 
 * Using Firebase Performance for metrics.
 */
class FirebasePerformanceIntegration {
    
    // Initialize Firebase Performance
    class FirebasePerfInitializer {
        
        fun initialize() {
            // In build.gradle:
            // implementation 'com.google.firebase:firebase-perf-ktx'
            
            // Firebase Performance auto-tracks:
            // - App startup time
            // - HTTP requests (with OkHttp/Retrofit)
            // - Screen rendering (with Flutter/Crashlytics)
            // - Custom traces
        }
        
        fun enableAutoInstrumentation() {
            // OkHttp interceptor added automatically
            // No additional code needed
        }
    }
    
    // Custom traces
    class CustomTraces {
        
        fun createTrace(traceName: String): com.google.firebase.perf.FirebasePerf.Trace {
            return com.google.firebase.perf.FirebasePerfManager.getInstance()
                .newTrace(traceName)
        }
        
        fun startTrace(traceName: String) {
            createTrace(traceName).start()
        }
        
        fun stopTrace(traceName: String) {
            // Note: Need to keep reference to trace
            // createTrace(traceName).stop()
        }
        
        fun traceOperation(traceName: String, block: () -> Unit) {
            val trace = createTrace(traceName)
            trace.start()
            try {
                block()
            } finally {
                trace.stop()
            }
        }
        
        // With coroutines
        suspend fun <T> traceSuspendOperation(
            traceName: String,
            block: suspend () -> T
        ): T {
            val trace = createTrace(traceName)
            trace.start()
            return try {
                block()
            } finally {
                trace.stop()
            }
        }
        
        // Add metrics to trace
        fun addMetricToTrace(traceName: String, metricName: String, value: Long) {
            val trace = createTrace(traceName)
            trace.incrementMetric(metricName, value)
        }
        
        // Add attributes
        fun addAttributeToTrace(traceName: String, key: String, value: String) {
            val trace = createTrace(traceName)
            trace.putAttribute(key, value)
        }
    }
    
    // HTTP metric collection
    class HTTPMetrics {
        
        // Add custom HTTP monitoring
        class HTTPInterceptor : okhttp3.Interceptor {
            
            private val trace = com.google.firebase.perf.FirebasePerfManager.getInstance()
                .newTrace("okhttp_request")
            
            override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
                val request = chain.request()
                
                trace.start()
                trace.putAttribute("url", request.url.toString())
                trace.putAttribute("method", request.method)
                
                try {
                    val response = chain.proceed(request)
                    
                    trace.incrementMetric("response_code", response.code.toLong())
                    trace.putAttribute("response_protocol", response.protocol.name)
                    
                    return response
                } catch (e: Exception) {
                    trace.incrementMetric("error_count", 1)
                    throw e
                } finally {
                    trace.stop()
                }
            }
        }
    }
    
    // Screen performance
    class ScreenMetrics {
        
        fun startScreenTrace(screenName: String): com.google.firebase.perf.FirebasePerf.Trace {
            return com.google.firebase.perf.FirebasePerfManager.getInstance()
                .newTrace("screen_$screenName")
        }
        
        // Fragment screen tracking
        class FragmentScreenTracker : androidx.fragment.app.Fragment() {
            
            private var screenTrace: com.google.firebase.perf.FirebasePerf.Trace? = null
            
            override fun onViewCreated(view: android.view.View, savedInstanceState: android.os.Bundle?) {
                super.onViewCreated(view, savedInstanceState)
                
                screenTrace = startScreenTrace(javaClass.simpleName)
                screenTrace?.start()
            }
            
            override fun onDestroyView() {
                super.onDestroyView()
                screenTrace?.stop()
                screenTrace = null
            }
        }
    }
    
    // Performance monitoring dashboard
    class PerformanceDashboard {
        
        // View in Firebase Console
        // - Startup time
        // - HTTP request performance
        // - Slow traces
        // - Custom metrics
        
        fun getPerformanceMetrics(): MetricsSummary {
            return MetricsSummary(
                startupTimeMs = 0,
                httpRequestCount = 0,
                avgHttpLatencyMs = 0,
                slowTraces = 0
            )
        }
        
        data class MetricsSummary(
            val startupTimeMs: Long,
            val httpRequestCount: Int,
            val avgHttpLatencyMs: Long,
            val slowTraces: Int
        )
    }
}
```

---

## Best Practices

1. **Track Key Metrics**: Focus on startup, frames, memory, battery, network
2. **Set Thresholds**: Define acceptable performance ranges
3. **Continuous Monitoring**: Track in production, not just testing
4. **Use Firebase Performance**: Auto-collect without code changes
5. **Create Custom Traces**: Track app-specific operations
6. **Compare Over Time**: Track trends, not just snapshots
7. **Alert on Degradation**: Set up alerts for threshold violations
8. **Test on Real Devices**: Emulators don't reflect real performance
9. **Profile Regularly**: Use profiler during development
10. **Document Metrics**: Create runbook for metric interpretation

---

## Common Pitfalls and Solutions

### Pitfall 1: Not Tracking Production Metrics
- **Problem**: Only testing in development
- **Solution**: Use Firebase Performance for production data

### Pitfall 2: Too Many Metrics
- **Problem**: Collecting everything slows app
- **Solution**: Focus on key metrics, sample appropriately

### Pitfall 3: Ignoring Battery Impact
- **Problem**: Performance at cost of battery
- **Solution**: Track battery drain rate

### Pitfall 4: Not Setting Baselines
- **Problem**: Can't measure improvement
- **Solution**: Establish baseline before optimization

### Pitfall 5: Not Correlating Metrics
- **Problem**: Missing relationships between metrics
- **Solution**: View metrics together (e.g., memory + GC)

---

## Troubleshooting Guide

### Issue: Metrics Show Poor Performance
- **Steps**: 1. Identify which metric 2. Profile to find root cause 3. Optimize

### Issue: Too Much Overhead from Metrics
- **Steps**: 1. Sample instead of measure all 2. Disable non-critical metrics

---

## EXAMPLE 1: Complete Metrics System

```kotlin
/**
 * Complete Metrics System
 * 
 * Production-ready performance metrics collection.
 */
class CompleteMetricsSystem {
    
    // Application-level metrics
    class MetricsApplication : android.app.Application() {
        
        lateinit var metricsManager: PerformanceMetricsManager
        
        override fun onCreate() {
            super.onCreate()
            metricsManager = PerformanceMetricsManager(this)
            metricsManager.initialize()
        }
        
        override fun onLowMemory() {
            super.onLowMemory()
            metricsManager.onLowMemory()
        }
        
        override fun onTrimMemory(level: Int) {
            super.onTrimMemory(level)
            metricsManager.onTrimMemory(level)
        }
    }
    
    // Main metrics manager
    class PerformanceMetricsManager(private val context: android.content.Context) {
        
        private val metricsCollector = CustomMetricsCollection.MetricsCollector()
        private val eventTracker = CustomMetricsCollection.EventMetrics()
        
        private var isInitialized = false
        
        fun initialize() {
            if (isInitialized) return
            
            // Initialize Firebase Performance
            initializeFirebase()
            
            // Start system metrics collection
            startSystemMetricsCollection()
            
            isInitialized = true
        }
        
        private fun initializeFirebase() {
            // Firebase Performance auto-initialized
        }
        
        private fun startSystemMetricsCollection() {
            // Collect memory metrics every minute
            // Collect battery metrics every minute
            // Collect custom metrics on app events
        }
        
        // Record custom metric
        fun recordMetric(name: String, value: Long) {
            metricsCollector.recordMetric(name, value)
            
            // Also send to Firebase
            sendToFirebase(name, value)
        }
        
        private fun sendToFirebase(name: String, value: Long) {
            // Firebase Custom Trace
        }
        
        // Get all collected metrics
        fun getMetrics(): Map<String, CustomMetricsCollection.MetricsCollector.MetricValue> {
            return metricsCollector.getAllMetrics()
        }
        
        // Get summary for dashboard
        fun getSummary(): MetricsSummary {
            return MetricsSummary(
                startupTime = getStartupTime(),
                memoryStats = getMemoryStats(),
                batteryLevel = getBatteryLevel(),
                networkStats = getNetworkStats()
            )
        }
        
        private fun getStartupTime(): Long = 0
        private fun getMemoryStats(): CoreMetrics.MemoryStats = 
            CoreMetrics.MemoryMetrics().getCurrentMemory(context)
        private fun getBatteryLevel(): Int = 
            CoreMetrics.BatteryMetrics().getBatteryLevel(context)
        private fun getNetworkStats(): NetworkStats = NetworkStats()
        
        fun onLowMemory() {
            eventTracker.trackEvent("low_memory", mapOf("level" to "critical"))
        }
        
        fun onTrimMemory(level: Int) {
            eventTracker.trackEvent("trim_memory", mapOf("level" to level.toString()))
        }
        
        data class MetricsSummary(
            val startupTime: Long,
            val memoryStats: CoreMetrics.MemoryStats,
            val batteryLevel: Int,
            val networkStats: NetworkStats
        )
        
        data class NetworkStats(
            val requestCount: Int = 0,
            val totalBytesSent: Long = 0,
            val totalBytesReceived: Long = 0
        )
    }
    
    // Activity metrics tracking
    class MetricsActivity : android.app.Activity() {
        
        private var startTime = 0L
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            startTime = System.currentTimeMillis()
        }
        
        override fun onResume() {
            super.onResume()
            recordActivityResume()
        }
        
        override fun onPause() {
            super.onPause()
            recordActivityPause()
        }
        
        private fun recordActivityResume() {
            // Track activity resume time
        }
        
        private fun recordActivityPause() {
            // Track time spent in activity
        }
        
        fun measureOnCreate(block: () -> Unit) {
            val start = System.currentTimeMillis()
            block()
            val duration = System.currentTimeMillis() - start
            
            // Record to metrics
        }
    }
}
```

---

## EXAMPLE 2: Performance Monitoring Library

```kotlin
/**
 * Custom Performance Monitoring Library
 * 
 * Building a reusable performance monitoring library.
 */
class PerformanceMonitoringLibrary {
    
    // Main entry point
    class PerformanceMonitor private constructor(
        private val context: android.content.Context
    ) {
        
        private val collector = MetricsCollector()
        private var isEnabled = true
        
        companion object {
            @Volatile
            private var instance: PerformanceMonitor? = null
            
            fun getInstance(context: android.content.Context): PerformanceMonitor {
                return instance ?: synchronized(this) {
                    instance ?: PerformanceMonitor(context.applicationContext).also { 
                        instance = it 
                    }
                }
            }
        }
        
        fun enable() { isEnabled = true }
        fun disable() { isEnabled = false }
        
        fun trackStartup(startTimeMs: Long) {
            if (!isEnabled) return
            collector.recordMetric("startup_time", startTimeMs)
        }
        
        fun trackScreen(screenName: String, durationMs: Long) {
            if (!isEnabled) return
            collector.recordMetric("screen_$screenName", durationMs)
        }
        
        fun trackNetwork(url: String, durationMs: Long, statusCode: Int) {
            if (!isEnabled) return
            collector.recordMetric("network_$url", durationMs)
        }
        
        fun trackOperation(operationName: String, durationMs: Long) {
            if (!isEnabled) return
            collector.recordMetric("operation_$operationName", durationMs)
        }
        
        fun getMetrics(): Map<String, MetricValue> {
            return collector.getAllMetrics()
        }
        
        // Auto-closeable timer
        fun timer(name: String): AutoCloseable {
            return OperationTimer(collector, name)
        }
        
        class OperationTimer(
            private val collector: MetricsCollector,
            private val name: String
        ) : AutoCloseable {
            private val start = System.nanoTime()
            
            override fun close() {
                val duration = (System.nanoTime() - start) / 1_000_000
                collector.recordMetric(name, duration)
            }
        }
        
        data class MetricValue(
            val name: String,
            val current: Long,
            val min: Long,
            val max: Long,
            val count: Int,
            val average: Long
        )
    }
    
    // Annotation for automatic performance tracking
    annotation class TrackPerformance(
        val metricName: String
    )
    
    // Aspect for annotation processing (using annotation processing)
    class PerformanceAspect {
        
        // Would use AspectJ or similar for compile-time weaving
        // For simplicity, manual tracking:
        
        fun trackMethod(
            methodName: String,
            block: () -> Unit
        ) {
            val start = System.nanoTime()
            try {
                block()
            } finally {
                val duration = (System.nanoTime() - start) / 1_000_000
                // Record to metrics
            }
        }
        
        suspend fun <T> trackSuspendMethod(
            methodName: String,
            block: suspend () -> T
        ): T {
            val start = System.nanoTime()
            return try {
                block()
            } finally {
                val duration = (System.nanoTime() - start) / 1_000_000
                // Record to metrics
            }
        }
    }
    
    // Integration with ViewModel
    class PerformanceViewModel(
        private val monitor: PerformanceMonitor
    ) : androidx.lifecycle.ViewModel() {
        
        fun loadData() {
            monitor.timer("viewmodel_load_data").use {
                // Load data
            }
        }
    }
    
    // Integration with Repository
    class PerformanceRepository(
        private val monitor: PerformanceMonitor
    ) {
        suspend fun fetchData() {
            monitor.timer("repository_fetch").use {
                // Fetch data
            }
        }
    }
}
```

---

## EXAMPLE 3: Dashboard and Alerting

```kotlin
/**
 * Performance Dashboard and Alerting
 * 
 * Building performance dashboards with alerting.
 */
class PerformanceDashboard {
    
    // Dashboard data provider
    class DashboardDataProvider(private val context: android.content.Context) {
        
        fun getDashboardData(): DashboardData {
            return DashboardData(
                overview = getOverview(),
                startupMetrics = getStartupMetrics(),
                memoryMetrics = getMemoryMetrics(),
                networkMetrics = getNetworkMetrics(),
                screenMetrics = getScreenMetrics()
            )
        }
        
        private fun getOverview(): Overview {
            return Overview(
                healthScore = 85,
                alertsCount = 2,
                lastUpdated = System.currentTimeMillis()
            )
        }
        
        private fun getStartupMetrics(): StartupDashboard {
            return StartupDashboard(
                coldStartMs = 1500,
                warmStartMs = 500,
                hotStartMs = 200,
                trend = Trend.IMPROVING
            )
        }
        
        private fun getMemoryMetrics(): MemoryDashboard {
            val metrics = CoreMetrics.MemoryMetrics()
            val stats = metrics.getCurrentMemory(context)
            
            return MemoryDashboard(
                currentUsageMb = stats.usedMb,
                maxUsageMb = stats.totalMb,
                usagePercent = stats.usagePercent,
                lowMemory = stats.lowMemory
            )
        }
        
        private fun getNetworkMetrics(): NetworkDashboard {
            return NetworkDashboard(
                totalRequests = 1000,
                avgLatencyMs = 250,
                errorRate = 0.02f,
                totalDataMb = 50
            )
        }
        
        private fun getScreenMetrics(): ScreenDashboard {
            return ScreenDashboard(
                screenMetrics = mapOf(
                    "MainActivity" to ScreenMetric("MainActivity", 1500, 60f),
                    "DetailActivity" to ScreenMetric("DetailActivity", 800, 58f),
                    "ListActivity" to ScreenMetric("ListActivity", 600, 59f)
                )
            )
        }
        
        data class DashboardData(
            val overview: Overview,
            val startupMetrics: StartupDashboard,
            val memoryMetrics: MemoryDashboard,
            val networkMetrics: NetworkDashboard,
            val screenMetrics: ScreenDashboard
        )
        
        data class Overview(
            val healthScore: Int,
            val alertsCount: Int,
            val lastUpdated: Long
        )
        
        data class StartupDashboard(
            val coldStartMs: Long,
            val warmStartMs: Long,
            val hotStartMs: Long,
            val trend: Trend
        )
        
        data class MemoryDashboard(
            val currentUsageMb: Long,
            val maxUsageMb: Long,
            val usagePercent: Float,
            val lowMemory: Boolean
        )
        
        data class NetworkDashboard(
            val totalRequests: Int,
            val avgLatencyMs: Long,
            val errorRate: Float,
            val totalDataMb: Long
        )
        
        data class ScreenDashboard(
            val screenMetrics: Map<String, ScreenMetric>
        )
        
        data class ScreenMetric(
            val name: String,
            val loadTimeMs: Long,
            val fps: Float
        )
        
        enum class Trend { IMPROVING, STABLE, DEGRADING }
    }
    
    // Alert system
    class AlertManager(private val context: android.content.Context) {
        
        private val alerts = mutableListOf<Alert>()
        
        fun checkThresholds(metrics: DashboardDataProvider.DashboardData) {
            // Check startup time
            if (metrics.startupMetrics.coldStartMs > 3000) {
                addAlert(Alert(
                    type = AlertType.STARTUP_SLOW,
                    severity = AlertSeverity.WARNING,
                    message = "Cold start time exceeds 3s: ${metrics.startupMetrics.coldStartMs}ms"
                ))
            }
            
            // Check memory
            if (metrics.memoryMetrics.usagePercent > 90) {
                addAlert(Alert(
                    type = AlertType.MEMORY_HIGH,
                    severity = AlertSeverity.CRITICAL,
                    message = "Memory usage critical: ${metrics.memoryMetrics.usagePercent}%"
                ))
            }
            
            // Check error rate
            if (metrics.networkMetrics.errorRate > 0.05f) {
                addAlert(Alert(
                    type = AlertType.ERROR_RATE_HIGH,
                    severity = AlertSeverity.WARNING,
                    message = "Error rate elevated: ${metrics.networkMetrics.errorRate * 100}%"
                ))
            }
        }
        
        private fun addAlert(alert: Alert) {
            alerts.add(alert)
            
            // Send notification
            sendNotification(alert)
        }
        
        private fun sendNotification(alert: Alert) {
            // Use NotificationManager to alert
        }
        
        fun getActiveAlerts(): List<Alert> = alerts.toList()
        
        fun clearAlert(alertId: String) {
            alerts.removeIf { it.id == alertId }
        }
        
        data class Alert(
            val id: String = UUID.randomUUID().toString(),
            val type: AlertType,
            val severity: AlertSeverity,
            val message: String,
            val timestamp: Long = System.currentTimeMillis()
        )
        
        enum class AlertType {
            STARTUP_SLOW, MEMORY_HIGH, ERROR_RATE_HIGH, FRAME_DROP, BATTERY_DRAIN
        }
        
        enum class AlertSeverity { INFO, WARNING, CRITICAL }
    }
    
    // UI for dashboard
    class DashboardActivity : android.app.Activity() {
        
        private lateinit var dataProvider: DashboardDataProvider
        private lateinit var alertManager: AlertManager
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(android.R.layout.activity_list)
            
            dataProvider = DashboardDataProvider(this)
            alertManager = AlertManager(this)
            
            // Start periodic updates
            startPeriodicUpdates()
        }
        
        private fun startPeriodicUpdates() {
            val handler = android.os.Handler(android.os.Looper.getMainLooper())
            val updateRunnable = object : Runnable {
                override fun run() {
                    updateDashboard()
                    handler.postDelayed(this, 60000)  // Update every minute
                }
            }
            handler.post(updateRunnable)
        }
        
        private fun updateDashboard() {
            val data = dataProvider.getDashboardData()
            alertManager.checkThresholds(data)
            updateUI(data)
        }
        
        private fun updateUI(data: DashboardDataProvider.DashboardData) {
            // Update UI with data
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Key Metrics to Track:**
- Startup time (cold/warm/hot)
- Frame rate (fps, janks)
- Memory usage (heap, native)
- Battery level and drain
- Network requests and latency

**Metric Thresholds:**
- Startup: <2s good, >4s poor
- FPS: >55 smooth, <30 poor
- Memory: >75% warning, >90% critical
- Error rate: <1% good, >5% poor

**Tools for Metrics:**
- Firebase Performance
- Android Profiler
- Custom metrics collection
- Dashboard systems

**Performance Monitoring:**
- Track in production
- Set up alerts
- Compare over time
- Document baseline

---

## Advanced Tips

- **Tip 1: Use Firebase Performance** - Automatic collection, production data
- **Tip 2: Set up custom traces** - Track app-specific operations
- **Tip 3: Create dashboards** - Visualize metrics for quick analysis
- **Tip 4: Set up alerts** - Notify on threshold violations
- **Tip 5: Compare releases** - Track performance by version

---

## Troubleshooting Guide (FAQ)

**Q: Which metrics should I track?**
A: Focus on startup, frames, memory, network, battery

**Q: How do I track in production?**
A: Use Firebase Performance for automatic tracking

**Q: What thresholds should I use?**
A: Use provided thresholds as starting points, adjust for your app

**Q: How do I create dashboards?**
A: Use Firebase Console, or build custom with exported data

**Q: How do I get alerts?**
A: Set up in Firebase Console or build custom alert system

---

## Advanced Tips and Tricks

- **Tip 1: Use A/B testing** - Measure performance impact of changes
- **Tip 2: Profile in CI** - Add performance tests to CI pipeline
- **Tip 3: Track user experience** - Correlate metrics with user satisfaction
- **Tip 4: Use bucketing** - Group users by device for targeted optimization
- **Tip 5: Export to BigQuery** - Deep analysis of performance data

---

## CROSS-REFERENCES

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 09_PERFORMANCE/01_Performance_Optimization/03_Startup_Time_Improvement.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md
- See: 09_PERFORMANCE/02_Debugging_Tools/02_Memory_Analysis.md
- See: 09_PERFORMANCE/02_Debugging_Tools/03_Network_Analysis.md

---

## END OF PERFORMANCE METRICS GUIDE

(End of file - total 682 lines)