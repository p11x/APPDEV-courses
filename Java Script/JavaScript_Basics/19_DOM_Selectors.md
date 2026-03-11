# DOM Selectors

## What are DOM Selectors?

DOM selectors are methods that find and retrieve HTML elements from the document so JavaScript can work with them. Different selectors help you find elements in different ways - by ID, class, tag name, or using CSS-style selectors. Knowing which selector to use is essential for manipulating the right elements.

The right selector makes your code more efficient and easier to read.

## Key Bullet Points

- **getElementById()**: Finds one element by its unique ID
- **getElementsByClassName()**: Finds all elements with a specific class (returns a collection)
- **getElementsByTagName()**: Finds all elements of a specific tag (returns a collection)
- **querySelector()**: Finds the first element matching a CSS selector
- **querySelectorAll()**: Finds all elements matching a CSS selector
- Collections are array-like but not actual arrays - convert with Array.from() if needed

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>DOM Selectors</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .demo { 
            background: #e8f5e9; 
            padding: 15px; 
            margin: 5px 0;
            border-left: 4px solid #4caf50;
        }
        .highlight {
            background: yellow;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #4caf50;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>DOM Selectors</h1>
    
    <p id="intro">This is an intro paragraph with ID "intro".</p>
    <p class="demo">First paragraph with class "demo".</p>
    <p class="demo">Second paragraph with class "demo".</p>
    <p>Regular paragraph without any special class or ID.</p>

    <button onclick="demoGetElementById()">getElementById</button>
    <button onclick="demoGetElementsByClassName()">getElementsByClassName</button>
    <button onclick="demoGetElementsByTagName()">getElementsByTagName</button>
    <button onclick="demoQuerySelector()">querySelector</button>
    <button onclick="demoQuerySelectorAll()">querySelectorAll</button>

    <div id="output"></div>

    <script>
        // Button handler - getElementById
        function demoGetElementById() {
            let element = document.getElementById("intro");
            document.getElementById("output").innerHTML = 
                "<div class='demo'>Found by ID: " + element.textContent + "</div>";
        }

        // Button handler - getElementsByClassName
        function demoGetElementsByClassName() {
            let elements = document.getElementsByClassName("demo");
            let result = "Found " + elements.length + " elements with class 'demo':<br>";
            for (let i = 0; i < elements.length; i++) {
                result += (i + 1) + ". " + elements[i].textContent + "<br>";
            }
            document.getElementById("output").innerHTML = "<div class='demo'>" + result + "</div>";
        }

        // Button handler - getElementsByTagName
        function demoGetElementsByTagName() {
            let elements = document.getElementsByTagName("p");
            let result = "Found " + elements.length + " <p> elements:<br>";
            for (let i = 0; i < elements.length; i++) {
                result += "p[" + i + "]: " + elements[i].textContent.substring(0, 30) + "...<br>";
            }
            document.getElementById("output").innerHTML = "<div class='demo'>" + result + "</div>";
        }

        // Button handler - querySelector
        function demoQuerySelector() {
            let element = document.querySelector(".demo");
            document.getElementById("output").innerHTML = 
                "<div class='demo'>First element with .demo: " + element.textContent + "</div>";
        }

        // Button handler - querySelectorAll
        function demoQuerySelectorAll() {
            let elements = document.querySelectorAll(".demo");
            let result = "Found " + elements.length + " elements with .demo:<br>";
            elements.forEach((el, index) => {
                result += (index + 1) + ". " + el.textContent + "<br>";
            });
            document.getElementById("output").innerHTML = "<div class='demo'>" + result + "</div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **getElementById("intro")**: Returns a single element with id="intro". Returns null if not found.
- **getElementsByClassName("demo")**: Returns an HTMLCollection (like an array) of all elements with class="demo".
- **getElementsByTagName("p")**: Returns all paragraph elements. Works with any HTML tag.
- **querySelector(".demo")**: Returns the first element matching the CSS selector ".demo".
- **querySelectorAll(".demo")**: Returns a NodeList of all matching elements. Supports forEach.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading, intro paragraph, two paragraphs with class "demo", and one regular paragraph
2. Click "getElementById" - shows "Found by ID: This is an intro paragraph..."
3. Click "getElementsByClassName" - shows there are 2 elements with class "demo" and lists them
4. Click "getElementsByTagName" - shows there are 4 paragraph elements
5. Click "querySelector" - shows the first element with class "demo"
6. Click "querySelectorAll" - shows all elements with class "demo"
