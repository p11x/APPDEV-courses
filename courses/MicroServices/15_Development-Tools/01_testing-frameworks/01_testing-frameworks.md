# Testing Frameworks for Microservices

## Overview

Testing is fundamental to microservices development. The distributed nature of microservices architecture requires comprehensive testing strategies that verify both individual services and their interactions. This guide covers the testing frameworks and tools that enable effective microservices testing at all levels.

Modern microservices testing involves multiple layers: unit tests for individual components, integration tests for service interactions, contract tests for API compatibility, and end-to-end tests for complete workflows. Each layer requires specific tools and approaches.

The testing frameworks discussed here support these various testing levels while providing features like test isolation, parallel execution, and detailed reporting.

## Unit Testing Frameworks

### JUnit for Java Microservices

JUnit is the standard testing framework for Java applications. It provides annotations for defining test methods, assertions for verifying behavior, and extensibility for custom test runners.

```java
// Example: JUnit test for a microservice
import org.junit.jupiter.api.*;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.when;

@ExtendWith(MockitoExtension.class)
class OrderServiceTest {
    
    @Mock
    private OrderRepository orderRepository;
    
    @Mock
    private InventoryService inventoryService;
    
    @InjectMocks
    private OrderService orderService;
    
    @Test
    void createOrder_WhenInventoryAvailable_ReturnsOrder() {
        // Arrange
        OrderRequest request = new OrderRequest();
        request.setProductId("prod-123");
        request.setQuantity(2);
        
        when(inventoryService.checkAvailability("prod-123", 2))
            .thenReturn(true);
        when(orderRepository.save(any(Order.class)))
            .thenAnswer(invocation -> {
                Order order = invocation.getArgument(0);
                order.setId("order-456");
                return order;
            });
        
        // Act
        Order result = orderService.createOrder(request);
        
        // Assert
        assertNotNull(result);
        assertEquals("order-456", result.getId());
        assertEquals("PENDING", result.getStatus());
    }
    
    @Test
    void createOrder_WhenInventoryUnavailable_ThrowsException() {
        // Arrange
        OrderRequest request = new OrderRequest();
        request.setProductId("prod-123");
        request.setQuantity(100);
        
        when(inventoryService.checkAvailability("prod-123", 100))
            .thenReturn(false);
        
        // Act & Assert
        assertThrows(InsufficientInventoryException.class, 
            () -> orderService.createOrder(request));
    }
}
```

### PyTest for Python Microservices

PyTest is the most popular testing framework for Python. It offers simple test discovery, fixtures for test setup/teardown, and powerful parameterization features.

```python
# Example: PyTest test for a Python microservice
import pytest
from unittest.mock import Mock, patch
from order_service import OrderService
from order_repository import OrderRepository


class TestOrderService:
    @pytest.fixture
    def order_repository(self):
        return Mock(spec=OrderRepository)
    
    @pytest.fixture
    def order_service(self, order_repository):
        return OrderService(order_repository)
    
    def test_create_order_success(self, order_service, order_repository):
        # Arrange
        order_data = {
            "product_id": "prod-123",
            "quantity": 2,
            "user_id": "user-456"
        }
        
        order_repository.save.return_value = {
            "id": "order-789",
            "status": "pending",
            **order_data
        }
        
        # Act
        result = order_service.create_order(order_data)
        
        # Assert
        assert result["id"] == "order-789"
        assert result["status"] == "pending"
        order_repository.save.assert_called_once()
    
    def test_create_order_insufficient_inventory(self, order_service):
        # Arrange
        order_data = {
            "product_id": "prod-123",
            "quantity": 1000,
            "user_id": "user-456"
        }
        
        # Act & Assert
        with pytest.raises(InsufficientInventoryError):
            order_service.create_order(order_data)
    
    @pytest.mark.parametrize("quantity,expected", [
        (1, "small"),
        (10, "medium"),
        (100, "large")
    ])
    def test_order_size_classification(self, quantity, expected):
        # Arrange
        order_service = OrderService(Mock())
        
        # Act
        result = order_service.classify_order_size(quantity)
        
        # Assert
        assert result == expected
```

## Integration Testing

### TestContainers for Database Integration

TestContainers provides Docker containers for integration testing, allowing tests to run against real database instances.

