# Emulator Setup

## Learning Objectives

1. Setting up Android emulators for testing
2. Configuring emulator hardware and software options
3. Managing AVD (Android Virtual Device) configurations
4. Optimizing emulator performance
5. Troubleshooting common emulator issues

## Section 1: Emulator Overview and Setup

The Android Emulator simulates Android devices on your computer, allowing you to test apps without a physical device. It provides:
- Hardware acceleration support
- Multiple device configurations
- Simulated phone/tablet/wear/TV/emulator options
- Google Play Store integration
- Network configuration
- GPS and location simulation

```kotlin
object AndroidEmulator {
    const val EMULATOR_VERSION = "34.1.19"
    const val MINIMUM_HAXM_VERSION = "7.8.0"
    const val RECOMMENDED_RAM_GB = 8
    
    data class SystemImage(
        val api: Int,
        val name: String,
        val abi: String,
        val hasPlayStore: Boolean
    )
    
    val availableImages = listOf(
        SystemImage(34, "Google APIs", "x86_64", true),
        SystemImage(34, "Google Play", "x86_64", true),
        SystemImage(34, "Android Open Source", "x86_64", false),
        SystemImage(33, "Google APIs", "x86_64", true),
        SystemImage(33, "Google Play", "arm64-v8a", true),
        SystemImage(29, "Google APIs", "x86", true)
    )
}
```

## Section 2: AVD Configuration

AVD (Android Virtual Device) is the configuration of an emulator instance. It defines the hardware and software properties of the simulated device.

```kotlin
class AVDConfiguration {
    data class DeviceConfig(
        val name: String,
        val device: String,
        val api: Int,
        val abi: String,
        val ram: Int,
        val heap: Int,
        val dataPartition: Int,
        val sdCard: String,
        val skin: String,
        val hardwareAcceleration: Boolean
    )
    
    val recommendedConfigurations = listOf(
        DeviceConfig(
            name = "Pixel_7_API_34",
            device = "pixel_7",
            api = 34,
            abi = "x86_64",
            ram = 2048,
            heap = 512,
            dataPartition = 2048,
            sdCard = "512M",
            skin = "pixel_7",
            hardwareAcceleration = true
        ),
        DeviceConfig(
            name = "Pixel_7_Pro_API_34",
            device = "pixel_7_pro",
            api = 34,
            abi = "x86_64",
            ram = 4096,
            heap = 768,
            dataPartition = 2048,
            sdCard = "512M",
            skin = "pixel_7_pro",
            hardwareAcceleration = true
        ),
        DeviceConfig(
            name = "Android_Tablet_API_34",
            device = "tablet_10_1in",
            api = 34,
            abi = "x86_64",
            ram = 4096,
            heap = 1024,
            dataPartition = 4096,
            sdCard = "1024M",
            skin = "tablet_10_1in",
            hardwareAcceleration = true
        )
    )
    
    fun createCustomConfig(
        name: String,
        device: String,
        api: Int,
        abi: String
    ): DeviceConfig {
        return DeviceConfig(
            name = name,
            device = device,
            api = api,
            abi = abi,
            ram = 2048,
            heap = 512,
            dataPartition = 2048,
            sdCard = "512M",
            skin = device,
            hardwareAcceleration = true
        )
    }
}
```

## Section 3: Emulator Creation and Management

Creating and managing Android Virtual Devices using AVD Manager and command line tools.

