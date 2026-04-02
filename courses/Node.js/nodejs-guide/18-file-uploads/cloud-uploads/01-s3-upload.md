# S3 Upload

## What You'll Learn

- How to upload files to AWS S3 with the AWS SDK v3
- How to stream files directly to S3 without saving locally first
- How to set ACL (access control) and metadata on uploads
- How to handle upload errors and retries
- How to configure S3 bucket policies for public or private access

## What Is S3?

Amazon S3 (Simple Storage Service) is cloud object storage. You upload files (objects) into **buckets** and access them via URLs. S3 is durable (99.999999999%), scalable, and pay-per-use.

## Project Setup

```bash
npm install @aws-sdk/client-s3 multer
```

Set environment variables:

```bash
# .env
AWS_REGION=us-east-1
AWS_ACCESS_KEY_ID=your-access-key
AWS_SECRET_ACCESS_KEY=your-secret-key
S3_BUCKET_NAME=my-upload-bucket
```

## Upload to S3

```js
// s3-upload.js — Upload files to AWS S3

import express from 'express';
import multer from 'multer';
import { S3Client, PutObjectCommand, GetObjectCommand } from '@aws-sdk/client-s3';
import { getSignedUrl } from '@aws-sdk/s3-request-presigner';
import { createReadStream } from 'node:fs';
import { readFile } from 'node:fs/promises';
import { extname } from 'node:path';

const app = express();

// Create S3 client — credentials are loaded from environment variables
const s3 = new S3Client({
  region: process.env.AWS_REGION || 'us-east-1',
  // Credentials are automatically loaded from:
  // 1. Environment variables (AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY)
  // 2. AWS credentials file (~/.aws/credentials)
  // 3. IAM role (when running on EC2/ECS/Lambda)
});

const BUCKET = process.env.S3_BUCKET_NAME || 'my-upload-bucket';

// Use memory storage — we will upload directly to S3
const upload = multer({
  storage: multer.memoryStorage(),
  limits: { fileSize: 50 * 1024 * 1024 },  // 50MB limit
});

// POST /upload — upload a file to S3
app.post('/upload', upload.single('file'), async (req, res) => {
  if (!req.file) {
    return res.status(400).json({ error: 'No file uploaded' });
  }

  // Generate a unique S3 key (path within the bucket)
  const key = `uploads/${Date.now()}-${req.file.originalname}`;

  try {
    // PutObjectCommand uploads the file to S3
    const command = new PutObjectCommand({
      Bucket: BUCKET,
      Key: key,
      Body: req.file.buffer,              // The file content as a Buffer
      ContentType: req.file.mimetype,     // Set the content type for when it is served
      ContentLength: req.file.size,       // Tell S3 the size for integrity checking

      // ACL: 'public-read' makes the file publicly accessible
      // Remove this for private files (use signed URLs instead)
      // ACL: 'public-read',

      // Metadata — custom key-value pairs stored with the object
      Metadata: {
        'original-name': req.file.originalname,
        'uploaded-by': req.ip,
      },
    });

    await s3.send(command);

    // Build the public URL (if bucket allows public access)
    const url = `https://${BUCKET}.s3.${process.env.AWS_REGION || 'us-east-1'}.amazonaws.com/${key}`;

    res.status(201).json({
      message: 'File uploaded to S3',
      key,
      url,
      size: req.file.size,
    });
  } catch (err) {
    console.error('S3 upload error:', err.message);
    res.status(500).json({ error: 'Upload failed' });
  }
});

