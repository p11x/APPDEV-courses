# Fetch API

## What is the Fetch API?

The Fetch API is a modern JavaScript interface for making HTTP requests to servers. It allows web applications to send and receive data from APIs. The Fetch API is Promise-based, making it cleaner than older methods like XMLHttpRequest.

The Fetch API is the standard way to communicate with REST APIs in modern JavaScript.

## Key Bullet Points

- **fetch()**: The main function for making requests
- **Promise-based**: Uses .then() and .catch() or async/await
- **response.json()**: Converts the response to JavaScript object
- **HTTP methods**: GET (retrieve), POST (create), PUT (update), DELETE (remove)
- **async/await**: Modern syntax for handling asynchronous operations
- **Error handling**: Always check response.ok and use try/catch

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>Fetch API</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #e0f2f1; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #009688;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #009688;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>Fetch API</h1>
    
    <button onclick="getData()">Fetch Data (GET)</button>
    <button onclick="getWithAsync()">Fetch with Async/Await</button>
    
    <div id="output">Click a button to see Fetch API in action!</div>

    <script>
        // Example 1: Using .then() syntax
        function getData() {
            document.getElementById("output").innerHTML = "Loading...";
            
            // Fetch data from a public API (JSONPlaceholder)
            fetch('https://jsonplaceholder.typicode.com/users/1')
                .then(response => {
                    if (!response.ok) {
                        throw new Error("HTTP error " + response.status);
                    }
                    return response.json();
                })
                .then(data => {
                    let result = "Fetched user data:<br><br>";
                    result += "<strong>Name:</strong> " + data.name + "<br>";
                    result += "<strong>Email:</strong> " + data.email + "<br>";
                    result += "<strong>City:</strong> " + data.address.city + "<br>";
                    result += "<strong>Company:</strong> " + data.company.name;
                    
                    document.getElementById("output").innerHTML = 
                        "<div class='result'>" + result + "</div>";
                })
                .catch(error => {
                    document.getElementById("output").innerHTML = 
                        "<div class='result' style='color:red'>Error: " + error.message + "</div>";
                });
        }

        // Example 2: Using async/await syntax
        async function getWithAsync() {
            document.getElementById("output").innerHTML = "Loading with async/await...";
            
            try {
                // Fetch data
                let response = await fetch('https://jsonplaceholder.typicode.com/posts/1');
                
                if (!response.ok) {
                    throw new Error("HTTP error " + response.status);
                }
                
                let data = await response.json();
                
                let result = "Fetched post data (async/await):<br><br>";
                result += "<strong>Title:</strong> " + data.title + "<br><br>";
                result += "<strong>Body:</strong> " + data.body;
                
                document.getElementById("output").innerHTML = 
                    "<div class='result'>" + result + "</div>";
                    
            } catch (error) {
                document.getElementById("output").innerHTML = 
                    "<div class='result' style='color:red'>Error: " + error.message + "</div>";
            }
        }
    </script>
</body>
</html>
```

## Code Explanation

- **fetch(url)**: Makes a GET request to the URL. Returns a Promise.
- **response.json()**: Converts the response body to a JavaScript object.
- **.then()**: Chains handlers for successful responses. Runs when the Promise resolves.
- **.catch()**: Handles errors. Runs when something goes wrong.
- **async/await**: Modern syntax that's easier to read. `await` pauses execution until the Promise resolves.
- **response.ok**: Checks if the response was successful (status 200-299).

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and two buttons
2. Click "Fetch Data (GET)" - shows "Loading...", then displays user data from the API:
   - Name, Email, City, Company
3. Click "Fetch with Async/Await" - shows the same data using modern async/await syntax
4. If there's an error, shows error message in red
