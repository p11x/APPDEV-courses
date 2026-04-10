# Type System and Collections

## Learning Objectives

1. Understanding Kotlin type system and type safety
2. Working with collection types and operations
3. Understanding generics and variance
4. Implementing type-safe patterns
5. Optimizing collection operations

```kotlin
package com.kotlin.types
```

## Section 1: Kotlin Type System

Kotlin Type System - Kotlin has a rich type system with:
- Primitive types (as objects)
- Reference types
- Nullable types
- Type inference
- Smart casts

```kotlin
object TypeSystem {
    
    // Basic types
    val int: Int = 42
    val long: Long = 42L
    val double: Double = 3.14
    val float: Float = 3.14f
    val boolean: Boolean = true
    val char: Char = 'A'
    val string: String = "Hello"
    
    // Arrays
    val intArray: IntArray = intArrayOf(1, 2, 3)
    val arrayOf: Array<String> = arrayOf("a", "b", "c")
    
    // Nullable types
    var nullable: String? = null
    var nonNull: String = "Hello"
    
    // Type checking
    fun checkType(value: Any): String {
        return when (value) {
            is Int -> "Integer: $value"
            is String -> "String with length: ${value.length}"
            is List<*> -> "List with ${value.size} elements"
            else -> "Unknown type"
        }
    }
    
    // Smart cast
    fun smartCastExample(value: Any): Int {
        if (value is String) {
            // value is automatically String here
            return value.length
        }
        return 0
    }
    
    // Explicit casting
    fun explicitCast() {
        val any: Any = "Hello"
        val str: String = any as String  // May throw
        val safeStr: String? = any as? String  // Safe, returns null
    }
}
```

## Section 2: Collection Types

Collection Types in Kotlin - Kotlin provides two types of collections:
- Immutable (read-only): List, Set, Map
- Mutable: MutableList, MutableSet, MutableMap

```kotlin
class CollectionTypes {
    
    // Immutable List
    fun immutableList(): List<String> {
        val list = listOf("apple", "banana", "cherry")
        return list
    }
    
    // Mutable List
    fun mutableList(): MutableList<String> {
        val mutable = mutableListOf("apple", "banana")
        mutable.add("cherry")
        mutable.remove("apple")
        mutable[0] = "mango"
        return mutable
    }
    
    // Immutable Set
    fun immutableSet(): Set<Int> {
        val set = setOf(1, 2, 3, 2, 1)  // [1, 2, 3]
        return set
    }
    
    // Mutable Set
    fun mutableSet(): MutableSet<Int> {
        val mutable = mutableSetOf(1, 2, 3)
        mutable.add(4)
        mutable.remove(1)
        return mutable
    }
    
    // Immutable Map
    fun immutableMap(): Map<String, Int> {
        val map = mapOf(
            "apple" to 1,
            "banana" to 2,
            "cherry" to 3
        )
        return map
    }
    
    // Mutable Map
    fun mutableMap(): MutableMap<String, Int> {
        val mutable = mutableMapOf("a" to 1)
        mutable["b"] = 2
        mutable.remove("a")
        return mutable
    }
    
    // Array types
    fun arrayTypes() {
        val array = arrayOf(1, 2, 3)  // Array<Int>
        val intArray = intArrayOf(1, 2, 3)  // IntArray
        val longArray = longArrayOf(1, 2, 3)  // LongArray
    }
}
```

## Section 3: Collection Operations

Collection Operations - Kotlin provides rich operations on collections.

