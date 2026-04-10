# Memory Management

## Learning Objectives

1. Understanding Android memory model and garbage collection
2. Identifying and preventing memory leaks
3. Implementing efficient memory usage patterns
4. Using profiling tools to analyze memory consumption
5. Optimizing memory allocation in Kotlin/Android applications

```kotlin
package com.kotlin.performance.memory
```

---

## Prerequisites

- See: 01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md
- See: 03_ARCHITECTURE/01_Architecture_Patterns/02_MVVM_Implementation.md
- See: 04_DATA_PERSISTENCE/01_Database_Development/01_Room_Database_Basics.md

---

## Core Concepts

### Android Memory Model

Android uses a managed memory environment with garbage collection, but developers must still be mindful of memory usage. The system has limited memory, and each app has an allocated heap size (varies by device).

### Key Memory Concepts

- **Heap**: Runtime memory area for objects
- **Stack**: Method execution and local variables
- **GC Roots**: Objects always retained (static fields, active threads)
- **Reference Types**: Strong, Soft, Weak, Phantom references
- **Memory Leaks**: Objects retained unintentionally

### SECTION 1: Understanding Memory Allocation

```kotlin
/**
 * Memory Allocation Basics
 * 
 * Understanding how memory is allocated in Android apps.
 */
class MemoryAllocation {
    
    // Stack allocation (primitives, references)
    fun stackVsHeap() {
        // Stack - primitive types and references
        val primitive: Int = 42  // Stored on stack
        val reference: String = "Hello"  // Reference on stack, object on heap
        
        // Heap - objects created with 'new'
        val user = User(1, "John")  // Entire object on heap
        
        // Arrays - always on heap in Kotlin
        val array = IntArray(1000)  // Heap allocation
    }
    
    // Object allocation patterns
    fun objectCreation() {
        // Factory pattern - controls object creation
        val instance = UserFactory.create(1, "John")
        
        // Object pooling - reuse objects
        val pool = ObjectPool<User>()
        val pooled = pool.acquire()
        pool.release(pooled)
    }
    
    data class User(val id: Int, val name: String)
}

/**
 * Factory pattern for controlled object creation.
 */
object UserFactory {
    private val cache = mutableMapOf<Int, User>()
    
    fun create(id: Int, name: String): User {
        return cache.getOrPut(id) { User(id, name) }
    }
}

/**
 * Object pool for reusing expensive objects.
 */
class ObjectPool<T> {
    private val pool = ArrayDeque<T>()
    private val maxSize = 10
    
    fun acquire(): T? = pool.pollFirst()
    
    fun release(obj: T) {
        if (pool.size < maxSize) {
            pool.addLast(obj)
        }
    }
}
```

---

## SECTION 2: Preventing Memory Leaks

```kotlin
/**
 * Memory Leaks Prevention
 * 
 * Common causes and solutions for memory leaks.
 */
class MemoryLeakPrevention {
    
    // Leak 1: Static context references
    object StaticLeakExample {
        // BAD: Static context reference
        var activity: android.app.Activity? = null
        
        // GOOD: Use application context
        var applicationContext: android.content.Context? = null
    }
    
    // Leak 2: Inner class holding outer class reference
    class OuterClass {
        // BAD: Non-static inner class retains outer reference
        inner class InnerClassBad {
            fun doSomething() {
                // Implicitly holds reference to OuterClass
            }
        }
        
        // GOOD: Static inner class
        class InnerClassGood {
            fun doSomething() {
                // No reference to OuterClass
            }
        }
    }
    
    // Leak 3: Listener not removed
    class ListenerLeakFix {
        interface ClickListener {
            fun onClick()
        }
        
        class ButtonManager {
            private var listener: ClickListener? = null
            
            fun setListener(listener: ClickListener) {
                this.listener = listener
            }
            
            // CRITICAL: Remove listener when done
            fun removeListener() {
                listener = null
            }
        }
    }
    
    // Leak 4: Handler with delayed messages
    class HandlerLeakFix {
        class MyActivity : android.app.Activity() {
            private val handler = android.os.Handler()
            
            // Use weak reference
            private val handlerCallback = object : android.os.Handler.Callback {
                override fun handleMessage(msg: android.os.Message): Boolean {
                    // Handle message safely
                    return true
                }
            }
            
            override fun onDestroy() {
                super.onDestroy()
                // Remove all callbacks and messages
                handler.removeCallbacksAndMessages(null)
            }
        }
    }
    
    // Leak 5: Thread not completed
    class ThreadLeakFix {
        class MyActivity : android.app.Activity() {
            private var thread: Thread? = null
            
            fun startWork() {
                thread = Thread {
                    // Work
                    doWork()
                }.apply { start() }
            }
            
            override fun onDestroy() {
                super.onDestroy()
                // Interrupt and wait
                thread?.interrupt()
                thread = null
            }
            
            private fun doWork() {}
        }
    }
}
```

