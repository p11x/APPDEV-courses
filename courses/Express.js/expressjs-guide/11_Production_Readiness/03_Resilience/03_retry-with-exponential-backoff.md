# Retry with Exponential Backoff

## 📌 What You'll Learn

- What transient failures are and when to retry
- How exponential backoff works to handle retries gracefully
- Using the async-retry library for reliable retries
- Adding jitter to prevent thundering herd problems

## 🧠 Concept Explained (Plain English)

Sometimes operations fail not because something is fundamentally broken, but because of temporary issues — a brief network blip, a server briefly overloaded, a database connection timing out. These are called **transient failures** and they often fix themselves if you wait a moment and try again.

**Retry logic** automatically re-attempts failed operations. But naive retry (try, fail, try immediately again, fail again) can make things worse:

1. You might hammer a struggling service with more requests
2. All your retries might hit at the same time as retries from other clients (**thundering herd**)
3. You waste resources retrying immediately when a delay would help

**Exponential backoff** solves this by waiting longer between each retry:
- Retry 1: wait 1 second
- Retry 2: wait 2 seconds
- Retry 3: wait 4 seconds
- Retry 4: wait 8 seconds
- ...

Each wait time doubles (or multiplies by some factor), giving the failing service time to recover.

**Jitter** (randomness) adds variation to wait times so that multiple clients don't all retry at exactly the same moment:
- Instead of everyone waiting exactly 1 second, they wait 0.8s, 1.2s, 1.1s, etc.

**When to retry:**
- Network timeouts
- Connection errors
- 5xx errors from external APIs (server errors, not client errors)
- Rate limiting (429 responses) — wait and retry after the specified time

**When NOT to retry:**
- 4xx errors (bad request, unauthorized) — the problem is your request, not the server
- Validation errors — the request will never succeed
- Operations that are already processed (idempotency issues)

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import retry from 'async-retry';

const app = express();


// ============================================
// Retry Configuration
// ============================================

// Configuration for external API calls
const apiRetryOptions = {
  // Maximum number of attempts
  retries: 3,
  
  // Initial delay in milliseconds
  factor: 2, // exponential factor
  
  // Minimum delay (useful when using jitter)
  minTimeout: 1000,
  
  // Maximum delay cap
  maxTimeout: 10000,
  
  // Randomize timeout (jitter)
  randomize: true,
  
  // Optional: callback on retry
  onRetry: (error, attempt) => {
    console.log(`Retry attempt ${attempt} after error: ${error.message}`);
  }
};

// Configuration for database operations
const dbRetryOptions = {
  retries: 3,
  factor: 1.5, // less aggressive for DB
  minTimeout: 500,
  maxTimeout: 5000,
  randomize: false
};


// ============================================
// Simulated External Services
// ============================================

let callCount = 0;

async function callPaymentAPI(amount) {
  callCount++;
  const attempt = callCount;
  
  console.log(`Payment API call #${attempt}`);
  
  // Simulate failures for first 2 attempts
  if (attempt <= 2) {
    throw new Error('Payment service temporarily unavailable');
  }
  
  return { transactionId: 'txn-' + Date.now(), status: 'success' };
}

async function fetchUserFromAPI(userId) {
  // Simulate random failures
  if (Math.random() < 0.3) {
    throw new Error('User service timeout');
  }
  
  return { id: userId, name: 'John Doe', email: 'john@example.com' };
}

async function sendNotification(message) {
  if (Math.random() < 0.4) {
    throw new Error('Notification service error');
  }
  
  return { sent: true, messageId: 'notif-' + Date.now() };
}


// ============================================
// Retry Wrapper Functions
// ============================================

async function retryPayment(amount) {
  return await retry(async (bail) => {
    try {
      return await callPaymentAPI(amount);
    } catch (error) {
      // Don't retry on certain errors (non-transient)
      if (error.message.includes('invalid') || error.message.includes('unauthorized')) {
        // Bail out immediately without retrying
        return bail(error);
      }
      
      // Re-throw for retry
      throw error;
    }
  }, apiRetryOptions);
}

async function retryDatabaseOperation(operation) {
  return await retry(async (bail) => {
    try {
      return await operation();
    } catch (error) {
      // Don't retry validation errors
      if (error.name === 'ValidationError') {
        return bail(error);
      }
      throw error;
    }
  }, dbRetryOptions);
}


// ============================================
// Manual Retry Implementation (for understanding)
// ============================================

async function retryWithBackoff(fn, options = {}) {
  const {
    maxRetries = 3,
    initialDelay = 1000,
    maxDelay = 10000,
    factor = 2,
    onRetry = () => {}
  } = options;
  
  let lastError;
  
  for (let attempt = 1; attempt <= maxRetries; attempt++) {
    try {
      return await fn();
    } catch (error) {
      lastError = error;
      
      // Don't retry on last attempt
      if (attempt === maxRetries) {
        break;
      }
      
      // Calculate delay with jitter
      const delay = Math.min(
        initialDelay * Math.pow(factor, attempt - 1) * (0.5 + Math.random()),
        maxDelay
      );
      
      console.log(`Retry ${attempt}/${maxRetries} after ${Math.round(delay)}ms`);
      onRetry(error, attempt);
      
      await new Promise(resolve => setTimeout(resolve, delay));
    }
  }
  
  throw lastError;
}


