# RxJava Integration

## Learning Objectives

1. Understanding RxJava fundamentals and operators
2. Implementing Retrofit with RxJava
3. Managing observable streams with disposables
4. Handling backpressure in network operations
5. Converting from callbacks to RxJava observables

## Prerequisites

- [Retrofit Basics](../01_HTTP_Communication/01_Retrofit_Basics.md)
- [Kotlin Syntax and Fundamentals](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)

## Section 1: RxJava Fundamentals

RxJava is a library for composing asynchronous and event-based programs using observable sequences. While Kotlin coroutines are now preferred, RxJava remains valuable for complex reactive streams.

Key RxJava concepts:
- Observables emit data streams
- Subscribers consume the data
- Operators transform and filter streams
- Schedulers control thread execution
- Disposables manage resource cleanup

```kotlin
import io.reactivex.rxjava3.core.Observable
import io.reactivex.rxjava3.core.Single
import io.reactivex.rxjava3.core.Maybe
import io.reactivex.rxjava3.core.Completable
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.disposables.Disposable
import io.reactivex.rxjava3.schedulers.Schedulers
import io.reactivex.rxjava3.android.schedulers.AndroidSchedulers

// Simple observable creation
class RxJavaBasics {
    
    // Create observable from list
    fun listToObservable(): Observable<Int> {
        return Observable.fromList(listOf(1, 2, 3, 4, 5))
    }
    
    // Create observable from range
    fun rangeObservable(): Observable<Int> {
        return Observable.range(1, 10)
    }
    
    // Create single item observable
    fun singleObservable(): Single<String> {
        return Single.just("Hello RxJava")
    }
    
    // Create observable that may or may not emit
    fun maybeObservable(): Maybe<String> {
        return Maybe.just("May or may not emit")
    }
    
    // Create observable that doesn't emit items
    fun completableObservable(): Completable {
        return Completable.complete()
    }
    
    // Create observable that emits after delay
    fun delayedObservable(): Observable<Long> {
        return Observable.timer(1, java.util.concurrent.TimeUnit.SECONDS)
            .map { it + 1 }
    }
}

// Basic operators
class RxJavaOperators {
    
    // Map - transform each item
    fun mapOperator(): Observable<String> {
        return Observable.just(1, 2, 3)
            .map { "Number: $it" }
    }
    
    // FlatMap - transform and flatten
    fun flatMapOperator(): Observable<String> {
        return Observable.just(1, 2, 3)
            .flatMap { i ->
                Observable.just("$i A", "$i B")
            }
    }
    
    // Filter - filter items
    fun filterOperator(): Observable<Int> {
        return Observable.just(1, 2, 3, 4, 5)
            .filter { it > 2 }
    }
    
    // Take - limit items
    fun takeOperator(): Observable<Int> {
        return Observable.just(1, 2, 3, 4, 5)
            .take(3)
    }
    
    // Skip - skip items
    fun skipOperator(): Observable<Int> {
        return Observable.just(1, 2, 3, 4, 5)
            .skip(2)
    }
    
    // Distinct - remove duplicates
    fun distinctOperator(): Observable<Int> {
        return Observable.just(1, 2, 2, 3, 3, 3)
            .distinct()
    }
    
    // Reduce - combine all items
    fun reduceOperator(): Observable<Int> {
        return Observable.just(1, 2, 3, 4, 5)
            .reduce { acc, value -> acc + value }
    }
    
    // ToList - convert to list
    fun toListOperator(): Single<List<Int>> {
        return Observable.just(1, 2, 3, 4, 5)
            .toList()
    }
}
```

## Section 2: Retrofit with RxJava

Integrating Retrofit with RxJava provides powerful reactive networking capabilities.

