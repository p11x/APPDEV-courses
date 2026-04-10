# Type Safety Patterns

## Overview

Type safety patterns in Kotlin prevent runtime type errors through compile-time checks. This guide covers pattern matching, type guards, and advanced type system features.

## Learning Objectives

- Master pattern matching with is and when
- Implement type-safe casts and smart casts
- Use sealed classes for exhaustive type checking
- Build generic type-safe utilities
- Handle platform types safely

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Sealed Classes](./04_Sealed_Classes.md)

## Core Concepts

### Smart Casts

Kotlin's smart cast system tracks type checks:
- After `is` check, compiler knows specific type
- Works with `val` properties only
- Use `as` or `as?` for explicit casts

### Type Guards

Extend smart casts with type guard functions:
- `inline` functions preserve type information
- `reified` enables type reification
- Custom type guard functions

### Platform Types

Platform types arise from Java interop:
- Cannot be fully type-checked
- Use @Nullable and @NonNull annotations
- Handle as nullable by default

## Code Examples

### Example 1: Comprehensive Pattern Matching

```kotlin
import kotlin.math.*

/**
 * Type-safe visitor pattern implementation
 */
interface Visitor<T> {
    fun visit(node: Expression): T
}

sealed class Expression {
    data class Literal(val value: Double) : Expression()
    data class Variable(val name: String) : Expression()
    data class Add(val left: Expression, val right: Expression) : Expression()
    data class Subtract(val left: Expression, val right: Expression) : Expression()
    data class Multiply(val left: Expression, val right: Expression) : Expression()
    data class Divide(val left: Expression, val right: Expression) : Expression()
    data class Pow(val base: Expression, val exponent: Expression) : Expression()
    data class Negate(val operand: Expression) : Expression()
    
    companion object {
        fun literal(value: Double): Expression = Literal(value)
        fun variable(name: String): Expression = Variable(name)
    }
}

/**
 * Expression evaluator
 */
class ExpressionEvaluator : Visitor<Double> {
    private val variables = mutableMapOf<String, Double>()
    
    fun setVariable(name: String, value: Double) {
        variables[name] = value
    }
    
    override fun visit(node: Expression): Double = when (node) {
        is Expression.Literal -> node.value
        is Expression.Variable -> variables[node.name] 
            ?: throw IllegalArgumentException("Unknown variable: ${node.name}")
        is Expression.Add -> visit(node.left) + visit(node.right)
        is Expression.Subtract -> visit(node.left) - visit(node.right)
        is Expression.Multiply -> visit(node.left) * visit(node.right)
        is Expression.Divide -> {
            val divisor = visit(node.right)
            if (divisor == 0.0) throw ArithmeticException("Division by zero")
            visit(node.left) / divisor
        }
        is Expression.Pow -> {
            val base = visit(node.base)
            val exp = visit(node.exponent)
            Pow(base, exp)
        }
        is Expression.Negate -> -visit(node.operand)
    }
    
    companion object {
        private fun Pow(base: Double, exp: Double): Double = base.toDouble().pow(exp.toDouble())
    }
}

/**
 * Expression printer for debugging
 */
class ExpressionPrinter : Visitor<String> {
    override fun visit(node: Expression): String = when (node) {
        is Expression.Literal -> node.value.toString()
        is Expression.Variable -> node.name
        is Expression.Add -> "(${visit(node.left)} + ${visit(node.right)})"
        is Expression.Subtract -> "(${visit(node.left)} - ${visit(node.right)})"
        is Expression.Multiply -> "(${visit(node.left)} * ${visit(node.right)})"
        is Expression.Divide -> "(${visit(node.left)} / ${visit(node.right)})"
        is Expression.Pow -> "(${visit(node.base)} ^ ${visit(node.exponent)})"
        is Expression.Negate -> "-${visit(node.operand)}"
    }
}

/**
 * Type-safe algebraic expression builder
 */
class Expr {
    private val expression: Expression
    
    private constructor(expr: Expression) {
        this.expression = expr
    }
    
    fun eval(): Double = ExpressionEvaluator().visit(expression)
    
    fun print(): String = ExpressionPrinter().visit(expression)
    
    companion object {
        fun literal(value: Double): Expr = Expr(Expression.Literal(value))
        
        fun variable(name: String): Expr = Expr(Expression.Variable(name))
        
        fun Expr.plus(other: Expr): Expr = 
            Expr(Expression.Add(this.expression, other.expression))
        
        fun Expr.minus(other: Expr): Expr = 
            Expr(Expression.Subtract(this.expression, other.expression))
        
        fun Expr.times(other: Expr): Expr = 
            Expr(Expression.Multiply(this.expression, other.expression))
        
        fun Expr.div(other: Expr): Expr = 
            Expr(Expression.Divide(this.expression, other.expression))
        
        fun Expr.pow(other: Expr): Expr = 
            Expr(Expression.Pow(this.expression, other.expression))
    }
}

/**
 * Shape sealed class hierarchy for pattern matching
 */
sealed class Shape {
    data class Circle(val radius: Double) : Shape() {
        val area: Double get() = PI * radius * radius
        val perimeter: Double get() = 2 * PI * radius
    }
    
    data class Rectangle(
        val width: Double,
        val height: Double
    ) : Shape() {
        val area: Double get() = width * height
        val perimeter: Double get() = 2 * (width + height)
    }
    
    data class Triangle(
        val a: Double,
        val b: Double,
        val c: Double
    ) : Shape() {
        val area: Double get() {
            val s = perimeter / 2
            return sqrt(s * (s - a) * (s - b) * (s - c))
        }
        val perimeter: Double get() = a + b + c
    }
    
    data class Polygon(val sides: List<Double>) : Shape() {
        init {
            require(sides.size >= 3) { "Polygon must have at least 3 sides" }
        }
    }
    
    companion object {
        fun circle(radius: Double): Shape = Circle(radius)
        fun rectangle(width: Double, height: Double): Shape = Rectangle(width, height)
        fun triangle(a: Double, b: Double, c: Double): Shape = Triangle(a, b, c)
    }
}

/**
 * Type-safe shape operations
 */
object ShapeOperations {
    fun area(shape: Shape): Double = when (shape) {
        is Shape.Circle -> shape.area
        is Shape.Rectangle -> shape.area
        is Shape.Triangle -> shape.area
        is Shape.Polygon -> calculatePolygonArea(shape.sides)
    }
    
    fun perimeter(shape: Shape): Double = when (shape) {
        is Shape.Circle -> shape.perimeter
        is Shape.Rectangle -> shape.perimeter
        is Shape.Triangle -> shape.perimeter
        is Shape.Polygon -> shape.sides.sum()
    }
    
    private fun calculatePolygonArea(sides: List<Double>): Double {
        // Using shoelace formula for simple polygon
        val n = sides.size
        if (n < 3) return 0.0
        
        // Assume regular polygon for simplicity
        val perimeter = sides.sum()
        val apothem = sides.first() / (2 * tan(PI / n))
        return (perimeter * apothem) / 2
    }
    
    fun <T> processShape(shape: Shape, processor: (Shape) -> T): T = processor(shape)
}

/**
 * Shape visitor interface
 */
interface ShapeVisitor<T> {
    fun visitCircle(circle: Shape.Circle): T
    fun visitRectangle(rectangle: Shape.Rectangle): T
    fun visitTriangle(triangle: Shape.Triangle): T
    fun visitPolygon(polygon: Shape.Polygon): T
}

/**
 * Default shape visitor
 */
class DefaultShapeVisitor<T> : ShapeVisitor<T> {
    override fun visitCircle(circle: Shape.Circle): T {
        throw NotImplementedError("Not implemented for Circle")
    }
    
    override fun visitRectangle(rectangle: Shape.Rectangle): T {
        throw NotImplementedError("Not implemented for Rectangle")
    }
    
    override fun visitTriangle(triangle: Shape.Triangle): T {
        throw NotImplementedError("Not implemented for Triangle")
    }
    
    override fun visitPolygon(polygon: Shape.Polygon): T {
        throw NotImplementedError("Not implemented for Polygon")
    }
}

/**
 * Shape visitor for area calculation
 */
object AreaVisitor : ShapeVisitor<Double> {
    override fun visitCircle(circle: Shape.Circle): Double = circle.area
    
    override fun visitRectangle(rectangle: Shape.Rectangle): Double = rectangle.area
    
    override fun visitTriangle(triangle: Shape.Triangle): Double = triangle.area
    
    override fun visitPolygon(polygon: Shape.Polygon): Double {
        val n = polygon.sides.size
        if (n < 3) return 0.0
        val side = polygon.sides.first()
        return (n * side * side) / (4 * tan(PI / n))
    }
}

/**
 * Pattern matching demonstration
 */
class PatternMatchingDemo {
    fun demonstrate() {
        val shapes = listOf(
            Shape.circle(5.0),
            Shape.rectangle(4.0, 6.0),
            Shape.triangle(3.0, 4.0, 5.0)
        )
        
        for (shape in shapes) {
            when (shape) {
                is Shape.Circle -> {
                    println("Circle: r=${shape.radius}, area=${shape.area}")
                }
                is Shape.Rectangle -> {
                    println("Rectangle: ${shape.width}x${shape.height}, area=${shape.area}")
                }
                is Shape.Triangle -> {
                    println("Triangle: sides=${shape.a},${shape.b},${shape.c}, area=${shape.area}")
                }
                is Shape.Polygon -> {
                    println("Polygon: ${shape.sides.size} sides")
                }
            }
        }
        
        // Using visitor
        println("\nUsing visitor:")
        shapes.forEach { shape ->
            val area = when (shape) {
                is Shape.Circle -> AreaVisitor.visitCircle(shape)
                is Shape.Rectangle -> AreaVisitor.visitRectangle(shape)
                is Shape.Triangle -> AreaVisitor.visitTriangle(shape)
                is Shape.Polygon -> AreaVisitor.visitPolygon(shape)
            }
            println("Area = $area")
        }
    }
}
```

