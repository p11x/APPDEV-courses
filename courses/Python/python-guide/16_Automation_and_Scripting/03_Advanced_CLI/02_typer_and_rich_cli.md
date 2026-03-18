# ✨ Typer + Rich: Beautiful Type-Safe CLIs

## 🎯 What You'll Learn

- Building CLIs with Typer
- Adding Rich for beautiful output

---

## Installation

```bash
pip install typer rich
```

---

## Typer Basics

```python
import typer

app = typer.Typer()

@app.command()
def greet(name: str = typer.Argument("World")):
    """Greet someone."""
    print(f"Hello, {name}!")

@app.command()
def add(a: int, b: int):
    """Add two numbers."""
    print(f"Result: {a + b}")

if __name__ == "__main__":
    app()
```

### Usage

```bash
python app.py greet Alice
python app.py add 5 3
```

---

## With Rich

```python
import typer
from rich.console import Console

app = typer.Typer()
console = Console()

@app.command()
def greet(name: str):
    console.print(f"[bold blue]Hello, {name}![/bold blue]")

@app.command()
def table_demo():
    from rich.table import Table
    
    table = Table(title="Star Wars Movies")
    table.add_column("Episode", style="cyan")
    table.add_column("Title", style="magenta")
    table.add_column("Year", style="green")
    
    table.add_row("4", "A New Hope", "1977")
    table.add_row("5", "The Empire Strikes Back", "1980")
    table.add_row("6", "Return of the Jedi", "1983")
    
    console.print(table)

if __name__ == "__main__":
    app()
```

---

## Progress Bars

```python
from rich.progress import Progress, SpinnerColumn, TextColumn

with Progress(
    SpinnerColumn(),
    TextColumn("[progress.description]{task.description}"),
) as progress:
    task = progress.add_task("Processing...", total=100)
    for i in range(100):
        progress.update(task, advance=1)
```

---

## ✅ Summary

- Typer: CLIs from type-annotated functions
- Rich: beautiful terminal output
- Use Console() instead of print()

## 🔗 Further Reading

- [Typer Documentation](https://typer.tiangolo.com/)
- [Rich Documentation](https://rich.readthedocs.io/)
