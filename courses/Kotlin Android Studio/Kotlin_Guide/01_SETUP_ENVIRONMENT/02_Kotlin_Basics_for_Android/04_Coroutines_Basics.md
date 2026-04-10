# LEARNING OBJECTIVES

1. Understanding coroutine fundamentals
2. Working with coroutine scopes and dispatchers
3. Implementing async operations with coroutines
4. Managing coroutine cancellation
5. Using coroutine patterns in Android

```kotlin
package com.kotlin.coroutines
```

---

## SECTION 1: COROUTINE FUNDAMENTALS

```kotlin
/**
 * Coroutine Fundamentals
 * 
 * Coroutines are Kotlin's solution for asynchronous programming.
 * They are lightweight threads that can be suspended and resumed.
 */
object CoroutineFundamentals {
    
    // Coroutine builder functions
    suspend fun basicCoroutine() {
        // launch - fire and forget
        kotlinx.coroutines.GlobalScope.launch {
            println("Running in coroutine")
        }
        
        // async - returns a value
        val deferred = kotlinx.coroutines.GlobalScope.async {
            42
        }
        val result = deferred.await()
        println("Result: $result")
        
        // runBlocking - blocks current thread
        kotlinx.coroutines.runBlocking {
            println("Blocking coroutine")
        }
    }
    
    // Suspend functions
    suspend fun suspendFunction(): String {
        kotlinx.coroutines.delay(1000)  // Non-blocking delay
        return "Result after delay"
    }
    
    // Coroutine scope
    fun coroutineScopeExample() = kotlinx.coroutines.coroutineScope {
        // Creates a new coroutine scope
        launch { println("Task 1") }
        launch { println("Task 2") }
    }
}
```

---

## SECTION 2: COROUTINE SCOPES AND DISPATCHERS

```kotlin
/**
 * Coroutine Scopes and Dispatchers
 * 
 * Scopes manage coroutine lifecycle.
 * Dispatchers determine which thread the coroutine runs on.
 */
class ScopesAndDispatchers {
    
    // Dispatchers
    object Dispatchers {
        // Main - UI thread
        val main = kotlinx.coroutines.Dispatchers.Main
        
        // IO - for I/O operations
        val io = kotlinx.coroutines.Dispatchers.IO
        
        // Default - for CPU-intensive work
        val default = kotlinx.coroutines.Dispatchers.Default
        
        // Unconfined - starts in caller thread
        val unconfined = kotlinx.coroutines.Dispatchers.Unconfined
    }
    
    // CoroutineScope
    class MyActivity : android.app.Activity(), kotlinx.coroutines.CoroutineScope {
        override val coroutineContext: kotlinx.coroutines.CoroutineContext
            get() = Dispatchers.Main + kotlinx.coroutines.Job()
        
        fun doAsyncWork() {
            launch(Dispatchers.IO) {
                // Background work
                val data = fetchData()
                
                // Back to main thread
                launch(Dispatchers.Main) {
                    updateUI(data)
                }
            }
        }
        
        private suspend fun fetchData(): String {
            kotlinx.coroutines.delay(1000)
            return "Data"
        }
        
        private fun updateUI(data: String) {
            println("UI updated with: $data")
        }
        
        override fun onDestroy() {
            super.onDestroy()
            coroutineContext[kotlinx.coroutines.Job]?.cancel()
        }
    }
    
    // ViewModelScope
    class MyViewModel : androidx.lifecycle.ViewModel() {
        // Using viewModelScope - automatically cancelled on ViewModel clear
        fun loadData() {
            androidx.lifecycle.viewModelScope.launch {
                val data = fetchData()
                _data.value = data
            }
        }
        
        private suspend fun fetchData(): String {
            kotlinx.coroutines.delay(1000)
            return "Loaded data"
        }
        
        private val _data = androidx.lifecycle.MutableLiveData<String>()
    }
    
    // LifecycleScope
    class MyFragment : android.app.Fragment() {
        // Using lifecycleScope - automatically cancelled on fragment destroy
        fun loadData() {
            viewLifecycleOwner.lifecycleScope.launch {
                val data = fetchData()
                updateUI(data)
            }
        }
        
        private suspend fun fetchData(): String {
            kotlinx.coroutines.delay(1000)
            return "Loaded data"
        }
        
        private fun updateUI(data: String) {
            println("UI: $data")
        }
    }
}
```

