# Advanced Django ORM Queries

## What You'll Learn
- Complex queryset operations
- Prefetching and select_related for performance
- Annotations and aggregations
- Raw SQL queries
- Database functions

## Prerequisites
- Completed `03-django-forms-and-modelforms.md` — Form handling
- Understanding of Django models
- Basic SQL knowledge

## Complex Lookups

### Q Objects for Complex Queries

```python
from django.db.models import Q

# OR queries
Article.objects.filter(
    Q(title__icontains="django") | Q(content__icontains="django")
)

# NOT queries
Article.objects.exclude(
    Q(status="draft") | Q(published__lt=timezone.now())
)

# Complex combinations
Article.objects.filter(
    (Q(author=request.user) | Q(status="published")) &
    Q(published__gte=timezone.now())
)
```

🔍 **Line-by-Line Breakdown:**
1. `Q(title__icontains="django")` — Q object for case-insensitive contains
2. `|` — OR operator between Q objects
3. `&` — AND operator
4. `~` — NOT operator (negation)

### Field Lookups Reference

```python
# Common lookups
Model.objects.filter(field__exact="value")      # Exact match
Model.objects.filter(field__iexact="value")    # Case-insensitive exact
Model.objects.filter(field__contains="value")  # Case-sensitive contains
Model.objects.filter(field__icontains="value") # Case-insensitive contains
Model.objects.filter(field__in=["a", "b", "c"])  # In list
Model.objects.filter(field__gt=10)             # Greater than
Model.objects.filter(field__gte=10)            # Greater than or equal
Model.objects.filter(field__lt=10)             # Less than
Model.objects.filter(field__lte=10)            # Less than or equal
Model.objects.filter(field__range=[start, end])  # Between (inclusive)
Model.objects.filter(field__isnull=True)       # IS NULL
Model.objects.filter(field__regex=r"^\w+$")    # Regex
Model.objects.filter(date__year=2024)         # Date extract
Model.objects.filter(date__month=12)           # Month extract
Model.objects.filter(date__week=1)             # Week number
```

## Prefetch and Select Related

### select_related — For ForeignKey/OneToOne

```python
# BEFORE: N+1 query problem!
# 1 query for articles + N queries for each author
articles = Article.objects.all()
for article in articles:
    print(article.author.name)  # Each access = new query!

# AFTER: Single query with JOIN
articles = Article.objects.select_related("author")
for article in articles:
    print(article.author.name)  # No extra query!
```

### prefetch_related — For ManyToMany/Reverse ForeignKey

```python
# BEFORE: N+1 for tags!
articles = Article.objects.all()
for article in articles:
    for tag in article.tags.all():  # Each access = new query!
        print(tag.name)

# AFTER: 2 queries total (articles + tags in batch)
articles = Article.objects.prefetch_related("tags")
for article in articles:
    for tag in article.tags.all():  # No extra queries!
        print(tag.name)
```

### Prefetch with Custom Queryset

```python
from django.db.models import Prefetch

# Prefetch only published articles for each author
authors = Author.objects.prefetch_related(
    Prefetch(
        "articles",
        queryset=Article.objects.filter(status="published"),
        to_attr="published_articles"
    )
)

for author in authors:
    print(f"{author.name}'s published articles:")
    for article in author.published_articles:
        print(f"  - {article.title}")
```

## Annotations

### Basic Annotations

```python
from django.db.models import Count, Max, Min, Avg, Sum

# Count related objects
Article.objects.annotate(
    comment_count=Count("comments")
)

# Multiple aggregations
Author.objects.annotate(
    article_count=Count("article"),
    latest_article=Max("article__published"),
    total_views=Sum("article__views")
)
```

### Conditional Annotation

```python
from django.db.models import Case, When, IntegerField

# Count published vs draft articles
Article.objects.values("author").annotate(
    published_count=Count(Case(When(status="published", then=1))),
    draft_count=Count(Case(When(status="draft", then=1)))
)
```

### Annotation with Filter

```python
# Count comments in last 30 days per article
Article.objects.annotate(
    recent_comments=Count(
        "comments",
        filter=Q(comments__created__gte=timezone.now() - timedelta(days=30))
    )
)
```

## Aggregations

```python
from django.db.models import Avg, Sum, Count, Max, Min

# Total comments
Comment.objects.aggregate(total=Count("id"))

# Multiple aggregations
Article.objects.aggregate(
    total_views=Sum("views"),
    avg_views=Avg("views"),
    max_views=Max("views"),
    article_count=Count("id")
)

# Group by category
Category.objects.annotate(
    article_count=Count("article"),
    total_views=Sum("article__views")
).order_by("-article_count")
```

## Database Functions

