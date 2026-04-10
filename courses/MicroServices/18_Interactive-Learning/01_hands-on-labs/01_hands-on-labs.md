# Interactive Learning: Hands-On Labs for Microservices

## Overview

Hands-on learning is essential for mastering microservices concepts. This guide provides interactive labs that allow developers to practice building, deploying, and managing microservices in a controlled environment. Each lab includes step-by-step instructions, expected outcomes, and validation checks.

The labs progress from foundational concepts to advanced patterns, enabling learners to build expertise incrementally. They can be completed individually or as part of a structured learning path.

## Lab Prerequisites

Before starting these labs, ensure you have:
- Docker Desktop installed
- Kubernetes (minikube or Docker Desktop Kubernetes)
- kubectl command-line tool
- An IDE with Python/Java support

## Lab 1: Building Your First Microservice

### Objectives
- Create a simple REST microservice
- Containerize the service
- Deploy to Kubernetes
- Verify functionality

### Step 1: Create the Service

```python
# Create app.py
from flask import Flask, jsonify, request

app = Flask(__name__)

# In-memory data store
products = [
    {"id": 1, "name": "Laptop", "price": 999.99},
    {"id": 2, "name": "Phone", "price": 599.99},
    {"id": 3, "name": "Tablet", "price": 399.99}
]

@app.route('/api/products', methods=['GET'])
def get_products():
    return jsonify(products)

@app.route('/api/products/<int:product_id>', methods=['GET'])
def get_product(product_id):
    product = next((p for p in products if p['id'] == product_id), None)
    if product:
        return jsonify(product)
    return jsonify({"error": "Product not found"}), 404

@app.route('/api/products', methods=['POST'])
def add_product():
    data = request.json
    new_id = max(p['id'] for p in products) + 1
    product = {"id": new_id, **data}
    products.append(product)
    return jsonify(product), 201

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
```

### Step 2: Containerize

```dockerfile
# Create Dockerfile
FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY app.py .

EXPOSE 5000

CMD ["python", "app.py"]
```

### Step 3: Deploy to Kubernetes

```yaml
# Create deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: product-service
spec:
  replicas: 2
  selector:
    matchLabels:
      app: product-service
  template:
    metadata:
      labels:
        app: product-service
    spec:
      containers:
      - name: product-service
        image: product-service:1.0
        ports:
        - containerPort: 5000
        resources:
          requests:
            memory: "128Mi"
            cpu: "250m"
          limits:
            memory: "256Mi"
            cpu: "500m"
---
apiVersion: v1
kind: Service
metadata:
  name: product-service
spec:
  selector:
    app: product-service
  ports:
  - port: 80
    targetPort: 5000
  type: ClusterIP
```

### Step 4: Verify

```bash
# Deploy the service
kubectl apply -f deployment.yaml

# Check pods
kubectl get pods -l app=product-service

# Test the service
kubectl port-forward svc/product-service 8080:80
curl http://localhost:8080/api/products
```

## Lab 2: Implementing Service Communication

### Objectives
- Create two services that communicate
- Implement REST client with retry logic
- Handle failures gracefully

### Implementation

```python
# order_service.py with communication
import requests
from requests.adapters import HTTPAdapter
from urllib3.util.retry import Retry

class ServiceClient:
    def __init__(self, service_url: str):
        self.service_url = service_url
        self.session = self._create_session()
    
    def _create_session(self):
        session = requests.Session()
        retry = Retry(
            total=3,
            backoff_factor=1,
            status_forcelist=[500, 502, 503, 504]
        )
        adapter = HTTPAdapter(max_retries=retry)
        session.mount('http://', adapter)
        session.mount('https://', adapter)
        return session
    
    def get(self, path: str):
        response = self.session.get(f"{self.service_url}{path}")
        response.raise_for_status()
        return response.json()
    
    def post(self, path: str, data: dict):
        response = self.session.post(
            f"{self.service_url}{path}",
            json=data
        )
        response.raise_for_status()
        return response.json()

# Use in service
class OrderService:
    def __init__(self):
        self.product_client = ServiceClient(
            "http://product-service/api"
        )
    
    def create_order(self, product_id: int, quantity: int):
        # Get product details
        product = self.product_client.get(f"/products/{product_id}")
        
        # Create order
        order = {
            "product_id": product_id,
            "product_name": product["name"],
            "quantity": quantity,
            "total": product["price"] * quantity
        }
        
        return order
```

## Lab 3: Adding Observability

### Objectives
- Add logging to services
- Configure Prometheus metrics
- Set up distributed tracing

### Metrics Implementation

```python
# metrics.py
from prometheus_client import Counter, Histogram, generate_latest
from flask import Response

# Define metrics
REQUEST_COUNT = Counter(
    'http_requests_total',
    'Total HTTP requests',
    ['method', 'endpoint', 'status']
)

REQUEST_LATENCY = Histogram(
    'http_request_duration_seconds',
    'HTTP request latency',
    ['method', 'endpoint']
)

def track_request(method, endpoint, status):
    REQUEST_COUNT.labels(
        method=method,
        endpoint=endpoint,
        status=status
    ).inc()

def track_latency(method, endpoint, duration):
    REQUEST_LATENCY.labels(
        method=method,
        endpoint=endpoint
    ).observe(duration)

@app.after_request
def metrics_middleware(response):
    track_request(
        request.method,
        request.path,
        response.status_code
    )
    return response

@app.route('/metrics')
def metrics():
    return Response(
        generate_latest(),
        mimetype='text/plain'
    )
```

## Validation Checklist

- [ ] Service responds to HTTP requests
- [ ] Container builds successfully
- [ ] Kubernetes deployment is healthy
- [ ] Service-to-service communication works
- [ ] Metrics are exposed on /metrics endpoint
- [ ] Logs are properly formatted

## Output Statement

```
Lab Completion Status
======================
Lab 1: First Microservice
- Status: COMPLETED
- Time: 45 minutes
- Validation: ALL PASSED

Lab 2: Service Communication
- Status: COMPLETED
- Time: 60 minutes
- Validation: ALL PASSED

Lab 3: Observability
- Status: COMPLETED
- Time: 30 minutes
- Validation: ALL PASSED

Overall Progress: 3/3 Labs Completed
```