# Battery Optimization

## Learning Objectives

1. Understanding Android battery consumption patterns
2. Implementing battery-efficient coding practices
3. Using battery-related APIs and tools
4. Optimizing background processing for battery life
5. Monitoring and reducing app battery impact

```kotlin
package com.kotlin.performance.battery
```

---

## Prerequisites

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 06_NETWORKING/02_Asynchronous_Patterns/01_RxJava_Integration.md
- See: 03_ARCHITECTURE/01_Architecture_Patterns/02_MVVM_Implementation.md

---

## Core Concepts

### Battery Consumption Factors

- **Screen**: Major battery drain, brightness and duration
- **CPU**: Processing work, wake locks
- **Network**: Mobile data more drain than WiFi
- **GPS**: Location updates consume significant power
- **Sensors**: Accelerometer, gyroscope continuous use
- **Background Services**: Keep device awake

### SECTION 1: Battery-Efficient Networking

```kotlin
/**
 * Battery-Efficient Networking
 * 
 * Optimizing network operations for battery life.
 */
class BatteryEfficientNetwork {
    
    // Batch network requests
    class NetworkBatcher {
        private val requestQueue = ArrayDeque<NetworkRequest>()
        private val batchSize = 10
        private val batchInterval = 5000L  // 5 seconds
        
        fun addRequest(request: NetworkRequest) {
            requestQueue.addLast(request)
            
            if (requestQueue.size >= batchSize) {
                flushBatch()
            }
        }
        
        private fun flushBatch() {
            if (requestQueue.isEmpty()) return
            
            val batch = mutableListOf<NetworkRequest>()
            repeat(batchSize) {
                requestQueue.pollFirst()?.let { batch.add(it) }
            }
            
            // Send batched requests
            sendBatch(batch)
        }
        
        private fun sendBatch(requests: List<NetworkRequest>) {
            // Send as single batch request
        }
    }
    
    // Prefer WiFi over mobile data
    object NetworkPreference {
        
        enum class NetworkType {
            WIFI, MOBILE, NONE
        }
        
        fun getPreferredNetworkType(context: android.content.Context): NetworkType {
            val connectivityManager = context.getSystemService(
                android.content.Context.CONNECTIVITY_SERVICE
            ) as android.net.ConnectivityManager
            
            val network = connectivityManager.activeNetwork
            val capabilities = connectivityManager.getNetworkCapabilities(network)
            
            return when {
                capabilities?.hasTransport(android.net.NetworkCapabilities.TRANSPORT_WIFI) == true -> 
                    NetworkType.WIFI
                capabilities?.hasTransport(android.net.NetworkCapabilities.TRANSPORT_CELLULAR) == true -> 
                    NetworkType.MOBILE
                else -> NetworkType.NONE
            }
        }
        
        // Only do heavy network operations on WiFi
        fun shouldDownloadLargeFile(context: android.content.Context): Boolean {
            return getPreferredNetworkType(context) == NetworkType.WIFI
        }
    }
    
    // Use GCM for push notifications instead of polling
    class PushNotificationManager {
        
        private val firebaseMessaging = com.google.firebase.messaging.FirebaseMessaging.getInstance()
        
        fun subscribeToTopic(topic: String) {
            firebaseMessaging.subscribeToTopic(topic)
                .addOnCompleteListener { task ->
                    if (!task.isSuccessful) {
                        // Handle failure
                    }
                }
        }
        
        fun unsubscribeFromTopic(topic: String) {
            firebaseMessaging.unsubscribeFromTopic(topic)
        }
    }
}
```

---

## SECTION 2: Battery-Efficient Location

