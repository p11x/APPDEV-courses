# App Signing

## Learning Objectives

1. Understanding Android app signing concepts and security
2. Configuring signing for development and release builds
3. Managing signing keys securely
4. Implementing Google Play App Signing
5. Migrating to Google Play App Signing
6. Handling keystore rotation and key management

## Prerequisites

- [Google Play Store](./01_Google_Play_Store.md)
- [App Versioning](./05_App_Versioning.md)

## Section 1: Understanding App Signing Fundamentals

App signing is a critical security mechanism that ensures the authenticity and integrity of your Android application. When you sign an app, you attach a cryptographic signature that verifies the app's origin and confirms that the code hasn't been tampered with since you published it. This signing process is fundamental to Android's security model and is required for app installation on user devices.

Android uses Public Key Infrastructure (PKI) for app signing. Each developer has a private key that never leaves their secure environment, and this key is used to create a digital signature for each app release. The corresponding public key is embedded in the APK and used by Android to verify the signature during installation. This asymmetric encryption ensures that only the holder of the private key can create valid signatures.

Understanding the distinction between debug and release signing is essential. Debug signing uses an auto-generated keystore that Android Studio creates the first time you build a debug variant. These keys are not secure and should never be used for production apps. Release signing requires a proper keystore that you generate and protect with strong passwords and potentially hardware security modules for enterprise-grade protection.

The signing configuration in your build.gradle file determines how your app is signed:

```kotlin
// build.gradle.kts (app module)
android {
    signingConfigs {
        create("release") {
            // Using keystore properties from gradle.properties
            storeFile = file(rootProject.file(keystoreProperties["keystorePath"] as String))
            storePassword = keystoreProperties["storePassword"] as String
            keyAlias = keystoreProperties["keyAlias"] as String
            keyPassword = keystoreProperties["keyPassword"] as String
            
            // Enable signing with V2 and V3 schemes for better security
            // V3 supports key rotation for improved security
            enableV3Signing = true
            enableV4Signing = true
        }
    }
    
    buildTypes {
        release {
            isMinifyEnabled = true
            isDebuggable = false
            signingConfig = signingConfigs.getByName("release")
        }
        debug {
            isDebuggable = true
            // Debug signing uses the default debug keystore
            signingConfig = signingConfigs.getByName("debug")
        }
    }
}

// gradle.properties
keystorePath=keystore/release-keystore.jks
storePassword=your_secure_store_password
keyAlias=your_key_alias
keyPassword=your_secure_key_password
```

## Section 2: Generating and Managing Keystores

Creating and managing signing keys properly is one of the most important aspects of app distribution. A compromised key can lead to unauthorized app updates, malicious versions of your app, and potential security breaches for your users.

The keytool command from the JDK allows you to create keystores and manage keys. Generate a new keystore with appropriate key sizes and algorithms:

