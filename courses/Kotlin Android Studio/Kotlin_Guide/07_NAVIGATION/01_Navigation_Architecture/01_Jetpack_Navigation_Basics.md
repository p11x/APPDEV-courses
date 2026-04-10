# Jetpack Navigation Basics

## Learning Objectives

1. Understanding the Jetpack Navigation component architecture
2. Setting up Navigation with NavHost and NavController
3. Creating navigation graphs with destinations and actions
4. Implementing basic screen transitions
5. Managing back stack and navigation state
6. Using Safe Args for type-safe navigation
7. Understanding the navigation lifecycle

## Section 1: Navigation Architecture Overview

The Jetpack Navigation component provides a unified navigation framework for Android apps. It simplifies navigation between destinations while following predictable navigation patterns.

```kotlin
/**
 * Jetpack Navigation Architecture
 * 
 * The Navigation component consists of:
 * - NavHost: Container for navigation destinations
 * - NavController: Manages navigation stack and transitions
 * - Navigation Graph: XML file defining destinations and actions
 * - Safe Args: Type-safe navigation arguments
 */
object NavigationArchitecture {
    
    // Core components
    const val NAV_VERSION = "2.7.6"
    
    // Setup dependencies
    fun getDependencies(): String {
        return """
dependencies {
    // Core Navigation
    implementation "androidx.navigation:navigation-fragment-ktx:$NAV_VERSION"
    implementation "androidx.navigation:navigation-ui-ktx:$NAV_VERSION"
    
    // Safe Args
    implementation "androidx.navigation:navigation-safe-args-generator:$NAV_VERSION"
}
        """.trimIndent()
    }
    
    // Basic navigation graph structure
    fun getNavigationGraphXml(): String {
        return """
<navigation xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/nav_graph"
    app:startDestination="@id/homeFragment">
    
    <!-- Home Destination -->
    <fragment
        android:id="@+id/homeFragment"
        android:name="com.example.HomeFragment"
        android:label="Home">
        
        <action
            android:id="@+id/action_home_to_detail"
            app:destination="@id/detailFragment"
            app:enterAnim="@anim/slide_in_right"
            app:exitAnim="@anim/slide_out_left"
            app:popEnterAnim="@anim/slide_in_left"
            app:popExitAnim="@anim/slide_out_right" />
    </fragment>
    
    <!-- Detail Destination -->
    <fragment
        android:id="@+id/detailFragment"
        android:name="com.example.DetailFragment"
        android:label="Detail">
        
        <argument
            android:name="itemId"
            app:argType="string"
            android:defaultValue="" />
        
        <action
            android:id="@+id/action_detail_to_settings"
            app:destination="@id/settingsFragment" />
    </fragment>
    
    <!-- Settings Destination -->
    <fragment
        android:id="@+id/settingsFragment"
        android:name="com.example.SettingsFragment"
        android:label="Settings" />
</navigation>
        """.trimIndent()
    }
}
```

## Section 2: NavController Implementation

The NavController manages the navigation stack and coordinates transitions between destinations. It provides methods for navigating and manages the back stack.

