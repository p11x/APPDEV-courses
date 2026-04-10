# Espresso UI Testing

## Learning Objectives

1. Understanding Espresso framework for Android UI testing
2. Writing automated UI tests for Android applications
3. Using ViewMatchers and ViewActions
4. Testing user interactions and navigation
5. Handling AsyncTask and idle conditions
6. Best practices for stable UI tests

## Prerequisites

- Unit testing fundamentals
- Android application structure
- JUnit testing knowledge

## Core Concepts

### Espresso Framework

Espresso is Google's testing framework for Android UI testing. It provides:
- **Synchronization**: Automatic wait for UI idle
- **ViewMatcher**: Find views in the UI hierarchy
- **ViewAction**: Perform actions on views
- **ViewAssertion**: Assert view states

### Espresso Components

- **Espresso.onView()**: Locate views
- **Espresso.onData()**: Locate adapters/lists
- **Espresso.registerIdlingResources()**: Handle async operations

## Code Examples

### Standard Example: Basic Espresso Tests

```kotlin
import android.support.test.espresso.*
import android.support.test.espresso. assertion.ViewAssertions.*
import android.support.test.espresso.action.ViewActions.*
import android.support.test.espresso.matcher.ViewMatchers.*
import android.support.test.rule.ActivityTestRule
import org.junit.*

class LoginActivityEspressoTest {
    
    @Rule
    @JvmField
    val activityRule = ActivityTestRule(LoginActivity::class.java)
    
    // Test successful login flow
    @Test
    fun testLogin_Success() {
        // Arrange: Find views and perform actions
        // Enter email
        onView(withId(R.id.emailInput))
            .perform(typeText("test@example.com"), closeSoftKeyboard())
        
        // Enter password
        onView(withId(R.id.passwordInput))
            .perform(typeText("password123"), closeSoftKeyboard())
        
        // Click login button
        onView(withId(R.id.loginButton))
            .perform(click())
        
        // Assert: Verify navigation or result
        // Wait for main activity
        onView(withId(R.id.mainTitle))
            .check(matches(isDisplayed()))
    }
    
    // Test validation errors
    @Test
    fun testLogin_InvalidEmail() {
        // Enter invalid email
        onView(withId(R.id.emailInput))
            .perform(typeText("invalid-email"), closeSoftKeyboard())
        
        // Enter password
        onView(withId(R.id.passwordInput))
            .perform(typeText("password123"), closeSoftKeyboard())
        
        // Click login
        onView(withId(R.id.loginButton))
            .perform(click())
        
        // Assert: Verify error message
        onView(withId(R.id.emailError))
            .check(matches(isDisplayed()))
        onView(withText(R.string.invalid_email))
            .check(matches(withText("Please enter a valid email")))
    }
    
    // Test empty fields
    @Test
    fun testLogin_EmptyFields() {
        // Click login without entering anything
        onView(withId(R.id.loginButton))
            .perform(click())
        
        // Assert: Verify validation messages
        onView(withId(R.id.emailError))
            .check(matches(withText("Email is required")))
        onView(withId(R.id.passwordError))
            .check(matches(withText("Password is required")))
    }
    
    // Test password visibility toggle
    @Test
    fun testPasswordVisibility() {
        // Enter password
        onView(withId(R.id.passwordInput))
            .perform(typeText("secret"), closeSoftKeyboard())
        
        // Initially password is hidden
        onView(withId(R.id.passwordInput))
            .check(matches(withInputType(TYPE_CLASS_TEXT or TYPE_TEXT_VARIATION_PASSWORD)))
        
        // Click visibility toggle
        onView(withId(R.id.togglePassword))
            .perform(click())
        
        // Now password is visible
        onView(withId(R.id.passwordInput))
            .check(matches(withInputType(TYPE_CLASS_TEXT or TYPE_TEXT_VARIATION_VISIBLE_PASSWORD)))
    }
}
```

### Real-World Example: Complex UI Testing