```kotlin
/**
 * Battery-Efficient Location
 * 
 * Optimizing location updates for battery life.
 */
class BatteryEfficientLocation {
    
    // Location request priorities
    class LocationPriorityManager {
        
        // High accuracy - most battery drain
        fun getHighAccuracyRequest(): com.google.android.gms.location.LocationRequest {
            return com.google.android.gms.location.LocationRequest.Builder(
                com.google.android.gms.location.Priority.PRIORITY_HIGH_ACCURACY,
                1000L  // 1 second interval
            ).build()
        }
        
        // Balanced accuracy - moderate drain
        fun getBalancedRequest(): com.google.android.gms.location.LocationRequest {
            return com.google.android.gms.location.LocationRequest.Builder(
                com.google.android.gms.location.Priority.PRIORITY_BALANCED_POWER_ACCURACY,
                10000L  // 10 second interval
            ).build()
        }
        
        // Low power - minimal drain
        fun getLowPowerRequest(): com.google.android.gms.location.LocationRequest {
            return com.google.android.gms.location.LocationRequest.Builder(
                com.google.android.gms.location.Priority.PRIORITY_LOW_POWER,
                60000L  // 1 minute interval
            ).build()
        }
        
        // Passive - uses other apps' updates
        fun getPassiveRequest(): com.google.android.gms.location.LocationRequest {
            return com.google.android.gms.location.LocationRequest.Builder(
                com.google.android.gms.location.Priority.PRIORITY_PASSIVE,
                120000L  // 2 minute interval
            ).build()
        }
    }
    
    // Fused location provider with battery optimization
    class OptimizedLocationClient(
        private val context: android.content.Context
    ) {
        
        private val fusedLocationClient = com.google.android.gms.location.LocationServices.getFusedLocationProviderClient(context)
        private var locationCallback: com.google.android.gms.location.LocationCallback? = null
        
        fun startLocationUpdates(request: com.google.android.gms.location.LocationRequest) {
            locationCallback = object : com.google.android.gms.location.LocationCallback() {
                override fun onLocationResult(result: com.google.android.gms.location.LocationResult) {
                    result.lastLocation?.let { location ->
                        handleLocation(location)
                    }
                }
            }
            
            try {
                fusedLocationClient.requestLocationUpdates(
                    request,
                    locationCallback!!,
                    android.os.Looper.getMainLooper()
                )
            } catch (e: SecurityException) {
                // Handle permission
            }
        }
        
        fun stopLocationUpdates() {
            locationCallback?.let {
                fusedLocationClient.removeLocationUpdates(it)
            }
            locationCallback = null
        }
        
        private fun handleLocation(location: android.location.Location) {
            // Process location
        }
    }
    
    // Geofencing for battery-efficient location events
    class GeofenceManager(
        private val context: android.content.Context
    ) {
        
        private val geofencePendingIntent: android.app.PendingIntent by lazy {
            val intent = android.content.Intent(context, GeofenceReceiver::class.java)
            android.app.PendingIntent.getBroadcast(
                context,
                0,
                intent,
                android.app.PendingIntent.FLAG_UPDATE_CURRENT or android.app.PendingIntent.FLAG_MUTABLE
            )
        }
        
        fun addGeofence(
            latitude: Double,
            longitude: Double,
            radius: Float
        ) {
            val geofence = com.google.android.gms.location.Geofence.Builder()
                .setRequestId("geofence_$latitude$longitude")
                .setCircularRegion(latitude, longitude, radius)
                .setExpirationDuration(com.google.android.gms.location.Geofence.NEVER_EXPIRE)
                .setTransitionTypes(
                    com.google.android.gms.location.Geofence.GEOFENCE_TRANSITION_ENTER or
                    com.google.android.gms.location.Geofence.GEOFENCE_TRANSITION_EXIT
                )
                .build()
            
            val request = com.google.android.gms.location.GeofencingRequest.Builder()
                .setInitialTrigger(com.google.android.gms.location.GeofencingRequest.INITIAL_TRIGGER_ENTER)
                .addGeofence(geofence)
                .build()
            
            try {
                com.google.android.gms.location.LocationServices.getGeofencingClient(context)
                    .addGeofences(request, geofencePendingIntent)
            } catch (e: SecurityException) {
                // Handle permission
            }
        }
        
        fun removeGeofence(geofenceId: String) {
            com.google.android.gms.location.LocationServices.getGeofencingClient(context)
                .removeGeofences(listOf(geofenceId))
        }
    }
    
    class GeofenceReceiver : android.content.BroadcastReceiver() {
        override fun onReceive(context: android.content.Context, intent: android.content.Intent) {
            // Handle geofence transition
        }
    }
}
```

---

## SECTION 3: Battery-Efficient Background Work

