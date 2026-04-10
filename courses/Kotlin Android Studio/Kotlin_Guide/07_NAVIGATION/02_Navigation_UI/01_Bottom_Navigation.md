# Bottom Navigation

## Learning Objectives

1. Understanding bottom navigation patterns
2. Implementing BottomNavigationView with Navigation component
3. Creating bottom navigation with Navigation Compose
4. Managing bottom navigation state
5. Handling badge updates
6. Customizing bottom navigation appearance
7. Implementing nested navigation with bottom nav
8. Handling navigation state preservation

## Section 1: Bottom Navigation Overview

Bottom navigation provides persistent access to top-level destinations in an app. It's ideal for apps with 3-5 primary navigation targets.

```kotlin
/**
 * Bottom Navigation Overview
 * 
 * Bottom Navigation is used when:
 * - 3-5 primary destinations
 * - Frequent switching between destinations
 * - Maintaining state per destination
 * - Quick access to main features
 * 
 * Android implementation:
 * - Material Design BottomNavigationView
 * - Navigation component integration
 * - Compose BottomNavigation
 */
object BottomNavigationOverview {
    
    // Bottom nav destinations
    object BottomNavItems {
        // Define items with ID, title, icon
        const val HOME = "home"
        const val SEARCH = "search"
        const val PROFILE = "profile"
        
        // Item IDs in navigation graph
        const val HOME_ID = R.id.homeFragment
        const val SEARCH_ID = R.id.searchFragment
        const val PROFILE_ID = R.id.profileFragment
    }
    
    // XML bottom navigation setup
    fun getXmlLayout(): String {
        return """
<!-- activity_main.xml -->
<androidx.constraintlayout.widget.ConstraintLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    
    <androidx.fragment.app.FragmentContainerView
        android:id="@+id/nav_host_fragment"
        android:name="androidx.navigation.fragment.NavHostFragment"
        android:layout_width="0dp"
        android:layout_height="0dp"
        app:defaultNavHost="true"
        app:layout_constraintBottom_toTopOf="@id/bottom_navigation"
        app:layout_constraintEnd_toEndOf="parent"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintTop_toTopOf="parent"
        app:navGraph="@navigation/nav_graph" />
    
    <com.google.android.material.bottomnavigation.BottomNavigationView
        android:id="@+id/bottom_navigation"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        app:layout_constraintBottom_toBottomOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        app:layout_constraintStart_toStartOf="parent"
        app:menu="@menu/bottom_nav_menu"
        app:labelVisibilityMode="labeled" />
    
</androidx.constraintlayout.widget.ConstraintLayout>
        """.trimIndent()
    }
    
    // Menu resource
    fun getMenuXml(): String {
        return """
<!-- res/menu/bottom_nav_menu.xml -->
<menu xmlns:android="http://schemas.android.com/apk/res/android">
    
    <item
        android:id="@+id/homeFragment"
        android:icon="@drawable/ic_home"
        android:title="@string/home" />
    
    <item
        android:id="@+id/searchFragment"
        android:icon="@drawable/ic_search"
        android:title="@string/search" />
    
    <item
        android:id="@+id/profileFragment"
        android:icon="@drawable/ic_profile"
        android:title="@string/profile" />
    
</menu>
        """.trimIndent()
    }
}
```

## Section 2: Navigation Component Integration

Integrating BottomNavigationView with the Navigation component for seamless navigation.

