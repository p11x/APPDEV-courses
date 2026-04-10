# Memory Analysis

## Learning Objectives

1. Understanding memory analysis fundamentals
2. Using heap dumps for memory leak detection
3. Analyzing memory allocation patterns
4. Identifying and fixing memory leaks
5. Using memory analysis tools effectively

```kotlin
package com.kotlin.debugging.memory
```

---

## Prerequisites

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md
- See: 03_ARCHITECTURE/01_Architecture_Patterns/02_MVVM_Implementation.md

---

## Core Concepts

### Memory Analysis Basics

- **Heap**: Memory area for object allocation
- **GC Roots**: Objects always retained
- **Retained Heap**: Total memory if object retained
- **Shallow Heap**: Object's own memory
- **Dominators**: Objects that must be retained

### SECTION 1: Heap Dump Analysis

```kotlin
/**
 * Heap Dump Analysis
 * 
 * Analyzing heap dumps to find memory issues.
 */
class HeapDumpAnalysis {
    
    // Parse heap dump
    class HeapDumpParser {
        
        fun parseHprofFile(filePath: String): HeapDump {
            // Use HPROF parser library
            val parser = com.android.tools.perflib.heap.HprofParser()
            return parser.parse(filePath)
        }
        
        fun findMemoryLeak(heapDump: HeapDump): List<MemoryLeakCandidate> {
            val candidates = mutableListOf<MemoryLeakCandidate>()
            
            // Find activities in memory
            val activities = heapDump.resolveClass("android.app.Activity")
                .filter { it.instanceCount > 0 }
            
            activities.forEach { clazz ->
                val instances = clazz.instances
                if (instances.size > expectedCount(clazz.name)) {
                    candidates.add(MemoryLeakCandidate(
                        className = clazz.name,
                        instanceCount = instances.size,
                        retainedHeap = calculateRetainedHeap(instances),
                        likelyCause = guessCause(clazz.name)
                    ))
                }
            }
            
            return candidates
        }
        
        private fun expectedCount(className: String): Int {
            return when {
                className.contains("Activity") -> 1
                className.contains("Fragment") -> 2
                else -> 0
            }
        }
        
        private fun calculateRetainedHeap(instances: List<com.android.tools.perflib.heap.Instance>): Long {
            return instances.sumOf { it.retainedSize }
        }
        
        private fun guessCause(className: String): String {
            return when {
                className.contains("Activity") -> "Activity leak - check lifecycle"
                className.contains("Fragment") -> "Fragment leak - check fragment manager"
                className.contains("Context") -> "Context leak - use application context"
                else -> "Unknown leak source"
            }
        }
        
        data class MemoryLeakCandidate(
            val className: String,
            val instanceCount: Int,
            val retainedHeap: Long,
            val likelyCause: String
        )
    }
    
    // Find large objects
    class LargeObjectFinder {
        
        fun findLargeObjects(heapDump: HeapDump, thresholdBytes: Long = 1024 * 1024): List<LargeObject> {
            val largeObjects = mutableListOf<LargeObject>()
            
            heapDump.allInstances.forEach { instance ->
                if (instance.retainedSize > thresholdBytes) {
                    largeObjects.add(LargeObject(
                        className = instance.className,
                        retainedSize = instance.retainedSize,
                        shallowSize = instance.size,
                        stackTrace = instance.referencePath
                    ))
                }
            }
            
            return largeObjects.sortedByDescending { it.retainedSize }
        }
        
        // Common large object types
        class CommonLargeTypes {
            val problematicTypes = listOf(
                "android.graphics.Bitmap",
                "android.graphics.drawable.BitmapDrawable",
                "[B",  // byte[]
                "[C",  // char[]
                "java.lang.String"
            )
        }
        
        data class LargeObject(
            val className: String,
            val retainedSize: Long,
            val shallowSize: Long,
            val stackTrace: String
        )
    }
    
    // Analyze bitmap allocations
    class BitmapAnalyzer {
        
        fun findBitmapLeaks(heapDump: HeapDump): List<BitmapLeak> {
            val bitmaps = heapDump.resolveClass("android.graphics.Bitmap")
                .flatMap { it.instances }
            
            return bitmaps.map { bitmap ->
                BitmapLeak(
                    width = getBitmapWidth(bitmap),
                    height = getBitmapHeight(bitmap),
                    byteCount = bitmap.retainedSize,
                    allocationStack = getAllocationStack(bitmap)
                )
            }.sortedByDescending { it.byteCount }
        }
        
        private fun getBitmapWidth(bitmap: com.android.tools.perflib.heap.Instance): Int {
            return 0  // Implementation would extract from bitmap fields
        }
        
        private fun getBitmapHeight(bitmap: com.android.tools.perflib.heap.Instance): Int {
            return 0
        }
        
        private fun getAllocationStack(bitmap: com.android.tools.perflib.heap.Instance): String {
            return bitmap.referencePath ?: "Unknown"
        }
        
        data class BitmapLeak(
            val width: Int,
            val height: Int,
            val byteCount: Long,
            val allocationStack: String
        )
    }
}
```

