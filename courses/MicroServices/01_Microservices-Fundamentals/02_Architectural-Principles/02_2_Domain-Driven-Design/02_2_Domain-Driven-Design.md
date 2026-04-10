# Domain-Driven Design (DDD)

Domain-Driven Design (DDD) is a software development methodology introduced by Eric Evans in his seminal book "Domain-Driven Design: Tackling Complexity in the Heart of Software" (2003). It provides a framework for understanding business domains and translating complex business logic into well-structured, maintainable software systems.

## What is Domain-Driven Design?

Domain-Driven Design is an approach to software development that emphasizes collaboration between technical and domain experts to create a shared model of the business domain. The core principle is that the software should reflect the language, concepts, and rules of the business domain it serves.

DDD is particularly valuable in complex domains where traditional data-driven approaches fail to capture business complexity. Instead of starting with database schemas or technical requirements, DDD begins with understanding the business domain deeply.

### Why DDD Matters

- **Focuses on business logic**: Places business rules at the center of software design
- **Improves communication**: Creates a shared language between technical and business teams
- **Manages complexity**: Provides patterns for handling complex business domains
- **Enables maintainability**: Creates modular, loosely coupled systems that are easier to change

---

## Core DDD Concepts

### Domain

The **Domain** refers to the specific area of knowledge or activity that the software is designed to address. It encompasses the:
- Business rules
- Processes
- Terminology
- Constraints
- Relationships between entities

The domain is the "heart" of the application - it represents the real-world problem the software solves.

### Domain Model

The **Domain Model** is a conceptual representation of the domain. It includes:
- Entities that have identity
- Value objects that describe characteristics
- Aggregates that group related entities
- Domain events that represent significant occurrences
- Domain services that encapsulate business logic

The domain model should be isolated from infrastructure concerns and represent pure business concepts.

### Bounded Context

A **Bounded Context** is a logical boundary within which a particular domain model is valid and consistent. Each bounded context has:
- Its own Ubiquitous Language
- Its own domain model
- Clear boundaries defining what belongs inside and outside
- Defined interfaces for communication with other contexts

```
┌─────────────────────────────────────────────────────────────────┐
│                        ENTERPRISE                                │
│  ┌─────────────────┐    ┌─────────────────┐    ┌─────────────┐ │
│  │  Order Context  │    │  Customer       │    │  Inventory  │ │
│  │                 │    │  Context        │    │  Context    │ │
│  │  - Orders       │    │                 │    │             │ │
│  │  - Order Items  │    │  - Customers    │    │  - Products │ │
│  │  - Pricing      │    │  - Addresses    │    │  - Stock    │ │
│  └─────────────────┘    └─────────────────┘    └─────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

---

## Ubiquitous Language

The **Ubiquitous Language** is a shared language developed by the development team and domain experts. It is used consistently in:
- Code (class names, method names, variable names)
- Documentation
- Discussions
- User stories
- Diagrams

### Characteristics of Ubiquitous Language

1. **Shared**: Understood by both technical and business teams
2. **Precise**: Terms have exact, unambiguous meanings
3. **Evolving**: Grows and refines as understanding deepens
4. **Consistent**: Used everywhere in the project

### Example

```
Business Term          | Technical Representation
-----------------------|--------------------------
Order                  | Order (Entity)
Order Line Item        | OrderItem (Entity)
Customer's Billing     | BillingAddress (Value Object)
                       | Address (Value Object)
Product Price          | Money (Value Object)
Order Total            | Order.calculateTotal() (Method)
```

---

## Aggregates, Entities, and Value Objects

### Entities

**Entities** are objects that have a distinct identity that runs through time and different representations. They are defined by their identity, not their attributes.

```java
public class Order implements Serializable {
    private UUID orderId;  // Identity - unique across the system
    private UUID customerId;
    private OrderStatus status;
    private List<OrderItem> items;
    private Money total;
    private Instant createdAt;
    
    public UUID getId() {
        return orderId;
    }
    
    public void addItem(Product product, int quantity) {
        // Business logic for adding items
    }
}
```

Key characteristics:
- Unique identity that persists over time
- Can change its state
- Equality is based on identity, not attributes

### Value Objects

**Value Objects** are immutable objects that describe characteristics or attributes without having a distinct identity. They are defined by their attributes, not their identity.

```java
public final class Money {
    private final BigDecimal amount;
    private final Currency currency;
    
    public Money(BigDecimal amount, Currency currency) {
        this.amount = amount;
        this.currency = currency;
    }
    
    public Money add(Money other) {
        if (!this.currency.equals(other.currency)) {
            throw new IllegalArgumentException("Cannot add different currencies");
        }
        return new Money(this.amount.add(other.amount), this.currency);
    }
    
