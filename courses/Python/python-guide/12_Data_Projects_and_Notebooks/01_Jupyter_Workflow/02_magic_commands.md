# ✨ Jupyter Magic Commands

## What You'll Learn

- Use magic commands for efficiency

## Line Magics

```python
%timeit sum(range(1000))  # Time execution
%time range(1000)           # Time one line
%who                       # List variables
%whos                      # Detailed variable list
%env                       # Show environment
```

## Cell Magics

```python
%%time
# Time entire cell
for i in range(1000):
    pass

%%writefile file.py
# Write cell content to file

%%capture
# Capture output
```

## File Operations

```python
%run script.py       # Run Python file
%load file.py        # Load file into cell
%pdb                 # Enable debugger
```

## matplotlib

```python
%matplotlib inline   # Show plots inline
%matplotlib widget   # Interactive plots
```