---

## SECTION 3: Efficient Collections

```kotlin
/**
 * Efficient Collections
 * 
 * Choosing the right collection type for memory efficiency.
 */
class EfficientCollections {
    
    // ArrayList vs LinkedList
    fun listComparison() {
        // ArrayList - better for random access, worse for insertions
        val arrayList = ArrayList<User>(100)
        arrayList[0]  // O(1) access
        arrayList.add(0, User(1, "John"))  // O(n) insertion
        
        // LinkedList - better for insertions, worse for random access
        val linkedList = LinkedList<User>()
        linkedList[0]  // O(n) access
        linkedList.addFirst(User(1, "John"))  // O(1) insertion
    }
    
    // Use appropriate collection size
    fun collectionSizing() {
        // Specify initial capacity to avoid resizing
        val list = ArrayList<User>(1000)  // Known size
        
        // Use primitive collections when possible
        val intList = IntArray(100)  // vs ArrayList<Int>
    }
    
    // Map implementations
    fun mapComparison() {
        // HashMap - O(1) average, no order
        val hashMap = HashMap<Int, User>()
        
        // LinkedHashMap - O(1) average, insertion order
        val linkedHashMap = LinkedHashMap<Int, User>()
        
        // TreeMap - O(log n), sorted keys
        val treeMap = TreeMap<Int, User>()
        
        // SparseArray - replaces Integer keys, memory efficient
        val sparseArray = android.util.SparseArray<User>()
    }
    
    // Memory-efficient alternatives
    fun memoryAlternatives() {
        // StringBuilder for string concatenation
        val builder = StringBuilder()
        for (i in 1..100) {
            builder.append(i).append(",")
        }
        val result = builder.toString()
    }
    
    data class User(val id: Int, val name: String)
}
```

---

## Best Practices

1. **Use Application Context**: Always use application context instead of activity context when possible to prevent leaks
2. **Remove Listeners**: Always remove listeners in onDestroy/onDestroyView lifecycle methods
3. **Clear Handler Messages**: Remove all handler callbacks and messages in cleanup
4. **Specify Collection Size**: Initialize collections with known size to avoid resizing
5. **Use Primitives**: Prefer primitive types (Int, Long, Boolean) over boxed types (Integer, Long, Boolean)
6. **Close Resources**: Always close streams, cursors, and database connections
7. **Avoid Static Activity References**: Never store activity references in static fields
8. **Use WeakReferences**: Use weak references for caches and callbacks that need to be GC'd
9. **Release Custom Views**: Properly recycle and release custom view resources
10. **Monitor Leak Canaries**: Use LeakCanary library for detecting memory leaks in development

---

## Common Pitfalls and Solutions

### Pitfall 1: Activity Context Leak
- **Problem**: Storing activity context in static field
- **Solution**: Use application context or weak reference

### Pitfall 2: Inner Class Memory Leak
- **Problem**: Non-static inner class holds outer class reference
- **Solution**: Use static inner class or separate class

### Pitfall 3: Handler Memory Leak
- **Problem**: Handler with delayed message holds activity reference
- **Solution**: Remove callbacks in onDestroy, use static handler

### Pitfall 4: Listener Leak
- **Problem**: Listener not removed when view destroyed
- **Solution**: Remove listener in onDestroyView

### Pitfall 5: Thread Leak
- **Problem**: Thread running after component destroyed
- **Solution**: Interrupt thread in onDestroy

### Pitfall 6: Cursor Not Closed
- **Problem**: Database cursor not closed
- **Solution**: Use try-with-resources or close in finally

