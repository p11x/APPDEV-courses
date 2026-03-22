# Example40.py
# Topic: Combined Examples - Docstrings and Type Annotations

# This file provides comprehensive combined examples demonstrating
# the practical use of docstrings and type annotations together.


from typing import Optional, Dict, List, Any, Tuple, Callable, Union
import json


# Example 1: Complete Function with All Documentation Elements
def calculate_billing(
    items: List[Dict[str, Union[str, float]]],
    tax_rate: float,
    discount_percent: float = 0.0,
    shipping_method: Optional[str] = None
) -> Dict[str, Any]:
    """Calculate total billing amount with taxes, discounts, and shipping.
    
    This function computes the complete billing for an order including
    item costs, applicable discounts, tax calculations, and shipping fees.
    
    Parameters
    ----------
    items : List[Dict[str, Union[str, float]]]
        List of item dictionaries, each containing 'name', 'price', and optional 'quantity'.
    tax_rate : float
        Tax rate as a decimal (e.g., 0.08 for 8% tax).
    discount_percent : float, optional
        Discount percentage to apply (default: 0.0).
    shipping_method : str, optional
        Shipping method ('standard', 'express', or 'overnight').
    
    Returns
    -------
    Dict[str, Any]
        Dictionary containing:
        - subtotal: Sum of all item prices
        - discount_amount: Discount applied
        - tax_amount: Tax calculated
        - shipping_cost: Shipping fee
        - total: Final total amount
    
    Raises
    ------
    ValueError
        If items list is empty or tax_rate is negative.
    
    Examples
    --------
    >>> items = [{'name': 'Widget', 'price': 10.00, 'quantity': 2}]
    >>> calculate_billing(items, 0.08, 10.0, 'express')
    {'subtotal': 20.0, 'discount_amount': 2.0, 'tax_amount': 1.44, 'shipping_cost': 15.0, 'total': 34.44}
    """
    if not items:
        raise ValueError("Items list cannot be empty")
    if tax_rate < 0:
        raise ValueError("Tax rate cannot be negative")
    
    # Calculate subtotal
    subtotal = sum(
        float(item.get("price", 0)) * item.get("quantity", 1)
        for item in items
    )
    
    # Calculate discount
    discount_amount = subtotal * (discount_percent / 100)
    
    # Calculate tax on discounted amount
    taxable_amount = subtotal - discount_amount
    tax_amount = taxable_amount * tax_rate
    
    # Calculate shipping cost
    shipping_costs = {
        None: 0,
        "standard": 5.99,
        "express": 15.00,
        "overnight": 25.00
    }
    shipping_cost = shipping_costs.get(shipping_method, 0)
    
    # Calculate total
    total = taxable_amount + tax_amount + shipping_cost
    
    return {
        "subtotal": round(subtotal, 2),
        "discount_amount": round(discount_amount, 2),
        "tax_amount": round(tax_amount, 2),
        "shipping_cost": round(shipping_cost, 2),
        "total": round(total, 2)
    }


cart_items = [
    {"name": "Laptop", "price": 999.99, "quantity": 1},
    {"name": "Mouse", "price": 29.99, "quantity": 2},
    {"name": "Keyboard", "price": 79.99, "quantity": 1}
]

billing = calculate_billing(cart_items, tax_rate=0.08, discount_percent=5.0, shipping_method="express")
print("=== Billing Calculator ===")    # === Billing Calculator ===
print(f"Subtotal: ${billing['subtotal']}")    # Subtotal: $1139.96
print(f"Discount: -${billing['discount_amount']}")    # Discount: -$56.998
print(f"Tax: ${billing['tax_amount']}")    # Tax: $86.64
print(f"Shipping: ${billing['shipping_cost']}")    # Shipping: $15.00
print(f"Total: ${billing['total']}")    # Total: $1184.64


# Example 2: Decorator with Type Annotations
def retry(max_attempts: int = 3, delay: float = 1.0) -> Callable:
    """Decorator that retries a function on failure.
    
    Parameters
    ----------
    max_attempts : int, optional
        Maximum number of retry attempts (default: 3).
    delay : float, optional
        Delay in seconds between attempts (default: 1.0).
    
    Returns
    -------
    Callable
        Decorated function that retries on failure.
    
    Example
    -------
    >>> @retry(max_attempts=5, delay=2.0)
    ... def unstable_operation():
    ...     # Might fail sometimes
    ...     return "Success"
    """
    def decorator(func: Callable) -> Callable:
        def wrapper(*args: Any, **kwargs: Any) -> Any:
            import time
            last_exception = None
            
            for attempt in range(max_attempts):
                try:
                    return func(*args, **kwargs)
                except Exception as e:
                    last_exception = e
                    if attempt < max_attempts - 1:
                        time.sleep(delay)
            
            raise last_exception
        
        wrapper.__doc__ = func.__doc__
        return wrapper
    
    return decorator


