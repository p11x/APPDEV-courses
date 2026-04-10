# Navigation Drawer

## Learning Objectives

1. Understanding navigation drawer patterns
2. Implementing DrawerLayout with Navigation component
3. Creating drawer with Navigation Compose
4. Managing drawer state and gestures
5. Customizing drawer appearance
6. Handling drawer with bottom navigation
7. Implementing multi-pane layouts
8. Creating collapsible drawer sections

## Section 1: Navigation Drawer Overview

Navigation drawer (DrawerLayout) provides a slide-out panel for primary navigation. It's ideal for apps with many destinations or hierarchical navigation.

```kotlin
/**
 * Navigation Drawer Overview
 * 
 * Navigation Drawer is used when:
 * - Many navigation destinations (5+)
 * - Hierarchical navigation structure
 * - Secondary actions and settings
 * - User profile sections
 * 
 * Implementation approaches:
 * - XML DrawerLayout with NavigationView
 * - Navigation Compose drawer
 * - Material3 DrawerSheet
 */
object NavigationDrawerOverview {
    
    // Drawer structure
    object DrawerStructure {
        // Header section (optional)
        const val HEADER = "header"
        
        // Menu items (primary navigation)
        const val MENU_MAIN = "main"
        
        // Subheader for grouping
        const val SUBHEADER = "subheader"
        
        // Secondary menu (settings, etc.)
        const val MENU_SECONDARY = "secondary"
        
        // Footer section
        const val FOOTER = "footer"
    }
    
    // XML drawer layout
    fun getXmlLayout(): String {
        return """
<!-- activity_main.xml -->
<androidx.drawerlayout.widget.DrawerLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/drawer_layout"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:fitsSystemWindows="true">
    
    <!-- Main content (host fragment) -->
    <androidx.fragment.app.FragmentContainerView
        android:id="@+id/nav_host_fragment"
        android:name="androidx.navigation.fragment.NavHostFragment"
        android:layout_width="match_parent"
        android:layout_height="match_parent"
        app:defaultNavHost="true"
        app:navGraph="@navigation/nav_graph" />
    
    <!-- Navigation drawer -->
    <com.google.android.material.navigation.NavigationView
        android:id="@+id/navigation_view"
        android:layout_width="wrap_content"
        android:layout_height="match_parent"
        android:layout_gravity="start"
        android:fitsSystemWindows="true"
        app:headerLayout="@layout/nav_header"
        app:menu="@menu/nav_menu" />
    
</androidx.drawerlayout.widget.DrawerLayout>
        """.trimIndent()
    }
    
    // Header layout
    fun getHeaderLayout(): String {
        return """
<!-- res/layout/nav_header.xml -->
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="180dp"
    android:background="@color/primary"
    android:gravity="bottom"
    android:orientation="vertical"
    android:padding="16dp">
    
    <ImageView
        android:id="@+id/profile_image"
        android:layout_width="64dp"
        android:layout_height="64dp"
        android:src="@drawable/ic_profile" />
    
    <TextView
        android:id="@+id/user_name"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:text="John Doe"
        android:textColor="@android:color/white"
        android:textSize="18sp"
        android:textStyle="bold" />
    
    <TextView
        android:id="@+id/user_email"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:text="john@example.com"
        android:textColor="@android:color/white"
        android:textSize="14sp" />
    
</LinearLayout>
        """.trimIndent()
    }
    
    // Menu resource
    fun getMenuXml(): String {
        return """
<!-- res/menu/drawer_menu.xml -->
<menu xmlns:android="http://schemas.android.com/apk/res/android">
    
    <!-- Main navigation items -->
    <group android:checkableBehavior="single">
        
        <item
            android:id="@+id/homeFragment"
            android:icon="@drawable/ic_home"
            android:title="Home" />
        
        <item
            android:id="@+id/productsFragment"
            android:icon="@drawable/ic_products"
            android:title="Products" />
        
        <item
            android:id="@+id/ordersFragment"
            android:icon="@drawable/ic_orders"
            android:title="My Orders" />
        
    </group>
    
    <!-- Subheader -->
    <item android:title="Account">
        <menu>
            <group android:checkableBehavior="single">
                
                <item
                    android:id="@+id/profileFragment"
                    android:icon="@drawable/ic_profile"
                    android:title="Profile" />
                
                <item
                    android:id="@+id/wishlistFragment"
                    android:icon="@drawable/ic_wishlist"
                    android:title="Wishlist" />
                
                <item
                    android:id="@+id/cartFragment"
                    android:icon="@drawable/ic_cart"
                    android:title="Cart" />
                
            </group>
        </menu>
    </item>
    
    <!-- Secondary items -->
    <item android:title="Settings">
        <menu>
            
            <item
                android:id="@+id/settingsFragment"
                android:icon="@drawable/ic_settings"
                android:title="Settings" />
            
            <item
                android:id="@+id/helpFragment"
                android:icon="@drawable/ic_help"
                android:title="Help &amp; Support" />
            
            <item
                android:id="@+id/logoutFragment"
                android:icon="@drawable/ic_logout"
                android:title="Logout" />
            
        </menu>
    </item>
    
</menu>
        """.trimIndent()
    }
}
```

