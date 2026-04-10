# Network Optimization

## Learning Objectives

1. Understanding network performance optimization
2. Implementing efficient data transfer patterns
3. Using caching strategies for network data
4. Optimizing API calls and response handling
5. Monitoring network usage and performance

```kotlin
package com.kotlin.performance.network
```

---

## Prerequisites

- See: 06_NETWORKING/01_HTTP_Communication/01_Retrofit_Basics.md
- See: 06_NETWORKING/01_HTTP_Communication/02_OkHttp_Configuration.md
- See: 04_DATA_PERSISTENCE/02_Data_Storage/04_Cache_Strategies.md

---

## Core Concepts

### Network Optimization Goals

- **Minimize Requests**: Combine and batch requests
- **Reduce Payload**: Compress, use efficient formats
- **Cache Aggressively**: Local caching for repeated data
- **Defer When Possible**: Background sync vs real-time
- **Monitor Performance**: Track network metrics

### SECTION 1: API Request Optimization

```kotlin
/**
 * API Request Optimization
 * 
 * Optimizing API calls for performance.
 */
class APIRequestOptimization {
    
    // Request batching
    class RequestBatcher(private val api: ApiService) {
        
        private val pendingRequests = ArrayDeque<suspend () -> Any>()
        private var isProcessing = false
        
        suspend fun <T> enqueue(request: suspend () -> T): T {
            if (!isProcessing) {
                isProcessing = true
                return processBatch()
            }
            
            return withContext(Dispatchers.IO) {
                suspendCoroutine { continuation ->
                    pendingRequests.addLast {
                        val result = request()
                        continuation.resume(result as Any)
                    }
                }
            }
        }
        
        private suspend fun <T> processBatch(): T {
            val requests = mutableListOf<suspend () -> Any>()
            while (pendingRequests.isNotEmpty()) {
                requests.add(pendingRequests.pollFirst())
            }
            
            // Process batch
            return api.batchRequest(requests.map { it() as RequestData }) as T
        }
    }
    
    // GraphQL-style query optimization
    class GraphQLQuery {
        
        // Use fragments to share query parts
        val userQuery = """
            query GetUser($id: ID!) {
                user(id: $id) {
                    ...UserFields
                }
            }
            
            fragment UserFields on User {
                id
                name
                email
                avatar {
                    url
                }
            }
        """.trimIndent()
        
        // Avoid over-fetching - request only needed fields
        val optimizedUserQuery = """
            query GetUserBrief($id: ID!) {
                user(id: $id) {
                    id
                    name
                }
            }
        """.trimIndent()
    }
    
    // Pagination with cursor-based approach
    class PaginationHandler<T> {
        
        private var cursor: String? = null
        private var hasMore = true
        private val pageSize = 20
        
        suspend fun loadNextPage(
            api: ApiService,
            transform: (Response) -> List<T>
        ): List<T>? {
            if (!hasMore) return null
            
            val response = api.getItems(cursor, pageSize)
            
            cursor = response.nextCursor
            hasMore = response.hasMore
            
            return transform(response)
        }
        
        fun reset() {
            cursor = null
            hasMore = true
        }
    }
    
    interface ApiService {
        suspend fun batchRequest(requests: List<RequestData>): Response
        suspend fun getItems(cursor: String?, limit: Int): Response
    }
    
    data class RequestData(val query: String, val variables: Map<String, Any>)
    data class Response(val items: List<Any>, val nextCursor: String?, val hasMore: Boolean)
}
```

---

## SECTION 2: Response Optimization

