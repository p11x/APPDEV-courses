# Network Analysis

## Learning Objectives

1. Understanding network traffic analysis
2. Using network profiler effectively
3. Identifying network performance issues
4. Debugging API calls and responses
5. Optimizing network usage

```kotlin
package com.kotlin.debugging.network
```

---

## Prerequisites

- See: 09_PERFORMANCE/01_Performance_Optimization/05_Network_Optimization.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md
- See: 06_NETWORKING/01_HTTP_Communication/01_Retrofit_Basics.md

---

## Core Concepts

### Network Analysis Overview

- **Request/Response Monitoring**: Track all HTTP traffic
- **Timing Analysis**: Measure request latency
- **Payload Analysis**: Inspect request and response bodies
- **Connection Analysis**: Monitor connection reuse

### SECTION 1: Network Traffic Monitoring

```kotlin
/**
 * Network Traffic Monitoring
 * 
 * Setting up network traffic monitoring.
 */
class NetworkTrafficMonitor {
    
    // OkHttp event listener for detailed monitoring
    class NetworkEventListener : okhttp3.EventListener() {
        
        private val events = mutableListOf<NetworkEvent>()
        
        override fun callStart(call: okhttp3.Call) {
            events.add(NetworkEvent(
                eventType = EventType.CALL_START,
                timestamp = System.currentTimeMillis(),
                callId = call.request().url.toString()
            ))
        }
        
        override fun dnsStart(call: okhttp3.Call, domainName: String) {
            events.add(NetworkEvent(
                eventType = EventType.DNS_START,
                timestamp = System.currentTimeMillis(),
                detail = domainName
            ))
        }
        
        override fun dnsEnd(call: okhttp3.Call, domainName: String, inetAddressList: List<okhttp3.InetAddress>) {
            events.add(NetworkEvent(
                eventType = EventType.DNS_END,
                timestamp = System.currentTimeMillis(),
                detail = inetAddressList.joinToString { it.hostAddress }
            ))
        }
        
        override fun connectStart(call: okhttp3.Call, inetSocketAddress: okhttp3.InetSocketAddress, proxy: okhttp3.Proxy) {
            events.add(NetworkEvent(
                eventType = EventType.CONNECT_START,
                timestamp = System.currentTimeMillis(),
                detail = "${inetSocketAddress.address}:${inetSocketAddress.port}"
            ))
        }
        
        override fun connectEnd(call: okhttp3.Call, protocol: okhttp3.Protocol, http2Connection: okhttp3.Http2Connection?) {
            events.add(NetworkEvent(
                eventType = EventType.CONNECT_END,
                timestamp = System.currentTimeMillis(),
                detail = protocol.name
            ))
        }
        
        override fun requestHeadersEnd(call: okhttp3.Call, request: okhttp3.Request) {
            events.add(NetworkEvent(
                eventType = EventType.REQUEST_HEADERS_END,
                timestamp = System.currentTimeMillis()
            ))
        }
        
        override fun requestBodyEnd(call: okhttp3.Call, byteCount: Long) {
            events.add(NetworkEvent(
                eventType = EventType.REQUEST_BODY_END,
                timestamp = System.currentTimeMillis(),
                bytes = byteCount
            ))
        }
        
        override fun responseHeadersStart(call: okhttp3.Call, response: okhttp3.Response) {
            events.add(NetworkEvent(
                eventType = EventType.RESPONSE_HEADERS_START,
                timestamp = System.currentTimeMillis(),
                detail = response.code.toString()
            ))
        }
        
        override fun responseHeadersEnd(call: okhttp3.Call, response: okhttp3.Response) {
            val headerSize = response.headers.byteCount()
            events.add(NetworkEvent(
                eventType = EventType.RESPONSE_HEADERS_END,
                timestamp = System.currentTimeMillis(),
                bytes = headerSize.toLong()
            ))
        }
        
        override fun responseBodyStart(call: okhttp3.Call) {
            events.add(NetworkEvent(
                eventType = EventType.RESPONSE_BODY_START,
                timestamp = System.currentTimeMillis()
            ))
        }
        
        override fun responseBodyEnd(call: okhttp3.Call, byteCount: Long) {
            events.add(NetworkEvent(
                eventType = EventType.RESPONSE_BODY_END,
                timestamp = System.currentTimeMillis(),
                bytes = byteCount
            ))
        }
        
        override fun callEnd(call: okhttp3.Call) {
            events.add(NetworkEvent(
                eventType = EventType.CALL_END,
                timestamp = System.currentTimeMillis()
            ))
        }
        
        override fun callFailed(call: okhttp3.Call, ioe: java.io.IOException) {
            events.add(NetworkEvent(
                eventType = EventType.CALL_FAILED,
                timestamp = System.currentTimeMillis(),
                detail = ioe.message
            ))
        }
        
        fun getEvents(): List<NetworkEvent> = events.toList()
        
        fun clearEvents() = events.clear()
        
        data class NetworkEvent(
            val eventType: EventType,
            val timestamp: Long,
            val callId: String = "",
            val detail: String = "",
            val bytes: Long = 0
        )
        
        enum class EventType {
            CALL_START, DNS_START, DNS_END, CONNECT_START, CONNECT_END,
            REQUEST_HEADERS_END, REQUEST_BODY_END, RESPONSE_HEADERS_START,
            RESPONSE_HEADERS_END, RESPONSE_BODY_START, RESPONSE_BODY_END,
            CALL_END, CALL_FAILED
        }
    }
    
    // Request/Response logging interceptor
    class LoggingInterceptor : okhttp3.Interceptor {
        
        override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
            val request = chain.request()
            
            val startTime = System.nanoTime()
            
            logRequest(request)
            
            val response = chain.proceed(request)
            
            val endTime = System.nanoTime()
            val durationMs = (endTime - startTime) / 1_000_000
            
            logResponse(response, durationMs)
            
            return response
        }
        
        private fun logRequest(request: okhttp3.Request) {
            println("--> ${request.method} ${request.url}")
            println("Headers: ${request.headers}")
            request.body?.let {
                println("Body: ${it.contentType()} - ${it.contentLength()} bytes")
            }
        }
        
        private fun logResponse(response: okhttp3.Response, durationMs: Long) {
            println("<-- ${response.code} ${response.message} (${durationMs}ms)")
            println("Headers: ${response.headers}")
            response.body?.let {
                println("Body: ${it.contentType()} - ${it.contentLength()} bytes")
            }
        }
    }
}
```

