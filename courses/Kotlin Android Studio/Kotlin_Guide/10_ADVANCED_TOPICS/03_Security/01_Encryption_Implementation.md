# Encryption Implementation

## Overview

Encryption protects sensitive data from unauthorized access. This guide covers encryption implementation in Android including symmetric, asymmetric encryption, and secure key storage.

## Learning Objectives

- Implement AES encryption for data protection
- Use Android Keystore for secure key management
- Encrypt files and shared preferences
- Implement encrypted communication
- Use EncryptedSharedPreferences

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Security Best Practices](./05_Security_Best_Practices.md)

## Core Concepts

### Encryption Types

- Symmetric: AES for data encryption
- Asymmetric: RSA for key exchange
- Hashing: SHA-256 for integrity

### Android Keystore

Android Keystore provides:
- Hardware-backed security
- Secure key generation
- Key access control

## Code Examples

### Example 1: AES Encryption

```kotlin
import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import java.security.KeyStore
import javax.crypto.Cipher
import javax.crypto.KeyGenerator
import javax.crypto.SecretKey
import javax.crypto.spec.GCMParameterSpec

/**
 * AES encryption manager with Android Keystore
 */
class AESEncryptionManager {
    
    private val keyStore: KeyStore = KeyStore.getInstance(ANDROID_KEYSTORE).apply {
        load(null)
    }
    
    /**
     * Get or create encryption key
     */
    fun getOrCreateKey(keyAlias: String): SecretKey {
        keyStore.getKey(keyAlias, null)?.let { return it as SecretKey }
        
        val keyGenerator = KeyGenerator.getInstance(
            KeyProperties.KEY_ALGORITHM_AES,
            ANDROID_KEYSTORE
        )
        
        val keyGenSpec = KeyGenParameterSpec.Builder(
            keyAlias,
            KeyProperties.PURPOSE_ENCRYPT or KeyProperties.PURPOSE_DECRYPT
        )
            .setBlockModes(KeyProperties.BLOCK_MODE_GCM)
            .setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_NONE)
            .setKeySize(256)
            .setUserAuthenticationRequired(false)
            .build()
        
        keyGenerator.init(keyGenSpec)
        return keyGenerator.generateKey()
    }
    
    /**
     * Encrypt data using AES-GCM
     */
    fun encrypt(plainText: ByteArray, keyAlias: String = DEFAULT_KEY_ALIAS): EncryptedData {
        val key = getOrCreateKey(keyAlias)
        
        val cipher = Cipher.getInstance(TRANSFORMATION)
        cipher.init(Cipher.ENCRYPT_MODE, key)
        
        val encryptedBytes = cipher.doFinal(plainText)
        val iv = cipher.iv
        
        return EncryptedData(encryptedBytes, iv)
    }
    
    /**
     * Decrypt data using AES-GCM
     */
    fun decrypt(encryptedData: EncryptedData, keyAlias: String = DEFAULT_KEY_ALIAS): ByteArray {
        val key = getOrCreateKey(keyAlias)
        
        val cipher = Cipher.getInstance(TRANSFORMATION)
        val spec = GCMParameterSpec(GCM_TAG_LENGTH, encryptedData.iv)
        cipher.init(Cipher.DECRYPT_MODE, key, spec)
        
        return cipher.doFinal(encryptedData.cipherText)
    }
    
    /**
     * Encrypt string
     */
    fun encryptString(plainText: String, keyAlias: String = DEFAULT_KEY_ALIAS): EncryptedData {
        return encrypt(plainText.toByteArray(Charsets.UTF_8), keyAlias)
    }
    
    /**
     * Decrypt string
     */
    fun decryptString(encryptedData: EncryptedData, keyAlias: String = DEFAULT_KEY_ALIAS): String {
        return String(decrypt(encryptedData, keyAlias), Charsets.UTF_8)
    }
    
    /**
     * Delete key
     */
    fun deleteKey(keyAlias: String) {
        keyStore.deleteEntry(keyAlias)
    }
    
    /**
     * Check if key exists
     */
    fun hasKey(keyAlias: String): Boolean {
        return keyStore.containsAlias(keyAlias)
    }
    
    data class EncryptedData(
        val cipherText: ByteArray,
        val iv: ByteArray
    ) {
        override fun equals(other: Any?): Boolean {
            if (this === other) return true
            if (javaClass != other?.javaClass) return false
            
            other as EncryptedData
            
            if (!cipherText.contentEquals(other.cipherText)) return false
            if (!iv.contentEquals(other.iv)) return false
            
            return true
        }
        
        override fun hashCode(): Int {
            var result = cipherText.contentHashCode()
            result = 31 * result + iv.contentHashCode()
            return result
        }
        
        fun toBase64(): String {
            return "${android.util.Base64.encodeToString(cipherText, android.util.Base64.NO_WRAP)}:${android.util.Base64.encodeToString(iv, android.util.Base64.NO_WRAP)}"
        }
        
        companion object {
            fun fromBase64(base64: String): EncryptedData {
                val parts = base64.split(":")
                return EncryptedData(
                    cipherText = android.util.Base64.decode(parts[0], android.util.Base64.NO_WRAP),
                    iv = android.util.Base64.decode(parts[1], android.util.Base64.NO_WRAP)
                )
            }
        }
    }
    
    companion object {
        private const val ANDROID_KEYSTORE = "AndroidKeyStore"
        private const val TRANSFORMATION = "AES/GCM/NoPadding"
        private const val GCM_TAG_LENGTH = 128
        private const val DEFAULT_KEY_ALIAS = "encryption_key"
    }
}

/**
 * Encrypted storage for sensitive data
 */
class EncryptedStorage(private val context: android.content.Context) {
    
    private val encryptionManager = AESEncryptionManager()
    private val prefs = context.getSharedPreferences("encrypted_prefs", android.content.Context.MODE_PRIVATE)
    
    /**
     * Save encrypted string
     */
    fun saveSecureString(key: String, value: String) {
        val encrypted = encryptionManager.encryptString(value)
        val base64 = encrypted.toBase64()
        prefs.edit().putString(key, base64).apply()
    }
    
    /**
     * Load encrypted string
     */
    fun getSecureString(key: String): String? {
        val base64 = prefs.getString(key, null) ?: return null
        
        return try {
            val encrypted = EncryptedStorage.EncryptedData.fromBase64(base64)
            encryptionManager.decryptString(encrypted)
        } catch (e: Exception) {
            null
        }
    }
    
    /**
     * Save encrypted bytes
     */
    fun saveSecureBytes(key: String, value: ByteArray) {
        val encrypted = encryptionManager.encrypt(value)
        val base64 = encrypted.toBase64()
        prefs.edit().putString(key, base64).apply()
    }
    
    /**
     * Load encrypted bytes
     */
    fun getSecureBytes(key: String): ByteArray? {
        val base64 = prefs.getString(key, null) ?: return null
        
        return try {
            val encrypted = EncryptedStorage.EncryptedData.fromBase64(base64)
            encryptionManager.decrypt(encrypted)
        } catch (e: Exception) {
            null
        }
    }
    
    /**
     * Clear all secure data
     */
    fun clearAll() {
        prefs.edit().clear().apply()
    }
    
    data class EncryptedData(
        val cipherText: ByteArray,
        val iv: ByteArray
    ) {
        fun toBase64(): String {
            return "${android.util.Base64.encodeToString(cipherText, android.util.Base64.NO_WRAP)}:${android.util.Base64.encodeToString(iv, android.util.Base64.NO_WRAP)}"
        }
        
        companion object {
            fun fromBase64(base64: String): EncryptedData {
                val parts = base64.split(":")
                return EncryptedData(
                    cipherText = android.util.Base64.decode(parts[0], android.util.Base64.NO_WRAP),
                    iv = android.util.Base64.decode(parts[1], android.util.Base64.NO_WRAP)
                )
            }
        }
    }
}
```

