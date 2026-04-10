# Android Studio Setup

## Learning Objectives

1. Understanding Android Studio IDE installation process
2. Configuring Android Studio for optimal Kotlin development
3. Setting up build tools and platform tools
4. Managing SDK components and updates
5. Configuring IDE appearance and editor settings

## Section 1: Android Studio Overview and Installation

Android Studio is Google's official IDE for Android application development. It is based on IntelliJ IDEA and provides a complete development environment for building, testing, and debugging Android applications.

**System Requirements:**
- Windows: Windows 7 or higher (64-bit)
- macOS: macOS 10.14 or higher
- Linux: GNOME or KDE desktop, glibc++ 3.8 or higher
- Minimum 4GB RAM (8GB recommended)
- 2GB disk space minimum, 4GB recommended
- 1280x800 minimum screen resolution

**Installation Steps:**
1. Download Android Studio from developer.android.com/studio
2. Run the installer executable
3. Follow the installation wizard
4. Select standard or custom installation
5. Complete the installation and launch Android Studio

```kotlin
object AndroidStudioOverview {
    const val VERSION = "2024.1.1"
    const val BUILD_NUMBER = "241.15928.555"
    const val MIN_MEMORY_GB = 4
    const val RECOMMENDED_MEMORY_GB = 8
    const val MIN_DISK_GB = 2
    const val RECOMMENDED_DISK_GB = 4
}
```

## Section 2: Initial Setup and Configuration

When you first launch Android Studio, the setup wizard guides you through:
1. Import settings (from previous installation or default)
2. UI theme selection (Light/Dark)
3. SDK components download
4. Android SDK location configuration
5. Memory allocation settings

**Best Practice:** Use default settings initially, then customize as needed

```kotlin
class InitialSetup {
    enum class Theme {
        LIGHT,
        DARK,
        SYSTEM_DEFAULT
    }
    
    fun configureTheme(theme: Theme): Unit {
        // Implementation: Go to File > Settings > Appearance & Behavior > Appearance
        // Select theme under "Theme" dropdown
        println("Theme configured to: $theme")
    }
    
    fun configureMemoryAllocation(): Unit {
        // Access Help > Edit Custom VM Options
        // Default: -Xmx2048m (2GB)
        // Recommended for development: -Xmx4096m (4GB)
        println("Memory allocation configured")
    }
}
```

## Section 3: Editor Configuration

Android Studio provides extensive editor customization options:
- Font family and size
- Line spacing
- Code style
- Color scheme
- Keymap customization
- Plugin management

```kotlin
class EditorConfiguration {
    data class EditorSettings(
        val fontFamily: String = "Consolas",
        val fontSize: Int = 12,
        val lineSpacing: Float = 1.2f,
        val showLineNumbers: Boolean = true,
        val highlightCurrentLine: Boolean = true,
        val codeCompletionAutoPopup: Boolean = true
    )
    
    fun applySettings(settings: EditorSettings): Unit {
        // Implementation: File > Settings > Editor
        println("Applying editor settings: $settings")
    }
    
    fun configureCodeStyle(): Unit {
        // File > Settings > Editor > Code Style
        // Configure Kotlin code style settings
        println("Code style configured")
    }
    
    fun customizeKeymap(): Unit {
        // File > Settings > Keymap
        // Customize keyboard shortcuts
        println("Keymap customized")
    }
}
```

## Section 4: Plugin Management

Android Studio supports various plugins:
- Kotlin Plugin (built-in)
- Android Gradle Plugin
- Git Integration
- Firebase Services
- Google Cloud Tools
- Third-party plugins (e.g., Material Theme Creator)

```kotlin
object PluginManager {
    val essentialPlugins = listOf(
        "Kotlin",
        "Android Gradle Plugin",
        "Git Integration",
        "Firebase Services"
    )
    
    fun installPlugin(pluginName: String): Boolean {
        // File > Settings > Plugins
        // Search and install from Marketplace
        println("Installing plugin: $pluginName")
        return true
    }
    
    fun enablePlugin(pluginName: String): Unit {
        // File > Settings > Plugins
        // Enable/disable installed plugins
        println("Plugin enabled: $pluginName")
    }
    
    fun disablePlugin(pluginName: String): Unit {
        println("Plugin disabled: $pluginName")
    }
}
```

## Section 5: Project Structure Setup

Android Studio projects have a specific structure:
- app/
  - src/
    - main/
      - java/ (Kotlin source files)
      - res/ (resources: layouts, values, drawables)
      - AndroidManifest.xml
  - build.gradle