```kotlin
import retrofit2.Retrofit
import retrofit2.adapter.rxjava3.RxJava3CallAdapterFactory
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query
import io.reactivex.rxjava3.core.Single
import io.reactivex.rxjava3.core.Observable

// Retrofit service with RxJava
interface RxJavaApiService {
    
    @GET("posts")
    fun getPosts(): Observable<List<Post>>
    
    @GET("posts/{id}")
    fun getPostById(@Path("id") id: Int): Single<Post>
    
    @GET("posts")
    fun getPostsByUser(
        @Query("userId") userId: Int
    ): Observable<List<Post>>
    
    @GET("posts")
    fun getPaginatedPosts(
        @Query("_page") page: Int,
        @Query("_limit") limit: Int
    ): Observable<List<Post>>
    
    @POST("posts")
    fun createPost(@Body post: Post): Single<Post>
    
    @DELETE("posts/{id}")
    fun deletePost(@Path("id") id: Int): Completable
}

// Retrofit client with RxJava adapter
object RxJavaRetrofitClient {
    
    private const val BASE_URL = "https://jsonplaceholder.typicode.com/"
    
    val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            // Add RxJava call adapter
            .addCallAdapterFactory(RxJava3CallAdapterFactory.create())
            .build()
    }
    
    val apiService: RxJavaApiService by lazy {
        retrofit.create(RxJavaApiService::class.java)
    }
}

// Repository with RxJava
class PostRepositoryRx(private val apiService: RxJavaApiService) {
    
    fun getPosts(): Observable<List<Post>> {
        return apiService.getPosts()
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
    }
    
    fun getPostById(id: Int): Single<Post> {
        return apiService.getPostById(id)
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
    }
    
    fun getPostsByUser(userId: Int): Observable<List<Post>> {
        return apiService.getPostsByUser(userId)
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
    }
}
```

## Section 3: Managing Disposables

Proper disposal is critical to prevent memory leaks in RxJava.

```kotlin
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.disposables.Disposable
import io.reactivex.rxjava3.core.Disposable
import androidx.lifecycle.ViewModel
import androidx.lifecycle.Lifecycle
import androidx.lifecycle.LifecycleOwner

// ViewModel with disposable management
class RxViewModel : ViewModel() {
    
    private val disposables = CompositeDisposable()
    
    fun <T : Disposable> addDisposable(disposable: T) {
        disposables.add(disposable)
    }
    
    override fun onCleared() {
        super.onCleared()
        disposables.clear()
    }
}

// Activity/Fragment with disposables
class NetworkActivity : AppCompatActivity() {
    
    private val disposables = CompositeDisposable()
    private val repository = PostRepositoryRx(RxJavaRetrofitClient.apiService)
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        
        loadPosts()
    }
    
    private fun loadPosts() {
        val disposable = repository.getPosts()
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe(
                { posts -> 
                    // Handle success
                    displayPosts(posts)
                },
                { error -> 
                    // Handle error
                    showError(error.message)
                }
            )
        
        disposables.add(disposable)
    }
    
    override fun onDestroy() {
        super.onDestroy()
        disposables.clear()
    }
}

// Lifecycle-aware disposable
class LifecycleAwareViewModel : ViewModel() {
    
    private val disposables = CompositeDisposable()
    
    fun <T : Disposable> addToDisposables(disposable: T) {
        disposables.add(disposable)
    }
    
    fun loadData() {
        val disposable = Observable.just(1, 2, 3)
            .subscribe { value ->
                println("Received: $value")
            }
        
        addToDisposables(disposable)
    }
    
    override fun onCleared() {
        super.onCleared()
        disposables.clear()
    }
}
```

## Section 4: Production Example - RxJava Network Layer

This example demonstrates a production-ready RxJava network layer with proper error handling and state management.

