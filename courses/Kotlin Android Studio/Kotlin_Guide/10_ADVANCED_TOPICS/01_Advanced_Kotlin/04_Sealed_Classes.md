# Sealed Classes

## Overview

Sealed classes in Kotlin represent restricted hierarchies where all subclasses are known at compile time. This enables exhaustive when expressions and provides type-safe modeling for domain logic.

## Learning Objectives

- Master sealed class hierarchies for domain modeling
- Implement exhaustive pattern matching
- Create type-safe error handling with sealed classes
- Build state machines with sealed classes
- Understand sealed class vs enum differences

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Type System and Collections](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/03_Type_System_and_Collections.md)

## Core Concepts

### Sealed Class Hierarchy

Sealed classes restrict class inheritance:
- All subclasses must be in the same file
- Compiler knows all possible subtypes
- Enables exhaustive when expressions
- Provides compile-time exhaustive checking

### Sealed vs Enums

Sealed classes support:
- Multiple instances per subclass
- Different constructor parameters
- State-carrying subclasses
- Type-specific behavior

### Modeling Approaches

Common sealed class patterns:
- Result types (Success/Error)
- State machines
- UI states
- Navigation destinations

## Code Examples

### Example 1: Result Type and Error Handling

```kotlin
/**
 * Sealed class representing operation results
 * Provides type-safe error handling
 */
sealed class Result<out T> {
    /**
     * Successful result with data
     */
    data class Success<T>(val data: T) : Result<T>()
    
    /**
     * Failure with error information
     */
    data class Error(val exception: Throwable) : Result<Nothing>()
    
    /**
     * Loading state
     */
    object Loading : Result<Nothing>()
    
    /**
     * Empty result
     */
    object Empty : Result<Nothing>()
    
    companion object {
        fun <T> success(data: T): Result<T> = Success(data)
        
        fun error(exception: Throwable): Result<Nothing> = Error(exception)
        
        fun loading(): Result<Nothing> = Loading
        
        fun empty(): Result<Nothing> = Empty
    }
    
    /**
     * Transform success data
     */
    inline fun <R> map(transform: (T) -> R): Result<R> = when (this) {
        is Success -> Success(transform(data))
        is Error -> Error(exception)
        Loading -> Loading
        Empty -> Empty
    }
    
    /**
     * Flat transform success data
     */
    inline fun <R> flatMap(transform: (T) -> Result<R>): Result<R> = when (this) {
        is Success -> transform(data)
        is Error -> Error(exception)
        Loading -> Loading
        Empty -> Empty
    }
    
    /**
     * Get data or default
     */
    fun getOrDefault(default: @UnsafeVariance T): T = when (this) {
        is Success -> data
        else -> default
    }
    
    /**
     * Get data or null
     */
    fun getOrNull(): T? = when (this) {
        is Success -> data
        else -> null
    }
    
    /**
     * Get data or throw
     */
    fun getOrThrow(): T = when (this) {
        is Success -> data
        is Error -> throw exception
        else -> throw IllegalStateException("Not a success")
    }
    
    /**
     * Check if success
     */
    fun isSuccess(): Boolean = this is Success
    
    /**
     * Check if error
     */
    fun isError(): Boolean = this is Error
}

/**
 * API result with HTTP status
 */
sealed class ApiResult<out T> {
    data class Success<T>(
        val data: T,
        val statusCode: Int = 200,
        val headers: Map<String, String> = emptyMap()
    ) : ApiResult<T>()
    
    data class Error<T>(
        val message: String,
        val statusCode: Int,
        val cause: Throwable? = null
    ) : ApiResult<T>()
    
    sealed class HttpError<T> : ApiResult<T>() {
        data class NotFound<T>(val message: String = "Resource not found") : HttpError<T>()
        data class Unauthorized<T>(val message: String = "Unauthorized") : HttpError<T>()
        data class Forbidden<T>(val message: String = "Access forbidden") : HttpError<T>()
        data class ServerError<T>(val message: String = "Server error") : HttpError<T>()
    }
    
    object Loading : ApiResult<Nothing>()
    
    companion object {
        fun <T> success(data: T): ApiResult<T> = Success(data)
        fun <T> notFound(message: String = "Not found"): ApiResult<T> = HttpError.NotFound(message)
        fun <T> unauthorized(): ApiResult<T> = HttpError.Unauthorized()
        fun <T> serverError(message: String = "Server error"): ApiResult<T> = HttpError.ServerError(message)
    }
}

/**
 * Database operation result
 */
sealed class DbResult<out T> {
    data class Inserted<T>(val id: Long) : DbResult<T>()
    data class Updated<T>(val rowsAffected: Int) : DbResult<T>()
    data class Deleted<T>(val rowsAffected: Int) : DbResult<T>()
    data class Selected<T>(val data: T) : DbResult<T>()
    data class QueryResult<T>(val items: List<T>) : DbResult<T>()
    data class Error(val exception: Throwable) : DbResult<Nothing>()
    object NoData : DbResult<Nothing>()
    
    fun getOrThrow(): T = when (this) {
        is Selected -> data
        is QueryResult -> throw IllegalStateException("Multiple results")
        is Inserted, is Updated, is Deleted -> throw IllegalStateException("Non-query operation")
        is Error -> throw exception
        NoData -> throw IllegalStateException("No data")
    }
}

/**
 * Use case with result handling
 */
class GetUserUseCase(
    private val repository: UserRepository
) {
    suspend fun execute(userId: String): Result<User> {
        return try {
            val user = repository.getUser(userId)
            if (user != null) {
                Result.success(user)
            } else {
                Result.error(UserNotFoundException(userId))
            }
        } catch (e: Exception) {
            Result.error(e)
        }
    }
    
    private class UserNotFoundException(val userId: String) : 
        Exception("User not found: $userId")
}

/**
 * Repository interface for use case
 */
interface UserRepository {
    suspend fun getUser(id: String): User?
    suspend fun saveUser(user: User): Long
    suspend fun deleteUser(id: String): Boolean
}

data class User(val id: Long, val name: String, val email: String)

/**
 * Example of exhaustive when
 */
class ResultExample {
    fun demonstrate(result: Result<User>) {
        // Kotlin ensures exhaustive handling
        val description = when (result) {
            is Result.Success -> "User: ${result.data.name}"
            is Result.Error -> "Error: ${result.exception.message}"
            is Result.Loading -> "Loading..."
            is Result.Empty -> "No data"
        }
        println(description)
        
        // Can also return from when
        val state = when (result) {
            is Result.Success -> "loaded"
            is Result.Error -> "error"
            is Result.Loading -> "loading"
            is Result.Empty -> "empty"
        }
        println("State: $state")
    }
}
```

