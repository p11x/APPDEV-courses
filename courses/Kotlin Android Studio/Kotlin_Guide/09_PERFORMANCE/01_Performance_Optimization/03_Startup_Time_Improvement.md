# Startup Time Improvement

## Learning Objectives

1. Understanding Android app startup process
2. Identifying startup bottlenecks
3. Implementing app startup optimization techniques
4. Using profiling tools for startup analysis
5. Applying best practices for fast app launch

```kotlin
package com.kotlin.performance.startup
```

---

## Prerequisites

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 03_ARCHITECTURE/01_Architecture_Patterns/02_MVVM_Implementation.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md

---

## Core Concepts

### Android Startup Process

- **Cold Start**: App starts from scratch (highest time)
- **Warm Start**: App in memory, needs recreate
- **Hot Start**: App in foreground, fastest start

### Startup Phases

- **Application.onCreate()**: App initialization
- **Activity.onCreate()**: Activity setup
- **Activity.onResume()**: UI ready

### SECTION 1: Application Class Optimization

```kotlin
/**
 * Application Class Optimization
 * 
 * Optimizing startup in Application class.
 */
class ApplicationOptimization {
    
    // BAD: Heavy work in Application.onCreate()
    class BadApplication : android.app.Application() {
        
        override fun onCreate() {
            super.onCreate()
            
            // BAD: Heavy initialization blocks startup
            initializeAnalytics()
            loadPreferences()
            setupDatabase()
            initializeNetwork()
            setupCrashReporting()
        }
        
        private fun initializeAnalytics() {}
        private fun loadPreferences() {}
        private fun setupDatabase() {}
        private fun initializeNetwork() {}
        private fun setupCrashReporting() {}
    }
    
    // GOOD: Deferred initialization
    class OptimizedApplication : android.app.Application() {
        
        override fun onCreate() {
            super.onCreate()
            
            // Initialize only what's absolutely needed
            initialize_core()
            
            // Defer everything else
            android.os.Handler(android.os.Looper.getMainLooper()).postDelayed({
                initialize_delayed()
            }, 2000)  // Defer by 2 seconds
        }
        
        private fun initialize_core() {
            // Essential initialization only
            // e.g., dependency injection setup, crash reporting
        }
        
        private fun initialize_delayed() {
            // Non-essential initialization
            initializeAnalytics()
            loadPreferences()
            setupDatabase()
            initializeNetwork()
        }
        
        private fun initializeAnalytics() {}
        private fun loadPreferences() {}
        private fun setupDatabase() {}
        private fun initializeNetwork() {}
    }
    
    // Using AppStartup for automatic deferred init
    class StartupInitializer {
        
        fun setupAppStartup(): androidx.startup.AppInitializer {
            return androidx.startup.AppInitializer.getInstance(this)
        }
        
        fun registerInitializers(): androidx.startup.AppInitializer {
            val appStartup = setupAppStartup()
            
            // Register initializers with dependencies
            appStartup.registerInitializer(DependencyLocator::class.java) {}
            
            return appStartup
        }
    }
}
```

---

## SECTION 2: Activity Startup Optimization

