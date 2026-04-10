/**
 * @group unit
 * @group tooling
 */
import { expect, fixture, html } from '@open-wc/testing';
import './12_2_Testing-Framework-Setup.js';

describe('TestingFrameworkSetup', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<testing-framework-setup></testing-framework-setup>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default framework', () => {
    expect(element._testFramework).to.equal('vitest');
  });

  it('should have default config', () => {
    expect(element._config.framework).to.equal('vitest');
    expect(element._config.watch).to.be.false;
    expect(element._config.coverage).to.be.true;
  });

  describe('Property Changes', () => {
    it('should set test framework', () => {
      element._testFramework = 'jest';
      expect(element._testFramework).to.equal('jest');
    });

    it('should set test files', () => {
      element._testFiles = ['test1.js', 'test2.js'];
      expect(element._testFiles.length).to.equal(2);
    });

    it('should set coverage', () => {
      element._config.coverage = false;
      expect(element._config.coverage).to.be.false;
    });
  });

  describe('Events', () => {
    it('should have test results array', () => {
      expect(element._testResults).to.be.an('array');
    });

    it('should have coverage object', () => {
      expect(element._coverage).to.have.property('statements');
      expect(element._coverage).to.have.property('branches');
    });
  });
});

describe('JestConfigurator', () => {
  let config;

  beforeEach(() => {
    config = new JestConfigurator();
  });

  it('should create config', () => {
    const jestConfig = config.createConfig();
    expect(jestConfig).to.exist;
  });

  it('should set test match', () => {
    config.setTestMatch(['**/*.test.js']);
    expect(config.testMatch).to.include('**/*.test.js');
  });

  it('should add transform', () => {
    config.addTransform({ name: 'ts-jest', extensions: ['.ts', '.js'] });
    expect(config.transforms.length).to.equal(1);
  });
});

describe('VitestConfigurator', () => {
  let config;

  beforeEach(() => {
    config = new VitestConfigurator();
  });

  it('should create config', () => {
    const vitestConfig = config.createConfig();
    expect(vitestConfig).to.exist;
  });

  it('should set test files', () => {
    config.setTestFiles(['**/*.test.js']);
    expect(config.testFiles).to.include('**/*.test.js');
  });
});