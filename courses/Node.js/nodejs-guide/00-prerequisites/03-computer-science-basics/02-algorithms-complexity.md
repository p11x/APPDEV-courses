# Algorithms and Time Complexity

## What You'll Learn

- Understanding algorithm complexity
- Big O notation and performance analysis
- Common algorithm patterns
- Memory concepts and data storage

## Big O Notation

Big O notation describes the upper bound of an algorithm's time or space complexity as input size grows.

### Common Complexities

```javascript
// O(1) - Constant time
function getFirst(arr) {
    return arr[0]; // Always same time regardless of array size
}

// O(log n) - Logarithmic time
function binarySearch(sortedArr, target) {
    let left = 0;
    let right = sortedArr.length - 1;
    
    while (left <= right) {
        const mid = Math.floor((left + right) / 2);
        if (sortedArr[mid] === target) return mid;
        if (sortedArr[mid] < target) left = mid + 1;
        else right = mid - 1;
    }
    return -1;
}

// O(n) - Linear time
function findMax(arr) {
    let max = arr[0];
    for (let i = 1; i < arr.length; i++) {
        if (arr[i] > max) max = arr[i];
    }
    return max;
}

// O(n log n) - Linearithmic time
function mergeSort(arr) {
    if (arr.length <= 1) return arr;
    
    const mid = Math.floor(arr.length / 2);
    const left = mergeSort(arr.slice(0, mid));
    const right = mergeSort(arr.slice(mid));
    
    return merge(left, right);
}

// O(n²) - Quadratic time
function bubbleSort(arr) {
    for (let i = 0; i < arr.length; i++) {
        for (let j = 0; j < arr.length - i - 1; j++) {
            if (arr[j] > arr[j + 1]) {
                [arr[j], arr[j + 1]] = [arr[j + 1], arr[j]];
            }
        }
    }
    return arr;
}

// O(2^n) - Exponential time
function fibonacci(n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

// O(n!) - Factorial time
function permutations(arr) {
    if (arr.length <= 1) return [arr];
    
    const result = [];
    for (let i = 0; i < arr.length; i++) {
        const rest = [...arr.slice(0, i), ...arr.slice(i + 1)];
        const perms = permutations(rest);
        for (const perm of perms) {
            result.push([arr[i], ...perm]);
        }
    }
    return result;
}
```

### Complexity Comparison Table

| Complexity | Name | 10 items | 100 items | 1000 items |
|------------|------|----------|-----------|------------|
| O(1) | Constant | 1 | 1 | 1 |
| O(log n) | Logarithmic | 3 | 7 | 10 |
| O(n) | Linear | 10 | 100 | 1000 |
| O(n log n) | Linearithmic | 30 | 700 | 10,000 |
| O(n²) | Quadratic | 100 | 10,000 | 1,000,000 |
| O(2^n) | Exponential | 1024 | 1.27e+30 | 1.07e+301 |
| O(n!) | Factorial | 3.6M | 9.3e+157 | 4.02e+2567 |

## Sorting Algorithms

### Built-in Sort

```javascript
// JavaScript's built-in sort uses TimSort (O(n log n))
const arr = [5, 3, 8, 1, 2];
arr.sort((a, b) => a - b); // [1, 2, 3, 5, 8]
```

### Quick Sort Implementation

```javascript
function quickSort(arr) {
    if (arr.length <= 1) return arr;
    
    const pivot = arr[Math.floor(arr.length / 2)];
    const left = arr.filter(x => x < pivot);
    const middle = arr.filter(x => x === pivot);
    const right = arr.filter(x => x > pivot);
    
    return [...quickSort(left), ...middle, ...quickSort(right)];
}
```

### Merge Sort Implementation

```javascript
function merge(left, right) {
    const result = [];
    let i = 0, j = 0;
    
    while (i < left.length && j < right.length) {
        if (left[i] <= right[j]) {
            result.push(left[i++]);
        } else {
            result.push(right[j++]);
        }
    }
    
    return [...result, ...left.slice(i), ...right.slice(j)];
}

function mergeSort(arr) {
    if (arr.length <= 1) return arr;
    
    const mid = Math.floor(arr.length / 2);
    const left = mergeSort(arr.slice(0, mid));
    const right = mergeSort(arr.slice(mid));
    
    return merge(left, right);
}
```

## Search Algorithms

### Linear Search

```javascript
// O(n) - Check every element
function linearSearch(arr, target) {
    for (let i = 0; i < arr.length; i++) {
        if (arr[i] === target) return i;
    }
    return -1;
}
```

### Binary Search

```javascript
// O(log n) - Requires sorted array
function binarySearch(arr, target) {
    let left = 0;
    let right = arr.length - 1;
    
    while (left <= right) {
        const mid = Math.floor((left + right) / 2);
        
        if (arr[mid] === target) return mid;
        if (arr[mid] < target) left = mid + 1;
        else right = mid - 1;
    }
    
    return -1;
}
```

### Hash Table Lookup

```javascript
// O(1) - Using object or Map
function hashSearch(arr, target) {
    const map = new Map();
    for (let i = 0; i < arr.length; i++) {
        map.set(arr[i], i);
    }
    return map.get(target) ?? -1;
}
```

## Algorithm Design Patterns

### Divide and Conquer

```javascript
// Example: Find maximum subarray sum
function maxSubarraySum(arr) {
    if (arr.length === 0) return 0;
    
    let maxCurrent = arr[0];
    let maxGlobal = arr[0];
    
    for (let i = 1; i < arr.length; i++) {
        maxCurrent = Math.max(arr[i], maxCurrent + arr[i]);
        maxGlobal = Math.max(maxGlobal, maxCurrent);
    }
    
    return maxGlobal;
}
```

### Dynamic Programming

