# Navigation Arguments

## Learning Objectives

1. Understanding navigation arguments
2. Passing data between destinations
3. Using Safe Args for type-safe navigation
4. Creating complex argument types
5. Handling optional arguments
6. Managing argument defaults
7. Using bundles for argument passing
8. Implementing custom argument type converters

## Section 1: Navigation Arguments Overview

Navigation arguments allow passing data between destinations. Arguments are defined in the navigation graph and passed via bundles or Safe Args.

```kotlin
/**
 * Navigation Arguments Overview
 * 
 * Arguments enable passing data to destinations:
 * - Simple types: String, Int, Boolean
 * - Complex types: Serializable, Parcelable, Enum
 * - Custom types: With type converters
 * 
 * Two approaches:
 * - Bundle-based: Direct bundle manipulation
 * - Safe Args: Generated direction classes
 */
object NavigationArgumentsOverview {
    
    // Argument types supported
    object ArgumentTypes {
        // Basic types
        const val STRING = "string"
        const val INT = "integer"
        const val BOOLEAN = "boolean"
        const val LONG = "long"
        const val FLOAT = "float"
        
        // Reference types
        const val REFERENCE = "reference"
        
        // Complex types
        const val PARCELABLE = "parcelable"
        const val SERIALIZABLE = "serializable"
        
        // Custom types
        // Using NavType with custom serializer
    }
    
    // XML argument definition
    fun getXmlArguments(): String {
        return """
<!-- String argument with default -->
<argument
    android:name="itemId"
    app:argType="string"
    app:nullable="false"
    android:defaultValue="" />

<!-- Integer argument -->
<argument
    android:name="quantity"
    app:argType="integer"
    android:defaultValue="0" />

<!-- Boolean argument -->
<argument
    android:name="isEditable"
    app:argType="boolean"
    android:defaultValue="false" />

<!-- Long argument -->
<argument
    android:name="timestamp"
    app:argType="long"
    android:defaultValue="0L" />

<!-- Float argument -->
<argument
    android:name="price"
    app:argType="float"
    android:defaultValue="0.0" />

<!-- Enum argument -->
<argument
    android:name="status"
    app:argType="com.example.Status"
    app:nullable="false"
    android:defaultValue="PENDING" />

<!-- Custom parcelable argument -->
<argument
    android:name="user"
    app:argType="com.example.User" />
        """.trimIndent()
    }
}
```

## Section 2: Bundle-Based Argument Passing

Traditional approach using bundles for passing arguments to destinations.

