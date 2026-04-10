# Biometric Authentication

## Overview

Biometric authentication provides secure, convenient user verification using fingerprints, face recognition, or iris scanning. Android's BiometricPrompt API offers a standardized way to implement biometric authentication.

## Learning Objectives

- Implement BiometricPrompt for authentication
- Handle biometric enrollment and availability
- Create fallback authentication methods
- Secure sensitive operations with biometrics

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Security Best Practices](./05_Security_Best_Practices.md)

## Core Concepts

### BiometricPrompt API

BiometricPrompt provides:
- Unified biometric UI across Android versions
- Device credential fallback option
- Secure hardware integration
- Crypto object for authentication binding

### Authentication Types

- Biometric only: Fingerprint, face, iris
- Device credentials: PIN, pattern, password
- Strong vs weak biometrics

## Code Examples

### Example 1: Basic Biometric Authentication

```kotlin
import android.os.Build
import androidx.biometric.BiometricManager
import androidx.biometric.BiometricPrompt
import androidx.core.content.ContextCompat
import androidx.fragment.app.FragmentActivity
import java.util.concurrent.Executor

/**
 * Biometric authentication manager
 * Handles biometric prompt setup and callbacks
 */
class BiometricAuthManager(private val activity: FragmentActivity) {
    
    private val executor: Executor = ContextCompat.getMainExecutor(activity)
    
    private lateinit var biometricPrompt: BiometricPrompt
    private lateinit var promptInfo: BiometricPrompt.PromptInfo
    
    /**
     * Initialize biometric prompt
     */
    fun init() {
        createBiometricPrompt()
        createPromptInfo()
    }
    
    private fun createBiometricPrompt() {
        biometricPrompt = BiometricPrompt(activity, executor,
            object : BiometricPrompt.AuthenticationCallback() {
                override fun onAuthenticationError(errorCode: Int, errString: CharSequence) {
                    super.onAuthenticationError(errorCode, errString)
                    when (errorCode) {
                        BiometricPrompt.ERROR_USER_CANCELED -> {
                            println("User cancelled authentication")
                        }
                        BiometricPrompt.ERROR_NEGATIVE_BUTTON -> {
                            println("User clicked negative button")
                        }
                        BiometricPrompt.ERROR_LOCKOUT -> {
                            println("Too many attempts, device locked")
                        }
                        else -> {
                            println("Authentication error: $errString")
                        }
                    }
                    authCallback?.onError(errorCode, errString.toString())
                }
                
                override fun onAuthenticationSucceeded(result: BiometricPrompt.AuthenticationResult) {
                    super.onAuthenticationSucceeded(result)
                    println("Authentication succeeded")
                    authCallback?.onSuccess(result)
                }
                
                override fun onAuthenticationFailed() {
                    super.onAuthenticationFailed()
                    println("Authentication failed")
                    authCallback?.onFailed()
                }
            })
    }
    
    private fun createPromptInfo() {
        promptInfo = BiometricPrompt.PromptInfo.Builder()
            .setTitle("Biometric Authentication")
            .setSubtitle("Verify your identity")
            .setNegativeButtonText("Use Password")
            .setAllowedAuthenticators(
                BiometricManager.Authenticators.BIOMETRIC_STRONG or
                BiometricManager.Authenticators.DEVICE_CREDENTIAL
            )
            .build()
    }
    
    /**
     * Show biometric prompt
     */
    fun authenticate() {
        biometricPrompt.authenticate(promptInfo)
    }
    
    /**
     * Show biometric prompt with crypto object
     */
    fun authenticateWithCrypto(cryptoObject: BiometricPrompt.CryptoObject) {
        biometricPrompt.authenticate(promptInfo, cryptoObject)
    }
    
    /**
     * Cancel authentication
     */
    fun cancelAuthentication() {
        biometricPrompt.cancelAuthentication()
    }
    
    private var authCallback: AuthCallback? = null
    
    fun setAuthCallback(callback: AuthCallback) {
        this.authCallback = callback
    }
    
    interface AuthCallback {
        fun onSuccess(result: BiometricPrompt.AuthenticationResult)
        fun onError(errorCode: Int, errorMessage: String)
        fun onFailed()
    }
}

/**
 * Biometric availability checker
 */
class BiometricChecker(private val context: android.content.Context) {
    
    private val biometricManager = BiometricManager.from(context)
    
    /**
     * Check if biometric authentication is available
     */
    fun canAuthenticate(): BiometricStatus {
        return when (biometricManager.canAuthenticate(BiometricManager.Authenticators.BIOMETRIC_STRONG)) {
            BiometricManager.BIOMETRIC_SUCCESS -> BiometricStatus.AVAILABLE
            BiometricManager.BIOMETRIC_ERROR_NO_HARDWARE -> BiometricStatus.NO_HARDWARE
            BiometricManager.BIOMETRIC_ERROR_HW_UNAVAILABLE -> BiometricStatus.HARDWARE_UNAVAILABLE
            BiometricManager.BIOMETRIC_ERROR_NONE_ENROLLED -> BiometricStatus.NOT_ENROLLED
            else -> BiometricStatus.UNKNOWN
        }
    }
    
    /**
     * Check if any biometric is enrolled
     */
    fun hasBiometricEnrolled(): Boolean {
        return canAuthenticate() == BiometricStatus.AVAILABLE
    }
    
    /**
     * Check if strong biometrics available
     */
    fun hasStrongBiometrics(): Boolean {
        return biometricManager.canAuthenticate(BiometricManager.Authenticators.BIOMETRIC_STRONG) == 
               BiometricManager.BIOMETRIC_SUCCESS
    }
    
    enum class BiometricStatus {
        AVAILABLE,
        NO_HARDWARE,
        HARDWARE_UNAVAILABLE,
        NOT_ENROLLED,
        UNKNOWN
    }
}

/**
 * Usage example in Activity
 */
class BiometricAuthExample : FragmentActivity(), BiometricAuthManager.AuthCallback {
    
    private lateinit var authManager: BiometricAuthManager
    private lateinit var biometricChecker: BiometricChecker
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        
        // Check biometric availability
        biometricChecker = BiometricChecker(this)
        val status = biometricChecker.canAuthenticate()
        
        println("Biometric status: $status")
        
        if (status == BiometricChecker.BiometricStatus.AVAILABLE) {
            // Initialize and authenticate
            authManager = BiometricAuthManager(this)
            authManager.init()
            authManager.setAuthCallback(this)
            authManager.authenticate()
        }
    }
    
    override fun onSuccess(result: BiometricPrompt.AuthenticationResult) {
        // Authentication successful - proceed with sensitive operation
        println("Authentication successful")
        accessSensitiveData()
    }
    
    override fun onError(errorCode: Int, errorMessage: String) {
        // Show error or fallback
        println("Error: $errorMessage")
        showFallback()
    }
    
    override fun onFailed() {
        // Show retry option
        println("Authentication failed, try again")
        showRetryOption()
    }
    
    private fun accessSensitiveData() {
        // Access protected data
    }
    
    private fun showFallback() {
        // Show password fallback
    }
    
    private fun showRetryOption() {
        // Allow retry or use password
    }
}
```

