<!-- FILE: 12_caching_and_performance/05_profiling_and_optimization/03_cdn_and_static_asset_optimization.md -->

## Overview

This file covers optimizing static assets using CDNs and browser caching.

## Code Walkthrough

```python
# static_optimization.py
from flask import Flask, send_from_directory
import os

app = Flask(__name__)

# Serve static files with caching headers
@app.route("/static/<path:filename>")
def serve_static(filename):
    response = send_from_directory("static", filename)
    
    # Cache static files for 1 year
    response.headers["Cache-Control"] = "public, max-age=31536000"
    response.headers["ETag"] = filename
    
    return response

# CDN configuration example
# app.config["CDN_DOMAIN"] = "cdn.example.com"

# Basic structure:
# /static/
#   /css/
#   /js/
#   /images/
#   /fonts/

# Best practices:
# 1. Use versioned filenames: app.abc123.js
# 2. Enable gzip/brotli compression
# 3. Set long cache expiry
# 4. Use CDN for distribution
```

## Next Steps

This concludes Topic 12 on Caching and Performance. The guide continues with Topic 13 on Websockets and Realtime.

<!-- Continue to [Topic 13: Websockets and Realtime](../../13_websockets_and_realtime/01_realtime_concepts/01_polling_vs_websockets_vs_sse.md) -->
