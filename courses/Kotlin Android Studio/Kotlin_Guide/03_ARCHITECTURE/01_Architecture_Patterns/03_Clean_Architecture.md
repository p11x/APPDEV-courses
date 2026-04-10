# Clean Architecture

## Learning Objectives

1. Understanding Clean Architecture principles
2. Implementing domain, data, and presentation layers
3. Creating use cases and repositories
4. Managing dependencies
5. Testing Clean Architecture

## Section 1: Clean Architecture Overview

Clean Architecture Overview

Clean Architecture organizes code into layers.

```kotlin
object CleanArchitectureOverview {
    
    // Layers
    val layers = listOf(
        "Presentation: UI, ViewModels",
        "Domain: Use Cases, Entities",
        "Data: Repositories, Data Sources"
    )
    
    // Dependencies rule: Inner layers don't depend on outer
    // - Domain layer has no dependencies
    // - Data layer depends on Domain
    // - Presentation depends on Domain
}
```

## Section 2: Domain Layer

Domain Layer

Contains business logic and entities.

```kotlin
object DomainLayer {
    
    // Entities
    data class User(val id: Int, val name: String, val email: String)
    
    // Use Cases
    class GetUserUseCase(private val repository: UserRepository) {
        suspend operator fun invoke(userId: Int): Result<User> {
            return try {
                val user = repository.getUser(userId)
                if (user != null) {
                    Result.success(user)
                } else {
                    Result.failure(NotFoundException("User not found"))
                }
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
    }
    
    class GetUsersUseCase(private val repository: UserRepository) {
        suspend operator fun invoke(): Result<List<User>> {
            return try {
                Result.success(repository.getUsers())
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
    }
    
    class SaveUserUseCase(private val repository: UserRepository) {
        suspend operator fun invoke(user: User): Result<Unit> {
            return try {
                repository.saveUser(user)
                Result.success(Unit)
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
    }
    
    // Repository Interface (in Domain)
    interface UserRepository {
        suspend fun getUser(id: Int): User?
        suspend fun getUsers(): List<User>
        suspend fun saveUser(user: User)
        suspend fun deleteUser(id: Int)
    }
    
    class NotFoundException(message: String) : Exception(message)
}
```

## Section 3: Data Layer

Data Layer

Implements repositories and handles data sources.

```kotlin
object DataLayer {
    
    // Repository Implementation
    class UserRepositoryImpl(
        private val localDataSource: UserLocalDataSource,
        private val remoteDataSource: UserRemoteDataSource
    ) : DomainLayer.UserRepository {
        
        override suspend fun getUser(id: Int): User? {
            return localDataSource.getUser(id) ?: remoteDataSource.getUser(id)
        }
        
        override suspend fun getUsers(): List<User> {
            return try {
                val remoteUsers = remoteDataSource.getUsers()
                localDataSource.saveUsers(remoteUsers)
                remoteUsers
            } catch (e: Exception) {
                localDataSource.getUsers()
            }
        }
        
        override suspend fun saveUser(user: DomainLayer.User) {
            localDataSource.saveUser(user)
            // Sync with remote
            try {
                remoteDataSource.saveUser(user)
            } catch (e: Exception) {
                // Handle sync error
            }
        }
        
        override suspend fun deleteUser(id: Int) {
            localDataSource.deleteUser(id)
            try {
                remoteDataSource.deleteUser(id)
            } catch (e: Exception) {
                // Handle sync error
            }
        }
    }
    
    // Local Data Source
    interface UserLocalDataSource {
        suspend fun getUser(id: Int): DomainLayer.User?
        suspend fun getUsers(): List<DomainLayer.User>
        suspend fun saveUsers(users: List<DomainLayer.User>)
        suspend fun saveUser(user: DomainLayer.User)
        suspend fun deleteUser(id: Int)
    }
    
    class UserLocalDataSourceImpl : UserLocalDataSource {
        private val users = mutableMapOf<Int, DomainLayer.User>()
        
        override suspend fun getUser(id: Int) = users[id]
        
        override suspend fun getUsers() = users.values.toList()
        
        override suspend fun saveUsers(newUsers: List<DomainLayer.User>) {
            users.clear()
            newUsers.forEach { users[it.id] = it }
        }
        
        override suspend fun saveUser(user: DomainLayer.User) {
            users[user.id] = user
        }
        
        override suspend fun deleteUser(id: Int) {
            users.remove(id)
        }
    }
    
    // Remote Data Source
    interface UserRemoteDataSource {
        suspend fun getUser(id: Int): DomainLayer.User?
        suspend fun getUsers(): List<DomainLayer.User>
        suspend fun saveUser(user: DomainLayer.User)
        suspend fun deleteUser(id: Int)
    }
    
    class UserRemoteDataSourceImpl : UserRemoteDataSource {
        // Simulated network calls
        override suspend fun getUser(id: Int): DomainLayer.User? {
            kotlinx.coroutines.delay(500)
            return DomainLayer.User(id, "Remote User", "remote@example.com")
        }
        
        override suspend fun getUsers(): List<DomainLayer.User> {
            kotlinx.coroutines.delay(500)
            return listOf(
                DomainLayer.User(1, "User 1", "user1@example.com"),
                DomainLayer.User(2, "User 2", "user2@example.com")
            )
        }
        
        override suspend fun saveUser(user: DomainLayer.User) {
            kotlinx.coroutines.delay(300)
        }
        
        override suspend fun deleteUser(id: Int) {
            kotlinx.coroutines.delay(300)
        }
    }
}
```

