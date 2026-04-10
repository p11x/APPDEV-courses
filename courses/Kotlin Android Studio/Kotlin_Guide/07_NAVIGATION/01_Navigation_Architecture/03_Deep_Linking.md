# Deep Linking

## Learning Objectives

1. Understanding deep linking concepts
2. Implementing URI-based deep links
3. Creating deep link configurations
4. Handling deep links in Navigation Compose
5. Supporting App Links (Android App Links)
6. Managing deep link intent filters
7. Testing deep links with ADB
8. Handling back navigation from deep links

## Section 1: Deep Linking Fundamentals

Deep linking allows users to navigate directly to specific content in your app via a URL. This enables external links, web URLs, and custom schemes to open specific screens.

```kotlin
/**
 * Deep Linking Fundamentals
 * 
 * Types of deep links:
 * - URI deep links: Custom scheme (myapp://...)
 * - App Links: HTTPS links (https://myapp.com/...)
 * - Intent links: Android intent-based links
 * 
 * Deep links enable:
 * - External website navigation
 * - Email link handling
 * - Social media sharing
 * - Cross-app navigation
 */
object DeepLinkingFundamentals {
    
    // Deep link types
    object DeepLinkTypes {
        // Custom scheme (non-secure)
        const val CUSTOM_SCHEME = "myapp://products/123"
        
        // App Links (verified HTTPS)
        const val APP_LINK = "https://myapp.com/products/123"
        
        // Alternative domains
        const val ALTERNATIVE_LINK = "https://www.myapp.com/products/123"
        
        // Short links
        const val SHORT_LINK = "myapp://go/product/123"
    }
    
    // Deep link components
    object DeepLinkComponents {
        // URI structure: scheme://host/path
        // Example: myapp://detail/123
        
        // Scheme: Protocol (myapp, https)
        const val SCHEME = "myapp"
        
        // Host: Domain (myapp.com, detail)
        const val HOST = "myapp.com"
        
        // Path: Resource location
        const val PATH = "/products/123"
        
        // Full URI
        const val FULL_URI = "myapp://myapp.com/products/123"
    }
}
```

## Section 2: Navigation Graph Deep Links

In traditional navigation (XML), deep links are defined in the navigation graph using `<deepLink>` elements.

```kotlin
/**
 * Navigation Graph Deep Links
 * 
 * XML-based navigation graph with deep link definitions.
 * Each deep link defines a URI pattern that maps to a destination.
 */
class NavigationGraphDeepLinks {
    
    // XML navigation graph with deep links
    fun getNavigationGraphXml(): String {
        return """
<navigation xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/nav_graph"
    app:startDestination="@id/homeFragment">
    
    <!-- Home with deep links -->
    <fragment
        android:id="@+id/homeFragment"
        android:name="com.example.HomeFragment"
        android:label="Home">
        
        <!-- Deep link for custom scheme -->
        <deepLink
            android:id="@+id/deepLinkHome"
            app:uri="myapp://home" />
        
        <!-- Deep link for HTTPS -->
        <deepLink
            app:uri="https://myapp.com/home" />
            
        <!-- Deep link with path parameters -->
        <deepLink
            app:uri="https://myapp.com/home/{userId}" />
    </fragment>
    
    <!-- Product detail with deep links -->
    <fragment
        android:id="@+id/productFragment"
        android:name="com.example.ProductFragment"
        android:label="Product">
        
        <!-- Product by ID -->
        <deepLink
            app:uri="myapp://product/{productId}" />
        
        <!-- App link for product -->
        <deepLink
            app:uri="https://myapp.com/product/{productId}" />
        
        <!-- Alternative domain -->
        <deepLink
            app:uri="https://www.myapp.com/product/{productId}" />
        
        <!-- Multiple path segments -->
        <deepLink
            app:uri="https://myapp.com/products/{category}/{productId}" />
        
        <!-- Query parameters -->
        <deepLink
            app:uri="myapp://search?query={query}" />
        
        <!-- Define argument -->
        <argument
            android:name="productId"
            app:argType="string"
            android:defaultValue="" />
    </fragment>
    
    <!-- User profile deep link -->
    <fragment
        android:id="@+id/profileFragment"
        android:name="com.example.ProfileFragment"
        android:label="Profile">
        
        <!-- User profile by username -->
        <deepLink
            app:uri="myapp://profile/{username}" />
        
        <!-- App link -->
        <deepLink
            app:uri="https://myapp.com/user/{username}" />
    </fragment>
</navigation>
        """.trimIndent()
    }
    
    // Intent filter configuration
    fun getManifestIntentFilter(): String {
        return """
<activity
    android:name=".MainActivity"
    android:exported="true">
    
    <intent-filter>
        <!-- Action for deep links -->
        <action android:name="android.intent.action.VIEW" />
        
        <!-- Category for main launch -->
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
        
        <!-- Custom scheme deep links -->
        <data
            android:scheme="myapp"
            android:host="home" />
    </intent-filter>
    
    <!-- App Links (HTTPS) -->
    <intent-filter android:autoVerify="true">
        <action android:name="android.intent.action.VIEW" />
        
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
        
        <!-- HTTPS scheme -->
        <data
            android:scheme="https"
            android:host="myapp.com" />
    </intent-filter>
    
    <!-- Alternative domain -->
    <intent-filter android:autoVerify="true">
        <action android:name="android.intent.action.VIEW" />
        
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
        
        <data
            android:scheme="https"
            android:host="www.myapp.com" />
    </intent-filter>
</activity>
        """.trimIndent()
    }
}
```

