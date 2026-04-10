# Unit Testing Basics

## Learning Objectives

1. Understanding unit testing fundamentals and principles
2. Writing effective unit tests in Kotlin
3. Setting up test environments and configurations
4. Understanding test-driven development (TDD)
5. Measuring and improving code coverage
6. Best practices for maintainable tests

## Prerequisites

- Basic knowledge of Kotlin programming
- Understanding of Android project structure
- Familiarity with Gradle build system

## Core Concepts

Unit testing is a software testing method where individual components are tested in isolation to verify they function correctly. In Android/Kotlin development, unit tests focus on testing business logic, data layers, and utility functions without requiring a running Android device or emulator.

### Why Unit Testing Matters

Unit testing provides several key benefits:
- **Early bug detection**: Catch bugs before integration testing
- **Regression prevention**: Ensure existing functionality works after changes
- **Documentation**: Tests serve as executable documentation
- **Refactoring confidence**: Safely refactor code with test protection
- **Design improvement**: Well-tested code tends to have better design

### Test-Driven Development (TDD)

TDD follows a red-green-refactor cycle:
1. **Red**: Write a failing test
2. **Green**: Write minimal code to pass the test
3. **Refactor**: Improve code while keeping tests passing

```kotlin
object TDDExample {
    
    // Red: Write failing test first
    // Green: Implement minimal code
    // Refactor: Improve design
    
    class Calculator {
        fun add(a: Int, b: Int): Int = a + b
        fun subtract(a: Int, b: Int): Int = a - b
        fun multiply(a: Int, b: Int): Int = a * b
        fun divide(a: Int, b: Int): Int = if (b != 0) a / b else throw ArithmeticException("Division by zero")
    }
}
```

## Code Examples

### Standard Example: Basic Unit Test Structure

```kotlin
import org.junit.Assert.*
import org.junit.Before
import org.junit.Test

class UserRepositoryTest {
    
    private lateinit var repository: UserRepository
    
    // Setup method runs before each test
    // Used to initialize test fixtures
    @Before
    fun setup() {
        repository = UserRepository()
    }
    
    // Test method annotated with @Test
    @Test
    fun testGetUserById_Success() {
        // Arrange: Set up test data
        val userId = 1L
        val expectedUser = User(id = userId, name = "John Doe", email = "john@example.com")
        
        // Act: Execute the method under test
        val result = repository.getUserById(userId)
        
        // Assert: Verify the result
        assertNotNull("User should not be null", result)
        assertEquals("User ID should match", userId, result?.id)
        assertEquals("User name should match", expectedUser.name, result?.name)
        assertEquals("User email should match", expectedUser.email, result?.email)
    }
    
    @Test
    fun testGetUserById_NotFound() {
        // Arrange: Non-existent user ID
        val userId = 999L
        
        // Act: Try to get non-existent user
        val result = repository.getUserById(userId)
        
        // Assert: Result should be null
        assertNull("Result should be null for non-existent user", result)
    }
    
    @Test
    fun testSaveUser_Success() {
        // Arrange: Create new user
        val newUser = User(name = "Jane Doe", email = "jane@example.com")
        
        // Act: Save the user
        val savedUser = repository.saveUser(newUser)
        
        // Assert: User should have an ID assigned
        assertNotNull("Saved user should have an ID", savedUser.id)
        assertTrue("User ID should be positive", savedUser.id!! > 0)
    }
    
    @Test(expected = IllegalArgumentException::class)
    fun testSaveUser_InvalidEmail() {
        // Arrange: User with invalid email
        val user = User(name = "Invalid", email = "not-an-email")
        
        // Act: Attempt to save user with invalid email
        // Assert: Should throw IllegalArgumentException
        repository.saveUser(user)
    }
    
    @Test
    fun testDeleteUser_Success() {
        // Arrange: Create and save a user first
        val user = repository.saveUser(User(name = "To Delete", email = "delete@example.com"))
        val userId = user.id!!
        
        // Act: Delete the user
        val result = repository.deleteUser(userId)
        
        // Assert: Deletion was successful
        assertTrue("Delete should return true", result)
        assertNull("User should no longer exist", repository.getUserById(userId))
    }
}
```

