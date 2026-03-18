# Background Job Patterns

## What You'll Learn
- Common background job patterns
- Handling large datasets
- Batch processing
- Event-driven architecture

## Prerequisites
- Completed task queues folder

## Batch Processing Pattern

```python
from celery import Celery, group
import math

app = Celery('tasks', broker='redis://localhost:6379/0')

@app.task
def process_single_item(item_id: int) -> dict:
    """Process one item"""
    return {'item_id': item_id, 'processed': True}

@app.task
def batch_process(items: list) -> dict:
    """Process batch of items"""
    results = []
    for item in items:
        result = process_single_item.delay(item)
        results.append(result.id)
    return {'queued': len(results)}

@app.route('/batch-process', methods=['POST'])
def batch_endpoint():
    # Process in chunks of 100
    all_items = list(range(1, 1001))
    chunks = [all_items[i:i+100] for i in range(0, len(all_items), 100)]
    
    jobs = group(process_single_item.s(item) for chunk in chunks for item in chunk)
    result = jobs.apply_async()
    
    return {'job_group_id': result.id, 'total_items': len(all_items)}
```

## Chunk Processing Pattern

```python
from celery import Celery

app = Celery('tasks', broker='redis://localhost:6379/0')

@app.task
def process_chunk(chunk: list) -> list:
    """Process a chunk of data"""
    results = []
    for item in chunk:
        # Process each item
        results.append({'id': item, 'status': 'processed'})
    return results

def process_large_dataset(items: list, chunk_size: int = 1000):
    """Split large dataset into chunks"""
    chunks = [items[i:i+chunk_size] for i in range(0, len(items), chunk_size)]
    
    jobs = [process_chunk.delay(chunk) for chunk in chunks]
    return [job.id for job in jobs]
```

## Pipeline Pattern

```python
from celery import chain

@app.task
def extract_data(source: str) -> dict:
    """Step 1: Extract"""
    return {'source': source, 'raw_data': [1, 2, 3]}

@app.task
def transform_data(data: dict) -> dict:
    """Step 2: Transform"""
    return {'transformed': [x * 2 for x in data['raw_data']]}

@app.task
def load_data(data: dict) -> dict:
    """Step 3: Load"""
    return {'loaded': len(data['transformed'])}

# Pipeline execution
pipeline = chain(
    extract_data.s('database'),
    transform_data.s(),
    load_data.s()
)
result = pipeline.apply_async()
```

## Event-Driven Pattern

```python
from celery import signals

@app.task
def on_task_success_handler(sender=None, result=None, **kwargs):
    """Handle successful task"""
    print(f"Task {sender.request.id} completed with result: {result}")

@app.task
def on_task_failure_handler(sender=None, exception=None, **kwargs):
    """Handle failed task"""
    print(f"Task {sender.request.id} failed: {exception}")

# Register handlers
signals.task_success.connect(on_task_success_handler)
signals.task_failure.connect(on_task_failure_handler)
```

## Summary
- Use batch processing for large datasets
- Implement chunking to avoid memory issues
- Chain tasks for pipelines
- Use signals for event handling

## Next Steps
→ Move to `17-security/`
