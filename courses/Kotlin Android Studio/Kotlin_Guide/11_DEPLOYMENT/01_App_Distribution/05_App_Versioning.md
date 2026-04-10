# App Versioning

## Learning Objectives

1. Understanding Android version code and version name
2. Implementing effective versioning strategies
3. Automating version increments in CI/CD
4. Handling version conflicts and rollback
5. Planning version roadmaps for major releases
6. Configuring version-specific features

## Prerequisites

- [Release Management](./04_Release_Management.md)
- [App Signing](./03_App_Signing.md)

## Section 1: Understanding Version Code and Version Name

Android uses two distinct versioning systems: version code and version name. Understanding how each works is fundamental to proper app version management and is required for successful Play Store submissions.

The version code is an integer value that Android uses internally to determine if one version is newer than another. It must be monotonically increasing for each release - every subsequent release must have a higher version code than the previous one. Version codes are critical for updates because the system compares them to determine whether an update is available. Using a simple incremental number like 1, 2, 3, and so on is the most straightforward approach, though some teams use more complex schemes.

The version name is the user-visible string that appears in the Play Store and device settings. This can follow any format you choose, such as "1.0.0", "Version 2.1.3", or "2024.1". Version names don't have any functional significance for Android's update mechanism - they are purely for user communication. However, they should follow a consistent pattern to avoid confusion.

```kotlin
// Version configuration in build.gradle.kts
android {
    namespace = "com.example.myapp"
    
    defaultConfig {
        // Version code - must be a positive integer
        // Use incrementing numbers: 1, 2, 3, ...
        versionCode = 1
        
        // Version name - user-visible string
        // Use semantic versioning: MAJOR.MINOR.PATCH
        versionName = "1.0.0"
        
        // Other default config options
        minSdk = 24
        targetSdk = 34
        compileSdk = 34
    }
}

// Using version properties in gradle.properties
// gradle.properties:
// app.versionCode=5
// app.versionName=1.2.0

// build.gradle.kts:
android {
    defaultConfig {
        versionCode = project.properties["app.versionCode"].toString().toInt()
        versionName = project.properties["app.versionName"] as String
    }
}
```

For complex projects with multiple build variants, you may need more sophisticated version handling:

```kotlin
// Advanced version configuration with build variants
plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
}

android {
    // Version configuration using root project properties
    defaultConfig {
        // Parse version from project properties or environment
        versionCode = calculateVersionCode()
        versionName = calculateVersionName()
    }
    
    // Version code calculation
    fun calculateVersionCode(): Int {
        // Use git commit count as version code for unique incremental values
        val commitCount = "git rev-list --count HEAD".execute().trim().toInt()
        
        // Or use a base offset for release management
        val baseVersion = 1000
        return baseVersion + commitCount
    }
    
    // Version name based on git tags
    fun calculateVersionName(): String {
        val tag = "git describe --tags --always".execute().trim()
        
        // If no tags, use commit SHA
        return if (tag.startsWith("v")) tag else "dev-$tag"
    }
}

// Execute shell command
fun String.execute(): String {
    return ProcessBuilder("bash", "-c", this)
        .redirectOutput(ProcessBuilder.Redirect.PIPE)
        .start()
        .inputStream.bufferedReader().readText().trim()
}
```

## Section 2: Semantic Versioning for Android

Semantic versioning (SemVer) provides a standardized way to communicate the nature of changes in each release. Following SemVer helps users and stakeholders understand the impact of an update and helps your team maintain a clear release cadence.

The SemVer format is MAJOR.MINOR.PATCH where each component has a specific meaning. MAJOR increments when you make incompatible API changes, MINOR when you add functionality in a backward-compatible manner, and PATCH when you make backward-compatible bug fixes. Additional metadata like alpha, beta, or release candidate designations can indicate pre-release versions.

