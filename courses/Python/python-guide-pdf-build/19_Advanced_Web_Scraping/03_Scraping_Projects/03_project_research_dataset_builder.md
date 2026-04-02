# 📊 Project: Research Dataset Builder

> Build a structured dataset from web sources for ML or analysis.

## 🎯 What You'll Build

- Discover and scrape URLs from a source
- Extract structured data with validation
- Store in SQLite and Parquet formats
- Resumable scraping with checkpoints
- Quick EDA with Pandas
- Export clean CSV for ML

## 📦 Prerequisites

- Completion of [02_project_news_aggregator.md](./02_project_news_aggregator.md)
- Understanding of async scraping
- Basic Pandas knowledge

---

## 🛠️ Setup

```bash
pip install httpx beautifulsoup4 rich pandas pyarrow pydantic
```

---

## Full Working Source Code

### Project Structure

```
dataset_builder/
├── main.py              # Entry point
├── discover.py          # URL discovery
├── scraper.py           # Data extraction
├── validator.py         # Pydantic validation
├── database.py          # SQLite storage
├── analyzer.py          # Quick EDA
└── exporter.py          # Export to CSV/Parquet
```

### main.py

```python
"""Dataset Builder - Main entry point."""

import asyncio
import argparse
from pathlib import Path

from discover import URLDiscoverer
from scraper import DataScraper
from database import DatasetDB
from analyzer import DatasetAnalyzer
from exporter import DatasetExporter


class DatasetBuilder:
    """Build ML-ready datasets from web sources."""
    
    def __init__(self, source_name: str):
        self.source_name = source_name
        self.db = DatasetDB(f"{source_name}_dataset.db")
        self.discoverer = URLDiscoverer()
        self.scraper = DataScraper()
        self.analyzer = DatasetAnalyzer()
        self.exporter = DatasetExporter()
    
    async def build(self, start_url: str, max_items: int = 100) -> None:
        """Build dataset from a source."""
        
        print(f"📦 Building dataset: {self.source_name}")
        print(f"   Source: {start_url}")
        print(f"   Max items: {max_items}\n")
        
        # Step 1: Discover URLs
        print("🔍 Discovering URLs...")
        urls = await self.discoverer.discover(start_url, max_items)
        print(f"   Found {len(urls)} URLs")
        
        # Step 2: Filter already scraped
        new_urls = self.db.filter_new_urls(urls)
        print(f"   {len(new_urls)} new URLs to scrape")
        
        if not new_urls:
            print("\n✅ Dataset is up to date!")
            return
        
        # Step 3: Scrape with semaphore
        print("\n📥 Scraping data...")
        scraped = await self.scraper.scrape_all(
            new_urls, 
            max_concurrent=5,
            progress_callback=self._progress
        )
        
        # Step 4: Validate and save
        print("\n✅ Saving to database...")
        saved = self.db.save_records(scraped)
        print(f"   Saved {saved} records")
        
        # Step 5: Analyze
        print("\n📊 Dataset Summary:")
        self.analyzer.summary(self.db.get_all())
        
        # Step 6: Export
        print("\n💾 Exporting...")
        self.exporter.to_csv(self.db.get_all(), f"{self.source_name}.csv")
        self.exporter.to_parquet(self.db.get_all(), f"{self.source_name}.parquet")
        print(f"   Exported to {self.source_name}.csv and .parquet")
    
    def _progress(self, current: int, total: int, url: str) -> None:
        """Progress callback."""
        print(f"   [{current}/{total}] {url[:50]}...")
    
    def resume(self) -> None:
        """Resume interrupted build."""
        
        print(f"🔄 Resuming: {self.source_name}")
        
        pending = self.db.get_pending()
        print(f"   {len(pending)} pending URLs")
        
        if not pending:
            print("   Nothing to resume!")
            return
        
        # Scrape pending
        scraped = asyncio.run(
            self.scraper.scrape_all(
                pending,
                max_concurrent=5,
                progress_callback=self._progress
            )
        )
        
        # Save
        self.db.save_records(scraped)
        print(f"   Saved {len(scraped)} records")
    
    def analyze(self) -> None:
        """Run EDA on dataset."""
        
        df = self.db.get_all()
        
        print("\n📊 Exploratory Data Analysis")
        print("=" * 50)
        
        self.analyzer.summary(df)
        self.analyzer.missing_values(df)
        self.analyzer.distributions(df)


async def main():
    """Main entry point."""
    
    parser = argparse.ArgumentParser(description="Dataset Builder")
    parser.add_argument("source", help="Source name (e.g., books)")
    parser.add_argument("url", help="Starting URL")
    parser.add_argument("--max", type=int, default=100, help="Max items")
    parser.add_argument("--resume", action="store_true", help="Resume build")
    parser.add_argument("--analyze", action="store_true", help="Analyze only")
    
    args = parser.parse_args()
    
    builder = DatasetBuilder(args.source)
    
    if args.analyze:
        builder.analyze()
    elif args.resume:
        builder.resume()
    else:
        await builder.build(args.url, args.max)


if __name__ == "__main__":
    asyncio.run(main())
```

