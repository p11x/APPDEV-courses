# LEARNING OBJECTIVES

1. Understanding Jetpack Compose basics
2. Setting up Compose in Android projects
3. Creating simple composable functions
4. Understanding Compose modifiers
5. Working with Compose previews

```kotlin
package com.android.compose.basics
```

---

## SECTION 1: COMPOSE OVERVIEW

```kotlin
/**
 * Jetpack Compose Overview
 * 
 * Jetpack Compose is Android's modern UI toolkit.
 * It simplifies and accelerates UI development.
 */
object ComposeOverview {
    
    const val COMPOSE_VERSION = "1.5.8"
    const val COMPILE_SDK = 34
    
    // Key dependencies
    val dependencies = listOf(
        "androidx.compose.ui:ui",
        "androidx.compose.ui:ui-graphics",
        "androidx.compose.ui:ui-tooling-preview",
        "androidx.compose.material3:material3",
        "androidx.compose.material:material-icons-extended",
        "androidx.activity:activity-compose"
    )
    
    // Key features
    val features = listOf(
        "Declarative UI paradigm",
        "Less boilerplate code",
        "State-driven UI updates",
        "Built-in animations",
        "Material Design 3 support"
    )
}
```

---

## SECTION 2: COMPOSE SETUP

```kotlin
/**
 * Compose Setup
 * 
 * Configuring Compose in Android project.
 */
class ComposeSetup {
    
    // Build.gradle (app) configuration
    fun getAppBuildGradle(): String {
        return """
android {
    ...
    composeOptions {
        kotlinCompilerExtensionVersion = "1.5.8"
    }
    buildFeatures {
        compose = true
    }
}

dependencies {
    // Compose BOM
    implementation platform('androidx.compose:compose-bom:2024.01.00')
    
    // Core Compose
    implementation 'androidx.compose.ui:ui'
    implementation 'androidx.compose.ui:ui-graphics'
    implementation 'androidx.compose.ui:ui-tooling-preview'
    implementation 'androidx.compose.material3:material3'
    
    // Activity Compose
    implementation 'androidx.activity:activity-compose:1.8.2'
    
    // ViewModel Compose
    implementation 'androidx.lifecycle:lifecycle-viewmodel-compose:2.7.0'
    implementation 'androidx.lifecycle:lifecycle-runtime-compose:2.7.0'
    
    // Navigation Compose
    implementation 'androidx.navigation:navigation-compose:2.7.6'
    
    // Debug
    debugImplementation 'androidx.compose.ui:ui-tooling'
    debugImplementation 'androidx.compose.ui:ui-test-manifest'
}
        """.trimIndent()
    }
    
    // Theme configuration
    fun getThemeSetup(): String {
        return """
// AppTheme.kt
@Composable
fun MyAppTheme(
    darkTheme: Boolean = isSystemInDarkTheme(),
    content: @Composable () -> Unit
) {
    val colorScheme = if (darkTheme) {
        darkColorScheme()
    } else {
        lightColorScheme()
    }
    
    MaterialTheme(
        colorScheme = colorScheme,
        content = content
    )
}
        """.trimIndent()
    }
    
    // Activity with Compose
    fun getComposeActivity(): String {
        return """
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MyAppTheme {
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    Greeting("Android")
                }
            }
        }
    }
}
        """.trimIndent()
    }
}
```

---

## SECTION 3: COMPOSABLE FUNCTIONS

