# API Gateway Pattern

## Overview

The API Gateway pattern is a server-side architectural component that acts as a single entry point for a set of microservices. It serves as a reverse proxy, routing requests from clients to the appropriate backend services while providing cross-cutting functionality such as authentication, rate limiting, request logging, and response caching.

In a microservices architecture without an API Gateway, clients would need to:

1. Maintain knowledge of all service endpoints
2. Handle service discovery and load balancing
3. Implement authentication across multiple services
4. Manage cross-service data aggregation
5. Handle retries and circuit breaking individually

The API Gateway centralizes all these concerns, allowing backend services to focus purely on business logic. By acting as an abstraction layer, it protects microservices from direct exposure to external traffic and provides a stable interface that can evolve independently of backend implementations.

Modern API Gateways are essential components in cloud-native applications. They provide:

- **Request Routing**: Dynamic routing based on URL paths, headers, or query parameters
- **Service Aggregation**: Combining multiple backend calls into single responses
- **Protocol Translation**: Converting between REST, gRPC, GraphQL, and other protocols
- **Security**: Authentication, authorization, and API key management
- **Performance Optimization**: Caching, compression, and connection pooling
- **Observability**: Request logging, metrics collection, and distributed tracing

Popular API Gateway solutions include Kong, NGINX, AWS API Gateway, Azure API Management, and cloud-native solutions like Ambassador and Gloo.

## Flow Chart

```mermaid
flowchart TD
    Client[Client Application] -->|HTTP Request| AG[API Gateway]
    
    AG -->|Authentication| Auth[Auth Service]
    Auth -->|Token Valid| AG
    Auth -->|Token Invalid| AG2{401 Unauthorized}
    
    AG -->|Rate Limiting| RL[Rate Limiter]
    RL -->|Allowed| AG3{200 OK}
    RL -->|Rate Limited| AG4{429 Too Many Requests}
    
    AG -->|Route & Transform| Router[Request Router]
    Router -->|Path: /users/**| UserSvc[User Service]
    Router -->|Path: /orders/**| OrderSvc[Order Service]
    Router -->|Path: /products/**| ProductSvc[Product Service]
    
    UserSvc -->|User Data| AG5[Response Aggregator]
    OrderSvc -->|Order Data| AG5
    ProductSvc -->|Product Data| AG5
    
    AG5 -->|Transform Response| Transform[Response Transformer]
    Transform -->|JSON Response| Client
    
    style AG fill:#4CAF50
    style Auth fill:#2196F3
    style RL fill:#FF9800
    style Router fill:#9C27B0
```

This flow demonstrates the request pipeline through an API Gateway, including authentication verification, rate limiting checks, routing to appropriate services, and response aggregation.

## Standard Example

### Building a Simple API Gateway with Express.js

