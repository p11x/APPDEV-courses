/**
 * Lazy Loading Strategies - Dynamic import patterns for web components
 * @module performance/09_3_Lazy-Loading-Strategies
 * @version 1.0.0
 * @example <lazy-loader></lazy-loader>
 */

class LazyLoader extends HTMLElement {
  constructor() {
    super();
    this._intersectionObserver = null;
    this._loadedElements = new Set();
    this._pendingLoads = new Map();
    this._prefetched = new Set();
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this._render();
    this._setupIntersectionObserver();
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          padding: 16px;
          font-family: system-ui, sans-serif;
        }
        .placeholder {
          min-height: 100px;
          display: flex;
          align-items: center;
          justify-content: center;
          background: #f5f5f5;
          border: 2px dashed #ccc;
          border-radius: 8px;
          cursor: pointer;
          margin: 8px 0;
        }
        .placeholder:hover {
          border-color: #2196f3;
          background: #e3f2fd;
        }
        .placeholder.loading {
          border-color: #ff9800;
          background: #fff3e0;
        }
        .placeholder.loaded {
          border-color: #4caf50;
          background: #e8f5e9;
          cursor: default;
        }
        .placeholder-text {
          font-size: 14px;
          color: #666;
        }
        .spinner {
          width: 24px;
          height: 24px;
          border: 3px solid #f3f3f3;
          border-top: 3px solid #ff9800;
          border-radius: 50%;
          animation: spin 1s linear infinite;
        }
        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }
      </style>
      <div class="container">
        <h3>Lazy Loading Strategies</h3>
        <div class="placeholder" data-id="widget-a">
          <span class="placeholder-text">Widget A (Scroll to load)</span>
        </div>
        <div class="placeholder" data-id="widget-b">
          <span class="placeholder-text">Widget B (Scroll to load)</span>
        </div>
        <div class="placeholder" data-id="widget-c">
          <span class="placeholder-text">Widget C (Scroll to load)</span>
        </div>
        <div class="placeholder" data-id="widget-d">
          <span class="placeholder-text">Widget D (Click to load)</span>
        </div>
      </div>
    `;
  }

  _setupIntersectionObserver() {
    this._intersectionObserver = new IntersectionObserver(
      (entries) => {
        entries.forEach(entry => {
          if (entry.isIntersecting && !this._loadedElements.has(entry.target.dataset.id)) {
            this._loadElement(entry.target.dataset.id);
          }
        });
      },
      {
        rootMargin: '100px',
        threshold: 0.1
      }
    );

    this.shadowRoot.querySelectorAll('.placeholder').forEach(el => {
      if (!el.dataset.id.includes('d')) {
        this._intersectionObserver.observe(el);
      }
    });

    this.shadowRoot.querySelectorAll('.placeholder[data-id="widget-d"]').forEach(el => {
      el.addEventListener('click', () => {
        this._loadElement('widget-d');
      });
    });
  }

  async _loadElement(id) {
    if (this._loadedElements.has(id) || this._pendingLoads.has(id)) {
      return;
    }

    const placeholder = this.shadowRoot.querySelector(`[data-id="${id}"]`);
    if (!placeholder) return;

    placeholder.classList.add('loading');
    placeholder.innerHTML = '<div class="spinner"></div>';

    const loadPromise = this._fetchModule(id);
    this._pendingLoads.set(id, loadPromise);

    try {
      const module = await loadPromise;
      this._loadedElements.add(id);
      this._pendingLoads.delete(id);

      if (module && module.default) {
        placeholder.innerHTML = module.default;
      } else {
        placeholder.innerHTML = `<span class="placeholder-text">${id} loaded successfully</span>`;
      }
      placeholder.classList.remove('loading');
      placeholder.classList.add('loaded');
    } catch (error) {
      placeholder.innerHTML = `<span class="placeholder-text">Error loading ${id}</span>`;
      placeholder.classList.remove('loading');
    }
  }

  async _fetchModule(id) {
    const modules = {
      'widget-a': () => import('./lazy-widget-a.js'),
      'widget-b': () => import('./lazy-widget-b.js'),
      'widget-c': () => import('./lazy-widget-c.js'),
      'widget-d': () => import('./lazy-widget-d.js'),
    };

    return modules[id]() || null;
  }

  prefetch(id) {
    if (this._prefetched.has(id)) return;

    this._prefetched.add(id);
    
    if ('link' in document) {
      const link = document.createElement('link');
      link.rel = 'prefetch';
      link.href = `./lazy-${id}.js`;
      document.head.appendChild(link);
    }
  }

  isLoaded(id) {
    return this._loadedElements.has(id);
  }

  getLoadedElements() {
    return Array.from(this._loadedElements);
  }

  disconnectCallback() {
    if (this._intersectionObserver) {
      this._intersectionObserver.disconnect();
    }
    this._loadedElements.clear();
    this._pendingLoads.clear();
  }
}

export { LazyLoader };