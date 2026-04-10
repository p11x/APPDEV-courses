# Error Handling Strategies

## Learning Objectives

1. Understanding HTTP error codes and their meanings
2. Implementing comprehensive error handling for network requests
3. Creating custom exceptions for different error types
4. Building error handling patterns with Result and sealed classes
5. Implementing retry logic and fallback mechanisms

## Prerequisites

- [Retrofit Basics](./01_Retrofit_Basics.md)
- [OkHttp Configuration](./02_OkHttp_Configuration.md)
- [Interceptor Patterns](./03_Interceptor_Patterns.md)
- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)

## Section 1: HTTP Error Code Understanding

Understanding HTTP error codes is fundamental to implementing proper error handling. Different error codes require different handling strategies.

```kotlin
// HTTP error code categories
object HttpErrorCodes {
    
    // 1xx: Informational
    // 100 Continue, 101 Switching Protocols
    
    // 2xx: Success
    const val OK = 200
    const val CREATED = 201
    const val ACCEPTED = 202
    const val NO_CONTENT = 204
    
    // 3xx: Redirection
    const val MOVED_PERMANENTLY = 301
    const val FOUND = 302
    const val NOT_MODIFIED = 304
    
    // 4xx: Client Errors
    const val BAD_REQUEST = 400
    const val UNAUTHORIZED = 401
    const val FORBIDDEN = 403
    const val NOT_FOUND = 404
    const val METHOD_NOT_ALLOWED = 405
    const val CONFLICT = 409
    const val TOO_MANY_REQUESTS = 429
    
    // 5xx: Server Errors
    const val INTERNAL_SERVER_ERROR = 500
    const val NOT_IMPLEMENTED = 501
    const val BAD_GATEWAY = 502
    const val SERVICE_UNAVAILABLE = 503
    const val GATEWAY_TIMEOUT = 504
}

// Extension function to check success
fun Int.isSuccess(): Boolean = this in 200..299

fun Int.isClientError(): Boolean = this in 400..499

fun Int.isServerError(): Boolean = this in 500..599

fun Int.isRedirect(): Boolean = this in 300..399
```

## Section 2: Custom Exception Architecture

Creating a comprehensive exception hierarchy helps differentiate between different types of network errors.

```kotlin
import okhttp3.Response
import okhttp3.HttpUrl
import retrofit2.HttpException
import java.io.IOException
import java.net.UnknownHostException
import java.net.SocketTimeoutException

// Base network exception
sealed class NetworkException(
    message: String,
    val code: Int? = null,
    val url: HttpUrl? = null
) : Exception(message) {
    
    // No internet connection
    class NoConnectionException(
        message: String = "No internet connection"
    ) : NetworkException(message)
    
    // Request timeout
    class TimeoutException(
        message: String = "Request timed out"
    ) : NetworkException(message)
    
    // Network I/O error
    class IoException(
        message: String = "Network error",
        cause: IOException? = null
    ) : NetworkException(message, cause = cause)
    
    // HTTP error with specific code
    class HttpError(
        message: String,
        code: Int,
        url: HttpUrl? = null,
        val response: Response? = null
    ) : NetworkException(message, code, url)
    
    // Server error (5xx)
    class ServerError(
        message: String,
        code: Int,
        url: HttpUrl? = null
    ) : NetworkException(message, code, url)
    
    // Client error (4xx)
    class ClientError(
        message: String,
        code: Int,
        url: HttpUrl? = null
    ) : NetworkException(message, code, url)
    
    // Unknown error
    class UnknownException(
        message: String = "Unknown error occurred",
        cause: Throwable? = null
    ) : NetworkException(message, cause = cause)
}

// Convert Retrofit/OkHttp exceptions to custom exceptions
object ExceptionConverter {
    
    fun convert(exception: Exception): NetworkException {
        return when (exception) {
            is HttpException -> convertHttpException(exception)
            is SocketTimeoutException -> NetworkException.TimeoutException()
            is UnknownHostException -> NetworkException.NoConnectionException()
            is IOException -> NetworkException.IoException(exception.message, exception)
            else -> NetworkException.UnknownException(exception.message, exception)
        }
    }
    
    private fun convertHttpException(exception: HttpException): NetworkException {
        val code = exception.code()
        val response = exception.response()
        val url = response?.raw()?.request?.url
        
        return when (code) {
            in 500..599 -> NetworkException.ServerError(
                message = "Server error: $code",
                code = code,
                url = url
            )
            401 -> NetworkException.HttpError(
                message = "Unauthorized - please login",
                code = code,
                url = url,
                response = response?.raw()
            )
            403 -> NetworkException.HttpError(
                message = "Forbidden - access denied",
                code = code,
                url = url,
                response = response?.raw()
            )
            404 -> NetworkException.HttpError(
                message = "Resource not found",
                code = code,
                url = url,
                response = response?.raw()
            )
            429 -> NetworkException.HttpError(
                message = "Too many requests - rate limited",
                code = code,
                url = url,
                response = response?.raw()
            )
            else -> NetworkException.HttpError(
                message = "HTTP error: $code",
                code = code,
                url = url,
                response = response?.raw()
            )
        }
    }
}
```

