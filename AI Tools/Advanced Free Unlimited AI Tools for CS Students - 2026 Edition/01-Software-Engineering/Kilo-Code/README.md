# Kilo Code

## Description

Kilo Code is a command-line AI coding assistant that integrates with your terminal. It provides code generation, refactoring, and explanation capabilities directly from the command line.

## Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| RAM | 4 GB | 8 GB |
| Node.js | 18+ | Latest |
| npm/yarn | Latest | Latest |
| Internet | Required | Stable connection |

## Installation

```bash
# Using npm
npm install -g @kilo-code/cli

# Or using yarn
yarn global add @kilo-code/cli
```

## Configuration

```bash
# Set API key
kilo config set api-key YOUR_API_KEY

# Or use local models with Ollama
kilo config set provider ollama
kilo config set model codellama

# Verify configuration
kilo config list
```

## Features

- **CLI Interface**: Works in terminal
- **Code Generation**: Create code from descriptions
- **Refactoring**: Transform existing code
- **Explanation**: Understand code snippets
- **Debugging**: Fix errors in code
- **Flexible Providers**: OpenAI, Anthropic, Ollama

## Functionalities

| Feature | Description |
|---------|-------------|
| generate | Create new code from prompts |
| explain | Explain code in plain English |
| refactor | Improve code structure |
| fix | Debug and fix errors |
| review | Get code feedback |
| chat | Interactive conversation |

## Use Cases

### For Students

- **Quick Code**: Generate snippets quickly
- **Terminal Work**: Work without leaving terminal
- **Scripting**: Create automation scripts
- **Learning**: Understand unfamiliar code
- **Debugging**: Fix errors efficiently

### Practical Examples

```bash
# Generate code
kilo generate "Create a React component for user login"

# Explain code
kilo explain ./src/utils.js

# Refactor code
kilo refactor ./src/app.py --style functional

# Fix bugs
kilo fix ./src/error.js

# Review code
kilo review ./src/main.py
```

```bash
# Example output for generate command
# Input: kilo generate "Python function to calculate factorial"
# Output:
def factorial(n):
    """Calculate the factorial of a non-negative integer."""
    if n < 0:
        raise ValueError("Factorial is not defined for negative numbers")
    if n == 0 or n == 1:
        return 1
    return n * factorial(n - 1)

# Example usage
print(factorial(5))  # Output: 120
```

---

*Back to [Category README](../README.md)*