```kotlin
/**
 * Bottom Navigation with Navigation Component
 * 
 * Navigation component provides:
 * - Automatic menu-item-to-destination mapping
 * - Back stack management
 * - State preservation
 * - Proper label visibility
 */
class NavigationComponentBottomNav {
    
    // Activity setup
    class MainActivity : androidx.appcompat.app.AppCompatActivity() {
        
        private lateinit var navController: androidx.navigation.NavController
        private lateinit var bottomNav: com.google.android.material.bottomnavigation.BottomNavigationView
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(R.layout.activity_main)
            
            // Setup Navigation
            val navHostFragment = supportFragmentManager
                .findFragmentById(R.id.nav_host_fragment) as androidx.navigation.fragment.NavHostFragment
            navController = navHostFragment.navController
            
            // Setup Bottom Navigation
            bottomNav = findViewById(R.id.bottom_navigation)
            
            // Connect BottomNavigationView with NavController
            // This automatically handles:
            // - Menu item selection
            // - Destination changes
            // - Back stack updates
            bottomNav.setupWithNavController(navController)
            
            // Optional: Listen for destination changes
            navController.addOnDestinationChangedListener { _, destination, _ ->
                // Hide bottom nav for specific destinations
                when (destination.id) {
                    R.id.detailFragment -> bottomNav.visibility = android.view.View.GONE
                    else -> bottomNav.visibility = android.view.View.VISIBLE
                }
            }
            
            // Optional: Handle reselection (tap on current tab)
            bottomNav.setOnItemReselectedListener { item ->
                // Option 1: Do nothing
                // Or: Pop to start of current tab's back stack
                // This provides a "scroll to top" behavior
                val currentDestination = navController.currentDestination?.id
                if (currentDestination == item.itemId) {
                    // Pop to start destination of this tab
                    navController.popBackStack(item.itemId, false)
                }
            }
        }
        
        // Handle up navigation
        override fun onSupportNavigateUp(): Boolean {
            return navController.navigateUp() || super.onSupportNavigateUp()
        }
    }
    
    // Fragment in bottom navigation
    class HomeFragment : androidx.fragment.app.Fragment() {
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Bottom navigation is already connected
            // No need to manually handle clicks
            
            // Navigate using NavController
            // This updates bottom nav selection automatically
            view.findViewById<android.view.View>(R.id.button).setOnClickListener {
                val navController = androidx.navigation.fragment.NavHostFragment
                    .findFragmentById(R.id.nav_host_fragment)
                    .navController
                
                // Navigate to another destination in the same tab
                navController.navigate(R.id.action_home_to_detail)
            }
        }
    }
    
    // Hiding bottom navigation for detail screens
    class DetailFragment : androidx.fragment.app.Fragment() {
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Get parent activity's bottom nav
            val bottomNav = requireActivity()
                .findViewById<com.google.android.material.bottomnavigation.BottomNavigationView>(
                    R.id.bottom_navigation
                )
            
            // Hide bottom nav
            bottomNav.visibility = android.view.View.GONE
        }
        
        override fun onDestroyView() {
            super.onDestroyView()
            
            // Show bottom nav when leaving
            val bottomNav = requireActivity()
                .findViewById<com.google.android.material.bottomnavigation.BottomNavigationView>(
                    R.id.bottom_navigation
                )
            bottomNav.visibility = android.view.View.VISIBLE
        }
    }
}
```

## Section 3: Navigation Compose Bottom Navigation

Bottom navigation with Jetpack Compose using Navigation Compose and Material3.

