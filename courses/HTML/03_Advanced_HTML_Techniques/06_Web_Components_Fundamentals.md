# Web Components Fundamentals

## Topic Title
Introduction to Web Components

## Concept Explanation

### What are Web Components?

Web Components are a set of web platform APIs that allow you to create reusable custom elements with encapsulated functionality.

### Key Technologies

1. **Custom Elements** - Define new HTML tags
2. **Shadow DOM** - Encapsulated styling and markup
3. **HTML Templates** - Reusable markup fragments
4. **HTML Imports** - Import component definitions

### Benefits

1. **Reusability** - Create once, use everywhere
2. **Encapsulation** - Styles don't leak
3. **Standard-based** - Native browser support
4. **Framework-agnostic** - Works with any framework

## Code Examples

### Example 1: Simple Custom Element

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Web Components</title>
</head>
<body>
    <h1>My First Web Component</h1>
    
    <!-- Using the custom element -->
    <user-card name="John Doe" role="Web Developer"></user-card>
    <user-card name="Jane Smith" role="Designer"></user-card>
    
    <script>
        class UserCard extends HTMLElement {
            constructor() {
                super();
                this.attachShadow({ mode: 'open' });
            }
            
            connectedCallback() {
                const name = this.getAttribute('name');
                const role = this.getAttribute('role');
                
                this.shadowRoot.innerHTML = `
                    <style>
                        :host {
                            display: block;
                            border: 2px solid #333;
                            padding: 20px;
                            margin: 10px;
                            max-width: 300px;
                        }
                        h2 { margin: 0 0 10px; }
                        p { color: #666; }
                    </style>
                    <h2>${name}</h2>
                    <p>${role}</p>
                `;
            }
        }
        
        customElements.define('user-card', UserCard);
    </script>
</body>
</html>
```

### Example 2: HTML Template

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>HTML Template</title>
</head>
<body>
    <h1>HTML Template Demo</h1>
    
    <!-- Template definition -->
    <template id="my-template">
        <style>
            .card {
                border: 1px solid #ddd;
                padding: 20px;
                margin: 10px;
                border-radius: 8px;
            }
            .title {
                font-weight: bold;
                font-size: 1.2em;
                color: #333;
            }
        </style>
        <div class="card">
            <div class="title"><slot name="title">Default Title</slot></div>
            <div class="content"><slot></slot></div>
        </div>
    </template>
    
    <!-- Using the template -->
    <my-card>
        <span slot="title">Card Title</span>
        <p>This is the card content.</p>
    </my-card>
    
    <script>
        class MyCard extends HTMLElement {
            constructor() {
                super();
                this.attachShadow({ mode: 'open' });
            }
            
            connectedCallback() {
                const template = document.getElementById('my-template');
                const clone = template.content.cloneNode(true);
                this.shadowRoot.appendChild(clone);
            }
        }
        
        customElements.define('my-card', MyCard);
    </script>
</body>
</html>
```

### Example 3: Reusable Button Component

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Button Component</title>
</head>
<body>
    <h1>Custom Button Component</h1>
    
    <my-button variant="primary">Click Me</my-button>
    <my-button variant="secondary">Secondary</my-button>
    <my-button variant="danger">Delete</my-button>
    
    <script>
        class MyButton extends HTMLElement {
            static get observedAttributes() {
                return ['variant'];
            }
            
            constructor() {
                super();
                this.attachShadow({ mode: 'open' });
            }
            
            connectedCallback() {
                this.render();
            }
            
            attributeChangedCallback(name, oldValue, newValue) {
                this.render();
            }
            
            render() {
                const variant = this.getAttribute('variant') || 'primary';
                const text = this.textContent;
                
                const colors = {
                    primary: '#007bff',
                    secondary: '#6c757d',
                    danger: '#dc3545'
                };
                
                this.shadowRoot.innerHTML = `
                    <style>
                        button {
                            padding: 10px 20px;
                            border: none;
                            border-radius: 5px;
                            color: white;
                            cursor: pointer;
                            font-size: 16px;
                            background: ${colors[variant]};
                        }
                        button:hover {
                            opacity: 0.9;
                        }
                    </style>
                    <button>${text}</button>
                `;
            }
        }
        
        customElements.define('my-button', MyButton);
    </script>
</body>
</html>
```

## Best Practices

1. **Use Shadow DOM** - For style encapsulation
2. **Follow naming convention** - Use kebab-case (my-element)
3. **Provide fallbacks** - For unsupported browsers
4. **Document your components** - Clear usage instructions

## When to Use

- **Reusable UI elements** - Buttons, cards, inputs
- **Design systems** - Consistent component library
- **Framework-agnostic code** - Work anywhere

## Exercises

### Exercise 1: Create Simple Card
Build a custom card component with title and content.

### Exercise 2: Add Properties
Add properties to customize the component.
