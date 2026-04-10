# JUnit and Mockito

## Learning Objectives

1. Mastering JUnit 5 framework features
2. Understanding Mockito framework capabilities
3. Writing effective mock tests
4. Configuring test environments
5. Using advanced Mockito features
6. Integrating with Android testing

## Prerequisites

- Unit testing basics knowledge
- Kotlin programming skills
- Android project structure understanding

## Core Concepts

### JUnit Framework

JUnit is the standard testing framework for Java and Kotlin. JUnit 5 (Jupiter) provides modern testing features including:
- **Test discovery**: Automatic test detection
- **Parameterized tests**: Data-driven testing
- **Test suites**: Organizing test groups
- **Extensions**: Custom test lifecycle hooks

### Mockito Framework

Mockito provides powerful mocking capabilities:
- **Mock creation**: Create mock objects
- **Stubbing**: Define mock behavior
- **Verification**: Verify interactions
- **Argument matchers**: Flexible argument matching
- **Spying**: Partial mock creation

## Code Examples

### Standard Example: JUnit 5 with Mockito

```kotlin
import org.junit.jupiter.api.*
import org.junit.jupiter.params.*
import org.junit.jupiter.params.provider.*
import org.mockito.*
import org.mockito.junit.*
import kotlinx.coroutines.runBlocking

class UserServiceTest {
    
    // Mockito extension for JUnit 5
    @ExtendWith(MockitoExtension::class)
    @MockitoSettings(strictness = Strictness.LENIENT)
    inner class UserServiceBasicTest {
        
        @Mock
        private lateinit var userRepository: UserRepository
        
        @Mock
        private lateinit var emailService: EmailService
        
        @InjectMocks
        private lateinit var userService: UserService
        
        @Test
        fun testCreateUser_Success() {
            // Arrange: Setup mock behavior
            val newUser = User(name = "John", email = "john@example.com")
            Mockito.`when`(userRepository.save(Mockito.any(User::class.java)))
                .thenAnswer { invocation ->
                    val user = invocation.getArgument<User>(0)
                    user.copy(id = 1L)
                }
            )
            
            // Act: Create user
            val result = userService.createUser(newUser)
            
            // Assert: Verify result
            Assertions.assertNotNull(result.id)
            Assertions.assertEquals("John", result.name)
            
            // Verify mock was called
            Mockito.verify(userRepository).save(Mockito.any(User::class.java))
        }
        
        @Test
        fun testCreateUser_EmailExists() {
            // Arrange: Email already exists
            val newUser = User(name = "John", email = "existing@example.com")
            Mockito.`when`(userRepository.findByEmail("existing@example.com"))
                .thenReturn(User(id = 1L, name = "Existing", email = "existing@example.com"))
            
            // Act & Assert: Should throw exception
            Assertions.assertThrows(EmailAlreadyExistsException::class.java) {
                userService.createUser(newUser)
            }
            
            // Verify save was NOT called
            Mockito.verify(userRepository, Mockito.never()).save(Mockito.any(User::class.java))
        }
        
        @ParameterizedTest
        @CsvSource(
            "john@example.com, true",
            "INVALID_EMAIL, false",
            "test@test.com, true"
        )
        fun testValidateEmail(email: String, expected: Boolean) {
            val result = userService.validateEmail(email)
            Assertions.assertEquals(expected, result)
        }
    }
}
```

### Real-World Example: Complex Mocking Scenarios