## Section 3: Result-Based Error Handling

Kotlin's Result type provides a functional approach to handling success and failure cases.

```kotlin
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

// Extension functions for Result
inline fun <T> Result<T>.onNetworkError(action: (NetworkException) -> Unit): Result<T> {
    exceptionOrNull()?.let { exception ->
        if (exception is NetworkException) {
            action(exception)
        } else if (exception is retrofit2.HttpException) {
            action(ExceptionConverter.convert(exception))
        }
    }
    return this
}

inline fun <T> Result<T>.onHttpError(action: (Int, String) -> Unit): Result<T> {
    exceptionOrNull()?.let { exception ->
        if (exception is NetworkException.HttpError) {
            action(exception.code ?: 0, exception.message ?: "")
        }
    }
    return this
}

// Generic API wrapper with Result
class ApiResult<out T> {
    val data: T?
    val error: NetworkException?
    val isSuccess: Boolean
    
    private constructor(data: T?, error: NetworkException?) {
        this.data = data
        this.error = error
        this.isSuccess = error == null && data != null
    }
    
    companion object {
        fun <T> success(data: T): ApiResult<T> = ApiResult(data, null)
        fun <T> failure(exception: NetworkException): ApiResult<T> = ApiResult(null, exception)
    }
    
    inline fun <R> map(transform: (T) -> R): ApiResult<R> {
        return when (isSuccess) {
            true -> ApiResult(data?.let { transform(it) }, null)
            false -> ApiResult(null, error)
        }
    }
    
    inline fun onSuccess(action: (T) -> Unit): ApiResult<T> {
        if (isSuccess) data?.let { action(it) }
        return this
    }
    
    inline fun onFailure(action: (NetworkException) -> Unit): ApiResult<T> {
        error?.let { action(it) }
        return this
    }
}

// Repository with Result-based error handling
class UserRepository(
    private val apiService: UserApiService
) {
    suspend fun getUsers(): ApiResult<List<User>> = withContext(Dispatchers.IO) {
        try {
            val users = apiService.getUsers()
            ApiResult.success(users)
        } catch (e: Exception) {
            val networkException = ExceptionConverter.convert(e)
            ApiResult.failure(networkException)
        }
    }
    
    suspend fun getUserById(id: Int): ApiResult<User> = withContext(Dispatchers.IO) {
        try {
            val user = apiService.getUserById(id)
            ApiResult.success(user)
        } catch (e: Exception) {
            val networkException = ExceptionConverter.convert(e)
            ApiResult.failure(networkException)
        }
    }
    
    suspend fun createUser(user: User): ApiResult<User> = withContext(Dispatchers.IO) {
        try {
            val created = apiService.createUser(user)
            ApiResult.success(created)
        } catch (e: Exception) {
            val networkException = ExceptionConverter.convert(e)
            ApiResult.failure(networkException)
        }
    }
}
```

