# Continue - Features

## Core Features

### 1. Code Chat

Continue provides an interactive chat interface for coding assistance:

```python
# Open chat panel (Ctrl+L or Cmd+L)
# Ask questions like:
# "Explain what this function does"
def quicksort(arr):
    if len(arr) <= 1:
        return arr
    pivot = arr[len(arr) // 2]
    left = [x for x in arr if x < pivot]
    middle = [x for x in arr if x == pivot]
    right = [x for x in arr if x > pivot]
    return quicksort(left) + middle + quicksort(right)
```

---

### 2. Slash Commands

Quick actions using slash commands:

| Command | Description |
|---------|-------------|
| `/edit` | Edit selected code |
| `/test` | Generate unit tests |
| `/explain` | Explain selected code |
| `/refactor` | Refactor selected code |
| `/document` | Add documentation |
| `/commit` | Generate commit message |

---

### 3. Context Awareness

Continue understands your codebase:

- **File Contents**: Current file context
- **Project Structure**: Imports and dependencies
- **Selection**: Highlighted code
- **History**: Previous messages

---

### 4. Multi-Provider Support

| Provider | Models | Features |
|----------|--------|----------|
| Ollama | llama3, codellama | Local, private |
| Anthropic | Claude 3 | Best for coding |
| OpenAI | GPT-4 | General purpose |
| Google | Gemini | Fast responses |

---

### 5. Custom Commands

Create your own slash commands:

```json
// In config.json
{
  "customCommands": [
    {
      "name": "review",
      "prompt": "{{{ input }}}\n\nWrite a code review for the above code."
    }
  ]
}
```

---

### 6. Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+L | Open chat |
| Ctrl+Shift+L | Chat from selection |
| Ctrl+Enter | Submit message |
| Escape | Close chat |

---

## Advanced Features

### 7. Remote Development

Continue works over SSH:

```json
{
  "remoteConfigs": [
    {
      "name": "server",
      "url": "ssh://user@server"
    }
  ]
}
```

---

### 8. Telemetry Options

Control data collection:

| Option | Description |
|--------|-------------|
| `allowAnonymousTelemetry` | Anonymous usage data |
| `disableIndexing` | Skip code indexing |

---

## VS Code Integration

### Status Bar

- Shows current model
- Provider indicator
- Quick actions

### Side Panel

- Chat history
- Code references
- Suggestions

---

*Back to [Continue README](./README.md)*
*Back to [Software Engineering README](../README.md)*
*Back to [Main README](../../README.md)*