# 📰 Project: News Aggregator

> Aggregate headlines from multiple sources into one clean feed.

## 🎯 What You'll Build

- Fetch news from RSS feeds and APIs
- Deduplicate stories by title similarity
- Categorize stories using AI
- Rich terminal output grouped by category
- Export daily digest to Markdown

## 📦 Prerequisites

- Completion of [01_project_price_tracker.md](./01_project_price_tracker.md)
- Understanding of asyncio for concurrent fetching
- Basic knowledge of RSS and APIs

---

## 🛠️ Setup

```bash
pip install httpx feedparser rich pandas difflib
```

---

## Project Structure

```
news_aggregator/
├── main.py          # Entry point
├── fetcher.py       # Fetch from RSS/API
├── deduplicator.py  # Remove duplicates
├── categorizer.py   # AI categorization
├── database.py      # SQLite storage
└── formatters.py    # Output formatting
```

---

## Full Working Source Code

### database.py - Database

```python
"""Database for storing news articles."""

import sqlite3
from datetime import datetime
from dataclasses import dataclass
from pathlib import Path


@dataclass
class Article:
    """Represents a news article."""
    title: str
    url: str
    source: str
    category: str
    published_at: str
    summary: str = ""


class NewsDatabase:
    """Database for news articles."""
    
    def __init__(self, db_path: str = "news.db"):
        self.db_path = db_path
        self._init_schema()
    
    def _init_schema(self) -> None:
        """Initialize database."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            CREATE TABLE IF NOT EXISTS articles (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                title TEXT NOT NULL,
                url TEXT UNIQUE NOT NULL,
                source TEXT NOT NULL,
                category TEXT,
                published_at TEXT,
                summary TEXT,
                fetched_at TEXT DEFAULT CURRENT_TIMESTAMP
            )
        """)
        
        # Index for fast lookups
        cursor.execute("""
            CREATE INDEX IF NOT EXISTS idx_articles_url ON articles(url)
        """)
        
        cursor.execute("""
            CREATE INDEX IF NOT EXISTS idx_articles_category ON articles(category)
        """)
        
        conn.commit()
        conn.close()
    
    def add_article(self, article: Article) -> bool:
        """Add an article. Returns True if added, False if duplicate."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        try:
            cursor.execute("""
                INSERT INTO articles (title, url, source, category, published_at, summary)
                VALUES (?, ?, ?, ?, ?, ?)
            """, (
                article.title,
                article.url,
                article.source,
                article.category,
                article.published_at,
                article.summary
            ))
            
            conn.commit()
            added = True
            
        except sqlite3.IntegrityError:
            # Duplicate URL
            added = False
        
        conn.close()
        return added
    
    def get_recent_articles(self, days: int = 1, limit: int = 100) -> list[Article]:
        """Get recent articles."""
        
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute("""
            SELECT title, url, source, category, published_at, summary
            FROM articles
            WHERE fetched_at >= datetime('now', '-' || ? || ' days')
            ORDER BY published_at DESC
            LIMIT ?
        """, (days, limit))
        
        rows = cursor.fetchall()
        conn.close()
        
        return [
            Article(
                title=row[0],
                url=row[1],
                source=row[2],
                category=row[3] or "Uncategorized",
                published_at=row[4] or "",
                summary=row[5] or ""
            )
            for row in rows
        ]
    
    def get_by_category(self, days: int = 1) -> dict[str, list[Article]]:
        """Get articles grouped by category."""
        
        articles = self.get_recent_articles(days)
        
        by_category = {}
        for article in articles:
            cat = article.category or "Uncategorized"
            if cat not in by_category:
                by_category[cat] = []
            by_category[cat].append(article)
        
        return by_category
```

### fetcher.py - News Fetching

