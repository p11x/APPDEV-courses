/**
 * Animation and Transition Patterns - Demonstrates animation and transition patterns for Web Components
 * including CSS transitions, keyframe animations, WAAPI, and scroll-driven animations
 * @module styling/06_7_Animation-and-Transition-Patterns
 * @version 1.0.0
 * @example <animated-card></animated-card>
 */

const ANIMATION_PRESETS = {
  fadeIn: {
    from: { opacity: 0 },
    to: { opacity: 1 }
  },
  fadeOut: {
    from: { opacity: 1 },
    to: { opacity: 0 }
  },
  slideInLeft: {
    from: { transform: 'translateX(-100%)', opacity: 0 },
    to: { transform: 'translateX(0)', opacity: 1 }
  },
  slideInRight: {
    from: { transform: 'translateX(100%)', opacity: 0 },
    to: { transform: 'translateX(0)', opacity: 1 }
  },
  slideInUp: {
    from: { transform: 'translateY(100%)', opacity: 0 },
    to: { transform: 'translateY(0)', opacity: 1 }
  },
  slideInDown: {
    from: { transform: 'translateY(-100%)', opacity: 0 },
    to: { transform: 'translateY(0)', opacity: 1 }
  },
  scaleIn: {
    from: { transform: 'scale(0)', opacity: 0 },
    to: { transform: 'scale(1)', opacity: 1 }
  },
  scaleOut: {
    from: { transform: 'scale(1)', opacity: 1 },
    to: { transform: 'scale(0)', opacity: 0 }
  },
  bounceIn: {
    from: { transform: 'scale(0.3)', opacity: 0 },
    to: { transform: 'scale(1)', opacity: 1 }
  },
  flipIn: {
    from: { transform: 'rotateY(90deg)', opacity: 0 },
    to: { transform: 'rotateY(0deg)', opacity: 1 }
  }
};

class AnimatedCard extends HTMLElement {
  #shadowRoot;
  #animationController;
  #styleSheet;
  #currentAnimation;