## Section 2: DrawerLayout with Navigation Component

Integrating DrawerLayout with Navigation component for seamless drawer navigation.

```kotlin
/**
 * DrawerLayout with Navigation Component
 * 
 * NavigationView integration:
 * - Automatic menu-to-destination mapping
 * - Checkable item behavior
 * - Header management
 * - Proper gesture handling
 */
class DrawerNavigationComponent {
    
    // Activity setup
    class MainActivity : androidx.appcompat.app.AppCompatActivity() {
        
        private lateinit var drawerLayout: androidx.drawerlayout.widget.DrawerLayout
        private lateinit var navController: androidx.navigation.NavController
        private lateinit var navigationView: com.google.android.material.navigation.NavigationView
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(R.layout.activity_main)
            
            // Get views
            drawerLayout = findViewById(R.id.drawer_layout)
            navigationView = findViewById(R.id.navigation_view)
            
            // Setup toolbar
            val toolbar = findViewById<androidx.appcompat.widget.Toolbar>(R.id.toolbar)
            setSupportActionBar(toolbar)
            
            // Setup Navigation
            val navHostFragment = supportFragmentManager
                .findFragmentById(R.id.nav_host_fragment) as androidx.navigation.fragment.NavHostFragment
            navController = navHostFragment.navController
            
            // Connect toolbar with navigation
            // This enables hamburger icon for drawer
            androidx.navigation.ui.setupActionBarWithNavController(
                this,
                navController,
                drawerLayout
            )
            
            // Connect NavigationView with NavController
            // This handles menu item selection
            navigationView.setupWithNavController(navController)
            
            // Optional: Handle navigation item selection
            navigationView.setNavigationItemSelectedListener { menuItem ->
                // Handle custom actions if needed
                when (menuItem.itemId) {
                    R.id.logoutFragment -> {
                        // Handle logout
                        true
                    }
                    else -> {
                        // Let Navigation component handle it
                        // Return false to let setupWithNavController handle it
                        false
                    }
                }
            }
            
            // Optional: Listen for destination changes
            navController.addOnDestinationChangedListener { _, destination, _ ->
                // Close drawer when destination changes
                drawerLayout.closeDrawers()
                
                // Update title based on destination
                supportActionBar?.title = destination.label
            }
            
            // Setup header click listener
            val headerView = navigationView.getHeaderView(0)
            headerView.findViewById<android.view.View>(R.id.profile_image).setOnClickListener {
                // Navigate to profile
                navController.navigate(R.id.profileFragment)
                drawerLayout.closeDrawers()
            }
        }
        
        // Handle up navigation (drawer icon -> back -> drawer)
        override fun onSupportNavigateUp(): Boolean {
            // This handles:
            // 1. Up arrow (back)
            // 2. Hamburger menu (toggle drawer)
            return androidx.navigation.ui.NavigationUI.navigateUp(
                navController,
                drawerLayout
            ) || super.onSupportNavigateUp()
        }
        
        // Handle back press for drawer
        override fun onBackPressed() {
            if (drawerLayout.isDrawerOpen(androidx.core.view.GravityCompat.START)) {
                drawerLayout.closeDrawer(androidx.core.view.GravityCompat.START)
            } else {
                super.onBackPressed()
            }
        }
    }
    
    // Fragment can access drawer
    class ProductsFragment : androidx.fragment.app.Fragment() {
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Get drawer layout
            val drawerLayout = requireActivity()
                .findViewById<androidx.drawerlayout.widget.DrawerLayout>(R.id.drawer_layout)
            
            // Open drawer programmatically
            view.findViewById<android.view.View>(R.id.filter_button).setOnClickListener {
                drawerLayout.openDrawer(androidx.core.view.GravityCompat.START)
            }
            
            // Access navigation view
            val navView = requireActivity()
                .findViewById<com.google.android.material.navigation.NavigationView>(R.id.navigation_view)
            
            // Update menu items
            val menu = navView.menu
            menu.findItem(R.id.productsFragment)?.isChecked = true
        }
    }
}
```

