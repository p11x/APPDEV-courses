# Room Database Basics

## Learning Objectives

1. Understanding Room database
2. Creating Room entities and DAOs
3. Setting up Room database
4. Using Room with coroutines

## Room Basics

```kotlin
package com.android.data.room

object RoomBasics {
    
    // Entity
    @androidx.room.Entity(tableName = "users")
    data class User(
        @androidx.room.PrimaryKey(autoGenerate = true)
        val id: Long = 0,
        val name: String,
        val email: String,
        val age: Int? = null
    )
    
    // DAO
    @androidx.room.Dao
    interface UserDao {
        
        @androidx.room.Query("SELECT * FROM users")
        suspend fun getAllUsers(): List<User>
        
        @androidx.room.Query("SELECT * FROM users WHERE id = :id")
        suspend fun getUserById(id: Long): User?
        
        @androidx.room.Insert(onConflict = androidx.room.OnConflictStrategy.REPLACE)
        suspend fun insertUser(user: User): Long
        
        @androidx.room.Insert(onConflict = androidx.room.OnConflictStrategy.REPLACE)
        suspend fun insertAll(users: List<User>)
        
        @androidx.room.Update
        suspend fun updateUser(user: User)
        
        @androidx.room.Delete
        suspend fun deleteUser(user: User)
        
        @androidx.room.Query("DELETE FROM users")
        suspend fun deleteAll()
        
        @androidx.room.Query("SELECT * FROM users WHERE name LIKE :query")
        fun searchUsers(query: String): kotlinx.coroutines.flow.Flow<List<User>>
    }
    
    // Database
    @androidx.room.Database(
        entities = [User::class],
        version = 1,
        exportSchema = false
    )
    abstract class AppDatabase : androidx.room.RoomDatabase() {
        
        abstract fun userDao(): UserDao
        
        companion object {
            @Volatile
            private var INSTANCE: AppDatabase? = null
            
            fun getDatabase(context: android.content.Context): AppDatabase {
                return INSTANCE ?: synchronized(this) {
                    val instance = androidx.room.Room.databaseBuilder(
                        context.applicationContext,
                        AppDatabase::class.java,
                        "app_database"
                    )
                        .fallbackToDestructiveMigration()
                        .build()
                    INSTANCE = instance
                    instance
                }
            }
        }
    }
}
```

## Room with Coroutines

```kotlin
object RoomWithCoroutines {
    
    // Repository
    class UserRepository(private val userDao: RoomBasics.UserDao) {
        
        fun getAllUsers(): kotlinx.coroutines.flow.Flow<List<RoomBasics.User>> {
            return userDao.getAllUsers().let { list ->
                kotlinx.coroutines.flow {
                    emit(list)
                }
            }
        }
        
        fun searchUsers(query: String): kotlinx.coroutines.flow.Flow<List<RoomBasics.User>> {
            return userDao.searchUsers("%$query%")
        }
        
        suspend fun insert(user: RoomBasics.User): Long {
            return userDao.insertUser(user)
        }
        
        suspend fun update(user: RoomBasics.User) {
            userDao.updateUser(user)
        }
        
        suspend fun delete(user: RoomBasics.User) {
            userDao.deleteUser(user)
        }
    }
    
    // ViewModel
    class UserViewModel(
        private val repository: UserRepository
    ) : androidx.lifecycle.ViewModel() {
        
        val users: kotlinx.coroutines.flow.StateFlow<List<RoomBasics.User>> = 
            repository.getAllUsers()
                .stateIn(
                    scope = viewModelScope,
                    started = kotlinx.coroutines.flow.WhileSubscribed(5000),
                    initialValue = emptyList()
                )
        
        fun addUser(name: String, email: String) {
            viewModelScope.launch {
                repository.insert(RoomBasics.User(name = name, email = email))
            }
        }
    }
}
```

## Output Statement Results

Room Components:
- Entity: Table definition with @Entity
- DAO: Data access with @Dao
- Database: Container for DAOs
- Repository: Abstraction over DAO

Room Features:
- Auto-generated IDs
- Flow for reactive queries
- Conflict strategies
- Type converters for complex types