---

## SECTION 2: Allocation Tracking

```kotlin
/**
 * Allocation Tracking
 * 
 * Tracking memory allocations to find hotspots.
 */
class AllocationTracking {
    
    // Monitor allocations
    class AllocationMonitor {
        
        private var isTracking = false
        private val allocations = mutableListOf<AllocationRecord>()
        
        fun startTracking() {
            isTracking = true
            allocations.clear()
            android.os.Debug.startAllocCounting()
        }
        
        fun stopTracking(): List<AllocationRecord> {
            isTracking = false
            android.os.Debug.stopAllocCounting()
            return allocations.toList()
        }
        
        fun recordAllocation(
            className: String,
            size: Long,
            stackTrace: String
        ) {
            if (isTracking) {
                allocations.add(AllocationRecord(
                    className = className,
                    size = size,
                    timestamp = System.currentTimeMillis(),
                    stackTrace = stackTrace
                ))
            }
        }
        
        fun getAllocationHotspots(thresholdMs: Long): List<AllocationHotspot> {
            return allocations
                .groupBy { it.className }
                .map { (className, records) ->
                    AllocationHotspot(
                        className = className,
                        totalAllocations = records.size,
                        totalBytes = records.sumOf { it.size },
                        avgSize = records.map { it.size }.average()
                    )
                }
                .sortedByDescending { it.totalBytes }
                .take(20)
        }
        
        data class AllocationRecord(
            val className: String,
            val size: Long,
            val timestamp: Long,
            val stackTrace: String
        )
        
        data class AllocationHotspot(
            val className: String,
            val totalAllocations: Int,
            val totalBytes: Long,
            val avgSize: Double
        )
    }
    
    // Allocation listener for fine-grained tracking
    class AllocationListener : android.os.Debug.AllocationListener {
        
        override fun onAllocation(
            className: String,
            size: Long,
            threadId: Int,
            allocationStackTrace: Array<String>
        ) {
            // Process allocation
            println("Allocation: $className, size: $size, thread: $threadId")
        }
    }
    
    // Object lifecycle tracking
    class ObjectLifecycleTracker {
        
        private val objectTracker = mutableMapOf<String, MutableList<ObjectLifeEvent>>()
        
        fun trackObjectCreation(tag: String, objectType: String, identity: Int) {
            val events = objectTracker.getOrPut(objectType) { mutableListOf() }
            events.add(ObjectLifeEvent(
                eventType = EventType.CREATED,
                timestamp = System.currentTimeMillis(),
                objectId = identity,
                tag = tag
            ))
        }
        
        fun trackObjectDestruction(tag: String, objectType: String, identity: Int) {
            val events = objectTracker[objectType] ?: return
            events.add(ObjectLifeEvent(
                eventType = EventType.DESTROYED,
                timestamp = System.currentTimeMillis(),
                objectId = identity,
                tag = tag
            ))
        }
        
        fun getLifecycleStats(objectType: String): LifecycleStats {
            val events = objectTracker[objectType] ?: return LifecycleStats(0, 0, 0)
            
            val created = events.count { it.eventType == EventType.CREATED }
            val destroyed = events.count { it.eventType == EventType.DESTROYED }
            val alive = created - destroyed
            
            return LifecycleStats(created, destroyed, alive)
        }
        
        enum class EventType { CREATED, DESTROYED }
        
        data class ObjectLifeEvent(
            val eventType: EventType,
            val timestamp: Long,
            val objectId: Int,
            val tag: String
        )
        
        data class LifecycleStats(
            val createdCount: Int,
            val destroyedCount: Int,
            val aliveCount: Int
        )
    }
}
```