## Section 3: Navigation Compose Deep Links

Navigation Compose provides the `navDeepLink` function for defining deep links programmatically.

```kotlin
/**
 * Navigation Compose Deep Links
 * 
 * Using navDeepLink in Compose for declarative deep link configuration.
 */
class NavigationComposeDeepLinks {
    
    // Basic deep link setup
    @Composable
    fun BasicDeepLinks() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "home"
        ) {
            // Home with deep links
            androidx.navigation.compose.composable(
                route = "home",
                deepLinks = listOf(
                    // Custom scheme
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://home"
                    },
                    // HTTPS
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://myapp.com/home"
                    }
                )
            ) {
                HomeScreen()
            }
            
            // Product with parameterized deep links
            androidx.navigation.compose.composable(
                route = "product/{productId}",
                arguments = listOf(
                    androidx.navigation.compose.navArgument("productId") {
                        type = androidx.navigation.compose.NavType.StringType
                    }
                ),
                deepLinks = listOf(
                    // Custom scheme with parameter
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://product/{productId}"
                    },
                    // App link with parameter
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://myapp.com/product/{productId}"
                    },
                    // Alternative domain
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://www.myapp.com/product/{productId}"
                    }
                )
            ) { backStackEntry ->
                val productId = backStackEntry.arguments?.getString("productId") ?: ""
                ProductScreen(productId = productId)
            }
        }
    }
    
    // Complex deep links with multiple parameters
    @Composable
    fun ComplexDeepLinks() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "home"
        ) {
            // Search with query parameter
            androidx.navigation.compose.composable(
                route = "search/{query}",
                arguments = listOf(
                    androidx.navigation.compose.navArgument("query") {
                        type = androidx.navigation.compose.NavType.StringType
                        defaultValue = ""
                    }
                ),
                deepLinks = listOf(
                    // Query parameter in deep link
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://search?query={query}"
                    },
                    // Path-based query
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://myapp.com/search/{query}"
                    }
                )
            ) { backStackEntry ->
                val query = backStackEntry.arguments?.getString("query") ?: ""
                SearchScreen(query = query)
            }
            
            // Category product with multiple arguments
            androidx.navigation.compose.composable(
                route = "category/{categoryId}/product/{productId}",
                arguments = listOf(
                    androidx.navigation.compose.navArgument("categoryId") {
                        type = androidx.navigation.compose.NavType.StringType
                    },
                    androidx.navigation.compose.navArgument("productId") {
                        type = androidx.navigation.compose.NavType.StringType
                    }
                ),
                deepLinks = listOf(
                    // Multiple path parameters
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://category/{categoryId}/product/{productId}"
                    },
                    // Multiple domains
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://myapp.com/category/{categoryId}/product/{productId}"
                    },
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://www.myapp.com/category/{categoryId}/product/{productId}"
                    }
                )
            ) { backStackEntry ->
                val categoryId = backStackEntry.arguments?.getString("categoryId") ?: ""
                val productId = backStackEntry.arguments?.getString("productId") ?: ""
                CategoryProductScreen(categoryId, productId)
            }
        }
    }
    
    @Composable
    fun HomeScreen() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Home")
        }
    }
    
    @Composable
    fun ProductScreen(productId: String) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Product: ${productId}")
        }
    }
    
    @Composable
    fun SearchScreen(query: String) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Search: ${query}")
        }
    }
    
    @Composable
    fun CategoryProductScreen(categoryId: String, productId: String) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Category: ${categoryId}, Product: ${productId}")
        }
    }
}
```

