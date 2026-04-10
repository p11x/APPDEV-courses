# Navigation Compose

## Learning Objectives

1. Understanding Navigation Compose library
2. Creating navigation graph with NavHost
3. Implementing type-safe routes with sealed classes
4. Managing navigation state with NavController
5. Integrating bottom navigation with Compose
6. Using nested navigation graphs
7. Implementing navigation animations
8. Handling back press and system navigation

## Section 1: Navigation Compose Overview

Navigation Compose provides declarative navigation for Jetpack Compose apps. It replaces the traditional XML-based navigation graph with Compose code.

```kotlin
/**
 * Navigation Compose Overview
 * 
 * Navigation Compose uses:
 * - NavHost: Container composable for destinations
 * - NavController: Manages navigation state
 * - composable(): DSL for defining destinations
 * - rememberNavController(): Creates controller
 */
object NavigationComposeOverview {
    
    // Dependencies
    const val NAV_VERSION = "2.7.6"
    
    fun getDependencies(): String {
        return """
dependencies {
    implementation "androidx.navigation:navigation-compose:$NAV_VERSION"
}
        """.trimIndent()
    }
    
    // Basic setup
    @Composable
    fun BasicNavigationSetup() {
        // Create NavController
        // Remember preserves state across recompositions
        val navController = androidx.navigation.compose.rememberNavController()
        
        // NavHost with navigation graph
        // NavHost connects destinations and handles transitions
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "home" // First destination
        ) {
            // Define destinations using composable DSL
            // Each composable block defines a screen
            
            // Home destination
            androidx.navigation.compose.composable("home") {
                // Compose UI for home screen
                androidx.compose.foundation.layout.Column(
                    modifier = Modifier.fillMaxSize(),
                    horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
                    verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
                ) {
                    androidx.compose.material3.Text("Home Screen")
                    
                    androidx.compose.material3.Button(
                        onClick = {
                            // Navigate to detail
                            navController.navigate("detail/123")
                        }
                    ) {
                        androidx.compose.material3.Text("Go to Detail")
                    }
                }
            }
            
            // Detail destination with argument
            androidx.navigation.compose.composable("detail/{itemId}") { backStackEntry ->
                // Get argument from back stack entry
                val itemId = backStackEntry.arguments?.getString("itemId") ?: ""
                
                androidx.compose.foundation.layout.Column(
                    modifier = Modifier.fillMaxSize(),
                    horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
                    verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
                ) {
                    androidx.compose.material3.Text("Detail: ${itemId}")
                    
                    androidx.compose.material3.Button(
                        onClick = { navController.popBackStack() }
                    ) {
                        androidx.compose.material3.Text("Back")
                    }
                }
            }
        }
    }
}
```

## Section 2: Type-Safe Routes with Sealed Classes

Using sealed classes provides type-safe routes that are easier to maintain and refactor.