@retry(max_attempts=3, delay=0.5)
def fetch_data(url: str) -> Dict[str, Any]:
    """Fetch data from a URL.
    
    Parameters
    ----------
    url : str
        The URL to fetch data from.
        
    Returns
    -------
    Dict[str, Any]
        Response data as dictionary.
    """
    # Simulated fetch
    return {"url": url, "data": "sample data"}


result = fetch_data("https://api.example.com/data")
print("\n=== Retry Decorator ===")    # (blank line)
print(f"Fetched: {result['url']}")    # Fetched: https://api.example.com/data


# Example 3: Context Manager with Full Documentation
class DatabaseTransaction:
    """Context manager for database transactions with automatic rollback.
    
    This class provides transaction management for database operations,
    automatically committing on success or rolling back on failure.
    
    Attributes
    ----------
    connection : Any
        Database connection object.
    committed : bool
        Whether the transaction was committed.
    
    Example
    -------
    >>> with DatabaseTransaction(conn) as txn:
    ...     conn.execute("INSERT INTO users VALUES (1, 'Alice')")
    ...     conn.execute("INSERT INTO emails VALUES (1, 'alice@example.com')")
    >>> print(txn.committed)
    True
    """
    
    def __init__(self, connection: Any):
        """Initialize the transaction context.
        
        Parameters
        ----------
        connection : Any
            Database connection object with execute and commit methods.
        """
        self.connection = connection
        self.committed = False
    
    def __enter__(self) -> 'DatabaseTransaction':
        """Enter the transaction context.
        
        Returns
        -------
        DatabaseTransaction
            Self reference for context management.
        """
        return self
    
    def __exit__(self, exc_type: Any, exc_val: Any, exc_tb: Any) -> bool:
        """Exit the transaction context.
        
        Parameters
        ----------
        exc_type : Any
            Exception type if an exception was raised.
        exc_val : Any
            Exception value if an exception was raised.
        exc_tb : Any
            Exception traceback if an exception was raised.
            
        Returns
        -------
        bool
            True to suppress exceptions, False otherwise.
        """
        if exc_type is None:
            self.connection.commit()
            self.committed = True
        else:
            self.connection.rollback()
            self.committed = False
        
        return False  # Don't suppress exceptions


class MockConnection:
    """Mock database connection for demonstration."""
    def __init__(self):
        self.rolled_back = False
        self.committed = False
    
    def commit(self) -> None:
        self.committed = True
    
    def rollback(self) -> None:
        self.rolled_back = True


conn = MockConnection()

print("\n=== Transaction Context Manager ===")    # (blank line)
with DatabaseTransaction(conn) as txn:
    print(f"Inside transaction, committed = {txn.committed}")    # Inside transaction, committed = False

print(f"After transaction, committed = {txn.committed}")    # After transaction, committed = True


# Example 4: Generic Class with Type Annotations
class EventBus:
    """Generic event bus for publish-subscribe messaging.
    
    This class implements a simple event bus allowing components to
    publish and subscribe to events of specific types.
    
    Type Parameters
    ----------
    T : Any
        Type of events this bus handles.
    
    Attributes
    ----------
    subscribers : Dict[str, List[Callable]]
        Map of event names to subscriber callbacks.
    
    Example
    -------
    >>> bus = EventBus()
    >>> bus.subscribe('user.created', lambda user: print(f'Created: {user}'))
    >>> bus.publish('user.created', {'id': 1, 'name': 'Alice'})
    Created: {'id': 1, 'name': 'Alice'}
    """
    
    def __init__(self) -> None:
        """Initialize the event bus."""
        self.subscribers: Dict[str, List[Callable[[Any], None]]] = {}
    
    def subscribe(self, event_name: str, callback: Callable[[Any], None]) -> None:
        """Subscribe to an event.
        
        Parameters
        ----------
        event_name : str
            Name of the event to subscribe to.
        callback : Callable[[Any], None]
            Function to call when event is published.
            
        Example
        -------
        >>> def on_user_created(user):
        ...     print(f"User created: {user['name']}")
        >>> bus.subscribe('user.created', on_user_created)
        """
        if event_name not in self.subscribers:
            self.subscribers[event_name] = []
        self.subscribers[event_name].append(callback)
    
    def publish(self, event_name: str, data: Any) -> None:
        """Publish an event to all subscribers.
        
        Parameters
        ----------
        event_name : str
            Name of the event to publish.
        data : Any
            Event data to pass to subscribers.
            
        Example
        -------
        >>> bus.publish('user.created', {'id': 1, 'name': 'Alice'})
        """
        if event_name in self.subscribers:
            for callback in self.subscribers[event_name]:
                callback(data)
    
    def unsubscribe(self, event_name: str, callback: Callable[[Any], None]) -> bool:
        """Unsubscribe from an event.
        
        Parameters
        ----------
        event_name : str
            Name of the event to unsubscribe from.
        callback : Callable[[Any], None]
            Callback function to remove.
            
        Returns
        -------
        bool
            True if callback was removed, False if not found.
        """
        if event_name in self.subscribers:
            try:
                self.subscribers[event_name].remove(callback)
                return True
            except ValueError:
                pass
        return False


