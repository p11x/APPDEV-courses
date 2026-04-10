# Tab Navigation

## Learning Objectives

1. Understanding tab navigation patterns
2. Implementing TabLayout with ViewPager
3. Creating tabs with Navigation Compose
4. Managing tab state and selection
5. Implementing scrollable tabs
6. Creating fixed tabs with icons
7. Handling nested tab navigation
8. Customizing tab appearance

## Section 1: Tab Navigation Overview

Tab navigation provides quick access to related content views. It's ideal when content can be divided into equal-priority sections.

```kotlin
/**
 * Tab Navigation Overview
 * 
 * Tab Navigation is used when:
 * - 2-5 related content sections
 * - Equal-priority content access
 * - Horizontal content switching
 * - Categorized information
 * 
 * Implementation approaches:
 * - TabLayout with ViewPager2
 * - TabRow in Compose
 * - Custom tab implementation
 */
object TabNavigationOverview {
    
    // Tab types
    object TabTypes {
        // Fixed tabs - equal width
        const val FIXED = "fixed"
        
        // Scrollable tabs - for many tabs
        const val SCROLLABLE = "scrollable"
        
        // Tab with icons only
        const val ICON_ONLY = "icon_only"
        
        // Tab with text only
        const val TEXT_ONLY = "text_only"
        
        // Tab with icon and text
        const val ICON_TEXT = "icon_text"
    }
    
    // XML TabLayout setup
    fun getXmlLayout(): String {
        return """
<!-- activity_main.xml -->
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:orientation="vertical">
    
    <com.google.android.material.tabs.TabLayout
        android:id="@+id/tab_layout"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        app:tabMode="fixed"
        app:tabGravity="fill"
        app:tabIndicatorColor="@color/primary"
        app:tabSelectedTextColor="@color/primary"
        app:tabTextColor="@color/secondary"
        app:tabIconTint="@color/tab_icon_color"
        app:tabInlineLabel="true" />
    
    <androidx.viewpager2.widget.ViewPager2
        android:id="@+id/view_pager"
        android:layout_width="match_parent"
        android:layout_height="match_parent" />
    
</LinearLayout>
        """.trimIndent()
    }
    
    // Tab configuration
    fun getTabConfiguration(): String {
        return """
<!-- Tab modes -->
app:tabMode="fixed"       <!-- All tabs same width -->
app:tabMode="scrollable"   <!-- Horizontal scroll -->

<!-- Tab gravity -->
app:tabGravity="fill"     <!-- Equal width tabs -->
app:tabGravity="center"    <!-- Centered tabs -->

<!-- Tab indicator -->
app:tabIndicatorColor="@color/primary"
app:tabIndicatorHeight="3dp"
app:tabIndicatorFullWidth="false"

<!-- Tab text -->
app:tabTextColor="@color/tab_text"
app:tabSelectedTextColor="@color/tab_selected"
app:tabTextAppearance="@style/TabTextAppearance"

<!-- Tab icon -->
app:tabIconTint="@color/tab_icon"
app:tabInlineLabel="true"  <!-- Icon and text inline -->

<!-- Tab padding -->
app:tabPaddingStart="12dp"
app:tabPaddingEnd="12dp"
        """.trimIndent()
    }
}
```

## Section 2: TabLayout with ViewPager2

Using TabLayout with ViewPager2 for classic tab navigation with swipe between pages.

