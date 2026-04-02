# Koa.js and NestJS Stream Processing

## What You'll Learn

- Koa response streaming with `ctx.body = readableStream`
- Koa request body streaming via `ctx.req` as a readable stream
- Koa stream middleware pattern for file uploads
- NestJS stream-based controller responses
- NestJS file upload with stream processing
- NestJS stream interceptor pattern
- NestJS Server-Sent Events (SSE) implementation
- Real-world: NestJS streaming file upload with virus scanning

## Koa Response Streaming

Koa natively supports stream responses. Assign a readable stream to `ctx.body` and Koa handles piping, backpressure, and `Transfer-Encoding: chunked` automatically.

```javascript
import Koa from 'koa';
import Router from '@koa/router';
import { createReadStream } from 'node:fs';
import { createGzip } from 'node:zlib';
import { Readable } from 'node:stream';
import { stat } from 'node:fs/promises';

const app = new Koa();
const router = new Router();

// Stream a file as the response body
router.get('/files/:name', async (ctx) => {
    const filePath = `./uploads/${ctx.params.name}`;
    const stats = await stat(filePath);

    ctx.set('Content-Length', stats.size.toString());
    ctx.set('Content-Type', 'application/octet-stream');
    ctx.body = createReadStream(filePath);
});

// Stream a large JSON array without buffering everything in memory
router.get('/api/users/stream', async (ctx) => {
    ctx.set('Content-Type', 'application/json');

    const readable = new Readable({ read() {} });
    ctx.body = readable;

    readable.push('[');
    for (let i = 0; i < 1_000_000; i++) {
        readable.push((i > 0 ? ',' : '') + JSON.stringify({ id: i, name: `User ${i}` }));
        if (i % 10000 === 0) await new Promise((r) => setTimeout(r, 0));
    }
    readable.push(']');
    readable.push(null);
});

// Gzip-compressed file streaming
router.get('/files/:name/compressed', async (ctx) => {
    ctx.set('Content-Encoding', 'gzip');
    ctx.body = createReadStream(`./uploads/${ctx.params.name}`).pipe(createGzip());
});

app.use(router.routes());
app.listen(3000);
```

## Koa Request Body Streaming

Access `ctx.req` (the raw Node.js `IncomingMessage`) directly to stream the request body without Koa's body parser buffering it.

```javascript
import Koa from 'koa';
import { createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';
import { createHash } from 'node:crypto';
import { Transform } from 'node:stream';

const app = new Koa();

// Stream request body to disk — no body-parser needed
app.use(async (ctx) => {
    if (ctx.method !== 'POST' || ctx.path !== '/upload') {
        ctx.status = 404;
        return;
    }

    const dest = createWriteStream(`./uploads/${ctx.get('X-Filename') || 'upload.bin'}`);
    const hasher = createHash('sha256');

    const hashPass = new Transform({
        transform(chunk, enc, cb) { hasher.update(chunk); cb(null, chunk); }
    });

    await pipeline(ctx.req, hashPass, dest);
    ctx.body = { message: 'Upload complete', sha256: hasher.digest('hex') };
});

app.listen(3000);
```

## Koa Stream Middleware for File Uploads

A reusable middleware that enforces size limits and exposes the upload stream to downstream handlers.

```javascript
import { Transform } from 'node:stream';
import { createWriteStream } from 'node:fs';
import { mkdir } from 'node:fs/promises';
import { pipeline } from 'node:stream/promises';
import { join } from 'node:path';

function streamUpload({ dest = './uploads', maxSize = 100 * 1024 * 1024 } = {}) {
    return async (ctx, next) => {
        if (ctx.method !== 'POST' || !ctx.path.startsWith('/upload')) return next();
        await mkdir(dest, { recursive: true });

        const filename = ctx.get('X-Filename') || `file-${Date.now()}`;
        let bytesReceived = 0;

        const sizeGuard = new Transform({
            transform(chunk, enc, cb) {
                bytesReceived += chunk.length;
                if (bytesReceived > maxSize) cb(new Error(`Exceeds ${maxSize} bytes`));
                else cb(null, chunk);
            }
        });

        await pipeline(ctx.req, sizeGuard, createWriteStream(join(dest, filename)));
        ctx.state.upload = { filename, path: join(dest, filename), size: bytesReceived };
        await next();
    };
}

// app.use(streamUpload({ maxSize: 50 * 1024 * 1024 }));
```

## NestJS Stream-Based Controller Responses

NestJS controllers can return a `StreamableFile` or pipe directly to the response.