**Output:**
```
User: John
State: loaded
```

This example demonstrates sealed classes for type-safe result handling.

### Example 2: State Machines with Sealed Classes

```kotlin
import kotlin.coroutines.*

/**
 * State machine for authentication flow
 */
sealed class AuthState {
    /**
     * Initial unauthenticated state
     */
    object Unauthenticated : AuthState()
    
    /**
     * Currently authenticating
     */
    data class Authenticating(
        val email: String,
        val progress: Float = 0f
    ) : AuthState()
    
    /**
     * Successfully authenticated
     */
    data class Authenticated(
        val userId: String,
        val token: String,
        val refreshToken: String? = null
    ) : AuthState()
    
    /**
     * Authentication failed
     */
    data class AuthError(
        val message: String,
        val errorCode: AuthErrorCode,
        val canRetry: Boolean = true
    ) : AuthState()
    
    /**
     * Session expired, needs re-authentication
     */
    data class SessionExpired(
        val userId: String,
        val reason: String
    ) : AuthState()
    
    enum class AuthErrorCode {
        INVALID_CREDENTIALS,
        ACCOUNT_LOCKED,
        NETWORK_ERROR,
        SERVER_ERROR,
        UNKNOWN
    }
    
    /**
     * Transition to authenticating
     */
    fun toAuthenticating(email: String): AuthState = Authenticating(email)
    
    /**
     * Transition to authenticated
     */
    fun toAuthenticated(userId: String, token: String): AuthState = 
        Authenticated(userId, token)
    
    /**
     * Transition to error
     */
    fun toError(message: String, errorCode: AuthErrorCode): AuthState =
        AuthError(message, errorCode)
    
    /**
     * Check if can perform action
     */
    fun canAuthenticate(): Boolean {
        return when (this) {
            is Unauthenticated -> true
            is AuthError -> canRetry
            else -> false
        }
    }
    
    /**
     * Get user ID if authenticated
     */
    fun getUserId(): String? = when (this) {
        is Authenticated -> userId
        is SessionExpired -> userId
        else -> null
    }
}

/**
 * Auth state reducer for state machine
 */
class AuthStateReducer {
    private var state: AuthState = AuthState.Unauthenticated
    
    fun currentState(): AuthState = state
    
    fun dispatch(action: AuthAction): AuthState {
        state = reduce(state, action)
        return state
    }
    
    private fun reduce(state: AuthState, action: AuthAction): AuthState {
        return when (action) {
            is AuthAction.Login -> handleLogin(state, action)
            is AuthAction.Logout -> handleLogout(state, action)
            is AuthAction.RefreshToken -> handleRefreshToken(state, action)
            is AuthAction.AuthSuccess -> handleAuthSuccess(state, action)
            is AuthAction.AuthError -> handleAuthError(state, action)
        }
    }
    
    private fun handleLogin(state: AuthState, action: AuthAction.Login): AuthState {
        return AuthState.Authenticating(action.email)
    }
    
    private fun handleLogout(state: AuthState, action: AuthAction.Logout): AuthState {
        return AuthState.Unauthenticated
    }
    
    private fun handleRefreshToken(state: AuthState, action: AuthAction.RefreshToken): AuthState {
        return when (state) {
            is AuthState.Authenticated -> state
            is AuthState.SessionExpired -> state
            else -> state
        }
    }
    
    private fun handleAuthSuccess(
        state: AuthState, 
        action: AuthAction.AuthSuccess
    ): AuthState {
        return AuthState.Authenticated(
            userId = action.userId,
            token = action.token,
            refreshToken = action.refreshToken
        )
    }
    
    private fun handleAuthError(
        state: AuthState, 
        action: AuthAction.AuthError
    ): AuthState {
        return AuthState.AuthError(
            message = action.message,
            errorCode = action.errorCode
        )
    }
}

/**
 * Auth actions for state machine
 */
sealed class AuthAction {
    data class Login(val email: String, val password: String) : AuthAction()
    object Logout : AuthAction()
    data class RefreshToken(val refreshToken: String) : AuthAction()
    data class AuthSuccess(
        val userId: String,
        val token: String,
        val refreshToken: String? = null
    ) : AuthAction()
    data class AuthError(
        val message: String,
        val errorCode: AuthState.AuthErrorCode
    ) : AuthAction()
}

/**
 * UI State sealed class for MVVM
 */
sealed class UiState<out T> {
    /**
     * Initial loading state
     */
    object Initial : UiState<Nothing>()
    
    /**
     * Loading content
     */
    data class Loading(
        val currentData: Any? = null
    ) : UiState<Nothing>()
    
    /**
     * Content loaded successfully
     */
    data class Success<T>(
        val data: T,
        val isRefreshing: Boolean = false
    ) : UiState<T>()
    
    /**
     * Error state with previous data
     */
    data class Error<T>(
        val message: String,
        val throwable: Throwable? = null,
        val previousData: T? = null,
        val canRetry: Boolean = true
    ) : UiState<T>()
    
    /**
     * Empty state
     */
    data class Empty<T>(
        val message: String = "No data available"
    ) : UiState<T>()
    
    companion object {
        fun <T> initial(): UiState<T> = Initial
        fun <T> loading(): UiState<T> = Loading()
        fun <T> success(data: T, isRefreshing: Boolean = false): UiState<T> = Success(data, isRefreshing)
        fun <T> error(
            message: String,
            previousData: T? = null,
            canRetry: Boolean = true
        ): UiState<T> = Error(message, null, previousData, canRetry)
        fun <T> empty(message: String = "No data"): UiState<T> = Empty(message)
    }
    
    /**
     * Get data or null
     */
    fun getData(): T? = when (this) {
        is Success -> data
        is Error -> previousData
        is Empty -> null
        Initial, is Loading -> null
    }
    
    /**
     * Check if can retry
     */
    fun canRetry(): Boolean = when (this) {
        is Error -> canRetry
        else -> false
    }
}

/**
 * Navigation state for app navigation
 */
sealed class NavigationState {
    data class Home(
        val greeting: String = "Welcome!"
    ) : NavigationState()
    
    data class Profile(val userId: String) : NavigationState()
    
    data class Settings(
        val section: SettingsSection = SettingsSection.General
    ) : NavigationState()
    
    sealed class SettingsSection {
        object General : SettingsSection()
        object Account : SettingsSection()
        object Notifications : SettingsSection()
        object Privacy : SettingsSection()
    }
    
    data class ProductDetail(val productId: String) : NavigationState()
    
    data class Cart(
        val itemCount: Int = 0,
        val total: Double = 0.0
    ) : NavigationState()
    
    data class Checkout(
        val step: CheckoutStep = CheckoutStep.SHIPPING
    ) : NavigationState()
    
    enum class CheckoutStep {
        SHIPPING,
        PAYMENT,
        REVIEW,
        CONFIRMATION
    }
    
    object Splash : NavigationState()
    object Login : NavigationState()
    object Register : NavigationState()
    object NotFound : NavigationState()
    
    /**
     * Create back stack
     */
    fun createBackStack(): List<NavigationState> = listOf(Home())
}

/**
 * State machine example
 */
class StateMachineExample {
    private val authReducer = AuthStateReducer()
    
    fun demonstrate() {
        // Initial state
        println("Initial: ${authReducer.currentState()}")
        
        // Dispatch login
        val afterLogin = authReducer.dispatch(
            AuthAction.Login("user@example.com", "password")
        )
        println("After login: ${afterLogin}")
        
        // Dispatch success
        val afterSuccess = authReducer.dispatch(
            AuthAction.AuthSuccess(
                userId = "user123",
                token = "token_abc",
                refreshToken = "refresh_xyz"
            )
        )
        println("After success: ${afterSuccess}")
        
        // Dispatch logout
        val afterLogout = authReducer.dispatch(AuthAction.Logout)
        println("After logout: ${afterLogout}")
        
        // UI state example
        val uiState = UiState.success(listOf("Item 1", "Item 2"))
        println("UI State: $uiState")
    }
}
```

