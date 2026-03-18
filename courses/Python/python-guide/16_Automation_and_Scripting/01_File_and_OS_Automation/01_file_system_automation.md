# 📁 File System Automation

## 🎯 What You'll Learn

- Using pathlib for file operations
- Bulk file operations
- Using watchdog for file watching

---

## pathlib Basics

```python
from pathlib import Path

# Create paths
p = Path("my_folder/file.txt")

# Check existence
p.exists()
p.is_file()
p.is_dir()

# Read/write
content = p.read_text()
p.write_text("Hello!")

# List files
for f in Path(".").rglob("*.py"):
    print(f)
```

---

## Bulk Operations

```python
from pathlib import Path

# Rename all files
for f in Path(".").glob("*.txt"):
    f.rename(f"backup_{f.name}")

# Move files by extension
for f in Path("downloads").glob("*.pdf"):
    f.rename(Path("documents") / f.name)
```

---

## Watch Files with watchdog

```bash
pip install watchdog
```

```python
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler

class MyHandler(FileSystemEventHandler):
    def on_modified(self, event):
        if not event.is_directory:
            print(f"Modified: {event.src_path}")

observer = Observer()
observer.schedule(MyHandler(), path=".", recursive=True)
observer.start()

# observer.stop() when done
```

---

## shutil Operations

```python
import shutil

# Copy, move, remove
shutil.copy("a.txt", "b.txt")
shutil.move("a.txt", "b.txt")
shutil.rmtree("folder")  # Recursive delete
shutil.make_archive("backup", "zip", "folder")
```

---

## ✅ Summary

- pathlib is the modern way to work with files
- Use rglob() for recursive file search
- watchdog watches files for changes

## 🔗 Further Reading

- [pathlib Documentation](https://docs.python.org/3/library/pathlib.html)
