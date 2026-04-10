# LEARNING OBJECTIVES

1. Understanding extension functions and properties
2. Implementing custom delegates
3. Using built-in delegates
4. Creating type-safe builders with extensions
5. Applying extensions and delegates in Android

```kotlin
package com.kotlin.extensions
```

---

## SECTION 1: EXTENSION FUNCTIONS

```kotlin
/**
 * Extension Functions
 * 
 * Extension functions allow adding new functions to existing classes
 * without modifying their source code.
 */
object ExtensionFunctions {
    
    // Basic extension function
    fun String.addExclamation(): String = "$this!"
    
    // Extension with parameters
    fun String.repeat(times: Int): String {
        return (1..times).joinToString("") { this }
    }
    
    // Extension on nullable type
    fun String?.orEmpty(): String = this ?: ""
    
    // Extension on specific type
    fun Int.isEven(): Boolean = this % 2 == 0
    
    // Extension with generic type
    fun <T> List<T>.secondOrNull(): T? = if (size >= 2) this[1] else null
    
    // Extension function for Context
    fun Context.showToast(message: String, duration: Int = android.widget.Toast.LENGTH_SHORT) {
        android.widget.Toast.makeText(this, message, duration).show()
    }
    
    // Extension for View
    fun View.visible() {
        visibility = android.view.View.VISIBLE
    }
    
    fun View.gone() {
        visibility = android.view.View.GONE
    }
    
    fun View.invisible() {
        visibility = android.view.View.INVISIBLE
    }
    
    // Usage
    fun usage() {
        val greeting = "Hello".addExclamation()  // "Hello!"
        val repeated = "ab".repeat(3)  // "ababab"
        val even = 4.isEven()  // true
    }
}
```

---

## SECTION 2: EXTENSION PROPERTIES

```kotlin
/**
 * Extension Properties
 * 
 * Extension properties add new properties to existing classes.
 */
object ExtensionProperties {
    
    // Extension property (must be val)
    val String.firstChar: Char
        get() = this.firstOrNull() ?: '\u0000'
    
    val String.lastChar: Char
        get() = this.lastOrNull() ?: '\u0000'
    
    // Read-write extension property (with var)
    var StringBuilder.lastChar: Char
        get() = getOrNull(length - 1) ?: '\u0000'
        set(value) {
            if (length > 0) {
                setCharAt(length - 1, value)
            }
        }
    
    // Extension property for View
    var View.visibleState: Boolean
        get() = visibility == android.view.View.VISIBLE
        set(value) {
            visibility = if (value) android.view.View.VISIBLE else android.view.View.GONE
        }
    
    // Extension for collection
    val <T> List<T>.lastIndex: Int
        get() = size - 1
    
    // Usage
    fun usage() {
        val first = "Hello".firstChar  // 'H'
        val last = "Hello".lastChar  // 'o'
        
        val sb = StringBuilder("Hello")
        sb.lastChar = '!'
        println(sb.toString())  // "Hell!"
    }
}
```

---

## SECTION 3: DELEGATES

```kotlin
/**
 * Delegates in Kotlin
 * 
 * Delegates allow delegating property access to another object.
 */
class Delegates {
    
    // Custom delegate
    class Delegate {
        private var value: String = ""
        
        operator fun getValue(thisRef: Any?, property: kotlin.reflect.KProperty<*>): String {
            println("Getting ${property.name} = $value")
            return value
        }
        
        operator fun setValue(thisRef: Any?, property: kotlin.reflect.KProperty<*>, value: String) {
            println("Setting ${property.name} = $value")
            this.value = value
        }
    }
    
    // Class using delegate
    class Example {
        var property: String by Delegate()
    }
    
    // Lazy delegate - evaluated on first access
    class LazyExample {
        val heavyResource: String by lazy {
            println("Initializing...")
            "Initialized"
        }
        
        // Thread-safe lazy
        val threadSafe: String by lazy(LazyThreadSafetyMode.SYNCHRONIZED) {
            "Value"
        }
    }
    
    // Observable delegate - triggers on changes
    class ObservableExample {
        var observable: String by kotlin.properties.observable("initial") { prop, old, new ->
            println("${prop.name} changed from $old to $new")
        }
    }
    
    // Vetoable - can reject changes
    class VetoableExample {
        var positive: Int by kotlin.properties.vetoable(0) { prop, old, new ->
            new >= 0  // Only accept non-negative
        }
    }
    
    // Map-backed properties
    class MapBacked(val map: Map<String, Any?>) {
        val name: String by map
        val age: Int by map
    }
}
```

