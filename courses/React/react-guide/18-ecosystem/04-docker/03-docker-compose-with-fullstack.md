# Docker Compose with Full Stack

## Overview
Docker Compose simplifies running multi-container applications. This guide covers containerizing a complete full-stack React + Express + PostgreSQL application, with hot reload for development and optimized builds for production.

## Prerequisites
- Docker and Docker Compose installed
- Basic understanding of Docker concepts
- Node.js and npm/pnpm knowledge

## Core Concepts

### docker-compose.yml Structure

The docker-compose.yml file defines your entire application stack with services, networks, and volumes. This orchestrates all containers as a single application.

```yaml
# [File: docker-compose.yml]
version: '3.8'

# Top-level compose configuration
services:
  # Service 1: Frontend React application
  frontend:
    build:
      context: ./frontend
      dockerfile: Dockerfile
    ports:
      - "5173:5173"  # Vite dev server port
    environment:
      - VITE_API_URL=http://backend:3001
      - VITE_WS_URL=ws://backend:3001
    volumes:
      - ./frontend/src:/app/src  # Mount source for hot reload
      - /app/node_modules       # Don't mount node_modules
    depends_on:
      - backend
      - database
    networks:
      - app-network

  # Service 2: Backend Express API
  backend:
    build:
      context: ./backend
      dockerfile: Dockerfile
    ports:
      - "3001:3001"
    environment:
      - NODE_ENV=development
      - DATABASE_URL=postgres://user:password@database:5432/todoapp
      - PORT=3001
      - CORS_ORIGIN=http://localhost:5173
    volumes:
      - ./backend/src:/app/src
      - /app/node_modules
    depends_on:
      database:
        condition: service_healthy  # Wait for database to be ready
    networks:
      - app-network

  # Service 3: PostgreSQL database
  database:
    image: postgres:16-alpine
    ports:
      - "5432:5432"
    environment:
      - POSTGRES_USER=user
      - POSTGRES_PASSWORD=password
      - POSTGRES_DB=todoapp
    volumes:
      - postgres_data:/var/lib/postgresql/data  # Named volume for persistence
      - ./database/init.sql:/docker-entrypoint-initdb.d/init.sql  # Init script
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U user -d todoapp"]
      interval: 5s
      timeout: 5s
      retries: 5
    networks:
      - app-network

# Define a bridge network for container communication
networks:
  app-network:
    driver: bridge

# Named volumes persist data between container restarts
volumes:
  postgres_data:
```

### Environment Variables with .env

Docker Compose automatically reads from .env files, making environment configuration simple and secure.

```bash
# [File: .env]
# Database configuration
POSTGRES_USER=user
POSTGRES_PASSWORD=password
POSTGRES_DB=todoapp

# Frontend
VITE_API_URL=http://backend:3001

# Backend
NODE_ENV=development
PORT=3001
DATABASE_URL=postgres://user:password@database:5432/todoapp
CORS_ORIGIN=http://localhost:5173
```

### Frontend Dockerfile for Development

The frontend Dockerfile uses Vite for fast development with hot module replacement (HMR).

```dockerfile
# [File: frontend/Dockerfile]
# Build stage for development
FROM node:20-alpine

# Set working directory inside container
WORKDIR /app

# Copy package files first (layer caching)
COPY package*.json ./

# Install dependencies including devDependencies for Vite
RUN npm ci

# Copy source code
COPY . .

# Expose Vite dev server port
EXPOSE 5173

# Start Vite in development mode with host access
# --host makes the server accessible from outside the container
CMD ["npm", "run", "dev", "--", "--host"]
```

### Backend Dockerfile for Development

The backend Dockerfile uses ts-node for TypeScript execution during development.

```dockerfile
# [File: backend/Dockerfile]
FROM node:20-alpine

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install all dependencies
RUN npm ci

# Copy TypeScript config
COPY tsconfig.json ./

# Copy source code
COPY src/ ./src/

# Install ts-node globally for running TypeScript directly
RUN npm install -g ts-node

EXPOSE 3001

# Start with ts-node-dev for hot reload
# --respawn restarts on file changes
# --transpile-only skips type checking for faster restarts
CMD ["ts-node-dev", "--respawn", "--transpile-only", "src/index.ts"]
```

### Docker Networking Explained

Understanding how containers communicate is crucial. Inside the Docker network, services reach each other by service name, not localhost.

