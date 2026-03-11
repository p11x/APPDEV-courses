# Gemini AI Studio Installation

## Web-Based Access (No Installation Required)

Since Gemini AI Studio is a web-based platform, no installation is required.

## Step-by-Step Access Guide

### Step 1: Navigate to Google AI Studio

1. Open your browser
2. Visit: [https://aistudio.google.com](https://aistudio.google.com)
3. Sign in with your Google account

### Step 2: Get API Key (Optional)

For API access:

1. In AI Studio, click "Get API Key"
2. Click "Create API Key" in the popup
3. Copy and save your API key securely

### Step 3: Start Creating Prompts

1. Select a model (Gemini Pro or Flash)
2. Enter your prompt in the chat interface
3. Press Enter or click Send

## Python SDK Installation

For programmatic access, install the Google Generative AI SDK:

```bash
pip install google-generativeai
```

Verify installation:

```bash
python -c "import google.generativeai as genai; print('Google Generative AI SDK installed successfully')"
```

## Configuration

### Environment Setup

Set your API key:

```bash
# Windows (Command Prompt)
setx GEMINI_API_KEY "your-api-key-here"

# Windows (PowerShell)
$env:GEMINI_API_KEY="your-api-key-here"

# Linux/Mac
export GEMINI_API_KEY="your-api-key-here"
```

### Python Configuration

```python
import google.generativeai as genai
import os

# Configure API
genai.configure(api_key=os.environ["GEMINI_API_KEY"])

# List available models
for model in genai.list_models():
    print(model.name)
```

## VS Code Integration

### Using in VS Code

1. Install Python extension
2. Create a new Python file
3. Install the SDK as shown above
4. Use the API in your code

## Verification

Test your setup:

```python
import google.generativeai as genai

model = genai.GenerativeModel('gemini-pro')
response = model.generate_content("Hello, world!")
print(response.text)
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| API key not working | Regenerate key in AI Studio |
| Rate limit exceeded | Wait 60 seconds, use Flash model |
| Import error | Reinstall: `pip install --upgrade google-generativeai` |