---

## SECTION 2: Performance Analysis

```kotlin
/**
 * Network Performance Analysis
 * 
 * Analyzing network performance metrics.
 */
class NetworkPerformanceAnalyzer {
    
    // Request timing breakdown
    class RequestTiming {
        
        data class Timing(
            val dnsLookup: Long = 0,      // DNS resolution
            val tcpConnect: Long = 0,      // TCP handshake
            val tlsHandshake: Long = 0,   // SSL/TLS
            val requestSend: Long = 0,    // Send request
            val waitingTTFB: Long = 0,    // Time to first byte
            val contentDownload: Long = 0 // Download response
        ) {
            val total: Long get() = dnsLookup + tcpConnect + tlsHandshake + 
                requestSend + waitingTTFB + contentDownload
        }
        
        fun calculateFromEvents(events: List<NetworkEventListener.NetworkEvent>): Timing {
            // Calculate timing from network events
            var dnsStart = 0L
            var dnsEnd = 0L
            var connectStart = 0L
            var connectEnd = 0L
            var requestEnd = 0L
            var responseStart = 0L
            var responseEnd = 0L
            
            events.forEach { event ->
                when (event.eventType) {
                    NetworkEventListener.EventType.DNS_START -> dnsStart = event.timestamp
                    NetworkEventListener.EventType.DNS_END -> dnsEnd = event.timestamp
                    NetworkEventListener.EventType.CONNECT_START -> connectStart = event.timestamp
                    NetworkEventListener.EventType.CONNECT_END -> connectEnd = event.timestamp
                    NetworkEventListener.EventType.REQUEST_BODY_END -> requestEnd = event.timestamp
                    NetworkEventListener.EventType.RESPONSE_HEADERS_START -> responseStart = event.timestamp
                    NetworkEventListener.EventType.RESPONSE_BODY_END -> responseEnd = event.timestamp
                    else -> {}
                }
            }
            
            return Timing(
                dnsLookup = dnsEnd - dnsStart,
                tcpConnect = connectEnd - connectStart,
                contentDownload = responseEnd - responseStart
            )
        }
    }
    
    // Slow request detector
    class SlowRequestDetector(
        private val thresholdMs: Long = 1000
    ) {
        
        private val slowRequests = mutableListOf<SlowRequestInfo>()
        
        fun recordRequest(
            url: String,
            method: String,
            durationMs: Long,
            statusCode: Int
        ) {
            if (durationMs > thresholdMs) {
                slowRequests.add(SlowRequestInfo(
                    url = url,
                    method = method,
                    durationMs = durationMs,
                    statusCode = statusCode,
                    timestamp = System.currentTimeMillis()
                ))
            }
        }
        
        fun getSlowRequests(): List<SlowRequestInfo> = slowRequests.toList()
        
        fun getAverageDuration(): Long {
            return if (slowRequests.isEmpty()) 0
            else slowRequests.map { it.durationMs }.average().toLong()
        }
        
        data class SlowRequestInfo(
            val url: String,
            val method: String,
            val durationMs: Long,
            val statusCode: Int,
            val timestamp: Long
        )
    }
    
    // Request size analyzer
    class RequestSizeAnalyzer {
        
        data class SizeInfo(
            val requestHeaders: Long = 0,
            val requestBody: Long = 0,
            val responseHeaders: Long = 0,
            val responseBody: Long = 0
        ) {
            val total: Long get() = requestHeaders + requestBody + 
                responseHeaders + responseBody
        }
        
        private val sizeHistory = mutableListOf<SizeInfo>()
        
        fun recordSize(info: SizeInfo) {
            sizeHistory.add(info)
        }
        
        fun getLargestRequests(): List<Pair<String, Long>> {
            // Return top requests by size
            return sizeHistory.sortedByDescending { it.total }
                .take(10)
                .map { "Request" to it.total }
        }
        
        fun getAverageRequestSize(): Long {
            return if (sizeHistory.isEmpty()) 0
            else sizeHistory.map { it.requestBody }.average().toLong()
        }
        
        fun getAverageResponseSize(): Long {
            return if (sizeHistory.isEmpty()) 0
            else sizeHistory.map { it.responseBody }.average().toLong()
        }
    }
    
    // Error rate monitor
    class ErrorRateMonitor {
        
        private var totalRequests = 0
        private var failedRequests = 0
        
        fun recordRequest(success: Boolean) {
            totalRequests++
            if (!success) failedRequests++
        }
        
        fun getErrorRate(): Float {
            return if (totalRequests == 0) 0f
            else failedRequests.toFloat() / totalRequests.toFloat()
        }
        
        fun reset() {
            totalRequests = 0
            failedRequests = 0
        }
    }
}
```