```kotlin
/**
 * TabLayout with ViewPager2 Implementation
 * 
 * ViewPager2 with TabLayout:
 * - Tab indicators sync with pages
 * - Swipe between tabs
 * - PagerAdapter for content
 * - Automatic tab selection
 */
class TabLayoutWithViewPager {
    
    // Activity setup
    class MainActivity : androidx.appcompat.app.AppCompatActivity() {
        
        private lateinit var tabLayout: com.google.android.material.tabs.TabLayout
        private lateinit var viewPager: androidx.viewpager2.widget.ViewPager2
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(R.layout.activity_main)
            
            // Get views
            tabLayout = findViewById(R.id.tab_layout)
            viewPager = findViewById(R.id.view_pager)
            
            // Setup adapter
            val adapter = ViewPagerAdapter(supportFragmentManager, lifecycle)
            viewPager.adapter = adapter
            
            // Connect TabLayout with ViewPager
            // TabLayoutMediator handles:
            // - Tab creation from adapter
            // - Tab selection sync
            // - Page scrolling sync
            androidx.tabs.TabLayoutMediator(
                tabLayout,
                viewPager
            ) { tab, position ->
                // Set tab text
                tab.text = when (position) {
                    0 -> "Home"
                    1 -> "Products"
                    2 -> "Orders"
                    3 -> "Settings"
                    else -> "Tab ${position + 1}"
                }
                
                // Optional: Set icon
                tab.setIcon(R.drawable.ic_home)
            }.attach()
            
            // Optional: Custom tab configuration
            tabLayout.tabMode = com.google.android.material.tabs.TabLayout.MODE_FIXED
            tabLayout.tabGravity = com.google.android.material.tabs.TabLayout.GRAVITY_FILL
            
            // Listen for tab selection
            tabLayout.addOnTabSelectedListener(object : com.google.android.material.tabs.TabLayout.OnTabSelectedListener {
                override fun onTabSelected(tab: com.google.android.material.tabs.TabLayout.Tab?) {
                    // Tab selected
                    val position = tab?.position ?: 0
                    println("Tab selected: ${position}")
                }
                
                override fun onTabUnselected(tab: com.google.android.material.tabs.TabLayout.Tab?) {
                    // Tab unselected
                }
                
                override fun onTabReselected(tab: com.google.android.material.tabs.TabLayout.Tab?) {
                    // Tab reselected (tap on current tab)
                    // Scroll to top or refresh
                }
            })
        }
    }
    
    // ViewPager adapter
    class ViewPagerAdapter(
        fragmentManager: androidx.fragment.app.FragmentManager,
        lifecycle: androidx.lifecycle.Lifecycle
    ) : androidx.viewpager2.adapter.FragmentStateAdapter(fragmentManager, lifecycle) {
        
        // Tab count
        override fun getItemCount(): Int = 4
        
        // Create fragment for position
        override fun createFragment(position: Int): androidx.fragment.app.Fragment {
            return when (position) {
                0 -> HomeFragment()
                1 -> ProductsFragment()
                2 -> OrdersFragment()
                3 -> SettingsFragment()
                else -> HomeFragment()
            }
        }
    }
    
    // Example fragments
    class HomeFragment : androidx.fragment.app.Fragment() {
        override fun onCreateView(
            inflater: android.view.LayoutInflater,
            container: android.view.ViewGroup?,
            savedInstanceState: android.os.Bundle?
        ): android.view.View {
            return android.widget.TextView(requireContext()).apply {
                text = "Home Content"
                setPadding(32, 32, 32, 32)
            }
        }
    }
    
    class ProductsFragment : androidx.fragment.app.Fragment() {
        override fun onCreateView(
            inflater: android.view.LayoutInflater,
            container: android.view.ViewGroup?,
            savedInstanceState: android.os.Bundle?
        ): android.view.View {
            return android.widget.TextView(requireContext()).apply {
                text = "Products Content"
                setPadding(32, 32, 32, 32)
            }
        }
    }
    
    class OrdersFragment : androidx.fragment.app.Fragment() {
        override fun onCreateView(
            inflater: android.view.LayoutInflater,
            container: android.view.ViewGroup?,
            savedInstanceState: android.os.Bundle?
        ): android.view.View {
            return android.widget.TextView(requireContext()).apply {
                text = "Orders Content"
                setPadding(32, 32, 32, 32)
            }
        }
    }
    
    class SettingsFragment : androidx.fragment.app.Fragment() {
        override fun onCreateView(
            inflater: android.view.LayoutInflater,
            container: android.view.ViewGroup?,
            savedInstanceState: android.os.Bundle?
        ): android.view.View {
            return android.widget.TextView(requireContext()).apply {
                text = "Settings Content"
                setPadding(32, 32, 32, 32)
            }
        }
    }
}
```

## Section 3: Navigation Compose Tab Navigation

Implementing tabs with Navigation Compose using TabRow and Pager.

