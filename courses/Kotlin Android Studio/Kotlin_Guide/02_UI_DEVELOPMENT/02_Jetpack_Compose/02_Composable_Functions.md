# LEARNING OBJECTIVES

1. Understanding Composable functions in depth
2. Creating reusable UI components
3. Working with different types of Composables
4. Managing composition scope
5. Using Compose stability

```kotlin
package com.android.compose.composable
```

---

## SECTION 1: COMPOSABLE FUNDAMENTALS

```kotlin
/**
 * Composable Fundamentals
 * 
 * Composable functions are the building blocks of Compose UI.
 */
object ComposableFundamentals {
    
    // Basic composable
    @Composable
    fun HelloWorld() {
        androidx.compose.material3.Text("Hello, World!")
    }
    
    // Composable with parameters
    @Composable
    fun Greeting(name: String) {
        androidx.compose.material3.Text("Hello, $name!")
    }
    
    // Composable with default parameters
    @Composable
    fun CustomText(
        text: String,
        style: androidx.compose.ui.text.TextStyle = androidx.compose.material3.MaterialTheme.typography.bodyLarge,
        color: androidx.compose.ui.graphics.Color = androidx.compose.material3.MaterialTheme.colorScheme.onSurface
    ) {
        androidx.compose.material3.Text(text = text, style = style, color = color)
    }
    
    // Composable with optional parameters
    @Composable
    fun OptionalParams(
        required: String,
        optional: String? = null,
        enabled: Boolean = true
    ) {
        Column {
            Text(required)
            optional?.let { Text(it) }
            Text(if (enabled) "Enabled" else "Disabled")
        }
    }
}
```

---

## SECTION 2: COMPOSABLE TYPES

```kotlin
/**
 * Composable Types
 * 
 * Different types and patterns for composables.
 */
class ComposableTypes {
    
    // Stateless composable
    @Composable
    fun StatelessCard(
        title: String,
        subtitle: String,
        onClick: () -> Unit
    ) {
        androidx.compose.material3.Card(
            onClick = onClick,
            modifier = Modifier.fillMaxWidth()
        ) {
            Column(modifier = Modifier.padding(16.dp)) {
                androidx.compose.material3.Text(
                    text = title,
                    style = androidx.compose.material3.MaterialTheme.typography.titleMedium
                )
                androidx.compose.material3.Text(
                    text = subtitle,
                    style = androidx.compose.material3.MaterialTheme.typography.bodyMedium
                )
            }
        }
    }
    
    // Stateful composable
    @Composable
    fun StatefulExpandableCard(
        title: String,
        content: String
    ) {
        var expanded by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        
        androidx.compose.material3.Card(
            onClick = { expanded = !expanded },
            modifier = Modifier.fillMaxWidth()
        ) {
            Column(modifier = Modifier.padding(16.dp)) {
                androidx.compose.material3.Text(text = title)
                androidx.compose.foundation.layout.AnimatedVisibility(visible = expanded) {
                    androidx.compose.material3.Text(text = content)
                }
            }
        }
    }
    
    // Composable with ViewModel
    @Composable
    fun ViewModelScreen(
        viewModel: MyViewModel = androidx.lifecycle.viewmodel.compose.viewmodel()
    ) {
        val state by viewModel.state.collectAsState()
        
        androidx.compose.material3.Scaffold { paddingValues ->
            Column(modifier = Modifier.padding(paddingValues)) {
                Text("Data: ${state.data}")
                androidx.compose.material3.Button(onClick = viewModel::refresh) {
                    Text("Refresh")
                }
            }
        }
    }
    
    class MyViewModel : androidx.lifecycle.ViewModel() {
        val state = androidx.compose.runtime.mutableStateOf(MyState())
        
        fun refresh() {
            state.value = state.value.copy(data = "New data")
        }
        
        data class MyState(val data: String = "")
    }
}
```

---

## SECTION 3: REUSABLE COMPOSABLES

