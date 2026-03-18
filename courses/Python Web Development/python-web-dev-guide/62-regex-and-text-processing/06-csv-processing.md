# CSV Processing

## What You'll Learn

- Reading CSV files
- Writing CSV files
- Using pandas for CSV

## Prerequisites

- Completed `05-working-with-json.md`

## Basic CSV Reading

```python
import csv

# Read CSV file
with open("data.csv", "r") as f:
    reader = csv.reader(f)
    for row in reader:
        print(row)

# Read with headers
with open("data.csv", "r") as f:
    reader = csv.DictReader(f)
    for row in reader:
        print(row["name"], row["email"])
```

## Basic CSV Writing

```python
import csv

# Write CSV
data = [
    ["name", "email", "age"],
    ["Alice", "alice@example.com", "30"],
    ["Bob", "bob@example.com", "25"]
]

with open("output.csv", "w", newline="") as f:
    writer = csv.writer(f)
    writer.writerows(data)

# Write with headers
with open("output.csv", "w", newline="") as f:
    fieldnames = ["name", "email", "age"]
    writer = csv.DictWriter(f, fieldnames=fieldnames)
    writer.writeheader()
    writer.writerow({"name": "Alice", "email": "alice@example.com", "age": "30"})
```

## Using Pandas

```python
import pandas as pd

# Read CSV
df = pd.read_csv("data.csv")
print(df.head())

# Read with specific columns
df = pd.read_csv("data.csv", usecols=["name", "email"])

# Filter rows
df = df[df["age"] > 25]

# Write CSV
df.to_csv("output.csv", index=False)
```

## Processing Large Files

```python
import csv

def process_large_csv(filepath: str):
    """Process large CSV file in chunks."""
    chunk_size = 1000
    
    with open(filepath, "r") as f:
        reader = csv.reader(f)
        headers = next(reader)  # Get headers
        
        chunk = []
        for row in reader:
            chunk.append(row)
            
            if len(chunk) >= chunk_size:
                # Process chunk
                yield chunk
                chunk = []
        
        # Process remaining rows
        if chunk:
            yield chunk
```

## Data Transformation

```python
import csv

def transform_csv(input_file: str, output_file: str):
    """Transform CSV data."""
    with open(input_file, "r") as infile, open(output_file, "w", newline="") as outfile:
        reader = csv.DictReader(infile)
        fieldnames = ["name", "email", "age", "status"]
        
        writer = csv.DictWriter(outfile, fieldnames=fieldnames)
        writer.writeheader()
        
        for row in reader:
            # Transform data
            row["status"] = "active" if int(row["age"]) > 18 else "minor"
            writer.writerow(row)
```

## Summary

- Use csv module for basic operations
- Use pandas for data analysis
- Process large files in chunks

## Next Steps

Continue to `07-log-parsing.md`.