```kotlin
/**
 * Response Optimization
 * 
 * Optimizing API responses and data handling.
 */
class ResponseOptimization {
    
    // JSON parsing optimization
    class JSONOptimizer {
        
        // Use Moshi/JsonClass instead of Gson for better performance
        fun parseWithMoshi(json: String): User {
            val moshi = com.squareup.moshi.Moshi.Builder().build()
            val adapter = moshi.adapter(User::class.java)
            return adapter.fromJson(json)!!
        }
        
        // Use data class with proper annotations
        @com.squareup.moshi.JsonClass(generateAdapter = true)
        data class User(
            val id: Long,
            val name: String,
            val email: String,
            val profile: Profile? = null
        )
        
        @com.squareup.moshi.JsonClass(generateAdapter = true)
        data class Profile(
            val avatar: String,
            val bio: String
        )
    }
    
    // Gzip compression
    class GzipCompression {
        
        fun setupGzipRequest(okHttpClient: okhttp3.OkHttpClient): okhttp3.OkHttpClient {
            return okHttpClient.newBuilder()
                .addInterceptor { chain ->
                    val request = chain.request().newBuilder()
                        .header("Accept-Encoding", "gzip")
                        .build()
                    chain.proceed(request)
                }
                .build()
        }
        
        // Automatic gzip handling in OkHttp
        // OkHttp automatically handles gzip decompression
        fun handleGzipResponse(response: okhttp3.Response): String {
            val body = response.body
            val contentEncoding = response.header("Content-Encoding")
            
            return when (contentEncoding) {
                "gzip" -> body?.let { gzipDecompress(it.byteStream()) } ?: ""
                else -> body?.string() ?: ""
            }
        }
        
        private fun gzipDecompress(inputStream: java.io.InputStream): String {
            return java.util.zip.GZIPInputStream(inputStream).bufferedReader().readText()
        }
    }
    
    // Binary protocol (Protocol Buffers)
    class ProtocolBufferExample {
        
        // .proto file: User.proto
        /*
        syntax = "proto3";
        
        message User {
            int64 id = 1;
            string name = 2;
            string email = 3;
            Profile profile = 4;
        }
        
        message Profile {
            string avatar = 1;
            string bio = 2;
        }
        */
        
        // Using protobuf in Kotlin
        fun serializeUser(user: UserProto): ByteArray {
            return user.toByteArray()
        }
        
        fun deserializeUser(data: ByteArray): UserProto {
            return UserProto.parseFrom(data)
        }
        
        // Protobuf is more efficient than JSON
        // - Smaller payload
        // - Faster parsing
        // - Schema validation
    }
    
    // Field selection to reduce payload
    class FieldSelector {
        
        interface UserFieldSelector {
            fun selectName(response: UserResponse): String = response.name
            fun selectEmail(response: UserResponse): String = response.email
            fun selectAvatar(response: UserResponse): String? = response.avatar
            fun selectBio(response: UserResponse): String? = response.bio
        }
        
        // Client specifies fields it needs
        fun getUserWithFields(
            api: ApiService,
            userId: Long,
            fields: List<String>
        ): UserResponse {
            val query = "SELECT ${fields.joinToString(", ")} FROM users WHERE id = $userId"
            return api.query(query)
        }
        
        // Response size comparison:
        // Full: {"id":1,"name":"John","email":"john@...","avatar": {...}, "bio": "...", "created_at": "...", "updated_at": "...", ...}
        // Minimal: {"id":1,"name":"John"}
        // Size reduction: 90%+ for minimal
    }
    
    data class UserResponse(
        val id: Long,
        val name: String,
        val email: String,
        val avatar: String?,
        val bio: String?
    )
    
    interface ApiService {
        fun query(sql: String): UserResponse
    }
}
```

---

## SECTION 3: Caching Strategy

