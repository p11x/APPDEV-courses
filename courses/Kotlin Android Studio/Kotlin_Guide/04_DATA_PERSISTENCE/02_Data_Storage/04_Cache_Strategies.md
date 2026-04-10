# Cache Strategies

## Cache Strategies for Android

```kotlin
object CacheStrategies {
    
    // Memory cache
    class MemoryCache<K, V> {
        private val cache = java.util.LinkedHashMap<K, V>(100, 0.75f, true)
        
        fun put(key: K, value: V) {
            cache[key] = value
        }
        
        fun get(key: K): V? = cache[key]
        
        fun clear() = cache.clear()
    }
    
    // Disk cache using LRU
    // Use android.util.LruCache for memory
    // Use android.content.CursorLoader for database caching
}
```