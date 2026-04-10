# Custom Navigation

## Learning Objectives

1. Understanding custom navigation patterns
2. Creating custom navigation components
3. Implementing gesture-based navigation
4. Building animated navigation transitions
5. Creating navigation state management
6. Implementing custom back handling
7. Building hybrid navigation systems
8. Extending navigation functionality

## Section 1: Custom Navigation Overview

Custom navigation allows creating unique navigation experiences beyond standard patterns. It's useful for specialized UI requirements or unique user experiences.

```kotlin
/**
 * Custom Navigation Overview
 * 
 * Custom navigation is used when:
 * - Standard patterns don't fit requirements
 * - Unique gesture-based navigation
 * - Custom transitions and animations
 * - Specialized navigation state management
 * - Non-standard navigation paradigms
 * 
 * Implementation approaches:
 * - Custom NavHost implementations
 * - Gesture-based navigation
 * - Custom state management
 * - Animated transitions
 */
object CustomNavigationOverview {
    
    // Custom navigation scenarios
    object CustomScenarios {
        // Carousel navigation
        const val CAROUSEL = "carousel"
        
        // Infinite scroll navigation
        const val INFINITE = "infinite"
        
        // Node-based navigation (graph)
        const val NODE_GRAPH = "node_graph"
        
        // Custom gesture navigation
        const val GESTURE = "gesture"
    }
    
    // Navigation control methods
    object NavigationControl {
        // Programmatic navigation
        const val PROGRAMMATIC = "programmatic"
        
        // Gesture-based
        const val GESTURE_BASED = "gesture_based"
        
        // Time-based (auto-advance)
        const val TIME_BASED = "time_based"
        
        // State-driven
        const val STATE_DRIVEN = "state_driven"
    }
}
```

## Section 2: Custom Navigation Components

Creating custom navigation components that extend or replace standard navigation.

```kotlin
/**
 * Custom Navigation Components
 * 
 * Building custom navigation:
 * - Custom NavHost
 * - Custom navigation state
 * - Custom transitions
 * - Custom back handling
 */
class CustomNavigationComponents {
    
    // Custom navigation state holder
    class CustomNavState<T>(
        initialRoute: T
    ) {
        // Current route
        private var _currentRoute by androidx.compose.runtime.observableStateOf(initialRoute)
        var currentRoute: T
            get() = _currentRoute
            private set(value) { _currentRoute = value }
        
        // Back stack
        private val _backStack = androidx.compose.runtime.observableStateOf(listOf<T>())
        val backStack: List<T>
            get() = _backStack.value
        
        // Forward stack (for redo)
        private val _forwardStack = androidx.compose.runtime.observableStateOf(listOf<T>())
        val forwardStack: List<T>
            get() = _forwardStack.value
        
        // Navigate forward
        fun navigateTo(route: T) {
            if (route != currentRoute) {
                _forwardStack.value = emptyList()
                _backStack.value = _backStack.value + currentRoute
                _currentRoute = route
            }
        }
        
        // Navigate back
        fun canGoBack(): Boolean = _backStack.value.isNotEmpty()
        
        fun goBack(): Boolean {
            if (canGoBack()) {
                _forwardStack.value = _forwardStack.value + currentRoute
                _currentRoute = _backStack.value.last()
                _backStack.value = _backStack.value.dropLast(1)
                return true
            }
            return false
        }
        
        // Clear history
        fun reset(route: T) {
            _currentRoute = route
            _backStack.value = emptyList()
            _forwardStack.value = emptyList()
        }
    }
    
    // Custom navigation host composable
    @Composable
    fun <T> CustomNavHost(
        state: CustomNavState<T>,
        routeToComposable: @Composable (T) -> Unit,
        modifier: Modifier = Modifier
    ) {
        // Current route content
        routeToComposable(state.currentRoute)
    }
    
    // Custom navigation with transitions
    @Composable
    fun AnimatedNavHost(
        routes: Map<String, @Composable () -> Unit>
    ) {
        var currentRoute by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(routes.keys.first()) }
        var previousRoute by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf<String?>(null) }
        
        // Transition state
        var isAnimating by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        
        // Direction for animation
        var direction by androidx.compose.runtime.remember { androidx.compose.runtime.mutableIntStateOf(0) }
        
        // Box with animation
        androidx.compose.foundation.layout.Box(modifier = modifier) {
            // Show current route
            routes[currentRoute]?.invoke()
        }
    }
    
    // Simple state-based navigation
    @Composable
    fun SimpleStateNavigation() {
        // Navigation state
        enum class Screen {
            HOME, DETAIL, SETTINGS
        }
        
        var currentScreen by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(Screen.HOME) }
        
        // Content based on state
        when (currentScreen) {
            Screen.HOME -> HomeContent(
                onNavigateToDetail = { currentScreen = Screen.DETAIL },
                onNavigateToSettings = { currentScreen = Screen.SETTINGS }
            )
            Screen.DETAIL -> DetailContent(
                onNavigateBack = { currentScreen = Screen.HOME }
            )
            Screen.SETTINGS -> SettingsContent(
                onNavigateBack = { currentScreen = Screen.HOME }
            )
        }
    }
    
    @Composable
    fun HomeContent(
        onNavigateToDetail: () -> Unit,
        onNavigateToSettings: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Home", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = onNavigateToDetail) { androidx.compose.material3.Text("Go to Detail") }
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            androidx.compose.material3.Button(onClick = onNavigateToSettings) { androidx.compose.material3.Text("Go to Settings") }
        }
    }
    
    @Composable
    fun DetailContent(onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Detail", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun SettingsContent(onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Settings", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    // Custom back stack management
    class BackStackManager<T> {
        private val stack = ArrayDeque<T>()
        
        fun push(item: T) {
            stack.addLast(item)
        }
        
        fun pop(): T? {
            return if (stack.isNotEmpty()) stack.removeLast() else null
        }
        
        fun peek(): T? = stack.lastOrNull()
        
        fun canPop(): Boolean = stack.isNotEmpty()
        
        fun clear() {
            stack.clear()
        }
        
        fun getStack(): List<T> = stack.toList()
    }
}
```