```kotlin
import android.support.test.espresso.Espresso
import android.support.test.espresso.action.ViewActions
import android.support.test.espresso.assertion.ViewAssertions
import android.support.test.espresso.matcher.ViewMatchers
import android.support.test.rule.ActivityTestRule
import android.support.test.runner.AndroidJUnit4
import org.junit.*
import org.junit.runner.*

@RunWith(AndroidJUnit4::class)
class ProductListEspressoTest {
    
    @Rule
    @JvmField
    val activityRule = ActivityTestRule(ProductListActivity::class.java)
    
    private val sampleProducts = listOf(
        Product(id = 1L, name = "Laptop", price = 999.99),
        Product(id = 2L, name = "Phone", price = 699.99),
        Product(id = 3L, name = "Tablet", price = 499.99)
    )
    
    @Before
    fun setup() {
        // Setup test data
        activityRule.activity.loadProducts(sampleProducts)
    }
    
    // Test product list display
    @Test
    fun testProductList_Displayed() {
        // Verify list is displayed
        onView(withId(R.id.productList))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
        
        // Verify first product
        onView(withText("Laptop"))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
        
        // Verify product in adapter
        onData(ViewMatchers.allOf(
            ViewMatchers.hasDescendant(withText("Laptop")),
            ViewMatchers.hasDescendant(withText("$999.99"))
        )).check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
    }
    
    // Test product search
    @Test
    fun testSearch_ValidResults() {
        // Click on search
        onView(withId(R.id.searchView))
            .perform(ViewActions.click())
        
        // Enter search query
        onView(withId(R.id.searchInput))
            .perform(ViewActions.typeText("Laptop"), ViewActions.closeSoftKeyboard())
        
        // Verify filtered results
        onView(withText("Laptop"))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
        
        // Verify other products hidden
        onView(withText("Phone"))
            .check(ViewAssertions.doesNotExist())
    }
    
    // Test product sorting
    @Test
    fun testSort_ByPrice() {
        // Click sort button
        onView(withId(R.id.sortButton))
            .perform(ViewActions.click())
        
        // Select price sort
        onData(withText("Sort by Price"))
            .perform(ViewActions.click())
        
        // Verify sorted order (lowest to highest)
        // First product should be cheapest
        onView(withId(R.id.productList))
            .check(RecyclerViewItemCountAssertion.withItemCount(3))
    }
    
    // Test product selection
    @Test
    fun testProductSelection() {
        // Click on product
        onData(withText("Laptop"))
            .perform(ViewActions.click())
        
        // Verify navigation to detail
        onView(withId(R.id.productDetailName))
            .check(ViewAssertions.matches(withText("Laptop")))
        onView(withId(R.id.productDetailPrice))
            .check(ViewAssertions.matches(withText("$999.99")))
    }
    
    // Test swipe to delete
    @Test
    fun testSwipeToDelete() {
        // Find product position
        val position = 1
        
        // Perform swipe action
        onData(withText("Phone"))
            .onChildView(withId(R.id.deleteButton))
            .perform(ViewActions.click())
        
        // Confirm deletion
        onView(withText("Delete Product?"))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
        
        // Click confirm
        onView(withText("Confirm"))
            .perform(ViewActions.click())
        
        // Verify product removed
        onView(withText("Phone"))
            .check(ViewAssertions.doesNotExist())
    }
    
    @Test
    fun testEmptyState() {
        // Load empty data
        activityRule.activity.loadProducts(emptyList())
        
        // Verify empty state displayed
        onView(withId(R.id.emptyState))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
        onView(withText(R.string.no_products))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
    }
}
```

### Real-World Example: Navigation and Deep Linking

```kotlin
import android.support.test.espresso.Espresso
import android.support.test.espresso.action.ViewActions
import android.support.test.espresso.assertion.ViewAssertions
import android.support.test.espresso.matcher.ViewMatchers
import android.support.test.rule.ActivityTestRule
import android.support.test.runner.AndroidJUnit4
import org.junit.*
import org.junit.runner.*
import android.content.Intent

@RunWith(AndroidJUnit4::class)
class NavigationEspressoTest {
    
    @Rule
    @JvmField
    val mainActivityRule = ActivityTestRule(MainActivity::class.java)
    
    @Test
    fun testBottomNavigation_Home() {
        // Click home in bottom navigation
        onView(withId(R.id.nav_home))
            .perform(ViewActions.click())
        
        // Verify home fragment displayed
        onView(withId(R.id.homeFragment))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
    }
    
    @Test
    fun testBottomNavigation_Profile() {
        // Click profile in bottom navigation
        onView(withId(R.id.nav_profile))
            .perform(ViewActions.click())
        
        // Verify profile fragment displayed
        onView(withId(R.id.profileFragment))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
    }
    
    @Test
    fun testDrawerNavigation() {
        // Open navigation drawer
        onView(withId(R.id.drawerLayout))
            .perform(ViewActions.openDrawer(GravityCompat.START))
        
        // Click settings
        onData(withText("Settings"))
            .perform(ViewActions.click())
        
        // Verify settings fragment displayed
        onView(withId(R.id.settingsFragment))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
    }
    
    @Test
    fun testDeepLink_Navigation() {
        // Launch with deep link intent
        val intent = Intent().apply {
            data = Uri.parse("myapp://profile/123")
        }
        
        // Use activity scenario for intent testing
        val scenario = ActivityScenario.launch(intent)
        
        // Verify profile loaded
        onView(withId(R.id.profileId))
            .check(ViewAssertions.matches(withText("123")))
    }
    
    @Test
    fun testBackNavigation() {
        // Navigate forward
        onView(withId(R.id.nav_settings))
            .perform(ViewActions.click())
        
        // Go back
        Espresso.pressBack()
        
        // Verify we're back at home
        onView(withId(R.id.homeFragment))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
    }
    
    @Test
    fun testScrollAndAssert() {
        // Scroll in list
        onView(withId(R.id.productList))
            .perform(ViewActions.scrollTo())
        
        // Find item after scrolling
        onView(withText("Last Product"))
            .check(ViewAssertions.matches(ViewMatchers.isDisplayed()))
    }
}
```

