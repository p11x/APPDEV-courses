# 🤖 Getting Started with the Claude API

## 🎯 What You'll Learn

- Setting up the Anthropic Python SDK
- Getting your API key safely (never hardcode it!)
- Making your first API call
- Understanding the response structure
- Token counting and cost awareness

## 📦 Prerequisites

- Python 3.8+ installed
- An Anthropic account (sign up at console.anthropic.com)
- Basic understanding of Python packages

---

## Installing the SDK

```bash
# Install the Anthropic Python SDK
pip install anthropic

# For async support (recommended!)
pip install anthropic[async]
```

---

## Getting Your API Key

### Step 1: Sign Up

1. Go to [console.anthropic.com](https://console.anthropic.com)
2. Create an account or sign in
3. Navigate to "API Keys" in the sidebar

### Step 2: Create a Key

1. Click "Create Key"
2. Give it a name (e.g., "My Python Scripts")
3. Copy the key immediately — it won't be shown again!

### ⚠️ Critical: Never Hardcode API Keys!

```python
# ❌ NEVER do this!
client = anthropic.Anthropic(api_key="sk-ant-api03-...")

# ✅ ALWAYS use environment variables!
import os
client = anthropic.Anthropic(api_key=os.environ.get("ANTHROPIC_API_KEY"))
```

---

## Setting Up Environment Variables

### Using python-dotenv

```bash
# Install python-dotenv
pip install python-dotenv
```

```python
# Create a .env file (add this to .gitignore!)
# ANTHROPIC_API_KEY=your_key_here

from dotenv import load_dotenv

# Load .env file
load_dotenv()  # This reads .env and sets environment variables

import anthropic

# Now this works!
client = anthropic.Anthropic()  # Reads from ANTHROPIC_API_KEY env var

print("✅ Client initialized successfully!")
```

### 💡 Line-by-Line Breakdown

```python
from dotenv import load_dotenv  # Import load_dotenv function

load_dotenv()  # Looks for .env file in current directory, loads variables

import anthropic  # Import the Anthropic SDK

# Anthropic() automatically reads ANTHROPIC_API_KEY from environment
client = anthropic.Anthropic()

# Try a simple request
message = client.messages.create(
    model="claude-sonnet-4-5",  # Use Sonnet 4.5 model
    max_tokens=1024,            # Limit response length
    messages=[{"role": "user", "content": "Hello Claude!"}]
)

print(message.content[0].text)  # Print Claude's response
```

---

## Your First API Call

```python
import os
from anthropic import Anthropic

# Initialize the client
client = anthropic.Anthropic(api_key=os.environ.get("ANTHROPIC_API_KEY"))

# Create a message
message = client.messages.create(
    model="claude-sonnet-4-5",  # Model to use
    max_tokens=1024,            # Maximum tokens in response
    messages=[
        {"role": "user", "content": "What is Python in one sentence?"}
    ]
)

# Access the response
print("📝 Response:")
print(message.content[0].text)
print(f"\n📊 Usage: {message.usage}")
```

### Output Example

```
📝 Response:
Python is a high-level, interpreted programming language known for its simplicity, readability, and versatility, widely used in web development, data science, AI, and automation.

📊 Usage: Usage(input_tokens=24, output_tokens=42)
```

### 💡 Line-by-Line Breakdown

```python
import os
from anthropic import Anthropic  # Import the SDK

client = anthropic.Anthropic(api_key=os.environ.get("ANTHROPIC_API_KEY"))  # Initialize client

# Create a message - structure is similar to OpenAI!
message = client.messages.create(  # Main API method
    model="claude-sonnet-4-5",    # Model name - Sonnet 4.5 is a good default
    max_tokens=1024,               # Limit response to 1024 tokens
    messages=[                     # List of message objects
        {"role": "user", "content": "What is Python in one sentence?"}  # Your message
    ]
)

# The response is an object with various attributes
print(message.content[0].text)  # The actual text response
print(message.usage)            # Token usage information
```

---

## Understanding the Response Object

```python
# The message object has several useful attributes
message = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=100,
    messages=[{"role": "user", "content": "Hello!"}]
)

# Access all response attributes
print(f"Model: {message.model}")          # Which model responded
print(f"Stop reason: {message.stop_reason}")  # Why did it stop?
print(f"Content: {message.content}")      # List of content blocks
print(f"Usage: {message.usage}")          # Token usage

# Content blocks can be text, tool use, or other types
for block in message.content:
    if hasattr(block, 'text'):
        print(f"Text: {block.text}")
```

---

## Available Models

| Model | Description | Best For |
|-------|-------------|----------|
| `claude-opus-4-5` | Most capable, slower | Complex reasoning, writing |
| `claude-sonnet-4-5` | Balanced (recommended) | General use |
| `claude-haiku-3-5` | Fast, cheaper | Simple tasks, high volume |

### Selecting the Right Model

```python
# For complex tasks - use Opus
response = client.messages.create(
    model="claude-opus-4-5",
    messages=[{"role": "user", "content": "Write a complex algorithm..."}]
)

# For quick, simple tasks - use Haiku
response = client.messages.create(
    model="claude-haiku-3-5",
    messages=[{"role": "user", "content": "What is 2+2?"}]
)
```

---

## Token Counting and Cost

### Understanding Tokens

Tokens are the basic unit the API uses:

- ~1 token ≈ 4 characters of English text
- ~1 token ≈ ¾ of a word
- Different models have different pricing per 1K tokens

### Checking Usage

```python
# The usage object shows token counts
message = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=1000,
    messages=[{"role": "user", "content": "Explain quantum computing."}]
)

print(f"Input tokens: {message.usage.input_tokens}")
print(f"Output tokens: {message.usage.output_tokens}")
print(f"Total tokens: {message.usage.input_tokens + message.usage.output_tokens}")
```

### 💡 Explanation

- **Input tokens**: What you sent (your messages + system prompt)
- **Output tokens**: What Claude generated (limited by max_tokens)
- **Cost**: (input_tokens × input_rate) + (output_tokens × output_rate)

---

## Rate Limits

### What Are Rate Limits?

Rate limits prevent abuse and ensure fair access:

- **Requests per minute**: How many API calls you can make
- **Tokens per minute**: How much content you can process

### Handling Rate Limits Gracefully

```python
import time
from anthropic import RateLimitError

def send_message_with_retry(client, message, max_retries=3):
    """Send a message with automatic retry on rate limit."""
    for attempt in range(max_retries):
        try:
            response = client.messages.create(**message)
            return response
        except RateLimitError as e:
            if attempt < max_retries - 1:
                wait_time = int(e.response.headers.get("retry-after", 60))
                print(f"Rate limited. Waiting {wait_time}s...")
                time.sleep(wait_time)
            else:
                raise

# Usage
message = {
    "model": "claude-sonnet-4-5",
    "max_tokens": 1024,
    "messages": [{"role": "user", "content": "Hello!"}]
}

response = send_message_with_retry(client, message)
```

---

## ✅ Summary

- Install with `pip install anthropic` — use environment variables for API keys!
- Create messages with `client.messages.create()`
- Response contains text in `message.content[0].text`
- Check token usage with `message.usage`
- Handle rate limits gracefully with retry logic

## ➡️ Next Steps

Continue to [02_system_prompts_and_roles.md](./02_system_prompts_and_roles.md) to learn how to shape Claude's behavior with system prompts.

## 🔗 Further Reading

- [Anthropic Python SDK Docs](https://docs.anthropic.com/en/docs/claude-code/python-sdk)
- [API Reference](https://docs.anthropic.com/en/docs/reference/api)
- [Pricing](https://www.anthropic.com/pricing)