### Pitfall 7: Bitmap Not Recycled
- **Problem**: Large bitmap consuming memory
- **Solution**: Call bitmap.recycle() when no longer needed

---

## Troubleshooting Guide

### Issue: OutOfMemoryError
- **Steps**: 1. Identify leak location 2. Check bitmap sizes 3. Review collection usage
- **Tools**: Android Profiler, LeakCanary, MAT

### Issue: High Memory Usage
- **Steps**: 1. Check for memory leaks 2. Reduce bitmap sizes 3. Use memory-efficient collections
- **Tools**: Android Profiler heap dump

### Issue: GC Running Frequently
- **Steps**: 1. Check object allocation rate 2. Use object pooling 3. Reduce allocations in loops
- **Tools**: Android Profiler allocation tracking

---

## EXAMPLE 1: ViewModel with Proper Lifecycle

```kotlin
/**
 * ViewModel with Proper Memory Management
 * 
 * Implementing memory-safe ViewModel pattern.
 */
class ProperMemoryManagement {
    
    // ViewModel with repository pattern
    class UserViewModel(
        private val repository: UserRepository
    ) : androidx.lifecycle.ViewModel() {
        
        private val _users = androidx.lifecycle.MutableLiveData<List<User>>()
        val users: androidx.lifecycle.LiveData<List<User>> = _users
        
        private val _selectedUser = androidx.lifecycle.MutableLiveData<User?>()
        val selectedUser: androidx.lifecycle.LiveData<User?> = _selectedUser
        
        fun loadUsers() {
            viewModelScope.launch {
                _users.value = repository.getUsers()
            }
        }
        
        fun selectUser(user: User) {
            _selectedUser.value = user
        }
        
        // CRITICAL: Clear data when ViewModel is cleared
        override fun onCleared() {
            super.onCleared()
            _users.value = emptyList()
            _selectedUser.value = null
        }
    }
    
    // Repository with proper resource management
    class UserRepository(private val dao: UserDao) {
        
        suspend fun getUsers(): List<User> {
            return dao.getAllUsers()
        }
        
        suspend fun insertUser(user: User) {
            dao.insertUser(user)
        }
    }
    
    // Fragment with proper cleanup
    class UserFragment : android.app.Fragment() {
        
        private var viewModel: UserViewModel? = null
        
        fun setViewModel(viewModel: UserViewModel) {
            this.viewModel = viewModel
        }
        
        // CRITICAL: Clear references in onDestroyView
        override fun onDestroyView() {
            super.onDestroyView()
            viewModel = null
        }
    }
    
    data class User(val id: Int, val name: String)
    
    interface UserDao {
        suspend fun getAllUsers(): List<User>
        suspend fun insertUser(user: User)
    }
}
```

---

## EXAMPLE 2: Image Loading with Memory Management

