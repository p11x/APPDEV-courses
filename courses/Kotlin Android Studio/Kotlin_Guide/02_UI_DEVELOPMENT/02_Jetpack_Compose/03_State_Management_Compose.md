# LEARNING OBJECTIVES

1. Understanding state in Compose
2. Using StateFlow and MutableStateFlow
3. Managing state with ViewModels
4. Collecting state in Compose
5. Implementing unidirectional data flow

```kotlin
package com.android.compose.state
```

---

## SECTION 1: COMPOSE STATE OVERVIEW

```kotlin
/**
 * Compose State Overview
 * 
 * State drives UI updates in Compose through observation.
 */
object StateOverview {
    
    // State in Compose - basic example
    @Composable
    fun BasicState() {
        // State declaration
        var message by androidx.compose.runtime.mutableStateOf("Hello")
        
        // Display state
        androidx.compose.material3.Text(text = message)
        
        // Update state
        androidx.compose.material3.Button(onClick = { message = "Updated!" }) {
            androidx.compose.material3.Text("Change")
        }
    }
    
    // Using remember
    @Composable
    fun RememberState() {
        // Remember preserves state across recompositions
        var count by androidx.compose.runtime.remember { 
            androidx.compose.runtime.mutableIntStateOf(0) 
        }
        
        androidx.compose.material3.Text("Count: $count")
        androidx.compose.material3.Button(onClick = { count++ }) {
            androidx.compose.material3.Text("Increment")
        }
    }
    
    // Using rememberSaveable
    @Composable
    fun PersistedState() {
        // Survives process death
        var name by androidx.compose.runtime.saveable.rememberSaveable { 
            androidx.compose.runtime.mutableStateOf("") 
        }
        
        androidx.compose.material3.OutlinedTextField(
            value = name,
            onValueChange = { name = it },
            label = { androidx.compose.material3.Text("Name") }
        )
    }
}
```

---

## SECTION 2: STATE AND VIEWMODEL

```kotlin
/**
 * State and ViewModel
 * 
 * ViewModel provides state management for Compose.
 */
class StateAndViewModel {
    
    // ViewModel with StateFlow
    class CounterViewModel : androidx.lifecycle.ViewModel() {
        
        private val _count = androidx.coroutines.flow.MutableStateFlow(0)
        val count: androidx.coroutines.flow.StateFlow<Int> = _count
        
        fun increment() {
            _count.value++
        }
        
        fun decrement() {
            _count.value--
        }
        
        fun reset() {
            _count.value = 0
        }
    }
    
    // Using ViewModel in Compose
    @Composable
    fun CounterScreen(
        viewModel: CounterViewModel = androidx.lifecycle.viewmodel.compose.viewmodel()
    ) {
        // Collect state
        val count by viewModel.count.collectAsState()
        
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.padding(16.dp)
        ) {
            androidx.compose.material3.Text(
                text = "Count: $count",
                style = androidx.compose.material3.MaterialTheme.typography.headlineLarge
            )
            
            androidx.compose.foundation.layout.Row(
                horizontalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                androidx.compose.material3.Button(onClick = viewModel::decrement) {
                    androidx.compose.material3.Text("-")
                }
                
                androidx.compose.material3.Button(onClick = viewModel::increment) {
                    androidx.compose.material3.Text("+")
                }
            }
            
            androidx.compose.material3.TextButton(onClick = viewModel::reset) {
                androidx.compose.material3.Text("Reset")
            }
        }
    }
    
    // UI State pattern
    data class UiState(
        val isLoading: Boolean = false,
        val data: String? = null,
        val error: String? = null
    )
    
    class DataViewModel : androidx.lifecycle.ViewModel() {
        
        private val _uiState = androidx.coroutines.flow.MutableStateFlow(UiState())
        val uiState: androidx.coroutines.flow.StateFlow<UiState> = _uiState
        
        fun loadData() {
            androidx.lifecycle.viewModelScope.launch {
                _uiState.value = UiState(isLoading = true)
                
                try {
                    // Simulate API call
                    kotlinx.coroutines.delay(1000)
                    _uiState.value = UiState(data = "Data loaded successfully!")
                } catch (e: Exception) {
                    _uiState.value = UiState(error = e.message ?: "Unknown error")
                }
            }
        }
        
        fun clearError() {
            _uiState.value = _uiState.value.copy(error = null)
        }
    }
    
    @Composable
    fun DataScreen(
        viewModel: DataViewModel = androidx.lifecycle.viewmodel.compose.viewmodel()
    ) {
        val uiState by viewModel.uiState.collectAsState()
        
        when {
            uiState.isLoading -> {
                androidx.compose.material3.CircularProgressIndicator()
            }
            uiState.error != null -> {
                Column {
                    androidx.compose.material3.Text(
                        text = uiState.error!!,
                        color = androidx.compose.material3.MaterialTheme.colorScheme.error
                    )
                    androidx.compose.material3.Button(onClick = viewModel::clearError) {
                        androidx.compose.material3.Text("Retry")
                    }
                }
            }
            uiState.data != null -> {
                androidx.compose.material3.Text(text = uiState.data!!)
            }
        }
    }
}
```