---

## SECTION 3: SUSPEND FUNCTIONS AND FLOW

```kotlin
/**
 * Suspend Functions and Flow
 * 
 * Flow is Kotlin's solution for handling asynchronous data streams.
 */
class SuspendAndFlow {
    
    // Suspend functions
    suspend fun fetchUser(userId: Int): User? {
        kotlinx.coroutines.delay(500)  // Simulate network call
        return User(userId, "User$userId", "user$userId@example.com")
    }
    
    suspend fun fetchUsers(): List<User> {
        kotlinx.coroutines.delay(500)
        return listOf(
            User(1, "Alice", "alice@example.com"),
            User(2, "Bob", "bob@example.com")
        )
    }
    
    // Flow - cold asynchronous stream
    fun userFlow(): kotlinx.coroutines.flow.Flow<User> = kotlinx.coroutines.flow.flow {
        val users = fetchUsers()
        users.forEach { emit(it) }
    }
    
    // StateFlow - state holder
    class StateHolder : androidx.lifecycle.ViewModel() {
        private val _state = kotlinx.coroutines.flow.MutableStateFlow(State())
        val state: kotlinx.coroutines.flow.StateFlow<State> = _state
        
        fun updateState(newValue: String) {
            _state.value = State(value = newValue)
        }
        
        data class State(val value: String = "", val loading: Boolean = false)
    }
    
    // SharedFlow - hot stream
    class EventHolder : androidx.lifecycle.ViewModel() {
        private val _events = kotlinx.coroutines.flow.MutableSharedFlow<String>()
        val events: kotlinx.coroutines.flow.SharedFlow<String> = _events
        
        fun emitEvent(event: String) {
            androidx.lifecycle.viewModelScope.launch {
                _events.emit(event)
            }
        }
    }
    
    data class User(val id: Int, val name: String, val email: String)
}
```

---

## SECTION 4: COROUTINE PATTERNS

```kotlin
/**
 * Coroutine Patterns
 * 
 * Common patterns for working with coroutines.
 */
class CoroutinePatterns {
    
    // Sequential execution
    suspend fun sequential() {
        val result1 = doFirst()
        val result2 = doSecond(result1)
        val result3 = doThird(result2)
        println(result3)
    }
    
    // Parallel execution
    suspend fun parallel() {
        val deferred1 = androidx.lifecycle.viewModelScope.async { doFirst() }
        val deferred2 = androidx.lifecycle.viewModelScope.async { doSecond("") }
        val deferred3 = androidx.lifecycle.viewModelScope.async { doThird("") }
        
        val result1 = deferred1.await()
        val result2 = deferred2.await()
        val result3 = deferred3.await()
        
        println("$result1, $result2, $result3")
    }
    
    // Timeout
    suspend fun withTimeout() {
        try {
            kotlinx.coroutines.withTimeout(1000) {
                longRunningTask()
            }
        } catch (e: kotlinx.coroutines.TimeoutCancellationException) {
            println("Task timed out")
        }
    }
    
    // Retry with exponential backoff
    suspend fun withRetry(): String {
        return kotlinx.coroutines.flow.flow {
            emit(doNetworkCall())
        }.retry(3) { cause ->
            println("Retrying after error: ${cause.message}")
            true  // Retry condition
        }.first()
    }
    
    suspend fun doFirst(): String { delay(100); return "First" }
    suspend fun doSecond(input: String): String { delay(100); return "$input + Second" }
    suspend fun doThird(input: String): String { delay(100); return "$input + Third" }
    suspend fun longRunningTask() { delay(2000) }
    suspend fun doNetworkCall(): String { delay(100); return "Data" }
    private fun delay(ms: Long) = kotlinx.coroutines.delay(ms)
}
```

