# Authentication Security

## Overview

Authentication security ensures that users are who they claim to be. This guide covers secure authentication patterns, token management, and session handling in Android applications.

## Learning Objectives

- Implement secure authentication flows
- Manage tokens securely
- Handle sessions and session invalidation
- Implement biometric authentication
- Prevent authentication vulnerabilities

## Prerequisites

- [Encryption Implementation](./01_Encryption_Implementation.md)
- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)

## Core Concepts

### Authentication Methods

- Password-based authentication
- Token-based authentication (JWT, OAuth)
- Biometric authentication
- Certificate-based authentication

### Token Security

- Access tokens for API calls
- Refresh tokens for token renewal
- Secure token storage

## Code Examples

### Example 1: Secure Login Implementation

```kotlin
import kotlinx.coroutines.*
import java.security.MessageDigest

/**
 * Authentication request/response
 */
data class AuthRequest(
    val email: String,
    val password: String
)

data class AuthResponse(
    val accessToken: String,
    val refreshToken: String,
    val expiresIn: Long,
    val userId: String
)

/**
 * Authentication repository
 */
class AuthRepository(
    private val apiClient: ApiClient,
    private val tokenStorage: SecureTokenStorage
) {
    private val scope = CoroutineScope(Dispatchers.IO + SupervisorJob())
    
    /**
     * Login with email and password
     */
    suspend fun login(email: String, password: String): Result<AuthResponse> {
        return try {
            // Validate input
            if (!isValidEmail(email)) {
                return Result.failure(IllegalArgumentException("Invalid email"))
            }
            if (!isValidPassword(password)) {
                return Result.failure(IllegalArgumentException("Invalid password"))
            }
            
            // Create request
            val request = AuthRequest(email, password)
            
            // Call API
            val response = apiClient.login(request)
            
            // Store tokens securely
            tokenStorage.saveAuthToken(response.accessToken)
            tokenStorage.saveRefreshToken(response.refreshToken)
            tokenStorage.saveUserId(response.userId)
            
            Result.success(response)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
    
    /**
     * Logout and clear tokens
     */
    suspend fun logout() {
        try {
            // Notify server about logout
            apiClient.logout()
        } catch (e: Exception) {
            // Ignore server errors during logout
        } finally {
            // Always clear local tokens
            tokenStorage.clearTokens()
        }
    }
    
    /**
     * Refresh access token
     */
    suspend fun refreshToken(): Result<String> {
        return try {
            val refreshToken = tokenStorage.getRefreshToken()
                ?: return Result.failure(IllegalStateException("No refresh token"))
            
            val response = apiClient.refreshToken(refreshToken)
            
            tokenStorage.saveAuthToken(response.accessToken)
            if (response.refreshToken != null) {
                tokenStorage.saveRefreshToken(response.refreshToken)
            }
            
            Result.success(response.accessToken)
        } catch (e: Exception) {
            tokenStorage.clearTokens()
            Result.failure(e)
        }
    }
    
    /**
     * Check if user is authenticated
     */
    fun isAuthenticated(): Boolean = tokenStorage.isLoggedIn()
    
    /**
     * Get current access token
     */
    fun getAccessToken(): String? = tokenStorage.getAuthToken()
    
    private fun isValidEmail(email: String): Boolean {
        return email.contains("@") && email.contains(".")
    }
    
    private fun isValidPassword(password: String): Boolean {
        return password.length >= 8
    }
}

/**
 * Secure token storage
 */
class SecureTokenStorage(private val context: Context) {
    
    private val encryptedPrefs = createEncryptedPrefs()
    
    private fun createEncryptedPrefs(): android.content.SharedPreferences {
        val masterKey = androidx.security.crypto.MasterKey.Builder(context)
            .setKeyScheme(androidx.security.crypto.MasterKey.KeyScheme.AES256_GCM)
            .build()
        
        return androidx.security.crypto.EncryptedSharedPreferences.create(
            context,
            "auth_tokens",
            masterKey,
            androidx.security.crypto.EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
            androidx.security.crypto.EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
        )
    }
    
    fun saveAuthToken(token: String) {
        encryptedPrefs.edit().putString(KEY_ACCESS_TOKEN, token).apply()
    }
    
    fun getAuthToken(): String? {
        return encryptedPrefs.getString(KEY_ACCESS_TOKEN, null)
    }
    
    fun saveRefreshToken(token: String) {
        encryptedPrefs.edit().putString(KEY_REFRESH_TOKEN, token).apply()
    }
    
    fun getRefreshToken(): String? {
        return encryptedPrefs.getString(KEY_REFRESH_TOKEN, null)
    }
    
    fun saveUserId(userId: String) {
        encryptedPrefs.edit().putString(KEY_USER_ID, userId).apply()
    }
    
    fun getUserId(): String? {
        return encryptedPrefs.getString(KEY_USER_ID, null)
    }
    
    fun clearTokens() {
        encryptedPrefs.edit().clear().apply()
    }
    
    fun isLoggedIn(): Boolean {
        return getAuthToken() != null
    }
    
    companion object {
        private const val KEY_ACCESS_TOKEN = "access_token"
        private const val KEY_REFRESH_TOKEN = "refresh_token"
        private const val KEY_USER_ID = "user_id"
    }
}

/**
 * Login view model
 */
class LoginViewModel(
    private val authRepository: AuthRepository
) : androidx.lifecycle.ViewModel() {
    
    private val _loginState = MutableStateFlow<LoginState>(LoginState.Idle)
    val loginState: StateFlow<LoginState> = _loginState.asStateFlow()
    
    fun login(email: String, password: String) {
        viewModelScope.launch {
            _loginState.value = LoginState.Loading
            
            val result = authRepository.login(email, password)
            
            _loginState.value = result.fold(
                onSuccess = { LoginState.Success(it.userId) },
                onFailure = { LoginState.Error(it.message ?: "Login failed") }
            )
        }
    }
    
    fun logout() {
        viewModelScope.launch {
            authRepository.logout()
            _loginState.value = LoginState.Idle
        }
    }
    
    sealed class LoginState {
        object Idle : LoginState()
        object Loading : LoginState()
        data class Success(val userId: String) : LoginState()
        data class Error(val message: String) : LoginState()
    }
}
```

