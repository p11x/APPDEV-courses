# EventEmitter in Express

## 📌 What You'll Learn

- How Node.js EventEmitter works for decoupled communication
- Creating custom events in Express applications
- Using typed events for better TypeScript integration
- Building event-driven architectures for scalability

## 🧠 Concept Explained (Plain English)

Express is built on top of Node.js's EventEmitter — when you call `res.on('finish', callback)`, you're using events! But you can also create your own custom events to decouple parts of your application.

**Why use custom events?** Imagine you have a checkout process:
- User places order
- Email is sent
- Inventory is updated  
- Payment is processed
- Analytics are tracked

Instead of calling all these sequentially in your route handler, you can **emit** an order-created event and let each service listen and handle its part independently. This is called **loose coupling** — the checkout code doesn't need to know how emails work or how analytics are tracked.

**Key EventEmitter methods:**
- `emit(eventName, data)` — triggers an event
- `on(eventName, listener)` — listens for events (persists)
- `once(eventName, listener)` — listens once, then auto-removes
- `removeListener(eventName, listener)` — stops listening

**Typed events** add TypeScript type safety, so you know exactly what data each event carries.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import { EventEmitter } from 'events';

const app = express();


// ============================================
// Custom Event Bus
// ============================================

// Create a custom EventEmitter for application events
class ApplicationEventBus extends EventEmitter {
  constructor() {
    super();
    
    // Set max listeners to avoid warnings with many subscribers
    this.setMaxListeners(20);
  }
  
  // Helper method to emit with logging
  emitEvent(eventName, data) {
    console.log(`📢 Event emitted: ${eventName}`, data ? JSON.stringify(data).slice(0, 100) : '');
    return this.emit(eventName, data);
  }
}

// Create singleton event bus
const events = new ApplicationEventBus();


// ============================================
// Event Types (for documentation/TypeScript)
// ============================================

/*
// TypeScript types would look like:
type OrderEvent = {
  orderId: string;
  userId: string;
  total: number;
  items: Array<{ productId: string; quantity: number }>;
};

type UserEvent = {
  userId: string;
  email: string;
  name: string;
};

type PaymentEvent = {
  orderId: string;
  amount: number;
  status: 'success' | 'failed';
};
*/


// ============================================
// Event Listeners (Handlers)
// ============================================

// Order created handler
events.on('order:created', (order) => {
  console.log('📦 Processing order:', order.orderId);
  
  // Simulate sending confirmation email
  setTimeout(() => {
    console.log(`   📧 Confirmation email sent to customer`);
  }, 100);
});

// Inventory update handler
events.on('order:created', (order) => {
  console.log('📦 Updating inventory for order:', order.orderId);
  
  order.items.forEach(item => {
    console.log(`   📉 Decreased stock for product ${item.productId}`);
  });
});

// Analytics tracking handler
events.on('order:created', (order) => {
  console.log('📊 Recording analytics event');
  // In production: await analytics.track('order_created', order);
});

// Payment success handler
events.on('payment:completed', (payment) => {
  console.log(`💰 Payment received: $${payment.amount}`);
});

// User registration handler
events.on('user:registered', (user) => {
  console.log(`👤 New user registered: ${user.email}`);
});


// ============================================
// Middleware to track events
// ============================================

// Add event bus to request for easy access
app.use((req, res, next) => {
  req.events = events;
  next();
});


// ============================================
// Routes that Emit Events
// ============================================

app.post('/api/orders', express.json(), async (req, res) => {
  const { userId, items, total } = req.body || {};
  
  // Create order
  const order = {
    orderId: 'order-' + Date.now(),
    userId,
    items: items || [],
    total: total || 0,
    createdAt: new Date().toISOString()
  };
  
  console.log('\n🆕 New order created:', order.orderId);
  
  // Emit order created event (async, non-blocking)
  // All listeners run in parallel
  events.emitEvent('order:created', order);
  
  // Respond immediately without waiting for handlers
  res.status(201).json({ 
    orderId: order.orderId, 
    status: 'processing' 
  });
});


app.post('/api/payments', express.json(), async (req, res) => {
  const { orderId, amount, method } = req.body || {};
  
  // Simulate payment processing
  const success = Math.random() > 0.2;
  
  const payment = {
    paymentId: 'pay-' + Date.now(),
    orderId,
    amount,
    method,
    status: success ? 'success' : 'failed',
    timestamp: new Date().toISOString()
  };
  
  // Emit payment event
  events.emitEvent('payment:completed', payment);
  
  res.json(payment);
});