```kotlin
/**
 * Type-Safe Routes
 * 
 * Sealed classes provide:
 * - Type-safe navigation routes
 * - Compile-time verification
 * - Better IDE support
 * - Easy maintenance
 */
object TypeSafeRoutes {
    
    // Sealed class defining all routes
    // Each object represents a destination
    sealed class Screen(val route: String) {
        // Home route (no arguments)
        object Home : Screen("home")
        
        // Detail route with itemId argument
        // The route includes argument placeholder
        object Detail : Screen("detail/{itemId}") {
            // Create full route with argument value
            fun createRoute(itemId: String) = "detail/${itemId}"
        }
        
        // Profile route
        object Profile : Screen("profile")
        
        // Settings route
        object Settings : Screen("settings")
        
        // Product with multiple arguments
        object Product : Screen("product/{productId}/category/{categoryId}") {
            fun createRoute(productId: String, categoryId: String) = 
                "product/${productId}/category/${categoryId}"
        }
    }
    
    // NavHost with type-safe routes
    @Composable
    fun TypedNavigationHost() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = Screen.Home.route
        ) {
            // Home
            androidx.navigation.compose.composable(Screen.Home.route) {
                HomeScreen(
                    onNavigateToDetail = { itemId ->
                        // Using type-safe route creation
                        navController.navigate(Screen.Detail.createRoute(itemId))
                    },
                    onNavigateToProfile = {
                        navController.navigate(Screen.Profile.route)
                    }
                )
            }
            
            // Detail with argument
            androidx.navigation.compose.composable(
                route = Screen.Detail.route, // "detail/{itemId}"
                arguments = listOf(
                    // Define argument with type
                    androidx.navigation.compose.navArgument("itemId") {
                        type = androidx.navigation.compose.NavType.StringType
                        // Optional: default value
                        defaultValue = "default"
                        // Optional: nullable
                        nullable = false
                    }
                )
            ) { backStackEntry ->
                val itemId = backStackEntry.arguments?.getString("itemId") ?: ""
                DetailScreen(
                    itemId = itemId,
                    onNavigateBack = { navController.popBackStack() }
                )
            }
            
            // Profile
            androidx.navigation.compose.composable(Screen.Profile.route) {
                ProfileScreen(
                    onNavigateToSettings = {
                        navController.navigate(Screen.Settings.route)
                    }
                )
            }
            
            // Settings
            androidx.navigation.compose.composable(Screen.Settings.route) {
                SettingsScreen(
                    onNavigateBack = { navController.popBackStack() }
                )
            }
        }
    }
    
    // Screen composables
    @Composable
    fun HomeScreen(
        onNavigateToDetail: (String) -> Unit,
        onNavigateToProfile: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Home Screen")
            
            androidx.compose.material3.Button(
                onClick = { onNavigateToDetail("123") }
            ) {
                androidx.compose.material3.Text("View Detail")
            }
            
            androidx.compose.material3.Button(
                onClick = onNavigateToProfile
            ) {
                androidx.compose.material3.Text("View Profile")
            }
        }
    }
    
    @Composable
    fun DetailScreen(
        itemId: String,
        onNavigateBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize()
        ) {
            androidx.compose.material3.Text("Item: ${itemId}")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun ProfileScreen(onNavigateToSettings: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Profile")
            androidx.compose.material3.Button(onClick = onNavigateToSettings) {
                androidx.compose.material3.Text("Settings")
            }
        }
    }
    
    @Composable
    fun SettingsScreen(onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Settings")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
}
```

## Section 3: Bottom Navigation Integration

Navigation Compose integrates seamlessly with bottom navigation for tab-based navigation.

```kotlin
/**
 * Bottom Navigation with Compose
 * 
 * Combining Navigation Compose with Material3 bottom navigation.
 * The bottom bar stays persistent across tab destinations.
 */
object BottomNavigationIntegration {
    
    // Bottom nav items definition
    // Each item has route, title, and icon
    enum class BottomNavItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    ) {
        HOME("home", "Home", androidx.compose.material.icons.Icons.Default.Home),
        SEARCH("search", "Search", androidx.compose.material.icons.Icons.Default.Search),
        PROFILE("profile", "Profile", androidx.compose.material.icons.Icons.Default.Person)
    }
    
    // Main scaffold with bottom navigation
    @Composable
    fun MainScaffold() {
        // Create NavController - must be outside Scaffold for proper state
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Get current back stack entry for state
        // This observes navigation state changes
        val currentBackStackEntry by navController.currentBackStackEntryAsState()
        
        // Get current route from destination
        val currentRoute = currentBackStackEntry?.destination?.route
        
        // Scaffold with bottom bar
        androidx.compose.material3.Scaffold(
            bottomBar = {
                // Bottom navigation bar
                androidx.compose.material3.NavigationBar {
                    // Iterate through bottom nav items
                    BottomNavItem.entries.forEach { item ->
                        // Determine if this item is selected
                        // Compare current route with item route
                        val selected = currentRoute == item.route
                        
                        // Navigation bar item
                        androidx.compose.material3.NavigationBarItem(
                            icon = {
                                androidx.compose.material3.Icon(
                                    item.icon,
                                    contentDescription = item.title
                                )
                            },
                            label = { androidx.compose.material3.Text(item.title) },
                            selected = selected,
                            onClick = {
                                // Only navigate if not already on this route
                                // Prevents unnecessary navigation
                                if (currentRoute != item.route) {
                                    // Navigate with options
                                    navController.navigate(item.route) {
                                        // Pop up to start destination
                                        // This clears stack when switching tabs
                                        popUpTo(navController.graph.startDestinationId) {
                                            // Save state for restoration
                                            saveState = true
                                        }
                                        
                                        // Launch single top
                                        // Prevents duplicate destinations
                                        launchSingleTop = true
                                        
                                        // Restore state when coming back
                                        restoreState = true
                                    }
                                }
                            }
                        )
                    }
                }
            }
        ) { paddingValues ->
            // NavHost with padding from scaffold
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = BottomNavItem.HOME.route,
                modifier = Modifier.padding(paddingValues)
            ) {
                // Home destination
                androidx.navigation.compose.composable(BottomNavItem.HOME.route) {
                    HomeContent()
                }
                
                // Search destination
                androidx.navigation.compose.composable(BottomNavItem.SEARCH.route) {
                    SearchContent()
                }
                
                // Profile destination
                androidx.navigation.compose.composable(BottomNavItem.PROFILE.route) {
                    ProfileContent()
                }
            }
        }
    }
    
    @Composable
    fun HomeContent() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Home")
        }
    }
    
    @Composable
    fun SearchContent() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Search")
        }
    }
    
    @Composable
    fun ProfileContent() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Profile")
        }
    }
    
    // Helper extension for state observation
    @Composable
    fun navController.currentBackStackEntryAsState(): androidx.compose.runtime.State<androidx.navigation.NavBackStackEntry?> {
        return androidx.compose.runtime.collectAsState()
    }
}
```

