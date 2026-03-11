# Data Types

## What are Data Types?

Data types classify the kind of data that can be stored and manipulated in a program. JavaScript has several built-in data types that determine what operations can be performed on values. Understanding data types helps you write correct and efficient code.

JavaScript is a dynamically typed language, meaning you don't have to specify the data type when declaring variables - the browser figures it out automatically based on the value you assign.

## Key Bullet Points

- **String**: Text data enclosed in quotes - "Hello" or 'Hello'
- **Number**: Both integers and decimals - 42, 3.14, -10
- **Boolean**: True or false values
- **Undefined**: A variable declared but not assigned a value
- **Null**: Intentionally empty or no value
- **typeof**: Operator that tells you the data type of a value

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Data Types</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { background: #f0f0f0; padding: 10px; margin: 5px 0; }
    </style>
</head>
<body>
    <h1>JavaScript Data Types</h1>
    <div id="output"></div>

    <script>
        // Primitive Data Types

        // String - text in quotes
        let studentName = "John Doe";
        console.log("String:", studentName, "Type:", typeof studentName);

        // Number - integers and decimals
        let studentAge = 25;
        let studentGPA = 3.75;
        console.log("Number:", studentAge, "Type:", typeof studentAge);
        console.log("Number:", studentGPA, "Type:", typeof studentGPA);

        // Boolean - true or false
        let isEnrolled = true;
        let hasGraduated = false;
        console.log("Boolean:", isEnrolled, "Type:", typeof isEnrolled);

        // Undefined - declared but not assigned
        let unknownValue;
        console.log("Undefined:", unknownValue, "Type:", typeof unknownValue);

        // Null - intentionally empty
        let emptyValue = null;
        console.log("Null:", emptyValue, "Type:", typeof emptyValue);  // Note: returns "object"

        // Reference Types

        // Array - ordered list of values
        let courses = ["HTML", "CSS", "JavaScript"];
        console.log("Array:", courses, "Type:", typeof courses);

        // Object - collection of key-value pairs
        let student = {
            name: "Alice",
            age: 22,
            major: "Computer Science"
        };
        console.log("Object:", student, "Type:", typeof student);

        // Display results on the page
        let output = "";
        output += "<div class='result'><strong>String:</strong> " + studentName + " - " + typeof studentName + "</div>";
        output += "<div class='result'><strong>Number:</strong> " + studentAge + " - " + typeof studentAge + "</div>";
        output += "<div class='result'><strong>Boolean:</strong> " + isEnrolled + " - " + typeof isEnrolled + "</div>";
        output += "<div class='result'><strong>Undefined:</strong> " + unknownValue + " - " + typeof unknownValue + "</div>";
        output += "<div class='result'><strong>Null:</strong> " + emptyValue + " - " + typeof emptyValue + " (bug: returns object)</div>";
        output += "<div class='result'><strong>Array:</strong> " + courses + " - " + typeof courses + "</div>";
        output += "<div class='result'><strong>Object:</strong> " + JSON.stringify(student) + " - " + typeof student + "</div>";

        document.getElementById("output").innerHTML = output;
    </script>
</body>
</html>
```

## Code Explanation

- **String**: Created with `let studentName = "John Doe"` - text wrapped in double quotes. Can also use single quotes.
- **Number**: Both whole numbers (`25`) and decimals (`3.75`) are the same data type in JavaScript.
- **Boolean**: `true` and `false` (no quotes - these are keywords). Used for logical operations.
- **Undefined**: Variable declared but not given a value - automatically assigned `undefined`.
- **Null**: Deliberately set to nothing. Note: `typeof null` returns "object" - this is a known JavaScript bug!
- **Array**: Created with square brackets `[]`. Contains an ordered list of items.
- **Object**: Created with curly braces `{}`. Contains key-value pairs for grouping related data.
- **`typeof`**: Operator that returns a string indicating the data type.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading "JavaScript Data Types"
2. A list of data type demonstrations appears showing:
   - String: John Doe - string
   - Number: 25 - number
   - Boolean: true - boolean
   - Undefined: undefined - undefined
   - Null: null - object (note the bug!)
   - Array: HTML,CSS,JavaScript - object
   - Object: {"name":"Alice","age":22,"major":"Computer Science"} - object
3. Open console to see the same information logged there as well