app.post('/api/users', express.json(), async (req, res) => {
  const { email, name } = req.body || {};
  
  const user = {
    userId: 'user-' + Date.now(),
    email,
    name,
    createdAt: new Date().toISOString()
  };
  
  // Emit user registered event
  events.emitEvent('user:registered', user);
  
  res.status(201).json(user);
});


// ============================================
// One-time Event Examples
// ============================================

app.get('/api/one-time', (req, res) => {
  // This listener will only fire once
  events.once('test:once', (data) => {
    console.log('This should only fire once:', data);
  });
  
  // Emit the event
  events.emit('test:once', { message: 'First emission' });
  events.emit('test:once', { message: 'Second emission (will be ignored)' });
  
  res.json({ message: 'One-time event triggered' });
});


// ============================================
// Remove Listeners Example
// ============================================

let requestCount = 0;
const requestHandler = (req) => {
  requestCount++;
  console.log(`Request #${requestCount}:`, req.path);
};

// Add listener
events.on('request:logged', requestHandler);

// Remove listener
setTimeout(() => {
  console.log('Removing request handler');
  events.removeListener('request:logged', requestHandler);
}, 10000);


// ============================================
// Error handling for events
// ============================================

events.on('error', (err) => {
  console.error('Event error:', err.message);
});


// ============================================
// Health Check
// ============================================

app.get('/health', (req, res) => {
  res.json({ 
    status: 'ok',
    eventCount: events.listenerCount('order:created'),
    listeners: {
      'order:created': events.listenerCount('order:created'),
      'payment:completed': events.listenerCount('payment:completed'),
      'user:registered': events.listenerCount('user:registered')
    }
  });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log('\n📝 Try these:');
  console.log(`   curl -X POST http://localhost:${PORT}/api/orders`);
  console.log(`   curl -X POST http://localhost:${PORT}/api/payments`);
  console.log(`   curl -X POST http://localhost:${PORT}/api/users`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 11-18 | `ApplicationEventBus` class | Custom EventEmitter with helper method |
| 26-29 | `setMaxListeners` | Prevents warnings with many subscribers |
| 55-62 | Order listener | Handles order:created events |
| 65-71 | Inventory listener | Updates stock for each item |
| 74-77 | Analytics listener | Tracks order in analytics |
| 84-89 | Payment listener | Handles completed payments |
| 92-96 | User listener | Handles new user registration |
| 107-110 | Request middleware | Attaches event bus to request object |
| 130-140 | Order route | Emits `order:created` event |
| 142 | `emitEvent()` | Triggers all listeners |
| 145 | Responds immediately | Non-blocking event emission |
| 169-179 | Payment route | Emits `payment:completed` event |
| 193-208 | One-time events | Using `.once()` for single-fire listeners |
| 212-221 | Remove listeners | Cleaning up listeners |
| 228-232 | Error handler | Catches event errors |

## ⚠️ Common Mistakes

### 1. Forgetting error handling on events

**What it is**: One listener throwing error breaks all other listeners.

**Why it happens**: EventEmitter doesn't automatically catch errors in listeners.

**How to fix it**: Add error handler: `events.on('error', handler)`. Wrap listener code in try/catch.

### 2. Blocking the event loop

**What it is**: Synchronous listeners delay response to client.

**Why it happens**: Putting heavy computation in event handlers.

**How to fix it**: Make handlers async or use a job queue (covered next).

### 3. Memory leaks from listeners

**What it is**: Adding listeners without removing them, accumulating memory.

**Why it happens**: Not cleaning up listeners that should be temporary.

**How to fix it**: Use `.once()` for one-time handlers, or explicitly remove with `.removeListener()`.

## ✅ Quick Recap

- EventEmitter enables decoupled communication between components
- Emit events to trigger multiple handlers simultaneously
- Use `.on()` for persistent listeners, `.once()` for one-time events
- Attach event bus to `req.events` for easy access in routes
- Always add error handlers for the EventEmitter
- Keep handlers fast and non-blocking

## 🔗 What's Next

Now that you understand events, learn about [Message Queues with BullMQ](./02_message-queues-with-bullmq.md) for handling background jobs reliably.
