# GCP Cloud Functions Monitoring

## What You'll Learn

- How to monitor Cloud Functions
- How to configure logging and metrics
- How to set up alerts
- How to analyze performance

---

## Layer 1: Monitoring

### Stackdriver Integration

```javascript
const { logger } = require('@google-cloud/logging');

const log = logger();

exports.helloHttp = (req, res) => {
  log.info('Function called', {
    method: req.method,
    path: req.path
  });
  
  res.status(200).json({ success: true });
};
```

---

## Next Steps

Continue to [GCP vs AWS](./06-gcp-cloud-functions-vs-aws.md)