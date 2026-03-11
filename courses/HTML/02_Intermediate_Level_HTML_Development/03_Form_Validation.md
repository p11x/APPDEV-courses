# Form Validation

## Topic Title
HTML5 Form Validation

## Concept Explanation

### What is Form Validation?

Form validation is the process of ensuring that user input is correct and complete before submitting the form. HTML5 provides built-in validation attributes that prevent invalid data from being submitted.

### Validation Attributes

- **required** - Field must have a value
- **minlength** - Minimum number of characters
- **maxlength** - Maximum number of characters
- **min** - Minimum value for numbers/dates
- **max** - Maximum value for numbers/dates
- **pattern** - Regular expression pattern
- **type** - Built-in validation (email, url, number)
- **step** - Valid increment for numbers

### Validation States

- **:valid** - Input meets all validation rules
- **:invalid** - Input fails validation
- **:required** - Required input field
- **:optional** - Optional input field
- **:in-range** - Number within range
- **:out-of-range** - Number outside range

## Why This Concept Is Important

Validation matters because:

1. **Data quality** - Ensures correct data format
2. **User feedback** - Immediate error indication
3. **Server load** - Reduces server processing
4. **User experience** - Guides users to correct input
5. **Security** - Prevents malicious input

## Step-by-Step Explanation

### Step 1: Required Fields

```html
<input type="text" required>
<input type="email" required>
```

### Step 2: Length Validation

```html
<input type="text" minlength="3" maxlength="20">
```

### Step 3: Number Validation

```html
<input type="number" min="0" max="100">
<input type="number" min="0" max="100" step="10">
```

### Step 4: Pattern Validation

```html
<input type="text" pattern="[A-Za-z]{3}">
<input type="tel" pattern="[0-9]{3}-[0-9]{3}-[0-9]{4}">
```

### Step 5: Custom Validation Messages

```html
<input type="text" required 
       oninvalid="this.setCustomValidity('Please enter your name')"
       oninput="this.setCustomValidity('')">
```

## Code Examples

### Example 1: Complete Validated Form

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Form Validation Demo</title>
    <style>
        input:invalid {
            border-color: red;
        }
        input:valid {
            border-color: green;
        }
        .error {
            color: red;
            font-size: 0.9em;
        }
    </style>
</head>
<body>
    <h1>Registration Form with Validation</h1>
    
    <form action="/register" method="POST">
        <!-- Required field -->
        <div>
            <label for="username">Username (required, 3-15 chars):</label><br>
            <input type="text" id="username" name="username" 
                   required minlength="3" maxlength="15"
                   pattern="[a-zA-Z0-9]+"
                   title="Letters and numbers only">
            <span class="error">*</span>
        </div>
        
        <!-- Email validation -->
        <div>
            <label for="email">Email (required):</label><br>
            <input type="email" id="email" name="email" required>
            <span class="error">*</span>
        </div>
        
        <!-- Password validation -->
        <div>
            <label for="password">Password (required, min 8 chars):</label><br>
            <input type="password" id="password" name="password" 
                   required minlength="8"
                   pattern="(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}"
                   title="Must contain at least one number, one uppercase and lowercase letter">
            <span class="error">*</span>
        </div>
        
        <!-- Confirm password -->
        <div>
            <label for="confirm-password">Confirm Password:</label><br>
            <input type="password" id="confirm-password" name="confirm-password" required>
        </div>
        
        <!-- Number validation -->
        <div>
            <label for="age">Age (18-100):</label><br>
            <input type="number" id="age" name="age" min="18" max="100">
        </div>
        
        <!-- URL validation -->
        <div>
            <label for="website">Website (optional):</label><br>
            <input type="url" id="website" name="website">
        </div>
        
        <!-- Date validation -->
        <div>
            <label for="birthdate">Date of Birth:</label><br>
            <input type="date" id="birthdate" name="birthdate" 
                   max="2006-01-01">
        </div>
        
        <!-- Checkbox validation -->
        <div>
            <input type="checkbox" id="terms" name="terms" required>
            <label for="terms">I agree to the Terms and Conditions</label>
            <span class="error">*</span>
        </div>
        
        <button type="submit">Register</button>
    </form>
