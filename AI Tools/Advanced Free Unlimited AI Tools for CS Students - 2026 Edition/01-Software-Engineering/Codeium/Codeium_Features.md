# Codeium - Features

## Core Features

### 1. Intelligent Code Completion

#### Inline Completions

Codeium provides real-time code suggestions as you type:

```python
# Start typing this...
def calculate_fibonacci(n):
    if n <= 1:
        return n
    # Codeium will suggest:
    return calculate_fibonacci(n-1) + calculate_fibonacci(n-2)
```

#### Multi-line Completions

```javascript
// Codeium can suggest entire functions
const fetchUserData = async (userId) => {
  const response = await fetch(`/api/users/${userId}`);
  const data = await response.json();
  return data;
};
```

#### Tabnine-like Speed

- **Latency**: <50ms average
- **Accuracy**: Context-aware suggestions
- **Learning**: Adapts to your code style

---

### 2. Code Generation

#### Generate Functions from Comments

```python
# Type this comment:
# Function to calculate the area of a circle
# Then Codeium suggests:
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
  
  updateEmail(newEmail: string): void {
    this.email = newEmail;
  }
}
```

---

### 3. Refactoring Capabilities

#### Rename Variables

- Highlight variable → Right-click → Codeium: Rename

#### Extract Method

```python
# Before
total = 0
for item in cart:
    total += item.price * item.quantity
    tax = total * 0.1
    final = total + tax

# Codeium can extract to:
def calculate_tax(total):
    return total * 0.1

total = sum(item.price * item.quantity for item in cart)
tax = calculate_tax(total)
final = total + tax
```

---

### 4. Unit Test Generation

#### Generate Tests

```python
# Select this function:
def add(a, b):
    return a + b

# Right-click → Codeium: Generate Tests
# Codeium generates:
import pytest

def test_add_integers():
    assert add(1, 2) == 3

def test_add_negatives():
    assert add(-1, 1) == 0

def test_add_floats():
    assert abs(add(0.1, 0.2) - 0.3) < 0.0001
```

---

### 5. Code Explanation

#### Explain Selection

1. Select code
2. Right-click → Codeium: Explain
3. View explanation in sidebar

Example output:
```markdown
This function implements binary search:
1. It takes a sorted array and target value
2. Repeatedly divides search interval in half
3. Returns index if found, -1 otherwise
4. Time complexity: O(log n)
```

---

### 6. Natural Language Search

#### Search with Description

- `Ctrl+Shift+P` → "Codeium: Search Code"
- Type: "function that calculates factorial"
- Codeium finds matching code in your project

---

## Advanced Features

### 7. Autocomplete Modes

| Mode | Description | Activation |
|------|-------------|------------|
| Inline | Gray text suggestions | Default |
| Suggestion | Completion popup | Tab to accept |
| Quick Fix | Error suggestions | On error |

### 8. Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Tab | Accept suggestion |
| Ctrl+→ | Accept word |
| Esc | Dismiss |
| Alt+\\ | Force completion |
| Ctrl+Shift+Enter | Accept line |

### 9. Context Awareness

Codeium understands:
- **Project structure**: Import paths, file organization
- **Framework**: React, Django, Express patterns
- **Style**: Your coding conventions
- **History**: Your previous code

### 10. Enterprise Features

| Feature | Free | Enterprise |
|---------|------|------------|
| Completions | Unlimited | Unlimited |
| Languages | 70+ | 70+ |
| Team Dashboard | ❌ | ✅ |
| API Access | ❌ | ✅ |
| SSO | ❌ | ✅ |

---

## Configuration Options

### VS Code Settings

```json
{
  // Enable/disable
  "codeium.enable": true,
  
  // Completion delay (ms)
  "codeium.completionDelay": 0,
  
  // Enable for all languages
  "codeium.enableForAllLanguages": true,
  
  // Offline mode
  "codeium.useOffline": false,
  
  // Language-specific settings
  "[python]": {
    "codeium.indentStyle": "space"
  }
}
```

---

## Feature Comparison

| Feature | Codeium | Copilot | CodeWhisperer |
|---------|---------|---------|---------------|
| Free Tier | Unlimited | Limited | Limited |
| Languages | 70+ | 10+ | 15+ |
| Test Generation | ✅ | ❌ | ✅ |
| Code Explanation | ✅ | ❌ | ❌ |
| Refactoring | ✅ | Basic | ❌ |

---

*Back to [Codeium README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*