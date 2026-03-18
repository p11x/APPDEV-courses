# Phase 9 — Frontend Integration

## Goal

By the end of this phase, you will have:
- API client for frontend
- Frontend integration examples
- Authentication handling
- WebSocket/streaming support

## Prerequisites

- Completed Phase 8 (deployment)
- Basic JavaScript/React knowledge

## Step-by-Step Implementation

### Step 9.1 — Create API Client

```typescript
// frontend/src/lib/api.ts
import { useState, useCallback } from 'react';

const API_BASE = import.meta.env.VITE_API_URL || 'http://localhost:8000';

interface TokenResponse {
  access_token: string;
  refresh_token: string;
  token_type: string;
}

interface Document {
  id: string;
  title: string;
  file_name: string;
  file_type: string;
  status: string;
  created_at: string;
}

interface ChatMessage {
  role: 'user' | 'assistant';
  content: string;
}

class ApiClient {
  private token: string | null = null;
  
  setToken(token: string | null) {
    this.token = token;
    if (token) {
      localStorage.setItem('token', token);
    } else {
      localStorage.removeItem('token');
    }
  }
  
  getToken() {
    if (!this.token) {
      this.token = localStorage.getItem('token');
    }
    return this.token;
  }
  
  private async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const token = this.getToken();
    
    const headers: HeadersInit = {
      ...options.headers,
    };
    
    if (token) {
      (headers as Record<string, string>)['Authorization'] = `Bearer ${token}`;
    }
    
    const response = await fetch(`${API_BASE}${endpoint}`, {
      ...options,
      headers,
    });
    
    if (!response.ok) {
      const error = await response.json().catch(() => ({}));
      throw new Error(error.detail || 'Request failed');
    }
    
    return response.json();
  }
  
  // Auth
  async register(email: string, username: string, password: string) {
    return this.request<{ id: string; email: string; username: string }>(
      '/auth/register',
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, username, password }),
      }
    );
  }
  
  async login(email: string, password: string): Promise<TokenResponse> {
    return this.request<TokenResponse>('/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password }),
    });
  }
  
  async refreshToken(refreshToken: string): Promise<TokenResponse> {
    return this.request<TokenResponse>('/auth/refresh', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ refresh_token: refreshToken }),
    });
  }
  
  // Documents
  async listDocuments() {
    return this.request<{ documents: Document[]; total: number }>('/documents/');
  }
  
  async uploadDocument(file: File) {
    const formData = new FormData();
    formData.append('file', file);
    
    const response = await fetch(`${API_BASE}/documents/upload`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${this.getToken()}`,
      },
      body: formData,
    });
    
    if (!response.ok) {
      throw new Error('Upload failed');
    }
    
    return response.json();
  }
  
  // Chat
  async sendMessage(message: string, conversationId?: string) {
    return this.request<{ message: string; conversation_id: string; sources: any[] }>(
      '/chat/',
      {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ message, conversation_id: conversationId }),
      }
    );
  }
  
  async *streamMessage(message: string, conversationId?: string) {
    const response = await fetch(`${API_BASE}/chat/stream`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.getToken()}`,
      },
      body: JSON.stringify({ message, conversation_id: conversationId }),
    });
    
    if (!response.ok || !response.body) {
      throw new Error('Stream failed');
    }
    
    const reader = response.body.getReader();
    const decoder = new TextDecoder();
    
    while (true) {
      const { done, value } = await reader.read();
      
      if (done) break;
      
      const chunk = decoder.decode(value);
      const lines = chunk.split('\n');
      
      for (const line of lines) {
        if (line.startsWith('data: ')) {
          const data = JSON.parse(line.slice(6));
          
          if (data.done) {
            return;
          }
          
          yield data.content;
        }
      }
    }
  }
}

export const api = new ApiClient();
```

### Step 9.2 — Create React Hooks

```typescript
// frontend/src/hooks/useAuth.ts
import { useState, useEffect, useCallback } from 'react';
import { api } from '../lib/api';

export function useAuth() {
  const [user, setUser] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    const token = api.getToken();
    if (token) {
      // Validate token and get user info
      setUser({ token }); // Simplified
    }
    setLoading(false);
  }, []);
  
  const login = useCallback(async (email: string, password: string) => {
    const tokens = await api.login(email, password);
    api.setToken(tokens.access_token);
    setUser({ token: tokens.access_token });
  }, []);
  
  const logout = useCallback(() => {
    api.setToken(null);
    setUser(null);
  }, []);
  
  return { user, loading, login, logout };
}

// frontend/src/hooks/useChat.ts
import { useState, useCallback, useRef } from 'react';
import { api } from '../lib/api';

export function useChat() {
  const [messages, setMessages] = useState<Array<{ role: string; content: string }>>([]);
  const [loading, setLoading] = useState(false);
  const [conversationId, setConversationId] = useState<string | null>(null);
  
  const sendMessage = useCallback(async (content: string) => {
    setLoading(true);
    
    // Add user message
    setMessages(prev => [...prev, { role: 'user', content }]);
    
    try {
      const response = await api.sendMessage(content, conversationId || undefined);
      
      // Add assistant message
      setMessages(prev => [...prev, { role: 'assistant', content: response.message }]);
      setConversationId(response.conversation_id);
    } catch (error) {
      console.error('Chat error:', error);
    } finally {
      setLoading(false);
    }
  }, [conversationId]);
  
  const clearChat = useCallback(() => {
    setMessages([]);
    setConversationId(null);
  }, []);
  
  return { messages, loading, sendMessage, clearChat };
}
```

### Step 9.3 — Create Components

```tsx
// frontend/src/components/Chat.tsx
import { useState } from 'react';
import { useChat } from '../hooks/useChat';

export function Chat() {
  const { messages, loading, sendMessage } = useChat();
  const [input, setInput] = useState('');
  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!input.trim() || loading) return;
    
    const message = input;
    setInput('');
    await sendMessage(message);
  };
  
  return (
    <div className="chat-container">
      <div className="messages">
        {messages.map((msg, i) => (
          <div key={i} className={`message ${msg.role}`}>
            {msg.content}
          </div>
        ))}
        {loading && <div className="message assistant">Thinking...</div>}
      </div>
      
      <form onSubmit={handleSubmit} className="input-area">
        <input
          value={input}
          onChange={e => setInput(e.target.value)}
          placeholder="Ask about your documents..."
          disabled={loading}
        />
        <button type="submit" disabled={loading || !input.trim()}>
          Send
        </button>
      </form>
    </div>
  );
}
```

## Phase Summary

**What was built:**
- TypeScript API client
- React hooks for auth and chat
- Chat component example

**What was learned:**
- API integration
- Authentication handling
- Streaming response handling
