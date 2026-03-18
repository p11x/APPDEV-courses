# Embeddings and Vector Databases

## What You'll Learn
- Text embeddings
- Vector databases
- Semantic search

## Prerequisites
- Completed prompt engineering

## Generating Embeddings

```python
response = openai.embeddings.create(
    model="text-embedding-ada-002",
    input="Hello, world!"
)

embedding = response.data[0].embedding
print(len(embedding))  # 1536 dimensions
```

## Using Vector DB

```bash
pip install pinecone
```

```python
import pinecone

pinecone.init(api_key="...", environment="us-west1")
index = pinecone.Index("my-index")

# Upsert vectors
index.upsert(vectors=[
    ("id1", [0.1, 0.2, ...], {"text": "Document 1"}),
    ("id2", [0.3, 0.4, ...], {"text": "Document 2"}),
])

# Query
query_embedding = openai.embeddings.create(
    model="text-embedding-ada-002",
    input="search query"
).data[0].embedding

results = index.query(
    vector=query_embedding,
    top_k=3
)
```

## Summary
- Embeddings for semantic search
- Pinecone, Weaviate for storage
- Use for RAG applications

## Next Steps
→ Continue to `05-llm-applications.md`
