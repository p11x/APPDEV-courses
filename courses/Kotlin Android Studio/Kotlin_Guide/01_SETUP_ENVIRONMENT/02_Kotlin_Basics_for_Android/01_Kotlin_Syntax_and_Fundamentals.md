# Kotlin Syntax and Fundamentals

## Learning Objectives

1. Mastering Kotlin syntax fundamentals
2. Understanding Kotlin type system and null safety
3. Learning control structures and expressions
4. Understanding functions and lambdas
5. Applying Kotlin conventions for Android development

## Section 1: Kotlin Basics and Syntax

Kotlin is a modern, statically-typed language that runs on the JVM. Key features include:
- Null safety
- Extension functions
- Data classes
- Coroutines
- Smart casts

```kotlin
object KotlinBasics {
    const val VERSION = "1.9.22"
    const val COMPILER_VERSION = "1.9.22"
    
    // Variables: val (immutable) and var (mutable)
    fun demonstrateVariables() {
        // Immutable reference
        val immutableValue: Int = 42
        // mutable reference
        var mutableValue: String = "Hello"
        
        // Type inference
        val inferredType = 100  // Int
        val stringValue = "Kotlin"  // String
        
        println("Immutable: $immutableValue")
        println("Mutable: $mutableValue")
    }
    
    // Basic data types
    fun demonstrateDataTypes() {
        // Numbers
        val intValue: Int = 42
        val longValue: Long = 42L
        val doubleValue: Double = 3.14
        val floatValue: Float = 3.14f
        
        // Strings
        val string: String = "Hello"
        val rawString = """
            |Multi-line
            |string with
            |indentation
        """.trimMargin()
        
        // Characters
        val char: Char = 'A'
        
        // Boolean
        val isActive: Boolean = true
        
        // Arrays
        val intArray: IntArray = intArrayOf(1, 2, 3, 4, 5)
        val list: List<String> = listOf("a", "b", "c")
    }
}
```

## Section 2: Control Flow Structures

Kotlin provides various control flow mechanisms.

```kotlin
class ControlFlow {
    
    // If expression (returns value)
    fun ifExpression(x: Int): String {
        val result = if (x > 0) "positive" else "non-positive"
        return result
    }
    
    // When expression (replaces switch)
    fun whenExpression(x: Any): String {
        return when (x) {
            1 -> "one"
            2 -> "two"
            in 3..10 -> "between three and ten"
            is String -> "string: $x"
            else -> "unknown"
        }
    }
    
    // For loop
    fun forLoop() {
        for (i in 1..5) {
            println(i)
        }
        
        // With step
        for (i in 0..10 step 2) {
            println(i)
        }
        
        // DownTo
        for (i in 5 downTo 1) {
            println(i)
        }
        
        // Iterate over collection
        val items = listOf("apple", "banana", "cherry")
        for (item in items) {
            println(item)
        }
    }
    
    // While loops
    fun whileLoop() {
        var x = 5
        while (x > 0) {
            println(x)
            x--
        }
        
        // Do-while
        do {
            println(x)
            x++
        } while (x < 5)
    }
    
    // Range expressions
    fun rangeExpressions() {
        val range = 1..10
        val exclusiveRange = 1 until 10
        val descending = 10 downTo 1
        
        // Check if value in range
        val inRange = 5 in 1..10
        
        // Range with step
        val stepRange = 0..100 step 10
    }
}
```

## Section 3: Functions

Kotlin functions are declared using the 'fun' keyword.

```kotlin
class Functions {
    
    // Basic function
    fun greet(name: String): String {
        return "Hello, $name!"
    }
    
    // Single-expression function
    fun double(x: Int) = x * 2
    
    // Default parameters
    fun greetWithDefault(name: String, greeting: String = "Hello"): String {
        return "$greeting, $name!"
    }
    
    // Named arguments
    fun createUser(
        name: String,
        age: Int,
        email: String
    ): String {
        return "User: $name, Age: $age, Email: $email"
    }
    
    // Unit return (no return value)
    fun printMessage(message: String) {
        println(message)
    }
    
    // Varargs
    fun sum(vararg numbers: Int): Int {
        return numbers.sum()
    }
    
    // Lambda functions
    val square: (Int) -> Int = { x -> x * x }
    
    // Higher-order functions
    fun operate(x: Int, operation: (Int) -> Int): Int {
        return operation(x)
    }
}
```