---

## SECTION 3: COLLECTING STATE

```kotlin
/**
 * Collecting State
 * 
 * Different ways to collect state in Compose.
 */
object CollectingState {
    
    // collectAsState with lifecycle awareness
    @Composable
    fun CollectAsStateExample(flow: androidx.coroutines.flow.Flow<String>) {
        val value by flow.collectAsState()
        androidx.compose.material3.Text(value)
    }
    
    // collectAsStateWithLifecycle (recommended)
    @Composable
    fun CollectWithLifecycle(flow: androidx.coroutines.flow.Flow<String>) {
        val value by flow.collectAsStateWithLifecycle()
        androidx.compose.material3.Text(value)
    }
    
    // produceState for one-shot values
    @Composable
    fun ProduceStateExample(dataLoader: suspend () -> String): String {
        return androidx.compose.runtime.produceState(initialValue = "") {
            value = dataLoader()
        }.value
    }
    
    // Derived state
    @Composable
    fun DerivedStateExample() {
        val items = listOf(1, 2, 3, 4, 5)
        var filter by androidx.compose.runtime.remember { 
            androidx.compose.runtime.mutableStateOf("") 
        }
        
        // Derived state - only recomputes when items or filter changes
        val filteredItems = androidx.compose.runtime.remember(items, filter) {
            items.filter { it.toString().contains(filter) }
        }
        
        Column {
            androidx.compose.material3.OutlinedTextField(
                value = filter,
                onValueChange = { filter = it },
                label = { androidx.compose.material3.Text("Filter") }
            )
            
            filteredItems.forEach { item ->
                androidx.compose.material3.Text(item.toString())
            }
        }
    }
    
    // using `key` parameter for selective collection
    @Composable
    fun SelectiveCollection(
        viewModel: MyViewModel
    ) {
        // Only collect specific state properties
        val loadingState by viewModel.loadingState.collectAsState()
        val dataState by viewModel.dataState.collectAsState()
        
        // Use individual states
        if (loadingState) {
            androidx.compose.material3.CircularProgressIndicator()
        }
        
        dataState?.let { data ->
            androidx.compose.material3.Text(data)
        }
    }
    
    class MyViewModel : androidx.lifecycle.ViewModel() {
        val loadingState = androidx.coroutines.flow.MutableStateFlow(false)
        val dataState = androidx.coroutines.flow.MutableStateFlow<String?>(null)
    }
}
```

---

## SECTION 4: UNIDIRECTIONAL DATA FLOW

