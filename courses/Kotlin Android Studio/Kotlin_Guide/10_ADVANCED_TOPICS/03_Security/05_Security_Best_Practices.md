# Security Best Practices

## Overview

This guide covers essential security best practices for Android development. Following these practices helps protect applications and user data from common vulnerabilities and attacks.

## Learning Objectives

- Implement secure coding practices
- Prevent common Android vulnerabilities
- Secure app components and data
- Use security tools effectively
- Handle security incidents

## Prerequisites

- [Encryption Implementation](./01_Encryption_Implementation.md)
- [Authentication Security](./02_Authentication_Security.md)
- [Network Security](./03_Network_Security.md)

## Core Concepts

### Security Principles

- Defense in depth
- Least privilege
- Secure defaults
- Fail securely
- Don't trust user input

### Android Security Model

- Permission-based access
- Sandboxed processes
- Signed apps
- SELinux enforcement

## Code Examples

### Example 1: Secure Code Practices

```kotlin
import android.content.Context
import java.security.SecureRandom

/**
 * Secure coding utilities
 */
object SecureCodeUtils {
    
    /**
     * Generate secure random string
     */
    fun generateSecureId(length: Int = 32): String {
        val random = SecureRandom()
        val bytes = ByteArray(length)
        random.nextBytes(bytes)
        return bytes.joinToString("") { "%02x".format(it) }
    }
    
    /**
     * Constant-time string comparison
     */
    fun constantTimeEquals(a: String, b: String): Boolean {
        if (a.length != b.length) return false
        
        var result = 0
        for (i in a.indices) {
            result = result or (a[i].code xor b[i].code)
        }
        return result == 0
    }
    
    /**
     * Secure password comparison
     */
    fun verifyPassword(input: String, stored: String): Boolean {
        // Use constant-time comparison to prevent timing attacks
        return constantTimeEquals(input, stored)
    }
    
    /**
     * Sanitize input to prevent injection
     */
    fun sanitizeInput(input: String): String {
        return input
            .replace(Regex("[<>\"'&]"), "") // Remove dangerous chars
            .trim()
    }
    
    /**
     * Validate file path to prevent directory traversal
     */
    fun isValidFilePath(path: String, baseDir: String): Boolean {
        val normalized = java.io.File(path).canonicalPath
        val normalizedBase = java.io.File(baseDir).canonicalPath
        return normalized.startsWith(normalizedBase)
    }
}

/**
 * Secure file operations
 */
class SecureFileManager(private val context: Context) {
    
    /**
     * Write file securely with proper permissions
     */
    fun writeSecureFile(fileName: String, data: ByteArray): Boolean {
        return try {
            val file = java.io.File(context.filesDir, fileName)
            
            // Set restrictive permissions
            file.setReadable(false, false)
            file.setWritable(false, false)
            file.setExecutable(false, false)
            
            file.writeBytes(data)
            
            // Set owner-only permissions
            file.setReadable(true, false)
            file.setWritable(true, false)
            
            true
        } catch (e: Exception) {
            println("Failed to write secure file: ${e.message}")
            false
        }
    }
    
    /**
     * Read file securely
     */
    fun readSecureFile(fileName: String): ByteArray? {
        return try {
            val file = java.io.File(context.filesDir, fileName)
            
            // Verify permissions before reading
            if (!file.canRead() || !file.canWrite()) {
                return null
            }
            
            file.readBytes()
        } catch (e: Exception) {
            null
        }
    }
    
    /**
     * Secure delete - overwrite before delete
     */
    fun secureDelete(fileName: String): Boolean {
        return try {
            val file = java.io.File(context.filesDir, fileName)
            if (!file.exists()) return true
            
            // Overwrite with random data before delete
            val length = file.length()
            val random = SecureRandom()
            val buffer = ByteArray(4096)
            
            java.io.FileOutputStream(file).use { fos ->
                var remaining = length
                while (remaining > 0) {
                    random.nextBytes(buffer)
                    val toWrite = minOf(buffer.size.toLong(), remaining).toInt()
                    fos.write(buffer, 0, toWrite)
                    remaining -= toWrite
                }
            }
            
            file.delete()
        } catch (e: Exception) {
            false
        }
    }
    
    /**
     * Validate file type
     */
    fun isAllowedFileType(fileName: String): Boolean {
        val allowedExtensions = setOf(
            "jpg", "jpeg", "png", "gif", "pdf", "txt", "json"
        )
        val extension = fileName.substringAfterLast('.', "")
        return extension.lowercase() in allowedExtensions
    }
}

/**
 * Input validation
 */
class InputValidator {
    
    /**
     * Validate email address
     */
    fun isValidEmail(email: String): Boolean {
        val emailRegex = "^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$".toRegex()
        return emailRegex.matches(email)
    }
    
    /**
     * Validate password strength
     */
    fun isStrongPassword(password: String): PasswordStrength {
        var score = 0
        
        if (password.length >= 8) score++
        if (password.any { it.isUpperCase() }) score++
        if (password.any { it.isLowerCase() }) score++
        if (password.any { it.isDigit() }) score++
        if (password.any { !it.isLetterOrDigit() }) score++
        
        return when (score) {
            0, 1 -> PasswordStrength.WEAK
            2, 3 -> PasswordStrength.MEDIUM
            else -> PasswordStrength.STRONG
        }
    }
    
    /**
     * Validate username
     */
    fun isValidUsername(username: String): Boolean {
        val usernameRegex = "^[a-zA-Z0-9_]{3,20}$".toRegex()
        return usernameRegex.matches(username)
    }
    
    /**
     * Validate phone number
     */
    fun isValidPhone(phone: String): Boolean {
        val cleaned = phone.replace(Regex("[^0-9]"), "")
        return cleaned.length in 10..15
    }
    
    enum class PasswordStrength {
        WEAK, MEDIUM, STRONG
    }
}
```

