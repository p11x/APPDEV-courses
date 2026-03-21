# Example129.py
# Topic: Built-in Context Managers — File Handling with open()

# The open() function is Python's most common context manager
# It automatically closes the file when the block ends
# This prevents resource leaks and data loss

import tempfile
import os

# First, create a test file for our examples
test_file = tempfile.NamedTemporaryFile(mode="w", delete=False, suffix=".txt")
test_file.write("Hello, World!\nThis is a test file.\nLine 3 here.\nLine 4 for testing.")
test_file.close()

# Reading a text file (most common use case)
# The 'r' mode means read, 't' means text (default)
with open(test_file.name, "r") as f:
    content = f.read()
    print(content)

# Reading line by line (good for large files)
with open(test_file.name, "r") as f:
    for line in f:
        print(line.rstrip())

# Writing to a file (creates new or overwrites)
output_file = tempfile.NamedTemporaryFile(mode="w", delete=False, suffix=".txt")
output_file.close()
with open(output_file.name, "w") as f:
    f.write("Hello, World!")

# Appending to a file (adds to end)
with open(output_file.name, "a") as f:
    f.write("\nNew line added")

# Reading and writing at once
with open(test_file.name, "r+") as f:
    content = f.read()
    f.seek(0)
    f.write("Modified: " + content)

# Binary mode for images, audio, etc.
binary_file = tempfile.NamedTemporaryFile(mode="wb", delete=False)
binary_file.write(b"\x00\x01\x02\x03")
binary_file.close()

with open(binary_file.name, "rb") as f:
    data = f.read()
    print(data)

# Writing binary data
with open(binary_file.name, "wb") as f:
    f.write(b"\x00\x01\x02\x03")

# Reading with encoding specified (important for non-English text)
unicode_file = tempfile.NamedTemporaryFile(mode="w", delete=False, encoding="utf-8")
unicode_file.write("Hello in Hindi: Hello")
unicode_file.close()

with open(unicode_file.name, "r", encoding="utf-8") as f:
    text = f.read()
    print(text)

# Using with multiple files at once (copy content)
input_file = tempfile.NamedTemporaryFile(mode="w", delete=False)
input_file.write("line 1\nline 2\nline 3")
input_file.close()

output_copy = tempfile.NamedTemporaryFile(mode="w", delete=False)
output_copy.close()

with open(input_file.name, "r") as fin, open(output_copy.name, "w") as fout:
    for line in fin:
        fout.write(line.upper())

with open(output_copy.name, "r") as f:
    print(f.read())

# Using tell() to get current position
with open(test_file.name, "r") as f:
    print(f.tell())
    f.read(5)
    print(f.tell())

# Using seek() to move around
with open(test_file.name, "r") as f:
    f.seek(0)
    first_char = f.read(1)
    f.seek(0)
    first_char_again = f.read(1)
    print(first_char)
    print(first_char_again)

# Cleanup
os.unlink(test_file.name)
os.unlink(output_file.name)
os.unlink(binary_file.name)
os.unlink(unicode_file.name)
os.unlink(input_file.name)
os.unlink(output_copy.name)

print("All tests passed!")