```kotlin
/**
 * Composable Functions
 * 
 * Building blocks of Compose UI.
 */
class ComposableFunctions {
    
    // Simple composable
    @Composable
    fun SimpleText() {
        Text("Hello, World!")
    }
    
    // Composable with parameters
    @Composable
    fun Greeting(name: String) {
        Text(text = "Hello, $name!")
    }
    
    // Composable with multiple parameters
    @Composable
    fun UserCard(
        name: String,
        email: String,
        imageUrl: String? = null
    ) {
        Row(
            modifier = Modifier.padding(16.dp),
            verticalAlignment = Alignment.CenterVertically
        ) {
            // Placeholder for image
            Surface(
                modifier = Modifier.size(48.dp),
                shape = CircleShape,
                color = MaterialTheme.colorScheme.primary
            ) {
                // Image would go here
            }
            
            Column(modifier = Modifier.padding(start = 16.dp)) {
                Text(text = name, style = MaterialTheme.typography.titleMedium)
                Text(text = email, style = MaterialTheme.typography.bodyMedium)
            }
        }
    }
    
    // State in composable
    @Composable
    fun Counter() {
        var count by remember { mutableIntStateOf(0) }
        
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.padding(16.dp)
        ) {
            Text(text = "Count: $count", style = MaterialTheme.typography.headlineMedium)
            
            Row {
                Button(onClick = { count++ }) {
                    Text("Increment")
                }
                
                Spacer(modifier = Modifier.width(8.dp))
                
                Button(onClick = { count-- }) {
                    Text("Decrement")
                }
            }
        }
    }
    
    // Remember for state
    @Composable
    fun rememberExample() {
        // Remember single value
        val value by remember { mutableStateOf("initial") }
        
        // Remember with key (recomputed when key changes)
        val computed by remember(key1 = value) { 
            value.uppercase() 
        }
        
        // Remember Saveable (survives process death)
        val saved by rememberSaveable { mutableStateOf("saved") }
    }
    
    // Derived state
    @Composable
    fun derivedStateExample() {
        val items = listOf("A", "B", "C", "D", "E")
        var filter by remember { mutableStateOf("") }
        
        // Derived state - only recalculated when items or filter changes
        val filteredItems = remember(items, filter) {
            items.filter { it.contains(filter) }
        }
    }
}
```

---

## SECTION 4: MODIFIERS

```kotlin
/**
 * Modifiers
 * 
 * Modifiers change the appearance or behavior of composables.
 */
object Modifiers {
    
    // Common modifiers
    @Composable
    fun modifierExamples() {
        Column {
            // Padding
            Text("With padding", modifier = Modifier.padding(16.dp))
            
            // Margin (using parent padding)
            Text("With margin", modifier = Modifier.padding(start = 16.dp))
            
            // Size
            Text("Fixed size", modifier = Modifier.size(100.dp))
            Text("Fill max", modifier = Modifier.fillMaxWidth())
            Text("Fill available", modifier = Modifier.fillMaxSize())
            
            // Background
            Text("With background", modifier = Modifier.background(Color.Blue))
            
            // Clickable
            Text("Clickable", modifier = Modifier.clickable { /* handle click */ })
            
            // Border
            Text("With border", modifier = Modifier.border(1.dp, Color.Black))
            
            // Shape
            Text("Rounded", modifier = Modifier.background(Color.Red, RoundedCornerShape(8.dp)))
            
            // Alignment
            Text("Center", modifier = Modifier.align(Alignment.CenterHorizontally))
            
            // Aspect ratio
            Box(modifier = Modifier.aspectRatio(16f / 9f))
            
            // Scroll
            Column(modifier = Modifier.verticalScroll(rememberScrollState())) {
                // Scrollable content
            }
        }
    }
    
    // Chaining modifiers
    @Composable
    fun chainedModifiers() {
        Text(
            text = "Styled text",
            modifier = Modifier
                .padding(16.dp)
                .background(Color.LightGray)
                .clickable { }
                .padding(8.dp)
                .border(1.dp, Color.DarkGray)
        )
    }
    
    // Conditional modifiers
    @Composable
    fun conditionalModifiers() {
        val isSelected = true
        
        Text(
            text = "Conditional",
            modifier = Modifier
                .padding(16.dp)
                .then(if (isSelected) Modifier.background(Color.Blue) else Modifier)
        )
    }
    
    // Modifier order matters
    @Composable
    fun modifierOrderMatters() {
        // Different order = different results
        // 1. Background then clickable - clicking affects background
        Text("Option 1", modifier = Modifier
            .background(Color.Blue)
            .clickable { }
        )
        
        // 2. Clickable then background - background is part of click area
        Text("Option 2", modifier = Modifier
            .clickable { }
            .background(Color.Blue)
        )
    }
}
```

