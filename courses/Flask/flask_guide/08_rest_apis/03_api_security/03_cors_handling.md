<!-- FILE: 08_rest_apis/03_api_security/03_cors_handling.md -->

## Overview

**CORS (Cross-Origin Resource Sharing)** is a mechanism that allows restricted resources on a web page to be requested from another domain. This file covers enabling CORS in Flask using the Flask-CORS extension.

## Core Concepts

### What is CORS?

Browsers restrict cross-origin HTTP requests for security. CORS allows servers to specify who can access their assets.

### Installation

```bash
pip install flask-cors
```

## Code Walkthrough

### Basic CORS Setup

```python
# app.py — Enable CORS
from flask import Flask, jsonify
from flask_cors import CORS

app = Flask(__name__)
CORS(app)  # Enable CORS for all routes

@app.route("/api/data")
def get_data():
    return jsonify({"message": "This is CORS-enabled!"})
```

### Configure CORS Options

```python
# app.py — Configure CORS
from flask_cors import CORS

app = Flask(__name__)

# Configure CORS for specific origins and methods
cors = CORS(app, resources={
    r"/api/*": {
        "origins": ["https://example.com", "https://trusted.com"],
        "methods": ["GET", "POST", "PUT", "DELETE"],
        "allow_headers": ["Content-Type", "Authorization"]
    }
})

@app.route("/api/data")
def get_data():
    return jsonify({"data": [1, 2, 3]})
```

### Decorator-Level CORS

```python
# app.py — Per-route CORS
from flask import Flask, jsonify
from flask_cors import cross_origin

app = Flask(__name__)

@app.route("/api/public")
@cross_origin()  # Allow all origins
def public():
    return jsonify({"message": "Public endpoint"})

@app.route("/api/private")
@cross_origin(origins="https://trusted.com")
def private():
    return jsonify({"message": "Private endpoint"})
```

## Next Steps

You have completed the REST APIs chapter. Continue to [01_why_test.md](../09_testing/01_testing_basics/01_why_test.md) to learn about testing.