---

## SECTION 5: CANCELLATION AND ERROR HANDLING

```kotlin
/**
 * Cancellation and Error Handling
 * 
 * Proper handling of coroutine cancellation and exceptions.
 */
class CancellationAndErrorHandling {
    
    // Cooperative cancellation
    suspend fun cooperativeCancellation() {
        for (i in 1..1000) {
            if (!isActive) break  // Check cancellation
            // Do work
            println("Working: $i")
            kotlinx.coroutines.delay(10)
        }
    }
    
    // Ensure cleanup
    suspend fun withFinally() {
        try {
            kotlinx.coroutines.withContext(Dispatchers.IO) {
                // Task that might fail
            }
        } finally {
            // Cleanup code always runs
            println("Cleanup")
        }
    }
    
    // Exception handling
    suspend fun exceptionHandling() {
        val handler = kotlinx.coroutines.CoroutineExceptionHandler { _, exception ->
            println("Caught: $exception")
        }
        
        androidx.lifecycle.viewModelScope.launch(handler) {
            throw RuntimeException("Error")
        }
    }
    
    // SupervisorJob - failure doesn't cancel siblings
    suspend fun supervisorScope() = kotlinx.coroutines.supervisorScope {
        launch { throw RuntimeException("Error in child 1") }
        launch { delay(100); println("Child 2 completed") }
    }
    
    // Result wrapper
    suspend fun <T> safeCall(block: suspend () -> T): Result<T> {
        return try {
            Result.success(block())
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: Forgetting to cancel coroutines**
- Solution: Use structured concurrency, properly scope to lifecycle, cancel in onDestroy/onCleared

**Pitfall 2: Blocking main thread**
- Solution: Use withContext(Dispatchers.IO), don't use runBlocking in production

**Pitfall 3: Memory leaks with coroutines**
- Solution: Use proper scope (viewModelScope, lifecycleScope), cancel when lifecycle ends, avoid GlobalScope

**Pitfall 4: Not handling exceptions**
- Solution: Use try-catch in coroutines, use CoroutineExceptionHandler, wrap in Result

**Pitfall 5: Confusing launch and async**
- Solution: launch for fire-and-forget, async when you need a result

---

## Best Practices

1. Use structured concurrency
2. Prefer viewModelScope/lifecycleScope
3. Use appropriate dispatcher
4. Handle exceptions properly
5. Use Flow for data streams
6. Cancel coroutines properly
7. Use suspend for async operations
8. Avoid GlobalScope in production
9. Check isActive for long operations
10. Use withTimeout for operations

---

## Troubleshooting Guide

**Issue: Coroutine not running**
- Steps: 1. Check if scope is active 2. Verify dispatcher 3. Check for exceptions

**Issue: Not switching to main thread**
- Steps: 1. Use withContext(Dispatchers.Main) 2. Use correct scope 3. Check launch context

**Issue: Memory leak**
- Steps: 1. Check lifecycle binding 2. Verify cancellation 3. Use proper scope

---

## Advanced Tips and Tricks

- **Tip 1: Use channel for communication** - Inter-coroutine messaging, Better than shared state

- **Tip 2: Use SharedFlow for events** - One-time events, Survives configuration changes

- **Tip 3: Use StateFlow for state** - Replaces LiveData, Initial value required

- **Tip 4: Use Flow operators** - map, filter, transform, debounce, distinctUntilChanged

- **Tip 5: Use receiveAsFlow** - Convert Channel to Flow, For event streams

---

## EXAMPLE 1: ANDROID NETWORK CALL WITH COROUTINES

```kotlin
/**
 * Network Call Example with Coroutines
 * 
 * Typical pattern for making network calls in Android.
 */
