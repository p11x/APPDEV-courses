# FastAPI with OpenAI

## What You'll Learn
- Creating AI endpoints
- Streaming responses
- Error handling

## Prerequisites
- Completed AI APIs

## AI Endpoint

```python
from fastapi import FastAPI
from pydantic import BaseModel
import openai

app = FastAPI()

class ChatRequest(BaseModel):
    message: str

@app.post("/chat")
async def chat(request: ChatRequest):
    response = openai.chat.completions.create(
        model="gpt-4",
        messages=[{"role": "user", "content": request.message}]
    )
    
    return {
        "response": response.choices[0].message.content
    }

@app.post("/chat/stream")
async def chat_stream(request: ChatRequest):
    """Streaming response"""
    response = openai.chat.completions.create(
        model="gpt-4",
        messages=[{"role": "user", "content": request.message}],
        stream=True
    )
    
    async def generate():
        for chunk in response:
            if chunk.choices[0].delta.content:
                yield chunk.choices[0].delta.content
    
    return generate()
```

## Summary
- Create AI-powered endpoints
- Handle streaming
- Manage costs

## Next Steps
→ Continue to `03-prompt-engineering.md`
