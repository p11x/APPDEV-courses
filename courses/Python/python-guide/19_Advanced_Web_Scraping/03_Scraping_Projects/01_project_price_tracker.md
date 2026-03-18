# 📦 Project: Price Tracker

> Track product prices over time and alert when they drop.

## 🎯 What You'll Build

- A CLI tool to track product prices from any website
- Store price history in SQLite
- Detect and alert on price drops
- Rich CLI output with price trends
- Scheduling support for automatic tracking

## 📦 Prerequisites

- Completion of [02_Data_Pipelines/03_scheduling_and_monitoring_pipelines.md](../../19_Advanced_Web_Scraping/02_Data_Pipelines/03_scheduling_and_monitoring_pipelines.md)
- Understanding of SQLite databases
- Basic knowledge of httpx and BeautifulSoup

---

## 🛠️ Setup

```bash
pip install httpx beautifulsoup4 rich schedule
```

---

## Project Structure

```
price_tracker/
├── main.py          # Entry point and CLI
├── scraper.py       # Web scraping logic
├── database.py      # SQLite database operations
├── notifier.py      # Alert system
└── config.py       # Configuration
```

---

## Full Working Source Code

### config.py - Configuration

```python
"""Configuration for price tracker."""

from pathlib import Path


# Database configuration
DB_PATH = Path("price_tracker.db")

# Scraping configuration
REQUEST_TIMEOUT = 30.0  # seconds
USER_AGENT = (
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
    "AppleWebKit/537.36 (KHTML, like Gecko) "
    "Chrome/120.0.0.0 Safari/537.36"
)

# Alert configuration
PRICE_DROP_THRESHOLD = 5.0  # Alert if price drops by this percentage
```

### database.py - Database Operations

```python
"""Database operations for price tracking."""

import sqlite3
from datetime import datetime
from pathlib import Path
from typing import Optional
from dataclasses import dataclass

import config


@dataclass
class Product:
    """Represents a tracked product."""
    url: str
    name: str
    current_price: Optional[float]
    previous_price: Optional[float]
    last_updated: str


class PriceDatabase:
    """Database for tracking product prices."""
    
    def __init__(self, db_path: Path = config.DB_PATH):
        self.db_path = db_path
        self._init_schema()
    
    def _init_schema(self) -> None:
        """Initialize database tables."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        # Products table
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS products (
                url TEXT PRIMARY KEY,
                name TEXT NOT NULL,
                current_price REAL,
                previous_price REAL,
                last_updated TEXT NOT NULL
            )
        """)
        
        # Price history table
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS price_history (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                url TEXT NOT NULL,
                price REAL NOT NULL,
                recorded_at TEXT DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (url) REFERENCES products(url)
            )
        """)
        
        # Index for fast lookups
        cursor.execute("""
            CREATE INDEX IF NOT EXISTS idx_history_url 
            ON price_history(url, recorded_at)
        """)
        
        conn.commit()
        conn.close()
    
    def add_product(self, url: str, name: str, price: float) -> None:
        """Add a new product to track."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        now = datetime.now().isoformat()
        
        cursor.execute("""
            INSERT INTO products (url, name, current_price, previous_price, last_updated)
            VALUES (?, ?, ?, NULL, ?)
            ON CONFLICT(url) DO UPDATE SET
                name = excluded.name,
                current_price = excluded.current_price,
                last_updated = excluded.last_updated
        """, (url, name, price, now))
        
        # Record price in history
        cursor.execute("""
            INSERT INTO price_history (url, price)
            VALUES (?, ?)
        """, (url, price))
        
        conn.commit()
        conn.close()
    
    def update_price(self, url: str, price: float) -> Optional[float]:
        """Update price for a product. Returns previous price if changed."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        # Get current price
        cursor.execute("SELECT current_price FROM products WHERE url = ?", (url,))
        row = cursor.fetchone()
        
        previous_price = row[0] if row else None
        
        now = datetime.now().isoformat()
        
        # Update product
        cursor.execute("""
            UPDATE products
            SET previous_price = current_price,
                current_price = ?,
                last_updated = ?
            WHERE url = ?
        """, (price, now, url))
        
        # Record in history
        cursor.execute("""
            INSERT INTO price_history (url, price)
            VALUES (?, ?)
        """, (url, price))
        
        conn.commit()
        conn.close()
        
        return previous_price
    
    def get_all_products(self) -> list[Product]:
        """Get all tracked products."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            SELECT url, name, current_price, previous_price, last_updated
            FROM products
            ORDER BY name
        """)
        
        rows = cursor.fetchall()
        conn.close()
        
        return [
            Product(
                url=row[0],
                name=row[1],
                current_price=row[2],
                previous_price=row[3],
                last_updated=row[4]
            )
            for row in rows
        ]
    
    def get_price_history(self, url: str, days: int = 30) -> list[dict]:
        """Get price history for a product."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            SELECT price, recorded_at
            FROM price_history
            WHERE url = ?
              AND recorded_at >= datetime('now', '-' || ? || ' days')
            ORDER BY recorded_at ASC
        """, (url, days))
        
        rows = cursor.fetchall()
        conn.close()
        
        return [
            {"price": row[0], "recorded_at": row[1]}
            for row in rows
        ]
    
    def remove_product(self, url: str) -> bool:
        """Remove a product from tracking."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("DELETE FROM products WHERE url = ?", (url,))
        
        deleted = cursor.rowcount > 0
        
        conn.commit()
        conn.close()
        
        return deleted
```