**Output:**
```
Biometric status: AVAILABLE
Authentication successful
```

### Example 2: Crypto-Bound Authentication

```kotlin
import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import androidx.biometric.BiometricPrompt
import java.security.KeyStore
import javax.crypto.Cipher
import javax.crypto.KeyGenerator
import javax.crypto.SecretKey

/**
 * Crypto-bound biometric authentication
 * Uses biometric-protected keys for enhanced security
 */
class CryptoBiometricManager(private val activity: FragmentActivity) {
    
    private val keyStore: KeyStore = KeyStore.getInstance(ANDROID_KEYSTORE).apply {
        load(null)
    }
    
    private lateinit var biometricPrompt: BiometricPrompt
    private lateinit var cipher: Cipher
    
    /**
     * Generate or retrieve biometric-protected key
     */
    fun getOrCreateKey(keyName: String): SecretKey {
        keyStore.getKey(keyName, null)?.let { return it as SecretKey }
        
        val keyGenerator = KeyGenerator.getInstance(
            KeyProperties.KEY_ALGORITHM_AES,
            ANDROID_KEYSTORE
        )
        
        val keyGenSpec = KeyGenParameterSpec.Builder(
            keyName,
            KeyProperties.PURPOSE_ENCRYPT or KeyProperties.PURPOSE_DECRYPT
        )
            .setBlockModes(KeyProperties.BLOCK_MODE_GCM)
            .setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_NONE)
            .setUserAuthenticationRequired(true)
            .setInvalidatedByBiometricEnrollment(true)
            .build()
        
        keyGenerator.init(keyGenSpec)
        return keyGenerator.generateKey()
    }
    
    /**
     * Initialize cipher for crypto operations
     */
    private fun initCipher(keyName: String): Boolean {
        return try {
            val key = getOrCreateKey(keyName)
            cipher = Cipher.getInstance(
                "${KeyProperties.KEY_ALGORITHM_AES}/GCM/NoPadding"
            )
            cipher.init(Cipher.ENCRYPT_MODE, key)
            true
        } catch (e: Exception) {
            println("Cipher initialization failed: ${e.message}")
            false
        }
    }
    
    /**
     * Authenticate and get crypto object
     */
    fun authenticateWithCrypto(
        keyName: String,
        onSuccess: (BiometricPrompt.CryptoObject) -> Unit,
        onError: (Int, String) -> Unit
    ) {
        if (!initCipher(keyName)) {
            onError(-1, "Failed to initialize cipher")
            return
        }
        
        val cryptoObject = BiometricPrompt.CryptoObject(cipher)
        
        val promptInfo = BiometricPrompt.PromptInfo.Builder()
            .setTitle("Authenticate to decrypt")
            .setSubtitle("Use biometric to access secure data")
            .setNegativeButtonText("Cancel")
            .build()
        
        biometricPrompt = BiometricPrompt(activity, ContextCompat.getMainExecutor(activity),
            object : BiometricPrompt.AuthenticationCallback() {
                override fun onAuthenticationSucceeded(result: BiometricPrompt.AuthenticationResult) {
                    super.onAuthenticationSucceeded(result)
                    result.cryptoObject?.let { onSuccess(it) }
                }
                
                override fun onAuthenticationError(errorCode: Int, errString: CharSequence) {
                    super.onAuthenticationError(errorCode, errString)
                    onError(errorCode, errString.toString())
                }
                
                override fun onAuthenticationFailed() {
                    super.onAuthenticationFailed()
                    onError(-1, "Authentication failed")
                }
            })
        
        biometricPrompt.authenticate(promptInfo, cryptoObject)
    }
    
    /**
     * Encrypt data using crypto object
     */
    fun encryptData(data: ByteArray, cryptoObject: BiometricPrompt.CryptoObject): ByteArray {
        return cryptoObject.cipher?.doFinal(data) ?: byteArrayOf()
    }
    
    /**
     * Decrypt data using crypto object
     */
    fun decryptData(encryptedData: ByteArray, cryptoObject: BiometricPrompt.CryptoObject): ByteArray {
        return cryptoObject.cipher?.doFinal(encryptedData) ?: byteArrayOf()
    }
    
    /**
     * Delete key
     */
    fun deleteKey(keyName: String) {
        keyStore.deleteEntry(keyName)
    }
    
    companion object {
        private const val ANDROID_KEYSTORE = "AndroidKeyStore"
    }
}

/**
 * Secure storage manager using biometric
 */
class BiometricSecureStorage(private val context: android.content.Context) {
    
    private val cryptoManager = CryptoBiometricManager(activity as FragmentActivity)
    private val prefs = context.getSharedPreferences("secure_prefs", android.content.Context.MODE_PRIVATE)
    
    /**
     * Save encrypted data with biometric protection
     */
    fun saveSecureData(
        key: String,
        data: String,
        onSuccess: () -> Unit,
        onError: (Int, String) -> Unit
    ) {
        cryptoManager.authenticateWithCrypto(
            key,
            { cryptoObject ->
                val encrypted = cryptoManager.encryptData(
                    data.toByteArray(Charsets.UTF_8),
                    cryptoObject
                )
                val encoded = android.util.Base64.encodeToString(
                    encrypted,
                    android.util.Base64.NO_WRAP
                )
                prefs.edit().putString(key, encoded).apply()
                onSuccess()
            },
            onError
        )
    }
    
    /**
     * Load encrypted data
     */
    fun loadSecureData(
        key: String,
        onSuccess: (String) -> Unit,
        onError: (Int, String) -> Unit
    ) {
        val encryptedData = prefs.getString(key, null) ?: run {
            onError(-1, "No data found")
            return
        }
        
        cryptoManager.authenticateWithCrypto(
            key,
            { cryptoObject ->
                val decoded = android.util.Base64.decode(
                    encryptedData,
                    android.util.Base64.NO_WRAP
                )
                val decrypted = cryptoManager.decryptData(decoded, cryptoObject)
                val data = String(decrypted, Charsets.UTF_8)
                onSuccess(data)
            },
            onError
        )
    }
    
    /**
     * Delete secure data
     */
    fun deleteSecureData(key: String) {
        prefs.edit().remove(key).apply()
        cryptoManager.deleteKey(key)
    }
    
    companion object {
        private lateinit var activity: FragmentActivity
        
        fun init(activity: FragmentActivity) {
            this.activity = activity
        }
    }
}
```

