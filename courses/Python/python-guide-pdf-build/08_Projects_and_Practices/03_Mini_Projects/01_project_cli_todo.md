# CLI To-Do App

## What You'll Learn

- Build a CLI To-Do app
- Using argparse, pathlib, json, dataclasses, rich

## Prerequisites

- Read [03_logging_and_debugging.md](./03_logging_and_debugging.md) first

## Project Structure

```python
# todo.py
import argparse
import json
from pathlib import Path
from dataclasses import dataclass
from datetime import datetime


@dataclass
class Task:
    id: int
    title: str
    completed: bool


def load_tasks() -> list[dict]:
    """Load tasks from file."""
    path = Path("tasks.json")
    if path.exists():
        return json.loads(path.read_text())
    return []


def save_tasks(tasks: list[dict]) -> None:
    """Save tasks to file."""
    Path("tasks.json").write_text(json.dumps(tasks, indent=2))


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("command", choices=["add", "list", "done", "delete"])
    parser.add_argument("title", nargs="?")
    args = parser.parse_args()
    
    tasks = load_tasks()
    
    if args.command == "add":
        task_id = max([t["id"] for t in tasks], default=0) + 1
        tasks.append({"id": task_id, "title": args.title, "completed": False})
        save_tasks(tasks)
        print(f"Added: {args.title}")
    
    elif args.command == "list":
        for task in tasks:
            status = "✓" if task["completed"] else " "
            print(f"{status} {task['id']}. {task['title']}")
    
    elif args.command == "done":
        for task in tasks:
            if task["id"] == int(args.title):
                task["completed"] = True
        save_tasks(tasks)
        print("Marked as done")


if __name__ == "__main__":
    main()
```

## Summary

This project demonstrates:
- argparse for CLI
- pathlib for file operations
- json for persistence

## Next Steps

Continue to **[02_project_web_scraper.md](./02_project_web_scraper.md)**