```kotlin
/**
 * Bundle-Based Argument Passing
 * 
 * Using Bundle to pass arguments between destinations.
 * This is the standard approach without Safe Args.
 */
class BundleArguments {
    
    // Fragment navigating with bundle arguments
    class SourceFragment : androidx.fragment.app.Fragment() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        fun navigateWithBundle() {
            // Create bundle with arguments
            val args = android.os.Bundle()
            
            // Add string argument
            args.putString("itemId", "123")
            
            // Add integer argument
            args.putInt("quantity", 5)
            
            // Add boolean argument
            args.putBoolean("isEditable", true)
            
            // Add long argument
            args.putLong("timestamp", System.currentTimeMillis())
            
            // Add float argument
            args.putFloat("price", 19.99f)
            
            // Navigate with bundle
            // Bundle is passed as second parameter
            navController.navigate(R.id.action_source_to_destination, args)
        }
        
        fun navigateWithMultipleArgs() {
            // Build bundle for complex navigation
            val args = android.os.Bundle().apply {
                putString("productId", "P123")
                putString("productName", "Widget")
                putDouble("price", 29.99)
                putBoolean("inStock", true)
                putInt("categoryId", 1)
            }
            
            navController.navigate(R.id.action_source_to_product, args)
        }
        
        fun navigateWithObject() {
            // Create parcelable object
            val user = User("1", "John", "john@example.com")
            
            // Create bundle
            val args = android.os.Bundle().apply {
                // Put parcelable
                putParcelable("user", user)
                
                // Or serialize as JSON
                val userJson = """{"id":"1","name":"John","email":"john@example.com"}"""
                putString("userJson", userJson)
            }
            
            navController.navigate(R.id.action_source_to_profile, args)
        }
    }
    
    // Fragment receiving bundle arguments
    class DestinationFragment : androidx.fragment.app.Fragment() {
        
        private var itemId: String = ""
        private var quantity: Int = 0
        private var isEditable: Boolean = false
        private var price: Float = 0f
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Retrieve arguments from bundle
            // Arguments are available in fragment's arguments bundle
            arguments?.let { args ->
                itemId = args.getString("itemId") ?: ""
                quantity = args.getInt("quantity", 0)
                isEditable = args.getBoolean("isEditable", false)
                price = args.getFloat("price", 0f)
            }
            
            // Display received data
            val textView = view.findViewById<android.widget.TextView>(R.id.text_view)
            textView.text = "Item: ${itemId}, Qty: ${quantity}, Editable: ${isEditable}, Price: ${price}"
        }
        
        // Safe way to get arguments with defaults
        private fun getArgumentsSafely(): Bundle? {
            return arguments ?: return null
        }
        
        private fun readArguments() {
            val args = getArgumentsSafely() ?: return
            
            // Get with default values
            val itemId = args.getString("itemId", "default")
            val quantity = args.getInt("quantity", 1)
            val isEditable = args.getBoolean("isEditable", false)
            
            println("Received: itemId=${itemId}, quantity=${quantity}")
        }
    }
    
    // Parcelable data class
    class User(val id: String, val name: String, val email: String) : android.os.Parcelable {
        
        constructor(parcel: android.os.Parcel) : this(
            parcel.readString() ?: "",
            parcel.readString() ?: "",
            parcel.readString() ?: ""
        )
        
        override fun writeToParcel(parcel: android.os.Parcel, flags: Int) {
            parcel.writeString(id)
            parcel.writeString(name)
            parcel.writeString(email)
        }
        
        override fun describeContents(): Int = 0
        
        companion object CREATOR : android.os.Parcelable.Creator<User> {
            override fun createFromParcel(parcel: android.os.Parcel): User {
                return User(parcel)
            }
            
            override fun newArray(size: Int): Array<User?> {
                return arrayOfNulls(size)
            }
        }
    }
}
```

## Section 3: Safe Args

Safe Args generates type-safe classes for navigation, providing compile-time verification and better IDE support.

```kotlin
/**
 * Safe Args Implementation
 * 
 * Safe Args generates:
 * - *Directions classes for navigation actions
 * - *Args classes for argument classes
 * - BuildConfig field for navigation IDs
 * 
 * Benefits:
 * - Type-safe argument passing
 * - Compile-time verification
 * - Refactor-friendly navigation
 * - Better IDE support
 */
class SafeArgsImplementation {
    
    // Build configuration for Safe Args
    // SafeArgs plugin generates classes
    fun getBuildConfig(): String {
        return """
// Project build.gradle
plugins {
    id 'com.android.application'
    id 'org.jetbrains.kotlin.android'
    id 'androidx.navigation.safeargs.kotlin'
}

// Navigation graph (nav_graph.xml)
<navigation xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/nav_graph"
    app:startDestination="@id/homeFragment">
    
    <fragment
        android:id="@+id/homeFragment"
        android:name="com.example.HomeFragment"
        android:label="Home">
        
        <action
            android:id="@+id/action_home_to_detail"
            app:destination="@id/detailFragment" />
    </fragment>
    
    <fragment
        android:id="@+id/detailFragment"
        android:name="com.example.DetailFragment"
        android:label="Detail">
        
        <argument
            android:name="itemId"
            app:argType="string"
            android:defaultValue="" />
        
        <argument
            android:name="quantity"
            app:argType="integer"
            android:defaultValue="1" />
    </fragment>
</navigation>
        """.trimIndent()
    }
    
    // Using generated direction classes
    class HomeFragment : androidx.fragment.app.Fragment() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        fun navigateWithSafeArgs() {
            // Navigate with Safe Args
            // Generates HomeFragmentDirections
            val directions = HomeFragmentDirections.actionHomeToDetail(
                itemId = "123",
                quantity = 5
            )
            
            // Navigate
            navController.navigate(directions)
        }
        
        // Alternative using NavOptions
        fun navigateWithOptions() {
            // Generated NavOptions
            val directions = HomeFragmentDirections.actionHomeToDetail(
                itemId = "123",
                quantity = 5
            )
            
            val navOptions = androidx.navigation.NavOptions.Builder()
                .setEnterAnim(R.anim.slide_in_right)
                .setExitAnim(R.anim.slide_out_left)
                .build()
            
            navController.navigate(directions, navOptions)
        }
    }
    
    // Using generated args classes
    class DetailFragment : androidx.fragment.app.Fragment() {
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Get arguments using generated Args class
            // This provides type-safe access to arguments
            val args = DetailFragmentArgs.fromBundle(arguments!!)
            
            // Access properties directly
            val itemId = args.itemId
            val quantity = args.quantity
            
            // Display data
            val textView = view.findViewById<android.widget.TextView>(R.id.text_view)
            textView.text = "Item: ${itemId}, Quantity: ${quantity}"
        }
        
        // Navigate back with result
        fun navigateBackWithResult() {
            // Set result using previous directions
            // This allows passing data back to source
            val navController = androidx.navigation.fragment.NavHostFragment
                .findFragmentById(R.id.nav_host_fragment)
                .navController
            
            // Use setExistingAddOrPopBackStack
            // This is useful for passing results
            navController.previousBackStackEntry?.savedStateHandle?.set(
                "result_key",
                "result_value"
            )
            
            navController.popBackStack()
        }
    }
    
    // Complex argument with Safe Args
    class ProductDetailFragment : androidx.fragment.app.Fragment() {
        
        private lateinit var navController: androidx.navigation.NavController
        
        fun navigateWithComplexArgs() {
            // Navigate with complex argument as JSON string
            val product = Product(
                id = "P123",
                name = "Widget",
                price = 29.99,
                inStock = true
            )
            
            // Serialize product to JSON
            val gson = com.google.gson.Gson()
            val productJson = gson.toJson(product)
            
            // Navigate with JSON argument
            val directions = ProductDetailFragmentDirections.actionListToDetail(
                productId = "P123",
                productJson = productJson
            )
            
            navController.navigate(directions)
        }
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Parse JSON argument
            val args = ProductDetailFragmentArgs.fromBundle(arguments!!)
            val productJson = args.productJson
            
            val gson = com.google.gson.Gson()
            val product = gson.fromJson(productJson, Product::class.java)
            
            println("Product: ${product.name}, Price: ${product.price}")
        }
        
        data class Product(
            val id: String,
            val name: String,
            val price: Double,
            val inStock: Boolean
        )
    }
}
```

