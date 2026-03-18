# Pandas with FastAPI

## What You'll Learn
- Using Pandas for data processing
- Building data APIs
- CSV/Excel file handling
- Data validation

## Prerequisites
- Basic Pandas knowledge

## Installation

```bash
pip install pandas openpyxl
```

## Basic Example

```python
from fastapi import FastAPI, File, UploadFile
from fastapi.responses import StreamingResponse
import pandas as pd
import io

app = FastAPI()

@app.post("/analyze")
async def analyze_data(file: UploadFile = File(...)):
    """Analyze uploaded CSV file."""
    
    # Read CSV
    df = pd.read_csv(file.file)
    
    # Process data
    result = {
        "row_count": len(df),
        "column_count": len(df.columns),
        "columns": df.columns.tolist(),
        "stats": df.describe().to_dict()
    }
    
    return result

@app.get("/export")
async def export_data():
    """Export processed data as CSV."""
    
    df = pd.DataFrame({
        "name": ["Alice", "Bob", "Charlie"],
        "age": [25, 30, 35]
    })
    
    # Create in-memory file
    stream = io.StringIO()
    df.to_csv(stream, index=False)
    
    response = StreamingResponse(
        iter([stream.getvalue()]),
        media_type="text/csv",
        headers={"Content-Disposition": "attachment; filename=data.csv"}
    )
    
    return response
```

## Data Validation

```python
from pydantic import BaseModel
import pandas as pd

class DataRow(BaseModel):
    name: str
    age: int
    email: str

@app.post("/validate")
async def validate_data(data: list[DataRow]):
    """Validate data using Pydantic models."""
    
    df = pd.DataFrame([d.model_dump() for d in data])
    
    # Additional validation
    errors = []
    if (df["age"] < 0).any():
        errors.append("Age cannot be negative")
    
    return {"valid": len(errors) == 0, "errors": errors}
```

## Summary

- Pandas handles data processing in Python
- Use UploadFile for file uploads
- Return data as CSV or Excel
- Validate with Pydantic models
