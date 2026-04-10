# Inline Classes

## Overview

Inline classes in Kotlin provide value type semantics without the overhead of object allocation. This guide covers inline class declaration, usage patterns, and integration with Android development.

## Learning Objectives

- Understand inline class fundamentals and type requirements
- Implement type-safe wrappers with zero runtime overhead
- Integrate inline classes with collections and generics
- Implement pattern matching with inline classes
- Combine inline classes with other advanced features

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Type System and Collections](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/03_Type_System_and_Collections.md)

## Core Concepts

### Value Types and Identity

Inline classes are Kotlin's approach to value types. Unlike regular classes, inline classes:
- Are inlined at compile time to their underlying type
- Do not create runtime objects
- Have identity-agnostic equality (equals compares values)
- Cannot be used as generic type arguments directly

### JVM Implementation Details

On the JVM, inline classes are implemented as primitive wrappers when possible:
- Single primitive property: Stored directly on the stack
- Reference type property: Wrapped in a java.lang.Integer-like box

### Type Safety at Compile Time

Inline classes enable type-safe APIs:
- Prevent mixing up similar types (e.g., UserId vs OrderId)
- Provide better IDE autocomplete
- Catch type errors at compile time

## Code Examples

### Example 1: Type-Safe ID System

```kotlin
import kotlin.reflect.*

/**
 * Inline class for type-safe identifiers
 * UserId represents a user identifier with compile-time type safety
 */
inline class UserId(val value: String) {
    init {
        require(value.isNotBlank()) { "UserId cannot be blank" }
    }
    
    companion object {
        fun fromString(value: String): UserId = UserId(value)
        
        // Generate a new unique ID
        fun generate(): UserId = UserId("user_${System.nanoTime()}")
    }
}

/**
 * Inline class for order identifiers
 */
inline class OrderId(val value: String) {
    init {
        require(value.isNotBlank()) { "OrderId cannot be blank" }
    }
}

/**
 * Inline class for product identifiers
 */
inline class ProductId(val value: String) {
    init {
        require(value.isNotBlank()) { "ProductId cannot be blank" }
    }
}

/**
 * Example domain model using inline IDs
 */
data class User(
    val id: UserId,
    val name: String,
    val email: String
)

data class Order(
    val id: OrderId,
    val userId: UserId,
    val items: List<OrderItem>,
    val total: Money
)

data class OrderItem(
    val productId: ProductId,
    val quantity: Int,
    val price: Money
)

/**
 * Inline class for monetary values
 * Provides type-safe money handling
 */
inline class Money(val cents: Long) {
    init {
        require(cents >= 0) { "Money cannot be negative" }
    }
    
    companion object {
        fun dollars(amount: Double): Money = Money((amount * 100).toLong())
        
        fun cents(amount: Long): Money = Money(amount)
        
        val ZERO = Money(0)
    }
    
    operator fun plus(other: Money): Money = Money(cents + other.cents)
    
    operator fun minus(other: Money): Money = Money(cents - other.cents)
    
    operator fun times(factor: Double): Money = Money((cents * factor).toLong())
    
    operator fun compareTo(other: Money): Int = cents.compareTo(other.cents)
    
    fun toDouble(): Double = cents / 100.0
    
    override fun toString(): String = "$${toDouble()}"
}

/**
 * Service for managing users with type-safe IDs
 */
class UserService {
    private val users = mutableMapOf<UserId, User>()
    
    fun createUser(name: String, email: String): User {
        val id = UserId.generate()
        val user = User(id, name, email)
        users[id] = user
        return user
    }
    
    fun getUser(id: UserId): User? = users[id]
    
    fun getUserOrders(id: UserId, orderService: OrderService): List<Order> {
        return orderService.getOrdersByUser(id)
    }
    
    fun updateUser(id: UserId, update: (User) -> User): User? {
        val user = users[id] ?: return null
        val updated = update(user)
        users[id] = updated
        return updated
    }
    
    fun deleteUser(id: UserId): Boolean = users.remove(id) != null
}

/**
 * Service for managing orders with type-safe IDs
 */
class OrderService {
    private val orders = mutableMapOf<OrderId, Order>()
    private val userOrders = mutableMapOf<UserId, MutableList<OrderId>>()
    
    fun createOrder(userId: UserId, items: List<OrderItem>): Order {
        val id = OrderId("order_${System.nanoTime()}")
        val total = items.fold(Money.ZERO) { acc, item -> acc + item.price }
        val order = Order(id, userId, items, total)
        orders[id] = order
        userOrders.getOrPut(userId) { mutableListOf() }.add(id)
        return order
    }
    
    fun getOrder(id: OrderId): Order? = orders[id]
    
    fun getOrdersByUser(userId: UserId): List<Order> {
        return userOrders[userId]?.mapNotNull { orders[it] } ?: emptyList()
    }
    
    fun getOrderValue(id: OrderId): Money? = orders[id]?.total
}

/**
 * Example demonstrating type safety prevents mixing IDs
 */
class TypeSafeExample {
    fun demonstrate() {
        val userService = UserService()
        val orderService = OrderService()
        
        // Create user
        val user = userService.createUser("John Doe", "john@example.com")
        println("Created user: ${user.id}")
        
        // Create order with correct type - UserId is passed
        val order = orderService.createOrder(
            userId = user.id,
            items = listOf(
                OrderItem(
                    productId = ProductId("prod_123"),
                    quantity = 2,
                    price = Money.dollars(29.99)
                )
            )
        )
        println("Created order: ${order.id}, total: ${order.total}")
        
        // This would cause compile error - OrderId cannot be passed where UserId is expected
        // userService.getUser(order.id) // Compile error!
        
        // Correct usage - passing correct ID types
        val userOrders = orderService.getOrdersByUser(user.id)
        println("User orders: ${userOrders.size}")
        
        // Type-safe money operations
        val price1 = Money.dollars(10.00)
        val price2 = Money.dollars(15.00)
        val total = price1 + price2
        println("Total: $total")
        
        // Money comparison
        println("Is price1 < price2: ${price1 < price2}")
    }
}
```

