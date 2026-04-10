/**
 * @group unit
 * @group tooling
 */
import { expect, fixture, html } from '@open-wc/testing';
import './12_1_Build-Tool-Integration.js';

describe('BuildToolIntegration', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<build-tool-integration></build-tool-integration>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default config', () => {
    expect(element._config.buildSystem).to.equal('webpack');
    expect(element._config.watch).to.be.false;
    expect(element._config.minify).to.be.true;
  });

  it('should observe build-system attribute', () => {
    const observed = BuildToolIntegration.observedAttributes;
    expect(observed).to.include('build-system');
  });

  describe('Property Changes', () => {
    it('should set build system', () => {
      element._config.buildSystem = 'vite';
      expect(element._config.buildSystem).to.equal('vite');
    });

    it('should set watch mode', () => {
      element._config.watch = true;
      expect(element._config.watch).to.be.true;
    });

    it('should set minify', () => {
      element._config.minify = false;
      expect(element._config.minify).to.be.false;
    });

    it('should set target', () => {
      element._config.target = 'es2021';
      expect(element._config.target).to.equal('es2021');
    });
  });

  describe('Events', () => {
    it('should have compilation results array', () => {
      expect(element._compilationResults).to.be.an('array');
    });
  });
});

describe('WebpackConfigBuilder', () => {
  let builder;

  beforeEach(() => {
    builder = new WebpackConfigBuilder();
  });

  it('should create config', () => {
    const config = builder.createConfig();
    expect(config).to.exist;
  });

  it('should set entry', () => {
    builder.setEntry('./src/index.js');
    expect(builder.entry).to.equal('./src/index.js');
  });

  it('should set output', () => {
    builder.setOutput({ path: './dist', filename: 'bundle.js' });
    expect(builder.output.path).to.equal('./dist');
  });

  it('should add plugin', () => {
    builder.addPlugin({ name: 'TestPlugin' });
    expect(builder.plugins.length).to.equal(1);
  });
});

describe('ViteConfigBuilder', () => {
  let builder;

  beforeEach(() => {
    builder = new ViteConfigBuilder();
  });

  it('should create config', () => {
    const config = builder.createConfig();
    expect(config).to.exist;
  });

  it('should set root', () => {
    builder.setRoot('./src');
    expect(builder.root).to.equal('./src');
  });
});