```kotlin
/**
 * Activity Startup Optimization
 * 
 * Optimizing Activity creation and display.
 */
class ActivityStartupOptimization {
    
    // Lazy view initialization
    class LazyInitActivity : android.app.Activity() {
        
        // Views initialized lazily
        private val textView: android.widget.TextView by lazy {
            findViewById(android.R.id.text1)
        }
        
        private val button: android.widget.Button by lazy {
            findViewById(android.R.id.button1)
        }
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(android.R.layout.activity_list)
            
            // Views not accessed yet - no inflation until used
        }
        
        fun onButtonClick() {
            // First access triggers inflation
            button.text = "Clicked"
        }
    }
    
    // ViewStub for deferred layout loading
    class ViewStubActivity : android.app.Activity() {
        
        private lateinit var optionalLayout: android.view.ViewStub
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(android.R.layout.activity_main)
            
            // Get ViewStub - doesn't inflate until needed
            optionalLayout = findViewById(android.R.id.stub)
        }
        
        fun showOptionalLayout() {
            // Inflate only when needed
            val inflated = optionalLayout.inflate()
            // Setup inflated view
        }
    }
    
    // Set content view with delay - show splash first
    class SplashFirstActivity : android.app.Activity() {
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            // Show splash immediately
            setContentView(android.R.layout.splash)
            
            // Defer main layout
            window.decorView.post {
                setContentView(android.R.layout.main)
                setupMainContent()
            }
        }
        
        private fun setupMainContent() {
            // Setup main content
        }
    }
    
    // Optimized theme and window
    class FastThemeActivity : android.app.Activity() {
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            // Set theme before super.onCreate() to avoid flicker
            setTheme(android.R.style.Theme_Material_Light)
            
            super.onCreate(savedInstanceState)
            setContentView(android.R.layout.activity_main)
        }
    }
}
```

---

## SECTION 3: Deferrable Work

```kotlin
/**
 * Deferrable Work
 * 
 * Using coroutines and handlers to defer non-critical work.
 */
class DeferrableWork {
    
    // Using coroutines to defer work
    class CoroutineDeferral : androidx.lifecycle.ViewModel() {
        
        private val viewModelScope = androidx.lifecycle.viewModelScope
        
        fun deferNonCriticalWork() {
            viewModelScope.launch {
                // Don't block - use Dispatchers.IO
                withContext(Dispatchers.IO) {
                    doHeavyInitialization()
                }
            }
        }
        
        private suspend fun doHeavyInitialization() {
            delay(1000)  // Simulate work
        }
    }
    
    // Using postDelayed to defer
    class HandlerDeferral(private val activity: android.app.Activity) {
        
        private val handler = android.os.Handler(android.os.Looper.getMainLooper())
        
        fun deferWork(delayMs: Long = 1000L) {
            handler.postDelayed({
                performDeferredInitialization()
            }, delayMs)
        }
        
        private fun performDeferredInitialization() {
            if (!activity.isFinishing) {
                // Safe to execute
            }
        }
        
        fun cancelDeferredWork() {
            handler.removeCallbacksAndMessages(null)
        }
    }
    
    // Using Choreographer for frame-perfect deferral
    class ChoreographerDeferral(private val activity: android.app.Activity) {
        
        fun deferUntilNextFrame(task: Runnable) {
            android.view.Choreographer.getInstance().postFrameCallback {
                if (!activity.isFinishing) {
                    task.run()
                }
            }
        }
    }
    
    // On-demand initialization
    class OnDemandInit private constructor() {
        
        companion object {
            @Volatile
            private var instance: OnDemandInit? = null
            
            fun getInstance(): OnDemandInit {
                return instance ?: synchronized(this) {
                    instance ?: OnDemandInit().also { instance = it }
                }
            }
        }
        
        private val initializedFeatures = mutableSetOf<String>()
        
        fun initializeFeature(name: String, init: () -> Unit) {
            if (name !in initializedFeatures) {
                synchronized(initializedFeatures) {
                    if (name !in initializedFeatures) {
                        init()
                        initializedFeatures.add(name)
                    }
                }
            }
        }
        
        fun isInitialized(name: String): Boolean = name in initializedFeatures
    }
}
```

---

## Best Practices

1. **Minimize onCreate() Work**: Move non-critical initialization to deferred tasks
2. **Use Lazy Initialization**: Initialize objects only when needed
3. **Show Splash Quickly**: Set content view immediately, defer rest
4. **Set Theme Early**: Set theme before super.onCreate() to avoid flicker
5. **Use Include Tags**: Split complex layouts into reusable components
6. **Optimize View Inflation**: Use ViewStub for rarely-used views
7. **Avoid Synchronous I/O**: Use coroutines for any I/O in startup path
8. **Profile Startup**: Use Android Profiler to identify bottlenecks
9. **Use ProGuard**: Enable R8 for smaller dex files, faster loading
10. **Avoid OnCreate Overrides**: Use AppStartup library for initialization