## Section 4: Navigation Compose Arguments

Arguments work differently in Navigation Compose but follow similar patterns.

```kotlin
/**
 * Navigation Compose Arguments
 * 
 * Arguments in Navigation Compose are defined using:
 * - navArgument in composable definition
 * - NavType for argument type
 * - defaultValue for optional arguments
 */
class NavigationComposeArguments {
    
    // Simple arguments with compose
    @Composable
    fun SimpleArguments() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "home"
        ) {
            // Home - no arguments
            androidx.navigation.compose.composable("home") {
                HomeScreen(
                    onNavigateToDetail = { itemId ->
                        navController.navigate("detail/${itemId}")
                    }
                )
            }
            
            // Detail - with argument
            androidx.navigation.compose.composable(
                route = "detail/{itemId}",
                arguments = listOf(
                    // Define itemId argument
                    androidx.navigation.compose.navArgument("itemId") {
                        // Type: String
                        type = androidx.navigation.compose.NavType.StringType
                        
                        // Default value if not provided
                        defaultValue = "default"
                        
                        // Nullable
                        nullable = false
                    }
                )
            ) { backStackEntry ->
                // Retrieve argument
                val itemId = backStackEntry.arguments?.getString("itemId") ?: ""
                
                DetailScreen(
                    itemId = itemId,
                    onNavigateBack = { navController.popBackStack() }
                )
            }
        }
    }
    
    // Multiple arguments
    @Composable
    fun MultipleArguments() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "product_list"
        ) {
            // Product detail with multiple arguments
            androidx.navigation.compose.composable(
                route = "product/{productId}/category/{categoryId}",
                arguments = listOf(
                    androidx.navigation.compose.navArgument("productId") {
                        type = androidx.navigation.compose.NavType.StringType
                        defaultValue = ""
                    },
                    androidx.navigation.compose.navArgument("categoryId") {
                        type = androidx.navigation.compose.NavType.IntType
                        defaultValue = 0
                    }
                )
            ) { backStackEntry ->
                val productId = backStackEntry.arguments?.getString("productId") ?: ""
                val categoryId = backStackEntry.arguments?.getInt("categoryId") ?: 0
                
                ProductDetailScreen(productId, categoryId)
            }
        }
    }
    
    // Complex arguments using serializable
    @Composable
    fun ComplexArguments() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Serializable data class
        @kotlinx.serialization.Serializable
        data class UserArgs(
            val userId: String,
            val username: String,
            val email: String
        )
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = "list"
        ) {
            // User profile with serializable argument
            androidx.navigation.compose.composable(
                route = "user/{userArgs}",
                arguments = listOf(
                    androidx.navigation.compose.navArgument("userArgs") {
                        type = androidx.navigation.compose.NavType.StringType
                        defaultValue = ""
                    }
                )
            ) { backStackEntry ->
                // Get encoded argument
                val encodedArgs = backStackEntry.arguments?.getString("userArgs") ?: ""
                
                // Decode if needed
                val userJson = java.net.URLDecoder.decode(encodedArgs, "UTF-8")
                
                // Parse JSON
                val userArgs = try {
                    kotlinx.serialization.json.Json.decodeFromString<UserArgs>(userJson)
                } catch (e: Exception) {
                    null
                }
                
                if (userArgs != null) {
                    androidx.compose.material3.Text("User: ${userArgs.username}")
                }
            }
        }
    }
    
    // Typed arguments using all types
    @Composable
    fun AllArgumentTypes() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        // Argument definitions for all types
        listOf(
            // String type - most common
            androidx.navigation.compose.navArgument("stringArg") {
                type = androidx.navigation.compose.NavType.StringType
                defaultValue = "default"
            },
            
            // Int type
            androidx.navigation.compose.navArgument("intArg") {
                type = androidx.navigation.compose.NavType.IntType
                defaultValue = 0
            },
            
            // Boolean type
            androidx.navigation.compose.navArgument("boolArg") {
                type = androidx.navigation.compose.NavType.BoolType
                defaultValue = false
            },
            
            // Long type
            androidx.navigation.compose.navArgument("longArg") {
                type = androidx.navigation.compose.NavType.LongType
                defaultValue = 0L
            },
            
            // Float type
            androidx.navigation.compose.navArgument("floatArg") {
                type = androidx.navigation.compose.NavType.FloatType
                defaultValue = 0.0f
            },
            
            // Serializable type
            androidx.navigation.compose.navArgument("serializableArg") {
                type = androidx.navigation.compose.NavType.SerializableType(
                    MySerializable::class.java
                )
                defaultValue = null
            }
        )
    }
    
    data class MySerializable(val value: String) : java.io.Serializable
    
    @Composable
    fun HomeScreen(onNavigateToDetail: (String) -> Unit) {
        androidx.compose.material3.Button(onClick = { onNavigateToDetail("123") }) {
            androidx.compose.material3.Text("Go to Detail")
        }
    }
    
    @Composable
    fun DetailScreen(itemId: String, onNavigateBack: () -> Unit) {
        androidx.compose.foundation.layout.Column {
            androidx.compose.material3.Text("Item: ${itemId}")
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
    
    @Composable
    fun ProductDetailScreen(productId: String, categoryId: Int) {
        androidx.compose.material3.Text("Product: ${productId}, Category: ${categoryId}")
    }
}
```