```kotlin
// Semantic versioning implementation
package com.example.myapp.versioning

import java.time.LocalDateTime
import java.time.format.DateTimeFormatter

data class SemanticVersion(
    val major: Int,
    val minor: Int,
    val patch: Int,
    val preRelease: PreReleaseType? = null,
    val buildMetadata: String? = null
) : Comparable<SemanticVersion> {
    
    override fun compareTo(other: SemanticVersion): Int {
        // Compare major.minor.patch
        val versionCompare = compareValuesBy(
            this, other,
            { it.major }, { it.minor }, { it.patch }
        )
        if (versionCompare != 0) return versionCompare
        
        // Pre-release versions have lower precedence
        val thisPreRelease = this.preRelease?.order ?: Int.MAX_VALUE
        val otherPreRelease = other.preRelease?.order ?: Int.MAX_VALUE
        return thisPreRelease.compareTo(otherPreRelease)
    }
    
    override fun toString(): String {
        val base = "$major.$minor.$patch"
        val preReleaseStr = preRelease?.let { "-$it" } ?: ""
        val metadataStr = buildMetadata?.let { "+$it" } ?: ""
        return "$base$preReleaseStr$metadataStr"
    }
    
    fun incrementMajor(): SemanticVersion = copy(
        major = major + 1,
        minor = 0,
        patch = 0
    )
    
    fun incrementMinor(): SemanticVersion = copy(
        minor = minor + 1,
        patch = 0
    )
    
    fun incrementPatch(): SemanticVersion = copy(
        patch = patch + 1
    )
    
    fun toReleaseCandidate(): SemanticVersion = copy(
        preRelease = PreReleaseType.RC,
        buildMetadata = LocalDateTime.now()
            .format(DateTimeFormatter.ofPattern("yyyyMMddHHmmss"))
    )
    
    fun toBeta(): SemanticVersion = copy(
        preRelease = PreReleaseType.BETA,
        buildMetadata = LocalDateTime.now()
            .format(DateTimeFormatter.ofPattern("yyyyMMddHHmmss"))
    )
    
    fun toAlpha(): SemanticVersion = copy(
        preRelease = PreReleaseType.ALPHA,
        buildMetadata = LocalDateTime.now()
            .format(DateTimeFormatter.ofPattern("yyyyMMddHHmmss"))
    )
    
    fun release(): SemanticVersion = copy(
        preRelease = null,
        buildMetadata = null
    )
}

enum class PreReleaseType(val order: Int) {
    ALPHA(1),
    BETA(2),
    RC(3)
}

class VersionBumpCalculator(
    private val currentVersion: SemanticVersion,
    private val changeType: ChangeType
) {
    
    fun calculateNextVersion(): SemanticVersion {
        return when (changeType) {
            ChangeType.MAJOR -> currentVersion.incrementMajor()
            ChangeType.MINOR -> currentVersion.incrementMinor()
            ChangeType.PATCH -> currentVersion.incrementPatch()
            ChangeType.RELEASE_CANDIDATE -> currentVersion.toReleaseCandidate()
            ChangeType.RELEASE -> currentVersion.release()
        }
    }
}

enum class ChangeType {
    MAJOR,  // Breaking changes
    MINOR,  // New features
    PATCH,  // Bug fixes
    RELEASE_CANDIDATE,
    RELEASE
}

// Example usage
fun main() {
    val currentVersion = SemanticVersion(1, 0, 0)
    
    val calculator = VersionBumpCalculator(currentVersion, ChangeType.MINOR)
    println("Next version: ${calculator.calculateNextVersion()}")
    // Output: 2.0.0
    
    val rcVersion = SemanticVersion(1, 2, 0, PreReleaseType.RC)
    val releaseCalculator = VersionBumpCalculator(rcVersion, ChangeType.RELEASE)
    println("Release version: ${releaseCalculator.calculateNextVersion()}")
    // Output: 1.2.0
}
```

## Section 3: Automating Version Management in CI/CD

Automating version management in your CI/CD pipeline ensures consistency and removes manual errors from the release process. A well-designed version automation system can generate version names from git history, increment versions based on commit messages, and ensure unique version codes for every build.

