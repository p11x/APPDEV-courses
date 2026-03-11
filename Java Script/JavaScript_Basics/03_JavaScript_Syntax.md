# JavaScript Syntax

## What is JavaScript Syntax?

JavaScript syntax refers to the set of rules that define how JavaScript code must be written for the browser to understand and execute it. This includes how statements are formed, how semicolons are used, how the language handles case sensitivity, and how to write comments.

Understanding syntax is fundamental because even small mistakes like a missing semicolon or incorrect capitalization can cause your code to fail. Good syntax habits make your code readable and easier to debug.

## Key Bullet Points

- **Statements**: Individual instructions that the browser executes, usually ending with a semicolon
- **Semicolons**: Mark the end of each statement; JavaScript can work without them but they are recommended
- **Case Sensitivity**: JavaScript treats `myVariable`, `MyVariable`, and `MYVARIABLE` as three different things
- **Comments**: Lines ignored by the browser - use `//` for single-line and `/* */` for multi-line comments
- **Whitespace**: Extra spaces and line breaks are ignored; use them to make code readable
- **Curly Braces `{}`**: Group related statements together in blocks (like in functions and loops)

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Syntax</title>
</head>
<body>
    <h1>JavaScript Syntax Examples</h1>
    <p id="output">Check the browser console (F12) to see output.</p>

    <script>
        // This is a single-line comment - it explains the code but doesn't run

        /*
         * This is a multi-line comment
         * It can span multiple lines
         * Very useful for longer explanations
         */

        // Each statement ends with a semicolon
        var firstName = "John";    // String variable (text)
        var lastName = "Doe";      // Another string variable

        // JavaScript is case-sensitive - these are DIFFERENT variables
        var age = 25;              // lowercase 'age'
        var Age = 30;              // uppercase 'Age' - different variable!
        var AGE = 35;              // all caps 'AGE' - also different!

        // Display values in the console
        console.log("First Name:", firstName);
        console.log("Last Name:", lastName);
        console.log("age:", age, "Age:", Age, "AGE:", AGE);

        // Using curly braces to group statements
        if (age > 18) {
            console.log("Person is an adult");
        }
    </script>
</body>
</html>
```

## Code Explanation

- **Single-line comment (`//`)**: The browser ignores everything after `//` on that line. Used for short explanations.
- **Multi-line comment (`/* */`)**: Everything between `/*` and `*/` is ignored, even across multiple lines.
- **Variables**: `var firstName = "John"` creates a variable named `firstName` with the text "John". The semicolon ends this statement.
- **Case sensitivity demonstration**: Three separate variables are created: `age`, `Age`, and `AGE` - all holding different values (25, 30, 35).
- **Console.log**: Outputs information to the browser's developer console (press F12 to view).
- **Curly braces**: The `if` statement uses `{}` to group the statement that runs when the condition is true.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading "JavaScript Syntax Examples"
2. Open the browser console (right-click > Inspect > Console tab)
3. You see the following output:
   - "First Name: John"
   - "Last Name: Doe"
   - "age: 25 AGE: 35"
   - "Person is an adult" Age: 30 (because 25 > 18)
