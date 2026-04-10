# Android Kotlin Conventions

## Learning Objectives

1. Understanding Android-specific Kotlin conventions
2. Following Android Kotlin coding standards
3. Applying Kotlin best practices in Android context
4. Using Kotlin extensions effectively
5. Implementing idiomatic Kotlin in Android projects

```kotlin
package com.kotlin.android
```

## Section 1: Coding Conventions

Android Kotlin Coding Conventions - Follow Android Kotlin style guide for consistent code.

```kotlin
object CodingConventions {
    
    // Naming conventions
    object Naming {
        // Classes: PascalCase
        class MyActivity : android.app.Activity()
        
        // Functions: camelCase
        fun calculateTotal() {}
        
        // Constants: UPPER_SNAKE_CASE
        const val MAX_RETRY_COUNT = 3
        
        // Variables/Properties: camelCase
        var currentUser: String? = null
        
        // Package names: lowercase
        package com.example.myapp.util
        
        // XML IDs: snake_case
        // android:id="@+id/user_name_text"
    }
    
    // Code organization
    object Organization {
        // Order in a class:
        // 1. Properties (val/var)
        // 2. Init block
        // 3. Secondary constructors
        // 4. Functions
        
        class ExampleClass {
            // 1. Properties
            private val name: String = "Default"
            var count: Int = 0
            
            // 2. Init block
            init {
                println("Initialized")
            }
            
            // 3. Functions
            fun doSomething() {}
            
            fun anotherMethod() {}
        }
    }
    
    // Import organization
    object Imports {
        // Order:
        // 1. android.*
        // 2. androidx.*
        // 3. com.google.*
        // 4. kotlin.*
        // 5. java.*
        // 6. javax.*
        // 7. other
        
        // Avoid wildcard imports except for kotlin.*
    }
}
```

## Section 2: Kotlin Android Extensions

Kotlin Android Extensions - Provides convenient access to Views and other Android components.

```kotlin
object AndroidExtensions {
    
    // View binding (recommended)
    // Activity with view binding:
    /*
    class MainActivity : AppCompatActivity() {
        private lateinit var binding: ActivityMainBinding
        
        override fun onCreate(savedInstanceState: Bundle?) {
            super.onCreate(savedInstanceState)
            binding = ActivityMainBinding.inflate(layoutInflater)
            setContentView(binding.root)
            
            // Access views directly
            binding.buttonSubmit.setOnClickListener { /* ... */ }
            binding.textViewTitle.text = "Hello"
        }
    }
    */
    
    // Fragment with view binding:
    /*
    class MyFragment : Fragment() {
        private var _binding: FragmentMyBinding? = null
        private val binding get() = _binding!!
        
        override fun onCreateView(
            inflater: LayoutInflater,
            container: ViewGroup?,
            savedInstanceState: Bundle?
        ): View {
            _binding = FragmentMyBinding.inflate(inflater, container, false)
            return binding.root
        }
        
        override fun onDestroyView() {
            super.onDestroyView()
            _binding = null
        }
    }
    */
    
    // Kotlin synthetic properties (deprecated, use view binding)
    // import kotlinx.android.synthetic.main.activity_main.*
    // Access: buttonSubmit.setOnClickListener { }
    
    // Context extensions
    fun Context.showToast(message: String, duration: Int = android.widget.Toast.LENGTH_SHORT) {
        android.widget.Toast.makeText(this, message, duration).show()
    }
    
    // Activity extensions
    fun Activity.showToast(message: String) {
        Toast.makeText(this, message, Toast.LENGTH_SHORT).show()
    }
    
    // View extensions
    fun View.visible() {
        visibility = android.view.View.VISIBLE
    }
    
    fun View.invisible() {
        visibility = android.view.View.INVISIBLE
    }
    
    fun View.gone() {
        visibility = android.view.View.GONE
    }
    
    fun View.setVisible(visible: Boolean) {
        visibility = if (visible) android.view.View.VISIBLE else android.view.View.GONE
    }
}
```

## Section 3: Idiomatic Kotlin Patterns

Idiomatic Kotlin Patterns for Android - Common patterns and idioms for Android development.

