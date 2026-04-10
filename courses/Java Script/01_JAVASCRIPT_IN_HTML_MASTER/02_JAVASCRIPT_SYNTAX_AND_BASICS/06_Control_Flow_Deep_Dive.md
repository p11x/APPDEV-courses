# 🔄 Control Flow Deep Dive

## 📋 Overview

Control flow determines the order in which your code executes. Understanding these structures is essential for writing dynamic, decision-making JavaScript applications.

---

## 🔀 Conditional Statements

### if...else Statement

```javascript
// Basic if
if (condition) {
    // code runs if true
}

// if...else
if (age >= 18) {
    console.log("Adult");
} else {
    console.log("Minor");
}

// if...else if...else
if (score >= 90) {
    grade = "A";
} else if (score >= 80) {
    grade = "B";
} else if (score >= 70) {
    grade = "C";
} else {
    grade = "F";
}
```

### Nested Conditionals

```javascript
function getDiscount(user) {
    if (user) {
        if (user.isMember) {
            if (user.years > 5) {
                return 0.20; // 20% discount
            } else {
                return 0.10; // 10% discount
            }
        } else {
            return 0.05; // 5% discount
        }
    }
    return 0; // No discount
}
```

---

## 🔀 Switch Statement

### Basic Syntax

```javascript
const day = "Monday";

switch (day) {
    case "Monday":
    case "Tuesday":
    case "Wednesday":
    case "Thursday":
    case "Friday":
        console.log("Weekday");
        break;
    case "Saturday":
    case "Sunday":
        console.log("Weekend");
        break;
    default:
        console.log("Invalid day");
}
```

### Switch with Return Values

```javascript
function getDayType(day) {
    switch (day) {
        case "Saturday":
        case "Sunday":
            return "Weekend";
        case "Monday":
        case "Tuesday":
        case "Wednesday":
        case "Thursday":
        case "Friday":
            return "Weekday";
        default:
            return "Invalid";
    }
}
```

### Switch Expression (ES2022)

```javascript
// Modern switch expression (not statement!)
const result = switch (day) {
    case "Monday" => "Start of week";
    case "Friday" => "End of week";
    default => "Midweek";
};
```

---

## 🔄 Loops

### for Loop

```javascript
// Classic for loop
for (let i = 0; i < 5; i++) {
    console.log(i); // 0, 1, 2, 3, 4
}

// for...in (object keys)
const user = { name: "John", age: 30, city: "NYC" };
for (let key in user) {
    console.log(`${key}: ${user[key]}`);
}

// for...of (iterable values)
const colors = ["red", "green", "blue"];
for (let color of colors) {
    console.log(color);
}
```

### while Loop

```javascript
// while - condition checked first
let count = 0;
while (count < 5) {
    console.log(count);
    count++;
}

// do...while - runs at least once
let input;
do {
    input = prompt("Enter 'yes':");
} while (input !== "yes");
```

### forEach and map

```javascript
const numbers = [1, 2, 3, 4, 5];

// forEach - iterate without returning
numbers.forEach((num, index) => {
    console.log(`Index ${index}: ${num}`);
});

// map - transform and return new array
const doubled = numbers.map(num => num * 2);
console.log(doubled); // [2, 4, 6, 8, 10]
```

---

## 🛑 Loop Control

### break Statement

```javascript
// Exit loop immediately
for (let i = 0; i < 10; i++) {
    if (i === 5) {
        break; // Exit when i is 5
    }
    console.log(i); // 0, 1, 2, 3, 4
}

// Find first match
const items = [1, 2, 3, 4, 5];
let found;
for (const item of items) {
    if (item > 3) {
        found = item;
        break;
    }
}
console.log(found); // 4
```

### continue Statement

```javascript
// Skip current iteration
for (let i = 0; i < 5; i++) {
    if (i === 2) {
        continue; // Skip when i is 2
    }
    console.log(i); // 0, 1, 3, 4
}

// Filter with continue
const numbers = [1, 2, 3, 4, 5, 6];
const evens = [];
for (const num of numbers) {
    if (num % 2 !== 0) continue;
    evens.push(num);
}
console.log(evens); // [2, 4, 6]
```

---

## 🎯 Real-World Examples

### Form Validation

```javascript
function validateForm(data) {
    const errors = [];
    
    if (!data.username || data.username.length < 3) {
        errors.push("Username must be at least 3 characters");
    }
    
    if (!data.email || !data.email.includes("@")) {
        errors.push("Invalid email");
    }
    
    if (!data.password || data.password.length < 8) {
        errors.push("Password must be at least 8 characters");
    }
    
    if (errors.length > 0) {
        return { valid: false, errors };
    }
    
    return { valid: true };
}
```

### State Machine

```javascript
function processOrder(order) {
    switch (order.status) {
        case "pending":
            return processPending(order);
        case "processing":
            return processProcessing(order);
        case "shipped":
            return processShipped(order);
        case "delivered":
            return processDelivered(order);
        default:
            return { error: "Unknown status" };
    }
}
```

---

## ⚡ Performance Tips

### Early Returns

```javascript
// ❌ Bad - nested conditionals
function processUser(user) {
    if (user) {
        if (user.isActive) {
            if (user.hasPermission) {
                return doSomething();
            }
        }
    }
    return null;
}

// ✅ Good - early returns
function processUser(user) {
    if (!user) return null;
    if (!user.isActive) return null;
    if (!user.hasPermission) return null;
    
    return doSomething();
}
```

### Avoiding Infinite Loops

```javascript
// ❌ Dangerous - no exit condition
while (true) {
    // Browser will freeze!
}

// ✅ Safe - with exit condition
let attempts = 0;
while (attempts < 5) {
    console.log(`Attempt ${attempts + 1}`);
    attempts++;
}
```

---

## 🔗 Related Topics

- [04_Variables_Deep_Dive.md](./04_Variables_Deep_Dive.md)
- [06_Operators_Mastery.md](./06_Operators_Mastery.md)

---

**Next: Learn about [Functions Complete Guide](./07_Functions_Complete_Guide.md)**