```typescript
// [File: frontend/src/api/client.ts]
import axios from 'axios';

// ❌ WRONG: Using localhost doesn't work from container to container
// The frontend container can't reach backend at localhost:3001
// because localhost refers to the frontend container itself
const badClient = axios.create({
  baseURL: 'http://localhost:3001', // ❌ FAILS!
});

// ✅ CORRECT: Use Docker service name as hostname
// Docker's internal DNS resolves 'backend' to the backend container's IP
const correctClient = axios.create({
  baseURL: 'http://backend:3001', // ✅ WORKS!
});

// For the browser (during development), we use a different URL
// Vite proxies requests or we use localhost
const browserClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:3001',
});

export { correctClient, browserClient };
```

### Volume Mounts for Hot Reload

Bind mounting source directories enables hot reload during development, so code changes reflect immediately.

```yaml
# [File: docker-compose.dev.yml] (additional config)
services:
  frontend:
    volumes:
      # Mount source directory for hot reload
      # Changes to source files trigger Vite rebuild
      - ./frontend/src:/app/src:ro
      
      # Mount public directory
      - ./frontend/public:/app/public:ro
      
      # DON'T mount node_modules — use container's own node_modules
      # This is an anonymous volume that takes precedence
      - /app/node_modules
      
    # Development-specific settings
    stdin_open: true  # Keep STDIN open (for npm run dev)
    tty: true         # Allocate pseudo-TTY

  backend:
    volumes:
      # Mount source for TypeScript hot reload
      - ./backend/src:/app/src:ro
      - /app/node_modules
```

## Common Mistakes

### ❌ Using localhost for Inter-Container Communication / ✅ Fix

```yaml
# ❌ WRONG: Backend trying to connect to localhost:5432
services:
  backend:
    environment:
      - DATABASE_URL=postgres://user:password@localhost:5432/todoapp
      # ❌ localhost in container refers to backend container itself!

# ✅ CORRECT: Use Docker service name
services:
  backend:
    environment:
      - DATABASE_URL=postgres://user:password@database:5432/todoapp
      # ✅ 'database' is the service name from docker-compose.yml
```

### ❌ Not Waiting for Database / ✅ Fix

```yaml
# ❌ WRONG: Backend starts before database is ready
services:
  backend:
    depends_on:
      - database  # Just waits for container to start, not to be ready!

# ✅ CORRECT: Use healthcheck condition
services:
  database:
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U user -d todoapp"]
      interval: 5s
      timeout: 5s
      retries: 5
      
  backend:
    depends_on:
      database:
        condition: service_healthy  # Waits for healthcheck to pass
```

### ❌ Forgetting to Expose Ports / ✅ Fix

```yaml
# ❌ WRONG: Port not exposed, can't access from host
services:
  frontend:
    build: ./frontend
    # Missing ports section!
    # Can't access at localhost:5173

# ✅ CORRECT: Expose ports
services:
  frontend:
    build: ./frontend
    ports:
      - "5173:5173"  # Host port:Container port
```

## Real-World Example: Complete Todo App

This example shows a working full-stack todo application with React frontend, Express backend, and PostgreSQL database.

```typescript
// [File: backend/src/index.ts]
import express from 'express';
import cors from 'cors';
import { Pool } from 'pg';

const app = express();
const port = process.env.PORT || 3001;

// Connect to PostgreSQL using Docker service name
const pool = new Pool({
  connectionString: process.env.DATABASE_URL,
});

app.use(cors({
  origin: process.env.CORS_ORIGIN || 'http://localhost:5173',
}));
app.use(express.json());

// Get all todos
app.get('/api/todos', async (req, res) => {
  try {
    const result = await pool.query('SELECT * FROM todos ORDER BY created_at DESC');
    res.json(result.rows);
  } catch (error) {
    console.error('Error fetching todos:', error);
    res.status(500).json({ error: 'Failed to fetch todos' });
  }
});

// Create todo
app.post('/api/todos', async (req, res) => {
  const { title } = req.body;
  try {
    const result = await pool.query(
      'INSERT INTO todos (title) VALUES ($1) RETURNING *',
      [title]
    );
    res.status(201).json(result.rows[0]);
  } catch (error) {
    console.error('Error creating todo:', error);
    res.status(500).json({ error: 'Failed to create todo' });
  }
});

// Toggle todo
app.patch('/api/todos/:id', async (req, res) => {
  const { id } = req.params;
  const { completed } = req.body;
  try {
    const result = await pool.query(
      'UPDATE todos SET completed = $1 WHERE id = $2 RETURNING *',
      [completed, id]
    );
    res.json(result.rows[0]);
  } catch (error) {
    console.error('Error updating todo:', error);
    res.status(500).json({ error: 'Failed to update todo' });
  }
});

// Delete todo
app.delete('/api/todos/:id', async (req, res) => {
  const { id } = req.params;
  try {
    await pool.query('DELETE FROM todos WHERE id = $1', [id]);
    res.status(204).send();
  } catch (error) {
    console.error('Error deleting todo:', error);
    res.status(500).json({ error: 'Failed to delete todo' });
  }
});

app.listen(port, () => {
  console.log(`Server running on port ${port}`);
});
```

