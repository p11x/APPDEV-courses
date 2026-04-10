# DI Testing Strategies

## Learning Objectives

1. Testing with DI frameworks
2. Mocking dependencies
3. Testing ViewModels
4. Integration testing with DI

## DI Testing

```kotlin
object DITesting {
    
    // Testing with Koin
    class KoinTest {
        @org.junit.Test
        fun testViewModel() {
            org.koin.core.KoinApplication.init(
                org.koin.core.context.GlobalContext.startKoin {
                    modules(testModule)
                }
            )
            
            val viewModel: MyViewModel by org.koin.androidx.viewmodel.ext.android.viewModel()
            // Test ViewModel
        }
        
        val testModule = org.koin.dsl.module {
            viewModel { MyViewModel(mockRepository()) }
        }
        
        fun mockRepository(): UserRepository = mock()
    }
    
    // Testing with Hilt
    class HiltTest {
        @org.junit.runner.RunWith(org.junit.runners.JUnit4::class)
        class MyTest {
            @org.junit.Test
            @org.hilt.android.testing.HiltAndroidTest
            fun testViewModel(
                @org.hilt.android.testing.HiltAndroidTest
                viewModel: MyViewModel
            ) {
                // Test ViewModel
            }
        }
    }
}
```