```python
from django.db.models import F, Value, CharField
from django.db.models.functions import Concat, Upper, Lower, Length, Substr

# F() — Reference field values
# Increase all article views by 1
Article.objects.update(views=F("views") + 1)

# Calculate discounted price
Product.objects.annotate(
    discounted_price=F("price") * 0.9
)

# String functions
Author.objects.annotate(
    full_name=Concat("first_name", Value(" "), "last_name"),
    name_upper=Upper("first_name"),
    name_length=Length("first_name")
)

# Date functions
from django.db.models.functions import ExtractYear, ExtractMonth

Article.objects.annotate(
    year=ExtractYear("published"),
    month=ExtractMonth("published")
).values("year", "month").annotate(count=Count("id"))
```

## Raw SQL Queries

### Raw Query for Complex Reports

```python
# Execute raw SQL
from django.db import connection

with connection.cursor() as cursor:
    cursor.execute("""
        SELECT status, COUNT(*) as count
        FROM blog_article
        GROUP BY status
    """)
    results = cursor.fetchall()

# With parameters (safe from SQL injection)
Article.objects.raw(
    "SELECT * FROM blog_article WHERE published > %s",
    [timezone.now()]
)
```

### Manager Methods for Raw Queries

```python
# Using .raw() on custom manager
class ArticleManager(models.Manager):
    def published_today(self):
        return self.raw("""
            SELECT * FROM blog_article
            WHERE DATE(published) = CURDATE()
        """)
    
    def with_comment_counts(self):
        return self.raw("""
            SELECT a.*, COUNT(c.id) as comment_count
            FROM blog_article a
            LEFT JOIN blog_comment c ON c.article_id = a.id
            GROUP BY a.id
        """)

class Article(models.Model):
    # ... fields ...
    objects = ArticleManager()
```

## Subqueries

```python
from django.db.models import Subquery, OuterRef

# Subquery: Latest article per author
latest_articles = Article.objects.filter(
    author=OuterRef("author")
).order_by("-published")

Author.objects.annotate(
    latest_title=Subquery(latest_articles.values("title")[:1])
)

# Subquery for update
Article.objects.update(
    views=F("views") + Subquery(
        DailyStats.objects.filter(
            article=OuterRef("pk"),
            date=timezone.now()
        ).values("views")[:1]
    )
)
```

## QuerySet Methods

### Chaining Filters

```python
# Chain multiple filters
Article.objects.filter(
    status="published"
).filter(
    published__gte=timezone.now()
).exclude(
    author__in=blocked_authors
).order_by("-published")

# Filter with OR
from django.db.models import Q
Article.objects.filter(
    Q(status="published") | Q(featured=True)
)
```

### QuerySet Slicing

```python
# Limit results
Article.objects.all()[:10]  # First 10

# Pagination
Article.objects.all()[offset:limit]

# Get random article (not efficient for large tables)
import random
Article.objects.all()[random.randint(0, Article.objects.count() - 1)]

# Better: Use database random function
Article.objects.order_by("?")[:1]
```

## Production Considerations

- **N+1 queries**: Always use `select_related` and `prefetch_related`
- **Indexes**: Add `db_index=True` to frequently queried fields
- **Explain**: Use `.query.__str__()` or Django Debug Toolbar to analyze queries
- **Pagination**: Never use offset without limit

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: N+1 queries

**Wrong:**
```python
# For each article, queries author separately
for article in Article.objects.all():
    print(article.author.name)
```

**Why it fails:** 1 query for articles + N queries for authors.

**Fix:**
```python
# Single query with JOIN
for article in Article.objects.select_related("author"):
    print(article.author.name)
```

### ❌ Mistake 2: Using Python filtering instead of database

**Wrong:**
```python
# Python filtering (loads all data!)
articles = Article.objects.all()
published = [a for a in articles if a.status == "published"]
```

**Why it fails:** Fetches ALL articles, filters in Python (memory).

**Fix:**
```python
# Database filtering
articles = Article.objects.filter(status="published")
```

### ❌ Mistake 3: Counting with len()

**Wrong:**
```python
count = len(Article.objects.all())  # Loads ALL records!
```

**Why it fails:** Counts in Python, not database.

**Fix:**
```python
count = Article.objects.count()  # Uses COUNT(*)
```

## Summary

- Use `Q` objects for OR/NOT complex queries
- Use `select_related` for ForeignKey, `prefetch_related` for ManyToMany
- Use annotations to add computed fields to querysets
- Use aggregations for summary statistics
- Raw SQL is a last resort — Django ORM can handle most cases

## Next Steps

→ Continue to `05-django-signals.md` to learn about Django's signal system for decoupled applications.
