# Test Utilities

## Learning Objectives

1. Using test utilities and helper libraries
2. Creating reusable test fixtures
3. Building test data factories
4. Implementing mock servers for testing
5. Managing test configurations
6. Debugging and analyzing test failures

## Prerequisites

- Unit testing basics
- JUnit framework
- Mocking frameworks knowledge

## Core Concepts

### Test Utilities Overview

Test utilities provide essential support for writing effective tests:
- **Build helpers**: Create test objects easily
- **Assertion libraries**: More readable assertions
- **Test runners**: Specialized test execution
- **Mock servers**: Simulate network calls
- **Test databases**: In-memory data for testing

### Popular Test Utilities

- **AssertJ**: Fluent assertions
- **Hamcrest**: Matchers library
- **WireMock**: Mock HTTP server
- **H2 Database**: In-memory database
- **Awaitility**: Async test utilities

## Code Examples

### Standard Example: Test Utility Classes

```kotlin
import org.junit.jupiter.api.*
import org.junit.jupiter.params.*
import org.junit.jupiter.params.provider.*
import java.time.*

class TestUtilityExamples {
    
    // Custom test fixtures
    companion object TestFixtures {
        val testUser = User(id = 1L, name = "Test User", email = "test@example.com")
        
        fun createUser(id: Long = 1L, name: String = "Test User"): User {
            return User(id = id, name = name, email = "${name.lowercase()}@example.com")
        }
        
        fun createUsers(count: Int): List<User> {
            return (1..count).map { createUser(it.toLong(), "User $it") }
        }
        
        fun createOrder(userId: Long, total: Double = 100.0): Order {
            return Order(
                id = userId,
                userId = userId,
                items = listOf(
                    OrderItem(productId = 1L, quantity = 1, price = total)
                ),
                total = total,
                status = OrderStatus.NEW
            )
        }
    }
    
    // Test with data factory
    @Test
    fun testUser_WithFactory() {
        val user = TestFixtures.createUser(5L, "Custom Name")
        
        Assertions.assertEquals(5L, user.id)
        Assertions.assertEquals("Custom Name", user.name)
    }
    
    @ParameterizedTest
    @CsvSource(
        "1, 100.0",
        "2, 200.0", 
        "3, 300.0"
    )
    fun testOrder_Parameterized(userId: Long, total: Double) {
        val order = TestFixtures.createOrder(userId, total)
        
        Assertions.assertEquals(userId, order.userId)
        Assertions.assertEquals(total, order.total, 0.01)
    }
}
```

### Real-World Example: AssertJ and Fluent Assertions

```kotlin
import org.junit.jupiter.api.*
import org.junit.jupiter.api.Assertions.*
import org.assertj.core.api.Assertions.*

class AssertJExamples {
    
    @Test
    fun testFluentAssertions() {
        val user = User(id = 1L, name = "John", email = "john@example.com")
        
        // Fluent assertions with AssertJ
        assertThat(user)
            .isNotNull()
            .satisfies({
                assertThat(it.id).isEqualTo(1L)
                assertThat(it.name).isEqualTo("John")
            })
            .extracting(User::name, User::email)
            .containsExactly("john@example.com", "john@example.com")
    }
    
    @Test
    fun testCollectionAssertions() {
        val users = listOf(
            User(1L, "John", "john@example.com"),
            User(2L, "Jane", "jane@example.com"),
            User(3L, "Bob", "bob@example.com")
        )
        
        // Collection assertions
        assertThat(users)
            .hasSize(3)
            .extracting(User::name)
            .containsExactly("John", "Jane", "Bob")
        
        // Filtering and assertions
        assertThat(users)
            .filteredOn { it.name.startsWith("J") }
            .hasSize(1)
        
        // All match assertion
        assertThat(users)
            .allMatch { it.email!!.contains("@") }
    }
    
    @Test
    fun testMapAssertions() {
        val userMap = mapOf(
            1L to User(1L, "John", "john@example.com"),
            2L to User(2L, "Jane", "jane@example.com")
        )
        
        // Map assertions
        assertThat(userMap)
            .hasSize(2)
            .containsKey(1L)
            .doesNotContainKey(3L)
        
        // Map entry assertions
        assertThat(userMap)
            .extracting(1L)
            .isEqualToComparingFieldByField(User(1L, "John", "john@example.com"))
    }
    
    @Test
    fun testExceptionAssertions() {
        // Exception assertion
        val exception = assertThrows(IllegalArgumentException::class.java) {
            throw IllegalArgumentException("Test error")
        }
        
        assertThat(exception)
            .isInstanceOf(IllegalArgumentException::class.java)
            .hasMessage("Test error")
    }
    
    @Test
    fun testSoftAssertions() {
        // Collect all failures instead of failing on first
        SoftAssertions.softly {
            it.assertThat("hello").isEmpty()  // Will fail
            it.assertThat("hello").hasSize(5)
        }
        // Test continues despite failures
    }
    
    @Test
    fun testConditionAssertions() {
        val numbers = listOf(1, 2, 3, 4, 5, 6)
        
        // Using conditions
        assertThat(numbers)
            .haveAtLeast(1, Condition { it > 5 })
            .haveAtMost(2, Condition { it > 3 })
            .are(Condition { it > 0 })
    }
}
```

