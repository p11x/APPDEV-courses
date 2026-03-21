# req.ip and req.hostname

## 📌 What You'll Learn
- How to access the client's IP address with req.ip
- How to access the hostname with req.hostname
- Understanding the difference between req.hostname and req.host
- How proxy settings affect these values

## 🧠 Concept Explained (Plain English)

When a client makes a request to your server, you might want to know who is making the request. The client's IP address can be useful for logging, security, or geolocation. The hostname can tell you what domain the client used to reach your server (which is useful if you're serving multiple domains on the same server).

In Express, `req.ip` provides the remote IP address of the client making the request. However, if your application is behind a proxy (like a load balancer or reverse proxy), `req.ip` might show the proxy's IP address instead of the client's real IP. Express has a feature called "trust proxy" that, when enabled, looks at headers like `X-Forwarded-For` to determine the real client IP.

Similarly, `req.hostname` gives you the hostname from the request's Host header (without the port), while `req.host` includes the port if it's not the standard port (80 for HTTP, 443 for HTTPS).

Think of it like receiving a package. The return address on the package might show the distribution center's address (the proxy) rather than the sender's actual address. To get the sender's real address, you might need to look at a label inside the package (the X-Forwarded-For header).

## 💻 Code Example

```javascript
// ES Module - Accessing Client IP and Hostname

import express from 'express';

const app = express();

// ========================================
// IMPORTANT: Trust Proxy Settings (if behind a proxy)
// ========================================
// If your Express app is behind a proxy (like Nginx, AWS ELB, Heroku, etc.),
// you need to set the trust proxy setting to get the real client IP.
// This tells Express to look at headers like X-Forwarded-For.
// 
// Uncomment the line below if you are behind a proxy:
// app.set('trust proxy', true);

// We'll use express.json() for parsing JSON bodies in other examples, but for this one we focus on IP and hostname
// app.use(express.json());

// Route to view IP and hostname information
app.get('/whoami', (req, res) => {
    // req.ip - the remote IP address of the request
    // If behind a proxy and trust proxy is not set, this might be the proxy's IP
    const ip = req.ip;
    
    // req.ips - when trust proxy is true, this contains an array of IPs from X-Forwarded-For
    // The first element is usually the client IP, followed by proxies
    const ips = req.ips;
    
    // req.hostname - the hostname from the Host header (without port)
    const hostname = req.hostname;
    
    // req.host - the hostname including port if non-standard
    const host = req.host;
    
    // The original Host header value
    const hostHeader = req.headers.host;
    
    res.json({
        ip,
        ips,
        hostname,
        host,
        hostHeader,
        notes: {
            ip: 'Remote address of the connection (might be proxy IP if behind proxy)',
            ips: 'Array of IPs from X-Forwarded-For (when trust proxy is enabled)',
            hostname: 'Hostname from Host header (without port)',
            host: 'Hostname including port if non-standard',
            hostHeader: 'Raw value of the Host header'
        }
    });
});

// Route to demonstrate the difference with and without trust proxy
app.get('/proxy-info', (req, res) => {
    const trustProxyEnabled = app.get('trust proxy');
    
    res.json({
        trustProxyEnabled,
        ip: req.ip,
        ips: req.ips,
        hostname: req.hostname,
        host: req.host,
        explanation: trustProxyEnabled 
            ? 'Trust proxy is enabled - req.ip should be the client IP (from X-Forwarded-For)' 
            : 'Trust proxy is disabled - req.ip is the remote address (might be proxy)'
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import the Express framework |
| 4 | `const app = express();` | Create an Express application instance |
| 8 | `// app.set('trust proxy', true);` | Commented out instruction for setting trust proxy |
| 14 | `app.get('/whoami', (req, res) => {` | Defines a route to view IP and hostname info |
| 17 | `const ip = req.ip;` | Gets the remote IP address |
| 20 | `const ips = req.ips;` | Gets array of IPs (when trust proxy is enabled) |
| 23 | `const hostname = req.hostname;` | Gets hostname from Host header (without port) |
| 26 | `const host = req.host;` | Gets hostname including port if non-standard |
| 29 | `const hostHeader = req.headers.host;` | Gets raw Host header value |
| 32-44 | `res.json({ ... });` | Sends the information in the response |
| 48 | `app.get('/proxy-info', (req, res) => {` | Defines another route to show proxy info |
| 50 | `const trustProxyEnabled = app.get('trust proxy');` | Checks if trust proxy is enabled |
| 52-60 | `res.json({ ... });` | Sends proxy-related information |
| 63 | `app.listen(PORT, ...)` | Start the server |