### scraper.py - Web Scraping

```python
"""Web scraping for price tracking."""

import httpx
from bs4 import BeautifulSoup
from dataclasses import dataclass

import config


@dataclass
class ScrapedProduct:
    """Result of scraping a product page."""
    name: str
    price: float
    in_stock: bool


def scrape_product(url: str) -> ScrapedProduct:
    """Scrape product information from a URL."""
    
    headers = {
        "User-Agent": config.USER_AGENT,
        "Accept-Language": "en-US,en;q=0.9",
    }
    
    response = httpx.get(url, headers=headers, timeout=config.REQUEST_TIMEOUT)
    response.raise_for_status()
    
    soup = BeautifulSoup(response.text, "html.parser")
    
    # Extract product name - adapt to your target site
    # This uses common patterns, may need adjustment
    name = _extract_name(soup)
    
    # Extract price
    price = _extract_price(soup)
    
    # Check stock status
    in_stock = _check_stock(soup)
    
    return ScrapedProduct(
        name=name,
        price=price,
        in_stock=in_stock
    )


def _extract_name(soup: BeautifulSoup) -> str:
    """Extract product name from page."""
    
    # Try multiple selectors
    selectors = [
        "h1.product-title",
        "h1.product-name",
        "h1[itemprop='name']",
        ".product-header h1",
        "h1",
    ]
    
    for selector in selectors:
        element = soup.select_one(selector)
        if element:
            return element.get_text(strip=True)
    
    # Fallback: page title
    title = soup.title
    return title.get_text(strip=True) if title else "Unknown Product"


def _extract_price(soup: BeautifulSoup) -> float:
    """Extract price from page."""
    
    import re
    
    # Try multiple selectors
    selectors = [
        "[itemprop='price']",
        ".product-price",
        ".price",
        ".current-price",
        "[data-price]",
    ]
    
    for selector in selectors:
        element = soup.select_one(selector)
        if element:
            # Get price from content or data attribute
            price_text = element.get("content", element.get_text())
            
            # Extract numeric value
            match = re.search(r"[\d,]+\.?\d*", price_text)
            if match:
                return float(match.group().replace(",", ""))
    
    raise ValueError("Could not find price on page")


def _check_stock(soup: BeautifulSoup) -> bool:
    """Check if product is in stock."""
    
    # Look for common out-of-stock indicators
    out_of_stock_texts = [
        "out of stock",
        "unavailable",
        "sold out",
        "currently unavailable",
    ]
    
    page_text = soup.get_text().lower()
    
    for text in out_of_stock_texts:
        if text in page_text:
            return False
    
    return True
```

### notifier.py - Notification System

