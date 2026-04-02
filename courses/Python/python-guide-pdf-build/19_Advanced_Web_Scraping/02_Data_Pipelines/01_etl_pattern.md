# 🔄 ETL Pattern

> ETL (Extract, Transform, Load) — the backbone of every data pipeline.

## 🎯 What You'll Learn

- What ETL is and why it matters
- Extract phase: async batch fetching with retry logic
- Transform phase: cleaning, deduplication, normalization
- Load phase: storing to SQLite, CSV, Parquet
- Building a complete ETL pipeline class

## 📦 Prerequisites

- Completion of [01_Scraping_Patterns/03_javascript_heavy_sites.md](../01_Scraping_Patterns/03_javascript_heavy_sites.md)
- Understanding of httpx and BeautifulSoup
- Basic knowledge of SQLite and Pandas

---

## What is ETL?

ETL stands for **Extract, Transform, Load** — three phases that turn raw data into clean, usable data:

```
[Source Website]  →  Extract  →  [Raw Data]
                                         ↓
                                    Transform
                                         ↓
                                    [Clean Data]
                                         ↓
                                       Load
                                         ↓
                              [SQLite / CSV / JSON]
```

### Each Phase

| Phase | What It Does | Examples |
|-------|--------------|----------|
| **Extract** | Pull data from source | HTTP requests, API calls, file reads |
| **Transform** | Clean and reshape data | Remove duplicates, fix types, normalize |
| **Load** | Save to destination | SQLite, CSV, JSON, database |

---

## Extract Phase

Getting data from multiple sources efficiently:

### Async Batch Fetching

```python
import asyncio
import httpx
from typing import Any


async def extract_from_urls(urls: list[str]) -> list[dict[str, Any]]:
    """Extract data from multiple URLs concurrently."""
    
    async with httpx.AsyncClient() as client:
        # Create tasks for all URLs
        tasks = [fetch_product(client, url) for url in urls]
        
        # Execute all tasks concurrently
        results = await asyncio.gather(*tasks, return_exceptions=True)
        
        # Filter out failed fetches
        valid_results = [r for r in results if isinstance(r, dict)]
        
        return valid_results


async def fetch_product(client: httpx.AsyncClient, url: str) -> dict[str, Any]:
    """Fetch a single product page."""
    
    response = await client.get(
        url,
        headers={"User-Agent": "Mozilla/5.0..."},
        timeout=30.0
    )
    response.raise_for_status()
    
    return {
        "url": url,
        "html": response.text,
        "status_code": response.status_code
    }


# Usage
urls = [
    "https://example.com/product/1",
    "https://example.com/product/2",
    "https://example.com/product/3",
]

raw_data = asyncio.run(extract_from_urls(urls))
print(f"Extracted {len(raw_data)} products")
```

### 💡 Explanation

- `asyncio.gather(*tasks)` — runs all requests in parallel
- `return_exceptions=True` — continues even if some requests fail
- List comprehension creates all tasks at once

### Retry Logic with Tenacity

```python
# pip install tenacity
from tenacity import (
    retry,
    stop_after_attempt,
    wait_exponential,
    retry_if_exception_type
)
import httpx


# Decorator to retry failed requests
@retry(
    stop=stop_after_attempt(3),  # Stop after 3 attempts
    wait=wait_exponential(multiplier=1, min=2, max=10),  # Wait 2-10 seconds
    retry=retry_if_exception_type((httpx.ConnectError, httpx.TimeoutException)),
    reraise=True  # Re-raise after all retries fail
)
async def fetch_with_retry(client: httpx.AsyncClient, url: str) -> str:
    """Fetch URL with automatic retry on failure."""
    
    response = await client.get(url)
    response.raise_for_status()
    return response.text


# Usage in extract function
async def extract_with_retry(urls: list[str]) -> list[dict]:
    
    async with httpx.AsyncClient() as client:
        tasks = [
            fetch_with_retry(client, url) 
            for url in urls
        ]
        
        results = await asyncio.gather(*tasks, return_exceptions=True)
        
        # Process results
        valid = []
        for url, result in zip(urls, results):
            if isinstance(result, Exception):
                print(f"Failed to fetch {url}: {result}")
            else:
                valid.append({"url": url, "html": result})
        
        return valid
```