## Understanding Proxy Headers

When behind a proxy, the original client IP might be sent in headers like:
- `X-Forwarded-For`: A comma-separated list of IPs (client, proxy1, proxy2, ...)
- `X-Real-IP`: Sometimes used to send the original client IP
- `Forwarded`: A standardized version of X-Forwarded-For

When you enable trust proxy with `app.set('trust proxy', true)`, Express:
1. Looks at the `X-Forwarded-For` header
2. Uses the first IP in the list as the client IP (req.ip)
3. Populates `req.ips` with the array of IPs
4. May also look at other headers depending on configuration

## Common Scenarios

### Direct Connection (No Proxy)
```
Client IP: 203.0.113.1
req.ip: "203.0.113.1"
req.ips: [] (empty array)
req.hostname: "example.com"
req.host: "example.com:3000" (if non-standard port)
```

### Behind a Proxy (with trust proxy enabled)
```
Client IP: 203.0.113.1
Proxy IP: 198.51.100.1
X-Forwarded-For: "203.0.113.1, 198.51.100.1"
req.ip: "203.0.113.1" (first IP from X-Forwarded-For)
req.ips: ["203.0.113.1", "198.51.100.1"]
req.hostname: "example.com"
req.host: "example.com:3000"
```

### Behind a Proxy (with trust proxy disabled)
```
Client IP: 203.0.113.1
Proxy IP: 198.51.100.1
X-Forwarded-For: "203.0.113.1, 198.51.100.1"
req.ip: "198.51.100.1" (the proxy's IP - the remote address)
req.ips: [] (empty array because trust proxy is disabled)
req.hostname: "example.com"
req.host: "example.com:3000"
```

## ⚠️ Common Mistakes

**1. Forgetting to set trust proxy when behind a proxy**
If your app is behind a proxy (like Nginx, AWS ELB, Heroku, etc.) and you don't set `app.set('trust proxy', true)`, `req.ip` will show the proxy's IP instead of the client's real IP.

**2. Trusting proxy headers inappropriately**
Only set trust proxy to true if you are actually behind a proxy that you trust. Setting it to true when directly exposed to the internet can allow clients to spoof their IP address.

**3. Confusing req.ip with req.ips**
- `req.ip` is a single string (the client IP, or proxy IP if not trusting proxy)
- `req.ips` is an array (only populated when trust proxy is enabled)

**4. Not understanding the difference between hostname and host**
- `req.hostname`: hostname from Host header (no port)
- `req.host`: hostname including port if it's not the default port (80 for HTTP, 443 for HTTPS)

**5. Assuming req.ip is always IPv4**
Clients can connect via IPv6, so `req.ip` might be an IPv6 address (like "::1" or "2001:db8::1").

## ✅ Quick Recap

- `req.ip` gives the remote IP address (client IP if trust proxy enabled, otherwise the immediate connection's IP)
- `req.ips` gives an array of IPs from X-Forwarded-For (when trust proxy enabled)
- `req.hostname` gives the hostname from the Host header (without port)
- `req.host` gives the hostname including port if non-standard
- Use `app.set('trust proxy', true)` when behind a trusted proxy to get the real client IP

## 🔗 What's Next

Let's learn about the request method and path with `req.method` and `req.path`.
