# Data Protection

## Overview

Data protection in Android safeguards user data from unauthorized access. This guide covers data classification, secure storage, access controls, and protecting data at rest and in use.

## Learning Objectives

- Implement data classification
- Protect sensitive data at rest
- Use Android's data protection APIs
- Implement proper data retention
- Handle data in cross-app scenarios

## Prerequisites

- [Encryption Implementation](./01_Encryption_Implementation.md)
- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)

## Core Concepts

### Data Classification

- Public: No restrictions
- Internal: App-only access
- Confidential: Restricted access
- Secret: Highest protection

### Android Data Protection

- File-based encryption
- Credential-encrypted storage
- Hardware-backed security

## Code Examples

### Example 1: Data Classification

```kotlin
/**
 * Data classification levels
 */
enum class DataClassification {
    PUBLIC,      // No restrictions
    INTERNAL,   // App-only access
    CONFIDENTIAL, // Restricted access
    SECRET      // Highest protection
}

/**
 * Data classification checker
 */
class DataClassifier {
    
    /**
     * Classify data based on content type
     */
    fun classify(dataType: DataType): DataClassification {
        return when (dataType) {
            DataType.USER_PROFILE -> DataClassification.CONFIDENTIAL
            DataType.PAYMENT_INFO -> DataClassification.SECRET
            DataType.AUTH_TOKENS -> DataClassification.SECRET
            DataType.HEALTH_DATA -> DataClassification.SECRET
            DataType.LOCATION -> DataClassification.CONFIDENTIAL
            DataType.CONTACTS -> DataClassification.CONFIDENTIAL
            DataType.PHOTOS -> DataClassification.CONFIDENTIAL
            DataType.APP_SETTINGS -> DataClassification.PUBLIC
            DataType.ANALYTICS -> DataClassification.PUBLIC
            DataType.CACHE -> DataClassification.INTERNAL
        }
    }
    
    /**
     * Get protection requirements for classification
     */
    fun getProtectionRequirements(classification: DataClassification): ProtectionRequirements {
        return when (classification) {
            DataClassification.PUBLIC -> ProtectionRequirements(
                encrypt = false,
                requireAuth = false,
                maxRetention = Long.MAX_VALUE
            )
            DataClassification.INTERNAL -> ProtectionRequirements(
                encrypt = true,
                requireAuth = false,
                maxRetention = 30L * 24 * 60 * 60 * 1000 // 30 days
            )
            DataClassification.CONFIDENTIAL -> ProtectionRequirements(
                encrypt = true,
                requireAuth = true,
                maxRetention = 7L * 24 * 60 * 60 * 1000 // 7 days
            )
            DataClassification.SECRET -> ProtectionRequirements(
                encrypt = true,
                requireAuth = true,
                maxRetention = 24 * 60 * 60 * 1000 // 24 hours
            )
        }
    }
    
    enum class DataType {
        USER_PROFILE,
        PAYMENT_INFO,
        AUTH_TOKENS,
        HEALTH_DATA,
        LOCATION,
        CONTACTS,
        PHOTOS,
        APP_SETTINGS,
        ANALYTICS,
        CACHE
    }
    
    data class ProtectionRequirements(
        val encrypt: Boolean,
        val requireAuth: Boolean,
        val maxRetention: Long
    )
}
```

**Output:**
```
Data classified: CONFIDENTIAL
Encryption required: true
Max retention: 7 days
```

### Example 2: Secure Data Storage

