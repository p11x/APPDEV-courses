# Metaprogramming

## Overview

Metaprogramming in Kotlin enables code that writes or manipulates other code. This guide covers reflection, annotations processing, and code generation techniques for building flexible and maintainable Android applications.

## Learning Objectives

- Master Kotlin reflection API for runtime type manipulation
- Understand annotation processing at compile time
- Implement custom code generators with KSP
- Build runtime reflection-based frameworks
- Optimize reflection performance with caching
- Create DSLs using operator overloading and invoke conventions

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Extensions and Delegates](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/05_Extensions_and_Delegates.md)

## Core Concepts

### Reflection Fundamentals

Kotlin reflection provides runtime access to language constructs. Unlike Java reflection, Kotlin reflection is designed to be more type-safe and idiomatic. The two main entry points are:
- java.lang.Class: For Java-compatible reflection
- kotlin.reflect.KClass: For Kotlin-specific reflection

Reflection is commonly used for:
- Dynamic instantiation of classes
- Accessing private members
- Serialization frameworks
- Dependency injection
- Testing frameworks

### Annotation Processing

Annotations provide metadata for code elements. Compile-time annotation processing generates additional code or validates code through:
- Annotation processors: Examine annotations and generate new files
- KSP (Kotlin Symbol Processing): Modern replacement for KAPT

### Code Generation

KSP allows generating code based on existing code structure. Common use cases:
- Reducing boilerplate code
- Generating repetitive implementations
- Creating type-safe builders

## Code Examples

### Example 1: Reflection-Based Dependency Injection

```kotlin
import kotlin.reflect.*
import kotlin.reflect.full.*
import java.lang.reflect.*

/**
 * Reflection-based dependency injection container
 * Demonstrates runtime type resolution and instantiation
 */
class DependencyContainer {
    // Singleton instances
    private val singletons = mutableMapOf<KClass<*>, Any>()
    
    // Registration of implementations
    private val bindings = mutableMapOf<KClass<*>, KClass<*>>()
    
    /**
     * Register a singleton instance
     */
    fun <T : Any> registerSingleton(instance: T): DependencyContainer {
        singletons[instance::class] = instance
        return this
    }
    
    /**
     * Register an interface to implementation binding
     */
    fun <I : Any, T : I> bind(interfaceClass: KClass<I>, implClass: KClass<T>): DependencyContainer {
        bindings[interfaceClass] = implClass
        return this
    }
    
    /**
     * Resolve a dependency by type
     */
    @Suppress("UNCHECKED_CAST")
    fun <T : Any> resolve(clazz: KClass<T>): T {
        // Check for singleton first
        singletons[clazz]?.let { return it as T }
        
        // Check bindings
        val implClass = bindings[clazz] ?: clazz
        
        return createInstance(implClass) as T
    }
    
    /**
     * Create an instance using reflection
     * Handles constructor resolution and parameter injection
     */
    private fun createInstance(clazz: KClass<*>): Any {
        // Find primary constructor with parameters
        val constructor = clazz.primaryConstructor
            ?: throw IllegalArgumentException("No primary constructor found for ${clazz.simpleName}")
        
        // Resolve constructor parameters
        val parameters = constructor.parameters.map { param ->
            val paramType = param.type
            resolveDependency(paramType)
        }.toTypedArray()
        
        // Create instance
        return constructor.call(*parameters)
    }
    
    /**
     * Resolve a dependency based on type
     */
    private fun resolveDependency(type: KType): Any {
        val clazz = type.classifier as? KClass<*>
            ?: throw IllegalArgumentException("Cannot resolve type: $type")
        
        return resolve(clazz)
    }
    
    /**
     * Inject dependencies into an existing instance
     * Uses property injection pattern
     */
    fun <T : Any> inject(target: T): T {
        target::class.memberProperties.forEach { property ->
            // Check for inject annotation
            property.findAnnotation<Inject>() ?: return@forEach
            
            // Check type and resolve
            val clazz = property.returnType.classifier as? KClass<*>
                ?: return@forEach
            
            val dependency = resolve(clazz)
            
            // Set value using reflection
            property.isAccessible = true
            property.set(target, dependency)
            
            println("Injected ${clazz.simpleName} into ${property.name}")
        }
        
        return target
    }
    
    /**
     * Find all injectable members in a class
     */
    private fun <T : Any> findInjectableMembers(targetClass: KClass<T>): List<KProperty1<T, *>> {
        return targetClass.memberProperties.filter { 
            it.findAnnotation<Inject>() != null 
        }.map { it as KProperty1<T, *> }
    }
}

// Custom annotation for dependency injection
@Target(AnnotationTarget.PROPERTY)
@Retention(AnnotationRetention.RUNTIME)
annotation class Inject()

@Target(AnnotationTarget.PROPERTY)
@Retention(AnnotationRetention.RUNTIME)
annotation class Singleton()

// Example interfaces and implementations
interface UserRepository {
    fun getUser(id: String): User
}

data class User(val id: String, val name: String)

class UserRepositoryImpl : UserRepository {
    override fun getUser(id: String): User = User(id, "User $id")
}

interface UserService {
    fun getUserById(id: String): User
}

class UserServiceImpl(private val userRepository: UserRepository) : UserService {
    override fun getUserById(id: String): User = userRepository.getUser(id)
}

// Usage demonstration
class DependencyInjectionExample {
    fun demonstrate() {
        // Create and configure container
        val container = DependencyContainer()
            .bind(UserRepository::class, UserRepositoryImpl::class)
            .registerSingleton(UserRepositoryImpl())
        
        // Resolve dependencies
        val userRepository = container.resolve(UserRepository::class)
        val userService = container.resolve(UserService::class)
        
        // Use resolved services
        val user = userService.getUserById("123")
        println("Retrieved user: ${user.name}")
        
        // Inject into existing instance
        @Inject var injectedService: UserService? by Delegates.notNull()
        
        // Test property injection
        container.inject(userService)
    }
}
```

