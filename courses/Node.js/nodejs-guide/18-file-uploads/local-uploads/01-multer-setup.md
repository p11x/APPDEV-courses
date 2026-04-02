# Multer Setup

## What You'll Learn

- What multipart form data is and why file uploads need it
- How to configure multer for disk and memory storage
- How to handle single and multiple file uploads
- How to set file size and count limits
- How to access uploaded file metadata in your route handler

## What Is Multipart Upload?

Regular HTTP requests send data as JSON or URL-encoded strings. Files (images, PDFs, videos) are binary data — they cannot be encoded as text. **Multipart form data** splits the request body into parts, each with its own headers and content.

Multer is a middleware that parses multipart requests and saves files to disk or memory.

## Project Setup

```bash
mkdir upload-demo && cd upload-demo
npm init -y
npm install express multer
mkdir uploads
```

## Basic Single File Upload

```js
// single-upload.js — Upload a single file

import express from 'express';
import multer from 'multer';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { mkdirSync } from 'node:fs';

const __dirname = dirname(fileURLToPath(import.meta.url));

const app = express();

// Configure multer storage
const storage = multer.diskStorage({
  // destination: where to save files
  destination(req, file, cb) {
    const dir = resolve(__dirname, 'uploads');
    mkdirSync(dir, { recursive: true });  // Ensure the directory exists
    cb(null, dir);  // null = no error, dir = save path
  },

  // filename: what to name the saved file
  filename(req, file, cb) {
    // Use timestamp + original name to prevent collisions
    // Date.now() ensures uniqueness even if two users upload "photo.jpg"
    const uniqueName = `${Date.now()}-${file.originalname}`;
    cb(null, uniqueName);
  },
});

// Create the multer instance with configuration
const upload = multer({
  storage,
  limits: {
    fileSize: 5 * 1024 * 1024,  // Max 5MB per file
    files: 1,                     // Max 1 file per request
  },
});

// POST /upload — single file field named "file"
app.post('/upload', upload.single('file'), (req, res) => {
  // req.file contains the uploaded file metadata
  // multer populated this when it saved the file
  if (!req.file) {
    return res.status(400).json({ error: 'No file uploaded' });
  }

  res.json({
    message: 'File uploaded successfully',
    file: {
      originalName: req.file.originalname,
      savedAs: req.file.filename,
      size: req.file.size,           // Size in bytes
      mimeType: req.file.mimetype,   // e.g., "image/png"
      path: req.file.path,           // Full path on disk
    },
  });
});

// Serve a simple upload form
app.get('/', (req, res) => {
  res.send(`
    <h1>File Upload</h1>
    <form action="/upload" method="POST" enctype="multipart/form-data">
      <input type="file" name="file">
      <button type="submit">Upload</button>
    </form>
  `);
});

// Handle multer errors
app.use((err, req, res, next) => {
  if (err instanceof multer.MulterError) {
    if (err.code === 'LIMIT_FILE_SIZE') {
      return res.status(413).json({ error: 'File too large (max 5MB)' });
    }
    if (err.code === 'LIMIT_FILE_COUNT') {
      return res.status(400).json({ error: 'Too many files' });
    }
    return res.status(400).json({ error: err.message });
  }
  next(err);  // Pass non-multer errors to the default error handler
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Multiple Files Upload

```js
// multiple-upload.js — Upload multiple files

import express from 'express';
import multer from 'multer';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { mkdirSync } from 'node:fs';

const __dirname = dirname(fileURLToPath(import.meta.url));

const app = express();

const storage = multer.diskStorage({
  destination(req, file, cb) {
    const dir = resolve(__dirname, 'uploads');
    mkdirSync(dir, { recursive: true });
    cb(null, dir);
  },
  filename(req, file, cb) {
    cb(null, `${Date.now()}-${file.originalname}`);
  },
});

const upload = multer({
  storage,
  limits: { fileSize: 10 * 1024 * 1024, files: 5 },
});

// upload.array('photos', 5) — accept up to 5 files in the "photos" field
app.post('/upload-multiple', upload.array('photos', 5), (req, res) => {
  if (!req.files?.length) {
    return res.status(400).json({ error: 'No files uploaded' });
  }

  res.json({
    message: `${req.files.length} file(s) uploaded`,
    files: req.files.map((f) => ({
      originalName: f.originalname,
      savedAs: f.filename,
      size: f.size,
      mimeType: f.mimetype,
    })),
  });
});