- build.gradle (project-level)
- settings.gradle
- gradle.properties

```kotlin
class ProjectStructure {
    fun createProjectStructure(): Map<String, List<String>> {
        return mapOf(
            "app/src/main/java" to listOf("com/example/app"),
            "app/src/main/res" to listOf("layout", "values", "drawable", "menu"),
            "app/src/main/assets" to listOf(),
            "gradle/wrapper" to listOf("gradle-wrapper.jar", "gradle-wrapper.properties")
        )
    }
    
    fun getDefaultManifest(): String {
        return """<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android"
    package="com.example.app">
    
    <application
        android:allowBackup="true"
        android:icon="@mipmap/ic_launcher"
        android:label="@string/app_name"
        android:roundIcon="@mipmap/ic_launcher_round"
        android:supportsRtl="true"
        android:theme="@style/AppTheme">
        
        <activity
            android:name=".MainActivity"
            android:exported="true">
            <intent-filter>
                <action android:name="android.intent.action.MAIN" />
                <category android:name="android.intent.category.LAUNCHER" />
            </intent-filter>
        </activity>
        
    </application>
    
</manifest>"""
    }
}
```

## Common Pitfalls and Solutions

**Pitfall 1: Android Studio not launching after installation**
- Check Java JDK is properly installed (JAVA_HOME environment variable)
- Delete caches folder at %USER%\.AndroidStudio[version]\cache
- Run as administrator

**Pitfall 2: Gradle build fails with out-of-memory error**
- Increase memory allocation in gradle.properties (org.gradle.jvmargs=-Xmx4g)
- Add to gradle.properties: org.gradle.parallel=true

**Pitfall 3: SDK components download failing**
- Check internet connection
- Configure proxy if behind corporate firewall
- Manual download SDK components from Android developer site

**Pitfall 4: Slow IDE performance**
- Disable unnecessary plugins
- Increase IDE memory allocation
- Exclude folders from file indexing
- Use SSD for development

**Pitfall 5: Kotlin plugin outdated**
- Check for updates in File > Settings > Plugins
- Update Kotlin plugin to latest version
- Ensure compatibility with Android Gradle Plugin

## Best Practices

1. Keep Android Studio updated to latest version
2. Regularly update Kotlin and Android Gradle plugins
3. Use SSD for development disk for faster builds
4. Allocate at least 4GB RAM to Android Studio
5. Enable power saver mode during development to reduce CPU usage
6. Use lightweight UI theme during development
7. Disable unused plugins to improve performance
8. Configure appropriate code style and formatter
9. Set up keyboard shortcuts for common actions
10. Enable Gradle daemon for faster builds

## Troubleshooting Guide

**Issue: IDE hangs or becomes unresponsive**
1. Check memory usage in task manager
2. Increase heap size in help > edit custom vm options
3. Invalidate caches: File > Invalidate Caches > Invalidate and Restart
4. Reinstall Android Studio if issue persists

**Issue: Build errors with "Unable to find SDK location"**
1. Verify ANDROID_HOME environment variable is set
2. Check File > Project Structure > SDK Location
3. Download SDK components via SDK Manager
4. Re-sync project with Gradle files

**Issue: Gradle sync fails**
1. Check internet connection
2. Verify Gradle wrapper version compatibility
3. Clean Gradle cache: gradle clean
4. Delete .gradle folder in user home directory
5. Re-import project

## Advanced Tips and Tricks

**Tip 1: Use Live Templates**
- File > Settings > Editor > Live Templates
- Create custom templates for common code patterns
- Use "kt" abbreviation for Kotlin templates

**Tip 2: Enable Power Save Mode**
- Disables background operations
- Reduces CPU and memory usage
- File > Power Save Mode

**Tip 3: Use Scratch Files**
- File > New > Scratch File
- Quick prototyping and testing
- Multiple file support (Kotlin, Java, Python, etc.)

**Tip 4: Enable/Disable Features**
- File > Settings > Advanced Settings
- Disable unused features for better performance

**Tip 5: Use Terminal Window**
- Alt+F12 opens embedded terminal
- Run shell commands without leaving IDE

## Example 1: Standard Android Studio Configuration

