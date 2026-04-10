# Secure Storage

## Learning Objectives

1. Implementing encrypted SharedPreferences using AndroidX Security library
2. Using Android Keystore for cryptographic key management
3. Integrating biometric authentication for secure data access
4. Encrypting SQLCipher databases
5. Implementing file encryption with AES-256

## Prerequisites

- [01_SharedPreferences.md](../02_Data_Storage/01_SharedPreferences.md) - Understanding basic SharedPreferences
- [02_Data_Store_Implementation.md](../02_Data_Storage/02_Data_Store_Implementation.md) - DataStore fundamentals
- [03_File_Handling.md](../02_Data_Storage/03_File_Handling.md) - Basic file operations
- Understanding of Android Context and Application lifecycle

## Core Concepts

### Secure SharedPreferences

Secure SharedPreferences wraps standard SharedPreferences with encryption using AES-256. The AndroidX Security library provides transparent encryption that protects sensitive preference data at rest.

```kotlin
package com.android.data.secure

object SecureStorage {
    
    // Using EncryptedSharedPreferences
    class SecurePreferencesManager(private val context: android.content.Context) {
        
        private val masterKey by lazy {
            androidx.security.crypto.MasterKey.Builder(context)
                .setKeyScheme(androidx.security.crypto.MasterKey.KeyScheme.AES256_GCM)
                .build()
        }
        
        private val securePrefs by lazy {
            androidx.security.crypto.EncryptedSharedPreferences.create(
                context,
                "secure_prefs",
                masterKey,
                androidx.security.crypto.EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
                androidx.security.crypto.EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
            )
        }
        
        var authToken: String
            get() = securePrefs.getString("auth_token", null) ?: ""
            set(value) = securePrefs.edit().putString("auth_token", value).apply()
        
        var userCredentials: String
            get() = securePrefs.getString("user_credentials", null) ?: ""
            set(value) = securePrefs.edit().putString("user_credentials", value).apply()
        
        var refreshToken: String
            get() = securePrefs.getString("refresh_token", null) ?: ""
            set(value) = securePrefs.edit().putString("refresh_token", value).apply()
        
        fun clear() {
            securePrefs.edit().clear().apply()
        }
    }
}
```

### EncryptedSharedPreferences

EncryptedSharedPreferences uses a two-layer encryption approach: keys are encrypted with AES-256-SIV and values with AES-256-GCM, providing both confidentiality and integrity protection.

```kotlin
package com.android.data.secure

object EncryptedPrefsAdvanced {
    
    class AdvancedSecurePrefs(private val context: android.content.Context) {
        
        private val masterKey: androidx.security.crypto.MasterKey by lazy {
            androidx.security.crypto.MasterKey.Builder(context)
                .setKeyScheme(androidx.security.crypto.MasterKey.KeyScheme.AES256_GCM)
                .setEncryptionPaddings(androidx.security.crypto.EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV)
                .setSignaturePaddings(androidx.security.crypto.EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM)
                .build()
        }
        
        private val prefsFile by lazy {
            androidx.security.crypto.EncryptedSharedPreferences.create(
                context,
                "encrypted_prefs_file",
                masterKey,
                androidx.security.crypto.EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
                androidx.security.crypto.EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
            )
        }
        
        fun saveSecureData(key: String, value: String) {
            prefsFile.edit().putString(key, value).apply()
        }
        
        fun getSecureData(key: String, defaultValue: String = ""): String {
            return prefsFile.getString(key, defaultValue) ?: defaultValue
        }
    }
}
```

### Android Keystore

The Android Keystore provides hardware-backed security for cryptographic keys. Keys stored in the Keystore never leave the secure hardware, providing protection against extraction attacks.

