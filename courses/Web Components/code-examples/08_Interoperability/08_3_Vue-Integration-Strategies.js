/**
 * Vue Integration Strategies - Web Components wrapped for Vue applications
 * @module interoperability/08_3_Vue-Integration-Strategies
 * @version 1.0.0
 * @example <vue-web-component v-bind="props" @card-accept="handleAccept" />
 */

const VueWebComponentPlugin = {
    install(app, options = {}) {
        const {
            tagPrefix = '',
            ignoreTags = [],
            autoDefine = true,
            shadowMode = 'open'
        } = options;

        const VueWebComponentMixin = {
            props: {
                tagName: {
                    type: String,
                    required: true
                },
                props: {
                    type: Object,
                    default: () => ({})
                },
                events: {
                    type: Object,
                    default: () => ({})
                }
            },
            emits: [],
            data() {
                return {
                    element: null,
                    eventListeners: new Map()
                };
            },
            watch: {
                props: {
                    handler(newProps) {
                        this.syncProps(newProps);
                    },
                    deep: true
                }
            },
            mounted() {
                this.$nextTick(() => {
                    this.createElement();
                    this.setupEventListeners();
                });
            },
            beforeUnmount() {
                this.removeEventListeners();
            },
            methods: {
                createElement() {
                    this.element = document.createElement(this.tagName);
                    this.syncProps(this.props);
                    this.$el.appendChild(this.element);
                },
                syncProps(props) {
                    if (!this.element) return;

                    Object.entries(props).forEach(([key, value]) => {
                        if (value === null || value === undefined) {
                            this.element.removeAttribute(key);
                        } else if (typeof value === 'boolean') {
                            if (value) {
                                this.element.setAttribute(key, '');
                            } else {
                                this.element.removeAttribute(key);
                            }
                        } else {
                            this.element.setAttribute(key, String(value));
                        }
                    });
                },
                setupEventListeners() {
                    Object.entries(this.events).forEach(([wcEvent, vueEvent]) => {
                        const handler = (detail) => {
                            this.$emit(vueEvent, detail);
                        };
                        this.element.addEventListener(wcEvent, handler);
                        this.eventListeners.set(wcEvent, handler);
                    });
                },
                removeEventListeners() {
                    this.eventListeners.forEach((handler, event) => {
                        this.element?.removeEventListener(event, handler);
                    });
                    this.eventListeners.clear();
                },
                getElement() {
                    return this.element;
                },
                callMethod(methodName, ...args) {
                    return this.element?.[methodName]?.(...args);
                }
            },
            template: '<div ref="container"></div>'
        };

        app.component('WebComponent', VueWebComponentMixin);

        app.config.globalProperties.$registerWebComponent = (tagName, component) => {
            if (!customElements.get(tagName)) {
                console.warn(`Web Component ${tagName} is not defined`);
                return false;
            }
            return true;
        };
    }
};

function defineWebComponentWrapper(tagName, vueOptions = {}) {
    const {
        props = [],
        events = {},
        emits = [],
        methods = [],
        computed = {},
        watch = {},
        onConnected = null,
        onDisconnected = null
    } = vueOptions;

    return {
        name: `${tagName}-wrapper`,
        props: props.reduce((acc, prop) => {
            acc[prop] = { type: [String, Number, Boolean, Object, Array] };
            return acc;
        }, {}),
        emits: emits,
        setup(props, { emit, expose }) {
            let element = null;
            const eventListeners = new Map();

            const propAttributes = computed(() => {
                const attrs = {};
                props.forEach(prop => {
                    if (props[prop] !== undefined) {
                        attrs[prop] = props[prop];
                    }
                });
                return attrs;
            });

            const setupElement = (el) => {
                element = el;

                Object.entries(propAttributes.value).forEach(([key, value]) => {
                    if (value !== null && value !== undefined) {
                        element.setAttribute(key, String(value));
                    }
                });

                Object.entries(events).forEach(([wcEvent, vueEvent]) => {
                    const handler = (event) => {
                        emit(vueEvent, event.detail);
                    };
                    element.addEventListener(wcEvent, handler);
                    eventListeners.set(wcEvent, handler);
                });

                if (onConnected && typeof onConnected === 'function') {
                    onConnected(element);
                }
            };

            const cleanupElement = () => {
                eventListeners.forEach((handler, event) => {
                    element?.removeEventListener(event, handler);
                });
                eventListeners.clear();

                if (onDisconnected && typeof onDisconnected === 'function') {
                    onDisconnected(element);
                }
            };

            const syncProp = (key, value) => {
                if (!element) return;
                if (value === null || value === undefined) {
                    element.removeAttribute(key);
                } else if (typeof value === 'boolean') {
                    if (value) {
                        element.setAttribute(key, '');
                    } else {
                        element.removeAttribute(key);
                    }
                } else {
                    element.setAttribute(key, String(value));
                }
            };

            methods.forEach(method => {
                expose({ [method]: (...args) => element?.[method]?.(...args) });
            });

            expose({
                getElement: () => element,
                callMethod: (name, ...args) => element?.[name]?.(...args)
            });

            return {
                setupElement,
                cleanupElement,
                syncProp,
                element
            };
        },
        template: '<div ref="setupElement" :class="$attrs.class" :style="$attrs.style"></div>',
        mounted() {
            this.setupElement(this.$refs.setupElement);
        },
        beforeUnmount() {
            this.cleanupElement();
        },
        watch: Object.entries(watch).reduce((acc, [prop, handler]) => {
            acc[prop] = (newValue, oldValue) => {
                this.syncProp(prop, newValue);
                if (typeof handler === 'function') {
                    handler(newValue, oldValue);
                }
            };
            return acc;
        }, {})
    };
}

