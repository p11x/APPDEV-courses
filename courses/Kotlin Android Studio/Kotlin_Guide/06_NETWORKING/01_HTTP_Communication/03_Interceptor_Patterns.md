# Interceptor Patterns

## Learning Objectives

1. Understanding OkHttp interceptor architecture
2. Implementing application interceptors for request/response modification
3. Building network interceptors for low-level HTTP operations
4. Creating interceptors for authentication, logging, and caching
5. Chaining multiple interceptors effectively

## Prerequisites

- [OkHttp Configuration](./02_OkHttp_Configuration.md)
- [Retrofit Basics](./01_Retrofit_Basics.md)
- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)

## Section 1: Interceptor Architecture

OkHttp interceptors are a powerful mechanism for observing and modifying HTTP requests and responses. They work in a chain, with each interceptor having the ability to pass the request down the chain or short-circuit by returning a cached response.

There are two types of interceptors:
- **Application Interceptors**: Added via addInterceptor(), they see every request once and are good for logging, auth addition, and error handling
- **Network Interceptors**: Added via addNetworkInterceptor(), they see network traffic including retries and redirects, useful for caching and monitoring

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.OkHttpClient

// Basic interceptor structure
class BasicInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        // Get the original request
        val request = chain.request()
        
        // Log the request (see logging section)
        println("Sending request to: ${request.url}")
        
        // Proceed with the request - this calls the next interceptor or the network
        val response = chain.proceed(request)
        
        // Log the response
        println("Received response: ${response.code}")
        
        // Return or modify the response
        return response
    }
}

// Adding interceptors to OkHttpClient
class InterceptorSetup {
    
    fun createClientWithInterceptors(): OkHttpClient {
        return OkHttpClient.Builder()
            // Application interceptor - executes once per request
            .addInterceptor(BasicInterceptor())
            // Another application interceptor
            .addInterceptor(AnotherInterceptor())
            // Network interceptor - can execute multiple times
            .addNetworkInterceptor(NetworkInterceptor())
            .build()
    }
}

class AnotherInterceptor : Interceptor {
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        // Modify request before sending
        val modifiedRequest = request.newBuilder()
            .header("X-Custom-Header", "value")
            .build()
        return chain.proceed(modifiedRequest)
    }
}

class NetworkInterceptor : Interceptor {
    override fun intercept(chain: Interceptor.Chain): Response {
        // Network-specific operations
        return chain.proceed(chain.request())
    }
}
```

## Section 2: Request and Response Modification

Interceptors can modify requests before they are sent and responses before they are returned to the calling code.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.MediaType
import okhttp3.Headers
import okio.Buffer

// Interceptor for modifying request headers
class RequestHeaderModifier : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val originalRequest = chain.request()
        
        // Build new request with modified headers
        val modifiedRequest = originalRequest.newBuilder()
            // Add new headers
            .addHeader("X-Request-ID", generateRequestId())
            .addHeader("X-Device-ID", getDeviceId())
            .addHeader("Accept-Language", "en-US,en;q=0.9")
            
            // Modify existing headers
            .header("User-Agent", "MyApp/1.0.0 (Android)")
            
            // Remove headers
            .removeHeader("X-Debug-Header")
            
            // Add authentication
            .addHeader("Authorization", "Bearer ${getAuthToken()}")
            
            .method(originalRequest.method, originalRequest.body)
            .build()
        
        return chain.proceed(modifiedRequest)
    }
    
    private fun generateRequestId(): String = java.util.UUID.randomUUID().toString()
    private fun getDeviceId(): String = "device-${android.os.Build.MODEL}"
    private fun getAuthToken(): String = "token-placeholder"
}

// Interceptor for modifying response
class ResponseModifier : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val response = chain.proceed(chain.request())
        
        // Check response status before modification
        if (response.code == 200) {
            // Return response with modified headers
            return response.newBuilder()
                .header("X-Response-Cache-Time", System.currentTimeMillis().toString())
                .header("X-App-Processed", "true")
                .build()
        }
        
        // Return original response for errors
        return response
    }
}

// Interceptor to handle specific status codes
class StatusCodeHandler : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val response = chain.proceed(chain.request())
        
        when (response.code) {
            401 -> {
                // Handle unauthorized - could trigger re-authentication
                handleUnauthorized(response)
            }
            403 -> {
                // Handle forbidden
                handleForbidden(response)
            }
            503 -> {
                // Handle service unavailable - retry later
                handleServiceUnavailable(response)
            }
        }
        
        return response
    }
    
    private fun handleUnauthorized(response: Response) {
        // Notify user to login again
    }
    
    private fun handleForbidden(response: Response) {
        // Show access denied message
    }
    
    private fun handleServiceUnavailable(response: Response) {
        // Show retry message
    }
}
```