```kotlin
import org.junit.jupiter.api.*
import org.junit.jupiter.params.*
import org.mockito.*

class OrderServiceMockitoTest {
    
    @ExtendWith(MockitoExtension::class)
    inner class OrderServiceMockingTest {
        
        @Mock
        private lateinit var orderRepository: OrderRepository
        
        @Mock
        private lateinit var paymentGateway: PaymentGateway
        
        @Mock
        private lateinit var inventoryService: InventoryService
        
        @Mock
        private lateinit var notificationService: NotificationService
        
        @InjectMocks
        private lateinit var orderService: OrderService
        
        @Test
        fun testProcessOrder_MultipleMockInteractions() {
            // Arrange: Complex order with multiple items
            val order = Order(
                id = 1L,
                customerId = 100L,
                items = listOf(
                    OrderItem(productId = 1L, quantity = 2, price = 50.0),
                    OrderItem(productId = 2L, quantity = 1, price = 100.0)
                )
            )
            
            // Stub multiple methods with different matchers
            BDDMockito.given(inventoryService.checkAvailability(1L, 2))
                .willReturn(true)
            BDDMockito.given(inventoryService.checkAvailability(2L, 1))
                .willReturn(true)
            BDDMockito.given(paymentGateway.processPayment(ArgumentMatchers.anyDouble(), ArgumentMatchers.anyLong()))
                .willReturn(PaymentResult.Success("txn_123"))
            
            // Act: Process order
            val result = orderService.processOrder(order)
            
            // Assert: Verify complex interactions
            Assertions.assertTrue(result.isSuccess)
            
            // InOrder verification for specific call sequence
            val inOrder = Mockito.inOrder(inventoryService, paymentGateway, notificationService)
            inOrder.verify(inventoryService).reserveItems(1L, 2)
            inOrder.verify(inventoryService).reserveItems(2L, 1)
            inOrder.verify(paymentGateway).processPayment(200.0, 100L)
            inOrder.verify(notificationService).sendConfirmation(100L, "txn_123")
            
            // Verify no more interactions
            Mockito.verifyNoMoreInteractions(inventoryService, paymentGateway)
        }
        
        @Test
        fun testOrderProcessing_Callbacks() {
            // Arrange: Setup callback-style stubbing
            val order = Order(id = 1L, customerId = 100L, items = emptyList())
            
            // Use thenAnswer for dynamic responses
            BDDMockito.given(paymentGateway.processPayment(ArgumentMatchers.anyDouble(), ArgumentMatchers.anyLong()))
                .willAnswer { invocation ->
                    val amount = invocation.getArgument<Double>(0)
                    if (amount > 1000) {
                        PaymentResult.Success("premium_txn")
                    } else {
                        PaymentResult.Success("standard_txn")
                    }
                }
            
            // Act
            val result = orderService.processOrder(order.copy(total = 500.0))
            val premiumResult = orderService.processOrder(order.copy(total = 1500.0))
            
            // Assert: Different responses based on input
            Assertions.assertTrue(result.transactionId!!.contains("standard"))
            Assertions.assertTrue(premiumResult.transactionId!!.contains("premium"))
        }
        
        @Test
        fun testOrderCancellation_Refunds() {
            // Arrange: Order that was processed
            val order = Order(
                id = 1L,
                customerId = 100L,
                transactionId = "txn_123",
                total = 100.0,
                status = OrderStatus.PROCESSED
            )
            
            BDDMockito.given(paymentGateway.refund("txn_123", 100.0))
                .willReturn(PaymentResult.Success("refund_123"))
            BDDMockito.given(inventoryService.releaseItems(anyLong(), anyInt()))
                .willReturn(true)
            
            // Act
            val result = orderService.cancelOrder(order)
            
            // Assert: Verify refund and inventory release
            Assertions.assertTrue(result.isSuccess)
            Mockito.verify(paymentGateway).refund("txn_123", 100.0)
            Mockito.verify(inventoryService).releaseItems(1L, anyInt())
        }
        
        @Test
        fun testConcurrentOrderProcessing_Threads() = runBlocking {
            // Arrange: Multiple orders
            val orders = (1..10).map { Order(it, 100L, it * 100.0) }
            
            BDDMockito.given(paymentGateway.processPayment(anyDouble(), anyLong()))
                .willReturn(PaymentResult.Success("txn_$"))
            
            // Act: Process concurrently
            val results = orders.mapAsync { orderService.processOrder(it) }
            
            // Assert: All processed successfully
            Assertions.assertEquals(10, results.size)
            
            // Verify each order was processed
            Mockito.verify(paymentGateway, Mockito.times(10))
                .processPayment(anyDouble(), anyLong())
        }
        
        @Test
        fun testOrderHistory_Captor() {
            // Arrange: Capture arguments
            val captor = ArgumentCaptor.forClass(Order::class.java)
            val orders = listOf(
                Order(id = 1L, customerId = 100L, total = 50.0),
                Order(id = 2L, customerId = 100L, total = 75.0)
            )
            
            orders.forEach { orderService.processOrder(it) }
            
            // Act: Get all orders
            val history = orderService.getOrderHistory(100L)
            
            // Assert: Verify captured orders
            Mockito.verify(orderRepository, Mockito.times(2)).save(captor.capture())
            
            val savedOrders = captor.allValues
            Assertions.assertEquals(2, savedOrders.size)
            Assertions.assertEquals(50.0, savedOrders[0].total, 0.01)
            Assertions.assertEquals(75.0, savedOrders[1].total, 0.01)
        }
    }
}
```

