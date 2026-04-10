# Advanced Form Patterns: Comprehensive Guide

**Table of Contents**
1. [Introduction](#introduction)
2. [Dynamic Forms](#dynamic-forms)
3. [Form Wizard Patterns](#form-wizard-patterns)
4. [Conditional Logic](#conditional-logic)
5. [Dependent Fields](#dependent-fields)
6. [Code Examples](#code-examples)
7. [Performance Optimization](#performance-optimization)
8. [Accessibility in Dynamic Forms](#accessibility-in-dynamic-forms)
9. [Key Takeaways](#key-takeaways)
10. [Common Pitfalls](#common-pitfalls)

---

## Introduction

Advanced form patterns address complex requirements that go beyond basic form implementation. These include dynamic forms that change based on user input, multi-step wizards for lengthy registrations, conditional fields that appear or disappear based on selections, and dependent fields where one field's value affects another's options.

Modern web applications frequently require these patterns to create efficient, user-friendly interfaces that adapt to user needs while collecting only necessary information. Understanding these patterns is essential for building sophisticated forms that provide excellent user experiences.

This guide covers implementation patterns for dynamic forms, wizards, conditional logic, and field dependencies, with practical code examples for production applications.

---

## Dynamic Forms

Dynamic forms change their structure based on user interactions, adding, removing, or modifying fields as needed. This pattern is essential for complex forms where the required information isn't known in advance.

### Adding Fields Dynamically

```javascript
class DynamicFormManager {
  constructor(containerId, options = {}) {
    this.container = document.getElementById(containerId);
    this.options = {
      maxFields: 10,
      minFields: 1,
      fieldTemplate: '',
      onAdd: () => {},
      onRemove: () => {},
      ...options
    };
    
    this.fieldCount = 0;
  }
  
  addField(data = {}) {
    if (this.fieldCount >= this.options.maxFields) {
      console.warn('Maximum fields reached');
      return null;
    }
    
    this.fieldCount++;
    
    const fieldWrapper = document.createElement('div');
    fieldWrapper.className = 'dynamic-field';
    fieldWrapper.dataset.fieldId = this.fieldCount;
    
    const template = this.options.fieldTemplate
      .replace(/{{index}}/g, this.fieldCount)
      .replace(/{{value}}/g, data.value || '')
      .replace(/{{label}}/g, data.label || `Field ${this.fieldCount}`);
    
    fieldWrapper.innerHTML = template + this.createRemoveButton();
    
    this.container.appendChild(fieldWrapper);
    this.options.onAdd(fieldWrapper);
    
    return fieldWrapper;
  }
  
  removeField(fieldWrapper) {
    if (this.container.children.length <= this.options.minFields) {
      console.warn('Minimum fields required');
      return;
    }
    
    fieldWrapper.remove();
    this.fieldCount--;
    this.options.onRemove(fieldWrapper);
  }
  
  createRemoveButton() {
    return `
      <button type="button" class="remove-field-btn" aria-label="Remove field">
        Remove
      </button>
    `;
  }
}
```

### Reordering Fields

```javascript
class SortableFormFields {
  constructor(containerId) {
    this.container = document.getElementById(containerId);
    this.init();
  }
  
  init() {
    let draggedItem = null;
    
    this.container.addEventListener('dragstart', (e) => {
      if (e.target.classList.contains('sortable-field')) {
        draggedItem = e.target;
        e.target.classList.add('dragging');
      }
    });
    
    this.container.addEventListener('dragend', (e) => {
      if (e.target.classList.contains('sortable-field')) {
        e.target.classList.remove('dragging');
        draggedItem = null;
      }
    });
    
    this.container.addEventListener('dragover', (e) => {
      e.preventDefault();
      const afterElement = this.getDragAfterElement(e.clientY);
      if (afterElement) {
        this.container.insertBefore(draggedItem, afterElement);
      } else {
        this.container.appendChild(draggedItem);
      }
    });
  }
  
  getDragAfterElement(y) {
    const draggableElements = [...this.container.querySelectorAll('.sortable-field:not(.dragging)')];
    
    return draggableElements.reduce((closest, child) => {
      const rect = child.getBoundingClientRect();
      const offset = y - rect.top - rect.height / 2;
      
      if (offset < 0 && offset > closest.offset) {
        return { offset, element: child };
      } else {
        return closest;
      }
    }, { offset: Number.NEGATIVE_INFINITY }).element;
  }
}
```

### Field Templates

```javascript
const fieldTemplates = {
  text: `
    <div class="form-field">
      <label for="field-{{index}}">{{label}}</label>
      <input type="text" id="field-{{index}}" name="field-{{index}}" value="{{value}}">
    </div>
  `,
  
  email: `
    <div class="form-field">
      <label for="field-{{index}}">{{label}}</label>
      <input type="email" id="field-{{index}}" name="field-{{index}}" value="{{value}}">
    </div>
  `,
  
  select: `
    <div class="form-field">
      <label for="field-{{index}}">{{label}}</label>
      <select id="field-{{index}}" name="field-{{index}}">
        <option value="">Select...</option>
        <option value="a">Option A</option>
        <option value="b">Option B</option>
      </select>
    </div>
  `,
  
  textarea: `
    <div class="form-field">
      <label for="field-{{index}}">{{label}}</label>
      <textarea id="field-{{index}}" name="field-{{index}}">{{value}}</textarea>
    </div>
  `
};

function createField(type, index, data = {}) {
  const template = fieldTemplates[type] || fieldTemplates.text;
  return template
    .replace(/\{\{index\}\}/g, index)
    .replace(/\{\{label\}\}/g, data.label || '')
    .replace(/\{\{value\}\}/g, data.value || '');
}
```

---

## Form Wizard Patterns

Form wizards break complex forms into manageable steps, improving user experience for lengthy forms by showing only relevant information at each stage.

### Step Navigation

```javascript
class FormWizard {
  constructor(formId, options = {}) {
    this.form = document.getElementById(formId);
    this.options = {
      initialStep: 0,
      validateOnNext: true,
      saveStateOnNavigate: true,
      animateTransitions: true,
      onStepChange: () => {},
      onComplete: () => {},
      onError: () => {},
      ...options
    };
    
    this.currentStep = this.options.initialStep;
    this.stepData = new Map();
    
    this.init();
  }
  
  init() {
    this.steps = this.form.querySelectorAll('.form-step');
    
    this.steps.forEach((step, index) => {
      step.dataset.stepIndex = index;
      step.hidden = index !== this.currentStep;
    });
    
    this.attachNavigationHandlers();
    this.updateProgressIndicator();
  }
  
  attachNavigationHandlers() {
    const nextBtn = this.form.querySelector('.wizard-next');
    const prevBtn = this.form.querySelector('.wizard-prev');
    const submitBtn = this.form.querySelector('.wizard-submit');
    
    nextBtn?.addEventListener('click', () => this.next());
    prevBtn?.addEventListener('click', () => this.prev());
    submitBtn?.addEventListener('click', () => this.submit());
  }
  
  get currentStepElement() {
    return this.steps[this.currentStep];
  }
  
  async next() {
    if (this.options.validateOnNext) {
      const isValid = await this.validateCurrentStep();
      if (!isValid) return false;
    }
    
    this.saveStepData();
    
    if (this.currentStep < this.steps.length - 1) {
      this.goToStep(this.currentStep + 1);
      return true;
    }
    
    return false;
  }
  
  prev() {
    if (this.currentStep > 0) {
      this.goToStep(this.currentStep - 1);
      return true;
    }
    return false;
  }
  
  goToStep(index) {
    if (index < 0 || index >= this.steps.length) return;
    
    const previousStep = this.currentStep;
    this.currentStep = index;
    
    if (this.options.animateTransitions) {
      this.animateStepTransition(previousStep, index);
    } else {
      this.showStep(index);
    }
    
    this.options.onStepChange(index, this.steps[index]);
  }
  
  showStep(index) {
    this.steps.forEach((step, i) => {
      step.hidden = i !== index;
    });
    
    this.updateProgressIndicator();
    this.updateNavigationButtons();
  }
  
  animateStepTransition(fromIndex, toIndex) {
    const fromStep = this.steps[fromIndex];
    const toStep = this.steps[toIndex];
    
    fromStep.hidden = false;
    toStep.hidden = false;
    
    fromStep.classList.add(toIndex > fromIndex ? 'slide-out-left' : 'slide-out-right');
    toStep.classList.add(toIndex > fromIndex ? 'slide-in-right' : 'slide-in-left');
    
    setTimeout(() => {
      fromStep.hidden = true;
      fromStep.classList.remove('slide-out-left', 'slide-out-right');
      toStep.classList.remove('slide-in-right', 'slide-in-left');
    }, 300);
  }
  
  async validateCurrentStep() {
    const step = this.currentStepElement;
    const inputs = step.querySelectorAll('input, select, textarea');
    
    let isValid = true;
    
    for (const input of inputs) {
      if (!input.checkValidity()) {
        isValid = false;
        input.reportValidity();
        this.options.onError(input);
      }
    }
    
    return isValid;
  }
  
  saveStepData() {
    const step = this.currentStepElement;
    const formData = new FormData(step);
    this.stepData.set(this.currentStep, Object.fromEntries(formData));
  }
  
  updateProgressIndicator() {
    const progress = this.form.querySelector('.wizard-progress');
    if (!progress) return;
    
    const total = this.steps.length;
    const current = this.currentStep + 1;
    
    const percent = (current / total) * 100;
    progress.style.width = `${percent}%`;
    
    const label = progress.parentElement.querySelector('.progress-label');
    if (label) {
      label.textContent = `Step ${current} of ${total}`;
    }
  }
  
  updateNavigationButtons() {
    const prevBtn = this.form.querySelector('.wizard-prev');
    const nextBtn = this.form.querySelector('.wizard-next');
    const submitBtn = this.form.querySelector('.wizard-submit');
    
    if (prevBtn) prevBtn.hidden = this.currentStep === 0;
    if (nextBtn) nextBtn.hidden = this.currentStep === this.steps.length - 1;
    if (submitBtn) submitBtn.hidden = this.currentStep < this.steps.length - 1;
  }
  
  async submit() {
    if (this.options.validateOnNext) {
      const isValid = await this.validateCurrentStep();
      if (!isValid) return;
    }
    
    this.saveStepData();
    
    const allData = {};
    this.stepData.forEach((data, step) => {
      Object.assign(allData, data);
    });
    
    this.options.onComplete(allData);
  }
}
```

### Step State Management

```javascript
class WizardStateManager {
  constructor(wizard) {
    this.wizard = wizard;
    this.storageKey = 'wizard-state';
  }
  
  saveState() {
    const state = {
      currentStep: this.wizard.currentStep,
      stepData: Object.fromEntries(this.wizard.stepData),
      timestamp: Date.now()
    };
    
    localStorage.setItem(this.storageKey, JSON.stringify(state));
  }
  
  restoreState() {
    const stored = localStorage.getItem(this.storageKey);
    if (!stored) return false;
    
    try {
      const state = JSON.parse(stored);
      
      if (this.isStateExpired(state)) {
        this.clearState();
        return false;
      }
      
      Object.entries(state.stepData).forEach(([step, data]) => {
        this.wizard.stepData.set(parseInt(step), data);
      });
      
      this.wizard.goToStep(state.currentStep);
      
      return true;
    } catch (e) {
      return false;
    }
  }
  
  isStateExpired(state) {
    const maxAge = 24 * 60 * 60 * 1000;
    return Date.now() - state.timestamp > maxAge;
  }
  
  clearState() {
    localStorage.removeItem(this.storageKey);
  }
}
```

---

## Conditional Logic

Conditional logic shows, hides, or modifies form fields based on user input, creating adaptive forms that respond to user needs.

### Basic Field Visibility

```javascript
class ConditionalFieldManager {
  constructor() {
    this.conditions = new Map();
    this.init();
  }
  
  init() {
    document.addEventListener('change', (e) => this.handleChange(e));
    this.evaluateAllConditions();
  }
  
  registerCondition(config) {
    const condition = {
      trigger: config.trigger,
      target: config.target,
      evaluate: new Function('value', 'return ' + config.condition),
      action: config.action || 'show'
    };
    
    this.conditions.set(config.target, condition);
    
    const trigger = document.querySelector(config.trigger);
    if (trigger) {
      trigger.addEventListener('change', () => this.evaluateCondition(config.target));
    }
  }
  
  handleChange(e) {
    const target = e.target;
    
    this.conditions.forEach((condition, targetSelector) => {
      if (target.matches(condition.trigger)) {
        this.evaluateCondition(targetSelector);
      }
    });
  }
  
  evaluateCondition(targetSelector) {
    const condition = this.conditions.get(targetSelector);
    if (!condition) return;
    
    const trigger = document.querySelector(condition.trigger);
    if (!trigger) return;
    
    const value = this.getFieldValue(trigger);
    const shouldShow = condition.evaluate(value);
    
    this.setFieldVisibility(targetSelector, shouldShow, condition.action);
  }
  
  getFieldValue(field) {
    if (field instanceof HTMLInputElement) {
      if (field.type === 'checkbox' || field.type === 'radio') {
        return field.checked ? field.value : null;
      }
      return field.value;
    }
    
    if (field instanceof HTMLSelectElement) {
      return field.value;
    }
    
    if (field instanceof HTMLTextAreaElement) {
      return field.value;
    }
    
    return null;
  }
  
  setFieldVisibility(selector, visible, action) {
    const target = document.querySelector(selector);
    if (!target) return;
    
    const wrapper = target.closest('.form-field');
    
    if (action === 'show' || action === 'toggle') {
      wrapper.hidden = !visible;
    }
    
    if (!visible) {
      this.disableField(target);
    } else {
      this.enableField(target);
    }
  }
  
  disableField(field) {
    field.disabled = true;
    field.setAttribute('aria-hidden', 'true');
  }
  
  enableField(field) {
    field.disabled = false;
    field.removeAttribute('aria-hidden');
  }
  
  evaluateAllConditions() {
    this.conditions.forEach((_, target) => {
      this.evaluateCondition(target);
    });
  }
}
```

### Complex Conditions

```javascript
class ComplexConditionEvaluator {
  constructor() {
    this.rules = [];
  }
  
  addRule(rule) {
    this.rules.push(rule);
    
    if (rule.triggers) {
      rule.triggers.forEach(trigger => {
        document.querySelector(trigger)?.addEventListener('change', () => {
          this.evaluate();
        });
      });
    }
  }
  
  evaluate() {
    this.rules.forEach(rule => {
      const result = this.evaluateRule(rule);
      this.applyRuleResult(rule, result);
    });
  }
  
  evaluateRule(rule) {
    const values = rule.triggers.reduce((acc, trigger) => {
      const field = document.querySelector(trigger);
      acc[trigger] = this.getValue(field);
      return acc;
    }, {});
    
    return rule.evaluate(values);
  }
  
  getValue(field) {
    if (!field) return null;
    
    if (field.type === 'checkbox') {
      return field.checked;
    }
    if (field.type === 'radio') {
      return field.checked ? field.value : null;
    }
    if (field.type === 'select-multiple') {
      return Array.from(field.selectedOptions).map(o => o.value);
    }
    
    return field.value;
  }
  
  applyRuleResult(rule, result) {
    const targets = document.querySelectorAll(rule.target);
    
    targets.forEach(target => {
      const action = result ? rule.onMatch : rule.onMismatch;
      
      if (action === 'show') {
        target.closest('.form-field').hidden = false;
        target.disabled = false;
      } else if (action === 'hide') {
        target.closest('.form-field').hidden = true;
        target.disabled = true;
      } else if (action === 'enable') {
        target.disabled = false;
      } else if (action === 'disable') {
        target.disabled = true;
      } else if (action === 'required') {
        target.required = result;
      }
    });
  }
}

// Usage
const conditions = new ComplexConditionEvaluator();

conditions.addRule({
  triggers: ['#accountType'],
  target: '#companyFields',
  evaluate: (values) => values['#accountType'] === 'business',
  onMatch: 'show',
  onMismatch: 'hide'
});

conditions.addRule({
  triggers: ['#hasPets'],
  target: '#petDetails',
  evaluate: (values) => values['#hasPets'] === true,
  onMatch: 'show',
  onMismatch: 'hide'
});

conditions.addRule({
  triggers: ['#accountType', '#subscription'],
  target: '#billingContact',
  evaluate: (values) => 
    values['#accountType'] === 'business' && 
    values['#subscription'] === 'premium',
  onMatch: 'required',
  onMismatch: 'disable'
});
```

---

## Dependent Fields

Dependent fields change their options based on values selected in other fields, creating linked select menus and conditional dropdowns.

### Cascading Select Menus

```javascript
class CascadingSelect {
  constructor(config) {
    this.masterField = document.querySelector(config.master);
    this.detailField = document.querySelector(config.detail);
    this.options = config.options || {};
    
    this.init();
  }
  
  init() {
    this.masterField.addEventListener('change', () => this.loadOptions());
    this.loadOptions();
  }
  
  loadOptions() {
    const masterValue = this.masterField.value;
    const options = this.options[masterValue] || [];
    
    this.detailField.innerHTML = '';
    
    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.textContent = 'Select...';
    this.detailField.appendChild(defaultOption);
    
    options.forEach(opt => {
      const option = document.createElement('option');
      option.value = opt.value;
      option.textContent = opt.label;
      this.detailField.appendChild(option);
    });
    
    this.detailField.disabled = options.length === 0;
  }
  
  reset() {
    this.detailField.innerHTML = '<option value="">Select...</option>';
    this.detailField.disabled = true;
  }
}

// Usage
const countryCity = new CascadingSelect({
  master: '#country',
  detail: '#city',
  options: {
    us: [
      { value: 'nyc', label: 'New York' },
      { value: 'la', label: 'Los Angeles' },
      { value: 'chi', label: 'Chicago' }
    ],
    uk: [
      { value: 'london', label: 'London' },
      { value: 'manchester', label: 'Manchester' }
    ],
    ca: [
      { value: 'toronto', label: 'Toronto' },
      { value: 'vancouver', label: 'Vancouver' }
    ]
  }
});
```

### Dependent Validation Rules

```javascript
class DependentValidator {
  constructor() {
    this.dependencies = new Map();
  }
  
  addDependency(parent, child, rules) {
    this.dependencies.set(child, { parent, rules });
    
    const parentField = document.querySelector(parent);
    parentField?.addEventListener('change', () => this.evaluate(child));
  }
  
  evaluate(childSelector) {
    const dep = this.dependencies.get(childSelector);
    if (!dep) return;
    
    const parentField = document.querySelector(dep.parent);
    const childField = document.querySelector(childSelector);
    
    const parentValue = parentField.value;
    const rule = dep.rules[parentValue];
    
    if (rule) {
      if (rule.required !== undefined) {
        childField.required = rule.required;
      }
      if (rule.minLength) {
        childField.minLength = rule.minLength;
      }
      if (rule.pattern) {
        childField.pattern = rule.pattern;
      }
      if (rule.message) {
        childField.setCustomValidity(rule.message || '');
      }
    }
  }
}

// Usage
const dependentValidation = new DependentValidator();

dependentValidation.addDependency('#shippingMethod', '#deliveryDate', {
  express: { required: true, message: '' },
  standard: { required: false },
  pickup: { required: false }
});

dependentValidation.addDependency('#country', '#postalCode', {
  us: { 
    required: true, 
    pattern: '[0-9]{5}', 
    message: 'Enter a 5-digit ZIP code' 
  },
  uk: { 
    required: true, 
    pattern: '[A-Z]{1,2}[0-9][A-Z0-9]? [0-9][A-Z]{2}',
    message: 'Enter a valid postcode'
  },
  ca: { 
    required: true, 
    pattern: '[A-Z][0-9][A-Z] [0-9][A-Z][0-9]',
    message: 'Enter a valid postal code'
  }
});
```

---

## Code Examples

### Example 1: Complete Dynamic Form System

```javascript
// File: js/dynamic-form.js

class DynamicForm {
  constructor(containerId, config = {}) {
    this.container = document.getElementById(containerId);
    this.config = {
      endpoint: config.endpoint || '',
      method: config.method || 'POST',
      maxFields: config.maxFields || 10,
      minFields: config.minFields || 1,
      fieldTypes: ['text', 'email', 'select', 'textarea'],
      ...config
    };
    
    this.fieldIndex = 0;
    this.fields = [];
    this.init();
  }
  
  init() {
    this.setupEventListeners();
    this.addInitialFields();
  }
  
  setupEventListeners() {
    const addFieldBtn = this.container.querySelector('.add-field-btn');
    addFieldBtn?.addEventListener('click', () => this.addField());
    
    this.container.addEventListener('click', (e) => {
      if (e.target.classList.contains('remove-field-btn')) {
        this.removeField(e.target.closest('.form-field-wrapper'));
      }
    });
    
    this.container.addEventListener('change', (e) => {
      if (e.target.classList.contains('field-type-select')) {
        this.handleFieldTypeChange(e.target);
      }
    });
  }
  
  addInitialFields() {
    this.addField();
    this.addField();
  }
  
  addField(type = 'text', value = '') {
    if (this.fields.length >= this.config.maxFields) {
      return null;
    }
    
    this.fieldIndex++;
    
    const wrapper = document.createElement('div');
    wrapper.className = 'form-field-wrapper';
    wrapper.dataset.fieldId = this.fieldIndex;
    
    const typeSelect = this.createTypeSelect();
    const fieldInput = this.createField(type);
    
    wrapper.innerHTML = `
      <div class="field-header">
        <label>Field ${this.fieldIndex}</label>
        <select class="field-type-select">
          ${this.config.fieldTypes.map(t => 
            `<option value="${t}" ${t === type ? 'selected' : ''}>${t}</option>`
          ).join('')}
        </select>
        <button type="button" class="remove-field-btn">Remove</button>
      </div>
      <div class="field-content">
        ${fieldInput}
      </div>
    `;
    
    this.container.querySelector('.fields-container')?.appendChild(wrapper);
    this.fields.push(wrapper);
    
    this.updateRemoveButtons();
    
    return wrapper;
  }
  
  createTypeSelect() {
    return `
      <select class="field-type-select">
        ${this.config.fieldTypes.map(t => 
          `<option value="${t}">${t}</option>`
        ).join('')}
      </select>
    `;
  }
  
  createField(type, value = '') {
    const templates = {
      text: `<input type="text" class="field-input" value="${value}">`,
      email: `<input type="email" class="field-input" value="${value}">`,
      select: `
        <select class="field-input">
          <option value="">Select...</option>
          <option value="a">Option A</option>
          <option value="b">Option B</option>
        </select>
      `,
      textarea: `<textarea class="field-input">${value}</textarea>`
    };
    
    return templates[type] || templates.text;
  }
  
  handleFieldTypeChange(select) {
    const wrapper = select.closest('.form-field-wrapper');
    const content = wrapper.querySelector('.field-content');
    const newType = select.value;
    
    content.innerHTML = this.createField(newType);
  }
  
  removeField(wrapper) {
    if (this.fields.length <= this.config.minFields) {
      return;
    }
    
    wrapper.remove();
    this.fields = this.fields.filter(f => f !== wrapper);
    
    this.updateRemoveButtons();
  }
  
  updateRemoveButtons() {
    const removeButtons = this.container.querySelectorAll('.remove-field-btn');
    const canRemove = this.fields.length > this.config.minFields;
    
    removeButtons.forEach(btn => {
      btn.disabled = !canRemove;
    });
  }
  
  getData() {
    const data = [];
    
    this.fields.forEach((wrapper, index) => {
      const typeSelect = wrapper.querySelector('.field-type-select');
      const fieldInput = wrapper.querySelector('.field-input');
      
      data.push({
        index: index + 1,
        type: typeSelect?.value || 'text',
        value: fieldInput?.value || '',
        name: fieldInput?.name || `field_${index + 1}`
      });
    });
    
    return data;
  }
  
  async submit() {
    const data = this.getData();
    
    const response = await fetch(this.config.endpoint, {
      method: this.config.method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });
    
    return response.json();
  }
}
```

### Example 2: Form Wizard with Validation

```javascript
// File: js/wizard-form.js

class WizardForm {
  constructor(formId, stepConfig) {
    this.form = document.getElementById(formId);
    this.steps = stepConfig;
    this.currentStep = 0;
    this.stepValues = {};
    this.errors = new Map();
    
    this.init();
  }
  
  init() {
    this.showStep(0);
    this.attachHandlers();
  }
  
  attachHandlers() {
    const nextBtn = this.form.querySelector('.btn-next');
    const prevBtn = this.form.querySelector('.btn-prev');
    const submitBtn = this.form.querySelector('.btn-submit');
    
    nextBtn?.addEventListener('click', () => this.handleNext());
    prevBtn?.addEventListener('click', () => this.handlePrev());
    submitBtn?.addEventListener('click', () => this.handleSubmit());
  }
  
  showStep(index) {
    this.currentStep = index;
    
    this.form.querySelectorAll('.wizard-step').forEach((step, i) => {
      step.classList.toggle('active', i === index);
      step.hidden = i !== index;
    });
    
    this.updateIndicators();
  }
  
  updateIndicators() {
    const progress = this.form.querySelector('.progress-bar');
    if (progress) {
      const percent = ((this.currentStep + 1) / this.steps.length) * 100;
      progress.style.width = `${percent}%`;
    }
    
    const stepLabels = this.form.querySelectorAll('.step-indicator');
    stepLabels.forEach((label, i) => {
      label.classList.toggle('active', i === this.currentStep);
      label.classList.toggle('completed', i < this.currentStep);
    });
  }
  
  async handleNext() {
    if (!await this.validateStep(this.currentStep)) {
      return;
    }
    
    this.saveStepValues();
    
    if (this.currentStep < this.steps.length - 1) {
      this.showStep(this.currentStep + 1);
    }
  }
  
  handlePrev() {
    if (this.currentStep > 0) {
      this.showStep(this.currentStep - 1);
    }
  }
  
  async handleSubmit() {
    if (!await this.validateStep(this.currentStep)) {
      return;
    }
    
    this.saveStepValues();
    
    const data = this.collectAllData();
    console.log('Form submitted:', data);
  }
  
  async validateStep(stepIndex) {
    const stepConfig = this.steps[stepIndex];
    const stepElement = this.form.querySelector(`#${stepConfig.id}`);
    const inputs = stepElement?.querySelectorAll('[name]');
    
    let isValid = true;
    
    for (const input of inputs || []) {
      if (!input.checkValidity()) {
        isValid = false;
        this.showFieldError(input, input.validationMessage);
      } else {
        this.clearFieldError(input);
      }
    }
    
    if (stepConfig.customValidators) {
      for (const validator of stepConfig.customValidators) {
        const result = await validator(this.stepValues[stepIndex] || {});
        if (result !== true) {
          isValid = false;
          break;
        }
      }
    }
    
    return isValid;
  }
  
  showFieldError(input, message) {
    const wrapper = input.closest('.form-field');
    const errorEl = wrapper?.querySelector('.error-message');
    
    if (errorEl) {
      errorEl.textContent = message;
      errorEl.setAttribute('role', 'alert');
    }
    
    input.setAttribute('aria-invalid', 'true');
  }
  
  clearFieldError(input) {
    const wrapper = input.closest('.form-field');
    const errorEl = wrapper?.querySelector('.error-message');
    
    if (errorEl) {
      errorEl.textContent = '';
    }
    
    input.removeAttribute('aria-invalid');
  }
  
  saveStepValues() {
    const stepElement = this.form.querySelector(`#${this.steps[this.currentStep].id}`);
    const formData = new FormData(stepElement);
    
    this.stepValues[this.currentStep] = Object.fromEntries(formData);
  }
  
  collectAllData() {
    return Object.assign({}, ...Object.values(this.stepValues));
  }
}

// Usage
const wizard = new WizardForm('registrationWizard', [
  { 
    id: 'step-account',
    validators: []
  },
  { 
    id: 'step-profile',
    validators: []
  },
  { 
    id: 'step-confirm',
    validators: [
      async (data) => {
        if (!data.acceptTerms) {
          return 'You must accept the terms';
        }
        return true;
      }
    ]
  }
]);
```

### Example 3: Dependent Fields Manager

```javascript
// File: js/dependent-fields.js

class DependentFieldsManager {
  constructor(containerId) {
    this.container = document.getElementById(containerId);
    this.dependencies = new Map();
    this.init();
  }
  
  init() {
    document.addEventListener('change', (e) => {
      this.handleFieldChange(e.target);
    });
  }
  
  register(config) {
    this.dependencies.set(config.child, {
      parent: config.parent,
      options: config.options,
      defaultOptions: config.defaultOptions || [],
      rules: config.rules
    });
    
    this.updateOptions(config.child);
  }
  
  handleFieldChange(changedField) {
    this.dependencies.forEach((config, child) => {
      if (config.parent === changedField.name) {
        this.updateOptions(child, changedField.value);
      }
    });
  }
  
  updateOptions(childName, selectedValue = null) {
    const dep = this.dependencies.get(childName);
    if (!dep) return;
    
    const child = this.container.querySelector(`[name="${childName}"]`);
    if (!child) return;
    
    const parent = this.container.querySelector(`[name="${dep.parent}"]`);
    const parentValue = selectedValue ?? parent?.value;
    
    const options = dep.options[parentValue] || dep.defaultOptions;
    
    this.clearOptions(child);
    
    options.forEach(opt => {
      const option = document.createElement('option');
      option.value = opt.value;
      option.textContent = opt.label;
      child.appendChild(option);
    });
    
    if (dep.rules && dep.rules[parentValue]) {
      this.applyRules(child, dep.rules[parentValue]);
    } else {
      this.clearRules(child);
    }
  }
  
  clearOptions(select) {
    select.innerHTML = '<option value="">Select...</option>';
  }
  
  applyRules(field, rules) {
    if (rules.required !== undefined) {
      field.required = rules.required;
    }
    if (rules.disabled !== undefined) {
      field.disabled = rules.disabled;
    }
    if (rules.placeholder) {
      field.querySelector('option').textContent = rules.placeholder;
    }
  }
  
  clearRules(field) {
    field.required = false;
    field.disabled = false;
  }
}

// Usage
const dependents = new DependentFieldsManager('formContainer');

dependents.register({
  parent: 'country',
  child: 'province',
  options: {
    us: [
      { value: 'ca', label: 'California' },
      { value: 'ny', label: 'New York' },
      { value: 'tx', label: 'Texas' }
    ],
    ca: [
      { value: 'on', label: 'Ontario' },
      { value: 'bc', label: 'British Columbia' }
    ]
  },
  defaultOptions: [
    { value: '', label: 'Select country first' }
  ],
  rules: {
    us: { required: true },
    ca: { required: true },
    default: { required: false }
  }
});

dependents.register({
  parent: 'province',
  child: 'city',
  options: {
    ca: {
      on: [
        { value: 'toronto', label: 'Toronto' },
        { value: 'ottawa', label: 'Ottawa' }
      ],
      bc: [
        { value: 'vancouver', label: 'Vancouver' },
        { value: 'victoria', label: 'Victoria' }
      ]
    },
    default: []
  },
  defaultOptions: [
    { value: '', label: 'Select province first' }
  ]
});
```

### Example 4: Multi-Level Conditional Form

```javascript
// File: js/conditional-form.js

class ConditionalFormBuilder {
  constructor(formId) {
    this.form = document.getElementById(formId);
    this.conditions = [];
  }
  
  addCondition(config) {
    this.conditions.push({
      trigger: config.trigger,
      condition: config.condition,
      actions: config.actions
    });
    
    const triggerField = this.form.querySelector(config.trigger);
    triggerField?.addEventListener('change', () => this.evaluateConditions());
  }
  
  evaluateConditions() {
    this.conditions.forEach(condition => {
      const trigger = this.form.querySelector(condition.trigger);
      const value = this.getFieldValue(trigger);
      const matches = condition.condition(value);
      
      condition.actions.forEach(action => {
        this.performAction(action.target, action.operation, matches ? action.onMatch : action.onMismatch);
      });
    });
  }
  
  getFieldValue(field) {
    if (!field) return null;
    
    if (field.type === 'checkbox') return field.checked;
    if (field.type === 'radio') return field.checked ? field.value : null;
    if (field.type === 'select-multiple') {
      return Array.from(field.selectedOptions).map(o => o.value);
    }
    
    return field.value;
  }
  
  performAction(target, operation, value) {
    const elements = this.form.querySelectorAll(target);
    
    elements.forEach(el => {
      switch (operation) {
        case 'show':
          el.closest('.form-field').hidden = !value;
          break;
        case 'enable':
          el.disabled = !value;
          break;
        case 'required':
          el.required = value;
          break;
        case 'setValue':
          el.value = value;
          break;
        case 'addClass':
          el.classList.toggle(value, value);
          break;
      }
    });
  }
}

// Usage
const conditionalForm = new ConditionalFormBuilder('registrationForm');

conditionalForm.addCondition({
  trigger: '#accountType',
  condition: (value) => value === 'business',
  actions: [
    {
      target: '#companyFields',
      operation: 'show',
      onMatch: true,
      onMismatch: false
    },
    {
      target: '#companyName',
      operation: 'required',
      onMatch: true,
      onMismatch: false
    }
  ]
});

conditionalForm.addCondition({
  trigger: '#newsletter',
  condition: (value) => value === true,
  actions: [
    {
      target: '#emailPreferences',
      operation: 'show',
      onMatch: true,
      onMismatch: false
    }
  ]
});

conditionalForm.addCondition({
  trigger: '#subscription',
  condition: (value) => value === 'premium',
  actions: [
    {
      target: '#premiumFeatures',
      operation: 'show',
      onMatch: true,
      onMismatch: false
    },
    {
      target: '#billingCycle',
      operation: 'enable',
      onMatch: true,
      onMismatch: false
    }
  ]
});
```

### Example 5: Form State Persistence

```javascript
// File: js/form-state.js

class FormStateManager {
  constructor(formId, options = {}) {
    this.form = document.getElementById(formId);
    this.options = {
      storageKey: `${formId}-state`,
      excludeFields: ['password', 'token', 'secret'],
      autoSaveInterval: 5000,
      ...options
    };
    
    this.init();
  }
  
  init() {
    this.attachListeners();
    this.startAutoSave();
    this.restore();
  }
  
  attachListeners() {
    this.form.addEventListener('input', () => {
      this.debouncedSave();
    });
    
    this.form.addEventListener('change', () => {
      this.debouncedSave();
    });
    
    this.form.addEventListener('submit', () => {
      this.clear();
    });
  }
  
  debouncedSave() {
    clearTimeout(this.saveTimeout);
    this.saveTimeout = setTimeout(() => {
      this.save();
    }, this.options.autoSaveInterval);
  }
  
  startAutoSave() {
    if (this.options.autoSaveInterval > 0) {
      setInterval(() => this.save(), this.options.autoSaveInterval);
    }
  }
  
  save() {
    const data = {};
    const inputs = this.form.querySelectorAll('[name]');
    
    inputs.forEach(input => {
      if (this.shouldExclude(input)) return;
      
      if (input.type === 'file') {
        if (input.files.length > 0) {
          data[input.name] = {
            name: input.files[0].name,
            size: input.files[0].size,
            type: input.files[0].type
          };
        }
      } else if (input.type === 'checkbox') {
        data[input.name] = input.checked;
      } else if (input.type === 'radio') {
        if (input.checked) {
          data[input.name] = input.value;
        }
      } else {
        data[input.name] = input.value;
      }
    });
    
    try {
      localStorage.setItem(this.options.storageKey, JSON.stringify(data));
    } catch (e) {
      console.warn('Failed to save form state:', e);
    }
  }
  
  restore() {
    const stored = localStorage.getItem(this.options.storageKey);
    if (!stored) return;
    
    try {
      const data = JSON.parse(stored);
      
      Object.entries(data).forEach(([name, value]) => {
        const input = this.form.querySelector(`[name="${name}"]`);
        if (!input || this.shouldExclude(input)) return;
        
        if (input.type === 'checkbox') {
          input.checked = value;
        } else if (input.type === 'radio') {
          if (input.value === value) {
            input.checked = true;
          }
        } else if (input.type !== 'file') {
          input.value = value;
        }
      });
    } catch (e) {
      console.warn('Failed to restore form state:', e);
    }
  }
  
  shouldExclude(input) {
    return this.options.excludeFields.includes(input.name) ||
           input.type === 'password';
  }
  
  clear() {
    localStorage.removeItem(this.options.storageKey);
  }
}
```

---

## Performance Optimization

### Lazy Loading Fields

```javascript
function createLazyFieldLoader(containerSelector, template) {
  const container = document.querySelector(containerSelector);
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        loadField(entry.target);
        observer.unobserve(entry.target);
      }
    });
  });
  
  function loadField(el) {
    const index = el.dataset.fieldIndex;
    el.innerHTML = template.replace('{{index}}', index);
  }
  
  container.querySelectorAll('.lazy-field').forEach(el => {
    observer.observe(el);
  });
}
```

---

## Accessibility in Dynamic Forms

### ARIA Live Regions

```javascript
function announceToScreenReader(message, priority = 'polite') {
  const announcement = document.createElement('div');
  announcement.setAttribute('role', 'status');
  announcement.setAttribute('aria-live', priority);
  announcement.className = 'sr-only';
  announcement.textContent = message;
  
  document.body.appendChild(announcement);
  
  setTimeout(() => announcement.remove(), 1000);
}
```

---

## Key Takeaways

1. **Dynamic Forms**: Use event delegation and DOM manipulation for adding/removing fields efficiently.

2. **Form Wizards**: Implement step-by-step navigation with validation at each step before progressing.

3. **Conditional Logic**: Use data attributes to store conditions, evaluate them on change events.

4. **Dependent Fields**: Create cascading selects where child options depend on parent selections.

5. **State Persistence**: Save form state to localStorage for recovery after page refresh.

---

## Common Pitfalls

1. **Missing Field Names**: Always ensure dynamically added fields have unique name attributes.

2. **Validation Not Running**: Re-run validation when showing/hiding fields conditionally.

3. **Memory Leaks**: Clean up event listeners and observers when removing dynamic fields.

4. **Accessibility**: Add ARIA live regions and announce changes to screen readers.

5. **State Not Saved**: Don't forget to restore form state when reloading dynamic forms.

---

## Cross-Reference

- Previous: [Form Data Handling](./03_FORM_DATA_HANDLING.md)
- Next: [Form Validation Libraries](./05_FORM_VALIDATION_LIBRARIES.md)
- Related: [JavaScript Form Validation](./02_JAVASCRIPT_FORM_VALIDATION.md)