---

## SECTION 3: Debugging Network Issues

```kotlin
/**
 * Network Debugging
 * 
 * Debugging common network issues.
 */
class NetworkDebugger {
    
    // Analyze HTTP errors
    class HTTPErrorAnalyzer {
        
        fun analyzeError(response: okhttp3.Response): ErrorAnalysis {
            val code = response.code
            val message = response.message
            val headers = response.headers
            
            return when (code) {
                in 400..499 -> analyzeClientError(code, message, headers)
                in 500..599 -> analyzeServerError(code, message, headers)
                else -> ErrorAnalysis(code, message, "Unknown error", null)
            }
        }
        
        private fun analyzeClientError(
            code: Int,
            message: String,
            headers: okhttp3.Headers
        ): ErrorAnalysis {
            val (type, suggestion) = when (code) {
                400 -> "Bad Request" to "Check request parameters and body format"
                401 -> "Unauthorized" to "Check authentication token"
                403 -> "Forbidden" to "Check user permissions"
                404 -> "Not Found" to "Check API endpoint URL"
                409 -> "Conflict" to "Check for duplicate data"
                422 -> "Unprocessable" to "Check validation rules"
                429 -> "Too Many Requests" to "Implement rate limiting"
                else -> "Client Error" to "Review request"
            }
            
            return ErrorAnalysis(code, message, type, suggestion)
        }
        
        private fun analyzeServerError(
            code: Int,
            message: String,
            headers: okhttp3.Headers
        ): ErrorAnalysis {
            return ErrorAnalysis(
                code = code,
                message = message,
                type = "Server Error",
                suggestion = "Server issue - retry later or contact backend"
            )
        }
        
        data class ErrorAnalysis(
            val code: Int,
            val message: String,
            val errorType: String,
            val suggestion: String?
        )
    }
    
    // Network connectivity checker
    class ConnectivityChecker(private val context: android.content.Context) {
        
        private val connectivityManager = context.getSystemService(
            android.content.Context.CONNECTIVITY_SERVICE
        ) as android.net.ConnectivityManager
        
        fun isConnected(): Boolean {
            val network = connectivityManager.activeNetwork ?: return false
            val capabilities = connectivityManager.getNetworkCapabilities(network) ?: return false
            return capabilities.hasCapability(android.net.NetworkCapabilities.NET_CAPABILITY_INTERNET)
        }
        
        fun getNetworkType(): NetworkType {
            val network = connectivityManager.activeNetwork ?: return NetworkType.NONE
            val capabilities = connectivityManager.getNetworkCapabilities(network)
            
            return when {
                capabilities?.hasTransport(android.net.NetworkCapabilities.TRANSPORT_WIFI) == true -> 
                    NetworkType.WIFI
                capabilities?.hasTransport(android.net.NetworkCapabilities.TRANSPORT_CELLULAR) == true -> 
                    NetworkType.MOBILE
                capabilities?.hasTransport(android.net.NetworkCapabilities.TRANSPORT_ETHERNET) == true -> 
                    NetworkType.ETHERNET
                else -> NetworkType.NONE
            }
        }
        
        fun isMetered(): Boolean {
            return connectivityManager.isActiveNetworkMetered
        }
        
        enum class NetworkType { WIFI, MOBILE, ETHERNET, NONE }
    }
    
    // Retry logic debugger
    class RetryDebugger {
        
        data class RetryInfo(
            val attempt: Int,
            val success: Boolean,
            val error: String?,
            val durationMs: Long
        )
        
        private val retryHistory = mutableListOf<RetryInfo>()
        
        fun recordAttempt(attempt: Int, success: Boolean, error: String?, durationMs: Long) {
            retryHistory.add(RetryInfo(attempt, success, error, durationMs))
        }
        
        fun getRetryStats(): RetryStats {
            val totalAttempts = retryHistory.size
            val successfulRetries = retryHistory.count { it.attempt > 1 && it.success }
            val failedRetries = retryHistory.count { it.attempt > 1 && !it.success }
            
            return RetryStats(
                totalAttempts = totalAttempts,
                successfulRetries = successfulRetries,
                failedRetries = failedRetries,
                avgRetriesPerRequest = if (totalAttempts > 0) totalAttempts.toFloat() / totalRequests() else 0f
            )
        }
        
        private fun totalRequests(): Int {
            return retryHistory.groupBy { /* request ID */ }.size
        }
        
        data class RetryStats(
            val totalAttempts: Int,
            val successfulRetries: Int,
            val failedRetries: Int,
            val avgRetriesPerRequest: Float
        )
    }
}
```

