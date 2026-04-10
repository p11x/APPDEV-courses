# Firebase App Distribution

## Learning Objectives

1. Understanding Firebase App Distribution for pre-release testing
2. Setting up Firebase App Distribution in your project
3. Managing testers and tester groups
4. Integrating with CI/CD pipelines
5. Handling feedback and collecting crash reports
6. Optimizing the testing workflow

## Prerequisites

- [Google Play Store](./01_Google_Play_Store.md)
- [App Versioning](./05_App_Versioning.md)

## Section 1: Introduction to Firebase App Distribution

Firebase App Distribution simplifies the process of distributing your app to trusted testers before a full release. It provides a centralized platform for collecting feedback, identifying issues, and ensuring your app is production-ready. This service bridges the gap between internal testing and production release, allowing you to catch issues with real devices and real-world usage scenarios.

The service offers several advantages over traditional testing methods. Testers receive easy-to-install APKs directly through email invitations or a dedicated tester portal. You can organize testers into groups for different testing purposes. The distribution platform supports both Android (APK and AAB) and iOS apps. Integration with Firebase Crashlytics provides automatic crash reporting, and the feedback feature allows testers to submit comments and screenshots directly from the app.

Firebase App Distribution integrates seamlessly with other Firebase services and Google tools. You can use it alongside Firebase Crashlytics for comprehensive issue tracking. The service works with Google Play internal testing for apps that will eventually be published to the Play Store. Integration with Android Studio and the Firebase CLI enables automated distribution from your build pipelines.

## Section 2: Setting Up Firebase App Distribution

Setting up Firebase App Distribution requires configuring your Firebase project and integrating the App Distribution SDK into your app. The setup process involves creating a Firebase project, adding your app, and configuring the distribution service.

First, create a Firebase project in the Firebase Console if you haven't already. Then, add your Android app by registering the app's package name. Download the google-services.json file and place it in your app's module directory. Configure the Google services plugin in your build files.

```kotlin
// build.gradle.kts (project level)
plugins {
    id("com.android.application") version "8.2.0" apply false
    id("org.jetbrains.kotlin.android") version "1.9.0" apply false
    id("com.google.gms.google-services") version "4.4.0" apply false
    id("com.google.firebase.appdistribution") version "4.0.0" apply false
}

// build.gradle.kts (app module)
plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    id("com.google.gms.google-services")
    id("com.google.firebase.appdistribution")
}

android {
    namespace = "com.example.myapp"
    defaultConfig {
        applicationId = "com.example.myapp"
        versionCode = 1
        versionName = "1.0.0"
    }
}

dependencies {
    // Firebase App Distribution
    implementation(platform("com.google.firebase:firebase-bom:32.7.0"))
    implementation("com.google.firebase:firebase-appdistribution-ktx")
    
    // Firebase Crashlytics for crash reporting
    implementation("com.google.firebase:firebase-crashlytics-ktx")
    
    // Google Play Services
    implementation("com.google.android.gms:play-services-base:18.3.0")
}
```

Configure the App Distribution SDK in your app's initialization code. The SDK enables in-app feedback collection and automatic update checking.

```kotlin
// MyApplication.kt
package com.example.myapp

import android.app.Application
import com.google.firebase.appdistribution.FirebaseAppDistribution
import com.google.firebase.appdistribution.FirebaseAppDistributionException
import com.google.firebase.appdistribution.FirebaseAppDistributionUpdatesManager
import com.google.firebase.ktx.Firebase
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.tasks.await

class MyApplication : Application() {
    
    private val applicationScope = CoroutineScope(Dispatchers.Main)
    
    override fun onCreate() {
        super.onCreate()
        
        // Initialize Firebase
        Firebase.initialize(this)
        
        // Check for updates when app starts
        checkForAppDistributionUpdates()
    }
    
    private fun checkForAppDistributionUpdates() {
        applicationScope.launch {
            try {
                val appDistribution = FirebaseAppDistribution.getInstance()
                
                // Check if a new release is available
                val updateInfo = appDistribution.checkForAppDistributionUpdate()
                    .await()
                
                if (updateInfo != null) {
                    // New release available - show update dialog
                    showUpdateDialog(updateInfo)
                }
            } catch (e: FirebaseAppDistributionException) {
                // Handle error - app not distributed via Firebase
                // or service not configured correctly
                logDistributionError(e)
            } catch (e: Exception) {
                // Handle unexpected error
                logDistributionError(e)
            }
        }
    }
    
    private fun showUpdateDialog(updateInfo: com.google.firebase.appdistribution.AppDistributionUpdate) {
        // Show dialog to tester requesting update
        // This would typically be shown in your UI layer
    }
    
    private fun logDistributionError(e: Exception) {
        // Log error for debugging
        android.util.Log.e("MyApp", "App Distribution error", e)
    }
}
```

## Section 3: Managing Testers and Groups

Effective tester management involves organizing testers into logical groups and managing invitations. Group management allows you to target different tester populations for various testing purposes.