```kotlin
class IdiomaticPatterns {
    
    // Scope functions - when to use each
    object ScopeFunctions {
        
        // run - Execute code block, return result
        fun runExample(): String {
            val result = "test".run {
                uppercase()
            }
            return result
        }
        
        // let - Execute block on non-null object
        fun letExample(user: String?): String {
            return user?.let {
                "User: $it"
            } ?: "No user"
        }
        
        // apply - Configure object, return it
        fun applyExample(): android.widget.Button {
            return android.widget.Button(null).apply {
                text = "Click Me"
                setTextColor(0xFF000000.toInt())
            }
        }
        
        // also - Additional actions, return object
        fun alsoExample(): String {
            return "Hello".also { 
                println("Value: $it")
            }
        }
        
        // with - Execute block on object, return result
        fun withExample(): Int {
            return with(listOf(1, 2, 3)) {
                size
            }
        }
    }
    
    // Builder pattern alternative
    fun createNotification(): android.app.Notification {
        // Using apply for builder-like pattern
        return android.app.Notification.Builder(null).apply {
            setSmallIcon(android.R.drawable.ic_dialog_info)
            setContentTitle("Title")
            setContentText("Content")
            setPriority(android.app.Notification.PRIORITY_DEFAULT)
        }.build()
    }
    
    // Lazy initialization
    class LazyInitExample {
        val heavyResource: String by lazy {
            println("Initializing...")
            "Resource loaded"
        }
        
        // Thread-safe lazy
        val threadSafeResource: String by lazy(LazyThreadSafetyMode.SYNCHRONIZED) {
            "Thread-safe resource"
        }
    }
    
    // Sealed classes for UI state
    sealed class UiState {
        object Loading : UiState()
        data class Success(val data: String) : UiState()
        data class Error(val message: String) : UiState()
    }
    
    // Handle state with when
    fun handleState(state: UiState): String {
        return when (state) {
            is UiState.Loading -> "Loading..."
            is UiState.Success -> state.data
            is UiState.Error -> "Error: ${state.message}"
        }
    }
}
```

## Section 4: Android Kotlin Best Practices

Android Kotlin Best Practices - Recommended practices for Android development.

```kotlin
object BestPractices {
    
    // 1. Use ViewModel + LiveData/StateFlow
    // Instead of handling UI state in Activity/Fragment
    class GoodViewModel : androidx.lifecycle.ViewModel() {
        private val _state = androidx.lifecycle.MutableLiveData<String>()
        val state: androidx.lifecycle.LiveData<String> = _state
        
        fun loadData() {
            _state.value = "Loading"
            // Simulate loading
            _state.postValue("Data loaded")
        }
    }
    
    // 2. Use coroutines for async operations
    suspend fun fetchData(): String {
        return kotlinx.coroutines.delay(1000)
        "Data fetched"
    }
    
    // 3. Use proper exception handling
    fun safeOperation() {
        try {
            // Operation that might fail
        } catch (e: Exception) {
            // Handle gracefully
            println("Error: ${e.message}")
        } finally {
            // Clean up if needed
        }
    }
    
    // 4. Use nullable types properly
    fun processUser(user: User?) {
        // Safe access
        user?.let {
            // Process only if user exists
            println("Processing: ${it.name}")
        }
        
        // With default
        val displayName = user?.name ?: "Guest"
    }
    
    // 5. Use data classes for models
    data class User(val id: Int, val name: String, val email: String)
    
    // 6. Use companion object for constants
    class ApiClient {
        companion object {
            const val BASE_URL = "https://api.example.com"
            const val TIMEOUT = 30L
        }
    }
    
    // 7. Use extension functions for utilities
    fun String.isValidEmail(): Boolean {
        return android.util.Patterns.EMAIL_ADDRESS.matcher(this).matches()
    }
    
    // 8. Use sealed classes for results
    sealed class Result<out T> {
        data class Success<T>(val data: T) : Result<T>()
        data class Error(val exception: Exception) : Result<Nothing>()
        object Loading : Result<Nothing>()
    }
}
```

## Section 5: Android-Specific Patterns

Android-Specific Kotlin Patterns - Patterns specifically useful for Android development.

```kotlin
object AndroidPatterns {
    
    // Click listener pattern
    interface OnClickListener {
        fun onClick(view: android.view.View)
    }
    
    // Single abstract method (SAM) conversion
    fun setButtonClick(button: android.widget.Button) {
        button.setOnClickListener { view ->
            println("Clicked: ${view.id}")
        }
    }
    
    // Parcelable implementation (using parcelize plugin)
    /*
    @Parcelize
    data class User(val name: String, val age: Int) : Parcelable
    */
    
    // Manual Parcelable
    class UserParcelable(val name: String, val age: Int) : android.os.Parcelable {
        constructor(source: android.os.Parcel) : this(
            source.readString() ?: "",
            source.readInt()
        )
        
        override fun describeContents(): Int = 0
        
        override fun writeToParcel(dest: android.os.Parcel, flags: Int) {
            dest.writeString(name)
            dest.writeInt(age)
        }
        
        companion object CREATOR : android.os.Parcelable.Creator<UserParcelable> {
            override fun createFromParcel(source: android.os.Parcel): UserParcelable {
                return UserParcelable(source)
            }
            
            override fun newArray(size: Int): Array<UserParcelable?> {
                return arrayOfNulls(size)
            }
        }
    }
    
    // Intent extension functions
    fun android.content.Intent.putExtra(key: String, value: String): android.content.Intent {
        putExtra(key, value)
        return this
    }
    
    // Bundle extensions
    fun android.os.Bundle.getStringOrDefault(key: String, default: String): String {
        return getString(key) ?: default
    }
    
    // Coroutine scope in Activity
    class CoroutineActivity : android.app.Activity(), androidx.lifecycle.CoroutineScope {
        override val coroutineContext: kotlinx.coroutines.CoroutineContext
            get() = kotlinx.coroutines.Dispatchers.Main + kotlinx.coroutines.Job()
        
        fun launchInScope(block: suspend () -> Unit) {
            launch { block() }
        }
        
        override fun onDestroy() {
            super.onDestroy()
            coroutineContext[kotlinx.coroutines.Job]?.cancel()
        }
    }
}
```

