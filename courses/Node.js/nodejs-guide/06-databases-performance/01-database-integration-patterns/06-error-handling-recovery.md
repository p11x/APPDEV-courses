# Database Error Handling and Recovery

## What You'll Learn

- Database error classification and handling
- Retry strategies with backoff
- Circuit breaker pattern for databases
- Connection recovery and reconnection
- Dead letter queue patterns

## Error Classification

```javascript
// Database error types
const ErrorTypes = {
    // Transient errors (retry may succeed)
    TRANSIENT: [
        'ECONNREFUSED',
        'ECONNRESET',
        'ETIMEDOUT',
        'PROTOCOL_CONNECTION_LOST',
        'ER_LOCK_DEADLOCK',
        'ER_LOCK_WAIT_TIMEOUT',
    ],
    
    // Permanent errors (retry won't help)
    PERMANENT: [
        'ER_DUP_ENTRY',         // Duplicate key
        'ER_NO_SUCH_TABLE',     // Table doesn't exist
        'ER_BAD_FIELD_ERROR',   // Column doesn't exist
        '23505',                // PostgreSQL unique violation
        '23503',                // PostgreSQL foreign key violation
    ],
    
    // Resource errors (may recover)
    RESOURCE: [
        'ER_CON_COUNT_ERROR',   // Too many connections
        'ER_OUTOFMEMORY',       // Out of memory
        '53300',                // PostgreSQL too many connections
    ],
};

class DatabaseError extends Error {
    constructor(message, { code, type, originalError, retryable = false } = {}) {
        super(message);
        this.name = 'DatabaseError';
        this.code = code;
        this.type = type;
        this.originalError = originalError;
        this.retryable = retryable;
        this.timestamp = new Date();
    }

    static fromNative(error) {
        const code = error.code || error.code?.toString();
        
        if (ErrorTypes.TRANSIENT.includes(code)) {
            return new DatabaseError(error.message, {
                code,
                type: 'TRANSIENT',
                originalError: error,
                retryable: true,
            });
        }
        
        if (ErrorTypes.PERMANENT.includes(code)) {
            return new DatabaseError(error.message, {
                code,
                type: 'PERMANENT',
                originalError: error,
                retryable: false,
            });
        }

        if (ErrorTypes.RESOURCE.includes(code)) {
            return new DatabaseError(error.message, {
                code,
                type: 'RESOURCE',
                originalError: error,
                retryable: true,
            });
        }

        return new DatabaseError(error.message, {
            code,
            type: 'UNKNOWN',
            originalError: error,
            retryable: false,
        });
    }
}
```

## Retry with Exponential Backoff

```javascript
class RetryPolicy {
    constructor(options = {}) {
        this.maxRetries = options.maxRetries || 3;
        this.baseDelay = options.baseDelay || 100;
        this.maxDelay = options.maxDelay || 5000;
        this.jitter = options.jitter !== false;
    }

    async execute(fn, context = 'operation') {
        let lastError;
        
        for (let attempt = 1; attempt <= this.maxRetries; attempt++) {
            try {
                return await fn();
            } catch (err) {
                const dbError = DatabaseError.fromNative(err);
                lastError = dbError;

                if (!dbError.retryable || attempt === this.maxRetries) {
                    throw dbError;
                }

                const delay = this.calculateDelay(attempt);
                console.warn(
                    `${context} failed (attempt ${attempt}/${this.maxRetries}), ` +
                    `retrying in ${delay}ms: ${err.message}`
                );
                await new Promise(r => setTimeout(r, delay));
            }
        }

        throw lastError;
    }

    calculateDelay(attempt) {
        const exponentialDelay = Math.min(
            this.baseDelay * Math.pow(2, attempt - 1),
            this.maxDelay
        );
        if (!this.jitter) return exponentialDelay;
        return exponentialDelay * (0.5 + Math.random() * 0.5);
    }
}

// Usage
const retry = new RetryPolicy({ maxRetries: 3, baseDelay: 200 });
const result = await retry.execute(
    () => pool.query('SELECT * FROM users WHERE id = $1', [userId]),
    'fetchUser'
);
```

## Circuit Breaker Pattern

