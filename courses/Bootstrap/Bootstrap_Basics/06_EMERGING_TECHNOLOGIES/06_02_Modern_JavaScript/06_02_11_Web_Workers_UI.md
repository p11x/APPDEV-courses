---
title: "Web Workers with Bootstrap UI"
slug: "web-workers-bootstrap-ui"
difficulty: 3
tags: ["bootstrap", "javascript", "web-workers", "performance", "async"]
prerequisites:
  - "06_02_10_Performance_Observer"
  - "06_02_01_Intersection_Observer"
related:
  - "06_02_12_Broadcast_Channel"
  - "06_02_09_Resize_Observer"
duration: "40 minutes"
---

# Web Workers with Bootstrap UI

## Overview

Web Workers offload heavy computation from the main thread, keeping Bootstrap UI responsive during data processing, sorting, filtering, and transformation tasks. Without workers, large dataset operations freeze the UI, causing janky animations and unresponsive interactions. Workers communicate with the main thread via `postMessage`, enabling progress reporting that drives Bootstrap progress bars, spinners, and status badges. This pattern is essential for data dashboards, spreadsheet-like interfaces, and real-time analytics.

## Basic Implementation

A Bootstrap progress bar driven by a Web Worker processing a large dataset.

```html
<div class="container mt-4">
  <div class="card">
    <div class="card-header d-flex justify-content-between align-items-center">
      <h5 class="mb-0">Data Processing</h5>
      <button class="btn btn-primary btn-sm" id="startProcess">
        <i class="bi bi-play-fill"></i> Start
      </button>
    </div>
    <div class="card-body">
      <div class="progress mb-3" style="height: 25px;">
        <div class="progress-bar progress-bar-striped progress-bar-animated"
             id="progressBar" style="width: 0%">0%</div>
      </div>
      <div id="statusText" class="text-muted">Ready</div>
      <div id="resultArea" class="mt-3 d-none">
        <div class="alert alert-success mb-0"></div>
      </div>
    </div>
  </div>
</div>

<script>
// worker.js content (inlined as Blob for simplicity)
const workerCode = `
  self.onmessage = function(e) {
    const { action, data } = e.data;
    if (action === 'process') {
      const total = data.length;
      const results = [];
      for (let i = 0; i < total; i++) {
        results.push({ ...data[i], processed: true, score: Math.random() * 100 });
        if (i % 1000 === 0) {
          self.postMessage({ type: 'progress', value: Math.round((i / total) * 100) });
        }
      }
      self.postMessage({ type: 'complete', results });
    }
  };
`;

const blob = new Blob([workerCode], { type: 'application/javascript' });
const worker = new Worker(URL.createObjectURL(blob));

worker.onmessage = (e) => {
  if (e.data.type === 'progress') {
    const bar = document.getElementById('progressBar');
    bar.style.width = `${e.data.value}%`;
    bar.textContent = `${e.data.value}%`;
    document.getElementById('statusText').textContent = `Processing... ${e.data.value}%`;
  }
  if (e.data.type === 'complete') {
    document.getElementById('progressBar').classList.remove('progress-bar-animated');
    document.getElementById('progressBar').classList.add('bg-success');
    document.getElementById('progressBar').textContent = 'Complete';
    document.getElementById('statusText').textContent = `Processed ${e.data.results.length} records`;
    document.getElementById('resultArea').classList.remove('d-none');
    document.getElementById('resultArea').querySelector('.alert').textContent =
      `Successfully processed ${e.data.results.length} items`;
  }
};

document.getElementById('startProcess').addEventListener('click', () => {
  const largeDataset = Array.from({ length: 50000 }, (_, i) => ({
    id: i, name: `Item ${i}`, value: Math.random() * 1000
  }));
  worker.postMessage({ action: 'process', data: largeDataset });
});
</script>
```

## Advanced Variations

### Parallel Worker Pool

Distribute work across multiple workers for faster processing with aggregate progress.