```kotlin
class StandardConfiguration {
    fun createStandardConfig(): EditorConfiguration.EditorSettings {
        return EditorConfiguration.EditorSettings(
            fontFamily = "JetBrains Mono",
            fontSize = 14,
            lineSpacing = 1.3f,
            showLineNumbers = true,
            highlightCurrentLine = true,
            codeCompletionAutoPopup = true
        )
    }
    
    fun configureForKotlin(): Unit {
        println("Configuring for Kotlin development...")
        println("1. Set Kotlin as primary language")
        println("2. Configure Kotlin code style")
        println("3. Enable Kotlin extensions")
        println("4. Set up Kotlin compiler")
    }
}
```

## Example 2: Performance-Optimized Configuration

```kotlin
class PerformanceOptimizedConfig {
    data class PerformanceSettings(
        val xmxMB: Int = 4096,
        val xmsMB: Int = 1024,
        val enableParallelBuilds: Boolean = true,
        val enableBuildCache: Boolean = true,
        val enableGradleDaemon: Boolean = true,
        val enableConfigurationCache: Boolean = true,
        val maxWorkers: Int = 4
    )
    
    fun applyPerformanceSettings(settings: PerformanceSettings): String {
        return """
# gradle.properties
org.gradle.jvmargs=-Xmx${settings.xmxMB}m -Xms${settings.xmsMB}m -XX:+HeapDumpOnOutOfMemoryError
org.gradle.parallel=${settings.enableParallelBuilds}
org.gradle.caching=${settings.enableBuildCache}
org.gradle.daemon=${settings.enableGradleDaemon}
org.gradle.configuration-cache=${settings.enableConfigurationCache}
org.gradle.workers.max=${settings.maxWorkers}
android.useAndroidX=true
kotlin.code.style=official
        """.trimIndent()
    }
    
    fun getIDEOptimizations(): List<String> {
        return listOf(
            "Disable unused plugins",
            "Disable power save mode for debugging",
            "Enable memory indicator",
            "Configure file templates",
            "Set up custom keymap"
        )
    }
}
```

## Example 3: Custom Theme Configuration

```kotlin
class CustomThemeConfiguration {
    data class ThemeColors(
        val primaryColor: String = "#3DDC84",
        val accentColor: String = "#FF4081",
        val backgroundColor: String = "#1E1E1E",
        val textColor: String = "#D4D4D4",
        val keywordColor: String = "#569CD6",
        val stringColor: String = "#CE9178",
        val commentColor: String = "#6A9955",
        val functionColor: String = "#DCDCAA"
    )
    
    fun applyCustomTheme(colors: ThemeColors): Unit {
        println("Applying custom theme with colors:")
        println("  Primary: ${colors.primaryColor}")
        println("  Accent: ${colors.accentColor}")
        println("  Background: ${colors.backgroundColor}")
    }
    
    fun exportTheme(): String {
        return """<?xml version="1.0" encoding="utf-8"?>
<scheme name="CustomTheme" parent="Default" version="1">
    <option name="EDITOR_FOREGROUND" value="#D4D4D4" />
    <option name="EDITOR_BACKGROUND" value="#1E1E1E" />
    <option name="CONSOLE_FOREGROUND" value="#D4D4D4" />
    <option name="CONSOLE_BACKGROUND" value="#1E1E1E" />
    <option name="CARET_ROW_COLOR" value="#2A2A2A" />
</scheme>"""
    }
}
```

## Output Statement Results

Android Studio Version: 2024.1.1
Build Number: 241.15928.555
System Requirements:
  - Minimum RAM: 4GB
  - Recommended RAM: 8GB
  - Minimum Disk: 2GB
  - Recommended Disk: 4GB

Standard Configuration Applied:
  - Font: JetBrains Mono, 14pt
  - Line Spacing: 1.3
  - Line Numbers: Enabled
  - Code Completion: Auto-popup Enabled

Performance Settings Applied:
  - Max Heap: 4096MB
  - Initial Heap: 1024MB
  - Parallel Builds: Enabled
  - Build Cache: Enabled
  - Gradle Daemon: Enabled
  - Configuration Cache: Enabled
  - Max Workers: 4

Custom Theme Applied:
  - Primary: #3DDC84 (Android Green)
  - Accent: #FF4081 (Pink)
  - Background: #1E1E1E (Dark)
  - Text: #D4D4D4 (Light Gray)

## Cross-References

See: 01_IDE_Installation_and_Configuration/02_SDK_Installation_and_Management.md
See: 01_IDE_Installation_and_Configuration/03_Emulator_Setup.md
See: 01_IDE_Installation_and_Configuration/04_Gradle_Configuration.md
See: 02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md