## Section 4: Null Safety

Kotlin's type system differentiates between nullable and non-nullable types.

```kotlin
class NullSafety {
    
    // Non-nullable type
    var neverNull: String = "Hello"
    
    // Nullable type (note the ?)
    var canBeNull: String? = null
    
    // Safe call operator (?.)
    fun safeCallExample(): Int? {
        val name: String? = "Kotlin"
        return name?.length  // Returns null if name is null
    }
    
    // Elvis operator (?:)
    fun elvisOperator(): Int {
        val name: String? = null
        return name?.length ?: 0  // Returns 0 if name is null
    }
    
    // Not-null assertion (!!) - use with caution
    fun notNullAssertion(): Int {
        val name: String? = "Kotlin"
        return name!!.length  // Throws NPE if null
    }
    
    // Let block with safe call
    fun letExample() {
        val name: String? = "Kotlin"
        name?.let {
            println("Name is not null: $it")
        }
    }
    
    // Smart cast
    fun smartCast(x: Any): Int {
        if (x is String) {
            // x is automatically cast to String
            return x.length
        }
        return 0
    }
    
    // Unsafe cast
    fun unsafeCast(): String {
        val x: Any = "Hello"
        return x as String  // May throw ClassCastException
    }
    
    // Safe cast
    fun safeCast(): String? {
        val x: Any = "Hello"
        return x as? String  // Returns null if cast fails
    }
}
```

## Section 5: Classes and Objects

```kotlin
class ClassesAndObjects {
    
    // Basic class
    class Person(val name: String, var age: Int) {
        fun introduce() = "I'm $name, $age years old"
    }
    
    // Data class (auto-generates equals, hashCode, toString, copy)
    data class User(val id: Int, val name: String, val email: String)
    
    // Object (singleton)
    object DatabaseConfig {
        const val URL = "jdbc:mysql://localhost:3306/db"
        const val MAX_CONNECTIONS = 10
    }
    
    // Companion object (like static members)
    class MyClass {
        companion object {
            const val TAG = "MyClass"
            fun create() = MyClass()
        }
    }
    
    // Sealed class (restricted inheritance)
    sealed class Result {
        data class Success(val data: String) : Result()
        data class Error(val message: String) : Result()
        object Loading : Result()
    }
    
    // Enum class
    enum class Priority(val value: Int) {
        LOW(1),
        MEDIUM(2),
        HIGH(3)
    }
}
```

## Common Pitfalls and Solutions

**Pitfall 1: NullPointerException in Java interop**
- Use ? for nullable types
- Use safe calls (?.)
- Use elvis operator (?:)

**Pitfall 2: Unchecked casts in generics**
- Use reified keyword where possible
- Add runtime type checks

**Pitfall 3: Mutable shared state**
- Use immutable data structures
- Prefer val over var
- Use pure functions

**Pitfall 4: Confusing Int? and Int**
- Always check nullability
- Use safe operators

**Pitfall 5: Lambda capture issues**
- Be aware of closure behavior
- Use explicit return values

## Best Practices

1. Use val instead of var when possible
2. Prefer expressions over statements
3. Use data classes for POJOs
4. Avoid null when possible
5. Use sealed classes for state
6. Follow naming conventions
7. Use companion objects for static-like members
8. Leverage type inference appropriately
9. Use extension functions for utilities
10. Prefer immutability

## Troubleshooting Guide

**Issue: Type mismatch in assignment**
1. Check type compatibility
2. Use explicit type conversion
3. Check nullable types

**Issue: Cannot access member on nullable type**
1. Use safe call (?.)
2. Use let block
3. Check for null

**Issue: Unresolved reference**
1. Check import statements
2. Verify class exists
3. Check package declaration

## Advanced Tips and Tricks

**Tip 1: Use inline functions**
- Reduces overhead for lambdas
- Enables reified type parameters

**Tip 2: Use type aliases**
- Improve code readability
- Create domain-specific types

**Tip 3: Use destructuring declarations**
- Extract multiple values
- For data classes

**Tip 4: Use labeled returns**
- Control nested lambda returns
- Avoid return in lambdas

**Tip 5: Use scope functions**
- apply, run, let, with, also

## Example 1: Android Activity with Kotlin

