/**
 * @group unit
 * @group advanced-patterns
 */
import { expect, fixture, html } from '@open-wc/testing';
import './10_9_Monorepo-Management-Patterns.js';

describe('WorkspaceManager', () => {
  let workspace;

  beforeEach(() => {
    workspace = new WorkspaceManager('./');
  });

  it('should add projects', () => {
    workspace.addProject({ name: 'ui-components', path: 'packages/ui' });
    const project = workspace.getProject('ui-components');
    expect(project.name).to.equal('ui-components');
  });

  it('should set dependencies', () => {
    workspace.addProject({ name: 'a' });
    workspace.addProject({ name: 'b' });
    workspace.setDependencies('b', ['a']);
    const node = workspace.graph.get('b');
    expect(node.dependencies).to.include('a');
  });

  it('should get affected projects', () => {
    workspace.addProject({ name: 'a' });
    workspace.addProject({ name: 'b' });
    workspace.setDependencies('b', ['a']);
    const affected = workspace.getAffected('HEAD~1');
    expect(affected).to.be.an('array');
  });

  it('should get topological order', () => {
    workspace.addProject({ name: 'a' });
    workspace.addProject({ name: 'b' });
    workspace.setDependencies('b', ['a']);
    const order = workspace.getTopologicalOrder();
    expect(order.indexOf('a')).to.be.lessThan(order.indexOf('b'));
  });

  it('should validate workspace', () => {
    workspace.addProject({ name: 'a' });
    workspace.setDependencies('a', ['nonexistent']);
    const validation = workspace.validate();
    expect(validation.valid).to.be.false;
    expect(validation.errors.length).to.be.greaterThan(0);
  });

  it('should get all projects', () => {
    workspace.addProject({ name: 'a' });
    workspace.addProject({ name: 'b' });
    const all = workspace.getAllProjects();
    expect(all.length).to.equal(2);
  });
});

describe('BuildCache', () => {
  let cache;

  beforeEach(() => {
    cache = new BuildCache();
  });

  it('should get and set cache', async () => {
    await cache.set('key', { data: 'value' });
    const result = await cache.get('key');
    expect(result).to.exist;
  });

  it('should hash data', async () => {
    const hash = await cache.hash({ test: 'data' });
    expect(hash).to.be.a('string');
    expect(hash.length).to.be.greaterThan(0);
  });

  it('should check validity', async () => {
    await cache.set('key', { data: 'value' });
    const valid = await cache.isValid('key');
    expect(valid).to.be.true;
  });

  it('should clear cache', async () => {
    await cache.set('key', { data: 'value' });
    cache.clear();
    const result = await cache.get('key');
    expect(result).to.be.null;
  });

  it('should enable and disable', () => {
    cache.disable();
    expect(cache.enabled).to.be.false;
    cache.enable();
    expect(cache.enabled).to.be.true;
  });
});

describe('ParallelBuilder', () => {
  let workspace;
  let builder;

  beforeEach(() => {
    workspace = new WorkspaceManager('./');
    workspace.addProject({ name: 'a' });
    workspace.addProject({ name: 'b' });
    builder = new ParallelBuilder(workspace);
  });

  it('should get batches', () => {
    const batches = builder.getBatches(['a', 'b']);
    expect(batches).to.be.an('array');
  });

  it('should build projects', async () => {
    const results = await builder.build(['a']);
    expect(results.length).to.equal(1);
    expect(results[0].success).to.be.true;
  });

  it('should clear cache', () => {
    builder.clearCache();
  });
});

describe('TaskExecutor', () => {
  let workspace;
  let executor;

  beforeEach(() => {
    workspace = new WorkspaceManager('./');
    workspace.addProject({ name: 'test', targets: { build: { command: 'echo test' } } });
    executor = new TaskExecutor(workspace);
  });

  it('should run target', async () => {
    const result = await executor.runTarget('test', 'build', { command: 'echo test' });
    expect(result.success).to.be.true;
  });

  it('should get status', () => {
    const status = executor.getStatus('test');
    expect(status).to.have.property('running');
    expect(status).to.have.property('completed');
  });

  it('should check if running', () => {
    expect(executor.isRunning('test')).to.be.false;
  });
});

describe('PackageManager', () => {
  let workspace;
  let pkgManager;

  beforeEach(() => {
    workspace = new WorkspaceManager('./');
    workspace.addProject({ name: 'test' });
    pkgManager = new PackageManager(workspace);
  });

  it('should load project package', () => {
    const pkg = pkgManager.loadProjectPackage('test');
    expect(pkg.name).to.equal('test');
  });

  it('should update dependency', () => {
    pkgManager.updateDependency('test', 'lodash', '^4.0.0', 'dependencies');
    const pkg = pkgManager.loadProjectPackage('test');
    expect(pkg.dependencies.lodash).to.equal('^4.0.0');
  });

  it('should resolve version', () => {
    const version = pkgManager.resolveVersion('lodash', { lodash: '^4.0.0' });
    expect(version).to.equal('^4.0.0');
  });

  it('should deduplicate dependencies', () => {
    const deps = { lodash: '^4.0.0', lodash: '^4.1.0' };
    const deduped = pkgManager.dedupe(deps);
    expect(Object.keys(deduped).length).to.equal(1);
  });

  it('should compare versions', () => {
    expect(pkgManager.compareVersions('1.0.0', '2.0.0')).to.be.lessThan(0);
    expect(pkgManager.compareVersions('2.0.0', '1.0.0')).to.be.greaterThan(0);
  });
});

describe('NxConfig', () => {
  let config;

  beforeEach(() => {
    config = new NxConfig();
  });

  it('should add plugins', () => {
    config.addPlugin({ name: 'test' });
    expect(config.plugins.length).to.equal(1);
  });

  it('should set named inputs', () => {
    config.setNamedInput('production', ['{projectRoot}/**/*.ts']);
    expect(config.namedInputs.production).to.exist;
  });

  it('should set target defaults', () => {
    config.setTargetDefaults('build', { cache: true });
    expect(config.targetDefaults.build.cache).to.be.true;
  });

  it('should convert to JSON', () => {
    const json = config.toJSON();
    expect(json).to.be.a('string');
  });
});

describe('Global Exports', () => {
  it('should export workspaceManager', () => {
    expect(workspaceManager).to.exist;
  });

  it('should export buildCache', () => {
    expect(buildCache).to.exist;
  });

  it('should export parallelBuilder', () => {
    expect(parallelBuilder).to.exist;
  });

  it('should export taskExecutor', () => {
    expect(taskExecutor).to.exist;
  });

  it('should export packageManager', () => {
    expect(packageManager).to.exist;
  });

  it('should export nxConfig', () => {
    expect(nxConfig).to.exist;
  });
});