### 💡 Explanation

- `@retry` decorator — automatically retries on failure
- `stop_after_attempt(3)` — gives up after 3 tries
- `wait_exponential()` — waits longer each retry (2s, 4s, 8s...)
- `retry_if_exception_type()` — only retries on specific errors

---

## Transform Phase

Cleaning and reshaping raw data:

### Basic Data Cleaning

```python
from bs4 import BeautifulSoup
import pandas as pd
from typing import Any


def transform_products(raw_data: list[dict[str, Any]]) -> pd.DataFrame:
    """Transform raw HTML into clean structured data."""
    
    products = []
    
    for item in raw_data:
        html = item.get("html", "")
        
        # Parse HTML
        soup = BeautifulSoup(html, "html.parser")
        
        # Extract fields
        try:
            product = {
                "url": item["url"],
                "name": clean_text(soup.select_one(".product-name")),
                "price": clean_price(soup.select_one(".product-price")),
                "rating": clean_rating(soup.select_one(".rating")),
                "in_stock": soup.select_one(".stock") is not None,
            }
            products.append(product)
        except Exception as e:
            print(f"Error parsing {item['url']}: {e}")
    
    return pd.DataFrame(products)


def clean_text(element) -> str:
    """Clean text by stripping whitespace."""
    if element is None:
        return ""
    return element.get_text(strip=True)


def clean_price(element) -> float | None:
    """Extract numeric price from string like '$29.99'."""
    if element is None:
        return None
    
    text = element.get_text(strip=True)
    # Remove $ and convert to float
    numeric = text.replace("$", "").replace(",", "")
    
    try:
        return float(numeric)
    except ValueError:
        return None


def clean_rating(element) -> float | None:
    """Extract rating from string like '4.5 out of 5 stars'."""
    if element is None:
        return None
    
    text = element.get_text(strip=True)
    # Extract first number
    import re
    match = re.search(r"(\d+\.?\d*)", text)
    
    if match:
        return float(match.group(1))
    return None


# Usage
df = transform_products(raw_data)
print(df.head())
```

### Deduplication

```python
def deduplicate(df: pd.DataFrame) -> pd.DataFrame:
    """Remove duplicate products based on URL."""
    
    before = len(df)
    
    # Keep first occurrence for each URL
    df_deduped = df.drop_duplicates(subset=["url"], keep="first")
    
    after = len(df_deduped)
    removed = before - after
    
    print(f"Removed {removed} duplicate(s)")
    
    return df_deduped


# Usage
df = deduplicate(df)
```

### Normalization

```python
def normalize_dates(df: pd.DataFrame) -> pd.DataFrame:
    """Normalize date columns to consistent format."""
    
    # If there's a date column
    if "date_added" in df.columns:
        df["date_added"] = pd.to_datetime(df["date_added"])
    
    return df


def normalize_strings(df: pd.DataFrame) -> pd.DataFrame:
    """Normalize string columns."""
    
    # Strip whitespace and lowercase
    for col in df.select_dtypes(include=["object"]).columns:
        df[col] = df[col].str.strip().str.lower()
    
    return df


def normalize_prices(df: pd.DataFrame) -> pd.DataFrame:
    """Normalize price column."""
    
    if "price" in df.columns:
        # Fill missing with median
        median_price = df["price"].median()
        df["price"] = df["price"].fillna(median_price)
        
        # Round to 2 decimal places
        df["price"] = df["price"].round(2)
    
    return df


# Chain all transformations
df = (df
    .pipe(deduplicate)
    .pipe(normalize_dates)
    .pipe(normalize_strings)
    .pipe(normalize_prices)
)
```

### Validation with Pydantic