---

## SECTION 3: Leak Detection Patterns

```kotlin
/**
 * Leak Detection Patterns
 * 
 * Common memory leak patterns and detection.
 */
class LeakDetectionPatterns {
    
    // Activity leak detection
    class ActivityLeakDetector(private val application: android.app.Application) {
        
        private val activityTracker = mutableListOf<ActivityRef>()
        
        fun registerActivity(activity: android.app.Activity) {
            activityTracker.add(ActivityRef(activity, System.currentTimeMillis()))
        }
        
        fun unregisterActivity(activity: android.app.Activity) {
            activityTracker.removeIf { it.activity.get() === activity }
        }
        
        fun detectLeaks(): List<LeakInfo> {
            val leaks = mutableListOf<LeakInfo>()
            
            activityTracker.forEach { ref ->
                val activity = ref.activity.get()
                if (activity != null && activity.isFinishing) {
                    leaks.add(LeakInfo(
                        leakedObject = activity.javaClass.name,
                        leakTime = System.currentTimeMillis() - ref.registeredAt,
                        likelyCause = "Activity still referenced after finish"
                    ))
                }
            }
            
            return leaks
        }
        
        data class ActivityRef(
            val activity: java.lang.ref.WeakReference<android.app.Activity>,
            val registeredAt: Long
        )
        
        data class LeakInfo(
            val leakedObject: String,
            val leakTime: Long,
            val likelyCause: String
        )
    }
    
    // Fragment leak detection
    class FragmentLeakDetector(private val fragmentManager: androidx.fragment.app.FragmentManager) {
        
        private val activeFragments = mutableSetOf<WeakReference<androidx.fragment.app.Fragment>>()
        
        fun registerFragment(fragment: androidx.fragment.app.Fragment) {
            activeFragments.add(WeakReference(fragment))
        }
        
        fun detectLeaks(): List<FragmentLeakInfo> {
            val leaks = mutableListOf<FragmentLeakInfo>()
            
            activeFragments.forEach { ref ->
                val fragment = ref.get()
                if (fragment != null && fragment.isDetached) {
                    // Check if fragment is still in fragment manager
                    val foundFragment = fragmentManager.findFragmentByTag(fragment.tag)
                    if (foundFragment != null) {
                        leaks.add(FragmentLeakInfo(
                            fragmentClass = fragment.javaClass.name,
                            tag = fragment.tag ?: "unknown",
                            cause = "Detached fragment still in FragmentManager"
                        ))
                    }
                }
            }
            
            return leaks
        }
        
        data class FragmentLeakInfo(
            val fragmentClass: String,
            val tag: String,
            val cause: String
        )
    }
    
    // Listener leak detection
    class ListenerLeakDetector {
        
        fun detectUnreleasedListeners(heapDump: HeapDump): List<ListenerLeak> {
            val leaks = mutableListOf<ListenerLeak>()
            
            // Find View.OnClickListener implementations
            val clickListeners = heapDump.resolveClass("android.view.View\$OnClickListener")
            
            clickListeners.forEach { clazz ->
                clazz.instances.forEach { instance ->
                    val enclosing = instance.getFieldValue("this\$0")  // Outer class reference
                    if (enclosing != null) {
                        leaks.add(ListenerLeak(
                            listenerType = clazz.name,
                            outerClass = enclosing.className,
                            retainedSize = instance.retainedSize
                        ))
                    }
                }
            }
            
            return leaks
        }
        
        data class ListenerLeak(
            val listenerType: String,
            val outerClass: String,
            val retainedSize: Long
        )
    }
    
    // Static reference leak detection
    class StaticReferenceLeak {
        
        fun findStaticLeaks(heapDump: HeapDump): List<StaticLeak> {
            val leaks = mutableListOf<StaticLeak>()
            
            // Find static fields holding activity/context references
            val activityClasses = heapDump.resolveClass("android.app.Activity")
            
            activityClasses.forEach { clazz ->
                clazz.staticFields.forEach { field ->
                    val fieldValue = field.value
                    if (fieldValue is com.android.tools.perflib.heap.Instance) {
                        leaks.add(StaticLeak(
                            className = clazz.name,
                            fieldName = field.name,
                            fieldType = field.type,
                            retainedSize = fieldValue.retainedSize
                        ))
                    }
                }
            }
            
            return leaks
        }
        
        data class StaticLeak(
            val className: String,
            val fieldName: String,
            val fieldType: String,
            val retainedSize: Long
        )
    }
}
```

