/**
 * Template Cloning and Instantiation
 * @description Patterns for efficiently cloning templates and instantiating custom elements from templates
 * @module templates/cloning
 * @version 1.0.0
 * @example <clone-instantiator></clone-instantiator>
 */

// ============================================================================
// Clone Instantiator Component
// ============================================================================

/**
 * Custom element that provides deep cloning capabilities with callbacks and instance management.
 * @description Efficiently clones nested template structures with lifecycle callbacks
 * @extends HTMLElement
 * @example
 * const instantiator = document.querySelector('clone-instantiator');
 * const clone = instantiator.cloneTemplate(template, { deep: true });
 */
class CloneInstantiator extends HTMLElement {
  /**
   * Default configuration options for cloning operations.
   * @type {Object}
   * @readonly
   * @property {boolean} deep - Whether to perform deep cloning (default: true)
   * @property {boolean} includeEvents - Whether to clone event listeners (default: false)
   * @property {boolean} includeStyles - Whether to clone inline styles (default: true)
   * @property {string} namespace - Optional namespace for cloned element IDs
   */
  static get defaultOptions() {
    return Object.freeze({
      deep: true,
      includeEvents: false,
      includeStyles: true,
      namespace: null
    });
  }

  /**
   * Custom element tag name.
   * @type {string}
   * @readonly
   */
  static get tagName() {
    return 'clone-instantiator';
  }

  /**
   * Creates a new CloneInstantiator instance.
   * @constructor
   */
  constructor() {
    super();
    this._initialized = false;
    this._clones = new Map();
    this._cloneIdCounter = 0;
    this._observers = new Map();
    this._boundHandlers = new WeakMap();
    this._options = { ...CloneInstantiator.defaultOptions };
    this._maxClones = 100;
    this._cleanupScheduled = false;
    this._observer = null;
  }

  /**
   * Called when the element is added to the document.
   * @description Sets up cloning infrastructure and initializes observers
   */
  connectedCallback() {
    if (this._initialized) return;
    this._initialized = true;
    this._setupMutationObserver();
    this._setupEventBridge();
    this._initializeTemplates();
    this.dispatchEvent(new CustomEvent('clone-instantiator:ready', { bubbles: true }));
  }

  /**
   * Called when the element is removed from the document.
   * @description Performs cleanup of clones and observers
   */
  disconnectedCallback() {
    this._cleanupAll();
    this._disposeObserver();
    this._initialized = false;
  }

  /**
   * Sets up mutation observer to track child changes.
   * @private
   */
  _setupMutationObserver() {
    if (!this._observer) {
      this._observer = new MutationObserver((mutations) => this._handleMutations(mutations));
      this._observer.observe(this, {
        childList: true,
        subtree: true,
        attributes: true,
        attributeOldValue: true
      });
    }
  }

  /**
   * Handles mutation observer events.
   * @private
   * @param {MutationRecord[]} mutations - Array of mutation records
   */
  _handleMutations(mutations) {
    const cloneEvent = new CustomEvent('clone-instantiator:mutated', {
      bubbles: true,
      cancelable: true,
      detail: { mutations }
    });
    this.dispatchEvent(cloneEvent);
  }

  /**
   * Disposes of the mutation observer.
   * @private
   */
  _disposeObserver() {
    if (this._observer) {
      this._observer.disconnect();
      this._observer = null;
    }
  }

  /**
   * Sets up event bridge for cross-frame cloning.
   * @private
   */
  _setupEventBridge() {
    window.addEventListener('message', this._handleMessage.bind(this));
  }

  /**
   * Handles messages from other frames.
   * @private
   * @param {MessageEvent} event - The message event
   */
  _handleMessage(event) {
    if (event.data?.type === 'clone-request') {
      this._handleCloneRequest(event);
    }
  }

  /**
   * Processes clone requests from other frames.
   * @private
   * @param {MessageEvent} event - The message event
   */
  _handleCloneRequest(event) {
    const { templateId, options, requestId } = event.data;
    try {
      const template = this.getTemplate(templateId);
        const clone = this.cloneTemplate(template, options);
        event.source.postMessage({
          type: 'clone-response',
          requestId,
          success: true,
          clone: clone.outerHTML
        }, event.origin);
    } catch (error) {
      event.source.postMessage({
        type: 'clone-response',
        requestId,
        success: false,
        error: error.message
      }, event.origin);
    }
  }

