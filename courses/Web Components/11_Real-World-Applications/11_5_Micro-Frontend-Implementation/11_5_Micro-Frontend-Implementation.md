# Micro-Frontend Implementation

## OVERVIEW

Micro-frontend implementation uses Web Components as integration boundaries. This guide covers component distribution, application shell patterns, and cross-team coordination.

## IMPLEMENTATION DETAILS

### Application Shell

```javascript
class AppShell extends HTMLElement {
  #modules = new Map();
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.render();
    this.#initRouter();
  }
  
  async #initRouter() {
    const path = window.location.pathname;
    await this.#loadModule(path);
  }
  
  async #loadModule(path) {
    const module = this.#modules.get(path);
    if (!module) return;
    
    const component = await import(module);
    const container = this.shadowRoot.querySelector('.content');
    container.innerHTML = '';
    container.appendChild(new component.default());
  }
  
  registerRoute(path, loader) {
    this.#modules.set(path, loader);
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host { display: block; }
        .shell { min-height: 100vh; display: flex; flex-direction: column; }
        header { padding: 16px; background: #f5f5f5; }
        nav a { margin-right: 16px; }
        .content { flex: 1; padding: 24px; }
      </style>
      <div class="shell">
        <header>
          <nav>
            <a href="/">Home</a>
            <a href="/products">Products</a>
            <a href="/cart">Cart</a>
          </nav>
        </header>
        <main class="content"></main>
      </div>
    `;
  }
}
customElements.define('app-shell', AppShell);
```

### Module Federation

```javascript
class RemoteLoader extends HTMLElement {
  #loaded = new Map();
  
  static get observedAttributes() { return ['src']; }
  
  async attributeChangedCallback() {
    await this.#loadRemote();
  }
  
  async #loadRemote() {
    const src = this.getAttribute('src');
    if (!src || this.#loaded.has(src)) return;
    
    const module = await import(src);
    this.#loaded.set(src, module);
    this.#render();
  }
  
  #render() {
    const src = this.getAttribute('src');
    const module = this.#loaded.get(src);
    if (!module) return;
    
    this.innerHTML = '';
    const Component = module.default || module;
    this.appendChild(new Component());
  }
}
customElements.define('remote-loader', RemoteLoader);
```

## NEXT STEPS

Proceed to **12_Tooling/12_1_Build-Tool-Integration**.