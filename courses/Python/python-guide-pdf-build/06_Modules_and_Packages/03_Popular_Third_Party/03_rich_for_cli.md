# rich for CLI

## What You'll Learn

- rich library for beautiful CLI output
- Console, print with markup
- Tables and Progress

## Prerequisites

- Read [02_pathlib_deep_dive.md](./02_pathlib_deep_dive.md) first

## Basic rich

```python
from rich.console import Console
from rich.progress import Progress

console = Console()

console.print("Hello, [bold red]World![/bold red]")
console.print("[green]Success![/green]")
```

## Tables

```python
from rich.table import Table

table = Table(title="Star Wars Movies")
table.add_column("Title")
table.add_column("Year")
table.add_row("A New Hope", "1977")
console.print(table)
```

## Progress

```python
from rich.progress import track

for i in track(range(10)):
    # Do work
    pass
```

## Summary

- **rich**: Beautiful CLI output
- Tables, progress bars, syntax highlighting

## Next Steps

This concludes Modules. Move to **[07_Advanced_Python/01_Generators_and_Iterators/01_iterators_protocol.md](../07_Advanced_Python/01_Generators_and_Iterators/01_iterators_protocol.md)**