```javascript
class CircuitBreaker {
    constructor(options = {}) {
        this.failureThreshold = options.failureThreshold || 5;
        this.resetTimeout = options.resetTimeout || 30000;
        this.halfOpenMaxAttempts = options.halfOpenMaxAttempts || 1;

        this.state = 'CLOSED';     // CLOSED, OPEN, HALF_OPEN
        this.failureCount = 0;
        this.lastFailureTime = null;
        this.successCount = 0;
    }

    async execute(fn) {
        if (this.state === 'OPEN') {
            if (Date.now() - this.lastFailureTime >= this.resetTimeout) {
                this.state = 'HALF_OPEN';
                console.log('Circuit breaker: HALF_OPEN');
            } else {
                throw new Error('Circuit breaker is OPEN - database unavailable');
            }
        }

        try {
            const result = await fn();
            this.onSuccess();
            return result;
        } catch (err) {
            this.onFailure();
            throw err;
        }
    }

    onSuccess() {
        if (this.state === 'HALF_OPEN') {
            this.successCount++;
            if (this.successCount >= this.halfOpenMaxAttempts) {
                this.state = 'CLOSED';
                this.failureCount = 0;
                this.successCount = 0;
                console.log('Circuit breaker: CLOSED (recovered)');
            }
        } else {
            this.failureCount = 0;
        }
    }

    onFailure() {
        this.failureCount++;
        this.lastFailureTime = Date.now();

        if (this.failureCount >= this.failureThreshold) {
            this.state = 'OPEN';
            console.error(`Circuit breaker: OPEN (${this.failureCount} failures)`);
        }
    }

    getStatus() {
        return {
            state: this.state,
            failureCount: this.failureCount,
            lastFailureTime: this.lastFailureTime,
        };
    }
}

// Usage with connection pool
const breaker = new CircuitBreaker({ failureThreshold: 5, resetTimeout: 30000 });

async function safeQuery(text, params) {
    return breaker.execute(() => pool.query(text, params));
}
```

## Connection Recovery Manager

```javascript
class ConnectionRecoveryManager {
    constructor(createPool, options = {}) {
        this.createPool = createPool;
        this.pool = null;
        this.maxReconnectAttempts = options.maxReconnectAttempts || 10;
        this.reconnectDelay = options.reconnectDelay || 1000;
        this.healthy = false;
        this.listeners = new Map();
    }

    async initialize() {
        this.pool = await this.createPool();
        this.healthy = true;
        this.setupListeners();
        return this.pool;
    }

    setupListeners() {
        this.pool.on('error', (err) => {
            console.error('Pool error:', err.message);
            this.healthy = false;
            this.attemptReconnect();
        });
    }

    async attemptReconnect() {
        for (let attempt = 1; attempt <= this.maxReconnectAttempts; attempt++) {
            try {
                console.log(`Reconnection attempt ${attempt}/${this.maxReconnectAttempts}`);
                
                if (this.pool) {
                    await this.pool.end().catch(() => {});
                }
                
                this.pool = await this.createPool();
                await this.pool.query('SELECT 1');
                
                this.healthy = true;
                this.setupListeners();
                this.emit('reconnected');
                console.log('Database reconnected successfully');
                return;
            } catch (err) {
                const delay = Math.min(
                    this.reconnectDelay * Math.pow(2, attempt - 1),
                    30000
                );
                console.error(`Reconnect attempt ${attempt} failed: ${err.message}`);
                await new Promise(r => setTimeout(r, delay));
            }
        }
        
        this.emit('reconnect_failed');
        throw new Error('Failed to reconnect to database');
    }

    on(event, listener) {
        if (!this.listeners.has(event)) this.listeners.set(event, []);
        this.listeners.get(event).push(listener);
    }

    emit(event, data) {
        const handlers = this.listeners.get(event) || [];
        handlers.forEach(handler => handler(data));
    }

    getPool() {
        if (!this.healthy) throw new Error('Database connection unhealthy');
        return this.pool;
    }
}

// Usage
const recoveryManager = new ConnectionRecoveryManager(
    () => new Pool({ host: 'localhost', database: 'myapp', max: 20 })
);

recoveryManager.on('reconnected', () => {
    console.log('Application resumed normal operation');
});

recoveryManager.on('reconnect_failed', () => {
    console.error('Critical: Database permanently unavailable');
    process.exit(1);
});

await recoveryManager.initialize();
```