```kotlin
package com.android.data.secure

object AndroidKeystoreManager {
    
    class SecureKeystore(private val context: android.content.Context) {
        
        private val keystore: java.security.KeyStore by lazy {
            java.security.KeyStore.getInstance("AndroidKeyStore").apply { load(null) }
        }
        
        private val keyAlias = "my_secure_key"
        
        fun generateKey(): Boolean {
            return try {
                val keyGenerator = javax.crypto.KeyGenerator.getInstance(
                    javax.crypto.KeyProperties.KEY_ALGORITHM_AES,
                    "AndroidKeyStore"
                )
                
                val keyGenSpec = android.security.keystore.KeyGenParameterSpec.Builder(
                    keyAlias,
                    android.security.keystore.KeyProperties.PURPOSE_ENCRYPT or android.security.keystore.KeyProperties.PURPOSE_DECRYPT
                )
                    .setBlockModes(android.security.keystore.KeyProperties.BLOCK_MODE_GCM)
                    .setEncryptionPaddings(android.security.keystore.KeyProperties.ENCRYPTION_PADDING_NONE)
                    .setKeySize(256)
                    .build()
                
                keyGenerator.init(keyGenSpec)
                keyGenerator.generateKey()
                true
            } catch (e: Exception) {
                false
            }
        }
        
        fun encrypt(data: ByteArray): ByteArray? {
            return try {
                val cipher = javax.crypto.Cipher.getInstance("AES/GCM/NoPadding")
                cipher.init(javax.crypto.Cipher.ENCRYPT_MODE, keystore.getKey(keyAlias, null))
                cipher.doFinal(data)
            } catch (e: Exception) {
                null
            }
        }
        
        fun decrypt(encryptedData: ByteArray): ByteArray? {
            return try {
                val cipher = javax.crypto.Cipher.getInstance("AES/GCM/NoPadding")
                cipher.init(javax.crypto.Cipher.DECRYPT_MODE, keystore.getKey(keyAlias, null))
                cipher.doFinal(encryptedData)
            } catch (e: Exception) {
                null
            }
        }
    }
}
```

### Biometric-protected Storage

Biometric authentication adds an additional layer of security by requiring user authentication before accessing sensitive data.

```kotlin
package com.android.data.secure

object BiometricStorage {
    
    class BiometricSecureStorage(private val context: android.content.Context) {
        
        private val executor = android.os.Handler(android.os.Looper.getMainLooper())
        
        private val biometricPrompt by lazy {
            androidx.biometric.BiometricPrompt(context, executor,
                object : androidx.biometric.BiometricPrompt.AuthenticationCallback() {
                    override fun onAuthenticationSucceeded(result: androidx.biometric.BiometricPrompt.AuthenticationResult) {
                        super.onAuthenticationSucceeded(result)
                    }
                    
                    override fun onAuthenticationError(errorCode: Int, errString: CharSequence) {
                        super.onAuthenticationError(errorCode, errString)
                    }
                    
                    override fun onAuthenticationFailed() {
                        super.onAuthenticationFailed()
                    }
                })
        }
        
        fun authenticate(requireStrongBiometric: Boolean = true, 
                       onSuccess: () -> Unit,
                       onError: (Int, String) -> Unit,
                       onFailed: () -> Unit) {
            
            val promptInfo = androidx.biometric.BiometricPrompt.PromptInfo.Builder()
                .setTitle("Authenticate")
                .setSubtitle("Confirm your identity to access secure data")
                .setNegativeButtonText("Cancel")
                .setAllowedAuthenticators(
                    if (requireStrongBiometric) {
                        androidx.biometric.BiometricManager.Authenticators.BIOMETRIC_STRONG
                    } else {
                        androidx.biometric.BiometricManager.Authenticators.BIOMETRIC_WEAK or 
                        androidx.biometric.BiometricManager.Authenticators.DEVICE_CREDENTIAL
                    }
                )
                .build()
            
            biometricPrompt.authenticate(promptInfo)
        }
        
        fun showBiometricPrompt(title: String, 
                              subtitle: String, 
                              negativeButtonText: String,
                              callback: androidx.biometric.BiometricPrompt.AuthenticationCallback) {
            
            val promptInfo = androidx.biometric.BiometricPrompt.PromptInfo.Builder()
                .setTitle(title)
                .setSubtitle(subtitle)
                .setNegativeButtonText(negativeButtonText)
                .build()
            
            biometricPrompt.authenticate(promptInfo)
        }
    }
}
```

### SQLCipher for Encrypted Databases

SQLCipher provides transparent AES-256 encryption for SQLite databases, protecting all data at rest.