class NetworkCallExample {
    
    // Repository pattern with coroutines
    class UserRepository {
        private val api = RetrofitClient.api
        
        suspend fun getUsers(): Result<List<User>> {
            return try {
                val response = api.getUsers()
                if (response.isSuccessful) {
                    Result.success(response.body() ?: emptyList())
                } else {
                    Result.failure(Exception("Error: ${response.code()}"))
                }
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
    }
    
    // ViewModel with coroutines
    class UserViewModel(private val repository: UserRepository) : androidx.lifecycle.ViewModel() {
        
        private val _uiState = androidx.lifecycle.MutableLiveData<UiState>()
        val uiState: androidx.lifecycle.LiveData<UiState> = _uiState
        
        fun loadUsers() {
            viewModelScope.launch {
                _uiState.value = UiState.Loading
                
                repository.getUsers()
                    .onSuccess { users ->
                        _uiState.value = UiState.Success(users)
                    }
                    .onFailure { error ->
                        _uiState.value = UiState.Error(error.message ?: "Unknown error")
                    }
            }
        }
        
        sealed class UiState {
            object Loading : UiState()
            data class Success(val users: List<User>) : UiState()
            data class Error(val message: String) : UiState()
        }
    }
    
    data class User(val id: Int, val name: String, val email: String)
    
    // Simple Retrofit setup
    object RetrofitClient {
        // Simplified - use actual Retrofit setup
        val api: UserApi get() = TODO("Initialize Retrofit")
    }
    
    interface UserApi {
        suspend fun getUsers(): retrofit2.Response<List<User>>
    }
}
```

---

## EXAMPLE 2: ROOM DATABASE WITH COROUTINES

```kotlin
/**
 * Room Database with Coroutines
 * 
 * Using coroutines with Room database.
 */
class RoomCoroutineExample {
    
    // DAO with suspend functions
    /*
    @Dao
    interface UserDao {
        @Query("SELECT * FROM users")
        suspend fun getAllUsers(): List<User>
        
        @Query("SELECT * FROM users WHERE id = :id")
        suspend fun getUserById(id: Int): User?
        
        @Insert(onConflict = OnConflictStrategy.REPLACE)
        suspend fun insertUser(user: User)
        
        @Delete
        suspend fun deleteUser(user: User)
        
        @Query("SELECT * FROM users WHERE name LIKE :query")
        fun searchUsers(query: String): kotlinx.coroutines.flow.Flow<List<User>>
    }
    */
    
    // Repository with Flow
    class UserRepository(private val userDao: UserDao) {
        
        // Get all users as Flow - auto-updates
        fun getAllUsersFlow(): kotlinx.coroutines.flow.Flow<List<User>> {
            return userDao.getAllUsersFlow()
        }
        
        suspend fun getUserById(id: Int): User? {
            return userDao.getUserById(id)
        }
        
        suspend fun insertUser(user: User) {
            userDao.insertUser(user)
        }
        
        suspend fun deleteUser(user: User) {
            userDao.deleteUser(user)
        }
    }
    
    // ViewModel with Flow collection
    class UserListViewModel(private val repository: UserRepository) : androidx.lifecycle.ViewModel() {
        
        val users: kotlinx.coroutines.flow.StateFlow<List<User>> = 
            repository.getAllUsersFlow()
                .stateIn(
                    scope = viewModelScope,
                    started = kotlinx.coroutines.flow.WhileSubscribed(5000),
                    initialValue = emptyList()
                )
    }
    
    data class User(val id: Int, val name: String, val email: String)
}
```

---

## EXAMPLE 3: COMPLETE COROUTINE PATTERNS

```kotlin
/**
 * Complete Coroutine Patterns
 * 
 * Production-ready patterns for coroutines in Android.
 */
class CompleteCoroutinePatterns {
    