```kotlin
// Shell command to generate keystore (for reference)
// keytool -genkeypair -v -storetype JKS -keyalg RSA -keysize 4096 -validity 10000 \
//   -keystore release-keystore.jks \
//   -alias myapp-key \
//   -storepass your_secure_store_password \
//   -keypass your_secure_key_password \
//   -dname "CN=Your Name, OU=Your Org, O=Your Company, L=City, ST=State, C=US"

// Kotlin utility for keystore management
package com.example.myapp.signing

import java.io.FileInputStream
import java.io.FileOutputStream
import java.security.KeyStore
import java.security.PrivateKey
import java.security.cert.X509Certificate
import java.security.cert.Certificate
import java.util.Date

class KeystoreManager(
    private val keystorePath: String,
    private val storePassword: String
) {
    
    private val keyStore: KeyStore = KeyStore.getInstance("JKS").apply {
        load(FileInputStream(keystorePath), storePassword.toCharArray())
    }
    
    fun getKey(alias: String, keyPassword: String): PrivateKey {
        return keyStore.getKey(alias, keyPassword.toCharArray()) as PrivateKey
    }
    
    fun getCertificate(alias: String): X509Certificate {
        return keyStore.getCertificate(alias) as X509Certificate
    }
    
    fun listKeys(): Set<String> {
        return keyStore.aliases().toList().toSet()
    }
    
    fun checkKeyValidity(alias: String): KeyValidityInfo {
        val cert = getCertificate(alias)
        val now = Date()
        val validFrom = cert.notBefore
        val validTo = cert.notAfter
        
        val daysUntilExpiry = ((validTo.time - now.time) / (1000 * 60 * 60 * 24)).toInt()
        
        return KeyValidityInfo(
            alias = alias,
            validFrom = validFrom,
            validTo = validTo,
            daysRemaining = daysUntilExpiry,
            isExpired = now.after(validTo),
            isExpiringSoon = daysUntilExpiry < 90
        )
    }
    
    fun exportPublicKey(alias: String, outputPath: String) {
        val cert = getCertificate(alias)
        val fos = FileOutputStream(outputPath)
        fos.write(cert.encoded)
        fos.close()
    }
    
    fun backupKeystore(outputPath: String, backupPassword: String) {
        // Create a backup of the keystore
        val backup = KeyStore.getInstance("JKS")
        backup.load(null)
        
        for (alias in keyStore.aliases()) {
            val entry = keyStore.getEntry(alias, 
                KeyStore.PasswordProtection(storePassword.toCharArray()))
            backup.setEntry(alias, entry, 
                KeyStore.PasswordProtection(backupPassword.toCharArray()))
        }
        
        val fos = FileOutputStream(outputPath)
        backup.store(fos, backupPassword.toCharArray())
        fos.close()
    }
}

data class KeyValidityInfo(
    val alias: String,
    val validFrom: Date,
    val validTo: Date,
    val daysRemaining: Int,
    val isExpired: Boolean,
    val isExpiringSoon: Boolean
)
```

Always maintain multiple backups of your keystore in secure locations. Never store keystores in version control or shared directories. Consider using hardware security modules (HSM) or cloud key management services for enterprise applications.

## Section 3: Google Play App Signing

Google Play App Signing allows Google to manage your app signing keys for you, simplifying key management and enabling advanced features. When you opt into App Signing, Google generates and manages the signing key used for your production releases, while you continue to use your own upload key to sign APKs for upload.

This approach provides several benefits. You no longer need to securely store and manage production signing keys, as Google handles this securely. If you lose your upload key, Google Play support can help you reset it. Google can also rotate your signing keys, providing forward secrecy if a key is compromised.

To configure Google Play App Signing, you have two options: let Google generate a new signing key, or upload your existing signing key:

```kotlin
// Configuration for uploading existing signing key to Google Play

// Step 1: Export your signing key in PKCS#12 format
// keytool -exportcert -keystore release-keystore.jks -alias myapp-key \
//   -file upload_key.pub -storepass your_password

// Step 2: Export the private key in PKCS#12 format with Google Play
// keytool -importkeystore -srckeystore release-keystore.jks \
//   -srcstorepass your_password -srcalias myapp-key \
//   -destkeystore upload_key.p12 \
//   -deststoretype PKCS12 \
//   -deststorepass your_password -destalias upload_key

// In Android build configuration, configure signing for App Signing

android {
    signingConfigs {
        release {
            // Upload key - used to sign APKs for Play Store upload
            storeFile = file("upload_key.jks")
            storePassword = System.getenv("UPLOAD_KEY_STORE_PASSWORD")
            keyAlias = "upload-key"
            keyPassword = System.getenv("UPLOAD_KEY_PASSWORD")
        }
    }
    
    buildTypes {
        release {
            // The APK will be signed with the upload key
            // Google Play will re-sign with the App Signing key
            signingConfig = signingConfigs.getByName("release")
        }
    }
}

// For additional security, use environment variables for passwords
// Never hardcode passwords in build.gradle files
```

