# JSON Basics

## What is JSON?

JSON (JavaScript Object Notation) is a lightweight format for storing and transporting data. It's the most common data format for API communication and data storage. JSON looks exactly like JavaScript objects, making it easy to work with.

JSON is essential for modern web development - APIs return data in JSON format.

## Key Bullet Points

- **JSON.stringify()**: Converts JavaScript objects to JSON strings
- **JSON.parse()**: Converts JSON strings back to JavaScript objects
- **JSON format**: Keys must be in double quotes, no trailing commas
- **Data types**: Strings, numbers, booleans, arrays, objects, null
- **API communication**: Most web APIs send and receive data in JSON format

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JSON Basics</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #e8eaf6; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #3f51b5;
            white-space: pre-wrap;
            font-family: monospace;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #3f51b5;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JSON Basics</h1>
    
    <button onclick="convertToJSON()">Object to JSON</button>
    <button onclick="convertToObject()">JSON to Object</button>
    <button onclick="processAPIResponse()">Simulate API</button>

    <div id="output">Click a button to see JSON in action!</div>

    <script>
        // Button handler - JavaScript object to JSON string
        function convertToJSON() {
            // JavaScript object
            let person = {
                name: "Alice",
                age: 25,
                city: "New York",
                skills: ["HTML", "CSS", "JavaScript"]
            };
            
            // Convert to JSON string
            let jsonString = JSON.stringify(person, null, 2);
            
            let result = "JavaScript Object:<br>";
            result += JSON.stringify(person, null, 2) + "<br><br>";
            result += "After JSON.stringify():<br>";
            result += "<div class='result'>" + jsonString + "</div>";
            
            document.getElementById("output").innerHTML = result;
        }

        // Button handler - JSON string to JavaScript object
        function convertToObject() {
            // JSON string (note: keys in double quotes)
            let jsonString = '{"name":"Bob","age":30,"city":"London","isStudent":false}';
            
            // Convert to JavaScript object
            let person = JSON.parse(jsonString);
            
            let result = "JSON String:<br>";
            result += jsonString + "<br><br>";
            result += "After JSON.parse():<br>";
            result += "person.name: " + person.name + "<br>";
            result += "person.age: " + person.age + "<br>";
            result += "person.city: " + person.city + "<br>";
            result += "person.isStudent: " + person.isStudent;
            
            document.getElementById("output").innerHTML = result;
        }

        // Button handler - simulating API response
        function processAPIResponse() {
            // Simulate API response (usually comes from fetch)
            let apiResponse = '[' +
                '{"id":1,"name":"Product A","price":29.99},' +
                '{"id":2,"name":"Product B","price":19.99},' +
                '{"id":3,"name":"Product C","price":39.99}' +
            ']';
            
            // Parse the JSON array
            let products = JSON.parse(apiResponse);
            
            let result = "Products from API:<br><br>";
            
            // Loop through products
            products.forEach(function(product) {
                result += product.id + ". " + product.name + " - $" + product.price + "<br>";
            });
            
            result += "<br>Total: $" + products.reduce((sum, p) => sum + p.price, 0).toFixed(2);
            
            document.getElementById("output").innerHTML = result;
        }
    </script>
</body>
</html>
```

## Code Explanation

- **JSON.stringify(object)**: Converts a JavaScript object into a JSON string. The second and third parameters format the output nicely.
- **JSON.parse(string)**: Converts a JSON string back into a JavaScript object.
- **JSON format differences**: JSON requires double quotes around keys ("name"), while JavaScript objects can use unquoted keys (name).
- **API simulation**: Shows how data often comes from APIs as JSON strings and needs to be parsed.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and three buttons
2. Click "Object to JSON" - shows the JavaScript object and the JSON string version
3. Click "JSON to Object" - shows the JSON string and how to access properties after parsing
4. Click "Simulate API" - shows a list of products and calculates the total price