```javascript
// gateway.js - Simple API Gateway implementation
const express = require('express');
const axios = require('axios');
const app = express();

app.use(express.json());

// Service registry with dynamic endpoints
const serviceRegistry = {
    users: 'http://users-service:3001',
    orders: 'http://orders-service:3002',
    products: 'http://products-service:3003',
    inventory: 'http://inventory-service:3004'
};

// Middleware: Authentication
async function authenticate(req, res, next) {
    const authHeader = req.headers.authorization;
    
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
        return res.status(401).json({ error: 'Missing or invalid authorization header' });
    }
    
    const token = authHeader.substring(7);
    
    try {
        const response = await axios.post('http://auth-service:3100/validate', { token });
        req.user = response.data.user;
        next();
    } catch (error) {
        return res.status(401).json({ error: 'Invalid token' });
    }
}

// Middleware: Rate Limiting
const rateLimiter = new Map();

function rateLimit(req, res, next) {
    const clientId = req.user?.id || req.ip;
    const now = Date.now();
    const windowMs = 60000; // 1 minute window
    const maxRequests = 100;
    
    if (!rateLimiter.has(clientId)) {
        rateLimiter.set(clientId, { count: 0, resetTime: now + windowMs });
    }
    
    const clientData = rateLimiter.get(clientId);
    
    if (now > clientData.resetTime) {
        clientData.count = 0;
        clientData.resetTime = now + windowMs;
    }
    
    clientData.count++;
    
    if (clientData.count > maxRequests) {
        return res.status(429).json({ 
            error: 'Rate limit exceeded',
            retryAfter: Math.ceil((clientData.resetTime - now) / 1000)
        });
    }
    
    res.setHeader('X-RateLimit-Limit', maxRequests);
    res.setHeader('X-RateLimit-Remaining', maxRequests - clientData.count);
    next();
}

// Route handler with error handling
async function proxyRequest(req, res) {
    const { service, path } = req.params;
    const serviceUrl = serviceRegistry[service];
    
    if (!serviceUrl) {
        return res.status(404).json({ error: 'Service not found' });
    }
    
    try {
        const targetUrl = `${serviceUrl}/${path}`;
        const response = await axios({
            method: req.method,
            url: targetUrl,
            headers: {
                ...req.headers,
                host: undefined,
                'x-forwarded-for': req.ip,
                'x-forwarded-proto': req.protocol
            },
            data: req.body,
            params: req.query,
            timeout: 5000
        });
        
        res.status(response.status).json(response.data);
    } catch (error) {
        if (error.code === 'ECONNABORTED') {
            return res.status(504).json({ error: 'Gateway timeout' });
        }
        if (error.response) {
            return res.status(error.response.status).json(error.response.data);
        }
        res.status(502).json({ error: 'Bad gateway' });
    }
}

// Aggregation endpoint
async function aggregateUserOrders(req, res) {
    const userId = req.user.id;
    
    try {
        const [userResponse, ordersResponse] = await Promise.all([
            axios.get(`${serviceRegistry.users}/users/${userId}`),
            axios.get(`${serviceRegistry.orders}/orders?userId=${userId}`)
        ]);
        
        const aggregated = {
            user: userResponse.data,
            orders: ordersResponse.data,
            totalOrders: ordersResponse.data.length
        };
        
        res.json(aggregated);
    } catch (error) {
        res.status(502).json({ error: 'Failed to aggregate data' });
    }
}

// Apply middleware
app.use('/api/:service/*', authenticate);
app.use('/api/:service/*', rateLimit);

// Register routes
app.all('/api/users/*', (req, res) => proxyRequest(req, res));
app.all('/api/orders/*', (req, res) => proxyRequest(req, res));
app.all('/api/products/*', (req, res) => proxyRequest(req, res));

// Aggregated endpoint
app.get('/api/my-data', authenticate, rateLimit, aggregateUserOrders);

app.listen(3000, () => {
    console.log('API Gateway running on port 3000');
});
```

### Kong API Gateway Configuration

Kong is one of the most popular open-source API Gateways. Here's how to configure it:

```yaml
# docker-compose.yml for Kong
version: '3.8'

services:
  kong:
    image: kong:3.4
    environment:
      KONG_DATABASE: postgres
      KONG_PG_HOST: postgres
      KONG_PG_DATABASE: kong
      KONG_PG_USER: kong
      KONG_PG_PASSWORD: kong
      KONG_DECLARATIVE_CONFIG: /usr/local/kong/declarative.yml
    ports:
      - "8000:8000"
      - "8443:8443"
    volumes:
      - ./declarative.yml:/usr/local/kong/declarative.yml
    depends_on:
      - postgres

  postgres:
    image: postgres:15
    environment:
      POSTGRES_DB: kong
      POSTGRES_USER: kong
      POSTGRES_PASSWORD: kong
```

```yaml
# declarative.yml - Kong services and routes
_format_version: "3.0"

services:
  - name: user-service
    url: http://users-service:3001
    routes:
      - name: user-routes
        paths:
          - /users
        methods:
          - GET
          - POST
    plugins:
      - name: jwt
        config:
          key_claim_name: kid
      - name: rate-limiting
        config:
          minute: 100
          policy: local
          fault_tolerant: true

  - name: order-service
    url: http://orders-service:3002
    routes:
      - name: order-routes
        paths:
          - /orders
    plugins:
      - name: jwt
      - name: rate-limiting
        config:
          minute: 50
      - name: correlation-id
        config:
          header_name: X-Correlation-ID
          generator: uuid

  - name: product-service
    url: http://products-service:3003
    routes:
      - name: product-routes
        paths:
          - /products
    plugins:
      - name: key-auth
      - name: rate-limiting
        config:
          minute: 200

consumers:
  - username: mobile-app
    keyauth_credentials:
      - key: mobile-api-key-12345

  - username: web-app
    jwt_secrets:
      - key: jwt-key-001
        algorithm: RS256
```

### AWS API Gateway Configuration

AWS API Gateway provides managed infrastructure for API deployment:

