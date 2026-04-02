# CPU Profiling

## What You'll Learn

- What CPU profiling is and when you need it
- How to use Node.js built-in `--prof` flag
- How to analyze the generated log file
- How to use Chrome DevTools for CPU profiling
- How to use clinic.js for flame graphs

## What Is CPU Profiling?

CPU profiling records which functions are running and how long each takes. It helps you find **hot paths** — functions that consume the most CPU time.

## Built-in Profiler

```bash
# Run with profiler enabled
node --prof server.js

# This creates a file like isolate-0xnnnnnnnnnnnn-v8.log
```

### Process the Log

```bash
# Print a human-readable summary
node --prof-process isolate-*.log
```

Output:

```
 [JavaScript]:
   ticks  total  nonlib   name
     45   30.0%   32.1%  fibonacci
     20   13.3%   14.3%  LazyCompile:processOrder server.js:12
     10    6.7%    7.1%  Builtin:ArrayReduce

 [Summary]:
   ticks  total  nonlib   name
    150  100.0%          TOTAL
```

## Chrome DevTools CPU Profile

```js
// profile-server.js — Server for CPU profiling

import { createServer } from 'node:http';

// Intentionally expensive function
function fibonacci(n) {
  if (n <= 1) return n;
  return fibonacci(n - 1) + fibonacci(n - 2);
}

function processRequest() {
  // Some work that takes CPU time
  const result = fibonacci(35);
  return result;
}

const server = createServer((req, res) => {
  if (req.url === '/heavy') {
    const result = processRequest();
    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({ result }));
    return;
  }

  res.writeHead(200);
  res.end('OK');
});

server.listen(3000, () => {
  console.log('Server on http://localhost:3000');
  console.log('1. Open chrome://inspect');
  console.log('2. Click "Open dedicated DevTools for Node"');
  console.log('3. Go to the Performance tab');
  console.log('4. Click Record, send requests, click Stop');
});
```

### Recording a CPU Profile in DevTools

1. Start with `node --inspect profile-server.js`
2. Open `chrome://inspect` → DevTools
3. Go to the **Performance** tab
4. Click **Record** (circle button)
5. Send requests: `curl http://localhost:3000/heavy`
6. Click **Stop**
7. Analyze the flame chart — the widest bars are the hottest functions

## Clinic.js

```bash
npm install -g clinic
```

### CPU Flame Graph

```bash
# Generate a flame graph
clinic flame -- node profile-server.js

# Send some requests in another terminal
# Press Ctrl+C to stop

# Opens an interactive flame graph in the browser
```

### Doctor (Automatic Analysis)

```bash
# clinic doctor analyzes your app and suggests optimizations
clinic doctor -- node profile-server.js
```

## How It Works

### Reading a Flame Graph

```
Bottom: Entry point (main, event loop)
  ↑
Middle: Functions called by main
  ↑
Top: Leaf functions (the actual work)

Width: Time spent (wider = more CPU time)
```

The widest bar at the top is your bottleneck — optimize that function first.

### V8 Log Format

The `--prof` flag creates a V8 log file. `--prof-process` parses it and summarizes:
- **ticks**: number of samples where this function was executing
- **total**: percentage of all ticks
- **nonlib**: percentage excluding Node.js internals

## Common Mistakes

### Mistake 1: Profiling in Development Without Load

```bash
# WRONG — profiling with no traffic shows nothing useful
node --prof server.js
# Just sits there

# CORRECT — generate load while profiling
node --prof server.js &
# In another terminal:
for i in $(seq 1 100); do curl http://localhost:3000/heavy; done
```

### Mistake 2: Not Isolating the Profiled Operation

```bash
# WRONG — profiling the entire app includes startup, which skews results
node --prof server.js

# CORRECT — profile only the operation you care about
# Use DevTools Performance panel to record only during the operation
```

### Mistake 3: Ignoring Node.js Internals

```
# WRONG — only looking at JavaScript ticks
   ticks  name
     10   fibonacci

# CORRECT — also check GC and C++ ticks
   ticks  name
     50   TOTAL
     10   fibonacci
      5   GarbageCollector  ← 10% time in GC — memory allocation issue!
```

## Try It Yourself

### Exercise 1: Find the Hot Path

Write a script that calls `fibonacci(40)` and `sort(1M items)`. Profile it and identify which function consumes more CPU.

### Exercise 2: Flame Graph

Generate a flame graph with clinic. Find the widest bar and identify the function.

### Exercise 3: Before/After

Profile a slow function. Optimize it (e.g., memoize Fibonacci). Profile again and compare the tick counts.

## Next Steps

You can profile CPU usage. For finding memory leaks, continue to [Memory Leaks](./02-memory-leaks.md).
