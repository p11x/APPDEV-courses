/**
 * React Integration Guide - Web Components wrapped for React applications
 * @module interoperability/08_2_React-Integration-Guide
 * @version 1.0.0
 * @example <ReactWrapper component="universal-card" {...props} />
 */

import React, { createElement, useEffect, useState, useRef, useCallback, forwardRef, useImperativeHandle } from 'react';

function createReactWrapper(componentName, options = {}) {
    const {
        observedAttributes = [],
        events = {},
        methods = [],
        passAttributes = true,
        passChildren = true,
        defaultProps = {}
    } = options;

    const ReactWrapper = forwardRef((props, ref) => {
        const elementRef = useRef(null);
        const [mounted, setMounted] = useState(false);

        useImperativeHandle(ref, () => ({
            getElement: () => elementRef.current,
            ...methods.reduce((acc, method) => ({
                ...acc,
                [method]: (...args) => elementRef.current?.[method]?.(...args)
            }), {})
        }), [elementRef]);

        useEffect(() => {
            setMounted(true);
        }, []);

        useEffect(() => {
            const element = elementRef.current;
            if (!element) return;

            const eventHandlers = {};

            Object.entries(events).forEach(([eventName, handlerName]) => {
                eventHandlers[handlerName] = (event) => {
                    const propHandler = props[handlerName];
                    if (propHandler) {
                        propHandler(event.detail, event);
                    }
                };
                element.addEventListener(eventName, eventHandlers[handlerName]);
            });

            return () => {
                Object.entries(events).forEach(([eventName, handlerName]) => {
                    element.removeEventListener(eventName, eventHandlers[handlerName]);
                });
            };
        }, [props, mounted]);

        useEffect(() => {
            const element = elementRef.current;
            if (!element) return;

            observedAttributes.forEach(attr => {
                if (props[attr] !== undefined) {
                    const value = props[attr];
                    if (value === null || value === false) {
                        element.removeAttribute(attr);
                    } else if (typeof value === 'boolean') {
                        if (value) element.setAttribute(attr, '');
                    } else {
                        element.setAttribute(attr, String(value));
                    }
                }
            });
        }, [props, mounted]);

        if (!mounted) {
            return createElement(componentName, { ref: elementRef, ...defaultProps });
        }

        const { children, className, style, ...restProps } = props;

        const wrapperProps = {
            ...(passAttributes && restProps),
            ref: elementRef,
            className,
            style
        };

        if (passChildren && children) {
            return createElement(componentName, wrapperProps, children);
        }

        return createElement(componentName, wrapperProps);
    });

    ReactWrapper.displayName = `${componentName}Wrapper`;

    return ReactWrapper;
}

const UniversalCardWrapper = createReactWrapper('universal-card', {
    observedAttributes: ['title', 'description', 'image', 'variant', 'disabled', 'loading'],
    events: {
        'card:accept': 'onAccept',
        'card:decline': 'onDecline'
    },
    methods: ['accept', 'decline', 'focus']
});

const FrameworkNeutralModalWrapper = createReactWrapper('framework-neutral-modal', {
    observedAttributes: ['open', 'title', 'size', 'closable'],
    events: {
        'confirm': 'onConfirm',
        'cancel': 'onCancel'
    },
    methods: ['open', 'close', 'toggle']
});

function useWebComponent(ref, componentName, options = {}) {
    const {
        observedAttributes = [],
        events = {},
        methods = [],
        immediate = false
    } = options;

    const elementRef = useRef(null);
    const [state, setState] = useState({});
    const listenersRef = useRef({});

    useEffect(() => {
        if (!elementRef.current) return;

        observedAttributes.forEach(attr => {
            const value = elementRef.current.getAttribute(attr);
            if (value !== null) {
                setState(prev => ({ ...prev, [attr]: value }));
            }
        });

        Object.entries(events).forEach(([eventName, handlerName]) => {
            listenersRef.current[handlerName] = (event) => {
                setState(prev => ({ ...prev, [`_${handlerName}_fired`]: Date.now() }));
            };
            elementRef.current.addEventListener(eventName, listenersRef.current[handlerName]);
        });

        return () => {
            Object.entries(events).forEach(([eventName, handlerName]) => {
                elementRef.current?.removeEventListener(eventName, listenersRef.current[handlerName]);
            });
        };
    }, []);

    const getElement = useCallback(() => elementRef.current, []);

    methods.forEach(method => {
        useCallback((...args) => elementRef.current?.[method]?.(...args), []);
    });

    return { ref: elementRef, state, getElement };
}

