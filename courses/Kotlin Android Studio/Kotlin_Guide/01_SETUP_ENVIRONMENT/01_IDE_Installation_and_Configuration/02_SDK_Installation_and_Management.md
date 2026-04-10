# SDK Installation and Management

## Learning Objectives

1. Understanding Android SDK components and their installation
2. Managing SDK tools and build tools
3. Configuring platform versions and API levels
4. Handling SDK updates and component management
5. Troubleshooting common SDK installation issues

## Section 1: Android SDK Overview

The Android SDK is a collection of tools and libraries required for developing Android applications. It includes:
- Platform Tools: adb, fastboot, etc.
- Build Tools: aapt, dx, etc.
- Platforms: Android API levels
- System Images: Emulator system images
- Documentation and APIs

```kotlin
object AndroidSDK {
    const val CURRENT_PLATFORM = 34
    const val MINIMUM_PLATFORM = 24
    const val BUILD_TOOLS_VERSION = "34.0.0"
    const val CMDLINE_TOOLS_VERSION = "11076768"
    
    data class SDKComponent(
        val name: String,
        val version: String,
        val size: Long,
        val installed: Boolean
    )
    
    val essentialComponents = listOf(
        SDKComponent("Build-Tools", BUILD_TOOLS_VERSION, 150_000_000, false),
        SDKComponent("Platforms;android-34", "34", 80_000_000, false),
        SDKComponent("Platform-Tools", "35.0.0", 25_000_000, false),
        SDKComponent("Emulator", "34.1.19", 200_000_000, false)
    )
}
```

## Section 2: SDK Manager Usage

The SDK Manager is used to install, update, and manage SDK components. It can be accessed via:
1. Android Studio: Tools > SDK Manager
2. Command Line: sdkmanager command

```kotlin
class SDKManager {
    private val installedPackages = mutableListOf<String>()
    
    // Common SDK Manager commands
    object Commands {
        const val LIST_INSTALLED = "sdkmanager --list_installed"
        const val INSTALL_PLATFORM = "sdkmanager \"platforms;android-34\""
        const val INSTALL_BUILD_TOOLS = "sdkmanager \"build-tools;34.0.0\""
        const val INSTALL_CMDLINE_TOOLS = "sdkmanager \"cmdline-tools;latest\""
        const val UNINSTALL = "sdkmanager \"platforms;android-33\""
        const val UPDATE_ALL = "sdkmanager --update"
    }
    
    fun installComponent(componentName: String): Boolean {
        println("Installing SDK component: $componentName")
        installedPackages.add(componentName)
        return true
    }
    
    fun uninstallComponent(componentName: String): Boolean {
        println("Uninstalling SDK component: $componentName")
        return installedPackages.remove(componentName)
    }
    
    fun listInstalled(): List<String> {
        return installedPackages.toList()
    }
    
    fun checkUpdates(): List<String> {
        return listOf(
            "platforms;android-34 (installed: 34)",
            "build-tools;34.0.0 (installed: 34.0.0)"
        )
    }
}
```

## Section 3: Environment Configuration

Android SDK requires environment variables to be set:
- ANDROID_HOME: Path to SDK installation
- ANDROID_SDK_ROOT: Alternative to ANDROID_HOME
- PATH: Include platform-tools and cmdline-tools