```python
"""Fetch news from various sources."""

import asyncio
import httpx
import feedparser
from dataclasses import dataclass
from typing import Optional


@dataclass
class NewsItem:
    """A single news item from any source."""
    title: str
    url: str
    source: str
    published_at: str
    summary: str


class RSSFetcher:
    """Fetch news from RSS feeds."""
    
    FEEDS = {
        "Hacker News": "https://hnrss.org/frontpage",
        "TechCrunch": "https://techcrunch.com/feed/",
        "BBC World": "http://feeds.bbci.co.uk/news/world/rss.xml",
    }
    
    async def fetch_all(self) -> list[NewsItem]:
        """Fetch from all RSS feeds concurrently."""
        
        async with httpx.AsyncClient() as client:
            tasks = [
                self._fetch_feed(client, name, url)
                for name, url in self.FEEDS.items()
            ]
            
            results = await asyncio.gather(*tasks, return_exceptions=True)
            
            items = []
            for result in results:
                if isinstance(result, list):
                    items.extend(result)
            
            return items
    
    async def _fetch_feed(
        self, 
        client: httpx.AsyncClient, 
        source: str, 
        url: str
    ) -> list[NewsItem]:
        """Fetch a single RSS feed."""
        
        try:
            response = await client.get(url, timeout=10.0)
            response.raise_for_status()
            
            feed = feedparser.parse(response.text)
            
            items = []
            for entry in feed.entries[:10]:  # Top 10 from each source
                items.append(NewsItem(
                    title=entry.get("title", "No title"),
                    url=entry.get("link", ""),
                    source=source,
                    published_at=entry.get("published", ""),
                    summary=entry.get("summary", "")[:200]
                ))
            
            return items
            
        except Exception as e:
            print(f"Error fetching {source}: {e}")
            return []


class APIfetcher:
    """Fetch news from APIs."""
    
    async def fetch_hackernews_topstories(self) -> list[NewsItem]:
        """Fetch top stories from Hacker News API."""
        
        async with httpx.AsyncClient() as client:
            # Get story IDs
            response = await client.get(
                "https://hacker-news.firebaseio.com/v0/topstories.json",
                timeout=10.0
            )
            story_ids = response.json()[:30]  # Top 30
            
            # Fetch story details concurrently
            tasks = [
                self._fetch_story(client, story_id)
                for story_id in story_ids
            ]
            
            results = await asyncio.gather(*tasks, return_exceptions=True)
            
            return [r for r in results if isinstance(r, NewsItem)]
    
    async def _fetch_story(
        self, 
        client: httpx.AsyncClient, 
        story_id: int
    ) -> Optional[NewsItem]:
        """Fetch a single Hacker News story."""
        
        try:
            response = await client.get(
                f"https://hacker-news.firebaseio.com/v0/item/{story_id}.json",
                timeout=5.0
            )
            story = response.json()
            
            if story:
                return NewsItem(
                    title=story.get("title", "No title"),
                    url=story.get("url", f"https://news.ycombinator.com/item?id={story_id}"),
                    source="Hacker News",
                    published_at="",
                    summary=f"{story.get('score', 0)} points | {story.get('descendants', 0)} comments"
                )
        except Exception:
            pass
        
        return None


class NewsFetcher:
    """Combined fetcher for all sources."""
    
    def __init__(self):
        self.rss = RSSFetcher()
        self.api = APIfetcher()
    
    async def fetch_all(self) -> list[NewsItem]:
        """Fetch from all sources concurrently."""
        
        rss_task = self.rss.fetch_all()
        api_task = self.api.fetch_hackernews_topstories()
        
        rss_items, api_items = await asyncio.gather(rss_task, api_task)
        
        return rss_items + api_items
```

### deduplicator.py - Deduplication

