/**
 * Advanced Animation Frameworks - GSAP and Framer Motion Integration
 * @description Integrate powerful animation libraries like GSAP and custom animation systems with Web Components
 * @module advanced/animation-frameworks
 * @version 1.0.0
 */

(function() {
  'use strict';

  const AnimationPresets = {
    fadeIn: { opacity: { from: 0, to: 1 }, duration: 0.3 },
    fadeOut: { opacity: { from: 1, to: 0 }, duration: 0.3 },
    slideInUp: { transform: { from: 'translateY(20px)', to: 'translateY(0)' }, opacity: { from: 0, to: 1 }, duration: 0.4 },
    slideInDown: { transform: { from: 'translateY(-20px)', to: 'translateY(0)' }, opacity: { from: 0, to: 1 }, duration: 0.4 },
    slideInLeft: { transform: { from: 'translateX(-20px)', to: 'translateX(0)' }, opacity: { from: 0, to: 1 }, duration: 0.4 },
    slideInRight: { transform: { from: 'translateX(20px)', to: 'translateX(0)' }, opacity: { from: 0, to: 1 }, duration: 0.4 },
    scaleIn: { transform: { from: 'scale(0.8)', to: 'scale(1)' }, opacity: { from: 0, to: 1 }, duration: 0.3 },
    scaleOut: { transform: { from: 'scale(1)', to: 'scale(0.8)' }, opacity: { from: 1, to: 0 }, duration: 0.3 },
    bounce: { transform: { from: 'scale(0.3)', to: 'scale(1)' }, keyframes: [0, 0.2, 0.4, 0.6, 0.8, 1], duration: 0.6 },
    shake: { transform: { from: 'translateX(0)', to: 'translateX(-10px)' }, keyframes: [0, -10, 10, -10, 10, -5, 5, 0], duration: 0.5 },
    pulse: { transform: { from: 'scale(1)', to: 'scale(1.05)' }, duration: 0.15, repeat: 1, yoyo: true },
    spin: { transform: { from: 'rotate(0deg)', to: 'rotate(360deg)' }, duration: 0.6, repeat: 1 }
  };

  class AnimationController {
    constructor(element, options = {}) {
      this.element = element;
      this.isAnimating = false;
      this.currentAnimation = null;
      this.animationQueue = [];
      this.isPaused = false;
      this._progress = 0;
      this._duration = options.duration || 300;
      this._easing = options.easing || this.easeOutCubic;
      this._onStart = options.onStart || null;
      this._onComplete = options.onComplete || null;
      this._onUpdate = options.onUpdate || null;
    }

    easeOutCubic(t) {
      return 1 - Math.pow(1 - t, 3);
    }

    easeInOutQuad(t) {
      return t < 0.5 ? 2 * t * t : 1 - Math.pow(-2 * t + 2, 2) / 2;
    }

    easeOutElastic(t) {
      const c4 = (2 * Math.PI) / 3;
      return t === 0 ? 0 : t === 1 ? 1 : Math.pow(2, -10 * t) * Math.sin((t * 10 - 0.75) * c4) + 1;
    }

    animate(properties, options = {}) {
      return new Promise((resolve) => {
        if (!this.element) {
          resolve();
          return;
        }

        const startTime = performance.now();
        const duration = options.duration || this._duration;
        const easing = options.easing || this._easing;
        const startProps = {};
        const targetProps = {};

        for (const [key, value] of Object.entries(properties)) {
          const currentValue = this.getPropertyValue(key);
          startProps[key] = currentValue;
          targetProps[key] = value;
        }

        const animateFrame = (currentTime) => {
          if (this.isPaused) {
            requestAnimationFrame(animateFrame);
            return;
          }

          const elapsed = currentTime - startTime;
          const progress = Math.min(elapsed / duration, 1);
          const easedProgress = easing(progress);

          for (const [key, targetValue] of Object.entries(targetProps)) {
            const startValue = startProps[key];
            const currentValue = startValue + (targetValue - startValue) * easedProgress;
            this.setPropertyValue(key, currentValue);
          }

          if (options.onUpdate) {
            options.onUpdate(easedProgress);
          }

          this._progress = easedProgress;

          if (progress < 1) {
            this.currentAnimation = requestAnimationFrame(animateFrame);
          } else {
            this.isAnimating = false;
            this.currentAnimation = null;
            if (options.onComplete) {
              options.onComplete();
            }
            resolve();
            this.processQueue();
          }
        };

        this.isAnimating = true;
        if (options.onStart) {
          options.onStart();
        }
        this.currentAnimation = requestAnimationFrame(animateFrame);
      });
    }

    getPropertyValue(property) {
      const computedStyle = window.getComputedStyle(this.element);
      if (property === 'opacity') return parseFloat(computedStyle.opacity) || 0;
      if (property === 'scale') {
        const transform = computedStyle.transform;
        if (transform && transform !== 'none') {
          const match = transform.match(/matrix\(([^)]+)\)/);
          if (match) {
            const values = match[1].split(', ');
            return parseFloat(values[0]) || 1;
          }
        }
        return 1;
      }
      if (property.includes('translate')) {
        const transform = computedStyle.transform;
        if (transform && transform !== 'none') {
          const match = transform.match(/matrix\(([^)]+)\)/);
          if (match) {
            const values = match[1].split(', ');
            if (property === 'translateX') return parseFloat(values[4]) || 0;
            if (property === 'translateY') return parseFloat(values[5]) || 0;
          }
        }
        return 0;
      }
      if (property === 'rotate') {
        const transform = computedStyle.transform;
        if (transform && transform !== 'none') {
          const match = transform.match(/rotate\(([^)]+)deg\)/);
          if (match) return parseFloat(match[1]) || 0;
        }
        return 0;
      }
      return 0;
    }

    setPropertyValue(property, value) {
      if (property === 'opacity') {
        this.element.style.opacity = value;
      } else if (property === 'scale') {
        this.element.style.transform = `scale(${value})`;
      } else if (property === 'translateX') {
        this.element.style.transform = `translateX(${value}px)`;
      } else if (property === 'translateY') {
        this.element.style.transform = `translateY(${value}px)`;
      } else if (property === 'rotate') {
        this.element.style.transform = `rotate(${value}deg)`;
      } else if (property.includes('translate') && property.includes('X') && property.includes('Y')) {
        this.element.style.transform = `translate(${value.x}px, ${value.y}px)`;
      }
    }

    play(preset, options = {}) {
      const animation = AnimationPresets[preset];
      if (!animation) {
        console.warn(`Animation preset "${preset}" not found`);
        return Promise.resolve();
      }

      const properties = {};
      for (const [key, config] of Object.entries(animation)) {
        if (typeof config === 'object' && 'from' in config && 'to' in config) {
          properties[key] = config.to;
        }
      }

      return this.animate(properties, {
        duration: (animation.duration || 0.3) * 1000,
        ...options
      });
    }

    pause() {
      this.isPaused = true;
    }

    resume() {
      this.isPaused = false;
    }

    stop() {
      if (this.currentAnimation) {
        cancelAnimationFrame(this.currentAnimation);
        this.currentAnimation = null;
      }
      this.isAnimating = false;
      this.isPaused = false;
      this.animationQueue = [];
    }

    reverse() {
      if (this._reverseProps) {
        return this.animate(this._reverseProps);
      }
    }

    queue(animation, options = {}) {
      this.animationQueue.push({ animation, options });
      if (!this.isAnimating) {
        this.processQueue();
      }
    }

    processQueue() {
      if (this.animationQueue.length === 0) return;

      const { animation, options } = this.animationQueue.shift();
      if (animation instanceof AnimationController) {
        animation.animate({}, options).then(() => this.processQueue());
      } else if (typeof animation === 'string') {
        this.play(animation, options).then(() => this.processQueue());
      }
    }

    get progress() {
      return this._progress;
    }
  }

  class GSAPAnimationBridge {
    constructor(element) {
      this.element = element;
      this.tweens = [];
    }

    createTween(properties, options = {}) {
      const tween = {
        element: this.element,
        properties: { ...properties },
        options: {
          duration: (options.duration || 0.3) * 1000,
          ease: options.ease || 'power2.out',
          delay: options.delay || 0,
          repeat: options.repeat || 0,
          yoyo: options.yoyo || false,
          onStart: options.onStart || null,
          onUpdate: options.onUpdate || null,
          onComplete: options.onComplete || null
        },
        isPlaying: false,
        progress: 0
      };

      return tween;
    }

    to(properties, options = {}) {
      return this.createTween(properties, options);
    }

    from(properties, options = {}) {
      const fromProps = {};
      for (const [key, value] of Object.entries(properties)) {
        const currentValue = this.getCurrentValue(key);
        fromProps[key] = { from: currentValue, to: value };
      }
      return this.createTween(fromProps, options);
    }

    getCurrentValue(property) {
      const computedStyle = window.getComputedStyle(this.element);
      if (property === 'opacity') return parseFloat(computedStyle.opacity);
      if (property === 'x' || property === 'y') {
        const transform = computedStyle.transform;
        return 0;
      }
      return 0;
    }

    timeline(options = {}) {
      return new GSAPTimeline(this.element, options);
    }

    stagger(delay, options = {}) {
      const elements = this.element.querySelectorAll(options.selector || '*');
      const tweens = [];

      elements.forEach((el, index) => {
        const tween = this.createTween(options.properties || {}, {
          ...options,
          delay: index * delay
        });
        tweens.push(tween);
      });

      return tweens;
    }
  }

  class GSAPTimeline {
    constructor(element, options = {}) {
      this.element = element;
      this.tweens = [];
      this.duration = 0;
      this.options = options;
    }

    add(tween, position = null) {
      if (position !== null) {
        tween.position = position;
      }
      this.tweens.push(tween);
      this.duration = Math.max(this.duration, position || 0);
    }

    play() {
      this.tweens.forEach(tween => {
        if (tween.play) tween.play();
      });
    }
  }

  class AnimatedWebComponent extends HTMLElement {
    static get observedAttributes() {
      return ['animation', 'animated'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._animationController = null;
      this._animationName = '';
      this._isAnimated = false;
    }

    static get observedAttributes() {
      return ['animation', 'animated', 'delay', 'duration'];
    }

    connectedCallback() {
      this._animationName = this.getAttribute('animation') || 'fadeIn';
      this._isAnimated = this.hasAttribute('animated');
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'animation') {
          this._animationName = newValue;
        } else if (name === 'animated') {
          this._isAnimated = this.hasAttribute('animated');
        }
      }
    }

    get animationController() {
      return this._animationController;
    }

    animate(preset, options = {}) {
      if (!this._animationController) {
        this._animationController = new AnimationController(this, {
          duration: options.duration || 300,
          easing: options.easing
        });
      }

      return this._animationController.play(preset, {
        onStart: () => {
          this.dispatchEvent(new CustomEvent('animation-start', { bubbles: true, composed: true }));
        },
        onComplete: () => {
          this.dispatchEvent(new CustomEvent('animation-complete', { bubbles: true, composed: true }));
        },
        ...options
      });
    }

    animateTo(properties, options = {}) {
      if (!this._animationController) {
        this._animationController = new AnimationController(this, options);
      }
      return this._animationController.animate(properties, options);
    }

    fadeIn(options = {}) {
      return this.animate('fadeIn', options);
    }

    fadeOut(options = {}) {
      return this.animate('fadeOut', options);
    }

    slideIn(direction = 'up', options = {}) {
      const preset = `slideIn${direction.charAt(0).toUpperCase() + direction.slice(1)}`;
      return this.animate(preset, options);
    }

    scaleIn(options = {}) {
      return this.animate('scaleIn', options);
    }

    bounce(options = {}) {
      return this.animate('bounce', options);
    }

    shake(options = {}) {
      return this.animate('shake', options);
    }

    pulse(options = {}) {
      return this.animate('pulse', options);
    }

    spin(options = {}) {
      return this.animate('spin', options);
    }

    render() {
      const delay = parseInt(this.getAttribute('delay') || '0', 10);
      const duration = parseInt(this.getAttribute('duration') || '300', 10);

      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
            contain: layout;
          }
          .animated-container {
            contain: content;
          }
          .animated-container[hidden] {
            display: none;
          }
        </style>
        <div class="animated-container" ${!this._isAnimated ? 'hidden' : ''}>
          <slot></slot>
        </div>
      `;
    }

    play() {
      if (this._animationController) {
        this._animationController.isPaused = false;
      }
    }

    pause() {
      if (this._animationController) {
        this._animationController.pause();
      }
    }

    stop() {
      if (this._animationController) {
        this._animationController.stop();
      }
    }
  }

  class TransitionGroup extends HTMLElement {
    static get observedAttributes() {
      return ['name', 'duration'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._transitionName = 'fade';
      this._duration = 300;
      this._children = new Map();
    }

    static get observedAttributes() {
      return ['name', 'duration', 'leave-active'];
    }

    connectedCallback() {
      this._transitionName = this.getAttribute('name') || 'fade';
      this._duration = parseInt(this.getAttribute('duration') || '300', 10);
      this.setupMutationObserver();
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'name') {
          this._transitionName = newValue;
        } else if (name === 'duration') {
          this._duration = parseInt(newValue, 10);
        }
      }
    }

    setupMutationObserver() {
      const observer = new MutationObserver((mutations) => {
        mutations.forEach(mutation => {
          if (mutation.type === 'childList') {
            mutation.addedNodes.forEach(node => {
              if (node.nodeType === Node.ELEMENT_NODE) {
                this.handleAdd(node);
              }
            });
            mutation.removedNodes.forEach(node => {
              if (node.nodeType === Node.ELEMENT_NODE) {
                this.handleRemove(node);
              }
            });
          }
        });
      });

      observer.observe(this, { childList: true });
    }

    handleAdd(element) {
      element.classList.add(`${this._transitionName}-enter`);
      element.classList.add(`${this._transitionName}-enter-active`);

      requestAnimationFrame(() => {
        element.classList.remove(`${this._transitionName}-enter-active`);
        element.classList.add(`${this._transitionName}-enter-to`);
      }, this._duration);
    }

    handleRemove(element) {
      element.classList.add(`${this._transitionName}-leave`);
      element.classList.add(`${this._transitionName}-leave-active`);

      setTimeout(() => {
        element.classList.remove(`${this._transitionName}-leave-active`);
        element.classList.add(`${this._transitionName}-leave-to`);
      }, 16);

      setTimeout(() => {
        element.remove();
      }, this._duration);
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .${this._transitionName}-enter {
            opacity: 0;
          }
          .${this._transitionName}-enter-active {
            opacity: 1;
            transition: opacity ${this._duration}ms ease;
          }
          .${this._transitionName}-enter-to {
            opacity: 1;
          }
          .${this._transitionName}-leave {
            opacity: 1;
          }
          .${this._transitionName}-leave-active {
            opacity: 0;
            transition: opacity ${this._duration}ms ease;
          }
          .${this._transitionName}-leave-to {
            opacity: 0;
          }
        </style>
        <slot></slot>
      `;
    }
  }

  customElements.define('animated-component', AnimatedWebComponent);
  customElements.define('transition-group', TransitionGroup);

  if (typeof window !== 'undefined') {
    window.AnimationFrameworks = {
      AnimationPresets,
      AnimationController,
      GSAPAnimationBridge,
      GSAPTimeline,
      AnimatedWebComponent,
      TransitionGroup
    };
  }

  export {
    AnimationPresets,
    AnimationController,
    GSAPAnimationBridge,
    GSAPTimeline,
    AnimatedWebComponent,
    TransitionGroup
  };
})();