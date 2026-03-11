# Number Methods

## What are Number Methods?

Number methods are built-in functions that help you work with numbers in JavaScript. They are essential for mathematical calculations, currency formatting, and converting between different number formats. Since JavaScript handles numbers in specific ways, these methods help ensure accurate results.

These methods are particularly useful for financial applications, measurements, and any situation requiring precise decimal control.

## Key Bullet Points

- **toFixed(digits)**: Rounds a number to specified decimal places, returns a string
- **parseInt(string)**: Converts a string to an integer (whole number)
- **parseFloat(string)**: Converts a string to a decimal number
- **Number()**: Converts a value to a number
- **isNaN()**: Checks if a value is "Not a Number"
- All number methods are called directly on numbers or with the Number object

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Number Methods</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #e8eaf6; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #3f51b5;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #3f51b5;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JavaScript Number Methods</h1>
    
    <button onclick="showToFixed()">toFixed()</button>
    <button onclick="showParseInt()">parseInt()</button>
    <button onclick="showParseFloat()">parseFloat()</button>
    <button onclick="showNumber()">Number()</button>
    
    <div id="output">Click a button to see number methods in action!</div>

    <script>
        // Button handler - toFixed method
        function showToFixed() {
            let pi = 3.14159265;
            let result = "Original: " + pi + "<br><br>";
            
            result += "toFixed(0): " + pi.toFixed(0) + "<br>";
            result += "toFixed(2): " + pi.toFixed(2) + "<br>";
            result += "toFixed(4): " + pi.toFixed(4) + "<br><br>";
            
            // For currency
            let price = 19.99;
            result += "Price $19.99 with toFixed(2): $" + price.toFixed(2);
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - parseInt method
        function showParseInt() {
            let result = "parseInt() converts strings to integers:<br><br>";
            
            result += "parseInt('42'): " + parseInt('42') + "<br>";
            result += "parseInt('3.14'): " + parseInt('3.14') + "<br>";
            result += "parseInt('Hello'): " + parseInt('Hello') + "<br>";
            result += "parseInt('100px'): " + parseInt('100px') + "<br><br>";
            
            result += "Type check:<br>";
            result += "typeof parseInt('42'): " + typeof parseInt('42');
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - parseFloat method
        function showParseFloat() {
            let result = "parseFloat() converts strings to decimals:<br><br>";
            
            result += "parseFloat('3.14'): " + parseFloat('3.14') + "<br>";
            result += "parseFloat('42'): " + parseFloat('42') + "<br>";
            result += "parseFloat('3.14abc'): " + parseFloat('3.14abc') + "<br>";
            result += "parseFloat('abc'): " + parseFloat('abc') + "<br><br>";
            
            // Useful for user input
            let userInput = "25.5";
            let converted = parseFloat(userInput);
            result += "User input '" + userInput + "' converted: " + converted;
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - Number method
        function showNumber() {
            let result = "Number() converts values to numbers:<br><br>";
            
            result += "Number('42'): " + Number('42') + "<br>";
            result += "Number('3.14'): " + Number('3.14') + "<br>";
            result += "Number('true'): " + Number(true) + "<br>";
            result += "Number('false'): " + Number(false) + "<br>";
            result += "Number(''): " + Number('') + "<br>";
            result += "Number('hello'): " + Number('hello') + "<br>";
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **toFixed(2)**: Rounds 3.14159265 to 2 decimal places, returning "3.14". Returns a string, not a number!
- **parseInt()**: Extracts the integer part from a string. "3.14" becomes 3. Stops at first non-numeric character.
- **parseFloat()**: Similar to parseInt but keeps decimal places. "3.14" stays 3.14.
- **Number()**: More strict conversion. Only works if the entire string is a valid number.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and four buttons
2. Click "toFixed()" - shows:
   - pi toFixed(0): 3
   - pi toFixed(2): 3.14
   - pi toFixed(4): 3.1416
   - Price formatting example
3. Click "parseInt()" - shows converting various strings to integers (whole numbers)
4. Click "parseFloat()" - shows converting strings to decimal numbers
5. Click "Number()" - shows converting different values to numbers, including boolean conversion (true=1, false=0)