```kotlin
/**
 * Navigation Compose Bottom Navigation
 * 
 * Implementing bottom navigation in Compose:
 * - NavigationBar for Material3
 * - NavigationBarItem for items
 * - Integration with NavController
 */
class NavigationComposeBottomNav {
    
    // Define bottom nav items
    enum class BottomNavItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector,
        val selectedIcon: androidx.compose.ui.graphics.vector.ImageVector
    ) {
        HOME(
            route = "home",
            title = "Home",
            icon = androidx.compose.material.icons.Icons.Default.Home,
            selectedIcon = androidx.compose.material.icons.Icons.Filled.Home
        ),
        SEARCH(
            route = "search",
            title = "Search",
            icon = androidx.compose.material.icons.Icons.Default.Search,
            selectedIcon = androidx.compose.material.icons.Icons.Filled.Search
        ),
        PROFILE(
            route = "profile",
            title = "Profile",
            icon = androidx.compose.material.icons.Icons.Default.Person,
            selectedIcon = androidx.compose.material.icons.Icons.Filled.Person
        ),
        CART(
            route = "cart",
            title = "Cart",
            icon = androidx.compose.material.icons.Icons.Default.ShoppingCart,
            selectedIcon = androidx.compose.material.icons.Icons.Filled.ShoppingCart
        )
    }
    
    // Main scaffold with bottom navigation
    @Composable
    fun BottomNavScaffold() {
        // Create NavController
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Get current back stack entry
        val currentBackStackEntry by navController.currentBackStackEntryAsState()
        
        // Get current route
        val currentRoute = currentBackStackEntry?.destination?.route
        
        // Define items that show bottom nav
        val bottomNavItems = listOf(
            BottomNavItem.HOME,
            BottomNavItem.SEARCH,
            BottomNavItem.PROFILE,
            BottomNavItem.CART
        )
        
        // Determine if bottom nav should be visible
        val showBottomBar = currentRoute in bottomNavItems.map { it.route }
        
        // Scaffold with bottom bar
        androidx.compose.material3.Scaffold(
            bottomBar = {
                if (showBottomBar) {
                    androidx.compose.material3.NavigationBar {
                        bottomNavItems.forEach { item ->
                            val selected = currentRoute == item.route
                            
                            androidx.compose.material3.NavigationBarItem(
                                selected = selected,
                                onClick = {
                                    // Navigate if not already selected
                                    if (currentRoute != item.route) {
                                        navController.navigate(item.route) {
                                            // Pop up to avoid building large stack
                                            popUpTo(navController.graph.startDestinationId) {
                                                saveState = true
                                            }
                                            
                                            // Launch single top
                                            launchSingleTop = true
                                            
                                            // Restore state
                                            restoreState = true
                                        }
                                    }
                                },
                                icon = {
                                    androidx.compose.material3.Icon(
                                        imageVector = if (selected) item.selectedIcon else item.icon,
                                        contentDescription = item.title
                                    )
                                },
                                label = { androidx.compose.material3.Text(item.title) },
                                // Badge for cart
                                badge = {
                                    if (item == BottomNavItem.CART) {
                                        // Show badge with count
                                        androidx.compose.material3.Text("3")
                                    }
                                }
                            )
                        }
                    }
                }
            }
        ) { paddingValues ->
            // NavHost with padding
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = BottomNavItem.HOME.route,
                modifier = Modifier.padding(paddingValues)
            ) {
                // Home destination
                androidx.navigation.compose.composable(BottomNavItem.HOME.route) {
                    HomeScreen(
                        onNavigateToSearch = {
                            navController.navigate(BottomNavItem.SEARCH.route)
                        }
                    )
                }
                
                // Search destination
                androidx.navigation.compose.composable(BottomNavItem.SEARCH.route) {
                    SearchScreen()
                }
                
                // Profile destination
                androidx.navigation.compose.composable(BottomNavItem.PROFILE.route) {
                    ProfileScreen()
                }
                
                // Cart destination (also bottom nav item)
                androidx.navigation.compose.composable(BottomNavItem.CART.route) {
                    CartScreen(
                        onNavigateToCheckout = {
                            navController.navigate("checkout")
                        }
                    )
                }
                
                // Checkout (not in bottom nav)
                androidx.navigation.compose.composable("checkout") {
                    CheckoutScreen(
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
                
                // Detail (not in bottom nav)
                androidx.navigation.compose.composable("detail/{itemId}") { backStackEntry ->
                    val itemId = backStackEntry.arguments?.getString("itemId") ?: ""
                    DetailScreen(
                        itemId = itemId,
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
            }
        }
    }
    
    @Composable
    fun HomeScreen(onNavigateToSearch: () -> Unit) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Home Screen")
        }
    }
    
    @Composable
    fun SearchScreen() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Search Screen")
        }
    }
    
    @Composable
    fun ProfileScreen() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Profile Screen")
        }
    }
    
    @Composable
    fun CartScreen(onNavigateToCheckout: () -> Unit) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Button(onClick = onNavigateToCheckout) {
                androidx.compose.material3.Text("Checkout")
            }
        }
    }
    
    @Composable
    fun CheckoutScreen(onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Checkout")
        }
    }
    
    @Composable
    fun DetailScreen(itemId: String, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Detail: ${itemId}")
        }
    }
}
```