## Section 4: Sealed Class-Based State Management

Using sealed classes for UI state provides exhaustive handling of all possible states.

```kotlin
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

// Sealed class for UI state
sealed class UiState<out T> {
    object Loading : UiState<Nothing>()
    data class Success<T>(val data: T) : UiState<T>()
    data class Error(val message: String, val retryAction: (() -> Unit)? = null) : UiState<Nothing>()
    object Empty : UiState<Nothing>()
}

// Wrapper for API errors
sealed class ApiError(
    val message: String,
    val code: Int? = null
) {
    class NetworkError(message: String) : ApiError(message)
    class ServerError(message: String, code: Int) : ApiError(message, code)
    class NotFound(message: String = "Resource not found") : ApiError(message, 404)
    class Unauthorized(message: String = "Please login") : ApiError(message, 401)
    class Forbidden(message: String = "Access denied") : ApiError(message, 403)
    class RateLimited(message: String = "Too many requests") : ApiError(message, 429)
    class Unknown(message: String = "Something went wrong") : ApiError(message)
}

// Convert exception to ApiError
object ErrorMapper {
    
    fun map(exception: Exception): ApiError {
        return when (exception) {
            is NetworkException.NoConnectionException -> 
                ApiError.NetworkError("No internet connection")
            is NetworkException.TimeoutException -> 
                ApiError.NetworkError("Request timed out")
            is NetworkException.ServerError -> 
                ApiError.ServerError(exception.message ?: "Server error", exception.code ?: 500)
            is NetworkException.HttpError -> when (exception.code) {
                401 -> ApiError.Unauthorized()
                403 -> ApiError.Forbidden()
                404 -> ApiError.NotFound()
                429 -> ApiError.RateLimited()
                else -> ApiError.Unknown(exception.message ?: "HTTP error")
            }
            else -> ApiError.Unknown(exception.message ?: "Unknown error")
        }
    }
    
    fun map(httpException: retrofit2.HttpException): ApiError {
        return when (httpException.code()) {
            401 -> ApiError.Unauthorized()
            403 -> ApiError.Forbidden()
            404 -> ApiError.NotFound()
            429 -> ApiError.RateLimited()
            in 500..599 -> ApiError.ServerError("Server error", httpException.code())
            else -> ApiError.Unknown(httpException.message() ?: "HTTP error")
        }
    }
}

// ViewModel with proper state management
class UserListViewModel(
    private val repository: UserRepository
) : ViewModel() {
    
    private val _uiState = MutableStateFlow<UiState<List<User>>>(UiState.Loading)
    val uiState: StateFlow<UiState<List<User>>> = _uiState.asStateFlow()
    
    private val _errorMessage = MutableStateFlow<String?>(null)
    val errorMessage: StateFlow<String?> = _errorMessage.asStateFlow()
    
    init {
        loadUsers()
    }
    
    fun loadUsers() {
        viewModelScope.launch {
            _uiState.value = UiState.Loading
            
            repository.getUsers()
                .onSuccess { users ->
                    _uiState.value = if (users.isEmpty()) {
                        UiState.Empty
                    } else {
                        UiState.Success(users)
                    }
                }
                .onFailure { exception ->
                    val error = ErrorMapper.map(exception)
                    _errorMessage.value = error.message
                    _uiState.value = UiState.Error(
                        message = error.message,
                        retryAction = { loadUsers() }
                    )
                }
        }
    }
    
    fun clearError() {
        _errorMessage.value = null
    }
}
```

## Section 5: Production Example - Complete Error Handling