## Section 4: Presentation Layer

Presentation Layer

UI and ViewModels.

```kotlin
object PresentationLayer {
    
    // ViewModel
    class UserListViewModel(
        private val getUsersUseCase: DomainLayer.GetUsersUseCase
    ) : androidx.lifecycle.ViewModel() {
        
        private val _state = androidx.coroutines.flow.MutableStateFlow(UserListState())
        val state: androidx.coroutines.flow.StateFlow<UserListState> = _state
        
        init {
            loadUsers()
        }
        
        fun loadUsers() {
            viewModelScope.launch {
                _state.value = _state.value.copy(isLoading = true)
                
                getUsersUseCase()
                    .onSuccess { users ->
                        _state.value = _state.value.copy(
                            isLoading = false,
                            users = users
                        )
                    }
                    .onFailure { error ->
                        _state.value = _state.value.copy(
                            isLoading = false,
                            error = error.message
                        )
                    }
            }
        }
        
        fun onUserClick(user: DomainLayer.User) {
            _state.value = _state.value.copy(selectedUser = user)
        }
        
        fun clearSelection() {
            _state.value = _state.value.copy(selectedUser = null)
        }
    }
    
    data class UserListState(
        val isLoading: Boolean = false,
        val users: List<DomainLayer.User> = emptyList(),
        val selectedUser: DomainLayer.User? = null,
        val error: String? = null
    )
}
```

## Example: Clean Architecture Implementation

Clean Architecture Implementation Example

```kotlin
class CleanArchitectureImpl {
    
    // DI Module (Hilt-like)
    object DI {
        // Data sources
        val localDataSource = DataLayer.UserLocalDataSourceImpl()
        val remoteDataSource = DataLayer.UserRemoteDataSourceImpl()
        
        // Repository
        val userRepository = DataLayer.UserRepositoryImpl(localDataSource, remoteDataSource)
        
        // Use cases
        val getUsersUseCase = DomainLayer.GetUsersUseCase(userRepository)
        val getUserUseCase = DomainLayer.GetUserUseCase(userRepository)
        val saveUserUseCase = DomainLayer.SaveUserUseCase(userRepository)
        
        // ViewModel
        fun createUserListViewModel() = PresentationLayer.UserListViewModel(getUsersUseCase)
    }
    
    // Usage in Activity
    class UserListActivity : android.app.Activity() {
        
        private val viewModel: PresentationLayer.UserListViewModel by lazy {
            DI.createUserListViewModel()
        }
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            
            viewLifecycleOwner.lifecycleScope.launch {
                viewModel.state.collect { state ->
                    // Update UI based on state
                }
            }
        }
    }
}
```

## Output Statement Results

Clean Architecture Layers:
- Domain: Entities, Use Cases, Repository Interfaces
- Data: Repository Implementation, Data Sources
- Presentation: ViewModels, UI

Dependencies:
- Domain has no external dependencies
- Data depends on Domain interfaces
- Presentation depends on Domain use cases

Benefits:
- Independent of frameworks
- Testable
- Independent of UI
- Independent of database
- Clean separation

## Cross-References

- See: [02_MVVM_Implementation.md](../02_MVVM_Implementation.md)
- See: [01_Dagger_and_Hilt_Basics.md](../../02_Dependency_Injection/01_Dagger_and_Hilt_Basics.md)