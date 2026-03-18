# Twitter/X API Integration

## What You'll Learn

- How to authenticate with the Twitter/X API
- How to fetch tweets and user data
- How to post tweets programmatically
- Best practices for rate limiting

## Prerequisites

- Completed the FastAPI section
- Basic understanding of OAuth 2.0
- A Twitter/X developer account

## Introduction

Integrating with social media APIs allows your application to interact with platforms like Twitter/X. This guide covers the Twitter API v2, the current version of Twitter's developer API.

## Setting Up Twitter Developer Account

Before writing any code, you need to set up your Twitter developer account:

1. Go to https://developer.twitter.com/
2. Apply for a developer account
3. Create a project and app
4. Generate API keys and access tokens

## Authentication with OAuth 2.0

Twitter API v2 uses OAuth 2.0 for authentication. You'll need to install the requests library:

```bash
pip install requests python-dotenv
```

Here's how to authenticate and make API calls:

```python
import os
import requests
from datetime import datetime
from dataclasses import dataclass
from typing import Optional


@dataclass
class TwitterConfig:
    """Configuration for Twitter API access."""
    api_key: str
    api_secret: str
    access_token: str
    access_token_secret: str
    bearer_token: str


class TwitterClient:
    """Client for interacting with Twitter API v2."""
    
    def __init__(self, config: TwitterConfig) -> None:
        self.config = config
        self.base_url = "https://api.twitter.com/2"
        self.headers = {
            "Authorization": f"Bearer {config.bearer_token}",
            "Content-Type": "application/json",
        }
    
    def get_user_by_username(self, username: str) -> dict:
        """Fetch user information by username."""
        url = f"{self.base_url}/users/by/username/{username}"
        response = requests.get(url, headers=self.headers)
        response.raise_for_status()
        return response.json()
    
    def get_user_tweets(self, user_id: str, max_results: int = 10) -> dict:
        """Fetch recent tweets for a user."""
        url = f"{self.base_url}/users/{user_id}/tweets"
        params = {
            "max_results": max_results,
            "tweet.fields": "created_at,public_metrics",
        }
        response = requests.get(url, headers=self.headers, params=params)
        response.raise_for_status()
        return response.json()
    
    def post_tweet(self, text: str) -> dict:
        """Post a new tweet."""
        url = f"{self.base_url}/tweets"
        payload = {"text": text}
        response = requests.post(url, headers=self.headers, json=payload)
        response.raise_for_status()
        return response.json()


# Example usage
def main() -> None:
    config = TwitterConfig(
        api_key=os.environ["TWITTER_API_KEY"],
        api_secret=os.environ["TWITTER_API_SECRET"],
        access_token=os.environ["TWITTER_ACCESS_TOKEN"],
        access_token_secret=os.environ["TWITTER_ACCESS_TOKEN_SECRET"],
        bearer_token=os.environ["TWITTER_BEARER_TOKEN"],
    )
    
    client = TwitterClient(config)
    
    # Get user info
    user = client.get_user_by_username("python")
    print(f"User: {user['data']['name']}")
    
    # Get recent tweets
    user_id = user['data']['id']
    tweets = client.get_user_tweets(user_id)
    
    for tweet in tweets['data']:
        print(f"- {tweet['text'][:100]}...")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `import os` — OS module for accessing environment variables where we store API keys securely.
2. `import requests` — HTTP library for making API requests to Twitter's servers.
3. `from dataclasses import dataclass` — Dataclass for creating a clean configuration object.
4. `TwitterConfig` — A dataclass that holds all authentication credentials. This keeps our credentials organized.
5. `class TwitterClient` — Main client class that handles all Twitter API interactions.
6. `self.base_url = "https://api.twitter.com/2"` — The base URL for Twitter API v2.
7. `self.headers` — HTTP headers including the Bearer token for authentication.
8. `get_user_by_username()` — Method to fetch user information. Takes a username string and returns a dict.
9. `requests.get(url, headers=self.headers)` — Makes a GET request to Twitter's API. The `raise_for_status()` method throws an exception if the request fails.
10. `get_user_tweets()` — Fetches recent tweets for a user. Uses query parameters to specify max results and additional tweet fields.
11. `post_tweet()` — Posts a new tweet. Uses POST request with JSON payload containing the tweet text.
12. `main()` — Example function showing how to use the client. Creates a config from environment variables, instantiates the client, and makes API calls.

## Rate Limiting

Twitter has strict rate limits. Here's how to handle them:

```python
import time
from requests.exceptions import HTTPError