---

## Common Pitfalls and Solutions

### Pitfall 1: Heavy Application.onCreate()
- **Problem**: App hangs during launch
- **Solution**: Defer all non-essential initialization

### Pitfall 2: Synchronous SharedPreferences
- **Problem**: I/O blocks main thread
- **Solution**: Use commitAsync() or coroutines

### Pitfall 3: Database in onCreate()
- **Problem**: SQLite blocks startup
- **Solution**: Use Room with AsyncQuery or defer

### Pitfall 4: Network in onCreate()
- **Problem**: Network call blocks startup
- **Solution**: Show UI first, fetch data after

### Pitfall 5: Large Layout Inflation
- **Problem**: Layout inflation slows startup
- **Solution**: Use ViewStub, lazy inflate

### Pitfall 6: Theme Change Flicker
- **Problem**: Theme applied after inflate
- **Solution**: Set theme before super.onCreate()

---

## Troubleshooting Guide

### Issue: App Takes Long to Display
- **Steps**: 1. Profile with Android Profiler 2. Find onCreate bottlenecks 3. Defer non-essential work

### Issue: Cold Start Time Too High
- **Steps**: 1. Minimize Application.onCreate() 2. Avoid heavy I/O 3. Use App Startup library

---

## EXAMPLE 1: Optimized App Startup

```kotlin
/**
 * Complete Startup Optimization Example
 * 
 * Production-ready startup pattern.
 */
class OptimizedAppStartup {
    
    // Application with deferred initialization
    class OptimizedApp : android.app.Application() {
        
        private lateinit var appStartup: androidx.startup.AppInitializer
        
        override fun onCreate() {
            super.onCreate()
            
            // Critical: Show splash immediately
            // (This happens in Activity, not here)
            
            // Minimal essential initialization
            setupCrashReporting()
            setupDependencyInjection()
            
            // Schedule deferred initialization
            scheduleDeferredInit()
        }
        
        private fun setupCrashReporting() {
            // Critical crash reporting only
        }
        
        private fun setupDependencyInjection() {
            // Essential DI setup only
        }
        
        private fun scheduleDeferredInit() {
            val handler = android.os.Handler(android.os.Looper.getMainLooper())
            handler.postDelayed({
                initializeNonCritical()
            }, 2000)
        }
        
        private fun initializeNonCritical() {
            // Analytics, crash reporting details
            // Feature flags
            // Network client initialization
        }
    }
    
    // Main Activity with fast boot
    class FastBootActivity : android.app.Activity() {
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            // Show UI immediately
            setContentView(android.R.layout.activity_main)
            
            // Load user data asynchronously
            loadUserData()
        }
        
        private fun loadUserData() {
            androidx.lifecycle.viewModelScope.launch {
                val userData = withContext(Dispatchers.IO) {
                    fetchUserData()
                }
                updateUI(userData)
            }
        }
        
        private suspend fun fetchUserData(): UserData? {
            delay(500)
            return UserData("John", "john@example.com")
        }
        
        private fun updateUI(userData: UserData) {
            // Update UI
        }
    }
    
    // Splash Activity for immediate display
    class SplashActivity : android.app.Activity() {
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(android.R.layout.splash)
            
            // Move to main immediately
            startActivity(android.content.Intent(this, MainActivity::class.java))
            finish()
        }
    }
    
    data class UserData(val name: String, val email: String)
}
```

---

## EXAMPLE 2: App Startup Library Usage

