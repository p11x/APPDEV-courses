# Advanced Navigation Patterns

## Learning Objectives

1. Implementing nested navigation graphs
2. Managing global navigation actions
3. Using Navigation UI with bottom navigation
4. Implementing conditional navigation
5. Creating reusable navigation components
6. Handling navigation state restoration
7. Managing deep links with authentication
8. Implementing navigation testing

## Section 1: Nested Navigation Graphs

Nested navigation graphs organize destinations into separate flows, each with its own start destination and back stack.

```kotlin
/**
 * Nested Navigation Graphs
 * 
 * Nested graphs provide:
 * - Separate navigation flows
 * - Scoped back stack management
 * - Reusable navigation components
 * - Clear task boundaries
 */
class NestedNavigationGraphs {
    
    // XML nested graph definition
    fun getXmlNestedGraph(): String {
        return """
<navigation xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/nav_graph"
    app:startDestination="@id/main_graph">
    
    <!-- Root graph -->
    <navigation
        android:id="@+id/main_graph"
        app:startDestination="@id/homeFragment">
        
        <fragment
            android:id="@+id/homeFragment"
            android:name="com.example.HomeFragment"
            android:label="Home">
            
            <!-- Action to nested checkout graph -->
            <action
                android:id="@+id/action_home_to_checkout"
                app:destination="@id/checkout_graph" />
        </fragment>
        
        <!-- More top-level destinations -->
        <fragment
            android:id="@+id/profileFragment"
            android:name="com.example.ProfileFragment"
            android:label="Profile" />
    </navigation>
    
    <!-- Nested checkout graph -->
    <navigation
        android:id="@+id/checkout_graph"
        app:startDestination="@id/cartFragment">
        
        <fragment
            android:id="@+id/cartFragment"
            android:name="com.example.CartFragment"
            android:label="Cart">
            
            <action
                android:id="@+id/action_cart_to_shipping"
                app:destination="@id/shippingFragment" />
        </fragment>
        
        <fragment
            android:id="@+id/shippingFragment"
            android:name="com.example.ShippingFragment"
            android:label="Shipping">
            
            <action
                android:id="@+id/action_shipping_to_payment"
                app:destination="@id/paymentFragment" />
        </fragment>
        
        <fragment
            android:id="@+id/paymentFragment"
            android:name="com.example.PaymentFragment"
            android:label="Payment" />
    </navigation>
</navigation>
        """.trimIndent()
    }
    
    // Navigation Compose nested graph
    @Composable
    fun NestedComposeGraph() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "main_graph"
        ) {
            // Main nested graph
            androidx.navigation.compose.navigation(
                startDestination = "home",
                route = "main_graph"
            ) {
                // Home route
                androidx.navigation.compose.composable("home") {
                    HomeScreen(
                        onStartCheckout = {
                            navController.navigate("checkout_graph")
                        },
                        onViewProfile = {
                            navController.navigate("profile")
                        }
                    )
                }
                
                // Profile route
                androidx.navigation.compose.composable("profile") {
                    ProfileScreen(
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
            }
            
            // Checkout nested graph
            androidx.navigation.compose.navigation(
                startDestination = "cart",
                route = "checkout_graph"
            ) {
                // Cart route
                androidx.navigation.compose.composable("cart") {
                    CartScreen(
                        onProceedToShipping = {
                            navController.navigate("shipping")
                        }
                    )
                }
                
                // Shipping route
                androidx.navigation.compose.composable("shipping") {
                    ShippingScreen(
                        onProceedToPayment = {
                            navController.navigate("payment")
                        },
                        onGoBack = { navController.popBackStack() }
                    )
                }
                
                // Payment route
                androidx.navigation.compose.composable("payment") {
                    PaymentScreen(
                        onComplete = {
                            // Pop to main graph start
                            navController.popBackStack("main_graph", false)
                        }
                    )
                }
            }
        }
    }
    
    @Composable
    fun HomeScreen(
        onStartCheckout: () -> Unit,
        onViewProfile: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize(),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Home")
            androidx.compose.material3.Button(onClick = onStartCheckout) {
                androidx.compose.material3.Text("Start Checkout")
            }
            androidx.compose.material3.Button(onClick = onViewProfile) {
                androidx.compose.material3.Text("Profile")
            }
        }
    }
    
    @Composable
    fun ProfileScreen(onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Profile")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun CartScreen(onProceedToShipping: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Cart")
            androidx.compose.material3.Button(onClick = onProceedToShipping) {
                androidx.compose.material3.Text("Continue")
            }
        }
    }
    
    @Composable
    fun ShippingScreen(
        onProceedToPayment: () -> Unit,
        onGoBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Shipping")
            androidx.compose.material3.Button(onClick = onProceedToPayment) {
                androidx.compose.material3.Text("Continue")
            }
            androidx.compose.material3.Button(onClick = onGoBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun PaymentScreen(onComplete: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Payment")
            androidx.compose.material3.Button(onClick = onComplete) {
                androidx.compose.material3.Text("Complete")
            }
        }
    }
}
```

