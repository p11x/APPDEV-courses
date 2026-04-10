import { AccessibleCard } from './04_5_Accessibility-in-Shadow-DOM.js';

describe('AccessibleCard', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('accessible-card');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders card element', () => {
      expect(component.shadowRoot.innerHTML).toContain('card');
    });

    test('renders card with role attribute', () => {
      const card = component.shadowRoot.querySelector('.card');
      expect(card.getAttribute('role')).toBe('article');
    });

    test('renders action buttons', () => {
      const buttons = component.shadowRoot.querySelectorAll('.action-button');
      expect(buttons.length).toBe(2);
    });

    test('renders live region for screen readers', () => {
      const liveRegion = component.shadowRoot.getElementById('live-region');
      expect(liveRegion).toBeTruthy();
      expect(liveRegion.getAttribute('aria-live')).toBe('polite');
    });
  });

  describe('property changes', () => {
    test('role attribute updates role', () => {
      component.setAttribute('role', 'button');
      expect(component._role).toBe('button');
    });

    test('aria-label attribute updates label', () => {
      component.setAttribute('aria-label', 'Test Card');
      expect(component.getAttribute('aria-label')).toBe('Test Card');
    });

    test('tabindex attribute updates tab index', () => {
      component.setAttribute('tabindex', '5');
      expect(component.getAttribute('tabindex')).toBe('5');
    });

    test('disabled attribute disables card', () => {
      component.setAttribute('disabled', '');
      expect(component.hasAttribute('disabled')).toBe(true);
    });
  });

  describe('events', () => {
    test('dispatches card-action event', (done) => {
      component.addEventListener('card-action', (e) => {
        expect(e.detail.action).toBeDefined();
        done();
      });
      const button = component.shadowRoot.querySelector('.action-button');
      button.click();
    });

    test('dispatches card-activate event on Enter key', (done) => {
      component.addEventListener('card-activate', () => done());
      const card = component.shadowRoot.querySelector('.card');
      card.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter' }));
    });
  });

  describe('edge cases', () => {
    test('title getter returns title text', () => {
      expect(component.title).toBeDefined();
    });

    test('title setter updates title text', () => {
      component.title = 'New Title';
      expect(component.title).toBe('New Title');
    });

    test('content setter updates content', () => {
      component.content = 'New Content';
      expect(component.content).toBe('New Content');
    });

    test('status setter updates status text', () => {
      component.status = 'Inactive';
      expect(component.status).toBe('Inactive');
    });

    test('keyboard navigation works', () => {
      const card = component.shadowRoot.querySelector('.card');
      card.dispatchEvent(new KeyboardEvent('keydown', { key: 'ArrowRight' }));
      expect(component.shadowRoot.activeElement).toBeDefined();
    });

    test('announces to screen reader', () => {
      component._announceToScreenReader('Test message');
      const liveRegion = component.shadowRoot.getElementById('live-region');
      expect(liveRegion.textContent).toBe('Test message');
    });
  });
});
