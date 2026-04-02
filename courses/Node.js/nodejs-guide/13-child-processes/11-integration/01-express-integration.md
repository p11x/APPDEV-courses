# Child Process Integration Patterns

## What You'll Learn

- Integrating child processes with Express.js
- Database migration runners
- File processing pipelines
- Build system integration
- Real-world integration patterns

## Express.js Integration

```js
// server.js — Express with child process offloading
import express from 'express';
import { fork } from 'node:child_process';

const app = express();
app.use(express.json());

// Background task worker
let worker;
function getWorker() {
    if (!worker || worker.killed) {
        worker = fork('./workers/background.js');
        worker.on('exit', () => { worker = null; });
    }
    return worker;
}

// Offload heavy tasks to child process
app.post('/api/export', async (req, res) => {
    const w = getWorker();

    const taskId = Date.now();
    w.send({ type: 'export', id: taskId, data: req.body });

    // Respond immediately, process in background
    res.json({ taskId, status: 'processing' });

    w.on('message', function handler(msg) {
        if (msg.id === taskId) {
            w.removeListener('message', handler);
            // Store result or notify via WebSocket
            console.log('Export complete:', msg);
        }
    });
});

// Run database migration
app.post('/api/admin/migrate', async (req, res) => {
    const { execFile } = await import('node:child_process');
    const { promisify } = await import('node:util');
    const execFileAsync = promisify(execFile);

    try {
        const { stdout } = await execFileAsync('node', ['scripts/migrate.js', 'up']);
        res.json({ success: true, output: stdout });
    } catch (err) {
        res.status(500).json({ success: false, error: err.stderr });
    }
});

app.listen(3000);
```

## File Processing Pipeline

```js
// file-pipeline.js — Process files with child processes
import { spawn } from 'node:child_process';
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

class FileProcessor {
    async compress(input, output) {
        const gzip = spawn('gzip', ['-c']);
        await pipeline(createReadStream(input), gzip.stdin, createWriteStream(output));
    }

    async decompress(input, output) {
        const gunzip = spawn('gunzip', ['-c']);
        await pipeline(createReadStream(input), gunzip.stdin, createWriteStream(output));
    }

    async convertImage(input, output, format = 'jpeg') {
        return new Promise((resolve, reject) => {
            const convert = spawn('convert', [input, `${format}:${output}`]);
            convert.on('close', code => code === 0 ? resolve() : reject(new Error(`convert exit ${code}`)));
        });
    }

    async extractText(pdfPath) {
        const { stdout } = await promisify(execFile)('pdftotext', [pdfPath, '-']);
        return stdout;
    }
}
```

## Common Mistakes

- Not offloading heavy tasks (blocks event loop)
- Not implementing proper error handling in integrations
- Not cleaning up child processes on shutdown
- Not testing integrations with real external tools

## Try It Yourself

### Exercise 1: Background Export
Implement a file export endpoint that processes in a child process.

### Exercise 2: Build Pipeline
Create a build pipeline that runs lint → test → build sequentially.

### Exercise 3: File Processing
Build a file processing queue that compresses uploaded files.

## Next Steps

Continue to [Future Trends](../12-future-trends/01-advanced-patterns.md).
