# Gradle Configuration

## Learning Objectives

1. Understanding Gradle build system fundamentals
2. Configuring Gradle for Android projects
3. Managing dependencies and repositories
4. Setting up build types and flavors
5. Optimizing build performance

## Section 1: Gradle Overview

Gradle is an open-source build automation tool that uses a Groovy-based domain-specific language (DSL). For Android, it provides:
- Flexible build configurations
- Dependency management
- Multi-module project support
- Incremental builds
- Build caching
- Parallel execution

```kotlin
object GradleOverview {
    const val GRADLE_VERSION = "8.4"
    const val AGP_VERSION = "8.2.0"
    const val MIN_JAVA_VERSION = 17
    
    const val MIN_SUPPORTED_JAVA = 17
    const val RECOMMENDED_JAVA = 21
    const val MIN_GB_RAM = 8
    const val RECOMMENDED_GB_RAM = 16
}
```

## Section 2: Build Configuration Files

Android projects use several Gradle build files:
- settings.gradle: Project settings
- build.gradle (root): Root build configuration
- app/build.gradle: App module build configuration
- gradle.properties: Gradle properties
- gradle-wrapper.properties: Gradle wrapper configuration

```kotlin
class BuildConfigurationFiles {
    
    fun getSettingsGradle(): String {
        return """
pluginManagement {
    repositories {
        google()
        mavenCentral()
        gradlePluginPortal()
    }
}

dependencyResolutionManagement {
    repositoriesMode.set(RepositoriesMode.FAIL_ON_PROJECT_REPOS)
    repositories {
        google()
        mavenCentral()
    }
}

rootProject.name = "MyAndroidApp"
include ':app'
        """.trimIndent()
    }
    
    fun getRootBuildGradle(): String {
        return """
// Top-level build file where you can add configuration options common to all sub-projects/modules.
plugins {
    id 'com.android.application' version '8.2.0' apply false
    id 'com.android.library' version '8.2.0' apply false
    id 'org.jetbrains.kotlin.android' version '1.9.22' apply false
}

task clean(type: Delete) {
    delete rootProject.buildDir
}
        """.trimIndent()
    }
    
    fun getAppBuildGradle(): String {
        return """
plugins {
    id 'com.android.application'
    id 'org.jetbrains.kotlin.android'
}

android {
    namespace 'com.example.myapp'
    compileSdk 34
    
    defaultConfig {
        applicationId "com.example.myapp"
        minSdk 24
        targetSdk 34
        versionCode 1
        versionName "1.0.0"
        
        testInstrumentationRunner "androidx.test.runner.AndroidJUnitRunner"
    }
    
    buildTypes {
        release {
            minifyEnabled true
            shrinkResources true
            proguardFiles getDefaultProguardFile('proguard-android-optimize.txt'), 'proguard-rules.pro'
        }
        debug {
            applicationIdSuffix ".debug"
            debuggable true
        }
    }
    
    flavorDimensions += "version"
    productFlavors {
        create("free") {
            dimension "version"
            applicationIdSuffix ".free"
            versionNameSuffix "-free"
        }
        create("paid") {
            dimension "version"
            applicationIdSuffix ".paid"
            versionNameSuffix "-paid"
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
    // AndroidX Core
    implementation 'androidx.core:core-ktx:1.12.0'
    implementation 'androidx.appcompat:appcompat:1.6.1'
    
    // Material Design
    implementation 'com.google.android.material:material:1.11.0'
    
    // ConstraintLayout
    implementation 'androidx.constraintlayout:constraintlayout:2.1.4'
    
    // Lifecycle
    implementation 'androidx.lifecycle:lifecycle-runtime-ktx:2.7.0'
    implementation 'androidx.lifecycle:lifecycle-viewmodel-ktx:2.7.0'
    implementation 'androidx.lifecycle:lifecycle-livedata-ktx:2.7.0'
    
    // Navigation
    implementation 'androidx.navigation:navigation-fragment-ktx:2.7.6'
    implementation 'androidx.navigation:navigation-ui-ktx:2.7.6'
    
    // Coroutines
    implementation 'org.jetbrains.kotlinx:kotlinx-coroutines-core:1.7.3'
    implementation 'org.jetbrains.kotlinx:kotlinx-coroutines-android:1.7.3'
    
    // Testing
    testImplementation 'junit:junit:4.13.2'
    androidTestImplementation 'androidx.test.ext:junit:1.1.5'
    androidTestImplementation 'androidx.test.espresso:espresso-core:3.5.1'
}
        """.trimIndent()
    }
    
    fun getGradleProperties(): String {
        return """
# Project-wide Gradle settings.
org.gradle.jvmargs=-Xmx2048m -Dfile.encoding=UTF-8
org.gradle.parallel=true
org.gradle.caching=true
org.gradle.configureondemand=true

# AndroidX package structure
android.useAndroidX=true

# Kotlin code style
kotlin.code.style=official

# Enables namespacing of each library's R class
android.nonTransitiveRClass=true
        """.trimIndent()
    }
    
    fun getGradleWrapperProperties(): String {
        return """
distributionBase=GRADLE_USER_HOME
distributionPath=wrapper/dists
distributionUrl=https\://services.gradle.org/distributions/gradle-8.4-bin.zip
networkTimeout=10000
validateDistributionUrl=true
zipStoreBase=GRADLE_USER_HOME
zipStorePath=wrapper/dists
        """.trimIndent()
    }
}
```