```kotlin
class CollectionOperations {
    
    // Transforming
    fun transform() {
        val numbers = listOf(1, 2, 3, 4, 5)
        
        val doubled = numbers.map { it * 2 }  // [2, 4, 6, 8, 10]
        val squared = numbers.map { it * it }  // [1, 4, 9, 16, 25]
        
        val flat = listOf(listOf(1, 2), listOf(3, 4))
        val flattened = flat.flatten()  // [1, 2, 3, 4]
        val flatMapped = flat.flatMap { it }  // Same
    }
    
    // Filtering
    fun filter() {
        val numbers = listOf(1, 2, 3, 4, 5)
        
        val even = numbers.filter { it % 2 == 0 }  // [2, 4]
        val odd = numbers.filterNot { it % 2 == 0 }  // [1, 3, 5]
        val first = numbers.first { it > 2 }  // 3
        val last = numbers.last { it < 5 }  // 4
        val firstOrNull = numbers.firstOrNull { it > 10 }  // null
        
        val partitioned = numbers.partition { it > 2 }
        // partitioned.first: [3, 4, 5]
        // partitioned.second: [1, 2]
    }
    
    // Sorting
    fun sorting() {
        val strings = listOf("banana", "apple", "cherry")
        
        val sorted = strings.sorted()  // [apple, banana, cherry]
        val sortedDesc = strings.sortedDescending()  // [cherry, banana, apple]
        
        val numbers = listOf(3, 1, 4, 1, 5, 9, 2, 6)
        val sortedNums = numbers.sortedBy { it }  // ascending
        val sortedByLength = strings.sortedBy { it.length }  // by length
    }
    
    // Aggregating
    fun aggregate() {
        val numbers = listOf(1, 2, 3, 4, 5)
        
        val sum = numbers.sum()  // 15
        val average = numbers.average()  // 3.0
        val count = numbers.count()  // 5
        val countEven = numbers.count { it % 2 == 0 }  // 2
        
        val min = numbers.minOrNull()  // 1
        val max = numbers.maxOrNull()  // 5
        val minBy = numbers.minByOrNull { -it }  // 5
        
        val reduced = numbers.reduce { acc, i -> acc + i }  // 15
        val folded = numbers.fold(10) { acc, i -> acc + i }  // 25
    }
    
    // Grouping
    fun grouping() {
        val strings = listOf("a", "ab", "abc", "b", "bc")
        
        val grouped = strings.groupBy { it.first() }
        // {a=[a, ab, abc], b=[b, bc]}
        
        val groupedByLength = strings.groupBy { it.length }
        // {1=[a, b], 2=[ab, bc], 3=[abc]}
    }
    
    // Element operations
    fun elementOps() {
        val list = listOf(1, 2, 3, 4, 5)
        
        val first = list.first()  // 1
        val last = list.last()  // 5
        val contains = list.contains(3)  // true
        val containsAll = list.containsAll(listOf(1, 2))  // true
        val isEmpty = list.isEmpty()  // false
        val indexOf = list.indexOf(3)  // 2
        val distinct = list.distinct()  // [1, 2, 3, 4, 5]
    }
}
```

## Section 4: Generics

Generics in Kotlin - Kotlin supports generic types with variance modifiers.

```kotlin
class Generics {
    
    // Generic class
    class Box<T>(val value: T) {
        fun get(): T = value
    }
    
    // Generic function
    fun <T> identity(value: T): T = value
    
    // Multiple type parameters
    class Pair<K, V>(val key: K, val value: V)
    
    // Generic constraints
    fun <T : Comparable<T>> maxOf(a: T, b: T): T {
        return if (a > b) a else b
    }
    
    // Multiple constraints
    fun <T> sortWith(
        list: List<T>,
        comparator: Comparator<T>,
        transformation: (T) -> Comparable<*>
    ): List<T> {
        return list.sortedBy { transformation(it) }
    }
    
    // Variance - Producer (out)
    interface Producer<out T> {
        fun produce(): T
    }
    
    // Variance - Consumer (in)
    interface Consumer<in T> {
        fun consume(item: T)
    }
    
    // Invariant
    class Storage<T> {
        private var item: T? = null
        fun set(item: T) { this.item = item }
        fun get(): T? = item
    }
    
    // Type projections
    fun processList(list: List<out Any>) {
        for (item in list) {
            println(item)
        }
    }
    
    // Reified type (inline functions only)
    inline fun <reified T> getType(): String {
        return T::class.java.name
    }
}
```

## Section 5: Type-Safe Patterns

Type-Safe Patterns - Implementing type safety in Kotlin.

