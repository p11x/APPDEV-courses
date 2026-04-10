# JavaScript Data Structures & Algorithms: Complete Mastery Guide

Understanding algorithms is essential for writing efficient JavaScript applications. This comprehensive guide covers searching algorithms, sorting algorithms, time complexity analysis using Big O notation, and practical algorithm implementations used in production applications.

---

## Table of Contents

1. [Big O Notation Fundamentals](#big-o-notation-fundamentals)
2. [Search Algorithms](#search-algorithms)
3. [Sort Algorithms](#sort-algorithms)
4. [Time Complexity Analysis](#time-complexity-analysis)
5. [Algorithm Patterns](#algorithm-patterns)
6. [Professional Applications](#professional-applications)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pitfalls)
9. [Related Files](#related-files)

---

## Big O Notation Fundamentals

Big O notation describes algorithm performance as input size grows, helping you choose the right approach for your use case.

### Common Time Complexities

Understanding the common complexity classes helps in algorithm selection.

```javascript
// O(1) - Constant Time
// Operations that take the same time regardless of input size
function getFirst(array) {
    return array[0];
}

function isEven(number) {
    return number % 2 === 0;
}

// O(log n) - Logarithmic Time
// Binary search cuts problem in half each step
function binarySearch(array, target) {
    let left = 0;
    let right = array.length - 1;
    
    while (left <= right) {
        const mid = Math.floor((left + right) / 2);
        
        if (array[mid] === target) return mid;
        if (array[mid] < target) left = mid + 1;
        else right = mid - 1;
    }
    
    return -1;
}

// O(n) - Linear Time
// Operations that process each input element once
function findMax(array) {
    let max = array[0];
    for (const item of array) {
        if (item > max) max = item;
    }
    return max;
}

function countOccurrences(array, target) {
    let count = 0;
    for (const item of array) {
        if (item === target) count++;
    }
    return count;
}

// O(n log n) - Linearithmic Time
// Efficient sorting algorithms
function mergeSort(array) {
    if (array.length <= 1) return array;
    
    const mid = Math.floor(array.length / 2);
    const left = mergeSort(array.slice(0, mid));
    const right = mergeSort(array.slice(mid));
    
    return merge(left, right);
}

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
    
    return result.concat(left.slice(i)).concat(right.slice(j));
}

// O(n²) - Quadratic Time
// Nested loops
function bubbleSort(array) {
    const arr = [...array];
    const n = arr.length;
    
    for (let i = 0; i < n - 1; i++) {
        for (let j = 0; j < n - i - 1; j++) {
            if (arr[j] > arr[j + 1]) {
                [arr[j], arr[j + 1]] = [arr[j + 1], arr[j]];
            }
        }
    }
    
    return arr;
}

// O(2^n) - Exponential Time
// Recursive algorithms that grow exponentially
function fibonacci(n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

// O(n!) - Factorial Time
// Permutations - extremely expensive
function permutations(arr) {
    if (arr.length <= 1) return [arr];
    
    const result = [];
    for (let i = 0; i < arr.length; i++) {
        const current = arr[i];
        const remaining = [...arr.slice(0, i), ...arr.slice(i + 1)];
        
        for (const perm of permutations(remaining)) {
            result.push([current, ...perm]);
        }
    }
    
    return result;
}
```

### Space Complexity

Memory usage is equally important as time complexity.

```javascript
// O(1) space - in-place operations
function reverseInPlace(array) {
    let left = 0;
    let right = array.length - 1;
    
    while (left < right) {
        [array[left], array[right]] = [array[right], array[left]];
        left++;
        right--;
    }
    
    return array;
}

// O(n) space - new array created
function reverseCopy(array) {
    return array.slice().reverse();
}

// O(n) space - recursion stack
function sumRecursively(array, index = 0) {
    if (index >= array.length) return 0;
    return array[index] + sumRecursively(array, index + 1);
}
```

---

## Search Algorithms

Finding elements efficiently is fundamental. Different algorithms suit different data characteristics.

### Linear Search

Simple but works on any sorted or unsorted data.

```javascript
// Basic linear search
function linearSearch(array, target) {
    for (let i = 0; i < array.length; i++) {
        if (array[i] === target) {
            return i;
        }
    }
    return -1;
}

// Linear search with custom comparator
function linearSearchBy(array, target, comparator) {
    for (let i = 0; i < array.length; i++) {
        if (comparator(array[i], target) === 0) {
            return i;
        }
    }
    return -1;
}

// Find all occurrences
function findAllOccurrences(array, target) {
    const indices = [];
    for (let i = 0; i < array.length; i++) {
        if (array[i] === target) {
            indices.push(i);
        }
    }
    return indices;
}

// Usage
const numbers = [4, 2, 7, 1, 9, 3, 5];
console.log(linearSearch(numbers, 7));     // 2
console.log(linearSearch(numbers, 6));     // -1

const users = [
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' },
    { id: 3, name: 'Charlie' }
];
console.log(linearSearchBy(users, 2, (a, b) => a.id - b));  // 1
```

### Binary Search

Requires sorted data but provides O(log n) performance.

```javascript
// Iterative binary search
function binarySearch(array, target) {
    let left = 0;
    let right = array.length - 1;
    
    while (left <= right) {
        const mid = Math.floor((left + right) / 2);
        
        if (array[mid] === target) {
            return mid;
        } else if (array[mid] < target) {
            left = mid + 1;
        } else {
            right = mid - 1;
        }
    }
    
    return -1;
}

// Recursive binary search
function binarySearchRecursive(array, target, left = 0, right = array.length - 1) {
    if (left > right) return -1;
    
    const mid = Math.floor((left + right) / 2);
    
    if (array[mid] === target) {
        return mid;
    } else if (array[mid] < target) {
        return binarySearchRecursive(array, target, mid + 1, right);
    } else {
        return binarySearchRecursive(array, target, left, mid - 1);
    }
}

// Find first occurrence
function binarySearchFirst(array, target) {
    let left = 0;
    let right = array.length - 1;
    let result = -1;
    
    while (left <= right) {
        const mid = Math.floor((left + right) / 2);
        
        if (array[mid] === target) {
            result = mid;
            right = mid - 1;  // Continue searching left
        } else if (array[mid] < target) {
            left = mid + 1;
        } else {
            right = mid - 1;
        }
    }
    
    return result;
}

// Find closest value
function binarySearchClosest(array, target) {
    if (array.length === 0) return -1;
    
    let left = 0;
    let right = array.length - 1;
    
    if (target <= array[left]) return left;
    if (target >= array[right]) return right;
    
    while (left < right - 1) {
        const mid = Math.floor((left + right) / 2);
        
        if (array[mid] === target) return mid;
        if (array[mid] < target) left = mid;
        else right = mid;
    }
    
    // Return index of closest value
    return target - array[left] < array[right] - target ? left : right;
}

// Usage
const sorted = [1, 3, 5, 7, 9, 11, 13, 15];
console.log(binarySearch(sorted, 7));     // 3
console.log(binarySearch(sorted, 6));   // -1
```

### Interpolation Search

Works on uniformly distributed sorted data with O(log log n) average complexity.

```javascript
// Interpolation search for uniformly distributed data
function interpolationSearch(array, target) {
    let left = 0;
    let right = array.length - 1;
    
    while (left <= right && target >= array[left] && target <= array[right]) {
        // Estimate position based on value range
        const pos = left + Math.floor(
            ((target - array[left]) * (right - left)) /
            (array[right] - array[left])
        );
        
        if (array[pos] === target) {
            return pos;
        } else if (array[pos] < target) {
            left = pos + 1;
        } else {
            right = pos - 1;
        }
    }
    
    return -1;
}

// Usage - works great for uniformly distributed data
const uniform = Array.from({ length: 1000 }, (_, i) => i * 2);
console.log(interpolationSearch(uniform, 500));  // 250
```

---

## Sort Algorithms

Sorting is among the most common operations in software. Understanding different algorithms helps choose the right one.

### Bubble Sort

Simple but inefficient for large datasets.

```javascript
// Basic bubble sort
function bubbleSort(array) {
    const arr = [...array];
    const n = arr.length;
    
    for (let i = 0; i < n; i++) {
        for (let j = 0; j < n - i - 1; j++) {
            if (arr[j] > arr[j + 1]) {
                [arr[j], arr[j + 1]] = [arr[j + 1], arr[j]];
            }
        }
    }
    
    return arr;
}

// Optimized bubble sort - early exit
function bubbleSortOptimized(array) {
    const arr = [...array];
    const n = arr.length;
    let swapped = true;
    
    for (let i = 0; i < n && swapped; i++) {
        swapped = false;
        
        for (let j = 0; j < n - i - 1; j++) {
            if (arr[j] > arr[j + 1]) {
                [arr[j], arr[j + 1]] = [arr[j + 1], arr[j]];
                swapped = true;
            }
        }
    }
    
    return arr;
}

// Bubble sort with callback for custom comparison
function bubbleSortBy(array, comparator) {
    const arr = [...array];
    const n = arr.length;
    
    for (let i = 0; i < n; i++) {
        for (let j = 0; j < n - i - 1; j++) {
            if (comparator(arr[j], arr[j + 1]) > 0) {
                [arr[j], arr[j + 1]] = [arr[j + 1], arr[j]];
            }
        }
    }
    
    return arr;
}

// Usage
console.log(bubbleSort([5, 3, 8, 4, 2]));  // [2, 3, 4, 5, 8]
console.log(bubbleSortBy(
    [{ name: 'Bob', age: 30 }, { name: 'Alice', age: 25 }],
    (a, b) => a.name.localeCompare(b.name)
));
```

### Quick Sort

Efficient average case O(n log n), widely used in practice.

```javascript
// Quick sort implementation
function quickSort(array) {
    if (array.length <= 1) return array;
    
    const pivot = array[array.length - 1];
    const left = [];
    const right = [];
    
    for (let i = 0; i < array.length - 1; i++) {
        if (array[i] < pivot) {
            left.push(array[i]);
        } else {
            right.push(array[i]);
        }
    }
    
    return [...quickSort(left), pivot, ...quickSort(right)];
}

// In-place quick sort with partitioning
function quickSortInPlace(array, left = 0, right = array.length - 1) {
    if (left >= right) return;
    
    const pivotIndex = partition(array, left, right);
    quickSortInPlace(array, left, pivotIndex - 1);
    quickSortInPlace(array, pivotIndex + 1, right);
    
    return array;
}

function partition(array, left, right) {
    const pivot = array[right];
    let i = left - 1;
    
    for (let j = left; j < right; j++) {
        if (array[j] < pivot) {
            i++;
            [array[i], array[j]] = [array[j], array[i]];
        }
    }
    
    [array[i + 1], array[right]] = [array[right], array[i + 1]];
    return i + 1;
}

// Quick sort with custom comparator
function quickSortBy(array, comparator) {
    if (array.length <= 1) return array;
    
    const pivot = array[array.length - 1];
    const left = [];
    const right = [];
    const equal = [];
    
    for (const item of array) {
        const cmp = comparator(item, pivot);
        if (cmp < 0) left.push(item);
        else if (cmp > 0) right.push(item);
        else equal.push(item);
    }
    
    return [
        ...quickSortBy(left, comparator),
        ...equal,
        ...quickSortBy(right, comparator)
    ];
}

// Usage
console.log(quickSort([5, 3, 8, 4, 2, 1, 9, 7]));  // [1, 2, 3, 4, 5, 7, 8, 9]
```

### Merge Sort

Stable O(n log n) sorting, excellent for linked lists.

```javascript
// Merge sort
function mergeSort(array) {
    if (array.length <= 1) return array;
    
    const mid = Math.floor(array.length / 2);
    const left = mergeSort(array.slice(0, mid));
    const right = mergeSort(array.slice(mid));
    
    return merge(left, right);
}

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
    
    return result.concat(left.slice(i)).concat(right.slice(j));
}

// In-place merge sort
function mergeSortInPlace(array, temp = [], left, right) {
    if (left === undefined) {
        left = 0;
        right = array.length - 1;
    }
    
    if (left >= right) return;
    
    const mid = Math.floor((left + right) / 2);
    mergeSortInPlace(array, temp, left, mid);
    mergeSortInPlace(array, temp, mid + 1, right);
    mergeInPlace(array, temp, left, mid, right);
    
    return array;
}

function mergeInPlace(array, temp, left, mid, right) {
    for (let i = left; i <= right; i++) {
        temp[i] = array[i];
    }
    
    let i = left;
    let j = mid + 1;
    let k = left;
    
    while (i <= mid && j <= right) {
        if (temp[i] <= temp[j]) {
            array[k++] = temp[i++];
        } else {
            array[k++] = temp[j++];
        }
    }
    
    while (i <= mid) {
        array[k++] = temp[i++];
    }
}

// Usage
console.log(mergeSort([5, 3, 8, 4, 2, 1, 9, 7]));
```

### Heap Sort

In-place O(n log n) sorting with guaranteed performance.

```javascript
// Heap sort
function heapSort(array) {
    const arr = [...array];
    const n = arr.length;
    
    // Build max heap
    for (let i = Math.floor(n / 2) - 1; i >= 0; i--) {
        heapify(arr, n, i);
    }
    
    // Extract elements from heap
    for (let i = n - 1; i > 0; i--) {
        [arr[0], arr[i]] = [arr[i], arr[0]];
        heapify(arr, i, 0);
    }
    
    return arr;
}

function heapify(array, n, i) {
    let largest = i;
    const left = 2 * i + 1;
    const right = 2 * i + 2;
    
    if (left < n && array[left] > array[largest]) {
        largest = left;
    }
    
    if (right < n && array[right] > array[largest]) {
        largest = right;
    }
    
    if (largest !== i) {
        [array[i], array[largest]] = [array[largest], array[i]];
        heapify(array, n, largest);
    }
}

// Usage
console.log(heapSort([5, 3, 8, 4, 2, 1, 9, 7]));
```

---

## Time Complexity Analysis

Understanding and analyzing algorithm complexity helps make informed decisions.

### Analyzing Code Complexity

```javascript
// O(1) - constant
function getMiddle(array) {
    return array[Math.floor(array.length / 2)];
}

// O(n) - linear - single loop
function sumArray(array) {
    let sum = 0;
    for (const n of array) {
        sum += n;
    }
    return sum;
}

// O(n²) - quadratic - nested loops
function findDuplicates(array) {
    const duplicates = [];
    for (let i = 0; i < array.length; i++) {
        for (let j = i + 1; j < array.length; j++) {
            if (array[i] === array[j]) {
                duplicates.push(array[i]);
            }
        }
    }
    return [...new Set(duplicates)];
}

// O(2^n) - exponential - branching recursion
function findSubsets(array) {
    if (array.length === 0) return [[]];
    
    const first = array[0];
    const rest = array.slice(1);
    const subsets = findSubsets(rest);
    
    return [
        ...subsets,
        ...subsets.map(s => [first, ...s])
    ];
}

// O(n!) - factorial
function permutations(arr) {
    if (arr.length <= 1) return [arr];
    
    return arr.flatMap((item, i) => {
        const rest = [...arr.slice(0, i), ...arr.slice(i + 1)];
        return permutations(rest).map(p => [item, ...p]);
    });
}
```

### Complexity of Built-in Methods

```javascript
// JavaScript built-in complexity (assumptions for average engine)

// Array operations
const arr = [];

// push/pop - O(1) amortized
arr.push(1);
arr.pop();

// shift/unshift - O(n) - requires shifting
arr.shift();
arr.unshift(1);

// indexOf/includes - O(n)
arr.indexOf(1);
arr.includes(1);

// sort - O(n log n)
arr.sort();

// Map operations - O(1) average
const map = new Map();
map.set(1, 'value');
map.get(1);
map.has(1);

// Set operations - O(1) average
const set = new Set();
set.add(1);
set.has(1);

// Object operations - O(1) average
const obj = {};
obj.key = 'value';
obj.key;
```

---

## Algorithm Patterns

Common patterns for solving algorithmic problems.

### Sliding Window

Efficient for processing subarrays.

```javascript
// Maximum sum of subarray of size k
function maxSubarraySum(array, k) {
    if (array.length < k) return null;
    
    // Calculate first window sum
    let windowSum = array.slice(0, k).reduce((a, b) => a + b, 0);
    let maxSum = windowSum;
    
    // Slide window
    for (let i = k; i < array.length; i++) {
        windowSum = windowSum + array[i] - array[i - k];
        maxSum = Math.max(maxSum, windowSum);
    }
    
    return maxSum;
}

// Average of each window
function slidingWindowAverage(array, k) {
    const averages = [];
    
    for (let i = 0; i <= array.length - k; i++) {
        const window = array.slice(i, i + k);
        const avg = window.reduce((a, b) => a + b, 0) / k;
        averages.push(avg);
    }
    
    return averages;
}

// Longest substring without repeat
function longestUniqueSubstring(s) {
    const seen = new Map();
    let maxLength = 0;
    let start = 0;
    let result = '';
    
    for (let i = 0; i < s.length; i++) {
        if (seen.has(s[i]) && seen.get(s[i]) >= start) {
            start = seen.get(s[i]) + 1;
        }
        
        seen.set(s[i], i);
        
        if (i - start + 1 > maxLength) {
            maxLength = i - start + 1;
            result = s.slice(start, i + 1);
        }
    }
    
    return result;
}

// Usage
console.log(maxSubarraySum([1, 4, 2, 10, 2, 3, 1, 0, 20], 4));  // 24
console.log(longestUniqueSubstring('abcabcbb'));  // 'abc'
```

### Two Pointers

Efficient for sorted arrays.

```javascript
// Two sum (sorted array)
function twoSumSorted(array, target) {
    let left = 0;
    let right = array.length - 1;
    
    while (left < right) {
        const sum = array[left] + array[right];
        
        if (sum === target) {
            return [left, right];
        } else if (sum < target) {
            left++;
        } else {
            right--;
        }
    }
    
    return null;
}

// Remove duplicates in place
function removeDuplicates(array) {
    if (array.length === 0) return 0;
    
    let slow = 0;
    
    for (let fast = 1; fast < array.length; fast++) {
        if (array[fast] !== array[slow]) {
            slow++;
            array[slow] = array[fast];
        }
    }
    
    return slow + 1;
}

// Merge sorted arrays
function mergeSortedArrays(a, b) {
    const result = [];
    let i = 0, j = 0;
    
    while (i < a.length && j < b.length) {
        if (a[i] <= b[j]) {
            result.push(a[i++]);
        } else {
            result.push(b[j++]);
        }
    }
    
    return result.concat(a.slice(i)).concat(b.slice(j));
}

// Is palindrome
function isPalindrome(s) {
    let left = 0;
    let right = s.length - 1;
    
    while (left < right) {
        if (s[left] !== s[right]) return false;
        left++;
        right--;
    }
    
    return true;
}

// Usage
console.log(twoSumSorted([1, 2, 3, 4, 6], 10));  // [3, 4]
console.log(removeDuplicates([1, 1, 2, 2, 3]));    // 3
```

### Divide and Conquer

Breaking problems into smaller subproblems.

```javascript
// Binary search is divide and conquer
function binarySearchDivideConquer(array, target) {
    if (array.length === 0) return -1;
    
    const mid = Math.floor(array.length / 2);
    
    if (array[mid] === target) {
        return mid;
    } else if (array[mid] < target) {
        const result = binarySearchDivideConquer(array.slice(mid + 1), target);
        return result === -1 ? -1 : mid + 1 + result;
    } else {
        return binarySearchDivideConquer(array.slice(0, mid), target);
    }
}

// Find peak element
function findPeak(array) {
    if (array.length === 0) return -1;
    if (array.length === 1) return 0;
    
    let left = 0;
    let right = array.length - 1;
    
    while (left <= right) {
        const mid = Math.floor((left + right) / 2);
        
        const leftVal = array[mid - 1] ?? -Infinity;
        const rightVal = array[mid + 1] ?? -Infinity;
        
        if (array[mid] >= leftVal && array[mid] >= rightVal) {
            return mid;
        } else if (array[mid] < leftVal) {
            right = mid - 1;
        } else {
            left = mid + 1;
        }
    }
    
    return -1;
}

// Usage
console.log(findPeak([1, 3, 2, 1, 5, 0, 6]));  // 1 or 5
```

### Dynamic Programming

Memoization for overlapping subproblems.

```javascript
// Fibonacci with memoization
function fibonacciMemo(n, memo = {}) {
    if (n in memo) return memo[n];
    if (n <= 1) return n;
    
    memo[n] = fibonacciMemo(n - 1, memo) + fibonacciMemo(n - 2, memo);
    return memo[n];
}

// Climbing stairs
function climbStairs(n) {
    if (n <= 2) return n;
    
    const dp = [0, 1, 2];
    for (let i = 3; i <= n; i++) {
        dp[i] = dp[i - 1] + dp[i - 2];
    }
    return dp[n];
}

// Longest common subsequence
function LCS(s1, s2) {
    const m = s1.length;
    const n = s2.length;
    const dp = Array(m + 1).fill(null).map(() => Array(n + 1).fill(0));
    
    for (let i = 1; i <= m; i++) {
        for (let j = 1; j <= n; j++) {
            if (s1[i - 1] === s2[j - 1]) {
                dp[i][j] = dp[i - 1][j - 1] + 1;
            } else {
                dp[i][j] = Math.max(dp[i - 1][j], dp[i][j - 1]);
            }
        }
    }
    
    return dp[m][n];
}

// Coin change
function coinChange(coins, amount) {
    const dp = Array(amount + 1).fill(Infinity);
    dp[0] = 0;
    
    for (let i = 1; i <= amount; i++) {
        for (const coin of coins) {
            if (coin <= i && dp[i - coin] + 1 < dp[i]) {
                dp[i] = dp[i - coin] + 1;
            }
        }
    }
    
    return dp[amount] === Infinity ? -1 : dp[amount];
}

// Usage
console.log(fibonacciMemo(50));  // Large number without stack overflow
console.log(climbStairs(10));  // 89
console.log(coinChange([1, 2, 5], 11));  // 3
```

---

## Professional Applications

### Search Implementation with Chaining

```javascript
class SearchIndex {
    constructor() {
        this.index = new Map();
    }
    
    addDocument(id, content) {
        const words = content.toLowerCase().split(/\s+/);
        
        for (const word of words) {
            if (!this.index.has(word)) {
                this.index.set(word, new Set());
            }
            this.index.get(word).add(id);
        }
    }
    
    search(query) {
        const words = query.toLowerCase().split(/\s+/);
        
        if (words.length === 1) {
            return [...(this.index.get(words[0]) || [])];
        }
        
        // AND search
        const resultSets = words.map(w => this.index.get(w) || new Set());
        const [first, ...rest] = resultSets;
        
        return [...first].filter(id => rest.every(set => set.has(id)));
    }
    
    removeDocument(id, content) {
        const words = content.toLowerCase().split(/\s+/);
        
        for (const word of words) {
            const ids = this.index.get(word);
            if (ids) {
                ids.delete(id);
                if (ids.size === 0) {
                    this.index.delete(word);
                }
            }
        }
    }
}

// Usage
const searchIndex = new SearchIndex();
searchIndex.addDocument(1, 'javascript tutorial');
searchIndex.addDocument(2, 'python tutorial');
searchIndex.addDocument(3, 'javascript algorithms');
console.log(searchIndex.search('javascript'));  // [1, 3]
console.log(searchIndex.search('javascript tutorial'));  // [1]
```

### Priority Queue

```javascript
class PriorityQueue {
    constructor() {
        this.heap = [];
    }
    
    enqueue(item, priority) {
        this.heap.push({ item, priority });
        this.bubbleUp(this.heap.length - 1);
    }
    
    dequeue() {
        if (this.isEmpty()) return null;
        
        const min = this.heap[0];
        const last = this.heap.pop();
        
        if (!this.isEmpty()) {
            this.heap[0] = last;
            this.bubbleDown(0);
        }
        
        return min.item;
    }
    
    bubbleUp(index) {
        while (index > 0) {
            const parent = Math.floor((index - 1) / 2);
            
            if (this.heap[index].priority >= this.heap[parent].priority) {
                break;
            }
            
            [this.heap[index], this.heap[parent]] = 
                [this.heap[parent], this.heap[index]];
            index = parent;
        }
    }
    
    bubbleDown(index) {
        while (true) {
            const left = 2 * index + 1;
            const right = 2 * index + 2;
            let smallest = index;
            
            if (left < this.heap.length && 
                this.heap[left].priority < this.heap[smallest].priority) {
                smallest = left;
            }
            
            if (right < this.heap.length && 
                this.heap[right].priority < this.heap[smallest].priority) {
                smallest = right;
            }
            
            if (smallest === index) break;
            
            [this.heap[index], this.heap[smallest]] = 
                [this.heap[smallest], this.heap[index]];
            index = smallest;
        }
    }
    
    isEmpty() {
        return this.heap.length === 0;
    }
    
    get size() {
        return this.heap.length;
    }
}

// Usage
const pq = new PriorityQueue();
pq.enqueue('low priority task', 3);
pq.enqueue('high priority task', 1);
pq.enqueue('medium priority task', 2);
console.log(pq.dequeue());  // 'high priority task'
```

### Trie (Prefix Tree)

```javascript
class Trie {
    constructor() {
        this.root = {};
    }
    
    insert(word) {
        let node = this.root;
        
        for (const char of word) {
            if (!node[char]) {
                node[char] = {};
            }
            node = node[char];
        }
        
        node.isEnd = true;
    }
    
    search(word) {
        const node = this.traverse(word);
        return node && node.isEnd === true;
    }
    
    startsWith(prefix) {
        return this.traverse(prefix) !== null;
    }
    
    traverse(prefix) {
        let node = this.root;
        
        for (const char of prefix) {
            if (!node[char]) {
                return null;
            }
            node = node[char];
        }
        
        return node;
    }
    
    getSuggestions(prefix) {
        const results = [];
        const node = this.traverse(prefix);
        
        if (!node) return results;
        
        this.collectAllWords(node, prefix, results);
        
        return results;
    }
    
    collectAllWords(node, prefix, results) {
        if (node.isEnd) {
            results.push(prefix);
        }
        
        for (const char in node) {
            if (char !== 'isEnd') {
                this.collectAllWords(node[char], prefix + char, results);
            }
        }
    }
}

// Usage
const trie = new Trie();
['apple', 'app', 'apricot', 'banana', 'band', 'bandana'].forEach(w => trie.insert(w));
console.log(trie.search('apple'));  // true
console.log(trie.startsWith('app'));  // true
console.log(trie.getSuggestions('ban'));  // ['banana', 'band', 'bandana']
```

---

## Key Takeaways

1. **Binary search requires sorted array**: O(log n) vs O(n) for linear search
2. **Quick sort average O(n log n)**: In-place but not stable
3. **Merge sort stable O(n log n)**: Requires O(n) extra space
4. **Heap sort in-place O(n log n)**: Not stable, good for memory constraints
5. **Use built-in sort for production**: Engine implementations are highly optimized
6. **Sliding window for subarray problems**: Converts O(n²) to O(n)
7. **Two pointers for sorted data**: Often O(n) instead of O(n²)

---

## Common Pitfalls

1. **Using sort without comparator**: Results in string comparison
2. **Binary search on unsorted data**: Produces incorrect results
3. **Forgetting base case in recursion**: Causes stack overflow
4. **Not handling edge cases**: Empty arrays, single elements
5. **Mutating original array**: Use copies to preserve data
6. **Fibonacci without memoization**: Exponential instead of linear
7. **Confusing time and space complexity**: Must consider both

---

## Related Files

- **01_ARRAYS_MASTER.md**: Sorting arrays, array operations
- **02_OBJECTS_AND_PROPERTIES.md**: Object-based algorithms
- **03_MAPS_AND_SETS.md**: Using Map/Set for fast lookups
- **05_JAVASCRIPT_DATA_STRUCTURES_PATTERNS.md**: Advanced algorithm patterns
- **06_MEMORY_MANAGEMENT_DATA_STRUCTURES.md**: Memory considerations for algorithms