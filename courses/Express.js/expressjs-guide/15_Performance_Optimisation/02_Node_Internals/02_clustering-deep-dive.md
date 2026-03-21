# Clustering Deep Dive

## 📌 What You'll Learn

- Node.js cluster module
- PM2 cluster mode
- Sticky sessions
- IPC

## 💻 Code Example

```js
import cluster from 'cluster';
import os from 'os';

if (cluster.isPrimary) {
  const numCPUs = os.cpus().length;
  
  for (let i = 0; i < numCPUs; i++) {
    cluster.fork();
  }
  
  cluster.on('exit', (worker) => {
    console.log(`Worker ${worker.id} died`);
    cluster.fork();
  });
} else {
  import express from 'express';
  const app = express();
  app.get('/', (req, res) => res.send(`Worker ${process.pid}`));
  app.listen(3000);
}
```
