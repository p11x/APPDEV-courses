# AWS Step Functions

## What You'll Learn

- How to orchestrate workflows with Step Functions
- How to implement state machines for complex business logic
- How to handle error handling and retry policies
- How to integrate with Lambda and other AWS services

---

## Layer 1: Academic Foundation

### Workflow Orchestration

Step Functions provides visual workflow orchestration for distributed applications.

**State Types:**
- **Pass**: Pass input to output
- **Task**: Execute Lambda or activity
- **Choice**: Branch based on conditions
- **Parallel**: Execute branches concurrently
- **Map**: Iterate over items
- **Wait**: Delay execution

---

## Layer 2: Code Evolution

### State Machine Definition

```json
{
  "Comment": "Order Processing State Machine",
  "StartAt": "ValidateOrder",
  "States": {
    "ValidateOrder": {
      "Type": "Task",
      "Resource": "arn:aws:lambda:us-east-1:123456789012:function:validateOrder",
      "Next": "CheckInventory"
    },
    "CheckInventory": {
      "Type": "Task",
      "Resource": "arn:aws:lambda:us-east-1:123456789012:function:checkInventory",
      "Retry": [{
        "ErrorEquals": ["States.ALL"],
        "MaxAttempts": 3,
        "IntervalSeconds": 2,
        "BackoffRate": 2
      }],
      "Next": "ProcessPayment"
    },
    "ProcessPayment": {
      "Type": "Task",
      "Resource": "arn:aws:lambda:us-east-1:123456789012:function:processPayment",
      "Catch": [{
        "ErrorEquals": ["PaymentFailed"],
        "Next": "NotifyFailure"
      }],
      "Next": "FulfillOrder"
    },
    "FulfillOrder": {
      "Type": "Parallel",
      "Branches": [
        {
          "StartAt": "ShipOrder",
          "States": {
            "ShipOrder": {
              "Type": "Task",
              "Resource": "arn:aws:lambda:us-east-1:123456789012:function:shipOrder",
              "End": true
            }
          }
        },
        {
          "StartAt": "SendConfirmation",
          "States": {
            "SendConfirmation": {
              "Type": "Task",
              "Resource": "arn:aws:lambda:us-east-1:123456789012:function:sendConfirmation",
              "End": true
            }
          }
        }
      ],
      "Next": "CompleteOrder"
    },
    "NotifyFailure": {
      "Type": "Task",
      "Resource": "arn:aws:lambda:us-east-1:123456789012:function:notifyFailure",
      "End": true
    },
    "CompleteOrder": {
      "Type": "Pass",
      "End": true
    }
  }
}
```

---

## Layer 3: Performance

### Execution Patterns

| Pattern | Use Case |
|---------|----------|
| Sequential | Linear workflows |
| Parallel | Concurrent tasks |
| Map | Batch processing |
| Choice | Conditional logic |

---

## Layer 4: Security

### IAM Role

```json
{
  "Version": "2012-10-17",
  "Statement": [{
    "Effect": "Allow",
    "Action": ["lambda:InvokeFunction"],
    "Resource": ["arn:aws:lambda:us-east-1:123456789012:function:*"]
  }]
}
```

---

## Next Steps

Continue to [AWS Lambda Powertools](./08-aws-lambda-powertools.md) for Node.js utilities.