```python
"""Deduplicate news articles by title similarity."""

import difflib
from dataclasses import dataclass


@dataclass
class DedupeResult:
    """Result of deduplication."""
    unique: list
    duplicates: int


def deduplicate_articles(articles: list, threshold: float = 0.7) -> DedupeResult:
    """
    Remove duplicate articles based on title similarity.
    
    Args:
        articles: List of articles to deduplicate
        threshold: Similarity threshold (0-1). Higher = stricter.
    
    Returns:
        DedupeResult with unique articles and duplicate count
    """
    
    unique = []
    seen_titles = []
    duplicates = 0
    
    for article in articles:
        title_lower = article.title.lower()
        
        # Check against all seen titles
        is_duplicate = False
        for seen in seen_titles:
            # Calculate similarity
            similarity = difflib.SequenceMatcher(
                None, 
                title_lower, 
                seen
            ).ratio()
            
            if similarity >= threshold:
                is_duplicate = True
                duplicates += 1
                break
        
        if not is_duplicate:
            unique.append(article)
            seen_titles.append(title_lower)
    
    return DedupeResult(
        unique=unique,
        duplicates=duplicates
    )


def normalize_title(title: str) -> str:
    """Normalize title for better comparison."""
    
    import re
    
    # Lowercase
    title = title.lower()
    
    # Remove punctuation
    title = re.sub(r'[^\w\s]', '', title)
    
    # Remove extra whitespace
    title = ' '.join(title.split())
    
    return title
```

### categorizer.py - AI Categorization

```python
"""Categorize articles using AI."""

import os
import json
import httpx


class CategoryClassifier:
    """Classify articles into categories using Claude API."""
    
    CATEGORIES = [
        "Tech", "Business", "Science", "Politics", 
        "World", "Entertainment", "Health", "Sports"
    ]
    
    def __init__(self, api_key: str | None = None):
        self.api_key = api_key or os.getenv("ANTHROPIC_API_KEY")
    
    def categorize(self, titles: list[str]) -> list[str]:
        """Categorize a batch of titles."""
        
        if not self.api_key or not titles:
            # Return default category if no API key
            return ["Uncategorized"] * len(titles)
        
        # Batch titles (max 20 per request)
        categories = []
        
        for i in range(0, len(titles), 20):
            batch = titles[i:i+20]
            batch_cats = self._categorize_batch(batch)
            categories.extend(batch_cats)
        
        return categories
    
    def _categorize_batch(self, titles: list[str]) -> list[str]:
        """Categorize a batch of titles with AI."""
        
        prompt = self._build_prompt(titles)
        
        try:
            response = httpx.post(
                "https://api.anthropic.com/v1/messages",
                headers={
                    "x-api-key": self.api_key,
                    "anthropic-version": "2023-06-01",
                    "content-type": "application/json"
                },
                json={
                    "model": "claude-3-haiku-20240307",
                    "max_tokens": 100,
                    "messages": [{"role": "user", "content": prompt}]
                },
                timeout=30.0
            )
            
            result = response.json()
            content = result.get("content", [{}])[0].get("text", "")
            
            # Parse categories from response
            categories = self._parse_categories(content, len(titles))
            
            return categories
            
        except Exception as e:
            print(f"Categorization error: {e}")
            return ["Uncategorized"] * len(titles)
    
    def _build_prompt(self, titles: list[str]) -> str:
        """Build prompt for categorization."""
        
        titles_list = "\n".join(f"- {t}" for t in titles)
        
        prompt = f"""Categorize each headline into ONE of these categories:
{', '.join(self.CATEGORIES)}

Headlines:
{titles_list}

Respond with a JSON list of categories, one per line matching each headline in order.
Example: ["Tech", "Business", "Tech"]

Categories:"""
        
        return prompt
    
    def _parse_categories(self, content: str, expected: int) -> list[str]:
        """Parse categories from AI response."""
        
        try:
            # Try to parse as JSON
            import re
            
            # Find JSON array in response
            match = re.search(r'\[.*\]', content, re.DOTALL)
            if match:
                categories = json.loads(match.group())
                
                if len(categories) == expected:
                    return categories
        except Exception:
            pass
        
        # Fallback
        return ["Uncategorized"] * expected


class RuleBasedClassifier:
    """Simple rule-based categorization."""
    
    KEYWORDS = {
        "Tech": ["ai", "software", "google", "apple", "microsoft", "startup", "tech", "code", "programming"],
        "Business": ["market", "stock", "economy", "business", "company", "revenue", "profit"],
        "Science": ["research", "study", "scientist", "discovery", "space", "nasa", "climate"],
        "Politics": ["government", "president", "congress", "election", "vote", "policy"],
        "World": ["ukraine", "china", "europe", "asia", "war", "international"],
        "Entertainment": ["movie", "film", "music", "celebrity", "hollywood", "netflix"],
        "Health": ["health", "covid", "virus", "disease", "doctor", "medical"],
        "Sports": ["game", "team", "player", "championship", "olympics", "football"],
    }
    
    def categorize(self, titles: list[str]) -> list[str]:
        """Categorize based on keywords."""
        
        categories = []
        
        for title in titles:
            title_lower = title.lower()
            
            best_match = "Uncategorized"
            best_count = 0
            
            for category, keywords in self.KEYWORDS.items():
                count = sum(1 for kw in keywords if kw in title_lower)
                
                if count > best_count:
                    best_count = count
                    best_match = category
            
            categories.append(best_match)
        
        return categories
```

