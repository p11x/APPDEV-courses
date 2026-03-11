# Hugging Face Spaces Installation

## Option 1: Using Existing Spaces (No Installation)

Access Spaces directly through your browser:

1. Visit [huggingface.co/spaces](https://huggingface.co/spaces)
2. Browse or search for demos
3. Use directly in browser

## Option 2: Creating Your Own Space

### Step 1: Create Hugging Face Account

1. Go to [huggingface.co](https://huggingface.co)
2. Click "Sign Up"
3. Enter email and password
4. Verify email

### Step 2: Create New Space

1. Navigate to [huggingface.co/spaces](https://huggingface.co/spaces)
2. Click "Create new Space"
3. Fill in details:
   - Space name
   - SDK (Streamlit, Gradio, or Docker)
   - Visibility (Public)
4. Click "Create Space"

### Step 3: Clone and Push

```bash
# Clone the repository
git clone https://huggingface.co/spaces/your-username/your-space-name

# Navigate to directory
cd your-space-name

# Make changes, then commit
git add .
git commit -m "Initial commit"

# Push to Hugging Face
git push origin main
```

## Option 3: Local Development

### Install Gradio (Recommended for Students)

```bash
pip install gradio
```

Test installation:

```python
import gradio
print("Gradio installed successfully!")
```

### Install Streamlit

```bash
pip install streamlit
```

Test installation:

```bash
streamlit --version
```

## Quick Gradio Example

Create a file called `app.py`:

```python
import gradio as gr

def greet(name):
    return f"Hello, {name}!"

demo = gr.Interface(
    fn=greet,
    inputs="text",
    outputs="text"
)

demo.launch()
```

Run locally:

```bash
python app.py
```

## Quick Streamlit Example

Create a file called `app.py`:

```python
import streamlit as st

st.title("Hello, World!")
name = st.text_input("Enter your name")
st.write(f"Hello, {name}!")
```

Run locally:

```bash
streamlit run app.py
```

## Deploy to Hugging Face

### Using Gradio

Update `requirements.txt`:

```
gradio
transformers
torch
```

Push to your Space repo.

### Using Spaces SDK

Install Hugging Face Hub:

```bash
pip install huggingface_hub
```

Login:

```bash
huggingface-cli login
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Git push fails | Check token permissions |
| Import errors | Reinstall packages |
| GPU not available | Check Space hardware setting |
| Cold starts | Upgrade to paid tier |