---

## Best Practices

1. **Use OkHttp EventListener**: For detailed request/response tracking
2. **Monitor Request Timing**: Identify slow API calls
3. **Track Error Rates**: Detect issues early
4. **Analyze Request Sizes**: Optimize payloads
5. **Check Connectivity State**: Handle offline gracefully
6. **Log Network Events**: Use interceptor for debugging
7. **Use Charles/Postman**: External debugging for complex scenarios
8. **Profile in Production**: Firebase Performance for real-world data
9. **Implement Retry Logic**: Handle transient failures
10. **Use Caching**: Reduce network calls

---

## Common Pitfalls and Solutions

### Pitfall 1: Large Request Payloads
- **Problem**: Sending too much data
- **Solution**: Use compression, pagination, field selection

### Pitfall 2: No Error Handling
- **Problem**: Unhandled network errors crash app
- **Solution**: Implement proper error handling and retry

### Pitfall 3: Not Checking Connectivity
- **Problem**: Requests fail silently when offline
- **Solution**: Check connectivity before making requests

### Pitfall 4: Missing Timeout
- **Problem**: Requests hang indefinitely
- **Solution**: Set connect and read timeouts

### Pitfall 5: No Caching
- **Problem**: Repeated requests for same data
- **Solution**: Implement HTTP and disk caching