// ============================================
// Routes
// ============================================

app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});


// Payment with retry
app.post('/api/payments', async (req, res) => {
  const { amount } = req.body || {};
  
  if (!amount) {
    return res.status(400).json({ error: 'Amount required' });
  }
  
  try {
    const result = await retryPayment(amount);
    res.json(result);
  } catch (error) {
    console.error('Payment failed after retries:', error.message);
    res.status(503).json({ 
      error: 'Payment service unavailable',
      message: error.message
    });
  }
});


// User fetch with retry
app.get('/api/users/:id', async (req, res) => {
  const userId = req.params.id;
  
  try {
    const user = await retry(async (bail) => {
      return await fetchUserFromAPI(userId);
    }, {
      retries: 3,
      factor: 2,
      minTimeout: 500,
      maxTimeout: 5000,
      randomize: true,
      onRetry: (err, attempt) => {
        console.log(`Fetching user ${userId}, attempt ${attempt}: ${err.message}`);
      }
    });
    
    res.json(user);
  } catch (error) {
    res.status(502).json({ error: 'User service unavailable' });
  }
});


// Batch operations with individual retries
app.post('/api/notifications', async (req, res) => {
  const { messages } = req.body || [];
  
  const results = await Promise.allSettled(
    messages.map(msg => 
      retry(async () => await sendNotification(msg), {
        retries: 2,
        minTimeout: 500,
        maxTimeout: 2000
      })
    )
  );
  
  const successful = results.filter(r => r.status === 'fulfilled');
  const failed = results.filter(r => r.status === 'rejected');
  
  res.json({
    total: messages.length,
    sent: successful.length,
    failed: failed.length,
    errors: failed.map(r => r.reason.message)
  });
});


// Manual retry example
app.post('/api/process', async (req, res) => {
  const { data } = req.body || {};
  
  try {
    const result = await retryWithBackoff(
      async () => {
        if (Math.random() < 0.3) {
          throw new Error('Processing failed');
        }
        return { processed: true, data };
      },
      {
        maxRetries: 5,
        initialDelay: 500,
        factor: 2,
        onRetry: (err, attempt) => {
          console.log(`Process retry ${attempt}`);
        }
      }
    );
    
    res.json(result);
  } catch (error) {
    res.status(500).json({ error: 'Processing failed' });
  }
});


// Retry status endpoint
app.get('/retry-status', (req, res) => {
  res.json({
    message: 'Uses async-retry library for exponential backoff',
    config: apiRetryOptions
  });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 14-30 | `apiRetryOptions` | Configuration for API retry behavior |
| 15 | `retries` | Maximum number of retry attempts |
| 16-17 | `factor` | Exponential multiplier (2 = double each time) |
| 19-20 | `minTimeout`, `maxTimeout` | Bounds for delay calculation |
| 22-25 | `randomize` | Enables jitter |
| 27-29 | `onRetry` | Callback when retry happens |
| 35-45 | `dbRetryOptions` | Less aggressive settings for database |
| 56-71 | `retryPayment()` | Wraps payment API with retry logic |
| 63 | `bail(error)` | Immediately stops retries for non-transient errors |
| 77-89 | `retryDatabaseOperation()` | Generic retry wrapper for DB |
| 95-125 | `retryWithBackoff()` | Manual implementation showing how retries work |
| 115 | `Math.pow(factor, attempt)` | Calculates exponential delay |
| 118 | `0.5 + Math.random()` | Adds jitter |
| 159-174 | `/api/payments` route | Uses retry wrapper for payment calls |
| 177-198 | `/api/users/:id` route | In-line retry configuration |
| 201-216 | `/api/notifications` route | Batch with individual retries |
| 218-236 | `/api/process` route | Uses manual retry implementation |

## ⚠️ Common Mistakes

### 1. Retrying non-transient errors

**What it is**: Retrying validation errors, 400 responses, or authentication failures.

**Why it happens**: Treating all errors the same.

**How to fix it**: Use `bail()` to stop retries for client errors that will never succeed.

### 2. No maximum retry limit

**What it is**: Retry forever, hanging requests indefinitely.

**Why it happens**: Not setting a `retries` limit.

**How to fix it**: Always set a reasonable `retries` limit (3-5 is usually good).

### 3. Not handling idempotency

**What it is**: Retrying creates duplicate operations (e.g., charging a card twice).

**Why it happens**: Not understanding that retry might result in multiple executions.

**How to fix it**: Use idempotency keys (covered in API Reliability section) for operations that shouldn't be repeated.

## ✅ Quick Recap

- Retry transient failures (timeouts, network errors, 5xx errors)
- Exponential backoff increases delay between retries (1s, 2s, 4s, 8s...)
- Jitter adds randomness to prevent thundering herd
- Use `bail()` to stop retries for non-transient errors
- Set reasonable limits to prevent infinite retries
- Consider idempotency for operations that shouldn't be repeated

## 🔗 What's Next

Now that you can handle transient failures, learn about [Memory Leak Detection](./04_memory-leak-detection.md) to find and fix memory issues.