  /**
   * Initializes default templates from the DOM.
   * @private
   */
  _initializeTemplates() {
    const templates = this.querySelectorAll('template');
    templates.forEach((template, index) => {
      const id = template.id || `default-template-${index}`;
      this.registerTemplate(id, template);
    });
  }

  /**
   * Clones a template with the specified options.
   * @description Performs deep or shallow cloning with various options
   * @param {string|HTMLTemplateElement|HTMLElement} template - Template source or ID
   * @param {Object} [options={}] - Cloning options
   * @param {boolean} [options.deep=true] - Deep clone flag
   * @param {boolean} [options.includeEvents=false] - Include event listeners
   * @param {boolean} [options.includeStyles=true] - Include inline styles
   * @param {string} [options.namespace] - Namespace for IDs
   * @returns {DocumentFragment|HTMLElement} Cloned template
   * @throws {Error} If template not found or cloning fails
   * @example
   * const clone = instantiator.cloneTemplate('my-template', { deep: true });
   */
  cloneTemplate(template, options = {}) {
    const mergedOptions = { ...this._options, ...options };
    const sourceTemplate = this._resolveTemplate(template);
    
    if (!sourceTemplate) {
      throw new Error(`Template not found: ${template}`);
    }

    try {
      const clone = this._performClone(sourceTemplate, mergedOptions);
      const cloneId = this._registerClone(clone, mergedOptions);
      
      this.dispatchEvent(new CustomEvent('clone-instantiator:cloned', {
        bubbles: true,
        detail: { clone, cloneId, options: mergedOptions }
      }));

      return clone;
    } catch (error) {
      this.dispatchEvent(new CustomEvent('clone-instantiator:error', {
        bubbles: true,
        detail: { error, template }
      }));
      throw error;
    }
  }

  /**
   * Resolves template from various input formats.
   * @private
   * @param {string|HTMLTemplateElement|HTMLElement} template - Template source
   * @returns {HTMLTemplateElement|HTMLElement|null}
   */
  _resolveTemplate(template) {
    if (template instanceof HTMLElement) {
      return template;
    }
    if (typeof template === 'string') {
      return this.getTemplate(template);
    }
    return null;
  }

  /**
   * Performs the actual cloning operation.
   * @private
   * @param {HTMLElement} template - Template to clone
   * @param {Object} options - Cloning options
   * @returns {HTMLElement|DocumentFragment}
   */
  _performClone(template, options) {
    let clone;
    
    if (template instanceof HTMLTemplateElement) {
      clone = template.content.cloneNode(options.deep);
    } else if (options.deep) {
      clone = template.cloneNode(true);
    } else {
      clone = template.cloneNode(false);
    }

    if (options.includeStyles) {
      this._cloneStyles(template, clone);
    }

    if (options.namespace) {
      this._remapIds(clone, options.namespace);
    }

    return clone;
  }

  /**
   * Clones inline styles from source to clone.
   * @private
   * @param {HTMLElement} source - Source element
   * @param {HTMLElement} clone - Target element
   */
  _cloneStyles(source, clone) {
    const sourceStyle = source.getAttribute('style');
    if (sourceStyle) {
      clone.setAttribute('style', sourceStyle);
    }
  }

  /**
   * Remaps element IDs with namespace prefix.
   * @private
   * @param {HTMLElement} root - Root element
   * @param {string} namespace - Namespace to apply
   */
  _remapIds(root, namespace) {
    const elements = root.querySelectorAll('[id]');
    elements.forEach(el => {
      el.id = `${namespace}-${el.id}`;
    });
  }

  /**
   * Registers a clone in the tracking map.
   * @private
   * @param {HTMLElement} clone - Cloned element
   * @param {Object} options - Cloning options
   * @returns {string} Clone identifier
   */
  _registerClone(clone, options) {
    const cloneId = options.id || `clone-${++this._cloneIdCounter}`;
    clone.dataset.cloneId = cloneId;
    
    this._clones.set(cloneId, {
      element: clone,
      timestamp: Date.now(),
      options
    });

    if (this._clones.size > this._maxClones) {
      this._scheduleCleanup();
    }

    return cloneId;
  }

  /**
   * Schedules cleanup of old clones.
   * @private
   */
  _scheduleCleanup() {
    if (this._cleanupScheduled) return;
    this._cleanupScheduled = true;
    requestIdleCallback(() => this._cleanupOldClones());
  }