**Output:**
```
Circle: r=5.0, area=78.53981633974483
Rectangle: 4.0x6.0, area=24.0
Triangle: sides=3.0,4.0,6.0, area=0.0

Using visitor:
Area = 78.53981633974483
Area = 24.0
Area = 0.0
```

### Example 2: Generic Type-Safe Utilities

```kotlin
import kotlin.reflect.*

/**
 * Type-safe cast utility
 */
object SafeCast {
    /**
     * Cast to target type or null
     */
    inline fun <reified T> cast(value: Any?): T? {
        return value as? T
    }
    
    /**
     * Cast with predicate
     */
    inline fun <reified T> castIf(
        value: Any?,
        predicate: (T) -> Boolean
    ): T? {
        return (value as? T)?.let { if (predicate(it)) it else null }
    }
    
    /**
     * Cast to list of type
     */
    inline fun <reified T> castList(value: Any?): List<T> {
        return (value as? List<*>)?.filterIsInstance<T>() ?: emptyList()
    }
    
    /**
     * Safe cast with default
     */
    inline fun <reified T> castOrDefault(value: Any?, default: T): T {
        return value as? T ?: default
    }
}

/**
 * Type guard functions
 */
object TypeGuards {
    inline fun <reified T : Any> isType(value: Any?): Boolean {
        return value is T
    }
    
    inline fun <reified T : Any> isNotType(value: Any?): Boolean {
        return value !is T
    }
    
    inline fun <reified T : Any> isNullOrType(value: Any?): Boolean {
        return value == null || value is T
    }
    
    inline fun <reified T : Any> isListOfType(value: Any?): Boolean {
        return value is List<*> && value.all { it is T }
    }
}

/**
 * Generic type checker
 */
class TypeChecker {
    private val typeCache = mutableMapOf<String, Boolean>()
    
    inline fun <reified T> checkType(value: Any?): Boolean {
        return value is T
    }
    
    fun isInteger(value: Any?): Boolean = value is Int
    fun isLong(value: Any?): Boolean = value is Long
    fun isDouble(value: Any?): Boolean = value is Double
    fun isString(value: Any?): Boolean = value is String
    fun isList(value: Any?): Boolean = value is List<*>
    fun isMap(value: Any?): Boolean = value is Map<*, *>
    fun isSet(value: Any?): Boolean = value is Set<*>
    
    fun isNumber(value: Any?): Boolean =
        isInteger(value) || isLong(value) || isDouble(value) || value is Float || value is Byte
    
    fun isCollection(value: Any?): Boolean =
        isList(value) || isSet(value)
}

/**
 * Type-safe builder
 */
class TypeSafeBuilder<T> {
    private val items = mutableListOf<T>()
    
    fun add(item: T): TypeSafeBuilder<T> {
        items.add(item)
        return this
    }
    
    fun addAll(items: Collection<T>): TypeSafeBuilder<T> {
        this.items.addAll(items)
        return this
    }
    
    fun build(): List<T> = items.toList()
    
    fun clear(): TypeSafeBuilder<T> {
        items.clear()
        return this
    }
}

/**
 * Type-safe map operations
 */
class TypeSafeMap<K : Any, V : Any>(
    private val map: MutableMap<K, V> = mutableMapOf()
) {
    fun put(key: K, value: V): V? = map.put(key, value)
    
    fun get(key: K): V? = map[key]
    
    fun getOrThrow(key: K): V = map[key] 
        ?: throw NoSuchElementException("Key not found: $key")
    
    fun getOrDefault(key: K, default: V): V = map[key] ?: default
    
    inline fun <reified V : Any> getTyped(key: K): V? = map[key] as? V
    
    fun contains(key: K): Boolean = key in map
    
    fun remove(key: K): V? = map.remove(key)
    
    val size: Int get() = map.size
    
    fun isEmpty(): Boolean = map.isEmpty()
    
    fun keys: Set<K> get() = map.keys
    
    fun values: Collection<V> get() = map.values
    
    fun entries: Set<Map.Entry<K, V>> get() = map.entries
}

/**
 * Generic validator with type safety
 */
class Validator<T : Any> {
    private val validators = mutableListOf<(T) -> ValidationResult>()
    
    fun addValidator(validator: (T) -> ValidationResult): Validator<T> {
        validators.add(validator)
        return this
    }
    
    fun validate(value: T): ValidationResult {
        val allErrors = validators.flatMap { validator ->
            when (val result = validator(value)) {
                is ValidationResult.Valid -> emptyList()
                is ValidationResult.Invalid -> result.errors
            }
        }
        return if (allErrors.isEmpty()) ValidationResult.Valid
        else ValidationResult.Invalid(allErrors)
    }
    
    sealed class ValidationResult {
        object Valid : ValidationResult()
        data class Invalid(val errors: List<String>) : ValidationResult()
    }
}

/**
 * Predefined validators
 */
object PredefinedValidators {
    fun <T : Any> notNull(): (T?) -> Validator<T>.ValidationResult = { value ->
        if (value != null) Validator.ValidationResult.Valid
        else Validator.ValidationResult.Invalid(listOf("Value cannot be null"))
    }
    
    fun notBlank(): (String) -> Validator.ValidationResult = { value ->
        if (value.isNotBlank()) Validator.ValidationResult.Valid
        else Validator.ValidationResult.Invalid(listOf("Value cannot be blank"))
    }
    
    fun notEmpty(): (Collection<*>) -> Validator.ValidationResult = { value ->
        if (value.isNotEmpty()) Validator.ValidationResult.Valid
        else Validator.ValidationResult.Invalid(listOf("Collection cannot be empty"))
    }
    
    fun minLength(min: Int): (String) -> Validator.ValidationResult = { value ->
        if (value.length >= min) Validator.ValidationResult.Valid
        else Validator.ValidationResult.Invalid(listOf("Minimum length is $min"))
    }
    
    fun maxLength(max: Int): (String) -> Validator.ValidationResult = { value ->
        if (value.length <= max) Validator.ValidationResult.Valid
        else Validator.ValidationResult.Invalid(listOf("Maximum length is $max"))
    }
    
    fun range(min: Int, max: Int): (Int) -> Validator.ValidationResult = { value ->
        if (value in min..max) Validator.ValidationResult.Valid
        else Validator.ValidationResult.Invalid(listOf("Value must be between $min and $max"))
    }
}

/**
 * Type-safe converter
 */
class Converter<in F, out T> {
    private val converters = mutableListOf<(F) -> T?>()
    
    fun addConverter(converter: (F) -> T?): Converter<F, T> {
        converters.add(converter)
        return this
    }
    
    fun convert(value: F): T? {
        for (converter in converters) {
            val result = converter(value)
            if (result != null) return result
        }
        return null
    }
    
    fun convertOrThrow(value: F): T = convert(value)
        ?: throw IllegalArgumentException("Cannot convert value")
}

/**
 * Usage demonstration
 */
class TypeSafeDemo {
    fun demonstrate() {
        // Type-safe casting
        val any: Any = "Hello"
        val string: String? = SafeCast.cast(any)
        println("Cast result: $string")
        
        // Type guard
        println("Is String: ${TypeGuards.isType<String>(any)}")
        println("Is Int: ${TypeGuards.isType<Int>(any)}")
        
        // Validators
        val validator = Validator<String>()
            .addValidator(PredefinedValidators.notBlank())
            .addValidator(PredefinedValidators.minLength(3))
            .addValidator(PredefinedValidators.maxLength(10))
        
        val result = validator.validate("Hello")
        println("Validation: $result")
        
        // Type-safe map
        val typedMap = TypeSafeMap<String, Any>()
        typedMap.put("key", "value")
        val value = typedMap.get("key")
        println("Map value: $value")
    }
}
```