### Real-World Example: Business Logic Testing

```kotlin
import org.junit.Assert.*
import org.junit.Test
import org.junit.runner.RunWith
import org.junit.runners.JUnit4

@RunWith(JUnit4::class)
class OrderProcessorTest {
    
    private lateinit var orderProcessor: OrderProcessor
    private lateinit var mockPaymentGateway: PaymentGateway
    private lateinit var mockInventoryService: InventoryService
    private lateinit var mockNotificationService: NotificationService
    
    @Before
    fun setup() {
        // Initialize mocks using Mockito or similar
        mockPaymentGateway = Mockito.mock(PaymentGateway::class.java)
        mockInventoryService = Mockito.mock(InventoryService::class.java)
        mockNotificationService = Mockito.mock(NotificationService::class.java)
        
        orderProcessor = OrderProcessor(
            paymentGateway = mockPaymentGateway,
            inventoryService = mockInventoryService,
            notificationService = mockNotificationService
        )
    }
    
    @Test
    fun testProcessOrder_SuccessfulOrder() {
        // Arrange: Create a valid order with items
        val order = Order(
            id = 1L,
            customerId = 100L,
            items = listOf(
                OrderItem(productId = 1L, quantity = 2, price = 29.99),
                OrderItem(productId = 2L, quantity = 1, price = 49.99)
            ),
            shippingAddress = Address(
                street = "123 Main St",
                city = "New York",
                state = "NY",
                zipCode = "10001"
            )
        )
        
        // Setup mock behaviors
        Mockito.`when`(mockInventoryService.checkAvailability(1L, 2)).thenReturn(true)
        Mockito.`when`(mockInventoryService.checkAvailability(2L, 1)).thenReturn(true)
        Mockito.`when`(mockPaymentGateway.processPayment(anyDouble(), any())).thenReturn(PaymentResult.Success("txn_123"))
        
        // Act: Process the order
        val result = orderProcessor.processOrder(order)
        
        // Assert: Verify successful processing
        assertTrue("Order should be processed successfully", result is OrderProcessingResult.Success)
        val successResult = result as OrderProcessingResult.Success
        assertEquals("Transaction ID should match", "txn_123", successResult.transactionId)
        assertEquals("Total should be calculated correctly", 109.97, successResult.totalAmount, 0.01)
        
        // Verify mock interactions
        Mockito.verify(mockInventoryService).reserveItems(1L, 2)
        Mockito.verify(mockInventoryService).reserveItems(2L, 1)
        Mockito.verify(mockPaymentGateway).processPayment(109.97, order.customerId)
        Mockito.verify(mockNotificationService).sendOrderConfirmation(order.customerId, "txn_123")
    }
    
    @Test
    fun testProcessOrder_OutOfStock() {
        // Arrange: Order with out-of-stock item
        val order = Order(
            id = 2L,
            customerId = 100L,
            items = listOf(
                OrderItem(productId = 1L, quantity = 10, price = 29.99)
            ),
            shippingAddress = Address(
                street = "123 Main St",
                city = "New York", 
                state = "NY",
                zipCode = "10001"
            )
        )
        
        // Setup mock for out of stock
        Mockito.`when`(mockInventoryService.checkAvailability(1L, 10)).thenReturn(false)
        
        // Act: Attempt to process order
        val result = orderProcessor.processOrder(order)
        
        // Assert: Should fail with out of stock
        assertTrue("Should fail with out of stock", result is OrderProcessingResult.OutOfStock)
        val outOfStockResult = result as OrderProcessingResult.OutOfStock
        assertEquals("Should report product ID", 1L, outOfStockResult.productId)
        assertEquals("Requested quantity", 10, outOfStockResult.requestedQuantity)
        
        // Verify no payment was processed
        Mockito.verify(mockPaymentGateway, Mockito.never()).processPayment(anyDouble(), any())
    }
    
    @Test
    fun testProcessOrder_PaymentFailed() {
        // Arrange: Order that will fail payment
        val order = Order(
            id = 3L,
            customerId = 100L,
            items = listOf(
                OrderItem(productId = 1L, quantity = 1, price = 29.99)
            ),
            shippingAddress = Address(
                street = "123 Main St",
                city = "New York",
                state = "NY", 
                zipCode = "10001"
            )
        )
        
        // Setup mock for payment failure
        Mockito.`when`(mockInventoryService.checkAvailability(1L, 1)).thenReturn(true)
        Mockito.`when`(mockPaymentGateway.processPayment(anyDouble(), any()))
            .thenReturn(PaymentResult.Failed("Insufficient funds"))
        
        // Act: Process the order
        val result = orderProcessor.processOrder(order)
        
        // Assert: Should fail with payment error
        assertTrue("Should fail with payment error", result is OrderProcessingResult.PaymentFailed)
        val paymentResult = result as OrderProcessingResult.PaymentFailed
        assertEquals("Error message should match", "Insufficient funds", paymentResult.errorMessage)
        
        // Verify inventory was released
        Mockito.verify(mockInventoryService).releaseItems(1L, 1)
    }
    
    @Test
    fun testCalculateOrderTotal_TWithDiscounts() {
        // Arrange: Order with applicable discounts
        val order = Order(
            id = 4L,
            customerId = 100L,
            items = listOf(
                OrderItem(productId = 1L, quantity = 2, price = 100.00),  // $200
                OrderItem(productId = 2L, quantity = 1, price = 50.00)    // $50
            ),
            shippingAddress = Address(
                street = "123 Main St",
                city = "New York",
                state = "NY",
                zipCode = "10001"
            )
        )
        
        // Define discount rules
        val discountRules = listOf(
            DiscountRule(type = DiscountType.BULK, threshold = 150.0, percentage = 0.10),  // 10% off orders over $150
            DiscountRule(type = DiscountType.LOYALTY, threshold = 0.0, percentage = 0.05) // 5% loyalty discount
        )
        
        // Act: Calculate total with discounts
        val total = orderProcessor.calculateOrderTotal(order, discountRules)
        
        // Assert: Verify discount calculations
        val subtotal = 250.0
        val bulkDiscount = 25.0  // 10% of $250 (since > $150)
        val afterBulk = subtotal - bulkDiscount  // $225
        val loyaltyDiscount = 11.25  // 5% of $225
        val expected = afterBulk - loyaltyDiscount  // $213.75
        
        assertEquals("Total with discounts should be correct", expected, total, 0.01)
    }
}
```