  /**
   * Cleans up old clones beyond the maximum.
   * @private
   */
  _cleanupOldClones() {
    if (this._clones.size <= this._maxClones) {
      this._cleanupScheduled = false;
      return;
    }

    const sorted = [...this._clones.entries()].sort((a, b) => a[1].timestamp - b[1].timestamp);
    const toRemove = sorted.slice(0, this._clones.size - this._maxClones);
    
    toRemove.forEach(([id]) => this.releaseClone(id));
    this._cleanupScheduled = false;
  }

  /**
   * Gets a registered clone by ID.
   * @param {string} cloneId - Clone identifier
   * @returns {HTMLElement|null}
   * @example
   * const clone = instantiator.getClone('clone-5');
   */
  getClone(cloneId) {
    const entry = this._clones.get(cloneId);
    return entry?.element || null;
  }

  /**
   * Releases a clone, removing it from tracking.
   * @param {string} cloneId - Clone identifier
   * @returns {boolean} True if clone was released
   * @example
   * instantiator.releaseClone('clone-5');
   */
  releaseClone(cloneId) {
    const entry = this._clones.get(cloneId);
    if (!entry) return false;

    const clone = entry.element;
    this._dispatchCleanup(clone);
    clone.remove();
    this._clones.delete(cloneId);
    
    this.dispatchEvent(new CustomEvent('clone-instantiator:released', {
      bubbles: true,
      detail: { cloneId }
    }));

    return true;
  }

  /**
   * Dispatches cleanup event for a clone.
   * @private
   * @param {HTMLElement} clone - Clone to clean up
   */
  _dispatchCleanup(clone) {
    clone.dispatchEvent(new CustomEvent('clone:cleanup', {
      bubbles: true,
      cancelable: true
    }));
  }

  /**
   * Gets all registered clones.
   * @returns {Array<{id: string, element: HTMLElement, timestamp: number}>}
   */
  getAllClones() {
    return [...this._clones.entries()].map(([id, entry]) => ({
      id,
      element: entry.element,
      timestamp: entry.timestamp,
      options: entry.options
    }));
  }

  /**
   * Clears all tracked clones.
   * @returns {number} Number of clones cleared
   */
  clearAllClones() {
    const count = this._clones.size;
    this._clones.forEach((_, id) => this.releaseClone(id));
    this._cloneIdCounter = 0;
    return count;
  }

  /**
   * Sets the maximum number of clones to track.
   * @param {number} max - Maximum clones
   * @throws {Error} If max is not a positive integer
   */
  setMaxClones(max) {
    if (!Number.isInteger(max) || max < 1) {
      throw new Error('Max clones must be a positive integer');
    }
    this._maxClones = max;
  }

  /**
   * Registers a template for cloning.
   * @param {string} id - Template identifier
   * @param {HTMLTemplateElement} template - Template element
   * @returns {boolean} True if registered successfully
   * @example
   * instantiator.registerTemplate('card', document.getElementById('card-template'));
   */
  registerTemplate(id, template) {
    if (!(template instanceof HTMLTemplateElement)) {
      console.error('Invalid template element provided');
      return false;
    }
    
    this._templates.set(id, template.cloneNode(true));
    this.dispatchEvent(new CustomEvent('clone-instantiator:registered', {
      bubbles: true,
      detail: { templateId: id }
    }));
    return true;
  }

  /**
   * Gets a registered template.
   * @param {string} id - Template identifier
   * @returns {HTMLTemplateElement|null}
   * @example
   * const template = instantiator.getTemplate('card');
   */
  getTemplate(id) {
    return this._templates.get(id) || null;
  }

  /**
   * Performs complete cleanup on disconnect.
   * @private
   */
  _cleanupAll() {
    this.clearAllClones();
    this._observers.forEach(obs => obs.disconnect());
    this._observers.clear();
  }

  /**
   * Internal templates storage.
   * @type {Map<string, HTMLTemplateElement>}
   * @private
   */
  get _templates() {
    if (!this.__templates) {
      this.__templates = new Map();
    }
    return this.__templates;
  }
}

// ============================================================================
// Instance Pool
// ============================================================================

/**
 * Object pooling system for efficient instance management.
 * @description Manages a pool of reusable instances to reduce allocation overhead
 * @example
 * const pool = new InstancePool(createWidget, { minSize: 2, maxSize: 10 });
 * const widget = pool.acquire();
 * pool.release(widget);
 */