---

## SECTION 4: BUILT-IN DELEGATES

```kotlin
/**
 * Built-in Delegates
 * 
 * Kotlin provides standard delegates for common patterns.
 */
object BuiltInDelegates {
    
    // lazy - Lazy initialization
    class ResourceManager {
        val database: Database by lazy {
            Database.connect()
        }
        
        val networkClient: NetworkClient by lazy(LazyThreadSafetyMode.PUBLICATION) {
            NetworkClient.connect()
        }
    }
    
    // Delegates.observable - Observe changes
    class StateManager {
        var state: String by kotlin.properties.observable("initial") { _, old, new ->
            println("State changed: $old -> $new")
        }
    }
    
    // Delegates.vetoable - Conditional changes
    class ConfigManager {
        var maxConnections: Int by kotlin.properties.vetoable(10) { _, old, new ->
            if (new in 1..100) {
                println("Accepting: $new")
                true
            } else {
                println("Rejecting: $new")
                false
            }
        }
    }
    
    // Map delegation
    class User(data: Map<String, Any?>) {
        val name: String by data
        val email: String by data
        val age: Int by data
    }
    
    // Not null delegate
    class LateInitDelegate {
        // lateinit var for reference types
        lateinit var lateInitValue: String
        
        // Alternative with delegate
        var delegatedValue: String by kotlin.properties Delegates.notNull()
    }
    
    // Usage
    fun usage() {
        val user = User(mapOf(
            "name" to "John",
            "email" to "john@example.com",
            "age" to 30
        ))
        
        println(user.name)  // "John"
    }
}
```

---

## SECTION 5: ADVANCED PATTERNS