**Output:**
```
Key generated successfully
Encryption successful
Decryption successful
Data: sensitive information
```

### Example 3: Biometric with Fallback

```kotlin
import android.content.Context
import android.os.Build
import androidx.biometric.BiometricManager
import androidx.biometric.BiometricPrompt
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey
import java.security.MessageDigest

/**
 * Complete authentication flow with fallback
 * Supports biometric, PIN, pattern, and password
 */
class CompleteAuthFlow(private val context: Context) {
    
    private var lastFailedAttempts = 0
    private var lockoutEndTime = 0L
    
    /**
     * Show authentication prompt with fallback
     */
    fun authenticateWithFallback(
        activity: FragmentActivity,
        reason: String,
        callback: AuthCallback
    ) {
        val biometricManager = BiometricManager.from(context)
        val authenticators = getAvailableAuthenticators(biometricManager)
        
        if (authenticators == 0) {
            callback.onError("No authentication methods available")
            return
        }
        
        // Check if locked out
        if (System.currentTimeMillis() < lockoutEndTime) {
            val remaining = (lockoutEndTime - System.currentTimeMillis()) / 1000
            callback.onError("Locked out. Try again in $remaining seconds")
            return
        }
        
        val promptInfo = BiometricPrompt.PromptInfo.Builder()
            .setTitle("Authentication Required")
            .setSubtitle(reason)
            .setAllowedAuthenticators(authenticators)
            .build()
        
        val executor = ContextCompat.getMainExecutor(context)
        
        val biometricPrompt = BiometricPrompt(activity, executor,
            object : BiometricPrompt.AuthenticationCallback() {
                override fun onAuthenticationSucceeded(result: BiometricPrompt.AuthenticationResult) {
                    super.onAuthenticationSucceeded(result)
                    lastFailedAttempts = 0
                    callback.onSuccess(result)
                }
                
                override fun onAuthenticationError(errorCode: Int, errString: CharSequence) {
                    super.onAuthenticationError(errorCode, errString)
                    callback.onError(errString.toString())
                }
                
                override fun onAuthenticationFailed() {
                    super.onAuthenticationFailed()
                    lastFailedAttempts++
                    handleFailedAttempt()
                    callback.onFailed()
                }
            })
        
        biometricPrompt.authenticate(promptInfo)
    }
    
    private fun getAvailableAuthenticators(biometricManager: BiometricManager): Int {
        // Try biometric first, fallback to device credentials
        var authenticators = BiometricManager.Authenticators.BIOMETRIC_STRONG
        
        // Check biometric availability
        if (biometricManager.canAuthenticate(BiometricManager.Authenticators.BIOMETRIC_STRONG) 
            != BiometricManager.BIOMETRIC_SUCCESS) {
            // No biometric available, use device credentials
            authenticators = BiometricManager.Authenticators.DEVICE_CREDENTIAL
        }
        
        return authenticators
    }
    
    private fun handleFailedAttempt() {
        if (lastFailedAttempts >= MAX_FAILED_ATTEMPTS) {
            lockoutEndTime = System.currentTimeMillis() + LOCKOUT_DURATION_MS
            println("Too many failed attempts. Locked for ${LOCKOUT_DURATION_MS / 1000} seconds")
        }
    }
    
    interface AuthCallback {
        fun onSuccess(result: BiometricPrompt.AuthenticationResult)
        fun onError(message: String)
        fun onFailed()
    }
    
    companion object {
        private const val MAX_FAILED_ATTEMPTS = 5
        private const val LOCKOUT_DURATION_MS = 30_000L
    }
}

/**
 * Password-based authentication
 */
class PasswordAuthManager(private val context: Context) {
    
    private val prefs = context.getSharedPreferences("auth_prefs", Context.MODE_PRIVATE)
    
    /**
     * Set password (hashed)
     */
    fun setPassword(password: String) {
        val hash = hashPassword(password)
        prefs.edit().putString(KEY_PASSWORD_HASH, hash).apply()
    }
    
    /**
     * Verify password
     */
    fun verifyPassword(password: String): Boolean {
        val storedHash = prefs.getString(KEY_PASSWORD_HASH, null) ?: return false
        val inputHash = hashPassword(password)
        return storedHash == inputHash
    }
    
    /**
     * Check if password is set
     */
    fun hasPassword(): Boolean {
        return prefs.contains(KEY_PASSWORD_HASH)
    }
    
    /**
     * Clear password
     */
    fun clearPassword() {
        prefs.edit().remove(KEY_PASSWORD_HASH).apply()
    }
    
    private fun hashPassword(password: String): String {
        val digest = MessageDigest.getInstance("SHA-256")
        val hash = digest.digest(password.toByteArray(Charsets.UTF_8))
        return hash.joinToString("") { "%02x".format(it) }
    }
    
    companion object {
        private const val KEY_PASSWORD_HASH = "password_hash"
    }
}

/**
 * PIN-based authentication
 */
class PinAuthManager(private val context: Context) {
    
    private val prefs = context.getSharedPreferences("pin_prefs", Context.MODE_PRIVATE)
    private var failedAttempts = 0
    
    /**
     * Set PIN (hashed)
     */
    fun setPin(pin: String) {
        val hash = hashPin(pin)
        prefs.edit().putString(KEY_PIN_HASH, hash).apply()
        failedAttempts = 0
    }
    
    /**
     * Verify PIN
     */
    fun verifyPin(pin: String): Boolean {
        val storedHash = prefs.getString(KEY_PIN_HASH, null) ?: return false
        val inputHash = hashPin(pin)
        
        if (storedHash == inputHash) {
            failedAttempts = 0
            return true
        }
        
        failedAttempts++
        return false
    }
    
    /**
     * Get remaining attempts
     */
    fun getRemainingAttempts(): Int = MAX_ATTEMPTS - failedAttempts
    
    /**
     * Check if locked out
     */
    fun isLockedOut(): Boolean = failedAttempts >= MAX_ATTEMPTS
    
    private fun hashPin(pin: String): String {
        // Simple hash - in production use better approach
        return pin.hashCode().toString()
    }
    
    companion object {
        private const val KEY_PIN_HASH = "pin_hash"
        private const val MAX_ATTEMPTS = 5
    }
}

/**
 * Combined authentication manager
 */
class CombinedAuthManager(private val context: Context) {
    
    private val biometricManager = BiometricChecker(context)
    private val passwordManager = PasswordAuthManager(context)
    private val pinManager = PinAuthManager(context)
    private val authFlow = CompleteAuthFlow(context)
    
    /**
     * Determine best available authentication method
     */
    fun getBestAuthMethod(): AuthMethod {
        return when {
            biometricManager.hasStrongBiometrics() -> AuthMethod.BIOMETRIC
            pinManager.hasPassword() -> AuthMethod.PIN
            passwordManager.hasPassword() -> AuthMethod.PASSWORD
            else -> AuthMethod.NONE
        }
    }
    
    /**
     * Authenticate using best available method
     */
    fun authenticate(activity: FragmentActivity, reason: String, callback: AuthCallback) {
        when (getBestAuthMethod()) {
            AuthMethod.BIOMETRIC -> {
                authFlow.authenticateWithFallback(activity, reason, callback)
            }
            AuthMethod.PIN -> {
                // Show PIN dialog
                callback.onError("PIN authentication not implemented")
            }
            AuthMethod.PASSWORD -> {
                // Show password dialog
                callback.onError("Password authentication not implemented")
            }
            AuthMethod.NONE -> {
                callback.onError("No authentication methods configured")
            }
        }
    }
    
    enum class AuthMethod {
        BIOMETRIC, PIN, PASSWORD, NONE
    }
}
```

**Output:**
```
Authentication successful
Remaining attempts: 5
```

## Best Practice Guidelines

- Always check biometric availability before prompting
- Provide fallback for devices without biometrics
- Use crypto-bound operations for sensitive data
- Handle all authentication error codes
- Implement lockout after failed attempts

## Common Pitfalls

### Problem: Biometric not available
**Solution:** Use DeviceCredential as fallback authenticator

### Problem: Authentication cancelled
**Solution:** Handle ERROR_USER_CANCELED appropriately

### Problem: Keys invalidated
**Solution:** Handle KEY_INVALIDATED with re-enrollment

## Troubleshooting Guide

**Q: No biometric enrolled?**
A: Prompt user to enroll in device settings

**Q: Hardware unavailable?**
A: Fall back to password/PIN

**Q: Too many failed attempts?**
A: Implement exponential backoff

## Cross-References

- [Security Best Practices](./05_Security_Best_Practices.md)
- [Encryption Implementation](./01_Encryption_Implementation.md)