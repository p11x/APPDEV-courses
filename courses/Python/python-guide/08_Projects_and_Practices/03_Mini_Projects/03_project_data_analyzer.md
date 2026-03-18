# CSV Data Analyzer

## What You'll Learn

- CSV data analyzer
- Using csv module
- statistics module
- collections.Counter
- rich tables

## Prerequisites

- Read [02_project_web_scraper.md](./02_project_web_scraper.md) first

## Data Analyzer

```python
# analyzer.py
import csv
from statistics import mean, median
from collections import Counter
from rich.console import Console
from rich.table import Table


def analyze_csv(filepath: str) -> None:
    """Analyze CSV file and print summary."""
    console = Console()
    
    # Read CSV
    with open(filepath, newline="") as f:
        reader = csv.DictReader(f)
        data = list(reader)
    
    if not data:
        console.print("[red]No data found![/red]")
        return
    
    # Analyze column
    numeric_col = "age"  # example
    values = [int(row[numeric_col]) for row in data]
    
    # Stats
    console.print(f"[bold]Data Summary[/bold]")
    console.print(f"Total rows: {len(data)}")
    console.print(f"Mean: {mean(values):.2f}")
    console.print(f"Median: {median(values):.2f}")
    
    # Count categories
    category_col = "category"
    counter = Counter(row[category_col] for row in data)
    
    # Table
    table = Table(title="Categories")
    table.add_column("Category")
    table.add_column("Count")
    
    for cat, count in counter.most_common():
        table.add_row(cat, str(count))
    
    console.print(table)


if __name__ == "__main__":
    analyze_csv("data.csv")
```

## Summary

- csv module for CSV handling
- statistics for calculations
- Counter for counting
- rich for beautiful output

## Congratulations!

You've completed the Python Programming Guide!
