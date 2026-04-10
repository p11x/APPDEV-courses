/**
 * Monorepo Management Patterns - Nx and Turborepo patterns for managing
 * Web Component monorepos with shared configs, build optimization, and caching
 * @module advanced-patterns/10_9_Monorepo-Management-Patterns
 * @version 1.0.0
 * @example <monorepo-component></monorepo-component>
 */

class WorkspaceManager {
  constructor(root) {
    this.root = root;
    this.projects = new Map();
    this.dependencies = new Map();
    this.graph = new Map();
  }

  addProject(config) {
    this.projects.set(config.name, {
      ...config,
      path: config.path || `packages/${config.name}`,
      targets: config.targets || {},
    });

    this.buildGraph(config.name);
  }

  buildGraph(projectName) {
    const project = this.projects.get(projectName);
    if (!project) return;

    const deps = this.dependencies.get(projectName) || [];
    const graphNode = {
      project: projectName,
      dependencies: [],
      dependents: [],
    };

    for (const dep of deps) {
      graphNode.dependencies.push(dep);
      const depNode = this.graph.get(dep);
      if (depNode) {
        depNode.dependents.push(projectName);
      }
    }

    this.graph.set(projectName, graphNode);
  }

  setDependencies(projectName, deps) {
    this.dependencies.set(projectName, deps);
    this.buildGraph(projectName);
  }

  getAffected(since, options = {}) {
    const { shallow = false } = options;
    const affected = new Set();
    const queue = [...this.getChanged(since)];

    while (queue.length > 0) {
      const current = queue.shift();
      if (affected.has(current)) continue;
      affected.add(current);

      if (!shallow) {
        const node = this.graph.get(current);
        if (node) {
          queue.push(...node.dependents);
        }
      }
    }

    return Array.from(affected);
  }

  getChanged(since) {
    return [];
  }

  getTopologicalOrder() {
    const visited = new Set();
    const order = [];

    const visit = (name) => {
      if (visited.has(name)) return;
      visited.add(name);

      const node = this.graph.get(name);
      if (node) {
        for (const dep of node.dependencies) {
          visit(dep);
        }
      }

      order.push(name);
    };

    for (const projectName of this.projects.keys()) {
      visit(projectName);
    }

    return order;
  }

  getProject(name) {
    return this.projects.get(name);
  }

  getAllProjects() {
    return Array.from(this.projects.values());
  }

  validate() {
    const errors = [];

    for (const [name, deps] of this.dependencies) {
      for (const dep of deps) {
        if (!this.projects.has(dep)) {
          errors.push(`Project ${name} depends on missing project ${dep}`);
        }
      }
    }

    return { valid: errors.length === 0, errors };
  }
}

class BuildCache {
  constructor() {
    this.cache = new Map();
    this.enabled = true;
    this.hashAlgo = 'sha256';
  }

  async get(key) {
    if (!this.enabled) return null;

    const hash = await this.hash(key);
    return this.cache.get(hash);
  }

  async set(key, value) {
    const hash = await this.hash(key);
    this.cache.set(hash, {
      value,
      timestamp: Date.now(),
    });
  }

  async hash(data) {
    const encoder = new TextEncoder();
    const dataBuffer = encoder.encode(JSON.stringify(data));

    const hashBuffer = await crypto.subtle.digest(this.hashAlgo, dataBuffer);
    const hashArray = Array.from(new Uint8Array(hashBuffer));

    return hashArray.map(b => b.toString(16).padStart(2, '0')).join('');
  }

  async isValid(key, maxAge) {
    const hash = await this.hash(key);
    const entry = this.cache.get(hash);

    if (!entry) return false;

    if (maxAge) {
      const age = Date.now() - entry.timestamp;
      if (age > maxAge) return false;
    }

    return true;
  }

  clear() {
    this.cache.clear();
  }

  enable() {
    this.enabled = true;
  }

  disable() {
    this.enabled = false;
  }
}

class ParallelBuilder {
  constructor(workspace) {
    this.workspace = workspace;
    this.cache = new BuildCache();
    this.running = new Map();
  }

  async build(projectNames, options = {}) {
    const { parallel = true, concurrency = 4, cache = true } = options;
    const results = [];

    if (parallel) {
      const batches = this.getBatches(projectNames);

      for (const batch of batches) {
        const batchResults = await Promise.all(
          batch.map(name => this.buildProject(name, options))
        );
        results.push(...batchResults);
      }
    } else {
      for (const name of projectNames) {
        results.push(await this.buildProject(name, options));
      }
    }

    return results;
  }