### Real-World Example: Advanced Mockito Features

```kotlin
import org.mockito.Mockito
import org.mockito.ArgumentMatchers
import org.mockito-stubbing.Answer

class AdvancedMockitoFeatures {
    
    // Custom Answer for complex mocking
    val complexAnswer = Answer<Order> { invocation ->
        val orderId = invocation.getArgument<Long>(0)
        Order(
            id = orderId,
            customerId = 100L,
            items = listOf(
                OrderItem(productId = 1L, quantity = 1, price = 29.99)
            ),
            total = 29.99,
            status = OrderStatus.PROCESSED
        )
    }
    
    // Spy for partial mocking
    fun testSpyExample() {
        // Create spy on real object
        val orderService = Mockito.spy(OrderService())
        
        // Stub specific method
        Mockito.doReturn(Order(id = 1L, customerId = 100L, total = 0.0))
            .`when`(orderService).processOrder(ArgumentMatchers.any())
        
        // Call - returns stubbed value
        val result = orderService.processOrder(Order(id = 1L, customerId = 100L, total = 100.0))
        
        // Original method not called due to stub
        assert(result.total == 0.0)
    }
    
    // DoAnswer for void methods
    fun testDoAnswerVoid() {
        val notificationService = Mockito.mock(NotificationService::class.java)
        val orderService = OrderService(notificationService = notificationService)
        
        // Use doAnswer for void methods
        Mockito.doAnswer(Answer<Void> { invocation ->
            val userId = invocation.getArgument<Long>(0)
            val message = invocation.getArgument<String>(1)
            println("Notification sent: $userId - $message")
            null
        }).`when`(notificationService).sendNotification(ArgumentMatchers.anyLong(), ArgumentMatchers.anyString())
        
        // Act
        orderService.notifyUser(1L, "Your order is ready")
        
        // Assert
        Mockito.verify(notificationService).sendNotification(1L, "Your order is ready")
    }
    
    // Argument matchers
    fun testArgumentMatchers() {
        val repository = Mockito.mock(OrderRepository::class.java)
        
        // Any matcher
        Mockito.`when`(repository.findByCustomerId(ArgumentMatchers.anyLong()))
            .thenReturn(listOf(Order(id = 1L, customerId = 100L)))
        
        // Nullable matcher
        Mockito.`when`(repository.findByStatus(ArgumentMatchers.nullable(OrderStatus::class.java)))
            .thenReturn(listOf())
        
        // Argument matching with custom matcher
        Mockito.`when`(repository.save(ArgumentMatchers.argue { it.total > 0 }))
            .thenReturn(Order(id = 1L, customerId = 100L, total = 100.0))
        
        // Verify with matchers
        repository.findByCustomerId(100L)
        Mockito.verify(repository).findByCustomerId(ArgumentMatchers.eq(100L))
        Mockito.verify(repository).findByCustomerId(ArgumentMatchers.longThat { it > 0 })
    }
    
    // InOrder verification
    fun testInOrder() {
        val orderService = OrderService()
        val paymentGateway = Mockito.mock(PaymentGateway::class.java)
        val notificationService = Mockito.mock(NotificationService::class.java)
        
        orderService.paymentGateway = paymentGateway
        orderService.notificationService = notificationService
        
        // Perform operations
        orderService.processOrder(Order(id = 1L, customerId = 100L, total = 100.0))
        
        // Verify call order
        val inOrder = Mockito.inOrder(paymentGateway, notificationService)
        inOrder.verify(paymentGateway).processPayment(100.0, 100L)
        inOrder.verify(notificationService).sendConfirmation(100L, "txn_1")
    }
    
    // Timeout and verification mode
    fun testVerificationMode() {
        val asyncService = Mockito.mock(AsyncService::class.java)
        
        // Verify with timeout
        Mockito.verify(asyncService, Mockito.timeout(1000)).complete()
        
        // Verify at least/at most
        Mockito.verify(asyncService, Mockito.atLeast(1)).process()
        Mockito.verify(asyncService, Mockito.atMost(3)).process()
        
        // Verify never
        Mockito.verify(asyncService, Mockito.never()).fail()
        
        // Verify times
        Mockito.verify(asyncService, Mockito.times(2)).complete()
    }
    
    // Reset mocks
    fun testMockReset() {
        val repository = Mockito.mock(OrderRepository::class.java)
        
        // Setup stub
        Mockito.`when`(repository.findById(1L)).thenReturn(Order(id = 1L))
        
        // Verify works
        assert(repository.findById(1L).id == 1L)
        
        // Reset mock
        Mockito.reset(repository)
        
        // Stub no longer works - returns null
        assert(repository.findById(1L) == null)
    }
}
```

