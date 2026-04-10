# Integration Testing

## Learning Objectives

1. Understanding integration testing fundamentals
2. Testing component interactions
3. Setting up integration test environments
4. Testing database operations
5. Testing network calls
6. Managing integration test data

## Prerequisites

- Unit testing basics
- Mocking frameworks
- Database basics
- Network API knowledge

## Core Concepts

### Integration Testing Overview

Integration tests verify that components work correctly together:
- **Component interactions**: Test how modules communicate
- **Database operations**: Test data persistence
- **Network calls**: Test API integrations
- **File operations**: Test file system interactions

### Integration vs Unit Tests

- **Unit tests**: Test single components in isolation
- **Integration tests**: Test multiple components together

## Code Examples

### Standard Example: Database Integration Tests

```kotlin
import org.junit.jupiter.api.*
import org.junit.jupiter.api.Assertions.*
import org.junit.test.junit.*

@TestMethodOrder(MethodOrderer.OrderAnnotation::class)
class DatabaseIntegrationTest {
    
    private lateinit var database: AppDatabase
    private lateinit var userDao: UserDao
    private lateinit var orderDao: OrderDao
    
    @BeforeAll
    @JvmStatic
    fun setupDatabase() {
        // Create in-memory database for testing
        database = Room.inMemoryDatabaseBuilder(
            ApplicationProvider.getApplicationContext(),
            AppDatabase::class.java
        ).build()
        
        userDao = database.userDao()
        orderDao = database.orderDao()
    }
    
    @AfterAll
    @JvmStatic
    fun tearDownDatabase() {
        database.close()
    }
    
    @Test
    @Order(1)
    fun testInsertUser_Success() {
        val user = User(name = "John", email = "john@example.com")
        
        // Insert user
        val id = userDao.insert(user)
        
        // Verify insertion
        val retrieved = userDao.findById(id)
        
        assertNotNull(retrieved)
        assertEquals("John", retrieved!!.name)
        assertEquals("john@example.com", retrieved.email)
    }
    
    @Test
    @Order(2)
    fun testInsertOrder_WithUser() {
        // Get existing user
        val user = userDao.findAll().first()
        
        // Create order for user
        val order = Order(
            userId = user.id,
            items = listOf(
                OrderItem(productId = 1L, quantity = 2, price = 29.99)
            ),
            total = 59.98,
            status = OrderStatus.NEW
        )
        
        // Insert order
        orderDao.insert(order)
        
        // Verify order
        val orders = orderDao.findByUserId(user.id)
        
        assertEquals(1, orders.size)
        assertEquals(59.98, orders[0].total, 0.01)
    }
    
    @Test
    @Order(3)
    fun testGetUserOrders_Join() {
        // Test complex join query
        val userOrders = userDao.getUserWithOrders().first()
        
        assertNotNull(userOrders)
        assertTrue(userOrders.orders.isNotEmpty())
    }
    
    @Test
    fun testTransaction_Rollback() {
        // Test transaction behavior
        database.runInTransaction {
            // Insert user
            val userId = userDao.insert(User(name = "Test", email = "test@example.com"))
            
            // Insert invalid order (will fail)
            try {
                orderDao.insert(Order(userId = 999L, total = 100.0, status = OrderStatus.NEW))
                fail("Expected exception")
            } catch (e: Exception) {
                // Expected - transaction should rollback
            }
        }
        
        // Verify user was not inserted
        val user = userDao.findByEmail("test@example.com")
        assertNull(user)
    }
}
```

### Real-World Example: Network Integration Tests