## Section 4: State Management and Badge Updates

Managing bottom navigation state and badge updates.

```kotlin
/**
 * Bottom Navigation State Management
 * 
 * Handling:
 * - Badge updates
 * - State preservation
 * - Dynamic menu items
 * - Cart badges
 */
class BottomNavState {
    
    // ViewModel for managing badge state
    class CartViewModel : androidx.lifecycle.ViewModel() {
        
        private val _cartCount = androidx.lifecycle.MutableLiveData(0)
        val cartCount: androidx.lifecycle.LiveData<Int> = _cartCount
        
        fun addToCart() {
            _cartCount.value = (_cartCount.value ?: 0) + 1
        }
        
        fun removeFromCart() {
            _cartCount.value = maxOf(0, (_cartCount.value ?: 0) - 1)
        }
        
        fun clearCart() {
            _cartCount.value = 0
        }
    }
    
    // Compose badge with state
    @Composable
    fun BottomNavWithBadge(
        viewModel: CartViewModel
    ) {
        val cartCount by viewModel.cartCount.observeAsState(0)
        
        val navController = androidx.navigation.compose.rememberNavController()
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        androidx.compose.material3.Scaffold(
            bottomBar = {
                androidx.compose.material3.NavigationBar {
                    // Cart item with badge
                    androidx.compose.material3.NavigationBarItem(
                        selected = currentRoute == "cart",
                        onClick = { navController.navigate("cart") },
                        icon = {
                            androidx.compose.material3.Icon(
                                imageVector = androidx.compose.material.icons.Icons.Default.ShoppingCart,
                                contentDescription = "Cart"
                            )
                        },
                        label = { androidx.compose.material3.Text("Cart") },
                        // Badge when items in cart
                        badge = {
                            if (cartCount > 0) {
                                androidx.compose.material3.BadgedBox(
                                    badge = {
                                        androidx.compose.material3.Badge {
                                            androidx.compose.material3.Text(cartCount.toString())
                                        }
                                    }
                                ) {
                                    // Icon is wrapped by badge
                                }
                            }
                        }
                    )
                }
            }
        ) { padding ->
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = "home",
                modifier = Modifier.padding(padding)
            ) {
                androidx.navigation.compose.composable("home") {
                    HomeScreen(onAddToCart = { viewModel.addToCart() })
                }
                androidx.navigation.compose.composable("cart") {
                    CartScreen(onRemoveItem = { viewModel.removeFromCart() })
                }
            }
        }
    }
    
    // Dynamic bottom navigation
    @Composable
    fun DynamicBottomNav(
        showAdmin: Boolean
    ) {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Dynamic items based on user role
        val navItems = buildList {
            add(BottomNavItem("home", "Home", androidx.compose.material.icons.Icons.Default.Home))
            add(BottomNavItem("search", "Search", androidx.compose.material.icons.Icons.Default.Search))
            add(BottomNavItem("profile", "Profile", androidx.compose.material.icons.Icons.Default.Person))
            if (showAdmin) {
                add(BottomNavItem("admin", "Admin", androidx.compose.material.icons.Icons.Default.Settings))
            }
        }
        
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        androidx.compose.material3.Scaffold(
            bottomBar = {
                androidx.compose.material3.NavigationBar {
                    navItems.forEach { item ->
                        androidx.compose.material3.NavigationBarItem(
                            selected = currentRoute == item.route,
                            onClick = { navController.navigate(item.route) },
                            icon = { androidx.compose.material3.Icon(item.icon, item.title) },
                            label = { androidx.compose.material3.Text(item.title) }
                        )
                    }
                }
            }
        ) { padding ->
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = "home",
                modifier = Modifier.padding(padding)
            ) {
                navItems.forEach { item ->
                    androidx.navigation.compose.composable(item.route) {
                        androidx.compose.material3.Text(item.title)
                    }
                }
            }
        }
    }
    
    data class BottomNavItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    )
    
    @Composable
    fun HomeScreen(onAddToCart: () -> Unit) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Button(onClick = onAddToCart) {
                androidx.compose.material3.Text("Add to Cart")
            }
        }
    }
    
    @Composable
    fun CartScreen(onRemoveItem: () -> Unit) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Button(onClick = onRemoveItem) {
                androidx.compose.material3.Text("Remove Item")
            }
        }
    }
}
```

