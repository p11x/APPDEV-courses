# Alerts and Notifications

## What You'll Learn
- Create alerts in Sentry
- Configure notification channels
- Set up alert rules

## Prerequisites
- Sentry project set up

## Do I Need This Right Now?
Alerts ensure you know about errors immediately. Without them, you'd have to check Sentry constantly to see if anything broke.

## Concept Explained Simply

Alerts are like smoke detectors — they notify you when something goes wrong. You configure when they trigger and how they reach you (email, Slack, PagerDuty).

## Creating Alerts

### In Sentry Dashboard

1. Go to **Alerts** → **Create Alert Rule**
2. Choose alert type:
   - **Errors** - when error count crosses threshold
   - **Performance** - when latency exceeds threshold
   - **Quota** - when usage approaches limits

### Alert Types

#### Error Alert

```
IF:
  Count of errors 
  IS GREATER THAN 
  5 
  in 1 hour
  
THEN:
  Send email to Team
  Send Slack message to #alerts
```

#### Performance Alert

```
IF:
  p95 Latency 
  IS GREATER THAN 
  2000ms 
  for /api/users
  
THEN:
  Send email to Team
```

## Configuring Notification Channels

### Email

1. Go to **Settings** → **Notifications**
2. Add email addresses
3. Configure which alerts send email

### Slack

1. Go to **Settings** → **Integrations**
2. Install Slack integration
3. Choose channel for alerts

```yaml
# Sentry Slack Integration
Alert Type: Error
Channel: #alerts
Message: |
  🔴 *New Error Alert*
  *{project}* has {count} errors
  {error.title}
  <{url}|View in Sentry>
```

### PagerDuty

1. Go to **Settings** → **Integrations**
2. Install PagerDuty
3. Map alert severities to PagerDuty

## Alert Rules

### Critical Alert (High Priority)

```yaml
Name: Critical Errors
Condition: 
  - Event count > 0
  - Level: error or fatal
Environment: production
Channels: 
  - Slack #critical
  - Email on-call
  - PagerDuty (high)
```

### Warning Alert (Medium Priority)

```yaml
Name: Error Spike
Condition:
  - Event count > 10
  - In 5 minutes
Environment: production
Channels:
  - Slack #warnings
```

### Performance Alert

```yaml
Name: Slow Page Loads
Condition:
  - p95 > 3000ms
  - Page loads
Environment: production
Channels:
  - Slack #performance
```

## Code-Based Alerts

You can also trigger alerts from code:

```typescript
import * as Sentry from '@sentry/nextjs';

// Trigger an alert manually
Sentry.captureMessage(
  'High error rate detected!',
  'warning' // This will trigger warning alerts
);

// Or trigger based on threshold
async function checkHealth() {
  const errorCount = await getRecentErrorCount();
  
  if (errorCount > 100) {
    Sentry.captureMessage(
      'Error rate above threshold',
      'error'
    );
  }
}
```

## Common Mistakes

### Mistake #1: Too Many Alerts
```typescript
// Wrong: Alert on every error!
IF: Count > 0
// Team gets spammed!
```

```typescript
// Correct: Reasonable threshold
IF: Count > 10 in 1 hour
// Only alerts on real issues
```

### Mistake #2: No Alert on Production
```typescript
// Wrong: Only test in development
IF: Environment = development
// No alerts when it matters!
```

```typescript
// Correct: Monitor production
IF: Environment = production
// Get notified of real issues
```

### Mistake #3: Wrong Channels
```typescript
// Wrong: Low priority channel for critical alerts
IF: Level = fatal
THEN: Send to #low-priority
// Might be missed!
```

```typescript
// Correct: Critical alerts to critical channel
IF: Level = fatal
THEN: Send to @oncall, PagerDuty
```

## Summary
- Create alerts in Sentry dashboard → Alerts → Create Alert Rule
- Configure thresholds to avoid alert fatigue
- Use multiple channels: email, Slack, PagerDuty
- Set up different alerts for different severity levels
- Monitor both errors and performance metrics
- Test alerts to ensure they work

## Next Steps
- [understanding-scores.md](../21-performance-auditing/01-lighthouse/understanding-scores.md) — Understanding performance scores