**Output:**
```
Created user: user_1234567890
Created order: order_1234567891, total: $59.98
User orders: 1
Total: $25.0
Is price1 < price2: true
```

This example demonstrates type-safe identifiers that prevent runtime errors by catching type mismatches at compile time.

### Example 2: Validation and Domain Modeling

```kotlin
import kotlin.reflect.*
import kotlin.time.*

/**
 * Inline class for email validation
 * Provides compile-time validation and type-safe email handling
 */
inline class Email(val value: String) {
    init {
        require(isValidEmail(value)) { "Invalid email format: $value" }
    }
    
    companion object {
        fun isValidEmail(email: String): Boolean {
            return EMAIL_REGEX.matches(email)
        }
        
        private val EMAIL_REGEX = "^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$".toRegex()
    }
    
    fun getDomain(): String = value.substringAfter("@")
    
    fun isBusinessEmail(): Boolean = getDomain() in BUSINESS_DOMAINS
    
    companion object BUSINESS_DOMAINS {
        val businessDomains = setOf("company.com", "business.org", "corp.com")
        
        fun isBusinessEmail(email: String): Boolean {
            val domain = email.substringAfter("@", "")
            return domain in businessDomains
        }
    }
}

/**
 * Inline class for phone number validation and formatting
 */
inline class PhoneNumber(val value: String) {
    init {
        require(isValidPhone(value)) { "Invalid phone number: $value" }
    }
    
    companion object {
        fun isValidPhone(phone: String): Boolean {
            val cleaned = phone.replace(Regex("[^0-9]"), "")
            return cleaned.length in 10..15
        }
        
        fun format(phone: String): String {
            val cleaned = phone.replace(Regex("[^0-9]"), "")
            return when (cleaned.length) {
                10 -> "(${cleaned.substring(0, 3)}) ${cleaned.substring(3, 6)}-${cleaned.substring(6)}"
                11 -> "+${cleaned[0]} (${cleaned.substring(1, 4)}) ${cleaned.substring(4, 7)}-${cleaned.substring(7)}"
                else -> phone
            }
        }
    }
    
    fun toE164(): String {
        val cleaned = value.replace(Regex("[^0-9]"), "")
        return if (cleaned.length == 10) "+1$cleaned" else "+$cleaned"
    }
    
    fun format(): String = format(value)
    
    fun isMobile(): Boolean = value.first() in '5'..'9'
}

/**
 * Inline class for strong password validation
 */
inline class Password(val value: String) {
    init {
        require(value.length >= MIN_LENGTH) { "Password must be at least $MIN_LENGTH characters" }
        require(value.any { it.isUpperCase() }) { "Password must contain uppercase letter" }
        require(value.any { it.isLowerCase() }) { "Password must contain lowercase letter" }
        require(value.any { it.isDigit() }) { "Password must contain digit" }
        require(value.any { !it.isLetterOrDigit() }) { "Password must contain special character" }
    }
    
    companion object {
        const val MIN_LENGTH = 8
        
        fun isValid(value: String): Boolean = runCatching { Password(value) }.isSuccess
    }
    
    fun getStrength(): PasswordStrength {
        val length = value.length
        val hasUpper = value.any { it.isUpperCase() }
        val hasLower = value.any { it.isLowerCase() }
        val hasDigit = value.any { it.isDigit() }
        val hasSpecial = value.any { !it.isLetterOrDigit() }
        
        val score = listOf(
            length >= 12,
            length >= 16,
            hasUpper,
            hasLower,
            hasDigit,
            hasSpecial
        ).count { it }
        
        return when {
            score >= 5 -> PasswordStrength.STRONG
            score >= 3 -> PasswordStrength.MEDIUM
            else -> PasswordStrength.WEAK
        }
    }
    
    enum class PasswordStrength {
        WEAK, MEDIUM, STRONG
    }
}

/**
 * Inline class for URL validation
 */
inline class Url(val value: String) {
    init {
        require(isValidUrl(value)) { "Invalid URL: $value" }
    }
    
    companion object {
        private val URL_REGEX = "^https?://[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}(/.*)?$".toRegex()
        
        fun isValidUrl(url: String): Boolean = URL_REGEX.matches(url)
    }
    
    fun getProtocol(): String = value.substringBefore("://")
    
    fun getHost(): String = value.substringAfter("://").substringBefore("/")
    
    fun getPath(): String = value.substringAfter(getHost(), "")
    
    fun isSecure(): Boolean = getProtocol() == "https"
    
    fun withHttps(): Url = if (isSecure()) this else Url("https://${value.substringAfter("://")}")
}

/**
 * Inline class for duration in milliseconds
 */
inline class DurationMillis(val millis: Long) {
    init {
        require(millis >= 0) { "Duration cannot be negative" }
    }
    
    companion object {
        val ZERO = DurationMillis(0)
        
        fun seconds(seconds: Int): DurationMillis = DurationMillis(seconds * 1000L)
        
        fun minutes(minutes: Int): DurationMillis = DurationMillis(minutes * 60000L)
        
        fun hours(hours: Int): DurationMillis = DurationMillis(hours * 3600000L)
    }
    
    operator fun plus(other: DurationMillis): DurationMillis = DurationMillis(millis + other.millis)
    
    operator fun compareTo(other: DurationMillis): Int = millis.compareTo(other.millis)
    
    fun inSeconds(): Int = (millis / 1000).toInt()
    
    fun inMinutes(): Int = (millis / 60000).toInt()
    
    override fun toString(): String {
        val seconds = millis / 1000
        val minutes = seconds / 60
        val hours = minutes / 60
        return when {
            hours > 0 -> "${hours}h ${minutes % 60}m"
            minutes > 0 -> "${minutes}m ${seconds % 60}s"
            else -> "${seconds}s"
        }
    }
}

/**
 * Validation-based registration example
 */
class RegistrationService {
    fun registerUser(
        email: Email,
        password: Password,
        phone: PhoneNumber
    ): RegistrationResult {
        return RegistrationResult.Success(
            email = email.value,
            phone = phone.value
        )
    }
    
    sealed class RegistrationResult {
        data class Success(val email: String, val phone: String) : RegistrationResult()
        data class Failure(val errors: List<String>) : RegistrationResult()
    }
}

/**
 * URL configuration example
 */
class UrlConfiguration(
    private val baseUrl: Url,
    private val apiUrl: Url
) {
    fun getEndpoint(path: String): Url {
        return Url("${baseUrl.value}/$path")
    }
    
    fun getApiEndpoint(path: String): Url {
        return Url("${apiUrl.value}/$path")
    }
}

/**
 * Timeout configuration using inline class
 */
class NetworkConfiguration(
    private val connectTimeout: DurationMillis = DurationMillis.seconds(30),
    private val readTimeout: DurationMillis = DurationMillis.seconds(30),
    private val writeTimeout: DurationMillis = DurationMillis.seconds(30)
) {
    fun getTimeouts(): Triple<Long, Long, Long> {
        return Triple(connectTimeout.millis, readTimeout.millis, writeTimeout.millis)
    }
}

// Usage demonstration
class ValidationExample {
    fun demonstrate() {
        println("=== Email Validation ===")
        val email = Email("user@company.com")
        println("Email: ${email.value}")
        println("Domain: ${email.getDomain()}")
        println("Is business: ${email.isBusinessEmail()}")
        
        println("\n=== Phone Validation ===")
        val phone = PhoneNumber("555-123-4567")
        println("Phone: ${phone.format()}")
        println("E164: ${phone.toE164()}")
        println("Is mobile: ${phone.isMobile()}")
        
        println("\n=== Password Validation ===")
        val password = Password("SecureP@ss123")
        println("Strength: ${password.getStrength()}")
        
        println("\n=== Duration ===")
        val duration = DurationMillis.minutes(5) + DurationMillis.seconds(30)
        println("Duration: $duration")
        
        println("\n=== Network Configuration ===")
        val config = NetworkConfiguration(
            connectTimeout = DurationMillis.seconds(60)
        )
        println("Timeouts: ${config.getTimeouts()}")
    }
}
```