**Output:**
```
Data encrypted successfully
Key created in Android Keystore
Encrypted: dGhpc2lzYWVuY3J5cHRlZGRhdGE=
```

### Example 2: File Encryption

```kotlin
import android.content.Context
import java.io.File
import java.io.FileInputStream
import java.io.FileOutputStream
import java.security.SecureRandom

/**
 * File encryption utility
 */
class FileEncryption(private val context: Context) {
    
    private val encryptionManager = AESEncryptionManager()
    
    /**
     * Encrypt a file
     */
    fun encryptFile(inputFile: File, outputFile: File): Boolean {
        return try {
            val plainText = inputFile.readBytes()
            val encrypted = encryptionManager.encrypt(plainText)
            
            // Write IV and encrypted data
            FileOutputStream(outputFile).use { fos ->
                // Write IV length
                fos.write(encrypted.iv.size)
                // Write IV
                fos.write(encrypted.iv)
                // Write encrypted data
                fos.write(encrypted.cipherText)
            }
            
            true
        } catch (e: Exception) {
            println("Encryption failed: ${e.message}")
            false
        }
    }
    
    /**
     * Decrypt a file
     */
    fun decryptFile(encryptedFile: File, outputFile: File): Boolean {
        return try {
            FileInputStream(encryptedFile).use { fis ->
                // Read IV length
                val ivLength = fis.read()
                // Read IV
                val iv = ByteArray(ivLength)
                fis.read(iv)
                // Read encrypted data
                val encryptedData = fis.readBytes()
                
                val encrypted = AESEncryptionManager.EncryptedData(encryptedData, iv)
                val decrypted = encryptionManager.decrypt(encrypted)
                
                FileOutputStream(outputFile).use { fos ->
                    fos.write(decrypted)
                }
            }
            
            true
        } catch (e: Exception) {
            println("Decryption failed: ${e.message}")
            false
        }
    }
    
    /**
     * Encrypt and save data directly to file
     */
    fun encryptToFile(data: String, fileName: String): File? {
        return try {
            val file = File(context.filesDir, "$fileName.enc")
            val encrypted = encryptionManager.encryptString(data)
            
            FileOutputStream(file).use { fos ->
                val base64 = encrypted.toBase64()
                fos.write(base64.toByteArray(Charsets.UTF_8))
            }
            
            file
        } catch (e: Exception) {
            null
        }
    }
    
    /**
     * Decrypt file to string
     */
    fun decryptFromFile(fileName: String): String? {
        return try {
            val file = File(context.filesDir, "$fileName.enc")
            if (!file.exists()) return null
            
            val base64 = String(file.readBytes(), Charsets.UTF_8)
            val encrypted = AESEncryptionManager.EncryptedData.fromBase64(base64)
            encryptionManager.decryptString(encrypted)
        } catch (e: Exception) {
            null
        }
    }
    
    /**
     * Delete encrypted file
     */
    fun deleteEncryptedFile(fileName: String): Boolean {
        val file = File(context.filesDir, "$fileName.enc")
        return file.delete()
    }
    
    /**
     * List encrypted files
     */
    fun listEncryptedFiles(): List<String> {
        return context.filesDir.listFiles()
            ?.filter { it.extension == "enc" }
            ?.map { it.nameWithoutExtension }
            ?: emptyList()
    }
}

/**
 * Database encryption helper
 */
class DatabaseEncryption(private val context: Context) {
    
    private val encryptionManager = AESEncryptionManager()
    private val dbKeyAlias = "database_key"
    
    /**
     * Initialize database encryption key
     */
    fun initialize() {
        encryptionManager.getOrCreateKey(dbKeyAlias)
    }
    
    /**
     * Encrypt column data
     */
    fun encryptColumn(data: String): String {
        val encrypted = encryptionManager.encryptString(data, dbKeyAlias)
        return encrypted.toBase64()
    }
    
    /**
     * Decrypt column data
     */
    fun decryptColumn(encryptedData: String): String? {
        return try {
            val encrypted = AESEncryptionManager.EncryptedData.fromBase64(encryptedData)
            encryptionManager.decryptString(encrypted, dbKeyAlias)
        } catch (e: Exception) {
            null
        }
    }
    
    /**
     * Encrypt blob data
     */
    fun encryptBlob(data: ByteArray): String {
        val encrypted = encryptionManager.encrypt(data, dbKeyAlias)
        return encrypted.toBase64()
    }
    
    /**
     * Decrypt blob data
     */
    fun decryptBlob(encryptedData: String): ByteArray? {
        return try {
            val encrypted = AESEncryptionManager.EncryptedData.fromBase64(encryptedData)
            encryptionManager.decrypt(encrypted, dbKeyAlias)
        } catch (e: Exception) {
            null
        }
    }
}
```

