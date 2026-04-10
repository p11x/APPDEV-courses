# Test Architecture

## Learning Objectives

1. Designing testable architectures
2. Implementing test automation patterns
3. Building test pyramid strategies
4. Creating maintainable test structures
5. Managing test dependencies
6. Implementing CI/CD test pipelines

## Prerequisites

- Unit testing fundamentals
- Mocking frameworks
- Build systems
- CI/CD basics

## Core Concepts

### Test Architecture Overview

Test architecture refers to the structure and organization of tests:
- **Test organization**: How tests are grouped and named
- **Test dependencies**: How tests relate to code
- **Test automation**: Automated test execution
- **Test reporting**: Results and metrics

### Test Pyramid

The test pyramid guides test distribution:
- **Unit tests**: 70% - Fast, focused tests
- **Integration tests**: 20% - Component interaction tests
- **UI tests**: 10% - End-to-end tests

## Code Examples

### Standard Example: Testable Architecture

```kotlin
import android.archtecture.common.*
import android.archtecture.data.*
import android.archtecture.domain.*

// Clean Architecture Layers

// 1. Domain Layer - Pure business logic (easily testable)
class OrderProcessor {
    
    fun processOrder(order: Order): Result<OrderProcessingResult> {
        // Validate order
        if (!order.isValid()) {
            return Result.failure(InvalidOrderError("Invalid order"))
        }
        
        // Calculate totals
        val total = order.calculateTotal()
        
        // Apply discounts
        val finalTotal = applyDiscounts(order, total)
        
        return Result.success(
            OrderProcessingResult(
                orderId = order.id,
                total = finalTotal,
                status = OrderStatus.PROCESSED
            )
        )
    }
    
    private fun Order.calculateTotal(): Double {
        return items.sumOf { it.price * it.quantity }
    }
    
    private fun applyDiscounts(order: Order, total: Double): Double {
        var result = total
        // Apply business rules
        if (total > 100) result -= 10  // $10 off orders over $100
        if (order.customer.isVIP) result *= 0.9  // 10% VIP discount
        return result
    }
    
    private fun Order.isValid(): Boolean {
        return items.isNotEmpty() && 
               items.all { it.quantity > 0 } &&
               customerId != null
    }
}

// 2. Data Layer - Repository with interface
interface OrderRepository {
    suspend fun save(order: Order): Order
    suspend fun findById(id: Long): Order?
    suspend fun findByCustomer(customerId: Long): List<Order>
    suspend fun delete(id: Long): Boolean
}

// 3. Use cases - Application business logic
class ProcessOrderUseCase(
    private val orderProcessor: OrderProcessor,
    private val orderRepository: OrderRepository,
    private val notificationService: NotificationService
) {
    suspend operator fun invoke(order: Order): Result<Order> {
        // Process order
        val result = orderProcessor.processOrder(order)
        
        return result.fold(
            onSuccess = { processingResult ->
                // Save to repository
                val savedOrder = orderRepository.save(
                    order.copy(
                        status = processingResult.status,
                        total = processingResult.total
                    )
                )
                
                // Send notification
                notificationService.sendOrderConfirmation(savedOrder)
                
                Result.success(savedOrder)
            },
            onFailure = { Result.failure(it) }
        )
    }
}
```

### Real-World Example: Test Architecture Implementation

```kotlin
// Test Architecture Components

// 1. Test Base Classes
abstract class BaseTest {
    
    @BeforeEach
    fun baseSetup() {
        // Setup common test resources
        initializeTestClock()
        initializeTestDispatcher()
    }
    
    @AfterEach
    fun baseTeardown() {
        // Cleanup common resources
        cleanupTestClock()
        cleanupTestDispatcher()
    }
    
    protected fun runBlockingTest(block: suspend () -> Unit) {
        runBlocking { block() }
    }
}

// 2. Test Fixtures
object TestFixtures {
    
    val defaultUser = User(
        id = 1L,
        name = "Test User",
        email = "test@example.com",
        isVIP = false
    )
    
    val vipUser = User(
        id = 2L,
        name = "VIP User",
        email = "vip@example.com",
        isVIP = true
    )
    
    fun createOrder(
        user: User = defaultUser,
        items: List<OrderItem> = emptyList()
    ): Order {
        return Order(
            id = System.currentTimeMillis(),
            userId = user.id,
            items = items,
            total = items.sumOf { it.price * it.quantity },
            status = OrderStatus.NEW
        )
    }
    
    fun createOrderItem(
        productId: Long = 1L,
        quantity: Int = 1,
        price: Double = 29.99
    ): OrderItem {
        return OrderItem(productId, quantity, price)
    }
}

// 3. Mock Factories
object MockFactories {
    
    fun createOrderRepository(): OrderRepository {
        val mock = Mockito.mock(OrderRepository::class.java)
        Mockito.`when`(mock.save(ArgumentMatchers.any(Order::class.java)))
            .thenAnswer { it.getArgument(0) }
        return mock
    }
    
    fun createNotificationService(): NotificationService {
        return Mockito.mock(NotificationService::class.java)
    }
    
    fun createOrderProcessor(): OrderProcessor {
        return OrderProcessor()
    }
}

// 4. Test Categories
@Target(AnnotationTarget.CLASS)
annotation class IntegrationTest

@Target(AnnotationTarget.CLASS)
annotation class SlowTest

@Target(AnnotationTarget.FUNCTION)
annotation class DbTest
```

### Real-World Example: Test Infrastructure