```kotlin
/**
 * Caching Strategy
 * 
 * Implementing effective caching for network data.
 */
class NetworkCachingStrategy {
    
    // OkHttp cache configuration
    class OkHttpCacheSetup {
        
        fun createCache(context: android.content.Context): okhttp3.Cache {
            val cacheDir = java.io.File(context.cacheDir, "http_cache")
            val maxSize = 10L * 1024 * 1024  // 10 MB
            
            return okhttp3.Cache(cacheDir, maxSize)
        }
        
        fun createOkHttpClient(cache: okhttp3.Cache): okhttp3.OkHttpClient {
            return okhttp3.OkHttpClient.Builder()
                .cache(cache)
                .addInterceptor { chain ->
                    val request = chain.request()
                    
                    // Add cache control headers
                    request.newBuilder()
                        .header("Cache-Control", "public, max-age=60")
                        .build()
                        .let { chain.proceed(it) }
                }
                .addNetworkInterceptor { chain ->
                    val response = chain.proceed(chain.request())
                    
                    // Cache for 5 minutes on network
                    response.newBuilder()
                        .header("Cache-Control", "public, max-age=300")
                        .build()
                }
                .build()
        }
    }
    
    // Room database cache
    class RoomCache(private val database: AppDatabase) {
        
        suspend fun getUsersWithCache(userId: Long): User? {
            // Try memory cache first
            memoryCache[userId]?.let { return it }
            
            // Try disk cache
            database.userDao().getUserById(userId)?.let { user ->
                memoryCache[userId] = user
                return user
            }
            
            // Fetch from network
            val networkUser = fetchFromNetwork(userId)
            networkUser?.let { user ->
                database.userDao().insertUser(user)
                memoryCache[userId] = user
            }
            
            return networkUser
        }
        
        private val memoryCache = mutableMapOf<Long, User>()
        
        private suspend fun fetchFromNetwork(userId: Long): User? {
            // Network call
            return null
        }
    }
    
    // Cache invalidation strategies
    class CacheInvalidation {
        
        // Time-based invalidation
        fun isCacheValid(cachedTime: Long, maxAge: Long): Boolean {
            return System.currentTimeMillis() - cachedTime < maxAge
        }
        
        // Event-based invalidation (e.g., after update)
        fun invalidateUser(userId: Long) {
            memoryCache.remove(userId)
            // Also invalidate in disk cache
        }
        
        // Stale-while-revalidate pattern
        suspend fun <T> getWithStaleRevalidate(
            cacheKey: String,
            maxAge: Long,
            fetch: suspend () -> T
        ): CachedResult<T> {
            val cached = getFromCache(cacheKey)
            
            if (cached != null && isCacheValid(cached.timestamp, maxAge)) {
                return CachedResult(cached.data, isStale = false)
            }
            
            // Return cached while revalidating in background
            if (cached != null) {
                // Trigger background refresh
                CoroutineScope(Dispatchers.IO).launch {
                    try {
                        val fresh = fetch()
                        saveToCache(cacheKey, fresh)
                    } catch (e: Exception) {
                        // Ignore - we have stale data
                    }
                }
                
                return CachedResult(cached.data, isStale = true)
            }
            
            // No cache, fetch and wait
            val data = fetch()
            saveToCache(cacheKey, data)
            return CachedResult(data, isStale = false)
        }
        
        private fun getFromCache(key: String): CacheEntry? = null
        private fun saveToCache(key: String, data: Any) {}
        
        data class CacheEntry(val data: Any, val timestamp: Long)
        data class CachedResult<T>(val data: T, val isStale: Boolean)
    }
}
```

---

## Best Practices

1. **Minimize Requests**: Combine multiple API calls into batches
2. **Use Compression**: Enable gzip for all requests
3. **Cache Aggressively**: Local cache for frequently accessed data
4. **Use Pagination**: Don't load all data at once
5. **Select Fields**: Request only needed fields from API
6. **Use HTTP Caching**: Leverage browser/server caching
7. **Prefetch Strategically**: Load next page before user scrolls
8. **Handle Offline**: Cache data for offline access
9. **Monitor Metrics**: Track request times, sizes, errors
10. **Use HTTP/2**: Enable HTTP/2 for connection reuse

---

## Common Pitfalls and Solutions

### Pitfall 1: N+1 Request Problem
- **Problem**: Loading related data causes multiple requests
- **Solution**: Use batch API, caching, or GraphQL

### Pitfall 2: Large JSON Payloads
- **Problem**: Slow parsing, high bandwidth
- **Solution**: Use compression, field selection, or binary formats

### Pitfall 3: No Caching
- **Problem**: Repeated network requests
- **Solution**: Implement memory and disk caching

### Pitfall 4: Unoptimized Images
- **Problem**: Large image downloads
- **Solution**: Use proper sizes, WebP format, lazy loading

### Pitfall 5: No Request Timeout
- **Problem**: Requests hang indefinitely
- **Solution**: Set appropriate timeouts in OkHttp