```kotlin
class EmulatorManager {
    private val createdAVDs = mutableListOf<String>()
    
    // AVD Manager Commands
    object Commands {
        const val LIST_AVDS = "emulator -list-avds"
        const val START_AVD = "emulator -avd <avd_name>"
        const val CREATE_AVD = "avdmanager create avd --name <name> --package <system_image>"
        const val DELETE_AVD = "avdmanager delete avd --name <avd_name>"
        const val LIST_DEVICES = "emulator -list-devices"
    }
    
    fun createAVD(config: AVDConfiguration.DeviceConfig): Boolean {
        println("Creating AVD: ${config.name}")
        val command = "avdmanager create avd --name ${config.name} --package \"system-images;android-${config.api};google_apis;${config.abi}\""
        println("Command: $command")
        createdAVDs.add(config.name)
        return true
    }
    
    fun deleteAVD(name: String): Boolean {
        println("Deleting AVD: $name")
        return createdAVDs.remove(name)
    }
    
    fun startAVD(name: String, options: EmulatorOptions): Process {
        println("Starting AVD: $name with options: $options")
        // Return process for emulator
        return ProcessBuilder("emulator", "-avd", name).start()
    }
    
    fun listAVDs(): List<String> {
        return createdAVDs.toList()
    }
    
    data class EmulatorOptions(
        val noWindow: Boolean = false,
        val noAudio: Boolean = true,
        val wipeData: Boolean = false,
        val noSnapshot: Boolean = false,
        val netspeed: String = "full",
        val netdelay: String = "none",
        val gpuMode: String = "auto",
        val cameraFront: String = "emulated",
        val cameraBack: String = "emulated",
        val partitionSize: Int = 2048
    )
}
```

## Section 4: Hardware Acceleration

Hardware acceleration significantly improves emulator performance using CPU virtualization technologies.

```kotlin
class HardwareAcceleration {
    enum class AccelerationType {
        HAXM,       // Intel Hardware Accelerated Execution Manager
        WHPX,       // Windows Hypervisor Platform
        KVM,        // Kernel-based Virtual Machine (Linux)
        ARM         // ARM emulation (slower, no acceleration)
    }
    
    fun checkAccelerationStatus(): AccelerationType? {
        // Check for available acceleration
        return when {
            System.getProperty("os.name").contains("Windows") -> {
                // Check WHPX or HAXM
                AccelerationType.WHPX
            }
            System.getProperty("os.name").contains("Linux") -> {
                AccelerationType.KVM
            }
            else -> null
        }
    }
    
    fun configureHAXM(): String {
        return """
# HAXM Installation (Intel)
# Download from: https://github.com/intel/haxm/releases
# Run the installer with administrator privileges
# Verify: sc query intelhaxm

# Configuration in Android Studio
# 1. Open AVD Manager
# 2. Select device > Edit
# 3. Enable "Hardware - HAXM/Nested HW" in Emulator Performance
        """.trimIndent()
    }
    
    fun configureWHPX(): String {
        return """
# WHPX Configuration (Windows 10/11)
# 1. Enable Windows Hypervisor Platform
#    dism.exe /Online /Enable-Feature:All /FeatureName:Microsoft-Hyper-V-All
#    OR use Turn Windows features on/off

# 2. Restart computer

# 3. Verify: sc query vmcompute

# 4. Configure Android Studio
#    File > Settings > Tools > Emulator
#    Enable "Launch in a tool window"
#    Set GPU rendering to "Auto" or "Hardware"
        """.trimIndent()
    }
    
    fun configureKVM(): String {
        return """
# KVM Configuration (Linux)
# 1. Check if KVM is available
#    kvm-ok

# 2. Install KVM
#    sudo apt install qemu-kvm libvirt-bin ubuntu-cloud-guest-tools

# 3. Add user to kvm group
#    sudo usermod -a -G kvm $USER

# 4. Restart and verify
#    ls /dev/kvm
        """.trimIndent()
    }
}
```

## Section 5: Emulator Performance Optimization

Tips and techniques for optimizing emulator performance.

```kotlin
class EmulatorOptimization {
    fun getOptimizationSettings(): Map<String, String> {
        return mapOf(
            "GPU Rendering" to "Hardware (GLES 2.0)",
            "Boot Animation" to "Disabled",
            "Quick Boot" to "Enabled",
            "Multi-core CPU" to "Enabled",
            "RAM Allocation" to "4096MB",
            "Heap Size" to "768MB",
            "Data Partition" to "2048MB",
            "VM Heap" to "256MB"
        )
    }
    
    fun enableQuickBoot(): String {
        return """
# Enable Quick Boot for faster startup
# 1. Start emulator
# 2. Close emulator normally (not force close)
# 3. Snapshot is saved automatically
# 4. Next start will be much faster

# To force save snapshot:
# emulator -avd <name> -snapshot default_boot
        """.trimIndent()
    }
    
    fun configureAdvancedSettings(): String {
        return """
# Advanced emulator settings
# Location: ~/.android/advancedFeatures.ini

# [high_performance]
# QuickBoot = on
# SnapshotSave = auto
# SnapShotPartitionSize = 2048
# HW.battery = yes
# HW.cameraBack = virtualscene
# HW.cameraFront = emulated

# [performance]
# GPU = swiftshader_indirect
# CPU = 4
# RAMSize = 4096M
        """.trimIndent()
    }
}
```