## Section 3: Navigation Compose Drawer

Implementing drawer navigation with Navigation Compose.

```kotlin
/**
 * Navigation Compose Drawer
 * 
 * Drawer in Compose:
 * - ModalNavigationDrawer
 * - DrawerSheet
 * - PermanentDrawer
 */
class NavigationComposeDrawer {
    
    // Drawer navigation items
    data class DrawerItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    )
    
    // Main drawer scaffold
    @Composable
    fun DrawerScaffold() {
        val navController = androidx.navigation.compose.rememberNavController()
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        // Drawer items
        val mainItems = listOf(
            DrawerItem("home", "Home", androidx.compose.material.icons.Icons.Default.Home),
            DrawerItem("products", "Products", androidx.compose.material.icons.Icons.Default.ShoppingBag),
            DrawerItem("orders", "Orders", androidx.compose.material.icons.Icons.Default.Receipt)
        )
        
        val secondaryItems = listOf(
            DrawerItem("profile", "Profile", androidx.compose.material.icons.Icons.Default.Person),
            DrawerItem("wishlist", "Wishlist", androidx.compose.material.icons.Icons.Default.Favorite),
            DrawerItem("cart", "Cart", androidx.compose.material.icons.Icons.Default.ShoppingCart)
        )
        
        val settingsItems = listOf(
            DrawerItem("settings", "Settings", androidx.compose.material.icons.Icons.Default.Settings),
            DrawerItem("help", "Help", androidx.compose.material.icons.Icons.Default.Help)
        )
        
        // Modal drawer state
        val drawerState = androidx.compose.material3.rememberDrawerState(initialValue = androidx.compose.material3.DrawerValue.Closed)
        
        // Modal drawer with scaffold
        androidx.compose.material3.ModalNavigationDrawer(
            drawerState = drawerState,
            drawerContent = {
                // Drawer content
                androidx.compose.material3.ModalDrawerSheet {
                    // Header
                    androidx.compose.foundation.layout.Column(
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(16.dp)
                    ) {
                        // Profile image placeholder
                        androidx.compose.foundation.layout.size(64.dp)
                        androidx.compose.foundation.layout.Box(
                            modifier = Modifier
                                .background(androidx.compose.material3.MaterialTheme.colorScheme.primary)
                        )
                        androidx.compose.material3.Text(
                            "John Doe",
                            style = androidx.compose.material3.MaterialTheme.typography.titleMedium
                        )
                        androidx.compose.material3.Text(
                            "john@example.com",
                            style = androidx.compose.material3.MaterialTheme.typography.bodyMedium,
                            color = androidx.compose.material3.MaterialTheme.colorScheme.onSurfaceVariant
                        )
                    }
                    
                    androidx.compose.material3.HorizontalDivider()
                    
                    // Main items
                    mainItems.forEach { item ->
                        androidx.compose.material3.NavigationDrawerItem(
                            icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                            label = { androidx.compose.material3.Text(item.title) },
                            selected = currentRoute == item.route,
                            onClick = {
                                navController.navigate(item.route)
                                androidx.compose.material3.LaunchedEffect(Unit) {
                                    drawerState.close()
                                }
                            },
                            modifier = Modifier.padding(androidx.compose.foundation.layout.HorizontalPadding(12.dp))
                        )
                    }
                    
                    androidx.compose.material3.HorizontalDivider(modifier = Modifier.padding(vertical = 8.dp))
                    
                    // Account subheader
                    androidx.compose.material3.Text(
                        "Account",
                        style = androidx.compose.material3.MaterialTheme.typography.labelMedium,
                        modifier = Modifier.padding(start = 16.dp, top = 8.dp)
                    )
                    
                    secondaryItems.forEach { item ->
                        androidx.compose.material3.NavigationDrawerItem(
                            icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                            label = { androidx.compose.material3.Text(item.title) },
                            selected = currentRoute == item.route,
                            onClick = {
                                navController.navigate(item.route)
                                androidx.compose.material3.LaunchedEffect(Unit) {
                                    drawerState.close()
                                }
                            },
                            modifier = Modifier.padding(androidx.compose.foundation.layout.HorizontalPadding(12.dp))
                        )
                    }
                    
                    androidx.compose.material3.HorizontalDivider(modifier = Modifier.padding(vertical = 8.dp))
                    
                    // Settings subheader
                    settingsItems.forEach { item ->
                        androidx.compose.material3.NavigationDrawerItem(
                            icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                            label = { androidx.compose.material3.Text(item.title) },
                            selected = currentRoute == item.route,
                            onClick = {
                                navController.navigate(item.route)
                                androidx.compose.material3.LaunchedEffect(Unit) {
                                    drawerState.close()
                                }
                            },
                            modifier = Modifier.padding(androidx.compose.foundation.layout.HorizontalPadding(12.dp))
                        )
                    }
                }
            },
            content = {
                // Main content scaffold
                androidx.compose.material3.Scaffold(
                    topBar = {
                        androidx.compose.material3.TopAppBar(
                            title = { androidx.compose.material3.Text(currentRoute ?: "App") },
                            navigationIcon = {
                                // Hamburger menu icon
                                androidx.compose.material3.IconButton(
                                    onClick = {
                                        androidx.compose.material3.LaunchedEffect(Unit) {
                                            drawerState.open()
                                        }
                                    }
                                ) {
                                    androidx.compose.material3.Icon(
                                        androidx.compose.material.icons.Icons.Default.Menu,
                                        contentDescription = "Menu"
                                    )
                                }
                            }
                        )
                    }
                ) { paddingValues ->
                    androidx.navigation.compose.NavHost(
                        navController = navController,
                        startDestination = "home",
                        modifier = Modifier.padding(paddingValues)
                    ) {
                        // Add composables for all routes
                        mainItems.forEach { item ->
                            androidx.navigation.compose.composable(item.route) {
                                ScreenContent(title = item.title)
                            }
                        }
                        secondaryItems.forEach { item ->
                            androidx.navigation.compose.composable(item.route) {
                                ScreenContent(title = item.title)
                            }
                        }
                        settingsItems.forEach { item ->
                            androidx.navigation.compose.composable(item.route) {
                                ScreenContent(title = item.title)
                            }
                        }
                    }
                }
            }
        )
    }
    
    // Drawer sheet variant
    @Composable
    fun DrawerSheetScaffold() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.compose.material3.Scaffold(
            topBar = {
                androidx.compose.material3.TopAppBar(
                    title = { androidx.compose.material3.Text("App") }
                )
            }
        ) { paddingValues ->
            androidx.compose.material3.PermanentNavigationDrawer(
                drawerContent = {
                    androidx.compose.material3.PermanentDrawerSheet {
                        androidx.compose.material3.Text("Navigation", modifier = Modifier.padding(16.dp))
                    }
                },
                modifier = Modifier.padding(paddingValues)
            ) {
                androidx.navigation.compose.NavHost(
                    navController = navController,
                    startDestination = "home"
                ) {
                    androidx.navigation.compose.composable("home") {
                        androidx.compose.material3.Text("Home")
                    }
                }
            }
        }
    }
    
    @Composable
    fun ScreenContent(title: String) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text(title, style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
        }
    }
}
```