This example demonstrates a complete error handling system with retry logic, fallback mechanisms, and user-friendly error messages.

```kotlin
import okhttp3.Interceptor
import okhttp3.Response
import okhttp3.Request
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import kotlinx.coroutines.delay
import kotlinx.coroutines.retry
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.launch
import androidx.lifecycle.ViewModel
import java.util.concurrent.TimeUnit

// Error response from API
data class ErrorResponse(
    val error: String,
    val message: String,
    val code: Int? = null,
    val details: Map<String, String>? = null
)

// Retry configuration
data class RetryConfig(
    val maxAttempts: Int = 3,
    val initialDelayMs: Long = 1000,
    val maxDelayMs: Long = 10000,
    val exponentialBackoff: Boolean = true,
    val retryableCodes: Set<Int> = setOf(429, 500, 502, 503, 504)
)

// Retry interceptor
class RetryInterceptor(private val config: RetryConfig) : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        var lastException: Exception? = null
        var response: Response? = null
        
        for (attempt in 0 until config.maxAttempts) {
            try {
                response?.close()
                response = chain.proceed(request)
                
                // Success or non-retryable error
                if (response.isSuccessful || !config.retryableCodes.contains(response.code)) {
                    return response
                }
                
                // Retryable error
                response.close()
                
                val delay = calculateDelay(attempt)
                if (attempt < config.maxAttempts - 1) {
                    Thread.sleep(delay)
                    continue
                }
                
            } catch (e: Exception) {
                lastException = e
                
                // Don't retry on non-network exceptions
                if (e !is java.io.IOException) {
                    throw e
                }
                
                val delay = calculateDelay(attempt)
                if (attempt < config.maxAttempts - 1) {
                    Thread.sleep(delay)
                }
            }
        }
        
        // Return last response or throw exception
        return response ?: throw (lastException ?: Exception("Request failed"))
    }
    
    private fun calculateDelay(attempt: Int): Long {
        return if (config.exponentialBackoff) {
            val delay = config.initialDelayMs * (1 shl attempt)
            minOf(delay, config.maxDelayMs)
        } else {
            config.initialDelayMs
        }
    }
}

// Complete network client with error handling
object NetworkClient {
    
    private const val BASE_URL = "https://api.example.com/v1/"
    
    val okHttpClient: OkHttpClient by lazy {
        OkHttpClient.Builder()
            .addInterceptor(RetryInterceptor(RetryConfig()))
            .addInterceptor(ErrorInterceptor())
            .connectTimeout(15, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .writeTimeout(30, TimeUnit.SECONDS)
            .build()
    }
    
    val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .client(okHttpClient)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }
}

// Error handling interceptor
class ErrorInterceptor : Interceptor {
    
    override fun intercept(chain: Interceptor.Chain): Response {
        val request = chain.request()
        val response = chain.proceed(request)
        
        if (!response.isSuccessful) {
            // Handle error response body
            val errorBody = response.body?.string()
            
            // Log error for debugging
            println("API Error: ${response.code} - $errorBody")
            
            // Check for specific error codes
            when (response.code) {
                401 -> {
                    // Handle unauthorized - could trigger logout
                }
                403 -> {
                    // Handle forbidden
                }
                429 -> {
                    // Handle rate limiting - get retry-after header
                    val retryAfter = response.header("Retry-After")
                }
                in 500..599 -> {
                    // Handle server errors
                }
            }
        }
        
        return response
    }
}

// Repository with comprehensive error handling
class PostRepository(
    private val apiService: PostApiService
) {
    private val retryConfig = RetryConfig()
    
    suspend fun getPosts(): Result<List<Post>> = withContext(Dispatchers.IO) {
        try {
            val posts = retry(times = retryConfig.maxAttempts) {
                apiService.getPosts()
            }
            Result.success(posts)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    suspend fun getPostById(id: Long): Result<Post> = withContext(Dispatchers.IO) {
        try {
            val post = apiService.getPostById(id)
            Result.success(post)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    suspend fun createPost(post: Post): Result<Post> = withContext(Dispatchers.IO) {
        try {
            val created = apiService.createPost(post)
            Result.success(created)
        } catch (e: Exception) {
            Result.failure(handleError(e))
        }
    }
    
    private fun handleError(e: Exception): NetworkException {
        return when (e) {
            is retrofit2.HttpException -> {
                val code = e.code()
                when (code) {
                    401 -> NetworkException.HttpError("Unauthorized", code)
                    403 -> NetworkException.HttpError("Forbidden", code)
                    404 -> NetworkException.HttpError("Not found", code)
                    429 -> NetworkException.HttpError("Rate limited", code)
                    in 500..599 -> NetworkException.ServerError("Server error", code)
                    else -> NetworkException.HttpError(e.message() ?: "Error", code)
                }
            }
            is java.net.UnknownHostException -> 
                NetworkException.NoConnectionException()
            is java.net.SocketTimeoutException -> 
                NetworkException.TimeoutException()
            is java.io.IOException -> 
                NetworkException.IoException(e.message ?: "Network error", e)
            else -> NetworkException.UnknownException(e.message, e)
        }
    }
}

// ViewModel with state management
class PostListViewModel(
    private val repository: PostRepository
) : ViewModel() {
    
    private val _posts = MutableStateFlow<List<Post>>(emptyList())
    val posts: StateFlow<List<Post>> = _posts.asStateFlow()
    
    private val _isLoading = MutableStateFlow(false)
    val isLoading: StateFlow<Boolean> = _isLoading.asStateFlow()
    
    private val _error = MutableStateFlow<UiState.Error?>(null)
    val error: StateFlow<UiState.Error?> = _error.asStateFlow()
    
    fun loadPosts() {
        viewModelScope.launch {
            _isLoading.value = true
            
            repository.getPosts()
                .onSuccess { postList ->
                    _posts.value = postList
                    _error.value = null
                }
                .onFailure { exception ->
                    _error.value = when (exception) {
                        is NetworkException.NoConnectionException -> 
                            UiState.Error("No internet connection", retryAction = { loadPosts() })
                        is NetworkException.TimeoutException -> 
                            UiState.Error("Request timed out", retryAction = { loadPosts() })
                        is NetworkException.ServerError -> 
                            UiState.Error("Server error. Please try again later", retryAction = { loadPosts() })
                        else -> 
                            UiState.Error(exception.message ?: "Something went wrong", retryAction = { loadPosts() })
                    }
                }
            
            _isLoading.value = false
        }
    }
    
    fun retry() {
        loadPosts()
    }
}
```

