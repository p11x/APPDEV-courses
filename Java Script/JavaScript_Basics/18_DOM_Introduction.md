# DOM Introduction

## What is the DOM?

The Document Object Model (DOM) is a programming interface for web pages. It represents the HTML document as a tree of objects, allowing JavaScript to access and modify the content, structure, and style of a webpage dynamically. When a browser loads a webpage, it creates the DOM, which JavaScript can then manipulate.

The DOM is what makes websites interactive - without it, JavaScript wouldn't be able to change what's displayed on the page.

## Key Bullet Points

- **DOM**: A tree-like structure representing HTML elements as objects
- **document object**: The root object that represents the entire webpage
- **Element objects**: HTML tags like `<h1>`, `<p>`, `<button>` become JavaScript objects
- **Properties**: Attributes of elements that can be read or changed
- **Methods**: Actions you can perform on elements
- **Dynamic changes**: JavaScript can change HTML, CSS, and attributes in real-time

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>DOM Introduction</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .demo-box {
            background: #e3f2fd;
            padding: 20px;
            border: 2px solid #2196f3;
            margin: 10px 0;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #2196f3;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>DOM Introduction</h1>
    
    <div class="demo-box">
        <h2 id="heading">Welcome to DOM!</h2>
        <p id="paragraph">This paragraph can be changed by JavaScript.</p>
    </div>

    <button onclick="changeHeading()">Change Heading</button>
    <button onclick="changeParagraph()">Change Paragraph</button>
    <button onclick="addNewElement()">Add New Element</button>

    <script>
        // The document object represents the entire webpage
        // We can access any element through the document object

        // Button handler - change the heading
        function changeHeading() {
            // Get the heading element by its ID
            let heading = document.getElementById("heading");
            
            // Change the text content
            heading.textContent = "DOM is Amazing!";
            
            // Change the style
            heading.style.color = "red";
        }

        // Button handler - change the paragraph
        function changeParagraph() {
            let paragraph = document.getElementById("paragraph");
            
            // Change the HTML content
            paragraph.innerHTML = "JavaScript can <strong>modify</strong> HTML content!";
            
            // Change background color
            paragraph.style.backgroundColor = "yellow";
            paragraph.style.padding = "10px";
        }

        // Button handler - add a new element
        function addNewElement() {
            // Create a new paragraph element
            let newP = document.createElement("p");
            
            // Set its content
            newP.textContent = "This is a brand new element added by JavaScript!";
            
            // Add it to the page
            document.body.appendChild(newP);
        }
    </script>
</body>
</html>
```

## Code Explanation

- **document object**: The main entry point to access the DOM. Everything starts with `document`.
- **getElementById()**: Finds an element by its ID attribute. This is the most common way to select elements.
- **textContent**: Property that gets or sets the text inside an element (ignores HTML tags).
- **innerHTML**: Property that gets or sets the HTML inside an element (parses HTML tags).
- **style object**: Accesses inline CSS styles. `element.style.color = "red"` changes the text color.
- **createElement()**: Creates a new HTML element in memory.
- **appendChild()**: Adds the new element as a child of another element, making it visible on the page.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading "Welcome to!", a paragraph saying "This paragraph can be changed by JavaScript."
2. Click "Change Heading" - the heading changes to "DOM is Amazing!" and becomes red
3. Click "Change Paragraph" - the paragraph text changes with bold formatting, and gets a yellow background
4. Click "Add New Element" - a new paragraph appears at the bottom of the page