---

## Troubleshooting Guide

### Issue: Request Taking Too Long
- **Steps**: 1. Check DNS 2. Check TCP connection 3. Check server response time

### Issue: High Error Rate
- **Steps**: 1. Categorize errors 2. Check server logs 3. Fix root cause

---

## EXAMPLE 1: OkHttp with Full Logging

```kotlin
/**
 * Complete OkHttp Setup with Network Monitoring
 * 
 * Production-ready OkHttp with comprehensive logging.
 */
class CompleteOkHttpSetup {
    
    class MonitoredOkHttpClient(private val context: android.content.Context) {
        
        private val eventListener = NetworkEventListener()
        
        private val okHttpClient: okhttp3.OkHttpClient by lazy {
            createClient()
        }
        
        private fun createClient(): okhttp3.OkHttpClient {
            return okhttp3.OkHttpClient.Builder()
                .connectTimeout(30, java.util.concurrent.TimeUnit.SECONDS)
                .readTimeout(30, java.util.concurrent.TimeUnit.SECONDS)
                .writeTimeout(30, java.util.concurrent.TimeUnit.SECONDS)
                .retryOnConnectionFailure(true)
                .eventListenerFactory(NetworkEventListener.Factory())
                .addInterceptor(createLoggingInterceptor())
                .addInterceptor(createErrorHandlingInterceptor())
                .addNetworkInterceptor(createNetworkStateInterceptor())
                .cache(createCache())
                .build()
        }
        
        private fun createLoggingInterceptor(): okhttp3.Interceptor {
            return okhttp3.Interceptor { chain ->
                val request = chain.request()
                val startTime = System.currentTimeMillis()
                
                println("--> ${request.method} ${request.url}")
                println("Headers: ${request.headers}")
                
                val response = try {
                    chain.proceed(request)
                } catch (e: Exception) {
                    println("<-- ERROR: ${e.message}")
                    throw e
                }
                
                val duration = System.currentTimeMillis() - startTime
                println("<-- ${response.code} ${response.message} (${duration}ms)")
                
                response
            }
        }
        
        private fun createErrorHandlingInterceptor(): okhttp3.Interceptor {
            return okhttp3.Interceptor { chain ->
                val response = try {
                    chain.proceed(chain.request())
                } catch (e: java.net.UnknownHostException) {
                    throw NetworkException("No internet connection", e)
                } catch (e: java.net.SocketTimeoutException) {
                    throw NetworkException("Request timed out", e)
                } catch (e: java.io.IOException) {
                    throw NetworkException("Network error", e)
                }
                
                if (!response.isSuccessful) {
                    println("Error response: ${response.code} - ${response.message}")
                }
                
                response
            }
        }
        
        private fun createNetworkStateInterceptor(): okhttp3.Interceptor {
            return okhttp3.Interceptor { chain ->
                val connectivityManager = context.getSystemService(
                    android.content.Context.CONNECTIVITY_SERVICE
                ) as android.net.ConnectivityManager
                
                val network = connectivityManager.activeNetwork
                val capabilities = connectivityManager.getNetworkCapabilities(network)
                
                if (capabilities?.hasCapability(android.net.NetworkCapabilities.NET_CAPABILITY_INTERNET) != true) {
                    throw NetworkException("No internet connection")
                }
                
                chain.proceed(chain.request())
            }
        }
        
        private fun createCache(): okhttp3.Cache {
            val cacheDir = java.io.File(context.cacheDir, "http_cache")
            return okhttp3.Cache(cacheDir, 10L * 1024 * 1024)  // 10 MB
        }
        
        fun getClient(): okhttp3.OkHttpClient = okHttpClient
        
        fun getEvents(): List<NetworkEventListener.NetworkEvent> = eventListener.getEvents()
        
        class NetworkException(message: String, cause: Throwable? = null) : 
            java.lang.Exception(message, cause)
    }
    
    // Retrofit service with monitoring
    class MonitoredRetrofit(
        private val okHttpClient: okhttp3.OkHttpClient
    ) {
        
        private val retrofit: retrofit2.Retrofit by lazy {
            retrofit2.Retrofit.Builder()
                .baseUrl("https://api.example.com/")
                .client(okHttpClient)
                .addConverterFactory(retrofit2.converter.gson.GsonConverterFactory.create())
                .addCallAdapterFactory(retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory.create())
                .build()
        }
        
        fun <T> createService(serviceClass: Class<T>): T {
            return retrofit.create(serviceClass)
        }
    }
}
```

