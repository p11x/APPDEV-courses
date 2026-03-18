# LLM Applications

## What You'll Learn
- RAG architecture
- Chatbots
- AI agents

## Prerequisites
- Completed embeddings

## RAG (Retrieval Augmented Generation)

```python
async def rag_query(query: str):
    # 1. Embed query
    query_emb = openai.embeddings.create(
        model="text-embedding-ada-002",
        input=query
    ).data[0].embedding
    
    # 2. Retrieve relevant docs
    docs = index.query(vector=query_emb, top_k=3)
    
    # 3. Build context
    context = "\n".join([d["metadata"]["text"] for d in docs])
    
    # 4. Generate answer
    response = openai.chat.completions.create(
        model="gpt-4",
        messages=[
            {"role": "system", "content": f"Context: {context}"},
            {"role": "user", "content": query}
        ]
    )
    
    return response.choices[0].message.content
```

## Summary
- RAG for knowledge bases
- Fine-tune for specific tasks
- Build AI agents

## Next Steps
→ Continue to `06-image-generation.md`
