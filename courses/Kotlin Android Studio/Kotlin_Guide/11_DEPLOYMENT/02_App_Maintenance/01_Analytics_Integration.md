# Analytics Integration

## Learning Objectives

1. Understanding mobile analytics fundamentals
2. Implementing Firebase Analytics
3. Creating custom events and parameters
4. Building analytics dashboards
5. Implementing user segmentation
6. Setting up conversion tracking

## Prerequisites

- [Google Play Store](../01_App_Distribution/01_Google_Play_Store.md)
- [Firebase App Distribution](../01_App_Distribution/02_Firebase_App_Distribution.md)

## Section 1: Mobile Analytics Fundamentals

Mobile analytics provides crucial insights into how users interact with your app. Understanding user behavior helps you make informed decisions about feature development, user experience improvements, and marketing strategies. Analytics integration should be considered early in development to ensure you capture meaningful data from the start.

The key metrics to track include user acquisition (how users find your app), user engagement (how users interact with your app), retention (how often users return), monetization (how your app generates revenue), and user demographics (who your users are). Each of these categories provides different insights and requires different event tracking implementations.

Modern analytics platforms like Firebase Analytics provide comprehensive solutions that integrate with other Firebase services. The platform handles data collection automatically for common events while allowing custom event creation for app-specific interactions. Understanding the data model of your analytics platform helps you design effective tracking strategies.

```kotlin
// Analytics event data models
package com.example.myapp.analytics

import android.os.Bundle

data class AnalyticsEvent(
    val name: String,
    val parameters: Map<String, Any>,
    val userProperty: UserProperty? = null,
    val timestamp: Long = System.currentTimeMillis()
)

data class UserProperty(
    val name: String,
    val value: String
)

// Event builder for type-safe event creation
class EventBuilder(private val eventName: String) {
    
    private val parameters = mutableMapOf<String, Any>()
    private var userProperty: UserProperty? = null
    
    fun param(key: String, value: String): EventBuilder {
        parameters[key] = value
        return this
    }
    
    fun param(key: String, value: Int): EventBuilder {
        parameters[key] = value
        return this
    }
    
    fun param(key: String, value: Long): EventBuilder {
        parameters[key] = value
        return this
    }
    
    fun param(key: String, value: Double): EventBuilder {
        parameters[key] = value
        return this
    }
    
    fun param(key: String, value: Boolean): EventBuilder {
        parameters[key] = value
        return this
    }
    
    fun userProperty(name: String, value: String): EventBuilder {
        userProperty = UserProperty(name, value)
        return this
    }
    
    fun build(): AnalyticsEvent {
        return AnalyticsEvent(
            name = eventName,
            parameters = parameters.toMap(),
            userProperty = userProperty
        )
    }
}

// Event names - following Firebase conventions
object EventNames {
    // App-specific events
    const val SCREEN_VIEW = "screen_view"
    const val SIGN_UP = "sign_up"
    const val LOGIN = "login"
    const val LOGOUT = "logout"
    const val SEARCH = "search"
    const val VIEW_ITEM = "view_item"
    const val ADD_TO_CART = "add_to_cart"
    const val BEGIN_CHECKOUT = "begin_checkout"
    const val PURCHASE = "purchase"
    const val SHARE = "share"
    const val LEVEL_UP = "level_up"
    const val TUTORIAL_COMPLETE = "tutorial_complete"
    
    // Custom events for this app
    const val ONBOARDING_COMPLETE = "onboarding_complete"
    const val PROFILE_UPDATED = "profile_updated"
    const val SETTINGS_CHANGED = "settings_changed"
    const val FEATURE_USED = "feature_used"
    const val CONTENT_RATED = "content_rated"
    const val SUBSCRIPTION_STARTED = "subscription_started"
    const val SUBSCRIPTION_RENEWED = "subscription_renewed"
}

// Parameter keys - consistent naming across events
object ParamKeys {
    const val SCREEN_NAME = "screen_name"
    const val SCREEN_CLASS = "screen_class"
    const val ITEM_ID = "item_id"
    const val ITEM_NAME = "item_name"
    const val ITEM_CATEGORY = "item_category"
    const val ITEM_VARIANT = "item_variant"
    const val PRICE = "price"
    const val CURRENCY = "currency"
    const val QUANTITY = "quantity"
    const val VALUE = "value"
    const val TRANSACTION_ID = "transaction_id"
    const val METHOD = "method"
    const val TERM = "term"
    const val CONTENT_TYPE = "content_type"
    const val SUCCESS = "success"
    const val ERROR_MESSAGE = "error_message"
    const val USER_TYPE = "user_type"
    const val LEVEL = "level"
}
```