```kotlin
/**
 * Image Loading with Memory Management
 * 
 * Efficient image loading with proper memory handling.
 */
class ImageLoadingExample {
    
    // Image loader with caching
    class ImageLoader(private val context: android.content.Context) {
        
        private val memoryCache: android.util.LruCache<String, android.graphics.Bitmap>
        private val diskCache: android.util.LruCache<String, android.graphics.Bitmap>
        
        init {
            // Calculate max memory cache size (1/8 of available memory)
            val maxMemory = (android.os.Runtime.getRuntime().maxMemory() / 1024).toInt()
            val cacheSize = maxMemory / 8
            
            memoryCache = object : android.util.LruCache<String, android.graphics.Bitmap>(cacheSize) {
                override fun sizeOf(key: String, bitmap: android.graphics.Bitmap): Int {
                    return bitmap.byteCount / 1024
                }
                
                override fun entryRemoved(
                    evicted: Boolean,
                    key: String,
                    oldValue: android.graphics.Bitmap,
                    newValue: android.graphics.Bitmap?
                ) {
                    // Don't recycle here - let system handle GC
                }
            }
            
            // Disk cache (simplified)
            diskCache = android.util.LruCache(100)
        }
        
        fun loadImage(url: String, callback: (android.graphics.Bitmap) -> Unit) {
            // Check memory cache first
            memoryCache.get(url)?.let {
                callback(it)
                return
            }
            
            // Load from disk or network
            viewModelScope.launch {
                val bitmap = loadBitmap(url)
                bitmap?.let {
                    memoryCache.put(url, it)
                    callback(it)
                }
            }
        }
        
        private suspend fun loadBitmap(url: String): android.graphics.Bitmap? {
            // Implementation would load from network
            return null
        }
        
        // Clear caches when memory is low
        fun onLowMemory() {
            memoryCache.evictAll()
        }
        
        // Trim cache based on memory level
        fun trimToSize(maxSize: Int) {
            memoryCache.trimToSize(maxSize)
        }
    }
    
    // Compose image with proper memory handling
    class ComposeImageLoader {
        
        @androidx.compose.runtime.Composable
        fun ImageWithMemory(
            url: String,
            contentDescription: String?
        ) {
            var bitmap by androidx.compose.runtime.remember { mutableStateOf<android.graphics.Bitmap?>(null) }
            
            androidx.compose.runtime.LaunchedEffect(url) {
                bitmap = loadImage(url)
            }
            
            bitmap?.let {
                androidx.compose.foundation.Image(
                    bitmap = it,
                    contentDescription = contentDescription,
                    contentScale = androidx.compose.ui.layout.ContentScale.Crop
                )
            }
        }
        
        private suspend fun loadImage(url: String): android.graphics.Bitmap? {
            // Load image
            return null
        }
    }
    
    // Handle bitmap recycling
    class BitmapManager {
        
        private val activeBits = android.util.ConcurrentHashMap<String, android.graphics.Bitmap>()
        private val recycleQueue = java.util.LinkedList<android.graphics.Bitmap>()
        
        fun registerBitmap(id: String, bitmap: android.graphics.Bitmap) {
            activeBits[id] = bitmap
        }
        
        fun unregisterBitmap(id: String) {
            activeBits.remove(id)?.let {
                recycleQueue.add(it)
                
                // Recycle old bitmaps when queue gets too large
                while (recycleQueue.size > 10) {
                    recycleQueue.pollFirst()?.recycle()
                }
            }
        }
        
        fun onLowMemory() {
            // Recycle all queued bitmaps
            recycleQueue.forEach { it.recycle() }
            recycleQueue.clear()
        }
    }
}
```

---

## EXAMPLE 3: Database with Cursor Management

