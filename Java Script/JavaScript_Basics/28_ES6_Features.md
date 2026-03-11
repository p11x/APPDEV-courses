# ES6 Features

## What is ES6?

ES6 (ECMAScript 2015) was a major update to JavaScript that introduced many new features. These features make JavaScript more powerful and easier to write. Even though ES6 was released in 2015, it's now supported by all modern browsers and is considered the standard for modern JavaScript development.

ES6 features are essential for modern web development, especially when learning frameworks like Angular.

## Key Bullet Points

- **let and const**: Block-scoped variables (prefer const by default)
- **Template Literals**: Use backticks for string interpolation `${variable}`
- **Arrow Functions**: Shorter syntax with implicit return
- **Spread Operator**: `...` to expand arrays or objects
- **Destructuring**: Extract values from arrays/objects easily
- **Default Parameters**: Set default values in function parameters

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>ES6 Features</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #f3e5f5; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #9c27b0;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #9c27b0;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>ES6 Features</h1>
    
    <button onclick="showLetConst()">let and const</button>
    <button onclick="showTemplateLiterals()">Template Literals</button>
    <button onclick="showSpreadOperator()">Spread Operator</button>
    <button onclick="showDestructuring()">Destructuring</button>

    <div id="output">Click a button to see ES6 features in action!</div>

    <script>
        // let and const
        function showLetConst() {
            // const - cannot be reassigned
            const PI = 3.14159;
            
            // let - can be reassigned
            let count = 5;
            count = 10;  // OK
            
            let result = "const PI: " + PI + "<br>";
            result += "let count (changed): " + count;
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Template Literals
        function showTemplateLiterals() {
            let name = "Alice";
            let age = 25;
            
            // Old way
            let oldWay = "My name is " + name + " and I am " + age + " years old.";
            
            // ES6 way (template literals)
            let newWay = `My name is ${name} and I am ${age} years old.`;
            
            // Multi-line
            let multiLine = `
                Name: ${name}
                Age: ${age}
            `;
            
            let result = "Old way: " + oldWay + "<br><br>";
            result += "Template literal: " + newWay + "<br><br>";
            result += "Multi-line: <pre>" + multiLine + "</pre>";
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Spread Operator
        function showSpreadOperator() {
            let fruits = ["apple", "banana"];
            let moreFruits = ["orange", "mango"];
            
            // Combine arrays
            let allFruits = [...fruits, ...moreFruits];
            
            // Copy array
            let fruitsCopy = [...fruits];
            
            // With objects
            let person = { name: "John", age: 30 };
            let employee = { ...person, salary: 50000 };
            
            let result = "Original fruits: " + JSON.stringify(fruits) + "<br>";
            result += "Combined: " + JSON.stringify(allFruits) + "<br><br>";
            result += "Person: " + JSON.stringify(person) + "<br>";
            result += "Employee: " + JSON.stringify(employee);
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Destructuring
        function showDestructuring() {
            // Array destructuring
            let colors = ["red", "green", "blue"];
            let [first, second, third] = colors;
            
            // Object destructuring
            let user = { name: "Bob", city: "Paris", age: 28 };
            let { name, city } = user;
            
            // Function parameter destructuring
            function greet({ name, city }) {
                return `Hello ${name} from ${city}!`;
            }
            
            let result = "Array destructuring:<br>";
            result += "first: " + first + ", second: " + second + ", third: " + third + "<br><br>";
            result += "Object destructuring:<br>";
            result += "name: " + name + ", city: " + city + "<br><br>";
            result += "Function: " + greet(user);
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **let/const**: Modern variable declarations. Use const by default, let when you need to reassign.
- **Template literals**: Use backticks (\`) instead of quotes. Insert variables with `${variable}`.
- **Spread operator**: `...array` expands an array into individual elements. Great for copying and combining arrays/objects.
- **Destructuring**: Extract values from arrays/objects into variables. `[a, b] = [1, 2]` creates a=1, b=2.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and four buttons
2. Click "let and const" - shows const PI value and let count being reassigned
3. Click "Template Literals" - compares old string concatenation with template literals
4. Click "Spread Operator" - shows combining arrays and objects with ...
5. Click "Destructuring" - shows extracting values from arrays and objects