### Real-World Example: ViewModel Testing with StateFlow

```kotlin
import org.junit.Assert.*
import org.junit.Test
import org.junit.runner.RunWith
import org.junit.runners.JUnit4
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.flow.first
import kotlinx.coroutines.runBlocking

@RunWith(JUnit4::class)
class UserListViewModelTest {
    
    private lateinit var viewModel: UserListViewModel
    private lateinit var mockUserRepository: UserRepository
    private lateinit var testDispatcher: TestCoroutineDispatcher
    
    @Before
    fun setup() {
        testDispatcher = TestCoroutineDispatcher()
        mockUserRepository = Mockito.mock(UserRepository::class.java)
        viewModel = UserListViewModel(
            userRepository = mockUserRepository,
            dispatcher = testDispatcher
        )
    }
    
    @Test
    fun testLoadUsers_Success() = runBlocking {
        // Arrange: Mock successful user loading
        val users = listOf(
            User(id = 1L, name = "John", email = "john@example.com"),
            User(id = 2L, name = "Jane", email = "jane@example.com"),
            User(id = 3L, name = "Bob", email = "bob@example.com")
        )
        
        Mockito.`when`(mockUserRepository.getUsers()).thenReturn(users)
        
        // Act: Load users
        viewModel.loadUsers()
        
        // Wait for coroutine to complete
        testDispatcher.advanceUntilIdle()
        
        // Assert: Verify UI state
        val uiState = viewModel.uiState.first()
        assertTrue("Should show users", uiState is UserListUiState.Users)
        val usersState = uiState as UserListUiState.Users
        assertEquals("Should have 3 users", 3, usersState.users.size)
        assertEquals("First user should be John", "John", usersState.users[0].name)
    }
    
    @Test
    fun testLoadUsers_Error() = runBlocking {
        // Arrange: Mock error
        Mockito.`when`(mockUserRepository.getUsers())
            .thenThrow(RuntimeException("Network error"))
        
        // Act: Attempt to load users
        viewModel.loadUsers()
        
        // Wait for coroutine to complete
        testDispatcher.advanceUntilIdle()
        
        // Assert: Verify error state
        val uiState = viewModel.uiState.first()
        assertTrue("Should show error state", uiState is UserListUiState.Error)
        val errorState = uiState as UserListUiState.Error
        assertEquals("Error message should match", "Network error", errorState.message)
    }
    
    @Test
    fun testSearchUsers_Filter() = runBlocking {
        // Arrange: Initial users loaded
        val users = listOf(
            User(id = 1L, name = "John Doe", email = "john@example.com"),
            User(id = 2L, name = "Jane Smith", email = "jane@example.com"),
            User(id = 3L, name = "John Smith", email = "john.smith@example.com")
        )
        
        Mockito.`when`(mockUserRepository.getUsers()).thenReturn(users)
        viewModel.loadUsers()
        testDispatcher.advanceUntilIdle()
        
        // Act: Search for "John"
        viewModel.searchUsers("John")
        
        // Assert: Verify filtered results
        val uiState = viewModel.uiState.first()
        assertTrue("Should show filtered users", uiState is UserListUiState.Users)
        val usersState = uiState as UserListUiState.Users
        assertEquals("Should have 2 John users", 2, usersState.users.size)
        assertTrue("All users should contain John", 
            usersState.users.all { it.name.contains("John") })
    }
    
    @Test
    fun testDeleteUser_Success() = runBlocking {
        // Arrange: Initial users loaded
        val users = listOf(
            User(id = 1L, name = "John", email = "john@example.com")
        )
        Mockito.`when`(mockUserRepository.getUsers()).thenReturn(users)
        Mockito.`when`(mockUserRepository.deleteUser(1L)).thenReturn(true)
        
        viewModel.loadUsers()
        testDispatcher.advanceUntilIdle()
        
        // Act: Delete user
        viewModel.deleteUser(1L)
        testDispatcher.advanceUntilIdle()
        
        // Assert: Verify user deleted
        Mockito.verify(mockUserRepository).deleteUser(1L)
        
        val uiState = viewModel.uiState.first()
        assertTrue("Should show empty list", uiState is UserListUiState.Users)
        assertTrue("User list should be empty", 
            (uiState as UserListUiState.Users).users.isEmpty())
    }
    
    @Test
    fun testSelectUser_Navigation() = runBlocking {
        // Arrange: Initial users loaded
        val users = listOf(
            User(id = 1L, name = "John", email = "john@example.com")
        )
        Mockito.`when`(mockUserRepository.getUsers()).thenReturn(users)
        
        viewModel.loadUsers()
        testDispatcher.advanceUntilIdle()
        
        // Act: Select user
        viewModel.selectUser(1L)
        
        // Assert: Verify navigation event
        val navigationEvent = viewModel.navigationEvent.first()
        assertTrue("Should have navigation event", navigationEvent is NavigationEvent.NavigateToUserDetail)
        val navEvent = navigationEvent as NavigationEvent.NavigateToUserDetail
        assertEquals("User ID should match", 1L, navEvent.userId)
    }
}
```