---

## SECTION 5: PREVIEWS

```kotlin
/**
 * Previews
 * 
 * Compose preview allows viewing composables in Android Studio.
 */
object Previews {
    
    // Simple preview
    @Preview(showBackground = true)
    @Composable
    fun SimplePreview() {
        Text("Hello, Preview!")
    }
    
    // Preview with theme
    @Preview(showBackground = true)
    @Composable
    fun ThemedPreview() {
        MyAppTheme {
            Greeting("Android")
        }
    }
    
    // Preview with multiple devices
    @Preview(
        name = "Phone",
        showSystemUi = true,
        device = "id:nexus_one"
    )
    @Preview(
        name = "Tablet",
        showSystemUi = true,
        device = "id:pixel_c"
    )
    @Composable
    fun DevicePreview() {
        Column {
            Text("Responsive content")
            Text("Adjusts to device")
        }
    }
    
    // Dark mode preview
    @Preview(uiMode = Configuration.UI_MODE_NIGHT_YES)
    @Composable
    fun DarkModePreview() {
        MyAppTheme(darkTheme = true) {
            Greeting("Dark Mode")
        }
    }
    
    // Preview with parameters
    @Preview(showBackground = true)
    @Composable
    fun ParameterPreview(
        @PreviewParameter(SampleDataProvider::class) data: String
    ) {
        Text(data)
    }
    
    // Sample data provider
    class SampleDataProvider : PreviewParameterProvider<String> {
        override val values: Sequence<String>
            get() = sequenceOf("Sample 1", "Sample 2", "Sample 3")
    }
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: Recomposition not happening**
- Solution: Use remember to store state, use mutableStateOf for state, pass state as parameters, use viewModel for shared state

**Pitfall 2: Infinite recomposition loop**
- Solution: Don't modify state during composition, use derivedStateOf for computed values, avoid mutable objects as state

**Pitfall 3: Modifier not working**
- Solution: Check modifier order, verify correct syntax, use correct Modifier import

**Pitfall 4: Preview not showing**
- Solution: Ensure @Composable functions, check @Preview annotation, rebuild project, check minSdk version

---

## Best Practices

1. Use stateless composables where possible
2. Pass state as parameters
3. Use remember for expensive operations
4. Use derivedStateOf for computed values
5. Keep composables small and focused
6. Use modifiers consistently
7. Test composables with previews
8. Follow Compose API guidelines
9. Use proper naming conventions
10. Document complex composables

---

## EXAMPLE 1: COMPLETE COMPOSE APP

```kotlin
/**
 * Complete Compose App Example
 * 
 * Full Compose application setup.
 */
class CompleteComposeApp {
    
