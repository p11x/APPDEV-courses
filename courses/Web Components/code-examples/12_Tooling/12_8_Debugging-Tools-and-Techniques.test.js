/**
 * @group unit
 * @group tooling
 */
import { expect, fixture, html } from '@open-wc/testing';
import './12_8_Debugging-Tools-and-Techniques.js';

describe('DebuggingToolsAndTechniques', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<debugging-tools-and-techniques></debugging-tools-and-techniques>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default config', () => {
    expect(element._config.logLevel).to.equal('info');
    expect(element._config.console).to.be.true;
    expect(element._config.network).to.be.true;
  });

  it('should observe log-level attribute', () => {
    const observed = DebuggingToolsAndTechniques.observedAttributes;
    expect(observed).to.include('log-level');
  });

  describe('Property Changes', () => {
    it('should set log level', () => {
      element._config.logLevel = 'debug';
      expect(element._config.logLevel).to.equal('debug');
    });

    it('should set console enabled', () => {
      element._config.console = false;
      expect(element._config.console).to.be.false;
    });
  });

  describe('Events', () => {
    it('should have breakpoints array', () => {
      expect(element._breakpoints).to.be.an('array');
    });

    it('should have call stack array', () => {
      expect(element._callStack).to.be.an('array');
    });

    it('should have logs array', () => {
      expect(element._logs).to.be.an('array');
    });
  });
});

describe('Logger', () => {
  let logger;

  beforeEach(() => {
    logger = new Logger();
  });

  it('should create logger config', () => {
    const config = logger.createConfig();
    expect(config).to.exist;
  });

  it('should set log level', () => {
    logger.setLevel('warn');
    expect(logger.level).to.equal('warn');
  });

  it('should add transport', () => {
    logger.addTransport({ log: () => {} });
    expect(logger.transports.length).to.equal(1);
  });
});

describe('Debugger', () => {
  let debugger_;

  beforeEach(() => {
    debugger_ = new Debugger();
  });

  it('should create debugger config', () => {
    const config = debugger_.createConfig();
    expect(config).to.exist;
  });

  it('should add breakpoint', () => {
    debugger_.addBreakpoint('test.js', 10);
    expect(debugger_.breakpoints.length).to.equal(1);
  });
});