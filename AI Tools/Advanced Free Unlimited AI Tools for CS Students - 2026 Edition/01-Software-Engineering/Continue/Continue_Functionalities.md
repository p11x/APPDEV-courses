# Continue - Functionalities

## Command Reference

### VS Code Commands

| Command | Shortcut | Description |
|---------|----------|-------------|
| Continue: Open Chat | Ctrl+L | Open chat panel |
| Continue: Chat from Selection | Ctrl+Shift+L | Chat with selected code |
| Continue: Submit Message | Ctrl+Enter | Send message |

---

## Code Examples

### Python

```python
# Ask Continue to explain code
def fibonacci(n):
    if n <= 1:
        return n
    return fibonacci(n-1) + fibonacci(n-2)

# Ask: "What is the time complexity?"
```

### JavaScript

```javascript
// Generate unit tests
const fetchUserData = async (userId) => {
  const response = await fetch(`/api/users/${userId}`);
  return response.json();
};

// Ask: "/test Generate pytest tests"
```

### React

```jsx
// Create component
import React, { useState } from 'react';

function Counter() {
  const [count, setCount] = useState(0);
  return (
    <button onClick={() => setCount(count + 1)}>
      Count: {count}
    </button>
  );
}
```

---

## Configuration Examples

### Ollama Configuration

```json
{
  "models": [
    {
      "provider": "ollama",
      "model": "llama3",
      "api_base": "http://localhost:11434"
    }
  ]
}
```

### Claude Configuration

```json
{
  "models": [
    {
      "provider": "anthropic",
      "model": "claude-3-opus",
      "api_key": "${ANTHROPIC_API_KEY}"
    }
  ]
}
```

---

## Slash Commands

### Built-in Commands

| Command | Usage |
|---------|-------|
| `/edit` | Edit selected code |
| `/test` | Generate tests |
| `/explain` | Explain code |
| `/refactor` | Refactor code |
| `/document` | Add docs |

### Custom Commands

```json
{
  "customCommands": [
    {
      "name": "review",
      "prompt": "{{{ input }}}\n\nWrite a code review."
    },
    {
      "name": "simplify",
      "prompt": "{{{ input }}}\n\nSimplify this code."
    }
  ]
}
```

---

## API Integration

### REST API

```bash
# Continue exposes a local API
curl http://localhost:31200/chat  \
  -H "Content-Type: application/json" \
  -d '{"message": "Hello"}'
```

---

## Error Handling

### Common Issues

| Issue | Solution |
|-------|----------|
| Model not found | Run `ollama pull <model>` |
| Connection refused | Start Ollama: `ollama serve` |
| API key invalid | Check key in config.json |

---

*Back to [Continue README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*