## Section 4: Drawer with Bottom Navigation

Combining drawer navigation with bottom navigation for complex navigation patterns.

```kotlin
/**
 * Drawer with Bottom Navigation
 * 
 * Combined patterns:
 * - Bottom navigation for primary sections
 * - Drawer for secondary navigation
 * - Common in content-heavy apps
 */
class DrawerWithBottomNav {
    
    // Combined navigation items
    @Composable
    fun CombinedNavigation() {
        val navController = androidx.navigation.compose.rememberNavController()
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        // Drawer state
        val drawerState = androidx.compose.material3.rememberDrawerState(initialValue = androidx.compose.material3.DrawerValue.Closed)
        
        // Bottom nav items
        val bottomNavItems = listOf(
            DrawerItem("home", "Home", androidx.compose.material.icons.Icons.Default.Home),
            DrawerItem("products", "Products", androidx.compose.material.icons.Icons.Default.ShoppingBag),
            DrawerItem("orders", "Orders", androidx.compose.material.icons.Icons.Default.Receipt)
        )
        
        // Drawer items (additional)
        val drawerItems = listOf(
            DrawerItem("profile", "Profile", androidx.compose.material.icons.Icons.Default.Person),
            DrawerItem("settings", "Settings", androidx.compose.material.icons.Icons.Default.Settings),
            DrawerItem("help", "Help", androidx.compose.material.icons.Icons.Default.Help)
        )
        
        // Routes that show bottom nav
        val bottomNavRoutes = bottomNavItems.map { it.route }
        
        val showBottomBar = currentRoute in bottomNavRoutes
        
        androidx.compose.material3.ModalNavigationDrawer(
            drawerState = drawerState,
            drawerContent = {
                androidx.compose.material3.ModalDrawerSheet {
                    // Header
                    androidx.compose.foundation.layout.Column(
                        modifier = Modifier.fillMaxWidth().padding(16.dp)
                    ) {
                        androidx.compose.material3.Text(
                            "App Name",
                            style = androidx.compose.material3.MaterialTheme.typography.titleLarge
                        )
                        androidx.compose.material3.Text(
                            "menu",
                            style = androidx.compose.material3.MaterialTheme.typography.bodyMedium
                        )
                    }
                    
                    androidx.compose.material3.HorizontalDivider()
                    
                    // Drawer items
                    drawerItems.forEach { item ->
                        androidx.compose.material3.NavigationDrawerItem(
                            icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                            label = { androidx.compose.material3.Text(item.title) },
                            selected = currentRoute == item.route,
                            onClick = {
                                navController.navigate(item.route)
                                androidx.compose.material3.LaunchedEffect(Unit) {
                                    drawerState.close()
                                }
                            }
                        )
                    }
                }
            },
            content = {
                androidx.compose.material3.Scaffold(
                    topBar = {
                        androidx.compose.material3.TopAppBar(
                            title = { androidx.compose.material3.Text(currentRoute ?: "App") },
                            navigationIcon = {
                                androidx.compose.material3.IconButton(
                                    onClick = { androidx.compose.material3.LaunchedEffect(Unit) { drawerState.open() } }
                                ) {
                                    androidx.compose.material3.Icon(
                                        androidx.compose.material.icons.Icons.Default.Menu,
                                        contentDescription = "Menu"
                                    )
                                }
                            }
                        )
                    },
                    bottomBar = {
                        if (showBottomBar) {
                            androidx.compose.material3.NavigationBar {
                                bottomNavItems.forEach { item ->
                                    androidx.compose.material3.NavigationBarItem(
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
                                        },
                                        icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                                        label = { androidx.compose.material3.Text(item.title) }
                                    )
                                }
                            }
                        }
                    }
                ) { paddingValues ->
                    // All destinations including bottom nav and drawer items
                    androidx.navigation.compose.NavHost(
                        navController = navController,
                        startDestination = "home",
                        modifier = Modifier.padding(paddingValues)
                    ) {
                        // Bottom nav destinations
                        bottomNavItems.forEach { item ->
                            androidx.navigation.compose.composable(item.route) {
                                CombinedContent(title = item.title)
                            }
                        }
                        
                        // Additional drawer destinations
                        drawerItems.forEach { item ->
                            androidx.navigation.compose.composable(item.route) {
                                CombinedContent(title = item.title)
                            }
                        }
                    }
                }
            }
        )
    }
    
    @Composable
    fun CombinedContent(title: String) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text(title, style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
        }
    }
}
```

