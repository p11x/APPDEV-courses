/**
 * @group unit
 * @group advanced-patterns
 */
import { expect, fixture, html } from '@open-wc/testing';
import './10_2_Component-Versioning-Strategies.js';

describe('VersionManager', () => {
  let manager;

  beforeEach(() => {
    manager = new VersionManager('1.0.0');
  });

  it('should parse version string', () => {
    const parsed = manager.parseVersion('2.3.4');
    expect(parsed.major).to.equal(2);
    expect(parsed.minor).to.equal(3);
    expect(parsed.patch).to.equal(4);
  });

  it('should format version', () => {
    expect(manager.formatVersion()).to.equal('1.0.0');
  });

  it('should bump major version', () => {
    const version = manager.bump('major');
    expect(version).to.equal('2.0.0');
    expect(manager.version.major).to.equal(2);
  });

  it('should bump minor version', () => {
    manager.bump('minor');
    expect(manager.formatVersion()).to.equal('1.1.0');
  });

  it('should bump patch version', () => {
    manager.bump('patch');
    expect(manager.formatVersion()).to.equal('1.0.1');
  });

  it('should compare versions', () => {
    expect(manager.compare('1.0.0', '2.0.0')).to.be.lessThan(0);
    expect(manager.compare('2.0.0', '1.0.0')).to.be.greaterThan(0);
    expect(manager.compare('1.0.0', '1.0.0')).to.equal(0);
  });

  it('should check compatibility', () => {
    expect(manager.isCompatible('1.0.0', '1.0.0')).to.be.true;
    expect(manager.isCompatible('1.0.0', '2.0.0')).to.be.false;
  });

  it('should add breaking changes', () => {
    manager.addBreakingChange({ description: 'Removed API' });
    expect(manager.breakingChanges.length).to.equal(1);
  });

  it('should deprecate properties', () => {
    manager.deprecate('oldProp', 'newProp', '3.0.0');
    const deprecation = manager.deprecated.get('oldProp');
    expect(deprecation.alternative).to.equal('newProp');
  });

  it('should get migration path', () => {
    manager.addBreakingChange({ description: 'Change 1' });
    manager.bump('major');
    manager.addBreakingChange({ description: 'Change 2' });
    const migrations = manager.getMigrationPath('1.0.0', '3.0.0');
    expect(migrations.length).to.be.greaterThan(0);
  });

  it('should enable feature flags', () => {
    manager.enableFeature('newFeature', '2.0.0');
    expect(manager.isFeatureEnabled('newFeature')).to.be.true;
  });

  it('should disable feature flags', () => {
    manager.enableFeature('newFeature');
    manager.disableFeature('newFeature');
    expect(manager.isFeatureEnabled('newFeature')).to.be.false;
  });

  it('should generate changelog', () => {
    manager.bump('minor');
    manager.bump('patch');
    const changelog = manager.getChangelog();
    expect(changelog).to.include('Minor');
    expect(changelog).to.include('Patch');
  });

  it('should get deprecations', () => {
    manager.deprecate('prop', 'alternative', '2.0.0');
    const deprecations = manager.getDeprecations();
    expect(deprecations.length).to.equal(1);
    expect(deprecations[0].property).to.equal('prop');
  });
});

describe('VersionDetector', () => {
  let detector;

  beforeEach(() => {
    detector = new VersionDetector();
  });

  it('should detect version from constructor', () => {
    const element = { constructor: { version: '2.0.0' } };
    expect(detector.detect(element)).to.equal('2.0.0');
  });

  it('should detect version from attribute', () => {
    const element = { getAttribute: () => '3.0.0', constructor: {} };
    expect(detector.detect(element)).to.equal('3.0.0');
  });

  it('should return default version', () => {
    const element = { getAttribute: () => null, constructor: {} };
    expect(detector.detect(element)).to.equal('1.0.0');
  });

  it('should compare versions', () => {
    expect(detector.compareVersions('1.0.0', '2.0.0')).to.be.lessThan(0);
    expect(detector.compareVersions('2.0.0', '1.0.0')).to.be.greaterThan(0);
  });

  it('should detect if migration needed', () => {
    expect(detector.needsMigration('1.0.0', '2.0.0')).to.be.true;
    expect(detector.needsMigration('2.0.0', '1.0.0')).to.be.false;
  });

  it('should notify listeners', () => {
    let notified = false;
    detector.onVersionChange(() => { notified = true; });
    detector.notifyListeners({});
    expect(notified).to.be.true;
  });

  it('should cache versions', () => {
    detector.cacheVersion('my-component', '2.0.0');
    expect(detector.getCachedVersion('my-component')).to.equal('2.0.0');
  });
});

describe('VersionCompatibility', () => {
  let compatibility;

  beforeEach(() => {
    compatibility = new VersionCompatibility();
  });

  it('should register adapters', () => {
    compatibility.registerAdapter('1.0.0', '2.0.0', (el) => el);
    expect(compatibility.canAdapt('1.0.0', '2.0.0')).to.be.true;
  });

  it('should register fallbacks', () => {
    compatibility.registerFallback('my-component', {});
    expect(compatibility.getFallback('my-component')).to.exist;
  });

  it('should adapt element', () => {
    const adapter = (el) => { el.adapted = true; return el; };
    compatibility.registerAdapter('1.0.0', '2.0.0', adapter);
    const element = { constructor: { version: '1.0.0' } };
    const adapted = compatibility.adapt(element, '2.0.0');
    expect(adapted.adapted).to.be.true;
  });

  it('should list adapters', () => {
    compatibility.registerAdapter('1.0.0', '2.0.0', () => {});
    const adapters = compatibility.listAdapters();
    expect(adapters).to.include('1.0.0->2.0.0');
  });
});

describe('ChangelogGenerator', () => {
  let generator;

  beforeEach(() => {
    generator = new ChangelogGenerator();
  });

  it('should add entries', () => {
    generator.addEntry('added', 'New feature added');
    expect(generator.entries.length).to.equal(1);
  });

  it('should generate changelog', () => {
    generator.addEntry('added', 'Feature A');
    generator.addEntry('fixed', 'Bug B');
    const changelog = generator.generate({ version: '1.0.0' });
    expect(changelog).to.include('1.0.0');
    expect(changelog).to.include('Added');
    expect(changelog).to.include('Fixed');
  });

  it('should group by category', () => {
    generator.addEntry('added', 'Feature 1');
    generator.addEntry('added', 'Feature 2');
    generator.addEntry('fixed', 'Bug 1');
    const grouped = generator.groupByCategory();
    expect(grouped.added.length).to.equal(2);
    expect(grouped.fixed.length).to.equal(1);
  });

  it('should serialize to JSON', () => {
    generator.addEntry('added', 'Test');
    const json = generator.toJSON();
    expect(json).to.include('Test');
  });

  it('should deserialize from JSON', () => {
    generator.fromJSON('[{"category":"added","description":"Test"}]');
    expect(generator.entries.length).to.equal(1);
  });
});

describe('Global Exports', () => {
  it('should export globalVersionManager', () => {
    expect(globalVersionManager).to.exist;
  });

  it('should export globalVersionDetector', () => {
    expect(globalVersionDetector).to.exist;
  });

  it('should export globalCompatibility', () => {
    expect(globalCompatibility).to.exist;
  });

  it('should export globalChangelog', () => {
    expect(globalChangelog).to.exist;
  });
});