</body>
</html>
```

### Example 2: Pattern Validation Examples

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Pattern Validation</title>
</head>
<body>
    <h1>Pattern Validation Examples</h1>
    
    <form>
        <!-- US Phone: 123-456-7890 -->
        <div>
            <label for="phone">US Phone (123-456-7890):</label><br>
            <input type="tel" id="phone" name="phone" 
                   pattern="[0-9]{3}-[0-9]{3}-[0-9]{4}"
                   placeholder="123-456-7890">
        </div>
        
        <!-- Zip Code: 12345 or 12345-6789 -->
        <div>
            <label for="zip">US Zip Code:</label><br>
            <input type="text" id="zip" name="zip" 
                   pattern="[0-9]{5}(-[0-9]{4})?"
                   placeholder="12345">
        </div>
        
        <!-- Credit Card: 16 digits -->
        <div>
            <label for="card">Credit Card (16 digits):</label><br>
            <input type="text" id="card" name="card" 
                   pattern="[0-9]{16}"
                   placeholder="1234567890123456">
        </div>
        
        <!-- Username: letters and numbers only -->
        <div>
            <label for="username">Username (letters and numbers):</label><br>
            <input type="text" id="username" name="username" 
                   pattern="[a-zA-Z0-9]+">
        </div>
        
        <!-- Password: must contain letter and number -->
        <div>
            <label for="password">Password (letter and number):</label><br>
            <input type="password" id="password" name="password" 
                   pattern="(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+">
        </div>
        
        <!-- Hex Color: #RRGGBB -->
        <div>
            <label for="color">Hex Color:</label><br>
            <input type="text" id="color" name="color" 
                   pattern="^#[0-9A-Fa-f]{6}$"
                   placeholder="#ff0000">
        </div>
        
        <button type="submit">Submit</button>
    </form>
</body>
</html>
```

### Example 3: Custom Validation Messages

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Custom Validation</title>
    <script>
        function validateForm() {
            const form = document.getElementById('myForm');
            const username = form.username;
            
            if (username.value === 'admin') {
                username.setCustomValidity('Username "admin" is not allowed');
                return false;
            }
            
            username.setCustomValidity('');
            return true;
        }
        
        function setMessage(input, message) {
            input.setCustomValidity(message);
        }
    </script>
</head>
<body>
    <h1>Custom Validation Messages</h1>
    
    <form id="myForm" onsubmit="return validateForm()">
        <div>
            <label for="username">Username:</label><br>
            <input type="text" id="username" name="username" 
                   required
                   oninvalid="this.setCustomValidity('Username is required')"
                   oninput="this.setCustomValidity('')">
        </div>
        
        <div>
            <label for="age">Age (must be 18 or older):</label><br>
            <input type="number" id="age" name="age" 
                   min="18"
                   oninvalid="this.setCustomValidity('You must be at least 18 years old')"
                   oninput="this.setCustomValidity('')">
        </div>
        
        <div>
            <label for="email">Email:</label><br>
            <input type="email" id="email" name="email" 
                   required
                   oninvalid="this.setCustomValidity('Please enter a valid email')"
                   oninput="this.setCustomValidity('')">
        </div>
        
        <button type="submit">Submit</button>
    </form>