## Example: Complete Bottom Navigation App

Full implementation with all bottom navigation features.

```kotlin
/**
 * Complete Bottom Navigation App
 * 
 * Full implementation showing:
 * - Multiple tabs
 * - State preservation
 * - Nested navigation
 * - Badge updates
 */
class CompleteBottomNavigation {
    
    // Routes
    object Routes {
        const val HOME = "home"
        const val SEARCH = "search"
        const val CART = "cart"
        const val PROFILE = "profile"
        const val PRODUCT_DETAIL = "product/{productId}"
        const val CHECKOUT = "checkout"
    }
    
    // Bottom nav items
    enum class BottomNavItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    ) {
        HOME(Routes.HOME, "Home", androidx.compose.material.icons.Icons.Default.Home),
        SEARCH(Routes.SEARCH, "Search", androidx.compose.material.icons.Icons.Default.Search),
        CART(Routes.CART, "Cart", androidx.compose.material.icons.Icons.Default.ShoppingCart),
        PROFILE(Routes.PROFILE, "Profile", androidx.compose.material.icons.Icons.Default.Person)
    }
    
    // Cart state (simplified)
    @Composable
    fun rememberCartCount(): androidx.compose.runtime.State<Int> {
        return androidx.compose.runtime.remember { androidx.compose.runtime.mutableIntStateOf(0) }
    }
    
    // Main app
    @Composable
    fun MainApp() {
        val navController = androidx.navigation.compose.rememberNavController()
        val cartCount by rememberCartCount()
        
        // Current route
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        // Bottom nav routes
        val bottomNavRoutes = BottomNavItem.entries.map { it.route }
        
        // Show bottom bar only for bottom nav destinations
        val showBottomBar = currentRoute in bottomNavRoutes
        
        androidx.compose.material3.Scaffold(
            bottomBar = {
                if (showBottomBar) {
                    androidx.compose.material3.NavigationBar {
                        BottomNavItem.entries.forEach { item ->
                            val selected = currentRoute == item.route
                            
                            androidx.compose.material3.NavigationBarItem(
                                selected = selected,
                                onClick = {
                                    if (!selected) {
                                        navController.navigate(item.route) {
                                            popUpTo(navController.graph.startDestinationId) {
                                                saveState = true
                                            }
                                            launchSingleTop = true
                                            restoreState = true
                                        }
                                    }
                                },
                                icon = {
                                    androidx.compose.material3.Icon(
                                        item.icon,
                                        contentDescription = item.title
                                    )
                                },
                                label = { androidx.compose.material3.Text(item.title) },
                                badge = {
                                    if (item == BottomNavItem.CART && cartCount > 0) {
                                        androidx.compose.material3.Badge {
                                            Text(cartCount.toString())
                                        }
                                    }
                                }
                            )
                        }
                    }
                }
            }
        ) { paddingValues ->
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = BottomNavItem.HOME.route,
                modifier = Modifier.padding(paddingValues)
            ) {
                // Home
                androidx.navigation.compose.composable(BottomNavItem.HOME.route) {
                    HomeScreen(
                        onProductClick = { productId ->
                            navController.navigate("product/${productId}")
                        }
                    )
                }
                
                // Search
                androidx.navigation.compose.composable(BottomNavItem.SEARCH.route) {
                    SearchScreen(
                        onProductClick = { productId ->
                            navController.navigate("product/${productId}")
                        }
                    )
                }
                
                // Cart
                androidx.navigation.compose.composable(BottomNavItem.CART.route) {
                    CartScreen(
                        onCheckout = { navController.navigate(Routes.CHECKOUT) },
                        onProductClick = { productId ->
                            navController.navigate("product/${productId}")
                        }
                    )
                }
                
                // Profile
                androidx.navigation.compose.composable(BottomNavItem.PROFILE.route) {
                    ProfileScreen()
                }
                
                // Product detail (not in bottom nav)
                androidx.navigation.compose.composable(Routes.PRODUCT_DETAIL) { backStack ->
                    val productId = backStack.arguments?.getString("productId") ?: ""
                    ProductDetailScreen(
                        productId = productId,
                        onNavigateBack = { navController.popBackStack() },
                        onAddToCart = { /* Add to cart */ }
                    )
                }
                
                // Checkout (not in bottom nav)
                androidx.navigation.compose.composable(Routes.CHECKOUT) {
                    CheckoutScreen(
                        onComplete = {
                            // Navigate to home and clear cart stack
                            navController.navigate(BottomNavItem.HOME.route) {
                                popUpTo(BottomNavItem.HOME.route) { inclusive = true }
                            }
                        },
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
            }
        }
    }
    
    @Composable
    fun HomeScreen(onProductClick: (String) -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally
        ) {
            androidx.compose.material3.Text("Home", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = { onProductClick("1") }) {
                androidx.compose.material3.Text("View Product 1")
            }
        }
    }
    
    @Composable
    fun SearchScreen(onProductClick: (String) -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Search", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = { onProductClick("2") }) {
                androidx.compose.material3.Text("View Product 2")
            }
        }
    }
    
    @Composable
    fun CartScreen(onCheckout: () -> Unit, onProductClick: (String) -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Cart", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = onCheckout) {
                androidx.compose.material3.Text("Proceed to Checkout")
            }
        }
    }
    
    @Composable
    fun ProfileScreen() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Profile", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
        }
    }
    
    @Composable
    fun ProductDetailScreen(
        productId: String,
        onNavigateBack: () -> Unit,
        onAddToCart: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Product: ${productId}")
            androidx.compose.material3.Button(onClick = onAddToCart) { androidx.compose.material3.Text("Add to Cart") }
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun CheckoutScreen(onComplete: () -> Unit, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Checkout")
            androidx.compose.material3.Button(onClick = onComplete) { androidx.compose.material3.Text("Complete Order") }
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
}
```

## Output Statement Results

**Bottom Navigation Components:**
- BottomNavigationView: Material component for XML
- NavigationBar: Material3 composable
- NavigationBarItem: Individual items
- setupWithNavController: Integration method

**State Management:**
- Badge: Item count display
- LiveData/StateFlow: Badge updates
- NavController: Route observation
- Reselection handling: Scroll to top

**Navigation Patterns:**
- Separate back stack per tab
- State preservation with saveState
- Launch single top
- Restore state on return

**Best Practices:**
- 3-5 items maximum
- Icon + label for clarity
- Hide for detail screens
- Preserve tab state

## Cross-References

- See: [01_Jetpack_Navigation_Basics.md](../01_Navigation_Architecture/01_Jetpack_Navigation_Basics.md)
- See: [02_Navigation_Compose.md](../01_Navigation_Architecture/02_Navigation_Compose.md)
- See: [03_Tab_Navigation.md](./03_Tab_Navigation.md)
- See: [04_Flow_Navigation.md](./04_Flow_Navigation.md)

## End of Bottom Navigation Guide