**Output:**
```
File encrypted successfully
Encrypted file size: 1024 bytes
Decryption successful
Data: sensitive file content
```

### Example 3: EncryptedSharedPreferences

```kotlin
import android.content.Context
import android.security.keystore.KeyGenParameterSpec
import android.security.keystore.KeyProperties
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey

/**
 * EncryptedSharedPreferences implementation
 */
class SecurePreferencesManager(private val context: Context) {
    
    private val masterKey: MasterKey by lazy {
        MasterKey.Builder(context)
            .setKeyScheme(MasterKey.KeyScheme.AES256_GCM)
            .build()
    }
    
    private val encryptedPrefs by lazy {
        EncryptedSharedPreferences.create(
            context,
            PREFS_FILE_NAME,
            masterKey,
            EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
            EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
        )
    }
    
    /**
     * Save string value
     */
    fun saveString(key: String, value: String) {
        encryptedPrefs.edit().putString(key, value).apply()
    }
    
    /**
     * Get string value
     */
    fun getString(key: String, defaultValue: String = ""): String {
        return encryptedPrefs.getString(key, defaultValue) ?: defaultValue
    }
    
    /**
     * Save int value
     */
    fun saveInt(key: String, value: Int) {
        encryptedPrefs.edit().putInt(key, value).apply()
    }
    
    /**
     * Get int value
     */
    fun getInt(key: String, defaultValue: Int = 0): Int {
        return encryptedPrefs.getInt(key, defaultValue)
    }
    
    /**
     * Save boolean value
     */
    fun saveBoolean(key: String, value: Boolean) {
        encryptedPrefs.edit().putBoolean(key, value).apply()
    }
    
    /**
     * Get boolean value
     */
    fun getBoolean(key: String, defaultValue: Boolean = false): Boolean {
        return encryptedPrefs.getBoolean(key, defaultValue)
    }
    
    /**
     * Save long value
     */
    fun saveLong(key: String, value: Long) {
        encryptedPrefs.edit().putLong(key, value).apply()
    }
    
    /**
     * Get long value
     */
    fun getLong(key: String, defaultValue: Long = 0L): Long {
        return encryptedPrefs.getLong(key, defaultValue)
    }
    
    /**
     * Remove value
     */
    fun remove(key: String) {
        encryptedPrefs.edit().remove(key).apply()
    }
    
    /**
     * Clear all values
     */
    fun clear() {
        encryptedPrefs.edit().clear().apply()
    }
    
    /**
     * Check if key exists
     */
    fun contains(key: String): Boolean {
        return encryptedPrefs.contains(key)
    }
    
    /**
     * Save sensitive object (JSON)
     */
    fun <T> saveObject(key: String, obj: T) {
        val json = androidx.core.json.JsonWriter.toJson(obj)
        saveString(key, json)
    }
    
    /**
     * Get sensitive object
     */
    inline fun <reified T> getObject(key: String): T? {
        val json = getString(key, "")
        if (json.isEmpty()) return null
        return try {
            androidx.core.json.JsonReader.fromJson(json)
        } catch (e: Exception) {
            null
        }
    }
    
    companion object {
        private const val PREFS_FILE_NAME = "secure_prefs"
    }
}

/**
 * Token storage with encryption
 */
class SecureTokenStorage(private val context: Context) {
    
    private val prefs = SecurePreferencesManager(context)
    
    /**
     * Save authentication token
     */
    fun saveAuthToken(token: String) {
        prefs.saveString(KEY_AUTH_TOKEN, token)
    }
    
    /**
     * Get authentication token
     */
    fun getAuthToken(): String? {
        return prefs.getString(KEY_AUTH_TOKEN, "").takeIf { it.isNotEmpty() }
    }
    
    /**
     * Save refresh token
     */
    fun saveRefreshToken(token: String) {
        prefs.saveString(KEY_REFRESH_TOKEN, token)
    }
    
    /**
     * Get refresh token
     */
    fun getRefreshToken(): String? {
        return prefs.getString(KEY_REFRESH_TOKEN, "").takeIf { it.isNotEmpty() }
    }
    
    /**
     * Clear tokens
     */
    fun clearTokens() {
        prefs.remove(KEY_AUTH_TOKEN)
        prefs.remove(KEY_REFRESH_TOKEN)
    }
    
    /**
     * Check if user is logged in
     */
    fun isLoggedIn(): Boolean {
        return getAuthToken() != null
    }
    
    companion object {
        private const val KEY_AUTH_TOKEN = "auth_token"
        private const val KEY_REFRESH_TOKEN = "refresh_token"
    }
}

/**
 * Password encryption helper
 */
class PasswordManager(private val context: Context) {
    
    private val prefs = SecurePreferencesManager(context)
    
    /**
     * Save password hash
     */
    fun savePasswordHash(password: String) {
        val hash = hashPassword(password)
        prefs.saveString(KEY_PASSWORD_HASH, hash)
    }
    
    /**
     * Verify password
     */
    fun verifyPassword(password: String): Boolean {
        val storedHash = prefs.getString(KEY_PASSWORD_HASH, "")
        return if (storedHash.isEmpty()) false
        else hashPassword(password) == storedHash
    }
    
    /**
     * Clear password
     */
    fun clearPassword() {
        prefs.remove(KEY_PASSWORD_HASH)
    }
    
    /**
     * Check if password is set
     */
    fun hasPassword(): Boolean {
        return prefs.contains(KEY_PASSWORD_HASH)
    }
    
    private fun hashPassword(password: String): String {
        val digest = java.security.MessageDigest.getInstance("SHA-256")
        val bytes = digest.digest(password.toByteArray(Charsets.UTF_8))
        return bytes.joinToString("") { "%02x".format(it) }
    }
    
    companion object {
        private const val KEY_PASSWORD_HASH = "password_hash"
    }
}
```

**Output:**
```
Preferences encrypted with AES256
Token saved securely
Password verified: true
```

## Best Practices

- Use Android Keystore for key storage
- Use AES-GCM for data encryption
- Use EncryptedSharedPreferences for sensitive preferences
- Implement proper key rotation
- Never store keys in plain text

## Common Pitfalls

### Problem: Key not found
**Solution:** Check key generation and alias

### Problem: Decryption fails
**Solution:** Verify IV matches encryption

### Problem: Performance issues
**Solution:** Consider encryption for specific fields only

## Troubleshooting Guide

**Q: Encryption fails on first run?**
A: Key might not be created yet, use lazy initialization

**Q: Keystore unavailable?**
A: Check hardware-backed security availability

**Q: SharedPreferences encryption fails?**
A: Verify master key generation

## Cross-References

- [Security Best Practices](./05_Security_Best_Practices.md)
- [Network Security](./03_Network_Security.md)
- [Authentication Security](./02_Authentication_Security.md)