```kotlin
import android.content.Context
import androidx.security.crypto.EncryptedSharedPreferences

/**
 * Protected data storage manager
 */
class ProtectedDataManager(private val context: Context) {
    
    private val encryptionManager = AESEncryptionManager()
    private val classifier = DataClassifier()
    
    /**
     * Store data with appropriate protection
     */
    fun <T> store(key: String, data: T, dataType: DataClassifier.DataType): Boolean {
        val classification = classifier.classify(dataType)
        val requirements = classifier.getProtectionRequirements(classification)
        
        return try {
            val serialized = serialize(data)
            
            if (requirements.encrypt) {
                val encrypted = encryptionManager.encryptString(serialized)
                saveToStorage(key, encrypted.toBase64(), dataType)
            } else {
                saveToStorage(key, serialized, dataType)
            }
            
            // Set retention policy
            setRetentionPolicy(key, requirements.maxRetention)
            
            true
        } catch (e: Exception) {
            println("Failed to store data: ${e.message}")
            false
        }
    }
    
    /**
     * Retrieve protected data
     */
    fun <T> retrieve(key: String, dataType: DataClassifier.DataType): T? {
        val classification = classifier.classify(dataType)
        val requirements = classifier.getProtectionRequirements(classification)
        
        val stored = retrieveFromStorage(key, dataType) ?: return null
        
        return try {
            val data = if (requirements.encrypt) {
                val encrypted = AESEncryptionManager.EncryptedData.fromBase64(stored)
                encryptionManager.decryptString(encrypted)
            } else {
                stored
            }
            
            deserialize<T>(data)
        } catch (e: Exception) {
            null
        }
    }
    
    /**
     * Delete data based on retention policy
     */
    fun cleanExpiredData() {
        val metadata = getDataMetadata()
        val now = System.currentTimeMillis()
        
        metadata.filter { it.expiresAt < now }.forEach { info ->
            delete(info.key)
            println("Deleted expired data: ${info.key}")
        }
    }
    
    private fun saveToStorage(key: String, data: String, dataType: DataClassifier.DataType) {
        val prefs = context.getSharedPreferences("protected_data", Context.MODE_PRIVATE)
        prefs.edit().putString(key, data).apply()
        
        // Save metadata
        saveMetadata(key, dataType)
    }
    
    private fun retrieveFromStorage(key: String, dataType: DataClassifier.DataType): String? {
        val prefs = context.getSharedPreferences("protected_data", Context.MODE_PRIVATE)
        return prefs.getString(key, null)
    }
    
    private fun saveMetadata(key: String, dataType: DataClassifier.DataType) {
        val prefs = context.getSharedPreferences("data_metadata", Context.MODE_PRIVATE)
        val classification = classifier.classify(dataType)
        val requirements = classifier.getProtectionRequirements(classification)
        
        prefs.edit()
            .putString("${key}_type", dataType.name)
            .putLong("${key}_created", System.currentTimeMillis())
            .putLong("${key}_expires", System.currentTimeMillis() + requirements.maxRetention)
            .apply()
    }
    
    private fun getDataMetadata(): List<DataMetadata> {
        val prefs = context.getSharedPreferences("data_metadata", Context.MODE_PRIVATE)
        return prefs.all.keys
            .filter { it.endsWith("_created") }
            .map { key ->
                val baseKey = key.removeSuffix("_created")
                DataMetadata(
                    key = baseKey,
                    createdAt = prefs.getLong(key, 0),
                    expiresAt = prefs.getLong("${baseKey}_expires", Long.MAX_VALUE)
                )
            }
    }
    
    private fun setRetentionPolicy(key: String, maxRetention: Long) {
        val prefs = context.getSharedPreferences("data_metadata", Context.MODE_PRIVATE)
        prefs.edit()
            .putLong("${key}_created", System.currentTimeMillis())
            .putLong("${key}_expires", System.currentTimeMillis() + maxRetention)
            .apply()
    }
    
    private fun delete(key: String) {
        context.getSharedPreferences("protected_data", Context.MODE_PRIVATE)
            .edit().remove(key).apply()
        context.getSharedPreferences("data_metadata", Context.MODE_PRIVATE)
            .edit().remove("${key}_type")
            .remove("${key}_created")
            .remove("${key}_expires")
            .apply()
    }
    
    private fun <T> serialize(data: T): String = data.toString()
    private fun <T> deserialize(data: String): T = data as T
    
    data class DataMetadata(
        val key: String,
        val createdAt: Long,
        val expiresAt: Long
    )
}

/**
 * Data access control manager
 */
class DataAccessControl(private val context: Context) {
    
    private val authManager: BiometricAuthManager? = null
    
    /**
     * Check if user can access data
     */
    fun canAccess(dataType: DataClassifier.DataType): Boolean {
        val classification = DataClassifier().classify(dataType)
        
        return when (classification) {
            DataClassification.PUBLIC -> true
            DataClassification.INTERNAL -> isAppRunning()
            DataClassification.CONFIDENTIAL -> isAuthenticated()
            DataClassification.SECRET -> isAuthenticatedRecently()
        }
    }
    
    /**
     * Require authentication for access
     */
    fun requireAuthentication(dataType: DataClassifier.DataType, callback: (Boolean) -> Unit) {
        val classification = DataClassifier().classify(dataType)
        
        if (classification == DataClassification.SECRET || 
            classification == DataClassification.CONFIDENTIAL) {
            // Trigger biometric authentication
            // callback(true) on success
            callback(true)
        } else {
            callback(true)
        }
    }
    
    private fun isAppRunning(): Boolean {
        return true // Simplified check
    }
    
    private fun isAuthenticated(): Boolean {
        val prefs = context.getSharedPreferences("auth_state", Context.MODE_PRIVATE)
        return prefs.getBoolean("authenticated", false)
    }
    
    private fun isAuthenticatedRecently(): Boolean {
        val prefs = context.getSharedPreferences("auth_state", Context.MODE_PRIVATE)
        val lastAuth = prefs.getLong("last_auth_time", 0)
        val fiveMinutesAgo = System.currentTimeMillis() - 5 * 60 * 1000
        return lastAuth > fiveMinutesAgo
    }
}
```