```kotlin
import org.junit.jupiter.api.*
import okhttp3.mock.*
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class NetworkIntegrationTest {
    
    private lateinit var apiService: ApiService
    private lateinit var mockServer: MockWebServer
    
    @BeforeEach
    fun setup() {
        // Setup mock server
        mockServer = MockWebServer()
        mockServer.start()
        
        // Create Retrofit instance
        apiService = Retrofit.Builder()
            .baseUrl(mockServer.url("/"))
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(ApiService::class.java)
    }
    
    @AfterEach
    fun tearDown() {
        mockServer.shutdown()
    }
    
    @Test
    fun testGetUsers_Success() {
        // Arrange: Mock successful response
        mockServer.enqueue(MockResponse()
            .setResponseCode(200)
            .setBody("""
                [
                    {"id": 1, "name": "John", "email": "john@example.com"},
                    {"id": 2, "name": "Jane", "email": "jane@example.com"}
                ]
            """.trimIndent())
        )
        
        // Act: Make API call
        val users = apiService.getUsers().execute()
        
        // Assert: Verify response
        assertTrue(users.isSuccessful)
        assertEquals(2, users.body()!!.size)
        assertEquals("John", users.body()!![0].name)
    }
    
    @Test
    fun testGetUser_NotFound() {
        // Arrange: Mock 404 response
        mockServer.enqueue(MockResponse()
            .setResponseCode(404)
            .setBody("User not found")
        )
        
        // Act: Make API call
        val response = apiService.getUser(999L).execute()
        
        // Assert: Verify 404
        assertEquals(404, response.code())
    }
    
    @Test
    fun testCreateUser_Success() {
        // Arrange: Mock created response
        mockServer.enqueue(MockResponse()
            .setResponseCode(201)
            .setBody("""{"id": 3, "name": "New User"}""")
        )
        
        // Create new user
        val newUser = CreateUserRequest(name = "New User", email = "new@example.com")
        
        // Act: Make API call
        val response = apiService.createUser(newUser).execute()
        
        // Assert: Verify Created
        assertEquals(201, response.code())
        assertEquals(3, response.body()!!.id)
    }
    
    @Test
    fun testUpdateUser_Success() {
        // Arrange: Mock successful update
        mockServer.enqueue(MockResponse()
            .setResponseCode(200)
            .setBody("""{"id": 1, "name": "Updated Name"}""")
        )
        
        // Update user
        val updateRequest = UpdateUserRequest(name = "Updated Name")
        
        // Act: Make API call
        val response = apiService.updateUser(1L, updateRequest).execute()
        
        // Assert: Verify update
        assertEquals(200, response.code())
        assertEquals("Updated Name", response.body()!!.name)
    }
    
    @Test
    fun testDeleteUser_Success() {
        // Arrange: Mock successful deletion
        mockServer.enqueue(MockResponse()
            .setResponseCode(204)
        )
        
        // Act: Make API call
        val response = apiService.deleteUser(1L).execute()
        
        // Assert: Verify deletion
        assertEquals(204, response.code())
    }
    
    @Test
    fun testNetworkError() {
        // Arrange: Mock network error
        mockServer.enqueue(MockResponse()
            .setResponseCode(500)
            .setBody("Server error")
        )
        
        // Act & Assert: Handle error
        try {
            apiService.getUsers().execute()
            fail("Expected IOException")
        } catch (e: Exception) {
            assertTrue(e is IOException)
        }
    }
}
```

### Real-World Example: End-to-End Integration

