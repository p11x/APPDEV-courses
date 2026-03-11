# Event Handling

## What is Event Handling?

Event handling allows JavaScript to respond to user actions like clicks, mouse movements, keyboard input, and more. When something happens (an event), you can run code to respond to it. This is what makes web pages interactive - buttons work, forms respond, and animations play.

Events are central to interactive web applications.

## Key Bullet Points

- **Events**: Actions that happen in the browser (click, mouseover, keypress, etc.)
- **Event Handler**: A function that runs when an event occurs
- **onclick attribute**: The old way to add event handlers in HTML
- **addEventListener()**: The modern, recommended way to add event handlers
- **removeEventListener()**: Removes an event handler
- Common events: click, mouseover, mouseout, keyup, submit, load

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>Event Handling</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .btn {
            padding: 15px 30px;
            margin: 10px;
            cursor: pointer;
            background: #e91e63;
            color: white;
            border: none;
            font-size: 16px;
        }
        .btn:hover {
            background: #c2185b;
        }
        .box {
            width: 200px;
            height: 100px;
            background: #e3f2fd;
            border: 2px solid #2196f3;
            margin: 20px 0;
            padding: 10px;
        }
        #output {
            margin-top: 20px;
            padding: 10px;
            background: #f5f5f5;
            min-height: 30px;
        }
    </style>
</head>
<body>
    <h1>Event Handling</h1>
    
    <button class="btn" onclick="handleClick()">Click Me (onclick)</button>
    <button class="btn" id="modernBtn">Click Me (addEventListener)</button>
    
    <div class="box" id="hoverBox">Hover over me!</div>
    
    <input type="text" id="textInput" placeholder="Type something...">
    <div id="output">Events will appear here...</div>

    <script>
        // Method 1: Using onclick attribute
        function handleClick() {
            document.getElementById("output").innerHTML = "Button clicked! (onclick method)";
        }

        // Method 2: Using addEventListener (recommended)
        let modernBtn = document.getElementById("modernBtn");
        modernBtn.addEventListener("click", function() {
            document.getElementById("output").innerHTML = "Button clicked! (addEventListener method)";
        });

        // Method 3: Mouse events on the box
        let box = document.getElementById("hoverBox");
        
        box.addEventListener("mouseover", function() {
            box.style.background = "#bbdefb";
            box.innerHTML = "Mouse is over!";
        });
        
        box.addEventListener("mouseout", function() {
            box.style.background = "#e3f2fd";
            box.innerHTML = "Hover over me!";
        });

        // Method 4: Keyboard event
        let input = document.getElementById("textInput");
        input.addEventListener("keyup", function(event) {
            let output = document.getElementById("output");
            output.innerHTML = "You typed: " + event.target.value;
            output.innerHTML += "<br>Key pressed: " + event.key;
        });
    </script>
</body>
</html>
```

## Code Explanation

- **onclick attribute**: Simple way to handle clicks - `<button onclick="functionName()">`. Good for quick examples.
- **addEventListener()**: The modern approach. `element.addEventListener("event", function)` attaches an event handler.
- **event parameter**: The function receives an event object with information about what happened.
- **mouseover/mouseout**: Events that fire when the mouse enters/leaves an element.
- **keyup**: Fires when a key is released while typing in an input field.
- **event.target**: The element that triggered the event.

## Expected Output

When you open this HTML file in a browser:
1. You see two pink buttons and a blue box, plus a text input
2. Click first button - shows "Button clicked! (onclick method)"
3. Click second button - shows "Button clicked! (addEventListener method)"
4. Hover over the blue box - it turns darker blue and says "Mouse is over!"
5. Move mouse away - box returns to original state
6. Type in the input field - shows what you typed and which key was pressed
