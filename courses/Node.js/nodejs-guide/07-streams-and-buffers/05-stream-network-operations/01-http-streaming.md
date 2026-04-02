# HTTP and Network Streaming

## What You'll Learn

- HTTP request/response streaming
- HTTP proxy implementation
- WebSocket streaming
- TCP socket streaming
- Stream-based API patterns

## HTTP Response Streaming

```javascript
import http from 'node:http';
import { createReadStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';

const server = http.createServer(async (req, res) => {
    // Stream a large file as HTTP response
    if (req.url === '/download') {
        res.writeHead(200, {
            'Content-Type': 'application/octet-stream',
            'Content-Disposition': 'attachment; filename="data.bin"',
        });

        try {
            await pipeline(
                createReadStream('large-file.dat', { highWaterMark: 64 * 1024 }),
                res
            );
        } catch (err) {
            console.error('Download stream error:', err);
            if (!res.headersSent) res.writeHead(500);
            res.end();
        }
        return;
    }

    // Stream JSON response for large datasets
    if (req.url === '/api/users') {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.write('[');

        let first = true;
        // Simulate streaming from database cursor
        for (let i = 0; i < 100000; i++) {
            if (!first) res.write(',');
            res.write(JSON.stringify({ id: i, name: `User ${i}` }));
            first = false;

            // Respect backpressure
            if (!res.write('')) {
                await new Promise(r => res.once('drain', r));
            }
        }

        res.end(']');
        return;
    }

    res.writeHead(404);
    res.end('Not found');
});

server.listen(3000);
```

## HTTP Request Streaming (Client)

```javascript
import http from 'node:http';
import { pipeline } from 'node:stream/promises';
import { createReadStream } from 'node:fs';
import { Transform } from 'node:stream';

// Upload a file with streaming
async function uploadFile(filePath, targetUrl) {
    const url = new URL(targetUrl);

    const req = http.request({
        hostname: url.hostname,
        port: url.port,
        path: url.pathname,
        method: 'POST',
        headers: {
            'Content-Type': 'application/octet-stream',
        },
    });

    await pipeline(
        createReadStream(filePath),
        req
    );

    return new Promise((resolve, reject) => {
        req.on('response', (res) => {
            let body = '';
            res.on('data', (chunk) => body += chunk);
            res.on('end', () => resolve({ status: res.statusCode, body }));
        });
        req.on('error', reject);
    });
}

// Stream and process HTTP response
async function fetchAndProcess(url) {
    const req = http.get(url);

    return new Promise((resolve, reject) => {
        req.on('response', (res) => {
            const lineParser = new Transform({
                transform(chunk, encoding, callback) {
                    const lines = chunk.toString().split('\n');
                    for (const line of lines) {
                        if (line.trim()) {
                            this.push(JSON.parse(line));
                        }
                    }
                    callback();
                }
            });

            const results = [];
            res.pipe(lineParser);
            lineParser.on('data', (record) => results.push(record));
            lineParser.on('end', () => resolve(results));
            lineParser.on('error', reject);
        });
        req.on('error', reject);
    });
}
```

## HTTP Proxy with Streams

```javascript
import http from 'node:http';

const proxy = http.createServer((clientReq, clientRes) => {
    const targetUrl = new URL(clientReq.url, 'http://backend:8080');

    const proxyReq = http.request({
        hostname: targetUrl.hostname,
        port: targetUrl.port,
        path: targetUrl.pathname + targetUrl.search,
        method: clientReq.method,
        headers: clientReq.headers,
    }, (proxyRes) => {
        clientRes.writeHead(proxyRes.statusCode, proxyRes.headers);
        proxyRes.pipe(clientRes);
    });

    // Stream client request body to backend
    clientReq.pipe(proxyReq);

    proxyReq.on('error', (err) => {
        console.error('Proxy error:', err.message);
        clientRes.writeHead(502);
        clientRes.end('Bad Gateway');
    });
});

proxy.listen(8000);
```

## TCP Socket Streaming

```javascript
import { createServer, connect } from 'node:net';
import { Transform } from 'node:stream';

// TCP server with streaming
const server = createServer((socket) => {
    console.log('Client connected');

    // Parse lines from TCP stream
    const lineParser = new Transform({
        transform(chunk, encoding, callback) {
            const lines = (this._buffer || '').concat(chunk.toString()).split('\n');
            this._buffer = lines.pop();
            for (const line of lines) {
                if (line.trim()) this.push(line);
            }
            callback();
        },
        flush(callback) {
            if (this._buffer) this.push(this._buffer);
            callback();
        }
    });

    socket.pipe(lineParser);

    lineParser.on('data', (line) => {
        console.log('Received:', line);
        socket.write(`Echo: ${line}\n`);
    });

    socket.on('end', () => console.log('Client disconnected'));
});

server.listen(9000);

// TCP client
const client = connect(9000, 'localhost', () => {
    client.write('Hello server\n');
    client.write('Second message\n');
    client.end('Goodbye\n');
});

client.on('data', (data) => {
    console.log('Server says:', data.toString());
});
```

## WebSocket Streaming

```javascript
import { WebSocketServer } from 'ws';
import { createReadStream } from 'node:fs';

const wss = new WebSocketServer({ port: 8080 });

wss.on('connection', (ws) => {
    console.log('WebSocket connected');

    // Stream file over WebSocket
    ws.on('message', (message) => {
        const command = JSON.parse(message.toString());

        if (command.action === 'stream-file') {
            const stream = createReadStream(command.path, {
                highWaterMark: 16 * 1024,
            });

            stream.on('data', (chunk) => {
                ws.send(chunk, { binary: true });
            });

            stream.on('end', () => {
                ws.send(JSON.stringify({ type: 'complete' }));
            });

            stream.on('error', (err) => {
                ws.send(JSON.stringify({ type: 'error', message: err.message }));
            });
        }
    });
});
```

## Best Practices Checklist

- [ ] Use `pipeline()` for HTTP streams with error handling
- [ ] Set appropriate `highWaterMark` for network streams
- [ ] Handle connection errors and timeouts
- [ ] Use backpressure to prevent memory exhaustion
- [ ] Implement reconnection logic for persistent connections
- [ ] Monitor stream throughput for debugging

## Cross-References

- See [Pipeline](../01-streams-architecture/01-duplex-passthrough-pipeline.md) for pipeline patterns
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for error patterns
- See [Performance](../08-stream-performance-optimization/01-profiling-memory.md) for optimization

## Next Steps

Continue to [Concurrency and Parallelism](../06-stream-concurrency-parallelism/01-parallel-processing.md).
