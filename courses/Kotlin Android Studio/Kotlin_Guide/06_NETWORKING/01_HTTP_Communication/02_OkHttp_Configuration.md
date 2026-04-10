# OkHttp Configuration

## Learning Objectives

1. Understanding OkHttp as the HTTP engine behind Retrofit
2. Configuring timeouts, connection pooling, and caching
3. Setting up interceptors for logging and debugging
4. Implementing certificate pinning for security
5. Optimizing OkHttp for production environments

## Prerequisites

- [Retrofit Basics](./01_Retrofit_Basics.md)
- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)

## Section 1: OkHttp Fundamentals

OkHttp is an efficient HTTP client that powers Retrofit. While Retrofit provides a high-level abstraction, OkHttp handles the low-level network operations. Understanding OkHttp configuration is crucial for building robust network layers in Android applications.

OkHttp key features include:
- Connection pooling for efficient reuse
- GZIP compression automatic handling
- Response caching with disk and memory
- HTTP/2 support for multiplexing
- Automatic retry on connection failures

```kotlin
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.Response
import okhttp3.logging.HttpLoggingInterceptor
import java.util.concurrent.TimeUnit

// Basic OkHttp client configuration
class OkHttpConfiguration {
    
    // Simple client with default settings
    fun createBasicClient(): OkHttpClient {
        return OkHttpClient()
    }
    
    // Configured client with timeouts
    fun createConfiguredClient(): OkHttpClient {
        return OkHttpClient.Builder()
            // Connection timeout - max time to establish connection
            .connectTimeout(30, TimeUnit.SECONDS)
            // Read timeout - max time between data packets
            .readTimeout(30, TimeUnit.SECONDS)
            // Write timeout - max time to send request data
            .writeTimeout(30, TimeUnit.SECONDS)
            // Ping interval for HTTP/2 connections
            .pingInterval(30, TimeUnit.SECONDS)
            .build()
    }
    
    // Simple synchronous request
    fun makeSimpleRequest(client: OkHttpClient, url: String): String? {
        val request = Request.Builder()
            .url(url)
            .build()
        
        return client.newCall(request).execute().use { response ->
            if (response.isSuccessful) {
                response.body?.string()
            } else {
                null
            }
        }
    }
}
```

## Section 2: Advanced Client Configuration

For production applications, OkHttp requires careful configuration to handle various network conditions, security requirements, and performance optimization.

```kotlin
import okhttp3.Cache
import okhttp3.CertificatePinner
import okhttp3.ConnectionPool
import okhttp3.Dispatcher
import okhttp3.OkHttpClient
import okhttp3.Request
import android.content.Context
import java.io.File
import java.util.concurrent.TimeUnit

// Advanced OkHttp configuration for production
object OkHttpClientFactory {
    
    private const val CACHE_SIZE = 10L * 1024 * 1024 // 10 MB
    private const val MAX_IDLE_CONNECTIONS = 5
    private const val KEEP_ALIVE_DURATION = 5L
    
    // Create production-ready OkHttp client
    fun createProductionClient(context: Context): OkHttpClient {
        return OkHttpClient.Builder()
            // Timeouts - adjust based on API requirements
            .connectTimeout(15, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .writeTimeout(30, TimeUnit.SECONDS)
            .pingInterval(20, TimeUnit.SECONDS)
            
            // Connection pool for connection reuse
            .connectionPool(ConnectionPool(
                maxIdleConnections = MAX_IDLE_CONNECTIONS,
                keepAliveDuration = KEEP_ALIVE_DURATION,
                timeUnit = TimeUnit.MINUTES
            ))
            
            // Enable caching
            .cache(createCache(context))
            
            // Dispatcher configuration for request limits
            .dispatcher(createDispatcher())
            
            // Add interceptors (see Interceptor Patterns guide)
            // .addInterceptor(...)
            // .addNetworkInterceptor(...)
            
            .build()
    }
    
    // Create disk cache
    private fun createCache(context: Context): Cache {
        val cacheDir = File(context.cacheDir, "http_cache")
        return Cache(cacheDir, CACHE_SIZE)
    }
    
    // Configure dispatcher for request management
    private fun createDispatcher(): Dispatcher {
        return Dispatcher().apply {
            // Max concurrent requests
            maxRequests = 64
            // Max requests per host
            maxRequestsPerHost = 16
        }
    }
}

// Certificate pinning for security
class SecureOkHttpConfig {
    
    private val certificatePinner = CertificatePinner.Builder()
        .add("api.example.com", "sha256/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=")
        .add("api.example.com", "sha256/BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB=")
        .build()
    
    fun createPinnedClient(): OkHttpClient {
        return OkHttpClient.Builder()
            .certificatePinner(certificatePinner)
            .build()
    }
}

// DNS configuration for custom resolution
class DnsConfiguration {
    
    // Custom DNS over HTTPS
    fun createDnsOverHttpsClient(): OkHttpClient {
        val dns = DnsOverHttps.Builder()
            .client(OkHttpClient())
            .url("https://dns.google/resolve")
            .build()
        
        return OkHttpClient.Builder()
            .dns(dns)
            .build()
    }
}
```

