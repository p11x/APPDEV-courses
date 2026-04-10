# 🧪 Automated Testing Framework

## 📋 Overview

This guide covers the fundamentals of automated testing in JavaScript, including unit testing, integration testing, and end-to-end testing strategies.

---

## 🎯 Why Test?

| Benefit | Description |
|---------|-------------|
| **Reliability** | Catch bugs before production |
| **Refactoring Safety** | Change code with confidence |
| **Documentation** | Tests serve as executable docs |
| **Regression Prevention** | Prevent old bugs from reappearing |

---

## 🏗️ Testing Pyramid

```
        /\
       /E2E\          ← Few, slow, expensive
      /----\
     /Integ\          ← Medium quantity
    /------\
   / Unit  \          ← Many, fast, cheap
  /--------\
```

---

## 🎯 Unit Testing Basics

### First Test

```javascript
// sum.js
function sum(a, b) {
    return a + b;
}

// sum.test.js
function testSum() {
    const result = sum(2, 3);
    if (result !== 5) {
        throw new Error(`Expected 5, got ${result}`);
    }
    console.log('✓ sum(2, 3) = 5');
}

testSum();
```

### Using Jest

```javascript
// sum.js
function sum(a, b) {
    return a + b;
}

module.exports = sum;

// sum.test.js
const sum = require('./sum');

describe('sum', () => {
    test('adds two positive numbers', () => {
        expect(sum(2, 3)).toBe(5);
    });
    
    test('adds negative numbers', () => {
        expect(sum(-1, -1)).toBe(-2);
    });
    
    test('adds zero', () => {
        expect(sum(5, 0)).toBe(5);
    });
});
```

---

## 🎯 Test Structure

### AAA Pattern

```javascript
describe('Calculator', () => {
    test('should add two numbers', () => {
        // Arrange
        const calculator = new Calculator();
        
        // Act
        const result = calculator.add(2, 3);
        
        // Assert
        expect(result).toBe(5);
    });
});
```

### Common Matchers

```javascript
// Equality
expect(value).toBe(5);
expect(value).toEqual({ a: 1 });

// Truthiness
expect(value).toBeTruthy();
expect(value).toBeFalsy();
expect(value).toBeNull();
expect(value).toBeUndefined();

// Numbers
expect(value).toBeGreaterThan(5);
expect(value).toBeLessThan(10);
expect(value).toBeCloseTo(3.14, 2);

// Strings
expect(text).toMatch(/regex/);
expect(text).toContain('substring');

// Arrays
expect(array).toContain(5);
expect(array).toHaveLength(3);

// Objects
expect(obj).toHaveProperty('name');
expect(obj).toMatchObject({ name: 'John' });
```

---

## 🎯 Testing Async Code

```javascript
// async function
async function fetchUser(id) {
    const response = await fetch(`/api/users/${id}`);
    return response.json();
}

// Test with async/await
test('fetches user correctly', async () => {
    const user = await fetchUser(1);
    expect(user).toHaveProperty('id');
});

// Test with promises
test('fetches user correctly', () => {
    return fetchUser(1).then(user => {
        expect(user).toHaveProperty('id');
    });
});

// Testing errors
test('throws error for invalid id', async () => {
    await expect(fetchUser('invalid')).rejects.toThrow();
});
```

---

## 🎯 Mocking

### Function Mocking

```javascript
// Mock a function
const mockFn = jest.fn();
mockFn.mockReturnValue(42);
expect(mockFn()).toBe(42);

// Mock implementation
const mockFn = jest.fn((x) => x * 2);
expect(mockFn(3)).toBe(6);

// Mock module
jest.mock('./api');
const { fetchUser } = require('./api');
fetchUser.mockResolvedValue({ id: 1, name: 'John' });
```

### Timer Mocking

```javascript
// Mock setTimeout
jest.useFakeTimers();

test('delays execution', () => {
    const callback = jest.fn();
    
    setTimeout(callback, 1000);
    
    jest.advanceTimersByTime(1000);
    
    expect(callback).toHaveBeenCalled();
});
```

---

## 🎯 Test Coverage

```javascript
// Run with coverage
// npx jest --coverage

// Common coverage metrics
// - Line coverage: % of lines executed
// - Branch coverage: % of branches executed
// - Function coverage: % of functions called

// Test specific files
// npx jest --coverage --collectCoverageFrom=src/**/*.js
```

---

## 🔗 Related Topics

- [02_Promises_Complete_Guide.md](../08_ASYNC_JAVASCRIPT/02_Promises_Complete_Guide.md)
- [03_Async_Await_Master_Class.md](../08_ASYNC_JAVASCRIPT/03_Async_Await_Master_Class.md)

---

**Next: [Unit Testing Master Class](./02_Unit_Testing_Master_Class.md)**