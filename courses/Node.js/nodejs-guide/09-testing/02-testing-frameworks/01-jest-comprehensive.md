# Jest Framework Comprehensive Guide

## What You'll Learn

- Jest setup and configuration
- Jest matchers and assertions
- Jest mocking capabilities
- Jest coverage and reporting
- Jest performance optimization

## Jest Setup

```bash
npm install --save-dev jest @types/jest
```

```javascript
// jest.config.js
export default {
    testEnvironment: 'node',
    roots: ['<rootDir>/src', '<rootDir>/test'],
    testMatch: ['**/*.test.js', '**/*.spec.js'],
    collectCoverageFrom: [
        'src/**/*.js',
        '!src/**/*.config.js',
        '!src/**/index.js',
    ],
    coverageDirectory: 'coverage',
    coverageThresholds: {
        global: {
            branches: 80,
            functions: 80,
            lines: 80,
            statements: 80,
        },
    },
    setupFilesAfterSetup: ['./test/setup.js'],
    testTimeout: 10000,
    verbose: true,
};
```

```json
// package.json scripts
{
    "scripts": {
        "test": "jest",
        "test:watch": "jest --watch",
        "test:coverage": "jest --coverage",
        "test:ci": "jest --ci --coverage --reporters=default --reporters=jest-junit"
    }
}
```

## Jest Matchers

```javascript
describe('Jest Matchers', () => {
    // Equality
    it('equality matchers', () => {
        expect(1 + 1).toBe(2);              // Strict equality
        expect({ a: 1 }).toEqual({ a: 1 }); // Deep equality
        expect(1 + 1).not.toBe(3);          // Negation
    });

    // Truthiness
    it('truthiness matchers', () => {
        expect(true).toBeTruthy();
        expect(false).toBeFalsy();
        expect(null).toBeNull();
        expect(undefined).toBeUndefined();
        expect('hello').toBeDefined();
    });

    // Numbers
    it('number matchers', () => {
        expect(0.1 + 0.2).toBeCloseTo(0.3);
        expect(5).toBeGreaterThan(3);
        expect(5).toBeGreaterThanOrEqual(5);
        expect(5).toBeLessThan(10);
        expect(5).toBeLessThanOrEqual(5);
    });

    // Strings
    it('string matchers', () => {
        expect('Hello World').toMatch(/World/);
        expect('Hello World').toMatch('World');
        expect('Hello World').toContain('World');
    });

    // Arrays
    it('array matchers', () => {
        expect([1, 2, 3]).toContain(2);
        expect([1, 2, 3]).toContainEqual({ a: 1 });
        expect([1, 2, 3]).toHaveLength(3);
    });

    // Objects
    it('object matchers', () => {
        expect({ a: 1, b: 2 }).toHaveProperty('a');
        expect({ a: 1, b: 2 }).toHaveProperty('a', 1);
        expect({ a: 1, b: 2 }).toMatchObject({ a: 1 });
        expect({ a: 1, b: 2, c: 3 }).toEqual(expect.objectContaining({ a: 1 }));
    });

    // Errors
    it('error matchers', () => {
        expect(() => { throw new Error('oops'); }).toThrow();
        expect(() => { throw new Error('oops'); }).toThrow('oops');
        expect(() => { throw new Error('oops'); }).toThrow(Error);
    });

    // Promises
    it('promise matchers', async () => {
        await expect(Promise.resolve('hello')).resolves.toBe('hello');
        await expect(Promise.reject(new Error('fail'))).rejects.toThrow('fail');
    });

    // Mock
    it('mock matchers', () => {
        const fn = jest.fn();
        fn('arg1', 'arg2');

        expect(fn).toHaveBeenCalled();
        expect(fn).toHaveBeenCalledTimes(1);
        expect(fn).toHaveBeenCalledWith('arg1', 'arg2');
        expect(fn).toHaveReturnedWith(undefined);
    });
});
```

## Jest Mocking

```javascript
// Mock entire module
jest.mock('./email-service', () => ({
    sendEmail: jest.fn().mockResolvedValue({ success: true }),
}));

// Mock with implementation
jest.mock('./database', () => ({
    query: jest.fn(),
    connect: jest.fn().mockResolvedValue(true),
}));

// Mock specific method
import { UserService } from './user-service';
jest.spyOn(UserService.prototype, 'findById').mockResolvedValue({
    id: 1,
    name: 'Test User',
});

// Mock timers
jest.useFakeTimers();
setTimeout(() => console.log('tick'), 1000);
jest.advanceTimersByTime(1000);

// Mock date
const mockDate = new Date('2024-01-01');
jest.spyOn(global, 'Date').mockImplementation(() => mockDate);

// Mock fetch
global.fetch = jest.fn(() =>
    Promise.resolve({
        ok: true,
        json: () => Promise.resolve({ data: 'mocked' }),
    })
);

// Reset mocks between tests
beforeEach(() => {
    jest.clearAllMocks();    // Clear call history
    jest.resetAllMocks();    // Clear + reset implementations
    jest.restoreAllMocks();  // Restore original implementations
});
```

## Test Coverage

```javascript
// Branch coverage example
function getDiscount(user) {
    if (!user) return 0;                    // Branch 1
    if (user.role === 'admin') return 0.5;  // Branch 2
    if (user.loyalty > 10) return 0.2;      // Branch 3
    return 0.1;                              // Branch 4
}

// Tests should cover all branches
describe('getDiscount', () => {
    it('returns 0 for null user', () => {
        expect(getDiscount(null)).toBe(0);
    });

    it('returns 0.5 for admin', () => {
        expect(getDiscount({ role: 'admin' })).toBe(0.5);
    });

    it('returns 0.2 for loyal user', () => {
        expect(getDiscount({ role: 'user', loyalty: 15 })).toBe(0.2);
    });

    it('returns 0.1 for regular user', () => {
        expect(getDiscount({ role: 'user', loyalty: 5 })).toBe(0.1);
    });
});
```

## Best Practices

- [ ] Use `describe` to group related tests
- [ ] Use `beforeEach` for test setup
- [ ] Mock external dependencies
- [ ] Set coverage thresholds
- [ ] Use `--watch` during development
- [ ] Run `--coverage` in CI

## Cross-References

- See [Node Test Runner](../node-test-runner/01-test-basics.md) for built-in runner
- See [Unit Testing](../03-unit-testing/01-functions-classes.md) for patterns
- See [CI/CD](../10-testing-automation/01-ci-cd-integration.md) for automation

## Next Steps

Continue to [Unit Testing](../03-unit-testing/01-functions-classes.md).
