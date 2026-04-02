# Elasticsearch Integration

## Overview

Elasticsearch provides powerful full-text search capabilities for FastAPI applications.

## Setup

### Elasticsearch Client

```python
# Example 1: Elasticsearch setup
from elasticsearch import AsyncElasticsearch
from fastapi import FastAPI

app = FastAPI()

es = AsyncElasticsearch(
    hosts=["http://localhost:9200"],
    retry_on_timeout=True,
    max_retries=3
)

@app.on_event("shutdown")
async def shutdown():
    await es.close()
```

## Search Operations

### Index and Search

```python
# Example 2: Search operations
async def index_document(index: str, doc_id: str, document: dict):
    """Index a document"""
    await es.index(index=index, id=doc_id, document=document)

async def search_documents(index: str, query: str):
    """Full-text search"""
    result = await es.search(
        index=index,
        query={
            "multi_match": {
                "query": query,
                "fields": ["title^2", "content"]
            }
        }
    )
    return [hit["_source"] for hit in result["hits"]["hits"]]

@app.get("/search/")
async def search(q: str):
    results = await search_documents("products", q)
    return {"results": results}
```

## Summary

Elasticsearch enables powerful search functionality.

## Next Steps

Continue learning about:
- [Redis Integration](./02_redis_integration.md)
- [MongoDB Integration](./01_mongodb_integration.md)
