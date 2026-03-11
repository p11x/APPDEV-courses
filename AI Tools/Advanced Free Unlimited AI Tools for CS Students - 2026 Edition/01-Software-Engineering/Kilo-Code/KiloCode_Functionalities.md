# Kilo-Code - Functionalities

## Command Reference

### Main Commands

| Command | Description |
|---------|-------------|
| `kilo generate` | Generate code from description |
| `kilo explain` | Explain code |
| `kilo refactor` | Refactor code |
| `kilo fix` | Fix bugs |
| `kilo review` | Code review |
| `kilo chat` | Interactive chat |

---

## Usage Examples

### Generate Code

```bash
# Generate Python function
kilo generate "Create a function to calculate factorial"

# Generate React component
kilo generate "React component for user login form"
```

### Explain Code

```bash
# Explain from file
kilo explain ./src/utils.js

# Explain from stdin
cat code.py | kilo explain
```

### Refactor Code

```bash
# Refactor to functional style
kilo refactor ./src/app.py --style functional

# Refactor to OOP
kilo refactor ./src/app.py --style oop
```

### Fix Bugs

```bash
# Fix errors
kilo fix ./src/error.js

# Fix with explanation
kilo fix ./src/error.js --explain
```

---

## Configuration

### API Key Setup

```bash
# Set OpenAI key
kilo config set api-key YOUR_OPENAI_KEY

# Use Ollama
kilo config set provider ollama
kilo config set model codellama
```

### Provider Options

| Provider | Model | Use Case |
|----------|-------|----------|
| ollama | llama3 | Local, private |
| openai | gpt-4 | General |
| anthropic | claude | Coding |

---

## Error Handling

### Common Issues

| Error | Solution |
|-------|----------|
| API key missing | Run `kilo config set api-key` |
| Model not found | Check provider configuration |
| Connection error | Check internet connection |

---

*Back to [Kilo-Code README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*