```java
// Example: TestContainers integration test
import org.testcontainers.containers.PostgreSQLContainer;
import org.testcontainers.junit.jupiter.Container;
import org.junit.jupiter.api.*;

import static org.junit.jupiter.api.Assertions.*;

@TestInstance(TestInstance.Lifecycle.PER_CLASS)
class OrderRepositoryIntegrationTest {
    
    @Container
    static PostgreSQLContainer<?> postgres = new PostgreSQLContainer<>("postgres:14")
        .withDatabaseName("orders")
        .withUsername("test")
        .withPassword("test");
    
    private OrderRepository repository;
    
    @BeforeAll
    void setup() {
        repository = new OrderRepository(
            postgres.getJdbcUrl(),
            postgres.getUsername(),
            postgres.getPassword()
        );
        
        // Initialize schema
        repository.initializeSchema();
    }
    
    @Test
    void save_and_retrieve_order() {
        // Arrange
        Order order = new Order();
        order.setProductId("prod-123");
        order.setQuantity(5);
        order.setUserId("user-456");
        
        // Act
        Order saved = repository.save(order);
        Order retrieved = repository.findById(saved.getId());
        
        // Assert
        assertNotNull(retrieved);
        assertEquals("prod-123", retrieved.getProductId());
        assertEquals(5, retrieved.getQuantity());
    }
}
```

## Contract Testing

### Pact for Consumer-Driven Contracts

Pact enables consumer-driven contract testing, ensuring that APIs meet consumer expectations.

```python
# Example: Pact consumer test
import pytest
from pact import Consumer, Provider

@pytest.fixture
def pact():
    return Consumer("order-service").has_pact_with(
        Provider("user-service")
    )

def test_get_user_profile(pact):
    # Define expected interaction
    (
        pact.given("user with id 123 exists")
        .upon_receiving("a request for user profile")
        .with_request(
            method="GET",
            path="/api/users/123",
            headers={"Authorization": "Bearer token123"}
        )
        .will_respond_with(
            status=200,
            body={
                "id": "123",
                "name": "John Doe",
                "email": "john@example.com"
            }
        )
    )
    
    # Verify interaction
    with pact:
        result = requests.get(
            "http://localhost:9999/api/users/123",
            headers={"Authorization": "Bearer token123"}
        )
    
    assert result.status_code == 200
    assert result.json()["name"] == "John Doe"
```

## Implementation Example

