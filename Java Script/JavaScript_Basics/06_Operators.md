# Operators

## What are Operators?

Operators are symbols that perform operations on values (operands). They are the building blocks of computation in JavaScript, allowing you to do math, compare values, combine conditions, and more. Without operators, programs wouldn't be able to calculate or make decisions.

Operators are essential for virtually every program - from simple calculators to complex business applications.

## Key Bullet Points

- **Arithmetic Operators**: `+` (add), `-` (subtract), `*` (multiply), `/` (divide), `%` (modulus/remainder)
- **Comparison Operators**: `==` (loose equality), `===` (strict equality), `!=`, `>`, `<`, `>=`, `<=`
- **Logical Operators**: `&&` (AND), `||` (OR), `!` (NOT)
- Always use `===` instead of `==` for accurate comparisons
- Arithmetic operators follow standard math order (PEMDAS)
- Logical operators combine multiple conditions

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Operators</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .section { background: #f5f5f5; padding: 15px; margin: 10px 0; border-left: 4px solid #007bff; }
    </style>
</head>
<body>
    <h1>JavaScript Operators</h1>
    <div id="output"></div>

    <script>
        let output = "";

        // ARITHMETIC OPERATORS
        let a = 10;
        let b = 3;
        
        output += "<div class='section'><h3>Arithmetic Operators</h3>";
        output += "<p>a = " + a + ", b = " + b + "</p>";
        output += "<p>a + b = " + (a + b) + "</p>";      // 13
        output += "<p>a - b = " + (a - b) + "</p>";      // 7
        output += "<p>a * b = " + (a * b) + "</p>";      // 30
        output += "<p>a / b = " + (a / b) + "</p>";      // 3.333...
        output += "<p>a % b = " + (a % b) + "</p>";      // 1 (remainder)
        output += "</div>";

        // COMPARISON OPERATORS
        let x = 5;
        let y = "5";
        let z = 10;

        output += "<div class='section'><h3>Comparison Operators</h3>";
        output += "<p>x = " + x + " (number), y = '" + y + "' (string), z = " + z + "</p>";
        output += "<p>x == y: " + (x == y) + " (loose - compares value only)</p>";
        output += "<p>x === y: " + (x === y) + " (strict - compares value AND type)</p>";
        output += "<p>x < z: " + (x < z) + "</p>";
        output += "<p>x >= 5: " + (x >= 5) + "</p>";
        output += "<p>x != z: " + (x != z) + "</p>";
        output += "</div>";

        // LOGICAL OPERATORS
        let age = 25;
        let hasLicense = true;

        output += "<div class='section'><h3>Logical Operators</h3>";
        output += "<p>age = " + age + ", hasLicense = " + hasLicense + "</p>";
        output += "<p>age >= 18 && hasLicense: " + (age >= 18 && hasLicense) + " (AND - both must be true)</p>";
        output += "<p>age < 18 || hasLicense: " + (age < 18 || hasLicense) + " (OR - at least one must be true)</p>";
        output += "<p>!hasLicense: " + (!hasLicense) + " (NOT - flips the value)</p>";
        output += "</div>";

        // Display results
        document.getElementById("output").innerHTML = output;

        // Also log to console
        console.log("Arithmetic:", a + b, a - b, a * b, a / b, a % b);
        console.log("Comparison - loose:", x == y, "strict:", x === y);
        console.log("Logical:", age >= 18 && hasLicense);
    </script>
</body>
</html>
```

## Code Explanation

- **Arithmetic**: `+`, `-`, `*`, `/` work as expected. The `%` (modulus) operator returns the remainder after division - useful for checking if numbers are even or odd.
- **Comparison - Loose (==)**: Compares values only. `5 == "5"` is `true` because JavaScript converts the string to a number.
- **Comparison - Strict (===)**: Compares both value AND type. `5 === "5"` is `false` because one is a number and one is a string.
- **Logical AND (&&)**: Returns true only if BOTH conditions are true.
- **Logical OR (||)**: Returns true if AT LEAST ONE condition is true.
- **Logical NOT (!)**: Flips the boolean value - `true` becomes `false` and vice versa.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading "JavaScript Operators"
2. Three sections appear showing:
   - **Arithmetic**: Shows results of 10 + 3 = 13, 10 - 3 = 7, 10 * 3 = 30, 10 / 3 = 3.33..., 10 % 3 = 1
   - **Comparison**: Shows that `==` returns true but `===` returns false for comparing number 5 to string "5"
   - **Logical**: Shows how AND, OR, and NOT work with age and license conditions