// GET /files/:key/url — get a temporary signed URL for a private file
app.get('/files/:key(*)/url', async (req, res) => {
  try {
    const command = new GetObjectCommand({
      Bucket: BUCKET,
      Key: req.params.key,
    });

    // Generate a signed URL that expires in 1 hour
    // This allows temporary access to private files without making them public
    const signedUrl = await getSignedUrl(s3, command, { expiresIn: 3600 });

    res.json({ url: signedUrl, expiresInSeconds: 3600 });
  } catch (err) {
    console.error('Signed URL error:', err.message);
    res.status(404).json({ error: 'File not found' });
  }
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Streaming Upload to S3

```js
// s3-stream.js — Stream a file directly to S3 without buffering

import { createServer } from 'node:http';
import { S3Client, PutObjectCommand } from '@aws-sdk/client-s3';
import { PassThrough } from 'node:stream';
import Busboy from 'busboy';

const s3 = new S3Client({ region: process.env.AWS_REGION || 'us-east-1' });
const BUCKET = process.env.S3_BUCKET_NAME || 'my-upload-bucket';

const server = createServer((req, res) => {
  if (req.method !== 'POST' || req.url !== '/stream-upload') {
    res.writeHead(200, { 'Content-Type': 'text/html' });
    res.end('<h1>Stream Upload</h1><form action="/stream-upload" method="POST" enctype="multipart/form-data"><input type="file" name="file"><button>Upload</button></form>');
    return;
  }

  const busboy = Busboy({ headers: req.headers });

  busboy.on('file', (fieldname, fileStream, info) => {
    const { filename, mimeType } = info;
    const key = `uploads/${Date.now()}-${filename}`;

    // PassThrough is a stream that passes data through — acts as a readable stream
    // that S3 can consume while data is still arriving from the client
    const passThrough = new PassThrough();

    // Pipe the file stream into the PassThrough
    fileStream.pipe(passThrough);

    // Upload the PassThrough stream to S3
    // S3 can consume streams — it does not need the full file in memory
    const command = new PutObjectCommand({
      Bucket: BUCKET,
      Key: key,
      Body: passThrough,          // Stream, not Buffer
      ContentType: mimeType,
    });

    s3.send(command)
      .then(() => {
        if (!res.writableEnded) {
          res.writeHead(201, { 'Content-Type': 'application/json' });
          res.end(JSON.stringify({ message: 'Streamed to S3', key }));
        }
      })
      .catch((err) => {
        console.error('S3 stream error:', err.message);
        if (!res.writableEnded) {
          res.writeHead(500, { 'Content-Type': 'application/json' });
          res.end(JSON.stringify({ error: 'Upload failed' }));
        }
      });
  });

  req.pipe(busboy);
});

server.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## How It Works

### S3 Upload Flow

```
Client → Node.js Server → S3
```

1. Client sends the file to your server
2. Your server receives it (buffered or streamed)
3. Your server uploads to S3 using the AWS SDK
4. S3 returns a success response
5. Your server returns the file URL to the client

### Buffer vs Stream

| Approach | Memory | Speed | Use Case |
|----------|--------|-------|----------|
| Buffer (`req.file.buffer`) | Full file in RAM | Faster | Files < 50MB |
| Stream (`PassThrough`) | Constant (~64KB) | Slower (but safer) | Large files, limited RAM |

### Signed URLs

Private S3 files cannot be accessed directly. **Signed URLs** grant temporary access:

```js
const signedUrl = await getSignedUrl(s3, new GetObjectCommand({
  Bucket: BUCKET, Key: 'private/file.pdf',
}), { expiresIn: 3600 });
// URL works for 1 hour, then expires
```

## Common Mistakes

### Mistake 1: Hardcoding Credentials

```js
// WRONG — credentials in source code
const s3 = new S3Client({
  credentials: { accessKeyId: 'AKIA...', secretAccessKey: 'secret...' },
});

// CORRECT — use environment variables or IAM roles
const s3 = new S3Client({ region: 'us-east-1' });
// Credentials are loaded automatically from env vars or IAM
```

### Mistake 2: No Content-Type

```js
// WRONG — S3 defaults to "application/octet-stream"
// Browsers download the file instead of displaying it
await s3.send(new PutObjectCommand({ Bucket, Key, Body }));

// CORRECT — set ContentType
await s3.send(new PutObjectCommand({
  Bucket, Key, Body,
  ContentType: req.file.mimetype,
}));
```

### Mistake 3: Not Handling S3 Errors

```js
// WRONG — S3 errors crash the server
await s3.send(command);  // Throws if bucket does not exist, access denied, etc.

// CORRECT — catch and handle S3 errors
try {
  await s3.send(command);
} catch (err) {
  if (err.name === 'NoSuchBucket') {
    return res.status(500).json({ error: 'Storage bucket not configured' });
  }
  throw err;
}
```

## Try It Yourself

### Exercise 1: Image Resize on Upload

Upload an image, resize it with sharp to three sizes (thumbnail, medium, large), and upload all three to S3.

### Exercise 2: Signed Upload URL

Create a `GET /upload-url` endpoint that returns a signed PUT URL. The client can upload directly to S3 without going through your server.

### Exercise 3: List Bucket Contents

Create a `GET /files` endpoint that lists all uploaded files in the bucket using `ListObjectsCommand`.

## Next Steps

You can upload to S3. For letting clients upload directly to S3 without your server, continue to [Presigned URLs](./02-presigned-urls.md).
