# Loops

## What are Loops?

Loops allow you to repeat code multiple times without writing it repeatedly. Instead of writing the same code 10 times, you can use a loop to run it 10 times with different values. Loops are essential for processing lists of data, automating repetitive tasks, and building efficient programs.

There are several types of loops in JavaScript, each suited for different situations.

## Key Bullet Points

- **for loop**: Best when you know how many times to repeat (uses counter variable)
- **while loop**: Best when you don't know how many times to repeat
- **do-while loop**: Like while, but runs at least once before checking condition
- **Array iteration**: Use for loop with array.length to go through array elements
- Be careful of infinite loops - always have a way for the condition to become false

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Loops</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #e8f5e9; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #4caf50;
            white-space: pre-wrap;
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
    <h1>JavaScript Loops</h1>
    
    <button onclick="showForLoop()">For Loop (1-5)</button>
    <button onclick="showWhileLoop()">While Loop</button>
    <button onclick="showDoWhileLoop()">Do-While Loop</button>
    <button onclick="showArrayLoop()">Array with Loop</button>
    
    <div id="output">Click a button to see different loop types!</div>

    <script>
        // Example 1: For Loop
        function showForLoop() {
            let output = "For Loop - Counting 1 to 5:\n";
            
            // start at 1, continue while i <= 5, increment by 1 each time
            for (let i = 1; i <= 5; i++) {
                output += "Count: " + i + "\n";
            }
            
            document.getElementById("output").innerHTML = "<div class='result'>" + output + "</div>";
        }

        // Example 2: While Loop
        function showWhileLoop() {
            let output = "While Loop - Countdown:\n";
            let count = 5;
            
            // continues while count is greater than 0
            while (count > 0) {
                output += "Countdown: " + count + "\n";
                count--;  // decrease count to eventually stop the loop
            }
            
            output += "Liftoff!";
            document.getElementById("output").innerHTML = "<div class='result'>" + output + "</div>";
        }

        // Example 3: Do-While Loop
        function showDoWhileLoop() {
            let output = "Do-While Loop:\n";
            let number = 1;
            
            // runs at least once, then checks condition
            do {
                output += "Number is: " + number + "\n";
                number++;
            } while (number <= 3);
            
            document.getElementById("output").innerHTML = "<div class='result'>" + output + "</div>";
        }

        // Example 4: Loop through Array
        function showArrayLoop() {
            let fruits = ["Apple", "Banana", "Orange", "Mango"];
            let output = "Array: " + JSON.stringify(fruits) + "\n\n";
            output += "Looping through fruits:\n";
            
            // Use for loop with array length
            for (let i = 0; i < fruits.length; i++) {
                output += (i + 1) + ". " + fruits[i] + "\n";
            }
            
            document.getElementById("output").innerHTML = "<div class='result'>" + output + "</div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **For loop syntax**: `for (let i = 1; i <= 5; i++)` means: start with i=1, keep going while i<=5, increase i by 1 each time.
- **While loop**: Checks the condition BEFORE running the code. If the condition is false initially, the code never runs.
- **Do-while loop**: Runs the code first, THEN checks the condition. This guarantees the code runs at least once.
- **Array loop**: `fruits.length` gives the number of items in the array. Loop from 0 to length-1 to access each element.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and four buttons
2. Click "For Loop (1-5)" - shows counting from 1 to 5
3. Click "While Loop" - shows countdown from 5 to 1, then "Liftoff!"
4. Click "Do-While Loop" - shows "Number is: 1", "Number is: 2", "Number is: 3"
5. Click "Array with Loop" - shows all fruits in the array with their index numbers:
   - 1. Apple
   - 2. Banana
   - 3. Orange
   - 4. Mango
