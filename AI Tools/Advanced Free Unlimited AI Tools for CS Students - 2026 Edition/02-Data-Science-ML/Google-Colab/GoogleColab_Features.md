# Google Colab Features

## Core Features

### 1. Jupyter Notebooks

| Feature | Description |
|---------|-------------|
| Code cells | Write and execute Python code |
| Markdown cells | Add documentation |
| Interactive output | View results inline |
| Auto-complete | Tab completion |

### 2. GPU Access

Free GPU acceleration:

| GPU | Availability |
|-----|--------------|
| T4 | Common |
| K80 | Occasional |
| P100 | Rare |

### 3. Pre-installed Libraries

Common ML libraries included:

- TensorFlow 2.x
- PyTorch
- Keras
- Scikit-learn
- Pandas
- NumPy
- Matplotlib
- OpenCV
- Seaborn

### 4. File Handling

| Method | Description |
|--------|-------------|
| Upload | Upload files directly |
| Drive | Mount Google Drive |
| GitHub | Import from GitHub |
| URLs | Download from web |

## Collaboration Features

### 5. Sharing

- Share via link
- Real-time collaboration
- Comment on cells

### 6. Version History

- Auto-save to Drive
- Version tracking
- Revert changes

## Productivity Features

### 7. Magic Commands

| Command | Function |
|---------|----------|
| %time | Measure execution time |
| %who | List variables |
| %load_ext | Load extension |
| %%writefile | Save to file |

### 8. Terminal Access

```bash
# Run shell commands
!pip install package
!git clone repo
!ls -la
```

## Advanced Features

### 9. Tensor Processing Units (TPU)

- Free TPU access available
- Faster for TensorFlow

### 10. Forms

Create interactive forms:

```python
#@title Enter your name
name = '' #@param {type:"string"}
```

### 11. Visualization

| Library | Purpose |
|---------|---------|
| Matplotlib | Basic plots |
| Seaborn | Statistical |
| Plotly | Interactive |
| Altair | Declarative |