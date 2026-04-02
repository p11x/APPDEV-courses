# Upload Validation

## What You'll Learn

- How to validate file MIME types (whitelist approach)
- How to verify actual file content matches the claimed MIME type
- How to validate image dimensions with sharp
- How to reject dangerous file types
- How to combine multiple validation steps in middleware

## Why Validate?

File uploads are a major security vector. Attackers can upload:
- **Executable files** disguised as images (`.php`, `.exe`)
- **Oversized files** that fill disk or memory
- **Malformed files** that crash your image processor
- **Files with wrong extensions** (`virus.jpg` that is actually a script)

Always validate the **actual file content**, not just the filename or MIME type.

## Project Setup

```bash
npm install express multer sharp file-type
```

## MIME Type Whitelist

```js
// validate-mime.js — Whitelist allowed file types

import express from 'express';
import multer from 'multer';
import { fileTypeFromBuffer } from 'file-type';  // Detects actual file type from content
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { mkdirSync } from 'node:fs';

const __dirname = dirname(fileURLToPath(import.meta.url));
const app = express();

// Allowed MIME types — whitelist approach (only these are accepted)
const ALLOWED_TYPES = new Set([
  'image/jpeg',
  'image/png',
  'image/gif',
  'image/webp',
  'application/pdf',
]);

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
  limits: { fileSize: 10 * 1024 * 1024 },
  // Multer's built-in file filter — runs BEFORE saving the file
  fileFilter(req, file, cb) {
    // Check the MIME type reported by the browser
    if (!ALLOWED_TYPES.has(file.mimetype)) {
      // Reject the file — pass false and an error message
      return cb(new Error(`File type ${file.mimetype} not allowed`), false);
    }
    cb(null, true);  // Accept the file
  },
});

// Additional validation AFTER multer saves the file
async function validateFileContent(req, res, next) {
  if (!req.file) return next();

  // Read the first few bytes to detect the actual file type
  // file-type reads the file's magic bytes (signature), not the extension
  const { readFileSync } = await import('node:fs');
  const buffer = readFileSync(req.file.path);

  const detected = await fileTypeFromBuffer(buffer);

  if (!detected) {
    // Could not detect type — probably a plain text file or empty
    return res.status(400).json({
      error: 'Could not determine file type — possibly not a valid binary file',
    });
  }

  // Verify the detected type matches the allowed types
  if (!ALLOWED_TYPES.has(detected.mime)) {
    // Delete the uploaded file — it is not allowed
    const { unlinkSync } = await import('node:fs');
    unlinkSync(req.file.path);

    return res.status(400).json({
      error: `Detected file type ${detected.mime} is not allowed`,
      claimedType: req.file.mimetype,
      actualType: detected.mime,
    });
  }

  // Verify browser-reported type matches detected type
  // This catches users renaming .exe to .jpg
  if (detected.mime !== req.file.mimetype) {
    console.warn(
      `MIME mismatch: browser said ${req.file.mimetype}, actual is ${detected.mime}`
    );
    // You can choose to reject or accept (if the detected type is allowed)
  }

  next();
}

// POST /upload — upload with full validation
app.post('/upload', upload.single('file'), validateFileContent, (req, res) => {
  res.json({
    message: 'File uploaded and validated',
    file: req.file.originalname,
    size: req.file.size,
  });
});

// Error handler
app.use((err, req, res, next) => {
  if (err.message?.includes('not allowed')) {
    return res.status(400).json({ error: err.message });
  }
  next(err);
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## Image Validation with Sharp

```js
// validate-image.js — Validate image files with sharp

import express from 'express';
import multer from 'multer';
import sharp from 'sharp';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { mkdirSync, unlinkSync } from 'node:fs';

const __dirname = dirname(fileURLToPath(import.meta.url));
const app = express();

const upload = multer({
  storage: multer.memoryStorage(),  // Use memory to validate before saving
  limits: { fileSize: 10 * 1024 * 1024 },
  fileFilter(req, file, cb) {
    if (!file.mimetype.startsWith('image/')) {
      return cb(new Error('Only image files are allowed'), false);
    }
    cb(null, true);
  },
});

