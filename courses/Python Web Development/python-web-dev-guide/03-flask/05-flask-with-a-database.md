# Flask with a Database

## What You'll Learn
- Introduction to databases for web apps
- Setting up SQLAlchemy with Flask
- Creating database models
- Performing CRUD operations
- Database migrations with Flask-Migrate
- Querying data

## Prerequisites
- Completed Flask Forms section
- Understanding of basic database concepts (tables, rows)

## Introduction to Databases

A **database** stores your application's data persistently. For web apps, you typically use **relational databases** like SQLite, PostgreSQL, or MySQL.

Think of a database like a spreadsheet:
- **Tables** are like worksheets (Users, Posts, Comments)
- **Columns** are the fields (name, email, age)
- **Rows** are individual records

## Setting Up SQLAlchemy

**SQLAlchemy** is Python's most popular ORM (Object-Relational Mapper). It lets you work with databases using Python objects instead of raw SQL.

### Installation

```bash
pip install flask-sqlalchemy
```

### Configuration

```python
from flask import Flask
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)

# Configure database
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///blog.db"
# Use PostgreSQL for production:
# app.config["SQLALCHEMY_DATABASE_URI"] = "postgresql://user:pass@localhost/blog"

app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

# Initialize SQLAlchemy
db = SQLAlchemy(app)
```

🔍 **Database URI Format:**

1. `"sqlite:///blog.db"` — SQLite (relative path, in project folder)
2. `"sqlite:////absolute/path/blog.db"` — Absolute path
3. `"postgresql://user:pass@localhost/dbname"` — PostgreSQL
4. `"mysql://user:pass@localhost/dbname"` — MySQL

## Creating Models

**Models** are Python classes that represent database tables:

```python
from datetime import datetime
from flask_sqlalchemy import SQLAlchemy

db = SQLAlchemy()

class User(db.Model):
    """User model representing the users table."""
    __tablename__ = "users"  # Table name (optional, auto-generated from class name)
    
    # Columns
    id: db.Column = db.Column(db.Integer, primary_key=True)
    username: db.Column = db.Column(db.String(80), unique=True, nullable=False)
    email: db.Column = db.Column(db.String(120), unique=True, nullable=False)
    password_hash: db.Column = db.Column(db.String(256))
    created_at: db.Column = db.Column(db.DateTime, default=datetime.utcnow)
    is_active: db.Column = db.Column(db.Boolean, default=True)
    
    # Relationship
    posts: db.relationship = db.relationship("Post", backref="author", lazy=True)
    
    def __repr__(self) -> str:
        return f"<User {self.username}>"


class Post(db.Model):
    """Post model representing the posts table."""
    __tablename__ = "posts"
    
    id: db.Column = db.Column(db.Integer, primary_key=True)
    title: db.Column = db.Column(db.String(200), nullable=False)
    content: db.Column = db.Column(db.Text, nullable=False)
    slug: db.Column = db.Column(db.String(200), unique=True)
    created_at: db.Column = db.Column(db.DateTime, default=datetime.utcnow)
    published: db.Column = db.Column(db.Boolean, default=False)
    
    # Foreign key
    user_id: db.Column = db.Column(db.Integer, db.ForeignKey("users.id"), nullable=False)
    
    def __repr__(self) -> str:
        return f"<Post {self.title}>"
```

🔍 **Model Components:**

1. `db.Model` — Base class for all models
2. `db.Column` — Defines a table column
3. `primary_key=True` — Unique identifier for each row
4. `nullable=False` — Column cannot be empty
5. `unique=True` — No duplicate values allowed
6. `default=...` — Default value for new records
7. `db.ForeignKey` — Links to another table
8. `db.relationship` — Links related tables

## Common Column Types

| Type | Python Type | Description |
|------|-------------|-------------|
| `Integer` | `int` | Whole number |
| `String(n)` | `str` | Text, max n characters |
| `Text` | `str` | Long text |
| `Boolean` | `bool` | True/False |
| `DateTime` | `datetime` | Date and time |
| `Float` | `float` | Decimal number |
| `LargeBinary` | `bytes` | Binary data |

## CRUD Operations

### Create (Insert)

```python
# Create a new user
new_user: User = User(
    username="alice",
    email="alice@example.com",
    password_hash="hashed_password_here"
)

# Add to session
db.session.add(new_user)

# Commit (save to database)
db.session.commit()

# Create and commit in one step
user: User = User(username="bob", email="bob@example.com")
db.session.add(user)
db.session.commit()
```

### Read (Query)

```python
# Get all users
all_users: list[User] = User.query.all()

# Get by primary key
user: User | None = User.query.get(1)

# Get by field
user: User | None = User.query.filter_by(username="alice").first()

# Filter with conditions
admin_users: list[User] = User.query.filter_by(is_active=True).all()

# Using filter() for more complex queries
users: list[User] = User.query.filter(
    User.email.like("%@example.com")
).all()

# Get first or create
user, created = User.get_or_create(
    username="alice",
    defaults={"email": "alice@example.com"}
)

# Count
user_count: int = User.query.count()
```

