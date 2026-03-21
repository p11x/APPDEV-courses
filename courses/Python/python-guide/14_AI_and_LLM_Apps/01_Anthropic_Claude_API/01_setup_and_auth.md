# Setup and Authentication

## What You'll Learn

- Installing the Anthropic SDK
- Setting up API keys
- Environment variables
- Making your first request

## Prerequisites

- Read [08_visualization_best_practices.md](../../09_Data_Science_Foundations/03_Visualization/08_visualization_best_practices.md) first

## Installation

```bash
# Install Anthropic SDK
pip install anthropic
```

## Authentication

```python
# auth_setup.py

import os
from anthropic import Anthropic

# Option 1: Set environment variable
# ANTHROPIC_API_KEY=your_api_key

# Option 2: Pass directly
client = Anthropic(
    api_key=os.environ.get("ANTHROPIC_API_KEY")
)

# Option 3: Using .env file
# pip install python-dotenv
from dotenv import load_dotenv
load_dotenv()

client = Anthropic()
```

## First Request

```python
# first_request.py

import os
from anthropic import Anthropic

client = Anthropic(api_key=os.environ.get("ANTHROPIC_API_KEY"))

message = client.messages.create(
    model="claude-3-5-sonnet-20241022",
    max_tokens=100,
    messages=[
        {"role": "user", "content": "Hello, Claude!"}
    ]
)

print(message.content[0].text)
```

## Annotated Full Example

```python
# anthropic_demo.py
"""Complete demonstration of Anthropic API setup."""

import os
from anthropic import Anthropic


def main() -> None:
    # Initialize client
    api_key = os.environ.get("ANTHROPIC_API_KEY", "")
    
    if not api_key:
        print("Please set ANTHROPIC_API_KEY environment variable")
        return
    
    client = Anthropic(api_key=api_key)
    
    # Simple message
    message = client.messages.create(
        model="claude-3-5-sonnet-20241022",
        max_tokens=100,
        messages=[
            {"role": "user", "content": "Say hello in 3 words!"}
        ]
    )
    
    print(f"Response: {message.content[0].text}")
    print(f"Model: {message.model}")
    print(f"Usage: {message.usage}")


if __name__ == "__main__":
    main()
```

## Summary

- Installing the Anthropic SDK
- Setting up API keys
- Environment variables

## Next Steps

Continue to **[02_messages_api.md](./02_messages_api.md)**
