# MVVM Implementation

## Learning Objectives

1. Understanding MVVM architecture
2. Implementing ViewModel and LiveData
3. Using state management with StateFlow
4. Connecting ViewModel with UI
5. Testing ViewModels

## Section 1: MVVM Overview

MVVM (Model-View-ViewModel) Overview

MVVM provides clear separation of UI and business logic.

```kotlin
object MVVMOverview {
    
    // Components
    // Model: Data layer (Repository, API, Database)
    // View: Activity/Fragment/Compose UI
    // ViewModel: UI state and logic
    
    // Key benefits
    val benefits = listOf(
        "Separation of concerns",
        "Testability",
        "Lifecycle awareness",
        "Observable state",
        "Reduced boilerplate with DataBinding/Compose"
    )
}
```

## Section 2: ViewModel and LiveData

ViewModel and LiveData

Core components of MVVM.

```kotlin
class ViewModelLiveData {
    
    // ViewModel
    class UserViewModel : androidx.lifecycle.ViewModel() {
        
        // LiveData for UI state
        private val _users = androidx.lifecycle.MutableLiveData<List<User>>()
        val users: androidx.lifecycle.LiveData<List<User>> = _users
        
        private val _selectedUser = androidx.lifecycle.MutableLiveData<User?>()
        val selectedUser: androidx.lifecycle.LiveData<User?> = _selectedUser
        
        private val _isLoading = androidx.lifecycle.MutableLiveData(false)
        val isLoading: androidx.lifecycle.LiveData<Boolean> = _isLoading
        
        private val _error = androidx.lifecycle.MutableLiveData<String?>()
        val error: androidx.lifecycle.LiveData<String?> = _error
        
        // Repository reference
        private val repository = UserRepository()
        
        fun loadUsers() {
            viewModelScope.launch {
                _isLoading.value = true
                try {
                    val result = repository.getUsers()
                    _users.value = result
                    _error.value = null
                } catch (e: Exception) {
                    _error.value = e.message
                } finally {
                    _isLoading.value = false
                }
            }
        }
        
        fun selectUser(user: User) {
            _selectedUser.value = user
        }
        
        fun clearSelection() {
            _selectedUser.value = null
        }
        
        fun clearError() {
            _error.value = null
        }
    }
    
    data class User(val id: Int, val name: String, val email: String)
    
    class UserRepository {
        suspend fun getUsers(): List<User> {
            kotlinx.coroutines.delay(500)
            return listOf(
                User(1, "John", "john@example.com"),
                User(2, "Jane", "jane@example.com")
            )
        }
    }
    
    // Activity/Fragment using ViewModel
    class UserActivity : android.app.Activity() {
        
        private val viewModel: UserViewModel by androidx.lifecycle.viewmodel.viewModel()
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            // Observe data
            viewModel.users.observe(this) { users ->
                // Update UI
                println("Users: ${users.size}")
            }
            
            viewModel.isLoading.observe(this) { isLoading ->
                // Show/hide loading
            }
            
            viewModel.error.observe(this) { error ->
                error?.let {
                    android.widget.Toast.makeText(this, it, android.widget.Toast.LENGTH_SHORT).show()
                    viewModel.clearError()
                }
            }
            
            // Load data
            viewModel.loadUsers()
        }
    }
}
```

## Section 3: StateFlow with ViewModel

StateFlow with ViewModel

Modern approach using StateFlow.

```kotlin
class StateFlowViewModel {
    
    // ViewModel with StateFlow
    class ArticleViewModel(
        private val repository: ArticleRepository
    ) : androidx.lifecycle.ViewModel() {
        
        // UI State as StateFlow
        private val _uiState = androidx.coroutines.flow.MutableStateFlow(ArticleUiState())
        val uiState: androidx.coroutines.flow.StateFlow<ArticleUiState> = _uiState
        
        // Events as SharedFlow
        private val _events = androidx.coroutines.flow.MutableSharedFlow<ArticleEvent>()
        val events: androidx.coroutines.flow.SharedFlow<ArticleEvent> = _events
        
        fun loadArticles() {
            viewModelScope.launch {
                _uiState.value = _uiState.value.copy(isLoading = true)
                
                try {
                    val articles = repository.getArticles()
                    _uiState.value = _uiState.value.copy(
                        isLoading = false,
                        articles = articles
                    )
                } catch (e: Exception) {
                    _uiState.value = _uiState.value.copy(
                        isLoading = false,
                        error = e.message
                    )
                }
            }
        }
        
        fun refresh() {
            loadArticles()
        }
        
        fun onArticleClicked(article: Article) {
            viewModelScope.launch {
                _events.emit(ArticleEvent.NavigateToDetail(article.id))
            }
        }
        
        fun onRefreshComplete() {
            _uiState.value = _uiState.value.copy(isRefreshing = false)
        }
    }
    
    // UI State
    data class ArticleUiState(
        val isLoading: Boolean = false,
        val isRefreshing: Boolean = false,
        val articles: List<Article> = emptyList(),
        val error: String? = null
    )
    
    // Events
    sealed class ArticleEvent {
        data class NavigateToDetail(val articleId: String) : ArticleEvent()
        data class ShowMessage(val message: String) : ArticleEvent()
        object NavigateBack : ArticleEvent()
    }
    
    data class Article(val id: String, val title: String, val content: String)
    
    class ArticleRepository {
        suspend fun getArticles(): List<Article> {
            kotlinx.coroutines.delay(500)
            return listOf(
                Article("1", "Article 1", "Content 1"),
                Article("2", "Article 2", "Content 2")
            )
        }
    }
    
    // Compose UI with StateFlow
    @Composable
    fun ArticleScreen(
        viewModel: ArticleViewModel = androidx.lifecycle.viewmodel.compose.viewModel()
    ) {
        val uiState by viewModel.uiState.collectAsStateWithLifecycle()
        
        when {
            uiState.isLoading -> {
                androidx.compose.material3.CircularProgressIndicator()
            }
            uiState.error != null -> {
                Column {
                    androidx.compose.material3.Text(uiState.error!!)
                    androidx.compose.material3.Button(onClick = viewModel::loadArticles) {
                        androidx.compose.material3.Text("Retry")
                    }
                }
            }
            else -> {
                LazyColumn {
                    items(uiState.articles) { article ->
                        ArticleItem(
                            article = article,
                            onClick = { viewModel.onArticleClicked(article) }
                        )
                    }
                }
            }
        }
    }
    
    @Composable
    fun ArticleItem(
        article: Article,
        onClick: () -> Unit
    ) {
        androidx.compose.material3.Card(
            onClick = onClick,
            modifier = Modifier.fillMaxWidth()
        ) {
            androidx.compose.material3.Text(article.title)
        }
    }
}
```

