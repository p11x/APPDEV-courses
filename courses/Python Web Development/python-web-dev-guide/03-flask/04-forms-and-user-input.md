# Forms and User Input

## What You'll Learn
- Creating HTML forms
- Handling form submissions
- Accessing form data
- Validating user input
- Using Flask-WTF for form handling
- Flash messages for feedback

## Prerequisites
- Completed Flask Templates
- Understanding of HTML forms

## HTML Forms Review

Forms are the primary way to collect user input on the web:

```html
<form action="/submit" method="POST">
    <div>
        <label for="username">Username:</label>
        <input type="text" id="username" name="username" required>
    </div>
    
    <div>
        <label for="email">Email:</label>
        <input type="email" id="email" name="email" required>
    </div>
    
    <div>
        <label for="password">Password:</label>
        <input type="password" id="password" name="password" minlength="8" required>
    </div>
    
    <div>
        <label>
            <input type="checkbox" name="remember"> Remember me
        </label>
    </div>
    
    <div>
        <label for="message">Message:</label>
        <textarea id="message" name="message" rows="4"></textarea>
    </div>
    
    <button type="submit">Submit</button>
</form>
```

## Handling Forms in Flask

### Basic Form Handling

```python
from flask import Flask, request, render_template_string

app = Flask(__name__)

# In-memory storage
users: list[dict[str, str]] = []

HTML_TEMPLATE = """
<!DOCTYPE html>
<html>
<head><title>User Registration</title></head>
<body>
    <h1>Register</h1>
    
    {% with messages = get_flashed_messages() %}
        {% if messages %}
            <ul style="color: green;">
            {% for message in messages %}
                <li>{{ message }}</li>
            {% endfor %}
            </ul>
        {% endif %}
    {% endwith %}
    
    <form method="POST">
        <div>
            <label>Username: <input type="text" name="username" required></label>
        </div>
        <div>
            <label>Email: <input type="email" name="email" required></label>
        </div>
        <div>
            <label>Password: <input type="password" name="password" required></label>
        </div>
        <button type="submit">Register</button>
    </form>
    
    <h2>Registered Users</h2>
    <ul>
    {% for user in users %}
        <li>{{ user.username }} ({{ user.email }})</li>
    {% endfor %}
    </ul>
</body>
</html>
"""

@app.route("/", methods=["GET", "POST"])
def register() -> str:
    if request.method == "POST":
        # Get form data
        username: str = request.form.get("username", "").strip()
        email: str = request.form.get("email", "").strip()
        password: str = request.form.get("password", "")
        
        # Simple validation
        if not username or not email or not password:
            return "All fields are required", 400
        
        # Save user (in memory)
        users.append({
            "username": username,
            "email": email
        })
        
        return f"User {username} registered!"
    
    return render_template_string(HTML_TEMPLATE, users=users)
```

🔍 **Form Handling Breakdown:**

1. `methods=["GET", "POST"]` — Accept both GET (show form) and POST (process data)
2. `request.form` — Access form data as a dictionary-like object
3. `request.form.get("field")` — Get value with optional default
4. `.strip()` — Remove whitespace from strings
5. Return response after processing POST

### Accessing Different Data Types

```python
from flask import request

@app.route("/process", methods=["POST"])
def process_form() -> str:
    # Text input
    name: str = request.form.get("name", "")
    
    # Multiple values (checkboxes with same name)
    hobbies: list[str] = request.form.getlist("hobbies")
    
    # File upload
    file = request.files.get("avatar")
    if file:
        filename: str = file.filename
    
    # Query parameters (for GET requests)
    page: str = request.args.get("page", "1")
    
    # JSON data (for API requests)
    json_data: dict | None = request.get_json()
    
    return f"Processed: {name}, hobbies: {hobbies}"
```

## Using Flask-WTF

**Flask-WTF** provides form classes that handle validation and rendering:

### Installation

```bash
pip install flask-wtf wtforms email-validator
```

### Creating Forms

```python
# forms.py
from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, TextAreaField, BooleanField
from wtforms.validators import DataRequired, Email, Length, EqualTo

class RegistrationForm(FlaskForm):
    """User registration form."""
    username = StringField(
        "Username",
        validators=[
            DataRequired(message="Username is required"),
            Length(min=3, max=20, message="Username must be 3-20 characters")
        ]
    )
    
    email = StringField(
        "Email",
        validators=[
            DataRequired(message="Email is required"),
            Email(message="Invalid email address")
        ]
    )
    
    password = PasswordField(
        "Password",
        validators=[
            DataRequired(message="Password is required"),
            Length(min=8, message="Password must be at least 8 characters")
        ]
    )
    
    confirm_password = PasswordField(
        "Confirm Password",
        validators=[
            DataRequired(),
            EqualTo("password", message="Passwords must match")
        ]
    )
    
    bio = TextAreaField(
        "Bio",
        validators=[Length(max=500)]
    )
    
    agree = BooleanField(
        "I agree to the terms",
        validators=[DataRequired(message="You must agree to the terms")]
    )
```

### Using Forms in Views

```python
# app.py
from flask import Flask, render_template, flash, redirect, url_for
from forms import RegistrationForm

app = Flask(__name__)
app.config["SECRET_KEY"] = "your-secret-key"  # Required for CSRF protection

# In-memory database
users: list[dict[str, str]] = []

@app.route("/register", methods=["GET", "POST"])
def register() -> str:
    form = RegistrationForm()
    
    if form.validate_on_submit():
        # Form is valid - process data
        username: str = form.username.data
        email: str = form.email.data
        password: str = form.password.data
        bio: str = form.bio.data
        
        # Save to database (simplified)
        users.append({
            "username": username,
            "email": email,
            "bio": bio
        })
        
        flash(f"Welcome, {username}! Registration successful.", "success")
        return redirect(url_for("register_success", username=username))
    
    # GET request - show form
    return render_template("register.html", form=form)

@app.route("/register-success/<username>")
def register_success(username: str) -> str:
    return f"<h1>Welcome, {username}!</h1><p>Registration successful!</p>"
```