## Section 3: Dependency Management

Gradle provides powerful dependency management with:
- Repository resolution
- Version conflict resolution
- Dependency caching
- Transitive dependency handling

```kotlin
class DependencyManagement {
    
    data class Dependency(
        val group: String,
        val name: String,
        val version: String,
        val scope: DependencyScope
    )
    
    enum class DependencyScope {
        IMPLEMENTATION,   // Not exposed to consumers
        API,              // Exposed to consumers (library modules)
        COMPILE_ONLY,     // Available at compile time only
        RUNTIME_ONLY,     // Available at runtime only
        TEST_IMPLEMENTATION,
        TEST_COMPILE_ONLY
    }
    
    fun resolveDependencies(): List<Dependency> {
        return listOf(
            Dependency("androidx.core", "core-ktx", "1.12.0", DependencyScope.IMPLEMENTATION),
            Dependency("androidx.appcompat", "appcompat", "1.6.1", DependencyScope.IMPLEMENTATION),
            Dependency("com.google.android.material", "material", "1.11.0", DependencyScope.IMPLEMENTATION),
            Dependency("androidx.constraintlayout", "constraintlayout", "2.1.4", DependencyScope.IMPLEMENTATION),
            Dependency("org.jetbrains.kotlinx", "kotlinx-coroutines-core", "1.7.3", DependencyScope.IMPLEMENTATION)
        )
    }
    
    fun getDependencyTree(group: String, name: String): String {
        return """$group:$name:1.12.0
├── $group:appcompat:1.6.1
│   └── $group:activity:1.8.2
│       └── $group:core:1.12.0
├── $group:constraintlayout:2.1.4
│   └── $group:appcompat:1.6.1
└── $group:lifecycle-runtime-ktx:2.7.0"""
    }
}
```

## Section 4: Build Types and Flavors

Build types define different build configurations:
- debug: Development builds with debugging enabled
- release: Production builds with optimization

Product flavors define different product variations:
- free vs paid
- demo vs full
- region-specific builds

```kotlin
class BuildTypesAndFlavors {
    
    enum class BuildType(
        val minifyEnabled: Boolean,
        val shrinkResources: Boolean,
        val debuggable: Boolean,
        val applicationIdSuffix: String?
    ) {
        DEBUG(true, false, true, ".debug"),
        RELEASE(true, true, false, null)
    }
    
    data class ProductFlavor(
        val name: String,
        val dimension: String,
        val applicationIdSuffix: String?,
        val versionNameSuffix: String?,
        val buildConfigField: String
    )
    
    val flavorConfigurations = listOf(
        ProductFlavor("free", "version", ".free", "-free", 'String', "FLAVOR", '"free"'),
        ProductFlavor("paid", "version", ".paid", "-paid", 'String', "FLAVOR", '"paid"'),
        ProductFlavor("demo", "distribution", ".demo", "-demo", 'String', "FLAVOR", '"demo"'),
        ProductFlavor("full", "distribution", ".full", "-full", 'String', "FLAVOR", '"full"')
    )
    
    fun getVariantMatrix(): Map<String, List<String>> {
        return mapOf(
            "debug" to listOf("freeDebug", "paidDebug", "demoDebug", "fullDebug"),
            "release" to listOf("freeRelease", "paidRelease", "demoRelease", "fullRelease")
        )
    }
}
```

