# High Cohesion in Microservices

## Table of Contents
1. [Definition](#definition)
2. [Why Cohesion Matters](#why-cohesion-matters)
3. [Cohesion vs Coupling](#cohesion-vs-coupling)
4. [Single Responsibility Principle](#single-responsibility-principle)
5. [How to Achieve High Cohesion](#how-to-achieve-high-cohesion)
6. [Cohesion in Domain Models](#cohesion-in-domain-models)
7. [Cohesive Service Interfaces](#cohesive-service-interfaces)
8. [Co-locating Related Functionality](#co-locating-related-functionality)
9. [Signs of Low Cohesion](#signs-of-low-cohesion)
10. [Cohesion Patterns Flowchart](#cohesion-patterns-flowchart)
11. [Code Examples](#code-examples)
12. [Real-World Examples](#real-world-examples)
13. [Best Practices](#best-practices)
14. [Summary](#summary)

---

## Definition

**High cohesion** refers to the degree to which elements inside a module or service belong together. In microservices architecture, high cohesion means that all related functionalities, data, and behaviors are organized within a single service boundary, while unrelated concerns are separated into different services.

### Key Characteristics of High Cohesion:
- **Relatedness**: Elements that change for the same reasons are grouped together
- **Focus**: Each service has a clearly defined purpose
- **Completeness**: A service provides everything needed to fulfill its responsibility
- **Independence**: Services can operate independently for their specific domain

### Cohesion Levels (From Highest to Lowest):

| Level | Description | Example |
|-------|-------------|---------|
| **Functional** | All elements work toward a single purpose | PaymentService handles all payment operations |
| **Sequential** | Output of one element is input of another | OrderProcessingService |
| **Communicational** | Elements use the same data | CustomerDataService |
| **Procedural** | Elements are in a specific execution order | UserOnboardingService |
| **Temporal** | Elements are related by time of execution | StartupService, ShutdownService |
| **Logical** | Elements are logically related | UtilityService |
| **Coincidental** | Elements have no meaningful relationship | RandomService |

---

## Why Cohesion Matters for Service Design

### 1. **Maintainability**
High cohesion simplifies maintenance because changes to a specific business capability typically affect only one service. Developers can understand, modify, and deploy changes with minimal risk of unintended side effects.

### 2. **Deployability**
Cohesive services can be deployed independently. When one domain changes, you only need to deploy that specific service, reducing deployment risk and enabling faster release cycles.

### 3. **Testability**
Testing becomes more straightforward when a service has a single, well-defined responsibility. Unit tests cover the complete behavior of the service without needing to mock numerous external dependencies.

### 4. **Team Autonomy**
High cohesion allows teams to own complete business capabilities. A team can make decisions about their service without coordinating with other teams for unrelated functionality.

### 5. **Scalability**
You can scale individual services based on their specific resource needs. A high-cohesion service that handles CPU-intensive operations can be scaled differently than one that handles memory-intensive operations.

### 6. **Understandability**
New team members can quickly understand a cohesive service because all its components relate to a single domain. This reduces onboarding time and improves code review quality.

---

## Cohesion vs Coupling

Cohesion and coupling are two sides of the same coin and must be balanced carefully in microservices design.

### The Relationship

```
┌─────────────────────────────────────────────────────────────┐
│                    DESIGN SPECTRUM                          │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  HIGH COHESION  ◄────────────────────────────────► LOW COUPLING
│         │                                                    │
│         │           IDEAL ZONE                               │
│         │       ┌─────────────┐                              │
│         └──────►│   STRONG    │◄────────────────────────────┘
│                 │   DOMAIN    │       (Your Goal!)
│                 │   FOCUS     │                             
│                 └─────────────┘                             
│                                                             │
│  LOW COHESION  ◄────────────────────────────────► HIGH COUPLING
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Comparison Table

| Aspect | High Cohesion | Low Coupling |
|--------|---------------|--------------|
| **Focus** | Inside service boundaries | Between service boundaries |
| **Goal** | Keep related things together | Keep services independent |
| **Dependency** | Internal organization | External relationships |
| **Change Impact** | Minimized within service | Minimized between services |
| **Team Impact** | Clear ownership | Reduced coordination needs |

### The Balance Point

The ideal microservices architecture achieves:
- **High cohesion within services**: All related functionality grouped together
- **Low coupling between services**: Services communicate through stable interfaces

```
┌─────────────────────────────────────────────────────────────────┐
│                    IDEAL ARCHITECTURE                           │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   ┌─────────────┐      ┌─────────────┐      ┌─────────────┐    │
│   │   ORDER     │      │  CUSTOMER   │      │  PAYMENT    │    │
│   │   SERVICE   │      │   SERVICE   │      │   SERVICE   │    │
│   │             │      │             │      │             │    │
│   │ ┌─────────┐ │      │ ┌─────────┐ │      │ ┌─────────┐ │    │
│   │ │ Domain  │ │      │ │ Domain  │ │      │ │ Domain  │ │    │
│   │ │ Logic   │ │      │ │ Logic   │ │      │ │ Logic   │ │    │
│   │ └─────────┘ │      │ └─────────┘ │      │ └─────────┘ │    │
│   │ ┌─────────┐ │      │ ┌─────────┐ │      │ ┌─────────┐ │    │
│   │ │  Data   │ │      │ │  Data   │ │      │ │  Data   │ │    │
│   │ │ Access  │ │      │ │ Access  │ │      │ │ Access  │ │    │
│   │ └─────────┘ │      │ └─────────┘ │      │ └─────────┘ │    │
│   │ ┌─────────┐ │      │ ┌─────────┐ │      │ ┌─────────┐ │    │
│   │ │External │ │      │ │External │ │      │ │External │ │    │
│   │ │  APIs   │ │      │ │  APIs   │ │      │ │  APIs   │ │    │
│   │ └─────────┘ │      │ └─────────┘ │      │ └─────────┘ │    │
│   └─────────────┘      └─────────────┘      └─────────────┘    │
│         │                   │                    │           │
│         └───────────────────┴────────────────────┘           │
│                             │                                │
│                    Loose Communication                       │
│                    (Events, APIs, Messages)                   │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Single Responsibility Principle

The **Single Responsibility Principle (SRP)**, introduced by Robert C. Martin, states that a class or module should have only one reason to change. High cohesion is the natural outcome of following SRP in microservices design.

### SRP and High Cohesion Connection

```
┌─────────────────────────────────────────────────────────────────┐
│              SINGLE RESPONSIBILITY PRINCIPLE                     │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   "A service should have only one reason to change"            │
│                                                                 │
│   ┌─────────────────────────────────────────────────────────┐  │
│   │                    SERVICE                               │  │
│   │  Responsibility: Order Management                        │  │
│   │  Reason to Change: Business rules for orders              │  │
│   │                                                         │  │
│   │  ✓ Create Orders                                        │  │
│   │  ✓ Update Orders                                         │  │
│   │  ✓ Cancel Orders                                         │  │
│   │  ✓ Calculate Order Total                                 │  │
│   │  ✓ Validate Order Items                                  │  │
│   │                                                         │  │
│   │  ✗ Payment Processing (different responsibility)         │  │
│   │  ✗ Customer Management (different responsibility)        │  │
│   │  ✗ Inventory Tracking (different responsibility)         │  │
│   └─────────────────────────────────────────────────────────┘  │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### How SRP Enforces High Cohesion

1. **Clear Boundaries**: Each service has exactly one reason to change, forcing a clear boundary around related functionality.

2. **Focused Code**: Code within a service tends to be more focused because there's no room for unrelated features.

3. **Consistent Change Patterns**: Related functionality changes together, maintaining cohesion over time.

4. **Reduced Scope**: Developers can fully understand their service without needing to know about unrelated domains.

---

## How to Achieve High Cohesion

### 1. **Start with Domain-Driven Design**

Use Domain-Driven Design (DDD) to identify bounded contexts and aggregate roots:

```
┌─────────────────────────────────────────────────────────────────┐
│                    DOMAIN MODELING PROCESS                       │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   Step 1: Identify Core Domain                                   │
│   ┌─────────────────────────────────────────────────────────┐   │
│   │  Business Context → Core Domain → Sub-Domains          │   │
│   └─────────────────────────────────────────────────────────┘   │
│                          ↓                                      │
│   Step 2: Define Bounded Contexts                               │
│   ┌──────────────┐ ┌──────────────┐ ┌──────────────┐          │
│   │   Catalog    │ │    Order    │ │   Customer   │          │
│   │   Context    │ │   Context   │ │   Context    │          │
│   └──────────────┘ └──────────────┘ └──────────────┘          │
│                          ↓                                      │
│   Step 3: Identify Aggregates                                    │
│   ┌─────────────────────────────────────────────────────────┐   │
│   │  Aggregate → Entity → Value Objects → Domain Events     │   │
│   └─────────────────────────────────────────────────────────┘   │
│                          ↓                                      │
│   Step 4: Define Services                                       │
│   ┌──────────────┐ ┌──────────────┐ ┌──────────────┐          │
│   │  Catalog     │ │   Order      │ │   Customer   │          │
│   │  Service     │ │   Service    │ │   Service    │          │
│   └──────────────┘ └──────────────┘ └──────────────┘          │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### 2. **Follow the "Should Know" Rule**

Group functionality that:
- Operates on the same data
- Shares the same business rules
- Changes for the same reasons

### 3. **Use the "Tell, Don't Ask" Principle**

Design services that tell other services what to do rather than asking for data and processing it externally.

### 4. **Prefer Rich Domain Models**

Keep business logic within services rather than spreading it across the application layer.

### 5. **Define Clear Service Contracts**

Strong, well-defined interfaces help maintain cohesion by making it clear what belongs inside each service.

---

## Cohesion in Domain Models

### Rich vs Anemic Domain Models

**Anemic Domain Model (Low Cohesion):**
```java
// Anemic - Data and behavior are separated
public class Order {
    private String orderId;
    private List<OrderItem> items;
    private Customer customer;
    private BigDecimal total;
    
    // Just getters and setters
    public String getOrderId() { return orderId; }
    public void setOrderId(String orderId) { this.orderId = orderId; }
    // ... other getters and setters
}

// Behavior scattered in service layer
public class OrderService {
    public void calculateTotal(Order order) {
        // Business logic outside domain object
    }
    
    public void validateOrder(Order order) {
        // More business logic outside domain object
    }
}
```

**Rich Domain Model (High Cohesion):**
```java
// Rich - Data and behavior together
public class Order {
    private OrderId orderId;
    private List<OrderItem> items;
    private Customer customer;
    private OrderStatus status;
    
    // Domain logic encapsulated within the entity
    public Money calculateTotal() {
        return items.stream()
            .map(OrderItem::getSubtotal)
            .reduce(Money.ZERO, Money::add);
    }
    
    public void addItem(Product product, int quantity) {
        if (status != OrderStatus.DRAFT) {
            throw new OrderModificationException(
                "Cannot modify order in status: " + status
            );
        }
        items.add(new OrderItem(product, quantity));
    }
    
    public void cancel() {
        if (status == OrderStatus.SHIPPED) {
            throw new OrderCancellationException(
                "Cannot cancel shipped order"
            );
        }
        this.status = OrderStatus.CANCELLED;
        DomainEvents.publish(new OrderCancelledEvent(this));
    }
    
    public void submit() {
        validate();
        this.status = OrderStatus.SUBMITTED;
        DomainEvents.publish(new OrderSubmittedEvent(this));
    }
    
    private void validate() {
        if (items.isEmpty()) {
            throw new EmptyOrderException();
        }
        if (!customer.isActive()) {
            throw new InactiveCustomerException();
        }
    }
}
```

### Aggregate Design for Cohesion

```
┌─────────────────────────────────────────────────────────────────┐
│                    ORDER AGGREGATE                               │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│                    ┌──────────────┐                             │
│                    │    Order     │ ← Aggregate Root            │
│                    │  (Cohesive   │                             │
│                    │   Boundary)  │                             │
│                    └──────┬───────┘                             │
│                           │                                     │
│           ┌───────────────┼───────────────┐                     │
│           │               │               │                     │
│           ▼               ▼               ▼                     │
│    ┌─────────────┐ ┌─────────────┐ ┌─────────────┐             │
│    │ OrderItem   │ │  Shipping   │ │    Order    │             │
│    │ (Part of    │ │   Address   │ │   Events    │             │
│    │  Aggregate) │ │  (Value Obj)│ │ (Part of    │             │
│    │             │ │             │ │  Aggregate) │             │
│    └─────────────┘ └─────────────┘ └─────────────┘             │
│                                                                 │
│   ┌─────────────────────────────────────────────────────────┐   │
│   │  Invariants Enforced Within Aggregate:                 │   │
│   │  • Order total matches item subtotals                  │   │
│   │  • Order status transitions are valid                   │   │
│   │  • Items reference valid products                       │   │
│   └─────────────────────────────────────────────────────────┘   │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Cohesive Service Interfaces

### Characteristics of Cohesive Interfaces

1. **Complete**: Interface provides everything needed for its domain
2. **Consistent**: Naming and patterns are consistent throughout
3. **Focused**: No mixed concerns or unrelated operations
4. **Stable**: Changes are rare and well-communicated

### Example: Cohesive vs Incohesive Service

**Incohesive Interface (OrderService):**
```java
public interface IncohesiveOrderService {
    void createOrder(Order order);
    void cancelOrder(String orderId);
    void calculateShippingRate(Address address);
    void sendEmail(String customerId, String message);
    void generateInvoice(String orderId);
    void updateInventory(String productId, int quantity);
    void processPayment(String orderId, PaymentDetails payment);
    void updateCustomerLoyaltyPoints(String customerId, int points);
}
```

**Cohesive Interface (OrderService):**
```java
public interface OrderService {
    // Pure order operations - all related to order management
    OrderId createOrder(CreateOrderCommand command);
    void submitOrder(OrderId orderId);
    void cancelOrder(CancelOrderCommand command);
    Order getOrder(OrderId orderId);
    List<Order> findOrdersByCustomer(CustomerId customerId);
    List<Order> findOrdersByStatus(OrderStatus status);
    void addOrderItem(OrderId orderId, AddItemCommand command);
    void removeOrderItem(OrderId orderId, OrderItemId itemId);
    void updateOrderItemQuantity(OrderId orderId, OrderItemId itemId, int quantity);
}

// Separate cohesive services for other concerns
public interface ShippingService {
    ShippingRate calculateRate(ShippingRequest request);
    ShippingLabel createLabel(Shipment shipment);
    TrackingInfo trackShipment(TrackingNumber trackingNumber);
}

public interface PaymentService {
    PaymentResult processPayment(PaymentRequest request);
    RefundResult processRefund(RefundRequest request);
    PaymentStatus getPaymentStatus(PaymentId paymentId);
}

public interface NotificationService {
    void sendOrderConfirmation(OrderId orderId);
    void sendOrderShippedNotification(OrderId orderId, TrackingNumber trackingNumber);
    void sendOrderCancelledNotification(OrderId orderId, CancellationReason reason);
}
```

---

## Co-locating Related Functionality

### The "Four Types of Cohesion" Framework

| Type | Description | When to Use |
|------|-------------|-------------|
| **Functional** | All code serves one purpose | Most services should aim for this |
| **Data** | Code that operates on the same data | Data access services |
| **Temporal** | Code that runs at the same time | Startup/initialization services |
| **Communication** | Code that processes the same input | Event handlers, message consumers |

### Co-location Strategies

```
┌─────────────────────────────────────────────────────────────────┐
│                  CO-LOCATION DECISION TREE                      │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│                        ┌─────────┐                              │
│                        │ START   │                              │
│                        └────┬────┘                              │
│                             │                                   │
│                             ▼                                   │
│                    ┌─────────────────┐                         │
│                    │ Is functionality │                         │
│                    │ related to the   │                         │
│                    │ same business    │                         │
│                    │ concept?         │                         │
│                    └────────┬────────┘                         │
│                      YES     │     NO                           │
│                      ┌───────┴───────┐                         │
│                      ▼               ▼                         │
│               ┌────────────┐   ┌────────────┐                  │
│               │   SAME     │   │  DIFFERENT │                  │
│               │  SERVICE   │   │  SERVICE   │                  │
│               └────────────┘   └────────────┘                  │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Example: E-Commerce Platform Co-location

```
┌─────────────────────────────────────────────────────────────────┐
│                  E-COMMERCE SERVICE ARCHITECTURE                │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │                   PRODUCT SERVICE                       │   │
│  │                                                         │   │
│  │  • Product catalog management                           │   │
│  │  • Product search and filtering                         │   │
│  │  • Inventory levels                                     │   │
│  │  • Product reviews and ratings                          │   │
│  │  • Category management                                  │   │
│  │                                                         │   │
│  │  Why: All product-related functionality changes         │   │
│  │       together and serves the same consumers            │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │                    ORDER SERVICE                        │   │
│  │                                                         │   │
│  │  • Order creation and management                        │   │
│  │  • Order validation                                     │   │
│  │  • Order status tracking                                │   │
│  │  • Order history                                        │   │
│  │  • Pricing calculations                                  │   │
│  │                                                         │   │
│  │  Why: Complete order lifecycle within one service      │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │                   PAYMENT SERVICE                       │   │
│  │                                                         │   │
│  │  • Payment processing                                   │   │
│  │  • Refund handling                                      │   │
│  │  • Payment gateway integration                          │   │
│  │  • Transaction recording                                 │   │
│  │  • Fraud detection (if tightly coupled to payments)     │   │
│  │                                                         │   │
│  │  Why: Payments require specialized security and        │   │
│  │       compliance handling separate from orders         │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Signs of Low Cohesion

### Warning Signs in Your Codebase

1. **God Objects/Services**: A single service with hundreds of methods handling dozens of unrelated concerns

2. **Feature Envy**: A service frequently accessing and manipulating another service's data

3. ** shotgun surgery**: A single change requires modifications across many different services

4. **Data Anemia**: Services that are essentially data bags with no business logic

5. **Inconsistent Naming**: Service methods that don't relate to a common domain

6. **Hidden Dependencies**: Services that implicitly depend on the execution order of other services

### Code Smells Indicating Low Cohesion

```java
// SMELL 1: Service with too many responsibilities
public class OrderCustomerPaymentShippingInventoryService {
    // If your service name needs multiple words joined by AND, 
    // it has low cohesion
}

// SMELL 2: No relationship between methods
public class UtilityService {
    void processOrder() { }
    void sendEmail() { }
    void generateReport() { }
    void backupDatabase() { }
    void calculateTax() { }
    // These methods have nothing in common
}

// SMELL 3: Services that know too much about each other
public class OrderService {
    public void completeOrder(String orderId) {
        Order order = orderRepository.findById(orderId);
        order.setStatus(OrderStatus.COMPLETED);
        
        // Should OrderService know about payment processing?
        Payment payment = paymentService.processPayment(order.getCustomerId());
        
        // Should OrderService know about inventory?
        inventoryService.reserveStock(order.getItems());
        
        // Should OrderService know about shipping?
        shippingService.scheduleDelivery(order.getShippingAddress());
    }
}
```

### Refactoring for Higher Cohesion

```java
// AFTER: Well-cohesive services with proper communication
public class OrderService {
    public void completeOrder(String orderId) {
        Order order = findOrder(orderId);
        order.markAsCompleted();
        save(order);
        
        // Publish domain event - let other services react
        eventPublisher.publish(new OrderCompletedEvent(order));
    }
}

public class PaymentService {
    @EventListener
    public void handleOrderCompleted(OrderCompletedEvent event) {
        // PaymentService reacts to the event
        processPaymentForOrder(event.getOrder());
    }
}

public class InventoryService {
    @EventListener
    public void handleOrderCompleted(OrderCompletedEvent event) {
        // InventoryService reacts to the event
        reserveStockForOrder(event.getOrder());
    }
}

public class ShippingService {
    @EventListener
    public void handleOrderCompleted(OrderCompletedEvent event) {
        // ShippingService reacts to the event
        scheduleDeliveryForOrder(event.getOrder());
    }
}
```

---

## Cohesion Patterns Flowchart

### Service Boundaries Decision Flowchart

```
┌─────────────────────────────────────────────────────────────────┐
│           MICROSERVICE BOUNDARY DECISION FLOWCHART              │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   ┌─────────────────────────────────────────────────────────┐   │
│   │ Q1: Do these functionalities share the same data?       │   │
│   └────────────────────────┬────────────────────────────────┘   │
│                    YES      │      NO                           │
│                   ┌─────────┴─────────┐                         │
│                   ▼                   ▼                          │
│            ┌────────────┐        ┌────────────┐                  │
│            │ Continue   │        │ Q2: Do they │                  │
│            │ evaluation │        │ change for  │                  │
│            │ (potential │        │ the same    │                  │
│            │  same      │        │ reason?     │                  │
│            │  service)  │        └──────┬───────┘                │
│            └────────────┘          YES  │    NO                   │
│                                        ▼        ▼                 │
│                               ┌────────────┐  ┌────────────┐      │
│                               │  Evaluate  │  │ SEPARATE   │      │
│                               │  together  │  │ SERVICES   │      │
│                               └─────┬──────┘  └────────────┘      │
│                                     │                             │
│                                     ▼                             │
│                    ┌─────────────────────────────────────┐        │
│                    │ Q3: Do they share the same          │        │
│                    │ business rules/invariants?          │        │
│                    └─────────────────┬───────────────────┘        │
│                               YES  │    NO                        │
│                              ┌─────┴──────┐                       │
│                              ▼            ▼                       │
│                     ┌────────────┐ ┌────────────┐                 │
│                     │    SAME    │ │  EVALUATE  │                 │
│                     │  SERVICE   │ │  BOUNDARY  │                 │
│                     │ (HIGH      │ │  (Consider │                 │
│                     │  COHESION) │ │  subdomain │                 │
│                     └────────────┘ │  coupling) │                 │
│                                    └────────────┘                 │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Cohesion Assessment Flowchart

```
┌─────────────────────────────────────────────────────────────────┐
│               COHESION QUALITY ASSESSMENT                       │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   ┌─────────────────────────────────────────────────────────┐   │
│   │                 ASSESS YOUR SERVICE                     │   │
│   └────────────────────────┬────────────────────────────────┘   │
│                            │                                     │
│                            ▼                                     │
│              ┌─────────────────────────────┐                     │
│              │ Can you describe the        │                     │
│              │ service's purpose in one    │                     │
│              │ sentence?                   │                     │
│              └──────────────┬──────────────┘                     │
│                    YES     │     NO                              │
│                    ┌───────┴───────┐                            │
│                    ▼               ▼                              │
│            ┌────────────┐   ┌────────────┐                       │
│            │  LIKELY    │   │  NEEDS     │                       │
│            │  COHESIVE  │   │  REFACTOR  │                       │
│            └──────┬─────┘   └────────────┘                       │
│                   │                                               │
│                   ▼                                               │
│        ┌─────────────────────────────┐                            │
│        │ Do changes to one feature  │                            │
│        │ require changes to other   │                            │
│        │ features in the service?   │                            │
│        └──────────────┬──────────────┘                            │
│               YES     │     NO                                    │
│              ┌────────┴────────┐                                  │
│              ▼                ▼                                    │
│      ┌────────────┐   ┌────────────┐                               │
│      │  NEEDS     │   │  CHECK:   │                               │
│      │  REFACTOR  │   │  Service  │                               │
│      │  - Separate│   │  has good │                               │
│      │  concerns  │   │  cohesion │                               │
│      └────────────┘   └─────┬──────┘                               │
│                             │                                      │
│                             ▼                                      │
│                  ┌─────────────────────────┐                      │
│                  │ Can teams work on       │                      │
│                  │ different parts of      │                      │
│                  │ the service independently│                      │
│                  │ without conflict?        │                      │
│                  └─────────────┬───────────┘                      │
│                          YES    │    NO                            │
│                         ┌────────┴────────┐                        │
│                         ▼                 ▼                        │
│                 ┌────────────┐   ┌────────────┐                    │
│                 │  GOOD      │   │  SPLIT BY  │                    │
│                 │  COHESION  │   │  TEAM      │                    │
│                 └────────────┘   │  BOUNDARY  │                    │
│                                  └────────────┘                    │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Code Examples

### Example 1: E-Commerce Product Service

```java
// HIGH COHESION: ProductService encapsulates all product-related behavior
@Service
@Transactional
public class ProductService {
    
    private final ProductRepository productRepository;
    private final ProductSearchRepository searchRepository;
    private final InventoryService inventoryService;
    private final EventPublisher eventPublisher;
    
    public ProductService(
            ProductRepository productRepository,
            ProductSearchRepository searchRepository,
            InventoryService inventoryService,
            EventPublisher eventPublisher) {
        this.productRepository = productRepository;
        this.searchRepository = searchRepository;
        this.inventoryService = inventoryService;
        this.eventPublisher = eventPublisher;
    }
    
    public Product createProduct(CreateProductCommand command) {
        validateProductCommand(command);
        
        Product product = Product.create(
            command.name(),
            command.description(),
            command.category(),
            Money.of(command.price(), command.currency()),
            command.sku()
        );
        
        product = productRepository.save(product);
        searchRepository.index(product);
        
        eventPublisher.publish(new ProductCreatedEvent(product));
        
        return product;
    }
    
    public void updatePricing(ProductId productId, Money newPrice) {
        Product product = findProductOrThrow(productId);
        
        product.updatePrice(newPrice);
        productRepository.save(product);
        searchRepository.updatePrice(productId, newPrice);
        
        eventPublisher.publish(new ProductPriceChangedEvent(productId, newPrice));
    }
    
    public void deactivateProduct(ProductId productId) {
        Product product = findProductOrThrow(productId);
        
        product.deactivate();
        productRepository.save(product);
        searchRepository.removeFromIndex(productId);
        inventoryService.blockStockForProduct(productId);
        
        eventPublisher.publish(new ProductDeactivatedEvent(productId));
    }
    
    public ProductDetails getProductDetails(ProductId productId) {
        Product product = findProductOrThrow(productId);
        StockLevel stockLevel = inventoryService.getStockLevel(productId);
        
        return new ProductDetails(product, stockLevel);
    }
    
    public SearchResult searchProducts(ProductSearchCriteria criteria) {
        return searchRepository.search(criteria);
    }
    
    public List<Product> findProductsByCategory(CategoryId categoryId) {
        return productRepository.findByCategory(categoryId);
    }
    
    private Product findProductOrThrow(ProductId productId) {
        return productRepository.findById(productId)
            .orElseThrow(() -> new ProductNotFoundException(productId));
    }
    
    private void validateProductCommand(CreateProductCommand command) {
        if (productRepository.existsBySku(command.sku())) {
            throw new DuplicateSkuException(command.sku());
        }
        if (command.price().isNegative()) {
            throw new InvalidPriceException("Price cannot be negative");
        }
    }
}
```

### Example 2: Order Service with Domain Events

```java
// HIGH COHESION: OrderService handles complete order lifecycle
@Service
@org.springframework.context.event.EventListener
public class OrderService {
    
    private final OrderRepository orderRepository;
    private final ProductService productService;
    private final CustomerService customerService;
    private final DomainEventPublisher eventPublisher;
    
    public OrderService(
            OrderRepository orderRepository,
            ProductService productService,
            CustomerService customerService,
            DomainEventPublisher eventPublisher) {
        this.orderRepository = orderRepository;
        this.productService = productService;
        this.customerService = customerService;
        this.eventPublisher = eventPublisher;
    }
    
    @Transactional
    public OrderId createOrder(CreateOrderCommand command) {
        Customer customer = customerService.getCustomer(command.customerId());
        
        Order order = Order.create(customer, command.shippingAddress());
        
        for (OrderItemCommand item : command.items()) {
            Product product = productService.getProduct(item.productId());
            order.addItem(product, item.quantity());
        }
        
        order.calculateTotal();
        order.validate();
        
        order = orderRepository.save(order);
        
        eventPublisher.publish(OrderCreatedEvent.from(order));
        
        return order.getId();
    }
    
    @Transactional
    public void submitOrder(OrderId orderId) {
        Order order = findOrderOrThrow(orderId);
        
        order.submit();
        orderRepository.save(order);
        
        eventPublisher.publish(OrderSubmittedEvent.from(order));
    }
    
    @Transactional
    public void cancelOrder(CancelOrderCommand command) {
        Order order = findOrderOrThrow(command.orderId());
        
        order.cancel(command.reason());
        orderRepository.save(order);
        
        eventPublisher.publish(OrderCancelledEvent.from(order, command.reason()));
    }
    
    @Transactional
    public void addItemToOrder(OrderId orderId, AddItemCommand command) {
        Order order = findOrderOrThrow(orderId);
        Product product = productService.getProduct(command.productId());
        
        order.addItem(product, command.quantity());
        order.calculateTotal();
        
        orderRepository.save(order);
        
        eventPublisher.publish(OrderUpdatedEvent.from(order));
    }
    
    private Order findOrderOrThrow(OrderId orderId) {
        return orderRepository.findById(orderId)
            .orElseThrow(() -> new OrderNotFoundException(orderId));
    }
    
    // Event handlers for maintaining consistency
    @org.springframework.context.event.EventListener
    public void handleProductPriceChanged(ProductPriceChangedEvent event) {
        List<Order> draftOrders = orderRepository.findDraftOrdersContaining(event.productId());
        
        for (Order order : draftOrders) {
            order.updateItemPrice(event.productId(), event.newPrice());
            order.calculateTotal();
            orderRepository.save(order);
        }
    }
}
```

### Example 3: Anti-pattern - Low Cohesion Service

```java
// LOW COHESION: Everything and the kitchen sink in one service
@Service
public class MonolithicService {
    
    public void createUser(User user) { /* ... */ }
    public void sendEmail(String to, String subject, String body) { /* ... */ }
    public void processPayment(Order order) { /* ... */ }
    public void generateReport(DateRange range) { /* ... */ }
    public void backupDatabase() { /* ... */ }
    public void calculateCommission(SalesRep rep) { /* ... */ }
    public void sendPushNotification(User user, String message) { /* ... */ }
    public void generateInvoice(Order order) { /* ... */ }
    public void archiveOldRecords() { /* ... */ }
    public void sendSms(String phoneNumber, String message) { /* ... */ }
    // This service has ZERO cohesion - nothing is related!
}
```

---

## Real-World Examples

### Netflix: High Cohesion Through Domain Decomposition

Netflix decomposed their monolithic streaming platform into highly cohesive services:

| Service | Cohesive Responsibilities |
|---------|---------------------------|
| **Content Delivery Service** | Video streaming, adaptive bitrate, CDN management |
| **Recommendation Service** | User preferences, ML recommendations, viewing history |
| **Subscription Service** | Plans, billing, payment processing, subscriptions |
| **User Profile Service** | Account management, authentication, profiles |
| **Playback Service** | Playback control, device synchronization, watch progress |

Each service owns its complete domain, enabling independent scaling and development.

### Amazon: Service-Oriented Architecture

Amazon's famous "service-oriented architecture" evolved to have:
- **Cart Service**: Shopping cart management, item manipulation
- **Catalog Service**: Product information, search, browse
- **Fulfillment Service**: Warehouse selection, shipping calculations
- **Payment Service**: Payment processing, refunds, gift cards

This high cohesion allows different teams to own complete business workflows while maintaining loose coupling through well-defined APIs.

### Uber: Domain-Based Service Boundaries

Uber organizes around cohesive business domains:

```
┌─────────────────────────────────────────────────────────────────┐
│                    UBER SERVICE DOMAINS                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────┐  ┌─────────────────┐  ┌────────────────┐  │
│  │   TRIP SERVICE  │  │  SUPPLY SERVICE │  │  MATCHING      │  │
│  │                 │  │                 │  │  SERVICE       │  │
│  │ • Trip lifecycle│  │ • Driver status │  │                │  │
│  │ • Route planning│  │ • Location      │  │ • Driver-rider │  │
│  │ • Fare估算      │  │   updates       │  │   matching     │  │
│  │ • Trip history  │  │ • Availability  │  │ • Surge pricing│  │
│  └─────────────────┘  └─────────────────┘  └────────────────┘  │
│                                                                 │
│  ┌─────────────────┐  ┌─────────────────┐  ┌────────────────┐  │
│  │  USER SERVICE   │  │  PAYMENT        │  │  NOTIFICATION  │  │
│  │                 │  │  SERVICE        │  │  SERVICE       │  │
│  │ • Account mgmt  │  │                 │  │                │  │
│  │ • Authentication│  │ • Transaction   │  │ • Push alerts  │  │
│  │ • User profiles │  │   processing    │  │ • SMS           │  │
│  │ • Loyalty      │  │ • Wallet mgmt   │  │ • Email        │  │
│  └─────────────────┘  └─────────────────┘  └────────────────┘  │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Spotify: Squad Model with High Cohesion

Spotify's organizational model supports high cohesion:
- **Squads** (small teams) own complete features
- Each squad has end-to-end responsibility for their domain
- Squads can deploy independently
- Clear ownership reduces coordination overhead

---

## Best Practices

### 1. **Start with a Monolith, Then Decompose**

```
┌─────────────────────────────────────────────────────────────────┐
│                    EVOLUTIONARY APPROACH                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   Phase 1: Well-Structured Monolith                            │
│   ┌─────────────────────────────────────────────────────────┐   │
│   │  Organized by packages/modules matching business domains│   │
│   │  Clear boundaries between modules (even in code)        │   │
│   └─────────────────────────────────────────────────────────┘   │
│                            │                                     │
│                            ▼                                     │
│   Phase 2: Identify Natural Service Boundaries                  │
│   ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐              │
│   │ Module A│ │ Module B│ │ Module C│ │ Module D│              │
│   └────┬────┘ └────┬────┘ └────┬────┘ └────┬────┘              │
│        └──────────┴───────────┴───────────┘                    │
│                            │                                     │
│                            ▼                                     │
│   Phase 3: Extract as Services When Needed                      │
│   ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐              │
│   │Service A│ │Service B│ │Service C│ │Service D│              │
│   └─────────┘ └─────────┘ └─────────┘ └─────────┘              │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### 2. **Use Domain Events for Cross-Service Communication**

Events maintain loose coupling while preserving business flow:

```java
// Domain event - creates natural boundaries
public record OrderPlacedEvent(
    OrderId orderId,
    CustomerId customerId,
    List<OrderLineItem> items,
    Money total,
    Instant occurredAt
) implements DomainEvent { }
```

### 3. **Apply the Re-release Rule**

Before adding code to a service, ask:
- Does this change for the same reason as existing code?
- Does this operate on the same data?
- Does this share the same invariants?

If all answers are yes, it belongs in the same service.

### 4. **Keep Data Close to Behavior**

```java
// GOOD: Data and behavior together
public class Order {
    private List<OrderItem> items;
    
    public Money calculateTotal() {
        return items.stream()
            .map(OrderItem::getSubtotal)
            .reduce(Money.ZERO, Money::add);
    }
}

// BAD: Data separate from behavior
public class Order {
    private List<OrderItem> items;
    // Just getters/setters
}

public class OrderCalculator {
    public Money calculateTotal(Order order) {
        // Behavior far from data
    }
}
```

### 5. **Document Service Contracts Thoroughly**

Clear API contracts enable independent development:

| Document | Purpose |
|----------|---------|
| API Specification | What operations are available |
| Data Models | What data structures are used |
| Event Schemas | How services communicate |
| Error Codes | How errors are communicated |
| SLAs | Performance expectations |

### 6. **Measure Cohesion**

Use metrics to track cohesion over time:

```
Cohesion Metrics:

1. Change Frequency Analysis
   - Services with high internal change coupling may need splitting
   - Services with low internal change coupling may be too fragmented

2. Service Size
   - Too large = potential low cohesion
   - Too small = potential over-engineering

3. Cross-Service Transactions
   - High cross-service transactions may indicate boundary issues

4. Team Assignment
   - Services should align with team ownership
```

### 7. **Avoid Premature Decomposition**

```
┌─────────────────────────────────────────────────────────────────┐
│                WHEN TO DECOMPOSE vs STAY MONOLITHIC             │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   DECOMPOSE when:                                               │
│   ✓ Team size exceeds 10-12 people                              │
│   ✓ Specific components have different scaling needs           │
│   ✓ Different deployment frequencies needed                     │
│   ✓ Clear domain boundaries identified                          │
│   ✓ Technical requirements differ (different DBs, etc.)         │
│                                                                 │
│   STAY MONOLITHIC when:                                         │
│   ✗ Team is small (<5 developers)                               │
│   ✗ Domain boundaries are unclear                               │
│   ✗ No significant scaling differences                          │
│   ✗ Rapid iteration is priority                                │
│   ✗ Resources are limited                                      │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Summary

**High cohesion** is a fundamental principle in microservices architecture that ensures related functionality, data, and behavior are grouped together within well-defined service boundaries. It directly supports:

- **Maintainability**: Changes affect fewer services
- **Deployability**: Services can be deployed independently
- **Scalability**: Services scale based on their specific needs
- **Team Autonomy**: Clear ownership enables parallel development
- **Understandability**: Focused services are easier to comprehend

### Key Takeaways

1. **Cohesion is about internal organization** - keeping related things together
2. **Coupling is about external relationships** - keeping services independent
3. **High cohesion + Low coupling = Ideal microservices architecture**
4. **Use domain-driven design** to identify natural service boundaries
5. **Follow Single Responsibility Principle** to maintain focused services
6. **Measure and monitor** cohesion metrics over time
7. **Prefer evolution over revolution** - decompose when boundaries become clear

### Quick Reference

```
┌─────────────────────────────────────────────────────────────────┐
│                 HIGH COHESION CHECKLIST                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│   □ Service has a single, well-defined purpose                  │
│   □ All business logic for the domain lives in the service      │
│   □ Service owns its data completely                            │
│   □ Changes are localized to one service                        │
│   □ Team can make decisions independently                       │
│   □ Service can be understood without knowing other services    │
│   □ Interface is focused and doesn't mix concerns               │
│   □ Domain events capture business workflows                    │
│   □ Aggregates maintain internal consistency                    │
│   □ Service can be tested in isolation                          │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## Further Reading

- **Domain-Driven Design** by Eric Evans - Bounded Contexts and Aggregates
- **Building Microservices** by Sam Newman - Service Design Principles
- **Clean Architecture** by Robert C. Martin - Single Responsibility Principle
- **Team Topologies** by Matthew Skelton - Team-aligned Service Design

---

*Last Updated: 2026*
