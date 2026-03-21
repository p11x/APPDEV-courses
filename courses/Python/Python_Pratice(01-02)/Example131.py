# Example131.py
# Topic: Built-in Context Managers — tempfile Module

# tempfile provides tools for creating temporary files and directories
# These are useful for caching, testing, intermediate data processing
# They are automatically cleaned up when the context exits

import tempfile
import os

# TemporaryFile - creates and automatically deletes the file
# Useful when you need a file but don't want to manage cleanup
with tempfile.TemporaryFile(mode="w+") as f:
    f.write("Hello, Temporary World!")
    f.seek(0)
    content = f.read()
    print(content)

# TemporaryFile in binary mode
with tempfile.TemporaryFile(mode="w+b") as f:
    f.write(b"\x00\x01\x02\x03")
    f.seek(0)
    data = f.read()
    print(data)

# NamedTemporaryFile - like TemporaryFile but with a name
# Useful when external programs need the filename
# Note: on Windows, cannot delete while file is open
with tempfile.NamedTemporaryFile(mode="w", delete=True, suffix=".txt") as f:
    f.write("Temporary content here")
    temp_name = f.name
    print(temp_name)

# TemporaryDirectory - creates a temp folder that auto-deletes
with tempfile.TemporaryDirectory() as tmpdir:
    print(tmpdir)
    # Create files inside
    path = os.path.join(tmpdir, "myfile.txt")
    with open(path, "w") as f:
        f.write("Inside temp directory")
    
    # List contents
    files = os.listdir(tmpdir)
    print(files)

# mkdtemp - creates directory without auto-delete
tmpdir = tempfile.mkdtemp()
print(tmpdir)
# Must clean up manually
os.rmdir(tmpdir)

# mkstemp - low-level temporary file (returns fd)
fd, path = tempfile.mkstemp()
print(path)
os.close(fd)
os.unlink(path)

# SpooledTemporaryFile - keeps data in memory until size threshold
# Then writes to disk (useful for large uploads)
with tempfile.SpooledTemporaryFile(max_size=1000) as f:
    f.write(b"Small data")
    f.seek(0)
    print(f.read())

# Practical: Processing large data in chunks
def process_large_data(data_generator):
    with tempfile.TemporaryDirectory() as tmpdir:
        output_path = os.path.join(tmpdir, "processed.txt")
        with open(output_path, "w") as out:
            count = 0
            for chunk in data_generator():
                out.write("Processed: " + chunk + "\n")
                count = count + 1
        return count, output_path

def fake_data():
    for i in range(5):
        yield "item" + str(i)

count, path = process_large_data(fake_data)
print("Processed " + str(count) + " items")

# Using temp directory for testing
test_dir = tempfile.mkdtemp(prefix="test_")
print(test_dir)
os.rmdir(test_dir)

# Getting system temp directory
print(tempfile.gettempdir())

# Real-world: Building a temporary CSV
import csv
with tempfile.NamedTemporaryFile(mode="w", suffix=".csv", delete=True) as f:
    writer = csv.writer(f)
    writer.writerow(["Name", "Age"])
    writer.writerow(["Alice", "30"])
    writer.writerow(["Bob", "25"])
    csv_path = f.name

print(csv_path)

# Real-world: Temporary cache in memory (SpooledTemporaryFile)
cache = tempfile.SpooledTemporaryFile(max_size=5000)
cache.write(b"cached data")
cache.seek(0)
print(cache.read())
cache.close()

# Using mktemp for unique filename (less safe, but sometimes needed)
temp_path = tempfile.mktemp(suffix=".txt")
with open(temp_path, "w") as f:
    f.write("Some temp content")
print(temp_path)
os.unlink(temp_path)

print("All tests passed!")