```kotlin
object TypeSafePatterns {
    
    // Nullable types
    fun safeNullable() {
        var nullable: String? = null
        
        // Safe call
        val length = nullable?.length
        
        // Elvis operator
        val len = nullable?.length ?: 0
        
        // Not null assertion
        // val len2 = nullable!!.length  // Throws if null
    }
    
    // Sealed classes for type-safe results
    sealed class Result<out T> {
        data class Success<T>(val data: T) : Result<T>()
        data class Error(val exception: Throwable) : Result<Nothing>()
        object Loading : Result<Nothing>()
    }
    
    fun handleResult(result: Result<String>) {
        when (result) {
            is Result.Success -> println(result.data)
            is Result.Error -> println(result.exception.message)
            is Result.Loading -> println("Loading...")
        }
    }
    
    // Type-safe builders
    class HtmlBuilder {
        private val elements = mutableListOf<String>()
        
        fun element(name: String, block: () -> String) {
            elements.add("<$name>${block()}</$name>")
        }
        
        fun build(): String = elements.joinToString("\n")
    }
    
    // Extension functions for type safety
    fun String?.orEmpty(): String = this ?: ""
    
    fun <T> List<T>.firstOrError(): T {
        return firstOrNull() ?: throw NoSuchElementException("List is empty")
    }
    
    // Type aliases
    typealias UserId = String
    typealias UserMap = Map<UserId, String>
    
    fun processUserMap(map: UserMap) {
        map["user1"]?.let { println("User: $it") }
    }
}
```

## Common Pitfalls and Solutions

Pitfall 1: Mutable collection passed as immutable
Solution:
- Use listOf() not mutableListOf() for immutability
- Be careful with type inference
- Clone before exposing

Pitfall 2: Type inference with null
Solution:
- Explicitly specify type
- Use nullable types (?)
- Handle null cases

Pitfall 3: Generic type erasure
Solution:
- Use reified for inline functions
- Pass Class<T> as parameter
- Use TypeToken

Pitfall 4: Covariance/Contravariance confusion
Solution:
- Use out for producer types
- Use in for consumer types
- Avoid * (star projection) when possible

## Best Practices

1. Use immutable collections when possible
2. Prefer functional operations over loops
3. Use appropriate collection type for needs
4. Leverage type inference appropriately
5. Use sealed classes for type-safe results
6. Avoid raw types
7. Use generics for reusability
8. Consider variance for API design
9. Use type aliases for clarity
10. Handle null safely

## Troubleshooting Guide

Issue: Type mismatch in generic list
Steps:
1. Check variance (out/in)
2. Use proper type projection
3. Review collection type

Issue: Collection is not modifiable
Steps:
1. Check if using mutable collection
2. Use toMutableList() to copy
3. Use mutableListOf() for building

Issue: Type inference failure
Steps:
1. Provide explicit type
2. Check nullability
3. Verify generic parameters

## Advanced Tips and Tricks

Tip 1: Use sequence for large collections
- Lazy evaluation
- Chain operations efficiently
- Use .asSequence()

Tip 2: Use toList() for defensive copying
- Prevents modification
- Returns immutable list

Tip 3: Use buildList for building
- Builder pattern
- Type-safe construction

Tip 4: Use associated functions
- associate, associateBy, associateWith
- Powerful transformations

Tip 5: Use group partition
- groupBy vs partition
- Understand difference

## Example 1: Android List Operations

Android List Operations Example - Common list operations in Android development.

```kotlin
class AndroidListOperations {
    
    // Data model
    data class User(val id: Int, val name: String, val email: String)
    
    // Convert cursor/list to domain models
    fun mapUsers(rawData: List<Map<String, Any>>): List<User> {
        return rawData.map { row ->
            User(
                id = row["id"] as? Int ?: 0,
                name = row["name"] as? String ?: "",
                email = row["email"] as? String ?: ""
            )
        }
    }
    
    // Filter users
    fun filterUsers(users: List<User>, searchQuery: String): List<User> {
        return users.filter { user ->
            user.name.contains(searchQuery, ignoreCase = true) ||
            user.email.contains(searchQuery, ignoreCase = true)
        }
    }
    
    // Group users
    fun groupUsersByFirstLetter(users: List<User>): Map<Char, List<User>> {
        return users.groupBy { it.name.firstOrNull() ?: '?' }
    }
    
    // Sort users
    fun sortUsers(users: List<User>): List<User> {
        return users.sortedWith(
            compareBy(User::name, User::email)
        )
    }
    
    // Pagination
    fun paginate(users: List<User>, page: Int, pageSize: Int): List<User> {
        val start = page * pageSize
        return users.drop(start).take(pageSize)
    }
}
```

## Example 2: Type-Safe Database Operations

Type-Safe Database Operations - Using generics for type-safe database operations.