bus = EventBus()

def on_order_created(order: Dict[str, Any]) -> None:
    """Handle order creation events."""
    print(f"Order created: #{order.get('id')} for ${order.get('total')}")

def on_order_shipped(order: Dict[str, Any]) -> None:
    """Handle order shipping events."""
    print(f"Order shipped: #{order.get('id')}")

bus.subscribe("order.created", on_order_created)
bus.subscribe("order.shipped", on_order_shipped)

print("\n=== Event Bus ===")    # (blank line)
bus.publish("order.created", {"id": "12345", "total": 99.99})    # Order created: #12345 for $99.99


# Example 5: Data Class with Comprehensive Annotations
from dataclasses import dataclass, field
from datetime import datetime
from typing import Optional


@dataclass
class Order:
    """Represents a customer order in the system.
    
    Attributes
    ----------
    order_id : str
        Unique identifier for the order.
    customer_id : str
        ID of the customer who placed the order.
    items : List[Dict[str, Any]]
        List of order items with name, quantity, and price.
    status : str
        Current status of the order (pending, processing, shipped, delivered).
    created_at : datetime
        Timestamp when the order was created.
    updated_at : datetime
        Timestamp when the order was last updated.
    """
    order_id: str
    customer_id: str
    items: List[Dict[str, Any]]
    status: str = "pending"
    created_at: datetime = field(default_factory=datetime.now)
    updated_at: datetime = field(default_factory=datetime.now)
    
    def calculate_total(self) -> float:
        """Calculate the total order amount.
        
        Returns
        -------
        float
            Sum of all item prices multiplied by quantities.
        """
        return sum(item.get("price", 0) * item.get("quantity", 1) for item in self.items)
    
    def update_status(self, new_status: str) -> None:
        """Update the order status.
        
        Parameters
        ----------
        new_status : str
            New status value.
            
        Raises
        ------
        ValueError
            If new_status is not a valid status.
        """
        valid_statuses = {"pending", "processing", "shipped", "delivered", "cancelled"}
        if new_status not in valid_statuses:
            raise ValueError(f"Invalid status. Must be one of: {valid_statuses}")
        
        self.status = new_status
        self.updated_at = datetime.now()
    
    def to_dict(self) -> Dict[str, Any]:
        """Convert order to dictionary.
        
        Returns
        -------
        Dict[str, Any]
            Dictionary representation of the order.
        """
        return {
            "order_id": self.order_id,
            "customer_id": self.customer_id,
            "items": self.items,
            "status": self.status,
            "total": self.calculate_total(),
            "created_at": self.created_at.isoformat(),
            "updated_at": self.updated_at.isoformat()
        }


order = Order(
    order_id="ORD-2024-001",
    customer_id="CUST-123",
    items=[
        {"name": "Python Book", "price": 49.99, "quantity": 1},
        {"name": "Coffee Mug", "price": 12.99, "quantity": 2}
    ]
)

print("\n=== Data Class Order ===")    # (blank line)
print(f"Order ID: {order.order_id}")    # Order ID: ORD-2024-001
print(f"Status: {order.status}")    # Status: pending
print(f"Total: ${order.calculate_total():.2f}")    # Total: $75.97

order.update_status("processing")
print(f"Updated Status: {order.status}")    # Updated Status: processing
print(f"Order Data: {json.dumps(order.to_dict(), indent=2, default=str)}")    # {...}
