/**
 * @group unit
 * @group tooling
 */
import { expect, fixture, html } from '@open-wc/testing';
import './12_3_Linting-and-Formatting-Guide.js';

describe('LintingAndFormattingGuide', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<linting-and-formatting-guide></linting-and-formatting-guide>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default linter', () => {
    expect(element._config.linter).to.equal('eslint');
  });

  it('should have default formatter', () => {
    expect(element._config.formatter).to.equal('prettier');
  });

  it('should have default max line length', () => {
    expect(element._config.maxLineLength).to.equal(80);
  });

  describe('Property Changes', () => {
    it('should set linter', () => {
      element._config.linter = 'tslint';
      expect(element._config.linter).to.equal('tslint');
    });

    it('should set formatter', () => {
      element._config.formatter = 'biome';
      expect(element._config.formatter).to.equal('biome');
    });

    it('should set auto fix', () => {
      element._config.autoFix = false;
      expect(element._config.autoFix).to.be.false;
    });
  });

  describe('Events', () => {
    it('should have lint results array', () => {
      expect(element._lintResults).to.be.an('array');
    });

    it('should have format results array', () => {
      expect(element._formatResults).to.be.an('array');
    });
  });
});

describe('ESLintConfigurator', () => {
  let config;

  beforeEach(() => {
    config = new ESLintConfigurator();
  });

  it('should create config', () => {
    const eslintConfig = config.createConfig();
    expect(eslintConfig).to.exist;
  });

  it('should set parser options', () => {
    config.setParserOptions({ ecmaVersion: '2021' });
    expect(config.parserOptions.ecmaVersion).to.equal('2021');
  });

  it('should add rule', () => {
    config.addRule('no-debugger', 'warn');
    expect(config.rules['no-debugger']).to.equal('warn');
  });
});

describe('PrettierConfigurator', () => {
  let config;

  beforeEach(() => {
    config = new PrettierConfigurator();
  });

  it('should create config', () => {
    const prettierConfig = config.createConfig();
    expect(prettierConfig).to.exist;
  });

  it('should set print width', () => {
    config.setPrintWidth(100);
    expect(config.printWidth).to.equal(100);
  });
});