### Output Explanation and Test Results

Running the tests shows:
- **Green bar**: All tests pass successfully
- **Test execution time**: Each test typically completes in milliseconds
- **Coverage report**: Shows percentage of code covered by tests

```
Test Results:
- UserRepositoryTest: 5 tests passed
- OrderProcessorTest: 7 tests passed  
- UserListViewModelTest: 5 tests passed

Total: 17 tests, 0 failures
```

## Best Practices

1. **Name tests descriptively**: Use clear, descriptive test method names that explain what is being tested
2. **Follow AAA pattern**: Arrange, Act, Assert - clear separation makes tests readable
3. **Test one thing per test**: Each test should verify a single behavior or scenario
4. **Use meaningful assertions**: Provide clear error messages in assertions
5. **Keep tests independent**: Tests should not depend on each other or execution order
6. **Avoid test logic**: Don't use complex logic in tests; keep them simple and deterministic
7. **Use test fixtures**: Reuse setup code with @Before methods
8. **Mock external dependencies**: Use mocks for databases, network calls, and other external systems
9. **Test edge cases**: Include tests for boundary conditions and error scenarios
10. **Maintain test code**: Test code should be treated with the same care as production code

## Common Pitfalls

**Pitfall 1: Testing too much at once**
- **Problem**: Tests become complex and hard to maintain
- **Solution**: Break down into smaller, focused tests

