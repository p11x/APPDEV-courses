# 💾 Storing and Managing Scraped Data

> Where and how to store scraped data for later analysis.

## 🎯 What You'll Learn

- Choosing the right format: CSV vs JSON vs SQLite vs Parquet
- Best practices for each format
- SQLite for structured data with queries
- Parquet for large-scale analytics
- Data versioning and incremental loads

## 📦 Prerequisites

- Completion of [01_etl_pattern.md](./01_etl_pattern.md)
- Understanding of ETL pipeline basics
- Basic knowledge of Python data types

---

## Format Decision Guide

| Format | Best For | Limitations | Size |
|--------|----------|-------------|------|
| **CSV** | Simple data, Excel compatibility | No relationships, slow for large data | Small |
| **JSON** | Nested data, APIs, configs | No queries, redundant for large data | Medium |
| **SQLite** | Structured data, queries, relationships | Not for >1GB | Medium |
| **Parquet** | Large datasets, analytics, ML | Not human-readable | Large |

---

## CSV Best Practices

### Writing CSV Files

```python
import csv
from pathlib import Path


def write_csv(filepath: str, data: list[dict], fieldnames: list[str] | None = None) -> None:
    """Write data to CSV file."""
    
    # Auto-detect fieldnames from first record
    if fieldnames is None and data:
        fieldnames = list(data[0].keys())
    
    filepath = Path(filepath)
    
    # Determine if we're appending
    write_header = not filepath.exists()
    
    with open(filepath, "a", newline="", encoding="utf-8-sig") as f:
        writer = csv.DictWriter(f, fieldnames=fieldnames)
        
        if write_header:
            writer.writeheader()
        
        # Write only specified fields (ignore extras)
        writer.writerows(data)
    
    print(f"Wrote {len(data)} rows to {filepath}")


# Usage
products = [
    {"name": "Widget A", "price": 29.99, "in_stock": True},
    {"name": "Widget B", "price": 49.99, "in_stock": False},
]

write_csv("products.csv", products)
```

### 💡 Explanation

- `newline=""` — prevents extra blank lines on Windows
- `encoding="utf-8-sig"` — adds BOM for Excel compatibility
- `extrasaction="ignore"` — silently ignores extra fields

### Reading CSV Files

```python
import pandas as pd


def read_csv(filepath: str) -> pd.DataFrame:
    """Read CSV file into DataFrame."""
    
    return pd.read_csv(
        filepath,
        encoding="utf-8-sig",  # Handle BOM
        parse_dates=["date"],  # Auto-parse date columns
    )


# Usage
df = read_csv("products.csv")
print(df.head())
```

---

## SQLite for Scraped Data

### Schema Design

```python
import sqlite3


def create_tables(db_path: str) -> None:
    """Create database schema for scraped products."""
    
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()
    
    # Products table
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS products (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            url TEXT UNIQUE NOT NULL,
            name TEXT NOT NULL,
            price REAL,
            rating REAL,
            in_stock INTEGER,
            date_added TEXT DEFAULT CURRENT_TIMESTAMP,
            last_updated TEXT DEFAULT CURRENT_TIMESTAMP
        )
    """)
    
    # Index for fast URL lookups (important for deduplication!)
    cursor.execute("""
        CREATE INDEX IF NOT EXISTS idx_products_url ON products(url)
    """)
    
    # Price history table (for tracking price changes)
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS price_history (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            product_url TEXT NOT NULL,
            price REAL NOT NULL,
            recorded_at TEXT DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (product_url) REFERENCES products(url)
        )
    """)
    
    # Index for fast price lookups
    cursor.execute("""
        CREATE INDEX IF NOT EXISTS idx_price_history_url 
        ON price_history(product_url)
    """)
    
    conn.commit()
    conn.close()
    
    print("Created database schema")


# Usage
create_tables("products.db")
```

### Upsert Pattern

```python
import sqlite3
import pandas as pd
from datetime import datetime


def upsert_product(conn: sqlite3.Connection, product: dict) -> None:
    """Insert or update a product (upsert)."""
    
    cursor = conn.cursor()
    
    cursor.execute("""
        INSERT INTO products (url, name, price, rating, in_stock, last_updated)
        VALUES (?, ?, ?, ?, ?, ?)
        ON CONFLICT(url) DO UPDATE SET
            name = excluded.name,
            price = excluded.price,
            rating = excluded.rating,
            in_stock = excluded.in_stock,
            last_updated = excluded.last_updated
    """, (
        product["url"],
        product["name"],
        product.get("price"),
        product.get("rating"),
        int(product.get("in_stock", False)),
        datetime.now().isoformat()
    ))
    
    # Also record price history
    if "price" in product and product["price"]:
        cursor.execute("""
            INSERT INTO price_history (product_url, price)
            VALUES (?, ?)
        """, (product["url"], product["price"]))


def load_to_sqlite_upsert(df: pd.DataFrame, db_path: str) -> None:
    """Load DataFrame to SQLite with upsert."""
    
    conn = sqlite3.connect(db_path)
    
    for _, row in df.iterrows():
        upsert_product(conn, row.to_dict())
    
    conn.commit()
    conn.close()
    
    print(f"Upserted {len(df)} products")


# Usage
load_to_sqlite_upsert(df, "products.db")
```