## Example: Complete Navigation with Arguments

Complete example using Safe Args with complete argument flow.

```kotlin
/**
 * Complete Navigation Arguments Example
 * 
 * Full implementation with Safe Args and complex arguments.
 */
class CompleteNavigationArguments {
    
    // Product data class
    data class Product(
        val id: String,
        val name: String,
        val price: Double,
        val category: String,
        val inStock: Boolean
    )
    
    // Routes object
    object Routes {
        const val HOME = "home"
        const val PRODUCT_LIST = "product_list"
        const val PRODUCT_DETAIL = "product_detail/{productId}/{quantity}"
        
        fun productDetail(productId: String, quantity: Int) = 
            "product_detail/${productId}/${quantity}"
    }
    
    // Main navigation
    @Composable
    fun MainNavigation() {
        val navController = androidx.navigation.compose.rememberNavController()
        
        androidx.navigation.compose.NavHost(
            navController = navController,
            startDestination = Routes.HOME
        ) {
            // Home
            androidx.navigation.compose.composable(Routes.HOME) {
                HomeScreen(
                    onNavigateToProductList = {
                        navController.navigate(Routes.PRODUCT_LIST)
                    }
                )
            }
            
            // Product list
            androidx.navigation.compose.composable(Routes.PRODUCT_LIST) {
                ProductListScreen(
                    products = listOf(
                        Product("P1", "Widget", 19.99, "Widget", true),
                        Product("P2", "Gadget", 29.99, "Gadget", false)
                    ),
                    onProductClick = { product, quantity ->
                        navController.navigate(
                            Routes.productDetail(product.id, quantity)
                        )
                    }
                )
            }
            
            // Product detail with arguments
            androidx.navigation.compose.composable(
                route = Routes.PRODUCT_DETAIL,
                arguments = listOf(
                    androidx.navigation.compose.navArgument("productId") {
                        type = androidx.navigation.compose.NavType.StringType
                    },
                    androidx.navigation.compose.navArgument("quantity") {
                        type = androidx.navigation.compose.NavType.IntType
                        defaultValue = 1
                    }
                )
            ) { backStackEntry ->
                val productId = backStackEntry.arguments?.getString("productId") ?: ""
                val quantity = backStackEntry.arguments?.getInt("quantity") ?: 1
                
                ProductDetailScreen(
                    productId = productId,
                    quantity = quantity,
                    onNavigateBack = { navController.popBackStack() }
                )
            }
        }
    }
    
    @Composable
    fun HomeScreen(onNavigateToProductList: () -> Unit) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            horizontalAlignment = androidx.compose.ui.Alignment.CenterHorizontally,
            verticalArrangement = androidx.compose.foundation.layout.Arrangement.Center
        ) {
            androidx.compose.material3.Text("Home")
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = onNavigateToProductList) {
                androidx.compose.material3.Text("View Products")
            }
        }
    }
    
    @Composable
    fun ProductListScreen(
        products: List<Product>,
        onProductClick: (Product, Int) -> Unit
    ) {
        androidx.compose.foundation.lazy.LazyColumn {
            items(products.size) { index ->
                val product = products[index]
                androidx.compose.material3.Card(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(8.dp)
                ) {
                    androidx.compose.foundation.layout.Column(
                        modifier = Modifier.padding(16.dp)
                    ) {
                        androidx.compose.material3.Text(
                            product.name,
                            style = androidx.compose.material3.MaterialTheme.typography.titleMedium
                        )
                        androidx.compose.material3.Text("Price: $${product.price}")
                        androidx.compose.material3.Text("In Stock: ${product.inStock}")
                        androidx.compose.material3.Spacer(modifier = Modifier.height(8.dp))
                        androidx.compose.material3.Button(
                            onClick = { onProductClick(product, 1) }
                        ) {
                            androidx.compose.material3.Text("View Detail")
                        }
                    }
                }
            }
        }
    }
    
    @Composable
    fun ProductDetailScreen(
        productId: String,
        quantity: Int,
        onNavigateBack: () -> Unit
    ) {
        androidx.compose.foundation.layout.Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp)
        ) {
            androidx.compose.material3.Text(
                "Product ID: ${productId}",
                style = androidx.compose.material3.MaterialTheme.typography.headlineSmall
            )
            androidx.compose.material3.Text("Quantity: ${quantity}")
            androidx.compose.material3.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Button(onClick = onNavigateBack) {
                androidx.compose.material3.Text("Back")
            }
        }
    }
}
```

## Output Statement Results

**Argument Types:**
- String: Most common for IDs and text
- Int: For quantities and indices
- Boolean: For flags and settings
- Long: For timestamps
- Float: For decimal values

**Safe Args Benefits:**
- Type-safe navigation methods
- IDE autocomplete support
- Refactoring support
- Default value handling
- Generated classes

**Argument Patterns:**
- Simple: Route parameter in path
- Complex: JSON encoded string
- Object: Parcelable/Serializable
- Query: URL query parameters

**Navigation Flow:**
- Put args: Bundle or directions
- Navigate: With args
- Retrieve: In destination
- Optional: Return results

## Cross-References

- See: [01_Jetpack_Navigation_Basics.md](./01_Jetpack_Navigation_Basics.md)
- See: [02_Navigation_Compose.md](./02_Navigation_Compose.md)
- See: [03_Deep_Linking.md](./03_Deep_Linking.md)
- See: [../02_Navigation_UI/03_Tab_Navigation.md](../02_Navigation_UI/03_Tab_Navigation.md)

## End of Navigation Arguments Guide