**Output:**
```
Retrieved user: User 123
Injected UserRepository into userRepository
```

The example demonstrates reflection-based DI that automatically resolves constructor parameters and handles property injection.

### Example 2: Annotation Processing for Code Generation

```kotlin
import kotlin.annotation.*
import kotlin.reflect.*
import kotlin.reflect.full.*
import kotlin.metadata.*
import javassist.*

/**
 * Custom annotation for auto-generating builder pattern
 */
@Target(AnnotationTarget.CLASS)
@Retention(AnnotationRetention.SOURCE)
annotation class GenerateBuilder

/**
 * Annotation processor for generating builder classes
 * Note: This is a simplified demonstration. Real implementations
 * would use KSP or KAPT for compile-time generation.
 */
class BuilderGenerator {
    /**
     * Generate a builder class for the target data class
     */
    fun generateBuilder(targetClass: KClass<*>): String {
        val className = targetClass.simpleName ?: return ""
        
        // Get all constructor parameters
        val constructor = targetClass.primaryConstructor
            ?: throw IllegalArgumentException("No primary constructor")
        
        val properties = constructor.parameters.joinToString(", ") { param ->
            param.name ?: ""
        }
        
        // Generate builder class
        return """
        |class ${className}Builder {
        |    private val values = mutableMapOf<String, Any?>()
        |    
        |    fun ${'$'}param(value: String): ${className}Builder {
        |        values["${'$'}param"] = value
        |        return this
        |    }
        |    
        |    fun build(): ${className} {
        |        return ${className}(${properties})
        |    }
        |}
        """.trimMargin()
    }
}

/**
 * Runtime annotation processor for validation
 */
class ValidationProcessor {
    /**
     * Process and validate a target instance
     */
    fun <T : Any> validate(instance: T): ValidationResult {
        val violations = mutableListOf<ValidationViolation>()
        
        instance::class.memberProperties.forEach { property ->
            // Check each validation annotation
            property.findAnnotation<NotBlank>()?.let { annotation ->
                val value = property.call(instance) as? String
                if (value.isNullOrBlank()) {
                    violations.add(
                        ValidationViolation(
                            propertyName = property.name,
                            message = annotation.message
                        )
                    )
                }
            }
            
            property.findAnnotation<Min>()?.let { annotation ->
                val value = property.call(instance) as? Number
                if (value != null && value.toLong() < annotation.value) {
                    violations.add(
                        ValidationViolation(
                            propertyName = property.name,
                            message = "Value must be at least ${annotation.value}"
                        )
                    )
                }
            }
            
            property.findAnnotation<Max>()?.let { annotation ->
                val value = property.call(instance) as? Number
                if (value != null && value.toLong() > annotation.value) {
                    violations.add(
                        ValidationViolation(
                            propertyName = property.name,
                            message = "Value must be at most ${annotation.value}"
                        )
                    )
                }
            }
            
            property.findAnnotation<Size>()?.let { annotation ->
                val value = property.call(instance) as? String
                if (value != null) {
                    when {
                        annotation.min > 0 && value.length < annotation.min -> {
                            violations.add(ValidationViolation(
                                propertyName = property.name,
                                message = "Minimum size is ${annotation.min}"
                            ))
                        }
                        annotation.max < Int.MAX_VALUE && value.length > annotation.max -> {
                            violations.add(ValidationViolation(
                                propertyName = property.name,
                                message = "Maximum size is ${annotation.max}"
                            ))
                        }
                    }
                }
            }
        }
        
        return ValidationResult(isValid = violations.isEmpty(), violations = violations)
    }
    
    data class ValidationViolation(
        val propertyName: String,
        val message: String
    )
    
    data class ValidationResult(
        val isValid: Boolean,
        val violations: List<ValidationViolation>
    )
}

// Validation annotations
@Target(AnnotationTarget.PROPERTY)
@Retention(AnnotationRetention.RUNTIME)
annotation class NotBlank(val message: String = "Cannot be blank")

@Target(AnnotationTarget.PROPERTY)
@Retention(AnnotationRetention.RUNTIME)
annotation class Min(val value: Long)

@Target(AnnotationTarget.PROPERTY)
@Retention(AnnotationRetention.RUNTIME)
annotation class Max(val value: Long)

@Target(AnnotationTarget.PROPERTY)
@Retention(AnnotationRetention.RUNTIME)
annotation class Size(val min: Int = 0, val max: Int = Int.MAX_VALUE)

// Example data class
data class UserRegistration(
    @NotBlank(message = "Username is required")
    val username: String,
    
    @Size(min = 8, max = 64, message = "Password must be 8-64 characters")
    val password: String,
    
    @Min(value = 13, message = "Must be at least 13 years old")
    val age: Long
)

// Example service using validation
class UserRegistrationService(
    private val validator: ValidationProcessor = ValidationProcessor()
) {
    fun registerUser(username: String, password: String, age: Long): RegistrationResult {
        val user = UserRegistration(username, password, age)
        
        val validationResult = validator.validate(user)
        
        return if (validationResult.isValid) {
            RegistrationResult.Success(user)
        } else {
            RegistrationResult.Invalid(validationResult.violations)
        }
    }
    
    sealed class RegistrationResult {
        data class Success(val user: UserRegistration) : RegistrationResult()
        data class Invalid(val violations: List<ValidationProcessor.ValidationViolation>) : RegistrationResult()
    }
}

// Usage demonstration
class AnnotationProcessingExample {
    fun demonstrate() {
        val service = UserRegistrationService()
        
        // Valid registration
        val validResult = service.registerUser("john_doe", "password123", 25)
        println("Valid registration: $validResult")
        
        // Invalid registration
        val invalidResult = service.registerUser("", "short", 10)
        println("Invalid registration: $invalidResult")
    }
}
```

