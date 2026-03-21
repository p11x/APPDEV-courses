# Module Federation Basics

## Overview
Webpack Module Federation allows sharing code between independent builds (micro-frontends). It enables applications to dynamically load code from other builds at runtime.

## Prerequisites
- Webpack knowledge
- Module systems

## Core Concepts

### Host Configuration

```javascript
// [File: webpack.config.js (host)]
const { ModuleFederationPlugin } = require('webpack').container;

module.exports = {
  plugins: [
    new ModuleFederationPlugin({
      name: 'host',
      remotes: {
        // Name: remote URL
        cart: 'cart@http://localhost:3001/remoteEntry.js',
      },
      shared: { 
        react: { singleton: true }, 
        'react-dom': { singleton: true } 
      },
    }),
  ],
};
```

### Remote Configuration

```javascript
// [File: webpack.config.js (remote)]
const { ModuleFederationPlugin } = require('webpack').container;

module.exports = {
  plugins: [
    new ModuleFederationPlugin({
      name: 'cart',
      filename: 'remoteEntry.js',
      exposes: {
        // Expose components
        './CartWidget': './src/CartWidget',
      },
      shared: { 
        react: { singleton: true }, 
        'react-dom': { singleton: true } 
      },
    }),
  ],
};
```

### Using Remote Components

```tsx
// [File: src/App.tsx]
import { mount } from 'cart/CartWidget';

function App() {
  return (
    <div>
      <h1>Host Application</h1>
      <ErrorBoundary>
        <Suspense fallback="Loading cart...">
          <CartWidget />
        </Suspense>
      </ErrorBoundary>
    </div>
  );
}
```

## Key Takeaways
- Use ModuleFederationPlugin
- Share React and ReactDOM as singletons
- Expose components with exposes config

## What's Next
Continue to [Domain Driven Design](03-domain-driven-design-in-react.md) to learn about DDD in React.