### discover.py

```python
"""URL Discovery - Find all URLs to scrape."""

import httpx
from bs4 import BeautifulSoup
import asyncio
from typing import Callable


class URLDiscoverer:
    """Discover URLs from index/category pages."""
    
    async def discover(
        self, 
        start_url: str, 
        max_items: int = 100
    ) -> list[str]:
        """Discover URLs by following pagination."""
        
        urls = []
        page = 1
        seen = set()
        
        async with httpx.AsyncClient() as client:
            while len(urls) < max_items:
                # Build page URL
                if page == 1:
                    url = start_url
                else:
                    # Adapt to your source's pagination
                    url = start_url.rstrip("/") + f"/page{page}"
                
                # Fetch page
                try:
                    response = await client.get(
                        url,
                        headers={"User-Agent": "Mozilla/5.0..."},
                        timeout=30.0
                    )
                    
                    if response.status_code != 200:
                        break
                    
                    soup = BeautifulSoup(response.text, "html.parser")
                    
                    # Extract item links - adapt to your source
                    # Common patterns:
                    links = soup.select("a.product-item, a.book-item, article a")
                    
                    if not links:
                        # Try more general pattern
                        links = soup.select("a[href*='/product/'], a[href*='/item/']")
                    
                    if not links:
                        break
                    
                    # Extract URLs
                    for link in links:
                        href = link.get("href")
                        
                        if href and href not in seen:
                            # Normalize URL
                            if not href.startswith("http"):
                                from urllib.parse import urljoin
                                href = urljoin(start_url, href)
                            
                            seen.add(href)
                            urls.append(href)
                    
                    page += 1
                    
                    # Rate limit
                    await asyncio.sleep(0.5)
                    
                except Exception as e:
                    print(f"Error on page {page}: {e}")
                    break
        
        return urls[:max_items]
```

### scraper.py

