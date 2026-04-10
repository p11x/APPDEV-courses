# 🎓 Your First JavaScript Program

## 📋 Overview

Time to write your first real JavaScript code! This guide will walk you through creating, running, and debugging your initial JavaScript programs.

---

## 👋 Hello World Program

### Basic Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Hello World</title>
</head>
<body>
    <h1>Hello, World!</h1>
    
    <script>
        console.log("Hello, World!");
    </script>
</body>
</html>
```

### With External File

```html
<!-- index.html -->
<!DOCTYPE html>
<html>
<head>
    <title>Hello World</title>
</head>
<body>
    <h1 id="message"></h1>
    <script src="app.js"></script>
</body>
</html>
```

```javascript
// app.js
const messageElement = document.getElementById('message');
messageElement.textContent = "Hello from JavaScript!";
```

---

## 🔄 Interactive Hello World

### Example 1: Click Handler

```html
<!DOCTYPE html>
<html>
<head>
    <title>Interactive Hello</title>
    <style>
        button { padding: 15px 30px; font-size: 18px; cursor: pointer; }
        #output { font-size: 24px; margin: 20px 0; color: green; }
    </style>
</head>
<body>
    <button id="greetBtn">Click Me!</button>
    <div id="output"></div>
    
    <script>
        const button = document.getElementById('greetBtn');
        const output = document.getElementById('output');
        
        button.addEventListener('click', () => {
            output.textContent = "Hello, JavaScript! 🎉";
        });
    </script>
</body>
</html>
```

### Example 2: Prompt Input

```html
<!DOCTYPE html>
<html>
<body>
    <button onclick="sayHello()">Enter Your Name</button>
    <h2 id="greeting"></h2>
    
    <script>
        function sayHello() {
            const name = prompt("What is your name?");
            
            if (name) {
                document.getElementById('greeting').textContent = 
                    `Hello, ${name}! Welcome to JavaScript!`;
            }
        }
    </script>
</body>
</html>
```

### Example 3: Mouse Events

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        .box {
            width: 200px;
            height: 200px;
            background: #3498db;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 20px;
            cursor: pointer;
            transition: background 0.3s;
        }
        .box:hover { background: #2980b9; }
    </style>
</head>
<body>
    <div class="box" id="hoverBox">Hover me!</div>
    
    <script>
        const box = document.getElementById('hoverBox');
        
        box.addEventListener('mouseenter', () => {
            box.textContent = "Mouse Entered!";
            box.style.background = "#e74c3c";
        });
        
        box.addEventListener('mouseleave', () => {
            box.textContent = "Hover me!";
            box.style.background = "#3498db";
        });
        
        box.addEventListener('click', () => {
            box.textContent = "Clicked! 🎉";
        });
    </script>
</body>
</html>
```

---

## 🧮 Simple Calculator

### Basic Calculator

```html
<!DOCTYPE html>
<html>
<head>
    <title>Simple Calculator</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        input { padding: 10px; font-size: 16px; width: 100px; }
        button { padding: 10px 20px; font-size: 16px; margin: 5px; cursor: pointer; }
        #result { font-size: 24px; margin-top: 20px; color: #2c3e50; }
    </style>
</head>
<body>
    <h1>Simple Calculator</h1>
    
    <input type="number" id="num1" placeholder="First number">
    <input type="number" id="num2" placeholder="Second number">
    <br>
    <button onclick="calculate('add')">+</button>
    <button onclick="calculate('subtract')">-</button>
    <button onclick="calculate('multiply')">×</button>
    <button onclick="calculate('divide')">÷</button>
    
    <div id="result"></div>
    
    <script>
        function calculate(operation) {
            const num1 = parseFloat(document.getElementById('num1').value);
            const num2 = parseFloat(document.getElementById('num2').value);
            let result;
            
            if (isNaN(num1) || isNaN(num2)) {
                document.getElementById('result').textContent = "Please enter valid numbers";
                return;
            }
            
            switch(operation) {
                case 'add':
                    result = num1 + num2;
                    break;
                case 'subtract':
                    result = num1 - num2;
                    break;
                case 'multiply':
                    result = num1 * num2;
                    break;
                case 'divide':
                    if (num2 === 0) {
                        document.getElementById('result').textContent = "Cannot divide by zero!";
                        return;
                    }
                    result = num1 / num2;
                    break;
            }
            
            document.getElementById('result').textContent = "Result: " + result;
        }
    </script>
</body>
</html>
```

---

## 🎮 Number Guessing Game

### Complete Game