```kotlin
import io.reactivex.rxjava3.core.Observable
import io.reactivex.rxjava3.core.Single
import io.reactivex.rxjava3.core.Completable
import io.reactivex.rxjava3.schedulers.Schedulers
import io.reactivex.rxjava3.android.schedulers.AndroidSchedulers
import io.reactivex.rxjava3.disposables.CompositeDisposable
import io.reactivex.rxjava3.subjects.BehaviorSubject

// Data models
data class Todo(
    val userId: Int,
    val id: Int,
    val title: String,
    val completed: Boolean
)

// Network result wrapper
sealed class NetworkResult<out T> {
    data class Success<T>(val data: T) : NetworkResult<T>()
    data class Error(val message: String, val code: Int? = null) : NetworkResult<Nothing>()
    object Loading : NetworkResult<Nothing>()
}

// State management with BehaviorSubject
class TodoListStateManager {
    
    private val _state = BehaviorSubject.createDefault<NetworkResult<List<Todo>>>(NetworkResult.Loading)
    val state: Observable<NetworkResult<List<Todo>>> = _state.hide()
    
    fun updateState(newState: NetworkResult<List<Todo>>) {
        _state.onNext(newState)
    }
}

// Repository with RxJava
class RxTodoRepository(
    private val apiService: RxJavaTodoApiService
) {
    fun getTodos(): Observable<NetworkResult<List<Todo>>> {
        return apiService.getTodos()
            .map<List<Todo>> { todos -> todos }
            .toSingle()
            .map<NetworkResult<List<Todo>>> { NetworkResult.Success(it) }
            .onErrorReturn { error -> 
                NetworkResult.Error(error.message ?: "Unknown error")
            }
            .startWithItem(NetworkResult.Loading)
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
    }
    
    fun createTodo(title: String): Single<NetworkResult<Todo>> {
        val todo = Todo(userId = 1, id = 0, title = title, completed = false)
        
        return apiService.createTodo(todo)
            .map<NetworkResult<Todo>> { created -> NetworkResult.Success(created) }
            .onErrorReturn { error ->
                NetworkResult.Error(error.message ?: "Failed to create todo")
            }
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
    }
    
    fun toggleTodo(todo: Todo): Single<NetworkResult<Todo>> {
        val updated = todo.copy(completed = !todo.completed)
        
        return apiService.updateTodo(todo.id, updated)
            .map<NetworkResult<Todo>> { NetworkResult.Success(it) }
            .onErrorReturn { error ->
                NetworkResult.Error(error.message ?: "Failed to update todo")
            }
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
    }
    
    fun deleteTodo(id: Int): Completable {
        return apiService.deleteTodo(id)
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
    }
}

// ViewModel with RxJava
class RxTodoViewModel(
    private val repository: RxTodoRepository
) : ViewModel() {
    
    private val disposables = CompositeDisposable()
    
    private val _todos = MutableLiveData<List<Todo>>()
    val todos: LiveData<List<Todo>> = _todos
    
    private val _isLoading = MutableLiveData<Boolean>()
    val isLoading: LiveData<Boolean> = _isLoading
    
    private val _error = MutableLiveData<String?>()
    val error: LiveData<String?> = _error
    
    init {
        loadTodos()
    }
    
    fun loadTodos() {
        val disposable = repository.getTodos()
            .subscribe { result ->
                when (result) {
                    is NetworkResult.Loading -> _isLoading.value = true
                    is NetworkResult.Success -> {
                        _todos.value = result.data
                        _isLoading.value = false
                        _error.value = null
                    }
                    is NetworkResult.Error -> {
                        _isLoading.value = false
                        _error.value = result.message
                    }
                }
            }
        
        disposables.add(disposable)
    }
    
    fun createTodo(title: String) {
        val disposable = repository.createTodo(title)
            .subscribe { result ->
                when (result) {
                    is NetworkResult.Success -> loadTodos()
                    is NetworkResult.Error -> _error.value = result.message
                    else -> {}
                }
            }
        
        disposables.add(disposable)
    }
    
    fun toggleTodo(todo: Todo) {
        val disposable = repository.toggleTodo(todo)
            .subscribe { result ->
                when (result) {
                    is NetworkResult.Success -> loadTodos()
                    is NetworkResult.Error -> _error.value = result.message
                    else -> {}
                }
            }
        
        disposables.add(disposable)
    }
    
    fun deleteTodo(id: Int) {
        val disposable = repository.deleteTodo(id)
            .subscribe(
                { loadTodos() },
                { error -> _error.value = error.message }
            )
        
        disposables.add(disposable)
    }
    
    override fun onCleared() {
        super.onCleared()
        disposables.clear()
    }
}
```

## Section 5: Production Example - Complex RxJava Streams

This example shows complex RxJava patterns for combining multiple requests and handling backpressure.

