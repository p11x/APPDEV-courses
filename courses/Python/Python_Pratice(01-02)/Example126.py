# Example126.py
# Topic: Context Managers — Multiple Managers

# Using multiple context managers with with

# === Multiple files at once ===
# Method 1: Nested with
with open("file1.txt", "w") as f1:
    f1.write("Content 1")
    
with open("file2.txt", "w") as f2:
    f2.write("Content 2")

# Method 2: Single line (comma) - Python 2.7+
with open("file1.txt", "r") as f1, open("file2.txt", "r") as f2:
    content1 = f1.read()
    content2 = f2.read()
    print("File 1: " + content1)
    print("File 2: " + content2)

# === Multiple files - read and write ===
# Read from one, write to another
with open("file1.txt", "r") as source, open("file3.txt", "w") as dest:
    data = source.read()
    dest.write(data.upper())
    print("Copied and uppercased!")

# === More than two files ===
with open("a.txt", "w") as a, open("b.txt", "w") as b, open("c.txt", "w") as c:
    a.write("A")
    b.write("B")
    c.write("C")
    print("Wrote to three files")

# === Cleanup in reverse order ===
# Files are closed in reverse order of opening
print("\nOpening files...")
with open("test_a.txt", "w") as a, open("test_b.txt", "w") as b:
    a.write("A content")
    b.write("B content")
    print("Inside with - files are open")

# Files automatically closed in order: b, then a

# === Practical: Process multiple files ===
# Count lines in multiple files
file_names = ["file1.txt", "file2.txt", "file3.txt"]

# Create test files first
for name in file_names:
    with open(name, "w") as f:
        f.write("line 1\nline 2\nline 3\n")

# Now count lines
total_lines = 0
with open(file_names[0], "r") as f1, \
     open(file_names[1], "r") as f2, \
     open(file_names[2], "r") as f3:
    total_lines = len(f1.readlines()) + len(f2.readlines()) + len(f3.readlines())

print("Total lines: " + str(total_lines))

# === Using contextlib for multiple managers ===
from contextlib import ExitStack

# ExitStack manages multiple context managers
with ExitStack() as stack:
    f1 = stack.enter_context(open("file1.txt", "r"))
    f2 = stack.enter_context(open("file2.txt", "r"))
    # Both automatically managed
    print("Stack f1: " + f1.read()[:20])
    print("Stack f2: " + f2.read()[:20])
# All closed automatically!

# === ExitStack with unknown number of files ===
filenames = ["file1.txt", "file2.txt", "file3.txt"]

with ExitStack() as stack:
    files = [stack.enter_context(open(name, "r")) for name in filenames]
    # All files now open
    for f in files:
        print("File: " + f.read().strip())
# All closed when block exits

# === Nested context managers ===
# Each level manages its own resources
with open("outer.txt", "w") as outer:
    outer.write("Outer")
    with open("inner.txt", "w") as inner:
        inner.write("Inner")
    # Inner closed here
# Outer closed here

print("\nNested files created")

# === Best practices for multiple managers ===
print("\n=== Best Practices ===")
print("1. Use comma syntax for simple cases")
print("2. Use \\ for line continuation with many files")
print("3. Use ExitStack for dynamic number of files")
print("4. Files close in reverse order of opening")
print("5. Clean up test files after use")