```typescript
// stream.controller.ts
import { Controller, Get, Param, Res, Header, StreamableFile } from '@nestjs/common';
import type { Response } from 'express';
import { createReadStream } from 'node:fs';
import { stat } from 'node:fs/promises';
import { join } from 'node:path';
import { Readable } from 'node:stream';

@Controller('files')
export class StreamController {

    @Get(':name')
    @Header('Content-Type', 'application/octet-stream')
    async getFile(@Param('name') name: string) {
        const path = join(process.cwd(), 'uploads', name);
        return new StreamableFile(createReadStream(path));
    }

    // Manual streaming — full control over headers and piping
    @Get(':name/raw')
    async getFileRaw(@Param('name') name: string, @Res() res: Response) {
        const path = join(process.cwd(), 'uploads', name);
        const stats = await stat(path);
        res.set({ 'Content-Length': stats.size.toString(), 'Content-Type': 'application/octet-stream' });
        createReadStream(path).pipe(res);
    }

    // Stream a generated CSV report
    @Get('report/csv')
    @Header('Content-Type', 'text/csv')
    async csvReport() {
        async function* generate() {
            yield 'id,name,email\n';
            for (let i = 0; i < 100_000; i++) yield `${i},User${i},user${i}@example.com\n`;
        }
        return new StreamableFile(Readable.from(generate()));
    }
}
```

## NestJS File Upload with Stream Processing

A custom interceptor that streams uploads directly to disk with hash computation.

```typescript
// upload.controller.ts
import { Controller, Post, Body, UseInterceptors } from '@nestjs/common';
import { CallHandler, ExecutionContext, Injectable, NestInterceptor } from '@nestjs/common';
import { createWriteStream } from 'node:fs';
import { mkdir } from 'node:fs/promises';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';
import { createHash } from 'node:crypto';
import { join } from 'node:path';

@Injectable()
class StreamUploadInterceptor implements NestInterceptor {
    constructor(private dest = './uploads') {}

    async intercept(ctx: ExecutionContext, next: CallHandler) {
        const req = ctx.switchToHttp().getRequest();
        await mkdir(this.dest, { recursive: true });

        const filename = req.headers['x-filename'] || `upload-${Date.now()}`;
        const hash = createHash('sha256');

        const hashPass = new Transform({
            transform(chunk, enc, cb) { hash.update(chunk); cb(null, chunk); }
        });

        await pipeline(req, hashPass, createWriteStream(join(this.dest, filename)));

        req.body = { ...req.body, uploadResult: { filename, sha256: hash.digest('hex') } };
        return next.handle();
    }
}

@Controller('upload')
export class UploadController {
    @Post('stream')
    @UseInterceptors(new StreamUploadInterceptor())
    async upload(@Body() body: any) {
        return body.uploadResult;
    }
}
```

## NestJS Stream Interceptor Pattern

An interceptor that wraps the response in gzip when the client supports it.

```typescript
import { CallHandler, ExecutionContext, Injectable, NestInterceptor } from '@nestjs/common';
import { Observable } from 'rxjs';
import { createGzip } from 'node:zlib';
import type { Response } from 'express';

@Injectable()
export class GzipStreamInterceptor implements NestInterceptor {
    intercept(context: ExecutionContext, next: CallHandler): Observable<any> {
        const res: Response = context.switchToHttp().getResponse();
        const accept = context.switchToHttp().getRequest().headers['accept-encoding'] || '';

        if (!accept.includes('gzip')) return next.handle();

        res.setHeader('Content-Encoding', 'gzip');
        const gzip = createGzip();
        gzip.pipe(res as any);

        res.write = ((chunk, enc, cb) => gzip.write(chunk, enc, cb)) as any;
        res.end = ((chunk, enc, cb) => { if (chunk) gzip.write(chunk, enc); gzip.end(cb); return true; }) as any;

        return next.handle();
    }
}
```

## NestJS Server-Sent Events (SSE)

NestJS supports SSE via the `@Sse()` decorator with `Observable`-based event streams.

```typescript
// sse.controller.ts
import { Controller, Sse, MessageEvent } from '@nestjs/common';
import { Observable, interval, map } from 'rxjs';
import { EventEmitter } from 'node:events';

@Controller('events')
export class SseController {

    @Sse('heartbeat')
    heartbeat(): Observable<MessageEvent> {
        return interval(1000).pipe(
            map((i) => ({ data: { tick: i, ts: Date.now() }, type: 'heartbeat' } as MessageEvent)),
        );
    }

    @Sse('db-changes')
    dbChanges(): Observable<MessageEvent> {
        const emitter = new EventEmitter();
        setInterval(() => emitter.emit('change', { table: 'users', action: 'UPDATE', id: Math.random() }), 2000);

        return new Observable<MessageEvent>((sub) => {
            const handler = (data: any) => sub.next({ data, type: 'db-change' } as MessageEvent);
            emitter.on('change', handler);
            return () => emitter.off('change', handler);
        });
    }
}
```

