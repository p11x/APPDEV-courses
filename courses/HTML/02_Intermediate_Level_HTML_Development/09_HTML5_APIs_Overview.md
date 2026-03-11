# HTML5 APIs Overview

## Topic Title
Introduction to HTML5 APIs

## Concept Explanation

### What are HTML5 APIs?

HTML5 APIs are JavaScript interfaces that provide powerful capabilities for web applications. They extend what HTML elements can do.

### Key HTML5 APIs

1. **Canvas API** - Draw graphics dynamically
2. **Geolocation API** - Get user's location
3. **Web Storage** - Store data locally
4. **Drag and Drop** - Drag elements
5. **Web Workers** - Background processing
6. **History API** - Manage browser history

## Code Examples

### Example 1: Canvas API

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Canvas API Demo</title>
</head>
<body>
    <h1>Canvas Drawing</h1>
    <canvas id="myCanvas" width="400" height="200" style="border: 1px solid black;"></canvas>
    
    <script>
        const canvas = document.getElementById('myCanvas');
        const ctx = canvas.getContext('2d');
        
        // Draw rectangle
        ctx.fillStyle = 'blue';
        ctx.fillRect(10, 10, 150, 100);
        
        // Draw circle
        ctx.beginPath();
        ctx.arc(300, 60, 40, 0, Math.PI * 2);
        ctx.fillStyle = 'red';
        ctx.fill();
        
        // Draw text
        ctx.font = '20px Arial';
        ctx.fillStyle = 'black';
        ctx.fillText('Canvas Demo', 130, 180);
    </script>
</body>
</html>
```

### Example 2: Geolocation API

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Geolocation Demo</title>
</head>
<body>
    <h1>Get My Location</h1>
    <button onclick="getLocation()">Show My Location</button>
    <p id="location"></p>
    
    <script>
        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            } else {
                document.getElementById('location').innerHTML = "Geolocation is not supported by this browser.";
            }
        }
        
        function showPosition(position) {
            document.getElementById('location').innerHTML = 
                "Latitude: " + position.coords.latitude + 
                "<br>Longitude: " + position.coords.longitude;
        }
        
        function showError(error) {
            switch(error.code) {
                case error.PERMISSION_DENIED:
                    document.getElementById('location').innerHTML = "User denied the request for Geolocation.";
                    break;
                case error.POSITION_UNAVAILABLE:
                    document.getElementById('location').innerHTML = "Location information is unavailable.";
                    break;
                case error.TIMEOUT:
                    document.getElementById('location').innerHTML = "The request to get user location timed out.";
                    break;
            }
        }
    </script>
</body>
</html>
```

### Example 3: Web Storage API

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Web Storage Demo</title>
</head>
<body>
    <h1>Web Storage</h1>
    
    <div>
        <label>Name:</label>
        <input type="text" id="nameInput">
        <button onclick="saveData()">Save</button>
        <button onclick="loadData()">Load</button>
        <button onclick="clearData()">Clear</button>
    </div>
    
    <p id="output"></p>
    
    <script>
        function saveData() {
            const name = document.getElementById('nameInput').value;
            localStorage.setItem('username', name);
            alert('Saved: ' + name);
        }
        
        function loadData() {
            const name = localStorage.getItem('username');
            document.getElementById('output').innerHTML = 'Loaded: ' + name;
        }
        
        function clearData() {
            localStorage.removeItem('username');
            document.getElementById('output').innerHTML = 'Data cleared';
        }
    </script>
</body>
</html>
```

### Example 4: Drag and Drop

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Drag and Drop</title>
    <style>
        .draggable {
            width: 100px;
            height: 50px;
            background: #3498db;
            color: white;
            text-align: center;
            line-height: 50px;
            cursor: move;
            margin: 10px;
        }
        #dropZone {
            width: 300px;
            height: 150px;
            border: 2px dashed #333;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <h1>Drag and Drop</h1>
    <p>Drag the blue box to the dashed area:</p>
    
    <div class="draggable" draggable="true" id="drag1">Drag Me</div>
    
    <div id="dropZone">Drop Here</div>
    
    <script>
        const draggable = document.getElementById('drag1');
        const dropZone = document.getElementById('dropZone');
        
        draggable.addEventListener('dragstart', function(e) {
            e.dataTransfer.setData('text', e.target.id);
        });
        
        dropZone.addEventListener('dragover', function(e) {
            e.preventDefault();
        });
        
        dropZone.addEventListener('drop', function(e) {
            e.preventDefault();
            const data = e.dataTransfer.getData('text');
            const element = document.getElementById(data);
            dropZone.appendChild(element);
            dropZone.innerHTML = 'Dropped!';
        });
    </script>
</body>
</html>
```

## Best Practices

1. **Check for API support** - Use feature detection
2. **Handle errors gracefully** - Provide fallbacks
3. **Consider privacy** - Always get user permission
4. **Test across browsers** - Not all APIs work everywhere