## Example: Complete Navigation Drawer App

Full implementation with all drawer features.

```kotlin
/**
 * Complete Navigation Drawer App
 * 
 * Full implementation showing:
 * - Complete drawer structure
 * - Navigation integration
 * - Header with user info
 * - Grouped menu items
 */
class CompleteDrawerApp {
    
    // Routes
    object Routes {
        const val HOME = "home"
        const val PRODUCTS = "products"
        const val ORDERS = "orders"
        const val PROFILE = "profile"
        const val WISHLIST = "wishlist"
        const val CART = "cart"
        const val SETTINGS = "settings"
        const val HELP = "help"
    }
    
    // Drawer items
    data class NavItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    )
    
    // Main app with drawer
    @Composable
    fun MainApp() {
        val navController = androidx.navigation.compose.rememberNavController()
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        // Drawer state
        val drawerState = androidx.compose.material3.rememberDrawerState(initialValue = androidx.compose.material3.DrawerValue.Closed)
        
        // Primary navigation items
        val primaryItems = listOf(
            NavItem(Routes.HOME, "Home", androidx.compose.material.icons.Icons.Default.Home),
            NavItem(Routes.PRODUCTS, "Products", androidx.compose.material.icons.Icons.Default.ShoppingBag),
            NavItem(Routes.ORDERS, "Orders", androidx.compose.material.icons.Icons.Default.Receipt)
        )
        
        // Account items
        val accountItems = listOf(
            NavItem(Routes.PROFILE, "Profile", androidx.compose.material.icons.Icons.Default.Person),
            NavItem(Routes.WISHLIST, "Wishlist", androidx.compose.material.icons.Icons.Default.Favorite),
            NavItem(Routes.CART, "Cart", androidx.compose.material.icons.Icons.Default.ShoppingCart)
        )
        
        // Settings items
        val settingsItems = listOf(
            NavItem(Routes.SETTINGS, "Settings", androidx.compose.material.icons.Icons.Default.Settings),
            NavItem(Routes.HELP, "Help & Support", androidx.compose.material.icons.Icons.Default.Help)
        )
        
        // Modal navigation drawer
        androidx.compose.material3.ModalNavigationDrawer(
            drawerState = drawerState,
            drawerContent = {
                // Drawer sheet
                androidx.compose.material3.ModalDrawerSheet(
                    modifier = Modifier.width(300.dp)
                ) {
                    // Header section
                    androidx.compose.foundation.layout.Column(
                        modifier = Modifier
                            .fillMaxWidth()
                            .background(androidx.compose.material3.MaterialTheme.colorScheme.primaryContainer)
                            .padding(24.dp)
                    ) {
                        androidx.compose.material3.Text(
                            "My App",
                            style = androidx.compose.material3.MaterialTheme.typography.headlineSmall,
                            color = androidx.compose.material3.MaterialTheme.colorScheme.onPrimaryContainer
                        )
                        androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
                        androidx.compose.material3.Text(
                            "user@example.com",
                            style = androidx.compose.material3.MaterialTheme.typography.bodyMedium,
                            color = androidx.compose.material3.MaterialTheme.colorScheme.onPrimaryContainer.copy(alpha = 0.7f)
                        )
                    }
                    
                    // Primary navigation
                    androidx.compose.foundation.layout.Column(
                        modifier = Modifier.padding(horizontal = 12.dp)
                    ) {
                        primaryItems.forEach { item ->
                            androidx.compose.material3.NavigationDrawerItem(
                                icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                                label = { androidx.compose.material3.Text(item.title) },
                                selected = currentRoute == item.route,
                                onClick = {
                                    if (currentRoute != item.route) {
                                        navController.navigate(item.route)
                                    }
                                    androidx.compose.material3.LaunchedEffect(Unit) {
                                        drawerState.close()
                                    }
                                },
                                modifier = Modifier.padding(vertical = 4.dp)
                            )
                        }
                    }
                    
                    androidx.compose.material3.HorizontalDivider(modifier = Modifier.padding(vertical = 12.dp))
                    
                    // Account section
                    androidx.compose.material3.Text(
                        "Account",
                        style = androidx.compose.material3.MaterialTheme.typography.labelLarge,
                        modifier = Modifier.padding(start = 16.dp, bottom = 8.dp)
                    )
                    
                    androidx.compose.foundation.layout.Column(
                        modifier = Modifier.padding(horizontal = 12.dp)
                    ) {
                        accountItems.forEach { item ->
                            androidx.compose.material3.NavigationDrawerItem(
                                icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                                label = { androidx.compose.material3.Text(item.title) },
                                selected = currentRoute == item.route,
                                onClick = {
                                    if (currentRoute != item.route) {
                                        navController.navigate(item.route)
                                    }
                                    androidx.compose.material3.LaunchedEffect(Unit) {
                                        drawerState.close()
                                    }
                                },
                                modifier = Modifier.padding(vertical = 4.dp)
                            )
                        }
                    }
                    
                    androidx.compose.material3.HorizontalDivider(modifier = Modifier.padding(vertical = 12.dp))
                    
                    // Settings section
                    androidx.compose.foundation.layout.Column(
                        modifier = Modifier.padding(horizontal = 12.dp)
                    ) {
                        settingsItems.forEach { item ->
                            androidx.compose.material3.NavigationDrawerItem(
                                icon = { androidx.compose.material3.Icon(item.icon, contentDescription = item.title) },
                                label = { androidx.compose.material3.Text(item.title) },
                                selected = currentRoute == item.route,
                                onClick = {
                                    if (currentRoute != item.route) {
                                        navController.navigate(item.route)
                                    }
                                    androidx.compose.material3.LaunchedEffect(Unit) {
                                        drawerState.close()
                                    }
                                },
                                modifier = Modifier.padding(vertical = 4.dp)
                            )
                        }
                    }
                }
            },
            content = {
                // Main scaffold with toolbar
                androidx.compose.material3.Scaffold(
                    topBar = {
                        androidx.compose.material3.TopAppBar(
                            title = { 
                                androidx.compose.material3.Text(
                                    currentRoute?.replaceFirstChar { it.uppercase() } ?: "Home"
                                ) 
                            },
                            navigationIcon = {
                                androidx.compose.material3.IconButton(
                                    onClick = { 
                                        androidx.compose.material3.LaunchedEffect(Unit) { 
                                            drawerState.open() 
                                        } 
                                    }
                                ) {
                                    androidx.compose.material3.Icon(
                                        androidx.compose.material.icons.Icons.Default.Menu,
                                        contentDescription = "Open menu"
                                    )
                                }
                            },
                            colors = androidx.compose.material3.TopAppBarDefaults.topAppBarColors(
                                containerColor = androidx.compose.material3.MaterialTheme.colorScheme.surface
                            )
                        )
                    }
                ) { paddingValues ->
                    // Navigation host
                    androidx.navigation.compose.NavHost(
                        navController = navController,
                        startDestination = Routes.HOME,
                        modifier = Modifier.padding(paddingValues)
                    ) {
                        // Primary destinations
                        primaryItems.forEach { item ->
                            androidx.navigation.compose.composable(item.route) {
                                ScreenContent(title = item.title)
                            }
                        }
                        
                        // Account destinations
                        accountItems.forEach { item ->
                            androidx.navigation.compose.composable(item.route) {
                                ScreenContent(title = item.title)
                            }
                        }
                        
                        // Settings destinations
                        settingsItems.forEach { item ->
                            androidx.navigation.compose.composable(item.route) {
                                ScreenContent(title = item.title)
                            }
                        }
                    }
                }
            }
        )
    }
    
    @Composable
    fun ScreenContent(title: String) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text(
                title,
                style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
            )
        }
    }
}
```

## Output Statement Results

**Drawer Components:**
- DrawerLayout: XML container for drawer
- NavigationView: Material drawer component
- ModalNavigationDrawer: Compose drawer
- ModalDrawerSheet: Compose drawer content

**Drawer Structure:**
- Header: User info at top
- Main menu: Primary navigation
- Subheader: Section labels
- Secondary menu: Settings, help

**Navigation Integration:**
- setupWithNavController: Auto menu handling
- ActionBar toggle: Hamburger menu
- Close on navigation: Auto-close after selection
- Back press handling: Close drawer first

**Best Practices:**
- fitsSystemWindows: Proper status bar handling
- 300dp width: Standard drawer width
- Grouped items: Clear navigation hierarchy
- Header: User identification

## Cross-References

- See: [01_Bottom_Navigation.md](./01_Bottom_Navigation.md)
- See: [01_Jetpack_Navigation_Basics.md](../01_Navigation_Architecture/01_Jetpack_Navigation_Basics.md)
- See: [02_Navigation_Compose.md](../01_Navigation_Architecture/02_Navigation_Compose.md)
- See: [04_Flow_Navigation.md](./04_Flow_Navigation.md)

## End of Navigation Drawer Guide