### formatters.py - Output Formatting

```python
"""Format and display news articles."""

from rich.console import Console
from rich.table import Table
from rich.panel import Panel
from rich.markdown import Markdown
from datetime import datetime


class NewsFormatter:
    """Format news for terminal display."""
    
    def __init__(self):
        self.console = Console()
    
    def display_by_category(self, articles: list, by_category: dict) -> None:
        """Display articles grouped by category."""
        
        for category, articles in sorted(by_category.items()):
            self._display_category(category, articles)
    
    def _display_category(self, category: str, articles: list) -> None:
        """Display a single category."""
        
        # Header
        emoji = self._category_emoji(category)
        self.console.print(f"\n{emoji} {category.upper()}")
        self.console.print("=" * 50)
        
        # Articles
        for i, article in enumerate(articles[:10], 1):
            self.console.print(f"{i}. [bold]{article.title}[/bold]")
            self.console.print(f"   📰 {article.source}")
            
            if article.published_at:
                date = article.published_at[:10] if len(article.published_at) >= 10 else article.published_at
                self.console.print(f"   📅 {date}")
            
            self.console.print(f"   🔗 {article.url}")
            self.console.print()
    
    def _category_emoji(self, category: str) -> str:
        """Get emoji for category."""
        
        emojis = {
            "Tech": "💻",
            "Business": "💼",
            "Science": "🔬",
            "Politics": "🏛️",
            "World": "🌍",
            "Entertainment": "🎬",
            "Health": "🏥",
            "Sports": "⚽",
            "Uncategorized": "📰"
        }
        
        return emojis.get(category, "📰")
    
    def display_table(self, articles: list) -> None:
        """Display articles in a table."""
        
        table = Table(title="Latest News")
        
        table.add_column("#", style="dim", width=4)
        table.add_column("Title", style="cyan", width=60)
        table.add_column("Source", style="magenta")
        table.add_column("Category", style="green")
        
        for i, article in enumerate(articles[:20], 1):
            table.add_row(
                str(i),
                article.title[:57] + "..." if len(article.title) > 60 else article.title,
                article.source,
                article.category or "—"
            )
        
        self.console.print(table)
    
    def export_markdown(self, articles: list, by_category: dict, filename: str) -> None:
        """Export to Markdown file."""
        
        lines = [
            "# 📰 News Digest",
            f"\nGenerated: {datetime.now().strftime('%Y-%m-%d %H:%M')}\n",
        ]
        
        for category, articles in sorted(by_category.items()):
            emoji = self._category_emoji(category)
            lines.append(f"## {emoji} {category}\n")
            
            for article in articles[:10]:
                lines.append(f"- [{article.title}]({article.url}) ({article.source})")
            
            lines.append("")
        
        content = "\n".join(lines)
        
        with open(filename, "w", encoding="utf-8") as f:
            f.write(content)
        
        self.console.print(f"\n✅ Exported to {filename}")
```

### main.py - Main Entry Point