```yaml
# serverless.yml - AWS API Gateway with Lambda
service: microservices-api

provider:
  name: aws
  runtime: nodejs18.x
  environment:
    USERS_TABLE: ${self:service}-users
    ORDERS_TABLE: ${self:service}-orders

functions:
  getUser:
    handler: handlers/getUser.handler
    events:
      - http:
          path: users/{userId}
          method: get
          cors: true
          authorizer:
            name: authorizer
            resultTtlInSeconds: 0
            identitySource: method.request.header.Authorization

  createUser:
    handler: handlers/createUser.handler
    events:
      - http:
          path: users
          method: post
          cors: true

  getOrders:
    handler: handlers/getOrders.handler
    events:
      - http:
          path: orders
          method: get
          cors: true

resources:
  Resources:
    ApiGatewayRestApi:
      Type: AWS::ApiGateway::RestApi
      Properties:
        Description: Microservices API Gateway
        EndpointConfiguration:
          Types:
            - REGIONAL

    UsagePlan:
      Type: AWS::ApiGateway::UsagePlan
      Properties:
        UsagePlanName: standard-plan
        Throttle:
          BurstLimit: 100
          RateLimit: 50
        Quota:
          Limit: 10000
          Period: MONTH

    ApiKey:
      Type: AWS::ApiGateway::ApiKey
      Properties:
        Description: API Key for clients
        Enabled: true
        StageKeys:
          - RestApiId: !Ref ApiGatewayRestApi
            StageName: !Ref ApiGatewayRestApi.DeploymentStage
```

```javascript
// authorizer.js - Custom Lambda authorizer
exports.handler = async (event) => {
    const token = event.authorizationToken;
    
    try {
        // Verify JWT token
        const decoded = jwt.verify(token, process.env.JWT_SECRET);
        
        return {
            principalId: decoded.sub,
            policyDocument: {
                Version: '2012-10-17',
                Statement: [
                    {
                        Action: 'execute-api:Invoke',
                        Effect: 'Allow',
                        Resource: event.methodArn
                    }
                ]
            },
            context: {
                userId: decoded.sub,
                role: decoded.role
            }
        };
    } catch (error) {
        return {
            principalId: 'unauthorized',
            policyDocument: {
                Version: '2012-10-17',
                Statement: [
                    {
                        Action: 'execute-api:Invoke',
                        Effect: 'Deny',
                        Resource: event.methodArn
                    }
                ]
            }
        };
    }
};
```

## Real-World Examples

### Example 1: E-Commerce Platform API Gateway

Large e-commerce platforms use API Gateways to handle millions of requests:

```javascript
// e-commerce-gateway.js
const Kong = require('kong-pongo');

class ECommerceGateway {
    constructor() {
        this.kong = new Kong({ baseUrl: 'http://localhost:8001' });
    }
    
    async configure() {
        // Configure product API with caching
        await this.kong.services.create({
            name: 'products',
            url: 'http://products-cluster.internal'
        });
        
        await this.kong.routes.create({
            service: { name: 'products' },
            paths: ['/v1/products'],
            methods: ['GET']
        });
        
        await this.kong.plugins.create({
            name: 'proxy-cache',
            config: {
                strategy: 'memory',
                cache_key: ['uri', 'querystring'],
                content_type: ['application/json'],
                ttl: 60,
                Vary: ['Accept']
            }
        });
        
        // Configure cart API with write-through caching
        await this.kong.services.create({
            name: 'cart',
            url: 'http://cart-service.internal'
        });
        
        await this.kong.routes.create({
            service: { name: 'cart' },
            paths: ['/v1/cart']
        });
        
        // Configure checkout with stricter rate limits
        await this.kong.plugins.create({
            name: 'rate-limiting',
            config: {
                minute: 10,
                hour: 100,
                policy: 'redis',
                redis_host: 'redis.internal'
            }
        });
        
        // Add request transformer for API versioning
        await this.kong.plugins.create({
            name: 'request-transformer',
            config: {
                add: {
                  headers: ['X-API-Version:1.0']
                }
            }
        });
    }
}
```

### Example 2: Banking API Gateway with High Security

Financial services require enhanced security:

