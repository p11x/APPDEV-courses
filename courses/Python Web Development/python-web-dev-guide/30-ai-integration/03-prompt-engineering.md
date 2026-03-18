# Prompt Engineering

## What You'll Learn
- Writing effective prompts
- System messages
- Few-shot learning

## Prerequisites
- Completed FastAPI with OpenAI

## System Messages

```python
system_message = """You are a Python expert.
Provide clear, concise code examples.
Always include type hints."""

response = openai.chat.completions.create(
    model="gpt-4",
    messages=[
        {"role": "system", "content": system_message},
        {"role": "user", "content": "How do I sort a list?"}
    ]
)
```

## Few-Shot Learning

```python
response = openai.chat.completions.create(
    model="gpt-4",
    messages=[
        {"role": "user", "content": "2 + 2"},
        {"role": "assistant", "content": "4"},
        {"role": "user", "content": "3 + 3"},
        # Model will respond: "6"
    ]
)
```

## Summary
- System messages set behavior
- Few-shot for examples
- Be specific and clear

## Next Steps
→ Continue to `04-embeddings-and-vector-db.md`