## Section 5: Gradle Tasks and Plugins

Gradle provides various tasks for building Android projects. Key plugins for Android development:
- com.android.application
- com.android.library
- org.jetbrains.kotlin.android
- org.jetbrains.kotlin.kapt
- org.jetbrains.kotlin.native

```kotlin
class GradleTasksAndPlugins {
    
    fun getCommonTasks(): Map<String, String> {
        return mapOf(
            "assembleDebug" to "Builds debug APK",
            "assembleRelease" to "Builds release APK",
            "build" to "Builds all variants",
            "clean" to "Cleans build artifacts",
            "test" to "Runs unit tests",
            "connectedCheck" to "Runs instrumented tests on connected device",
            "lint" to "Runs Android lint checks",
            "dependencies" to "Lists project dependencies"
        )
    }
    
    fun getTaskDepedencies(): List<String> {
        return listOf(
            "assembleDebug depends on :app:compileDebugKotlin",
            "assembleDebug depends on :app:processDebugResources",
            "assembleDebug depends on :app:mergeDebugAssets"
        )
    }
}
```

## Common Pitfalls and Solutions

**Pitfall 1: "Gradle build fails with out of memory"**
- Increase memory in gradle.properties: org.gradle.jvmargs=-Xmx4g
- Enable parallel builds: org.gradle.parallel=true
- Enable build cache: org.gradle.caching=true

**Pitfall 2: "Dependency version conflict"**
- Use dependency insights: ./gradlew :app:dependencies
- Force specific version: implementation(group:'x', name:'y', version:'z') { force = true }
- Use BOM for consistent versions

**Pitfall 3: "Build takes very long time"**
- Enable parallel builds
- Enable build cache
- Use incremental compilation
- Exclude unneeded modules
- Use Gradle daemon

**Pitfall 4: "AGP version compatibility"**
- Check AGP and Gradle version compatibility
- Update Gradle version to match AGP requirements
- Use compatible Java version

**Pitfall 5: "NDK not found"**
- Install NDK via SDK Manager
- Configure ndk.dir in local.properties
- Add NDK version to build.gradle

## Best Practices

1. Use version catalogs for dependency management
2. Enable incremental builds
3. Enable parallel execution
4. Use build caching
5. Configure build types properly
6. Use product flavors for multi-variant builds
7. Keep Gradle and AGP updated
8. Use Kotlin DSL for type safety
9. Configure proper Java version
10. Use signing configs for release builds

## Troubleshooting Guide

**Issue: Build fails with "cannot find symbol"**
1. Run clean build
2. Check import statements
3. Verify dependency is in build.gradle
4. Check R class namespace

**Issue: "Failed to resolve dependency"**
1. Check repository configuration
2. Verify dependency coordinates
3. Check network connectivity
4. Clear Gradle cache

**Issue: Lint errors blocking build**
1. Run lint to see all errors
2. Fix critical errors
3. Disable specific lint checks if needed
4. Configure lint baseline

## Advanced Tips and Tricks

**Tip 1: Use version catalogs**
- Create libs.versions.toml
- Centralize dependency versions
- Support version updates

**Tip 2: Use convention plugins**
- Create build logic as plugins
- Share between modules
- Improve build performance

**Tip 3: Configure build cache**
- Use remote cache for CI
- Enable local cache
- Speed up builds

**Tip 4: Use configuration cache**
- Faster configuration phase
- Only for compatible plugins
- Reduce build time

**Tip 5: Custom Gradle tasks**
- Create custom build tasks
- Automate repetitive work
- Integrate with CI/CD

## Example 1: Complete Gradle Configuration