```python
#!/usr/bin/env python3
"""
Microservices Testing Framework
Comprehensive testing utilities for microservices
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional, Callable
from enum import Enum
import json
import time


class TestLevel(Enum):
    UNIT = "unit"
    INTEGRATION = "integration"
    CONTRACT = "contract"
    END_TO_END = "end_to_end"


class TestResult(Enum):
    PASSED = "passed"
    FAILED = "failed"
    SKIPPED = "skipped"
    ERROR = "error"


@dataclass
class TestCase:
    """Represents a test case"""
    test_id: str
    name: str
    level: TestLevel
    service: str
    duration_ms: int = 0
    result: TestResult = TestResult.SKIPPED
    error: Optional[str] = None


@dataclass
class TestSuite:
    """Collection of test cases"""
    suite_id: str
    name: str
    tests: List[TestCase] = field(default_factory=list)
    passed: int = 0
    failed: int = 0


class TestRunner:
    """Test runner for microservices"""
    
    def __init__(self):
        self.suites: Dict[str, TestSuite] = {}
        self.test_functions: Dict[str, Callable] = {}
    
    def register_suite(self, suite_id: str, name: str) -> TestSuite:
        """Register a test suite"""
        suite = TestSuite(suite_id=suite_id, name=name)
        self.suites[suite_id] = suite
        return suite
    
    def register_test(
        self,
        suite_id: str,
        test_id: str,
        name: str,
        level: TestLevel,
        service: str,
        test_func: Callable
    ):
        """Register a test function"""
        test = TestCase(
            test_id=test_id,
            name=name,
            level=level,
            service=service
        )
        
        suite = self.suites.get(suite_id)
        if suite:
            suite.tests.append(test)
            self.test_functions[test_id] = test_func
    
    def run_suite(self, suite_id: str) -> Dict:
        """Run all tests in a suite"""
        suite = self.suites.get(suite_id)
        if not suite:
            return {"error": f"Suite {suite_id} not found"}
        
        suite.passed = 0
        suite.failed = 0
        
        for test in suite.tests:
            test_func = self.test_functions.get(test.test_id)
            
            if not test_func:
                test.result = TestResult.SKIPPED
                continue
            
            try:
                start_time = time.time()
                test_func()
                test.result = TestResult.PASSED
                suite.passed += 1
                test.duration_ms = int((time.time() - start_time) * 1000)
                
            except Exception as e:
                test.result = TestResult.FAILED
                test.error = str(e)
                suite.failed += 1
        
        return {
            "suite_id": suite_id,
            "total": len(suite.tests),
            "passed": suite.passed,
            "failed": suite.failed,
            "pass_rate": f"{(suite.passed / len(suite.tests) * 100):.1f}%"
        }
    
    def run_by_level(self, level: TestLevel) -> Dict:
        """Run all tests at a specific level"""
        results = []
        
        for suite_id, suite in self.suites.items():
            for test in suite.tests:
                if test.level == level:
                    test_func = self.test_functions.get(test.test_id)
                    
                    if test_func:
                        try:
                            test_func()
                            test.result = TestResult.PASSED
                        except Exception as e:
                            test.result = TestResult.FAILED
                            test.error = str(e)
        
        return {
            "level": level.value,
            "tests_run": len([
                t for s in self.suites.values() 
                for t in s.tests if t.level == level
            ])
        }
    
    def generate_report(self) -> str:
        """Generate test execution report"""
        
        total_tests = sum(len(s.tests) for s in self.suites.values())
        total_passed = sum(s.passed for s in self.suites.values())
        total_failed = sum(s.failed for s in self.suites.values())
        
        report = {
            "summary": {
                "total_tests": total_tests,
                "passed": total_passed,
                "failed": total_failed,
                "pass_rate": f"{(total_passed / total_tests * 100):.1f}%"
            },
            "by_suite": [
                {
                    "suite": s.suite_id,
                    "name": s.name,
                    "passed": s.passed,
                    "failed": s.failed
                }
                for s in self.suites.values()
            ]
        }
        
        return json.dumps(report, indent=2)


# Example usage
def test_order_creation():
    # Unit test implementation
    assert True

def test_service_integration():
    # Integration test implementation
    assert True

if __name__ == "__main__":
    runner = TestRunner()
    
    # Create suites
    unit_suite = runner.register_suite("unit_tests", "Unit Tests")
    integration_suite = runner.register_suite(
        "integration_tests", 
        "Integration Tests"
    )
    
    # Register tests
    runner.register_test(
        "unit_tests",
        "test_order_creation",
        "Order Creation",
        TestLevel.UNIT,
        "order-service",
        test_order_creation
    )
    
    runner.register_test(
        "integration_tests",
        "test_service_integration",
        "Service Integration",
        TestLevel.INTEGRATION,
        "order-service",
        test_service_integration
    )
    
    # Run tests
    result = runner.run_suite("unit_tests")
    print(f"Suite Results: {result}")
    
    # Generate report
    print("\n" + runner.generate_report())
```

## Best Practices

1. **Test at Multiple Levels**: Combine unit, integration, contract, and E2E tests for comprehensive coverage.

2. **Use Test Fixtures**: Reuse setup code across tests to reduce duplication.

3. **Mock External Dependencies**: Use mocks for external services to ensure test isolation.

4. **Run Tests in Parallel**: Use parallel test execution to reduce build times.

5. **Monitor Test Coverage**: Track code coverage to identify untested areas.

---

## Output Statement

```
Test Execution Report
=====================
Total Tests: 150
Passed: 142
Failed: 8
Pass Rate: 94.7%

By Level:
- Unit: 98% pass rate
- Integration: 92% pass rate
- Contract: 100% pass rate
- E2E: 85% pass rate

Failed Tests:
- test_order_validation: Invalid input handling
- test_payment_integration: Timeout issue
- test_user_search_performance: Slow query

Recommendations:
1. Review error handling in order service
2. Increase timeout for payment integration tests
3. Add database query optimization
```