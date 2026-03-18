# Maintaining Open Source Projects

## What You'll Learn

- Responsibilities of maintainers
- Managing issues and PRs
- Releasing versions

## Prerequisites

- Completed `06-code-review-process.md`

## Responsibilities

Maintainers are responsible for:

- Reviewing and merging contributions
- Responding to issues
- Managing releases
- Setting project direction

## Managing Issues

```python
from dataclasses import dataclass
from datetime import datetime
from typing import Literal

@dataclass
class Issue:
    title: str
    description: str
    status: Literal["open", "closed"]
    priority: Literal["low", "medium", "high", "critical"]
    created_at: datetime
    labels: list[str]

class IssueManager:
    def __init__(self):
        self._issues: dict[int, Issue] = {}
        self._next_id = 1
    
    def create_issue(self, title: str, description: str, priority: str = "medium") -> Issue:
        issue = Issue(
            title=title,
            description=description,
            status="open",
            priority=priority,
            created_at=datetime.now(),
            labels=[]
        )
        issue.id = self._next_id
        self._issues[self._next_id] = issue
        self._next_id += 1
        return issue
    
    def close_issue(self, issue_id: int) -> None:
        if issue_id in self._issues:
            self._issues[issue_id].status = "closed"
    
    def get_issues_by_priority(self, priority: str) -> list[Issue]:
        return [i for i in self._issues.values() if i.priority == priority]
```

## Release Process

```python
from dataclasses import dataclass
from datetime import datetime

@dataclass
class Release:
    version: str
    changelog: str
    released_at: datetime
    
    def __str__(self) -> str:
        return f"Release {self.version} ({self.released_at.date()})"

class ReleaseManager:
    def __init__(self):
        self._releases: list[Release] = []
    
    def create_release(self, version: str, changelog: str) -> Release:
        release = Release(
            version=version,
            changelog=changelog,
            released_at=datetime.now()
        )
        self._releases.append(release)
        return release
    
    def get_latest_release(self) -> Release | None:
        return self._releases[-1] if self._releases else None
```

## Summary

- Maintainers guide project direction
- Good issue management is crucial
- Follow semantic versioning for releases

## Next Steps

Continue to `08-building-community.md`.
