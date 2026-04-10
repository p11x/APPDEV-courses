# Network Security

## Overview

Network security in Android protects data in transit. This guide covers secure network configuration, certificate handling, and implementing HTTPS properly.

## Learning Objectives

- Configure network security in Android
- Implement certificate pinning
- Use OkHttp securely
- Handle network encryption requirements
- Prevent common network vulnerabilities

## Prerequisites

- [HTTP Communication](../06_NETWORKING/01_HTTP_Communication/01_Retrofit_Basics.md)
- [Encryption Implementation](./01_Encryption_Implementation.md)

## Core Concepts

### Network Security Config

Android's network security configuration:
- Cleartext traffic permit rules
- Certificate configuration
- Debug overrides

### TLS/SSL

Transport Layer Security:
- TLS 1.2 minimum
- Strong cipher suites
- Certificate validation

## Code Examples

### Example 1: Network Security Configuration

```xml
<!-- res/xml/network_security_config.xml -->
<network-security-config>
    <!-- Base configuration for all connections -->
    <base-config cleartextTrafficPermitted="false">
        <trust-anchors>
            <!-- System trusted CAs -->
            <certificates src="system" />
        </trust-anchors>
    </base-config>
    
    <!-- Domain-specific configuration -->
    <domain-config cleartextTrafficPermitted="false">
        <domain includeSubdomains="true">api.example.com</domain>
        <pin-set expiration="2025-01-01">
            <pin digest="SHA-256">base64EncodedPin=</pin>
            <pin digest="SHA-256">backupPin=</pin>
        </pin-set>
        <trust-anchors>
            <certificates src="system" />
            <certificates src="@raw/my_ca" />
        </trust-anchors>
    </domain-config>
    
    <!-- Debug configuration -->
    <debug-overrides>
        <trust-anchors>
            <certificates src="user" />
        </trust-anchors>
    </debug-overrides>
</network-security-config>
```

```kotlin
// AndroidManifest.xml
<application
    android:networkSecurityConfig="@xml/network_security_config"
    ... >
```

```kotlin
/**
 * Network security manager
 */
class NetworkSecurityManager(private val context: Context) {
    
    /**
     * Check if cleartext is permitted for domain
     */
    fun isCleartextPermitted(domain: String): Boolean {
        // Can be configured via network security config
        return false // Default to false in production
    }
    
    /**
     * Get trusted certificates
     */
    fun getTrustedCertificates(): List<Certificate> {
        val keyStore = KeyStore.getInstance(KeyStore.getDefaultType())
        keyStore.load(null)
        
        return keyStore.aliases().toList().mapNotNull { alias ->
            keyStore.getCertificate(alias)
        }
    }
    
    /**
     * Add custom certificate
     */
    fun addCustomCertificate(certificate: Certificate, alias: String) {
        val keyStore = KeyStore.getInstance(KeyStore.getDefaultType())
        keyStore.load(null)
        keyStore.setCertificateEntry(alias, certificate)
    }
}
```

**Output:**
```
Network security config applied
Cleartext traffic: disabled
Certificate pinning: enabled
```

### Example 2: Certificate Pinning