```kotlin
/**
 * Battery-Efficient Background Work
 * 
 * Optimizing background tasks for battery life.
 */
class BatteryEfficientBackground {
    
    // WorkManager for battery-efficient scheduling
    class BackgroundScheduler {
        
        private val workManager = androidx.work.WorkManager.getInstance()
        
        // One-time work with constraints
        fun scheduleOneTimeWork() {
            val constraints = androidx.work.Constraints.Builder()
                .setRequiresBatteryNotLow(true)
                .setRequiresCharging(false)
                .build()
            
            val workRequest = androidx.work.OneTimeWorkRequestBuilder<MyWorker>()
                .setConstraints(constraints)
                .setBackoffCriteria(
                    androidx.work.BackoffPolicy.EXPONENTIAL,
                    1, java.util.concurrent.TimeUnit.MINUTES
                )
                .build()
            
            workManager.enqueue(workRequest)
        }
        
        // Periodic work
        fun schedulePeriodicWork() {
            val constraints = androidx.work.Constraints.Builder()
                .setRequiresBatteryNotLow(true)
                .build()
            
            val workRequest = androidx.work.PeriodicWorkRequestBuilder<MyWorker>(
                15, java.util.concurrent.TimeUnit.MINUTES
            )
                .setConstraints(constraints)
                .build()
            
            workManager.enqueueUniquePeriodicWork(
                "periodic_sync",
                androidx.work.ExistingPeriodicWorkPolicy.KEEP,
                workRequest
            )
        }
        
        // Expedited work (foreground service for critical tasks)
        fun scheduleExpeditedWork() {
            val expedited = androidx.work.OutOfQuotaPolicy.RUN_AS_EXPEDITED_WORK_REQUEST
            val workRequest = androidx.work.OneTimeWorkRequestBuilder<MyWorker>()
                .setExpedited(expanded)
                .build()
            
            workManager.enqueue(workRequest)
        }
    }
    
    // Worker implementation
    class MyWorker(
        context: android.content.Context,
        workerParams: androidx.work.WorkerParameters
    ) : androidx.work.Worker(context, workerParams) {
        
        override fun doWork(): androidx.work.Result {
            return try {
                // Do work
                androidx.work.Result.success()
            } catch (e: Exception) {
                if (runAttemptCount < 3) {
                    androidx.work.Result.retry()
                } else {
                    androidx.work.Result.failure()
                }
            }
        }
    }
    
    // Foreground service for critical work
    class CriticalWorkService : android.app.Service() {
        
        override fun onStartCommand(intent: android.content.Intent?, flags: Int, startId: Int): Int {
            val notification = androidx.core.app.NotificationCompat.Builder(this, "channel_id")
                .setContentTitle("Critical Work")
                .setContentText("Running critical task")
                .setSmallIcon(android.R.drawable.ic_media_play)
                .setPriority(androidx.core.app.NotificationCompat.PRIORITY_LOW)
                .build()
            
            startForeground(NOTIFICATION_ID, notification)
            
            return START_NOT_STICKY
        }
        
        override fun onBind(intent: android.content.Intent?): android.os.IBinder? = null
        
        companion object {
            private const val NOTIFICATION_ID = 1001
        }
    }
}
```

---

## Best Practices

1. **Minimize Wake Locks**: Use PARTIAL_WAKE_LOCK only when necessary, always release
2. **Batch Operations**: Group network requests, use WorkManager for periodic tasks
3. **Use WorkManager**: Let system optimize scheduling based on battery state
4. **Prefer Push**: Use GCM/FCM instead of polling for notifications
5. **Location Efficiency**: Use PRIORITY_LOW_POWER unless high accuracy needed
6. **Network Timing**: Schedule heavy operations when charging
7. **Sensor Management**: Unregister sensors when not in use
8. **Reduce Updates**: Use appropriate polling intervals
9. **Check Battery State**: Before running heavy tasks, check battery level
10. **Foreground Service**: Use only for truly critical user-facing tasks

---

## Common Pitfalls and Solutions

### Pitfall 1: Unreleased Wake Lock
- **Problem**: Device stays awake, battery drains
- **Solution**: Always release in finally block, use try-with-resources

