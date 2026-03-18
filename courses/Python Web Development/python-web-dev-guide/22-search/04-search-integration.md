# Search Integration

## What You'll Learn
- Syncing database with search
- Real-time indexing
- Search UI

## Prerequisites
- Completed Meilisearch basics

## Syncing Database with Search

```python
import meilisearch

client = meilisearch.Client("http://localhost:7700")
index = client.index("products")

def sync_products_to_search(products: list):
    """Sync products to search index"""
    documents = [
        {
            "id": p.id,
            "name": p.name,
            "description": p.description,
            "price": p.price,
            "category": p.category
        }
        for p in products
    ]
    
    index.add_documents(documents)

# Call after creating/updating products
@app.post("/products")
async def create_product(product: Product):
    db.add(product)
    db.commit()
    
    # Sync to search
    sync_products_to_search([product])
    
    return product
```

## Real-time with Celery

```python
from celery import Celery

celery = Celery("tasks", broker="redis://localhost:6379/0")

@celery.task
def index_product(product_id: int):
    """Index product in background"""
    product = db.query(Product).get(product_id)
    client.index("products").add_documents([{
        "id": product.id,
        "name": product.name,
        "price": product.price
    }])

@app.post("/products")
async def create_product(product: Product):
    db.add(product)
    db.commit()
    
    # Index asynchronously
    index_product.delay(product.id)
    
    return product
```

## Summary
- Sync data to search engine
- Use background tasks for indexing
- Keep search in sync with database

## Next Steps
→ Move to `23-monitoring-and-logging/`