## Section 3: Authentication Interceptors

Authentication interceptors add tokens or credentials to requests automatically, handling token refresh and error scenarios.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor

// Token provider interface
interface TokenProvider {
    suspend fun getAccessToken(): String?
    suspend fun refreshToken(): String?
    fun isTokenExpired(): Boolean
}

// Session manager for token handling
class SessionManager(private val secureStorage: SecureStorage) : TokenProvider {
    
    private var cachedToken: String? = null
    private var tokenExpiry: Long = 0
    
    override suspend fun getAccessToken(): String? {
        // Return cached token if valid
        if (!isTokenExpired() && cachedToken != null) {
            return cachedToken
        }
        
        // Try to load from storage
        cachedToken = secureStorage.getToken()
        
        return cachedToken
    }
    
    override suspend fun refreshToken(): String? {
        // Request new token from auth server
        val newToken = secureStorage.refreshToken()
        cachedToken = newToken
        return newToken
    }
    
    override fun isTokenExpired(): Boolean {
        return System.currentTimeMillis() > tokenExpiry
    }
}

class SecureStorage {
    suspend fun getToken(): String? = null
    suspend fun refreshToken(): String? = null
}

// Authentication interceptor with token refresh
class AuthInterceptor(private val tokenProvider: TokenProvider) : Interceptor {
    
    companion object {
        private const val AUTH_HEADER = "Authorization"
        private const val BEARER_PREFIX = "Bearer "
    }
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val originalRequest = chain.request()
        
        // Skip auth for auth endpoints
        if (originalRequest.url.encodedPath.contains("/auth/")) {
            return chain.proceed(originalRequest)
        }
        
        // Get current token
        val token = runCatching {
            kotlinx.coroutines.runBlocking {
                tokenProvider.getAccessToken()
            }
        }.getOrNull()
        
        // Build authenticated request
        val authenticatedRequest = if (token != null) {
            originalRequest.newBuilder()
                .header(AUTH_HEADER, BEARER_PREFIX + token)
                .build()
        } else {
            originalRequest
        }
        
        // Proceed with request
        val response = chain.proceed(authenticatedRequest)
        
        // Handle 401 - token expired
        if (response.code == 401) {
            response.close()
            
            // Attempt to refresh token
            val newToken = runCatching {
                kotlinx.coroutines.runBlocking {
                    tokenProvider.refreshToken()
                }
            }.getOrNull()
            
            if (newToken != null) {
                // Retry with new token
                val retryRequest = originalRequest.newBuilder()
                    .header(AUTH_HEADER, BEARER_PREFIX + newToken)
                    .build()
                
                return chain.proceed(retryRequest)
            }
        }
        
        return response
    }
}

// API Key interceptor for public APIs
class ApiKeyInterceptor(private val apiKey: String) : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val originalRequest = chain.request()
        
        // Add API key as query parameter
        val urlWithKey = originalRequest.url.newBuilder()
            .addQueryParameter("api_key", apiKey)
            .build()
        
        val requestWithKey = originalRequest.newBuilder()
            .url(urlWithKey)
            .build()
        
        return chain.proceed(requestWithKey)
    }
}