## Section 6: User-Friendly Error Display

Presenting errors to users in a clear and actionable way improves the user experience.

```kotlin
import androidx.compose.foundation.layout.*
import androidx.compose.material.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp

// Error types for UI
enum class ErrorType {
    NETWORK,
    SERVER,
    AUTH,
    VALIDATION,
    UNKNOWN
}

// User-friendly error messages
object ErrorMessages {
    
    fun getMessage(error: ApiError): String {
        return when (error) {
            is ApiError.NetworkError -> "Please check your internet connection and try again."
            is ApiError.ServerError -> "We're having trouble connecting to our servers. Please try again later."
            is ApiError.NotFound -> "The content you're looking for is no longer available."
            is ApiError.Unauthorized -> "Please log in to continue."
            is ApiError.Forbidden -> "You don't have permission to access this content."
            is ApiError.RateLimited -> "Please wait a moment before trying again."
            is ApiError.Unknown -> "Something unexpected happened. Please try again."
        }
    }
    
    fun getErrorType(error: ApiError): ErrorType {
        return when (error) {
            is ApiError.NetworkError -> ErrorType.NETWORK
            is ApiError.ServerError -> ErrorType.SERVER
            is ApiError.Unauthorized, is ApiError.Forbidden -> ErrorType.AUTH
            is ApiError.NotFound -> ErrorType.VALIDATION
            else -> ErrorType.UNKNOWN
        }
    }
}

// Error dialog composable
@Composable
fun ErrorDialog(
    error: UiState.Error,
    onDismiss: () -> Unit,
    onRetry: (() -> Unit)? = error.retryAction
) {
    AlertDialog(
        onDismissRequest = onDismiss,
        title = { Text("Error") },
        text = { Text(error.message) },
        confirmButton = {
            if (onRetry != null) {
                TextButton(onClick = onRetry) {
                    Text("Retry")
                }
            }
        },
        dismissButton = {
            TextButton(onClick = onDismiss) {
                Text("OK")
            }
        }
    )
}

// Error content with retry
@Composable
fun ErrorContent(
    message: String,
    onRetry: () -> Unit
) {
    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center
    ) {
        Text(
            text = message,
            style = MaterialTheme.typography.body1
        )
        
        Spacer(modifier = Modifier.height(16.dp))
        
        Button(onClick = onRetry) {
            Text("Retry")
        }
    }
}

// Snackbar for errors
@Composable
fun ErrorSnackbar(
    errorMessage: String?,
    onDismiss: () -> Unit,
    onRetry: (() -> Unit)? = null
) {
    val snackbarHostState = remember { SnackbarHostState() }
    
    LaunchedEffect(errorMessage) {
        errorMessage?.let {
            val result = snackbarHostState.showSnackbar(
                message = it,
                actionLabel = if (onRetry != null) "Retry" else null,
                duration = SnackbarDuration.Short
            )
            
            if (result == SnackbarResult.ActionPerformed && onRetry != null) {
                onRetry()
            }
            
            onDismiss()
        }
    }
    
    SnackbarHost(hostState = snackbarHostState)
}
```