---

## Best Practices

1. **Take Baseline Heap Dump**: Capture heap state at app startup
2. **Compare Heap Dumps**: Use diff to find leaked objects
3. **Focus on Retained Heap**: Large retained size means bigger issue
4. **Check GC Roots**: Understand why object is retained
5. **Use MAT for Deep Analysis**: Standalone memory analyzer
6. **Run on Real Device**: Emulators have different memory behavior
7. **Reproduce Issue in Loop**: Make action that causes leak multiple times
8. **Check LeakCanary Warnings**: Use library for automatic detection
9. **Track Fragment Manager**: Common source of fragment leaks
10. **Review Static Fields**: Common cause of context leaks

---

## Common Pitfalls and Solutions

### Pitfall 1: Not Capturing at Right Time
- **Problem**: Capturing heap after GC runs
- **Solution**: Force GC before capture, capture immediately after leak action

### Pitfall 2: Interpreting Shallow vs Retained
- **Problem**: Confusing object size with retained size
- **Solution**: Focus on retained heap for leak impact

### Pitfall 3: Missing Native Memory Leaks
- **Problem**: Only checking Java heap
- **Solution**: Use native memory profiling for Bitmap issues

### Pitfall 4: Not Using LeakCanary
- **Problem**: Manual leak detection only
- **Solution**: Use LeakCanary library in debug builds

### Pitfall 5: Fragment Not Added to Manager
- **Problem**: Detached fragment still retained
- **Solution**: Ensure proper fragment lifecycle handling

---

## Troubleshooting Guide

### Issue: High Memory Usage
- **Steps**: 1. Take heap dump 2. Check large objects 3. Find bitmap allocations

### Issue: Memory Keeps Growing
- **Steps**: 1. Capture multiple heap dumps 2. Compare retained objects 3. Identify leak

---

## EXAMPLE 1: LeakCanary Integration