  static get observedAttributes() {
    return ['animation', 'duration', 'easing', 'delay', 'loop', 'fill'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
    this.#animationController = null;
    this.#currentAnimation = null;
  }

  #initializeStyles() {
    this.#styleSheet.replaceSync(`
      :host {
        display: block;
        contain: content layout style;
      }
      .card {
        background: #ffffff;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        padding: 16px;
        opacity: 0;
      }
      .card.animated {
        animation-fill-mode: forwards;
      }
      .title {
        color: #333333;
        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        font-size: 1.25rem;
        font-weight: 600;
        margin: 0 0 8px 0;
      }
      .content {
        color: #666666;
        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        font-size: 1rem;
        line-height: 1.5;
      }
      @keyframes fadeIn {
        from { opacity: 0; }
        to { opacity: 1; }
      }
      @keyframes slideInUp {
        from { transform: translateY(100%); opacity: 0; }
        to { transform: translateY(0); opacity: 1; }
      }
      @keyframes slideInLeft {
        from { transform: translateX(-100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
      }
      @keyframes slideInRight {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
      }
      @keyframes scaleIn {
        from { transform: scale(0); opacity: 0; }
        to { transform: scale(1); opacity: 1; }
      }
      @keyframes bounceIn {
        0% { transform: scale(0.3); opacity: 0; }
        50% { transform: scale(1.05); }
        70% { transform: scale(0.9); }
        100% { transform: scale(1); opacity: 1; }
      }
      @keyframes flipIn {
        from { transform: rotateY(90deg); opacity: 0; }
        to { transform: rotateY(0deg); opacity: 1; }
      }
      @keyframes pulse {
        0% { transform: scale(1); }
        50% { transform: scale(1.05); }
        100% { transform: scale(1); }
      }
      @keyframes shimmer {
        0% { background-position: -200% 0; }
        100% { background-position: 200% 0; }
      }
      .shimmer {
        background: linear-gradient(90deg, transparent, rgba(255,255,255,0.4), transparent);
        background-size: 200% 100%;
        animation: shimmer 2s infinite;
      }
    `);
  }

  connectedCallback() {
    this.#initializeStyles();
    this.#render();
    this.#setupAnimation();
  }

  #render() {
    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];
    
    const template = document.createElement('template');
    template.innerHTML = `
      <article class="card">
        <h2 class="title"><slot name="title">Animated Card</slot></h2>
        <div class="content">
          <slot></slot>
        </div>
      </article>
    `;
    
    this.#shadowRoot.appendChild(template.content.cloneNode(true));
  }

  #setupAnimation() {
    if (!this.hasAttribute('animation')) {
      this.#currentAnimation = 'fadeIn';
      return;
    }

    const animation = this.getAttribute('animation');
    this.#currentAnimation = animation;
    
    const card = this.#shadowRoot.querySelector('.card');
    if (card) {
      card.classList.add('animated');
    }
  }

  get animationName() {
    return this.#currentAnimation;
  }

  set animationName(value) {
    this.setAttribute('animation', value);
  }

  play() {
    return this.#playAnimation();
  }

  pause() {
    if (this.#animationController) {
      this.#animationController.pause();
    }
  }

  cancel() {
    if (this.#animationController) {
      this.#animationController.cancel();
    }
  }

  async #playAnimation() {
    const card = this.#shadowRoot.querySelector('.card');
    if (!card) return null;

    const duration = parseInt(this.getAttribute('duration')) || 300;
    const easing = this.getAttribute('easing') || 'ease-out';
    const delay = parseInt(this.getAttribute('delay')) || 0;
    const loop = this.hasAttribute('loop');
    const fill = this.getAttribute('fill') || 'forwards';

    const preset = ANIMATION_PRESETS[this.#currentAnimation] || ANIMATION_PRESETS.fadeIn;

    const keyframes = this.#createKeyframes(preset);
    const options = {
      duration,
      easing,
      delay,
      fill,
      iterations: loop ? Infinity : 1
    };

    this.#animationController = card.animate(keyframes, options);
    
    return this.#animationController.finished;
  }

  #createKeyframes(preset) {
    return [
      { ...preset.from, offset: 0 },
      { ...preset.to, offset: 1 }
    ];
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    if (name === 'animation') {
      this.#currentAnimation = newValue;
    }
  }
}

class TransitionComponent extends HTMLElement {
  #shadowRoot;
  #transitionController;
  #styleSheet;

  static get observedAttributes() {
    return ['transition', 'duration', 'easing'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
  }

  #initializeStyles() {
    this.#styleSheet.replaceSync(`
      :host {
        display: block;
      }
      .transition-element {
        background: #ffffff;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        padding: 24px;
        transition-property: all;
        transition-duration: 300ms;
        transition-timing-function: ease-out;
      }
      :host([hover]) .transition-element {
        transform: translateY(-4px);
        box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
      }
      .hidden {
        opacity: 0;
        transform: scale(0.9);
      }
      .visible {
        opacity: 1;
        transform: scale(1);
      }
    `);
  }

  connectedCallback() {
    this.#initializeStyles();
    this.#render();
    this.#setupTransitions();
  }

  #render() {
    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];
    
    const template = document.createElement('template');
    template.innerHTML = `
      <div class="transition-element">
        <slot></slot>
      </div>
    `;
    
    this.#shadowRoot.appendChild(template.content.cloneNode(true));
  }

  #setupTransitions() {
    const element = this.#shadowRoot.querySelector('.transition-element');
    if (!element) return;

    const duration = parseInt(this.getAttribute('duration')) || 300;
    const easing = this.getAttribute('easing') || 'ease-out';
    
    element.style.transitionDuration = `${duration}ms`;
    element.style.transitionTimingFunction = easing;
  }

  transitionTo(properties, options = {}) {
    return new Promise((resolve) => {
      const element = this.#shadowRoot.querySelector('.transition-element');
      if (!element) {
        resolve();
        return;
      }

      const duration = options.duration || parseInt(this.getAttribute('duration')) || 300;
      const easing = options.easing || this.getAttribute('easing') || 'ease-out';

      element.style.transitionProperty = Object.keys(properties).join(', ');
      element.style.transitionDuration = `${duration}ms`;
      element.style.transitionTimingFunction = easing;

      for (const [prop, value] of Object.entries(properties)) {
        element.style[prop] = value;
      }

      setTimeout(() => resolve(), duration);
    });
  }

  setHover(hover) {
    if (hover) {
      this.setAttribute('hover', '');
    } else {
      this.removeAttribute('hover');
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    this.#setupTransitions();
  }
}

class ScrollAnimationComponent extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #scrollObserver;

  static get observedAttributes() {
    return ['threshold', 'root-margin', 'once'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
  }

  #initializeStyles() {
    this.#styleSheet.replaceSync(`
      :host {
        display: block;
      }
      .scroll-element {
        opacity: 0;
        transform: translateY(50px);
        transition: opacity 0.6s ease-out, transform 0.6s ease-out;
      }
      .scroll-element.visible {
        opacity: 1;
        transform: translateY(0);
      }
    `);
  }

  connectedCallback() {
    this.#initializeStyles();
    this.#render();
    this.#setupScrollObserver();
  }

  #render() {
    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];
    
    const template = document.createElement('template');
    template.innerHTML = `
      <div class="scroll-element">
        <slot></slot>
      </div>
    `;
    
    this.#shadowRoot.appendChild(template.content.cloneNode(true));
  }

  #setupScrollObserver() {
    if (!('IntersectionObserver' in window)) return;

    const threshold = parseFloat(this.getAttribute('threshold')) || 0.1;
    const rootMargin = this.getAttribute('root-margin') || '0px';
    const once = this.hasAttribute('once');

    const options = { threshold, rootMargin };
    
    this.#scrollObserver = new IntersectionObserver(
      (entries) => {
        for (const entry of entries) {
          if (entry.isIntersecting) {
            entry.target.classList.add('visible');
            
            if (once) {
              this.#scrollObserver.unobserve(entry.target);
            }
            
            this.dispatchEvent(
              new CustomEvent('scroll-visible', {
                bubbles: true,
                composed: true,
                detail: { isVisible: true }
              })
            );
          } else if (!once) {
            entry.target.classList.remove('visible');
          }
        }
      },
      options
    );

    const element = this.#shadowRoot.querySelector('.scroll-element');
    if (element) {
      this.#scrollObserver.observe(element);
    }
  }

  disconnectedCallback() {
    this.#scrollObserver?.disconnect();
  }
}

class GestureAnimationComponent extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #startPosition;
  #isDragging;

  static get observedAttributes() {
    return ['drag', 'swipe'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
    this.#startPosition = { x: 0, y: 0 };
    this.#isDragging = false;
  }

  #initializeStyles() {
    this.#styleSheet.replaceSync(`
      :host {
        display: block;
        touch-action: none;
        user-select: none;
      }
      .gesture-element {
        cursor: grab;
      }
      .gesture-element:active {
        cursor: grabbing;
      }
    `);
  }

  connectedCallback() {
    this.#initializeStyles();
    this.#render();
    this.#setupGestures();
  }

  #render() {
    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];
    
    const template = document.createElement('template');
    template.innerHTML = `
      <div class="gesture-element">
        <slot></slot>
      </div>
    `;
    
    this.#shadowRoot.appendChild(template.content.cloneNode(true));
  }

  #setupGestures() {
    const element = this.#shadowRoot.querySelector('.gesture-element');
    if (!element) return;

    if (this.hasAttribute('drag')) {
      element.addEventListener('pointerdown', this.#handleDragStart.bind(this));
      element.addEventListener('pointermove', this.#handleDragMove.bind(this));
      element.addEventListener('pointerup', this.#handleDragEnd.bind(this));
      element.addEventListener('pointercancel', this.#handleDragEnd.bind(this));
    }

    if (this.hasAttribute('swipe')) {
      element.addEventListener('pointerdown', this.#handleSwipeStart.bind(this));
      element.addEventListener('pointerup', this.#handleSwipeEnd.bind(this));
    }
  }

  #handleDragStart(event) {
    this.#isDragging = true;
    this.#startPosition = { x: event.clientX, y: event.clientY };
    
    this.dispatchEvent(
      new CustomEvent('drag-start', {
        bubbles: true,
        composed: true,
        detail: { position: this.#startPosition }
      })
    );
  }

  #handleDragMove(event) {
    if (!this.#isDragging) return;
    
    const currentPosition = { x: event.clientX, y: event.clientY };
    const delta = {
      x: currentPosition.x - this.#startPosition.x,
      y: currentPosition.y - this.#startPosition.y
    };

    this.dispatchEvent(
      new CustomEvent('drag-move', {
        bubbles: true,
        composed: true,
        detail: { position: currentPosition, delta }
      })
    );
  }

  #handleDragEnd(event) {
    if (!this.#isDragging) return;
    this.#isDragging = false;

    const endPosition = { x: event.clientX, y: event.clientY };
    
    this.dispatchEvent(
      new CustomEvent('drag-end', {
        bubbles: true,
        composed: true,
        detail: { position: endPosition }
      })
    );
  }

  #handleSwipeStart(event) {
    this.#startPosition = { x: event.clientX, y: event.clientY };
  }

  #handleSwipeEnd(event) {
    const endPosition = { x: event.clientX, y: event.clientY };
    const delta = {
      x: endPosition.x - this.#startPosition.x,
      y: endPosition.y - this.#startPosition.y
    };

    const threshold = 50;
    let direction = null;

    if (Math.abs(delta.x) > Math.abs(delta.y)) {
      if (delta.x > threshold) direction = 'right';
      else if (delta.x < -threshold) direction = 'left';
    } else {
      if (delta.y > threshold) direction = 'down';
      else if (delta.y < -threshold) direction = 'up';
    }

    if (direction) {
      this.dispatchEvent(
        new CustomEvent('swipe', {
          bubbles: true,
          composed: true,
          detail: { direction, delta }
        })
      );
    }
  }
}

window.customElements.define('animated-card', AnimatedCard);
window.customElements.define('transition-component', TransitionComponent);
window.customElements.define('scroll-animation', ScrollAnimationComponent);
window.customElements.define('gesture-animation', GestureAnimationComponent);

export { AnimatedCard, TransitionComponent, ScrollAnimationComponent, GestureAnimationComponent, ANIMATION_PRESETS };