/**
 * @group unit
 * @group tooling
 */
import { expect, fixture, html } from '@open-wc/testing';
import './12_5_Deployment-Strategies.js';

describe('DeploymentStrategies', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<deployment-strategies></deployment-strategies>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default target', () => {
    expect(element._deploymentTarget).to.equal('cdn');
  });

  it('should have default config', () => {
    expect(element._config.target).to.equal('cdn');
    expect(element._config.environment).to.equal('production');
    expect(element._config.optimize).to.be.true;
  });

  it('should observe target attribute', () => {
    const observed = DeploymentStrategies.observedAttributes;
    expect(observed).to.include('target');
  });

  describe('Property Changes', () => {
    it('should set target', () => {
      element._deploymentTarget = 'npm';
      expect(element._deploymentTarget).to.equal('npm');
    });

    it('should set environment', () => {
      element._config.environment = 'staging';
      expect(element._config.environment).to.equal('staging');
    });

    it('should set cdn provider', () => {
      element._config.cdnProvider = 'jsdelivr';
      expect(element._config.cdnProvider).to.equal('jsdelivr');
    });
  });

  describe('Events', () => {
    it('should track is deploying', () => {
      expect(element._isDeploying).to.be.false;
    });

    it('should have deployment log', () => {
      expect(element._deploymentLog).to.be.an('array');
    });

    it('should have build artifacts', () => {
      expect(element._buildArtifacts).to.be.an('array');
    });
  });
});

describe('StaticHosting', () => {
  let hosting;

  beforeEach(() => {
    hosting = new StaticHosting();
  });

  it('should create deployment config', () => {
    const config = hosting.createConfig();
    expect(config).to.exist;
  });

  it('should set provider', () => {
    hosting.setProvider('netlify');
    expect(hosting.provider).to.equal('netlify');
  });
});

describe('CDNPublisher', () => {
  let publisher;

  beforeEach(() => {
    publisher = new CDNPublisher();
  });

  it('should create publisher config', () => {
    const config = publisher.createConfig();
    expect(config).to.exist;
  });

  it('should set cdn url', () => {
    publisher.setCDNUrl('https://cdn.example.com');
    expect(publisher.cdnUrl).to.equal('https://cdn.example.com');
  });
});