## Section 2: Global Actions

Global actions allow navigation from anywhere in the app to a specific destination, regardless of the current navigation graph.

```kotlin
/**
 * Global Navigation Actions
 * 
 * Global actions provide:
 * - Navigation from any destination
 * - Centralized navigation handling
 * - Login/authentication redirects
 * - Error handling navigation
 */
class GlobalActions {
    
    // XML global actions
    fun getXmlGlobalActions(): String {
        return """
<navigation xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/nav_graph"
    app:startDestination="@id/homeFragment">
    
    <!-- Global action - can be used from any destination -->
    <action
        android:id="@+id/action_global_login"
        app:destination="@id/loginFragment" />
    
    <!-- Global action with arguments -->
    <action
        android:id="@+id/action_global_error"
        app:destination="@id/errorFragment">
        
        <argument
            android:name="errorMessage"
            app:argType="string"
            android:defaultValue="An error occurred" />
    </action>
    
    <!-- Home fragment -->
    <fragment
        android:id="@+id/homeFragment"
        android:name="com.example.HomeFragment"
        android:label="Home" />
    
    <!-- Login fragment -->
    <fragment
        android:id="@+id/loginFragment"
        android:name="com.example.LoginFragment"
        android:label="Login" />
    
    <!-- Error fragment -->
    <fragment
        android:id="@+id/errorFragment"
        android:name="com.example.ErrorFragment"
        android:label="Error">
        
        <argument
            android:name="errorMessage"
            app:argType="string"
            android:defaultValue="An error occurred" />
    </fragment>
</navigation>
        """.trimIndent()
    }
    
    // Using global action in fragment
    class UsingGlobalAction : androidx.fragment.app.Fragment() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        fun useGlobalAction() {
            // Use global action - navigate from anywhere
            navController.navigate(R.id.action_global_login)
        }
        
        fun useGlobalActionWithArgs() {
            // Pass argument with global action
            val bundle = android.os.Bundle().apply {
                putString("errorMessage", "Custom error message")
            }
            navController.navigate(R.id.action_global_error, bundle)
        }
        
        fun useGlobalActionInCompose() {
            val navController = androidx.navigation.compose.rememberNavController()
            
            // Similar in Compose - use route directly
            // navController.navigate("login")
            
            // With Safe Args
            // val action = HomeFragmentDirections.actionGlobalLogin()
            // navController.navigate(action)
        }
    }
    
    // Use case: Authentication required
    class AuthNavigation {
        
        // Check if user is logged in
        private val isLoggedIn = false
        
        fun navigateIfAuthenticated() {
            val navController = androidx.navigation.compose.rememberNavController()
            
            if (isLoggedIn) {
                // Navigate to profile
                navController.navigate("profile")
            } else {
                // Redirect to login
                navController.navigate("login")
            }
        }
        
        // Use case: Redirect after login
        fun handlePostLogin() {
            val navController = androidx.navigation.compose.rememberNavController()
            
            // After successful login, navigate to intended destination
            // Option 1: Pop back and navigate
            // navController.popBackStack()
            // navController.navigate("profile")
            
            // Option 2: Use popUpTo to clear stack
            navController.navigate("profile") {
                popUpTo("login") { inclusive = true }
            }
        }
    }
}
```

