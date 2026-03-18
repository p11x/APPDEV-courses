# Discord Webhook Integration

## What You'll Learn

- How to create Discord webhooks
- How to send rich embed messages
- How to create interactive notifications
- Best practices for Discord bot integration

## Prerequisites

- Completed `06-twilio-sms-integration.md`
- A Discord account

## Introduction

Discord webhooks allow your application to send messages to a Discord channel without building a full bot. They're perfect for sending notifications, alerts, and updates to your team's Discord server.

## Setting Up Discord Webhooks

No library needed — Discord webhooks use simple HTTP requests:

```python
import os
import requests
from dataclasses import dataclass, field
from typing import Optional
from datetime import datetime


@dataclass
class DiscordConfig:
    """Configuration for Discord webhook."""
    webhook_url: str
    bot_username: str = "Your App Bot"
    bot_avatar_url: Optional[str] = None


@dataclass
class DiscordEmbed:
    """A Discord embed object."""
    title: str = ""
    description: str = ""
    color: int = 0x5865F2  # Discord blurple
    url: str = ""
    timestamp: Optional[str] = None
    footer: Optional[dict] = None
    image: Optional[dict] = None
    thumbnail: Optional[dict] = None
    author: Optional[dict] = None
    fields: list[dict] = field(default_factory=list)
    
    def to_dict(self) -> dict:
        """Convert embed to dictionary."""
        data = {
            "title": self.title,
            "description": self.description,
            "color": self.color,
        }
        
        if self.url:
            data["url"] = self.url
        
        if self.timestamp:
            data["timestamp"] = self.timestamp
        elif self.timestamp is not False:
            data["timestamp"] = datetime.utcnow().isoformat()
        
        if self.footer:
            data["footer"] = self.footer
        
        if self.image:
            data["image"] = self.image
        
        if self.thumbnail:
            data["thumbnail"] = self.thumbnail
        
        if self.author:
            data["author"] = self.author
        
        if self.fields:
            data["fields"] = self.fields
        
        return data


class DiscordClient:
    """Client for sending messages via Discord webhooks."""
    
    def __init__(self, config: DiscordConfig) -> None:
        self.config = config
    
    def send_message(
        self,
        content: str,
        username: Optional[str] = None,
        avatar_url: Optional[str] = None,
        embeds: Optional[list[DiscordEmbed]] = None,
        tts: bool = False,
    ) -> dict:
        """Send a message to the Discord channel."""
        payload = {
            "content": content,
            "username": username or self.config.bot_username,
            "avatar_url": avatar_url or self.config.bot_avatar_url,
            "tts": tts,
        }
        
        if embeds:
            payload["embeds"] = [embed.to_dict() for embed in embeds]
        
        response = requests.post(
            self.config.webhook_url,
            json=payload,
        )
        
        # Discord returns 204 No Content on success
        if response.status_code == 204:
            return {"success": True}
        
        return {
            "success": False,
            "status_code": response.status_code,
            "error": response.text,
        }
    
    def send_embed(self, embed: DiscordEmbed) -> dict:
        """Send an embed message."""
        return self.send_message(embeds=[embed])


# Example usage
def main() -> None:
    config = DiscordConfig(
        webhook_url=os.environ["DISCORD_WEBHOOK_URL"],
        bot_username="My App",
    )
    
    client = DiscordClient(config)
    
    # Simple message
    result = client.send_message("Hello from my app!")
    print(f"Message sent: {result}")
    
    # Rich embed
    embed = DiscordEmbed(
        title="New User Signup",
        description="A new user has registered!",
        color=0x00FF00,  # Green
        fields=[
            {"name": "Username", "value": "john_doe", "inline": True},
            {"name": "Email", "value": "john@example.com", "inline": True},
            {"name": "Plan", "value": "Pro", "inline": True},
        ],
    )
    
    result = client.send_embed(embed)
    print(f"Embed sent: {result}")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `requests` — The requests library (no special Discord library needed for webhooks).
2. `DiscordConfig` — Configuration holding the webhook URL and bot details.
3. `DiscordEmbed` — A dataclass representing a Discord embed (rich message with title, description, fields, etc.).
4. `color: int = 0x5865F2` — The default Discord blurple color in hexadecimal.
5. `to_dict()` — Converts the embed to a dictionary for the JSON payload.
6. `send_message()` — Sends a POST request to the webhook URL with the message payload.
7. `response.status_code == 204` — Discord returns 204 No Content on successful webhook delivery.

## Rich Embed Examples

### Deployment Notification

```python
def create_deployment_embed(
    environment: str,
    status: str,
    commit: str,
    deployed_by: str,
    duration: str,
    url: Optional[str] = None,
) -> DiscordEmbed:
    """Create an embed for deployment notifications."""
    
    # Color based on status
    colors = {
        "success": 0x00FF00,    # Green
        "failed": 0xFF0000,    # Red
        "in_progress": 0xFFFF00,  # Yellow
    }
    
    embed = DiscordEmbed(
        title=f"🚀 Deployment {status.title()}",
        description=f"Deployment to **{environment}** {status}",
        color=colors.get(status, 0x5865F2),
        timestamp=datetime.utcnow().isoformat(),
        footer={"text": "My App"},
        fields=[
            {"name": "Commit", "value": f"```{commit[:7]}```", "inline": True},
            {"name": "Deployed By", "value": deployed_by, "inline": True},
            {"name": "Duration", "value": duration, "inline": True},
        ],
    )
    
    if url:
        embed.url = url
    
    return embed