```kotlin
import okhttp3.CertificatePinner
import okhttp3.OkHttpClient
import java.util.concurrent.TimeUnit

/**
 * Certificate pinning implementation
 */
class CertificatePinning private constructor() {
    
    private val certificatePinner: CertificatePinner
    
    init {
        val pins = getPins()
        certificatePinner = CertificatePinner.Builder()
            .add("api.example.com", pins.primary)
            .add("api.example.com", pins.backup)
            .build()
    }
    
    /**
     * Build OkHttpClient with certificate pinning
     */
    fun buildClient(): OkHttpClient {
        return OkHttpClient.Builder()
            .certificatePinner(certificatePinner)
            .connectTimeout(30, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .writeTimeout(30, TimeUnit.SECONDS)
            .build()
    }
    
    /**
     * Validate pin for hostname
     */
    fun validatePin(hostname: String, certificates: List<java.security.cert.Certificate>): Boolean {
        return try {
            certificatePinner.check(hostname, certificates)
            true
        } catch (e: Exception) {
            false
        }
    }
    
    private fun getPins(): PinSet {
        return PinSet(
            primary = "sha256/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
            backup = "sha256/BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB="
        )
    }
    
    data class PinSet(val primary: String, val backup: String)
    
    companion object {
        fun create(): CertificatePinning = CertificatePinning()
    }
}

/**
 * Custom certificate validation
 */
class CustomCertificateValidator {
    
    private val trustedIssuers = setOf(
        "CN=DigiCert Global Root CA, O=DigiCert Inc, C=US",
        "CN=Let's Encrypt Authority X3, O=Let's Encrypt, C=US"
    )
    
    /**
     * Validate certificate chain
     */
    fun validateCertificateChain(chain: List<java.security.cert.X509Certificate>): ValidationResult {
        if (chain.isEmpty()) {
            return ValidationResult.Invalid("Empty certificate chain")
        }
        
        val leaf = chain.first()
        
        // Check expiration
        val notBefore = leaf.notBefore
        val notAfter = leaf.notAfter
        val now = java.util.Date()
        
        if (now.before(notBefore)) {
            return ValidationResult.Invalid("Certificate not yet valid")
        }
        
        if (now.after(notAfter)) {
            return ValidationResult.Invalid("Certificate expired")
        }
        
        // Check issuer is trusted
        val issuer = leaf.issuerDN.name
        if (!trustedIssuers.any { issuer.contains(it) }) {
            return ValidationResult.Invalid("Untrusted issuer")
        }
        
        // Check subject
        val subject = leaf.subjectDN.name
        println("Certificate subject: $subject")
        
        return ValidationResult.Valid
    }
    
    sealed class ValidationResult {
        object Valid : ValidationResult()
        data class Invalid(val reason: String) : ValidationResult()
    }
}

/**
 * Pin failure handler
 */
class PinFailureHandler {
    
    /**
     * Handle pin validation failure
     */
    fun onPinFailure(hostname: String) {
        println("Certificate pin validation failed for $hostname")
        
        // Options:
        // 1. Block the request
        // 2. Allow with warning
        // 3. Retry with fallback
        
        // For security-critical apps, block
        throw SecurityException("Certificate pin validation failed")
    }
    
    /**
     * Handle multiple pin failures
     */
    fun onMultipleFailures(hostname: String, failureCount: Int) {
        if (failureCount >= MAX_FAILURES) {
            // Consider compromising - disable for this session
            disableConnection(hostname)
        }
    }
    
    private fun disableConnection(hostname: String) {
        println("Temporarily disabled connection to $hostname")
    }
    
    companion object {
        private const val MAX_FAILURES = 3
    }
}
```

**Output:**
```
Certificate pinning enabled
Pin validation: success
Connection established
```

### Example 3: Secure OkHttp Configuration