## Section 3: Client with Interceptors

Interceptors are powerful mechanisms for observing, modifying, and retrying network requests. OkHttp supports application interceptors and network interceptors with different use cases.

```kotlin
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.Response
import okhttp3.logging.HttpLoggingInterceptor
import java.util.concurrent.TimeUnit

// Custom interceptor for adding headers
class HeaderInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val originalRequest = chain.request()
        
        // Add custom headers
        val newRequest = originalRequest.newBuilder()
            .header("Accept", "application/json")
            .header("Content-Type", "application/json")
            .header("X-App-Version", "1.0.0")
            .header("X-Device-ID", "device-uuid-here")
            .header("X-Platform", "Android")
            .header("Accept-Language", "en-US")
            .method(originalRequest.method, originalRequest.body)
            .build()
        
        return chain.proceed(newRequest)
    }
}

// Authentication interceptor
class AuthInterceptor(private val tokenProvider: TokenProvider) : Interceptor {
    
    interface TokenProvider {
        fun getToken(): String?
    }
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val originalRequest = chain.request()
        val token = tokenProvider.getToken()
        
        val authenticatedRequest = if (token != null) {
            originalRequest.newBuilder()
                .header("Authorization", "Bearer $token")
                .build()
        } else {
            originalRequest
        }
        
        return chain.proceed(authenticatedRequest)
    }
}

// Logging interceptor configuration
object LoggingInterceptorConfig {
    
    // Create logging interceptor with custom settings
    fun createLoggingInterceptor(): HttpLoggingInterceptor {
        return HttpLoggingInterceptor().apply {
            // Log level determines what gets logged
            level = HttpLoggingInterceptor.Level.BODY
            // BODY: logs request and response headers and body
            // HEADERS: logs request and response headers
            // BASIC: logs request method, URL, response status
            // NONE: no logging
        }
    }
    
    // Alternative: custom logging implementation
    class CustomLoggingInterceptor : Interceptor {
        
        override fun intercept(chain: Interceptor.Chain): Response {
            val request = chain.request()
            
            val startTime = System.nanoTime()
            println("Sending request: ${request.url}")
            println("Headers: ${request.headers}")
            
            val response = chain.proceed(request)
            
            val endTime = System.nanoTime()
            val duration = (endTime - startTime) / 1_000_000
            
            println("Received response for ${request.url} in ${duration}ms")
            println("Status: ${response.code}")
            
            return response
        }
    }
}

// Complete client with all interceptors
class CompleteOkHttpClient {
    
    fun createClient(): OkHttpClient {
        val loggingInterceptor = LoggingInterceptorConfig.createLoggingInterceptor()
        val headerInterceptor = HeaderInterceptor()
        
        return OkHttpClient.Builder()
            .addInterceptor(headerInterceptor) // Application interceptor
            .addInterceptor(loggingInterceptor) // Application interceptor
            .connectTimeout(15, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .build()
    }
}
```

## Section 4: Production Example - Secure Network Client

This example demonstrates a production-ready OkHttp client with comprehensive security, caching, and monitoring features.