## Section 2: Firebase Analytics Implementation

Firebase Analytics provides free, unlimited analytics for Android apps. It integrates with other Firebase services and provides detailed insights through the Firebase console. Implementation involves adding the SDK, configuring events, and understanding how to interpret the data.

```kotlin
// Firebase Analytics wrapper for consistent event tracking
package com.example.myapp.analytics

import android.content.Context
import android.os.Bundle
import com.google.firebase.analytics.FirebaseAnalytics
import com.google.firebase.analytics.logEvent

class FirebaseAnalyticsManager(context: Context) {
    
    private val analytics = FirebaseAnalytics.getInstance(context)
    
    // Log screen views automatically
    fun logScreenView(screenName: String, screenClass: String) {
        val params = Bundle().apply {
            putString(FirebaseAnalytics.Param.SCREEN_NAME, screenName)
            putString(FirebaseAnalytics.Param.SCREEN_CLASS, screenClass)
        }
        analytics.logEvent(FirebaseAnalytics.Event.SCREEN_VIEW, params)
    }
    
    // Log custom events
    fun logEvent(event: AnalyticsEvent) {
        val bundle = Bundle()
        event.parameters.forEach { (key, value) ->
            when (value) {
                is String -> bundle.putString(key, value)
                is Int -> bundle.putInt(key, value)
                is Long -> bundle.putLong(key, value)
                is Double -> bundle.putDouble(key, value)
                is Boolean -> bundle.putBoolean(key, value)
            }
        }
        analytics.logEvent(event.name, bundle)
    }
    
    // Convenience methods for common events
    fun logUserSignup(method: String) {
        val params = Bundle().apply {
            putString(ParamKeys.METHOD, method)
        }
        analytics.logEvent(EventNames.SIGN_UP, params)
    }
    
    fun logLogin(method: String, success: Boolean) {
        val params = Bundle().apply {
            putString(ParamKeys.METHOD, method)
            putBoolean(ParamKeys.SUCCESS, success)
        }
        analytics.logEvent(EventNames.LOGIN, params)
    }
    
    fun logItemView(itemId: String, itemName: String, category: String, price: Double) {
        val params = Bundle().apply {
            putString(ParamKeys.ITEM_ID, itemId)
            putString(ParamKeys.ITEM_NAME, itemName)
            putString(ParamKeys.ITEM_CATEGORY, category)
            putDouble(ParamKeys.PRICE, price)
        }
        analytics.logEvent(EventNames.VIEW_ITEM, params)
    }
    
    fun logAddToCart(itemId: String, itemName: String, price: Double, currency: String, quantity: Int) {
        val params = Bundle().apply {
            putString(ParamKeys.ITEM_ID, itemId)
            putString(ParamKeys.ITEM_NAME, itemName)
            putDouble(ParamKeys.PRICE, price)
            putString(ParamKeys.CURRENCY, currency)
            putInt(ParamKeys.QUANTITY, quantity)
            putDouble(ParamKeys.VALUE, price * quantity)
        }
        analytics.logEvent(EventNames.ADD_TO_CART, params)
    }
    
    fun logPurchase(transactionId: String, value: Double, currency: String, items: List<CartItem>) {
        val params = Bundle().apply {
            putString(ParamKeys.TRANSACTION_ID, transactionId)
            putDouble(ParamKeys.VALUE, value)
            putString(ParamKeys.CURRENCY, currency)
            putString(ParamKeys.ITEMS, items.joinToString(",") { it.id })
        }
        analytics.logEvent(EventNames.PURCHASE, params)
    }
    
    fun logOnboardingComplete(userType: String) {
        val params = Bundle().apply {
            putString(ParamKeys.USER_TYPE, userType)
        }
        analytics.logEvent(EventNames.ONBOARDING_COMPLETE, params)
    }
    
    fun logFeatureUsed(featureName: String, interactionType: String) {
        val params = Bundle().apply {
            putString(ParamKeys.FEATURE_NAME, featureName)
            putString(ParamKeys.INTERACTION_TYPE, interactionType)
        }
        analytics.logEvent(EventNames.FEATURE_USED, params)
    }
    
    // Set user properties
    fun setUserProperty(name: String, value: String) {
        analytics.setUserProperty(name, value)
    }
    
    fun setUserId(userId: String) {
        analytics.setUserId(userId)
    }
    
    // User segmentation properties
    fun setUserProperties(
        userType: String,
        subscriptionStatus: String,
        preferredLanguage: String,
        region: String
    ) {
        analytics.setUserProperty("user_type", userType)
        analytics.setUserProperty("subscription_status", subscriptionStatus)
        analytics.setUserProperty("preferred_language", preferredLanguage)
        analytics.setUserProperty("region", region)
    }
    
    // Reset analytics for logout
    fun resetAnalyticsData() {
        analytics.setUserId(null)
    }
}

// Data classes
data class CartItem(
    val id: String,
    val name: String,
    val price: Double,
    val quantity: Int
)

private object ParamKeys {
    const val FEATURE_NAME = "feature_name"
    const val INTERACTION_TYPE = "interaction_type"
    const val ITEMS = "items"
}

private object EventNames {
    const val FEATURE_USED = "feature_used"
    const val ONBOARDING_COMPLETE = "onboarding_complete"
}
```