const useWebComponent = (tagName, options = {}) => {
    const { props = [], events = {} } = options;

    const state = Vue.reactive({});
    const element = Vue.ref(null);
    const isDefined = Vue.ref(false);
    const eventListeners = new Map();

    Vue.onMounted(() => {
        customElements.whenDefined(tagName).then(() => {
            isDefined.value = true;
        });
    });

    Vue.onUnmounted(() => {
        eventListeners.forEach((handler, event) => {
            element.value?.removeEventListener(event, handler);
        });
        eventListeners.clear();
    });

    const bindProps = (el, propsToBind) => {
        Object.entries(propsToBind).forEach(([key, value]) => {
            if (value !== null && value !== undefined) {
                el.setAttribute(key, String(value));
            }
        });
    };

    const bindEvents = (el, eventsMap) => {
        Object.entries(eventsMap).forEach(([wcEvent, callback]) => {
            const handler = (event) => callback(event.detail, event);
            el.addEventListener(wcEvent, handler);
            eventListeners.set(wcEvent, handler);
        });
    };

    const callMethod = (methodName, ...args) => {
        return element.value?.[methodName]?.(...args);
    };

    return {
        state,
        element,
        isDefined,
        bindProps,
        bindEvents,
        callMethod,
        tagName
    };
};

function createVueComponent(webComponentClass, vueConfig = {}) {
    const tagName = vueConfig.tagName;
    const {
        props: propDefs = [],
        events: eventDefs = {},
        methods: methodDefs = [],
        modelProp = null,
        modelEvent = null
    } = vueConfig;

    return {
        name: `${tagName}VueComponent`,
        props: propDefs.reduce((acc, prop) => {
            acc[prop] = {
                type: [String, Number, Boolean, Object, Array],
                default: undefined
            };
            return acc;
        }, {}),
        emits: Object.values(eventDefs),
        setup(props, { emit, expose }) {
            let el = null;

            const createElement = () => {
                el = document.createElement(tagName);
            };

            const syncProps = () => {
                if (!el) return;
                Object.entries(props).forEach(([key, value]) => {
                    if (value !== undefined) {
                        if (typeof value === 'boolean') {
                            value ? el.setAttribute(key, '') : el.removeAttribute(key);
                        } else {
                            el.setAttribute(key, String(value));
                        }
                    }
                });
            };

            const attachListeners = () => {
                Object.entries(eventDefs).forEach(([wcEvent, vueEvent]) => {
                    el.addEventListener(wcEvent, (detail) => {
                        emit(vueEvent, detail.detail);
                    });
                });
            };

            expose({
                getElement: () => el,
                callMethod: (name, ...args) => el?.[name]?.(...args)
            });

            return {
                createElement,
                syncProps,
                attachListeners
            };
        },
        mounted() {
            this.createElement();
            this.syncProps();
            this.attachListeners();
            this.$el.appendChild(el);
        },
        watch: Object.keys(propDefs).reduce((acc, prop) => {
            acc[prop] = () => this.syncProps();
            return acc;
        }, {}),
        beforeUnmount() {
            el?.remove();
        },
        template: '<div></div>'
    };
}

function vueComponentToWebComponent(VueComponent, options = {}) {
    const {
        tagName,
        props = [],
        events = []
    } = options;

    class WrappedWebComponent extends HTMLElement {
        static get observedAttributes() {
            return props;
        }

        constructor() {
            super();
            this.attachShadow({ mode: 'open' });
            this._vueInstance = null;
        }

        connectedCallback() {
            this.render();
        }

        disconnectedCallback() {
            if (this._vueInstance) {
                this._vueInstance.$destroy();
                this._vueInstance = null;
            }
        }

        attributeChangedCallback(name, oldValue, newValue) {
            if (oldValue !== newValue && this._vueInstance) {
                this._vueInstance[name] = newValue;
            }
        }

        render() {
            const container = document.createElement('div');
            this.shadowRoot.appendChild(container);

            events.forEach(eventName => {
                this.addEventListener(eventName, (e) => {
                    this.dispatchEvent(new CustomEvent(eventName, { detail: e.detail }));
                });
            });
        }
    }

    if (tagName) {
        customElements.define(tagName, WrappedWebComponent);
    }

    return WrappedWebComponent;
}

const VueWebComponentComposable = (tagName, config = {}) => {
    const { props: propConfig = [], events: eventConfig = {} } = config;

    return () => {
        const element = Vue.ref(null);
        const isReady = Vue.ref(false);
        const listeners = new Map();

        Vue.onMounted(async () => {
            await customElements.whenDefined(tagName);
            isReady.value = true;
        });

        Vue.onUnmounted(() => {
            listeners.forEach((handler, event) => {
                element.value?.removeEventListener(event, handler);
            });
            listeners.clear();
        });

        const setAttribute = (name, value) => {
            if (element.value) {
                if (value === null || value === undefined) {
                    element.value.removeAttribute(name);
                } else {
                    element.value.setAttribute(name, String(value));
                }
            }
        };

        const addEventListener = (eventName, handler) => {
            if (element.value) {
                element.value.addEventListener(eventName, handler);
                listeners.set(eventName, handler);
            }
        };

        const removeEventListener = (eventName, handler) => {
            if (element.value) {
                element.value.removeEventListener(eventName, handler);
                listeners.delete(eventName);
            }
        };

        return {
            element,
            isReady,
            setAttribute,
            addEventListener,
            removeEventListener
        };
    };
};

export {
    VueWebComponentPlugin,
    defineWebComponentWrapper,
    useWebComponent,
    createVueComponent,
    vueComponentToWebComponent,
    VueWebComponentComposable
};
