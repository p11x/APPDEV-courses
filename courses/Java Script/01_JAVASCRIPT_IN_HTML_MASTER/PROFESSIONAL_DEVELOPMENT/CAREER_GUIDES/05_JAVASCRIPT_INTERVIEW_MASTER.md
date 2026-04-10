# 🎯 JavaScript Interview Prep Master

## Complete Interview Preparation Guide

---

## Table of Contents

1. [Common Questions](#common-questions)
2. [Algorithm Challenges](#algorithm-challenges)
3. [System Design](#system-design)
4. [Code Review Scenarios](#code-review-scenarios)

---

## Common Questions

### Variable Scope

```javascript
// What will this output?
let a = 1;

function outer() {
  let a = 2;
  
  function inner() {
    let a = 3;
    console.log(a);  // 3 - innermost
  }
  
  inner();
  console.log(a);    // 2 - outer scope
}

outer();
console.log(a);      // 1 - global scope
```

### Hoisting

```javascript
// What happens here?
console.log(foo());  // Returns "called"
console.log(bar());  // ERROR: bar is not a function

function foo() {
  return "called";
}

var bar = function() {
  return "called";
};

// Function declarations are hoisted entirely
// Variable declarations are hoisted (not values)
```

### This Binding

```javascript
const user = {
  name: 'John',
  greet() {
    console.log(`Hello, ${this.name}`);
  }
};

const greet = user.greet;
greet(); // 'Hello, undefined' - loses context

// Fix with arrow function
const greetArrow = () => console.log(`Hello, ${this.name}`);
```

---

## Algorithm Challenges

### Two Sum

```javascript
function twoSum(nums, target) {
  const map = new Map();
  
  for (let i = 0; i < nums.length; i++) {
    const complement = target - nums[i];
    
    if (map.has(complement)) {
      return [map.get(complement), i];
    }
    
    map.set(nums[i], i);
  }
  
  return [];
}
```

### Reverse Linked List

```javascript
function reverseList(head) {
  let prev = null;
  let current = head;
  
  while (current) {
    const next = current.next;
    current.next = prev;
    prev = current;
    current = next;
  }
  
  return prev;
}
```

### Binary Search

```javascript
function binarySearch(arr, target) {
  let left = 0;
  let right = arr.length - 1;
  
  while (left <= right) {
    const mid = Math.floor((left + right) / 2);
    
    if (arr[mid] === target) {
      return mid;
    } else if (arr[mid] < target) {
      left = mid + 1;
    } else {
      right = mid - 1;
    }
  }
  
  return -1;
}
```

---

## System Design

### URL Shortener

```javascript
class URLShortener {
  constructor() {
    this.urlToCode = new Map();
    this.codeToUrl = new Map();
  }

  encode(url) {
    if (this.urlToCode.has(url)) {
      return this.urlToCode.get(url);
    }
    
    const code = this.generateCode();
    this.urlToCode.set(url, code);
    this.codeToUrl.set(code, url);
    
    return code;
  }

  decode(code) {
    return this.codeToUrl.get(code);
  }

  generateCode() {
    const chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let code = '';
    
    for (let i = 0; i < 6; i++) {
      code += chars[Math.floor(Math.random() * chars.length)];
    }
    
    return code;
  }
}
```

### Rate Limiter

```javascript
class RateLimiter {
  constructor(limit, windowMs) {
    this.requests = new Map();
    this.limit = limit;
    this.windowMs = windowMs;
  }

  isAllowed(key) {
    const now = Date.now();
    const requests = this.requests.get(key) || [];
    
    // Remove old requests outside window
    const validRequests = requests.filter(
      time => now - time < this.windowMs
    );
    
    if (validRequests.length >= this.limit) {
      return false;
    }
    
    validRequests.push(now);
    this.requests.set(key, validRequests);
    
    return true;
  }
}
```

---

## Code Review Scenarios

### Memory Leak Detection

```javascript
// ❌ BAD - Memory leak
function createLeak() {
  const largeArray = new Array(1000000);
  
  setInterval(() => {
    console.log(largeArray);
  }, 1000);
}

// ✅ GOOD - Properly cleaned
function noLeak() {
  const largeArray = new Array(1000000);
  
  const interval = setInterval(() => {
    console.log(largeArray);
  }, 1000);
  
  return () => clearInterval(interval);  // Cleanup function
}
```

### Promise Handling

```javascript
// ❌ BAD - Unhandled rejection
async function bad() {
  fetch('/api/data')  // No .catch()
    .then(data => process(data));
}

// ✅ GOOD - Proper error handling
async function good() {
  try {
    const response = await fetch('/api/data');
    return await response.json();
  } catch (error) {
    console.error('Failed:', error);
    throw error;
  }
}
```

---

## Summary

### Key Takeaways

1. **Scope** - Understand closure
2. **Algorithms** - Practice common patterns
3. **System Design** - Scale considerations
4. **Code Review** - Spot common issues

---

*Last updated: 2024*