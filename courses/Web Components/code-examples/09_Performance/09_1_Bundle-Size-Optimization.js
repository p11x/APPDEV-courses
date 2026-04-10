/**
 * Bundle Size Optimization - Code splitting techniques for web components
 * @module performance/09_1_Bundle-Size-Optimization
 * @version 1.0.0
 * @example <bundle-optimizer></bundle-optimizer>
 */

class BundleOptimizer extends HTMLElement {
  constructor() {
    super();
    this._modules = new Map();
    this._loadedChunks = new Set();
    this._pendingLoads = new Map();
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this._render();
    this._setupLazyLoading();
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 16px;
          font-family: system-ui, sans-serif;
        }
        .chunk-list {
          list-style: none;
          padding: 0;
          margin: 0;
        }
        .chunk-item {
          padding: 8px 12px;
          margin: 4px 0;
          background: #f5f5f5;
          border-radius: 4px;
        }
        .chunk-item.loaded {
          background: #e8f5e9;
        }
        .chunk-item.loading {
          background: #fff3e0;
        }
        button {
          padding: 8px 16px;
          margin: 4px;
          border: 1px solid #ccc;
          border-radius: 4px;
          background: white;
          cursor: pointer;
        }
        button:hover {
          background: #f0f0f0;
        }
      </style>
      <div class="container">
        <h3>Bundle Size Optimizer</h3>
        <div class="controls">
          <button id="load-chunk-1">Load Feature A</button>
          <button id="load-chunk-2">Load Feature B</button>
          <button id="load-chunk-3">Load Feature C</button>
        </div>
        <ul class="chunk-list" id="chunk-list"></ul>
      </div>
    `;
  }

  _setupLazyLoading() {
    const chunkList = this.shadowRoot.getElementById('chunk-list');
    
    this.shadowRoot.querySelectorAll('button').forEach(button => {
      button.addEventListener('click', async (e) => {
        const chunkId = e.target.id.replace('load-chunk-', '');
        await this._loadChunk(chunkId);
        this._updateChunkList();
      });
    });

    this._updateChunkList();
  }

  async _loadChunk(chunkId) {
    if (this._loadedChunks.has(chunkId)) {
      return this._modules.get(chunkId);
    }

    if (this._pendingLoads.has(chunkId)) {
      return this._pendingLoads.get(chunkId);
    }

    const loadPromise = this._fetchChunk(chunkId);
    this._pendingLoads.set(chunkId, loadPromise);
    
    const chunk = await loadPromise;
    this._modules.set(chunkId, chunk);
    this._loadedChunks.add(chunkId);
    this._pendingLoads.delete(chunkId);

    return chunk;
  }

  async _fetchChunk(chunkId) {
    await new Promise(resolve => setTimeout(resolve, 100 + parseInt(chunkId) * 50));
    
    const modules = {
      '1': () => import('./chunk-feature-a.js'),
      '2': () => import('./chunk-feature-b.js'),
      '3': () => import('./chunk-feature-c.js'),
    };

    return modules[chunkId]?.() || null;
  }

  _updateChunkList() {
    const chunkList = this.shadowRoot.getElementById('chunk-list');
    chunkList.innerHTML = '';

    const chunks = [
      { id: '1', name: 'Feature A', description: 'UI Components' },
      { id: '2', name: 'Feature B', description: 'Data Processing' },
      { id: '3', name: 'Feature C', description: 'Charts & Graphs' },
    ];

    chunks.forEach(chunk => {
      const li = document.createElement('li');
      li.className = 'chunk-item';
      
      let status = 'Not loaded';
      if (this._loadedChunks.has(chunk.id)) {
        li.classList.add('loaded');
        status = 'Loaded';
      } else if (this._pendingLoads.has(chunk.id)) {
        li.classList.add('loading');
        status = 'Loading...';
      }

      li.textContent = `${chunk.name}: ${chunk.description} (${status})`;
      chunkList.appendChild(li);
    });
  }

  getLoadedChunks() {
    return Array.from(this._loadedChunks);
  }

  getModule(chunkId) {
    return this._modules.get(chunkId);
  }

  disconnectCallback() {
    this._modules.clear();
    this._loadedChunks.clear();
    this._pendingLoads.clear();
  }
}

export { BundleOptimizer };