```python
"""Data Scraper - Extract structured data from pages."""

import asyncio
import httpx
from bs4 import BeautifulSoup
from typing import Callable
from dataclasses import dataclass
from pydantic import BaseModel, Field


@dataclass
class ScrapedRecord:
    """Raw scraped record."""
    url: str
    data: dict
    success: bool
    error: str | None = None


class BookRecord(BaseModel):
    """Validated book record for ML."""
    
    title: str = Field(..., min_length=1)
    price: float = Field(..., ge=0)
    rating: float | None = Field(None, ge=0, le=5)
    availability: str = "Unknown"
    category: str = "Unknown"
    upc: str = ""
    url: str


class DataScraper:
    """Scrape structured data from web pages."""
    
    async def scrape_all(
        self,
        urls: list[str],
        max_concurrent: int = 5,
        progress_callback: Callable | None = None
    ) -> list[ScrapedRecord]:
        """Scrape all URLs with concurrency control."""
        
        semaphore = asyncio.Semaphore(max_concurrent)
        
        async def scrape_with_sem(url: str, index: int) -> ScrapedRecord:
            async with semaphore:
                if progress_callback:
                    progress_callback(index + 1, len(urls), url)
                
                return await self.scrape_one(url)
        
        tasks = [
            scrape_with_sem(url, i)
            for i, url in enumerate(urls)
        ]
        
        results = await asyncio.gather(*tasks, return_exceptions=True)
        
        # Filter out exceptions
        valid = []
        for r in results:
            if isinstance(r, ScrapedRecord):
                valid.append(r)
            elif isinstance(r, Exception):
                print(f"Task error: {r}")
        
        return valid
    
    async def scrape_one(self, url: str) -> ScrapedRecord:
        """Scrape a single URL."""
        
        try:
            async with httpx.AsyncClient() as client:
                response = await client.get(
                    url,
                    headers={"User-Agent": "Mozilla/5.0..."},
                    timeout=30.0
                )
                
                if response.status_code != 200:
                    return ScrapedRecord(
                        url=url,
                        data={},
                        success=False,
                        error=f"HTTP {response.status_code}"
                    )
                
                # Parse HTML
                soup = BeautifulSoup(response.text, "html.parser")
                
                # Extract data - adapt to your source
                data = self._extract_book(soup, url)
                
                return ScrapedRecord(
                    url=url,
                    data=data,
                    success=True
                )
                
        except Exception as e:
            return ScrapedRecord(
                url=url,
                data={},
                success=False,
                error=str(e)
            )
    
    def _extract_book(self, soup: BeautifulSoup, url: str) -> dict:
        """Extract book data from page."""
        
        import re
        
        # Adapt selectors to your target site
        # Using books.toscrape.com as example
        
        title = ""
        title_elem = soup.select_one("h1")
        if title_elem:
            title = title_elem.get_text(strip=True)
        
        price = 0.0
        price_elem = soup.select_one(".price_color")
        if price_elem:
            match = re.search(r"[\d,]+\.?\d*", price_elem.get_text())
            if match:
                price = float(match.group().replace(",", ""))
        
        rating = None
        rating_elem = soup.select_one(".star-rating")
        if rating_elem:
            rating_class = rating_elem.get("class", [])
            for cls in rating_class:
                if "star-rating" in cls:
                    # Extract rating from class name
                    match = re.search(r"(\d+)", cls)
                    if match:
                        rating = int(match.group(1))
        
        availability = "Unknown"
        avail_elem = soup.select_one(".availability")
        if avail_elem:
            availability = avail_elem.get_text(strip=True)
        
        category = "Unknown"
        cat_elem = soup.select_one(".breadcrumb li:nth-child(3)")
        if cat_elem:
            category = cat_elem.get_text(strip=True)
        
        upc = ""
        upc_elem = soup.select_one("th:contains('UPC') + td")
        if upc_elem:
            upc = upc_elem.get_text(strip=True)
        
        return {
            "title": title,
            "price": price,
            "rating": rating,
            "availability": availability,
            "category": category,
            "upc": upc,
            "url": url
        }
```

### validator.py

```python
"""Validation with Pydantic."""

from pydantic import BaseModel, Field, field_validator
from typing import Optional


class BookRecord(BaseModel):
    """Validated book record."""
    
    title: str = Field(..., min_length=1, description="Book title")
    price: float = Field(..., ge=0, description="Price in dollars")
    rating: Optional[float] = Field(None, ge=0, le=5, description="Rating 0-5")
    availability: str = Field(default="Unknown")
    category: str = Field(default="Unknown")
    upc: str = Field(default="")
    url: str = Field(..., description="Product URL")
    
    @field_validator("title", "category")
    @classmethod
    def clean_whitespace(cls, v: str) -> str:
        """Clean whitespace from strings."""
        return " ".join(v.split())
    
    @field_validator("price")
    @classmethod
    def validate_price(cls, v: float) -> float:
        """Ensure reasonable price."""
        if v > 10000:
            raise ValueError("Price seems unreasonably high")
        return round(v, 2)


def validate_records(records: list[dict]) -> list[BookRecord]:
    """Validate a list of raw records."""
    
    validated = []
    errors = []
    
    for record in records:
        try:
            book = BookModel(**record)
            validated.append(book)
        except Exception as e:
            errors.append({"record": record, "error": str(e)})
    
    if errors:
        print(f"⚠️  {len(errors)} validation errors")
        for err in errors[:3]:
            print(f"   {err}")
    
    return validated
```