  getBatches(projectNames) {
    const order = this.workspace.getTopologicalOrder();
    const batches = [];
    const processed = new Set();

    for (const name of order) {
      if (!projectNames.includes(name)) continue;
      if (processed.has(name)) continue;

      const project = this.workspace.projects.get(name);
      const deps = this.workspace.dependencies.get(name) || [];

      const ready = deps.every(d => processed.has(d));
      if (ready) {
        batches.push([name]);
        processed.add(name);
      }
    }

    return batches;
  }

  async buildProject(name, options) {
    if (this.running.has(name)) {
      return this.running.get(name);
    }

    const promise = this.doBuild(name, options);
    this.running.set(name, promise);

    try {
      const result = await promise;
      return result;
    } finally {
      this.running.delete(name);
    }
  }

  async doBuild(name, options) {
    const project = this.workspace.getProject(name);
    if (!project) return { success: false, error: 'Project not found' };

    const cacheKey = { name, targets: project.targets };
    const cached = await this.cache.get(cacheKey);

    if (cached && options.cache !== false) {
      return { success: true, cached: true, ...cached };
    }

    return { success: true, output: {} };
  }

  clearCache() {
    this.cache.clear();
  }
}

class TaskExecutor {
  constructor(workspace) {
    this.workspace = workspace;
    this.running = new Set();
    this.completed = new Map();
  }

  async run(target, options = {}) {
    const { parallel = false, failFast = true } = options;
    const projects = this.workspace.getAllProjects();
    const results = [];

    for (const project of projects) {
      const targetConfig = project.targets[target];
      if (!targetConfig) continue;

      const result = await this.runTarget(project.name, target, targetConfig);

      results.push({ project: project.name, target, ...result });

      if (!result.success && failFast) {
        throw new Error(`Target ${target} failed for ${project.name}`);
      }
    }

    return results;
  }

  async runTarget(projectName, target, config) {
    const startTime = Date.now();

    try {
      if (config.command) {
        await this.executeCommand(config.command);
      }

      return {
        success: true,
        duration: Date.now() - startTime,
      };
    } catch (error) {
      return {
        success: false,
        error: error.message,
        duration: Date.now() - startTime,
      };
    }
  }

  async executeCommand(cmd) {
    return { output: '', exitCode: 0 };
  }

  getStatus(projectName) {
    return {
      running: this.running.has(projectName),
      completed: this.completed.get(projectName),
    };
  }

  isRunning(projectName) {
    return this.running.has(projectName);
  }
}

class PackageManager {
  constructor(workspace) {
    this.workspace = workspace;
    this.packageJson = new Map();
  }

  loadProjectPackage(projectName) {
    const project = this.workspace.getProject(projectName);
    if (!project) return null;

    return this.packageJson.get(projectName) || { name: projectName, version: '1.0.0' };
  }

  updateDependency(projectName, dependency, version, type = 'dependencies') {
    const pkg = this.loadProjectPackage(projectName);
    if (!pkg) return;

    if (!pkg[type]) {
      pkg[type] = {};
    }

    pkg[type][dependency] = version;
  }

  resolveVersion(dependency, lockfile = {}) {
    return lockfile[dependency] || '*';
  }

  dedupe(dependencies) {
    const resolved = new Map();

    for (const [name, version] of Object.entries(dependencies)) {
      if (!resolved.has(name) || this.compareVersions(version, resolved.get(name)) > 0) {
        resolved.set(name, version);
      }
    }

    return Object.fromEntries(resolved);
  }

  compareVersions(a, b) {
    const pa = a.replace(/[\^~]/, '').split('.');
    const pb = b.replace(/[\^~]/, '').split('.');

    for (let i = 0; i < Math.max(pa.length, pb.length); i++) {
      const na = parseInt(pa[i]) || 0;
      const nb = parseInt(pb[i]) || 0;
      if (na !== nb) return na - nb;
    }

    return 0;
  }
}

class NxConfig {
  constructor() {
    this.plugins = [];
    this.namedInputs = {};
    this.targetDefaults = {};
  }

  addPlugin(plugin) {
    this.plugins.push(plugin);
  }

  setNamedInput(name, input) {
    this.namedInputs[name] = input;
  }

  setTargetDefaults(target, defaults) {
    this.targetDefaults[target] = defaults;
  }

  toJSON() {
    return JSON.stringify({
      plugins: this.plugins,
      namedInputs: this.namedInputs,
      targetDefaults: this.targetDefaults,
    }, null, 2);
  }
}

const workspaceManager = new WorkspaceManager('.');
const buildCache = new BuildCache();
const parallelBuilder = new ParallelBuilder(workspaceManager);
const taskExecutor = new TaskExecutor(workspaceManager);
const packageManager = new PackageManager(workspaceManager);
const nxConfig = new NxConfig();

export { WorkspaceManager, BuildCache, ParallelBuilder, TaskExecutor, PackageManager, NxConfig };
export { workspaceManager, buildCache, parallelBuilder, taskExecutor, packageManager, nxConfig };

export default workspaceManager;