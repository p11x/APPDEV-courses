# MVI Pattern

## Learning Objectives

1. Understanding MVI (Model-View-Intent) pattern
2. Implementing MVI with sealed classes
3. Managing state and events in MVI
4. Using unidirectional data flow
5. Comparing MVI with MVVM

## Section 1: MVI Overview

MVI (Model-View-Intent) Overview

MVI provides unidirectional data flow with explicit intents.

```kotlin
object MVIOverview {
    
    // Components
    // Model: UI State (immutable)
    // View: Renders state (Activity/Fragment/Compose)
    // Intent: User actions/events
    
    // Data flow: Intent -> Processor -> State -> View
}
```

## Section 2: MVI Implementation

MVI Implementation

```kotlin
object MVIImplementation {
    
    // State
    data class CounterState(
        val count: Int = 0,
        val isLoading: Boolean = false,
        val error: String? = null
    )
    
    // Intent/Event
    sealed class CounterIntent {
        object Increment : CounterIntent()
        object Decrement : CounterIntent()
        object Reset : CounterIntent()
        object LoadInitial : CounterIntent()
    }
    
    // Side Effects (one-time events)
    sealed class CounterEffect {
        data class ShowMessage(val message: String) : CounterEffect()
        object NavigateToDetails : CounterEffect()
    }
    
    // ViewModel
    class CounterViewModel : androidx.lifecycle.ViewModel() {
        
        private val _state = androidx.coroutines.flow.MutableStateFlow(CounterState())
        val state: androidx.coroutines.flow.StateFlow<CounterState> = _state
        
        private val _effects = androidx.coroutines.flow.MutableSharedFlow<CounterEffect>()
        val effects: androidx.coroutines.flow.SharedFlow<CounterEffect> = _effects
        
        fun processIntent(intent: CounterIntent) {
            when (intent) {
                is CounterIntent.Increment -> increment()
                is CounterIntent.Decrement -> decrement()
                is CounterIntent.Reset -> reset()
                is CounterIntent.LoadInitial -> loadInitial()
            }
        }
        
        private fun increment() {
            _state.value = _state.value.copy(count = _state.value.count + 1)
            emitEffect(CounterEffect.ShowMessage("Count: ${_state.value.count}"))
        }
        
        private fun decrement() {
            if (_state.value.count > 0) {
                _state.value = _state.value.copy(count = _state.value.count - 1)
            }
        }
        
        private fun reset() {
            _state.value = CounterState()
        }
        
        private fun loadInitial() {
            viewModelScope.launch {
                _state.value = _state.value.copy(isLoading = true)
                // Simulate load
                kotlinx.coroutines.delay(500)
                _state.value = _state.value.copy(isLoading = false, count = 10)
            }
        }
        
        private fun emitEffect(effect: CounterEffect) {
            viewModelScope.launch {
                _effects.emit(effect)
            }
        }
    }
    
    // Compose UI
    @Composable
    fun CounterScreen(
        viewModel: CounterViewModel = androidx.lifecycle.viewmodel.compose.viewModel()
    ) {
        val state by viewModel.state.collectAsStateWithLifecycle()
        
        // Handle effects
        LaunchedEffect(Unit) {
            viewModel.effects.collect { effect ->
                when (effect) {
                    is CounterEffect.ShowMessage -> {
                        // Show snackbar
                    }
                    is CounterEffect.NavigateToDetails -> {
                        // Navigate
                    }
                }
            }
        }
        
        Column(
            horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.padding(16.dp)
        ) {
            if (state.isLoading) {
                CircularProgressIndicator()
            } else {
                Text(
                    text = "Count: ${state.count}",
                    style = MaterialTheme.typography.headlineLarge
                )
                
                Row(
                    horizontalArrangement = Arrangement.spacedBy(8.dp)
                ) {
                    Button(onClick = { viewModel.processIntent(CounterIntent.Decrement) }) {
                        Text("-")
                    }
                    Button(onClick = { viewModel.processIntent(CounterIntent.Increment) }) {
                        Text("+")
                    }
                }
                
                TextButton(onClick = { viewModel.processIntent(CounterIntent.Reset) }) {
                    Text("Reset")
                }
            }
        }
    }
}
```

## Output Statement Results

MVI Components:
- State: Immutable UI state
- Intent: User actions
- Effect: One-time events
- Reducer: Updates state based on intent

MVI Flow:
1. User interacts with View
2. View emits Intent
3. ViewModel processes Intent
4. ViewModel updates State
5. View re-renders with new State
6. ViewModel may emit Effects

Comparison with MVVM:
- MVI: Explicit intents, immutable state
- MVVM: Observable state, less explicit
- MVI: Better for complex state management