```python
from pydantic import BaseModel, Field, validator
from typing import Optional


class Product(BaseModel):
    """Validated product model."""
    
    url: str = Field(..., description="Product URL")
    name: str = Field(..., min_length=1, description="Product name")
    price: float = Field(..., ge=0, description="Product price")
    rating: Optional[float] = Field(None, ge=0, le=5, description="Rating 0-5")
    in_stock: bool = Field(default=False, description="Availability")
    
    @validator("name", pre=True)
    def clean_name(cls, v):
        """Clean name before validation."""
        if isinstance(v, str):
            return v.strip()
        return v
    
    @validator("url")
    def validate_url(cls, v):
        """Ensure URL is valid."""
        if not v.startswith(("http://", "https://")):
            raise ValueError("Invalid URL")
        return v


def validate_products(df: pd.DataFrame) -> list[Product]:
    """Validate all products and return valid ones."""
    
    products = []
    errors = []
    
    for record in df.to_dict("records"):
        try:
            product = Product(**record)
            products.append(product)
        except Exception as e:
            errors.append({"record": record, "error": str(e)})
    
    print(f"Valid: {len(products)}, Invalid: {len(errors)}")
    
    if errors:
        print("Sample errors:")
        for err in errors[:3]:
            print(f"  {err}")
    
    return products
```

---

## Load Phase

Saving cleaned data to storage:

### SQLite

```python
import sqlite3
import pandas as pd


def load_to_sqlite(df: pd.DataFrame, db_path: str, table_name: str) -> None:
    """Load DataFrame to SQLite table."""
    
    # Connect to database
    conn = sqlite3.connect(db_path)
    
    # Upsert: replace existing data with same URL
    # First, create table if not exists
    df.to_sql(
        table_name,
        conn,
        if_exists="replace",  # replace, append, or fail
        index=False
    )
    
    conn.close()
    print(f"Loaded {len(df)} rows to {table_name}")


def load_incremental_sqlite(df: pd.DataFrame, db_path: str, table_name: str) -> None:
    """Load only new records (incremental update)."""
    
    conn = sqlite3.connect(db_path)
    
    # Get existing URLs
    existing = pd.read_sql_query(
        f"SELECT url FROM {table_name}",
        conn
    )
    existing_urls = set(existing["url"])
    
    # Filter to new records only
    new_df = df[~df["url"].isin(existing_urls)]
    
    if len(new_df) > 0:
        new_df.to_sql(
            table_name,
            conn,
            if_exists="append",
            index=False
        )
        print(f"Added {len(new_df)} new records")
    else:
        print("No new records to add")
    
    conn.close()
```

### CSV

```python
import csv
import pandas as pd


def load_to_csv(df: pd.DataFrame, filepath: str, append: bool = False) -> None:
    """Load DataFrame to CSV file."""
    
    mode = "a" if append else "w"
    
    if append and Path(filepath).exists():
        # Append without rewriting headers
        df.to_csv(
            filepath,
            mode=mode,
            header=False,  # Don't write header
            index=False
        )
    else:
        # Write new file with header
        df.to_csv(
            filepath,
            mode=mode,
            header=True,
            index=False,
            encoding="utf-8-sig"  # UTF-8 with BOM for Excel compatibility
        )
    
    print(f"Wrote {len(df)} rows to {filepath}")
```

### Parquet (for Large Datasets)

```python
import pandas as pd


def load_to_parquet(df: pd.DataFrame, filepath: str) -> None:
    """Load DataFrame to Parquet file (compressed, columnar)."""
    
    df.to_parquet(
        filepath,
        compression="gzip",  # gzip, snappy, or None
        index=False
    )
    
    print(f"Wrote {len(df)} rows to {filepath}")


def read_parquet(filepath: str) -> pd.DataFrame:
    """Read Parquet file - fast for large datasets."""
    
    return pd.read_parquet(filepath)
```

---

## Complete ETL Pipeline Class

