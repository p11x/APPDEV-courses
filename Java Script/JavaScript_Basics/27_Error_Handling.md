# Error Handling

## What is Error Handling?

Error handling is how JavaScript deals with problems that occur while your code is running. Instead of the program crashing, you can catch errors and handle them gracefully. This makes your applications more robust and provides better user experiences.

Good error handling prevents unexpected crashes and helps developers debug issues.

## Key Bullet Points

- **try block**: Contains code that might cause an error
- **catch block**: Handles the error if one occurs
- **finally block**: Runs whether or not an error occurred
- **throw**: Manually create your own errors
- **Error types**: SyntaxError, TypeError, ReferenceError, RangeError
- **try/catch**: Essential for async operations and external data

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>Error Handling</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #ffebee; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #f44336;
        }
        .success {
            background: #e8f5e9;
            border-left-color: #4caf50;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #f44336;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>Error Handling</h1>
    
    <button onclick="tryCatchExample()">Try/Catch</button>
    <button onclick="throwError()">Throw Error</button>
    <button onclick="finallyExample()">Finally Block</button>
    <button onclick="handleError()">Handle Type Error</button>

    <div id="output">Click a button to see error handling in action!</div>

    <script>
        // Basic try/catch
        function tryCatchExample() {
            try {
                let result = 10 / 0;
                let num = parseInt("hello");  // This will cause NaN
                
                if (isNaN(num)) {
                    throw new Error("Cannot convert 'hello' to a number");
                }
                
                document.getElementById("output").innerHTML = 
                    "<div class='result success'>No errors! Result: " + num + "</div>";
                    
            } catch (error) {
                document.getElementById("output").innerHTML = 
                    "<div class='result'>Error caught: " + error.message + "</div>";
            }
        }

        // Throwing custom errors
        function throwError() {
            let age = -5;
            
            try {
                if (age < 0) {
                    throw new Error("Age cannot be negative!");
                }
                if (age > 150) {
                    throw new Error("Age seems unrealistic!");
                }
                
                document.getElementById("output").innerHTML = 
                    "<div class='result success'>Age is valid: " + age + "</div>";
                    
            } catch (error) {
                document.getElementById("output").innerHTML = 
                    "<div class='result'>Validation Error: " + error.message + "</div>";
            }
        }

        // Finally block
        function finallyExample() {
            let output = "Starting function...<br>";
            
            try {
                let x = 5;
                let y = 10;
                output += "x + y = " + (x + y) + "<br>";
            } catch (error) {
                output += "Error: " + error.message + "<br>";
            } finally {
                output += "<strong>Finally block always runs!</strong>";
            }
            
            document.getElementById("output").innerHTML = 
                "<div class='result'>" + output + "</div>";
        }

        // Handling specific errors
        function handleError() {
            let obj = null;
            
            try {
                // This will throw TypeError
                let value = obj.property;
            } catch (error) {
                let errorType = error.name;
                let message = error.message;
                
                document.getElementById("output").innerHTML = 
                    "<div class='result'>Error Type: " + errorType + "<br>Message: " + message + "</div>";
            }
        }
    </script>
</body>
</html>
```

## Code Explanation

- **try**: Contains code that might fail. JavaScript watches for any errors.
- **catch(error)**: Runs if an error occurs. The error object contains information about what went wrong.
- **error.message**: The human-readable error message.
- **error.name**: The type of error (SyntaxError, TypeError, etc.).
- **throw**: Creates a custom error. Used for validation and custom business logic.
- **finally**: Always runs - cleanup code that should run whether there was an error or not.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and four buttons
2. Click "Try/Catch" - catches the error from trying to parse "hello" as a number
3. Click "Throw Error" - shows validation error because age is negative
4. Click "Finally Block" - shows the message including the finally block running
5. Click "Handle Type Error" - catches and displays the TypeError from accessing a null object's property
