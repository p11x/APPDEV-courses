/**
 * @group unit
 * @group real-world
 */
import { expect, fixture, html } from '@open-wc/testing';
import './11_3_Form-Validation-Framework.js';

describe('ValidationRule', () => {
  it('should create validation rule', () => {
    const rule = new ValidationRule('test', () => true, 'Test message');
    expect(rule.name).to.equal('test');
    expect(rule.message).to.equal('Test message');
  });

  it('should validate value', () => {
    const rule = new ValidationRule('test', (val) => val === 'valid', 'Invalid');
    expect(rule.validate('valid')).to.be.true;
    expect(rule.validate('invalid')).to.be.false;
  });
});

describe('FormValidator', () => {
  let validator;

  beforeEach(() => {
    validator = new FormValidator();
  });

  it('should have built-in rules', () => {
    expect(FormValidator.builtInRules).to.exist;
    expect(FormValidator.builtInRules.required).to.exist;
    expect(FormValidator.builtInRules.email).to.exist;
  });

  it('should add rules', () => {
    validator.addRule('username', [FormValidator.builtInRules.required]);
    expect(validator.rules.username).to.exist;
  });

  it('should validate field', () => {
    validator.addRule('email', [FormValidator.builtInRules.required, FormValidator.builtInRules.email]);
    const result = validator.validate('email', 'test@example.com');
    expect(result.valid).to.be.true;
  });

  it('should add errors', () => {
    validator.addError('field', 'Error message');
    expect(validator.errors.field).to.equal('Error message');
  });

  it('should get errors', () => {
    validator.addError('field', 'Error');
    const errors = validator.getErrors('field');
    expect(errors).to.include('Error');
  });

  it('should clear errors', () => {
    validator.addError('field', 'Error');
    validator.clearErrors();
    expect(Object.keys(validator.errors).length).to.equal(0);
  });

  it('should add async validators', () => {
    validator.addAsyncValidator(async () => true);
    expect(validator.asyncValidators.length).to.equal(1);
  });
});