# CodeWhisperer - Functionalities

## How CodeWhisperer Works

### 1. Code Analysis Engine

CodeWhisperer analyzes your code in real-time to understand:
- Current code context
- Programming language
- Coding patterns
- Imported libraries and frameworks
- Function and variable names

### 2. Suggestion Generation

When you type code, CodeWhisperer:
1. **Analyzes context** around your cursor
2. **Predicts next likely code** based on patterns
3. **Generates suggestions** in real-time
4. **Displays inline** as you type

---

## Code Completion Examples

### Python

```python
# Type this comment and press Tab
# Function to calculate factorial
def factorial(n):
    if n == 0 or n == 1:
        return 1
    return n * factorial(n - 1)

# Type: import random and get random choice
import random
names = ["Alice", "Bob", "Charlie"]
selected = random.choice(names)  # CodeWhisperer suggests this

# Type: with open and get file handling
with open('data.txt', 'r') as f:
    content = f.read()  # Auto-completed
```

### JavaScript/TypeScript

```javascript
// Type this comment
// Fetch user data from API
async function fetchUserData(userId) {
  try {
    const response = await fetch(`/api/users/${userId}`);
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error:', error);
    return null;
  }
}

// Type: array map
const numbers = [1, 2, 3, 4, 5];
const doubled = numbers.map(n => n * 2);

// Type: event listener
button.addEventListener('click', (event) => {
  console.log('Clicked!', event.target);
});
```

### AWS Integration

```python
# Type this to get AWS Lambda handler
import boto3

def lambda_handler(event, context):
    s3 = boto3.client('s3')
    # CodeWhisperer suggests s3 operations
    
# Type: DynamoDB operations
import boto3
from boto3.dynamodb.conditions import Key

dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table('Users')

def get_user(user_id):
    response = table.get_item(Key={'user_id': user_id})
    return response.get('Item')
```

---

## Code Generation from Comments

### Example 1: Python

```python
# Write a function to validate email
def validate_email(email):
    import re
    pattern = r'^[\w\.-]+@[\w\.-]+\.\w+$'
    return re.match(pattern, email) is not None
```

### Example 2: JavaScript

```javascript
// Create a debounce function
function debounce(func, delay) {
  let timeoutId;
  return function(...args) {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => func.apply(this, args), delay);
  };
}
```

---

## Unit Test Generation

### Python

```python
# Original function
def add(a, b):
    return a + b

# Generated test (select function + right-click)
import unittest

class TestMathOperations(unittest.TestCase):
    def test_add_positive_numbers(self):
        self.assertEqual(add(1, 2), 3)
    
    def test_add_negative_numbers(self):
        self.assertEqual(add(-1, -2), -3)
    
    def test_add_zero(self):
        self.assertEqual(add(5, 0), 5)

if __name__ == '__main__':
    unittest.main()
```

---

## Security Scanning

### Detected Issues

1. **SQL Injection**
```python
# Unsafe - flagged by CodeWhisperer
query = f"SELECT * FROM users WHERE id = {user_id}"
# Warning: Use parameterized queries instead

# Safe - recommended
query = "SELECT * FROM users WHERE id = %s"
cursor.execute(query, (user_id,))
```

2. **Hardcoded Secrets**
```python
# Flagged
api_key = "sk-1234567890abcdef"

# Recommended
import os
api_key = os.environ.get('API_KEY')
```

---

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Tab | Accept suggestion |
| Alt + C | Show next suggestion |
| Alt + ] | Show next suggestion |
| Alt + [ | Show previous suggestion |
| Esc | Dismiss suggestion |
| Ctrl + Enter | Open full suggestion panel |

---

## Settings Configuration

### Enable/Disable

```json
{
  "aws.codewhisperer.enabled": true,
  "aws.codewhisperer.autoSuggestion": "enabled",
  "aws.codewhisperer.shareCodeWhispererContentWithAWS": false
}
```

### Language-specific Settings

```json
{
  "aws.codewhisperer.supportedLanguages": [
    "python",
    "javascript",
    "java",
    "typescript",
    "csharp",
    "go",
    "rust",
    "ruby"
  ]
}
```

---

*Back to [08-Coding README](../README.md)*
*Back to [Main README../../README.md)*