# Example282: Working with Files
import os
import tempfile
from pathlib import Path

# Create temp file
print("File Operations:")

# Using tempfile
with tempfile.NamedTemporaryFile(mode='w', delete=False, suffix='.txt') as f:
    f.write("Hello, World!\n")
    f.write("Second line\n")
    temp_path = f.name

print(f"Temp file: {temp_path}")

# Read file
with open(temp_path, 'r') as f:
    content = f.read()
    print(f"Content: {content}")

# Read lines
with open(temp_path, 'r') as f:
    lines = f.readlines()
    print(f"Lines: {lines}")

# Append to file
with open(temp_path, 'a') as f:
    f.write("Third line\n")

# Read again
with open(temp_path, 'r') as f:
    print(f"After append: {f.read()}")

# Using pathlib
print("\nPathlib:")
path = Path(temp_path)
print(f"Exists: {path.exists()}")
print(f"Name: {path.name}")
print(f"Stem: {path.stem}")
print(f"Suffix: {path.suffix}")

# Cleanup
os.unlink(temp_path)
print(f"\nAfter cleanup exists: {os.path.exists(temp_path)}")
