# User Input

## What is User Input?

User input allows programs to get information from the person using the application. In JavaScript, the most basic way to get input is through the `prompt()` function, which displays a dialog box where users can type their response. This makes web pages interactive by allowing users to provide data.

Getting user input is essential for building interactive applications like forms, calculators, and games.

## Key Bullet Points

- **prompt()**: Shows a dialog box with an input field for the user to type a response
- The value entered by the user is returned as a string
- If the user clicks Cancel or doesn't enter anything, `null` is returned
- Always remember that input from prompt() is a string - convert to number when needed
- The input can be stored in a variable and used throughout your code

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript User Input</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #e3f2fd; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #2196f3;
        }
        button { 
            padding: 10px 20px; 
            font-size: 16px;
            cursor: pointer;
            background: #2196f3;
            color: white;
            border: none;
            border-radius: 4px;
        }
    </style>
</head>
<body>
    <h1>JavaScript User Input</h1>
    <p>Click the button to enter your name and age.</p>
    
    <button onclick="getUserInfo()">Enter Your Info</button>
    
    <div id="output"></div>

    <script>
        function getUserInfo() {
            // Get user's name using prompt
            let name = prompt("What is your name?");
            
            // Check if user cancelled or entered nothing
            if (name === null || name === "") {
                alert("You didn't enter a name!");
                return;
            }
            
            // Get user's age using prompt
            let ageInput = prompt("How old are you?");
            
            // Convert string to number for calculations
            let age = Number(ageInput);
            
            // Display the results
            let output = document.getElementById("output");
            output.innerHTML = 
                "<div class='result'>" +
                "<h3>Welcome, " + name + "!</h3>" +
                "<p>Your name is: <strong>" + name + "</strong></p>" +
                "<p>Your age is: <strong>" + age + "</strong></p>" +
                "<p>In 10 years, you will be: <strong>" + (age + 10) + "</strong></p>" +
                "</div>";
            
            // Also log to console
            console.log("User entered:", { name: name, age: age });
        }
    </script>
</body>
</html>
```

## Code Explanation

- **`prompt("What is your name?")`**: Opens a dialog box with the message "What is your name?" and an input field. The user's response is stored in the `name` variable.
- **Checking for null/cancel**: `if (name === null || name === "")` handles cases where the user clicked Cancel or didn't type anything.
- **Number conversion**: `Number(ageInput)` converts the string input to a number so we can do math with it (like `age + 10`).
- **Displaying results**: We use `innerHTML` to show the user's name and calculated age in the output div.
- **Console logging**: Good practice to log user input for debugging purposes.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading "JavaScript User Input" and a button "Enter Your Info"
2. Click the button
3. A prompt appears asking "What is your name?" - enter your name and click OK
4. Another prompt appears asking "How old are you?" - enter your age and click OK
5. The page displays a blue box showing:
   - Welcome, [your name]!
   - Your name is: [your name]
   - Your age is: [your age]
   - In 10 years, you will be: [your age + 10]
6. If you click Cancel or don't enter anything, an alert shows "You didn't enter a name!"
