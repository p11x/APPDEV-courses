# LangChain Document Loaders

## What You'll Learn

- How to load documents from various sources
- How to split documents into chunks
- How to process loaded documents
- How to combine loaders with embeddings

## Document Loaders

```ts
// loaders.ts

import { TextLoader } from 'langchain/document_loaders/fs/text';
import { JSONLoader } from 'langchain/document_loaders/fs/json';
import { CSVLoader } from 'langchain/document_loaders/fs/csv';
import { PDFLoader } from 'langchain/document_loaders/fs/pdf';
import { DirectoryLoader } from 'langchain/document_loaders/fs/directory';
import { RecursiveCharacterTextSplitter } from 'langchain/text_splitter';

// Load single file
const textLoader = new TextLoader('./docs/guide.txt');
const textDocs = await textLoader.load();

// Load directory
const directoryLoader = new DirectoryLoader('./docs', {
  '.txt': (path) => new TextLoader(path),
  '.pdf': (path) => new PDFLoader(path),
  '.json': (path) => new JSONLoader(path),
});
const allDocs = await directoryLoader.load();

// Split documents into chunks
const splitter = new RecursiveCharacterTextSplitter({
  chunkSize: 1000,      // Characters per chunk
  chunkOverlap: 200,    // Overlap between chunks
});

const chunks = await splitter.splitDocuments(allDocs);
console.log(`Split ${allDocs.length} docs into ${chunks.length} chunks`);
```

## RAG Pipeline

```ts
// rag-pipeline.ts — Complete RAG pipeline

import { TextLoader } from 'langchain/document_loaders/fs/text';
import { RecursiveCharacterTextSplitter } from 'langchain/text_splitter';
import { OpenAIEmbeddings } from '@langchain/openai';
import { MemoryVectorStore } from 'langchain/vectorstores/memory';
import { RetrievalQAChain } from 'langchain/chains';
import { model } from './llm.js';

// Step 1: Load documents
const loader = new TextLoader('./docs/guide.txt');
const docs = await loader.load();

// Step 2: Split into chunks
const splitter = new RecursiveCharacterTextSplitter({
  chunkSize: 1000,
  chunkOverlap: 200,
});
const chunks = await splitter.splitDocuments(docs);

// Step 3: Create embeddings and store
const embeddings = new OpenAIEmbeddings();
const vectorStore = await MemoryVectorStore.fromDocuments(chunks, embeddings);

// Step 4: Create retrieval chain
const chain = RetrievalQAChain.fromLLM(model, vectorStore.asRetriever());

// Step 5: Query
const result = await chain.call({
  query: 'What is the event loop?',
});

console.log(result.text);
```

## Next Steps

For vector databases, continue to [Vector Databases](../03-vector-databases/01-pinecone-setup.md).