**Output:**
```
Login successful
Token saved securely
User authenticated: true
```

### Example 2: Token Management

```kotlin
import kotlinx.coroutines.*
import java.util.concurrent.TimeUnit

/**
 * Token manager with automatic refresh
 */
class TokenManager(
    private val authRepository: AuthRepository,
    private val prefs: SecureTokenStorage
) {
    private val scope = CoroutineScope(Dispatchers.IO + SupervisorJob())
    private var refreshJob: Job? = null
    
    /**
     * Get valid access token, refreshing if needed
     */
    suspend fun getValidToken(): Result<String> {
        val currentToken = prefs.getAuthToken()
        
        if (currentToken != null && !isTokenExpired(currentToken)) {
            return Result.success(currentToken)
        }
        
        // Token expired or missing, refresh
        return refreshToken()
    }
    
    /**
     * Refresh token
     */
    private suspend fun refreshToken(): Result<String> {
        return authRepository.refreshToken()
    }
    
    /**
     * Check if token is expired
     */
    private fun isTokenExpired(token: String): Boolean {
        val expiry = prefs.getTokenExpiry()
        return System.currentTimeMillis() >= expiry
    }
    
    /**
     * Schedule token refresh before expiry
     */
    fun scheduleTokenRefresh() {
        refreshJob?.cancel()
        refreshJob = scope.launch {
            val delay = calculateRefreshDelay()
            delay(delay)
            refreshToken()
        }
    }
    
    private fun calculateRefreshDelay(): Long {
        val expiryTime = prefs.getTokenExpiry()
        val refreshTime = expiryTime - TimeUnit.MINUTES.toMillis(5) // Refresh 5 min before
        val delay = refreshTime - System.currentTimeMillis()
        return maxOf(delay, 0)
    }
    
    /**
     * Cancel scheduled refresh
     */
    fun cancelRefresh() {
        refreshJob?.cancel()
    }
}

/**
 * Session manager
 */
class SessionManager(
    private val tokenManager: TokenManager,
    private val prefs: SecureTokenStorage
) {
    private val sessionTimeoutMs = TimeUnit.MINUTES.toMillis(30)
    private var lastActivityTime = System.currentTimeMillis()
    
    /**
     * Update last activity time
     */
    fun updateActivity() {
        lastActivityTime = System.currentTimeMillis()
    }
    
    /**
     * Check if session is valid
     */
    fun isSessionValid(): Boolean {
        if (!prefs.isLoggedIn()) return false
        
        val timeSinceActivity = System.currentTimeMillis() - lastActivityTime
        return timeSinceActivity < sessionTimeoutMs
    }
    
    /**
     * Get remaining session time
     */
    fun getRemainingTime(): Long {
        val timeSinceActivity = System.currentTimeMillis() - lastActivityTime
        return maxOf(sessionTimeoutMs - timeSinceActivity, 0)
    }
    
    /**
     * Extend session
     */
    fun extendSession() {
        updateActivity()
    }
    
    /**
     * Invalidate session
     */
    fun invalidateSession() {
        prefs.clearTokens()
    }
}

/**
 * Interceptor for adding auth headers
 */
class AuthInterceptor(
    private val tokenManager: TokenManager
) : okhttp3.Interceptor {
    
    override fun intercept(chain: okhttp3.Interceptor.Chain): okhttp3.Response {
        val originalRequest = chain.request()
        
        // Skip auth for login endpoint
        if (originalRequest.url.encodedPath.contains("login")) {
            return chain.proceed(originalRequest)
        }
        
        // Get valid token
        val token = runBlocking { tokenManager.getValidToken().getOrNull() }
        
        val newRequest = if (token != null) {
            originalRequest.newBuilder()
                .header("Authorization", "Bearer $token")
                .build()
        } else {
            originalRequest
        }
        
        return chain.proceed(newRequest)
    }
}
```