```kotlin
/**
 * Advanced Extension and Delegate Patterns
 */
object AdvancedPatterns {
    
    // Type-safe builders with extensions
    class HtmlBuilder {
        private val elements = mutableListOf<String>()
        
        fun div(cssClass: String? = null, block: DIVBuilder.() -> Unit) {
            val builder = DIVBuilder()
            builder.block()
            val content = builder.build()
            val classAttr = cssClass?.let { """class="$it"""" } ?: ""
            elements.add("""<div $classAttr>$content</div>""")
        }
        
        fun build() = elements.joinToString("\n")
    }
    
    class DIVBuilder {
        private val content = StringBuilder()
        
        fun text(text: String) {
            content.append(text)
        }
        
        fun build() = content.toString()
    }
    
    // DSL-like builder
    fun html(block: HtmlBuilder.() -> Unit): String {
        val builder = HtmlBuilder()
        builder.block()
        return builder.build()
    }
    
    // Extension function on generic
    fun <T : android.view.View> T.onClick(block: (T) -> Unit) {
        setOnClickListener { block(this) }
    }
    
    // Extension with receivers
    fun String.withPrefix(prefix: String): String = "$prefix$this"
    
    // Extension on function types
    fun (() -> Unit).debounce(delayMs: Long): () -> Unit {
        var debounceJob: kotlinx.coroutines.Job? = null
        return {
            debounceJob?.cancel()
            debounceJob = kotlinx.coroutines.GlobalScope.launch {
                kotlinx.coroutines.delay(delayMs)
                this@debounce()
            }
        }
    }
    
    // Delegate for SharedPreferences
    class PreferenceDelegate<T>(
        private val context: android.content.Context,
        private val key: String,
        private val default: T
    ) {
        private val prefs: android.content.SharedPreferences
            get() = context.getSharedPreferences("default", android.content.Context.MODE_PRIVATE)
        
        operator fun getValue(thisRef: Any?, property: kotlin.reflect.KProperty<*>): T {
            return when (default) {
                is String -> prefs.getString(key, default) as T
                is Int -> prefs.getInt(key, default) as T
                is Boolean -> prefs.getBoolean(key, default) as T
                is Float -> prefs.getFloat(key, default) as T
                is Long -> prefs.getLong(key, default) as T
                else -> throw IllegalArgumentException("Unsupported type")
            }
        }
        
        operator fun setValue(thisRef: Any?, property: kotlin.reflect.KProperty<*>, value: T) {
            val editor = prefs.edit()
            when (value) {
                is String -> editor.putString(key, value)
                is Int -> editor.putInt(key, value)
                is Boolean -> editor.putBoolean(key, value)
                is Float -> editor.putFloat(key, value)
                is Long -> editor.putLong(key, value)
                else -> throw IllegalArgumentException("Unsupported type")
            }
            editor.apply()
        }
    }
    
    // Context extension for preferences
    fun <T> android.content.Context.preferences(
        key: String,
        default: T
    ): PreferenceDelegate<T> = PreferenceDelegate(this, key, default)
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: Extension function not being called**
- Solution: Check import statement, ensure proper receiver type, avoid name conflicts

**Pitfall 2: lateinit property not initialized**
- Solution: Use lazy instead, check initialization before access, use nullable and null check

**Pitfall 3: Delegate not working as expected**
- Solution: Check delegate syntax, verify getValue/setValue signatures, ensure proper imports

**Pitfall 4: Extension on nullable type**
- Solution: Use extension on nullable (?), use safe call in function body

**Pitfall 5: Confusing property and function**
- Solution: Properties should be fast, functions for complex operations

---

## Best Practices

1. Use extensions for utility functions
2. Keep extensions focused and small
3. Use lazy for expensive initialization
4. Use notNull for lateinit alternative
5. Use observable for change tracking
6. Use vetoable for validation
7. Avoid extensions on Any
8. Document extension purpose
9. Consider performance impact
10. Use type-safe builders for DSL

---

## Troubleshooting Guide

**Issue: Extension not visible**
- Steps: 1. Check import is correct 2. Verify file is compiled 3. Check for conflicting names

**Issue: Lazy delegate not working**
- Steps: 1. Verify lazy block executes 2. Check for exceptions in block 3. Ensure proper LazyThreadSafetyMode

**Issue: Observable not triggering**
- Steps: 1. Verify delegate syntax 2. Check change detection 3. Ensure correct property reference

---

## Advanced Tips and Tricks

- **Tip 1: Use receiver with** - builder pattern, configuration blocks

- **Tip 2: Scope functions as extensions** - apply, also as extensions, Chain operations

- **Tip 3: Use reified with inline** - Get type at runtime, Type-safe casting

- **Tip 4: Create custom delegates** - Property delegation, Resource management

- **Tip 5: Extension on collections** - Utility functions, Transformation helpers

---

## EXAMPLE 1: ANDROID UI EXTENSIONS

```kotlin
/**
 * Android UI Extensions Example
 * 
 * Common extensions for Android UI development.
 */
class AndroidUIExtensions {
    
    // View extensions
    object ViewExtensions {
        fun View.show() {
            visibility = android.view.View.VISIBLE
        }
        
        fun View.hide() {
            visibility = android.view.View.GONE
        }
        
        fun View.invisible() {
            visibility = android.view.View.INVISIBLE
        }
        
        fun View.showIf(condition: Boolean) {
            visibility = if (condition) android.view.View.VISIBLE else android.view.View.GONE
        }
        