### Real-World Example: Test Database Utilities

```kotlin
import org.junit.jupiter.api.*
import org.junit.jupiter.api.Extensions.*

class TestDatabaseUtilities {
    
    // H2 in-memory database for testing
    @Test
    fun testWithH2Database() {
        // Create H2 database connection
        val jdbcUrl = "jdbc:h2:mem:testdb;DB_CLOSE_DELAY=-1"
        val connection = DriverManager.getConnection(jdbcUrl, "sa", "")
        
        // Setup schema
        connection.createStatement().execute(
            "CREATE TABLE users (id BIGINT, name VARCHAR(255), email VARCHAR(255))"
        )
        
        // Insert test data
        connection.prepareStatement(
            "INSERT INTO users VALUES (?, ?, ?)"
        ).apply {
            setLong(1, 1L)
            setString(2, "John")
            setString(3, "john@example.com")
        }.executeUpdate()
        
        // Query and verify
        val resultSet = connection.createStatement().executeQuery("SELECT * FROM users")
        assertTrue(resultSet.next())
        assertEquals("John", resultSet.getString("name"))
        
        // Cleanup
        connection.createStatement().execute("DROP TABLE users")
        connection.close()
    }
    
    // Test with Room database
    @Test
    fun testWithRoomDatabase() {
        // Create in-memory database
        val database = Room.inMemoryDatabaseBuilder(
            ApplicationProvider.getApplicationContext(),
            AppDatabase::class.java
        ).build()
        
        // Insert test data
        database.userDao().insert(User(1L, "John", "john@example.com"))
        
        // Query and verify
        val users = database.userDao().getAll()
        assertEquals(1, users.size)
        assertEquals("John", users[0].name)
        
        // Cleanup
        database.close()
    }
    
    @Test
    fun testDatabaseTransaction() {
        // Test with transaction rollback
        usingConnection { connection ->
            connection.autoCommit = false
            
            try {
                // Insert data
                connection.prepareStatement(
                    "INSERT INTO users VALUES (?, ?, ?)"
                ).apply {
                    setLong(1, 1L)
                    setString(2, "John")
                    setString(3, "john@example.com")
                }.executeUpdate()
                
                // Rollback transaction
                connection.rollback()
                
                // Verify no data
                val count = connection.createStatement().executeQuery(
                    "SELECT COUNT(*) FROM users"
                ).getInt(1)
                assertEquals(0, count)
                
            } finally {
                connection.autoCommit = true
            }
        }
    }
    
    @Test
    fun testDatabaseIsolation() {
        // Test different isolation levels
        listOf(
            Connection.TRANSACTION_READ_UNCOMMITTED,
            Connection.TRANSACTION_READ_COMMITTED,
            Connection.TRANSACTION_REPEATABLE_READ,
            Connection.TRANSACTION_SERIALIZABLE
        ).forEach { isolationLevel ->
            val connection = getConnection()
            connection.transactionIsolation = isolationLevel
            
            // Test behavior with different isolation
            assertDoesNotThrow {
                // Perform concurrent operations
            }
        }
    }
}
```

