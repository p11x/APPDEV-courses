# Image Generation

## What You'll Learn
- DALL-E API
- Generating images
- Variations

## Prerequisites
- Completed LLM applications

## DALL-E

```python
response = openai.images.generate(
    model="dall-e-3",
    prompt="A cute cat sitting on a laptop",
    size="1024x1024",
    quality="standard",
    n=1
)

image_url = response.data[0].url
```

## Summary
- Use DALL-E for image generation
- Consider costs per image
- Follow content guidelines

## Next Steps
→ Move to `31-advanced-projects/`
