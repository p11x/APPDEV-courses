# Google Colab Installation Guide

## Getting Started

Google Colab requires no installation - it's a web-based service.

### Step 1: Access Google Colab

1. Open your browser
2. Navigate to [colab.research.google.com](https://colab.research.google.com)
3. Sign in with your Google account

### Step 2: Create New Notebook

1. Click "New Notebook" 
2. A new Jupyter notebook opens
3. Start coding in Python

## Alternative Access Methods

### From Google Drive

1. Go to [drive.google.com](https://drive.google.com)
2. Right-click > More > Google Colaboratory
3. Creates new notebook

### From GitHub

1. Go to File > Open notebook
2. Select GitHub tab
3. Enter repository URL

## Configuration

### Setting GPU Access

1. Runtime > Change runtime type
2. Select "GPU" under Hardware accelerator
3. Click Save

### Installing Packages

```python
# Using pip
!pip install package-name

# Using conda
!conda install -c conda-forge package-name
```

### Common Pre-installed Libraries

- TensorFlow
- PyTorch
- Keras
- Scikit-learn
- Pandas
- NumPy
- Matplotlib

## Local Runtime (Optional)

Connect to local Jupyter runtime:

1. Runtime > Connect to local runtime
2. Enter local Jupyter URL
3. Requires Jupyter installation locally

## Tips for Students

| Tip | Description |
|-----|-------------|
| Save often | Use Ctrl+S or Cmd+S |
| Use Drive | Save to Google Drive |
| Check GPU | Runtime > View resources |
| Clear output | Edit > Clear all outputs |