```kotlin
// CI/CD version automation task
package com.example.myapp.versioning

import org.gradle.api.DefaultTask
import org.gradle.api.tasks.Input
import org.gradle.api.tasks.OutputFile
import org.gradle.api.tasks.TaskAction
import org.gradle.api.provider.Property
import java.io.File
import java.text.SimpleDateFormat
import java.util.Date

open class VersionAutomationTask : DefaultTask() {
    
    @Input
    val versioningStrategy: Property<String> = project.objects.property(String::class.java)
    
    @Input
    val autoIncrement: Property<Boolean> = project.objects.property(Boolean::class.java)
    
    @Input
    val releaseType: Property<String> = project.objects.property(String::class.java)
    
    @OutputFile
    val versionFile: File = project.file("${project.buildDir}/version.properties")
    
    @TaskAction
    fun run() {
        // Read current version from version file or git
        val currentVersion = readCurrentVersion()
        
        // Determine if this is a release or development build
        val isRelease = releaseType.get() == "release"
        
        // Calculate new version
        val newVersion = if (isRelease && autoIncrement.get()) {
            incrementVersion(currentVersion)
        } else {
            currentVersion.copy(
                patch = currentVersion.patch + 1,
                buildMetadata = generateBuildMetadata()
            )
        }
        
        // Generate version code
        val versionCode = calculateVersionCode(newVersion)
        
        // Write version properties for build
        writeVersionProperties(newVersion, versionCode)
        
        // Log version info
        logger.quiet("Version Name: ${newVersion}")
        logger.quiet("Version Code: $versionCode")
    }
    
    private fun readCurrentVersion(): SemanticVersion {
        val versionFile = File("${project.rootDir}/version.txt")
        
        if (versionFile.exists()) {
            val parts = versionFile.readText().trim().split(".")
            return SemanticVersion(
                major = parts.getOrElse(0) { "1" }.toInt(),
                minor = parts.getOrElse(1) { "0" }.toInt(),
                patch = parts.getOrElse(2) { "0" }.toInt()
            )
        }
        
        // Default to 1.0.0 if no version file exists
        return SemanticVersion(1, 0, 0)
    }
    
    private fun incrementVersion(current: SemanticVersion): SemanticVersion {
        val strategy = versioningStrategy.get()
        
        return when (strategy) {
            "major" -> current.copy(
                major = current.major + 1,
                minor = 0,
                patch = 0
            )
            "minor" -> current.copy(
                minor = current.minor + 1,
                patch = 0
            )
            else -> current.copy(patch = current.patch + 1)
        }
    }
    
    private fun generateBuildMetadata(): String {
        val dateFormat = SimpleDateFormat("yyyyMMdd")
        val timeFormat = SimpleDateFormat("HHmm")
        val now = Date()
        
        // Get short git hash
        val gitHash = runCommand("git rev-parse --short HEAD")
            .take(7)
        
        return "${dateFormat.format(now)}-${timeFormat.format(now)}-$gitHash"
    }
    
    private fun calculateVersionCode(version: SemanticVersion): Int {
        // Generate a unique version code from version components
        // Use format: MAJOR * 10000 + MINOR * 100 + PATCH
        return version.major * 10000 + version.minor * 100 + version.patch
    }
    
    private fun writeVersionProperties(version: SemanticVersion, versionCode: Int) {
        val content = """
            VERSION_NAME=${version}
            VERSION_CODE=$versionCode
            MAJOR=${version.major}
            MINOR=${version.minor}
            PATCH=${version.patch}
            BUILD_METADATA=${version.buildMetadata ?: ""}
        """.trimIndent()
        
        versionFile.parentFile?.mkdirs()
        versionFile.writeText(content)
    }
    
    private fun runCommand(command: String): String {
        return try {
            val process = Runtime.getRuntime().exec(arrayOf("bash", "-c", command))
            process.inputStream.bufferedReader().readText().trim()
        } catch (e: Exception) {
            "unknown"
        }
    }
}
```

## Section 4: Feature-Based Version Targeting

Some features may only be available on certain app versions or may behave differently based on the version. Implementing version-aware feature selection allows your app to provide the best experience for each user while gracefully degrading on older versions.