## Common Pitfalls and Solutions

Pitfall 1: Memory leaks with Context
Solution:
- Use applicationContext for long-lived objects
- Use weak references when needed
- Clear references in onDestroy

Pitfall 2: ViewModel scope issues
Solution:
- Use viewModelScope (from lifecycle-view-ktx)
- Cancel coroutines properly
- Don't store UI state references

Pitfall 3: Wrong dispatcher usage
Solution:
- Use Dispatchers.Main for UI
- Use Dispatchers.IO for file/DB operations
- Use Dispatchers.Default for CPU work

Pitfall 4: Mutable state in UI
Solution:
- Use immutable state
- Use LiveData/StateFlow
- Recreate views on state changes

Pitfall 5: Incorrect null handling
Solution:
- Use safe calls consistently
- Avoid !! operator
- Provide defaults with elvis

## Best Practices

1. Use ViewModel for UI state
2. Use LiveData/StateFlow for observable data
3. Use Coroutines for async operations
4. Use proper context for operations
5. Follow single responsibility
6. Use dependency injection
7. Handle lifecycle properly
8. Use proper thread dispatchers
9. Avoid memory leaks
10. Test thoroughly

## Troubleshooting Guide

Issue: " lateinit has not been initialized"
Steps:
1. Ensure initialization before access
2. Use lazy instead of lateinit
3. Add null check

Issue: Coroutine scope not found
Steps:
1. Add lifecycle-view-ktx dependency
2. Use viewModelScope
3. Create proper CoroutineScope

Issue: View not found
Steps:
1. Check view ID in XML
2. Ensure binding is inflated
3. Check view is in correct layout

## Advanced Tips and Tricks

Tip 1: Use inline functions for performance
- Reduces overhead
- Enables reified types

Tip 2: Use type aliases
- Improve readability
- Create domain types

Tip 3: Use reified type parameters
- Get type information at runtime
- Useful for generic operations

Tip 4: Use DSL builders
- Create expressive APIs
- Type-safe builders

Tip 5: Use expect/actual
- Platform-specific implementations
- Platform channels

## Example 1: Conventional Kotlin Activity

Conventional Kotlin Activity Example - Following all Android Kotlin conventions.

```kotlin
class ConventionalActivity : android.app.Activity() {
    
    // Properties with proper initialization
    private var userName: String? = null
    private val userId: Int = 0
    
    // Lazy initialization for heavy resources
    private val preferences by lazy {
        getSharedPreferences("prefs", android.content.Context.MODE_PRIVATE)
    }
    
    // View binding (recommended approach)
    // private lateinit var binding: ActivityConventionalBinding
    
    override fun onCreate(savedInstanceState: android.os.Bundle?) {
        super.onCreate(savedInstanceState)
        // setContentView(R.layout.activity_conventional)
        
        // Initialize user name safely
        userName = savedInstanceState?.getString("user_name")
        
        // Use safe call
        userName?.let {
            println("Welcome back: $it")
        }
    }
    
    override fun onSaveInstanceState(outState: android.os.Bundle) {
        super.onSaveInstanceState(outState)
        
        // Save state safely
        userName?.let {
            outState.putString("user_name", it)
        }
    }
    
    // Single responsibility - button click handler
    fun onSubmitClick(view: android.view.View) {
        // Handle click
        showToast("Submit clicked")
    }
    
    // Extension function for toast
    private fun showToast(message: String) {
        android.widget.Toast.makeText(this, message, android.widget.Toast.LENGTH_SHORT).show()
    }
    
    companion object {
        // Constants in companion object
        private const val KEY_USER_NAME = "user_name"
    }
}
```

## Example 2: Proper Lifecycle Handling