```python
import asyncio
import httpx
import pandas as pd
from bs4 import BeautifulSoup
from pathlib import Path
from typing import Any, Protocol
from tenacity import retry, stop_after_attempt, wait_exponential


class DataSource(Protocol):
    """Protocol for data sources."""
    
    async def fetch(self) -> list[dict[str, Any]]:
        ...


class ProductScraper:
    """Complete ETL pipeline for scraping products."""
    
    def __init__(self, db_path: str = "products.db"):
        self.db_path = db_path
        self.data: list[dict] = []
    
    # ========== EXTRACT ==========
    
    async def extract(self, urls: list[str]) -> list[dict]:
        """Extract raw HTML from URLs."""
        
        print(f"📥 Extracting {len(urls)} URLs...")
        
        async with httpx.AsyncClient() as client:
            tasks = [self._fetch_with_retry(client, url) for url in urls]
            results = await asyncio.gather(*tasks, return_exceptions=True)
        
        self.data = [
            {"url": url, "html": html}
            for url, html in zip(urls, results)
            if isinstance(html, str)
        ]
        
        print(f"   Extracted {len(self.data)} records")
        return self.data
    
    @retry(stop=stop_after_attempt(3), wait=wait_exponential(min=2, max=10))
    async def _fetch_with_retry(self, client: httpx.Client, url: str) -> str:
        """Fetch URL with retry."""
        
        response = await client.get(
            url,
            headers={"User-Agent": "Mozilla/5.0..."},
            timeout=30.0
        )
        response.raise_for_status()
        return response.text
    
    # ========== TRANSFORM ==========
    
    def transform(self) -> pd.DataFrame:
        """Transform raw HTML to clean DataFrame."""
        
        print("🔄 Transforming data...")
        
        records = []
        for item in self.data:
            soup = BeautifulSoup(item["html"], "html.parser")
            
            record = {
                "url": item["url"],
                "name": self._clean_text(soup.select_one(".product-name")),
                "price": self._clean_price(soup.select_one(".price")),
                "rating": self._clean_rating(soup.select_one(".rating")),
            }
            records.append(record)
        
        df = pd.DataFrame(records)
        
        # Deduplicate
        df = df.drop_duplicates(subset=["url"], keep="first")
        
        print(f"   Transformed {len(df)} records")
        return df
    
    def _clean_text(self, element) -> str:
        return element.get_text(strip=True) if element else ""
    
    def _clean_price(self, element) -> float | None:
        if not element:
            return None
        try:
            return float(element.get_text(strip=True).replace("$", ""))
        except ValueError:
            return None
    
    def _clean_rating(self, element) -> float | None:
        if not element:
            return None
        import re
        match = re.search(r"(\d+\.?\d*)", element.get_text())
        return float(match.group(1)) if match else None
    
    # ========== LOAD ==========
    
    def load(self, df: pd.DataFrame, table_name: str = "products") -> None:
        """Load DataFrame to SQLite."""
        
        print(f"💾 Loading to database...")
        
        import sqlite3
        
        conn = sqlite3.connect(self.db_path)
        df.to_sql(table_name, conn, if_exists="replace", index=False)
        conn.close()
        
        print(f"   Loaded {len(df)} records")
    
    # ========== RUN PIPELINE ==========
    
    async def run(self, urls: list[str]) -> pd.DataFrame:
        """Run complete ETL pipeline."""
        
        # Extract
        await self.extract(urls)
        
        # Transform
        df = self.transform()
        
        # Load
        self.load(df)
        
        return df


# Usage
async def main():
    pipeline = ProductScraper("products.db")
    
    urls = [
        "https://example.com/product/1",
        "https://example.com/product/2",
        # ... more URLs
    ]
    
    df = await pipeline.run(urls)
    print(df.head())


asyncio.run(main())
```

---

## Summary

✅ **ETL = Extract, Transform, Load** phases for data pipelines

✅ — three **Extract** — use asyncio.gather for concurrent fetching

✅ **Transform** — clean with BeautifulSoup, deduplicate, normalize

✅ **Load** — SQLite for queries, CSV for compatibility, Parquet for large data

✅ **Use tenacity** — add retry logic for reliability

✅ **Validate with Pydantic** — ensure data quality

---

## ➡️ Next Steps

Continue to [02_storing_and_managing_scraped_data.md](./02_storing_and_managing_scraped_data.md) to learn about storage formats and data management.

---

## 🔗 Further Reading

- [Pandas Documentation](https://pandas.pydata.org/)
- [Tenacity Documentation](https://tenacity.readthedocs.io/)
- [SQLite Tutorial](https://www.sqlite.org/tutorial.html)
- [Parquet Format](https://parquet.apache.org/)