## Section 4: Handling Deep Links in Code

Handling deep links in activities and fragments to properly navigate to the correct content.

```kotlin
/**
 * Handling Deep Links in Code
 * 
 * Processing incoming deep links and navigating appropriately.
 */
class DeepLinkHandling {
    
    // Activity handling deep links
    class DeepLinkActivity : androidx.appcompat.app.AppCompatActivity() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            // Setup navigation as usual
            val navHostFragment = supportFragmentManager
                .findFragmentById(R.id.nav_host_fragment) as androidx.navigation.fragment.NavHostFragment
            navController = navHostFragment.navController
            
            // Handle deep link on initial launch
            // onNewIntent is called for singleTop activities
            intent?.let { handleDeepLink(it) }
        }
        
        // Handle new intent for deep links
        override fun onNewIntent(intent: android.content.Intent?) {
            super.onNewIntent(intent)
            intent?.let { handleDeepLink(it) }
        }
        
        // Process deep link intent
        private fun handleDeepLink(intent: android.content.Intent) {
            // NavController handles deep links automatically
            // when navigation graph has matching deep link
            val handled = navController.handleDeepLink(intent)
            
            if (!handled) {
                // Handle manually if navigation didn't process it
                handleManualDeepLink(intent)
            }
        }
        
        // Manual deep link handling
        private fun handleManualDeepLink(intent: android.content.Intent) {
            val uri = intent.data
            
            uri?.let {
                println("Unhandled deep link: ${it}")
                
                // Parse and route manually
                when (it.host) {
                    "product" -> {
                        val productId = it.lastPathSegment
                        // Navigate to product
                    }
                    "search" -> {
                        val query = it.getQueryParameter("query")
                        // Navigate to search
                    }
                }
            }
        }
        
        // Back stack management for deep links
        override fun onSupportNavigateUp(): Boolean {
            val deepLinkNavOptions = androidx.navigation.NavOptions.Builder()
                .setPopUpTo(R.id.homeFragment, false)
                .build()
            
            return navController.navigateUp(deepLinkNavOptions) || super.onSupportNavigateUp()
        }
    }
    
    // Fragment handling deep links
    class ProductFragment : androidx.fragment.app.Fragment() {
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Get deep link arguments
            // Arguments are automatically populated from deep link
            arguments?.let { args ->
                val productId = args.getString("productId")
                val source = args.getString("android:deepLinkLaunchMode")
                
                // Source indicates how we got here
                // "standard" = normal navigation
                // "single_top" = deep link from external
                // "clear_task" = deep link cleared task
                
                if (source != null) {
                    // Handle deep link specific behavior
                    println("Launched via deep link: ${source}")
                }
                
                productId?.let { loadProduct(it) }
            }
        }
        
        private fun loadProduct(productId: String) {
            println("Loading product: ${productId}")
        }
    }
}
```

## Example: Complete Deep Link Implementation

Complete example showing deep links with Navigation Compose.