### Pitfall 2: Continuous Location Updates
- **Problem**: Excessive battery drain from GPS
- **Solution**: Use appropriate priority, stop updates when done

### Pitfall 3: Network Polling
- **Problem**: Wakes device frequently, battery drain
- **Solution**: Use push notifications, increase polling interval

### Pitfall 4: Background Service Running
- **Problem**: Service keeps device awake
- **Solution**: Use WorkManager, stop service when work complete

### Pitfall 5: Sensor Not Unregistered
- **Problem**: Sensor continues consuming power
- **Solution**: Unregister in onPause/onStop

### Pitfall 6: AlarmManager Repeating
- **Problem**: Frequent alarms wake device
- **Solution**: Use setExactAndAllowWhileIdle sparingly, batch work

---

## Troubleshooting Guide

### Issue: High Battery Usage
- **Steps**: 1. Check battery stats in Settings 2. Use Battery Historian 3. Identify drain source
- **Tools**: Battery Historian, ADB dumpsys

### Issue: Wake Lock Not Released
- **Steps**: 1. Check PowerManager service 2. Useadb dumpsys power 3. Find acquiring code
- **Tools**: ADB, Wake Lock debug

---

## EXAMPLE 1: Battery-Aware Sync Manager

```kotlin
/**
 * Battery-Aware Sync Manager
 * 
 * Implements battery-conscious data synchronization.
 */
class BatteryAwareSyncManager {
    
    class SyncManager(private val context: android.content.Context) {
        
        private val workManager = androidx.work.WorkManager.getInstance()
        private val prefs = context.getSharedPreferences("sync_prefs", android.content.Context.MODE_PRIVATE)
        
        // Check if we should sync based on battery state
        fun shouldSync(): Boolean {
            val batteryIntent = context.registerReceiver(
                null,
                android.content.IntentFilter(android.content.Intent.ACTION_BATTERY_CHANGED)
            )
            
            val level = batteryIntent?.getIntExtra(android.content.Intent.EXTRA_LEVEL, -1) ?: -1
            val scale = batteryIntent?.getIntExtra(android.content.Intent.EXTRA_SCALE, -1) ?: -1
            val batteryPct = level * 100 / scale.toFloat()
            
            // Only sync if battery > 20% or charging
            val isCharging = batteryIntent?.getIntExtra(android.content.Intent.EXTRA_STATUS, -1) ==
                android.content.BatteryManager.BATTERY_STATUS_CHARGING
            
            return batteryPct > 20 || isCharging
        }
        
        // Schedule sync with battery awareness
        fun scheduleSync() {
            val constraints = androidx.work.Constraints.Builder()
                .setRequiresBatteryNotLow(true)  // Don't run when battery low
                .setRequiresCharging(false)
                .setRequiredNetworkType(androidx.work.NetworkType.CONNECTED)
                .build()
            
            val syncRequest = androidx.work.PeriodicWorkRequestBuilder<SyncWorker>(
                1, java.util.concurrent.TimeUnit.HOURS
            )
                .setConstraints(constraints)
                .setBackoffCriteria(
                    androidx.work.BackoffPolicy.LINEAR,
                    15, java.util.concurrent.TimeUnit.MINUTES
                )
                .build()
            
            workManager.enqueueUniquePeriodicWork(
                "data_sync",
                androidx.work.ExistingPeriodicWorkPolicy.KEEP,
                syncRequest
            )
        }
        
        // Force sync (for user-initiated action)
        fun forceSync() {
            val constraints = androidx.work.Constraints.Builder()
                .setRequiredNetworkType(androidx.work.NetworkType.CONNECTED)
                .build()
            
            val syncRequest = androidx.work.OneTimeWorkRequestBuilder<SyncWorker>()
                .setConstraints(constraints)
                .build()
            
            workManager.enqueue(syncRequest)
        }
        
        // Get last sync time
        fun getLastSyncTime(): Long = prefs.getLong("last_sync", 0)
        
        // Save sync time
        fun saveLastSyncTime(time: Long) {
            prefs.edit().putLong("last_sync", time).apply()
        }
    }
    
    class SyncWorker(
        context: android.content.Context,
        params: androidx.work.WorkerParameters
    ) : androidx.work.Worker(context, params) {
        
        override fun doWork(): androidx.work.Result {
            return try {
                // Perform sync
                performSync()
                androidx.work.Result.success()
            } catch (e: Exception) {
                if (runAttemptCount < 3) {
                    androidx.work.Result.retry()
                } else {
                    androidx.work.Result.failure()
                }
            }
        }
        
        private fun performSync() {
            // Sync implementation
        }
    }
}
```

