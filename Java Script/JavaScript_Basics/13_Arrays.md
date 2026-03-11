# Arrays

## What are Arrays?

Arrays are ordered lists that can hold multiple values in a single variable. Instead of creating separate variables for each item, you can store them all in one array. Arrays are one of the most commonly used data structures in JavaScript for handling collections of items.

Arrays can hold any type of data - strings, numbers, objects, even other arrays.

## Key Bullet Points

- **Array creation**: `let fruits = ["Apple", "Banana", "Orange"]`
- **Access by index**: First item is at index 0, second at index 1, etc.
- **push()**: Adds item to the END of array
- **pop()**: Removes item from the END of array
- **shift()**: Removes item from the BEGINNING of array
- **unshift()**: Adds item to the BEGINNING of array
- **length**: Property that returns the number of items

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Arrays</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #fff8e1; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #ffc107;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #ffc107;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JavaScript Arrays</h1>
    
    <button onclick="showArrayBasics()">Array Basics</button>
    <button onclick="showPushPop()">push() and pop()</button>
    <button onclick="showShiftUnshift()">shift() and unshift()</button>
    
    <div id="output">Click a button to see arrays in action!</div>

    <script>
        // Create an array with some initial values
        let fruits = ["Apple", "Banana", "Orange"];

        // Button handler - array basics
        function showArrayBasics() {
            let result = "Original array: " + JSON.stringify(fruits) + "<br>";
            result += "Length: " + fruits.length + "<br>";
            result += "First item (index 0): " + fruits[0] + "<br>";
            result += "Second item (index 1): " + fruits[1] + "<br>";
            result += "Last item (index 2): " + fruits[2] + "<br>";
            
            // Change an item
            fruits[1] = "Mango";
            result += "<br>After changing index 1: " + JSON.stringify(fruits);
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - push and pop
        function showPushPop() {
            let fruits = ["Apple", "Banana"];
            let result = "Starting: " + JSON.stringify(fruits) + "<br><br>";
            
            // push - adds to the end
            fruits.push("Orange");
            result += "After push('Orange'): " + JSON.stringify(fruits) + "<br>";
            
            fruits.push("Mango");
            result += "After push('Mango'): " + JSON.stringify(fruits) + "<br><br>";
            
            // pop - removes from the end
            let removed = fruits.pop();
            result += "After pop(): " + JSON.stringify(fruits) + "<br>";
            result += "Removed item: " + removed;
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - shift and unshift
        function showShiftUnshift() {
            let fruits = ["Apple", "Banana", "Orange"];
            let result = "Starting: " + JSON.stringify(fruits) + "<br><br>";
            
            // unshift - adds to the beginning
            fruits.unshift("Mango");
            result += "After unshift('Mango'): " + JSON.stringify(fruits) + "<br>";
            
            fruits.unshift("Grape");
            result += "After unshift('Grape'): " + JSON.stringify(fruits) + "<br><br>";
            
            // shift - removes from the beginning
            let removed = fruits.shift();
            result += "After shift(): " + JSON.stringify(fruits) + "<br>";
            result += "Removed item: " + removed;
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **Creating arrays**: `let fruits = ["Apple", "Banana", "Orange"]` - items are separated by commas and wrapped in square brackets.
- **Accessing items**: `fruits[0]` gets the first item, `fruits[1]` gets the second, and so on. Arrays are zero-indexed!
- **Changing items**: `fruits[1] = "Mango"` replaces the item at index 1.
- **push()**: Adds one or more items to the end of the array. The array grows.
- **pop()**: Removes and returns the last item. The array shrinks.
- **unshift()**: Adds items to the beginning of the array.
- **shift()**: Removes and returns the first item.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and three buttons
2. Click "Array Basics" - shows:
   - Original array: ["Apple","Banana","Orange"]
   - Length: 3
   - First item (index 0): Apple
   - Second item (index 1): Banana
   - Last item (index 2): Orange
   - After changing index 1: ["Apple","Mango","Orange"]
3. Click "push() and pop()" - shows push adding to end, pop removing from end
4. Click "shift() and unshift()" - shows unshift adding to beginning, shift removing from beginning
