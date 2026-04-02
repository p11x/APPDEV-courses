# ⌨️ Building CLIs with argparse and Click

## 🎯 What You'll Learn

- Using argparse for CLI argument parsing
- Building CLIs with Click
- When to use each

---

## argparse Basics

```python
import argparse

parser = argparse.ArgumentParser(description="My CLI tool")

# Positional argument
parser.add_argument("name", help="Name of the user")

# Optional argument
parser.add_argument("--age", type=int, help="User's age")

# Flag (boolean)
parser.add_argument("--verbose", "-v", action="store_true")

# With choices
parser.add_argument("--format", choices=["json", "csv", "table"])

# Parse
args = parser.parse_args()

print(f"Name: {args.name}")
print(f"Age: {args.age}")
print(f"Verbose: {args.verbose}")
```

### Usage

```bash
python script.py Alice --age 30 --verbose
python script.py --format json Bob
```

---

## Click Basics

```bash
pip install click
```

```python
import click

@click.command()
@click.argument("name")
@click.option("--age", type=int, help="User's age")
@click.option("--verbose", "-v", is_flag=True)
def greet(name, age, verbose):
    """Greet a user."""
    if verbose:
        click.echo("Verbose mode enabled!")
    click.echo(f"Hello, {name}!")
    if age:
        click.echo(f"You are {age} years old.")

if __name__ == "__main__":
    greet()
```

---

## Subcommands

```python
import click

@click.group()
def cli():
    """My CLI application."""
    pass

@cli.command()
def init():
    """Initialize the project."""
    click.echo("Initialized!")

@cli.group()
def config():
    """Manage configuration."""
    pass

@config.command()
def show():
    """Show configuration."""
    click.echo("Config: ...")

if __name__ == "__main__":
    cli()
```

---

## Comparison

| Feature | argparse | Click |
|---------|----------|-------|
| Boilerplate | More | Less |
| Subcommands | Manual | Built-in |
| Styling | Limited | Rich |
| Type coercion | Manual | Automatic |

---

## ✅ Summary

- argparse: stdlib, more control
- Click: cleaner syntax, built-in subcommands

## 🔗 Further Reading

- [Click Documentation](https://click.palletsprojects.com/)
