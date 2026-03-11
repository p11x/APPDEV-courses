# Kilo-Code - Features

## Core Features

### 1. Code Generation

Generate code from natural language descriptions:

```bash
# Example: Generate code
kilo generate "Create a Python function to reverse a string"
```

---

### 2. Code Explanation

Understand code in plain English:

```bash
# Explain code from file
kilo explain ./src/utils.js

# Explain selected code
kilo explain
```

---

### 3. Code Refactoring

Transform code with AI assistance:

```bash
# Refactor code
kilo refactor ./src/app.py --style functional
```

---

### 4. Bug Fixing

Fix errors automatically:

```bash
# Fix bugs
kilo fix ./src/error.js
```

---

### 5. Code Review

Get feedback on code quality:

```bash
# Review code
kilo review ./src/main.py
```

---

## Additional Features

### 6. Chat Interface

Interactive conversational AI:

```bash
# Start chat
kilo chat
```

### 7. Multiple Provider Support

| Provider | Configuration |
|----------|---------------|
| Ollama | Local models |
| OpenAI | API-based |
| Anthropic | Claude |

### 8. IDE Integration

Works with VS Code and other editors through API.

---

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| kilo generate | Generate code |
| kilo explain | Explain code |
| kilo refactor | Refactor code |
| kilo fix | Fix bugs |
| kilo review | Review code |

---

*Back to [Kilo-Code README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*