    // Immutable - no setters, no equals based on identity
}
```

Key characteristics:
- Immutable
- No identity
- Equality based on all attribute values
- Can be shared freely

### Aggregates

An **Aggregate** is a cluster of related entities and value objects with a single root entity (Aggregate Root) that controls access to the entire cluster.

```java
public class OrderAggregate {
    private Order order;                    // Aggregate Root
    private List<OrderItem> orderItems;     // Part of aggregate
    
    public void addItem(Product product, int quantity) {
        validateProduct(product);
        OrderItem item = new OrderItem(product, quantity);
        orderItems.add(item);
        recalculateTotal();
    }
    
    public void removeItem(UUID itemId) {
        orderItems.removeIf(item -> item.getId().equals(itemId));
        recalculateTotal();
    }
    
    private void recalculateTotal() {
        // All modifications go through aggregate root
    }
}
```

```
┌────────────────────────────────────────────────────┐
│              ORDER AGGREGATE                       │
│  ┌──────────────────────────────────────────────┐  │
│  │  Order (Aggregate Root)                     │  │
│  │  - orderId: UUID                             │  │
│  │  - customerId: UUID                          │  │
│  │  - status: OrderStatus                      │  │
│  │  - addItem()                                 │  │
│  │  - removeItem()                              │  │
│  │  - submit()                                  │  │
│  └──────────────────────────────────────────────┘  │
│                        │                           │
│                        │ contains                  │
│                        ▼                           │
│  ┌──────────────────────────────────────────────┐  │
│  │  OrderItem (Entity)                           │  │
│  │  - itemId: UUID                              │  │
│  │  - productId: UUID                           │  │
│  │  - quantity: int                             │  │
│  │  - unitPrice: Money                          │  │
│  └──────────────────────────────────────────────┘  │
│                        │                           │
│                        │ has                       │
│                        ▼                           │
│  ┌──────────────────────────────────────────────┐  │
│  │  Money (Value Object)                        │  │
│  │  - amount: BigDecimal                        │  │
│  │  - currency: Currency                         │  │
│  └──────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────┘
```

---

## Domain Events

**Domain Events** represent something significant that happened in the domain. They are used to capture changes in state and enable eventual consistency between bounded contexts.

### Structure of a Domain Event

```java
public abstract class DomainEvent {
    private final UUID eventId;
    private final Instant occurredOn;
    
    public DomainEvent() {
        this.eventId = UUID.randomUUID();
        this.occurredOn = Instant.now();
    }
}

public class OrderPlacedEvent extends DomainEvent {
    private final UUID orderId;
    private final UUID customerId;
    private final Money totalAmount;
    private final List<OrderItemSnapshot> items;
    
    public OrderPlacedEvent(UUID orderId, UUID customerId, 
                           Money totalAmount, List<OrderItemSnapshot> items) {
        this.orderId = orderId;
        this.customerId = customerId;
        this.totalAmount = totalAmount;
        this.items = items;
    }
}
```

### Publishing Domain Events

```java
public class OrderService {
    private final DomainEventPublisher eventPublisher;
    
    public Order placeOrder(Customer customer, List<OrderItem> items) {
        Order order = new Order(customer.getId(), items);
        orderRepository.save(order);
        
        // Publish domain event
        eventPublisher.publish(new OrderPlacedEvent(
            order.getId(),
            customer.getId(),
            order.getTotal(),
            order.getItemsSnapshot()
        ));
        
        return order;
    }
}
```

---

## Strategic DDD: Bounded Contexts and Context Mapping

### Bounded Context Patterns

1. **Single Bounded Context**: Entire application in one context (simple projects)
2. **Multiple Bounded Contexts**: Distributed across the enterprise (complex systems)

### Context Mapping

**Context Mapping** defines the relationships between bounded contexts. Key patterns include:

```
┌─────────────────┐      ┌─────────────────┐
│   Order        │      │    Shipping     │
│   Context      │──────│    Context      │
│                 │Customer│              │
└─────────────────┘      └─────────────────┘
        │                        │
        │ Customer Service      │
        │ (Shared Kernel)        │
        ▼                        ▼
┌─────────────────────────────────────────────┐
│            SHARED KERNEL                    │
│   (Customer Data Structure)                  │
└─────────────────────────────────────────────┘
```

### Context Mapping Patterns

| Pattern | Description |
|---------|-------------|
| **Shared Kernel** | Two contexts share a common subset of the domain model |
| **Customer-Supplier** | One context provides services to another |
| **Anticorruption Layer** | One context translates another's model |
| **Conformist** | Downstream context adapts to upstream's model |
| **Open Host Service** | Context defines a protocol for others to use |
| **Published Language** | Standardized format for communication |

---

## Tactical DDD: Domain Services, Repositories, and Factories

### Domain Services

**Domain Services** encapsulate complex business operations that don't naturally fit within entities or value objects.

```java
public class OrderCalculationService {
    