```typescript
// [File: frontend/src/App.tsx]
import { useState, useEffect } from 'react';
import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:3001';

interface Todo {
  id: number;
  title: string;
  completed: boolean;
}

function App() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [newTodo, setNewTodo] = useState('');

  useEffect(() => {
    fetchTodos();
  }, []);

  const fetchTodos = async () => {
    try {
      const response = await axios.get(`${API_URL}/api/todos`);
      setTodos(response.data);
    } catch (error) {
      console.error('Failed to fetch todos:', error);
    }
  };

  const addTodo = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newTodo.trim()) return;

    try {
      const response = await axios.post(`${API_URL}/api/todos`, {
        title: newTodo,
      });
      setTodos([response.data, ...todos]);
      setNewTodo('');
    } catch (error) {
      console.error('Failed to add todo:', error);
    }
  };

  const toggleTodo = async (id: number, completed: boolean) => {
    try {
      await axios.patch(`${API_URL}/api/todos/${id}`, { completed: !completed });
      setTodos(todos.map(todo =>
        todo.id === id ? { ...todo, completed: !completed } : todo
      ));
    } catch (error) {
      console.error('Failed to toggle todo:', error);
    }
  };

  const deleteTodo = async (id: number) => {
    try {
      await axios.delete(`${API_URL}/api/todos/${id}`);
      setTodos(todos.filter(todo => todo.id !== id));
    } catch (error) {
      console.error('Failed to delete todo:', error);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 p-8">
      <div className="max-w-md mx-auto">
        <h1 className="text-3xl font-bold mb-6">Todo App</h1>
        
        <form onSubmit={addTodo} className="flex gap-2 mb-6">
          <input
            type="text"
            value={newTodo}
            onChange={(e) => setNewTodo(e.target.value)}
            placeholder="Add a todo..."
            className="flex-1 px-4 py-2 rounded-lg border"
          />
          <button
            type="submit"
            className="px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600"
          >
            Add
          </button>
        </form>

        <ul className="space-y-2">
          {todos.map((todo) => (
            <li
              key={todo.id}
              className="flex items-center gap-3 bg-white p-4 rounded-lg shadow"
            >
              <input
                type="checkbox"
                checked={todo.completed}
                onChange={() => toggleTodo(todo.id, todo.completed)}
                className="w-5 h-5"
              />
              <span className={`flex-1 ${todo.completed ? 'line-through text-gray-400' : ''}`}>
                {todo.title}
              </span>
              <button
                onClick={() => deleteTodo(todo.id)}
                className="text-red-500 hover:text-red-700"
              >
                Delete
              </button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}

export default App;
```

```sql
-- [File: database/init.sql]
-- Initialize database schema
CREATE TABLE IF NOT EXISTS todos (
  id SERIAL PRIMARY KEY,
  title VARCHAR(255) NOT NULL,
  completed BOOLEAN DEFAULT FALSE,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Insert some sample data
INSERT INTO todos (title, completed) VALUES 
  ('Learn Docker', false),
  ('Build a full-stack app', true),
  ('Deploy to production', false);
```

### Docker Compose Commands

Essential commands for managing your development environment.

```bash
# [File: Terminal commands]

# Start all services (builds images if needed)
docker-compose up --build

# Start in background
docker-compose up -d

# View logs for all services
docker-compose logs -f

# View logs for specific service
docker-compose logs -f backend

# Stop all services
docker-compose down

# Stop and remove volumes (database data)
docker-compose down -v

# Rebuild specific service
docker-compose build frontend

# Restart specific service
docker-compose restart backend

# View running containers
docker-compose ps

# Execute command in running container
docker-compose exec backend sh

# Scale a service (run multiple instances)
docker-compose up -d --scale backend=3
```

## Key Takeaways

- Use Docker Compose to orchestrate multi-container applications
- Services communicate via service names, not localhost
- Use healthchecks with `depends_on` condition for database readiness
- Bind mount source directories for hot reload during development
- Store sensitive data in .env files, not in Dockerfiles
- Named volumes persist data between container restarts

## What's Next

Continue to [README.md](../README.md) to see the master index of the entire React development guide.