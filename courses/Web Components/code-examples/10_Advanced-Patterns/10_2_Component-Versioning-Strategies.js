/**
 * Component Versioning Strategies - Semantic versioning for Web Components with
 * changelog generation, migration guides, and backward compatibility management
 * @module advanced-patterns/10_2_Component-Versioning-Strategies
 * @version 1.0.0
 * @example <version-manager></version-manager>
 */

class VersionManager {
  constructor(initialVersion = '1.0.0') {
    this.version = this.parseVersion(initialVersion);
    this.history = [];
    this.deprecated = new Map();
    this.breakingChanges = [];
    this.featureFlags = new Map();
  }

  parseVersion(versionString) {
    const parts = versionString.split('.');
    return {
      major: parseInt(parts[0]) || 0,
      minor: parseInt(parts[1]) || 0,
      patch: parseInt(parts[2]) || 0,
      prerelease: '',
      build: '',
    };
  }

  formatVersion(version = this.version) {
    let formatted = `${version.major}.${version.minor}.${version.patch}`;
    if (version.prerelease) {
      formatted += `-${version.prerelease}`;
    }
    if (version.build) {
      formatted += `+${version.build}`;
    }
    return formatted;
  }

  bump(type, prerelease = '') {
    const oldVersion = { ...this.version };
    switch (type) {
      case 'major':
        this.version.major++;
        this.version.minor = 0;
        this.version.patch = 0;
        break;
      case 'minor':
        this.version.minor++;
        this.version.patch = 0;
        break;
      case 'patch':
        this.version.patch++;
        break;
      case 'prerelease':
        this.version.prerelease = prerelease || 'alpha';
        this.version.patch++;
        break;
    }

    this.history.push({
      from: this.formatVersion(oldVersion),
      to: this.formatVersion(),
      type,
      timestamp: Date.now(),
    });

    return this.formatVersion();
  }

  compare(a, b) {
    const v1 = typeof a === 'string' ? this.parseVersion(a) : a;
    const v2 = typeof b === 'string' ? this.parseVersion(b) : b;

    if (v1.major !== v2.major) return v1.major - v2.major;
    if (v1.minor !== v2.minor) return v1.minor - v2.minor;
    if (v1.patch !== v2.patch) return v1.patch - v2.patch;

    const aPre = v1.prerelease ? 0 : 1;
    const bPre = v2.prerelease ? 0 : 1;
    return bPre - aPre;
  }

  isCompatible(current, target) {
    const cmp = this.compare(current, target);
    return cmp >= 0;
  }

  addBreakingChange(change) {
    this.breakingChanges.push({
      ...change,
      version: this.formatVersion(),
      addedAt: Date.now(),
    });
  }

  deprecate(property, alternative, removalVersion) {
    this.deprecated.set(property, {
      alternative,
      removalVersion,
      deprecatedAt: Date.now(),
    });
  }

  getMigrationPath(fromVersion, toVersion) {
    const migrations = [];

    for (const change of this.breakingChanges) {
      const cmp = this.compare(fromVersion, change.version);
      if (cmp < 0 && this.compare(change.version, toVersion) <= 0) {
        migrations.push(change);
      }
    }

    return migrations;
  }

  enableFeature(name, version) {
    this.featureFlags.set(name, { enabled: true, since: version || this.formatVersion() });
  }

  disableFeature(name) {
    this.featureFlags.set(name, { enabled: false, removedAt: Date.now() });
  }

  isFeatureEnabled(name) {
    const flag = this.featureFlags.get(name);
    return flag ? flag.enabled : false;
  }

  getChangelog() {
    return this.history.map(entry => {
      const type = entry.type === 'major' ? '### Breaking' : `### ${entry.type.charAt(0).toUpperCase() + entry.type.slice(1)}`;
      return `${type}\n- ${entry.from} → ${entry.to}`;
    }).join('\n');
  }

  getDeprecations() {
    return Array.from(this.deprecated.entries()).map(([property, info]) => ({
      property,
      ...info,
    }));
  }

  getHistory() {
    return [...this.history];
  }
}

class VersionDetector {
  constructor() {
    this.cache = new Map();
    this.listeners = new Set();
  }