    public Money calculateOrderTotal(List<CartItem> items, 
                                     Customer customer,
                                     DiscountPolicy discountPolicy) {
        Money subtotal = items.stream()
            .map(item -> item.getProduct().getPrice())
            .reduce(new Money(BigDecimal.ZERO, Currency.USD), Money::add);
        
        BigDecimal discount = discountPolicy.calculateDiscount(customer, subtotal);
        Money discountAmount = new Money(discount, Currency.USD);
        
        return subtotal.subtract(discountAmount);
    }
}
```

### Repositories

**Repositories** abstract the persistence layer, providing a collection-like interface for accessing domain objects.

```java
public interface OrderRepository {
    Optional<Order> findById(UUID orderId);
    List<Order> findByCustomerId(UUID customerId);
    List<Order> findByStatus(OrderStatus status);
    void save(Order order);
    void delete(Order order);
}

public class JpaOrderRepository implements OrderRepository {
    private final EntityManager entityManager;
    
    @Override
    public Optional<Order> findById(UUID orderId) {
        Order order = entityManager.find(Order.class, orderId);
        return Optional.ofNullable(order);
    }
    
    @Override
    public void save(Order order) {
        entityManager.persist(order);
    }
}
```

### Factories

**Factories** encapsulate the creation of complex domain objects and aggregates.

```java
public class OrderFactory {
    
    public Order createOrder(Customer customer, List<CartItem> items) {
        validateCustomer(customer);
        validateItems(items);
        
        Order order = new Order(customer.getId());
        items.forEach(order::addItem);
        
        return order;
    }
    
    private void validateCustomer(Customer customer) {
        if (!customer.isActive()) {
            throw new IllegalStateException("Customer is not active");
        }
    }
}
```

---

## DDD and Microservices Architecture

### How DDD Influences Microservices

DDD and microservices share a natural alignment. Each microservice can correspond to a bounded context, providing:

1. **Autonomous Teams**: Each bounded context can be owned by a separate team
2. **Loose Coupling**: Bounded contexts communicate via well-defined interfaces
3. **Single Responsibility**: Each service focuses on one domain
4. **Scalability**: Services can be scaled independently

```
┌──────────────────────────────────────────────────────────────────────┐
│                        MICROSERVICES ARCHITECTURE                    │
├──────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────────────────┐  │
│  │   Order     │    │  Customer   │    │       Payment          │  │
│  │  Service    │    │  Service    │    │       Service          │  │
│  │             │    │             │    │                         │  │
│  │  Order      │    │  Customer   │    │    Payment             │  │
│  │  Domain     │    │  Domain     │    │    Domain              │  │
│  └─────────────┘    └─────────────┘    └─────────────────────────┘  │
│         │                  │                      │                │
│         └──────────────────┼──────────────────────┘                │
│                            │                                        │
│                     API Gateway                                     │
└──────────────────────────────────────────────────────────────────────┘
```

### DDD Patterns for Microservices

1. **Bounded Context as Service Boundary**: Each microservice owns one bounded context
2. **Aggregate as Transaction Boundary**: Each service manages its own aggregates
3. **Domain Events for Integration**: Services communicate via events
4. **Anticorruption Layer**: Translate between service models when needed
5. **Event Sourcing**: Store domain events for audit and replay

---

## Real-World Examples

### Amazon

Amazon pioneered microservices architecture heavily influenced by DDD principles:
- Each service owns its domain (Orders, Inventory, Recommendations)
- Services communicate via well-defined APIs
- Domain events drive inter-service communication
- Teams are organized around domain boundaries

### Netflix

Netflix uses DDD-inspired patterns:
- Separate services for Content, User Profiles, Streaming
- Event-driven architecture for state changes
- Domain events for cross-service communication
- Each service has its own bounded context

### Uber

Uber's architecture demonstrates DDD principles:
- Separate contexts for Rider, Driver, Trip, Payment
- Clear bounded contexts with explicit interfaces
- Domain events for trip state changes
- Aggregates manage trip consistency

---

## Flow Chart: Domain Model Structure

```
                           ┌─────────────────┐
                           │  Bounded        │
                           │  Context        │
                           └────────┬────────┘
                                    │
                                    ▼
                    ┌───────────────────────────────┐
                    │         Aggregate             │
                    │  ┌─────────────────────────┐  │
                    │  │   Aggregate Root        │  │
                    │  │   (Entity with ID)      │  │
                    │  └─────────────────────────┘  │
                    │              │                │
                    │              │ contains       │
                    │              ▼                │
                    │  ┌─────────────────────────┐  │
                    │  │   Child Entities        │  │
                    │  └─────────────────────────┘  │
                    │              │                │
                    │              │ has            │
                    │              ▼                │
                    │  ┌─────────────────────────┐  │
                    │  │   Value Objects         │  │
                    │  └─────────────────────────┘  │
                    └───────────────────────────────┘
                                    │
                                    │ uses
                                    ▼
                    ┌───────────────────────────────┐
                    │       Domain Services         │
                    │   (Business Operations)       │
                    └───────────────────────────────┘
                                    │
                                    │ creates/manages
                                    ▼
                    ┌───────────────────────────────┐
                    │         Repositories          │
                    │    (Persistence Abstraction) │
                    └───────────────────────────────┘