```kotlin
/**
 * Unidirectional Data Flow (UDF)
 * 
 * UDF pattern: UI events → ViewModel → UI State → UI
 */
object UnidirectionalDataFlow {
    
    // Event/Intent
    sealed class CounterEvent {
        object Increment : CounterEvent()
        object Decrement : CounterEvent()
        object Reset : CounterEvent()
    }
    
    // ViewModel with UDF
    class UdfViewModel : androidx.lifecycle.ViewModel() {
        
        private val _state = androidx.coroutines.flow.MutableStateFlow(UdfState())
        val state: androidx.coroutines.flow.StateFlow<UdfState> = _state
        
        // Handle events
        fun onEvent(event: CounterEvent) {
            when (event) {
                is CounterEvent.Increment -> {
                    _state.value = _state.value.copy(count = _state.value.count + 1)
                }
                is CounterEvent.Decrement -> {
                    _state.value = _state.value.copy(count = _state.value.count - 1)
                }
                is CounterEvent.Reset -> {
                    _state.value = _state.value.copy(count = 0)
                }
            }
        }
    }
    
    data class UdfState(
        val count: Int = 0,
        val lastModified: Long = System.currentTimeMillis()
    )
    
    // Compose UI with UDF
    @Composable
    fun UdfScreen(
        viewModel: UdfViewModel = androidx.lifecycle.viewmodel.compose.viewmodel()
    ) {
        val state by viewModel.state.collectAsState()
        
        // UI state -> UI
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.padding(16.dp)
        ) {
            // Display state
            androidx.compose.material3.Text(
                text = "Count: ${state.count}",
                style = androidx.compose.material3.MaterialTheme.typography.headlineLarge
            )
            
            androidx.compose.material3.Text(
                text = "Last modified: ${state.lastModified}",
                style = androidx.compose.material3.MaterialTheme.typography.bodySmall
            )
            
            // Events
            androidx.compose.foundation.layout.Row(
                horizontalArrangement = Arrangement.spacedBy(8.dp)
            ) {
                androidx.compose.material3.Button(
                    onClick = { viewModel.onEvent(CounterEvent.Decrement) }
                ) {
                    androidx.compose.material3.Text("-")
                }
                
                androidx.compose.material3.Button(
                    onClick = { viewModel.onEvent(CounterEvent.Increment) }
                ) {
                    androidx.compose.material3.Text("+")
                }
            }
            
            androidx.compose.material3.TextButton(
                onClick = { viewModel.onEvent(CounterEvent.Reset) }
            ) {
                androidx.compose.material3.Text("Reset")
            }
        }
    }
}
```

---

## SECTION 5: STATE HANDLE AND SIDE EFFECTS

