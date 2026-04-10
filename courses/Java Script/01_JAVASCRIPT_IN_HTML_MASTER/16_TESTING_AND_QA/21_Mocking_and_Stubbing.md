# 🎭 Mocking and Stubbing

## 📋 Overview

Mocking and stubbing replace dependencies with controlled implementations to isolate the code under test.

---

## 🎯 Types of Test Doubles

| Type | Purpose |
|------|---------|
| **Dummy** | Passed but never used |
| **Stub** | Returns predefined values |
| **Spy** | Records calls for verification |
| **Mock** | Pre-programmed expectations |

---

## 🎯 Jest Mocks

### Function Mocking

```javascript
// Mock a function
const myFunc = jest.fn();
myFunc('hello');
expect(myFunc).toHaveBeenCalledWith('hello');

// Mock return value
const myFunc = jest.fn().mockReturnValue('default');
expect(myFunc()).toBe('default');

// Mock implementation
const myFunc = jest.fn((x) => x * 2);
expect(myFunc(5)).toBe(10);
```

### Module Mocking

```javascript
// Mock a module
jest.mock('./api');
const api = require('./api');

// Configure mock
api.fetchUser.mockResolvedValue({ id: 1, name: 'John' });
```

---

## 🔗 Related Topics

- [02_Unit_Testing_Master_Class.md](./02_Unit_Testing_Master_Class.md)
- [01_Automated_Testing_Framework.md](./01_Automated_Testing_Framework.md)

---

**Next: [Performance Testing](./22_Performance_Testing.md)**