## Section 3: Gesture-Based Navigation

Implementing navigation driven by gestures like swipe, drag, or pinch.

```kotlin
/**
 * Gesture-Based Navigation
 * 
 * Gesture navigation types:
 * - Swipe to navigate (horizontal/vertical)
 * - Drag to navigate
 * - Long press for menu
 * - Pinch to zoom or navigate
 */
class GestureBasedNavigation {
    
    // Swipe-based navigation
    @Composable
    fun SwipeNavigation() {
        // Routes
        val routes = listOf("home", "search", "profile", "settings")
        
        // Current index
        var currentIndex by androidx.compose.runtime.remember { androidx.compose.runtime.mutableIntStateOf(0) }
        
        // Swipeable state
        val swipeableState = androidx.compose.foundation.gestures.detectHorizontalDragGestures(
            onDragEnd = { /* Handle swipe completion */ },
            onHorizontalDrag = { _, dragAmount ->
                // Update index based on drag direction
                if (dragAmount > 50 && currentIndex > 0) {
                    currentIndex--
                } else if (dragAmount < -50 && currentIndex < routes.size - 1) {
                    currentIndex++
                }
            }
        )
        
        // Column layout
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .pointerInput(Unit) { swipeableState }
        ) {
            // Swipe indicator
            androidx.compose.material3.Text(
                "Swipe left or right to navigate",
                modifier = Modifier.padding(16.dp)
            )
            
            // Current content
            androidx.compose.foundation.layout.Box(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(16.dp),
                contentAlignment = androidx.compose.ui.Alignment.Center
            ) {
                androidx.compose.material3.Text(
                    routes[currentIndex].replaceFirstChar { it.uppercase() },
                    style = androidx.compose.material3.MaterialTheme.typography.headlineLarge
                )
            }
            
            // Page indicators
            androidx.compose.foundation.layout.Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp),
                horizontalArrangement = androidx.compose.foundation.layout.Arrangement.Center
            ) {
                repeat(routes.size) { index ->
                    val isSelected = index == currentIndex
                    androidx.compose.foundation.layout.size(8.dp),
                    androidx.compose.foundation.background(
                        if (isSelected) androidx.compose.material3.MaterialTheme.colorScheme.primary
                        else androidx.compose.material3.MaterialTheme.colorScheme.outline,
                        shape = androidx.compose.foundation.shape.CircleShape
                    ),
                    androidx.compose.foundation.layout.Spacer(modifier = Modifier.width(4.dp))
                }
            }
        }
    }
    
    // Drag-to-reveal navigation
    @Composable
    fun DragRevealNavigation() {
        var showMenu by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        
        // Offset for drag
        var offsetX by androidx.compose.runtime.remember { androidx.compose.runtime.mutableFloatStateOf(0f) }
        
        androidx.compose.foundation.layout.Box(modifier = Modifier.fillMaxSize()) {
            // Main content
            androidx.compose.foundation.layout.Box(
                modifier = Modifier
                    .fillMaxSize()
                    .offset(x = offsetX.dp)
                    .background(androidx.compose.material3.MaterialTheme.colorScheme.surface)
            ) {
                androidx.compose.foundation.layout.Column(
                    modifier = Modifier
                        .fillMaxSize()
                        .padding(16.dp),
                    horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
                    verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
                ) {
                    androidx.compose.material3.Text(
                        "Main Content",
                        style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
                    )
                    androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
                    androidx.compose.material3.Text("Drag from left edge to reveal menu")
                }
            }
            
            // Revealed menu
            androidx.compose.foundation.layout.Box(
                modifier = Modifier
                    .fillMaxHeight()
                    .width(250.dp)
                    .offset(x = (-250 + offsetX).dp.coerceAtLeast(0f).dp)
                    .background(androidx.compose.material3.MaterialTheme.colorScheme.primaryContainer)
            ) {
                androidx.compose.foundation.layout.Column(
                    modifier = Modifier.padding(16.dp)
                ) {
                    androidx.compose.material3.Text("Menu", style = androidx.compose.material3.MaterialTheme.typography.titleLarge)
                    androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
                    androidx.compose.material3.Text("Home")
                    androidx.compose.material3.Text("Search")
                    androidx.compose.material3.Text("Profile")
                    androidx.compose.material3.Text("Settings")
                }
            }
            
            // Drag gesture handler
            androidx.compose.foundation.gestures.detectHorizontalDragGestures(
                onDragEnd = {
                    // Snap to open or closed
                    offsetX = if (offsetX > 125) 250f else 0f
                },
                onHorizontalDrag = { _, dragAmount ->
                    val newOffset = (offsetX + dragAmount).coerceIn(0f, 250f)
                    offsetX = newOffset
                }
            )
        }
    }
    
    // Pinch navigation (zoom to navigate)
    @Composable
    fun PinchNavigation() {
        var scale by androidx.compose.runtime.remember { androidx.compose.runtime.mutableFloatStateOf(1f) }
        var navigateToDetail by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        
        // Transformable for pinch
        val transformableState = androidx.compose.foundation.gestures.transformable { 
            zoomChange, _, _ ->
            scale = (scale * zoomChange).coerceIn(0.5f, 2f)
            
            // Navigate when zoomed in past threshold
            if (scale > 1.8f && !navigateToDetail) {
                navigateToDetail = true
            } else if (scale < 1.5f && navigateToDetail) {
                navigateToDetail = false
            }
        }
        
        // Content
        androidx.compose.foundation.layout.Box(
            modifier = Modifier
                .fillMaxSize()
                .graphicsLayer(
                    scaleX = scale,
                    scaleY = scale
                )
                .transformable(state = transformableState),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            if (navigateToDetail) {
                androidx.compose.material3.Text("Zoomed In - Detail View", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            } else {
                androidx.compose.material3.Text("Pinch to zoom in", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            }
        }
    }
    
    // Long press navigation menu
    @Composable
    fun LongPressNavigation() {
        var showMenu by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        var menuPosition by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(androidx.compose.ui.geometry.Offset.Zero) }
        
        androidx.compose.foundation.layout.Box(
            modifier = Modifier
                .fillMaxSize()
                .pointerInput(Unit) {
                    detectTapGestures(
                        onLongPress = { offset ->
                            menuPosition = offset
                            showMenu = true
                        }
                    )
                }
        ) {
            // Main content
            androidx.compose.foundation.layout.Box(
                modifier = Modifier.fillMaxSize(),
                contentAlignment = androidx.compose.ui.Alignment.Center
            ) {
                androidx.compose.material3.Text("Long press anywhere for menu")
            }
            
            // Context menu
            if (showMenu) {
                androidx.compose.material3.Surface(
                    modifier = Modifier
                        .offset(x = menuPosition.x.dp, y = menuPosition.y.dp),
                    shadowElevation = 4.dp
                ) {
                    androidx.compose.foundation.layout.Column {
                        androidx.compose.material3.TextButton(onClick = { showMenu = false }) { androidx.compose.material3.Text("Home") }
                        androidx.compose.material3.TextButton(onClick = { showMenu = false }) { androidx.compose.material3.Text("Search") }
                        androidx.compose.material3.TextButton(onClick = { showMenu = false }) { androidx.compose.material3.Text("Profile") }
                    }
                }
            }
        }
    }
}
```

