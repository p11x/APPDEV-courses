# 💼 Control Flow Examples

## 📋 Table of Contents

1. [Overview](#overview)
2. [User Authentication Flow](#user-authentication-flow)
3. [Data Processing Pipeline](#data-processing-pipeline)
4. [Form Validation System](#form-validation-system)
5. [State Management](#state-management)
6. [Event Processing](#event-processing)
7. [Error Handling](#error-handling)
8. [Best Practices](#best-practices)
9. [Key Takeaways](#key-takeaways)

---

## Overview

This practical guide demonstrates real-world control flow patterns used in production applications. Each example shows how to combine if-else statements, loops, logical operators, and jump statements to solve common programming challenges.

---

## User Authentication Flow

### Complete Authentication System

```javascript
// File: auth-flow.js
// Description: Complete authentication flow

class AuthenticationSystem {
    constructor() {
        this.maxLoginAttempts = 3;
        this.lockoutTime = 15 * 60 * 1000; // 15 minutes
    }

    async login(username, password) {
        // Step 1: Validate input
        if (!this.validateInput(username, password)) {
            return { success: false, error: 'Invalid input' };
        }

        // Step 2: Check account lockout
        const user = await this.getUser(username);
        if (this.isLocked(user)) {
            return { 
                success: false, 
                error: 'Account locked',
                lockedUntil: user.lockedUntil 
            };
        }

        // Step 3: Verify credentials
        if (!this.verifyCredentials(user, password)) {
            return this.handleFailedLogin(user);
        }

        // Step 4: Check password expiry
        if (this.isPasswordExpired(user)) {
            return {
                success: false,
                requirePasswordChange: true
            };
        }

        // Step 5: Generate token
        return this.handleSuccessfulLogin(user);
    }

    validateInput(username, password) {
        return (
            typeof username === 'string' &&
            username.length >= 3 &&
            typeof password === 'string' &&
            password.length >= 8
        );
    }

    async getUser(username) {
        // Simulate database lookup
        return null;
    }

    isLocked(user) {
        return (
            user?.lockedUntil && 
            user.lockedUntil > Date.now()
        );
    }

    verifyCredentials(user, password) {
        // In production, use proper password hashing
        return user?.passwordHash === password;
    }

    handleFailedLogin(user) {
        const attempts = (user?.failedAttempts || 0) + 1;
        
        if (attempts >= this.maxLoginAttempts) {
            return {
                success: false,
                error: 'Account locked due to too many attempts',
                lockedUntil: Date.now() + this.lockoutTime
            };
        }

        return {
            success: false,
            error: `Invalid credentials. ${this.maxLoginAttempts - attempts} attempts remaining.`
        };
    }

    isPasswordExpired(user) {
        const maxAge = 90 * 24 * 60 * 60 * 1000;
        return Date.now() - user.passwordChangedAt > maxAge;
    }

    handleSuccessfulLogin(user) {
        return {
            success: true,
            token: 'jwt-token-here',
            user: {
                id: user.id,
                username: user.username,
                role: user.role
            }
        };
    }
}
```

---

## Data Processing Pipeline

### Multi-Stage Pipeline

```javascript
// File: data-pipeline.js
// Description: Data processing pipeline

class DataPipeline {
    constructor(stages = []) {
        this.stages = stages;
    }

    addStage(name, processor, options = {}) {
        this.stages.push({
            name,
            processor,
            async: options.async || false
        });
        return this;
    }

    async process(data) {
        let result = data;
        const logs = [];

        for (const stage of this.stages) {
            try {
                logs.push(`Starting stage: ${stage.name}`);
                
                if (stage.async) {
                    result = await stage.processor(result);
                } else {
                    result = stage.processor(result);
                }

                logs.push(`Completed stage: ${stage.name}`);
            } catch (error) {
                logs.push(`Error in stage ${stage.name}: ${error.message}`);
                
                if (!options.continueOnError) {
                    return { success: false, error: error.message, logs };
                }
            }
        }

        return { success: true, data: result, logs };
    }
}

// Usage
const pipeline = new DataPipeline();

pipeline
    .addStage('validate', (data) => {
        if (!data || typeof data !== 'object') {
            throw new Error('Invalid data format');
        }
        return data;
    })
    .addStage('transform', (data) => ({
        ...data,
        processed: true,
        timestamp: Date.now()
    }))
    .addStage('enrich', async (data) => {
        // Simulate API call
        return { ...data, source: 'pipeline' };
    })
    .addStage('validate-output', (data) => {
        if (!data.processed) {
            throw new Error('Processing incomplete');
        }
        return data;
    });

const result = pipeline.process({ name: 'test' });
console.log(result);
```

---

## Form Validation System

### Comprehensive Validator

```javascript
// File: form-validator.js
// Description: Form validation system

class FormValidator {
    constructor(schema) {
        this.schema = schema;
        this.errors = {};
    }

    validate(data) {
        this.errors = {};

        for (const [field, rules] of Object.entries(this.schema)) {
            const value = data[field];

            // Required check
            if (rules.required && !value) {
                this.addError(field, 'required');
                continue;
            }

            // Skip other checks if empty and not required
            if (!value) continue;

            // Type check
            if (rules.type && typeof value !== rules.type) {
                this.addError(field, 'invalid type');
            }

            // Min length
            if (rules.minLength && value.length < rules.minLength) {
                this.addError(field, `minimum ${rules.minLength} characters`);
            }

            // Max length
            if (rules.maxLength && value.length > rules.maxLength) {
                this.addError(field, `maximum ${rules.maxLength} characters`);
            }

            // Pattern
            if (rules.pattern && !rules.pattern.test(value)) {
                this.addError(field, 'invalid format');
            }

            // Custom validator
            if (rules.validator && !rules.validator(value)) {
                this.addError(field, 'validation failed');
            }

            // Custom function
            if (rules.custom) {
                const customResult = rules.custom(value, data);
                if (customResult !== true) {
                    this.addError(field, customResult || 'invalid');
                }
            }
        }

        return {
            valid: Object.keys(this.rules).length === 0,
            errors: this.errors
        };
    }

    addError(field, message) {
        if (!this.errors[field]) {
            this.errors[field] = [];
        }
        this.errors[field].push(message);
    }
}

// Usage
const validator = new FormValidator({
    username: {
        required: true,
        type: 'string',
        minLength: 3,
        maxLength: 20,
        pattern: /^[a-zA-Z0-9_]+$/
    },
    email: {
        required: true,
        type: 'string',
        custom: (value) => {
            const emailRegex = /^[^@]+@[^@]+\.[^@]+$/;
            return emailRegex.test(value) || 'Invalid email format';
        }
    },
    password: {
        required: true,
        type: 'string',
        minLength: 8,
        custom: (value) => {
            if (!/[A-Z]/.test(value)) return 'Must contain uppercase';
            if (!/[a-z]/.test(value)) return 'Must contain lowercase';
            if (!/[0-9]/.test(value)) return 'Must contain number';
            return true;
        }
    },
    age: {
        required: false,
        type: 'number',
        custom: (value) => {
            if (value < 13) return 'Must be at least 13 years old';
            if (value > 120) return 'Invalid age';
            return true;
        }
    }
});

const result = validator.validate({
    username: 'john',
    email: 'john@example.com',
    password: 'Password123',
    age: 25
});

console.log(result);
```

---

## State Management

### State Machine

```javascript
// File: state-machine.js
// Description: Application state machine

class StateMachine {
    constructor(initialState, transitions) {
        this.state = initialState;
        this.transitions = transitions;
        this.history = [initialState];
    }

    transition(event) {
        const stateTransitions = this.transitions[this.state];
        
        if (!stateTransitions) {
            return { success: false, error: 'No transitions for current state' };
        }

        const nextState = stateTransitions[event];

        if (!nextState) {
            return { 
                success: false, 
                error: `Invalid event '${event}' for state '${this.state}'`,
                validEvents: Object.keys(stateTransitions)
            };
        }

        this.state = nextState;
        this.history.push(nextState);

        return { 
            success: true, 
            previousState: this.history[this.history.length - 2],
            currentState: nextState 
        };
    }

    can(event) {
        const stateTransitions = this.transitions[this.state];
        return event in (stateTransitions || {});
    }

    getState() {
        return this.state;
    }

    getHistory() {
        return [...this.history];
    }
}

// Usage
const orderMachine = new StateMachine('created', {
    created: {
        confirm: 'confirmed',
        cancel: 'cancelled'
    },
    confirmed: {
        process: 'processing',
        cancel: 'cancelled'
    },
    processing: {
        complete: 'completed',
        fail: 'failed'
    },
    completed: {
        deliver: 'delivered',
        return: 'returned'
    },
    delivered: {
        complete: 'closed'
    },
    failed: {
        retry: 'confirmed'
    },
    cancelled: {},
    returned: {}
});

console.log(orderMachine.transition('confirm'));
console.log(orderMachine.transition('process'));
console.log(orderMachine.transition('complete'));
console.log(orderMachine.getHistory());
```

---

## Event Processing

### Event Bus

```javascript
// File: event-bus.js
// Description: Event processing system

class EventBus {
    constructor() {
        this.listeners = new Map();
    }

    on(event, handler) {
        if (!this.listeners.has(event)) {
            this.listeners.set(event, []);
        }
        this.listeners.get(event).push({
            handler,
            once: false
        });
    }

    once(event, handler) {
        if (!this.listeners.has(event)) {
            this.listeners.set(event, []);
        }
        this.listeners.set(event, [
            ...this.listeners.get(event),
            { handler, once: true }
        ]);
    }

    off(event, handlerToRemove) {
        if (!this.listeners.has(event)) return;

        this.listeners.set(
            event,
            this.listeners.get(event).filter(
                h => h.handler !== handlerToRemove
            )
        );
    }

    emit(event, ...args) {
        const handlers = this.listeners.get(event) || [];
        const results = [];

        for (const { handler, once } of handlers) {
            try {
                const result = handler(...args);
                results.push({ success: true, result });
                
                if (once) {
                    this.off(event, handler);
                }
            } catch (error) {
                results.push({ success: false, error: error.message });
            }
        }

        return results;
    }
}

// Usage
const bus = new EventBus();

const logHandler = (data) => console.log('Logged:', data);
const analyticsHandler = (data) => ({ tracked: true });

bus.on('user.created', logHandler);
bus.on('user.created', analyticsHandler);
bus.on('user.login', (user) => ({ userId: user.id }));

bus.emit('user.created', { name: 'John', email: 'john@example.com' });
bus.emit('user.login', { id: 1, name: 'John' });
```

---

## Error Handling

### Try-Catch Flow

```javascript
// File: error-handling.js
// Description: Comprehensive error handling

class ErrorHandler {
    constructor() {
        this.errors = [];
    }

    async execute(fn, context = 'operation') {
        try {
            const result = await fn();
            return { success: true, result };
        } catch (error) {
            return this.handleError(error, context);
        }
    }

    handleError(error, context) {
        const errorInfo = {
            message: error.message,
            stack: error.stack,
            context,
            timestamp: new Date().toISOString()
        };

        this.errors.push(errorInfo);

        return {
            success: false,
            error: errorInfo
        };
    }

    getErrors() {
        return [...this.errors];
    }

    clearErrors() {
        this.errors = [];
    }
}

// Usage
const handler = new ErrorHandler();

async function riskyOperation(shouldFail) {
    if (shouldFail) {
        throw new Error('Operation failed');
    }
    return { data: 'success' };
}

const result1 = await handler.execute(() => riskyOperation(false));
const result2 = await handler.execute(() => riskyOperation(true));

console.log(result1);
console.log(result2);
console.log(handler.getErrors());
```

---

## Best Practices

1. **Use early returns** to reduce nesting
2. **Keep conditions simple** and readable
3. **Handle all edge cases** explicitly
4. **Log important state changes**
5. **Use const for state machines**

---

## Key Takeaways

- Combine basic control flow constructs for complex logic
- Use guard clauses to validate inputs early
- Implement state machines for predictable behavior
- Process data in stages with clear error handling

---

## Related Files

- [01_IF_ELSE_CONDITIONAL_STATEMENTS](./01_IF_ELSE_CONDITIONAL_STATEMENTS.md)
- [02_LOOPS_MASTER](./02_LOOPS_MASTER.md)
- [03_LOGICAL_OPERATORS_MASTER](./03_LOGICAL_OPERATORS_MASTER.md)
- [04_JUMP_STATEMENTS_ADVANCED](./04_JUMP_STATEMENTS_ADVANCED.md)
- [07_DEBUGGING_CONTROL_FLOW](./07_DEBUGGING_CONTROL_FLOW.md)

---

*Last updated: 2026-04-03*