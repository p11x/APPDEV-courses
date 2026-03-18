# Slack Integration

## What You'll Learn

- How to send messages to Slack channels
- How to create rich Slack messages with blocks
- How to handle interactive components
- How to create Slack slash commands

## Prerequisites

- Completed `07-discord-webhook-integration.md`
- A Slack workspace

## Introduction

Slack is the most popular team communication tool. Integrating your application with Slack allows you to send notifications, create interactive workflows, and receive commands from users.

## Setting Up Slack

Slack offers multiple integration methods:

1. **Incoming Webhooks** — Simple way to send messages
2. **Slack Bot Users** — More interactive with buttons, menus, etc.
3. **Slack API** — Full access to all Slack features

Install the Slack SDK:

```bash
pip install slack-sdk
```

## Using Incoming Webhooks

The simplest integration:

```python
import os
import requests
from dataclasses import dataclass
from typing import Optional
from datetime import datetime


@dataclass
class SlackConfig:
    """Configuration for Slack webhook."""
    webhook_url: str
    channel: Optional[str] = None
    username: str = "My App"


@dataclass
class SlackBlock:
    """A Slack block component."""
    type: str
    text: Optional[dict] = None
    accessory: Optional[dict] = None
    elements: Optional[list] = None
    fields: Optional[list] = None
    
    def to_dict(self) -> dict:
        data = {"type": self.type}
        if self.text:
            data["text"] = self.text
        if self.accessory:
            data["accessory"] = self.accessory
        if self.elements:
            data["elements"] = self.elements
        if self.fields:
            data["fields"] = self.fields
        return data


class SlackClient:
    """Client for sending messages to Slack."""
    
    def __init__(self, config: SlackConfig) -> None:
        self.config = config
    
    def send_message(
        self,
        text: str,
        channel: Optional[str] = None,
        username: Optional[str] = None,
        icon_emoji: Optional[str] = None,
        blocks: Optional[list[SlackBlock]] = None,
    ) -> dict:
        """Send a message to Slack."""
        payload = {
            "text": text,
            "channel": channel or self.config.channel,
            "username": username or self.config.username,
        }
        
        if icon_emoji:
            payload["icon_emoji"] = icon_emoji
        
        if blocks:
            payload["blocks"] = [block.to_dict() for block in blocks]
        
        response = requests.post(
            self.config.webhook_url,
            json=payload,
        )
        
        return {
            "success": response.status_code == 200,
            "status_code": response.status_code,
        }
    
    def send_embed(
        self,
        title: str,
        text: str,
        color: str = "#5865F2",
        fields: Optional[list[dict]] = None,
    ) -> dict:
        """Send a message with attachment (legacy format)."""
        attachment = {
            "color": color,
            "title": title,
            "text": text,
            "fields": fields or [],
            "footer": "My App",
            "ts": int(datetime.utcnow().timestamp()),
        }
        
        payload = {
            "text": title,
            "attachments": [attachment],
            "username": self.config.username,
        }
        
        response = requests.post(self.config.webhook_url, json=payload)
        return {"success": response.status_code == 200}


# Example usage
def main() -> None:
    config = SlackConfig(
        webhook_url=os.environ["SLACK_WEBHOOK_URL"],
        channel="#alerts",
        username="My App",
    )
    
    client = SlackClient(config)
    
    # Simple message
    result = client.send_message(
        text="Hello from my app!",
        icon_emoji=":wave:",
    )
    print(f"Message sent: {result}")
    
    # Rich message with fields
    result = client.send_embed(
        title="New User Signup",
        text="A new user has registered",
        fields=[
            {"title": "Email", "value": "user@example.com", "short": True},
            {"title": "Plan", "value": "Pro", "short": True},
        ],
    )
    print(f"Embed sent: {result}")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `pip install slack-sdk` — The official Slack SDK for Python.
2. `SlackConfig` — Configuration holding webhook URL, default channel, and username.
3. `SlackBlock` — Dataclass representing a Slack block (modular message component).
4. `send_message()` — Sends a message to Slack via webhook. Supports text, channel override, custom username, emoji icon, and block kit.
5. `icon_emoji` — The emoji to use as the bot icon (e.g., ":wave:", ":rocket:", ":warning:").
6. `blocks` — Slack Block Kit components for rich layouts.

## Using Slack Block Kit

Block Kit is Slack's modern message building system:

```python
def create_deployment_block(
    environment: str,
    status: str,
    commit: str,
    deployed_by: str,
) -> list[SlackBlock]:
    """Create a Slack block for deployment notifications."""
    
    # Determine emoji and color based on status
    status_emoji = {
        "success": ":white_check_mark:",
        "failed": ":x:",
        "in_progress": ":hourglass_flowing_sand:",
    }
    
    emoji = status_emoji.get(status, ":question:")
    color = "#00FF00" if status == "success" else "#FF0000" if status == "failed" else "#FFFF00"
    
    blocks = [
        SlackBlock(
            type="header",
            text={
                "type": "plain_text",
                "text": f"{emoji} Deployment {status.title()}",
            },
        ),
        SlackBlock(
            type="section",
            fields=[
                {
                    "type": "mrkdwn",
                    "text": f"*Environment:*\n{environment}",
                },
                {
                    "type": "mrkdwn",
                    "text": f"*Deployed by:*\n{deployed_by}",
                },
                {
                    "type": "mrkdwn",
                    "text": f"*Commit:*\n```{commit[:7]}```",
                },
            ],
        ),
        SlackBlock(
            type="actions",
            elements=[
                {
                    "type": "button",
                    "text": {
                        "type": "plain_text",
                        "text": "View Dashboard",
                    },
                    "url": "https://yourapp.com/dashboard",
                    "style": "primary",
                },
                {
                    "type": "button",
                    "text": {
                        "type": "plain_text",
                        "text": "View Logs",
                    },
                    "url": "https://yourapp.com/logs",
                },
            ],
        ),
    ]
    
    return blocks


