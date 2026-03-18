# Finding Projects to Contribute To

## What You'll Learn

- How to find open source projects
- Evaluating projects for beginners
- Good first issue labels

## Prerequisites

- Completed `01-introduction-to-open-source.md`

## Where to Find Projects

### GitHub Explore

GitHub has a dedicated explore section with topics for beginners:

- [github.com/explore](https://github.com/explore) - Browse topics
- [github.com/topics](https://github.com/topics) - Topics by category
- [github.com/issues](https://github.com/issues) - Find issues

### Python-Specific Resources

- **PyPI Packages** - Any package you've used
- **Django** - Large, welcoming community
- **FastAPI** - Active modern framework
- **SQLAlchemy** - Database toolkit
- **Requests** - HTTP library

### Good First Issue Labels

Most well-maintained projects label beginner-friendly issues:

```
good first issue
good first PR
beginner
help wanted
```

## Finding Issues

```python
# You can search GitHub issues programmatically
import httpx
from dataclasses import dataclass

@dataclass
class Issue:
    title: str
    url: str
    labels: list[str]
    repository: str

async def find_good_first_issues(query: str) -> list[Issue]:
    """Search for good first issues on GitHub."""
    async with httpx.AsyncClient() as client:
        response = await client.get(
            "https://api.github.com/search/issues",
            params={
                "q": f"{query} label:good-first-issue is:issue is:open",
                "per_page": 10
            },
            headers={"Accept": "application/vnd.github.v3+json"}
        )
        
        if response.status_code != 200:
            return []
        
        data = response.json()
        return [
            Issue(
                title=item["title"],
                url=item["html_url"],
                labels=[l["name"] for l in item["labels"]],
                repository=item["repository_url"].split("/")[-2:]
            )
            for item in data.get("items", [])
        ]
```

## Summary

- Use GitHub Explore and topic pages to find projects
- Look for "good first issue" labels
- Start with tools you already use

## Next Steps

Continue to `03-setting-up-development-environment.md` to prepare for contributions.
