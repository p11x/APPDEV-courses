<!-- FILE: 11_async_flask_and_background_tasks/04_celery_advanced/01_task_chaining_and_groups.md -->

## Overview

Advanced Celery patterns like task chaining, groups, and chords allow you to compose complex workflows from simple tasks. This file teaches you how to orchestrate multiple tasks that depend on each other, run tasks in parallel, and coordinate complex multi-step processing pipelines.

## Prerequisites

- Celery basics and task creation knowledge
- Understanding of how to run and test Celery tasks

## Core Concepts

### Task Primitives

Celery provides several primitives for combining tasks:

| Primitive | Description | Execution |
|-----------|-------------|----------|
| **Chain** | Tasks run sequentially, output of one feeds into next | Sequential |
| **Group** | Tasks run in parallel, results collected as list | Parallel |
| **Chord** | Group + callback - run tasks, then run callback with results | Parallel then sequential |
| **Map** | Same task with different arguments | Parallel |
| **Starmap** | Same task with different argument tuples | Parallel |

### Chains

A **chain** runs tasks one after another, passing the result of each to the next:

```python
# chain(task1, task2, task3)(args)
# Equivalent to: task3(task2(task1(args)))

chain(add.s(1, 2), multiply.s(10)).apply()  # (1 + 2) * 10 = 30
```

### Groups

A **group** runs multiple tasks in parallel:

```python
# group(task1, task2, task3)(args_list)
# All tasks run at the same time

group(add.s(i, i) for i in range(5)).apply()  # [0, 2, 4, 6, 8]
```

### Chords

A **chord** runs a group of tasks, then runs a callback with all results:

```python
# chord(group)(callback)
# group runs in parallel, then callback gets list of results

chord(
    [fetch_user.s(1), fetch_posts.s(1), fetch_comments.s(1)]
)(combine_data.s()).apply()
```

## Code Walkthrough

### Setup: Flask + Celery App

```python
# app/__init__.py
from flask import Flask
from celery import Celery
from config import config

def make_celery(app=None):
    celery = Celery(
        "app",
        broker=config["development"].CELERY_BROKER_URL,
        backend=config["development"].CELERY_RESULT_BACKEND
    )
    
    if app:
        celery.conf.update(app.config)
        celery.autodiscover_tasks(["app"])
        
        class ContextTask(celery.Task):
            def __call__(self, *args, **kwargs):
                with app.app_context():
                    return self.run(*args, **kwargs)
        
        celery.Task = ContextTask
    
    return celery

def create_app(config_name="development"):
    app = Flask(__name__)
    app.config.from_object(config[config_name])
    
    from app import routes
    app.register_blueprint(routes.bp)
    
    return app

celery = make_celery()
```

### Advanced Task Patterns

```python
# app/tasks.py
from app import celery
import time
import random
from datetime import datetime

# ============================================
# Chain Examples
# ============================================

@celery.task(name="tasks.chain.step1")
def step1(data):
    """First step: Transform data"""
    print(f"Step 1: Processing {data}")
    time.sleep(1)  # Simulate work
    result = {
        "input": data,
        "step1_output": data.upper(),
        "timestamp": datetime.utcnow().isoformat()
    }
    return result

@celery.task(name="tasks.chain.step2")
def step2(previous_result):
    """Second step: Validate result from step 1"""
    print(f"Step 2: Validating {previous_result}")
    time.sleep(0.5)
    
    # Extract output from previous step
    processed_data = previous_result["step1_output"]
    
    result = {
        **previous_result,
        "step2_output": processed_data + "_validated",
        "valid": len(processed_data) > 0
    }
    return result

@celery.task(name="tasks.chain.step3")
def step3(previous_result):
    """Final step: Save result"""
    print(f"Step 3: Saving {previous_result}")
    time.sleep(0.5)
    
    return {
        **previous_result,
        "step3_output": "SAVED",
        "completed_at": datetime.utcnow().isoformat()
    }

# ============================================
# Group Examples
# ============================================

@celery.task(name="tasks.group.process_single")
def process_single(item_id):
    """Process a single item"""
    print(f"Processing item {item_id}")
    time.sleep(random.uniform(0.5, 1.5))
    
    return {
        "item_id": item_id,
        "processed": True,
        "result": item_id * 2
    }

@celery.task(name="tasks.group.aggregate")
def aggregate_results(results):
    """Aggregate results from all parallel tasks"""
    print(f"Aggregating {len(results)} results")
    
    total = sum(r["result"] for r in results)
    return {
        "total_items": len(results),
        "total_sum": total,
        "items": [r["item_id"] for r in results]
    }

# ============================================
# Chord Examples
# ============================================

@celery.task(name="tasks.chord.fetch_user")
def fetch_user(user_id):
    """Fetch user data"""
    print(f"Fetching user {user_id}")
    time.sleep(0.5)
    return {"user_id": user_id, "name": f"User {user_id}", "type": "user"}

@celery.task(name="tasks.chord.fetch_orders")
def fetch_orders(user_id):
    """Fetch user orders"""
    print(f"Fetching orders for user {user_id}")
    time.sleep(1)
    return {"user_id": user_id, "orders": [1, 2, 3], "type": "orders"}

@celery.task(name="tasks.chord.fetch_preferences")
def fetch_preferences(user_id):
    """Fetch user preferences"""
    print(f"Fetching preferences for user {user_id}")
    time.sleep(0.5)
    return {"user_id": user_id, "preferences": {"theme": "dark"}, "type": "preferences"}

@celery.task(name="tasks.chord.combine_data")
def combine_data(results):
    """Combine all fetched data"""
    print(f"Combining {len(results)} results")
    
    user_data = None
    orders_data = None
    preferences_data = None
    
    for r in results:
        if r["type"] == "user":
            user_data = r
        elif r["type"] == "orders":
            orders_data = r
        elif r["type"] == "preferences":
            preferences_data = r
    
    return {
        "user": user_data,
        "orders": orders_data,
        "preferences": preferences_data,
        "combined_at": datetime.utcnow().isoformat()
    }

# ============================================
# Map/Starmap Examples
# ============================================

@celery.task(name="tasks.map.calculate")
def calculate(item):
    """Simple calculation task for mapping"""
    time.sleep(0.3)
    return item * item

@celery.task(name="tasks.starmap.add")
def add_numbers(a, b):
    """Add two numbers"""
    time.sleep(0.3)
    return a + b
```