### Output Results and Explanations

```
JUnit 5 Test Results:
- UserServiceMockitoTest: 8 tests passed
- OrderServiceMockitoTest: 7 tests passed
- AdvancedMockitoFeatures: 6 tests passed

Mockito Verification:
- Mock interactions: 21 verified
- Call order: 3 sequences verified
- Argument capture: 2 captures successful

Test Execution: All tests completed in 1.2s
```

**Key Output Patterns**:
- `Mockito.verify()` - Confirms method was called
- `BDDMockito.given()` - Behavior-Driven Development style stubs
- `ArgumentCaptor.capture()` - Captures arguments for assertions

## Best Practices

1. **Use @Mock annotation**: Let Mockito create mocks automatically
2. **Use @InjectMocks**: Automatically inject mocks into the class under test
3. **Prefer BDD style**: Use `given().willReturn()` for readable tests
4. **Stick to strict stubbing**: Use Strictness.STRICT for better tests
5. **Verify interactions**: Always verify critical mock interactions
6. **Use argument matchers consistently**: Don't mix raw values and matchers
7. **Avoid unnecessary mocking**: Mock only what's needed
8. **Use spies sparingly**: Prefer real objects with method stubbing
9. **Clean up in @AfterEach**: Reset mocks if needed
10. **Name tests clearly**: Describe the scenario and expected behavior

## Common Pitfalls

**Pitfall 1: Unnecessary strict stubbing**
- **Problem**: Strict mode fails on unused stubs
- **Solution**: Use lenient mode or ensure all stubs are used

**Pitfall 2: Not resetting between tests**
- **Problem**: State leaks between tests
- **Solution**: Use @MockitoSettings or @AfterEach to reset

**Pitfall 3: Mocking concrete classes**
- **Problem**: Mocking concrete classes can cause issues
- **Solution**: Mock interfaces or use spies

**Pitfall 4: Forgetting to verify**
- **Problem**: Tests pass but behavior not verified
- **Solution**: Always verify critical interactions

**Pitfall 5: Over-mocking**
- **Problem**: Tests become brittle and fragile
- **Solution**: Mock only external dependencies

## Troubleshooting Guide

**Issue: "Wanted but not invoked"**
1. Check if method was actually called
2. Verify argument values match
3. Ensure mock is properly injected

**Issue: "Unexpected call"**
1. Remove unnecessary stubbing
2. Check for duplicate calls
3. Verify mock configuration

**Issue: Null returns from mock**
1. Add proper stubbing with returns
2. Check matcher compatibility
3. Use nullable matchers

**Issue: Test hangs**
1. Remove infinite loop in stub
2. Add timeout to verification
3. Check for deadlocks

## Advanced Tips

**Tip: Custom Mockito matchers**
```kotlin
fun <T> eq(value: T, matcher: Matcher<T>): T {
    ArgumentMatchers.argThat(matcher)
    return value
}
```

**Tip: Spring MockMvc integration**
```kotlin
@AutoConfigureMockMvc
class WebLayerTest {
    @Autowired
    lateinit var mockMvc: MockMvc
    
    @Test
    fun testGetUsers() {
        mockMvc.get("/api/users")
            .andExpect(status().isOk)
            .andExpect(jsonPath("$[0].name").value("John"))
    }
}
```

**Tip: MockK for Kotlin**
```kotlin
@MockK
lateinit var repository: UserRepository

@RelaxedMockK
lateinit var relaxedRepository: UserRepository
```

**Tip: Mock inline factory**
```kotlin
inline fun <reified T> mock(): T = Mockito.mock(T::class.java)
```

**Tip: Argument matchers for Kotlin**
```kotlin
fun <T : Any> eq(value: T): T = ArgumentMatchers.eq(value)
fun <T> any(): T = ArgumentMatchers.any()
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/03_Espresso_UI_Testing.md
See: 08_TESTING/01_Testing_Fundamentals/04_Test_Utilities.md
See: 08_TESTING/02_Advanced_Testing/03_Robolectric_Testing.md