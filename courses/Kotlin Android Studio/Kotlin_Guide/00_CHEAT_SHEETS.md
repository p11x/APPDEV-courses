# Cheat Sheets

Quick reference guide for essential Kotlin Android development commands, patterns, and syntax.

## Table of Contents

1. [Kotlin Syntax Quick Reference](#kotlin-syntax-quick-reference)
2. [Common Android Patterns](#common-android-patterns)
3. [Gradle Commands](#gradle-commands)
4. [Git Commands](#git-commands)
5. [Debugging Commands](#debugging-commands)

---

## Kotlin Syntax Quick Reference

### Variables and Types

```kotlin
// Variable declaration
val immutable = "value"          // Read-only (like final)
var mutable = "value"              // Mutable

// Type inference
val string = "hello"              // String
val number = 42                  // Int
val decimal = 3.14              // Double
val flag = true                  // Boolean

// Explicit types
val list: List<String> = listOf("a", "b")
val map: Map<String, Int> = mapOf("a" to 1)
```

###Null Safety

```kotlin
// Null safety
val name: String? = null         // Nullable
val name: String = "value"       // Non-null

// Safe calls
val length = name?.length        // Returns null if name is null

// Elvis operator
val len = name?.length ?: 0     // Default 0 if null

// Not-null assertion (avoid if possible)
val len = name!!.length         // Throws if null

// Safe cast
val item = value as? String      // Returns null if cast fails
```

### Functions

```kotlin
// Basic function
fun greet(name: String): String {
    return "Hello, $name"
}

// Single expression
fun greet(name: String) = "Hello, $name"

// Default parameters
fun greet(name: String, greeting: String = "Hello"): String {
    return "$greeting, $name"
}

// Named arguments
greet(greeting = "Hi", name = "World")

// Vararg
fun printAll(vararg words: String) {
    words.forEach { println(it) }
}

// Lambda
val doubled = list.map { it * 2 }
```

### Collections

```kotlin
// List operations
val list = listOf(1, 2, 3)
val mutableList = mutableListOf(1, 2, 3)
val item = list[0]              // Get first item
val first = list.first()       // First item
val last = list.last()         // Last item
val filtered = list.filter { it > 2 }
val mapped = list.map { it * 2 }

// Map operations
val map = mapOf("a" to 1, "b" to 2)
val value = map["a"]
val keys = map.keys
val values = map.values

// Sequence operations
list
    .filter { it > 1 }
    .map { it * 2 }
    .take(3)
    .forEach { println(it) }
```

### Classes

```kotlin
// Data class
data class User(
    val id: Long,
    val name: String,
    val email: String
)

// Sealed class
sealed class Result<out T> {
    data class Success<T>(val data: T) : Result<T>()
    data class Error(val message: String) : Result<Nothing>()
    object Loading : Result<Nothing>()
}

// Object (singleton)
object AppConfig {
    const val API_URL = "https://api.example.com"
}

// Companion object
class MyClass {
    companion object {
        const val TAG = "MyClass"
    }
}
```

### Coroutines Basics

```kotlin
// Launch coroutine
viewModelScope.launch {
    val data = repository.getData()
}

// Async coroutine
suspend fun getData(): Data {
    return withContext(Dispatchers.IO) {
        api.getData()
    }
}

// Flow collection
viewModelScope.launch {
    repository.getDataFlow()
        .collect { data ->
            // Handle data
        }
}

// Flow operators
flow
    .map { it.name }
    .filter { it.isNotEmpty() }
    .first()
```

### Extension Functions

```kotlin
// Extension function
fun String.capitalize(): String {
    return this.replaceFirstChar { it.uppercase() }
}

// Extension property
val String.wordCount: Int
    get() = this.split(" ").size

// Usage
"hello world".capitalize()
"hello world".wordCount
```

---

## Common Android Patterns

### Activity/Fragment Setup

```kotlin
// Activity
class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
    }
}

// Fragment
class HomeFragment : Fragment() {
    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        return inflater.inflate(R.layout.fragment_home, container, false)
    }
}
```

### ViewModel Creation

```kotlin
// ViewModel
class MyViewModel : ViewModel() {
    private val _state = MutableStateFlow(MyState())
    val state: StateFlow<MyState> = _state
}

// Hilt ViewModel
@HiltViewModel
class MyViewModel @Inject constructor(
    private val useCase: MyUseCase
) : ViewModel() {
    private val _state = MutableStateFlow(MyState())
    val state: StateFlow<MyState> = _state
}
```

### Repository Pattern

```kotlin
interface UserRepository {
    fun getUsers(): Flow<List<User>>
    suspend fun getUser(id: Long): User?
}

class UserRepositoryImpl(
    private val api: UserApi,
    private val dao: UserDao
) : UserRepository {
    
    override fun getUsers(): Flow<List<User>> = flow {
        // Try API first
        try {
            val users = api.getUsers()
            dao.insert(users)
            emit(users)
        } catch (e: Exception) {
            // Fallback to cache
            emit(dao.getAll())
        }
    }
}
```

### Use Case Pattern

```kotlin
class GetUsersUseCase(
    private val repository: UserRepository
) {
    operator fun invoke(): Flow<List<User>> = repository.getUsers()
}
```

### DI Module

```kotlin
@Module
@InstallIn(SingletonComponent::class)
object NetworkModule {
    @Provides
    @Singleton
    fun provideOkHttpClient(): OkHttpClient {
        return OkHttpClient.Builder()
            .build()
    }
    
    @Provides
    @Singleton
    fun provideRetrofit(okHttpClient: OkHttpClient): Retrofit {
        return Retrofit.Builder()
            .client(okHttpClient)
            .baseUrl("https://api.example.com/")
            .build()
    }
}
```

### Compose Basic Components

```kotlin
@Composable
fun MyScreen() {
    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Text(
            text = "Hello",
            style = MaterialTheme.typography.headlineMedium
        )
        
        Spacer(modifier = Modifier.height(16.dp))
        
        Button(onClick = { /* Handle click */ }) {
            Text("Click Me")
        }
    }
}
```

### Navigation

```kotlin
// Navigation Compose
@Composable
fun NavHost() {
    NavHost(
        navController = navController,
        startDestination = "home"
    ) {
        composable("home") {
            HomeScreen()
        }
        
        composable("detail/{id}") { backStackEntry ->
            val id = backStackEntry.arguments?.getString("id")
            DetailScreen(id = id!!)
        }
    }
}
```

### Room Entity and DAO

```kotlin
@Entity(tableName = "users")
data class User(
    @PrimaryKey
    val id: Long,
    val name: String,
    val email: String
)

@Dao
interface UserDao {
    @Query("SELECT * FROM users")
    fun getAll(): Flow<List<User>>
    
    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun insert(user: User)
    
    @Delete
    suspend fun delete(user: User)
}
```

### HTTP with Retrofit

```kotlin
interface ApiService {
    @GET("users")
    suspend fun getUsers(): List<User>
    
    @POST("users")
    suspend fun createUser(@Body user: User): User
}
```

---

## Gradle Commands

### Common Commands

```bash
# Build debug APK
./gradlew assembleDebug

# Build release APK
./gradlew assembleRelease

# Run unit tests
./gradlew test

# Run instrumented tests
./gradlew connectedAndroidTest

# Clean build
./gradlew clean

# Build with specific task
./gradlew build

# Dependencies
./gradlew dependencies

# Dependency updates
./gradlew dependencyUpdates

# Run lint
./gradlew lint

# Stop all daemons
./gradlew --stop
```

### Gradle Wrapper Commands

```bash
# Upgrade Gradle wrapper
./gradlew wrapper --gradle-version=8.4

# Check Gradle version
./gradlew -v
```

### Kotlin/Sync Commands

```bash
# Generate Kotlin code
./gradlew kapt

# Sync project
./gradlew sync
```

### Memory and Performance

```bash
# Set JVM args
GRADLE_OPTS="-Xmx2048m" ./gradlew build

# Daemon
./gradlew --no-daemon build
```

### Troubleshooting

```bash
# Debug info
./gradlew tasks --debug

# Dry run
./gradlew assembleDebug --dry-run

# Verbose output
./gradlew assembleDebug --info

# Stacktrace
./gradlew assembleDebug --stacktrace
```

---

## Git Commands

### Basic Commands

```bash
# Initialize repository
git init

# Clone repository
git clone https://github.com/user/repo.git

# Add files
git add .
git add filename.kt

# Commit
git commit -m "Commit message"

# Push
git push origin main

# Pull
git pull origin main

# Status
git status
```

### Branching

```bash
# Create branch
git checkout -b feature/new-feature

# Switch branch
git checkout main

# List branches
git branch -a

# Delete branch
git branch -d feature/old-feature

# Force delete
git branch -D feature/old-feature
```

### Merging and Rebasing

```bash
# Merge branch
git checkout main
git merge feature/new-feature

# Rebase
git checkout feature/new-feature
git rebase main

# Interactive rebase
git rebase -i HEAD~3
```

### Stashing

```bash
# Save changes
git stash

# List stashes
git stash list

# Apply stash
git stash apply

# Apply and remove
git stash pop
```

### History

```bash
# View log
git log

# View log with changes
git log -p

# View single commit
git show abc123

# Diff
git diff
git diff main..feature
```

### Remote

```bash
# Add remote
git remote add origin https://github.com/user/repo.git

# View remotes
git remote -v

# Fetch
git fetch origin

# Remove remote
git remote remove origin
```

### Undo Changes

```bash
# Discard changes
git checkout -- filename.kt
git restore filename.kt

# Unstage
git reset HEAD filename.kt

# Undo commit (keep changes)
git reset --soft HEAD~1

# Undo commit (discard changes)
git reset --hard HEAD~1
```

### Tags

```bash
# Create tag
git tag -a v1.0.0 -m "Release 1.0.0"

# Push tag
git push origin v1.0.0

# List tags
git tag -l
```

---

## Debugging Commands

### ADB Commands

```bash
# List devices
adb devices

# Install app
adb install app.apk

# Uninstall
adb uninstall com.example.app

# Start app
adb shell am start -n com.example.app/.MainActivity

# Stop app
adb shell am force-stop com.example.app

# Clear data
adb shell pm clear com.example.app

# Grant permission
adb shell pm grant com.example.app android.permission.INTERNET

# View logs
adb logcat

# Filter logs
adb logcat | grep "TAG"

# Clear logs
adb logcat -c

# Copy file to device
adb push localfile.txt /sdcard/file.txt

# Copy file from device
adb pull /sdcard/file.txt localfile.txt
```

### Log Commands

```kotlin
// Log.d - Debug
Log.d("TAG", "Debug message")

// Log.i - Info
Log.i("TAG", "Info message")

// Log.w - Warning
Log.w("TAG", "Warning message")

// Log.e - Error
Log.e("TAG", "Error message", exception)
```

### Profiler Commands

```bash
# Start CPU profiler
adb shell am profile start com.example.app /data/local/tmp/profile.perf

# Stop CPU profiler
adb shell am profile stop com.example.app

# Memory dump
adb shell am dumpheap com.example.app /data/local/tmp/heap.hprof
```

### Troubleshooting Common Issues

```bash
# Check Gradle cache
rm -rf ~/.gradle/caches

# Clean .gradle folder
rm -rf .gradle

# Invalidate caches (Android Studio)
# File > Invalidate Caches > Invalidate and Restart

# Clear build folder
rm -rf app/build

# Rebuild
./gradlew clean assembleDebug
```

### Keyboard Shortcuts (Android Studio)

| Action | Windows/Linux | Mac |
|-------|---------------|-----|
| Quick fix | Alt + Enter | Option + Enter |
| Find action | Ctrl + Shift + A | Cmd + Shift + A |
| Find class | N | Cmd + O |
| Find file | Shift + N | Cmd + Shift + O |
| Recent files | Ctrl + E | Cmd + E |
| Go to line | Ctrl + G | Cmd + L |
| Rename | Shift + F6 | Shift + F6 |
| Format code | Ctrl + Alt + L | Cmd + Option + L |

---

## Summary

Quick Reference Table:

| Task | Command |
|------|---------|
| Build debug | `./gradlew assembleDebug` |
| Build release | `./gradlew assembleRelease` |
| Run tests | `./gradlew test` |
| Install APK | `adb install app.apk` |
| View logs | `adb logcat` |
| Commit | `git add . && git commit -m ""` |
| Push | `git push origin main` |

---

**Related Files:**
- [00_INTEGRATION_GUIDE.md](00_INTEGRATION_GUIDE.md)
- [00_MIGRATION_GUIDES.md](00_MIGRATION_GUIDES.md)
- [01_SETUP_ENVIRONMENT/](01_SETUP_ENVIRONMENT/)
- [02_UI_DEVELOPMENT/](02_UI_DEVELOPMENT/)
- [03_ARCHITECTURE/](03_ARCHITECTURE/)