class InstancePool {
  /**
   * Creates a new InstancePool.
   * @constructor
   * @param {Function} factory - Factory function to create instances
   * @param {Object} [options={}] - Pool configuration
   * @param {number} [options.minSize=0] - Minimum pool size
   * @param {number} [options.maxSize=10] - Maximum pool size
   * @param {number} [options.initialSize=0] - Initial pool size
   * @param {number} [options.idleTimeout=30000] - Idle timeout in ms
   * @param {Function} [options.validator] - Instance validation function
   * @param {Function} [options.reset] - Instance reset function
   * @param {Function} [options.dispose] - Instance disposal function
   * @throws {Error} If factory is not a function
   */
  constructor(factory, options = {}) {
    if (typeof factory !== 'function') {
      throw new Error('Factory must be a function');
    }

    this._factory = factory;
    this._options = {
      minSize: 0,
      maxSize: 10,
      initialSize: 0,
      idleTimeout: 30000,
      validator: null,
      reset: null,
      dispose: null,
      ...options
    };

    this._pool = [];
    this._active = new Set();
    this._stats = {
      acquired: 0,
      released: 0,
      created: 0,
      disposed: 0,
      failed: 0
    };

    this._initPool();
    this._startCleanupInterval();
  }

  /**
   * Initializes the pool with initial instances.
   * @private
   */
  async _initPool() {
    const { initialSize } = this._options;
    if (initialSize <= 0) return;

    const promises = [];
    for (let i = 0; i < initialSize; i++) {
      promises.push(this._createInstance());
    }

    try {
      const instances = await Promise.all(promises);
      instances.forEach(instance => {
        if (instance) this._pool.push(instance);
      });
    } catch (error) {
      console.error('Failed to initialize pool:', error);
    }
  }

  /**
   * Creates a new instance using the factory.
   * @private
   * @returns {Promise<Object>}
   */
  async _createInstance() {
    try {
      const instance = await Promise.resolve(this._factory());
      if (this._options.validator && !this._options.validator(instance)) {
        throw new Error('Instance validation failed');
      }
      this._stats.created++;
      return instance;
    } catch (error) {
      this._stats.failed++;
      throw error;
    }
  }

  /**
   * Starts the cleanup interval for idle instances.
   * @private
   */
  _startCleanupInterval() {
    this._cleanupInterval = setInterval(
      () => this._cleanupIdle(),
      this._options.idleTimeout / 2
    );
  }

  /**
   * Cleans up idle instances beyond minimum.
   * @private
   */
  _cleanupIdle() {
    const { minSize } = this._options;
    while (this._pool.length > minSize) {
      const instance = this._pool.pop();
      this._disposeInstance(instance);
    }
  }

  /**
   * Disposes of an instance.
   * @private
   * @param {Object} instance - Instance to dispose
   */
  _disposeInstance(instance) {
    if (this._options.dispose) {
      try {
        this._options.dispose(instance);
      } catch (error) {
        console.error('Dispose failed:', error);
      }
    }
    this._stats.disposed++;
  }

  /**
   * Acquires an instance from the pool.
   * @description Gets an instance, creating one if the pool is empty
   * @returns {Promise<Object>} Acquired instance
   * @throws {Error} If no instance can be acquired
   * @example
   * const widget = await pool.acquire();
   * widget.render(data);
   */
  async acquire() {
    if (this._pool.length > 0) {
      const instance = this._pool.pop();
      
      if (this._options.validator && !this._options.validator(instance)) {
        this._disposeInstance(instance);
        return this.acquire();
      }

      this._stats.acquired++;
      this._active.add(instance);
      return instance;
    }

    if (this._active.size >= this._options.maxSize) {
      return new Promise((resolve, reject) => {
        const timeout = setTimeout(() => {
          this._pending?.delete({ resolve, reject });
          reject(new Error('Pool acquisition timeout'));
        }, 5000);

        this._pending = this._pending || new Set();
        this._pending.add({ resolve, reject, timeout });
      });
    }

    const instance = await this._createInstance();
    this._active.add(instance);
    this._stats.acquired++;
    return instance;
  }

  /**
   * Releases an instance back to the pool.
   * @description Returns an instance to the pool for reuse
   * @param {Object} instance - Instance to release
   * @returns {boolean} True if instance was released
   * @example
   * pool.release(widget);
   */
  release(instance) {
    if (!instance) return false;
    
    this._active.delete(instance);

    if (this._pool.length >= this._options.maxSize) {
      this._disposeInstance(instance);
      this._stats.released++;
      return false;
    }

    if (this._options.reset) {
      try {
        this._options.reset(instance);
      } catch (error) {
        console.error('Reset failed:', error);
        this._disposeInstance(instance);
        this._stats.released++;
        return false;
      }
    }

    this._pool.push(instance);
    this._stats.released++;
    this._handlePending();
    return true;
  }