Proper Lifecycle Handling Example - Demonstrates proper Kotlin coroutine lifecycle management.

```kotlin
class LifecycleAwareActivity : android.app.Activity() {
    
    // Using viewModelScope equivalent with CoroutineScope
    private val lifecycleScope = androidx.lifecycle.lifecycleScope
    
    private var data: String? = null
    
    override fun onCreate(savedInstanceState: android.os.Bundle?) {
        super.onCreate(savedInstanceState)
        
        // Launch coroutine with lifecycle awareness
        lifecycleScope.launchWhenStarted {
            try {
                // Safe to update UI here
                loadData()
            } catch (e: Exception) {
                println("Error: ${e.message}")
            }
        }
    }
    
    private suspend fun loadData() {
        withContext(Dispatchers.IO) {
            // Load data
            data = "Loaded data"
        }
        
        // Update UI on main thread
        withContext(Dispatchers.Main) {
            println("Data loaded: $data")
        }
    }
    
    // Proper cleanup
    override fun onDestroy() {
        super.onDestroy()
        // Cancellation is automatic with lifecycleScope
    }
    
    // Extension for lifecycle-aware coroutines
    private fun androidx.lifecycle.LifecycleOwner.launchWhenStarted(
        block: suspend () -> Unit
    ) {
        lifecycleScope.launchWhenStarted { block() }
    }
    
    suspend fun <T> withContext(
        context: kotlinx.coroutines.CoroutineContext,
        block: suspend () -> T
    ): T = kotlinx.coroutines.withContext(context, block)
}
```

## Example 3: Idiomatic State Management

Idiomatic State Management Example - Using sealed classes and StateFlow for state management.

```kotlin
class StateManagementExample {
    
    // State classes using sealed classes
    sealed class UiState {
        object Idle : UiState()
        object Loading : UiState()
        data class Success(val message: String) : UiState()
        data class Error(val error: String) : UiState()
    }
    
    // ViewModel with StateFlow
    class MyViewModel : androidx.lifecycle.ViewModel() {
        
        private val _uiState = kotlinx.coroutines.flow.MutableStateFlow<UiState>(UiState.Idle)
        val uiState: kotlinx.coroutines.flow.StateFlow<UiState> = _uiState
        
        fun loadData() {
            viewModelScope.launch {
                _uiState.value = UiState.Loading
                
                try {
                    // Simulate API call
                    kotlinx.coroutines.delay(1000)
                    _uiState.value = UiState.Success("Data loaded successfully!")
                } catch (e: Exception) {
                    _uiState.value = UiState.Error(e.message ?: "Unknown error")
                }
            }
        }
        
        private val viewModelScope = androidx.lifecycle.viewModelScope
    }
    
    // Fragment collecting state
    class MyFragment : android.app.Fragment() {
        
        private val viewModel: MyViewModel by viewModels()
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Collect state flow
            viewLifecycleOwner.lifecycleScope.launch {
                viewLifecycleOwner.repeatOnLifecycle(androidx.lifecycle.Lifecycle.State.STARTED) {
                    viewModel.uiState.collect { state ->
                        handleState(state)
                    }
                }
            }
        }
        
        private fun handleState(state: UiState) {
            when (state) {
                is UiState.Idle -> { /* Show idle UI */ }
                is UiState.Loading -> { /* Show loading */ }
                is UiState.Success -> { /* Show data */ }
                is UiState.Error -> { /* Show error */ }
            }
        }
        
        // Extension to get ViewModel
        private inline fun <reified T : androidx.lifecycle.ViewModel> Fragment.viewModels(): T {
            return androidx.lifecycle.viewmodel.viewModels(T::class.java).get()
        }
    }
}
```

## Output Statement Results

Android Kotlin Conventions Applied:
- Naming: camelCase, PascalCase, UPPER_SNAKE_CASE
- Code organization: proper class member order
- Imports: organized by package type

Android Extensions:
- View binding: proper initialization and cleanup
- Context extensions: toast, etc.
- View extensions: visible/invisible/gone

Idiomatic Patterns:
- Scope functions: run, let, apply, also, with
- Lazy initialization: proper usage
- Sealed classes: for state representation

Best Practices:
- ViewModel + StateFlow
- Coroutines for async
- Proper exception handling
- Nullable type safety

Lifecycle Management:
- Lifecycle-aware coroutines
- Proper cleanup in onDestroy
- StateFlow for state

## Cross-References

See: 01_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md
See: 01_Kotlin_Basics_for_Android/03_Type_System_and_Collections.md
See: 01_Kotlin_Basics_for_Android/04_Coroutines_Basics.md
See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md

---

*End of Android Kotlin Conventions Guide*