### Workflow Implementation

```python
# app/workflows.py
from celery import chain, group, chord
from app.tasks import (
    step1, step2, step3,
    process_single, aggregate_results,
    fetch_user, fetch_orders, fetch_preferences, combine_data,
    calculate, add_numbers
)

# ============================================
# Chain Workflows
# ============================================

def run_data_pipeline(initial_data):
    """
    Run a chain of tasks sequentially
    
    Flow: step1 -> step2 -> step3
    Each step receives the output of the previous
    """
    # Method 1: Using chain() function
    result = chain(
        step1.s(initial_data),
        step2.s(),
        step3.s()
    ).apply()
    
    return result

def run_data_pipeline_async(initial_data):
    """
    Queue chain for async execution
    """
    result = chain(
        step1.s(initial_data),
        step2.s(),
        step3.s()
    ).delay()
    
    return result

# ============================================
# Group Workflows  
# ============================================

def process_items_parallel(item_ids):
    """
    Process multiple items in parallel, then aggregate
    
    Flow: [process_single] x N -> aggregate_results
    """
    # Create a group of tasks
    process_group = group(
        process_single.s(item_id) for item_id in item_ids
    )
    
    # Chain group results to aggregation task
    result = chord(process_group)(aggregate_results.s())
    
    return result

def process_items_simple(item_ids):
    """
    Simple parallel processing (just wait for all results)
    """
    process_group = group(
        process_single.s(item_id) for item_id in item_ids
    )
    
    result = process_group.apply()
    # result.results contains list of AsyncResult objects
    
    return [r.get() for r in result.results]

# ============================================
# Complex Workflows
# ============================================

def get_user_dashboard(user_id):
    """
    Complex workflow: Fetch user + orders + preferences in parallel,
    then combine results
    
    Flow: [fetch_user, fetch_orders, fetch_preferences] -> combine_data
    """
    result = chord(
        [
            fetch_user.s(user_id),
            fetch_orders.s(user_id),
            fetch_preferences.s(user_id)
        ]
    )(combine_data.s())
    
    return result

def process_user_upload(user_id, file_list):
    """
    More complex: Process files in parallel, 
    then update user record when all done
    
    Flow: [process_file] x N -> update_user_record
    """
    # Note: This would need a process_file task defined
    file_group = group(
        process_single.s(file_id) for file_id in file_list
    )
    
    # Chord with callback
    result = chord(file_group)(aggregate_results.s())
    
    return result

# ============================================
# Map Operations
# ============================================

def square_numbers(numbers):
    """
    Map: Run same task with different inputs
    
    Equivalent to: [calculate(n) for n in numbers]
    """
    # Using map
    result = calculate.map(numbers)
    return result.apply().get()

def add_pairs(pairs):
    """
    Starmap: Run same task with different argument tuples
    
    Equivalent to: [add(a, b) for (a, b) in pairs]
    """
    # Using starmap
    result = add_numbers.starmap(pairs)
    return result.apply().get()
```

### Flask Routes for Workflows

