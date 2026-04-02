# Testing Tools, Utilities, and Test Data Management

## What You'll Learn

- Test data factories and builders
- Test utility functions
- Mock server implementations
- Test data generation
- Test debugging utilities

## Test Data Factories

```javascript
// test/factories/user.factory.js
import { faker } from '@faker-js/faker';
import bcrypt from 'bcrypt';

export class UserFactory {
    static defaults = {
        passwordHash: null, // Generated on create
    };

    static async create(overrides = {}) {
        const password = overrides.password || 'TestPass123!';
        const passwordHash = await bcrypt.hash(password, 4);

        return {
            id: faker.string.uuid(),
            name: faker.person.fullName(),
            email: faker.internet.email().toLowerCase(),
            passwordHash,
            role: 'user',
            active: true,
            createdAt: new Date(),
            ...this.defaults,
            ...overrides,
        };
    }

    static async createMany(count, overrides = {}) {
        return Promise.all(
            Array.from({ length: count }, (_, i) =>
                this.create({ email: `user${i}@test.com`, ...overrides })
            )
        );
    }

    static async createAdmin(overrides = {}) {
        return this.create({ role: 'admin', ...overrides });
    }

    static async createInDb(pool, overrides = {}) {
        const user = await this.create(overrides);
        const { rows } = await pool.query(
            'INSERT INTO users (name, email, password_hash, role) VALUES ($1, $2, $3, $4) RETURNING *',
            [user.name, user.email, user.passwordHash, user.role]
        );
        return rows[0];
    }
}

// test/factories/product.factory.js
export class ProductFactory {
    static create(overrides = {}) {
        return {
            id: faker.string.uuid(),
            name: faker.commerce.productName(),
            description: faker.commerce.productDescription(),
            price: parseFloat(faker.commerce.price()),
            category: faker.commerce.department(),
            inStock: true,
            createdAt: new Date(),
            ...overrides,
        };
    }

    static createMany(count, overrides = {}) {
        return Array.from({ length: count }, (_, i) =>
            this.create({ name: `Product ${i}`, ...overrides })
        );
    }
}

// test/factories/order.factory.js
export class OrderFactory {
    static async create(pool, overrides = {}) {
        const user = overrides.user || await UserFactory.createInDb(pool);
        const items = overrides.items || [
            { productId: faker.string.uuid(), quantity: 1, price: 29.99 },
        ];

        return {
            id: faker.string.uuid(),
            userId: user.id,
            items,
            total: items.reduce((sum, i) => sum + i.price * i.quantity, 0),
            status: 'pending',
            createdAt: new Date(),
            ...overrides,
        };
    }
}
```

## Test Utility Functions

```javascript
// test/helpers/test-utils.js
import { Readable, Writable } from 'node:stream';
import jwt from 'jsonwebtoken';

// Auth helper
export function generateAuthToken(user = {}) {
    return jwt.sign(
        { sub: user.id || '1', role: user.role || 'user' },
        process.env.JWT_SECRET || 'test-secret',
        { expiresIn: '1h' }
    );
}

// Stream helpers
export function collectStream(stream) {
    return new Promise((resolve, reject) => {
        const chunks = [];
        stream.on('data', (chunk) => chunks.push(chunk));
        stream.on('end', () => resolve(chunks));
        stream.on('error', reject);
    });
}

export function arrayToReadable(arr) {
    let index = 0;
    return new Readable({
        objectMode: true,
        read() { this.push(index < arr.length ? arr[index++] : null); },
    });
}

export function createWritable(collector = []) {
    return new Writable({
        objectMode: true,
        write(chunk, encoding, callback) {
            collector.push(chunk);
            callback();
        },
    });
}

// Date helpers
export function freezeTime(date) {
    const now = new Date(date);
    const originalDate = Date;
    global.Date = class extends originalDate {
        constructor() { return now; }
        static now() { return now.getTime(); }
    };
    return () => { global.Date = originalDate; };
}

// Wait helper
export function wait(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

// Retry helper
export async function retry(fn, { attempts = 3, delay = 100 } = {}) {
    for (let i = 0; i < attempts; i++) {
        try {
            return await fn();
        } catch (err) {
            if (i === attempts - 1) throw err;
            await wait(delay * (i + 1));
        }
    }
}

// Assertion helpers
export function assertEventually(fn, { timeout = 5000, interval = 100 } = {}) {
    return new Promise((resolve, reject) => {
        const startTime = Date.now();
        const check = async () => {
            try {
                await fn();
                resolve();
            } catch (err) {
                if (Date.now() - startTime > timeout) {
                    reject(new Error(`Assertion failed after ${timeout}ms: ${err.message}`));
                } else {
                    setTimeout(check, interval);
                }
            }
        };
        check();
    });
}
```

