<!-- FILE: 12_caching_and_performance/05_profiling_and_optimization/02_response_compression.md -->

## Overview

Response compression reduces bandwidth and improves performance. This file covers how to compress Flask responses.

## Code Walkthrough

```python
# compression.py
from flask import Flask
from flask_compress import Compress

app = Flask(__name__)

# Enable compression
app.config["COMPRESS_MIMETYPES"] = ["text/html", "text/css", "application/json"]
app.config["COMPRESS_LEVEL"] = 6  # 1-9, higher = more compression
app.config["COMPRESS_MIN_SIZE"] = 500  # Only compress if > 500 bytes

compress = Compress(app)

@app.route("/api/data")
def get_data():
    import json
    data = {"message": "Hello" * 1000}
    return json.dumps(data)

if __name__ == "__main__":
    app.run()
```

## Installation

```bash
pip install Flask-Compress
```

## Next Steps

Continue to [03_cdn_and_static_asset_optimization.md](03_cdn_and_static_asset_optimization.md)