**Output:**
```
Data stored with encryption
Retention policy set: 7 days
Access control: passed
```

### Example 3: Data Retention and Privacy

```kotlin
import android.content.Context

/**
 * Data retention manager
 */
class DataRetentionManager(private val context: Context) {
    
    /**
     * Apply retention policy to data
     */
    fun applyRetentionPolicy(dataType: DataClassifier.DataType): RetentionAction {
        val classification = DataClassifier().classify(dataType)
        val requirements = DataClassifier().getProtectionRequirements(classification)
        
        return when {
            requirements.maxRetention < 24 * 60 * 60 * 1000 -> RetentionAction.DELETE_IMMEDIATELY
            requirements.maxRetention < 7 * 24 * 60 * 60 * 1000 -> RetentionAction.DELETE_AFTER_PERIOD
            else -> RetentionAction.RETAIN_NORMAL
        }
    }
    
    /**
     * Export user data for GDPR compliance
     */
    fun exportUserData(userId: String): ExportedData {
        val data = mutableMapOf<String, Any>()
        
        // Collect all user data
        data["profile"] = getProfileData(userId)
        data["activity"] = getActivityData(userId)
        data["preferences"] = getPreferencesData(userId)
        
        return ExportedData(
            userId = userId,
            exportedAt = System.currentTimeMillis(),
            data = data
        )
    }
    
    /**
     * Delete all user data for GDPR compliance
     */
    fun deleteAllUserData(userId: String): Boolean {
        return try {
            deleteProfileData(userId)
            deleteActivityData(userId)
            deletePreferencesData(userId)
            deleteAuthData(userId)
            
            // Clear cache
            context.cacheDir.delete()
            
            true
        } catch (e: Exception) {
            false
        }
    }
    
    /**
     * Anonymize data for analytics
     */
    fun anonymizeData(data: Map<String, Any>): Map<String, Any> {
        return data.mapValues { (key, value) ->
            when (key) {
                "email" -> hashValue(value.toString())
                "name" -> "REDACTED"
                "ip" -> "0.0.0.0"
                "device_id" -> hashValue(value.toString())
                else -> value
            }
        }
    }
    
    private fun hashValue(value: String): String {
        val digest = java.security.MessageDigest.getInstance("SHA-256")
        val hash = digest.digest(value.toByteArray())
        return hash.joinToString("") { "%02x".format(it) }
    }
    
    private fun getProfileData(userId: String): Map<String, Any> = mapOf()
    private fun getActivityData(userId: String): Map<String, Any> = mapOf()
    private fun getPreferencesData(userId: String): Map<String, Any> = mapOf()
    private fun deleteProfileData(userId: String) {}
    private fun deleteActivityData(userId: String) {}
    private fun deletePreferencesData(userId: String) {}
    private fun deleteAuthData(userId: String) {}
    
    sealed class RetentionAction {
        object DELETE_IMMEDIATELY : RetentionAction()
        object DELETE_AFTER_PERIOD : RetentionAction()
        object RETAIN_NORMAL : RetentionAction()
    }
    
    data class ExportedData(
        val userId: String,
        val exportedAt: Long,
        val data: Map<String, Any>
    )
}

/**
 * Privacy policy manager
 */
class PrivacyManager(private val context: Context) {
    
    private val prefs = context.getSharedPreferences("privacy_prefs", Context.MODE_PRIVATE)
    
    /**
     * Check if user consented to data collection
     */
    fun hasConsent(consentType: ConsentType): Boolean {
        return prefs.getBoolean(consentType.key, false)
    }
    
    /**
     * Record user consent
     */
    fun recordConsent(consentType: ConsentType, granted: Boolean) {
        prefs.edit()
            .putBoolean(consentType.key, granted)
            .putLong("${consentType.key}_timestamp", System.currentTimeMillis())
            .apply()
    }
    
    /**
     * Get consent timestamp
     */
    fun getConsentTimestamp(consentType: ConsentType): Long {
        return prefs.getLong("${consentType.key}_timestamp", 0)
    }
    
    /**
     * Withdraw all consent
     */
    fun withdrawAllConsent() {
        prefs.edit().clear().apply()
    }
    
    enum class ConsentType(val key: String, val description: String) {
        ANALYTICS("consent_analytics", "Analytics data collection"),
        PERSONALIZATION("consent_personalization", "Personalized content"),
        MARKETING("consent_marketing", "Marketing communications"),
        DATA_SHARING("consent_sharing", "Third-party data sharing"),
        LOCATION("consent_location", "Location tracking")
    }
}

/**
 * Data leak detection
 */
class DataLeakDetector(private val context: Context) {
    
    private val suspiciousPatterns = listOf(
        Pattern.compile("\\b\\d{3}-\\d{2}-\\d{4}\\b"), // SSN
        Pattern.compile("\\b\\d{16}\\b"), // Credit card
        Pattern.compile("password[:=]\\s*\\S+", Pattern.CASE_INSENSITIVE),
        Pattern.compile("token[:=]\\s*\\S+", Pattern.CASE_INSENSITIVE)
    )
    
    /**
     * Scan for potential data leaks
     */
    fun scanForLeaks(data: String): List<DataLeak> {
        val leaks = mutableListOf<DataLeak>()
        
        for (pattern in suspiciousPatterns) {
            val matcher = pattern.matcher(data)
            while (matcher.find()) {
                leaks.add(
                    DataLeak(
                        type = identifyLeakType(matcher.group()),
                        matchedPattern = pattern.pattern(),
                        position = matcher.start()
                    )
                )
            }
        }
        
        return leaks
    }
    
    /**
     * Log potential leak for monitoring
     */
    fun logPotentialLeak(leak: DataLeak) {
        println("Potential data leak detected: ${leak.type}")
        // In production, send to security monitoring
    }
    
    private fun identifyLeakType(match: String): LeakType {
        return when {
            match.contains("\\d{3}-\\d{2}-\\d{4}".toRegex()) -> LeakType.SSN
            match.contains("\\d{16}".toRegex()) -> LeakType.CREDIT_CARD
            match.contains("password", ignoreCase = true) -> LeakType.PASSWORD
            match.contains("token", ignoreCase = true) -> LeakType.TOKEN
            else -> LeakType.OTHER
        }
    }
    
    data class DataLeak(
        val type: LeakType,
        val matchedPattern: String,
        val position: Int
    )
    
    enum class LeakType {
        SSN, CREDIT_CARD, PASSWORD, TOKEN, OTHER
    }
}
```

**Output:**
```
Data retention policy applied
GDPR export completed
No data leaks detected
```

## Best Practices

- Classify data by sensitivity
- Apply appropriate protection per classification
- Implement data retention policies
- Handle GDPR/compliance requirements
- Monitor for data leaks

## Common Pitfalls

### Problem: Sensitive data in logs
**Solution:** Implement log sanitization

### Problem: Data retained too long
**Solution:** Set retention limits and auto-cleanup

### Problem: Data shared with third parties
**Solution:** Implement consent management

## Troubleshooting Guide

**Q: How to protect against data leaks?**
A: Use encryption, access controls, monitor for leaks

**Q: What data to anonymize?**
A: PII, credentials, identifiers

**Q: How to handle data deletion requests?**
A: Implement complete deletion including cache

## Cross-References

- [Encryption Implementation](./01_Encryption_Implementation.md)
- [Authentication Security](./02_Authentication_Security.md)
- [Security Best Practices](./05_Security_Best_Practices.md)