### 💡 Explanation

- `ON CONFLICT(url) DO UPDATE` — updates if URL exists, inserts if not
- Records every price change in price_history table
- `last_updated` tracks when data was last modified

### Querying Scraped Data

```python
import sqlite3
import pandas as pd


def query_products(db_path: str) -> pd.DataFrame:
    """Query products from SQLite."""
    
    conn = sqlite3.connect(db_path)
    
    # Query with Pandas
    df = pd.read_sql_query("""
        SELECT 
            name,
            price,
            rating,
            in_stock,
            last_updated
        FROM products
        WHERE price > 20
        ORDER BY price DESC
    """, conn)
    
    conn.close()
    return df


def get_price_history(db_path: str, url: str) -> pd.DataFrame:
    """Get price history for a product."""
    
    conn = sqlite3.connect(db_path)
    
    df = pd.read_sql_query("""
        SELECT 
            price,
            recorded_at
        FROM price_history
        WHERE product_url = ?
        ORDER BY recorded_at DESC
    """, conn, params=[url])
    
    conn.close()
    return df


# Usage
df = query_products("products.db")
print(df.head())
```

---

## Parquet for Large Datasets

### When to Use Parquet

```python
import pandas as pd


def load_to_parquet(df: pd.DataFrame, filepath: str) -> None:
    """Save DataFrame to Parquet format."""
    
    df.to_parquet(
        filepath,
        compression="gzip",     # gzip, snappy, brotli, or None
        index=False,            # Don't save index
        engine="pyarrow"        # or fastparquet
    )
    
    print(f"Saved {len(df)} rows to {filepath}")
    print(f"File size: {Path(filepath).stat().st_size / 1024 / 1024:.2f} MB")


def read_parquet(filepath: str) -> pd.DataFrame:
    """Read Parquet file - very fast for large datasets."""
    
    return pd.read_parquet(filepath)


# Usage
# For 100k+ rows, Parquet is MUCH faster than CSV
df = read_parquet("large_dataset.parquet")
print(df.info())
```

### Why Parquet?

```
CSV File (100MB):
  - Reads ALL columns every time
  - Slow for large files
  - No compression

Parquet File (10MB):
  - Columnar: reads only needed columns
  - Compressed by default
  - Fast for analytics/ML
```

---

## Data Versioning

Track when you scraped what:

```python
import sqlite3
from datetime import datetime
from pathlib import Path


class ScraperDB:
    """Database manager with versioning."""
    
    def __init__(self, db_path: str):
        self.db_path = db_path
        self._init_db()
    
    def _init_db(self) -> None:
        """Initialize database with versioning."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        # Scrape runs table
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS scrape_runs (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                run_date TEXT NOT NULL,
                source_url TEXT NOT NULL,
                records_fetched INTEGER,
                duration_seconds REAL
            )
        """)
        
        # Products table
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS products (
                url TEXT PRIMARY KEY,
                name TEXT,
                price REAL,
                scraped_at TEXT NOT NULL
            )
        """)
        
        conn.commit()
        conn.close()
    
    def start_run(self, source_url: str) -> int:
        """Record start of a scrape run."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            INSERT INTO scrape_runs (run_date, source_url)
            VALUES (?, ?)
        """, (datetime.now().isoformat(), source_url))
        
        run_id = cursor.lastrowid
        conn.commit()
        conn.close()
        
        return run_id
    
    def end_run(self, run_id: int, records: int, duration: float) -> None:
        """Record end of a scrape run."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            UPDATE scrape_runs
            SET records_fetched = ?, duration_seconds = ?
            WHERE id = ?
        """, (records, duration, run_id))
        
        conn.commit()
        conn.close()
    
    def get_run_history(self) -> list[dict]:
        """Get history of scrape runs."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            SELECT 
                run_date,
                source_url,
                records_fetched,
                duration_seconds
            FROM scrape_runs
            ORDER BY run_date DESC
            LIMIT 10
        """)
        
        rows = cursor.fetchall()
        conn.close()
        
        return [
            {
                "date": row[0],
                "source": row[1],
                "records": row[2],
                "duration": row[3]
            }
            for row in rows
        ]


# Usage
db = ScraperDB("scraper.db")

# Start tracking
run_id = db.start_run("https://example.com/products")
print(f"Started run #{run_id}")

# ... do scraping ...

# End tracking
db.end_run(run_id, records=100, duration=45.5)

# View history
for run in db.get_run_history():
    print(f"{run['date']}: {run['records']} records in {run['duration']:.1f}s")
```

---

## Full Example: Price Tracker Database