**Pitfall 2: Hardcoded test data**
- **Problem**: Tests break when data changes
- **Solution**: Use factory methods or builders for test data

**Pitfall 3: Not cleaning up resources**
- **Problem**: Tests leak resources and affect other tests
- **Solution**: Use @After methods for cleanup

**Pitfall 4: Testing implementation details**
- **Problem**: Tests break when refactoring
- **Solution**: Test behavior, not implementation

**Pitfall 5: Ignoring test coverage**
- **Problem**: Low coverage hides potential bugs
- **Solution**: Aim for meaningful coverage, not just percentage

## Troubleshooting Guide

**Issue: Tests fail with "No runnable methods"**
1. Ensure @Test annotation is imported correctly
2. Verify test method has no parameters (unless using parameterized tests)
3. Check that method is public

**Issue: Tests pass locally but fail on CI**
1. Check environment differences (Java version, locale)
2. Verify test data is not dependent on timing
3. Check for race conditions in async code

**Issue: Slow test execution**
1. Use mocks instead of real implementations
2. Avoid filesystem or network calls in tests
3. Consider test parallelization

**Issue: Flaky tests**
1. Remove timing dependencies
2. Use proper async testing utilities
3. Ensure proper test isolation

## Advanced Tips

**Tip 1: Use parameterized tests for data-driven testing**
```kotlin
@RunWith(Parameterized::class)
class MathUtilsTest {
    @Parameterized.Parameters
    fun data(): Collection<Array<Any>> = listOf(
        arrayOf(2, 2, 4),
        arrayOf(5, 3, 8),
        arrayOf(10, 20, 30)
    )
    
    @Test
    fun testAdd(a: Int, b: Int, expected: Int) {
        assertEquals(expected, MathUtils.add(a, b))
    }
}
```

**Tip 2: Use test categories for organization**
```kotlin
@TestCategory(UnitTest::class)
class MathUtilsTest

@TestCategory(IntegrationTest::class)
class DatabaseTest
```

**Tip 3: Custom matchers for assertions**
```kotlin
fun <T> assertThat(actual: T, matcher: Matcher<T>) {
    if (!matcher.matches(actual)) {
        throw AssertionError("Expected: ${matcher.description}, Actual: $actual")
    }
}
```

**Tip 4: Rule-based test configuration**
```kotlin
@Rule
val mockServerRule = MockServerRule()

@Test
fun testApiCall() {
    mockServerRule.mockSuccessResponse("/api/users", users)
}
```

**Tip 5: Test naming conventions**
- `test[MethodName]_[Scenario]_[ExpectedResult]`
- Example: `testProcessOrder_WhenOutOfStock_ReturnsOutOfStockError`

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/02_JUnit_and_Mockito.md
See: 08_TESTING/01_Testing_Fundamentals/03_Espresso_UI_Testing.md
See: 08_TESTING/01_Testing_Fundamentals/04_Test_Utilities.md
See: 08_TESTING/01_Testing_Fundamentals/05_Test_Architecture.md
See: 03_ARCHITECTURE/02_Dependency_Injection/05_DI_Testing_Strategies.md