```kotlin
class MainActivity {
    // State variables using Kotlin properties
    private var counter: Int = 0
    private val userName: String? = null
    
    // Lifecycle methods
    fun onCreate() {
        println("Activity created")
        
        // Safe call with let
        userName?.let { name ->
            println("User: $name")
        }
        
        // Elvis operator for defaults
        val displayName = userName ?: "Guest"
        println("Display: $displayName")
    }
    
    fun onClick() {
        counter++
        
        // Expression body function
        val message = if (counter > 5) "High score!" else "Keep going"
        
        // Lambda event handler
        val onCounterChange: (Int) -> Unit = { newValue ->
            println("Counter changed to: $newValue")
        }
        
        onCounterChange(counter)
    }
    
    // Data class for user
    data class User(val id: Int, val name: String, val email: String)
}
```

## Example 2: Function Composition and Lambda Patterns

```kotlin
class FunctionComposition {
    
    // Function type
    typealias Transformation = (Int) -> Int
    
    // Compose functions
    fun <A, B, C> compose(f: (B) -> C, g: (A) -> B): (A) -> C {
        return { x -> f(g(x)) }
    }
    
    // Pipeline example
    fun processPipeline(value: Int): String {
        val transformations = listOf(
            { x: Int -> x + 1 },
            { x: Int -> x * 2 },
            { x: Int -> "Result: $x" }
        )
        
        return transformations.fold(value) { acc, fn -> fn(acc) }
    }
    
    // Currying
    fun curriedAdd(x: Int): (Int) -> Int = { y -> x + y }
    
    // Partial application
    fun partiallyApplied(): Int {
        val addFive = curriedAdd(5)
        return addFive(10)  // Returns 15
    }
    
    // Extension function as pipeline
    fun Int.pipe(transformation: (Int) -> Int): Int {
        return transformation(this)
    }
    
    fun usePipe(): Int {
        return 10.pipe { it + 5 }.pipe { it * 2 }  // Returns 30
    }
}
```

## Example 3: Seamless Java Interop

Kotlin integrates seamlessly with Java code.

```kotlin
class JavaInterop {
    
    // Calling Java from Kotlin
    // Java method: public void setName(String name)
    fun callJavaCode() {
        val javaObject = JavaClass()
        javaObject.setName("Kotlin")
        val name = javaObject.name
    }
    
    // Kotlin null safety with Java
    // Java can return null, so Kotlin treats as nullable
    fun handleJavaNull(javaValue: String?): String {
        return javaValue ?: "Default value"
    }
    
    // SAM conversions
    // Java interface with single abstract method
    fun setClickListener(listener: OnClickListener) {
        listener.onClick()
    }
    
    // Kotlin lambda to Java interface
    fun setClickListenerWithLambda() {
        setClickListener { println("Clicked!") }
    }
    
    // Extension functions for Java classes
    fun String.toTitleCase(): String {
        return this.split(" ").joinToString(" ") { 
            it.replaceFirstChar { char -> char.uppercase() }
        }
    }
    
    // @JvmOverloads for default parameters
    @JvmOverloads
    fun greetWithDefault(name: String, greeting: String = "Hello"): String {
        return "$greeting, $name!"
    }
    
    // @JvmStatic for companion object methods
    companion object {
        @JvmStatic
        fun staticMethod() = "Static method"
        
        @JvmField
        val staticField = "Static field"
    }
}

// Hypothetical Java class for reference
abstract class OnClickListener {
    abstract fun onClick()
}

class JavaClass {
    var name: String = ""
    fun setName(name: String) { this.name = name }
}
```

## Output Statement Results

Kotlin Basics Demonstrated:
- Variables: val and var
- Data Types: Int, Long, Double, Float, String, Char, Boolean
- Control Flow: if, when, for, while, ranges
- Functions: basic, default params, varargs, lambdas
- Null Safety: ?, !!, ?., ?:
- Classes: regular, data, object, companion, sealed, enum

Best Practices Applied:
- val over var
- Nullable type safety
- Expression bodies
- Data classes for POJOs
- Extension functions
- Lambda expressions

Android Usage:
- Activity lifecycle with Kotlin
- Safe null handling
- Lambda event handlers
- Data class for models

## Cross-References

See: 02_Kotlin_Basics_for_Android/02_Android_Kotlin_Conventions.md
See: 02_Kotlin_Basics_for_Android/03_Type_System_and_Collections.md
See: 02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md
See: 02_Kotlin_Basics_for_Android/05_Extensions_and_Delegates.md
