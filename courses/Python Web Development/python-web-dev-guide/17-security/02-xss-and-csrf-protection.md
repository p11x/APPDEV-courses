# XSS and CSRF Protection

## What You'll Learn
- Cross-Site Scripting (XSS) prevention
- Cross-Site Request Forgery (CSRF) protection
- Content Security Policy
- Safe template rendering

## Prerequisites
- Completed web security fundamentals

## XSS (Cross-Site Scripting)

XSS attacks inject malicious scripts into web pages viewed by other users.

### Reflected XSS

```python
# VULNERABLE - Don't do this!
@app.get("/search")
def search(q: str):
    # Directly inserting user input into HTML
    return f"<h1>Results for: {q}</h1>"

# SAFE - Using templates that auto-escape
@app.get("/search")
def search(q: str):
    # Jinja2 auto-escapes by default
    return render_template("search.html", query=q)
```

### Stored XSS

```python
# VULNERABLE
@app.post("/comment")
def add_comment(content: str):
    # Storing without sanitization
    db.execute("INSERT INTO comments (content) VALUES (?)", content)
    return {"status": "ok"}

# SAFE - Sanitize before storing
import html

@app.post("/comment")
def add_comment(content: str):
    # Escape HTML entities
    sanitized = html.escape(content)
    db.execute("INSERT INTO comments (content) VALUES (?)", sanitized)
    return {"status": "ok"}
```

## CSRF (Cross-Site Request Forgery)

CSRF tricks users into performing unwanted actions.

### Flask-WTF CSRF Protection

```bash
pip install flask-wtf
```

```python
from flask import Flask, render_template, request
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired

app = Flask(__name__)
app.config['SECRET_KEY'] = 'your-secret-key'  # Change this!

class CommentForm(FlaskForm):
    name = StringField('Name', validators=[DataRequired()])
    comment = StringField('Comment', validators=[DataRequired()])
    submit = SubmitField('Submit')

@app.route('/comment', methods=['GET', 'POST'])
def comment():
    form = CommentForm()
    
    if form.validate_on_submit():
        # CSRF token is automatically validated
        save_comment(form.name.data, form.comment.data)
        return {"status": "success"}
    
    return render_template('comment.html', form=form)
```

```html
<!-- comment.html -->
<form method="POST">
    {{ form.hidden_tag() }}  <!-- CSRF token -->
    {{ form.name() }}
    {{ form.comment() }}
    {{ form.submit() }}
</form>
```

### FastAPI CSRF Protection

```bash
pip install python-multipart
```

```python
from fastapi import FastAPI, Request, Form
from fastapi.templating import Jinja2Templates
from fastapi.csrf import CSRFMiddleware
import secrets

app = FastAPI()
templates = Jinja2Templates(directory="templates")

# Generate secret key
app.add_middleware(
    CSRFMiddleware,
    secret_key=secrets.token_hex(32)
)

@app.get("/contact")
async def contact(request: Request):
    return templates.TemplateResponse("contact.html", {"request": request})

@app.post("/contact")
async def contact(
    request: Request,
    name: str = Form(...),
    message: str = Form(...)
):
    # CSRF validation happens automatically
    return {"status": "success", "name": name}
```

## Content Security Policy

```python
from fastapi import FastAPI
from fastapi.responses import Response

app = FastAPI()

@app.middleware("http")
async def csp_middleware(response: Response):
    """Strict Content Security Policy"""
    response.headers["Content-Security-Policy"] = (
        "default-src 'self'; "
        "script-src 'self' 'unsafe-inline'; "
        "style-src 'self' 'unsafe-inline'; "
        "img-src 'self' data: https:; "
        "font-src 'self'; "
        "connect-src 'self' https://api.example.com; "
        "frame-ancestors 'none';"
    )
    return response
```

## Summary
- Always escape user input in HTML
- Use CSRF tokens for state-changing operations
- Implement strict Content Security Policy
- Use template engines that auto-escape

## Next Steps
→ Continue to `03-sql-injection-prevention.md`