</body>
</html>
```

### Example 4: Real-World Login Form Validation

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Login Form Validation</title>
    <style>
        .form-group {
            margin-bottom: 15px;
        }
        label {
            display: block;
            margin-bottom: 5px;
        }
        input:invalid {
            border: 2px solid red;
            background-color: #fff0f0;
        }
        input:valid {
            border: 2px solid green;
            background-color: #f0fff0;
        }
    </style>
</head>
<body>
    <h1>Login</h1>
    
    <form action="/login" method="POST" novalidate>
        <div class="form-group">
            <label for="email">Email Address</label>
            <input type="email" id="email" name="email" 
                   required
                   placeholder="you@example.com">
        </div>
        
        <div class="form-group">
            <label for="password">Password</label>
            <input type="password" id="password" name="password" 
                   required
                   minlength="8"
                   placeholder="Enter your password">
        </div>
        
        <div class="form-group">
            <input type="checkbox" id="remember" name="remember">
            <label for="remember">Remember me</label>
        </div>
        
        <button type="submit">Login</button>
    </form>
</body>
</html>
```

## Best Practices

### Validation Best Practices

1. **Use HTML5 validation** - Built-in validation is reliable
2. **Combine attributes** - Use multiple validation rules
3. **Provide clear messages** - Tell users what's wrong
4. **Show examples** - Use placeholder for format hint

### Security Best Practices

1. **Don't rely only on client-side** - Always validate server-side
2. **Sanitize input** - Remove harmful characters
3. **Use HTTPS** - Secure form submission

### User Experience Best Practices

1. **Validate on blur** - Check field when user leaves it
2. **Show success state** - Indicate valid input
3. **Don't validate too early** - Wait for user to finish typing

## Real-World Examples

### Example 1: Checkout Form Validation

```html
<form action="/checkout" method="POST">
    <!-- Card number: 13-19 digits -->
    <div>
        <label for="card-number">Card Number:</label>
        <input type="text" id="card-number" name="card-number" 
               pattern="[0-9]{13,19}"
               title="Enter 13-19 digit card number"
               required>
    </div>
    
    <!-- Expiry: MM/YY -->
    <div>
        <label for="expiry">Expiry Date:</label>
        <input type="text" id="expiry" name="expiry" 
               pattern="(0[1-9]|1[0-2])\/[0-9]{2}"
               placeholder="MM/YY"
               required>
    </div>
    
    <!-- CVV: 3-4 digits -->
    <div>
        <label for="cvv">CVV:</label>
        <input type="text" id="cvv" name="cvv" 
               pattern="[0-9]{3,4}"
               title="Enter 3 or 4 digit CVV"
               required>
    </div>
</form>
```

### Example 2: Password Strength Validation

```html
<form>
    <div>
        <label for="password">Password:</label>
        <input type="password" id="password" name="password" 
               required
               minlength="8"
               pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"
               title="Must contain: 8+ chars, uppercase, lowercase, number, special char">
    </div>
</form>
```

## Common Mistakes Students Make

### Mistake 1: No Validation

```html
<!-- Wrong - no validation -->
<input type="text" name="email">

<!-- Correct - with validation -->
<input type="email" name="email" required>
```

### Mistake 2: Conflicting Validation

```html
<!-- Wrong - min > max -->
<input type="number" min="100" max="50">

<!-- Correct - proper range -->
<input type="number" min="0" max="100">
```

### Mistake 3: Missing Required on Optional Fields

```html
<!-- Wrong - placeholder as label -->
<input type="email" placeholder="Enter your email">

<!-- Correct - proper label and optional field -->
<label for="email">Email (optional):</label>
<input type="email" id="email" name="email">
```

### Mistake 4: Only Client-Side Validation

```html
<!-- Wrong - only client validation -->
<input type="text" required>

<!-- Correct - also validate server-side -->
<!-- Server-side code should also check the data -->
```

## Exercises

### Exercise 1: Validate Registration Form
Add validation to a registration form.

### Exercise 2: Create Custom Messages
Add custom validation error messages.

### Exercise 3: Pattern Validation
Use pattern for phone number validation.

### Exercise 4: Password Requirements
Create password with multiple requirements.

## Mini Practice Tasks

### Task 1: Required Field
Add required attribute to a form field.

### Task 2: Length Limits
Add minlength and maxlength.

### Task 3: Number Range
Add min and max to a number field.

### Task 4: Email Validation
Use email input type with required.

### Task 5: Custom Message
Add a custom validation message.