async function validateImage(req, res, next) {
  if (!req.file) return next();

  try {
    // sharp reads the image metadata — this validates the file is a real image
    // If the file is corrupted or not an image, sharp throws an error
    const metadata = await sharp(req.file.buffer).metadata();

    // Enforce maximum dimensions
    const MAX_WIDTH = 4096;
    const MAX_HEIGHT = 4096;

    if (metadata.width > MAX_WIDTH || metadata.height > MAX_HEIGHT) {
      return res.status(400).json({
        error: `Image too large: ${metadata.width}x${metadata.height}. Max: ${MAX_WIDTH}x${MAX_HEIGHT}`,
      });
    }

    // Enforce minimum dimensions
    if (metadata.width < 50 || metadata.height < 50) {
      return res.status(400).json({
        error: 'Image too small: minimum 50x50 pixels',
      });
    }

    // Attach metadata to the request for downstream handlers
    req.imageMetadata = metadata;
    next();
  } catch (err) {
    // sharp failed to read the image — it is not a valid image
    return res.status(400).json({
      error: 'Invalid or corrupted image file',
      details: err.message,
    });
  }
}

app.post('/upload-image', upload.single('image'), validateImage, async (req, res) => {
  const { format, width, height, size } = req.imageMetadata;

  // Save the validated image
  const filename = `${Date.now()}.${format}`;
  const dest = resolve(__dirname, 'uploads', filename);
  mkdirSync(resolve(__dirname, 'uploads'), { recursive: true });

  // sharp can resize, convert format, and optimize
  await sharp(req.file.buffer)
    .resize(800, 800, { fit: 'inside', withoutEnlargement: true })  // Max 800px, keep aspect ratio
    .toFile(dest);

  res.json({
    message: 'Image uploaded and validated',
    original: { format, width, height, size },
    savedAs: filename,
  });
});

app.use((err, req, res, next) => {
  if (err instanceof multer.MulterError) {
    return res.status(400).json({ error: err.message });
  }
  if (err.message?.includes('Only image')) {
    return res.status(400).json({ error: err.message });
  }
  next(err);
});

app.listen(3000, () => {
  console.log('Server on http://localhost:3000');
});
```

## How It Works

### Two-Layer Validation

```
Browser reports: "image/jpeg"
    │
    ▼
Layer 1: multer fileFilter (before saving)
    │  Check: is "image/jpeg" in the whitelist?
    │
    ▼
Layer 2: content validation (after saving)
    │  Check: do the file's magic bytes actually match JPEG?
    │  Check: can sharp parse it as an image?
    │  Check: are dimensions within limits?
    │
    ▼
File accepted ✅
```

### Magic Bytes

Every file format has a **signature** in its first few bytes:
- JPEG: `FF D8 FF`
- PNG: `89 50 4E 47`
- PDF: `25 50 44 46`

`file-type` reads these bytes to determine the actual type, regardless of the file extension.

## Common Mistakes

### Mistake 1: Trusting the Browser MIME Type

```js
// WRONG — browser-supplied MIME type can be spoofed
if (file.mimetype === 'image/jpeg') {
  // Accept — but the file could be anything
}

// CORRECT — check actual file content
const detected = await fileTypeFromBuffer(buffer);
if (detected?.mime === 'image/jpeg') {
  // Accept — verified from magic bytes
}
```

### Mistake 2: Blacklist Instead of Whitelist

```js
// WRONG — blacklist is easy to bypass
const BLOCKED = ['.exe', '.php', '.bat'];
if (BLOCKED.includes(ext)) reject();
// Attacker uploads .phtml, .php5, .shtml — not blocked!

// CORRECT — whitelist only allows known-safe types
const ALLOWED = new Set(['image/jpeg', 'image/png', 'application/pdf']);
if (!ALLOWED.has(detected.mime)) reject();
```

### Mistake 3: Not Cleaning Up Rejected Files

```js
// WRONG — multer saves the file, then validation rejects it
// The rejected file stays on disk forever
if (!isValid) {
  return res.status(400).json({ error: 'Invalid file' });
  // File is still on disk!
}

// CORRECT — delete rejected files
if (!isValid) {
  unlinkSync(req.file.path);  // Remove from disk
  return res.status(400).json({ error: 'Invalid file' });
}
```

## Try It Yourself

### Exercise 1: PDF Page Count

Upload a PDF file. Use a library like `pdf-parse` to count its pages. Reject PDFs with more than 100 pages.

### Exercise 2: Virus Scan Simulation

Create a validation middleware that scans file content for a "virus signature" string. If found, reject the file and delete it.

### Exercise 3: Aspect Ratio Validation

Upload an image. Validate that it has a 16:9 aspect ratio (±5% tolerance). Reject images that do not match.

## Next Steps

You can validate uploads. For streaming uploads without saving to disk first, continue to [Streaming Upload](./03-streaming-upload.md).