```java
// Spring Cloud Gateway for Banking
@Configuration
public class BankingGatewayConfig {
    
    @Bean
    public RouteLocator customRouteLocator(RouteLocatorBuilder builder) {
        return builder.routes()
            .route("account-route", r -> r
                .path("/api/accounts/**")
                .filters(f -> f
                    .stripPrefix(1)
                    .addRequestHeader("X-Bank-ID", "PRIMARY")
                    .modifyRequestParameters(modify -> 
                        modify.replace("apiVersion", "v2"))
                    .circuitBreaker(config -> config
                        .setName("accountCircuitBreaker")
                        .setFallbackUri("forward:/fallback/account")))
                .uri("lb://account-service"))
            
            .route("transfer-route", r -> r
                .path("/api/transfers/**")
                .filters(f -> f
                    .stripPrefix(1)
                    .addRequestHeader("X-Request-ID", 
                        "{random.uuid}")
                    .modifyRequestParameters(modify -> 
                        modify.replace("requestTimestamp", 
                            "#{T(java.time.Instant).now()}")))
                .uri("lb://transfer-service"))
            
            .route("auth-route", r -> r
                .path("/api/auth/**")
                .filters(f -> f.stripPrefix(1))
                .uri("lb://auth-service"))
            
            .build();
    }
}

@Component
public class SecurityValidationFilter implements GatewayFilter {
    
    @Override
    public Mono<Void> filter(ServerWebExchange exchange, 
                            GatewayFilterChain chain) {
        ServerHttpRequest request = exchange.getRequest();
        
        // Validate required headers
        if (!request.getHeaders().containsKey("X-Client-ID")) {
            return unAuthorized(exchange, "Missing client identification");
        }
        
        // Validate request size
        if (request.getHeaders().getContentLength() > MAX_REQUEST_SIZE) {
            return badRequest(exchange, "Request too large");
        }
        
        // Log for audit
        auditLogService.log(request);
        
        return chain.filter(exchange);
    }
}
```

### Example 3: IoT Platform API Aggregation

IoT platforms aggregate data from multiple device types:

```python
# iot-gateway.py - IoT Platform API Gateway
from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
import asyncio

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

class IoTDataAggregator:
    def __init__(self):
        self.device_services = {
            'sensors': 'http://sensor-service:3001',
            'actuators': 'http://actuator-service:3002',
            'gateways': 'http://gateway-service:3003',
            'analytics': 'http://analytics-service:3004'
        }
    
    async def aggregate_dashboard(self, user_id: str):
        tasks = [
            self.get_device_summary(user_id),
            self.get_recent_alerts(user_id),
            self.get_energy_usage(user_id),
            self.get_active_sessions(user_id)
        ]
        
        results = await asyncio.gather(*tasks, return_exceptions=True)
        
        return {
            'deviceSummary': results[0] if not isinstance(results[0], Exception) else None,
            'alerts': results[1] if not isinstance(results[1], Exception) else [],
            'energyUsage': results[2] if not isinstance(results[2], Exception) else None,
            'activeSessions': results[3] if not isinstance(results[3], Exception) else []
        }
    
    async def get_device_summary(self, user_id: str):
        async with aiohttp.ClientSession() as session:
            async with session.get(
                f"{self.device_services['sensors']}/summary",
                params={'userId': user_id}
            ) as response:
                return await response.json()

@app.get("/api/dashboard/{user_id}")
async def get_dashboard(user_id: str):
    aggregator = IoTDataAggregator()
    return await aggregator.aggregate_dashboard(user_id)

# Device-specific routing with protocol translation
@app.get("/api/devices/{device_type}/{device_id}")
async def route_to_device(device_type: str, device_id: str):
    if device_type not in ['sensors', 'actuators', 'gateways']:
        raise HTTPException(404, "Device type not found")
    
    # Translate MQTT to HTTP
    response = await mqtt_to_http_proxy(
        device_type, 
        device_id, 
        'get_state'
    )
    
    return response
```

## Best Practices

### 1. Service Discovery Integration

Integrate the API Gateway with service discovery:

```java
@Configuration
public class ServiceDiscoveryConfig {
    
    @Bean
    public DiscoveryClientRouteDefinitionLocator 
            discoveryClientRouteDefinitionLocator(
            DiscoveryClient discoveryClient,
            DiscoveryLocatorProperties properties) {
        properties.setEnabled(true);
        properties.setLowerCaseServiceId(true);
        
        return new DiscoveryClientRouteDefinitionLocator(
            discoveryClient, 
            properties
        );
    }
}
```

### 2. Health Checking and Circuit Breaking

Implement comprehensive health checking:

```yaml
# Kong health check plugin configuration
plugins:
  - name: proxy-cache
    config:
      strategy: memory
  
  - name: circuit-breaker
    config:
      break_threshold: 5
      break_timeout: 10
      unhealthy:
        http_statuses:
          - 500
          - 502
          - 503
        tcp_failures: 3
        timeouts: 3
```