## Section 4: Nested Navigation Graphs

Nested navigation graphs allow organizing navigation into hierarchical structures.

```kotlin
/**
 * Nested Navigation Graphs
 * 
 * Nested graphs allow:
 * - Organizing related destinations
 * - Creating separate navigation flows
 * - Reusing navigation logic
 * - Scoped back stack management
 */
object NestedNavigation {
    
    // Root graph with nested graphs
    @Composable
    fun NestedNavigationHost() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "main_graph"
        ) {
            // Root/nested graph start
            // Using navigation call to create nested graph
            androidx.navigation.compose.navigation(
                startDestination = "home",
                route = "main_graph"
            ) {
                // Home in nested graph
                androidx.navigation.compose.composable("home") {
                    androidx.compose.material3.Text("Home")
                }
                
                // Detail - also in nested graph
                androidx.navigation.compose.composable("detail/{itemId}") { backStackEntry ->
                    val itemId = backStackEntry.arguments?.getString("itemId") ?: ""
                    androidx.compose.material3.Text("Detail: ${itemId}")
                }
            }
            
            // Another nested graph for auth
            androidx.navigation.compose.navigation(
                startDestination = "login",
                route = "auth_graph"
            ) {
                androidx.navigation.compose.composable("login") {
                    androidx.compose.material3.Text("Login")
                }
                
                androidx.navigation.compose.composable("register") {
                    androidx.compose.material3.Text("Register")
                }
            }
        }
    }
    
    // Nested graph for specific feature
    @Composable
    fun FeatureNestedGraph() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "product_list"
        ) {
            // Products nested graph
            androidx.navigation.compose.navigation(
                startDestination = "product_list",
                route = "products_graph"
            ) {
                // Product list
                androidx.navigation.compose.composable("product_list") {
                    ProductListScreen(
                        onProductClick = { productId ->
                            navController.navigate("product_detail/${productId}")
                        }
                    )
                }
                
                // Product detail
                androidx.navigation.compose.composable("product_detail/{productId}") { backStackEntry ->
                    val productId = backStackEntry.arguments?.getString("productId") ?: ""
                    ProductDetailScreen(
                        productId = productId,
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
            }
        }
    }
    
    @Composable
    fun ProductListScreen(onProductClick: (String) -> Unit) {
        androidx.compose.foundation.layout.Column {
            androidx.compose.material3.Text("Products")
            androidx.compose.material3.Button(onClick = { onProductClick("1") }) {
                androidx.compose.material3.Text("Product 1")
            }
        }
    }
    
    @Composable
    fun ProductDetailScreen(productId: String, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column {
            androidx.compose.material3.Text("Product: ${productId}")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
}
```