```kotlin
import org.junit.jupiter.api.*

class EndToEndIntegrationTest {
    
    private lateinit var app: Application
    private lateinit var database: AppDatabase
    private lateinit var apiService: ApiService
    
    @BeforeEach
    fun setup() {
        // Initialize full application
        app = ApplicationProvider.getApplicationContext()
        
        // Setup dependencies
        database = Room.databaseBuilder(
            app,
            AppDatabase::class.java,
            "app-database"
        ).build()
        
        apiService = Retrofit.Builder()
            .baseUrl("https://api.example.com/")
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(ApiService::class.java)
    }
    
    @AfterEach
    fun tearDown() {
        database.close()
    }
    
    @Test
    fun testFullUserFlow() {
        // 1. Create user locally
        val userId = database.userDao().insert(
            User(name = "John", email = "john@example.com")
        )
        
        // 2. Sync user to server
        val response = apiService.createUser(
            CreateUserRequest(name = "John", email = "john@example.com")
        ).execute()
        
        assertTrue(response.isSuccessful)
        val serverId = response.body()!!.id
        
        // 3. Update local record with server ID
        database.userDao().updateServerId(userId, serverId)
        
        // 4. Verify sync
        val syncedUser = database.userDao().findById(userId)
        assertEquals(serverId, syncedUser.serverId)
    }
    
    @Test
    fun testOrderPlacementFlow() {
        // 1. Get or create user
        val user = database.userDao().findAll().firstOrNull()
            ?: User(name = "Test", email = "test@example.com").also {
                database.userDao().insert(it)
            }
        
        // 2. Create order
        val order = Order(
            userId = user.id,
            items = listOf(
                OrderItem(productId = 1L, quantity = 1, price = 29.99)
            ),
            total = 29.99,
            status = OrderStatus.NEW
        )
        
        val orderId = database.orderDao().insert(order)
        
        // 3. Process payment (simulated)
        val paymentResult = processPayment(order)
        assertTrue(paymentResult)
        
        // 4. Update order status
        database.orderDao().updateStatus(orderId, OrderStatus.PROCESSED)
        
        // 5. Verify final state
        val processedOrder = database.orderDao().findById(orderId)
        assertEquals(OrderStatus.PROCESSED, processedOrder.status)
    }
    
    private fun processPayment(order: Order): Boolean {
        // Simulated payment processing
        return order.total > 0
    }
}
```

### Output Results

```
Integration Test Results:
- DatabaseIntegrationTest: 4 tests passed
- NetworkIntegrationTest: 6 tests passed
- EndToEndIntegrationTest: 2 tests passed

Test Execution:
- User flow: 1.2s
- Order flow: 0.8s
- Network tests: 2.1s
```

## Best Practices

1. **Use test databases**: Use in-memory databases for fast tests
2. **Mock external services**: Use mock servers for network tests
3. **Clean up data**: Delete test data after tests
4. **Use transactions**: Rollback changes after tests
5. **Isolate tests**: Each test should be independent
6. **Test realistic scenarios**: Test real user flows
7. **Verify state changes**: Check database state
8. **Use proper assertions**: Verify complete states
9. **Manage dependencies**: Set up correctly
10. **Use testcontainers**: For complex database testing

## Common Pitfalls

**Pitfall 1: Shared test data**
- **Problem**: Tests interfere with each other
- **Solution**: Use @Before to setup fresh data

**Pitfall 2: Network dependencies**
- **Problem**: Tests fail without network
- **Solution**: Use mock servers

**Pitfall 3: Database state**
- **Problem**: Stale data causes failures
- **Solution**: Clean database in @After

**Pitfall 4: Slow tests**
- **Problem**: Integration tests are slow
- **Solution**: Use mocks where possible

**Pitfall 5: Complex setup**
- **Problem**: Too much setup code
- **Solution**: Use test fixtures

## Troubleshooting Guide

**Issue: "Database locked"**
1. Close database connections properly
2. Use single connection for tests
3. Check for open transactions

**Issue: "Connection refused"**
1. Check mock server startup
2. Verify base URL matches
3. Check port availability

**Issue: "Test timeout"**
1. Increase timeout
2. Check for deadlocks
3. Optimize queries

## Advanced Tips

**Tip 1: Testcontainers**
```kotlin
// Use Testcontainers for database testing
val container = MySQLContainer("mysql:8.0")
container.start()
// Use container.jdbcUrl for connection
```

**Tip 2: WireMock stateful behavior**
```kotlin
wireMockServer.stubFor(post("/api/orders")
    .willReturn(aResponse()
        .withStatus(200)
        .withBody("created")))
```

**Tip 3: Database seeding**
```kotlin
databaseCallback.onCreate(database)
seedingCallback.seed(database)
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/02_JUnit_and_Mockito.md
See: 08_TESTING/02_Advanced_Testing/03_Robolectric_Testing.md
See: 08_TESTING/02_Advanced_Testing/05_Test_Reporting.md
See: 04_DATA_PERSISTENCE/01_Database_Development/01_Room_Database_Basics.md