    // Main Activity
    class MainActivity : androidx.activity.ComponentActivity() {
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            setContent {
                ComposeAppTheme {
                    MainScreen()
                }
            }
        }
    }
    
    // Theme
    @Composable
    fun ComposeAppTheme(
        darkTheme: Boolean = androidx.compose.foundation.isSystemInDarkTheme(),
        content: @Composable () -> Unit
    ) {
        val colorScheme = if (darkTheme) {
            androidx.compose.material3.darkColorScheme()
        } else {
            androidx.compose.material3.lightColorScheme()
        }
        
        androidx.compose.material3.MaterialTheme(
            colorScheme = colorScheme,
            typography = androidx.compose.material3.Typography(),
            content = content
        )
    }
    
    // Main Screen
    @Composable
    fun MainScreen() {
        var showDialog by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(false) }
        
        androidx.compose.material3.Scaffold(
            topBar = {
                androidx.compose.material3.TopAppBar(
                    title = { Text("Compose App") }
                )
            },
            floatingActionButton = {
                androidx.compose.material3.FloatingActionButton(
                    onClick = { showDialog = true }
                ) {
                    androidx.compose.material.icons.Icons.Default.Add.let { 
                        androidx.compose.material3.Icon(it, contentDescription = "Add")
                    }
                }
            }
        ) { paddingValues ->
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(paddingValues)
                    .padding(16.dp)
            ) {
                Text("Welcome to Compose!")
                androidx.compose.foundation.layout.Spacer(modifier = Modifier.height(16.dp))
                androidx.compose.material3.Button(onClick = { /* action */ }) {
                    Text("Click Me")
                }
            }
        }
        
        if (showDialog) {
            androidx.compose.material3.AlertDialog(
                onDismissRequest = { showDialog = false },
                title = { Text("Dialog") },
                text = { Text("This is a Compose dialog!") },
                confirmButton = {
                    androidx.compose.material3.TextButton(onClick = { showDialog = false }) {
                        Text("OK")
                    }
                }
            )
        }
    }
}
```

---

## EXAMPLE 2: COMPLEX UI WITH STATE

```kotlin
/**
 * Complex UI with State Example
 * 
 * Managing state in a more complex Compose UI.
 */
class ComplexStateUI {
    
    // Data class for state
    data class FormState(
        val name: String = "",
        val email: String = "",
        val isLoading: Boolean = false,
        val error: String? = null
    )
    
    // Form Screen
    @Composable
    fun FormScreen(
        viewModel: FormViewModel
    ) {
        val state by viewModel.state.collectAsState()
        
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            // Name field
            androidx.compose.material3.OutlinedTextField(
                value = state.name,
                onValueChange = viewModel::updateName,
                label = { Text("Name") },
                isError = state.error != null && state.name.isEmpty(),
                modifier = Modifier.fillMaxWidth()
            )
            
            // Email field
            androidx.compose.material3.OutlinedTextField(
                value = state.email,
                onValueChange = viewModel::updateEmail,
                label = { Text("Email") },
                isError = state.error != null && state.email.isEmpty(),
                modifier = Modifier.fillMaxWidth()
            )
            
            // Error message
            state.error?.let { error ->
                Text(
                    text = error,
                    color = androidx.compose.material3.MaterialTheme.colorScheme.error,
                    style = androidx.compose.material3.MaterialTheme.typography.bodySmall
                )
            }
            
            // Submit button
            androidx.compose.material3.Button(
                onClick = viewModel::submit,
                enabled = !state.isLoading,
                modifier = Modifier.fillMaxWidth()
            ) {
                if (state.isLoading) {
                    androidx.compose.material3.CircularProgressIndicator(
                        modifier = Modifier.size(20.dp),
                        strokeWidth = 2.dp
                    )
                } else {
                    Text("Submit")
                }
            }
        }
    }
    
    // ViewModel
    class FormViewModel : androidx.lifecycle.ViewModel() {
        private val _state = androidx.compose.runtime.mutableStateOf(FormState())
        val state: androidx.compose.runtime.State<FormState> = _state
        
        fun updateName(name: String) {
            _state.value = _state.value.copy(name = name, error = null)
        }
        
        fun updateEmail(email: String) {
            _state.value = _state.value.copy(email = email, error = null)
        }
        
        fun submit() {
            val currentState = _state.value
            
            // Validation
            if (currentState.name.isBlank() || currentState.email.isBlank()) {
                _state.value = currentState.copy(error = "Please fill all fields")
                return
            }
            
            _state.value = currentState.copy(isLoading = true)
            
            // Simulate async operation
            androidx.lifecycle.viewModelScope.launch {
                try {
                    kotlinx.coroutines.delay(1000)
                    _state.value = _state.value.copy(isLoading = false, error = null)
                } catch (e: Exception) {
                    _state.value = _state.value.copy(isLoading = false, error = e.message)
                }
            }
        }
    }
}
```

---

## EXAMPLE 3: LISTS AND ANIMATIONS

```kotlin
/**
 * Lists and Animations Example
 * 
 * Building animated lists with Compose.
 */