**Output:**
```
Initial: Unauthenticated
After login: Authenticating(user123@..., 0.0)
After success: Authenticated(user123, token_abc, refresh_xyz)
After logout: Unauthenticated
UI State: Success(data=[Item 1, Item 2], isRefreshing=false)
```

This example demonstrates sealed classes for state machine implementation.

### Example 3: Domain Modeling Patterns

```kotlin
/**
 * Permission system using sealed classes
 */
sealed class Permission {
    object ReadUsers : Permission()
    object WriteUsers : Permission()
    object DeleteUsers : Permission()
    object ReadOrders : Permission()
    object WriteOrders : Permission()
    object ReadProducts : Permission()
    object WriteProducts : Permission()
    object ManageSettings : Permission()
    object ViewAnalytics : Permission()
    data class Custom(val name: String) : Permission()
    
    companion object {
        val allPermissions = setOf(
            ReadUsers, WriteUsers, DeleteUsers,
            ReadOrders, WriteOrders,
            ReadProducts, WriteProducts,
            ManageSettings, ViewAnalytics
        )
    }
}

/**
 * Role with permissions
 */
sealed class Role {
    abstract val permissions: Set<Permission>
    abstract val name: String
    
    object Admin : Role() {
        override val name = "Admin"
        override val permissions = Permission.allPermissions
    }
    
    object Manager : Role() {
        override val name = "Manager"
        override val permissions = setOf(
            Permission.ReadUsers, Permission.WriteUsers,
            Permission.ReadOrders, Permission.WriteOrders,
            Permission.ReadProducts, Permission.WriteProducts,
            Permission.ViewAnalytics
        )
    }
    
    object Employee : Role() {
        override val name = "Employee"
        override val permissions = setOf(
            Permission.ReadOrders,
            Permission.ReadProducts
        )
    }
    
    object Guest : Role() {
        override val name = "Guest"
        override val permissions = setOf()
    }
    
    data class CustomRole(
        override val name: String,
        override val permissions: Set<Permission>
    ) : Role()
}

/**
 * Event system using sealed classes
 */
sealed class AppEvent {
    // User events
    data class UserLoggedIn(val userId: String) : AppEvent()
    data class UserLoggedOut(val userId: String) : AppEvent()
    data class UserProfileUpdated(val userId: String) : AppEvent()
    
    // Order events
    data class OrderCreated(val orderId: String) : AppEvent()
    data class OrderUpdated(val orderId: String) : AppEvent()
    data class OrderCancelled(val orderId: String) : AppEvent()
    
    // Navigation events
    data class ScreenViewed(val screenName: String) : AppEvent()
    data class ScreenDismissed(val screenName: String) : AppEvent()
    
    // System events
    object NetworkConnected : AppEvent()
    object NetworkDisconnected : AppEvent()
    data class ErrorOccurred(val error: Throwable) : AppEvent()
    
    /**
     * Get event priority
     */
    fun getPriority(): Priority = when (this) {
        is UserLoggedIn, is UserLoggedOut -> Priority.HIGH
        is OrderCreated, is OrderCancelled -> Priority.HIGH
        is ErrorOccurred -> Priority.CRITICAL
        else -> Priority.NORMAL
    }
    
    enum class Priority {
        CRITICAL, HIGH, NORMAL, LOW
    }
}

/**
 * Command pattern with sealed classes
 */
sealed class Command<T> {
    abstract suspend fun execute(): T
    
    class GetUser(val userId: String) : Command<User?>()
    class SaveUser(val user: User) : Command<Long>()
    class DeleteUser(val userId: String) : Command<Boolean>()
    class GetOrders(val userId: String) : Command<List<Order>>()
    class CreateOrder(val items: List<OrderItem>) : Command<Order>()
    class CancelOrder(val orderId: String) : Command<Boolean>()
}

/**
 * Validation result using sealed classes
 */
sealed class ValidationResult {
    object Valid : ValidationResult()
    
    data class Invalid(val errors: List<ValidationError>) : ValidationResult()
    
    data class ValidationError(
        val field: String,
        val message: String,
        val severity: Severity = Severity.ERROR
    ) {
        enum class Severity {
            WARNING, ERROR
        }
    }
    
    companion object {
        fun valid(): ValidationResult = Valid
        
        fun invalid(errors: List<ValidationError>): ValidationResult = 
            if (errors.isEmpty()) Valid else Invalid(errors)
        
        fun invalid(error: ValidationError): ValidationResult = 
            Invalid(listOf(error))
    }
}

/**
 * Form validation example
 */
class FormValidator {
    fun validate(userForm: UserForm): ValidationResult {
        val errors = mutableListOf<ValidationResult.ValidationError>()
        
        // Validate name
        if (userForm.name.isBlank()) {
            errors.add(
                ValidationResult.ValidationError(
                    field = "name",
                    message = "Name is required"
                )
            )
        } else if (userForm.name.length < 2) {
            errors.add(
                ValidationResult.ValidationError(
                    field = "name",
                    message = "Name must be at least 2 characters"
                )
            )
        }
        
        // Validate email
        if (userForm.email.isBlank()) {
            errors.add(
                ValidationResult.ValidationError(
                    field = "email",
                    message = "Email is required"
                )
            )
        } else if (!userForm.email.contains("@")) {
            errors.add(
                ValidationResult.ValidationError(
                    field = "email",
                    message = "Invalid email format"
                )
            )
        }
        
        // Validate age
        if (userForm.age < 0) {
            errors.add(
                ValidationResult.ValidationError(
                    field = "age",
                    message = "Age cannot be negative"
                )
            )
        } else if (userForm.age < 13) {
            errors.add(
                ValidationResult.ValidationError(
                    field = "age",
                    message = "Must be at least 13 years old",
                    severity = ValidationResult.ValidationError.Severity.WARNING
                )
            )
        }
        
        return ValidationResult.invalid(errors)
    }
}

data class UserForm(
    val name: String,
    val email: String,
    val age: Int
)

data class Order(
    val id: String,
    val userId: String,
    val items: List<OrderItem>,
    val status: OrderStatus
)

data class OrderItem(
    val productId: String,
    val quantity: Int,
    val price: Double
)

enum class OrderStatus {
    PENDING, CONFIRMED, SHIPPED, DELIVERED, CANCELLED
}

/**
 * Exhaustive handling example
 */
class DomainExample {
    fun handleEvent(event: AppEvent) {
        when (event) {
            is AppEvent.UserLoggedIn -> {
                println("User ${event.userId} logged in")
            }
            is AppEvent.UserLoggedOut -> {
                println("User ${event.userId} logged out")
            }
            is AppEvent.UserProfileUpdated -> {
                println("User ${event.userId} profile updated")
            }
            is AppEvent.OrderCreated -> {
                println("Order ${event.orderId} created")
            }
            is AppEvent.OrderUpdated -> {
                println("Order ${event.orderId} updated")
            }
            is AppEvent.OrderCancelled -> {
                println("Order ${event.orderId} cancelled")
            }
            is AppEvent.ScreenViewed -> {
                println("Screen ${event.screenName} viewed")
            }
            is AppEvent.ScreenDismissed -> {
                println("Screen ${event.screenName} dismissed")
            }
            AppEvent.NetworkConnected -> {
                println("Network connected")
            }
            AppEvent.NetworkDisconnected -> {
                println("Network disconnected")
            }
            is AppEvent.ErrorOccurred -> {
                println("Error: ${event.error.message}")
            }
        }
    }
    
    fun checkPermission(role: Role, permission: Permission): Boolean {
        return permission in role.permissions
    }
}

// Usage demonstration
class SealedClassExample {
    fun demonstrate() {
        val userForm = UserForm("John", "john@example.com", 25)
        val validator = FormValidator()
        
        val result = validator.validate(userForm)
        println("Validation result: $result")
        
        val domainExample = DomainExample()
        domainExample.handleEvent(AppEvent.UserLoggedIn("user123"))
        domainExample.handleEvent(AppEvent.ErrorOccurred(Exception("Test error")))
    }
}
```

**Output:**
```
Validation result: Valid
User user123 logged in
Error: Test error
```

## Best Practices

- Use sealed classes for restricted hierarchies where all types are known
- Prefer sealed classes over enums when subtypes need data
- Use object for stateless subclasses
- Use data class for state-carrying subclasses
- Use companion objects for factory methods
- Leverage exhaustive when for comprehensive handling

## Common Pitfalls

### Problem: Forgetting to handle all cases
**Solution:** Let compiler help by using exhaustive when

### Problem: Modifying sealed class externally
**Solution:** Keep all subclasses in same file

### Problem: Generic variance issues
**Solution:** Use @UnsafeVariance or projection types

## Troubleshooting Guide

**Q: When should I use sealed classes?**
A: When you need exhaustive pattern matching or type-safe hierarchies.

**Q: Can sealed classes have abstract members?**
A: Yes, they can have abstract properties and functions.

**Q: How do I add new types to sealed class?**
A: Add new subclass in the same file.

## Cross-References

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Type System and Collections](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/03_Type_System_and_Collections.md)
- [MVI Pattern](../03_ARCHITECTURE/01_Architecture_Patterns/04_MVI_Pattern.md)
- [MVVM Implementation](./02_MVVM_Implementation.md)