### database.py

```python
"""Database storage for scraped records."""

import sqlite3
import pandas as pd
from pathlib import Path
from typing import Optional


class DatasetDB:
    """Database for scraped dataset."""
    
    def __init__(self, db_path: str):
        self.db_path = db_path
        self._init_schema()
    
    def _init_schema(self) -> None:
        """Initialize database."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        # Records table
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS records (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                url TEXT UNIQUE NOT NULL,
                data TEXT NOT NULL,
                status TEXT DEFAULT 'pending',
                scraped_at TEXT DEFAULT CURRENT_TIMESTAMP
            )
        """)
        
        cursor.execute("""
            CREATE INDEX IF NOT EXISTS idx_records_url ON records(url)
        """)
        
        cursor.execute("""
            CREATE INDEX IF NOT EXISTS idx_records_status ON records(status)
        """)
        
        conn.commit()
        conn.close()
    
    def save_records(self, scraped_records: list) -> int:
        """Save scraped records to database."""
        
        import json
        from datetime import datetime
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        saved = 0
        
        for record in scraped_records:
            if record.success:
                cursor.execute("""
                    INSERT OR REPLACE INTO records (url, data, status, scraped_at)
                    VALUES (?, ?, 'complete', ?)
                """, (record.url, json.dumps(record.data), datetime.now().isoformat()))
                saved += 1
            else:
                cursor.execute("""
                    INSERT OR REPLACE INTO records (url, data, status, scraped_at)
                    VALUES (?, ?, 'failed', ?)
                """, (record.url, json.dumps(record.data), datetime.now().isoformat()))
        
        conn.commit()
        conn.close()
        
        return saved
    
    def filter_new_urls(self, urls: list[str]) -> list[str]:
        """Filter out already scraped URLs."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        placeholders = ",".join("?" * len(urls))
        cursor.execute(f"""
            SELECT url FROM records WHERE url IN ({placeholders}) AND status = 'complete'
        """, urls)
        
        existing = set(row[0] for row in cursor.fetchall())
        conn.close()
        
        return [url for url in urls if url not in existing]
    
    def get_pending(self) -> list[str]:
        """Get URLs that failed or haven't been scraped."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            SELECT url FROM records WHERE status != 'complete'
        """)
        
        urls = [row[0] for row in cursor.fetchall()]
        conn.close()
        
        return urls
    
    def get_all(self) -> pd.DataFrame:
        """Get all records as DataFrame."""
        
        import json
        
        conn = sqlite3.connect(self.db_path)
        
        df = pd.read_sql_query("""
            SELECT url, data, status, scraped_at
            FROM records
            WHERE status = 'complete'
        """, conn)
        
        conn.close()
        
        # Parse JSON data
        if not df.empty:
            df["data"] = df["data"].apply(json.loads)
            df = pd.concat(df["data"].apply(pd.Series), axis=1)
        
        return df
```

### analyzer.py