```kotlin
import io.reactivex.rxjava3.core.Observable
import io.reactivex.rxjava3.core.Single
import io.reactivex.rxjava3.schedulers.Schedulers
import io.reactivex.rxjava3.android.schedulers.AndroidSchedulers
import io.reactivex.rxjava3.subjects.PublishSubject
import io.reactivex.rxjava3.subjects.BehaviorSubject

// Search with debounce
class SearchManager {
    
    private val searchSubject = PublishSubject.create<String>()
    private val searchResults = MutableLiveData<List<SearchResult>>()
    val results: LiveData<List<SearchResult>> = searchResults
    
    init {
        setupSearchStream()
    }
    
    private fun setupSearchStream() {
        val disposable = searchSubject
            .debounce(300, java.util.concurrent.TimeUnit.MILLISECONDS)
            .distinctUntilChanged()
            .switchMap { query ->
                searchApi(query)
            }
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe(
                { searchResults.value = it },
                { error -> searchResults.value = emptyList() }
            )
    }
    
    fun search(query: String) {
        searchSubject.onNext(query)
    }
    
    private fun searchApi(query: String): Observable<List<SearchResult>> {
        // Simulate API call
        return Observable.just(listOf(
            SearchResult(query, "Result 1 for $query"),
            SearchResult(query, "Result 2 for $query")
        ))
    }
}

data class SearchResult(val query: String, val title: String)

// Combine multiple API calls
class CombinedDataManager {
    
    fun getDashboardData(): Observable<DashboardData> {
        val userSingle = getUserData()
        val postsSingle = getPostsData()
        val statsSingle = getStatsData()
        
        return Single.zip(
            userSingle,
            postsSingle,
            statsSingle
        ) { user, posts, stats ->
            DashboardData(user, posts, stats)
        }.toObservable()
    }
    
    private fun getUserData(): Single<User> {
        return Single.just(User("John", "john@example.com"))
    }
    
    private fun getPostsData(): Single<List<Post>> {
        return Single.just(listOf(Post(1, "Post 1"), Post(2, "Post 2")))
    }
    
    private fun getStatsData(): Single<Stats> {
        return Single.just(Stats(100, 50))
    }
}

data class DashboardData(val user: User, val posts: List<Post>, val stats: Stats)
data class User(val name: String, val email: String)
data class Stats(val views: Int, val likes: Int)

// Pagination with RxJava
class PaginationManager<T>(
    private val loadPage: (Int) -> Single<List<T>>
) {
    private var currentPage = 1
    private var isLoading = false
    private val items = mutableListOf<T>()
    private val itemsSubject = BehaviorSubject.createDefault<List<T>>(emptyList())
    
    val allItems: Observable<List<T>> = itemsSubject.hide()
    
    fun loadNextPage() {
        if (isLoading) return
        
        isLoading = true
        
        loadPage(currentPage)
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe(
                { newItems ->
                    items.addAll(newItems)
                    itemsSubject.onNext(items.toList())
                    currentPage++
                    isLoading = false
                },
                { error ->
                    isLoading = false
                }
            )
    }
    
    fun refresh() {
        currentPage = 1
        items.clear()
        loadNextPage()
    }
}
```

## Best Practices

- **Use Proper Schedulers**: Always specify subscribeOn and observeOn for thread control
- **Dispose Properly**: Clear disposables in onCleared or onDestroy
- **Use Single for One-Shot Requests**: Use Single instead of Observable for single-item responses
- **Handle Errors**: Always implement onErrorReturn or onErrorResumeNext
- **Avoid Memory Leaks**: Use CompositeDisposable for managing multiple subscriptions
- **Consider Coroutines**: For new code, prefer coroutines over RxJava
- **Use backpressure strategies**: Handle slow consumers with backpressure operators

## Common Pitfalls

**Problem**: Memory leaks from undisposed observables
**Solution**: Always dispose in lifecycle methods

**Problem**: Network calls on main thread
**Solution**: Use subscribeOn(Schedulers.io())

**Problem**: Too many subscriptions
**Solution**: Use CompositeDisposable

**Problem**: Not handling errors
**Solution**: Always add error handlers

## Troubleshooting Guide

**Q: Why is my observable not emitting?**
A: Check if you're subscribed and if there's a subscriber

**Q: How to convert callback to RxJava?**
A: Use Observable.fromCallable or custom Observable.create

**Q: Why is backpressure happening?**
A: Use Flowable instead of Observable for backpressure

## Advanced Tips

- **Testing with TestSubscriber**: Test RxJava streams
- **Custom Operators**: Create operators for repeated logic
- **Migration to Coroutines**: Consider migrating to coroutines/Flow

## Cross-References

- [Retrofit Basics](../01_HTTP_Communication/01_Retrofit_Basics.md) - RxJava with Retrofit
- [Flow Implementation](./02_Flow_Implementation.md) - Kotlin Flow alternative
- [Coroutines Basics](../../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md) - Coroutines fundamentals
- [Async Task Management](./04_Async_Task_Management.md) - Task management
