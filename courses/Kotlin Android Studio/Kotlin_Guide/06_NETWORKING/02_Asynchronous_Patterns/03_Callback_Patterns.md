# Callback Patterns

## Learning Objectives

1. Understanding traditional callback patterns in Android
2. Converting callbacks to coroutines
3. Implementing callback-based API clients
4. Handling callback errors and lifecycle
5. Creating reusable callback wrappers

## Prerequisites

- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)
- [Retrofit Basics](../01_HTTP_Communication/01_Retrofit_Basics.md)
- [Flow Implementation](./02_Flow_Implementation.md)

## Section 1: Traditional Callback Patterns

Callbacks are the traditional way to handle asynchronous operations in Android. Understanding them helps when working with legacy code or certain Android APIs.

Key callback concepts:
- Interface-based callbacks
- Success/failure patterns
- Listener registration
- Callback cleanup

```kotlin
import android.os.Handler
import android.os.Looper

// Simple callback interface
interface Callback<T> {
    fun onSuccess(result: T)
    fun onError(error: Exception)
}

// Generic callback with loading
interface LoadingCallback<T> {
    fun onLoading()
    fun onSuccess(result: T)
    fun onError(error: Exception)
}

// Result wrapper
sealed class Result<out T> {
    data class Success<T>(val data: T) : Result<T>()
    data class Error(val exception: Exception) : Result<Nothing>()
    object Loading : Result<Nothing>()
}

// Simple async task with callbacks
class AsyncTaskWithCallbacks {
    
    private val handler = Handler(Looper.getMainLooper())
    
    fun executeAsync(
        task: () -> String,
        onSuccess: (String) -> Unit,
        onError: (Exception) -> Unit
    ) {
        Thread {
            try {
                val result = task()
                handler.post { onSuccess(result) }
            } catch (e: Exception) {
                handler.post { onError(e) }
            }
        }.start()
    }
    
    // Usage
    fun loadData() {
        executeAsync(
            task = { fetchDataFromNetwork() },
            onSuccess = { data -> 
                println("Data loaded: $data")
            },
            onError = { error -> 
                println("Error: ${error.message}")
            }
        )
    }
    
    private fun fetchDataFromNetwork(): String {
        Thread.sleep(1000)
        return "Data from network"
    }
}

// Callback interface for network operations
interface NetworkCallback<T> {
    fun onSuccess(data: T)
    fun onFailure(error: NetworkError)
}

sealed class NetworkError(
    val message: String,
    val code: Int? = null
) {
    class NoConnection : NetworkError("No internet connection")
    class Timeout : NetworkError("Request timed out")
    class ServerError(val statusCode: Int, msg: String) : NetworkError(msg, statusCode)
    class NotFound(msg: String = "Resource not found") : NetworkError(msg, 404)
    class Unauthorized(msg: String = "Unauthorized") : NetworkError(msg, 401)
    class Unknown(msg: String = "Unknown error") : NetworkError(msg)
}
```

## Section 2: Callback-Based API Client

Creating a callback-based API client for making network requests.

