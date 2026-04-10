# 01_1 Introduction to Web Components - Specification

## Overview
Basic greeting element demonstrating fundamental Web Component concepts.

## Implementation Details

### Component Class
- **Name**: `GreetingElement`
- **Tag**: `greeting-element`
- **Extends**: `HTMLElement`

### Properties
| Property | Type | Default | Description |
|----------|------|---------|-------------|
| name | string | 'World' | Name to display in greeting |
| greetingType | string | 'hello' | Type of greeting (hello, hi, hey, welcome) |

### Attributes
| Attribute | Type | Description |
|-----------|------|-------------|
| name | string | Name to display |
| greeting-type | string | Type of greeting |
| highlight | boolean | Visual highlight state |

### Lifecycle Callbacks
- `constructor()` - Initialize shadow DOM
- `connectedCallback()` - Render component
- `disconnectedCallback()` - Cleanup
- `attributeChangedCallback()` - React to changes

### Observed Attributes
- name
- greeting-type

## Usage Examples

### HTML
```html
<greeting-element></greeting-element>
<greeting-element name="Developer"></greeting-element>
<greeting-element name="World" greeting-type="hi"></greeting-element>
```

### JavaScript
```javascript
const greeting = document.querySelector('greeting-element');
greeting.name = 'New Name';
greeting.greetingType = 'welcome';
```

## Accessibility
- ARIA role: `status`
- ARIA live region: `polite`

## Browser Support
- Chrome 66+
- Firefox 63+
- Safari 12+
- Edge 79+

## Performance
- Shadow DOM encapsulation
- Efficient attribute observation
- No external dependencies