## Section 3: Navigation State Restoration

Navigation state can be saved and restored to maintain user context across process death and recreation.

```kotlin
/**
 * Navigation State Restoration
 * 
 * Saving and restoring navigation state:
 * - Process death handling
 * - Back stack preservation
 * - Deep link handling
 * - SavedStateHandle for data
 */
class NavigationStateRestoration {
    
    // Using SavedStateHandle in ViewModel
    class SharedViewModel(
        private val savedStateHandle: androidx.savedstate.SavedStateHandle
    ) : androidx.lifecycle.ViewModel() {
        
        // Save state
        fun saveData(key: String, value: String) {
            savedStateHandle[key] = value
        }
        
        // Get saved state
        fun getData(key: String): String? {
            return savedStateHandle[key]
        }
        
        // Get with default
        fun getDataWithDefault(key: String, default: String): String {
            return savedStateHandle[key] ?: default
        }
        
        // LiveData from SavedStateHandle
        val dataLiveData: androidx.lifecycle.LiveData<String> = 
            savedStateHandle.getLiveData("key")
    }
    
    // Navigation Compose state
    @Composable
    fun StateRestorationCompose() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Save back stack state
        val backStackEntry = navController.currentBackStackEntry
        
        // Access saved state handle
        val savedStateHandle = backStackEntry?.savedStateHandle
        
        // Save value
        androidx.compose.runtime.LaunchedEffect(Unit) {
            savedStateHandle?.set("key", "value")
            
            // Read value
            val value = savedStateHandle?.get<String>("key")
            println("Retrieved: ${value}")
        }
        
        // Restore from deep link
        val deepLinkArgs = navController.currentBackStackEntry?.arguments
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "home"
        ) {
            androidx.navigation.compose.composable("home") {
                HomeContent()
            }
        }
    }
    
    // Using NavBackStackEntry state
    @Composable
    fun observeNavigationState() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Current back stack entry
        val currentBackStackEntry by navController.currentBackStackEntryAsState()
        
        // Get destination
        val destination = currentBackStackEntry?.destination
        
        // Get arguments
        val arguments = currentBackStackEntry?.arguments
        
        // Get saved state handle
        val savedStateHandle = currentBackStackEntry?.savedStateHandle
        
        // Previous back stack entry (for returning data)
        val previousBackStackEntry = navController.previousBackStackEntry
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "list"
        ) {
            // Set result for returning
            androidx.navigation.compose.composable("detail/{itemId}") { backStackEntry ->
                // Set result
                backStackEntry.savedStateHandle.setResult("item", "value")
                
                androidx.compose.material3.Text("Detail")
            }
            
            // Get result from previous
            androidx.navigation.compose.composable("list") { backStackEntry ->
                // Get result from detail screen
                val result = backStackEntry.savedStateHandle.get<String>("item")
                
                androidx.compose.material3.Text("List: ${result ?: "no result"}")
            }
        }
    }
    
    // Save state across navigation
    @Composable
    fun FormStateExample() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Form state that persists across navigation
        var formData by androidx.compose.runtime.remember { 
            androidx.compose.runtime.mutableStateOf(FormData()) 
        }
        
        fun saveFormState() {
            // Save to saved state handle for restoration
            navController.currentBackStackEntry?.savedStateHandle?.let { handle ->
                handle["formName"] = formData.name
                handle["formEmail"] = formData.email
            }
        }
        
        fun restoreFormState() {
            navController.currentBackStackEntry?.savedStateHandle?.let { handle ->
                formData = FormData(
                    name = handle["formName"] ?: "",
                    email = handle["formEmail"] ?: ""
                )
            }
        }
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "form"
        ) {
            androidx.navigation.compose.composable("form") {
                FormScreen(
                    formData = formData,
                    onDataChange = { newData ->
                        formData = newData
                    },
                    onSave = {
                        saveFormState()
                        navController.navigate("confirm")
                    },
                    onRestore = { restoreFormState() }
                )
            }
            
            androidx.navigation.compose.composable("confirm") {
                ConfirmScreen(
                    onNavigateBack = { navController.popBackStack() }
                )
            }
        }
    }
    
    data class FormData(val name: String = "", val email: String = "")
    
    @Composable
    fun FormScreen(
        formData: FormData,
        onDataChange: (FormData) -> Unit,
        onSave: () -> Unit,
        onRestore: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize().padding(16.dp)) {
            androidx.compose.material3.OutlinedTextField(
                value = formData.name,
                onValueChange = { onDataChange(formData.copy(name = it)) },
                label = { androidx.compose.material3.Text("Name") }
            )
            androidx.compose.material3.OutlinedTextField(
                value = formData.email,
                onValueChange = { onDataChange(formData.copy(email = it)) },
                label = { androidx.compose.material3.Text("Email") }
            )
            androidx.compose.material3.Button(onClick = onSave) {
                androidx.compose.material3.Text("Save")
            }
            androidx.compose.material3.Button(onClick = onRestore) {
                androidx.compose.material3.Text("Restore")
            }
        }
    }
    
    @Composable
    fun ConfirmScreen(onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Confirmed")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun HomeContent() {
        androidx.compose.material3.Text("Home")
    }
}
```