## Section 4: Custom Transitions and Animations

Creating custom animated transitions between navigation destinations.

```kotlin
/**
 * Custom Transitions and Animations
 * 
 * Custom transition types:
 * - Slide transitions (various directions)
 * - Fade transitions
 * - Scale transitions
 * - Combined transitions
 * - Shared element transitions
 */
class CustomTransitions {
    
    // Custom slide transition
    @Composable
    fun SlideTransition(
        enter: Boolean,
        content: @Composable () -> Unit
    ) {
        var animate by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        
        androidx.compose.runtime.LaunchedEffect(enter) {
            animate = enter
        }
        
        // Animated content
        androidx.compose.animation.AnimatedContent(
            targetState = animate,
            transitionSpec = {
                // Slide from right on enter
                if (targetState) {
                    slideInHorizontally { width -> width } + fadeIn() togetherWith
                            slideOutHorizontally { width -> -width } + fadeOut()
                } else {
                    // Slide to left on exit
                    slideInHorizontally { width -> -width } + fadeIn() togetherWith
                            slideOutHorizontally { width -> width } + fadeOut()
                }.using(androidx.compose.animation.ExperimentalAnimationApi)
            },
            label = "slide_transition"
        ) { target ->
            content()
        }
    }
    
    // Fade scale transition
    @Composable
    fun FadeScaleTransition(
        route: String,
        content: @Composable () -> Unit
    ) {
        var isVisible by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(true) }
        
        // Trigger animation on route change
        androidx.compose.runtime.LaunchedEffect(route) {
            isVisible = false
            kotlinx.coroutines.delay(50)
            isVisible = true
        }
        
        androidx.compose.animation.AnimatedVisibility(
            visible = isVisible,
            enter = androidx.compose.animation.fadeIn(
                animationSpec = androidx.compose.animation.core.tween(300)
            ) + androidx.compose.animation.scaleIn(
                initialScale = 0.8f,
                animationSpec = androidx.compose.animation.core.tween(300)
            ),
            exit = androidx.compose.animation.fadeOut(
                animationSpec = androidx.compose.animation.core.tween(300)
            ) + androidx.compose.animation.scaleOut(
                targetScale = 0.8f,
                animationSpec = androidx.compose.animation.core.tween(300)
            )
        ) {
            content()
        }
    }
    
    // Shared element transition simulation
    @Composable
    fun SharedElementTransition(
        fromRoute: String,
        toRoute: String,
        content: @Composable () -> Unit
    ) {
        // Track if transitioning
        var isTransitioning by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        
        // Animate between routes
        androidx.compose.animation.AnimatedContent(
            targetState = toRoute,
            transitionSpec = {
                // Crossfade with scale
                fadeIn(animationSpec = androidx.compose.animation.core.tween(300)) togetherWith
                        fadeOut(animationSpec = androidx.compose.animation.core.tween(300))
            },
            label = "shared_element"
        ) { route ->
            content()
        }
    }
    
    // Carousel-style transition
    @Composable
    fun CarouselTransition() {
        val routes = listOf("home", "products", "cart", "profile")
        var currentIndex by androidx.compose.runtime.remember { androidx.compose.runtime.mutableIntStateOf(0) }
        
        // Animated pager for carousel effect
        val pagerState = androidx.compose.foundation.pager.rememberPagerState(pageCount = { routes.size })
        
        // Sync with state
        androidx.compose.runtime.LaunchedEffect(pagerState.currentPage) {
            currentIndex = pagerState.currentPage
        }
        
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            // Carousel pager
            androidx.compose.foundation.pager.HorizontalPager(
                state = pagerState,
                modifier = Modifier
                    .fillMaxWidth()
                    .weight(1f),
                pageSpacing = 16.dp,
                flingBehavior = androidx.compose.foundation.gestures.snapping.rememberSnapFlingBehavior(
                    pagerState = pagerState
                )
            ) { page ->
                androidx.compose.foundation.layout.Box(
                    modifier = Modifier
                        .fillMaxSize()
                        .background(
                            androidx.compose.material3.MaterialTheme.colorScheme.surfaceVariant
                        ),
                    contentAlignment = androidx.compose.ui.Alignment.Center
                ) {
                    androidx.compose.material3.Text(
                        routes[page].replaceFirstChar { it.uppercase() },
                        style = androidx.compose.material3.MaterialTheme.typography.headlineLarge
                    )
                }
            }
            
            // Indicators
            androidx.compose.foundation.layout.Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(16.dp),
                horizontalArrangement = androidx.compose.foundation.layout.Arrangement.Center
            ) {
                repeat(routes.size) { index ->
                    androidx.compose.foundation.layout.size(8.dp)
                    androidx.compose.foundation.background(
                        if (index == currentIndex) androidx.compose.material3.MaterialTheme.colorScheme.primary
                        else androidx.compose.material3.MaterialTheme.colorScheme.outline,
                        shape = androidx.compose.foundation.shape.CircleShape
                    )
                    androidx.compose.foundation.layout.Spacer(modifier = Modifier.width(4.dp))
                }
            }
        }
    }
    
    // Animated navigation wrapper
    @Composable
    fun AnimatedNavigationWrapper(
        currentRoute: String,
        routes: Map<String, @Composable () -> Unit>
    ) {
        androidx.compose.animation.AnimatedContent(
            targetState = currentRoute,
            transitionSpec = {
                // Custom transition
                val direction = if (targetState < initialState) 1 else -1
                
                slideInHorizontally { it * direction } + fadeIn() togetherWith
                        slideOutHorizontally { -it * direction } + fadeOut()
            },
            label = "animated_nav"
        ) { route ->
            routes[route]?.invoke()
        }
    }
}
```

