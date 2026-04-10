# Component Versioning Strategies

## OVERVIEW

Versioning strategies manage component updates and compatibility. This guide covers semantic versioning, migration patterns, and deprecation handling.

## IMPLEMENTATION DETAILS

### Semantic Versioning

```javascript
class VersionedElement extends HTMLElement {
  static get version() { return '1.2.3'; }
  // major: breaking changes
  // minor: new features (backward compatible)
  // patch: bug fixes
}
```

### Version Detection

```javascript
customElements.whenDefined('my-element').then(() => {
  const el = document.querySelector('my-element');
  console.log('Version:', el.constructor.version);
});
```

### Deprecation Pattern

```javascript
class DeprecatedElement extends HTMLElement {
  attributeChangedCallback(name, oldVal, newVal) {
    if (name === 'oldProp') {
      console.warn('oldProp is deprecated. Use newProp instead.');
      this.setAttribute('newProp', newVal);
    }
  }
}
```

## NEXT STEPS

Proceed to **10_Advanced-Patterns/10_3_Testing-Framework-Integration**.