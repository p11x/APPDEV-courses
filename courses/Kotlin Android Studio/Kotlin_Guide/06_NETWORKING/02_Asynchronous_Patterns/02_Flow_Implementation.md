# Flow Implementation

## Learning Objectives

1. Understanding Kotlin Flow as a reactive stream
2. Implementing Flow with Retrofit for network requests
3. Using Flow operators for data transformation
4. Collecting Flow with proper coroutine scopes
5. Converting callbacks to Flow

## Prerequisites

- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Retrofit Basics](../01_HTTP_Communication/01_Retrofit_Basics.md)
- [OkHttp Configuration](../01_HTTP_Communication/02_OkHttp_Configuration.md)

## Section 1: Kotlin Flow Fundamentals

Kotlin Flow is a cold asynchronous stream that emits values sequentially. It's the modern way to handle async data streams in Kotlin.

Key Flow concepts:
- Cold stream - doesn't produce values until collected
- Cancellable - respects coroutine cancellation
- Sequential - emits values one at a time
- Structured - works with coroutine scopes

```kotlin
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.delay
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.onEach
import kotlinx.coroutines.flow.catch

// Simple Flow creation
class FlowBasics {
    
    // Create flow from list
    fun listToFlow(): Flow<Int> = flowOf(1, 2, 3, 4, 5)
    
    // Create flow using builder
    fun flowBuilder(): Flow<Int> = flow {
        for (i in 1..5) {
            delay(100)
            emit(i)
        }
    }
    
    // Flow with transformations
    fun transformedFlow(): Flow<String> = flowOf(1, 2, 3, 4, 5)
        .map { "Number: $it" }
        .filter { it.length > 8 }
    
    // Flow with transformations and error handling
    fun safeFlow(): Flow<String> = flowOf(1, 2, 3, 4, 5)
        .map { "Value: $it" }
        .catch { e -> emit("Error: ${e.message}") }
    
    // Transform operator
    fun transformFlow(): Flow<String> = flowOf(1, 2, 3)
        .transform { value ->
            emit("Start: $value")
            delay(50)
            emit("End: $value")
        }
}

// Flow operators
class FlowOperators {
    
    // Take - limit emissions
    fun takeFlow(): Flow<Int> = flowOf(1, 2, 3, 4, 5)
        .take(3)
    
    // Skip - skip emissions
    fun skipFlow(): Flow<Int> = flowOf(1, 2, 3, 4, 5)
        .skip(2)
    
    // First - get first or default
    fun firstFlow(): Flow<Int> = flowOf(1, 2, 3)
    
    // Distinct - remove duplicates
    fun distinctFlow(): Flow<Int> = flowOf(1, 2, 2, 3, 3, 3)
        .distinct()
    
    // Debounce - wait for quiet period
    fun debounceFlow(): Flow<Int> = flow {
        for (i in 1..10) {
            delay(50)
            emit(i)
        }
    }.debounce(100)
    
    // DistinctUntilChanged - only emit different from previous
    fun distinctUntilChanged(): Flow<Int> = flowOf(1, 1, 2, 2, 3, 2)
        .distinctUntilChanged()
    
    // OnEach - side effect for each emission
    fun onEachFlow(): Flow<Int> = flowOf(1, 2, 3)
        .onEach { println("Emitting: $it") }
    
    // Reduce - combine all values
    fun reduceFlow(): Flow<Int> = flowOf(1, 2, 3, 4, 5)
        .reduce { acc, value -> acc + value }
    
    // ToList - collect to list
    fun toListFlow(): kotlinx.coroutines.flow.FlowCollector<List<Int>> = 
        flowOf(1, 2, 3).toList()
}
```

## Section 2: Retrofit with Flow

Retrofit supports Flow natively, providing a clean way to handle network streams.