```kotlin
/**
 * State Handle and Side Effects
 * 
 * Handling side effects in Compose.
 */
class StateEffects {
    
    // LaunchedEffect for one-shot side effects
    @Composable
    fun LaunchedEffectExample() {
        var data by androidx.compose.runtime.mutableStateOf<String?>(null)
        
        androidx.compose.runtime.LaunchedEffect(Unit) {
            // This runs on first composition
            data = loadData()
        }
        
        data?.let {
            androidx.compose.material3.Text(it)
        }
    }
    
    suspend fun loadData(): String {
        kotlinx.coroutines.delay(1000)
        return "Loaded data"
    }
    
    // LaunchedEffect with key
    @Composable
    fun KeyedLaunchedEffect(userId: String) {
        var user by androidx.compose.runtime.mutableStateOf<User?>(null)
        
        // Re-runs when userId changes
        androidx.compose.runtime.LaunchedEffect(userId) {
            user = fetchUser(userId)
        }
        
        user?.let { u ->
            androidx.compose.material3.Text(u.name)
        }
    }
    
    suspend fun fetchUser(id: String): User? {
        kotlinx.coroutines.delay(500)
        return User(id, "User $id")
    }
    
    data class User(val id: String, val name: String)
    
    // RememberCoroutineScope for structured concurrency
    @Composable
    fun CoroutineScopeExample() {
        val scope = androidx.compose.runtime.rememberCoroutineScope()
        
        androidx.compose.material3.Button(
            onClick = {
                scope.launch {
                    // Launch coroutine in composition
                    kotlinx.coroutines.delay(1000)
                    println("Background work done")
                }
            }
        ) {
            androidx.compose.material3.Text("Start Task")
        }
    }
    
    // DisposableEffect for cleanup
    @Composable
    fun DisposableEffectExample() {
        var text by androidx.compose.runtime.mutableStateOf("")
        
        androidx.compose.foundation.text.KeyboardOptions
        androidx.compose.material3.OutlinedTextField(
            value = text,
            onValueChange = { text = it },
            label = { androidx.compose.material3.Text("Enter text") }
        )
        
        // Cleanup on disposal
        androidx.compose.runtime.DisposableEffect(text) {
            onDispose {
                println("Text changed to: $text")
            }
        }
    }
    
    // SideEffect for non-compose state
    @Composable
    fun SideEffectExample() {
        var hasPermission by androidx.compose.runtime.mutableStateOf(false)
        
        // Run on every successful recomposition
        androidx.compose.runtime.SideEffect {
            // Can modify non-Compose state
            hasPermission = checkPermission()
        }
    }
    
    fun checkPermission(): Boolean = true
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: State not updating UI**
- Solution: Use mutableStateOf or StateFlow, call collectAsState() in Compose, ensure proper state observation

**Pitfall 2: State resets on configuration change**
- Solution: Use rememberSaveable or ViewModel, use StateFlow with ViewModel, Survives configuration change

**Pitfall 3: Infinite loop with derived state**
- Solution: Use remember for computed values, use derivedStateOf for expensive computation, avoid side effects during composition

**Pitfall 4: Memory leaks with coroutines**
- Solution: Use viewModelScope for ViewModel coroutines, use LaunchedEffect in Compose, Clean up in DisposableEffect

---

## Best Practices

1. Use StateFlow in ViewModels
2. Use collectAsState() in Compose
3. Follow unidirectional data flow
4. Use sealed classes for UI state
5. Handle loading/error states explicitly
6. Use remember for local state
7. Use rememberSaveable for persisted state
8. Use LaunchedEffect for side effects
9. Clean up in DisposableEffect
10. Test state management separately

---

## EXAMPLE 1: COMPLETE STATE MANAGEMENT

```kotlin
/**
 * Complete State Management Example
 * 
 * Full implementation with all state patterns.
 */
class CompleteStateExample {
    
    // UI State sealed class
    sealed class HomeUiState {
        object Loading : HomeUiState()
        data class Success(val items: List<String>) : HomeUiState()
        data class Error(val message: String) : HomeUiState()
    }
    
    // Event sealed class
    sealed class HomeEvent {
        object Refresh : HomeEvent()
        data class Delete(val item: String) : HomeEvent()
        data class Navigate(val item: String) : HomeEvent()
    }
    
    // ViewModel
    class HomeViewModel : androidx.lifecycle.ViewModel() {
        
        private val _uiState = androidx.coroutines.flow.MutableStateFlow<HomeUiState>(
            HomeUiState.Loading
        )
        val uiState: androidx.coroutines.flow.StateFlow<HomeUiState> = _uiState
        
        private val _navigationEvent = androidx.coroutines.flow.MutableSharedFlow<String>()
        val navigationEvent: androidx.coroutines.flow.SharedFlow<String> = _navigationEvent
        
        init {
            loadItems()
        }
        
        fun onEvent(event: HomeEvent) {
            when (event) {
                is HomeEvent.Refresh -> loadItems()
                is HomeEvent.Delete -> deleteItem(event.item)
                is HomeEvent.Navigate -> navigateTo(event.item)
            }
        }
        
        private fun loadItems() {
            androidx.lifecycle.viewModelScope.launch {
                _uiState.value = HomeUiState.Loading
                try {
                    kotlinx.coroutines.delay(1000)
                    _uiState.value = HomeUiState.Success(
                        listOf("Item 1", "Item 2", "Item 3")
                    )
                } catch (e: Exception) {
                    _uiState.value = HomeUiState.Error(e.message ?: "Error loading")
                }
            }
        }
        
