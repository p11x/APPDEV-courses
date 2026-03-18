<!-- FILE: 14_email_and_notifications/04_transactional_email_services/03_choosing_an_email_provider.md -->

## Overview

Compare email providers for your Flask application.

## Comparison

| Provider | Free Tier | Pros | Cons |
|----------|-----------|------|------|
| SendGrid | 100/day | Easy API, good docs | Pricey at scale |
| Mailgun | 5,000/month | Great for devs | Complicated pricing |
| AWS SES | 62,000/month | Cheapest | Harder setup |
| Postmark | 100/month | High deliverability | Limited free tier |

## Recommendation

- **Startups**: Mailgun or SendGrid
- **High volume**: AWS SES
- **Deliverability focus**: Postmark
