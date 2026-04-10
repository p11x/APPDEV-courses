# Retrofit Basics

## Learning Objectives

1. Understanding Retrofit as a type-safe HTTP client
2. Configuring Retrofit with base URL and converters
3. Creating API interfaces with various HTTP methods
4. Implementing synchronous and asynchronous calls
5. Handling request/response with Retrofit

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Coroutines Basics](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)

## Section 1: Introduction to Retrofit

Retrofit is a type-safe HTTP client for Android and Java applications. It turns your HTTP API into a Kotlin interface, making network calls declarative and easy to manage. Retrofit works as a layer over OkHttp, handling request/response serialization automatically through configurable converters.

Key features of Retrofit include:
- Type-safe API interface
- Automatic JSON/XML parsing
- Built-in coroutines support
- Request/response interception
- Custom converter support
- Error handling mechanisms

Retrofit uses annotations to define API endpoints, making the code clean and readable. The library handles the boilerplate of network communication, allowing developers to focus on business logic.

## Section 2: Basic Retrofit Configuration

Setting up Retrofit requires defining a base URL, choosing a converter factory for parsing responses, and creating an API interface. The base URL should end with a forward slash, and the converter depends on your API's response format (JSON, XML, Protocol Buffers).

```kotlin
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query

// Data classes for API response
data class User(
    val id: Int,
    val name: String,
    val email: String,
    val phone: String
)

data class UserResponse(
    val data: List<User>,
    val total: Int,
    val page: Int
)

// API interface defining endpoints
interface UserApiService {
    
    @GET("users")
    suspend fun getUsers(): UserResponse
    
    @GET("users/{id}")
    suspend fun getUserById(@Path("id") userId: Int): User
    
    @GET("users")
    suspend fun getUsersByPage(
        @Query("page") page: Int,
        @Query("limit") limit: Int
    ): UserResponse
}

// Retrofit instance configuration
object RetrofitClient {
    
    private const val BASE_URL = "https://api.example.com/v1/"
    
    // Lazy initialization ensures single instance
    val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            // Gson converter for JSON parsing
            .addConverterFactory(GsonConverterFactory.create())
            // Enable logging for debugging
            .build()
    }
    
    // API service instance
    val userApiService: UserApiService by lazy {
        retrofit.create(UserApiService::class.java)
    }
}

// Usage in a ViewModel or Repository
class UserRepository {
    
    private val apiService = RetrofitClient.userApiService
    
    suspend fun fetchUsers(): Result<UserResponse> {
        return try {
            val response = apiService.getUsers()
            Result.success(response)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
    
    suspend fun fetchUserById(id: Int): Result<User> {
        return try {
            val response = apiService.getUserById(id)
            Result.success(response)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}
```

## Section 3: HTTP Methods and Annotations

Retrofit supports all HTTP methods through annotations. The most commonly used are GET, POST, PUT, PATCH, and DELETE. Each annotation can include a relative URL path, and parameters can be passed through path variables, query parameters, or request body.

```kotlin
import okhttp3.RequestBody
import okhttp3.MultipartBody
import retrofit2.http.*

// Complex API service interface with all HTTP methods
interface ApiService {
    
    // GET request - retrieve data
    @GET("posts")
    suspend fun getPosts(): List<Post>
    
    // GET with query parameters
    @GET("posts")
    suspend fun getPostsByUser(
        @Query("userId") userId: Int,
        @Query("_sort") sortBy: String? = null
    ): List<Post>
    
    // GET with path variables
    @GET("posts/{id}")
    suspend fun getPostById(@Path("id") postId: Int): Post
    
    // POST request - create new resource
    @POST("posts")
    suspend fun createPost(@Body post: Post): Post
    
    // PUT request - complete update
    @PUT("posts/{id}")
    suspend fun updatePost(
        @Path("id") postId: Int,
        @Body post: Post
    ): Post
    
    // PATCH request - partial update
    @PATCH("posts/{id}")
    suspend fun patchPost(
        @Path("id") postId: Int,
        @Body updates: Map<String, Any>
    ): Post
    
    // DELETE request - remove resource
    @DELETE("posts/{id}")
    suspend fun deletePost(@Path("id") postId: Int): Unit
    
    // Multipart POST for file uploads
    @Multipart
    @POST("upload")
    suspend fun uploadFile(
        @Part file: MultipartBody.Part,
        @Part("description") description: String
    ): UploadResponse
    
    // Form URL-encoded POST
    @FormUrlEncoded
    @POST("login")
    suspend fun login(
        @Field("username") username: String,
        @Field("password") password: String
    ): AuthResponse
}

// Data models
data class Post(
    val userId: Int,
    val id: Int? = null,
    val title: String,
    val body: String
)

data class UploadResponse(
    val id: String,
    val url: String,
    val size: Long
)

data class AuthResponse(
    val token: String,
    val expiresIn: Long,
    val user: User
)
```

