# LocalStorage

## What is LocalStorage?

LocalStorage is a way to store data in the user's browser that persists even after the browser is closed. It allows web applications to save data locally without a database. Data is stored as key-value pairs where both keys and values are strings.

LocalStorage is useful for storing user preferences, simple data, and session information.

## Key Bullet Points

- **setItem(key, value)**: Stores data with a key
- **getItem(key)**: Retrieves data by key
- **removeItem(key)**: Deletes a specific item
- **clear()**: Removes all stored data
- **Data is always strings**: Numbers and objects must be converted
- **Storage limit**: About 5-10 MB per domain

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>LocalStorage</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #fce4ec; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #e91e63;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #e91e63;
            color: white;
            border: none;
        }
        input { padding: 8px; margin: 5px; }
    </style>
</head>
<body>
    <h1>LocalStorage</h1>
    
    <input type="text" id="nameInput" placeholder="Enter your name">
    <button onclick="saveName()">Save Name</button>
    
    <button onclick="getName()">Get Saved Name</button>
    <button onclick="removeName()">Remove Name</button>
    <button onclick="clearAll()">Clear All</button>
    
    <div id="output"></div>

    <script>
        // Save name to localStorage
        function saveName() {
            let name = document.getElementById("nameInput").value;
            
            if (name === "") {
                alert("Please enter a name first!");
                return;
            }
            
            // setItem stores data as strings
            localStorage.setItem("userName", name);
            
            document.getElementById("output").innerHTML = 
                "<div class='result'>Saved: '" + name + "' to localStorage</div>";
        }

        // Get name from localStorage
        function getName() {
            // getItem returns null if the key doesn't exist
            let name = localStorage.getItem("userName");
            
            if (name === null) {
                document.getElementById("output").innerHTML = 
                    "<div class='result'>No name saved yet!</div>";
            } else {
                document.getElementById("output").innerHTML = 
                    "<div class='result'>Saved name: <strong>" + name + "</strong></div>";
            }
        }

        // Remove specific item
        function removeName() {
            localStorage.removeItem("userName");
            document.getElementById("output").innerHTML = 
                "<div class='result'>Removed 'userName' from localStorage</div>";
        }

        // Clear all localStorage
        function clearAll() {
            localStorage.clear();
            document.getElementById("output").innerHTML = 
                "<div class='result'>Cleared all localStorage data</div>";
        }

        // Check if there's saved data on page load
        window.onload = function() {
            let savedName = localStorage.getItem("userName");
            if (savedName) {
                document.getElementById("output").innerHTML = 
                    "<div class='result'>Welcome back! Your saved name is: <strong>" + savedName + "</strong></div>";
            }
        };
    </script>
</body>
</html>
```

## Code Explanation

- **localStorage.setItem(key, value)**: Stores a value with a key. Note: values are always converted to strings.
- **localStorage.getItem(key)**: Retrieves the value stored with that key. Returns null if not found.
- **localStorage.removeItem(key)**: Deletes a specific item from storage.
- **localStorage.clear()**: Removes ALL data stored for this domain.
- **window.onload**: Runs when the page finishes loading - we use this to check if there's saved data.

## Expected Output

When you open this HTML file in a browser:
1. You see an input field and four buttons
2. Type your name and click "Save Name" - shows "Saved: 'YourName' to localStorage"
3. Refresh the page - shows "Welcome back! Your saved name is: YourName"
4. Click "Get Saved Name" - shows the saved name
5. Click "Remove Name" - removes the saved data
6. Click "Clear All" - clears all localStorage data for this page
