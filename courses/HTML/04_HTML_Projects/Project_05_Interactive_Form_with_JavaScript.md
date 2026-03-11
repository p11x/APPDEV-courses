# Project 5: Interactive Form with JavaScript Validation

## Project Overview

### Project Title
Interactive Registration Form with JavaScript Validation

### Project Description
Create a comprehensive registration form with real-time JavaScript validation, custom error messages, and interactive user feedback. This project demonstrates form handling with JavaScript.

### Learning Objectives

- Implement client-side validation with JavaScript
- Create custom error messages
- Add real-time validation feedback
- Form data handling

### Estimated Duration
3-4 hours

---

## Project Requirements

### Validation Features

1. **Real-time Validation**
   - Validate as user types
   - Show errors immediately
   - Clear errors on correction

2. **Field Validation**
   - Username: 3-20 characters, alphanumeric
   - Email: Valid email format
   - Password: Min 8 chars, uppercase, lowercase, number
   - Confirm Password: Must match

3. **Visual Feedback**
   - Green border for valid
   - Red border for invalid
   - Error messages below fields

---

## Complete Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Interactive Registration Form</title>
    <style>
        * { box-sizing: border-box; }
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }
        .container {
            background: white;
            padding: 40px;
            border-radius: 10px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.2);
            max-width: 500px;
            width: 100%;
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
        .form-group {
            margin-bottom: 20px;
        }
        label {
            display: block;
            margin-bottom: 5px;
            color: #333;
            font-weight: 600;
        }
        input {
            width: 100%;
            padding: 12px;
            border: 2px solid #ddd;
            border-radius: 5px;
            font-size: 14px;
            transition: all 0.3s;
        }
        input:focus {
            outline: none;
            border-color: #667eea;
        }
        input.valid {
            border-color: #28a745;
            background: #f8fff8;
        }
        input.invalid {
            border-color: #dc3545;
            background: #fff8f8;
        }
        .error-message {
            color: #dc3545;
            font-size: 12px;
            margin-top: 5px;
            display: none;
        }
        .error-message.show {
            display: block;
        }
        .success-icon {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            color: #28a745;
            display: none;
        }
        .input-wrapper {
            position: relative;
        }
        .input-wrapper input.valid + .success-icon {
            display: block;
        }
        button {
            width: 100%;
            padding: 15px;
            background: #667eea;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: background 0.3s;
        }
        button:hover {
            background: #5568d3;
        }
        button:disabled {
            background: #ccc;
            cursor: not-allowed;
        }
        .progress-bar {
            height: 5px;
            background: #e0e0e0;
            border-radius: 3px;
            margin-top: 5px;
            overflow: hidden;
        }
        .progress-fill {
            height: 100%;
            width: 0;
            transition: width 0.3s, background 0.3s;
        }
        .password-requirements {
            background: #f8f9fa;
            padding: 15px;
            border-radius: 5px;
            margin-top: 10px;
        }
        .requirement {
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: 12px;
            color: #666;
            margin-bottom: 5px;
        }
        .requirement.valid {
            color: #28a745;
        }
        .requirement::before {
            content: '○';
        }
        .requirement.valid::before {
            content: '✓';
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>Create Account</h1>
        <p class="subtitle">Join our community today</p>
        
        <form id="registrationForm" novalidate>
            <!-- Username -->
            <div class="form-group">
                <label for="username">Username</label>
                <div class="input-wrapper">
                    <input type="text" id="username" name="username" 
                           placeholder="Enter username">
                    <span class="success-icon">✓</span>
                </div>
                <div class="error-message" id="username-error"></div>
            </div>
            
            <!-- Email -->
            <div class="form-group">
                <label for="email">Email Address</label>
                <div class="input-wrapper">
                    <input type="email" id="email" name="email" 
                           placeholder="Enter email">
                    <span class="success-icon">✓</span>
                </div>
                <div class="error-message" id="email-error"></div>
            </div>
            
            <!-- Password -->
            <div class="form-group">
                <label for="password">Password</label>
                <input type="password" id="password" name="password" 
                       placeholder="Enter password">
                <div class="progress-bar">
                    <div class="progress-fill" id="password-strength"></div>
                </div>
                <div class="password-requirements">
                    <div class="requirement" id="req-length">At least 8 characters</div>
                    <div class="requirement" id="req-upper">One uppercase letter</div>
                    <div class="requirement" id="req-lower">One lowercase letter</div>
                    <div class="requirement" id="req-number">One number</div>
                </div>
                <div class="error-message" id="password-error"></div>
            </div>
            
            <!-- Confirm Password -->
            <div class="form-group">
                <label for="confirm-password">Confirm Password</label>
                <div class="input-wrapper">
                    <input type="password" id="confirm-password" name="confirm-password" 
                           placeholder="Confirm password">
                    <span class="success-icon">✓</span>
                </div>
                <div class="error-message" id="confirm-error"></div>
            </div>
            
            <!-- Submit Button -->
            <button type="submit" id="submit-btn" disabled>Create Account</button>
        </form>
    </div>
    
    <script>
        // Form Elements
        const form = document.getElementById('registrationForm');
        const username = document.getElementById('username');
        const email = document.getElementById('email');
        const password = document.getElementById('password');
        const confirmPassword = document.getElementById('confirm-password');
        const submitBtn = document.getElementById('submit-btn');
        
        // Validation State
        const validationState = {
            username: false,
            email: false,
            password: false,
            confirmPassword: false
        };
        
        // Validate Username
        function validateUsername() {
            const value = username.value;
            const errorEl = document.getElementById('username-error');
            
            if (value.length < 3) {
                showError(username, errorEl, 'Username must be at least 3 characters');
                return false;
            }
            
            if (!/^[a-zA-Z0-9]+$/.test(value)) {
                showError(username, errorEl, 'Username can only contain letters and numbers');
                return false;
            }
            
            if (value.length > 20) {
                showError(username, errorEl, 'Username cannot exceed 20 characters');
                return false;
            }
            
            showSuccess(username, errorEl);
            return true;
        }
        
        // Validate Email
        function validateEmail() {
            const value = email.value;
            const errorEl = document.getElementById('email-error');
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            
            if (!value) {
                showError(email, errorEl, 'Email is required');
                return false;
            }
            
            if (!emailRegex.test(value)) {
                showError(email, errorEl, 'Please enter a valid email address');
                return false;
            }
            
            showSuccess(email, errorEl);
            return true;
        }
        
        // Validate Password
        function validatePassword() {
            const value = password.value;
            const errorEl = document.getElementById('password-error');
            const requirements = {
                length: value.length >= 8,
                upper: /[A-Z]/.test(value),
                lower: /[a-z]/.test(value),
                number: /[0-9]/.test(value)
            };
            
            // Update requirement indicators
            document.getElementById('req-length').classList.toggle('valid', requirements.length);
            document.getElementById('req-upper').classList.toggle('valid', requirements.upper);
            document.getElementById('req-lower').classList.toggle('valid', requirements.lower);
            document.getElementById('req-number').classList.toggle('valid', requirements.number);
            
            // Update progress bar
            const strength = Object.values(requirements).filter(Boolean).length;
            const progressFill = document.getElementById('password-strength');
            progressFill.style.width = (strength * 25) + '%';
            progressFill.style.background = strength < 2 ? '#dc3545' : 
                                           strength < 4 ? '#ffc107' : '#28a745';
            
            if (!requirements.length) {
                showError(password, errorEl, 'Password must be at least 8 characters');
                return false;
            }
            
            if (!Object.values(requirements).every(Boolean)) {
                showError(password, errorEl, 'Password does not meet all requirements');
                return false;
            }
            
            showSuccess(password, errorEl);
            return true;
        }
        
        // Validate Confirm Password
        function validateConfirmPassword() {
            const value = confirmPassword.value;
            const errorEl = document.getElementById('confirm-error');
            
            if (value !== password.value) {
                showError(confirmPassword, errorEl, 'Passwords do not match');
                return false;
            }
            
            if (!value) {
                showError(confirmPassword, errorEl, 'Please confirm your password');
                return false;
            }
            
            showSuccess(confirmPassword, errorEl);
            return true;
        }
        
        // Show Error
        function showError(input, errorEl, message) {
            input.classList.remove('valid');
            input.classList.add('invalid');
            errorEl.textContent = message;
            errorEl.classList.add('show');
            validationState[input.id] = false;
            updateSubmitButton();
        }
        
        // Show Success
        function showSuccess(input, errorEl) {
            input.classList.remove('invalid');
            input.classList.add('valid');
            errorEl.classList.remove('show');
            validationState[input.id] = true;
            updateSubmitButton();
        }
        
        // Update Submit Button
        function updateSubmitButton() {
            const allValid = Object.values(validationState).every(Boolean);
            submitBtn.disabled = !allValid;
        }
        
        // Event Listeners - Real-time validation
        username.addEventListener('input', validateUsername);
        username.addEventListener('blur', validateUsername);
        
        email.addEventListener('input', validateEmail);
        email.addEventListener('blur', validateEmail);
        
        password.addEventListener('input', validatePassword);
        password.addEventListener('blur', validatePassword);
        
        confirmPassword.addEventListener('input', validateConfirmPassword);
        confirmPassword.addEventListener('blur', validateConfirmPassword);
        
        // Form Submission
        form.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Final validation
            const isValid = [
                validateUsername(),
                validateEmail(),
                validatePassword(),
                validateConfirmPassword()
            ].every(Boolean);
            
            if (isValid) {
                alert('Registration successful! Welcome to our community!');
                form.reset();
                document.querySelectorAll('.valid').forEach(el => el.classList.remove('valid'));
                document.querySelectorAll('.requirement').forEach(el => el.classList.remove('valid'));
                document.getElementById('password-strength').style.width = '0';
                Object.keys(validationState).forEach(key => validationState[key] = false);
                updateSubmitButton();
            }
        });
    </script>
</body>
</html>
```

---

## Project Checklist

### Validation Logic (20 points)
- [ ] Username validation (length, characters)
- [ ] Email validation (format)
- [ ] Password validation (requirements)
- [ ] Confirm password match

### Visual Feedback (15 points)
- [ ] Valid state styling
- [ ] Invalid state styling
- [ ] Error messages display
- [ ] Success indicators

### User Experience (10 points)
- [ ] Real-time validation
- [ ] Password strength indicator
- [ ] Requirements display
- [ ] Form submission handling