```kotlin
/**
 * NavController Implementation
 * 
 * NavController is the central API for navigation.
 * It manages the back stack and coordinates fragment transactions.
 */
class NavControllerImplementation {
    
    // Creating NavController in Fragment
    class HomeFragment : androidx.fragment.app.Fragment() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Get NavController from NavHostFragment
            // This is the recommended way to obtain NavController
            // It ensures proper integration with the navigation graph
            val navHostFragment = childFragmentManager
                .findFragmentById(R.id.nav_host_fragment) as androidx.navigation.fragment.NavHostFragment
            navController = navHostFragment.navController
            
            // Alternative: Get from view by ID
            // Use when you have a NavHostFragment in your layout
            // navController = requireView().findNavController()
            
            // Set up toolbar for navigation
            val toolbar = view.findViewById<androidx.appcompat.widget.Toolbar>(R.id.toolbar)
            requireActivity().setActionBar(toolbar)
            navController.setupActionBarWithNavController(toolbar)
            
            // Observe navigation events
            navController.addOnDestinationChangedListener { _, destination, arguments ->
                // Called when destination changes
                // Update UI based on destination
                val label = destination.label
                val args = arguments
                println("Navigation to: ${destination.displayName}")
            }
        }
        
        // Navigate using action ID
        fun navigateToDetail(itemId: String) {
            // Create bundle with arguments
            val bundle = android.os.Bundle().apply {
                putString("itemId", itemId)
            }
            
            // Navigate with action and arguments
            // This adds the destination to back stack
            navController.navigate(R.id.action_home_to_detail, bundle)
            
            // Navigate directly to destination
            // navController.navigate(R.id.detailFragment, bundle)
            
            // Using Safe Args (recommended)
            // val action = HomeFragmentDirections.actionHomeToDetail(itemId)
            // navController.navigate(action)
        }
        
        // Navigate using NavDirections
        fun navigateWithDirections() {
            // Create NavDirections object
            val directions = object : androidx.navigation.NavDirections {
                override val actionId: Int = R.id.action_home_to_detail
                override val arguments: android.os.Bundle 
                    get() = android.os.Bundle().apply {
                        putString("itemId", "123")
                    }
            }
            navController.navigate(directions)
        }
    }
    
    // Handling navigation in Activity
    class MainActivity : androidx.appcompat.app.AppCompatActivity() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            val navHostFragment = supportFragmentManager
                .findFragmentById(R.id.nav_host_fragment) as androidx.navigation.fragment.NavHostFragment
            navController = navHostFragment.navController
            
            // Setup action bar
            val toolbar = findViewById<androidx.appcompat.widget.Toolbar>(R.id.toolbar)
            setSupportActionBar(toolbar)
            navController.setupActionBarWithNavController(supportActionBar!!)
            
            // Setup bottom navigation
            val bottomNav = findViewById<com.google.android.material.bottomnavigation.BottomNavigationView>(R.id.bottom_nav)
            navController.setupWithNavController(bottomNav)
        }
        
        // Handle back button press
        override fun onSupportNavigateUp(): Boolean {
            // Navigate up in navigation hierarchy
            return navController.navigateUp() || super.onSupportNavigateUp()
        }
    }
}
```

## Section 3: Navigation Graph Destinations

Destinations are the screens in your app. They can be fragments, activities, or dialogs. Each destination has an ID and can have arguments and actions.

```kotlin
/**
 * Navigation Graph Destinations
 * 
 * Destinations represent the screens in your app.
 * Each destination can have arguments, actions, and deep links.
 */
class NavigationDestinations {
    
    // Fragment destination with arguments
    class DetailFragment : androidx.fragment.app.Fragment() {
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Get arguments passed to this destination
            // Arguments are available in the fragment's arguments bundle
            val itemId = arguments?.getString("itemId") ?: ""
            
            // Display the item
            val textView = view.findViewById<android.widget.TextView>(R.id.text_view)
            textView.text = "Item ID: ${itemId}"
            
            // Update toolbar title
            (requireActivity() as? androidx.appcompat.app.AppCompatActivity)
                ?.supportActionBar?.title = "Detail: ${itemId}"
        }
        
        // Navigate to next screen
        fun navigateToSettings() {
            // Get NavController
            val navController = androidx.navigation.fragment.NavHostFragment
                .findFragmentById(R.id.nav_host_fragment)
                .navController
            
            // Navigate using action
            navController.navigate(R.id.action_detail_to_settings)
            
            // Navigate with arguments
            // val action = DetailFragmentDirections.actionDetailToSettings()
            // navController.navigate(action)
        }
    }
    
    // Destination with complex arguments
    class UserProfileFragment : androidx.fragment.app.Fragment() {
        
        // Arguments data class for type safety
        data class ProfileArgs(
            val userId: String,
            val username: String,
            val isEditable: Boolean = false
        )
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Parse arguments using Safe Args
            // val args = ProfileFragmentArgs.fromBundle(arguments!!)
            // val userId = args.userId
            
            // Manual parsing (without Safe Args)
            val userId = arguments?.getString("userId") ?: ""
            val username = arguments?.getString("username") ?: ""
            val isEditable = arguments?.getBoolean("isEditable", false) ?: false
            
            // Display user info
            println("User: ${username} (${userId}), Editable: ${isEditable}")
        }
    }
    
    // Activity destination
    class SettingsActivity : androidx.appcompat.app.AppCompatActivity() {
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            // Navigation in activities
            // Activity destinations are simpler and don't use NavController
            val intent = intent
            val category = intent.getStringExtra("category")
            println("Settings Category: ${category}")
        }
        
        override fun onSupportNavigateUp(): Boolean {
            // Navigate back to previous destination
            val navUpIntent = navController?.navigateUp() ?: false
            if (!navUpIntent) {
                finish()
            }
            return navUpIntent
        }
    }
}
```