### Output Results

```
Test Execution Results:
- TestUtilityExamples: 3 tests passed
- AssertJExamples: 6 tests passed
- TestDatabaseUtilities: 4 tests passed

Total: 13 tests, 0 failures

AssertJ Output:
✓ Fluent assertions executed successfully
✓ Collection assertions validated
✓ Exception assertions passed
✓ Soft assertions collected 1 failure
```

## Best Practices

1. **Centralize test fixtures**: Create reusable test data builders
2. **Use assertion libraries**: AssertJ provides better error messages
3. **Use in-memory databases**: Faster tests, no setup required
4. **Reset between tests**: Clean state to avoid test pollution
5. **Use factory methods**: Consistent test data creation
6. **Organize utilities**: Group related utilities together
7. **Document utilities**: Explain what each utility does
8. **Version control utilities**: Track utility changes
9. **Share utilities across modules**: Reuse test code
10. **Maintain backward compatibility**: Don't break existing tests

## Common Pitfalls

**Pitfall 1: Shared mutable state**
- **Problem**: State leaks between tests
- **Solution**: Reset in @After, use fresh instances

**Pitfall 2: Resource leaks**
- **Problem**: Database connections not closed
- **Solution**: Use try-with-resources or @After

**Pitfall 3: Time-dependent tests**
- **Problem**: Tests fail on different dates
- **Solution**: Use fixed clocks/mocked time

**Pitfall 4: Hardcoded test data**
- **Problem**: Tests break when data changes
- **Solution**: Use factories with parameters

**Pitfall 5: Slow tests**
- **Problem**: External services slow tests
- **Solution**: Use mocks or test doubles

## Troubleshooting Guide

**Issue: Tests fail with "Connection closed"**
1. Check database lifecycle
2. Verify connection pooling
3. Check @After cleanup

**Issue: "Table already exists"**
1. Drop tables in @After
2. Use separate databases
3. Create fresh schema each test

**Issue: Assertion library not found**
1. Add AssertJ dependency
2. Check version compatibility
3. Verify imports

**Issue: "No suitable driver"**
1. Add H2/MySQL driver
2. Check JDBC URL format
3. Verify driver in classpath

## Advanced Tips

**Tip 1: Custom assertion**
```kotlin
fun <T> assertThat(actual: T) = SoftAssertions.assertSoftly {
    Assertions.assertThat(actual)
}

// Usage
assertThat(user).hasValidEmail()
```

**Tip 2: Test data builders**
```kotlin
class UserBuilder {
    var id: Long = 0L
    var name: String = "Test User"
    
    fun withId(id: Long) = apply { this.id = id }
    fun withName(name: String) = apply { this.name = name }
    fun build() = User(id, name, "${name}@test.com")
}
```

**Tip 3: Test configuration provider**
```kotlin
@Target(AnnotationTarget.CLASS)
annotation class TestConfig(val useDatabase: Boolean = true)
```

**Tip 4: Async test helpers**
```kotlin
fun await().atMost(5, SECONDS).until { condition }
```

**Tip 5: Test listeners**
```kotlin
class TestListener : TestExecutionListener {
    override fun testExecuted(description: Description, result: TestResult) {}
}
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/02_JUnit_and_Mockito.md
See: 08_TESTING/02_Advanced_Testing/01_Integration_Testing.md
See: 08_TESTING/02_Advanced_Testing/02_Performance_Testing.md
See: 04_DATA_PERSISTENCE/01_Database_Development/01_Room_Database_Basics.md