## Section 3: Screen View Tracking

Understanding which screens users visit and how they navigate through your app is fundamental to understanding user behavior. Proper screen view tracking helps you identify popular features and potential navigation issues.

```kotlin
// Screen tracking integration with Compose
package com.example.myapp.analytics

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.platform.LocalContext
import androidx.navigation.NavDestination
import androidx.navigation.NavDestination.Companion.hierarchy
import androidx.navigation.NavGraph
import androidx.navigation.NavHostController

@Composable
fun AnalyticsScreenTracker(
    navController: NavHostController,
    currentRoute: String?
) {
    val context = LocalContext.current
    val analyticsManager = FirebaseAnalyticsManager(context)
    
    LaunchedEffect(currentRoute) {
        currentRoute?.let { route ->
            val screenName = getScreenNameFromRoute(route)
            val screenClass = getScreenClassFromRoute(route)
            analyticsManager.logScreenView(screenName, screenClass)
        }
    }
}

private fun getScreenNameFromRoute(route: String): String {
    return when {
        route.startsWith("home") -> "HomeScreen"
        route.startsWith("profile") -> "ProfileScreen"
        route.startsWith("settings") -> "SettingsScreen"
        route.startsWith("detail") -> "DetailScreen"
        route.startsWith("checkout") -> "CheckoutScreen"
        else -> "Unknown"
    }
}

private fun getScreenClassFromRoute(route: String): String {
    return when {
        route.startsWith("home") -> "HomeFragment"
        route.startsWith("profile") -> "ProfileFragment"
        route.startsWith("settings") -> "SettingsFragment"
        route.startsWith("detail") -> "DetailFragment"
        route.startsWith("checkout") -> "CheckoutFragment"
        else -> "UnknownFragment"
    }
}

// Traditional Fragment tracking
abstract class AnalyticsFragment : androidx.fragment.app.Fragment() {
    
    protected val analyticsManager by lazy {
        FirebaseAnalyticsManager(requireContext())
    }
    
    override fun onResume() {
        super.onResume()
        logScreenView()
    }
    
    abstract fun getScreenName(): String
    
    open fun getScreenClass(): String = this::class.java.simpleName
    
    private fun logScreenView() {
        analyticsManager.logScreenView(getScreenName(), getScreenClass())
    }
}
```

