# Codeium - Functionalities

## Command Reference

### VS Code Commands

| Command | Shortcut | Description |
|---------|----------|-------------|
| Codeium: Accept | Tab | Accept current suggestion |
| Codeium: Dismiss | Escape | Dismiss suggestion |
| Codeium: Next | Alt+] | Next suggestion |
| Codeium: Previous | Alt+[ | Previous suggestion |
| Codeium: Toggle | Ctrl+Alt+Space | Toggle completions |
| Codeium: Explain | Ctrl+Shift+P | Explain selected code |
| Codeium: Generate Tests | Ctrl+Shift+P | Generate unit tests |
| Codeium: Refactor | Ctrl+Shift+P | Refactor code |

---

## Code Snippets by Language

### Python

```python
# Function completion
def process_data(data):
    # Codeium suggests processing logic
    result = []
    for item in data:
        if isinstance(item, dict):
            result.append({k: v for k, v in item.items()})
    return result

# Class generation
class DataProcessor:
    def __init__(self, config):
        self.config = config
        self.data = []
    
    def add(self, item):
        self.data.append(item)
    
    def process(self):
        return [self._transform(item) for item in self.data]
    
    def _transform(self, item):
        return {**item, 'processed': True}
```

### JavaScript/TypeScript

```javascript
// Async function
async function fetchUserData(userId) {
  try {
    const response = await fetch(`/api/users/${userId}`);
    if (!response.ok) {
      throw new Error('User not found');
    }
    return await response.json();
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

// React Component
import React, { useState, useEffect } from 'react';

function UserProfile({ userId }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    fetchUserData(userId).then(data => {
      setUser(data);
      setLoading(false);
    });
  }, [userId]);
  
  if (loading) return <div>Loading...</div>;
  return <div>{user.name}</div>;
}
```

### Java

```java
// Spring Controller
@RestController
@RequestMapping("/api/users")
public class UserController {
    
    @Autowired
    private UserService userService;
    
    @GetMapping("/{id}")
    public ResponseEntity<User> getUser(@PathVariable Long id) {
        return userService.findById(id)
            .map(ResponseEntity::ok)
            .orElse(ResponseEntity.notFound().build());
    }
    
    @PostMapping
    public ResponseEntity<User> createUser(@RequestBody User user) {
        return ResponseEntity.ok(userService.save(user));
    }
}
```

---

## API Usage

### REST API (Enterprise)

```bash
# Make completion request
curl -X POST https://api.codeium.com/completions \
  -H "Authorization: Bearer YOUR_API_KEY" \
  -H "Content-Type: application/json" \
  -d '{
    "filename": "main.py",
    "contents": "def hello():",
    "cursor": 13,
    "language": "python"
  }'
```

### Response Format

```json
{
  "completions": [
    {
      "text": "    print('Hello, World!')",
      "uuid": "abc-123",
      "probability": 0.95
    }
  ]
}
```

---

## Integration Examples

### VS Code Extension API

```typescript
// Using Codeium in extension
import * as codeium from 'codeium';

// Get completions
const completions = await codeium.getCompletions({
  document: editor.document,
  position: editor.selection.active
});

// Accept completion
await codeium.acceptCompletion(completionId);
```

### Language Server Protocol

```python
# Codeium Language Server
# Implemented via LSP protocol
# Communicates via stdio or TCP
```

---

## Advanced Usage

### Custom Snippets

```json
// Add to keybindings.json
{
  "key": "ctrl+shift+]",
  "command": "codeium.nextCompletion"
}
```

### Context Variables

| Variable | Description |
|----------|-------------|
| `${currentLine}` | Current line content |
| `${currentFile}` | Current filename |
| `${language}` | Current language |
| `${workspaceFolder}` | Workspace path |

---

## Error Handling

### Common Errors

```python
# Error: API rate limit
# Solution: Wait and retry

# Error: Network timeout
# Solution: Check internet connection

# Error: Authentication failed
# Solution: Re-authenticate via Codeium website
```

---

## Performance Optimization

### Settings for Speed

```json
{
  "codeium.completionDelay": 100,
  "codeium.maxSuggestions": 3,
  "codeium.enableForFilesLargerThan": 500000
}
```

### Settings for Accuracy

```json
{
  "codeium.completionDelay": 0,
  "codeium.maxSuggestions": 10,
  "codeium.enableForFilesLargerThan": 0
}
```

---

*Back to [Codeium README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*