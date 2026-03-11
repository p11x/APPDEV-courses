# Tabnine - Features

## Core Features

### 1. Intelligent Code Completion

#### Inline Completions

Tabnine provides real-time code suggestions as you type:

```python
# Start typing this...
def calculate_fibonacci(n):
    if n <= 1:
        return n
    # Tabnine will suggest:
    return calculate_fibonacci(n-1) + calculate_fibonacci(n-2)
```

#### Multi-line Completions

```javascript
// Tabnine can suggest entire functions
const fetchUserData = async (userId) => {
  const response = await fetch(`/api/users/${userId}`);
  const data = await response.json();
  return data;
};
```

#### Speed and Accuracy

- **Latency**: <30ms average
- **Accuracy**: Context-aware suggestions
- **Learning**: Adapts to your code style

---

### 2. Code Generation

#### Generate Functions from Comments

```python
# Type this comment:
# Function to calculate the area of a circle
# Then Tabnine suggests:
def calculate_circle_area(radius):
    """Calculate the area of a circle given its radius."""
    import math
    return math.pi * radius ** 2
```

#### Generate Classes

```typescript
// Type:
// Create a User class with id, name, email and methods
class User {
  constructor(
    public id: number,
    public name: string,
    public email: string
  ) {}
  
  getInfo(): string {
    return `${this.name} (${this.email})`;
  }
}
```

---

### 3. Privacy Features

#### Local Mode

- **Offline Processing**: Code never leaves your machine
- **No Cloud**: Works completely offline
- **Privacy First**: Ideal for sensitive projects

#### Cloud Mode (Optional)

- **Faster Suggestions**: Cloud-powered when online
- **User Choice**: Toggle between local and cloud

---

### 4. IDE Integration Features

| IDE | Support Level |
|-----|--------------|
| VS Code | Full |
| IntelliJ | Full |
| PyCharm | Full |
| WebStorm | Full |
| Eclipse | Full |
| Vim/Neovim | Full |
| Sublime Text | Full |
| Atom | Full |

### 5. Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Tab | Accept suggestion |
| Alt+\ | Force completion |
| Ctrl+Space | Trigger completion |
| Esc | Dismiss suggestion |

---

## Advanced Features

### 6. Context Awareness

Tabnine understands:
- **Project structure**: Import paths, file organization
- **Framework**: React, Django, Express patterns
- **Style**: Your coding conventions
- **History**: Your previous code

### 7. Language Support

| Language | Support Level |
|----------|---------------|
| Python | Full |
| JavaScript | Full |
| TypeScript | Full |
| Java | Full |
| C/C++ | Full |
| Go | Full |
| Rust | Full |
| Ruby | Full |
| PHP | Full |
| Swift | Full |
| Kotlin | Full |
| SQL | Full |

### 8. Feature Comparison

| Feature | Free Tier | Pro | Enterprise |
|---------|-----------|-----|------------|
| Completions | Limited | Unlimited | Unlimited |
| Languages | 10+ | 20+ | 20+ |
| Local Mode | ✅ | ✅ | ✅ |
| Team Features | ❌ | ❌ | ✅ |
| Custom Models | ❌ | ❌ | ✅ |

---

*Back to [08-Coding README](../README.md)*
*Back to [Main README](../../README.md)*