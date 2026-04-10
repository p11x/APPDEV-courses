# Release Management

## Learning Objectives

1. Understanding release management strategies
2. Implementing staged rollouts
3. Managing multiple release tracks
4. Configuring CI/CD for automated releases
5. Handling release failures and rollbacks
6. Optimizing release workflows

## Prerequisites

- [Google Play Store](./01_Google_Play_Store.md)
- [App Signing](./03_App_Signing.md)
- [App Versioning](./05_App_Versioning.md)

## Section 1: Release Management Fundamentals

Release management encompasses all activities required to successfully deliver your app from development to production users. This includes planning releases, building release artifacts, testing releases, deploying to various tracks, monitoring release health, and managing issues that arise. Effective release management balances speed of delivery with stability and user experience.

Modern Android app release management involves several concepts that work together. Track-based releases allow you to control which users receive which versions. Staged rollouts gradually expose your release to more users, providing an early warning system for issues. Rollback capabilities let you quickly reverse a problematic release. Monitoring and alerts keep you informed of release health metrics.

Understanding the release lifecycle in the Play Store helps you design effective release processes. Each release progresses through stages: upload, review, rollout, and monitoring. Issues can arise at any stage, requiring procedures for handling them. Your release management system should address each stage with appropriate tooling and automation.

```kotlin
// Release management data models
package com.example.myapp.release

import java.time.LocalDateTime

data class ReleaseConfiguration(
    val versionCode: Int,
    val versionName: String,
    val releaseNotes: String,
    val targetTrack: ReleaseTrack,
    val rolloutPercentage: Int,
    val releaseMetadata: ReleaseMetadata
)

enum class ReleaseTrack(val displayName: String, val description: String) {
    INTERNAL("Internal Testing", "Quick testing by development team"),
    CLOSED("Closed Beta", "Testing by trusted external testers"),
    OPEN("Open Beta", "Public beta testing"),
    PRODUCTION("Production", "Full production release")
}

data class ReleaseMetadata(
    val minSdkVersion: Int,
    val targetSdkVersion: Int,
    val deviceExclusions: List<DeviceExclusion>,
    val countryTargeting: List<String>,
    val user targeting: UserTargeting?
)

data class DeviceExclusion(
    val deviceModel: String,
    val reason: String
)

data class UserTargeting(
    val accountCountryCodes: List<String>,
    val registrationCountryCodes: List<String>
)

data class ReleaseStatus(
    val versionCode: Int,
    val track: ReleaseTrack,
    val state: ReleaseState,
    val rolloutPercentage: Int,
    val releaseDate: LocalDateTime,
    val crashRate: Double,
    val anrRate: Double,
    val averageRating: Double
)

enum class ReleaseState {
    UPLOADED,
    IN_REVIEW,
    ROLLED_OUT,
    PAUSED,
    ROLLED_BACK,
    SUPERSEDED
}

class ReleaseManager(private val playStoreClient: PlayStoreClient) {
    
    suspend fun createRelease(config: ReleaseConfiguration): ReleaseResult {
        return try {
            // Validate release configuration
            validateRelease(config)
            
            // Upload APK
            val uploadResult = playStoreClient.uploadApk(config)
            
            // Create release in target track
            val releaseResult = playStoreClient.createRelease(
                track = config.targetTrack,
                versionCode = config.versionCode,
                rolloutPercentage = config.rolloutPercentage,
                releaseNotes = config.releaseNotes
            )
            
            ReleaseResult(
                success = true,
                versionCode = config.versionCode,
                track = config.targetTrack,
                message = "Release created successfully"
            )
        } catch (e: Exception) {
            ReleaseResult(
                success = false,
                versionCode = config.versionCode,
                track = config.targetTrack,
                message = "Release failed: ${e.message}"
            )
        }
    }
    
    private fun validateRelease(config: ReleaseConfiguration) {
        require(config.versionCode > 0) { "Invalid version code" }
        require(config.rolloutPercentage in 0..100) { "Rollout percentage must be 0-100" }
        require(config.releaseNotes.isNotBlank()) { "Release notes required" }
    }
    
    suspend fun promoteRelease(
        fromTrack: ReleaseTrack,
        toTrack: ReleaseTrack,
        rolloutPercentage: Int = 10
    ): PromoteResult {
        // Get current version from source track
        val fromRelease = playStoreClient.getCurrentRelease(fromTrack)
        
        // Create release in target track with rollout
        val promoteResult = playStoreClient.promoteRelease(
            fromVersionCode = fromRelease.versionCode,
            toTrack = toTrack,
            rolloutPercentage = rolloutPercentage
        )
        
        return PromoteResult(
            success = true,
            fromTrack = fromTrack,
            toTrack = toTrack,
            versionCode = fromRelease.versionCode,
            newRolloutPercentage = rolloutPercentage
        )
    }
    
    suspend fun rollbackRelease(track: ReleaseTrack): RollbackResult {
        // Stop current rollout
        playStoreClient.stopRollout(track)
        
        // The track will now use the previous release
        return RollbackResult(
            success = true,
            track = track,
            message = "Rollback initiated successfully"
        )
    }
    
    suspend fun monitorReleaseStatus(track: ReleaseTrack): ReleaseStatus {
        return playStoreClient.getReleaseStatus(track)
    }
}
```