```kotlin
class TypeSafeDatabase {
    
    // Generic repository
    interface Repository<T, ID> {
        suspend fun findById(id: ID): T?
        suspend fun findAll(): List<T>
        suspend fun insert(item: T)
        suspend fun update(item: T)
        suspend fun delete(id: ID)
    }
    
    // User entity
    data class User(val id: Long, val name: String, val email: String)
    
    // User repository implementation
    class UserRepository : Repository<User, Long> {
        private val users = mutableListOf<User>()
        
        override suspend fun findById(id: Long): User? {
            return users.find { it.id == id }
        }
        
        override suspend fun findAll(): List<User> {
            return users.toList()
        }
        
        override suspend fun insert(item: User) {
            users.add(item)
        }
        
        override suspend fun update(item: User) {
            val index = users.indexOfFirst { it.id == item.id }
            if (index >= 0) users[index] = item
        }
        
        override suspend fun delete(id: Long) {
            users.removeIf { it.id == id }
        }
    }
    
    // Type-safe result wrapper
    sealed class DbResult<out T> {
        data class Success<T>(val data: T) : DbResult<T>()
        data class Failure(val error: String) : DbResult<Nothing>()
    }
    
    suspend fun <T> executeQuery(query: () -> T): DbResult<T> {
        return try {
            DbResult.Success(query())
        } catch (e: Exception) {
            DbResult.Failure(e.message ?: "Unknown error")
        }
    }
}
```

## Example 3: Advanced Collection Patterns

Advanced Collection Patterns - Complex collection operations and patterns.

```kotlin
class AdvancedCollectionPatterns {
    
    // Sequence for lazy evaluation
    fun sequenceExample() {
        // Instead of multiple passes through list
        val result = (1..100)
            .asSequence()
            .filter { it % 2 == 0 }
            .map { it * 2 }
            .take(10)
            .toList()
        // Only processes what it needs
    }
    
    // Complex transformation
    data class Person(val name: String, val age: Int, val city: String)
    
    fun complexTransform(people: List<Person>): Map<String, List<String>> {
        return people
            .filter { it.age >= 18 }
            .groupBy({ it.city }, { it.name })
            .mapValues { (_, names) -> names.sorted() }
    }
    
    // Fold with internal state
    fun foldWithState(items: List<Int>): Pair<Int, Int> {
        return items.fold(Pair(0, 0)) { (sum, count), item ->
            Pair(sum + item, count + 1)
        }
    }
    
    // Running fold
    fun runningFold(): List<Int> {
        return listOf(1, 2, 3, 4, 5).runningFold(0) { acc, item ->
            acc + item
        }
        // [0, 1, 3, 6, 10, 15]
    }
    
    // Window and chunk
    fun windowAndChunk() {
        val numbers = (1..10).toList()
        
        // Sliding window of size 3
        val windows = numbers.windowed(3)
        // [[1,2,3], [2,3,4], [3,4,5], ...]
        
        // Chunk into groups
        val chunks = numbers.chunked(3)
        // [[1,2,3], [4,5,6], [7,8,9], [10]]
    }
    
    // Zipping
    fun zipExample() {
        val names = listOf("Alice", "Bob", "Charlie")
        val ages = listOf(25, 30, 35)
        
        val zipped = names.zip(ages)
        // [(Alice, 25), (Bob, 30), (Charlie, 35)]
        
        // Unzip
        val (unzippedNames, unzippedAges) = zipped.unzip()
    }
}
```

## Output Statement Results

Kotlin Type System:
- Basic Types: Int, Long, Double, Float, Boolean, Char, String
- Nullable Types: String? with safe calls (?., ?:, !!)
- Smart Casts: Automatic type conversion
- Type Checking: is, !is operators

Collections Available:
- Immutable: List, Set, Map (listOf, setOf, mapOf)
- Mutable: MutableList, MutableSet, MutableMap
- Specialized: IntArray, LongArray, etc.

Collection Operations:
- Transform: map, flatMap, flatten
- Filter: filter, filterNot, partition
- Sort: sorted, sortedBy, sortedWith
- Aggregate: sum, average, count, reduce, fold
- Group: groupBy, partition

Generics:
- Generic classes and functions
- Type constraints
- Variance: out (producer), in (consumer)
- Reified types for inline functions

Type-Safe Patterns:
- Nullable type safety
- Sealed classes for results
- Type aliases
- Extension functions

## Cross-References

See: 01_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md
See: 01_Kotlin_Basics_for_Android/02_Android_Kotlin_Conventions.md
See: 04_DATA_PERSISTENCE/01_Database_Development/01_Room_Database_Basics.md
See: 05_NETWORKING/01_HTTP_Communication/01_Retrofit_Basics.md

---

*End of Type System and Collections Guide*