```kotlin
// Feature version compatibility
package com.example.myapp.features

import android.content.Context
import android.os.Build

sealed class FeatureVersion {
    object AlwaysAvailable : FeatureVersion()
    
    data class SinceApi(val apiLevel: Int) : FeatureVersion()
    
    data class UntilApi(val apiLevel: Int) : FeatureVersion()
    
    data class Range(val minApi: Int, val maxApi: Int) : FeatureVersion()
    
    data class VersionRange(val minVersion: String, val maxVersion: String) : FeatureVersion()
}

class FeatureManager(private val context: Context) {
    
    private val packageInfo = context.packageManager
        .getPackageInfo(context.packageName, 0)
    
    val appVersionCode: Long = packageInfo.versionCode.toLong()
    val appVersionName: String = packageInfo.versionName ?: "1.0.0"
    
    fun isFeatureAvailable(feature: FeatureVersion): Boolean {
        return when (feature) {
            is FeatureVersion.AlwaysAvailable -> true
            
            is FeatureVersion.SinceApi -> Build.VERSION.SDK_INT >= feature.apiLevel
            
            is FeatureVersion.UntilApi -> Build.VERSION.SDK_INT <= feature.apiLevel
            
            is FeatureVersion.Range -> Build.VERSION.SDK_INT in 
                feature.minApi..feature.maxApi
            
            is FeatureVersion.VersionRange -> 
                compareVersion(appVersionName, feature.minVersion) >= 0 &&
                compareVersion(appVersionName, feature.maxVersion) <= 0
        }
    }
    
    private fun compareVersion(v1: String, v2: String): Int {
        val parts1 = v1.split(".").map { it.toIntOrNull() ?: 0 }
        val parts2 = v2.split(".").map { it.toIntOrNull() ?: 0 }
        
        for (i in 0..maxOf(parts1.size, parts2.size) - 1) {
            val p1 = parts1.getOrElse(i) { 0 }
            val p2 = parts2.getOrElse(i) { 0 }
            if (p1 != p2) return p1.compareTo(p2)
        }
        return 0
    }
    
    // Feature availability examples
    object Features {
        // Camera X requires API 21+
        val cameraX = FeatureVersion.SinceApi(Build.VERSION_CODES.LOLLIPOP)
        
        // Biometric authentication requires API 23+
        val biometricAuth = FeatureVersion.SinceApi(Build.VERSION_CODES.M)
        
        // Dark theme is only available 29-33
        val darkTheme = FeatureVersion.Range(29, 33)
        
        // New onboarding flow from version 2.5.0
        val newOnboarding = FeatureVersion.VersionRange("2.5.0", "9.0.0")
        
        // Predictive back gesture from API 33
        val predictiveBack = FeatureVersion.SinceApi(Build.VERSION_CODES.TIRAMISU)
    }
}

class FeatureSample private constructor(
    val name: String,
    val featureVersion: FeatureVersion,
    val description: String
) {
    companion object {
        val ALL_FEATURES = listOf(
            FeatureSample("New Camera", FeatureManager.Features.cameraX, "Use CameraX library"),
            FeatureSample("Biometric Login", FeatureManager.Features.biometricAuth, "Fingerprint/face unlock"),
            FeatureSample("Dark Theme", FeatureManager.Features.darkTheme, "System dark mode"),
            FeatureSample("New Onboarding", FeatureManager.Features.newOnboarding, "Redesigned onboarding flow"),
            FeatureSample("Predictive Back", FeatureManager.Features.predictiveBack, "Animation support")
        )
    }
}
```

## Section 5: Version Compatibility and Migration

When you release new versions, you must consider backward compatibility with existing user data and previous app behavior. Proper version handling ensures smooth user experiences during upgrades.