## Example: Complete Navigation Compose App

Full implementation example.

```kotlin
/**
 * Complete Navigation Compose App
 * 
 * Full example with all Navigation Compose features.
 */
class CompleteNavigationComposeApp {
    
    // App routes
    object Routes {
        const val HOME = "home"
        const val DETAIL = "detail/{itemId}"
        const val PROFILE = "profile"
        const val SETTINGS = "settings"
        
        fun detail(itemId: String) = "detail/${itemId}"
    }
    
    // Main app composable
    @Composable
    fun AppNavigation() {
        // Create NavController
        val navController = androidx.navigation.compose.rememberNavController()
        
        // NavHost
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = Routes.HOME
        ) {
            // Home screen
            androidx.navigation.compose.composable(Routes.HOME) {
                HomeScreen(
                    onNavigateToDetail = { itemId ->
                        navController.navigate(Routes.detail(itemId))
                    },
                    onNavigateToProfile = {
                        navController.navigate(Routes.PROFILE)
                    }
                )
            }
            
            // Detail screen
            androidx.navigation.compose.composable(
                route = Routes.DETAIL,
                arguments = listOf(
                    androidx.navigation.compose.navArgument("itemId") {
                        type = androidx.navigation.compose.NavType.StringType
                        defaultValue = ""
                    }
                )
            ) { backStackEntry ->
                val itemId = backStackEntry.arguments?.getString("itemId") ?: ""
                DetailScreen(
                    itemId = itemId,
                    onNavigateBack = { navController.popBackStack() }
                )
            }
            
            // Profile screen
            androidx.navigation.compose.composable(Routes.PROFILE) {
                ProfileScreen(
                    onNavigateToSettings = {
                        navController.navigate(Routes.SETTINGS)
                    }
                )
            }
            
            // Settings screen
            androidx.navigation.compose.composable(Routes.SETTINGS) {
                SettingsScreen(
                    onNavigateBack = { navController.popBackStack() }
                )
            }
        }
    }
    
    @Composable
    fun HomeScreen(
        onNavigateToDetail: (String) -> Unit,
        onNavigateToProfile: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Home Screen")
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            
            androidx.compose.material3.Button(
                onClick = { onNavigateToDetail("123") }
            ) {
                androidx.compose.material3.Text("View Detail")
            }
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            
            androidx.compose.material3.Button(
                onClick = onNavigateToProfile
            ) {
                androidx.compose.material3.Text("View Profile")
            }
        }
    }
    
    @Composable
    fun DetailScreen(
        itemId: String,
        onNavigateBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Item: ${itemId}")
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun ProfileScreen(onNavigateToSettings: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Profile Screen")
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            
            androidx.compose.material3.Button(onClick = onNavigateToSettings) {
                androidx.compose.material3.Text("Settings")
            }
        }
    }
    
    @Composable
    fun SettingsScreen(onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
}
```

## Output Statement Results

**Navigation Compose Components:**
- NavHost: Navigation container composable
- NavController: Manages navigation state
- composable(): Defines destinations
- rememberNavController(): Creates controller
- currentBackStackEntryAsState(): Observes state

**Route Types:**
- String routes: Simple but not type-safe
- Sealed classes: Type-safe and maintainable
- Nested routes: Include arguments in route

**Navigation Options:**
- popUpTo: Clear back stack
- launchSingleTop: Prevent duplicates
- restoreState: Restore saved state
- saveState: Save state for restoration

**Bottom Navigation:**
- Persistent bottom bar across destinations
- State saving/restoration
- Single top for tab switching
- Proper padding handling

**Best Practices:**
- Use sealed classes for routes
- Keep NavController at top level
- Handle padding from Scaffold
- Use proper state observation

## Cross-References

- See: [01_Jetpack_Navigation_Basics.md](./01_Jetpack_Navigation_Basics.md)
- See: [03_Deep_Linking.md](./03_Deep_Linking.md)
- See: [04_Navigation_Arguments.md](./04_Navigation_Arguments.md)
- See: [../02_Navigation_UI/01_Bottom_Navigation.md](../02_Navigation_UI/01_Bottom_Navigation.md)

## End of Navigation Compose Guide