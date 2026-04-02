# Nginx Gateway

## What You'll Learn

- How to use Nginx as an API gateway
- How to configure routing and load balancing
- How to add SSL termination
- How to implement rate limiting

## Configuration

```nginx
# /etc/nginx/conf.d/api-gateway.conf

upstream user_service {
    least_conn;
    server user-service-1:3000;
    server user-service-2:3000;
    server user-service-3:3000;
}

upstream order_service {
    least_conn;
    server order-service-1:3000;
    server order-service-2:3000;
}

server {
    listen 80;
    server_name api.example.com;
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl http2;
    server_name api.example.com;

    ssl_certificate /etc/ssl/certs/api.pem;
    ssl_certificate_key /etc/ssl/private/api-key.pem;

    # Rate limiting
    limit_req_zone $binary_remote_addr zone=api:10m rate=100r/s;

    # Route to user service
    location /api/users {
        limit_req zone=api burst=20 nodelay;

        proxy_pass http://user_service;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        # Timeouts
        proxy_connect_timeout 5s;
        proxy_read_timeout 30s;
    }

    # Route to order service
    location /api/orders {
        limit_req zone=api burst=20 nodelay;

        proxy_pass http://order_service;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }

    # Health check endpoint
    location /healthz {
        access_log off;
        return 200 "OK";
    }
}
```

## Next Steps

For Kong, continue to [Kong Gateway](./02-kong-gateway.md).