```kotlin
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow

// API service with Flow
interface FlowApiService {
    
    @GET("posts")
    fun getPosts(): Flow<List<Post>>
    
    @GET("posts/{id}")
    fun getPostById(@Path("id") id: Int): Flow<Post>
    
    @GET("posts")
    fun getPostsByUser(@Query("userId") userId: Int): Flow<List<Post>>
    
    @GET("posts")
    fun getPaginatedPosts(
        @Query("_page") page: Int,
        @Query("_limit") limit: Int
    ): Flow<List<Post>>
}

// Retrofit client with Flow support
object FlowRetrofitClient {
    
    private const val BASE_URL = "https://jsonplaceholder.typicode.com/"
    
    val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            // Flow is built-in with Retrofit 2.9.0+
            .build()
    }
    
    val apiService: FlowApiService by lazy {
        retrofit.create(FlowApiService::class.java)
    }
}

// Repository with Flow
class PostRepositoryFlow(
    private val apiService: FlowApiService
) {
    // Basic Flow collection
    fun getPostsFlow(): Flow<List<Post>> = flow {
        emit(emptyList())
        apiService.getPosts().collect { posts ->
            emit(posts)
        }
    }.flowOn(Dispatchers.IO)
    
    // Better approach - use flow builder with Retrofit
    fun getPosts(): Flow<List<Post>> = flow {
        val response = apiService.getPosts()
        response.collect { emit(it) }
    }.flowOn(Dispatchers.IO)
        .catch { e ->
            emit(emptyList())
            throw e
        }
    
    fun getPostById(id: Int): Flow<Post> = flow {
        apiService.getPostById(id).collect { emit(it) }
    }.flowOn(Dispatchers.IO)
    
    fun getPostsByUser(userId: Int): Flow<List<Post>> = flow {
        apiService.getPostsByUser(userId).collect { emit(it) }
    }.flowOn(Dispatchers.IO)
}
```

## Section 3: Flow with State Management

Flow integrates well with Android's state management, particularly with StateFlow and SharedFlow.

```kotlin
import kotlinx.coroutines.flow.*
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope

// State management with StateFlow
class StateFlowManager {
    
    private val _uiState = MutableStateFlow<UiState<List<Post>>>(UiState.Loading)
    val uiState: StateFlow<UiState<List<Post>>> = _uiState.asStateFlow()
    
    // Update state directly
    fun updateState(state: UiState<List<Post>>) {
        _uiState.value = state
    }
    
    // Update from repository
    fun loadData(repository: PostRepositoryFlow) {
        viewModelScope.launch {
            repository.getPosts()
                .collect { posts ->
                    _uiState.value = UiState.Success(posts)
                }
        }
    }
}

// Sealed class for UI state
sealed class UiState<out T> {
    object Loading : UiState<Nothing>()
    data class Success<T>(val data: T) : UiState<T>()
    data class Error(val message: String) : UiState<Nothing>()
    object Empty : UiState<Nothing>()
}

// SharedFlow for events
class EventManager {
    
    private val _events = MutableSharedFlow<Event>()
    val events: SharedFlow<Event> = _events.asSharedFlow()
    
    fun emitEvent(event: Event) {
        viewModelScope.launch {
            _events.emit(event)
        }
    }
    
    sealed class Event {
        data class Navigate(val destination: String) : Event()
        data class ShowSnackbar(val message: String) : Event()
        data class ShowError(val error: String) : Event()
        object RefreshList : Event()
    }
}

// ViewModel with Flow state
class PostListViewModel(
    private val repository: PostRepositoryFlow
) : ViewModel() {
    
    private val _posts = MutableStateFlow<List<Post>>(emptyList())
    val posts: StateFlow<List<Post>> = _posts.asStateFlow()
    
    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading.asStateFlow()
    
    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error.asStateFlow()
    
    init {
        loadPosts()
    }
    
    fun loadPosts() {
        viewModelScope.launch {
            _isLoading.value = true
            
            repository.getPosts()
                .catch { e ->
                    _error.value = e.message
                    _isLoading.value = false
                }
                .collect { postList ->
                    _posts.value = postList
                    _isLoading.value = false
                }
        }
    }
    
    fun refresh() {
        loadPosts()
    }
    
    fun clearError() {
        _error.value = null
    }
}
```

