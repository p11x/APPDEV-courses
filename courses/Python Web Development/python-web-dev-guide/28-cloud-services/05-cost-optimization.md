# Cost Optimization

## What You'll Learn
- Reducing costs
- Right-sizing
- Reserved instances

## Prerequisites
- Completed GCP and Azure

## Tips

1. **Use serverless** - Pay only for usage
2. **Right-size resources** - Don't over-provision
3. **Use spot instances** - 70-90% discount
4. **Set budgets** - Get alerts
5. **Use free tier** - AWS/GCP/Azure all have free tiers

## Budget Alerts

```python
import boto3

budgets = boto3.client('budgets')

budgets.create_budget(
    AccountId='123456789',
    Budget={
        'BudgetName': 'monthly-limit',
        'BudgetLimit': {'Amount': '100', 'Unit': 'USD'},
        'TimeUnit': 'MONTHLY'
    },
    NotificationsWithSubscribers=[
        {
            'Notification': {
                'NotificationType': 'ACTUAL',
                'Threshold': 80
            },
            'Subscribers': [
                {'SubscriptionType': 'EMAIL', 'Address': 'you@example.com'}
            ]
        }
    ]
)
```

## Summary
- Monitor costs regularly
- Use budgets and alerts
- Right-size resources

## Next Steps
→ Move to `29-scraping-and-automation/`