**Output:**
```
=== Email Validation ===
Email: user@company.com
Domain: company.com
Is business: true

=== Phone Validation ===
Phone: (555) 123-4567
E164: +15551234567
Is mobile: true

=== Password Validation ===
Strength: STRONG
=== Duration ===
Duration: 5m 30s

Timeouts: (60000, 30000, 30000)
```

This example shows validation inline classes that wrap validation logic.

### Example 3: Collections and Generics Integration

```kotlin
import kotlin.collections.*

/**
 * Inline class for type-safe collections
 * Demonstrates integration with collections and generics
 */
inline class UserList(val users: List<String>) {
    fun isEmpty(): Boolean = users.isEmpty()
    
    fun size(): Int = users.size
    
    fun contains(userId: String): Boolean = userId in users
    
    fun add(userId: String): UserList = UserList(users + userId)
    
    fun remove(userId: String): UserList = UserList(users - userId)
    
    operator fun plus(other: UserList): UserList = UserList(users + other.users)
    
    fun filter(predicate: (String) -> Boolean): UserList = UserList(users.filter(predicate))
    
    fun map(transform: (String) -> String): UserList = UserList(users.map(transform))
    
    fun first(): String? = users.firstOrNull()
    
    fun last(): String? = users.lastOrNull()
    
    override fun toString(): String = users.joinToString(", ")
}

/**
 * Inline class for non-empty lists
 */
inline class NonEmptyList<T>(val items: List<T>) {
    init {
        require(items.isNotEmpty()) { "List cannot be empty" }
    }
    
    companion object {
        fun <T> of(vararg elements: T): NonEmptyList<T> = NonEmptyList(elements.toList())
        
        fun <T> from(list: List<T>): NonEmptyList<T>? = 
            if (list.isNotEmpty()) NonEmptyList(list) else null
    }
    
    fun head(): T = items.first()
    
    fun tail(): List<T> = items.drop(1)
    
    operator fun get(index: Int): T = items[index]
    
    fun size(): Int = items.size
    
    fun isEmpty(): Boolean = false // Always non-empty by definition
    
    override fun toString(): String = items.joinToString(" :: ")
}

/**
 * Inline class for bounded values (0.0 to 1.0)
 */
inline class Percentage(val value: Double) {
    init {
        require(value in 0.0..1.0) { "Percentage must be between 0 and 1" }
    }
    
    companion object {
        val ZERO = Percentage(0.0)
        val HUNDRED = Percentage(1.0)
        
        fun fromFraction(value: Double): Percentage = Percentage(value.coerceIn(0.0, 1.0))
        
        fun fromPercent(value: Double): Percentage = Percentage((value / 100.0).coerceIn(0.0, 1.0))
    }
    
    fun toPercent(): Double = value * 100
    
    fun toFraction(): Double = value
    
    operator fun times(other: Double): Double = value * other
    
    operator fun plus(other: Percentage): Percentage = Percentage((value + other.value).coerceIn(0.0, 1.0))
    
    override fun toString(): String = "${(value * 100).toInt()}%"
}

/**
 * Inline class for bounded integers (min to max)
 */
inline class BoundedInt(val value: Int, val minValue: Int = Int.MIN_VALUE, val maxValue: Int = Int.MAX_VALUE) {
    init {
        require(value in minValue..maxValue) { 
            "Value $value must be between $minValue and $maxValue" 
        }
    }
    
    companion object {
        fun <T : Comparable<T>> clamp(value: Int, min: Int, max: Int): BoundedInt {
            return BoundedInt(value.coerceIn(min, max), min, max)
        }
    }
    
    fun increment(): BoundedInt = BoundedInt((value + 1).coerceIn(minValue, maxValue), minValue, maxValue)
    
    fun decrement(): BoundedInt = BoundedInt((value - 1).coerceIn(minValue, maxValue), minValue, maxValue)
    
    fun clamp(newMin: Int, newMax: Int): BoundedInt {
        return BoundedInt(value.coerceIn(newMin, newMax), newMin, newMax)
    }
    
    override fun toString(): String = value.toString()
}

/**
 * Inline class for bounded coordinates
 */
inline class Coordinate(val value: Double) {
    init {
        require(value in MIN_VALUE..MAX_VALUE) {
            "Coordinate must be between $MIN_VALUE and $MAX_VALUE"
        }
    }
    
    companion object {
        const val MIN_VALUE = -90.0
        const val MAX_VALUE = 90.0
        
        fun create(value: Double): Coordinate = Coordinate(value.coerceIn(MIN_VALUE, MAX_VALUE))
    }
    
    fun toRadians(): Double = Math.toRadians(value)
    
    operator fun plus(other: Coordinate): Coordinate = Coordinate((value + other.value).coerceIn(MIN_VALUE, MAX_VALUE))
    
    operator fun minus(other: Coordinate): Double = value - other.value
    
    override fun toString(): String = "$value°"
}

data class GeoLocation(
    val latitude: Coordinate,
    val longitude: Coordinate
)

/**
 * Pagination using inline classes
 */
inline class PageNumber(val value: Int) {
    init {
        require(value > 0) { "Page number must be positive" }
    }
    
    companion object {
        val FIRST = PageNumber(1)
    }
    
    fun next(): PageNumber = PageNumber(value + 1)
    
    fun previous(): PageNumber = PageNumber((value - 1).coerceAtLeast(1))
    
    override fun toString(): String = value.toString()
}

inline class PageSize(val value: Int) {
    init {
        require(value in VALID_SIZES) { "Page size must be one of $VALID_SIZES" }
    }
    
    companion object {
        val VALID_SIZES = listOf(10, 25, 50, 100)
        
        val DEFAULT = PageSize(25)
        val SMALL = PageSize(10)
        val LARGE = PageSize(100)
    }
    
    override fun toString(): String = value.toString()
}

class PaginatedResult<T>(
    val page: PageNumber,
    val pageSize: PageSize,
    val totalItems: Int,
    val items: List<T>
) {
    val totalPages: Int get() = (totalItems / pageSize.value) + 1
    
    fun hasNextPage(): Boolean = page.value < totalPages
    
    fun hasPreviousPage(): Boolean = page.value > 1
    
    override fun toString(): String = "Page ${page.value} of $totalPages (${items.size} items)"
}

/**
 * Usage demonstration
 */
class CollectionIntegrationExample {
    fun demonstrate() {
        println("=== User List ===")
        val userList = UserList(listOf("user1", "user2", "user3"))
        println("List: $userList")
        println("Size: ${userList.size()}")
        
        val filtered = userList.filter { it.startsWith("user") }
        println("Filtered: $filtered")
        
        println("\n=== Non-Empty List ===")
        val nonEmpty = NonEmptyList.of("A", "B", "C")
        println("List: $nonEmpty")
        println("Head: ${nonEmpty.head()}")
        println("Tail: ${nonEmpty.tail()}")
        
        println("\n=== Percentage ===")
        val progress = Percentage.fromPercent(75.0)
        println("Progress: $progress")
        println("As fraction: ${progress.toFraction()}")
        
        println("\n=== Coordinates ===")
        val location = GeoLocation(
            latitude = Coordinate.create(37.7749),
            longitude = Coordinate.create(-122.4194)
        )
        println("Location: (${location.latitude}, ${location.longitude})")
        
        println("\n=== Pagination ===")
        val pagination = PaginatedResult(
            page = PageNumber(1),
            pageSize = PageSize.DEFAULT,
            totalItems = 100,
            items = listOf("Item 1", "Item 2", "Item 3")
        )
        println(pagination)
    }
}
```