## Section 4: Production Example - Weather API Client

This example demonstrates a production-ready API client implementation with proper error handling, configuration, and type-safe responses.

```kotlin
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.GET
import retrofit2.http.Query
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.launch
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope

// Weather API response models
data class WeatherResponse(
    val location: Location,
    val current: CurrentWeather,
    val forecast: Forecast?
)

data class Location(
    val name: String,
    val region: String,
    val country: String,
    val lat: Double,
    val lon: Double,
    val localtime: String
)

data class CurrentWeather(
    val temp_c: Double,
    val temp_f: Double,
    val condition: WeatherCondition,
    val wind_kph: Double,
    val humidity: Int,
    val cloud: Int,
    val feelslike_c: Double
)

data class WeatherCondition(
    val text: String,
    val icon: String,
    val code: Int
)

data class Forecast(
    val forecastday: List<ForecastDay>
)

data class ForecastDay(
    val date: String,
    val day: DayWeather,
    val hour: List<HourWeather>
)

data class DayWeather(
    val maxtemp_c: Double,
    val mintemp_c: Double,
    val avgtemp_c: Double,
    val condition: WeatherCondition,
    val chance_of_rain: Int
)

data class HourWeather(
    val time: String,
    val temp_c: Double,
    val condition: WeatherCondition
)

// Weather API service interface
interface WeatherApiService {
    
    @GET("current.json")
    suspend fun getCurrentWeather(
        @Query("key") apiKey: String,
        @Query("q") location: String,
        @Query("aqi") aqi: String = "no"
    ): WeatherResponse
    
    @GET("forecast.json")
    suspend fun getForecast(
        @Query("key") apiKey: String,
        @Query("q") location: String,
        @Query("days") days: Int = 7,
        @Query("aqi") aqi: String = "no",
        @Query("alerts") alerts: String = "no"
    ): WeatherResponse
}

// Retrofit configuration with custom settings
object WeatherRetrofitClient {
    
    private const val BASE_URL = "https://api.weatherapi.com/v1/"
    private const val TIMEOUT_SECONDS = 30
    
    val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            // OkHttp client should be configured separately for production
            // See OkHttp Configuration guide
            .build()
    }
    
    val weatherApiService: WeatherApiService by lazy {
        retrofit.create(WeatherApiService::class.java)
    }
}

// Repository pattern implementation
class WeatherRepository {
    
    private val apiService = WeatherRetrofitClient.weatherApiService
    private val apiKey = "YOUR_API_KEY" // Should be in BuildConfig
    
    suspend fun getCurrentWeather(location: String): Result<WeatherResponse> {
        return withContext(Dispatchers.IO) {
            try {
                val response = apiService.getCurrentWeather(apiKey, location)
                Result.success(response)
            } catch (e: Exception) {
                Result.failure(handleNetworkError(e))
            }
        }
    }
    
    suspend fun getForecast(location: String, days: Int = 7): Result<WeatherResponse> {
        return withContext(Dispatchers.IO) {
            try {
                val response = apiService.getForecast(apiKey, location, days)
                Result.success(response)
            } catch (e: Exception) {
                Result.failure(handleNetworkError(e))
            }
        }
    }
    
    private fun handleNetworkError(e: Exception): Exception {
        return when (e) {
            is retrofit2.HttpException -> {
                when (e.code()) {
                    401 -> ApiException("Invalid API key")
                    403 -> ApiException("API key suspended")
                    404 -> ApiException("Location not found")
                    429 -> ApiException("Too many requests")
                    else -> ApiException("HTTP Error: ${e.code()}")
                }
            }
            is java.net.UnknownHostException -> 
                ApiException("No internet connection")
            is java.net.SocketTimeoutException -> 
                ApiException("Request timed out")
            else -> ApiException(e.message ?: "Unknown error")
        }
    }
}

class ApiException(message: String) : Exception(message)

// ViewModel for UI integration
class WeatherViewModel : ViewModel() {
    
    private val repository = WeatherRepository()
    
    private val _weatherState = MutableStateFlow<WeatherUiState>(WeatherUiState.Loading)
    val weatherState: StateFlow<WeatherUiState> = _weatherState
    
    fun loadWeather(location: String) {
        viewModelScope.launch {
            _weatherState.value = WeatherUiState.Loading
            
            repository.getCurrentWeather(location)
                .onSuccess { weather ->
                    _weatherState.value = WeatherUiState.Success(weather)
                }
                .onFailure { error ->
                    _weatherState.value = WeatherUiState.Error(error.message ?: "Unknown error")
                }
        }
    }
}

sealed class WeatherUiState {
    object Loading : WeatherUiState()
    data class Success(val data: WeatherResponse) : WeatherUiState()
    data class Error(val message: String) : WeatherUiState()
}
```