### Pitfall 6: Inefficient Parsing
- **Problem**: Slow JSON parsing
- **Solution**: Use Moshi with codegen, or Protocol Buffers

---

## Troubleshooting Guide

### Issue: Slow Network Requests
- **Steps**: 1. Check response size 2. Enable compression 3. Implement caching

### Issue: High Network Usage
- **Steps**: 1. Implement caching 2. Reduce request frequency 3. Use pagination

---

## EXAMPLE 1: Optimized Retrofit Client

```kotlin
/**
 * Optimized Retrofit Client
 * 
 * Production-ready Retrofit setup with optimizations.
 */
class OptimizedRetrofit {
    
    class RetrofitClient(private val context: android.content.Context) {
        
        private val okHttpClient: okhttp3.OkHttpClient by lazy {
            createOptimizedOkHttpClient()
        }
        
        private val retrofit: retrofit2.Retrofit by lazy {
            createRetrofit()
        }
        
        private fun createOptimizedOkHttpClient(): okhttp3.OkHttpClient {
            val cacheDir = java.io.File(context.cacheDir, "http_cache")
            val cache = okhttp3.Cache(cacheDir, 10L * 1024 * 1024)  // 10 MB
            
            return okhttp3.OkHttpClient.Builder()
                .cache(cache)
                .connectTimeout(15, java.util.concurrent.TimeUnit.SECONDS)
                .readTimeout(15, java.util.concurrent.TimeUnit.SECONDS)
                .writeTimeout(15, java.util.concurrent.TimeUnit.SECONDS)
                .retryOnConnectionFailure(true)
                .addInterceptor(createLoggingInterceptor())
                .addInterceptor(createCacheInterceptor())
                .addNetworkInterceptor(createCacheControlInterceptor())
                .addInterceptor(RetryInterceptor(maxRetries = 3))
                .build()
        }
        
        private fun createRetrofit(): retrofit2.Retrofit {
            return retrofit2.Retrofit.Builder()
                .baseUrl("https://api.example.com/")
                .client(okHttpClient)
                .addConverterFactory(createMoshiConverter())
                .addCallAdapterFactory(createCoroutinesAdapter())
                .build()
        }
        
        private fun createMoshiConverter(): retrofit2.converter.moshi.MoshiConverterFactory {
            val moshi = com.squareup.moshi.Moshi.Builder()
                .addLast(com.squareup.moshi.kotlin.reflect.KotlinJsonAdapterFactory())
                .build()
            return retrofit2.converter.moshi.MoshiConverterFactory.create(moshi)
        }
        
        private fun createCoroutinesAdapter(): retrofit2.converter.kotlinx.coroutines.CoroutineCallAdapterFactory {
            return retrofit2.converter.kotlinx.coroutines.CoroutineCallAdapterFactory()
        }
        
        private fun createLoggingInterceptor(): okhttp3.Interceptor {
            return okhttp3.LoggingInterceptor.Builder()
                .logLevel(okhttp3.logging.HttpLoggingInterceptor.Level.BODY)
                .build()
        }
        
        private fun createCacheInterceptor(): okhttp3.Interceptor {
            return okhttp3.Interceptor { chain ->
                var request = chain.request()
                
                request = request.newBuilder()
                    .header("Cache-Control", "public, max-age=60")
                    .build()
                
                chain.proceed(request)
            }
        }
        
        private fun createCacheControlInterceptor(): okhttp3.Interceptor {
            return okhttp3.Interceptor { chain ->
                val response = chain.proceed(chain.request())
                
                response.newBuilder()
                    .header("Cache-Control", "public, max-age=300")
                    .removeHeader("Pragma")
                    .build()
            }
        }
        
        // Retry interceptor for failed requests
        class RetryInterceptor(private val maxRetries: Int) : okhttp3.Interceptor {
            override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
                var request = chain.request()
                var response: okhttp3.Response? = null
                var exception: java.io.IOException? = null
                
                var tryCount = 0
                while (tryCount < maxRetries && (response == null || !response.isSuccessful)) {
                    tryCount++
                    
                    try {
                        response?.close()
                        response = chain.proceed(request)
                    } catch (e: java.io.IOException) {
                        exception = e
                    }
                }
                
                if (response == null) {
                    throw exception ?: java.io.IOException("Unknown error")
                }
                
                return response
            }
        }
        
        fun <T> getService(serviceClass: Class<T>): T {
            return retrofit.create(serviceClass)
        }
    }
    
    // Usage with coroutines
    class UserRepository(private val api: UserApi) {
        
        suspend fun getUser(userId: Long): Result<User> {
            return try {
                val response = api.getUser(userId)
                if (response.isSuccessful) {
                    Result.success(response.body()!!)
                } else {
                    Result.failure(java.lang.Exception("Error: ${response.code()}"))
                }
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
        
        suspend fun getUsers(): Result<List<User>> {
            return try {
                val response = api.getUsers()
                if (response.isSuccessful) {
                    Result.success(response.body() ?: emptyList())
                } else {
                    Result.failure(java.lang.Exception("Error: ${response.code()}"))
                }
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
    }
    
    interface UserApi {
        @GET("users/{id}")
        suspend fun getUser(@Path("id") id: Long): retrofit2.Response<User>
        
        @GET("users")
        suspend fun getUsers(): retrofit2.Response<List<User>>
    }
    
    data class User(val id: Long, val name: String, val email: String)
}
```

