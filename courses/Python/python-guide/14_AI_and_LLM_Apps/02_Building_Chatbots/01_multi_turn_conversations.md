# 💬 Building Multi-Turn Conversations

## 🎯 What You'll Learn

- How conversation memory works (the messages list)
- Building a chatbot that remembers context
- Context window limits and trimming strategies
- Saving and loading conversation history
- Building a rich terminal UI for chatbots

## 📦 Prerequisites

- Completed the Claude API setup
- Understanding of Python lists and dictionaries

---

## How Conversation Memory Works

In the Claude API, **the messages list IS the memory**:

```python
# The conversation is just a list of messages!
messages = [
    {"role": "user", "content": "Hi, my name is Alice!"},
    {"role": "assistant", "content": "Hello Alice! Nice to meet you!"},
    {"role": "user", "content": "What's my name?"},  # Claude knows from context!
]
```

### Every Request Sends Full History

```python
# Claude doesn't remember between requests - you must send history each time!
response = client.messages.create(
    model="claude-sonnet-4-5",
    messages=[
        {"role": "user", "content": "Hi, my name is Alice!"},         # Message 1
        {"role": "assistant", "content": "Hello Alice!"},              # Message 2
        {"role": "user", "content": "What's my name?"},                # Message 3
    ]
)

# In Message 3, Claude sees ALL previous messages and knows your name!
```

---

## Building a Simple Chatbot

### The Basic Pattern

```python
import os
from anthropic import Anthropic

client = Anthropic(api_key=os.environ.get("ANTHROPIC_API_KEY"))

# Start with empty conversation
messages: list[dict] = []

print("🤖 Claude Chatbot (type 'quit' to exit)")
print("-" * 40)

while True:
    # Get user input
    user_input = input("\nYou: ").strip()
    
    if user_input.lower() in ("quit", "exit", "q"):
        print("Goodbye!")
        break
    
    # Add user message to history
    messages.append({"role": "user", "content": user_input})
    
    # Send entire conversation history
    response = client.messages.create(
        model="claude-sonnet-4-5",
        max_tokens=500,
        messages=messages  # Send ALL messages!
    )
    
    # Get Claude's response
    claude_reply = response.content[0].text
    
    # Add assistant response to history
    messages.append({"role": "assistant", "content": claude_reply})
    
    print(f"\nClaude: {claude_reply}")
```

### 💡 Line-by-Line Breakdown

```python
messages: list[dict] = []  # Start with empty conversation history

while True:
    user_input = input("\nYou: ").strip()  # Get user input
    
    if user_input.lower() in ("quit", "exit", "q"):  # Check for exit
        break
    
    # Add user's message to the conversation
    messages.append({"role": "user", "content": user_input})
    
    # Send the ENTIRE conversation history to Claude
    # This is how Claude knows what was said before!
    response = client.messages.create(
        model="claude-sonnet-4-5",
        max_tokens=500,
        messages=messages  # ⚠️ Must include ALL previous messages!
    )
    
    claude_reply = response.content[0].text  # Get response
    
    messages.append({"role": "assistant", "content": claude_reply})  # Save for next turn
    
    print(f"\nClaude: {claude_reply}")
```

---

## Full Working Chatbot with Rich UI

```python
import os
from anthropic import Anthropic
from dataclasses import dataclass, field
from pathlib import Path
import json

@dataclass
class ChatSession:
    """A chatbot session with history and persistence."""
    
    messages: list[dict] = field(default_factory=list)
    client: Anthropic = field(default_factory=lambda: Anthropic(api_key=os.environ.get("ANTHROPIC_API_KEY")))
    
    def __post_init__(self):
        # Optional system prompt for personality
        self.system = "You are a helpful, friendly assistant."
    
    def chat(self, user_input: str) -> str:
        """Send a message and get a response."""
        # Build API params
        params = {
            "model": "claude-sonnet-4-5",
            "max_tokens": 500,
            "messages": self.messages + [{"role": "user", "content": user_input}]
        }
        
        # Add system prompt for first message only
        if not self.messages:
            params["system"] = self.system
        
        response = self.client.messages.create(**params)
        reply = response.content[0].text
        
        # Update history
        self.messages.append({"role": "user", "content": user_input})
        self.messages.append({"role": "assistant", "content": reply})
        
        return reply
    
    def save(self, filepath: Path) -> None:
        """Save conversation to file."""
        filepath.write_text(json.dumps(self.messages, indent=2))
    
    def load(self, filepath: Path) -> None:
        """Load conversation from file."""
        self.messages = json.loads(filepath.read_text())

# Interactive chat function
def chat_loop():
    """Run an interactive chat session."""
    session = ChatSession()
    
    print("🤖 Claude Chatbot")
    print("Commands: quit, save <file>, load <file>")
    print("-" * 50)
    
    while True:
        try:
            user_input = input("\nYou: ").strip()
            
            if not user_input:
                continue
            
            # Handle special commands
            if user_input.lower() == "quit":
                print("Goodbye! 👋")
                break
            
            if user_input.lower().startswith("save "):
                filename = user_input[5:].strip()
                session.save(Path(filename))
                print(f"💾 Saved to {filename}")
                continue
            
            if user_input.lower().startswith("load "):
                filename = user_input[5:].strip()
                session.load(Path(filename))
                print(f"📂 Loaded from {filename}")
                continue
            
            # Regular chat
            response = session.chat(user_input)
            print(f"\nClaude: {response}")
            
        except KeyboardInterrupt:
            print("\n\nGoodbye! 👋")
            break
        except Exception as e:
            print(f"Error: {e}")

# Run it!
# chat_loop()
```

