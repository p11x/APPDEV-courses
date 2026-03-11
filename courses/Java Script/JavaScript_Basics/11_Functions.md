# Functions

## What are Functions?

Functions are reusable blocks of code that perform a specific task. Instead of writing the same code multiple times, you can create a function once and call it whenever you need it. Functions make code organized, reusable, and easier to maintain.

Functions can accept input (parameters), perform actions, and return output (values).

## Key Bullet Points

- **Function Declaration**: Creates a function using the `function` keyword
- **Function Expression**: Assigns a function to a variable
- **Parameters**: Variables listed in the function definition (inside parentheses)
- **Arguments**: Actual values passed to the function when calling it
- **Return**: Sends a value back from the function
- Functions can have 0 or more parameters and can return 0 or 1 value

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Functions</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #fce4ec; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #e91e63;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #e91e63;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JavaScript Functions</h1>
    
    <button onclick="testGreet()">Simple Function</button>
    <button onclick="testWithParam()">Function with Parameter</button>
    <button onclick="testReturn()">Function with Return</button>
    <button onclick="testCalculator()">Function Expression</button>
    
    <div id="output">Click a button to see functions in action!</div>

    <script>
        // Example 1: Simple function with no parameters
        function greet() {
            document.getElementById("output").innerHTML = 
                "<div class='result'>Hello! This is a simple function.</div>";
        }

        // Example 2: Function with parameter
        function greetUser(name) {
            document.getElementById("output").innerHTML = 
                "<div class='result'>Hello, <strong>" + name + "</strong>! Welcome to our site.</div>";
        }

        // Example 3: Function with return value
        function addNumbers(a, b) {
            return a + b;
        }

        // Example 4: Function expression
        const multiply = function(x, y) {
            return x * y;
        };

        // Button handlers
        function testGreet() {
            greet();  // Call the simple function
        }

        function testWithParam() {
            let name = prompt("Enter your name:") || "Guest";
            greetUser(name);  // Call with argument
        }

        function testReturn() {
            let num1 = 10;
            let num2 = 5;
            let sum = addNumbers(num1, num2);  // Store returned value
            document.getElementById("output").innerHTML = 
                "<div class='result'>addNumbers(" + num1 + ", " + num2 + ") returned: <strong>" + sum + "</strong></div>";
        }

        function testCalculator() {
            let num1 = 4;
            let num2 = 7;
            let product = multiply(num1, num2);  // Use function expression
            document.getElementById("output").innerHTML = 
                "<div class='result'>multiply(" + num1 + ", " + num2 + ") returned: <strong>" + product + "</strong></div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **Function declaration**: `function greet()` creates a function named "greet" with no parameters.
- **Parameters**: `function greetUser(name)` has one parameter called "name" - it's a placeholder for the actual value.
- **Arguments**: When we call `greetUser("John")`, "John" is the argument that gets assigned to the parameter.
- **Return**: `return a + b` sends the result back to wherever the function was called. The function stops executing after return.
- **Function expression**: `const multiply = function(x, y) {...}` assigns an anonymous function to a variable. This is useful when you want to pass functions around.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and four buttons
2. Click "Simple Function" - shows "Hello! This is a simple function."
3. Click "Function with Parameter" - prompts for your name, then shows "Hello, [name]! Welcome to our site."
4. Click "Function with Return" - shows "addNumbers(10, 5) returned: 15"
5. Click "Function Expression" - shows "multiply(4, 7) returned: 28"