```kotlin
import okhttp3.Call
import okhttp3.Callback
import okhttp3.Request
import okhttp3.Response
import okhttp3.ResponseBody
import java.io.IOException
import java.util.concurrent.TimeUnit

// Callback-based network client
class CallbackNetworkClient {
    
    private val client = okhttp3.OkHttpClient.Builder()
        .connectTimeout(30, TimeUnit.SECONDS)
        .readTimeout(30, TimeUnit.SECONDS)
        .writeTimeout(30, TimeUnit.SECONDS)
        .build()
    
    // Generic GET request
    fun get(
        url: String,
        callback: Callback<Response>
    ) {
        val request = Request.Builder()
            .url(url)
            .get()
            .build()
        
        client.newCall(request).enqueue(callback)
    }
    
    // Generic POST request
    fun post(
        url: String,
        body: okhttp3.RequestBody,
        callback: Callback<Response>
    ) {
        val request = Request.Builder()
            .url(url)
            .post(body)
            .build()
        
        client.newCall(request).enqueue(callback)
    }
    
    // Generic request with headers
    fun request(
        request: Request,
        callback: Callback<Response>
    ) {
        client.newCall(request).enqueue(callback)
    }
}

// API client implementation
class ApiClient(
    private val baseUrl: String,
    private val networkClient: CallbackNetworkClient
) {
    
    private val gson = com.google.gson.Gson()
    
    fun fetchUser(
        userId: Int,
        callback: NetworkCallback<User>
    ) {
        val url = "$baseUrl/users/$userId"
        
        networkClient.get(url, object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                callback.onFailure(NetworkError.Unknown(e.message ?: "Network error"))
            }
            
            override fun onResponse(call: Call, response: Response) {
                if (!response.isSuccessful) {
                    callback.onFailure(
                        NetworkError.ServerError(response.code, "Server error: ${response.code}")
                    )
                    return
                }
                
                try {
                    val body = response.body?.string()
                    val user = gson.fromJson(body, User::class.java)
                    callback.onSuccess(user)
                } catch (e: Exception) {
                    callback.onFailure(NetworkError.Unknown("Failed to parse response"))
                }
            }
        })
    }
    
    fun fetchPosts(
        callback: NetworkCallback<List<Post>>
    ) {
        val url = "$baseUrl/posts"
        
        networkClient.get(url, object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                callback.onFailure(NetworkError.Unknown(e.message ?: "Network error"))
            }
            
            override fun onResponse(call: Call, response: Response) {
                if (!response.isSuccessful) {
                    callback.onFailure(
                        NetworkError.ServerError(response.code, "Server error: ${response.code}")
                    )
                    return
                }
                
                try {
                    val body = response.body?.string()
                    val posts = gson.fromJson(body, Array<Post>::class.java).toList()
                    callback.onSuccess(posts)
                } catch (e: Exception) {
                    callback.onFailure(NetworkError.Unknown("Failed to parse response"))
                }
            }
        })
    }
}

// Data models
data class User(
    val id: Int,
    val name: String,
    val email: String
)

data class Post(
    val userId: Int,
    val id: Int,
    val title: String,
    val body: String
)
```

## Section 3: Converting Callbacks to Coroutines

Converting callback-based code to coroutines provides cleaner, more maintainable code.

```kotlin
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import kotlin.coroutines.Continuation
import kotlin.coroutines.resume
import kotlin.coroutines.resumeWithException

// Extension function to convert callback to suspend function
suspend fun <T> networkCall(
    call: () -> Unit
): T = suspendCancellableCoroutine { continuation ->
    // This is a simplified example - actual implementation depends on the API
    call()
}

// Suspend function wrapper for OkHttp callbacks
suspend fun OkHttpClient.suspendRequest(request: Request): Response = 
    suspendCancellableCoroutine { continuation ->
        val call = newCall(request)
        
        call.enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                if (continuation.isCancelled) return
                continuation.resumeWithException(e)
            }
            
            override fun onResponse(call: Call, response: Response) {
                if (continuation.isCancelled) {
                    response.close()
                    return
                }
                continuation.resume(response)
            }
        })
        
        continuation.invokeOnCancellation {
            call.cancel()
        }
    }

// Retrofit callback to suspend function
suspend fun <T> retrofitCall(
    call: () -> retrofit2.Call<T>
): T = suspendCancellableCoroutine { continuation ->
    val retrofitCall = call()
    
    retrofitCall.enqueue(object : retrofit2.Callback<T> {
        override fun onResponse(call: retrofit2.Call<T>, response: retrofit2.Response<T>) {
            if (continuation.isCancelled) return
            
            if (response.isSuccessful) {
                response.body()?.let {
                    continuation.resume(it)
                } ?: continuation.resumeWithException(
                    Exception("Empty response body")
                )
            } else {
                continuation.resumeWithException(
                    retrofit2.HttpException(response)
                )
            }
        }
        
        override fun onFailure(call: retrofit2.Call<T>, t: Throwable) {
            if (continuation.isCancelled) return
            continuation.resumeWithException(t)
        }
    })
    
    continuation.invokeOnCancellation {
        retrofitCall.cancel()
    }
}

// Generic callback to Flow converter
fun <T> callbackFlow(
    block: (onData: (T) -> Unit, onError: (Exception) -> Unit) -> Unit
): kotlinx.coroutines.flow.Flow<T> = kotlinx.coroutines.flow.callbackFlow {
    block(
        { data -> trySend(data) },
        { error -> close(error) }
    )
    awaitClose { }
}
```

## Section 4: Production Example - Callback Manager

This example demonstrates a complete callback-based system with proper lifecycle management and coroutine conversion.