        fun View.setOnSingleClick(debounceTime: Long = 500L, action: (View) -> Unit) {
            var lastClickTime = 0L
            setOnClickListener { view ->
                val currentTime = System.currentTimeMillis()
                if (currentTime - lastClickTime > debounceTime) {
                    lastClickTime = currentTime
                    action(view)
                }
            }
        }
        
        fun View.fadeIn(duration: Long = 300) {
            alpha = 0f
            visibility = android.view.View.VISIBLE
            animate()
                .alpha(1f)
                .setDuration(duration)
                .start()
        }
        
        fun View.fadeOut(duration: Long = 300) {
            animate()
                .alpha(0f)
                .setDuration(duration)
                .withEndAction { visibility = android.view.View.GONE }
                .start()
        }
    }
    
    // Context extensions
    object ContextExtensions {
        fun Context.dpToPx(dp: Float): Float {
            return dp * resources.displayMetrics.density
        }
        
        fun Context.pxToDp(px: Float): Float {
            return px / resources.displayMetrics.density
        }
        
        fun Context.showToast(message: String, duration: Int = android.widget.Toast.LENGTH_SHORT) {
            android.widget.Toast.makeText(this, message, duration).show()
        }
        
        fun Context.getColorCompat(colorRes: Int): Int {
            return if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.M) {
                getColor(colorRes)
            } else {
                @Suppress("DEPRECATION")
                resources.getColor(colorRes)
            }
        }
        
        fun <T : android.view.View> Activity.findView(id: Int): T {
            return findViewById(id)
        }
    }
    
    // Fragment extensions
    object FragmentExtensions {
        fun <T : android.view.View> android.app.Fragment.view(id: Int): T? {
            return view?.findViewById(id)
        }
        
        fun android.app.Fragment.toast(message: String) {
            android.widget.Toast.makeText(requireContext(), message, android.widget.Toast.LENGTH_SHORT).show()
        }
        
        fun android.app.Fragment.showSnackbar(message: String, duration: Int = android.widget.Toast.LENGTH_SHORT) {
            view?.let { 
                com.google.android.material.snackbar.Snackbar.make(it, message, duration).show()
            }
        }
    }
    
    // TextView extensions
    object TextViewExtensions {
        fun android.widget.TextView.setTextOrHide(text: String?) {
            if (text.isNullOrBlank()) {
                visibility = android.view.View.GONE
            } else {
                visibility = android.view.View.VISIBLE
                setText(text)
            }
        }
        
        fun android.widget.TextView.setTextColorRes(colorRes: Int) {
            setTextColor(context.getColorCompat(colorRes))
        }
    }
}
```

---

## EXAMPLE 2: DELEGATES FOR ANDROID STATE

```kotlin
/**
 * Delegates for Android State
 * 
 * Using delegates for ViewModel state management.
 */
class DelegateStateExample {
    
    // ViewModel with delegates
    class StateViewModel : androidx.lifecycle.ViewModel() {
        
        // Not-null delegate
        var userName: String by kotlin.properties.Delegates.notNull()
        
        // Observable delegate
        var isLoading: Boolean by kotlin.properties.observable(false) { _, old, new ->
            println("Loading: $old -> $new")
        }
        
        // Vetoable delegate
        var page: Int by kotlin.properties.vetoable(0) { _, old, new ->
            new >= 0 && new <= 100
        }
        
        // Lazy delegate
        val userData: UserData by lazy {
            loadUserData()
        }
        
        private fun loadUserData(): UserData {
            return UserData("John", "john@example.com")
        }
        
        data class UserData(val name: String, val email: String)
    }
    
    // SharedPreferences delegate
    class PreferencesDelegate(private val context: android.content.Context) {
        
        private val prefs: android.content.SharedPreferences
            get() = context.getSharedPreferences("app_prefs", android.content.Context.MODE_PRIVATE)
        
        var isLoggedIn: Boolean by kotlin.properties.Delegates
            .observable(false) { _, _, new ->
                println("Login state: $new")
            }
        
        var userId: String by kotlin.properties.Delegates.notNull()
        