---

## EXAMPLE 2: Stetho Integration

```kotlin
/**
 * Stetho Integration
 * 
 * Using Stetho for network debugging with Chrome DevTools.
 */
class StethoIntegration {
    
    // Initialize Stetho in Application
    class StethoInitializer {
        
        fun initialize(context: android.content.Context) {
            com.facebook.stetho.Stetho.initializeWithDefaults(context)
        }
        
        // Add to build.gradle:
        // debugImplementation 'com.facebook.stetho:stetho:1.6.0'
        // debugImplementation 'com.facebook.stetho:stetho-okhttp3:1.6.0'
    }
    
    // Stetho interceptor for OkHttp
    class StethoInterceptor : okhttp3.Interceptor {
        
        override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
            return com.facebook.stetho.okhttp3.StethoInterceptor.intercept(chain)
        }
    }
    
    // Configure Stetho with custom options
    class StethoConfiguration {
        
        fun configureStetho(context: android.content.Context) {
            val initializer = com.facebook.stetho.Stetho.NewInstanceBuilder(context)
                .enableWebKitInspector(com.facebook.stetho.WebKitMeta(1))
                .enableDumpapp(com.facebook.stetho.dumpapp.DumperPluginsProvider { 
                    emptyList() 
                })
                .build()
            
            initializer.invoke()
        }
        
        // Enable specific modules
        class ModuleConfiguration {
            
            fun enableNetworkInspector(context: android.content.Context) {
                // Enable network inspection in Chrome:
                // 1. Open Chrome
                // 2. Navigate to chrome://inspect
                // 3. Find your device
                // 4. Click "Inspect"
            }
            
            fun enableDatabaseInspector(context: android.content.Context) {
                // View database contents in Chrome DevTools
            }
            
            fun enableSharedPreferencesInspector(context: android.content.Context) {
                // View SharedPreferences in Chrome DevTools
            }
        }
    }
    
    // Custom Dumper for app-specific debugging
    class CustomDumper {
        
        class MyDumperPlugin : com.facebook.stetho.dumpapp.DumperPlugin {
            
            override fun getName(): String = "myapp"
            
            override fun dump(
                args: com.facebook.stetho.dumpapp.DumperArgs,
                output: java.io.OutputStream
            ) {
                val writer = java.io.PrintWriter(output)
                writer.println("Custom dump output:")
                writer.println("App version: ${android.os.BuildConfig.VERSION_NAME}")
                writer.println("Network requests: ${getRequestCount()}")
                writer.flush()
            }
            
            private fun getRequestCount(): Int = 0
        }
    }
    
    // Use Stetho to inspect network traffic
    class NetworkInspection {
        
        fun inspectWithStetho() {
            // In Chrome DevTools:
            // 1. Open Network tab
            // 2. See all HTTP requests/responses
            // 3. Inspect headers, body, timing
            // 4. Export as HAR
        }
        
        // Chrome DevTools features:
        // - Request/response viewer
        // - Headers inspection
        // - Query parameters
        // - Request timing
        // - Response body preview
        // - Export to HAR
    }
}
```

---

## EXAMPLE 3: Firebase Performance Monitoring