// Basic authentication interceptor
class BasicAuthInterceptor(private val username: String, private val password: String) : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val credentials = okhttp3.Credentials.basic(username, password)
        
        val request = chain.request().newBuilder()
            .header("Authorization", credentials)
            .build()
        
        return chain.proceed(request)
    }
}
```

## Section 4: Logging and Debugging Interceptors

Logging interceptors are essential for debugging network issues and monitoring API behavior.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.MediaType
import okio.Buffer
import java.text.SimpleDateFormat
import java.util.Date
import java.util.Locale

// Comprehensive logging interceptor
class DetailedLoggingInterceptor : Interceptor {
    
    private val dateFormat = SimpleDateFormat("HH:mm:ss.SSS", Locale.getDefault())
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        
        // Log request details
        logRequest(request)
        
        val startTime = System.nanoTime()
        
        // Proceed with request
        val response = chain.proceed(request)
        
        val endTime = System.nanoTime()
        val durationMs = (endTime - startTime) / 1_000_000
        
        // Log response details
        logResponse(response, durationMs)
        
        return response
    }
    
    private fun logRequest(request: Request) {
        println("┌─────────────────────────────────────────────────────────────")
        println("│ REQUEST")
        println("├─────────────────────────────────────────────────────────────")
        println("│ ${request.method} ${request.url}")
        println("│ Headers:")
        request.headers.forEach { (name, value) ->
            println("│   $name: $value")
        }
        
        // Log body for non-null bodies
        request.body?.let { body ->
            if (body.isNotRepeatable()) {
                println("│ Body: (non-repeatable)")
            } else {
                println("│ Body: ${bodyToString(body)}")
            }
        }
    }
    
    private fun logResponse(response: Response, durationMs: Long) {
        println("├─────────────────────────────────────────────────────────────")
        println("│ RESPONSE (${durationMs}ms)")
        println("├─────────────────────────────────────────────────────────────")
        println("│ ${response.request.url}")
        println("│ Status: ${response.code} ${response.message}")
        println("│ Headers:")
        response.headers.forEach { (name, value) ->
            println("│   $name: $value")
        }
        
        // Log response body (careful with large responses)
        response.body?.let { body ->
            val source = body.source()
            source.request(Long.MAX_VALUE)
            val buffer = source.buffer.clone()
            val bodyString = buffer.readUtf8()
            if (bodyString.length < 1000) {
                println("│ Body: $bodyString")
            } else {
                println("│ Body: (${bodyString.length} bytes)")
            }
        }
        println("└─────────────────────────────────────────────────────────────")
    }
    
    private fun bodyToString(body: RequestBody): String {
        val buffer = Buffer()
        body.writeTo(buffer)
        return buffer.readUtf8()
    }
}

// Conditional logging interceptor
class ConditionalLoggingInterceptor(private val logLevel: LogLevel) : Interceptor {
    
    enum class LogLevel {
        NONE,
        BASIC,
        HEADERS,
        BODY
    }
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        
        when (logLevel) {
            LogLevel.NONE -> {}
            LogLevel.BASIC -> logBasic(request)
            LogLevel.HEADERS -> logHeaders(request)
            LogLevel.BODY -> logBody(request)
        }
        
        val startTime = System.currentTimeMillis()
        val response = chain.proceed(request)
        val duration = System.currentTimeMillis() - startTime
        
        println("Request completed in ${duration}ms with status ${response.code}")
        
        return response
    }
    
    private fun logBasic(request: Request) {
        println("--> ${request.method} ${request.url.encodedPath}")
    }
    
    private fun logHeaders(request: Request) {
        println("--> ${request.method} ${request.url.encodedPath}")
        println("Headers: ${request.headers}")
    }
    
    private fun logBody(request: Request) {
        println("--> ${request.method} ${request.url.encodedPath}")
        request.body?.let {
            println("Body: ${it}")
        }
    }
}
```

## Section 5: Caching Interceptors

Caching interceptors enable offline functionality and reduce network load by serving cached responses.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.CacheControl
import okhttp3.Headers
import java.util.concurrent.TimeUnit
import android.content.Context

// Force cache interceptor - serves cached response when offline
class ForceCacheInterceptor(private val context: Context) : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        var request = chain.request()
        
        // Check if network is available
        if (!isNetworkAvailable()) {
            // Modify request to only accept cached responses
            val cacheControl = CacheControl.Builder()
                .onlyIfCached()
                .maxStale(7, TimeUnit.DAYS) // Accept stale cache up to 7 days
                .build()
            
            request = request.newBuilder()
                .cacheControl(cacheControl)
                .build()
        }
        
        return chain.proceed(request)
    }
    
    private fun isNetworkAvailable(): Boolean {
        val connectivityManager = context.getSystemService(Context.CONNECTIVITY_SERVICE) 
            as android.net.ConnectivityManager
        
        val network = connectivityManager.activeNetwork ?: return false
        val capabilities = connectivityManager.getNetworkCapabilities(network) ?: return false
        
        return capabilities.hasCapability(NetworkCapabilities.NET_CAPABILITY_INTERNET)
    }
}

// Cache control interceptor - adds cache headers
class CacheControlHeadersInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        
        // Add cache control to request
        val modifiedRequest = request.newBuilder()
            .cacheControl(CacheControl.Builder()
                .maxAge(5, TimeUnit.MINUTES)
                .build())
            .build()
        
        val response = chain.proceed(modifiedRequest)
        
        // Cache response for specified duration
        return response.newBuilder()
            .cacheControl(CacheControl.Builder()
                .maxAge(5, TimeUnit.MINUTES)
                .build())
            .build()
    }
}