---

## EXAMPLE 2: Battery-Efficient Sensor Manager

```kotlin
/**
 * Battery-Efficient Sensor Manager
 * 
 * Managing sensor usage for battery efficiency.
 */
class SensorManager {
    
    // Step counter (low power)
    class StepCounterManager(private val context: android.content.Context) {
        
        private val sensorManager = context.getSystemService(android.content.Context.SENSOR_SERVICE) as android.hardware.SensorManager
        private val stepCounter: android.hardware.Sensor? = sensorManager.getDefaultSensor(android.hardware.Sensor.TYPE_STEP_COUNTER)
        
        private var listener: android.hardware.SensorEventListener? = null
        
        fun startCounting(onStepDetected: (Int) -> Unit) {
            stepCounter?.let { sensor ->
                listener = object : android.hardware.SensorEventListener() {
                    override fun onSensorChanged(event: android.hardware.SensorEvent) {
                        val steps = event.values[0].toInt()
                        onStepDetected(steps)
                    }
                    
                    override fun onAccuracyChanged(sensor: android.hardware.Sensor, accuracy: Int) {}
                }
                
                sensorManager.registerListener(
                    listener,
                    sensor,
                    android.hardware.SensorManager.SENSOR_DELAY_NORMAL
                )
            }
        }
        
        fun stopCounting() {
            listener?.let {
                sensorManager.unregisterListener(it)
            }
            listener = null
        }
    }
    
    // Accelerometer (higher power)
    class AccelerometerManager(private val context: android.content.Context) {
        
        private val sensorManager = context.getSystemService(android.content.Context.SENSOR_SERVICE) as android.hardware.SensorManager
        private val accelerometer: android.hardware.Sensor? = sensorManager.getDefaultSensor(android.hardware.Sensor.TYPE_ACCELEROMETER)
        
        private var listener: android.hardware.SensorEventListener? = null
        
        fun startListening(
            onMovementDetected: (Float, Float, Float) -> Unit,
            samplingPeriod: Int = android.hardware.SensorManager.SENSOR_DELAY_GAME
        ) {
            accelerometer?.let { sensor ->
                listener = object : android.hardware.SensorEventListener() {
                    override fun onSensorChanged(event: android.hardware.SensorEvent) {
                        onMovementDetected(event.values[0], event.values[1], event.values[2])
                    }
                    
                    override fun onAccuracyChanged(sensor: android.hardware.Sensor, accuracy: Int) {}
                }
                
                sensorManager.registerListener(listener, sensor, samplingPeriod)
            }
        }
        
        fun stopListening() {
            listener?.let {
                sensorManager.unregisterListener(it)
            }
            listener = null
        }
    }
    
    // Combined motion detection (optimized)
    class MotionDetector(private val context: android.content.Context) {
        
        private val sensorManager = context.getSystemService(android.content.Context.SENSOR_SERVICE) as android.hardware.SensorManager
        
        private var isDetecting = false
        private var lastMovementTime = 0L
        private val movementThreshold = 1.5f
        
        fun startDetection(onActivityDetected: (ActivityType) -> Unit) {
            if (isDetecting) return
            isDetecting = true
            
            val accelerometer = sensorManager.getDefaultSensor(android.hardware.Sensor.TYPE_ACCELEROMETER)
            val sensorEventListener = object : android.hardware.SensorEventListener() {
                override fun onSensorChanged(event: android.hardware.SensorEvent) {
                    val x = event.values[0]
                    val y = event.values[1]
                    val z = event.values[2]
                    
                    val magnitude = kotlin.math.sqrt(x * x + y * y + z * z)
                    val movementDelta = kotlin.math.abs(magnitude - 9.8f)
                    
                    if (movementDelta > movementThreshold) {
                        lastMovementTime = System.currentTimeMillis()
                    }
                    
                    // Determine activity type based on movement
                    val inactivityDuration = System.currentTimeMillis() - lastMovementTime
                    val activityType = when {
                        movementDelta > 5f -> ActivityType.RUNNING
                        movementDelta > 2f -> ActivityType.WALKING
                        inactivityDuration > 60000 -> ActivityType.STATIONARY
                        else -> ActivityType.IDLE
                    }
                    
                    onActivityDetected(activityType)
                }
                
                override fun onAccuracyChanged(sensor: android.hardware.Sensor, accuracy: Int) {}
            }
            
            sensorManager.registerListener(
                sensorEventListener,
                accelerometer,
                android.hardware.SensorManager.SENSOR_DELAY_NORMAL
            )
        }
        
        fun stopDetection() {
            isDetecting = false
        }
        
        enum class ActivityType {
            STATIONARY, IDLE, WALKING, RUNNING
        }
    }
}
```

