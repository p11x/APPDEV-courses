# 🎤 JavaScript Interview Prep

## Questions and Coding Challenges

---

## Table of Contents

1. [Technical Questions](#technical-questions)
2. [Coding Challenges](#coding-challenges)
3. [System Design](#system-design)

---

## Technical Questions

### Difference between == and ===

```javascript
// == compares value (type coercion)
'1' == 1  // true
'1' === 1   // false

// === compares value and type
'1' === '1'  // true
```

### Explain closure

```javascript
function counter() {
  let count = 0;
  
  return function() {
    return ++count;
  };
}

const increment = counter();
increment(); // 1
increment(); // 2
```

### Event Loop

```javascript
console.log('1');
setTimeout(() => console.log('2'), 0);
Promise.resolve().then(() => console.log('3'));
console.log('4');

// Output: 1, 4, 3, 2
// microtasks (Promise) run before macrotasks (setTimeout)
```

---

## Coding Challenges

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

### Reverse String

```javascript
function reverseString(str) {
  return str.split('').reverse().join('');
}

// Or with reduce
function reverseString(str) {
  return [...str].reduce((rev, char) => char + rev, '');
}
```

### Palindrome

```javascript
function isPalindrome(str) {
  const cleaned = str.toLowerCase().replace(/[^a-z0-9]/g, '');
  return cleaned === cleaned.split('').reverse().join('');
}
```

---

## Summary

### Key Takeaways

1. **Fundamentals**: == vs ===, closures
2. **Async**: Event loop
3. **Algorithms**: Array, string

### Next Steps

- Continue with: [02_JAVASCRIPT_CAREER_DEVELOPMENT.md](02_JAVASCRIPT_CAREER_DEVELOPMENT.md)
- Practice coding
- Study solutions

---

## Cross-References

- **Previous**: [../REFERENCE_MATERIALS/08_JAVASCRIPT_PERFORMANCE_OPTIMIZATION.md](../REFERENCE_MATERIALS/08_JAVASCRIPT_PERFORMANCE_OPTIMIZATION.md)
- **Next**: [02_JAVASCRIPT_CAREER_DEVELOPMENT.md](02_JAVASCRIPT_CAREER_DEVELOPMENT.md)

---

*Last updated: 2024*