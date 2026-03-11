# DOM Manipulation

## What is DOM Manipulation?

DOM manipulation involves changing the content, attributes, and styles of HTML elements using JavaScript. This is what makes web pages dynamic - you can update text, show/hide elements, change colors, modify attributes, and more in response to user actions.

DOM manipulation is the foundation of interactive web applications.

## Key Bullet Points

- **textContent**: Changes only the text (not HTML tags)
- **innerHTML**: Changes HTML content including tags
- **style property**: Modifies CSS styles inline
- **setAttribute()**: Sets HTML attributes like src, href, class
- **getAttribute()**: Gets attribute values
- **classList**: Add, remove, or toggle CSS classes

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>DOM Manipulation</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .box {
            background: #e0e0e0;
            padding: 20px;
            margin: 10px 0;
            border: 2px solid #333;
        }
        .highlight {
            background: yellow;
            font-weight: bold;
        }
        .hidden {
            display: none;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #673ab7;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>DOM Manipulation</h1>
    
    <div id="myBox" class="box">
        <h2 id="boxTitle">Original Title</h2>
        <p id="boxContent">Original content goes here.</p>
    </div>

    <button onclick="changeText()">Change Text</button>
    <button onclick="changeHTML()">Change HTML</button>
    <button onclick="changeStyles()">Change Styles</button>
    <button onclick="changeAttributes()">Change Attributes</button>
    <button onclick="toggleClass()">Toggle Class</button>

    <script>
        // Button handler - textContent
        function changeText() {
            document.getElementById("boxTitle").textContent = "Text Changed!";
            document.getElementById("boxContent").textContent = "This was changed using textContent.";
        }

        // Button handler - innerHTML
        function changeHTML() {
            document.getElementById("boxTitle").innerHTML = "<em>HTML Changed!</em>";
            document.getElementById("boxContent").innerHTML = "This has <strong>bold</strong> and <u>underlined</u> text.";
        }

        // Button handler - style
        function changeStyles() {
            let box = document.getElementById("myBox");
            box.style.backgroundColor = "#ffccbc";
            box.style.borderColor = "#ff5722";
            box.style.borderRadius = "10px";
            document.getElementById("boxTitle").style.color = "#d84315";
        }

        // Button handler - attributes
        function changeAttributes() {
            let box = document.getElementById("myBox");
            box.setAttribute("data-info", "This is custom data");
            box.setAttribute("title", "This is a tooltip");
            
            alert("Added data-info and title attributes!\nCheck the box with your mouse.");
        }

        // Button handler - classList
        function toggleClass() {
            let box = document.getElementById("myBox");
            box.classList.toggle("highlight");
            
            let isHighlighted = box.classList.contains("highlight");
            document.getElementById("boxContent").textContent = 
                "highlight class: " + (isHighlighted ? "ADDED" : "REMOVED");
        }
    </script>
</body>
</html>
```

## Code Explanation

- **textContent**: Replaces the text inside an element. Any HTML tags are treated as plain text, not rendered.
- **innerHTML**: Replaces the HTML inside an element. HTML tags are parsed and rendered.
- **style**: Accesses inline CSS. Properties use camelCase (backgroundColor, not background-color).
- **setAttribute(name, value)**: Sets any attribute on an element, including custom attributes (data-*).
- **classList**: Provides methods to work with CSS classes: add(), remove(), toggle(), contains().

## Expected Output

When you open this HTML file in a browser:
1. You see a gray box with "Original Title" and "Original content goes here."
2. Click "Change Text" - text changes to "Text Changed!" and new content
3. Click "Change HTML" - title becomes italic, content has bold and underlined text
4. Click "Change Styles" - box turns orange/peach, has rounded corners
5. Click "Change Attributes" - shows alert, then hovering over the box shows a tooltip
6. Click "Toggle Class" - toggles yellow highlight on/off