```kotlin
// TesterGroupManager.kt
package com.example.myapp.distribution

import com.google.api.services.firebaseappdistribution.v1.FirebaseAppDistribution
import com.google.api.services.firebaseappdistribution.v1.model.*

class TesterGroupManager(
    private val distribution: FirebaseAppDistribution,
    private val projectId: String
) {
    
    suspend fun createTesterGroup(
        name: String,
        displayName: String
    ): TesterGroup {
        val group = TesterGroup()
            .setName(name)
            .setDisplayName(displayName)
        
        return distribution.projects()
            .testerGroups()
            .create(projectId, group)
            .execute()
    }
    
    suspend fun addTestersToGroup(
        groupId: String,
        emails: List<String>
    ): AddTestersResponse {
        val request = AddTestersRequest()
            .setEmails(emails)
        
        return distribution.projects()
            .testerGroups()
            .testers()
            .add(projectId, groupId, request)
            .execute()
    }
    
    suspend fun getTesterGroups(): List<TesterGroup> {
        val groups = distribution.projects()
            .testerGroups()
            .list(projectId)
            .execute()
        
        return groups.groups ?: emptyList()
    }
    
    suspend fun getTestersInGroup(groupId: String): List<Tester> {
        val testers = distribution.projects()
            .testerGroups()
            .testers()
            .list(projectId, groupId)
            .execute()
        
        return testers.testers ?: emptyList()
    }
}
```

Organize your testers into meaningful groups based on testing goals. Create groups like "QA Team" for professional testers, "Beta Users" for engaged early adopters, and "Device Partners" for testing specific hardware configurations.

## Section 4: CI/CD Integration

Integrating Firebase App Distribution with your CI/CD pipeline automates the distribution process, ensuring testers receive new builds quickly after each code change. This integration is essential for continuous testing workflows.

```kotlin
// FirebaseAppDistributionPlugin.kt
package com.example.myapp.ci

import org.gradle.api.DefaultTask
import org.gradle.api.tasks.Input
import org.gradle.api.tasks.TaskAction
import org.gradle.api.provider.Property
import com.google.api.client.googleapis.googleapis
import com.google.api.client.http.FileContent
import java.io.File

open class DistributeToFirebaseTask : DefaultTask() {
    
    @Input
    val serviceAccountFile: Property<String> = project.objects.property(String::class.java)
    
    @Input
    val firebaseProjectId: Property<String> = project.objects.property(String::class.java)
    
    @Input
    val appId: Property<String> = project.objects.property(String::class.java)
    
    @Input
    val apkFile: Property<String> = project.objects.property(String::class.java)
    
    @Input
    val testerGroups: Property<List<String>> = project.objects.property(List::class.java) as Property<List<String>>
    
    @Input
    val releaseNotes: Property<String> = project.objects.property(String::class.java)
    
    @TaskAction
    fun distribute() {
        val credential = ServiceAccountCredentials.fromStream(
            File(serviceAccountFile.get()).inputStream()
        )
        
        val transport = ApacheHttpTransport()
        val jsonFactory = JacksonFactory.getDefaultInstance()
        val initializer = HttpRequestInitializer { request ->
            credential.intercept(request)
        }
        
        val distributor = FirebaseAppDistribution.Builder(transport, jsonFactory, initializer)
            .setApplicationName("MyApp")
            .build()
        
        // Read APK file
        val apk = File(apkFile.get())
        val mediaContent = FileContent("application/vnd.android.package-archive", apk)
        
        // Upload release
        val release = Release()
            .setNotes(releaseNotes.get())
        
        val uploadRequest = AabObject()
            .setRelease(release)
            .setAabFile(Upload())
        
        val uploadedAab = distributor.projects()
            .aabs()
            .upload(firebaseProjectId.get(), appId.get(), mediaContent)
            .execute()
        
        // Distribute to tester groups
        for (groupAlias in testerGroups.get()) {
            val batchRelease = BatchRelease()
                .setAabs(listOf(RelDtoAabRelease().setAabVersionCode(
                    uploadedAab.aabVersionCode)))
            
            distributor.projects()
                .testerGroups()
                .releases()
                .batch(firebaseProjectId.get(), groupAlias, batchRelease)
                .execute()
        }
        
        println("Successfully distributed to Firebase App Distribution")
        println("Release version: ${uploadedAab.aabVersionCode}")
    }
}
```

This task can be registered in your build.gradle.kts and called as part of your CI pipeline:

```kotlin
// In your build.gradle.kts
tasks.register<DistributeToFirebaseTask>("distributeFirebase") {
    serviceAccountFile.set(file("service-account.json"))
    firebaseProjectId.set("my-firebase-project")
    appId.set("1:123456789:android:abc123")
    apkFile.set("$buildDir/outputs/apk/release/myapp-release.apk")
    testerGroups.set(listOf("qa-team", "beta-testers"))
    releaseNotes.set("New feature: User feedback system\nBug fixes: Crash on login screen")
}
```

## Section 5: Collecting and Managing Feedback

