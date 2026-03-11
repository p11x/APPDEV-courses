# Variables

## What are Variables?

Variables are containers for storing data values. Think of a variable as a labeled box where you can put information and retrieve it later. In JavaScript, variables can hold different types of data like numbers, text, and more complex information.

Variables are essential to programming because they allow you to store, manipulate, and use data throughout your code. Without variables, you would have to hard-code every value, making your programs inflexible.

## Key Bullet Points

- **var**: The older way to declare variables. Function-scoped (available throughout the function).
- **let**: The modern way to declare variables that can be reassigned. Block-scoped (only available in the block where defined).
- **const**: Declares constants - values that cannot be reassigned. Also block-scoped.
- Use `const` by default for values that won't change, `let` for values that will change, avoid `var`
- Variable names cannot start with numbers, cannot contain spaces, and are case-sensitive
- Use descriptive names like `firstName` or `userAge` instead of `x` or `y`

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Variables</title>
</head>
<body>
    <h1>JavaScript Variables</h1>
    <p id="output"></p>

    <script>
        // Using var (older method - avoid in modern code)
        var oldWay = "This is var";
        console.log("var:", oldWay);

        // Using let - can be reassigned
        let message = "Hello";
        console.log("Initial let:", message);
        
        message = "Hello World";  // Reassigning the value
        console.log("Reassigned let:", message);

        // Using const - cannot be reassigned
        const PI = 3.14159;
        console.log("const PI:", PI);
        
        // This would cause an error - uncomment to try:
        // PI = 3.14;  // Error! Cannot reassign const

        // Block scope demonstration
        let outsideBlock = "I am outside";
        
        if (true) {
            let insideBlock = "I am inside the block";
            var functionScoped = "I am function scoped";
            console.log(insideBlock);  // Works here
            console.log(functionScoped);  // Works here
        }
        
        console.log(outsideBlock);  // Works
        // console.log(insideBlock);  // Error! let is block-scoped
        console.log(functionScoped);  // Works - var is function-scoped

        // Display results on page
        let outputText = "Check console for variable demonstrations.<br>";
        outputText += "Notice how const cannot be changed after setting!";
        document.getElementById("output").innerHTML = outputText;
    </script>
</body>
</html>
```

## Code Explanation

- **`var oldWay`**: The old keyword for declaring variables. It has function scope (not block scope), which can cause unexpected behavior.
- **`let message`**: Modern way to declare variables that can change. Can be reassigned to new values.
- **`const PI`**: Declares a constant value that cannot be changed. Useful for values that should never change like mathematical constants.
- **Block scope**: The `if` block demonstrates how `let` variables only exist inside the curly braces, while `var` variables are accessible outside the block.
- **Reassignment**: `message = "Hello World"` shows how `let` variables can be changed, but trying to reassign `const` would cause an error.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading "JavaScript Variables"
2. Open the browser console (F12 > Console tab)
3. You see these logged values:
   - "var: This is var"
   - "Initial let: Hello"
   - "Reassigned let: Hello World"
   - "const PI: 3.14159"
   - "I am inside the block"
   - "I am function scoped"
   - "I am outside"
   - "I am function scoped"
4. The webpage displays the text explaining that const cannot be changed after setting