```kotlin
/**
 * AndroidX App Startup Library
 * 
 * Using App Startup for automatic deferred initialization.
 */
class AppStartupLibrary {
    
    // Custom Initializer
    class AnalyticsInitializer : androidx.startup.Initializer<Analytics> {
        
        override fun create(context: android.content.Context): Analytics {
            return Analytics().apply {
                initialize()
            }
        }
        
        override fun dependencies(): List<kotlin.reflect.KClass<*>> {
            return emptyList()  // No dependencies
        }
    }
    
    class NetworkInitializer : androidx.startup.Initializer<NetworkClient> {
        
        override fun create(context: android.content.Context): NetworkClient {
            return NetworkClient()
        }
        
        override fun dependencies(): List<kotlin.reflect.KClass<*>> {
            return listOf(AnalyticsInitializer::class)  // Depends on Analytics
        }
    }
    
    class CacheInitializer : androidx.startup.Initializer<CacheManager> {
        
        override fun create(context: android.content.Context): CacheManager {
            return CacheManager(context)
        }
        
        override fun dependencies(): List<kotlin.reflect.KClass<*>> {
            return emptyList()
        }
    }
    
    // Simple classes
    class Analytics {
        fun initialize() {}
    }
    
    class NetworkClient {
        fun connect() {}
    }
    
    class CacheManager(context: android.content.Context) {}
    
    // XML configuration (in AndroidManifest.xml)
    /*
    <provider
        android:name="androidx.startup.InitializationProvider"
        android:authorities="${applicationId}.androidx-startup"
        android:exported="false"
        tools:node="merge">
        <meta-data
            android:name="com.example.AnalyticsInitializer"
            android:value="androidx.startup" />
        <meta-data
            android:name="com.example.NetworkInitializer"
            android:value="androidx.startup" />
    </provider>
    */
    
    // Manual initialization (when App Startup not needed)
    class ManualStartup(private val context: android.content.Context) {
        
        private var analytics: Analytics? = null
        private var networkClient: NetworkClient? = null
        private var cacheManager: CacheManager? = null
        private var initialized = false
        
        fun initialize() {
            if (initialized) return
            initialized = true
            
            analytics = Analytics()
            networkClient = NetworkClient()
            cacheManager = CacheManager(context)
            
            // Wire up dependencies
            networkClient?.let {
                analytics?.let (analyticsDependency -> 
                    // Setup dependency
                }
            }
        }
        
        fun getAnalytics(): Analytics = analytics!!
        fun getNetworkClient(): NetworkClient = networkClient!!
        fun getCacheManager(): CacheManager = cacheManager!!
    }
}
```

---

## EXAMPLE 3: Compose Startup Optimization

```kotlin
/**
 * Jetpack Compose Startup Optimization
 * 
 * Optimizing Compose app startup.
 */
class ComposeStartupOptimization {
    
    // Compose Application
    class ComposeApp : androidx.compose.runtime.Experimental Compose {
        
        @Composable
        fun App() {
            var isLoading by androidx.compose.runtime.remember { mutableStateOf(true) }
            var data by androidx.compose.runtime.remember { mutableStateOf<Data?>(null) }
            
            LaunchedEffect(Unit) {
                data = loadData()
                isLoading = false
            }
            
            if (isLoading) {
                SplashScreen()
            } else {
                MainScreen(data!!)
            }
        }
        
        private suspend fun loadData(): Data {
            delay(500)  // Simulate loading
            return Data("content")
        }
    }
    
    // Optimized Compose Theme
    class OptimizedCompose {
        
        @Composable
        fun FastTheme(
            darkTheme: Boolean = false,
            content: @Composable () -> Unit
        ) {
            // Set up theme without blocking
            androidx.compose.material3.MaterialTheme(
                colorScheme = if (darkTheme) darkColors() else lightColors(),
                typography = typography
            ) {
                content()
            }
        }
        
        private fun lightColors() = androidx.compose.material3.lightColorScheme()
        private fun darkColors() = androidx.compose.material3.darkColorScheme()
        private val typography = androidx.compose.material3.Typography()
    }
    
    // Remember optimized for startup
    class OptimizedRemember {
        
        @Composable
        fun rememberOptimized() {
            // Use remember for computed values
            val computedValue = androidx.compose.runtime.remember(key1) {
                // Expensive computation
                doExpensiveComputation()
            }
            
            // Use rememberSaveable for state that survives config changes
            var counter by androidx.compose.runtime.rememberSaveable {
                mutableStateOf(0)
            }
        }
        
        private fun doExpensiveComputation(): String = "computed"
    }
    
    // Skeleton loading screen
    class SkeletonLoading {
        
        @Composable
        fun UserListSkeleton(
            isLoading: Boolean,
            content: @Composable () -> Unit
        ) {
            if (isLoading) {
                androidx.compose.foundation.layout.Column {
                    repeat(5) {
                        SkeletonItem()
                    }
                }
            } else {
                content()
            }
        }
        
        @Composable
        private fun SkeletonItem() {
            androidx.compose.foundation.layout.Row(
                modifier = androidx.compose.ui.Modifier.padding(16.dp),
                verticalAlignment = androidx.compose.ui.Alignment.CenterVertically
            ) {
                androidx.compose.material3.Icon(
                    imageVector = androidx.compose.material.Icons.Default.Person,
                    contentDescription = null,
                    modifier = Modifier.size(48.dp)
                )
                androidx.compose.foundation.layout.Spacer(
                    modifier = Modifier.width(16.dp)
                )
                Column {
                    Box(
                        modifier = Modifier
                            .fillMaxWidth(0.6f)
                            .height(16.dp)
                            .background(androidx.compose.foundation.shape.RoundedCornerShape(4.dp))
                    )
                    Spacer(modifier = Modifier.height(8.dp))
                    Box(
                        modifier = Modifier
                            .fillMaxWidth(0.3f)
                            .height(12.dp)
                            .background(androidx.compose.foundation.shape.RoundedCornerShape(4.dp))
                    )
                }
            }
        }
    }
    
    data class Data(val content: String)
}
```

