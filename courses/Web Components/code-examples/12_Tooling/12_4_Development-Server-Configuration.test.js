/**
 * @group unit
 * @group tooling
 */
import { expect, fixture, html } from '@open-wc/testing';
import './12_4_Development-Server-Configuration.js';

describe('DevelopmentServerConfiguration', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<development-server-configuration></development-server-configuration>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default port', () => {
    expect(element._serverConfig.port).to.equal(3000);
  });

  it('should have default host', () => {
    expect(element._serverConfig.host).to.equal('localhost');
  });

  it('should observe port attribute', () => {
    const observed = DevelopmentServerConfiguration.observedAttributes;
    expect(observed).to.include('port');
  });

  describe('Property Changes', () => {
    it('should set port', () => {
      element._serverConfig.port = 8080;
      expect(element._serverConfig.port).to.equal(8080);
    });

    it('should set host', () => {
      element._serverConfig.host = '0.0.0.0';
      expect(element._serverConfig.host).to.equal('0.0.0.0');
    });

    it('should set https', () => {
      element._serverConfig.https = true;
      expect(element._serverConfig.https).to.be.true;
    });
  });

  describe('Events', () => {
    it('should track is running', () => {
      expect(element._isRunning).to.be.false;
    });

    it('should have request log', () => {
      expect(element._requestLog).to.be.an('array');
    });
  });
});

describe('ViteDevServer', () => {
  let server;

  beforeEach(() => {
    server = new ViteDevServer();
  });

  it('should create server config', () => {
    const config = server.createConfig();
    expect(config).to.exist;
  });

  it('should set port', () => {
    server.setPort(3001);
    expect(server.port).to.equal(3001);
  });

  it('should add proxy', () => {
    server.addProxy('/api', 'http://localhost:8080');
    expect(server.proxies.length).to.equal(1);
  });
});

describe('WebpackDevServer', () => {
  let server;

  beforeEach(() => {
    server = new WebpackDevServer();
  });

  it('should create server config', () => {
    const config = server.createConfig();
    expect(config).to.exist;
  });
});