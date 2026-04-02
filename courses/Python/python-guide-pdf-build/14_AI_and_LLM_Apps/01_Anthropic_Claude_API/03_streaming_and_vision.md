# 🌊 Streaming Responses and Vision API

## 🎯 What You'll Learn

- Using streaming for real-time response display
- The streaming API in Python
- Sending images to Claude for understanding
- Base64 encoding images for the API
- Combining streaming with vision

## 📦 Prerequisites

- Completed [01_claude_api_setup.md](./01_claude_api_setup.md)
- Basic understanding of async Python (optional)

---

## Why Streaming Matters

### Without Streaming

```python
# Traditional request - waits for COMPLETE response
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=1000,
    messages=[{"role": "user", "content": "Write a long story..."}]
)

# User waits 5+ seconds, then sees entire response at once
print(response.content[0].text)
```

### With Streaming

```python
# Streaming - response appears word by word as it's generated
# User sees first word after ~100ms, not 5 seconds!
with client.messages.stream(
    model="claude-sonnet-4-5",
    max_tokens=1000,
    messages=[{"role": "user", "content": "Write a story..."}]
) as stream:
    for text in stream.text_stream:
        print(text, end="", flush=True)  # Print as it arrives!
```

### The Difference

| Approach | First Response | User Experience |
|----------|----------------|-----------------|
| Non-streaming | 3-10 seconds | Dead air, then all at once |
| Streaming | ~100ms | Words appear in real-time |

---

## Basic Streaming

### Using the Streaming API

```python
from anthropic import Anthropic

client = Anthropic()

# Create a streaming response
with client.messages.stream(
    model="claude-sonnet-4-5",
    max_tokens=500,
    messages=[{"role": "user", "content": "Explain Python generators in simple terms."}]
) as stream:
    # stream.text_stream yields text chunks as they're generated
    for text_chunk in stream.text_stream:
        print(text_chunk, end="", flush=True)  # Print each chunk immediately
    print()  # Final newline

# Get the full message for usage data
message = stream.get_final_message()
print(f"\n📊 Total tokens: {message.usage.output_tokens}")
```

### 💡 Line-by-Line Breakdown

```python
from anthropic import Anthropic

client = Anthropic()

# Use .stream() instead of .create()
with client.messages.stream(
    model="claude-sonnet-4-5",    # Model
    max_tokens=500,              # Max tokens to generate
    messages=[{"role": "user", "content": "Explain Python generators..."}]
) as stream:                     # Context manager for streaming
    
    # Iterate over text chunks as they arrive
    for text_chunk in stream.text_stream:
        print(text_chunk, end="", flush=True)  # Flush=True shows immediately
    
    print()  # Newline after streaming completes

# After exiting the with block, get the final message object
message = stream.get_final_message()  # Full response with usage data
print(f"\n📊 Tokens used: {message.usage}")
```

---

## Using Async Streaming

### For Async Applications

```python
import asyncio
from anthropic import AsyncAnthropic

async def main():
    client = AsyncAnthropic()
    
    # Async streaming
    async with client.messages.stream(
        model="claude-sonnet-4-5",
        max_tokens=500,
        messages=[{"role": "user", "content": "What is async/await in Python?"}]
    ) as stream:
        async for text in stream.text_stream:
            print(text, end="", flush=True)
    
    message = await stream.get_final_message()
    print(f"\n\n📊 Used {message.usage.output_tokens} tokens")

asyncio.run(main())
```

---

## Using Callbacks for Streaming

### Alternative: on_text Callback

```python
from anthropic import Anthropic

client = Anthropic()

full_response = ""

def on_text(text: str):
    """Called for each text chunk."""
    global full_response
    print(text, end="", flush=True)
    full_response += text

with client.messages.stream(
    model="claude-sonnet-4-5",
    max_tokens=300,
    messages=[{"role": "user", "content": "List 5 Python tips."}],
    extra_headers={"anthropic-beta": "messages-2024-01-01"}  # May be needed for callbacks
) as stream:
    # Process stream events
    for event in stream:
        if event.type == "content_block_delta":
            if event.delta.type == "text_delta":
                on_text(event.delta.text)

print("\n\nDone!")
```

---

## Vision API: Understanding Images

### Sending Images to Claude

Claude can see images! You can send:

- JPEG
- PNG
- GIF
- WebP

### Method 1: From a File

```python
from anthropic import Anthropic
from pathlib import Path

client = Anthropic()

# Read image from file
image_path = Path("screenshot.png")
image_data = image_path.read_bytes()

# Encode as base64
import base64
image_b64 = base64.b64encode(image_data).decode("utf-8")

# Send to Claude
response = client.messages.create(
    model="claude-sonnet-4-5-20250514",  # Use vision-enabled model
    max_tokens=500,
    messages=[
        {
            "role": "user",
            "content": [
                {
                    "type": "image",
                    "source": {
                        "type": "base64",
                        "media_type": "image/png",
                        "data": image_b64
                    }
                },
                {
                    "type": "text",
                    "text": "What's in this image?"
                }
            ]
        }
    ]
)

print(response.content[0].text)
```