```kotlin
/**
 * Database with Proper Cursor Management
 * 
 * Managing database resources properly.
 */
class DatabaseCursorManagement {
    
    // DAO with proper resource management
    class UserDao(private val db: AppDatabase) {
        
        // Use suspend function (auto-closes cursor)
        suspend fun getAllUsers(): List<User> {
            return withContext(Dispatchers.IO) {
                db.query("SELECT * FROM users").use { cursor ->
                    cursorToList(cursor)
                }
            }
        }
        
        suspend fun getUserById(id: Int): User? {
            return withContext(Dispatchers.IO) {
                db.query("SELECT * FROM users WHERE id = ?", arrayOf(id)).use { cursor ->
                    if (cursor.moveToFirst()) {
                        cursorToUser(cursor)
                    } else {
                        null
                    }
                }
            }
        }
        
        // Flow for reactive queries (Room handles cursor)
        fun getUsersFlow(): Flow<List<User>> {
            return db.userDao().getAllUsersFlow()
        }
        
        private fun cursorToList(cursor: android.database.Cursor): List<User> {
            val users = mutableListOf<User>()
            while (cursor.moveToNext()) {
                users.add(cursorToUser(cursor))
            }
            return users
        }
        
        private fun cursorToUser(cursor: android.database.Cursor): User {
            return User(
                id = cursor.getInt(cursor.getColumnIndexOrThrow("id")),
                name = cursor.getString(cursor.getColumnIndexOrThrow("name")),
                email = cursor.getString(cursor.getColumnIndexOrThrow("email"))
            )
        }
    }
    
    // Repository with proper resource management
    class UserRepository(private val dao: UserDao) {
        
        suspend fun getUsers(): List<User> = dao.getAllUsers()
        
        suspend fun getUser(id: Int): User? = dao.getUserById(id)
        
        fun getUsersFlow(): Flow<List<User>> = dao.getUsersFlow()
    }
    
    // Custom cursor wrapper for proper closing
    class ManagedCursor(private val cursor: android.database.Cursor) : android.database.Cursor by cursor {
        
        fun use<T>(block: (android.database.Cursor) -> T): T {
            return try {
                block(cursor)
            } finally {
                close()
            }
        }
        
        override fun close() {
            if (!isClosed) {
                super.close()
            }
        }
    }
    
    // Activity with proper cursor handling
    class UserListActivity : android.app.Activity() {
        
        private lateinit var cursorLoader: android.content.CursorLoader
        private var cursor: android.database.Cursor? = null
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            cursorLoader = android.content.CursorLoader(
                this,
                android.provider ContactsContract.CommonDataKinds.Phone.CONTENT_URI,
                null, null, null, null
            )
        }
        
        override fun onResume() {
            super.onResume()
            cursor = cursorLoader.loadInBackground()
            displayUsers()
        }
        
        // CRITICAL: Close cursor in onPause or onDestroy
        override fun onPause() {
            super.onPause()
            cursor?.close()
            cursor = null
        }
        
        private fun displayUsers() {
            cursor?.let {
                while (it.moveToNext()) {
                    // Display user
                }
            }
        }
    }
    
    data class User(val id: Int, val name: String, val email: String)
    
    interface AppDatabase {
        fun query(sql: String, args: Array<String>? = null): android.database.Cursor
        fun userDao(): UserDao
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Memory Allocation:**
- Stack: Primitive types, references
- Heap: Objects, arrays
- LruCache: Automatic size management

**Memory Leak Causes:**
- Static context references
- Inner class holds outer reference
- Unremoved listeners
- Handler delayed messages
- Uncompleted threads

**Collection Efficiency:**
- ArrayList: Random access O(1)
- LinkedList: Insertion O(1)
- SparseArray: Integer keys
- Primitive arrays: Less memory

**Best Collection Choice:**
- Unknown size, random access: ArrayList
- Known size, random access: ArrayList(size)
- Frequent insertions: LinkedList
- Key-value, int keys: SparseArray
- String keys: HashMap

**Cache Sizes:**
- Memory cache: 1/8 of max memory
- Disk cache: Depends on device
- LruCache: Automatic eviction

---

## Advanced Tips

- **Tip 1: Use Profile GPU Rendering** - Monitor per-frame rendering time
- **Tip 2: Enable LargeHeap** - Request more memory in manifest (use sparingly)
- **Tip 3: Use R8** - Enable code shrinking for smaller APK
- **Tip 4: ProGuard rules** - Keep data classes for reflection
- **Tip 5: Use bitmapFactory.Options** - Set inSampleSize for thumbnails

---

## Troubleshooting Guide (FAQ)

**Q: How do I find memory leaks?**
A: Use LeakCanary library for automatic leak detection during development.

**Q: How much memory can my app use?**
A: Check ActivityManager.getMemoryClass() for heap limit.

**Q: When should I recycle bitmaps?**
A: Recycle bitmaps when navigating away from image-heavy screens.

**Q: How do I reduce memory usage in lists?**
A: Use RecyclerView with ViewHolder pattern, load images on demand.

**Q: What causes OutOfMemoryError with images?**
A: Loading images at full resolution; use inSampleSize to load smaller versions.

---

## Advanced Tips and Tricks

- **Tip 1: Monitor with Android Profiler** - Track memory allocations in real-time
- **Tip 2: Use Allocation Tracking** - Find allocation hot spots
- **Tip 3: Heap Dump Analysis** - Use MAT for detailed analysis
- **Tip 4: Reference Queue** - Track GC'd weak references
- **Tip 5: Use BitmapPool** - Reuse bitmap memory allocations

---

## CROSS-REFERENCES

- See: 09_PERFORMANCE/01_Performance_Optimization/02_Battery_Optimization.md
- See: 09_PERFORMANCE/02_Debugging_Tools/02_Memory_Analysis.md
- See: 03_ARCHITECTURE/01_Architecture_Patterns/02_MVVM_Implementation.md

---

## END OF MEMORY MANAGEMENT GUIDE

(End of file - total 679 lines)