def send_deployment_notification(
    client: DiscordClient,
    environment: str,
    status: str,
    commit: str,
    deployed_by: str,
    duration: str,
) -> dict:
    """Send a deployment notification."""
    embed = create_deployment_embed(
        environment=environment,
        status=status,
        commit=commit,
        deployed_by=deployed_by,
        duration=duration,
    )
    
    return client.send_embed(embed)
```

### Error Alert

```python
def create_error_embed(
    error_type: str,
    message: str,
    file: str,
    line: int,
    user_id: Optional[str] = None,
    url: Optional[str] = None,
) -> DiscordEmbed:
    """Create an embed for error alerts."""
    
    embed = DiscordEmbed(
        title="⚠️ Error Alert",
        description=f"**{error_type}**",
        color=0xFF0000,  # Red
        timestamp=datetime.utcnow().isoformat(),
        fields=[
            {"name": "Message", "value": f"```{message[:500]}```"},
            {"name": "Location", "value": f"`{file}:{line}`"},
        ],
    )
    
    if user_id:
        embed.fields.append(
            {"name": "User ID", "value": user_id, "inline": True}
        )
    
    if url:
        embed.url = url
    
    return embed
```

### User Activity Summary

```python
def create_activity_summary_embed(
    total_signups: int,
    active_users: int,
    revenue: float,
    changes: dict,
) -> DiscordEmbed:
    """Create an embed showing daily activity summary."""
    
    embed = DiscordEmbed(
        title="📊 Daily Activity Summary",
        description=f"Activity for {datetime.now().strftime('%Y-%m-%d')}",
        color=0x5865F2,
        timestamp=datetime.utcnow().isoformat(),
        fields=[
            {
                "name": "📈 Key Metrics",
                "value": (
                    f"**New Signups:** {total_signups}\n"
                    f"**Active Users:** {active_users}\n"
                    f"**Revenue:** ${revenue:,.2f}"
                ),
                "inline": True,
            },
        ],
    )
    
    # Add change indicators
    changes_text = ""
    for metric, change in changes.items():
        emoji = "📈" if change > 0 else "📉"
        changes_text += f"{emoji} {metric}: {change:+.1f}%\n"
    
    if changes_text:
        embed.fields.append({
            "name": "📊 Changes (vs yesterday)",
            "value": changes_text,
            "inline": True,
        })
    
    return embed
```

## FastAPI Integration

Create webhook endpoints in FastAPI:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional


app = FastAPI()

# Discord client
discord_config = DiscordConfig(
    webhook_url=os.environ["DISCORD_WEBHOOK_URL"],
    bot_username="My App Alerts",
)
discord_client = DiscordClient(discord_config)


class DeploymentPayload(BaseModel):
    """Payload for deployment notifications."""
    environment: str
    status: str  # success, failed, in_progress
    commit: str
    deployed_by: str
    duration: str


class ErrorPayload(BaseModel):
    """Payload for error notifications."""
    error_type: str
    message: str
    file: str
    line: int
    user_id: Optional[str] = None


@app.post("/api/webhooks/deployment")
async def deployment_notification(payload: DeploymentPayload) -> dict:
    """Send deployment notification to Discord."""
    embed = create_deployment_embed(
        environment=payload.environment,
        status=payload.status,
        commit=payload.commit,
        deployed_by=payload.deployed_by,
        duration=payload.duration,
    )
    
    result = discord_client.send_embed(embed)
    
    if not result.get("success"):
        raise HTTPException(
            status_code=500,
            detail="Failed to send Discord notification"
        )
    
    return {"success": True}


@app.post("/api/webhooks/error")
async def error_notification(payload: ErrorPayload) -> dict:
    """Send error alert to Discord."""
    embed = create_error_embed(
        error_type=payload.error_type,
        message=payload.message,
        file=payload.file,
        line=payload.line,
        user_id=payload.user_id,
    )
    
    result = discord_client.send_embed(embed)
    return {"success": result.get("success", False)}


@app.post("/api/webhooks/custom")
async def custom_notification(
    content: str,
    title: str = "",
    color: int = 0x5865F2,
) -> dict:
    """Send custom notification to Discord."""
    if title:
        embed = DiscordEmbed(
            title=title,
            description=content,
            color=color,
        )
        result = discord_client.send_embed(embed)
    else:
        result = discord_client.send_message(content)
    
    return result
```

## Interactive Components (Buttons & Selects)

Discord supports interactive components in webhooks:

```python
def send_message_with_buttons(
    self,
    content: str,
    title: str = "",
) -> dict:
    """Send a message with action buttons."""
    
    payload = {
        "content": content,
        "username": self.config.bot_username,
        "components": [
            {
                "type": 1,  # Action row
                "components": [
                    {
                        "type": 2,  # Button
                        "style": 1,  # Primary (blue)
                        "label": "View Dashboard",
                        "url": "https://yourapp.com/dashboard",
                    },
                    {
                        "type": 2,
                        "style": 3,  # Success (green)
                        "label": "View Logs",
                        "url": "https://yourapp.com/logs",
                    },
                ],
            }
        ],
    }
    
    if title:
        payload["embeds"] = [{
            "title": title,
            "color": 0x5865F2,
        }]
    
    response = requests.post(self.config.webhook_url, json=payload)
    return {"success": response.status_code == 204}
```

## Summary

- Discord webhooks use simple HTTP POST requests
- Create rich messages with Discord embeds
- Use color coding (green for success, red for errors)
- Include timestamps and contextual information
- Discord webhooks are great for: deployment notifications, error alerts, user activity summaries, and team updates

## Next Steps

→ Continue to `08-slack-integration.md` to learn about Slack integration.