```kotlin
class EnvironmentConfiguration {
    data class EnvironmentVars(
        val androidHome: String = "C:\\Users\\User\\AppData\\Local\\Android\\Sdk",
        val androidSdkRoot: String = "C:\\Users\\User\\AppData\\Local\\Android\\Sdk",
        val platformTools: String = "C:\\Users\\User\\AppData\\Local\\Android\\Sdk\\platform-tools",
        val buildTools: String = "C:\\Users\\User\\AppData\\Local\\Android\\Sdk\\build-tools\\34.0.0"
    )
    
    fun verifyEnvironment(): Boolean {
        val androidHome = System.getenv("ANDROID_HOME")
        val androidSdkRoot = System.getenv("ANDROID_SDK_ROOT")
        return !androidHome.isNullOrEmpty() || !androidSdkRoot.isNullOrEmpty()
    }
    
    fun getRecommendedPath(): EnvironmentVars {
        return EnvironmentVars()
    }
    
    fun configureWindowsPath(): String {
        return """
# Add to System Properties > Environment Variables > PATH
%ANDROID_HOME%\platform-tools
%ANDROID_HOME%\cmdline-tools\latest\bin
%ANDROID_HOME%\emulator
        """.trimIndent()
    }
    
    fun configureLinuxBashrc(): String {
        return """
# Add to ~/.bashrc or ~/.bash_profile
export ANDROID_HOME=~/Android/Sdk
export PATH=\$PATH:\$ANDROID_HOME/platform-tools:\$ANDROID_HOME/cmdline-tools/latest/bin
        """.trimIndent()
    }
    
    fun configureMacZshrc(): String {
        return """
# Add to ~/.zshrc
export ANDROID_HOME=~/Android/Sdk
export PATH=\$PATH:\$ANDROID_HOME/platform-tools:\$ANDROID_HOME/cmdline-tools/latest/bin
        """.trimIndent()
    }
}
```

## Section 4: SDK Component Details

Different SDK components and their purposes:
- Platforms: API levels (android-24, android-34, etc.)
- Build Tools: Compilation tools (aapt, dx, etc.)
- Platform Tools: Debugging tools (adb, etc.)
- System Images: Emulator images (x86, x86_64, arm64-v8a)

```kotlin
class SDKComponentDetails {
    data class Platform(
        val apiLevel: Int,
        val versionName: String,
        val releaseDate: String,
        val targetPercentage: Float
    )
    
    data class BuildTools(
        val version: String,
        val aaptVersion: String,
        val dxVersion: String,
        val releaseDate: String
    )
    
    val supportedPlatforms = listOf(
        Platform(34, "14", "October 2023", 0.95f),
        Platform(33, "13", "February 2023", 0.90f),
        Platform(31, "12", "August 2021", 0.85f),
        Platform(29, "10", "September 2019", 0.80f),
        Platform(24, "7", "August 2016", 0.75f)
    )
    
    val availableBuildTools = listOf(
        BuildTools("34.0.0", "r", "1.6.0", "May 2024"),
        BuildTools("33.0.1", "r", "1.6.0", "August 2023"),
        BuildTools("32.0.0", "r", "1.5.0", "February 2023"),
        BuildTools("30.0.3", "r", "1.4.3", "May 2021")
    )
}
```

## Section 5: Platform Selection Strategy

Guidelines for selecting target and minimum SDK versions:
- minSdk: Lowest supported API level
- targetSdk: Latest stable API
- compileSdk: Latest available API (for compilation)

```kotlin
class PlatformSelection {
    data class Selection(
        val minSdk: Int = 24,
        val targetSdk: Int = 34,
        val compileSdk: Int = 34
    )
    
    // Recommended selections based on app requirements
    fun getRecommendedSelection(appRequirements: AppRequirements): Selection {
        return when (appRequirements) {
            AppRequirements.MODERN -> Selection(24, 34, 34)
            AppRequirements.BALANCED -> Selection(21, 33, 34)
            AppRequirements.BROAD_COMPATIBILITY -> Selection(16, 31, 34)
        }
    }
    
    enum class AppRequirements {
        MODERN,      // Latest features, limited devices
        BALANCED,    // Good balance of features and compatibility
        BROAD_COMPATIBILITY  // Maximum device coverage
    }
}
```

## Common Pitfalls and Solutions

**Pitfall 1: "sdkmanager" not recognized as command**
- Add cmdline-tools to PATH
- Use full path to sdkmanager
- Download and install command line tools

**Pitfall 2: SDK download stuck or very slow**
- Use mirror or proxy
- Download manually from Android developer site
- Check firewall/proxy settings

**Pitfall 3: Platform or build tools version not available**
- Check available versions with: sdkmanager --list
- Update SDK tools first: sdkmanager "cmdline-tools;latest"
- Verify Android Studio can update SDK

**Pitfall 4: ANDROID_HOME not set correctly**
- Set ANDROID_HOME in system environment variables
- Add ANDROID_HOME to PATH
- Verify path in Android Studio settings

