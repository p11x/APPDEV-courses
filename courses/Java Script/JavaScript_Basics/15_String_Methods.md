# String Methods

## What are String Methods?

String methods are built-in functions that let you manipulate and work with text (strings) in JavaScript. Strings are one of the most commonly used data types, and string methods make it easy to transform, search, and extract parts of text.

These methods are essential for form validation, text processing, and building user interfaces.

## Key Bullet Points

- **length**: Property that returns the number of characters in a string
- **toUpperCase()**: Converts all characters to uppercase
- **toLowerCase()**: Converts all characters to lowercase
- **substring(start, end)**: Extracts part of a string from start to end (exclusive)
- **indexOf(search)**: Returns the position of the first occurrence, or -1 if not found
- **trim()**: Removes whitespace from both ends of a string

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript String Methods</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #e0f2f1; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #009688;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #009688;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JavaScript String Methods</h1>
    
    <button onclick="showLength()">length Property</button>
    <button onclick="showCaseMethods()">Case Methods</button>
    <button onclick="showSubstring()">substring()</button>
    <button onclick="showTrim()">trim()</button>
    
    <div id="output">Click a button to see string methods in action!</div>

    <script>
        let text = "  Hello, World!  ";

        // Button handler - length property
        function showLength() {
            let result = "Original: '" + text + "'<br>";
            result += "text.length: " + text.length + " characters";
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - case conversion methods
        function showCaseMethods() {
            let original = "Hello World";
            let result = "Original: " + original + "<br><br>";
            
            result += "toUpperCase(): " + original.toUpperCase() + "<br>";
            result += "toLowerCase(): " + original.toLowerCase();
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - substring method
        function showSubstring() {
            let text = "Hello World";
            let result = "Original: " + text + "<br><br>";
            
            result += "substring(0, 5): " + text.substring(0, 5) + "<br>";
            result += "substring(6): " + text.substring(6) + "<br>";
            result += "substring(6, 11): " + text.substring(6, 11);
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - trim method
        function showTrim() {
            let text = "   Hello World   ";
            let result = "Before trim (with brackets): ['" + text + "']<br>";
            result += "text.length: " + text.length + "<br><br>";
            
            let trimmed = text.trim();
            result += "After trim: ['" + trimmed + "']<br>";
            result += "trimmed.length: " + trimmed.length;
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **length**: Returns the total number of characters including spaces. `"Hello".length` returns 5.
- **toUpperCase()**: Converts every character to capital letters. "hello" becomes "HELLO".
- **toLowerCase()**: Converts every character to lowercase. "HELLO" becomes "hello".
- **substring(start, end)**: Extracts characters from position `start` to `end-1`. If you omit end, it goes to the end of the string.
- **trim()**: Removes whitespace (spaces, tabs, newlines) from the beginning and end of a string. Very useful for cleaning user input.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and four buttons
2. Click "length Property" - shows "  Hello, World!  " has 17 characters (including spaces and exclamation mark)
3. Click "Case Methods" - shows:
   - toUpperCase(): HELLO WORLD
   - toLowerCase(): hello world
4. Click "substring()" - shows:
   - substring(0, 5): Hello (characters 0-4)
   - substring(6): World (from position 6 to end)
   - substring(6, 11): World (positions 6-10)
5. Click "trim()" - shows text before and after trimming, demonstrating length reduction from 19 to 11