function createReactComponent(webComponentName, config = {}) {
    const {
        mapPropsToAttributes = (props) => ({}),
        mapPropsToChildren = (props) => null,
        eventMap = {},
        methodNames = [],
        defaultAttributes = {}
    } = config;

    return function WebComponent(props) {
        const [elementRef, setElementRef] = useState(null);
        const eventListenersRef = useRef({});

        useEffect(() => {
            if (!elementRef) return;

            Object.entries(eventMap).forEach(([wcEvent, handlerProp]) => {
                const handler = (event) => {
                    const userHandler = props[handlerProp];
                    if (typeof userHandler === 'function') {
                        userHandler(event.detail, event);
                    }
                };
                eventListenersRef.current[`${wcEvent}_${handlerProp}`] = handler;
                elementRef.addEventListener(wcEvent, handler);
            });

            return () => {
                Object.entries(eventMap).forEach(([wcEvent, handlerProp]) => {
                    const key = `${wcEvent}_${handlerProp}`;
                    if (eventListenersRef.current[key]) {
                        elementRef.removeEventListener(wcEvent, eventListenersRef.current[key]);
                    }
                });
            };
        }, [elementRef, props]);

        useEffect(() => {
            if (!elementRef) return;

            const attributes = {
                ...defaultAttributes,
                ...mapPropsToAttributes(props)
            };

            Object.entries(attributes).forEach(([key, value]) => {
                if (value === undefined || value === null) {
                    elementRef.removeAttribute(key);
                } else if (typeof value === 'boolean') {
                    if (value) {
                        elementRef.setAttribute(key, '');
                    } else {
                        elementRef.removeAttribute(key);
                    }
                } else {
                    elementRef.setAttribute(key, value);
                }
            });
        }, [elementRef, props]);

        const attributes = mapPropsToAttributes(props);
        const children = mapPropsToChildren(props);

        const { className, style, ref, ...restProps } = props;
        const passThroughProps = { ...restProps };

        return createElement(
            webComponentName,
            {
                ref: setElementRef,
                className,
                style,
                ...attributes,
                ...passThroughProps
            },
            children
        );
    };
}

class ReactWebComponentBridge extends React.Component {
    constructor(props) {
        super(props);
        this.elementRef = createElement ? null : React.createRef();
        this.state = { mounted: false };
    }

    componentDidMount() {
        this.setState({ mounted: true });
    }

    render() {
        const { tagName, children, forwardedRef, ...props } = this.props;

        if (!this.state.mounted) {
            return createElement(tagName, { ref: forwardedRef });
        }

        return createElement(tagName, { ref: forwardedRef, ...props }, children);
    }
}

function withWebComponent(WrappedComponent, webComponentTag) {
    const WithWebComponent = forwardRef((props, ref) => {
        const elementRef = useRef(null);
        const [state, setState] = useState({});

        useImperativeHandle(ref, () => ({
            getElement: () => elementRef.current,
            callMethod: (method, ...args) => elementRef.current?.[method]?.(...args)
        }));

        return createElement(WrappedComponent, {
            ...props,
            ref: elementRef,
            webComponentRef: elementRef
        });
    });

    WithWebComponent.displayName = `withWebComponent(${WrappedComponent.displayName || WrappedComponent.name})`;

    return WithWebComponent;
}

function useCustomElement(tagName, options = {}) {
    const [element, setElement] = useState(null);
    const [isDefined, setIsDefined] = useState(false);
    const ref = useRef(null);

    useEffect(() => {
        if (customElements.get(tagName)) {
            setIsDefined(true);
        } else {
            customElements.whenDefined(tagName).then(() => {
                setIsDefined(true);
            });
        }
    }, [tagName]);

    useEffect(() => {
        if (isDefined && ref.current) {
            setElement(ref.current);
        }
    }, [isDefined]);

    return { ref, element, isDefined, tagName };
}

export {
    createReactWrapper,
    ReactWrapper,
    UniversalCardWrapper,
    FrameworkNeutralModalWrapper,
    useWebComponent,
    createReactComponent,
    ReactWebComponentBridge,
    withWebComponent,
    useCustomElement
};