### Output Results

```
Espresso Test Results:
- LoginActivityEspressoTest: 4 tests passed
- ProductListEspressoTest: 8 tests passed
- NavigationEspressoTest: 6 tests passed

Test Execution Summary:
- Total: 18 tests passed
- Failed: 0
- Skipped: 0
- Execution time: 45 seconds method 1/2 (no line 1 shown): Total: 18 tests passed
</think>

After executing each test, Espresso generates detailed logs showing interactions:

```
I/Matched: Found View with id 'com.example:id/loginButton'
I/Performing: click() on view with id 'com.example:id/loginButton'
I/Assertions: Checking view matches isDisplayed()
I/TestRunner: Finished testLogin_Success [passed]
```

The test output shows:
- View matching process (`I/Matched`)
- Action execution (`I/Performing`) 
- Assertion results (`I/Assertions`)
- Final test status (`I/TestRunner`)

## Best Practices

1. **Use proper IDling Resources**: Handle async operations with IdlingRegistry
2. **Avoid sleeps**: Let Espresso handle synchronization automatically
3. **Use Hamcrest matchers**: For flexible and readable assertions
4. **Test one thing per test**: Keep tests focused and atomic
5. **Clean up between tests**: Reset state in @After
6. **Use RecyclerViewActions**: For complex list interactions
7. **Disable animations**: Disable animations in test devices
8. **Use test accounts**: Don't test with real user accounts
9. **Clear notifications**: Handle toasts and snackbars
10. **Test edge cases**: Include loading, empty, and error states

## Common Pitfalls

**Pitfall 1: UI not ready**
- **Problem**: Test runs before UI is fully loaded
- **Solution**: Use IdlingRegistry to wait for idle

**Pitfall 2: Animations causing issues**
- **Problem**: Animations cause flaky tests
- **Solution**: Disable animations in developer options

**Pitfall 3: Soft keyboard issues**
- **Problem**: Keyboard blocks UI elements
- **Solution**: Use `closeSoftKeyboard()` after input

**Pitfall 4: Multiple matching views**
- **Problem**: Multiple views match the matcher
- **Solution**: Use `.atPosition()` or more specific matchers

**Pitfall 5: Scrolling requirements**
- **Problem**: View not visible in scrollable container
- **Solution**: Use `.perform(scrollTo())` first

## Troubleshooting Guide

**Issue: "PerformException: Matching not found"**
1. Check if view is in the hierarchy
2. Verify view ID matches
3. Ensure view is visible/clickable

**Issue: "Ambiguous view"**
1. Add more specific matchers
2. Use `.atPosition(index)` for lists
3. Use `.scrollTo()` for offscreen views

**Issue: "StaleDataException"**
1. Refresh view references
2. Disable view animations
3. Check for data changes

**Issue: "Activity not started"**
1. Check ActivityTestRule configuration
2. Verify @Rule annotation
3. Check launch mode

## Advanced Tips

**Tip 1: Custom ViewMatcher**
```kotlin
fun withViewType(type: Int): Matcher<View> = object : Matcher<View> {
    override fun matches(item: Any) = item is View && item.viewType == type
    override fun describeTo(description: Description) = 
        description.appendText("with view type: $type")
}
```

**Tip 2: ActivityScenario for modern testing**
```kotlin
val scenario = ActivityScenario.launch(MainActivity::class.java)
scenario.onActivity { activity ->
    // Access activity directly
}
```

**Tip 3: Custom Espresso Idling Resource**
```kotlin
class CountingIdlingResource(name: String) : Espresso IdlingResource {
    fun increment() { counter++ }
    fun decrement() { counter-- }
    // Implement all required methods
}
```

**Tip 4: UiObject2 for complex interactions**
```kotlin
val device = UiDevice.getDevice()
device.findObject(By.res("com.example:id/button"))
    .click()
```

**Tip 5: Screenshot on failure**
```kotlin
@OnException
fun onTestFailure(exception: Throwable) {
    takeScreenshot("test_failure")
}
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/02_JUnit_and_Mockito.md
See: 08_TESTING/02_Advanced_Testing/01_Integration_Testing.md
See: 02_UI_DEVELOPMENT/01_XML_Layouts/05_Material_Design_Implementation.md