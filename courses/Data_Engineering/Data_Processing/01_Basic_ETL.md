---
Category: Data Engineering
Subcategory: Data Processing
Concept: ETL Fundamentals
Purpose: Understanding Extract, Transform, Load processes for cloud data
Difficulty: beginner
Prerequisites: SQL Basics
RelatedFiles: 02_Advanced_ETL.md
UseCase: Data pipeline construction
LastUpdated: 2025
---

## WHY

ETL processes are fundamental for data integration and analytics in the cloud.

## WHAT

### ETL Components

- **Extract**: Source data retrieval
- **Transform**: Data transformation
- **Load**: Destination storage

## HOW

### AWS Glue Example

```python
import sys
from awsglue.transforms import *
from awsglue.utils import getResolvedOptions
from pyspark.context import SparkContext
from awsglue.context import GlueContext

sc = SparkContext.getOrCreate()
glueContext = GlueContext(sc)

# Extract
datasource = glueContext.create_dynamic_frame().from_catalog(
    database="source_db",
    table_name="source_table"
)

# Transform
transformed = Map.apply(
    frame=datasource,
    f=lambda x: {"id": x["id"], "name": x["name"].upper()}
)

# Load
glueContext.write_dynamic_frame().from_catalog(
    transformed,
    database="target_db",
    table_name="target_table"
)
```

## CROSS-REFERENCES

### Related Services

- Data Pipeline: AWS managed ETL
- Lambda: Event-driven ETL
- Athena: Query-based ETL