---

## OUTPUT STATEMENT RESULTS

**Startup Optimization Techniques:**
- Minimize Application.onCreate()
- Use deferred initialization
- Show UI immediately
- Set theme before super.onCreate()
- Use ViewStub for optional layouts
- Use coroutines for background work
- Avoid synchronous I/O

**Cold Start vs Warm Start:**
- Cold Start: Full initialization
- Warm Start: Partial recreation
- Hot Start: Fastest, in memory

**App Startup Library:**
- Automatic initialization
- Dependency graph resolution
- Automatic parallelization
- Manual override available

**AndroidX Startup Providers:**
- AppCompatVerify
- WorkManagerInitializer
- ProcessLifecycleOwner

**Startup Trace Points:**
- Application.onCreate()
- Activity.onCreate()
- Activity.onResume()
- First frame displayed

---

## Advanced Tips

- **Tip 1: Use Baseline Profiles** - Add baseline profiles for faster startup
- **Tip 2: Enable R8** - Minify dex for faster class loading
- **Tip 3: Use Splash Screen API** - Android 12+ splash screen
- **Tip 4: Preload Data** - Cache data for faster restarts
- **Tip 5: Lazy View Binding** - Use view binding late

---

## Troubleshooting Guide (FAQ)

**Q: How do I measure startup time?**
A: Use "adb shell am start -W -n package/.Activity" and check time_to_first_byte

**Q: What blocks startup most?**
A: Usually onCreate() heavy work, synchronous I/O, database access

**Q: Should I use splash screen?**
A: Yes, but keep it minimal - use Android 12+ SplashScreen API

**Q: How do I find startup bottlenecks?**
A: Use Android Profiler CPU profiler, systrace, or perfetto

---

## Advanced Tips and Tricks

- **Tip 1: Use systrace** - Analyze startup traces in detail
- **Tip 2: Check handleMessage** - Monitor main thread messages
- **Tip 3: Optimize inflate** - Use RecyclerView for lists
- **Tip 4: Lazy load resources** - Defer resource loading
- **Tip 5: Pre-dex** - Precompile critical paths

---

## CROSS-REFERENCES

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 09_PERFORMANCE/01_Performance_Optimization/02_Battery_Optimization.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md

---

## END OF STARTUP TIME IMPROVEMENT GUIDE

(End of file - total 682 lines)