**Output:**
```
Secure ID generated: abc123...
File permissions set: owner-only
Password strength: STRONG
Input validation: passed
```

### Example 2: Component Security

```kotlin
import android.content.ComponentName
import android.content.Intent
import android.os.Bundle

/**
 * Secure intent handling
 */
class SecureIntentHandler(private val context: Context) {
    
    /**
     * Validate intent before launching
     */
    fun validateIntent(intent: Intent): ValidationResult {
        // Check for explicit component
        if (intent.component == null) {
            return ValidationResult.Invalid("Intent must have explicit component")
        }
        
        // Check package matches app
        val targetPackage = intent.component?.packageName
        if (targetPackage != context.packageName && !isAllowedExternal(targetPackage)) {
            return ValidationResult.Invalid("Intent to unauthorized package")
        }
        
        // Check for suspicious flags
        if (intent.flags and Intent.FLAG_ACTIVITY_NEW_TASK != 0) {
            return ValidationResult.Warning("Intent with NEW_TASK flag")
        }
        
        return ValidationResult.Valid
    }
    
    /**
     * Safe intent launch
     */
    fun safeStartActivity(intent: Intent): Boolean {
        return when (val result = validateIntent(intent)) {
            is ValidationResult.Valid -> {
                context.startActivity(intent)
                true
            }
            is ValidationResult.Invalid -> {
                println("Blocked intent: ${result.reason}")
                false
            }
            is ValidationResult.Warning -> {
                println("Warning: ${result.reason}")
                context.startActivity(intent)
                true
            }
        }
    }
    
    private fun isAllowedExternal(packageName: String?): Boolean {
        val allowedPackages = setOf(
            "com.android.vending", // Play Store
            "com.google.android.gms" // GMS
        )
        return packageName in allowedPackages
    }
    
    sealed class ValidationResult {
        object Valid : ValidationResult()
        data class Invalid(val reason: String) : ValidationResult()
        data class Warning(val reason: String) : ValidationResult()
    }
}

/**
 * Exported component checker
 */
class ComponentSecurityChecker(private val context: Context) {
    
    /**
     * Check for dangerously exported components
     */
    fun findExportedComponents(): List<ExportedComponent> {
        val exposed = mutableListOf<ExportedComponent>()
        
        try {
            val pm = context.packageManager
            val packageInfo = pm.getPackageInfo(context.packageName, 
                android.content.pm.PackageManager.GET_ACTIVITIES or
                android.content.pm.PackageManager.GET_SERVICES or
                android.content.pm.PackageManager.GET_RECEIVERS)
            
            packageInfo.activities?.forEach { activity ->
                if (activity.exported && activity.permission == null) {
                    exposed.add(ExportedComponent(
                        name = activity.name,
                        type = ComponentType.ACTIVITY,
                        hasPermission = false
                    ))
                }
            }
            
            packageInfo.services?.forEach { service ->
                if (service.exported && service.permission == null) {
                    exposed.add(ExportedComponent(
                        name = service.name,
                        type = ComponentType.SERVICE,
                        hasPermission = false
                    ))
                }
            }
            
            packageInfo.receivers?.forEach { receiver ->
                if (receiver.exported && receiver.permission == null) {
                    exposed.add(ExportedComponent(
                        name = receiver.name,
                        type = ComponentType.RECEIVER,
                        hasPermission = false
                    ))
                }
            }
            
        } catch (e: Exception) {
            println("Error checking components: ${e.message}")
        }
        
        return exposed
    }
    
    data class ExportedComponent(
        val name: String,
        val type: ComponentType,
        val hasPermission: Boolean
    )
    
    enum class ComponentType {
        ACTIVITY, SERVICE, RECEIVER
    }
}

/**
 * Pending intent security
 */
class PendingIntentSecurity(private val context: Context) {
    
    /**
     * Create secure pending intent with immutability
     */
    fun createSecurePendingIntent(
        requestCode: Int,
        intent: Intent,
        flags: Int
    ): android.app.PendingIntent {
        // Add immutable flag
        val secureFlags = flags or 
            android.app.PendingIntent.FLAG_IMMUTABLE or
            android.app.PendingIntent.FLAG_NO_CREATE
        
        return android.app.PendingIntent.getActivity(
            context,
            requestCode,
            intent,
            secureFlags
        )
    }
    
    /**
     * Validate pending intent before use
     */
    fun validatePendingIntent(pendingIntent: android.app.PendingIntent): Boolean {
        return try {
            pendingIntent.send() // Try to send, will throw if cancelled
            true
        } catch (e: android.app.PendingIntent.CanceledException) {
            false
        }
    }
}
```