After enabling App Signing in Play Console, your deployment process changes. You sign APKs with your upload key, upload them to Google Play, and Google re-signs them before delivery to users. This separation between upload and signing keys provides additional security.

## Section 4: Configuring Signing Variants

Modern Android development often requires multiple signing configurations for different distribution channels. Understanding how to configure these variants ensures your app works correctly across all channels while maintaining appropriate security levels.

```kotlin
// build.gradle.kts with multiple signing configurations

android {
    signingConfigs {
        // Debug signing - auto-generated by Android Studio
        getByName("debug") {
            storeFile = file("${System.getProperty("user.home")}/.android/debug.keystore")
            storePassword = "android"
            keyAlias = "androiddebugkey"
            keyPassword = "android"
        }
        
        // Internal testing signing - uses same key as release but different alias
        create("internal") {
            storeFile = file(keystoreProperties["keystorePath"])
            storePassword = keystoreProperties["storePassword"] as String
            keyAlias = "internal-signing-key"
            keyPassword = keystoreProperties["keyPassword"] as String
        }
        
        // Release signing
        create("release") {
            storeFile = file(keystoreProperties["keystorePath"])
            storePassword = keystoreProperties["storePassword"] as String
            keyAlias = keystoreProperties["keyAlias"] as String
            keyPassword = keystoreProperties["keyPassword"] as String
        }
        
        // Firebase App Distribution signing
        create("firebase") {
            storeFile = file(keystoreProperties["keystorePath"])
            storePassword = keystoreProperties["storePassword"] as String
            keyAlias = "firebase-signing-key"
            keyPassword = keystoreProperties["keyPassword"] as String
        }
    }
    
    flavorDimensions += "distribution"
    
    productFlavors {
        create("internal") {
            dimension = "distribution"
            // Internal builds signed with internal key
            signingConfig = signingConfigs.getByName("internal")
            // Enable debug for easier testing
            isDebuggable = true
            applicationIdSuffix = ".internal"
            versionNameSuffix = "-internal"
        }
        
        create("playstore") {
            dimension = "distribution"
            // Production builds signed with release key
            signingConfig = signingConfigs.getByName("release")
            isDebuggable = false
        }
        
        create("firebase") {
            dimension = "distribution"
            // Firebase builds signed with Firebase key
            signingConfig = signingConfigs.getByName("firebase")
            isDebuggable = true
            applicationIdSuffix = ".firebase"
            versionNameSuffix = "-firebase"
        }
    }
    
    buildTypes {
        release {
            isMinifyEnabled = true
            isShrinkResources = true
            // Use production signing configuration
            signingConfig = signingConfigs.getByName("release")
        }
        debug {
            // Use debug signing for development
            signingConfig = signingConfigs.getByName("debug")
            isDebuggable = true
        }
    }
}
```

## Section 5: Handling Key Migration and Rotation

Key management doesn't end with initial setup. Over time, you may need to rotate keys for security reasons or migrate to new signing approaches. Google Play supports key rotation, allowing you to update the signing key while maintaining compatibility with existing app installations.

