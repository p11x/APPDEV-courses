# Internationalization (i18n)

## What You'll Learn
- Adding i18n support
- Using Babel for translations
- Locale-aware formatting
- Language detection

## Prerequisites
- Basic web development

## Installation

```bash
pip install babel
```

## Configuration

```python
from fastapi import FastAPI, Request
from babel import Locale
from babel.dates import format_date, format_datetime
import pytz

app = FastAPI()

def get_locale(request: Request) -> Locale:
    """Get locale from header or default."""
    accept_language = request.headers.get("Accept-Language", "en")
    # Parse first language
    lang = accept_language.split(",")[0].split("-")[0]
    return Locale(lang)

@app.get("/hello")
async def hello(request: Request):
    """Localized greeting."""
    locale = get_locale(request)
    
    return {
        "greeting": locale.get_display_name(),
        "formatted_date": format_datetime(
            datetime.now(),
            locale=locale
        )
    }
```

## Summary

- Use Babel for internationalization
- Detect locale from Accept-Language header
- Use locale-aware formatting for dates/numbers
- Store translations in gettext files
