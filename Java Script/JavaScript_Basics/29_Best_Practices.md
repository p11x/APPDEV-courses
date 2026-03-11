# Best Practices

## What are Best Practices?

Best practices are guidelines that help you write clean, maintainable, and efficient JavaScript code. Following these practices makes your code easier to read, debug, and maintain. They also help prevent common mistakes and make collaboration with other developers easier.

Good coding habits from the start will make you a better developer.

## Key Bullet Points

- **Use meaningful names**: Variables and functions should clearly describe their purpose
- **Comment your code**: Explain complex logic, not obvious statements
- **Avoid global variables**: Use const/let and functions to scope variables
- **Use const by default**: Only use let when you need to reassign
- **Keep functions small**: Each function should do one thing well
- **Format consistently**: Use consistent indentation and spacing

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Best Practices</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .good, .bad { 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid;
        }
        .good {
            background: #e8f5e9;
            border-left-color: #4caf50;
        }
        .bad {
            background: #ffebee;
            border-left-color: #f44336;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #607d8b;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JavaScript Best Practices</h1>
    
    <button onclick="showNaming()">Meaningful Names</button>
    <button onclick="showScope()">Variable Scope</button>
    <button onclick="showFunctions()">Function Best Practices</button>

    <div id="output"></div>

    <script>
        // GOOD PRACTICE: Meaningful variable names
        function showNaming() {
            // BAD: Unclear names
            let x = 5;
            let d = new Date();
            
            // GOOD: Descriptive names
            let userAge = 5;
            let currentDate = new Date();
            
            let result = "<div class='bad'><strong>BAD:</strong><br>";
            result += "let x = 5; // What is x?<br>";
            result += "let d = new Date(); // What is d?</div>";
            
            result += "<div class='good'><strong>GOOD:</strong><br>";
            result += "let userAge = 5; // Clear!<br>";
            result += "let currentDate = new Date(); // Clear!</div>";
            
            document.getElementById("output").innerHTML = result;
        }

        // GOOD PRACTICE: Proper variable scope
        function showScope() {
            // BAD: Creating global variables
            // total = 0; // This pollutes global scope!
            
            // GOOD: Use const/let with proper scope
            function calculateTotal() {
                const items = [10, 20, 30];
                let total = 0;
                
                for (let i = 0; i < items.length; i++) {
                    total += items[i]; // Using const and let properly
                }
                
                return total;
            }
            
            let result = "<div class='bad'><strong>BAD:</strong><br>";
            result += "total = 0; // Creates global variable!<br>";
            result += "// Can conflict with other code</div>";
            
            result += "<div class='good'><strong>GOOD:</strong><br>";
            result += "function calculateTotal() {<br>";
            result += "&nbsp;&nbsp;const items = [10, 20, 30];<br>";
            result += "&nbsp;&nbsp;let total = 0; // Local scope<br>";
            result += "}</div>";
            
            document.getElementById("output").innerHTML = result;
        }

        // GOOD PRACTICE: Small, focused functions
        function showFunctions() {
            // BAD: One function doing too much
            // function processUser() { ... all logic here ... }
            
            // GOOD: Small, focused functions
            const getUserName = (user) => user.name;
            const formatUserName = (name) => name.trim().toUpperCase();
            const displayUser = (name) => `Hello, ${name}!`;
            
            let result = "<div class='bad'><strong>BAD:</strong><br>";
            result += "function processUser() {<br>";
            result += "&nbsp;&nbsp;// Does everything: validate, format, save<br>";
            result += "}</div>";
            
            result += "<div class='good'><strong>GOOD:</strong><br>";
            result += "const getUserName = (user) => user.name;<br>";
            result += "const formatUserName = (name) => name.trim().toUpperCase();<br>";
            result += "const displayUser = (name) => `Hello, ${name}!`;<br><br>";
            result += "// Each function does ONE thing</div>";
            
            document.getElementById("output").innerHTML = result;
        }
    </script>
</body>
</html>
```

## Code Explanation

- **Meaningful names**: Use names like `userAge` instead of `x`. Makes code self-documenting.
- **const by default**: Use const for values that won't change. Only use let when you need to reassign. Avoid var.
- **Block scope**: Variables declared with let/const are only available in their block (between {}).
- **Small functions**: Each function should do one thing. Makes code easier to test and maintain.
- **Comments**: Explain WHY you did something, not WHAT the code does (the code shows what).

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and three buttons
2. Click "Meaningful Names" - shows bad examples with unclear names vs good examples with descriptive names
3. Click "Variable Scope" - shows how using const/let properly keeps variables in local scope
4. Click "Function Best Practices" - shows how to split code into small, focused functions
