# GitHub API Integration

## What You'll Learn

- How to authenticate with GitHub's REST API
- How to fetch repositories, issues, and pull requests
- How to create and manage GitHub issues programmatically
- How to work with GitHub Actions

## Prerequisites

- Completed `01-twitter-api-integration.md`
- A GitHub account
- A GitHub personal access token

## Introduction

GitHub provides a powerful REST API that allows you to interact with repositories, issues, pull requests, and more. This guide covers the most common use cases for web developers.

## Authentication

GitHub API supports several authentication methods. For most use cases, a Personal Access Token (PAT) is the easiest approach:

```bash
pip install requests PyGithub
```

Create a Personal Access Token:
1. Go to GitHub Settings → Developer settings → Personal access tokens
2. Generate a new token (classic)
3. Select the required scopes (repo, read:user, etc.)

## Using the GitHub API

### Basic API Client

Here's how to interact with the GitHub API:

```python
import os
from dataclasses import dataclass
from typing import Optional
from datetime import datetime

import requests
from github import Github  # PyGithub library


@dataclass
class GitHubConfig:
    """Configuration for GitHub API access."""
    personal_access_token: str


class GitHubClient:
    """Client for interacting with GitHub's REST API."""
    
    def __init__(self, config: GitHubConfig) -> None:
        self.config = config
        self.base_url = "https://api.github.com"
        self.headers = {
            "Authorization": f"token {config.personal_access_token}",
            "Accept": "application/vnd.github.v3+json",
        }
        # Also initialize PyGithub for convenience
        self._pygithub = Github(config.personal_access_token)
    
    def get_user(self, username: str) -> dict:
        """Fetch user information."""
        url = f"{self.base_url}/users/{username}"
        response = requests.get(url, headers=self.headers)
        response.raise_for_status()
        return response.json()
    
    def get_user_repositories(self, username: str) -> list[dict]:
        """Fetch all repositories for a user."""
        url = f"{self.base_url}/users/{username}/repos"
        params = {"sort": "updated", "per_page": 30}
        response = requests.get(url, headers=self.headers, params=params)
        response.raise_for_status()
        return response.json()
    
    def get_repository(self, owner: str, repo: str) -> dict:
        """Fetch repository information."""
        url = f"{self.base_url}/repos/{owner}/{repo}"
        response = requests.get(url, headers=self.headers)
        response.raise_for_status()
        return response.json()
    
    def get_issues(self, owner: str, repo: str, state: str = "open") -> list[dict]:
        """Fetch issues for a repository."""
        url = f"{self.base_url}/repos/{owner}/{repo}/issues"
        params = {"state": state, "per_page": 30}
        response = requests.get(url, headers=self.headers, params=params)
        response.raise_for_status()
        return response.json()
    
    def create_issue(
        self,
        owner: str,
        repo: str,
        title: str,
        body: str,
        labels: Optional[list[str]] = None,
    ) -> dict:
        """Create a new issue."""
        url = f"{self.base_url}/repos/{owner}/{repo}/issues"
        payload = {"title": title, "body": body}
        if labels:
            payload["labels"] = labels
        response = requests.post(url, headers=self.headers, json=payload)
        response.raise_for_status()
        return response.json()
    
    def get_pull_requests(
        self,
        owner: str,
        repo: str,
        state: str = "open",
    ) -> list[dict]:
        """Fetch pull requests for a repository."""
        url = f"{self.base_url}/repos/{owner}/{repo}/pulls"
        params = {"state": state, "per_page": 30}
        response = requests.get(url, headers=self.headers, params=params)
        response.raise_for_for_status()
        return response.json()


# Example usage
def main() -> None:
    config = GitHubConfig(
        personal_access_token=os.environ["GITHUB_TOKEN"],
    )
    
    client = GitHubClient(config)
    
    # Get user info
    user = client.get_user("torvalds")
    print(f"User: {user['name']}")
    print(f"Followers: {user['followers']}")
    
    # Get recent repositories
    repos = client.get_user_repositories("torvalds")
    for repo in repos[:5]:
        print(f"- {repo['name']}: {repo['description']}")
    
    # Create an issue (example)
    issue = client.create_issue(
        owner="your-username",
        repo="your-repo",
        title="Bug in user login",
        body="Users are unable to log in with Google OAuth",
        labels=["bug", "high-priority"],
    )
    print(f"Created issue: {issue['html_url']}")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `import os` — OS module for accessing environment variables.
2. `from dataclasses import dataclass` — Dataclass for clean configuration.
3. `from typing import Optional` — Type hint for optional parameters.
4. `from datetime import datetime` — For handling dates from GitHub's API.
5. `import requests` — HTTP library for making API requests.
6. `from github import Github` — PyGithub library provides a higher-level interface.
7. `GitHubConfig` — Dataclass holding the personal access token.
8. `GitHubClient` — Main client class for GitHub API interactions.
9. `self._pygithub = Github(...)` — PyGithub instance for convenience methods.
10. `get_user()` — Fetches user information by username.
11. `get_user_repositories()` — Gets all repos for a user, sorted by last update.
12. `get_repository()` — Fetches detailed repo information.
13. `get_issues()` — Gets issues with optional state filtering (open/closed/all).
14. `create_issue()` — Creates a new issue with title, body, and optional labels.
15. `get_pull_requests()` — Fetches PRs with state filtering.

### Using PyGithub (Higher-Level Interface)

PyGithub provides a more Pythonic interface:

```python
from github import Github