## Section 4: Advanced Event Tracking

Beyond basic events, you need to track complex user interactions and business metrics. This includes e-commerce events, gaming metrics, and custom events specific to your app's functionality.

```kotlin
// Advanced event tracking for e-commerce
package com.example.myapp.analytics.ecommerce

import android.content.Context
import com.google.firebase.analytics.FirebaseAnalytics

class ECommerceAnalytics(context: Context) {
    
    private val analytics = FirebaseAnalytics.getInstance(context)
    
    // Track product views with impression data
    fun logProductView(
        product: Product,
        category: String,
        listSource: String
    ) {
        val itemBundle = createItemBundle(product, category, listSource)
        analytics.logEvent(FirebaseAnalytics.Event.VIEW_ITEM, itemBundle)
    }
    
    // Track product selection from a list
    fun logProductSelect(
        product: Product,
        listSource: String,
        position: Int
    ) {
        val itemBundle = createItemBundle(product, "", listSource)
        itemBundle.putLong(FirebaseAnalytics.Param.INDEX, position.toLong())
        analytics.logEvent(FirebaseAnalytics.Event.SELECT_ITEM, itemBundle)
    }
    
    // Track add to cart with custom attributes
    fun logAddToCart(
        product: Product,
        quantity: Int,
        currency: String
    ) {
        val params = Bundle().apply {
            putString(FirebaseAnalytics.Param.CURRENCY, currency)
            putDouble(FirebaseAnalytics.Param.VALUE, product.price * quantity)
            
            putString(FirebaseAnalytics.Param.ITEM_ID, product.id)
            putString(FirebaseAnalytics.Param.ITEM_NAME, product.name)
            putString(FirebaseAnalytics.Param.ITEM_CATEGORY, product.category)
            putString(FirebaseAnalytics.Param.ITEM_VARIANT, product.variant)
            putDouble(FirebaseAnalytics.Param.PRICE, product.price)
            putLong(FirebaseAnalytics.Param.QUANTITY, quantity.toLong())
        }
        
        analytics.logEvent(FirebaseAnalytics.Event.ADD_TO_CART, params)
    }
    
    // Track checkout progress
    fun logCheckoutProgress(
        step: Int,
        option: String,
        cartValue: Double,
        currency: String
    ) {
        val params = Bundle().apply {
            putLong(FirebaseAnalytics.Param.CHECKOUT_STEP, step.toLong())
            putString(FirebaseAnalytics.Param.CHECKOUT_OPTION, option)
            putDouble(FirebaseAnalytics.Param.VALUE, cartValue)
            putString(FirebaseAnalytics.Param.CURRENCY, currency)
        }
        
        analytics.logEvent(FirebaseAnalytics.Event.BEGIN_CHECKOUT, params)
    }
    
    // Track successful purchase
    fun logPurchase(
        transactionId: String,
        revenue: Double,
        tax: Double,
        shipping: Double,
        currency: String,
        items: List<Product>,
        coupon: String?
    ) {
        val params = Bundle().apply {
            putString(FirebaseAnalytics.Param.TRANSACTION_ID, transactionId)
            putDouble(FirebaseAnalytics.Param.VALUE, revenue)
            putDouble("tax", tax)
            putDouble("shipping", shipping)
            putString(FirebaseAnalytics.Param.CURRENCY, currency)
            coupon?.let { putString("coupon", it) }
            
            // Add items as a bundle array
            val itemList = items.map { createItemBundle(it, "", "") }
            putString("items", itemList.joinToString { "test" })
        }
        
        analytics.logEvent(FirebaseAnalytics.Event.PURCHASE, params)
    }
    
    // Track refund
    fun logRefund(
        transactionId: String,
        amount: Double,
        currency: String
    ) {
        val params = Bundle().apply {
            putString(FirebaseAnalytics.Param.TRANSACTION_ID, transactionId)
            putDouble(FirebaseAnalytics.Param.VALUE, amount)
            putString(FirebaseAnalytics.Param.CURRENCY, currency)
        }
        
        analytics.logEvent(FirebaseAnalytics.Event.REFUND, params)
    }
    
    // Track promotion views and clicks
    fun logPromotionView(promotion: Promotion) {
        val params = Bundle().apply {
            putString(FirebaseAnalytics.Param.CREATIVE_NAME, promotion.name)
            putString(FirebaseAnalytics.Param.CREATIVE_SLOT, promotion.slot)
            putString(FirebaseAnalytics.Param.PROMOTION_ID, promotion.id)
            putString(FirebaseAnalytics.Param.PROMOTION_NAME, promotion.name)
        }
        
        analytics.logEvent(FirebaseAnalytics.Event.VIEW_PROMOTION, params)
    }
    
    fun logPromotionClick(promotion: Promotion) {
        val params = Bundle().apply {
            putString(FirebaseAnalytics.Param.CREATIVE_NAME, promotion.name)
            putString(FirebaseAnalytics.Param.CREATIVE_SLOT, promotion.slot)
            putString(FirebaseAnalytics.Param.PROMOTION_ID, promotion.id)
            putString(FirebaseAnalytics.Param.PROMOTION_NAME, promotion.name)
        }
        
        analytics.logEvent(FirebaseAnalytics.Event.SELECT_PROMOTION, params)
    }
    
    private fun createItemBundle(
        product: Product,
        category: String,
        listSource: String
    ): android.os.Bundle {
        return android.os.Bundle().apply {
            putString(FirebaseAnalytics.Param.ITEM_ID, product.id)
            putString(FirebaseAnalytics.Param.ITEM_NAME, product.name)
            putString(FirebaseAnalytics.Param.ITEM_CATEGORY, category)
            putString(FirebaseAnalytics.Param.ITEM_VARIANT, product.variant)
            putString(FirebaseAnalytics.Param.ITEM_BRAND, product.brand)
            putDouble(FirebaseAnalytics.Param.PRICE, product.price)
            putString(FirebaseAnalytics.Param.CURRENCY, product.currency)
            if (listSource.isNotEmpty()) {
                putString(FirebaseAnalytics.Param.ITEM_LIST_NAME, listSource)
            }
        }
    }
}

// Data models
data class Product(
    val id: String,
    val name: String,
    val category: String,
    val variant: String = "",
    val brand: String = "",
    val price: Double,
    val currency: String = "USD"
)

data class Promotion(
    val id: String,
    val name: String,
    val slot: String
)

// Bundle extension for items
private fun android.os.Bundle.putString(key: String, value: String?) {
    value?.let { putString(key, it) }
}
```