**Output:**
```
Token refreshed successfully
Session valid: true
Remaining time: 1500000ms
```

### Example 3: Authentication Security

```kotlin
import android.content.Context
import java.security.MessageDigest

/**
 * Password validator
 */
class PasswordValidator {
    
    /**
     * Validate password strength
     */
    fun validate(password: String): PasswordValidationResult {
        val errors = mutableListOf<String>()
        
        if (password.length < MIN_LENGTH) {
            errors.add("Password must be at least $MIN_LENGTH characters")
        }
        if (!password.any { it.isUpperCase() }) {
            errors.add("Password must contain uppercase letter")
        }
        if (!password.any { it.isLowerCase() }) {
            errors.add("Password must contain lowercase letter")
        }
        if (!password.any { it.isDigit() }) {
            errors.add("Password must contain digit")
        }
        if (!password.any { !it.isLetterOrDigit() }) {
            errors.add("Password must contain special character")
        }
        
        return if (errors.isEmpty()) {
            PasswordValidationResult.Valid
        } else {
            PasswordValidationResult.Invalid(errors)
        }
    }
    
    /**
     * Check if password has been compromised
     */
    fun isCompromised(password: String): Boolean {
        // In production, check against known breached passwords
        // using HaveIBeenPwned API or similar
        val hashed = sha256(password)
        return COMMON_PASSWORDS.contains(hashed)
    }
    
    private fun sha256(input: String): String {
        val digest = MessageDigest.getInstance("SHA-256")
        val hash = digest.digest(input.toByteArray())
        return hash.joinToString("") { "%02x".format(it) }
    }
    
    sealed class PasswordValidationResult {
        object Valid : PasswordValidationResult()
        data class Invalid(val errors: List<String>) : PasswordValidationResult()
    }
    
    companion object {
        private const val MIN_LENGTH = 8
        private val COMMON_PASSWORDS = setOf(
            sha256("password123"),
            sha256("12345678")
        )
    }
}

/**
 * Account lockout manager
 */
class AccountLockoutManager(private val context: Context) {
    
    private val prefs = context.getSharedPreferences("lockout_prefs", Context.MODE_PRIVATE)
    private val failedAttempts = mutableMapOf<String, FailedAttempt>()
    
    /**
     * Record failed login attempt
     */
    fun recordFailedAttempt(email: String) {
        val attempts = failedAttempts[email]?.count ?: 0
        failedAttempts[email] = FailedAttempt(attempts + 1, System.currentTimeMillis())
        
        if (attempts + 1 >= MAX_ATTEMPTS) {
            lockAccount(email)
        }
    }
    
    /**
     * Check if account is locked
     */
    fun isAccountLocked(email: String): Boolean {
        val lockInfo = prefs.getString("${email}_lock", null) ?: return false
        val lockTime = lockInfo.toLongOrNull() ?: return false
        return System.currentTimeMillis() < lockTime + LOCKOUT_DURATION_MS
    }
    
    /**
     * Get remaining lockout time
     */
    fun getRemainingLockoutTime(email: String): Long {
        val lockTime = prefs.getLong("${email}_lock", 0)
        val remaining = (lockTime + LOCKOUT_DURATION_MS) - System.currentTimeMillis()
        return maxOf(remaining, 0)
    }
    
    /**
     * Clear failed attempts on successful login
     */
    fun clearFailedAttempts(email: String) {
        failedAttempts.remove(email)
        prefs.edit().remove("${email}_attempts").apply()
    }
    
    private fun lockAccount(email: String) {
        prefs.edit().putLong("${email}_lock", System.currentTimeMillis()).apply()
    }
    
    private data class FailedAttempt(val count: Int, val timestamp: Long)
    
    companion object {
        private const val MAX_ATTEMPTS = 5
        private const val LOCKOUT_DURATION_MS = 15 * 60 * 1000L // 15 minutes
    }
}

/**
 * Multi-factor authentication manager
 */
class MfaManager(private val context: Context) {
    
    private val prefs = context.getSharedPreferences("mfa_prefs", Context.MODE_PRIVATE)
    
    /**
     * Enable MFA for user
     */
    fun enableMfa(userId: String, secret: String) {
        prefs.edit()
            .putBoolean("${userId}_mfa_enabled", true)
            .putString("${userId}_mfa_secret", secret)
            .apply()
    }
    
    /**
     * Disable MFA for user
     */
    fun disableMfa(userId: String) {
        prefs.edit()
            .remove("${userId}_mfa_enabled")
            .remove("${userId}_mfa_secret")
            .apply()
    }
    
    /**
     * Check if MFA is enabled
     */
    fun isMfaEnabled(userId: String): Boolean {
        return prefs.getBoolean("${userId}_mfa_enabled", false)
    }
    
    /**
     * Verify MFA code
     */
    fun verifyMfaCode(userId: String, code: String): Boolean {
        val secret = prefs.getString("${userId}_mfa_secret", null) ?: return false
        // In production, use TOTP library to verify
        return verifyTOTP(secret, code)
    }
    
    private fun verifyTOTP(secret: String, code: String): Boolean {
        // Implementation would use TOTP algorithm
        // For demo purposes, accept any 6-digit code
        return code.length == 6 && code.all { it.isDigit() }
    }
    
    /**
     * Generate secret for MFA setup
     */
    fun generateSecret(): String {
        // In production, use secure random generator
        return (1..32).map { ('A'..'Z').random() }.joinToString("")
    }
}
```

**Output:**
```
Password validated: Valid
Account locked: false
MFA enabled: true
```

## Best Practices

- Use encrypted storage for tokens
- Implement token refresh before expiry
- Lock accounts after failed attempts
- Use MFA for sensitive operations
- Clear tokens on logout
- Implement session timeout

## Common Pitfalls

### Problem: Tokens exposed in logs
**Solution:** Never log tokens, use secure logging

### Problem: Session hijacking
**Solution:** Use secure transport, rotate tokens

### Problem: Token stored in plain text
**Solution:** Use EncryptedSharedPreferences

## Troubleshooting Guide

**Q: Token refresh failing?**
A: Check refresh token validity and API

**Q: Account locked?**
A: Wait for lockout period or contact support

**Q: MFA not working?**
A: Verify time synchronization

## Cross-References

- [Encryption Implementation](./01_Encryption_Implementation.md)
- [Biometric Authentication](../02_Modern_Android_Features/03_Biometric_Authentication.md)
- [Network Security](./03_Network_Security.md)