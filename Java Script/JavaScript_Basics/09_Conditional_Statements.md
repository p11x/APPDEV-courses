# Conditional Statements

## What are Conditional Statements?

Conditional statements allow your program to make decisions based on certain conditions. They let your code choose different actions depending on whether something is true or false. This is fundamental to programming - it allows programs to respond differently in different situations.

Without conditional statements, every program would do the same thing every time it runs.

## Key Bullet Points

- **if**: Executes code only if the condition is true
- **else**: Executes code when the if condition is false
- **else if**: Checks another condition if the previous ones were false
- **switch**: Useful when comparing one value against many possibilities
- Conditions use comparison operators (===, >, <, etc.)
- Only one block of code executes - the first one with a true condition

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Conditional Statements</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #fff3e0; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #ff9800;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #ff9800;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JavaScript Conditional Statements</h1>
    
    <button onclick="checkAge()">Check Age (if/else)</button>
    <button onclick="checkDay()">Check Day (switch)</button>
    <button onclick="checkGrade()">Check Grade (else if)</button>
    
    <div id="output">Click a button to see conditional statements in action!</div>

    <script>
        // Example 1: if/else statement
        function checkAge() {
            let age = prompt("Enter your age:");
            
            if (age === null) {
                document.getElementById("output").innerHTML = "You cancelled!";
                return;
            }
            
            age = Number(age);  // Convert to number
            
            if (age >= 18) {
                document.getElementById("output").innerHTML = 
                    "<div class='result'>You are <strong>18 or older</strong>. You are an adult!</div>";
            } else {
                document.getElementById("output").innerHTML = 
                    "<div class='result'>You are <strong>under 18</strong>. You are a minor!</div>";
            }
        }

        // Example 2: switch statement
        function checkDay() {
            let dayNumber = new Date().getDay();  // 0 = Sunday, 1 = Monday, etc.
            let dayName;
            
            switch(dayNumber) {
                case 0:
                    dayName = "Sunday";
                    break;
                case 1:
                    dayName = "Monday";
                    break;
                case 2:
                    dayName = "Tuesday";
                    break;
                case 3:
                    dayName = "Wednesday";
                    break;
                case 4:
                    dayName = "Thursday";
                    break;
                case 5:
                    dayName = "Friday";
                    break;
                case 6:
                    dayName = "Saturday";
                    break;
                default:
                    dayName = "Unknown";
            }
            
            document.getElementById("output").innerHTML = 
                "<div class='result'>Today is <strong>" + dayName + "</strong> (Day " + dayNumber + ")</div>";
        }

        // Example 3: else if chain
        function checkGrade() {
            let score = prompt("Enter a score (0-100):");
            
            if (score === null) {
                return;
            }
            
            score = Number(score);
            let grade;
            
            if (score >= 90) {
                grade = "A - Excellent!";
            } else if (score >= 80) {
                grade = "B - Good job!";
            } else if (score >= 70) {
                grade = "C - Satisfactory";
            } else if (score >= 60) {
                grade = "D - Needs improvement";
            } else {
                grade = "F - Failed";
            }
            
            document.getElementById("output").innerHTML = 
                "<div class='result'>Score: <strong>" + score + "</strong><br>Grade: <strong>" + grade + "</strong></div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **if statement**: Checks if age >= 18. If true, shows "adult" message.
- **else**: Executes when the if condition is false - shows "minor" message.
- **switch statement**: Checks the value of `dayNumber` against multiple cases. The `break` keyword stops checking other cases once a match is found.
- **else if chain**: Checks multiple conditions in order. Once one is true, it executes that block and skips the rest.
- **`getDay()`**: Built-in JavaScript function that returns the day of the week (0-6).

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and three buttons
2. Click "Check Age (if/else)" - enter an age like 20
   - If 18 or older: shows "You are 18 or older. You are an adult!"
   - If under 18: shows "You are under 18. You are a minor!"
3. Click "Check Day (switch)" - shows the current day of the week
4. Click "Check Grade (else if)" - enter a score like 85
   - Shows grade based on score: A (90+), B (80+), C (70+), D (60+), F (below 60)