def send_deployment_notification(
    client: SlackClient,
    environment: str,
    status: str,
    commit: str,
    deployed_by: str,
) -> dict:
    """Send a deployment notification."""
    blocks = create_deployment_block(
        environment=environment,
        status=status,
        commit=commit,
        deployed_by=deployed_by,
    )
    
    return client.send_message(
        text=f"Deployment to {environment} {status}",
        blocks=blocks,
    )
```

## Using the Slack SDK (Bot User)

For more advanced features, use a Slack Bot:

```python
from slack_sdk import WebClient
from slack_sdk.errors import SlackApiError
from slack_sdk.signature import SignatureVerifier
from slack_sdk.models.blocks import (
    SectionBlock,
    DividerBlock,
    ButtonBlock,
    StaticSelectBlock,
)


class SlackBotClient:
    """Advanced Slack client using Bot User."""
    
    def __init__(self, bot_token: str, signing_secret: str) -> None:
        self.client = WebClient(token=bot_token)
        self.signing_secret = signing_secret
        self.signature_verifier = SignatureVerifier(signing_secret)
    
    def send_message(
        self,
        channel: str,
        text: str,
        blocks: Optional[list] = None,
    ) -> dict:
        """Send a message to a channel."""
        try:
            response = self.client.chat_postMessage(
                channel=channel,
                text=text,
                blocks=blocks,
            )
            return {"success": True, "message_id": response["ts"]}
        except SlackApiError as e:
            return {"success": False, "error": str(e)}
    
    def send_interactive_message(
        self,
        channel: str,
        text: str,
    ) -> dict:
        """Send a message with interactive components."""
        blocks = [
            SectionBlock(
                text="Select an option:",
                accessory=StaticSelectBlock(
                    placeholder="Choose an option",
                    options=[
                        {"text": "Option 1", "value": "option_1"},
                        {"text": "Option 2", "value": "option_2"},
                        {"text": "Option 3", "value": "option_3"},
                    ],
                ),
            ),
            DividerBlock(),
            SectionBlock(
                text="Or click a button:",
                accessory=ButtonBlock(
                    text="Click Me",
                    action_id="button_click",
                    value="clicked",
                ),
            ),
        ]
        
        return self.send_message(channel, text, blocks)
    
    def update_message(
        self,
        channel: str,
        ts: str,
        text: str,
        blocks: Optional[list] = None,
    ) -> dict:
        """Update an existing message."""
        try:
            response = self.client.chat_update(
                channel=channel,
                ts=ts,
                text=text,
                blocks=blocks,
            )
            return {"success": True}
        except SlackApiError as e:
            return {"success": False, "error": str(e)}
    
    def verify_request(
        self,
        timestamp: str,
        body: str,
        signature: str,
    ) -> bool:
        """Verify that a request came from Slack."""
        return self.signature_verifier.is_valid_request(
            body, {"x-slack-signature": signature, "x-slack-request-timestamp": timestamp}
        )


# Example usage with Bot Token
def main() -> None:
    client = SlackBotClient(
        bot_token=os.environ["SLACK_BOT_TOKEN"],
        signing_secret=os.environ["SLACK_SIGNING_SECRET"],
    )
    
    # Send a message
    result = client.send_message(
        channel="#alerts",
        text="Hello from the Slack Bot!",
    )
    print(f"Message sent: {result}")
