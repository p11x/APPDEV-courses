# API Marketplace

## What You'll Learn
- Building API products
- Rate limiting
- API key management

## Prerequisites
- Completed real-time chat

## API Key Management

```python
@app.post("/api-keys")
async def create_api_key(user_id: int, name: str):
    api_key = f"sk_{secrets.token_urlsafe(32)}"
    
    # Hash key before storing
    key_hash = hashlib.sha256(api_key.encode()).hexdigest()
    
    db.add(APIKey(
        user_id=user_id,
        name=name,
        key_hash=key_hash,
        rate_limit=1000
    ))
    db.commit()
    
    return {"api_key": api_key}
```

## Summary
- Build API products
- Implement rate limiting
- Track usage

## Next Steps
→ Continue to `05-next-steps.md`
