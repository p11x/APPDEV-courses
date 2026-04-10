/**
 * @group unit
 * @group tooling
 */
import { expect, fixture, html } from '@open-wc/testing';
import './12_7_Dependency-Management.js';

describe('DependencyManagement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<dependency-management></dependency-management>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default config', () => {
    expect(element._config.autoUpdate).to.be.false;
    expect(element._config.exactVersions).to.be.false;
    expect(element._config.audit).to.be.true;
  });

  it('should observe auto-update attribute', () => {
    const observed = DependencyManagement.observedAttributes;
    expect(observed).to.include('auto-update');
  });

  describe('Property Changes', () => {
    it('should set auto update', () => {
      element._config.autoUpdate = true;
      expect(element._config.autoUpdate).to.be.true;
    });

    it('should set exact versions', () => {
      element._config.exactVersions = true;
      expect(element._config.exactVersions).to.be.true;
    });
  });

  describe('Events', () => {
    it('should have dependencies array', () => {
      expect(element._dependencies).to.be.an('array');
    });

    it('should have dev dependencies array', () => {
      expect(element._devDependencies).to.be.an('array');
    });

    it('should have audit results', () => {
      expect(element._auditResults).to.have.property('vulnerabilities');
    });
  });
});

describe('PackageResolver', () => {
  let resolver;

  beforeEach(() => {
    resolver = new PackageResolver();
  });

  it('should create resolver config', () => {
    const config = resolver.createConfig();
    expect(config).to.exist;
  });

  it('should add registry', () => {
    resolver.addRegistry('npm', 'https://registry.npmjs.org');
    expect(resolver.registries.get('npm')).to.equal('https://registry.npmjs.org');
  });
});

describe('VersionResolver', () => {
  let resolver;

  beforeEach(() => {
    resolver = new VersionResolver();
  });

  it('should resolve version range', () => {
    const version = resolver.resolve('^1.0.0', { '1.0.0': {} });
    expect(version).to.exist;
  });

  it('should handle semver', () => {
    expect(resolver.compare('1.0.0', '2.0.0')).to.be.lessThan(0);
  });
});