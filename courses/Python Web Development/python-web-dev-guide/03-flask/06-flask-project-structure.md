# Flask Project Structure

## What You'll Learn
- How to structure larger Flask applications
- Using Blueprints for modular organization
- Application factory pattern
- Configuration management
- Best practices for Flask projects

## Prerequisites
- Completed Flask Database section
- Understanding of Flask fundamentals

## Why Project Structure Matters

As your Flask application grows, organization becomes crucial. A good structure:
- Makes code easier to find and maintain
- Enables team collaboration
- Simplifies testing
- Supports scalability

## The Application Factory Pattern

The **application factory** pattern creates the Flask app inside a function. This allows:
- Multiple configurations (dev, test, production)
- Easier testing
- Circular import prevention

### Basic Factory Pattern

```python
# app/__init__.py
from flask import Flask

def create_app(config_name: str = "development") -> Flask:
    """Application factory to create and configure the Flask app."""
    app = Flask(__name__)
    
    # Load configuration
    if config_name == "development":
        app.config.from_object("config.DevelopmentConfig")
    elif config_name == "testing":
        app.config.from_object("config.TestingConfig")
    else:
        app.config.from_object("config.ProductionConfig")
    
    # Initialize extensions
    from extensions import db
    db.init_app(app)
    
    # Register blueprints
    from routes.main import main_bp
    from routes.auth import auth_bp
    from routes.blog import blog_bp
    
    app.register_blueprint(main_bp)
    app.register_blueprint(auth_bp, url_prefix="/auth")
    app.register_blueprint(blog_bp, url_prefix="/blog")
    
    # Create tables
    with app.app_context():
        db.create_all()
    
    return app
```

### Configuration Classes

```python
# config.py
import os

class Config:
    """Base configuration."""
    SECRET_KEY = os.environ.get("SECRET_KEY") or "dev-secret-key"
    SQLALCHEMY_TRACK_MODIFICATIONS = False

class DevelopmentConfig(Config):
    """Development configuration."""
    DEBUG = True
    SQLALCHEMY_DATABASE_URI = "sqlite:///dev.db"

class TestingConfig(Config):
    """Testing configuration."""
    TESTING = True
    SQLALCHEMY_DATABASE_URI = "sqlite:///:memory:"

class ProductionConfig(Config):
    """Production configuration."""
    DEBUG = False
    SQLALCHEMY_DATABASE_URI = os.environ.get("DATABASE_URL")
```

### Running the Application

```python
# run.py
from app import create_app

app = create_app()

if __name__ == "__main__":
    app.run()
```

## Complete Project Structure

```
my_flask_app/
├── app/
│   ├── __init__.py           # Application factory
│   ├── extensions.py         # Flask extensions
│   ├── models.py             # Database models
│   ├── config.py             # Configuration
│   │
│   ├── routes/
│   │   ├── __init__.py
│   │   ├── main.py          # Main routes (home, about)
│   │   ├── auth.py          # Authentication routes
│   │   └── blog.py          # Blog routes
│   │
│   ├── templates/
│   │   ├── base.html
│   │   ├── main/
│   │   │   ├── index.html
│   │   │   └── about.html
│   │   ├── auth/
│   │   │   ├── login.html
│   │   │   └── register.html
│   │   └── blog/
│   │       ├── index.html
│   │       └── post.html
│   │
│   └── static/
│       ├── css/
│       │   └── style.css
│       ├── js/
│       │   └── main.js
│       └── images/
│
├── tests/
│   ├── __init__.py
│   ├── test_routes.py
│   └── test_models.py
│
├── venv/                     # Virtual environment
├── requirements.txt
├── .gitignore
└── run.py
```

## Blueprints in Practice

### Creating Blueprints

```python
# app/routes/main.py
from flask import Blueprint, render_template

main_bp = Blueprint("main", __name__)

@main_bp.route("/")
def index() -> str:
    return render_template("main/index.html")

@main_bp.route("/about")
def about() -> str:
    return render_template("main/about.html")
```