## Dead Letter Queue Pattern

```javascript
class DeadLetterQueue {
    constructor(pool) {
        this.pool = pool;
    }

    async initialize() {
        await this.pool.query(`
            CREATE TABLE IF NOT EXISTS dead_letter_queue (
                id SERIAL PRIMARY KEY,
                operation_type VARCHAR(50) NOT NULL,
                payload JSONB NOT NULL,
                error_message TEXT,
                error_code VARCHAR(50),
                attempts INT DEFAULT 0,
                max_attempts INT DEFAULT 3,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                last_attempt TIMESTAMP,
                status VARCHAR(20) DEFAULT 'pending'
            )
        `);
    }

    async enqueue(operationType, payload, error) {
        await this.pool.query(
            `INSERT INTO dead_letter_queue (operation_type, payload, error_message, error_code)
             VALUES ($1, $2, $3, $4)`,
            [operationType, JSON.stringify(payload), error.message, error.code]
        );
    }

    async processPending(handler, batchSize = 10) {
        const { rows } = await this.pool.query(
            `SELECT * FROM dead_letter_queue 
             WHERE status = 'pending' AND attempts < max_attempts
             ORDER BY created_at ASC
             LIMIT $1`,
            [batchSize]
        );

        for (const item of rows) {
            try {
                await this.pool.query(
                    'UPDATE dead_letter_queue SET attempts = attempts + 1, last_attempt = NOW() WHERE id = $1',
                    [item.id]
                );
                
                await handler(item.operation_type, item.payload);
                
                await this.pool.query(
                    "UPDATE dead_letter_queue SET status = 'completed' WHERE id = $1",
                    [item.id]
                );
            } catch (err) {
                if (item.attempts + 1 >= item.max_attempts) {
                    await this.pool.query(
                        "UPDATE dead_letter_queue SET status = 'failed' WHERE id = $1",
                        [item.id]
                    );
                }
            }
        }
    }
}

// Usage: Wrap unreliable operations
async function safeOrderCreate(orderData) {
    try {
        return await createOrder(orderData);
    } catch (err) {
        const dbError = DatabaseError.fromNative(err);
        if (dbError.retryable) {
            await dlq.enqueue('create_order', orderData, dbError);
            return { queued: true };
        }
        throw err;
    }
}
```

## Global Error Handler Middleware

```javascript
function databaseErrorHandler() {
    return (err, req, res, next) => {
        if (!(err instanceof DatabaseError)) return next(err);

        const statusMap = {
            TRANSIENT: 503,
            RESOURCE: 503,
            PERMANENT: 400,
            UNKNOWN: 500,
        };

        const status = statusMap[err.type] || 500;

        console.error('Database error:', {
            type: err.type,
            code: err.code,
            message: err.message,
            retryable: err.retryable,
            path: req.path,
        });

        res.status(status).json({
            error: status === 503 ? 'Service temporarily unavailable' : 'Request failed',
            retryable: err.retryable,
            ...(err.retryable && { retryAfter: 5 }),
        });
    };
}

app.use(databaseErrorHandler());
```

## Best Practices Checklist

- [ ] Classify errors as transient vs permanent
- [ ] Implement retry with exponential backoff for transient errors
- [ ] Use circuit breaker to prevent cascade failures
- [ ] Implement connection recovery for long-running processes
- [ ] Use dead letter queue for failed write operations
- [ ] Log all database errors with context
- [ ] Return appropriate HTTP status codes
- [ ] Never expose internal error details to clients
- [ ] Monitor error rates and alert on thresholds
- [ ] Test error handling with fault injection

## Cross-References

- See [Connection Pooling](./04-connection-pooling.md) for pool health checks
- See [Transaction Management](./05-transaction-management.md) for transaction error handling
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for error tracking

## Next Steps

Continue to [Migration and Schema Management](./07-migration-schema-management.md) for database migration strategies.