```kotlin
package com.android.data.secure

object SecureDatabase {
    
    class EncryptedDatabase(private val context: android.content.Context) {
        
        private var database: android.database.sqlite.SQLiteDatabase? = null
        private var helper: DatabaseHelper? = null
        
        private val dbKey = "your_secret_key_here"
        
        init {
            net.sqlcipher.database.SQLiteDatabase.loadLibs(context)
        }
        
        fun openOrCreateDatabase(name: String) {
            helper = DatabaseHelper(context, name, dbKey)
            database = helper?.writableDatabase
        }
        
        fun closeDatabase() {
            database?.close()
            helper?.close()
        }
        
        inner class DatabaseHelper(
            context: android.content.Context,
            name: String,
            password: String
        ) : android.database.sqlite.SQLiteOpenHelper(context, name, null, 1) {
            
            init {
                net.sqlcipher.database.SupportFactory(password)
            }
            
            override fun onCreate(db: android.database.sqlite.SQLiteDatabase) {
                db.execSQL("CREATE TABLE IF NOT EXISTS users (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "username TEXT NOT NULL, " +
                    "email TEXT NOT NULL, " +
                    "password_hash TEXT NOT NULL)")
            }
            
            override fun onUpgrade(db: android.database.sqlite.SQLiteDatabase, 
                                   oldVersion: Int, newVersion: Int) {
                // Handle migrations
            }
        }
        
        fun insertUser(username: String, email: String, passwordHash: String) {
            val db = helper?.writableDatabase ?: return
            
            val values = android.content.ContentValues().apply {
                put("username", username)
                put("email", email)
                put("password_hash", passwordHash)
            }
            
            db.insert("users", null, values)
        }
        
        fun getAllUsers(): List<User> {
            val db = helper?.readableDatabase ?: return emptyList()
            val users = mutableListOf<User>()
            
            val cursor = db.rawQuery("SELECT * FROM users", null)
            while (cursor.moveToNext()) {
                users.add(User(
                    cursor.getLong(0),
                    cursor.getString(1),
                    cursor.getString(2),
                    cursor.getString(3)
                ))
            }
            cursor.close()
            
            return users
        }
        
        data class User(val id: Long, val username: String, val email: String, val passwordHash: String)
    }
}
```

### Secure File Encryption

Files can be encrypted using AES-256-GCM, providing both confidentiality and integrity verification.

```kotlin
package com.android.data.secure

object FileEncryption {
    
    class SecureFileManager(private val context: android.content.Context) {
        
        private val keystore: java.security.KeyStore by lazy {
            java.security.KeyStore.getInstance("AndroidKeyStore").apply { load(null) }
        }
        
        private val keyAlias = "file_encryption_key"
        
        fun initKeystoreKey() {
            if (!keystore.containsAlias(keyAlias)) {
                generateKey()
            }
        }
        
        private fun generateKey() {
            val keyGenerator = javax.crypto.KeyGenerator.getInstance(
                javax.crypto.KeyProperties.KEY_ALGORITHM_AES,
                "AndroidKeyStore"
            )
            
            val keyGenSpec = android.security.keystore.KeyGenParameterSpec.Builder(
                keyAlias,
                android.security.keystore.KeyProperties.PURPOSE_ENCRYPT or android.security.keystore.KeyProperties.PURPOSE_DECRYPT
            )
                .setBlockModes(android.security.keystore.KeyProperties.BLOCK_MODE_GCM)
                .setEncryptionPaddings(android.security.keystore.KeyProperties.ENCRYPTION_PADDING_NONE)
                .setKeySize(256)
                .build()
            
            keyGenerator.init(keyGenSpec)
            keyGenerator.generateKey()
        }
        
        fun encryptFile(inputFileName: String, outputFileName: String): Boolean {
            return try {
                val inputStream = context.openFileInput(inputFileName)
                val plaintext = inputStream.readBytes()
                inputStream.close()
                
                val cipher = javax.crypto.Cipher.getInstance("AES/GCM/NoPadding")
                cipher.init(javax.crypto.Cipher.ENCRYPT_MODE, keystore.getKey(keyAlias, null))
                
                val encryptedBytes = cipher.doFinal(plaintext)
                val outputStream = context.openFileOutput(outputFileName, android.content.Context.MODE_PRIVATE)
                outputStream.write(cipher.iv)
                outputStream.write(encryptedBytes)
                outputStream.close()
                
                true
            } catch (e: Exception) {
                false
            }
        }
        
        fun decryptFile(inputFileName: String, outputFileName: String): Boolean {
            return try {
                val inputStream = context.openFileInput(inputFileName)
                val iv = ByteArray(12)
                inputStream.read(iv)
                val encryptedBytes = inputStream.readBytes()
                inputStream.close()
                
                val cipher = javax.crypto.Cipher.getInstance("AES/GCM/NoPadding")
                val key = keystore.getKey(keyAlias, null) as javax.crypto.SecretKey
                val spec = javax.crypto.spec.GCMParameterSpec(128, iv)
                cipher.init(javax.crypto.Cipher.DECRYPT_MODE, key, spec)
                
                val decryptedBytes = cipher.doFinal(encryptedBytes)
                val outputStream = context.openFileOutput(outputFileName, android.content.Context.MODE_PRIVATE)
                outputStream.write(decryptedBytes)
                outputStream.close()
                
                true
            } catch (e: Exception) {
                false
            }
        }
    }
}
```