  detect(element) {
    const version = element.constructor.version;
    if (version) return version;

    const attr = element.getAttribute('data-version');
    if (attr) return attr;

    const moduleVersion = element.constructor.moduleVersion;
    if (moduleVersion) return moduleVersion;

    return '1.0.0';
  }

  detectAll(selector) {
    const elements = document.querySelectorAll(selector);
    return Array.from(elements).map(el => ({
      element: el,
      version: this.detect(el),
      tagName: el.tagName.toLowerCase(),
    }));
  }

  compareVersions(a, b) {
    const va = this.parseSemver(a);
    const vb = this.parseSemver(b);

    if (va.major !== vb.major) return va.major - vb.major;
    if (va.minor !== vb.minor) return va.minor - vb.minor;
    return va.patch - vb.patch;
  }

  parseSemver(version) {
    const match = version.match(/^(\d+)\.(\d+)\.(\d+)/);
    return {
      major: parseInt(match[1]) || 0,
      minor: parseInt(match[2]) || 0,
      patch: parseInt(match[3]) || 0,
    };
  }

  needsMigration(current, target) {
    const cmp = this.compareVersions(current, target);
    return cmp < 0;
  }

  onVersionChange(callback) {
    this.listeners.add(callback);
    return () => this.listeners.delete(callback);
  }

  notifyListeners(details) {
    for (const listener of this.listeners) {
      listener(details);
    }
  }

  cacheVersion(tagName, version) {
    this.cache.set(tagName, version);
  }

  getCachedVersion(tagName) {
    return this.cache.get(tagName);
  }
}

class VersionCompatibility {
  constructor() {
    this.adapters = new Map();
    this.fallbacks = new Map();
  }

  registerAdapter(fromVersion, toVersion, adapter) {
    const key = `${fromVersion}->${toVersion}`;
    this.adapters.set(key, adapter);
  }

  registerFallback(componentName, fallback) {
    this.fallbacks.set(componentName, fallback);
  }

  adapt(element, targetVersion) {
    const currentVersion = element.constructor.version || '1.0.0';
    const adapterKey = `${currentVersion}->${targetVersion}`;

    const adapter = this.adapters.get(adapterKey);
    if (adapter) {
      return adapter(element);
    }

    return element;
  }

  getFallback(componentName) {
    return this.fallbacks.get(componentName);
  }

  canAdapt(current, target) {
    const key = `${current}->${target}`;
    return this.adapters.has(key);
  }

  listAdapters() {
    return Array.from(this.adapters.keys());
  }
}

class ChangelogGenerator {
  constructor() {
    this.entries = [];
    this.categories = {
      added: 'Added',
      changed: 'Changed',
      deprecated: 'Deprecated',
      removed: 'Removed',
      fixed: 'Fixed',
      security: 'Security',
    };
  }

  addEntry(category, description, links = []) {
    this.entries.push({
      category,
      description,
      links,
      date: new Date().toISOString(),
    });
  }

  generate(options = {}) {
    const { version, date, compact = false } = options;
    const lines = [];

    if (version) {
      lines.push(`## ${version}${date ? ` (${date})` : ''}`);
      lines.push('');
    }

    const grouped = this.groupByCategory();

    for (const [cat, label] of Object.entries(this.categories)) {
      const items = grouped[cat] || [];
      if (items.length > 0) {
        lines.push(`### ${label}`);
        for (const item of items) {
          lines.push(`- ${item.description}`);
          for (const link of item.links) {
            lines.push(`  [${link.text}](${link.url})`);
          }
        }
        lines.push('');
      }
    }

    return lines.join('\n');
  }

  groupByCategory() {
    const grouped = {};
    for (const entry of this.entries) {
      if (!grouped[entry.category]) {
        grouped[entry.category] = [];
      }
      grouped[entry.category].push(entry);
    }
    return grouped;
  }

  toJSON() {
    return JSON.stringify(this.entries, null, 2);
  }

  fromJSON(json) {
    this.entries = JSON.parse(json);
  }
}

const globalVersionManager = new VersionManager();
const globalVersionDetector = new VersionDetector();
const globalCompatibility = new VersionCompatibility();
const globalChangelog = new ChangelogGenerator();

export { VersionManager, VersionDetector, VersionCompatibility, ChangelogGenerator };
export { globalVersionManager, globalVersionDetector, globalCompatibility, globalChangelog };

export default globalVersionManager;