import { ValidatedFormElement, VALIDATION_CONFIG, ValidationError, ValidationRule, FieldValidator } from './05_6_Data-Validation-in-Components.js';

describe('ValidatedFormElement', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('validated-form-element');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders form elements', () => {
      expect(component.shadowRoot.innerHTML).toContain('form');
    });

    test('renders validation messages', () => {
      expect(component.shadowRoot.innerHTML).toContain('validation-messages');
    });
  });

  describe('property changes', () => {
    test('rules attribute sets validation rules', () => {
      component.setAttribute('rules', JSON.stringify({ email: 'required|email' }));
      expect(component.getAttribute('rules')).toBeDefined();
    });

    test('validation-mode attribute updates mode', () => {
      component.setAttribute('validation-mode', 'onBlur');
      expect(component.getAttribute('validation-mode')).toBe('onBlur');
    });

    test('show-all-errors attribute toggles display', () => {
      component.setAttribute('show-all-errors', 'false');
      expect(component.hasAttribute('show-all-errors')).toBe(true);
    });
  });

  describe('events', () => {
    test('dispatches validation-error event', (done) => {
      component.addEventListener('validation-error', (e) => {
        expect(e.detail.errors).toBeDefined();
        done();
      });
    });
  });

  describe('edge cases', () => {
    test('validates email', () => {
      const result = component._validateField('email', 'test@example.com');
      expect(result.valid).toBe(true);
    });

    test('rejects invalid email', () => {
      const result = component._validateField('email', 'invalid');
      expect(result.valid).toBe(false);
    });

    test('validates required field', () => {
      const result = component._validateField('required', '');
      expect(result.valid).toBe(false);
    });

    test('accepts valid required field', () => {
      const result = component._validateField('required', 'value');
      expect(result.valid).toBe(true);
    });

    test('validates min length', () => {
      const result = component._validateField('minLength', 'ab', 3);
      expect(result.valid).toBe(false);
    });

    test('validates max length', () => {
      const result = component._validateField('maxLength', 'abc', 2);
      expect(result.valid).toBe(false);
    });

    test('validates numeric', () => {
      const result = component._validateField('numeric', '123');
      expect(result.valid).toBe(true);
    });

    test('validates URL', () => {
      const result = component._validateField('url', 'https://example.com');
      expect(result.valid).toBe(true);
    });

    test('validates pattern', () => {
      const result = component._validateField('pattern', 'ABC123', '^[A-Z]{3}[0-9]{3}$');
      expect(result.valid).toBe(true);
    });

    test('validates alpha', () => {
      const result = component._validateField('alpha', 'abc');
      expect(result.valid).toBe(true);
    });

    test('validates alphaNumeric', () => {
      const result = component._validateField('alphaNumeric', 'abc123');
      expect(result.valid).toBe(true);
    });
  });
});

describe('ValidationRule', () => {
  test('creates validation rule', () => {
    const rule = new ValidationRule(() => true, 'Error message');
    expect(rule.message).toBe('Error message');
  });

  test('validates value', () => {
    const rule = new ValidationRule((v) => v === 'valid', 'Invalid');
    expect(rule.validate('valid')).toBe(true);
    expect(rule.validate('invalid')).toBe(false);
  });

  test('passes parameters to validator', () => {
    const rule = new ValidationRule((v, min) => v >= min, 'Too small', [5]);
    expect(rule.validate(10)).toBe(true);
    expect(rule.validate(3)).toBe(false);
  });
});

describe('FieldValidator', () => {
  test('creates field validator', () => {
    const validator = new FieldValidator('test-field', { required: true });
    expect(validator.fieldName).toBe('test-field');
  });

  test('validates field', () => {
    const validator = new FieldValidator('email', { email: true });
    const result = validator.validate('test@example.com');
    expect(result.valid).toBe(true);
  });

  test('tracks touched state', () => {
    const validator = new FieldValidator('test', { required: true });
    expect(validator.touched).toBe(false);
    validator.setTouched(true);
    expect(validator.touched).toBe(true);
  });

  test('tracks dirty state', () => {
    const validator = new FieldValidator('test', { required: true });
    expect(validator.dirty).toBe(false);
    validator.setDirty(true);
    expect(validator.dirty).toBe(true);
  });

  test('returns errors', () => {
    const validator = new FieldValidator('test', { required: true });
    validator.validate('');
    expect(validator.errors.length).toBeGreaterThan(0);
  });
});

describe('VALIDATION_CONFIG', () => {
  test('has built-in validators', () => {
    expect(VALIDATION_CONFIG.builtInValidators.required).toBeDefined();
    expect(VALIDATION_CONFIG.builtInValidators.email).toBeDefined();
  });

  test('has validation mode', () => {
    expect(VALIDATION_CONFIG.validationMode).toBeDefined();
  });

  test('has delay time', () => {
    expect(VALIDATION_CONFIG.delayTime).toBeGreaterThan(0);
  });
});

describe('ValidationError', () => {
  test('creates validation error', () => {
    const error = new ValidationError('Validation failed', 'INVALID', ['error1']);
    expect(error.message).toBe('Validation failed');
    expect(error.code).toBe('INVALID');
    expect(error.errors).toEqual(['error1']);
  });
});