## Section 5: Hybrid Navigation Systems

Combining multiple navigation patterns into cohesive user experiences.

```kotlin
/**
 * Hybrid Navigation Systems
 * 
 * Combining patterns:
 * - Bottom nav + drawer
 * - Tabs + nested nav
 * - Flow + standard navigation
 * - Custom + standard
 */
class HybridNavigationSystems {
    
    // Bottom nav with drawer and custom flow
    @Composable
    fun HybridNavigationApp() {
        val navController = androidx.navigation.compose.rememberNavController()
        var showDrawer by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        
        // Bottom nav items
        val bottomNavItems = listOf(
            "home", "products", "cart"
        )
        
        // Current route
        val currentRoute = navController.currentBackStackEntryAsState()
            .value?.destination?.route
        
        // Determine if showing bottom nav
        val showBottomNav = currentRoute in bottomNavItems
        
        // Drawer state
        val drawerState = androidx.compose.material3.rememberDrawerState(initialValue = androidx.compose.material3.DrawerValue.Closed)
        
        // Main scaffold with drawer
        androidx.compose.material3.ModalNavigationDrawer(
            drawerState = drawerState,
            drawerContent = {
                // Drawer content with additional navigation
                androidx.compose.material3.ModalDrawerSheet {
                    androidx.compose.material3.Text("Additional Navigation", modifier = Modifier.padding(16.dp))
                    androidx.compose.material3.HorizontalDivider()
                    androidx.compose.material3.TextButton(onClick = {
                        androidx.compose.foundation.LaunchedEffect(Unit) {
                            drawerState.close()
                            navController.navigate("settings")
                        }
                    }) {
                        androidx.compose.material3.Text("Settings")
                    }
                    androidx.compose.material3.TextButton(onClick = {
                        androidx.compose.foundation.LaunchedEffect(Unit) {
                            drawerState.close()
                            navController.navigate("profile")
                        }
                    }) {
                        androidx.compose.material3.Text("Profile")
                    }
                }
            },
            content = {
                // Main scaffold
                androidx.compose.material3.Scaffold(
                    topBar = {
                        androidx.compose.material3.TopAppBar(
                            title = { androidx.compose.material3.Text(currentRoute?.replaceFirstChar { it.uppercase() } ?: "App") },
                            navigationIcon = {
                                androidx.compose.material3.IconButton(
                                    onClick = {
                                        androidx.compose.foundation.LaunchedEffect(Unit) {
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
                    },
                    bottomBar = {
                        if (showBottomNav) {
                            androidx.compose.material3.NavigationBar {
                                bottomNavItems.forEach { route ->
                                    androidx.compose.material3.NavigationBarItem(
                                        selected = currentRoute == route,
                                        onClick = {
                                            if (currentRoute != route) {
                                                navController.navigate(route) {
                                                    popUpTo(navController.graph.startDestinationId) {
                                                        saveState = true
                                                    }
                                                    launchSingleTop = true
                                                    restoreState = true
                                                }
                                            }
                                        },
                                        icon = {
                                            when (route) {
                                                "home" -> androidx.compose.material3.Icon(androidx.compose.material.icons.Icons.Default.Home, contentDescription = route)
                                                "products" -> androidx.compose.material3.Icon(androidx.compose.material.icons.Icons.Default.ShoppingBag, contentDescription = route)
                                                "cart" -> androidx.compose.material3.Icon(androidx.compose.material.icons.Icons.Default.ShoppingCart, contentDescription = route)
                                            }
                                        },
                                        label = { androidx.compose.material3.Text(route.replaceFirstChar { it.uppercase() }) }
                                    )
                                }
                            }
                        }
                    }
                ) { padding ->
                    // Navigation host
                    androidx.navigation.compose.NavHost(
                        navController = navController,
                        startDestination = "home",
                        modifier = Modifier.padding(padding)
                    ) {
                        // Bottom nav destinations
                        bottomNavItems.forEach { route ->
                            androidx.navigation.compose.composable(route) {
                                HybridContent(title = route)
                            }
                        }
                        
                        // Additional destinations
                        listOf("settings", "profile").forEach { route ->
                            androidx.navigation.compose.composable(route) {
                                HybridContent(title = route)
                            }
                        }
                        
                        // Custom flow (checkout)
                        checkoutFlow(navController)
                    }
                }
            }
        )
    }
    
    // Nested checkout flow
    @Composable
    fun androidx.navigation.NavGraphBuilder.checkoutFlow(navController: androidx.navigation.compose.NavHostController) {
        // Nested navigation for checkout
        androidx.navigation.compose.navigation(
            startDestination = "cart",
            route = "checkout_flow"
        ) {
            androidx.navigation.compose.composable("cart") {
                CheckoutStepContent("Cart", onProceed = { navController.navigate("shipping") })
            }
            androidx.navigation.compose.composable("shipping") {
                CheckoutStepContent("Shipping", onProceed = { navController.navigate("payment") }, onBack = { navController.popBackStack() })
            }
            androidx.navigation.compose.composable("payment") {
                CheckoutStepContent("Payment", onProceed = { 
                    navController.navigate("confirmation") {
                        popUpTo("cart") { inclusive = true }
                    }
                }, onBack = { navController.popBackStack() })
            }
            androidx.navigation.compose.composable("confirmation") {
                CheckoutStepContent("Done!", onProceed = {
                    navController.navigate("home") {
                        popUpTo("checkout_flow") { inclusive = true }
                    }
                })
            }
        }
    }
    
    @Composable
    fun CheckoutStepContent(title: String, onProceed: () -> Unit, onBack: (() -> Unit)? = null) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text(title, style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            androidx.compose.material3.Button(onClick = onProceed) {
                androidx.compose.material3.Text("Continue")
            }
            if (onBack != null) {
                androidx.compose.material3.TextButton(onClick = onBack) {
                    androidx.compose.material3.Text("Back")
                }
            }
        }
    }
    
    @Composable
    fun HybridContent(title: String) {
        androidx.compose.foundation.layout.Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = androidx.compose.ui.Alignment.Center
        ) {
            androidx.compose.material3.Text(title, style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
        }
    }
    
    // Navigation controller wrapper
    class HybridNavController<T>(
        private val routes: Map<T, List<T>>
    ) {
        private val _currentRoute = androidx.compose.runtime.observableStateOf<T?>(null)
        var currentRoute: T?
            get() = _currentRoute.value
            set(value) { _currentRoute.value = value }
        
        private val _backStack = androidx.compose.runtime.observableStateOf(listOf<T>())
        val backStack: List<T>
            get() = _backStack.value
        
        fun navigate(route: T) {
            val current = currentRoute
            if (current != null) {
                _backStack.value = _backStack.value + current
            }
            currentRoute = route
        }
        
        fun navigateUp(): Boolean {
            return if (_backStack.value.isNotEmpty()) {
                currentRoute = _backStack.value.last()
                _backStack.value = _backStack.value.dropLast(1)
                true
            } else {
                false
            }
        }
        
        fun canNavigateUp(): Boolean = _backStack.value.isNotEmpty()
    }
}
```