```python
import sqlite3
import pandas as pd
from datetime import datetime
from pathlib import Path


class PriceTrackerDB:
    """Complete price tracker database."""
    
    def __init__(self, db_path: str = "price_tracker.db"):
        self.db_path = db_path
        self._init_schema()
    
    def _init_schema(self) -> None:
        """Create all tables and indexes."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        # Products table
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS products (
                url TEXT PRIMARY KEY,
                name TEXT NOT NULL,
                category TEXT,
                first_seen TEXT DEFAULT CURRENT_TIMESTAMP,
                last_seen TEXT DEFAULT CURRENT_TIMESTAMP
            )
        """)
        
        # Prices table (history)
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS prices (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                url TEXT NOT NULL,
                price REAL NOT NULL,
                recorded_at TEXT DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (url) REFERENCES products(url)
            )
        """)
        
        # Indexes
        cursor.execute("""
            CREATE INDEX IF NOT EXISTS idx_prices_url 
            ON prices(url, recorded_at)
        """)
        
        conn.commit()
        conn.close()
    
    def upsert_product(self, url: str, name: str, price: float, 
                       category: str = None) -> None:
        """Insert or update product with current price."""
        
        now = datetime.now().isoformat()
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        # Upsert product
        cursor.execute("""
            INSERT INTO products (url, name, category, last_seen)
            VALUES (?, ?, ?, ?)
            ON CONFLICT(url) DO UPDATE SET
                name = excluded.name,
                category = excluded.category,
                last_seen = excluded.last_seen
        """, (url, name, category, now))
        
        # Insert price
        cursor.execute("""
            INSERT INTO prices (url, price, recorded_at)
            VALUES (?, ?, ?)
        """, (url, price, now))
        
        conn.commit()
        conn.close()
    
    def get_current_prices(self) -> pd.DataFrame:
        """Get current prices for all products."""
        
        conn = sqlite3.connect(self.db_path)
        
        df = pd.read_sql_query("""
            SELECT 
                p.name,
                p.category,
                p.url,
                pr.price as current_price,
                p.last_seen
            FROM products p
            JOIN (
                SELECT url, price, recorded_at
                FROM prices p1
                WHERE recorded_at = (
                    SELECT MAX(recorded_at)
                    FROM prices p2
                    WHERE p1.url = p2.url
                )
            ) pr ON p.url = pr.url
            ORDER BY p.name
        """, conn)
        
        conn.close()
        return df
    
    def get_price_history(self, url: str, days: int = 30) -> pd.DataFrame:
        """Get price history for a product."""
        
        conn = sqlite3.connect(self.db_path)
        
        df = pd.read_sql_query("""
            SELECT 
                price,
                recorded_at
            FROM prices
            WHERE url = ?
              AND recorded_at >= datetime('now', '-' || ? || ' days')
            ORDER BY recorded_at ASC
        """, conn, params=[url, days])
        
        conn.close()
        
        # Parse dates
        df["recorded_at"] = pd.to_datetime(df["recorded_at"])
        
        return df
    
    def get_price_drops(self, threshold: float = 10.0) -> list[dict]:
        """Find products with price drops."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            WITH latest_prices AS (
                SELECT 
                    url,
                    price,
                    recorded_at,
                    LAG(price) OVER (
                        PARTITION BY url 
                        ORDER BY recorded_at
                    ) as prev_price
                FROM prices
            )
            SELECT 
                p.name,
                lp.prev_price as old_price,
                lp.price as new_price,
                ((lp.prev_price - lp.price) / lp.prev_price * 100) as discount_percent
            FROM latest_prices lp
            JOIN products p ON lp.url = p.url
            WHERE lp.prev_price IS NOT NULL
              AND lp.price < lp.prev_price
              AND ((lp.prev_price - lp.price) / lp.prev_price * 100) >= ?
            ORDER BY discount_percent DESC
        """, (threshold,))
        
        rows = cursor.fetchall()
        conn.close()
        
        return [
            {
                "name": row[0],
                "old_price": row[1],
                "new_price": row[2],
                "discount": row[3]
            }
            for row in rows
        ]


# Usage
tracker = PriceTrackerDB()

# Add some products
tracker.upsert_product(
    "https://example.com/product/1",
    "Wireless Mouse",
    29.99,
    "Electronics"
)

tracker.upsert_product(
    "https://example.com/product/1",
    "Wireless Mouse",
    24.99,  # Price dropped!
    "Electronics"
)

# Get current prices
print("Current Prices:")
print(tracker.get_current_prices())

# Find price drops
print("\nPrice Drops:")
for drop in tracker.get_price_drops():
    print(f"  {drop['name']}: ${drop['old_price']} → ${drop['new_price']} ({drop['discount']:.1f}% off)")
```

---

## Summary

✅ **CSV** — simple, Excel-compatible, but slow for large data

✅ **SQLite** — structured, queryable, good for up to ~1GB

✅ **Parquet** — columnar, compressed, perfect for analytics/ML

✅ **Upsert pattern** — INSERT OR REPLACE for deduplication

✅ **Index URLs** — fast lookups for deduplication

✅ **Version everything** — track scrape runs and history

---

## ➡️ Next Steps

Continue to [03_scheduling_and_monitoring_pipelines.md](./03_scheduling_and_monitoring_pipelines.md) to learn about running scrapers automatically and monitoring for failures.

---

## 🔗 Further Reading

- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [Parquet Format](https://parquet.apache.org/documentation/latest/)
- [Pandas IO Tools](https://pandas.pydata.org/docs/user_guide/io.html)