class RateLimitedClient(TwitterClient):
    """Twitter client with automatic rate limit handling."""
    
    def __init__(self, config: TwitterConfig) -> None:
        super().__init__(config)
        self.retry_after: Optional[int] = None
    
    def _handle_rate_limit(self, response: requests.Response) -> None:
        """Check for rate limit headers and wait if necessary."""
        if response.status_code == 429:
            retry_after = int(response.headers.get("x-rate-limit-reset", 60))
            wait_time = retry_after - int(time.time())
            print(f"Rate limited. Waiting {wait_time} seconds...")
            time.sleep(wait_time)
    
    def get_user_tweets(self, user_id: str, max_results: int = 10) -> dict:
        """Fetch tweets with automatic rate limit handling."""
        while True:
            try:
                return super().get_user_tweets(user_id, max_results)
            except HTTPError as e:
                if e.response.status_code == 429:
                    self._handle_rate_limit(e.response)
                else:
                    raise
```

🔍 **Line-by-Line Breakdown:**

1. `import time` — Time module for implementing sleep functionality.
2. `from requests.exceptions import HTTPError` — Exception class for HTTP errors.
3. `class RateLimitedClient(TwitterClient)` — Extends the base TwitterClient to add rate limiting.
4. `self.retry_after: Optional[int]` — Stores the retry-after time from rate limit responses.
5. `_handle_rate_limit()` — Private method that checks for rate limiting and waits the appropriate time.
6. `response.status_code == 429` — HTTP 429 means "Too Many Requests" - we've exceeded our rate limit.
7. `x-rate-limit-reset` — Header containing the Unix timestamp when the rate limit resets.
8. `time.sleep(wait_time)` — Pauses execution until the rate limit resets.
9. `get_user_tweets()` — Overridden method that wraps the parent method in a retry loop.

## Posting a Tweet with Media

You can also post tweets with images:

```python
def post_tweet_with_media(self, text: str, media_path: str) -> dict:
    """Post a tweet with an image."""
    url = f"{self.base_url}/tweets"
    
    # First, upload the media
    media_url = "https://upload.twitter.com/1.1/media/upload.json"
    
    with open(media_path, "rb") as media_file:
        files = {"media": media_file}
        media_response = requests.post(
            media_url,
            files=files,
            auth=self._get_oauth_auth(),
        )
    
    media_id = media_response.json()["media_id_string"]
    
    # Then, post the tweet with the media
    payload = {
        "text": text,
        "media": {"media_ids": [media_id]},
    }
    
    response = requests.post(url, headers=self.headers, json=payload)
    response.raise_for_status()
    return response.json()

def _get_oauth_auth(self) -> tuple:
    """Get OAuth 1.0a authentication for media uploads."""
    import oauthlib
    from requests_oauthlib import OAuth1
    
    return OAuth1(
        self.config.api_key,
        client_secret=self.config.api_secret,
        resource_owner_key=self.config.access_token,
        resource_owner_secret=self.config.access_token_secret,
    )
```

## Environment Variables

Never hardcode your API keys. Use environment variables:

```bash
# .env file (add to .gitignore)
TWITTER_API_KEY=your_api_key_here
TWITTER_API_SECRET=your_api_secret_here
TWITTER_ACCESS_TOKEN=your_access_token_here
TWITTER_ACCESS_TOKEN_SECRET=your_access_token_secret_here
TWITTER_BEARER_TOKEN=your_bearer_token_here
```

Load them in your code:

```python
from dotenv import load_dotenv

load_dotenv()  # Load from .env file
```

## Summary

- Twitter API v2 uses OAuth 2.0 with Bearer tokens
- Always store API keys in environment variables
- Implement rate limiting handling to avoid being blocked
- Use the requests library for HTTP communication
- Dataclasses provide clean configuration management

## Next Steps

→ Continue to `02-github-api-integration.md` to learn about integrating with GitHub's API.