### Rendering Forms in Templates

**templates/register.html:**
```html
<!DOCTYPE html>
<html>
<head>
    <title>Register</title>
    <style>
        .form-group { margin-bottom: 15px; }
        label { display: block; margin-bottom: 5px; }
        input, textarea { padding: 8px; width: 300px; }
        .error { color: red; font-size: 0.9em; }
        .btn { padding: 10px 20px; background: #007bff; color: white; border: none; cursor: pointer; }
    </style>
</head>
<body>
    <h1>Register</h1>
    
    <!-- Flash messages -->
    {% with messages = get_flashed_messages(with_categories=true) %}
        {% if messages %}
            {% for category, message in messages %}
                <div class="alert alert-{{ category }}">{{ message }}</div>
            {% endfor %}
        {% endif %}
    {% endwith %}
    
    <form method="POST">
        <!-- CSRF token (automatic with Flask-WTF) -->
        {{ form.hidden_tag() }}
        
        <div class="form-group">
            {{ form.username.label }}
            {{ form.username(size=30) }}
            {% if form.username.errors %}
                <div class="error">
                    {% for error in form.username.errors %}
                        {{ error }}
                    {% endfor %}
                </div>
            {% endif %}
        </div>
        
        <div class="form-group">
            {{ form.email.label }}
            {{ form.email(size=30) }}
            {% for error in form.email.errors %}
                <div class="error">{{ error }}</div>
            {% endfor %}
        </div>
        
        <div class="form-group">
            {{ form.password.label }}
            {{ form.password(size=30) }}
            {% for error in form.password.errors %}
                <div class="error">{{ error }}</div>
            {% endfor %}
        </div>
        
        <div class="form-group">
            {{ form.confirm_password.label }}
            {{ form.confirm_password(size=30) }}
            {% for error in form.confirm_password.errors %}
                <div class="error">{{ error }}</div>
            {% endfor %}
        </div>
        
        <div class="form-group">
            {{ form.bio.label }}
            {{ form.bio(rows=4) }}
        </div>
        
        <div class="form-group">
            {{ form.agree() }} {{ form.agree.label }}
            {% for error in form.agree.errors %}
                <div class="error">{{ error }}</div>
            {% endfor %}
        </div>
        
        {{ form.submit(class="btn") }}
    </form>
</body>
</html>
```

🔍 **Flask-WTF Features:**

1. `form.hidden_tag()` — Generates CSRF token (prevents cross-site attacks)
2. `form.validate_on_submit()` — Validates form and checks if submitted
3. `form.field.data` — Access cleaned form data
4. `form.field.errors` — List of validation errors
5. Automatic CSRF protection
6. Built-in validators

## Flash Messages

**Flash messages** show temporary feedback to users:

```python
from flask import flash, redirect, url_for

@app.route("/login", methods=["GET", "POST"])
def login() -> str | Response:
    if request.method == "POST":
        # ... check credentials ...
        
        if credentials_valid:
            flash("Login successful!", "success")
            return redirect(url_for("dashboard"))
        else:
            flash("Invalid credentials. Please try again.", "error")
            return redirect(url_for("login"))
    
    return render_template("login.html")
```

In templates:
```html
{% with messages = get_flashed_messages(with_categories=true) %}
    {% if messages %}
        {% for category, message in messages %}
            <div class="alert alert-{{ category }}">{{ message }}</div>
        {% endfor %}
    {% endif %}
{% endwith %}
```

## File Uploads

```python
from flask import request, send_from_directory
from werkzeug.utils import secure_filename
import os

app = Flask(__name__)
app.config["UPLOAD_FOLDER"] = "uploads"
app.config["MAX_CONTENT_LENGTH"] = 16 * 1024 * 1024  # 16MB max

# Create upload folder
os.makedirs(app.config["UPLOAD_FOLDER"], exist_ok=True)

@app.route("/upload", methods=["GET", "POST"])
def upload_file() -> str:
    if request.method == "POST":
        if "file" not in request.files:
            return "No file selected", 400
        
        file = request.files["file"]
        if file.filename == "":
            return "No file selected", 400
        
        if file:
            filename: str = secure_filename(file.filename)
            file.save(os.path.join(app.config["UPLOAD_FOLDER"], filename))
            return f"File {filename} uploaded successfully!"
    
    return '''
    <form method="post" enctype="multipart/form-data">
        <input type="file" name="file">
        <button type="submit">Upload</button>
    </form>
    '''

@app.route("/uploads/<filename>")
def uploaded_file(filename: str):
    return send_from_directory(app.config["UPLOAD_FOLDER"], filename)
```

🔍 **File Upload Security:**

1. `secure_filename()` — Sanitizes filename to prevent directory traversal
2. Check `file.filename == ""` — Ensure file was actually selected
3. `MAX_CONTENT_LENGTH` — Limit upload size
4. Never trust user-provided filenames

## Summary
- Handle forms with `request.form` (form data) and `request.args` (query strings)
- Use Flask-WTF for form classes with built-in validation
- Use `form.validate_on_submit()` to check if form was submitted and is valid
- Use **flash messages** for user feedback
- Use `form.hidden_tag()` for CSRF protection
- Handle **file uploads** with `request.files` and `secure_filename()`

## Next Steps
→ Continue to `05-flask-with-a-database.md` to learn how to integrate a database with Flask.
