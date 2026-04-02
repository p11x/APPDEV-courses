# Streaming Uploads with Busboy

## What You'll Learn

- How to handle file uploads as streams without temporary files
- How to use busboy directly (the library multer is built on)
- How to pipe uploaded files directly to disk or cloud storage
- How to handle backpressure during large uploads
- How streaming uploads differ from multer's approach

## Why Stream Uploads?

Multer saves the entire file to disk or memory before your handler runs. For large files (hundreds of MB), this means:
- The file must be fully received before processing starts
- Memory storage fills up for large files
- Disk storage creates a temp file you then move

**Streaming** processes the upload chunk-by-chunk as data arrives. No temp files, constant memory usage, and you can start processing before the upload finishes.

## How Busboy Works

Busboy is the underlying parser that multer uses. It emits events as form fields and files are received:

```
HTTP multipart stream
    │
    ▼
busboy parses headers
    │
    ├── 'file' event → file name, MIME, readable stream
    │
    └── 'field' event → text field name, value
```

## Project Setup

```bash
npm install express busboy
```

## Streaming to Disk

```js
// stream-disk.js — Stream file uploads directly to disk

import { createServer } from 'node:http';
import { createWriteStream } from 'node:fs';
import { mkdir } from 'node:fs/promises';
import { resolve, dirname, extname } from 'node:path';
import { fileURLToPath } from 'node:url';
import Busboy from 'busboy';

const __dirname = dirname(fileURLToPath(import.meta.url));
const UPLOAD_DIR = resolve(__dirname, 'uploads');

await mkdir(UPLOAD_DIR, { recursive: true });

const server = createServer((req, res) => {
  if (req.method === 'POST' && req.url === '/upload') {
    // Create busboy instance — it reads the Content-Type header to parse multipart
    const busboy = Busboy({ headers: req.headers });

    const uploadedFiles = [];
    const fields = {};

    // 'file' event fires for each file in the multipart request
    busboy.on('file', (fieldname, fileStream, info) => {
      // info contains { filename, encoding, mimeType }
      const { filename, mimeType } = info;

      // Generate a unique filename
      const safeName = `${Date.now()}-${filename}`;
      const dest = resolve(UPLOAD_DIR, safeName);

      // Create a write stream — pipe the upload directly to disk
      // This is the key difference from multer: data flows through immediately
      const writeStream = createWriteStream(dest);

      // Track file size for validation
      let fileSize = 0;
      const MAX_SIZE = 10 * 1024 * 1024;  // 10MB

      fileStream.on('data', (chunk) => {
        fileSize += chunk.length;

        // Enforce size limit during streaming — kill the stream if too large
        if (fileSize > MAX_SIZE) {
          fileStream.destroy();  // Stop reading from the client
          writeStream.destroy(); // Stop writing to disk

          // Delete the partially written file
          import('node:fs').then(({ unlinkSync }) => {
            try { unlinkSync(dest); } catch {}
          });

          // Send error response
          res.writeHead(413, { 'Content-Type': 'application/json' });
          res.end(JSON.stringify({ error: 'File too large (max 10MB)' }));
        }
      });

      // Pipe the file stream to the write stream
      // Backpressure: if disk is slow, fileStream pauses automatically
      fileStream.pipe(writeStream);

      fileStream.on('end', () => {
        uploadedFiles.push({
          fieldname,
          filename: safeName,
          originalName: filename,
          mimeType,
          size: fileSize,
        });
      });

      fileStream.on('error', (err) => {
        console.error('File stream error:', err.message);
      });
    });

    // 'field' event fires for text fields (not files)
    busboy.on('field', (fieldname, value) => {
      fields[fieldname] = value;
    });

    // 'finish' fires when all files and fields are received
    busboy.on('finish', () => {
      // Only respond if we haven't already sent an error
      if (!res.writableEnded) {
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({
          message: 'Upload complete',
          fields,
          files: uploadedFiles,
        }));
      }
    });

    // 'error' fires on parse errors
    busboy.on('error', (err) => {
      console.error('Busboy error:', err.message);
      if (!res.writableEnded) {
        res.writeHead(400, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({ error: 'Upload failed' }));
      }
    });

    // Pipe the request stream into busboy
    // busboy reads from req, parses multipart, and emits events
    req.pipe(busboy);

    return;
  }

  // Serve a test form
  res.writeHead(200, { 'Content-Type': 'text/html' });
  res.end(`
    <h1>Streaming Upload</h1>
    <form action="/upload" method="POST" enctype="multipart/form-data">
      <input type="text" name="description" placeholder="Description">
      <input type="file" name="file">
      <button type="submit">Upload</button>
    </form>
  `);
});

server.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Streaming to Memory (for Processing)

```js
// stream-process.js — Stream file into a buffer for immediate processing

