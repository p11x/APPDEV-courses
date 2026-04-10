# LEARNING OBJECTIVES

1. Understanding Navigation in Compose
2. Creating navigation graph
3. Passing arguments between screens
4. Managing navigation state
5. Using deep links

```kotlin
package com.android.compose.navigation
```

---

## SECTION 1: NAVIGATION BASICS

```kotlin
/**
 * Navigation in Compose
 * 
 * Jetpack Navigation Compose for declarative navigation.
 */
object NavigationBasics {
    
    // Dependencies
    const val NAV_VERSION = "2.7.6"
    
    // Setup
    fun getDependencies(): String {
        return """
dependencies {
    implementation "androidx.navigation:navigation-compose:$NAV_VERSION"
}
        """.trimIndent()
    }
    
    // NavHost setup
    @Composable
    fun NavigationSetup() {
        val navController = androidx.navigation.compose.NavHostController
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "home"
        ) {
            androidx.navigation.compose.composable("home") {
                HomeScreen(
                    onNavigateToDetail = { itemId ->
                        navController.navigate("detail/$itemId")
                    }
                )
            }
            
            androidx.navigation.compose.composable("detail/{itemId}") { backStackEntry ->
                val itemId = backStackEntry.arguments?.getString("itemId")
                DetailScreen(itemId = itemId ?: "")
            }
        }
    }
    
    @Composable
    fun HomeScreen(onNavigateToDetail: (String) -> Unit) {
        androidx.compose.material3.Button(onClick = { onNavigateToDetail("1") }) {
            androidx.compose.material3.Text("Go to Detail")
        }
    }
    
    @Composable
    fun DetailScreen(itemId: String) {
        androidx.compose.material3.Text("Item: $itemId")
    }
}
```

---

## SECTION 2: NAVIGATION GRAPH

```kotlin
/**
 * Navigation Graph
 * 
 * Creating type-safe navigation routes.
 */
object NavigationGraph {
    
    // Sealed class for routes (type-safe)
    sealed class Screen(val route: String) {
        object Home : Screen("home")
        object Detail : Screen("detail/{itemId}") {
            fun createRoute(itemId: String) = "detail/$itemId"
        }
        object Profile : Screen("profile")
        object Settings : Screen("settings")
    }
    
    // NavHost with typed routes
    @Composable
    fun TypedNavigation(navController: androidx.navigation.compose.NavHostController) {
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = Screen.Home.route
        ) {
            // Home
            androidx.navigation.compose.composable(Screen.Home.route) {
                HomeScreen(
                    onNavigateToDetail = { itemId ->
                        navController.navigate(Screen.Detail.createRoute(itemId))
                    }
                )
            }
            
            // Detail with arguments
            androidx.navigation.compose.composable(
                route = Screen.Detail.route,
                arguments = listOf(
                    androidx.navigation.compose.navArgument("itemId") {
                        type = androidx.navigation.compose.NavType.StringType
                    }
                )
            ) { backStackEntry ->
                val itemId = backStackEntry.arguments?.getString("itemId") ?: ""
                DetailScreen(itemId = itemId)
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
    
    @Composable
    fun HomeScreen(onNavigateToDetail: (String) -> Unit) {
        androidx.compose.material3.Text("Home Screen")
    }
    
    @Composable
    fun DetailScreen(itemId: String) {
        androidx.compose.material3.Text("Detail: $itemId")
    }
    
    @Composable
    fun ProfileScreen(onNavigateToSettings: () -> Unit) {
        androidx.compose.material3.Text("Profile Screen")
    }
    
    @Composable
    fun SettingsScreen(onNavigateBack: () -> Unit) {
        androidx.compose.material3.Text("Settings Screen")
    }
}
```

---

## SECTION 3: NAVIGATION WITH BOTTOM NAV