```python
"""News Aggregator - Main entry point."""

import asyncio
import argparse

from database import NewsDatabase, Article
from fetcher import NewsFetcher
from deduplicator import deduplicate_articles
from categorizer import CategoryClassifier, RuleBasedClassifier
from formatters import NewsFormatter


class NewsAggregator:
    """Main news aggregator application."""
    
    def __init__(self):
        self.db = NewsDatabase()
        self.fetcher = NewsFetcher()
        self.formatter = NewsFormatter()
        
        # Use rule-based if no API key
        api_key = None  # Or set os.getenv("ANTHROPIC_API_KEY")
        
        if api_key:
            self.classifier = CategoryClassifier(api_key)
        else:
            self.classifier = RuleBasedClassifier()
    
    async def fetch_and_process(self) -> None:
        """Fetch, deduplicate, categorize, and save news."""
        
        print("📥 Fetching news from all sources...")
        
        # Fetch
        items = await self.fetcher.fetch_all()
        print(f"   Fetched {len(items)} articles")
        
        # Convert to Article objects
        articles = [
            Article(
                title=item.title,
                url=item.url,
                source=item.source,
                category="",  # Will be filled by classifier
                published_at=item.published_at,
                summary=item.summary
            )
            for item in items
        ]
        
        # Deduplicate
        print("🔄 Deduplicating...")
        result = deduplicate_articles(articles, threshold=0.75)
        print(f"   Found {result.duplicates} duplicates")
        articles = result.unique
        print(f"   {len(articles)} unique articles")
        
        # Categorize
        print("🏷️ Categorizing...")
        titles = [a.title for a in articles]
        categories = self.classifier.categorize(titles)
        
        for article, category in zip(articles, categories):
            article.category = category
        
        # Save to database
        print("💾 Saving to database...")
        saved = 0
        for article in articles:
            if self.db.add_article(article):
                saved += 1
        
        print(f"   Added {saved} new articles")
        
        # Display
        by_category = self.db.get_by_category(days=1)
        self.formatter.display_by_category(articles, by_category)
        
        # Export
        self.formatter.export_markdown(articles, by_category, "news_digest.md")
    
    def show_recent(self, days: int = 1) -> None:
        """Show recent articles."""
        
        by_category = self.db.get_by_category(days=days)
        self.formatter.display_by_category([], by_category)


async def main():
    """Main entry point."""
    
    parser = argparse.ArgumentParser(description="News Aggregator")
    parser.add_argument("--days", type=int, default=1, help="Days to show")
    
    args = parser.parse_args()
    
    aggregator = NewsAggregator()
    
    # Fetch fresh news
    await aggregator.fetch_and_process()
    
    # Or show recent from database:
    # aggregator.show_recent(args.days)


if __name__ == "__main__":
    asyncio.run(main())
```

---

## Usage

```bash
python main.py
```

### Output

```
📥 Fetching news from all sources...
   Fetched 50 articles
🔄 Deduplicating...
   Found 5 duplicates
   45 unique articles
🏷️ Categorizing...
💾 Saving to database...
   Added 45 new articles

💻 TECH
==================================================
1. AI Company Raises $100M
   📰 TechCrunch
   🔗 https://techcrunch.com/...


💼 BUSINESS
==================================================
1. Stock Market Update
   📰 BBC World
   🔗 https://bbc.com/...

✅ Exported to news_digest.md
```

---

## 🚀 Challenge

Extend the news aggregator with:

1. **Sentiment Analysis** - Add sentiment scores to articles
2. **HTML Report** - Generate a simple HTML dashboard
3. **More Sources** - Add Reddit, Twitter APIs
4. **Email Digest** - Send daily digest via email

---

## Summary

✅ Built complete news aggregator

✅ Fetches from RSS and APIs concurrently

✅ Deduplicates by title similarity

✅ Categorizes with AI or rule-based fallback

✅ Rich terminal output and Markdown export

---

## 🔗 Further Reading

- [feedparser Documentation](https://feedparser.readthedocs.io/)
- [Rich Library](https://rich.readthedocs.io/)
- [Hacker News API](https://github.com/HackerNews/API)