**Pitfall 5: License agreement not accepted**
- Run: yes | sdkmanager --licenses
- Accept licenses in Android Studio
- Manually accept in sdk folder

## Best Practices

1. Use latest stable SDK platform for compilation
2. Set targetSdk to latest stable version
3. Keep minSdk based on app requirements
4. Regularly update build tools
5. Use platform-tools version matching compileSdk
6. Test on multiple SDK versions in emulator
7. Keep SDK components updated for security
8. Use SDK manager for component management
9. Backup SDK installation for quick recovery
10. Document SDK version requirements

## Troubleshooting Guide

**Issue: Command "adb" not found**
1. Verify platform-tools in ANDROID_HOME
2. Add platform-tools to PATH
3. Verify ANDROID_HOME environment variable
4. Reinstall platform-tools via SDK Manager

**Issue: Platform API level not shown in Android Studio**
1. Open SDK Manager
2. Go to SDK Platforms tab
3. Check "Show Package Details"
4. Install required platform

**Issue: Build fails with "compileSdk not specified"**
1. Open app-level build.gradle
2. Add compileSdk version
3. Sync project with Gradle
4. Download required platform if missing

## Advanced Tips and Tricks

**Tip 1: Use local SDK repositories for teams**
- Configure local repository in build.gradle
- Cache SDK components locally
- Share SDK via network drive

**Tip 2: Automated SDK installation**
- Create script with sdkmanager commands
- Use version pinning for reproducibility
- Include in CI/CD pipeline

**Tip 3: Multiple SDK installations**
- Maintain separate SDK folders
- Use ANDROID_HOME to switch
- Quick switching via script

**Tip 4: SDK component caching**
- Configure Gradle caching
- Use SDK Manager with offline mode
- Archive SDK for backup

**Tip 5: Custom SDK distributions**
- Create local SDK mirror
- Use corporate repository
- Offline installation scripts

## Example 1: Standard SDK Installation

```kotlin
class StandardSDKInstallation {
    fun installRequiredComponents(): Boolean {
        println("Starting SDK component installation...")
        
        val components = listOf(
            "\"cmdline-tools;latest\"",
            "\"platforms;android-34\"",
            "\"build-tools;34.0.0\"",
            "\"platform-tools\"",
            "\"emulator\""
        )
        
        components.forEach { component ->
            println("Installing: $component")
            // In practice: Runtime.getRuntime().exec("sdkmanager $component")
        }
        
        println("All components installed successfully!")
        return true
    }
    
    fun verifyInstallation(): SDKManager {
        println("Verifying SDK installation...")
        val manager = SDKManager()
        
        // Check essential components
        AndroidSDK.essentialComponents.forEach { component ->
            println("  ${component.name}: ${component.version}")
        }
        
        return manager
    }
    
    fun configureEnvironment(): EnvironmentConfiguration.EnvironmentVars {
        println("Configuring environment variables...")
        return EnvironmentConfiguration().getRecommendedPath()
    }
}
```

## Example 2: Automated SDK Setup Script