```kotlin
/**
 * Reusable Composables
 * 
 * Creating reusable UI components.
 */
object ReusableComposables {
    
    // Custom button composable
    @Composable
    fun ActionButton(
        text: String,
        onClick: () -> Unit,
        modifier: Modifier = Modifier,
        enabled: Boolean = true,
        icon: androidx.compose.ui.graphics.vector.ImageVector? = null
    ) {
        androidx.compose.material3.Button(
            onClick = onClick,
            enabled = enabled,
            modifier = modifier
        ) {
            icon?.let {
                androidx.compose.material3.Icon(
                    imageVector = it,
                    contentDescription = null,
                    modifier = Modifier.size(18.dp)
                )
                androidx.compose.foundation.layout.Spacer(modifier = Modifier.width(8.dp))
            }
            androidx.compose.material3.Text(text)
        }
    }
    
    // Custom text field
    @Composable
    fun ValidatedTextField(
        value: String,
        onValueChange: (String) -> Unit,
        label: String,
        error: String? = null,
        modifier: Modifier = Modifier,
        keyboardOptions: androidx.compose.foundation.text.KeyboardOptions = androidx.compose.foundation.text.KeyboardOptions.Default
    ) {
        androidx.compose.material3.OutlinedTextField(
            value = value,
            onValueChange = onValueChange,
            label = { androidx.compose.material3.Text(label) },
            isError = error != null,
            supportingText = error?.let { { androidx.compose.material3.Text(it) } },
            keyboardOptions = keyboardOptions,
            modifier = modifier
        )
    }
    
    // Loading indicator
    @Composable
    fun LoadingIndicator(
        message: String? = null,
        modifier: Modifier = Modifier
    ) {
        Column(
            modifier = modifier,
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.Center
        ) {
            androidx.compose.material3.CircularProgressIndicator()
            message?.let {
                androidx.compose.foundation.layout.Spacer(modifier = Modifier.height(16.dp))
                androidx.compose.material3.Text(
                    text = it,
                    style = androidx.compose.material3.MaterialTheme.typography.bodyMedium
                )
            }
        }
    }
    
    // Empty state
    @Composable
    fun EmptyState(
        icon: androidx.compose.ui.graphics.vector.ImageVector,
        title: String,
        message: String,
        actionLabel: String? = null,
        onAction: (() -> Unit)? = null,
        modifier: Modifier = Modifier
    ) {
        Column(
            modifier = modifier,
            horizontalAlignment = Alignment.CenterHorizontally,
            verticalArrangement = Arrangement.Center
        ) {
            androidx.compose.material3.Icon(
                imageVector = icon,
                contentDescription = null,
                modifier = Modifier.size(64.dp),
                tint = androidx.compose.material3.MaterialTheme.colorScheme.onSurfaceVariant
            )
            androidx.compose.foundation.layout.Spacer(modifier = Modifier.height(16.dp))
            androidx.compose.material3.Text(
                text = title,
                style = androidx.compose.material3.MaterialTheme.typography.titleLarge
            )
            androidx.compose.foundation.layout.Spacer(modifier = Modifier.height(8.dp))
            androidx.compose.material3.Text(
                text = message,
                style = androidx.compose.material3.MaterialTheme.typography.bodyMedium,
                color = androidx.compose.material3.MaterialTheme.colorScheme.onSurfaceVariant
            )
            actionLabel?.let { label ->
                androidx.compose.foundation.layout.Spacer(modifier = Modifier.height(16.dp))
                androidx.compose.material3.Button(onClick = { onAction?.invoke() }) {
                    androidx.compose.material3.Text(label)
                }
            }
        }
    }
}
```

---

## SECTION 4: COMPOSITION SCOPE

```kotlin
/**
 * Composition Scope
 * 
 * Understanding scope and context in Compose.
 */
object CompositionScope {
    
    // Local composition - using CompositionLocal
    object LocalExample {
        val LocalUser = androidx.compose.runtime.CompositionLocalOf { "Default User" }
        
        @Composable
        fun UserProvider(
            user: String,
            content: @Composable () -> Unit
        ) {
            androidx.compose.runtime.ProvidedValue.LocalProvides.compose(user) {
                content()
            }
        }
        
        @Composable
        fun UserName() {
            val user = LocalUser.current
            Text(user)
        }
    }
    
    // Scoped state
    @Composable
    fun ScopedState() {
        // State is scoped to this composable
        var count by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(0) }
        
        // Child composables access same state
        Column {
            CounterDisplay(count = count)
            CounterControls(onIncrement = { count++ }, onReset = { count = 0 })
        }
    }
    
    @Composable
    fun CounterDisplay(count: Int) {
        Text("Count: $count", style = MaterialTheme.typography.headlineMedium)
    }
    
    @Composable
    fun CounterControls(onIncrement: () -> Unit, onReset: () -> Unit) {
        Row {
            Button(onClick = onIncrement) { Text("Increment") }
            Button(onClick = onReset) { Text("Reset") }
        }
    }
    
    // Remember in different scopes
    @Composable
    fun ScopeExample() {
        // Function-level scope - survives recomposition
        val functionLevel by androidx.compose.runtime.remember { mutableStateOf(0) }
        
        // With key - survives recomposition when key changes
        val keyed by androidx.compose.runtime.remember(key1 = functionLevel) { mutableStateOf(0) }
        
        // Saveable - survives configuration change
        val saveable by androidx.compose.runtime.saveable.rememberSaveable { mutableStateOf(0) }
    }
}
```