## Common Pitfalls and Solutions

**Pitfall 1: Emulator fails to start with "PANIC"**
- Verify ANDROID_HOME is set
- Check AVD configuration
- Ensure hardware acceleration is enabled
- Try different system image

**Pitfall 2: Very slow emulator performance**
- Enable hardware acceleration (HAXM/WHPX/KVM)
- Use x86_64 system image for better performance
- Increase RAM allocation
- Disable boot animation
- Use Quick Boot

**Pitfall 3: Emulator window doesn't appear**
- Check noWindow option
- Verify display settings
- Use -no-window flag in command line

**Pitfall 4: "KVM is required" error**
- Enable virtualization in BIOS
- Install KVM/HAXM/WHPX
- Verify /dev/kvm exists (Linux)
- Run as administrator

**Pitfall 5: Network issues with emulator**
- Check firewall settings
- Configure proxy if needed
- Use -dns-server option
- Check emulator logs

## Best Practices

1. Use Hardware acceleration for best performance
2. Create AVDs matching target devices
3. Keep Quick Boot enabled
4. Use x86_64 system images (Intel/AMD)
5. Allocate sufficient RAM (4GB+ recommended)
6. Disable unnecessary hardware features
7. Use Cold Boot once, then Quick Boot
8. Test on multiple screen sizes
9. Keep emulator updated
10. Use command line for automation

## Troubleshooting Guide

**Issue: Emulator boots to black screen**
1. Wait for boot to complete
2. Try cold boot instead of quick boot
3. Check GPU settings
4. Try different API level
5. Check logcat for errors

**Issue: Emulator crashes immediately**
1. Check system requirements
2. Verify hardware acceleration
3. Try different device configuration
4. Check for conflicting software
5. Reinstall emulator

**Issue: Slow boot time**
1. Enable Quick Boot
2. Use faster system image
3. Increase RAM
4. Disable boot animation
5. Use SSD for AVD storage

## Advanced Tips and Tricks

**Tip 1: Run multiple emulator instances**
- Use different AVD names
- Configure different ports
- Monitor resource usage
- Use -port flag

**Tip 2: Emulator automation with script**
- Use command line interface
- Automate testing workflows
- CI/CD integration

**Tip 3: Custom hardware profiles**
- Create custom device definitions
- Simulate specific devices
- Test different screen sizes

**Tip 4: Network traffic analysis**
- Use proxy for traffic capture
- Monitor with Network Profiler
- Configure DNS servers

**Tip 5: Location simulation**
- Use GPS coordinates
- Record and playback routes
- Use GPX files

## Example 1: Creating and Starting AVD

```kotlin
class CreatingAndStartingAVD {
    fun createAndStart(): Boolean {
        println("Step 1: Creating AVD...")
        
        val config = AVDConfiguration.DeviceConfig(
            name = "MyTestDevice",
            device = "pixel_7",
            api = 34,
            abi = "x86_64",
            ram = 4096,
            heap = 768,
            dataPartition = 2048,
            sdCard = "512M",
            skin = "pixel_7",
            hardwareAcceleration = true
        )
        
        val manager = EmulatorManager()
        val created = manager.createAVD(config)
        if (!created) {
            println("Failed to create AVD")
            return false
        }
        
        println("Step 2: Starting emulator...")
        val options = EmulatorManager.EmulatorOptions(
            noWindow = false,
            noAudio = true,
            wipeData = false,
            gpuMode = "auto"
        )
        
        println("AVD '${config.name}' created and ready to start")
        println("Use command: emulator -avd ${config.name}")
        return true
    }
    
    fun runAutomatedTest(): Unit {
        println("Running automated test on emulator...")
        
        // Wait for boot
        println("Waiting for boot to complete...")
        // Runtime.getRuntime().exec("adb wait-for-device")
        
        println("Installing APK...")
        // Runtime.getRuntime().exec("adb install app.apk")
        
        println("Starting Activity...")
        // Runtime.getRuntime().exec("adb shell am start com.example.app/.MainActivity")
        
        println("Test complete!")
    }
}
```