```kotlin
/**
 * Navigation with Bottom Navigation
 * 
 * Implementing bottom navigation with Compose.
 */
object BottomNavigation {
    
    // Bottom nav items
    enum class BottomNavItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    ) {
        HOME("home", "Home", androidx.compose.material.icons.Icons.Default.Home),
        SEARCH("search", "Search", androidx.compose.material.icons.Icons.Default.Search),
        PROFILE("profile", "Profile", androidx.compose.material.icons.Icons.Default.Person)
    }
    
    // Main scaffold with bottom nav
    @Composable
    fun MainScaffold() {
        val navController = androidx.navigation.compose.rememberNavController()
        val currentBackStackEntry by navController.currentBackStackEntryAsState()
        val currentRoute = currentBackStackEntry?.destination?.route
        
        androidx.compose.material3.Scaffold(
            bottomBar = {
                androidx.compose.material3.NavigationBar {
                    BottomNavItem.entries.forEach { item ->
                        androidx.compose.material3.NavigationBarItem(
                            icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                            label = { androidx.compose.material3.Text(item.title) },
                            selected = currentRoute == item.route,
                            onClick = {
                                if (currentRoute != item.route) {
                                    navController.navigate(item.route) {
                                        popUpTo(navController.graph.startDestinationId) {
                                            saveState = true
                                        }
                                        launchSingleTop = true
                                        restoreState = true
                                    }
                                }
                            }
                        )
                    }
                }
            }
        ) { paddingValues ->
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = BottomNavItem.HOME.route,
                modifier = Modifier.padding(paddingValues)
            ) {
                androidx.navigation.compose.composable(BottomNavItem.HOME.route) {
                    HomeContent()
                }
                androidx.navigation.compose.composable(BottomNavItem.SEARCH.route) {
                    SearchContent()
                }
                androidx.navigation.compose.composable(BottomNavItem.PROFILE.route) {
                    ProfileContent()
                }
            }
        }
    }
    
    @Composable
    fun HomeContent() {
        androidx.compose.material3.Text("Home")
    }
    
    @Composable
    fun SearchContent() {
        androidx.compose.material3.Text("Search")
    }
    
    @Composable
    fun ProfileContent() {
        androidx.compose.material3.Text("Profile")
    }
    
    // Helper
    @Composable
    fun navController.currentBackStackEntryAsState(): androidx.compose.runtime.State<androidx.navigation.NavBackStackEntry?> {
        return androidx.compose.runtime.collectAsState()
    }
}
```

---

## SECTION 4: PASSING ARGUMENTS

```kotlin
/**
 * Passing Arguments
 * 
 * Type-safe argument passing in navigation.
 */
object PassingArguments {
    
    // Argument data class
    data class DetailArgs(val itemId: String, val category: String?)
    
    // Serializable argument (for complex types)
    @kotlinx.serialization.Serializable
    data class UserArgs(val userId: String, val username: String)
    
    // Navigation with complex arguments
    @Composable
    fun ComplexArgsNavigation(navController: androidx.navigation.compose.NavHostController) {
        // Pass simple args
        navController.navigate("detail/123")
        
        // Pass serializable (using JSON)
        val userJson = java.net.URLEncoder.encode(
            kotlinx.serialization.json.Json.encodeToString(UserArgs.serializer(), UserArgs("1", "John")),
            "UTF-8"
        )
        navController.navigate("user/$userJson")
    }
    
    // Argument types
    @Composable
    fun ArgumentTypes() {
        // String type
        androidx.navigation.compose.navArgument("stringArg") {
            type = androidx.navigation.compose.NavType.StringType
            defaultValue = "default"
        }
        
        // Int type
        androidx.navigation.compose.navArgument("intArg") {
            type = androidx.navigation.compose.NavType.IntType
            defaultValue = 0
        }
        
        // Bool type
        androidx.navigation.compose.navArgument("boolArg") {
            type = androidx.navigation.compose.NavType.BoolType
            defaultValue = false
        }
        
        // Long type
        androidx.navigation.compose.navArgument("longArg") {
            type = androidx.navigation.compose.NavType.LongType
        }
        
        // Serializable type
        androidx.navigation.compose.navArgument("userArg") {
            type = androidx.navigation.compose.NavType.SerializableType(UserArgs::class.java)
        }
    }
}
```

---

## SECTION 5: DEEP LINKS