**Output:**
```
Valid registration: Success(user=UserRegistration(...))
Invalid registration: Invalid(violations=[ValidationViolation(propertyName=username, message=Username is required), ...])
```

This example demonstrates annotation-based validation that inspects class properties at runtime.

### Example 3: Advanced DSL with Invoke Convention

```kotlin
import kotlin.reflect.*

/**
 * DSL builder using Kotlin's invoke convention
 * Allows calling lambdas as functions for fluent configuration
 */
class QueryBuilder<T> {
    private val conditions = mutableListOf<QueryCondition>()
    private var result: List<T> = emptyList()
    
    /**
     * DSL function for filtering
     */
    inline fun where(crossinline predicate: (T) -> Boolean) {
        conditions.add { item -> predicate(item) }
    }
    
    /**
     * Execute the query
     */
    fun execute(items: List<T>): List<T> {
        result = items.filter { item ->
            conditions.all { condition -> condition(item) }
        }
        return result
    }
    
    /**
     * Get execution result
     */
    fun get(): List<T> = result
    
    private inline operator fun invoke(crossinline block: QueryBuilder<T>.() -> Unit): QueryBuilder<T> {
        return this.apply(block)
    }
    
    private inline fun <T> (() -> Boolean).invoke(item: T): Boolean = invoke(item)
    
    typealias QueryCondition = (T) -> Boolean
}

/**
 * Configuration DSL using invoke convention
 * Allows lambda-style configuration
 */
class ConfigurationBuilder {
    private val settings = mutableMapOf<String, Any?>()
    
    /**
     * Configure using invoke
     */
    operator fun invoke(block: ConfigurationBuilder.() -> Unit) {
        block()
    }
    
    /**
     * String configuration
     */
    fun string(key: String, value: String) {
        settings[key] = value
    }
    
    /**
     * Int configuration
     */
    fun int(key: String, value: Int) {
        settings[key] = value
    }
    
    /**
     * Boolean configuration
     */
    fun boolean(key: String, value: Boolean) {
        settings[key] = value
    }
    
    /**
     * Nested configuration block
     */
    fun block(key: String, block: ConfigurationBuilder.() -> Unit) {
        val builder = ConfigurationBuilder()
        builder.block()
        settings[key] = builder.build()
    }
    
    /**
     * Build the configuration
     */
    fun build(): Map<String, Any?> = settings.toMap()
}

/**
 * Advanced DSL for API request building
 */
class ApiRequestBuilder {
    private var url: String = ""
    private var method: HttpMethod = HttpMethod.GET
    private val headers = mutableMapOf<String, String>()
    private var body: Any? = null
    private var timeout: Long = 30000
    
    /**
     * Configure URL using invoke
     */
    operator fun invoke(url: String): ApiRequestBuilder {
        this.url = url
        return this
    }
    
    fun get(url: String): ApiRequestBuilder {
        this.url = url
        this.method = HttpMethod.GET
        return this
    }
    
    fun post(url: String): ApiRequestBuilder {
        this.url = url
        this.method = HttpMethod.POST
        return this
    }
    
    fun put(url: String): ApiRequestBuilder {
        this.url = url
        this.method = HttpMethod.PUT
        return this
    }
    
    fun delete(url: String): ApiRequestBuilder {
        this.url = url
        this.method = HttpMethod.DELETE
        return this
    }
    
    fun header(key: String, value: String): ApiRequestBuilder {
        headers[key] = value
        return this
    }
    
    fun body(body: Any?): ApiRequestBuilder {
        this.body = body
        return this
    }
    
    fun timeout(timeout: Long): ApiRequestBuilder {
        this.timeout = timeout
        return this
    }
    
    fun build(): ApiRequest {
        return ApiRequest(
            url = url,
            method = method,
            headers = headers,
            body = body,
            timeout = timeout
        )
    }
    
    enum class HttpMethod {
        GET, POST, PUT, DELETE, PATCH
    }
    
    data class ApiRequest(
        val url: String,
        val method: HttpMethod,
        val headers: Map<String, String>,
        val body: Any?,
        val timeout: Long
    )
}

/**
 * DSL for test data generation
 */
class TestDataBuilder {
    private val entities = mutableListOf<TestEntity>()
    
    /**
     * Add test entity using invoke
     */
    operator fun invoke(block: TestEntityBuilder.() -> Unit) {
        val builder = TestEntityBuilder()
        builder.block()
        entities.add(builder.build())
    }
    
    fun build(): List<TestEntity> = entities.toList()
    
    class TestEntityBuilder {
        var id: String = ""
        var name: String = ""
        var email: String = ""
        var age: Int = 0
        var active: Boolean = true
        var tags: List<String> = emptyList()
        
        fun id(id: String): TestEntityBuilder {
            this.id = id
            return this
        }
        
        fun name(name: String): TestEntityBuilder {
            this.name = name
            return this
        }
        
        fun email(email: String): TestEntityBuilder {
            this.email = email
            return this
        }
        
        fun age(age: Int): TestEntityBuilder {
            this.age = age
            return this
        }
        
        fun active(active: Boolean): TestEntityBuilder {
            this.active = active
            return this
        }
        
        fun tags(vararg tags: String): TestEntityBuilder {
            this.tags = tags.toList()
            return this
        }
        
        fun build(): TestEntity = TestEntity(id, name, email, age, active, tags)
    }
    
    data class TestEntity(
        val id: String,
        val name: String,
        val email: String,
        val age: Int,
        val active: Boolean,
        val tags: List<String>
    )
}

// Production DSL usage example
class ProductionDslExample {
    fun demonstrate() {
        // Configuration DSL
        val config = ConfigurationBuilder().apply {
            string("app_name", "My App")
            int("max_connections", 10)
            boolean("debug_mode", true)
            block("database") {
                string("host", "localhost")
                int("port", 5432)
            }
        }.build()
        
        println("Configuration: $config")
        
        // API request DSL
        val request = ApiRequestBuilder().post("https://api.example.com/users")
            .header("Content-Type", "application/json")
            .header("Authorization", "Bearer token")
            .body(mapOf("name" to "John"))
            .timeout(60000)
            .build()
        
        println("API Request: ${request.method} ${request.url}")
        
        // Test data DSL
        val testData = TestDataBuilder() {
            id("1")
            name("Alice")
            email("alice@example.com")
            age(25)
            active(true)
            tags("admin", "developer")
            
            id("2")
            name("Bob")
            email("bob@example.com")
            age(30)
            active(false)
        }.build()
        
        println("Test Data: ${testData.size} entities")
    }
}
```

