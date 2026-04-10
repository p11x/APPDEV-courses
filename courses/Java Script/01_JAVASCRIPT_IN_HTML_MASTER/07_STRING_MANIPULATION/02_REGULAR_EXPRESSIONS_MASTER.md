# 📚 Regular Expressions Mastery

## 📋 Overview

Regular expressions (regex) are patterns used to match character combinations in strings. They are an incredibly powerful tool for text processing, validation, and extraction. Mastery of regular expressions is essential for any JavaScript developer working with text data.

This guide covers regular expressions comprehensively, from basic pattern matching to advanced techniques like lookarounds and performance optimization. We'll explore both the JavaScript regex API and best practices for writing efficient patterns.

---

## 🔤 Table of Contents

1. [Creating Regular Expressions](#creating-regular-expressions)
2. [Basic Patterns](#basic-patterns)
3. [Character Classes](#character-classes)
4. [Quantifiers](#quantifiers)
5. [Anchors](#anchors)
6. [Groups and Capturing](#groups-and-capturing)
7. [Flags](#flags)
8. [Lookarounds](#lookarounds)
9. [Common Patterns](#common-patterns)
10. [Performance Considerations](#performance-considerations)
11. [Key Takeaways](#key-takeaways)
12. [Common Pitfalls](#common-pitfalls)

---

## 🏗️ Creating Regular Expressions

There are two ways to create a regular expression in JavaScript:

**1. Literal Notation (Preferred)**
```javascript
const pattern = /abc/;
```

**2. Constructor Function**
```javascript
const pattern = new RegExp('abc');
```

### When to Use Each

The literal notation is generally preferred because:
- Simpler syntax
- Parsed at compile time (slightly faster)
- More readable

The constructor is useful when:
- The pattern needs to be constructed dynamically
- You're accepting a pattern from user input
- Building patterns from variables

**Code Example 1: Creating Regex Patterns**

```javascript
// file: examples/creating-regex.js
// Description: Creating regular expressions in JavaScript

// Literal notation - most common approach
const exactMatch = /hello/;
const numberPattern = /\d+/;
const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

// Constructor - for dynamic patterns
const searchTerm = "javascript";
const dynamicPattern = new RegExp(searchTerm, "i");

// Escaping special characters in dynamic patterns
function escapeRegExp(string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

const userInput = "file.txt";
const safePattern = new RegExp(escapeRegExp(userInput));
console.log(safePattern); // /file\.txt/

// Validating if something is a regex
function isRegex(value) {
    return value instanceof RegExp;
}

console.log(isRegex(/abc/));           // true
console.log(isRegex(new RegExp('abc'))); // true
console.log(isRegex("abc"));            // false
```

---

## 🔍 Basic Patterns

Regular expressions use special characters to define matching rules. Let's start with the most basic patterns.

### Literal Characters

The simplest regex matches exact character sequences:

```javascript
/hello/.test("hello world");  // true
/hello/.test("world");       // false
```

### Metacharacters

Special characters with specific meanings:

| Metacharacter | Meaning |
|--------------|---------|
| `.` | Any character except newline |
| `\` | Escape character |
| `\|` | Alternation (or) |

**Code Example 2: Basic Pattern Matching**

```javascript
// file: examples/basic-patterns.js
// Description: Basic regex pattern matching

// Simple literal match
console.log(/hello/.test("hello world")); // true

// Dot matches any single character
console.log(/h.t/.test("hot"));   // true
console.log(/h.t/.test("heat"));  // false (only one dot)

// Escaping special characters
console.log(/file\.txt/.test("file.txt")); // true
console.log(/file.txt/.test("file txt"));  // true (! dot is special)

// Alternation with |
console.log(/cat|dog/.test("cat"));     // true
console.log(/cat|dog/.test("dog"));    // true
console.log(/cat|dog/.test("bird"));   // false

// Case-insensitive with inline flag
console.log(/hello/i.test("HELLO"));   // true

// Word boundary with metacharacters
console.log(/\bword\b/.test("a word here")); // true
console.log(/\bword\b/.test("sword"));     // false
```

---

## 📊 Character Classes

Character classes define sets of characters to match.

### Basic Character Classes

| Class | Matches |
|-------|---------|
| `[abc]` | Any of a, b, or c |
| `[^abc]` | Not a, b, or c |
| `[a-z]` | Range (a to z) |
| `\d` | Digit [0-9] |
| `\D` | Non-digit |
| `\w` | Word character [a-zA-Z0-9_] |
| `\W` | Non-word character |
| `\s` | Whitespace |
| `\S` | Non-whitespace |
| `.` | Any character (except newline) |

### Negated Classes

Use `^` inside brackets to negate:

```javascript
/[^0-9]/.test("abc");  // true (contains non-digit)
```

**Code Example 3: Character Classes in Practice**

```javascript
// file: examples/character-classes.js
// Description: Using character classes for matching

// Match any digit
console.log(/\d/.test("abc123"));     // true
console.log(/\d/.test("abcdef"));   // false

// Match non-digit
console.log(/\D/.test("123"));      // false
console.log(/\D/.test("abc"));       // true

// Match word character
console.log(/\w+/.test("hello_123")); // true
console.log(/\w+/.test("hello!"));     // false

// Match specific character set
console.log(/[aeiou]/.test("hello")); // true
console.log(/[aeiou]/.test("xyz"));  // false

// Negated character class
console.log(/[^0-9]/.test("abc"));  // true
console.log(/[^0-9]/.test("123"));   // false

// Character ranges
console.log(/[a-z]/.test("a"));     // true
console.log(/[a-z]/.test("Z"));     // false (case-sensitive)
console.log(/[A-Za-z]/.test("Z"));   // true (case insensitive)

// Complex character class
const passwordPattern = /^[A-Za-z\d@$!%*?&]{8,}$/;
console.log(passwordPattern.test("Password1!")); // true
console.log(passwordPattern.test("short"));     // false
```

---

## 🔢 Quantifiers

Quantifiers specify how many times a pattern should match.

### Quantifier Types

| Quantifier | Meaning |
|------------|---------|
| `*` | 0 or more |
| `+` | 1 or more |
| `?` | 0 or 1 |
| `{n}` | Exactly n |
| `{n,}` | n or more |
| `{n,m}` | Between n and m |
| `*?`, `+?`, `??` | Lazy versions (matches as few as possible) |

**Code Example 4: Quantifiers for Flexible Matching**

```javascript
// file: examples/quantifiers.js
// Description: Using quantifiers for pattern repetition

// Zero or more (*)
console.log(/ab*c/.test("ac"));    // true
console.log(/ab*c/.test("abc"));  // true
console.log(/ab*c/.test("abbbc"));// true

// One or more (+)
console.log(/ab+c/.test("ac"));    // false
console.log(/ab+c/.test("abc"));    // true

// Optional (?)
console.log(/colou?r/.test("color"));  // true
console.log(/colou?r/.test("colour")); // true

// Exact count {n}
console.log(/\d{3}/.test("123"));    // true
console.log(/\d{3}/.test("12"));     // false

// Range {n,m}
console.log(/\d{2,4}/.test("12"));     // true
console.log(/\d{2,4}/.test("1234"));   // true
console.log(/\d{2,4}/.test("12345")); // false

// At least {n,}
console.log(/\d{2,}/.test("123"));  // true
console.log(/\d{2,}/.test("1"));   // false

// Greedy vs Lazy
console.log(/".+"/.test('"hello" world"'));
// Greedy matches as much as possible: "hello" world"

// Lazy (? makes it match as few as possible)
console.log(/".*?"/.test('"hello" world"'));
// Lazy matches minimally: "hello"

// Practical: match anything in quotes
function extractQuoted(text) {
    const matches = text.match(/"[^"]*"/g);
    return matches ? matches.map(m => m.slice(1, -1)) : [];
}

console.log(extractQuoted('He said "hello" and "goodbye"'));
// ["hello", "goodbye"]
```

---

## ⚓ Anchors

Anchors match positions in the string, not characters.

### Anchor Types

| Anchor | Matches |
|--------|---------|
| `^` | Start of string |
| `$` | End of string |
| `\b` | Word boundary |
| `\B` | Non-word boundary |

**Code Example 5: Using Anchors**

```javascript
// file: examples/anchors.js
// Description: Using anchors for position matching

// Start of string (^)
console.log(/^hello/.test("hello world")); // true
console.log(/^hello/.test("say hello"));   // false

// End of string ($)
console.log(/world$/.test("hello world")); // true
console.log(/world$/.test("world hello"));   // false

// Combined (exact match)
console.log(/^hello world$/.test("hello world")); // true

// Word boundary
console.log(/\bword\b/.test("a word"));      // true
console.log(/\bword\b/.test("sword"));       // false
console.log(/\bword\b/.test("wordy"));       // false

// Non-word boundary
console.log(/\Bword/.test("sword"));       // true
console.log(/\Bword/.test("a word"));      // false

// Practical: validate entire input
function validateUsername(username) {
    const pattern = /^[a-zA-Z][a-zA-Z0-9_]{2,19}$/;
    return pattern.test(username);
}

console.log(validateUsername("john_doe"));   // true
console.log(validateUsername("1john"));       // false (can't start with number)
console.log(validateUsername("ab"));         // false (too short)

// Validate email format
function validateEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

console.log(validateEmail("test@example.com"));    // true
console.log(validateEmail("invalid@"));               // false
```

---

## 👥 Groups and Capturing

Groups allow you to treat multiple characters as a single unit and capture matched portions.

### Group Types

| Group Type | Syntax | Description |
|------------|--------|-------------|
| Capturing | `(abc)` | Groups and captures |
| Non-capturing | `(?:abc)` | Groups without capturing |
| Named | `(?<name>abc)` | ES2018+ named capture |
| Backreference | `\1`, `\2` | References captured group |

**Code Example 6: Working with Groups**

```javascript
// file: examples/groups.js
// Description: Using groups for complex patterns

// Capturing groups
const match = "2024-01-15".match(/(\d{4})-(\d{2})-(\d{2})/);
console.log(match[0]); // "2024-01-15" (full match)
console.log(match[1]); // "2024" (first group)
console.log(match[2]); // "01" (second group)
console.log(match[3]); // "15" (third group)
console.log(match.groups); // undefined (not named)

// Named capturing groups (ES2018+)
const namedMatch = "2024-01-15".match(/(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})/);
console.log(namedMatch.groups.year);   // "2024"
console.log(namedMatch.groups.month); // "01"
console.log(namedMatch.groups.day);   // "15"

// Non-capturing groups (?:)
const ncMatch = "hello".match(/(?:he)llo/);
console.log(ncMatch[1]); // undefined (not captured)

// Backreferences - match repeated characters
const repeated = /(\w)\1/.test("aa");   // true
const notRepeated = /(\w)\1/.test("ab"); // false

// Match quotes
const quotePattern = /(["'])(.*?)\1/g;
const quoted = 'He said "hello"';
const quoteMatch = quoted.match(quotePattern);
console.log(quoteMatch); // ["\"hello\""]

// Replace with captured group
const text = "2024-01-15";
const replaced = text.replace(/(\d{4})-(\d{2})-(\d{2})/, "$3/$2/$1");
console.log(replaced); // "15/01/2024"

// Named backreference (modern)
const replaced2 = text.replace(/(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})/, "$<day>/$<month>/$<year>");
console.log(replaced2); // "15/01/2024"
```

---

## 🚩 Flags

Flags modify how the pattern is interpreted. JavaScript supports several flags.

### Available Flags

| Flag | Name | Description |
|------|------|-------------|
| `g` | Global | Match all occurrences |
| `i` | Case Insensitive | Ignore case |
| `m` | Multiline | ^ and $ match line boundaries |
| `s` | Dotall | . matches newlines |
| `u` | Unicode | Enable Unicode matching |
| `v` | Unicode Set | ES2024+ Unicode properties |

### Combined Flags

```javascript
const pattern = /pattern/gi;  // global + case-insensitive
```

**Code Example 7: Using Flags Effectively**

```javascript
// file: examples/flags.js
// Description: Using regex flags

// Global flag (g) - find all matches
let text = "one two one three";
console.log(text.match(/one/g)); // ["one", "one"]

// Case insensitive (i)
console.log(/HELLO/i.test("hello")); // true

// Multiline (m) - ^ and $ match line boundaries
const multiline = "line1\nline2\nline1";
console.log(multiline.match(/^line1$/gm));
// ["line1", "line1"]

// Dotall (s) - . matches newlines
console.log(/foo.bar/s.test("foo\nbar")); // true
console.log(/foo.bar/.test("foo\nbar")); // false

// Unicode (u) - proper Unicode handling
// Enables proper surrogate pair handling
const unicodePattern = /^.+$/u;

// Using flag v (ES2024+) for Unicode properties
if (typeof new RegExp('', 'v') !== 'undefined') {
    // Match emoji
    const emojiPattern = /\p{RGI_Emoji}/gu;
    console.log(emojiPattern.test("😀")); // true
}

// Combining flags
const uriPattern = /^[a-z][a-z0-9]*$/gi;
console.log(uriPattern.test("Hello123")); // true
console.log(uriPattern.test("123hello")); // false
```

---

## 👀 Lookarounds

Lookarounds are advanced patterns that match based on what follows or precedes, without including that context in the match.

### Lookaround Types

| Lookaround | Type | Matches |
|------------|------|---------|
| `(?=...)` | Positive lookahead | If followed by pattern |
| `(?!...)` | Negative lookahead | If NOT followed by pattern |
| `(?<=...)` | Positive lookbehind | If preceded by pattern |
| `(?<!...)` | Negative lookbehind | If NOT preceded by pattern |

**Important Note**: JavaScript added lookbehind support in ES2018. Older browsers may not support it.

**Code Example 8: Lookarounds Advanced**

```javascript
// file: examples/lookarounds.js
// Description: Using lookarounds for context-aware matching

// Positive lookahead (?=)
const withNumber = "abc123def";
console.log(withNumber.match(/\d+(?=def)/)); // ["123"] - only matches digits before "def"

// Negative lookahead (?!)
console.log(withNumber.match(/\d+(?!abc)/)); // ["123"] - matches digits not after something else

// Extract numbers NOT preceded by $
const prices = "$100 200 $50";
const nonDollarPrices = prices.match(/(?<!\$)\d+/g);
console.log(nonDollarPrices); // ["200", "50"]

// Match word after "password:"
const config = "username: john\npassword: secret123\napi: key456";
const afterPassword = config.match(/(?<=password: )\w+/);
console.log(afterPassword); // ["secret123"]

// Negative lookbehind
const lines = "price: $100\ntotal: 200\ndiscount: $50";
const nonDollar = lines.match(/(?<!\$)\d+/g);
console.log(nonDollar); // ["200"]

// Practical: password validation
function validatePassword(password) {
    // At least 8 chars, has uppercase, has lowercase, has digit, has special
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(password);
}

console.log(validatePassword("Password1!")); // true
console.log(validatePassword("short"));      // false
console.log(validatePassword("alllower1!")); // false
console.log(validatePassword("ALLUPPER1!"));  // false

// Valid US phone numbers (not preceded by country code)
const phones = "+1-555-1234\n555-1234\n(555) 1234";
const validPhone = phones.match(/(?<!\+)\d{3}[-.)]\d{3}[-.)]\d{4}/g);
console.log(validPhone); // ["555-1234", "(555) 1234"]
```

---

## 📝 Common Patterns

Here are commonly needed patterns for real-world use cases.

**Code Example 9: Common Regex Patterns**

```javascript
// file: examples/common-patterns.js
// Description: Useful regex patterns for common tasks

// Email validation
const email = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
console.log(email.test("user@example.com")); // true
console.log(email.test("invalid"));           // false

// URL validation
const url = /^(https?:\/\/)?([\da-z.-]+)\.([a-z.]{2,6})([\/\w .-]*)*\/?$/;
console.log(url.test("https://example.com"));  // true
console.log(url.test("example.com"));         // true

// US phone number
const phone = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
console.log(phone.test("555-123-4567"));   // true
console.log(phone.test("(555) 123 4567"));  // true

// Date (MM/DD/YYYY)
const date = /^(0[1-9]|1[0-2])\/(0[1-9]|[12]\d|3[01])\/(\d{4})$/;
console.log(date.test("01/15/2024"));    // true

// IP address (IPv4)
const ipv4 = /^(?:(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d\d?)$/;
console.log(ipv4.test("192.168.1.1"));   // true

// Hex color
const hexColor = /^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$/;
console.log(hexColor.test("#ff0000")); // true
console.log(hexColor.test("f00"));     // true

// Username (alphanumeric with underscore)
const username = /^[a-zA-Z][a-zA-Z0-9_]{2,19}$/;
console.log(username.test("john_doe")); // true

// Strong password (has upper, lower, digit, special, 8+ chars)
const strongPassword = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
console.log(strongPassword.test("Passw0rd!")); // true
```

**Code Example 10: Practical Applications**

```javascript
// file: examples/practical-regex.js
// Description: Real-world regex applications

// Highlight search terms in text
function highlightMatches(text, searchTerm) {
    const regex = new RegExp(`(${searchTerm})`, 'gi');
    return text.replace(regex, '<mark>$1</mark>');
}

console.log(highlightMatches("Hello World", "world"));
// "Hello <mark>World</mark>"

// Extract all URLs from text
function extractUrls(text) {
    const urlPattern = /https?:\/\/[^\s<>"']+/g;
    return text.match(urlPattern) || [];
}

const text = "Visit https://example.com or http://test.org today!";
console.log(extractUrls(text)); // ["https://example.com", "http://test.org"]

// Slugify a string
function slugify(text) {
    return text
        .toLowerCase()
        .replace(/[^\w\s-]/g, '')  // Remove non-word chars
        .replace(/\s+/g, '-')      // Replace spaces with dashes
        .replace(/-+/g, '-')      // Remove duplicate dashes
        .replace(/^-|-$/g, '');   // Remove leading/trailing dashes
}

console.log(slugify("Hello World! #test")); // "hello-world-test"

// Parse log entries
const logLine = "2024-01-15 10:30:45 [ERROR] Connection failed";
const parseLog = /^(\d{4}-\d{2}-\d{2}) (\d{2}:\d{2}:\d{2}) \[(\w+)\] (.*)$/;
const parsed = logLine.match(parseLog);
console.log({
    date: parsed[1],
    time: parsed[2],
    level: parsed[3],
    message: parsed[4]
});
// { date: "2024-01-15", time: "10:30:45", level: "ERROR", message: "Connection failed" }

// Validate credit card format (not actual cards)
const creditCard = /^\d{4}[- ]?\d{4}[- ]?\d{4}[- ]?\d{4}$/;
console.log(creditCard.test("1234 5678 9012 3456")); // true
```

---

## ⚡ Performance Considerations

Regular expressions can be slow for certain patterns. Here are tips for optimization.

### Common Performance Issues

| Issue | Problem | Solution |
|-------|---------|----------|
| Catastrophic backtracking | Nested quantifiers | Simplify pattern |
| Greedy matching | Matches too much | Use lazy quantifiers |
| Repeated alternation | Check same patterns multiple times | Use character classes |
| No anchors | Scans entire string | Use anchors |

**Code Example 11: Performance Optimization**

```javascript
// file: examples/performance.js
// Description: Optimizing regex performance

// ❌ Problem: Catastrophic backtracking
// /^(a+)+$/ on "aaaaaaaaaaaaaaaaaaaaaaX" will be extremely slow

// ✅ Fixed: Use proper anchoring and simpler patterns
const safePattern = /^a{1,100}$/;

// ❌ Problem: Greedy quantifier
const input = 'first "quoted" second "another"';
const greedyMatch = input.match(/"[^"]*"/);
console.log(greedyMatch[0]); // "quoted" second "another" - too much!

// ✅ Fixed: Lazy quantifier
const lazyMatch = input.match(/"[^"]*?"/);
console.log(lazyMatch[0]); // "quoted"

// ❌ Problem: No anchoring (scans entire string)
const hasDigitAnywhere = /\d+/;  // Will find digit anywhere

// ✅ Fixed: Anchor if possible
const startsWithDigit = /^\d+/;  // Only checks start

// Performance: cache compiled regex
const cachedPattern = /pattern/g;  // Create once, reuse

// Test performance
const largeText = "a".repeat(10000) + "X";

console.time("with anchor");
/a+$/.test(largeText);
console.timeEnd("with anchor");

console.time("without anchor");
/a+/.test(largeText);
console.timeEnd("without anchor");

// Use match with limit if only need first N matches
function getFirstN(text, pattern, n) {
    const regex = new RegExp(pattern, 'g');
    const results = [];
    let match;
    let count = 0;
    while ((match = regex.exec(text)) && count < n) {
        results.push(match[0]);
        count++;
    }
    return results;
}
```

---

## 🎯 Key Takeaways

1. **Use Literal Notation**: Prefer `/pattern/` over `new RegExp()` for better readability and slight performance gain.

2. **Anchor Your Patterns**: Use `^` and `$` when matching entire strings to avoid partial matches.

3. **Careful with Quantifiers**: Prefer lazy quantifiers (`*?`, `+?`) when appropriate to avoid greedy matching issues.

4. **Groups for Capture**: Use capturing groups to extract specific parts of matches.

5. **Lookarounds**: Use lookarounds to match based on context without including context in the match.

6. **Flag Appropriately**: Use the right flags (`g`, `i`, `m`, `s`, `u`, `v`) for your use case.

7. **Performance**: Avoid nested quantifiers that can cause catastrophic backtracking. Test patterns against large inputs.

---

## ⚠️ Common Pitfalls

1. **Forgetting Global Flag**: Methods return first match only without `g` flag.

2. **Greedy vs Lazy**: Default quantifiers are greedy and may match too much.

3. **Not Escaping Special Characters**: When building patterns from user input, escape regex metacharacters.

4. **Dot Doesn't Match Newline**: Use `s` flag or `[\s\S]` to match newlines.

5. **Case Sensitivity**: Remember to add `i` flag for case-insensitive matching.

6. **Lookbehind Support**: Not supported in older browsers; check your target environment.

7. **Backreference References**: Remember `\1` references the first captured group.

---

## 🔗 Related Files

- **[String Methods Comprehensive](./01_STRING_METHODS_COMPREHENSIVE.md)** - String manipulation with regex
- **[String Validation and Sanitization](./04_STRING_VALIDATION_AND_SANITIZATION.md)** - Input validation
- **[String Templates and Interpolation](./03_STRING_TEMPLATES_AND_INTERPOLATION.md)** - Template literal security

---

## 📚 Further Reading

- [MDN: Regular Expressions](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Regular_Expressions)
- [RegexLearn](https://regexlearn.com/)
- [Regular Expressions info](https://www.regular-expressions.info/)