## Example: Complete MVVM Implementation

Complete MVVM Implementation

Full example with all MVVM components.

```kotlin
class CompleteMVVM {
    
    // 1. Repository (Data Layer)
    class ProductRepository {
        private val api = ProductApi()
        private val dao = ProductDao()
        
        suspend fun getProducts(): List<Product> {
            return try {
                val networkProducts = api.fetchProducts()
                dao.insertAll(networkProducts)
                networkProducts
            } catch (e: Exception) {
                dao.getAll()
            }
        }
    }
    
    class ProductApi {
        suspend fun fetchProducts(): List<Product> {
            kotlinx.coroutines.delay(500)
            return listOf(
                Product("1", "Product 1", 10.0),
                Product("2", "Product 2", 20.0)
            )
        }
    }
    
    class ProductDao {
        private val products = mutableListOf<Product>()
        suspend fun getAll() = products.toList()
        suspend fun insertAll(newProducts: List<Product>) {
            products.clear()
            products.addAll(newProducts)
        }
    }
    
    data class Product(val id: String, val name: String, val price: Double)
    
    // 2. Use Case (Optional - for Clean Architecture)
    class GetProductsUseCase(private val repository: ProductRepository) {
        suspend operator fun invoke(): List<Product> {
            return repository.getProducts()
        }
    }
    
    // 3. ViewModel
    class ProductListViewModel(
        private val getProductsUseCase: GetProductsUseCase
    ) : androidx.lifecycle.ViewModel() {
        
        private val _state = androidx.coroutines.flow.MutableStateFlow(ProductListState())
        val state: androidx.coroutines.flow.StateFlow<ProductListState> = _state
        
        init {
            loadProducts()
        }
        
        fun loadProducts() {
            viewModelScope.launch {
                _state.value = _state.value.copy(isLoading = true)
                
                try {
                    val products = getProductsUseCase()
                    _state.value = _state.value.copy(
                        isLoading = false,
                        products = products
                    )
                } catch (e: Exception) {
                    _state.value = _state.value.copy(
                        isLoading = false,
                        error = e.message
                    )
                }
            }
        }
        
        fun onProductClick(product: Product) {
            viewModelScope.launch {
                _state.value = _state.value.copy(selectedProduct = product)
            }
        }
        
        fun clearSelection() {
            _state.value = _state.value.copy(selectedProduct = null)
        }
    }
    
    data class ProductListState(
        val isLoading: Boolean = false,
        val products: List<Product> = emptyList(),
        val selectedProduct: Product? = null,
        val error: String? = null
    )
    
    // 4. Fragment (View)
    class ProductListFragment : androidx.fragment.app.Fragment() {
        
        private val viewModel: ProductListViewModel by viewModels {
            androidx.lifecycle.viewmodel.viewModelFactory {
                val repository = ProductRepository()
                val useCase = GetProductsUseCase(repository)
                ProductListViewModel(useCase)
            }
        }
        
        override fun onViewCreated(
            view: android.view.View,
            savedInstanceState: android.os.Bundle?
        ) {
            super.onViewCreated(view, savedInstanceState)
            
            // Observe state with repeatOnLifecycle
            viewLifecycleOwner.lifecycleScope.launch {
                viewLifecycleOwner.repeatOnLifecycle(androidx.lifecycle.Lifecycle.State.STARTED) {
                    viewModel.state.collect { state ->
                        updateUI(state)
                    }
                }
            }
        }
        
        private fun updateUI(state: ProductListState) {
            if (state.isLoading) {
                // Show loading
            } else if (state.error != null) {
                // Show error
            } else {
                // Show products
            }
        }
    }
}
```

## Output Statement Results

MVVM Components:
- Model/Repository: Data access
- ViewModel: UI state and logic
- View: UI rendering (Activity/Fragment/Compose)

LiveData:
- Observable data holder
- Lifecycle-aware
- Requires manual observation
- SetValue/PostValue for updates

StateFlow:
- Modern reactive state
- Lifecycle-aware with collectAsStateWithLifecycle
- Single current value
- Initial value required

SharedFlow:
- Event handling
- Multiple subscribers
- Replay to new subscribers

ViewModel Creation:
- ViewModelProvider
- viewModels delegate
- Factory pattern
- Hilt integration

## Cross-References

- See: [01_MVC_and_MVP.md](../01_MVC_and_MVP.md)
- See: [03_Clean_Architecture.md](../03_Clean_Architecture.md)