// Network-first with cache fallback
class NetworkFirstWithCacheFallback : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        
        try {
            // Try network first
            val networkResponse = chain.proceed(request)
            
            if (networkResponse.isSuccessful) {
                return networkResponse
            }
            
            networkResponse.close()
            
            // Fall back to cache
            return getCachedResponse(request) ?: networkResponse
        } catch (e: Exception) {
            // Network failed, try cache
            return getCachedResponse(request) ?: throw e
        }
    }
    
    private fun getCachedResponse(request: Request): Response? {
        // Implementation depends on cache setup
        return null
    }
}
```

## Section 6: Production Example - Complete Interceptor Chain

This example demonstrates a production-ready interceptor chain with authentication, logging, caching, and error handling.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.CacheControl
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import kotlinx.coroutines.runBlocking
import java.util.concurrent.TimeUnit

// Complete interceptor chain for production
object InterceptorChainFactory {
    
    fun createProductionChain(context: Context, sessionManager: SessionManager): OkHttpClient {
        return OkHttpClient.Builder()
            // 1. Authentication - must be first to add tokens
            .addInterceptor(AuthInterceptor(sessionManager))
            
            // 2. Request modification
            .addInterceptor(RequestHeadersInterceptor())
            
            // 3. Cache - for offline support
            .addInterceptor(ForceCacheInterceptor(context))
            
            // 4. Logging - last for application layer
            .addInterceptor(createLoggingInterceptor())
            
            // Network interceptors
            .addNetworkInterceptor(CacheControlNetworkInterceptor())
            .addNetworkInterceptor(NetworkMonitoringInterceptor())
            
            // Timeouts
            .connectTimeout(15, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .writeTimeout(30, TimeUnit.SECONDS)
            
            .build()
    }
    
    private fun createLoggingInterceptor(): HttpLoggingInterceptor {
        return HttpLoggingInterceptor().apply {
            level = HttpLoggingInterceptor.Level.BODY
        }
    }
}

// Request headers interceptor
class RequestHeadersInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        
        val modifiedRequest = request.newBuilder()
            .header("Accept", "application/json")
            .header("Content-Type", "application/json")
            .header("X-App-Version", "1.0.0")
            .header("X-Platform", "Android")
            .header("X-Device-Model", android.os.Build.MODEL)
            .header("X-OS-Version", android.os.Build.VERSION.RELEASE)
            .header("Accept-Language", java.util.Locale.getDefault().language)
            .build()
        
        return chain.proceed(modifiedRequest)
    }
}

// Cache control network interceptor
class CacheControlNetworkInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val response = chain.proceed(chain.request())
        
        return response.newBuilder()
            .removeHeader("Pragma")
            .removeHeader("Cache-Control")
            .header("Cache-Control", "public, max-age=300") // 5 minutes
            .build()
    }
}

// Network monitoring interceptor
class NetworkMonitoringInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        val startTime = System.nanoTime()
        
        val response = chain.proceed(request)
        
        val endTime = System.nanoTime()
        val duration = (endTime - startTime) / 1_000_000
        
        // Log slow requests
        if (duration > 3000) {
            println("SLOW REQUEST: ${request.url} took ${duration}ms")
        }
        
        return response
    }
}

// Error handling interceptor
class ErrorHandlingInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        
        return try {
            val response = chain.proceed(request)
            
            when (response.code) {
                401 -> throw AuthenticationException("Unauthorized")
                403 -> throw AuthorizationException("Forbidden")
                404 -> throw NotFoundException("Not found: ${request.url}")
                in 500..599 -> throw ServerException("Server error: ${response.code}")
            }
            
            response
        } catch (e: Exception) {
            throw NetworkException("Network error: ${e.message}")
        }
    }
}

class AuthenticationException(message: String) : Exception(message)
class AuthorizationException(message: String) : Exception(message)
class NotFoundException(message: String) : Exception(message)
class ServerException(message: String) : Exception(message)
class NetworkException(message: String) : Exception(message)
```

## Section 7: Retry and Circuit Breaker Patterns

Implement retry logic with exponential backoff and circuit breaker patterns for resilience.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import kotlin.math.min