```kotlin
/**
 * Firebase Performance Integration
 * 
 * Production network performance monitoring.
 */
class FirebasePerformanceMonitoring {
    
    // Initialize Firebase Performance
    class FirebasePerfInitializer {
        
        fun initialize() {
            // Add to build.gradle:
            // implementation 'com.google.firebase:firebase-perf-ktx'
            // implementation 'com.google.firebase:firebase-analytics-ktx'
            
            // Firebase Performance auto-collects:
            // - HTTP request timing
            // - Trace data
            // - App start time
        }
        
        // Enable custom traces
        fun enableCustomTraces() {
            // Manually create traces for important operations
        }
    }
    
    // Custom HTTP interceptor for Firebase
    class FirebasePerfInterceptor : okhttp3.Interceptor {
        
        private val tracer = com.google.firebase.perf.FirebasePerfManager.getInstance()
        
        override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
            val request = chain.request()
            val url = request.url.toString()
            
            // Create Firebase trace
            val trace = tracer.newTrace("network_$url")
            trace.start()
            
            try {
                val response = chain.proceed(request)
                
                // Record metrics
                trace.incrementMetric("response_code", response.code.toLong())
                trace.incrementMetric("response_size", response.body?.contentLength() ?: 0)
                
                return response
            } catch (e: Exception) {
                trace.incrementMetric("error_count", 1)
                throw e
            } finally {
                trace.stop()
            }
        }
    }
    
    // Manual trace for specific operations
    class ManualTraces {
        
        fun traceNetworkRequest(
            operation: String,
            url: String,
            startTime: Long
        ): AutoCloseable {
            val trace = com.google.firebase.perf.FirebasePerfManager.getInstance()
                .newTrace("custom_$operation")
            
            trace.putAttribute("url", url)
            trace.start()
            
            return AutoCloseable {
                val duration = System.currentTimeMillis() - startTime
                trace.putMetric("duration_ms", duration)
                trace.stop()
            }
        }
        
        fun traceApiCall(url: String, block: () -> Unit) {
            traceNetworkRequest("api_call", url, System.currentTimeMillis()) {
                block()
            }
        }
    }
    
    // Performance monitoring dashboard
    class PerformanceDashboard {
        
        // View in Firebase Console:
        // - Network request performance
        // - Slow requests
        // - Error rates
        // - Custom traces
        
        fun getNetworkMetrics(): NetworkMetrics {
            return NetworkMetrics(
                totalRequests = 0,
                avgLatency = 0,
                errorRate = 0,
                slowRequests = 0
            )
        }
        
        data class NetworkMetrics(
            val totalRequests: Int,
            val avgLatency: Long,
            val errorRate: Float,
            val slowRequests: Int
        )
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Network Monitoring Tools:**
- OkHttp EventListener - detailed timing
- Android Profiler - visual timeline
- Stetho - Chrome DevTools integration
- Firebase Performance - production monitoring

**Timing Breakdown:**
- DNS: Domain resolution
- TCP: Connection establishment
- TLS: SSL handshake
- TTFB: Time to first byte
- Download: Content download

**Performance Metrics:**
- Request count
- Average latency
- Error rate
- Payload sizes

**Debugging Tools:**
- Charles Proxy - full request inspection
- Postman - API testing
- curl - command line debugging

---

## Advanced Tips

- **Tip 1: Use Charles Proxy** - Inspect all network traffic
- **Tip 2: Export as HAR** - Save and share network data
- **Tip 3: Compare network profiles** - Before/after optimization
- **Tip 4: Monitor in production** - Firebase Performance
- **Tip 5: Create custom traces** - For specific operations

---

## Troubleshooting Guide (FAQ)

**Q: How do I see all network requests?**
A: Use Android Profiler Network tab or Stetho

**Q: Why is request slow?**
A: Check each timing component in Network Profiler

**Q: How do I debug API errors?**
A: Use logging interceptor, check response codes

**Q: How do I reduce network usage?**
A: Implement caching, compression, pagination

---

## Advanced Tips and Tricks

- **Tip 1: Use request batching** - Combine multiple API calls
- **Tip 2: Enable HTTP/2** - Connection reuse
- **Tip 3: Use persistent connections** - OkHttp does this
- **Tip 4: Monitor on real device** - Emulator different
- **Tip 5: Test on slow network** - Use network throttling

---

## CROSS-REFERENCES

- See: 09_PERFORMANCE/01_Performance_Optimization/05_Network_Optimization.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md
- See: 06_NETWORKING/01_HTTP_Communication/02_OkHttp_Configuration.md

---

## END OF NETWORK ANALYSIS GUIDE

(End of file - total 682 lines)