```kotlin
/**
 * Navigation Compose Tab Navigation
 * 
 * Tab navigation in Compose:
 * - TabRow for fixed tabs
 * - ScrollableTabRow for scrollable
 * - Pager for swipe content
 * - Combined with Navigation
 */
class NavigationComposeTabs {
    
    // Define tabs
    data class TabItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    )
    
    // Fixed tabs example
    @Composable
    fun FixedTabs() {
        val pagerState = androidx.compose.foundation.pager.rememberPagerState(pageCount = { 4 })
        
        // Tab items
        val tabs = listOf(
            TabItem("home", "Home", androidx.compose.material.icons.Icons.Default.Home),
            TabItem("products", "Products", androidx.compose.material.icons.Icons.Default.ShoppingBag),
            TabItem("orders", "Orders", androidx.compose.material.icons.Icons.Default.Receipt),
            TabItem("settings", "Settings", androidx.compose.material.icons.Icons.Default.Settings)
        )
        
        // Vertical layout
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Tab Row
            androidx.compose.material3.TabRow(
                selectedTabIndex = pagerState.currentPage
            ) {
                tabs.forEachIndexed { index, tab ->
                    androidx.compose.material3.Tab(
                        selected = pagerState.currentPage == index,
                        onClick = {
                            // Animate to page
                            androidx.compose.runtime.LaunchedEffect(Unit) {
                                pagerState.animateScrollToPage(index)
                            }
                        },
                        text = { androidx.compose.material3.Text(tab.title) },
                        icon = {
                            androidx.compose.material3.Icon(
                                tab.icon,
                                contentDescription = tab.title
                            )
                        }
                    )
                }
            }
            
            // Horizontal pager for swipe
            androidx.compose.foundation.pager.HorizontalPager(
                state = pagerState,
                modifier = Modifier.fillMaxSize()
            ) { page ->
                // Page content
                when (page) {
                    0 -> TabContent("Home")
                    1 -> TabContent("Products")
                    2 -> TabContent("Orders")
                    3 -> TabContent("Settings")
                }
            }
        }
    }
    
    // Scrollable tabs
    @Composable
    fun ScrollableTabs() {
        val pagerState = androidx.compose.foundation.pager.rememberPagerState(pageCount = { 6 })
        
        val tabs = listOf(
            "Home", "Products", "Orders", "Settings", "Profile", "Help"
        )
        
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Scrollable Tab Row
            androidx.compose.material3.ScrollableTabRow(
                selectedTabIndex = pagerState.currentPage,
                edgePadding = 16.dp
            ) {
                tabs.forEachIndexed { index, title ->
                    androidx.compose.material3.Tab(
                        selected = pagerState.currentPage == index,
                        onClick = {
                            androidx.compose.runtime.LaunchedEffect(Unit) {
                                pagerState.animateScrollToPage(index)
                            }
                        },
                        text = { androidx.compose.material3.Text(title) }
                    )
                }
            }
            
            // Pager
            androidx.compose.foundation.pager.HorizontalPager(
                state = pagerState,
                modifier = Modifier.fillMaxSize()
            ) { page ->
                TabContent("Tab ${page + 1}")
            }
        }
    }
    
    // Tabs with navigation
    @Composable
    fun TabsWithNavigation() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Tab routes
        val tabs = listOf(
            TabItem("home", "Home", androidx.compose.material.icons.Icons.Default.Home),
            TabItem("search", "Search", androidx.compose.material.icons.Icons.Default.Search),
            TabItem("profile", "Profile", androidx.compose.material.icons.Icons.Default.Person)
        )
        
        // Current route
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        // Current tab index
        val currentIndex = tabs.indexOfFirst { it.route == currentRoute }
            .coerceAtLeast(0)
        
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Tab Row
            androidx.compose.material3.TabRow(selectedTabIndex = currentIndex) {
                tabs.forEachIndexed { index, tab ->
                    androidx.compose.material3.Tab(
                        selected = currentIndex == index,
                        onClick = {
                            if (currentIndex != index) {
                                navController.navigate(tab.route) {
                                    popUpTo(navController.graph.startDestinationId) {
                                        saveState = true
                                    }
                                    launchSingleTop = true
                                    restoreState = true
                                }
                            }
                        },
                        text = { androidx.compose.material3.Text(tab.title) },
                        icon = { androidx.compose.material3.Icon(tab.icon, contentDescription = tab.title) }
                    )
                }
            }
            
            // NavHost
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = tabs.first().route,
                modifier = Modifier.fillMaxSize()
            ) {
                tabs.forEach { tab ->
                    androidx.navigation.compose.composable(tab.route) {
                        androidx.compose.foundation.layout.Box(
                            modifier = Modifier.fillMaxSize(),
                            contentAlignment = androidx.compose.ui.Alignment.Center
                        ) {
                            androidx.compose.material3.Text(tab.title)
                        }
                    }
                }
            }
        }
    }
    
    @Composable
    fun TabContent(title: String) {
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

## Section 4: Advanced Tab Patterns

Advanced patterns including badges, custom tabs, and nested navigation.

```kotlin
/**
 * Advanced Tab Patterns
 * 
 * Advanced tab features:
 * - Badge indicators
 * - Custom tab content
 * - Nested tab navigation
 * - Tab state persistence
 */