```kotlin
import okhttp3.Cache
import okhttp3.CertificatePinner
import okhttp3.Credentials
import okhttp3.Dispatcher
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.Response
import okhttp3.logging.HttpLoggingInterceptor
import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import java.io.File
import java.io.IOException
import java.util.concurrent.TimeUnit

// Network security configuration
object NetworkSecurityConfig {
    
    // Certificate pinning hashes for production APIs
    // Calculate using: openssl s_client -servername example.com -connect example.com:443 | openssl x509 -fingerprint -sha256 -noout
    private val pinnedHosts = mapOf(
        "api.example.com" to listOf(
            "sha256/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
            "sha256/BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB="
        ),
        "secure.example.com" to listOf(
            "sha256/CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC="
        )
    )
    
    fun buildCertificatePinner(): CertificatePinner {
        val builder = CertificatePinner.Builder()
        pinnedHosts.forEach { (host, pins) ->
            pins.forEach { pin ->
                builder.add(host, pin)
            }
        }
        return builder.build()
    }
}

// Cache control interceptor for force caching
class CacheControlInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        
        // Add cache control headers
        val modifiedRequest = request.newBuilder()
            .header("Cache-Control", "public, max-age=60")
            .build()
        
        return chain.proceed(modifiedRequest)
    }
}

// Offline caching interceptor
class OfflineCacheInterceptor(private val context: Context) : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        var request = chain.request()
        
        // Apply cache if offline
        if (!isNetworkAvailable(context)) {
            request = request.newBuilder()
                .header("Cache-Control", "public, only-if-cached, max-stale=604800")
                .build()
        }
        
        return chain.proceed(request)
    }
    
    private fun isNetworkAvailable(context: Context): Boolean {
        val connectivityManager = context.getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager
        val network = connectivityManager.activeNetwork ?: return false
        val capabilities = connectivityManager.getNetworkCapabilities(network) ?: return false
        
        return capabilities.hasCapability(NetworkCapabilities.NET_CAPABILITY_INTERNET) &&
                capabilities.hasCapability(NetworkCapabilities.NET_CAPABILITY_VALIDATED)
    }
}

// Retry interceptor with exponential backoff
class RetryInterceptor(private val maxRetries: Int = 3) : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        var response: Response? = null
        var exception: IOException? = null
        
        var retryCount = 0
        while (retryCount < maxRetries) {
            try {
                response?.close()
                response = chain.proceed(request)
                
                if (response.isSuccessful) {
                    return response
                }
                
                // Retry on server errors
                if (response.code in 500..599) {
                    response.close()
                    retryCount++
                    Thread.sleep((retryCount * 1000).toLong()) // Exponential backoff
                    continue
                }
                
                return response
            } catch (e: IOException) {
                exception = e
                retryCount++
                if (retryCount < maxRetries) {
                    Thread.sleep((retryCount * 1000).toLong())
                }
            }
        }
        
        throw exception ?: IOException("Max retries exceeded")
    }
}

// Complete production client factory
class ProductionOkHttpClientFactory(private val context: Context) {
    
    private val cacheDir = File(context.cacheDir, "http_cache")
    private val cacheSize = 50L * 1024 * 1024 // 50 MB
    
    fun create(): OkHttpClient {
        return OkHttpClient.Builder()
            // Timeouts
            .connectTimeout(15, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .writeTimeout(30, TimeUnit.SECONDS)
            .pingInterval(20, TimeUnit.SECONDS)
            
            // Cache
            .cache(Cache(cacheDir, cacheSize))
            
            // Connection pool
            .connectionPool(ConnectionPool(
                maxIdleConnections = 5,
                keepAliveDuration = 5,
                timeUnit = TimeUnit.MINUTES
            ))
            
            // Dispatcher
            .dispatcher(createDispatcher())
            
            // Interceptors - order matters!
            // Application interceptors (execute once per request)
            .addInterceptor(AuthInterceptor(object : AuthInterceptor.TokenProvider {
                override fun getToken(): String? = getAuthToken()
            }))
            .addInterceptor(HeaderInterceptor())
            .addInterceptor(RetryInterceptor(maxRetries = 3))
            .addInterceptor(HttpLoggingInterceptor().apply {
                level = HttpLoggingInterceptor.Level.BODY
            })
            
            // Network interceptors (can execute multiple times for retries)
            .addNetworkInterceptor(OfflineCacheInterceptor(context))
            .addNetworkInterceptor(CacheControlInterceptor())
            
            // Security
            .certificatePinner(NetworkSecurityConfig.buildCertificatePinner())
            
            .build()
    }
    
    private fun createDispatcher(): Dispatcher {
        return Dispatcher(Dispatchers.IO.let {
            // Use coroutine dispatcher if available
            object : java.util.concurrent.ExecutorService {
                override fun shutdown() {}
                override fun shutdownNow() = java.util.Collections.emptyList()
                override fun isShutdown() = false
                override fun isTerminated() = false
                override fun awaitTermination(timeout: Long, unit: java.util.concurrent.TimeUnit) = true
                override fun submit(task: Runnable) = null
                override fun invokeAny(tasks: java.util.Collection<out java.util.concurrent Callable<Any>>) = null
                override fun invokeAny(tasks: java.util.Collection<out java.util.concurrent Callable<Any>>, timeout: Long, unit: java.util.concurrent.TimeUnit) = null
                override fun <T : Any?> submit(task: Runnable, result: T) = null
                override fun <T : Any?> submit(task: java.util.concurrent.Callable<T>) = null
                override fun invokeAll(tasks: java.util.Collection<out java.util.concurrent Callable<Any>>) = java.util.Collections.emptyList()
                override fun invokeAll(tasks: java.util.Collection<out java.util.concurrent Callable<Any>>, timeout: Long, unit: java.util.concurrent.TimeUnit) = java.util.Collections.emptyList()
                override fun <T : Any?> invokeAll(tasks: java.util.Collection<out java.util.concurrent Callable<T>>) = java.util.Collections.emptyList()
                override fun <T : Any?> invokeAll(tasks: java.util.Collection<out java.util.concurrent Callable<T>>, timeout: Long, unit: java.util.concurrent.TimeUnit) = java.util.Collections.emptyList()
                override fun execute(command: Runnable) {}
                override fun equals(other: Any?) = false
                override fun hashCode() = 0
                override fun toString() = "CoroutineDispatcher"
            }
        }.let {
            Dispatcher()
        }).apply {
            maxRequests = 64
            maxRequestsPerHost = 16
        }
    }
    
    private fun getAuthToken(): String? {
        // Retrieve from secure storage
        return null
    }
}
```