## Real-World: NestJS Streaming File Upload with Virus Scanning

A production-grade upload that streams to disk while hashing and scanning for virus signatures in a single pass.

```typescript
// scan-upload.controller.ts
import { Controller, Post, Req, Res, HttpException, HttpStatus } from '@nestjs/common';
import type { Request, Response } from 'express';
import { createWriteStream } from 'node:fs';
import { mkdir, unlink, stat } from 'node:fs/promises';
import { join, extname } from 'node:path';
import { pipeline } from 'node:stream/promises';
import { Transform } from 'node:stream';
import { createHash, randomUUID } from 'node:crypto';

const MAX_SIZE = 200 * 1024 * 1024;
const ALLOWED = new Set(['.pdf', '.docx', '.xlsx', '.png', '.jpg', '.csv', '.txt']);

@Controller('api/upload')
export class ScanUploadController {
    @Post()
    async upload(@Req() req: Request, @Res() res: Response) {
        const filename = req.headers['x-filename'] as string;
        if (!filename) throw new HttpException('X-Filename required', HttpStatus.BAD_REQUEST);

        const ext = extname(filename).toLowerCase();
        if (!ALLOWED.has(ext)) throw new HttpException(`Extension ${ext} not allowed`, HttpStatus.BAD_REQUEST);

        await mkdir('./uploads', { recursive: true });
        const safeName = `${randomUUID()}${ext}`;
        let bytes = 0;

        const sizeGuard = new Transform({
            transform(chunk, enc, cb) {
                bytes += chunk.length;
                if (bytes > MAX_SIZE) cb(new Error('MAX_SIZE_EXCEEDED'));
                else cb(null, chunk);
            }
        });

        const hasher = createHash('sha256');
        const hashPass = new Transform({
            transform(chunk, enc, cb) { hasher.update(chunk); cb(null, chunk); }
        });

        const scanner = new Transform({
            transform(chunk, enc, cb) {
                const sigs = ['X5O!P%@AP[4\\PZX54(P^)7CC)7'];
                if (sigs.some((s) => chunk.includes(Buffer.from(s)))) cb(new Error('VIRUS_DETECTED'));
                else cb(null, chunk);
            }
        });

        try {
            await pipeline(req, sizeGuard, hashPass, scanner, createWriteStream(join('./uploads', safeName)));
        } catch (err: any) {
            await unlink(join('./uploads', safeName)).catch(() => {});
            if (err.message === 'MAX_SIZE_EXCEEDED') throw new HttpException('File too large', HttpStatus.PAYLOAD_TOO_LARGE);
            if (err.message === 'VIRUS_DETECTED') throw new HttpException('Virus detected', HttpStatus.UNPROCESSABLE_ENTITY);
            throw new HttpException('Upload failed', HttpStatus.INTERNAL_SERVER_ERROR);
        }

        const fileStats = await stat(join('./uploads', safeName));
        res.status(201).json({
            id: safeName.split('.')[0], filename: safeName,
            originalName: filename, size: fileStats.size,
            sha256: hasher.digest('hex'), status: 'clean',
        });
    }
}
```

## Best Practices Checklist

- [ ] Use `ctx.body = stream` in Koa instead of manually piping to `ctx.res`
- [ ] Access `ctx.req` directly for raw request body streaming
- [ ] Implement size-guard transforms to reject oversized uploads early
- [ ] Use `StreamableFile` in NestJS for simple file responses
- [ ] Use `@Res()` with manual piping when you need full header control
- [ ] Prefer `@Sse()` with `Observable` for NestJS SSE endpoints
- [ ] Always clean up partial files when stream pipelines fail
- [ ] Hash and scan uploads in the stream pipeline to avoid a second read
- [ ] Test stream error paths — a failing transform must abort the entire pipeline
- [ ] Set `Transfer-Encoding: chunked` for indefinite-length stream responses

## Cross-References

- See [Express & Fastify Integration](./01-express-fastify-integration.md) for equivalent patterns
- See [Pipeline](../01-streams-architecture/01-duplex-passthrough-pipeline.md) for `pipeline()` fundamentals
- See [Transform Streams](../03-transform-streams/01-custom-transforms.md) for custom transforms
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for stream error patterns

## Next Steps

Continue to [GraphQL, WebSocket, and SSE Streaming](./03-graphql-websocket-sse.md).
