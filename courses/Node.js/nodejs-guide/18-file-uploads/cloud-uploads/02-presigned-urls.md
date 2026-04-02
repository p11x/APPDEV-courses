# Presigned URLs

## What You'll Learn

- What presigned URLs are and how they work
- How to generate presigned PUT URLs for direct browser uploads
- How to generate presigned GET URLs for private file downloads
- How to set expiration times on presigned URLs
- How direct-to-S3 uploads reduce server load

## The Problem with Server-Side Uploads

When a user uploads a 1GB video, the data flows:

```
Browser → Your Server → S3
```

Your server must receive and forward all 1GB. This:
- Consumes your server's bandwidth
- Ties up a connection for minutes
- Costs money (you pay for server bandwidth)

## The Presigned URL Solution

With **presigned URLs**, the browser uploads directly to S3:

```
Your Server: Generate presigned URL → Return to client
Browser: Upload directly to S3 using the URL (bypasses your server)
```

Your server only generates a short URL — the actual data transfer goes browser → S3.

## Generating Presigned URLs

```js
// presigned-server.js — API that generates presigned URLs

import express from 'express';
import { S3Client, PutObjectCommand, GetObjectCommand } from '@aws-sdk/client-s3';
import { getSignedUrl } from '@aws-sdk/s3-request-presigner';
import { randomUUID } from 'node:crypto';
import { extname } from 'node:path';

const app = express();
app.use(express.json());

const s3 = new S3Client({ region: process.env.AWS_REGION || 'us-east-1' });
const BUCKET = process.env.S3_BUCKET_NAME || 'my-upload-bucket';

// GET /presign-upload — generate a presigned PUT URL
app.get('/presign-upload', async (req, res) => {
  const { filename, contentType } = req.query;

  if (!filename || !contentType) {
    return res.status(400).json({ error: 'filename and contentType required' });
  }

  // Validate content type — do not allow arbitrary types
  const allowed = ['image/jpeg', 'image/png', 'image/webp', 'video/mp4', 'application/pdf'];
  if (!allowed.includes(contentType)) {
    return res.status(400).json({ error: `Content type ${contentType} not allowed` });
  }

  // Generate a unique key for the file
  const ext = extname(filename);
  const key = `uploads/${randomUUID()}${ext}`;

  // Create the PUT command — this defines what the upload will do
  const command = new PutObjectCommand({
    Bucket: BUCKET,
    Key: key,
    ContentType: contentType,  // Client must send this header when uploading

    // Optional: limit file size with a content-length-range condition
    // The URL will only work for files between 0 and 100MB
    // Conditions: [['content-length-range', 0, 100 * 1024 * 1024]],
  });

  // Generate a signed URL that expires in 15 minutes
  const url = await getSignedUrl(s3, command, { expiresIn: 900 });

  res.json({
    uploadUrl: url,        // Client PUTs the file here
    key,                   // Store this in your database
    expiresIn: 900,        // Seconds until the URL expires
    method: 'PUT',         // Client must use HTTP PUT
    headers: {
      'Content-Type': contentType,  // Client must send this exact header
    },
  });
});

// GET /presign-download — generate a presigned GET URL for private files
app.get('/presign-download', async (req, res) => {
  const { key } = req.query;

  if (!key) {
    return res.status(400).json({ error: 'key required' });
  }

  const command = new GetObjectCommand({
    Bucket: BUCKET,
    Key: key,
  });

  // URL works for 1 hour
  const url = await getSignedUrl(s3, command, { expiresIn: 3600 });

  res.json({
    downloadUrl: url,
    expiresIn: 3600,
  });
});

// POST /complete-upload — client calls this after uploading to S3
// Store the file reference in your database
app.post('/complete-upload', async (req, res) => {
  const { key, originalName, size, userId } = req.body;

  // In a real app, save to your database
  // db.insert('files', { key, originalName, size, userId, createdAt: new Date() });

  res.json({
    message: 'Upload recorded',
    file: { key, originalName, size },
    downloadUrl: `/presign-download?key=${encodeURIComponent(key)}`,
  });
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Client: Direct Browser Upload

```html
<!-- client.html — Upload directly to S3 from the browser -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>Direct S3 Upload</title>
</head>
<body>
  <h1>Direct Upload to S3</h1>
  <input type="file" id="fileInput">
  <button id="uploadBtn">Upload</button>
  <div id="progress"></div>
  <div id="result"></div>

  <script>
    document.getElementById('uploadBtn').addEventListener('click', async () => {
      const file = document.getElementById('fileInput').files[0];
      if (!file) return alert('Select a file');

      const progress = document.getElementById('progress');
      const result = document.getElementById('result');

      progress.textContent = 'Getting upload URL...';

      // Step 1: Get a presigned URL from our server
      const presignRes = await fetch(
        `/presign-upload?filename=${encodeURIComponent(file.name)}&contentType=${encodeURIComponent(file.type)}`
      );
      const { uploadUrl, key, headers } = await presignRes.json();

      progress.textContent = 'Uploading to S3...';

      // Step 2: Upload directly to S3 using the presigned URL
      // No data passes through our server!
      const uploadRes = await fetch(uploadUrl, {
        method: 'PUT',
        headers: {
          'Content-Type': file.type,  // Must match the presigned URL's ContentType
        },
        body: file,  // The file content
      });

      if (uploadRes.ok) {
        progress.textContent = 'Upload complete!';

        // Step 3: Tell our server the upload is done
        await fetch('/complete-upload', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            key,
            originalName: file.name,
            size: file.size,
            userId: 'user-1',  // From your auth system
          }),
        });

        result.textContent = `Uploaded: ${key}`;
      } else {
        progress.textContent = 'Upload failed';
        result.textContent = await uploadRes.text();
      }
    });
  </script>