## Section 5: Analytics Debugging and Validation

Before trusting your analytics data, you need to ensure events are being collected correctly. Firebase provides debug mode and validation tools to verify your implementation.

```kotlin
// Analytics debugging utilities
package com.example.myapp.analytics.debug

import android.content.Context
import android.util.Log
import com.google.firebase.analytics.FirebaseAnalytics

class AnalyticsDebugger(context: Context) {
    
    private val analytics = FirebaseAnalytics.getInstance(context)
    
    // Enable debug mode - events logged to logcat
    fun enableDebugMode() {
        // This is handled by passing -ea to the app or through adb
        // adb shell setprop debug.firebase.analytics.app <package_name>
    }
    
    // Validate event parameters
    fun validateEvent(
        eventName: String,
        params: Map<String, Any>
    ): ValidationResult {
        val warnings = mutableListOf<String>()
        
        // Check for reserved parameter names
        val reservedParams = listOf(
            "app_id", "app_version", "app_installer_id",
            "device_model", "os_version", "country",
            "language", "timezone", "random_user_id"
        )
        
        params.keys.forEach { key ->
            if (key in reservedParams) {
                warnings.add("Parameter '$key' may conflict with reserved names")
            }
        }
        
        // Validate parameter length
        params.forEach { (key, value) ->
            if (key.length > 40) {
                warnings.add("Parameter name '$key' exceeds 40 characters")
            }
            if (value is String && value.length > 100) {
                warnings.add("Parameter value for '$key' exceeds 100 characters")
            }
        }
        
        return ValidationResult(
            isValid = warnings.isEmpty(),
            warnings = warnings
        )
    }
    
    // Log event to console for debugging
    fun logEventForDebug(eventName: String, params: Map<String, Any>) {
        Log.d("Analytics", "Event: $eventName")
        params.forEach { (key, value) ->
            Log.d("Analytics", "  $key = $value")
        }
    }
    
    // Check if analytics is enabled
    fun isAnalyticsEnabled(): Boolean {
        return analytics.isAnalyticsCollectionEnabled
    }
    
    // Check data collection status
    fun getDataCollectionStatus(): DataCollectionStatus {
        return DataCollectionStatus(
            analyticsEnabled = analytics.isAnalyticsCollectionEnabled,
            adIdEnabled = true, // Check separately
            appSetIdentifierEnabled = true
        )
    }
}

data class ValidationResult(
    val isValid: Boolean,
    val warnings: List<String>
)

data class DataCollectionStatus(
    val analyticsEnabled: Boolean,
    val adIdEnabled: Boolean,
    val appSetIdentifierEnabled: Boolean
)
```