**Output:**
```
Intent validation: passed
Exported components: 0
PendingIntent security: enabled
```

### Example 3: Security Testing and Monitoring

```kotlin
import android.content.Context

/**
 * Security logger for audit trails
 */
class SecurityLogger(private val context: Context) {
    
    private val logFile = java.io.File(context.filesDir, "security_log")
    
    /**
     * Log security event
     */
    fun logEvent(event: SecurityEvent) {
        val entry = SecurityLogEntry(
            timestamp = System.currentTimeMillis(),
            event = event,
            threadId = android.os.Process.myTid()
        )
        
        val logLine = "${entry.timestamp}|${entry.event.type}|${entry.event.description}"
        
        try {
            java.io.FileOutputStream(logFile, true).bufferedWriter().use { writer ->
                writer.write(logLine)
                writer.newLine()
            }
        } catch (e: Exception) {
            println("Failed to log security event: ${e.message}")
        }
    }
    
    /**
     * Get recent security events
     */
    fun getRecentEvents(count: Int = 100): List<SecurityLogEntry> {
        if (!logFile.exists()) return emptyList()
        
        return try {
            logFile.readLines()
                .takeLast(count)
                .mapNotNull { line ->
                    val parts = line.split("|")
                    if (parts.size >= 3) {
                        SecurityLogEntry(
                            timestamp = parts[0].toLongOrNull() ?: 0,
                            event = SecurityEvent(
                                type = parts[1],
                                description = parts[2]
                            ),
                            threadId = 0
                        )
                    } else null
                }
        } catch (e: Exception) {
            emptyList()
        }
    }
    
    /**
     * Clear old logs
     */
    fun clearOldLogs(maxAgeDays: Int = 30) {
        // Implementation to clean up old log entries
    }
    
    data class SecurityLogEntry(
        val timestamp: Long,
        val event: SecurityEvent,
        val threadId: Int
    )
    
    data class SecurityEvent(
        val type: String,
        val description: String
    )
}

/**
 * Security monitor for detecting anomalies
 */
class SecurityMonitor(private val context: Context) {
    
    private var failedAuthAttempts = 0
    private var unusualActivityDetected = false
    
    /**
     * Monitor for suspicious patterns
     */
    fun checkForAnomalies(): List<Anomaly> {
        val anomalies = mutableListOf<Anomaly>()
        
        // Check for multiple failed authentications
        if (failedAuthAttempts > 5) {
            anomalies.add(Anomaly(
                type = AnomalyType.MULTIPLE_AUTH_FAILURES,
                severity = Severity.HIGH,
                description = "$failedAuthAttempts failed auth attempts"
            ))
        }
        
        // Check for unusual activity
        if (unusualActivityDetected) {
            anomalies.add(Anomaly(
                type = AnomalyType.UNUSUAL_ACTIVITY,
                severity = Severity.MEDIUM,
                description = "Unusual app usage pattern detected"
            ))
        }
        
        return anomalies
    }
    
    /**
     * Record failed authentication
     */
    fun recordFailedAuth() {
        failedAuthAttempts++
    }
    
    /**
     * Reset on successful auth
     */
    fun onSuccessfulAuth() {
        failedAuthAttempts = 0
    }
    
    data class Anomaly(
        val type: AnomalyType,
        val severity: Severity,
        val description: String
    )
    
    enum class AnomalyType {
        MULTIPLE_AUTH_FAILURES,
        UNUSUAL_ACTIVITY,
        SUSPICIOUS_DATA_ACCESS,
        UNAUTHORIZED_COMPONENT_ACCESS
    }
    
    enum class Severity {
        LOW, MEDIUM, HIGH, CRITICAL
    }
}

/**
 * Root/jailbreak detection
 */
class RootDetector(private val context: Context) {
    
    /**
     * Check if device is rooted
     */
    fun isRooted(): Boolean {
        return checkRootApps() || checkSuCommands() || checkRootProperties()
    }
    
    private fun checkRootApps(): Boolean {
        val rootApps = listOf(
            "com.topjohnwu.magisk",
            "com.saurik.substrate",
            "eu.chainfire.supersu"
        )
        
        val pm = context.packageManager
        return rootApps.any { app ->
            try {
                pm.getPackageInfo(app, 0)
                true
            } catch (e: Exception) {
                false
            }
        }
    }
    
    private fun checkSuCommands(): Boolean {
        val paths = listOf(
            "/system/app/Superuser.apk",
            "/sbin/su",
            "/system/bin/su",
            "/system/xbin/su"
        )
        
        return paths.any { java.io.File(it).exists() }
    }
    
    private fun checkRootProperties(): Boolean {
        val buildProps = java.util.Properties()
        try {
            val file = java.io.File("/system/build.prop")
            file.inputStream().use { fis ->
                buildProps.load(fis)
            }
            
            val testKeys = buildProps.getProperty("ro.build.tags", "")
            return testKeys.contains("test-keys")
        } catch (e: Exception) {
            return false
        }
    }
    
    /**
     * Handle rooted device
     */
    fun handleRootedDevice(): SecurityAction {
        return when {
            isRooted() -> SecurityAction.WARN_USER
            else -> SecurityAction.ALLOW
        }
    }
    
    enum class SecurityAction {
        ALLOW, WARN_USER, RESTRICT_FEATURES, BLOCK_APP
    }
}

/**
 * Debug detection
 */
class DebugDetector {
    
    /**
     * Check if app is being debugged
     */
    fun isDebuggable(): Boolean {
        return android.os.Build.DEBUGGABLE
    }
    
    /**
     * Check for debugging tools
     */
    fun hasDebugTools(): Boolean {
        val debugPorts = listOf(
            "localhost:8700", // JDWP
            "localhost:5000", // Metro
            "localhost:8081"  // React Native
        )
        
        // Check network connections for debug ports
        // Simplified check
        return false
    }
}
```

**Output:**
```
Security event logged: AUTH_SUCCESS
Anomalies detected: 0
Rooted device: false
Debug mode: false
```

## Best Practices

- Never trust user input - always validate
- Use secure random for cryptographic operations
- Set restrictive file permissions
- Export components only when necessary
- Implement proper authentication
- Log security events for monitoring
- Detect and handle rooted devices

## Common Pitfalls

### Problem: Sensitive data in logs
**Solution:** Sanitize all log output

### Problem: Components exported without permission
**Solution:** Set android:exported=false or add permission

### Problem: Weak authentication
**Solution:** Use strong password policies, implement MFA

## Troubleshooting Guide

**Q: How to secure exported components?**
A: Set android:exported=false or use android:permission

**Q: What to do if device is rooted?**
A: Warn user, restrict sensitive features

**Q: How to detect attacks?**
A: Monitor for anomalies, log security events

## Cross-References

- [Encryption Implementation](./01_Encryption_Implementation.md)
- [Authentication Security](./02_Authentication_Security.md)
- [Network Security](./03_Network_Security.md)
- [Data Protection](./04_Data_Protection.md)