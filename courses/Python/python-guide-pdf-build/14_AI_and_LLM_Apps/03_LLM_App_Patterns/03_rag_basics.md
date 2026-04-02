# 📚 RAG: Retrieval-Augmented Generation

## 🎯 What You'll Learn

- What RAG is and why it matters
- Building a simple keyword-based RAG system
- Understanding embeddings for semantic search
- When RAG beats fine-tuning

## 📦 Prerequisites

- Understanding of Claude API basics

---

## What is RAG?

RAG lets Claude answer questions about YOUR data without retraining:

```
User Query → Search Your Docs → Get Top K Chunks → Inject into Prompt → Claude Answers
```

---

## Simple Keyword RAG

```python
from pathlib import Path
import json

class SimpleRAG:
    """Simple keyword-based RAG system."""
    
    def __init__(self, docs_folder: Path):
        self.docs_folder = docs_folder
        self.chunks = self._load_and_chunk()
    
    def _load_and_chunk(self) -> list[dict]:
        """Load documents and split into chunks."""
        chunks = []
        
        for file_path in self.docs_folder.rglob("*.txt"):
            text = file_path.read_text(encoding="utf-8")
            
            # Simple chunking: split by paragraphs
            for i, para in enumerate(text.split("\n\n")):
                if para.strip():
                    chunks.append({
                        "file": str(file_path),
                        "chunk_id": i,
                        "text": para.strip()
                    })
        
        return chunks
    
    def _keyword_search(self, query: str, top_k: int = 3) -> list[dict]:
        """Find chunks with most keyword overlap."""
        query_words = set(query.lower().split())
        
        scored = []
        for chunk in self.chunks:
            chunk_words = set(chunk["text"].lower().split())
            overlap = len(query_words & chunk_words)
            scored.append((overlap, chunk))
        
        scored.sort(reverse=True)
        return [chunk for _, chunk in scored[:top_k]]
    
    def ask(self, client, question: str) -> str:
        """Ask a question using RAG."""
        # 1. Find relevant chunks
        chunks = self._keyword_search(question)
        
        # 2. Build context
        context = "\n\n".join(
            f"[From {c['file']}]:\n{c['text']}" 
            for c in chunks
        )
        
        # 3. Ask Claude with context
        prompt = f"""Based on these documents, answer the question.

Documents:
{context}

Question: {question}

Answer based only on the provided documents."""

        response = client.messages.create(
            model="claude-sonnet-4-5",
            max_tokens=500,
            messages=[{"role": "user", "content": prompt}]
        )
        
        return response.content[0].text

# Use it
rag = SimpleRAG(Path("my_documents"))
answer = rag.ask(client, "What is the refund policy?")
print(answer)
```

---

## Embedding-Based RAG

```python
# Install sentence-transformers
# pip install sentence-transformers

from sentence_transformers import SentenceTransformer
import numpy as np
from pathlib import Path

class EmbeddingRAG:
    """RAG using semantic embeddings."""
    
    def __init__(self, docs_folder: Path):
        self.model = SentenceTransformer("all-MiniLM-L6-v2")
        self.docs_folder = docs_folder
        self.chunks = self._load_chunks()
        self.embeddings = self._compute_embeddings()
    
    def _load_chunks(self) -> list[dict]:
        chunks = []
        for file_path in self.docs_folder.rglob("*.txt"):
            text = file_path.read_text()
            # Split into chunks of ~500 chars
            for i in range(0, len(text), 500):
                chunk = text[i:i+500]
                if chunk.strip():
                    chunks.append({"text": chunk, "source": str(file_path)})
        return chunks
    
    def _compute_embeddings(self):
        texts = [c["text"] for c in self.chunks]
        return self.model.encode(texts)
    
    def _find_similar(self, query: str, top_k: int = 3) -> list[dict]:
        query_emb = self.model.encode([query])
        
        # Cosine similarity
        similarities = np.dot(self.embeddings, query_emb.T).flatten()
        top_indices = np.argsort(similarities)[-top_k:][::-1]
        
        return [self.chunks[i] for i in top_indices]
    
    def ask(self, client, question: str) -> str:
        chunks = self._find_similar(question)
        
        context = "\n\n".join(c["text"] for c in chunks)
        
        response = client.messages.create(
            model="claude-sonnet-4-5",
            max_tokens=500,
            messages=[{
                "role": "user",
                "content": f"Based on:\n{context}\n\nQuestion: {question}"
            }]
        )
        
        return response.content[0].text
```

---

## ✅ Summary

- RAG injects relevant documents into the prompt
- Simple RAG uses keyword overlap for retrieval
- Embedding-based RAG uses semantic similarity
- RAG beats fine-tuning for most hobbyist use cases

## ➡️ Next Steps

Continue to folder [15_Python_for_Platforms/01_FastAPI/01_fastapi_basics.md](../15_Python_for_Platforms/01_FastAPI/01_fastapi_basics.md) to learn FastAPI.

## 🔗 Further Reading

- [LangChain](https://python.langchain.com/) - popular RAG framework
- [sentence-transformers](https://sbert.net/)
