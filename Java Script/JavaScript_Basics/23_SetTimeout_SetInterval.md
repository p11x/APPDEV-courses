# SetTimeout and SetInterval

## What are SetTimeout and SetInterval?

SetTimeout and SetInterval are functions that allow you to execute code after a delay. SetTimeout runs code once after a specified delay, while SetInterval runs code repeatedly at a specified interval. These are essential for animations, countdowns, auto-refreshing content, and timing-based features.

Understanding timing functions is crucial for creating dynamic, time-sensitive web applications.

## Key Bullet Points

- **setTimeout()**: Executes code once after a delay (in milliseconds)
- **setInterval()**: Executes code repeatedly at a specific interval
- **clearTimeout()**: Cancels a setTimeout before it executes
- **clearInterval()**: Stops a setInterval from continuing
- 1000 milliseconds =- Both return 1 second
 an ID that can be used to cancel them

## Simple HTML + JavaScript Example

```html
<!DOCTYPE name="SetTimeout and SetInterval">
<html>
<head>
    <title>SetTimeout and SetInterval</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .display {
            font-size: 48px;
            text-align: center;
            padding: 20px;
            background: #e3f2fd;
            margin: 10px 0;
        }
        button { 
            padding: 10px 20px; 
            margin: 5px;
            cursor: pointer;
            background: #00bcd4;
            color: white;
            border: none;
        }
    </style>
</head>
<body>
    <h1>SetTimeout and SetInterval</h1>
    
    <div class="display" id="countdown">5</div>
    
    <button onclick="startCountdown()">Start Countdown</button>
    <button onclick="stopCountdown()">Stop</button>
    
    <div class="display" id="clock">--:--:--</div>
    <button onclick="startClock()">Start Clock</button>
    <button onclick="stopClock()">Stop Clock</button>
    
    <div class="display" id="delayed" style="background: #e8f5e9;">Wait for it...</div>
    <button onclick="showDelayed()">Show Delayed Message</button>

    <script>
        let countdownInterval;
        let clockInterval;
        
        // setTimeout example
        function showDelayed() {
            let display = document.getElementById("delayed");
            display.textContent = "Wait for it...";
            
            // This runs after 3 seconds (3000 milliseconds)
            setTimeout(function() {
                display.textContent = "Hello! This appeared after 3 seconds!";
            }, 3000);
        }
        
        // setInterval example - countdown
        function startCountdown() {
            let count = 5;
            let display = document.getElementById("countdown");
            display.textContent = count;
            
            // Clear any existing interval
            if (countdownInterval) clearInterval(countdownInterval);
            
            // Run every 1 second
            countdownInterval = setInterval(function() {
                count--;
                display.textContent = count;
                
                if (count <= 0) {
                    clearInterval(countdownInterval);
                    display.textContent = "Time's up!";
                }
            }, 1000);
        }
        
        function stopCountdown() {
            clearInterval(countdownInterval);
            document.getElementById("countdown").textContent = "Stopped";
        }
        
        // setInterval example - clock
        function startClock() {
            updateClock();  // Show immediately
            
            if (clockInterval) clearInterval(clockInterval);
            
            clockInterval = setInterval(updateClock, 1000);
        }
        
        function updateClock() {
            let now = new Date();
            let time = now.toLocaleTimeString();
            document.getElementById("clock").textContent = time;
        }
        
        function stopClock() {
            clearInterval(clockInterval);
        }
    </script>
</body>
</html>
```

## Code Explanation

- **setTimeout(function, delay)**: Schedules the function to run after the delay (in milliseconds). The page continues loading while waiting.
- **setInterval(function, interval)**: Runs the function repeatedly at each interval. The code inside runs every X milliseconds.
- **clearInterval(id)**: Stops the interval from running further. Always store the interval ID so you can stop it.
- **toLocaleTimeString()**: Converts the time to the user's local format.
- **Countdown example**: Shows how to use setInterval to count down and clearInterval to stop it.

## Expected Output

When you open this HTML file in a browser:
1. You see a countdown display (starting at 5), a clock display, and a "Wait for it..." message area
2. Click "Show Delayed Message" - wait 3 seconds - the message changes to "Hello! This appeared after 3 seconds!"
3. Click "Start Countdown" - counts down from 5 to 0, then shows "Time's up!"
4. Click "Stop" - stops the countdown early
5. Click "Start Clock" - shows the current time, updating every second
6. Click "Stop Clock" - stops the clock