---

## EXAMPLE 2: Offline-First Architecture

```kotlin
/**
 * Offline-First Architecture
 * 
 * Network optimization with offline support.
 */
class OfflineFirstArchitecture {
    
    // Repository with offline support
    class OfflineFirstRepository(
        private val api: UserApi,
        private val localDataSource: LocalDataSource,
        private val networkMonitor: NetworkMonitor
    ) {
        
        suspend fun getUser(userId: Long): UserResult {
            // Always return cached data first
            localDataSource.getUser(userId)?.let { cached ->
                // Emit cached data immediately
                emit(UserResult.Data(cached))
                
                // Try to update from network if online
                if (networkMonitor.isOnline()) {
                    try {
                        val fresh = fetchFromNetwork(userId)
                        localDataSource.saveUser(fresh)
                        return UserResult.Data(fresh)
                    } catch (e: Exception) {
                        // Keep using cached data
                    }
                }
                
                return UserResult.Data(cached)
            }
            
            // No cache - must fetch from network
            return try {
                val user = fetchFromNetwork(userId)
                localDataSource.saveUser(user)
                UserResult.Data(user)
            } catch (e: Exception) {
                UserResult.Error(e.message ?: "Network error")
            }
        }
        
        suspend fun saveUser(user: User) {
            // Save locally first
            localDataSource.saveUser(user)
            
            // Then sync to network in background
            if (networkMonitor.isOnline()) {
                try {
                    syncToNetwork(user)
                } catch (e: Exception) {
                    // Will be retried later
                    localDataSource.markPendingSync(user.id)
                }
            } else {
                localDataSource.markPendingSync(user.id)
            }
        }
        
        private suspend fun fetchFromNetwork(userId: Long): User {
            val response = api.getUser(userId)
            return response.body()!!
        }
        
        private suspend fun syncToNetwork(user: User) {
            api.updateUser(user)
        }
    }
    
    // Network connectivity monitor
    class NetworkMonitor(private val context: android.content.Context) {
        
        private val connectivityManager = context.getSystemService(
            android.content.Context.CONNECTIVITY_SERVICE
        ) as android.net.ConnectivityManager
        
        private val _isOnline = MutableStateFlow(true)
        val isOnline: StateFlow<Boolean> = _isOnline
        
        fun isOnline(): Boolean = _isOnline.value
        
        fun startMonitoring() {
            val callback = object : android.net.ConnectivityManager.NetworkCallback() {
                override fun onAvailable(network: android.net.Network) {
                    _isOnline.value = true
                }
                
                override fun onLost(network: android.net.Network) {
                    _isOnline.value = false
                }
            }
            
            val request = android.net.NetworkRequest.Builder()
                .addCapability(android.net.NetworkCapabilities.NET_CAPABILITY_INTERNET)
                .build()
            
            connectivityManager.registerNetworkCallback(request, callback)
        }
    }
    
    // Local data source
    class LocalDataSource(private val database: AppDatabase) {
        
        suspend fun getUser(userId: Long): User? {
            return database.userDao().getUserById(userId)
        }
        
        suspend fun saveUser(user: User) {
            database.userDao().insertUser(user)
        }
        
        suspend fun markPendingSync(userId: Long) {
            // Mark user as needing sync
        }
    }
    
    // Sealed class for result
    sealed class UserResult {
        data class Data(val user: User) : UserResult()
        data class Error(val message: String) : UserResult()
    }
    
    interface UserApi {
        @GET("users/{id}")
        suspend fun getUser(@Path("id") id: Long): retrofit2.Response<User>
        
        @PUT("users/{id}")
        suspend fun updateUser(@Body user: User): retrofit2.Response<Unit>
    }
    
    interface LocalDataSource {
        suspend fun getUser(userId: Long): User?
        suspend fun saveUser(user: User)
        suspend fun markPendingSync(userId: Long)
    }
    
    data class User(val id: Long, val name: String, val email: String)
    interface AppDatabase { fun userDao(): UserDao }
    interface UserDao { suspend fun getUserById(id: Long): User?; suspend fun insertUser(user: User) }
}
```