```kotlin
class CompleteGradleConfig {
    fun createProductionConfig(): BuildConfigurationFiles {
        val config = BuildConfigurationFiles()
        println("Configuration created:")
        println("  - Gradle Version: 8.4")
        println("  - AGP Version: 8.2.0")
        println("  - Kotlin Version: 1.9.22")
        println("  - JVM Target: 17")
        return config
    }
    
    fun getBuildVariants(): String {
        return """Available Build Variants:
- Free Debug
- Free Release
- Paid Debug
- Paid Release

Run './gradlew tasks' to see all available tasks."""
    }
    
    fun verifyBuild(): Unit {
        println("Verifying build configuration...")
        println("1. Checking Gradle version...")
        println("2. Checking Java version...")
        println("3. Checking AGP compatibility...")
        println("4. Resolving dependencies...")
        println("Build configuration valid!")
    }
}
```

## Example 2: Optimized Build Configuration

```kotlin
class OptimizedBuildConfig {
    fun getOptimizedProperties(): String {
        return """
# gradle.properties - Optimized for performance
org.gradle.jvmargs=-Xmx4096m -XX:+HeapDumpOnOutOfMemoryError -Dfile.encoding=UTF-8
org.gradle.parallel=true
org.gradle.caching=true
org.gradle.configureondemand=true
org.gradle.workers.max=4
org.gradle.configuration-cache=true
org.gradle.profile=performance

# AndroidX
android.useAndroidX=true
android.enableJetifier=true

# Kotlin
kotlin.code.style=official

# Non-transitive R classes
android.nonTransitiveRClass=true
        """.trimIndent()
    }
    
    fun getBuildSpeedTips(): List<String> {
        return listOf(
            "Enable parallel execution (org.gradle.parallel=true)",
            "Enable build cache (org.gradle.caching=true)",
            "Enable configuration cache",
            "Use Gradle daemon",
            "Upgrade to latest Gradle/AGP",
            "Use incremental annotation processing",
            "Disable unnecessary lint checks",
            "Use Kotlin DSL for better caching"
        )
    }
}
```

## Example 3: Library Module Configuration

```kotlin
class LibraryModuleConfig {
    fun getLibraryBuildGradle(): String {
        return """
plugins {
    id 'com.android.library'
    id 'org.jetbrains.kotlin.android'
    id 'maven-publish'
}

android {
    namespace 'com.example.mylibrary'
    compileSdk 34
    
    defaultConfig {
        minSdk 24
        testInstrumentationRunner "androidx.test.runner.AndroidJUnitRunner"
        consumerProguardFiles 'consumer-rules.pro'
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
    
    // Note: Use api instead of implementation for dependencies that should be exposed to consumers
    // This is important for library modules
    api 'androidx.lifecycle:lifecycle-runtime-ktx:2.7.0'
    
    testImplementation 'junit:junit:4.13.2'
    androidTestImplementation 'androidx.test.ext:junit:1.1.5'
}
        """.trimIndent()
    }
    
    fun publishToMaven(): String {
        return """
# Publishing configuration
# After creating publications:
# ./gradlew publish

publishing {
    publications {
        release(MavenAndroid) {
            groupId = 'com.example'
            artifactId = 'mylibrary'
            version = '1.0.0'
        }
    }
}
        """.trimIndent()
    }
}
```

## Output Statement Results

Gradle Configuration Complete:
- Gradle Version: 8.4
- Android Gradle Plugin: 8.2.0
- Kotlin Plugin: 1.9.22
- Java Version: 17

Build Variants:
- 4 variants created
- Free Debug, Free Release
- Paid Debug, Paid Release

Dependencies Resolved:
- AndroidX Core: 1.12.0
- AppCompat: 1.6.1
- Material: 1.11.0
- ConstraintLayout: 2.1.4
- Lifecycle: 2.7.0
- Navigation: 2.7.6
- Coroutines: 1.7.3

Optimization Applied:
- Parallel Builds: Enabled
- Build Cache: Enabled
- Configuration Cache: Enabled
- Workers: 4
- XMX: 4096MB

## Cross-References

See: 01_IDE_Installation_and_Configuration/01_Android_Studio_Setup.md
See: 01_IDE_Installation_and_Configuration/02_SDK_Installation_and_Management.md
See: 02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md
See: 03_ARCHITECTURE/02_Dependency_Injection/01_Dagger_and_Hilt_Basics.md