// Retry interceptor with exponential backoff
class RetryInterceptor(
    private val maxRetries: Int = 3,
    private val initialDelayMs: Long = 1000,
    private val maxDelayMs: Long = 10000
) : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        var lastException: Exception? = null
        
        for (attempt in 0 until maxRetries) {
            try {
                val response = chain.proceed(request)
                
                // Retry on server errors
                if (response.code in 500..599 && attempt < maxRetries - 1) {
                    response.close()
                    val delay = calculateDelay(attempt)
                    Thread.sleep(delay)
                    continue
                }
                
                return response
            } catch (e: Exception) {
                lastException = e
                
                // Don't retry on client errors (except rate limiting)
                if (e is java.net.HttpRetryException) {
                    throw e
                }
                
                if (attempt < maxRetries - 1) {
                    val delay = calculateDelay(attempt)
                    Thread.sleep(delay)
                }
            }
        }
        
        throw lastException ?: Exception("Request failed after $maxRetries attempts")
    }
    
    private fun calculateDelay(attempt: Int): Long {
        val delay = initialDelayMs * (1 shl attempt) // Exponential backoff
        return min(delay, maxDelayMs)
    }
}

// Circuit breaker interceptor
class CircuitBreakerInterceptor : Interceptor {
    
    private var failureCount = 0
    private var lastFailureTime = 0L
    private val failureThreshold = 5
    private val recoveryTimeoutMs = 30000L
    
    @Volatile
    private var isOpen = false
    
    override fun intercept(chain: Interceptor.Chain): Response {
        // Check if circuit is open
        if (isOpen) {
            if (System.currentTimeMillis() - lastFailureTime > recoveryTimeoutMs) {
                // Half-open: allow one request to test
                isOpen = false
            } else {
                throw CircuitBreakerOpenException("Circuit breaker is open")
            }
        }
        
        return try {
            val response = chain.proceed(chain.request())
            
            // Success - reset failure count
            synchronized(this) {
                failureCount = 0
            }
            
            response
        } catch (e: Exception) {
            // Record failure
            synchronized(this) {
                failureCount++
                lastFailureTime = System.currentTimeMillis()
                
                if (failureCount >= failureThreshold) {
                    isOpen = true
                }
            }
            
            throw e
        }
    }
}

class CircuitBreakerOpenException(message: String) : Exception(message)
```

## Best Practices

- **Order Matters**: Place authentication interceptors first, then request modifiers, cache, and logging last
- **Close Responses**: Always close response bodies to prevent resource leaks
- **Handle Exceptions**: Wrap network operations in try-catch for graceful error handling
- **Avoid Blocking**: Don't perform long-running operations in interceptors
- **Use Network Interceptors Carefully**: They run after connection processing, not for cached responses
- **Implement Retry Logic**: Use exponential backoff to avoid overwhelming servers
- **Log Appropriately**: Disable detailed logging in production for security
- **Test Interceptors**: Write unit tests for interceptor logic separately

## Common Pitfalls

**Problem**: Interceptor causes infinite loop
**Solution**: Ensure you don't modify the request URL to point back to the same URL

**Problem**: Responses not being cached
**Solution**: Add proper Cache-Control headers and configure OkHttp cache

**Problem**: Token refresh loop
**Solution**: Add flag to prevent retry on 401 after refresh attempt

**Problem**: Memory leaks in interceptors
**Solution**: Close response bodies properly, avoid capturing large objects

**Problem**: Slow network requests
**Solution**: Implement caching and timeout configuration

## Troubleshooting Guide

**Q: Why is my interceptor not being called?**
A: Verify interceptor is added to OkHttpClient.Builder and client is being used

**Q: How do I debug interceptor issues?**
A: Use logging interceptor to see request/response details

**Q: Why are responses cached indefinitely?**
A: Check Cache-Control headers are being set properly

**Q: How to skip interceptor for certain requests?**
A: Add header checked in interceptor or use different OkHttpClient

## Advanced Tips

- **Request Tagging**: Use request.tag() to identify requests in interceptors
- **Conditional Interceptors**: Add interceptors based on build variants or features
- **Interceptor Chaining**: Combine multiple interceptors for complex scenarios
- **Mock Interceptors**: Use interceptors to mock responses in testing

## Cross-References

- [OkHttp Configuration](./02_OkHttp_Configuration.md) - Client configuration
- [Retrofit Basics](./01_Retrofit_Basics.md) - HTTP client setup
- [Authentication Implementation](./04_Authentication_Implementation.md) - Token management
- [Error Handling Strategies](./05_Error_Handling_Strategies.md) - Network error handling
- [Background Threading](../02_Asynchronous_Patterns/05_Background_Threading.md) - Thread management
