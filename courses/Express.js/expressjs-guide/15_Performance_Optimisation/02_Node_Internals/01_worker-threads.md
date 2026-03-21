# Worker Threads

## 📌 What You'll Learn

- Offloading CPU-bound tasks
- Shared memory
- Message passing

## 💻 Code Example

```js
import { Worker } from 'worker_threads';

function runWorker(data) {
  return new Promise((resolve, reject) => {
    const worker = new Worker('./worker.js', { workerData: data });
    worker.on('message', resolve);
    worker.on('error', reject);
  });
}

app.get('/process', async (req, res) => {
  const result = await runWorker({ input: 'data' });
  res.json({ result });
});
```