```javascript
// Example: Fibonacci with memoization
function fibonacci(n, memo = {}) {
    if (n in memo) return memo[n];
    if (n <= 1) return n;
    
    memo[n] = fibonacci(n - 1, memo) + fibonacci(n - 2, memo);
    return memo[n];
}

// Bottom-up approach
function fibonacciDP(n) {
    if (n <= 1) return n;
    
    const dp = [0, 1];
    for (let i = 2; i <= n; i++) {
        dp[i] = dp[i - 1] + dp[i - 2];
    }
    
    return dp[n];
}
```

### Greedy Algorithms

```javascript
// Example: Coin change (greedy approach)
function coinChange(coins, amount) {
    coins.sort((a, b) => b - a); // Sort descending
    let count = 0;
    let remaining = amount;
    
    for (const coin of coins) {
        while (remaining >= coin) {
            remaining -= coin;
            count++;
        }
    }
    
    return remaining === 0 ? count : -1;
}
```

## Space Complexity

### In-Place vs Out-of-Place

```javascript
// O(n) space - Creates new array
function reverseOutOfPlace(arr) {
    const reversed = [];
    for (let i = arr.length - 1; i >= 0; i--) {
        reversed.push(arr[i]);
    }
    return reversed;
}

// O(1) space - Modifies in place
function reverseInPlace(arr) {
    let left = 0;
    let right = arr.length - 1;
    
    while (left < right) {
        [arr[left], arr[right]] = [arr[right], arr[left]];
        left++;
        right--;
    }
    
    return arr;
}
```

### Memory-Efficient Data Structures

```javascript
// Typed arrays for numeric data
const intArray = new Int32Array(1000); // 4 bytes per element
const floatArray = new Float64Array(1000); // 8 bytes per element

// ArrayBuffer for binary data
const buffer = new ArrayBuffer(16);
const view = new DataView(buffer);
view.setInt32(0, 42);
```

## Performance Analysis

### Measuring Performance

```javascript
function measurePerformance(fn, iterations = 1000) {
    const start = performance.now();
    
    for (let i = 0; i < iterations; i++) {
        fn();
    }
    
    const end = performance.now();
    const avgTime = (end - start) / iterations;
    
    return {
        totalTime: end - start,
        averageTime: avgTime,
        iterations
    };
}

// Example usage
const result = measurePerformance(() => {
    const arr = Array.from({ length: 1000 }, () => Math.random());
    arr.sort((a, b) => a - b);
}, 100);

console.log(`Average time: ${result.averageTime.toFixed(3)}ms`);
```

### Profiling Tips

```javascript
// Use console.time for quick measurements
console.time('sort');
arr.sort((a, b) => a - b);
console.timeEnd('sort');

// Use performance.mark for detailed profiling
performance.mark('start');
// ... code to measure
performance.mark('end');
performance.measure('operation', 'start', 'end');

const measures = performance.getEntriesByName('operation');
console.log(`Duration: ${measures[0].duration}ms`);
```

## Troubleshooting Common Issues

### Stack Overflow with Recursion

```javascript
// Problem: Deep recursion causes stack overflow
function factorial(n) {
    if (n <= 1) return 1;
    return n * factorial(n - 1); // Stack overflow for large n
}

// Solution: Use iteration
function factorialIterative(n) {
    let result = 1;
    for (let i = 2; i <= n; i++) {
        result *= i;
    }
    return result;
}

// Solution: Use tail call optimization (if supported)
function factorialTail(n, acc = 1) {
    if (n <= 1) return acc;
    return factorialTail(n - 1, n * acc); // Tail position
}
```

### Memory Leaks in Algorithms

```javascript
// Problem: Unbounded cache growth
const cache = new Map();
function expensiveOperation(key) {
    if (cache.has(key)) return cache.get(key);
    
    const result = compute(key);
    cache.set(key, result); // Never cleared
    return result;
}

// Solution: Use LRU cache or WeakRef
class LRUCache {
    constructor(maxSize) {
        this.maxSize = maxSize;
        this.cache = new Map();
    }
    
    get(key) {
        if (!this.cache.has(key)) return undefined;
        
        const value = this.cache.get(key);
        this.cache.delete(key);
        this.cache.set(key, value); // Move to end
        return value;
    }
    
    set(key, value) {
        if (this.cache.has(key)) {
            this.cache.delete(key);
        } else if (this.cache.size >= this.maxSize) {
            const firstKey = this.cache.keys().next().value;
            this.cache.delete(firstKey);
        }
        
        this.cache.set(key, value);
    }
}
```

## Best Practices Checklist

- [ ] Analyze algorithm complexity before implementation
- [ ] Choose appropriate data structures for operations
- [ ] Use built-in methods when available (sort, map, filter)
- [ ] Consider space complexity, not just time
- [ ] Memoize expensive computations
- [ ] Use iterative solutions for large inputs
- [ ] Profile performance-critical code
- [ ] Implement caching for repeated operations
- [ ] Consider algorithm trade-offs (time vs space)
- [ ] Test with various input sizes

## Performance Optimization Tips

- Use hash tables (Map/Object) for O(1) lookups
- Prefer iteration over recursion for large inputs
- Use binary search on sorted data
- Implement caching for expensive computations
- Use appropriate data structures for operations
- Consider algorithm parallelization when possible
- Use typed arrays for numeric operations
- Profile before optimizing

## Cross-References

- See [Data Structures](./01-data-structures.md) for data structure details
- See [Performance Optimization](../06-performance-optimization/) for Node.js specifics
- See [Node.js Installation](../05-nodejs-installation/) for environment setup
- See [Testing Environment](../06-testing-environment/) for performance testing

## Next Steps

Now that you understand algorithms and complexity, let's analyze system requirements. Continue to [System Requirements Analysis](../04-system-requirements/).