```kotlin
class AutomatedSDKSetup {
    data class SDKScript(
        val os: String,
        val downloadUrl: String,
        val installCommands: List<String>
    )
    
    fun createSetupScript(os: String): SDKScript {
        return when (os.lowercase()) {
            "windows" -> SDKScript(
                "Windows",
                "https://dl.google.com/android/repository/commandlinetools-win-11076768_latest.zip",
                listOf(
                    "mkdir %ANDROID_HOME%\\cmdline-tools",
                    "unzip commandlinetools-win-11076768_latest.zip -d %ANDROID_HOME%\\cmdline-tools",
                    "move %ANDROID_HOME%\\cmdline-tools\\cmdline-tools %ANDROID_HOME%\\cmdline-tools\\latest",
                    "sdkmanager --install \"platforms;android-34\" \"build-tools;34.0.0\""
                )
            )
            "macos" -> SDKScript(
                "macOS",
                "https://dl.google.com/android/repository/commandlinetools-mac-11076768_latest.zip",
                listOf(
                    "mkdir -p $ANDROID_HOME/cmdline-tools",
                    "unzip commandlinetools-mac-11076768_latest.zip -d $ANDROID_HOME/cmdline-tools",
                    "mv $ANDROID_HOME/cmdline-tools/cmdline-tools $ANDROID_HOME/cmdline-tools/latest",
                    "sdkmanager --install \"platforms;android-34\" \"build-tools;34.0.0\""
                )
            )
            "linux" -> SDKScript(
                "Linux",
                "https://dl.google.com/android/repository/commandlinetools-linux-11076768_latest.zip",
                listOf(
                    "mkdir -p $ANDROID_HOME/cmdline-tools",
                    "unzip commandlinetools-linux-11076768_latest.zip -d $ANDROID_HOME/cmdline-tools",
                    "mv $ANDROID_HOME/cmdline-tools/cmdline-tools $ANDROID_HOME/cmdline-tools/latest",
                    "sdkmanager --install \"platforms;android-34\" \"build-tools;34.0.0\""
                )
            )
            else -> throw IllegalArgumentException("Unsupported OS: $os")
        }
    }
    
    fun executeScript(script: SDKScript): Boolean {
        println("Executing setup script for ${script.os}...")
        if (script.os == "Windows") {
            // Execute PowerShell or batch commands
            println("Running: ${script.installCommands.joinToString(" && ")}")
        }
        return true
    }
}
```

## Example 3: Gradle Build Configuration

```kotlin
class GradleBuildConfiguration {
    fun getAppBuildConfig(): String {
        return """
android {
    namespace "com.example.app"
    compileSdk 34
    
    defaultConfig {
        applicationId "com.example.app"
        minSdk 24
        targetSdk 34
        versionCode 1
        versionName "1.0"
        
        testInstrumentationRunner "androidx.test.runner.AndroidJUnitRunner"
    }
    
    buildTypes {
        release {
            minifyEnabled false
            proguardFiles getDefaultProguardFile('proguard-android-optimize.txt'), 'proguard-rules.pro'
        }
    }
    
    compileOptions {
        sourceCompatibility JavaVersion.VERSION_17
        targetCompatibility JavaVersion.VERSION_17
    }
    
    kotlinOptions {
        jvmTarget = '17'
    }
}

dependencies {
    implementation 'androidx.core:core-ktx:1.12.0'
    implementation 'androidx.appcompat:appcompat:1.6.1'
    implementation 'com.google.android.material:material:1.11.0'
    
    testImplementation 'junit:junit:4.13.2'
    androidTestImplementation 'androidx.test.ext:junit:1.1.5'
    androidTestImplementation 'androidx.test.espresso:espresso-core:3.5.1'
}
        """.trimIndent()
    }
    
    fun getProjectBuildConfig(): String {
        return """
// Top-level build file where you can add configuration options common to all sub-projects/modules.
plugins {
    id 'com.android.application' version '8.2.0' apply false
    id 'org.jetbrains.kotlin.android' version '1.9.22' apply false
}
        """.trimIndent()
    }
}
```

## Output Statement Results

SDK Installation Complete:
- Platforms: android-34 (API 34)
- Build Tools: 34.0.0
- Platform Tools: 35.0.0
- Command Line Tools: 11076768
- Emulator: 34.1.19

Environment Configuration:
- ANDROID_HOME: C:\Users\User\AppData\Local\Android\Sdk
- ANDROID_SDK_ROOT: C:\Users\User\AppData\Local\Android\Sdk
- PATH: includes platform-tools, cmdline-tools, emulator

Platform Selection:
- Minimum SDK: 24 (Android 7.0)
- Target SDK: 34 (Android 14)
- Compile SDK: 34

Build Configuration:
- Java Version: 17
- Kotlin JVM Target: 17
- Android Gradle Plugin: 8.2.0
- Kotlin Plugin: 1.9.22

## Cross-References

See: 01_IDE_Installation_and_Configuration/01_Android_Studio_Setup.md
See: 01_IDE_Installation_and_Configuration/03_Emulator_Setup.md
See: 01_IDE_Installation_and_Configuration/04_Gradle_Configuration.md
See: 03_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md