```python
"""Notification system for price alerts."""

import httpx
from dataclasses import dataclass


@dataclass
class PriceAlert:
    """Represents a price change alert."""
    product_name: str
    url: str
    old_price: float
    new_price: float
    change_percent: float


class TelegramNotifier:
    """Send notifications via Telegram."""
    
    def __init__(self, bot_token: str, chat_id: str):
        self.bot_token = bot_token
        self.chat_id = chat_id
    
    def send(self, message: str) -> None:
        """Send a message to Telegram."""
        
        url = f"https://api.telegram.org/bot{self.bot_token}/sendMessage"
        
        data = {
            "chat_id": self.chat_id,
            "text": message,
            "parse_mode": "HTML"
        }
        
        httpx.post(url, data=data)
    
    def send_price_alert(self, alert: PriceAlert) -> None:
        """Send a price change alert."""
        
        emoji = "🔴" if alert.change_percent > 0 else "🟢"
        
        message = f"{emoji} <b>Price Alert</b>\n\n"
        message += f"Product: {alert.product_name}\n"
        
        if alert.change_percent < 0:
            message += f"Price dropped: ${alert.old_price:.2f} → <b>${alert.new_price:.2f}</b>\n"
            message += f" Savings: <b>{abs(alert.change_percent):.1f}%</b>\n"
        else:
            message += f"Price increased: ${alert.old_price:.2f} → ${alert.new_price:.2f}\n"
        
        message += f"\n<a href='{alert.url}'>View Product</a>"
        
        self.send(message)
    
    def send_summary(self, alerts: list[PriceAlert]) -> None:
        """Send a summary of all price changes."""
        
        if not alerts:
            return
        
        drops = [a for a in alerts if a.change_percent < 0]
        increases = [a for a in alerts if a.change_percent > 0]
        
        message = "📊 <b>Daily Price Summary</b>\n\n"
        
        if drops:
            message += f"🔽 <b>Price Drops ({len(drops)})</b>\n"
            for alert in drops[:5]:  # Show top 5
                pct = abs(alert.change_percent)
                message += f"• {alert.product_name[:30]}: {pct:.1f}%\n"
        
        if increases:
            message += f"\n🔼 <b>Price Increases ({len(increases)})</b>\n"
            for alert in increases[:5]:
                message += f"• {alert.product_name[:30]}: +{alert.change_percent:.1f}%\n"
        
        self.send(message)


class ConsoleNotifier:
    """Print notifications to console."""
    
    def send_price_alert(self, alert: PriceAlert) -> None:
        """Print price alert to console."""
        
        emoji = "🔴" if alert.change_percent > 0 else "🟢"
        
        print(f"\n{emoji} Price Alert: {alert.product_name}")
        print(f"   {alert.old_price:.2f} → {alert.new_price:.2f} ({alert.change_percent:+.1f}%)")
        print(f"   {alert.url}")
    
    def send_summary(self, alerts: list[PriceAlert]) -> None:
        """Print summary to console."""
        
        if not alerts:
            print("No price changes detected.")
            return
        
        drops = [a for a in alerts if a.change_percent < 0]
        
        print(f"\n📊 Price Summary: {len(drops)} drops detected")
        for alert in drops:
            pct = abs(alert.change_percent)
            print(f"  🟢 {alert.product_name[:40]}: {pct:.1f}% off (${alert.new_price:.2f})")
```

### main.py - Main CLI