```kotlin
/**
 * Deep Links
 * 
 * Handling deep links in Navigation Compose.
 */
object DeepLinks {
    
    // Define deep links
    @Composable
    fun DeepLinkSetup() {
        val deepLinks = listOf(
            // URI pattern
            androidx.navigation.compose.navDeepLink {
                uriPattern = "https://myapp.com/detail/{itemId}"
            },
            // Action
            androidx.navigation.compose.navDeepLink {
                action = "android.intent.action.VIEW"
                uriPattern = "myapp://detail/{itemId}"
            }
        )
        
        // Apply to composable
        androidx.navigation.compose.composable(
            route = "detail/{itemId}",
            deepLinks = deepLinks
        ) { backStackEntry ->
            val itemId = backStackEntry.arguments?.getString("itemId")
            androidx.compose.material3.Text("Detail: $itemId")
        }
    }
    
    // Multiple deep links
    @Composable
    fun MultipleDeepLinks() {
        androidx.navigation.compose.composable(
            route = "product/{productId}",
            deepLinks = listOf(
                // App links
                androidx.navigation.compose.navDeepLink {
                    uriPattern = "https://myapp.com/product/{productId}"
                },
                // App links with www
                androidx.navigation.compose.navDeepLink {
                    uriPattern = "https://www.myapp.com/product/{productId}"
                },
                // Custom scheme
                androidx.navigation.compose.navDeepLink {
                    uriPattern = "myapp://product/{productId}"
                },
                // Short link
                androidx.navigation.compose.navDeepLink {
                    uriPattern = "myapp://go/product/{productId}"
                }
            )
        ) { backStackEntry ->
            val productId = backStackEntry.arguments?.getString("productId")
            ProductScreen(productId = productId ?: "")
        }
    }
    
    @Composable
    fun ProductScreen(productId: String) {
        androidx.compose.material3.Text("Product: $productId")
    }
}
```

---

## EXAMPLE: COMPLETE NAVIGATION APP

```kotlin
/**
 * Complete Navigation App Example
 * 
 * Full implementation of navigation.
 */
class CompleteNavigationApp {
    
    // Define all routes
    object Routes {
        const val HOME = "home"
        const val DETAIL = "detail/{itemId}"
        const val PROFILE = "profile"
        const val SETTINGS = "settings"
        
        fun detail(itemId: String) = "detail/$itemId"
    }
    
    // Main app composable
    @Composable
    fun AppNavigation() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Nav graph
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = Routes.HOME
        ) {
            // Home
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
            
            // Detail
            androidx.navigation.compose.composable(
                route = Routes.DETAIL,
                arguments = listOf(
                    androidx.navigation.compose.navArgument("itemId") {
                        type = androidx.navigation.compose.NavType.StringType
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
            androidx.navigation.compose.composable(Routes.PROFILE) {
                ProfileScreen(
                    onNavigateToSettings = {
                        navController.navigate(Routes.SETTINGS)
                    }
                )
            }
            
            // Settings
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
        Column {
            androidx.compose.material3.Text("Home Screen")
            androidx.compose.material3.Button(onClick = { onNavigateToDetail("1") }) {
                androidx.compose.material3.Text("View Detail")
            }
            androidx.compose.material3.Button(onClick = onNavigateToProfile) {
                androidx.compose.material3.Text("View Profile")
            }
        }
    }
    
    @Composable
    fun DetailScreen(
        itemId: String,
        onNavigateBack: () -> Unit
    ) {
        Column {
            androidx.compose.material3.Text("Detail: $itemId")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun ProfileScreen(onNavigateToSettings: () -> Unit) {
        Column {
            androidx.compose.material3.Text("Profile Screen")
            androidx.compose.material3.Button(onClick = onNavigateToSettings) {
                androidx.compose.material3.Text("Settings")
            }
        }
    }
    
    @Composable
    fun SettingsScreen(onNavigateBack: () -> Unit) {
        Column {
            androidx.compose.material3.Text("Settings Screen")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Navigation Compose:**
- NavHost: Navigation container
- NavController: Navigation control
- composable: Screen definitions
- NavType: Argument types
- navDeepLink: Deep link handling

**Navigation Patterns:**
- Type-safe routes with sealed classes
- Arguments via route parameters
- Bottom navigation integration
- Deep links for external navigation
- Navigation state management
- Pop up to with state restoration
- Single top launch mode

**Common Use Cases:**
- Home to Detail navigation
- Bottom tab navigation
- Passing data between screens
- Deep linking from URLs
- Nested navigation

---

## CROSS-REFERENCES

- See: 06_NAVIGATION/01_Navigation_Architecture/01_Jetpack_Navigation_Basics.md
- See: 06_NAVIGATION/01_Navigation_Architecture/02_Navigation_Compose.md

---

## END OF NAVIGATION COMPOSE GUIDE