```kotlin
// Test Infrastructure Components

// 1. Dependency Injection for Tests
class TestApp : Application() {
    
    lateinit var testComponent: TestComponent
    
    override fun onCreate() {
        super.onCreate()
        testComponent = DaggerTestComponent.builder()
            .repository(MockFactories.createOrderRepository())
            .build()
    }
}

interface TestComponent {
    fun inject(test: Any)
}

// 2. Test Configuration
class TestConfiguration {
    
    data class Config(
        val useDatabase: Boolean = false,
        val useNetwork: Boolean = false,
        val debugMode: Boolean = true
    )
    
    companion object {
        val default = Config()
        val integration = Config(useDatabase = true)
    }
}

// 3. Test Runner
class CustomTestRunner : AndroidJUnitRunner() {
    
    override fun newApplication(
        cl: Class<*>,
        className: String,
        context: Context
    ): Application {
        // Replace application for tests
        return super.newApplication(cl, TestApp::class.java.name, context)
    }
}

// 4. Test Rules
class TestSchedulerRule : TestRule {
    
    private val scheduler = TestCoroutineScheduler()
    
    override fun apply(base: Statement, description: Description): Statement {
        return object : Statement() {
            override fun evaluate() {
                // Replace main scheduler
                Dispatchers.setMain(TestDispatcher(scheduler))
                try {
                    base.evaluate()
                } finally {
                    Dispatchers.resetMain()
                }
            }
        }
    }
}

// 5. Test Reporters
class TestReporter : TestExecutionListener {
    
    private val results = mutableListOf<TestResult>()
    
    override fun testStarted(description: Description) {
        println("Started: ${description.methodName}")
    }
    
    override fun testFinished(description: Description, result: TestResult) {
        results.add(result)
        println("Finished: ${description.methodName} - ${result.status}")
    }
    
    fun generateReport(): String {
        val passed = results.count { it.status == TestResult.Status.SUCCESSFUL }
        val failed = results.count { it.status == TestResult.Status.FAILURE }
        
        return """
            Test Report
            ==========
            Total: ${results.size}
            Passed: $passed
            Failed: $failed
            Success Rate: ${(passed * 100) / results.size}%
        """.trimIndent()
    }
}
```

### Output Results

```
Test Architecture Execution:

Unit Tests:
- OrderProcessorTest: 10 tests passed
- ProcessOrderUseCaseTest: 8 tests passed

Integration Tests:
- OrderRepositoryIntegrationTest: 5 tests passed
- OrderFlowIntegrationTest: 3 tests passed

Total: 26 tests, 0 failures
Coverage:
- OrderProcessor: 95%
- Use Cases: 88%
- Repositories: 72%

Test Execution Time:
- Unit: 1.2s
- Integration: 4.5s
- Total: 5.7s
```

## Best Practices

1. **Separate concerns**: Keep tests focused and single-purpose
2. **Use test doubles**: Mock external dependencies
3. **Minimize dependencies**: Reduce test setup complexity
4. **Consistent naming**: Clear test method names
5. **Test the boundary**: Test public interfaces
6. **Use basesparingly**: Prefer composition
7. **Disable tests temporarily**: Mark with @Ignore
8. **Parallelize tests**: Run independent tests simultaneously
9. **Organize test data**: Use factories and builders
10. **Document test intent**: Why this test exists

## Common Pitfalls

**Pitfall 1: Testing implementation details**
- **Problem**: Tests break with refactoring
- **Solution**: Test behavior, not implementation

**Pitfall 2: Test dependency on order**
- **Problem**: Tests fail when run in different order
- **Solution**: Make tests independent

**Pitfall 3: Shared test state**
- **Problem**: Tests affect each other
- **Solution**: Reset in @Before/@After

**Pitfall 4: Slow test execution**
- **Problem**: Long feedback cycles
- **Solution**: Prefer unit tests

**Pitfall 5: No test automation**
- **Problem**: Manual testing
- **Solution**: Implement CI pipelines

## Troubleshooting Guide

**Issue: "Test failed in CI but passes locally"**
1. Check environment differences
2. Verify file encoding
3. Look for timing issues
4. Check resource cleanup

**Issue: "Flaky tests"**
1. Add explicit waits
2. Check for race conditions
3. Verify test isolation
4. Use stable test data

**Issue: "Slow test suite"**
1. Identify slow tests
2. Run tests in parallel
3. Use test doubles
4. Consider test selection

## Advanced Tips

**Tip 1: Mutation testing**
```kotlin
// Use Pitest for mutation testing
// Identifies weak tests that don't catch bugs
```

**Tip 2: Property-based testing**
```kotlin
// Use Kotest for property-based testing
// Test general properties of functions
```

**Tip 3: Snapshot testing**
```kotlin
// Use SnaphotTesting for UI tests
// Verify large objects against known correct state
```

**Tip 4: Contract testing**
```kotlin
// Use Pact for consumer-driven contracts
// Verify API compatibility
```

**Tip 5: Chaos testing**
```kotlin
// Introduce failures intentionally
// Verify error handling
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/02_JUnit_and_Mockito.md
See: 08_TESTING/01_Testing_Fundamentals/04_Test_Utilities.md
See: 08_TESTING/02_Advanced_Testing/01_Integration_Testing.md
See: 08_TESTING/02_Advanced_Testing/04_Continuous_Testing.md
See: 03_ARCHITECTURE/01_Architecture_Patterns/03_Clean_Architecture.md