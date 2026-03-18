# Elasticsearch Basics

## What You'll Learn
- Elasticsearch concepts
- Indexing documents
- Querying

## Prerequisites
- Completed search fundamentals

## Concepts

- **Index**: Like a database
- **Document**: Like a row
- **Field**: Like a column

## Installation

```bash
pip install elasticsearch
```

## Basic Operations

```python
from elasticsearch import Elasticsearch

es = Elasticsearch(["http://localhost:9200"])

# Create index
es.indices.create(index="products")

# Index document
es.index(
    index="products",
    id=1,
    document={
        "name": "iPhone 15",
        "description": "Latest iPhone",
        "price": 999
    }
)

# Search
result = es.search(
    index="products",
    query={"match": {"name": "iPhone"}}
)

for hit in result["hits"]["hits"]:
    print(hit["_source"])
```

## Summary
- Elasticsearch is powerful but complex
- Good for large-scale search

## Next Steps
→ Continue to `03-meilisearch.md`
