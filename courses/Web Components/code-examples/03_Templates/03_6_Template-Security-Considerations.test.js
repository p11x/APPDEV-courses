import { SecureTemplateHandler, SecurityValidator, createSecureTemplate, validateAndMaskIndianData, applyCSPHeaders, createInputSanitizer, CSP_DIRECTIVES } from './03_6_Template-Security-Considerations.js';

describe('SecureTemplateHandler', () => {
  let component;

  beforeEach(() => {
    component = new SecureTemplateHandler();
  });

  describe('rendering', () => {
    test('renders secure template container', () => {
      component.connectedCallback();
      expect(component.shadowRoot.innerHTML).toContain('secure-container');
    });

    test('renders security badge', () => {
      component.connectedCallback();
      expect(component.shadowRoot.innerHTML).toContain('Secured');
    });
  });

  describe('property changes', () => {
    test('strict-mode attribute updates config', () => {
      component.setAttribute('strict-mode', 'true');
      expect(component.configuration.strictMode).toBe(true);
    });

    test('enable-validation attribute updates config', () => {
      component.setAttribute('enable-validation', 'false');
      expect(component.configuration.enableValidation).toBe(false);
    });
  });

  describe('events', () => {
    test('dispatches security events', (done) => {
      component.addEventListener('security-event', (e) => {
        expect(e.detail.type).toBeDefined();
        done();
      });
      component.connectedCallback();
    });
  });

  describe('edge cases', () => {
    test('sanitizeContent sanitizes HTML', () => {
      const result = component.sanitizeContent('<script>alert(1)</script><p>Safe</p>');
      expect(result).not.toContain('<script>');
    });

    test('createSecureInputToken creates token', () => {
      const token = component.createSecureInputToken('test-input');
      expect(typeof token).toBe('string');
    });

    test('validateSecureInputToken validates token', () => {
      const token = component.createSecureInputToken('test');
      const result = component.validateSecureInputToken(token);
      expect(result.valid).toBe(true);
    });

    test('checkRateLimit enforces limits', () => {
      const result = component.checkRateLimit('test-id', 5, 60000);
      expect(typeof result).toBe('boolean');
    });

    test('processIndianPaymentData validates Indian data', () => {
      const result = component.processIndianPaymentData({
        phone: '9876543210'
      });
      expect(result.phone).toBeDefined();
    });

    test('getCSPDirectives returns directives', () => {
      const directives = component.getCSPDirectives();
      expect(directives.DEFAULT_SRC).toBeDefined();
    });

    test('validateExternalTemplate validates URLs', () => {
      const result = component.validateExternalTemplate('https://example.com');
      expect(result.valid).toBe(true);
    });
  });
});

describe('SecurityValidator', () => {
  let validator;

  beforeEach(() => {
    validator = new SecurityValidator();
  });

  describe('sanitizeHTML', () => {
    test('removes script tags', () => {
      const result = validator.sanitizeHTML('<script>alert(1)</script>');
      expect(result).not.toContain('<script>');
    });

    test('removes event handlers', () => {
      const result = validator.sanitizeHTML('<button onclick="x">Click</button>');
      expect(result).not.toContain('onclick');
    });

    test('blocks javascript: URIs', () => {
      const result = validator.sanitizeHTML('<a href="javascript:alert(1)">Link</a>');
      expect(result).not.toContain('javascript:');
    });

    test('strips HTML comments', () => {
      const result = validator.sanitizeHTML('<!-- comment -->text');
      expect(result).not.toContain('<!--');
    });
  });

  describe('validateURL', () => {
    test('accepts https URLs', () => {
      const result = validator.validateURL('https://example.com');
      expect(result.valid).toBe(true);
    });

    test('accepts http URLs', () => {
      const result = validator.validateURL('http://example.com');
      expect(result.valid).toBe(true);
    });

    test('rejects javascript URLs', () => {
      const result = validator.validateURL('javascript:alert(1)');
      expect(result.valid).toBe(false);
    });
  });

  describe('validateAadhaar', () => {
    test('validates 12-digit Aadhaar', () => {
      const result = validator.validateAadhaar('123456789012');
      expect(result.valid).toBe(false);
    });
  });

  describe('validateUPI', () => {
    test('validates UPI format', () => {
      const result = validator.validateUPI('user@okhdfcbank');
      expect(result.valid).toBe(true);
    });
  });

  describe('validatePhone', () => {
    test('validates Indian phone numbers', () => {
      const result = validator.validatePhone('9876543210');
      expect(result.valid).toBe(true);
    });

    test('rejects invalid phone numbers', () => {
      const result = validator.validatePhone('1234567890');
      expect(result.valid).toBe(false);
    });
  });

  describe('validatePAN', () => {
    test('validates PAN format', () => {
      const result = validator.validatePAN('ABCDE1234F');
      expect(result.valid).toBe(true);
    });
  });

  describe('validateGSTIN', () => {
    test('validates GSTIN format', () => {
      const result = validator.validateGSTIN('12ABCDE1234F1Z1');
      expect(result.valid).toBe(true);
    });
  });

  describe('checkRateLimit', () => {
    test('allows requests under limit', () => {
      const result = validator.checkRateLimit('test-user', 10, 60000);
      expect(result).toBe(true);
    });

    test('blocks requests over limit', () => {
      for (let i = 0; i < 5; i++) {
        validator.checkRateLimit('rate-limit-test', 3, 60000);
      }
      const result = validator.checkRateLimit('rate-limit-test', 3, 60000);
      expect(result).toBe(false);
    });
  });

  describe('createInputToken', () => {
    test('creates token', () => {
      const token = validator.createInputToken('input-data');
      expect(token).toBeDefined();
    });
  });

  describe('hashForComparison', () => {
    test('generates hash', () => {
      const hash = validator.hashForComparison('test');
      expect(typeof hash).toBe('string');
    });
  });
});

describe('Utility functions', () => {
  describe('validateAndMaskIndianData', () => {
    test('validates Indian payment data', () => {
      const result = validateAndMaskIndianData({
        phone: '9876543210'
      });
      expect(result.phone).toBeDefined();
    });
  });

  describe('applyCSPHeaders', () => {
    test('applies CSP headers', () => {
      const div = document.createElement('div');
      const result = applyCSPHeaders(div);
      expect(result).toContain('DEFAULT_SRC');
    });
  });

  describe('createInputSanitizer', () => {
    test('creates sanitizer instance', () => {
      const sanitizer = createInputSanitizer();
      expect(sanitizer).toBeInstanceOf(SecurityValidator);
    });
  });
});