## Section 2: Managing Release Tracks

The Play Store's track system provides granular control over release distribution. Understanding each track's purpose and how to use them effectively is essential for safe, rapid releases.

The internal testing track serves as your first validation step. Use it to quickly test builds uploaded to the Play Store without requiring manual APK distribution. This track supports up to 100 testers and is perfect for catching platform-specific issues before wider distribution.

```kotlin
// Internal track management
class InternalTrackManager(private val client: PlayStoreClient) {
    
    suspend fun uploadToInternal(
        apkPath: String,
        releaseNotes: String
    ): InternalReleaseResult {
        // Upload to internal track
        val editId = client.createEdit()
        
        // Upload APK
        val versionCode = client.uploadApk(editId, apkPath)
        
        // Create release in internal track
        client.createRelease(
            editId = editId,
            track = "internal",
            versionCode = versionCode,
            releaseNotes = releaseNotes
        )
        
        // Commit the edit
        client.commitEdit(editId)
        
        return InternalReleaseResult(
            success = true,
            versionCode = versionCode,
            testerCount = getInternalTesterCount()
        )
    }
    
    suspend fun getInternalRelease(): InternalRelease {
        val releases = client.getTrackReleases("internal")
        return releases.first()
    }
    
    private fun getInternalTesterCount(): Int {
        // Get count of testers in internal track
        return 100 // Placeholder
    }
}
```

Closed tracks allow you to test with specific user groups before general release. This is ideal for testing with beta testers who have opted into your beta program. Create multiple closed tracks to test different release variants with different user groups.

```kotlin
// Closed track management with multiple test groups
class ClosedTrackManager(private val client: PlayStoreClient) {
    
    private val trackAliases = mapOf(
        "qa-team" to "closed-qa",
        "beta-users" to "closed-beta",
        "device-partners" to "closed-devices"
    )
    
    suspend fun releaseToTrack(
        apkPath: String,
        trackAlias: String,
        releaseNotes: String
    ): ClosedReleaseResult {
        val trackName = trackAliases[trackAlias]
            ?: throw IllegalArgumentException("Unknown track: $trackAlias")
        
        val editId = client.createEdit()
        val versionCode = client.uploadApk(editId, apkPath)
        
        client.createRelease(
            editId = editId,
            track = trackName,
            versionCode = versionCode,
            releaseNotes = releaseNotes,
            userFraction = 1.0 // All testers in this track
        )
        
        client.commitEdit(editId)
        
        return ClosedReleaseResult(
            success = true,
            trackName = trackName,
            versionCode = versionCode,
            testersNotified = notifyTesters(trackName)
        )
    }
    
    private suspend fun notifyTesters(trackName: String): Int {
        // Notify testers via Firebase In-App Messaging or email
        return 0 // Placeholder
    }
}
```