## Section 5: Production Example - RESTful Todo API Client

This example demonstrates a complete production implementation with CRUD operations, proper error handling, and state management using Kotlin Flow.

```kotlin
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.*
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.catch
import kotlinx.coroutines.flow.flow
import kotlinx.coroutines.Dispatchers

// Todo item model
data class TodoItem(
    val id: Long = 0,
    val title: String,
    val completed: Boolean = false,
    val userId: Long = 1,
    val priority: Priority = Priority.MEDIUM,
    val dueDate: String? = null,
    val tags: List<String> = emptyList()
)

enum class Priority {
    LOW, MEDIUM, HIGH
}

// API response wrapper
data class ApiResponse<T>(
    val success: Boolean,
    val data: T?,
    val message: String?,
    val error: String?
)

// Todo API service with full CRUD operations
interface TodoApiService {
    
    @GET("todos")
    suspend fun getAllTodos(
        @Query("_page") page: Int = 1,
        @Query("_limit") limit: Int = 20,
        @Query("completed") completed: Boolean? = null,
        @Query("userId") userId: Long? = null
    ): List<TodoItem>
    
    @GET("todos/{id}")
    suspend fun getTodoById(@Path("id") id: Long): TodoItem
    
    @POST("todos")
    suspend fun createTodo(@Body todo: TodoItem): TodoItem
    
    @PUT("todos/{id}")
    suspend fun updateTodo(
        @Path("id") id: Long,
        @Body todo: TodoItem
    ): TodoItem
    
    @PATCH("todos/{id}")
    suspend fun patchTodo(
        @Path("id") id: Long,
        @Body updates: Map<String, Any>
    ): TodoItem
    
    @DELETE("todos/{id}")
    suspend fun deleteTodo(@Path("id") id: Long): Unit
    
    @DELETE("todos")
    suspend fun deleteCompletedTodos(@Query("completed") completed: Boolean = true): List<Long>
    
    @GET("todos")
    suspend fun searchTodos(@Query("q") query: String): List<TodoItem>
}

// Retrofit client with enhanced configuration
object TodoRetrofitClient {
    
    private const val BASE_URL = "https://jsonplaceholder.typicode.com/"
    private const val CONNECT_TIMEOUT = 15L
    private const val READ_TIMEOUT = 30L
    private const val WRITE_TIMEOUT = 30L
    
    val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            // For production, use OkHttp client with interceptors
            // See OkHttp Configuration and Interceptor Patterns guides
            .build()
    }
    
    val todoApiService: TodoApiService by lazy {
        retrofit.create(TodoApiService::class.java)
    }
}

// Repository with Flow-based reactive streams
class TodoRepository {
    
    private val apiService = TodoRetrofitClient.todoApiService
    
    // Flow-based approach for reactive data
    fun getTodosFlow(page: Int = 1, limit: Int = 20): Flow<Result<List<TodoItem>>> = flow {
        try {
            val todos = apiService.getAllTodos(page, limit)
            emit(Result.success(todos))
        } catch (e: Exception) {
            emit(Result.failure(e))
        }
    }.catch { e ->
        emit(Result.failure(e))
    }.flowOn(Dispatchers.IO)
    
    suspend fun getAllTodos(page: Int = 1, limit: Int = 20): Result<List<TodoItem>> {
        return try {
            val todos = apiService.getAllTodos(page, limit)
            Result.success(todos)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    suspend fun getTodoById(id: Long): Result<TodoItem> {
        return try {
            val todo = apiService.getTodoById(id)
            Result.success(todo)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    suspend fun createTodo(title: String, priority: Priority = Priority.MEDIUM): Result<TodoItem> {
        return try {
            val newTodo = TodoItem(
                title = title,
                priority = priority,
                userId = 1
            )
            val created = apiService.createTodo(newTodo)
            Result.success(created)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    suspend fun updateTodo(todo: TodoItem): Result<TodoItem> {
        return try {
            val updated = apiService.updateTodo(todo.id, todo)
            Result.success(updated)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    suspend fun toggleTodoCompletion(id: Long, completed: Boolean): Result<TodoItem> {
        return try {
            val updates = mapOf("completed" to completed)
            val patched = apiService.patchTodo(id, updates)
            Result.success(patched)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    suspend fun deleteTodo(id: Long): Result<Unit> {
        return try {
            apiService.deleteTodo(id)
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    suspend fun searchTodos(query: String): Result<List<TodoItem>> {
        return try {
            val results = apiService.searchTodos(query)
            Result.success(results)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    private fun handleError(e: Exception): Exception {
        return when (e) {
            is retrofit2.HttpException -> {
                val code = e.code()
                val message = when (code) {
                    400 -> "Bad request - invalid data"
                    401 -> "Unauthorized - please login"
                    403 -> "Forbidden - access denied"
                    404 -> "Resource not found"
                    500 -> "Server error - try again later"
                    else -> "HTTP error: $code"
                }
                ApiException(message, code)
            }
            is java.net.UnknownHostException -> 
                NetworkException("No internet connection")
            is java.net.SocketTimeoutException -> 
                NetworkException("Connection timed out")
            else -> ApiException(e.message ?: "Unknown error", null)
        }
    }
}

class ApiException(message: String, val code: Int?) : Exception(message)
class NetworkException(message: String) : Exception(message)

// Usage example in a use case
class GetTodosUseCase {
    
    private val repository = TodoRepository()
    
    operator fun invoke(page: Int = 1, limit: Int = 20): Flow<Result<List<TodoItem>>> {
        return repository.getTodosFlow(page, limit)
    }
}

// Usage in ViewModel with StateFlow
class TodoListViewModel(
    private val getTodosUseCase: GetTodosUseCase = GetTodosUseCase()
) : ViewModel() {
    
    private val _todos = MutableStateFlow<List<TodoItem>>(emptyList())
    val todos: StateFlow<List<TodoItem>> = _todos
    
    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading
    
    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error
    
    fun loadTodos() {
        viewModelScope.launch {
            _isLoading.value = true
            _error.value = null
            
            getTodosUseCase()
                .catch { e ->
                    _error.value = e.message
                }
                .collect { result ->
                    result.onSuccess { todoList ->
                        _todos.value = todoList
                    }.onFailure { e ->
                        _error.value = e.message
                    }
                    _isLoading.value = false
                }
        }
    }
}
```

