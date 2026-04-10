/**
 * Web Component Libraries Guide - Library patterns for building and managing
 * reusable Web Component libraries with registration, bundling, and distribution
 * @module advanced-patterns/10_1_Web-Component-Libraries-Guide
 * @version 1.0.0
 * @example <library-registry></library-registry>
 */

class ComponentRegistry {
  constructor() {
    this.components = new Map();
    this.dependencies = new Map();
    this.version = '1.0.0';
    this.config = {
      autoRegister: true,
      lazyLoad: false,
      debug: false,
    };
  }

  register(name, definition, options = {}) {
    if (this.components.has(name)) {
      if (!options.force) {
        this.log(`Component ${name} already registered`);
        return false;
      }
    }

    const componentDef = {
      name,
      definition,
      version: options.version || '1.0.0',
      dependencies: options.dependencies || [],
      options: { ...this.config, ...options },
      registeredAt: Date.now(),
    };

    this.components.set(name, componentDef);
    this.processDependencies(componentDef);
    this.log(`Registered: ${name} v${componentDef.version}`);
    return true;
  }

  processDependencies(componentDef) {
    for (const dep of componentDef.dependencies) {
      if (!this.components.has(dep)) {
        this.dependencies.set(dep, []);
      }
      const depList = this.dependencies.get(dep);
      depList.push(componentDef.name);
    }
  }

  unregister(name) {
    const dependents = this.dependencies.get(name) || [];
    if (dependents.length > 0) {
      this.log(`Cannot unregister ${name}, required by: ${dependents.join(', ')}`);
      return false;
    }
    return this.components.delete(name);
  }

  get(name) {
    return this.components.get(name);
  }

  getAll() {
    return Array.from(this.components.values());
  }

  getComponentsByTag(tagName) {
    return this.getAll().filter(c => c.definition.tagName === tagName);
  }

  resolveDependencies(name) {
    const resolved = new Set();
    const queue = [name];

    while (queue.length > 0) {
      const current = queue.shift();
      if (resolved.has(current)) continue;
      resolved.add(current);

      const component = this.components.get(current);
      if (component) {
        queue.push(...component.dependencies);
      }
    }

    return Array.from(resolved);
  }

  log(message) {
    if (this.config.debug) {
      console.log(`[Registry] ${message}`);
    }
  }

  setConfig(config) {
    this.config = { ...this.config, ...config };
  }
}

class ComponentLoader {
  constructor(registry) {
    this.registry = registry;
    this.loadedModules = new Map();
    this.loadingPromises = new Map();
  }

  async load(components, baseUrl = '') {
    const loadOrder = this.resolveLoadOrder(components);
    const results = [];

    for (const name of loadOrder) {
      const result = await this.loadComponent(name, baseUrl);
      results.push(result);
    }

    return results;
  }

  resolveLoadOrder(components) {
    const ordered = [];
    const visited = new Set();
    const visit = (name) => {
      if (visited.has(name)) return;
      visited.add(name);

      const comp = this.registry.get(name);
      if (comp) {
        comp.dependencies.forEach(visit);
      }
      ordered.push(name);
    };

    components.forEach(visit);
    return ordered;
  }

  async loadComponent(name, baseUrl) {
    if (this.loadedModules.has(name)) {
      return this.loadedModules.get(name);
    }

    if (this.loadingPromises.has(name)) {
      return this.loadingPromises.get(name);
    }

    const promise = this.fetchAndRegister(name, baseUrl);
    this.loadingPromises.set(name, promise);

    try {
      const result = await promise;
      this.loadedModules.set(name, result);
      return result;
    } finally {
      this.loadingPromises.delete(name);
    }
  }

  async fetchAndRegister(name, baseUrl) {
    const url = `${baseUrl}/${name}.js`;
    const module = await import(url);
    const definition = module.default || module;

    if (definition) {
      this.registry.register(name, definition);
    }

    return definition;
  }

  preload(components) {
    return components.map(name => {
      return this.loadComponent(name, '').catch(err => {
        console.warn(`Failed to preload ${name}:`, err);
        return null;
      });
    });
  }
}

class LibraryBundler {
  constructor() {
    this.entries = new Set();
    this.externals = new Set();
    this.plugins = [];
  }

  addEntry(name, path) {
    this.entries.add({ name, path });
  }

  addExternal(name) {
    this.externals.add(name);
  }

  usePlugin(plugin) {
    this.plugins.push(plugin);
  }

  async bundle(options = {}) {
    const bundles = [];

    for (const entry of this.entries) {
      const bundle = {
        name: entry.name,
        chunks: await this.createChunks(entry, options),
        external: Array.from(this.externals),
      };
      bundles.push(bundle);
    }

    return bundles;
  }

  async createChunks(entry, options) {
    const chunks = {
      main: entry.path,
      polyfills: [],
      dependencies: [],
      shared: [],
    };

    for (const plugin of this.plugins) {
      if (plugin.transform) {
        await plugin.transform(chunks, entry);
      }
    }

    return chunks;
  }

  generateEntryPoints() {
    const entries = {};

    for (const entry of this.entries) {
      const name = entry.name;
      const filename = name.replace(/^(wc|component)-/, '');
      entries[filename] = entry.path;
    }

    return entries;
  }

  getExternalImports() {
    return Array.from(this.externals).map(name => {
      return { name, import: `import '${name}';` };
    });
  }
}

class ComponentPublisher {
  constructor(registry) {
    this.registry = registry;
    this.distribution = {
      npm: false,
      cdn: false,
      github: false,
    };
  }

  configure(options) {
    this.distribution = { ...this.distribution, ...options };
  }

  async publish(packageJson, options = {}) {
    const results = [];

    if (this.distribution.npm) {
      results.push(await this.publishToNpm(packageJson));
    }

    if (this.distribution.cdn) {
      results.push(await this.publishToCdn(options.cdnUrl));
    }

    if (this.distribution.github) {
      results.push(await this.publishToGitHub(options.repo));
    }

    return results;
  }

  async publishToNpm(packageJson) {
    const packageData = this.generatePackageJson(packageJson);
    return { type: 'npm', data: packageData };
  }

  async publishToCdn(cdnUrl) {
    const components = this.registry.getAll().map(c => ({
      name: c.name,
      tagName: c.definition.tagName,
      version: c.version,
      url: `${cdnUrl}/${c.name}.js`,
    }));
    return { type: 'cdn', components };
  }

  async publishToGitHub(repo) {
    const releaseData = {
      repo,
      components: this.registry.getAll().map(c => c.name),
      version: this.registry.version,
    };
    return { type: 'github', data: releaseData };
  }

  generatePackageJson(basePkg) {
    const exports = {};
    for (const comp of this.registry.getAll()) {
      exports[`./${comp.name}`] = {
        import: `./dist/${comp.name}.js`,
        types: `./dist/${comp.name}.d.ts`,
      };
    }

    return {
      ...basePkg,
      exports,
      type: 'module',
      main: './dist/index.js',
      module: './dist/index.js',
      types: './dist/index.d.ts',
    };
  }

  generateUMD() {
    return this.registry.getAll().map(c => ({
      name: c.name,
      umd: `${c.name}.umd.js`,
    }));
  }
}

const globalRegistry = new ComponentRegistry();
const globalLoader = new ComponentLoader(globalRegistry);
const globalBundler = new LibraryBundler();
const globalPublisher = new ComponentPublisher(globalRegistry);

export { ComponentRegistry, ComponentLoader, LibraryBundler, ComponentPublisher };
export { globalRegistry, globalLoader, globalBundler, globalPublisher };

export default globalRegistry;