class ListsAndAnimations {
    
    // Animated list
    @Composable
    fun AnimatedList() {
        var items by androidx.compose.runtime.remember { 
            androidx.compose.runtime.mutableStateListOf("Item 1", "Item 2", "Item 3") 
        }
        
        Column {
            androidx.compose.material3.Button(onClick = { 
                items.add("Item ${items.size + 1}")
            }) {
                Text("Add Item")
            }
            
            androidx.compose.foundation.lazy.LazyColumn {
                items(items.size) { index ->
                    AnimatedItem(
                        text = items[index],
                        onRemove = { items.removeAt(index) }
                    )
                }
            }
        }
    }
    
    @Composable
    fun AnimatedItem(
        text: String,
        onRemove: () -> Unit
    ) {
        var visible by androidx.compose.runtime.remember { androidx.compose.runtime.mutableStateOf(true) }
        
        if (visible) {
            androidx.compose.animation.AnimatedVisibility(
                visible = visible,
                exit = androidx.compose.animation.fadeOut() + 
                       androidx.compose.animation.slideOutHorizontally()
            ) {
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(8.dp),
                    verticalAlignment = Alignment.CenterVertically
                ) {
                    Text(text, modifier = Modifier.weight(1f))
                    androidx.compose.material3.IconButton(onClick = { 
                        visible = false
                        kotlinx.coroutines.delay(300)
                        onRemove()
                    }) {
                        androidx.compose.material.icons.Icons.Default.Delete.let {
                            androidx.compose.material3.Icon(it, contentDescription = "Delete")
                        }
                    }
                }
            }
        }
    }
    
    // Animated content
    @Composable
    fun AnimatedContent() {
        var state by androidx.compose.runtime.remember { 
            androidx.compose.runtime.mutableStateOf(0) 
        }
        
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.fillMaxWidth()
        ) {
            androidx.compose.animation.AnimatedContent(
                targetState = state,
                transitionSpec = {
                    androidx.compose.animation.fadeIn() togetherWith 
                    androidx.compose.animation.fadeOut()
                },
                label = "content"
            ) { targetState ->
                Text(
                    text = "State: $targetState",
                    style = androidx.compose.material3.MaterialTheme.typography.headlineMedium
                )
            }
            
            androidx.compose.foundation.layout.Spacer(modifier = Modifier.height(16.dp))
            
            androidx.compose.material3.Button(onClick = { state = (state + 1) % 5 }) {
                Text("Next")
            }
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Compose Setup:**
- Dependencies: compose-bom, ui, material3
- Activity Compose: ComponentActivity
- ViewModel Compose: viewModelScope
- Navigation Compose

**Composable Functions:**
- @Composable annotation for UI functions
- remember for state persistence
- mutableStateOf for reactive state
- rememberSaveable for process death survival
- derivedStateOf for computed values

**Modifiers:**
- padding, margin for spacing
- size, fillMax for dimensions
- background, border for decoration
- clickable for interactions
- scroll, verticalScroll for scrolling
- Modifier chaining and ordering
- Conditional modifiers with then

**Previews:**
- @Preview annotation
- showBackground for visual
- Dark mode previews
- Device-specific previews
- PreviewParameterProvider for data

**Key Concepts:**
- Declarative paradigm
- State-driven UI
- Recomposition on state change
- Side effects with LaunchedEffect
- Scoped state with remember

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/02_Composable_Functions.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/03_State_Management_Compose.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/04_Navigation_Compose.md
- See: 06_NAVIGATION/01_Navigation_Architecture/02_Navigation_Compose.md

---

## END OF COMPOSE BASICS AND SETUP GUIDE