```kotlin
/**
 * LeakCanary Integration
 * 
 * Using LeakCanary for automatic leak detection.
 */
class LeakCanaryIntegration {
    
    // Add to build.gradle:
    // debugImplementation 'com.squareup.leakcanary:leakcanary-android:2.x'
    
    // LeakCanary automatically detects:
    // - Activity leaks
    // - Fragment leaks
    // - View leaks
    // - Custom object leaks
    
    // Custom leak analysis
    class CustomLeakAnalysis {
        
        // Create custom leak watcher
        class CustomObjectWatcher {
            
            private val watchedObjects = mutableMapOf<String, java.lang.ref.WeakReference<Any>>()
            
            fun watchObject(obj: Any, description: String) {
                val referenceKey = UUID.randomUUID().toString()
                watchedObjects[referenceKey] = java.lang.ref.WeakReference(obj)
                
                // This would be handled by LeakCanary
                // leakcanary.internal.watch(obj, description)
            }
            
            fun removeWatchedObject(key: String) {
                watchedObjects.remove(key)
            }
        }
        
        // Custom leak suspect
        class LeakSuspectAnalyzer {
            
            fun analyzeHeapDump(heapDump: java.io.File): LeakAnalysisReport {
                // Use LeakCanary's built-in analysis
                // Or create custom analysis using perflib
                
                return LeakAnalysisReport(
                    leaks = listOf(
                        Leak(
                            className = "com.example.MyActivity",
                            leakSize = 1024 * 1024,
                            stackTrace = "Activity -> mContext -> ... -> Activity"
                        )
                    )
                )
            }
            
            data class LeakAnalysisReport(val leaks: List<Leak>)
            data class Leak(
                val className: String,
                val leakSize: Long,
                val stackTrace: String
            )
        }
    }
    
    // Configure LeakCanary
    class LeakCanaryConfig {
        
        fun configureLeakCanary() {
            // In Application class:
            /*
            if (BuildConfig.DEBUG) {
                LeakCanary.config = LeakCanary.config.copy(
                    retainedVisibleThreshold = 3,
                    dumpHeapThreshold = 1000 * 1024 * 1024,  // 1GB
                    maxStoredHeapDumps = 7,
                    requestWriteExternalStoragePermission = false
                )
            }
            */
        }
        
        // Custom filtering
        class LeakFilter {
            
            val ignoredClasses = listOf(
                "com.squareup.okhttp3.OkHttpClient",
                "retrofit2.Retrofit",
                "com.google.gson.Gson"
            )
            
            fun shouldIgnore(leakClass: String): Boolean {
                return ignoredClasses.any { leakClass.startsWith(it) }
            }
        }
    }
    
    // Manual heap dump trigger
    class ManualHeapDump {
        
        fun triggerHeapDump(context: android.content.Context) {
            // LeakCanary watches all activities automatically
            // To manually trigger:
            
            // 1. Use Android Profiler
            // 2. Or trigger via code:
            
            // Note: This is handled automatically by LeakCanary
            // Manual triggering is for special cases
            
            // Show current leak count
            // val leakCount = LeakCanary.leakCount
        }
    }
}
```

---

## EXAMPLE 2: MAT Integration

```kotlin
/**
 * Memory Analyzer Tool (MAT) Integration
 * 
 * Using MAT for deep memory analysis.
 */
class MATIntegration {
    
    // Export heap dump for MAT
    class HeapDumpExporter {
        
        fun exportForMAT(context: android.content.Context): java.io.File {
            val file = java.io.File(context.cacheDir, "heap_for_mat.hprof")
            android.os.Debug.dumpHprofData(file.absolutePath)
            return file
        }
        
        // Convert to standard format if needed
        fun convertToMATFormat(inputFile: java.io.File, outputFile: java.io.File) {
            // Use hprof-conv tool from Android SDK
            // hprof-conv input.hprof output.hprof
            val process = java.lang.Runtime.getRuntime().exec(
                arrayOf(
                    "${System.getenv("ANDROID_HOME")}/platform-tools/hprof-conv",
                    inputFile.absolutePath,
                    outputFile.absolutePath
                )
            )
            process.waitFor()
        }
    }
    
    // MAT query language (OQL) examples
    class OQLQueries {
        
        // Find all Activities
        val findActivities = "SELECT * FROM instanceof android.app.Activity"
        
        // Find large bitmaps
        val findLargeBitmaps = "SELECT * FROM android.graphics.Bitmap WHERE mHeight > 1000 OR mWidth > 1000"
        
        // Find String instances with specific pattern
        val findLargeStrings = "SELECT * FROM java.lang.String WHERE length > 1000"
        
        // Find View click listeners
        val findClickListeners = "SELECT * FROM android.view.View\$OnClickListener"
        
        // Find custom objects by class
        val findCustomObjects = "SELECT * FROM com.example.myapp.MyClass"
        
        // Find objects with large retained heap
        val findLargeRetained = "SELECT * FROM java.lang.Object WHERE @retainedHeapSize > 1024000"
        
        // Execute query
        fun executeQuery(heapDump: HeapDump, query: String): List<Any> {
            // Use MAT's OQL engine
            // return OQLEngine.execute(heapDump, query)
            return emptyList()
        }
    }
    
    // Path to GC Roots analysis
    class GCRootAnalysis {
        
        fun findGCRoots(heapDump: HeapDump, instance: com.android.tools.perflib.heap.Instance): List<GCRootPath> {
            val paths = mutableListOf<GCRootPath>()
            
            // Find all paths from instance to GC roots
            // This helps understand why object is retained
            
            // Common GC roots:
            // - Thread objects
            // - Stack local variables
            // - Static fields
            // - JNI references
            // - VM internal objects
            
            return paths
        }
        
        data class GCRootPath(
            val rootType: GCRootType,
            val path: List<String>
        )
        
        enum class GCRootType {
            THREAD, STACK_LOCAL, STATIC, JNI_LOCAL, JNI_GLOBAL, INTERNAL
        }
    }
    
    // Dominator tree analysis
    class DominatorAnalysis {
        
        fun findRetainedMemory(heapDump: HeapDump): Map<String, Long> {
            // Dominator tree shows which objects keep others in memory
            
            val retainedByClass = mutableMapOf<String, Long>()
            
            heapDump.allClasses.forEach { clazz ->
                val retained = clazz.instances.sumOf { it.retainedSize }
                if (retained > 1024 * 1024) {  // > 1MB
                    retainedByClass[clazz.name] = retained
                }
            }
            
            return retainedByClass.toList().sortedByDescending { it.second }.toMap()
        }
    }
}
```

