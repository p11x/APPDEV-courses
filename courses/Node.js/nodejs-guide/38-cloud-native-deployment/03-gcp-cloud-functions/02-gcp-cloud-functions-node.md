# GCP Cloud Functions with Node.js

## What You'll Learn

- How to create HTTP Cloud Functions
- How to implement background functions
- How to handle function context and events
- How to manage function dependencies

---

## Layer 1: Function Types

### HTTP Function

```javascript
exports.helloHttp = (req, res) => {
  const name = req.query.name || req.body.name || 'World';
  
  res.status(200).json({
    message: `Hello, ${name}!`,
    timestamp: new Date().toISOString()
  });
};
```

### Background Function

exports.helloBackground = (data, context) => {
  console.log('Function triggered:', context.eventId);
  console.log('Event type:', context.eventType);
  console.log('Timestamp:', context.timestamp);
  console.log('Data:', data);
};
```

---

## Next Steps

Continue to [GCP Cloud Functions Triggers](./03-gcp-cloud-functions-triggers.md)