### 3. Request/Response Transformation

Use transformation middleware for API versioning:

```javascript
// Request transformation
app.use('/api/v2/*', (req, res, next) => {
    // Translate v1 response format to v2
    if (req.headers['accept-version'] === '1') {
        req.query.translate = true;
    }
    next();
});

// Response transformation
app.use('/api/*', (req, res, next) => {
    const originalJson = res.json;
    res.json = function(data) {
        if (req.query.translate && data.format === 'v1') {
            data = transformV1ToV2(data);
        }
        return originalJson.call(this, data);
    };
    next();
});
```

### 4. Logging and Observability

Implement distributed tracing:

```java
// Tracing filter
@Component
public class TracingFilter extends GatewayFilter {
    
    @Override
    public Mono<Void> filter(ServerWebExchange exchange,
                            GatewayFilterChain chain) {
        ServerHttpRequest request = exchange.getRequest();
        String traceId = request.getHeaders()
            .getFirst("X-Trace-ID");
        
        if (traceId == null) {
            traceId = UUID.randomUUID().toString();
        }
        
        ServerHttpRequest modifiedRequest = request.mutate()
            .header("X-Trace-ID", traceId)
            .build();
        
        return chain.filter(
            exchange.mutate().request(modifiedRequest).build()
        );
    }
}
```

### 5. API Key Management

Implement secure API key management:

```python
# Kong API key rotation
import hashlib
import secrets

def rotate_api_key(consumer_id: str):
    new_key = f"sk_{secrets.token_urlsafe(32)}"
    key_hash = hashlib.sha256(new_key.encode()).hexdigest()
    
    # Store hash, not plaintext
    store_key_hash(consumer_id, key_hash)
    
    return new_key

def verify_api_key(consumer_id: str, provided_key: str):
    stored_hash = get_stored_hash(consumer_id)
    provided_hash = hashlib.sha256(provided_key.encode()).hexdigest()
    
    return secrets.compare_digest(stored_hash, provided_hash)
```

### 6. Load Balancing Configuration

Configure load balancing strategies:

```yaml
# Kong load balancing configuration
services:
  - name: user-service
    url: http://user-service-1:3001
    load_balancer:
      hashing:
        header_name: X-Consistent-Hash
    
    routes:
      - name: user-routes
        paths: [ /users ]
```

### 7. SSL/TLS Termination

Implement proper SSL termination:

```yaml
# Kong SSL configuration
services:
  - name: secure-service
    url: http://internal-service:3001
    tls:
      enabled: true
      certificate_id: server-cert

certificates:
  - id: server-cert
    cert: |
      -----BEGIN CERTIFICATE-----
      ...
      -----END CERTIFICATE-----
    key: |
      -----BEGIN PRIVATE KEY-----
      ...
      -----END PRIVATE KEY-----
```

### 8. API Gateway as Code

Manage gateway configuration as code:

```yaml
# api-config.yaml - Infrastructure as Code
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: microservices-ingress
  annotations:
    nginx.ingress.kubernetes.io/proxy-read-timeout: "60"
    nginx.ingress.kubernetes.io/proxy-send-timeout: "60"
    nginx.ingress.kubernetes.io/proxy-body-size: "10m"
    nginx.ingress.kubernetes.io/rate-limit: "100"
    nginx.ingress.kubernetes.io/rate-limit-window: "1m"
spec:
  ingressClassName: nginx
  rules:
    - host: api.example.com
      http:
        paths:
          - path: /users
            pathType: Prefix
            backend:
              service:
                name: user-service
                port:
                  number: 80
          - path: /orders
            pathType: Prefix
            backend:
              service:
                name: order-service
                port:
                  number: 80
```

## Summary

The API Gateway pattern provides a centralized entry point for microservices architectures, offering:

- **Centralized Security**: Authentication, authorization, and API key management
- **Traffic Management**: Rate limiting, load balancing, and request routing
- **Service Aggregation**: Combining multiple backend calls into unified responses
- **Observability**: Request logging, metrics, and distributed tracing
- **Protocol Translation**: Converting between different API protocols

Key implementation considerations:

1. Choose between cloud-managed solutions (AWS API Gateway, Azure API Management) or self-hosted solutions (Kong, NGINX)
2. Implement proper health checking and circuit breaking for resilience
3. Configure request/response transformation for API versioning
4. Manage configuration as code for reproducibility
5. Implement comprehensive logging and monitoring

The API Gateway is a crucial component for securing, managing, and observing microservices in production environments.