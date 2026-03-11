# Tabnine - Functionalities

## How Tabnine Works

### Core Technology

Tabnine uses deep learning models trained on millions of open-source code repositories to predict and suggest code completions in real-time.

### Processing Flow

1. **Code Analysis**: Tabnine analyzes the code context around the cursor
2. **Pattern Matching**: It matches patterns from trained models
3. **Prediction**: Generates relevant code suggestions
4. **Display**: Shows suggestions in real-time

---

## Code Completion Examples

### Python

```python
# Function completion
def process_data(data):
    # Tabnine suggests processing logic
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

## Usage Modes

### Local Mode (Offline)

- All processing happens on your machine
- No internet required after installation
- Privacy-first approach
- Slightly fewer suggestions than cloud

### Cloud Mode (Online)

- More accurate suggestions
- Requires internet connection
- Optional data collection (can be disabled)

---

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Tab | Accept suggestion |
| Alt+\ | Force completion |
| Ctrl+Space | Trigger completion manually |
| Esc | Dismiss suggestion |
| Alt+] | Next suggestion |
| Alt+[ | Previous suggestion |

---

## Settings Configuration

### VS Code Settings

```json
{
  "tabnine.enable": true,
  "tabnine.cloud_enabled": false,
  "tabnine.maxInlineSuggestions": 3,
  "tabnine.disable_line_completion": false,
  "tabnine.disable_autocomplete": false
}
```

### Vim/Neovim Settings

```vim
let g:tabnine_enabled = 1
let g:tabnine_max_lines = 10
let g:tabnine_max_at_a_time = 3
```

---

## Performance Optimization

### For Speed

```json
{
  "tabnine.maxInlineSuggestions": 1,
  "tabnine.disable_line_completion": true
}
```

### For Accuracy

```json
{
  "tabnine.maxInlineSuggestions": 5,
  "tabnine.cloud_enabled": true
}
```

---

## Integration Examples

### VS Code Extension API

```javascript
// Tabnine extension provides these commands
tabnine.start
tabnine.disable
tabnine.enable
tabnine.configure
```

### Language Server Protocol

Tabnine implements LSP protocol for communication with editors.

---

*Back to [08-Coding README](../README.md)*
*Back to [Main README](../../README.md)*