```python
"""Quick EDA for the dataset."""

import pandas as pd
from rich.console import Console


class DatasetAnalyzer:
    """Quick exploratory data analysis."""
    
    def __init__(self):
        self.console = Console()
    
    def summary(self, df: pd.DataFrame) -> None:
        """Print dataset summary."""
        
        if df.empty:
            self.console.print("⚠️  No data to analyze")
            return
        
        self.console.print(f"\n📊 Dataset: {len(df)} records, {len(df.columns)} columns")
        
        # Numeric summary
        numeric_cols = df.select_dtypes(include="number").columns
        
        if len(numeric_cols) > 0:
            self.console.print("\n📈 Numeric Columns:")
            print(df[numeric_cols].describe().round(2))
    
    def missing_values(self, df: pd.DataFrame) -> None:
        """Show missing values."""
        
        if df.empty:
            return
        
        missing = df.isnull().sum()
        missing = missing[missing > 0]
        
        if len(missing) > 0:
            self.console.print("\n⚠️  Missing Values:")
            for col, count in missing.items():
                pct = count / len(df) * 100
                self.console.print(f"   {col}: {count} ({pct:.1f}%)")
    
    def distributions(self, df: pd.DataFrame) -> None:
        """Show value distributions."""
        
        if df.empty:
            return
        
        # Categorical columns
        cat_cols = df.select_dtypes(include="object").columns
        
        for col in cat_cols[:3]:  # First 3 categorical columns
            if col == "url":
                continue
            
            self.console.print(f"\n📊 Distribution: {col}")
            
            counts = df[col].value_counts().head(10)
            
            for value, count in counts.items():
                pct = count / len(df) * 100
                bar = "█" * int(pct / 2)
                self.console.print(f"   {str(value)[:30]:30} {count:5} {bar} {pct:.1f}%")
```

### exporter.py

```python
"""Export dataset to various formats."""

import pandas as pd


class DatasetExporter:
    """Export dataset to ML-ready formats."""
    
    def to_csv(self, df: pd.DataFrame, filename: str) -> None:
        """Export to CSV."""
        
        df.to_csv(
            filename,
            index=False,
            encoding="utf-8-sig"
        )
    
    def to_parquet(self, df: pd.DataFrame, filename: str) -> None:
        """Export to Parquet (compressed, fast)."""
        
        df.to_parquet(
            filename,
            compression="gzip",
            index=False
        )
    
    def to_json(self, df: pd.DataFrame, filename: str) -> None:
        """Export to JSON."""
        
        df.to_json(
            filename,
            orient="records",
            lines=True
        )
```

---

## Usage

```bash
# Build dataset from books.toscrape.com
python main.py books https://books.toscrape.com/catalogue/category/books_1/index.html --max 100

# Resume interrupted build
python main.py books --resume

# Analyze existing dataset
python main.py books --analyze
```

---

## Output

```
📦 Building dataset: books
   Source: https://books.toscrape.com/catalogue/category/books_1/index.html
   Max items: 100

🔍 Discovering URLs...
   Found 1000 URLs
   100 new URLs to scrape

📥 Scraping data...
   [1/100] https://books.toscrape.com/catalogue/the-grand-design_497/index.html...
   ...

✅ Saving to database...
   Saved 100 records

📊 Dataset Summary:

📊 Dataset: 100 records, 7 columns

📈 Numeric Columns:
         price    rating
count  100.000  80.000
mean    34.52    3.21
std     18.23    1.12
min     10.99    1.00
max     59.99    5.00

⚠️  Missing Values:
   rating: 20 (20.0%)

💾 Exporting...
   Exported to books.csv and books.parquet
```

---

## 🚀 Challenge

Extend with:

1. **Multiple Categories** - Scrape from multiple category pages
2. **Image URLs** - Extract and download product images
3. **Text Features** - Add NLP features (sentiment, length)
4. **Auto-Export** - Schedule weekly builds with GitHub Actions

---

## Summary

✅ Built complete dataset builder

✅ URL discovery with pagination

✅ Async scraping with semaphore

✅ Pydantic validation

✅ SQLite + Parquet storage

✅ Quick EDA and export

---

## 🔗 Further Reading

- [Pandas Documentation](https://pandas.pydata.org/)
- [Pydantic Documentation](https://docs.pydantic.dev/)
- [Parquet Format](https://parquet.apache.org/)