```html
<div class="card">
  <div class="card-header">
    <h5 class="mb-0">Parallel Processing</h5>
  </div>
  <div class="card-body">
    <div class="row mb-3">
      <div class="col-md-4">
        <label class="form-label">Workers</label>
        <select class="form-select" id="workerCount">
          <option value="2">2</option>
          <option value="4" selected>4</option>
          <option value="8">8</option>
        </select>
      </div>
      <div class="col-md-4">
        <label class="form-label">Records</label>
        <input type="number" class="form-control" id="recordCount" value="100000">
      </div>
      <div class="col-md-4 d-flex align-items-end">
        <button class="btn btn-primary w-100" id="runParallel">Process</button>
      </div>
    </div>
    <div id="workerProgress" class="row g-2"></div>
    <div id="aggregateResult" class="mt-3"></div>
  </div>
</div>

<script>
document.getElementById('runParallel').addEventListener('click', () => {
  const numWorkers = parseInt(document.getElementById('workerCount').value);
  const totalRecords = parseInt(document.getElementById('recordCount').value);
  const chunkSize = Math.ceil(totalRecords / numWorkers);

  const progressContainer = document.getElementById('workerProgress');
  progressContainer.innerHTML = Array.from({ length: numWorkers }, (_, i) => `
    <div class="col-md-6">
      <div class="border rounded p-2">
        <small class="text-muted">Worker ${i + 1}</small>
        <div class="progress mt-1" style="height: 8px;">
          <div class="progress-bar" id="worker-${i}" style="width: 0%"></div>
        </div>
      </div>
    </div>
  `).join('');

  let completed = 0;
  const startTime = performance.now();

  for (let i = 0; i < numWorkers; i++) {
    const worker = new Worker(URL.createObjectURL(new Blob([`
      self.onmessage = function(e) {
        const { start, end } = e.data;
        let count = 0;
        for (let j = start; j < end; j++) {
          count++;
          if (count % 5000 === 0) {
            self.postMessage({ type: 'progress', workerId: ${i}, pct: Math.round(((j - start) / (end - start)) * 100) });
          }
        }
        self.postMessage({ type: 'done', workerId: ${i}, count });
      };
    `], { type: 'application/javascript' })));

    worker.onmessage = (e) => {
      if (e.data.type === 'progress') {
        document.getElementById(`worker-${e.data.workerId}`).style.width = `${e.data.pct}%`;
      }
      if (e.data.type === 'done') {
        completed++;
        document.getElementById(`worker-${e.data.workerId}`).classList.add('bg-success');
        document.getElementById(`worker-${e.data.workerId}`).style.width = '100%';
        if (completed === numWorkers) {
          const elapsed = (performance.now() - startTime).toFixed(0);
          document.getElementById('aggregateResult').innerHTML =
            `<div class="alert alert-success">Processed ${totalRecords} records with ${numWorkers} workers in ${elapsed}ms</div>`;
        }
      }
    };

    worker.postMessage({ start: i * chunkSize, end: Math.min((i + 1) * chunkSize, totalRecords) });
  }
});
</script>
```

### Worker-Driven Table Filtering

Filter large datasets in a worker while keeping the table UI responsive.

```html
<div class="input-group mb-3">
  <span class="input-group-text"><i class="bi bi-search"></i></span>
  <input type="text" class="form-control" id="filterInput" placeholder="Filter 100k rows...">
  <span class="input-group-text" id="filterStatus">100,000 rows</span>
</div>
<div class="table-responsive" style="max-height: 400px; overflow-y: auto;">
  <table class="table table-sm table-hover">
    <thead class="table-light sticky-top">
      <tr><th>ID</th><th>Name</th><th>Category</th><th>Value</th></tr>
    </thead>
    <tbody id="filteredBody"></tbody>
  </table>
</div>

<script>
const filterWorker = new Worker(URL.createObjectURL(new Blob([`
  let dataset = [];
  self.onmessage = function(e) {
    if (e.data.action === 'setData') {
      dataset = e.data.data;
    }
    if (e.data.action === 'filter') {
      const query = e.data.query.toLowerCase();
      const filtered = dataset.filter(row =>
        row.name.toLowerCase().includes(query) ||
        row.category.toLowerCase().includes(query)
      ).slice(0, 200);
      self.postMessage({ type: 'filtered', results: filtered, total: filtered.length });
    }
  };