### 💡 Line-by-Line Breakdown

```python
from anthropic import Anthropic
from pathlib import Path
import base64

client = Anthropic()

image_path = Path("screenshot.png")  # Path to your image
image_data = image_path.read_bytes()  # Read as bytes

# Convert to base64 - the API expects base64-encoded images
image_b64 = base64.b64encode(image_data).decode("utf-8")

# Create message with image
response = client.messages.create(
    model="claude-sonnet-4-5-20250514",  # Vision model - note the date!
    max_tokens=500,
    messages=[{
        "role": "user",
        "content": [
            # First block: the image
            {
                "type": "image",
                "source": {
                    "type": "base64",
                    "media_type": "image/png",  # Adjust for your image type!
                    "data": image_b64
                }
            },
            # Second block: the question
            {
                "type": "text",
                "text": "What's in this image?"
            }
        ]
    }]
)

print(response.content[0].text)  # Claude describes the image!
```

### Method 2: From a URL

```python
# Alternative: use a URL instead of base64
response = client.messages.create(
    model="claude-sonnet-4-5-20250514",
    max_tokens=500,
    messages=[
        {
            "role": "user",
            "content": [
                {
                    "type": "image",
                    "source": {
                        "type": "url",
                        "url": "https://example.com/photo.jpg"
                    }
                },
                {
                    "type": "text",
                    "text": "Describe this image."
                }
            ]
        }
    ]
)
```

---

## Practical Examples

### Extracting Text from Screenshots

```python
import base64
from pathlib import Path
from anthropic import Anthropic

def extract_text_from_image(image_path: str) -> str:
    """Extract readable text from an image (OCR)."""
    client = Anthropic()
    
    # Read and encode image
    image_data = Path(image_path).read_bytes()
    image_b64 = base64.b64encode(image_data).decode("utf-8")
    
    # Determine media type
    suffix = Path(image_path).suffix.lower()
    media_type = {
        ".png": "image/png",
        ".jpg": "image/jpeg",
        ".jpeg": "image/jpeg",
        ".gif": "image/gif",
        ".webp": "image/webp"
    }.get(suffix, "image/png")
    
    # Extract text
    response = client.messages.create(
        model="claude-sonnet-4-5-20250514",
        max_tokens=2000,
        messages=[{
            "role": "user",
            "content": [
                {
                    "type": "image",
                    "source": {"type": "base64", "media_type": media_type, "data": image_b64}
                },
                {
                    "type": "text",
                    "text": "Extract ALL text visible in this image. Preserve formatting."
                }
            ]
        }]
    )
    
    return response.content[0].text

# Usage
text = extract_text_from_image("screenshot.png")
print(text)
```

---

## Combining Streaming + Vision

### Real-Time Image Analysis

```python
import base64
from pathlib import Path
from anthropic import Anthropic

def stream_image_analysis(image_path: str) -> None:
    """Analyze an image with streaming response."""
    client = Anthropic()
    
    image_data = Path(image_path).read_bytes()
    image_b64 = base64.b64encode(image_data).decode("utf-8")
    
    with client.messages.stream(
        model="claude-sonnet-4-5-20250514",
        max_tokens=500,
        messages=[{
            "role": "user",
            "content": [
                {"type": "image", "source": {"type": "base64", "media_type": "image/png", "data": image_b64}},
                {"type": "text", "text": "What do you see? Describe in detail."}
            ]
        }]
    ) as stream:
        for text in stream.text_stream:
            print(text, end="", flush=True)
        print()

stream_image_analysis("photo.jpg")
```

---

## ✅ Summary

- Streaming shows responses in real-time as Claude generates them
- Use `client.messages.stream()` with a context manager
- Iterate over `stream.text_stream` to get chunks as they arrive
- Claude can see images (JPEG, PNG, GIF, WebP) and describe them
- Encode images as base64 or use URLs to send to Claude

## ➡️ Next Steps

Continue to [../02_Building_Chatbots/01_multi_turn_conversations.md](../02_Building_Chatbots/01_multi_turn_conversations.md) to learn how to build chatbots with memory.

## 🔗 Further Reading

- [Streaming Messages](https://docs.anthropic.com/en/docs/claude-code/messages)
- [Vision](https://docs.anthropic.com/en/docs/claude-code/vision)
- [Base64 Encoding](https://en.wikipedia.org/wiki/Base64)