### Update

```python
user: User | None = User.query.get(1)
if user:
    user.email = "newemail@example.com"
    db.session.commit()
    
# Bulk update
User.query.filter_by(is_active=False).update({"is_active": True})
db.session.commit()
```

### Delete

```python
user: User | None = User.query.get(1)
if user:
    db.session.delete(user)
    db.session.commit()

# Bulk delete
User.query.filter_by(is_active=False).delete()
db.session.commit()
```

## Initializing the Database

Create the tables before running the app:

```python
# app.py
from flask import Flask
from models import db, User, Post

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///blog.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

db.init_app(app)

# Create tables
with app.app_context():
    db.create_all()
    
    # Add sample data
    if not User.query.first():
        user: User = User(username="admin", email="admin@example.com")
        db.session.add(user)
        db.session.commit()

@app.route("/")
def index() -> str:
    posts: list[Post] = Post.query.filter_by(published=True).all()
    return f"<h1>Found {len(posts)} posts</h1>"

if __name__ == "__main__":
    app.run(debug=True)
```

## Flask-Migrate for Database Migrations

**Flask-Migrate** handles database schema changes (migrations):

### Installation

```bash
pip install flask-migrate
```

### Setup

```python
# app.py
from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///blog.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

db = SQLAlchemy(app)
migrate = Migrate(app, db)

# Import models so Migrate can see them
from models import User, Post
```

### Migration Commands

```bash
# Initialize migrations (first time only)
flask db init

# Create a migration after changing models
flask db migrate -m "Add published field to Post"

# Apply migrations
flask db upgrade

# Rollback (undo last migration)
flask db downgrade
```

🔍 **Migration Workflow:**

1. `flask db init` — Creates `migrations/` folder
2. Change your model (add field, create new model)
3. `flask db migrate -m "description"` — Creates migration file
4. `flask db upgrade` — Applies changes to database
5. Commit both migration and model changes

## Query Building

### Ordering

```python
# Newest posts first
recent_posts: list[Post] = Post.query.order_by(Post.created_at.desc()).all()

# Oldest first
oldest_posts: list[Post] = Post.query.order_by(Post.created_at.asc()).all()
```

### Pagination

```python
# Get page 1 with 10 items per page
page: int = 1
per_page: int = 10

pagination: flask_sqlalchemy.Pagination = Post.query.paginate(
    page=page, 
    per_page=per_page
)

posts: list[Post] = pagination.items
total: int = pagination.total
has_next: bool = pagination.has_next
has_prev: bool = pagination.has_prev
next_num: int | None = pagination.next_num
prev_num: int | None = pagination.prev_num
```

### Joins

```python
# Get all published posts with their authors
results = db.session.query(Post, User).join(
    User, Post.user_id == User.id
).filter(Post.published == True).all()

for post, author in results:
    print(f"{post.title} by {author.username}")
```

## Complete Example

```python
# app.py
from flask import Flask, render_template, request, redirect, url_for, flash
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///blog.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False
app.config["SECRET_KEY"] = "your-secret-key"

db = SQLAlchemy(app)

# Models
class Post(db.Model):
    id: db.Column = db.Column(db.Integer, primary_key=True)
    title: db.Column = db.Column(db.String(200), nullable=False)
    content: db.Column = db.Column(db.Text, nullable=False)
    created_at: db.Column = db.Column(db.DateTime, default=datetime.utcnow)
    
    def __repr__(self) -> str:
        return f"<Post {self.title}>"

# Create tables
with app.app_context():
    db.create_all()

# Routes
@app.route("/")
def index() -> str:
    posts: list[Post] = Post.query.order_by(Post.created_at.desc()).all()
    return f"""
    <h1>Blog Posts</h1>
    <a href=\"{url_for('new_post')}\">New Post</a>
    <ul>
        {''.join(f'<li>{p.title}: {p.content[:50]}...</li>' for p in posts)}
    </ul>
    """

@app.route("/new", methods=["GET", "POST"])
def new_post() -> str:
    if request.method == "POST":
        title: str = request.form.get("title", "").strip()
        content: str = request.form.get("content", "").strip()
        
        if title and content:
            post: Post = Post(title=title, content=content)
            db.session.add(post)
            db.session.commit()
            flash("Post created!", "success")
            return redirect(url_for("index"))
        else:
            flash("Title and content required", "error")
    
    return """
    <form method="POST">
        <input type="text" name="title" placeholder="Title" required>
        <textarea name="content" placeholder="Content" required></textarea>
        <button type="submit">Create Post</button>
    </form>
    """

if __name__ == "__main__":
    app.run(debug=True)
```

## Summary
- **SQLAlchemy** is an ORM that lets you work with databases using Python
- Define **models** as classes extending `db.Model`
- Use **db.session** to add, commit, and query data
- CRUD = Create, Read, Update, Delete
- Use **Flask-Migrate** for database migrations
- Use **pagination** for large datasets

## Next Steps
→ Continue to `06-flask-project-structure.md` to learn how to structure larger Flask applications.