## Section 5: Production Example - Multi-API Client Manager

This example shows how to manage multiple OkHttp clients for different API endpoints with shared configuration.

```kotlin
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.Response
import okhttp3.logging.HttpLoggingInterceptor
import android.content.Context
import java.util.concurrent.ConcurrentHashMap
import java.util.concurrent.TimeUnit

// API endpoint configuration
data class ApiEndpoint(
    val baseUrl: String,
    val timeoutSeconds: Long = 30,
    val requiresAuth: Boolean = false,
    val isPinned: Boolean = false
)

// Manager for multiple OkHttp clients
class NetworkClientManager(private val context: Context) {
    
    // Store clients for different APIs
    private val clients = ConcurrentHashMap<String, OkHttpClient>()
    
    // Default endpoints configuration
    private val endpoints = mapOf(
        "weather" to ApiEndpoint(
            baseUrl = "https://api.weatherapi.com/v1/",
            timeoutSeconds = 30,
            requiresAuth = true
        ),
        "todo" to ApiEndpoint(
            baseUrl = "https://jsonplaceholder.typicode.com/",
            timeoutSeconds = 15,
            requiresAuth = false
        ),
        "auth" to ApiEndpoint(
            baseUrl = "https://auth.example.com/api/v1/",
            timeoutSeconds = 60,
            requiresAuth = false,
            isPinned = true
        )
    )
    
    // Get or create client for specific API
    fun getClient(apiName: String): OkHttpClient {
        return clients.getOrPut(apiName) {
            createClientForApi(apiName)
        }
    }
    
    // Create client with specific configuration
    private fun createClientForApi(apiName: String): OkHttpClient {
        val endpoint = endpoints[apiName] ?: endpoints["todo"]!!
        
        return OkHttpClient.Builder()
            .connectTimeout(endpoint.timeoutSeconds, TimeUnit.SECONDS)
            .readTimeout(endpoint.timeoutSeconds, TimeUnit.SECONDS)
            .writeTimeout(endpoint.timeoutSeconds, TimeUnit.SECONDS)
            .addInterceptor(createBaseInterceptor(endpoint))
            .addInterceptor(HttpLoggingInterceptor().apply {
                level = HttpLoggingInterceptor.Level.BASIC
            })
            .build()
    }
    
    // Base interceptor for all APIs
    private fun createBaseInterceptor(endpoint: ApiEndpoint) = Interceptor { chain ->
        val request = chain.request().newBuilder()
            .header("Accept", "application/json")
            .header("Content-Type", "application/json")
            .header("X-Client-Version", "1.0.0")
            .header("X-Platform", "Android")
            .apply {
                if (endpoint.requiresAuth) {
                    // Add auth token if required
                    // addHeader("Authorization", "Bearer $token")
                }
            }
            .build()
        
        chain.proceed(request)
    }
    
    // Clear all cached clients
    fun clearClients() {
        clients.values.forEach { client ->
            client.dispatcher.executorService.shutdown()
            client.connectionPool.evictAll()
            client.cache?.close()
        }
        clients.clear()
    }
}

// Usage example
class ApiServiceFactory(private val manager: NetworkClientManager) {
    
    // Create Retrofit with specific OkHttp client
    fun <T> createService(apiName: String, serviceClass: Class<T>): T {
        val okHttpClient = manager.getClient(apiName)
        
        val retrofit = Retrofit.Builder()
            .baseUrl(endpoints[apiName]?.baseUrl ?: "")
            .client(okHttpClient)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
        
        return retrofit.create(serviceClass)
    }
    
    // Direct HTTP calls without Retrofit
    fun makeRequest(apiName: String, path: String): Response? {
        val client = manager.getClient(apiName)
        val endpoint = endpoints[apiName]
        
        val request = Request.Builder()
            .url("${endpoint?.baseUrl}$path")
            .build()
        
        return client.newCall(request).execute()
    }
}
```

