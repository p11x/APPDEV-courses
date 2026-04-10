/**
 * HTML Standards and History - Semantic Element Examples
 * @description Demonstrates semantic HTML elements and history of HTML standards
 * @module basics/html-standards
 * @version 1.0.0
 */

// ============================================
// Semantic Article Component
// ============================================

/**
 * ArticleElement - Semantic article component
 * Demonstrates proper use of semantic HTML5 elements
 */
class ArticleElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
  }

  static get observedAttributes() {
    return ['author', 'publish-date', 'category'];
  }

  attributeChangedCallback() {
    this.render();
  }

  get author() {
    return this.getAttribute('author') || 'Anonymous';
  }

  get publishDate() {
    return this.getAttribute('publish-date') || new Date().toISOString();
  }

  get category() {
    return this.getAttribute('category') || 'General';
  }

  render() {
    const date = new Date(this.publishDate).toLocaleDateString();
    
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        }
        
        article {
          background: white;
          border-radius: 12px;
          overflow: hidden;
          box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        
        header {
          padding: 20px;
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          color: white;
        }
        
        h1 {
          margin: 0 0 10px;
          font-size: 24px;
          font-weight: 700;
        }
        
        .meta {
          display: flex;
          gap: 15px;
          font-size: 14px;
          opacity: 0.9;
        }
        
        .meta span {
          display: flex;
          align-items: center;
        }
        
        .content {
          padding: 20px;
          line-height: 1.6;
          color: #333;
        }
        
        footer {
          padding: 15px 20px;
          background: #f8f9fa;
          border-top: 1px solid #eee;
        }
        
        .tag {
          display: inline-block;
          padding: 4px 12px;
          background: #e9ecef;
          border-radius: 20px;
          font-size: 12px;
          color: #495057;
        }
        
        [hidden] {
          display: none;
        }
      </style>
      
      <article itemscope itemtype="https://schema.org/Article">
        <header>
          <h1 itemprop="headline">
            <slot name="title">Untitled Article</slot>
          </h1>
          <div class="meta">
            <span itemprop="author">👤 ${this.author}</span>
            <span itemprop="datePublished" datetime="${this.publishDate}">📅 ${date}</span>
          </div>
        </header>
        
        <div class="content" itemprop="articleBody">
          <slot></slot>
        </div>
        
        <footer>
          <span class="tag" itemprop="articleSection">${this.category}</span>
        </footer>
      </article>
    `;
  }
}

customElements.define('semantic-article', ArticleElement);

// ============================================
// Form Element with Semantic Markup
// ============================================

/**
 * SemanticFormElement - Demonstrates semantic form elements
 */
class SemanticFormElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
    this._setupValidation();
  }

  _setupValidation() {
    const form = this.shadowRoot.querySelector('form');
    form.addEventListener('submit', (e) => {
      e.preventDefault();
      this._handleSubmit(e);
    });
  }

  _handleSubmit(event) {
    const formData = new FormData(event.target);
    const data = Object.fromEntries(formData.entries());
    
    this.dispatchEvent(new CustomEvent('form-submit', {
      detail: data,
      bubbles: true,
      composed: true
    }));
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
        }
        
        form {
          background: white;
          padding: 24px;
          border-radius: 12px;
          box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        
        fieldset {
          border: none;
          padding: 0;
          margin: 0 0 20px;
        }
        
        legend {
          font-size: 18px;
          font-weight: 600;
          margin-bottom: 15px;
          color: #333;
        }
        
        label {
          display: block;
          margin-bottom: 8px;
          font-weight: 500;
          color: #555;
        }
        
        input, textarea, select {
          width: 100%;
          padding: 10px;
          border: 1px solid #ddd;
          border-radius: 6px;
          font-size: 14px;
          font-family: inherit;
          box-sizing: border-box;
        }
        
        input:focus, textarea:focus, select:focus {
          outline: none;
          border-color: #667eea;
          box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
        }
        
        button[type="submit"] {
          background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
          color: white;
          border: none;
          padding: 12px 24px;
          border-radius: 6px;
          font-weight: 600;
          cursor: pointer;
        }
        
        button[type="submit"]:hover {
          transform: translateY(-2px);
          box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
        }
      </style>
      
      <form>
        <fieldset>
          <legend>Contact Information</legend>
          
          <label for="name">Full Name</label>
          <input type="text" id="name" name="name" required 
                 placeholder="Enter your name" autocomplete="name">
          
          <label for="email">Email Address</label>
          <input type="email" id="email" name="email" required 
                 placeholder="Enter your email" autocomplete="email">
          
          <label for="message">Message</label>
          <textarea id="message" name="message" rows="4" 
                    placeholder="Your message here"></textarea>
        </fieldset>
        
        <button type="submit">Submit</button>
      </form>
    `;
  }
}

customElements.define('semantic-form', SemanticFormElement);

// ============================================
// Navigation Component
// ============================================

/**
 * SemanticNavElement - Semantic navigation component
 */
class SemanticNavElement extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }

  connectedCallback() {
    this.render();
    this._handleCurrentPage();
  }

  _handleCurrentPage() {
    const currentPath = window.location.pathname;
    const links = this.shadowRoot.querySelectorAll('a');
    
    links.forEach(link => {
      if (link.getAttribute('href') === currentPath) {
        link.setAttribute('aria-current', 'page');
      }
    });
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
        }
        
        nav {
          background: white;
          padding: 0;
          box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        
        ul {
          display: flex;
          list-style: none;
          margin: 0;
          padding: 0;
        }
        
        li {
          margin: 0;
        }
        
        a {
          display: block;
          padding: 16px 24px;
          color: #555;
          text-decoration: none;
          font-weight: 500;
          transition: all 0.2s ease;
          border-bottom: 3px solid transparent;
        }
        
        a:hover {
          color: #667eea;
          background: #f8f9fa;
        }
        
        a[aria-current="page"] {
          color: #667eea;
          border-bottom-color: #667eea;
        }
      </style>
      
      <nav role="navigation" aria-label="Main navigation">
        <ul>
          <li><a href="/">Home</a></li>
          <li><a href="/about">About</a></li>
          <li><a href="/services">Services</a></li>
          <li><a href="/blog">Blog</a></li>
          <li><a href="/contact">Contact</a></li>
        </ul>
      </nav>
    `;
  }
}

customElements.define('semantic-nav', SemanticNavElement);

export { ArticleElement, SemanticFormElement, SemanticNavElement };