// upload.fields() — accept different fields with different limits
app.post('/upload-profile', upload.fields([
  { name: 'avatar', maxCount: 1 },     // One avatar image
  { name: 'photos', maxCount: 5 },     // Up to 5 photos
]), (req, res) => {
  // req.files is now an object: { avatar: [...], photos: [...] }
  res.json({
    avatar: req.files.avatar?.map((f) => f.filename) || [],
    photos: req.files.photos?.map((f) => f.filename) || [],
  });
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Memory Storage

```js
// memory-upload.js — Store file in memory (Buffer) instead of disk

import express from 'express';
import multer from 'multer';

const app = express();

// memoryStorage keeps the file in req.file.buffer as a Buffer
// Use this when you need to process the file (resize, hash, upload to S3)
// without saving to disk first
const upload = multer({
  storage: multer.memoryStorage(),
  limits: { fileSize: 5 * 1024 * 1024 },
});

app.post('/upload', upload.single('file'), (req, res) => {
  if (!req.file) {
    return res.status(400).json({ error: 'No file uploaded' });
  }

  // req.file.buffer contains the entire file as a Buffer
  const buffer = req.file.buffer;

  res.json({
    originalName: req.file.originalname,
    size: buffer.length,
    // You can now process the buffer: hash it, resize it, upload to S3, etc.
    md5: createHash('md5').update(buffer).digest('hex'),
  });
});

import { createHash } from 'node:crypto';

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## How It Works

### The Multer Middleware

```
POST /upload (multipart/form-data)
    │
    ▼
multer middleware parses the request body
    │
    ├── Text fields → req.body
    │
    └── File fields → req.file (single) or req.files (multiple)
        │
        └── Saved to disk (diskStorage) or memory (memoryStorage)
```

### Storage Options

| Storage | Where Files Go | Use Case |
|---------|---------------|----------|
| `diskStorage` | Filesystem | Large files, direct serving |
| `memoryStorage` | RAM (Buffer) | Processing before upload to cloud, hashing |

### Upload Methods

| Method | Usage | Field Access |
|--------|-------|-------------|
| `upload.single('file')` | One file | `req.file` |
| `upload.array('files', 5)` | Up to 5 files, same field | `req.files` |
| `upload.fields([...])` | Multiple fields | `req.files.avatar`, `req.files.photos` |

## Common Mistakes

### Mistake 1: Not Setting `enctype`

```html
<!-- WRONG — form sends data as URL-encoded, file is not sent -->
<form action="/upload" method="POST">
  <input type="file" name="file">
</form>

<!-- CORRECT — enctype="multipart/form-data" is required -->
<form action="/upload" method="POST" enctype="multipart/form-data">
  <input type="file" name="file">
</form>
```

### Mistake 2: No File Size Limit

```js
// WRONG — a user can upload a 10GB file, crashing the server
const upload = multer({ storage });

// CORRECT — always set limits
const upload = multer({
  storage,
  limits: { fileSize: 5 * 1024 * 1024 },  // 5MB
});
```

### Mistake 3: Not Handling Multer Errors

```js
// WRONG — multer errors crash the app with a 500
app.post('/upload', upload.single('file'), (req, res) => {
  res.json({ file: req.file });
});
// If file is too large, the client gets a raw 500 error

// CORRECT — add error handling middleware
app.use((err, req, res, next) => {
  if (err instanceof multer.MulterError) {
    return res.status(400).json({ error: err.message });
  }
  next(err);
});
```

## Try It Yourself

### Exercise 1: Image Gallery

Create a form that accepts up to 10 images. Save them with unique names. Return a list of uploaded files with their sizes.

### Exercise 2: File Info Endpoint

After uploading, create a `GET /files/:filename` endpoint that returns the file's metadata (name, size, MIME type) and serves the file.

### Exercise 3: CSV Upload

Create a route that accepts a CSV file, reads it from the buffer, parses the rows, and returns the data as JSON.

## Next Steps

You can upload files. For validating file types and sizes before saving, continue to [Validation](./02-validation.md).
