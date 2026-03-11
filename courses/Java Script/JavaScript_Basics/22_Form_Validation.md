# Form Validation

## What is Form Validation?

Form validation ensures that user input is correct before submitting a form. It checks that required fields are filled, email addresses are valid, passwords meet requirements, and more. Validation can be done on the client-side (JavaScript) before sending to the server.

Good validation improves user experience by catching errors early.

## Key Bullet Points

- **required attribute**: HTML5 built-in validation
- **value property**: Gets the text entered in an input field
- **trim()**: Removes whitespace from the beginning and end
- **Regular expressions**: Patterns for validating complex data like emails
- **submit event**: The event that fires when a form is submitted
- **preventDefault()**: Stops the form from actually submitting

## Simple HTML + JavaScript Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>Form Validation</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        form {
            max-width: 400px;
            padding: 20px;
            background: #f5f5f5;
            border-radius: 8px;
        }
        label {
            display: block;
            margin-top: 15px;
            font-weight: bold;
        }
        input {
            width: 100%;
            padding: 10px;
            margin-top: 5px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }
        button {
            margin-top: 20px;
            padding: 10px 20px;
            background: #4caf50;
            color: white;
            border: none;
            cursor: pointer;
        }
        .error {
            border-color: red !important;
        }
        .error-message {
            color: red;
            font-size: 12px;
            margin-top: 5px;
        }
        .success {
            background: #c8e6c9;
            padding: 15px;
            margin-top: 20px;
            border-radius: 4px;
        }
    </style>
</head>
<body>
    <h1>Form Validation</h1>
    
    <form id="myForm">
        <label for="name">Name (required):</label>
        <input type="text" id="name" name="name">
        <div id="nameError" class="error-message"></div>
        
        <label for="email">Email (required):</label>
        <input type="text" id="email" name="email">
        <div id="emailError" class="error-message"></div>
        
        <label for="password">Password (min 6 chars):</label>
        <input type="password" id="password" name="password">
        <div id="passwordError" class="error-message"></div>
        
        <button type="submit">Submit</button>
    </form>
    
    <div id="result"></div>

    <script>
        let form = document.getElementById("myForm");
        
        form.addEventListener("submit", function(event) {
            // Prevent form from submitting to server
            event.preventDefault();
            
            // Clear previous errors
            clearErrors();
            
            // Get values
            let name = document.getElementById("name").value.trim();
            let email = document.getElementById("email").value.trim();
            let password = document.getElementById("password").value;
            
            let isValid = true;
            
            // Validate name
            if (name === "") {
                showError("name", "Name is required");
                isValid = false;
            } else if (name.length < 2) {
                showError("name", "Name must be at least 2 characters");
                isValid = false;
            }
            
            // Validate email
            if (email === "") {
                showError("email", "Email is required");
                isValid = false;
            } else if (!isValidEmail(email)) {
                showError("email", "Please enter a valid email");
                isValid = false;
            }
            
            // Validate password
            if (password === "") {
                showError("password", "Password is required");
                isValid = false;
            } else if (password.length < 6) {
                showError("password", "Password must be at least 6 characters");
                isValid = false;
            }
            
            // If valid, show success message
            if (isValid) {
                document.getElementById("result").innerHTML = 
                    '<div class="success">Form submitted successfully!<br>' +
                    'Name: ' + name + '<br>Email: ' + email + '</div>';
            }
        });
        
        function showError(fieldId, message) {
            document.getElementById(fieldId).classList.add("error");
            document.getElementById(fieldId + "Error").textContent = message;
        }
        
        function clearErrors() {
            document.getElementById("name").classList.remove("error");
            document.getElementById("email").classList.remove("error");
            document.getElementById("password").classList.remove("error");
            document.getElementById("nameError").textContent = "";
            document.getElementById("emailError").textContent = "";
            document.getElementById("passwordError").textContent = "";
            document.getElementById("result").innerHTML = "";
        }
        
        function isValidEmail(email) {
            // Simple email validation using regex
            let pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return pattern.test(email);
        }
    </script>
</body>
</html>
```

## Code Explanation

- **event.preventDefault()**: Stops the form from actually submitting, allowing us to validate first.
- **value.trim()**: Removes leading and trailing whitespace before validation.
- **showError()**: A helper function that shows error messages and highlights the input field with red border.
- **clearErrors()**: Clears all error messages and styling before each validation.
- **isValidEmail()**: Uses a regular expression (regex) to check if the email format is valid.
- **form validation**: We check each field one by one and set isValid to false if any check fails.

## Expected Output

When you open this HTML file in a browser:
1. You see a form with Name, Email, and Password fields
2. Click Submit with empty fields - error messages appear: "Name is required", "Email is required", "Password is required"
3. Enter a name less than 2 characters - shows "Name must be at least 2 characters"
4. Enter an invalid email (like "test") - shows "Please enter a valid email"
5. Enter a short password - shows "Password must be at least 6 characters"
6. Fill all fields correctly - shows success message with the entered values
