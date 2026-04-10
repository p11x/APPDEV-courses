import { DataPipelineElement, PipelineNode, SourceNode, TransformNode, FilterNode, DATA_FLOW_CONFIG, DataFlowError } from './05_3_Data-Flow-Architecture.js';

describe('DataPipelineElement', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('data-pipeline-element');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders pipeline visual', () => {
      expect(component.shadowRoot.innerHTML).toContain('pipeline-visual');
    });

    test('renders data preview', () => {
      expect(component.shadowRoot.innerHTML).toContain('data-preview');
    });

    test('renders flow controls', () => {
      const buttons = component.shadowRoot.querySelectorAll('.flow-btn');
      expect(buttons.length).toBe(3);
    });
  });

  describe('property changes', () => {
    test('source attribute updates source', () => {
      component.setAttribute('source', 'api');
      expect(component.getAttribute('source')).toBe('api');
    });

    test('mode attribute updates mode', () => {
      component.setAttribute('mode', 'batch');
      expect(component.getAttribute('mode')).toBe('batch');
    });

    test('buffer-size attribute updates buffer', () => {
      component.setAttribute('buffer-size', '20');
      expect(component._bufferSize).toBe(20);
    });
  });

  describe('events', () => {
    test('logs flow on connect', (done) => {
      component.addEventListener('component-connected', () => {
        expect(component._flowHistory.length).toBeGreaterThan(0);
        done();
      });
    });
  });

  describe('edge cases', () => {
    test('pushData adds data to pipeline', async () => {
      await component.pushData({ id: 'test-1', value: 100 });
      expect(component._source.dataBuffer.length).toBe(1);
    });

    test('processData processes pipeline', async () => {
      component.pushData({ id: 'test-2', value: 50 });
      await component.processData();
      expect(component._processing).toBe(false);
    });

    test('clearPipeline clears all nodes', () => {
      component.pushData({ id: 'clear-test', value: 10 });
      component.clearPipeline();
      expect(component._currentData).toBeNull();
    });

    test('subscribe adds subscriber', () => {
      const callback = jest.fn();
      const unsubscribe = component.subscribe(callback);
      expect(component._subscribers.has(callback)).toBe(true);
      unsubscribe();
      expect(component._subscribers.has(callback)).toBe(false);
    });

    test('source getter and setter', () => {
      component.source = 'test-source';
      expect(component.source).toBe('test-source');
    });

    test('mode getter and setter', () => {
      component.mode = 'batch';
      expect(component.mode).toBe('batch');
    });

    test('logs flow history', () => {
      component._logFlow('test', 'Test message');
      expect(component._flowHistory.length).toBe(1);
    });
  });
});

describe('PipelineNode', () => {
  test('creates pipeline node', () => {
    const node = new PipelineNode({ id: 'test-node', type: 'test' });
    expect(node.id).toBe('test-node');
    expect(node.type).toBe('test');
  });

  test('connects nodes', () => {
    const node1 = new PipelineNode({ id: 'node1' });
    const node2 = new PipelineNode({ id: 'node2' });
    const result = node1.connect(node2);
    expect(result).toBe(node2);
    expect(node1.next).toBe(node2);
  });

  test('processes data', async () => {
    const node = new PipelineNode({
      id: 'process-test',
      transform: (data) => data.value * 2
    });
    const result = await node.process({ value: 5 });
    expect(result.value).toBe(10);
  });

  test('filters data', async () => {
    const node = new PipelineNode({
      id: 'filter-test',
      filter: (data) => data.value > 10
    });
    const result = await node.process({ value: 20 });
    expect(result).toBeTruthy();
  });

  test('pushes to buffer', () => {
    const node = new PipelineNode({ id: 'push-test' });
    node.push({ data: 'test' });
    expect(node.dataBuffer.length).toBe(1);
  });

  test('pop from buffer', () => {
    const node = new PipelineNode({ id: 'pop-test' });
    node.push({ data: 'test' });
    const data = node.pop();
    expect(data.data).toBe('test');
  });

  test('flushes buffer', () => {
    const node = new PipelineNode({ id: 'flush-test' });
    node.push({ data: '1' });
    node.push({ data: '2' });
    const flushed = node.flush();
    expect(flushed.length).toBe(2);
    expect(node.dataBuffer.length).toBe(0);
  });
});

describe('SourceNode', () => {
  test('creates source node', () => {
    const node = new SourceNode({ id: 'source' });
    expect(node.type).toBe('source');
  });

  test('fetches data', async () => {
    const node = new SourceNode({
      id: 'fetch-test',
      fetchFn: async () => ({ fetched: true })
    });
    await node.fetch();
    expect(node.dataBuffer.length).toBe(1);
  });

  test('starts and stops polling', () => {
    const node = new SourceNode({ id: 'poll-test' });
    node.startPolling(100);
    expect(node.pollingEnabled).toBe(true);
    node.stopPolling();
    expect(node.pollingEnabled).toBe(false);
  });
});

describe('TransformNode', () => {
  test('creates transform node', () => {
    const node = new TransformNode({ id: 'transform' });
    expect(node.type).toBe('transform');
  });

  test('adds transformations', () => {
    const node = new TransformNode({ id: 'add-transform' });
    node.addTransformation((data) => data.value + 1);
    expect(node.transformations.length).toBe(2);
  });
});

describe('FilterNode', () => {
  test('creates filter node', () => {
    const node = new FilterNode({ id: 'filter' });
    expect(node.type).toBe('filter');
  });

  test('adds filters', () => {
    const node = new FilterNode({ id: 'add-filter' });
    node.addFilter((data) => data.active);
    expect(node.filters.length).toBe(2);
  });
});

describe('DATA_FLOW_CONFIG', () => {
  test('has pipeline stages', () => {
    expect(DATA_FLOW_CONFIG.pipelineStages.length).toBeGreaterThan(0);
  });

  test('has default buffer size', () => {
    expect(DATA_FLOW_CONFIG.defaultBufferSize).toBeGreaterThan(0);
  });
});