## Mock Server

```javascript
// test/helpers/mock-server.js
import express from 'express';

export class MockServer {
    constructor() {
        this.app = express();
        this.app.use(express.json());
        this.routes = new Map();
        this.calls = [];
    }

    mock(method, path, response) {
        const key = `${method.toUpperCase()} ${path}`;
        this.routes.set(key, response);

        this.app[method.toLowerCase()]('*', (req, res) => {
            this.calls.push({ method: req.method, path: req.path, body: req.body });

            const mockResponse = this.routes.get(`${req.method} ${req.path}`);
            if (mockResponse) {
                if (mockResponse.status) res.status(mockResponse.status);
                if (mockResponse.headers) {
                    Object.entries(mockResponse.headers).forEach(([k, v]) => res.setHeader(k, v));
                }
                if (mockResponse.delay) {
                    setTimeout(() => res.json(mockResponse.body), mockResponse.delay);
                } else {
                    res.json(mockResponse.body);
                }
            } else {
                res.status(404).json({ error: 'Not mocked' });
            }
        });

        return this;
    }

    async start(port = 0) {
        return new Promise((resolve) => {
            this.server = this.app.listen(port, () => {
                this.port = this.server.address().port;
                resolve(this.port);
            });
        });
    }

    async stop() {
        return new Promise((resolve) => {
            this.server?.close(resolve);
        });
    }

    getCalls(path) {
        return this.calls.filter(c => c.path === path);
    }

    reset() {
        this.calls = [];
    }

    get url() {
        return `http://localhost:${this.port}`;
    }
}

// Usage
const mockApi = new MockServer();
mockApi
    .mock('GET', '/external-api/users', { body: [{ id: 1, name: 'Mock User' }] })
    .mock('POST', '/external-api/notify', { status: 200, body: { sent: true } });

const port = await mockApi.start();
process.env.EXTERNAL_API_URL = `http://localhost:${port}`;

// Run tests...
await mockApi.stop();
```

## Test Debugging Utilities

```javascript
// test/helpers/debug.js
export function debugTest(label, value) {
    if (process.env.DEBUG_TESTS) {
        console.log(`[DEBUG] ${label}:`, JSON.stringify(value, null, 2));
    }
}

export function logTestDuration(label) {
    const start = performance.now();
    return {
        end() {
            const elapsed = performance.now() - start;
            if (process.env.DEBUG_TESTS) {
                console.log(`[TIMING] ${label}: ${elapsed.toFixed(1)}ms`);
            }
            return elapsed;
        },
    };
}

export function captureConsole() {
    const logs = [];
    const original = { ...console };

    console.log = (...args) => logs.push({ level: 'log', args });
    console.error = (...args) => logs.push({ level: 'error', args });
    console.warn = (...args) => logs.push({ level: 'warn', args });

    return {
        logs,
        restore() {
            Object.assign(console, original);
        },
    };
}
```

## Best Practices Checklist

- [ ] Use factories for consistent test data
- [ ] Use faker for realistic random data
- [ ] Create reusable test utilities
- [ ] Use mock servers for external dependencies
- [ ] Capture console output in tests
- [ ] Measure test duration for slow tests
- [ ] Share utilities across test suites

## Cross-References

- See [Testing Fundamentals](../01-testing-fundamentals/01-testing-pyramid-architecture.md) for patterns
- See [Database Testing](../07-database-testing/01-unit-testing.md) for DB factories
- See [CI/CD](../10-testing-automation/01-ci-cd-integration.md) for automation