---

## EXAMPLE 3: Memory Leak Investigation

```kotlin
/**
 * Complete Memory Leak Investigation
 * 
 * Step-by-step leak finding and fixing.
 */
class MemoryLeakInvestigation {
    
    // Investigation steps
    class InvestigationSteps {
        
        // Step 1: Identify symptoms
        fun identifySymptoms(): LeakSymptoms {
            return LeakSymptoms(
                oomErrors = true,
                increasingMemory = true,
                gcFrequency = "high",
                appRestart = true
            )
        }
        
        // Step 2: Reproduce leak
        fun reproduceLeak() {
            // Navigate through screens where leak occurs
            // Press back, rotate device, go through navigation
            // Multiple times to accumulate leak
        }
        
        // Step 3: Capture heap dump
        fun captureHeapDump(): java.io.File {
            // Use Android Profiler or code:
            val file = java.io.File(android.os.Environment.getExternalStorageDirectory(), "heap.hprof")
            android.os.Debug.dumpHprofData(file.absolutePath)
            return file
        }
        
        // Step 4: Analyze with LeakCanary
        fun analyzeWithLeakCanary(): LeakCanaryResult {
            // LeakCanary automatically detects and reports leaks
            // Check Logcat for leak warnings
            return LeakCanaryResult(
                leakClass = "com.example.MainActivity",
                leakSize = 1024 * 1024,
                stackTrace = """
                    com.example.MainActivity (0x12345678)
                    ├─ this$0 (android.app.Activity)
                    │  └─ static ActivityHelper.sContext (android.content.Context)
                    """.trimIndent()
            )
        }
        
        // Step 5: Fix the leak
        class LeakFix {
            
            // Before fix - leak
            class BeforeFix {
                class ActivityHelper {
                    // BAD: Static context reference
                    companion object {
                        var context: android.content.Context? = null
                    }
                }
                
                class MainActivity : android.app.Activity() {
                    override fun onCreate(savedInstanceState: android.os.Bundle?) {
                        super.onCreate(savedInstanceState)
                        ActivityHelper.context = this  // LEAK!
                    }
                }
            }
            
            // After fix - no leak
            class AfterFix {
                class ActivityHelper {
                    // GOOD: Use application context
                    companion object {
                        var context: android.content.Context? = null
                        
                        fun init(appContext: android.content.Context) {
                            context = appContext.applicationContext
                        }
                    }
                }
                
                class MainActivity : android.app.Activity() {
                    override fun onCreate(savedInstanceState: android.os.Bundle?) {
                        super.onCreate(savedInstanceState)
                        // No static reference to activity
                    }
                }
            }
        }
        
        // Step 6: Verify fix
        fun verifyFix() {
            // Run the same scenario
            // Check heap dump again
            // Confirm leak is fixed
        }
        
        data class LeakSymptoms(
            val oomErrors: Boolean,
            val increasingMemory: Boolean,
            val gcFrequency: String,
            val appRestart: Boolean
        )
        
        data class LeakCanaryResult(
            val leakClass: String,
            val leakSize: Long,
            val stackTrace: String
        )
    }
    
    // Common leak patterns and fixes
    class CommonLeakPatterns {
        
        // Pattern 1: Static Activity reference
        object StaticActivityFix {
            // Problem
            class BadExample {
                companion object { var activity: android.app.Activity? = null }
            }
            
            // Solution
            class GoodExample {
                companion object { 
                    var appContext: android.content.Context? = null
                    fun init(ctx: android.content.Context) {
                        appContext = ctx.applicationContext
                    }
                }
            }
        }
        
        // Pattern 2: Non-static inner class
        object InnerClassFix {
            // Problem - inner class holds outer reference
            class Bad {
                class Inner { val outer = this@Outer }
            }
            
            // Solution - use static inner class
            class Good {
                class StaticInner { /* no outer reference */ }
            }
        }
        
        // Pattern 3: Handler with delayed message
        object HandlerFix {
            // Problem - handler keeps activity reference
            class Bad(val activity: android.app.Activity) {
                val handler = android.os.Handler().apply {
                    postDelayed({ activity.onBackPressed() }, 1000)
                }
            }
            
            // Solution - use static handler with weak reference
            class Good : android.os.Handler.Callback {
                private val handler = android.os.Handler(android.os.Looper.getMainLooper(), this)
                private val activityRef: java.lang.ref.WeakReference<android.app.Activity>
                
                override fun handleMessage(msg: android.os.Message): Boolean {
                    activityRef.get()?.onBackPressed()
                    return true
                }
            }
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Heap Dump Analysis:**
- Instance count per class
- Retained heap per object
- Shallow heap size
- Reference paths to GC roots

**Memory Leak Indicators:**
- Activities not garbage collected
- Fragment instances retained
- Bitmaps not recycled
- Listeners not removed

**Heap Dump Tools:**
- Android Studio Profiler
- LeakCanary (automatic)
- MAT (detailed analysis)
- perflib (programmatic)

**Common Leak Causes:**
- Static context references
- Inner class references
- Unreleased listeners
- Handler delayed messages
- Thread references

---

## Advanced Tips

- **Tip 1: Use LeakCanary from start** - Automatic leak detection
- **Tip 2: Run MAT on real device** - Emulators have different behavior
- **Tip 3: Use Dominator Tree** - Find memory consumers quickly
- **Tip 4: Compare Heap Dumps** - Find what changed between states
- **Tip 5: Focus on Retained Size** - Shallow size misleading for impact

---

## Troubleshooting Guide (FAQ)

**Q: How do I find memory leaks?**
A: Use LeakCanary, capture heap dumps, compare with baseline

**Q: Why is retained heap larger than shallow?**
A: Retained includes all objects retained by this object

**Q: How do I fix Activity leak?**
A: Remove static references, use application context

**Q: What's the difference between shallow and retained?**
A: Shallow = object's own size, retained = total if GC'd

---

## Advanced Tips and Tricks

- **Tip 1: Use ref-watcher** - Manual leak tracking for custom objects
- **Tip 2: Profile in release** - Debug builds different than release
- **Tip 3: Check LeakCanary exclusions** - Ignore known safe leaks
- **Tip 4: Use native memory profiling** - For Bitmap leaks
- **Tip 5: Monitor with systrace** - For allocation timing

---

## CROSS-REFERENCES

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md
- See: 09_PERFORMANCE/02_Debugging_Tools/05_Performance_Metrics.md

---

## END OF MEMORY ANALYSIS GUIDE

(End of file - total 682 lines)