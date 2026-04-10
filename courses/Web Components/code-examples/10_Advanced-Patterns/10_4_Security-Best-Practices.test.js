/**
 * @group unit
 * @group advanced-patterns
 */
import { expect, fixture, html } from '@open-wc/testing';
import './10_4_Security-Best-Practices.js';

describe('SecurityValidator', () => {
  let validator;

  beforeEach(() => {
    validator = new SecurityValidator();
  });

  it('should validate URLs', () => {
    expect(validator.validate('https://example.com', 'url')).to.be.true;
    expect(validator.validate('javascript:alert(1)', 'url')).to.be.false;
  });

  it('should validate email', () => {
    expect(validator.validate('test@example.com', 'email')).to.be.true;
    expect(validator.validate('invalid', 'email')).to.be.false;
  });

  it('should validate alphanumeric', () => {
    expect(validator.validate('abc123', 'alphanumeric')).to.be.true;
    expect(validator.validate('abc-123', 'alphanumeric')).to.be.false;
  });

  it('should validate numeric', () => {
    expect(validator.validate('123', 'numeric')).to.be.true;
    expect(validator.validate('abc', 'numeric')).to.be.false;
  });

  it('should sanitize HTML entities', () => {
    const result = validator.sanitize('<script>alert(1)</script>');
    expect(result).to.not.include('<script>');
  });

  it('should sanitize dangerous attributes', () => {
    const result = validator.sanitize('<img onerror="alert(1)" src="x">');
    expect(result).to.not.include('onerror');
  });

  it('should sanitize HTML with allowed tags', () => {
    const result = validator.sanitizeHTML('<p>Safe</p><script>Dangerous</script>', ['p']);
    expect(result).to.include('<p>');
    expect(result).to.not.include('<script>');
  });

  it('should validate attributes', () => {
    expect(validator.validateAttr('href', 'https://example.com')).to.be.true;
    expect(validator.validateAttr('href', 'javascript:alert(1)')).to.be.false;
    expect(validator.validateAttr('onclick', 'func()')).to.be.false;
  });
});

describe('CSPManager', () => {
  let csp;

  beforeEach(() => {
    csp = new CSPManager();
  });

  it('should have default policies', () => {
    expect(csp.policies.default).to.exist;
    expect(csp.policies.script).to.exist;
  });

  it('should generate nonce', () => {
    const nonce = csp.getNonceAttribute();
    expect(nonce).to.include('nonce-');
  });

  it('should get policy by name', () => {
    const policy = csp.getPolicy('script');
    expect(policy).to.include("script-src");
  });

  it('should get full policy', () => {
    const fullPolicy = csp.getFullPolicy();
    expect(fullPolicy).to.include(';');
  });

  it('should set policy', () => {
    csp.setPolicy('script', "script-src 'self'");
    expect(csp.policies.script).to.equal("script-src 'self'");
  });

  it('should update nonce', () => {
    const oldNonce = csp.nonce;
    csp.setNonce();
    expect(csp.nonce).to.not.equal(oldNonce);
  });
});

describe('InputSanitizer', () => {
  let sanitizer;

  beforeEach(() => {
    sanitizer = new InputSanitizer();
  });

  it('should validate strings', () => {
    expect(sanitizer.validate('test', 'string')).to.be.true;
    expect(sanitizer.validate(123, 'string')).to.be.false;
  });

  it('should validate numbers', () => {
    expect(sanitizer.validate(42, 'number')).to.be.true;
    expect(sanitizer.validate(NaN, 'number')).to.be.false;
  });

  it('should validate booleans', () => {
    expect(sanitizer.validate(true, 'boolean')).to.be.true;
    expect(sanitizer.validate('true', 'boolean')).to.be.false;
  });

  it('should sanitize strings', () => {
    const result = sanitizer.sanitize('  test  ', 'string');
    expect(result).to.equal('test');
  });

  it('should sanitize numbers', () => {
    const result = sanitizer.sanitize('42.5', 'number');
    expect(result).to.equal(42.5);
  });

  it('should clean with validation', () => {
    const result = sanitizer.clean('test', 'string');
    expect(result).to.equal('test');
  });

  it('should return default on invalid with strict false', () => {
    const result = sanitizer.clean(123, 'string', { strict: false });
    expect(result).to.be.null;
  });

  it('should throw on invalid with strict true', () => {
    expect(() => sanitizer.clean(123, 'string', { strict: true })).to.throw();
  });
});

describe('SecureEventManager', () => {
  let manager;

  beforeEach(() => {
    manager = new SecureEventManager();
  });

  it('should add event types', () => {
    manager.addEventType('custom');
    expect(manager.eventTypes.has('custom')).to.be.true;
  });

  it('should set allowed origins', () => {
    manager.setAllowedOrigins(['https://example.com']);
    expect(manager.isOriginAllowed('https://example.com')).to.be.true;
  });

  it('should block origins', () => {
    manager.blockOrigin('https://evil.com');
    expect(manager.getBlockedOrigins()).to.include('https://evil.com');
  });

  it('should unblock origins', () => {
    manager.blockOrigin('https://evil.com');
    manager.unblockOrigin('https://evil.com');
    expect(manager.getBlockedOrigins()).to.not.include('https://evil.com');
  });

  it('should validate events', () => {
    const event = { origin: 'https://example.com' };
    expect(manager.validateEvent(event)).to.be.true;
  });
});

describe('PermissionManager', () => {
  let permissions;

  beforeEach(() => {
    permissions = new PermissionManager();
  });

  it('should grant permissions', () => {
    permissions.grant('my-component', 'observeAttributes');
    expect(permissions.check('my-component', 'observeAttributes')).to.be.true;
  });

  it('should revoke permissions', () => {
    permissions.grant('my-component', 'observeAttributes');
    permissions.revoke('my-component', 'observeAttributes');
    expect(permissions.check('my-component', 'observeAttributes')).to.be.false;
  });

  it('should check default permissions', () => {
    expect(permissions.check('unknown-component', 'observeAttributes')).to.be.true;
  });

  it('should revoke all permissions', () => {
    permissions.grant('my-component', 'observeAttributes');
    permissions.grant('my-component', 'modifyDOM');
    permissions.revokeAll('my-component');
    expect(permissions.check('my-component', 'observeAttributes')).to.be.false;
  });
});

describe('Global Exports', () => {
  it('should export securityValidator', () => {
    expect(securityValidator).to.exist;
  });

  it('should export cspManager', () => {
    expect(cspManager).to.exist;
  });

  it('should export inputSanitizer', () => {
    expect(inputSanitizer).to.exist;
  });

  it('should export secureEventManager', () => {
    expect(secureEventManager).to.exist;
  });

  it('should export permissionManager', () => {
    expect(permissionManager).to.exist;
  });
});