def use_pygithub(token: str) -> None:
    """Higher-level GitHub interaction using PyGithub."""
    g = Github(token)
    
    # Get current user
    user = g.get_user()
    print(f"Logged in as: {user.login}")
    
    # Get a specific repository
    repo = g.get_repo("psf/requests")
    print(f"Repository: {repo.full_name}")
    print(f"Stars: {repo.stargazers_count}")
    
    # Get issues
    for issue in repo.get_issues(state="open")[:5]:
        print(f"- Issue #{issue.number}: {issue.title}")
    
    # Create an issue
    repo.create_issue(
        title="Test issue from API",
        body="This issue was created using PyGithub",
        labels=["enhancement"],
    )
    
    # Get commits
    for commit in repo.get_commits()[:3]:
        print(f"- Commit: {commit.sha[:7]} - {commit.commit.message}")
```

🔍 **Line-by-Line Breakdown:**

1. `from github import Github` — Import PyGithub's main class.
2. `g = Github(token)` — Initialize the GitHub client with your token.
3. `g.get_user()` — Gets the authenticated user's information.
4. `g.get_repo("psf/requests")` — Gets a specific repository by full name (owner/repo).
5. `repo.get_issues(state="open")` — Gets open issues as a PaginatedList.
6. `repo.create_issue()` — Creates a new issue with title, body, and labels.
7. `repo.get_commits()` — Gets recent commits to the repository.

## Working with GitHub Actions

You can also interact with GitHub Actions:

```python
def get_workflow_runs(self, owner: str, repo: str, workflow_id: str) -> list[dict]:
    """Get recent workflow runs."""
    url = f"{self.base_url}/repos/{owner}/{repo}/actions/workflows/{workflow_id}/runs"
    response = requests.get(url, headers=self.headers)
    response.raise_for_status()
    return response.json()["workflow_runs"]


def trigger_workflow(
    self,
    owner: str,
    repo: str,
    workflow_id: str,
    ref: str = "main",
) -> dict:
    """Trigger a workflow dispatch event."""
    url = f"{self.base_url}/repos/{owner}/{repo}/actions/workflows/{workflow_id}/dispatches"
    payload = {"ref": ref}
    response = requests.post(url, headers=self.headers, json=payload)
    response.raise_for_status()
    return {"status": "workflow triggered"}


def get_workflow_jobs(self, owner: str, repo: str, run_id: int) -> list[dict]:
    """Get jobs for a workflow run."""
    url = f"{self.base_url}/repos/{owner}/{repo}/actions/runs/{run_id}/jobs"
    response = requests.get(url, headers=self.headers)
    response.raise_for_status()
    return response.json()["jobs"]
```

## Searching GitHub

GitHub's search API is powerful:

```python
def search_repositories(
    self,
    query: str,
    language: Optional[str] = None,
    sort: str = "stars",
) -> list[dict]:
    """Search for repositories."""
    url = f"{self.base_url}/search/repositories"
    params = {"q": query, "sort": sort, "per_page": 30}
    if language:
        params["q"] += f" language:{language}"
    response = requests.get(url, headers=self.headers, params=params)
    response.raise_for_status()
    return response.json()["items"]


def search_issues(self, query: str) -> list[dict]:
    """Search for issues and pull requests."""
    url = f"{self.base_url}/search/issues"
    params = {"q": query, "per_page": 30}
    response = requests.get(url, headers=self.headers, params=params)
    response.raise_for_status()
    return response.json()["items"]
```

Example searches:
- `language:python stars:>1000` — Popular Python repos
- `is:issue is:open label:bug` — Open bug issues
- `user:fastapi stars:>500` — Popular repos by a user

## Rate Limiting

GitHub has rate limits:
- 60 requests/hour for unauthenticated
- 5,000 requests/hour for authenticated

```python
def check_rate_limit(self) -> dict:
    """Check current rate limit status."""
    url = f"{self.base_url}/rate_limit"
    response = requests.get(url, headers=self.headers)
    response.raise_for_status()
    return response.json()


def handle_rate_limit(self, response: requests.Response) -> None:
    """Handle rate limiting."""
    if response.status_code == 403:
        import time
        reset_time = int(response.headers.get("X-RateLimit-Reset", 0))
        wait_time = reset_time - int(time.time())
        if wait_time > 0:
            print(f"Rate limited. Waiting {wait_time} seconds...")
            time.sleep(wait_time)
```

## Summary

- GitHub API uses Personal Access Tokens for authentication
- Use the requests library for direct API calls
- PyGithub provides a higher-level, more Pythonic interface
- The search API is powerful for finding repositories and issues
- Be mindful of rate limits (5,000/hour authenticated)

## Next Steps

→ Continue to `03-stripe-payment-integration.md` to learn about accepting payments.
