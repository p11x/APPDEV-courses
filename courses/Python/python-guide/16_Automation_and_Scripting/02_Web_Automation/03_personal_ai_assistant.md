# 🤖 Personal AI Assistant

## 🎯 What You'll Learn

- Building an AI assistant CLI
- Combining multiple tools
- Architecture overview

---

## Architecture

```
User Input → Command Parser → Route to Agent
                        ↓
        ┌──────────────┬──────────────┬──────────────┐
        ↓              ↓              ↓              ↓
   Chat Agent    File Agent    Web Agent    Task Agent
        ↓              ↓              ↓              ↓
        └──────────────┴──────────────┴──────────────┘
                        ↓
              Claude API
                        ↓
              Rich Output
```

---

## Main Structure

```python
# assistant/main.py
import typer
import os
from pathlib import Path
from dotenv import load_dotenv
from rich.console import Console

load_dotenv()
app = typer.Typer()
console = Console()

@app.command()
def chat():
    """Interactive chat mode."""
    console.print("[bold blue]Chat mode![/bold blue] (Ctrl+C to exit)")
    # Implement chat loop...

@app.command()
def summarize(file: str):
    """Summarize a file using AI."""
    # Read file
    path = Path(file)
    if not path.exists():
        console.print("[red]File not found![/red]")
        return
    
    content = path.read_text()
    # Send to Claude, print summary...

@app.command()
def scrape(url: str):
    """Scrape a URL and summarize."""
    # Fetch URL, summarize with Claude...

if __name__ == "__main__":
    app()
```

---

## Running

```bash
# Chat mode
python -m assistant chat

# Summarize file
python -m assistant summarize document.txt

# Scrape URL
python -m assistant scrape https://example.com
```

---

## ✅ Summary

- Combine multiple tools in one CLI
- Use Rich for beautiful output
- Separate agents for different tasks