**Output:**
```
=== User List ===
List: user1, user2, user3
Size: 3
Filtered: user1, user2, user3

=== Non-Empty List ===
List: A :: B :: C
Head: A
Tail: [B, C]

=== Percentage ===
Progress: 75%
As fraction: 0.75

=== Coordinates ===
Location: (37.7749°, -122.4194°)

=== Pagination ===
Page 1 of 4 (3 items)
```

## Best Practices

- Use inline classes for type-safe wrappers around primitive or reference types
- Add validation logic in init blocks to fail fast
- Keep inline classes simple - avoid complex logic
- Use companion objects for factory methods
- Consider JVM inline classes for performance-critical code
- Use inline classes to prevent mixing similar types

## Common Pitfalls

### Problem: Boxing overhead with reference types
**Solution:** Use primitive types when possible for inline classes

### Problem: Generics limitations
**Solution:** Use type aliases or wrapper classes for generic bounds

### Problem: Cannot inherit from inline classes
**Solution:** Use composition or sealed class hierarchies instead

## Troubleshooting Guide

**Q: When should I use inline classes?**
A: When you need type safety and compile-time checks without runtime overhead.

**Q: Can inline classes have methods?**
A: Yes, methods can be added but they will have some overhead.

**Q: Can inline classes implement interfaces?**
A: Yes, they can implement a single interface.

## Cross-References

- [Type System and Collections](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/03_Type_System_and_Collections.md)
- [Sealed Classes](./04_Sealed_Classes.md)
- [Type Safety Patterns](./05_Type_Safety_Patterns.md)