---

## SECTION 5: COMPOSE STABILITY

```kotlin
/**
 * Compose Stability
 * 
 * Understanding stability and its impact on recomposition.
 */
object ComposeStability {
    
    // Stable class (can skip recomposition if values are equal)
    @androidx.compose.runtime.Stable
    class StableConfig(val name: String, val enabled: Boolean)
    
    // Unstable class (might cause unnecessary recomposition)
    class UnstableConfig(val items: List<String>)
    
    // Using @Stable explicitly
    @androidx.compose.runtime.Stable
    interface State
    
    // Marking as stable for external types
    @androidx.compose.runtime.Stable
    class ExternalData(val value: String)
    
    // Stability annotations
    @Suppress("UNSTABLE_TYPE_LATTICE")
    @androidx.compose.runtime.Stable
    class PartiallyStable(val stable: String, val unstable: java.util.List<String>)
    
    // Function stability
    // Lambda stability - use remember to make stable
    @Composable
    fun StableLambda() {
        var count by remember { mutableStateOf(0) }
        
        // Stable callback - use remember
        val stableCallback = remember { { count++ } }
        
        // Using callback
        ChildComponent(onClick = stableCallback)
    }
    
    @Composable
    fun ChildComponent(onClick: () -> Unit) {
        Button(onClick = onClick) { Text("Click") }
    }
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: State not persisting across recompositions**
- Solution: Use remember for state, use rememberSaveable for process death, use ViewModel for shared state

**Pitfall 2: Unnecessary recompositions**
- Solution: Use stable types, use derivedStateOf for computed values, pass stable lambdas

**Pitfall 3: Modifying state during composition**
- Solution: Move state modification to event handlers, use LaunchedEffect for side effects, use remember with side effect

**Pitfall 4: Scope errors with local composition**
- Solution: Verify CompositionLocal is provided, check scope of LocalProvider, use correct composition order

---

## Best Practices

1. Keep composables focused and small
2. Make composables stateless where possible
3. Use stable types for parameters
4. Document public API surface
5. Use sensible default values
6. Test composables in isolation
7. Use previews for development
8. Consider performance implications
9. Follow naming conventions
10. Keep state close to where it's used

---

## EXAMPLE 1: COMPLETE CUSTOM COMPONENT

```kotlin
/**
 * Complete Custom Component
 * 
 * A fully-featured custom composable component.
 */
class CustomComponents {
    
    // Custom rating component
    @Composable
    fun RatingBar(
        rating: Float,
        onRatingChange: ((Float) -> Unit)? = null,
        modifier: Modifier = Modifier,
        maxRating: Int = 5,
        starSize: androidx.compose.ui.unit.Dp = 32.dp,
        starColor: androidx.compose.ui.graphics.Color = androidx.compose.material3.MaterialTheme.colorScheme.primary,
        emptyColor: androidx.compose.ui.graphics.Color = androidx.compose.material3.MaterialTheme.colorScheme.outline
    ) {
        Row(
            modifier = modifier,
            verticalAlignment = Alignment.CenterVertically
        ) {
            for (i in 1..maxRating) {
                val icon = if (i <= rating) {
                    androidx.compose.material.icons.Icons.Default.Star
                } else {
                    androidx.compose.material.icons.Icons.Default.StarOutline
                }
                
                androidx.compose.material3.IconButton(
                    onClick = { onRatingChange?.invoke(i.toFloat()) },
                    modifier = Modifier.size(starSize)
                ) {
                    androidx.compose.material3.Icon(
                        imageVector = icon,
                        contentDescription = "$i stars",
                        tint = if (i <= rating) starColor else emptyColor,
                        modifier = Modifier.size(starSize * 0.8f)
                    )
                }
            }
        }
    }
    