```

---

## Code Examples

### Complete Domain Model Example

```java
// Value Objects
public record Money(BigDecimal amount, Currency currency) {
    public Money add(Money other) {
        if (!this.currency.equals(other.currency)) {
            throw new IllegalArgumentException();
        }
        return new Money(this.amount.add(other.amount), this.currency);
    }
    
    public static Money zero(Currency currency) {
        return new Money(BigDecimal.ZERO, currency);
    }
}

// Entity
public class OrderItem {
    private final UUID id;
    private final UUID productId;
    private final String productName;
    private final int quantity;
    private final Money unitPrice;
    
    public OrderItem(UUID productId, String productName, 
                     int quantity, Money unitPrice) {
        this.id = UUID.randomUUID();
        this.productId = productId;
        this.productName = productName;
        this.quantity = quantity;
        this.unitPrice = unitPrice;
    }
    
    public Money calculateSubtotal() {
        return unitPrice.multiply(quantity);
    }
}

// Aggregate Root
public class Order {
    private UUID id;
    private UUID customerId;
    private OrderStatus status;
    private List<OrderItem> items;
    private Money total;
    
    public Order(UUID customerId) {
        this.id = UUID.randomUUID();
        this.customerId = customerId;
        this.status = OrderStatus.DRAFT;
        this.items = new ArrayList<>();
        this.total = Money.zero(Currency.USD);
    }
    
    public void addItem(Product product, int quantity) {
        if (status != OrderStatus.DRAFT) {
            throw new IllegalStateException("Cannot modify submitted order");
        }
        OrderItem item = new OrderItem(
            product.getId(), 
            product.getName(), 
            quantity, 
            product.getPrice()
        );
        items.add(item);
        recalculateTotal();
    }
    
    public void submit() {
        if (items.isEmpty()) {
            throw new IllegalStateException("Cannot submit empty order");
        }
        this.status = OrderStatus.SUBMITTED;
    }
    
    private void recalculateTotal() {
        this.total = items.stream()
            .map(OrderItem::calculateSubtotal)
            .reduce(Money.zero(Currency.USD), Money::add);
    }
}

// Domain Service
public class OrderDomainService {
    
    public void placeOrder(Order order, PaymentService paymentService) {
        order.submit();
        paymentService.processPayment(order.getCustomerId(), order.getTotal());
    }
}
```

---

## Best Practices for Implementing DDD in Microservices

### 1. Start with Context, Not Code

- Identify bounded contexts first
- Don't try to model everything at once
- Use Event Storming or Domain Storytelling to discover domains

### 2. Invest in Ubiquitous Language

- Create a glossary of terms
- Use consistent naming across code and documentation
- Involve domain experts in modeling sessions

### 3. Keep Domain Models Pure

- Separate domain logic from infrastructure
- Use dependency injection to keep domain testable
- Avoid leaking technical concerns into domain objects

### 4. Use Aggregates Wisely

- Keep aggregates small
- Use eventual consistency across aggregates
- Don't model every entity as an aggregate

### 5. Embrace Domain Events

- Use events for cross-context communication
- Design events for loose coupling
- Consider event sourcing for complex domains

### 6. Start Simple

- Don't over-engineer simple domains
- Apply DDD selectively where complexity warrants it
- Refactor toward DDD patterns iteratively

### 7. Test the Domain Model

- Write unit tests for domain logic
- Use test-driven development in the domain layer
- Verify business rules in tests

---

## Summary

Domain-Driven Design provides a powerful framework for building complex software systems that accurately model business domains. Key takeaways include:

- **DDD focuses on the domain** as the center of software design
- **Bounded contexts** define clear boundaries for domain models
- **Ubiquitous language** improves communication between teams
- **Aggregates, entities, and value objects** create rich domain models
- **Domain events** enable loose coupling between contexts
- **Strategic and tactical patterns** work together for effective design
- **DDD naturally complements microservices** architecture

By applying DDD principles thoughtfully, teams can create software that better serves business needs and adapts to changing requirements.

---

## References

- Evans, E. (2003). Domain-Driven Design: Tackling Complexity in the Heart of Software
- Vernon, V. (2013). Implementing Domain-Driven Design
- Domain-Driven Design Community: www.domainlanguage.com
