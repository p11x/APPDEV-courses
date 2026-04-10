# Dagger and Hilt Basics

## Learning Objectives

1. Understanding Dagger and Hilt basics
2. Setting up Hilt in Android project
3. Creating modules and dependencies
4. Using @Inject and @HiltViewModel

## Section 1: Hilt Setup

```kotlin
object HiltSetup {
    
    // Build.gradle setup
    fun getBuildGradle(): String {
        return """
// Root build.gradle
plugins {
    id 'com.google.dagger.hilt.android' version '2.48.1' apply false
    id 'org.jetbrains.kotlin.kapt' version '1.9.22' apply false
}

// App build.gradle
plugins {
    id 'com.google.dagger.hilt.android'
    id 'org.jetbrains.kotlin.kapt'
}

android {
    ...
}

dependencies {
    implementation 'com.google.dagger:hilt-android:2.48.1'
    kapt 'com.google.dagger:hilt-android-compiler:2.48.1'
}
        """.trimIndent()
    }
    
    // Application class
    @dagger.hilt.android.HiltAndroidApp
    class MyApplication : android.app.Application()
    
    // Module example
    @dagger.Module
    @dagger.hilt.InstallIn(androidx.hilt.lifecycle.ViewModelAssistedComponent::class)
    object ViewModelModule {
        
        @dagger.Provides
        fun provideUserRepository(
            localDataSource: UserLocalDataSource,
            remoteDataSource: UserRemoteDataSource
        ): UserRepository {
            return UserRepositoryImpl(localDataSource, remoteDataSource)
        }
        
        @dagger.Provides
        @dagger.hilt.android.lifecycle.HiltViewModel
        fun provideViewModel(repository: UserRepository): MyViewModel {
            return MyViewModel(repository)
        }
    }
}
```

## Section 2: Dependency Injection

```kotlin
object DependencyInjection {
    
    // Constructor injection
    class UserRepository @dagger.Inject constructor(
        private val localDataSource: UserLocalDataSource,
        private val remoteDataSource: UserRemoteDataSource
    ) {
        suspend fun getUser(id: Int): User? {
            return localDataSource.getUser(id)
        }
    }
    
    // Field injection
    class MyActivity : android.app.Activity() {
        
        @dagger.hilt.android.AndroidEntryPoint
        class Annotated
        
        @javax.inject.Inject
        lateinit var userRepository: UserRepository
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            // Use repository
        }
    }
    
    // ViewModel injection
    @dagger.hilt.android.lifecycle.HiltViewModel
    class MyViewModel @javax.inject.Inject constructor(
        private val repository: UserRepository
    ) : androidx.lifecycle.ViewModel() {
        // ViewModel implementation
    }
    
    // Interface bindings
    interface UserRepository {
        suspend fun getUser(id: Int): User?
    }
    
    @dagger.Binds
    @javax.inject.Singleton
    abstract fun bindUserRepository(
        impl: UserRepositoryImpl
    ): UserRepository
    
    // Qualifiers
    @javax.inject.Qualifier
    @Retention(androidx.annotation.AnnotationRetention.BINARY)
    annotation class IoDispatcher
    
    @dagger.Module
    object DispatcherModule {
        @IoDispatcher
        @dagger.Provides
        fun provideIoDispatcher(): kotlinx.coroutines.CoroutineDispatcher {
            return kotlinx.coroutines.Dispatchers.IO
        }
    }
}
```

## Output Statement Results

Hilt Setup:
- Add Hilt plugin to build.gradle
- Create Application with @HiltAndroidApp
- Add @AndroidEntryPoint to Activities/Fragments

Dependency Injection:
- Constructor injection with @Inject
- Field injection with @Inject
- ViewModel injection with @HiltViewModel
- Module with @Module and @InstallIn
- @Provides for complex dependencies
- @Binds for interface bindings
- @Qualifier for multiple implementations