    // Flow collection with lifecycle
    class FlowCollection(private val viewModel: androidx.lifecycle.ViewModel) {
        
        fun collectFlow() {
            val flow = getDataFlow()
            
            viewModel.lifecycleScope.launch {
                viewModel.repeatOnLifecycle(androidx.lifecycle.Lifecycle.State.STARTED) {
                    flow.collect { data ->
                        updateUI(data)
                    }
                }
            }
        }
        
        private suspend fun updateUI(data: String) {}
        private fun getDataFlow(): kotlinx.coroutines.flow.Flow<String> = TODO()
    }
    
    // Parallel with combine
    suspend fun combineParallel() {
        val flow1 = getFlow1()
        val flow2 = getFlow2()
        
        kotlinx.coroutines.flow.combine(flow1, flow2) { a, b ->
            "$a and $b"
        }.collect { combined ->
            println(combined)
        }
    }
    
    // Debounced search
    class SearchViewModel : androidx.lifecycle.ViewModel() {
        private val _searchQuery = androidx.lifecycle.MutableLiveData("")
        
        val searchResults: kotlinx.coroutines.flow.Flow<List<Result>> = 
            _searchQuery.asFlow()
                .debounce(300)
                .filter { it.length >= 2 }
                .distinctUntilChanged()
                .flatMapLatest { query ->
                    searchAPI(query)
                }
                .stateIn(
                    viewModelScope,
                    kotlinx.coroutines.flow.WhileSubscribed(5000),
                    emptyList()
                )
        
        private fun <T> androidx.lifecycle.MutableLiveData<T>.asFlow(): kotlinx.coroutines.flow.Flow<T> = TODO()
        private fun searchAPI(query: String): kotlinx.coroutines.flow.Flow<List<Result>> = TODO()
        
        data class Result(val id: Int, val title: String)
    }
    
    // Error boundary with catch
    fun errorBoundary() = kotlinx.coroutines.flow.flow {
        emit(1)
        throw RuntimeException("Error")
    }.catch { e ->
        println("Caught: $e")
        emit(-1)  // Emit fallback value
    }.collect { value ->
        println("Value: $value")
    }
    
    private fun getFlow1() = kotlinx.coroutines.flow.flowOf("A")
    private fun getFlow2() = kotlinx.coroutines.flow.flowOf("B")
}
```

---

## OUTPUT STATEMENT RESULTS

**Coroutine Fundamentals:**
- launch: Fire and forget
- async: Returns Deferred (future)
- runBlocking: Blocks thread
- suspend: Mark function as async

**Dispatchers:**
- Main: UI thread (Android)
- IO: Network/DB operations
- Default: CPU-intensive
- Unconfined: Caller thread

**Scopes:**
- GlobalScope: Long-running, manual management
- viewModelScope: ViewModel lifecycle
- lifecycleScope: Activity/Fragment lifecycle
- coroutineScope: Structured concurrency

**Flow Types:**
- Flow: Cold stream
- StateFlow: State holder with current value
- SharedFlow: Hot event stream

**Patterns:**
- Sequential: Await each task
- Parallel: Use async for multiple tasks
- Timeout: withTimeout/withTimeoutOrNull
- Retry: flow.retry

**Error Handling:**
- try-catch in coroutines
- CoroutineExceptionHandler
- Result wrapper
- SupervisorJob

---

## CROSS-REFERENCES

- See: 01_Kotlin_Basics_for_Android/02_Android_Kotlin_Conventions.md
- See: 01_Kotlin_Basics_for_Android/03_Type_System_and_Collections.md
- See: 05_NETWORKING/01_HTTP_Communication/01_Retrofit_Basics.md
- See: 04_DATA_PERSISTENCE/01_Database_Development/01_Room_Database_Basics.md

---

## END OF COROUTINES BASICS GUIDE
