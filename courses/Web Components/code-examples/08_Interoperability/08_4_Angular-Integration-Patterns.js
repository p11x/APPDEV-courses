/**
 * Angular Integration Patterns - Web Components wrapped for Angular applications
 * @module interoperability/08_4_Angular-Integration-Patterns
 * @version 1.0.0
 * @example <wc-card [title]="cardTitle" (cardAccept)="onAccept($event)"></wc-card>
 */

function createAngularWebComponentModule(options = {}) {
    const {
        moduleName = 'WebComponentsModule',
        selectorPrefix = 'wc',
        declarations = []
    } = options;

    return {
        moduleName,
        selectorPrefix,
        declarations
    };
}

const WebComponentConfig = {
    ATTRIBUTE_PREFIX: 'attr',
    PROPERTY_PREFIX: 'prop',
    EVENT_PREFIX: 'on',
    CssClassBindings: {}
};

class AngularWebComponentWrapper extends HTMLElement {
    static get observedAttributes() {
        return this._observedAttributes || [];
    }

    constructor(componentRef) {
        super();
        this._componentRef = componentRef;
        this._inputProperties = new Map();
        this._outputProperties = new Map();
        this._bound = false;
    }

    static defineInputs(properties) {
        this._observedAttributes = [...(this._observedAttributes || []), ...properties];
        return this;
    }

    static defineOutputs(events) {
        return this;
    }

    connectedCallback() {
        if (!this._bound) {
            this._bindComponent();
            this._bound = true;
        }
    }

    disconnectedCallback() {
        if (this._componentRef?.destroy) {
            this._componentRef.destroy();
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (oldValue !== newValue && this._componentRef) {
            const propertyName = this._toCamelCase(name);
            if (this._inputProperties.has(name)) {
                const value = this._parseValue(newValue);
                this._componentRef.instance[propertyName] = value;
                this._componentRef.instance.checkUpdate();
            }
        }
    }

    _bindComponent() {
        // Component binding logic
    }

    _toCamelCase(str) {
        return str.replace(/-([a-z])/g, (g) => g[1].toUpperCase());
    }

    _toKebabCase(str) {
        return str.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
    }

    _parseValue(value) {
        if (value === 'true') return true;
        if (value === 'false') return false;
        if (!isNaN(value) && value !== '') return Number(value);
        return value;
    }
}

function createWebComponentInjector(component, options = {}) {
    const {
        tagName,
        inputs = [],
        outputs = [],
        changeDetection = 'OnPush'
    } = options;

    return function($injector) {
        $injector.invoke(['$compile', '$rootScope', ($compile, $rootScope) => {
            class WrappedComponent extends HTMLElement {
                static get observedAttributes() {
                    return inputs.map(i => typeof i === 'string' ? i : i.prop);
                }

                constructor() {
                    super();
                    this.attachShadow({ mode: 'open' });
                    this._scope = $rootScope.$new();
                    this._component = null;
                }

                connectedCallback() {
                    this._bootstrap();
                    this._setupEventListeners();
                }

                disconnectedCallback() {
                    this._scope.$destroy();
                }

                attributeChangedCallback(name, oldValue, newValue) {
                    if (oldValue !== newValue) {
                        const inputConfig = inputs.find(i => (typeof i === 'string' ? i : i.prop) === name);
                        if (inputConfig) {
                            const binding = typeof inputConfig === 'string' ? inputConfig : inputConfig.bind;
                            this._scope[binding] = this._parseAttributeValue(newValue, inputConfig.type);
                            this._scope.$apply();
                        }
                    }
                }

                _bootstrap() {
                    const template = document.createElement('div');
                    template.setAttribute('ng-app', '');
                    this._component = $compile(`<${tagName}></${tagName}>`)(this._scope);
                    template.appendChild(this._component[0]);
                    this.shadowRoot.appendChild(template);
                }

                _setupEventListeners() {
                    outputs.forEach(output => {
                        const eventName = typeof output === 'string' ? output : output.event;
                        const selector = typeof output === 'string' ? output : output.selector;
                        this.addEventListener(eventName, (e) => {
                            if (selector) {
                                this._scope[selector](e.detail);
                                this._scope.$apply();
                            }
                        });
                    });
                }

                _parseAttributeValue(value, type) {
                    if (type === 'boolean') return value !== 'false';
                    if (type === 'number') return Number(value);
                    if (type === 'object' || type === 'array') {
                        try { return JSON.parse(value); } catch { return value; }
                    }
                    return value;
                }
            }

            customElements.define(`${options.prefix || 'wc'}-${tagName}`, WrappedComponent);
        }]);
    };
}

const WebComponentBridge = {
    createComponent: (componentType, options = {}) => {
        const {
            selector,
            template,
            inputs = [],
            outputs = [],
            hostBindings = {}
        } = options;

        return {
            selector,
            template,
            inputs,
            outputs,
            hostBindings,
            createEmbeddedView: (context, injector) => {
                // View creation logic
            }
        };
    },

    toWebComponent: (componentClass, config) => {
        const { tagName, inputs, outputs } = config;

        class ConvertedWebComponent extends HTMLElement {
            static get observedAttributes() {
                return inputs?.map(i => i) || [];
            }

            constructor() {
                super();
                this.attachShadow({ mode: 'open' });
            }

            connectedCallback() {
                this.render();
            }

            attributeChangedCallback(name, oldValue, newValue) {
                if (oldValue !== newValue) {
                    this.dispatchEvent(new CustomEvent('attrChange', {
                        detail: { name, oldValue, newValue }
                    }));
                }
            }

            render() {
                this.shadowRoot.innerHTML = `<div>${tagName}</div>`;
            }
        }

        return ConvertedWebComponent;
    }
};

const NgElementStrategyFactory = {
    createStrategy: (component, options) => {
        return {
            connect() {},
            disconnect() {},
            getValue(name) { return null; },
            setValue(name, value) {},
            invokeMethod(name, args) {}
        };
    }
};

function createWebComponentAdapter(element, ngZone) {
    return {
        element,
        ngZone,
        getProperty(name) {
            return element[name];
        },
        setProperty(name, value) {
            element[name] = value;
        },
        callMethod(name, ...args) {
            return element[name]?.(...args);
        },
        addEventListener(name, handler) {
            element.addEventListener(name, (e) => ngZone.run(() => handler(e.detail)));
        },
        removeEventListener(name, handler) {
            element.removeEventListener(name, handler);
        },
        setAttribute(name, value) {
            if (value === null || value === undefined) {
                element.removeAttribute(name);
            } else {
                element.setAttribute(name, String(value));
            }
        },
        getAttribute(name) {
            return element.getAttribute(name);
        },
        removeAttribute(name) {
            element.removeAttribute(name);
        }
    };
}

@Injectable()
class WebComponentService {
    private defined = new Map<string, boolean>();

