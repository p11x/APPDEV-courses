# MVC and MVP

## Learning Objectives

1. Understanding MVC and MVP architecture patterns
2. Implementing Model-View-Controller in Android
3. Implementing Model-View-Presenter
4. Choosing between MVC and MVP

## Section 1: MVC Overview

MVC (Model-View-Controller) Overview

MVC separates application into three components.

```kotlin
object MVCOverview {
    
    // Model - data and business logic
    class Model {
        data class User(val id: Int, val name: String, val email: String)
        
        fun getUsers(): List<User> {
            return listOf(
                User(1, "John", "john@example.com"),
                User(2, "Jane", "jane@example.com")
            )
        }
        
        fun getUser(id: Int): User? {
            return getUsers().find { it.id == id }
        }
    }
    
    // View - UI (Activity/Fragment)
    interface View {
        fun showUsers(users: List<Model.User>)
        fun showError(message: String)
        fun showLoading()
    }
    
    // Controller - handles user interactions
    class Controller(private val view: View) {
        private val model = Model()
        
        fun loadUsers() {
            view.showLoading()
            try {
                val users = model.getUsers()
                view.showUsers(users)
            } catch (e: Exception) {
                view.showError(e.message ?: "Error loading users")
            }
        }
        
        fun loadUser(id: Int) {
            val user = model.getUser(id)
            if (user != null) {
                view.showUsers(listOf(user))
            } else {
                view.showError("User not found")
            }
        }
    }
}
```

## Section 2: MVP Overview

MVP (Model-View-Presenter) Overview

MVP improves testability over MVC.

```kotlin
object MVPOverview {
    
    // Model
    data class Item(val id: Int, val name: String, val description: String)
    
    // View interface
    interface ItemListView {
        fun showItems(items: List<Item>)
        fun showError(message: String)
        fun showLoading()
        fun hideLoading()
        fun navigateToDetail(itemId: Int)
    }
    
    // Presenter
    class ItemListPresenter(
        private val view: ItemListView,
        private val repository: ItemRepository
    ) {
        fun loadItems() {
            view.showLoading()
            
            // Using coroutines
            androidx.lifecycle.viewModelScope.launch {
                try {
                    val items = repository.getItems()
                    view.hideLoading()
                    view.showItems(items)
                } catch (e: Exception) {
                    view.hideLoading()
                    view.showError(e.message ?: "Error loading items")
                }
            }
        }
        
        fun onItemClicked(item: Item) {
            view.navigateToDetail(item.id)
        }
    }
    
    // Repository
    class ItemRepository {
        suspend fun getItems(): List<Item> {
            kotlinx.coroutines.delay(500)
            return listOf(
                Item(1, "Item 1", "Description 1"),
                Item(2, "Item 2", "Description 2"),
                Item(3, "Item 3", "Description 3")
            )
        }
    }
}
```

## Example: MVP Implementation

MVP Implementation Example

Complete MVP pattern implementation.

