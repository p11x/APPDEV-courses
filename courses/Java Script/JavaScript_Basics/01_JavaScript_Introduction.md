# JavaScript Introduction

## What is JavaScript?

JavaScript is a programming language that makes web pages interactive. It runs in web browsers and allows you to create dynamic content that responds to user actions. Without JavaScript, websites would be static and non-interactive.

In web development, JavaScript works alongside HTML and CSS to create complete web applications. HTML provides the structure (the skeleton), CSS handles the visual styling (the appearance), and JavaScript adds behavior and interactivity (the actions).

## Key Bullet Points

- JavaScript is the programming language of the web - it's supported by all modern browsers
- HTML (HyperText Markup Language) creates the structure of web pages - headings, paragraphs, images
- CSS (Cascading Style Sheets) styles and designs web pages - colors, fonts, layouts
- JavaScript adds interactivity - animations, form validation, dynamic content updates
- JavaScript can manipulate HTML elements after the page loads
- JavaScript runs on the client-side (in the browser), not on the server

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Introduction</title>
    <!-- CSS - Styles the page -->
    <style>
        h1 {
            color: blue;           /* Makes the heading blue */
            text-align: center;   /* Centers the heading */
        }
        p {
            font-size: 18px;      /* Sets paragraph font size */
        }
        button {
            padding: 10px 20px;   /* Adds space inside button */
            background-color: green;
            color: white;
            border: none;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <!-- HTML - Creates the structure -->
    <h1>Welcome to JavaScript!</h1>
    <p id="message">Click the button to see JavaScript in action.</p>
    <button onclick="changeMessage()">Click Me</button>

    <!-- JavaScript - Adds interactivity -->
    <script>
        // This function runs when the button is clicked
        function changeMessage() {
            // Get the paragraph element and change its text
            document.getElementById("message").innerText = "Hello! JavaScript changed this text!";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **HTML Section (`<body>`)**: Creates a heading, a paragraph with an ID, and a button. The paragraph has `id="message"` so JavaScript can find it later.
- **CSS Section (`<style>`)**: Styles the heading blue and centered, sets paragraph font size, and styles the button with green background.
- **JavaScript Section (`<script>`)**: Contains a function called `changeMessage()`. When the button is clicked (via `onclick="changeMessage()"`), JavaScript finds the paragraph using its ID and changes the text using `innerText`.

## Expected Output

When you open this HTML file in a browser:
1. You see a blue centered heading saying "Welcome to JavaScript!"
2. Below it, a paragraph says "Click the button to see JavaScript in action."
3. A green button labeled "Click Me" appears
4. When you click the button, the paragraph text changes to "Hello! JavaScript changed this text!"

This demonstrates how HTML provides structure, CSS provides styling, and JavaScript provides the interactive behavior.