```python
# app/routes.py
from flask import Blueprint, jsonify, request
from app import create_app, celery
from app.workflows import (
    run_data_pipeline, process_items_parallel,
    get_user_dashboard, square_numbers, add_pairs
)

bp = Blueprint("workflows", __name__)

@bp.route("/chain", methods=["POST"])
def chain_route():
    """
    Start a chain workflow
    
    Request: {"data": "hello"}
    """
    data = request.json.get("data")
    
    # Queue the chain
    task = run_data_pipeline_async(data)
    
    return jsonify({
        "workflow": "chain",
        "task_id": task.id,
        "status": "started"
    })

@bp.route("/parallel", methods=["POST"])
def parallel_route():
    """
    Process items in parallel
    
    Request: {"items": [1, 2, 3, 4, 5]}
    """
    items = request.json.get("items", [])
    
    # Run in parallel and aggregate
    task = process_items_parallel(items)
    
    return jsonify({
        "workflow": "parallel",
        "task_id": task.id,
        "status": "started"
    })

@bp.route("/dashboard/<int:user_id>")
def dashboard_route(user_id):
    """
    Get user dashboard data
    
    Fetches user, orders, preferences in parallel
    """
    task = get_user_dashboard(user_id)
    
    return jsonify({
        "workflow": "dashboard",
        "task_id": task.id
    })

@bp.route("/map", methods=["POST"])
def map_route():
    """
    Map operation
    
    Request: {"numbers": [1, 2, 3, 4, 5]}
    """
    numbers = request.json.get("numbers", [])
    
    task = square_numbers(numbers)
    
    return jsonify({
        "workflow": "map",
        "task_id": task.id
    })

@bp.route("/task-status/<task_id>")
def task_status(task_id):
    """Get task status and result"""
    task = celery.AsyncResult(task_id)
    
    response = {
        "task_id": task.id,
        "status": task.state
    }
    
    if task.ready():
        response["result"] = task.result
    
    return jsonify(response)
```

### Testing the Workflows

```bash
# Start workers (in separate terminals)
celery -A app.celery worker --loglevel=info -Q default,celery

# Run Flask
python run.py

# Test chains
curl -X POST http://localhost:5000/chain \
  -H "Content-Type: application/json" \
  -d '{"data": "test"}'

# Test parallel processing
curl -X POST http://localhost:5000/parallel \
  -H "Content-Type: application/json" \
  -d '{"items": [1, 2, 3, 4, 5]}'

# Test dashboard
curl http://localhost:5000/dashboard/123

# Check status
curl http://localhost:5000/task-status/<TASK_ID>
```

## Common Mistakes

### ❌ Not handling partial results

```python
# WRONG: Chain doesn't handle errors gracefully
chain(task1.s(), task2.s(), task3.s()).apply()
# If task2 fails, task3 never runs
```

### ✅ Handle errors in chains

```python
# CORRECT: Use chord with error handling or task options
@celery.task(bind=True)
def safe_chain(self, data):
    try:
        result1 = step1.delay(data).get(timeout=30)
        result2 = step2.delay(result1).get(timeout=30)
        return step3.delay(result2).get(timeout=30)
    except Exception as e:
        return {"error": str(e)}
```

### ❌ Using wrong signature type

```python
# WRONG: Using wrong signature (.s() vs .s())
group(task.s(item) for item in items)  # .s() for positional args
group(task.s(item_id=item) for item in items)  # .s() with kwargs
```

### ✅ Use correct signature

```python
# CORRECT: Using proper signatures
# .s() - positional arguments
# .s(arg1, arg2) - positional args
# .s(arg1=val1, arg2=val2) - keyword args
# .s(arg1, arg2, arg3=val3) - mixed

group(task.s(item) for item in items)  # Pass each item as single arg
group(task.s(item, extra="value") for item in items)  # With extra arg
```

### ❌ Blocking with .get() in web requests

```python
# WRONG: Calling .get() blocks the request!
@app.route("/bad")
def bad():
    result = slow_task.delay().get()  # Wait here = not async!
    return jsonify(result)
```

### ✅ Return task ID, check later

```python
# CORRECT: Return task ID immediately
@app.route("/good")
def good():
    task = slow_task.delay()
    return jsonify({"task_id": task.id})

@app.route("/status/<task_id>")
def status(task_id):
    task = celery.AsyncResult(task_id)
    if task.ready():
        return jsonify({"result": task.result})
    return jsonify({"status": task.state})
```

## Quick Reference

| Primitive | Syntax | Use Case |
|-----------|--------|----------|
| Chain | `chain(t1.s(), t2.s())` | Sequential dependencies |
| Group | `group(t1.s(), t2.s())` | Parallel execution |
| Chord | `chord(group)(callback)` | Parallel + final callback |
| Map | `task.map(args)` | Same task, different data |
| Starmap | `task.starmap(pairs)` | Same task, different args |

**Chord tips:**

```python
# Chord with specific results passing
chord(
    [fetch_user.s(user_id), fetch_orders.s(user_id)]
)(combine_results.s()).apply()

# Access results in callback by index or type
@celery.task
def combine_results(results):
    user = results[0]  # First task result
    orders = results[1]  # Second task result
```

## Next Steps

Continue to [02_scheduled_periodic_tasks.md](02_scheduled_periodic_tasks.md) to learn how to set up periodic tasks with Celery Beat.