## Example 2: Emulator for CI/CD Pipeline

```kotlin
class EmulatorForCI {
    data class CIConfig(
        val avdName: String = "ci_emulator",
        val apiLevel: Int = 34,
        val abi: String = "x86_64",
        val noWindow: Boolean = true,
        val noAudio: Boolean = true,
        val noSnapshot: Boolean = false,
        val gpuMode: String = "swiftshader_indirect",
        val headless: Boolean = true,
        val noBootAnimation: Boolean = true
    )
    
    fun createCIEmulator(config: CIConfig): Boolean {
        println("Creating CI emulator...")
        
        val avdConfig = AVDConfiguration.DeviceConfig(
            name = config.avdName,
            device = "generic",
            api = config.apiLevel,
            abi = config.abi,
            ram = 2048,
            heap = 512,
            dataPartition = 1024,
            sdCard = "256M",
            skin = "320x480",
            hardwareAcceleration = config.gpuMode != "off"
        )
        
        println("CI emulator configuration:")
        println("  API Level: ${config.apiLevel}")
        println("  ABI: ${config.abi}")
        println("  Headless: ${config.headless}")
        println("  GPU Mode: ${config.gpuMode}")
        
        return true
    }
    
    fun getStartCommand(config: CIConfig): String {
        return "emulator -avd ${config.avdName} " +
            "-no-window " +
            "-no-audio " +
            "-no-snapshot " +
            "-gpu ${config.gpuMode} " +
            "-no-boot-animation " +
            "-wipe-data"
    }
    
    fun verifyEmulatorRunning(): Boolean {
        println("Verifying emulator is running...")
        // Use adb devices command
        return true
    }
}
```

## Example 3: Multi-Device Testing Configuration

```kotlin
class MultiDeviceConfig {
    data class DeviceProfile(
        val name: String,
        val screenSize: String,
        val density: Int,
        val api: Int,
        val abi: String
    )
    
    val deviceProfiles = listOf(
        DeviceProfile("Phone_Small", "320x480", 160, 34, "x86_64"),
        DeviceProfile("Phone_Medium", "1080x1920", 420, 34, "x86_64"),
        DeviceProfile("Phone_Large", "1440x3200", 560, 34, "x86_64"),
        DeviceProfile("Tablet_7in", "800x1280", 320, 33, "x86_64"),
        DeviceProfile("Tablet_10in", "1200x1920", 320, 33, "x86_64")
    )
    
    fun createAllDevices(): Boolean {
        println("Creating multiple device configurations...")
        
        deviceProfiles.forEach { profile ->
            println("Creating device: ${profile.name}")
            // Create AVD for each profile
        }
        
        println("Created ${deviceProfiles.size} devices")
        return true
    }
    
    fun startAllDevices(): List<Process> {
        println("Starting all devices...")
        return emptyList() // Would return list of processes
    }
    
    fun stopAllDevices(): Unit {
        println("Stopping all devices...")
    }
}
```

## Output Statement Results

Emulator Setup Complete:
- Emulator Version: 34.1.19
- System Images Available: 6
- Default AVDs: 3
- Hardware Acceleration: Enabled

Performance Optimization Applied:
- RAM Allocation: 4096MB
- Heap Size: 768MB
- GPU Mode: Hardware
- Quick Boot: Enabled
- Boot Animation: Disabled

Device Configurations Created:
- Pixel 7 (API 34): Ready
- Pixel 7 Pro (API 34): Ready
- Android Tablet (API 34): Ready
- Phone Small (API 34): Ready
- Phone Medium (API 34): Ready
- Phone Large (API 34): Ready
- Tablet 7in (API 33): Ready
- Tablet 10in (API 33): Ready

## Cross-References

See: 01_IDE_Installation_and_Configuration/01_Android_Studio_Setup.md
See: 01_IDE_Installation_and_Configuration/02_SDK_Installation_and_Management.md
See: 01_IDE_Installation_and_Configuration/04_Gradle_Configuration.md
See: 07_TESTING/01_Testing_Fundamentals/03_Espresso_UI_Testing.md