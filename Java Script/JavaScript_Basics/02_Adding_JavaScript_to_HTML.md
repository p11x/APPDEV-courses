# Adding JavaScript to HTML

## What is Adding JavaScript to HTML?

Adding JavaScript to HTML means embedding or linking JavaScript code within an HTML document so that the browser can execute it. There are three main ways to add JavaScript: inline (within HTML elements), internal (within script tags in the page), and external (in separate .js files linked to the page).

Understanding how to properly add JavaScript to HTML is crucial because it affects how your code is loaded and executed. Each method has its use cases, and knowing when to use each approach makes you a better web developer.

## Key Bullet Points

- **Inline JavaScript**: Placed directly inside HTML event attributes like `onclick`
- **Internal JavaScript**: Written inside `<script>` tags in the HTML body or head
- **External JavaScript**: Stored in separate .js files and linked using `<script src="file.js">`
- External files are best for code organization and reusability across multiple pages
- Best practice: Place `<script>` tags just before the closing `</body>` tag for faster page loading
- The `defer` attribute can be used to load scripts after the HTML parses

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>Adding JavaScript Methods</title>
</head>
<body>
    <h1>Three Ways to Add JavaScript</h1>

    <!-- METHOD 1: Inline JavaScript -->
    <!-- JavaScript is placed directly in the onclick attribute -->
    <button onclick="alert('Inline JavaScript!')">Inline Button</button>

    <!-- METHOD 2: Internal JavaScript -->
    <!-- JavaScript is written inside script tags in the body -->
    <h2 id="internal-heading">Click to see internal JavaScript</h2>
    <button onclick="showInternalMessage()">Internal Button</button>

    <script>
        // This is internal JavaScript
        function showInternalMessage() {
            document.getElementById("internal-heading").innerText = "Internal JavaScript works!";
        }
    </script>

    <!-- METHOD 3: External JavaScript -->
    <!-- Links to an external file called "external.js" -->
    <h2 id="external-heading">Click to see external JavaScript</h2>
    <button onclick="showExternalMessage()">External Button</button>

    <script src="external.js"></script>
</body>
</html>
```

## Code Explanation

- **Inline JavaScript**: The first button uses `onclick="alert('Inline JavaScript!')"` - the JavaScript code is written directly in the HTML attribute. This is quick but not recommended for complex code.
- **Internal JavaScript**: The `<script>` tag contains a function `showInternalMessage()`. This runs when the second button is clicked, changing the heading text.
- **External JavaScript**: The `<script src="external.js">` tag links to an external file. You'd create a file named "external.js" with the function `showExternalMessage()` inside it.
- Placing scripts at the bottom (`</body>` before) ensures HTML loads first, so elements exist when JavaScript tries to access them.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading "Three Ways to Add JavaScript"
2. Three buttons labeled "Inline Button", "Internal Button", and "External Button" appear
3. Clicking "Inline Button" shows an alert popup saying "Inline JavaScript!"
4. Clicking "Internal Button" changes the heading below it to "Internal JavaScript works!"
5. Clicking "External Button" would work if you create the external.js file with the matching function