  /**
   * Handles pending acquisition requests.
   * @private
   */
  _handlePending() {
    if (!this._pending?.size) return;
    
    const pending = this._pending.values().next().value;
    if (!pending) return;

    clearTimeout(pending.timeout);
    this._pending.delete(pending);
    
    const instance = this._pool.pop();
    if (instance) {
      this._active.add(instance);
      pending.resolve(instance);
    }
  }

  /**
   * Gets current pool statistics.
   * @returns {Object} Pool statistics
   * @readonly
   * @property {number} size - Current pool size
   * @property {number} active - Active instance count
   * @property {number} available - Available instance count
   * @property {number} acquired - Total acquisitions
   * @property {number} released - Total releases
   * @property {number} created - Total created
   * @property {number} disposed - Total disposed
   * @property {number} failed - Creation failures
   * @example
   * const stats = pool.getStats();
   * console.log(`Active: ${stats.active}/${stats.size}`);
   */
  getStats() {
    return {
      size: this._pool.length + this._active.size,
      active: this._active.size,
      available: this._pool.length,
      ...this._stats
    };
  }

  /**
   * Checks if the pool contains a specific instance.
   * @param {Object} instance - Instance to check
   * @returns {boolean}
   */
  has(instance) {
    return this._active.has(instance) || this._pool.includes(instance);
  }

  /**
   * Gets all active instances.
   * @returns {Array<Object>}
   */
  getActiveInstances() {
    return [...this._active];
  }

  /**
   * Drains the pool, disposing all instances.
   * @returns {number} Number of instances disposed
   */
  drain() {
    this._disposeAll(this._pool);
    this._disposeAll([...this._active]);
    
    const count = this._pool.length + this._active.size;
    this._pool = [];
    this._active.clear();
    this._stats.disposed += count;
    
    return count;
  }

  /**
   * Disposes of all instances in an array.
   * @private
   * @param {Array<Object>} instances - Instances to dispose
   */
  _disposeAll(instances) {
    instances.forEach(instance => this._disposeInstance(instance));
  }

  /**
   * Resets the pool to initial state.
   * @param {number} [initialSize] - Optional new initial size
   * @returns {Promise<number>}
   */
  async reset(initialSize) {
    this.drain();
    
    if (initialSize !== undefined) {
      this._options.initialSize = initialSize;
    }

    this._stats = {
      acquired: 0,
      released: 0,
      created: 0,
      disposed: 0,
      failed: 0
    };

    await this._initPool();
    return this._pool.length;
  }

  /**
   * Disposes of the pool.
   * @description Cleanup method to release all resources
   */
  dispose() {
    clearInterval(this._cleanupInterval);
    this.drain();
    this._pending?.clear();
  }
}

// ============================================================================
// Clone Manager - Factory for Clone Operations
// ============================================================================

/**
 * Factory for managing multiple clone instantiators.
 * @description Provides centralized template cloning management
 * @example
 * const manager = new CloneManager();
 * const clone = manager.create('header', { deep: true });
 */
class CloneManager {
  /**
   * Creates a new CloneManager.
   * @constructor
   */
  constructor() {
    this._instantiators = new Map();
    this._templates = new Map();
  }

  /**
   * Registers a default instantiator.
   * @param {CloneInstantiator} instantiator - Instantiator to register
   */
  registerInstantiator(instantiator) {
    this._instantiators.set('default', instantiator);
  }

  /**
   * Gets an instantiator by name.
   * @param {string} [name='default'] - Instantiator name
   * @returns {CloneInstantiator|undefined}
   */
  getInstantiator(name = 'default') {
    return this._instantiators.get(name);
  }

  /**
   * Creates a clone using the registered instantiator.
   * @param {string} templateId - Template identifier
   * @param {Object} [options={}] - Clone options
   * @returns {HTMLElement}
   */
  create(templateId, options = {}) {
    const instantiator = this.getInstantiator();
    if (!instantiator) {
      throw new Error('No instantiator registered');
    }
    return instantiator.cloneTemplate(templateId, options);
  }
}

// ============================================================================
// Export
// ============================================================================

export { CloneInstantiator, InstancePool, CloneManager };