```python
"""Price Tracker CLI - Main entry point."""

import argparse
import sys
from pathlib import Path
from datetime import datetime
import schedule
import time
import threading

import config
from database import PriceDatabase, Product
from scraper import scrape_product, ScrapedProduct
from notifier import TelegramNotifier, ConsoleNotifier, PriceAlert


class PriceTracker:
    """Main price tracker application."""
    
    def __init__(
        self,
        db_path: Path = config.DB_PATH,
        notifier: ConsoleNotifier = None,
        alert_threshold: float = config.PRICE_DROP_THRESHOLD
    ):
        self.db = PriceDatabase(db_path)
        self.notifier = notifier or ConsoleNotifier()
        self.alert_threshold = alert_threshold
    
    def add_product(self, url: str) -> None:
        """Add a product to track."""
        
        print(f"Adding product: {url}")
        
        try:
            # Scrape product
            product = scrape_product(url)
            
            # Save to database
            self.db.add_product(url, product.name, product.price)
            
            print(f"✅ Added: {product.name} - ${product.price:.2f}")
            
        except Exception as e:
            print(f"❌ Failed to add product: {e}")
            raise
    
    def update_all(self) -> list[PriceAlert]:
        """Update all tracked products and return alerts."""
        
        products = self.db.get_all_products()
        alerts = []
        
        print(f"\nUpdating {len(products)} products...")
        
        for product in products:
            try:
                # Scrape current price
                scraped = scrape_product(product.url)
                
                # Update in database
                previous = self.db.update_price(product.url, scraped.price)
                
                # Check for price change
                if previous and previous != scraped.price:
                    change_pct = ((scraped.price - previous) / previous) * 100
                    
                    alert = PriceAlert(
                        product_name=product.name,
                        url=product.url,
                        old_price=previous,
                        new_price=scraped.price,
                        change_percent=change_pct
                    )
                    
                    alerts.append(alert)
                    
                    # Send alert
                    self.notifier.send_price_alert(alert)
                    
                    status = "🔽" if change_pct < 0 else "🔼"
                    print(f"  {status} {product.name}: ${previous:.2f} → ${scraped.price:.2f}")
                else:
                    print(f"  ✓ {product.name}: ${scraped.price:.2f} (no change)")
                    
            except Exception as e:
                print(f"  ❌ Failed to update {product.name}: {e}")
        
        # Send summary
        if alerts:
            self.notifier.send_summary(alerts)
        
        return alerts
    
    def list_products(self) -> None:
        """List all tracked products."""
        
        products = self.db.get_all_products()
        
        if not products:
            print("No products tracked. Add some with: add <url>")
            return
        
        print("\n📦 Tracked Products:")
        print("=" * 60)
        
        for p in products:
            # Get trend indicator
            if p.current_price and p.previous_price:
                if p.current_price < p.previous_price:
                    trend = "📉"
                elif p.current_price > p.previous_price:
                    trend = "📈"
                else:
                    trend = "➡️"
            else:
                trend = "❓"
            
            price_str = f"${p.current_price:.2f}" if p.current_price else "N/A"
            
            print(f"{trend} {p.name}")
            print(f"   Price: {price_str} | Updated: {p.last_updated[:10]}")
            print(f"   URL: {p.url}")
            print()
    
    def show_history(self, url: str, days: int = 30) -> None:
        """Show price history for a product."""
        
        history = self.db.get_price_history(url, days)
        
        if not history:
            print("No price history available.")
            return
        
        print(f"\n📈 Price History (last {days} days):")
        print("-" * 40)
        
        for entry in history:
            date = entry["recorded_at"][:10]
            price = entry["price"]
            print(f"  {date}: ${price:.2f}")


def run_scheduler(tracker: PriceTracker, interval_hours: int = 6):
    """Run the tracker on a schedule."""
    
    # Initial run
    tracker.update_all()
    
    # Schedule regular updates
    schedule.every(interval_hours).hours.do(tracker.update_all)
    
    print(f"\n⏰ Scheduler started - updating every {interval_hours} hours")
    
    while True:
        schedule.run_pending()
        time.sleep(60)


def main():
    """Main CLI entry point."""
    
    parser = argparse.ArgumentParser(description="Price Tracker CLI")
    subparsers = parser.add_subparsers(dest="command", help="Commands")
    
    # Add product command
    add_parser = subparsers.add_parser("add", help="Add a product to track")
    add_parser.add_argument("url", help="Product URL")
    
    # List command
    subparsers.add_parser("list", help="List all tracked products")
    
    # Update command
    subparsers.add_parser("update", help="Update all product prices")
    
    # History command
    history_parser = subparsers.add_parser("history", help="Show price history")
    history_parser.add_argument("url", help="Product URL")
    history_parser.add_argument("--days", type=int, default=30, help="Days of history")
    
    # Watch command (continuous)
    watch_parser = subparsers.add_parser("watch", help="Run continuously")
    watch_parser.add_argument("--hours", type=int, default=6, help="Update interval")
    
    args = parser.parse_args()
    
    # Create tracker
    tracker = PriceTracker()
    
    # Handle commands
    if args.command == "add":
        tracker.add_product(args.url)
    
    elif args.command == "list":
        tracker.list_products()
    
    elif args.command == "update":
        tracker.update_all()
    
    elif args.command == "history":
        tracker.show_history(args.url, args.days)
    
    elif args.command == "watch":
        run_scheduler(tracker, args.hours)
    
    else:
        parser.print_help()


if __name__ == "__main__":
    main()
```

---

## Usage

### Add a Product

```bash
python main.py add https://books.toscrape.com/catalogue/the-grand-design_497/index.html
```

### List Products

```bash
python main.py list
```

### Update Prices

```bash
python main.py update
```

### Watch Mode (Continuous)

```bash
python main.py watch --hours 6
```

### Output Example

```
📦 Tracked Products:
============================================================
➡️ The Grand Design
   Price: $19.89 | Updated: 2024-01-15
   URL: https://books.toscrape.com/catalogue/the-grand-design_497/index.html
```

---

## 🚀 Challenge

Extend the price tracker with these features:

1. **Plotly Chart** - Add a price history chart
2. **Multiple Products** - Track from multiple sites
3. **Email Alerts** - Add email notification support
4. **Web Dashboard** - Create a simple Flask/FastAPI dashboard

---

## Summary

✅ Built a complete CLI price tracker

✅ SQLite database for persistent storage

✅ Rich console output with trends

✅ Scheduled automatic updates

✅ Alert system for price drops

---

## 🔗 Further Reading

- [BeautifulSoup Documentation](https://www.crummy.com/software/BeautifulSoup/bs4/doc/)
- [httpx Documentation](https://www.python-httpx.org/)
- [Rich Library](https://rich.readthedocs.io/)