## Example: Complete Custom Navigation System

Complete implementation showing a fully custom navigation system.

```kotlin
/**
 * Complete Custom Navigation System
 * 
 * Full implementation showing:
 * - Custom navigation state
 * - Gesture support
 * - Custom transitions
 * - Hybrid patterns
 */
class CompleteCustomNavigation {
    
    // Navigation routes
    enum class AppRoute {
        HOME, PRODUCTS, PRODUCT_DETAIL, CART, CHECKOUT, PROFILE, SETTINGS
    }
    
    // Navigation state
    class AppNavState {
        private val _currentRoute = androidx.compose.runtime.observableStateOf(AppRoute.HOME)
        var currentRoute: AppRoute
            get() = _currentRoute.value
            set(value) { _currentRoute.value = value }
        
        private val _backStack = androidx.compose.runtime.observableStateOf(listOf<AppRoute>())
        val backStack: List<AppRoute>
            get() = _backStack.value
        
        private val _canGoBack = androidx.compose.runtime.derivedStateOf { _backStack.value.isNotEmpty() }
        val canGoBack: Boolean
            get() = _canGoBack.value
        
        // Current item ID for detail
        private val _currentItemId = androidx.compose.runtime.observableStateOf<String?>(null)
        var currentItemId: String?
            get() = _currentItemId.value
            set(value) { _currentItemId.value = value }
        
        fun navigate(route: AppRoute, itemId: String? = null) {
            _backStack.value = _backStack.value + _currentRoute.value
            _currentRoute.value = route
            _currentItemId.value = itemId
        }
        
        fun goBack(): Boolean {
            return if (_backStack.value.isNotEmpty()) {
                _currentRoute.value = _backStack.value.last()
                _backStack.value = _backStack.value.dropLast(1)
                true
            } else {
                false
            }
        }
        
        fun popToRoot() {
            _currentRoute.value = AppRoute.HOME
            _backStack.value = emptyList()
            _currentItemId.value = null
        }
        
        // Navigate to checkout flow
        fun startCheckout() {
            navigate(AppRoute.CART)
            navigate(AppRoute.CHECKOUT)
        }
    }
    
    // Main app composable
    @Composable
    fun CustomNavigationApp() {
        // Create navigation state
        val navState = androidx.compose.runtime.remember { AppNavState() }
        
        // Gesture detection for back
        val onBack = {
            navState.goBack()
        }
        
        // Scaffold structure
        androidx.compose.material3.Scaffold(
            topBar = {
                CustomAppBar(
                    currentRoute = navState.currentRoute,
                    canGoBack = navState.canGoBack,
                    onBack = onBack,
                    onMenuClick = { /* Open menu */ }
                )
            }
        ) { padding ->
            // Custom transition wrapper
            CustomTransitionWrapper(
                route = navState.currentRoute
            ) {
                // Route-based content
                when (navState.currentRoute) {
                    AppRoute.HOME -> HomeScreen(
                        onNavigateToProducts = { navState.navigate(AppRoute.PRODUCTS) },
                        onNavigateToProfile = { navState.navigate(AppRoute.PROFILE) }
                    )
                    
                    AppRoute.PRODUCTS -> ProductsScreen(
                        onProductClick = { productId -> 
                            navState.navigate(AppRoute.PRODUCT_DETAIL, productId) 
                        },
                        onNavigateToCart = { navState.navigate(AppRoute.CART) }
                    )
                    
                    AppRoute.PRODUCT_DETAIL -> ProductDetailScreen(
                        productId = navState.currentItemId,
                        onAddToCart = { navState.navigate(AppRoute.CART) },
                        onBack = onBack
                    )
                    
                    AppRoute.CART -> CartScreen(
                        onCheckout = { navState.startCheckout() },
                        onBack = onBack
                    )
                    
                    AppRoute.CHECKOUT -> CheckoutFlowContent(
                        onComplete = { navState.popToRoot() },
                        onBack = onBack
                    )
                    
                    AppRoute.PROFILE -> ProfileScreen(onBack = onBack)
                    
                    AppRoute.SETTINGS -> SettingsScreen(onBack = onBack)
                }
            }
        }
    }
    
    // Custom app bar
    @Composable
    fun CustomAppBar(
        currentRoute: AppRoute,
        canGoBack: Boolean,
        onBack: () -> Unit,
        onMenuClick: () -> Unit
    ) {
        androidx.compose.material3.TopAppBar(
            title = {
                androidx.compose.material3.Text(
                    currentRoute.name.replace("_", " ").lowercase()
                        .replaceFirstChar { it.uppercase() }
                )
            },
            navigationIcon = {
                if (canGoBack) {
                    androidx.compose.material3.IconButton(onClick = onBack) {
                        androidx.compose.material3.Icon(
                            androidx.compose.material.icons.Icons.Default.ArrowBack,
                            contentDescription = "Back"
                        )
                    }
                } else {
                    androidx.compose.material3.IconButton(onClick = onMenuClick) {
                        androidx.compose.material3.Icon(
                            androidx.compose.material.icons.Icons.Default.Menu,
                            contentDescription = "Menu"
                        )
                    }
                }
            }
        )
    }
    
    // Custom transition wrapper
    @Composable
    fun CustomTransitionWrapper(
        route: AppRoute,
        content: @Composable () -> Unit
    ) {
        // Simple fade transition
        androidx.compose.animation.AnimatedContent(
            targetState = route,
            transitionSpec = {
                val direction = if (targetState.ordinal > initialState.ordinal) 1 else -1
                
                // Slide with fade
                (slideInHorizontally { it * direction } + fadeIn()) togetherWith
                        (slideOutHorizontally { -it * direction } + fadeOut())
            },
            label = "custom_transition"
        ) { _ ->
            content()
        }
    }
    
    // Screen implementations
    @Composable
    fun HomeScreen(
        onNavigateToProducts: () -> Unit,
        onNavigateToProfile: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Home", style = androidx.compose.material3.MaterialTheme.typography.headlineLarge)
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            androidx.compose.material3.Button(onClick = onNavigateToProducts) { androidx.compose.material3.Text("Browse Products") }
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = onNavigateToProfile) { androidx.compose.material3.Text("Profile") }
        }
    }
    
    @Composable
    fun ProductsScreen(
        onProductClick: (String) -> Unit,
        onNavigateToCart: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Products", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            listOf("Product 1", "Product 2", "Product 3").forEach { product ->
                androidx.compose.material3.Card(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 4.dp)
                ) {
                    androidx.compose.material3.TextButton(
                        onClick = { onProductClick(product) }
                    ) {
                        androidx.compose.material3.Text(product)
                    }
                }
            }
        }
    }
    
    @Composable
    fun ProductDetailScreen(
        productId: String?,
        onAddToCart: () -> Unit,
        onBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Product: ${productId ?: "Unknown"}", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            androidx.compose.material3.Button(onClick = onAddToCart) { androidx.compose.material3.Text("Add to Cart") }
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun CartScreen(onCheckout: () -> Unit, onBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Cart", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.Button(onClick = onCheckout) { androidx.compose.material3.Text("Checkout") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun CheckoutFlowContent(onComplete: () -> Unit, onBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Checkout Flow", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Text("Shipping -> Payment -> Confirm")
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.Button(onClick = onComplete) { androidx.compose.material3.Text("Complete Order") }
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun ProfileScreen(onBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Profile", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun SettingsScreen(onBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp)
        ) {
            androidx.compose.material3.Text("Settings", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.weight(1f))
            androidx.compose.material3.TextButton(onClick = onBack) { androidx.compose.material3.Text("Back") }
        }
    }
}
```