```html
<!DOCTYPE html>
<html>
<head>
    <title>Number Guessing Game</title>
    <style>
        body { font-family: Arial; text-align: center; padding: 50px; }
        input { padding: 10px; font-size: 18px; width: 100px; }
        button { padding: 10px 20px; font-size: 18px; cursor: pointer; }
        #message { font-size: 24px; margin: 20px 0; }
        .correct { color: green; }
        .wrong { color: red; }
    </style>
</head>
<body>
    <h1>🎯 Number Guessing Game</h1>
    <p>I'm thinking of a number between 1 and 100</p>
    
    <input type="number" id="guessInput" placeholder="Your guess">
    <button onclick="makeGuess()">Guess!</button>
    <button onclick="resetGame()">New Game</button>
    
    <div id="message"></div>
    <div id="attempts">Attempts: 0</div>
    
    <script>
        // Game state
        let randomNumber = Math.floor(Math.random() * 100) + 1;
        let attempts = 0;
        
        function makeGuess() {
            const input = document.getElementById('guessInput');
            const guess = parseInt(input.value);
            const message = document.getElementById('message');
            const attemptsDisplay = document.getElementById('attempts');
            
            if (!guess || guess < 1 || guess > 100) {
                message.textContent = "Please enter a number between 1 and 100";
                message.className = "wrong";
                return;
            }
            
            attempts++;
            attemptsDisplay.textContent = "Attempts: " + attempts;
            
            if (guess === randomNumber) {
                message.textContent = `🎉 Correct! The number was ${randomNumber}!`;
                message.className = "correct";
            } else if (guess < randomNumber) {
                message.textContent = "Too low! Try again.";
                message.className = "wrong";
            } else {
                message.textContent = "Too high! Try again.";
                message.className = "wrong";
            }
            
            input.value = "";
            input.focus();
        }
        
        function resetGame() {
            randomNumber = Math.floor(Math.random() * 100) + 1;
            attempts = 0;
            document.getElementById('message').textContent = "";
            document.getElementById('attempts').textContent = "Attempts: 0";
            document.getElementById('guessInput').value = "";
        }
        
        // Allow Enter key to submit
        document.getElementById('guessInput').addEventListener('keypress', (e) => {
            if (e.key === 'Enter') makeGuess();
        });
    </script>
</body>
</html>
```

---

## 🔍 Debugging Basics

### Using Console

```javascript
// Basic debugging
console.log("Variable value:", myVariable);

// Error logging
console.error("Something went wrong:", error);

// Warning
console.warn("This might cause issues");
```

### Using debugger

```javascript
function calculate(a, b) {
    debugger; // Execution pauses here in DevTools
    return a + b;
}
```

### Common Errors

```javascript
// ❌ Undefined variable
console.log(undefinedVariable); // ReferenceError

// ✅ Define first
let definedVariable = "Hello";
console.log(definedVariable); // "Hello"

// ❌ Missing semicolon (sometimes)
let x = 1
let y = 2
// ⚠️ ASI might cause issues

// ✅ Use semicolons
let x = 1;
let y = 2;

// ❌ Typos
const myVar = "hello";
console.log(myvarr); // ReferenceError

// ✅ Correct spelling
console.log(myVar);
```

---

## 🛠️ Practice Exercise

### Challenge 1: Temperature Converter

Create a temperature converter that:
- Has input field for temperature
- Has buttons for Celsius and Fahrenheit
- Shows converted temperature

```html
<!-- Starter code -->
<!DOCTYPE html>
<html>
<head>
    <title>Temperature Converter</title>
</head>
<body>
    <h1>🌡️ Temperature Converter</h1>
    <input type="number" id="tempInput" placeholder="Enter temperature">
    <button onclick="toCelsius()">To °C</button>
    <button onclick="toFahrenheit()">To °F</button>
    <h2 id="result"></h2>
    
    <script>
        // Your code here!
    </script>
</body>
</html>
```

### Challenge 2: Simple Counter

Create a counter that:
- Displays current count
- Has + and - buttons
- Has reset button

---

## 🎯 Key Takeaways

1. JavaScript runs in the browser with `<script>` tags
2. External files use `src` attribute
3. Use `document.getElementById()` to select elements
4. Use `addEventListener()` for interactions
5. Use `console.log()` for debugging

---

## 🔗 Related Topics

- [05_Output_Methods.md](./05_Output_Methods.md)
- [04_Syntax_and_Style.md](./04_Syntax_and_Style.md)

---

**Next: Learn about [Variables Deep Dive](../02_JAVASCRIPT_SYNTAX_AND_BASICS/04_Variables_Deep_Dive.md)**