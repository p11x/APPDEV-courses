# Full Stack App

## What You'll Learn
- Combining frontend and backend
- Connecting React to FastAPI

## Prerequisites
- Completed Flask and FastAPI basics
- Basic JavaScript knowledge

## Project Overview

Build a full stack app with:
- FastAPI backend (REST API)
- React frontend
- PostgreSQL database

## Architecture

```
┌─────────────┐      ┌─────────────┐      ┌─────────────┐
│   React     │ ←──→ │   FastAPI   │ ←──→ │ PostgreSQL  │
│   Frontend  │      │   Backend   │      │  Database   │
└─────────────┘      └─────────────┘      └─────────────┘
```

## Step 1: Backend

```python
# main.py
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel

app = FastAPI()

# Enable CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:3000"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

class Item(BaseModel):
    name: str
    description: str | None = None

items_db = []

@app.get("/items")
def get_items():
    return items_db

@app.post("/items")
def create_item(item: Item):
    items_db.append(item)
    return item

@app.delete("/items/{index}")
def delete_item(index: int):
    items_db.pop(index)
    return {"message": "Deleted"}
```

## Step 2: Frontend (React)

```bash
npx create-react-app frontend
cd frontend
npm install axios
```

```jsx
// src/App.js
import { useState, useEffect } from 'react';
import axios from 'axios';

function App() {
  const [items, setItems] = useState([]);
  const [name, setName] = useState('');
  const [desc, setDesc] = useState('');

  useEffect(() => {
    fetchItems();
  }, []);

  const fetchItems = async () => {
    const response = await axios.get('http://localhost:8000/items');
    setItems(response.data);
  };

  const addItem = async () => {
    await axios.post('http://localhost:8000/items', {
      name,
      description: desc
    });
    setName('');
    setDesc('');
    fetchItems();
  };

  const deleteItem = async (index) => {
    await axios.delete(`http://localhost:8000/items/${index}`);
    fetchItems();
  };

  return (
    <div>
      <h1>Full Stack App</h1>
      <input value={name} onChange={e => setName(e.target.value)} placeholder="Name" />
      <input value={desc} onChange={e => setDesc(e.target.value)} placeholder="Description" />
      <button onClick={addItem}>Add</button>
      
      {items.map((item, index) => (
        <div key={index}>
          <h3>{item.name}</h3>
          <p>{item.description}</p>
          <button onClick={() => deleteItem(index)}>Delete</button>
        </div>
      ))}
    </div>
  );
}

export default App;
```

## Step 3: Run

Terminal 1:
```bash
uvicorn main:app --reload
```

Terminal 2:
```bash
npm start
```

## Summary
- Connected React to FastAPI
- Used CORS to allow cross-origin requests
- Built full stack CRUD application