## Best Practices

- **Use Result Type**: Wrap API calls in Result for clean error handling
- **Create Custom Exceptions**: Differentiate between network, server, and client errors
- **Implement Retry Logic**: Use exponential backoff for transient failures
- **Log Errors**: Log errors for debugging but sanitize sensitive data
- **Handle All States**: Use sealed classes for exhaustive state handling
- **User-Friendly Messages**: Map technical errors to user-friendly messages
- **Provide Retry Actions**: Give users actionable options when errors occur
- **Separate Error Handling**: Keep error handling logic separate from business logic
- **Test Error Paths**: Test both success and error paths in your code

## Common Pitfalls

**Problem**: Catching all exceptions indiscriminately
**Solution**: Catch specific exception types and handle each appropriately

**Problem**: Not handling 401 properly
**Solution**: Implement proper auth refresh or logout flow

**Problem**: Silent failures
**Solution**: Always show meaningful feedback to users

**Problem**: Memory leaks from error handling
**Solution**: Ensure coroutines are properly scoped and cancelled

## Troubleshooting Guide

**Q: Why is my error handling not working?**
A: Check that you're catching the right exception types and not swallowing them

**Q: How do I handle offline mode?**
A: Check network availability before making requests and provide offline UI

**Q: Why do I get 429 errors?**
A: Implement rate limiting in your client and respect Retry-After header

## Advanced Tips

- **Circuit Breaker**: Implement circuit breaker pattern for cascading failures
- **Error Tracking**: Integrate error tracking services (Crashlytics, Sentry)
- **Error Analytics**: Track error patterns to improve API

## Cross-References

- [Retrofit Basics](./01_Retrofit_Basics.md) - API calls
- [Interceptor Patterns](./03_Interceptor_Patterns.md) - Error interceptors
- [Authentication Implementation](./04_Authentication_Implementation.md) - Auth errors
- [Flow Implementation](../02_Asynchronous_Patterns/02_Flow_Implementation.md) - Reactive error handling
