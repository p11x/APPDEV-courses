---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Datastore
Purpose: Hands-on exercises for Cloud Datastore database management
Difficulty: advanced
Prerequisites: 01_Basic_Cloud_Datastore.md, 02_Advanced_Cloud_Datastore.md
RelatedFiles: 01_Basic_Cloud_Datastore.md, 02_Advanced_Cloud_Datastore.md
UseCase: NoSQL database operations, scalable application development
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Cloud Datastore is essential for building NoSQL data-driven applications and managing large-scale document storage.

### Lab Goals

- Create and query entities
- Configure indexes
- Implement transactions

## 📖 WHAT

### Exercise Overview

1. **Entity Operations**: CRUD on entities
2. **Index Configuration**: Complex queries
3. **Transactions**: Batch operations

## 🔧 HOW

### Exercise 1: Entity Operations

```python
#!/usr/bin/env python3
# Cloud Datastore entity operations

from google.cloud import datastore
import uuid

def create_entities():
    client = datastore.Client()
    
    # Create entities
    tasks = [
        {'title': 'Buy groceries', 'done': False, 'priority': 2},
        {'title': 'Walk the dog', 'done': True, 'priority': 1},
        {'title': 'Pay bills', 'done': False, 'priority': 3},
    ]
    
    for task in tasks:
        key = client.key('Task', f"task_{uuid.uuid4()}")
        entity = datastore.Entity(key)
        entity.update(task)
        client.put(entity)
    
    print(f"Created {len(tasks)} tasks")

def query_entities():
    client = datastore.Client()
    
    # Simple query
    query = client.query(kind='Task')
    query.add_filter('done', '=', False)
    results = list(query.fetch())
    
    print(f"Found {len(results)} incomplete tasks")
    
    # Projection query
    proj_query = client.query(kind='Task')
    proj_query.projection = ['title', 'priority']
    results = list(proj_query.fetch(limit=10))
    
    for entity in results:
        print(f"Task: {entity['title']}, Priority: {entity['priority']}")

if __name__ == '__main__':
    create_entities()
    query_entities()
```

### Exercise 2: Index Configuration

```bash
#!/bin/bash
# Configure Datastore indexes

# Create index configuration
cat > index.yaml << 'EOF'
indexes:
- kind: Task
  properties:
  - name: done
    direction: asc
  - name: priority
    direction: desc

- kind: Task
  properties:
  - name: done
    direction: asc
  - name: created
    direction: asc

- kind: Task
  properties:
  - name: priority
    direction: desc
  - name: due_date
    direction: asc

- kind: Task
  properties:
  - name: tags
    direction: asc
  - name: done
    direction: asc
EOF

# Deploy indexes
gcloud datastore indexes create index.yaml

# List indexes
gcloud datastore indexes list

# Wait for building
echo "Waiting for indexes to build..."

# Test complex query
gcloud datastore entities query \
    --kind=Task \
    --filter="done = false AND priority >= 2"

echo "Index configuration complete!"
```

### Exercise 3: Transactions

```python
#!/usr/bin/env python3
# Cloud Datastore transactions

from google.cloud import datastore
from datetime import datetime

def transactional_update(client, task_id, new_status):
    """Update task with transaction"""
    with client.transaction():
        key = client.key('Task', task_id)
        task = client.get(key)
        
        if task:
            task['done'] = new_status
            task['updated_at'] = datetime.utcnow()
            client.put(task)
            print(f"Updated task {task_id}")
        else:
            print(f"Task {task_id} not found")

def atomic_counter(client, task_id):
    """Atomic counter using transaction"""
    with client.transaction():
        key = client.key('Counter', task_id)
        counter = client.get(key)
        
        if counter:
            counter['count'] += 1
        else:
            counter = datastore.Entity(key)
            counter['count'] = 1
        
        client.put(counter)
        return counter['count']

def batch_operations():
    """Batch create and delete"""
    client = datastore.Client()
    
    # Batch insert
    entities = []
    for i in range(100):
        key = client.key('Task', f"task_{i}")
        entity = datastore.Entity(key)
        entity['title'] = f"Task {i}"
        entity['done'] = False
        entities.append(entity)
    
    client.put_multi(entities)
    print("Batch insert complete")
    
    # Batch delete
    keys = [client.key('Task', f"task_{i}") for i in range(100)]
    client.delete_multi(keys)
    print("Batch delete complete")

if __name__ == '__main__':
    client = datastore.Client()
    transactional_update(client, 'task_123', True)
    count = atomic_counter(client, 'view_count')
    print(f"Count: {count}")
    batch_operations()
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Query fails | Check indexes |
| Transaction fails | Check entity groups |
| High costs | Use projection |

### Validation

```bash
# Check entities
gcloud datastore entities query --kind=Task --limit=10

# List indexes
gcloud datastore indexes list
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Functions: Triggers
- Cloud Run: Deployment
- Cloud Logging: Audit logs

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Firestore
- Cloud Functions
- Cloud Run

### Next Steps

- Migrate to Firestore
- Set up monitoring
- Configure access control

## ✅ EXAM TIPS

- Practice entity operations
- Know composite indexes
- Use transactions wisely
- Monitor costs
