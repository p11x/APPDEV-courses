# Meilisearch

## What You'll Learn
- Meilisearch basics
- Simple integration
- Typo tolerance

## Prerequisites
- Completed Elasticsearch basics

## Why Meilisearch?

- Easy to set up
- Built-in typo tolerance
- Instant results
- Open source

## Installation

```bash
pip install meilisearch
```

## Basic Usage

```python
import meilisearch

client = meilisearch.Client("http://localhost:7700", "masterKey")

# Create index
client.create_index("products", {"primaryKey": "id"})

# Add documents
client.index("products").add_documents([
    {"id": 1, "name": "iPhone", "price": 999},
    {"id": 2, "name": "Samsung", "price": 899}
])

# Search
results = client.index("products").search("iphon")

print(results["hits"])
```

## FastAPI Integration

```python
from fastapi import FastAPI, Query
import meilisearch

app = FastAPI()
client = meilisearch.Client("http://localhost:7700", "masterKey")

@app.get("/search")
async def search_products(q: str = Query(...)):
    results = client.index("products").search(q)
    return results
```

## Summary
- Meilisearch is simpler than Elasticsearch
- Great typo tolerance
- Easy to set up

## Next Steps
→ Continue to `04-search-integration.md`
