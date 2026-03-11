# Date Object

## What is the Date Object?

The Date object in JavaScript allows you to work with dates and times. It can create dates, get current date/time, and extract specific parts like the year, month, day, hour, minute, and second. This is essential for time-sensitive features like timestamps, calendars, and countdown timers.

The Date object counts months from 0 (January) to 11 (December).

## Key Bullet Points

- **new Date()**: Creates a new Date object with the current date and time
- **getFullYear()**: Returns the 4-digit year (e.g., 2024)
- **getMonth()**: Returns the month (0-11) - NOTE: January is 0!
- **getDate()**: Returns the day of the month (1-31)
- **getHours(), getMinutes(), getSeconds()**: Returns time components
- **getDay()**: Returns the day of the week (0-6) - Sunday is 0

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>JavaScript Date Object</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .result { 
            background: #fff3e0; 
            padding: 15px; 
            margin: 10px 0;
            border-left: 4px solid #ff5722;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #ff5722;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>JavaScript Date Object</h1>
    
    <button onclick="showCurrentDate()">Current Date</button>
    <button onclick="showDateParts()">Date Parts</button>
    <button onclick="showFormatted()">Custom Format</button>
    
    <div id="output">Click a button to see the Date object in action!</div>

    <script>
        // Button handler - current date and time
        function showCurrentDate() {
            let now = new Date();
            let result = "Current Date and Time:<br><br>";
            result += "new Date(): " + now + "<br><br>";
            result += "Current timestamp: " + now.getTime() + " (milliseconds since 1970)";
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - date parts
        function showDateParts() {
            let now = new Date();
            let result = "Date parts from new Date():<br><br>";
            
            result += "getFullYear(): " + now.getFullYear() + " (year)<br>";
            result += "getMonth(): " + now.getMonth() + " (0-11, Jan=0)<br>";
            result += "getDate(): " + now.getDate() + " (day 1-31)<br>";
            result += "getDay(): " + now.getDay() + " (day 0-6, Sun=0)<br><br>";
            
            result += "getHours(): " + now.getHours() + " (0-23)<br>";
            result += "getMinutes(): " + now.getMinutes() + " (0-59)<br>";
            result += "getSeconds(): " + now.getSeconds() + " (0-59)";
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }

        // Button handler - custom date format
        function showFormatted() {
            let now = new Date();
            
            // Get parts
            let year = now.getFullYear();
            let month = now.getMonth();
            let date = now.getDate();
            
            // Month names array (months are 0-11)
            let monthNames = ["January", "February", "March", "April", "May", "June",
                            "July", "August", "September", "October", "November", "December"];
            
            // Format as readable string
            let formatted = monthNames[month] + " " + date + ", " + year;
            
            // Get time
            let hours = now.getHours();
            let minutes = now.getMinutes();
            let ampm = hours >= 12 ? 'PM' : 'AM';
            hours = hours % 12;
            hours = hours ? hours : 12; // 0 should be 12
            minutes = minutes < 10 ? '0' + minutes : minutes;
            
            let timeFormatted = hours + ":" + minutes + " " + ampm;
            
            let result = "Today's date: <strong>" + formatted + "</strong><br>";
            result += "Current time: <strong>" + timeFormatted + "</strong>";
            
            document.getElementById("output").innerHTML = "<div class='result'>" + result + "</div>";
        }
    </script>
</body>
</html>
```

## Code Explanation

- **new Date()**: Creates a Date object with the current date and time. This is the starting point for all date operations.
- **getFullYear()**: Returns the four-digit year (e.g., 2024).
- **getMonth()**: Returns 0-11, where 0 is January and 11 is December. This is a common source of bugs!
- **getDate()**: Returns the day of the month (1-31).
- **getDay()**: Returns the day of the week (0-6), where 0 is Sunday.
- **Custom formatting**: The example shows how to create a readable format like "March 7, 2026" and convert to 12-hour format.

## Expected Output

When you open this HTML file in a browser:
1. You see the heading and three buttons
2. Click "Current Date" - shows current date/time and Unix timestamp
3. Click "Date Parts" - shows all individual components:
   - getFullYear(), getMonth() (0-11), getDate(), getDay() (0-6)
   - getHours(), getMinutes(), getSeconds()
4. Click "Custom Format" - shows formatted date like "March 7, 2026" and time like "5:30 PM"