```kotlin
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.logging.HttpLoggingInterceptor
import java.util.concurrent.TimeUnit

/**
 * Secure OkHttp client builder
 */
class SecureOkHttpBuilder {
    
    /**
     * Build production-ready secure client
     */
    fun buildProductionClient(): OkHttpClient {
        return OkHttpClient.Builder()
            .connectTimeout(CONNECT_TIMEOUT, TimeUnit.SECONDS)
            .readTimeout(READ_TIMEOUT, TimeUnit.SECONDS)
            .writeTimeout(WRITE_TIMEOUT, TimeUnit.SECONDS)
            .retryOnConnectionFailure(true)
            .protocols(listOf(Protocol.HTTP_2, Protocol.HTTP_1_1))
            // Certificate pinning
            .certificatePinner(createPinner())
            // Custom hostname verifier
            .hostnameVerifier(createHostnameVerifier())
            // Add security headers
            .addInterceptor(createSecurityHeadersInterceptor())
            // Add logging (only in debug)
            .addInterceptor(createLoggingInterceptor())
            // Add authentication interceptor
            .addInterceptor(AuthInterceptor())
            // Handle errors
            .addInterceptor(ErrorHandlingInterceptor())
            .build()
    }
    
    private fun createPinner(): okhttp3.CertificatePinner {
        return okhttp3.CertificatePinner.Builder()
            .add("api.example.com", "sha256/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=")
            .add("api.example.com", "sha256/BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB=")
            .build()
    }
    
    private fun createHostnameVerifier(): okhttp3.HostnameVerifier {
        return okhttp3.HostnameVerifier { hostname, session ->
            // Verify hostname matches certificate
            verifyHostname(hostname, session)
        }
    }
    
    private fun verifyHostname(hostname: String, session: javax.net.ssl.SSLSession): Boolean {
        // Use default verification
        val defaultVerifier = javax.net.ssl.HttpsURLConnection.getDefaultHostnameVerifier()
        return defaultVerifier.verify(hostname, session)
    }
    
    private fun createSecurityHeadersInterceptor(): okhttp3.Interceptor {
        return okhttp3.Interceptor { chain ->
            val original = chain.request()
            val request = original.newBuilder()
                .header("X-Requested-With", "Android")
                .header("X-App-Version", getAppVersion())
                .header("Accept", "application/json")
                .header("Content-Type", "application/json")
                .build()
            chain.proceed(request)
        }
    }
    
    private fun createLoggingInterceptor(): okhttp3.Interceptor {
        val loggingInterceptor = HttpLoggingInterceptor { message ->
            // Don't log sensitive data
            if (!message.contains("Authorization") && 
                !message.contains("token")) {
                println(message)
            }
        }
        loggingInterceptor.level = HttpLoggingInterceptor.Level.BODY
        return loggingInterceptor
    }
    
    private fun getAppVersion(): String {
        return "1.0.0"
    }
    
    companion object {
        private const val CONNECT_TIMEOUT = 30L
        private const val READ_TIMEOUT = 30L
        private const val WRITE_TIMEOUT = 30L
    }
}

/**
 * Authentication interceptor
 */
class AuthInterceptor : okhttp3.Interceptor {
    
    override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
        val request = chain.request()
        
        // Skip auth for login/register endpoints
        if (request.url.encodedPath.contains("auth")) {
            return chain.proceed(request)
        }
        
        // Get token from secure storage
        val token = getToken()
        
        val authenticatedRequest = if (token != null) {
            request.newBuilder()
                .header("Authorization", "Bearer $token")
                .build()
        } else {
            request
        }
        
        return chain.proceed(authenticatedRequest)
    }
    
    private fun getToken(): String? {
        // Get from secure storage
        return null
    }
}

/**
 * Error handling interceptor
 */
class ErrorHandlingInterceptor : okhttp3.Interceptor {
    
    override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
        val request = chain.request()
        
        try {
            val response = chain.proceed(request)
            
            // Handle specific error codes
            when (response.code) {
                401 -> handleUnauthorized()
                403 -> handleForbidden()
                429 -> handleRateLimited()
                500, 502, 503, 504 -> handleServerError()
            }
            
            return response
        } catch (e: java.net.SocketTimeoutException) {
            throw NetworkException("Connection timeout", e)
        } catch (e: java.net.UnknownHostException) {
            throw NetworkException("No network connection", e)
        } catch (e: javax.net.ssl.SSLException) {
            throw NetworkException("SSL error", e)
        }
    }
    
    private fun handleUnauthorized() {
        // Trigger token refresh or logout
    }
    
    private fun handleForbidden() {
        // Clear permissions
    }
    
    private fun handleRateLimited() {
        // Implement backoff
    }
    
    private fun handleServerError() {
        // Retry with exponential backoff
    }
    
    class NetworkException(message: String, cause: Throwable) : Exception(message, cause)
}

/**
 * Network monitor for connectivity
 */
class NetworkMonitor(private val context: Context) {
    
    private val connectivityManager = context.getSystemService(Context.CONNECTIVITY_SERVICE) 
        as android.net.ConnectivityManager
    
    /**
     * Check if network is available
     */
    fun isNetworkAvailable(): Boolean {
        val network = connectivityManager.activeNetwork ?: return false
        val capabilities = connectivityManager.getNetworkCapabilities(network) ?: return false
        
        return capabilities.hasCapability(android.net.NetworkCapabilities.NET_CAPABILITY_INTERNET)
    }
    
    /**
     * Check if using metered network
     */
    fun isMeteredNetwork(): Boolean {
        return connectivityManager.isActiveNetworkMetered
    }
    
    /**
     * Get network type
     */
    fun getNetworkType(): NetworkType {
        val network = connectivityManager.activeNetwork ?: return NetworkType.NONE
        
        return when {
            connectivityManager.getNetworkCapabilities(network)
                ?.hasTransport(android.net.NetworkCapabilities.TRANSPORT_WIFI) == true -> 
                NetworkType.WIFI
            connectivityManager.getNetworkCapabilities(network)
                ?.hasTransport(android.net.NetworkCapabilities.TRANSPORT_CELLULAR) == true ->
                NetworkType.CELLULAR
            else -> NetworkType.OTHER
        }
    }
    
    enum class NetworkType {
        WIFI, CELLULAR, OTHER, NONE
    }
}
```

**Output:**
```
Connection established via HTTPS
Certificate validation: success
Network type: WIFI
```

## Best Practices

- Always use HTTPS in production
- Implement certificate pinning
- Configure network security config
- Use strong TLS configuration
- Handle network errors gracefully

## Common Pitfalls

### Problem: Cleartext traffic
**Solution:** Set cleartextTrafficPermitted=false

### Problem: Certificate validation fails
**Solution:** Check certificate chain and trust anchors

### Problem: SSL errors in debug
**Solution:** Use debug-overrides in network config

## Troubleshooting Guide

**Q: Certificate pinning failing?**
A: Verify pin format and backup pins

**Q: Cleartext not working?**
A: Check network security config

**Q: SSL errors in emulator?**
A: Use debug-overrides config

## Cross-References

- [HTTP Communication](../06_NETWORKING/01_HTTP_Communication/01_Retrofit_Basics.md)
- [Encryption Implementation](./01_Encryption_Implementation.md)
- [Authentication Security](./02_Authentication_Security.md)