## Best Practices

- **Use Coroutines**: Always use suspend functions with coroutines for network calls instead of callbacks
- **Define Base URL Correctly**: Ensure the base URL ends with a forward slash (/) for proper path concatenation
- **Use Data Classes**: Define response models as Kotlin data classes for automatic equals(), hashCode(), and toString() methods
- **Implement Error Handling**: Wrap network calls in Result or use sealed classes for proper error handling
- **Avoid Hardcoding URLs**: Use BuildConfig or configuration files for API keys and base URLs
- **Use Single Retrofit Instance**: Create a singleton Retrofit instance to avoid resource waste
- **Add Converter Factory**: Always add an appropriate converter factory (Gson, Moshi, Jackson) for response parsing
- **Use Repository Pattern**: Implement repositories to abstract network operations from ViewModels
- **Configure Timeouts**: Set appropriate connect, read, and write timeouts in OkHttp client
- **Enable Logging in Debug**: Use HttpLoggingInterceptor for debugging but disable in production

## Common Pitfalls

**Problem**: Retrofit throws IllegalArgumentException for base URL
**Solution**: Ensure base URL ends with "/" and all @GET paths start without "/"

**Problem**: Network calls cause NetworkOnMainThreadException
**Solution**: Always execute Retrofit suspend functions with Dispatchers.IO or using viewModelScope

