# 📚 Performance String Operations

## 📋 Overview

String operations are fundamental to JavaScript applications, but they can become performance bottlenecks when not handled correctly. JavaScript strings are immutable, meaning every modification creates a new string object. This characteristic has significant implications for memory usage and execution speed, especially when processing large amounts of text or performing repeated string manipulations.

This comprehensive guide covers string performance optimization techniques, memory management strategies, efficient concatenation methods, and string builder patterns. We'll explore both theoretical concepts and practical implementations, with code examples that demonstrate real-world performance improvements.

Understanding string performance is crucial for developers building data-intensive applications, text processing pipelines, code generators, or any JavaScript application that manipulates substantial amounts of text. The techniques covered here apply to both browser and Node.js environments.

---

## 🔤 Table of Contents

1. [Understanding String Performance](#understanding-string-performance)
2. [Efficient Concatenation](#efficient-concatenation)
3. [String Builder Patterns](#string-builder-patterns)
4. [Memory Optimization](#memory-optimization)
5. [Search and Replace Optimization](#search-and-replace-optimization)
6. [Profiling and Benchmarking](#profiling-and-benchmarking)
7. [Real-World Optimizations](#real-world-optimizations)
8. [Key Takeaways](#key-takeaways)
9. [Common Pitfalls](#common-pitfalls)

---

## 🧠 Understanding String Performance

### String Immutability and Performance

In JavaScript, strings are immutable. Every operation that appears to modify a string actually creates a new string. This fundamental characteristic has major performance implications.

**Code Example 1: Understanding String Immutability**

```javascript
// file: examples/immutability-impact.js
// Description: Demonstrating how string immutability affects performance

// ❌ Inefficient: Creating many intermediate strings
function inefficientConcat(parts) {
    let result = "";
    for (let i = 0; i < parts.length; i++) {
        result = result + parts[i];  // Creates new string each iteration
    }
    return result;
}

// Each iteration creates a new string:
// "a" + "b" = "ab" (new allocation)
// "ab" + "c" = "abc" (new allocation)
// "abc" + "d" = "abcd" (new allocation)

// ✅ Efficient: Using array and join
function efficientConcat(parts) {
    return parts.join("");
}

// Performance comparison
const testParts = Array.from({ length: 1000 }, (_, i) => `item${i}`);

console.time("Inefficient");
inefficientConcat(testParts);
console.timeEnd("Inefficient");

console.time("Efficient");
efficientConcat(testParts);
console.timeEnd("Efficient");

// Memory allocation comparison
function measureAllocations(parts) {
    let allocations = 0;
    let result = "";
    for (const part of parts) {
        result += part;
        allocations++;
    }
    return allocations;
}

// With 1000 parts, inefficient creates ~1000 string allocations
// Efficient creates 1 array + 1 final string = ~2 allocations
console.log("Approximate allocations:", measureAllocations(testParts.slice(0, 100)));
```

### String Interning and Caching

JavaScript engines often intern short strings for performance, but this optimization has limits.

**Code Example 2: String Interning**

```javascript
// file: examples/string-interning.js
// Description: String interning behavior

// Short string interning
const a = "hello";
const b = "hello";
console.log(a === b); // true (same reference - interned)

// Longer strings typically not interned
const long1 = "This is a much longer string that is unlikely to be interned";
const long2 = "This is a much longer string that is unlikely to be interned";
console.log(long1 === long2); // true (but might be different reference)

// String methods return new strings
const original = "hello";
const modified = original.toUpperCase();
console.log(original === modified); // false (different objects)

// Using String.intern() (not standard JS, but V8 has similar internally)
function internString(str) {
    // Trivial implementation - real interning is engine-level
    return String.prototype.slice.call(str);
}
```

---

## 🔗 Efficient Concatenation

### When to Use Different Methods

**Code Example 3: Concatenation Method Comparison**

```javascript
// file: examples/concatenation-methods.js
// Description: Comparing different string concatenation methods

// Method 1: Plus operator
function plusConcat(items) {
    let result = "";
    for (const item of items) {
        result += item;
    }
    return result;
}

// Method 2: Array.join()
function joinConcat(items) {
    return items.join("");
}

// Method 3: Template literal
function templateConcat(items) {
    return `${items.join("")}`;
}

// Method 4: String.concat()
function concatConcat(items) {
    let result = "";
    for (const item of items) {
        result = result.concat(item);
    }
    return result;
}

// Benchmark
const items = Array.from({ length: 10000 }, (_, i) => `item${i}`);

console.time("Plus operator");
plusConcat(items);
console.timeEnd("Plus operator");

console.time("Array.join");
joinConcat(items);
console.timeEnd("Array.join");

// Results: Array.join() is typically fastest for many items
// Plus operator is competitive for simple cases
// concat() method is generally slowest

// Practical recommendation
function buildQueryString(params) {
    if (!params || Object.keys(params).length === 0) return "";
    
    return Object.entries(params)
        .map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(value)}`)
        .join("&");
}

const params = { name: "John", age: "30", city: "NYC" };
console.log(buildQueryString(params)); // "name=John&age=30&city=NYC"
```

### Template Literals Performance

**Code Example 4: Template Literals vs Concatenation**

```javascript
// file: examples/template-performance.js
// Description: Template literal performance characteristics

// Modern template literals are well-optimized by engines
// But complex templates still create intermediate strings

// Simple case - similar performance
const a = "hello";
const b = "world";
const result1 = a + " " + b;  // Typically fast
const result2 = `${a} ${b}`;  // Also fast

// Complex case with expressions - still creates new strings
function createUserCard(user) {
    // Each expression creates a new string temporarily
    return `<div class="card">
        <h2>${user.name}</h2>
        <p>${user.email}</p>
        <span class="badge">${user.role}</span>
    </div>`;
}

// When to prefer one over the other
const simpleConcatenation = "prefix" + value + "suffix";
const simpleTemplate = `prefix${value}suffix`;

// For complex HTML generation, consider template libraries or
// separating concerns (logic + rendering)
```

---

## 🏗️ String Builder Patterns

### Array-Based String Builder

**Code Example 5: String Builder Implementation**

```javascript
// file: examples/string-builder.js
// Description: Efficient string builder pattern

class StringBuilder {
    constructor() {
        this.parts = [];
        this.length = 0;
    }

    append(str) {
        if (str) {
            this.parts.push(str);
            this.length += str.length;
        }
        return this;  // Enable chaining
    }

    appendLine(str) {
        return this.append(str).append("\n");
    }

    appendFormat(template, ...args) {
        return this.append(this.format(template, args));
    }

    format(template, args) {
        return template.replace(/\{(\d+)\}/g, (match, index) => {
            return args[index] !== undefined ? args[index] : match;
        });
    }

    toString() {
        return this.parts.join("");
    }

    clear() {
        this.parts = [];
        this.length = 0;
        return this;
    }

    isEmpty() {
        return this.parts.length === 0;
    }
}

// Usage
const builder = new StringBuilder();
const output = builder
    .append("<html>")
    .appendLine("<head>")
    .append("  <title>Example</title>")
    .appendLine("</head>")
    .appendLine("<body>")
    .append("  <h1>{0}</h1>").format(["Hello World"])
    .appendLine("</body>")
    .append("</html>")
    .toString();

console.log(output);
```

### Pool-Based String Builder

**Code Example 6: Pool-Based Builder for High Performance**

```javascript
// file: examples/pool-builder.js
// Description: Pool-based string builder for frequent use

class PooledStringBuilder {
    constructor(initialCapacity = 16) {
        this.buffer = new Array(initialCapacity);
        this.length = 0;
        this.partsCount = 0;
    }

    append(str) {
        if (!str) return this;

        // Expand buffer if needed
        if (this.partsCount >= this.buffer.length) {
            this.expandBuffer();
        }

        this.buffer[this.partsCount++] = str;
        this.length += str.length;
        return this;
    }

    appendLine(str) {
        return this.append(str).append("\n");
    }

    expandBuffer() {
        const newBuffer = new Array(this.buffer.length * 2);
        for (let i = 0; i < this.partsCount; i++) {
            newBuffer[i] = this.buffer[i];
        }
        this.buffer = newBuffer;
    }

    toString() {
        if (this.partsCount === 0) return "";
        
        // Use built-in string joining
        let result = "";
        for (let i = 0; i < this.partsCount; i++) {
            result += this.buffer[i];
        }
        return result;
    }

    // Optimized toString using join
    toStringOptimized() {
        if (this.partsCount === 0) return "";
        // Only creates one array copy + final string
        return this.buffer.slice(0, this.partsCount).join("");
    }

    clear() {
        this.partsCount = 0;
        this.length = 0;
        return this;
    }
}

// Pre-allocated version for known sizes
class FixedStringBuilder {
    constructor(expectedLength) {
        this.buffer = new Array(expectedLength);
        this.position = 0;
    }

    append(str) {
        const len = str.length;
        for (let i = 0; i < len; i++) {
            this.buffer[this.position++] = str[i];
        }
        return this;
    }

    toString() {
        return this.buffer.slice(0, this.position).join("");
    }
}
```

### Streaming String Processing

**Code Example 7: Processing Large Text Data**

```javascript
// file: examples/streaming.js
// Description: Processing large text efficiently

// Process large CSV data in chunks
function processLargeCSV(csvContent, chunkSize = 8192) {
    const results = [];
    let currentLine = "";
    let bytesProcessed = 0;

    for (let i = 0; i < csvContent.length; i++) {
        const char = csvContent[i];
        currentLine += char;
        bytesProcessed++;

        if (char === "\n" || bytesProcessed === csvContent.length) {
            const parsed = parseCSVLine(currentLine);
            results.push(parsed);
            currentLine = "";

            if (results.length >= chunkSize) {
                // Yield to prevent blocking (simulated)
                yieldResults(results);
                results.length = 0;
            }
        }
    }

    return results;
}

function parseCSVLine(line) {
    const parts = [];
    let current = "";
    let inQuotes = false;

    for (const char of line) {
        if (char === '"') {
            inQuotes = !inQuotes;
        } else if (char === "," && !inQuotes) {
            parts.push(current.trim());
            current = "";
        } else {
            current += char;
        }
    }
    parts.push(current.trim());
    return parts;
}

function yieldResults(results) {
    // In real implementation, this might be async
    return results;
}

// Line-by-line processing
function processLines(text) {
    const lines = text.split("\n");
    return lines.filter(line => line.trim().length > 0);
}

// Memory-efficient line processing
function* lineGenerator(text) {
    let lineStart = 0;
    
    for (let i = 0; i < text.length; i++) {
        if (text[i] === "\n") {
            yield text.slice(lineStart, i);
            lineStart = i + 1;
        }
    }
    
    if (lineStart < text.length) {
        yield text.slice(lineStart);
    }
}
```

---

## 💾 Memory Optimization

### Avoiding Memory Leaks

**Code Example 8: Memory-Safe String Operations**

```javascript
// file: examples/memory-safety.js
// Description: Preventing memory issues with strings

// ❌ Problem: Accumulating strings in long-running operations
function problematicLog(messages) {
    let log = "";
    for (const msg of messages) {
        log += msg + "\n";  // Each iteration creates new string
    }
    return log;
}

// ✅ Solution: Use array-based approach
function safeLog(messages) {
    const lines = [];
    for (const msg of messages) {
        lines.push(msg);
    }
    return lines.join("\n");
}

// ✅ Even better: Process in chunks/streaming
function streamLog(messages, callback) {
    const buffer = [];
    for (const msg of messages) {
        buffer.push(msg);
        if (buffer.length >= 100) {
            callback(buffer.join("\n") + "\n");
            buffer.length = 0; // Clear without new allocation
        }
    }
    if (buffer.length > 0) {
        callback(buffer.join("\n") + "\n");
    }
}

// Limiting memory for user input
function truncateToMemory(str, maxLength = 10000) {
    if (str.length <= maxLength) return str;
    return str.slice(0, maxLength) + "...";
}

// Clearing string references for GC
function processWithCleanup() {
    let largeString = "x".repeat(1000000);
    // Process...
    largeString = null;  // Allow garbage collection
    // Continue processing...
}
```

### String Caching

**Code Example 9: Caching for Performance**

```javascript
// file: examples/string-caching.js
// Description: Implementing string caching

class StringCache {
    constructor(maxSize = 1000) {
        this.cache = new Map();
        this.maxSize = maxSize;
    }

    get(key) {
        return this.cache.get(key);
    }

    set(key, value) {
        if (this.cache.size >= this.maxSize) {
            // Remove oldest entry
            const firstKey = this.cache.keys().next().value;
            this.cache.delete(firstKey);
        }
        this.cache.set(key, value);
    }

    has(key) {
        return this.cache.has(key);
    }

    clear() {
        this.cache.clear();
    }
}

// Memoization for expensive string operations
function memoizeStringOperation(fn) {
    const cache = new StringCache();
    return function(...args) {
        const key = JSON.stringify(args);
        const cached = cache.get(key);
        if (cached !== undefined) return cached;
        
        const result = fn.apply(this, args);
        cache.set(key, result);
        return result;
    };
}

// Example: Expensive string transformation
const expensiveTransform = memoizeStringOperation((input) => {
    // Simulate expensive operation
    return input
        .toUpperCase()
        .replace(/\s+/g, "_")
        .split("")
        .reverse()
        .join("");
});

console.log(expensiveTransform("hello world")); // "DLROW_OLLEH"
console.log(expensiveTransform("hello world")); // Cached result
```

### WeakRef for String Caching

**Code Example 10: Using WeakRef for Memory-Efficient Caching**

```javascript
// file: examples/weakref-cache.js
// Description: WeakRef-based caching

class WeakStringCache {
    constructor() {
        this.cache = new Map();
    }

    get(key) {
        const entry = this.cache.get(key);
        if (!entry) return null;
        
        // Check if value has been garbage collected
        const value = entry.deref();
        if (value === undefined) {
            this.cache.delete(key);
            return null;
        }
        return value;
    }

    set(key, value) {
        this.cache.set(key, new WeakRef(value));
    }

    clean() {
        for (const [key, ref] of this.cache) {
            if (ref.deref() === undefined) {
                this.cache.delete(key);
            }
        }
    }
}

// Usage with string interning
function internString(str) {
    if (!internString.cache) {
        internString.cache = new WeakStringCache();
    }
    
    const cached = internString.cache.get(str);
    if (cached) return cached;
    
    internString.cache.set(str, str);
    return str;
}

const s1 = internString("hello");
const s2 = internString("hello");
console.log(s1 === s2); // true (same reference)
```

---

## 🔍 Search and Replace Optimization

### Efficient Pattern Matching

**Code Example 11: Optimized Search Operations**

```javascript
// file: examples/search-optimization.js
// Description: Optimizing string search operations

// Simple search for small strings
function naiveSearch(haystack, needle) {
    for (let i = 0; i <= haystack.length - needle.length; i++) {
        let found = true;
        for (let j = 0; j < needle.length; j++) {
            if (haystack[i + j] !== needle[j]) {
                found = false;
                break;
            }
        }
        if (found) return i;
    }
    return -1;
}

// Built-in methods are generally faster (implemented in native code)
function builtinSearch(haystack, needle) {
    return haystack.indexOf(needle);
}

// Using indexOf with start position for multiple occurrences
function findAllOccurrences(haystack, needle) {
    const positions = [];
    let pos = haystack.indexOf(needle);
    
    while (pos !== -1) {
        positions.push(pos);
        pos = haystack.indexOf(needle, pos + 1);
    }
    
    return positions;
}

// Efficient replacement
function replaceAll(str, search, replace) {
    // Simple approach using split/join (works for string search)
    return str.split(search).join(replace);
    
    // Or with regex (use carefully for special characters)
    // return str.replace(new RegExp(escapeRegExp(search), 'g'), replace);
}

function escapeRegExp(string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

// Benchmark different approaches
function benchmarkSearch(text, patterns) {
    const results = {};
    
    for (const pattern of patterns) {
        const start = performance.now();
        const index = text.indexOf(pattern);
        const time = performance.now() - start;
        
        results[pattern] = { index, time };
    }
    
    return results;
}
```

### Regex Optimization

**Code Example 12: Optimizing Regular Expressions**

```javascript
// file: examples/regex-optimization.js
// Description: Optimizing regex performance

// ❌ Inefficient: Creating regex in loop
function inefficientRegex(strings) {
    return strings.map(s => s.match(/pattern/gi));
}

// ✅ Efficient: Create regex once
const pattern = /pattern/gi;
function efficientRegex(strings) {
    return strings.map(s => s.match(pattern));
}

// Pre-compile patterns
const cachedPatterns = new Map();

function getPattern(regexStr, flags = "") {
    const key = `${regexStr}|${flags}`;
    
    if (!cachedPatterns.has(key)) {
        cachedPatterns.set(key, new RegExp(regexStr, flags));
    }
    
    return cachedPatterns.get(key);
}

// Optimized: Use specific character classes
// ❌ Slow: /[a-zA-Z0-9]/
// ✅ Fast: /\w/

// Avoid catastrophic backtracking
// ❌ Dangerous: /(a+)+b/ on long string
// ✅ Safe: /a+b/

// Use anchors when possible
// ❌ Scans entire string: /\d+/
// ✅ Anchored: /^\d+$/ - fails fast on non-matches

// Compiled regex for hot paths
function createCompiledSearch(pattern) {
    return new RegExp(pattern, "g");
}

// Lazy quantifiers for early termination
const greedy = /".*"/.exec('"hello" world "foo"');
// Result: '"hello" world "foo"' (matches too much)

const lazy = /".*?"/.exec('"hello" world "foo"');
// Result: '"hello"' (stops at first quote)

// Practical optimization: limit search scope
function optimizedSearch(text, pattern, maxResults = 100) {
    const regex = new RegExp(pattern, "g");
    const results = [];
    let match;
    
    while ((match = regex.exec(text)) && results.length < maxResults) {
        results.push(match[0]);
    }
    
    return results;
}
```

---

## 📊 Profiling and Benchmarking

### Simple Benchmarking

**Code Example 13: Benchmarking String Operations**

```javascript
// file: examples/benchmarking.js
// Description: Benchmarking string operations

function benchmark(fn, iterations = 1000) {
    const start = performance.now();
    for (let i = 0; i < iterations; i++) {
        fn();
    }
    const end = performance.now();
    return (end - start) / iterations;
}

// Compare concatenation methods
const testString = "test";

console.time("plus");
for (let i = 0; i < 10000; i++) {
    const result = testString + "suffix";
}
console.timeEnd("plus");

console.time("concat");
for (let i = 0; i < 10000; i++) {
    const result = testString.concat("suffix");
}
console.timeEnd("concat");

console.time("template");
for (let i = 0; i < 10000; i++) {
    const result = `${testString}suffix`;
}
console.timeEnd("template");

// More accurate benchmarking with warmup
function runBenchmark(fn, iterations, warmup) {
    // Warmup runs
    for (let i = 0; i < warmup; i++) fn();
    
    // Actual benchmark
    const times = [];
    for (let i = 0; i < iterations; i++) {
        const start = performance.now();
        fn();
        times.push(performance.now() - start);
    }
    
    // Calculate statistics
    const avg = times.reduce((a, b) => a + b, 0) / times.length;
    const min = Math.min(...times);
    const max = Math.max(...times);
    
    return { avg, min, max };
}

const results = runBenchmark(
    () => "test".toUpperCase(),
    1000,
    100
);

console.log("Avg:", results.avg.toFixed(4), "ms");
console.log("Min:", results.min.toFixed(4), "ms");
console.log("Max:", results.max.toFixed(4), "ms");
```

### Performance Testing Framework

**Code Example 14: Test Framework for String Operations**

```javascript
// file: examples/test-framework.js
// Description: Testing framework for string performance

class PerformanceTest {
    constructor(name) {
        this.name = name;
        this.results = [];
    }

    run(fn, iterations = 1000) {
        // Force garbage collection if available
        if (global.gc) global.gc();
        
        const start = performance.now();
        for (let i = 0; i < iterations; i++) {
            fn();
        }
        const end = performance.now();
        
        const time = (end - start) / iterations;
        this.results.push({ iterations, time });
        return time;
    }

    getAverageTime() {
        const sum = this.results.reduce((a, r) => a + r.time, 0);
        return sum / this.results.length;
    }

    report() {
        console.log(`\n=== ${this.name} ===`);
        for (const result of this.results) {
            console.log(`  ${result.iterations} iterations: ${result.time.toFixed(4)}ms`);
        }
        console.log(`  Average: ${this.getAverageTime().toFixed(4)}ms`);
    }
}

// Compare methods
const concatTest = new PerformanceTest("String Concatenation");

// Test 1: Plus operator
concatTest.run(() => {
    let s = "";
    for (let i = 0; i < 100; i++) s += "x";
}, 100);

// Test 2: Array join
concatTest.run(() => {
    const arr = [];
    for (let i = 0; i < 100; i++) arr.push("x");
    arr.join("");
}, 100);

concatTest.report();
```

---

## 🚀 Real-World Optimizations

### CSV Generation

**Code Example 15: Efficient CSV Generation**

```javascript
// file: examples/csv-generation.js
// Description: Optimized CSV generation

class CSVGenerator {
    constructor() {
        this.rows = [];
    }

    addRow(row) {
        this.rows.push(row.map(cell => this.escapeCell(cell)));
        return this;
    }

    escapeCell(cell) {
        if (cell === null || cell === undefined) return "";
        
        const str = String(cell);
        
        // Check if escaping needed
        if (str.includes(",") || str.includes('"') || str.includes("\n")) {
            return `"${str.replace(/"/g, '""')}"`;
        }
        
        return str;
    }

    toString() {
        return this.rows.map(row => row.join(",")).join("\n");
    }

    // Optimized version avoiding array allocations
    toStringOptimized() {
        let result = "";
        for (let i = 0; i < this.rows.length; i++) {
            const row = this.rows[i];
            for (let j = 0; j < row.length; j++) {
                if (j > 0) result += ",";
                result += row[j];
            }
            if (i < this.rows.length - 1) result += "\n";
        }
        return result;
    }
}

// Usage
const csv = new CSVGenerator();
csv.addRow(["Name", "Email", "Age"]);
csv.addRow(["John Doe", "john@example.com", "30"]);
csv.addRow(["Jane Smith", "jane@test.com", "25"]);
csv.addRow(["Test, Inc", "test@company.com", "40"]);

console.log(csv.toString());
```

### JSON Stringification

**Code Example 16: Optimized JSON Processing**

```javascript
// file: examples/json-optimization.js
// Description: Optimizing JSON string operations

// Pre-serialize known objects
const cachedJSON = new Map();

function cachedStringify(obj) {
    const key = JSON.stringify(obj);
    
    if (!cachedJSON.has(key)) {
        cachedJSON.set(key, key);
    }
    
    return cachedJSON.get(key);
}

// Stream large JSON objects
function* streamJSONParse(jsonString) {
    let depth = 0;
    let current = "";
    let inString = false;
    
    for (let i = 0; i < jsonString.length; i++) {
        const char = jsonString[i];
        
        if (char === '"' && jsonString[i-1] !== '\\') {
            inString = !inString;
        }
        
        if (!inString) {
            if (char === '{' || char === '[') {
                depth++;
            } else if (char === '}' || char === ']') {
                depth--;
                if (depth === 0) {
                    yield JSON.parse(current + char);
                    current = "";
                    continue;
                }
            }
        }
        
        current += char;
    }
}

// Efficient large object merging
function mergeObjects(target, source) {
    const result = { ...target };
    
    for (const key in source) {
        if (source.hasOwnProperty(key)) {
            if (typeof source[key] === "object" && source[key] !== null) {
                result[key] = mergeObjects(result[key] || {}, source[key]);
            } else {
                result[key] = source[key];
            }
        }
    }
    
    return result;
}

// Compact JSON for storage (remove whitespace)
function compactJSON(obj) {
    return JSON.stringify(obj);
}

function prettyJSON(obj) {
    return JSON.stringify(obj, null, 2);
}
```

### HTML Generation

**Code Example 17: Optimized HTML Generation**

```javascript
// file: examples/html-generation.js
// Description: Efficient HTML string building

class HTMLBuilder {
    constructor() {
        this.stack = [];
        this.current = "";
    }

    tag(name, content = "", attributes = {}) {
        this.openTag(name, attributes);
        this.current += content;
        this.closeTag(name);
        return this;
    }

    openTag(name, attributes = {}) {
        const attrs = Object.entries(attributes)
            .filter(([_, v]) => v !== null && v !== false)
            .map(([k, v]) => v === true ? k : `${k}="${this.escapeAttr(v)}"`)
            .join(" ");
        
        this.current += attrs ? `<${name} ${attrs}>` : `<${name}>`;
        return this;
    }

    closeTag(name) {
        this.current += `</${name}>`;
        return this;
    }

    text(content) {
        this.current += this.escapeText(content);
        return this;
    }

    raw(content) {
        this.current += content;
        return this;
    }

    escapeText(text) {
        return String(text)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;");
    }

    escapeAttr(text) {
        return String(text)
            .replace(/&/g, "&amp;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#39;");
    }

    toString() {
        return this.current;
    }
}

// Usage
const html = new HTMLBuilder()
    .openTag("div", { class: "container" })
    .tag("h1", "Welcome", { id: "title" })
    .tag("p", "This is a paragraph.")
    .openTag("ul", { class: "items" })
    .tag("li", "Item 1")
    .tag("li", "Item 2")
    .tag("li", "Item 3")
    .closeTag("ul")
    .closeTag("div")
    .toString();

console.log(html);
```

---

## 📊 Key Takeaways

1. **String Immutability**: Every string modification creates a new string object. Design operations to minimize intermediate string creation.

2. **Array.join() for Multiple Strings**: Use `join()` instead of repeated concatenation for multiple strings.

3. **StringBuilder Pattern**: For complex building operations, use a string builder pattern to collect parts before final concatenation.

4. **Pre-compile Regex**: Create regex patterns once and reuse them to avoid repeated compilation overhead.

5. **Use Built-in Methods**: JavaScript's built-in string methods are implemented in native code and are typically faster than custom implementations.

6. **Cache Expensive Operations**: Use memoization for repeated string transformations with the same inputs.

7. **Profile Before Optimizing**: Use performance profiling to identify actual bottlenecks rather than guessing.

---

## ⚠️ Common Pitfalls

1. **Premature Optimization**: Don't optimize until you've identified actual performance issues through profiling.

2. **String Concatenation in Loops**: Avoid `str += part` in tight loops; use array.join() or StringBuilder instead.

3. **Creating Regex in Loops**: Never create a new RegExp inside a loop that runs frequently.

4. **Ignoring Memory**: Large string operations can cause memory pressure; consider streaming or chunking for large data.

5. **Not Considering Unicode**: Optimizations for ASCII may fail with Unicode; test with international text.

6. **Assuming Plus is Slower**: For simple 2-3 string concatenations, the plus operator is often fast enough.

7. **Not Using Native Methods**: Custom implementations are rarely faster than native methods.

---

## 🔗 Related Files

- **[String Methods Comprehensive](./01_STRING_METHODS_COMPREHENSIVE.md)** - Basic string manipulation
- **[Regular Expressions Master](./02_REGULAR_EXPRESSIONS_MASTER.md)** - Pattern optimization
- **[Template Literals](./03_STRING_TEMPLATES_AND_INTERPOLATION.md)** - Template performance
- **[Performance Encyclopedia](../65_PERFORMANCE_ENCYCLOPEDIA.md)** - Broader performance topics

---

## 📚 Further Reading

- [MDN: String](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String)
- [V8 Blog: String Operations](https://v8.dev/blog)
- [JSPerf](https://jsperf.com/) - Performance testing