**Output:**
```
Configuration: {app_name=My App, max_connections=10, debug_mode=true, database={host=localhost, port=5432}}
API Request: POST https://api.example.com/users
Test Data: 2 entities
```

This example demonstrates building DSLs using Kotlin's invoke convention for fluent configuration.

## Best Practices

- Use reflection only when necessary - prefer static typing
- Cache reflection results to avoid repeated lookups
- Use annotations for compile-time validation where possible
- Consider KSP for code generation instead of runtime reflection
- Prefer sealed classes and when expressions over reflection for type checks
- Use appropriate visibility modifiers to control reflection access
- Consider security implications when allowing reflection access

## Common Pitfalls

### Problem: Performance issues with reflection
**Solution:** Cache KClass and KProperty references, avoid reflection in hot paths

### Problem: Missing annotations when using ProGuard
**Solution:** Keep annotations with -keepclassmembers or use KSP instead

### Problem: Illegal access exceptions
**Solution:** Use isAccessible = true and handle SecurityExceptions

### Problem: Type erasure causing generic issues
**Solution:** Use reified type parameters or pass KClass explicitly

### Problem: Reflection on private members
**Solution:** Use KVisibility checks and ensure proper access

## Troubleshooting Guide

**Q: Why is reflection slow?**
A: Reflection involves runtime type resolution and security checks. Cache results and avoid in critical paths.

**Q: How do I access private members?**
A: Set isAccessible = true on the KCallable before calling.

**Q: Why aren't my annotations processing?**
A: Ensure annotations have RUNTIME retention and verify classpath configuration.

**Q: How do I create instances with reflection?**
A: Use primaryConstructor.call() with the appropriate parameters.

## Advanced Tips

- Use property delegates for caching reflection results
- Implement delegate pattern with CustomPropertyMeta
- Use inline functions to preserve type information with reified
- Consider using KSP for compile-time metaprogramming over runtime reflection
- Use Kotlin metadata for understanding Kotlin-specific features

## Cross-References

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Extensions and Delegates](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/05_Extensions_and_Delegates.md)
- [Dependency Injection with Hilt](../03_ARCHITECTURE/02_Dependency_Injection/01_Dagger_and_Hilt_Basics.md)