---

## EXAMPLE 3: Image Loading Optimization

```kotlin
/**
 * Image Loading Optimization
 * 
 * Optimizing network image loading with caching and compression.
 */
class ImageLoadingOptimization {
    
    // Optimized image loader
    class ImageLoader(
        private val context: android.content.Context,
        private val okHttpClient: okhttp3.OkHttpClient
    ) {
        
        private val memoryCache: android.util.LruCache<String, android.graphics.Bitmap>
        private val diskCache: okhttp3.Cache
        
        init {
            val maxMemory = (android.os.Runtime.getRuntime().maxMemory() / 1024).toInt()
            val cacheSize = maxMemory / 8
            
            memoryCache = object : android.util.LruCache<String, android.graphics.Bitmap>(cacheSize) {
                override fun sizeOf(key: String, bitmap: android.graphics.Bitmap): Int {
                    return bitmap.byteCount / 1024
                }
            }
            
            val cacheDir = java.io.File(context.cacheDir, "image_cache")
            diskCache = okhttp3.Cache(cacheDir, 50L * 1024 * 1024)  // 50 MB
        }
        
        fun loadImage(
            url: String,
            targetWidth: Int,
            targetHeight: Int,
            quality: Int = 80,
            onLoaded: (android.graphics.Bitmap) -> Unit
        ) {
            // Check memory cache
            val cacheKey = "$url-$targetWidth-$targetHeight"
            memoryCache.get(cacheKey)?.let {
                onLoaded(it)
                return
            }
            
            // Load from disk or network
            CoroutineScope(Dispatchers.IO).launch {
                val bitmap = loadFromNetworkOrCache(url, targetWidth, targetHeight, quality)
                bitmap?.let {
                    memoryCache.put(cacheKey, it)
                    withContext(Dispatchers.Main) {
                        onLoaded(it)
                    }
                }
            }
        }
        
        private suspend fun loadFromNetworkOrCache(
            url: String,
            width: Int,
            height: Int,
            quality: Int
        ): android.graphics.Bitmap? {
            // Build optimized URL with dimensions
            val optimizedUrl = buildOptimizedUrl(url, width, height, quality)
            
            val request = okhttp3.Request.Builder()
                .url(optimizedUrl)
                .build()
            
            return try {
                val response = okHttpClient.newCall(request).execute()
                
                if (response.isSuccessful) {
                    response.body?.byteStream()?.use { inputStream ->
                        android.graphics.BitmapFactory.decodeStream(inputStream)
                    }
                } else {
                    null
                }
            } catch (e: Exception) {
                null
            }
        }
        
        private fun buildOptimizedUrl(
            originalUrl: String,
            width: Int,
            height: Int,
            quality: Int
        ): String {
            // Add image service parameters (e.g., Cloudinary, Imgix, or custom)
            val separator = if (originalUrl.contains("?")) "&" else "?"
            return "$originalUrl${separator}w=$width&h=$height&q=$quality&fmt=webp"
        }
        
        fun preloadImages(urls: List<String>, width: Int, height: Int) {
            urls.forEach { url ->
                loadImage(url, width, height) { }
            }
        }
    }
    
    // Coil for Compose
    class CoilCompose {
        
        @Composable
        fun AsyncImage(
            url: String,
            contentDescription: String?
        ) {
            coil.compose.AsyncImage(
                model = coil.request.ImageRequest.Builder(androidx.compose.ui.platform.LocalContext.current)
                    .data(url)
                    .crossfade(true)
                    .size(coil.size.Size.ORIGINAL)
                    .build(),
                contentDescription = contentDescription,
                contentScale = androidx.compose.ui.layout.ContentScale.Crop,
                modifier = Modifier.fillMaxWidth()
            )
        }
        
        // Optimized image with caching
        @Composable
        fun CachedImage(
            url: String,
            contentDescription: String?
        ) {
            val context = androidx.compose.ui.platform.LocalContext.current
            
            coil.compose.AsyncImage(
                model = coil.request.ImageRequest.Builder(context)
                    .data(url)
                    .memoryCacheKey(url)
                    .diskCacheKey(url)
                    .crossfade(true)
                    .build(),
                contentDescription = contentDescription
            )
        }
    }
    
    // Picasso for Views
    class PicassoImageLoader {
        
        private val picasso = com.squareup.picasso.Picasso.get()
        
        fun loadOptimized(
            imageView: android.widget.ImageView,
            url: String,
            width: Int,
            height: Int
        ) {
            picasso.load(url)
                .resize(width, height)
                .centerCrop()
                .placeholder(android.R.drawable.ic_menu_gallery)
                .error(android.R.drawable.ic_delete)
                .into(imageView)
        }
        
        // Prefetch for smooth scrolling
        fun prefetch(urls: List<String>) {
            urls.forEach { url ->
                picasso.load(url).fetch()
            }
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Network Optimization Techniques:**
- Use OkHttp with caching
- Implement memory and disk cache
- Enable gzip compression
- Use field selection in API
- Implement pagination
- Use HTTP/2 for connection reuse
- Add retry interceptor

**Response Size Comparison:**
- Full JSON: ~500 bytes average field
- Compressed: ~80% reduction
- Binary (protobuf): ~60% smaller
- Field selection: ~90% reduction

**Caching Strategies:**
- Memory cache: Fastest access
- Disk cache: Persistent
- HTTP cache: Automatic
- Stale-while-revalidate: Best UX

**Image Optimization:**
- Resize to display size
- Use WebP format
- Progressive loading
- Memory + disk cache

---

## Advanced Tips

- **Tip 1: Use HTTP/2** - Multiplexing, header compression
- **Tip 3: Implement request queuing** - Batch background requests
- **Tip 4: Use OkHttp interceptors** - For logging, caching, auth
- **Tip 5: Profile network with Chrome DevTools** - Debug network issues

---

## Troubleshooting Guide (FAQ)

**Q: How do I reduce network requests?**
A: Use caching, batching, and pagination

**Q: Why are responses slow?**
A: Check compression, response size, server latency

**Q: How do I optimize image loading?**
A: Use Coil/Glide with proper sizing and caching

**Q: Should I use HTTP/2?**
A: Yes - better performance for multiple requests

---

## Advanced Tips and Tricks

- **Tip 1: Use Cronet** - Chromium network stack for better performance
- **Tip 2: Implement request deduplication** - OkHttp can automatically deduplicate
- **Tip 3: Usequic** - QUIC protocol for faster connection
- **Tip 4: Monitor with Stetho** - Debug network traffic with Chrome
- **Tip 5: Use Charles Proxy** - Full network debugging

---

## CROSS-REFERENCES

- See: 06_NETWORKING/01_HTTP_Communication/01_Retrofit_Basics.md
- See: 06_NETWORKING/01_HTTP_Communication/02_OkHttp_Configuration.md
- See: 04_DATA_PERSISTENCE/02_Data_Storage/04_Cache_Strategies.md

---

## END OF NETWORK OPTIMIZATION GUIDE

(End of file - total 682 lines)