## Best Practices

- Set up key events early in development to establish baselines
- Use consistent naming conventions for events and parameters
- Implement user properties to enable meaningful segmentation
- Log screen views automatically using a navigation interceptor
- Limit custom events to meaningful interactions; avoid noise
- Test events in debug mode before production release
- Set up conversion events for important user actions
- Document your analytics implementation for team reference
- Use Firebase DebugView to verify events during development
- Consider data privacy and consent requirements

## Common Pitfalls

- **Too many events causing noise**
  - Solution: Focus on key user actions and business metrics; don't track everything
  
- **Inconsistent event naming**
  - Solution: Create an EventBuilder class with constants for all events
  
- **Missing screen view tracking**
  - Solution: Implement automatic screen tracking in your navigation
  
- **Not testing analytics in debug builds**
  - Solution: Use Firebase DebugView before releasing
  
- **Forgetting user properties after login/logout**
  - Solution: Update user properties in authentication callbacks

## Troubleshooting Guide

**Q: Events don't appear in Firebase Console**
A: Check debug mode is enabled, wait up to 24 hours for data processing, verify network connectivity.

**Q: User properties not working**
A: User properties take effect for new sessions; check for character limits on values.

**Q: Duplicate events being logged**
  - Solution: Check lifecycle methods and debounce rapid events
  
**Q: Revenue tracking not working**
A: Ensure currency parameter is set; check that VALUE is in the correct numeric format.

## Advanced Tips

- Use BigQuery export for advanced analysis beyond Firebase console
- Implement predictive audiences based on user behavior
- Set up custom audiences for targeted messaging
- Use Firebase Remote Config to control event collection
- Implement server-side analytics for sensitive data

## Cross-References

- [Crash Reporting](./02_Crash_Reporting.md) - Monitor app stability alongside analytics
- [User Feedback Systems](./05_User_Feedback_Systems.md) - Connect feedback to analytics
- [Google Play Store](../01_App_Distribution/01_Google_Play_Store.md) - Track acquisition analytics
- [Update Strategies](./03_Update_Strategies.md) - Analytics for update adoption tracking