## Section 4: Production Example - Complete Flow Network Layer

This example demonstrates a complete production-ready Flow implementation with pagination, error handling, and state management.

```kotlin
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.*

// Data models
data class TodoItem(
    val id: Long = 0,
    val title: String,
    val completed: Boolean = false,
    val userId: Long = 1
)

data class ApiResponse<T>(
    val data: T,
    val success: Boolean,
    val message: String?
)

// Network state wrapper
sealed class NetworkState<out T> {
    object Loading : NetworkState<Nothing>()
    data class Success<T>(val data: T) : NetworkState<T>()
    data class Error(val message: String, val code: Int? = null) : NetworkState<Nothing>()
    object Empty : NetworkState<Nothing>()
}

// API service
interface TodoFlowApiService {
    
    @GET("todos")
    fun getTodos(
        @Query("_page") page: Int = 1,
        @Query("_limit") limit: Int = 20
    ): Flow<List<TodoItem>>
    
    @GET("todos/{id}")
    fun getTodoById(@Path("id") id: Long): Flow<TodoItem>
    
    @POST("todos")
    fun createTodo(@Body todo: TodoItem): Flow<TodoItem>
    
    @PUT("todos/{id}")
    fun updateTodo(@Path("id") id: Long, @Body todo: TodoItem): Flow<TodoItem>
    
    @DELETE("todos/{id}")
    fun deleteTodo(@Path("id") id: Long): Flow<Unit>
    
    @GET("todos")
    fun searchTodos(@Query("q") query: String): Flow<List<TodoItem>>
}

// Retrofit configuration
object TodoFlowClient {
    
    private const val BASE_URL = "https://jsonplaceholder.typicode.com/"
    
    val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }
    
    val apiService: TodoFlowApiService by lazy {
        retrofit.create(TodoFlowApiService::class.java)
    }
}

// Repository with comprehensive Flow
class TodoRepositoryFlow(
    private val apiService: TodoFlowApiService
) {
    
    // Get all todos with loading state
    fun getTodosFlow(page: Int = 1, limit: Int = 20): Flow<NetworkState<List<TodoItem>>> = flow {
        emit(NetworkState.Loading)
        
        try {
            apiService.getTodos(page, limit)
                .catch { e ->
                    emit(NetworkState.Error(e.message ?: "Error", null))
                }
                .collect { todos ->
                    emit(
                        if (todos.isEmpty()) NetworkState.Empty
                        else NetworkState.Success(todos)
                    )
                }
        } catch (e: Exception) {
            emit(NetworkState.Error(e.message ?: "Unknown error", null))
        }
    }.flowOn(Dispatchers.IO)
    
    // Get single todo
    fun getTodoByIdFlow(id: Long): Flow<NetworkState<TodoItem>> = flow {
        emit(NetworkState.Loading)
        
        try {
            apiService.getTodoById(id)
                .catch { e ->
                    emit(NetworkState.Error(e.message ?: "Error", null))
                }
                .collect { todo ->
                    emit(NetworkState.Success(todo))
                }
        } catch (e: Exception) {
            emit(NetworkState.Error(e.message ?: "Unknown error", null))
        }
    }.flowOn(Dispatchers.IO)
    
    // Create todo
    fun createTodoFlow(title: String): Flow<NetworkState<TodoItem>> = flow {
        emit(NetworkState.Loading)
        
        val newTodo = TodoItem(title = title, userId = 1)
        
        try {
            apiService.createTodo(newTodo)
                .catch { e ->
                    emit(NetworkState.Error(e.message ?: "Error", null))
                }
                .collect { todo ->
                    emit(NetworkState.Success(todo))
                }
        } catch (e: Exception) {
            emit(NetworkState.Error(e.message ?: "Unknown error", null))
        }
    }.flowOn(Dispatchers.IO)
    
    // Search todos
    fun searchTodosFlow(query: String): Flow<NetworkState<List<TodoItem>>> = flow {
        emit(NetworkState.Loading)
        
        try {
            apiService.searchTodos(query)
                .catch { e ->
                    emit(NetworkState.Error(e.message ?: "Error", null))
                }
                .collect { todos ->
                    emit(
                        if (todos.isEmpty()) NetworkState.Empty
                        else NetworkState.Success(todos)
                    )
                }
        } catch (e: Exception) {
            emit(NetworkState.Error(e.message ?: "Unknown error", null))
        }
    }.flowOn(Dispatchers.IO)
    
    // Toggle todo completion
    fun toggleTodoFlow(todo: TodoItem): Flow<NetworkState<TodoItem>> = flow {
        emit(NetworkState.Loading)
        
        val updated = todo.copy(completed = !todo.completed)
        
        try {
            apiService.updateTodo(todo.id, updated)
                .catch { e ->
                    emit(NetworkState.Error(e.message ?: "Error", null))
                }
                .collect { updatedTodo ->
                    emit(NetworkState.Success(updatedTodo))
                }
        } catch (e: Exception) {
            emit(NetworkState.Error(e.message ?: "Unknown error", null))
        }
    }.flowOn(Dispatchers.IO)
    
    // Delete todo
    fun deleteTodoFlow(id: Long): Flow<NetworkState<Unit>> = flow {
        emit(NetworkState.Loading)
        
        try {
            apiService.deleteTodo(id)
                .catch { e ->
                    emit(NetworkState.Error(e.message ?: "Error", null))
                }
                .collect {
                    emit(NetworkState.Success(Unit))
                }
        } catch (e: Exception) {
            emit(NetworkState.Error(e.message ?: "Unknown error", null))
        }
    }.flowOn(Dispatchers.IO)
}

// ViewModel with pagination
class TodoViewModel(
    private val repository: TodoRepositoryFlow
) : ViewModel() {
    
    private val _todos = MutableStateFlow<List<TodoItem>>(emptyList())
    val todos: StateFlow<List<TodoItem>> = _todos.asStateFlow()
    
    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading.asStateFlow()
    
    private val _isLoadingMore = MutableStateFlow(false)
    val isLoadingMore: StateFlow<Boolean> = _isLoadingMore.asStateFlow()
    
    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error.asStateFlow()
    
    private var currentPage = 1
    private var hasMore = true
    
    init {
        loadTodos()
    }
    
    fun loadTodos() {
        currentPage = 1
        hasMore = true
        
        viewModelScope.launch {
            repository.getTodosFlow(page = currentPage)
                .collect { state ->
                    when (state) {
                        is NetworkState.Loading -> {
                            _isLoading.value = true
                            _error.value = null
                        }
                        is NetworkState.Success -> {
                            _todos.value = state.data
                            _isLoading.value = false
                            currentPage++
                        }
                        is NetworkState.Error -> {
                            _error.value = state.message
                            _isLoading.value = false
                        }
                        is NetworkState.Empty -> {
                            _todos.value = emptyList()
                            _isLoading.value = false
                            hasMore = false
                        }
                    }
                }
        }
    }
    
    fun loadMore() {
        if (_isLoadingMore.value || !hasMore) return
        
        viewModelScope.launch {
            _isLoadingMore.value = true
            
            repository.getTodosFlow(page = currentPage)
                .collect { state ->
                    when (state) {
                        is NetworkState.Success -> {
                            _todos.value = _todos.value + state.data
                            currentPage++
                            if (state.data.isEmpty()) hasMore = false
                        }
                        is NetworkState.Empty -> {
                            hasMore = false
                        }
                        is NetworkState.Error -> {
                            _error.value = state.message
                        }
                        else -> {}
                    }
                    _isLoadingMore.value = false
                }
        }
    }
    
    fun createTodo(title: String) {
        viewModelScope.launch {
            repository.createTodoFlow(title)
                .collect { state ->
                    when (state) {
                        is NetworkState.Success -> {
                            loadTodos()
                        }
                        is NetworkState.Error -> {
                            _error.value = state.message
                        }
                        else -> {}
                    }
                }
        }
    }
    
    fun toggleTodo(todo: TodoItem) {
        viewModelScope.launch {
            repository.toggleTodoFlow(todo)
                .collect { state ->
                    when (state) {
                        is NetworkState.Success -> {
                            val index = _todos.value.indexOfFirst { it.id == todo.id }
                            if (index != -1) {
                                _todos.value = _todos.value.toMutableList().apply {
                                    set(index, state.data)
                                }
                            }
                        }
                        is NetworkState.Error -> {
                            _error.value = state.message
                        }
                        else -> {}
                    }
                }
        }
    }
    
    fun deleteTodo(id: Long) {
        viewModelScope.launch {
            repository.deleteTodoFlow(id)
                .collect { state ->
                    when (state) {
                        is NetworkState.Success -> {
                            _todos.value = _todos.value.filter { it.id != id }
                        }
                        is NetworkState.Error -> {
                            _error.value = state.message
                        }
                        else -> {}
                    }
                }
        }
    }
    
    fun refresh() {
        loadTodos()
    }
    
    fun clearError() {
        _error.value = null
    }
}
```

