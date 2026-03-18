# Working with CSV and Excel

## What You'll Learn
- Reading and writing CSV
- Working with Excel files
- Exporting data

## Prerequisites
- Completed cloud file storage

## CSV Files

```bash
pip install pandas
```

```python
import pandas as pd
from fastapi import FastAPI, UploadFile
from io import StringIO

app = FastAPI()

@app.post("/upload-csv")
async def upload_csv(file: UploadFile):
    """Parse CSV file"""
    content = await file.read()
    df = pd.read_csv(StringIO(content.decode('utf-8')))
    
    return {"rows": len(df), "columns": list(df.columns)}

@app.get("/export-csv")
async def export_csv():
    """Export data as CSV"""
    data = [{"id": 1, "name": "John"}, {"id": 2, "name": "Jane"}]
    df = pd.DataFrame(data)
    
    csv = df.to_csv(index=False)
    
    return Response(
        content=csv,
        media_type="text/csv",
        headers={"Content-Disposition": "attachment; filename=data.csv"}
    )
```

## Excel Files

```python
from io import BytesIO

@app.get("/export-excel")
async def export_excel():
    """Export data as Excel"""
    data = [{"id": 1, "name": "John"}, {"id": 2, "name": "Jane"}]
    df = pd.DataFrame(data)
    
    buffer = BytesIO()
    with pd.ExcelWriter(buffer, engine='openpyxl') as writer:
        df.to_excel(writer, index=False, sheet_name="Users")
    
    return Response(
        content=buffer.getvalue(),
        media_type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        headers={"Content-Disposition": "attachment; filename=users.xlsx"}
    )
```

## Summary
- Use pandas for CSV/Excel
- Return proper content types

## Next Steps
→ Continue to `05-image-processing.md`