The production track is where your general audience receives your app. Use staged rollouts to gradually expose updates, monitoring metrics at each stage.

```kotlin
// Production track management with staged rollouts
class ProductionReleaseManager(private val client: PlayStoreClient) {
    
    data class RolloutConfig(
        val initialPercentage: Int = 5,
        val incrementPercentage: Int = 20,
        val holdForMetrics: Long = 24 * 60 * 60 * 1000L, // 24 hours
        val maxCrashRate: Double = 0.01, // 1%
        val maxAnrRate: Double = 0.005 // 0.5%
    )
    
    suspend fun createProductionRelease(
        versionCode: Int,
        releaseNotes: String,
        config: RolloutConfig
    ): ProductionReleaseResult {
        val editId = client.createEdit()
        
        client.createRelease(
            editId = editId,
            track = "production",
            versionCode = versionCode,
            releaseNotes = releaseNotes,
            userFraction = config.initialPercentage / 100.0
        )
        
        client.commitEdit(editId)
        
        // Schedule monitoring
        scheduleHealthCheck(config, versionCode)
        
        return ProductionReleaseResult(
            success = true,
            versionCode = versionCode,
            initialRollout = config.initialPercentage
        )
    }
    
    suspend fun promoteProductionRelease(
        versionCode: Int,
        targetPercentage: Int
    ): PromotionResult {
        val status = client.getReleaseStatus("production", versionCode)
        
        // Check if safe to promote
        if (!isSafeToPromote(status, targetPercentage)) {
            return PromotionResult(
                success = false,
                message = "Release metrics below threshold"
            )
        }
        
        val editId = client.createEdit()
        
        client.updateRelease(
            editId = editId,
            track = "production",
            versionCode = versionCode,
            userFraction = targetPercentage / 100.0
        )
        
        client.commitEdit(editId)
        
        return PromotionResult(
            success = true,
            newPercentage = targetPercentage
        )
    }
    
    private fun isSafeToPromote(
        status: ReleaseStatus,
        targetPercentage: Int
    ): Boolean {
        if (status.crashRate > 0.01 || status.anrRate > 0.005) {
            return false
        }
        return true
    }
    
    private suspend fun scheduleHealthCheck(
        config: RolloutConfig,
        versionCode: Int
    ) {
        // Schedule periodic health checks
    }
}
```

## Section 3: CI/CD Integration

Automating your release process through CI/CD ensures consistent, reliable deployments that can be executed by any authorized team member. This automation reduces human error and speeds up your release cycle.