        private fun deleteItem(item: String) {
            val currentState = _uiState.value
            if (currentState is HomeUiState.Success) {
                _uiState.value = currentState.copy(
                    items = currentState.items.filter { it != item }
                )
            }
        }
        
        private fun navigateTo(item: String) {
            androidx.lifecycle.viewModelScope.launch {
                _navigationEvent.emit(item)
            }
        }
    }
    
    // Compose Screen
    @Composable
    fun HomeScreen(
        viewModel: HomeViewModel = androidx.lifecycle.viewmodel.compose.viewmodel()
    ) {
        val uiState by viewModel.uiState.collectAsState()
        
        // Handle navigation events
        androidx.compose.runtime.LaunchedEffect(Unit) {
            viewModel.navigationEvent.collect { item ->
                println("Navigate to: $item")
            }
        }
        
        when (val state = uiState) {
            is HomeUiState.Loading -> {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    androidx.compose.material3.CircularProgressIndicator()
                }
            }
            
            is HomeUiState.Success -> {
                androidx.compose.material3.Scaffold(
                    topBar = {
                        androidx.compose.material3.TopAppBar(
                            title = { androidx.compose.material3.Text("Home") },
                            actions = {
                                androidx.compose.material3.IconButton(
                                    onClick = { viewModel.onEvent(HomeEvent.Refresh) }
                                ) {
                                    androidx.compose.material3.Icon(
                                        androidx.compose.material.icons.Icons.Default.Refresh,
                                        contentDescription = "Refresh"
                                    )
                                }
                            }
                        )
                    }
                ) { paddingValues ->
                    LazyColumn(
                        modifier = Modifier
                            .fillMaxSize()
                            .padding(paddingValues)
                    ) {
                        items(state.items) { item ->
                            ListItem(
                                headlineContent = { androidx.compose.material3.Text(item) },
                                trailingContent = {
                                    androidx.compose.material3.IconButton(
                                        onClick = { viewModel.onEvent(HomeEvent.Delete(item)) }
                                    ) {
                                        androidx.compose.material3.Icon(
                                            androidx.compose.material.icons.Icons.Default.Delete,
                                            contentDescription = "Delete"
                                        )
                                    }
                                },
                                modifier = Modifier.clickable {
                                    viewModel.onEvent(HomeEvent.Navigate(item))
                                }
                            )
                        }
                    }
                }
            }
            
            is HomeUiState.Error -> {
                Box(
                    modifier = Modifier.fillMaxSize(),
                    contentAlignment = Alignment.Center
                ) {
                    Column(horizontalAlignment = Alignment.CenterHorizontally) {
                        androidx.compose.material3.Text(
                            text = state.message,
                            color = androidx.compose.material3.MaterialTheme.colorScheme.error
                        )
                        androidx.compose.material3.Button(
                            onClick = { viewModel.onEvent(HomeEvent.Refresh) }
                        ) {
                            androidx.compose.material3.Text("Retry")
                        }
                    }
                }
            }
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**State in Compose:**
- mutableStateOf: Basic state
- remember: Survives recomposition
- rememberSaveable: Survives process death
- StateFlow: For ViewModels

**Collecting State:**
- collectAsState: Basic collection
- collectAsStateWithLifecycle: Lifecycle-aware
- produceState: One-shot values
- remember + lambda: Derived state

**UDF Pattern:**
- Events/Intents: User actions
- ViewModel: Processes events, updates state
- UI State: Represented by data class/sealed
- UI: Renders based on state

**Side Effects:**
- LaunchedEffect: One-shot effects
- DisposableEffect: Cleanup
- SideEffect: Non-Compose state
- rememberCoroutineScope: Structured concurrency

**Best Practices:**
- Use sealed classes for state
- Separate events from state
- Use collectAsState for StateFlow
- Handle all state cases explicitly

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/02_Composable_Functions.md
- See: 03_ARCHITECTURE/01_Architecture_Patterns/02_MVVM_Implementation.md

---

## END OF STATE MANAGEMENT COMPOSE GUIDE