```kotlin
// Version-based migration handling
package com.example.myapp.migration

import android.content.Context
import android.content.SharedPreferences
import androidx.core.content.edit

class VersionMigrationManager(
    private val context: Context,
    private val prefs: SharedPreferences
) {
    
    private val currentVersion: Int
        get() = prefs.getInt(KEY_APP_VERSION, 0)
    
    fun migrateIfNeeded() {
        val databaseVersion = getDatabaseVersion()
        val currentVersion = currentVersion
        
        if (databaseVersion < 1) {
            migrateFromZero()
        }
        
        if (currentVersion < 1) {
            migrateToVersion1()
        }
        
        if (currentVersion < 2) {
            migrateToVersion2()
        }
        
        if (currentVersion < 3) {
            migrateToVersion3()
        }
        
        // Always update version after migrations
        updateVersion(getCurrentVersionCode())
    }
    
    private fun migrateFromZero() {
        // Initial setup - first app launch
        prefs.edit {
            putBoolean(KEY_FIRST_LAUNCH, true)
            putString(KEY_DEFAULT_THEME, "system")
        }
    }
    
    private fun migrateToVersion1() {
        // Version 1 migration: Add new preferences
        prefs.edit {
            putString(KEY_NOTIFICATION_PREFERENCES, "default")
            putBoolean(KEY_ANALYTICS_ENABLED, true)
        }
        
        // Migrate user data format
        migrateUserDataFormat("1.0")
    }
    
    private fun migrateToVersion2() {
        // Version 2 migration: Data structure changes
        prefs.edit {
            remove(KEY_OLD_NOTIFICATION_SETTING)
            putString(KEY_NEW_NOTIFICATION_SETTING, "smart")
        }
        
        // Migrate cached data
        migrateCacheStructure("2.0")
    }
    
    private fun migrateToVersion3() {
        // Version 3: Breaking changes
        prefs.edit {
            putBoolean(KEY_FEATURE_X_ENABLED, true)
            putBoolean(KEY_ONBOARDING_COMPLETED, false) // Re-show onboarding
        }
        
        // Clear incompatible cache
        clearIncompatibleCache()
    }
    
    private fun migrateUserDataFormat(fromVersion: String) {
        // Example: Convert old JSON format to new format
    }
    
    private fun migrateCacheStructure(fromVersion: String) {
        // Example: Recreate cache directory
    }
    
    private fun clearIncompatibleCache() {
        context.cacheDir.deleteRecursively()
    }
    
    private fun getDatabaseVersion(): Int {
        // Read from Room database or preferences
        return prefs.getInt(KEY_DATABASE_VERSION, 0)
    }
    
    private fun updateVersion(versionCode: Int) {
        prefs.edit {
            putInt(KEY_APP_VERSION, versionCode)
        }
    }
    
    private fun getCurrentVersionCode(): Int {
        return try {
            context.packageManager
                .getPackageInfo(context.packageName, 0)
                .versionCode
        } catch (e: Exception) {
            0
        }
    }
    
    companion object {
        private const val KEY_APP_VERSION = "app_version"
        private const val KEY_DATABASE_VERSION = "database_version"
        private const val KEY_FIRST_LAUNCH = "first_launch"
        private const val KEY_DEFAULT_THEME = "default_theme"
        private const val KEY_NOTIFICATION_PREFERENCES = "notification_preferences"
        private const val KEY_ANALYTICS_ENABLED = "analytics_enabled"
        private const val KEY_OLD_NOTIFICATION_SETTING = "old_notification"
        private const val KEY_NEW_NOTIFICATION_SETTING = "notification_setting"
        private const val KEY_FEATURE_X_ENABLED = "feature_x_enabled"
        private const val KEY_ONBOARDING_COMPLETED = "onboarding_completed"
    }
}
```

## Best Practices

- Always increment versionCode for every release; never reuse or decrease values
- Use semantic versioning to communicate the nature of changes clearly
- Automate version management in your CI/CD pipeline to prevent errors
- Store version information in a central location (like version.txt or git tags)
- Document your versioning scheme in CONTRIBUTING.md or similar documentation
- Use versionName for user-facing information and versionCode for system-level comparisons
- Plan for key rotation and version compatibility when updating signing keys
- Test migration code thoroughly before releasing to production
- Use BuildConfig.VERSION_NAME to access version info in code
- Include version information in crash reports for easier debugging

## Common Pitfalls

- **Reusing version codes causes update failures**
  - Solution: Always use a unique incrementing version code; automate this in CI/CD
  
- **Version name doesn't match actual functionality**
  - Solution: Follow semantic versioning strictly; don't skip version numbers
  
- **Forgetting to update version in multiple places**
  - Solution: Centralize version configuration; read from a single source
  
- **Release branch version conflicts**
  - Solution: Use automation to calculate and assign versions from git history
  
- **Hardcoded versions cause maintenance issues**
  - Solution: Use BuildConfig or gradle properties for version information

## Troubleshooting Guide

**Q: Why is my app not updating for some users?**
A: Check that versionCode is higher than the installed version. Also verify the app packageName matches exactly.

**Q: Can I skip version numbers?**
A: You can skip versionName but never skip versionCode. Always increment versionCode.

**Q: How do I handle hotfix releases on older versions?**
A: Use the same versionCode increment logic regardless of which version you're fixing.

**Q: Version code collision in Firebase and Play Store**
A: Ensure both systems use consistent version codes; automate the numbering.

## Advanced Tips

- Use versionCode offsets for different app variants (e.g., free=1000, paid=2000)
- Implement version-based feature toggles for gradual rollouts
- Use BuildConfig fields for runtime version checks
- Track version distribution using analytics to plan updates
- Implement "forced update" functionality for critical security updates

## Cross-References

- [Google Play Store](./01_Google_Play_Store.md) - Publishing with version requirements
- [Release Management](./04_Release_Management.md) - Release workflow with versioning
- [App Signing](./03_App_Signing.md) - Version signing requirements
- [Firebase App Distribution](./02_Firebase_App_Distribution.md) - Pre-release version testing