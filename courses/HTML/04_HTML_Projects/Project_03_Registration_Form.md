# Project 3: Registration Form

## Project Overview

### Project Title
User Registration Form

### Project Description
Create a comprehensive user registration form with multiple input types, proper validation, and accessible structure. This project demonstrates intermediate form handling skills.

### Learning Objectives

- Create forms with various input types
- Implement client-side validation
- Use proper label associations
- Apply accessibility best practices

### Estimated Duration
2-3 hours

---

## Project Requirements

### Required Fields

1. **Account Information**
   - Username (text, required, min 3 chars)
   - Email (email, required)
   - Password (password, required, min 8 chars)
   - Confirm Password (password, required)

2. **Personal Information**
   - First Name (text, required)
   - Last Name (text, required)
   - Date of Birth (date)
   - Phone Number (tel)

3. **Preferences**
   - Newsletter subscription (checkbox)
   - Terms acceptance (checkbox, required)

4. **Security**
   - CAPTCHA or verification
   - Form validation

---

## Complete Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Create Account - Registration</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background: #f5f5f5;
            margin: 0;
            padding: 20px;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            background: white;
            padding: 40px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        h1 {
            text-align: center;
            color: #333;
            margin-bottom: 10px;
        }
        .subtitle {
            text-align: center;
            color: #666;
            margin-bottom: 30px;
        }
        fieldset {
            border: 1px solid #ddd;
            padding: 20px;
            margin-bottom: 20px;
            border-radius: 5px;
        }
        legend {
            font-weight: bold;
            color: #333;
            padding: 0 10px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        label {
            display: block;
            margin-bottom: 5px;
            color: #333;
            font-weight: 500;
        }
        input[type="text"],
        input[type="email"],
        input[type="password"],
        input[type="tel"],
        input[type="date"],
        select,
        textarea {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-sizing: border-box;
            font-size: 14px;
        }
        input:focus {
            outline: none;
            border-color: #007bff;
            box-shadow: 0 0 5px rgba(0,123,255,0.3);
        }
        input:invalid {
            border-color: red;
        }
        input:valid {
            border-color: green;
        }
        .checkbox-group {
            display: flex;
            align-items: center;
        }
        .checkbox-group input {
            width: auto;
            margin-right: 10px;
        }
        .checkbox-group label {
            margin-bottom: 0;
        }
        .required::after {
            content: " *";
            color: red;
        }
        button {
            width: 100%;
            padding: 15px;
            background: #007bff;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: background 0.3s;
        }
        button:hover {
            background: #0056b3;
        }
        .login-link {
            text-align: center;
            margin-top: 20px;
            color: #666;
        }
        .login-link a {
            color: #007bff;
            text-decoration: none;
        }
        .error-message {
            color: red;
            font-size: 12px;
            margin-top: 5px;
            display: none;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>Create Account</h1>
        <p class="subtitle">Join our community today</p>
        
        <form action="/register" method="POST" id="registrationForm">
            
            <!-- Account Information -->
            <fieldset>
                <legend>Account Information</legend>
                
                <div class="form-group">
                    <label for="username" class="required">Username</label>
                    <input type="text" 
                           id="username" 
                           name="username" 
                           required 
                           minlength="3" 
                           maxlength="20"
                           pattern="[a-zA-Z0-9]+"
                           title="Letters and numbers only, 3-20 characters"
                           placeholder="Enter username">
                    <span class="error-message">Username must be 3-20 characters with letters and numbers only</span>
                </div>
                
                <div class="form-group">
                    <label for="email" class="required">Email Address</label>
                    <input type="email" 
                           id="email" 
                           name="email" 
                           required
                           placeholder="you@example.com">
                    <span class="error-message">Please enter a valid email address</span>
                </div>
                
                <div class="form-group">
                    <label for="password" class="required">Password</label>
                    <input type="password" 
                           id="password" 
                           name="password" 
                           required 
                           minlength="8"
                           pattern="(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}"
                           title="Must contain at least one number, one uppercase and lowercase letter, and at least 8 characters"
                           placeholder="Enter password">
                    <span class="error-message">Password must be at least 8 characters with uppercase, lowercase, and number</span>
                </div>
                
                <div class="form-group">
                    <label for="confirm-password" class="required">Confirm Password</label>
                    <input type="password" 
                           id="confirm-password" 
                           name="confirm-password" 
                           required
                           placeholder="Confirm your password">
                    <span class="error-message">Passwords do not match</span>
                </div>
            </fieldset>
            
            <!-- Personal Information -->
            <fieldset>
                <legend>Personal Information</legend>
                
                <div class="form-group">
                    <label for="first-name" class="required">First Name</label>
                    <input type="text" 
                           id="first-name" 
                           name="first-name" 
                           required
                           placeholder="Enter first name">
                </div>
                
                <div class="form-group">
                    <label for="last-name" class="required">Last Name</label>
                    <input type="text" 
                           id="last-name" 
                           name="last-name" 
                           required
                           placeholder="Enter last name">
                </div>
                
                <div class="form-group">
                    <label for="birthdate">Date of Birth</label>
                    <input type="date" 
                           id="birthdate" 
                           name="birthdate"
                           max="2006-01-01">
                </div>
                
                <div class="form-group">
                    <label for="phone">Phone Number</label>
                    <input type="tel" 
                           id="phone" 
                           name="phone"
                           pattern="[0-9]{3}-[0-9]{3}-[0-9]{4}"
                           placeholder="123-456-7890">
                </div>
                
                <div class="form-group">
                    <label for="country">Country</label>
                    <select id="country" name="country">
                        <option value="">Select your country</option>
                        <option value="us">United States</option>
                        <option value="uk">United Kingdom</option>
                        <option value="ca">Canada</option>
                        <option value="au">Australia</option>
                        <option value="other">Other</option>
                    </select>
                </div>
            </fieldset>
            
            <!-- Preferences -->
            <fieldset>
                <legend>Preferences</legend>
                
                <div class="form-group">
                    <label for="newsletter">
                        <input type="checkbox" 
                               id="newsletter" 
                               name="newsletter" 
                               value="yes">
                        Subscribe to our newsletter for updates and special offers
                    </label>
                </div>
                
                <div class="form-group">
                    <label for="terms" class="required">
                        <input type="checkbox" 
                               id="terms" 
                               name="terms" 
                               required>
                        I agree to the <a href="/terms">Terms of Service</a> and <a href="/privacy">Privacy Policy</a> *
                    </label>
                </div>
            </fieldset>
            
            <!-- Submit Button -->
            <button type="submit">Create Account</button>
            
            <p class="login-link">
                Already have an account? <a href="/login">Sign in here</a>
            </p>
        </form>
    </div>
    
    <script>
        // Client-side validation
        const form = document.getElementById('registrationForm');
        const password = document.getElementById('password');
        const confirmPassword = document.getElementById('confirm-password');
        
        // Password match validation
        confirmPassword.addEventListener('input', function() {
            if (confirmPassword.value !== password.value) {
                confirmPassword.setCustomValidity('Passwords do not match');
            } else {
                confirmPassword.setCustomValidity('');
            }
        });
        
        // Form submission
        form.addEventListener('submit', function(event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                alert('Please fill in all required fields correctly');
            } else {
                // Show success message (in real app, would submit to server)
                alert('Registration successful!');
            }
        });
    </script>
</body>
</html>
```

---

## Project Checklist

### Form Structure (10 points)
- [ ] Proper form element with action and method
- [ ] Fieldsets for grouping related fields
- [ ] Legends for fieldset descriptions

### Input Fields (15 points)
- [ ] Text input for username
- [ ] Email input for email
- [ ] Password inputs with validation
- [ ] Date input for birthdate
- [ ] Tel input for phone
- [ ] Select dropdown for country

### Validation (15 points)
- [ ] Required attributes on mandatory fields
- [ ] Minlength/maxlength attributes
- [ ] Pattern validation for usernames
- [ ] Email format validation
- [ ] Password confirmation match
- [ ] Terms checkbox required

### Accessibility (10 points)
- [ ] Labels properly associated with inputs
- [ ] Required fields marked with asterisk
- [ ] Focus states visible
- [ ] Error messages accessible

### Styling (5 points)
- [ ] Professional appearance
- [ ] Clear visual hierarchy
- [ ] Mobile responsive

---

## Grading Rubric

| Criteria | Excellent (A) | Good (B) | Needs Work (C) |
|----------|---------------|----------|----------------|
| Structure | Perfect form structure | Good structure | Incomplete |
| Validation | All validations working | Most validations | Missing |
| Accessibility | Fully accessible | Partially accessible | Not accessible |
| Code Quality | Clean, organized | Acceptable | Needs work |
