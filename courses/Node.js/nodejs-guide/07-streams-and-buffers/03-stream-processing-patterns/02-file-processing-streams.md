# Stream-Based File Processing

## What You'll Learn

- Large file streaming with memory optimization
- File format parsing with streams
- File conversion pipelines
- File monitoring with streams
- File upload/download streaming

## Large File Processing

```javascript
import { createReadStream, createWriteStream } from 'node:fs';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createInterface } from 'node:readline';

// Process 10GB+ file with constant memory usage
async function processLargeCSV(inputPath, outputPath) {
    const lineReader = createInterface({
        input: createReadStream(inputPath, { encoding: 'utf8' }),
        crlfDelay: Infinity,
    });

    const output = createWriteStream(outputPath);
    let lineNumber = 0;
    let headers = null;

    for await (const line of lineReader) {
        lineNumber++;

        if (lineNumber === 1) {
            headers = line.split(',');
            output.write(headers.join(',') + ',processed_at\n');
            continue;
        }

        const fields = line.split(',');
        const record = Object.fromEntries(headers.map((h, i) => [h, fields[i]]));

        // Process record
        const processed = {
            ...record,
            processed_at: new Date().toISOString(),
        };

        output.write(Object.values(processed).join(',') + '\n');
    }

    output.end();
    return { lines: lineNumber };
}
```

## File Format Conversion

```javascript
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { Transform } from 'node:stream';

// JSONL to CSV converter
const jsonlToCsv = new Transform({
    transform(chunk, encoding, callback) {
        const lines = chunk.toString().split('\n');
        for (const line of lines) {
            if (!line.trim()) continue;
            try {
                const record = JSON.parse(line);
                if (!this.headersWritten) {
                    this.push(Object.keys(record).join(',') + '\n');
                    this.headersWritten = true;
                }
                this.push(Object.values(record).map(v =>
                    typeof v === 'string' && v.includes(',') ? `"${v}"` : v
                ).join(',') + '\n');
            } catch {
                // Skip invalid lines
            }
        }
        callback();
    }
});

await pipeline(
    createReadStream('data.jsonl'),
    jsonlToCsv,
    createWriteStream('data.csv')
);
```

## File Upload Streaming

```javascript
import express from 'express';
import { createWriteStream } from 'node:fs';
import { pipeline } from 'node:stream/promises';
import { createHash } from 'node:crypto';

const app = express();

app.post('/upload', async (req, res) => {
    const hash = createHash('sha256');
    const filename = `uploads/${Date.now()}-${req.headers['x-filename']}`;
    const writeStream = createWriteStream(filename);

    try {
        // Pipe request body to file while computing hash
        const { Transform } = await import('node:stream');
        
        const hasher = new Transform({
            transform(chunk, encoding, callback) {
                hash.update(chunk);
                callback(null, chunk);
            }
        });

        await pipeline(req, hasher, writeStream);

        const fileHash = hash.digest('hex');
        res.json({ success: true, hash: fileHash, path: filename });
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});
```

## File Download Streaming

```javascript
app.get('/download/:filename', (req, res) => {
    const filePath = `uploads/${req.params.filename}`;
    const { statSync, createReadStream } = require('node:fs');

    try {
        const stats = statSync(filePath);
        res.setHeader('Content-Length', stats.size);
        res.setHeader('Content-Type', 'application/octet-stream');
        res.setHeader('Content-Disposition', `attachment; filename="${req.params.filename}"`);

        const stream = createReadStream(filePath, { highWaterMark: 64 * 1024 });
        stream.pipe(res);

        stream.on('error', (err) => {
            console.error('Download stream error:', err);
            if (!res.headersSent) {
                res.status(500).json({ error: 'Download failed' });
            }
        });
    } catch (err) {
        res.status(404).json({ error: 'File not found' });
    }
});
```

## Best Practices Checklist

- [ ] Use streams for files larger than available memory
- [ ] Set appropriate `highWaterMark` for file I/O
- [ ] Handle file not found and permission errors
- [ ] Compute hashes during streaming (not after)
- [ ] Use `pipeline()` for automatic cleanup
- [ ] Monitor upload/download progress

## Cross-References

- See [File System Integration](../04-stream-filesystem-integration/01-file-read-write.md) for fs streams
- See [Network Operations](../05-stream-network-operations/01-http-streaming.md) for HTTP streaming
- See [Compression](./03-compression-encryption.md) for compressed files

## Next Steps

Continue to [Compression and Encryption Streams](./03-compression-encryption.md).