```kotlin
// Complete CI/CD release pipeline task
package com.example.myapp.release

import org.gradle.api.DefaultTask
import org.gradle.api.tasks.Input
import org.gradle.api.tasks.OutputFile
import org.gradle.api.tasks.TaskAction
import org.gradle.api.tasks.Optional
import java.io.File
import java.text.SimpleDateFormat
import java.util.Date

open class ReleasePipelineTask : DefaultTask() {
    
    @Input
    @Optional
    val releaseType: String = "stable"
    
    @Input
    @Optional
    val initialRollout: Int = 10
    
    @Input
    @Optional
    val releaseNotes: String = ""
    
    @Input
    @Optional
    val dryRun: Boolean = false
    
    @Input
    @Optional
    val skipTests: Boolean = false
    
    @OutputFile
    val outputFile: File = project.file("${buildDir}/release-info.json")
    
    @TaskAction
    fun execute() {
        logger.quiet("=== Starting Release Pipeline ===")
        
        // Step 1: Build the release APK
        if (!skipTests) {
            logger.quiet("Running tests...")
            runtests()
        }
        
        logger.quiet("Building release APK...")
        val apkFile = buildReleaseApk()
        
        // Step 2: Validate build
        logger.quiet("Validating build...")
        validateBuild(apkFile)
        
        // Step 3: Create changelog
        val changelog = generateChangelog(releaseNotes)
        
        // Step 4: Upload to Play Store
        if (!dryRun) {
            logger.quiet("Uploading to Play Store...")
            val versionCode = uploadToPlayStore(apkFile, changelog, initialRollout)
            
            // Write release info for later reference
            writeReleaseInfo(apkFile, versionCode)
            
            logger.quiet("=== Release Pipeline Complete ===")
            logger.quiet("Version: $versionCode")
            logger.quiet("Rollout: $initialRollout%")
        } else {
            logger.quiet("Dry run - skipping upload")
        }
    }
    
    private fun runtests() {
        project.exec {
            commandLine("gradlew", "testReleaseUnitTest", "connectedReleaseAndroidTest")
        }
    }
    
    private fun buildReleaseApk(): File {
        val variant = if (releaseType == "stable") "release" else "release"
        project.exec {
            commandLine("gradlew", "assemble$variant")
        }
        return project.file("app/build/outputs/apk/${variant}/release/app-${variant}-release.apk")
    }
    
    private fun validateBuild(apkFile: File) {
        require(apkFile.exists()) { "APK file not found" }
        
        // Validate APK is properly signed
        project.exec {
            commandLine("apksigner", "verify", "--print-certs", apkFile.absolutePath)
        }
        
        // Check APK size
        val maxSizeMB = 150
        val sizeMB = apkFile.length() / (1024 * 1024)
        require(sizeMB < maxSizeMB) { "APK size exceeds $maxSizeMB MB" }
    }
    
    private fun generateChangelog(customNotes: String): String {
        if (customNotes.isNotBlank()) {
            return customNotes
        }
        // Generate from git commits
        return project.exec {
            commandLine("git", "log", "--oneline", "-10")
        }.let { result ->
            result.out.toString()
        }
    }
    
    private fun uploadToPlayStore(
        apkFile: File,
        changelog: String,
        rollout: Int
    ): Int {
        // Using Play Developer API to upload
        logger.quiet("Would upload: ${apkFile.name}")
        logger.quiet("Rollout: $rollout%")
        logger.quiet("Changelog: $changelog")
        
        // Return version code for output
        return 12345
    }
    
    private fun writeReleaseInfo(apkFile: File, versionCode: Int) {
        val dateFormat = SimpleDateFormat("yyyy-MM-dd HH:mm:ss")
        val info = """
            {
                "versionCode": $versionCode,
                "releaseType": "$releaseType",
                "rolloutPercentage": $initialRollout,
                "releasedAt": "${dateFormat.format(Date())}",
                "apkFile": "${apkFile.name}",
                "changelog": "${releaseNotes.replace("\"", "\\\"")}"
            }
        """.trimIndent()
        
        outputFile.writeText(info)
    }
}

// Register the task in build.gradle.kts
tasks.register<ReleasePipelineTask>("releasePipeline") {
    releaseType.set("stable")
    initialRollout.set(10)
    releaseNotes.set("Bug fixes and performance improvements")
}
```

## Section 4: Monitoring and Rollback

Effective release management requires robust monitoring and the ability to quickly respond to issues. Set up automated health checks and manual override capabilities for handling problems.

