# CLI Tools with Click

## What You'll Learn

- Creating command-line interfaces
- Using Click framework
- Adding arguments and options

## Prerequisites

- Completed `06-documentation.md`

## Installing Click

```bash
pip install click
```

## Basic CLI

```python
# mycli/cli.py
import click

@click.command()
@click.argument("name")
@click.option("--count", default=1, help="Number of times to greet")
def hello(name: str, count: int) -> None:
    """Simple CLI program."""
    for _ in range(count):
        click.echo(f"Hello, {name}!")

if __name__ == "__main__":
    hello()
```

🔍 **Line-by-Line Breakdown:**

1. `import click` — Click is a composable CLI framework for Python.
2. `@click.command()` — Decorator that turns a function into a Click command.
3. `@click.argument("name")` — Defines a required positional argument.
4. `@click.option("--count", default=1, help="...")` — Defines an optional flag.
5. `def hello(name: str, count: int) -> None:` — Function with type hints.
6. `click.echo(...)` — Print to console (better than print).
7. `if __name__ == "__main__":` — Entry point for running the script.

## Arguments vs Options

- **Arguments** - Positional (required)
- **Options** - Flags (optional, start with --)

```python
@click.command()
@click.argument("input_file")  # Required positional
@click.argument("output_file")  # Required positional
@click.option("--format", "-f", default="json", help="Output format")  # Optional flag
@click.option("--verbose", "-v", is_flag=True, help="Enable verbose mode")  # Boolean flag
def convert(input_file: str, output_file: str, format: str, verbose: bool) -> None:
    """Convert INPUT_FILE to OUTPUT_FILE."""
    if verbose:
        click.echo(f"Converting {input_file} to {format}...")
    # Conversion logic
```

## Groups and Subcommands

```python
@click.group()
def cli() -> None:
    """Main CLI group."""
    pass

@cli.group()
def db() -> None:
    """Database commands."""
    pass

@db.command()
def init() -> None:
    """Initialize database."""
    click.echo("Database initialized!")

@db.command()
def migrate() -> None:
    """Run migrations."""
    click.echo("Running migrations...")

@cli.command()
def version() -> None:
    """Show version."""
    click.echo("CLI v1.0.0")

if __name__ == "__main__":
    cli()
```

## Prompts

```python
@click.command()
def create_user() -> None:
    """Create a new user."""
    name = click.prompt("Enter name", type=str)
    email = click.prompt("Enter email", type=click.Email())
    age = click.prompt("Enter age", type=int, default=18)
    
    click.echo(f"Created user: {name} ({email})")
```

## Progress Bars

```python
@click.command()
def process_items() -> None:
    """Process items with progress bar."""
    items = list(range(100))
    
    with click.progressbar(items, label="Processing") as bar:
        for item in bar:
            # Process item
            pass
    
    click.echo("Done!")
```

## Configuration in pyproject.toml

```toml
[project.scripts]
mycli = "mycli.cli:cli"
```

## Summary

- Click makes CLI creation easy
- Use arguments for required input
- Use options for optional flags and flags

## Next Steps

Continue to `08-distributing-cli-tools.md`.
