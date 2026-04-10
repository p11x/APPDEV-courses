# Manual DI Implementation

## Learning Objectives

1. Understanding manual dependency injection
2. Creating simple DI without frameworks
3. Using service locator pattern
4. Managing dependencies manually

## Manual DI

```kotlin
object ManualDI {
    
    // Simple DI container
    class SimpleContainer {
        private val dependencies = mutableMapOf<String, Any>()
        
        fun <T> register(creator: () -> T) {
            dependencies[T::class.java.name] = creator()
        }
        
        @Suppress("UNCHECKED_CAST")
        fun <T> get(): T {
            return dependencies[T::class.java.name] as T
        }
    }
    
    // Service Locator
    object ServiceLocator {
        private val services = mutableMapOf<Class<*>, Any>()
        
        fun <T : Any> register(service: T) {
            services[service::class.java] = service
        }
        
        inline fun <reified T : Any> resolve(): T {
            return services[T::class.java] as T
        }
    }
}
```