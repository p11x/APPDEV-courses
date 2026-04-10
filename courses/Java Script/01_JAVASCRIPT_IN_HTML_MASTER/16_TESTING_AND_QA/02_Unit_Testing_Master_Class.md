# 🔬 Unit Testing Master Class

## 📋 Overview

Deep dive into unit testing patterns, test organization, and best practices for writing maintainable test suites.

---

## 🎯 Testing Functions

### Pure Functions

```javascript
// Pure function - easy to test
function capitalize(str) {
    if (!str) return '';
    return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
}

// Tests
test('capitalizes first letter', () => {
    expect(capitalize('hello')).toBe('Hello');
});

test('handles empty string', () => {
    expect(capitalize('')).toBe('');
});

test('handles single character', () => {
    expect(capitalize('a')).toBe('A');
});

test('preserves non-first letters lowercase', () => {
    expect(capitalize('HELLO')).toBe('Hello');
});
```

### Testing Edge Cases

```javascript
function divide(a, b) {
    if (b === 0) {
        throw new Error('Cannot divide by zero');
    }
    return a / b;
}

describe('divide', () => {
    test('divides positive numbers', () => {
        expect(divide(10, 2)).toBe(5);
    });
    
    test('divides negative numbers', () => {
        expect(divide(-10, 2)).toBe(-5);
    });
    
    test('throws on division by zero', () => {
        expect(() => divide(10, 0)).toThrow('Cannot divide by zero');
    });
    
    test('returns decimal for non-even division', () => {
        expect(divide(7, 2)).toBe(3.5);
    });
});
```

---

## 🎯 Testing Classes

```javascript
class Calculator {
    constructor() {
        this.history = [];
    }
    
    add(a, b) {
        const result = a + b;
        this.history.push({ operation: 'add', result });
        return result;
    }
    
    getHistory() {
        return [...this.history];
    }
    
    clearHistory() {
        this.history = [];
    }
}

describe('Calculator', () => {
    let calculator;
    
    beforeEach(() => {
        calculator = new Calculator();
    });
    
    test('adds two numbers', () => {
        expect(calculator.add(2, 3)).toBe(5);
    });
    
    test('records history', () => {
        calculator.add(1, 2);
        expect(calculator.getHistory()).toHaveLength(1);
    });
    
    test('clears history', () => {
        calculator.add(1, 2);
        calculator.clearHistory();
        expect(calculator.getHistory()).toHaveLength(0);
    });
});
```

---

## 🔗 Related Topics

- [01_Automated_Testing_Framework.md](./01_Automated_Testing_Framework.md)
- [03_Integration_Testing_Guide.md](./03_Integration_Testing_Guide.md)

---

**Next: [JavaScript Cheat Sheet](./17_DOCUMENTATION/01_JavaScript_Cheat_Sheet.md)**