```kotlin
// Release health monitoring
class ReleaseHealthMonitor(private val client: PlayStoreClient) {
    
    data class HealthMetrics(
        val crashesPerMillion: Double,
        val anrsPerMillion: Double,
        val averageRating: Double,
        val installConversion: Double,
        val uninstallRate: Double
    )
    
    data class HealthThreshold(
        val maxCrashesPerMillion: Double = 50.0,
        val maxAnrsPerMillion: Double = 25.0,
        val minAverageRating: Double = 4.0,
        val maxUninstallRate: Double = 0.1
    )
    
    suspend fun checkHealth(
        versionCode: Int,
        threshold: HealthThreshold
    ): HealthCheckResult {
        val metrics = getMetrics(versionCode)
        
        val violations = mutableListOf<String>()
        
        if (metrics.crashesPerMillion > threshold.maxCrashesPerMillion) {
            violations.add("Crash rate too high: ${metrics.crashesPerMillion}")
        }
        
        if (metrics.anrsPerMillion > threshold.maxAnrsPerMillion) {
            violations.add("ANR rate too high: ${metrics.anrsPerMillion}")
        }
        
        if (metrics.averageRating < threshold.minAverageRating) {
            violations.add("Rating too low: ${metrics.averageRating}")
        }
        
        if (metrics.uninstallRate > threshold.maxUninstallRate) {
            violations.add("Uninstall rate too high: ${metrics.uninstallRate}")
        }
        
        return HealthCheckResult(
            healthy = violations.isEmpty(),
            metrics = metrics,
            violations = violations,
            recommendation = if (violations.isEmpty()) "Continue rollout" else "Pause and investigate"
        )
    }
    
    private suspend fun getMetrics(versionCode: Int): HealthMetrics {
        // Fetch from Play Console metrics API or Firebase
        return HealthMetrics(
            crashesPerMillion = 10.0,
            anrsPerMillion = 5.0,
            averageRating = 4.5,
            installConversion = 0.85,
            uninstallRate = 0.05
        )
    }
}
```

## Best Practices

- Always test in internal/closed tracks before production rollout
- Start with small rollout percentages and gradually increase
- Implement automated health monitoring that can pause rollouts
- Use semantic versioning for clear communication
- Keep detailed release notes for each version
- Document rollback procedures and test them periodically
- Use CI/CD to automate repetitive release tasks
- Implement multiple approval stages for production releases
- Monitor both technical metrics and user feedback
- Maintain a rollback plan for critical issues

## Common Pitfalls

- **Releasing without checking crash metrics**
  - Solution: Always monitor pre-launch reports and wait 24-48 hours before increasing rollout
  
- **Publishing to wrong track**
  - Solution: Double-check track configuration in your build pipeline
  
- **Forgetting release notes**
  - Solution: Automate changelog generation from version control
  
- **Releasing from local machine**
  - Solution: Use CI/CD pipelines to ensure consistent builds
  
- **Rollback doesn't fully remove the release**
  - Solution: Verify rollback procedures and understand that users may have cached APKs

## Troubleshooting Guide

**Q: Release appears stuck in "In Review"**
A: Initial releases can take 1-3 days. Check Play Console status page for any service issues.

**Q: Crash rate shows zero**
A: Wait 24 hours for crash data to propagate. Also verify Crashlytics is properly integrated.

**Q: Can't increase rollout percentage**
A: Verify no critical issues exist. Contact Play support if the issue persists.

**Q: Want to release to all users immediately**
A: Use the "Promote" feature to set rollout to 100%. Do this after validation.

## Advanced Tips

- Use Play Developer API for programmatic release management
- Implement gradual feature rollouts using feature flags combined with releases
- Use device exclusion to target specific device configurations
- Leverage app bundles for more efficient installation sizes
- Use in-app updates to prompt users to update within the app
- Implement canary releases using multiple production tracks

## Cross-References

- [Google Play Store](./01_Google_Play_Store.md) - Play Store release tracks
- [App Signing](./03_App_Signing.md) - Signing configurations
- [App Versioning](./05_App_Versioning.md) - Version management
- [Firebase App Distribution](./02_Firebase_App_Distribution.md) - Pre-release distribution
- [Crash Reporting](../02_App_Maintenance/02_Crash_Reporting.md) - Release monitoring