```kotlin
class MVPImplementation {
    
    // Contract
    interface LoginContract {
        interface View {
            fun showLoading()
            fun hideLoading()
            fun showError(message: String)
            fun navigateToHome()
            fun showEmailError(message: String?)
            fun showPasswordError(message: String?)
        }
        
        interface Presenter {
            fun attachView(view: View)
            fun detachView()
            fun login(email: String, password: String)
            fun onGuestClicked()
        }
    }
    
    // Presenter implementation
    class LoginPresenter(
        private val authRepository: AuthRepository
    ) : LoginContract.Presenter {
        
        private var view: LoginContract.View? = null
        
        override fun attachView(view: LoginContract.View) {
            this.view = view
        }
        
        override fun detachView() {
            this.view = null
        }
        
        override fun login(email: String, password: String) {
            // Validate
            var hasError = false
            
            if (email.isBlank()) {
                view?.showEmailError("Email is required")
                hasError = true
            } else if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                view?.showEmailError("Invalid email format")
                hasError = true
            } else {
                view?.showEmailError(null)
            }
            
            if (password.isBlank()) {
                view?.showPasswordError("Password is required")
                hasError = true
            } else if (password.length < 6) {
                view?.showPasswordError("Password must be at least 6 characters")
                hasError = true
            } else {
                view?.showPasswordError(null)
            }
            
            if (hasError) return
            
            // Proceed with login
            view?.showLoading()
            
            androidx.lifecycle.viewModelScope.launch {
                try {
                    val result = authRepository.login(email, password)
                    view?.hideLoading()
                    view?.navigateToHome()
                } catch (e: Exception) {
                    view?.hideLoading()
                    view?.showError(e.message ?: "Login failed")
                }
            }
        }
        
        override fun onGuestClicked() {
            view?.navigateToHome()
        }
    }
    
    // Activity as View
    class LoginActivity : android.app.Activity(), LoginContract.View {
        
        private lateinit var presenter: LoginContract.Presenter
        private lateinit var binding: LoginLayoutBinding
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            binding = LoginLayoutBinding.inflate(layoutInflater)
            setContentView(binding.root)
            
            presenter = LoginPresenter(AuthRepository())
            presenter.attachView(this)
            
            setupViews()
        }
        
        private fun setupViews() {
            binding.loginButton.setOnClickListener {
                val email = binding.emailInput.text.toString()
                val password = binding.passwordInput.text.toString()
                presenter.login(email, password)
            }
            
            binding.guestButton.setOnClickListener {
                presenter.onGuestClicked()
            }
        }
        
        override fun showLoading() {
            binding.progressBar.visibility = android.view.View.VISIBLE
            binding.loginButton.isEnabled = false
        }
        
        override fun hideLoading() {
            binding.progressBar.visibility = android.view.View.GONE
            binding.loginButton.isEnabled = true
        }
        
        override fun showError(message: String) {
            android.widget.Toast.makeText(this, message, android.widget.Toast.LENGTH_SHORT).show()
        }
        
        override fun navigateToHome() {
            startActivity(android.content.Intent(this, HomeActivity::class.java))
            finish()
        }
        
        override fun showEmailError(message: String?) {
            binding.emailLayout.error = message
        }
        
        override fun showPasswordError(message: String?) {
            binding.passwordLayout.error = message
        }
        
        override fun onDestroy() {
            super.onDestroy()
            presenter.detachView()
        }
    }
    
    // Simple binding
    class LoginLayoutBinding(
        val root: android.view.View,
        val emailInput: android.widget.EditText,
        val passwordInput: android.widget.EditText,
        val loginButton: android.widget.Button,
        val guestButton: android.widget.Button,
        val progressBar: android.widget.ProgressBar,
        val emailLayout: com.google.android.material.textfield.TextInputLayout,
        val passwordLayout: com.google.android.material.textfield.TextInputLayout
    )
    
    class HomeActivity : android.app.Activity()
    
    // Auth repository
    class AuthRepository {
        suspend fun login(email: String, password: String): Boolean {
            kotlinx.coroutines.delay(1000)
            return email.isNotBlank() && password.length >= 6
        }
    }
}
```

## Output Statement Results

MVC Pattern:
- Model: Data and business logic
- View: UI (Activity/Fragment)
- Controller: Handles interactions
- Direct communication between View and Model

MVP Pattern:
- Model: Data and business logic
- View: UI interface
- Presenter: Business logic
- View and Presenter communicate via interface
- Better testability than MVC

Key Differences:
- MVC: View and Model can communicate directly
- MVP: View and Presenter communicate via contract
- MVP: Presenter handles all business logic
- MVP: View is passive (all UI logic in Presenter)

Advantages of MVP:
- Testable: Presenter can be unit tested
- Clear separation of concerns
- Easier to mock View in tests
- View is passive (simpler)

Disadvantages:
- More boilerplate than MVC
- Need to maintain interfaces
- View can't access Model directly

## Cross-References

- See: [02_MVVM_Implementation.md](../02_MVVM_Implementation.md)
- See: [03_Clean_Architecture.md](../03_Clean_Architecture.md)