class AdvancedTabPatterns {
    
    // Tabs with badges
    @Composable
    fun TabsWithBadges() {
        val notificationCount = 5
        
        val tabs = listOf(
            TabItem("home", "Home", androidx.compose.material.icons.Icons.Default.Home),
            TabItem("messages", "Messages", androidx.compose.material.icons.Icons.Default.Email),
            TabItem("notifications", "Notifications", androidx.compose.material.icons.Icons.Default.Notifications)
        )
        
        // Track selected tab
        var selectedTabIndex by androidx.compose.runtime.remember { androidx.compose.runtime.mutableIntStateOf(0) }
        
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.TabRow(selectedTabIndex = selectedTabIndex) {
                tabs.forEachIndexed { index, tab ->
                    androidx.compose.material3.Tab(
                        selected = selectedTabIndex == index,
                        onClick = { selectedTabIndex = index },
                        text = { androidx.compose.material3.Text(tab.title) },
                        icon = {
                            androidx.compose.foundation.layout.Box {
                                androidx.compose.material3.Icon(
                                    tab.icon,
                                    contentDescription = tab.title
                                )
                                
                                // Badge for notifications
                                if (tab.route == "notifications" && notificationCount > 0) {
                                    androidx.compose.material3.Badge(
                                        modifier = Modifier.offset(x = 12.dp, y = (-8).dp)
                                    ) {
                                        androidx.compose.material3.Text(notificationCount.toString())
                                    }
                                }
                            }
                        }
                    )
                }
            }
            
            // Tab content
            androidx.compose.foundation.layout.Box(
                modifier = Modifier.fillMaxSize(),
                contentAlignment = androidx.compose.ui.Alignment.Center
            ) {
                androidx.compose.material3.Text(tabs[selectedTabIndex].title)
            }
        }
    }
    
    // Custom indicator
    @Composable
    fun CustomIndicatorTabs() {
        var selectedTabIndex by androidx.compose.runtime.remember { androidx.compose.runtime.mutableIntStateOf(0) }
        
        val tabs = listOf("Home", "Products", "Orders")
        
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Custom tab indicator
            androidx.compose.material3.TabRow(
                selectedTabIndex = selectedTabIndex,
                containerColor = androidx.compose.material3.MaterialTheme.colorScheme.surface,
                contentColor = androidx.compose.material3.MaterialTheme.colorScheme.primary,
                divider = {
                    androidx.compose.material3.HorizontalDivider(
                        color = androidx.compose.material3.MaterialTheme.colorScheme.outline.copy(alpha = 0.3f)
                    )
                }
            ) {
                tabs.forEachIndexed { index, title ->
                    androidx.compose.material3.Tab(
                        selected = selectedTabIndex == index,
                        onClick = { selectedTabIndex = index },
                        selectedContentColor = androidx.compose.material3.MaterialTheme.colorScheme.primary,
                        unselectedContentColor = androidx.compose.material3.MaterialTheme.colorScheme.onSurface.copy(alpha = 0.6f),
                        text = {
                            androidx.compose.material3.Text(
                                text = title,
                                style = androidx.compose.material3.MaterialTheme.typography.labelLarge
                            )
                        }
                    )
                }
            }
            
            // Content
            androidx.compose.foundation.layout.Box(
                modifier = Modifier.fillMaxSize(),
                contentAlignment = androidx.compose.ui.Alignment.Center
            ) {
                androidx.compose.material3.Text("Content: ${tabs[selectedTabIndex]}")
            }
        }
    }
    
    // Nested tabs
    @Composable
    fun NestedTabs() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Main tabs
        val mainTabs = listOf(
            TabItem("feed", "Feed", androidx.compose.material.icons.Icons.Default.Home),
            TabItem("discover", "Discover", androidx.compose.material.icons.Icons.Default.Explore)
        )
        
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        val currentIndex = mainTabs.indexOfFirst { it.route == currentRoute }.coerceAtLeast(0)
        
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Main tabs
            androidx.compose.material3.TabRow(selectedTabIndex = currentIndex) {
                mainTabs.forEachIndexed { index, tab ->
                    androidx.compose.material3.Tab(
                        selected = currentIndex == index,
                        onClick = {
                            if (currentIndex != index) {
                                navController.navigate(tab.route) {
                                    popUpTo(navController.graph.startDestinationId) { saveState = true }
                                    launchSingleTop = true
                                    restoreState = true
                                }
                            }
                        },
                        text = { androidx.compose.material3.Text(tab.title) },
                        icon = { androidx.compose.material3.Icon(tab.icon, contentDescription = tab.title) }
                    )
                }
            }
            
            // Nested navigation
            androidx.navigation.compose.NavHost(
                navController = navController,
                startDestination = mainTabs.first().route
            ) {
                // Feed tab with sub-tabs
                androidx.navigation.compose.navigation(
                    startDestination = "feed_home",
                    route = "feed"
                ) {
                    FeedSubTabs()
                }
                
                // Discover tab
                androidx.navigation.compose.composable("discover") {
                    DiscoverContent()
                }
            }
        }
    }
    
    // Feed sub-tabs
    @Composable
    fun FeedSubTabs() {
        var subTabIndex by androidx.compose.runtime.remember { androidx.compose.runtime.mutableIntStateOf(0) }
        
        val subTabs = listOf("Trending", "Recent", "Following")
        
        // Nested tab row
        androidx.compose.foundation.layout.Column {
            androidx.compose.material3.TabRow(
                selectedTabIndex = subTabIndex,
                containerColor = androidx.compose.material3.MaterialTheme.colorScheme.surfaceVariant
            ) {
                subTabs.forEachIndexed { index, title ->
                    androidx.compose.material3.Tab(
                        selected = subTabIndex == index,
                        onClick = { subTabIndex = index },
                        text = { androidx.compose.material3.Text(title, style = androidx.compose.material3.MaterialTheme.typography.labelSmall) }
                    )
                }
            }
            
            androidx.compose.foundation.layout.Box(
                modifier = Modifier.fillMaxSize(),
                contentAlignment = androidx.compose.ui.Alignment.Center
            ) {
                androidx.compose.material3.Text("Feed: ${subTabs[subTabIndex]}")
            }
        }
    }
    
    @Composable
    fun DiscoverContent() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text("Discover", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
        }
    }
    
    data class TabItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector
    )
}
```

## Example: Complete Tab Navigation App

Full implementation with all tab features.

```kotlin
/**
 * Complete Tab Navigation App
 * 
 * Full implementation showing:
 * - Multiple tabs
 * - Swipe navigation
 * - Badge indicators
 * - Nested content
 */