```kotlin
/**
 * Complete Deep Link Implementation
 * 
 * Full example with multiple deep link types.
 */
class CompleteDeepLinkApp {
    
    // Routes with deep links
    @Composable
    fun DeepLinkNavigation() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "home"
        ) {
            // Home destination
            androidx.navigation.compose.composable(
                route = "home",
                deepLinks = listOf(
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://home"
                    },
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://myapp.com/"
                    },
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://www.myapp.com/"
                    }
                )
            ) {
                HomeScreen(
                    onNavigateToProduct = { productId ->
                        navController.navigate("product/${productId}")
                    }
                )
            }
            
            // Product destination
            androidx.navigation.compose.composable(
                route = "product/{productId}",
                arguments = listOf(
                    androidx.navigation.compose.navArgument("productId") {
                        type = androidx.navigation.compose.NavType.StringType
                    }
                ),
                deepLinks = listOf(
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://product/{productId}"
                    },
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://myapp.com/product/{productId}"
                    },
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://www.myapp.com/product/{productId}"
                    },
                    // Short link format
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://p/{productId}"
                    }
                )
            ) { backStackEntry ->
                val productId = backStackEntry.arguments?.getString("productId") ?: ""
                ProductScreen(
                    productId = productId,
                    onNavigateBack = { navController.popBackStack() }
                )
            }
            
            // Category destination
            androidx.navigation.compose.composable(
                route = "category/{categoryId}",
                arguments = listOf(
                    androidx.navigation.compose.navArgument("categoryId") {
                        type = androidx.navigation.compose.NavType.StringType
                    }
                ),
                deepLinks = listOf(
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://category/{categoryId}"
                    },
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://myapp.com/category/{categoryId}"
                    }
                )
            ) { backStackEntry ->
                val categoryId = backStackEntry.arguments?.getString("categoryId") ?: ""
                CategoryScreen(
                    categoryId = categoryId,
                    onNavigateBack = { navController.popBackStack() }
                )
            }
            
            // Search destination with query params
            androidx.navigation.compose.composable(
                route = "search?query={query}",
                arguments = listOf(
                    androidx.navigation.compose.navArgument("query") {
                        type = androidx.navigation.compose.NavType.StringType
                        defaultValue = ""
                    }
                ),
                deepLinks = listOf(
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://search"
                    },
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "https://myapp.com/search"
                    },
                    // Query parameter deep link
                    androidx.navigation.compose.navDeepLink {
                        uriPattern = "myapp://search?query={query}"
                    }
                )
            ) { backStackEntry ->
                val query = backStackEntry.arguments?.getString("query") ?: ""
                SearchScreen(
                    query = query,
                    onNavigateBack = { navController.popBackStack() }
                )
            }
        }
    }
    
    @Composable
    fun HomeScreen(onNavigateToProduct: (String) -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            androidx.compose.material3.Text("Home Screen")
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            
            androidx.compose.material3.Button(
                onClick = { onNavigateToProduct("123") }
            ) {
                androidx.compose.material3.Text("View Product 123")
            }
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            
            // Display deep link examples
            androidx.compose.material3.Text(
                "Deep link examples:",
                style = androidx.compose.material3.MaterialTheme.typography.titleMedium
            )
            androidx.compose.material3.Text("myapp://product/123")
            androidx.compose.material3.Text("https://myapp.com/product/123")
            androidx.compose.material3.Text("myapp://search?query=abc")
        }
    }
    
    @Composable
    fun ProductScreen(productId: String, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            androidx.compose.material3.Text("Product: ${productId}")
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun CategoryScreen(categoryId: String, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            androidx.compose.material3.Text("Category: ${categoryId}")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun SearchScreen(query: String, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            androidx.compose.material3.Text("Search: ${if (query.isEmpty()) "(empty)" else query}")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
}
```

## Output Statement Results

**Deep Link Types:**
- Custom scheme: myapp://path (not verified)
- App Links: https://myapp.com/path (verified, autoVerify)
- Intent-based: android.intent.action.VIEW

**Deep Link Components:**
- uriPattern: URI pattern with {parameter}
- navDeepLink: DSL for defining deep links
- arguments: Map URI parameters to arguments

**Navigation Graph:**
- <deepLink>: XML element for deep link
- autoVerify: Enable App Links verification
- android:scheme: Custom or https
- android:host: Domain for App Links

**Handling Deep Links:**
- handleDeepLink(): Auto-processes deep links
- onNewIntent(): For singleTop activities
- Intent data: Contains deep link URI
- Launch modes: standard, single_top, clear_task

**Testing Deep Links:**
- ADB: adb shell am start -W -a android.intent.action.VIEW -d "myapp://product/123"
- Browser: Open URL in browser
- Link: Click on links in other apps

## Cross-References

- See: [01_Jetpack_Navigation_Basics.md](./01_Jetpack_Navigation_Basics.md)
- See: [02_Navigation_Compose.md](./02_Navigation_Compose.md)
- See: [04_Navigation_Arguments.md](./04_Navigation_Arguments.md)
- See: [../../04_DATA_PERSISTENCE/01_Database_Development/01_Room_Database_Basics.md](../../04_DATA_PERSISTENCE/01_Database_Development/01_Room_Database_Basics.md)

## End of Deep Linking Guide