        var theme: String by kotlin.properties.Delegates.vetoable("light") { _, _, new ->
            new in listOf("light", "dark", "system")
        }
    }
    
    // SavedStateHandle delegate
    class SavedStateViewModel(private val handle: android.os.Bundle) : androidx.lifecycle.ViewModel() {
        
        var searchQuery: String? by androidx.lifecycle.SavedStateHandle
            .getSavedStateProvider() as androidx.lifecycle.SavedStateHandle
        
        fun saveState() {
            handle.putString("searchQuery", "kotlin")
        }
    }
}
```

---

## EXAMPLE 3: DSL BUILDER WITH EXTENSIONS

```kotlin
/**
 * DSL Builder with Extensions
 * 
 * Creating type-safe builders for Android views.
 */
class DSLBuilderExample {
    
    // ViewBuilder for declarative UI
    class ViewBuilder(val parent: android.view.ViewGroup) {
        private val children = mutableListOf<android.view.View>()
        
        fun button(id: Int? = null, text: String, config: android.widget.Button.() -> Unit) {
            val button = android.widget.Button(parent.context).apply {
                this.text = text
                config()
            }
            id?.let { button.id = it }
            children.add(button)
            parent.addView(button)
        }
        
        fun textView(id: Int? = null, text: String, config: android.widget.TextView.() -> Unit) {
            val tv = android.widget.TextView(parent.context).apply {
                this.text = text
                config()
            }
            id?.let { tv.id = it }
            children.add(tv)
            parent.addView(tv)
        }
        
        fun build(): List<android.view.View> = children
    }
    
    // Extension function to start builder
    fun android.view.ViewGroup.buildViews(block: ViewBuilder.() -> Unit): List<android.view.View> {
        return ViewBuilder(this).apply(block).build()
    }
    
    // Complex DSL for forms
    class FormBuilder {
        private val fields = mutableListOf<FormField>()
        
        fun textField(name: String, label: String, required: Boolean = false) {
            fields.add(FormField.Text(name, label, required))
        }
        
        fun emailField(name: String, label: String) {
            fields.add(FormField.Email(name, label))
        }
        
        fun build(): List<FormField> = fields.toList()
    }
    
    sealed class FormField {
        data class Text(val name: String, val label: String, val required: Boolean) : FormField()
        data class Email(val name: String, val label: String) : FormField()
    }
    
    // Form DSL
    fun form(block: FormBuilder.() -> Unit): List<FormField> {
        return FormBuilder().apply(block).build()
    }
    
    // Usage
    fun createForm(): List<FormField> {
        return form {
            textField("username", "Username", true)
            textField("password", "Password", true)
            emailField("email", "Email Address")
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Extension Functions:**
- String.addExclamation() -> "Hello!"
- Context.showToast() -> Toast message
- View.visible()/gone() -> visibility
- Int.isEven() -> Boolean
- List<T>.secondOrNull() -> T?

**Extension Properties:**
- String.firstChar -> Char
- String.lastIndex -> Int
- View.visibleState -> Boolean

**Delegates:**
- lazy: Deferred initialization
- observable: Change tracking
- vetoable: Conditional changes
- notNull: Non-null with initialization check
- Map: Map-backed properties

**Built-in Delegates:**
- LazyThreadSafetyMode.SYNCHRONIZED
- LazyThreadSafetyMode.PUBLICATION
- Observable with handler
- Vetoable with predicate

**Advanced Patterns:**
- Type-safe builders
- DSL-like construction
- Preference delegates
- Debounced click handling

---

## CROSS-REFERENCES

- See: 01_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md
- See: 01_Kotlin_Basics_for_Android/02_Android_Kotlin_Conventions.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/04_Custom_Views_and_Components.md
- See: 03_ARCHITECTURE/02_Dependency_Injection/01_Dagger_and_Hilt_Basics.md

---

## END OF EXTENSIONS AND DELEGATES GUIDE