## Example: Complete Advanced Navigation

Complete example with all advanced patterns.

```kotlin
/**
 * Complete Advanced Navigation Example
 * 
 * Implementing advanced patterns for production apps.
 */
class CompleteAdvancedNavigation {
    
    // Navigation routes
    object Routes {
        const val MAIN_GRAPH = "main"
        const val AUTH_GRAPH = "auth"
        const val HOME = "home"
        const val PROFILE = "profile"
        const val SETTINGS = "settings"
        const val LOGIN = "login"
        const val REGISTER = "register"
        const val PRODUCT_LIST = "product_list"
        const val PRODUCT_DETAIL = "product_detail/{productId}"
        const val CART = "cart"
        const val CHECKOUT_GRAPH = "checkout"
    }
    
    // Auth state
    object AuthManager {
        private var isLoggedIn = false
        private var currentUser: String? = null
        
        fun isAuthenticated(): Boolean = isLoggedIn
        
        fun login(username: String) {
            isLoggedIn = true
            currentUser = username
        }
        
        fun logout() {
            isLoggedIn = false
            currentUser = null
        }
        
        fun getCurrentUser(): String? = currentUser
    }
    
    // Main navigation with auth check
    @Composable
    fun AppNavigation() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Determine start destination based on auth state
        val startDestination = if (AuthManager.isAuthenticated()) {
            Routes.MAIN_GRAPH
        } else {
            Routes.AUTH_GRAPH
        }
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = startDestination
        ) {
            // === Auth nested graph ===
            androidx.navigation.compose.navigation(
                startDestination = Routes.LOGIN,
                route = Routes.AUTH_GRAPH
            ) {
                androidx.navigation.compose.composable(Routes.LOGIN) {
                    LoginScreen(
                        onLoginSuccess = { username ->
                            AuthManager.login(username)
                            // Navigate to main, clear auth stack
                            navController.navigate(Routes.MAIN_GRAPH) {
                                popUpTo(Routes.AUTH_GRAPH) { inclusive = true }
                            }
                        },
                        onNavigateToRegister = {
                            navController.navigate(Routes.REGISTER)
                        }
                    )
                }
                
                androidx.navigation.compose.composable(Routes.REGISTER) {
                    RegisterScreen(
                        onRegisterSuccess = { username ->
                            AuthManager.login(username)
                            navController.navigate(Routes.MAIN_GRAPH) {
                                popUpTo(Routes.AUTH_GRAPH) { inclusive = true }
                            }
                        },
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
            }
            
            // === Main nested graph ===
            androidx.navigation.compose.navigation(
                startDestination = Routes.HOME,
                route = Routes.MAIN_GRAPH
            ) {
                // Home
                androidx.navigation.compose.composable(Routes.HOME) {
                    HomeScreen(
                        onNavigateToProducts = {
                            navController.navigate(Routes.PRODUCT_LIST)
                        },
                        onNavigateToProfile = {
                            navController.navigate(Routes.PROFILE)
                        },
                        onNavigateToSettings = {
                            navController.navigate(Routes.SETTINGS)
                        },
                        onLogout = {
                            AuthManager.logout()
                            navController.navigate(Routes.AUTH_GRAPH) {
                                popUpTo(Routes.MAIN_GRAPH) { inclusive = true }
                            }
                        }
                    )
                }
                
                // Product list
                androidx.navigation.compose.composable(Routes.PRODUCT_LIST) {
                    ProductListScreen(
                        products = listOf("Product 1", "Product 2", "Product 3"),
                        onProductClick = { productId ->
                            navController.navigate("product_detail/${productId}")
                        },
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
                
                // Product detail
                androidx.navigation.compose.composable(Routes.PRODUCT_DETAIL) { backStackEntry ->
                    val productId = backStackEntry.arguments?.getString("productId") ?: ""
                    ProductDetailScreen(
                        productId = productId,
                        onAddToCart = {
                            navController.navigate(Routes.CART)
                        },
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
                
                // Profile - requires auth
                androidx.navigation.compose.composable(Routes.PROFILE) {
                    if (!AuthManager.isAuthenticated()) {
                        // Redirect to login
                        androidx.compose.runtime.LaunchedEffect(Unit) {
                            navController.navigate(Routes.AUTH_GRAPH) {
                                popUpTo(Routes.MAIN_GRAPH)
                            }
                        }
                        androidx.compose.material3.Text("Redirecting...")
                    } else {
                        ProfileScreen(
                            user = AuthManager.getCurrentUser() ?: "",
                            onNavigateBack = { navController.popBackStack() }
                        )
                    }
                }
                
                // Settings
                androidx.navigation.compose.composable(Routes.SETTINGS) {
                    SettingsScreen(
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
                
                // Cart
                androidx.navigation.compose.composable(Routes.CART) {
                    CartScreen(
                        onCheckout = {
                            navController.navigate(Routes.CHECKOUT_GRAPH)
                        },
                        onNavigateBack = { navController.popBackStack() }
                    )
                }
            }
            
            // === Checkout nested graph ===
            androidx.navigation.compose.navigation(
                startDestination = "shipping",
                route = Routes.CHECKOUT_GRAPH
            ) {
                androidx.navigation.compose.composable("shipping") {
                    ShippingScreen(
                        onContinue = { navController.navigate("payment") },
                        onCancel = { navController.popBackStack() }
                    )
                }
                
                androidx.navigation.compose.composable("payment") {
                    PaymentScreen(
                        onComplete = {
                            // Clear checkout and go to home
                            navController.navigate(Routes.HOME) {
                                popUpTo(Routes.MAIN_GRAPH) { inclusive = true }
                            }
                        }
                    )
                }
            }
        }
    }
    
    @Composable
    fun LoginScreen(
        onLoginSuccess: (String) -> Unit,
        onNavigateToRegister: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Login", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = { onLoginSuccess("user1") }) {
                androidx.compose.material3.Text("Login")
            }
            androidx.compose.material3.TextButton(onClick = onNavigateToRegister) {
                androidx.compose.material3.Text("Register")
            }
        }
    }
    
    @Composable
    fun RegisterScreen(
        onRegisterSuccess: (String) -> Unit,
        onNavigateBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Register")
            androidx.compose.material3.Button(onClick = { onRegisterSuccess("newuser") }) {
                androidx.compose.material3.Text("Register")
            }
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun HomeScreen(
        onNavigateToProducts: () -> Unit,
        onNavigateToProfile: () -> Unit,
        onNavigateToSettings: () -> Unit,
        onLogout: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier.fillMaxSize().padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Home", style = androidx.compose.material3.MaterialTheme.typography.headlineMedium)
            androidx.compose.material3.Spacer(modifier = Modifier.height(24.dp))
            androidx.compose.material3.Button(onClick = onNavigateToProducts) { androidx.compose.material3.Text("Products") }
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            androidx.compose.material3.Button(onClick = onNavigateToProfile) { androidx.compose.material3.Text("Profile") }
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            androidx.compose.material3.Button(onClick = onNavigateToSettings) { androidx.compose.material3.Text("Settings") }
            androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
            androidx.compose.material3.Button(onClick = onLogout) { androidx.compose.material3.Text("Logout") }
        }
    }
    
    @Composable
    fun ProductListScreen(
        products: List<String>,
        onProductClick: (String) -> Unit,
        onNavigateBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Products")
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun ProductDetailScreen(
        productId: String,
        onAddToCart: () -> Unit,
        onNavigateBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Product: ${productId}")
            androidx.compose.material3.Button(onClick = onAddToCart) { androidx.compose.material3.Text("Add to Cart") }
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun ProfileScreen(user: String, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Profile: ${user}")
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun SettingsScreen(onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Settings")
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun CartScreen(onCheckout: () -> Unit, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Cart")
            androidx.compose.material3.Button(onClick = onCheckout) { androidx.compose.material3.Text("Checkout") }
            androidx.compose.material3.Button(onClick = onNavigateBack) { androidx.compose.material3.Text("Back") }
        }
    }
    
    @Composable
    fun ShippingScreen(onContinue: () -> Unit, onCancel: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Shipping")
            androidx.compose.material3.Button(onClick = onContinue) { androidx.compose.material3.Text("Continue") }
            androidx.compose.material3.Button(onClick = onCancel) { androidx.compose.material3.Text("Cancel") }
        }
    }
    
    @Composable
    fun PaymentScreen(onComplete: () -> Unit) {
        androidx.compose.foundation.layout.Column(modifier = Modifier.fillMaxSize()) {
            androidx.compose.material3.Text("Payment")
            androidx.compose.material3.Button(onClick = onComplete) { androidx.compose.material3.Text("Complete") }
        }
    }
}
```

## Output Statement Results

**Nested Navigation:**
- Separate back stack per graph
- Clean task boundaries
- Reusable flow components
- Clear navigation scopes

**Global Actions:**
- Navigate from anywhere
- Centralized auth handling
- Error redirect patterns
- Deep link handling

**State Restoration:**
- SavedStateHandle for data
- Back stack entry state
- Process death handling
- Inter-screen data passing

**Advanced Patterns:**
- Auth-aware navigation
- Conditional routing
- Nested checkout flows
- State preservation

## Cross-References

- See: [01_Jetpack_Navigation_Basics.md](./01_Jetpack_Navigation_Basics.md)
- See: [02_Navigation_Compose.md](./02_Navigation_Compose.md)
- See: [03_Deep_Linking.md](./03_Deep_Linking.md)
- See: [04_Navigation_Arguments.md](./04_Navigation_Arguments.md)
- See: [../02_Navigation_UI/01_Bottom_Navigation.md](../02_Navigation_UI/01_Bottom_Navigation.md)
- See: [../02_Navigation_UI/04_Flow_Navigation.md](../02_Navigation_UI/04_Flow_Navigation.md)

## End of Advanced Navigation Patterns Guide