## Section 4: Navigation Actions and Back Stack

Actions define how to navigate between destinations. They can include animations and control the back stack behavior.

```kotlin
/**
 * Navigation Actions and Back Stack
 * 
 * Actions define transitions between destinations.
 * They control back stack behavior with popUpTo and popUpToInclusive.
 */
class NavigationActions {
    
    // Navigate with action
    fun navigateWithAction(navController: androidx.navigation.NavController) {
        // Simple navigation with default animation
        // Adds destination to back stack
        navController.navigate(R.id.action_home_to_detail)
        
        // Navigate with bundle
        val bundle = android.os.Bundle().apply {
            putString("itemId", "123")
        }
        navController.navigate(R.id.action_home_to_detail, bundle)
    }
    
    // Navigate with pop up to (back stack control)
    fun navigateWithPopUp(navController: androidx.navigation.NavController) {
        // Pop up to home and remove all destinations above
        // This clears the back stack back to home
        navController.navigate(
            R.id.action_home_to_detail,
            null,
            androidx.navigation.NavOptions.Builder()
                .setPopUpTo(R.id.homeFragment, false) // false = not inclusive
                .build()
        )
        
        // Pop up to home and remove it too (clear entire stack)
        navController.navigate(
            R.id.action_home_to_detail,
            null,
            androidx.navigation.NavOptions.Builder()
                .setPopUpTo(R.id.homeFragment, true) // true = inclusive
                .build()
        )
        
        // Pop up to save state (restore when navigating back)
        navController.navigate(
            R.id.action_home_to_detail,
            null,
            androidx.navigation.NavOptions.Builder()
                .setPopUpTo(R.id.homeFragment, false)
                .setPopUpToSaveState(true) // Save state for restoration
                .build()
        )
    }
    
    // Single top launch mode
    fun navigateSingleTop(navController: androidx.navigation.NavController) {
        // Navigate only if not already on destination
        // Similar to Activity singleTop
        navController.navigate(
            R.id.action_home_to_detail,
            null,
            androidx.navigation.NavOptions.Builder()
                .setLaunchSingleTop(true)
                .build()
        )
        
        // Single top with pop up to
        navController.navigate(
            R.id.action_home_to_detail,
            null,
            androidx.navigation.NavOptions.Builder()
                .setLaunchSingleTop(true)
                .setPopUpTo(R.id.homeFragment, false)
                .build()
        )
    }
    
    // Custom animations
    fun navigateWithAnimations(navController: androidx.navigation.NavController) {
        // Custom navigation animations
        navController.navigate(
            R.id.action_home_to_detail,
            null,
            androidx.navigation.NavOptions.Builder()
                .setEnterAnim(R.anim.slide_in_right)
                .setExitAnim(R.anim.slide_out_left)
                .setPopEnterAnim(R.anim.slide_in_left)
                .setPopExitAnim(R.anim.slide_out_right)
                .build()
        )
    }
    
    // Pop back stack
    fun popBackStack(navController: androidx.navigation.NavController) {
        // Simple pop back
        navController.popBackStack()
        
        // Pop to specific destination
        // Returns true if pop was successful
        val popped = navController.popBackStack(R.id.homeFragment, false)
        println("Popped: ${popped}")
        
        // Pop with inclusive
        navController.popBackStack(R.id.homeFragment, true)
    }
    
    // Clear back stack
    fun clearBackStack(navController: androidx.navigation.NavController) {
        // Pop all destinations except current
        while (navController.popBackStack()) {
            // Continue popping
        }
        
        // Or use NavOptions to clear
        navController.navigate(
            R.id.homeFragment,
            null,
            androidx.navigation.NavOptions.Builder()
                .setPopUpTo(R.id.homeFragment, true)
                .build()
        )
    }
}
```

## Example: Complete Navigation Setup

Complete example showing all components working together.