    // Usage
    @Composable
    fun RatingExample() {
        var currentRating by remember { mutableStateOf(0f) }
        
        Column {
            Text("Rate this:")
            RatingBar(
                rating = currentRating,
                onRatingChange = { currentRating = it }
            )
            Text("Your rating: ${currentRating.toInt()}")
        }
    }
}
```

---

## EXAMPLE 2: LAYOUT COMPOSABLES

```kotlin
/**
 * Layout Composables
 * 
 * Creating custom layout composables.
 */
class LayoutComposables {
    
    // Custom layout composable
    @Composable
    fun CustomRow(
        modifier: Modifier = Modifier,
        horizontalArrangement: Arrangement.Horizontal = Arrangement.Start,
        verticalAlignment: Alignment.Vertical = Alignment.Top,
        content: @Composable () -> Unit
    ) {
        androidx.compose.foundation.layout.Row(
            modifier = modifier,
            horizontalArrangement = horizontalArrangement,
            verticalAlignment = verticalAlignment,
            content = { content() }
        )
    }
    
    // Custom layout with Box
    @Composable
    fun OverlayLayout(
        modifier: Modifier = Modifier,
        overlay: @Composable () -> Unit,
        content: @Composable () -> Unit
    ) {
        Box(modifier = modifier) {
            content()
            Box(modifier = Modifier.align(Alignment.BottomEnd)) {
                overlay()
            }
        }
    }
    
    // Flow layout (wrapping)
    @Composable
    fun FlowRow(
        modifier: Modifier = Modifier,
        horizontalArrangement: Arrangement.Horizontal = Arrangement.Start,
        verticalArrangement: Arrangement.Vertical = Arrangement.Top,
        maxItemsInEachRow: Int = Int.MAX_VALUE,
        content: @Composable () -> Unit
    ) {
        // Using Compose Foundation's FlowRow
        androidx.compose.foundation.layout.FlowRow(
            modifier = modifier,
            horizontalArrangement = horizontalArrangement,
            verticalArrangement = verticalArrangement,
            maxItemsInEachRow = maxItemsInEachRow,
            content = { content() }
        )
    }
}
```

---

## EXAMPLE 3: INTEROP WITH XML

```kotlin
/**
 * Interop with XML
 * 
 * Using Compose with XML views.
 */
class ComposeInterop {
    
    // AndroidView for XML views
    @Composable
    fun LegacyViewIntegration() {
        androidx.compose.ui.viewinterop.AndroidView(
            factory = { context ->
                android.widget.TextView(context).apply {
                    text = "Hello from XML View"
                    setTextColor(android.graphics.Color.BLACK)
                }
            },
            update = { view ->
                view.text = "Updated: ${System.currentTimeMillis()}"
            },
            modifier = Modifier.padding(16.dp)
        )
    }
    
    // ComposeView in XML
    // In XML: <androidx.compose.ui.platform.ComposeView android:id="@+id/compose_view" />
    
    fun setupComposeView(view: androidx.compose.ui.platform.ComposeView) {
        view.setContent {
            androidx.compose.material3.MaterialTheme {
                Text("Hello from Compose in XML!")
            }
        }
    }
    
    // Fragment with Compose
    class ComposeFragment : androidx.fragment.app.Fragment() {
        
        override fun onCreateView(
            inflater: android.view.LayoutInflater,
            container: android.view.ViewGroup?,
            savedInstanceState: android.os.Bundle?
        ): android.view.View {
            return androidx.compose.ui.platform.ComposeView(requireContext()).apply {
                setContent {
                    MyComposeContent()
                }
            }
        }
        
        @Composable
        fun MyComposeContent() {
            Text("Hello from Fragment Compose!")
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Composable Types:**
- Stateless: Props in, UI out
- Stateful: Internal state management
- ViewModel-backed: Shared state
- Reusable: Configurable components

**Composable Patterns:**
- Remember: State persistence
- MutableState: Reactive updates
- DerivedState: Computed values
- Saveable: Configuration surviving

**Reusability:**
- Default parameters
- Modifier parameter for flexibility
- Public API surface documentation
- Testability

**Stability:**
- @Stable annotation for custom types
- Stable lambdas with remember
- Unstable types trigger recomposition
- Use data classes carefully

**Interop:**
- AndroidView for XML in Compose
- ComposeView in XML layouts
- Fragment with ComposeView
- Activity with setContent

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/03_State_Management_Compose.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/05_Advanced_Compose_Patterns.md

---

## END OF COMPOSABLE FUNCTIONS GUIDE
