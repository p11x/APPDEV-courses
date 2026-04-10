# Koin DI Framework

## Learning Objectives

1. Understanding Koin DI framework
2. Setting up Koin in Android project
3. Creating modules with Koin
4. Using Koin for ViewModels

## Koin Setup

```kotlin
object KoinSetup {
    
    // Dependencies
    fun getDependencies(): String {
        return """
dependencies {
    implementation 'io.insert-koin:koin-android:3.5.0'
    implementation 'io.insert-koin:koin-androidx-navigation:3.5.0'
}
        """.trimIndent()
    }
    
    // Application setup
    class MyApplication : android.app.Application() {
        
        override fun onCreate() {
            super.onCreate()
            
            // Start Koin
            org.koin.core.KoinApplication.init(
                org.koin.core.context.GlobalContext.startKoin {
                    androidLogger()
                    modules(appModule, viewModelModule)
                }
            )
        }
    }
    
    // Module definitions
    val appModule = org.koin.dsl.module {
        // Repository
        single { UserRepository() }
        
        // Data sources
        single { UserLocalDataSource() }
        single { UserRemoteDataSource() }
    }
    
    val viewModelModule = org.koin.dsl.module {
        // ViewModel
        viewModel { UserViewModel(get()) }
    }
    
    // Usage in Activity
    class UserActivity : android.app.Activity() {
        
        private val viewModel: UserViewModel by org.koin.androidx.viewmodel.ext.android.viewModel()
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            // Use viewModel
        }
    }
}
```