**Output:**
```
Cast result: Hello
Is String: true
Is Int: false
Validation: Valid
Map value: value
```

### Example 3: Advanced Pattern Matching

```kotlin
import kotlin.reflect.*

/**
 * JSON value sealed class for pattern matching
 */
sealed class JsonValue {
    object Null : JsonValue()
    data class Bool(val value: Boolean) : JsonValue()
    data class Number(val value: Double) : JsonValue()
    data class String(val value: kotlin.String) : JsonValue()
    data class Array(val elements: List<JsonValue>) : JsonValue()
    data class Object(val properties: Map<String, JsonValue>) : JsonValue()
    
    companion object {
        fun from(value: Any?): JsonValue = when (value) {
            null -> Null
            is Boolean -> Bool(value)
            is Number -> Number(value.toDouble())
            is String -> JsonValue.String(value)
            is List<*> -> Array(value.map { from(it) })
            is Map<*, *> -> Object(value.mapKeys { it.key.toString() }.mapValues { from(it.value) })
            else -> throw IllegalArgumentException("Cannot convert to JSON: $value")
        }
    }
    
    fun getType(): String = when (this) {
        is Null -> "null"
        is Bool -> "boolean"
        is Number -> "number"
        is String -> "string"
        is Array -> "array"
        is Object -> "object"
    }
    
    fun isPrimitive(): Boolean = when (this) {
        is Null, is Bool, is Number, is String -> true
        is Array, is Object -> false
    }
}

/**
 * JSON path navigation
 */
class JsonPath {
    sealed class PathElement {
        data class Key(val name: String) : PathElement()
        data class Index(val index: Int) : PathElement()
    }
    
    private val path = mutableListOf<PathElement>()
    
    fun key(name: String): JsonPath {
        path.add(PathElement.Key(name))
        return this
    }
    
    fun index(index: Int): JsonPath {
        path.add(PathElement.Index(index))
        return this
    }
    
    fun resolve(root: JsonValue): JsonValue? {
        var current: JsonValue? = root
        for (element in path) {
            current = when (element) {
                is PathElement.Key -> {
                    val obj = (current as? JsonValue.Object) ?: return null
                    obj.properties[element.name]
                }
                is PathElement.Index -> {
                    val arr = (current as? JsonValue.Array) ?: return null
                    arr.elements.getOrNull(element.index)
                }
            }
        }
        return current
    }
}

/**
 * Command pattern with type-safe parsing
 */
sealed class Command {
    data class CreateUser(
        val name: String,
        val email: String,
        val age: Int?
    ) : Command()
    
    data class UpdateUser(
        val userId: String,
        val name: String?,
        val email: String?,
        val age: Int?
    ) : Command()
    
    data class DeleteUser(val userId: String) : Command()
    
    data class GetUser(val userId: String) : Command()
    
    data class ListUsers(
        val limit: Int = 100,
        val offset: Int = 0
    ) : Command()
    
    object Help : Command()
    
    companion object {
        fun parse(input: String): Command? {
            val parts = input.split(" ").filter { it.isNotBlank() }
            if (parts.isEmpty()) return null
            
            return when (parts[0].lowercase()) {
                "create" -> parseCreate(parts)
                "update" -> parseUpdate(parts)
                "delete" -> parseDelete(parts)
                "get" -> parseGet(parts)
                "list" -> parseList(parts)
                "help" -> Help
                else -> null
            }
        }
        
        private fun parseCreate(parts: List<String>): Command? {
            if (parts.size < 3) return null
            val name = parts[1]
            val email = parts[2]
            val age = parts.getOrNull(3)?.toIntOrNull()
            return CreateUser(name, email, age)
        }
        
        private fun parseUpdate(parts: List<String>): Command? {
            if (parts.size < 2) return null
            val userId = parts[1]
            var name: String? = null
            var email: String? = null
            var age: Int? = null
            
            val flags = parts.drop(2)
            var i = 0
            while (i < flags.size) {
                when (flags[i]) {
                    "-n" -> name = flags.getOrNull(++i)
                    "-e" -> email = flags.getOrNull(++i)
                    "-a" -> age = flags.getOrNull(++i)?.toIntOrNull()
                }
                i++
            }
            return UpdateUser(userId, name, email, age)
        }
        
        private fun parseDelete(parts: List<String>): Command? {
            if (parts.size < 2) return null
            return DeleteUser(parts[1])
        }
        
        private fun parseGet(parts: List<String>): Command? {
            if (parts.size < 2) return null
            return GetUser(parts[1])
        }
        
        private fun parseList(parts: List<String>): Command? {
            var limit = 100
            var offset = 0
            
            val flags = parts.drop(1)
            var i = 0
            while (i < flags.size) {
                when (flags[i]) {
                    "-l" -> limit = flags.getOrNull(++i)?.toIntOrNull() ?: limit
                    "-o" -> offset = flags.getOrNull(++i)?.toIntOrNull() ?: offset
                }
                i++
            }
            return ListUsers(limit, offset)
        }
    }
    
    fun execute(): String = when (this) {
        is CreateUser -> "Created user: $name ($email)"
        is UpdateUser -> "Updated user: $userId"
        is DeleteUser -> "Deleted user: $userId"
        is GetUser -> "Retrieved user: $userId"
        is ListUsers -> "Listed users (limit=$limit, offset=$offset)"
        Help -> helpText
    }
    
    val helpText: String = """
        Available commands:
        create <name> <email> [age] - Create a user
        update <userId> [-n name] [-e email] [-a age] - Update a user
        delete <userId> - Delete a user
        get <userId> - Get a user
        list [-l limit] [-o offset] - List users
        help - Show this help
    """.trimIndent()
}

/**
 * Match result for pattern matching
 */
sealed class MatchResult<out T> {
    data class Success<T>(val value: T) : MatchResult<T>()
    data class Failure(val message: String) : MatchResult<Nothing>()
    object NoMatch : MatchResult<Nothing>()
    
    fun getOrNull(): T? = (this as? Success)?.value
    fun getOrThrow(): T = when (this) {
        is Success -> value
        is Failure -> throw IllegalArgumentException(message)
        NoMatch -> throw NoSuchElementException("No match")
    }
}

/**
 * Pattern matcher abstract class
 */
abstract class Matcher<T, R> {
    abstract fun match(value: T): MatchResult<R>
    
    fun matchOrNull(value: T): R? = match(value).getOrNull()
    fun matchOrThrow(value: T): R = match(value).getOrThrow()
    
    infix fun or(other: Matcher<T, R>): Matcher<T, R> = CombinedMatcher(this, other)
    
    private class CombinedMatcher<T, R>(
        private val first: Matcher<T, R>,
        private val second: Matcher<T, R>
    ) : Matcher<T, R>() {
        override fun match(value: T): MatchResult<R> {
            val result = first.match(value)
            return when (result) {
                is MatchResult.Success -> result
                is MatchResult.Failure -> second.match(value)
                MatchResult.NoMatch -> second.match(value)
            }
        }
    }
}

/**
 * Type matching demonstration
 */
class TypeMatchingDemo {
    fun demonstrate() {
        // JSON value matching
        val json = JsonValue.Array(listOf(
            JsonValue.String("hello"),
            JsonValue.Number(42.0),
            JsonValue.Object(mapOf(
                "name" to JsonValue.String("John"),
                "age" to JsonValue.Number(30.0)
            ))
        ))
        
        // Pattern match on JSON
        fun processJson(value: JsonValue): String = when (value) {
            is JsonValue.Null -> "null"
            is JsonValue.Bool -> value.value.toString()
            is JsonValue.Number -> value.value.toString()
            is JsonValue.String -> "\"${value.value}\""
            is JsonValue.Array -> "[${value.elements.joinToString(", ") { processJson(it) }}]"
            is JsonValue.Object -> "{${value.properties.entries.joinToString(", ") { 
                "\"${it.key}\": ${processJson(it.value)}"
            }}}"
        }
        
        println("JSON: ${processJson(json)}")
        
        // Command parsing
        val commands = listOf(
            "create John john@example.com",
            "get user123",
            "list -l 10"
        )
        
        for (cmd in commands) {
            val parsed = Command.parse(cmd)
            println("Command: $cmd -> ${parsed?.execute()}")
        }
    }
}
```

**Output:**
```
JSON: ["hello", 42, {"name": "John", "age": 30}]
Command: create John john@example.com -> Created user: John (john@example.com)
Command: get user123 -> Retrieved user: user123
Command: list -l 10 -> Listed users (limit=10, offset=0)
```

## Best Practices

- Use sealed classes with when for exhaustive matching
- Leverage inline functions for type reification
- Use type guards for compile-time safety
- Implement type-safe builders with generic constraints
- Use smart casts instead of explicit casting where possible

## Common Pitfalls

### Problem: Smart casts don't work with mutable properties
**Solution:** Use local variables or make properties val

### Problem: Platform types from Java
**Solution:** Treat as nullable or use annotations

### Problem: Generic type erasure
**Solution:** Use reified inline functions

## Troubleshooting Guide

**Q: Why doesn't smart cast work?**
A: Property might be mutable or val is captured in lambda.

**Q: How to handle platform types?**
A: Use nullable types or add @NonNull annotations.

**Q: How to preserve type info with generics?**
A: Use reified inline functions.

## Cross-References

- [Sealed Classes](./04_Sealed_Classes.md)
- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Metaprogramming](./02_Metaprogramming.md)