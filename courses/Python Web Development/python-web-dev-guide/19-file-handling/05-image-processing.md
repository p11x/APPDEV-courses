# Image Processing

## What You'll Learn
- Basic image manipulation
- Creating thumbnails
- Image formats

## Prerequisites
- Completed CSV/Excel handling

## Using Pillow

```bash
pip install Pillow
```

```python
from PIL import Image
from fastapi import UploadFile, File
from io import BytesIO
import httpx

app = FastAPI()

@app.post("/thumbnail")
async def create_thumbnail(file: UploadFile = File(...)):
    """Create thumbnail"""
    # Read image
    content = await file.read()
    image = Image.open(BytesIO(content))
    
    # Resize
    image.thumbnail((200, 200))
    
    # Save to buffer
    buffer = BytesIO()
    image.save(buffer, format="JPEG")
    buffer.seek(0)
    
    return Response(
        content=buffer.getvalue(),
        media_type="image/jpeg"
    )

@app.post("/resize")
async def resize_image(
    file: UploadFile = File(...),
    width: int = 800,
    height: int = 600
):
    """Resize image"""
    content = await file.read()
    image = Image.open(BytesIO(content))
    
    # Resize
    resized = image.resize((width, height))
    
    buffer = BytesIO()
    resized.save(buffer, format="PNG")
    buffer.seek(0)
    
    return Response(
        content=buffer.getvalue(),
        media_type="image/png"
    )
```

## Summary
- Use Pillow for image processing
- Create thumbnails for previews
- Return proper image types

## Next Steps
→ Move to `20-email/`
