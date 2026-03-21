# Circuit Breaker Pattern

## 📌 What You'll Learn

- What the circuit breaker pattern is and why it prevents cascading failures
- Understanding the three states: closed, open, and half-open
- How to implement circuit breakers using the Opossum library
- How to configure thresholds and fallbacks

## 🧠 Concept Explained (Plain English)

Imagine you're running a restaurant and one of your suppliers (say, a bakery) is having problems — they're taking 30 seconds to answer the phone, or worse, not answering at all. If every customer order that needs bread triggers a call to this broken bakery, your restaurant will grind to a halt.

The solution? Put a circuit breaker between you and the bakery. It works like an electrical circuit breaker:

- **Closed state (Normal)**: Requests go through normally. Everything is fine.
- **Open state (Tripped)**: Requests don't go through at all. The circuit is "tripped" because too many failures happened. Instead of calling the broken bakery, you immediately return a fallback response (like "sorry, no bread today").
- **Half-open state (Testing)**: After some time, the circuit breaker tries again. If the supplier is working, it closes. If not, it opens again.

This pattern prevents **cascading failures** — where one failing service brings down your entire application.

**Key terms:**
- **Failure threshold**: How many failures before opening the circuit (e.g., 5 failures in 10 seconds)
- **Timeout**: How long to stay open before trying again (e.g., 30 seconds)
- **Fallback**: What to return when the circuit is open (cache data, error message, default value)

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import opossum from 'opossum';

const app = express();

// ============================================
// Circuit Breaker for External Payment Service
// ============================================

// Options for the payment service circuit breaker
const paymentServiceOptions = {
  // Open circuit after 5 failures
  volumeThreshold: 5,
  
  // Time in ms before trying again (default: 60000)
  timeout: 30000,
  
  // Percentage of failures to trigger circuit (0-1)
  errorThresholdPercentage: 50,
  
  // Reset timeout after each success
  resetTimeoutEnabled: true,
  
  // Allow 3 requests through in half-open state to test
  halfOpenTime: 10000
};

// Function that might fail
async function callPaymentService(amount) {
  // Simulate external API call - may fail randomly
  await new Promise((resolve, reject) => {
    setTimeout(() => {
      // 30% chance of failure
      if (Math.random() < 0.3) {
        reject(new Error('Payment service unavailable'));
      } else {
        resolve({ transactionId: 'txn-' + Date.now(), status: 'success' });
      }
    }, 100);
  });
}

// Create circuit breaker
const paymentBreaker = new opossum(callPaymentService, paymentServiceOptions);

// Circuit state event handlers
paymentBreaker.on('open', () => {
  console.log('🔴 Circuit breaker OPENED - Payment service unavailable');
});

paymentBreaker.on('close', () => {
  console.log('🟢 Circuit breaker CLOSED - Payment service recovered');
});

paymentBreaker.on('halfOpen', () => {
  console.log '🟡 Circuit breaker HALF-OPEN - Testing payment service');
});


// Fallback function - what to return when circuit is open
function paymentFallback(amount, error) {
  console.log('Using fallback for payment:', error?.message);
  return {
    status: 'pending',
    message: 'Payment processing is temporarily unavailable. Please try again later.',
    fallback: true
  };
}


// ============================================
// Circuit Breaker for Database Queries
// ============================================

const databaseOptions = {
  volumeThreshold: 10,
  timeout: 10000,
  errorThresholdPercentage: 50,
  resetTimeoutEnabled: true
};

async function queryDatabase(query) {
  // Simulate database query
  await new Promise((resolve, reject) => {
    setTimeout(() => {
      if (Math.random() < 0.2) {
        reject(new Error('Database connection timeout'));
      } else {
        resolve({ rows: [{ id: 1, name: 'Test' }] });
      }
    }, 50);
  });
}

const dbBreaker = new opossum(queryDatabase, databaseOptions);

dbBreaker.on('open', () => {
  console.log('🔴 Database circuit breaker OPENED');
});

dbBreaker.on('close', () => {
  console.log('🟢 Database circuit breaker CLOSED');
});


// ============================================
// Circuit Status Endpoint
// ============================================
app.get('/circuit-status', (req, res) => {
  const status = {
    payment: {
      state: paymentBreaker.status.state,
      failures: paymentBreaker.status.failures,
      successes: paymentBreaker.status.successes,
      isOpen: paymentBreaker.isOpen
    },
    database: {
      state: dbBreaker.status.state,
      failures: dbBreaker.status.failures,
      isOpen: dbBreaker.isOpen
    }
  };
  
  res.json(status);
});


// ============================================
// Routes Using Circuit Breakers
// ============================================

// Payment endpoint with circuit breaker
app.post('/api/payments', async (req, res) => {
  const { amount } = req.body || {};
  
  if (!amount) {
    return res.status(400).json({ error: 'Amount is required' });
  }
  
  try {
    // fire() returns a Promise - automatically uses circuit breaker
    const result = await paymentBreaker.fire(amount);
    
    res.json(result);
  } catch (error) {
    // This catches both the fallback result AND actual errors
    if (error?.fallback) {
      return res.status(503).json({
        error: error.message,
        retry_after: 30
      });
    }
    
    res.status(500).json({ error: 'Payment failed' });
  }
});