## Best Practices

- Use EncryptedSharedPreferences for all sensitive preference data including tokens, credentials, and user settings
- Generate keys in the Android Keystore to leverage hardware-backed security where available
- Require biometric authentication for accessing highly sensitive data such as payment information or private keys
- Use SQLCipher for any database containing user personal information or sensitive business data
- Implement key invalidation when device security is compromised (e.g., after device unlock method change)
- Store encryption keys separately from encrypted data when using manual encryption approaches
- Rotate encryption keys periodically according to security policy requirements
- Use GCM mode for file encryption to ensure both confidentiality and integrity protection
- Implement secure key backup using Android Keystore's backup features for enterprise device management

## Common Pitfalls

| Problem | Solution |
|---------|----------|
| Security library not included | Add `androidx.security:security-crypto` dependency to build.gradle |
| Biometric hardware not available | Check availability with `BiometricManager.canAuthenticate()` before prompting |
| Keystore operation failing | Wrap in try-catch and handle `KeyStoreException` appropriately |
| Database not closing properly | Always call `close()` on database helper in lifecycle methods |
| Encryption too slow for large files | Use streaming encryption with chunked processing for files > 1MB |
| Keys invalidated on device reset | Use backup-aware key generation or implement key escrow |

## Troubleshooting Guide

**Q: EncryptedSharedPreferences.create() throws exception**
- Ensure MasterKey is properly initialized before creating preferences
- Verify the encryption scheme is compatible with API level

**Q: BiometricPrompt not showing**
- Check that BiometricManager.canAuthenticate() returns BIOMETRIC_SUCCESS
- Verify prompt info is properly configured with allowed authenticators

**Q: SQLCipher database corrupted**
- Verify password is consistent across app launches
- Check for proper database closure before app termination

**Q: File decryption returning wrong data**
- Verify IV (initialization vector) is stored with encrypted data
- Ensure proper GCM specification with correct tag length

## Advanced Tips

- Hardware-backed Keystore keys provide superior protection but may not be available on all devices - implement fallback software encryption
- Use Android Protected Confirmation for extremely sensitive operations requiring user confirmation
- Implement secure channel for key transfer between app components using BoundKeyServices
- Use EncryptedFile for Android 10+ for transparent file encryption
- Implement MTE (Memory Tagging Extension) for detection of memory safety vulnerabilities on supported devices

## Cross-References

- [01_SharedPreferences.md](../02_Data_Storage/01_SharedPreferences.md) - Basic SharedPreferences
- [02_Data_Store_Implementation.md](../02_Data_Storage/02_Data_Store_Implementation.md) - DataStore API
- [03_File_Handling.md](../02_Data_Storage/03_File_Handling.md) - Basic file operations
- [04_Cache_Strategies.md](../02_Data_Storage/04_Cache_Strategies.md) - Cache management