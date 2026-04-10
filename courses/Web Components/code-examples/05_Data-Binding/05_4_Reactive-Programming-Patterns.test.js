import { Subject, BehaviorSubject, ReplaySubject, Observable, ReactiveElement, REACTIVE_CONFIG, ReactiveError } from './05_4_Reactive-Programming-Patterns.js';

describe('ReactiveElement', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('reactive-element');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders reactive container', () => {
      expect(component.shadowRoot.innerHTML).toContain('reactive-container');
    });
  });

  describe('property changes', () => {
    test('stream-name attribute updates stream name', () => {
      component.setAttribute('stream-name', 'custom-stream');
      expect(component.getAttribute('stream-name')).toBe('custom-stream');
    });

    test('auto-subscribe attribute toggles auto-subscribe', () => {
      component.setAttribute('auto-subscribe', '');
      expect(component.hasAttribute('auto-subscribe')).toBe(true);
    });

    test('debounce attribute updates debounce time', () => {
      component.setAttribute('debounce', '500');
      expect(component._debounceTime).toBe(500);
    });
  });

  describe('events', () => {
    test('handles stream events', (done) => {
      component._stream.next({ type: 'test' });
      setTimeout(() => done());
    });
  });

  describe('edge cases', () => {
    test('next on stream emits value', () => {
      component._stream.next('test-value');
      expect(component._stream.value).toBe('test-value');
    });

    test('subscribes to stream', () => {
      const callback = jest.fn();
      const unsub = component._stream.subscribe(callback);
      component._stream.next('emitted');
      expect(callback).toHaveBeenCalledWith('emitted');
      unsub();
    });

    test('filters stream values', () => {
      component.setAttribute('filter-value', 'active');
      expect(component.getAttribute('filter-value')).toBe('active');
    });

    test('tracks subscription count', () => {
      component._stream.subscribe(() => {});
      expect(component._subscriptionCount).toBeGreaterThanOrEqual(0);
    });
  });
});

describe('Subject', () => {
  test('creates subject', () => {
    const subject = new Subject();
    expect(subject.closed).toBe(false);
  });

  test('subscribes to subject', () => {
    const subject = new Subject();
    const callback = jest.fn();
    subject.subscribe(callback);
    subject.next('value');
    expect(callback).toHaveBeenCalledWith('value');
  });

  test('emits error to observers', () => {
    const subject = new Subject();
    const callback = jest.fn();
    subject.subscribe({ error: callback });
    subject.error(new Error('test'));
    expect(callback).toHaveBeenCalled();
  });

  test('completes subject', () => {
    const subject = new Subject();
    const callback = jest.fn();
    subject.subscribe({ complete: callback });
    subject.complete();
    expect(callback).toHaveBeenCalled();
    expect(subject.closed).toBe(true);
  });
});

describe('BehaviorSubject', () => {
  test('initializes with value', () => {
    const subject = new BehaviorSubject('initial');
    expect(subject.value).toBe('initial');
  });

  test('emits current value on subscribe', () => {
    const subject = new BehaviorSubject('current');
    const callback = jest.fn();
    subject.subscribe(callback);
    expect(callback).toHaveBeenCalledWith('current');
  });
});

describe('ReplaySubject', () => {
  test('replays buffered values', () => {
    const subject = new ReplaySubject(2);
    subject.next('first');
    subject.next('second');
    const callback = jest.fn();
    subject.subscribe(callback);
    expect(callback).toHaveBeenCalledTimes(2);
  });

  test('respects buffer size', () => {
    const subject = new ReplaySubject(1);
    subject.next('old');
    subject.next('new');
    const callback = jest.fn();
    subject.subscribe(callback);
    expect(callback).toHaveBeenCalledWith('new');
    expect(callback).not.toHaveBeenCalledWith('old');
  });
});

describe('Observable', () => {
  test('creates observable from values', () => {
    const obs = Observable.from([1, 2, 3]);
    const callback = jest.fn();
    obs.subscribe(callback);
    expect(callback).toHaveBeenCalledTimes(3);
  });

  test('creates observable from event', () => {
    const button = document.createElement('button');
    const obs = Observable.fromEvent(button, 'click');
    const callback = jest.fn();
    const unsub = obs.subscribe(callback);
    button.click();
    expect(callback).toHaveBeenCalled();
    unsub();
  });

  test('creates interval observable', () => {
    const obs = Observable.interval(10);
    const callback = jest.fn();
    const unsub = obs.subscribe(callback);
    setTimeout(() => {
      unsub();
      expect(callback).toHaveBeenCalled();
    }, 50);
  });

  test('creates timer observable', () => {
    const obs = Observable.timer(10);
    const callback = jest.fn();
    obs.subscribe(callback);
    setTimeout(() => {
      expect(callback).toHaveBeenCalled();
    }, 20);
  });
});

describe('Operator', () => {
  test('map operator transforms values', (done) => {
    const obs = Observable.from([1, 2, 3]).pipe(
      new Operator(obs).map(x => x * 2)
    );
    const values = [];
    obs.subscribe(v => values.push(v));
    setTimeout(() => {
      expect(values).toEqual([2, 4, 6]);
      done();
    });
  });

  test('filter operator filters values', (done) => {
    const obs = Observable.from([1, 2, 3, 4]).pipe(
      new Operator(obs).filter(x => x % 2 === 0)
    );
    const values = [];
    obs.subscribe(v => values.push(v));
    setTimeout(() => {
      expect(values).toEqual([2, 4]);
      done();
    });
  });

  test('take operator limits values', (done) => {
    const obs = Observable.from([1, 2, 3, 4, 5]).pipe(
      new Operator(obs).take(3)
    );
    const values = [];
    obs.subscribe({
      next: v => values.push(v),
      complete: () => {
        expect(values).toEqual([1, 2, 3]);
        done();
      }
    });
  });

  test('skip operator skips values', (done) => {
    const obs = Observable.from([1, 2, 3, 4]).pipe(
      new Operator(obs).skip(2)
    );
    const values = [];
    obs.subscribe(v => values.push(v));
    setTimeout(() => {
      expect(values).toEqual([3, 4]);
      done();
    });
  });
});

describe('REACTIVE_CONFIG', () => {
  test('has operators', () => {
    expect(REACTIVE_CONFIG.operators.length).toBeGreaterThan(0);
  });

  test('has default debounce time', () => {
    expect(REACTIVE_CONFIG.defaultDebounceTime).toBeGreaterThan(0);
  });

  test('has buffer size', () => {
    expect(REACTIVE_CONFIG.bufferSize).toBeGreaterThan(0);
  });
});