```

## Slack Slash Commands

Handle slash commands from users:

```python
from fastapi import FastAPI, HTTPException, Request, Header
from pydantic import BaseModel
from typing import Optional


app = FastAPI()

# Your Slack app credentials
SLACK_SIGNING_SECRET = os.environ["SLACK_SIGNING_SECRET"]
slack_client = SlackBotClient(
    bot_token=os.environ["SLACK_BOT_TOKEN"],
    signing_secret=SLACK_SIGNING_SECRET,
)


class SlashCommandPayload(BaseModel):
    """Payload from a Slack slash command."""
    command: str
    text: str
    user_id: str
    channel_id: str
    response_url: str


@app.post("/api/slack/commands")
async def handle_slash_command(
    request: Request,
    x_slack_signature: str = Header(...),
    x_slack_request_timestamp: str = Header(...),
) -> dict:
    """Handle incoming slash command."""
    
    # Verify the request came from Slack
    body = await request.body()
    body_str = body.decode("utf-8")
    
    # Note: In production, verify the signature properly
    # This is a simplified version
    
    form = await request.form()
    
    command = form.get("command")
    text = form.get("text")
    user_id = form.get("user_id")
    response_url = form.get("response_url")
    
    # Parse the command
    match command:
        case "/status":
            return await handle_status_command(text, user_id)
        case "/deploy":
            return await handle_deploy_command(text, user_id)
        case "/help":
            return await handle_help_command()
        case _:
            return {
                "text": f"Unknown command: {command}",
                "response_type": "ephemeral",
            }


async def handle_status_command(text: str, user_id: str) -> dict:
    """Handle /status command."""
    # Extract service name from text
    service = text.strip() or "all"
    
    # Build status message
    message = f":white_check_mark: Status for *{service}*:\n"
    message += "- API: Operational\n"
    message += "- Database: Operational\n"
    message += "- Cache: Operational"
    
    return {
        "text": message,
        "response_type": "ephemeral",  # Only visible to user
    }


async def handle_deploy_command(text: str, user_id: str) -> dict:
    """Handle /deploy command."""
    parts = text.split()
    
    if len(parts) < 2:
        return {
            "text": "Usage: /deploy <environment> <version>",
            "response_type": "ephemeral",
        }
    
    environment = parts[0]
    version = parts[1]
    
    # Acknowledge immediately
    # In production, start async deployment
    
    return {
        "text": f":rocket: Starting deployment of `{version}` to *{environment}*...",
        "response_type": "in_channel",  # Visible to everyone
    }


async def handle_help_command() -> dict:
    """Handle /help command."""
    return {
        "text": "Available commands:\n"
        "- `/status [service]` - Check service status\n"
        "- `/deploy <env> <version>` - Deploy a version\n"
        "- `/help` - Show this help",
        "response_type": "ephemeral",
    }
```

## Interactive Components (Buttons & Menus)

Handle button clicks and menu selections:

```python
@app.post("/api/slack/interactive")
async def handle_interactive(
    request: Request,
) -> dict:
    """Handle interactive component actions."""
    
    form = await request.form()
    payload = form.get("payload")
    
    # Parse the payload (it's JSON stringified)
    import json
    data = json.loads(payload)
    
    action_id = data.get("action_id")
    user_id = data.get("user", {}).get("id")
    callback_id = data.get("callback_id")
    
    match action_id:
        case "button_click":
            # Handle button click
            return {
                "response_action": "update",
                "message": data.get("message"),
                "blocks": [
                    {
                        "type": "section",
                        "text": {
                            "type": "mrkdwn",
                            "text": f":white_check_mark: Button clicked by <@{user_id}>!",
                        },
                    }
                ],
            }
        
        case "menu_select":
            # Handle menu selection
            selected_value = data.get("actions", [{}])[0].get("selected_option", {}).get("value")
            return {
                "response_action": "update",
                "message": data.get("message"),
                "blocks": [
                    {
                        "type": "section",
                        "text": {
                            "type": "mrkdwn",
                            "text": f"Selected: *{selected_value}* by <@{user_id}>",
                        },
                    }
                ],
            }
        
        case _:
            return {"text": "Unknown action"}
```

## Summary

- Slack offers multiple integration methods: webhooks, bot users, and full API
- Incoming webhooks are simple for sending notifications
- Slack Block Kit provides rich, interactive messages
- Bot users can update messages and handle interactive components
- Slash commands let users interact with your app from Slack
- Always verify requests came from Slack using signing secrets

## Next Steps

→ Continue to `09-webhook-security-best-practices.md` to learn about securing your API integrations.