class CompleteTabNavigation {
    
    // Tab items
    data class TabItem(
        val route: String,
        val title: String,
        val icon: androidx.compose.ui.graphics.vector.ImageVector,
        val badge: Int = 0
    )
    
    // Main app
    @Composable
    fun MainApp() {
        // Pager state
        val pagerState = androidx.compose.foundation.pager.rememberPagerState(pageCount = { 4 })
        
        // Tab items
        val tabs = listOf(
            TabItem("home", "Home", androidx.compose.material.icons.Icons.Default.Home),
            TabItem("search", "Search", androidx.compose.material.icons.Icons.Default.Search, 3),
            TabItem("cart", "Cart", androidx.compose.material.icons.Icons.Default.ShoppingCart, 5),
            TabItem("profile", "Profile", androidx.compose.material.icons.Icons.Default.Person)
        )
        
        // Column layout
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // App Bar
            androidx.compose.material3.CenterAlignedTopAppBar(
                title = { androidx.compose.material3.Text("My App") },
                colors = androidx.compose.material3.TopAppBarDefaults.centerAlignedTopAppBarColors(
                    containerColor = androidx.compose.material3.MaterialTheme.colorScheme.primaryContainer
                )
            )
            
            // Tab Row with badges
            androidx.compose.material3.TabRow(
                selectedTabIndex = pagerState.currentPage,
                containerColor = androidx.compose.material3.MaterialTheme.colorScheme.surface,
                contentColor = androidx.compose.material3.MaterialTheme.colorScheme.primary,
                indicator = { tabPositions ->
                    if (pagerState.currentPage < tabPositions.size) {
                        androidx.compose.material3.TabRowDefaults.SecondaryIndicator(
                            modifier = Modifier.tabIndicatorOffset(tabPositions[pagerState.currentPage]),
                            color = androidx.compose.material3.MaterialTheme.colorScheme.primary
                        )
                    }
                }
            ) {
                tabs.forEachIndexed { index, tab ->
                    // Tab with badge
                    androidx.compose.material3.Tab(
                        selected = pagerState.currentPage == index,
                        onClick = {
                            androidx.compose.runtime.LaunchedEffect(Unit) {
                                pagerState.animateScrollToPage(index)
                            }
                        },
                        text = { androidx.compose.material3.Text(tab.title) },
                        icon = {
                            androidx.compose.foundation.layout.Box {
                                androidx.compose.material3.Icon(
                                    tab.icon,
                                    contentDescription = tab.title
                                )
                                
                                // Badge when > 0
                                if (tab.badge > 0) {
                                    androidx.compose.material3.Badge(
                                        modifier = Modifier
                                            .align(androidx.compose.ui.Alignment.TopEnd)
                                            .offset(x = 8.dp, y = (-8).dp)
                                    ) {
                                        if (tab.badge > 9) "9+" else tab.badge.toString()
                                    }
                                }
                            }
                        },
                        selectedContentColor = androidx.compose.material3.MaterialTheme.colorScheme.primary,
                        unselectedContentColor = androidx.compose.material3.MaterialTheme.colorScheme.onSurfaceVariant
                    )
                }
            }
            
            // Horizontal Pager for swipe
            androidx.compose.foundation.pager.HorizontalPager(
                state = pagerState,
                modifier = Modifier.fillMaxSize(),
                beyondViewportPageCount = 1
            ) { page ->
                // Page content
                when (page) {
                    0 -> HomeTabContent()
                    1 -> SearchTabContent()
                    2 -> CartTabContent()
                    3 -> ProfileTabContent()
                }
            }
        }
    }
    
    @Composable
    fun HomeTabContent() {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.foundation.layout.Column(
                horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally
            ) {
                androidx.compose.material3.Text(
                    "Home",
                    style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
                )
                androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
                androidx.compose.material3.Text(
                    "Welcome to the app!",
                    style = androidx.compose.material3.MaterialTheme.typography.bodyLarge
                )
            }
        }
    }
    
    @Composable
    fun SearchTabContent() {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            // Search bar
            androidx.compose.material3.OutlinedTextField(
                value = "",
                onValueChange = { },
                modifier = Modifier.fillMaxWidth(),
                placeholder = { androidx.compose.material3.Text("Search...") },
                leadingIcon = {
                    androidx.compose.material3.Icon(
                        androidx.compose.material.icons.Icons.Default.Search,
                        contentDescription = "Search"
                    )
                },
                singleLine = true
            )
            
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            
            // Search results
            androidx.compose.foundation.layout.Box(
                modifier = Modifier.fillMaxSize(),
                contentAlignment = androidx.compose.ui.Alignment.Center
            ) {
                androidx.compose.material3.Text("Search results will appear here")
            }
        }
    }
    
    @Composable
    fun CartTabContent() {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text(
                "Your Cart",
                style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
            )
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Text("5 items in cart")
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = { }) {
                androidx.compose.material3.Text("Checkout")
            }
        }
    }
    
    @Composable
    fun ProfileTabContent() {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text(
                "Profile",
                style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
            )
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Text("user@example.com")
        }
    }
}
```

## Output Statement Results

**Tab Components:**
- TabLayout: Material tabs container
- ViewPager2: Swipe page container
- TabRow: Compose tab row
- HorizontalPager: Compose pager

**Tab Configuration:**
- Fixed mode: Equal width tabs
- Scrollable mode: Horizontal scroll
- Icon with text: Inline label
- Badge: Notification indicator

**Navigation Integration:**
- TabLayoutMediator: ViewPager + TabLayout sync
- TabRow + NavHost: Compose + Navigation
- Pager state: Track current page

**Best Practices:**
- 2-5 tabs: Avoid too many tabs
- Icon + text: Clear identification
- Swipe: Combine with tap
- Badges: For notifications/cart

## Cross-References

- See: [01_Bottom_Navigation.md](./01_Bottom_Navigation.md)
- See: [01_Jetpack_Navigation_Basics.md](../01_Navigation_Architecture/01_Jetpack_Navigation_Basics.md)
- See: [02_Navigation_Compose.md](../01_Navigation_Architecture/02_Navigation_Compose.md)
- See: [04_Flow_Navigation.md](./04_Flow_Navigation.md)

## End of Tab Navigation Guide