`], { type: 'application/javascript' })));

// Generate and load dataset
const dataset = Array.from({ length: 100000 }, (_, i) => ({
  id: i,
  name: `Product ${String.fromCharCode(65 + (i % 26))}${i}`,
  category: ['Electronics', 'Clothing', 'Food', 'Books', 'Sports'][i % 5],
  value: (Math.random() * 500).toFixed(2)
}));
filterWorker.postMessage({ action: 'setData', data: dataset });

let debounceTimer;
document.getElementById('filterInput').addEventListener('input', (e) => {
  clearTimeout(debounceTimer);
  document.getElementById('filterStatus').textContent = 'Filtering...';
  debounceTimer = setTimeout(() => {
    filterWorker.postMessage({ action: 'filter', query: e.target.value });
  }, 150);
});

filterWorker.onmessage = (e) => {
  const { results } = e.data;
  document.getElementById('filterStatus').textContent = `${results.length} rows`;
  document.getElementById('filteredBody').innerHTML = results.map(r =>
    `<tr><td>${r.id}</td><td>${r.name}</td><td>${r.category}</td><td>$${r.value}</td></tr>`
  ).join('');
};
</script>
```

## Best Practices

1. Keep worker code self-contained with no DOM access
2. Use `Transferable` objects for large data transfers to avoid copying overhead
3. Implement worker termination and cleanup when processing completes
4. Use Blob URLs for inline worker code to avoid separate file dependencies
5. Limit the number of concurrent workers to navigator.hardwareConcurrency
6. Report progress at reasonable intervals (not every iteration) to avoid message flooding
7. Handle worker errors with `onerror` and display user-friendly failure messages
8. Use structured cloning for complex objects passed to workers
9. Pool and reuse workers instead of creating new ones for each task
10. Test with large datasets to verify UI remains responsive during processing
11. Provide a cancel button that terminates running workers
12. Show estimated time remaining alongside progress indicators
13. Use `SharedArrayBuffer` for high-frequency data sharing when cross-origin isolation is available
14. Log worker lifecycle events for debugging

## Common Pitfalls

1. **DOM access in workers**: Workers cannot access `document` or `window` objects
2. **Message flooding**: Posting progress updates on every iteration overwhelms the main thread
3. **No error handling**: Worker crashes silently without notifying the UI
4. **Memory leaks**: Not terminating workers after task completion
5. **Data copying overhead**: Passing large objects via postMessage creates deep copies
6. **Browser support**: Some older browsers do not support Web Workers
7. **Race conditions**: Multiple workers writing to shared state without synchronization

## Accessibility Considerations

Announce processing status with `aria-live="polite"` regions. Use `aria-busy="true"` on containers during active processing. Ensure progress bars include text labels readable by screen readers. Provide keyboard-accessible cancel buttons for long-running operations. Do not trap focus in processing modals. Announce completion with clear status messages. Use `role="progressbar"` with `aria-valuenow`, `aria-valuemin`, and `aria-valuemax` on progress indicators.

```html
<div class="progress" role="progressbar" aria-valuenow="45" aria-valuemin="0"
     aria-valuemax="100" aria-label="Data processing progress">
  <div class="progress-bar" style="width: 45%">45%</div>
</div>
<div aria-live="polite" class="visually-hidden" id="processAnnounce"></div>
```

## Responsive Behavior

Display worker progress cards in a 2-column grid on desktop (`col-md-6`) and single column on mobile. Use `table-responsive` for filter results on small screens. Reduce progress bar height on mobile with conditional `style` attributes. Stack worker count and record count inputs vertically on mobile using `col-12`. Ensure the processing status text remains visible on all screen sizes. Use `d-grid gap-2` for action buttons on mobile.
