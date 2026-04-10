# Robolectric Testing

## Learning Objectives

1. Understanding Robolectric framework
2. Setting up Robolectric tests
3. Testing Android components without emulator
4. Using Robolectric shadows
5. Managing Android resources
6. Testing Activities and Fragments

## Prerequisites

- Android testing basics
- JUnit knowledge
- Android SDK understanding

## Core Concepts

### Robolectric Overview

Robolectric allows testing Android components without emulator:
- **Runs on JVM**: Fast test execution
- **Shadows**: Custom implementations of Android classes
- **Resource handling**: Loads Android resources
- **Lifecycle**: Simulates Activity lifecycle

### Key Features

- **Activity testing**: Test Activities directly
- **Fragment testing**: Test Fragments in isolation
- **Service testing**: Test background services
- **Resource loading**: Access string, drawable, etc.

## Code Examples

### Standard Example: Robolectric Activity Tests

```kotlin
import org.robolectric.*
import org.robolectric.annotation.*
import org.robolectric.shadows.*

@RunWith(RobolectricTestRunner::class)
@Config(sdk = [Build.VERSION_CODES.Q])
class MainActivityRobolectricTest {
    
    @Test
    fun testActivity_CreatesSuccessfully() {
        // Arrange: Build activity intent
        val intent = Intent(Intent.ACTION_VIEW).apply {
            data = Uri.parse("myapp://main")
        }
        
        // Act: Start activity
        val activity = Robolectric.buildActivity(MainActivity::class.java, intent)
            .create()
            .start()
            .resume()
            .get()
        
        // Assert: Activity created
        assertThat(activity).isNotNull()
        assertThat(activity).isResumed()
    }
    
    @Test
    fun testActivity_Lifecycle() {
        // Test complete lifecycle
        val controller = Robolectric.buildActivity(MainActivity::class.java)
        
        // Create
        val activity = controller.create().get()
        assertThat(activity).isCreated()
        
        // Start
        controller.start()
        assertThat(activity).isStarted()
        
        // Resume
        controller.resume()
        assertThat(activity).isResumed()
        
        // Pause
        controller.pause()
        assertThat(activity).isPaused()
        
        // Stop
        controller.stop()
        assertThat(activity).isStopped()
        
        // Destroy
        controller.destroy()
    }
    
    @Test
    fun testActivity_ClickHandled() {
        // Setup activity
        val activity = Robolectric.buildActivity(MainActivity::class.java)
            .create()
            .resume()
            .get()
        
        // Find button and click
        val button = activity.findViewById<Button>(R.id.submitButton)
        Shadows.shadowOf(button).click()
        
        // Assert: Result
        assertThat(activity.resultText).isEqualTo("Button clicked!")
    }
    
    @Test
    fun testActivity_Navigation() {
        // Setup activity
        val activity = Robolectric.buildActivity(MainActivity::class.java)
            .create()
            .resume()
            .get()
        
        // Start navigation
        activity.findViewById<Button>(R.id.nextButton).performClick()
        
        // Check next activity
        val nextStarted = ShadowApplication.getInstance().getNextStartedActivity()
        assertThat(nextStarted).isNotNull()
        assertThat(nextStarted.component.className).isEqualTo(NextActivity::class.java.name)
    }
    
    @Test
    fun testActivity_IntentExtras() {
        // Setup intent with extras
        val intent = Intent().apply {
            putExtra("user_id", 123L)
            putExtra("user_name", "John")
        }
        
        val activity = Robolectric.buildActivity(MainActivity::class.java, intent)
            .create()
            .resume()
            .get()
        
        // Assert: Extras received
        assertThat(activity.userId).isEqualTo(123L)
        assertThat(activity.userName).isEqualTo("John")
    }
}
```

### Real-World Example: Fragment Testing

```kotlin
import org.robolectric.*

@RunWith(RobolectricTestRunner::class)
class UserListFragmentRobolectricTest {
    
    @Test
    fun testFragment_DisplaysUsers() {
        // Setup fragment with arguments
        val fragmentController = SupportFragmentController.of(
            UserListFragment(),
            Bundle().apply {
                putBoolean("show_all", true)
            }
        )
        
        val fragment = fragmentController.create().start().resume().get()
        
        // Wait for data loading
        ShadowLooper.idle()
        
        // Assert: Users displayed
        val recyclerView = fragment.view.findViewById<RecyclerView>(R.id.userList)
        assertThat(recyclerView.adapter.itemCount).isEqualTo(10)
    }
    
    @Test
    fun testFragment_ClickNavigates() {
        val fragment = SupportFragmentController.of(UserListFragment())
            .create()
            .start()
            .resume()
            .get()
        
        // Click on item
        val recyclerView = fragment.view.findViewById<RecyclerView>(R.id.userList)
        Shadows.shadowOf(recyclerView).itemClick(0)
        
        // Check navigation
        val nextStarted = ShadowApplication.getInstance().getNextStartedActivity()
        assertThat(nextStarted).isNotNull()
    }
    
    @Test
    fun testFragment_EmptyState() {
        // Create fragment with no data
        val fragment = SupportFragmentController.of(UserListFragment())
            .create()
            .start()
            .resume()
            .get()
        
        // Assert: Empty state shown
        val emptyView = fragment.view.findViewById<View>(R.id.emptyState)
        assertThat(emptyView.visibility).isEqualTo(View.VISIBLE)
    }
    
    @Test
    fun testFragment_ErrorState() {
        // Setup fragment with error
        val fragmentController = SupportFragmentController.of(
            UserListFragment.newInstance().apply {
                error = "Network error"
            }
        )
        
        val fragment = fragmentController.create().resume().get()
        
        // Assert: Error shown
        val errorView = fragment.view.findViewById<TextView>(R.id.errorText)
        assertThat(errorView.text).isEqualTo("Network error")
    }
}
```