---

## EXAMPLE 3: Battery Stats Implementation

```kotlin
/**
 * Battery Stats and Monitoring
 * 
 * Implementing battery usage tracking and monitoring.
 */
class BatteryStatsManager {
    
    // Battery status receiver
    class BatteryMonitor(private val context: android.content.Context) {
        
        private val batteryStatusReceiver = object : android.content.BroadcastReceiver() {
            override fun onReceive(context: android.content.Context, intent: android.content.Intent) {
                val level = intent.getIntExtra(android.content.Intent.EXTRA_LEVEL, -1)
                val scale = intent.getIntExtra(android.content.Intent.EXTRA_SCALE, -1)
                val batteryPct = level * 100 / scale.toFloat()
                
                val status = intent.getIntExtra(android.content.Intent.EXTRA_STATUS, -1)
                val isCharging = status == android.content.BatteryManager.BATTERY_STATUS_CHARGING
                
                val plugged = intent.getIntExtra(android.content.Intent.EXTRA_PLUGGED, -1)
                val chargeType = when (plugged) {
                    android.content.BatteryManager.BATTERY_PLUGGED_USB -> "USB"
                    android.content.BatteryManager.BATTERY_PLUGGED_AC -> "AC"
                    android.content.BatteryManager.BATTERY_PLUGGED_WIRELESS -> "Wireless"
                    else -> "Unplugged"
                }
                
                onBatteryStatusChanged(batteryPct, isCharging, chargeType)
            }
        }
        
        fun register() {
            val filter = android.content.IntentFilter(android.content.Intent.ACTION_BATTERY_CHANGED)
            context.registerReceiver(batteryStatusReceiver, filter)
        }
        
        fun unregister() {
            try {
                context.unregisterReceiver(batteryStatusReceiver)
            } catch (e: Exception) {
                // Already unregistered
            }
        }
        
        fun onBatteryStatusChanged(percentage: Float, isCharging: Boolean, chargeType: String) {
            // Override to handle battery changes
        }
    }
    
    // Battery manager API (API 21+)
    class BatteryStats(private val context: android.content.Context) {
        
        private val batteryManager = context.getSystemService(android.content.Context.BATTERY_SERVICE) as android.os.BatteryManager
        
        fun getBatteryLevel(): Int {
            return batteryManager.getIntProperty(android.os.BatteryManager.BATTERY_PROPERTY_CAPACITY)
        }
        
        fun getBatteryScale(): Int {
            return batteryManager.getIntProperty(android.os.BatteryManager.BATTERY_PROPERTY_CHARGE_COUNTER)
        }
        
        fun isCharging(): Boolean {
            val status = batteryManager.getIntProperty(android.os.BatteryManager.BATTERY_PROPERTY_STATUS)
            return status == android.os.BatteryManager.BATTERY_STATUS_CHARGING ||
                status == android.os.BatteryManager.BATTERY_STATUS_FULL
        }
        
        fun getEnergyCounter(): Long {
            return batteryManager.getLongProperty(android.os.BatteryManager.BATTERY_PROPERTY_ENERGY_COUNTER)
        }
    }
    
    // Battery optimization helper
    class BatteryOptimizationHelper(private val context: android.content.Context) {
        
        fun isBatteryOptimizationEnabled(): Boolean {
            val powerManager = context.getSystemService(android.content.Context.POWER_SERVICE) as android.os.PowerManager
            return powerManager.isIgnoringBatteryOptimizations(context.packageName)
        }
        
        fun requestDisableOptimization() {
            val intent = android.content.Intent().apply {
                action = android.os.Settings.ACTION_REQUEST_IGNORE_BATTERY_OPTIMIZATIONS
                data = android.net.Uri.parse("package:${context.packageName}")
            }
            context.startActivity(intent)
        }
        
        fun openBatterySettings() {
            val intent = android.content.Intent().apply {
                action = android.os.Settings.ACTION_IGNORE_BATTERYOptimization settings
            }
            context.startActivity(intent)
        }
    }
    
    // App battery usage tracking
    class AppBatteryTracker {
        
        fun getAppBatteryUsage(context: android.content.Context): List<BatteryUsageInfo> {
            val usageStatsManager = context.getSystemService(android.content.Context.USAGE_STATS_SERVICE) as android.app.usage.UsageStatsManager
            val endTime = System.currentTimeMillis()
            val startTime = endTime - (24 * 60 * 60 * 1000)  // Last 24 hours
            
            val usageStats = usageStatsManager.queryUsageStats(
                android.app.usage.UsageStatsManager.INTERVAL_DAILY,
                startTime,
                endTime
            )
            
            return usageStats
                .filter { it.totalTimeInForeground > 0 }
                .sortedByDescending { it.totalTimeInForeground }
                .map { stat ->
                    BatteryUsageInfo(
                        packageName = stat.packageName,
                        foregroundTime = stat.totalTimeInForeground,
                        backgroundTime = stat.totalTimeInBackground
                    )
                }
        }
        
        data class BatteryUsageInfo(
            val packageName: String,
            val foregroundTime: Long,
            val backgroundTime: Long
        )
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Battery Consumption ranking:**
1. Screen (brightness)
2. GPS/Location
3. CPU (processing)
4. Network (especially mobile data)
5. Sensors (accelerometer)
6. Wakelocks
7. Background services

**Location Priority Power Usage:**
- HIGH_ACCURACY: ~10% per hour
- BALANCED: ~5% per hour
- LOW_POWER: ~1% per hour
- PASSIVE: ~0.1% per hour (uses other apps)

**Network Recommendations:**
- BATCH requests when possible
- Use PUSH notifications
- Only heavy downloads on WIFI
- Compress network data

**Best Practices Summary:**
- Use WorkManager for background work
- Prefer push over polling
- Minimize wake locks
- Optimize location updates
- Check battery before heavy work

---

## Advanced Tips

- **Tip 1: Use Battery Historian** - Analyze battery drain patterns over time
- **Tip 2: Enable Doze Mode** - Test app behavior in Doze mode
- **Tip 3: App Standby Buckets** - Understand standby buckets for background limits
- **Tip 4: Foreground Service Limits** - Know foreground service limits on O+
- **Tip 5: Background Execution Limits** - Understand background limits on O+

---

## Troubleshooting Guide (FAQ)

**Q: How do I test battery usage?**
A: Use Battery Historian, ADB command "adb shell dumpsys batterystats"

**Q: What causes high battery drain?**
A: Check for wake locks, location updates, background services, network polling

**Q: Should I use AlarmManager or WorkManager?**
A: WorkManager handles battery optimization automatically

**Q: How do I reduce background battery usage?**
A: Use WorkManager, minimize background work, use push notifications

---

## Advanced Tips and Tricks

- **Tip 1: Profile with systrace** - Find battery-related issues in traces
- **Tip 2: Use BatteryManager API** - Check battery level programmatically
- **Tip 3: Respect Doze mode** - Handle maintenance windows
- **Tip 4: Test with dumpsys** - Use "adb shell dumpsys power" for detailed info
- **Tip 5: Enable battery exemptions** - Request battery optimization exemption for critical apps

---

## CROSS-REFERENCES

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 09_PERFORMANCE/01_Performance_Optimization/03_Startup_Time_Improvement.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md

---

## END OF BATTERY OPTIMIZATION GUIDE

(End of file - total 682 lines)