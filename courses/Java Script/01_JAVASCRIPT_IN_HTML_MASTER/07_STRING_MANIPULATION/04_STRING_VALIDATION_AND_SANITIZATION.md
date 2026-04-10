# 📚 String Validation and Sanitization

## 📋 Overview

String validation and sanitization are critical security practices in JavaScript applications. Validation ensures that user input meets expected formats and constraints, while sanitization transforms potentially dangerous input into safe forms. These practices are essential for preventing security vulnerabilities like XSS attacks, SQL injection, and data corruption.

This comprehensive guide covers validation techniques using regular expressions, built-in validation methods, and industry-standard sanitization approaches. We will explore practical implementations for web forms, APIs, and data processing pipelines, emphasizing security best practices that every JavaScript developer must understand.

---

## 🔤 Table of Contents

1. [Input Validation Fundamentals](#input-validation-fundamentals)
2. [Common Validation Patterns](#common-validation-patterns)
3. [XSS Prevention Techniques](#xss-prevention-techniques)
4. [Sanitization Strategies](#sanitization-strategies)
5. [Form Validation Implementation](#form-validation-implementation)
6. [API Input Validation](#api-input-validation)
7. [Security Best Practices](#security-best-practices)
8. [Key Takeaways](#key-takeaways)
9. [Common Pitfalls](#common-pitfalls)

---

## 🎯 Input Validation Fundamentals

Input validation is the first line of defense against malformed or malicious data. It ensures that data conforms to expected formats before processing.

### Why Validation Matters

Proper validation prevents:
- Data corruption from invalid formats
- Security vulnerabilities from malicious input
- Application crashes from unexpected data types
- Poor user experience from unclear error messages

### Validation Approaches

**Code Example 1: Basic Validation Patterns**

```javascript
// file: examples/validation-basics.js
// Description: Basic validation approaches

// Simple presence check
function isPresent(value) {
    return value !== null && value !== undefined && value !== "";
}

console.log(isPresent("hello"));  // true
console.log(isPresent(""));       // false
console.log(isPresent(null));      // false

// Type checking
function isString(value) {
    return typeof value === "string";
}

console.log(isString("hello"));  // true
console.log(isString(123));      // false
console.log(isString(null));     // false

// Length validation
function validateLength(value, min, max) {
    if (!isString(value)) return false;
    const length = value.length;
    return length >= min && length <= max;
}

console.log(validateLength("hello", 3, 10)); // true
console.log(validateLength("hi", 3, 10));    // false

// Pattern validation
function matchesPattern(value, pattern) {
    if (!isString(value)) return false;
    return pattern.test(value);
}

const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
console.log(matchesPattern("test@example.com", emailPattern)); // true

// Combined validation
function validateUsername(username) {
    return isPresent(username) &&
           validateLength(username, 3, 20) &&
           matchesPattern(username, /^[a-zA-Z0-9_]+$/);
}

console.log(validateUsername("john_doe"));   // true
console.log(validateUsername("ab"));         // false
console.log(validateUsername("john@doe"));   // false
```

### Validation Result Objects

**Code Example 2: Structured Validation Results**

```javascript
// file: examples/validation-results.js
// Description: Creating structured validation feedback

class ValidationResult {
    constructor(isValid, errors = []) {
        this.isValid = isValid;
        this.errors = errors;
    }

    static success() {
        return new ValidationResult(true);
    }

    static failure(errors) {
        return new ValidationResult(false, Array.isArray(errors) ? errors : [errors]);
    }

    addError(error) {
        this.errors.push(error);
        this.isValid = false;
        return this;
    }
}

// Validation function returning structured results
function validateUserInput(input) {
    const result = new ValidationResult(true);

    // Check presence
    if (!isPresent(input.username)) {
        result.addError("Username is required");
    } else if (!validateLength(input.username, 3, 20)) {
        result.addError("Username must be between 3 and 20 characters");
    } else if (!matchesPattern(input.username, /^[a-zA-Z0-9_]+$/)) {
        result.addError("Username can only contain letters, numbers, and underscores");
    }

    // Email validation
    if (!isPresent(input.email)) {
        result.addError("Email is required");
    } else if (!matchesPattern(input.email, /^[^\s@]+@[^\s@]+\.[^\s@]+$/)) {
        result.addError("Invalid email format");
    }

    // Password validation
    if (!isPresent(input.password)) {
        result.addError("Password is required");
    } else if (input.password.length < 8) {
        result.addError("Password must be at least 8 characters");
    } else if (!matchesPattern(input.password, /[A-Z]/)) {
        result.addError("Password must contain at least one uppercase letter");
    } else if (!matchesPattern(input.password, /[a-z]/)) {
        result.addError("Password must contain at least one lowercase letter");
    } else if (!matchesPattern(input.password, /[0-9]/)) {
        result.addError("Password must contain at least one number");
    }

    return result;
}

// Test validation
const testInput = {
    username: "john_doe",
    email: "john@example.com",
    password: "Password123"
};

const result = validateUserInput(testInput);
console.log(result.isValid);        // true
console.log(result.errors);        // []

const invalidInput = {
    username: "ab",
    email: "invalid",
    password: "weak"
};

const invalidResult = validateUserInput(invalidInput);
console.log(invalidResult.isValid);  // false
console.log(invalidResult.errors);
// ["Username must be between 3 and 20 characters", 
//  "Invalid email format", 
//  "Password must be at least 8 characters", ...]
```

---

## 📝 Common Validation Patterns

### Email Validation

**Code Example 3: Comprehensive Email Validation**

```javascript
// file: examples/email-validation.js
// Description: Email validation patterns and implementations

// Basic email pattern (suitable for most cases)
const basicEmailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

// More strict pattern
const strictEmailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

function validateEmail(email) {
    if (!email || typeof email !== "string") {
        return { valid: false, error: "Email is required" };
    }

    const trimmed = email.trim();
    
    if (trimmed.length === 0) {
        return { valid: false, error: "Email cannot be empty" };
    }

    if (trimmed.length > 254) {
        return { valid: false, error: "Email is too long" };
    }

    if (!strictEmailPattern.test(trimmed)) {
        return { valid: false, error: "Invalid email format" };
    }

    // Check for disposable email domains
    const disposableDomains = ["tempmail.com", "throwaway.com", "10minutemail.com"];
    const domain = trimmed.split("@")[1]?.toLowerCase();
    
    if (disposableDomains.includes(domain)) {
        return { valid: false, error: "Disposable email addresses are not allowed" };
    }

    return { valid: true };
}

console.log(validateEmail("test@example.com"));  // { valid: true }
console.log(validateEmail("invalid@"));          // { valid: false }
console.log(validateEmail("@example.com"));      // { valid: false }
console.log(validateEmail("test@tempmail.com")); // { valid: false }
```

### URL Validation

**Code Example 4: URL Validation**

```javascript
// file: examples/url-validation.js
// Description: URL validation techniques

function validateURL(url) {
    if (!url || typeof url !== "string") {
        return { valid: false, error: "URL is required" };
    }

    try {
        const parsed = new URL(url);
        
        // Validate protocol
        if (!["http:", "https:"].includes(parsed.protocol)) {
            return { valid: false, error: "Only HTTP and HTTPS protocols are allowed" };
        }

        // Validate hostname
        if (!parsed.hostname || parsed.hostname.length === 0) {
            return { valid: false, error: "Invalid hostname" };
        }

        return { valid: true, parsed: parsed };
    } catch (e) {
        return { valid: false, error: "Invalid URL format" };
    }
}

// Usage
const result1 = validateURL("https://example.com");
console.log(result1.valid); // true

const result2 = validateURL("javascript:alert(1)");
console.log(result2.valid); // false (protocol validation)

const result3 = validateURL("not-a-url");
console.log(result3.valid); // false
```

### Phone Number Validation

**Code Example 5: Phone Number Validation**

```javascript
// file: examples/phone-validation.js
// Description: Phone number validation patterns

// US phone number pattern
const usPhonePattern = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;

// International phone pattern (basic)
const intlPhonePattern = /^\+?[1-9]\d{1,14}$/;

function validatePhone(phone, options = {}) {
    if (!phone || typeof phone !== "string") {
        return { valid: false, error: "Phone number is required" };
    }

    const digitsOnly = phone.replace(/\D/g, "");
    const { format = "US", allowInternational = true } = options;

    if (format === "US") {
        if (digitsOnly.length !== 10) {
            return { valid: false, error: "US phone number must be 10 digits" };
        }
        
        // Don't allow phone numbers starting with certain patterns
        if (digitsOnly[0] === "0" || digitsOnly[0] === "1") {
            return { valid: false, error: "Invalid US phone number" };
        }
    }

    if (allowInternational) {
        if (digitsOnly.length < 7 || digitsOnly.length > 15) {
            return { valid: false, error: "Phone number must be between 7 and 15 digits" };
        }
    }

    return { 
        valid: true, 
        formatted: format === "US" 
            ? `(${digitsOnly.slice(0,3)}) ${digitsOnly.slice(3,6)}-${digitsOnly.slice(6)}`
            : digitsOnly
    };
}

console.log(validatePhone("555-123-4567"));         // { valid: true }
console.log(validatePhone("(555) 123 4567"));       // { valid: true }
console.log(validatePhone("5551234567"));          // { valid: true }
console.log(validatePhone("123-456-7890"));        // { valid: false }
console.log(validatePhone("+1-555-123-4567"));    // { valid: true }
```

---

## 🛡️ XSS Prevention Techniques

Cross-Site Scripting (XSS) is one of the most common and dangerous web vulnerabilities. Preventing XSS requires careful sanitization of any user-generated content that could be rendered in a browser.

### Understanding XSS

XSS attacks inject malicious scripts into web pages viewed by other users. There are three main types:
1. **Reflected XSS**: Malicious script is part of the request
2. **Stored XSS**: Malicious script is saved in the database
3. **DOM-based XSS**: Malicious script manipulates the DOM

### HTML Context Escaping

**Code Example 6: HTML Escaping Functions**

```javascript
// file: examples/html-escaping.js
// Description: Escaping HTML for safe rendering

function escapeHTML(str) {
    if (typeof str !== "string") {
        return str;
    }
    
    return str
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#39;")
        .replace(/\//g, "&#x2F;");
}

// Usage in template
function safeRender(template, data) {
    const escaped = {
        username: escapeHTML(data.username),
        content: escapeHTML(data.content)
    };
    
    return template
        .replace("${username}", escaped.username)
        .replace("${content}", escaped.content);
}

const userData = {
    username: "<script>alert('XSS')</script>",
    content: "Hello <b>World</b>!"
};

const result = safeRender(
    "<div class='user'>${username}: ${content}</div>",
    userData
);

console.log(result);
// "<div class='user'>&lt;script&gt;alert('XSS')&lt;/script&gt;: Hello &lt;b&gt;World&lt;/b&gt;!</div>"
```

### Attribute Context Escaping

**Code Example 7: Attribute Escaping**

```javascript
// file: examples/attribute-escaping.js
// Description: Escaping for HTML attributes

function escapeAttribute(str) {
    if (typeof str !== "string") {
        return str;
    }
    
    return str
        .replace(/&/g, "&amp;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#39;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;");
}

function createSafeElement(tag, attributes, content) {
    const safeAttrs = Object.entries(attributes)
        .map(([key, value]) => {
            const safeValue = escapeAttribute(String(value));
            return `${key}="${safeValue}"`;
        })
        .join(" ");
    
    return `<${tag} ${safeAttrs}>${escapeHTML(content)}</${tag}>`;
}

// Safe button creation
const button = createSafeElement("button", {
    "class": "btn",
    "onclick": "handleClick()"  // This should be event handler, not arbitrary
}, "Click me");

// Input with safe value
const input = createSafeElement("input", {
    type: "text",
    value: '"><script>alert(1)</script>',
    placeholder: "Enter text"
}, "");

console.log(input);
// '<input type="text" value="&quot;&gt;&lt;script&gt;alert(1)&lt;/script&gt;" placeholder="Enter text">'
```

### URL Context Escaping

**Code Example 8: URL Sanitization**

```javascript
// file: examples/url-sanitization.js
// Description: Safe URL handling

function sanitizeURL(url, allowRelative = false) {
    if (typeof url !== "string") {
        return null;
    }

    // Allow relative URLs if specified
    if (allowRelative && !url.startsWith("//") && !url.match(/^[a-z]+:/i)) {
        return url;
    }

    try {
        const parsed = new URL(url);
        
        // Block dangerous protocols
        const dangerous = ["javascript:", "data:", "vbscript:"];
        if (dangerous.includes(parsed.protocol)) {
            return null;
        }

        return parsed.href;
    } catch {
        return null;
    }
}

// Safe link rendering
function createSafeLink(url, text) {
    const safeUrl = sanitizeURL(url);
    
    if (!safeUrl) {
        return `<span>${escapeHTML(text)}</span>`;
    }
    
    return `<a href="${escapeAttribute(safeUrl)}">${escapeHTML(text)}</a>`;
}

console.log(createSafeLink("https://example.com", "Visit Site"));
// '<a href="https://example.com">Visit Site</a>'

console.log(createSafeLink("javascript:alert(1)", "Click"));
// '<span>Click</span>'

console.log(createSafeLink("javascript:void(0)", "Dangerous"));
// '<span>Dangerous</span>'
```

---

## 🧹 Sanitization Strategies

Sanitization transforms input into a safe format by removing or encoding dangerous characters.

### Text Sanitization

**Code Example 9: Text Sanitization Functions**

```javascript
// file: examples/text-sanitization.js
// Description: Text sanitization approaches

function sanitizeText(input, options = {}) {
    const {
        maxLength = 1000,
        trim = true,
        removeControlChars = true,
        allowNewlines = true
    } = options;

    if (typeof input !== "string") {
        return "";
    }

    let sanitized = trim ? input.trim() : input;

    // Remove null bytes and control characters
    if (removeControlChars) {
        sanitized = sanitized.replace(/[\x00-\x08\x0B\x0C\x0E-\x1F\x7F]/g, "");
    }

    // Normalize newlines if allowed
    if (allowNewlines) {
        sanitized = sanitized.replace(/\r\n/g, "\n").replace(/\r/g, "\n");
    } else {
        sanitized = sanitized.replace(/[\r\n]/g, " ");
    }

    // Remove excessive whitespace
    sanitized = sanitized.replace(/\s+/g, " ");

    // Enforce max length
    if (sanitized.length > maxLength) {
        sanitized = sanitized.substring(0, maxLength);
    }

    return sanitized;
}

// Usage
const dirtyInput = "  Hello   World  \n\n\n  ";
console.log(sanitizeText(dirtyInput)); // "Hello World"

const withControl = "Hello\x00World\x1FTest";
console.log(sanitizeText(withControl)); // "HelloWorldTest"

const longText = "a".repeat(2000);
console.log(sanitizeText(longText).length); // 1000
```

### SQL Injection Prevention

**Code Example 10: SQL Safety**

```javascript
// file: examples/sql-sanitization.js
// Description: SQL injection prevention

// Parameterized queries are the best defense
// This example shows sanitization for legacy scenarios

function sanitizeForSQL(input) {
    if (typeof input !== "string") {
        return input;
    }

    // Replace dangerous characters
    return input
        .replace(/'/g, "''")  // Escape single quotes
        .replace(/\\/g, "\\\\") // Escape backslashes
        .replace(/;/g, "\\;"); // Escape semicolons (for extra safety)
}

// Better: Use parameterized queries
function buildParameterizedQuery(tableName, data) {
    const columns = Object.keys(data).join(", ");
    const placeholders = Object.keys(data).map(() => "?").join(", ");
    
    // Validate table name (allow only alphanumeric and underscore)
    if (!/^[a-zA-Z_][a-zA-Z0-9_]*$/.test(tableName)) {
        throw new Error("Invalid table name");
    }
    
    return {
        query: `INSERT INTO ${tableName} (${columns}) VALUES (${placeholders})`,
        params: Object.values(data)
    };
}

// Usage
const queryData = {
    name: "O'Brien",
    description: "Some description; DROP TABLE users--"
};

const { query, params } = buildParameterizedQuery("users", queryData);
console.log(query);  // "INSERT INTO users (name, description) VALUES (?, ?)"
console.log(params); // ["O'Brien", "Some description; DROP TABLE users--"]
```

### Number and Date Sanitization

**Code Example 11: Numeric and Date Validation**

```javascript
// file: examples/numeric-sanitization.js
// Description: Sanitizing numeric and date inputs

function sanitizeNumber(input, options = {}) {
    const {
        min = -Infinity,
        max = Infinity,
        defaultValue = null
    } = options;

    if (input === null || input === undefined || input === "") {
        return defaultValue;
    }

    const num = Number(input);

    if (isNaN(num)) {
        return defaultValue;
    }

    if (num < min) return min;
    if (num > max) return max;

    return num;
}

function sanitizeInteger(input, options = {}) {
    const sanitized = sanitizeNumber(input, options);
    return sanitized === null ? null : Math.floor(sanitized);
}

function sanitizePositiveInteger(input, defaultValue = null) {
    return sanitizeInteger(input, { min: 1, defaultValue });
}

// Usage
console.log(sanitizeNumber("123.45"));       // 123.45
console.log(sanitizeNumber("abc", { defaultValue: 0 })); // 0
console.log(sanitizeNumber("1000", { max: 100 }));      // 100
console.log(sanitizeInteger("123.99"));     // 123

// Date sanitization
function sanitizeDate(input, defaultValue = null) {
    if (input instanceof Date) {
        return isNaN(input.getTime()) ? defaultValue : input;
    }

    if (typeof input === "string") {
        const date = new Date(input);
        return isNaN(date.getTime()) ? defaultValue : date;
    }

    return defaultValue;
}

console.log(sanitizeDate("2024-01-15"));    // Date object
console.log(sanitizeDate("invalid"));      // null
console.log(sanitizeDate(null, new Date())); // Current date
```

---

## 📝 Form Validation Implementation

### Complete Form Validator

**Code Example 12: Full Form Validation**

```javascript
// file: examples/form-validation.js
// Description: Complete form validation implementation

class FormValidator {
    constructor(schema) {
        this.schema = schema;
    }

    validate(formData) {
        const errors = {};
        
        for (const [field, rules] of Object.entries(this.schema)) {
            const value = formData[field];
            const fieldErrors = this.validateField(field, value, rules);
            
            if (fieldErrors.length > 0) {
                errors[field] = fieldErrors;
            }
        }

        return {
            isValid: Object.keys(errors).length === 0,
            errors
        };
    }

    validateField(name, value, rules) {
        const errors = [];

        // Required check
        if (rules.required && (value === undefined || value === null || value === "")) {
            errors.push(`${name} is required`);
            return errors; // Don't validate further if required fails
        }

        // Skip other validations if empty and not required
        if (value === undefined || value === null || value === "") {
            return errors;
        }

        // Type validation
        if (rules.type === "string") {
            if (typeof value !== "string") {
                errors.push(`${name} must be a string`);
            }
        } else if (rules.type === "number") {
            if (typeof value !== "number" || isNaN(value)) {
                errors.push(`${name} must be a number`);
            }
        } else if (rules.type === "email") {
            const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailPattern.test(value)) {
                errors.push(`${name} must be a valid email`);
            }
        }

        // Length validation
        if (rules.minLength !== undefined && value.length < rules.minLength) {
            errors.push(`${name} must be at least ${rules.minLength} characters`);
        }

        if (rules.maxLength !== undefined && value.length > rules.maxLength) {
            errors.push(`${name} must be at most ${rules.maxLength} characters`);
        }

        // Range validation
        if (rules.min !== undefined && value < rules.min) {
            errors.push(`${name} must be at least ${rules.min}`);
        }

        if (rules.max !== undefined && value > rules.max) {
            errors.push(`${name} must be at most ${rules.max}`);
        }

        // Pattern validation
        if (rules.pattern) {
            if (!rules.pattern.test(value)) {
                errors.push(rules.patternMessage || `${name} has invalid format`);
            }
        }

        // Custom validator
        if (rules.custom) {
            const customResult = rules.custom(value);
            if (customResult !== true) {
                errors.push(customResult || `${name} is invalid`);
            }
        }

        return errors;
    }
}

// Define validation schema
const registrationSchema = {
    username: {
        required: true,
        type: "string",
        minLength: 3,
        maxLength: 20,
        pattern: /^[a-zA-Z0-9_]+$/,
        patternMessage: "Username can only contain letters, numbers, and underscores"
    },
    email: {
        required: true,
        type: "email"
    },
    password: {
        required: true,
        type: "string",
        minLength: 8,
        custom: (value) => {
            if (!/[A-Z]/.test(value)) return "Password must contain an uppercase letter";
            if (!/[a-z]/.test(value)) return "Password must contain a lowercase letter";
            if (!/[0-9]/.test(value)) return "Password must contain a number";
            return true;
        }
    },
    age: {
        required: false,
        type: "number",
        min: 13,
        max: 120
    }
};

// Test validator
const validator = new FormValidator(registrationSchema);

const validData = {
    username: "john_doe",
    email: "john@example.com",
    password: "Password123",
    age: 25
};

const invalidData = {
    username: "ab",
    email: "invalid",
    password: "weak",
    age: 10
};

console.log(validator.validate(validData));   // { isValid: true, errors: {} }
console.log(validator.validate(invalidData)); // { isValid: false, errors: {...} }
```

---

## 🔌 API Input Validation

### Request Validation Middleware

**Code Example 13: API Validation Middleware**

```javascript
// file: examples/api-validation.js
// Description: API request validation

function validateRequest(schema, data) {
    const errors = [];
    
    // Validate required fields
    for (const field of schema.required || []) {
        if (data[field] === undefined) {
            errors.push(`Missing required field: ${field}`);
        }
    }

    // Validate each field type
    for (const [field, rules] of Object.entries(schema.properties || {})) {
        const value = data[field];
        
        if (value === undefined) continue;

        // Type checking
        if (rules.type === "string" && typeof value !== "string") {
            errors.push(`${field} must be a string`);
        } else if (rules.type === "number" && typeof value !== "number") {
            errors.push(`${field} must be a number`);
        } else if (rules.type === "boolean" && typeof value !== "boolean") {
            errors.push(`${field} must be a boolean`);
        } else if (rules.type === "array" && !Array.isArray(value)) {
            errors.push(`${field} must be an array`);
        } else if (rules.type === "object" && (typeof value !== "object" || Array.isArray(value))) {
            errors.push(`${field} must be an object`);
        }

        // String validations
        if (rules.type === "string" && typeof value === "string") {
            if (rules.minLength !== undefined && value.length < rules.minLength) {
                errors.push(`${field} must be at least ${rules.minLength} characters`);
            }
            if (rules.maxLength !== undefined && value.length > rules.maxLength) {
                errors.push(`${field} must be at most ${rules.maxLength} characters`);
            }
            if (rules.pattern !== undefined && !rules.pattern.test(value)) {
                errors.push(`${field} has invalid format`);
            }
            if (rules.enum !== undefined && !rules.enum.includes(value)) {
                errors.push(`${field} must be one of: ${rules.enum.join(", ")}`);
            }
        }

        // Number validations
        if (rules.type === "number" && typeof value === "number") {
            if (rules.minimum !== undefined && value < rules.minimum) {
                errors.push(`${field} must be at least ${rules.minimum}`);
            }
            if (rules.maximum !== undefined && value > rules.maximum) {
                errors.push(`${field} must be at most ${rules.maximum}`);
            }
        }
    }

    return {
        valid: errors.length === 0,
        errors
    };
}

// JSON Schema-like validation
const userSchema = {
    required: ["username", "email"],
    properties: {
        username: {
            type: "string",
            minLength: 3,
            maxLength: 20,
            pattern: /^[a-zA-Z0-9_]+$/
        },
        email: {
            type: "string",
            format: "email"
        },
        age: {
            type: "number",
            minimum: 0,
            maximum: 150
        },
        role: {
            type: "string",
            enum: ["user", "admin", "moderator"]
        }
    }
};

// Test API validation
const validRequest = {
    username: "john_doe",
    email: "john@example.com",
    age: 25,
    role: "user"
};

const invalidRequest = {
    username: "ab",
    email: "invalid",
    age: -5,
    role: "superadmin"
};

console.log(validateRequest(userSchema, validRequest));
// { valid: true, errors: [] }

console.log(validateRequest(userSchema, invalidRequest));
// { valid: false, errors: [...] }
```

---

## 🔒 Security Best Practices

### Input Validation Security

**Code Example 14: Security Best Practices**

```javascript
// file: examples/security-best-practices.js
// Description: Security-focused validation and sanitization

class SecureValidator {
    // Whitelist approach - only allow known good values
    static whitelist(value, allowedValues) {
        return allowedValues.includes(value) ? value : null;
    }

    // Blacklist approach - remove known bad patterns
    static blacklist(value, patterns) {
        let result = value;
        for (const pattern of patterns) {
            result = result.replace(pattern, "");
        }
        return result;
    }

    // Normalize input - standardizes format
    static normalize(input, type) {
        switch (type) {
            case "email":
                return input.toLowerCase().trim();
            case "phone":
                return input.replace(/\D/g, "");
            case "url":
                try {
                    return new URL(input).href;
                } catch {
                    return null;
                }
            default:
                return input;
        }
    }

    // Strip tags from HTML content
    static stripTags(input) {
        const div = document.createElement("div");
        div.textContent = input;
        return div.innerHTML;
    }
}

// Defense in depth - multiple validation layers
function secureInputProcessing(input) {
    // Layer 1: Type validation
    if (typeof input !== "string") {
        return null;
    }

    // Layer 2: Length limits
    if (input.length > 10000) {
        return null;
    }

    // Layer 3: Sanitization
    const sanitized = SecureValidator.stripTags(input);

    // Layer 4: Validation
    if (!/^[a-zA-Z0-9\s.,!?-]+$/.test(sanitized)) {
        return null;
    }

    return sanitized;
}

// Usage
console.log(SecureValidator.whitelist("admin", ["user", "admin", "moderator"]));
// "admin"

console.log(SecureValidator.whitelist("superuser", ["user", "admin", "moderator"]));
// null

console.log(secureInputProcessing("Hello <script>alert(1)</script>"));
// "Hello alert(1)"
```

### Content Security Policy

**Code Example 15: CSP Header Generation**

```javascript
// file: examples/csp.js
// Description: Content Security Policy helpers

function generateCSP(options = {}) {
    const {
        defaultSrc = ["'self'"],
        scriptSrc = ["'self'"],
        styleSrc = ["'self'"],
        imgSrc = ["'self'", "data:"],
        fontSrc = ["'self'"],
        connectSrc = ["'self'"],
        frameSrc = ["'none'"]
    } = options;

    const directives = [
        `default-src ${defaultSrc.join(" ")}`,
        `script-src ${scriptSrc.join(" ")}`,
        `style-src ${styleSrc.join(" ")}`,
        `img-src ${imgSrc.join(" ")}`,
        `font-src ${fontSrc.join(" ")}`,
        `connect-src ${connectSrc.join(" ")}`,
        `frame-src ${frameSrc.join(" ")}`,
        "object-src 'none'",
        "base-uri 'self'",
        "form-action 'self'"
    ];

    return directives.join("; ");
}

// CSP for development
const devCSP = generateCSP({
    scriptSrc: ["'self'", "'unsafe-inline'"],
    styleSrc: ["'self'", "'unsafe-inline'"]
});

// CSP for production (stricter)
const prodCSP = generateCSP({
    scriptSrc: ["'self'"],
    styleSrc: ["'self'"],
    imgSrc: ["'self'", "https:"],
    connectSrc: ["'self'", "https://api.example.com"]
});

console.log(prodCSP);
// "default-src 'self'; script-src 'self'; ..."
```

---

## 📊 Key Takeaways

1. **Validate Early, Validate Often**: Validate input at entry points and throughout processing.

2. **Use Whitelist Over Blacklist**: Whitelist allowed values rather than trying to block all bad values.

3. **Defense in Depth**: Apply multiple layers of validation and sanitization.

4. **Context Matters**: Different contexts (HTML, attributes, URLs, SQL) require different escaping.

5. **Parameterized Queries**: Always use parameterized queries for database operations.

6. **Sanitize Before Display**: Always escape output before rendering in HTML.

7. **Limit Input Length**: Enforce reasonable length limits to prevent buffer overflow attacks.

---

## ⚠️ Common Pitfalls

1. **Client-Side Only Validation**: Never rely solely on client-side validation; always validate on the server.

2. **Incomplete Validation**: Validating only presence but not format or type.

3. **Forgetting Context**: Different contexts require different sanitization approaches.

4. **Not Sanitizing Output**: Validating input but forgetting to escape when displaying.

5. **Over-Reliance on Regex**: Some patterns are too complex and may have edge cases.

6. **Not Handling Null/Undefined**: Failing to check for null/undefined before processing.

7. **Ignoring Encoding**: Not considering character encoding issues.

---

## 🔗 Related Files

- **[String Methods Comprehensive](./01_STRING_METHODS_COMPREHENSIVE.md)** - String manipulation
- **[Regular Expressions Master](./02_REGULAR_EXPRESSIONS_MASTER.md)** - Pattern-based validation
- **[String Templates and Interpolation](./03_STRING_TEMPLATES_AND_INTERPOLATION.md)** - Template security
- **[Security Deep Dive](../70_JAVASCRIPT_SECURITY_DEEP_DIVE.md)** - Security patterns

---

## 📚 Further Reading

- [OWASP Input Validation](https://cheatsheetseries.owasp.org/cheatsheets/Input_Validation_Cheat_Sheet.html)
- [OWASP XSS Prevention](https://cheatsheetseries.owasp.org/cheatsheets/Cross_Site_Scripting_Prevention_Cheat_Sheet.html)
- [MDN: Security](https://developer.mozilla.org/en-US/docs/Web/Security)