### Real-World Example: Service Testing

```kotlin
import org.robolectric.*

@RunWith(RobolectricTestRunner::class)
class SyncServiceRobolectricTest {
    
    @Test
    fun testService_BindsSuccessfully() {
        // Setup service
        val intent = Intent(Intent.ACTION_VIEW, Uri.parse("myapp://sync"))
        
        val controller = Robolectric.buildService(SyncService::class.java, intent)
            .create()
            .startCommand(0, 0)
        
        // Assert: Service started
        val service = controller.get()
        assertThat(service).isNotNull()
    }
    
    @Test
    fun testService_PerformsSync() {
        // Setup service
        val intent = Intent().apply {
            putExtra("sync_type", "full")
        }
        
        val service = Robolectric.buildService(SyncService::class.java, intent)
            .create()
            .startCommand(0, 0)
            .get()
        
        // Wait for sync
        ShadowLooper.idle()
        
        // Assert: Sync completed
        val pref = ShadowSharedPreference.get("last_sync")
        assertThat(pref).isNotNull()
    }
    
    @Test
    fun testService_HandlesError() {
        // Setup service that will error
        val service = Robolectric.buildService(SyncService::class.java)
            .create()
            .startCommand(0, 0)
            .get()
        
        // Trigger error
        service.triggerError()
        
        // Wait for processing
        ShadowLooper.idle()
        
        // Assert: Error handled
        val notification = ShadowNotificationService.getLatestNotification()
        assertThat(notification).isNotNull()
    }
}
```

### Real-World Example: Resource Loading

```kotlin
import org.robolectric.*

@RunWith(RobolectricTestRunner::class)
class ResourceRobolectricTest {
    
    @Test
    fun testStringResource() {
        // Load string resource
        val string = RuntimeEnvironment.application.getString(R.string.app_name)
        
        assertThat(string).isEqualTo("MyApp")
    }
    
    @Test
    fun testColorResource() {
        // Load color resource
        val color = RuntimeEnvironment.application.getColor(R.color.primary)
        
        assertThat(color).isNotNull()
    }
    
    @Test
    fun testDrawableResource() {
        // Load drawable resource
        val drawable = RuntimeEnvironment.application.getDrawable(R.drawable.ic_launcher)
        
        assertThat(drawable).isNotNull()
    }
    
    @Test
    fun testLayoutResource() {
        // Inflate layout
        val layout = LayoutInflater.from(RuntimeEnvironment.application)
            .inflate(R.layout.activity_main, null)
        
        assertThat(layout).isNotNull()
        assertThat(layout.findViewById<View>(R.id.container)).isNotNull()
    }
    
    @Test
    fun testDimensionResource() {
        // Load dimension
        val dimension = RuntimeEnvironment.application.resources
            .getDimension(R.dimen.padding_medium)
        
        assertThat(dimension).isGreaterThan(0f)
    }
}
```

### Output Results

```
Robolectric Test Results:
- MainActivityRobolectricTest: 5 tests passed
- UserListFragmentRobolectricTest: 4 tests passed
- SyncServiceRobolectricTest: 3 tests passed
- ResourceRobolectricTest: 5 tests passed

Test Execution:
- Activity tests: ~200ms each
- Fragment tests: ~150ms each
- Service tests: ~100ms each
- Resource tests: ~50ms each

Total: 17 tests passed in 3.2s
```

## Best Practices

1. **Use @Config annotation**: Specify SDK version
2. **Test lifecycle**: Use lifecycle methods properly
3. **Use shadows**: Leverage custom shadows
4. **Handle async**: Use ShadowLooper.idle()
5. **Use real resources**: Load real resources
6. **Test navigation**: Verify activity navigation
7. **Test fragments**: Test fragments with SupportFragmentController
8. **Clean state**: Reset between tests

## Common Pitfalls

**Pitfall 1: Missing @Config**
- **Problem**: Uses default SDK
- **Solution**: Always specify SDK in @Config

**Pitfall 2: Not handling async**
- **Problem**: Tests run before async completes
- **Solution**: Use ShadowLooper.idle()

**Pitfall 3: Wrong lifecycle**
- **Problem**: Activity not in expected state
- **Solution**: Use appropriate lifecycle methods

**Pitfall 4: Missing manifest**
- **Problem**: Component not found
- **Solution**: Configure AndroidManifest

**Pitfall 5: Resource loading fails**
- **Problem**: Resources not found
- **Solution**: Add resource directories

## Troubleshooting Guide

**Issue: "Activity not found"**
1. Check build.gradle for androidTestCompile
2. Verify Robolectric dependency
3. Check manifest configuration

**Issue: "Shadow not found"**
1. Use correct Shadow import
2. Add shadow library
3. Check version compatibility

**Issue: "Test timeout"**
1. Use ShadowLooper.idle()
2. Disable animations
3. Increase timeout

## Advanced Tips

**Tip 1: Custom shadows**
```kotlin
@Implements(ShadowMyClass)
class MyClassShadow {
    // Custom implementation
}
```

**Tip 2: Config file**
```kotlin
@Config(constants = BuildConfig::class)
```

**Tip 3: ShadowApplication**
```kotlin
ShadowApplication.getInstance().getString(R.string.app_name)
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/03_Espresso_UI_Testing.md
See: 08_TESTING/02_Advanced_Testing/01_Integration_Testing.md
See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md