## Section 5: Advanced Flow Patterns

Advanced Flow patterns for complex scenarios like debouncing, retrying, and combining streams.

```kotlin
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.delay
import kotlinx.coroutines.timeout

// Search with debounce
class SearchManager {
    
    private val _searchQuery = MutableStateFlow("")
    val searchQuery: StateFlow<String> = _searchQuery.asStateFlow()
    
    private val _searchResults = MutableStateFlow<List<SearchResult>>(emptyList())
    val searchResults: StateFlow<List<SearchResult>> = _searchResults.asStateFlow()
    
    private val _isSearching = MutableStateFlow(false)
    val isSearching: StateFlow<Boolean> = _isSearching.asStateFlow()
    
    private val repository = SearchRepository()
    
    init {
        observeSearch()
    }
    
    fun updateQuery(query: String) {
        _searchQuery.value = query
    }
    
    private fun observeSearch() {
        viewModelScope.launch {
            _searchQuery
                .debounce(300)
                .distinctUntilChanged()
                .filter { it.isNotBlank() }
                .flatMapLatest { query ->
                    _isSearching.value = true
                    repository.search(query)
                        .catch { e ->
                            _searchResults.value = emptyList()
                        }
                }
                .collect { results ->
                    _searchResults.value = results
                    _isSearching.value = false
                }
        }
    }
}

data class SearchResult(val id: Long, val title: String)

class SearchRepository {
    fun search(query: String): Flow<List<SearchResult>> = flow {
        // Simulate API call
        delay(500)
        emit(listOf(
            SearchResult(1, "Result 1 for $query"),
            SearchResult(2, "Result 2 for $query")
        ))
    }.flowOn(Dispatchers.IO)
}

// Retry with exponential backoff
class RetryFlow {
    
    fun <T> Flow<T>.withRetry(
        maxAttempts: Int = 3,
        initialDelay: Long = 1000,
        maxDelay: Long = 10000
    ): Flow<T> = this
        .retry { cause ->
            val delay = minOf(initialDelay * 2.toDouble().pow(retryCount - 1).toLong(), maxDelay)
            delay(delay)
            retryCount < maxAttempts
        }
    
    private val retryCount: Int
        get() = 0
}

// Combine multiple flows
class CombinedFlowManager {
    
    fun getUserProfile(): Flow<UserProfile> {
        val userFlow = getUser()
        val postsFlow = getUserPosts()
        val statsFlow = getUserStats()
        
        return combine(userFlow, postsFlow, statsFlow) { user, posts, stats ->
            UserProfile(user, posts, stats)
        }
    }
    
    private fun getUser(): Flow<User> = flow {
        emit(User("John", "john@example.com"))
    }
    
    private fun getUserPosts(): Flow<List<Post>> = flow {
        emit(listOf(Post("Title 1"), Post("Title 2")))
    }
    
    private fun getUserStats(): Flow<Stats> = flow {
        emit(Stats(100, 50, 25))
    }
}

data class User(val name: String, val email: String)
data class Post(val title: String)
data class Stats(val posts: Int, val followers: Int, val following: Int)
data class UserProfile(val user: User, val posts: List<Post>, val stats: Stats)

// Converting callback to Flow
class CallbackToFlow {
    
    private val callbackFlow = callbackFlow<List<String>> {
        val callback = object : Callback {
            override fun onData(data: List<String>) {
                trySend(data)
            }
            
            override fun onError(error: Exception) {
                close(error)
            }
        }
        
        // Register callback
        // someApi.registerCallback(callback)
        
        awaitClose {
            // Unregister callback
            // someApi.unregisterCallback(callback)
        }
    }
    
    fun dataFlow(): Flow<List<String>> = callbackFlow.flowOn(Dispatchers.IO)
}

interface Callback {
    fun onData(data: List<String>)
    fun onError(error: Exception)
}

// Zip multiple requests
class ZippedFlowManager {
    
    fun getDashboard(): Flow<Dashboard> {
        val userFlow = getUser()
        val postsFlow = getPosts()
        val statsFlow = getStats()
        
        return userFlow.zip(postsFlow) { user, posts ->
            Pair(user, posts)
        }.zip(statsFlow) { (user, posts), stats ->
            Dashboard(user, posts, stats)
        }
    }
    
    private fun getUser(): Flow<User> = flow { emit(User("John", "john@example.com")) }
    private fun getPosts(): Flow<List<Post>> = flow { emit(listOf(Post("Title"))) }
    private fun getStats(): Flow<Stats> = flow { emit(Stats(100, 50, 25)) }
}

data class Dashboard(
    val user: User,
    val posts: List<Post>,
    val stats: Stats
)
```