```python
# app/routes/blog.py
from flask import Blueprint, render_template, request, redirect, url_for
from flask_login import login_required, current_user
from app.extensions import db
from app.models import Post

blog_bp = Blueprint("blog", __name__, url_prefix="/blog")

@blog_bp.route("/")
def index() -> str:
    posts: list[Post] = Post.query.order_by(Post.created_at.desc()).all()
    return render_template("blog/index.html", posts=posts)

@blog_bp.route("/post/<int:post_id>")
def view_post(post_id: int) -> str:
    post: Post | None = Post.query.get_or_404(post_id)
    return render_template("blog/post.html", post=post)

@blog_bp.route("/create", methods=["GET", "POST"])
@login_required
def create_post() -> str:
    if request.method == "POST":
        title: str = request.form.get("title", "").strip()
        content: str = request.form.get("content", "").strip()
        
        post: Post = Post(
            title=title,
            content=content,
            author=current_user
        )
        db.session.add(post)
        db.session.commit()
        
        return redirect(url_for("blog.view_post", post_id=post.id))
    
    return render_template("blog/create.html")
```

### Registering Blueprints

```python
# app/__init__.py
def create_app(config_name: str = "development") -> Flask:
    app = Flask(__name__)
    
    # ... config setup ...
    
    # Register blueprints
    app.register_blueprint(main_bp)
    app.register_blueprint(auth_bp, url_prefix="/auth")
    app.register_blueprint(blog_bp, url_prefix="/blog")
    
    return app
```

## Extensions Setup

```python
# app/extensions.py
from flask_sqlalchemy import SQLAlchemy
from flask_login import LoginManager
from flask_migrate import Migrate

# Initialize extensions
db = SQLAlchemy()
migrate = Migrate()
login_manager = LoginManager()

def init_extensions(app: Flask) -> None:
    """Initialize Flask extensions with the app."""
    db.init_app(app)
    migrate.init_app(app, db)
    login_manager.init_app(app)
    login_manager.login_view = "auth.login"
```

## URL Building with Blueprints

When using blueprints, reference the blueprint name in `url_for()`:

```html
<!-- templates/base.html -->
<a href="{{ url_for('main.index') }}">Home</a>
<a href="{{ url_for('blog.index') }}">Blog</a>
<a href="{{ url_for('auth.login') }}">Login</a>
```

In Python:
```python
# Inside blog blueprint
return redirect(url_for("blog.view_post", post_id=post.id))
```

## Best Practices

### 1. Environment Variables

```python
# .env (add to .gitignore)
FLASK_APP=run.py
FLASK_ENV=development
SECRET_KEY=your-secret-key
DATABASE_URL=sqlite:///dev.db
```

```python
# config.py
import os
from dotenv import load_dotenv

load_dotenv()  # Load .env file

class Config:
    SECRET_KEY = os.environ.get("SECRET_KEY")
    SQLALCHEMY_DATABASE_URI = os.environ.get("DATABASE_URL")
```

### 2. Use Application Context

```python
# When accessing Flask objects outside routes
with app.app_context():
    # Database operations
    db.create_all()
    user: User = User.query.first()
```

### 3. Error Handling

```python
# app/__init__.py
def create_app(config_name: str = "development") -> Flask:
    # ... setup ...
    
    # Register error handlers
    from errors import register_error_handlers
    register_error_handlers(app)
    
    return app
```

```python
# app/errors.py
from flask import render_template

def register_error_handlers(app: Flask) -> None:
    @app.errorhandler(404)
    def page_not_found(e) -> tuple:
        return render_template("errors/404.html"), 404
    
    @app.errorhandler(500)
    def internal_error(e) -> tuple:
        return render_template("errors/500.html"), 500
```

### 4. Logging

```python
import logging

def create_app(config_name: str = "development") -> Flask:
    app = Flask(__name__)
    
    # Configure logging
    if not app.debug:
        logging.basicConfig(level=logging.INFO)
        # Add file handler for production
    
    return app
```

## Summary
- Use the **application factory pattern** for flexible app creation
- Organize code with **Blueprints** for different features
- Separate **configuration** for different environments
- Use **extensions** for database, login, migrations
- Keep templates in subfolders matching blueprints
- Use **environment variables** for secrets
- Register **error handlers** for better UX

## Next Steps
→ Continue to `../04-fastapi/01-fastapi-introduction.md` to learn about FastAPI, a modern alternative to Flask.
