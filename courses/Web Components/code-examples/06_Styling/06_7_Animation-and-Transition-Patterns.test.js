import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/06_Styling/06_7_Animation-and-Transition-Patterns.js';

describe('06_7_Animation-and-Transition-Patterns', () => {
  describe('AnimatedCard', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<animated-card></animated-card>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders card elements', () => {
      const card = el.shadowRoot.querySelector('.card');
      const title = el.shadowRoot.querySelector('.title');
      const content = el.shadowRoot.querySelector('.content');
      expect(card).to.exist;
      expect(title).to.exist;
      expect(content).to.exist;
    });

    it('applies animation attribute', async () => {
      el.setAttribute('animation', 'slideInUp');
      await el.updateComplete;
      expect(el.getAttribute('animation')).to.equal('slideInUp');
    });

    it('applies duration attribute', async () => {
      el.setAttribute('duration', '500');
      await el.updateComplete;
      expect(el.getAttribute('duration')).to.equal('500');
    });

    it('applies easing attribute', async () => {
      el.setAttribute('easing', 'linear');
      await el.updateComplete;
      expect(el.getAttribute('easing')).to.equal('linear');
    });

    it('applies delay attribute', async () => {
      el.setAttribute('delay', '100');
      await el.updateComplete;
      expect(el.getAttribute('delay')).to.equal('100');
    });

    it('applies loop attribute', async () => {
      el.setAttribute('loop', '');
      await el.updateComplete;
      expect(el.hasAttribute('loop')).to.be.true;
    });

    it('applies fill attribute', async () => {
      el.setAttribute('fill', 'both');
      await el.updateComplete;
      expect(el.getAttribute('fill')).to.equal('both');
    });

    it('gets animation name', () => {
      expect(el.animationName).to.equal('fadeIn');
    });

    it('sets animation name via property', async () => {
      el.animationName = 'scaleIn';
      await el.updateComplete;
      expect(el.animationName).to.equal('scaleIn');
    });

    it('plays animation', async () => {
      const animation = el.play();
      expect(animation).to.be.instanceOf(Promise);
    });

    it('pauses animation', () => {
      el.play();
      el.pause();
    });

    it('cancels animation', () => {
      el.play();
      el.cancel();
    });

    it('handles different animation presets', async () => {
      const animations = ['fadeIn', 'fadeOut', 'slideInLeft', 'slideInRight', 'slideInUp', 'slideInDown', 'scaleIn', 'scaleOut', 'bounceIn', 'flipIn'];
      for (const anim of animations) {
        el.setAttribute('animation', anim);
        await el.updateComplete;
        expect(el.animationName).to.equal(anim);
      }
    });
  });

  describe('TransitionComponent', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<transition-component>Content</transition-component>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders transition element', () => {
      const element = el.shadowRoot.querySelector('.transition-element');
      expect(element).to.exist;
    });

    it('applies transition attribute', async () => {
      el.setAttribute('transition', 'transform');
      await el.updateComplete;
      expect(el.getAttribute('transition')).to.equal('transform');
    });

    it('applies duration attribute', async () => {
      el.setAttribute('duration', '500');
      await el.updateComplete;
      expect(el.getAttribute('duration')).to.equal('500');
    });

    it('applies easing attribute', async () => {
      el.setAttribute('easing', 'linear');
      await el.updateComplete;
      expect(el.getAttribute('easing')).to.equal('linear');
    });

    it('transitions to new properties', async () => {
      const transition = el.transitionTo({ opacity: 0.5, transform: 'scale(1.1)' }, { duration: 100 });
      expect(transition).to.be.instanceOf(Promise);
    });

    it('sets hover state', () => {
      el.setHover(true);
      expect(el.hasAttribute('hover')).to.be.true;
      el.setHover(false);
      expect(el.hasAttribute('hover')).to.be.false;
    });
  });

  describe('ScrollAnimationComponent', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<scroll-animation>Scroll Content</scroll-animation>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders scroll element', () => {
      const element = el.shadowRoot.querySelector('.scroll-element');
      expect(element).to.exist;
    });

    it('applies threshold attribute', async () => {
      el.setAttribute('threshold', '0.5');
      await el.updateComplete;
      expect(el.getAttribute('threshold')).to.equal('0.5');
    });

    it('applies root-margin attribute', async () => {
      el.setAttribute('root-margin', '100px');
      await el.updateComplete;
      expect(el.getAttribute('root-margin')).to.equal('100px');
    });

    it('applies once attribute', async () => {
      el.setAttribute('once', '');
      await el.updateComplete;
      expect(el.hasAttribute('once')).to.be.true;
    });

    it('dispatches scroll-visible event', async () => {
      const listener = oneEvent(el, 'scroll-visible');
      el.setAttribute('once', '');
      await el.updateComplete;
      await listener;
    });
  });

  describe('GestureAnimationComponent', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<gesture-animation>Gesture Content</gesture-animation>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders gesture element', () => {
      const element = el.shadowRoot.querySelector('.gesture-element');
      expect(element).to.exist;
    });

    it('applies drag attribute', async () => {
      el.setAttribute('drag', '');
      await el.updateComplete;
      expect(el.hasAttribute('drag')).to.be.true;
    });

    it('applies swipe attribute', async () => {
      el.setAttribute('swipe', '');
      await el.updateComplete;
      expect(el.hasAttribute('swipe')).to.be.true;
    });

    it('dispatches drag-start event', async () => {
      el.setAttribute('drag', '');
      const element = el.shadowRoot.querySelector('.gesture-element');
      const listener = oneEvent(el, 'drag-start');
      element.dispatchEvent(new PointerEvent('pointerdown', { clientX: 100, clientY: 100 }));
      const event = await listener;
      expect(event.detail).to.have.property('position');
    });
  });

  describe('ANIMATION_PRESETS', () => {
    it('contains fadeIn preset', () => {
      expect(ANIMATION_PRESETS.fadeIn).to.exist;
      expect(ANIMATION_PRESETS.fadeIn.from).to.have.property('opacity');
    });

    it('contains slideIn presets', () => {
      expect(ANIMATION_PRESETS.slideInLeft).to.exist;
      expect(ANIMATION_PRESETS.slideInRight).to.exist;
      expect(ANIMATION_PRESETS.slideInUp).to.exist;
      expect(ANIMATION_PRESETS.slideInDown).to.exist;
    });

    it('contains scaleIn preset', () => {
      expect(ANIMATION_PRESETS.scaleIn).to.exist;
      expect(ANIMATION_PRESETS.scaleIn.to).to.have.property('transform');
    });

    it('contains bounceIn preset', () => {
      expect(ANIMATION_PRESETS.bounceIn).to.exist;
    });

    it('contains flipIn preset', () => {
      expect(ANIMATION_PRESETS.flipIn).to.exist;
    });
  });

  describe('Edge Cases', () => {
    it('handles animation without animation attribute', () => {
      const el = document.createElement('animated-card');
      expect(el.animationName).to.equal('fadeIn');
    });

    it('handles transition with default attributes', () => {
      const el = document.createElement('transition-component');
      expect(el.shadowRoot).to.exist;
    });

    it('handles empty threshold', async () => {
      const el = await fixture(html`<scroll-animation threshold=""></scroll-animation>`);
      expect(el.getAttribute('threshold')).to.equal('');
    });
  });
});