## Best Practices

- **Configure Appropriate Timeouts**: Set connectTimeout, readTimeout, and writeTimeout based on API requirements
- **Enable Response Caching**: Use disk cache to reduce network calls and enable offline support
- **Use Connection Pooling**: OkHttp automatically pools connections; tune maxIdleConnections for your use case
- **Implement Proper Interceptor Order**: Application interceptors run first, network interceptors run after
- **Pin Certificates in Production**: Use CertificatePinner to prevent man-in-the-middle attacks
- **Configure Dispatcher Limits**: Set maxRequests and maxRequestsPerHost to prevent overwhelming servers
- **Use Logging Sparingly**: Disable detailed logging in production for security and performance
- **Handle Network State**: Implement offline caching with NetworkInterceptors
- **Close Responses**: Always use response.close() or use block to auto-close
- **Singleton Pattern**: Create a single OkHttpClient instance and reuse it across the app

## Common Pitfalls

**Problem**: OkHttp creates too many connections causing resource exhaustion
**Solution**: Configure connection pool properly and use single OkHttpClient instance

**Problem**: Responses not being cached
**Solution**: Ensure Cache is configured and server returns proper Cache-Control headers

**Problem**: Certificate pinning failures in production
**Solution**: Provide backup pins and handle PinValidationException gracefully

**Problem**: Memory leaks with OkHttp
**Solution**: Use singleton client and ensure responses are properly closed

**Problem**: Requests timing out on slow networks
**Solution**: Increase timeouts or implement retry logic with exponential backoff

## Troubleshooting Guide

**Q: Why are my requests timing out?**
A: Check network connectivity, increase timeout values, or implement retry logic.

**Q: How do I enable HTTP response caching?**
A: Create a Cache object and pass it to OkHttpClient.Builder().cache().

**Q: Why isn't certificate pinning working?**
A: Ensure the pin format is correct (sha256/Base64) and include backup pins.

**Q: How to handle different APIs with different configurations?**
A: Create multiple OkHttpClient instances or use Interceptor to vary configuration per request.

**Q: Why is OkHttp creating new connections for each request?**
A: Check connection pool settings and ensure you're reusing the same OkHttpClient instance.

## Advanced Tips

- **HTTP/2 Push**: OkHttp automatically supports HTTP/2 server push when available
- **WebSocket Support**: Use OkHttp's WebSocket for real-time communication
- **Brotli Compression**: OkHttp supports Brotli for better compression ratios
- **Protocol Negotiation**: OkHttp automatically negotiates HTTP/1.1 or HTTP/2
- **Metrics Collection**: Use EventListener to collect performance metrics
- **Connection Specs**: Configure custom TLS versions and cipher suites for security

## Cross-References

- [Retrofit Basics](./01_Retrofit_Basics.md) - Using OkHttp with Retrofit
- [Interceptor Patterns](./03_Interceptor_Patterns.md) - Custom interceptors for auth and logging
- [Authentication Implementation](./04_Authentication_Implementation.md) - Token-based authentication
- [Error Handling Strategies](./05_Error_Handling_Strategies.md) - Network error handling
- [Flow Implementation](../02_Asynchronous_Patterns/02_Flow_Implementation.md) - Async data streams