import { createServer } from 'node:http';
import { createHash } from 'node:crypto';
import Busboy from 'busboy';

const server = createServer((req, res) => {
  if (req.method !== 'POST' || req.url !== '/hash') {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end(`
      <h1>File Hash Calculator</h1>
      <form action="/hash" method="POST" enctype="multipart/form-data">
        <input type="file" name="file">
        <button type="submit">Calculate Hash</button>
      </form>
    `);
    return;
  }

  const busboy = Busboy({ headers: req.headers });

  busboy.on('file', (fieldname, fileStream, info) => {
    const { filename } = info;

    // Create a hash stream — data flows through the hash as it arrives
    // No need to buffer the entire file
    const hash = createHash('sha256');

    let size = 0;
    fileStream.on('data', (chunk) => {
      size += chunk.length;
      hash.update(chunk);  // Feed each chunk into the hash
    });

    fileStream.on('end', () => {
      const digest = hash.digest('hex');

      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({
        filename,
        size,
        sha256: digest,
      }));
    });

    // Consume the stream (busboy requires this even if we handle data ourselves)
    fileStream.resume();
  });

  req.pipe(busboy);
});

server.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## How It Works

### Stream Flow

```
Client uploads 100MB file
    │
    ▼
busboy receives chunks (each ~64KB)
    │
    ├── chunk 1 → writeStream.write(chunk) → disk
    ├── chunk 2 → writeStream.write(chunk) → disk
    ├── ...
    └── chunk N → writeStream.write(chunk) → disk
         │
         ▼
    'finish' event → respond to client
```

Memory usage stays constant (~64KB buffer) regardless of file size.

### Backpressure

When the write stream (disk) is slower than the read stream (network), Node.js pauses the read stream automatically. This prevents memory buildup.

```
Network (fast) → [buffer] → Disk (slow)
                      │
                      └── Buffer fills → Network pauses → Buffer drains → Network resumes
```

## Common Mistakes

### Mistake 1: Not Consuming the File Stream

```js
// WRONG — busboy requires all streams to be consumed
busboy.on('file', (name, stream) => {
  // Ignoring the stream — busboy will stall
});

// CORRECT — consume or pipe the stream
busboy.on('file', (name, stream) => {
  stream.resume();  // Discard the data
});
```

### Mistake 2: No Backpressure Handling

```js
// WRONG — collecting chunks in memory without limits
let data = Buffer.alloc(0);
stream.on('data', (chunk) => {
  data = Buffer.concat([data, chunk]);  // Grows unbounded
});

// CORRECT — pipe to a write stream (handles backpressure automatically)
stream.pipe(createWriteStream(dest));
```

### Mistake 3: Not Handling Client Disconnect

```js
// WRONG — if the client cancels the upload, streams hang
req.pipe(busboy);
// No cleanup if req is aborted

// CORRECT — handle the 'close' event
req.on('close', () => {
  if (!res.writableEnded) {
    // Client disconnected — clean up partially uploaded file
    try { unlinkSync(dest); } catch {}
  }
});
```

## Try It Yourself

### Exercise 1: Progress Tracking

Track upload progress by counting bytes received. Log the percentage every 1MB. (Hint: use the `data` event on the file stream.)

### Exercise 2: CSV Streaming

Upload a large CSV file. Parse each row as it arrives (do not buffer the whole file). Count the total rows and return the count.

### Exercise 3: Direct to Cloud

Modify the streaming example to pipe the file stream directly to an S3 upload (see [S3 Upload](../cloud-uploads/01-s3-upload.md)) instead of saving to disk.

## Next Steps

You can stream uploads locally. For uploading to cloud storage, continue to [S3 Upload](../cloud-uploads/01-s3-upload.md).