```kotlin
import android.content.Context
import android.os.Handler
import android.os.Looper
import okhttp3.*
import java.io.IOException
import java.util.concurrent.TimeUnit
import kotlinx.coroutines.*

// Callback manager for lifecycle-aware operations
class CallbackManager(
    private val context: Context,
    private val scope: CoroutineScope
) {
    
    private val handler = Handler(Looper.getMainLooper())
    private val disposables = mutableListOf<Disposable>()
    
    // Add operation that can be cancelled
    fun addDisposable(disposable: Disposable) {
        disposables.add(disposable)
    }
    
    // Cancel all operations
    fun cancelAll() {
        disposables.forEach { it.dispose() }
        disposables.clear()
    }
}

// Network client with callbacks
class CallbackBasedClient(private val baseUrl: String) {
    
    private val client = OkHttpClient.Builder()
        .connectTimeout(15, TimeUnit.SECONDS)
        .readTimeout(30, TimeUnit.SECONDS)
        .writeTimeout(30, TimeUnit.SECONDS)
        .build()
    
    private val gson = com.google.gson.Gson()
    
    // Callback-based user fetch
    fun getUser(
        userId: Int,
        callback: UserCallback
    ) {
        val request = Request.Builder()
            .url("$baseUrl/users/$userId")
            .build()
        
        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                handler.post {
                    callback.onError(UserError.NetworkError(e.message ?: "Network error"))
                }
            }
            
            override fun onResponse(call: Call, response: Response) {
                if (!response.isSuccessful) {
                    handler.post {
                        callback.onError(UserError.HttpError(response.code, response.message))
                    }
                    return
                }
                
                try {
                    val body = response.body?.string()
                    val user = gson.fromJson(body, User::class.java)
                    handler.post {
                        callback.onSuccess(user)
                    }
                } catch (e: Exception) {
                    handler.post {
                        callback.onError(UserError.ParseError(e.message ?: "Parse error"))
                    }
                }
            }
        })
    }
    
    // Callback-based post list fetch
    fun getPosts(
        userId: Int? = null,
        callback: PostsCallback
    ) {
        val urlBuilder = HttpUrl.parse("$baseUrl/posts")?.newBuilder()
        
        userId?.let { urlBuilder?.addQueryParameter("userId", it.toString()) }
        
        val request = Request.Builder()
            .url(urlBuilder?.build()!!)
            .build()
        
        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                handler.post {
                    callback.onError(PostsError.NetworkError(e.message ?: "Network error"))
                }
            }
            
            override fun onResponse(call: Call, response: Response) {
                if (!response.isSuccessful) {
                    handler.post {
                        callback.onError(PostsError.HttpError(response.code, response.message))
                    }
                    return
                }
                
                try {
                    val body = response.body?.string()
                    val posts = gson.fromJson(body, Array<Post>::class.java).toList()
                    handler.post {
                        callback.onSuccess(posts)
                    }
                } catch (e: Exception) {
                    handler.post {
                        callback.onError(PostsError.ParseError(e.message ?: "Parse error"))
                    }
                }
            }
        })
    }
    
    // Callback-based create post
    fun createPost(
        post: Post,
        callback: CreatePostCallback
    ) {
        val json = gson.toJson(post)
        val body = RequestBody.create(
            MediaType.get("application/json; charset=utf-8"),
            json
        )
        
        val request = Request.Builder()
            .url("$baseUrl/posts")
            .post(body)
            .build()
        
        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                handler.post {
                    callback.onError(CreatePostError.NetworkError(e.message ?: "Network error"))
                }
            }
            
            override fun onResponse(call: Call, response: Response) {
                if (!response.isSuccessful) {
                    handler.post {
                        callback.onError(CreatePostError.HttpError(response.code, response.message))
                    }
                    return
                }
                
                try {
                    val body = response.body?.string()
                    val createdPost = gson.fromJson(body, Post::class.java)
                    handler.post {
                        callback.onSuccess(createdPost)
                    }
                } catch (e: Exception) {
                    handler.post {
                        callback.onError(CreatePostError.ParseError(e.message ?: "Parse error"))
                    }
                }
            }
        })
    }
}

// Callback interfaces
interface UserCallback {
    fun onSuccess(user: User)
    fun onError(error: UserError)
}

sealed class UserError {
    data class NetworkError(val message: String) : UserError()
    data class HttpError(val code: Int, val message: String) : UserError()
    data class ParseError(val message: String) : UserError()
}

interface PostsCallback {
    fun onSuccess(posts: List<Post>)
    fun onError(error: PostsError)
}

sealed class PostsError {
    data class NetworkError(val message: String) : PostsError()
    data class HttpError(val code: Int, val message: String) : PostsError()
    data class ParseError(val message: String) : PostsError()
}

interface CreatePostCallback {
    fun onSuccess(post: Post)
    fun onError(error: CreatePostError)
}

sealed class CreatePostError {
    data class NetworkError(val message: String) : CreatePostError()
    data class HttpError(val code: Int, val message: String) : CreatePostError()
    data class ParseError(val message: String) : CreatePostError()
}

// ViewModel using callbacks
class CallbackViewModel(
    private val client: CallbackBasedClient
) : ViewModel() {
    
    private val _user = MutableLiveData<User>()
    val user: LiveData<User> = _user
    
    private val _posts = MutableLiveData<List<Post>>()
    val posts: LiveData<List<Post>> = _posts
    
    private val _isLoading = MutableLiveData<Boolean>()
    val isLoading: LiveData<Boolean> = _isLoading
    
    private val _error = MutableLiveData<String>()
    val error: LiveData<String> = _error
    
    fun loadUser(userId: Int) {
        _isLoading.value = true
        
        client.getUser(userId, object : UserCallback {
            override fun onSuccess(user: User) {
                _user.value = user
                _isLoading.value = false
            }
            
            override fun onError(error: UserError) {
                _error.value = when (error) {
                    is UserError.NetworkError -> error.message
                    is UserError.HttpError -> "Error ${error.code}: ${error.message}"
                    is UserError.ParseError -> error.message
                }
                _isLoading.value = false
            }
        })
    }
    
    fun loadPosts(userId: Int? = null) {
        _isLoading.value = true
        
        client.getPosts(userId, object : PostsCallback {
            override fun onSuccess(posts: List<Post>) {
                _posts.value = posts
                _isLoading.value = false
            }
            
            override fun onError(error: PostsError) {
                _error.value = when (error) {
                    is PostsError.NetworkError -> error.message
                    is PostsError.HttpError -> "Error ${error.code}: ${error.message}"
                    is PostsError.ParseError -> error.message
                }
                _isLoading.value = false
            }
        })
    }
    
    fun createPost(title: String, body: String) {
        val post = Post(userId = 1, id = 0, title = title, body = body)
        
        client.createPost(post, object : CreatePostCallback {
            override fun onSuccess(createdPost: Post) {
                loadPosts()
            }
            
            override fun onError(error: CreatePostError) {
                _error.value = when (error) {
                    is CreatePostError.NetworkError -> error.message
                    is CreatePostError.HttpError -> "Error ${error.code}: ${error.message}"
                    is CreatePostError.ParseError -> error.message
                }
            }
        })
    }
}
```

