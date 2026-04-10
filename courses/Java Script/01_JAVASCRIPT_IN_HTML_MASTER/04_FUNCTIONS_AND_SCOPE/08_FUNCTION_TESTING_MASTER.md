# 📝 Function Testing Mastery

## 📋 Table of Contents

1. [Overview](#overview)
2. [Testing Fundamentals](#testing-fundamentals)
3. [Unit Testing Functions](#unit-testing-functions)
4. [Testing Edge Cases](#testing-edge-cases)
5. [Mocking Dependencies](#mocking-dependencies)
6. [Testing Strategies](#testing-strategies)
7. [Async Function Testing](#async-function-testing)
8. [Professional Use Cases](#professional-use-cases)
9. [Common Pitfalls](#common-pitfalls)
10. [Key Takeaways](#key-takeaways)

---

## Overview

Testing functions is the foundation of software quality assurance. Well-tested functions are more reliable, easier to refactor, and provide documentation of expected behavior. This comprehensive guide covers unit testing fundamentals, edge case identification, mocking strategies, and professional testing patterns using modern JavaScript testing frameworks.

Whether you're writing pure utility functions or complex async operations, understanding how to thoroughly test them is essential. This guide provides production-ready examples and covers both synchronous and asynchronous function testing, along with strategies for handling dependencies and side effects.

---

## Testing Fundamentals

### What Makes Good Tests

```javascript
// students/01_testBasics.js

// Good tests follow AAA pattern: Arrange, Act, Assert

// Example function to test
function validateEmail(email) {
    if (!email || typeof email !== 'string') {
        return { valid: false, error: 'Email is required' };
    }
    
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    
    if (!emailRegex.test(email)) {
        return { valid: false, error: 'Invalid email format' };
    }
    
    return { valid: true, error: null };
}

// Tests following AAA pattern
function testValidateEmail() {
    // Arrange
    const testCases = [
        { input: 'test@example.com', expected: { valid: true, error: null } },
        { input: 'invalid', expected: { valid: false, error: 'Invalid email format' } },
        { input: '', expected: { valid: false, error: 'Email is required' } },
        { input: null, expected: { valid: false, error: 'Email is required' } },
        { input: undefined, expected: { valid: false, error: 'Email is required' } }
    ];
    
    // Act & Assert
    testCases.forEach(({ input, expected }) => {
        const result = validateEmail(input);
        console.assert(
            JSON.stringify(result) === JSON.stringify(expected),
            `Failed for input: ${input}`
        );
    });
}

testValidateEmail();
```

### Test Structure

```javascript
// students/02_testStructure.js

// Organize tests by function and scenario
const TestRunner = {
    results: [],
    
    describe(name, fn) {
        console.log(`\n📦 ${name}`);
        fn();
    },
    
    test(description, fn) {
        try {
            fn();
            this.results.push({ description, passed: true });
            console.log(`  ✅ ${description}`);
        } catch (error) {
            this.results.push({ description, passed: false, error });
            console.log(`  ❌ ${description}`);
            console.log(`     Error: ${error.message}`);
        }
    },
    
    expect(actual) {
        return {
            toBe: (expected) => {
                if (actual !== expected) {
                    throw new Error(`Expected ${expected}, got ${actual}`);
                }
            },
            toEqual: (expected) => {
                if (JSON.stringify(actual) !== JSON.stringify(expected)) {
                    throw new Error(`Expected ${JSON.stringify(expected)}, got ${JSON.stringify(actual)}`);
                }
            },
            toBeTruthy: () => {
                if (!actual) {
                    throw new Error(`Expected truthy, got ${actual}`);
                }
            },
            toBeFalsy: () => {
                if (actual) {
                    throw new Error(`Expected falsy, got ${actual}`);
                }
            },
            toThrow: () => {
                let threw = false;
                try {
                    actual();
                } catch (e) {
                    threw = true;
                }
                if (!threw) {
                    throw new Error('Expected function to throw');
                }
            },
            toContain: (item) => {
                if (!actual.includes(item)) {
                    throw new Error(`Expected ${JSON.stringify(actual)} to contain ${item}`);
                }
            }
        };
    },
    
    summary() {
        const passed = this.results.filter(r => r.passed).length;
        const failed = this.results.filter(r => !r.passed).length;
        console.log(`\n📊 Summary: ${passed} passed, ${failed} failed`);
    }
};

const { describe, test, expect } = TestRunner;

// Usage
describe('validateEmail', () => {
    test('valid email returns valid: true', () => {
        expect(validateEmail('test@example.com').valid).toBe(true);
    });
    
    test('invalid email returns valid: false', () => {
        expect(validateEmail('invalid').valid).toBe(false);
    });
});

TestRunner.summary();
```

---

## Unit Testing Functions

### Testing Pure Functions

```javascript
// students/03_pureFunctions.js

// Pure function: same input always produces same output, no side effects
function calculateTotal(items, taxRate = 0.1) {
    const subtotal = items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const tax = subtotal * taxRate;
    return {
        subtotal: Math.round(subtotal * 100) / 100,
        tax: Math.round(tax * 100) / 100,
        total: Math.round((subtotal + tax) * 100) / 100
    };
}

// Test suite
describe('calculateTotal', () => {
    test('calculates correct total for single item', () => {
        const items = [{ price: 10, quantity: 2 }];
        const result = calculateTotal(items);
        
        expect(result.subtotal).toBe(20);
        expect(result.tax).toBe(2);
        expect(result.total).toBe(22);
    });
    
    test('calculates correct total for multiple items', () => {
        const items = [
            { price: 10, quantity: 2 },
            { price: 5, quantity: 3 }
        ];
        const result = calculateTotal(items);
        
        expect(result.subtotal).toBe(35);
        expect(result.tax).toBe(3.5);
        expect(result.total).toBe(38.5);
    });
    
    test('handles empty array', () => {
        const result = calculateTotal([]);
        
        expect(result.subtotal).toBe(0);
        expect(result.tax).toBe(0);
        expect(result.total).toBe(0);
    });
    
    test('uses custom tax rate', () => {
        const items = [{ price: 100, quantity: 1 }];
        const result = calculateTotal(items, 0.2);
        
        expect(result.tax).toBe(20);
        expect(result.total).toBe(120);
    });
    
    test('handles negative prices (credit)', () => {
        const items = [{ price: 100, quantity: 1 }, { price: -20, quantity: 1 }];
        const result = calculateTotal(items);
        
        expect(result.subtotal).toBe(80);
        expect(result.total).toBe(88);
    });
});
```

### Testing Functions with Objects

```javascript
// students/04_objectFunctions.js

function processUser(user, options = {}) {
    if (!user || typeof user !== 'object') {
        throw new Error('User is required');
    }
    
    const {
        sanitize = true,
        addDefaults = true,
        validation = true
    } = options;
    
    let processed = { ...user };
    
    if (addDefaults) {
        processed.role = processed.role || 'user';
        processed.active = processed.active !== false;
    }
    
    if (sanitize) {
        if (processed.name) {
            processed.name = processed.name.trim();
        }
        if (processed.email) {
            processed.email = processed.email.toLowerCase().trim();
        }
    }
    
    if (validation) {
        if (processed.email && !processed.email.includes('@')) {
            throw new Error('Invalid email');
        }
    }
    
    return processed;
}

describe('processUser', () => {
    test('sanitizes user data', () => {
        const user = { 
            name: '  John Doe  ', 
            email: 'JOHN@EXAMPLE.COM' 
        };
        
        const result = processUser(user);
        
        expect(result.name).toBe('John Doe');
        expect(result.email).toBe('john@example.com');
    });
    
    test('adds default values', () => {
        const user = { name: 'Alice' };
        
        const result = processUser(user);
        
        expect(result.role).toBe('user');
        expect(result.active).toBe(true);
    });
    
    test('throws for invalid input', () => {
        expect(() => processUser(null)).toThrow();
        expect(() => processUser('not an object')).toThrow();
    });
    
    test('respects options', () => {
        const user = { name: '  Bob  ', role: 'admin' };
        
        const result = processUser(user, { 
            sanitize: false, 
            addDefaults: false 
        });
        
        expect(result.name).toBe('  Bob  ');  // Not sanitized
        expect(result.role).toBe('admin');   // Not defaulted
    });
});
```

---

## Testing Edge Cases

### Identifying Edge Cases

```javascript
// students/05_edgeCases.js

// Function to test
function divide(a, b) {
    if (typeof a !== 'number' || typeof b !== 'number') {
        throw new Error('Both arguments must be numbers');
    }
    return a / b;
}

describe('divide edge cases', () => {
    test('normal division', () => {
        expect(divide(10, 2)).toBe(5);
    });
    
    test('division by zero returns Infinity', () => {
        expect(divide(10, 0)).toBe(Infinity);
        expect(divide(-10, 0)).toBe(-Infinity);
    });
    
    test('zero divided by number', () => {
        expect(divide(0, 5)).toBe(0);
    });
    
    test('negative numbers', () => {
        expect(divide(-10, 2)).toBe(-5);
        expect(divide(10, -2)).toBe(-5);
        expect(divide(-10, -2)).toBe(5);
    });
    
    test('decimal numbers', () => {
        expect(divide(0.1, 0.2)).toBeCloseTo(0.5);
    });
    
    test('throws for non-numbers', () => {
        expect(() => divide('10', 2)).toThrow();
        expect(() => divide(10, '2')).toThrow();
        expect(() => divide(null, 2)).toThrow();
        expect(() => divide(10, undefined)).toThrow();
    });
    
    test('NaN results', () => {
        expect(divide(0, 0)).toBe(NaN);
    });
    
    test('very large numbers', () => {
        expect(divide(Number.MAX_VALUE, 2)).toBeCloseTo(Number.MAX_VALUE / 2);
    });
    
    test('very small numbers', () => {
        const result = divide(Number.MIN_VALUE, 2);
        expect(result).toBeGreaterThan(0);
    });
});
```

### Comprehensive Edge Case Testing

```javascript
// students/06_comprehensiveEdges.js

function filterAndTransform(data, config = {}) {
    if (!Array.isArray(data)) {
        throw new Error('Data must be an array');
    }
    
    const {
        filterFn = () => true,
        transformFn = (x) => x,
        limit = Infinity,
        sortBy = null,
        sortDirection = 'asc'
    } = config;
    
    let result = data.filter(filterFn).map(transformFn);
    
    if (sortBy) {
        result = result.sort((a, b) => {
            const aVal = a[sortBy];
            const bVal = b[sortBy];
            
            if (aVal < bVal) return sortDirection === 'asc' ? -1 : 1;
            if (aVal > bVal) return sortDirection === 'asc' ? 1 : -1;
            return 0;
        });
    }
    
    return result.slice(0, limit);
}

describe('filterAndTransform', () => {
    describe('filter', () => {
        test('filters by condition', () => {
            const data = [1, 2, 3, 4, 5];
            const result = filterAndTransform(data, { 
                filterFn: n => n > 2 
            });
            
            expect(result).toEqual([3, 4, 5]);
        });
        
        test('empty result when no matches', () => {
            const data = [1, 2, 3];
            const result = filterAndTransform(data, { 
                filterFn: n => n > 100 
            });
            
            expect(result).toEqual([]);
        });
    });
    
    describe('transform', () => {
        test('transforms each element', () => {
            const data = [1, 2, 3];
            const result = filterAndTransform(data, { 
                transformFn: n => n * 2 
            });
            
            expect(result).toEqual([2, 4, 6]);
        });
        
        test('can change type', () => {
            const data = [1, 2, 3];
            const result = filterAndTransform(data, { 
                transformFn: n => n.toString() 
            });
            
            expect(result).toEqual(['1', '2', '3']);
        });
    });
    
    describe('limit', () => {
        test('limits results', () => {
            const data = [1, 2, 3, 4, 5];
            const result = filterAndTransform(data, { limit: 3 });
            
            expect(result.length).toBe(3);
        });
        
        test('limit larger than array', () => {
            const data = [1, 2, 3];
            const result = filterAndTransform(data, { limit: 10 });
            
            expect(result.length).toBe(3);
        });
        
        test('limit of zero', () => {
            const data = [1, 2, 3];
            const result = filterAndTransform(data, { limit: 0 });
            
            expect(result).toEqual([]);
        });
    });
    
    describe('sorting', () => {
        test('sorts ascending', () => {
            const data = [{ value: 3 }, { value: 1 }, { value: 2 }];
            const result = filterAndTransform(data, { sortBy: 'value' });
            
            expect(result[0].value).toBe(1);
            expect(result[2].value).toBe(3);
        });
        
        test('sorts descending', () => {
            const data = [{ value: 3 }, { value: 1 }, { value: 2 }];
            const result = filterAndTransform(data, { 
                sortBy: 'value', 
                sortDirection: 'desc' 
            });
            
            expect(result[0].value).toBe(3);
            expect(result[2].value).toBe(1);
        });
    });
    
    describe('errors', () => {
        test('throws for non-array data', () => {
            expect(() => filterAndTransform('not array')).toThrow();
            expect(() => filterAndTransform({})).toThrow();
            expect(() => filterAndTransform(null)).toThrow();
        });
    });
});
```

---

## Mocking Dependencies

### Function Mocking

```javascript
// students/07_mocking.js

// Mock function creation
function createMock(fn) {
    const mockFn = function(...args) {
        mockFn.calls.push(args);
        
        if (mockFn.mockImplementation) {
            return mockFn.mockImplementation(...args);
        }
        
        return undefined;
    };
    
    mockFn.calls = [];
    mockFn.mockImplementation = null;
    mockFn.mockReturnValue = (value) => {
        mockFn.mockImplementation = () => value;
        return mockFn;
    };
    
    return mockFn;
}

// Example: Mocking fetch
function fetchUserData(userId, fetchFn = fetch) {
    return fetchFn(`/api/users/${userId}`)
        .then(response => response.json());
}

describe('fetchUserData with mocks', () => {
    test('returns user data on success', async () => {
        const mockFetch = createMock();
        mockFetch.mockReturnValue(Promise.resolve({
            json: () => Promise.resolve({ id: 1, name: 'Alice' })
        }));
        
        const result = await fetchUserData(1, mockFetch);
        
        expect(result.name).toBe('Alice');
        expect(mockFetch.calls[0][0]).toBe('/api/users/1');
    });
    
    test('handles fetch error', async () => {
        const mockFetch = createMock();
        mockFetch.mockReturnValue(Promise.reject(new Error('Network error')));
        
        await expect(fetchUserData(1, mockFetch)).rejects.toThrow('Network error');
    });
});
```

### Spy Functions

```javascript
// students/08_spies.js

// Create spy that wraps existing function
function spy(fn) {
    const spyFn = function(...args) {
        spyFn.calls.push({ args, this: this });
        return fn.apply(this, args);
    };
    
    spyFn.calls = [];
    spyFn.wasCalled = () => spyFn.calls.length > 0;
    spyFn.getCall = (n) => spyFn.calls[n];
    spyFn.reset = () => { spyFn.calls = []; };
    
    return spyFn;
}

// Usage with array method
const originalFilter = Array.prototype.filter;
Array.prototype.filter = spy(Array.prototype.filter);

const numbers = [1, 2, 3, 4, 5];
const evens = numbers.filter(n => n % 2 === 0);

console.log('Was called:', Array.prototype.filter.wasCalled());
console.log('Call count:', Array.prototype.filter.calls.length);
console.log('First call args:', Array.prototype.filter.calls[0].args);

// Reset
Array.prototype.filter.reset();
```

### Module Mocking

```javascript
// students/09_moduleMocking.js

// File: logger.js (module to test)
function log(level, message) {
    console.log(`[${level}] ${message}`);
}

function info(message) { log('INFO', message); }
function error(message) { log('ERROR', message); }

// Test file
describe('logger', () => {
    let originalConsole;
    
    beforeEach(() => {
        originalConsole = { ...console };
        console.log = createMock();
    });
    
    afterEach(() => {
        console = originalConsole;
    });
    
    test('log formats message correctly', () => {
        log('INFO', 'Test message');
        
        expect(console.log.calls[0][0]).toBe('[INFO] Test message');
    });
    
    test('info calls log with INFO level', () => {
        info('Hello');
        
        expect(console.log.calls[0][0]).toBe('[INFO] Hello');
    });
    
    test('error calls log with ERROR level', () => {
        error('Something went wrong');
        
        expect(console.log.calls[0][0]).toBe('[ERROR] Something went wrong');
    });
});
```

---

## Testing Strategies

### Test Coverage Strategy

```javascript
// students/10_coverageStrategy.js

// Focus on: Input space coverage, behavior coverage, edge cases

function parseDate(dateInput) {
    if (!dateInput) return null;
    
    if (dateInput instanceof Date) {
        return dateInput;
    }
    
    if (typeof dateInput === 'number') {
        return new Date(dateInput);
    }
    
    if (typeof dateInput === 'string') {
        const parsed = Date.parse(dateInput);
        return isNaN(parsed) ? null : new Date(parsed);
    }
    
    return null;
}

// Coverage matrix
const coverageTests = [
    // Null/undefined
    { input: null, desc: 'null input' },
    { input: undefined, desc: 'undefined input' },
    
    // Date object
    { input: new Date('2024-01-01'), desc: 'Date object' },
    
    // Number (timestamp)
    { input: 1704067200000, desc: 'timestamp number' },
    { input: 0, desc: 'zero timestamp' },
    
    // String
    { input: '2024-01-01', desc: 'ISO date string' },
    { input: 'January 1, 2024', desc: 'long date string' },
    { input: 'invalid', desc: 'invalid string' },
    { input: '', desc: 'empty string' },
    
    // Other types
    { input: {}, desc: 'object' },
    { input: [], desc: 'array' },
    { input: true, desc: 'boolean' },
    { input: () => {}, desc: 'function' }
];

coverageTests.forEach(({ input, desc }) => {
    test(`handles ${desc}`, () => {
        const result = parseDate(input);
        
        if (input === null || input === undefined || 
            input === 'invalid' || input === '' || 
            typeof input === 'object') {
            expect(result).toBeNull();
        } else {
            expect(result).toBeInstanceOf(Date);
        }
    });
});
```

### Property-Based Testing

```javascript
// students/11_propertyTesting.js

// Property-based testing: test properties that should always hold

function sortByKey(arr, key) {
    return [...arr].sort((a, b) => {
        if (a[key] < b[key]) return -1;
        if (a[key] > b[key]) return 1;
        return 0;
    });
}

describe('sortByKey properties', () => {
    test('output length equals input length', () => {
        const input = [{ a: 3 }, { a: 1 }, { a: 2 }];
        const result = sortByKey(input, 'a');
        
        expect(result.length).toBe(input.length);
    });
    
    test('does not mutate original array', () => {
        const original = [{ a: 3 }, { a: 1 }];
        const originalCopy = JSON.stringify(original);
        
        sortByKey(original, 'a');
        
        expect(JSON.stringify(original)).toBe(originalCopy);
    });
    
    test('sorting is stable (elements with same key maintain relative order)', () => {
        const input = [
            { key: 'a', id: 1 },
            { key: 'b', id: 2 },
            { key: 'a', id: 3 }
        ];
        
        const result = sortByKey(input, 'key');
        
        expect(result[0].id).toBe(1);  // First 'a' stays first
        expect(result[1].id).toBe(3); // Second 'a' stays second
    });
    
    test('handles empty array', () => {
        expect(sortByKey([], 'key')).toEqual([]);
    });
    
    test('handles single element', () => {
        const result = sortByKey([{ a: 1 }], 'a');
        expect(result[0].a).toBe(1);
    });
});
```

---

## Async Function Testing

### Testing Promises

```javascript
// students/12_asyncTesting.js

// Async function to test
async function fetchWithRetry(url, maxRetries = 3) {
    let lastError;
    
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
        try {
            const response = await fetch(url);
            
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            
            return await response.json();
        } catch (error) {
            lastError = error;
            
            if (attempt < maxRetries) {
                await new Promise(r => setTimeout(r, 100 * attempt));
            }
        }
    }
    
    throw lastError;
}

describe('fetchWithRetry', () => {
    test('succeeds on first attempt', async () => {
        const mockFetch = createMock();
        mockFetch.mockReturnValue(Promise.resolve({
            ok: true,
            json: () => Promise.resolve({ data: 'success' })
        }));
        
        const result = await fetchWithRetry('/api/test', 3);
        
        expect(result.data).toBe('success');
        expect(mockFetch.calls.length).toBe(1);
    });
    
    test('retries on failure', async () => {
        let attempts = 0;
        const mockFetch = createMock();
        
        mockFetch.mockImplementation = () => {
            attempts++;
            if (attempts < 3) {
                return Promise.reject(new Error('Network error'));
            }
            return Promise.resolve({
                ok: true,
                json: () => Promise.resolve({ data: 'success' })
            });
        };
        
        const result = await fetchWithRetry('/api/test', 3);
        
        expect(result.data).toBe('success');
        expect(attempts).toBe(3);
    });
    
    test('fails after max retries', async () => {
        const mockFetch = createMock();
        mockFetch.mockReturnValue(Promise.reject(new Error('Network error')));
        
        await expect(fetchWithRetry('/api/test', 3))
            .rejects.toThrow('Network error');
        
        expect(mockFetch.calls.length).toBe(3);
    });
});
```

### Testing Callback-Based Functions

```javascript
// students/13_callbackTesting.js

// Callback-based function
function readFile(path, callback) {
    setTimeout(() => {
        if (path.includes('error')) {
            callback(new Error('File not found'), null);
            return;
        }
        
        callback(null, { content: 'file content', path });
    }, 10);
}

// Convert to promise for easier testing
function readFilePromise(path) {
    return new Promise((resolve, reject) => {
        readFile(path, (error, data) => {
            if (error) reject(error);
            else resolve(data);
        });
    });
}

describe('readFilePromise', () => {
    test('resolves with file data', async () => {
        const result = await readFilePromise('/data/file.txt');
        
        expect(result.content).toBe('file content');
        expect(result.path).toBe('/data/file.txt');
    });
    
    test('rejects on error', async () => {
        await expect(readFilePromise('/error/file.txt'))
            .rejects.toThrow('File not found');
    });
});
```

---

## Professional Use Cases

### 1. Integration Testing with Mocks

```javascript
// students/14_integrationTesting.js

// Service under test
class UserService {
    constructor(apiClient, logger) {
        this.apiClient = apiClient;
        this.logger = logger;
    }
    
    async createUser(userData) {
        this.logger.info('Creating user', userData.email);
        
        const validation = this.validateUser(userData);
        if (!validation.valid) {
            throw new Error(validation.error);
        }
        
        const result = await this.apiClient.post('/users', userData);
        
        this.logger.info('User created', result.id);
        return result;
    }
    
    validateUser(userData) {
        if (!userData.email) {
            return { valid: false, error: 'Email is required' };
        }
        if (!userData.email.includes('@')) {
            return { valid: false, error: 'Invalid email' };
        }
        if (!userData.name) {
            return { valid: false, error: 'Name is required' };
        }
        return { valid: true };
    }
    
    async getUser(id) {
        return this.apiClient.get(`/users/${id}`);
    }
}

// Test setup
function createTestService() {
    const mockClient = {
        get: createMock().mockReturnValue(Promise.resolve({ id: 1, name: 'Test' })),
        post: createMock().mockReturnValue(Promise.resolve({ id: 1 }))
    };
    
    const mockLogger = {
        info: createMock(),
        error: createMock()
    };
    
    return new UserService(mockClient, mockLogger);
}

describe('UserService', () => {
    test('createUser validates and calls API', async () => {
        const service = createTestService();
        
        await service.createUser({ 
            email: 'test@example.com', 
            name: 'Test User' 
        });
        
        expect(service.apiClient.post.calls.length).toBe(1);
        expect(service.logger.info.calls.length).toBe(2); // Start and success
    });
    
    test('createUser throws on invalid data', async () => {
        const service = createTestService();
        
        await expect(service.createUser({ 
            email: 'invalid' 
        })).rejects.toThrow('Invalid email');
    });
});
```

### 2. Testing Error Handling

```javascript
// students/15_errorHandling.js

function executeWithTimeout(fn, timeout = 5000) {
    return new Promise((resolve, reject) => {
        const timeoutId = setTimeout(() => {
            reject(new Error('Operation timed out'));
        }, timeout);
        
        Promise.resolve()
            .then(fn)
            .then(result => {
                clearTimeout(timeoutId);
                resolve(result);
            })
            .catch(error => {
                clearTimeout(timeoutId);
                reject(error);
            });
    });
}

describe('executeWithTimeout', () => {
    test('resolves successful operation', async () => {
        const result = await executeWithTimeout(
            () => Promise.resolve('success'),
            1000
        );
        
        expect(result).toBe('success');
    });
    
    test('rejects on error', async () => {
        await expect(executeWithTimeout(
            () => Promise.reject(new Error('Failed')),
            1000
        )).rejects.toThrow('Failed');
    });
    
    test('rejects on timeout', async () => {
        await expect(executeWithTimeout(
            () => new Promise(() => {}),  // Never resolves
            50
        )).rejects.toThrow('Operation timed out');
    });
    
    test('allows synchronous functions', async () => {
        const result = await executeWithTimeout(
            () => 'sync result',
            1000
        );
        
        expect(result).toBe('sync result');
    });
});
```

### 3. Testing Factory Functions

```javascript
// students/16_factoryTesting.js

function createValidator(rules) {
    return {
        validate(value) {
            const errors = [];
            
            for (const [rule, test, message] of rules) {
                if (!test(value)) {
                    errors.push({ rule, message });
                }
            }
            
            return {
                valid: errors.length === 0,
                errors
            };
        }
    };
}

describe('createValidator', () => {
    test('creates validator with rules', () => {
        const validator = createValidator([
            ['required', v => v && v.length > 0, 'Field is required'],
            ['minLength', v => !v || v.length >= 3, 'Minimum 3 characters']
        ]);
        
        const result = validator.validate('');
        
        expect(result.valid).toBe(false);
        expect(result.errors.length).toBe(2);
    });
    
    test('passes valid input', () => {
        const validator = createValidator([
            ['required', v => v && v.length > 0, 'Required']
        ]);
        
        const result = validator.validate('hello');
        
        expect(result.valid).toBe(true);
        expect(result.errors.length).toBe(0);
    });
    
    test('handles empty rules', () => {
        const validator = createValidator([]);
        
        const result = validator.validate('anything');
        
        expect(result.valid).toBe(true);
    });
    
    test('each rule is checked independently', () => {
        const validator = createValidator([
            ['rule1', () => false, 'Rule 1 failed'],
            ['rule2', () => true, 'Rule 2 failed']
        ]);
        
        const result = validator.validate('test');
        
        expect(result.valid).toBe(false);
        expect(result.errors.find(e => e.rule === 'rule1')).toBeDefined();
        expect(result.errors.find(e => e.rule === 'rule2')).toBeUndefined();
    });
});
```

---

## Common Pitfalls

### 1. Testing Implementation, Not Behavior

```javascript
// students/17_pitfallImplementation.js

// ❌ WRONG: Testing how it works, not what it does
function processDataBad(data) {
    const temp = [];
    for (const item of data) {
        temp.push(item * 2);
    }
    return temp;
}

// Tests break when implementation changes even if result is same!
test('uses for loop', () => {
    // This is implementation testing - BAD!
});

// ✅ CORRECT: Test the outcome
test('doubles each number', () => {
    const result = processDataBad([1, 2, 3]);
    expect(result).toEqual([2, 4, 6]);
});

test('handles empty array', () => {
    const result = processDataBad([]);
    expect(result).toEqual([]);
});
```

### 2. Not Testing Edge Cases

```javascript
// students/18_pitfallEdges.js

// ❌ WRONG: Only happy path tests
function addItemBad(items, item) {
    return [...items, item];
}

test('adds item', () => {
    expect(addItemBad([1, 2], 3)).toEqual([1, 2, 3]);
});
// Missing: empty array, undefined items, etc.

// ✅ CORRECT: Comprehensive edge cases
function addItem(items, item) {
    if (!Array.isArray(items)) {
        throw new Error('Items must be an array');
    }
    
    return [...items, item];
}

describe('addItem', () => {
    test('adds item to array', () => {
        expect(addItem([1, 2], 3)).toEqual([1, 2, 3]);
    });
    
    test('handles empty array', () => {
        expect(addItem([], 1)).toEqual([1]);
    });
    
    test('throws for non-array', () => {
        expect(() => addItem(null, 1)).toThrow();
        expect(() => addItem('not array', 1)).toThrow();
    });
    
    test('does not mutate original', () => {
        const original = [1, 2];
        addItem(original, 3);
        expect(original).toEqual([1, 2]);
    });
});
```

### 3. Forgetting to Reset Mocks

```javascript
// students/19_pitfallReset.js

// ❌ WRONG: Mock state persists between tests
let callCount = 0;
const mockFn = () => { callCount++; return 'result'; };

test('first call', () => {
    mockFn();
    expect(callCount).toBe(1);
});

test('second call', () => {
    mockFn();
    expect(callCount).toBe(1);  // FAIL! Will be 2!
});

// ✅ CORRECT: Reset in beforeEach
describe('mock functions', () => {
    let mockFn;
    let callCount;
    
    beforeEach(() => {
        callCount = 0;
        mockFn = () => {
            callCount++;
            return 'result';
        };
    });
    
    test('first call', () => {
        mockFn();
        expect(callCount).toBe(1);
    });
    
    test('second call', () => {
        mockFn();
        expect(callCount).toBe(1);  // PASS - reset each test
    });
});
```

---

## Key Takeaways

1. **AAA Pattern**: Arrange, Act, Assert - organize tests clearly.

2. **Test Behavior, Not Implementation**: Focus on what the function does, not how.

3. **Edge Cases**: Test null, undefined, empty values, edge boundaries, and error conditions.

4. **Mocks**: Use mocks for external dependencies (API, database, external services).

5. **Async Testing**: Use async/await with proper error handling and timeout tests.

6. **Reset State**: Always reset mocks and state between tests to prevent pollution.

7. **Property Testing**: Test invariants that should always hold, not just specific inputs.

8. **Coverage**: Aim for meaningful coverage - branch coverage, edge cases, error paths.

---

## Related Files

- [01_FUNCTION_DECLARATIONS_EXPRESSIONS.md](./01_FUNCTION_DECLARATIONS_EXPRESSIONS.md) - Pure functions
- [06_HIGHER_ORDER_FUNCTIONS.md](./06_HIGHER_ORDER_FUNCTIONS.md) - Testing callbacks
- [07_FUNCTION_MEMORIZATION_AND_OPTIMIZATION.md](./07_FUNCTION_MEMORIZATION_AND_OPTIMIZATION.md) - Testing memoized functions