**Problem**: Gson fails to parse response with custom field names
**Solution**: Use @SerializedName annotation or configure Gson with FieldNamingPolicy

**Problem**: NullPointerException when response field is missing
**Solution**: Make data class fields nullable or use @SerializedName with default values

**Problem**: Too many connections or memory leaks
**Solution**: Use singleton pattern for Retrofit instance and proper lifecycle management

## Troubleshooting Guide

**Q: Why is my API call not being made?**
A: Verify that you are calling the suspend function in a coroutine context. Check if the base URL is correct and accessible.

**Q: How do I handle different response formats?**
A: Add appropriate converter factories - GsonConverterFactory for JSON, SimpleXmlConverterFactory for XML, or ProtoConverterFactory for Protocol Buffers.

**Q: Why am I getting 404 errors for existing endpoints?**
A: Check that your base URL is correct and that endpoint paths in annotations don't start with "/".

**Q: How to handle authentication tokens?**
A: Use interceptors to add authorization headers. See the Authentication Implementation guide.

**Q: Why does Retrofit not convert my response?**
A: Ensure you've added a converter factory and that your data classes match the JSON structure.

## Advanced Tips

- **Custom Converters**: Implement Converter.Factory for custom serialization/deserialization
- **Response Caching**: Configure OkHttp with CacheControl for offline support
- **Mock Server**: Use MockWebServer for testing Retrofit without network calls
- **Scalars Converter**: Use ScalarsConverterFactory for primitive type responses
- **Dynamic URLs**: Use @URL annotation for endpoints that vary at runtime
- **Deferred Requests**: Use Retrofit's enqueue() for callback-based asynchronous calls
- **Call Adapters**: Implement custom CallAdapter for different return types (LiveData, Flow, RxJava)

## Cross-References

- [OkHttp Configuration](./02_OkHttp_Configuration.md) - Configuring HTTP client timeouts and cache
- [Interceptor Patterns](./03_Interceptor_Patterns.md) - Adding authentication and logging interceptors
- [Authentication Implementation](./04_Authentication_Implementation.md) - Token-based authentication
- [Error Handling Strategies](./05_Error_Handling_Strategies) - Comprehensive error handling
- [Coroutines Basics](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md) - Async programming fundamentals
- [Flow Implementation](../02_Asynchronous_Patterns/02_Flow_Implementation.md) - Reactive streams with Retrofit