```kotlin
/**
 * Complete Navigation Setup
 * 
 * Full implementation of Jetpack Navigation with fragments.
 */
class CompleteNavigationSetup {
    
    // 1. Main Activity
    class MainActivity : androidx.appcompat.app.AppCompatActivity() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(R.layout.activity_main)
            
            // Get NavController
            val navHostFragment = supportFragmentManager
                .findFragmentById(R.id.nav_host_fragment) as androidx.navigation.fragment.NavHostFragment
            navController = navHostFragment.navController
            
            // Setup ActionBar
            val toolbar = findViewById<androidx.appcompat.widget.Toolbar>(R.id.toolbar)
            setSupportActionBar(toolbar)
            navController.setupActionBarWithNavController(supportActionBar!!)
            
            // Optional: Handle deep links on launch
            // onNewIntent(intent)?.let { handleDeepLink(it) }
        }
        
        override fun onSupportNavigateUp(): Boolean {
            return navController.navigateUp() || super.onSupportNavigateUp()
        }
    }
    
    // 2. Home Fragment
    class HomeFragment : androidx.fragment.app.Fragment() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            navController = androidx.navigation.fragment.NavHostFragment
                .findFragmentById(R.id.nav_host_fragment)
                .navController
            
            // Click listener for navigation
            view.findViewById<android.view.View>(R.id.button_detail).setOnClickListener {
                // Navigate using Safe Args action
                // val action = HomeFragmentDirections.actionHomeToDetail("123")
                // navController.navigate(action)
                
                // Or using bundle
                val bundle = android.os.Bundle().apply {
                    putString("itemId", "123")
                }
                navController.navigate(R.id.action_home_to_detail, bundle)
            }
            
            // Navigate to profile
            view.findViewById<android.view.View>(R.id.button_profile).setOnClickListener {
                navController.navigate(R.id.action_home_to_profile)
            }
        }
    }
    
    // 3. Detail Fragment
    class DetailFragment : androidx.fragment.app.Fragment() {
        
        private var itemId: String = ""
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Get arguments
            itemId = arguments?.getString("itemId") ?: ""
            
            // Display item ID
            view.findViewById<android.widget.TextView>(R.id.text_item_id).text = itemId
            
            // Get NavController
            val navController = androidx.navigation.fragment.NavHostFragment
                .findFragmentById(R.id.nav_host_fragment)
                .navController
            
            // Navigate to Settings
            view.findViewById<android.view.View>(R.id.button_settings).setOnClickListener {
                navController.navigate(R.id.action_detail_to_settings)
            }
        }
    }
    
    // 4. Profile Fragment
    class ProfileFragment : androidx.fragment.app.Fragment() {
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            val navController = androidx.navigation.fragment.NavHostFragment
                .findFragmentById(R.id.nav_host_fragment)
                .navController
            
            view.findViewById<android.view.View>(R.id.button_back).setOnClickListener {
                navController.popBackStack()
            }
        }
    }
    
    // 5. Settings Fragment
    class SettingsFragment : androidx.fragment.app.Fragment() {
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            val navController = androidx.navigation.fragment.NavHostFragment
                .findFragmentById(R.id.nav_host_fragment)
                .navController
            
            view.findViewById<android.view.View>(R.id.button_back_home).setOnClickListener {
                // Pop all way back to start
                navController.popBackStack(R.id.homeFragment, false)
            }
        }
    }
}
```

## Output Statement Results

**Navigation Components:**
- NavHostFragment: Container for navigation destinations
- NavController: Manages navigation stack and transitions
- Navigation Graph: XML defining destinations and actions
- NavOptions: Controls navigation behavior

**Navigation Destinations:**
- Fragment: Most common destination type
- Activity: For external navigation
- Dialog: For dialog destinations

**Navigation Actions:**
- navigate(): Navigate to destination with optional bundle
- popBackStack(): Pop current destination from stack
- navigateUp(): Navigate up in hierarchy
- setPopUpTo(): Control back stack behavior
- setLaunchSingleTop(): Prevent duplicate destinations

**Back Stack Patterns:**
- Simple push/pop: Default stack behavior
- popUpTo: Clear to specific destination
- popUpToInclusive: Clear including start destination
- clearBackStack: Clear entire stack
- Single top: Prevent duplicates

**Safe Args Benefits:**
- Type-safe argument passing
- Generated direction classes
- Compile-time verification
- Cleaner navigation code

## Cross-References

- See: [02_Navigation_Compose.md](./02_Navigation_Compose.md)
- See: [03_Deep_Linking.md](./03_Deep_Linking.md)
- See: [04_Navigation_Arguments.md](./04_Navigation_Arguments.md)
- See: [../02_UI_DEVELOPMENT/02_Jetpack_Compose/04_Navigation_Compose.md](../../02_UI_DEVELOPMENT/02_Jetpack_Compose/04_Navigation_Compose.md)

## End of Jetpack Navigation Basics Guide