The App Distribution SDK includes a feedback feature that allows testers to submit feedback directly from the app. This feedback includes comments, screenshots, and device information, providing valuable context for issue reproduction.

```kotlin
// FeedbackManager.kt
package com.example.myapp.feedback

import android.content.Context
import com.google.firebase.appdistribution.FirebaseAppDistribution
import com.google.firebase.appdistribution.FeedbackListener
import com.google.firebase.appdistribution.FeedbackResult
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlin.coroutines.resume
import kotlin.coroutines.resumeWithException

class FeedbackManager(private val context: Context) {
    
    private val scope = CoroutineScope(Dispatchers.Main)
    
    fun showFeedbackFlow() {
        val firebaseAppDistribution = FirebaseAppDistribution.getInstance()
        
        // Check if app was installed via Firebase App Distribution
        if (!firebaseAppDistribution.isTesterDevice) {
            return  // Only show for distributed test apps
        }
        
        // Start the feedback flow
        firebaseAppDistribution.startFeedbackFlow()
    }
    
    suspend fun submitFeedback(
        message: String,
        screenshotPath: String? = null
    ): FeedbackResult {
        return suspendCancellableCoroutine { continuation ->
            val firebaseAppDistribution = FirebaseAppDistribution.getInstance()
            
            val feedbackConfig = FeedbackConfig.Builder(message)
                .build()
            
            if (screenshotPath != null) {
                val screenshot = android.graphics.BitmapFactory.decodeFile(screenshotPath)
                if (screenshot != null) {
                    feedbackConfig.setScreenshot(screenshot)
                }
            }
            
            firebaseAppDistribution.submitFeedback(feedbackConfig)
                .addOnSuccessListener { result ->
                    continuation.resume(result)
                }
                .addOnFailureListener { error ->
                    continuation.resumeWithException(error)
                }
        }
    }
    
    fun registerFeedbackListener(listener: FeedbackListener) {
        FirebaseAppDistribution.getInstance()
            .addOnFeedbackListener { feedbackResult ->
                // Handle incoming feedback
                handleFeedback(feedbackResult)
            }
    }
    
    private fun handleFeedback(feedbackResult: FeedbackResult) {
        val feedbackInfo = feedbackResult.feedback
        val screenshot = feedbackInfo?.screenshot
        
        // Log or upload feedback for review
        android.util.Log.d("FeedbackManager", 
            "Feedback received: ${feedbackInfo?.userMessage}")
    }
}
```

## Best Practices

- Use meaningful release notes to communicate changes to testers
- Organize testers into groups based on their testing roles and device configurations
- Keep APKs small for easier distribution to testers in different regions
- Integrate with Crashlytics to automatically associate crashes with the distribution version
- Use consistent version naming to track which build each tester is using
- Set up automated distribution from your CI/CD pipeline for rapid iteration
- Collect and organize tester feedback to track issue resolution
- Use the tester portal URL for testers to self-manage their installation
- Enable in-app feedback for seamless tester communication

## Common Pitfalls

- **Upload fails with "App not found"**
  - Solution: Verify your app ID matches exactly what's in the Firebase Console
  
- **Testers not receiving invitation emails**
  - Solution: Check spam folders and ensure email addresses are correct
  
- **APK installation fails on tester devices**
  - Solution: Ensure "Install unknown apps" is enabled in device settings
  
- **In-app feedback not appearing**
  - Solution: Verify the App Distribution SDK is properly initialized and the app was distributed via Firebase
  
- **Release not appearing for testers**
  - Solution: Verify testers have been added to the correct group and have accepted invitations

## Troubleshooting Guide

**Q: How do I get the tester device ID?**
A: Use logcat and filter for "App Distribution" to find the tester device ID in the log output.

**Q: Can I distribute to testers without requiring email invitations?**
A: Yes, use internal app sharing to create public links that anyone with the link can access.

**Q: How do I remove a tester from receiving updates?**
A: Remove the tester from their group in the Firebase Console or use the API to manage testers.

**Q: Can I limit what devices my app is distributed to?**
A: Use device Exclusion patterns in the Firebase Console to filter which devices can install.

**Q: Why are my testers not getting the latest version?**
A: Testers need to have auto-update enabled or manually check for updates in the tester portal.

## Advanced Tips

- Use the Firebase App Distribution REST API to build custom distribution dashboards
- Integrate with Slack or other communication tools to notify testers of new releases
- Use app distribution shortlinks for easy sharing on social media
- Analyze tester engagement metrics to optimize your beta testing programs
- Use the testers endpoint to add testers programmatically for invite-only beta programs

## Cross-References

- [Google Play Store](./01_Google_Play_Store.md) - Next step for production release
- [App Versioning](./05_App_Versioning.md) - Manage versions for distribution
- [Analytics Integration](../02_App_Maintenance/01_Analytics_Integration.md) - Track testing metrics
- [Crash Reporting](../02_App_Maintenance/02_Crash_Reporting.md) - Monitor test build issues
- [User Feedback Systems](../02_App_Maintenance/05_User_Feedback_Systems.md) - Build comprehensive feedback collection