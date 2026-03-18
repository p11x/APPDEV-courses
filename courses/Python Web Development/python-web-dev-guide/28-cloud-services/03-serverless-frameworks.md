# Serverless Frameworks

## What You'll Learn
- Serverless Framework
- AWS SAM
- Chalice

## Prerequisites
- Completed AWS services

## Serverless Framework

```bash
npm install -g serverless
```

```yaml
# serverless.yml
service: my-app

provider:
  name: aws
  runtime: python3.11

functions:
  hello:
    handler: handler.hello
    events:
      - http:
          path: /
          method: get
```

## AWS Chalice

```bash
pip install chalice
```

```python
from chalice import Chalice

app = Chalice(app_name='myapp')

@app.route('/')
def index():
    return {'hello': 'world'}
```

## Summary
- Simplify serverless deployments
- Choose one framework
- Test locally first

## Next Steps
→ Continue to `04-gcp-and-azure.md`
