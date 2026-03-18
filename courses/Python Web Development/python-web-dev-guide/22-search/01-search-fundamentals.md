# Search Fundamentals

## What You'll Learn
- Database search
- Full-text search
- Search ranking

## Prerequisites
- Completed payments folder

## Simple Database Search

```python
from fastapi import FastAPI
from sqlalchemy import or_

app = FastAPI()

@app.get("/search")
async def search_products(query: str):
    """Simple search in database"""
    results = db.query(Product).filter(
        or_(
            Product.name.ilike(f"%{query}%"),
            Product.description.ilike(f"%{query}%")
        )
    ).all()
    return results
```

## Limitations

- No fuzzy matching
- No ranking
- Slow on large datasets
- Can't handle typos

## When to Use Search Engines

- Large datasets (100k+ records)
- Need fuzzy matching
- Need relevance ranking
- Complex queries

## Summary
- Use database for simple searches
- Use search engine for complex needs

## Next Steps
→ Continue to `02-elasticsearch-basics.md`