## Section 5: Callback to Coroutine Bridge

Creating bridges between callback-based APIs and coroutines.

```kotlin
import kotlinx.coroutines.suspendCancellableCoroutine
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.*
import java.io.IOException
import java.util.concurrent.TimeUnit

// Complete callback-to-coroutine bridge
class CallbackCoroutineBridge {
    
    private val client = OkHttpClient.Builder()
        .connectTimeout(15, TimeUnit.SECONDS)
        .readTimeout(30, TimeUnit.SECONDS)
        .build()
    
    // Generic suspend wrapper for OkHttp
    suspend fun makeRequest(request: Request): Result<Response> = 
        suspendCancellableCoroutine { continuation ->
            val call = client.newCall(request)
            
            call.enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    if (continuation.isCancelled) return
                    continuation.resume(Result.failure(e))
                }
                
                override fun onResponse(call: Call, response: Response) {
                    if (continuation.isCancelled) {
                        response.close()
                        return
                    }
                    continuation.resume(Result.success(response))
                }
            })
            
            continuation.invokeOnCancellation {
                call.cancel()
            }
        }
    
    // Get string from URL
    suspend fun getString(url: String): Result<String> = withContext(Dispatchers.IO) {
        val request = Request.Builder()
            .url(url)
            .build()
        
        makeRequest(request).mapCatching { response ->
            if (!response.isSuccessful) {
                throw IOException("Unexpected response: ${response.code}")
            }
            response.body?.string() ?: throw IOException("Empty body")
        }
    }
    
    // Get JSON from URL
    suspend inline fun <reified T> getJson(url: String): Result<T> = 
        getString(url).mapCatching { json ->
            com.google.gson.Gson().fromJson(json, T::class.java)
        }
    
    // Post JSON
    suspend fun postJson(url: String, json: String): Result<String> = 
        withContext(Dispatchers.IO) {
            val body = RequestBody.create(
                MediaType.get("application/json"),
                json
            )
            
            val request = Request.Builder()
                .url(url)
                .post(body)
                .build()
            
            makeRequest(request).mapCatching { response ->
                if (!response.isSuccessful) {
                    throw IOException("Unexpected response: ${response.code}")
                }
                response.body?.string() ?: throw IOException("Empty body")
            }
        }
    
    // Result extension for mapCatching
    private inline fun <T, R> Result<T>.mapCatching(transform: (T) -> R): Result<R> {
        return fold(
            onSuccess = { Result.success(transform(it)) },
            onFailure = { Result.failure(it) }
        )
    }
}

// Android callback API wrapper
class AndroidCallbackWrapper<T>(
    private val scope: CoroutineScope
) {
    fun awaitCallback(
        block: (onComplete: (T) -> Unit, onError: (Exception) -> Unit) -> Unit
    ): Deferred<T> = scope.async(Dispatchers.Main) {
        suspendCancellableCoroutine { continuation ->
            block(
                { result -> 
                    if (continuation.isActive) {
                        continuation.resume(result)
                    }
                },
                { error ->
                    if (continuation.isActive) {
                        continuation.resumeWithException(error)
                    }
                }
            )
            
            continuation.invokeOnCancellation {
                // Cleanup if needed
            }
        }
    }
}

// Location callback wrapper example
class LocationCallbackWrapper(
    private val scope: CoroutineScope
) {
    suspend fun awaitLocation(): Location = suspendCancellableCoroutine { continuation ->
        // This is a conceptual example - actual implementation would use LocationManager
        // val locationManager = context.getSystemService(Context.LOCATION_SERVICE) as LocationManager
        
        // locationManager.requestSingleUpdate(android.location.LocationManager.GPS_PROVIDER, object : LocationListener {
        //     override fun onLocationChanged(location: Location) {
        //         if (continuation.isActive) {
        //             continuation.resume(location)
        //         }
        //     }
        // })
        
        // Simulating for example
        continuation.resume(Location(0.0, 0.0))
        
        continuation.invokeOnCancellation {
            // locationManager.removeUpdates(this)
        }
    }
}

data class Location(val latitude: Double, val longitude: Double)
```