</body>
</html>
```

## How It Works

### The Presigned URL Flow

```
1. Client → GET /presign-upload?filename=photo.jpg&contentType=image/jpeg
2. Server → Generate unique key: uploads/abc123.jpg
3. Server → Create PutObjectCommand with that key
4. Server → getSignedUrl() → returns a URL with query-string signature
5. Server → Return { uploadUrl, key } to client
6. Client → PUT uploadUrl (file goes directly to S3)
7. Client → POST /complete-upload { key } (notify our server)
```

### What Makes It Secure

The presigned URL contains an **HMAC signature** calculated from:
- Your AWS credentials
- The bucket, key, and content type
- The expiration time

Anyone with the URL can upload to that specific key, but only until the URL expires. After expiration, S3 rejects the request.

## Common Mistakes

### Mistake 1: No Expiration

```js
// WRONG — URL never expires, anyone with it can upload forever
const url = await getSignedUrl(s3, command);

// CORRECT — always set a short expiration
const url = await getSignedUrl(s3, command, { expiresIn: 900 });  // 15 minutes
```

### Mistake 2: Not Validating Content-Type After Upload

```js
// WRONG — client could upload a .exe to a .jpg URL
// The presigned URL checks Content-Type header, but verify after upload

// CORRECT — validate the file after upload
app.post('/complete-upload', async (req, res) => {
  const { key } = req.body;
  // Use HeadObjectCommand to check the actual content type
  const head = await s3.send(new HeadObjectCommand({ Bucket, Key: key }));
  if (!head.ContentType.startsWith('image/')) {
    await s3.send(new DeleteObjectCommand({ Bucket, Key: key }));
    return res.status(400).json({ error: 'Not an image' });
  }
});
```

### Mistake 3: Exposing the Key Before Upload Completes

```js
// WRONG — returning the key before the upload finishes
// Client might call /complete-upload without actually uploading
res.json({ key, uploadUrl });

// CORRECT — only record the file in your database after confirming the upload
// Use S3 event notifications or verify with HeadObject before saving
```

## Try It Yourself

### Exercise 1: Progress Bar

Add an XMLHttpRequest-based upload with `onprogress` to show a real-time progress bar during the S3 upload.

### Exercise 2: Resumable Upload

Research S3 multipart upload. Implement a resumable upload that can continue after a network interruption.

### Exercise 3: Upload Policy

Add a content-length-range condition to the presigned URL so files must be between 1KB and 100MB. Test with files outside this range.

## Next Steps

You understand presigned URLs. For securing your API, continue to [Chapter 19: Security & Rate Limiting](../../19-security-rate-limiting/http-security/01-helmet.md).