### 💡 Line-by-Line Breakdown

```python
@dataclass  # Dataclass for clean state management
class ChatSession:
    messages: list[dict] = field(default_factory=list)  # Conversation history
    client: Anthropic = field(default_factory=lambda: Anthropic(...))  # API client
    
    def chat(self, user_input: str) -> str:
        # Build message list with new user input
        api_messages = self.messages + [{"role": "user", "content": user_input}]
        
        # Send to API
        response = self.client.messages.create(
            model="claude-sonnet-4-5",
            max_tokens=500,
            messages=api_messages  # Full history!
        )
        
        reply = response.content[0].text  # Extract reply
        
        # Update history for next turn
        self.messages.append({"role": "user", "content": user_input})
        self.messages.append({"role": "assistant", "content": reply})
        
        return reply
    
    def save(self, filepath: Path):
        # Save messages to JSON
        filepath.write_text(json.dumps(self.messages))
    
    def load(self, filepath: Path):
        # Load messages from JSON
        self.messages = json.loads(filepath.read_text())
```

---

## Context Window Limits

### Understanding Limits

Claude has a maximum context window (typically 200K tokens):

- **Input tokens** = all messages sent + system prompt
- **Output tokens** = Claude's response (limited by max_tokens)
- **Total** = input + output must be < context limit

### Sliding Window Strategy

```python
from anthropic import Anthropic
import tiktoken  # Token counting library

client = Anthropic()

MAX_TOKENS = 150000  # Leave room for response

def count_tokens(messages: list[dict]) -> int:
    """Approximate token count."""
    # Rough estimate: 4 chars per token
    total = sum(len(m["content"]) // 4 for m in messages)
    return total

def trim_messages(messages: list[dict], max_tokens: int = MAX_TOKENS) -> list[dict]:
    """Trim messages to fit in context window."""
    while count_tokens(messages) > max_tokens and len(messages) > 2:
        # Remove oldest message pair (user + assistant)
        messages = messages[2:]
    
    return messages

# In your chat function:
messages = trim_messages(messages)  # Before sending!
response = client.messages.create(model="claude-sonnet-4-5", messages=messages)
```

---

## Using Rich for Color Output

```python
from rich.console import Console
from rich.theme import Theme

# Create themed console
console = Console(theme=Theme({
    "info": "dim cyan",
    "warning": "yellow",
    "danger": "bold red",
}))

# Print with colors
console.print("[bold blue]Claude:[/bold blue] Hello!")

# Rich also supports panels, tables, progress bars, etc.
from rich.panel import Panel
console.print(Panel("[green]Welcome to the chatbot![/green]", title="🤖 Bot"))

# Use in chat loop
def rich_chat():
    console = Console()
    
    while True:
        user_input = console.input("\n[bold cyan]You:[/bold cyan] ")
        
        if user_input.lower() in ("quit", "exit"):
            break
        
        response = session.chat(user_input)
        
        console.print(f"\n[bold blue]Claude:[/bold blue] {response}")
```

---

## ✅ Summary

- The messages list is the conversation memory — send full history each request
- Claude doesn't remember between requests — you must include all previous messages
- Save conversation history to files for persistence
- Implement sliding window to handle long conversations
- Use Rich library for beautiful terminal output

## ➡️ Next Steps

Continue to [02_personas_and_memory.md](./02_personas_and_memory.md) to learn about persistent personas and long-term memory strategies.

## 🔗 Further Reading

- [Anthropic Messages API](https://docs.anthropic.com/en/docs/claude-code/messages)
- [Rich Library](https://rich.readthedocs.io/)
- [tiktoken - Token counting](https://github.com/openai/tiktoken)