## Output Statement Results

**Custom Navigation Types:**
- State-based: Simple enum/State navigation
- Gesture-based: Swipe, drag, pinch, long-press
- Animated: Custom transitions and animations
- Hybrid: Combined navigation patterns

**Custom Components:**
- CustomNavState: Programmatic navigation
- Custom transitions: Slide, fade, scale
- Gesture handlers: Various gesture detection
- Back stack management: Custom stack implementation

**Implementation Patterns:**
- Observable state: For reactive navigation
- Gesture detection: PointerInput handling
- AnimatedContent: For transitions
- Navigation composables: Custom routing

**Best Practices:**
- Clear state management: Centralized navigation state
- Smooth transitions: Proper animation specs
- Gesture handling: Appropriate sensitivity
- Back handling: Consistent back navigation

## Cross-References

- See: [01_Jetpack_Navigation_Basics.md](../01_Navigation_Architecture/01_Jetpack_Navigation_Basics.md)
- See: [02_Navigation_Compose.md](../01_Navigation_Architecture/02_Navigation_Compose.md)
- See: [01_Bottom_Navigation.md](./01_Bottom_Navigation.md)
- See: [02_Navigation_Drawer.md](./02_Navigation_Drawer.md)
- See: [03_Tab_Navigation.md](./03_Tab_Navigation.md)
- See: [04_Flow_Navigation.md](./04_Flow_Navigation.md)

## End of Custom Navigation Guide