    constructor(private ngZone: NgZone) {}

    async whenDefined(tagName: string): Promise<CustomElementConstructor> {
        if (this.defined.has(tagName)) {
            return customElements.whenDefined(tagName);
        }
        return customElements.whenDefined(tagName);
    }

    createBridge<T extends HTMLElement>(
        tagName: string,
        options?: {
            inputs?: string[];
            outputs?: string[];
        }
    ): WebComponentBridge<T> {
        return new WebComponentBridge<T>(tagName, this.ngZone, options);
    }
}

class WebComponentBridge<T extends HTMLElement> {
    private element: T | null = null;
    private listeners = new Map<string, EventListener>();

    constructor(
        private tagName: string,
        private ngZone: NgZone,
        private options?: { inputs?: string[]; outputs?: string[] }
    ) {}

    async connect(): Promise<void> {
        await customElements.whenDefined(this.tagName);
        this.element = document.createElement(this.tagName) as T;
    }

    disconnect(): void {
        this.listeners.forEach((listener, event) => {
            this.element?.removeEventListener(event, listener);
        });
        this.listeners.clear();
        this.element = null;
    }

    setProperty(name: string, value: any): void {
        if (this.element) {
            (this.element as any)[name] = value;
        }
    }

    getProperty(name: string): any {
        return this.element?.[name as keyof T];
    }

    addListener(eventName: string, handler: (detail: any) => void): void {
        if (this.element) {
            const wrappedHandler = (e: Event) => {
                const customEvent = e as CustomEvent;
                this.ngZone.run(() => handler(customEvent.detail));
            };
            this.element.addEventListener(eventName, wrappedHandler);
            this.listeners.set(eventName, wrappedHandler);
        }
    }

    callMethod(name: string, ...args: any[]): any {
        return (this.element as any)?.[name]?.(...args);
    }
}

@Directive({
    selector: 'wc-wrapper'
})
class WcWrapperDirective implements OnInit, OnDestroy {
    @Input() wcTag!: string;
    @Input() wcProps: Record<string, any> = {};
    @Output() wcEvent = new EventEmitter<any>();

    private element: HTMLElement | null = null;

    constructor(private ngZone: NgZone, private el: ElementRef) {}

    ngOnInit(): void {
        this.createElement();
    }

    ngOnDestroy(): void {
        this.element?.remove();
    }

    private async createElement(): Promise<void> {
        await customElements.whenDefined(this.wcTag);
        this.element = document.createElement(this.wcTag);
        this.applyProps();
        this.setupEvents();
        this.el.nativeElement.appendChild(this.element);
    }

    private applyProps(): void {
        Object.entries(this.wcProps).forEach(([key, value]) => {
            if (this.element) {
                (this.element as any)[key] = value;
            }
        });
    }

    private setupEvents(): void {
        // Event setup
    }
}

@Pipe({ name: 'wcProperty' })
class WcPropertyPipe implements PipeTransform {
    transform(value: any, type: string): any {
        if (type === 'boolean') return value !== 'false';
        if (type === 'number') return Number(value);
        if (type === 'json') {
            try { return JSON.parse(value); } catch { return value; }
        }
        return value;
    }
}

export {
    createAngularWebComponentModule,
    AngularWebComponentWrapper,
    WebComponentConfig,
    WebComponentBridge,
    WebComponentService,
    WcWrapperDirective,
    WcPropertyPipe,
    createWebComponentAdapter
};