## Best Practices

- **Use StateFlow for UI State**: StateFlow survives configuration changes
- **FlowOn for Threading**: Use flowOn(Dispatchers.IO) for network/disk operations
- **Catch Errors**: Always handle errors with catch operator
- **Collect in ViewModelScope**: Never collect in lifecycle-unaware scopes
- **Use Loading States**: Show loading states during data fetches
- **Prefer Coroutines**: For new code, prefer Flow over RxJava

## Common Pitfalls

**Problem**: Flow not collecting
**Solution**: Ensure collector is in active coroutine scope

**Problem**: Memory leaks
**Solution**: Cancel collection in onDestroy

**Problem**: Not handling errors
**Solution**: Use catch operator before collect

**Problem**: Thread issues
**Solution**: Use flowOn for CPU-intensive work

## Troubleshooting Guide

**Q: Why is my Flow not emitting?**
A: Check if collector is active and Flow is being collected

**Q: How to convert callback to Flow?**
A: Use callbackFlow builder

**Q: Why isn't retry working?**
A: retry operator must be before terminal operation

## Advanced Tips

- **SharedFlow**: Use for events that should not be replayed
- **channelFlow**: For concurrent data emission
- **Flow timeout**: Add timeout for network calls

## Cross-References

- [Retrofit Basics](../01_HTTP_Communication/01_Retrofit_Basics.md) - Retrofit with Flow
- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md) - Coroutines
- [RxJava Integration](./01_RxJava_Integration.md) - RxJava alternative
- [Background Threading](./05_Background_Threading.md) - Thread management
