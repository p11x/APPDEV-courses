# AI APIs

## What You'll Learn
- OpenAI API
- Using GPT models
- Making API calls

## Prerequisites
- Completed scraping folder

## OpenAI API

```bash
pip install openai
```

```python
import openai

openai.api_key = "your-api-key"

response = openai.chat.completions.create(
    model="gpt-4",
    messages=[
        {"role": "system", "content": "You are a helpful assistant."},
        {"role": "user", "content": "What is Python?"}
    ]
)

print(response.choices[0].message.content)
```

## Summary
- Use OpenAI for LLMs
- Use API keys securely
- Consider costs

## Next Steps
→ Continue to `02-fastapi-with-openai.md`