// Database query endpoint
app.get('/api/users/:id', async (req, res) => {
  const userId = req.params.id;
  
  try {
    const result = await dbBreaker.fire(`SELECT * FROM users WHERE id = ${userId}`);
    res.json(result.rows[0]);
  } catch (error) {
    if (dbBreaker.isOpen) {
      return res.status(503).json({
        error: 'Service temporarily unavailable',
        cached: false
      });
    }
    res.status(500).json({ error: 'Database error' });
  }
});


// Bulk operation - fire multiple calls
app.post('/api/bulk-users', async (req, res) => {
  const { userIds } = req.body || [];
  
  const results = await Promise.allSettled(
    userIds.map(id => dbBreaker.fire(`SELECT * FROM users WHERE id = ${id}`))
  );
  
  const successful = results.filter(r => r.status === 'fulfilled');
  const failed = results.filter(r => r.status === 'rejected');
  
  res.json({
    total: userIds.length,
    successful: successful.length,
    failed: failed.length,
    results: successful.map(r => r.value)
  });
});


// Cache fallback example
const userCache = new Map();

app.get('/api/products/:id', async (req, res) => {
  const productId = req.params.id;
  
  // Try cache first
  if (userCache.has(productId)) {
    return res.json({ 
      ...userCache.get(productId), 
      cached: true 
    });
  }
  
  try {
    // Attempt database query through circuit breaker
    const result = await dbBreaker.fire(`SELECT * FROM products WHERE id = ${productId}`);
    userCache.set(productId, result.rows[0]);
    res.json({ ...result.rows[0], cached: false });
  } catch (error) {
    // Return cached data if available, even if stale
    if (userCache.has(productId)) {
      return res.json({ 
        ...userCache.get(productId), 
        cached: true,
        stale: true
      });
    }
    
    res.status(503).json({ error: 'Service unavailable' });
  }
});


// Health endpoint
app.get('/health', (req, res) => {
  const healthy = !paymentBreaker.isOpen && !dbBreaker.isOpen;
  res.status(healthy ? 200 : 503).json({ 
    status: healthy ? 'ok' : 'degraded',
    circuits: {
      payment: paymentBreaker.status.state,
      database: dbBreaker.status.state
    }
  });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
  console.log(`Check circuit status at http://localhost:${PORT}/circuit-status`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 10-18 | `paymentServiceOptions` | Configuration for payment service breaker |
| 13 | `volumeThreshold` | Number of failures before opening |
| 15 | `timeout` | How long circuit stays open |
| 17 | `errorThresholdPercentage` | Failure % threshold |
| 21-33 | `callPaymentService()` | Function that may fail (wrapped in breaker) |
| 38 | `new opossum(fn, options)` | Creates circuit breaker wrapper |
| 41-52 | Event handlers | Logs state changes |
| 56-60 | `paymentFallback()` | Returns fallback when circuit is open |
| 68-77 | Database breaker | Separate breaker for database operations |
| 86-99 | `/circuit-status` endpoint | Shows current state of all breakers |
| 115-130 | `/api/payments` route | Uses payment breaker with fallback |
| 134-148 | `/api/users/:id` route | Database query with circuit breaker |
| 164-179 | `/api/bulk-users` route | Bulk operations with allSettled |
| 185-209 | `/api/products/:id` route | Cache fallback when DB circuit open |
| 213-224 | `/health` endpoint | Health check considering circuit states |

## ⚠️ Common Mistakes

### 1. Not providing fallbacks

**What it is**: Circuit opens and all requests fail with 500 errors.

**Why it happens**: Forgetting to implement fallback logic.

**How to fix it**: Always provide fallback behavior — return cached data, default values, or meaningful error messages.

### 2. Using same breaker for unrelated operations

**What it is**: One failure type trips the breaker for everything.

**Why it happens**: Grouping unrelated calls under one circuit breaker.

**How to fix it**: Create separate breakers for different dependencies (payment service, database, external APIs).

### 3. Thresholds too strict or too lenient

**What it is**: Circuit opens for minor issues, or never opens during real outages.

**Why it happens**: Not tuning based on actual failure patterns.

**How to fix it**: Start with defaults, monitor, and adjust based on production behavior. Consider time of day and traffic patterns.

## ✅ Quick Recap

- Circuit breakers prevent cascading failures from external service outages
- Three states: closed (normal), open (blocked), half-open (testing)
- Opossum library provides easy circuit breaker implementation for Node.js
- Always provide fallback behavior when circuit opens
- Monitor circuit state via health endpoints
- Different services should have separate circuit breakers

## 🔗 What's Next

Now that you can prevent cascading failures, learn about [Retry with Exponential Backoff](./03_retry-with-exponential-backoff.md) to handle transient failures.