## Best Practices

- **Prefer Coroutines**: Convert callbacks to suspend functions for cleaner code
- **Handle Errors**: Always provide both success and error callbacks
- **Post to Main Thread**: Use Handler for UI updates in callbacks
- **Cancel Properly**: Cancel operations when lifecycle ends
- **Use Lifecycle Awareness**: Bind callbacks to lifecycle appropriately

## Common Pitfalls

**Problem**: Memory leaks with callbacks
**Solution**: Cancel callbacks in onDestroy or onDestroyView

**Problem**: Callbacks executing after destruction
**Solution**: Check isActive/isCancelled before UI updates

**Problem**: Not handling all error cases
**Solution**: Create comprehensive error types

**Problem**: Thread issues
**Solution**: Use Handler to post to main thread

## Troubleshooting Guide

**Q: Why is my callback not firing?**
A: Check if the async operation completed or failed without calling callback

**Q: How to cancel callback operations?**
A: Store reference and cancel in lifecycle methods

**Q: Why is callback updating UI from background thread?**
A: Use Handler to post to main thread

## Advanced Tips

- **CompletableFuture**: Use Java 8 CompletableFuture with coroutines
- **ListenableFuture**: Use Guava's ListenableFuture with ListenableFutureCallback

## Cross-References

- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md) - Coroutines
- [Flow Implementation](./02_Flow_Implementation.md) - Flow from callbacks
- [Retrofit Basics](../01_HTTP_Communication/01_Retrofit_Basics.md) - Retrofit callbacks
- [Async Task Management](./04_Async_Task_Management.md) - Task management
