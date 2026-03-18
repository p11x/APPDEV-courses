# Todo App with Flask

## What You'll Learn
- Building a complete todo application
- Using Flask with SQLite
- Implementing CRUD operations

## Prerequisites
- Completed Flask fundamentals

## Project Overview

Build a todo list app where users can:
- View all todos
- Add new todos
- Mark todos as complete
- Delete todos

## Step 1: Setup

```bash
mkdir todo-app
cd todo-app
python -m venv venv
venv\Scripts\activate  # Windows
source venv/bin/activate  # Mac/Linux
pip install flask flask-sqlalchemy
```

## Step 2: Application Code

```python
# app.py
from flask import Flask, render_template, request, redirect, url_for
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///todos.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False
db = SQLAlchemy(app)

class Todo(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(200), nullable=False)
    completed = db.Column(db.Boolean, default=False)

with app.app_context():
    db.create_all()

@app.route("/")
def index():
    todos = Todo.query.all()
    return render_template("index.html", todos=todos)

@app.route("/add", methods=["POST"])
def add():
    title = request.form.get("title")
    todo = Todo(title=title)
    db.session.add(todo)
    db.session.commit()
    return redirect(url_for("index"))

@app.route("/toggle/<int:id>")
def toggle(id):
    todo = Todo.query.get(id)
    todo.completed = not todo.completed
    db.session.commit()
    return redirect(url_for("index"))

@app.route("/delete/<int:id>")
def delete(id):
    todo = Todo.query.get(id)
    db.session.delete(todo)
    db.session.commit()
    return redirect(url_for("index"))

if __name__ == "__main__":
    app.run(debug=True)
```

## Step 3: Templates

```html
<!-- templates/index.html -->
<!DOCTYPE html>
<html>
<head>
    <title>Todo App</title>
    <style>
        body { font-family: Arial; max-width: 600px; margin: 50px auto; }
        .todo { padding: 10px; margin: 5px 0; display: flex; justify-content: space-between; }
        .completed { text-decoration: line-through; color: gray; }
        form { margin-bottom: 20px; }
    </style>
</head>
<body>
    <h1>Todo List</h1>
    
    <form action="/add" method="POST">
        <input type="text" name="title" placeholder="New todo" required>
        <button type="submit">Add</button>
    </form>
    
    {% for todo in todos %}
    <div class="todo {% if todo.completed %}completed{% endif %}">
        <span>{{ todo.title }}</span>
        <div>
            <a href="/toggle/{{ todo.id }}">✓</a>
            <a href="/delete/{{ todo.id }}">✗</a>
        </div>
    </div>
    {% endfor %}
</body>
</html>
```

## Step 4: Run

```bash
python app.py
```

Visit http://127.0.0.1:5000

## Summary
- Built a complete Flask todo app
- Used SQLAlchemy for database
- Implemented full CRUD