```kotlin
// Key rotation management for Google Play App Signing
package com.example.myapp.rotation

import com.google.api.services.androidpublisher.AndroidPublisher
import com.google.api.services.androidpublisher.model.AppVersion

class AppSigningKeyManager(
    private val publisher: AndroidPublisher,
    private val packageName: String
) {
    
    fun checkCurrentSigningKey(): SigningKeyInfo {
        val appEdit = publisher.edits().insert(
            com.google.api.services.androidpublisher.model.AppEdit()
                .setId("key-check")
        ).execute()
        
        // Check existing tracks for signing configuration
        val tracks = publisher.edits().tracks()
            .list(packageName, appEdit.id)
            .execute()
        
        publisher.edits().commit(packageName, appEdit.id).execute()
        
        return SigningKeyInfo(
            packageName = packageName,
            tracks = tracks.tracks?.map { it.track } ?: emptyList(),
            hasAppSigningEnabled = true
        )
    }
    
    fun uploadNewSigningKey(newKeyFile: ByteArray): UploadResult {
        val appEdit = publisher.edits().insert(
            com.google.api.services.androidpublisher.model.AppEdit()
                .setId("key-rotation")
        ).execute()
        
        // Upload new signing key
        val uploadRequest = publisher.projects()
            .appDetectionConfig()
            .upload(packageName, appEdit.id, newKeyFile)
            .execute()
        
        publisher.edits().commit(packageName, appEdit.id).execute()
        
        return UploadResult(
            success = true,
            message = "New signing key uploaded successfully"
        )
    }
    
    fun verifyKeyCompatibility(): CompatibilityResult {
        // Verify that old and new keys are properly configured
        return CompatibilityResult(
            isCompatible = true,
            recommendations = listOf(
                "Test app update with existing users",
                "Monitor crash rates during rollout",
                "Plan 100% rollout after validation"
            )
        )
    }
}

data class SigningKeyInfo(
    val packageName: String,
    val tracks: List<String>,
    val hasAppSigningEnabled: Boolean
)

data class UploadResult(
    val success: Boolean,
    val message: String
)

data class CompatibilityResult(
    val isCompatible: Boolean,
    val recommendations: List<String>
)
```

## Best Practices

- Generate keys with RSA 4096-bit or EC P-256 keys for modern security
- Store keystores in secure, backed-up locations that are not in version control
- Use different signing keys for development, testing, and production environments
- Enable Google Play App Signing to simplify key management and enable key rotation
- Monitor key expiration dates and plan for renewal at least 6 months in advance
- Use environment variables or secure credential stores for passwords in CI/CD
- Implement automated signing in your CI/CD pipeline with proper security controls
- Never share keystores or signing credentials outside your organization
- Document your key management procedures for team onboarding and compliance
- Use separate upload keys from production signing keys when using App Signing

## Common Pitfalls

- **Losing keystore causes permanent app update loss**
  - Solution: Maintain multiple secure backups and consider hardware security modules for critical keys
  
- **Using debug signing for production builds**
  - Solution: Configure proper release signing and enable ProGuard/R8 to strip debug info
  
- **Passwords exposed in build.gradle files**
  - Solution: Use gradle.properties with environment variables or external credential management
  
- **Forgetting key aliases or passwords**
  - Solution: Use a password manager and maintain documented procedures for key management
  
- **Keys expiring causing app update failures**
  - Solution: Monitor key validity proactively and plan for renewal before expiration

## Troubleshooting Guide

**Q: How do I recover a lost keystore?**
A: If you lost your keystore, and you're using App Signing by Google Play, contact Play Developer support. If you lost the upload key for App Signing, they can help reset it. Without App Signing, lost keystores cannot be recovered.

**Q: Can I change my app's signing key?**
A: Yes, but this requires releasing a new app with a new package name. Google Play doesn't support changing the signing key for an existing package.

**Q: Why does my app show as "Not installed" after signing differently?**
A: Android requires the same signing key for updates. If you use a different key, users must uninstall the old version before installing the new one.

**Q: How do I verify my APK signing?**
A: Use `apksigner verify -v myapp.apk` to verify the signing certificate and details.

## Advanced Tips

- Use APK Signature Scheme v3 or v4 for forward-secret key rotation
- Consider using multiple signing keys for different app variants
- Implement signing key validation at app startup to detect tampering
- Use hardware-backed keystores on supported devices for enhanced security
- Explore APK Signature Scheme v2+ incremental updates for faster delivery

## Cross-References

- [Google Play Store](./01_Google_Play_Store.md) - Publishing signed apps
- [Firebase App Distribution](./02_Firebase_App_Distribution.md) - Testing with proper signing
- [App Versioning](./05_App_Versioning.md) - Coordinating version and signing
- [Release Management](./04_Release_Management.md) - Release signing workflows