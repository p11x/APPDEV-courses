# Custom Responses

## Overview

FastAPI supports various response types beyond JSON, including HTML, plain text, files, streaming, and redirects. Custom responses give full control over HTTP output.

## Response Types

### Plain Text Response

```python
# Example 1: Plain text response
from fastapi import FastAPI
from fastapi.responses import PlainTextResponse

app = FastAPI()

@app.get("/text/", response_class=PlainTextResponse)
async def get_text():
    """Return plain text instead of JSON"""
    return "Hello, World!"

@app.get("/text/custom/", response_class=PlainTextResponse)
async def get_custom_text():
    """Plain text with custom content type"""
    return PlainTextResponse(
        content="Custom text response",
        media_type="text/plain",
        headers={"X-Custom": "value"}
    )
```

### HTML Response

```python
# Example 2: HTML response
from fastapi import FastAPI
from fastapi.responses import HTMLResponse

app = FastAPI()

@app.get("/html/", response_class=HTMLResponse)
async def get_html():
    """Return HTML content"""
    return """
    <!DOCTYPE html>
    <html>
        <head><title>FastAPI Page</title></head>
        <body>
            <h1>Hello from FastAPI!</h1>
            <p>This is an HTML response.</p>
        </body>
    </html>
    """

@app.get("/html/template/", response_class=HTMLResponse)
async def get_template(name: str = "World"):
    """Dynamic HTML response"""
    return f"""
    <html>
        <body>
            <h1>Hello, {name}!</h1>
        </body>
    </html>
    """
```

### File Response

```python
# Example 3: File response
from fastapi import FastAPI
from fastapi.responses import FileResponse
from pathlib import Path

app = FastAPI()

@app.get("/download/{filename}")
async def download_file(filename: str):
    """Download a file"""
    file_path = Path("files") / filename

    if not file_path.exists():
        return {"error": "File not found"}

    return FileResponse(
        path=file_path,
        filename=filename,
        media_type="application/octet-stream"
    )

@app.get("/image/{image_name}")
async def get_image(image_name: str):
    """Serve an image"""
    image_path = Path("images") / image_name

    return FileResponse(
        path=image_path,
        media_type="image/jpeg",
        headers={"Cache-Control": "max-age=3600"}
    )
```

### Streaming Response

```python
# Example 4: Streaming response
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
import asyncio
import io

app = FastAPI()

async def generate_numbers():
    """Generate numbers asynchronously"""
    for i in range(10):
        yield f"Number: {i}\n"
        await asyncio.sleep(0.5)

@app.get("/stream/")
async def stream_data():
    """Stream data to client"""
    return StreamingResponse(
        generate_numbers(),
        media_type="text/plain"
    )

@app.get("/stream/csv/")
async def stream_csv():
    """Stream CSV data"""
    async def generate_csv():
        yield "id,name,price\n"
        for i in range(100):
            yield f"{i},Item {i},{i * 10.99}\n"

    return StreamingResponse(
        generate_csv(),
        media_type="text/csv",
        headers={"Content-Disposition": "attachment; filename=data.csv"}
    )
```

### Redirect Response

```python
# Example 5: Redirect response
from fastapi import FastAPI
from fastapi.responses import RedirectResponse

app = FastAPI()

@app.get("/old-endpoint/")
async def old_endpoint():
    """Permanent redirect"""
    return RedirectResponse(
        url="/new-endpoint/",
        status_code=301  # Permanent
    )

@app.get("/temporary/")
async def temporary():
    """Temporary redirect"""
    return RedirectResponse(
        url="/target/",
        status_code=302  # Temporary
    )

@app.get("/new-endpoint/")
async def new_endpoint():
    return {"message": "New endpoint"}

@app.get("/target/")
async def target():
    return {"message": "Target endpoint"}
```

### JSON Response

```python
# Example 6: Custom JSON response
from fastapi import FastAPI
from fastapi.responses import JSONResponse
import json

app = FastAPI()

@app.get("/custom-json/")
async def custom_json():
    """Custom JSON response with headers"""
    return JSONResponse(
        content={"message": "Custom response"},
        status_code=200,
        headers={
            "X-Custom-Header": "value",
            "Cache-Control": "no-cache"
        }
    )

@app.get("/orjson/")
async def orjson_response():
    """Faster JSON with orjson"""
    from fastapi.responses import ORJSONResponse
    return ORJSONResponse(content={"items": list(range(1000))})
```

## Response with Cookies

### Cookie Management

```python
# Example 7: Response cookies
from fastapi import FastAPI, Response

app = FastAPI()

@app.post("/login/")
async def login(username: str, response: Response):
    """Set cookies in response"""
    response.set_cookie(
        key="session_id",
        value="abc123",
        httponly=True,
        secure=True,
        samesite="lax",
        max_age=3600
    )

    return {"message": "Logged in"}

@app.post("/logout/")
async def logout(response: Response):
    """Clear cookies"""
    response.delete_cookie(key="session_id")
    return {"message": "Logged out"}
```

## Best Practices

### Response Guidelines

```python
# Example 8: Best practices
from fastapi import FastAPI
from fastapi.responses import JSONResponse, FileResponse, StreamingResponse

app = FastAPI()

@app.get("/api/data/")
async def api_data():
    """
    Best practices:
    1. Use appropriate response type
    2. Set correct content-type
    3. Include relevant headers
    4. Handle large files with streaming
    """
    return JSONResponse(
        content={"data": "value"},
        headers={"Cache-Control": "max-age=3600"}
    )

@app.get("/download/{filename}")
async def download(filename: str):
    """Use FileResponse for downloads"""
    return FileResponse(
        path=f"files/{filename}",
        filename=filename
    )

@app.get("/stream/")
async def stream():
    """Use StreamingResponse for large data"""
    return StreamingResponse(
        generate_data(),
        media_type="text/plain"
    )
```

## Summary

| Response Type | Use Case | Class |
|---------------|----------|-------|
| JSON | API data | `JSONResponse` |
| Plain text | Simple text | `PlainTextResponse` |
| HTML | Web pages | `HTMLResponse` |
| File | Downloads | `FileResponse` |
| Streaming | Large data | `StreamingResponse` |
| Redirect | URL redirect | `RedirectResponse` |

## Next Steps

Continue learning about:
- [Dependencies](../05_dependencies/01_dependency_injection_basics.md) - DI patterns
- [Middleware](../06_middleware/01_middleware_overview.md) - Request processing
- [CORS Middleware](../06_middleware/02_cors_middleware.md) - Cross-origin requests
